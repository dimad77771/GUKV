using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Syncfusion.XlsIO;

public partial class Reports1NF_Report1NFDogContinue : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SectionMenu.Visible = Roles.IsUserInRole(Utils.Report1NFReviewerRole);

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

		FreeSquareGridView.SettingsEditing.Mode = GridViewEditingMode.Inline;
		GridViewCommission.SettingsEditing.Mode = GridViewEditingMode.Inline;

		if (Utils.IsBigBossUser())
		{
			var col = FreeSquareGridView.Columns.OfType<GridViewCommandColumn>().First();
			col.ShowEditButton = false;
			col.ShowUpdateButton = false;
			col.ShowCancelButton = false;

			var commissionCol = GridViewCommission.Columns.OfType<GridViewCommandColumn>().First();
			commissionCol.ShowEditButton = false;
			commissionCol.ShowUpdateButton = false;
			commissionCol.ShowCancelButton = false;
			commissionCol.ShowNewButton = false;
			commissionCol.ShowDeleteButton = false;
		}

		if (Utils.IsCabinetBalansoderzhatel())
		{
			var col = FreeSquareGridView.Columns.OfType<GridViewCommandColumn>().First();
			col.ShowEditButton = false;
			col.ShowUpdateButton = false;
			col.ShowCancelButton = false;
			col.ShowNewButton = false;
			col.ShowDeleteButton = false;

			var commissionCol = GridViewCommission.Columns.OfType<GridViewCommandColumn>().First();
			commissionCol.ShowEditButton = false;
			commissionCol.ShowUpdateButton = false;
			commissionCol.ShowCancelButton = false;
			commissionCol.ShowNewButton = false;
			commissionCol.ShowDeleteButton = false;
		}


		SectionMenu.Visible = false;

		FreeSquareGridView.TemplateColumnsStyles("may_pravo_prodov_text");
	}

	protected void ASPxButton_FreeSquare_ExportXLS_Click(object sender, EventArgs e)
	{
		this.ExportGridToXLS(GridViewFreeSquareExporter, FreeSquareGridView, LabelReportTitle1.Text, "",
			exportHiddenColumnCallback: q => (q.FieldName == "pdfurl"),
			afterBuildXlsx: ASPxButton_FreeSquare_ExportXLS_AfterBuildXlsx
		);
	}

	void ASPxButton_FreeSquare_ExportXLS_AfterBuildXlsx(string excelfilename)
	{
		var excelEngineMain = new ExcelEngine();
		var workbook = excelEngineMain.Excel.Workbooks.Open(excelfilename);
		var worksheet = workbook.Worksheets[0];

		//var style = workbook.Styles.Add("HrefNewStyle");
		//style.Color = Color.FromArgb(0, 0, 255);
		//style.Font.Underline = ExcelUnderline.Single;

		var coln = 26;
		var allrows = worksheet.Rows.Length;
		for (int rown = 1; rown <= allrows; rown++)
		{
			var cell = worksheet.Range[rown, coln];
			var text = cell.Text ?? "";
			if (text.ToLower().StartsWith("http"))
			{
				worksheet.HyperLinks.Add(cell, ExcelHyperLinkType.Url, text, "Відкрити фото/плани");
				//cell.CellStyle = style;
				cell.CellStyle.Font.Underline = ExcelUnderline.Single;
				cell.CellStyle.Font.Color = ExcelKnownColors.Blue;
			}
		}

		workbook.Save();
		workbook.Close();
		excelEngineMain.Dispose();
	}

	protected void ASPxButton_FreeSquare_ExportPDF_Click(object sender, EventArgs e)
	{
		this.ExportGridToPDF(GridViewFreeSquareExporter, FreeSquareGridView, LabelReportTitle1.Text, "");
	}

	protected void ASPxButton_FreeSquare_ExportCSV_Click(object sender, EventArgs e)
	{
		this.ExportGridToCSV(GridViewFreeSquareExporter, FreeSquareGridView, LabelReportTitle1.Text, "");
	}

	protected void GridViewFreeSquare_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		Utils.ProcessDataGridSaveLayoutCallback(e.Parameters, FreeSquareGridView, Utils.GridIDReports1NF_FreeSquare, "");

		FreeSquareGridView.DataBind();
	}

	protected void GridViewFreeSquare_CustomFilterExpressionDisplayText(object sender,
		DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
	{
		this.UpdateFilterDisplayTextCache(e.DisplayText, FreeSquareGridView);
	}

	protected void GridViewFreeSquare_ProcessColumnAutoFilter(object sender,
		DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
	{
		Utils.ProcessGridColumnAutoFilter(sender, e);
	}

	protected void SqlDataSourceFreeSquare_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
	{
		e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
		e.Command.Parameters["@period_year"].Value = DateTime.Now.Date.Month == 1 ? DateTime.Now.Date.Year - 1 : DateTime.Now.Date.Year;
		e.Command.Parameters["@baseurl"].Value = Utils.WebsiteBaseUrl;
		e.Command.Parameters["@p_show_neziznacheni"].Value = CheckBoxBalansObjectsShowNeziznacheni.Checked ? 1 : 0;
		e.Command.Parameters["@bal_organization_id"].Value = Utils.RdaDistrictID > 0 ? -1 : Utils.UserOrganizationID;
	}

	protected void SqlDataSourceFreeSquare_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		var dbparams = (System.Data.SqlClient.SqlParameterCollection)(e.Command.Parameters);
		dbparams.AddWithValue("@modify_date2", DateTime.Now);
		var user = Membership.GetUser();
		var username = (user == null ? String.Empty : (String)user.UserName);
		dbparams.AddWithValue("@modified_by2", username);

		var geodata_map_points = (string)(e.Command.Parameters["@geodata_map_points"].Value);
		if (!Validate_geodata_map_points(geodata_map_points))
		{
			throw new Exception("Невірно заповнене поле \"Координати на мапі\". Приклад вірно заповненого поля (широта довгота) \"50.509205 30.426741\"");
		}

		var free_square_id = (int)(e.Command.Parameters["@id"].Value);
		var freecycle_step_dict_id = (int?)(e.Command.Parameters["@freecycle_step_dict_id"].Value);
		var current_stage_docdate = (DateTime?)(e.Command.Parameters["@current_stage_docdate"].Value);
		var current_stage_docnum = (string)(e.Command.Parameters["@current_stage_docnum"].Value);
		var current_step = Utils.GetStepContinue(free_square_id);
		var change_step = (freecycle_step_dict_id != current_step);

		if (change_step && new int?[] { 150, 300 }.Contains(freecycle_step_dict_id))
		{
			using (var connection = Utils.ConnectToDatabase())
			using (var transaction = connection.BeginTransaction())
			{
				AfterDogovorReestration(free_square_id, connection, transaction, current_stage_docnum, current_stage_docdate);
				transaction.Commit();
			}
		}
	}

	public static void AfterDogovorReestration(int free_square_id, SqlConnection connection, SqlTransaction transaction, string stage_docnum, DateTime? stage_docdate)
	{
		var username = Utils.GetUser();


		var result = new List<string>();
		var data = Utils.GetDataTable(@"
SELECT 
fs.id,
b.building_id,
rep.report_id,
org.id as orgBalansID,
isnull(ddd.name, 'Невизначені') as sf_upr

FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.org_balans_id
LEFT JOIN (
			select obp.org_id
			, occ.name
			, occ.id
			, per.name as period 
			from org_by_period obp
			join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
			join dict_rent_occupation occ on occ.id = obp.org_occupation_id
				) DDD ON DDD.org_id = rep.organization_id
where fs.id = " + free_square_id,
connection, transaction);

		var row = data.Rows[0];
		var building_id = (int)row["building_id"];
		var report_id = (int)row["report_id"];
		var orgBalansID = (int)row["orgBalansID"];
		var sf_upr = (string)row["sf_upr"];

		var dogparm = new CreateNewArendaDogovorData
		{
			AgreementNum = stage_docnum,
			AgreementDateYear = stage_docdate.Value.Year,
			AgreementDateMonth = stage_docdate.Value.Month,
			AgreementDateDay = stage_docdate.Value.Day,
			BuildingID = building_id,
			OrgBalansID = orgBalansID,
			OrgRenterID = null,
			OrgGiverID = Utils.GetRenterID(sf_upr),
			OrgGiverComment = "",
		};
		Utils.CreateNewArendaDogovor(report_id, username, dogparm);
	}



	bool Validate_geodata_map_points(string geodata_map_points)
	{
		if (string.IsNullOrEmpty(geodata_map_points))
		{
			return true;
		}

		var is_good = false;
		var regpoints = (new Regex(@"^(\d+\.\d+)\s+(\d+\.\d+)$")).Match(geodata_map_points);
		if (regpoints.Groups.Count == 3)
		{
			try
			{
				var point1 = Decimal.Parse(regpoints.Groups[1].Value, CultureInfo.InvariantCulture);
				var point2 = Decimal.Parse(regpoints.Groups[2].Value, CultureInfo.InvariantCulture);
				is_good = true;
			}
			catch (Exception ex)
			{
				is_good = false;
			}
		}

		return is_good;
	}

	private const string DeputiesEditorSessionKey = "DogContinue_DeputiesEditorDataSource";

	private const string DistrictRepresentativesEditorSessionKey = "DogContinue_DistrictRepresentativesEditorDataSource";

	private static readonly string[] AllDistricts = new[]
	{
		"Голосеевский",
		"Дарницкий",
		"Деснянский",
		"Днепровский",
		"Оболонский",
		"Печерский",
		"Подольский",
		"Святошинский",
		"Соломенский",
		"Шевченковский"
	};

	private DataTable CreateDeputiesEditorTable()
	{
		var table = new DataTable();
		table.Columns.Add("id", typeof(int));
		table.Columns.Add("deputy_name", typeof(string));
		return table;
	}

	private DataTable DeputiesEditorDataSource
	{
		get
		{
			var table = Session[DeputiesEditorSessionKey] as DataTable;
			if (table == null)
			{
				table = CreateDeputiesEditorTable();
				Session[DeputiesEditorSessionKey] = table;
			}

			return table;
		}
		set
		{
			Session[DeputiesEditorSessionKey] = value;
		}
	}

	private DataTable CreateDeputiesEditorTableFromString(string deputiesList)
	{
		var table = CreateDeputiesEditorTable();
		if (!string.IsNullOrWhiteSpace(deputiesList))
		{
			var items = deputiesList.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			var id = 1;
			foreach (var item in items)
			{
				var value = item.Trim();
				if (!string.IsNullOrWhiteSpace(value))
				{
					table.Rows.Add(id++, value);
				}
			}
		}

		return table;
	}

	private void BindDeputiesEditorGrid()
	{
		GridViewDeputiesEditor.DataSource = DeputiesEditorDataSource;
		GridViewDeputiesEditor.DataBind();
	}

	protected void ButtonEditDeputiesList_Click(object sender, EventArgs e)
	{
		var memo = GridViewCommission.FindEditRowCellTemplateControl(GridViewCommission.Columns["colDeputiesList"] as GridViewDataColumn, "EditDeputiesListText") as ASPxMemo;
		var deputiesList = memo == null ? string.Empty : memo.Text;

		DeputiesEditorDataSource = CreateDeputiesEditorTableFromString(deputiesList);
		BindDeputiesEditorGrid();
		PopupDeputiesEditor.ShowOnPageLoad = true;
	}

	protected void GridViewDeputiesEditor_DataBinding(object sender, EventArgs e)
	{
		GridViewDeputiesEditor.DataSource = DeputiesEditorDataSource;
	}

	protected void GridViewDeputiesEditor_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		var table = DeputiesEditorDataSource;
		var newId = 1;
		if (table.Rows.Count > 0)
		{
			newId = table.AsEnumerable().Max(q => q.Field<int>("id")) + 1;
		}

		table.Rows.Add(newId, Convert.ToString(e.NewValues["deputy_name"] ?? string.Empty).Trim());
		DeputiesEditorDataSource = table;

		e.Cancel = true;
		GridViewDeputiesEditor.CancelEdit();
		BindDeputiesEditorGrid();
		PopupDeputiesEditor.ShowOnPageLoad = true;
	}

	protected void GridViewDeputiesEditor_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		var table = DeputiesEditorDataSource;
		var id = Convert.ToInt32(e.Keys["id"]);
		var deputyName = Convert.ToString(e.NewValues["deputy_name"] ?? string.Empty).Trim();

		foreach (DataRow row in table.Rows)
		{
			if ((int)row["id"] == id)
			{
				row["deputy_name"] = deputyName;
				break;
			}
		}

		DeputiesEditorDataSource = table;

		e.Cancel = true;
		GridViewDeputiesEditor.CancelEdit();
		BindDeputiesEditorGrid();
		PopupDeputiesEditor.ShowOnPageLoad = true;
	}

	protected void GridViewDeputiesEditor_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		var table = DeputiesEditorDataSource;
		var id = Convert.ToInt32(e.Keys["id"]);

		for (var i = table.Rows.Count - 1; i >= 0; i--)
		{
			if ((int)table.Rows[i]["id"] == id)
			{
				table.Rows.RemoveAt(i);
				break;
			}
		}

		DeputiesEditorDataSource = table;

		e.Cancel = true;
		BindDeputiesEditorGrid();
		PopupDeputiesEditor.ShowOnPageLoad = true;
	}

	protected void ButtonDeputiesEditorOk_Click(object sender, EventArgs e)
	{
		var result = string.Join("; ", DeputiesEditorDataSource.AsEnumerable()
			.Select(q => Convert.ToString(q["deputy_name"]).Trim())
			.Where(q => !string.IsNullOrWhiteSpace(q)));

		var memo = GridViewCommission.FindEditRowCellTemplateControl(GridViewCommission.Columns["colDeputiesList"] as GridViewDataColumn, "EditDeputiesListText") as ASPxMemo;
		if (memo != null)
		{
			memo.Text = result;
		}

		PopupDeputiesEditor.ShowOnPageLoad = false;
	}

	protected void ButtonDeputiesEditorCancel_Click(object sender, EventArgs e)
	{
		PopupDeputiesEditor.ShowOnPageLoad = false;
	}


	private DataTable CreateDistrictRepresentativesEditorTable()
	{
		var table = new DataTable();
		table.Columns.Add("id", typeof(int));
		table.Columns.Add("district_name", typeof(string));
		table.Columns.Add("representative_name", typeof(string));

		for (var i = 0; i < AllDistricts.Length; i++)
		{
			table.Rows.Add(i + 1, AllDistricts[i], string.Empty);
		}

		return table;
	}

	private DataTable DistrictRepresentativesEditorDataSource
	{
		get
		{
			var table = Session[DistrictRepresentativesEditorSessionKey] as DataTable;
			if (table == null)
			{
				table = CreateDistrictRepresentativesEditorTable();
				Session[DistrictRepresentativesEditorSessionKey] = table;
			}

			return table;
		}
		set
		{
			Session[DistrictRepresentativesEditorSessionKey] = value;
		}
	}

	private DataTable CreateDistrictRepresentativesEditorTableFromString(string districtRepresentatives)
	{
		var table = CreateDistrictRepresentativesEditorTable();
		if (string.IsNullOrWhiteSpace(districtRepresentatives))
		{
			return table;
		}

		var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		var parts = districtRepresentatives.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		foreach (var part in parts)
		{
			var item = part.Trim();
			if (string.IsNullOrWhiteSpace(item))
			{
				continue;
			}

			var pos = item.IndexOf(':');
			if (pos <= 0)
			{
				continue;
			}

			var districtName = item.Substring(0, pos).Trim();
			var representativeName = item.Substring(pos + 1).Trim();
			map[districtName] = representativeName;
		}

		foreach (DataRow row in table.Rows)
		{
			var districtName = Convert.ToString(row["district_name"]);
			string representativeName;
			if (!string.IsNullOrWhiteSpace(districtName) && map.TryGetValue(districtName, out representativeName))
			{
				row["representative_name"] = representativeName;
			}
		}

		return table;
	}

	private void BindDistrictRepresentativesEditorGrid()
	{
		GridViewDistrictRepresentativesEditor.DataSource = DistrictRepresentativesEditorDataSource;
		GridViewDistrictRepresentativesEditor.DataBind();
	}

	protected void ButtonEditDistrictRepresentatives_Click(object sender, EventArgs e)
	{
		var memo = GridViewCommission.FindEditRowCellTemplateControl(GridViewCommission.Columns["colDistrictRepresentatives"] as GridViewDataColumn, "EditDistrictRepresentativesText") as ASPxMemo;
		var districtRepresentatives = memo == null ? string.Empty : memo.Text;

		DistrictRepresentativesEditorDataSource = CreateDistrictRepresentativesEditorTableFromString(districtRepresentatives);
		BindDistrictRepresentativesEditorGrid();
		PopupDistrictRepresentativesEditor.ShowOnPageLoad = true;
	}

	protected void GridViewDistrictRepresentativesEditor_DataBinding(object sender, EventArgs e)
	{
		GridViewDistrictRepresentativesEditor.DataSource = DistrictRepresentativesEditorDataSource;
	}

	protected void GridViewDistrictRepresentativesEditor_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		var table = DistrictRepresentativesEditorDataSource;
		var id = Convert.ToInt32(e.Keys["id"]);
		var representativeName = Convert.ToString(e.NewValues["representative_name"] ?? string.Empty).Trim();

		foreach (DataRow row in table.Rows)
		{
			if ((int)row["id"] == id)
			{
				row["representative_name"] = representativeName;
				break;
			}
		}

		DistrictRepresentativesEditorDataSource = table;

		e.Cancel = true;
		GridViewDistrictRepresentativesEditor.CancelEdit();
		BindDistrictRepresentativesEditorGrid();
		PopupDistrictRepresentativesEditor.ShowOnPageLoad = true;
	}

	protected void ButtonDistrictRepresentativesEditorOk_Click(object sender, EventArgs e)
	{
		var result = string.Join("; ", DistrictRepresentativesEditorDataSource.AsEnumerable()
			.Select(q => new
			{
				DistrictName = Convert.ToString(q["district_name"]).Trim(),
				RepresentativeName = Convert.ToString(q["representative_name"]).Trim()
			})
			.Where(q => !string.IsNullOrWhiteSpace(q.RepresentativeName))
			.Select(q => q.DistrictName + ": " + q.RepresentativeName));

		var memo = GridViewCommission.FindEditRowCellTemplateControl(GridViewCommission.Columns["colDistrictRepresentatives"] as GridViewDataColumn, "EditDistrictRepresentativesText") as ASPxMemo;
		if (memo != null)
		{
			memo.Text = result;
		}

		PopupDistrictRepresentativesEditor.ShowOnPageLoad = false;
	}

	protected void ButtonDistrictRepresentativesEditorCancel_Click(object sender, EventArgs e)
	{
		PopupDistrictRepresentativesEditor.ShowOnPageLoad = false;
	}

	protected void SqlDataSourceCommission_Inserting(object sender, SqlDataSourceCommandEventArgs e)
	{
		var user = Membership.GetUser();
		var username = (user == null ? String.Empty : (String)user.UserName);
		e.Command.Parameters["@modify_date"].Value = DateTime.Now;
		e.Command.Parameters["@modified_by"].Value = username;
	}

	protected void SqlDataSourceCommission_Updating(object sender, SqlDataSourceCommandEventArgs e)
	{
		var user = Membership.GetUser();
		var username = (user == null ? String.Empty : (String)user.UserName);
		e.Command.Parameters["@modify_date"].Value = DateTime.Now;
		e.Command.Parameters["@modified_by"].Value = username;
	}

	protected void SqlDataSourceCommission_Deleting(object sender, SqlDataSourceCommandEventArgs e)
	{
		var commissionId = Convert.ToInt32(e.Command.Parameters["@id"].Value);

		using (var connection = Utils.ConnectToDatabase())
		using (var command = new SqlCommand("SELECT COUNT(1) FROM [reports1nf_arenda_dogcontinue] WHERE [commission_id] = @commission_id", connection))
		{
			command.Parameters.AddWithValue("@commission_id", commissionId);
			var count = Convert.ToInt32(command.ExecuteScalar());
			if (count > 0)
			{
				throw new Exception("Комісію не можна видалити, оскільки вона використовується у реєстрі продовження договорів.");
			}
		}
	}

	protected void GridViewCommission_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
	{
		if (e.Exception != null && !string.IsNullOrWhiteSpace(e.Exception.Message))
		{
			e.ErrorText = e.Exception.Message;
		}
	}

	protected void ObjectDataSourcePhotoFiles_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
	{
		if (Request.Cookies["RecordID"] != null)
			e.InputParameters["RecordID"] = Request.Cookies["RecordID"].Value;

		//if (Request.QueryString["bid"] != null)
		//    e.InputParameters["arenda_id"] = int.Parse(Request.QueryString["bid"]);
	}

	protected void FreeSquareGridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
	{
		e.Items.ForEach(x => x.Visible = false);

		if (e.MenuType == GridViewContextMenuType.Rows)
		{
			e.Items.Add("Лист щодо продовження", "Report_2");
			//e.Items.Add("Звит 2", "Report_2");
		}
	}

}