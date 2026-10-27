using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

// Values crossing the browser boundary use a single, culture-independent representation.
// An empty editor is null; IDs (including legacy negative IDs) are never null sentinels.
public sealed class AssessmentField
{
    public string Name { get; private set; }
    public string Caption { get; private set; }
    public string Kind { get; private set; }
    public int Length { get; private set; }

    public AssessmentField(string name, string caption, string kind, int length = 0)
    {
        Name = name; Caption = caption; Kind = kind; Length = length;
    }

    public string Normalize(object value)
    {
        if (value == null || value == DBNull.Value) return Kind == "bool" ? "0" : null;
        string text = Convert.ToString(value, CultureInfo.InvariantCulture).Trim();
        if (text.Length == 0) return Kind == "bool" ? "0" : null;
        if (Kind == "date")
        {
            DateTime date;
            if (value is DateTime) date = (DateTime)value;
            else if (!DateTime.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date)) throw new ArgumentException("Невірна дата: " + Caption);
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        if (Kind == "decimal")
        {
            decimal number;
            if (!Decimal.TryParse(text, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out number) || number <= -10000000000000m || number >= 10000000000000m)
                throw new ArgumentException("Невірне числове значення: " + Caption);
            return Decimal.Round(number, 2, MidpointRounding.AwayFromZero).ToString("0.00", CultureInfo.InvariantCulture);
        }
        if (Kind == "int" || Kind == "bool")
        {
            int number;
            if (!Int32.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out number) ||
                (Kind == "bool" && number != 0 && number != 1))
                throw new ArgumentException("Невірне значення: " + Caption);
            return number.ToString(CultureInfo.InvariantCulture);
        }
        if (Length > 0 && text.Length > Length)
            throw new ArgumentException(Caption + ": максимум " + Length + " символів.");
        return text;
    }
}

public sealed class AssessmentSchema
{
    public string Table { get; private set; }
    public string Caption { get; private set; }
    public AssessmentField[] Fields { get; private set; }
    public string[][] Groups { get; private set; }
    public bool IsDocument { get { return Table == "expert_input_doc" || Table == "expert_output_doc"; } }

    private AssessmentSchema(string table, string caption, AssessmentField[] fields, params string[][] groups)
    {
        Table = table; Caption = caption; Fields = fields; Groups = groups;
    }

    public Dictionary<string, string> Normalize(IDictionary<string, string> values)
    {
        var result = new Dictionary<string, string>();
        foreach (var field in Fields)
        {
            string value;
            if (!values.TryGetValue(field.Name, out value))
                throw new ArgumentException("Відсутнє поле: " + field.Caption);
            result[field.Name] = field.Normalize(value);
        }
        return result;
    }

    public static AssessmentSchema Get(string table)
    {
        var schema = All.FirstOrDefault(x => x.Table == table);
        if (schema == null) throw new ArgumentException("Невідомий розділ картки.");
        return schema;
    }

