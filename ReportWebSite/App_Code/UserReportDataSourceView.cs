using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Data.SqlClient;

/// <summary>
/// Represents a View within hierarchical data source os user-defined reports
/// </summary>
public class UserReportDataSourceView : HierarchicalDataSourceView
{
    private UserReportDataSourceFolderItem folder = null;

    public UserReportDataSourceView(UserReportDataSourceFolderItem f)
	{
        folder = f;
	}

    public override IHierarchicalEnumerable Select()
    {
        UserReportDataSourceEnumerable items = new UserReportDataSourceEnumerable();

        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                if (folder == null)
                {
                    // Enumerate all primary folders (which have no parent folder)
                    string queryPrimaryFolders = "SELECT id, folder_name FROM user_folders " +
                        "WHERE parent_folder_id IS NULL ORDER BY id";

                    using (SqlCommand command = new SqlCommand(queryPrimaryFolders, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new UserReportDataSourceFolderItem(
                                    null, /* No parent folder */
                                    (string)reader.GetValue(1), /* Folder name */
                                    (int)reader.GetValue(0), /* Folder ID */
                                    true, /* Enumerate reports */
                                    true /* Use global URLs for report sub-nodes */ ));
                            }

                            reader.Close();
                        }
                    }
                }
                else
                {
                    // Enumerate subfolders or user-defined reports in particular folder
                    IHierarchicalEnumerable subItems = folder.GetChildren();

                    if (subItems != null)
                    {
                        System.Collections.IEnumerator enumerator = subItems.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            items.Add(enumerator.Current);
                        }
                    }
                }

                // Close the connection
                connection.Close();
            }
        }

        return items;
    }
}
