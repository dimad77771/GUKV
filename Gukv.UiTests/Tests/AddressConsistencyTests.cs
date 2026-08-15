using Gukv.UiTests.Infrastructure;
using Gukv.UiTests.Pages;

namespace Gukv.UiTests.Tests;

[TestFixture]
[Category("ReadOnly")]
public sealed class AddressConsistencyTests : GukvPageTest
{
    [Test]
    public async Task SubmittedAddressMatchesCardHolderListAndCentre()
    {
        (int reportId, int balansId) = Settings.RequireObject();
        await LoginAsync();

        AddressCardPage cardPage = new(Page, Settings.BaseUri, Settings.TimeoutMilliseconds);
        await cardPage.OpenAsync(reportId, balansId);
        VisualAddress cardAddress = (await cardPage.ReadAddressAsync()).Normalize();

        HolderBalansState holderState = await new HolderBalansListPage(Page, Settings.BaseUri)
            .ReadStateAsync(reportId, balansId);
        VisualAddress holderAddress = holderState.Address.Normalize();

        VisualAddress centralAddress = (await new CentralBalansListPage(Page, Settings.BaseUri)
            .ReadAddressAsync(balansId)).Normalize();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(holderState.SubmittedDisplay.Trim(), Is.EqualTo("ТАК"),
                "The configured object is not currently in the submitted state.");
            Assert.That(holderAddress, Is.EqualTo(cardAddress),
                $"Holder list differs from card. Card: {cardAddress}; list: {holderAddress}");
            Assert.That(centralAddress, Is.EqualTo(cardAddress),
                $"Centre differs from card. Card: {cardAddress}; centre: {centralAddress}");
        }
    }
}
