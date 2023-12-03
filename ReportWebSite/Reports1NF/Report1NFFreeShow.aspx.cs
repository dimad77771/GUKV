using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Syncfusion.XlsIO;
using DevExpress.Compression;


public partial class Reports1NF_Report1NFFreeShow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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

		if (Utils.NoUseCabinet)
		{
			FreeSquareGridView.Columns["btn_send"].Visible = false;
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
		if ((e.Parameters ?? "").StartsWith("auctionZayavka:"))
		{
			var free_square_id = int.Parse(e.Parameters.Replace("auctionZayavka:",""));

			using(var connection = Utils.ConnectToDatabase())
			using (var transaction = connection.BeginTransaction())
			{
				var orendarUserId = Utils.GetUserId();
				var zayavkaDate = DateTime.Now;


				CabinetUtils.AddUchasnik(free_square_id, orendarUserId, zayavkaDate, connection, transaction);
				CabinetUtils.ChangeStage(free_square_id, 200100, connection, transaction);

				CabinetUtils.SendEmail(connection, transaction, free_square_id, "Заявка подана -- Орендодавцю", zayavkaDate: zayavkaDate);
				CabinetUtils.SendEmail(connection, transaction, free_square_id, "Заявка подана -- Орендарю", orendarUserId: orendarUserId);

				transaction.Commit();
			}

			FreeSquareGridView.DataBind();
		}

		else if ((e.Parameters ?? "").StartsWith("auctionOtkaz:"))
		{
			var free_square_id = int.Parse(e.Parameters.Replace("auctionOtkaz:", ""));

			using (var connection = Utils.ConnectToDatabase())
			using (var transaction = connection.BeginTransaction())
			{
				var orendarUserId = Utils.GetUserId();
				var zayavkaDate = DateTime.Now;

				CabinetUtils.ChangeStage(free_square_id, 200860, connection, transaction);

				//CabinetUtils.SendEmail(connection, transaction, free_square_id, "Заявка подана -- Орендодавцю", zayavkaDate: zayavkaDate);
				//CabinetUtils.SendEmail(connection, transaction, free_square_id, "Заявка подана -- Орендарю", orendarUserId: orendarUserId);

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

	SqlParameter GetSqlParameter(string parameterName, object value)
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

	protected void GridViewFreeSquare_CustomFilterExpressionDisplayText(object sender, DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, FreeSquareGridView);
    }

    protected void GridViewFreeSquare_ProcessColumnAutoFilter(object sender, DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
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
		e.Command.Parameters["@userId"].Value = Utils.GetUserId();
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

	protected void ZayavkaRichEdit_Callback(object sender, CallbackEventArgsBase e)
	{
		var EditedDocuemntID = Guid.NewGuid().ToString();
		var data = File.ReadAllBytes(@"D:\Projects\DKVSOURCESFINALEDITION_v20\ReportWebSite\Reports1NF\Templates\ProzoroDogovor.docx");
		ZayavkaRichEdit.Open(
					EditedDocuemntID,
					DevExpress.XtraRichEdit.DocumentFormat.OpenXml,
					() => { return data; }
				);
	}

	protected void ZayavkaRichEdit_Init(object sender, EventArgs e)
	{
		ZayavkaRichEdit.CreateDefaultRibbonTabs(true);
		ZayavkaRichEdit.RibbonTabs.RemoveAt(0);
	}

	protected bool IsConnected
	{
		get
		{
			var user = Membership.GetUser();
			return (user != null);
		}
	}

	protected void RegisterUser_CreatedUser(object sender, EventArgs e)
	{
		FormsAuthentication.SetAuthCookie(RegisterUser.UserName, true);
	}

	protected void AuctionZayavkaBtn_Init(object sender, EventArgs e)
	{
		var button = sender as ASPxButton;
		var container = ((ASPxButton)sender).NamingContainer as GridViewDataItemTemplateContainer;
		button.ClientSideEvents.Click = String.Format("function (s, e) {{ AuctionZayavkaClick(s, e, {0}); }}", container.VisibleIndex);
	}

	protected void AuctionZayavkaLoadPodpisBtn_Init(object sender, EventArgs e)
	{
		var button = sender as ASPxButton;
		var container = ((ASPxButton)sender).NamingContainer as GridViewDataItemTemplateContainer;
		button.ClientSideEvents.Click = String.Format("function (s, e) {{ UploadAdogvorClick(s, e, {0}); }}", container.VisibleIndex);
	}

	protected void AuctionZayavkaOtkazBtn_Init(object sender, EventArgs e)
	{
		var button = sender as ASPxButton;
		var container = ((ASPxButton)sender).NamingContainer as GridViewDataItemTemplateContainer;
		button.ClientSideEvents.Click = String.Format("function (s, e) {{ AuctionOtkazClick(s, e, {0}); }}", container.VisibleIndex);
	}

	protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
	{
		using (var connection = Utils.ConnectToDatabase())
		using (var transaction = connection.BeginTransaction())
		{
			var free_square_id = Int32.Parse(Upload_Adogvor_info["free_square_id"].ToString());
			var mode = Upload_Adogvor_info["mode"].ToString();
			var zipdata = e.UploadedFile.FileBytes;

			var text = CabinetUtils.ProcUploadAdogvor(zipdata, free_square_id, mode, DateTime.Now, connection, transaction);
			transaction.Commit();

			//FreeSquareGridView.DataBind();

			e.ErrorText = text;
		}
	}
}


public static class CabinetUtils
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
		if (mode == BALANSODERZHATEL) new_freecycle_step_dict_id = 200840;  //Договір знаходиться на підпису орендаря
		else if (mode == ORENDAR) new_freecycle_step_dict_id = 200870;  //Договір знаходиться на підпису орендодавця
		else if (mode == ORENDODAVECZ) new_freecycle_step_dict_id = 200880;  //Договір підписано
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
			subject = "?";
			podpises = new[] { BALANSODERZHATEL };
		}
		else if (mode == ORENDAR)
		{
			sendToUserIds.AddRange(GetOrendodavecUserIds(connection, transaction));
			template = "Договір потрібно підписати -- Орендодавцю";

			podpises = new[] { BALANSODERZHATEL, ORENDAR };
		}
		else if (mode == ORENDODAVECZ)
		{
			sendToUserIds.Add(GetAuctionWinnerUserId(free_square_id, connection, transaction));
			sendToUserIds.AddRange(GetOrendodavecUserIds(connection, transaction));
			sendToUserIds.AddRange(GetAllBalansoderzhatels(free_square_id, connection, transaction));
			template = "Договір потрібно підписати -- Всім";
			subject = "?";
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

	public static string[] GetOrendodavecUserIds(SqlConnection connection, SqlTransaction transaction)
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
		var balans_zkpo = GetBalansoderzhatelZkpo(free_square_id, connection, transaction);
		var userIds = RdaZkpo2UserIds(balans_zkpo, connection, transaction);
		return userIds;
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

	public static string GetObjectDescription(int free_square_id, SqlConnection connection, SqlTransaction transaction)
	{
		var result = "";
		var sql = @"select
fs.id
,b.district
,b.street_full_name as street_name
,(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')) as addr_nomer
,fs.total_free_sqr
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
			var total_free_sqr = (decimal)data.Rows[0]["total_free_sqr"];

			result =
				//(data.Rows[0]["district"] ?? "").ToString().Trim() + ", " +
				"м.Київ" + ", " +
				(data.Rows[0]["street_name"] ?? "").ToString().Trim() + ", " +
				(data.Rows[0]["addr_nomer"] ?? "").ToString().Trim() + ", " +
				"загальною площею " + total_free_sqr.ToString("0.00") + " кв.м., " +
				"регістраційний номер " + (data.Rows[0]["id"] ?? "").ToString()
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

	public static int? GetStep(int free_square_id)
	{
		using (var connection = Utils.ConnectToDatabase())
		{
			var result = (int?)null;
			var sql = @"select freecycle_step_dict_id from reports1nf_balans_free_square fs where fs.id = " + dd(free_square_id);
			var data = GetDataTable(sql, connection, null);
			if (data.Rows.Count > 0)
			{
				result = (int?)data.Rows[0]["freecycle_step_dict_id"];
			}
			return result;
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

	public static void SendEmail(SqlConnection connection, SqlTransaction transaction, int free_square_id, string emailtype,
		string orendarUserId = null, DateTime? zayavkaDate = null, IEnumerable<MailAttachment> attachments = null,
		IEnumerable<string> explicit_userIds = null, string explicit_subject = null)
	{
		string[] userIds = null;
		string subject = "";

		if (emailtype == "Заявка подана -- Орендодавцю")
		{
			userIds = GetOrendodavecUserIds(connection, transaction);
			subject = "Заявка оренди приміщення";
		}
		else if (emailtype == "Заявка подана -- Орендарю")
		{
			//userIds = new[] { orendarUserId };
			userIds = new string[0];
			subject = "Заявка оренди приміщення";
		}
		else if (emailtype == "Учасників поінформовано")
		{
			userIds = GetAllOrendars(free_square_id, connection, transaction);
			subject = "Інформація про оголошення аукціону";
		}

		if (explicit_userIds != null)
		{
			userIds = explicit_userIds.ToArray();
		}
		if (explicit_subject != null)
		{
			subject = explicit_subject;
		}


		string template = Path.Combine(WebConfigurationManager.AppSettings["Cabinet.Templates"], emailtype + ".txt");
		string text = File.ReadAllText(template, Encoding.GetEncoding(1251));

		ReplaceText(ref text, "{{ZAYAVKA_DATETIME}}", () => zayavkaDate.Value.ToString("dd.MM.yyyy HH:mm"));
		ReplaceText(ref text, "{{OBJECT_DECSRIPTION}}", () => GetObjectDescription(free_square_id, connection, transaction));
		ReplaceText(ref text, "{{AUCTION_LINK}}", () => GetProzoroNumberLink(free_square_id, connection, transaction));

		var emails = userIds.Select(q => GetEmail(q, connection, transaction)).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();

		foreach (var email in emails)
		{
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

		{
			if (to == "test1@google.com") to = "dimad77772@yandex.ru";
			else if (to == "aistspfu@gmail.com") to = "dimad77771@gmail.com";
			else if (to == "seic@gukv.gov.ua") to = "torbanaruche@mail.com";
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