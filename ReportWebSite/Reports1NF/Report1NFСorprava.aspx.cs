﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Syncfusion.XlsIO;

public partial class Reports1NF_Report1NFСorprava : System.Web.UI.Page
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

		СorpravaGridView.SettingsEditing.Mode = GridViewEditingMode.Inline;
		
		//((GridViewDataSpinEditColumn)FreeSquareGridView.Columns["privat_year"]).PropertiesSpinEdit.SpinButtons.Visible = false;
	}

	protected void ASPxButton_FreeSquare_ExportXLS_Click(object sender, EventArgs e)
	{
		this.ExportGridToXLS(GridViewFreeSquareExporter, СorpravaGridView, LabelReportTitle1.Text, "",
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
		for (int rown = 1; rown <= allrows; rown++)
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
		this.ExportGridToPDF(GridViewFreeSquareExporter, СorpravaGridView, LabelReportTitle1.Text, "");
	}

	protected void ASPxButton_FreeSquare_ExportCSV_Click(object sender, EventArgs e)
	{
		this.ExportGridToCSV(GridViewFreeSquareExporter, СorpravaGridView, LabelReportTitle1.Text, "");
	}

	protected void GridViewFreeSquare_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		Utils.ProcessDataGridSaveLayoutCallback(e.Parameters, СorpravaGridView, Utils.GridIDReports1NF_FreeSquare, "");

		СorpravaGridView.DataBind();
	}

	protected void GridViewFreeSquare_CustomFilterExpressionDisplayText(object sender,
		DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
	{
		this.UpdateFilterDisplayTextCache(e.DisplayText, СorpravaGridView);
	}

	protected void GridViewFreeSquare_ProcessColumnAutoFilter(object sender,
		DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
	{
		Utils.ProcessGridColumnAutoFilter(sender, e);
	}

	protected void SqlDataSourceСorprava_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		//e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
		//e.Command.Parameters["@period_year"].Value = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
		e.Command.Parameters["@baseurl"].Value = Utils.WebsiteBaseUrl;
	}

	protected void SqlDataSourceСorprava_Inserting(object sender, SqlDataSourceCommandEventArgs e)
	{
		OnInsertingUpdating(e, isNew: true);
	}

	protected void SqlDataSourceСorprava_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		OnInsertingUpdating(e, isNew: false);
	}

	void OnInsertingUpdating(SqlDataSourceCommandEventArgs e, bool isNew)
	{
		var dbparams = (System.Data.SqlClient.SqlParameterCollection)(e.Command.Parameters);
		dbparams.AddWithValue("@modify_date2", DateTime.Now);
		var user = Membership.GetUser();
		var username = (user == null ? String.Empty : (String)user.UserName);
		dbparams.AddWithValue("@modified_by2", username);

		var geodata_map_points = (string)(e.Command.Parameters["@geodata_map_points"].Value);
		if (!Validate_geodata_map_points(geodata_map_points))
		{
			throw new Exception("Невірно заповнене поле \"Координати на мапі\". Приклад вірно заповненого поля (широта довгота) \"50.509205 30.426741\"");
		}
	}

	bool Validate_geodata_map_points(string geodata_map_points)
	{
		if (string.IsNullOrEmpty(geodata_map_points))
		{
			return true;
		}

		var is_good = false;
		var regpoints = (new Regex(@"^(\d+\.\d+)\s+(\d+\.\d+)$")).Match(geodata_map_points);
		if (regpoints.Groups.Count == 3)
		{
			try
			{
				var point1 = Decimal.Parse(regpoints.Groups[1].Value, CultureInfo.InvariantCulture);
				var point2 = Decimal.Parse(regpoints.Groups[2].Value, CultureInfo.InvariantCulture);
				is_good = true;
			}
			catch (Exception ex)
			{
				is_good = false;
			}
		}

		return is_good;
	}

	protected void ObjectDataSourcePhotoFiles_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
	{
		if (Request.Cookies["RecordID"] != null)
			e.InputParameters["RecordID"] = Request.Cookies["RecordID"].Value;

		//if (Request.QueryString["bid"] != null)
		//    e.InputParameters["balans_id"] = int.Parse(Request.QueryString["bid"]);
	}

}