using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using GUKV.Common;

public partial class Reports1NF_Report1NFList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		//var gg = this.Master;
		//var ribbon = ((ASPxRibbon)Utils.FindControlRecursive(this.Master, "MainRibbon"));
		//var tabs = ribbon.Tabs;
		//ribbon.ActiveTabIndex = 2;
		//var ritem = ribbon.Tabs[2].Groups[0].Items[0] as RibbonButtonItem;
		//ritem.ItemStyle.ForeColor = System.Drawing.Color.Blue;

		//SectionMenu.Visible = Roles.IsUserInRole(Utils.Report1NFReviewerRole);
		SectionMenu.Visible = false;

		var cols = ASPxFileManagerPhotoFiles.SettingsFileList.DetailsViewSettings.Columns;
        cols.Add(FileInfoType.Thumbnail, " ");
        var col1 = cols.Add(FileInfoType.FileName, "Файл");
        col1.Width = 430;
        var col2 = cols.Add(FileInfoType.LastWriteTime, "Дата");
        col2.SortOrder = DevExpress.Data.ColumnSortOrder.Descending;



        // The 'Notifications' page must be visible only to users that can receive some notifications
        if (Roles.IsUserInRole(Utils.DKVOrganizationControllerRole) ||
            Roles.IsUserInRole(Utils.DKVObjectControllerRole) ||
            Roles.IsUserInRole(Utils.DKVArendaControllerRole) ||
            Roles.IsUserInRole(Utils.DKVArendaPaymentsControllerRole))
        {
            SectionMenu.Items[2].Visible = true;
        }
        else
        {
            SectionMenu.Items[2].Visible = false;
        }


		if (Roles.IsUserInRole(Utils.RDAControllerRole))
		{
			//SectionMenuForRDARole.Visible = true;
		}

        if (Roles.IsUserInRole(Utils.MISTOControllerRole))
		{
            ParmMistoId = Utils.GetUserMistoId("");
        }



            

        if (IsSmallForm)
		{
            SectionMenuForSmallMode.Visible = true;
            SectionMenu.Visible = false;
            //PrimaryGridView.Set.
            var commandColumns = PrimaryGridView.Columns.OfType<GridViewCommandColumn>().ToArray();
			foreach(var commandColumn in commandColumns)
			{
				commandColumn.ShowEditButton = false;
			}
        }
		else if (Roles.IsUserInRole(Utils.RDAControllerRole))
		{
			SectionMenu.Visible = false;
			var commandColumns = PrimaryGridView.Columns.OfType<GridViewCommandColumn>().ToArray();
			foreach (var commandColumn in commandColumns)
			{
				commandColumn.ShowEditButton = false;
			}
		}


		PrimaryGridView.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;
		PrimaryGridView.SettingsEditing.Mode = GridViewEditingMode.Inline;

		if (IsReportForm)
		{
			CustomizeReportForm();
		}

		if (SMode == 2)
		{
			LabelReportTitle1.Text = @"Інші суб'єкти використання";
			this.Title = LabelReportTitle1.Text;
		}
		else if (SMode == 1)
		{
			LabelReportTitle1.Text = @"Перелік балансоутримувачів";
			this.Title = LabelReportTitle1.Text;
		}
		else if (SMode == 0)
		{
			LabelReportTitle1.Text = @"Реєстр користувачів майна";
			this.Title = LabelReportTitle1.Text;
		}

		SectionMenuForSmallMode.Visible = false;

		//ForTest();
	}

	void ForTest()
	{
		var lines = new List<string>();
		var zags = System.IO.File.ReadAllLines(@"C:\Users\ASUS\Documents\SQL Server Management Studio\qqqq2232.txt", System.Text.Encoding.GetEncoding("windows-1251"));
		foreach(var zag in zags)
		{
			var column = PrimaryGridView.Columns.OfType<GridViewDataColumn>().Single(q => getNormalizeString(q.Caption) == getNormalizeString(zag));
			var findcol = column.FieldName;

			//lines.Add(findcol + " as [" + zag + "]");
			lines.Add(findcol);
		}

		var sql = string.Join(",\n", lines);
	}

	string getNormalizeString(string arg)
	{
		return arg.Replace("″", "\"");
	}


	void CustomizeReportForm()
	{
		if (!IsPostBack)
		{
			PrimaryGridView.FilterExpression = @"[dict_rent_occupation_name] <> 'Невизначені' And [dict_rent_occupation_name] <> 'Невідомо' And [NumOfObj] >= 1";
		}

		PrimaryGridView.SettingsCookies.Enabled = false;
		ASPxButtonEditColumnList.Visible = false;
		ASPxPopupControl_Reports1NF_SaveAs.Visible = false;
		//ASPxButton_Reports1NF_SaveAs.Visible = false;
		ASPxButtonZvedeniyBuild.Visible = false;
		SectionMenu.Visible = false;

		//PrimaryGridView.Columns["zkpo_code"].fi

		var visibleColumns = new[]
		{
			@"Назва Організації",
			@"Коротка Назва Організації",
			@"Код ЄДРПОУ",
			@"Сфера діяльності",
			@"Район",
			@"Назва Вулиці",
			@"Номер Будинку",
			@"Поштовий Індекс",
			@"Галузь",
			@"Вид Діяльності",
			@"Форма фінансування",
			@"Форма Власності",
			@"Госп. Структура",
			@"Орг.-правова форма госп.",
			@"Орган госп. упр.",
			@"ФІО Директора",
			@"Відповідальна особа",
			@"ФІО Бухгалтера",
			@"Тел. Бухгалтера",
			@"Реєстраційний Орган",
			@"Номер Запису про Реєстрацію",
			@"Дата Реєстрації",
			@"Номер Свідоцтва про Реєстрацію",
			@"КВЕД",
			@"Ел. Адреса",
			@"Кількість об'єктів на балансі",
			@"Ставка відрахувань до бюджету (%)",
			@"Загальна площа, що знаходиться на балансі, кв.м.",
			@"Площа, що орендується, кв.м.",
			@"Загальна площа, що надається в оренду, кв.м.",
			@"Кількість договорів оренди",
			@"Кількість орендарів",
			@"Кількість договорів орендування",
			@"Загальна вільна площа, що може бути надана в оренду, кв.м.",
			@"Нараховано орендної плати за звітний період, грн. (без ПДВ)",
			@"Надходження орендної плати за звітний період, всього, грн. (без ПДВ)",
			@"Переплата орендної плати всього, грн. (без ПДВ)",
			@"Отримано орендної плати в тому числі інші платежі",
			@"Загальна заборгованість по орендній платі, грн. (без ПДВ)",
			@"Нарахована сума до бюджету % від загальної суми надходжень орендної плати за звітний період, грн. (без ПДВ)",
			@"Перераховано коштів до бюджету, у звітному періоді ″КАЗНАЧЕЙСТВО″, грн. (без ПДВ)",
			@"Заборгованість зі сплати % до бюджету від оренди майна за  звітний період, грн. (без ПДВ)",
			@"Перераховано до бюджету за користування індивідуально визначеним майном ('Київенерго' та 'Водоканал')",
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


    protected bool IsSmallForm
    {
        get
        {
            return Request.QueryString["smallform"] == null ? false : true;
        }
    }

	protected int SMode
	{
		get
		{
			return Request.QueryString["smode"] == null ? 0 : Int32.Parse(Request.QueryString["smode"]);
		}
	}

	protected bool IsReportForm
	{
		get
		{
			return Request.QueryString["reportform"] == null ? false : true;
		}
	}

	protected void ASPxButton_Reports1NF_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewReports1NFExporter, PrimaryGridView, LabelReportTitle1.Text, "");
    }

    protected void ASPxButton_Reports1NF_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewReports1NFExporter, PrimaryGridView, LabelReportTitle1.Text, "");
    }

    protected void ASPxButton_Reports1NF_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewReports1NFExporter, PrimaryGridView, LabelReportTitle1.Text, "");
    }

    protected void GridViewReports1NF_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        Utils.ProcessDataGridSaveLayoutCallback(e.Parameters, PrimaryGridView, Utils.GridIDReports1NF_ReportList, "");
        //PrimaryGridView.DataBind();
    }

    protected void GridViewReports1NF_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewReports1NF_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void SqlDataSourceReports_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
        e.Command.Parameters["@period_year"].Value = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
        e.Command.Parameters["@p_misto_id"].Value = ParmMistoId;
		e.Command.Parameters["@smode"].Value = SMode;
	}

	protected void SqlDataSourceReports_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		var dbparams = (System.Data.SqlClient.SqlParameterCollection)(e.Command.Parameters);
		dbparams.AddWithValue("@stan_recieve_date", DateTime.Now);

		//var user = Membership.GetUser();
		//var username = (user == null ? String.Empty : (String)user.UserName);
		//dbparams.AddWithValue("@modified_by2", username);
	}

    protected void ASPxButton_Zvedeniy_Build(object sender, EventArgs e)
    {
        var builder = new ZvedZvitBuilder
        {
            Page = this,
        };
        builder.Go();
    }

	protected void ASPxButton_UpdatePlanuvania(object sender, EventArgs e)
	{
		var connection = Utils.ConnectToDatabase();
		using (SqlCommand cmd = new SqlCommand("exec [update_planuvania]", connection))
		{
			cmd.ExecuteNonQuery();
		}
		//PrimaryGridView.DataBind();
		System.Threading.Thread.Sleep(1000);
		//gridParticipantes.DataBind();
	}

	int ParmMistoId;
	



}