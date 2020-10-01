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
using DevExpress.Web.ASPxGridView;
using Syncfusion.XlsIO;

public partial class Reports1NF_Report1NFPrivatisatMap : System.Web.UI.Page
{
	protected string ReportID = "11";
	protected string[] GGG = new[] { "UUU\nKKK", "8.2", "11.0" };
	protected List<PointInfo> AllPoints = new List<PointInfo>();
	protected string selected_fs_id;

	protected void Page_Load(object sender, EventArgs e)
    {
		selected_fs_id = (Request.QueryString["fs_id"] != null ? Request.QueryString["fs_id"] : "null");

		var connection = Utils.ConnectToDatabase();
		using (var cmd = new SqlCommand(SqlQuery, connection))
		{
			//cmd.Parameters.Add(new SqlParameter("rep_id", ReportID));
			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					var fs_id = reader.GetInt32(0);
					var geodata_map_points = GetStringValue(reader, 1);
					var street_name = GetStringValue(reader, 2);
					var addr_nomer = GetStringValue(reader, 3);
					var org_name = GetStringValue(reader, 4);
					var vidpov_osoba = GetStringValue(reader, 5);
					var total_free_sqr = GetDecimalValue(reader, 6);
					var obj_price = GetDecimalValue(reader, 7);
					var orendar = GetStringValue(reader, 8);
					var buyer = GetStringValue(reader, 9);
					var organizator = GetStringValue(reader, 10);
					var has_documents = GetStringValue(reader, 11);
					var prozoro_number = GetStringValue(reader, 12);

					var regpoints = (new Regex(@"(\d+\.\d+)\s+(\d+\.\d+)")).Match(geodata_map_points);
					if (regpoints.Groups.Count != 3) throw new Exception();
					var point1 = Decimal.Parse(regpoints.Groups[1].Value, CultureInfo.InvariantCulture);
					var point2 = Decimal.Parse(regpoints.Groups[2].Value, CultureInfo.InvariantCulture);

					var pointInfo = new PointInfo
					{
						fs_id = fs_id,
						geodata_map_points = geodata_map_points,
						point1 = point1,
						point2 = point2,

						organizator = organizator,
						street_name = street_name,
						addr_nomer = addr_nomer,
						full_address = ((street_name ?? "").Trim() + " " + (addr_nomer ?? "").Trim()).Trim(),

						org_name = org_name,
						vidpov_osoba = vidpov_osoba,
						total_free_sqr = total_free_sqr,
						obj_price = obj_price,
						orendar = orendar,
						buyer = buyer,
						has_buyer = !string.IsNullOrEmpty(buyer),

						has_documents = has_documents,
						prozoro_number = prozoro_number,
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


	protected void SqlDataSourceMapPrivatisat_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
		//e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
		//e.Command.Parameters["@period_year"].Value = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
		//e.Command.Parameters["@baseurl"].Value = Utils.WebsiteBaseUrl;
	}


	public class PointInfo
	{
		public int fs_id;
		public string geodata_map_points;
		public string organizator;
		public decimal point1;
		public decimal point2;
		public string street_name;
		public string addr_nomer;
		public string full_address;
		public decimal? total_free_sqr;
		public decimal? obj_price;
		public string org_name;
		public string orendar;
		public string vidpov_osoba;
		public string buyer;
		public bool has_buyer;
		public string has_documents;
		public string prozoro_number;
	}




	string SqlQuery = @"
SELECT
	A.id,
	A.geodata_map_points,
    (select Q.name from dict_streets Q where Q.id = A.addr_street_id) as addr_street,
	A.addr_nomer,
	(select Q.short_name from reports1nf_org_info Q where Q.report_id = A.org_info_id) as org_name,
	(select Q.director_title from reports1nf_org_info Q where Q.report_id = A.org_info_id) as vidpov_osoba,
	A.total_free_sqr,
	A.obj_price,
	A.orendar,
	A.buyer_name as buyer,
	A.organizator,
	case when exists (select 1 from [privatisat_documents] Q where Q.free_square_id = A.id) then '+' else '-' end as has_documents,
	A.prozoro_number
FROM [privatisat] A
WHERE A.geodata_map_points <> ''
";

}