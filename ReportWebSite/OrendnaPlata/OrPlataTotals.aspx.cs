using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using System.Data.SqlClient;

public partial class OrendnaPlata_OrPlataTotals : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        ButtonShowFoldersPopup.Visible = userIsReportManager;

        // Check if this is first loading of this page
        object uniqueKey = ViewState["PageUniqueKey"];
        bool isFirstLoading = (uniqueKey == null);

        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // If user-defined report is displayed, switch to the proper page of the Page control
        if (isFirstLoading)
        {
            string reportTitle = "";
            string preFilter = "";
            var fixedColumns = string.Empty;

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDOrendnaPlata_Totals)
            {
                LabelReportTitle4.Text = reportTitle;
                Utils.RestoreFixedColumns(PrimaryGridView, fixedColumns);

                if (preFilter.Length > 0)
                {
                    ParseRentTotalsPreFilter(preFilter);
                }
            }
            else
            {
                // Restore grid fixed columns
                Utils.RestoreFixedColumns(PrimaryGridView);
            }
        }

        if (!IsCallback && !IsPostBack)
        {
            ViewState["GridViewRentTotals.DataSourceID"] = PrimaryGridView.DataSourceID;
            PrimaryGridView.DataSourceID = null;
        }
        else if (IsCallback)
        {
            System.Web.UI.Control callbackTarget = FindControl(Request.Params["__CALLBACKID"]);

            if (string.IsNullOrEmpty(PrimaryGridView.DataSourceID))
            {
                SetRentTotalsGridDataSource();

                if (callbackTarget == PrimaryGridView)
                    PrimaryGridView.DataBind();
            }
        }

        // Enable advanced header filter for all grid columns
        Utils.AdjustColumnsVisibleInFilter(PrimaryGridView);

        // Disable the 'Vipiski' menu item if user is RDA
        if (Utils.RdaDistrictID > 0)
        {
            SectionMenu.Items[6].ClientVisible = false;
        }
    }

    protected void SetRentTotalsGridDataSource()
    {
        if (RbRentTotalsActiveAgreements.Checked)
        {
            PrimaryGridView.DataSourceID = "SqlDataSourceRentTotalsActive";
        }
        else if (RbRentTotalsInactiveAgreements.Checked)
        {
            PrimaryGridView.DataSourceID = "SqlDataSourceRentTotalsInactive";
        }
        else
        {
            PrimaryGridView.DataSourceID = "SqlDataSourceRentTotalsAll";
        }
    }

    protected void ASPxButton_RentTotals_ExportXLS_Click(object sender, EventArgs e)
    {
        SetRentTotalsGridDataSource();
        PrimaryGridView.DataBind();

        this.ExportGridToXLS(GridViewRentTotalsExporter, PrimaryGridView, LabelReportTitle4.Text, "");
    }

    protected void ASPxButton_RentTotals_ExportPDF_Click(object sender, EventArgs e)
    {
        SetRentTotalsGridDataSource();
        PrimaryGridView.DataBind();

        this.ExportGridToPDF(GridViewRentTotalsExporter, PrimaryGridView, LabelReportTitle4.Text, "");
    }

    protected void ASPxButton_RentTotals_ExportCSV_Click(object sender, EventArgs e)
    {
        SetRentTotalsGridDataSource();
        PrimaryGridView.DataBind();

        this.ExportGridToCSV(GridViewRentTotalsExporter, PrimaryGridView, LabelReportTitle4.Text, "");
    }

    protected void GridViewRentTotals_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;

        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 35);

        if (param == "rebind:")
        {
            SetRentTotalsGridDataSource();
            PrimaryGridView.DataBind();
        }
        else
        {
            Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView,
                Utils.GridIDOrendnaPlata_Totals, GetRentTotalsPreFilter());
        }
    }

    protected void GridViewRentTotals_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.ASPxEditors.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewRentTotals_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected const string rentTotalsPreFilterAllAgreements = "[T]";
    protected const string rentTotalsPreFilterActiveAgreements = "[A]";
    protected const string rentTotalsPreFilterInactiveAgreements = "[I]";

    protected string GetRentTotalsPreFilter()
    {
        if (RbRentTotalsActiveAgreements.Checked)
        {
            return rentTotalsPreFilterActiveAgreements;
        }
        else if (RbRentTotalsInactiveAgreements.Checked)
        {
            return rentTotalsPreFilterInactiveAgreements;
        }
        else
        {
            return rentTotalsPreFilterAllAgreements;
        }
    }

    protected void ParseRentTotalsPreFilter(string preFilter)
    {
        RbRentTotalsActiveAgreements.Checked = (preFilter == rentTotalsPreFilterActiveAgreements);
        RbRentTotalsInactiveAgreements.Checked = (preFilter == rentTotalsPreFilterInactiveAgreements);
        RbRentTotalsAllAgreements.Checked = (preFilter == rentTotalsPreFilterAllAgreements);
    }

    protected void SqlDataSourceRentTotalsAll_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@RDA_DISTRICT_ID"].Value = Utils.RdaDistrictID;
    }

    protected void SqlDataSourceRentTotalsActive_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@RDA_DISTRICT_ID"].Value = Utils.RdaDistrictID;
    }

    protected void SqlDataSourceRentTotalsInactive_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@RDA_DISTRICT_ID"].Value = Utils.RdaDistrictID;
    }

    protected string GetPageUniqueKey()
    {
        object key = ViewState["PageUniqueKey"];

        if (key is string)
        {
            return (string)key;
        }

        // Generate unique key
        Guid guid = Guid.NewGuid();

        string str = guid.ToString();

        ViewState["PageUniqueKey"] = str;

        return str;
    }

    #region Data binding support

    public string EvaluateObjectLink(object buildingId, object address)
    {
        if (!(address is string))
        {
            return "";
        }

        if (!(buildingId is int))
        {
            return address.ToString();
        }

        return "<a href=\"javascript:ShowObjectCardSimple(" + buildingId.ToString() + ")\">" + address.ToString() + "</a>";
    }

    public string EvaluateOrganizationLink(object organizationId, object name)
    {
        if (!(name is string))
        {
            return "";
        }

        if (!(organizationId is int))
        {
            return name.ToString();
        }

        return "<a href=\"javascript:ShowOrganizationCard(" + organizationId.ToString() + ")\">" + name.ToString() + "</a>";
    }

    #endregion (Data binding support)
}