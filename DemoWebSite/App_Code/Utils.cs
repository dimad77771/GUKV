using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Configuration;
using DevExpress.Web.ASPxGridView.Export;
using System.IO;
using System.Text;

public static class Utils
{
    const string
        CurrentThemeCookieKeyPrefix = "DXCurrentTheme",
        DefaultTheme = "DevEx";

    static HttpContext Context
    {
        get { return HttpContext.Current; }
    }

    static HttpRequest Request
    {
        get { return Context.Request; }
    }

    public static string CurrentThemeCookieKey
    {
        get { return CurrentThemeCookieKeyPrefix + ConfigurationManager.AppSettings["ThemeCookie"]; }
    }

    public static string CurrentTheme
    {
        get
        {
            if (Request.Cookies[CurrentThemeCookieKey] != null)
                return HttpUtility.UrlDecode(Request.Cookies[CurrentThemeCookieKey].Value);
            return DefaultTheme;
        }
    }

    public static string GetVersionText()
    {
        string[] parts = AssemblyInfo.Version.Split('.');
        return string.Format("v{0} vol {1}.{2}", 2000 + int.Parse(parts[0]), parts[1], parts[2]);
    }

    public static void RegisterCssLink(Page page, string url)
    {
        RegisterCssLink(page, url, null);
    }

    public static void RegisterCssLink(Page page, string url, string clientId)
    {
        HtmlLink link = new HtmlLink();
        page.Header.Controls.Add(link);
        link.EnableViewState = false;
        link.Attributes["type"] = "text/css";
        link.Attributes["rel"] = "stylesheet";
        if (!string.IsNullOrEmpty(clientId))
            link.Attributes["id"] = clientId;
        link.Href = url;
    }

    public static void EnsureRequestValidationMode()
    {
        try
        {
            if (Environment.Version.Major >= 4)
            {
                Type type = typeof(WebControl).Assembly.GetType("System.Web.Configuration.RuntimeConfig");
                MethodInfo getConfig = type.GetMethod("GetConfig", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { }, null);
                object runtimeConfig = getConfig.Invoke(null, null);
                MethodInfo getSection = type.GetMethod("GetSection", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string), typeof(Type) }, null);
                HttpRuntimeSection httpRuntimeSection = (HttpRuntimeSection)getSection.Invoke(runtimeConfig, new object[] { "system.web/httpRuntime", typeof(HttpRuntimeSection) });
                FieldInfo bReadOnly = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                bReadOnly.SetValue(httpRuntimeSection, false);

                PropertyInfo pi = typeof(HttpRuntimeSection).GetProperty("RequestValidationMode");
                if (pi != null)
                {
                    Version version = (Version)pi.GetValue(httpRuntimeSection, null);
                    Version RequiredRequestValidationMode = new Version(2, 0);
                    if (version != null && !Version.Equals(version, RequiredRequestValidationMode))
                    {
                        pi.SetValue(httpRuntimeSection, RequiredRequestValidationMode, null);
                    }
                }
                bReadOnly.SetValue(httpRuntimeSection, true);
            }
        }
        catch { }
    }

    public static void ExportGridToXLS(this Page page, ASPxGridViewExporter exporter)
    {
        page.Response.AddHeader("Content-Type", "application/vnd.ms-excel");

        page.Response.AddHeader("Content-Disposition",
            string.Format("attachment; filename={0}.xls", exporter.FileName));

        using (TempFile tempFile = new TempFile())
        {
            using (FileStream stream = File.Open(tempFile.FileName, FileMode.Create, FileAccess.ReadWrite))
            {
                exporter.WriteXls(stream);

                // Rewind and pipe the stream contents to the output stream
                stream.Position = 0;
                stream.CopyTo(page.Response.OutputStream);
            }
        }

        page.Response.End();
    }

    public static void ExportGridToPDF(this Page page, ASPxGridViewExporter exporter)
    {
        page.Response.AddHeader("Content-Type", "application/pdf");

        page.Response.AddHeader("Content-Disposition",
            string.Format("attachment; filename={0}.pdf", exporter.FileName));

        using (TempFile tempFile = new TempFile())
        {
            using (FileStream stream = File.Open(tempFile.FileName, FileMode.Create, FileAccess.ReadWrite))
            {
                exporter.WritePdf(stream);

                // Rewind and pipe the stream contents to the output stream
                stream.Position = 0;
                stream.CopyTo(page.Response.OutputStream);
            }
        }

        page.Response.End();
    }

    public static void ExportGridToCSV(this Page page, ASPxGridViewExporter exporter)
    {
        DevExpress.XtraPrinting.CsvExportOptions options = new DevExpress.XtraPrinting.CsvExportOptions()
        {
            Separator = ",",
            Encoding = Encoding.UTF8,
            TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text,
            QuoteStringsWithSeparators = true,
        };

        page.Response.AddHeader("Content-Type", "text/csv");

        page.Response.AddHeader("Content-Disposition",
            string.Format("attachment; filename={0}.csv", exporter.FileName));

        exporter.WriteCsv(page.Response.OutputStream, options);

        page.Response.End();
    }

    public static string EscapeJSONParam(this string text)
    {
        if (text == null)
            return text;
        return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
