using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using DevExpress.Web;
using System.Data.SqlClient;

public partial class OrendnaPlata_OrPlataObjectsUse : System.Web.UI.Page
{
    #region Nested classes

    /// <summary>
    /// Stores all information about custom 'value' columns
    /// </summary>
    protected class PeriodValueColumnInfo
    {
        /// <summary>
        /// Column caption
        /// </summary>
        public string caption = "";

        /// <summary>
        /// ID of reporting period
        /// </summary>
        public int periodId = -1;

        /// <summary>
        /// Name of the first field to compare
        /// </summary>
        public string fieldName1 = "";

        /// <summary>
        /// Name of the second field to compare
        /// </summary>
        public string fieldName2 = "";

        /// <summary>
        /// Indicates that comparison must be relative (in %), not absolute
        /// </summary>
        public bool percentComparison = false;

        /// <summary>
        /// Separator of period ID and field name
        /// </summary>
        public static string periodSeparator = "?";

        /// <summary>
        /// Separator of the column caption in the serialized state
        /// </summary>
        public static string captionSeparator = "<@>";

        /// <summary>
        /// Separator of two field names in the absolute comparison column
        /// </summary>
        public static string comparisonSeparatorAbs = "_#_";

        /// <summary>
        /// Separator of two field names in the relative comparison column
        /// </summary>
        public static string comparisonSeparatorRel = "_&_";

        /// <summary>
        /// Default constructor
        /// </summary>
        public PeriodValueColumnInfo()
        {
        }

        /// <summary>
        /// Creates a column definition for a simple column (not comparison)
        /// </summary>
        /// <param name="capt">Column caption</param>
        /// <param name="period">Period ID</param>
        /// <param name="field">Field name</param>
        public PeriodValueColumnInfo(string capt, int period, string field)
        {
            caption = capt;
            fieldName1 = field;
            periodId = period;
        }

        /// <summary>
        /// Creates a column definition for a comparison column
        /// </summary>
        /// <param name="capt">Column caption</param>
        /// <param name="field1">Name of the first field to compare</param>
        /// <param name="field2">Name of the second field to compare</param>
        public PeriodValueColumnInfo(string capt, string field1, string field2, bool percent)
        {
            caption = capt;
            fieldName1 = field1;
            fieldName2 = field2;
            percentComparison = percent;
        }

