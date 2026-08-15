using Gukv.UiTests.Configuration;
using Gukv.UiTests.Infrastructure;
using Gukv.UiTests.Pages;

namespace Gukv.UiTests.Tests;

[TestFixture]
[Category("ReadOnly")]
public sealed class AddressPickerTests : GukvPageTest
{
    [Test]
    public async Task EquivalentGroupIsShownOnceAndContainsExpectedObjects()
    {
        (int reportId, int balansId) = Settings.RequireObject();
        GroupScenario scenario = Settings.RequireGroupScenario();
        await LoginAsync();

        AddressCardPage cardPage = new(Page, Settings.BaseUri, Settings.TimeoutMilliseconds);
        await cardPage.OpenAsync(reportId, balansId);
        await cardPage.OpenPickerAsync();

        BuildingPickerPage picker = new(Page, Settings.TimeoutMilliseconds);
        await picker.LoadStreetAsync(scenario.StreetId);

        IReadOnlyList<int> rowsBeforeNumberSelection = await picker.ReadVisibleBalansIdsAsync();
        Assert.That(rowsBeforeNumberSelection, Is.Empty,
            "Objects from the previous address remained visible after the street changed.");

        IReadOnlyList<BuildingPickerItem> items = await picker.ReadBuildingItemsAsync();
        BuildingPickerItem[] representatives = items
            .Where(item => item.Id == scenario.RepresentativeBuildingId)
            .ToArray();

        Assert.That(representatives, Has.Length.EqualTo(1),
            "The representative must occur exactly once in the grouped number list.");
        Assert.That(representatives[0].Text, Is.EqualTo(scenario.DisplayNumber));

        await picker.SelectBuildingGroupAsync(scenario.RepresentativeBuildingId);
        IReadOnlyList<int> visibleBalansIds = await picker.ReadAllBalansIdsAsync();

        Assert.That(visibleBalansIds, Is.Unique, "The existing-objects grid contains duplicate balans_id values.");
        Assert.That(visibleBalansIds, Is.EquivalentTo(scenario.ExpectedBalansIds),
            "The picker must show exactly the union of objects from all equivalent building records.");
    }
}
