using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class MergeTests
{
    private readonly AssessmentSchema _schema = AssessmentSchema.Get("expert_note");

    private Dictionary<string, string> Values(params string[] pairs)
    {
        var values = _schema.Fields.ToDictionary(f => f.Name, f => f.Kind == "bool" ? "0" : (string)null);
        for (int i = 0; i < pairs.Length; i += 2) values[pairs[i]] = pairs[i + 1];
        return values;
    }

    [Test]
    public void DifferentFieldsAreMergedAcrossObjectAndNoteTabs()
    {
        var original = Values("obj_square", "250.40", "note_text", "old");
        var mine = Values("obj_square", "260.00", "note_text", "old");
        var current = Values("obj_square", "250.40", "note_text", "someone else's note");
        var result = AssessmentMerge.Merge(_schema, original, mine, current);
        Assert.That(result.Conflicts, Is.Empty);
        Assert.That(result.Values["obj_square"], Is.EqualTo("260.00"));
        Assert.That(result.Values["note_text"], Is.EqualTo("someone else's note"));
    }

    [Test]
    public void SameFieldHasThreeValuesAndRequiresChoice()
    {
        var result = AssessmentMerge.Merge(_schema, Values("obj_square", "250.40"),
            Values("obj_square", "260.00"), Values("obj_square", "255.00"));
        Assert.That(result.Conflicts.Count, Is.EqualTo(1));
        Assert.That(result.Conflicts[0].Original, Is.EqualTo("250.40"));
        Assert.That(result.Conflicts[0].Current, Is.EqualTo("255.00"));
        Assert.That(result.Conflicts[0].Mine, Is.EqualTo("260.00"));
    }

    [Test]
    public void SameNewValueIsIdempotent()
    {
        var result = AssessmentMerge.Merge(_schema, Values("note_text", "old"), Values("note_text", "new"), Values("note_text", "new"));
        Assert.That(result.Conflicts, Is.Empty);
    }

    [Test]
    public void ExplicitClearingIsAnEditAndUnchangedNullIsNot()
    {
        var result = AssessmentMerge.Merge(_schema, Values("note_text", "old"), Values(), Values("note_text", "old", "arch_num", "other"));
        Assert.That(result.Conflicts, Is.Empty);
        Assert.That(result.Values["note_text"], Is.Null);
        Assert.That(result.Values["arch_num"], Is.EqualTo("other"));
    }

    [Test]
    public void StreetAndHouseCannotBecomeAHybridAddress()
    {
        var result = AssessmentMerge.Merge(_schema, Values("addr_street_id", "1", "addr_nomer", "3"),
            Values("addr_street_id", "2", "addr_nomer", "3"), Values("addr_street_id", "1", "addr_nomer", "5"));
        Assert.That(result.Conflicts.Single().Fields, Is.EqualTo(new[] { "addr_street_id", "addr_nomer" }));
    }

    [TestCase("expert_input_doc")]
    [TestCase("expert_output_doc")]
    public void DateAndNumberAreOneConflictGroup(string table)
    {
        var schema = AssessmentSchema.Get(table);
        var original = schema.Fields.ToDictionary(f => f.Name, f => (string)null);
        original["doc_date"] = "2026-10-01"; original["doc_num"] = "001";
        var mine = new Dictionary<string, string>(original); mine["doc_date"] = "2026-10-02";
        var current = new Dictionary<string, string>(original); current["doc_num"] = "002";
        var result = AssessmentMerge.Merge(schema, original, mine, current);
        Assert.That(result.Conflicts.Single().Fields, Is.EqualTo(new[] { "doc_date", "doc_num" }));
    }

    [TestCase(-1)]
    [TestCase(-100405)]
    [TestCase(0)]
    public void IdSignIsNotAValidityCheck(int id)
    {
        var field = _schema.Fields.Single(f => f.Name == "org_balans_id");
        Assert.That(field.Normalize(id), Is.EqualTo(id.ToString(System.Globalization.CultureInfo.InvariantCulture)));
    }

    [Test]
    public void EmptyAndFormattedValuesNormalizeWithoutSpuriousEdits()
    {
        Assert.That(new AssessmentField("n", "n", "decimal").Normalize(250.4m), Is.EqualTo("250.40"));
        Assert.That(new AssessmentField("d", "d", "date").Normalize(new DateTime(2026, 10, 26, 12, 30, 0)), Is.EqualTo("2026-10-26"));
        Assert.That(new AssessmentField("s", "s", "text", 20).Normalize("  001/24  "), Is.EqualTo("001/24"));
        Assert.That(new AssessmentField("b", "b", "bool").Normalize(DBNull.Value), Is.EqualTo("0"));
    }

    [Test]
    public void InvalidValuesAreRejectedBeforeSql()
    {
        Assert.Throws<ArgumentException>(() => new AssessmentField("d", "d", "date").Normalize("26/10/2026"));
        Assert.Throws<ArgumentException>(() => new AssessmentField("n", "n", "decimal").Normalize("NaN"));
        Assert.Throws<ArgumentException>(() => new AssessmentField("s", "s", "text", 2).Normalize("abc"));
        Assert.Throws<ArgumentException>(() => _schema.Normalize(new Dictionary<string, string>()));
    }

    [TestCase("mine", "my note")]
    [TestCase("current", "other note")]
    public void ResolutionRebasesOnlyFieldsActuallyChosen(string choice, string expected)
    {
        var original = Values("note_text", "old");
        var mine = Values("note_text", "my note", "obj_square", "260.00");
        var current = Values("note_text", "other note");
        var merge = AssessmentMerge.Merge(_schema, original, mine, current);
        var pending = new AssessmentSaveResult {
            Operation = new AssessmentOperation { Action = "update", Original = new AssessmentSnapshot { Values = original }, Values = mine },
            Current = current, Conflicts = merge.Conflicts
        };
        var operation = AssessmentCardStore.Resolve(pending, new Dictionary<string, string> { { "note_text", choice } });
        Assert.That(operation.Original.Values["note_text"], Is.EqualTo("other note"));
        Assert.That(operation.Values["note_text"], Is.EqualTo(expected));
        Assert.That(operation.Values["obj_square"], Is.EqualTo("260.00"));
        Assert.That(operation.Original.Values["obj_square"], Is.Null);
        if (choice == "mine")
        {
            current["note_text"] = "changed again";
            Assert.That(AssessmentMerge.Merge(_schema, operation.Original.Values, operation.Values, current).Conflicts.Count, Is.EqualTo(1));
        }
    }

    [Test]
    public void MissingOrDisallowedChoicesCannotForceDeletedRecordsBack()
    {
        var pending = new AssessmentSaveResult {
            Operation = new AssessmentOperation { Action = "update" },
            Conflicts = new List<AssessmentConflict> { new AssessmentConflict { Key = "__missing", CanUseMine = false } }
        };
        Assert.Throws<ArgumentException>(() => AssessmentCardStore.Resolve(pending, new Dictionary<string, string>()));
        Assert.Throws<ArgumentException>(() => AssessmentCardStore.Resolve(pending, new Dictionary<string, string> { { "__missing", "mine" } }));
        Assert.That(AssessmentCardStore.Resolve(pending, new Dictionary<string, string> { { "__missing", "current" } }).Action, Is.EqualTo("none"));
    }
}