    public static readonly AssessmentSchema[] All = {
        new AssessmentSchema("expert_note", "Відомості про об'єкт / Примітка", new[] {
            new AssessmentField("addr_district_id", "Район", "int"),
            new AssessmentField("expert_obj_type_id", "Вид об'єкту", "int"),
            new AssessmentField("addr_street_id", "Вулиця", "int"),
            new AssessmentField("addr_nomer", "Номер будинку", "text", 256),
            new AssessmentField("org_balans_id", "Балансоутримувач", "int"),
            new AssessmentField("org_renter_id", "Замовник оцінки", "int"),
            new AssessmentField("expert_id", "СОД", "int"),
            new AssessmentField("obj_square", "Площа об'єкту", "decimal"),
            new AssessmentField("cost_prim", "Вартість об'єкту без ПДВ", "decimal"),
            new AssessmentField("is_archived", "Переміщено в архів", "bool"),
            new AssessmentField("arch_num", "Архівний номер", "text", 10),
            new AssessmentField("final_date", "Дата затвердження", "date"),
            new AssessmentField("note_text", "Примітка", "text", 4000)
        }, new[] { "addr_street_id", "addr_nomer" }),
        new AssessmentSchema("expert_input_doc", "Вхідна інформація", new[] {
            new AssessmentField("doc_date", "Дата документа", "date"),
            new AssessmentField("doc_num", "Номер документа", "text", 20),
            new AssessmentField("korrespondent_id", "Кореспондент", "int"),
            new AssessmentField("control_date", "Контрольна дата", "date"),
            new AssessmentField("rezenz_id", "Рецензент", "int")
        }, new[] { "doc_date", "doc_num" }),
        new AssessmentSchema("expert_output_doc", "Вихідна інформація", new[] {
            new AssessmentField("doc_date", "Дата документа", "date"),
            new AssessmentField("doc_num", "Номер документа", "text", 20),
            new AssessmentField("rezenz_type_id", "Стан/категорія рецензування", "int"),
            new AssessmentField("rezenz_date", "Дата рецензування", "date")
        }, new[] { "doc_date", "doc_num" }),
        new AssessmentSchema("expert_note_detail", "Характеристика об'єкта", new[] {
            new AssessmentField("floors", "Поверх", "text", 100),
            new AssessmentField("obj_square", "Площа", "decimal"),
            new AssessmentField("purpose", "НЕВ", "text", 100),
            new AssessmentField("cost_1_usd", "Вартість 1 кв.м. без ПДВ, $", "decimal"),
            new AssessmentField("valuation_date", "Дата оцінки", "date"),
            new AssessmentField("note", "Примітка", "text", 255)
        })
    };
}

public sealed class AssessmentSnapshot
{
    public int CardId { get; set; }
    public int? RowId { get; set; }
    public string Table { get; set; }
    public Dictionary<string, string> Values { get; set; }
}

public sealed class AssessmentOperation
{
    public string Action { get; set; }
    public AssessmentSnapshot Original { get; set; }
    public Dictionary<string, string> Values { get; set; }
}

public sealed class AssessmentConflict
{
    public string Key { get; set; }
    public string Caption { get; set; }
    public string[] Fields { get; set; }
    public string Original { get; set; }
    public string Current { get; set; }
    public string Mine { get; set; }
    public bool CanUseMine { get; set; }
}

public sealed class AssessmentMergeResult
{
    public Dictionary<string, string> Values { get; set; }
    public List<AssessmentConflict> Conflicts { get; set; }
}

public static class AssessmentMerge
{
    public static AssessmentMergeResult Merge(AssessmentSchema schema, IDictionary<string, string> original,
        IDictionary<string, string> mine, IDictionary<string, string> current)
    {
        var result = new AssessmentMergeResult {
            Values = new Dictionary<string, string>(current), Conflicts = new List<AssessmentConflict>()
        };
        var grouped = new HashSet<string>(schema.Groups.SelectMany(x => x));
        var groups = schema.Groups.Concat(schema.Fields.Where(x => !grouped.Contains(x.Name))
            .Select(x => new[] { x.Name }));
        foreach (var fields in groups)
        {
            if (Equal(fields, original, mine)) continue;
            if (Equal(fields, original, current) || Equal(fields, mine, current))
            {
                foreach (string name in fields) result.Values[name] = mine[name];
            }
            else
            {
                result.Conflicts.Add(new AssessmentConflict {
                    Key = String.Join("+", fields), Fields = fields,
                    Caption = String.Join(" + ", fields.Select(n => schema.Fields.First(f => f.Name == n).Caption)),
                    Original = Describe(fields, original), Current = Describe(fields, current),
                    Mine = Describe(fields, mine), CanUseMine = true
                });
            }
        }
        return result;
    }

    public static bool Equal(IEnumerable<string> fields, IDictionary<string, string> left, IDictionary<string, string> right)
    {
        return fields.All(n => String.Equals(left[n], right[n], StringComparison.Ordinal));
    }

    public static string Describe(IEnumerable<string> fields, IDictionary<string, string> values)
    {
        return String.Join(" / ", fields.Select(n => values[n] ?? "(порожньо)"));
    }
}
