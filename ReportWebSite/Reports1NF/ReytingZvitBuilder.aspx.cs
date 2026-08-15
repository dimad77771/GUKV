using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using DevExpress.Spreadsheet;
using GUKV.Common;

public partial class Reports1NF_Cabinet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var builder = new ReytingZvitBuilder
		{
			Page = this,
			Details = string.Equals(Request.QueryString["details"], "1", StringComparison.Ordinal),
		};
		builder.Go();
	}
}

public class ReytingZvitBuilder
{
	static readonly string[] Codes = { "001", "003", "004", "005", "006", "007", "008", "111", "131", "210", "230", "220", "280", "270", "250", "240", "260", "140" };

	const int DistrictStartRowIndex = 4;
	const int DistrictCount = 10;
	const int TotalRowIndex = 14;
	const int StartDataColumnIndex = 2;
	const int IntegratedColumnIndex = 20;
	const int DistrictRankColumnIndex = 21;
	const int HolderRankColumnIndex = 22;

	public Page Page { get; set; }
	public bool Details { get; set; }

	public void Go()
	{
		string templateFileName = Page.Server.MapPath("Templates/reyting_zvit.xlsx");
		using (var tempFile = TempFile.FromExistingFile(templateFileName))
		{
			var dataTable = LoadData();
			BuildWorkbook(tempFile.FileName, dataTable);

			var fileName = "Рейтинги РДА.xlsx";
			Page.Response.Clear();
			Page.Response.ClearHeaders();
			Page.Response.ClearContent();
			Page.Response.ContentType = "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			Page.Response.AddHeader(
				"Content-Disposition",
				"attachment; filename*=UTF-8''" + Uri.EscapeDataString(fileName));
			using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
			{
				stream.CopyTo(Page.Response.OutputStream);
			}
		}
		Page.Response.End();
	}

	DataTable LoadData()
	{
		var connection = CommonUtils.ConnectToDatabase();
		if (connection == null) throw new Exception("Database GUKV not found");

		using (connection)
		{
			var factory = DbProviderFactories.GetFactory(connection);
			var dataTable = new DataTable();
			using (var cmd = factory.CreateCommand())
			{
				cmd.CommandText = GetMainSql();
				cmd.CommandType = CommandType.Text;
				cmd.CommandTimeout = 180;
				cmd.Connection = connection;
				using (var adapter = factory.CreateDataAdapter())
				{
					adapter.SelectCommand = cmd;
					adapter.Fill(dataTable);
				}
			}
			return dataTable;
		}
	}

	public void BuildWorkbook(string fileName, DataTable dataTable)
	{
		using (var workbook = new Workbook())
		{
			workbook.LoadDocument(fileName, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
			var wsheet = workbook.Worksheets[0];

			workbook.BeginUpdate();
			try
			{
				var holderRows = ReadHolderRows(dataTable);
				var model = BuildReportModel(wsheet, holderRows);

				WriteBaseWorksheet(wsheet, model);
				if (Details)
				{
					WriteDetailsWorksheet(wsheet, model);
				}

				wsheet["R1"].Value = "Станом на: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
			}
			finally
			{
				workbook.EndUpdate();
			}

			workbook.SaveDocument(fileName);
		}
	}

	List<HolderRating> ReadHolderRows(DataTable dataTable)
	{
		var holders = new List<HolderRating>();
		foreach (DataRow row in dataTable.Rows)
		{
			var values = CreateRatingValues();
			foreach (var code in Codes)
			{
				values.Metrics[code].Value0 = GetValue(row, "v" + code + "_0");
				values.Metrics[code].Value1 = GetValue(row, "v" + code + "_1");
			}
			values.IntegratedScore = GetIntegratedScore(values);

			var district = GetString(row, "addr_district");
			holders.Add(new HolderRating
			{
				ReportId = GetInt(row, "report_id"),
				District = district,
				DistrictKey = NormalizeDistrictName(district),
				ZkpoCode = GetString(row, "zkpo_code"),
				FullName = GetString(row, "full_name"),
				Values = values,
			});
		}
		return holders;
	}

	ReportRatingModel BuildReportModel(Worksheet wsheet, List<HolderRating> holders)
	{
		var model = new ReportRatingModel();
		var holdersByDistrict = holders
			.Where(x => !string.IsNullOrEmpty(x.DistrictKey))
			.GroupBy(x => x.DistrictKey)
			.ToDictionary(x => x.Key, x => x.ToList(), StringComparer.OrdinalIgnoreCase);
		var matchedHolders = new List<HolderRating>();
		var templateDistrictKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		for (int districtIndex = 0; districtIndex < DistrictCount; districtIndex++)
		{
			var rowIndex = DistrictStartRowIndex + districtIndex;
			var districtName = wsheet[rowIndex, 1].Value.TextValue ?? "";
			var districtKey = NormalizeDistrictName(districtName);
			if (string.IsNullOrEmpty(districtKey) || !templateDistrictKeys.Add(districtKey))
			{
				throw new InvalidOperationException("The report template contains an empty or duplicate district row.");
			}
			List<HolderRating> districtHolders;
			if (!holdersByDistrict.TryGetValue(districtKey, out districtHolders))
			{
				districtHolders = new List<HolderRating>();
			}

			var district = new DistrictRating
			{
				TemplateOrder = districtIndex,
				TemplateRowIndex = rowIndex,
				DistrictKey = districtKey,
				Holders = districtHolders,
				HasData = districtHolders.Count > 0,
				Values = AggregateRatingValues(districtHolders),
			};
			RankHolders(district);
			model.Districts.Add(district);
			matchedHolders.AddRange(districtHolders);
		}

		var matchedReportIds = new HashSet<int>(matchedHolders.Select(x => x.ReportId));
		var unmatchedHolders = holders.Where(x => !matchedReportIds.Contains(x.ReportId)).ToList();
		if (unmatchedHolders.Count > 0)
		{
			var unknownDistricts = string.Join(", ", unmatchedHolders
				.Select(x => string.IsNullOrEmpty(x.District) ? "report_id: " + x.ReportId : x.District)
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.Take(10));
			throw new InvalidOperationException("Some balance holders do not match a district in the report template: " + unknownDistricts);
		}

		model.Total = AggregateRatingValues(matchedHolders);
		RankDistricts(model.Districts);
		return model;
	}

	RatingValues AggregateRatingValues(IEnumerable<HolderRating> holders)
	{
		var result = CreateRatingValues();
		foreach (var holder in holders)
		{
			foreach (var code in Codes)
			{
				result.Metrics[code].Value0 += holder.Values.Metrics[code].Value0;
				result.Metrics[code].Value1 += holder.Values.Metrics[code].Value1;
			}
		}
		result.IntegratedScore = GetIntegratedScore(result);
		return result;
	}

	RatingValues CreateRatingValues()
	{
		var values = new RatingValues();
		foreach (var code in Codes)
		{
			values.Metrics.Add(code, new MetricCounts());
		}
		return values;
	}

	void RankDistricts(IEnumerable<DistrictRating> districts)
	{
		var ranked = districts
			.Where(x => x.HasData)
			.OrderByDescending(x => x.Values.IntegratedScore)
			.ThenBy(x => x.TemplateOrder)
			.ToList();
		for (int index = 0; index < ranked.Count; index++)
		{
			ranked[index].Rank = index + 1;
		}
	}

	void RankHolders(DistrictRating district)
	{
		district.Holders = district.Holders
			.OrderByDescending(x => x.Values.IntegratedScore)
			.ThenBy(x => string.IsNullOrEmpty(x.FullName) ? 1 : 0)
			.ThenBy(x => x.FullName, StringComparer.OrdinalIgnoreCase)
			.ThenBy(x => string.IsNullOrEmpty(x.ZkpoCode) ? 1 : 0)
			.ThenBy(x => x.ZkpoCode, StringComparer.Ordinal)
			.ThenBy(x => x.ReportId)
			.ToList();
		for (int index = 0; index < district.Holders.Count; index++)
		{
			district.Holders[index].RankWithinDistrict = index + 1;
		}
	}

	void WriteBaseWorksheet(Worksheet wsheet, ReportRatingModel model)
	{
		wsheet["B2"].Value = "Район";
		foreach (var district in model.Districts.Where(x => x.HasData))
		{
			WritePercentages(wsheet, district.TemplateRowIndex, district.Values);
			wsheet[district.TemplateRowIndex, IntegratedColumnIndex].Value = district.Values.IntegratedScore;
			wsheet[district.TemplateRowIndex, DistrictRankColumnIndex].Value = district.Rank;
		}
		WritePercentages(wsheet, TotalRowIndex, model.Total);
	}

	void WriteDetailsWorksheet(Worksheet wsheet, ReportRatingModel model)
	{
		PrepareDetailsLayout(wsheet);

		for (int districtIndex = model.Districts.Count - 1; districtIndex >= 0; districtIndex--)
		{
			var district = model.Districts[districtIndex];
			if (district.Holders.Count == 0)
			{
				wsheet[district.TemplateRowIndex, 1].Font.Bold = true;
				continue;
			}

			var insertRowIndex = district.TemplateRowIndex + 1;
			wsheet.Rows.Insert(insertRowIndex, district.Holders.Count, RowFormatMode.FormatAsPrevious);
			wsheet[district.TemplateRowIndex, 1].Font.Bold = true;
			for (int holderIndex = 0; holderIndex < district.Holders.Count; holderIndex++)
			{
				var holder = district.Holders[holderIndex];
				var rowIndex = insertRowIndex + holderIndex;
				wsheet[rowIndex, 1].Value = GetHolderLabel(holder);
				wsheet[rowIndex, 1].Font.Bold = false;
				wsheet[rowIndex, 1].Alignment.WrapText = true;
				wsheet[rowIndex, 1].Alignment.ShrinkToFit = false;
				WritePercentages(wsheet, rowIndex, holder.Values);
				wsheet[rowIndex, IntegratedColumnIndex].Value = holder.Values.IntegratedScore;
				wsheet[rowIndex, HolderRankColumnIndex].Value = holder.RankWithinDistrict;
			}
			wsheet.Rows.AutoFit(insertRowIndex, insertRowIndex + district.Holders.Count - 1);
		}
	}

	string GetHolderLabel(HolderRating holder)
	{
		if (!string.IsNullOrEmpty(holder.ZkpoCode) && !string.IsNullOrEmpty(holder.FullName))
		{
			return holder.ZkpoCode + " - " + holder.FullName;
		}
		if (!string.IsNullOrEmpty(holder.ZkpoCode))
		{
			return holder.ZkpoCode;
		}
		if (!string.IsNullOrEmpty(holder.FullName))
		{
			return holder.FullName;
		}
		return "report_id: " + holder.ReportId;
	}

	void PrepareDetailsLayout(Worksheet wsheet)
	{
		wsheet.UnMergeCells(wsheet.Range["R1:V1"]);
		wsheet["W1"].CopyFrom(wsheet["V1"], PasteSpecial.Formats);
		wsheet.MergeCells(wsheet.Range["R1:W1"]);
		wsheet["W2"].CopyFrom(wsheet["V2"], PasteSpecial.Formats);
		wsheet["W3"].CopyFrom(wsheet["V3"], PasteSpecial.Formats);
		wsheet.MergeCells(wsheet.Range["W2:W3"]);
		wsheet["W4"].CopyFrom(wsheet["V4"], PasteSpecial.Formats);
		wsheet.Range["W5:W15"].CopyFrom(wsheet.Range["V5:V15"], PasteSpecial.Formats);
		wsheet.Columns["W"].Width = wsheet.Columns["V"].Width;
		wsheet["B2"].Value = "Район / Балансоутримувач";
		wsheet["W2"].Value = "Місце балансоутримувача у районі";
		wsheet.Columns["B"].WidthInCharacters = 46;
		wsheet.PrintOptions.FitToPage = true;
		wsheet.PrintOptions.FitToWidth = 1;
		wsheet.PrintOptions.FitToHeight = 0;
	}

	void WritePercentages(Worksheet wsheet, int rowIndex, RatingValues values)
	{
		for (int codeIndex = 0; codeIndex < Codes.Length; codeIndex++)
		{
			var metric = values.Metrics[Codes[codeIndex]];
			wsheet[rowIndex, StartDataColumnIndex + codeIndex].Value = GetPercent(metric);
		}
	}

	decimal GetIntegratedScore(RatingValues values)
	{
		return Codes.Sum(code => GetPercent(values.Metrics[code]) * GetWeight(code)) / Codes.Length;
	}

	decimal GetPercent(MetricCounts metric)
	{
		var total = metric.Value0 + metric.Value1;
		return total > 0 ? metric.Value1 / total * 100.0M : 0M;
	}

	decimal GetValue(DataRow row, string column)
	{
		var value = row[column];
		return value is DBNull ? 0M : Convert.ToDecimal(value);
	}

	int GetInt(DataRow row, string column)
	{
		var value = row[column];
		return value is DBNull ? 0 : Convert.ToInt32(value);
	}

	string GetString(DataRow row, string column)
	{
		var value = row[column];
		return value is DBNull ? "" : value.ToString().Trim();
	}

	string NormalizeDistrictName(string value)
	{
		return (value ?? "").ToLower().Replace(" район", "").Trim();
	}

	class MetricCounts
	{
		public decimal Value0 { get; set; }
		public decimal Value1 { get; set; }
	}

	class RatingValues
	{
		public RatingValues()
		{
			Metrics = new Dictionary<string, MetricCounts>();
		}

		public Dictionary<string, MetricCounts> Metrics { get; private set; }
		public decimal IntegratedScore { get; set; }
	}

	class HolderRating
	{
		public int ReportId { get; set; }
		public string District { get; set; }
		public string DistrictKey { get; set; }
		public string ZkpoCode { get; set; }
		public string FullName { get; set; }
		public RatingValues Values { get; set; }
		public int RankWithinDistrict { get; set; }
	}

	class DistrictRating
	{
		public int TemplateOrder { get; set; }
		public int TemplateRowIndex { get; set; }
		public string DistrictKey { get; set; }
		public List<HolderRating> Holders { get; set; }
		public RatingValues Values { get; set; }
		public int Rank { get; set; }
		public bool HasData { get; set; }
	}

	class ReportRatingModel
	{
		public ReportRatingModel()
		{
			Districts = new List<DistrictRating>();
		}

		public List<DistrictRating> Districts { get; private set; }
		public RatingValues Total { get; set; }
	}

	decimal GetWeight(string code)
	{
		if (code == "001") return 1.0M;
		else if (code == "003") return 1.0M;
		else if (code == "004") return 1.0M;
		else if (code == "023") return 1.0M;
		else if (code == "005") return 1.0M;
		else if (code == "006") return 1.0M;
		else if (code == "007") return 1.0M;
		else if (code == "008") return 1.0M;
		else if (code == "111") return 1.0M;
		else if (code == "131") return 1.0M;
		else if (code == "210") return 1.0M;
		else if (code == "230") return 1.0M;
		else if (code == "220") return 1.0M;
		else if (code == "280") return 1.0M;
		else if (code == "270") return 1.0M;
		else if (code == "250") return 1.0M;
		else if (code == "240") return 1.0M;
		else if (code == "260") return 1.0M;
		else if (code == "140") return 1.0M;
		else throw new Exception(code);
	}



	string GetMainSql()
	{
		return @"
drop table if exists #reports

declare @period_year int = year(getdate()) - case when month(getdate()) = 1 then 1 else 0 end
declare @p_rda_district_id int = 0
declare @p_misto_id int = 0
declare @smode int = 0
declare @p_show_neziznacheni int = 1
declare @p_show_neviznacheni int = 0
declare @show_num_problem_dog int = 1

    
drop table if exists #arenda_payment_problems;
select * into #arenda_payment_problems from arenda_payment_problems;
    
SELECT 
	rep.report_id,
	rep.addr_district,
	rep.zkpo_code,
	rep.full_name,
	case when inventar_recieve_date is not null and inventar_recieve_date >= w.year_minus_1 then 1 else 0 end as v111,
	case when director_title <> '' then 1 else 0 end as v131,
	director_title,
	inventar_recieve_date,
	isnull(num_given,0) as num_given,
	isnull(num_problem_dog,0) as num_problem_dog
        
into #reports

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
		,SUM(bal.sqr_vlas_potreb) AS 'SQR_VLAS_POTREB'
	    ,SUM(case when bal.sqr_vlas_potreb > 0 then 1 else 0 end) AS 'SQR_VLAS_POTREB_COUNT'
--            ,SUM(CASE WHEN bal.is_free_sqr = 1 THEN bal.free_sqr_useful ELSE 0 END) AS 'SQR_FREE'
--            ,SUM(bal.free_sqr_useful) AS 'SQR_FREE'
            ,sum(fs.sqr_free) AS 'SQR_FREE'
			,sum(fs.sqr_free_count) AS 'SQR_FREE_COUNT'
        ,COUNT(*) AS 'NUM_BALANS'
        ,bal.report_id
    FROM reports1nf_balans bal
    outer apply (select sum(fs.total_free_sqr) as sqr_free, count(*) as sqr_free_count from reports1nf_balans_free_square fs where fs.balans_id = bal.id and fs.report_id = bal.report_id and fs.is_included = 1 and fs.total_free_sqr > 0) fs  
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
            ,case when @show_num_problem_dog = 1 then sum(NUM_PROBLEM_DOG) else 0 end as 'NUM_PROBLEM_DOG'
--        SELECT R.report_id, SUM(R.row_count) AS 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN'
--         SELECT R.report_id, SUM(NUM_GIVEN)  as 'NUM_GIVEN', SUM(R.SQR_GIVEN) AS 'SQR_GIVEN'
        FROM (
        SELECT 
	SUM(ar.rent_square) AS 'SQR_GIVEN'
           	,COUNT(ar.id) AS 'NUM_GIVEN'
           	,1 AS 'row_count'
            	,ar.report_id
	,ar.org_renter_id
    ,sum(case when pr.[is_problem] = 1 then 1 else 0 end) as 'NUM_PROBLEM_DOG'
	FROM reports1nf_arenda ar
    LEFT JOIN #arenda_payment_problems pr on pr.arenda_id = ar.id and pr.report_id = ar.report_id
	WHERE (ar.is_deleted IS NULL OR ar.is_deleted = 0)
		AND NOT EXISTS(SELECT id FROM arenda a WHERE a.id = ar.id AND ISNULL(a.is_deleted, 0) = 1)
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

        --,org.budget_zvit_50_uah AS 'PAY_50_PAYED'-- 30
        --,dbo.[get_kazna_total](org.zkpo_code,null,null) AS 'PAY_50_PAYED'-- 30 
        ,kazna.pay_sum AS 'PAY_50_PAYED'

        ,org.budget_prev_50_uah AS 'PAY_50_DEBT'-- 31
        ,org.budget_debt_30_50_uah AS 'PAY_50_DEBT_OLD' -- 32
        ,org.payment_budget_special AS 'PAY_SPECIAL'-- 33
        --,org.konkurs_payments -- 34
        --,org.unknown_payments -- 35
        --,CASE WHEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) > 0 THEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(org.budget_zvit_50_uah, 0)) ELSE 0 END AS 'PAY_50_DEBT_CUR'
        ,CASE WHEN (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(kazna.pay_sum, 0) + ISNULL(org.budget_prev_50_uah, 0)) - ISNULL(org.unknown_payments,0) < 0 THEN 0 Else (ISNULL(org.budget_narah_50_uah, 0) - ISNULL(kazna.pay_sum, 0) + ISNULL(org.budget_prev_50_uah, 0)) - ISNULL(org.unknown_payments,0) END AS 'PAY_50_DEBT_CUR'
        ,org.konkurs_payments AS 'PAY_RECV_OTHER'
        ,[org].[report_id]
        ,[dict_otdel_gukv].name as 'otdel_gukv'
        ,[org].[prim_balanc]
        ,org.unknown_payments AS 'PAY_UNKNOWN_PAYMENTS'

        ,org.planuvania_1
        ,org.planuvania_2
        ,org.planuvania_3
        ,org.planuvania_4
        ,org.planuvania_5
        ,org.corporav_prava

        --,[dbo].[get_conveyancingRequests_count]([org].[report_id]) AS conveyancingRequests_count
        ,isnull(view_conveyancingRequests_count.cnt,0) AS conveyancingRequests_count

    FROM
        reports1nf_org_info org
        LEFT OUTER JOIN view_conveyancingRequests_count on view_conveyancingRequests_count.report_id = [org].[report_id]
        LEFT OUTER JOIN kazna_total_info(null, null) kazna on kazna.ident_bal_zkpo = org.zkpo_code
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
            isnull(SUM(pay.payment_narah),0) - isnull(SUM(pay.znyato_nadmirno_narah),0) as 'PAY_NARAH_ZVIT' -- 4
            ,SUM(pay.last_year_saldo) as 'PAY_PEREPLATA' -- 5
            ,SUM(pay.zabezdepoz_prishlo) as 'PAY_ZABEZDEPOZ_PRISHLO' -- 5
            ,SUM(pay.payment_received) as 'PAY_RECV_ZVIT' -- 6
            ,SUM(pay.payment_nar_zvit) as 'PAY_RECV_NARAH' -- 7
            --,SUM(pay.payment_budget_special) as 'payment_budget_special' -- 8
            ,SUM(pay.debt_total) as 'PAY_DEBT_TOTAL' -- 9
            ,SUM(pay.debt_zvit) as 'PAY_DEBT_ZVIT' -- 10
            ,SUM(pay.debt_3_month) as 'PAY_debt_3_month' -- 11
            ,SUM(pay.debt_12_month) as 'PAY_debt_12_month' -- 12
            ,SUM(pay.debt_3_years) as 'PAY_debt_3_years' -- 13
            ,SUM(pay.debt_over_3_years) as 'PAY_debt_over_3_years' -- 14
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
            ,SUM(pay.zabezdepoz_saldo) as 'PAY_AVANCE_SALDO' -- 25
            ,SUM(pay.avance_plat) as 'PAY_AVANCE_PLAT' -- 26
            ,SUM(pay.total_pereplata) as 'PAY_PEREPLATA_ALL' -- 27
            ,SUM(pay.avance_debt) as 'PAY_AVANCE_DEBT' -- 28
            ,SUM(pay.avance_paymentnar) as 'PAY_AVANCE_PAYMENTNAR' -- 28
            ,SUM(pay.return_orend_payed) as 'PAY_RETURN_OREND_PAYED' -- 28
            ,SUM(pay.znyato_from_avance) as 'PAY_ZNYATO_FROM_AVANCE' -- 28
            ,SUM(pay.return_all_orend_payed) as 'PAY_RETURN_ALL_OREND_PAYED' -- 28
			,report_id
			,rent_period_id
        FROM reports1nf_arenda_payments pay
        WHERE NOT EXISTS(SELECT id FROM arenda a WHERE a.id = pay.arenda_id AND ISNULL(a.is_deleted, 0) = 1)  and pay.arenda_id > 0
        GROUP BY pay.report_id,pay.rent_period_id) RentPaymentProperties1 ON RentPaymentProperties1.report_id = rep.report_id and RentPaymentProperties1.rent_period_id = mrp.max_rent_period_id
            
    LEFT JOIN (
    SELECT
        SUM(pay.cmk_sqr_rented) as 'PAY_CMK_SQR'
        ,SUM(pay.cmk_payment_narah) as 'PAY_CMK_NARAH'
        ,SUM(pay.cmk_payment_to_budget) as 'PAY_CMK_BUDGET'
        ,SUM(pay.cmk_rent_debt) as 'PAY_CMK_DEBT'
        ,pay.report_id
    FROM reports1nf_arenda_rented pay
	WHERE pay.is_cmk > 0 and ISNULL(pay.is_deleted, 0) = 0
    GROUP BY pay.report_id) RentPaymentProperties2 ON RentPaymentProperties2.report_id = rep.report_id

	LEFT OUTER JOIN (
select obp.org_id,occ.name from org_by_period obp
join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
join dict_rent_occupation occ on occ.id = obp.org_occupation_id
	) DDD ON DDD.org_id = rep.organization_id
