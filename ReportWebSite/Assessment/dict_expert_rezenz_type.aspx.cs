using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using System.Drawing;

public partial class Catalogue_OrgCatalogue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		//Response.Cache.SetNoStore();

		//bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

		//ButtonShowFoldersPopup2.Visible = userIsReportManager;

		// Check if this is first loading of this page
		//object uniqueKey = ViewState["PageUniqueKey"];
		//bool isFirstLoading = (uniqueKey == null);

		// Generate a unique key for this instance of the page
		//GetPageUniqueKey();

		//// If user-defined report is displayed, switch to the proper page of the Page control
		//if (isFirstLoading)
		//{
		//    string reportTitle = "";
		//    string preFilter = "";
		//    var fixedColumns = string.Empty;

		//    if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDCatalogue_Organizations)
		//    {
		//        LabelReportTitle2.Text = reportTitle;
		//        Utils.RestoreFixedColumns(PrimaryGridView, fixedColumns);
		//    }
		//    else
		//    {
		//        // Restore grid fixed columns
		//        Utils.RestoreFixedColumns(PrimaryGridView);
		//    }
		//}

		//// Bind data to the grid dynamically
		//this.ProcessGridDataFetch(ViewState, PrimaryGridView);

		//// Enable advanced header filter for all grid columns
		//Utils.AdjustColumnsVisibleInFilter(PrimaryGridView);
	}

	protected void ASPxButton_AllOrganizations_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewAllOrganizationsExporter, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_AllOrganizations_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewAllOrganizationsExporter, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_AllOrganizations_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewAllOrganizationsExporter, PrimaryGridView, LabelReportTitle2.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected string GetPageUniqueKey()
    {
        object key = ViewState["PageUniqueKey"];

        if (key is string)
        {
            return (string)key;
        }

        // Generate unique key
        Guid guid = Guid.NewGuid();

        string str = guid.ToString();

        ViewState["PageUniqueKey"] = str;

        return str;
    }

	protected void PrimaryGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.FieldName == "color_rgb")
		{
			if (e.CellValue is int)
			{
				var value = (int)e.CellValue;
				if (value == 0)
				{
					e.Cell.BackColor = Color.Transparent;
				}
				else
				{
					e.Cell.BackColor = Color.FromArgb(value);
				}
			}
		}
	}

	protected void ASPxColorEdit1_Init(object sender, EventArgs e)
	{
		ASPxColorEdit edt = sender as ASPxColorEdit;
		GridViewEditItemTemplateContainer container = edt.NamingContainer as GridViewEditItemTemplateContainer;

		var name = DataBinder.Eval(container.DataItem, "name");
		if (name == null)
		{
			edt.Visible = false;
		}
		else
		{
			edt.Visible = true;
			var vv = DataBinder.Eval(container.DataItem, "color_rgb");
			var value = vv == null ? 0 : (int)(vv);
			Int32 color = Convert.ToInt32(value);
			edt.Color = value == 0 ? Color.Transparent : Color.FromArgb(color);
		}
	}

	protected void PrimaryGridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		ASPxColorEdit edt = PrimaryGridView.FindEditRowCellTemplateControl(PrimaryGridView.Columns["color_rgb"] as GridViewDataColumn, "ASPxColorEdit1") as ASPxColorEdit;
		e.NewValues["color_rgb"] = edt.Color.ToArgb();
	}

	protected void PrimaryGridView_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
	{
		e.NewValues["color_rgb"] = 0;
	}

	protected void PrimaryGridView_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
	{
		ASPxColorEdit edt = PrimaryGridView.FindEditRowCellTemplateControl(PrimaryGridView.Columns["color_rgb"] as GridViewDataColumn, "ASPxColorEdit1") as ASPxColorEdit;
		e.NewValues.Add("color_rgb", edt.Color.ToArgb());
	}
}