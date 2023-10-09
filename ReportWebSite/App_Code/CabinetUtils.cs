//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Reflection;
//using System.Web.UI.WebControls;
//using System.Web.Configuration;
//using System.Configuration;
//using DevExpress.Web.Export;
//using DevExpress.Web;
//using DevExpress.Web.ASPxTreeList;
//using DevExpress.Web.ASPxHtmlEditor;
//using DevExpress.Data.Filtering;
//using System.IO;
//using System.Text;
//using System.Web.Security;

//using System.Data;
//using System.Data.SqlClient;

//using System.Net.Mail;
//using FirebirdSql.Data.FirebirdClient;

//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;

//using Cache;
//using log4net;
//using System.Collections;
//using System.Text.RegularExpressions;
//using System.Threading;
//using GUKV.Common;
//using System.Data.Common;

//public static class CabinetUtils
//{
//    public static void AddUchasnik(int free_square_id, string userid, SqlConnection connection, SqlTransaction transaction)
//    {
//        var username = Utils.GetUser();
//        var modify_date = DateTime.Now;

//		using (var cmd = new SqlCommand(
//			@"insert into [auction_uchasnik]([free_square_id], [UserId], [zayavka_date], modify_date, modified_by) 
//									values(@free_square_id, @userid, @zayavka_date, @modify_date, @modified_by)", connection, transaction))
//		{
//			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
//			cmd.Parameters.Add(GetSqlParameter("userid", userid));
//			cmd.Parameters.Add(GetSqlParameter("zayavka_date", modify_date));
//			cmd.Parameters.Add(GetSqlParameter("modify_date", modify_date));
//			cmd.Parameters.Add(GetSqlParameter("modified_by", username));
//			var rowUpdated = cmd.ExecuteNonQuery();
//			if (rowUpdated != 1) throw new Exception("update error");
//		}
//	}

//	public static void ChangeStage(int free_square_id, int stage_id, SqlConnection connection, SqlTransaction transaction)
//	{
//		var username = Utils.GetUser();
//		var userid = Utils.GetUserId();
//		var modify_date = DateTime.Now;

//		using (var cmd = new SqlCommand(
//										@"update reports1nf_balans_free_square set [freecycle_step_dict_id] = @stage_id where id = @free_square_id", connection, transaction))
//		{
//			cmd.Parameters.Add(GetSqlParameter("free_square_id", free_square_id));
//			cmd.Parameters.Add(GetSqlParameter("stage_id", stage_id));
//			var rowUpdated = cmd.ExecuteNonQuery();
//			if (rowUpdated != 1) throw new Exception("update error");
//		}
//	}


//	private static SqlParameter GetSqlParameter(string parameterName, object value)
//	{
//		if (value == null)
//		{
//			return new SqlParameter(parameterName, DBNull.Value);
//		}
//		else
//		{
//			return new SqlParameter(parameterName, value);
//		}
//	}

//	private static DataTable GetDataTable(string sql, SqlConnection connection)
//	{
//		var factory = DbProviderFactories.GetFactory(connection);
//		var dataTable = new DataTable();
//		using (var cmd = factory.CreateCommand())
//		{
//			cmd.CommandText = sql;
//			cmd.CommandType = CommandType.Text;
//			cmd.Connection = connection;
//			using (var adapter = factory.CreateDataAdapter())
//			{
//				adapter.SelectCommand = cmd;
//				adapter.Fill(dataTable);
//			}
//		}

//		return dataTable;
//	}

//	public static string GetOrendodavecUserId()
//	{
//		return WebConfigurationManager.AppSettings["Cabinet.OrendodavecUserId"];
//	}

//	public static string GetOrendarUserId(int free_square_id, SqlConnection connection)
//	{
//		//GetDataTable("select distinct UserId auction_uchasnik 

//		return WebConfigurationManager.AppSettings["Cabinet.OrendodavecUserId"];
//	}

//	public static string GetEmail(string userId, SqlConnection connection)
//	{
//		var email = "";
//		var data = GetDataTable("select email from aspnet_Membership where UserId = " + dd(userId), connection);
//		if (data.Rows.Count > 0)
//		{
//			email = (data.Rows[0]["email"] ?? "").ToString();
//		}
//		return email;
//	}

//	public static string GetObjectDescription(int free_square_id, SqlConnection connection)
//	{
//		var result = "";
//		var sql = @"select
//fs.id
//,b.district
//,b.street_full_name as street_name
//,(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')) as addr_nomer
//FROM view_reports1nf rep
//join reports1nf_balans bal on bal.report_id = rep.report_id
//JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
//join dbo.reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = rep.report_id
//join reports1nf_org_info org on org.id = bal.organization_id
//left join [dbo].[dict_streets] st on b.addr_street_id = st.id
//left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
//left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id
//where fs.id = " + dd(free_square_id);

