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
		var menu1 = LoginView1.FindControlRecursive("LoginMenu1") as DevExpress.Web.ASPxMenu;
		if (menu1 != null)
		{
			var menuitem = menu1.Items.Single(q => q.Name == "LoginInCabinet");
			menuitem.NavigateUrl += "?ReturnUrl=" + Request.Url.AbsolutePath;
		}

		var menu2 = LoginView1.FindControlRecursive("LoginMenu2") as DevExpress.Web.ASPxMenu;
		if (menu2 != null)
		{
			var menuitem = menu2.Items.Single(q => q.Name == "LogoutCabinet");
			menuitem.NavigateUrl += "?ReturnUrl=" + Request.Url.AbsolutePath;
		}
	}

	protected void ASPChangeMapSystem_Click(object sender, EventArgs e)
	{
		MapUtils.SetSystem(MapUtils.MBK);
		Response.Redirect("Report1NFFreeMap.aspx" + Request.Url.Query);
	}
}
