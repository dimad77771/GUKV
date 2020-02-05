using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;

public partial class SubmitterList_SubmitterList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button_Submitters_ExportXLS_Click(object sender, EventArgs e)
    {
        Utils.SetGridExporterProps(GridViewSubmitters, GridViewSubmittersExporter, LabelReportTitle.Text, this);

        this.ExportGridToXLS(GridViewSubmittersExporter);
    }

    protected void Button_Submitters_ExportPDF_Click(object sender, EventArgs e)
    {
        Utils.SetGridExporterProps(GridViewSubmitters, GridViewSubmittersExporter, LabelReportTitle.Text, this);

        this.ExportGridToPDF(GridViewSubmittersExporter);
    }

    protected void Button_Submitters_ExportCSV_Click(object sender, EventArgs e)
    {
        Utils.SetGridExporterProps(GridViewSubmitters, GridViewSubmittersExporter, LabelReportTitle.Text, this);

        this.ExportGridToCSV(GridViewSubmittersExporter);
    }

    protected void GridViewSubmitters_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string fieldChooserRequest = "columns:";

        if (e.Parameters.StartsWith(fieldChooserRequest))
        {
            string listOfFields = e.Parameters.Substring(fieldChooserRequest.Length);

            foreach (GridViewDataColumn column in GridViewSubmitters.Columns)
            {
                if (column.ShowInCustomizationForm)
                {
                    if (listOfFields.Contains("#" + column.FieldName + "++#"))
                    {
                        column.Visible = true;
                    }
                    else if (listOfFields.Contains("#" + column.FieldName + "--#"))
                    {
                        column.Visible = false;
                    }
                }
            }
        }
    }

    protected void GridViewSubmitters_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.ASPxEditors.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, GridViewSubmitters);
    }
}