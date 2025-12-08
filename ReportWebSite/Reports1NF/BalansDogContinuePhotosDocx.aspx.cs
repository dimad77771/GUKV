using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.Cryptography;
using System.Text;
using DevExpress.Web;
using System.IO;
using Syncfusion.Pdf;
using System.Web.Configuration;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Parsing;
using Syncfusion.DocToPDFConverter;
using Syncfusion.DocIO.DLS;
using Syncfusion.Compression.Zip;
using System.Drawing.Imaging;
using WP = DocumentFormat.OpenXml.Wordprocessing;
using System.Data.Common;
using System.Diagnostics;



public partial class Reports1NF_Cabinet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var id = Int32.Parse(Request.QueryString["id"]);

		var builder = new BalansDogContinuePhotosDocxRun
		{
			ID = id,
			Page = Page,
		};
		builder.Run();
	}
}

public class BalansDogContinuePhotosDocxRun
{
	public Page Page;
	public int ID;
	public bool IsOgoloshena;

	void UpdateTemplateFile(MainDocumentPart mainPart)
	{
		Dictionary<string, object> properties = new Dictionary<string, object>();

		SqlConnection connection = Utils.ConnectToDatabase();

		if (connection != null)
		{
			GetData(connection, properties);
			connection.Close();
		}


		foreach (KeyValuePair<string, object> pair in properties)
		{
			var value = pair.Value == null ? "" : pair.Value.ToString();
			ReplaceDocTagInElements(mainPart, pair.Key, value);
		}
	}

	void GetData(SqlConnection connection, Dictionary<string, object> properties)
	{
		DateTime dtNow = DateTime.Now;
		string currentDate = "\xAB" + " " + dtNow.Day.ToString() + " " + "\xBB" + " " + GetDateMonthName(dtNow) + " " + dtNow.Year.ToString();
		properties.Add("{REPORT_PRINT_DATE}", currentDate);

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
		var r = dataTable.Rows[0];

		var allcolums = dataTable.Columns;
		foreach(DataColumn dcolumn in allcolums)
		{
			var name = dcolumn.ColumnName;
			var val = r[dcolumn.ColumnName];
			var text = GetCellText(val, dcolumn);
			properties.Add("{" + name + "}", text);

			Debug.WriteLine("name=" + name);
		}
	}

	string GetCellText(object val, DataColumn datacolumn)
	{
		if (val == null || val is DBNull)
			return "";

		var datatype = datacolumn.DataType;
		if (datatype == typeof(string))
		{
			return val.ToString();
		}
		else if (datatype == typeof(DateTime))
		{
			var datetime = (DateTime)val;
			return datetime.ToString("dd.MM.yyyy");
		}
		else if (datatype == typeof(Decimal))
		{
			var dec = (Decimal)val;
			return dec.ToString("0.00");
		}
		else if (datatype == typeof(int))
		{
			var intval = (int)val;
			return intval.ToString();
		}
		else
		{
			throw new Exception();
		}
	}


	string nayavn(object arg)
	{
		if (arg == null) return "";
		else if (arg.ToString().ToLower() == "Так") return "в наявності";
		else if (arg.ToString().ToLower() == "Ні") return "відсутнє";
		else return arg.ToString().Trim();
	}

	string p(string prefix, object arg)
	{
		var text = (arg == null ? "" : arg.ToString()).Trim();
		return (prefix + text).Trim();
	}

	string j(string del, params object[] arg)
	{
		return string.Join(del, arg.Select(q => q == null ? "" : q.ToString()).Where(q => !string.IsNullOrEmpty(q)));
	}

	string GetDecimal(object arg, string format = "0.00")
	{
		return arg is DBNull ? "" : ((decimal)arg).ToString(format);
	}

	string GetDate(object arg)
	{
		return arg is DBNull ? "" : ((DateTime)arg).ToString("dd.MM.yyyy");
	}