CROSS APPLY (select cast(concat(year(getdate()) - 1,'0101') as date) year_minus_1) W


    WHERE 

	form_of_ownership = 'КОМУНАЛЬНА (СФЕРА УПРАВЛІННЯ РДА)'
	AND

        (@p_rda_district_id = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @p_rda_district_id))
            AND
        (@p_misto_id = 0 OR rep.old_organ_id = @p_misto_id)
			AND
		( (@smode = 0) OR (@smode = 1 and obj.NumOfObj > 0) OR (@smode = 2 and isnull(obj.NumOfObj,0) <= 0) )
            AND
        ( (@p_show_neziznacheni = 1) OR (@p_show_neziznacheni = 0 AND (isnull(ddd.name, 'Невідомо') <> 'Невизначені')) ) 
            AND
        ( (@p_show_neviznacheni = 1) OR (@p_show_neviznacheni = 0 AND (isnull(ddd.name, '') not in ('Невизначені','АРХИВНІ(ПРИПИНЕНІ)'))) ) 
            AND
            
		(
			(
				--isnull(ddd.name, 'Невідомо') <> 'Невизначені' and 
				obj.NumOfObj >= 1
			)
			or
			(
				--isnull(ddd.name, 'Невідомо') = 'Невизначені' and
				(
					PAY_DEBT_TOTAL >= 1.0 or
					PAY_50_DEBT_CUR >= 1.0 or
					PAY_50_DEBT_OLD >= 1.0 
				)
			)	
		)
            AND
