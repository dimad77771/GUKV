using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// A hierarchical data source of user-defined folders
/// </summary>
[Serializable]
public class UserFoldersDataSource : IHierarchicalDataSource
{
    public UserFoldersDataSource()
    {
    }

#pragma warning disable 0067

    public event EventHandler DataSourceChanged;

#pragma warning restore 0067

    public HierarchicalDataSourceView GetHierarchicalView(string viewPath)
    {
        if (viewPath == "")
        {
            return new UserFoldersDataSourceView(null);
        }
        else
        {
            int folderId = int.Parse(viewPath);

            return new UserFoldersDataSourceView(new UserReportDataSourceFolderItem(null, "", folderId, false, false));
        }
    }
}
