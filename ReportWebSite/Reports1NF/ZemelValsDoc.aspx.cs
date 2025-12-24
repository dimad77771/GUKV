using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using DevExpress.Web;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using GUKV;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using DevExpress.Spreadsheet;
using GUKV.Common;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using WP = DocumentFormat.OpenXml.Wordprocessing;
using System.Drawing;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;


public partial class Account_Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected void Unnamed_Click(object sender, EventArgs e)
	{
		new ZemelValsDocOgoloshena() { Page = this.Page, Main = this }.Run();
	}

	public class ZemelValsDocOgoloshena
	{
		public Page Page;
		public Account_Register Main;
		public int ID;
		public bool IsOgoloshena;

		DataTable dataTable;
		DataTable dogDataTable;

		void UpdateTemplateFile(MainDocumentPart mainPart)
		{
			Dictionary<string, object> properties = new Dictionary<string, object>();

			properties.Add("{{column_1}}", Main.Column_1.Text);
			properties.Add("{{column_2}}", Main.Column_2.Text);
			properties.Add("{{column_3}}", Main.Column_3.Text);
			properties.Add("{{column_4}}", Main.Column_4.Text);
			properties.Add("{{column_5}}", Main.Column_5.Text);
			properties.Add("{{column_6}}", Main.Column_6.Text);

			SqlConnection connection = Utils.ConnectToDatabase();

			if (connection != null)
			{
				BuildExistsObject(connection, properties);
				connection.Close();
			}

			foreach (KeyValuePair<string, object> pair in properties)
			{
				var value = pair.Value == null ? "" : pair.Value.ToString();
				ReplaceDocTagInElements(mainPart, pair.Key, value);
			}
		}

		void BuildExistsObject(SqlConnection connection, Dictionary<string, object> properties)
		{
			var factory = DbProviderFactories.GetFactory(connection);
			dataTable = new DataTable();
			using (var cmd = factory.CreateCommand())
			{
				var sql = @"
select
*
from
(
	select
	vb.street_full_name, 
	(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '')) as dom,
	org_full_name,
	district,
	(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)), '')) as addr_nomer_new,
	orggospupr = (select old_organ from view_organizations WHERE organization_id =  vb.organization_id),
	vb.sqr_total,
	realestateobj,
	object_kind,
	object_type,
	bal.id,
	bal.report_id
	FROM view_balans_all vb
	LEFT JOIN reports1nf_balans bal on vb.balans_id = bal.id
	LEFT JOIN reports1nf_buildings b on bal.building_1nf_unique_id = b.unique_id
) T
where street_full_name like '%ГАВЕЛА%' and dom = '19'
";
				sql = sql.Replace("ГАВЕЛА", rd(Main.Column_7.Text));
				sql = sql.Replace("19", rd(Main.Column_5.Text));

				cmd.CommandText = sql;
				cmd.CommandType = CommandType.Text;
				cmd.Connection = connection;
				using (var adapter = factory.CreateDataAdapter())
				{
					adapter.SelectCommand = cmd;
					adapter.Fill(dataTable);
				}
			}
			var ex = false;
			var street_full_name = Main.Column_4.Text;
			var dom = Main.Column_5.Text;
			if (dataTable.Rows.Count > 0)
			{
				ex = true;
				var r = dataTable.Rows[0];
				street_full_name = (string)r["street_full_name"];
				dom = (string)r["dom"];
			}

			var moreOne = dataTable.Rows.Count > 1;

			var info = !ex ? 
				"Одночасно зазначаємо, що в Модулі «Облік та відображення об’єктів нерухомого майна територіальної громади міста Києва» (ІАС «Майно») за ознакою «просп. Повітрофлотський, 120» об’єктів не виявлено. Приватизація об’єктів нерухомості за вказаною адресою Департаментом не здійснювалась, договори оренди не укладались." :
				"Одночасно зазначаємо, що в Модулі «Облік та відображення об’єктів нерухомого майна територіальної громади міста Києва» (ІАС «Майно») за ознакою «просп. Повітрофлотський, 120» виявлено актуалізовані дані на " + (moreOne ? "об’єкти" : "об’єкт") + " комунальної власності.";

			info = info.Replace("просп. Повітрофлотський, 120", "" + street_full_name + ", " + dom);

			properties.Add("{{ADD_INFO}}", info);
			if (!ex)
			{
				properties.Add("{{ADD_INFO_TABLE}}", "");
			}

			BuildDogovorInfo(connection);

			if (dogDataTable.Rows.Count == 0)
			{
				properties.Add("{{ADD_DOGOVOR_TABLE}}", "");
			}
		}

		string rd(string arg)
		{
			if (string.IsNullOrEmpty(arg))
				return "XXXXX";
			else
				return arg.Replace("'","''");
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
			string templateFileName = Page.Server.MapPath("Templates/Шаблон_Деп_зем.docx");

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

						ReplaceObjectTable(tempFile.FileName);
						ReplaceDogovorTable(tempFile.FileName);

						// Dump the document contents to the output stream
						System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

						var outfile = "ЛИСТ_ДЕПАРТАМЕНТУ_КОМУНАЛЬНОЇ_ВЛАСНОСТІ_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".docx";
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

		public void ReplaceObjectTable(string docxfilename)
		{
			if (string.IsNullOrEmpty(docxfilename))
				throw new ArgumentNullException("docxfilename");

			// Open existing document
			using (WordDocument document = new WordDocument(docxfilename, FormatType.Docx))
			{
				const string placeholder = "{{ADD_INFO_TABLE}}";

				// Find placeholder paragraph
				TextSelection selection = document.Find(placeholder, false, false);
				if (selection == null)
				{
					return;
				}

				WTextRange textRange = selection.GetAsOneRange();
				WParagraph ownerParagraph = textRange.OwnerParagraph;
				WTextBody textBody = ownerParagraph.OwnerTextBody;
				WSection ownerSection = textBody.Owner as WSection;

				if (ownerSection == null)
					throw new InvalidOperationException("Cannot determine section for placeholder paragraph.");

				int insertIndex = textBody.ChildEntities.IndexOf(ownerParagraph);

				textBody.ChildEntities.Remove(ownerParagraph);

				//WParagraph heading = new WParagraph(document);
				//IWTextRange headingText = heading.AppendText("Об'єкти на балансах");
				//headingText.CharacterFormat.Bold = true;
				//headingText.CharacterFormat.FontSize = 14f;
				//heading.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
				//heading.ParagraphFormat.BeforeSpacing = 6f;
				//heading.ParagraphFormat.AfterSpacing = 6f;
				//textBody.ChildEntities.Insert(insertIndex++, heading);

				WTable table = new WTable(document);
				table.TableFormat.IsAutoResized = true;

				table.ResetCells(2, 9);

				WTableRow headerRow0 = table.Rows[0];
				int rowIndex = headerRow0.GetRowIndex();
				table.ApplyHorizontalMerge(rowIndex, 0, headerRow0.Cells.Count - 1);
				headerRow0.IsHeader = true;
				headerRow0.RowFormat.Paddings.All = 2f;
				var cell0 = headerRow0.Cells[0];
				cell0.CellFormat.BackColor = Color.LightGray;
				cell0.CellFormat.VerticalAlignment = VerticalAlignment.Middle;

				IWParagraph p0 = cell0.AddParagraph();
				p0.ParagraphFormat.AfterSpacing = 1f;
				p0.ParagraphFormat.BeforeSpacing = 1f;
				p0.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;

				IWTextRange t0 = p0.AppendText(string.Empty);
				t0.CharacterFormat.Bold = true;
				t0.CharacterFormat.FontSize = 14f;
				t0.Text = dataTable.Rows.Count > 1 ? "Об'єкти на балансах" : "Об'єкт на балансі";

				WTableRow headerRow = table.Rows[1];
				headerRow.IsHeader = true;
				headerRow.RowFormat.Paddings.All = 2f;
				var headers = new string[] {
					"Балансоутримувач - Повна Назва",
					"Район",
					"Назва Вулиці",
					"Номер об'єкту",
					"Орган госп. упр.",
					"Площа нежилих приміщень об'єкту (кв.м.)",
					"Реєстрація у Державному реєстрі (Об'єкт нерухомого майна)",
					"Вид Об'єкту відповідно Класифікатора майна",
					"Тип Об'єкту"
				};

				// Simple header formatting
				for (int i = 0; i < headerRow.Cells.Count; i++)
				{
					headerRow.Cells[i].CellFormat.BackColor = Color.LightGray;
					headerRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;

					IWParagraph p = headerRow.Cells[i].AddParagraph();
					p.ParagraphFormat.AfterSpacing = 1f;
					p.ParagraphFormat.BeforeSpacing = 1f;
					p.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;

					IWTextRange t = p.AppendText(string.Empty);
					t.CharacterFormat.Bold = true;
					t.CharacterFormat.FontSize = 8f;
					t.Text = headers[i];
				}

				// Data rows - put your loop here
				FillObjectTableRows(table);

				// Insert table after heading
				textBody.ChildEntities.Insert(insertIndex, table);

				// Save and close
				document.Save(docxfilename, FormatType.Docx);
			}
		}

		private void FillObjectTableRows(WTable table)
		{
			foreach (DataRow r in dataTable.Rows)
			{
				var row = table.AddRow(true);

				var values = new string[] {
					s(r["org_full_name"]),
					s(r["district"]),
					s(r["street_full_name"]),
					s(r["addr_nomer_new"]),
					s(r["orggospupr"]),
					s(r["sqr_total"]),
					s(r["realestateobj"]),
					s(r["object_kind"]),
					s(r["object_type"]),
				};

				for (int i = 0; i < row.Cells.Count; i++)
				{
					row.Cells[i].CellFormat.BackColor = Color.White;
					row.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;

					var p = row.Cells[i].AddParagraph();
					p.ParagraphFormat.AfterSpacing = 1f;
					p.ParagraphFormat.BeforeSpacing = 1f;
					p.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;

					IWTextRange t = p.AppendText(string.Empty);
					t.CharacterFormat.Bold = false;
					t.CharacterFormat.FontSize = 8f;
					t.Text = values[i];
				}
			}
		}

		public void ReplaceDogovorTable(string docxfilename)
		{
			if (string.IsNullOrEmpty(docxfilename))
				throw new ArgumentNullException("docxfilename");

			// Open existing document
			using (WordDocument document = new WordDocument(docxfilename, FormatType.Docx))
			{
				const string placeholder = "{{ADD_DOGOVOR_TABLE}}";

				// Find placeholder paragraph
				TextSelection selection = document.Find(placeholder, false, false);
				if (selection == null)
				{
					return;
				}

				WTextRange textRange = selection.GetAsOneRange();
				WParagraph ownerParagraph = textRange.OwnerParagraph;
				WTextBody textBody = ownerParagraph.OwnerTextBody;
				WSection ownerSection = textBody.Owner as WSection;

				if (ownerSection == null)
					throw new InvalidOperationException("Cannot determine section for placeholder paragraph.");

				int insertIndex = textBody.ChildEntities.IndexOf(ownerParagraph);

				textBody.ChildEntities.Remove(ownerParagraph);

				WTable table = new WTable(document);
				table.TableFormat.IsAutoResized = true;

				table.ResetCells(2, 6);

				WTableRow headerRow0 = table.Rows[0];
				int rowIndex = headerRow0.GetRowIndex();
				table.ApplyHorizontalMerge(rowIndex, 0, headerRow0.Cells.Count - 1);
				headerRow0.IsHeader = true;
				headerRow0.RowFormat.Paddings.All = 2f;
				var cell0 = headerRow0.Cells[0];
				cell0.CellFormat.BackColor = Color.LightGray;
				cell0.CellFormat.VerticalAlignment = VerticalAlignment.Middle;

				IWParagraph p0 = cell0.AddParagraph();
				p0.ParagraphFormat.AfterSpacing = 1f;
				p0.ParagraphFormat.BeforeSpacing = 1f;
				p0.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;

				IWTextRange t0 = p0.AppendText(string.Empty);
				t0.CharacterFormat.Bold = true;
				t0.CharacterFormat.FontSize = 14f;
				//t0.Text = dataTable.Rows.Count > 1 ? "Об'єкти на балансах" : "Об'єкт на балансі";
				t0.Text = "Таблиця договорів оренди";

				WTableRow headerRow = table.Rows[1];
				headerRow.IsHeader = true;
				headerRow.RowFormat.Paddings.All = 2f;
				var headers = new string[] {
					"Орендар - Коротка Назва",
					"Орендодавець - Коротка Назва",
					"Назва Вулиці",
					"Номер Будинку",
					"Номер Договору Оренди",
					"Закінчення Оренди",
				};

				// Simple header formatting
				for (int i = 0; i < headerRow.Cells.Count; i++)
				{
					headerRow.Cells[i].CellFormat.BackColor = Color.LightGray;
					headerRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;

					IWParagraph p = headerRow.Cells[i].AddParagraph();
					p.ParagraphFormat.AfterSpacing = 1f;
					p.ParagraphFormat.BeforeSpacing = 1f;
					p.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;

					IWTextRange t = p.AppendText(string.Empty);
					t.CharacterFormat.Bold = true;
					t.CharacterFormat.FontSize = 8f;
					t.Text = headers[i];
				}

				// Data rows - put your loop here
				FillDogovorObjectTableRows(table);

				// Insert table after heading
				textBody.ChildEntities.Insert(insertIndex, table);

				// Save and close
				document.Save(docxfilename, FormatType.Docx);
			}
		}

		private void FillDogovorObjectTableRows(WTable table)
		{
			foreach (DataRow r in dogDataTable.Rows)
			{
				var row = table.AddRow(true);

				var values = new string[] {
					s(r["org_renter_short_name"]),
					s(r["org_giver_short_name"]),
					s(r["street_full_name"]),
					s(r["addr_nomer"]),
					s(r["agreement_num"]),
					s(r["rent_finish_date"]),
				};

				for (int i = 0; i < row.Cells.Count; i++)
				{
					row.Cells[i].CellFormat.BackColor = Color.White;
					row.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;

					var p = row.Cells[i].AddParagraph();
					p.ParagraphFormat.AfterSpacing = 1f;
					p.ParagraphFormat.BeforeSpacing = 1f;
					p.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;

					IWTextRange t = p.AppendText(string.Empty);
					t.CharacterFormat.Bold = false;
					t.CharacterFormat.FontSize = 8f;
					t.Text = values[i];
				}
			}
		}

		private static string s(object arg)
		{
			if (arg == null) return "";

			if (arg.GetType() == typeof(DateTime))
			{
				var arg2 = (DateTime)arg;
				return arg2.ToString("dd.MM.yyyy");
			}
			else if (arg.GetType() == typeof(Decimal))
			{
				var arg2 = (Decimal)arg;
				return arg2.ToString("0.00");
			}
			else if (arg.GetType() == typeof(bool))
			{
				var arg2 = (bool)arg;
				return arg2 ? "так" : "ні";
			}
			return arg.ToString();

		}

		void BuildDogovorInfo(SqlConnection connection)
		{
			List<int> object_ids = new List<int>();
			foreach (DataRow r in dataTable.Rows)
			{
				var id = (int)r["id"];
				object_ids.Add(id);
			}

			var factory = DbProviderFactories.GetFactory(connection);
			dogDataTable = new DataTable();
			using (var cmd = factory.CreateCommand())
			{
				var sql = @"
SELECT 
org_renter_short_name,
org_giver_short_name,
street_full_name,
addr_nomer,
agreement_num,

rent_finish_date

FROM reptab_RentAgreements A
WHERE 
A.arenda_id in (select distinct Q.arenda_id from view_arenda Q where Q.ref_balans_id in (45980,30782) and isnull(Q.is_deleted,0)=0)
";
				sql = sql.Replace("45980,30782", string.Join(",", object_ids.Select(x => x.ToString())));
				cmd.CommandText = sql;
				cmd.CommandType = CommandType.Text;
				cmd.Connection = connection;
				using (var adapter = factory.CreateDataAdapter())
				{
					adapter.SelectCommand = cmd;
					adapter.Fill(dogDataTable);
				}
			}
		}

	}
}
