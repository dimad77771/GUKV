using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for UserReportDataSourceReportItem
/// </summary>
public class UserReportDataSourceReportItem : IHierarchyData
{
    private UserReportDataSourceFolderItem parentFolder = null;

    private UserReportDataSourceEnumerable children = new UserReportDataSourceEnumerable();

    private string linkToReport = "";

    public string name = "";

    public int id = 0;

    public int gridId = 0;

    public UserReportDataSourceReportItem(UserReportDataSourceFolderItem parent,
        string reportName, int reportId, int grId)
	{
        parentFolder = parent;
        name = reportName;
        id = reportId;
        gridId = grId;

        // Generate the link that points to the report
        switch (gridId)
        {
            case Utils.GridIDObjects_Buildings:
            case Utils.GridIDObjects_Transfer:
                linkToReport = "Objects/Objects.aspx?grid=" + gridId.ToString() + "&report=" + id.ToString();
                break;

            case Utils.GridIDBalans_Organizations:
            case Utils.GridIDBalans_Objects:
                linkToReport = "Balans/BalansReports.aspx?grid=" + gridId.ToString() + "&report=" + id.ToString();
                break;

            case Utils.GridIDArenda_Organizations:
            case Utils.GridIDArenda_Objects:
                linkToReport = "Arenda/ArendaReports.aspx?grid=" + gridId.ToString() + "&report=" + id.ToString();
                break;

            case Utils.GridIDDocuments_Documents:
            case Utils.GridIDDocuments_Objects:
                linkToReport = "Documents/DocsView.aspx?grid=" + gridId.ToString() + "&report=" + id.ToString();
                break;

            case Utils.GridIDPrivatization_Documents:
            case Utils.GridIDPrivatization_Objects:
                linkToReport = "Privatization/PrivatizationView.aspx?grid=" + gridId.ToString() + "&report=" + id.ToString();
                break;

            case Utils.GridID_Finance:
                linkToReport = "Finance/FinanceView.aspx?grid=" + gridId.ToString() + "&report=" + id.ToString();
                break;
        }

        // Make URL global or relative, depending on the parent folder setting
        if (parent != null)
        {
            if (parent.useGlobalURLsForReports)
            {
                linkToReport = "~/" + linkToReport;
            }
            else
            {
                linkToReport = "../" + linkToReport;
            }
        }
	}

    #region IHierarchyData implementation

    public IHierarchicalEnumerable GetChildren()
    {
        return children;
    }

    public IHierarchyData GetParent()
    {
        return parentFolder;
    }

    public bool HasChildren
    {
        get
        {
            return false;
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
            return parentFolder.id.ToString() + "/" + id.ToString();
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
            return Utils.ReportTreeNodeToken + id.ToString();
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

    /// <summary>
    /// This property is required to allow ASPxTreeView to find out the URL of tree node
    /// </summary>
    public string NavigateUrl
    {
        get
        {
            return linkToReport;
        }
    }
}