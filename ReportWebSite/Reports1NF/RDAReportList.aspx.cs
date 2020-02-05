using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports1NF_RDAReportList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Utils.FindUserOrganizationAndRda("");

        SqlDataSourceRdaReports.SelectParameters["distr_id"].DefaultValue = Utils.RdaDistrictID.ToString();
    }
}