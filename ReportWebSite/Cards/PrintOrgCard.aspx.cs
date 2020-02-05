using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class Cards_PrintOrgCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ReportOrgCard report = new ReportOrgCard(Request.QueryString["orgid"]);

        if (IsReportPrinted())
        {
            report.DefaultPrinterSettingsUsing.UsePaperKind = true;
            report.DefaultPrinterSettingsUsing.UseLandscape = true;

            report.CreateDocument();
            report.PrintingSystem.Document.AutoFitToPagesWidth = 1;
        }
        else
        {
            float scaleFactor = 1.0f;

            HtmlInputHidden hf = (HtmlInputHidden)this.Page.FindControl("Scale");

            if (!string.IsNullOrEmpty(hf.Value))
            {
                scaleFactor = Convert.ToSingle(hf.Value) / 100f;
            }

            report.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            report.PageWidth = Convert.ToInt32(report.PageWidth * scaleFactor);
            report.PageHeight = Convert.ToInt32(report.PageHeight * scaleFactor);

            report.CreateDocument();
            report.PrintingSystem.Document.ScaleFactor = scaleFactor;
        }

        this.ReportViewer1.Report = report;
        this.ReportViewer1.DataBind();
    }

    protected bool IsReportPrinted()
    {
        return (
            ((Request.Params["__EVENTTARGET"] == ReportViewer1.UniqueID) && (Request.Params["__EVENTARGUMENT"].Contains("showPrintDialog:true"))) ||
            ((Request.Params["__EVENTTARGET"] == ReportViewer1.UniqueID) && (Request.Params["__EVENTARGUMENT"].Contains("saveToDisk"))) ||
            ((Request.Params["__CALLBACKID"] == ReportViewer1.UniqueID) && (Request.Params["__CALLBACKPARAM"].Contains("print=idx")))
            );
    }
}