using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;

public partial class Reports1NF_ReportErrors : System.Web.UI.Page
{
    public int[] criticalErrors = new int[] { 101, 102, 201, 203, 301, 401 }; // Errors with such codes can not be overridden

    public int ColumnErrorID = 0;
    public int ColumnErrorType = 1;
    public int ColumnErrorObjDescription = 2;
    public int ColumnErrorMessage = 3;
    public int ColumnErrorOverridden = 4;
    public int ColumnErrorKind = 5;
    public int ColumnErrorIsCritical = 6;
    public int ColumnErrorModifiedBy = 7;
    public int ColumnErrorModifyDate = 8;

    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];

        // Check if the user is logged in
        if (Membership.GetUser() == null)
        {
            FormsAuthentication.RedirectToLoginPage();
        }

        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // Generate a data source for the grid
        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            GetErrorsDataSource(int.Parse(reportIdStr));
        }

        // Set the grid data source
        GridViewErrors.DataSource = GetErrorsDataSource(-1);

        // Set SQL data source parameters
        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            SqlDataSourceErrors.SelectParameters["rid"].DefaultValue = reportIdStr.Trim();
            SqlDataSourceReportDetails.SelectParameters["rid"].DefaultValue = reportIdStr.Trim();
            // SqlDataSourceNumOverriddenErrors.SelectParameters["rid"].DefaultValue = reportIdStr.Trim();
        }
    }

    protected string GetPageUniqueKey()
    {
        object key = ViewState["PageUniqueKey"];

        if (key is string)
        {
            return (string)key;
        }

        // Generate unique key
        Guid guid = Guid.NewGuid();

        string str = guid.ToString();

        ViewState["PageUniqueKey"] = str;

        return str;
    }

    protected void chk_Init(object sender, EventArgs e)
    {
        ASPxCheckBox cb = sender as ASPxCheckBox;
        GridViewDataItemTemplateContainer container = cb.NamingContainer as GridViewDataItemTemplateContainer;
        cb.ID = "cb" + container.KeyValue.ToString();

        // Set the checked/unchecked value from the current state
        int errorId = int.Parse(container.KeyValue.ToString());
        DataTable t = GetErrorsDataSource(-1);

        if (t != null)
        {
            DataRow row = t.Rows.Find(errorId);

            if (row != null)
            {
                object[] rowValues = row.ItemArray;

                if (rowValues[ColumnErrorOverridden] is int && rowValues[ColumnErrorKind] is int)
                {
                    int isOverridden = (int)rowValues[ColumnErrorOverridden];
                    int errorKind = (int)rowValues[ColumnErrorKind];

                    // Do not allow to override critical errors
                    if (IsCriticalError(errorKind))
                    {
                        cb.Enabled = false;
                        cb.ClientEnabled = false;
                    }
                    else
                    {
                        cb.Checked = (isOverridden == 1);
                    }
                }
            }
        }
    }

    protected void chk_Checked(object sender, EventArgs e)
    {
        ASPxCheckBox cb = sender as ASPxCheckBox;

        int errorId = int.Parse(cb.ID.Substring(2));
        DataTable t = GetErrorsDataSource(-1);

        if (errorId > 0 && t != null)
        {
            DataRow row = t.Rows.Find(errorId);

            if (row != null)
            {
                object[] rowValues = row.ItemArray;

                // Do not allow to override critical errors
                if (!IsCriticalError((int)rowValues[ColumnErrorKind]))
                {
                    rowValues[ColumnErrorOverridden] = cb.Checked ? 1 : 0;

                    row.ItemArray = rowValues;
                }
            }
        }
    }

    protected void GridViewErrors_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;

        bool allowAll = param.StartsWith("allowall:");
        bool denyAll = param.StartsWith("denyall:");

        if (allowAll || denyAll)
        {
            // Mark all errors in the data source as overridden (or not)
            DataTable t = GetErrorsDataSource(-1);

            foreach (DataRow row in t.Rows)
            {
                object[] rowValues = row.ItemArray;

                // Do not allow to override critical errors
                if (!IsCriticalError((int)rowValues[ColumnErrorKind]))
                {
                    rowValues[ColumnErrorOverridden] = allowAll ? 1 : 0;

                    row.ItemArray = rowValues;
                }
            }

            GridViewErrors.DataSource = t;
            GridViewErrors.DataBind();
        }
    }

    protected void ButtonSaveChanges_Click(object sender, EventArgs e)
    {
        DataTable t = GetErrorsDataSource(-1);
        object objInitialState = Session[GetPageUniqueKey() + "_InitialState"];

        if (t != null && objInitialState is Dictionary<int, int>)
        {
            Dictionary<int, int> initialState = objInitialState as Dictionary<int, int>;
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // Get name of the current user
                string userName = "???";
                MembershipUser user = Membership.GetUser();

                if (user != null)
                {
                    userName = user.UserName;
                }

                // Save "overridden" status for all errors to the database
                bool dataModified = false;

                foreach (DataRow row in t.Rows)
                {
                    object[] rowValues = row.ItemArray;

                    if (rowValues[ColumnErrorID] is int &&
                        rowValues[ColumnErrorOverridden] is int &&
                        rowValues[ColumnErrorKind] is int)
                    {
                        int errorId = (int)rowValues[ColumnErrorID];
                        int isOverridden = (int)rowValues[ColumnErrorOverridden];
                        int errorKind = (int)rowValues[ColumnErrorKind];

                        // Check if state for this error was changed
                        bool errorStateChanged = true;

                        if (initialState.ContainsKey(errorId))
                        {
                            errorStateChanged = initialState[errorId] != isOverridden;
                        }

                        // Do not allow to override critical errors
                        if (!IsCriticalError(errorKind) && errorStateChanged)
                        {
                            using (SqlCommand cmd = new SqlCommand("UPDATE validation_errors SET is_overridden = @ov, modified_by = @mby, modify_date = @mdt WHERE id = @errid", connection))
                            {
                                cmd.Parameters.Add(new SqlParameter("ov", isOverridden));
                                cmd.Parameters.Add(new SqlParameter("errid", errorId));
                                cmd.Parameters.Add(new SqlParameter("mby", userName));
                                cmd.Parameters.Add(new SqlParameter("mdt", DateTime.Now));

                                cmd.ExecuteNonQuery();
                                dataModified = true;

                                // Modify the data in local data source
                                rowValues[ColumnErrorModifiedBy] = userName;
                                rowValues[ColumnErrorModifyDate] = DateTime.Now;

                                row.ItemArray = rowValues;
                            }
                        }
                    }
                }

                if (dataModified)
                {
                    // Update the state of report, to make sure that it will be validated again
                    int reportId = int.Parse(SqlDataSourceReportDetails.SelectParameters["rid"].DefaultValue);

                    if (reportId > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE reports SET validation_status = 0, modified_by = @mby, modify_date = @mdt WHERE id = @rid", connection))
                        {
                            cmd.Parameters.Add(new SqlParameter("rid", reportId));
                            cmd.Parameters.Add(new SqlParameter("mby", userName));
                            cmd.Parameters.Add(new SqlParameter("mdt", DateTime.Now));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                connection.Close();

                GridViewErrors.DataSource = t;
                GridViewErrors.DataBind();
            }
        }

        ViewReportDetails.DataBind();
    }

    private DataTable GetErrorsDataSource(int reportId)
    {
        string keyDataSource = GetPageUniqueKey() + "_ErrorTable";
        string keyInitialState = GetPageUniqueKey() + "_InitialState";

        object table = Session[keyDataSource];

        if (table is DataTable)
        {
            return table as DataTable;
        }

        // If we can create a data source, do it
        SqlConnection connection = Utils.ConnectToDatabase();

        if (reportId > 0 && connection != null)
        {
            // Create a new data table, if not found in the Session
            DataTable t = new DataTable("Errors");

            t.Columns.Add("id", typeof(int)); // ColumnErrorID
            t.Columns.Add("error_kind_descr", typeof(string)); // ColumnErrorType
            t.Columns.Add("object_descr", typeof(string)); // ColumnErrorObjDescription
            t.Columns.Add("error_msg", typeof(string)); // ColumnErrorMessage
            t.Columns.Add("is_overridden", typeof(int)); // ColumnErrorOverridden
            t.Columns.Add("error_kind", typeof(int)); // ColumnErrorKind
            t.Columns.Add("is_critical", typeof(int)); // ColumnErrorIsCritical
            t.Columns.Add("modified_by", typeof(string)); // ColumnErrorModifiedBy
            t.Columns.Add("modify_date", typeof(DateTime)); // ColumnErrorModifyDate

            // Create a primary key in the table
            DataColumn[] keys = new DataColumn[1];
            keys[0] = t.Columns[0];

            t.PrimaryKey = keys;

            // Fill the data source from SQL Server
            Dictionary<int, int> initialState = new Dictionary<int, int>();

            using (SqlCommand cmd = new SqlCommand(
                "SELECT id, error_kind_descr, object_descr, error_msg, is_overridden, error_kind, is_critical, modified_by, modify_date" +
                " FROM view_validation_errors WHERE report_id = @rid ORDER BY is_critical ASC, error_kind",
                connection))
            {
                cmd.Parameters.Add(new SqlParameter("rid", reportId));

                object[] dataRow = new object[t.Columns.Count];

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add the row to the data table
                        for (int i = 0; i < dataRow.Length; i++)
                        {
                            dataRow[i] = reader.GetValue(i);
                        }

                        t.Rows.Add(dataRow);

                        // Save the initial state of this error
                        object dataErrorId = reader.GetValue(0);
                        object dataIsOverridden = reader.GetValue(4);

                        if (dataErrorId is int && dataIsOverridden is int)
                        {
                            initialState[(int)dataErrorId] = (int)dataIsOverridden;
                        }
                    }

                    reader.Close();
                }
            }

            // Save the data table for future lookup
            Session[keyDataSource] = t;
            Session[keyInitialState] = initialState;

            connection.Close();

            return t;
        }

        return null;
    }

    protected void ButtonExportErrors_Click(object sender, EventArgs e)
    {
        string organizationName = "";
        Control labelOrgName = ViewReportDetails.FindControl("LabelOrgName");

        if (labelOrgName is ASPxLabel)
        {
            organizationName = (labelOrgName as ASPxLabel).Text;
        }

        // Customize the report title
        GridViewErrors.Settings.ShowTitlePanel = true;
        GridViewErrors.SettingsText.Title = Resources.Strings.ErrorsExportTitle + "\n" + organizationName;

        GridViewExporterErrors.Styles.Title.Font.Size = 12;
        GridViewExporterErrors.Styles.Title.Font.Bold = true;
        GridViewExporterErrors.Styles.Title.Font.Name = "Arial";
        GridViewExporterErrors.Styles.Title.ForeColor = System.Drawing.Color.Black;

        // GridViewExporterErrors.Landscape = true;

        // Hide the columns which should not be exported
        GridViewErrors.Columns[3].Visible = false;
        GridViewErrors.Columns[4].Visible = false;
        GridViewErrors.Columns[5].Visible = false;

        // Perform export
        string returnFileName = GridViewExporterErrors.FileName;

        if (organizationName.Length > 0)
        {
            returnFileName += "_" + organizationName;
        }

        returnFileName = returnFileName.Replace(' ', '_');
        returnFileName = returnFileName.Replace("\"", "");
        returnFileName += ".xlsx";

        Response.AddHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        if (Request.Browser.Browser.ToLower().Contains("firefox"))
        {
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", returnFileName));
        }

        using (TempFile tempFile = new TempFile(returnFileName))
        {
            using (FileStream stream = File.Open(tempFile.FileName, FileMode.Create, FileAccess.ReadWrite))
            {
                DevExpress.XtraPrinting.XlsxExportOptions options = new DevExpress.XtraPrinting.XlsxExportOptions();
                options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                /*
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                options.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
                */

                GridViewExporterErrors.WriteXlsx(stream, options);

                // Rewind and pipe the stream contents to the output stream
                stream.Position = 0;
                stream.CopyTo(Response.OutputStream);
            }
        }

        // We need to hide the Grid title panel; it is made visible for export only
        if (GridViewExporterErrors.GridView != null)
        {
            GridViewExporterErrors.GridView.Settings.ShowTitlePanel = false;
        }

        // Show the columns which should not be exported
        GridViewErrors.Columns[3].Visible = true;
        GridViewErrors.Columns[4].Visible = true;
        GridViewErrors.Columns[5].Visible = true;

        Response.End();
    }

    protected bool IsCriticalError(int errorKind)
    {
        foreach (int err in criticalErrors)
        {
            if (errorKind == err)
            {
                return true;
            }
        }

        return false;
    }

    protected bool ResponseHeaderExists(string header)
    {
        header = header.Trim().ToLower();

        System.Collections.IEnumerator e = Response.Headers.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current is string)
            {
                string existingHeader = ((string)e.Current).Trim().ToLower();

                if (existingHeader == header)
                {
                    return true;
                }
            }
        }

        return false;
    }
}