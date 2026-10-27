using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

// These tests create and drop their own database. They never update the configured application DB.
[TestFixture]
[Category("AssessmentSql")]
public class SqlConcurrencyTests
{
    private string _connectionString;
    private string _masterConnectionString;
    private string _database;
    private const int Card = -100405;
    public string TestConnectionString { get { return _connectionString; } }

    [OneTimeSetUp]
    public void CreateIsolatedDatabase()
    {
        string configured = Environment.GetEnvironmentVariable("GUKV_ASSESSMENT_TEST_CONNECTION");
        if (String.IsNullOrEmpty(configured)) Assert.Ignore("Set GUKV_ASSESSMENT_TEST_CONNECTION to run against an isolated, disposable SQL database.");
        var builder = new SqlConnectionStringBuilder(configured) { InitialCatalog = "master", ConnectTimeout = 10 };
        _masterConnectionString = builder.ConnectionString;
        _database = "GUKV_AssessmentTests_" + Guid.NewGuid().ToString("N");
        Execute(_masterConnectionString, "CREATE DATABASE [" + _database + "] COLLATE Cyrillic_General_CI_AS");
        builder.InitialCatalog = _database;
        _connectionString = builder.ConnectionString;
        Execute(_connectionString, @"
CREATE TABLE expert_note(id int NOT NULL PRIMARY KEY, is_deleted int NULL DEFAULT 0,
 addr_district_id int NULL, expert_obj_type_id int NULL, addr_street_id int NULL, addr_nomer varchar(256) NULL,
 org_balans_id int NULL, org_renter_id int NULL, expert_id int NULL, obj_square numeric(15,2) NULL,
 cost_prim numeric(15,2) NULL, is_archived int NULL DEFAULT 0, arch_num varchar(10) NULL,
 final_date date NULL, note_text varchar(4000) NULL, modify_date datetime NULL, modified_by varchar(128) NULL,
 rezenz_id int NULL, valuation_kind_id int NULL, stan_id int NULL, valuation_date date NULL);
CREATE TABLE expert_input_doc(id int IDENTITY PRIMARY KEY, expert_note_id int NULL, is_deleted int NULL DEFAULT 0,
 doc_date date NULL, doc_num varchar(20) NULL, korrespondent_id int NULL, control_date date NULL,
 rezenz_id int NULL, rezenz_name varchar(255) NULL, modify_date date NULL, modified_by varchar(128) NULL);
CREATE TABLE expert_output_doc(id int IDENTITY PRIMARY KEY, expert_note_id int NULL, is_deleted int NULL DEFAULT 0,
 doc_date date NULL, doc_num varchar(20) NULL, rezenz_type_id int NULL, rezenz_date date NULL,
 modify_date date NULL, modified_by varchar(128) NULL);
CREATE TABLE expert_note_detail(id int IDENTITY PRIMARY KEY, expert_note_id int NULL, is_deleted int NULL DEFAULT 0,
 floors varchar(100) NULL, obj_square numeric(15,2) NULL, purpose varchar(100) NULL,
 cost_1_usd numeric(15,2) NULL, valuation_date date NULL, note varchar(255) NULL,
 modify_date date NULL, modified_by varchar(128) NULL);");
    }

    [OneTimeTearDown]
    public void DropOnlyTheDatabaseCreatedByThisFixture()
    {
        if (_database == null) return;
        if (!_database.StartsWith("GUKV_AssessmentTests_", StringComparison.Ordinal) || _database.Length != 53)
            throw new InvalidOperationException("Unexpected test database name; refusing cleanup.");
        SqlConnection.ClearAllPools();
        Execute(_masterConnectionString, "ALTER DATABASE [" + _database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [" + _database + "]");
    }

    [SetUp]
    public void Seed()
    {
        Execute(_connectionString, "DELETE FROM expert_input_doc; DELETE FROM expert_output_doc; DELETE FROM expert_note_detail; DELETE FROM expert_note;" +
            "INSERT expert_note(id,obj_square,note_text,modified_by,modify_date,rezenz_id,valuation_kind_id,stan_id,valuation_date)" +
            " VALUES(-100405,250.40,'old','original','2024-08-29T10:26:00',17,18,19,'2024-08-29')");
    }

    [Test]
    public void IndependentEditsAcrossMainAndNoteTabsSurviveAndHiddenFieldsStayUntouched()
    {
        var first = Edit("expert_note", Card, "obj_square", "260.00");
        var second = Edit("expert_note", Card, "note_text", "new note");
        Assert.That(Save(first).Changed, Is.True);
        Assert.That(Save(second).Changed, Is.True);
        Assert.That(Snapshot("expert_note", Card).Values["obj_square"], Is.EqualTo("260.00"));
        Assert.That(Scalar("SELECT rezenz_id FROM expert_note WHERE id=-100405"), Is.EqualTo(17));
        Assert.That(Scalar("SELECT stan_id FROM expert_note WHERE id=-100405"), Is.EqualTo(19));
    }

    [Test]
    public void ConflictRollsBackOtherFieldsInTheSameSave()
    {
        var mine = Edit("expert_note", Card, "obj_square", "260.00", "note_text", "must not save yet");
        Save(Edit("expert_note", Card, "obj_square", "255.00"));
        var result = Save(mine);
        Assert.That(result.Conflicts.Single().Key, Is.EqualTo("obj_square"));
        Assert.That(Snapshot("expert_note", Card).Values["note_text"], Is.EqualTo("old"));
    }

    [TestCase("mine", "260.00")]
    [TestCase("current", "255.00")]
    public void ChosenValueIsSavedTogetherWithNonconflictingEdits(string choice, string expected)
    {
        var mine = Edit("expert_note", Card, "obj_square", "260.00", "note_text", "my note");
        Save(Edit("expert_note", Card, "obj_square", "255.00"));
        var pending = Save(mine);
        var saved = Save(AssessmentCardStore.Resolve(pending, new Dictionary<string, string> { { "obj_square", choice } }));
        Assert.That(saved.Conflicts, Is.Empty);
        Assert.That(saved.Snapshot.Values["obj_square"], Is.EqualTo(expected));
        Assert.That(saved.Snapshot.Values["note_text"], Is.EqualTo("my note"));
    }

    [Test]
    public void AnotherEditWhileDialogIsOpenRequiresAnotherChoice()
    {
        var mine = Edit("expert_note", Card, "obj_square", "260.00");
        Save(Edit("expert_note", Card, "obj_square", "255.00"));
        var pending = Save(mine);
        Save(Edit("expert_note", Card, "obj_square", "257.00"));
        var result = Save(AssessmentCardStore.Resolve(pending, new Dictionary<string, string> { { "obj_square", "mine" } }));
        Assert.That(result.Conflicts.Single().Current, Is.EqualTo("257.00"));
        Assert.That(Snapshot("expert_note", Card).Values["obj_square"], Is.EqualTo("257.00"));
    }

    [TestCase("expert_input_doc", "korrespondent_id", "-17", "rezenz_id", "18")]
    [TestCase("expert_output_doc", "rezenz_type_id", "3", "rezenz_date", "2026-10-26")]
    [TestCase("expert_note_detail", "floors", "1", "note", "second editor")]
    public void GridRowEditsMergeFieldByField(string table, string field1, string value1, string field2, string value2)
    {
        int row = Insert(table, "doc_date", "2026-10-01", "doc_num", "001");
        var first = Edit(table, row, field1, value1);
        var second = Edit(table, row, field2, value2);
        Save(first);
        var result = Save(second);
        Assert.That(result.Conflicts, Is.Empty);
        Assert.That(result.Snapshot.Values[field1], Is.EqualTo(value1));
        Assert.That(result.Snapshot.Values[field2], Is.EqualTo(value2));
        Assert.That(Scalar("SELECT modified_by FROM expert_note WHERE id=-100405"), Is.EqualTo("test editor"));
    }

    [TestCase("expert_input_doc")]
    [TestCase("expert_output_doc")]
    public void DuplicateInsertionRequiresExplicitFieldChoicesInsteadOfCreatingAnotherDocument(string table)
    {
        string field = table == "expert_input_doc" ? "korrespondent_id" : "rezenz_type_id";
        int row = Insert(table, "doc_date", "2026-10-01", "doc_num", "001", field, "1");
        var duplicate = New(table, "doc_date", "2026-10-01", "doc_num", " 001 ", field, "2");
        var pending = Save(duplicate);
        Assert.That(pending.Conflicts.Count, Is.EqualTo(1));
        Assert.That(pending.Operation.Original.RowId, Is.EqualTo(row));
        var saved = Save(AssessmentCardStore.Resolve(pending, pending.Conflicts.ToDictionary(c => c.Key, c => "mine")));
        Assert.That(saved.Snapshot.Values[field], Is.EqualTo("2"));
        Assert.That(Scalar("SELECT COUNT(*) FROM " + table), Is.EqualTo(1));
    }

    [TestCase("expert_input_doc")]
    [TestCase("expert_output_doc")]
    public void IdenticalDocumentInsertIsIdempotent(string table)
    {
        Insert(table, "doc_date", "2026-10-01", "doc_num", "001");
        var result = Save(New(table, "doc_date", "2026-10-01", "doc_num", "001"));
        Assert.That(result.Changed, Is.False);
        Assert.That(result.Conflicts, Is.Empty);
        Assert.That(Scalar("SELECT COUNT(*) FROM " + table), Is.EqualTo(1));
    }

    [TestCase("expert_input_doc")]
    [TestCase("expert_output_doc")]
    public void RenamingDocumentCannotReuseAnotherRowsKey(string table)
    {
        int row = Insert(table, "doc_date", "2026-10-01", "doc_num", "001");
        Insert(table, "doc_date", "2026-10-01", "doc_num", "002");
        var pending = Save(Edit(table, row, "doc_num", "002"));
        Assert.That(pending.Conflicts.Single().Key, Is.EqualTo("__duplicate"));
        var result = Save(AssessmentCardStore.Resolve(pending, new Dictionary<string, string> { { "__duplicate", "current" } }));
        Assert.That(result.Snapshot.Values["doc_num"], Is.EqualTo("001"));
    }

    [Test]
    public void DeleteDetectsChangesAndDoesNotResurrectAlreadyDeletedRows()
    {
        int row = Insert("expert_note_detail", "note", "original");
        var deletion = Edit("expert_note_detail", row); deletion.Action = "delete";
        var stale = Edit("expert_note_detail", row, "note", "stale edit");
        Save(Edit("expert_note_detail", row, "note", "someone else"));
        var conflict = Save(deletion);
        Assert.That(conflict.Conflicts.Single().Key, Is.EqualTo("__delete"));
        Assert.That(Save(AssessmentCardStore.Resolve(conflict, new Dictionary<string, string> { { "__delete", "mine" } })).Changed, Is.True);
        Assert.That(Save(stale).Conflicts.Single().Key, Is.EqualTo("__missing"));
        Assert.That(Scalar("SELECT COUNT(*) FROM expert_note_detail"), Is.EqualTo(0));
    }

    [Test]
    public void NoOpAndRepeatedSaveDoNotChangeSignature()
    {
        var result = Save(Edit("expert_note", Card));
        Assert.That(result.Changed, Is.False);
        Assert.That(Scalar("SELECT modified_by FROM expert_note WHERE id=-100405"), Is.EqualTo("original"));
        Assert.That(Scalar("SELECT modify_date FROM expert_note WHERE id=-100405"), Is.EqualTo(new DateTime(2024, 8, 29, 10, 26, 0)));
    }

    [Test]
    public void WrongParentAndDeletedCardCannotBeUpdated()
    {
        int row = Insert("expert_note_detail", "note", "original");
        var edit = Edit("expert_note_detail", row, "note", "overwrite");
        Execute(_connectionString, "INSERT expert_note(id) VALUES(-100406); UPDATE expert_note_detail SET expert_note_id=-100406");
        Assert.That(Save(edit).Conflicts.Single().Key, Is.EqualTo("__missing"));
        Execute(_connectionString, "UPDATE expert_note SET is_deleted=1 WHERE id=-100405");
        Assert.That(Save(New("expert_input_doc", "doc_date", "2026-10-01", "doc_num", "001")).Conflicts.Single().Key, Is.EqualTo("__missing"));
    }

    [Test]
    public async Task SimultaneousInsertsCannotCreateDuplicateDocuments()
    {
        var a = New("expert_input_doc", "doc_date", "2026-10-01", "doc_num", "001", "korrespondent_id", "1");
        var b = New("expert_input_doc", "doc_date", "2026-10-01", "doc_num", "001", "korrespondent_id", "2");
        using (var start = new ManualResetEventSlim())
        {
            var tasks = new[] { a, b }.Select(op => Task.Run(() => { start.Wait(); return Save(op); })).ToArray();
            start.Set();
            var results = await Task.WhenAll(tasks);
            Assert.That(results.Count(r => r.Changed), Is.EqualTo(1));
            Assert.That(results.Count(r => r.Conflicts.Count > 0), Is.EqualTo(1));
            Assert.That(Scalar("SELECT COUNT(*) FROM expert_input_doc"), Is.EqualTo(1));
        }
    }

    [Test]
    public async Task SimultaneousIndependentEditsBothSurvive()
    {
        var a = Edit("expert_note", Card, "obj_square", "260.00");
        var b = Edit("expert_note", Card, "note_text", "parallel note");
        using (var start = new ManualResetEventSlim())
        {
            var tasks = new[] { a, b }.Select(op => Task.Run(() => { start.Wait(); return Save(op); })).ToArray();
            start.Set();
            var results = await Task.WhenAll(tasks);
            Assert.That(results.All(r => r.Changed && r.Conflicts.Count == 0), Is.True);
            var row = Snapshot("expert_note", Card);
            Assert.That(row.Values["obj_square"], Is.EqualTo("260.00"));
            Assert.That(row.Values["note_text"], Is.EqualTo("parallel note"));
        }
    }

    private AssessmentSaveResult Save(AssessmentOperation operation)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            return AssessmentCardStore.Save(connection, operation, "test editor");
        }
    }

