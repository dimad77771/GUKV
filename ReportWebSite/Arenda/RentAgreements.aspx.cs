using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using Cache;

public partial class Arenda_RentAgreements : System.Web.UI.Page, CachingPageIdSupport
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        ButtonShowFoldersPopup1.Visible = userIsReportManager;

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

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDArenda_Agreements)
            {
                LabelReportTitle1.Text = reportTitle;
                Utils.RestoreFixedColumns(PrimaryGridView, fixedColumns);
            }
            else
            {
                // Restore grid fixed columns
                Utils.RestoreFixedColumns(PrimaryGridView);
            }
        }

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);

        // Enable advanced header filter for all grid columns
        Utils.AdjustColumnsVisibleInFilter(PrimaryGridView);

		PrimaryGridView.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;

		if (IsReportForm)
		{
			CustomizeReportForm();
		}
        else
        {
            CustomizeColumnList();
        }
    }

    void CustomizeColumnList()
    {
        if (this.Request.Cookies[PrimaryGridView.SettingsCookies.CookiesID] != null)
        {
            return;
        }

        var visibleColumns = new[]
        {
            @"Картка",
            @"Балансоутримувач - Повна Назва",
            @"Балансоутримувач - Код ЄДРПОУ",
            @"Орендар - Коротка Назва",
            @"Орендодавець - Коротка Назва",
            @"Дата укладання договору",
            @"Номер Договору Оренди",
            @"Призначення за Документом",
            @"Плата за використання, грн.",
            @"Максимальна Орендна Плата за об'єкт договору (грн.)",
            @"Початок Оренди",
            @"Закінчення Оренди",
            @"Площа (кв.м.)",
            @"Вид Розрахунків",
            @"Стан договору",
            @"Дата Актуальності",
            @"Нараховано орендної плати за звітний період, грн. (без ПДВ)",
            @"Надходження орендної плати за звітний період, всього, грн. (без ПДВ)",
            @"Загальна заборгованість по орендній платі - всього",
            @"Сфера діяльності",
        };
        int npp = 0;
        foreach (GridViewColumn column in PrimaryGridView.Columns)
        {
            var visible = visibleColumns.Any(colnam => Utils.EqualColumnTitle(column, colnam));
            column.Visible = visible;
            if (column.Visible) npp++;
        }
        npp = npp;
    }

    void CustomizeReportForm()
	{
		if (!IsPostBack)
		{
			PrimaryGridView.FilterExpression = @"[agreement_active_s] = 'Договір діє'";
		}

		PrimaryGridView.SettingsCookies.Enabled = false;
		ASPxButtonEditColumnList.Visible = false;
		ASPxButtonEditColumnList2.Visible = false;
		//ASPxButton_ArendaObjects_SaveAs.Visible = false;
		CheckBoxRentedObjectsDPZ.Visible = false;
		CheckBoxRentedObjectsComVlasn.Visible = false;
		//CheckBoxBalansObjectsDPZ.Visible = false;
		ButtonQuickSearchAddr1.Visible = false;
		SectionMenu.Visible = false;

		//PrimaryGridView.Columns["zkpo_code"].fi

		var visibleColumns = new[]
		{
			@"Балансоутримувач - Повна Назва",
			@"Балансоутримувач - Код ЄДРПОУ",
			@"Балансоутримувач - Вид Діяльності",
			@"Орендар - Повна Назва",
			@"Орендар - Код ЄДРПОУ",
			@"Орендар - Вид Діяльності",
			@"Орендодавець - Повна Назва",
			@"Орендодавець - Код ЄДРПОУ",
			@"Район",
			@"Назва Вулиці",
			@"Номер Будинку",
			@"Дата укладання договору",
			@"Номер Договору Оренди",
			@"Призначення за Документом",
			@"Плата за використання, грн.",
			@"Оціночна вартість приміщень за договором, грн",
			@"Початок Оренди",
			@"Закінчення Оренди",
			@"Площа (кв.м.)",
			@"Вид Розрахунків",
			@"Стан договору",
			@"Балансоутримувач - Форма Власності",
			@"Ставка відрахувань до бюджету (%)",
			@"Дата Актуальності",
			@"Нараховано орендної плати за звітний період, грн. (без ПДВ)",
			@"Надходження орендної плати за звітний період, всього, грн. (без ПДВ)",
			@"Переплата орендної плати всього, грн. (без ПДВ)",
			@"Загальна заборгованість по орендній платі - всього",
			@"Заборгованість по орендній платі поточна до 3-х місяців",
			@"Заборгованість по орендній платі прострочена від 4 до 12 місяців",
			@"Заборгованість по орендній платі прострочена від 1 до 3 років",
			@"Заборгованість по орендній платі безнадійна більше 3-х років",
			@"Сфера діяльності",
			@"Орендар - Форма власності",
		};
		int npp = 0;
		foreach (GridViewColumn column in PrimaryGridView.Columns)
		{
			var visible = visibleColumns.Any(colnam => Utils.EqualColumnTitle(column, colnam));
			column.Visible = visible;
			if (column.Visible) npp++;
		}
		npp = npp;
	}


	protected bool IsReportForm
	{
		get
		{
			return Request.QueryString["reportform"] == null ? false : true;
		}
	}


	protected void ASPxButton_ArendaObjects_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterArendaObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_ArendaObjects_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterArendaObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_ArendaObjects_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterArendaObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewArendaObjects_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;
        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 60);

        Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDArenda_Agreements, "");
    }

    protected void GridViewArendaObjects_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewArendaObjects_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
    {
        Utils.CheckCustomSummaryVisibility(sender as ASPxGridView, e);

        // Get the field name for which the summary is being calculated
        string field = "";

        if (e.Item is ASPxSummaryItem)
        {
            field = (e.Item as ASPxSummaryItem).FieldName.ToLower();
        }

        if (field.StartsWith("sqr_free_"))
        {
            Utils.CustomSummaryUniqueBalansOrgAndObject(e, "org_balans_id", true);
        }
        else
        {
            Utils.CustomSummaryExcludeSubarenda(e);
        }
    }

    protected void GridViewArendaObjects_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void GridViewArendaObjects_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
    }

    protected void SqlDataSourceArendaObjects_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
		e.Command.CommandTimeout = 600;

		e.Command.Parameters["@p_dpz_filter"].Value = CheckBoxRentedObjectsDPZ.Checked ? 1 : 0;
        e.Command.Parameters["@p_com_filter"].Value = CheckBoxRentedObjectsComVlasn.Checked ? 1 : 0;
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
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

	public string GetCachingPageId()
	{
		return GetPageUniqueKey();
	}
}