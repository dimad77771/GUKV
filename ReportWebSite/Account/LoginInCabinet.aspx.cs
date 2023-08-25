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
using DevExpress.Web;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var link = LoginUser.FindControlRecursive("RegisterInCabinet") as ASPxHyperLink;
		if (link != null)
		{
			object returnUrl = Request.QueryString["ReturnUrl"];
			if (returnUrl != null)
			{
				link.NavigateUrl += "?ReturnUrl=" + returnUrl.ToString();
			}
		}
		

		//var user = Membership.GetUser("Януш С.О.");
		//var password = user.ResetPassword();
		//user.ChangePassword(password, "fq,jkbn66+++");

		//var user = Membership.GetUser("О.О26199714");
		//var password = user.ResetPassword();
		//user.ChangePassword(password, "fq,jkbn66+++");

		//var user = Membership.GetUser("О.О37371727_РДА");
		//var password = user.ResetPassword();
		//user.ChangePassword(password, "fq,jkbn66+++");

		//var user = Membership.GetUser("О.Синенко");
		//var password = user.ResetPassword();
		//user.ChangePassword(password, "fq,jkbn66+++");

		//TestChangePasswordAll();
	}

	protected void LoginUser_LoggedIn(object sender, EventArgs e)
    {
        String username = LoginUser.UserName.Trim();

        Utils.FindUserOrganizationAndRda(username);

        ILog log = LogManager.GetLogger("ReportWebSite");

        if (log != null)
            log.Info("User login: " + username);

        var returnUrl = Request.QueryString["ReturnUrl"];
		if (returnUrl != null)
		{
			Response.Redirect(returnUrl.ToString());
		}
    }


}