case when rep.zkpo_code in 
(
SELECT
distinct
org.zkpo_code
FROM reports1nf_accounts acc
INNER JOIN aspnet_Users usr ON usr.UserId = acc.UserId
INNER JOIN aspnet_Membership mem ON mem.UserId = acc.UserId
LEFT OUTER JOIN organizations org ON org.id = acc.organization_id
LEFT OUTER JOIN dict_districts2 rda ON rda.id = rda_district_id
LEFT OUTER JOIN dict_org_old_organ misto ON misto.id = misto_district_id
) then 1 else 0 end = 1
         
alter table #reports add primary key(report_id)


drop table if exists #arenda

select
ar.report_id, ar.id as arenda_id

,case when ar.modify_date is not null and ar.modify_date >= W.kvartal_minus_1 then 1 else 0 end as v210
,case when U.ex_ref_balans_id > 0 then 1 else 0 end as v220
,case when exists (select 1 from reports1nf_arendaphotos Q where Q.arenda_id = ar.id) then 1 else 0 end as v230
,case when U.ex_purpose_group_id > 0 then 1 else 0 end as v240
,case when U.ex_payment_type_id > 0 then 1 else 0 end as v250
,case when U.ex_cost_agreement > 0 then 1 else 0 end as v260
,case when form_gosp = 'БЮДЖЕТНА' then 999 when ar.is_insured = 1 and ar.insurance_end is not null and ar.insurance_end >= today then 1 else 0 end as v270
,case when ar.payment_type_id in (8,3) then 999 when (I.max_payment_date >= plat_month_minus_2) then 1 else 0 end as v280

