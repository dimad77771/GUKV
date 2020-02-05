using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using DevExpress.Web.ASPxGridView;

public partial class Balans_BalansInventarizations : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        {
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("name", typeof(string));

            for (int month = 1; month <= 12; month++)
            {
                table.Rows.Add(month, string.Format("{0} - {1}", month, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)));
            }
/*
            ((GridViewDataComboBoxColumn)PrimaryGridView.Columns["ColumnCommissionedDateMonth"]).PropertiesComboBox.DataSource = table;
            ((GridViewDataComboBoxColumn)PrimaryGridView.Columns["ColumnDecommissionedDateMonth"]).PropertiesComboBox.DataSource = table;
            ((GridViewDataComboBoxColumn)PrimaryGridView.Columns["ColumnDocumentDateMonth"]).PropertiesComboBox.DataSource = table;
            ((GridViewDataComboBoxColumn)PrimaryGridView.Columns["ColumnOnBalanceDateMonth"]).PropertiesComboBox.DataSource = table;

*/
            }
        {
            DataTable table = new DataTable();
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("begin_date", typeof(string));
            table.Columns.Add("end_date", typeof(string));

            int periodLength = 12;

            DateTime currentPeriodBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month - (DateTime.Now.Month - 1) % periodLength, 1);
            for (int count = 12; count >= 0; count--)
            {
                DateTime currentPeriodEnd = currentPeriodBegin.AddMonths(periodLength).AddDays(-1);
                table.Rows.Add(string.Format("{0} {1}-{2}", currentPeriodBegin.Year,
                        CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentPeriodBegin.Month),
                        CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentPeriodEnd.Month)),
                    currentPeriodBegin.ToString("o"),
                    currentPeriodEnd.ToString("o"));
                currentPeriodBegin = currentPeriodBegin.AddMonths(-periodLength);
            }

/*            ASPxComboBoxPeriod.DataSource = table;
            ASPxComboBoxPeriod.DataBind();
            if (ASPxComboBoxPeriod.SelectedIndex < 0)
                ASPxComboBoxPeriod.SelectedIndex = 0;
*/
        }
    }

    protected void SqlDataSourceInventarizations_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
/*       
        e.Command.Parameters["@changes_only"].Value = CheckBoxShowChangesOnly.Checked;
        e.Command.Parameters["@period_begin_date"].Value = DateTime.Parse((string)((DataTable)ASPxComboBoxPeriod.DataSource).Rows[ASPxComboBoxPeriod.SelectedIndex]["begin_date"]);
        e.Command.Parameters["@period_end_date"].Value = DateTime.Parse((string)((DataTable)ASPxComboBoxPeriod.DataSource).Rows[ASPxComboBoxPeriod.SelectedIndex]["end_date"]);

 */ 
    }

    protected void GridViewBalansInventarizations_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;
        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 60);

        //Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDBalans_Arenda, "");
    }

    protected void ASPxButton_BalansInventarizations_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterBalansInventarizations, PrimaryGridView, LabelReportTitle3.Text);
    }

    protected void ASPxButton_BalansInventarizations_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterBalansInventarizations, PrimaryGridView, LabelReportTitle3.Text);
    }

    protected void ASPxButton_BalansInventarizations_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterBalansInventarizations, PrimaryGridView, LabelReportTitle3.Text);
    }
}
