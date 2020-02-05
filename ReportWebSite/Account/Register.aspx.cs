using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Account_Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
    }

    protected void RegisterUser_CreatedUser(object sender, EventArgs e)
    {
        FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

        /*
        // Redirect to the main page
        string continueUrl = RegisterUser.ContinueDestinationPageUrl;

        if (String.IsNullOrEmpty(continueUrl))
        {
            continueUrl = "~/";
        }

        Response.Redirect(continueUrl);
        */

        Response.Redirect("~/Account/RegisterSuccess.aspx");
    }
}