into #arenda

FROM reports1nf_arenda ar
INNER JOIN #reports rep ON rep.report_id = ar.report_id
LEFT OUTER JOIN arenda a ON a.id = ar.id
CROSS APPLY
(
	select max(Q.payment_date) as max_payment_date from reports1nf_payment_documents Q where Q.report_id = ar.report_id and Q.arenda_id = ar.id
) I
OUTER APPLY
(
	select 
		TOP 1 * 
	from organizations org_renter 
	where org_renter.id = ar.org_renter_id and (org_renter.is_deleted is null or org_renter.is_deleted = 0)
) K
OUTER APPLY
(
	select top 1 * FROM view_reports1nf Q where Q.zkpo_code = K.zkpo_code
) N
CROSS APPLY
(
	select 
		sum(case when ref_balans_id is not null then 1 else 0 end) as ex_ref_balans_id,
		sum(case when purpose_group_id is not null then 1 else 0 end) as ex_purpose_group_id,
		sum(case when payment_type_id is not null then 1 else 0 end) as ex_payment_type_id,
		sum(case when cost_agreement is not null then 1 else 0 end) as ex_cost_agreement
	from reports1nf_arenda_notes Q where (Q.is_deleted IS NULL OR Q.is_deleted = 0) AND Q.report_id = ar.report_id AND Q.arenda_id = ar.id
) U
CROSS APPLY 
(
	select 
	cast(now as date) as today,
	DATEADD(MONTH,-3,
	cast(concat(
		year(now),
		case 
			when MONTH(now) in (1,2,3) then '01'
			when MONTH(now) in (4,5,6) then '04'
			when MONTH(now) in (7,8,9) then '07'
			when MONTH(now) in (10,11,12) then '10'
		end,
		'01') as date)) as kvartal_minus_1,
	DATEADD(MONTH,-2,cast(concat(year(now),'-',MONTH(now),'-','01') as date)) as plat_month_minus_2
	from (select getdate() as now) T
) W
where 1=1
AND isnull(a.is_deleted, 0) = 0
--and ar.report_id = 209
	--and ar.id = 83147
