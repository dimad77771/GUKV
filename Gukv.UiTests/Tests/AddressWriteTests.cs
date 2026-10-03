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
        SqlProbe sql = await GetVerifiedWriteSqlAsync(scenario);

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

        BalansLinkState? guardBefore = scenario.GuardBalansId.HasValue
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

            if (guardBefore is not null && scenario.GuardBalansId.HasValue)
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

            if (guardBefore is not null && scenario.GuardBalansId.HasValue)
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

    [Test]
    public async Task FirstSendCreatesReportOnlyObjectInCentre_RepeatSendKeepsSameIdAndAddress()
    {
        int? configuredId = Settings.ReportOnlyBalansId;
        if (!configuredId.HasValue)
        {
            Assert.Ignore("Set GUKV_E2E_REPORT_ONLY_BALANS_ID to a report object absent from the centre.");
        }

        int balansId = configuredId ?? throw new InvalidOperationException("Report-only balance ID was not configured.");
        WriteScenario scenario = Settings.RequireWriteScenario();
        SqlProbe sql = await GetVerifiedWriteSqlAsync(scenario);
        Assert.That(await sql.GetCentralBalansCountAsync(balansId), Is.Zero,
            "The first-send fixture must exist only in the report; choose a fresh disposable fixture for each run.");

        BalansLinkState before = await sql.GetBalansLinkStateAsync(scenario.ReportId, balansId);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(before.ReportBuildingId, Is.EqualTo(scenario.TargetBuildingId));
            Assert.That(before.SnapshotBuildingId, Is.EqualTo(scenario.TargetBuildingId),
                "The report-only object must already reference the exact selected building.");
            Assert.That(before.CentralBuildingId, Is.Null);
            Assert.That(scenario.GuardBalansId, Is.Not.EqualTo(balansId),
                "The optional guard must be a different report object.");
        }

        BalansLinkState? guardBefore = scenario.GuardBalansId.HasValue
            ? await sql.GetBalansLinkStateAsync(scenario.ReportId, scenario.GuardBalansId.Value)
            : null;
        VisualAddress selectedAddressBefore = (await sql.GetBuildingAddressAsync(scenario.TargetBuildingId)).Normalize();

        await LoginAsync();
        AddressCardPage card = new(Page, Settings.BaseUri, Settings.TimeoutMilliseconds);
        await card.OpenAsync(scenario.ReportId, balansId);
        Assert.That((await card.ReadAddressAsync()).Normalize(), Is.EqualTo(selectedAddressBefore));

        // The fixture already has its selected address. Send must create the centre row
        // without changing that selection; the inserted row deliberately remains in the test DB.
        await card.SendAsync();
        BalansLinkState afterFirstSend = await sql.GetBalansLinkStateAsync(scenario.ReportId, balansId);
        await AssertReportOnlySubmissionAsync(sql, scenario, balansId, before, afterFirstSend, selectedAddressBefore);
        if (before.SubmitDate.HasValue)
        {
            Assert.That(afterFirstSend.SubmitDate, Is.GreaterThan(before.SubmitDate.Value),
                "First Send must advance the report object's submission date.");
        }
        Assert.That((await card.ReadAddressAsync()).Normalize(), Is.EqualTo(selectedAddressBefore));
        await AssertGuardUnchangedAsync(sql, scenario, guardBefore);

        await card.SendAsync();
        BalansLinkState afterSecondSend = await sql.GetBalansLinkStateAsync(scenario.ReportId, balansId);
        await AssertReportOnlySubmissionAsync(sql, scenario, balansId, before, afterSecondSend, selectedAddressBefore);
        Assert.That(afterSecondSend.SubmitDate, Is.GreaterThanOrEqualTo(afterFirstSend.SubmitDate!.Value));
        Assert.That((await card.ReadAddressAsync()).Normalize(), Is.EqualTo(selectedAddressBefore));
        await AssertGuardUnchangedAsync(sql, scenario, guardBefore);

        VisualAddress centralAddress = (await new CentralBalansListPage(Page, Settings.BaseUri)
            .ReadAddressAsync(balansId)).Normalize();
        Assert.That(centralAddress, Is.EqualTo(selectedAddressBefore));
    }

    private async Task<SqlProbe> GetVerifiedWriteSqlAsync(WriteScenario scenario)
    {
        Settings.RequireCredentials();
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
        return sql;
    }

    private static async Task AssertReportOnlySubmissionAsync(
        SqlProbe sql,
        WriteScenario scenario,
        int balansId,
        BalansLinkState before,
        BalansLinkState after,
        VisualAddress selectedAddressBefore)
    {
        int centralCount = await sql.GetCentralBalansCountAsync(balansId);
        VisualAddress selectedAddressAfter = (await sql.GetBuildingAddressAsync(scenario.TargetBuildingId)).Normalize();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(centralCount, Is.EqualTo(1), "Send must retain exactly one central object with the original ID.");
            Assert.That(after.CentralBuildingId, Is.EqualTo(scenario.TargetBuildingId));
            Assert.That(after.ReportBuildingId, Is.EqualTo(scenario.TargetBuildingId));
            Assert.That(after.SnapshotBuildingId, Is.EqualTo(scenario.TargetBuildingId));
            Assert.That(after.ReportBuildingUniqueId, Is.EqualTo(before.ReportBuildingUniqueId));
            Assert.That(after.SubmitDate, Is.Not.Null);
            Assert.That(selectedAddressAfter, Is.EqualTo(selectedAddressBefore),
                "Sending a report-only object must not change the selected shared building's address.");
        }
    }

    private static async Task AssertGuardUnchangedAsync(SqlProbe sql, WriteScenario scenario, BalansLinkState? guardBefore)
    {
        if (guardBefore is not null && scenario.GuardBalansId.HasValue)
        {
            BalansLinkState guardAfter = await sql.GetBalansLinkStateAsync(scenario.ReportId, scenario.GuardBalansId.Value);
            Assert.That(guardAfter, Is.EqualTo(guardBefore), "Sending the test object changed the optional guard object.");
        }
    }
}
