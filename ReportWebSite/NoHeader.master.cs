using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;

public partial class NoHeader : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Utils.FindUserOrganizationAndRda("");

        bool userIsReportReviewer = Roles.IsUserInRole(Utils.Report1NFReviewerRole);
        bool userIsRdaController = Roles.IsUserInRole(Utils.RDAControllerRole);
        bool userIsMistoController = Roles.IsUserInRole(Utils.MISTOControllerRole);
        bool userIsOcenka = Roles.IsUserInRole(Utils.OcenkaRole);
        bool userIsChmo400 = Roles.IsUserInRole(Utils.Chmo400Role);

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
			if (userIsRdaController)
			{
				foreach(var tab in MainRibbon.Tabs.Where(q => !new[] { "Користувачі майна", "Майно (Об'єкти)", "Оренда", "Каталог", "Контроль використання", "Довідка", "Реєстри" }.Contains(q.Text)))
				{
					tab.Visible = false;
				}
			}
			else
			{
				MainMenuRDA.Visible = true;
				MainRibbon.Visible = false;
				for (int i = 0; i < MainMenuRDA.Items.Count; i++)
				{
					MainMenuRDA.Items[i].NavigateUrl += "?rid=" + reportID;
				}
			}
		}

		if (!Roles.IsUserInRole(Utils.AdministratorRole))
		{
			foreach(var mitem in MainRibbon.Tabs.Single(q => q.Text == "Адміністрування").Groups.Single().Items.Cast<RibbonButtonItem>()
				.Where(q => q.NavigateUrl.Contains("Register.aspx") || q.NavigateUrl.Contains("ManageRoles.aspx")))
			{
				mitem.Visible = false;
			}
		}

        if (userIsRdaController)
        {
            var menu = MainRibbon.Tabs.Single(q => q.Text == "Контроль використання");
            var smenu = menu.Groups[0].Items.Single(q => q.Text == "Надходження до бюджету");
            smenu.Visible = false;
        }


        if (!userIsOcenka)
        {
            var menu = MainRibbon.Tabs.Single(q => q.Text == "Оцінка");
            menu.Visible = false;
        }

        if (!userIsChmo400)
        {
            var menu = MainRibbon.Tabs.Single(q => q.Text == "Реєстри");
            var smenu = menu.Groups[0].Items.Single(q => q.Text == "Запити на передопрацьовані вільні приміщення");
            smenu.Visible = false;
        }

    }

	RibbonTab MainMenuTab(string text)
	{
		return MainRibbon.Tabs.Single(q => q.Text == text);
	}
}
