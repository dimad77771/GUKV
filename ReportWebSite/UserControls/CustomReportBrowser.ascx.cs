using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_CustomReportBrowser : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // If the hierarchical data source does not exist, create it
        TreeViewUserReports.DataSource = TreeViewDataSource;
        TreeViewUserReports.DataBind();
    }

    protected UserReportDataSource TreeViewDataSource
    {
        get
        {
            object dataSource = Session["UserReportDataSource"];

            if (dataSource == null)
            {
                dataSource = new UserReportDataSource();

                Session["UserReportDataSource"] = new UserReportDataSource();
            }

            return dataSource as UserReportDataSource;
        }
    }

    protected void CallbackPanelTreeViewUserReports_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        /*
        string param = e.Parameter;

        string addFolder = "addfolder:";
        string delFolder = "delfolder:";
        string delReport = "delreport:";
        string refresh = "refresh:";

        if (param.StartsWith(addFolder))
        {
            string folderName = param.Substring(addFolder.Length).ToUpper();

            Utils.AddUserFolder(folderName);

            // Refresh the tree
            TreeViewUserReports.DataSource = TreeViewDataSource;
            TreeViewUserReports.DataBind();
        }
        else if (param.StartsWith(delFolder))
        {
            string folderNodeName = param.Substring(delFolder.Length);
            string folderId = folderNodeName.Substring(Utils.FolderTreeNodeToken.Length);

            Utils.DeleteUserFolder(int.Parse(folderId));

            // Refresh the tree
            TreeViewUserReports.DataSource = TreeViewDataSource;
            TreeViewUserReports.DataBind();
        }
        else if (param.StartsWith(delReport))
        {
            string reportNodeName = param.Substring(delReport.Length);
            string reportId = reportNodeName.Substring(Utils.ReportTreeNodeToken.Length);

            Utils.DeleteUserReport(int.Parse(reportId));

            // Refresh the tree
            TreeViewUserReports.DataSource = TreeViewDataSource;
            TreeViewUserReports.DataBind();
        }
        else if (param == refresh)
        {
            // Just refresh the tree
            TreeViewUserReports.DataSource = TreeViewDataSource;
            TreeViewUserReports.DataBind();
        }
        */
    }
}