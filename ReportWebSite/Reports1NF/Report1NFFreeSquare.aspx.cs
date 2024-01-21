using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Compression;
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
			//SectionMenuForRDARole.Visible = true;
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
			CheckBoxBalansObjectsShowControl.Visible = false;
		}

		if (Utils.IsCabinetBalansoderzhatel())
		{
			var col = FreeSquareGridView.Columns.OfType<GridViewCommandColumn>().First();
			col.ShowEditButton = false;
			col.ShowUpdateButton = false;
			col.ShowCancelButton = false;
			col.ShowNewButton = false;
			col.ShowDeleteButton = false;
			CheckBoxBalansObjectsShowControl.Visible = false;
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
				foreach (GridViewColumn dcolumn in (column as GridViewBandColumn).Columns)
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
		this.ExportGridToPDF(GridViewFreeSquareExporter, FreeSquareGridView, LabelReportTitle1.Text, "");
	}

	protected void ASPxButton_FreeSquare_ExportCSV_Click(object sender, EventArgs e)
	{
		this.ExportGridToCSV(GridViewFreeSquareExporter, FreeSquareGridView, LabelReportTitle1.Text, "");
	}

	protected void GridViewFreeSquare_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if ((e.Parameters ?? "").StartsWith("auctionConfirmSendToOrendar:"))
		{
			var free_square_id = int.Parse(e.Parameters.Replace("auctionConfirmSendToOrendar:", ""));

			using (var connection = Utils.ConnectToDatabase())
			using (var transaction = connection.BeginTransaction())
			{
				CabinetUtils222.ChangeStage(free_square_id, 200840, connection, transaction);
				transaction.Commit();
			}

			FreeSquareGridView.DataBind();
		}

		else if (e.Parameters == "onAdogvorFileUploadComplete")
		{
			FreeSquareGridView.DataBind();
		}

		else
		{
			Utils.ProcessDataGridSaveLayoutCallback(e.Parameters, FreeSquareGridView, Utils.GridIDReports1NF_FreeSquare, "");
			FreeSquareGridView.DataBind();	
		}
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
		e.Command.Parameters["@p_show_neziznacheni"].Value = CheckBoxBalansObjectsShowNeziznacheni.Checked ? 1 : 0;
		e.Command.Parameters["@p_show_control"].Value = CheckBoxBalansObjectsShowControl.Checked ? 1 : 0;
		e.Command.Parameters["@userId"].Value = Utils.GetUserId();
		e.Command.Parameters["@bal_zkpo"].Value = Utils.GetCabinetBalansoderzhatelZkpo();
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

		//var freecycle_step_date = (DateTime?)(e.Command.Parameters["@freecycle_step_date"].Value);
		var free_square_id = (int)(e.Command.Parameters["@id"].Value);
		var freecycle_step_dict_id = (int?)(e.Command.Parameters["@freecycle_step_dict_id"].Value);
		var prozoro_number = (string)(e.Command.Parameters["@prozoro_number"].Value);
		var current_stage_docdate = (DateTime?)(e.Command.Parameters["@current_stage_docdate"].Value);
		var current_stage_docnum = (string)(e.Command.Parameters["@current_stage_docnum"].Value);
		var current_step = CabinetUtils222.GetStep(free_square_id);
		var change_step = (freecycle_step_dict_id != current_step);
		//var freecycle_step_date = (DateTime?)(e.Command.Parameters["@freecycle_step_date"].Value);

		if (freecycle_step_dict_id == null)
		{
			dbparams.AddWithValue("@freecycle_step_date", null);
			dbparams["@winner_id"].Value = null;
			dbparams["@prozoro_number"].Value = null;
			dbparams["@current_stage_docdate"].Value = null;
			dbparams["@current_stage_docnum"].Value = null;
		}
		else if (change_step)
		{
			dbparams.AddWithValue("@freecycle_step_date", DateTime.Today);
		}
		else
		{
			dbparams.AddWithValue("@freecycle_step_date", DateTime.Today);
		}

		CabinetUtils222.ValidateProzoroNumber(freecycle_step_dict_id, prozoro_number);
		CabinetUtils222.ValidateDocdateDocnum(freecycle_step_dict_id, current_stage_docnum, current_stage_docdate);

		if (change_step && freecycle_step_dict_id == 200300)
		{
			using (var connection = Utils.ConnectToDatabase())
			using (var transaction = connection.BeginTransaction())
			{
				CabinetUtils222.SendEmail(connection, transaction, free_square_id, "Учасників поінформовано");
				transaction.Commit();
			}
		}
		else if (change_step && freecycle_step_dict_id == 200820)
		{
			using (var connection = Utils.ConnectToDatabase())
			using (var transaction = connection.BeginTransaction())
			{
				CabinetUtils222.SendEmail(connection, transaction, free_square_id, "Розпочати процедуру підписання");
				transaction.Commit();
			}
		}
		else if (change_step && freecycle_step_dict_id == 200900)
		{
			using (var connection = Utils.ConnectToDatabase())
			using (var transaction = connection.BeginTransaction())
			{
				var additionalData = new Dictionary<string, object>();
				additionalData.Add("current_stage_docnum", current_stage_docnum);
				additionalData.Add("current_stage_docdate", current_stage_docdate);
				CabinetUtils222.SendEmail(connection, transaction, free_square_id, "Договір зареєстровано", additionalData: additionalData);
				transaction.Commit();
			}
		}
		else if (change_step && freecycle_step_dict_id == null)
		{
			using (var connection = Utils.ConnectToDatabase())
			using (var transaction = connection.BeginTransaction())
			{
				CabinetUtils222.ResetStage(free_square_id, connection, transaction);
				transaction.Commit();
			}
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

	protected void ButtonProzoroPrint_Click(object sender, EventArgs e)
	{
		var ids = FreeSquareGridView.GetSelectedFieldValues("id").Select(q => (int)q).ToArray();
		//if (!ids.Any())
		//{
			//throw new Exception("Не вибрано жодного запису");
		//}

		var builder = new ZvitProzoroBuilder
		{
			Page = this,
			Ids = ids,
		};
		builder.Go();
	}



	protected void FreeSquareGridView_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
	{
		if (e.Column.FieldName == "winner_id")
		{
			var combo = (ASPxComboBox)e.Editor;

			var grid = e.Column.Grid;
			if (!combo.IsCallback)
			{
				var id = (int)grid.GetRowValues(e.VisibleIndex, "id");
				combo.DataSourceID = "SqlDataSourceWinners";
				SqlDataSourceWinners.SelectParameters["free_square_id"].DefaultValue = id.ToString();
				combo.DataBindItems();
			}
		}
	}

	protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
	{
		using (var connection = Utils.ConnectToDatabase())
		using (var transaction = connection.BeginTransaction())
		{
			var free_square_id = Int32.Parse(Upload_Adogvor_info["free_square_id"].ToString());
			var mode = Upload_Adogvor_info["mode"].ToString();
			var zipdata = e.UploadedFile.FileBytes;

			var text = CabinetUtils222.ProcUploadAdogvor(zipdata, free_square_id, mode, DateTime.Now, connection, transaction);
			transaction.Commit();

			e.ErrorText = text;
		}
	}

	protected void FreeSquareGridView_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
	{
		//System.Diagnostics.Debug.WriteLine("e.ButtonID=" + e.ButtonID);

		if (e.ButtonID == "bnt_adogovor_balansoderzhatel_1" || e.ButtonID == "bnt_adogovor_balansoderzhatel_2")
		{
			var show_1 = false;
			var show_2 = false;
			var isCabinetBalansoderzhatel = (int?)FreeSquareGridView.GetRowValues(e.VisibleIndex, "isCabinetBalansoderzhatel");
			var winner_id = FreeSquareGridView.GetRowValues(e.VisibleIndex, "winner_id");
			var freecycle_step_dict_id = FreeSquareGridView.GetRowValues(e.VisibleIndex, "freecycle_step_dict_id");
			var cabinetOrendarStage = (string)FreeSquareGridView.GetRowValues(e.VisibleIndex, "cabinetOrendarStage");
			if (isCabinetBalansoderzhatel == 1 && object.Equals(freecycle_step_dict_id,200820))
			{
				if (winner_id == System.DBNull.Value)
				{
					show_2 = true;
				}
				else
				{
					show_1 = true;
				}
			}

			if (e.ButtonID == "bnt_adogovor_balansoderzhatel_1" && !show_1)
			{
				e.Visible = DevExpress.Utils.DefaultBoolean.False;
			}
			else if (e.ButtonID == "bnt_adogovor_balansoderzhatel_2" && !show_2)
			{
				e.Visible = DevExpress.Utils.DefaultBoolean.False;
			}
		}

		else if (e.ButtonID == "bnt_adogovor_orendar")
		{
			e.Visible = DevExpress.Utils.DefaultBoolean.False;
		}

		else if (e.ButtonID == "bnt_adogovor_orendodavecz")
		{
			var show = false;
			var isCabinetOrendodavecz = (int?)FreeSquareGridView.GetRowValues(e.VisibleIndex, "isCabinetOrendodavecz");
			var cabinetOrendarStage = (string)FreeSquareGridView.GetRowValues(e.VisibleIndex, "cabinetOrendarStage");
			if (isCabinetOrendodavecz == 1 && cabinetOrendarStage == "podpis_balansoderzhatel_orendar")
			{
				show = true;
			}

			if (!show)
			{
				e.Visible = DevExpress.Utils.DefaultBoolean.False;
			}
		}
	}
}



