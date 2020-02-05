using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Reports1NF_ReportList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check if the user is logged in
        if (Membership.GetUser() == null)
        {
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}