//		var data = GetDataTable(sql, connection);
//		if (data.Rows.Count > 0)
//		{
//			result = 
//				(data.Rows[0]["district"] ?? "").ToString() + ", " +
//				(data.Rows[0]["street_name"] ?? "").ToString() + ", " +
//				(data.Rows[0]["addr_nomer"] ?? "").ToString() + " (реєстраційний № " + (data.Rows[0]["id"] ?? "").ToString() + ")"
//				;
//		}
//		return result;
//	}

//	public static string dd(object arg)
//	{
//		if (arg == null) return "null";
//		else if (arg is string) return "'" + arg.ToString().Replace("'", "''") + "'";
//		else return arg.ToString();
//	}

//	public static void SendEmail(SqlConnection connection, int free_square_id, string emailtype, string orendarUserId = null, DateTime? zayavkaDate = null)
//	{
//		string[] userIds;
//		string subject;

//		if (emailtype == "Заявка подана -- Орендодавцю")
//		{
//			userIds = new[] { GetOrendodavecUserId() };
//			subject = "Заявка подана";
//		}
//		else if (emailtype == "Заявка подана -- Орендарю")
//		{
//			userIds = new[] { orendarUserId };
//			subject = "Заявка подана";
//		}
//		else throw new Exception();

//		string template = Path.Combine(WebConfigurationManager.AppSettings["Cabinet.Templates"], emailtype + ".txt");
//		string text = File.ReadAllText(template);

//		ReplaceText(ref text, "{{ZAYAVKA_DATETIME}}", () => zayavkaDate.Value.ToString("dd.MM.yyyy HH:mm"));
//		ReplaceText(ref text, "{{OBJECT_DECSRIPTION}}", () => GetObjectDescription(free_square_id, connection));

//		var emails = userIds.Select(q => GetEmail(q, connection)).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();

//		foreach(var email in emails)
//		{
//			SendMailMessage(email, "", "", subject, text);
//		}
//	}

//	private static void ReplaceText(ref string text, string field, Func<string> valueFunc)
//	{
//		if (text.Contains(field))
//		{
//			text = text.Replace(field, valueFunc());
//		}
//	}


//	private static void SendMailMessage(string to, string cc, string bcc, string subject, string body)
//	{
//		if (WebConfigurationManager.AppSettings["Cabinet.SendEmail"] != "1")
//		{
//			return;
//		}

//		var from = WebConfigurationManager.AppSettings["Cabinet.EmailFrom"];
//		if (string.IsNullOrEmpty(from))
//		{
//			return;
//		}

//		bool isTest = false;

//		body = body.Replace("\r", "");
//		body = body.Replace("\n", "<br/>");

//		// Instantiate a new instance of MailMessage
//		MailMessage mMailMessage = new MailMessage();
//		mMailMessage.IsBodyHtml = true;

//		// Set the sender address of the mail message
//		mMailMessage.From = new MailAddress(from);
//		// Set the recepient address of the mail message
//		mMailMessage.To.Add(new MailAddress(to));
//		//mMailMessage.To.Add(new MailAddress("ILazarieva@itgukraine.com"));
//		//mMailMessage.Bcc.Add(new MailAddress("pul@ukr.net"));
//		//mMailMessage.Bcc.Add(new MailAddress("pul@yandex.com"));

//		// Check if the cc value is null or an empty value
//		if ((cc != null) && (cc != string.Empty))
//		{
//			// Set the CC address of the mail message
//			mMailMessage.CC.Add(new MailAddress(cc));
//		}

//		// Check if the bcc value is null or an empty string
//		if ((bcc != null) && (bcc != string.Empty))
//		{
//			// Set the Bcc address of the mail message
//			mMailMessage.Bcc.Add(new MailAddress(bcc));
//		}

//		// Set the subject of the mail message
//		mMailMessage.Subject = subject;
//		if (isTest)
//			mMailMessage.Subject = "ТЕСТОВАЯ ВЕРСИЯ " + mMailMessage.Subject;
//		// Set the body of the mail message

//		mMailMessage.Body = body;
//		if (isTest)
//			mMailMessage.Body = "ТЕСТОВАЯ ВЕРСИЯ. ЕСЛИ ВЫ НЕ ЯВЛЯЕТЕСЬ ТЕСТИРОВЩИКОМ, ПРОСТО ИГНОРИРУЙТЕ ДАННОЕ ПИСЬМО<BR/>" + mMailMessage.Body;

//		// Set the priority of the mail message to normal
//		mMailMessage.Priority = MailPriority.Normal;

//		System.Net.ServicePointManager.ServerCertificateValidationCallback = (x, y, z, t) => true;

//		// Instantiate a new instance of SmtpClient
//		SmtpClient mSmtpClient = new SmtpClient();

//		// Send the mail message
//		mSmtpClient.Send(mMailMessage);
//	}

//}
