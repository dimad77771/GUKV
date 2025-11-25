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
			var dataTable = new DataTable();
			using (var cmd = factory.CreateCommand())
			{
				var sql = @"select
street_full_name, dom
from
(
	select
	vb.street_full_name, 
	-- (COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer3)) + ' ', '') + COALESCE(LTRIM(RTRIM(b.addr_nomer2)), '')) as find_dom,
	(COALESCE(LTRIM(RTRIM(b.addr_nomer1)) + ' ', '')) as dom
	FROM view_balans_all vb
	LEFT JOIN reports1nf_balans bal on vb.balans_id = bal.id
	LEFT JOIN reports1nf_buildings b on bal.building_1nf_unique_id = b.unique_id
) T
where street_full_name like '%ГАВЕЛА%' and dom = '19'";
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

			var info = !ex ? 
				"Одночасно зазначаємо, що в Модулі «Облік та відображення об’єктів нерухомого майна територіальної громади міста Києва» (ІАС «Майно») за ознакою «просп. Повітрофлотський, 120» об’єктів не виявлено. Приватизація об’єктів нерухомості за вказаною адресою Департаментом не здійснювалась, договори оренди не укладались." :
				"Одночасно зазначаємо, що в Модулі «Облік та відображення об’єктів нерухомого майна територіальної громади міста Києва» (ІАС «Майно») за ознакою «просп. Повітрофлотський, 120» виявлено актуалізовані дані на об’єкт комунальної власності.";

			info = info.Replace("просп. Повітрофлотський, 120", "" + street_full_name + ", " + dom);

			properties.Add("{{ADD_INFO}}", info);
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

	}
}
