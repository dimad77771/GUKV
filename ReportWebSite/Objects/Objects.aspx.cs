using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using DevExpress.Web;

public partial class ObjRights_ObjRightTransfer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        // Enable the role-dependent functionality
        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        //ButtonShowFoldersPopup1.Visible = userIsReportManager;
        ButtonShowFoldersPopup2.Visible = userIsReportManager;

        SectionMenu.ClientVisible = Roles.IsUserInRole(Utils.RishProjectCreatorRole);

        // Check if this is first loading of this page
        object uniqueKey = ViewState["PageUniqueKey"];
        bool isFirstLoading = (uniqueKey == null);

        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // If user-defined report is displayed, switch to the proper page of the Page control
        string reportTitle = "";
        string preFilter = "";

        if (isFirstLoading)
        {
            var fixedColumns = string.Empty;

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDObjects_Transfer)
            {
                LabelReportTitle2.Text = reportTitle;
                Utils.RestoreFixedColumns(PrimaryGridView, fixedColumns);

                ParsePreFilterString(preFilter);
                UpdatePreFilterControls();
            }
            else
            {
                // Restore grid fixed columns
                Utils.RestoreFixedColumns(PrimaryGridView);
            } 
        }

        if (!IsCallback && !IsPostBack)
        {
            ViewState["PrimaryGridView.DataSourceID"] = PrimaryGridView.DataSourceID;
            PrimaryGridView.DataSourceID = null;
        }
        else if (IsCallback)
        {
            Control callbackTarget = FindControl(Request.Params["__CALLBACKID"]);

            if (string.IsNullOrEmpty(PrimaryGridView.DataSourceID))
            {
                PrimaryGridView.DataSourceID = ViewState["PrimaryGridView.DataSourceID"] as string;

                if (true /*callbackTarget == GridViewTransfer*/)
                {
                    // Set the data source for 'object transfer' grid
                    SetTransferGridDataSource(preFilter);

                    if (PrimaryGridView.DataSourceID == ViewState["PrimaryGridView.DataSourceID"] as string)
                    {
                        // It appears if SetTransferGridDataSource() changes the DataSourceID,
                        // ASPxGridView binds to the new datasource automatically. That's why
                        // we force DataBind() only when the DataSourceID remains unchanged after
                        // the SetTransferGridDataSource() call.
                        PrimaryGridView.DataBind();
                    }
                }
            }
        }

        // Enable advanced header filter for all grid columns
        Utils.AdjustColumnsVisibleInFilter(PrimaryGridView);

        // Set the checked state of pre-filter button, if a user-defined report was loaded
        if (PreFilterRightId > 0 && PreFilterOrgOwnershipIdList.Count > 0)
        {
            ButtonPreFilter.Checked = true;
        }
    }

    #region Object transfer

    protected void ASPxButton_Transfer_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterTransfer, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Transfer_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterTransfer, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Transfer_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterTransfer, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewTransfer_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;
        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 55);

        if (param == "applypre:")
        {
            // Clear the old pre-filter parameters
            PreFilterRightId = -1;
            PreFilterOrgOwnershipIdList.Clear();
            PreFilterMode = "";

            string preFilter = GetPreFilterString();

            ParsePreFilterString(preFilter);
            SetTransferGridDataSource(preFilter);
            PrimaryGridView.DataBind();
        }
        else if (param == "clearpre:")
        {
            // Clear the pre-filter parameters
            PreFilterRightId = -1;
            PreFilterOrgOwnershipIdList.Clear();
            PreFilterMode = "";

            SetTransferGridDataSource("");
            PrimaryGridView.DataBind();
        }
        else
        {
            Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView,
                Utils.GridIDObjects_Transfer, GetPreFilterString());
        }
    }

    protected void GridViewTransfer_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
    {
        Utils.CheckCustomSummaryVisibility(PrimaryGridView, e);
        Utils.CustomSummaryUniqueObjectTransfer(e);
    }

    protected void GridViewTransfer_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewTransfer_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void GridViewTransfer_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
    }

    #endregion (Object transfer)

    #region Pre-filter builder for 'object transfer' grid

    protected void SetTransferGridDataSource(string preFilter)
    {
        bool inclusiveSource = false;
        bool exclusiveSource = false;

        if (preFilter.Length > 0)
        {
            if (preFilter.StartsWith("+"))
            {
                inclusiveSource = true;
            }
            else if (preFilter.StartsWith("-"))
            {
                exclusiveSource = true;
            }
        }
        else
        {
            // Select data source dynamically by the values of pre-filter drop-downs
            if (PreFilterRightId >= 0 && PreFilterOrgOwnershipIdList.Count > 0)
            {
                if (PreFilterMode.StartsWith("+"))
                {
                    inclusiveSource = true;
                }
                else
                {
                    exclusiveSource = true;
                }
            }
        }

        // Select data source dynamically for the 'rights transfer' grid
        if (inclusiveSource)
        {
            PrimaryGridView.DataSourceID = "SqlDataSourceObjectTransfer_InclusivePreFilter";
        }
        else if (exclusiveSource)
        {
            PrimaryGridView.DataSourceID = "SqlDataSourceObjectTransfer_ExclusivePreFilter";
        }
        else
        {
            PrimaryGridView.DataSourceID = "SqlDataSourceObjectTransfer";
        }

        ButtonPreFilter.Checked = inclusiveSource || exclusiveSource;
    }

    protected string GetPreFilterString()
    {
        object dataTransferRight = ComboPreFilterRight.Value;

        // Format the list of ownership dictionary values
        string ownershipIds = "";

        foreach (object data in ListPreFilterOrgOwnership.SelectedValues)
        {
            if (ownershipIds.Length > 0)
            {
                ownershipIds = ownershipIds + "|";
            }

            ownershipIds = ownershipIds + data.ToString();
        }

        if (dataTransferRight is int && ownershipIds.Length > 0)
        {
            if (RadioButtonLeaveObjects.Checked)
            {
                return "+;" + dataTransferRight.ToString() + ";[" + ownershipIds + "]";
            }
            else
            {
                return "-;" + dataTransferRight.ToString() + ";[" + ownershipIds + "]";
            }
        }

        return "";
    }

    protected void ParsePreFilterString(string preFilter)
    {
        PreFilterRightId = -1;
        PreFilterOrgOwnershipIdList.Clear();
        PreFilterMode = "";

        if (preFilter.Length > 0)
        {
            string[] parts = preFilter.Split(new char[] { ';' });

            if (parts.Length == 3)
            {
                PreFilterMode = parts[0];
                PreFilterRightId = int.Parse(parts[1]);

                // The list of ownership types looks like [A|B|C]
                if (parts[2].StartsWith("["))
                {
                    parts[2] = parts[2].Substring(1);
                }

                if (parts[2].EndsWith("]"))
                {
                    parts[2] = parts[2].Substring(0, parts[2].Length - 1);
                }

                string[] ownershipIds = parts[2].Split(new char[] { '|' });

                foreach (string ownership in ownershipIds)
                {
                    try
                    {
                        PreFilterOrgOwnershipIdList.Add(int.Parse(ownership));
                    }
                    finally
                    {
                    }
                }
            }
        }
    }

    protected void SqlDataSourceObjectTransfer_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
    }

    protected void SqlDataSourceObjectTransfer_InclusivePreFilter_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;

        if (PreFilterRightId >= 0)
        {
            e.Command.Parameters["@rid"].Value = PreFilterRightId;
        }
        else
        {
            if (ComboPreFilterRight.Value is int)
            {
                e.Command.Parameters["@rid"].Value = (int)ComboPreFilterRight.Value;
            }
            else
            {
                e.Command.Parameters["@rid"].Value = -1;
            }
        }

        if (PreFilterOrgOwnershipIdList.Count > 0)
        {
            e.Command.Parameters["@foid"].Value = FormatOwnershipListForSqlQuery(PreFilterOrgOwnershipIdList);
        }
        else
        {
            if (ListPreFilterOrgOwnership.SelectedValues.Count > 0)
            {
                e.Command.Parameters["@foid"].Value = FormatOwnershipListForSqlQuery(ListPreFilterOrgOwnership.SelectedValues);
            }
            else
            {
                e.Command.Parameters["@foid"].Value = "";
            }
        }
    }

    protected void SqlDataSourceObjectTransfer_ExclusivePreFilter_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        SqlDataSourceObjectTransfer_InclusivePreFilter_Selecting(sender, e);
    }

    protected string FormatOwnershipListForSqlQuery(System.Collections.IEnumerable en)
    {
        string res = "+";

        System.Collections.IEnumerator enumerator = en.GetEnumerator();

        while (enumerator.MoveNext())
        {
            if (enumerator.Current is int || enumerator.Current is string)
            {
                res += enumerator.Current.ToString();
                res += "+";
            }
        }

        return res;
    }

    protected void UpdatePreFilterControls()
    {
        if (PreFilterMode.Length > 0 && PreFilterRightId >= 0 && PreFilterOrgOwnershipIdList.Count > 0)
        {
            ComboPreFilterRight.DataBind();
            ListPreFilterOrgOwnership.DataBind();

            ComboPreFilterRight.SelectedItem = ComboPreFilterRight.Items.FindByValue(PreFilterRightId);

            ListPreFilterOrgOwnership.UnselectAll();

            foreach (int ownership in PreFilterOrgOwnershipIdList)
            {
                ListEditItem item = ListPreFilterOrgOwnership.Items.FindByValue(ownership.ToString());

                if (item != null)
                {
                    item.Selected = true;
                }
            }

            RadioButtonLeaveObjects.Checked = (PreFilterMode == "+");
            RadioButtonExcludeObjects.Checked = (PreFilterMode == "-");
        }
        else
        {
            ComboPreFilterRight.SelectedIndex = -1;
            ListPreFilterOrgOwnership.UnselectAll();
        }
    }

    #endregion Pre-filter builder for 'object transfer' grid

    #region Session management

    protected string GetPageUniqueKey()
    {
        object key = ViewState["PageUniqueKey"];

        if (key is string)
        {
            return (string)key;
        }

        // Generate unique key
        Guid guid = Guid.NewGuid();

        string str = guid.ToString();

        ViewState["PageUniqueKey"] = str;

        return str;
    }

    protected int PreFilterRightId
    {
        get
        {
            string key = GetPageUniqueKey() + "_PreFilterRightId";

            object rightId = Session[key];

            if (rightId is int)
            {
                return (int)rightId;
            }

            return -1;
        }

        set
        {
            string key = GetPageUniqueKey() + "_PreFilterRightId";

            Session[key] = value;
        }
    }

    protected List<int> PreFilterOrgOwnershipIdList
    {
        get
        {
            string key = GetPageUniqueKey() + "_PreFilterOrgOwnershipIdList";

            object list = Session[key];

            if (list is List<int>)
            {
                return list as List<int>;
            }

            list = new List<int>();

            Session[key] = list;

            return list as List<int>;
        }
    }

    protected string PreFilterMode
    {
        get
        {
            string key = GetPageUniqueKey() + "_PreFilterMode";

            object mode = Session[key];

            if (mode is string)
            {
                return (string)mode;
            }

            return "";
        }

        set
        {
            string key = GetPageUniqueKey() + "_PreFilterMode";

            Session[key] = value;
        }
    }

    #endregion Session management
}