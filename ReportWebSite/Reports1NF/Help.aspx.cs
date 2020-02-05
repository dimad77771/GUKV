using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports1NF_Help : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.Params["rid"];

        int reportId;
        if (!string.IsNullOrEmpty(reportIdStr) && int.TryParse(reportIdStr, out reportId))
        {
            for (int i = 0; i < SectionMenu.Items.Count; i++)
            {
                if (SectionMenu.Items[i].NavigateUrl.IndexOf('?') < 0)
                    SectionMenu.Items[i].NavigateUrl += "?rid=" + reportId;
            }
        }
    }
}