using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

public static class RentInspectionAct
{
    public static void Download(HttpContext context, int reportId, int agreementId, int inspectionId)
    {
        const string sql = @"SELECT i.*, tenant.full_name AS tenant_name, tenant.zkpo_code AS tenant_code,
                giver.full_name AS lessor_name
            FROM dbo.reports1nf_arenda_inspections i
            JOIN dbo.reports1nf_arenda a ON a.report_id=i.report_id AND a.id=i.arenda_id
            LEFT JOIN dbo.organizations tenant ON tenant.id=a.org_renter_id
            LEFT JOIN dbo.organizations giver ON giver.id=a.org_giver_id
            WHERE i.id=@id AND i.report_id=@rid AND i.arenda_id=@aid";
        var values = new Dictionary<string, string>();
        var culture = CultureInfo.GetCultureInfo("uk-UA");
        using (var connection = Utils.ConnectToDatabase())
        using (var cmd = new SqlCommand(sql, connection))
        {
            cmd.Parameters.AddWithValue("@id", inspectionId);
            cmd.Parameters.AddWithValue("@rid", reportId);
            cmd.Parameters.AddWithValue("@aid", agreementId);
            using (var row = cmd.ExecuteReader())
            {
                if (!row.Read()) throw new HttpException(404, "Перевірку не знайдено");
                for (int i = 0; i < row.FieldCount; i++)
                {
                    object value = row.GetValue(i);
                    values[row.GetName(i)] = value is DateTime
                        ? ((DateTime)value).ToString("dd.MM.yyyy")
                        : Convert.ToString(value, culture);
                }
            }
        }

        using (var stream = new MemoryStream())
        {
            using (var template = File.OpenRead(context.Server.MapPath("~/Reports1NF/Templates/RentInspectionAct.docx")))
                template.CopyTo(stream);
            stream.Position = 0;
            using (var doc = WordprocessingDocument.Open(stream, true))
            {
                foreach (var text in doc.MainDocumentPart.Document.Descendants<Text>().ToList())
                {
                    string filled = Regex.Replace(text.Text, @"\{(\w+)\}", m => values[m.Groups[1].Value]);
                    string[] lines = filled.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
                    foreach (var line in lines.Take(lines.Length - 1))
                    {
                        text.InsertBeforeSelf(new Text(line) { Space = SpaceProcessingModeValues.Preserve });
                        text.InsertBeforeSelf(new Break());
                    }
                    text.Text = lines[lines.Length - 1];
                    text.Space = SpaceProcessingModeValues.Preserve;
                }
                doc.MainDocumentPart.Document.Save();
            }
            var response = context.Response;
            response.Clear();
            response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            response.AddHeader("Content-Disposition", "attachment; filename=RentInspection-" + inspectionId + ".docx");
            stream.WriteTo(response.OutputStream);
            response.End();
        }
    }
}
