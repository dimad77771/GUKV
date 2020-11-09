using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class SysReports_SysReports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Corrected Errors

    protected void ASPxButton_Corrected_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewCorrectedErrorsExporter, GridViewCorrectedErrors, LabelReportTitle1.Text, "");
    }

    protected void ASPxButton_Corrected_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewCorrectedErrorsExporter, GridViewCorrectedErrors, LabelReportTitle1.Text, "");
    }

    protected void ASPxButton_Corrected_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewCorrectedErrorsExporter, GridViewCorrectedErrors, LabelReportTitle1.Text, "");
    }

    protected void GridViewCorrectedErrors_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;

        Utils.ProcessGridPageSizeInCallback(GridViewCorrectedErrors, ref param, 35);

        Utils.ProcessDataGridSaveLayoutCallback(param, GridViewCorrectedErrors, Utils.GridIDSysReports_Corrected, "");
    }

    protected void GridViewCorrectedErrors_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, GridViewCorrectedErrors);
    }

    protected void GridViewCorrectedErrors_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    #endregion (Corrected Errors)

    #region Mismatches

    protected void ASPxButton_Mismatches_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewMismatchesExporter, GridViewMismatches, LabelReportTitle2.Text, "");
    }

    protected void ASPxButton_Mismatches_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewMismatchesExporter, GridViewMismatches, LabelReportTitle2.Text, "");
    }

    protected void ASPxButton_Mismatches_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewMismatchesExporter, GridViewMismatches, LabelReportTitle2.Text, "");
    }

    protected void GridViewMismatches_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;

        Utils.ProcessGridPageSizeInCallback(GridViewMismatches, ref param, 35);

        Utils.ProcessDataGridSaveLayoutCallback(param, GridViewMismatches, Utils.GridIDSysReports_Mismatches, "");
    }

    protected void GridViewMismatches_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, GridViewMismatches);
    }

    protected void GridViewMismatches_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    #endregion (Mismatches)
}