    private AssessmentSnapshot Snapshot(string table, int row)
    {
        var schema = AssessmentSchema.Get(table);
        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand("SELECT * FROM " + schema.Table + " WHERE id=@id", connection))
        {
            connection.Open(); command.Parameters.AddWithValue("@id", row);
            using (var reader = command.ExecuteReader())
            {
                Assert.That(reader.Read(), Is.True);
                return new AssessmentSnapshot { CardId = Card, RowId = row, Table = schema.Table,
                    Values = schema.Fields.ToDictionary(f => f.Name, f => f.Normalize(reader[f.Name])) };
            }
        }
    }

    private AssessmentOperation Edit(string table, int row, params string[] pairs)
    {
        var snapshot = Snapshot(table, row);
        var values = new Dictionary<string, string>(snapshot.Values);
        Apply(values, pairs);
        return new AssessmentOperation { Action = "update", Original = snapshot, Values = values };
    }

    private AssessmentOperation New(string table, params string[] pairs)
    {
        var schema = AssessmentSchema.Get(table);
        var values = schema.Fields.ToDictionary(f => f.Name, f => (string)null);
        Apply(values, pairs);
        return new AssessmentOperation { Action = "insert", Original = new AssessmentSnapshot {
            CardId = Card, Table = table, Values = new Dictionary<string, string>() }, Values = values };
    }

    private static void Apply(Dictionary<string, string> values, string[] pairs)
    {
        for (int i = 0; i < pairs.Length; i += 2) if (values.ContainsKey(pairs[i])) values[pairs[i]] = pairs[i + 1];
    }

    private int Insert(string table, params string[] pairs) { return Save(New(table, pairs)).Snapshot.RowId.Value; }

    private object Scalar(string sql)
    {
        using (var connection = new SqlConnection(_connectionString))
        using (var command = new SqlCommand(sql, connection)) { connection.Open(); return command.ExecuteScalar(); }
    }

    private static void Execute(string connectionString, string sql)
    {
        using (var connection = new SqlConnection(connectionString))
        using (var command = new SqlCommand(sql, connection)) { connection.Open(); command.ExecuteNonQuery(); }
    }
}
