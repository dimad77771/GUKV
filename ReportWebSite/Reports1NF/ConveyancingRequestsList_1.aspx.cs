using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;

public partial class Reports1NF_ConveyancingRequestsList : System.Web.UI.Page
{
    protected int userOrgId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];

        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);
        }

        if (ReportID > 0)
        {
            int reportRdaDistrictId = -1;
            int reportOrganizationId = Reports1NFUtils.GetReportOrganizationId(ReportID, ref reportRdaDistrictId);

            for (int i = 0; i < SectionMenu.Items.Count; i++)
            {
                if (SectionMenu.Items[i].NavigateUrl.IndexOf('?') < 0)
                    SectionMenu.Items[i].NavigateUrl += "?rid=" + ReportID.ToString();
            }
        }

        userOrgId = Utils.GetUserOrganizationID(User.Identity.Name);

        int requestId;
        if (int.TryParse(Request.Params["reqid"], out requestId) 
            && requestId > 0 
            && Request.Params["mode"] == "delete")
        {
            using (SqlConnection connection = Utils.ConnectToDatabase())
            if (connection != null)
            {
                int orgFromId = 0, orgToId = 0;

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 600;
                    command.CommandText = "SELECT org_from_id, org_to_id FROM transfer_requests WHERE request_id = @reqid";

                    command.Parameters.AddWithValue("reqid", requestId);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("org_from_id")))
                                orgFromId = Convert.ToInt32(reader["org_from_id"]);
                            if (!reader.IsDBNull(reader.GetOrdinal("org_to_id")))
                                orgToId = Convert.ToInt32(reader["org_to_id"]);
                        }
                    }
                }

                if (orgFromId == userOrgId || orgToId == userOrgId)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = "DELETE FROM transfer_requests WHERE request_id = @reqid";

                        command.Parameters.AddWithValue("reqid", requestId);

                        command.ExecuteNonQuery();
                    }
                }
            }

            Response.Redirect("ConveyancingRequestsList.aspx?rid=" + reportIdStr);
        }
    }

    protected int ReportID
    {
        get
        {
            object reportId = ViewState["REPORT_ID"];

            if (reportId is int)
            {
                return (int)reportId;
            }

            return 0;
        }

        set
        {
            ViewState["REPORT_ID"] = value;
        }
    }

    protected void SqlDataSourceConveyancingRequests_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@our_org_id"].Value = userOrgId;
    }
}