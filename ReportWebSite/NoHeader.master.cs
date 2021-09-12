using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class NoHeader : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Utils.FindUserOrganizationAndRda("");

        bool userIsReportReviewer = Roles.IsUserInRole(Utils.Report1NFReviewerRole);
        bool userIsRdaController = Roles.IsUserInRole(Utils.RDAControllerRole);
        bool userIsMistoController = Roles.IsUserInRole(Utils.MISTOControllerRole);

		/*
        // If user does not belong to the role "1NFReportReviewer", hide the menu items related to 1NF reports
        if (!userIsReportReviewer && !userIsRdaController && !userIsMistoController)
        {
            foreach (DevExpress.Web.MenuItem item in MainMenu.Items)
            {
                if (item.NavigateUrl.Contains("Reports1NF"))
                {
                    item.ClientVisible = false;
                    break;
                }
            }
        }

        // If user belongs to the RDA role, hide some items
        if (userIsRdaController)
        {
            foreach (DevExpress.Web.MenuItem item in MainMenu.Items)
            {
                if (item.NavigateUrl.Contains("/Default") ||
                    item.NavigateUrl.Contains("/Documents") ||
                    item.NavigateUrl.Contains("/Finance") ||
                    item.NavigateUrl.Contains("/Privatization") ||
                    item.NavigateUrl.Contains("/Assessment"))
                {
                    item.ClientVisible = false;
                }
            }
        }

        // If user belongs to the RDA role, hide some items
        if (userIsMistoController)
        {
            foreach (DevExpress.Web.MenuItem item in MainMenu.Items)
            {
                if (item.NavigateUrl.Contains("/Default") ||
                    item.NavigateUrl.Contains("/Documents") ||
                    item.NavigateUrl.Contains("/Finance") ||
                    item.NavigateUrl.Contains("/Privatization") ||
                    item.NavigateUrl.Contains("/Assessment") ||
                    item.NavigateUrl.Contains("/OrPlataByBalans") ||
                    item.NavigateUrl.Contains("/RentAgreements") ||
                    item.NavigateUrl.Contains("/RishProjects") ||
                    item.NavigateUrl.Contains("/BalansObjects") ||
                    item.NavigateUrl.Contains("/AddrCatalogue"))
                {
                    item.ClientVisible = false;
                }
            }
        }
		*/

		// If user is not authenticated, hide the main menu
		if (Membership.GetUser() == null)
        {
			MainRibbon.Visible = false;

			//foreach (DevExpress.Web.MenuItem item in MainMenu.Items)
   //         {
   //             item.ClientVisible = false;
   //         }
        }

		var reportID = Utils.GetLastReportId();

		if (reportID > 0)
		{
			MainMenuRDA.Visible = true;
			MainRibbon.Visible = false;
			for (int i = 0; i < MainMenuRDA.Items.Count; i++)
			{
				MainMenuRDA.Items[i].NavigateUrl += "?rid=" + reportID;
			}
		}
	}
}