        /// <summary>
        /// Restores this object from a serialized string
        /// </summary>
        /// <param name="serialized">A serialized state, obtained from FormatForPresentationFilter() function</param>
        public PeriodValueColumnInfo(string serialized)
        {
            int dividerPos = serialized.IndexOf(captionSeparator);

            if (dividerPos > 0)
            {
                caption = serialized.Substring(dividerPos + captionSeparator.Length);

                string fieldsAndPeriod = serialized.Substring(0, dividerPos);

                dividerPos = fieldsAndPeriod.IndexOf(comparisonSeparatorAbs);

                if (dividerPos > 0)
                {
                    fieldName1 = fieldsAndPeriod.Substring(0, dividerPos);
                    fieldName2 = fieldsAndPeriod.Substring(dividerPos + comparisonSeparatorAbs.Length);
                    percentComparison = false;
                }
                else
                {
                    dividerPos = fieldsAndPeriod.IndexOf(comparisonSeparatorRel);

                    if (dividerPos > 0)
                    {
                        fieldName1 = fieldsAndPeriod.Substring(0, dividerPos);
                        fieldName2 = fieldsAndPeriod.Substring(dividerPos + comparisonSeparatorRel.Length);
                        percentComparison = true;
                    }
                    else
                    {
                        dividerPos = fieldsAndPeriod.IndexOf(periodSeparator);

                        if (dividerPos > 0)
                        {
                            periodId = int.Parse(fieldsAndPeriod.Substring(0, dividerPos));
                            fieldName1 = fieldsAndPeriod.Substring(dividerPos + periodSeparator.Length);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a correct combined field name for period and field
        /// </summary>
        /// <returns>Combined field name</returns>
        public string GetUnifiedFieldName()
        {
            if (fieldName2.Length > 0)
            {
                if (percentComparison)
                {
                    return fieldName1 + comparisonSeparatorRel + fieldName2;
                }
                else
                {
                    return fieldName1 + comparisonSeparatorAbs + fieldName2;
                }
            }
            else
            {
                return fieldName1 + "_" + periodId.ToString();
            }
        }

        /// <summary>
        /// Returns a serialized state of this object, suitable for Presentation Filter
        /// </summary>
        /// <returns>Serialized state as a string</returns>
        public string FormatForPresentationFilter()
        {
            if (fieldName2.Length > 0)
            {
                if (percentComparison)
                {
                    return fieldName1 + comparisonSeparatorRel + fieldName2 + captionSeparator + caption;
                }
                else
                {
                    return fieldName1 + comparisonSeparatorAbs + fieldName2 + captionSeparator + caption;
                }
            }
            else
            {
                return periodId.ToString() + periodSeparator + fieldName1 + captionSeparator + caption;
            }
        }
    }

    #endregion (Nested classes)

    protected Dictionary<string, string> objectsUseFields = new Dictionary<string, string>();

    protected Dictionary<int, string> rentPeriodNames = new Dictionary<int, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        ButtonShowFoldersPopup.Visible = userIsReportManager;

        // Check if this is first loading of this page
        object uniqueKey = ViewState["PageUniqueKey"];
        bool isFirstLoading = (uniqueKey == null);

        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // Bind the list box of fields for "Objects Use" table
        PopulateObjectsUseFields();

        ListBoxObjectsUseFields.DataSource = GetObjectsUseFieldDataSource();
        ListBoxObjectsUseFields.DataBind();

        // Add required amount of unbound columns to the "Objects Use" grid
        while (PrimaryGridView.Columns.Count < 40)
        {
            GridViewDataTextColumn col = new GridViewDataTextColumn();

            col.Caption = "";
            col.FieldName = "";
            col.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            col.PropertiesTextEdit.DisplayFormatString = "F";
            col.VisibleIndex = PrimaryGridView.Columns.Count;
            col.Visible = false;

            PrimaryGridView.Columns.Add(col);
        }

        // Recreate column captions and field names in the "Objects Use" grid
        Dictionary<int, PeriodValueColumnInfo> definitions = GetColumnDefinitions();

        if (definitions != null)
        {
            foreach (KeyValuePair<int, PeriodValueColumnInfo> def in definitions)
            {
                GridViewColumn col = PrimaryGridView.Columns[def.Key];

                if (col is GridViewDataTextColumn)
                {
                    GridViewDataTextColumn column = col as GridViewDataTextColumn;

                    column.FieldName = def.Value.GetUnifiedFieldName();
                    column.Caption = def.Value.caption;

                    if (def.Value.fieldName2.Length > 0 && def.Value.percentComparison)
                    {
                        column.PropertiesTextEdit.DisplayFormatString = "P";
                    }
                    else if (column.FieldName.StartsWith("num_"))
                    {
                        column.PropertiesTextEdit.DisplayFormatString = "#####";
                    }
                    else
                    {
                        column.PropertiesTextEdit.DisplayFormatString = "F";
                    }
                }
            }
        }

        // Hide the custom columns that have no data in the "Objects Use" grid
        for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
        {
            GridViewColumn col = PrimaryGridView.Columns[i];

            if (col is GridViewDataColumn)
            {
                GridViewDataColumn column = col as GridViewDataColumn;

                if (column.UnboundType == DevExpress.Data.UnboundColumnType.Decimal)
                {
                    column.ShowInCustomizationForm = column.Caption.Length > 0 && column.FieldName.Length > 0;

                    if (column.FieldName.Length == 0)
                    {
                        column.Visible = false;
                    }
                }
            }
        }

        // Get data for "Objects Use" grid custom columns from the SQL Server
        GenerateObjectsUseCustomColumnData();

        // If user-defined report is displayed, switch to the proper page of the Page control
        if (isFirstLoading)
        {
            string reportTitle = "";
            string preFilter = "";
            var fixedColumns = string.Empty;

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridIDOrendnaPlata_ObjectsUse)
            {
                LabelReportTitle6.Text = reportTitle;
                Utils.RestoreFixedColumns(PrimaryGridView, fixedColumns);

                if (preFilter.Length > 0)
                {
                    RestoreFromPresentationFilter(preFilter);
                }
            }
            else
            {
                // Restore grid fixed columns
                Utils.RestoreFixedColumns(PrimaryGridView);
            }
        }

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);

        // Enable advanced header filter for all grid columns
        Utils.AdjustColumnsVisibleInFilter(PrimaryGridView);

        // Add total summaries for all custom columns
        AddCustomTotalSummaries();

        // Fetch the data to the "Objects Use" Grid
        if (!IsCallback)
        {
            PrimaryGridView.DataBind();
        }

        // Disable the 'Vipiski' menu item if user is RDA
        if (Utils.RdaDistrictID > 0)
        {
            SectionMenu.Items[6].ClientVisible = false;
        }
    }

    protected void PopulateObjectsUseFields()
    {
        if (objectsUseFields.Count == 0)
        {
            objectsUseFields.Add("sqr_balans", Resources.Strings.RentObjectsUse_SqrBalans);
            objectsUseFields.Add("sqr_rent_total", Resources.Strings.RentObjectsUse_SqrRentTotal);
            objectsUseFields.Add("payment_zvit_uah", Resources.Strings.RentObjectsUse_Narahovano);
            objectsUseFields.Add("received_zvit_uah", Resources.Strings.RentObjectsUse_Otrymano);
            objectsUseFields.Add("debt_total_uah", Resources.Strings.RentObjectsUse_RentDebt);
            objectsUseFields.Add("avg_rent_payment", Resources.Strings.RentObjectsUse_RentAvg);
            objectsUseFields.Add("budget_narah_50_uah", Resources.Strings.RentObjectsUse_50_Narah);
            objectsUseFields.Add("budget_zvit_50_uah", Resources.Strings.RentObjectsUse_50_Payed);
            objectsUseFields.Add("budget_debt_50_uah", Resources.Strings.RentObjectsUse_50_DebtZvit);
            objectsUseFields.Add("budget_debt_30_50_uah", Resources.Strings.RentObjectsUse_50_DebtPrev);
            objectsUseFields.Add("num_agreements", Resources.Strings.RentObjectsUse_NumAgreements);
            objectsUseFields.Add("num_renters", Resources.Strings.RentObjectsUse_NumRenters);
        }

        // Get the names of rent periods
        rentPeriodNames.Clear();

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT id, name FROM dict_rent_period", connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataName = reader.IsDBNull(1) ? null : reader.GetValue(1);

                        if (dataId is int && dataName is string)
                        {
                            rentPeriodNames[(int)dataId] = (string)dataName;
                        }
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }
    }

