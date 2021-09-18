using System;
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

public partial class Reports1NF_Report1NFFreeSquare : System.Web.UI.Page
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

		FreeSquareGridView.SettingsEditing.Mode = GridViewEditingMode.Inline;

		if (Utils.IsBigBossUser())
		{
			var col = FreeSquareGridView.Columns.OfType<GridViewCommandColumn>().First();
			col.ShowEditButton = false;
			col.ShowUpdateButton = false;
			col.ShowCancelButton = false;
			col.ShowNewButton = false;
			col.ShowDeleteButton = false;
		}

		if (IsReportForm)
		{
			CustomizeReportForm();
		}

		SectionMenu.Visible = false;
	}

	void CustomizeReportForm()
	{
		if (!IsPostBack)
		{
			//var ggg = FreeSquareGridView.FilterExpression;
			FreeSquareGridView.FilterExpression = @"[komis_protocol] Not Like '0%' And [komis_protocol] Is Not Null And [is_included] = True And [geodata_map_points] Is Not Null";
		}

		FreeSquareGridView.SettingsCookies.Enabled = false;
		ASPxButtonEditColumnList.Visible = false;
		//ASPxButton_FreeSquare_SaveAs.Visible = false;
		SectionMenu.Visible = false;


		var visibleColumns = new[]
		{
			@"Балансоутримувач",
			@"Код ЄДРПОУ",
			@"Район",
			@"Назва Вулиці",
			@"Номер Будинку",
			@"Включено до переліку вільних приміщень",
			@"Включено до переліку №",
			@"Погодження орендодавця",
			@"Координати на мапі",
			@"Форма Власності",
			@"Орган госп. упр.",
			@"Наявність рішень про проведення інвестиційного конкурсу",
			@"Ініціатор оренди",
			@"Погодження органу управління балансоутримувача",
			@"Погодження органу охорони культурної спадщини",
			@"Тип об’єкта",
			@"Пам’ятка культурної спадщини",
			@"Унікальний код обєкту у ЕТС Прозорро-продажі",
			@"Pdf",
			@"Залишкова балансова вартість, грн.",
			@"Первісна балансова вартість, грн.",
			@"Пропонований строк оренди (у роках)",
			@"Номер запису про право власності у Реєстрація у Державному реєстрі речових прав на нерухоме майно",
			@"Реєстраційний номер об'єкту нерухомого майна у Реєстрація у Державному реєстрі речових прав на нерухоме майно",
			@"Період часу, протягом якого об’єкт не використовується",
			@"Інформацію про особу, яка використовувала об’єкт перед тим, як він став вакантним (якщо такою особою був балансоутримувач, проставляється позначка “об’єкт використовувався балансоутримувачем”)",
			@"Дата формування залишкової вартості",
			@"Особа відповідальна за ознайомлення з об’єктом",
			@"Характеристика об’єкта оренди",
			@"Загальна площа об’єкта",
			@"Технічний стан об’єкта",
			@"Можливе використання вільного приміщення",
		};
		int npp = 0;
		foreach (GridViewColumn column in FreeSquareGridView.Columns)
		{
			if (column is GridViewCommandColumn)
			{
				column.Visible = false;
			}
			else if (column is GridViewBandColumn)
			{
				var col_visible = false;
				foreach(GridViewColumn dcolumn in (column as GridViewBandColumn).Columns)
				{
					var visible = visibleColumns.Any(colnam => Utils.EqualColumnTitle(dcolumn, colnam));
					dcolumn.Visible = visible;
					if (visible)
					{
						col_visible = true;
					}
					if (dcolumn.Visible) npp++;
				}
				column.Visible = col_visible;
			}
			else
			{
				var visible = visibleColumns.Any(colnam => Utils.EqualColumnTitle(column, colnam));
				column.Visible = visible;
				if (column.Visible) npp++;
			}
		}
		npp = npp;
	}


	protected bool IsReportForm
	{
		get
		{
			return Request.QueryString["reportform"] == null ? false : true;
		}
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
	}

	protected void SqlDataSourceFreeSquare_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		//DevExpress.Web.ASPxComboBox nnn;
		//GridViewDataComboBoxColumn rrr;
		//rrr.PropertiesComboBox.sc
		//rrr.PropertiesComboBox.ClientSideEvents
		//GridViewDataTextColumn a;
		//a.PropertiesEditType
		//a.PropertiesEditType
		//GridViewDataTextColumn
		//DevExpress.Web.GridViewDataDateColumn


		//if (!string.IsNullOrEmpty(Request.QueryString["rid"]))
		//	e.Command.Parameters["@report_id"].Value = int.Parse(Request.QueryString["rid"]);

		//if (!string.IsNullOrEmpty(Request.QueryString["bid"]))
		//	e.Command.Parameters["@balans_id"].Value = int.Parse(Request.QueryString["bid"]);

		//+e.Command.Parameters    { System.Data.SqlClient.SqlParameterCollection}
		//System.Data.Common.DbParameterCollection { System.Data.SqlClient.SqlParameterCollection}

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


		//e.Command.Parameters.Add("@modify_date2");
		//e.Command.Parameters.Add("@modified_by2");

		//e.Command.Parameters["@modify_date2"].Value = DateTime.Now;
		//var user = Membership.GetUser();
		//e.Command.Parameters["@modified_by2"].Value = (user == null ? String.Empty : (String)user.UserName);

		//e.Command.Parameters["@id"].Value = 1;
		//e.Command.Parameters["@komis_protocol"].Value = "fff";
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