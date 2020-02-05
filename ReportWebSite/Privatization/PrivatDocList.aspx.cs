using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web.ASPxGridView;

public partial class Privatization_PrivatDocList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
            var fixedColumns = string.Empty;

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDPrivatization_Documents)
            {
                LabelReportTitle2.Text = reportTitle;
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
    }

    protected void ASPxButton_Privatization_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewPrivatizationExporter, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Privatization_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewPrivatizationExporter, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Privatization_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewPrivatizationExporter, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    private int ExpandedPrivatMasterDocID
    {
        get { return Session["ExpandedPrivatMasterDocID"] is int ? (int)Session["ExpandedPrivatMasterDocID"] : 0; }
        set { Session["ExpandedPrivatMasterDocID"] = value; }
    }

    private int ExpandedPrivatSlaveDocID
    {
        get { return Session["ExpandedPrivatSlaveDocID"] is int ? (int)Session["ExpandedPrivatSlaveDocID"] : 0; }
        set { Session["ExpandedPrivatSlaveDocID"] = value; }
    }

    protected void GridViewPrivatizationDetail_BeforePerformDataSelect(object sender, EventArgs e)
    {
        ASPxGridView GridViewPrivatizationDetail = (ASPxGridView)sender;

        object masterDocId = GridViewPrivatizationDetail.GetMasterRowFieldValues("master_doc_id");
        object slaveDocId = GridViewPrivatizationDetail.GetMasterRowFieldValues("slave_doc_id");

        if (masterDocId is int)
        {
            ExpandedPrivatMasterDocID = (int)masterDocId;
        }
        else
        {
            ExpandedPrivatMasterDocID = 0;
        }

        if (slaveDocId is int)
        {
            ExpandedPrivatSlaveDocID = (int)slaveDocId;
        }
        else
        {
            ExpandedPrivatSlaveDocID = 0;
        }
    }

    protected void SqlDataSourceViewPrivatDetail_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_master_doc_id"].Value = ExpandedPrivatMasterDocID;
        e.Command.Parameters["@p_slave_doc_id"].Value = ExpandedPrivatSlaveDocID;
    }

    protected void GridViewPrivatization_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;
        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 70);

        Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDPrivatization_Documents, "");
    }

    protected void GridViewPrivatization_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.ASPxEditors.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewPrivatization_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
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