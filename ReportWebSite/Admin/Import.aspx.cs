using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Import : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlConnection connectionSql = Utils.ConnectToDatabase();
        SqlTransaction transactionSql = connectionSql.BeginTransaction();

        NormalizeAddress(connectionSql, transactionSql);

        StringBuilder s;
        List<int> balansList;

        ImportData(connectionSql, transactionSql, out s, out balansList);

        AddTo1NFBalans(connectionSql, transactionSql, balansList);

        TextBox1.Text = s.ToString();

        transactionSql.Commit();
        connectionSql.Close();

    }

    private static void AddTo1NFBalans(SqlConnection connectionSql, SqlTransaction transactionSql, List<int> balansList)
    {
        using (SqlCommand cmd = new SqlCommand("ALTER TABLE dbo.buildings DROP COLUMN normalized_number", connectionSql, transactionSql))
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        foreach (var t in balansList)
        {
            using (SqlCommand cmd = new SqlCommand("dbo.fnAddBalansObjectToReport", connectionSql, transactionSql))
            {
                try
                {
                    cmd.Parameters.Add(new SqlParameter("BALANS_ID", t));

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();


                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }

    private void ImportData(SqlConnection connectionSql, SqlTransaction transactionSql, out StringBuilder s, out List<int> balansList)
    {
        StringReader r = new StringReader(TextBox1.Text);

        s = new StringBuilder();
        balansList = new List<int>();
        while (true)
        {
            string line = r.ReadLine();
            if (String.IsNullOrEmpty(line))
                break;

            string[] parts = line.Split(';');
            String StreetName = parts[0].ToUpper();// "ВЕРХОВНОЇ РАДИ БУЛЬВ.";
            String AddrNomer = parts[1];// " 10а";

            var address = new GUKV.ImportToolUtils.ObjectFinder.UnifiedAddress();
            address.streetName = new GUKV.ImportToolUtils.ObjectFinder.StreetName(StreetName.ToString());
            GUKV.ImportToolUtils.ObjectFinder.ParseAddressNumbers(AddrNomer, address);


            string fullAddr = address.FormatFullAddress();

            Int32 buildingid = -1;
            using (SqlCommand cmd = new SqlCommand("select id from buildings where normalized_number = @p_normalized_number", connectionSql, transactionSql))
            {
                try
                {
                    cmd.Parameters.Add(new SqlParameter("p_normalized_number", fullAddr));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            buildingid = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        }

                        reader.Close();
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            if (buildingid > 0)
            {
                int maxID = -1;
                using (SqlCommand cmd = new SqlCommand("select max(id) + 1 from balans", connectionSql, transactionSql))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            maxID = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        }

                        reader.Close();
                    }
                }

                int p_id = maxID;
                int p_building_id = (int)buildingid;
                double p_sqr_total = double.Parse(parts[2]);
                double p_sqr_non_habit = double.Parse(parts[3]);
                double p_cost_balans = double.Parse(parts[4]);
                double p_cost_zalishkova = double.Parse(parts[5]);
                int p_object_kind_id = int.Parse(parts[6]);
                int p_object_type_id = int.Parse(parts[7]);
                string p_purpose_str = parts[8];
                double p_znos = double.Parse(parts[9]);
                string p_reestr_no = parts[10];

                string p_obj_nomer1 = "";
                string p_obj_nomer2 = "";
                string p_obj_nomer3 = "";

                using (SqlCommand cmd = new SqlCommand("select addr_nomer1,addr_nomer2,addr_nomer3 from buildings where id = @p_id", connectionSql, transactionSql))
                {
                    cmd.Parameters.AddWithValue("p_id", p_building_id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            p_obj_nomer1 = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                            p_obj_nomer2 = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);
                            p_obj_nomer3 = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);
                        }

                        reader.Close();
                    }
                }

                var queryInsertBalans = @"insert into balans (

id,year_balans,building_id,organization_id,sqr_total,sqr_non_habit,cost_balans,cost_zalishkova,num_privat_apt,
form_ownership_id,object_kind_id,object_type_id,history_id,tech_condition_id,purpose_group_id,purpose_id,
purpose_str,priznak_1nf,ownership_type_id,obj_nomer1,obj_nomer2,obj_nomer3,modified_by,modify_date,
update_src_id,znos,znos_date,reestr_no,arch_flag,is_free_sqr,free_sqr_useful,ownership_doc_type,
ownership_doc_num,ownership_doc_date,balans_doc_type,balans_doc_num,balans_doc_date,obj_status_id

) VALUES (

@p_id, 
2016,
@p_building_id,
142122,
@p_sqr_total,
@p_sqr_non_habit,
@p_cost_balans,
@p_cost_zalishkova,
0,
34,
@p_object_kind_id, -- ЖИЛЕ = 4 НЕ ЖИЛЕ = 2, 
@p_object_type_id, -- ОКРЕМО СТОЯЧИЙ = 1, ПРИМІЩЕННЯ В Ж/Б = 3
2,
2,
37,
29003,
@p_purpose_str,
0,
4,
@p_obj_nomer1,
@p_obj_nomer2,
@p_obj_nomer3,
'А.О39606435',
'2016-08-15 15:12:38.827',
10,
@p_znos,
'2016-08-15',
@p_reestr_no,
0,
0,
0.00,
3,
'б\н',
'2015-04-01 00:00:00.000',
23,
61,
'2015-02-13 00:00:00.000',
2

)";
                using (SqlCommand cmd = new SqlCommand(queryInsertBalans, connectionSql, transactionSql))
                {
                    cmd.Parameters.AddWithValue("p_id", p_id);
                    cmd.Parameters.AddWithValue("p_building_id", p_building_id);
                    cmd.Parameters.AddWithValue("p_sqr_total", p_sqr_total);
                    cmd.Parameters.AddWithValue("p_sqr_non_habit", p_sqr_non_habit);
                    cmd.Parameters.AddWithValue("p_cost_balans", p_cost_balans);
                    cmd.Parameters.AddWithValue("p_cost_zalishkova", p_cost_zalishkova);
                    cmd.Parameters.AddWithValue("p_object_kind_id", p_object_kind_id);
                    cmd.Parameters.AddWithValue("p_object_type_id", p_object_type_id);
                    cmd.Parameters.AddWithValue("p_purpose_str", p_purpose_str);
                    cmd.Parameters.AddWithValue("p_obj_nomer1", p_obj_nomer1);
                    cmd.Parameters.AddWithValue("p_obj_nomer2", p_obj_nomer2);
                    cmd.Parameters.AddWithValue("p_obj_nomer3", p_obj_nomer3);
                    cmd.Parameters.AddWithValue("p_znos", p_znos);
                    cmd.Parameters.AddWithValue("p_reestr_no", p_reestr_no);

                    cmd.ExecuteNonQuery();

                    balansList.Add(p_id);
                }



                s.AppendLine(line + ";True");

            }
            else
                s.AppendLine(line + ";False");

        }
    }

    private static void NormalizeAddress(SqlConnection connectionSql, SqlTransaction transactionSql)
    {
        using (SqlCommand cmd = new SqlCommand("ALTER TABLE dbo.buildings ADD normalized_number varchar(MAX) NULL", connectionSql, transactionSql))
        {
            cmd.ExecuteNonQuery();
        }

        Dictionary<int, String> normAddr = new Dictionary<int, string>();

        using (SqlCommand cmd = new SqlCommand("select id,addr_street_name,addr_nomer from buildings", connectionSql, transactionSql))
        {
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string street = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);
                        string num = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);

                        var address = new GUKV.ImportToolUtils.ObjectFinder.UnifiedAddress();
                        address.streetName = new GUKV.ImportToolUtils.ObjectFinder.StreetName(street);
                        GUKV.ImportToolUtils.ObjectFinder.ParseAddressNumbers(num, address);
                        normAddr.Add(id, address.FormatFullAddress());
                    }

                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        foreach (var item in normAddr)
        {
            using (SqlCommand cmdUpd = new SqlCommand("update buildings set normalized_number = @p_normalized_number where id = @p_id", connectionSql, transactionSql))
            {
                cmdUpd.Parameters.AddWithValue("p_normalized_number", item.Value);
                cmdUpd.Parameters.AddWithValue("p_id", item.Key);
                cmdUpd.ExecuteNonQuery();
            }
        }
    }
}