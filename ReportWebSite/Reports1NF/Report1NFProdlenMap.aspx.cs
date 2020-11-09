using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Syncfusion.XlsIO;

public partial class Reports1NF_Report1NFProdlenMap : System.Web.UI.Page
{
	protected string ReportID = "11";
	protected string[] GGG = new[] { "UUU\nKKK", "8.2", "11.0" };
	protected List<PointInfo> AllPoints = new List<PointInfo>();
	protected string selected_fs_id;

	protected void Page_Load(object sender, EventArgs e)
    {
		selected_fs_id = (Request.QueryString["fs_id"] != null ? Request.QueryString["fs_id"] : "null");

		var SqlQuery = SqlQuery_1 + "\n\n UNION ALL \n\n" + SqlQuery_2;

		var connection = Utils.ConnectToDatabase();
		using (var cmd = new SqlCommand(SqlQuery, connection))
		{
			//cmd.Parameters.Add(new SqlParameter("rep_id", ReportID));
			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					var fs_id = reader.GetInt32(0);
					var komis_protocol = GetStringValue(reader, 1);
					var geodata_map_points = GetStringValue(reader, 2);
					var street_name = GetStringValue(reader, 3); ;
					var addr_nomer = GetStringValue(reader, 4); ;
					var sqr_for_rent = GetDecimalValue(reader, 5);
					var possible_using = GetStringValue(reader, 6); ;
					var total_free_sqr = GetDecimalValue(reader, 7);
					var free_sql_usefull = GetDecimalValue(reader, 8);
					var floor = GetStringValue(reader, 9); ;
					var water = GetBoolValue(reader, 10);
					var heating = GetBoolValue(reader, 11);
					var power = GetBoolValue(reader, 12);
					var gas = GetBoolValue(reader, 13);
					var org_name = GetStringValue(reader, 14);
					var condition = GetStringValue(reader, 15);
					var vidpov_osoba = GetStringValue(reader, 16);
					var current_stage_name = GetStringValue(reader, 17);
					var current_stage_docdate = GetDateTimeValue(reader, 18);
					var current_stage_docnum = GetStringValue(reader, 19);
					var current_stage_has_documents = GetStringValue(reader, 20);
					var period_used_name = GetStringValue(reader, 21);
					var need_zgoda = GetStringValue(reader, 22);
					var invest_solution = GetStringValue(reader, 23);
					var orandodatel = GetStringValue(reader, 24);
					var include_in_perelik = GetStringValue(reader, 25);
					var prozoro_number = GetStringValue(reader, 26);
					var source = GetStringValue(reader, 27);

					var regpoints = (new Regex(@"(\d+\.\d+)\s+(\d+\.\d+)")).Match(geodata_map_points);
					if (regpoints.Groups.Count != 3) throw new Exception();
					var point1 = Decimal.Parse(regpoints.Groups[1].Value, CultureInfo.InvariantCulture);
					var point2 = Decimal.Parse(regpoints.Groups[2].Value, CultureInfo.InvariantCulture);

					var pointInfo = new PointInfo
					{
						fs_id = fs_id,
						komis_protocol = komis_protocol,
						geodata_map_points = geodata_map_points,
						point1 = point1,
						point2 = point2,
						sqr_for_rent = sqr_for_rent,
						street_name = street_name,
						addr_nomer = addr_nomer,
						full_address = ((street_name ?? "").Trim() + " " + (addr_nomer ?? "").Trim()).Trim(),
						possible_using = possible_using,
						total_free_sqr = total_free_sqr,
						free_sql_usefull = free_sql_usefull,
						floor = floor,
						water = water,
						heating = heating,
						power = power,
						gas = gas,
						org_name = org_name,
						condition = condition,
						vidpov_osoba = vidpov_osoba,
						current_stage_name = current_stage_name,
						current_stage_docdate = current_stage_docdate,
						current_stage_docnum = current_stage_docnum,
						current_stage_has_documents = current_stage_has_documents,
						period_used_name = period_used_name,
						need_zgoda = need_zgoda,
						invest_solution = invest_solution,
						orandodatel = orandodatel,
						include_in_perelik = include_in_perelik,
						prozoro_number = prozoro_number,
						source = source,
					};
					AllPoints.Add(pointInfo);
				}

				reader.Close();
			}
		}
	}

	decimal? GetDecimalValue(SqlDataReader reader, int colno)
	{
		var vv = reader.GetSqlDecimal(colno);
		if (vv.IsNull)
		{
			return (decimal?)null;
		}
		else
		{
			return (decimal)vv;
		}
	}

	int? GetIntValue(SqlDataReader reader, int colno)
	{
		var vv = reader.GetSqlInt32(colno);
		if (vv.IsNull)
		{
			return (int?)null;
		}
		else
		{
			return (int)vv;
		}
	}

	bool? GetBoolValue(SqlDataReader reader, int colno)
	{
		var vv = reader.GetSqlBoolean(colno);
		if (vv.IsNull)
		{
			return (bool?)null;
		}
		else
		{
			return (bool)vv;
		}
	}

	DateTime? GetDateTimeValue(SqlDataReader reader, int colno)
	{
		var vv = reader.GetSqlDateTime(colno);
		if (vv.IsNull)
		{
			return (DateTime?)null;
		}
		else
		{
			return (DateTime)vv;
		}
	}


	string GetStringValue(SqlDataReader reader, int colno)
	{
		var vv = reader.GetSqlString(colno);
		if (vv.IsNull)
		{
			return "";
		}
		else
		{
			return (string)vv;
		}
	}


	protected void SqlDataSourceFreeSquare_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
		//e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
		//e.Command.Parameters["@period_year"].Value = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
		//e.Command.Parameters["@baseurl"].Value = Utils.WebsiteBaseUrl;
	}



	public class PointInfo
	{
		public int fs_id;
		public string komis_protocol;
		public string geodata_map_points;
		public decimal point1;
		public decimal point2;
		public decimal? sqr_for_rent;
		public string street_name;
		public string addr_nomer;
		public string full_address;
		public string possible_using;
		public decimal? total_free_sqr;
		public decimal? free_sql_usefull;
		public string floor;
		public bool? water;
		public bool? heating;
		public bool? power;
		public bool? gas;
		public string org_name;
		public string condition;
		public string vidpov_osoba;
		public string current_stage_name;
		public DateTime? current_stage_docdate;
		public string current_stage_docnum;
		public string current_stage_has_documents;
		public string period_used_name;
		public string need_zgoda;
		public string invest_solution;
		public string orandodatel;
		public string include_in_perelik;
		public string prozoro_number;
		public string source;
	}




	string SqlQuery_1 = @"
