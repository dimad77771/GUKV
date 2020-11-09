using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;

public partial class Balans_BalansObjects : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        ButtonShowFoldersPopup1.Visible = userIsReportManager;

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

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDBalans_Objects)
            {
                LabelReportTitle1.Text = reportTitle;
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

	PrimaryGridView.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;	
    }

    protected void ASPxButton_BalansObjects_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterBalansObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_BalansObjects_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterBalansObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_BalansObjects_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterBalansObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewBalansObjects_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;
        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 60);

        Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDBalans_Objects, "");
    }

    protected void GridViewBalansObjects_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewBalansObjects_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void SqlDataSourceBalansObjects_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_dpz_filter"].Value = CheckBoxBalansObjectsDPZ.Checked ? 1 : 0;
        e.Command.Parameters["@p_com_filter"].Value = CheckBoxBalansObjectsComVlasn.Checked ? 1 : 0;
        e.Command.Parameters["@p_show_deleted"].Value = CheckBoxBalansObjectsShowDeleted.Checked ? 1 : 0;
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
    }

    protected void GridViewBalansObjects_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
    {
        Utils.CheckCustomSummaryVisibility(PrimaryGridView, e);
        Utils.CustomSummaryUniqueBalansOrgAndObject(e, "organization_id", false);
    }

    protected void GridViewBalansObjects_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
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
}