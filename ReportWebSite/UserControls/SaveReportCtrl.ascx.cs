using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class UserControls_SaveReportCtrl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // If the hierarchical data source does not exist, create it
        TreeViewUserFolders.DataSource = TreeViewFoldersDataSource;
        TreeViewUserFolders.DataBind();
    }

    protected UserFoldersDataSource TreeViewFoldersDataSource
    {
        get
        {
            object dataSource = Session["UserFoldersDataSource"];

            if (dataSource == null)
            {
                dataSource = new UserFoldersDataSource();

                Session["UserFoldersDataSource"] = dataSource;
            }

            return dataSource as UserFoldersDataSource;
        }
    }

    protected void CallbackPanelTreeViewUserFolders_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        string param = e.Parameter;

        string addFolder = "addfolder:";
        string delFolder = "delfolder:";
        string refresh = "refresh:";

        if (param.StartsWith(addFolder))
        {
            string folderNameAndParent = param.Substring(addFolder.Length);

            int separatorPos = folderNameAndParent.IndexOf(':');

            if (separatorPos > 0)
            {
                string parentNodeName = folderNameAndParent.Substring(0, separatorPos);
                string folderName = folderNameAndParent.Substring(separatorPos + 1);

                string parentFolderIdStr = parentNodeName.Substring(Utils.FolderTreeNodeToken.Length);
                int parentFolderId = int.Parse(parentFolderIdStr);

                Utils.AddUserFolder(parentFolderId, folderName);

                // Refresh the tree
                TreeViewUserFolders.DataSource = TreeViewFoldersDataSource;
                TreeViewUserFolders.DataBind();
            }
        }
        else if (param.StartsWith(delFolder))
        {
            string folderNodeName = param.Substring(delFolder.Length);
            string folderId = folderNodeName.Substring(Utils.FolderTreeNodeToken.Length);

            Utils.DeleteUserFolder(int.Parse(folderId));

            // Refresh the tree
            TreeViewUserFolders.DataSource = TreeViewFoldersDataSource;
            TreeViewUserFolders.DataBind();
        }
        else if (param == refresh)
        {
            // Just refresh the tree
            TreeViewUserFolders.DataSource = TreeViewFoldersDataSource;
            TreeViewUserFolders.DataBind();
        }
    }

    protected void TreeViewUserFolders_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
    {
        ASPxTreeView grid = sender as ASPxTreeView;

        // Enumerate names of all user-defined reports
        System.Collections.ArrayList listReportNames = new System.Collections.ArrayList();

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            using (SqlCommand command = new SqlCommand("SELECT id, report_name FROM user_reports", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object name = reader.IsDBNull(1) ? null : reader.GetValue(1); ;

                        if (name is string)
                        {
                            listReportNames.Add(name);
                        }
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        e.Properties["cpReportNames"] = listReportNames.ToArray();
        e.Properties["cpReportNameCount"] = listReportNames.Count;
    }
}