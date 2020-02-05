using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;

public partial class Finance_FinForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // Generate the form data
        string formIdStr = Request.QueryString["fid"];

        if (!IsPostBack && !IsCallback && formIdStr != null && formIdStr.Length > 0)
        {
            FinFormID = int.Parse(formIdStr);

            GetFormDataSource();
        }

        UpdateFormTitle();

        // Bind the data grid to the data source
        GridViewFinForm.DataSource = GetFormDataSource();
        GridViewFinForm.DataBind();
    }

    protected void UpdateFormTitle()
    {
        int formId = FinFormID;

        if (formId > 0)
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                string query = @"SELECT
                    f.form_name,
                    f.form_number,
                    d.name,
                    per.name AS 'period_name'
                FROM fin_report_forms f
                LEFT OUTER JOIN dict_fin_report_type d ON d.id = f.report_type_id
                LEFT OUTER JOIN fin_report_periods per ON per.id = f.period_id
                WHERE f.id = @fid";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("fid", formId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string formName = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            string formNumber = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string formType = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            string periodName = reader.IsDBNull(3) ? "" : reader.GetString(3);

                            string title = "";

                            if (formType.Length > 0)
                            {
                                title = string.Format(Resources.Strings.FinFormTitleWithType, formName, formNumber, formType);
                            }
                            else
                            {
                                title = string.Format(Resources.Strings.FinFormTitle, formName, formNumber);
                            }

                            if (periodName.Length > 0)
                            {
                                LabelFormTitle.Text = title + ", " + periodName;
                            }
                            else
                            {
                                LabelFormTitle.Text = title;
                            }
                        }

                        reader.Close();
                    }
                }

                connection.Close();
            }
        }
    }

    protected void GridViewFinForm_DataBound(object sender, EventArgs e)
    {
        ASPxGridView grid = sender as ASPxGridView;

        for (int i = 0; i < grid.Columns.Count; i++)
        {
            if (i == 0)
            {
                grid.Columns[i].Width = 250;
            }
            else
            {
                grid.Columns[i].Width = 120;
            }

            if (grid.Columns[i] is GridViewDataColumn)
            {
                (grid.Columns[i] as GridViewDataColumn).Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            }
        }
    }

    #region Export to file

    protected void ASPxButton_Form_ExportXLS_Click(object sender, EventArgs e)
    {
        GridViewFinForm.DataSource = GetFormDataSource();
        GridViewFinForm.DataBind();

        this.ExportGridToXLS(ASPxGridViewExporterFinForm, GridViewFinForm, LabelFormTitle.Text, "");
    }

    protected void ASPxButton_Form_ExportPDF_Click(object sender, EventArgs e)
    {
        GridViewFinForm.DataSource = GetFormDataSource();
        GridViewFinForm.DataBind();

        this.ExportGridToPDF(ASPxGridViewExporterFinForm, GridViewFinForm, LabelFormTitle.Text, "");
    }

    protected void ASPxButton_Form_ExportCSV_Click(object sender, EventArgs e)
    {
        GridViewFinForm.DataSource = GetFormDataSource();
        GridViewFinForm.DataBind();

        this.ExportGridToCSV(ASPxGridViewExporterFinForm, GridViewFinForm, LabelFormTitle.Text, "");
    }

    #endregion (Export to file)

    #region Data Source Generation

    protected void GenerateFormDataSource(DataTable table)
    {
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            int formId = FinFormID;

            // Get all columns of this form
            SortedDictionary<int, string> columns = new SortedDictionary<int, string>();

            string query = "SELECT col_index FROM fin_report_columns WHERE form_id = @fid";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("fid", formId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            columns[reader.GetInt32(0)] = "";
                        }
                    }

                    reader.Close();
                }
            }

            // Get all rows of this form
            SortedDictionary<int, string> rows = new SortedDictionary<int, string>();

            query = "SELECT row_index, name FROM fin_report_rows WHERE form_id = @fid";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("fid", formId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            string rowName = reader.IsDBNull(1) ? "" : reader.GetString(1);

                            rows[reader.GetInt32(0)] = rowName;
                        }
                    }

                    reader.Close();
                }
            }

            // Get the form text
            string[,] formText = new string[rows.Count, columns.Count];

            for (int nRow = 0; nRow < rows.Count; nRow++)
            {
                for (int nCol = 0; nCol < columns.Count; nCol++)
                {
                    formText[nRow, nCol] = "";
                }
            }

            query = "SELECT row_index, col_index, cell_text FROM fin_report_cells WHERE form_id = @fid";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("fid", formId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                        {
                            int numRow = reader.GetInt32(0);
                            int numCol = reader.GetInt32(1);
                            string txt = reader.GetString(2);

                            formText[numRow - 1, numCol - 1] = txt;
                        }
                    }

                    reader.Close();
                }
            }

            // Get the form data
            decimal[,] formData = new decimal[rows.Count, columns.Count];

            for (int nRow = 0; nRow < rows.Count; nRow++)
            {
                for (int nCol = 0; nCol < columns.Count; nCol++)
                {
                    formData[nRow, nCol] = 0m;
                }
            }

            query = "SELECT num_row, num_col, [value] FROM fin_report_values WHERE organization_id = @oid AND form_id = @fid";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("oid", 28583));
                cmd.Parameters.Add(new SqlParameter("fid", formId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                        {
                            int numRow = reader.GetInt32(0);
                            int numCol = reader.GetInt32(1);
                            decimal val = reader.GetDecimal(2);

                            formData[numRow - 1, numCol - 1] += val;
                        }
                    }

                    reader.Close();
                }
            }

            // Add the table columns
            for (int nCol = 0; nCol < columns.Count; nCol++)
            {
                table.Columns.Add(formText[0, nCol], typeof(string));
            }

            // Add the table rows
            object[] rowValues = new object[columns.Count];

            for (int nRow = 1; nRow < rows.Count; nRow++)
            {
                for (int nCol = 0; nCol < columns.Count; nCol++)
                {
                    string txt = formText[nRow, nCol];
                    decimal val = formData[nRow, nCol];

                    if (txt.Length > 0)
                    {
                        rowValues[nCol] = txt;
                    }
                    else if (val != 0m)
                    {
                        rowValues[nCol] = val.ToString("F");
                    }
                    else
                    {
                        rowValues[nCol] = "";
                    }
                }

                table.Rows.Add(rowValues);
            }

            // Finished
            connection.Close();
        }
    }

    #endregion (Data Source Generation)

    #region Working with Session

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

    protected DataTable GetFormDataSource()
    {
        string key = GetPageUniqueKey() + "_FormDataSource";

        object t = Session[key];

        if (t is DataTable)
        {
            return t as DataTable;
        }

        DataTable table = new DataTable("FormData");

        GenerateFormDataSource(table);

        Session[key] = table;

        return table;
    }

    protected int FinFormID
    {
        get
        {
            string key = GetPageUniqueKey() + "_FinFormID";

            object formId = Session[key];

            if (formId is int)
            {
                return (int)formId;
            }

            return 0;
        }

        set
        {
            string key = GetPageUniqueKey() + "_FinFormID";

            Session[key] = value;
        }
    }

    #endregion (Working with Session)
}