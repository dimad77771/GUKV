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
using ExtDataEntry.Models;
using DevExpress.Compression;

using Syncfusion.XlsIO;
using WP = DocumentFormat.OpenXml.Wordprocessing;
using System.Data.Common;

public partial class Reports1NF_Cabinet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var id = Int32.Parse(Request.QueryString["id"]);

		new Report1NFFreeSquareZvit1()
		{
			Page = Page,
			ID = id
		}.Run();
	}


	public class Report1NFFreeSquareZvit1
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
				cmd.CommandText = @"
select
a.*,
b.org_full_name,
b.street_full_name,
b.addr_nomer
FROM reports1nf_zvernenya a 
left join view_balans_all b on b.balans_id = a.balans_id
where a.id = " + ID;

				cmd.CommandType = CommandType.Text;
				cmd.Connection = connection;
				using (var adapter = factory.CreateDataAdapter())
				{
					adapter.SelectCommand = cmd;
					adapter.Fill(dataTable);
				}
			}
			var r = dataTable.Rows[0];

			properties.Add("{Звернення від}", r["zvernen_vid"]);
			properties.Add("{Орендована площа кВ.м.}", r["bazhana_ploshad"]);
			properties.Add("{Балансоутримувач - Повна Назва}", r["org_full_name"]);
			properties.Add("{Назва Вулиці}", r["street_full_name"]);
			properties.Add("{Номер Будинку}", r["addr_nomer"]);
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
			string templateFileName = Page.Server.MapPath("Templates/" + "Звернення орендаря.docx");

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

						var outfile = "Звернення орендаря.docx";
						Page.Response.Clear();
						Page.Response.ClearHeaders();
						Page.Response.ClearContent();
						Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
						Page.Response.AddHeader("content-disposition", "attachment; filename=" + outfile + "; size=" + info.Length.ToString());

						// Pipe the stream contents to the output stream
						using (var stream = File.Open(tempFile.FileName, FileMode.Open, FileAccess.ReadWrite))
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