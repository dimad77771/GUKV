using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web;
using FirebirdSql.Data.FirebirdClient;
using DevExpress.Web;
using DevExpress.Web;

public partial class Reports1NF_OrgBalansDeletedList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];

        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);

            // Set the parameters for SQL queries
            SqlDataSourceBalansStatus.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
            SqlDataSourceReportBalansDeleted.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
        }

        if (ReportID > 0)
        {
            int reportRdaDistrictId = -1;
            int reportOrganizationId = Reports1NFUtils.GetReportOrganizationId(ReportID, ref reportRdaDistrictId);

            for (int i = 0; i < SectionMenu.Items.Count; i++)
            {
                if (SectionMenu.Items[i].NavigateUrl.IndexOf('?') < 0)
                    SectionMenu.Items[i].NavigateUrl += "?rid=" + ReportID.ToString();
            }

            if (Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
            {
                if (Utils.UserOrganizationID != reportOrganizationId)
                {
                    Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
                }
            }
            else if (Roles.IsUserInRole(Utils.RDAControllerRole))
            {
                if (Utils.RdaDistrictID != reportRdaDistrictId)
                {
                    Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
                }
            }
        }
        else
        {
            Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
        }

        if (!Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
        {
            if (SectionMenu.Items.Count > 6)
                SectionMenu.Items[6].ClientVisible = false;

            ButtonSendAll.ClientVisible = false;
            CPStatus.ClientVisible = false;
        }

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);

        if (!Roles.IsUserInRole(Utils.RDAControllerRole))
        {
            SectionMenu.Visible = (Utils.GetLastReportId() <= 0);
        }
	}

    protected int ReportID
    {
        get
        {
            object reportId = ViewState["REPORT_ID"];

            if (reportId is int)
            {
                return (int)reportId;
            }

            return 0;
        }

        set
        {
            ViewState["REPORT_ID"] = value;
        }
    }

    protected void PrimaryGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        PrimaryGridView.DataBind();
    }

    protected void PrimaryGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "is_submitted")
        {
            if (e.CellValue is string && ((string)e.CellValue).Length < 3)
            {
                e.Cell.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void PrimaryGridView_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
    }

    protected void CPStatus_Callback(object sender, CallbackEventArgsBase e)
    {
        BalansStatusForm.DataBind();
    }

    protected void CPProgress_Callback(object sender, CallbackEventArgsBase e)
    {
        Reports1NFUtils.ProcessProgressPanelCallback(sender as ASPxCallbackPanel, e.Parameter, ProgressSendAll, LabelProgress,
            ReportID, new SendAllBalansDeletedObjectsWorkItem(), "DEL_BALANS_WORK_ITEM_", Resources.Strings.ProcessingDeletedBalansObjects);
    }

    protected void ASPxButton_BalansDeletedObjects_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterBalansDeletedObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_BalansDeletedObjects_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterBalansDeletedObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string);
    }
}