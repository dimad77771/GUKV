using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using GUKV.Common;
using Syncfusion.XlsIO;

public partial class Reports1NF_Report1NFPrivatisatSquare : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		//SectionMenu.Visible = Roles.IsUserInRole(Utils.Report1NFReviewerRole);
		SectionMenu.Visible = false;

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

		PrivatisatGridView.SettingsEditing.Mode = GridViewEditingMode.Inline;

		if (Utils.IsBigBossUser())
		{
			var col = PrivatisatGridView.Columns.OfType<GridViewCommandColumn>().First();
			col.ShowEditButton = false;
			col.ShowUpdateButton = false;
			col.ShowCancelButton = false;
			col.ShowNewButton = false;
			col.ShowDeleteButton = false;
		}

		//((GridViewDataSpinEditColumn)FreeSquareGridView.Columns["privat_year"]).PropertiesSpinEdit.SpinButtons.Visible = false;

		//ForTest();
	}

	void ForTest()
	{
		var lines = new List<string>();
		var zags = System.IO.File.ReadAllLines(@"C:\Users\ASUS\Documents\SQL Server Management Studio\qqqq2233.txt", System.Text.Encoding.GetEncoding("windows-1251"));
		foreach (var zag in zags)
		{
			var column = PrivatisatGridView.Columns.OfType<GridViewDataColumn>().Single(q => getNormalizeString(q.Caption) == getNormalizeString(zag));
			var findcol = column.FieldName;

			lines.Add(findcol + " as [" + zag + "]");
			//lines.Add(findcol);
		}

		var sql = string.Join(",\n", lines);
	}

	string getNormalizeString(string arg)
	{
		return arg.Replace("″", "\"");
	}

	protected void ASPxButton_FreeSquare_ExportXLS_Click(object sender, EventArgs e)
	{
		this.ExportGridToXLS(GridViewFreeSquareExporter, PrivatisatGridView, LabelReportTitle1.Text, "",
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

		for (coln = 1; coln <= worksheet.Columns.Length; coln++)
		{
			if (worksheet.Range[2, coln].Text == "Унікальний код обєкту у ЕТС Прозорро-продажі")
			{
				for (int rown = 3; rown <= allrows; rown++)
				{
					var cell = worksheet.Range[rown, coln];
					var text = cell.Text ?? "";
					if (!string.IsNullOrEmpty(text))
					{
						var url = "https://prozorro.sale/planning/" + text;
						worksheet.HyperLinks.Add(cell, ExcelHyperLinkType.Url, url, "Відкрити обєкт у ЕТС Прозорро-продажі");
						cell.CellStyle.Font.Underline = ExcelUnderline.Single;
						cell.CellStyle.Font.Color = ExcelKnownColors.Blue;
					}
				}
			}
		}

		workbook.Save();
		workbook.Close();
		excelEngineMain.Dispose();
	}

	protected void ASPxButton_FreeSquare_ExportPDF_Click(object sender, EventArgs e)
	{
		this.ExportGridToPDF(GridViewFreeSquareExporter, PrivatisatGridView, LabelReportTitle1.Text, "");
	}

	protected void ASPxButton_FreeSquare_ExportCSV_Click(object sender, EventArgs e)
	{
		this.ExportGridToCSV(GridViewFreeSquareExporter, PrivatisatGridView, LabelReportTitle1.Text, "");
	}

	protected void GridViewFreeSquare_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		Utils.ProcessDataGridSaveLayoutCallback(e.Parameters, PrivatisatGridView, Utils.GridIDReports1NF_FreeSquare, "");

		PrivatisatGridView.DataBind();
	}

	protected void GridViewFreeSquare_CustomFilterExpressionDisplayText(object sender,
		DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
	{
		this.UpdateFilterDisplayTextCache(e.DisplayText, PrivatisatGridView);
	}

	protected void GridViewFreeSquare_ProcessColumnAutoFilter(object sender,
		DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
	{
		Utils.ProcessGridColumnAutoFilter(sender, e);
	}


	protected void SqlDataSourcePrivatisat_Inserting(object sender, SqlDataSourceCommandEventArgs e)
	{
		OnInsertingUpdating(e, isNew: true);
	}

	protected void SqlDataSourcePrivatisat_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		//OnInsertingUpdating(e, isNew: false);
	}

	void OnInsertingUpdating(SqlDataSourceCommandEventArgs e, bool isNew)
	{
		var dbparams = (System.Data.SqlClient.SqlParameterCollection)(e.Command.Parameters);
		dbparams.AddWithValue("@modify_date2", DateTime.Now);
		var user = Membership.GetUser();
		var username = (user == null ? String.Empty : (String)user.UserName);
		dbparams.AddWithValue("@modified_by2", username);

		//var geodata_map_points = (string)(e.Command.Parameters["@geodata_map_points"].Value);
		//if (!Validate_geodata_map_points(geodata_map_points))
		//{
		//	throw new Exception("Невірно заповнене поле \"Координати на мапі\". Приклад вірно заповненого поля (широта довгота) \"50.509205 30.426741\"");
		//}
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


	protected void ASPxButton_Report_Click(object sender, EventArgs e)
	{
		var builder = new PrognozPaymentZvitBuilder
		{
			Page = this,
			UseInflation = CheckBoxInflation.Checked,
			UseDictRentalRate = CheckBoxDictRentalRate.Checked,
		};
		builder.Go();
	}

	protected void SqlDataSourceReports_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
		e.Command.Parameters["@period_year"].Value = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
		e.Command.Parameters["@p_misto_id"].Value = 0;
		e.Command.Parameters["@smode"].Value = 0;
		e.Command.Parameters["@p_show_neziznacheni"].Value = true;
		e.Command.Parameters["@p_show_neviznacheni"].Value = true;
	}



	protected void ASPxButton_change_Click(object sender, EventArgs e)
	{
		var contribution_rate = EditChange.Value;

		var reportIds = new List<int>();
		for (int i = PrivatisatGridView.VisibleStartIndex; i < PrivatisatGridView.VisibleRowCount; i++)
		{
			var report_id = (int)PrivatisatGridView.GetRowValues(i, new[] { "report_id" });
			reportIds.Add(report_id);

		}

		var connectionSql = CommonUtils.ConnectToDatabase2016();
		using (SqlTransaction transaction = connectionSql.BeginTransaction())
		{
			foreach(var report_id in reportIds)
			{
				var sql = "delete from reports1nf_org_info_new_contribution_rate where report_id = @report_id;";
				if (contribution_rate != null)
				{
					sql += "INSERT INTO reports1nf_org_info_new_contribution_rate values (@report_id, @contribution_rate)";
				}

				using (var cmd = new SqlCommand(sql, connectionSql,transaction))
				{
					cmd.Parameters.Add("@report_id", report_id);
					if (contribution_rate != null)
					{
						cmd.Parameters.Add("@contribution_rate", contribution_rate);
					}
					cmd.ExecuteNonQuery();
				}
			}

			transaction.Commit();
		}

		PrivatisatGridView.DataBind();
	}
}


