using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;

public partial class Balans_BalansOrgList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect(Page.ResolveClientUrl("~/Reports1NF/Report1NFList.aspx?smallform=1"));
        return;


        Response.Cache.SetNoStore();

        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        ButtonShowFoldersPopup2.Visible = userIsReportManager;

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

            if (this.ProcessPageLoad(out reportTitle, out preFilter) == Utils.GridIDBalans_Organizations)
            {
                LabelReportTitle2.Text = reportTitle;
            }
        }

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);

        // Enable advanced header filter for all grid columns
        Utils.AdjustColumnsVisibleInFilter(PrimaryGridView);

	PrimaryGridView.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;
    }

    private int ExpandedBalansOrgID
    {
        get { return Session["ExpandedBalansOrgID"] is int ? (int)Session["ExpandedBalansOrgID"] : 0; }
        set { Session["ExpandedBalansOrgID"] = value; }
    }

    protected void ASPxGridViewBalansDetail_BeforePerformDataSelect(object sender, EventArgs e)
    {
        ASPxGridView ASPxGridViewBalansDetail = (ASPxGridView)sender;

        ExpandedBalansOrgID = (int)ASPxGridViewBalansDetail.GetMasterRowKeyValue();
    }

    protected void SqlDataSourceBalansDetail_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@organization_id"].Value = ExpandedBalansOrgID;
    }

    protected void ASPxButton_Balans_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterBalans, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Balans_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterBalans, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Balans_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterBalans, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewBalans_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;
        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 60);

        Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDBalans_Organizations, "");
    }

    protected void GridViewBalans_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewBalans_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void GridViewBalans_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
    }

    protected void SqlDataSourceBalansOrganizations_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_dpz_filter"].Value = CheckBoxRentedObjectsDPZ.Checked ? 1 : 0;
        e.Command.Parameters["@p_com_filter"].Value = CheckBoxBalansHoldersComVlasn.Checked ? 1 : 0;
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
}