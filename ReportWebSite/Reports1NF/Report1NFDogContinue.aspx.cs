using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
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

	protected void ASPxButtonCommissionProrydok_Click(object sender, EventArgs e)
	{
		var ids = GetFilteredFreeSquareIds();
		var builder = new CommissionProrydokBuilder
		{
			Page = Page,
			IDs = ids,
		};
		builder.Run();
	}

	protected void ASPxButtonCommissionProrydokText_Click(object sender, EventArgs e)
	{
		var ids = GetFilteredFreeSquareIds();
		var builder = new CommissionProrydokTextBuilder
		{
			Page = Page,
			IDs = ids,
		};
		builder.Run();
	}

	protected void ASPxButtonCommissionResultTable_Click(object sender, EventArgs e)
	{
		var ids = GetFilteredFreeSquareIds();
		var builder = new CommissionResultTableBuilder
		{
			Page = Page,
			IDs = ids,
		};
		builder.Run();
	}

	protected void ASPxButtonCommissionResultText_Click(object sender, EventArgs e)
	{
		var ids = GetFilteredFreeSquareIds();
		var builder = new CommissionResultTextBuilder
		{
			Page = Page,
			IDs = ids,
		};
		builder.Run();
	}

	private List<int> GetFilteredFreeSquareIds()
	{
		var result = new List<int>();

		var oldPagerMode = FreeSquareGridView.SettingsPager.Mode;
		var oldPageSize = FreeSquareGridView.SettingsPager.PageSize;
		try
		{
			FreeSquareGridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
			FreeSquareGridView.DataBind();

			for (var i = 0; i < FreeSquareGridView.VisibleRowCount; i++)
			{
				var value = FreeSquareGridView.GetRowValues(i, "id");
				if (value != null && value != DBNull.Value)
				{
					result.Add(Convert.ToInt32(value));
				}
			}
		}
		finally
		{
			FreeSquareGridView.SettingsPager.Mode = oldPagerMode;
			FreeSquareGridView.SettingsPager.PageSize = oldPageSize;
			FreeSquareGridView.DataBind();
		}

		return result.Distinct().ToList();
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

		var newCommissionId = GetNullableInt(e.Command.Parameters["@commission_id"].Value);
		var buildingType = Convert.ToString(e.Command.Parameters["@building_type"].Value ?? string.Empty).Trim();
		var rentalType = Convert.ToString(e.Command.Parameters["@rental_type"].Value ?? string.Empty).Trim();
		var rentalRatePercentValue = e.Command.Parameters["@rental_rate_percent"].Value;
		var oldCommissionId = GetCurrentCommissionId(free_square_id);

		if (!oldCommissionId.HasValue && newCommissionId.HasValue && string.IsNullOrWhiteSpace(buildingType))
		{
			var calculatedBuildingType = GetCalculatedBuildingType(free_square_id);
			e.Command.Parameters["@building_type"].Value = string.IsNullOrWhiteSpace(calculatedBuildingType)
				? (object)DBNull.Value
				: calculatedBuildingType;
		}

		if (!oldCommissionId.HasValue && newCommissionId.HasValue && string.IsNullOrWhiteSpace(rentalType))
		{
			var calculatedRentalType = GetCalculatedRentalType(free_square_id);
			e.Command.Parameters["@rental_type"].Value = string.IsNullOrWhiteSpace(calculatedRentalType)
				? (object)DBNull.Value
				: calculatedRentalType;
		}

		if (!oldCommissionId.HasValue && newCommissionId.HasValue && IsEmptyParameterValue(rentalRatePercentValue))
		{
			var calculatedRentalRatePercent = "" + GetCalculatedRentalRatePercent(free_square_id);
			e.Command.Parameters["@rental_rate_percent"].Value = string.IsNullOrWhiteSpace(calculatedRentalRatePercent)
				? (object)calculatedRentalRatePercent
				: DBNull.Value;
		}

		var parameters = (SqlParameterCollection)e.Command.Parameters;
		if (!parameters.Contains("@prozoro_number"))
		{
			parameters.AddWithValue("@prozoro_number", DBNull.Value);
		}

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

	private int? GetNullableInt(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return null;
		}

		var text = Convert.ToString(value).Trim();
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}

		int result;
		return Int32.TryParse(text, out result) ? (int?)result : null;
	}

	private int? GetCurrentCommissionId(int freeSquareId)
	{
		int? result = null;

		using (var connection = Utils.ConnectToDatabase())
		using (var command = new SqlCommand("SELECT [commission_id] FROM [reports1nf_arenda_dogcontinue] WHERE [id] = @id", connection))
		{
			command.Parameters.Add(new SqlParameter("@id", freeSquareId));

			using (var reader = command.ExecuteReader())
			{
				if (reader.Read())
				{
					result = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0);
				}

				reader.Close();
			}
		}

		return result;
	}


	private bool IsEmptyParameterValue(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return true;
		}

		var text = Convert.ToString(value);
		return String.IsNullOrWhiteSpace(text);
	}

	private string GetCalculatedBuildingType(int freeSquareId)
	{
		var result = new List<string>();

		using (var connection = Utils.ConnectToDatabase())
		using (var command = new SqlCommand(@"
SELECT distinct
	h.name
FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join reports1nf_arenda_notes u on (u.is_deleted IS NULL OR u.is_deleted = 0) AND u.report_id = bal.report_id AND u.arenda_id = bal.id
join reports1nf_balans w on w.report_id = u.report_id and w.id = u.ref_balans_id
join dict_1nf_object_type h on h.id = w.object_type_id
WHERE fs.id = @id
and h.name <> ''
order by 1", connection))
		{
			command.Parameters.Add(new SqlParameter("@id", freeSquareId));

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var name = reader.IsDBNull(0) ? string.Empty : reader.GetString(0).Trim();
					if (!string.IsNullOrWhiteSpace(name))
					{
						result.Add(name);
					}
				}

				reader.Close();
			}
		}

		return string.Join("; ", result.Distinct());
	}

	private string GetCalculatedRentalType(int freeSquareId)
	{
		var result = new List<string>();

		using (var connection = Utils.ConnectToDatabase())
		using (var command = new SqlCommand(@"
SELECT distinct
	h.name
FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join dict_arenda_payment_type h on h.id = bal.payment_type_id
WHERE fs.id = @id
and h.name <> ''
order by 1", connection))
		{
			command.Parameters.Add(new SqlParameter("@id", freeSquareId));

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var name = reader.IsDBNull(0) ? string.Empty : reader.GetString(0).Trim();
					if (!string.IsNullOrWhiteSpace(name))
					{
						result.Add(name);
					}
				}

				reader.Close();
			}
		}

		return string.Join("; ", result.Distinct());
	}

	private decimal? GetCalculatedRentalRatePercent(int freeSquareId)
	{
		decimal? result = null;

		using (var connection = Utils.ConnectToDatabase())
		using (var command = new SqlCommand(@"
SELECT
	max(u.cost_narah)
FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join reports1nf_arenda_notes u on (u.is_deleted IS NULL OR u.is_deleted = 0) AND u.report_id = bal.report_id AND u.arenda_id = bal.id
WHERE fs.id = @id", connection))
		{
			command.Parameters.Add(new SqlParameter("@id", freeSquareId));
			using (var reader = command.ExecuteReader())
			{
				if (reader.Read())
				{
					result = reader.IsDBNull(0) ? (decimal?)null : reader.GetDecimal(0);
				}

				reader.Close();
			}
		}

		return result;
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

	private const string VotingEditorSessionKey = "DogContinue_VotingEditorDataSource";

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


	private DataTable CreateVotingEditorTable()
	{
		var table = new DataTable();
		table.Columns.Add("id", typeof(int));
		table.Columns.Add("deputy_name", typeof(string));
		table.Columns.Add("vote_value", typeof(string));
		return table;
	}

	private DataTable VotingEditorDataSource
	{
		get
		{
			var table = Session[VotingEditorSessionKey] as DataTable;
			if (table == null)
			{
				table = CreateVotingEditorTable();
				Session[VotingEditorSessionKey] = table;
			}

			return table;
		}
		set
		{
			Session[VotingEditorSessionKey] = value;
		}
	}

	private List<string> GetCommissionDeputyNames(int? commissionId)
	{
		var result = new List<string>();
		if (!commissionId.HasValue || commissionId.Value <= 0)
		{
			return result;
		}

		using (var connection = Utils.ConnectToDatabase())
		using (var command = new SqlCommand("SELECT [deputies_list] FROM [dogcontinue_commission] WHERE [id] = @id", connection))
		{
			command.Parameters.AddWithValue("@id", commissionId.Value);
			var obj = command.ExecuteScalar();
			var deputiesList = obj == DBNull.Value || obj == null ? string.Empty : Convert.ToString(obj);
			if (!string.IsNullOrWhiteSpace(deputiesList))
			{
				result = deputiesList
					.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(q => q.Trim())
					.Where(q => !string.IsNullOrWhiteSpace(q))
					.ToList();
			}
		}

		return result;
	}

	private DataTable CreateVotingEditorTableFromData(int? commissionId, string golosovanie)
	{
		var table = CreateVotingEditorTable();
		var deputies = GetCommissionDeputyNames(commissionId);
		var voteMap = ParseGolosovanieText(golosovanie);

		for (var i = 0; i < deputies.Count; i++)
		{
			var deputyName = deputies[i];
			var voteValue = string.Empty;
			if (voteMap != null)
			{
				string parsedVote;
				if (voteMap.TryGetValue(deputyName, out parsedVote))
				{
					voteValue = parsedVote;
				}
			}

			table.Rows.Add(i + 1, deputyName, voteValue);
		}

		return table;
	}

	private Dictionary<string, string> ParseGolosovanieText(string golosovanie)
	{
		if (string.IsNullOrWhiteSpace(golosovanie))
		{
			return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

		var regex = new Regex(
			"^\\s*\"за\"\\s*\\((?<zaCount>\\d+)\\)(?:\\s*-\\s*(?<zaNames>.*?))?,\\s*\"проти\"\\s*\\((?<protyCount>\\d+)\\)(?:\\s*-\\s*(?<protyNames>.*?))?,\\s*\"утримались\"\\s*\\((?<utrymCount>\\d+)\\)(?:\\s*-\\s*(?<utrymNames>.*?))?,\\s*\"не голосували\"\\s*\\((?<noVoteCount>\\d+)\\)(?:\\s*-\\s*(?<noVoteNames>.*?))?,?\\s*$",
			RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant);

		var match = regex.Match(golosovanie.Trim());
		if (!match.Success)
		{
			return null;
		}

		var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		if (!TryAddVotesFromGroup(match, "zaCount", "zaNames", "За", result))
		{
			return null;
		}

		if (!TryAddVotesFromGroup(match, "protyCount", "protyNames", "Проти", result))
		{
			return null;
		}

		if (!TryAddVotesFromGroup(match, "utrymCount", "utrymNames", "Утримався", result))
		{
			return null;
		}

		if (!TryAddVotesFromGroup(match, "noVoteCount", "noVoteNames", "Не голосував", result))
		{
			return null;
		}

		return result;
	}

	private bool TryAddVotesFromGroup(Match match, string countGroupName, string namesGroupName, string voteValue, Dictionary<string, string> result)
	{
		var count = Int32.Parse(match.Groups[countGroupName].Value);
		var namesText = match.Groups[namesGroupName].Success ? match.Groups[namesGroupName].Value.Trim() : string.Empty;
		var names = new List<string>();

		if (!string.IsNullOrWhiteSpace(namesText))
		{
			names = namesText
				.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(q => q.Trim())
				.Where(q => !string.IsNullOrWhiteSpace(q))
				.ToList();
		}

		if (names.Count != count)
		{
			return false;
		}

		foreach (var name in names)
		{
			if (result.ContainsKey(name))
			{
				return false;
			}

			result.Add(name, voteValue);
		}

		return true;
	}

	private void BindVotingEditorGrid()
	{
		GridViewVotingEditor.DataSource = VotingEditorDataSource;
		GridViewVotingEditor.DataBind();
	}

	protected void ButtonEditGolosovanie_Click(object sender, EventArgs e)
	{
		var memo = FreeSquareGridView.FindEditRowCellTemplateControl(FreeSquareGridView.Columns["colGolosovanie"] as GridViewDataColumn, "EditGolosovanieText") as ASPxMemo;
		var editCommissionId = FreeSquareGridView.FindEditRowCellTemplateControl(FreeSquareGridView.Columns["colCommissionId"] as GridViewDataColumn, "EditCommissionId") as ASPxComboBox;
		var golosovanie = memo == null ? string.Empty : memo.Text;

		int? commissionId = null;
		if (editCommissionId != null && editCommissionId.Value != null)
		{
			commissionId = Convert.ToInt32(editCommissionId.Value);
		}

		VotingEditorDataSource = CreateVotingEditorTableFromData(commissionId, golosovanie);
		BindVotingEditorGrid();
		PopupVotingEditor.ShowOnPageLoad = true;
	}

	protected void GridViewVotingEditor_DataBinding(object sender, EventArgs e)
	{
		GridViewVotingEditor.DataSource = VotingEditorDataSource;
	}

	protected void GridViewVotingEditor_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		var table = VotingEditorDataSource;
		var id = Convert.ToInt32(e.Keys["id"]);
		var voteValue = Convert.ToString(e.NewValues["vote_value"] ?? string.Empty).Trim();
		if (voteValue != "За" && voteValue != "Проти" && voteValue != "Утримався" && voteValue != "Не голосував")
		{
			voteValue = string.Empty;
		}

		foreach (DataRow row in table.Rows)
		{
			if ((int)row["id"] == id)
			{
				row["vote_value"] = voteValue;
				break;
			}
		}

		VotingEditorDataSource = table;

		e.Cancel = true;
		GridViewVotingEditor.CancelEdit();
		BindVotingEditorGrid();
		PopupVotingEditor.ShowOnPageLoad = true;
	}

	private string BuildGolosovanieText(DataTable table)
	{
		var groups = new[]
		{
			new { Value = "За", Label = "за" },
			new { Value = "Проти", Label = "проти" },
			new { Value = "Утримався", Label = "утримались" },
			new { Value = "Не голосував", Label = "не голосували" }
		};

		var hasAnyVote = table.AsEnumerable().Any(q => !string.IsNullOrWhiteSpace(Convert.ToString(q["vote_value"])));
		if (!hasAnyVote)
		{
			return string.Empty;
		}

		var parts = new List<string>();
		foreach (var group in groups)
		{
			var names = table.AsEnumerable()
				.Where(q => Convert.ToString(q["vote_value"]) == group.Value)
				.Select(q => Convert.ToString(q["deputy_name"]).Trim())
				.Where(q => !string.IsNullOrWhiteSpace(q))
				.ToList();

			var part = string.Format("\"{0}\" ({1})", group.Label, names.Count);
			if (names.Count > 0)
			{
				part += " - " + string.Join(", ", names);
			}

			parts.Add(part);
		}

		return string.Join(", ", parts) + ",";
	}

	protected void ButtonVotingEditorOk_Click(object sender, EventArgs e)
	{
		var result = BuildGolosovanieText(VotingEditorDataSource);

		var memo = FreeSquareGridView.FindEditRowCellTemplateControl(FreeSquareGridView.Columns["colGolosovanie"] as GridViewDataColumn, "EditGolosovanieText") as ASPxMemo;
		if (memo != null)
		{
			memo.Text = result;
		}

		PopupVotingEditor.ShowOnPageLoad = false;
	}

	protected void ButtonVotingEditorCancel_Click(object sender, EventArgs e)
	{
		PopupVotingEditor.ShowOnPageLoad = false;
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

	protected void EditCommissionId_Init(object sender, EventArgs e)
	{
		var combo = sender as ASPxComboBox;
		if (combo == null)
		{
			return;
		}

		combo.DataBound += (s, ea) =>
		{
			var cb = s as ASPxComboBox;
			if (cb == null)
			{
				return;
			}

			if (cb.Items.FindByValue(DBNull.Value) == null && cb.Items.FindByValue(null) == null)
			{
				cb.Items.Insert(0, new ListEditItem(string.Empty, null));
			}
		};
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

public class CommissionProrydokBuilder
{
	public Page Page;
	public List<int> IDs;

	public void Run()
	{
		var ids = (IDs ?? new List<int>()).Where(q => q > 0).Distinct().ToList();
		if (ids.Count == 0)
		{
			return;
		}

		DataTable data;
		DateTime? singleCommissionDate;

		using (var connection = Utils.ConnectToDatabase())
		{
			data = GetData(connection, ids);
			singleCommissionDate = GetSingleCommissionDate(connection, ids);
		}

		using (var excelEngine = new ExcelEngine())
		{
			var application = excelEngine.Excel;
			application.DefaultVersion = ExcelVersion.Excel2016;
			var workbook = application.Workbooks.Create(1);
			var worksheet = workbook.Worksheets[0];
			worksheet.Name = "Порядок денний";

			BuildWorksheet(worksheet, data, singleCommissionDate);

			using (var stream = new MemoryStream())
			{
				workbook.SaveAs(stream);
				workbook.Close();
				stream.Position = 0;

				var suffix = singleCommissionDate.HasValue
					? singleCommissionDate.Value.ToString("dd.MM.yyyy")
					: DateTime.Now.ToString("dd.MM.yyyy");
				var outfile = "Порядок денний " + suffix + ".xlsx";

				Page.Response.Clear();
				Page.Response.ClearHeaders();
				Page.Response.ClearContent();
				Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				Page.Response.AddHeader("content-disposition", "attachment; filename=" + outfile + "; size=" + stream.Length.ToString(CultureInfo.InvariantCulture));

				stream.CopyTo(Page.Response.OutputStream);
				Page.Response.End();
			}
		}
	}

	private DataTable GetData(SqlConnection connection, List<int> ids)
	{
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = GetMainSql(ids);
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		return dataTable;
	}

	private DateTime? GetSingleCommissionDate(SqlConnection connection, List<int> ids)
	{
		var sql = @"
SELECT
	min(dc.commission_date) as min_commission_date,
	max(dc.commission_date) as max_commission_date
FROM dbo.reports1nf_arenda_dogcontinue fs
left join dbo.dogcontinue_commission dc on dc.id = fs.commission_id
WHERE fs.id in (" + string.Join(",", ids) + @")";

		using (var command = new SqlCommand(sql, connection))
		using (var reader = command.ExecuteReader())
		{
			if (reader.Read())
			{
				var minDate = reader.IsDBNull(0) ? (DateTime?)null : reader.GetDateTime(0);
				var maxDate = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1);
				if (minDate.HasValue && maxDate.HasValue && minDate.Value.Date == maxDate.Value.Date)
				{
					return minDate.Value.Date;
				}
			}
		}

		return null;
	}

	private void BuildWorksheet(IWorksheet worksheet, DataTable data, DateTime? commissionDate)
	{
		var headers = new[]
		{
			"№",
			"Тип питання",
			"Орендодавець",
			"Балансоутримувач",
			"Орендар",
			"Адреса",
			"Тип будинку",
			"Характеристика об'єкта оренди",
			"Категорія",
			"Цільове призначення",
			"Орендована площа, кв.м.",
			"Орендна ставка",
			"Місячна орендна плата, грн",
			"Тип оренди",
			"Вартість об'єкту, грн",
			"Термін оренди",
			"Примітка користувача",
			"Додаткова інформація"
		};

		double[] widths = { 6, 14, 18, 28, 24, 20, 14, 18, 12, 20, 12, 12, 14, 12, 14, 14, 18, 24 };
		for (var i = 0; i < widths.Length; i++)
		{
			worksheet.SetColumnWidth(i + 1, widths[i]);
		}

		var lastColLetter = GetExcelColumnName(headers.Length);
		var title = "Порядок денний" + (commissionDate.HasValue ? " " + commissionDate.Value.ToString("dd.MM.yyyy") : string.Empty);

		worksheet.Range["A1:" + lastColLetter + "1"].Merge();
		worksheet.Range["A1"].Text = title;
		worksheet.Range["A1"].CellStyle.Font.Bold = true;
		worksheet.Range["A1"].CellStyle.Font.Size = 14;
		worksheet.Range["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
		worksheet.Range["A1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
		worksheet.Range["A1"].CellStyle.WrapText = true;

		for (var i = 0; i < headers.Length; i++)
		{
			worksheet[2, i + 1].Text = headers[i];
			worksheet[3, i + 1].Text = (i + 1).ToString(CultureInfo.InvariantCulture);
		}

		var headerRange = worksheet.Range["A2:" + lastColLetter + "3"];
		headerRange.CellStyle.Font.Bold = true;
		headerRange.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
		headerRange.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
		headerRange.CellStyle.WrapText = true;

		worksheet.SetRowHeight(1, 24);
		worksheet.SetRowHeight(2, 42);
		worksheet.SetRowHeight(3, 20);

		var rowIndex = 4;
		foreach (DataRow row in data.Rows)
		{
			worksheet[rowIndex, 1].Text = GetCellText(row, "№");
			worksheet[rowIndex, 2].Text = GetCellText(row, "Тип питання");
			worksheet[rowIndex, 3].Text = GetCellText(row, "Орендодавець");
			worksheet[rowIndex, 4].Text = GetCellText(row, "Балансоутримувач");
			worksheet[rowIndex, 5].Text = GetCellText(row, "Орендар");
			worksheet[rowIndex, 6].Text = GetCellText(row, "Адреса");
			worksheet[rowIndex, 7].Text = GetCellText(row, "Тип будинку");
			worksheet[rowIndex, 8].Text = GetCellText(row, "Характеристика об'єкта оренди");
			worksheet[rowIndex, 9].Text = GetCellText(row, "Категорія");
			worksheet[rowIndex, 10].Text = GetCellText(row, "Цільове призначення");
			worksheet[rowIndex, 11].Text = GetCellText(row, "Орендована площа, кв.м.");
			worksheet[rowIndex, 12].Text = GetCellText(row, "Орендна ставка");
			worksheet[rowIndex, 13].Text = GetCellText(row, "Місячна орендна плата, грн");
			worksheet[rowIndex, 14].Text = GetCellText(row, "Тип оренди");
			worksheet[rowIndex, 15].Text = GetCellText(row, "Вартість об'єкту, грн");
			worksheet[rowIndex, 16].Text = GetCellText(row, "Термін оренди");
			worksheet[rowIndex, 17].Text = GetCellText(row, "Примітка користувача");
			worksheet[rowIndex, 18].Text = GetCellText(row, "Додаткова інформація");
			worksheet.SetRowHeight(rowIndex, 42);
			rowIndex++;
		}

		var lastRow = Math.Max(rowIndex - 1, 3);
		for (var r = 1; r <= lastRow; r++)
		{
			for (var c = 1; c <= headers.Length; c++)
			{
				var cell = worksheet[r, c];
				cell.CellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
				cell.CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
				cell.CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
				cell.CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
				cell.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
				cell.CellStyle.WrapText = true;
				if (r >= 4)
				{
					cell.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
				}
			}
		}
	}

	private string GetExcelColumnName(int columnNumber)
	{
		var dividend = columnNumber;
		var columnName = string.Empty;
		while (dividend > 0)
		{
			var modulo = (dividend - 1) % 26;
			columnName = Convert.ToChar(65 + modulo) + columnName;
			dividend = (dividend - modulo) / 26;
		}

		return columnName;
	}

	private string GetCellText(DataRow row, string columnName)
	{
		if (!row.Table.Columns.Contains(columnName))
			return string.Empty;

		var value = row[columnName];
		if (value == null || value is DBNull)
			return string.Empty;

		var dataType = row.Table.Columns[columnName].DataType;
		if (dataType == typeof(string))
			return value.ToString();
		if (dataType == typeof(DateTime))
			return ((DateTime)value).ToString("dd.MM.yyyy");
		if (dataType == typeof(decimal))
			return ((decimal)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(double))
			return ((double)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(float))
			return ((float)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(int))
			return ((int)value).ToString(CultureInfo.InvariantCulture);
		if (dataType == typeof(long))
			return ((long)value).ToString(CultureInfo.InvariantCulture);

		return value.ToString();
	}

	private string GetMainSql(List<int> ids)
	{
		return @"
SELECT
	row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr) as ""№"",
	N'продовження' as ""Тип питання"",
	N'Департамент комунальної власності м. Києва' as ""Орендодавець"",
	org.short_name as ""Балансоутримувач"",
	org_renter.full_name as ""Орендар"",
	LTRIM(RTRIM(b.street_full_name)) + N' ' + LTRIM(RTRIM(b.addr_nomer)) as ""Адреса"",
	fs.building_type as ""Тип будинку"",
	fs.floor as ""Характеристика об'єкта оренди"",
	fs.category as ""Категорія"",
	(select Q.name from dict_may_pravo_prodov Q where Q.id = fs.may_pravo_prodov) as ""Цільове призначення"",
	fs.total_free_sqr as ""Орендована площа, кв.м."",
	fs.rental_rate_percent as ""Орендна ставка"",
	fs.orend_plat_last_month as ""Місячна орендна плата, грн"",
	fs.rental_type as ""Тип оренди"",
	fs.zal_balans_vartist as ""Вартість об'єкту, грн"",
	fs.rental_term as ""Термін оренди"",
	fs.commission_note as ""Примітка користувача"",
	fs.additional_info as ""Додаткова інформація""
FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.org_balans_id
left join organizations org_renter on org_renter.id = bal.org_renter_id
WHERE fs.id in (" + string.Join(",", ids) + @")
ORDER BY 1";
	}
}



public class CommissionResultTableBuilder
{
	public Page Page;
	public List<int> IDs;

	private class CommissionHeaderData
	{
		public string CommissionNum;
		public DateTime? CommissionDate;
	}

	public void Run()
	{
		var ids = (IDs ?? new List<int>()).Where(q => q > 0).Distinct().ToList();
		if (ids.Count == 0)
		{
			return;
		}

		DataTable data;
		CommissionHeaderData headerData;
		using (var connection = Utils.ConnectToDatabase())
		{
			data = GetData(connection, ids);
			headerData = GetSingleCommissionHeaderData(connection, ids);
		}

		using (var excelEngine = new ExcelEngine())
		{
			var application = excelEngine.Excel;
			application.DefaultVersion = ExcelVersion.Excel2016;
			var workbook = application.Workbooks.Create(1);
			var worksheet = workbook.Worksheets[0];
			worksheet.Name = "Результат";

			BuildWorksheet(worksheet, data, headerData);

			using (var stream = new MemoryStream())
			{
				workbook.SaveAs(stream);
				workbook.Close();
				stream.Position = 0;

				var suffix = headerData != null && headerData.CommissionDate.HasValue
					? headerData.CommissionDate.Value.ToString("dd.MM.yyyy")
					: DateTime.Now.ToString("dd.MM.yyyy");
				var outfile = "Результат " + suffix + ".xlsx";

				Page.Response.Clear();
				Page.Response.ClearHeaders();
				Page.Response.ClearContent();
				Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				Page.Response.AddHeader("content-disposition", "attachment; filename=" + outfile + "; size=" + stream.Length.ToString(CultureInfo.InvariantCulture));
				stream.CopyTo(Page.Response.OutputStream);
				Page.Response.End();
			}
		}
	}

	private DataTable GetData(SqlConnection connection, List<int> ids)
	{
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = GetMainSql(ids);
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		return dataTable;
	}

	private CommissionHeaderData GetSingleCommissionHeaderData(SqlConnection connection, List<int> ids)
	{
		var sql = @"
SELECT
	min(dc.commission_num) as min_commission_num,
	max(dc.commission_num) as max_commission_num,
	min(dc.commission_date) as min_commission_date,
	max(dc.commission_date) as max_commission_date
FROM dbo.reports1nf_arenda_dogcontinue fs
left join dbo.dogcontinue_commission dc on dc.id = fs.commission_id
WHERE fs.id in (" + string.Join(",", ids) + @")";

		using (var command = new SqlCommand(sql, connection))
		using (var reader = command.ExecuteReader())
		{
			if (reader.Read())
			{
				var minNum = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
				var maxNum = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
				var minDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2);
				var maxDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3);

				var result = new CommissionHeaderData();
				if (!string.IsNullOrWhiteSpace(minNum) && minNum == maxNum)
				{
					result.CommissionNum = minNum;
				}

				if (minDate.HasValue && maxDate.HasValue && minDate.Value.Date == maxDate.Value.Date)
				{
					result.CommissionDate = minDate.Value.Date;
				}

				return result;
			}
		}

		return new CommissionHeaderData();
	}

	private void BuildWorksheet(IWorksheet worksheet, DataTable data, CommissionHeaderData headerData)
	{
		var headers = new[]
		{
			"№ питання у протоколі",
			"Дата оцінки",
			"Дата договору оренди, який продовжується, змінюється",
			"Тип питання",
			"Балансоутримувач",
			"Орендар, код",
			"Адреса",
			"Тип будинку",
			"Характеристика об'єкта оренди, поверх",
			"Цільове призначення",
			"Орендована площа кв.м",
			"Поточна ставка, %**",
			"Місячна орендна плата грн",
			"Тип оренди *",
			"Вартість об'єкта",
			"Строк або термін оренди",
			"Додаткова інформація"
		};

		double[] widths = { 7, 11, 13, 12, 27, 24, 20, 12, 14, 24, 12, 10, 13, 12, 13, 13, 16 };
		for (var i = 0; i < widths.Length; i++)
		{
			worksheet.SetColumnWidth(i + 1, widths[i]);
		}

		var lastColLetter = GetExcelColumnName(headers.Length);

		worksheet.Range["A1:" + lastColLetter + "1"].Merge();
		worksheet.Range["A2:" + lastColLetter + "2"].Merge();
		worksheet.Range["A3:" + lastColLetter + "3"].Merge();
		worksheet.Range["A4:" + lastColLetter + "4"].Merge();

		worksheet.Range["A1"].Text = !string.IsNullOrWhiteSpace(headerData.CommissionNum)
			? "Додаток до протоколу № " + headerData.CommissionNum
			: "Додаток до протоколу";
		worksheet.Range["A2"].Text = "засідання постійної комісії Київської міської ради";
		worksheet.Range["A3"].Text = "з питань власності та регуляторної політики" + (headerData.CommissionDate.HasValue ? " від " + headerData.CommissionDate.Value.ToString("dd.MM.yyyy") : string.Empty);
		worksheet.Range["A4"].Text = "Перелік погоджених постійною комісією Київської міської ради з питань власності питань оренди щодо нежитлових приміщень комунальної власності м.Києва, орендодавцем яких виступає Департамент комунальної власності м. Києва";

		for (var r = 1; r <= 3; r++)
		{
			worksheet.Range[r, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
			worksheet.Range[r, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
			worksheet.Range[r, 1].CellStyle.Font.Size = 9;
		}

		worksheet.Range["A4"].CellStyle.Font.Bold = true;
		worksheet.Range["A4"].CellStyle.Font.Size = 12;
		worksheet.Range["A4"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
		worksheet.Range["A4"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
		worksheet.Range["A4"].CellStyle.WrapText = true;

		for (var i = 0; i < headers.Length; i++)
		{
			worksheet[5, i + 1].Text = headers[i];
		}

		var headerRange = worksheet.Range["A5:" + lastColLetter + "5"];
		headerRange.CellStyle.Font.Bold = true;
		headerRange.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
		headerRange.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
		headerRange.CellStyle.WrapText = true;

		worksheet.SetRowHeight(1, 16);
		worksheet.SetRowHeight(2, 16);
		worksheet.SetRowHeight(3, 16);
		worksheet.SetRowHeight(4, 44);
		worksheet.SetRowHeight(5, 62);

		var rowIndex = 6;
		foreach (DataRow row in data.Rows)
		{
			worksheet[rowIndex, 1].Text = GetCellText(row, "№ питання у протоколі");
			worksheet[rowIndex, 2].Text = GetCellText(row, "Дата оцінки");
			worksheet[rowIndex, 3].Text = GetCellText(row, "Дата договору оренди, який продовжується, змінюється");
			worksheet[rowIndex, 4].Text = GetCellText(row, "Тип питання");
			worksheet[rowIndex, 5].Text = GetCellText(row, "Балансоутримувач");
			worksheet[rowIndex, 6].Text = GetCellText(row, "Орендар, код");
			worksheet[rowIndex, 7].Text = GetCellText(row, "Адреса");
			worksheet[rowIndex, 8].Text = GetCellText(row, "Тип будинку");
			worksheet[rowIndex, 9].Text = GetCellText(row, "Характеристика об'єкта оренди, поверх");
			worksheet[rowIndex, 10].Text = GetCellText(row, "Цільове призначення");
			worksheet[rowIndex, 11].Text = GetCellText(row, "Орендована площа кв.м");
			worksheet[rowIndex, 12].Text = GetCellText(row, "Поточна ставка, %**");
			worksheet[rowIndex, 13].Text = GetCellText(row, "Місячна орендна плата грн");
			worksheet[rowIndex, 14].Text = GetCellText(row, "Тип оренди *");
			worksheet[rowIndex, 15].Text = GetCellText(row, "Вартість об'єкта");
			worksheet[rowIndex, 16].Text = GetCellText(row, "Строк або термін оренди");
			worksheet[rowIndex, 17].Text = GetCellText(row, "Додаткова інформація");
			worksheet.SetRowHeight(rowIndex, 54);
			rowIndex++;
		}

		var lastRow = Math.Max(rowIndex - 1, 5);
		for (var r = 4; r <= lastRow; r++)
		{
			for (var c = 1; c <= headers.Length; c++)
			{
				var cell = worksheet[r, c];
				cell.CellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
				cell.CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
				cell.CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
				cell.CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
				cell.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
				cell.CellStyle.WrapText = true;
				cell.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
			}
		}
	}

	private string GetExcelColumnName(int columnNumber)
	{
		var dividend = columnNumber;
		var columnName = string.Empty;
		while (dividend > 0)
		{
			var modulo = (dividend - 1) % 26;
			columnName = Convert.ToChar(65 + modulo) + columnName;
			dividend = (dividend - modulo) / 26;
		}

		return columnName;
	}

	private string GetCellText(DataRow row, string columnName)
	{
		if (!row.Table.Columns.Contains(columnName))
			return string.Empty;

		var value = row[columnName];
		if (value == null || value is DBNull)
			return string.Empty;

		var dataType = row.Table.Columns[columnName].DataType;
		if (dataType == typeof(string))
			return value.ToString();
		if (dataType == typeof(DateTime))
			return ((DateTime)value).ToString("dd.MM.yyyy");
		if (dataType == typeof(decimal))
			return ((decimal)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(double))
			return ((double)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(float))
			return ((float)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(int))
			return ((int)value).ToString(CultureInfo.InvariantCulture);
		if (dataType == typeof(long))
			return ((long)value).ToString(CultureInfo.InvariantCulture);

		return value.ToString();
	}

	private string GetMainSql(List<int> ids)
	{
		return @"
SELECT
	isnull(fs.protocol_question_num, row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr)) as ""№ питання у протоколі"",
	fs.zalbalansvartist_date as ""Дата оцінки"",
	bal.agreement_date as ""Дата договору оренди, який продовжується, змінюється"",
	N'Продовження' as ""Тип питання"",
	org.full_name as ""Балансоутримувач"",
	LTRIM(RTRIM(isnull(org_renter.full_name, N''))) + case when isnull(org_renter.zkpo_code, N'') <> N'' then N', код ' + LTRIM(RTRIM(org_renter.zkpo_code)) else N'' end as ""Орендар, код"",
	LTRIM(RTRIM(b.street_full_name)) + N' ' + LTRIM(RTRIM(b.addr_nomer)) as ""Адреса"",
	fs.building_type as ""Тип будинку"",
	fs.floor as ""Характеристика об'єкта оренди, поверх"",
	(select Q.name from dict_may_pravo_prodov Q where Q.id = fs.may_pravo_prodov) as ""Цільове призначення"",
	fs.total_free_sqr as ""Орендована площа кв.м"",
	fs.rental_rate_percent as ""Поточна ставка, %**"",
	fs.orend_plat_last_month as ""Місячна орендна плата грн"",
	fs.rental_type as ""Тип оренди *"",
	fs.zal_balans_vartist as ""Вартість об'єкта"",
	fs.rental_term as ""Строк або термін оренди"",
	fs.additional_info as ""Додаткова інформація""
FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.org_balans_id
left join organizations org_renter on org_renter.id = bal.org_renter_id
WHERE fs.id in (" + string.Join(",", ids) + @")
ORDER BY 1";
	}
}

public class CommissionProrydokTextBuilder
{
	public Page Page;
	public List<int> IDs;

	public void Run()
	{
		var ids = (IDs ?? new List<int>()).Where(q => q > 0).Distinct().ToList();
		if (ids.Count == 0)
		{
			return;
		}

		DataTable data;
		using (var connection = Utils.ConnectToDatabase())
		{
			data = GetData(connection, ids);
		}

		using (var document = new WordDocument())
		{
			var section = document.AddSection();
			ConfigureSection(section);
			BuildDocument(section, data);

			using (var stream = new MemoryStream())
			{
				document.Save(stream, FormatType.Docx);
				document.Close();
				stream.Position = 0;

				var outfile = "Порядок денний (текст) " + DateTime.Now.ToString("dd.MM.yyyy") + ".docx";
				Page.Response.Clear();
				Page.Response.ClearHeaders();
				Page.Response.ClearContent();
				Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
				Page.Response.AddHeader("content-disposition", "attachment; filename=" + outfile + "; size=" + stream.Length.ToString(CultureInfo.InvariantCulture));
				stream.CopyTo(Page.Response.OutputStream);
				Page.Response.End();
			}
		}
	}

	private void ConfigureSection(IWSection section)
	{
		section.PageSetup.Margins.Top = 36f;
		section.PageSetup.Margins.Bottom = 36f;
		section.PageSetup.Margins.Left = 56f;
		section.PageSetup.Margins.Right = 56f;
	}

	private DataTable GetData(SqlConnection connection, List<int> ids)
	{
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = GetMainSql(ids);
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		return dataTable;
	}

	private void BuildDocument(IWSection section, DataTable data)
	{
		AddCenteredParagraph(section, "Порядок денний", 18f, true, false, 0f, 0f);
		AddCenteredParagraph(section, "II частина", 18f, true, false, 0f, 0f);
		AddCenteredParagraph(section, "питання оренди", 16f, true, true, 0f, 18f);

		foreach (DataRow row in data.Rows)
		{
			var number = GetCellText(row, "№");
			var convertedOrgGiver = GetCellText(row, "converted_org_giver");
			var renterName = GetCellText(row, "Найменування орендаря");
			var streetName = GetCellText(row, "Назва Вулиці");
			var houseNumber = GetCellText(row, "Номер Будинку");
			var speakerName = GetCellText(row, "Доповідач");

			var objectText = JoinParts(", ", renterName, streetName, houseNumber);
			var mainText = number + ". Про розгляд звернення " + convertedOrgGiver + " щодо питання \"Продовження\" - " + objectText;

			AddItemParagraph(section, mainText);
			AddSpeakerParagraph(section, speakerName);
			AddSpacerParagraph(section, 8f);
		}
	}

	private void AddCenteredParagraph(IWSection section, string text, float fontSize, bool bold, bool italic, float beforeSpacing, float afterSpacing)
	{
		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
		paragraph.ParagraphFormat.BeforeSpacing = beforeSpacing;
		paragraph.ParagraphFormat.AfterSpacing = afterSpacing;
		var range = paragraph.AppendText(text);
		range.CharacterFormat.FontName = "Times New Roman";
		range.CharacterFormat.FontSize = fontSize;
		range.CharacterFormat.Bold = bold;
		range.CharacterFormat.Italic = italic;
	}

	private void AddItemParagraph(IWSection section, string text)
	{
		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
		paragraph.ParagraphFormat.BeforeSpacing = 0f;
		paragraph.ParagraphFormat.AfterSpacing = 6f;
		var range = paragraph.AppendText(text);
		range.CharacterFormat.FontName = "Times New Roman";
		range.CharacterFormat.FontSize = 16f;
		range.CharacterFormat.Bold = true;
	}

	private void AddSpeakerParagraph(IWSection section, string speakerName)
	{
		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
		paragraph.ParagraphFormat.BeforeSpacing = 0f;
		paragraph.ParagraphFormat.AfterSpacing = 0f;
		var range = paragraph.AppendText("Доповідач: " + speakerName);
		range.CharacterFormat.FontName = "Times New Roman";
		range.CharacterFormat.FontSize = 16f;
		range.CharacterFormat.Italic = true;
	}

	private void AddSpacerParagraph(IWSection section, float afterSpacing)
	{
		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.AfterSpacing = afterSpacing;
		var range = paragraph.AppendText(" ");
		range.CharacterFormat.FontName = "Times New Roman";
		range.CharacterFormat.FontSize = 1f;
	}

	private string JoinParts(string separator, params string[] parts)
	{
		return string.Join(separator, parts.Where(q => !string.IsNullOrWhiteSpace(q)).Select(q => q.Trim()));
	}

	private string GetCellText(DataRow row, string columnName)
	{
		if (!row.Table.Columns.Contains(columnName))
			return string.Empty;

		var value = row[columnName];
		if (value == null || value is DBNull)
			return string.Empty;

		var dataType = row.Table.Columns[columnName].DataType;
		if (dataType == typeof(string))
			return value.ToString();
		if (dataType == typeof(DateTime))
			return ((DateTime)value).ToString("dd.MM.yyyy");
		if (dataType == typeof(decimal))
			return ((decimal)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(double))
			return ((double)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(float))
			return ((float)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(int))
			return ((int)value).ToString(CultureInfo.InvariantCulture);
		if (dataType == typeof(long))
			return ((long)value).ToString(CultureInfo.InvariantCulture);

		return value.ToString();
	}

	private string GetMainSql(List<int> ids)
	{
		return @"
SELECT
	row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr) as ""№"",
	isnull(conv.target_text, org_giver.short_name) as ""converted_org_giver"",
	org_renter.full_name as ""Найменування орендаря"",
	b.street_full_name as ""Назва Вулиці"",
	b.addr_nomer as ""Номер Будинку"",
	fs.speaker_name as ""Доповідач""
FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.org_balans_id
left join organizations org_renter on org_renter.id = bal.org_renter_id
left join organizations org_giver ON org_giver.id = bal.org_giver_id and (org_giver.is_deleted is null or org_giver.is_deleted = 0)
left join dbo.texts_org_giver_convert conv on conv.source_text = org_giver.short_name
WHERE fs.id in (" + string.Join(",", ids) + @")
ORDER BY 1";
	}
}

public class CommissionResultTextBuilder
{
	public Page Page;
	public List<int> IDs;

	public void Run()
	{
		var ids = (IDs ?? new List<int>()).Where(q => q > 0).Distinct().ToList();
		if (ids.Count == 0)
		{
			return;
		}

		DataTable data;
		using (var connection = Utils.ConnectToDatabase())
		{
			data = GetData(connection, ids);
		}

		using (var document = new WordDocument())
		{
			var section = document.AddSection();
			ConfigureSection(section);
			BuildDocument(section, data);

			using (var stream = new MemoryStream())
			{
				document.Save(stream, FormatType.Docx);
				document.Close();
				stream.Position = 0;

				var outfile = "Результат (текст) " + DateTime.Now.ToString("dd.MM.yyyy") + ".docx";
				Page.Response.Clear();
				Page.Response.ClearHeaders();
				Page.Response.ClearContent();
				Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
				Page.Response.AddHeader("content-disposition", "attachment; filename=" + outfile + "; size=" + stream.Length.ToString(CultureInfo.InvariantCulture));
				stream.CopyTo(Page.Response.OutputStream);
				Page.Response.End();
			}
		}
	}

	private void ConfigureSection(IWSection section)
	{
		section.PageSetup.Margins.Top = 36f;
		section.PageSetup.Margins.Bottom = 36f;
		section.PageSetup.Margins.Left = 56f;
		section.PageSetup.Margins.Right = 56f;
	}

	private DataTable GetData(SqlConnection connection, List<int> ids)
	{
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = GetMainSql(ids);
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		return dataTable;
	}

	private void BuildDocument(IWSection section, DataTable data)
	{
		AddCenteredParagraph(section, "Розгляд питань оренди", 16f, true, false, 0f, 18f);

		foreach (DataRow row in data.Rows)
		{
			var number = GetCellText(row, "№");
			var convertedOrgGiver = GetCellText(row, "converted_org_giver");
			var renterName = GetCellText(row, "Найменування орендаря");
			var streetName = GetCellText(row, "Назва Вулиці");
			var houseNumber = GetCellText(row, "Номер Будинку");
			var incomingDocNum = GetCellText(row, "Вхідний номер");
			var incomingDocDate = GetCellText(row, "Дата вхідного документа");
			var outgoingDocNum = GetCellText(row, "Вихідний номер");
			var outgoingDocDate = GetCellText(row, "Дата вихідного документа");
			var speakerName = GetCellText(row, "Доповідач");
			var slukhaliText = GetCellText(row, "СЛУХАЛИ");
			var virishylyText = GetCellText(row, "ВИРІШИЛИ");
			var golosovanie = GetCellText(row, "ГОЛОСУВАЛИ");
			var commissionResult = GetCellText(row, "Результат");

			var objectText = JoinParts(", ", renterName, streetName, houseNumber);
			var incomingText = BuildDocumentRefText("Вх. № ", incomingDocNum, incomingDocDate);
			var outgoingText = BuildDocumentRefText("Вих. № ", outgoingDocNum, outgoingDocDate);
			var refsText = JoinParts(" ", incomingText, outgoingText);
			if (!string.IsNullOrWhiteSpace(refsText))
			{
				objectText = objectText + " (" + refsText + ")";
			}

			var mainText = number + ". Про розгляд звернення " + convertedOrgGiver + " щодо питання \"Продовження\" - " + objectText;

			AddItemParagraph(section, mainText);
			AddSpeakerParagraph(section, speakerName);
			AddLabelParagraph(section, "СЛУХАЛИ: ", slukhaliText);
			AddLabelParagraph(section, "ВИРІШИЛИ: ", virishylyText);
			AddLabelParagraph(section, "ГОЛОСУВАЛИ: ", golosovanie);
			AddDecisionParagraph(section, string.IsNullOrWhiteSpace(commissionResult) ? "-" : commissionResult);
			AddSpacerParagraph(section, 16f);
		}
	}

	private string BuildDocumentRefText(string prefix, string docNum, string docDate)
	{
		if (string.IsNullOrWhiteSpace(docNum) && string.IsNullOrWhiteSpace(docDate))
		{
			return string.Empty;
		}

		if (string.IsNullOrWhiteSpace(docDate))
		{
			return prefix + docNum;
		}

		if (string.IsNullOrWhiteSpace(docNum))
		{
			return "від " + docDate;
		}

		return prefix + docNum + " від " + docDate;
	}

	private void AddCenteredParagraph(IWSection section, string text, float fontSize, bool bold, bool italic, float beforeSpacing, float afterSpacing)
	{
		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
		paragraph.ParagraphFormat.BeforeSpacing = beforeSpacing;
		paragraph.ParagraphFormat.AfterSpacing = afterSpacing;
		var range = paragraph.AppendText(text);
		range.CharacterFormat.FontName = "Times New Roman";
		range.CharacterFormat.FontSize = fontSize;
		range.CharacterFormat.Bold = bold;
		range.CharacterFormat.Italic = italic;
	}

	private void AddItemParagraph(IWSection section, string text)
	{
		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
		paragraph.ParagraphFormat.BeforeSpacing = 0f;
		paragraph.ParagraphFormat.AfterSpacing = 6f;
		var range = paragraph.AppendText(text);
		range.CharacterFormat.FontName = "Times New Roman";
		range.CharacterFormat.FontSize = 14f;
		range.CharacterFormat.Bold = true;
	}

	private void AddSpeakerParagraph(IWSection section, string speakerName)
	{
		if (string.IsNullOrWhiteSpace(speakerName))
		{
			return;
		}

		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
		paragraph.ParagraphFormat.BeforeSpacing = 0f;
		paragraph.ParagraphFormat.AfterSpacing = 6f;
		var range = paragraph.AppendText("Доповідач: " + speakerName);
		range.CharacterFormat.FontName = "Times New Roman";
		range.CharacterFormat.FontSize = 14f;
		range.CharacterFormat.Italic = true;
	}

	private void AddLabelParagraph(IWSection section, string label, string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}

		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
		paragraph.ParagraphFormat.BeforeSpacing = 0f;
		paragraph.ParagraphFormat.AfterSpacing = 6f;

		var labelRange = paragraph.AppendText(label);
		labelRange.CharacterFormat.FontName = "Times New Roman";
		labelRange.CharacterFormat.FontSize = 14f;
		labelRange.CharacterFormat.Bold = false;

		var textRange = paragraph.AppendText(text);
		textRange.CharacterFormat.FontName = "Times New Roman";
		textRange.CharacterFormat.FontSize = 14f;
		textRange.CharacterFormat.Bold = false;
	}

	private void AddDecisionParagraph(IWSection section, string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}

		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
		paragraph.ParagraphFormat.BeforeSpacing = 0f;
		paragraph.ParagraphFormat.AfterSpacing = 0f;
		var range = paragraph.AppendText(text);
		range.CharacterFormat.FontName = "Times New Roman";
		range.CharacterFormat.FontSize = 14f;
		range.CharacterFormat.Bold = true;
		range.CharacterFormat.Italic = true;
	}

	private void AddSpacerParagraph(IWSection section, float afterSpacing)
	{
		var paragraph = section.AddParagraph();
		paragraph.ParagraphFormat.AfterSpacing = afterSpacing;
		var range = paragraph.AppendText(" ");
		range.CharacterFormat.FontName = "Times New Roman";
		range.CharacterFormat.FontSize = 1f;
	}

	private string JoinParts(string separator, params string[] parts)
	{
		return string.Join(separator, parts.Where(q => !string.IsNullOrWhiteSpace(q)).Select(q => q.Trim()));
	}

	private string GetCellText(DataRow row, string columnName)
	{
		if (!row.Table.Columns.Contains(columnName))
			return string.Empty;

		var value = row[columnName];
		if (value == null || value is DBNull)
			return string.Empty;

		var dataType = row.Table.Columns[columnName].DataType;
		if (dataType == typeof(string))
			return value.ToString();
		if (dataType == typeof(DateTime))
			return ((DateTime)value).ToString("dd.MM.yyyy");
		if (dataType == typeof(decimal))
			return ((decimal)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(double))
			return ((double)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(float))
			return ((float)value).ToString("0.##", CultureInfo.InvariantCulture);
		if (dataType == typeof(int))
			return ((int)value).ToString(CultureInfo.InvariantCulture);
		if (dataType == typeof(long))
			return ((long)value).ToString(CultureInfo.InvariantCulture);

		return value.ToString();
	}

	private string GetMainSql(List<int> ids)
	{
		return @"
SELECT
	isnull(fs.protocol_question_num, row_number() over (order by org.short_name, b.street_full_name, b.addr_nomer, fs.total_free_sqr)) as ""№"",
	isnull(conv.target_text, org_giver.short_name) as ""converted_org_giver"",
	org_renter.full_name as ""Найменування орендаря"",
	b.street_full_name as ""Назва Вулиці"",
	b.addr_nomer as ""Номер Будинку"",
	fs.incoming_doc_num as ""Вхідний номер"",
	fs.incoming_doc_date as ""Дата вхідного документа"",
	fs.outgoing_doc_num as ""Вихідний номер"",
	fs.outgoing_doc_date as ""Дата вихідного документа"",
	fs.speaker_name as ""Доповідач"",
	fs.slukhali_text as ""СЛУХАЛИ"",
	fs.virishyly_text as ""ВИРІШИЛИ"",
	fs.golosovanie as ""ГОЛОСУВАЛИ"",
	fs.commission_result as ""Результат""
FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.org_balans_id
left join organizations org_renter on org_renter.id = bal.org_renter_id
left join organizations org_giver ON org_giver.id = bal.org_giver_id and (org_giver.is_deleted is null or org_giver.is_deleted = 0)
left join dbo.texts_org_giver_convert conv on conv.source_text = org_giver.short_name
WHERE fs.id in (" + string.Join(",", ids) + @")
ORDER BY 1";
	}
}

