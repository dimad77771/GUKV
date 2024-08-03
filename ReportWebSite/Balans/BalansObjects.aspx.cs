using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using Cache;

public partial class Balans_BalansObjects : System.Web.UI.Page, CachingPageIdSupport
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

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDBalans_Objects)
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

        //ForTest();
    }

    void ForTest()
    {
        var lines = new List<string>();
        //var zags = System.IO.File.ReadAllLines(@"C:\Users\ASUS\Documents\SQL Server Management Studio\qqqq2238.txt", System.Text.Encoding.GetEncoding("windows-1251"));
        var zags = PrimaryGridView.Columns.OfType<GridViewDataColumn>().Select(q => getNormalizeString(q.Caption)).ToArray();
        foreach (var zag in zags)
        {
            var column = PrimaryGridView.Columns.OfType<GridViewDataColumn>().Single(q => getNormalizeString(q.Caption) == getNormalizeString(zag));
            var findcol = column.FieldName;

            lines.Add(findcol + " as [" + zag + "]");
            //lines.Add(findcol);
        }

        var sql = string.Join(",\n", lines);
    }

    string getNormalizeString(string arg)
    {
        return arg.Replace("″", "\"");
    }

    void CustomizeColumnList()
    {
		if (this.Request.Cookies[PrimaryGridView.SettingsCookies.CookiesID] != null)
		{
			return;
		}

		var visibleColumns = new[]
		{
            @"Балансоутримувач - Повна Назва",
            @"Балансоутримувач - Код ЄДРПОУ",
            @"Район",
            @"Назва Вулиці",
            @"Номер Будинку",
            @"Орган госп. упр.",
            @"Загальна Площа будинку (кв.м.)",
            @"Площа нежилих приміщень будинку (кв.м.)",
            @"Площа нежилих приміщень об'єкту (кв.м.)",
            @"Площа об'єкту Для Власних Потреб (кв.м.)",
            @"Площа вільних приміщень (кв.м.)",
            @"Кількість Договорів Оренди",
            @"Вид Об'єкту відповідно Класифікатора майна",
            @"Тип Об'єкту",
            @"Назва Об'єкту",
            @"Дата Актуальності",
            @"Сфера діяльності",
            @"Контактні телефони",
            @"Док. на право користування об’єктом",
            @"Док. що підтверджує передачу на баланс",
            @"Документація БТІ",
            @"Реєстрація у реєстрі речових прав",
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
			PrimaryGridView.FilterExpression = @"[sphera_dialnosti] <> 'Невизначені' And [sphera_dialnosti] <> 'Невідомо'";
		}

		PrimaryGridView.SettingsCookies.Enabled = false;
		ASPxButtonEditColumnList.Visible = false;
		ASPxButtonEditColumnList2.Visible = false;
		//ASPxButton_BalansObjects_SaveAs.Visible = false;
		CheckBoxBalansObjectsShowDeleted.Visible = false;
        CheckBoxBalansObjectsShowNeziznacheni.Visible = false;
        CheckBoxBalansObjectsComVlasn.Visible = false;
		CheckBoxBalansObjectsDPZ.Visible = false;
		ButtonQuickSearchAddr1.Visible = false;
		SectionMenu.Visible = false;

		//PrimaryGridView.Columns["zkpo_code"].fi

		var visibleColumns = new[]
		{
			@"Балансоутримувач - Повна Назва",
			@"Балансоутримувач - Коротка Назва",
			@"Балансоутримувач - Код ЄДРПОУ",
			@"Балансоутримувач - Вид Діяльності",
			@"Район",
			@"Назва Вулиці",
			@"Номер Будинку",
			@"Площа вільних приміщень (кв.м.)",
			@"Площа нежилих приміщень об'єкту (кв.м.)",
			@"Площа об'єкту Для Власних Потреб (кв.м.)",
			@"Корисна площа об'єкту (кв.м.)",
			@"Залишкова Вартість, тис.грн.",
			@"Інвентаризаційний № справи",
			@"Дата виготовлення технічного паспорту",
			@"Реєстрація у Державному реєстрі (Об'єкт нерухомого майна)",
			@"Реєстрація у Державному реєстрі (Номер запису про право власності)",
			@"Реєстрація у Державному реєстрі (Реєстраційний номер об'єкту нерухомого майна)",
			@"Інвентарний номер об'єкту",
			@"Рік будівництва",
			@"Рік здачі в експлуатацію",
			@"Форма Власності Об'єкту",
			@"Вид об'єкту відповідно Класифікатора майна",
			@"Тип Об'єкту",
			@"Стан Об'єкту",
			@"Група Призначення",
			@"Призначення",
			@"Історична Цінність",
			@"Розташування приміщення (поверх)",
			@"Балансоутримувач - Форма Власності",
			@"Назва Об'єкту",
			@"Дата Актуальності",
			@"Сфера діяльності",
			@"Госп. Структура",
			@"Контактні телефони",
            @"Док. на право користування об’єктом",
            @"Док. що підтверджує передачу на баланс",
            @"Документація БТІ",
            @"Реєстрація у реєстрі речових прав",
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


	protected void ASPxButton_BalansObjects_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterBalansObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_BalansObjects_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterBalansObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_BalansObjects_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterBalansObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewBalansObjects_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;
        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 60);

        Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDBalans_Objects, "");
    }

    protected void GridViewBalansObjects_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewBalansObjects_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void SqlDataSourceBalansObjects_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_dpz_filter"].Value = CheckBoxBalansObjectsDPZ.Checked ? 1 : 0;
        e.Command.Parameters["@p_com_filter"].Value = CheckBoxBalansObjectsComVlasn.Checked ? 1 : 0;
        e.Command.Parameters["@p_show_deleted"].Value = CheckBoxBalansObjectsShowDeleted.Checked ? 1 : 0;
        e.Command.Parameters["@p_show_neziznacheni"].Value = CheckBoxBalansObjectsShowNeziznacheni.Checked ? 1 : 0;
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
    }

    protected void GridViewBalansObjects_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
    {
        Utils.CheckCustomSummaryVisibility(PrimaryGridView, e);
        Utils.CustomSummaryUniqueBalansOrgAndObject(e, "organization_id", false);
    }

    protected void GridViewBalansObjects_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
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