	void ReplaceDocTagInElements(MainDocumentPart mainPart, string tag, string replacement)
	{
		List<OpenXmlElement> elements = mainPart.Document.Body.OfType<OpenXmlElement>().ToList();

		foreach (OpenXmlElement element in elements)
		{
			if (element is WP.Paragraph)
			{
				ReplaceDocTag(mainPart, element as WP.Paragraph, tag, replacement);
			}
			else if (element is DocumentFormat.OpenXml.Wordprocessing.Table)
			{
				List<DocumentFormat.OpenXml.Wordprocessing.TableRow> rows = element.ChildElements.OfType<DocumentFormat.OpenXml.Wordprocessing.TableRow>().ToList();

				foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow row in rows)
				{
					List<DocumentFormat.OpenXml.Wordprocessing.TableCell> cells = row.ChildElements.OfType<DocumentFormat.OpenXml.Wordprocessing.TableCell>().ToList();

					foreach (DocumentFormat.OpenXml.Wordprocessing.TableCell cell in cells)
					{
						foreach (OpenXmlElement e in cell.ChildElements)
						{
							if (e is WP.Paragraph)
							{
								ReplaceDocTag(mainPart, e as WP.Paragraph, tag, replacement);
							}
						}
					}
				}
			}
		}
	}

	void ReplaceDocTag(MainDocumentPart mainPart, WP.Paragraph para, string tag, string replacement)
	{
		string paragraphText = GetParagraphText(para);

		// Replace the tag
		bool replaced = false;
		int pos = paragraphText.IndexOf(tag);

		while (pos >= 0)
		{
			replaced = true;

			paragraphText = paragraphText.Replace(tag, replacement);

			// Search once again
			pos = paragraphText.IndexOf(tag);
		}

		if (replaced)
		{
			WP.Run firstRun = para.OfType<WP.Run>().First<WP.Run>();

			if (firstRun == null)
			{
				firstRun = new WP.Run();
			}

			// Delete all paragraph sub-items
			para.RemoveAllChildren<WP.Run>();

			// Add the text to the first Run
			firstRun.RemoveAllChildren<WP.Text>();
			firstRun.AppendChild(new WP.Text(paragraphText));

			// Create a new run with the modified text
			para.AppendChild(firstRun);
		}
	}

	string GetParagraphText(WP.Paragraph para)
	{
		string paragraphText = "";

		List<WP.Run> runs = para.OfType<WP.Run>().ToList();

		foreach (WP.Run run in runs)
		{
			List<WP.Text> texts = run.OfType<WP.Text>().ToList();

			foreach (WP.Text text in texts)
			{
				paragraphText += text.Text;
			}
		}

		return paragraphText;
	}

	public void Run()
	{
		string templateFileName = Page.Server.MapPath("Templates/" + "ProzoroDogContinue.docx");

		if (templateFileName.Length > 0)
		{
			using (TempFile tempFile = TempFile.FromExistingFile(templateFileName))
			{
				using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(tempFile.FileName, true))
				{
					MainDocumentPart mainPart = wordDocument.MainDocumentPart;

					if (mainPart != null)
					{
						UpdateTemplateFile(mainPart);
					}

					//SaltedHash sh = new SaltedHash("123");
					//DocumentSettingsPart docSett = wordDocument.MainDocumentPart.DocumentSettingsPart;
					//DocumentProtection documentProtection = new DocumentProtection();
					//documentProtection.Edit = DocumentProtectionValues.ReadOnly;
					//OnOffValue docProtection = new OnOffValue(true);
					//documentProtection.Enforcement = docProtection;

					//documentProtection.CryptographicAlgorithmClass = CryptAlgorithmClassValues.Hash;
					//documentProtection.CryptographicProviderType = CryptProviderValues.RsaFull;
					//documentProtection.CryptographicAlgorithmType = CryptAlgorithmValues.TypeAny;
					//documentProtection.CryptographicAlgorithmSid = 4; // SHA1
					//												  //    The iteration count is unsigned
					//												  //UInt32Value uintVal = new UInt32Value();
					//												  //uintVal.Value = (uint)123;
					//documentProtection.CryptographicSpinCount = 1;
					//documentProtection.Hash = sh.Hash;
					//documentProtection.Salt = sh.Salt;
					//wordDocument.MainDocumentPart.DocumentSettingsPart.Settings.AppendChild(documentProtection);
					//wordDocument.MainDocumentPart.DocumentSettingsPart.Settings.Save();

					wordDocument.Close();

					// Dump the document contents to the output stream
					System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

					var outfile = "Оголошення про продовження договорів оренди на аукціоні " + ID + ".docx";
					Page.Response.Clear();
					Page.Response.ClearHeaders();
					Page.Response.ClearContent();
					Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
					Page.Response.AddHeader("content-disposition", "attachment; filename=" + outfile + "; size=" + info.Length.ToString());

					// Pipe the stream contents to the output stream
					using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName,
						System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
					{
						stream.CopyTo(Page.Response.OutputStream);
					}
				}

				Page.Response.End();
			}
		}
	}

	string GetDateMonthName(object date)
	{
		if (date is DateTime)
		{
			switch (((DateTime)date).Month)
			{
				case 1:
					return Resources.Strings.Month1;

				case 2:
					return Resources.Strings.Month2;

				case 3:
					return Resources.Strings.Month3;

				case 4:
					return Resources.Strings.Month4;

				case 5:
					return Resources.Strings.Month5;

				case 6:
					return Resources.Strings.Month6;

				case 7:
					return Resources.Strings.Month7;

				case 8:
					return Resources.Strings.Month8;

				case 9:
					return Resources.Strings.Month9;

				case 10:
					return Resources.Strings.Month10;

				case 11:
					return Resources.Strings.Month11;

				case 12:
					return Resources.Strings.Month12;
			}
		}

		return "";
	}

	string GetMainSql() 
	{
		return @"
SELECT
	fs.total_free_sqr as ""Загальна площа об’єкта"",
	b.street_full_name as ""Назва Вулиці"",
	b.addr_nomer as ""Номер Будинку"",
	agreement_date as ""Дата укладання договору"",
	agreement_num as ""Номер договору"",
	rent_finish_date as ""Дата закінчення договору"",
	org_renter.full_name as ""Найменування орендаря"",
	org_renter.zkpo_code as ""Код ЕДРПОУ орендаря"",
	org.short_name as ""Найменування балансоутримувача"",
	org.zkpo_code as ""Код ЕДРПОУ балансоутримувача"",
	(select Q.name from dict_streets Q where Q.id = org.addr_street_id) as ""Адреса балансоутримувача(вулиця)"",
	org.addr_nomer as ""Адреса балансоутримувача(номер дому)"",
	total_free_sqr as ""Загальна площа об’єкта"",
	zalbalansvartist_date as ""Дата формування залишкової вартості"",
	zal_balans_vartist as ""Залишкова балансова вартість, грн."",
	perv_balans_vartist as ""Первісна балансова вартість, грн."",
	floor as ""Характеристика об’єкта оренди"",
	cast(round(DATEDIFF(month, bal.agreement_date, bal.rent_finish_date) / 12.0, 0) as int) as ""Строк оренди(роки)"",
	fs.free_sqr_korysna as ""Корисна площа об’єкта"",
	power_text as ""Потужність електромережі"",
	zg.name as ""Погодження органу охорони культурної спадщини"",
	fs.orend_plat_last_month as ""Місячна орендна плата за останній місяць(проіндексована)"",
	(select Q.name from dict_may_pravo_prodov Q where Q.id = fs.may_pravo_prodov) as ""Цільове використання"",
	rozmir_vidshkoduv as ""Розмір відшкодування земельного податку та інших"",
	case when prozoro_number <> '' then 'https://prozorro.sale/auction/' + rtrim(ltrim(prozoro_number)) else '' end as ""Унікальний код обєкту у ЕТС Прозорро-продажі"",
	(SELECT TOP 1 Q.prozoro_title FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Контактні дані працівника балансоутримувача""
FROM view_reports1nf rep
join reports1nf_arenda bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_arenda_dogcontinue fs on fs.arenda_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.org_balans_id
left join[dbo].[dict_streets] st on b.addr_street_id = st.id
left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id
left join organizations org_renter on org_renter.id = bal.org_renter_id
left outer join organizations org_giver ON org_giver.id = bal.org_giver_id and(org_giver.is_deleted is null or org_giver.is_deleted = 0)
LEFT JOIN
(
	select obp.org_id
	, occ.name
	, occ.id
	, per.name as period
	from org_by_period obp
	join dict_rent_period per on per.id = obp.period_id and per.is_active = 1
	join dict_rent_occupation occ on occ.id = obp.org_occupation_id
) DDD ON DDD.org_id = rep.organization_id

WHERE fs.id = 1001775
".Replace("1001775", "" + ID);

	}

}