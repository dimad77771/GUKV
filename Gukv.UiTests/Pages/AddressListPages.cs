using Gukv.UiTests.Infrastructure;
using Microsoft.Playwright;

namespace Gukv.UiTests.Pages;

public sealed class HolderBalansListPage(IPage page, Uri baseUri)
{
    public async Task<HolderBalansState> ReadStateAsync(int reportId, int balansId)
    {
        await page.GotoAsync(
            new Uri(baseUri, $"Reports1NF/OrgBalansList.aspx?rid={reportId}").AbsoluteUri,
            new() { WaitUntil = WaitUntilState.DOMContentLoaded });

        GridRow row = await DevExpressClient.FilterAndReadFirstRowAsync(
            page,
            "PrimaryGridView",
            $"[balans_id] = {balansId}",
            "district",
            "addr_street_name",
            "addr_nomer",
            "is_submitted");

        Assert.That(row.Key, Is.EqualTo(balansId.ToString(CultureInfo.InvariantCulture)));
        Assert.That(row.Values, Has.Length.EqualTo(4));
        return new HolderBalansState(
            new VisualAddress(row.Values[0], row.Values[1], row.Values[2]),
            row.Values[3]);
    }
}

public sealed record HolderBalansState(VisualAddress Address, string SubmittedDisplay);

public sealed class CentralBalansListPage(IPage page, Uri baseUri)
{
    public async Task<VisualAddress> ReadAddressAsync(int balansId)
    {
        await page.GotoAsync(
            new Uri(baseUri, "Balans/BalansObjects.aspx").AbsoluteUri,
            new() { WaitUntil = WaitUntilState.DOMContentLoaded });

        GridRow row = await DevExpressClient.FilterAndReadFirstRowAsync(
            page,
            "GridViewBalansObjects",
            $"[balans_id] = {balansId}",
            "district",
            "street_full_name",
            "addr_nomer_new");

        Assert.That(row.Key, Is.EqualTo(balansId.ToString(CultureInfo.InvariantCulture)));
        Assert.That(row.Values, Has.Length.EqualTo(3));
        return new VisualAddress(row.Values[0], row.Values[1], row.Values[2]);
    }
}