SELECT 
	fs.id,
	fs.komis_protocol,
	fs.geodata_map_points,
	b.street_full_name as street_name,
	(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')) as addr_nomer,
	b.sqr_for_rent,

	--(select qq.name2 from view_dict_rental_rate qq where qq.id = fs.using_possible_id) as possible_using,
	fs.possible_using,

	fs.total_free_sqr,
	fs.free_sqr_korysna as free_sql_usefull,
	fs.floor,
	fs.water,
	fs.heating,
	fs.power,
	fs.gas,
	org.short_name as org_name,
	(select q.name from dict_1nf_tech_stane q where q.id = fs.free_sqr_condition_id) as condition,
	org.director_title as vidpov_osoba,
	(select qq.step_name from freecycle_step_dict qq where qq.step_id = fs.freecycle_step_dict_id) as current_stage_name,
	fs.current_stage_docdate,
	fs.current_stage_docnum,
	case when exists (select 1 from reports1nf_arenda_dogcontinue_current_stage_documents qq where qq.free_square_id = fs.id) then '+' else '-' end as current_stage_has_documents,
	(select qq.name from dict_1nf_period_used qq where qq.id = fs.period_used_id) as period_used_name,
	case when fs.zgoda_control_id = 100 or fs.zgoda_renter_id = 100 then '+' else '-' end as need_zgoda,
	(select qq.name from dict_1nf_invest_solution qq where qq.id = fs.invest_solution_id) as invest_solution,
	(select Q.full_name from organizations Q where Q.id = bal.org_giver_id) as orandodatel,
	--dbo.get_reports1NF_orandodatel(b.district, rep.form_of_ownership) as orandodatel,
	fs.include_in_perelik,
	fs.prozoro_number,	
	'1' as source,
		
 row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr) as npp     
,fs.id
,org.zkpo_code


,b.district

,b.object_type 
,b.object_kind
,b.sqr_total



--,null as free_sql_usefull
--,null as mzk
--,rfs.sqr_free_korysna as free_sql_usefull
--,rfs.sqr_free_mzk as mzk




,fs.modify_date
,fs.note
, solution = fs.is_solution
, fs.initiator
, zg2.name as zgoda_control
, zg.name as zgoda_renter

,st.kind
,rep.form_of_ownership
,rep.old_organ

,b.object_kind as vydbudynku
,history = case when isnull(b.history, 'НІ') = 'НІ' then '' else 'ТАК' end 
, isnull(ddd.name, 'Невизначені') as sf_upr

, case when exists (select 1 from reports1nf_arenda_dogcontinue_photos qq where qq.free_square_id = fs.id) then 1 else 0 end as isexistsphoto

FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
--left join (select * from dbo.reports1nf_arenda_dogcontinue where id = (select top 1 id from dbo.reports1nf_arenda_dogcontinue where arenda_id = bal.id)) fs on fs.arenda_id = bal.id
join reports1nf_org_info org on org.id = bal.org_balans_id
left join [dbo].[dict_streets] st on b.addr_street_id = st.id
left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id

