using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cards_AssessmentCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string valuationIdStr = Request.QueryString["vid"];

        if (!IsPostBack && valuationIdStr != null && valuationIdStr.Length > 0)
        {
            SqlDataSourceAssessmentProperties.SelectParameters["vid"].DefaultValue = valuationIdStr.Trim();
            SqlDataSourceAssessmentInputDoc.SelectParameters["vid"].DefaultValue = valuationIdStr.Trim();
            SqlDataSourceAssessmentOutputDoc.SelectParameters["vid"].DefaultValue = valuationIdStr.Trim();
            SqlDataSourceAssessmentDetails.SelectParameters["vid"].DefaultValue = valuationIdStr.Trim();
        }
    }
}