--and ar.report_id = 690 and ar.id = 65340

alter table #arenda add primary key(report_id,arenda_id)

drop table if exists #balans;

select
bal.report_id, bal.id as balans_id
,case when len(rtrim(ltrim(geodata_map_opoints))) >= 5 then 1 else 0 end as v001
,case when addr_distr_new_id > 0 then 1 else 0 end v002
,case when sqr_vlas_potreb is not null and bal.modify_date >= year_minus_1 then 1 else 0 end as v003
,case when cost_zalishkova is not null and znos_date >= year_minus_1 then 1 else 0 end as v004
,case when exists (select 1 from reports1nf_balans_akt_attachfiles Q where Q.free_square_id = 500000 * bal.report_id + bal.id) then 1 else 0 end as v005
,case when exists (select 1 from reports1nf_balans_rish_attachfiles Q where Q.free_square_id = 500000 * bal.report_id + bal.id) then 1 else 0 end as v006
,case when exists (select 1 from reports1nf_balans_bti_attachfiles Q where Q.free_square_id = 500000 * bal.report_id + bal.id) then 1 else 0 end as v007
,case when exists (select 1 from reports1nf_balans_dinfo_attachfiles Q where Q.free_square_id = 500000 * bal.report_id + bal.id) then 1 else 0 end as v008
--,case when bfs.total_free_sqr >= 0 then 1 else 0 end v023
--,1 as v009

