using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

public sealed class AssessmentSaveResult
{
    public AssessmentOperation Operation { get; set; }
    public Dictionary<string, string> Current { get; set; }
    public AssessmentSnapshot Snapshot { get; set; }
    public List<AssessmentConflict> Conflicts { get; set; }
    public bool Changed { get; set; }
    public string SignatureUser { get; set; }
    public DateTime? SignatureDate { get; set; }
}

public static class AssessmentCardStore
{
    public static AssessmentSaveResult Save(SqlConnection connection, AssessmentOperation operation, string user)
    {
        var original = operation.Original;
        var schema = AssessmentSchema.Get(original.Table);
        if (schema.Table == "expert_note" && original.RowId != original.CardId)
            throw new ArgumentException("Невірна картка.");
        if (operation.Action != "insert" && operation.Action != "update" &&
            operation.Action != "delete" && operation.Action != "none") throw new ArgumentException("Невірна операція.");
        if (operation.Action == "insert" && schema.Table == "expert_note") throw new ArgumentException("Невірна операція.");
        if (operation.Action != "insert" && operation.Action != "none") schema.Normalize(original.Values);
        if (operation.Action == "insert" || operation.Action == "update") operation.Values = schema.Normalize(operation.Values);
        var result = new AssessmentSaveResult { Operation = operation, Conflicts = new List<AssessmentConflict>() };
        if (operation.Action == "none") return result;

        using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
        {
            // Every writer on this card takes this lock first. It also protects two simultaneous
            // inserts with the same business key, including when no matching document exists yet.
            bool cardExists;
            using (var command = new SqlCommand("SELECT id FROM expert_note WITH (UPDLOCK,HOLDLOCK) " +
                "WHERE id=@card AND ISNULL(is_deleted,0)=0", connection, transaction))
            {
                command.Parameters.Add("@card", SqlDbType.Int).Value = original.CardId;
                cardExists = command.ExecuteScalar() != null;
            }
            AssessmentSnapshot current = cardExists && original.RowId.HasValue
                ? Read(connection, transaction, schema, original.CardId, original.RowId.Value) : null;
            if (!cardExists || (operation.Action != "insert" && current == null))
            {
                result.Conflicts.Add(new AssessmentConflict {
                    Key = "__missing", Fields = new string[0], Caption = cardExists ? "Рядок видалено" : "Картку видалено",
                    Original = "Існував під час відкриття", Current = "Видалено іншим користувачем",
                    Mine = "Зберегти зміни", CanUseMine = false
                });
                transaction.Rollback();
                return result;
            }

            var names = schema.Fields.Select(f => f.Name).ToArray();
            Dictionary<string, string> merged;
            if (operation.Action == "delete")
            {
                result.Current = current.Values;
                if (!AssessmentMerge.Equal(names, original.Values, current.Values))
                {
                    result.Conflicts.Add(new AssessmentConflict {
                        Key = "__delete", Fields = names, Caption = "Видалення зміненого рядка",
                        Original = AssessmentMerge.Describe(names, original.Values),
                        Current = AssessmentMerge.Describe(names, current.Values), Mine = "Видалити рядок", CanUseMine = true
                    });
                    transaction.Rollback();
                    return result;
                }
                using (var command = new SqlCommand("DELETE FROM " + schema.Table +
                    " WHERE id=@id AND expert_note_id=@card", connection, transaction))
                {
                    AddIdentity(command, original.CardId, original.RowId.Value);
                    if (command.ExecuteNonQuery() != 1) throw new DBConcurrencyException("Рядок вже змінено.");
                }
                result.Changed = true;
                StampCard(connection, transaction, original.CardId, user, result);
                transaction.Commit();
                return result;
            }

            if (operation.Action == "insert") merged = operation.Values;
            else
            {
                result.Current = current.Values;
                var merge = AssessmentMerge.Merge(schema, original.Values, operation.Values, current.Values);
                merged = merge.Values;
                result.Conflicts = merge.Conflicts;
                if (result.Conflicts.Count != 0) { transaction.Rollback(); return result; }
            }

            // Null/empty keys remain permitted for legacy drafts. A complete date + document
            // number is unique within this card and this direction. Never identify a row by number alone.
            bool keyChanged = operation.Action == "insert" || (schema.IsDocument &&
                !AssessmentMerge.Equal(new[] { "doc_date", "doc_num" }, current.Values, merged));
            if (schema.IsDocument && keyChanged && merged["doc_date"] != null && merged["doc_num"] != null)
            {
                int? duplicateId = FindDocument(connection, transaction, schema, original, merged);
                if (duplicateId.HasValue)
                {
                    var duplicate = Read(connection, transaction, schema, original.CardId, duplicateId.Value);
                    if (operation.Action == "insert")
                    {
                        if (AssessmentMerge.Equal(names, merged, duplicate.Values))
                        {
                            result.Snapshot = duplicate;
                            transaction.Commit();
                            return result;
                        }
                        // The user must explicitly decide how to reconcile the already existing
                        // document. No automatic upsert may overwrite its fields.
                        operation.Action = "update";
                        operation.Original = duplicate;
                        result.Current = duplicate.Values;
                        foreach (var field in schema.Fields.Where(f => merged[f.Name] != duplicate.Values[f.Name]))
                            result.Conflicts.Add(new AssessmentConflict {
                                Key = field.Name, Fields = new[] { field.Name }, Caption = field.Caption + " (документ вже існує)",
                                Original = "Новий документ", Current = duplicate.Values[field.Name] ?? "(порожньо)",
                                Mine = merged[field.Name] ?? "(порожньо)", CanUseMine = true
                            });
                    }
                    else result.Conflicts.Add(new AssessmentConflict {
                        Key = "__duplicate", Fields = new[] { "doc_date", "doc_num" },
                        Caption = "Дата + номер: інший документ з таким ключем вже існує",
                        Original = AssessmentMerge.Describe(new[] { "doc_date", "doc_num" }, original.Values),
                        Current = AssessmentMerge.Describe(new[] { "doc_date", "doc_num" }, current.Values),
                        Mine = AssessmentMerge.Describe(new[] { "doc_date", "doc_num" }, merged), CanUseMine = false
                    });
                    transaction.Rollback();
                    return result;
                }
            }

            var changed = schema.Fields.Where(f => operation.Action == "insert" || merged[f.Name] != current.Values[f.Name]).ToArray();
            if (changed.Length == 0)
            {
                result.Snapshot = current;
                transaction.Commit();
                return result;
            }
            int rowId;
            DateTime now = DateTime.Now;
            using (var command = new SqlCommand())
            {
                command.Connection = connection; command.Transaction = transaction;
                foreach (var field in changed) AddValue(command, field, merged[field.Name]);
                command.Parameters.Add("@user", SqlDbType.VarChar, 128).Value = user;
                command.Parameters.Add("@now", SqlDbType.DateTime).Value = now;
                command.Parameters.Add("@card", SqlDbType.Int).Value = original.CardId;
                if (operation.Action == "insert")
                {
                    command.CommandText = "INSERT INTO " + schema.Table + " (expert_note_id," +
                        String.Join(",", changed.Select(f => "[" + f.Name + "]")) + ",modify_date,modified_by) OUTPUT INSERTED.id VALUES (@card," +
                        String.Join(",", changed.Select(f => "@" + f.Name)) + ",@now,@user)";
                    rowId = Convert.ToInt32(command.ExecuteScalar(), CultureInfo.InvariantCulture);
                }
                else
                {
                    rowId = original.RowId.Value;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = rowId;
                    command.CommandText = "UPDATE " + schema.Table + " SET " +
                        String.Join(",", changed.Select(f => "[" + f.Name + "]=@" + f.Name)) +
                        ",modify_date=@now,modified_by=@user WHERE id=@id" +
                        (schema.Table == "expert_note" ? "" : " AND expert_note_id=@card");
                    if (command.ExecuteNonQuery() != 1) throw new DBConcurrencyException("Рядок вже змінено.");
                }
            }
            result.Changed = true;
            if (schema.Table != "expert_note") StampCard(connection, transaction, original.CardId, user, result);
            else { result.SignatureUser = user; result.SignatureDate = now; }
            result.Snapshot = Read(connection, transaction, schema, original.CardId, rowId);
            transaction.Commit();
            return result;
        }
    }

