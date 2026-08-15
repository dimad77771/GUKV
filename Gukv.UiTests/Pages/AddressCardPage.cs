using Gukv.UiTests.Infrastructure;
using Microsoft.Playwright;

namespace Gukv.UiTests.Pages;

public sealed class AddressCardPage(IPage page, Uri baseUri, int timeoutMilliseconds)
{
    public async Task OpenAsync(int reportId, int balansId)
    {
        string relativeUrl = $"Reports1NF/OrgBalansObject.aspx?rid={reportId}&bid={balansId}";
        await page.GotoAsync(
            new Uri(baseUri, relativeUrl).AbsoluteUri,
            new() { WaitUntil = WaitUntilState.DOMContentLoaded });

        await InputBySuffix("EditBuildingNum1_I").WaitForAsync(new() { State = WaitForSelectorState.Visible });
    }

    public async Task<VisualAddress> ReadAddressAsync()
    {
        string district = await InputBySuffix("ComboAddrDistrict_I").InputValueAsync();
        string street = await InputBySuffix("ComboAddrStreet_I").InputValueAsync();
        string number1 = await InputBySuffix("EditBuildingNum1_I").InputValueAsync();
        string number2 = await InputBySuffix("EditBuildingNum2_I").InputValueAsync();
        string number3 = await InputBySuffix("EditBuildingNum3_I").InputValueAsync();

        return new VisualAddress(
            district,
            street,
            VisualAddress.JoinNumber(number1, number2, number3));
    }

    public async Task OpenPickerAsync()
    {
        await page.EvaluateAsync("() => ButtonSelectBalansObj.DoClick()");
        await page.WaitForFunctionAsync("() => PopupSelectBalansObject.IsVisible()");
    }

    public async Task SelectBuildingAsync(int buildingId)
    {
        await DevExpressClient.PerformCallbackAsync(
            page,
            "CPObjSel",
            "sel_obj:" + buildingId,
            timeoutMilliseconds);
    }

    public Task SaveAsync()
    {
        return DevExpressClient.PerformCallbackAsync(
            page,
            "CPMainPanel",
            "save:",
            timeoutMilliseconds);
    }

    public Task SendAsync()
    {
        return DevExpressClient.PerformCallbackAsync(
            page,
            "CPMainPanel",
            "send:",
            timeoutMilliseconds);
    }

    private ILocator InputBySuffix(string suffix)
    {
        return page.Locator($"input[id$='{suffix}']").First;
    }
}
