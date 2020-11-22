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

public partial class Reports1NF_Report1NFFreeMap : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
    {
		var url = "Report1NFFreeMap__" + MapUtils.GetSystem() + ".aspx" + Request.Url.Query;
		Response.Redirect(Page.ResolveClientUrl(url));
	}
}