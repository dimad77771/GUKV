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
}


public static class CabinetUtils
{
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

	public static void ChangeStage(int free_square_id, int stage_id, SqlConnection connection, SqlTransaction transaction)
	{
		var username = Utils.GetUser();
		var userid = Utils.GetUserId();
		var modify_date = DateTime.Now;

		using (var cmd = new SqlCommand(
										@"update reports1nf_balans_free_square set [freecycle_step_dict_id] = @stage_id where id = @free_square_id", connection, transaction))
		{
			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
			cmd.Parameters.Add(GetSqlParameter("stage_id", stage_id));
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

	public static string GetOrendodavecUserId()
	{
		return WebConfigurationManager.AppSettings["Cabinet.OrendodavecUserId"];
	}

	public static string GetOrendarUserId(int free_square_id, SqlConnection connection)
	{
		//GetDataTable("select distinct UserId auction_uchasnik 

		return WebConfigurationManager.AppSettings["Cabinet.OrendodavecUserId"];
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
				(data.Rows[0]["district"] ?? "").ToString() + ", " +
				(data.Rows[0]["street_name"] ?? "").ToString() + ", " +
				(data.Rows[0]["addr_nomer"] ?? "").ToString() + " (реєстраційний № " + (data.Rows[0]["id"] ?? "").ToString() + ")"
				;
		}
		return result;
	}

	public static string dd(object arg)
	{
		if (arg == null) return "null";
		else if (arg is string) return "'" + arg.ToString().Replace("'", "''") + "'";
		else return arg.ToString();
	}

	public static void SendEmail(SqlConnection connection, SqlTransaction transaction, int free_square_id, string emailtype, string orendarUserId = null, DateTime? zayavkaDate = null)
	{
		string[] userIds;
		string subject;

		if (emailtype == "Заявка подана -- Орендодавцю")
		{
			userIds = new[] { GetOrendodavecUserId() };
			subject = "Заявка подана";
		}
		else if (emailtype == "Заявка подана -- Орендарю")
		{
			userIds = new[] { orendarUserId };
			subject = "Заявка подана";
		}
		else throw new Exception();

		string template = Path.Combine(WebConfigurationManager.AppSettings["Cabinet.Templates"], emailtype + ".txt");
		string text = File.ReadAllText(template, Encoding.GetEncoding(1251));

		ReplaceText(ref text, "{{ZAYAVKA_DATETIME}}", () => zayavkaDate.Value.ToString("dd.MM.yyyy HH:mm"));
		ReplaceText(ref text, "{{OBJECT_DECSRIPTION}}", () => GetObjectDescription(free_square_id, connection, transaction));

		var emails = userIds.Select(q => GetEmail(q, connection, transaction)).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();

		foreach (var email in emails)
		{
			SendMailMessage(email, "", "", subject, text);
		}
	}

	private static void ReplaceText(ref string text, string field, Func<string> valueFunc)
	{
		if (text.Contains(field))
		{
			text = text.Replace(field, valueFunc());
		}
	}


	private static void SendMailMessage(string to, string cc, string bcc, string subject, string body)
	{
		if (WebConfigurationManager.AppSettings["Cabinet.SendEmail.Log"] == "1")
		{
			var logemail = Path.Combine(WebConfigurationManager.AppSettings["Cabinet.SendEmail.Log.Folder"], Guid.NewGuid().ToString() + ".txt");
			var logtxt = 
				"To:" + to + "\n" +
				"Subject:" + subject + "\n" +
				"Body:" + body + "\n" +
				"";
			File.WriteAllText(logemail, logtxt);
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

		// Set the priority of the mail message to normal
		mMailMessage.Priority = MailPriority.Normal;

		System.Net.ServicePointManager.ServerCertificateValidationCallback = (x, y, z, t) => true;

		// Instantiate a new instance of SmtpClient
		SmtpClient mSmtpClient = new SmtpClient();

		// Send the mail message
		mSmtpClient.Send(mMailMessage);
	}

}