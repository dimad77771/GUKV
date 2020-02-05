using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgOccupation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(Utils.OrgOccupationAdmin))
        {
            string errorurl = Page.ResolveClientUrl("~/Account/Restricted.aspx");
            if (Page.IsCallback)
                DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback(errorurl);
            else
                Response.Redirect(errorurl);
        }
    }

    protected void SqlDataSourceOrganizations_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        //return;
        Dictionary<string, Control> controls = new Dictionary<string, Control>();
        Reports1NFUtils.GetAllControls(this.ASPxGridViewOrgOccupation, controls);

        //int organizationId = (int)e.Command.Parameters["@org_id"].Value;
        //int periodId = (int)e.Command.Parameters["@period_id"].Value;
        int occupationId = Reports1NFUtils.GetDropDownValue(controls, "ComboOccupation");

        //System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        //string username = (user == null ? "Auto-import" : user.UserName);

        //SqlConnection connection = Utils.ConnectToDatabase();
        //ArchiverSql.CreateOrganizationArchiveRecord(connection, organizationId, username, null);

        //using (SqlCommand cmd = new SqlCommand("update [org_by_period] set org_occupation_id = @org_occupation_id where org_id = @org_id and period_id = @period_id", connection))
        //{
        //    cmd.Parameters.AddWithValue("org_id", organizationId);
        //    cmd.Parameters.AddWithValue("period_id", periodId);
        //    cmd.Parameters.AddWithValue("occupation_id", occupationId);
        //    cmd.ExecuteNonQuery();
        //}

        //e.Command.Parameters["@org_id"].Value = organizationId;
        //e.Command.Parameters["@period_id"].Value = periodId;
        e.Command.Parameters["@org_occupation_id"].Value = occupationId;
    }
}