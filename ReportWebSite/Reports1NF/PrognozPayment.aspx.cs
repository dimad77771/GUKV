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
using System.Data;
using System.Data.Common;
using DevExpress.Spreadsheet;



public partial class Reports1NF_Report1NFPrivatisatSquare : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Roles.IsUserInRole(Utils.Prognoz)) Response.Redirect("~/Account/Restricted.aspx");

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
		var reportIds = new List<int>();
		for (int i = PrivatisatGridView.VisibleStartIndex; i < PrivatisatGridView.VisibleRowCount; i++)
		{
			var report_id = (int)PrivatisatGridView.GetRowValues(i, new[] { "report_id" });
			reportIds.Add(report_id);
		}

		var report_id_where = String.Join(",", reportIds);
		if (string.IsNullOrEmpty(report_id_where))
		{
			report_id_where = "-1";
		}


		var builder = new PrognozPaymentZvitBuilder
		{
			Page = this,
			UseInflation = CheckBoxInflation.Checked,
			UseDictRentalRate = CheckBoxDictRentalRate.Checked,
			report_id_where = report_id_where,
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

	protected void ASPxButton_Recalculate_Click(object sender, EventArgs e)
	{
		var builder = new NarazhCalculationAll
		{
		};
		builder.Run();
	}
}


public class PrognozPaymentZvitBuilder
{
	public Page Page { get; set; }
	public bool UseInflation { get; set; }
	public bool UseDictRentalRate { get; set; }
	public int year = 2025;
	public string report_id_where = "-1";

