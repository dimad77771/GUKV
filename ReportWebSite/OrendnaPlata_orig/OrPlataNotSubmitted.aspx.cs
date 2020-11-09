using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using DevExpress.Web;
using System.Data.SqlClient;

public partial class OrendnaPlata_OrPlataNotSubmitted : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        ButtonShowFoldersPopup.Visible = userIsReportManager;

        // Check if this is first loading of this page
        object uniqueKey = ViewState["PageUniqueKey"];
        bool isFirstLoading = (uniqueKey == null);

        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // If user-defined report is displayed, switch to the proper page of the Page control
        if (isFirstLoading)
        {
            string reportTitle = "";
            string preFilter = "";
            var fixedColumns = string.Empty;

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDOrendnaPlata_NotSubmitted)
            {
                LabelReportTitle5.Text = reportTitle;
                Utils.RestoreFixedColumns(PrimaryGridView, fixedColumns);
            }
            else
            {
                // Restore grid fixed columns
                Utils.RestoreFixedColumns(PrimaryGridView);
            }
        }

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);

        // Enable advanced header filter for all grid columns
        Utils.AdjustColumnsVisibleInFilter(PrimaryGridView);

        // Disable the 'Vipiski' menu item if user is RDA
        if (Utils.RdaDistrictID > 0)
        {
            SectionMenu.Items[6].ClientVisible = false;
        }
    }

    protected void ASPxButton_RentNotSubmitted_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewRentNotSubmittedExporter, PrimaryGridView, LabelReportTitle5.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_RentNotSubmitted_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewRentNotSubmittedExporter, PrimaryGridView, LabelReportTitle5.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_RentNotSubmitted_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewRentNotSubmittedExporter, PrimaryGridView, LabelReportTitle5.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewRentNotSubmitted_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;

        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 35);

        Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDOrendnaPlata_NotSubmitted, "");
    }

    protected void GridViewRentNotSubmitted_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewRentNotSubmitted_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void SqlDataSourceRentNotSubmitted_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
    }

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

    #region Data binding support

    public string EvaluateObjectLink(object buildingId, object address)
    {
        if (!(address is string))
        {
            return "";
        }

        if (!(buildingId is int))
        {
            return address.ToString();
        }

        return "<a href=\"javascript:ShowObjectCardSimple(" + buildingId.ToString() + ")\">" + address.ToString() + "</a>";
    }

    public string EvaluateOrganizationLink(object organizationId, object name)
    {
        if (!(name is string))
        {
            return "";
        }

        if (!(organizationId is int))
        {
            return name.ToString();
        }

        return "<a href=\"javascript:ShowOrganizationCard(" + organizationId.ToString() + ")\">" + name.ToString() + "</a>";
    }

    #endregion (Data binding support)
}