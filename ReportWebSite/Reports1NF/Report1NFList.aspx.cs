using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class Reports1NF_Report1NFList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SectionMenu.Visible = Roles.IsUserInRole(Utils.Report1NFReviewerRole);

        // The 'Notifications' page must be visible only to users that can receive some notifications
        if (Roles.IsUserInRole(Utils.DKVOrganizationControllerRole) ||
            Roles.IsUserInRole(Utils.DKVObjectControllerRole) ||
            Roles.IsUserInRole(Utils.DKVArendaControllerRole) ||
            Roles.IsUserInRole(Utils.DKVArendaPaymentsControllerRole))
        {
            SectionMenu.Items[2].Visible = true;
        }
        else
        {
            SectionMenu.Items[2].Visible = false;
        }


		if (Roles.IsUserInRole(Utils.RDAControllerRole))
		{
			SectionMenuForRDARole.Visible = true;
		}

        if (IsSmallForm)
		{
            SectionMenuForSmallMode.Visible = true;
            SectionMenu.Visible = false;
            //PrimaryGridView.Set.
            var commandColumn = PrimaryGridView.Columns.OfType<GridViewCommandColumn>().Single();
            commandColumn.ShowEditButton = false;
        }

		PrimaryGridView.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;
		PrimaryGridView.SettingsEditing.Mode = GridViewEditingMode.Inline;
	}

    protected bool IsSmallForm
    {
        get
        {
            return Request.QueryString["smallform"] == null ? false : true;
        }
    }

    protected void ASPxButton_Reports1NF_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewReports1NFExporter, PrimaryGridView, LabelReportTitle1.Text, "");
    }

    protected void ASPxButton_Reports1NF_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewReports1NFExporter, PrimaryGridView, LabelReportTitle1.Text, "");
    }

    protected void ASPxButton_Reports1NF_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewReports1NFExporter, PrimaryGridView, LabelReportTitle1.Text, "");
    }

    protected void GridViewReports1NF_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        Utils.ProcessDataGridSaveLayoutCallback(e.Parameters, PrimaryGridView, Utils.GridIDReports1NF_ReportList, "");

        PrimaryGridView.DataBind();
    }

    protected void GridViewReports1NF_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewReports1NF_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void SqlDataSourceReports_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
        e.Command.Parameters["@period_year"].Value = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
    }

	protected void SqlDataSourceReports_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		var dbparams = (System.Data.SqlClient.SqlParameterCollection)(e.Command.Parameters);
		dbparams.AddWithValue("@stan_recieve_date", DateTime.Now);

		//var user = Membership.GetUser();
		//var username = (user == null ? String.Empty : (String)user.UserName);
		//dbparams.AddWithValue("@modified_by2", username);
	}
}