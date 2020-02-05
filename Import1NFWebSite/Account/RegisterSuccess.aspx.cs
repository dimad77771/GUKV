using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;
using System.Data.SqlClient;

public partial class Account_RegisterSuccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // [Stanislav Shevchenko] In the current version all user-defined reports are shared
        // between users, so no need to pre-create reports from web.config

        /*
        if (Membership.GetUser() != null)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // Create predefined folders and reports for the new user
                CreatePredefinedFoldersAndReports(connection);

                connection.Close();
            }
        }
        */
    }

    /*
    protected void CreatePredefinedFoldersAndReports(SqlConnection connection)
    {
        foreach (DefUserFolder folder in DefReportsConfig.GetConfig().Folders)
        {
            int folderId = Utils.AddUserFolder(folder.Name);

            if (folderId > 0)
            {
                foreach (DefUserReport report in folder.Reports)
                {
                    Utils.AddUserReport(folderId, report.GridID, report.Name, report.Layout);
                }
            }
        }
    }
    */
}