    public static AssessmentOperation Resolve(AssessmentSaveResult pending, IDictionary<string, string> choices)
    {
        var operation = pending.Operation;
        foreach (var conflict in pending.Conflicts)
        {
            string choice;
            if (!choices.TryGetValue(conflict.Key, out choice) || (choice != "current" && choice != "mine") ||
                (choice == "mine" && !conflict.CanUseMine)) throw new ArgumentException("Оберіть значення для кожного конфлікту.");
            if (conflict.Key == "__missing" || (conflict.Key == "__delete" && choice == "current"))
            {
                operation.Action = "none";
                continue;
            }
            foreach (string field in conflict.Fields)
            {
                operation.Original.Values[field] = pending.Current[field];
                if (operation.Action != "delete" && choice == "current") operation.Values[field] = pending.Current[field];
            }
        }
        return operation;
    }

    private static AssessmentSnapshot Read(SqlConnection connection, SqlTransaction transaction, AssessmentSchema schema, int cardId, int rowId)
    {
        using (var command = new SqlCommand("SELECT " + String.Join(",", schema.Fields.Select(f => "[" + f.Name + "]")) +
            " FROM " + schema.Table + " WITH (UPDLOCK,HOLDLOCK) WHERE id=@id AND ISNULL(is_deleted,0)=0" +
            (schema.Table == "expert_note" ? "" : " AND expert_note_id=@card"), connection, transaction))
        {
            AddIdentity(command, cardId, rowId);
            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                var values = new Dictionary<string, string>();
                foreach (var field in schema.Fields) values[field.Name] = field.Normalize(reader[field.Name]);
                return new AssessmentSnapshot { CardId = cardId, RowId = rowId, Table = schema.Table, Values = values };
            }
        }
    }

    private static int? FindDocument(SqlConnection connection, SqlTransaction transaction, AssessmentSchema schema,
        AssessmentSnapshot original, IDictionary<string, string> values)
    {
        using (var command = new SqlCommand("SELECT TOP(1) id FROM " + schema.Table + " WITH (UPDLOCK,HOLDLOCK) " +
            "WHERE expert_note_id=@card AND ISNULL(is_deleted,0)=0 AND doc_date=@doc_date " +
            "AND LTRIM(RTRIM(doc_num))=@doc_num" + (original.RowId.HasValue ? " AND id<>@id" : ""), connection, transaction))
        {
            command.Parameters.Add("@card", SqlDbType.Int).Value = original.CardId;
            if (original.RowId.HasValue) command.Parameters.Add("@id", SqlDbType.Int).Value = original.RowId.Value;
            foreach (var field in schema.Fields.Where(f => f.Name == "doc_date" || f.Name == "doc_num")) AddValue(command, field, values[field.Name]);
            object id = command.ExecuteScalar();
            return id == null ? (int?)null : Convert.ToInt32(id, CultureInfo.InvariantCulture);
        }
    }

    private static void AddIdentity(SqlCommand command, int cardId, int rowId)
    {
        command.Parameters.Add("@card", SqlDbType.Int).Value = cardId;
        command.Parameters.Add("@id", SqlDbType.Int).Value = rowId;
    }

    private static void AddValue(SqlCommand command, AssessmentField field, string value)
    {
        SqlParameter parameter;
        if (field.Kind == "date") parameter = command.Parameters.Add("@" + field.Name, SqlDbType.Date);
        else if (field.Kind == "int" || field.Kind == "bool") parameter = command.Parameters.Add("@" + field.Name, SqlDbType.Int);
        else if (field.Kind == "decimal")
        {
            parameter = command.Parameters.Add("@" + field.Name, SqlDbType.Decimal);
            parameter.Precision = 15; parameter.Scale = 2;
        }
        else parameter = command.Parameters.Add("@" + field.Name, SqlDbType.VarChar, field.Length);
        if (value == null) parameter.Value = DBNull.Value;
        else if (field.Kind == "date") parameter.Value = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        else if (field.Kind == "int" || field.Kind == "bool") parameter.Value = Int32.Parse(value, CultureInfo.InvariantCulture);
        else if (field.Kind == "decimal") parameter.Value = Decimal.Parse(value, CultureInfo.InvariantCulture);
        else parameter.Value = value;
    }

    private static void StampCard(SqlConnection connection, SqlTransaction transaction, int cardId, string user, AssessmentSaveResult result)
    {
        DateTime now = DateTime.Now;
        using (var command = new SqlCommand("UPDATE expert_note SET modified_by=@user,modify_date=@now WHERE id=@card", connection, transaction))
        {
            command.Parameters.Add("@user", SqlDbType.VarChar, 128).Value = user;
            command.Parameters.Add("@now", SqlDbType.DateTime).Value = now;
            command.Parameters.Add("@card", SqlDbType.Int).Value = cardId;
            if (command.ExecuteNonQuery() != 1) throw new DBConcurrencyException("Картку вже змінено.");
        }
        result.SignatureUser = user; result.SignatureDate = now;
    }
}
