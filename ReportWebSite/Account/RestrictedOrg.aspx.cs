using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Account_RestrictedOrg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
        {
            LinkReturn.NavigateUrl = Page.ResolveClientUrl("~/Reports1NF/Cabinet.aspx");
        }
    }
}