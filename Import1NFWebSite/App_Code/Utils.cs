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
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Data.Filtering;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Data.SqlClient;

public static class Utils
{
    const string CurrentThemeCookieKeyPrefix = "DXCurrentTheme";
    
    const string DefaultTheme = "DevEx";

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

    public static string EscapeJSONParam(this string text)
    {
        if (text == null)
            return text;
        return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    #region Database access

    /// <summary>
    /// Attempts to connect to the Import1NF database
    /// </summary>
    /// <returns>Created connection object, or NULL if connection could not be established</returns>
    public static SqlConnection ConnectToDatabase()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Import1NFConnectionString"].ConnectionString;

        if (connectionString != null && connectionString.Length > 0)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                return connection;
            }
            catch
            {
                //
            }
        }

        return null;
    }

    #endregion (Database access)

    #region Grid export

    public static void ExportGridToXLS(this Page page, ASPxGridViewExporter exporter)
    {
        page.Response.AddHeader("Content-Type", "application/vnd.ms-excel");
        //page.Response.AddHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        page.Response.AddHeader("Content-Disposition",
            string.Format("attachment; filename={0}.xls", exporter.FileName));

        using (TempFile tempFile = new TempFile())
        {
            using (FileStream stream = File.Open(tempFile.FileName, FileMode.Create, FileAccess.ReadWrite))
            {
                /*
                DevExpress.XtraPrinting.XlsxExportOptions options = new DevExpress.XtraPrinting.XlsxExportOptions();
                options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                */
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;

                exporter.WriteXls(stream, options);

                // Rewind and pipe the stream contents to the output stream
                stream.Position = 0;
                stream.CopyTo(page.Response.OutputStream);
            }
        }

        // We need to hide the Grid title panel; it is made visible for export only
        if (exporter.GridView != null)
        {
            exporter.GridView.Settings.ShowTitlePanel = false;
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

        // We need to hide the Grid title panel; it is made visible for export only
        if (exporter.GridView != null)
        {
            exporter.GridView.Settings.ShowTitlePanel = false;
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

        // We need to hide the Grid title panel; it is made visible for export only
        if (exporter.GridView != null)
        {
            exporter.GridView.Settings.ShowTitlePanel = false;
        }

        page.Response.End();
    }

    #endregion (Grid export)

    #region Grid filter formatting

    public static void UpdateFilterDisplayTextCache(this Page page, string filter,
        DevExpress.Web.ASPxGridView.ASPxGridView grid)
    {
        // Update the filter string in the cache
        Dictionary<string, string> filterCache = null;

        if (page.Session["GridFilterDisplayTextCache"] is Dictionary<string, string>)
        {
            filterCache = page.Session["GridFilterDisplayTextCache"] as Dictionary<string, string>;
        }
        else
        {
            filterCache = new Dictionary<string, string>();

            page.Session["GridFilterDisplayTextCache"] = filterCache;
        }

        if (filterCache != null)
        {
            filterCache[grid.ID] = filter;
        }
    }

    public static string GetFilterDisplayTextFromCache(this Page page,
        DevExpress.Web.ASPxGridView.ASPxGridView grid)
    {
        Dictionary<string, string> filterCache = null;

        if (page.Session["GridFilterDisplayTextCache"] is Dictionary<string, string>)
        {
            filterCache = page.Session["GridFilterDisplayTextCache"] as Dictionary<string, string>;
        }
        else
        {
            filterCache = new Dictionary<string, string>();

            page.Session["GridFilterDisplayTextCache"] = filterCache;
        }

        if (filterCache != null && filterCache.ContainsKey(grid.ID))
        {
            object text = filterCache[grid.ID];

            if (text is string)
            {
                return (string)text;
            }
        }

        return "";
    }

    public static void AdjustGridColumnsForExport(DevExpress.Web.ASPxGridView.ASPxGridView grid)
    {
        double defaultColumnWidth = 80;

        // Calculate the total width of all visible columns
        double totalColumnWidth = 0;

        foreach (GridViewDataColumn column in grid.Columns)
        {
            if (column.Visible)
            {
                totalColumnWidth += (column.Width.IsEmpty) ? defaultColumnWidth : column.Width.Value;
            }
        }

        double maxTotalWidth = 900.0;
        double scale = maxTotalWidth / totalColumnWidth;

        if (scale > 1.0)
        {
            scale = 1.0;
        }

        // Scale the width of all visible columns so that they fit to one page (roughly)
        foreach (GridViewDataColumn column in grid.Columns)
        {
            if (column.Visible)
            {
                double curWidth = (column.Width.IsEmpty) ? defaultColumnWidth : column.Width.Value;
                double exportWidth = curWidth * scale;

                if (exportWidth < defaultColumnWidth)
                {
                    exportWidth = defaultColumnWidth;
                }

                column.ExportWidth = (int)exportWidth;
            }
        }
    }

    public static void SetGridExporterProps(DevExpress.Web.ASPxGridView.ASPxGridView grid,
        DevExpress.Web.ASPxGridView.Export.ASPxGridViewExporter exporter,
        string reportTitle, Page page)
    {
        string strFilter = page.GetFilterDisplayTextFromCache(exporter.GridView);

        string dateSubTitle = String.Format(Resources.Strings.ReportDateFormat, DateTime.Now);
        string filterSubTitle = String.Format(Resources.Strings.ReportFilterFormat, strFilter);

        grid.Settings.ShowTitlePanel = true;

        if (strFilter.Length > 0)
        {
            grid.SettingsText.Title = reportTitle + "\n" + dateSubTitle + "\n" + filterSubTitle;
        }
        else
        {
            grid.SettingsText.Title = reportTitle + "\n" + dateSubTitle;
        }

        exporter.Styles.Title.Font.Size = 12;
        exporter.Styles.Title.Font.Bold = true;
        exporter.Styles.Title.Font.Name = "Arial";
        exporter.Styles.Title.ForeColor = System.Drawing.Color.Black;

        exporter.Landscape = true;

        Utils.AdjustGridColumnsForExport(exporter.GridView);
    }

    #endregion (Grid filter formatting)

    #region Custom field chooser support

    public static void GenerateFieldChooserNodes(ASPxListBox list, ASPxGridView grid, string captionPattern)
    {
        string pattern = captionPattern.Trim().ToLower();

        foreach (GridViewDataColumn column in grid.Columns)
        {
            if (column.ShowInCustomizationForm && column.Caption.Length > 0)
            {
                // Take the caption pattern into consideration
                if (pattern.Length == 0 || (pattern.Length > 0 && column.Caption.ToLower().Contains(pattern)))
                {
                    ListEditItem item = list.Items.Add(column.Caption, column.FieldName);

                    item.Selected = column.Visible;
                }
            }
        }

        list.DataBind();
    }

    #endregion (Custom field chooser support)
}
