using Gukv.UiTests.Infrastructure;
using Microsoft.Playwright;

namespace Gukv.UiTests.Pages;

public sealed class BuildingPickerPage(IPage page, int timeoutMilliseconds)
{
    public async Task LoadStreetAsync(int streetId)
    {
        await page.EvaluateAsync("streetId => ComboBalansStreet.SetValue(streetId)", streetId);

        await DevExpressClient.PerformCallbackAsync(
            page,
            "GridViewBalansObjects",
            string.Empty,
            timeoutMilliseconds);

        await DevExpressClient.PerformCallbackAsync(
            page,
            "ComboBalansBuilding",
            streetId.ToString(CultureInfo.InvariantCulture),
            timeoutMilliseconds);
    }

    public async Task<IReadOnlyList<BuildingPickerItem>> ReadBuildingItemsAsync()
    {
        BuildingPickerItem[]? items = await page.EvaluateAsync<BuildingPickerItem[]>(
            @"() => {
                const result = [];
                for (let index = 0; index < ComboBalansBuilding.GetItemCount(); index++) {
                    const item = ComboBalansBuilding.GetItem(index);
                    result.push({ id: Number(item.value), text: String(item.text) });
                }
                return result;
            }");

        return items ?? Array.Empty<BuildingPickerItem>();
    }

    public async Task<IReadOnlyList<int>> ReadVisibleBalansIdsAsync()
    {
        int[]? ids = await page.EvaluateAsync<int[]>(
            @"() => {
                const result = [];
                for (let index = 0; index < GridViewBalansObjects.GetVisibleRowsOnPage(); index++) {
                    result.push(Number(GridViewBalansObjects.GetRowKey(index)));
                }
                return result;
            }");

        return ids ?? Array.Empty<int>();
    }

    public async Task<IReadOnlyList<int>> ReadAllBalansIdsAsync()
    {
        int[]? ids = await page.EvaluateAsync<int[]>(
            @"args => new Promise(async (resolve, reject) => {
                const grid = GridViewBalansObjects;
                const result = [];

                const readCurrentPage = () => {
                    for (let index = 0; index < grid.GetVisibleRowsOnPage(); index++) {
                        result.push(Number(grid.GetRowKey(index)));
                    }
                };

                const goToPage = pageIndex => new Promise((pageResolve, pageReject) => {
                    let timer = setTimeout(() => {
                        grid.EndCallback.RemoveHandler(endHandler);
                        pageReject(new Error('Grid paging timeout at page ' + pageIndex));
                    }, args.timeoutMilliseconds);

                    const endHandler = function () {
                        clearTimeout(timer);
                        grid.EndCallback.RemoveHandler(endHandler);
                        pageResolve();
                    };

                    grid.EndCallback.AddHandler(endHandler);
                    grid.GotoPage(pageIndex);
                });

                try {
                    const pageCount = grid.GetPageCount();
                    for (let pageIndex = 0; pageIndex < pageCount; pageIndex++) {
                        if (grid.GetPageIndex() !== pageIndex) {
                            await goToPage(pageIndex);
                        }
                        readCurrentPage();
                    }
                    resolve(result);
                } catch (error) {
                    reject(error);
                }
            })",
            new { timeoutMilliseconds });

        return ids ?? Array.Empty<int>();
    }

    public async Task SelectBuildingGroupAsync(int representativeBuildingId)
    {
        await page.EvaluateAsync(
            "buildingId => ComboBalansBuilding.SetValue(buildingId)",
            representativeBuildingId);

        await DevExpressClient.PerformCallbackAsync(
            page,
            "GridViewBalansObjects",
            representativeBuildingId.ToString(CultureInfo.InvariantCulture),
            timeoutMilliseconds);
    }
}

public sealed class BuildingPickerItem
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
}
