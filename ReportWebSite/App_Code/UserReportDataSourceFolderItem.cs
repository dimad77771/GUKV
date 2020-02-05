using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Data.SqlClient;

/// <summary>
/// A Folder item within hierarchical data source os user-defined reports
/// </summary>
public class UserReportDataSourceFolderItem : IHierarchyData
{
    public UserReportDataSourceFolderItem parentFolder = null;

    public string name = "";

    public int id = 0;

    public bool useGlobalURLsForReports = true;

    private bool needToEnumerateReports = true;

    private UserReportDataSourceEnumerable childItems = new UserReportDataSourceEnumerable();

    public UserReportDataSourceFolderItem(UserReportDataSourceFolderItem parent,
        string folderName, int folderId, bool needReports, bool globalReportURLs)
	{
        parentFolder = parent;
        name = folderName;
        id = folderId;
        needToEnumerateReports = needReports;
        useGlobalURLsForReports = globalReportURLs;

        // Check if there are some subfolders or reports in this folder
        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // If the folder name is empty, get it from the database
                if (name == "")
                {
                    using (SqlCommand command = new SqlCommand("SELECT folder_name FROM user_folders " +
                        " WHERE (id = @fid)", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@fid", id));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                name = (string)reader.GetValue(0);
                            }

                            reader.Close();
                        }
                    }
                }

                // Check if subfolders or subreports exist
                CheckIfChildFoldersExist((System.Guid)uid, connection);

                if (needToEnumerateReports)
                {
                    CheckIfChildReportsExist((System.Guid)uid, connection);
                }

                connection.Close();
            }
        }
	}

    #region IHierarchyData implementation

    public IHierarchicalEnumerable GetChildren()
    {
        return childItems;
    }

    public IHierarchyData GetParent()
    {
        return parentFolder;
    }

    public bool HasChildren
    {
        get
        {
            return childItems.Count > 0;
        }
    }

    public object Item
    {
        get
        {
            return this;
        }
    }

    public string Path
    {
        get
        {
            return id.ToString();
        }
    }

    public string Type
    {
        get
        {
            return GetType().Name;
        }
    }

    #endregion (IHierarchyData implementation)

    /// <summary>
    /// This property is required to allow ASPxTreeView to find out the name of tree node
    /// </summary>
    public string Name
    {
        get
        {
            return Utils.FolderTreeNodeToken + id.ToString();
        }
    }

    /// <summary>
    /// This property is required to allow ASPxTreeView to find out the text of tree node
    /// </summary>
    public string Text
    {
        get
        {
            return name;
        }
    }

    #region Implementation

    private void CheckIfChildFoldersExist(System.Guid uid, SqlConnection connection)
    {
        // Fill the list of child items
        using (SqlCommand command = new SqlCommand("SELECT id, folder_name FROM user_folders " +
            "WHERE (parent_folder_id = @pfid) ORDER BY id", connection))
        {
            command.Parameters.Add(new SqlParameter("@pfid", id));

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    childItems.Add(new UserReportDataSourceFolderItem(
                        this,
                        (string)reader.GetValue(1),
                        (int)reader.GetValue(0),
                        needToEnumerateReports,
                        useGlobalURLsForReports));
                }

                reader.Close();
            }
        }
    }

    private void CheckIfChildReportsExist(System.Guid uid, SqlConnection connection)
    {
        // Fill the list of child items
        using (SqlCommand command = new SqlCommand("SELECT id, report_name, category FROM user_reports " +
            "WHERE (folder_id = @fid) ORDER BY id", connection))
        {
            command.Parameters.Add(new SqlParameter("@fid", id));

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    childItems.Add(new UserReportDataSourceReportItem(this,
                        (string)reader.GetValue(1),
                        (int)reader.GetValue(0),
                        (int)reader.GetValue(2)));
                }

                reader.Close();
            }
        }
    }

    #endregion (Implementation)
}