using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web.ASPxGridView;
using System.Data.SqlClient;

public partial class Documents_DocRelations : System.Web.UI.Page
{
	private class GridCallbackData
	{
		public string AgreementToDeleteID { get; set; }
	}

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

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDDocuments_Documents)
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

    protected void ASPxButton_Documents_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterDocuments, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Documents_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterDocuments, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Documents_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterDocuments, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewDocuments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;
        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 75);

        Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDDocuments_Documents, "");

		if (e.Parameters.StartsWith("init:"))
		{
			// Fake callback; do nothing
		}
		else
		{
			GridCallbackData data = e.Parameters.FromJSON<GridCallbackData>();

			int agreementToDeleteID = -1;
			string s1 = (data.AgreementToDeleteID ?? "");
			var p = s1.IndexOf("|");
			if (p > 0)
			{
				s1 = s1.Substring(0, p);
				Int32.TryParse(s1, out agreementToDeleteID);
			}
			if (agreementToDeleteID > 0)
			{
				// Mark the rent agreement as deleted
				SqlConnection connection = Utils.ConnectToDatabase();

				// -- update
				//select* from arenda_applications where building_id is null
				//select* from arenda_applications where org_renter_id is null
				//select* from building_docs where doc_change_doc_id is null
				//select* from link_arenda_2_decisions where doc_raspor_id is null
				//select* from object_rights where akt_id is null
				//select* from object_rights where rozp_id is null

				// -- delete
				//select* from balans_other_docs where document_id is null
				//select* from bp_rish_project_info where document_id is null
				//select* from building_docs where document_id is null
				//select* from doc_appendices where doc_id is null
				//select* from doc_dependencies where master_doc_id is null
				//select* from doc_dependencies where slave_doc_id is null
				//select* from object_rights_building_docs where document_id is null
				//select* from org_docs where document_id is null
				//select* from priv_object_docs where document_id is null
				//select* from restrictions where document_id is null

				var updatesInfo = new List<Tuple<string, string>>();
				updatesInfo.Add(new Tuple<string, string>("arenda_applications", "building_id"));
				updatesInfo.Add(new Tuple<string, string>("arenda_applications", "org_renter_id"));
				updatesInfo.Add(new Tuple<string, string>("building_docs", "doc_change_doc_id"));
				updatesInfo.Add(new Tuple<string, string>("link_arenda_2_decisions", "doc_raspor_id"));
				updatesInfo.Add(new Tuple<string, string>("object_rights", "akt_id"));
				updatesInfo.Add(new Tuple<string, string>("object_rights", "rozp_id"));

				var deletesInfo = new List<Tuple<string, string>>();
				deletesInfo.Add(new Tuple<string, string>("balans_other_docs", "document_id"));
				deletesInfo.Add(new Tuple<string, string>("bp_rish_project_info", "document_id"));
				deletesInfo.Add(new Tuple<string, string>("building_docs", "document_id"));
				deletesInfo.Add(new Tuple<string, string>("doc_appendices", "doc_id"));
				deletesInfo.Add(new Tuple<string, string>("doc_dependencies", "master_doc_id"));
				deletesInfo.Add(new Tuple<string, string>("doc_dependencies", "slave_doc_id"));
				deletesInfo.Add(new Tuple<string, string>("object_rights_building_docs", "document_id"));
				deletesInfo.Add(new Tuple<string, string>("org_docs", "document_id"));
				deletesInfo.Add(new Tuple<string, string>("priv_object_docs", "document_id"));
				deletesInfo.Add(new Tuple<string, string>("restrictions", "document_id"));


				if (connection != null)
				{
					using (var transaction = connection.BeginTransaction())
					{
						foreach (var uinfo in updatesInfo)
						{
							using (SqlCommand cmd = new SqlCommand("UPDATE " + uinfo.Item1 + " set " + uinfo.Item2 + " = null WHERE " + uinfo.Item2 + " = @id", connection, transaction))
							{
								cmd.Parameters.Add(new SqlParameter("id", agreementToDeleteID));
								cmd.ExecuteNonQuery();
							}
						}

						foreach (var dinfo in deletesInfo)
						{
							using (SqlCommand cmd = new SqlCommand("DELETE FROM " + dinfo.Item1 + " WHERE " + dinfo.Item2 + " = @id", connection, transaction))
							{
								cmd.Parameters.Add(new SqlParameter("id", agreementToDeleteID));
								cmd.ExecuteNonQuery();
							}
						}

						using (SqlCommand cmd = new SqlCommand("DELETE FROM documents WHERE id = @id", connection, transaction))
						{
							cmd.Parameters.Add(new SqlParameter("id", agreementToDeleteID));
							cmd.ExecuteNonQuery();
						}


						transaction.Commit();
					}
					connection.Close();
				}

				//PrimaryGridView.DataSourceID = "SqlDataSourceDocuments";
				//PrimaryGridView.DataBind();
				//(PrimaryGridView.DataSource as Cache.CachingDataSource).Rel
			}

		}
	}

    protected void GridViewDocuments_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.ASPxEditors.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewDocuments_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
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
}