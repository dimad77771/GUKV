using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.Cryptography;
using System.Text;
using DevExpress.Web;

public partial class Reports1NF_Cabinet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];

        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);
        }

        Utils.FindUserOrganizationAndRda("");

        if (ReportID <= 0)
        {
            GetLastReportId();
        }

        // Check if report belongs to this user
        int reportRdaDistrictId = -1;
        int reportOrganizationId = Reports1NFUtils.GetReportOrganizationId(ReportID, ref reportRdaDistrictId);

        if (ReportID > 0)
        {
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

            for (int i = 0; i < SectionMenu.Items.Count; i++)
            {
                if (SectionMenu.Items[i].NavigateUrl.IndexOf('?') < 0)
                    SectionMenu.Items[i].NavigateUrl += "?rid=" + ReportID.ToString();
            }

            if (LinkOrgInfo.NavigateUrl.IndexOf('?') < 0)
                LinkOrgInfo.NavigateUrl += "?rid=" + ReportID.ToString();

            if (LinkBalansObjects.NavigateUrl.IndexOf('?') < 0)
                LinkBalansObjects.NavigateUrl += "?rid=" + ReportID.ToString();

            if (LinkBalansDeleted.NavigateUrl.IndexOf('?') < 0)
                LinkBalansDeleted.NavigateUrl += "?rid=" + ReportID.ToString();

            if (LinkRentAgreements.NavigateUrl.IndexOf('?') < 0)
                LinkRentAgreements.NavigateUrl += "?rid=" + ReportID.ToString();

            if (LinkRentedObjects.NavigateUrl.IndexOf('?') < 0)
                LinkRentedObjects.NavigateUrl += "?rid=" + ReportID.ToString();
        }

        // Hide the Cabinet buttons if user is not a report submitter
        //if (Roles.IsUserInRole(Utils.Report1NFReviewerRole))
        //{
        //    ButtonVerify.ClientVisible = true;
        //}

        if (!Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
        {
            ButtonPrint.ClientVisible = false;
            ButtonLogout.ClientVisible = false;
			ButtonSendReport.ClientVisible = false;

			if (SectionMenu.Items.Count > 6)
            {
                SectionMenu.Items[6].ClientVisible = false;
            }
        }
    }

    protected void SqlDataSourceReport_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@rep_id"].Value = ReportID;
    }

	protected int UserOrganizationID
	{
		get
		{
			return Utils.UserOrganizationID;
		}
	}

	protected void ObjectDataSourcePhotoFiles_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
	{
		if (Request.Cookies["RecordID"] != null)
			e.InputParameters["RecordID"] = Request.Cookies["RecordID"].Value;
	}

	protected void ObjectDataSourcePhotoFiles_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
	{
		if (e.Exception != null) return;

		var connection = Utils.ConnectToDatabase();
		var transaction = connection.BeginTransaction();
		var stan_recieve_date = DateTime.Today;

		using (var cmd = new SqlCommand(
			@"update [reports1nf] 
				set [stan_recieve_id] = @stan_recieve_id, [stan_recieve_date] = @stan_recieve_date
				where [organization_id] = @organization_id", connection, transaction))
		{
			cmd.Parameters.Add(GetSqlParameter("stan_recieve_id", 2));
			cmd.Parameters.Add(GetSqlParameter("stan_recieve_date", stan_recieve_date));
			cmd.Parameters.Add(GetSqlParameter("organization_id", UserOrganizationID));
			var rowUpdated = cmd.ExecuteNonQuery();
			if (rowUpdated != 1) throw new Exception("Cabinet update error");
		}

		transaction.Commit();
		connection.Close();
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


	#region Report initialization

	protected void GetLastReportId()
    {
        int userOrganizationId = Utils.UserOrganizationID;

        if (userOrganizationId > 0)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // Get the maximum report ID for this organization
                int reportId = -1;

                using (SqlCommand cmd = new SqlCommand("SELECT MAX(id) FROM reports1nf WHERE organization_id = @org_id", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("org_id", userOrganizationId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                reportId = reader.GetInt32(0);
                            }
                        }

                        reader.Close();
                    }
                }

                // If no report exists, create it
                if (reportId < 0)
                {
                    reportId = GenerateDefaultReport(connection, userOrganizationId);
                }

                // Save the report ID
                ReportID = reportId;

                connection.Close();
            }
        }
    }

    protected int GenerateDefaultReport(SqlConnection connection, int organizationId)
    {
        // Generate the first report for this organization by executing the stored procedure
        SqlParameter outputIdParam = new SqlParameter("REPORT_ID", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        using (SqlCommand cmd = new SqlCommand("fnGenerateInitial1NFReport", connection))
        {
            cmd.Parameters.Add(new SqlParameter("ORG_ID", organizationId));
            cmd.Parameters.Add(outputIdParam);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
        }

        if (outputIdParam.Value is int)
        {
            return (int)outputIdParam.Value;
        }

        return -1;
    }

    #endregion (Report initialization)

    protected int ReportID
    {
        get
        {
            object reportId = ViewState["REPORT_ID_1NF"];

            if (reportId is int)
            {
                return (int)reportId;
            }

            return -1;
        }

        set
        {
            ViewState["REPORT_ID_1NF"] = value;
        }
    }

    protected object GetMaxDate(object date1, object date2)
    {
        if (date1 is DateTime)
        {
            return (date2 is DateTime) ? ((DateTime)date2 > (DateTime)date1 ? date2 : date1) : date1;
        }
        else
        {
            return (date2 is DateTime) ? date2 : System.DBNull.Value;
        }
    }

    protected void ButtonLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect(Page.ResolveClientUrl("~/Account/Logout.aspx"));
    }

    //protected void ButtonVerify_Click(object sender, EventArgs e)
    //{
    //    // If user is a Report Viewer, mark the report as 'viewed'
    //    if (Roles.IsUserInRole(Utils.Report1NFReviewerRole) && ReportID > 0)
    //    {
    //        SqlConnection connection = Utils.ConnectToDatabase();

    //        if (connection != null)
    //        {
    //            using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf SET is_reviewed = 1 WHERE id = @rid", connection))
    //            {
    //                cmd.Parameters.Add(new SqlParameter("rid", ReportID));
    //                cmd.ExecuteNonQuery();
    //            }

    //            connection.Close();

    //            ReportStatus.DataBind();
    //        }
    //    }
    //}

    #region Export to Word

    protected void ButtonPrint_Click(object sender, EventArgs e)
    {
		//var connection = Utils.ConnectToDatabase();
		//var cur_state = "";
		//using (var cmd = new SqlCommand(@"select cur_state from view_reports1nf where report_id = @rep_id", connection))
		//{
		//	cmd.Parameters.Add(GetSqlParameter("rep_id", ReportID));
		//	cur_state = (string)cmd.ExecuteScalar();
		//}
		//connection.Close();
		//if (cur_state == "Не надісланий")
		//{
		//	throw new Exception("Поточний стан актуализации данных \"Не надісланий\". Звіт не може бути сформований");
		//}

		string templateFileName = Server.MapPath("Templates/Report.docx");

        if (templateFileName.Length > 0)
        {
            using (TempFile tempFile = TempFile.FromExistingFile(templateFileName))
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(tempFile.FileName, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;

                    if (mainPart != null)
                    {
                        UpdateTemplateFile(mainPart);
                    }

                    SaltedHash sh = new SaltedHash("123");
                    DocumentSettingsPart docSett = wordDocument.MainDocumentPart.DocumentSettingsPart;
                    DocumentProtection documentProtection = new DocumentProtection();
                    documentProtection.Edit = DocumentProtectionValues.ReadOnly;
                    OnOffValue docProtection = new OnOffValue(true);
                    documentProtection.Enforcement = docProtection;

                    documentProtection.CryptographicAlgorithmClass = CryptAlgorithmClassValues.Hash;
                    documentProtection.CryptographicProviderType = CryptProviderValues.RsaFull;
                    documentProtection.CryptographicAlgorithmType = CryptAlgorithmValues.TypeAny;
                    documentProtection.CryptographicAlgorithmSid = 4; // SHA1
                    //    The iteration count is unsigned
                    //UInt32Value uintVal = new UInt32Value();
                    //uintVal.Value = (uint)123;
                    documentProtection.CryptographicSpinCount = 1;
                    documentProtection.Hash = sh.Hash;
                    documentProtection.Salt = sh.Salt;
                    wordDocument.MainDocumentPart.DocumentSettingsPart.Settings.AppendChild(documentProtection);
                    wordDocument.MainDocumentPart.DocumentSettingsPart.Settings.Save();

                    wordDocument.Close();

                    // Dump the document contents to the output stream
                    System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    Response.AddHeader("content-disposition", "attachment; filename=Report.docx; size=" + info.Length.ToString());

                    // Pipe the stream contents to the output stream
                    using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName,
                        System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
                    {
                        stream.CopyTo(Response.OutputStream);
                    }
                }

                Response.End();
            }
        }
    }

    protected void UpdateTemplateFile(MainDocumentPart mainPart)
    {
        Dictionary<string, string> properties = new Dictionary<string, string>();

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            decimal otherRentPayments = 0m;

            GetReportDates(connection, properties);
            GetObjectAndRentTotals(connection, properties);
            GetOrganizationProperties(connection, properties, ref otherRentPayments);
            GetRentPaymentProperties(connection, properties, otherRentPayments);

            connection.Close();
        }

        foreach (KeyValuePair<string, string> pair in properties)
        {
            ReplaceDocTagInElements(mainPart, pair.Key, pair.Value);
        }
    }

    protected void GetReportDates(SqlConnection connection, Dictionary<string, string> properties)
    {
        // Format the current date
        DateTime dtNow = DateTime.Now;

        string currentDate = "\xAB" + " " + dtNow.Day.ToString() + " " + "\xBB" + " " + GetDateMonthName(dtNow) + " " + dtNow.Year.ToString();

        properties.Add("{REPORT_PRINT_DATE}", currentDate);

        // Format the last submit date
        string query = @"SELECT
	        (SELECT MAX(org.submit_date) FROM reports1nf_org_info org WHERE org.report_id = r.id) AS 'org_date',
	        (SELECT MAX(bal.submit_date) FROM reports1nf_balans bal WHERE bal.report_id = r.id AND (bal.is_deleted IS NULL OR bal.is_deleted = 0)) AS 'bal_date',
	        (SELECT MAX(bd.submit_date) FROM reports1nf_balans_deleted bd WHERE bd.report_id = r.id) AS 'bal_deleted_date',
	        (SELECT MAX(ar.submit_date) FROM reports1nf_arenda ar WHERE ar.report_id = r.id AND (ar.is_deleted IS NULL OR ar.is_deleted = 0)) AS 'arenda_date',
	        (SELECT MAX(arr.submit_date) FROM reports1nf_arenda_rented arr WHERE arr.report_id = r.id AND (arr.is_deleted IS NULL OR arr.is_deleted = 0)) AS 'arenda_rented_date'
        FROM reports1nf r WHERE r.id = @rep";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("rep", ReportID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    object submitDate1 = reader.IsDBNull(0) ? null : reader.GetValue(0);
                    object submitDate2 = reader.IsDBNull(1) ? null : reader.GetValue(1);
                    object submitDate3 = reader.IsDBNull(2) ? null : reader.GetValue(2);
                    object submitDate4 = reader.IsDBNull(3) ? null : reader.GetValue(3);
                    object submitDate5 = reader.IsDBNull(4) ? null : reader.GetValue(4);

                    submitDate1 = SelectMaxDate(submitDate1, submitDate2);
                    submitDate1 = SelectMaxDate(submitDate1, submitDate3);
                    submitDate1 = SelectMaxDate(submitDate1, submitDate4);
                    submitDate1 = SelectMaxDate(submitDate1, submitDate5);

                    if (submitDate1 is DateTime)
                    {
                        DateTime dtSubmit = (DateTime)submitDate1;

                        string submitDate = "\xAB" + " " + dtSubmit.Day.ToString() + " " + "\xBB" + " " + GetDateMonthName(dtSubmit) + " " + dtSubmit.Year.ToString();

                        properties.Add("{REPORT_INFO_DATE}", submitDate);
                    }
                    else
                    {
                        properties.Add("{REPORT_INFO_DATE}", Resources.Strings.ReportNotSubmitted);
                    }
                }

                reader.Close();
            }
        }
    }

    protected object SelectMaxDate(object date1, object date2)
    {
        if (date1 == null)
            return date2;

        if (date2 == null)
            return date1;

        DateTime dt1 = (DateTime)date1;
        DateTime dt2 = (DateTime)date2;

        return dt1 > dt2 ? dt1 : dt2;
    }

    protected void GetOrganizationProperties(SqlConnection connection, Dictionary<string, string> properties, ref decimal otherRentPayments)
    {
        // Get the organization properties
        string query = @"SELECT
            [org].[full_name]
           ,[org].[short_name]
           ,[org].[zkpo_code]
           ,[dict_org_industry].[name] AS 'industry'
           ,[dict_org_occupation].[name] AS 'occupation'
           ,[dict_org_status].[name] AS 'status'
           ,[dict_org_form_gosp].[name] AS 'form_gosp'
           ,[dict_org_ownership].[name] AS 'form_of_ownership'
           ,[dict_org_gosp_struct].[name] AS 'gosp_struct'
           ,[dict_org_vedomstvo].[name] AS 'vedomstvo'
           ,[dict_org_form].[name] AS 'org_form'
           ,[dict_org_gosp_struct_type].[name] AS 'gosp_struct_type'
           ,[dict_org_sfera_upr].[name] AS 'sfera_upr'
           ,[dict_org_old_organ].[name] AS 'old_organ'
           ,[director_fio]
           ,[director_phone]
           ,[director_email]
           ,[director_title]
           ,[kved_code]
           ,jr_street.name AS 'addr_street'
           ,org.addr_nomer
           ,org.addr_misc
           ,ph_street.name AS 'phys_addr_street'
           ,org.phys_addr_nomer
           ,org.phys_addr_misc
           ,org.contribution_rate
           ,org.buhgalter_fio
           ,org.buhgalter_phone
           ,org.buhgalter_email
           ,org.budget_narah_50_uah

           --,org.budget_zvit_50_uah
           ,dbo.[get_kazna_total]([org].[zkpo_code],null,null)

           ,org.budget_prev_50_uah
           ,org.budget_debt_30_50_uah
           ,org.payment_budget_special
           ,org.konkurs_payments
           ,org.unknown_payments
        FROM
            reports1nf_org_info org
            LEFT OUTER JOIN dict_org_industry ON org.industry_id = dict_org_industry.id
            LEFT OUTER JOIN dict_org_occupation ON org.occupation_id = dict_org_occupation.id
            LEFT OUTER JOIN dict_org_status ON org.status_id = dict_org_status.id
            LEFT OUTER JOIN dict_org_form_gosp ON org.form_gosp_id = dict_org_form_gosp.id
            LEFT OUTER JOIN dict_org_ownership ON org.form_ownership_id = dict_org_ownership.id
            LEFT OUTER JOIN dict_org_gosp_struct ON org.gosp_struct_id = dict_org_gosp_struct.id
            LEFT OUTER JOIN dict_org_gosp_struct_type ON org.gosp_struct_type_id = dict_org_gosp_struct_type.id
            LEFT OUTER JOIN dict_org_vedomstvo ON org.vedomstvo_id = dict_org_vedomstvo.id
            LEFT OUTER JOIN dict_org_form ON org.form_id = dict_org_form.id
            LEFT OUTER JOIN dict_org_sfera_upr ON org.sfera_upr_id = dict_org_sfera_upr.id
            LEFT OUTER JOIN dict_org_old_organ ON org.old_organ_id = dict_org_old_organ.id
            LEFT OUTER JOIN dict_streets jr_street ON org.addr_street_id = jr_street.id
            LEFT OUTER JOIN dict_streets ph_street ON org.phys_addr_street_id = ph_street.id
        WHERE
            org.report_id = @rep";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("rep", ReportID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {

                    properties.Add("{SUBMITTER_FULL_NAME}", reader.IsDBNull(0) ? "" : reader.GetString(0).Trim());
                    properties.Add("{SUBMITTER_SHORT_NAME}", reader.IsDBNull(1) ? "" : reader.GetString(1).Trim());
                    properties.Add("{SUBMITTER_ZKPO}", reader.IsDBNull(2) ? "" : reader.GetString(2).Trim());


                    string addrStreet = reader.IsDBNull(19) ? "" : reader.GetString(19).Trim();
                    string addrNumber = reader.IsDBNull(20) ? "" : reader.GetString(20).Trim();
                    string addrMisc = reader.IsDBNull(21) ? "" : reader.GetString(21).Trim();

                    string physAddrStreet = reader.IsDBNull(22) ? "" : reader.GetString(22).Trim();
                    string physAddrNumber = reader.IsDBNull(23) ? "" : reader.GetString(23).Trim();
                    string physAddrMisc = reader.IsDBNull(24) ? "" : reader.GetString(24).Trim();

                    if (addrNumber.Length > 0)
                        addrStreet += ", " + addrNumber;

                    if (addrMisc.Length > 0)
                        addrStreet += ", " + addrMisc;

                    if (physAddrNumber.Length > 0)
                        physAddrStreet += ", " + physAddrNumber;

                    if (physAddrMisc.Length > 0)
                        physAddrStreet += ", " + physAddrMisc;

                    properties.Add("{SUBMITTER_JR_ADDR}", addrStreet);
                    properties.Add("{SUBMITTER_PH_ADDR}", physAddrStreet);

                    properties.Add("{ORG_INDUSTRY}", reader.IsDBNull(3) ? "" : reader.GetString(3).Trim());
                    properties.Add("{ORG_OCCUPATION}", reader.IsDBNull(4) ? "" : reader.GetString(4).Trim());
                    properties.Add("{ORG_STATUS}", reader.IsDBNull(5) ? "" : reader.GetString(5).Trim());
                    properties.Add("{ORG_FIN_FORM}", reader.IsDBNull(6) ? "" : reader.GetString(6).Trim());
                    properties.Add("{ORG_OWNERSHIP}", reader.IsDBNull(7) ? "" : reader.GetString(7).Trim());

                    properties.Add("{ORG_VEDOMSTVO}", reader.IsDBNull(9) ? "" : reader.GetString(9).Trim());
                    properties.Add("{ORG_GOSP_FORM}", reader.IsDBNull(10) ? "" : reader.GetString(10).Trim());

                    properties.Add("{ORG_GOSP_UPR}", reader.IsDBNull(13) ? "" : reader.GetString(13).Trim());
                    properties.Add("{BOSS_FIO}", reader.IsDBNull(14) ? "" : reader.GetString(14).Trim());
                    properties.Add("{BOSS_TEL}", reader.IsDBNull(15) ? "" : reader.GetString(15).Trim());
                    properties.Add("{BOSS_EMAIL}", reader.IsDBNull(16) ? "" : reader.GetString(16).Trim());
                    properties.Add("{BOSS_POSADA}", reader.IsDBNull(17) ? "" : reader.GetString(17).Trim());
                    properties.Add("{ORG_KVED}", reader.IsDBNull(18) ? "" : reader.GetString(18).Trim());
                    properties.Add("{ORG_CONTRIB_RATE}", reader.IsDBNull(25) ? "50" : reader.GetInt32(25).ToString());

                    properties.Add("{USER_FIO}", reader.IsDBNull(26) ? "" : reader.GetString(26).Trim());
                    properties.Add("{USER_POSADA}", "");
                    properties.Add("{USER_TEL}", reader.IsDBNull(27) ? "" : reader.GetString(27).Trim());
                    properties.Add("{USER_EMAIL}", reader.IsDBNull(28) ? "" : reader.GetString(28).Trim());

                    decimal pay50narah = reader.IsDBNull(29) ? 0m : reader.GetDecimal(29);
                    decimal pay50zvit = reader.IsDBNull(30) ? 0m : reader.GetDecimal(30);
                    decimal pay50prev = reader.IsDBNull(31) ? 0m : reader.GetDecimal(31);
                    decimal payUnknown = reader.IsDBNull(35) ? 0m : reader.GetDecimal(35);
                    decimal pay50debt_cur = pay50narah - pay50zvit + pay50prev - payUnknown;

					
					properties.Add("{PAY_SALDO}", payUnknown > 0 ? payUnknown.ToString("F2") : "");
					properties.Add("{PAY_50_DEBT_CUR}", pay50debt_cur > 0 ? pay50debt_cur.ToString("F2") : "");

                    properties.Add("{PAY_50_NARAH}", reader.IsDBNull(29) ? "" : reader.GetDecimal(29).ToString("F2"));
//pgv                    properties.Add("{PAY_50_PAYED}", reader.IsDBNull(30) ? "" : reader.GetDecimal(30).ToString("F2"));
                    properties.Add("{PAY_50_DEBT}", reader.IsDBNull(31) ? "" : reader.GetDecimal(31).ToString("F2"));

                    //decimal pay50debt_old = reader.IsDBNull(32) ? 0m : reader.GetDecimal(32);
                    //pay50debt_old = pay50debt_old + pay50debt_cur;
                    properties.Add("{PAY_50_DEBT_OLD}", reader.IsDBNull(32) ? "" : reader.GetDecimal(32).ToString("F2"));
                    //properties.Add("{PAY_50_DEBT_OLD}", pay50debt_old > 0 ? pay50debt_old.ToString("F2") : "");
                    //properties.Add("{PAY_50_DEBT_OLD}", pay50debt_old.ToString("F2"));


                    properties.Add("{PAY_SPECIAL}", reader.IsDBNull(33) ? "" : reader.GetDecimal(33).ToString("F2"));
                    decimal payKonkurs = reader.IsDBNull(34) ? 0m : reader.GetDecimal(34);
//                    decimal payUnknown = reader.IsDBNull(35) ? 0m : reader.GetDecimal(35);

//pgv                 otherRentPayments = payUnknown + payKonkurs;
                    otherRentPayments = payKonkurs;
                    properties.Add("{PAY_RECV_OTHER}", otherRentPayments > 0 ? otherRentPayments.ToString("F2") : "");
                    //pay50zvit = pay50zvit + payUnknown;
                    //pay50zvit = pay50zvit;
                    properties.Add("{PAY_50_PAYED}", pay50zvit > 0 ? pay50zvit.ToString("F2") : "");
                }

                reader.Close();
            }
        }
    }

    protected void GetObjectAndRentTotals(SqlConnection connection, Dictionary<string, string> properties)
    {
        // Get the properties of balans objects
 /*       string query = @"SELECT
             SUM(bal.sqr_total) AS 'sqr_total'
            ,SUM(bal.sqr_kor) AS 'sqr_kor'
            ,SUM(CASE WHEN bal.is_free_sqr = 1 THEN bal.free_sqr_useful ELSE 0 END) AS 'sqr_free'
            ,COUNT(*) AS 'obj_count'
        FROM reports1nf_balans bal
        WHERE bal.report_id = @rep AND (bal.is_deleted IS NULL OR bal.is_deleted = 0)
        GROUP BY bal.report_id";
*/
        string query = @"SELECT
             SUM(bal.sqr_total) AS 'sqr_total'
            ,SUM(bal.sqr_kor) AS 'sqr_kor'
            ,sum(fs.sqr_free) as sqr_free
            ,COUNT(*) AS 'obj_count'
        FROM reports1nf_balans bal
	outer apply (select sum(fs.total_free_sqr) as 'sqr_free' from reports1nf_balans_free_square fs where fs.balans_id = bal.id and fs.report_id = bal.report_id and fs.is_included = 1) fs
        WHERE bal.report_id = @rep AND (bal.is_deleted IS NULL OR bal.is_deleted = 0)
        GROUP BY bal.report_id";


        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("rep", ReportID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    properties.Add("{SQR_TOTAL}", reader.IsDBNull(0) ? "0.00" : reader.GetDecimal(0).ToString("F2"));
                    properties.Add("{SQR_KOR}", reader.IsDBNull(1) ? "0.00" : reader.GetDecimal(1).ToString("F2"));
                    properties.Add("{SQR_FREE}", reader.IsDBNull(2) ? "0.00" : reader.GetDecimal(2).ToString("F2"));
                    properties.Add("{NUM_BALANS}", reader.IsDBNull(3) ? "0" : reader.GetInt32(3).ToString());
                }
                else
                {
                    properties.Add("{SQR_TOTAL}", "0.00");
                    properties.Add("{SQR_KOR}", "0.00");
                    properties.Add("{SQR_FREE}", "0.00");
                    properties.Add("{NUM_BALANS}", "0");
                }

                reader.Close();
            }
        }

        // Get the properties of deleted balans objects