    protected void GenerateObjectsUseCustomColumnData()
    {
        Dictionary<string, Dictionary<int, decimal>> customColumnData = GetCustomColumnData();

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
            {
                if (PrimaryGridView.Columns[i] is GridViewDataColumn)
                {
                    GridViewDataColumn column = PrimaryGridView.Columns[i] as GridViewDataColumn;

                    if (column.FieldName.Length > 0 && column.UnboundType == DevExpress.Data.UnboundColumnType.Decimal)
                    {
                        // Check if values for this column are already calculated
                        if (!column.FieldName.Contains(PeriodValueColumnInfo.comparisonSeparatorRel) &&
                            !column.FieldName.Contains(PeriodValueColumnInfo.comparisonSeparatorAbs) &&
                            !customColumnData.ContainsKey(column.FieldName))
                        {
                            Dictionary<int, decimal> values = new Dictionary<int, decimal>();

                            GenerateColumnDataValues(connection, column.FieldName, values);

                            customColumnData.Add(column.FieldName, values);
                        }
                    }
                }
            }

            connection.Close();
        }
    }

    protected void GenerateColumnDataValues(SqlConnection connection, string fieldName, Dictionary<int, decimal> values)
    {
        int dividerPos = fieldName.LastIndexOf('_');

        if (dividerPos > 0 && dividerPos < fieldName.Length - 1)
        {
            try
            {
                string actualFieldName = fieldName.Substring(0, dividerPos);
                string periodIdStr = fieldName.Substring(dividerPos + 1);

                int periodId = int.Parse(periodIdStr);

                using (SqlCommand cmd = new SqlCommand("SELECT org_balans_id, " + actualFieldName + " FROM view_rent_payments WHERE rent_period_id = @per", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("per", periodId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object dataOrgId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                            object dataValue = reader.IsDBNull(1) ? null : reader.GetValue(1);

                            if (dataValue is int)
                            {
                                dataValue = (decimal)((int)dataValue);
                            }

                            if (dataOrgId is int && dataValue is decimal)
                            {
                                values[(int)dataOrgId] = (decimal)dataValue;
                            }
                        }

                        reader.Close();
                    }
                }
            }
            finally
            {
            }
        }
    }

