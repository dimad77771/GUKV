﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Account_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Abandon();

        FormsAuthentication.SignOut();

        //FormsAuthentication.RedirectToLoginPage();

        Response.Redirect("~/Default.aspx");
    }
}