//        query = @"SELECT
//             SUM(bal.sqr_total) AS 'sqr_total'
//            ,COUNT(*) AS 'obj_count'
//        FROM reports1nf_balans_deleted bal
//        WHERE bal.report_id = @rep and bal.sqr_total > 0
//        GROUP BY bal.report_id";

        query = @"SELECT
            SUM(bal.sqr_total) AS 'sqr_total',
            COUNT(*) AS 'obj_count'
            FROM reports1nf_balans_deleted bal
            WHERE 
            bal.report_id = @rep and 
            bal.sqr_total > 0 and
            (year(bal.vidch_doc_date) = @year or bal.vidch_doc_date is null)
            GROUP BY bal.report_id";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            int reportYear = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
            cmd.Parameters.Add(new SqlParameter("rep", ReportID));
            cmd.Parameters.Add(new SqlParameter("year", reportYear));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    properties.Add("{SQR_VIDCH}", reader.IsDBNull(0) ? "0.00" : reader.GetDecimal(0).ToString("F2"));
                    properties.Add("{NUM_VIDCH}", reader.IsDBNull(1) ? "0" : reader.GetInt32(1).ToString());
                }
                else
                {
                    properties.Add("{SQR_VIDCH}", "0.00");
                    properties.Add("{NUM_VIDCH}", "0");
                }

                reader.Close();
            }
        }

        // Get the properties of rent agreements