	public void Go()
	{
		string templateFileName = Page.Server.MapPath("Templates/prognoz_zvit.xlsx");
		var tempFile = TempFile.FromExistingFile(templateFileName);

		var connection = CommonUtils.ConnectToDatabase2016();
		if (connection == null) throw new Exception("Database not found");
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


		for (int r = 0; r < dataTable.Rows.Count; r++)
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

				var val = default(decimal);
				if (dval is DBNull)
				{
					val = 0;
				}
				else if (dval is decimal?)
				{
					val = (decimal?)dval ?? 0;
				}
				else if (dval is int?)
				{
					val = (int?)dval ?? 0;
				}
				else
				{
					throw new Exception("dval=" + dval);
				}



				if (new[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 }.Contains(vnum))
				{
					val = val / 1000M;
				}

				wsheet[erow, vnum - 1].Value = val;
			}
		}

		SumBuild(7, new[] { 8, 9, 10, 11, 12, 13, 14 }, wsheet);
		SumBuild(19, new[] { 5, 6, 8, 9, 10, 11, 12, 13, 14, 16, 17, 18 }, wsheet);
		SumBuild(30, new[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 }, wsheet);
		SumBuild(31, new[] { 19, 30 }, wsheet);


		//using (var cmd = new SqlCommand(@"SELECT [name]+' р.' as dict_rent_period FROM [dbo].[dict_rent_period] where [is_active] = 1", connection))
		//{
		//	using (SqlDataReader reader = cmd.ExecuteReader())
		//	{
		//		while (reader.Read())
		//		{
		//			var dict_rent_period = reader.GetString(0);

		//		}
		//	}
		//}

		var text = "Прогнозні показники надходжень від оренди та перерахування її частини до бюджету у " + year + " р.";
		wsheet["A1"].Value = text;
		wsheet["F1"].Value = "Друком на:\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");


		workbook.SaveDocument(tempFile.FileName);


		var info = new System.IO.FileInfo(tempFile.FileName);
		Page.Response.Clear();
		Page.Response.ClearHeaders();
		Page.Response.ClearContent();
		Page.Response.ContentType = "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		Page.Response.AddHeader("content-disposition", "attachment; filename=prognoz_zvit_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx; size=" + info.Length.ToString());
		using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
		{
			stream.CopyTo(Page.Response.OutputStream);
		}
		tempFile.Dispose();
		Page.Response.End();
	}

	void SumBuild(int erow_total, int[] erows_sum, Worksheet wsheet)
	{
		for (int cnum = 3; cnum <= 18; cnum++)
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
		var sql = @"
WITH DG AS
(
SELECT 
rep.zkpo_code, rep.report_id,
CASE
	WHEN rep.zkpo_code IN ( '02772037', '03327664', '03346331' )
		THEN 'Від прибутку згідно з угодой'
		ELSE Isnull(ddd.NAME, 'Невідомо')
END AS dict_rent_occupation_name,
case when exists (select 1 from reports1nf_arenda Q where Q.report_id = rep.report_id and Q.agreement_state = 1) then 1 else 0 end has_active_dog
--count(*)
--Isnull(ddd.NAME, 'Невідомо') AS 'dict_rent_occupation_name'
FROM   view_reports1nf rep
		LEFT OUTER JOIN (
			select obp.org_id,occ.name from org_by_period obp
			join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
			join dict_rent_occupation occ on occ.id = obp.org_occupation_id
		) DDD ON DDD.org_id = rep.organization_id
       LEFT JOIN (SELECT Sum(CASE
                               WHEN ( r1a.submit_date IS NULL
                                       OR r1a.modify_date IS NULL
                                       OR r1a.modify_date > r1a.submit_date )
                             THEN 0
                               ELSE 1
                             END)      AS NumOfSubmAgr,
                         Count(r1a.id) AS NumOfAgr,
                         report_id
                  FROM   reports1nf_arenda r1a
                         LEFT JOIN arenda a
                                ON r1a.id = a.id
                  WHERE  a.is_deleted IS NULL
                          OR a.is_deleted = 0
                  GROUP  BY report_id) ar
              ON rep.report_id = ar.report_id
       LEFT JOIN (SELECT Sum(CASE
                               WHEN ( b.submit_date IS NULL
                                       OR b.modify_date IS NULL
                                       OR b.modify_date > b.submit_date ) THEN 0
                               ELSE 1
                             END)  AS NumOfSubmObj,
                         Count(id) AS NumOfObj,
                         report_id
                  FROM   reports1nf_balans b
                  WHERE  is_deleted IS NULL
                          OR is_deleted = 0
                  GROUP  BY report_id) obj
              ON rep.report_id = obj.report_id
WHERE  ( 8888 = 8888 )
       AND CASE
             WHEN rep.zkpo_code IN (SELECT DISTINCT org.zkpo_code
                                    FROM   reports1nf_accounts acc
                                           INNER JOIN aspnet_users usr
                                                   ON usr.userid = acc.userid
                                           INNER JOIN aspnet_membership mem
                                                   ON mem.userid = acc.userid
                                           LEFT OUTER JOIN organizations org
                                                        ON
                                           org.id = acc.organization_id
                                           LEFT OUTER JOIN dict_districts2 rda
                                                        ON rda.id =
                                           rda_district_id
                                           LEFT OUTER JOIN dict_org_old_organ
                                                           misto
                                                        ON
                                           misto.id = misto_district_id
                                   )
           THEN 1
             ELSE 0
           END = 1 

and obj.NumOfObj > 0
and isnull(ddd.name, 'Невідомо') <> 'Невизначені'
) 
select
dict_rent_occupation_name,
count(*) as v3,
sum(case when v4_sum > 0 then 1 else 0 end) as v4,
sum(case when v5_sum > 0 then 1 else 0 end) as v5,

sum(v9) as v6,
sum(v11) as v7,
sum(v9) - sum(v11) as v8,

sum(v9) as v9,
sum(v10) as v10,
sum(v11) as v11,
sum(v12) as v12,

sum(0 + v14_part) as v13,
sum(0 + v14_part) * (sum(v10) / case when sum(v9) = 0 then null else sum(v9) end) * 0.5 as v14,

sum(v15) as v15,

sum(v16) * 0.5 as v16,
sum(v17) * 0.5 as v17,
sum(v18) * 0.5 as v18

from
(
	select
	dict_rent_occupation_name, DG.zkpo_code,
	1 as v3,
	sum(case when DG.has_active_dog = 1 then 1 else 0 end) v4_sum,
	sum(case when DG.has_active_dog = 1 and new_contribution_rate > 0 then 1 else 0 end) v5_sum,

	sum(case when is_active_dogovor = 1 then ""Нараховано орендної плати за звітний період"" else 0 end) as v9,
	sum(case when is_active_dogovor = 1 then ""Надходження орендної плати за звітний період"" else 0 end) as v10,

	sum(case when is_active_dogovor = 1 then ""Нараховано орендної плати за звітний період"" * T.contribution_rate else 0 end) as v11,
	sum(case when is_active_dogovor = 1 then ""Надходження орендної плати за звітний період"" * T.contribution_rate else 0 end) as v12,

	sum(narah_prognoz_year_2026 * new_contribution_rate) as v14_part,
	sum(prognoz_without_borg_2026) as v15,

	sum(narah_prognoz_year_2027 * new_contribution_rate) as v16,
	sum(narah_prognoz_year_2028 * new_contribution_rate) as v17,
	sum(narah_prognoz_year_2029 * new_contribution_rate) as v18

	from
	DG 

	OUTER APPLY (select isnull((select Q.contribution_rate from reports1nf_org_info Q where Q.report_id = DG.report_id),0) as contribution_rate) CR
	OUTER APPLY (select Q.contribution_rate from reports1nf_org_info_new_contribution_rate Q where Q.report_id = DG.report_id) CN
	CROSS APPLY (SELECT top 1 Year(Q.period_end) as cur_year, DATEFROMPARTS(Year(Q.period_end), 1, 1) start_year, Q.* FROM dict_rent_period Q where Q.is_active = 1 order by Q.id desc) PER

	OUTER APPLY
	(
		select
		r.id, r.report_id, rep.zkpo_code, 
		case when r.agreement_state = 1 then 1 else 0 end as is_active_dogovor,
		isnull(P.payment_narah,0) as ""Нараховано орендної плати за звітний період"",
		isnull(P.payment_received,0) as ""Надходження орендної плати за звітний період"",
		(select sum(Q.narah_sum) from reports1nf_payment_narah_prognoz Q where Q.arenda_id = r.id and Q.report_id = r.report_id and year(Q.narah_date) = 2026 and Q.narah_date > PER.period_end) narah_prognoz_year_2026,
		(select sum(Q.narah_sum) from reports1nf_payment_narah_prognoz Q where Q.arenda_id = r.id and Q.report_id = r.report_id and year(Q.narah_date) = 2027 and Q.narah_date > PER.period_end) narah_prognoz_year_2027,
		(select sum(Q.narah_sum) from reports1nf_payment_narah_prognoz Q where Q.arenda_id = r.id and Q.report_id = r.report_id and year(Q.narah_date) = 2028 and Q.narah_date > PER.period_end) narah_prognoz_year_2028,
		(select sum(Q.narah_sum) from reports1nf_payment_narah_prognoz Q where Q.arenda_id = r.id and Q.report_id = r.report_id and year(Q.narah_date) = 2029 and Q.narah_date > PER.period_end) narah_prognoz_year_2029,

		(select sum(Q.narah_sum) from reports1nf_payment_narah_prognoz_without_borg Q where Q.arenda_id = r.id and Q.report_id = r.report_id and year(Q.narah_date) = 2026 and Q.narah_date > PER.period_end) prognoz_without_borg_2026,

		case when CR.contribution_rate > 0 then 1.0 else 0.0 end as contribution_rate,
		case when isnull(CN.contribution_rate,CR.contribution_rate) > 0 then 1.0 else 0.0 end as new_contribution_rate

		FROM reports1nf_arenda r 
		LEFT JOIN arenda a ON r.id = a.id 
		JOIN view_reports1nf rep ON rep.report_id = r.report_id
		OUTER APPLY 
		(
			select	
				top 1
				* 
			from reports1nf_arenda_payments Q 
			where Q.arenda_id = r.id and Q.report_id = r.report_id and Q.rent_period_id = PER.id
		) P

		WHERE 1=1
		and rep.zkpo_code = DG.zkpo_code
		and isnull(a.is_deleted, 0) = 0
		and exists 
		(
			select	
				* 
			from reports1nf_arenda_payments Q 
			where Q.arenda_id = r.id and Q.report_id = r.report_id and Q.rent_period_id = PER.id
		)
	) T
	where DG.report_id in (499,386,410,546)
	group by DG.dict_rent_occupation_name, DG.zkpo_code
	--order by 1,2
) T
group by dict_rent_occupation_name
order by 1
";
		sql = sql.Replace("499,386,410,546", report_id_where);

		//if (!UseInflation)
		//{
		//	sql = sql.Replace("total_cost * I.inflation", "total_cost");
		//}
		//if (!UseDictRentalRate)
		//{
		//	sql = sql.Replace("cost_agreement * koef", "cost_agreement");
		//}
		return sql;
	}

}