    protected void ASPxButton_ObjectsUse_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewRentObjectsUseExporter, PrimaryGridView, LabelReportTitle6.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_ObjectsUse_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewRentObjectsUseExporter, PrimaryGridView, LabelReportTitle6.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_ObjectsUse_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewRentObjectsUseExporter, PrimaryGridView, LabelReportTitle6.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewRentObjectsUse_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;

        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 35);

        string selPeriods = "selperiods:";
        string selFields = "selfields:";
        string compare = "compare:";

        if (param.StartsWith("init:"))
        {
            // Default processing
            Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView, Utils.GridIDOrendnaPlata_ObjectsUse, "");

            AddCustomTotalSummaries();
        }
        else if (param.StartsWith(selPeriods))
        {
            string periods = param.Substring(selPeriods.Length);
            string[] parts = periods.Split(new char[] { '?' });

            List<int> selectedPeriods = SelectedPeriods;

            selectedPeriods.Clear();

            foreach (string part in parts)
            {
                if (part.Length > 0)
                {
                    selectedPeriods.Add(int.Parse(part));
                }
            }

            RecreateCustomColumnDefinitions();
            AddCustomTotalSummaries();
        }
        else if (param.StartsWith(selFields))
        {
            string fields = param.Substring(selFields.Length);
            int dividerPos = fields.IndexOf('#');

            if (dividerPos >= 0)
            {
                string primaryFields = (dividerPos > 0) ? fields.Substring(0, dividerPos) : "";
                string additionalFields = (dividerPos < fields.Length - 1) ? fields.Substring(dividerPos + 1) : "";

                if (primaryFields.Length > 0)
                {
                    string[] parts = primaryFields.Split(new char[] { '?' });
                    HashSet<int> columnIndices = new HashSet<int>();

                    foreach (string part in parts)
                    {
                        if (part.Length > 0)
                        {
                            columnIndices.Add(int.Parse(part));
                        }
                    }

                    // Show / hide the primary grid columns
                    for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
                    {
                        if (PrimaryGridView.Columns[i] is GridViewDataColumn)
                        {
                            GridViewDataColumn column = PrimaryGridView.Columns[i] as GridViewDataColumn;

                            if (column.UnboundType == DevExpress.Data.UnboundColumnType.Bound)
                            {
                                column.Visible = columnIndices.Contains(i);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                List<string> selectedFields = SelectedFields;

                selectedFields.Clear();

                if (additionalFields.Length > 0)
                {
                    string[] parts = additionalFields.Split(new char[] { '?' });

                    foreach (string part in parts)
                    {
                        if (part.Length > 0)
                        {
                            selectedFields.Add(part);
                        }
                    }
                }
            }

            RecreateCustomColumnDefinitions();
            AddCustomTotalSummaries();
        }
        else if (param.StartsWith(compare))
        {
            bool percentComparison = false;

            if (param.EndsWith("%"))
            {
                percentComparison = true;
                param = param.Substring(0, param.Length - 1);
            }

            string[] parts = param.Split(new char[] { ':' });

            if (parts.Length == 3 && parts[1].Length > 0 && parts[2].Length > 0)
            {
                CompareColumns(int.Parse(parts[1]), int.Parse(parts[2]), percentComparison);

                RecreateCustomColumnDefinitions();
                AddCustomTotalSummaries();
            }
        }
        else
        {
            Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView,
                Utils.GridIDOrendnaPlata_ObjectsUse, GetObjectsUsePreFilter());
        }
    }

    protected void GridViewRentObjectsUse_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewRentObjectsUse_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void GridViewRentObjectsUse_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
    {
        Dictionary<string, Dictionary<int, decimal>> customColumnData = GetCustomColumnData();
        Dictionary<int, decimal> values1 = null;
        Dictionary<int, decimal> values2 = null;

        // Assign the default value, just in case
        e.Value = 0m;

        object orgId = e.GetListSourceFieldValue("organization_id");

        if (orgId is int)
        {
            int dividerPos = e.Column.FieldName.IndexOf(PeriodValueColumnInfo.comparisonSeparatorAbs);
            bool percentComparison = false;

            if (dividerPos < 0)
            {
                dividerPos = e.Column.FieldName.IndexOf(PeriodValueColumnInfo.comparisonSeparatorRel);
                percentComparison = (dividerPos > 0);
            }

            if (dividerPos > 0)
            {
                string fieldName1 = e.Column.FieldName.Substring(0, dividerPos);
                string fieldName2 = e.Column.FieldName.Substring(dividerPos + PeriodValueColumnInfo.comparisonSeparatorAbs.Length);

                if (customColumnData.TryGetValue(fieldName1, out values1) &&
                    customColumnData.TryGetValue(fieldName2, out values2))
                {
                    decimal value1 = 0;
                    decimal value2 = 0;

                    bool value1Exists = values1.TryGetValue((int)orgId, out value1);
                    bool value2Exists = values2.TryGetValue((int)orgId, out value2);

                    if (value1Exists || value2Exists)
                    {
                        if (percentComparison)
                        {
                            if (value1 != 0m)
                            {
                                e.Value = (value2 - value1) / value1;
                            }
                            else
                            {
                                e.Value = 0m;
                            }
                        }
                        else
                        {
                            e.Value = value2 - value1;
                        }
                    }
                }
            }
            else
            {
                if (customColumnData.TryGetValue(e.Column.FieldName, out values1))
                {
                    decimal value = 0;

                    if (values1.TryGetValue((int)orgId, out value))
                    {
                        e.Value = value;
                    }
                }
            }
        }
    }

    protected void CompareColumns(int columnIndex1, int columnIndex2, bool percentComparison)
    {
        GridViewDataColumn columnField1 = PrimaryGridView.Columns[columnIndex1] as GridViewDataColumn;
        GridViewDataColumn columnField2 = PrimaryGridView.Columns[columnIndex2] as GridViewDataColumn;

        // Both columns must contain custom data
        if (columnField1.UnboundType != DevExpress.Data.UnboundColumnType.Bound &&
            columnField2.UnboundType != DevExpress.Data.UnboundColumnType.Bound &&
            !columnField1.FieldName.Contains(PeriodValueColumnInfo.comparisonSeparatorAbs) &&
            !columnField1.FieldName.Contains(PeriodValueColumnInfo.comparisonSeparatorRel) &&
            !columnField2.FieldName.Contains(PeriodValueColumnInfo.comparisonSeparatorAbs) &&
            !columnField2.FieldName.Contains(PeriodValueColumnInfo.comparisonSeparatorRel))
        {
            // Add a new comparison column definition
            Dictionary<int, PeriodValueColumnInfo> definitions = GetColumnDefinitions();

            if (definitions != null)
            {
                string caption = string.Format(Resources.Strings.RentObjectsUseCompColumnTitle, columnField1.Caption, columnField2.Caption);

                definitions.Add(
                    PrimaryGridView.Columns.Count + 1,
                    new PeriodValueColumnInfo(caption, columnField1.FieldName, columnField2.FieldName, percentComparison));
            }
        }
    }

    protected void RecreateCustomColumnDefinitions()
    {
        List<int> selectedPeriods = SelectedPeriods;
        List<string> selectedFields = SelectedFields;
        Dictionary<int, PeriodValueColumnInfo> definitions = GetColumnDefinitions();

        // Save all definitions for comparison columns
        HashSet<PeriodValueColumnInfo> comparisonColumns = new HashSet<PeriodValueColumnInfo>();

        foreach (KeyValuePair<int, PeriodValueColumnInfo> pair in definitions)
        {
            if (pair.Value.fieldName2.Length > 0)
            {
                comparisonColumns.Add(pair.Value);
            }
        }

        // Recreate the column definitions
        definitions.Clear();

        int lastColumnIndex = 0;
        HashSet<string> newColumnNames = new HashSet<string>();
        HashSet<PeriodValueColumnInfo> comparisonColumnsToAdd = new HashSet<PeriodValueColumnInfo>();

        foreach (string field in selectedFields)
        {
            // Try to get the caption for this column
            string caption = "";

            if (objectsUseFields.TryGetValue(field, out caption))
            {
                // Add column for this field for all periods
                foreach (int periodId in selectedPeriods)
                {
                    // Try to get the period name
                    string periodName = "";

                    if (rentPeriodNames.TryGetValue(periodId, out periodName))
                    {
                        // Find the first unused unbound column
                        for (int i = lastColumnIndex + 1; i < PrimaryGridView.Columns.Count; i++)
                        {
                            if (PrimaryGridView.Columns[i] is GridViewDataTextColumn)
                            {
                                GridViewDataTextColumn column = PrimaryGridView.Columns[i] as GridViewDataTextColumn;

                                if (column.UnboundType == DevExpress.Data.UnboundColumnType.Decimal)
                                {
                                    lastColumnIndex = i;

                                    PeriodValueColumnInfo definition = new PeriodValueColumnInfo(
                                        string.Format(Resources.Strings.RentObjectsUseColumnFormat, caption, periodName), periodId, field);

                                    definitions[i] = definition;

                                    column.FieldName = definition.GetUnifiedFieldName();
                                    column.Caption = definition.caption;
                                    column.Visible = true;

                                    if (field.StartsWith("num_"))
                                    {
                                        column.PropertiesTextEdit.DisplayFormatString = "#####";
                                    }
                                    else
                                    {
                                        column.PropertiesTextEdit.DisplayFormatString = "F";
                                    }

                                    newColumnNames.Add(column.FieldName);

                                    break;
                                }
                            }
                        }
                    }
                }

                // Add all comparison columns related to this field
                comparisonColumnsToAdd.Clear();

                foreach (PeriodValueColumnInfo compColumn in comparisonColumns)
                {
                    if (newColumnNames.Contains(compColumn.fieldName1) &&
                        newColumnNames.Contains(compColumn.fieldName2))
                    {
                        comparisonColumnsToAdd.Add(compColumn);
                    }
                }

                foreach (PeriodValueColumnInfo compColumn in comparisonColumnsToAdd)
                {
                    comparisonColumns.Remove(compColumn);

                    // Find the first unused unbound column
                    for (int i = lastColumnIndex + 1; i < PrimaryGridView.Columns.Count; i++)
                    {
                        if (PrimaryGridView.Columns[i] is GridViewDataTextColumn)
                        {
                            GridViewDataTextColumn column = PrimaryGridView.Columns[i] as GridViewDataTextColumn;

                            if (column.UnboundType == DevExpress.Data.UnboundColumnType.Decimal)
                            {
                                lastColumnIndex = i;
                                definitions[i] = compColumn;

                                column.FieldName = compColumn.GetUnifiedFieldName();
                                column.Caption = compColumn.caption;
                                column.Visible = true;

                                if (compColumn.fieldName2.Length > 0 && compColumn.percentComparison)
                                {
                                    column.PropertiesTextEdit.DisplayFormatString = "P";
                                }
                                else if (field.StartsWith("num_"))
                                {
                                    column.PropertiesTextEdit.DisplayFormatString = "#####";
                                }
                                else
                                {
                                    column.PropertiesTextEdit.DisplayFormatString = "F";
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        // Clear all unbound columns that were not used
        for (int i = lastColumnIndex + 1; i < PrimaryGridView.Columns.Count; i++)
        {
            if (PrimaryGridView.Columns[i] is GridViewDataColumn)
            {
                GridViewDataColumn column = PrimaryGridView.Columns[i] as GridViewDataColumn;

                if (column.UnboundType == DevExpress.Data.UnboundColumnType.Decimal)
                {
                    column.Visible = false;
                    column.Caption = "";
                    column.FieldName = "";
                }
            }
        }

        GenerateObjectsUseCustomColumnData();
    }

    protected string GetObjectsUsePreFilter()
    {
        string preFilter = "";

        Dictionary<int, PeriodValueColumnInfo> definitions = GetColumnDefinitions();

        if (definitions != null)
        {
            SortedDictionary<int, PeriodValueColumnInfo> sortedDefinitions = new SortedDictionary<int, PeriodValueColumnInfo>();

            foreach (KeyValuePair<int, PeriodValueColumnInfo> def in definitions)
            {
                sortedDefinitions.Add(def.Key, def.Value);
            }

            foreach (KeyValuePair<int, PeriodValueColumnInfo> def in sortedDefinitions)
            {
                if (preFilter.Length > 0)
                {
                    preFilter += "|";
                }

                preFilter += def.Value.FormatForPresentationFilter();
            }
        }

        return preFilter;
    }

    protected void RestoreFromPresentationFilter(string preFilter)
    {
        if (preFilter.Contains("|") || preFilter.Contains(PeriodValueColumnInfo.captionSeparator))
        {
            RemoveAllSpecialColumns();

            List<int> selectedPeriods = SelectedPeriods;
            List<string> selectedFields = SelectedFields;

            selectedPeriods.Clear();
            selectedFields.Clear();

            Dictionary<int, PeriodValueColumnInfo> definitions = GetColumnDefinitions();

            if (definitions != null)
            {
                string[] parts = preFilter.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                int gridColumn = 0;

                for (int i = 0; i < parts.Length; i++)
                {
                    PeriodValueColumnInfo definition = new PeriodValueColumnInfo(parts[i]);

                    // Save the used period and field name
                    if (definition.periodId > 0 && !selectedPeriods.Contains(definition.periodId))
                    {
                        selectedPeriods.Add(definition.periodId);
                    }

                    if (definition.fieldName1.Length > 0 && !selectedFields.Contains(definition.fieldName1))
                    {
                        selectedFields.Add(definition.fieldName1);
                    }

                    if (definition.fieldName2.Length > 0 && !selectedFields.Contains(definition.fieldName2))
                    {
                        selectedFields.Add(definition.fieldName2);
                    }

                    // Find an unbound column in the Grid which is not used yet
                    while (gridColumn < PrimaryGridView.Columns.Count)
                    {
                        if (PrimaryGridView.Columns[gridColumn] is GridViewDataColumn)
                        {
                            GridViewDataColumn column = PrimaryGridView.Columns[gridColumn] as GridViewDataColumn;

                            if (column.FieldName.Length == 0 && column.UnboundType == DevExpress.Data.UnboundColumnType.Decimal)
                            {
                                column.FieldName = definition.GetUnifiedFieldName();
                                column.Caption = definition.caption;
                                column.Visible = true;

                                // Save the column definition
                                definitions[gridColumn] = definition;
                                break;
                            }
                        }

                        gridColumn++;
                    }
                }

                GenerateObjectsUseCustomColumnData();
            }
        }
    }

    protected void RemoveAllSpecialColumns()
    {
        // Remove all unbound columns
        foreach (GridViewDataColumn column in PrimaryGridView.Columns)
        {
            if (column.UnboundType == DevExpress.Data.UnboundColumnType.Decimal)
            {
                column.Visible = false;
                column.FieldName = "";
                column.Caption = "";
            }
        }

        // Remove definitions for all special columns
        Dictionary<int, PeriodValueColumnInfo> definitions = GetColumnDefinitions();

        if (definitions != null)
        {
            definitions.Clear();
        }
    }

    protected void AddCustomTotalSummaries()
    {
        HashSet<string> existingSummaries = new HashSet<string>();

        foreach (ASPxSummaryItem summary in PrimaryGridView.TotalSummary)
        {
            existingSummaries.Add(summary.FieldName.ToLower());
        }

        foreach (GridViewDataColumn column in PrimaryGridView.Columns)
        {
            // Process only columns related to financial data
            if (column.UnboundType != DevExpress.Data.UnboundColumnType.Bound &&
                column.FieldName.Length > 0)
            {
                string fieldName = column.FieldName.ToLower();

                if (!existingSummaries.Contains(fieldName))
                {
                    if (fieldName.StartsWith("avg_"))
                    {
                        ASPxSummaryItem avgItem = new ASPxSummaryItem(fieldName, DevExpress.Data.SummaryItemType.Average);

                        avgItem.DisplayFormat = "{0:#####}";

                        PrimaryGridView.TotalSummary.Add(avgItem);
                    }
                    else
                    {
                        ASPxSummaryItem sumItem = new ASPxSummaryItem(fieldName, DevExpress.Data.SummaryItemType.Sum);

                        if (fieldName.StartsWith("num_"))
                        {
                            sumItem.DisplayFormat = "{0:#####}";
                        }
                        else
                        {
                            sumItem.DisplayFormat = "{0:F}";
                        }

                        PrimaryGridView.TotalSummary.Add(sumItem);
                    }
                }
            }
        }
    }

    protected void CallbackPanelObjectsUsePeriods_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        // Update ListBoxObjectsUsePeriods selection from the SelectedPeriods session variable
        List<int> selPeriods = SelectedPeriods;

        foreach (ListEditItem item in ListBoxObjectsUsePeriods.Items)
        {
            if (item.Value is int)
            {
                item.Selected = selPeriods.Contains((int)item.Value);
            }
        }
    }

    protected void CallbackPanelObjectsUseFields_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        // Update ListBoxObjectsUseColumns selection from the selected grid columns
        for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
        {
            if (PrimaryGridView.Columns[i] is GridViewDataColumn)
            {
                GridViewDataColumn column = PrimaryGridView.Columns[i] as GridViewDataColumn;

                if (column.UnboundType == DevExpress.Data.UnboundColumnType.Bound)
                {
                    ListBoxObjectsUseColumns.Items[i].Selected = column.Visible;
                }
                else
                {
                    break;
                }
            }
        }

        // Update ListBoxObjectsUseFields selection from the SelectedFields session variable
        List<string> selFields = SelectedFields;

        foreach (ListEditItem item in ListBoxObjectsUseFields.Items)
        {
            if (item.Value is string)
            {
                item.Selected = selFields.Contains((string)item.Value);
            }
        }
    }

    protected void ComboCompareField_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        ASPxComboBox comboBox = sender as ASPxComboBox;

        for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
        {
            GridViewDataColumn column = PrimaryGridView.Columns[i] as GridViewDataColumn;

            // Process only columns related to financial data, and skip comparison columns
            if (column.UnboundType != DevExpress.Data.UnboundColumnType.Bound &&
                column.FieldName.Length > 0 &&
                !column.FieldName.Contains(PeriodValueColumnInfo.comparisonSeparatorAbs) &&
                !column.FieldName.Contains(PeriodValueColumnInfo.comparisonSeparatorRel))
            {
                comboBox.Items.Add(column.Caption, i);
            }
        }
    }

    protected void SqlDataSourceRentObjectsUse_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_rda_district_id"].Value = Utils.RdaDistrictID;
    }

    #region Session management

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

    protected List<int> SelectedPeriods
    {
        get
        {
            string key = GetPageUniqueKey() + "_SelPeriods";

            object periods = Session[key];

            if (periods is List<int>)
            {
                return periods as List<int>;
            }

            List<int> list = new List<int>();

            Session[key] = list;

            return list;
        }
    }

    protected List<string> SelectedFields
    {
        get
        {
            string key = GetPageUniqueKey() + "_SelFields";

            object periods = Session[key];

            if (periods is List<string>)
            {
                return periods as List<string>;
            }

            List<string> list = new List<string>();

            Session[key] = list;

            return list;
        }
    }

    protected DataTable GetObjectsUseFieldDataSource()
    {
        object t = GetPageUniqueKey() + Session["ObjectsUseFieldDataSource"];

        if (t is DataTable)
        {
            return t as DataTable;
        }

        DataTable table = new DataTable("ObjectsUseFieldDataSource");

        // Add the table columns
        table.Columns.Add("id", typeof(string));
        table.Columns.Add("name", typeof(string));

        // Create a primary key in the table
        DataColumn[] keys = new DataColumn[1];
        keys[0] = table.Columns[0];

        table.PrimaryKey = keys;

        // Add rows to the table
        object[] rowValues = new object[2];

        foreach (KeyValuePair<string, string> pair in objectsUseFields)
        {
            rowValues[0] = pair.Key;
            rowValues[1] = pair.Value;

            table.Rows.Add(rowValues);
        }

        Session["ObjectsUseFieldDataSource"] = table;

        return table;
    }

    protected Dictionary<int, PeriodValueColumnInfo> GetColumnDefinitions()
    {
        string key = GetPageUniqueKey() + "_ColumnDefinitions";

        object list = Session[key];

        if (list is Dictionary<int, PeriodValueColumnInfo>)
        {
            return list as Dictionary<int, PeriodValueColumnInfo>;
        }

        Dictionary<int, PeriodValueColumnInfo> definitions = new Dictionary<int, PeriodValueColumnInfo>();

        Session[key] = definitions;

        return definitions;
    }

    protected Dictionary<string, Dictionary<int, decimal>> GetCustomColumnData()
    {
        string key = GetPageUniqueKey() + "ObjectsUseCustomColumnData";

        object list = Session[key];

        if (list is Dictionary<string, Dictionary<int, decimal>>)
        {
            return list as Dictionary<string, Dictionary<int, decimal>>;
        }

        Dictionary<string, Dictionary<int, decimal>> columnData = new Dictionary<string, Dictionary<int, decimal>>();

        Session[key] = columnData;

        return columnData;
    }

    #endregion (Session management)

    #region Data binding support

    public string EvaluateObjectLink(object buildingId, object address)
    {
        if (!(address is string))
        {
            return "";
        }

        if (!(buildingId is int))
        {
            return address.ToString();
        }

        return "<a href=\"javascript:ShowObjectCardSimple(" + buildingId.ToString() + ")\">" + address.ToString() + "</a>";
    }

    public string EvaluateOrganizationLink(object organizationId, object name)
    {
        if (!(name is string))
        {
            return "";
        }

        if (!(organizationId is int))
        {
            return name.ToString();
        }

        return "<a href=\"javascript:ShowOrganizationCard(" + organizationId.ToString() + ")\">" + name.ToString() + "</a>";
    }

    #endregion (Data binding support)
}