//        query = @"SELECT
//             SUM(ar.rent_square) AS 'rent_square'
//            ,COUNT(*) AS 'obj_count'
//        FROM reports1nf_arenda ar
//        WHERE ar.report_id = @rep AND (ar.is_deleted IS NULL OR ar.is_deleted = 0)
//            AND NOT EXISTS(SELECT id FROM arenda a WHERE a.id = ar.id AND ISNULL(a.is_deleted, 1) = 1)
//            AND ar.agreement_state = 1
//        GROUP BY ar.report_id";

        query = @"SELECT SUM(ar.rent_square) AS 'rent_square'
            ,COUNT(*) AS 'obj_count'
        FROM reports1nf_arenda ar
        WHERE ar.report_id = @rep AND (ar.is_deleted IS NULL OR ar.is_deleted = 0)
            AND NOT EXISTS(SELECT id FROM arenda a WHERE a.id = ar.id AND ISNULL(a.is_deleted, 1) = 1)
            AND ar.agreement_state = 1
        GROUP BY ar.report_id, ar.id /*ar.agreement_num,ar.agreement_date,ar.rent_start_date,ar.rent_finish_date,ar.org_giver_id*/";

        decimal sqr_given = 0;
        Int32 num_given = 0;

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("rep", ReportID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    sqr_given += reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                    num_given++;

                    //properties.Add("{SQR_GIVEN}", reader.IsDBNull(0) ? "0.00" : reader.GetDecimal(0).ToString("F2"));
                    //properties.Add("{NUM_GIVEN}", reader.IsDBNull(1) ? "0" : reader.GetInt32(1).ToString());
                }

                reader.Close();

                properties.Add("{SQR_GIVEN}", sqr_given.ToString("F2"));
                properties.Add("{NUM_GIVEN}", num_given.ToString());
            }
        }




        // Get the properties of rented objects
        query = @"SELECT
             SUM(ar.rent_square) AS 'rent_square'
            ,COUNT(*) AS 'obj_count'
        FROM reports1nf_arenda_rented ar
        WHERE ar.report_id = @rep AND (ar.is_deleted IS NULL OR ar.is_deleted = 0)
        GROUP BY ar.report_id";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("rep", ReportID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    properties.Add("{SQR_RENTED}", reader.IsDBNull(0) ? "0.00" : reader.GetDecimal(0).ToString("F2"));
                    properties.Add("{NUM_RENTED}", reader.IsDBNull(1) ? "0" : reader.GetInt32(1).ToString());
                }
                else
                {
                    properties.Add("{SQR_RENTED}", "0.00");
                    properties.Add("{NUM_RENTED}", "0");
                }

                reader.Close();
            }
        }

        if (!properties.ContainsKey("{SQR_RENTED}"))
        {
            properties.Add("{SQR_RENTED}", "0.00");
        }

        if (!properties.ContainsKey("{NUM_RENTED}"))
        {
            properties.Add("{NUM_RENTED}", "0");
        }
    }

    protected void GetRentPaymentProperties(SqlConnection connection, Dictionary<string, string> properties, decimal otherRentPayments)
    {
        // Select the last period that user specified in his report
        int periodId = -1;

        using (SqlCommand cmd = new SqlCommand("SELECT MAX(rent_period_id) FROM reports1nf_arenda_payments WHERE report_id = @rep", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rep", ReportID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        periodId = reader.GetInt32(0);
                }

                reader.Close();
            }
        }

        bool paymentFound = false;

        if (periodId > 0)
        {
            // Get the rent payment totals
            string query = @"SELECT
                 SUM(pay.sqr_total_rent)
                ,SUM(pay.sqr_payed_by_percent)
                ,SUM(pay.sqr_payed_by_1uah)
                ,SUM(pay.sqr_payed_hourly)
                ,SUM(pay.payment_narah)
                ,SUM(pay.last_year_saldo)
                ,SUM(pay.payment_received)
                ,SUM(pay.payment_nar_zvit)
                ,SUM(pay.payment_budget_special)
                ,SUM(pay.debt_total)
                ,SUM(pay.debt_zvit)
                ,SUM(pay.debt_3_month)
                ,SUM(pay.debt_12_month)
                ,SUM(pay.debt_3_years)
                ,SUM(pay.debt_over_3_years)
                ,SUM(pay.debt_v_mezhah_vitrat)
                ,SUM(pay.debt_spysano)
                ,SUM(budget_narah_50_uah)
                ,SUM(budget_zvit_50_uah)
                ,SUM(budget_prev_50_uah)
                ,SUM(budget_debt_50_uah)
                ,SUM(budget_debt_30_50_uah)
                ,SUM(pay.old_debts_payed)
                ,SUM(pay.total_pereplata)
                ,SUM(pay.return_all_orend_payed)
                ,isnull(SUM(pay.payment_narah),0) - isnull(SUM(pay.znyato_nadmirno_narah),0) as payment_narah_normal
                ,SUM(znyato_nadmirno_narah)
                ,SUM(avance_plat)
                ,SUM(zabezdepoz_saldo)
                ,SUM(avance_debt)
            FROM reports1nf_arenda_payments pay
            WHERE pay.report_id = @rep AND pay.rent_period_id = @per
                AND NOT EXISTS(SELECT id FROM arenda a WHERE a.id = pay.arenda_id AND ISNULL(a.is_deleted, 1) = 1) and pay.arenda_id > 0
           GROUP BY pay.report_id";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("rep", ReportID));
                cmd.Parameters.Add(new SqlParameter("per", periodId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal payRecvZvit = reader.IsDBNull(6) ? 0m : reader.GetDecimal(6);
                        var payReturnAorend = reader.IsDBNull(24) ? 0m : reader.GetDecimal(24);

                        payRecvZvit += otherRentPayments;
                        payRecvZvit -= payReturnAorend;

                        properties.Add("{PAY_NARAH_ZVIT}", reader.IsDBNull(25) ? "" : reader.GetDecimal(25).ToString("F2"));
                        properties.Add("{PAY_PEREPLATA}", reader.IsDBNull(5) ? "" : reader.GetDecimal(5).ToString("F2"));
                        properties.Add("{PAY_RECV_ZVIT}", payRecvZvit > 0m ? payRecvZvit.ToString("F2") : "");
                        properties.Add("{PAY_RECV_NARAH}", reader.IsDBNull(7) ? "" : reader.GetDecimal(7).ToString("F2"));
                        properties.Add("{PAY_RET_OREND}", reader.IsDBNull(23) ? "" : reader.GetDecimal(23).ToString("F2"));
                        properties.Add("{PAY_RET_AOREND}", reader.IsDBNull(24) ? "" : reader.GetDecimal(24).ToString("F2"));
                        properties.Add("{PAY_LAST_PER}", reader.IsDBNull(22) ? "" : reader.GetDecimal(22).ToString("F2"));                        
                        properties.Add("{PAY_DEBT_TOTAL}", reader.IsDBNull(9) ? "" : reader.GetDecimal(9).ToString("F2"));
                        properties.Add("{PAY_DEBT_ZVIT}", reader.IsDBNull(10) ? "" : reader.GetDecimal(10).ToString("F2"));
                        properties.Add("{PAY_DEBT_V_MEZH}", reader.IsDBNull(15) ? "" : reader.GetDecimal(15).ToString("F2"));
                        properties.Add("{ZNYATO_NADM}", reader.IsDBNull(26) ? "" : reader.GetDecimal(26).ToString("F2"));
                        properties.Add("{AVANCE_PLAT}", reader.IsDBNull(27) ? "" : reader.GetDecimal(27).ToString("F2"));
                        properties.Add("{AVANCE_SALDO}", reader.IsDBNull(28) ? "" : reader.GetDecimal(28).ToString("F2"));
                        properties.Add("{AVANCE_DEBT}", reader.IsDBNull(29) ? "" : reader.GetDecimal(29).ToString("F2"));
                        // properties.Add("{PAY_SPECIAL}", reader.IsDBNull(8) ? "" : reader.GetDecimal(8).ToString("F2"));
                        // properties.Add("{PAY_50_NARAH}", reader.IsDBNull(17) ? "" : reader.GetDecimal(17).ToString("F2"));
                        // properties.Add("{PAY_50_PAYED}", reader.IsDBNull(18) ? "" : reader.GetDecimal(18).ToString("F2"));
                        // properties.Add("{PAY_50_DEBT}", reader.IsDBNull(19) ? "" : reader.GetDecimal(19).ToString("F2"));
                        // properties.Add("{PAY_50_DEBT_CUR}", reader.IsDBNull(20) ? "" : reader.GetDecimal(20).ToString("F2"));
                        // properties.Add("{PAY_50_DEBT_OLD}", reader.IsDBNull(21) ? "" : reader.GetDecimal(21).ToString("F2"));

                        paymentFound = true;
                    }

                    reader.Close();
                }
            }
        }

        if (!paymentFound)
        {
            // No data in Report
            properties.Add("{PAY_NARAH_ZVIT}", "");
            properties.Add("{PAY_PEREPLATA}", "");
            properties.Add("{PAY_RECV_ZVIT}", "");
            properties.Add("{PAY_RECV_NARAH}", "");
            properties.Add("{PAY_RET_OREND}", "");
            properties.Add("{PAY_RET_AOREND}", "");
            properties.Add("{PAY_LAST_PER}", "");            
            properties.Add("{PAY_DEBT_TOTAL}", "");
            properties.Add("{PAY_DEBT_ZVIT}", "");
            properties.Add("{PAY_DEBT_V_MEZH}", "");
            properties.Add("{ZNYATO_NADM}", "");
            properties.Add("{AVANCE_PLAT}", "");
            properties.Add("{AVANCE_SALDO}", "");
            properties.Add("{AVANCE_DEBT}", "");
            // properties.Add("{PAY_SPECIAL}", "");
            // properties.Add("{PAY_50_NARAH}", "");
            // properties.Add("{PAY_50_PAYED}", "");
            // properties.Add("{PAY_50_DEBT}", "");
            // properties.Add("{PAY_50_DEBT_CUR}", "");
            // properties.Add("{PAY_50_DEBT_OLD}", "");
        }

        // Get the CMK totals
        bool cmkFound = false;

        string queryCmk = @"SELECT
             SUM(pay.cmk_sqr_rented)
            ,SUM(pay.cmk_payment_narah)
            ,SUM(pay.cmk_payment_to_budget)
            ,SUM(pay.cmk_rent_debt)
        FROM reports1nf_arenda_rented pay
        WHERE pay.report_id = @rep AND pay.is_cmk > 0
        GROUP BY pay.report_id";

        using (SqlCommand cmd = new SqlCommand(queryCmk, connection))
        {
            cmd.Parameters.Add(new SqlParameter("rep", ReportID));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    properties.Add("{PAY_CMK_SQR}", reader.IsDBNull(0) ? "" : reader.GetDecimal(0).ToString("F2"));
                    properties.Add("{PAY_CMK_NARAH}", reader.IsDBNull(1) ? "" : reader.GetDecimal(1).ToString("F2"));
                    properties.Add("{PAY_CMK_BUDGET}", reader.IsDBNull(2) ? "" : reader.GetDecimal(2).ToString("F2"));
                    properties.Add("{PAY_CMK_DEBT}", reader.IsDBNull(3) ? "" : reader.GetDecimal(3).ToString("F2"));

                    cmkFound = true;
                }

                reader.Close();
            }
        }

        if (!cmkFound)
        {
            // No CMK data in Report
            properties.Add("{PAY_CMK_SQR}", "");
            properties.Add("{PAY_CMK_NARAH}", "");
            properties.Add("{PAY_CMK_BUDGET}", "");
            properties.Add("{PAY_CMK_DEBT}", "");
        }
    }

    protected void ReplaceDocTagInElements(MainDocumentPart mainPart, string tag, string replacement)
    {
        List<OpenXmlElement> elements = mainPart.Document.Body.OfType<OpenXmlElement>().ToList();

        foreach (OpenXmlElement element in elements)
        {
            if (element is Paragraph)
            {
                ReplaceDocTag(mainPart, element as Paragraph, tag, replacement);
            }
            else if (element is DocumentFormat.OpenXml.Wordprocessing.Table)
            {
                List<DocumentFormat.OpenXml.Wordprocessing.TableRow> rows = element.ChildElements.OfType<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ToList();

                foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow row in rows)
                {
                    List<DocumentFormat.OpenXml.Wordprocessing.TableCell> cells = row.ChildElements.OfType<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ToList();

                    foreach (DocumentFormat.OpenXml.Wordprocessing.TableCell cell in cells)
                    {
                        foreach (OpenXmlElement e in cell.ChildElements)
                        {
                            if (e is Paragraph)
                            {
                                ReplaceDocTag(mainPart, e as Paragraph, tag, replacement);
                            }
                        }
                    }
                }
            }
        }
    }

    protected void ReplaceDocTag(MainDocumentPart mainPart, Paragraph para, string tag, string replacement)
    {
        string paragraphText = GetParagraphText(para);

        // Replace the tag
        bool replaced = false;
        int pos = paragraphText.IndexOf(tag);

        while (pos >= 0)
        {
            replaced = true;

            paragraphText = paragraphText.Replace(tag, replacement);

            // Search once again
            pos = paragraphText.IndexOf(tag);
        }

        if (replaced)
        {
            Run firstRun = para.OfType<Run>().First<Run>();

            if (firstRun == null)
            {
                firstRun = new Run();
            }

            // Delete all paragraph sub-items
            para.RemoveAllChildren<Run>();

            // Add the text to the first Run
            firstRun.RemoveAllChildren<Text>();
            firstRun.AppendChild(new Text(paragraphText));

            // Create a new run with the modified text
            para.AppendChild(firstRun);
        }
    }

    protected string GetParagraphText(Paragraph para)
    {
        string paragraphText = "";

        List<Run> runs = para.OfType<Run>().ToList();

        foreach (Run run in runs)
        {
            List<Text> texts = run.OfType<Text>().ToList();

            foreach (Text text in texts)
            {
                paragraphText += text.Text;
            }
        }

        return paragraphText;
    }

    protected string GetDateMonthName(object date)
    {
        if (date is DateTime)
        {
            switch (((DateTime)date).Month)
            {
                case 1:
                    return Resources.Strings.Month1;

                case 2:
                    return Resources.Strings.Month2;

                case 3:
                    return Resources.Strings.Month3;

                case 4:
                    return Resources.Strings.Month4;

                case 5:
                    return Resources.Strings.Month5;

                case 6:
                    return Resources.Strings.Month6;

                case 7:
                    return Resources.Strings.Month7;

                case 8:
                    return Resources.Strings.Month8;

                case 9:
                    return Resources.Strings.Month9;

                case 10:
                    return Resources.Strings.Month10;

                case 11:
                    return Resources.Strings.Month11;

                case 12:
                    return Resources.Strings.Month12;
            }
        }

        return "";
    }

    #endregion (Export to Word)
}

public class SaltedHash
{
    public string Hash { get; private set; }
    public string Salt { get; private set; }

    public SaltedHash(string password)
    {
        var saltBytes = new byte[32];
        using (var provider = RandomNumberGenerator.Create())
        {
            provider.GetNonZeroBytes(saltBytes);
        }
        Salt = Convert.ToBase64String(saltBytes);
        var passwordAndSaltBytes = Concat(password, saltBytes);
        Hash = ComputeHash(passwordAndSaltBytes);
    }

    static string ComputeHash(byte[] bytes)
    {
        using (var sha256 = SHA256.Create())
        {
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }
    }

    static byte[] Concat(string password, byte[] saltBytes)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        return passwordBytes.Concat(saltBytes).ToArray();
    }

    public static bool Verify(string salt, string hash, string password)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var passwordAndSaltBytes = Concat(password, saltBytes);
        var hashAttempt = ComputeHash(passwordAndSaltBytes);
        return hash == hashAttempt;
    }
}