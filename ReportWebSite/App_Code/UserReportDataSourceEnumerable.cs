using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// A collection of items within hierarchical data source os user-defined reports
/// </summary>
public class UserReportDataSourceEnumerable : System.Collections.ArrayList, IHierarchicalEnumerable
{
	public UserReportDataSourceEnumerable()
	{
	}

    public IHierarchyData GetHierarchyData(object enumeratedItem)
    {
        return enumeratedItem as IHierarchyData;
    }
}
