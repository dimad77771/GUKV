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
using DevExpress.Web.Export;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Data.Filtering;
using System.IO;
using System.Text;
using System.Web.Security;

using System.Data;
using System.Data.SqlClient;

using System.Net.Mail;
using FirebirdSql.Data.FirebirdClient;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

using Cache;
using log4net;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using GUKV.Common;

public static class Utils
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Utils));

    const string CurrentThemeCookieKeyPrefix = "DXCurrentTheme";

    const string DefaultTheme = "DevEx";

    public const string ReportTreeNodeToken = "Report";
    public const string FolderTreeNodeToken = "Folder";

    public const string ReportManagerRole = "ReportManager";
    public const string RishProjectCreatorRole = "RishProjectCreator";
    public const string Report1NFReviewerRole = "1NFReportReviewer";
    public const string Report1NFSubmitterRole = "1NFReportSubmitter";
	public const string Report1NFReportCenterSubmitRole = "1NFReportCenterSubmit";
	public const string DKVOrganizationControllerRole = "DKVOrganizationController";
    public const string DKVObjectControllerRole = "DKVObjectController";
    public const string DKVArendaControllerRole = "DKVArendaController";
    public const string DKVArendaPaymentsControllerRole = "DKVArendaPaymentsController";
    public const string RDAControllerRole = "RDAController";
    public const string MISTOControllerRole = "MISTOController";
    public const string DictionartyAdmin = "DictionartyAdmin";
    public const string OrgOccupationAdmin = "OrgOccupationAdmin";
    public const string UsersAdmin = "UsersAdmin";
    public const string ConveyancingConfirmation = "ConveyancingConfirmation";

    public const string AdministratorRole = "Administrator";

    public const int AutoFilterRowInputDelay = 2500;

    public const int GridIDObjects_Buildings = 1;
    public const int GridIDObjects_Transfer = 2;
    // public const int GridIDObjects_Universal = 3;
    public const int GridIDBalans_Organizations = 4;
    public const int GridIDBalans_Objects = 5;
    public const int GridIDArenda_Organizations = 6;
    public const int GridIDArenda_Objects = 7;
    public const int GridIDDocuments_Documents = 8;
    public const int GridIDDocuments_Objects = 9;
    public const int GridIDPrivatization_Documents = 10;
    public const int GridIDPrivatization_Objects = 11;
    public const int GridID_Finance = 12;
    public const int GridIDBalans_Arenda = 13;
    public const int GridIDCatalogue_Buildings = 14;
    public const int GridIDCatalogue_Organizations = 15;
    public const int GridIDSysReports_Corrected = 16;
    public const int GridIDSysReports_Mismatches = 17;
    public const int GridIDOrendnaPlata_ByBalansOrg = 18;
    public const int GridIDOrendnaPlata_ByRenters = 19;
    public const int GridIDOrendnaPlata_ByBuildings = 20;
    public const int GridIDOrendnaPlata_Totals = 21;
    public const int GridIDOrendnaPlata_NotSubmitted = 22;
    public const int GridIDOrendnaPlata_ObjectsUse = 23;
    public const int GridIDAssessment_Objects = 24;
    public const int GridIDAssessment_Experts = 25;

    public const int GridIDArenda_Agreements = 26;
	public const int GridIDArenda_Subleases = 33;

	public const int GridIDReports1NF_ReportList = 30;

    public const int GridIDReports1NF_FreeSquare = 31;


    // TODO!!! УДАЛИТЬ ИДЕНТИФИКАТОРЫ ИЗ КОДА
    public const int GUKVOrganizationID = 27065;

    public const string GUKVzkpo = "19020407";

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

	#region Grid export

	public delegate bool ExportGridToXLS__ExportHiddenColumn(GridViewDataColumn column);
	public delegate void ExportGridToXLS__AfterBuildXlsx(string excelfilename);

	public static void ExportGridToXLS(this Page page, 
			ASPxGridViewExporter exporter,
            ASPxGridView grid, 
			string reportTitle, 
			string gridDataSourceID = null, 
			ExportGridToXLS__ExportHiddenColumn exportHiddenColumnCallback = null,
			ExportGridToXLS__AfterBuildXlsx afterBuildXlsx = null)
    {
        // If the data source ID is specified, bind the grid to data
        if (!string.IsNullOrEmpty(gridDataSourceID))
        {
            grid.DataSourceID = gridDataSourceID;
            grid.DataBind();
        }

        // Hide the Card columns; they should not be exported
        Dictionary<int, bool> cardColumns = new Dictionary<int, bool>();

        HideGridCardColumns(grid, cardColumns, exportHiddenColumnCallback);

        // Adjust column width
        SetGridExporterProps(grid, exporter, reportTitle, page);

        // page.Response.AddHeader("Content-Type", "application/vnd.ms-excel");
        // page.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", exporter.FileName));
        page.Response.AddHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        page.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", exporter.FileName));

        using (TempFile tempFile = new TempFile())
        {
            using (FileStream stream = File.Open(tempFile.FileName, FileMode.Create, FileAccess.ReadWrite))
            {
                // exporter.WriteXls(stream, new XlsExportOptions { TextExportMode = TextExportMode.Text });
                //exporter.WriteXlsx(stream, new DevExpress.XtraPrinting.XlsxExportOptions { TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value });
                exporter.WriteXlsx(stream, new DevExpress.XtraPrinting.XlsxExportOptionsEx
                {
                    TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value,
                    ExportType = DevExpress.Export.ExportType.WYSIWYG
                });


                // Rewind and pipe the stream contents to the output stream
                // stream.Position = 0;
                // stream.CopyTo(page.Response.OutputStream);
            }

            SetXlsxFilePrintOptions(tempFile.FileName);

			if (afterBuildXlsx != null)
			{
				afterBuildXlsx(tempFile.FileName);
			}

            // Rewind and pipe the stream contents to the output stream
            var readyToPrintFileStream = new MemoryStream(File.ReadAllBytes(tempFile.FileName));
            readyToPrintFileStream.Position = 0;
            readyToPrintFileStream.CopyTo(page.Response.OutputStream);
        }

        // We need to hide the Grid title panel; it is made visible for export only
        if (exporter.GridView != null)
        {
            exporter.GridView.Settings.ShowTitlePanel = false;
        }

        page.Response.End();

        // Restore column visibility
        RestoreGridCardColumns(grid, cardColumns);
    }

    public static void ExportGridToPDF(this Page page, ASPxGridViewExporter exporter,
        DevExpress.Web.ASPxGridView grid, string reportTitle, string gridDataSourceID = null)
    {
        // If the data source ID is specified, bind the grid to data
        if (!string.IsNullOrEmpty(gridDataSourceID))
        {
            grid.DataSourceID = gridDataSourceID;
            grid.DataBind();
        }

        // Hide the Card columns; they should not be exported
        Dictionary<int, bool> cardColumns = new Dictionary<int, bool>();

        HideGridCardColumns(grid, cardColumns);

        // Adjust column width
        //SetGridExporterProps(grid, exporter, reportTitle, page); we've switched to AutoFitToPagesWidth

        page.Response.AddHeader("Content-Type", "application/pdf");

        page.Response.AddHeader("Content-Disposition",
            string.Format("attachment; filename={0}.pdf", exporter.FileName));

        using (TempFile tempFile = new TempFile())
        {
            using (FileStream stream = File.Open(tempFile.FileName, FileMode.Create, FileAccess.ReadWrite))
            {
                DevExpress.XtraPrinting.PrintableComponentLink pcl
                    = new DevExpress.XtraPrinting.PrintableComponentLink(new DevExpress.XtraPrinting.PrintingSystem());
                pcl.Component = exporter;
                pcl.Margins.Left = pcl.Margins.Right = pcl.Margins.Bottom = pcl.Margins.Top = 5; //5 mm 
                pcl.Landscape = true;
                pcl.CreateDocument(false);
                pcl.PrintingSystem.Document.AutoFitToPagesWidth = 1;
                pcl.ExportToPdf(stream);

                //exporter.WritePdf(stream);

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

        // Restore column visibility
        RestoreGridCardColumns(grid, cardColumns);
    }

    public static void ExportGridToCSV(this Page page, ASPxGridViewExporter exporter,
        DevExpress.Web.ASPxGridView grid, string reportTitle, string gridDataSourceID = null)
    {
        // If the data source ID is specified, bind the grid to data
        if (!string.IsNullOrEmpty(gridDataSourceID))
        {
            grid.DataSourceID = gridDataSourceID;
            grid.DataBind();
        }

        // Hide the Card columns; they should not be exported
        Dictionary<int, bool> cardColumns = new Dictionary<int, bool>();

        HideGridCardColumns(grid, cardColumns);

        // Adjust column width
        SetGridExporterProps(grid, exporter, reportTitle, page);

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

        // Restore column visibility
        RestoreGridCardColumns(grid, cardColumns);
    }

    private static void HideGridCardColumns(DevExpress.Web.ASPxGridView grid,
        Dictionary<int, bool> cardColumns, ExportGridToXLS__ExportHiddenColumn exportHiddenColumnCallback = null)
    {
        for (int i = 0; i < grid.Columns.Count; i++)
        {
            GridViewDataColumn column = grid.Columns[i] as GridViewDataColumn;

			if (exportHiddenColumnCallback != null)
			{
				if (column != null)
				{
					if (exportHiddenColumnCallback(column))
					{
						column.Visible = true;
						continue;
					}
				}
			}

            // Not all columns in all grids are DATA columns, therefore casting
            // may result in a null reference (ex: command columns).

            if (column != null && column.Caption.ToLower().Trim().StartsWith(Resources.Strings.CardColumnTitle))
            {
                cardColumns[i] = column.Visible;

                column.Visible = false;
            }
        }
    }

    private static void RestoreGridCardColumns(DevExpress.Web.ASPxGridView grid,
        Dictionary<int, bool> cardColumns)
    {
        foreach (KeyValuePair<int, bool> pair in cardColumns)
        {
            (grid.Columns[pair.Key] as GridViewDataColumn).Visible = pair.Value;
        }
    }

    private static void SetXlsxFilePrintOptions(string targetFileName)
    {
        using (var spreadsheetDocument = SpreadsheetDocument.Open(targetFileName, true))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;

            foreach (var worksheetPart in workbookPart.WorksheetParts)
            {
                var ws = worksheetPart.Worksheet;
                var sp = new DocumentFormat.OpenXml.Spreadsheet.SheetProperties(new DocumentFormat.OpenXml.Spreadsheet.PageSetupProperties());
                ws.SheetProperties = sp;

                ws.SheetProperties.PageSetupProperties.FitToPage = BooleanValue.FromBoolean(true);
                //var pgOr = new DocumentFormat.OpenXml.Spreadsheet.PageSetup
                //{
                //    Orientation = DocumentFormat.OpenXml.Spreadsheet.OrientationValues.Landscape,
                //    FitToHeight = 0,
                //    FitToWidth = 1,
                //    PaperSize = 9
                //};

                //ws.AppendChild(pgOr);

                var ggg = ws.ChildElements.OfType<DocumentFormat.OpenXml.Spreadsheet.PageSetup>().ToArray();
                var gg1 = ggg[0];
                gg1.Orientation = DocumentFormat.OpenXml.Spreadsheet.OrientationValues.Landscape;
                gg1.FitToHeight = 0;
                gg1.FitToWidth = 1;
                gg1.PaperSize = 9;
            }
        }
    }

    #endregion (Grid export)

    #region Database access



    /// <summary>
    /// Attempts to connect to the 1NF database
    /// </summary>
    /// <returns>Created connection object, or NULL if connection could not be established</returns>
    //public static FbConnection ConnectTo1NF()
    //{
    //    string connectionString = ConfigurationManager.ConnectionStrings["1NFconnectionString"].ConnectionString;

    //    if (connectionString != null && connectionString.Length > 0)
    //    {
    //        FbConnection connection = new FbConnection(connectionString);

    //        try
    //        {
    //            connection.Open();

    //            return connection;
    //        }
    //        catch
    //        {
    //            //
    //        }
    //    }

    //    return null;
    //}



    /// <summary>

    /// Attempts to connect to the NJF database

    /// </summary>

    /// <returns>Created connection object, or NULL if connection could not be established</returns>

    //public static FbConnection ConnectToNJF()
    //{

    //    string connectionString = ConfigurationManager.ConnectionStrings["NJFconnectionString"].ConnectionString;



    //    if (connectionString != null && connectionString.Length > 0)
    //    {

    //        FbConnection connection = new FbConnection(connectionString);



    //        try
    //        {

    //            connection.Open();



    //            return connection;

    //        }

    //        catch
    //        {

    //            //

    //        }

    //    }



    //    return null;

    //}

    #endregion (Database access)

    #region Working with user-defined folders and reports

    public static int GetUserFolderId(string folderName, SqlConnection connection)
    {
        int folderId = -1;
        string name = folderName.Trim().ToUpper();

        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid && name.Length > 0 && connection != null)
        {
            using (SqlCommand command = new SqlCommand("SELECT id FROM user_folders " +
                "WHERE UPPER(RTRIM(LTRIM(folder_name))) = @fname", connection))
            {
                // command.Parameters.Add(new SqlParameter("@uid", (System.Guid)uid));
                command.Parameters.Add(new SqlParameter("@fname", name));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        folderId = (int)reader.GetValue(0);
                    }

                    reader.Close();
                }
            }
        }

        return folderId;
    }

    public static int AddUserFolder(int parentFolderId, string folderName)
    {
        int folderId = -1;
        string name = folderName.Trim();

        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid && folderName.Length > 0)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // Check that such folder already exists
                folderId = GetUserFolderId(name, connection);

                // Create a new folder if it does not exists
                if (folderId < 0)
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO user_folders " +
                            "(parent_folder_id, usr_id, folder_name) VALUES (@pfid, @uid, @fname)", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@pfid", parentFolderId));
                        command.Parameters.Add(new SqlParameter("@uid", (System.Guid)uid));
                        command.Parameters.Add(new SqlParameter("@fname", name));

                        command.ExecuteNonQuery();
                    }

                    // Retrieve the folder Id
                    folderId = GetUserFolderId(name, connection);
                }

                connection.Close();
            }
        }

        return folderId;
    }

    public static void DeleteUserFolder(int folderId)
    {
        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid && folderId > 0)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // Delete all reports that belong to this folder
                using (SqlCommand command = new SqlCommand("DELETE FROM user_reports " +
                        "WHERE (folder_id = @fid)", connection))
                {
                    command.Parameters.Add(new SqlParameter("@fid", folderId));
                    command.ExecuteNonQuery();
                }

                // Delete the folder itself
                using (SqlCommand command = new SqlCommand("DELETE FROM user_folders " +
                        "WHERE (id = @fid)", connection))
                {
                    command.Parameters.Add(new SqlParameter("@fid", folderId));
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }

    public static void DeleteUserFolder(string folderName)
    {
        string name = folderName.Trim().ToUpper();

        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid && name.Length > 0)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                int folderId = GetUserFolderId(name, connection);

                if (folderId > 0)
                {
                    // Delete all reports that belong to this folder
                    using (SqlCommand command = new SqlCommand("DELETE FROM user_reports " +
                            "WHERE (folder_id = @fid)", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@fid", folderId));
                        command.ExecuteNonQuery();
                    }

                    // Delete the folder itself
                    using (SqlCommand command = new SqlCommand("DELETE FROM user_folders " +
                            "WHERE (id = @fid)", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@fid", folderId));
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }
    }

    public static void AddUserReport(int folderId, int gridId, string reportName, string layout, string preFilter, string fixed_columns)
    {
        reportName = reportName.Trim().ToUpper();



        if (fixed_columns == null)
        {

            fixed_columns = "";

        }

        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // Check that such report already exists
                int existingReportId = -1;

                using (SqlCommand command = new SqlCommand("SELECT id FROM user_reports " +
                    "WHERE UPPER(RTRIM(LTRIM(report_name))) = @rname", connection))
                {
                    // command.Parameters.Add(new SqlParameter("@uid", (System.Guid)uid));
                    command.Parameters.Add(new SqlParameter("@rname", reportName));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            existingReportId = (int)reader.GetValue(0);
                        }

                        reader.Close();
                    }
                }

                if (existingReportId >= 0)
                {
                    // Update an existing report
                    using (SqlCommand command = new SqlCommand("UPDATE user_reports " +
                        "SET folder_id = @fid, category = @cat, grid_layout = @layout, pre_filter = @pf, fixed_columns = @pfixed_columns " +
                        "WHERE id = @rid", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@rid", existingReportId));
                        command.Parameters.Add(new SqlParameter("@fid", folderId));
                        command.Parameters.Add(new SqlParameter("@cat", gridId));
                        command.Parameters.Add(new SqlParameter("@layout", layout));
                        command.Parameters.Add(new SqlParameter("@pf", preFilter));
                        command.Parameters.Add(new SqlParameter("@pfixed_columns", fixed_columns));

                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Add a new report
                    using (SqlCommand command = new SqlCommand("INSERT INTO user_reports " +
                        "(usr_id, folder_id, report_name, category, grid_layout, pre_filter, fixed_columns) " +
                        "VALUES (@uid, @fid, @rname, @cat, @layout, @pf, @pfixed_columns)", connection))
                    {
                        command.Parameters.Add(new SqlParameter("@uid", (System.Guid)uid));
                        command.Parameters.Add(new SqlParameter("@fid", folderId));
                        command.Parameters.Add(new SqlParameter("@rname", reportName));
                        command.Parameters.Add(new SqlParameter("@cat", gridId));
                        command.Parameters.Add(new SqlParameter("@layout", layout));
                        command.Parameters.Add(new SqlParameter("@pf", preFilter));
                        command.Parameters.Add(new SqlParameter("@pfixed_columns", fixed_columns));

                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }
    }

    public static void DeleteUserReport(int reportId)
    {
        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid && reportId > 0)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM user_reports " +
                        "WHERE id = @rid", connection))
                {
                    // command.Parameters.Add(new SqlParameter("@uid", (System.Guid)uid));
                    command.Parameters.Add(new SqlParameter("@rid", reportId));

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }

    public static string GetUserReportLayout(int reportId, out int folderId,
        out string reportTitle, out string preFilter, out string fixedColumns)
    {
        folderId = 0;
        reportTitle = "";
        preFilter = "";
        fixedColumns = string.Empty;
        string layout = "";

        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid && reportId > 0)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                using (SqlCommand command = new SqlCommand("SELECT grid_layout, folder_id, report_name, pre_filter, fixed_columns " +
                        "FROM user_reports WHERE id = @rid", connection))
                {
                    // command.Parameters.Add(new SqlParameter("@uid", (System.Guid)uid));
                    command.Parameters.Add(new SqlParameter("@rid", reportId));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            layout = reader.GetString(0);
                            folderId = reader.GetInt32(1);
                            reportTitle = reader.GetString(2);

                            preFilter = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            fixedColumns = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        }

                        reader.Close();
                    }
                }

                connection.Close();
            }
        }

        return layout;
    }

    #endregion (Working with user-defined folders and reports)

    #region Document text downloading

    public static bool GetDocProperties(int documentId,
        out object docNumber,
        out object docDate,
        out object docTitle,
        out int externalDocId)
    {
        docNumber = null;
        docDate = null;
        docTitle = null;
        externalDocId = -1;

        bool documentFound = false;
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string query = "SELECT doc_date, doc_num, topic, extern_doc_id FROM documents WHERE id = " + documentId.ToString();

            using (SqlCommand commandSelect = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = commandSelect.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        object externDocId = reader.IsDBNull(3) ? null : reader.GetValue(3);

                        if (externDocId is int)
                        {
                            docDate = reader.IsDBNull(0) ? null : reader.GetValue(0);
                            docNumber = reader.IsDBNull(1) ? null : reader.GetValue(1);
                            docTitle = reader.IsDBNull(2) ? null : reader.GetValue(2);

                            externalDocId = (int)externDocId;
                            documentFound = true;
                        }
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        return documentFound;
    }

    public static bool GetExternalDocText(int documentId, TempFile tempFile, out bool isHtml)
    {
        isHtml = false;
        bool documentExists = false;

        string connectionString = ConfigurationManager.ConnectionStrings["GUKVDocBaseConnString"].ConnectionString;

        if (connectionString.Length > 0)
        {
            FbConnection connection = new FbConnection(connectionString);

            try
            {
                connection.Open();

                // Try to get the document text
                string query = "SELECT TEXT FROM RELEASE WHERE DOC_ID = " + documentId.ToString();

                using (FbCommand commandSelect = new FbCommand(query, connection))
                {
                    using (FbDataReader reader = commandSelect.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                object dataText = reader.GetValue(0);

                                if (dataText is string)
                                {
                                    // Extract the document text
                                    string text = (string)dataText;

                                    // Check if this is a HTML text
                                    if (text.StartsWith("<"))
                                    {
                                        isHtml = true;
                                    }

                                    // Convert the text to bytes using proper encoding
                                    byte[] bytes = null;

                                    if (isHtml)
                                    {
                                        bytes = System.Text.Encoding.GetEncoding("windows-1251").GetBytes(text);
                                    }
                                    else
                                    {
                                        bytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(text);
                                    }

                                    using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName,
                                        System.IO.FileMode.Append, System.IO.FileAccess.Write))
                                    {
                                        stream.Write(bytes, 0, bytes.Length);
                                        stream.Close();

                                        documentExists = true;
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }
                }

                connection.Close();
            }
            catch
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        return documentExists;
    }

    #endregion (Document text downloading)

    #region Common event handlers for all data grids

    public static void ProcessGridDataFetch(this Page page, StateBag viewState, DevExpress.Web.ASPxGridView grid)
    {
        if (!page.IsCallback && !page.IsPostBack)
        {
            viewState["PrimaryGridView.DataSourceID"] = grid.DataSourceID;
            grid.DataSourceID = null;
        }
        else if (page.IsCallback)
        {
            Control callbackTarget = page.FindControl(Request.Params["__CALLBACKID"]);

            if (string.IsNullOrEmpty(grid.DataSourceID))
            {
                grid.DataSourceID = viewState["PrimaryGridView.DataSourceID"] as string;

                if (callbackTarget == grid)
                    grid.DataBind();
            }
        }
    }

    public static void ProcessDataGridSaveLayoutCallback(string callbackParameter,
        ASPxGridView grid, int gridId, string preFilter)
    {
        string init = "init:";
        string bind = "bind:";
        string addReport = "addreport:";
        string addrPickerRequest = "building:";
        string fieldChooserRequest = "columns:";
        string fieldFixxerRequest = "columnsFixxer:";

        if (callbackParameter.StartsWith(fieldFixxerRequest))
        {
            var listOfFieldsToFix = callbackParameter.Substring(fieldFixxerRequest.Length);

            var cookieKey = string.Format("{0}.FixedColumns", grid.SettingsCookies.CookiesID);
            HttpContext.Current.Response.Cookies.Set(new HttpCookie(cookieKey, listOfFieldsToFix) { Expires = DateTime.Now.AddDays(15) });

            foreach (GridViewDataColumn column in grid.Columns.Cast<GridViewColumn>().Where(x => x is GridViewDataColumn))
            {
                if (column.ShowInCustomizationForm)
                {
                    column.FixedStyle = listOfFieldsToFix.Contains("#" + column.FieldName + "++#")
                        ? GridViewColumnFixedStyle.Left
                        : GridViewColumnFixedStyle.None;

                    column.CellStyle.BackColor = listOfFieldsToFix.Contains("#" + column.FieldName + "++#")
                        ? System.Drawing.ColorTranslator.FromHtml("#ffffd6")
                        : System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                }
            }

            grid.DataBind();
        }
        else if (callbackParameter.StartsWith(fieldChooserRequest))
        {
            string listOfFields = callbackParameter.Substring(fieldChooserRequest.Length);

            foreach (GridViewDataColumn column in grid.Columns.Cast<GridViewColumn>().Where(x => x is GridViewDataColumn))
            {
                if (column.ShowInCustomizationForm)
                {
                    if (listOfFields.Contains("#" + column.FieldName + "++#"))
                    {
                        column.Visible = true;
                    }
                    else if (listOfFields.Contains("#" + column.FieldName + "--#"))
                    {
                        column.Visible = false;
                    }
                }
            }

            grid.DataBind();
        }
        else if (callbackParameter.StartsWith(addrPickerRequest))
        {
            string streetAndBuilding = callbackParameter.Substring(addrPickerRequest.Length);

            if (streetAndBuilding.Length > 0)
            {
                // Apostrophe is a special character for Grid; it must be replaced with '' sequence
                streetAndBuilding = streetAndBuilding.Replace("'", "''");

                string[] parts = streetAndBuilding.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    string filter = "StartsWith([street_full_name], '" + parts[0] + "') And StartsWith([addr_nomer], '" + parts[1] + "')";

                    grid.FilterExpression = filter;
                }
            }
        }
        else if (callbackParameter.StartsWith(init))
        {
            if (GridLayoutToLoad.Length > 0 && GridIdToLoad == gridId)
            {
                string layout = GridLayoutToLoad;

                AdjustGridCookieVersion(grid, ref layout);



                grid.LoadClientLayout(FixGridFilterFormatPre_13_1(layout));

                GridLayoutToLoad = "";
                GridIdToLoad = -1;

                // Make sure that 'Card' columns are always visible
                AdjustColumnsVisibleInFilter(grid);
            }
        }
        else if (callbackParameter.StartsWith(addReport))
        {
            string param = callbackParameter.Substring(addReport.Length);

            // The parameter has format "NodeName:ReportName"

            // Exract the folder ID
            int pos = param.IndexOf(':');

            if (pos > 0)
            {
                string folderNameStr = param.Substring(0, pos);
                string folderIdStr = folderNameStr.Substring(Utils.FolderTreeNodeToken.Length);

                string reportName = param.Substring(pos + 1);
                int folderId = int.Parse(folderIdStr);

                if (folderId > 0)
                {
                    string layout = grid.SaveClientLayout();

                    var cookieKey = string.Format("{0}.FixedColumns", grid.SettingsCookies.CookiesID);

                    var listOfFieldsToFix = HttpContext.Current.Request.Cookies[cookieKey] != null
                        ? HttpContext.Current.Request.Cookies[cookieKey].Value
                        : null;

                    Utils.AddUserReport(folderId, gridId, reportName, layout, preFilter, listOfFieldsToFix);
                }
            }
        }
        else if (callbackParameter.StartsWith(bind))
        {
            grid.DataBind();
        }
    }

    private static string FixGridFilterFormatPre_13_1(string oldFilter)
    {
        string[] parts = oldFilter.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        string fixedFilter = "";

        foreach (string part in parts)
        {
            if (fixedFilter.Length > 0)
                fixedFilter += "|";

            if (part.StartsWith("filter"))
            {
                if (part.Contains('[') || part.Contains(']'))
                {
                    // This is an old style filter; convert it to a new format
                    string filter = part.Substring(6);

                    fixedFilter += "filter" + filter.Length.ToString() + "|";
                    fixedFilter += filter;
                }
                else
                {
                    // This is a new style filter
                    fixedFilter += part;
                }
            }
            else
            {
                fixedFilter += part;
            }
        }

        return fixedFilter;
    }

    public static int ProcessPageLoad(this Page page, out string reportTitle, out string preFilter)
    {
        var fixedColumns = string.Empty;
        return page.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns);
    }

    public static int ProcessPageLoad(this Page page, out string reportTitle, out string preFilter, out string fixedColumns)
    {
        Utils.GridLayoutToLoad = "";
        Utils.GridIdToLoad = -1;

        int gridId = 0;
        int reportId = 0;
        int folderId = 0;
        reportTitle = "";
        preFilter = "";
        fixedColumns = string.Empty;

        string gridIdStr = Request.QueryString["grid"];
        string reportIdStr = Request.QueryString["report"];

        if (gridIdStr != null && reportIdStr != null)
        {
            if (gridIdStr.Length > 0 && reportIdStr.Length > 0)
            {
                gridId = int.Parse(gridIdStr);
                reportId = int.Parse(reportIdStr);

                Utils.GridLayoutToLoad = Utils.GetUserReportLayout(reportId, out folderId, out reportTitle, out preFilter, out fixedColumns);
                Utils.GridIdToLoad = gridId;
                Utils.GridPreFilterToLoad = preFilter;
            }
        }

        return gridId;
    }

    public static void AdjustColumnsVisibleInFilter(DevExpress.Web.ASPxGridView grid)
    {
        foreach (GridViewDataColumn column in grid.Columns.Cast<GridViewColumn>().Where(x => x is GridViewDataColumn))
        {
            string fieldName = column.FieldName;

            if (fieldName.Contains("industry") ||
                fieldName.Contains("occupation") ||
                fieldName.Contains("status") ||
                fieldName.Contains("district") ||
                fieldName.Contains("form_gosp") ||
                fieldName.Contains("ownership") ||
                fieldName.Contains("org_form") ||
                fieldName.Contains("gosp_struct_type") ||
                fieldName.Contains("gukv") ||
                fieldName.Contains("purpose") ||
                fieldName.StartsWith("is_") ||
                fieldName.Contains("_kind") ||
                fieldName.Contains("_type") ||
                fieldName.Contains("year") ||
                fieldName.Contains("quarter") ||
                fieldName.Contains("active") ||
                fieldName.Contains("vedomstvo") ||
                fieldName.Contains("history") ||
                fieldName.Contains("condition") ||
                fieldName.Contains("dkk") ||
                fieldName.Contains("rent_period") ||
                fieldName.Contains("period_id") ||
                fieldName.StartsWith("gosp") ||
                fieldName.StartsWith("sphera") ||
                fieldName.StartsWith("right_name"))
            {
                column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
            }
            else
            {
                column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            }

            // Make sure that all columns show in Filter Editor correctly
            string filter = grid.FilterExpression;

            if (column.ShowInCustomizationForm)
            {
                if (filter.Contains(column.FieldName))
                {
                    column.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.True;
                }
            }

            // Make sure that 'Card' columns are always visible
            if (column.Caption.ToLower().StartsWith(Resources.Strings.CardColumnTitle))
            {
                column.Visible = true;
            }
        }
    }

    public static int ExtractBrowserWndHeight(ref string gridCallbackParam)
    {
        if (gridCallbackParam.StartsWith("["))
        {
            int dividerPos = gridCallbackParam.IndexOf("]:");

            string winHeightStr = gridCallbackParam.Substring(1, dividerPos - 1);

            gridCallbackParam = gridCallbackParam.Substring(dividerPos + 2);

            return int.Parse(winHeightStr);
        }

        return -1;
    }

    public static int GetProperGridPageSize(int winHeight, int usualRowHeight)
    {
        int pageSize = (winHeight - 350) / usualRowHeight;

        if (pageSize < 5)
        {
            pageSize = 5;
        }

        return pageSize;
    }

    public static void ProcessGridPageSizeInCallback(DevExpress.Web.ASPxGridView grid,
        ref string gridCallbackParam, int usualRowHeight)
    {
        int clientWinHeight = ExtractBrowserWndHeight(ref gridCallbackParam);

        if (clientWinHeight > 0)
        {
            // grid.SettingsPager.PageSize = GetProperGridPageSize(clientWinHeight, usualRowHeight);
        }
    }

    public static void ProcessGridColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        if (e.Kind == GridViewAutoFilterEventKind.CreateCriteria)
        {
            DevExpress.Data.Filtering.CriteriaOperator criteria = e.Criteria;

            if (criteria is DevExpress.Data.Filtering.UnaryOperator)
            {
                DevExpress.Data.Filtering.UnaryOperator u = criteria as DevExpress.Data.Filtering.UnaryOperator;

                if (u.OperatorType == DevExpress.Data.Filtering.UnaryOperatorType.Not)
                {
                    criteria = u.Operand;
                }
            }

            if (criteria is DevExpress.Data.Filtering.FunctionOperator)
            {
                DevExpress.Data.Filtering.FunctionOperator op = criteria as DevExpress.Data.Filtering.FunctionOperator;

                if (op.Operands.Count == 2 && op.Operands[1] is DevExpress.Data.Filtering.OperandValue)
                {
                    DevExpress.Data.Filtering.OperandValue val = op.Operands[1] as DevExpress.Data.Filtering.OperandValue;

                    if (val.Value is string)
                    {
                        val.Value = e.Value.ToUpper();
                    }
                }
            }
        }
    }

    private static void AdjustGridCookieVersion(DevExpress.Web.ASPxGridView grid, ref string cookie)
    {
        if (cookie.ToLower().StartsWith("version"))
        {
            int dividerPos = cookie.IndexOf('|');

            if (dividerPos > 0)
            {
                cookie = "version" + grid.SettingsCookies.Version + cookie.Substring(dividerPos);
            }
        }
    }

    public static void RestoreFixedColumns(ASPxGridView grid)
    {
        var cookieKey = string.Format("{0}.FixedColumns", grid.SettingsCookies.CookiesID);

        var listOfFieldsToFix = HttpContext.Current.Request.Cookies[cookieKey] != null
            ? HttpContext.Current.Request.Cookies[cookieKey].Value
            : null;

        RestoreFixedColumns(grid, listOfFieldsToFix);
    }

    public static void RestoreFixedColumns(ASPxGridView grid, string listOfFieldsToFix)
    {
        if (string.IsNullOrWhiteSpace(listOfFieldsToFix))
            return;

        foreach (GridViewDataColumn column in grid.Columns.Cast<GridViewColumn>().Where(x => x is GridViewDataColumn))
        {
            if (column.ShowInCustomizationForm)
            {
                column.FixedStyle = listOfFieldsToFix.Contains("#" + column.FieldName + "++#")
                    ? GridViewColumnFixedStyle.Left
                    : GridViewColumnFixedStyle.None;

                column.CellStyle.BackColor = listOfFieldsToFix.Contains("#" + column.FieldName + "++#")
                    ? System.Drawing.ColorTranslator.FromHtml("#ffffd6")
                    : System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            }
        }
    }



    public static void ProcessGridSortByBuildingNumber(DevExpress.Web.CustomColumnSortEventArgs e)
    {

        string value1 = e.Value1 is string ? (string)e.Value1 : "";

        string value2 = e.Value2 is string ? (string)e.Value2 : "";



        string number1 = ConvertToBuildingNumber(ref value1);

        string number2 = ConvertToBuildingNumber(ref value2);



        if (number1.Length > 0 && number2.Length > 0)
        {

            int num1 = int.Parse(number1);

            int num2 = int.Parse(number2);



            if (num1 < num2)
            {

                e.Handled = true;

                e.Result = -1;

            }

            else if (num1 > num2)
            {

                e.Handled = true;

                e.Result = 1;

            }

            else
            {

                // Building numbers are equal. Compare by string remainder

                e.Handled = true;

                e.Result = value1.CompareTo(value2);



                if (e.Result < 0)

                    e.Result = -1;



                if (e.Result > 0)

                    e.Result = 1;

            }

        }

        else if (number1.Length > 0)
        {

            e.Handled = true;

            e.Result = 1;

        }

        else if (number2.Length > 0)
        {

            e.Handled = true;

            e.Result = -1;

        }

    }



    private static string ConvertToBuildingNumber(ref string number)
    {

        int i = 0;

        string s = "";



        while (i < number.Length && char.IsWhiteSpace(number[i]))
        {

            i++;

        }



        while (i < number.Length && char.IsDigit(number[i]))
        {

            s += number[i];

            i++;

        }



        if (s.Length > 0)

            number = number.Remove(0, s.Length).Trim();



        return s;

    }

    #endregion (Common event handlers for all data grids)

    #region Grid filter formatting

    public static void UpdateFilterDisplayTextCache(this Page page, string filter,
        DevExpress.Web.ASPxGridView grid)
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
            filterCache[grid.DataSourceID] = filter;
        }
    }

    public static string GetFilterDisplayTextFromCache(this Page page,
        DevExpress.Web.ASPxGridView grid)
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

        if (filterCache != null && filterCache.ContainsKey(grid.DataSourceID))
        {
            object text = filterCache[grid.DataSourceID];

            if (text is string)
            {
                return (string)text;
            }
        }

        return "";
    }

    public static void AdjustGridColumnsForExport(DevExpress.Web.ASPxGridView grid)
    {
        double defaultColumnWidth = 80;

        // Calculate the total width of all visible columns
        double totalColumnWidth = 0;

        foreach (GridViewColumn column in grid.Columns)
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
        foreach (GridViewColumn column in grid.Columns)
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

    public static void SetGridExporterProps(DevExpress.Web.ASPxGridView grid,
        DevExpress.Web.ASPxGridViewExporter exporter,
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

    #region Custom summaries

    public static void CustomSummaryExcludeSubarenda(DevExpress.Data.CustomSummaryEventArgs e)
    {
        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
        {
            // Skip the subrent rows
            object subarenda = e.GetValue("is_subarenda_int");

            if (subarenda is int && (int)subarenda != 1)
            {
                subarenda = null;
            }

            if (subarenda == null && e.FieldValue is decimal)
            {
                e.TotalValue = (decimal)e.TotalValue + (decimal)e.FieldValue;
            }
        }
        else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
        {
            e.TotalValue = (decimal)0;
        }
    }

    private static HashSet<int> balansIDsInCustomSummary = new HashSet<int>();

    public static void CustomSummaryUniqueBalans(DevExpress.Data.CustomSummaryEventArgs e)
    {
        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
        {
            // Skip the rows for which balans_id is already processed
            object balans_id = e.GetValue("balans_id");

            if (balans_id is int)
            {
                int balansId = (int)balans_id;

                if (!balansIDsInCustomSummary.Contains(balansId))
                {
                    balansIDsInCustomSummary.Add(balansId);

                    if (e.FieldValue is decimal)
                    {
                        e.TotalValue = (decimal)e.TotalValue + (decimal)e.FieldValue;
                    }
                }
            }
        }
        else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
        {
            e.TotalValue = (decimal)0;

            balansIDsInCustomSummary.Clear();
        }
    }

    private static HashSet<string> transferKeysInCustomSummary = new HashSet<string>();

    public static void CustomSummaryUniqueObjectTransfer(DevExpress.Data.CustomSummaryEventArgs e)
    {
        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
        {
            // Skip the rows with already processed objects
            object building_id = e.GetValue("building_id");
            object akt_id = e.GetValue("akt_id");
            object name = e.GetValue("name");

            if (building_id is int && akt_id is int)
            {
                string objName = (name is string) ? ((string)name).Trim() : "null";
                string key = building_id.ToString() + "_" + akt_id.ToString() + "_" + objName;

                if (!transferKeysInCustomSummary.Contains(key))
                {
                    transferKeysInCustomSummary.Add(key);

                    if (e.FieldValue is decimal)
                    {
                        e.TotalValue = (decimal)e.TotalValue + (decimal)e.FieldValue;
                    }
                }
            }
            else
            {
                // The row is not valid; include it into the summary any way
                if (e.FieldValue is decimal)
                {
                    e.TotalValue = (decimal)e.TotalValue + (decimal)e.FieldValue;
                }
            }
        }
        else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
        {
            e.TotalValue = (decimal)0;

            transferKeysInCustomSummary.Clear();
        }
    }

    private static HashSet<string> balansOrgAndObjectKeysInCustomSummary = new HashSet<string>();

    public static void CustomSummaryUniqueBalansOrgAndObject(DevExpress.Data.CustomSummaryEventArgs e,
        string orgIdFieldName, bool excludeSubRent)
    {
        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
        {
            // Skip Sub-rent, if requested
            bool isSubRent = false;

            if (excludeSubRent)
            {
                object subarenda = e.GetValue("is_subarenda_int");

                if (subarenda is int && (int)subarenda > 0)
                {
                    isSubRent = true;
                }
            }

            if (!isSubRent)
            {
                // Skip the rows for which organization and building is already processed
                object building_id = e.GetValue("building_id");
                object org_id = e.GetValue(orgIdFieldName);

                if (building_id is int && org_id is int)
                {
                    string key = building_id.ToString() + "_" + org_id.ToString();

                    if (!balansOrgAndObjectKeysInCustomSummary.Contains(key))
                    {
                        balansOrgAndObjectKeysInCustomSummary.Add(key);

                        if (e.FieldValue is decimal)
                        {
                            e.TotalValue = (decimal)e.TotalValue + (decimal)e.FieldValue;
                        }
                    }
                }
            }
        }
        else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
        {
            e.TotalValue = (decimal)0;

            balansOrgAndObjectKeysInCustomSummary.Clear();
        }
    }



    public static void CheckCustomSummaryVisibility(ASPxGridView grid, DevExpress.Data.CustomSummaryEventArgs e)
    {

        if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start && e.IsTotalSummary)
        {

            // If the column is not visible, do not calculate the summary

            bool columnVisible = true;



            if (e.Item is ASPxSummaryItem)
            {

                string fieldName = (e.Item as ASPxSummaryItem).FieldName;



                foreach (GridViewColumn col in grid.Columns)
                {

                    if (col is GridViewDataColumn && (col as GridViewDataColumn).FieldName == fieldName)
                    {

                        columnVisible = col.Visible;

                        break;

                    }

                }

            }



            if (!columnVisible)
            {

                e.TotalValue = (decimal)0;

                e.TotalValueReady = true;

            }

        }

    }

    #endregion (Custom summaries)

    #region Custom field chooser support

    public static void GenerateFieldChooserNodes(ASPxListBox list, ASPxGridView grid, string captionPattern)
    {
        string pattern = captionPattern.Trim().ToLower();

        SortedDictionary<string, ListEditItem> sortedColumns = new SortedDictionary<string, ListEditItem>();

        foreach (GridViewDataColumn column in grid.Columns.Cast<GridViewColumn>().Where(x => x is GridViewDataColumn))
        {
            if (column.ShowInCustomizationForm && column.Caption.Length > 0 && column.FieldName.Length > 0)
            {
                // Take the caption pattern into consideration
                if (pattern.Length == 0 || (pattern.Length > 0 && column.Caption.ToLower().Contains(pattern)))
                {
                    ListEditItem item = new ListEditItem(column.Caption, column.FieldName);

                    item.Selected = column.Visible;

                    sortedColumns.Add(column.Caption, item);
                }
            }
        }

        foreach (KeyValuePair<string, ListEditItem> pair in sortedColumns)
        {
            list.Items.Add(pair.Value);
        }

        list.DataBind();
    }

    public static void GenerateFieldFixxerNodes(ASPxListBox list, ASPxGridView grid, string captionPattern)
    {
        string pattern = captionPattern.Trim().ToLower();

        SortedDictionary<string, ListEditItem> sortedColumns = new SortedDictionary<string, ListEditItem>();

        foreach (GridViewDataColumn column in grid.Columns.Cast<GridViewColumn>().Where(x => x is GridViewDataColumn))
        {
            if (column.ShowInCustomizationForm && column.Caption.Length > 0 && column.FieldName.Length > 0 && column.Visible)
            {
                // Take the caption pattern into consideration
                if (pattern.Length == 0 || (pattern.Length > 0 && column.Caption.ToLower().Contains(pattern)))
                {
                    ListEditItem item = new ListEditItem(column.Caption, column.FieldName);

                    item.Selected = column.FixedStyle == GridViewColumnFixedStyle.Left;

                    sortedColumns.Add(column.Caption, item);
                }
            }
        }

        foreach (KeyValuePair<string, ListEditItem> pair in sortedColumns)
        {
            list.Items.Add(pair.Value);
        }

        list.DataBind();
    }

    #endregion (Custom field chooser support)

    #region Session management

    public static string GridLayoutToLoad
    {
        get
        {
            object layout = HttpContext.Current.Session["GridLayoutToLoad"];

            if (layout is string)
            {
                return (string)layout;
            }

            return "";
        }

        set
        {
            HttpContext.Current.Session["GridLayoutToLoad"] = value;
        }
    }

    public static string GridPreFilterToLoad
    {
        get
        {
            object preFilter = HttpContext.Current.Session["GridPreFilterToLoad"];

            if (preFilter is string)
            {
                return (string)preFilter;
            }

            return "";
        }

        set
        {
            HttpContext.Current.Session["GridPreFilterToLoad"] = value;
        }
    }

    public static int GridIdToLoad
    {
        get
        {
            object gridId = HttpContext.Current.Session["GridIdToLoad"];

            if (gridId is int)
            {
                return (int)gridId;
            }

            return -1;
        }

        set
        {
            HttpContext.Current.Session["GridIdToLoad"] = value;
        }
    }

    #endregion (Session management)

    #region Rishennya Project Utility code

    //public static string EncodeZipEntryName(this string entryName)
    //{
    //    Encoding iso = Encoding.GetEncoding("ISO-8859-1");
    //    byte[] utfBytes = Encoding.UTF8.GetBytes(entryName);
    //    byte[] isoBytes = Encoding.Convert(Encoding.UTF8, iso, utfBytes);
    //    return iso.GetString(isoBytes);
    //}

    public static void RishProjectDelete(int projectId, SqlConnection connection)
    {
        bool ownConnection = false;

        if (connection == null)
        {
            connection = Utils.ConnectToDatabase();
            ownConnection = true;
        }

        if (connection != null)
        {
            using (DisposableScope s = new DisposableScope())
            {
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // Delete all entries from external_document table that are associated with any
                    // project, appendix or item records.
                    // ---
                    // Generally speaking, I would like to use RDBMS built-in capabilities to propagate
                    // ID changes, but SQL Server, as always, complains about "multiple update paths" 
                    // or somesuch asinine nonsense. Shame, M$, SHAME!
                    List<int> externalDocumentIDs = new List<int>();

                    // Step 1: identify all external_document.id values that are referenced by
                    //         records from bp_rish_project table that belong to the scope of
                    //         the project being deleted (it means appendixes, too).
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = "SELECT external_document_id FROM bp_rish_project WHERE (id = @projectID OR app_for_project_id = @projectID) AND external_document_id IS NOT NULL";
                        command.Parameters.Add(new SqlParameter("projectID", projectId));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                externalDocumentIDs.Add((int)reader[0]);
                            }
                        }
                    }
                    // Step 2: disentangle bp_rish_project from external_document by setting external_document_id
                    //         to NULL.
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = "UPDATE bp_rish_project SET external_document_id = NULL WHERE (id = @projectID OR app_for_project_id = @projectID) AND external_document_id IS NOT NULL";
                        command.Parameters.Add(new SqlParameter("projectID", projectId));

                        command.ExecuteNonQuery();
                    }
                    // Step 3: identify all external_document.id values that are referenced by
                    //         records from bp_rish_project_item table that belong to the scope of
                    //         the project being deleted (it means appendixes, too).
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = "SELECT external_document_id FROM bp_rish_project_item WHERE (project_id = @projectID OR project_id IN (SELECT id FROM bp_rish_project WHERE app_for_project_id = @projectID)) AND external_document_id IS NOT NULL";
                        command.Parameters.Add(new SqlParameter("projectID", projectId));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                externalDocumentIDs.Add((int)reader[0]);
                            }
                        }
                    }
                    // Step 4: disentangle bp_rish_project_item from external_document by setting 
                    //         external_document_id to NULL.
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = "UPDATE bp_rish_project_item SET external_document_id = NULL WHERE (project_id = @projectID OR project_id IN (SELECT id FROM bp_rish_project WHERE app_for_project_id = @projectID)) AND external_document_id IS NOT NULL";
                        command.Parameters.Add(new SqlParameter("projectID", projectId));

                        command.ExecuteNonQuery();
                    }
                    // Step 5: not only the records from external_document must be disposed of, but
                    //         the physical files as well. We are going to take care of the files right
                    //         after the database transaction commit completes.
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = string.Format("SELECT unique_filename FROM external_document WHERE id IN (SELECT STRINGS FROM dbo.fnTokenizeStringList(@ids, ','))");
                        command.Parameters.AddWithValue("@ids", string.Join(",", externalDocumentIDs.Select(x => x.ToString()).ToArray()));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fileName = (string)reader[0];

                                s.OnCommit += delegate() { try { File.Delete(fileName); } catch { } };
                            }
                        }
                    }
                    // Step 6: finally dispose of the [no longer referenced] external_document records.
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = string.Format("DELETE FROM external_document WHERE id IN (SELECT STRINGS FROM dbo.fnTokenizeStringList(@ids, ','))");
                        command.Parameters.AddWithValue("@ids", string.Join(",", externalDocumentIDs.Select(x => x.ToString()).ToArray()));

                        command.ExecuteNonQuery();
                    }

                    // Get all the appendices of this project
                    List<int> appendices = new List<int>();

                    using (SqlCommand command = new SqlCommand("SELECT id FROM bp_rish_project WHERE app_for_project_id = @projid", connection, transaction))
                    {
                        command.Parameters.Add(new SqlParameter("projid", projectId));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                appendices.Add(reader.GetInt32(0));
                            }

                            reader.Close();
                        }
                    }

                    // Delete all appendices
                    foreach (int appendixId in appendices)
                    {
                        RishProjectDeleteAppendix(appendixId, connection, transaction);
                    }

                    // Delete the additional information related to the Project
                    using (SqlCommand command = new SqlCommand("DELETE FROM bp_rish_project_info WHERE project_id = @projid", connection, transaction))
                    {
                        command.Parameters.Add(new SqlParameter("projid", projectId));
                        command.ExecuteNonQuery();
                    }

                    // Delete the Project, just like appendix
                    RishProjectDeleteAppendix(projectId, connection, transaction);

                    transaction.Commit();

                    s.Commit();
                }
            }

            if (ownConnection)
            {
                connection.Close();
            }
        }
    }

    private static void RishProjectDeleteAppendix(int appendixId, SqlConnection connection, SqlTransaction transaction)
    {
        // Delete the history related to the Appendix
        using (SqlCommand command = new SqlCommand("DELETE FROM bp_rish_project_history WHERE project_id = @projid", connection, transaction))
        {
            command.Parameters.Add(new SqlParameter("projid", appendixId));
            command.ExecuteNonQuery();
        }

        // Delete all signatures of this Appendix
        using (SqlCommand command = new SqlCommand("DELETE FROM bp_rish_project_signature WHERE project_id = @projid", connection, transaction))
        {
            command.Parameters.Add(new SqlParameter("projid", appendixId));
            command.ExecuteNonQuery();
        }

        // Delete all punkts of this Appendix
        using (SqlCommand command = new SqlCommand("DELETE FROM bp_rish_project_item WHERE project_id = @projid", connection, transaction))
        {
            command.Parameters.Add(new SqlParameter("projid", appendixId));
            command.ExecuteNonQuery();
        }

        // Delete the appendix itself
        using (SqlCommand command = new SqlCommand("DELETE FROM bp_rish_project WHERE id = @projid", connection, transaction))
        {
            command.Parameters.Add(new SqlParameter("projid", appendixId));
            command.ExecuteNonQuery();
        }
    }

    public static string ExtractTextFromHtml(string html)
    {
        if (html.IndexOf('<') < 0)
        {
            return html;
        }

        try
        {
            //ParsedHtml parsedHtml = new ParsedHtml();
            //parsedHtml.Process(html);
            //return parsedHtml.Document.ToString();
            if (html == null)
            {
                throw new ArgumentNullException("html");
            }

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var chunks = new List<string>();

            foreach (var item in doc.DocumentNode.DescendantNodesAndSelf())
            {
                if (item.NodeType == HtmlAgilityPack.HtmlNodeType.Text)
                {
                    if (item.InnerText.Trim() != "")
                    {
                        chunks.Add(item.InnerText.Trim());
                    }
                }
            }
            return String.Join(" ", chunks);
        }
        catch (Exception)
        {
        }

        return html;
    }

    public static void HideUnnecessaryHtmlEditorButtons(ASPxHtmlEditor editor, bool flattenToolbars = false)
    {

        if (editor.Toolbars.Count == 0)
        {
            editor.CreateDefaultToolbars(false);

            for (int n = 0; n < editor.Toolbars.Count; n++)
            {
                HtmlEditorToolbar toolbar = editor.Toolbars[n];

                for (int i = 0; i < toolbar.Items.Count; i++)
                {
                    HtmlEditorToolbarItem item = toolbar.Items[i];

                    if (item is ToolbarCheckSpellingButton
                        || item is ToolbarParagraphFormattingEdit
                        || item is ToolbarInsertOrderedListButton
                        || item is ToolbarInsertUnorderedListButton
                        || item is ToolbarIndentButton
                        || item is ToolbarOutdentButton
                        || item is ToolbarInsertLinkDialogButton
                        || item is ToolbarUnlinkButton
                        || item is ToolbarInsertImageDialogButton
                        || item is ToolbarSubscriptButton
                        || item is ToolbarSuperscriptButton
                        || item is ToolbarPasteFromWordButton
                        || item is ToolbarCopyButton

                        || item is ToolbarPasteButton

                        || item is ToolbarCutButton)
                    {
                        item.Visible = false;
                    }
                }

                // now we're a going to add justify full button
                for (int i = 0; i < toolbar.Items.Count; i++)
                {
                    HtmlEditorToolbarItem item = toolbar.Items[i];
                    if (item is ToolbarJustifyRightButton)
                    {
                        toolbar.Items.Insert(i + 1, new ToolbarJustifyFullButton());
                        break;
                    }
                }
            }

            if (flattenToolbars)
            {
                editor.Toolbars[0].Items.AddRange(editor.Toolbars[1].Items);
                editor.Toolbars.RemoveAt(1);
            }
        }

        editor.ClientSideEvents.HtmlChanged = @"function (s, e) {
    var origValue = s.GetHtml();
    var cleanValue = origValue.replace(/<!--[\s\S]*?-->/gm, '').trim();
    if (origValue != cleanValue) {
        s.SetHtml(cleanValue);
    }
}";
    }

    /// <summary>
    /// Converts the specified string to a decimal number. This function tolerates
    /// both '.' and ',' separators for the non-integer numbers, and ignores all
    /// non-digits present in the string.
    /// </summary>
    /// <param name="value">A decimal number represented as a string</param>
    /// <returns>The extracted decimal number. If the provided string could not be parsed,
    /// this method returns 0.</returns>
    public static decimal ConvertStrToDecimal(string value)
    {
        return ConvertStrToDecimal(value, 0m);
    }

    /// <summary>
    /// Converts the specified string to a decimal number. This function tolerates
    /// both '.' and ',' separators for the non-integer numbers, and ignores all
    /// non-digits present in the string.
    /// </summary>
    /// <param name="value">A decimal number represented as a string</param>
    /// <param name="defaultValue">The value that must be returned when the input string could not be parsed</param>
    /// <returns>The extracted decimal number, or the default value.</returns>
    public static decimal ConvertStrToDecimal(string value, decimal defaultValue)
    {
        // Remove all unnecessary characters; leave just the number
        string strippedValue = "";
        bool commaFound = false;
        bool dotFound = false;

        for (int i = 0; i < value.Length; i++)
        {
            if (char.IsDigit(value[i]))
            {
                strippedValue += value[i];
            }
            else if (value[i] == ',')
            {
                if (!dotFound && !commaFound)
                {
                    strippedValue += ',';
                    commaFound = true;
                }
            }
            else if (value[i] == '.')
            {
                if (!dotFound && !commaFound)
                {
                    strippedValue += '.';
                    dotFound = true;
                }
            }
        }

        try
        {
            decimal d = decimal.Parse(strippedValue);
        }
        catch (Exception)
        {
            if (dotFound)
            {
                strippedValue = strippedValue.Replace('.', ',');
            }
            else if (commaFound)
            {
                strippedValue = strippedValue.Replace(',', '.');
            }
        }

        try
        {
            return decimal.Parse(strippedValue);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Converts the specified string to an integer number. This function ignores all
    /// non-digits present in the string.
    /// </summary>
    /// <param name="value">An integer number represented as a string</param>
    /// <returns>The extracted integer number. If the provided string could not be parsed,
    /// this method returns 0.</returns>
    public static int ConvertStrToInt(string value)
    {
        return ConvertStrToInt(value, 0);
    }

    /// <summary>
    /// Converts the specified string to an integer number. This function ignores all
    /// non-digits present in the string.
    /// </summary>
    /// <param name="value">An integer number represented as a string</param>
    /// <param name="defaultValue">The value that must be returned when the input string could not be parsed</param>
    /// <returns>The extracted integer number, or the default value.</returns>
    public static int ConvertStrToInt(string value, int defaultValue)
    {
        // Remove all unnecessary characters; leave just the number
        string strippedValue = "";

        for (int i = 0; i < value.Length; i++)
        {
            if (char.IsDigit(value[i]))
            {
                strippedValue += value[i];
            }
        }

        try
        {
            return int.Parse(strippedValue);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Describes a function that is called by the ProcessHyperlinks method to
    /// process all hyperlinks in the provided HTML markup.
    /// </summary>
    /// <param name="openingTag">The opening tag of the hyperlink</param>
    /// <param name="innerText">Inner text of the hyperlink</param>
    /// <param name="closingTag">The closing tag of the hyperlink</param>
    /// <returns>TRUE if the hyperlink should be preserved, or FALSE if the hyperlink must be erased from the HTML markup</returns>
    public delegate bool ProcessHyperlinkDelegate(ref string openingTag, ref string innerText, ref string closingTag, object parameter);

    /// <summary>
    /// Extracts all HTML hyperlinks from the given HTML markup, and
    /// allows the caller to invoke particular function for each hyperlink.
    /// </summary>
    /// <param name="html">The input HTML markup</param>
    /// <param name="handler">The delegate that must be invoked for each hyperlink</param>
    /// <param name="parameter">A custom parameter that should be passed to the delegate function</param>
    public static void ProcessHyperlinks(ref string html, ProcessHyperlinkDelegate handler, object parameter)
    {
        string oldHtml = html;
        string newHtml = "";

        int aPos = oldHtml.IndexOf("<a");

        while (aPos >= 0)
        {
            // Move everything before the <a> tag to the output stream
            newHtml += oldHtml.Substring(0, aPos);
            oldHtml = oldHtml.Remove(0, aPos);

            string openingTag = "";
            string innerText = "";
            string closingTag = "";

            ExtractLinkTag(ref oldHtml, out openingTag, out innerText, out closingTag);

            // Allow the caller to modify the hyperlink
            bool preserveLink = handler(ref openingTag, ref innerText, ref closingTag, parameter);

            // Copy the passed text to the result
            if (preserveLink)
            {
                newHtml += openingTag;
                newHtml += innerText;
                newHtml += closingTag;
            }

            // Search for the next hyperlink
            aPos = oldHtml.IndexOf("<a");
        }

        // Do not leave the remainder of the initial HTML
        newHtml += oldHtml;

        // Return the result to the caller
        html = newHtml;
    }

    private static void ExtractLinkTag(ref string html, out string openingTag, out string innerText, out string closingTag)
    {
        openingTag = "";
        innerText = "";
        closingTag = "";

        // All characters before the > sign comprise the opening <a> tag
        int pos = 0;

        while (pos < html.Length && html[pos] != '>')
        {
            openingTag += html[pos];
            pos++;
        }

        if (pos < html.Length)
        {
            openingTag += '>';
            pos++;

            // All characters before the '<' sign comprise the inner text of the <a> ... </a> tag
            while (pos < html.Length && html[pos] != '<')
            {
                innerText += html[pos];
                pos++;
            }

            if (pos < html.Length)
            {
                // All characters before the > sign comprise the closing </a> tag
                while (pos < html.Length && html[pos] != '>')
                {
                    closingTag += html[pos];
                    pos++;
                }

                if (pos < html.Length)
                {
                    closingTag += '>';
                    pos++;
                }
            }
        }

        // Remove the extracted portion from the html text
        html = html.Remove(0, pos);
    }

    public static string GetRelAttributeFromHyperlink(string hyperlinkTag)
    {
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(
            @"rel\s*=\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        System.Text.RegularExpressions.Match match = regex.Match(hyperlinkTag);

        if (match.Success)
        {
            // Extract all non-whitespace characters
            int pos = match.Index + match.Length + 1;
            string relValue = "";

            if (pos < hyperlinkTag.Length && hyperlinkTag[pos] == '"')
            {
                pos++;
            }

            while (pos < hyperlinkTag.Length && !char.IsWhiteSpace(hyperlinkTag[pos]) && hyperlinkTag[pos] != '"' && hyperlinkTag[pos] != '>')
            {
                relValue += hyperlinkTag[pos];
                pos++;
            }

            if (relValue.StartsWith("\""))
            {
                relValue = relValue.Substring(1);
            }

            if (relValue.EndsWith("\""))
            {
                relValue = relValue.Substring(0, relValue.Length - 1);
            }

            return relValue;
        }

        return "";
    }

    #endregion (Rishennya Project Utility code)

    #region Email notifications

    /// <summary>
    /// Sends an mail message
    /// </summary>
    /// <param name="from">Sender address</param>
    /// <param name="to">Recepient address</param>
    /// <param name="cc">Cc recepient</param>
    /// <param name="bcc">Bcc recepient</param>
    /// <param name="subject">Subject of mail message</param>
    /// <param name="body">Body of mail message</param>
    //public static void SendMailMessage(string from, string to, string cc, string bcc, string subject, string body)
    //{
    //    body = body.Replace("\r", "");
    //    body = body.Replace("\n", "<br/>");

    //    // Instantiate a new instance of MailMessage
    //    MailMessage mMailMessage = new MailMessage();
    //    mMailMessage.IsBodyHtml = true;

    //    // Set the sender address of the mail message
    //    if (from != null)
    //        mMailMessage.From = new MailAddress(from);
    //    // Set the recepient address of the mail message
    //    mMailMessage.To.Add(new MailAddress(to));

    //    // Check if the bcc value is null or an empty string
    //    if ((bcc != null) && (bcc != string.Empty))
    //    {
    //        // Set the Bcc address of the mail message
    //        mMailMessage.Bcc.Add(new MailAddress(bcc));
    //    }

    //    // Check if the cc value is null or an empty value
    //    if ((cc != null) && (cc != string.Empty))
    //    {
    //        // Set the CC address of the mail message
    //        mMailMessage.CC.Add(new MailAddress(cc));
    //    }

    //    // Set the subject of the mail message
    //    mMailMessage.Subject = subject;
    //    // Set the body of the mail message
    //    mMailMessage.Body = body;

    //    // Set the priority of the mail message to normal
    //    mMailMessage.Priority = MailPriority.Normal;

    //    // Instantiate a new instance of SmtpClient
    //    SmtpClient mSmtpClient = new SmtpClient();
    //    // Send the mail message
    //    mSmtpClient.Send(mMailMessage);
    //}

    #endregion (Email notifications)

    #region JSON



    public static string EscapeJSONParam(this string text)
    {

        if (text == null)

            return text;

        return text.Replace("\\", "\\\\").Replace("\"", "\\\"");

    }



    public static string ToJSON(this object graph)
    {

        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

        using (StringWriter writer = new StringWriter())
        {

            serializer.Serialize(writer, graph);

            return writer.ToString();

        }

    }



    public static T FromJSON<T>(this string text)
    {

        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

        using (StringReader reader = new StringReader(text))
        {

            using (Newtonsoft.Json.JsonTextReader jsonReader = new Newtonsoft.Json.JsonTextReader(reader))
            {

                return serializer.Deserialize<T>(jsonReader);

            }

        }

    }



    public static string ToXML(this object graph)
    {

        if (graph == null)

            return null;



        using (StringWriter writer = new StringWriter())
        {

            System.Xml.Serialization.XmlSerializer serializer =

                new System.Xml.Serialization.XmlSerializer(graph.GetType());

            serializer.Serialize(writer, graph);

            return writer.ToString();

        }

    }



    public static T FromXML<T>(this string xml)
    {

        if (xml == null)

            return default(T);



        using (StringReader reader = new StringReader(xml))
        {

            System.Xml.Serialization.XmlSerializer serializer =

                new System.Xml.Serialization.XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(reader);

        }

    }



    #endregion (JSON)

    #region Authentication

    public static int UserOrganizationID
    {
        get
        {
            object orgId = HttpContext.Current.Session["USER_ORGANIZATION_1NF"];

            if (orgId is int)
            {
                return (int)orgId;
            }

            return 0;
        }

        set
        {
            HttpContext.Current.Session["USER_ORGANIZATION_1NF"] = value;
        }
    }

    public static int RdaDistrictID
    {
        get
        {
            object districtId = HttpContext.Current.Session["RDA_DISTRICT_ID_1NF"];

            if (districtId is int)
            {
                return (int)districtId;
            }

            return 0;
        }

        set
        {
            HttpContext.Current.Session["RDA_DISTRICT_ID_1NF"] = value;
        }
    }


	public static string WebsiteBaseUrl
	{
		get
		{
			return WebConfigurationManager.AppSettings["WebsiteBaseUrl"];
		}
	}

	public static int GetUserOrganizationID(string username)
    {
        int org_id = 0;
        if (!string.IsNullOrEmpty(username))
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                string query = @"select r.organization_id from aspnet_Users u
                                left join reports1nf_accounts r
                                on u.UserId = r.UserId
                                where UserName = @usrname";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("usrname", username));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            org_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }
        }

        return org_id;
    }

    public static int GetOrganizationIdByReportId(int reportId)
    {
        int org_id = 0;

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string query = @"select organization_id from reports1nf where id = @report_id";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("report_id", reportId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        org_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }

        return org_id;
    }

    public static int GetReportIdByOrganizationId(int organizationId)
    {
        int rep_id = 0;

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string query = @"select id  from reports1nf where organization_id = @organization_id";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("organization_id", organizationId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rep_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }

        return rep_id;
    }

    public static void FindUserOrganizationAndRda(string userName)
    {
        //if (UserOrganizationID <= 0)
        {
            if (string.IsNullOrEmpty(userName))
            {
                MembershipUser user = Membership.GetUser();

                if (user != null)
                    userName = user.UserName;
            }

            if (!string.IsNullOrEmpty(userName))
            {
                userName = userName.ToLower();
                SqlConnection connection = Utils.ConnectToDatabase();

                if (connection != null)
                {
                    // Get the 1NF organization related to this user
                    int organizationId = -1;
                    int rdaDistrictId = -1;

                    string query = @"SELECT acc.organization_id, acc.rda_district_id
                        FROM reports1nf_accounts acc INNER JOIN aspnet_Users usr ON usr.UserId = acc.UserId
                        WHERE RTRIM(LTRIM(usr.LoweredUserName)) = @usrname";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("usrname", userName));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                organizationId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                rdaDistrictId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            }

                            reader.Close();
                        }
                    }

                    if (organizationId > 0)
                    {
                        UserOrganizationID = organizationId;
                    }

                    if (rdaDistrictId > 0)
                    {
                        RdaDistrictID = rdaDistrictId;
                    }

                    connection.Close();
                }
            }
        }
    }

    public static int GetUserMistoId(string userName)
    {
        int misto_district_id = -1;
        //if (UserOrganizationID <= 0)
        {
            if (string.IsNullOrEmpty(userName))
            {
                MembershipUser user = Membership.GetUser();

                if (user != null)
                    userName = user.UserName;
            }

            if (!string.IsNullOrEmpty(userName))
            {
                userName = userName.ToLower();
                SqlConnection connection = Utils.ConnectToDatabase();

                if (connection != null)
                {
                    string query = @"SELECT acc.organization_id, acc.misto_district_id
                        FROM reports1nf_accounts acc INNER JOIN aspnet_Users usr ON usr.UserId = acc.UserId
                        WHERE RTRIM(LTRIM(usr.LoweredUserName)) = @usrname";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("usrname", userName));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                misto_district_id = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            }

                            reader.Close();
                        }
                    }

                    connection.Close();
                }
            }
        }
        return misto_district_id;
    }


    #endregion (Authentication)

    /// <summary>
    /// Allocate a new Data Adapter and associate it with the transaction.
    /// </summary>
    public static T NewAdapter<T>(this SqlTransaction transaction, Action<T> init = null) where T : new()
    {
        T adapter = new T();

        typeof(T).GetProperty("Connection").SetValue(adapter, transaction.Connection, new object[0]);

        PropertyInfo propertyCommandCollection =
            typeof(T).GetProperty("CommandCollection", BindingFlags.Instance | BindingFlags.NonPublic);
        if (propertyCommandCollection != null)
        {
            SqlCommand[] commands = (SqlCommand[])propertyCommandCollection.GetValue(adapter, new object[0]);
            if (commands != null && commands.Length > 0)
            {
                foreach (SqlCommand command in commands)
                {
                    command.Connection = transaction.Connection;
                    command.Transaction = transaction;
                }
            }
        }

        SqlDataAdapter underlyingAdapter = (SqlDataAdapter)typeof(T)
            .GetProperty("Adapter", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(adapter, new object[0]);

        if (underlyingAdapter.SelectCommand != null)
            underlyingAdapter.SelectCommand.Transaction = transaction;
        if (underlyingAdapter.InsertCommand != null)
            underlyingAdapter.InsertCommand.Transaction = transaction;
        if (underlyingAdapter.UpdateCommand != null)
            underlyingAdapter.UpdateCommand.Transaction = transaction;
        if (underlyingAdapter.DeleteCommand != null)
            underlyingAdapter.DeleteCommand.Transaction = transaction;

        if (init != null)
            init(adapter);

        return adapter;
    }

    /// <summary>
    /// Adds a new parameter with the specified name to the parameter collection. If the specified
    /// value equals the specified default value, the parameter will be added with the null value.
    /// </summary>
    public static void AddWithValue<T>(this FbParameterCollection parameters, string parameterName,
        T value, T defaultValue)
    {
        if (object.Equals(value, defaultValue))
            parameters.AddWithValue(parameterName, null);
        else
            parameters.AddWithValue(parameterName, value);
    }

    public static IEnumerable<Control> FindParents(this Control control)
    {
        for (Control parent = control.Parent; parent != null; parent = parent.Parent)
            yield return parent;
    }

    /// <summary>
    /// recursively finds a child control of the specified parent.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Control FindControlRecursive(this Control control, string id)
    {
        if (control == null) return null;

        //try to find the control at the current level
        Control ctrl = control.FindControl(id);

        if (ctrl == null)
        {
            //search the children
            foreach (Control child in control.Controls)
            {
                ctrl = FindControlRecursive(child, id);
                if (ctrl != null) break;
            }
        }

        return ctrl;
    }

    public static void ReloadCacheForBalans()
    {
        DataSourceCache.RemoveCacheEntries(
            x => Regex.IsMatch(x.Query, @"\b(view_)?balans(_all)?\b", RegexOptions.IgnoreCase));
    }

    public static SqlConnection ConnectToDatabase()
    {
        return CommonUtils.ConnectToDatabase();
    }

	public static string GetUser()
	{
		var user = Membership.GetUser();
		var username = (user == null ? "System" : user.UserName);
		return username;
	}

	public static bool IsBigBossUser()
	{
		var user = Membership.GetUser();
		var connection = Utils.ConnectToDatabase();

		var isBigBossUser = false;

		using (SqlCommand cmd = new SqlCommand("select m.IsBigBossUser from aspnet_Users u join aspnet_Membership m on m.UserId = u.UserId where u.UserName = @UserName and m.IsBigBossUser = 1", connection))
		{
			cmd.Parameters.AddWithValue("UserName", user.UserName);
			using (SqlDataReader r = cmd.ExecuteReader())
			{
				while (r.Read())
				{
					isBigBossUser = true;
				}
				r.Close();
			}
		}


		connection.Close();
		connection.Dispose();

		return isBigBossUser;
	}

	public static string IsBigBossUserToString()
	{
		return IsBigBossUser().ToString().ToLower();
	}

	public static void SetGridViewColumnCssClass(this GridViewColumn col, string cssClass)
	{
		col.HeaderStyle.CssClass = cssClass;
		col.CellStyle.CssClass = cssClass;
		col.FooterCellStyle.CssClass = cssClass;
		col.GroupFooterCellStyle.CssClass = cssClass;
		if (col is GridViewDataColumn)
		{
			(col as GridViewDataColumn).EditCellStyle.CssClass = cssClass;
			(col as GridViewDataColumn).FilterCellStyle.CssClass = cssClass;
		}
	}

}
