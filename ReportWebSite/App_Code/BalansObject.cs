using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;


namespace GUKV
{
    /// <summary>
    /// Summary description for BalansObjectsDataSource
    /// </summary>
[Serializable]
    public class BalansObject
    {
        public BalansObject()
        {

        }

        public int OrganizationId { get; set; }
        public int ReportId { get; set; }
        public int BalansId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public decimal SqrTotal { get; set; }
        public string Purpose { get; set; }

        public static IEnumerable<BalansObject> Select(int organizationId, int reportId)
        {
            SqlConnection connectionSql = Utils.ConnectToDatabase();

            string query = @"select b.id,d.addr_street_name as obj_street_name,b.obj_nomer,b.sqr_total,
                COALESCE(b.purpose_str, dict_balans_purpose.name),
                b.report_id from reports1nf_balans b 
                join reports1nf r on r.organization_id = b.organization_id and r.id = b.report_id
                join buildings d on d.id = b.building_id
                LEFT OUTER JOIN dict_balans_purpose ON dict_balans_purpose.id = b.purpose_id
                where b.organization_id = @organization_id and b.report_id = @report_id";

            using (SqlCommand cmd = new SqlCommand(query, connectionSql))
            {
                cmd.Parameters.AddWithValue("organization_id", organizationId);
                cmd.Parameters.AddWithValue("report_id", reportId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int balabs_id = (int)reader.GetValue(0);
                        string obj_street_name = reader.IsDBNull(1) ? string.Empty : (string)reader.GetValue(1);
                        string obj_nomer = reader.IsDBNull(2) ? string.Empty : (string)reader.GetValue(2);
                        decimal sqr_total = reader.IsDBNull(3) ? 0 : (decimal)reader.GetValue(3);
                        string purpose_str = reader.IsDBNull(4) ? string.Empty : (string)reader.GetValue(4);
                        int report_id = reader.IsDBNull(5) ? 0 : (int)reader.GetValue(5);

                        yield return new BalansObject()
                        {
                            OrganizationId = organizationId,
                            BalansId = balabs_id,
                            Street = obj_street_name,
                            Number = obj_nomer,
                            SqrTotal = sqr_total,
                            Purpose = purpose_str,
                            ReportId = report_id,
                        };
                    }

                    reader.Close();
                }
            }
            connectionSql.Close();

        }
    }
}