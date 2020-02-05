using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;


public partial class Account_FinAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            var username = User.Identity.Name;
            var user = Membership.GetUser(username);
            var userId = user.ProviderUserKey;
            var connection = Utils.ConnectToDatabase();
            if (connection != null)
            {
                using (SqlCommand command = new SqlCommand("update reports1nf_accounts set fin_add_showed = 1 where UserId = @userid", connection))
                {
                    command.Parameters.AddWithValue("@userid", userId);
                    //command.ExecuteNonQuery();
                }
                connection.Close();
            }

            if (Roles.IsUserInRole(username, Utils.Report1NFSubmitterRole))
                LinkToCabinet.NavigateUrl = Page.ResolveUrl("~/Reports1NF/Cabinet.aspx");
        }
    }
}