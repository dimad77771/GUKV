using Microsoft.Playwright;

namespace Gukv.UiTests.Infrastructure;

public static class DevExpressClient
{
    public static async Task PerformCallbackAsync(
        IPage page,
        string controlName,
        string parameter,
        int timeoutMilliseconds)
    {
        await page.EvaluateAsync(
            @"args => new Promise((resolve, reject) => {
                const control = window[args.controlName];
                if (!control) {
                    reject(new Error('DevExpress control not found: ' + args.controlName));
                    return;
                }

                let timer = setTimeout(() => {
                    control.EndCallback.RemoveHandler(handler);
                    reject(new Error('Callback timeout: ' + args.controlName));
                }, args.timeoutMilliseconds);

                const handler = function () {
                    clearTimeout(timer);
                    control.EndCallback.RemoveHandler(handler);
                    resolve();
                };

                control.EndCallback.AddHandler(handler);
                control.PerformCallback(args.parameter);
            })",
            new { controlName, parameter, timeoutMilliseconds });
    }

    public static async Task<GridRow> FilterAndReadFirstRowAsync(
        IPage page,
        string gridName,
        string filter,
        params string[] fields)
    {
        await page.WaitForFunctionAsync(
            "name => typeof window[name] !== 'undefined'",
            gridName);

        GridRow? row = await page.EvaluateAsync<GridRow>(
            @"args => new Promise((resolve, reject) => {
                const grid = window[args.gridName];
                let timer = setTimeout(() => {
                    grid.EndCallback.RemoveHandler(endHandler);
                    reject(new Error('Grid filter timeout: ' + args.gridName));
                }, args.timeoutMilliseconds);

                const endHandler = function () {
                    clearTimeout(timer);
                    grid.EndCallback.RemoveHandler(endHandler);

                    if (grid.GetVisibleRowsOnPage() < 1) {
                        reject(new Error('No grid row matched filter: ' + args.filter));
                        return;
                    }

                    grid.GetRowValues(0, args.fields.join(';'), function (values) {
                        const array = Array.isArray(values) ? values : [values];
                        resolve({
                            key: String(grid.GetRowKey(0)),
                            values: array.map(value => value == null ? '' : String(value))
                        });
                    });
                };

                grid.EndCallback.AddHandler(endHandler);
                grid.ApplyFilter(args.filter);
            })",
            new
            {
                gridName,
                filter,
                fields,
                timeoutMilliseconds = 60_000
            });

        return row ?? throw new InvalidOperationException($"Grid {gridName} returned no data.");
    }
}

public sealed class GridRow
{
    public string Key { get; init; } = string.Empty;
    public string[] Values { get; init; } = Array.Empty<string>();
}
