using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Syncfusion.XlsIO;

public partial class Reports1NF_RentAgreementsShow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		/*       SectionMenu.Visible = System.Web.Security.Roles.IsUserInRole(Utils.Report1NFReviewerRole);

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
	   */
		FreeSquareGridView.SettingsEditing.Mode = GridViewEditingMode.Inline;

		if (HasFsid)
		{
			FreeSquareGridView.SettingsDetail.ShowDetailRow = true;
			FreeSquareGridView.TotalSummary.Clear();
			FreeSquareGridView.Settings.ShowFilterRow = false;
			FreeSquareGridView.Settings.ShowFilterRowMenu = false;
			FreeSquareGridView.Settings.ShowGroupPanel = false;
			FreeSquareGridView.Settings.ShowFooter = false;
			FreeSquareGridView.Settings.ShowHeaderFilterButton = false;
			FreeSquareGridView.Settings.ShowFilterBar = GridViewStatusBarMode.Hidden;
		}

		//DevExpress.Web.ASPxFileManager.ASPxFileManager a;
		//a.SettingsUpload
		//a.SettingsToolbar
		//a.SettingsEditing
		//a.SettingsUpload
	}

    protected void ASPxButton_FreeSquare_ExportXLS_Click(object sender, EventArgs e)
    {
		this.ExportGridToXLS(GridViewFreeSquareExporter, FreeSquareGridView, LabelReportTitle1.Text, "",
			exportHiddenColumnCallback: q => (q.FieldName == "pdfurl"),
			afterBuildXlsx: ASPxButton_FreeSquare_ExportXLS_AfterBuildXlsx
		);
    }

	void ASPxButton_FreeSquare_ExportXLS_AfterBuildXlsx(string excelfilename)
	{
		var excelEngineMain = new ExcelEngine();
		var workbook = excelEngineMain.Excel.Workbooks.Open(excelfilename);
		var worksheet = workbook.Worksheets[0];

		//var style = workbook.Styles.Add("HrefNewStyle");
		//style.Color = Color.FromArgb(0, 0, 255);
		//style.Font.Underline = ExcelUnderline.Single;

		var coln = 26;
		var allrows = worksheet.Rows.Length;
		for(int rown = 1; rown <= allrows; rown++)
		{
			var cell = worksheet.Range[rown, coln];
			var text = cell.Text ?? "";
			if (text.ToLower().StartsWith("http"))
			{
				worksheet.HyperLinks.Add(cell, ExcelHyperLinkType.Url, text, "Відкрити фото/плани");
				//cell.CellStyle = style;
				cell.CellStyle.Font.Underline = ExcelUnderline.Single;
				cell.CellStyle.Font.Color = ExcelKnownColors.Blue;
			}
		}

		workbook.Save();
		workbook.Close();
		excelEngineMain.Dispose();
	}

	protected void ASPxButton_FreeSquare_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewFreeSquareExporter, FreeSquareGridView, LabelReportTitle1.Text, "");
    }

    protected void ASPxButton_FreeSquare_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewFreeSquareExporter, FreeSquareGridView, LabelReportTitle1.Text, "");
    }

    protected void GridViewFreeSquare_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        Utils.ProcessDataGridSaveLayoutCallback(e.Parameters, FreeSquareGridView, Utils.GridIDReports1NF_FreeSquare, "");

        FreeSquareGridView.DataBind();
    }

    protected void GridViewFreeSquare_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, FreeSquareGridView);
    }

    protected void GridViewFreeSquare_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void SqlDataSourceFreeSquare_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
        e.Command.Parameters["@period_year"].Value = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
		e.Command.Parameters["@baseurl"].Value = Utils.WebsiteBaseUrl;
		e.Command.Parameters["@fs_id"].Value = (string.IsNullOrEmpty(ParamFsid) ? -1 : Int32.Parse(ParamFsid));
		e.Command.Parameters["@mode50"].Value = (string.IsNullOrEmpty(ParamMode50) ? 0 : 1);
	}

	protected void SqlDataSourceFreeSquare_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		//if (!string.IsNullOrEmpty(Request.QueryString["rid"]))
		//	e.Command.Parameters["@report_id"].Value = int.Parse(Request.QueryString["rid"]);

		//if (!string.IsNullOrEmpty(Request.QueryString["bid"]))
		//	e.Command.Parameters["@balans_id"].Value = int.Parse(Request.QueryString["bid"]);

		//e.Command.Parameters["@modify_date"].Value = DateTime.Now;
		//MembershipUser user = Membership.GetUser();
		//e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;

		//e.Command.Parameters["@id"].Value = 1;
		//e.Command.Parameters["@komis_protocol"].Value = "fff";
	}

	protected string ParamFsid
	{
		get
		{
			return Request.QueryString["fs_id"];
		}
	}

	protected bool HasFsid
	{
		get
		{
			return !string.IsNullOrEmpty(Request.QueryString["fs_id"]);
		}
	}


	protected string ParamMode50
	{
		get
		{
			return Request.QueryString["mode50"] ?? "";
		}
	}


	protected void FreeSquareGridView_DataBound(object sender, EventArgs e)
	{
		if (HasFsid)
		{ 
			FreeSquareGridView.DetailRows.ExpandAllRows();
			FreeSquareGridView.SettingsDetail.ShowDetailButtons = false;
		}
	}

	protected void SqlDataSourceBalansObjects_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		e.Command.Parameters["@p_dpz_filter"].Value = 1;
		e.Command.Parameters["@p_com_filter"].Value = 0;
		e.Command.Parameters["@p_show_deleted"].Value = 0;
		e.Command.Parameters["@p_rda_district_id"].Value = 0;
	}

	protected void GridViewBalansObjects_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		//string param = e.Parameters;
		//Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 60);
		//Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDBalans_Objects, "");
	}

	protected void GridViewBalansObjects_CustomFilterExpressionDisplayText(object sender, DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
	{
		//this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
	}

	protected void GridViewBalansObjects_ProcessColumnAutoFilter(object sender, DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
	{
		//Utils.ProcessGridColumnAutoFilter(sender, e);
	}

	protected void GridViewBalansObjects_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
	{
		//Utils.CheckCustomSummaryVisibility(PrimaryGridView, e);
		//Utils.CustomSummaryUniqueBalansOrgAndObject(e, "organization_id", false);
	}

	protected void GridViewBalansObjects_CustomColumnSort(object sender, DevExpress.Web.CustomColumnSortEventArgs e)
	{
		//Utils.ProcessGridSortByBuildingNumber(e);
	}

	protected void SqlDataSourceArendaObjects_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		e.Command.CommandTimeout = 600;

		e.Command.Parameters["@p_dpz_filter"].Value = 1;
		e.Command.Parameters["@p_com_filter"].Value = 0;
		e.Command.Parameters["@p_rda_district_id"].Value = 0;
	}

	protected void GridViewArendaObjects_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		//string param = e.Parameters;
		//Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 60);
		//Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDArenda_Agreements, "");
	}

	protected void GridViewArendaObjects_CustomFilterExpressionDisplayText(object sender, CustomFilterExpressionDisplayTextEventArgs e)
	{
		//this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
	}

	protected void GridViewArendaObjects_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
	{
		//Utils.CheckCustomSummaryVisibility(sender as ASPxGridView, e);

		//// Get the field name for which the summary is being calculated
		//string field = "";

		//if (e.Item is ASPxSummaryItem)
		//{
		//	field = (e.Item as ASPxSummaryItem).FieldName.ToLower();
		//}

		//if (field.StartsWith("sqr_free_"))
		//{
		//	Utils.CustomSummaryUniqueBalansOrgAndObject(e, "org_balans_id", true);
		//}
		//else
		//{
		//	Utils.CustomSummaryExcludeSubarenda(e);
		//}
	}

	protected void GridViewArendaObjects_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e)
	{
		//Utils.ProcessGridColumnAutoFilter(sender, e);
	}

	protected void GridViewArendaObjects_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
	{
		//Utils.ProcessGridSortByBuildingNumber(e);
	}


}