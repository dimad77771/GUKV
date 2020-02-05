using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

public partial class Reports1NF_SendingDeletedObjectsDiagnostic : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int reportId;
        int.TryParse(Request.QueryString["rid"], out reportId);

        if (reportId == null)
            reportId = 0;

        if (Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
        {
            int reportRdaDistrictId = -1;

            if (Utils.UserOrganizationID != Reports1NFUtils.GetReportOrganizationId(reportId, ref reportRdaDistrictId))
            {
                Response.Redirect("SendingDeletedObjectsDiagnostic.aspx?rid=" + reportId.ToString());
            }
        }

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Message", Type.GetType("System.String"));
        dt.Columns.Add("Errors", Type.GetType("System.String"));
        dt.Columns.Add("Link", Type.GetType("System.String"));

        SqlConnection connection = Utils.ConnectToDatabase();
        if (connection != null)
        {
            using (SqlCommand cmd = new SqlCommand(@"SELECT a.id, a.validation_errors FROM reports1nf_balans_deleted a LEFT JOIN reports1nf r ON a.report_id = r.id WHERE a.report_id = @rid AND a.is_valid = 0 " /*and ((a.is_deleted is null) or (a.is_deleted = 0))"*/, connection))
            {
                cmd.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dr = dt.NewRow();
                        dr["Message"] = "Відчужений об'єкт не був відправлений";
                        dr["Errors"] = reader["validation_errors"];
                        dr["Link"] = "<a target='_blank' href='OrgBalansDeletedObject.aspx?rid=" + reportId.ToString() + "&bid=" + reader["id"] + "'>Відкрити</a>";
                        dt.Rows.Add(dr);
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        if (dt.Rows.Count != 0)
        {
            dxGridView.DataSource = dt;
            dxGridView.DataBind();            
        }
        else
        {
            dxGridView.Visible = false;
            lblSuccess.Visible = true;
        }        
    }
}