into #balans

FROM reports1nf_balans bal
INNER JOIN #reports rep ON rep.report_id = bal.report_id
LEFT OUTER JOIN reports1nf_buildings bld ON bld.unique_id = bal.building_1nf_unique_id
outer apply (select sum(case when fs.is_included = 1 then fs.total_free_sqr else 0 end) as total_free_sqr from reports1nf_balans_free_square fs where fs.balans_id = bal.id and fs.report_id = bal.report_id /*and fs.is_included = 1*/) bfs 
CROSS APPLY (select cast(concat(year(getdate()) - 1,'0101') as date) year_minus_1) W
where 1=1
and ISNULL(bal.is_deleted,0)=0
--and bal.report_id = 209
--	and bal.id = 6232

alter table #balans add primary key(report_id, balans_id)

/*
select * from #reports
select * from #arenda
select * from #balans
*/

drop table if exists #sum_arenda;

select 
report_id
,sum(case when v210 = 1 then 1 else 0 end) as v210_1, sum(case when v210 = 0 then 1 else 0 end) as v210_0
,sum(case when v220 = 1 then 1 else 0 end) as v220_1, sum(case when v220 = 0 then 1 else 0 end) as v220_0
,sum(case when v230 = 1 then 1 else 0 end) as v230_1, sum(case when v230 = 0 then 1 else 0 end) as v230_0
,sum(case when v240 = 1 then 1 else 0 end) as v240_1, sum(case when v240 = 0 then 1 else 0 end) as v240_0
,sum(case when v250 = 1 then 1 else 0 end) as v250_1, sum(case when v250 = 0 then 1 else 0 end) as v250_0
,sum(case when v260 = 1 then 1 else 0 end) as v260_1, sum(case when v260 = 0 then 1 else 0 end) as v260_0
,sum(case when v270 = 1 then 1 else 0 end) as v270_1, sum(case when v270 = 0 then 1 else 0 end) as v270_0
,sum(case when v280 = 1 then 1 else 0 end) as v280_1, sum(case when v280 = 0 then 1 else 0 end) as v280_0
into #sum_arenda
from #arenda A
group by report_id

