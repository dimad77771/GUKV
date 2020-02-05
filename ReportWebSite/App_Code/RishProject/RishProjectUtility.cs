using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;

public static class RishProjectUtility
{
    ///// <summary>
    ///// Find all rows within the given table that have their RowState other than Deleted and Detached
    ///// and format the results as an IEnumerable. The generic function is suitable for Typed Datasets.
    ///// </summary>
    //public static IEnumerable<T> ActiveRows<T>(this DataTable table) where T: DataRow
    //{
    //    return table.Rows.Cast<T>()
    //        .Where(x => x.RowState != DataRowState.Deleted && x.RowState != DataRowState.Detached);
    //}

    /// <summary>
    /// Find all rows within the given table that have their RowState other than Deleted and Detached
    /// and format the results as an IEnumerable.
    /// </summary>
    public static IEnumerable<DataRow> ActiveRows(this DataTable table)
    {
        return table.Rows.Cast<DataRow>()
            .Where(x => x.RowState != DataRowState.Deleted && x.RowState != DataRowState.Detached);
    }

    /// <summary>
    /// Filters the given collection of DataRow (or derivative) entries by their RowState, and
    /// emits only those rows which are not Deleted or Detached.
    /// </summary>
    public static IEnumerable<T> ActiveRows<T>(this IEnumerable<T> rows) where T: DataRow
    {
        foreach (T row in rows)
        {
            if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached)
                continue;

            yield return row;
        }
    }

    /// <summary>
    /// Retrieves the value of PageUniqueKey view state setting from the specified page. If none exists,
    /// returns a null reference.
    /// </summary>
    public static string GetPageUniqueKey(this Page page)
    {
        // Page.ViewState is marked as protected, effectively preventing querying its content directly.
        // We want its content because the underlying page has established the PageUniqueKey value, and
        // we'll use it to store our state information in the session ourselves. Don't be greedy, Page,
        // share the wealth!
        return ((StateBag)typeof(Page)
            .GetProperty("ViewState", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .GetValue(page, new object[0]))["PageUniqueKey"] as string;
    }

    /// <summary>
    /// Place the specified tree item into the session cache for use by ASCX editors. This method also
    /// resets the editor state. The edited item reference is stored in a page-specific way, ensuring
    /// that two concurrent editors won't contaminate each other's state.
    /// </summary>
    public static void SetEditedTreeItem<T>(this Page page, T item)
    {
        string pageUniqueKey = GetPageUniqueKey(page);
        if (pageUniqueKey == null)
            return;

        page.Session["EditedTreeItem_" + pageUniqueKey] = item;
        page.Session["RishProjectTableEditor_DataSource_" + pageUniqueKey] = null;
        page.Session["EditorFormInitialized_" + pageUniqueKey] = null;
    }

    /// <summary>
    /// Retrieve a tree item selected for editing from the session cache. If none exists, the function
    /// returns a null reference.
    /// </summary>
    public static T GetEditedTreeItem<T>(this Page page)
    {
        string pageUniqueKey = GetPageUniqueKey(page);
        if (pageUniqueKey == null)
            return default(T);

        object value = page.Session["EditedTreeItem_" + pageUniqueKey];
        if (value is T)
            return (T)value;
        return default(T);
    }

    public static void SetEditedCustomData<T>(this Page page, string name, T data)
    {
        string pageUniqueKey = GetPageUniqueKey(page);
        if (pageUniqueKey == null)
            return;

        page.Session[name + "_" + pageUniqueKey] = data;
    }

    public static T GetEditedCustomData<T>(this Page page, string name)
    {
        string pageUniqueKey = GetPageUniqueKey(page);
        if (pageUniqueKey == null)
            return default(T);

        object value = page.Session[name + "_" + pageUniqueKey];
        if (value is T)
            return (T)value;
        return default(T);
    }

    public static bool GetEditorFormInitialized(this Page page, string key = null)
    {
        string pageUniqueKey = GetPageUniqueKey(page);
        if (pageUniqueKey == null)
            return false;

        return Convert.ToBoolean(page.Session[(key ?? "EditorFormInitialized_") + pageUniqueKey] ?? false);
    }

    public static void SetEditorFormInitialized(this Page page, bool value, string key = null)
    {
        string pageUniqueKey = GetPageUniqueKey(page);
        if (pageUniqueKey == null)
            return;

        page.Session[(key ?? "EditorFormInitialized_") + pageUniqueKey] = value;
    }
}
