﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class FreeShowPublic : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

	protected void ASPChangeMapSystem_Click(object sender, EventArgs e)
	{
		MapUtils.SetSystem(MapUtils.GIS);
		Response.Redirect("Report1NFBalansMap.aspx" + Request.Url.Query);
	}

}