--- select * from #balans

drop table if exists #sum_balans;

select 
report_id
,sum(case when v001 = 1 then 1 else 0 end) as v001_1, sum(case when v001 = 0 then 1 else 0 end) as v001_0
,sum(case when v002 = 1 then 1 else 0 end) as v002_1, sum(case when v002 = 0 then 1 else 0 end) as v002_0
,sum(case when v003 = 1 then 1 else 0 end) as v003_1, sum(case when v003 = 0 then 1 else 0 end) as v003_0
,sum(case when v004 = 1 then 1 else 0 end) as v004_1, sum(case when v004 = 0 then 1 else 0 end) as v004_0
,sum(case when v005 = 1 then 1 else 0 end) as v005_1, sum(case when v005 = 0 then 1 else 0 end) as v005_0
,sum(case when v006 = 1 then 1 else 0 end) as v006_1, sum(case when v006 = 0 then 1 else 0 end) as v006_0
,sum(case when v007 = 1 then 1 else 0 end) as v007_1, sum(case when v007 = 0 then 1 else 0 end) as v007_0
,sum(case when v008 = 1 then 1 else 0 end) as v008_1, sum(case when v008 = 0 then 1 else 0 end) as v008_0
--,sum(case when v023 = 1 then 1 else 0 end) as v023_1, sum(case when v023 = 0 then 1 else 0 end) as v023_0
into #sum_balans
from #balans A
group by report_id

