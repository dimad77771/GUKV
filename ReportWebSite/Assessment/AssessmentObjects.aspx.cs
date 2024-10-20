﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;

public partial class Assessment_AssessmentObjects : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        ButtonShowFoldersPopup1.Visible = userIsReportManager;

        // Check if this is first loading of this page
        object uniqueKey = ViewState["PageUniqueKey"];
        bool isFirstLoading = (uniqueKey == null);

        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // If user-defined report is displayed, switch to the proper page of the Page control
        if (isFirstLoading)
        {
            string reportTitle = "";
            string preFilter = "";
            var fixedColumns = string.Empty;

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDAssessment_Objects)
            {
                LabelReportTitle1.Text = reportTitle;
                Utils.RestoreFixedColumns(PrimaryGridView, fixedColumns);
            }
            else
            {
                // Restore grid fixed columns
                Utils.RestoreFixedColumns(PrimaryGridView);
            }
        }

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);

        // Enable advanced header filter for all grid columns
        Utils.AdjustColumnsVisibleInFilter(PrimaryGridView);
    }

    protected void ASPxButton_AssessmentObjects_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewAssessmentObjectsExporter, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_AssessmentObjects_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewAssessmentObjectsExporter, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_AssessmentObjects_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewAssessmentObjectsExporter, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

	private class GridCallbackData
	{
		public int AgreementToDeleteID { get; set; }
	}
	protected void GridViewAssessmentObjects_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
		if (e.Parameters != null && e.Parameters.Contains("AgreementToDeleteID"))
		{ 
			var data = e.Parameters.FromJSON<GridCallbackData>();
			var user = Membership.GetUser();
			var username = (user == null ? "System" : user.UserName);

			SqlConnection connection = Utils.ConnectToDatabase();

			if (connection != null)
			{
				using (SqlCommand cmd = new SqlCommand("UPDATE expert_note SET is_deleted = 1, modified_by = @usr, modify_date = @dt WHERE id = @id", connection))
				{
					cmd.Parameters.Add(new SqlParameter("id", data.AgreementToDeleteID));
					cmd.Parameters.Add(new SqlParameter("usr", username));
					cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));

					cmd.ExecuteNonQuery();
				}

				connection.Close();
			}

			PrimaryGridView.DataSourceID = "SqlDataSourceAssessmentObjects";
			PrimaryGridView.DataBind();

			return;
		}

		string param = e.Parameters;

        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 35);

        Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDAssessment_Objects, "");
    }

    protected void GridViewAssessmentObjects_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewAssessmentObjects_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void GridViewAssessmentObjects_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName.StartsWith("in_") ||
            e.Column.FieldName.StartsWith("out_"))
		{
			e.DisplayText = e.Value.ToString().Replace("\n", "<br />");
		}
	}

    protected void GridViewAssessmentObjects_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
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

    #region Data binding support

    public string EvaluateObjectLink(object buildingId, object address)
    {
        if (!(address is string))
        {
            return "";
        }

        if (!(buildingId is int))
        {
            return address.ToString();
        }

        return "<a href=\"javascript:ShowObjectCardSimple(" + buildingId.ToString() + ")\">" + address.ToString() + "</a>";
    }

    public string EvaluateOrganizationLink(object organizationId, object name)
    {
        if (!(name is string))
        {
            return "";
        }

        if (!(organizationId is int))
        {
            return name.ToString();
        }

        return "<a href=\"javascript:ShowOrganizationCard(" + organizationId.ToString() + ")\">" + name.ToString() + "</a>";
    }

	public string EvaluateMultiLineValue(object value)
	{
		if (string.IsNullOrEmpty("" + value))
		{
			return "";
		}

		return value.ToString().Replace("\n", "<br />");
	}

	public string EvaluateСontrolDate(object value)
	{
		if (string.IsNullOrEmpty("" + value))
		{
			return "";
		}

		var result = "";
		var lines = ("" + value).Split('\n');
		foreach(var line in lines)
		{
			if (result != "")
			{
				result += "<br />";
			}

			var color = "";
			DateTime date;
			if (DateTime.TryParseExact(line, @"dd.MM.yyyy", null, DateTimeStyles.None, out date))
			{
				if (date.Date <= DateTime.Today)
				{
					color = "red";
				}
			}

			result += @"<span style='color:" + color + "'>" + line + @"</span>";
		}

		return result;
		//return value.ToString().Replace("\n", "<br />");
	}

	#endregion (Data binding support)

	protected void PrimaryGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if (e.RowType == GridViewRowType.Data)
		{
			var value = e.GetValue("background_color_rgb");
			if (value != System.DBNull.Value)
			{ 
				var vv = Convert.ToInt32(value);
				if (vv != 0)
				{
					e.Row.BackColor = Color.FromArgb(vv);
				}
			}
		}
	}

	protected void PrimaryGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		var hh = 10;
	}
}