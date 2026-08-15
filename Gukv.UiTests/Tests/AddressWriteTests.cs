using Gukv.UiTests.Configuration;
using Gukv.UiTests.Infrastructure;
using Gukv.UiTests.Pages;

namespace Gukv.UiTests.Tests;

[TestFixture]
[Category("Write")]
public sealed class AddressWriteTests : GukvPageTest
{
    [Test]
    public async Task SaveChangesOnlyReport_SendChangesCentre_AndOtherObjectStaysUntouched()
    {
        WriteScenario scenario = Settings.RequireWriteScenario();
        Assert.That(Settings.BaseUri.IsLoopback, Is.True,
            "Write tests are allowed only against a site opened through a loopback URL.");

        SqlProbe sql = new(scenario.SqlConnectionString);
        SqlProbe applicationSql = new(WebConfigProbe.ReadGukvConnectionString(Settings.WebConfigPath));

        SqlTarget probeTarget = await sql.GetTargetAsync();
        SqlTarget applicationTarget = await applicationSql.GetTargetAsync();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(probeTarget.DatabaseName, Is.EqualTo(scenario.ExpectedDatabase),
                "The control SQL connection uses a database other than the explicitly confirmed database.");
            Assert.That(applicationTarget.DatabaseName, Is.EqualTo(scenario.ExpectedDatabase),
                "The local site's Web.config points to a database other than the explicitly confirmed database.");
            Assert.That(applicationTarget.ServerName, Is.EqualTo(probeTarget.ServerName).IgnoreCase,
                "The local site and the control SQL connection point to different SQL Server instances.");
        }

        BalansLinkState before = await sql.GetBalansLinkStateAsync(scenario.ReportId, scenario.BalansId);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(before.ReportBuildingId, Is.EqualTo(before.CentralBuildingId),
                "The test object must start with matching report and central building IDs.");
            Assert.That(before.SnapshotBuildingId, Is.EqualTo(before.CentralBuildingId),
                "The test object must start with a synchronized report snapshot.");
            Assert.That(before.CentralBuildingId, Is.Not.EqualTo(scenario.TargetBuildingId),
                "Choose a target building different from the current address.");
        }

        BalansLinkState? guardBefore = scenario.GuardBalansId is > 0
            ? await sql.GetBalansLinkStateAsync(scenario.ReportId, scenario.GuardBalansId.Value)
            : null;

        await LoginAsync();
        AddressCardPage card = new(Page, Settings.BaseUri, Settings.TimeoutMilliseconds);
        bool addressWasChanged = false;

        try
        {
            await card.OpenAsync(scenario.ReportId, scenario.BalansId);
            await card.SelectBuildingAsync(scenario.TargetBuildingId);
            VisualAddress expectedAddress = (await sql.GetBuildingAddressAsync(scenario.TargetBuildingId)).Normalize();
            Assert.That((await card.ReadAddressAsync()).Normalize(), Is.EqualTo(expectedAddress));

            await card.SaveAsync();
            addressWasChanged = true;

            BalansLinkState afterSave = await sql.GetBalansLinkStateAsync(scenario.ReportId, scenario.BalansId);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(afterSave.ReportBuildingId, Is.EqualTo(scenario.TargetBuildingId));
                Assert.That(afterSave.SnapshotBuildingId, Is.EqualTo(scenario.TargetBuildingId));
                Assert.That(afterSave.CentralBuildingId, Is.EqualTo(before.CentralBuildingId),
                    "Save must not transfer the address to the centre.");
            }

            if (guardBefore is not null && scenario.GuardBalansId is > 0)
            {
                BalansLinkState guardAfterSave = await sql.GetBalansLinkStateAsync(
                    scenario.ReportId,
                    scenario.GuardBalansId.Value);
                Assert.That(guardAfterSave, Is.EqualTo(guardBefore),
                    "Saving the test object changed another report object.");
            }

            await card.SendAsync();
            BalansLinkState afterSend = await sql.GetBalansLinkStateAsync(scenario.ReportId, scenario.BalansId);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(afterSend.CentralBuildingId, Is.EqualTo(scenario.TargetBuildingId));
                Assert.That(afterSend.ReportBuildingId, Is.EqualTo(scenario.TargetBuildingId));
                Assert.That(afterSend.SnapshotBuildingId, Is.EqualTo(scenario.TargetBuildingId));
                Assert.That(afterSend.SubmitDate, Is.Not.Null);
            }

            VisualAddress centralAddress = (await new CentralBalansListPage(Page, Settings.BaseUri)
                .ReadAddressAsync(scenario.BalansId)).Normalize();
            Assert.That(centralAddress, Is.EqualTo(expectedAddress));

            if (guardBefore is not null && scenario.GuardBalansId is > 0)
            {
                BalansLinkState guardAfterSend = await sql.GetBalansLinkStateAsync(
                    scenario.ReportId,
                    scenario.GuardBalansId.Value);
                Assert.That(guardAfterSend, Is.EqualTo(guardBefore),
                    "Sending the test object changed another object.");
            }
        }
        finally
        {
            if (addressWasChanged && scenario.RestoreAfterWrite && before.CentralBuildingId is > 0)
            {
                await card.OpenAsync(scenario.ReportId, scenario.BalansId);
                await card.SelectBuildingAsync(before.CentralBuildingId.Value);
                await card.SaveAsync();
                await card.SendAsync();
            }
        }
    }
}
