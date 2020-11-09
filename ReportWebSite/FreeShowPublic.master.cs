using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class FreeShowPublic : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Utils.FindUserOrganizationAndRda("");

        //bool userIsReportReviewer = Roles.IsUserInRole(Utils.Report1NFReviewerRole);
        //bool userIsRdaController = Roles.IsUserInRole(Utils.RDAControllerRole);

        //// If user does not belong to the role "1NFReportReviewer", hide the menu items related to 1NF reports
        //if (!userIsReportReviewer && !userIsRdaController)
        //{
        //    foreach (DevExpress.Web.MenuItem item in MainMenu.Items)
        //    {
        //        if (item.NavigateUrl.Contains("Reports1NF"))
        //        {
        //            item.ClientVisible = false;
        //            break;
        //        }
        //    }
        //}

        //// If user belongs to the RDA role, hide some items
        //if (userIsRdaController)
        //{
        //    foreach (DevExpress.Web.MenuItem item in MainMenu.Items)
        //    {
        //        if (item.NavigateUrl.Contains("/Default") ||
        //            item.NavigateUrl.Contains("/Documents") ||
        //            item.NavigateUrl.Contains("/Finance") ||
        //            item.NavigateUrl.Contains("/Privatization") ||
        //            item.NavigateUrl.Contains("/Assessment"))
        //        {
        //            item.ClientVisible = false;
        //        }
        //    }
        //}

        //// If user is not authenticated, hide the main menu
        //if (Membership.GetUser() == null)
        //{
        //    foreach (DevExpress.Web.MenuItem item in MainMenu.Items)
        //    {
        //        item.ClientVisible = false;
        //    }
        //}
    }
}
