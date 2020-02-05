using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;

public partial class Cards_ReportViewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        ASPxGridView grid = null;
        string[] autoGroupFields = null;
        string[] primaryFields = null;
        string[] secondaryFields = null;

        string gridToken = Request.QueryString["grid"];

        if (gridToken != null && gridToken.Length > 0)
        {
            gridToken = gridToken.Trim().ToLower();

            switch (gridToken)
            {
                case "finance":
                    grid = Utils.GridViewFinance;
                    autoGroupFields = Utils.FinanceGrid_AutoGroupFields;
                    primaryFields = Utils.FinanceGrid_PrimaryFields;
                    secondaryFields = Utils.FinanceGrid_SecondaryFields;
                    break;
            }
        }

        if (grid != null && autoGroupFields != null && primaryFields != null && secondaryFields != null)
        {
            GeneralReport report = new GeneralReport();

            report.FillDetailFromGrid(grid, autoGroupFields, primaryFields, secondaryFields);

            MainReportViewer.Report = report;
        }
        */
    }
}