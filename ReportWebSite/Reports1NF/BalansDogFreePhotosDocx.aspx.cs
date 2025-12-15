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
		var repmode = Request.QueryString["repmode"];
		if (string.IsNullOrEmpty(repmode))
		{
			repmode = "1";
		}

		var builder = new BalansDogContinuePhotosDocxRun
		{
			ID = id,
			Page = Page,
			RepMode = repmode,
		};
		builder.Run();
	}
}

public class BalansDogContinuePhotosDocxRun
{
	public Page Page;
	public int ID;
	public string RepMode;
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
		else if (datatype == typeof(bool))
		{
			var boolval = (bool)val;
			return boolval ? "так" : "ні";
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
		string templateFileName = Page.Server.MapPath("Templates/" + 
			(RepMode == "1" ? "Шаблон_Оголошення_на_аукціон_вільні.docx" : "Шаблон_договір_аукціон_вільні.docx"));

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


					wordDocument.Close();

					// Dump the document contents to the output stream
					System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

					var outfile = (RepMode == "1" ? "Оголошення про передачу нерухомого майна в оренду на аукціоні " : "Проект договору оренди ") 
										+ ID + ".docx";
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
	org.full_name as ""Балансоутримувач"",
	include_in_perelik as ""Включено до переліку №"",
	zalbalansvartist_date as ""Дата формування залишкової вартості"",
	total_free_sqr as ""Загальна площа об’єкта"",
	zal_balans_vartist as ""Залишкова балансова вартість, грн."",
	possible_using as ""Можливе використання вільного приміщення"",
	b.street_full_name as ""Назва Вулиці"",
	(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)), '')) as ""Номер Будинку"",
	osoba_oznakoml as ""Особа відповідальна за ознайомлення з об’єктом"",
	case when isnull(b.history, 'НІ') = 'НІ' then '' else 'ТАК' end as ""Пам’ятка культурної спадщини"", 
	perv_balans_vartist as ""Первісна балансова вартість, грн."",
	zg.name as ""Погодження органу охорони культурної спадщини"",
	komis_protocol as ""Погодження орендодавця"",
	power_text as ""Потужність електромережі"",
	prop_srok_orands as ""Пропонований строк оренди (у роках)"",
	rozmir_vidshkoduv as ""Розмір відшкодування земельного податку та інших"",
	heating as ""Теплопостачання"",
	condition as ""Технічний стан об’єкта"",
	(select qq.name from dict_free_object_type qq where qq.id = fs.free_object_type_id) as ""Тип об’єкта"",
	floor as ""Характеристика об’єкта оренди"",

	(SELECT TOP 1 Q.full_name FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Повна Назва"",
	(SELECT TOP 1 Q.short_name FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Скорочена Назва"",
	(SELECT TOP 1 Q.zkpo_code FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Код ЄДРПОУ"",
	(SELECT TOP 1 Q.addr_zip_code FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Поштовий Індекс"",
	(SELECT TOP 1 Q2.name FROM reports1nf_org_info Q join dict_streets Q2 on Q2.id = Q.phys_addr_street_id WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Назва Вулиці"",
	(SELECT TOP 1 Q.phys_addr_nomer FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Номер Будинку"",
	(SELECT TOP 1 Q.buhgalter_phone FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Тел. Бухгалтера"",
	(SELECT TOP 1 Q.buhgalter_email FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Ел. Адреса Бухгалтера"",
	(SELECT TOP 1 Q.director_email FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Ел. Адреса Керівника"",
	(SELECT TOP 1 Q.bank_rahunok FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.Поточні рахунки у відділеннях банку"",
	(SELECT TOP 1 Q.director_fio FROM reports1nf_org_info Q WHERE Q.report_id = rep.report_id) as ""Балансоутримувач.ПІБ Керівника""
FROM view_reports1nf rep
join reports1nf_balans bal on bal.report_id = rep.report_id
JOIN view_reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
join dbo.reports1nf_balans_free_square fs on fs.balans_id = bal.id and fs.report_id = rep.report_id
join reports1nf_org_info org on org.id = bal.organization_id
left join [dbo].[dict_streets] st on b.addr_street_id = st.id
left join dbo.dict_zgoda_renter zg on fs.zgoda_renter_id = zg.id
left join dbo.dict_zgoda_renter zg2 on fs.zgoda_control_id = zg2.id
WHERE fs.id = 4606
".Replace("4606", "" + ID);

	}

}