--num_given,
--num_problem_dog

drop table if exists #sum_reports;

select
report_id
,sum(case when v111 = 1 then 1 else 0 end) as v111_1, sum(case when v111 = 0 then 1 else 0 end) as v111_0
,sum(case when v131 = 1 then 1 else 0 end) as v131_1, sum(case when v131 = 0 then 1 else 0 end) as v131_0
,sum(num_problem_dog) as v140_1, sum(num_given - num_problem_dog) as v140_0
into #sum_reports
from #reports A
group by report_id

drop table if exists #result;

select 
v111_0, v111_1,
v131_0, v131_1,
v140_0, v140_1,
v001_1, v001_0,
v002_1, v002_0,
v003_1, v003_0,
v004_1, v004_0,
v005_1, v005_0,
v006_1, v006_0,
v007_1, v007_0,
v008_1, v008_0,
--v023_1, v023_0,
v210_1, v210_0,
v220_1, v220_0,
v230_1, v230_0,
v240_1, v240_0,
v250_1, v250_0,
v260_1, v260_0,
v270_1, v270_0,
v280_1, v280_0,
R.addr_district,
R.zkpo_code,
R.full_name,
A.report_id
into #result
from #sum_reports A
inner join #reports R on R.report_id = A.report_id
left join #sum_arenda B on B.report_id = A.report_id
left join #sum_balans C on C.report_id = A.report_id

select * from #result order by addr_district, full_name, zkpo_code, report_id
";
	}

}
