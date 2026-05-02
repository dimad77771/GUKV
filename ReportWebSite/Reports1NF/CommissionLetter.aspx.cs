using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

public partial class Reports1NF_Cabinet : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		var id = Int32.Parse(Request.QueryString["id"]);

		var builder = new CommissionLetter
		{
			ID = id,
			Page = Page,
		};
		builder.Run();
	}
}

public class CommissionLetter
{
	public Page Page;
	public int ID;

	public void Run()
	{
		string templateFileName = Page.Server.MapPath("Templates/" + "Шаблон_власком_ПК_КМКЛ_продовження.docx");

		if (templateFileName.Length > 0)
		{
			using (TempFile tempFile = TempFile.FromExistingFile(templateFileName))
			{
				var properties = new Dictionary<string, string>();
				using (var connection = Utils.ConnectToDatabase())
				{
					GetData(connection, properties);
				}

				var docx = new WordDocument(tempFile.FileName, FormatType.Docx);
				ReplacePlaceholders(docx, properties);
				docx.Save(tempFile.FileName, FormatType.Docx);
				docx.Close();

				var info = new FileInfo(tempFile.FileName);
				var outfile = "Лист на комісію " + ID + ".docx";
				Page.Response.Clear();
				Page.Response.ClearHeaders();
				Page.Response.ClearContent();
				Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
				Page.Response.AddHeader("content-disposition", "attachment; filename=" + outfile + "; size=" + info.Length.ToString());

				using (var stream = File.Open(tempFile.FileName, FileMode.Open, FileAccess.ReadWrite))
				{
					stream.CopyTo(Page.Response.OutputStream);
				}
				Page.Response.End();
			}
		}
	}

	void ReplacePlaceholders(WordDocument docx, Dictionary<string, string> properties)
	{
		foreach (var pair in properties)
		{
			docx.Replace(pair.Key, pair.Value ?? string.Empty, true, true);
		}
	}

	void GetData(SqlConnection connection, Dictionary<string, string> properties)
	{
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = GetMainSql();
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}

		if (dataTable.Rows.Count == 0)
		{
			return;
		}

		var row = dataTable.Rows[0];

		properties["{{Вхідний номер звернення}}"] = GetCellText(row, "Вхідний номер звернення");
		properties["{{Дата вхідного звернення}}"] = GetCellText(row, "Дата вхідного звернення");

		properties["{{TN-унікальний номер}}"] = GetCellText(row, "Реєстраційний номер");
		properties["{{Балансоутримувач}}"] = JoinParts(
			GetCellText(row, "Найменування балансоутримувача"),
			GetCellText(row, "Код ЕДРПОУ балансоутримувача")
		);
		properties["{{Об'єкт оренди}}"] = JoinParts(
			GetCellText(row, "Назва Вулиці"),
			GetCellText(row, "Номер Будинку")
		);
		properties["{{Тип будинку}}"] = GetCellText(row, "Тип будинку");
		properties["{{Характеристика об'єкта оренди}}"] = GetCellText(row, "Характеристика об’єкта оренди");
		properties["{{Вартість об'єкту, грн.}}"] = GetCellText(row, "Залишкова балансова вартість, грн.");
		properties["{{Дата оцінки}}"] = GetCellText(row, "Дата формування залишкової вартості");

		properties["{{Назва орендаря}}"] = GetCellText(row, "Найменування орендаря");
		properties["{{Код ЄДРПОУ}}"] = GetCellText(row, "Код ЕДРПОУ орендаря");

		properties["{{Цільове призначення}}"] = GetCellText(row, "Цільове використання");
		properties["{{Орендована площа, кв.м.}}"] = GetCellText(row, "Загальна площа об’єкта");
		properties["{{Орендна ставка, %}}"] = GetCellText(row, "Орендна ставка, %");
		properties["{{Тип оренди}}"] = GetCellText(row, "Тип оренди");
		properties["{{Місячна орендна плата, грн.}}"] = GetCellText(row, "Місячна орендна плата за договором");
		properties["{{Місячна орендна плата за останній}}"] = GetCellText(row, "Місячна орендна плата за останній");

		properties["{{Строк / термін оренди}}"] = GetCellText(row, "Строк / термін оренди");
		properties["{{Примітка}}"] = GetCellText(row, "Примітка");
		properties["{{Додаткова інформація}}"] = GetCellText(row, "Додаткова інформація");
	}

	string JoinParts(params string[] parts)
	{
		return string.Join(" ", parts.Where(q => !string.IsNullOrWhiteSpace(q)).Select(q => q.Trim()));
	}

	string GetCellText(DataRow row, string columnName)
	{
		if (!row.Table.Columns.Contains(columnName))
			return string.Empty;

		var value = row[columnName];
		if (value == null || value is DBNull)
			return string.Empty;

		var dataColumn = row.Table.Columns[columnName];
		var dataType = dataColumn.DataType;

		if (dataType == typeof(string))
			return value.ToString();
		if (dataType == typeof(DateTime))
			return ((DateTime)value).ToString("dd.MM.yyyy");
		if (dataType == typeof(decimal))
			return ((decimal)value).ToString("0.00");
		if (dataType == typeof(double))
			return ((double)value).ToString("0.00");
		if (dataType == typeof(float))
			return ((float)value).ToString("0.00");
		if (dataType == typeof(int))
			return ((int)value).ToString();
		if (dataType == typeof(long))
			return ((long)value).ToString();

		return value.ToString();
	}

	string GetMainSql()
	{
		return @"
SELECT
    cast(fs.id as varchar(50)) as ""Реєстраційний номер"",

	fs.""incoming_doc_num"" as ""Вхідний номер звернення"",
	fs.""incoming_doc_date"" as ""Дата вхідного звернення"",

    org.full_name as ""Найменування балансоутримувача"",
	org.zkpo_code as ""Код ЕДРПОУ балансоутримувача"",
    org_renter.zkpo_code as ""Код ЕДРПОУ орендаря"",
    b.street_full_name as ""Назва Вулиці"",
    b.addr_nomer as ""Номер Будинку"",
    fs.building_type as ""Тип будинку"",
    fs.floor as ""Характеристика об’єкта оренди"",
    fs.zal_balans_vartist as ""Залишкова балансова вартість, грн."",
    fs.zalbalansvartist_date as ""Дата формування залишкової вартості"",

    org_renter.full_name as ""Найменування орендаря"",
    org_renter.zkpo_code as ""Код ЕДРПОУ орендаря"",

    fs.possible_using as ""Цільове використання"",
    fs.total_free_sqr as ""Загальна площа об’єкта"",
    fs.rental_rate_percent as ""Орендна ставка, %"",
    fs.rental_type as ""Тип оренди"",
    fs.orend_plat_dogovor as ""Місячна орендна плата за договором"",
	fs.orend_plat_last_month as ""Місячна орендна плата за останній"",

    fs.rental_term as ""Строк / термін оренди"",
    fs.commission_note as ""Примітка"",
    fs.additional_info as ""Додаткова інформація""
FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.org_balans_id
left join organizations org_renter on org_renter.id = bal.org_renter_id
WHERE fs.id = " + ID;
	}
}
