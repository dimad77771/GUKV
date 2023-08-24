using System;
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
		var gg = Request.Url;

		var menu = LoginView1.FindControlRecursive("LoginMenu1") as DevExpress.Web.ASPxMenu;
		if (menu != null)
		{
			var menuitem = menu.Items.Single(q => q.Name == "LoginInCabinet");
			menuitem.NavigateUrl += "?ReturnUrl=" + Request.Url.AbsolutePath;
		}
	}

	protected void ASPChangeMapSystem_Click(object sender, EventArgs e)
	{
		MapUtils.SetSystem(MapUtils.MBK);
		Response.Redirect("Report1NFFreeMap.aspx" + Request.Url.Query);
	}
}
