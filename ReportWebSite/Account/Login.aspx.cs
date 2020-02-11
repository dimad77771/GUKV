using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using log4net;
using GUKV.Common;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		//var user = Membership.GetUser("О.Синенко");
		//var password = user.ResetPassword();
		//user.ChangePassword(password, "fq,jkbn66+++");
	}

	protected void LoginUser_LoggedIn(object sender, EventArgs e)
    {
        String username = LoginUser.UserName.Trim();

        Utils.FindUserOrganizationAndRda(username);

        ILog log = LogManager.GetLogger("ReportWebSite");

        if (log != null)
            log.Info("User login: " + username);

        /*
        // If user belongs to "RDAController" role, redirect him to the RDA section
        if (Roles.IsUserInRole(username, Utils.RDAControllerRole))
        {
            Response.Redirect(Page.ResolveClientUrl("~/Reports1NF/RDAReportList.aspx"));
        }
        */

        // If user belongs to "1NFReportSubmitter" role, redirect him to the Cabinet
        if (Roles.IsUserInRole(username, Utils.Report1NFSubmitterRole))
        {
            var user = Membership.GetUser(username);
            var userId = user.ProviderUserKey;
            var fin_add_showed = false;
            var connection = Utils.ConnectToDatabase();
/*-----
            if (connection != null)
            {
                using (SqlCommand command = new SqlCommand("select * from [reports1nf_accounts] where UserId = @userid", connection))
                {
                    command.Parameters.AddWithValue("@userid", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                fin_add_showed = (reader["fin_add_showed"].ToString() == "1") ? true : false;
                                break;
                            }
                        }
                    }
                }
                connection.Close();
            }
-----*/
            if (fin_add_showed)
            {
                // Do not redirect to Cabinet if there is some Return Url
                object returnUrl = Request.QueryString["ReturnUrl"];

                bool returnUrlExists = (returnUrl is string) && !string.IsNullOrEmpty((string)returnUrl);

                if (!returnUrlExists)
                {
                    Response.Redirect(Page.ResolveClientUrl("~/Reports1NF/Cabinet.aspx"));
                }
            }
            else
            {
                Response.Redirect(Page.ResolveClientUrl("~/Account/FinAdd.aspx"));
            }
        }
    }
}
