using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// A hierarchical data source of user-defined reports
/// </summary>
[Serializable]
public class UserReportDataSource : IHierarchicalDataSource
{
    public UserReportDataSource()
    {
    }

#pragma warning disable 0067

    public event EventHandler DataSourceChanged;

#pragma warning restore 0067

    public HierarchicalDataSourceView GetHierarchicalView(string viewPath)
    {
        if (viewPath == "")
        {
            return new UserReportDataSourceView(null);
        }
        else
        {
            int folderId = int.Parse(viewPath);

            return new UserReportDataSourceView(new UserReportDataSourceFolderItem(null, "", folderId, true, true));
        }
    }
}
