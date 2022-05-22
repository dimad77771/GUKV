using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Syncfusion.XlsIO;

public partial class Reports1NF_Report1NFProdlenMap : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		//TEMP_DISABLE_MAP
		Response.Redirect("Report1NFProdlenShow.aspx");

		var url = "Report1NFProdlenMap__" + MapUtils.GetSystem() + ".aspx" + Request.Url.Query;
		Response.Redirect(Page.ResolveClientUrl(url));
	}
}