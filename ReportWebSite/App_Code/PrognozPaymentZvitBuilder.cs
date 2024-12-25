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

public class PrognozPaymentZvitBuilder
{
	public Page Page { get; set; }
	public bool UseInflation { get; set; }
	public bool UseDictRentalRate { get; set; }
	public int year = 2025;

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



				if (new[] { 6, 7, 8, 9 }.Contains(vnum))
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
		for (int cnum = 3; cnum <= 9; cnum++)
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
select 
--*
--* into #qqq2
dict_rent_occupation_name,
count(*) as v3,
sum(case when COUNT_PAY_NARAH > 0 then 1 else 0 end) as v4,
sum(case when CONTRIBUTION_RATE > 0 then 1 else 0 end) as v5,
isnull(sum(PAY_NARAH_ZVIT * koef_this),0) as v6,
isnull(sum(PAY_50_NARAH * koef_this),0) as v7,
isnull(sum(PAY_50_PAYED * koef_this),0) as v8,
isnull(sum(PROGNOZ_PAY_NARAH),0) as v9
from
(
	SELECT 
        case when rep.zkpo_code in ('02772037','03327664','03346331') then 'Від прибутку згідно з угодой' else isnull(ddd.name, 'Невідомо') end as dict_rent_occupation_name,
		rep.report_id,
		rep.zkpo_code,
		rep.cur_state,
		rep.stan_recieve_date,
		PAY_NARAH_ZVIT, PAY_50_NARAH, PAY_50_PAYED, PROGNOZ_PAY_NARAH, COUNT_DIUCHI, COUNT_PAY_NARAH,

		(select Q.contribution_rate from reports1nf_org_info Q where Q.report_id = rep.report_id) as CONTRIBUTION_RATE,
		K.koef_this,

		(SELECT MAX(sdt) FROM (VALUES
        (rep.bal_max_submit_date),
        (rep.bal_del_max_submit_date),
        (rep.arenda_max_submit_date),
        (rep.arenda_rented_max_submit_date),
        (rep.org_max_submit_date)) AS AllMaxSubmitDates(sdt)) AS 'max_submit_date'


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
        
           
		cross apply (select 2025 yy) Y
		cross apply (select cast(concat(yy,'0101') as date) date_b, cast(concat(yy,'1231') as date) date_e) X
		cross apply (select DATEDIFF(day, X.date_b, X.date_e) + 1 day_cnt) X2
		cross apply (select top 1 isnull(prognoz_inflation_this,1) as inflation_this, isnull(prognoz_inflation_next,1) as inflation_next from current_inflation) I
		cross apply (select 1.0 + 0.33333333 * I.inflation_this as koef_this) K
		outer apply 
		(
			select top 1 Q.contribution_rate from
			(
				select 1 ordnum, Q.contribution_rate from reports1nf_org_info_new_contribution_rate Q where Q.report_id = rep.report_id
					union
				select 2 ordnum, Q.contribution_rate from reports1nf_org_info Q where Q.report_id = rep.report_id
			) Q
			order by Q.ordnum
		) G
			
		LEFT JOIN 
		(
			SELECT
			   org.budget_narah_50_uah AS 'PAY_50_NARAH' -- 29
			   ,kazna.pay_sum AS 'PAY_50_PAYED'-- 30
			   ,[org].[report_id]
			FROM
				reports1nf_org_info org
				LEFT OUTER JOIN kazna_total_info(null, null) kazna on kazna.ident_bal_zkpo = org.zkpo_code
		) OrganizationProperties ON OrganizationProperties.report_id = rep.report_id
            
		LEFT JOIN (SELECT report_id,MAX(rent_period_id) AS 'max_rent_period_id' FROM reports1nf_arenda_payments group by report_id) mrp on mrp.report_id = rep.report_id
            		
		outer apply
		(
			SELECT
				isnull(SUM(pay.PAY_NARAH_ZVIT),0) as 'PAY_NARAH_ZVIT'
				,isnull(sum(pay.PROGNOZ_PAY_NARAH),0) as 'PROGNOZ_PAY_NARAH'
				,count(case when agreement_state = 1 then 1 else 0 end) as 'COUNT_DIUCHI'
				,count(case when pay.PAY_NARAH_ZVIT > 0 then 1 else 0 end) as 'COUNT_PAY_NARAH'
				,pay.report_id
				,rent_period_id                
            FROM 
			(
				select 
					(Q.PAY_NARAH_ZVIT * K.koef_this * I.inflation_next) 
						* (case when dogovor_day_cnt >= X2.day_cnt then 1 else cast(dogovor_day_cnt as decimal) / cast(X2.day_cnt as decimal) end)
						* (case when agreement_state = 1 then 1 else 0 end)
						* (isnull(G.contribution_rate,0) / 100.0)
					as PROGNOZ_PAY_NARAH, 
					Q.*
				from 
				(
					select 
						isnull(U.payment_narah,0) - isnull(U.znyato_nadmirno_narah,0) as 'PAY_NARAH_ZVIT',
						dogovor_day_cnt,
						agreement_state,
						U.* 
					from reports1nf_arenda_payments U
					left join
					(
						select
						case when rent_start < rent_finish then DATEDIFF(day, rent_start, rent_finish) + 1 else 0 end as dogovor_day_cnt,
						*
						from
						(
							select 
								case when Q.rent_start_date >= X.date_b then Q.rent_start_date else X.date_b end rent_start,
								case when Q.rent_finish_date <= X.date_e then Q.rent_finish_date else X.date_e end rent_finish,
								*
							from reports1nf_arenda Q
						) Q
					) ar ON ar.id = U.arenda_id and ar.report_id = U.report_id
				) Q
			) pay
			
            WHERE NOT EXISTS(SELECT id FROM reports1nf_arenda a WHERE a.id = pay.arenda_id AND ISNULL(a.is_deleted, 0) = 1)  and pay.arenda_id > 0
			and pay.report_id = rep.report_id and rent_period_id = mrp.max_rent_period_id
					--and pay.arenda_id in (78131)
            GROUP BY pay.report_id,pay.rent_period_id
			) RentPaymentProperties1 
            

		LEFT OUTER JOIN (
select obp.org_id,occ.name from org_by_period obp
join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
join dict_rent_occupation occ on occ.id = obp.org_occupation_id
		) DDD ON DDD.org_id = rep.organization_id

) as T
where 1=1
and cur_state = 'Надісланий'
and max_submit_date >= (select DATEADD(day, 1, Q.period_end) FROM dict_rent_period Q where Q.is_active = 1)
and stan_recieve_date >= (select DATEADD(day, 1, Q.period_end) FROM dict_rent_period Q where Q.is_active = 1)
	--and isnull(COUNT_DIUCHI,0) <> isnull(COUNT_PAY_NARAH,0)
group by dict_rent_occupation_name
order by 1

--select rent_start_date, rent_finish_date, agreement_state, * from reports1nf_arenda where id = 78131
--update reports1nf_arenda set rent_start_date = '2019-09-19', rent_finish_date = '2025-07-01', agreement_state = 1 where id = 78131

--select 122056.480 + (122056.480 * 0.25 * 1.060) = 154401.44720000
--select 154401.44720000 * 1.230
--select 189913.78005600000 * 0.3 = 56974.134016800000
--select 56974.134016800000
--select DATEDIFF(day, '2025-01-01', '2025-07-01') + 1
--select 182.0 / 365.0 * 56974.134016800000 = 28409.012444796984000000

--select zkpo_code, count(*) from #qqq2 group by zkpo_code having COUNT(*) > 1
--alter table #qqq2 add unique(zkpo_code)
--alter table #qqq2 add unique(report_id)

";

		sql = sql.Replace("2025", "" + year);
		sql = sql.Replace("0.33333333", "0.33333333");	//!!! тут надо думать каждый раз прежде чем менять !!!
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