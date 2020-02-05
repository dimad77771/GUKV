using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTreeView;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ASPxTreeView treeView = LoginView1.FindControl("TreeViewStdReports") as ASPxTreeView;
        ASPxGridView gridView = LoginView1.FindControl("GridViewStdReports") as ASPxGridView;

        // Check if the user is logged in
        if (Membership.GetUser() != null)
        {
            if (Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
            {
                Response.Redirect(Page.ResolveClientUrl("~/Reports1NF/Cabinet.aspx"));
            }

            // If the hierarchical data source for the tree of reports does not exist, create it
            if (treeView != null)
            {
                treeView.DataSource = TreeViewStdReportsDataSource;
                treeView.DataBind();
                treeView.ExpandAll();

                if (treeView.SelectedNode == null)
                {
                    // Perform initial selection
                    int folderId = FolderId;

                    if (folderId > 0)
                    {
                        TreeViewNode node = FindFolderNode(treeView.Nodes, folderId);

                        if (node != null)
                            treeView.SelectedNode = node;
                    }
                }
            }

            // The command column should be visible to users in "ReportManager" role only
            bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

            GridViewStdReports.Columns[3].Visible = userIsReportManager;

            //if (Roles.IsUserInRole(Utils.ConveyancingConfirmation))
            if (Roles.IsUserInRole("ConveyancingConfirmation"))
            {
                ShowCountOfAktRequests();
            }
            else
            {
                LabelAktRequestsInfo.Visible = false;
                BtnConveyancingConfirmation.Visible = false;
            }
        }

        // If user is RDA, he should not see the reports
        if (Utils.RdaDistrictID > 0)
        {
            LinkSysReports.Visible = false;
            LabelMainTitle.Visible = false;

            if (treeView != null)
                treeView.Visible = false;

            if (gridView != null)
                gridView.Visible = false;
        }
    }

    private TreeViewNode FindFolderNode(TreeViewNodeCollection nodes, int folderId)
    {
        foreach (TreeViewNode node in nodes)
        {
            UserReportDataSourceFolderItem folder = node.DataItem as UserReportDataSourceFolderItem;

            if (folder == null)
                continue;

            if (folder.id == folderId)
                return node;

            TreeViewNode subNode = FindFolderNode(node.Nodes, folderId);

            if (subNode != null)
                return subNode;
        }

        return null;
    }

    protected UserReportDataSource TreeViewStdReportsDataSource
    {
        get
        {
            UserReportDataSource dataSource = Session["MainPageDataSource"] as UserReportDataSource;

            if (dataSource == null)
            {
                dataSource = new UserReportDataSource();
                Session["MainPageDataSource"] = dataSource;
            }

            return dataSource;
        }
    }

    protected void TreeViewStdReports_NodeDataBound(object source, TreeViewNodeEventArgs e)
    {
        UserReportDataSourceFolderItem folder = ((e.Node.DataItem as IHierarchyData).Item as UserReportDataSourceFolderItem);
        UserReportDataSourceReportItem report = ((e.Node.DataItem as IHierarchyData).Item as UserReportDataSourceReportItem);

        if (folder != null)
        {
            int reportCount = folder.GetChildren()
                .Cast<IHierarchyData>()
                .Where(x => x is UserReportDataSourceReportItem)
                .Count();

            if (reportCount > 0)
                e.Node.Text += string.Format(" ({0} звітів)", reportCount);

            if (folder.parentFolder == null)
                e.Node.TextStyle.Font.Size = new FontUnit(24, UnitType.Pixel);
            else
                e.Node.TextStyle.Font.Size = new FontUnit(1.3, UnitType.Em);
        }
        else if (report != null)
        {
            e.Node.Visible = false;
            e.Node.ClientVisible = false;
        }
    }

    private int FolderId
    {
        get
        {
            object folderIdObj = Session["FolderId"];

            if (folderIdObj is int)
            {
                return (int)folderIdObj;
            }

            return 0;
        }
        set
        {
            Session["FolderId"] = value;
        }
    }

    private bool SetFolderId(string nodeName)
    {
        FolderId = 0;

        if (nodeName.StartsWith(Utils.FolderTreeNodeToken))
        {
            int folderId;

            if (int.TryParse(nodeName.Substring(Utils.FolderTreeNodeToken.Length), out folderId))
            {
                FolderId = folderId;
                return true;
            }
        }

        return false;
    }

    protected void GridViewStdReports_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        SetFolderId(e.Parameters);

        // Re-binding the datasource will re-run the query with the new parameter
        ASPxGridView gridView = LoginView1.FindControl("GridViewStdReports") as ASPxGridView;

        if (gridView != null)
        {
            gridView.DataBind();
        }

        // Perform node selection
        int folderId = FolderId;

        if (folderId > 0)
        {
            ASPxTreeView treeView = LoginView1.FindControl("TreeViewStdReports") as ASPxTreeView;

            if (treeView != null)
            {
                TreeViewNode node = FindFolderNode(treeView.Nodes, folderId);

                if (node != null)
                    treeView.SelectedNode = node;
            }
        }
    }

    protected void GridViewStdReports_DataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        // Provide a value for the query parameter (if known)
        e.Command.Parameters["@FolderID"].Value = FolderId;
    }

    protected void GridViewStdReports_RowDeleted(object sender, ASPxDataDeletedEventArgs e)
    {
        // Refresh the tree of folders when some report is deleted
        ASPxTreeView treeView = LoginView1.FindControl("TreeViewStdReports") as ASPxTreeView;

        if (treeView != null)
        {
            treeView.DataSource = TreeViewStdReportsDataSource;
            treeView.DataBind();
            treeView.ExpandAll();
        }
    }

    protected void ShowCountOfAktRequests()
    {
        var reqCount = 0;
        SqlConnection connection = Utils.ConnectToDatabase();
        if (connection != null)
        {
            var query = @"
                SELECT Count(*) 
                FROM transfer_requests req   
                LEFT JOIN 
	                (
		                SELECT organization_id,
		                CAST(CASE WHEN count(*) > 0 THEN 1
			                 ELSE 0 
		                END AS BIT) AS has_account
		                FROM reports1nf_accounts
		                group by organization_id
	                ) AS acc
                ON req.org_id_for_confirm = acc.organization_id	
                WHERE 
                (req.is_object_exists = 0 AND req.status = 1)
                OR
                (req.org_id_for_confirm is null
                OR acc.has_account <> 1 OR acc.has_account is null)
                AND (req.status = 1) AND (req.is_object_exists = 1)
                ";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            reqCount = reader.GetInt32(0);
                    }
                }
            }
            connection.Close();
            LabelAktRequestsInfo.Text = string.Format("В системі знаходиться {0} непідтверджених актів", reqCount);
        }
    }
}