--OUTER APPLY (SELECT TOP 1 * FROM rent_free_square rfs
--		WHERE rfs.building_id = bal.building_id AND
--		      rfs.organization_id = bal.organization_id order by rfs.rent_period_id DESC) rfs

LEFT JOIN (
			select obp.org_id
			, occ.name
			, occ.id
			, per.name as period 
			from org_by_period obp
			join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
			join dict_rent_occupation occ on occ.id = obp.org_occupation_id
				) DDD ON DDD.org_id = rep.organization_id

		WHERE (komis_protocol <> '' and komis_protocol not like '0%' and is_included = 1) and geodata_map_points <> ''
        --WHERE (@p_rda_district_id = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @p_rda_district_id))

    --order by org_name, street_name, addr_nomer, total_free_sqr
";

	string SqlQuery_2 = @"
SELECT 
	fs.id,
	fs.komis_protocol,
	fs.geodata_map_points,
	b.street_full_name as street_name,
	(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')) as addr_nomer,
	b.sqr_for_rent,

	--(select qq.name2 from view_dict_rental_rate qq where qq.id = fs.using_possible_id) as possible_using,
	fs.possible_using,

	fs.total_free_sqr,
	fs.free_sqr_korysna as free_sql_usefull,
	fs.floor,
	fs.water,
	fs.heating,
	fs.power,
	fs.gas,
	org.short_name as org_name,
	(select q.name from dict_1nf_tech_stane q where q.id = fs.free_sqr_condition_id) as condition,
	org.director_title as vidpov_osoba,
	(select qq.step_name from freecycle_step_dict qq where qq.step_id = fs.freecycle_step_dict_id) as current_stage_name,
	fs.current_stage_docdate,
	fs.current_stage_docnum,
	case when exists (select 1 from reports1nf_balans_free_square_current_stage_documents qq where qq.free_square_id = fs.id) then '+' else '-' end as current_stage_has_documents,
	(select qq.name from dict_1nf_period_used qq where qq.id = fs.period_used_id) as period_used_name,
	case when fs.zgoda_control_id = 100 or fs.zgoda_renter_id = 100 then '+' else '-' end as need_zgoda,
	(select qq.name from dict_1nf_invest_solution qq where qq.id = fs.invest_solution_id) as invest_solution,
	dbo.get_reports1NF_orandodatel(b.district, rep.form_of_ownership) as orandodatel,
	fs.include_in_perelik,
	fs.prozoro_number,	
	'2' as source,
		
 row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr) as npp     
,fs.id
,org.zkpo_code


,b.district

,b.object_type 
,b.object_kind
,b.sqr_total



--,null as free_sql_usefull
--,null as mzk
--,rfs.sqr_free_korysna as free_sql_usefull
--,rfs.sqr_free_mzk as mzk




,fs.modify_date
,fs.note
, solution = fs.is_solution
, fs.initiator
, zg2.name as zgoda_control
, zg.name as zgoda_renter

,st.kind
,rep.form_of_ownership
,rep.old_organ

,b.object_kind as vydbudynku
,history = case when isnull(b.history, 'НІ') = 'НІ' then '' else 'ТАК' end 
, isnull(ddd.name, 'Невизначені') as sf_upr

, case when exists (select 1 from reports1nf_balans_free_square_photos qq where qq.free_square_id = fs.id) then 1 else 0 end as isexistsphoto

FROM view_reports1nf rep
join reports1nf_balans bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = rep.report_id
--left join (select * from dbo.reports1nf_balans_free_square where id = (select top 1 id from dbo.reports1nf_balans_free_square where balans_id = bal.id)) fs on fs.balans_id = bal.id
join reports1nf_org_info org on org.id = bal.organization_id
left join [dbo].[dict_streets] st on b.addr_street_id = st.id
left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id

--OUTER APPLY (SELECT TOP 1 * FROM rent_free_square rfs
--		WHERE rfs.building_id = bal.building_id AND
--		      rfs.organization_id = bal.organization_id order by rfs.rent_period_id DESC) rfs

LEFT JOIN (
			select obp.org_id
			, occ.name
			, occ.id
			, per.name as period 
			from org_by_period obp
			join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
			join dict_rent_occupation occ on occ.id = obp.org_occupation_id
				) DDD ON DDD.org_id = rep.organization_id

		WHERE (komis_protocol <> '' and komis_protocol not like '0%' and is_included = 1) and geodata_map_points <> ''
				and prozoro_number <> ''
				
        --WHERE (@p_rda_district_id = 0 OR (rep.org_form_ownership_id in (select id from dict_org_ownership where is_rda = 1) AND rep.org_district_id = @p_rda_district_id))

    order by org_name, street_name, addr_nomer, total_free_sqr
";

	

}