using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using GUKV.Common;

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

        if (Roles.IsUserInRole(Utils.MISTOControllerRole))
		{
            ParmMistoId = Utils.GetUserMistoId("");
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
        e.Command.Parameters["@p_misto_id"].Value = ParmMistoId;
    }

	protected void SqlDataSourceReports_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		var dbparams = (System.Data.SqlClient.SqlParameterCollection)(e.Command.Parameters);
		dbparams.AddWithValue("@stan_recieve_date", DateTime.Now);

		//var user = Membership.GetUser();
		//var username = (user == null ? String.Empty : (String)user.UserName);
		//dbparams.AddWithValue("@modified_by2", username);
	}

    protected void ASPxButton_Zvedeniy_Build(object sender, EventArgs e)
    {
        var builder = new ZvedZvitBuilder
        {
            Page = this,
        };
        builder.Go();
    }

    int ParmMistoId;

    class ZvedZvitBuilder
	{
        public Page Page { get; set; }

        public void Go()
        {
            string templateFileName = Page.Server.MapPath("Templates/zved_zvit.xlsx");
            var tempFile = TempFile.FromExistingFile(templateFileName);

            var connection = CommonUtils.ConnectToDatabase2016();
            if (connection == null) throw new Exception("Database GUKV2016 not found");
            var factory = DbProviderFactories.GetFactory(connection);
            var dataTable = new DataTable();
            using (var cmd = factory.CreateCommand())
            {
                cmd.CommandText = GetMainSql();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                using (var adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }

            var workbook = new Workbook();
            workbook.LoadDocument(tempFile.FileName, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            var wsheet = workbook.Worksheets[0];
            var usedRange = wsheet.GetUsedRange();
            var bcolumn = wsheet.Columns["B"];
            var rowOccupations = new List<string>();
            for (int r = 0; r < usedRange.RowCount; r++)
			{
                var value = bcolumn[r].Value.TextValue;
                rowOccupations.Add(value);
            }


            for(int r = 0; r < dataTable.Rows.Count; r++)
			{
                var occupation = dataTable.Rows[r]["dict_rent_occupation_name"].ToString() ?? "";
                var erow = rowOccupations.FindIndex(q => (q ?? "").ToLower() == occupation.ToLower());
                if (erow < 0)
                {
                    Debug.WriteLine("occupation=" + occupation); continue;
                    //throw new ArgumentException("occupation=" + occupation);
                        //нету в Excel-файле
                        //--occupation = Соціальна сфера
                        //--occupation = Невідомо
                }

                for (int cnum = 1; cnum < dataTable.Columns.Count; cnum++)
				{
                    var column = dataTable.Columns[cnum];
                    var vnum = Int32.Parse(column.ColumnName.Replace("v", ""));
                    var dval = dataTable.Rows[r][cnum];
                    var val = (dval as decimal?) ?? 0M;
                    wsheet[erow, vnum - 1].Value = val;
                }
            }

            SumBuild(18, new[] { 5, 6, 8, 9, 10, 11, 12, 13, 14, 16, 17 }, wsheet);
            SumBuild(30, new[] { 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 }, wsheet);
            SumBuild(31, new[] { 18, 30 }, wsheet);


            using (var cmd = new SqlCommand(@"SELECT [name]+' р.' as dict_rent_period FROM [dbo].[dict_rent_period] where [is_active] = 1", connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dict_rent_period = reader.GetString(0);
                        wsheet["A1"].Value = "Зведені показники надходжень від оренди та перерахування її частини до бюджету\nза " + dict_rent_period;
                    }
                }
            }




            workbook.SaveDocument(tempFile.FileName);


            var info = new System.IO.FileInfo(tempFile.FileName);
            Page.Response.Clear();
            Page.Response.ClearHeaders();
            Page.Response.ClearContent();
            Page.Response.ContentType = "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Page.Response.AddHeader("content-disposition", "attachment; filename=zved_zvit.xlsx; size=" + info.Length.ToString());
            using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
            {
                stream.CopyTo(Page.Response.OutputStream);
            }
            tempFile.Dispose();
            Page.Response.End();
        }

        void SumBuild(int erow_total, int[] erows_sum, Worksheet wsheet)
		{
            for(int cnum = 3; cnum <= 25; cnum++)
			{
                decimal sum = 0;
                foreach (var erow in erows_sum)
                {
                    var value = wsheet[erow - 1, cnum - 1].Value.NumericValue;
                    sum += (decimal)value;
                }
                wsheet[erow_total - 1, cnum - 1].Value = sum;
            }
		}


        string GetMainSql()
		{
            // C:\Users\dima7777\Documents\SQL Server Management Studio\__22222222233333333.sql
            var sql = @"
select 
dict_rent_occupation_name,
sum(SQR_TOTAL_BAL) as v3,
sum(SQR_GIVEN) as v4,
sum(NUM_RENTER) as v5,
sum(PAY_NARAH_ZVIT) as v6,	
sum(PAY_RECV_ZVIT) as v7,	
sum(PAY_DEBT_TOTAL) as v8,	
sum(PAY_DEBT_ZVIT) as v9,	
sum(PAY_DEBT_OVER_3_YEARS) as v10,	
sum(PAY_DEBT_3_YEARS) as v11,		
sum(PAY_DEBT_12_MONTH) as v12,		
sum(PAY_50_NARAH) as v13,
sum(PAY_50_PAYED) as v14,
sum(PAY_50_DEBT) as v15,
sum(PAY_50_DEBT_CUR) as v16,
sum(PAY_50_DEBT_CUR) as v17,
sum(PAY_NUM_ZAHODIV_TOTAL) as v18,	
sum(PAY_NUM_POZOV_TOTAL) as v19,		
sum(PAY_NUM_POZOV_ZADOV_TOTAL) as v20,		
sum(PAY_NUM_POZOV_VIKON_TOTAL) as v21,		
sum(PAY_DEBT_POGASHENO_TOTAL) as v22,		
sum(cast(null as decimal)) as v23,
sum(PAY_50_DEBT_OLD) as v24,
sum(debt_spysano) as v25		
from
(
SELECT 
		
        isnull(ddd.name, 'Невідомо') as 'dict_rent_occupation_name',
        (SELECT Q.stan_recieve_name FROM dict_stan_recieve Q where Q.stan_recieve_id = rep.stan_recieve_id) stan_recieve_name,
		rep.cur_state,
		--rep.*,
		ar.NumOfSubmAgr, 
		ar.NumOfAgr, 
		obj.NumOfSubmObj, 
		obj.NumOfObj,
        (SELECT MAX(sdt) FROM (VALUES
        (rep.bal_max_submit_date),
        (rep.bal_del_max_submit_date),
        (rep.arenda_max_submit_date),
        (rep.arenda_rented_max_submit_date),
        (rep.org_max_submit_date)) AS AllMaxSubmitDates(sdt)) AS 'max_submit_date',
        CASE WHEN rep.is_reviewed = 0 THEN N'НI' ELSE N'ТАК' END AS 'review_performed',

		SQR_TOTAL_BAL,SQR_GIVEN,NUM_RENTER,PAY_NARAH_ZVIT,PAY_DEBT_TOTAL,PAY_DEBT_ZVIT,PAY_50_NARAH,PAY_50_PAYED,PAY_50_DEBT,PAY_50_DEBT_CUR,PAY_50_DEBT_OLD,PAY_RECV_ZVIT,
		PAY_DEBT_OVER_3_YEARS,PAY_DEBT_3_YEARS,PAY_DEBT_12_MONTH,PAY_NUM_ZAHODIV_TOTAL,PAY_NUM_POZOV_TOTAL,PAY_NUM_POZOV_ZADOV_TOTAL,PAY_NUM_POZOV_VIKON_TOTAL,PAY_DEBT_POGASHENO_TOTAL,debt_spysano


        
        --,ReportDates.*
        
        --,ObjectAndRentTotals1.*
        --,ObjectAndRentTotals2.*
        --,ObjectAndRentTotals3.*
        --,ObjectAndRentTotals4.*
		--,OrganizationProperties.*
		--,RentPaymentProperties1.*
		--,RentPaymentProperties2.*
        
        FROM view_reports1nf rep
        LEFT JOIN (SELECT sum(CASE WHEN (r1a.submit_date IS NULL OR r1a.modify_date IS NULL OR r1a.modify_date > r1a.submit_date) THEN 0 ELSE 1 END) as NumOfSubmAgr, 
			Count(r1a.ID) AS NumOfAgr, report_id
        FROM reports1nf_arenda r1a LEFT JOIN arenda a
        ON r1a.id = a.id WHERE a.is_deleted IS NULL OR a.is_deleted = 0
        GROUP BY report_id) ar on rep.report_id = ar.report_id
        
        LEFT JOIN (SELECT sum(CASE WHEN (b.submit_date IS NULL OR b.modify_date IS NULL OR b.modify_date > b.submit_date) THEN 0 ELSE 1 END) AS NumOfSubmObj,
        Count(ID) AS NumOfObj,
        report_id
        FROM reports1nf_balans b
        WHERE is_deleted IS NULL OR is_deleted = 0
        GROUP BY report_id) obj on rep.report_id = obj.report_id
        
  --      LEFT JOIN (
		--SELECT 
	 --       (SELECT MAX(org.submit_date) FROM reports1nf_org_info org WHERE org.report_id = r.id) AS 'org_date',
	 --       (SELECT MAX(bal.submit_date) FROM reports1nf_balans bal WHERE bal.report_id = r.id AND (bal.is_deleted IS NULL OR bal.is_deleted = 0)) AS 'bal_date',
	 --       (SELECT MAX(bd.submit_date) FROM reports1nf_balans_deleted bd WHERE bd.report_id = r.id) AS 'bal_deleted_date',
	 --       (SELECT MAX(ar.submit_date) FROM reports1nf_arenda ar WHERE ar.report_id = r.id AND (ar.is_deleted IS NULL OR ar.is_deleted = 0)) AS 'arenda_date',
	 --       (SELECT MAX(arr.submit_date) FROM reports1nf_arenda_rented arr WHERE arr.report_id = r.id AND (arr.is_deleted IS NULL OR arr.is_deleted = 0)) AS 'arenda_rented_date',
	 --       id as report_id
  --      FROM reports1nf r GROUP BY r.id) ReportDates on ReportDates.report_id = rep.report_id
        
        LEFT JOIN (
        SELECT
             SUM(bal.sqr_total) AS 'SQR_TOTAL_BAL'
            ,SUM(bal.sqr_kor) AS 'SQR_KOR'
--            ,SUM(CASE WHEN bal.is_free_sqr = 1 THEN bal.free_sqr_useful ELSE 0 END) AS 'SQR_FREE'
--            ,SUM(bal.free_sqr_useful) AS 'SQR_FREE'
              ,sum(fs.sqr_free) AS 'SQR_FREE'
            ,COUNT(*) AS 'NUM_BALANS'
            ,bal.report_id
        FROM reports1nf_balans bal
        outer apply (select sum(fs.total_free_sqr) as sqr_free from reports1nf_balans_free_square fs where fs.balans_id = bal.id and fs.report_id = bal.report_id and fs.is_included = 1) fs  
        WHERE (bal.is_deleted IS NULL OR bal.is_deleted = 0)
        GROUP BY bal.report_id) ObjectAndRentTotals1 ON ObjectAndRentTotals1.report_id = rep.report_id
        
        LEFT JOIN (
        SELECT
            SUM(bal.sqr_total) AS 'SQR_VIDCH',
            COUNT(*) AS 'NUM_VIDCH',
            bal.report_id
            FROM reports1nf_balans_deleted bal
            WHERE 
            bal.sqr_total > 0 and
            (year(bal.vidch_doc_date) = @period_year or bal.vidch_doc_date is null)
            GROUP BY bal.report_id) ObjectAndRentTotals2 ON ObjectAndRentTotals2.report_id = rep.report_id
            
         LEFT JOIN (
         SELECT R.report_id, SUM(R.row_count) AS 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN', COUNT(distinct org_renter_id) AS 'NUM_RENTER'
--        SELECT R.report_id, SUM(R.row_count) AS 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN'
--         SELECT R.report_id, SUM(NUM_GIVEN)  as 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN'
         FROM (
         	SELECT 
		SUM(ar.rent_square) AS 'SQR_GIVEN'
           		,COUNT(ar.id) AS 'NUM_GIVEN'
           		,1 AS 'row_count'
            		,ar.report_id
		,ar.org_renter_id
		FROM reports1nf_arenda ar
		WHERE (ar.is_deleted IS NULL OR ar.is_deleted = 0)
			AND NOT EXISTS(SELECT id FROM arenda a WHERE a.id = ar.id AND ISNULL(a.is_deleted, 1) = 1)
			AND ar.agreement_state = 1		
		GROUP BY ar.report_id,ar.id /*ar.agreement_num,ar.agreement_date,ar.rent_start_date,ar.rent_finish_date*/,ar.org_renter_id) R GROUP BY R.report_id /*, R.row_count*/) ObjectAndRentTotals3 ON ObjectAndRentTotals3.report_id = rep.report_id
--		GROUP BY ar.report_id ,ar.agreement_num /*,ar.agreement_date,ar.rent_start_date,ar.rent_finish_date,ar.org_giver_id*/) R GROUP BY R.report_id /*, R.row_count*/) ObjectAndRentTotals3 ON ObjectAndRentTotals3.report_id = rep.report_id
			
         LEFT JOIN (
	SELECT
             SUM(ar.rent_square) AS 'SQR_RENTED'
            ,COUNT(*) AS 'NUM_RENTED',
            ar.report_id
			FROM reports1nf_arenda_rented ar
			WHERE (ar.is_deleted IS NULL OR ar.is_deleted = 0)
			GROUP BY ar.report_id) ObjectAndRentTotals4 ON ObjectAndRentTotals4.report_id = rep.report_id
			
		LEFT JOIN (
		SELECT
            [org].[full_name] AS 'SUBMITTER_FULL_NAME' -- 0
           ,[org].[short_name] AS 'SUBMITTER_SHORT_NAME' -- 1
           ,[org].[zkpo_code] AS 'SUBMITTER_ZKPO'-- 2
           ,[dict_org_industry].[name] AS 'ORG_INDUSTRY' -- 3
           ,[dict_org_occupation].[name] AS 'ORG_OCCUPATION' -- 4
           ,[dict_org_status].[name] AS 'ORG_STATUS' -- 5
           ,[dict_org_form_gosp].[name] AS 'ORG_FIN_FORM' -- 6
           ,[dict_org_ownership].[name] AS 'ORG_OWNERSHIP' -- 7
           ,[dict_org_gosp_struct].[name] AS 'gosp_struct' -- 8
           ,[dict_org_vedomstvo].[name] AS 'ORG_VEDOMSTVO' -- 9
           ,[dict_org_form].[name] AS 'ORG_GOSP_FORM' -- 10
           --,[dict_org_gosp_struct_type].[name] AS 'gosp_struct_type' -- 11
           --,[dict_org_sfera_upr].[name] AS 'sfera_upr' -- 12
           ,[dict_org_old_organ].[name] AS 'ORG_GOSP_UPR' -- 13
           ,[director_fio] AS 'BOSS_FIO' -- 14
           ,[director_phone] AS 'BOSS_TEL' -- 15
           ,[director_email] AS 'BOSS_EMAIL'-- 16
           ,[director_title] AS 'BOSS_POSADA'-- 17
           ,[kved_code] AS 'ORG_KVED' -- 18
           --,jr_street.name AS 'addr_street' -- 19
           --,org.addr_nomer -- 20
           --,org.addr_misc -- 21
           ,ph_street.name AS 'physAddrStreet' -- 22
           ,org.phys_addr_nomer AS 'physAddrNumber' -- 23
           ,org.phys_addr_misc AS 'physAddrMisc'-- 24
           ,CASE WHEN org.contribution_rate IS NULL THEN 0 ELSE ORG.contribution_rate END AS 'ORG_CONTRIB_RATE' -- 25
           ,org.buhgalter_fio AS 'USER_FIO' -- 26
           ,org.buhgalter_phone AS 'USER_TEL' -- 27
           ,org.buhgalter_email AS 'USER_EMAIL' -- 28
           ,org.budget_narah_50_uah AS 'PAY_50_NARAH' -- 29
           ,org.budget_zvit_50_uah AS 'PAY_50_PAYED'-- 30
           ,org.budget_prev_50_uah AS 'PAY_50_DEBT'-- 31
           ,org.budget_debt_30_50_uah AS 'PAY_50_DEBT_OLD' -- 32
           ,org.payment_budget_special AS 'PAY_SPECIAL'-- 33
           --,org.konkurs_payments -- 34
           --,org.unknown_payments -- 35
           --,CASE WHEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) > 0 THEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) ELSE 0 END AS 'PAY_50_DEBT_CUR'
           ,CASE WHEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0) + ISNULL(org.budget_prev_50_uah, 0)) - ISNULL(org.unknown_payments,0) < 0 THEN 0 Else (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0) + ISNULL(org.budget_prev_50_uah, 0)) - ISNULL(org.unknown_payments,0) END AS 'PAY_50_DEBT_CUR'
           ,org.konkurs_payments AS 'PAY_RECV_OTHER'
           ,[org].[report_id]
           ,[dict_otdel_gukv].name as 'otdel_gukv'
           ,[org].[prim_balanc]
           ,org.unknown_payments AS 'PAY_UNKNOWN_PAYMENTS'
        FROM
            reports1nf_org_info org
            LEFT OUTER JOIN dict_otdel_gukv ON org.otdel_gukv_id = dict_otdel_gukv.id
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
            LEFT OUTER JOIN dict_streets ph_street ON org.phys_addr_street_id = ph_street.id) OrganizationProperties ON OrganizationProperties.report_id = rep.report_id
            
		LEFT JOIN (SELECT report_id,MAX(rent_period_id) AS 'max_rent_period_id' FROM reports1nf_arenda_payments group by report_id) mrp on mrp.report_id = rep.report_id
            		
		LEFT JOIN (
		SELECT
                 --SUM(pay.sqr_total_rent) as 'sqr_total_rent' -- 0
                --,SUM(pay.sqr_payed_by_percent) as 'sqr_payed_by_percent' -- 1
                --,SUM(pay.sqr_payed_by_1uah) as 'sqr_payed_by_1uah' -- 2
                --,SUM(pay.sqr_payed_hourly) as 'sqr_payed_hourly' -- 3
                SUM(pay.payment_narah) as 'PAY_NARAH_ZVIT' -- 4
                ,SUM(pay.last_year_saldo) as 'PAY_PEREPLATA' -- 5
                ,SUM(pay.payment_received) as 'PAY_RECV_ZVIT' -- 6
                ,SUM(pay.payment_nar_zvit) as 'PAY_RECV_NARAH' -- 7
                --,SUM(pay.payment_budget_special) as 'payment_budget_special' -- 8
                ,SUM(pay.debt_total) as 'PAY_DEBT_TOTAL' -- 9
                ,SUM(pay.debt_zvit) as 'PAY_DEBT_ZVIT' -- 10
                --,SUM(pay.debt_3_month) as 'debt_3_month' -- 11
                --,SUM(pay.debt_12_month) as 'debt_12_month' -- 12
                --,SUM(pay.debt_3_years) as 'debt_3_years' -- 13
                --,SUM(pay.debt_over_3_years) as 'debt_over_3_years' -- 14
                ,SUM(pay.debt_v_mezhah_vitrat) as 'PAY_DEBT_V_MEZH' -- 15
                ,SUM(pay.debt_spysano) as 'debt_spysano' -- 16
                --,SUM(budget_narah_50_uah) as 'budget_narah_50_uah' -- 17
                --,SUM(budget_zvit_50_uah) as 'budget_zvit_50_uah' -- 18
                --,SUM(budget_prev_50_uah) as 'budget_prev_50_uah' -- 19
                --,SUM(budget_debt_50_uah) as 'budget_debt_50_uah' -- 20
                --,SUM(budget_debt_30_50_uah) as 'budget_debt_30_50_uah' -- 21
                ,SUM(pay.old_debts_payed) as 'PAY_LAST_PER' -- 22
                ,SUM( isnull(pay.payment_narah,0) - isnull(pay.znyato_nadmirno_narah,0) ) as 'PAY_NARAH_ZVIT_NORMAL' -- 23
                ,SUM(pay.znyato_nadmirno_narah) as 'PAY_ZNYATO_NADMIRNO_NARAH' -- 24
                ,SUM(pay.avance_saldo) as 'PAY_AVANCE_SALDO' -- 25
                ,SUM(pay.avance_plat) as 'PAY_AVANCE_PLAT' -- 26
                ,SUM( isnull(pay.return_orend_payed,0) + isnull(pay.last_year_saldo,0) ) as 'PAY_PEREPLATA_ALL' -- 27
                ,SUM(pay.avance_debt) as 'PAY_AVANCE_DEBT' -- 28
                ,SUM(pay.avance_paymentnar) as 'PAY_AVANCE_PAYMENTNAR' -- 28
                ,SUM(pay.return_orend_payed) as 'PAY_RETURN_OREND_PAYED' -- 28
                ,SUM(pay.znyato_from_avance) as 'PAY_ZNYATO_FROM_AVANCE' -- 28
                ,SUM(pay.return_all_orend_payed) as 'PAY_RETURN_ALL_OREND_PAYED' -- 28

					--новые поля
					,SUM(pay.debt_over_3_years) as 'PAY_DEBT_OVER_3_YEARS'
					,SUM(pay.debt_3_years) as 'PAY_DEBT_3_YEARS'
					,SUM(pay.debt_12_month) as 'PAY_DEBT_12_MONTH'
					,SUM(pay.num_zahodiv_total) as 'PAY_NUM_ZAHODIV_TOTAL'
					,SUM(pay.num_pozov_total) as 'PAY_NUM_POZOV_TOTAL'
					,SUM(pay.num_pozov_zadov_total) as 'PAY_NUM_POZOV_ZADOV_TOTAL'
					,SUM(pay.num_pozov_vikon_total) as 'PAY_NUM_POZOV_VIKON_TOTAL'
					,SUM(pay.debt_pogasheno_total) as 'PAY_DEBT_POGASHENO_TOTAL'
					
					
				
				,report_id
				,rent_period_id
            FROM reports1nf_arenda_payments pay
            WHERE NOT EXISTS(SELECT id FROM arenda a WHERE a.id = pay.arenda_id AND ISNULL(a.is_deleted, 1) = 1)  and pay.arenda_id > 0
            GROUP BY pay.report_id,pay.rent_period_id) RentPaymentProperties1 ON RentPaymentProperties1.report_id = rep.report_id and RentPaymentProperties1.rent_period_id = mrp.max_rent_period_id
            
        LEFT JOIN (
        SELECT
            SUM(pay.cmk_sqr_rented) as 'PAY_CMK_SQR'
            ,SUM(pay.cmk_payment_narah) as 'PAY_CMK_NARAH'
            ,SUM(pay.cmk_payment_to_budget) as 'PAY_CMK_BUDGET'
            ,SUM(pay.cmk_rent_debt) as 'PAY_CMK_DEBT'
            ,pay.report_id
        FROM reports1nf_arenda_rented pay
        WHERE pay.is_cmk > 0
        GROUP BY pay.report_id) RentPaymentProperties2 ON RentPaymentProperties2.report_id = rep.report_id

		LEFT OUTER JOIN (
select obp.org_id,occ.name from org_by_period obp
join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
join dict_rent_occupation occ on occ.id = obp.org_occupation_id
		) DDD ON DDD.org_id = rep.organization_id


--        WHERE (@p_rda_district_id = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @p_rda_district_id))
) as T
where 1=1
and cur_state = 'Надісланий'
and max_submit_date >= '20201001'
group by dict_rent_occupation_name
";
            var period_year = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
            sql = sql.Replace("@period_year", period_year.ToString());

            return sql;
        }

    }

}