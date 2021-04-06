using DevExpress.Web;
using DevExpress.Web.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Users : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(Utils.UsersAdmin))
        {
            string errorurl = Page.ResolveClientUrl("~/Account/Restricted.aspx");
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(errorurl);
            else
                Response.Redirect(errorurl);
        }
    }

    protected void ASPxGridViewFreeSquare_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        foreach (GridViewColumn column in ASPxGridViewFreeSquare.Columns)
        {
            GridViewDataColumn dataColumn = column as GridViewDataColumn;
            if (dataColumn == null) continue;
            string fieldName = dataColumn.FieldName.ToLower();

            if (fieldName == "Email")
            {
                if (e.NewValues[dataColumn.FieldName] == null)
                    e.Errors[dataColumn] = "Заповніть адресу електронної пошти";
                if (e.NewValues[dataColumn.FieldName] != null && ((String)e.NewValues[dataColumn.FieldName]).Length > 256)
                    e.Errors[dataColumn] = "Адреса електронної пошти не може бути довше 256 знаків";
            }

            if (fieldName == "UserName")
            {
                if (e.NewValues[dataColumn.FieldName] == null)
                    e.Errors[dataColumn] = "Заповніть ПІБ користувача";
                if (e.NewValues[dataColumn.FieldName] != null && ((String)e.NewValues[dataColumn.FieldName]).Length > 256)
                    e.Errors[dataColumn] = "ПІБ користувача не може бути довше 256 знаків";
            }

        }

        if (e.Errors.Count > 0)
            e.RowError = "Заповніть обов'язкові поля.";
    }

    protected void ASPxGridViewFreeSquare_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        if (!ASPxGridViewFreeSquare.IsNewRowEditing)
        {
            ASPxGridViewFreeSquare.DoRowValidation();
        }
    }

	protected void ASPxButton_AllBuildings_ExportCSV_Click(object sender, EventArgs e)
	{
		string templateFileName = Server.MapPath("Templates/UsersIncludedToEmail.docx");

		if (templateFileName.Length > 0)
		{
			using (TempFile tempFile = TempFile.FromExistingFile(templateFileName))
			{
				using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(tempFile.FileName, true))
				{
					MainDocumentPart mainPart = wordDocument.MainDocumentPart;

					UpdateTemplateFile(mainPart);

					wordDocument.Close();

					// Dump the document contents to the output stream
					System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

					Response.Clear();
					Response.ClearHeaders();
					Response.ClearContent();
					Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
					Response.AddHeader("content-disposition", "attachment; filename=UsersIncludedToEmail.docx; size=" + info.Length.ToString());

					// Pipe the stream contents to the output stream
					using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName,
						System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
					{
						stream.CopyTo(Response.OutputStream);
					}
				}

				Response.End();
			}
		}
	}

	void UpdateTemplateFile(MainDocumentPart mainPart)
	{
		Dictionary<string, string> properties = new Dictionary<string, string>();

		SqlConnection connection = Utils.ConnectToDatabase();

		if (connection != null)
		{
			var emails = new List<string>();

			string query = @"select 
distinct m.Email
from aspnet_Users u
join aspnet_Membership m on m.UserId = u.UserId
where m.IsIncludedToEmail = 1
order by 1";
			using (SqlCommand cmd = new SqlCommand(query, connection))
			{
				//cmd.Parameters.AddWithValue("free_square_id", Free_square_id);
				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						string email = reader.IsDBNull(0) ? string.Empty : (string)reader.GetValue(0);
						if (!string.IsNullOrEmpty(email))
						{
							emails.Add(email);
						}
					}
					reader.Close();
				}
			}

			var allemails = string.Join(";", emails);
			properties.Add("{EMAIL_LIST}", allemails);

			connection.Close();
		}

		foreach (KeyValuePair<string, string> pair in properties)
		{
			ReplaceDocTagInElements(mainPart, pair.Key, pair.Value);
		}
	}

	void ReplaceDocTagInElements(MainDocumentPart mainPart, string tag, string replacement)
	{
		List<OpenXmlElement> elements = mainPart.Document.Body.OfType<OpenXmlElement>().ToList();

		foreach (OpenXmlElement element in elements)
		{
			if (element is Paragraph)
			{
				ReplaceDocTag(mainPart, element as Paragraph, tag, replacement);
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
							if (e is Paragraph)
							{
								ReplaceDocTag(mainPart, e as Paragraph, tag, replacement);
							}
						}
					}
				}
			}
		}
	}



	void ReplaceDocTag(MainDocumentPart mainPart, Paragraph para, string tag, string replacement)
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
			Run firstRun = para.OfType<Run>().First<Run>();

			if (firstRun == null)
			{
				firstRun = new Run();
			}

			// Delete all paragraph sub-items
			para.RemoveAllChildren<Run>();

			// Add the text to the first Run
			firstRun.RemoveAllChildren<Text>();
			firstRun.AppendChild(new Text(paragraphText));

			// Create a new run with the modified text
			para.AppendChild(firstRun);
		}
	}

	protected string GetParagraphText(Paragraph para)
	{
		string paragraphText = "";

		List<Run> runs = para.OfType<Run>().ToList();

		foreach (Run run in runs)
		{
			List<Text> texts = run.OfType<Text>().ToList();

			foreach (Text text in texts)
			{
				paragraphText += text.Text;
			}
		}

		return paragraphText;
	}

}