public static class CabinetUtils222
{
	const string BALANSODERZHATEL = "balansoderzhatel";
	const string ORENDAR = "orendar";
	const string ORENDODAVECZ = "orendodavecz";

	public static string ProcUploadAdogvor(byte[] zipdata, int free_square_id, string mode, DateTime date, SqlConnection connection, SqlTransaction transaction)
	{
		//System.Threading.Thread.Sleep(5000);
		var zipdir = ExtractZipFile(zipdata);
		var allfiles = Directory.GetFiles(zipdir);
		var files = allfiles.Where(x => !Path.GetFileName(x).EndsWith("_Validation_Report.pdf", StringComparison.OrdinalIgnoreCase)).ToArray();

		var errortext = "Невірний склад архіву";
		if (files.Length != 2)
		{
			throw new Exception(errortext);
		}

		var file_1 = files[0].Length < files[1].Length ? files[0] : files[1];
		var file_2 = files[0].Length < files[1].Length ? files[1] : files[0];
		if (string.Compare(file_1 + ".xml", file_2, StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw new Exception(errortext);
		}


		var body_old = GetAdogvorBody(free_square_id, connection, transaction);
		var body_new = File.ReadAllBytes(file_1);
		var filename = Path.GetFileName(file_1);
		var podpis = File.ReadAllBytes(file_2);
		if (mode == BALANSODERZHATEL)
		{
			if (!IsEqual(body_new, body_old))
			{
				AdogvorReset(free_square_id, connection, transaction);
				AdogvorSetBody(free_square_id, body_new, filename, connection, transaction);
			}
		}
		else
		{
			if (!IsEqual(body_new, body_old))
			{
				throw new Exception("Договір усередині архіву відрізняється від завантаженого балансоутримувачем");
			}
		}

		AdogvorSetPodpis(free_square_id, podpis, mode, connection, transaction);

		int new_freecycle_step_dict_id;
		if (mode == BALANSODERZHATEL)	new_freecycle_step_dict_id = 200840;  //Договір знаходиться на підпису орендаря
		else if (mode == ORENDAR)		new_freecycle_step_dict_id = 200870;  //Договір знаходиться на підпису орендодавця
		else if (mode == ORENDODAVECZ)	new_freecycle_step_dict_id = 200880;  //Договір підписано
		else throw new Exception();

		ChangeStage(free_square_id, new_freecycle_step_dict_id, connection, transaction);

		AdogvorSendEmail(free_square_id, mode, connection, transaction);

		ClearCatalog(zipdir);

		if (mode == BALANSODERZHATEL) return "Підписаний документ завантажено успішно";
		else if (mode == ORENDAR) return "Підпис завантажено успішно";
		else if (mode == ORENDODAVECZ) return "Підпис завантажено успішно";
		return "";

	}


	static void AdogvorSendEmail(int free_square_id, string mode, SqlConnection connection, SqlTransaction transaction)
	{
		var sendToUserIds = new List<string>();
		var template = "";
		var subject = "";
		string[] podpises;

		if (mode == BALANSODERZHATEL)
		{
			sendToUserIds.Add(GetAuctionWinnerUserId(free_square_id, connection, transaction));
			template = "Договір потрібно підписати -- Орендарю";
			subject = "Пропонуємо підписати договір";
			podpises = new[] { BALANSODERZHATEL };
		}
		else if (mode == ORENDAR)
		{
			sendToUserIds.AddRange(GetOrendodavecUserIds(free_square_id, connection, transaction));
			template = "Договір потрібно підписати -- Орендодавцю";

			podpises = new[] { BALANSODERZHATEL, ORENDAR };
		}
		else if (mode == ORENDODAVECZ)
		{
			sendToUserIds.Add(GetAuctionWinnerUserId(free_square_id, connection, transaction));
			sendToUserIds.AddRange(GetOrendodavecUserIds(free_square_id, connection, transaction));
			sendToUserIds.AddRange(GetAllBalansoderzhatels(free_square_id, connection, transaction));
			template = "Договір потрібно підписати -- Всім";
			subject = "Надсилаємо оригінал договору";
			podpises = new[] { BALANSODERZHATEL, ORENDAR, ORENDODAVECZ };
		}
		else throw new Exception();

		var attachments = new List<MailAttachment>();
		var filename = GetAdogvorBodyName(free_square_id, connection, transaction);
		var body = GetAdogvorBody(free_square_id, connection, transaction);
		attachments.Add(new MailAttachment(filename, body));

		foreach (var podpistype in podpises)
		{
			var podisfilename = filename + "." + Mode2PodpisFile(podpistype) + ".xml";
			var podis = GetAdogvorPodpis(free_square_id, podpistype, connection, transaction);
			attachments.Add(new MailAttachment(podisfilename, podis));
		}

		SendEmail(connection, transaction, free_square_id, template, attachments: attachments, explicit_userIds: sendToUserIds, explicit_subject: subject);
	}

	static string Mode2Name(string mode)
	{
		switch (mode)
		{
			case ORENDAR:
				return "Орендар";
			case ORENDODAVECZ:
				return "Орендодавць";
			case BALANSODERZHATEL:
				return "Балансоутримувач";
		}
		return "";
	}

	static string Mode2PodpisFile(string mode)
	{
		switch (mode)
		{
			case ORENDAR:
				return "підпис орендаря";
			case ORENDODAVECZ:
				return "підпис орендодавця";
			case BALANSODERZHATEL:
				return "підпис балансоутримувача";
		}
		return "";
	}

	static void AdogvorSetPodpis(int free_square_id, byte[] podpis, string mode, SqlConnection connection, SqlTransaction transaction)
	{
		var podpis_date = DateTime.Now;
		var sql = @"update [reports1nf_balans_free_square] set 
				[adogovor_podpis_balansoderzhatel] = @podpis,
				[adogovor_podpis_balansoderzhatel_date] = @podpis_date
			 where [id] = @free_square_id".Replace("podpis_balansoderzhatel", "podpis_" + mode);

		using (var cmd = new SqlCommand(sql, connection, transaction))
		{
			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
			cmd.Parameters.Add(GetSqlParameter("podpis", podpis));
			cmd.Parameters.Add(GetSqlParameter("podpis_date", podpis_date));
			var rowUpdated = cmd.ExecuteNonQuery();
			if (rowUpdated != 1) throw new Exception("update error");
		}
	}

	static void AdogvorSetBody(int free_square_id, byte[] body, string body_name, SqlConnection connection, SqlTransaction transaction)
	{
		var load_date = DateTime.Now;

		using (var cmd = new SqlCommand(
			@"update [reports1nf_balans_free_square] set 
				[adogovor_body] = @body,
				[adogovor_filename] = @body_name,
				[adogovor_load_date] = @load_date
			 where [id] = @free_square_id", connection, transaction))
		{
			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
			cmd.Parameters.Add(GetSqlParameter("body", body));
			cmd.Parameters.Add(GetSqlParameter("body_name", body_name));
			cmd.Parameters.Add(GetSqlParameter("load_date", load_date));
			var rowUpdated = cmd.ExecuteNonQuery();
			if (rowUpdated != 1) throw new Exception("update error");
		}
	}

	public static void ResetStage(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		AdogvorReset(free_square_id, connection, transaction);

		using (var cmd = new SqlCommand(@"update [reports1nf_balans_free_square] set [winner_id] = null, [prozoro_number] = null where [id] = @free_square_id", connection, transaction))
		{
			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
			var rowUpdated = cmd.ExecuteNonQuery();
		}

		using (var cmd = new SqlCommand(@"delete from [auction_uchasnik] where [free_square_id] = @free_square_id", connection, transaction))
		{
			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
			var rowUpdated = cmd.ExecuteNonQuery();
		}
	}

	static void AdogvorReset(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		using (var cmd = new SqlCommand(
			@"update [reports1nf_balans_free_square] set 
				[adogovor_body] = null,
				[adogovor_load_date] = null,
				[adogovor_podpis_balansoderzhatel] = null,
				[adogovor_podpis_balansoderzhatel_date] = null,
				[adogovor_podpis_orendar] = null,
				[adogovor_podpis_orendar_date] = null,
				[adogovor_podpis_orendodavecz] = null,
				[adogovor_podpis_orendodavecz_date] = null,
				[adogovor_filename] = null
			 where [id] = @free_square_id", connection, transaction))
		{
			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
			var rowUpdated = cmd.ExecuteNonQuery();
			if (rowUpdated != 1) throw new Exception("update error");
		}
	}

	static string ExtractZipFile(byte[] zipdata)
	{
		var tempFile = Path.GetTempFileName();
		var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		File.WriteAllBytes(tempFile, zipdata);

		using (var archive = ZipArchive.Read(tempFile))
		{
			foreach (ZipItem item in archive)
			{
				item.Extract(tempDir);
			}
		}

		try
		{
			File.Delete(tempFile);
		}
		catch (Exception) { }

		return tempDir;
	}

	static byte[] GetAdogvorBody(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		byte[] result = null;
		var data = GetDataTable("select adogovor_body from reports1nf_balans_free_square where id = " + dd(free_square_id), connection, transaction);
		if (data.Rows.Count > 0)
		{
			var val = data.Rows[0]["adogovor_body"];
			result = (val == System.DBNull.Value ? null : (byte[])val);
		}
		return result;
	}

	static byte[] GetAdogvorPodpis(int free_square_id, string mode, SqlConnection connection, SqlTransaction transaction)
	{
		byte[] result = null;
		var sql = ("select adogovor_podpis_orendar as podpis from reports1nf_balans_free_square where id = " + dd(free_square_id)).Replace("podpis_orendar", "podpis_" + mode);
		var data = GetDataTable(sql, connection, transaction);
		if (data.Rows.Count > 0)
		{
			var val = data.Rows[0]["podpis"];
			result = (val == System.DBNull.Value ? null : (byte[])val);
		}
		return result;
	}

	static string GetAdogvorBodyName(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		string result = "";
		var data = GetDataTable("select adogovor_filename from reports1nf_balans_free_square where id = " + dd(free_square_id), connection, transaction);
		if (data.Rows.Count > 0)
		{
			result = (data.Rows[0]["adogovor_filename"] ?? "").ToString();
		}
		return result;
	}

	public static void AddUchasnik(int free_square_id, string userid, DateTime zayavkaDate, SqlConnection connection, SqlTransaction transaction)
	{
		var username = Utils.GetUser();

		using (var cmd = new SqlCommand(
			@"insert into [auction_uchasnik]([free_square_id], [UserId], [zayavka_date], modify_date, modified_by) 
									values(@free_square_id, @userid, @zayavka_date, @modify_date, @modified_by)", connection, transaction))
		{
			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
			cmd.Parameters.Add(GetSqlParameter("userid", userid));
			cmd.Parameters.Add(GetSqlParameter("zayavka_date", zayavkaDate));
			cmd.Parameters.Add(GetSqlParameter("modify_date", zayavkaDate));
			cmd.Parameters.Add(GetSqlParameter("modified_by", username));
			var rowUpdated = cmd.ExecuteNonQuery();
			if (rowUpdated != 1) throw new Exception("update error");
		}
	}

	static bool IsEqual(byte[] arr_1, byte[] arr_2)
	{
		if (arr_1 == null && arr_2 == null)
		{
			return true;
		}
		else if (arr_1 == null || arr_2 == null)
		{
			return false;
		}
		else
		{
			return arr_1.SequenceEqual(arr_2);
		}
	}

	static void ClearCatalog(string dir)
	{
		try
		{
			Directory.Delete(dir, true);
		}
		catch (Exception) { }
	}


	public static void ChangeStage(int free_square_id, int stage_id, SqlConnection connection, SqlTransaction transaction)
	{
		var username = Utils.GetUser();
		var userid = Utils.GetUserId();
		var modify_date = DateTime.Now;
		var today = DateTime.Today;

		using (var cmd = new SqlCommand(
					@"update reports1nf_balans_free_square set [freecycle_step_dict_id] = @stage_id, freecycle_step_date = @today where id = @free_square_id", connection, transaction))
		{
			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
			cmd.Parameters.Add(GetSqlParameter("stage_id", stage_id));
			cmd.Parameters.Add(GetSqlParameter("today", today));
			var rowUpdated = cmd.ExecuteNonQuery();
			if (rowUpdated != 1) throw new Exception("update error");
		}
	}


	private static SqlParameter GetSqlParameter(string parameterName, object value)
	{
		if (value == null)
		{
			return new SqlParameter(parameterName, DBNull.Value);
		}
		else
		{
			return new SqlParameter(parameterName, value);
		}
	}

	private static DataTable GetDataTable(string sql, SqlConnection connection, SqlTransaction transaction)
	{
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			cmd.Transaction = transaction;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		return dataTable;
	}

	public static string[] GetOrendodavecUserIds(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var result = new List<string>();
		var data = GetDataTable("select distinct A.UserId from [aspnet_Membership] A join [aspnet_Users] B on B.UserId = A.UserId where IsCabinetOrendodavecz = 1", connection, transaction);
		for (var rownum = 0; rownum < data.Rows.Count; rownum++)
		{
			result.Add((data.Rows[rownum]["UserId"] ?? "").ToString());
		}
		return result.ToArray();
	}

	public static string[] GetAllOrendars(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var result = new List<string>();
		var data = GetDataTable("select distinct UserId from auction_uchasnik where free_square_id = " + dd(free_square_id) + " and is_arhiv = 0", connection, transaction);
		for (var rownum = 0; rownum < data.Rows.Count; rownum++)
		{
			result.Add((data.Rows[rownum]["UserId"] ?? "").ToString());
		}
		return result.ToArray();
	}

	public static string[] GetAllBalansoderzhatels(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var userId = GetBalansoderzhatelUserId(free_square_id, connection, transaction);
		return new[] { userId };
	}

	public static string[] RdaZkpo2UserIds(string balans_zkpo, SqlConnection connection, SqlTransaction transaction)
	{
		var result = new List<string>();
		var data = GetDataTable("select distinct A.UserId from [aspnet_Membership] A join [aspnet_Users] B on B.UserId = A.UserId where CabinetBalansoderzhatelZkpo = " + dd(balans_zkpo), connection, transaction);
		for (var rownum = 0; rownum < data.Rows.Count; rownum++)
		{
			result.Add((data.Rows[rownum]["UserId"] ?? "").ToString());
		}
		return result.ToArray();
	}

	public static string[] RdaZkpo2UserIds___(string balans_zkpo, SqlConnection connection, SqlTransaction transaction)
	{
		var result = new List<string>();
		var data = GetDataTable(@"
						SELECT usr.UserName, org.zkpo_code, usr.UserId
						FROM reports1nf_accounts acc 
						JOIN aspnet_Users usr ON usr.UserId = acc.UserId
						JOIN reports1nf_org_info org ON org.id = acc.organization_id
						WHERE org.zkpo_code = " + dd(balans_zkpo), connection, transaction);
		for (var rownum = 0; rownum < data.Rows.Count; rownum++)
		{
			var userId = (data.Rows[rownum]["UserId"] ?? "").ToString();
			result.Add(userId);
		}
		return result.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
	}

	public static string GetAuctionWinnerUserId(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var result = "";
		var data = GetDataTable(@"
				select distinct A.UserId 
				from auction_uchasnik A join reports1nf_balans_free_square B on B.id = A.free_square_id and B.winner_id = A.id
				where A.free_square_id = " + dd(free_square_id) + " and is_arhiv = 0", connection, transaction);
		for (var rownum = 0; rownum < data.Rows.Count; rownum++)
		{
			result = (data.Rows[rownum]["UserId"] ?? "").ToString();
		}
		return result;
	}

	public static string GetBalansoderzhatelZkpo(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var result = "";
		var data = GetDataTable(@"select balans_zkpo from reports1nf_balans_free_square_view A where A.free_square_id = " + dd(free_square_id), connection, transaction);
		for (var rownum = 0; rownum < data.Rows.Count; rownum++)
		{
			result = (data.Rows[rownum]["balans_zkpo"] ?? "").ToString();
		}
		return result;
	}

	public static string GetBalansoderzhatelUserId(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var result = "";
		var data = GetDataTable(@"select B.UserId from reports1nf_balans_free_square A join aspnet_Users B on A.modified_by = B.UserName where A.id = " + dd(free_square_id), connection, transaction);
		for (var rownum = 0; rownum < data.Rows.Count; rownum++)
		{
			result = (data.Rows[rownum]["UserId"] ?? "").ToString();
		}
		return result;
	}

	public static string GetEmail(string userId, SqlConnection connection, SqlTransaction transaction)
	{
		var email = "";
		var data = GetDataTable("select email from aspnet_Membership where UserId = " + dd(userId), connection, transaction);
		if (data.Rows.Count > 0)
		{
			email = (data.Rows[0]["email"] ?? "").ToString();
		}
		return email;
	}

	public static string GetUserFio(string userId, SqlConnection connection, SqlTransaction transaction)
	{
		var fio = "";
		var data = GetDataTable("select ltrim(ltrim(CONCAT(namef,' ',namei,' ',nameo))) fio from aspnet_Membership where UserId = " + dd(userId), connection, transaction);
		if (data.Rows.Count > 0)
		{
			fio = (data.Rows[0]["fio"] ?? "").ToString();
		}
		return fio;
	}

	public static string GetObjectDescription(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var result = "";
		var sql = @"select
fs.id
,b.district
,b.street_full_name as street_name
,(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')) as addr_nomer
FROM view_reports1nf rep
join reports1nf_balans bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.organization_id
left join [dbo].[dict_streets] st on b.addr_street_id = st.id
left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id
where fs.id = " + dd(free_square_id);

		var data = GetDataTable(sql, connection, transaction);
		if (data.Rows.Count > 0)
		{
			result =
				//(data.Rows[0]["district"] ?? "").ToString() + ", " +
				"м.Київ" + ", " +
				(data.Rows[0]["street_name"] ?? "").ToString() + ", " +
				(data.Rows[0]["addr_nomer"] ?? "").ToString() + " (реєстраційний № " + (data.Rows[0]["id"] ?? "").ToString() + ")"
				;
		}
		return result;
	}

	public static string GetProzoroNumber(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var result = "";
		var sql = @"select prozoro_number from reports1nf_balans_free_square fs where fs.id = " + dd(free_square_id);
		var data = GetDataTable(sql, connection, transaction);
		if (data.Rows.Count > 0)
		{
			result = (data.Rows[0]["prozoro_number"] ?? "").ToString();
		}
		return result;
	}

	public static string GetStageDoc(Dictionary<string, object> additionalData)
	{
		var result = "";
		var current_stage_docnum = "";
		var current_stage_docdate = default(DateTime?);
		if (additionalData.ContainsKey("current_stage_docnum"))
		{
			current_stage_docnum = (string)additionalData["current_stage_docnum"];
		}
		if (additionalData.ContainsKey("current_stage_docdate"))
		{
			current_stage_docdate = (DateTime?)additionalData["current_stage_docdate"];
		}


		if (current_stage_docdate != null && !string.IsNullOrEmpty(current_stage_docnum))
		{
			result = "№ " + current_stage_docnum + " від " +  current_stage_docdate.Value.ToString("dd.MM.yyyy");
		}

		return result;
	}

	public static string GetInstructions(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var result = @"
Електронний підпис. Інструкція: https://dkv.kyivcity.gov.ua/Static/sign.htm
Електронний підпис: https://czo.gov.ua/sign
Перевірка електронного підпису: https://czo.gov.ua/verify
";

		return result;
	}


	public static int? GetStep(int free_square_id)
	{
		using (var connection = Utils.ConnectToDatabase())
		{
			var result = (int?)null;
			var sql = @"select freecycle_step_dict_id from reports1nf_balans_free_square fs where fs.id = " + dd(free_square_id);
			var data = GetDataTable(sql, connection, null);
			if (data.Rows.Count > 0)
			{
				result = GetData<int>(data.Rows[0]["freecycle_step_dict_id"]);
			}
			return result;
		}
	}

	static T? GetData<T>(object data) where T: struct
	{
		if (data == System.DBNull.Value)
		{
			return null;
		}
		else
		{
			return (T)data;
		}
	}

	public static string GetProzoroNumberLink(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var num = GetProzoroNumber(free_square_id, connection, transaction);
		var result = @"https://prozorro.sale/auction/" + num;
		return result;
	}

	private static string dd(object arg)
	{
		if (arg == null) return "null";
		else if (arg is string) return "'" + arg.ToString().Replace("'", "''") + "'";
		else return arg.ToString();
	}

	public static void ValidateProzoroNumber(int? freecycle_step_dict_id, string prozoro_number)
	{
		if (new int?[] { 200200, 200300, 200400, 200500, 200600, 200700, 200800, 200820, 200840, 200860, 200870, 200880, 200900 }.Contains(freecycle_step_dict_id))
		{
			if (string.IsNullOrEmpty((prozoro_number ?? "").Trim()))
			{
				throw new Exception("Необхідно заповнити поле \"Унікальний код обєкту у ЕТС Прозорро-продажі\"");
			}
		}
	}

	public static void ValidateDocdateDocnum(int? freecycle_step_dict_id, string current_stage_docnum, DateTime? current_stage_docdate)
	{
		if (new int?[] { 200900 }.Contains(freecycle_step_dict_id))
		{
			if (string.IsNullOrEmpty((current_stage_docnum ?? "").Trim()))
			{
				throw new Exception("Необхідно заповнити поле \"№ документа\"");
			}
			if (current_stage_docdate == null)
			{
				throw new Exception("Необхідно заповнити поле \"Дата документа\"");
			}
		}
	}

	public static void SendEmail(SqlConnection connection, SqlTransaction transaction, int free_square_id, string emailtype,
		string orendarUserId = null, DateTime? zayavkaDate = null, IEnumerable<MailAttachment> attachments = null,
		IEnumerable<string> explicit_userIds = null, string explicit_subject = null, Dictionary<string, object> additionalData = null)
	{
		string[] userIds = null;
		string subject = "";

		if (emailtype == "Заявка подана -- Орендодавцю")
		{
			userIds = GetOrendodavecUserIds(free_square_id, connection, transaction);
			subject = "Заявка подана";
		}
		else if (emailtype == "Заявка подана -- Орендарю")
		{
			userIds = new[] { orendarUserId };
			subject = "Заявка подана";
		}
		else if (emailtype == "Учасників поінформовано")
		{
			var userIds1 = GetAllOrendars(free_square_id, connection, transaction);
			var userIds2 = GetAllBalansoderzhatels(free_square_id, connection, transaction);
			userIds = userIds1.Union(userIds2).Distinct().ToArray();
			subject = "Інформація про оголошення аукціону";
		}
		else if (emailtype == "Розпочати процедуру підписання")
		{
			userIds = GetAllBalansoderzhatels(free_square_id, connection, transaction);
			subject = "Пропонуємо розпочати процедуру підписання договору";
		}
		else if (emailtype == "Договір зареєстровано")
		{
			var userIds1 = GetAllOrendars(free_square_id, connection, transaction);
			var userIds2 = GetAllBalansoderzhatels(free_square_id, connection, transaction);
			userIds = userIds1.Union(userIds2).Distinct().ToArray();
			subject = "Договір зареєстровано";
		}

		if (explicit_userIds != null)
		{
			userIds = explicit_userIds.ToArray();
		}
		if (explicit_subject != null)
		{
			subject = explicit_subject;
		}



		//var emails = userIds.Select(q => GetEmail(q, connection, transaction)).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();

		foreach (var userId in userIds)
		{
			var email = GetEmail(userId, connection, transaction);
			if (string.IsNullOrEmpty(email)) continue;

			var fio = GetUserFio(userId, connection, transaction);

			var template = Path.Combine(WebConfigurationManager.AppSettings["Cabinet.Templates"], emailtype + ".txt");
			var text = File.ReadAllText(template, Encoding.GetEncoding(1251));

			ReplaceText(ref text, "{{ZAYAVKA_DATETIME}}", () => zayavkaDate.Value.ToString("dd.MM.yyyy HH:mm"));
			ReplaceText(ref text, "{{OBJECT_DECSRIPTION}}", () => GetObjectDescription(free_square_id, connection, transaction));
			ReplaceText(ref text, "{{AUCTION_LINK}}", () => GetProzoroNumberLink(free_square_id, connection, transaction));
			ReplaceText(ref text, "{{USER_FIO}}", () => fio);
			ReplaceText(ref text, "{{STAGE_DOC}}", () => GetStageDoc(additionalData));
			ReplaceText(ref text, "{{INSTRUCTIONS}}", () => GetInstructions(free_square_id, connection, transaction));

			

			SendMailMessage(email, "", "", subject, text, attachments0: attachments);
		}
	}

	private static void ReplaceText(ref string text, string field, Func<string> valueFunc)
	{
		if (text.Contains(field))
		{
			text = text.Replace(field, valueFunc());
		}
	}

	public class MailAttachment
	{
		public string FileName { get; set; }
		public byte[] Bytes { get; set; }

		public MailAttachment(string fileName, byte[] bytes)
		{
			this.FileName = fileName;
			this.Bytes = bytes;
		}
	}

	static byte[] ZipFiles(Dictionary<string, byte[]> files)
	{
		var result = default(byte[]);
		var tempFile = Path.GetTempFileName();
		var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		Directory.CreateDirectory(tempDir);

		using (ZipArchive archive = new ZipArchive())
		{
			foreach (var file in files)
			{
				var tempfile = Path.Combine(tempDir, file.Key);
				File.WriteAllBytes(tempfile, file.Value);
				archive.AddFile(tempfile, "\\");
			}

			archive.Save(tempFile);
			result = File.ReadAllBytes(tempFile);
		}

		try
		{
			File.Delete(tempFile);
			Directory.Delete(tempDir, true);
		}
		catch (Exception) { }

		return result;
	}

	private static void SendMailMessage(string to, string cc, string bcc, string subject, string body, IEnumerable<MailAttachment> attachments0 = null)
	{
		MailAttachment[] attachments = null;
		if (attachments0 != null && attachments0.Any())
		{
			var attachinfo = attachments0.ToDictionary(x => x.FileName, x => x.Bytes);
			var zipattach = ZipFiles(attachinfo);
			var zipname = "Документ_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".zip";
			attachments = new[] { new MailAttachment(zipname, zipattach) };
		}

		if (WebConfigurationManager.AppSettings["Cabinet.SendEmail.Log"] == "1")
		{
			var logfolder = WebConfigurationManager.AppSettings["Cabinet.SendEmail.Log.Folder"];
			var logidmail = Guid.NewGuid().ToString();
			var logemail = Path.Combine(logfolder, logidmail + ".txt");
			var logtxt =
				"To:" + to + "\n" +
				"Subject:" + subject + "\n" +
				"Body:" + body + "\n" +
				"";
			File.WriteAllText(logemail, logtxt);

			if (attachments != null)
			{
				var attachdir = Path.Combine(logfolder, logidmail);
				Directory.CreateDirectory(attachdir);
				foreach (var attachment in attachments)
				{
					File.WriteAllBytes(Path.Combine(attachdir, attachment.FileName), attachment.Bytes);
				}
			}
		}

		if (WebConfigurationManager.AppSettings["Cabinet.SendEmail"] != "1")
		{
			return;
		}

		var from = WebConfigurationManager.AppSettings["Cabinet.EmailFrom"];
		if (string.IsNullOrEmpty(from))
		{
			return;
		}

		if (WebConfigurationManager.AppSettings["Cabinet.UseDebugEmail"] == "1")
		{
			var debugEmail = WebConfigurationManager.AppSettings["Cabinet.DebugEmail"];
			
			body += body 
				+ "\n\n\n\n\n" 
				+ "-------------------------------------------------------------------\n"
				+ "to: " + to + "\n"
				+ "cc: " + cc + "\n"
				+ "bcc: " + bcc + "\n"
				+ "-------------------------------------------------------------------\n"
				;

			to = debugEmail;
			cc = null;
			bcc = null;
		}

		if (WebConfigurationManager.AppSettings["Cabinet.ReplaceEmail"] == "1")
		{
			to = to;

			if (to == "test1@google.com") to = "dimad77772@yandex.ru";

			else if (to == "aistspfu@gmail.com") to = "dimad77771@gmail.com";
			else if (to == "katya2000pavlyuk@gmail.com") to = "dimad77771@gmail.com";

			else if (to == "seic@gukv.gov.ua") to = "torbanaruche@mail.com";
			else if (to == "knmc2@ukr.net") to = "torbanaruche@mail.com";

			else throw new Exception();
		}

		bool isTest = false;

		body = body.Replace("\r", "");
		body = body.Replace("\n", "<br/>");

		// Instantiate a new instance of MailMessage
		MailMessage mMailMessage = new MailMessage();
		mMailMessage.IsBodyHtml = true;

		// Set the sender address of the mail message
		mMailMessage.From = new MailAddress(from);
		// Set the recepient address of the mail message
		mMailMessage.To.Add(new MailAddress(to));
		//mMailMessage.To.Add(new MailAddress("ILazarieva@itgukraine.com"));
		//mMailMessage.Bcc.Add(new MailAddress("pul@ukr.net"));
		//mMailMessage.Bcc.Add(new MailAddress("pul@yandex.com"));

		// Check if the cc value is null or an empty value
		if ((cc != null) && (cc != string.Empty))
		{
			// Set the CC address of the mail message
			mMailMessage.CC.Add(new MailAddress(cc));
		}

		// Check if the bcc value is null or an empty string
		if ((bcc != null) && (bcc != string.Empty))
		{
			// Set the Bcc address of the mail message
			mMailMessage.Bcc.Add(new MailAddress(bcc));
		}

		// Set the subject of the mail message
		mMailMessage.Subject = subject;
		if (isTest)
			mMailMessage.Subject = "ТЕСТОВАЯ ВЕРСИЯ " + mMailMessage.Subject;
		// Set the body of the mail message

		mMailMessage.Body = body;
		if (isTest)
			mMailMessage.Body = "ТЕСТОВАЯ ВЕРСИЯ. ЕСЛИ ВЫ НЕ ЯВЛЯЕТЕСЬ ТЕСТИРОВЩИКОМ, ПРОСТО ИГНОРИРУЙТЕ ДАННОЕ ПИСЬМО<BR/>" + mMailMessage.Body;

		if (attachments != null)
		{
			foreach (var attachment in attachments)
			{
				mMailMessage.Attachments.Add(new Attachment(new MemoryStream(attachment.Bytes), attachment.FileName));
			}
		}

		// Set the priority of the mail message to normal
		mMailMessage.Priority = MailPriority.Normal;

		System.Net.ServicePointManager.ServerCertificateValidationCallback = (x, y, z, t) => true;

		// Instantiate a new instance of SmtpClient
		SmtpClient mSmtpClient = new SmtpClient();

		// Send the mail message
		mSmtpClient.Send(mMailMessage);
	}

}