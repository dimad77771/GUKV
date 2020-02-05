using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;

public partial class Reports1NF_Report1NFNotifications : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SectionMenu.Visible = System.Web.Security.Roles.IsUserInRole(Utils.Report1NFReviewerRole);
    }

    protected void SqlDataSourceNotifications_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        MembershipUser user = Membership.GetUser();

        if (user != null)
        {
            e.Command.Parameters["@usrid"].Value = (Guid)user.ProviderUserKey;
        }
        else
        {
            e.Command.Parameters["@usrid"].Value = Guid.Empty;
        }
    }

    protected void GridViewNotifications_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        MembershipUser user = Membership.GetUser();

        if (user != null)
        {
            if (e.Parameters.StartsWith("init:"))
            {
                // Get the selected rows from the database
                Dictionary<int, int> selectionStatus = new Dictionary<int, int>();
                SqlConnection connection = Utils.ConnectToDatabase();

                if (connection != null)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT organization_id, is_notify FROM user_notification_settings WHERE UserId = @usrid", connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("usrid", user.ProviderUserKey));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                                {
                                    selectionStatus[reader.GetInt32(0)] = reader.GetInt32(1);
                                }
                            }
                        }
                    }

                    connection.Close();
                }

                PrimaryGridView.Selection.UnselectAll();

                for (int i = 0; i < PrimaryGridView.VisibleRowCount; i++)
                {
                    object rowOrgId = PrimaryGridView.GetRowValues(i, "id");

                    if (rowOrgId is int)
                    {
                        int selectState = 0;

                        if (selectionStatus.TryGetValue((int)rowOrgId, out selectState))
                        {
                            if (selectState == 1)
                                PrimaryGridView.Selection.SelectRow(i);
                        }
                        else
                        {
                            PrimaryGridView.Selection.SelectRow(i);
                        }
                    }
                }
            }
            else if (e.Parameters.StartsWith("set:"))
            {
                string organizationIDs = e.Parameters.Substring(4);
                string[] ids = organizationIDs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                HashSet<int> selectedIDs = new HashSet<int>();
                selectedIDs.UnionWith(ids.Select(x => int.Parse(x)));

                SqlConnection connection = Utils.ConnectToDatabase();

                if (connection != null)
                {
                    for (int i = 0; i < PrimaryGridView.VisibleRowCount; i++)
                    {
                        object rowOrgId = PrimaryGridView.GetRowValues(i, "id");

                        if (rowOrgId is int)
                        {
                            // If there is an entry for this organization, delete the entry
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM user_notification_settings WHERE UserId = @usrid AND organization_id = @orgid", connection))
                            {
                                cmd.Parameters.Add(new SqlParameter("usrid", user.ProviderUserKey));
                                cmd.Parameters.Add(new SqlParameter("orgid", (int)rowOrgId));
                                cmd.ExecuteNonQuery();
                            }

                            using (SqlCommand cmd = new SqlCommand("INSERT INTO user_notification_settings (UserId, organization_id, is_notify) VALUES (@usrid, @orgid, @isn)", connection))
                            {
                                cmd.Parameters.Add(new SqlParameter("usrid", user.ProviderUserKey));
                                cmd.Parameters.Add(new SqlParameter("orgid", (int)rowOrgId));
                                cmd.Parameters.Add(new SqlParameter("isn", selectedIDs.Contains((int)rowOrgId) ? 1 : 0));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    connection.Close();
                }
            }
        }

        PrimaryGridView.DataBind();
    }
}