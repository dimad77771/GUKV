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

public class PlanZvitBuilder
{
	public Page Page { get; set; }
	public bool UseInflation { get; set; }
	public bool UseDictRentalRate { get; set; }
	public int year = 2025;

	public void Go()
	{
		string templateFileName = Page.Server.MapPath("Templates/plan_zvit.xlsx");
		var tempFile = TempFile.FromExistingFile(templateFileName);

		var connection = CommonUtils.ConnectToDatabase();
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



				if (new[] { 5, 6 }.Contains(vnum))
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

		var text = "Прогнозні показники надходжень від оренди та перерахування її частини до бюджету у " + year + " р." + "\n" +
				"(використовувати індекс інфляції: " + (UseInflation ? "так" : "ні") + "; " + 
				" використовувати нову ставку за використання: " + (UseDictRentalRate ? "так" : "ні") + ")";
		wsheet["A1"].Value = text;
		wsheet["F1"].Value = "Друком на:\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm");


		workbook.SaveDocument(tempFile.FileName);


		var info = new System.IO.FileInfo(tempFile.FileName);
		Page.Response.Clear();
		Page.Response.ClearHeaders();
		Page.Response.ClearContent();
		Page.Response.ContentType = "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		Page.Response.AddHeader("content-disposition", "attachment; filename=plan_zvit_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx; size=" + info.Length.ToString());
		using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
		{
			stream.CopyTo(Page.Response.OutputStream);
		}
		tempFile.Dispose();
		Page.Response.End();
	}

	void SumBuild(int erow_total, int[] erows_sum, Worksheet wsheet)
	{
		for (int cnum = 3; cnum <= 6; cnum++)
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
dict_rent_occupation_name,
count(distinct report_id) as v3,
count(distinct case when T.contribution_rate > 0 then T.report_id end) as v4,
cast(isnull(round(sum(total_cost),2),0) as decimal(38,2)) as v5,
cast(isnull(round(sum(total_cost * (contribution_rate / 100.0)),2),0) as decimal(38,2)) as v6
from
(
	select
	dict_rent_occupation_name,
	T.report_id,
	L.contribution_rate,
	G.id as arenda_id,
	M.total_cost
	from
	(
		select
		case when rep.zkpo_code in ('02772037','03327664','03346331') then 'Від прибутку згідно з угодой' else isnull(ddd.name, 'Невідомо') end as [dict_rent_occupation_name],
		rep.report_id
		FROM view_reports1nf rep
		LEFT OUTER JOIN (
		select obp.org_id,occ.name from org_by_period obp
		join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
		join dict_rent_occupation occ on occ.id = obp.org_occupation_id
		) DDD ON DDD.org_id = rep.organization_id
		--where rep.report_id = 265
	) T
	cross apply (select 2025 yy) Y
	cross apply (select cast(concat(yy,'0101') as date) date_b, cast(concat(yy,'1231') as date) date_e) X
	cross apply (select DATEDIFF(day, X.date_b, X.date_e) + 1 day_cnt) X2
	cross apply (select top 1 isnull(next_inflation,1) as inflation from current_inflation) I
	outer apply (select top 1 Q.contribution_rate from reports1nf_org_info Q where Q.report_id = T.report_id) L
	outer apply
	(
		select
		*
		from
		(
			select
			case when rent_start < rent_finish then DATEDIFF(day, rent_start, rent_finish) + 1 else 0 end rent_days,
			A.*
			from
			(
				select 
				ar.id,
				case when ar.rent_start_date >= X.date_b then ar.rent_start_date else X.date_b end rent_start,
				case when ar.rent_finish_date <= X.date_e then ar.rent_finish_date else X.date_e end rent_finish,
				ar.rent_start_date,
				ar.rent_finish_date
				FROM reports1nf_arenda ar
				INNER JOIN reports1nf rep ON rep.id = ar.report_id
				LEFT OUTER JOIN arenda a ON a.id = ar.id
				WHERE ar.report_id = T.report_id
					AND (isnull(a.is_deleted, 0) = 0 OR 0 = 1)
					AND ar.agreement_state = 1
			) A
		) A where A.rent_days > 0
	) G
	outer apply 
	(
		select
		total_cost * I.inflation as total_cost,
		total_cost as total_cost0
		from
		(
			select
			case when rent_days = X2.day_cnt then month_cost * 12 else month_cost * 12 * (cast(rent_days as decimal(38,2)) / X2.day_cnt) end as total_cost
			,month_cost * 12 as total_cost2
			from
			(
				select 
					sum(cost_agreement * koef) as month_cost
				from
				(
					SELECT 
					A.cost_agreement, 
					(100 + isnull(isnull(B.new_rental_rate,B.rental_rate),0)) / (100 + isnull(B.rental_rate,0)) as koef,
					B.rental_rate,
					B.new_rental_rate,
					A.payment_type_id
					FROM reports1nf_arenda_notes A
					left join dict_rental_rate B on B.id = A.payment_type_id
					WHERE (A.is_deleted IS NULL OR A.is_deleted = 0) 
						AND A.report_id = T.report_id AND A.arenda_id = G.id
				) A
			) A
		) A
	) M
	--order by report_id
) T
group by dict_rent_occupation_name
";
		
		sql = sql.Replace("2025", "" + year);
		if (!UseInflation)
		{
			sql = sql.Replace("total_cost * I.inflation", "total_cost");
		}
		if (!UseDictRentalRate)
		{
			sql = sql.Replace("cost_agreement * koef", "cost_agreement");
		}
		return sql;
	}

}