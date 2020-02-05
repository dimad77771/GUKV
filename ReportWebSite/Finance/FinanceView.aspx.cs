using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;

public partial class Finance_FinanceView : System.Web.UI.Page
{
    #region Nested classes

    /// <summary>
    /// Lists various aggregate column types
    /// </summary>
    protected enum AggregateColumnType
    {
        Simple,
        RelativeComparison,
        AbsoluteComparison,
        Sum,
        Diff
    }

    /// <summary>
    /// Stores all information about custom 'value' columns
    /// </summary>
    protected class FinValueColumnInfo
    {
        /// <summary>
        /// Column caption
        /// </summary>
        public string caption = "";

        /// <summary>
        /// Name of the first field (for comparison)
        /// </summary>
        public string fieldName1 = "";

        /// <summary>
        /// Name of the second field (for an aggregate column). If this value is empty,
        /// the column simply displays data for the column specified in 'fieldName1'
        /// </summary>
        public string fieldName2 = "";

        /// <summary>
        /// Aggregate column type
        /// </summary>
        public AggregateColumnType columnType = AggregateColumnType.Simple;

        /// <summary>
        /// Separator of field name parts
        /// </summary>
        public static string fieldNameSeparator = "=";

        /// <summary>
        /// Separator of the column caption in the serialized state
        /// </summary>
        public static string captionSeparator = "<@>";

        /// <summary>
        /// Field name should be enclosed in braces
        /// </summary>
        public static string fieldNameOpeningTag = "{";

        /// <summary>
        /// Field name should be enclosed in braces
        /// </summary>
        public static string fieldNameClosingTag = "}";

        /// <summary>
        /// Default constructor
        /// </summary>
        public FinValueColumnInfo()
        {
        }

        /// <summary>
        /// Creates a column definition for a simple column (not comparison)
        /// </summary>
        /// <param name="field">Field name</param>
        /// <param name="capt">Column caption</param>
        public FinValueColumnInfo(string field, string capt)
        {
            caption = capt;
            fieldName1 = field;
        }

        /// <summary>
        /// Creates a column definition for comparison column
        /// </summary>
        /// <param name="field1">Name of the first field to compare</param>
        /// <param name="field2">Name of the second field to compare</param>
        /// <param name="capt">Column caption</param>
        public FinValueColumnInfo(string field1, string field2, AggregateColumnType colType, string capt)
        {
            caption = capt;
            fieldName1 = field1;
            fieldName2 = field2;
            columnType = colType;
        }

        /// <summary>
        /// Restores this object from a serialized string
        /// </summary>
        /// <param name="serialized">A serialized state, obtained from FormatForPresentationFilter() function</param>
        public FinValueColumnInfo(string serialized)
        {
            int dividerPos = serialized.IndexOf(captionSeparator);

            if (dividerPos > 0)
            {
                caption = serialized.Substring(dividerPos + captionSeparator.Length);

                string fields = serialized.Substring(0, dividerPos);

                ParseAggregateFieldName(fields, ref fieldName1, ref columnType, ref fieldName2);
            }
        }

        /// <summary>
        /// Returns a correct combined field name for both types of columns ('value' and 'comparison')
        /// </summary>
        /// <returns>Combined field name</returns>
        public string GetUnifiedFieldName()
        {
            if (fieldName2.Length > 0 && columnType != AggregateColumnType.Simple)
            {
                return fieldNameOpeningTag + fieldName1 + fieldNameClosingTag + AggregateSeparator +
                    fieldNameOpeningTag + fieldName2 + fieldNameClosingTag;
            }

            return fieldName1;
        }

        /// <summary>
        /// Returns a serialized state of this object, suitable for Presentation Filter
        /// </summary>
        /// <returns>Serialized state as a string</returns>
        public string FormatForPresentationFilter(bool columnVisible)
        {
            string visibility = columnVisible ? Finance_FinanceView.VisibleColumnTag : Finance_FinanceView.InvisibleColumnTag;

            return GetUnifiedFieldName() + captionSeparator + caption + visibility;
        }

        public static string GetAggregateColumnSeparator(AggregateColumnType colType)
        {
            switch (colType)
            {
                case AggregateColumnType.AbsoluteComparison:
                    return "?";

                case AggregateColumnType.RelativeComparison:
                    return "%";

                case AggregateColumnType.Sum:
                    return "+";

                case AggregateColumnType.Diff:
                    return "~";
            }

            return "";
        }

        public static IEnumerable<AggregateColumnType> GetAggregateColumnTypes()
        {
            return Enum.GetValues(typeof(AggregateColumnType)).Cast<AggregateColumnType>();
        }

        public static bool IsAggregateFieldName(string fieldName)
        {
            AggregateColumnType colType = AggregateColumnType.Simple;
            string field1 = "";
            string field2 = "";

            ParseAggregateFieldName(fieldName, ref field1, ref colType, ref field2);

            return colType != AggregateColumnType.Simple;
        }

        public string AggregateSeparator
        {
            get
            {
                return GetAggregateColumnSeparator(columnType);
            }
        }

        public static void ParseAggregateFieldName(string fieldName,
            ref string field1, ref AggregateColumnType columnType, ref string field2)
        {
            fieldName = RemoveRedundantBraces(fieldName);

            field1 = "";
            columnType = AggregateColumnType.Simple;
            field2 = "";

            if (fieldName.StartsWith(fieldNameOpeningTag))
            {
                int pos = 1;
                int braceCount = 1;

                while (braceCount > 0 && pos < fieldName.Length)
                {
                    if (fieldName[pos] == fieldNameOpeningTag[0])
                    {
                        braceCount++;
                    }
                    else if (fieldName[pos] == fieldNameClosingTag[0])
                    {
                        braceCount--;
                    }

                    pos++;
                }

                if (braceCount > 0)
                {
                    throw new ArgumentException("Invalid field name - opening and closing braces do not match: " + fieldName);
                }

                if (pos >= fieldName.Length)
                {
                    field1 = fieldName;
                }
                else
                {
                    field1 = fieldName.Substring(0, pos);

                    string remainder = fieldName.Substring(pos);

                    // Determine the aggregate column type
                    foreach (AggregateColumnType colType in GetAggregateColumnTypes())
                    {
                        string separator = GetAggregateColumnSeparator(colType);

                        if (separator.Length > 0)
                        {
                            if (remainder.StartsWith(separator))
                            {
                                columnType = colType;
                                field2 = remainder.Substring(separator.Length);
                                break;
                            }
                        }
                    }

                    if (columnType == AggregateColumnType.Simple || field2.Length == 0)
                    {
                        throw new ArgumentException("Could not parse aggregate field name: " + fieldName);
                    }
                }
            }
            else
            {
                // Determine the aggregate column type
                foreach (AggregateColumnType colType in GetAggregateColumnTypes())
                {
                    string separator = GetAggregateColumnSeparator(colType);

                    if (separator.Length > 0)
                    {
                        int dividerPos = fieldName.IndexOf(separator);

                        if (dividerPos > 0)
                        {
                            columnType = colType;
                            field1 = fieldName.Substring(0, dividerPos);
                            field2 = fieldName.Substring(dividerPos + 1);
                            break;
                        }
                    }
                }

                // If no aggregate column type, then this is a simple column
                if (columnType == AggregateColumnType.Simple)
                {
                    field1 = fieldName;
                }
            }

            field1 = RemoveRedundantBraces(field1);
            field2 = RemoveRedundantBraces(field2);
        }

        /// <summary>
        /// Removes the opening and closing braces from the field name, if they are redundant
        /// </summary>
        /// <param name="fieldName"></param>
        private static string RemoveRedundantBraces(string fieldName)
        {
            if (fieldName.StartsWith(fieldNameOpeningTag))
            {
                bool redundant = true;
                int pos = 1;
                int braceCount = 1;

                while (pos < fieldName.Length)
                {
                    if (fieldName[pos] == fieldNameOpeningTag[0])
                    {
                        braceCount++;
                    }
                    else if (fieldName[pos] == fieldNameClosingTag[0])
                    {
                        braceCount--;
                    }
                    else
                    {
                        if (braceCount == 0)
                        {
                            redundant = false;
                            break;
                        }
                    }

                    pos++;
                }

                if (redundant)
                {
                    return RemoveRedundantBraces(fieldName.Substring(1, fieldName.Length - 2));
                }
                else
                {
                    return fieldName;
                }
            }
            else
            {
                return fieldName;
            }
        }

        public static AggregateColumnType GetAggregateColumnTypeFromFieldName(string fieldName)
        {
            AggregateColumnType colType = AggregateColumnType.Simple;
            string field1 = "";
            string field2 = "";

            ParseAggregateFieldName(fieldName, ref field1, ref colType, ref field2);

            return colType;
        }
    }

    #endregion (Nested classes)

    public static string VisibleColumnTag = "+++";
    public static string InvisibleColumnTag = "---";

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        // Check if this is first loading of this page
        object uniqueKey = ViewState["PageUniqueKey"];
        bool isFirstLoading = (uniqueKey == null);

        // Generate a unique key for this instance of the page
        GetPageUniqueKey();

        // Add required amount of unbound columns
        while (PrimaryGridView.Columns.Count < 200)
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

        // Recreate column captions and field names
        Dictionary<int, FinValueColumnInfo> definitions = GetColumnDefinitions();

        if (definitions != null)
        {
            foreach (KeyValuePair<int, FinValueColumnInfo> def in definitions)
            {
                GridViewColumn col = PrimaryGridView.Columns[def.Key];

                if (col is GridViewDataTextColumn)
                {
                    GridViewDataTextColumn column = col as GridViewDataTextColumn;

                    column.FieldName = def.Value.GetUnifiedFieldName();
                    column.Caption = def.Value.caption;

                    if (def.Value.fieldName2.Length > 0 && def.Value.columnType == AggregateColumnType.RelativeComparison)
                    {
                        column.PropertiesTextEdit.DisplayFormatString = "P";
                    }
                    else
                    {
                        column.PropertiesTextEdit.DisplayFormatString = "F";
                    }
                }
            }
        }

        // Hide the custom columns that have no data
        HideCustomColumnsThatHaveNoData();

        // Get data for custom columns from the SQL Server
        GenerateCustomColumnData();

        // Enable or disable some controls based on user roles
        bool userIsReportManager = Roles.IsUserInRole(Utils.ReportManagerRole);

        ButtonShowFoldersPopup1.Visible = userIsReportManager;

        // Load the grids (only on first loading of the page)
        if (isFirstLoading)
        {
            string reportTitle = "";
            string preFilter = "";
            var fixedColumns = string.Empty;

            if (this.ProcessPageLoad(out reportTitle, out preFilter, out fixedColumns) == Utils.GridID_Finance)
            {
                LabelReportTitle1.Text = reportTitle;
                Utils.RestoreFixedColumns(PrimaryGridView, fixedColumns);
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

        // Add total summaries for all financial columns
        AddCustomTotalSummaries();

        // Fetch the data to the Grid
        if (!IsCallback)
        {
            PrimaryGridView.DataBind();
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
            if (column.FieldName.StartsWith(FinValueColumnInfo.fieldNameSeparator) ||
                column.FieldName.StartsWith(FinValueColumnInfo.fieldNameOpeningTag))
            {
                if (!existingSummaries.Contains(column.FieldName.ToLower()))
                {
                    AggregateColumnType colType = FinValueColumnInfo.GetAggregateColumnTypeFromFieldName(column.FieldName);

                    if (colType != AggregateColumnType.RelativeComparison)
                    {
                        if (!column.Caption.ToUpper().StartsWith(Resources.Strings.FinNormativVidrahuvannia))
                        {
                            ASPxSummaryItem sumItem = new ASPxSummaryItem(column.FieldName, DevExpress.Data.SummaryItemType.Sum);

                            sumItem.DisplayFormat = "{0:F}";

                            PrimaryGridView.TotalSummary.Add(sumItem);
                        }
                    }
                }
            }
        }
    }

    protected void HideCustomColumnsThatHaveNoData()
    {
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
    }

    protected void ASPxButton_Finance_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterFinance, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Finance_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterFinance, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_Finance_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterFinance, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void GridViewFinance_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string param = e.Parameters;
        Utils.ProcessGridPageSizeInCallback(PrimaryGridView, ref param, 60);

        if (param.StartsWith("addvalcolumn&#"))
        {
            string[] parts = param.Split(new string[] { "&#" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 5 && parts[1].Length > 0 && parts[2].Length > 0 && parts[3].Length > 0 && parts[4].Length > 0)
            {
                AddCustomColumn(int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]), parts[4]);

                AddCustomTotalSummaries();
                PrimaryGridView.DataBind();
            }
        }
        else if (param.StartsWith("addformulacolumn&#"))
        {
            string[] parts = param.Split(new string[] { "&#" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 3 && parts[1].Length > 0 && parts[2].Length > 0)
            {
                AddCustomColumn(int.Parse(parts[1]), -1, -1, parts[2]);

                AddCustomTotalSummaries();
                PrimaryGridView.DataBind();
            }
        }
        else if (param.StartsWith("compare:"))
        {
            bool percentComparison = false;

            if (param.EndsWith("%"))
            {
                percentComparison = true;
                param = param.Substring(0, param.Length - 1);
            }

            string[] parts = param.Split(new char[] { ':' });

            if (parts.Length > 3 && parts[1].Length > 0 && parts[2].Length > 0)
            {
                string caption = "";

                for (int i = 3; i < parts.Length; i++)
                {
                    if (i > 3)
                    {
                        caption += ":";
                    }

                    caption += parts[i];
                }

                CompareColumns(int.Parse(parts[1]), int.Parse(parts[2]),
                    percentComparison ? AggregateColumnType.RelativeComparison : AggregateColumnType.AbsoluteComparison,
                    caption);

                AddCustomTotalSummaries();
                PrimaryGridView.DataBind();
            }
        }
        else if (param.StartsWith("sum:") || param.StartsWith("diff:"))
        {
            string[] parts = param.Split(new char[] { ':' });

            if (parts.Length > 3 && parts[1].Length > 0 && parts[2].Length > 0)
            {
                string caption = "";

                for (int i = 3; i < parts.Length; i++)
                {
                    if (i > 3)
                    {
                        caption += ":";
                    }

                    caption += parts[i];
                }

                AggregateColumnType colType = AggregateColumnType.Sum;

                if (param.StartsWith("diff:"))
                {
                    colType = AggregateColumnType.Diff;
                }

                CompareColumns(int.Parse(parts[1]), int.Parse(parts[2]), colType, caption);

                AddCustomTotalSummaries();
                PrimaryGridView.DataBind();
            }
        }
        else if (param.StartsWith("clearcols:"))
        {
            RemoveAllSpecialColumns();

            PrimaryGridView.DataBind();
        }
        else
        {
            Utils.ProcessDataGridSaveLayoutCallback(param, PrimaryGridView,
                Utils.GridID_Finance, GetFinGridPreFilter());

            if (param.StartsWith("init:"))
            {
                string preFilter = Utils.GridPreFilterToLoad;

                if (preFilter.Length > 0)
                {
                    RestoreFromPresentationFilter(preFilter);
                    Utils.GridPreFilterToLoad = "";

                    HideCustomColumnsThatHaveNoData();
                    AddCustomTotalSummaries();
                }
            }
        }
    }

    protected void GridViewFinance_ProcessColumnAutoFilter(object sender,
        DevExpress.Web.ASPxGridView.ASPxGridViewAutoFilterEventArgs e)
    {
        Utils.ProcessGridColumnAutoFilter(sender, e);
    }

    protected void GridViewFinance_CustomFilterExpressionDisplayText(object sender,
        DevExpress.Web.ASPxEditors.CustomFilterExpressionDisplayTextEventArgs e)
    {
        this.UpdateFilterDisplayTextCache(e.DisplayText, PrimaryGridView);
    }

    protected void GridViewFinance_CustomColumnSort(object sender,
        DevExpress.Web.ASPxGridView.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
    }

    #region Column comparison

    protected void ComboCompareField_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        ASPxComboBox comboBox = sender as ASPxComboBox;

        for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
        {
            GridViewDataColumn column = PrimaryGridView.Columns[i] as GridViewDataColumn;

            // Process only columns related to financial data, and skip aggregate columns
            if (column.FieldName.StartsWith(FinValueColumnInfo.fieldNameSeparator) ||
                column.FieldName.StartsWith(FinValueColumnInfo.fieldNameOpeningTag))
            {
                comboBox.Items.Add(column.Caption, i);
            }
        }
    }

    protected void ComboSumField_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        ComboCompareField_Callback(sender, e);
    }

    protected void CompareColumns(int columnIndex1, int columnIndex2, AggregateColumnType colType, string caption)
    {
        GridViewDataColumn columnField1 = PrimaryGridView.Columns[columnIndex1] as GridViewDataColumn;
        GridViewDataColumn columnField2 = PrimaryGridView.Columns[columnIndex2] as GridViewDataColumn;

        // Both columns must contain custom data
        if (columnField1.UnboundType == DevExpress.Data.UnboundColumnType.Decimal &&
            columnField2.UnboundType == DevExpress.Data.UnboundColumnType.Decimal &&
            (columnField1.FieldName.StartsWith(FinValueColumnInfo.fieldNameSeparator) ||
            columnField1.FieldName.StartsWith(FinValueColumnInfo.fieldNameOpeningTag)) &&
            (columnField2.FieldName.StartsWith(FinValueColumnInfo.fieldNameSeparator) ||
            columnField2.FieldName.StartsWith(FinValueColumnInfo.fieldNameOpeningTag)))
        {
            Dictionary<int, FinValueColumnInfo> definitions = GetColumnDefinitions();

            if (caption.Length == 0)
            {
                caption = string.Format(Resources.Strings.CompFieldColumn, columnField1.Caption, columnField2.Caption);
            }

            FinValueColumnInfo definition = new FinValueColumnInfo(columnField1.FieldName, columnField2.FieldName, colType, caption);

            // If such field already exists in the Grid, just replace the column caption
            bool fieldExists = false;

            for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
            {
                if (PrimaryGridView.Columns[i] is GridViewDataTextColumn)
                {
                    GridViewDataTextColumn column = PrimaryGridView.Columns[i] as GridViewDataTextColumn;

                    if (column.FieldName == definition.GetUnifiedFieldName())
                    {
                        fieldExists = true;

                        column.Caption = caption;
                        column.Visible = true;

                        // Update the column definition
                        FinValueColumnInfo oldDefinition = null;

                        if (definitions != null && definitions.TryGetValue(i, out oldDefinition))
                        {
                            oldDefinition.caption = caption;
                        }

                        break;
                    }
                }
            }

            if (!fieldExists)
            {
                // Find an unbound column in the Grid which is not used yet
                for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
                {
                    if (PrimaryGridView.Columns[i] is GridViewDataTextColumn)
                    {
                        GridViewDataTextColumn column = PrimaryGridView.Columns[i] as GridViewDataTextColumn;

                        if (column.FieldName.Length == 0 && column.UnboundType == DevExpress.Data.UnboundColumnType.Decimal)
                        {
                            column.FieldName = definition.GetUnifiedFieldName();
                            column.Caption = definition.caption;
                            column.Visible = true;

                            if (colType == AggregateColumnType.RelativeComparison)
                            {
                                column.PropertiesTextEdit.DisplayFormatString = "P";
                            }
                            else
                            {
                                column.PropertiesTextEdit.DisplayFormatString = "F";
                            }

                            // Save the column definition
                            if (definitions != null)
                            {
                                definitions[i] = definition;
                            }

                            break;
                        }
                    }
                }
            }
        }
    }

    #endregion (Column comparison)

    #region Adding of custom columns

    private int ValueColPeriodID
    {
        get { return Session[GetPageUniqueKey() + "_ValueColPeriodID"] is int ? (int)Session[GetPageUniqueKey() + "_ValueColPeriodID"] : 0; }
        set { Session[GetPageUniqueKey() + "_ValueColPeriodID"] = value; }
    }

    private int FormulaColPeriodID
    {
        get { return Session[GetPageUniqueKey() + "_FormulaColPeriodID"] is int ? (int)Session[GetPageUniqueKey() + "_FormulaColPeriodID"] : 0; }
        set { Session[GetPageUniqueKey() + "_FormulaColPeriodID"] = value; }
    }

    private int ValueColReportTypeID
    {
        get { return Session[GetPageUniqueKey() + "_ValueColReportTypeID"] is int ? (int)Session[GetPageUniqueKey() + "_ValueColReportTypeID"] : 0; }
        set { Session[GetPageUniqueKey() + "_ValueColReportTypeID"] = value; }
    }

    private int ValueColFormID
    {
        get { return Session[GetPageUniqueKey() + "_ValueColFormID"] is int ? (int)Session[GetPageUniqueKey() + "_ValueColFormID"] : 0; }
        set { Session[GetPageUniqueKey() + "_ValueColFormID"] = value; }
    }

    protected void SqlDataSourceValueColForms_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@per_id"].Value = ValueColPeriodID;
        e.Command.Parameters["@rep_t"].Value = ValueColReportTypeID;
    }

    protected void SqlDataSourceFormulae_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@per_id"].Value = FormulaColPeriodID;
    }

    protected void SqlDataSourceValueColColumns_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@fid"].Value = ValueColFormID;
    }

    protected void SqlDataSourceValueColRows_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@fid"].Value = ValueColFormID;
    }

    protected void ComboValueColForm_Callback(object source, CallbackEventArgsBase e)
    {
        try
        {
            string[] parts = e.Parameter.Split(new char[] { '|' });

            if (parts.Length == 2 && parts[0].Length > 0 && parts[1].Length > 0)
            {
                ValueColPeriodID = int.Parse(parts[0]);
                ValueColReportTypeID = int.Parse(parts[1]);

                (source as ASPxComboBox).DataBind();
            }
        }
        finally
        {
        }
    }

    protected void ComboFormula_Callback(object source, CallbackEventArgsBase e)
    {
        try
        {
            if (e.Parameter.Length > 0)
            {
                FormulaColPeriodID = int.Parse(e.Parameter);

                (source as ASPxComboBox).DataBind();
            }
        }
        finally
        {
        }
    }

    protected void CPValueColRows_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (e.Parameter.Length > 0)
        {
            ValueColFormID = int.Parse(e.Parameter);

            ListBoxValueColRows.DataBind();
            ComboValueColColumns.DataBind();
        }
    }

    protected void AddCustomColumn(int formOrFormulaId, int rowNumber, int colNumber, string caption)
    {
        Dictionary<int, FinValueColumnInfo> definitions = GetColumnDefinitions();

        // Generate a special field name which contains all required information
        string fieldName = "";

        if (rowNumber >= 0 && colNumber >= 0)
        {
            fieldName =
                FinValueColumnInfo.fieldNameSeparator + formOrFormulaId.ToString() +
                FinValueColumnInfo.fieldNameSeparator + rowNumber.ToString() +
                FinValueColumnInfo.fieldNameSeparator + colNumber.ToString();
        }
        else
        {
            fieldName = FinValueColumnInfo.fieldNameSeparator + formOrFormulaId.ToString();
        }

        // If such field already exists in the Grid, just replace the column caption
        bool fieldExists = false;

        for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
        {
            if (PrimaryGridView.Columns[i] is GridViewDataTextColumn)
            {
                GridViewDataTextColumn column = PrimaryGridView.Columns[i] as GridViewDataTextColumn;

                if (column.FieldName == fieldName)
                {
                    fieldExists = true;

                    column.Caption = caption;
                    column.Visible = true;

                    // Update the column definition
                    FinValueColumnInfo definition = null;

                    if (definitions != null && definitions.TryGetValue(i, out definition))
                    {
                        definition.fieldName1 = fieldName;
                        definition.caption = caption;
                    }

                    break;
                }
            }
        }

        if (!fieldExists)
        {
            // Find an unbound column in the Grid which is not used yet
            for (int i = 0; i < PrimaryGridView.Columns.Count; i++)
            {
                if (PrimaryGridView.Columns[i] is GridViewDataTextColumn)
                {
                    GridViewDataTextColumn column = PrimaryGridView.Columns[i] as GridViewDataTextColumn;

                    if (column.FieldName.Length == 0 && column.UnboundType == DevExpress.Data.UnboundColumnType.Decimal)
                    {
                        column.FieldName = fieldName;
                        column.Caption = caption;
                        column.Visible = true;

                        GenerateCustomColumnData();

                        // Save the column definition
                        if (definitions != null)
                        {
                            definitions[i] = new FinValueColumnInfo(fieldName, caption);
                        }

                        break;
                    }
                }
            }
        }
    }

    protected void GenerateCustomColumnData()
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
                        if ((column.FieldName.StartsWith(FinValueColumnInfo.fieldNameSeparator) ||
                            column.FieldName.StartsWith(FinValueColumnInfo.fieldNameOpeningTag)) &&
                            !FinValueColumnInfo.IsAggregateFieldName(column.FieldName))
                        {
                            // Check if values for this column are already calculated
                            if (!customColumnData.ContainsKey(column.FieldName))
                            {
                                Dictionary<int, decimal> values = new Dictionary<int, decimal>();

                                GenerateColumnDataValues(connection, column.FieldName, values);

                                customColumnData.Add(column.FieldName, values);
                            }
                        }
                    }
                }
            }

            connection.Close();
        }
    }

    protected void GenerateColumnDataValues(SqlConnection connection, string fieldName, Dictionary<int, decimal> values)
    {
        using (SqlCommand commandSelect = new SqlCommand("", connection))
        {
            string[] parts = fieldName.Split(new char[] { FinValueColumnInfo.fieldNameSeparator[0] }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 3 && parts[0].Length > 0 && parts[1].Length > 0 && parts[2].Length > 0)
            {
                int formId = int.Parse(parts[0]);
                int rowNum = int.Parse(parts[1]);
                int colNum = int.Parse(parts[2]);

                commandSelect.CommandText = "SELECT organization_id, [value] FROM fin_report_values WHERE form_id = @fid AND num_row = @nrow AND num_col = @ncol ORDER BY id";

                commandSelect.Parameters.Add(new SqlParameter("fid", formId));
                commandSelect.Parameters.Add(new SqlParameter("nrow", rowNum));
                commandSelect.Parameters.Add(new SqlParameter("ncol", colNum));
            }
            else if (parts.Length == 1 && parts[0].Length > 0)
            {
                int formulaId = int.Parse(parts[0]);

                commandSelect.CommandText = "SELECT organization_id, [value] FROM fin_report_formula_values WHERE formula_id = @fid";

                commandSelect.Parameters.Add(new SqlParameter("fid", formulaId));
            }

            if (commandSelect.CommandText.Length > 0)
            {
                using (SqlDataReader reader = commandSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataOrgId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataValue = reader.IsDBNull(1) ? null : reader.GetValue(1);

                        if (dataOrgId is int && dataValue is decimal)
                        {
                            values[(int)dataOrgId] = (decimal)dataValue;

                            // The old algorithm, which merges all values together
                            /*
                            int organizationId = (int)dataOrgId;
                            decimal prevValue = 0;

                            if (values.TryGetValue(organizationId, out prevValue))
                            {
                                // Multiple values for one row/cell are possible. We must merge them
                                values[organizationId] = prevValue + (decimal)dataValue;
                            }
                            else
                            {
                                values.Add(organizationId, (decimal)dataValue);
                            }
                            */
                        }
                    }

                    reader.Close();
                }
            }
        }
    }

    protected void GridViewFinance_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
    {
        Dictionary<string, Dictionary<int, decimal>> customColumnData = GetCustomColumnData();

        bool isPercentage = false;
        decimal value = CalcColumnValue(e.Column.FieldName, e, customColumnData, out isPercentage);

        e.Value = isPercentage ? value : value / 1000m;
    }

    protected decimal CalcColumnValue(string fieldName, ASPxGridViewColumnDataEventArgs e,
        Dictionary<string, Dictionary<int, decimal>> customColumnData, out bool isPercentage)
    {
        isPercentage = false;

        string field1 = "";
        string field2 = "";
        AggregateColumnType colType = AggregateColumnType.Simple;

        FinValueColumnInfo.ParseAggregateFieldName(fieldName, ref field1, ref colType, ref field2);

        switch (colType)
        {
            case AggregateColumnType.Simple:
                {
                    Dictionary<int, decimal> values = null;

                    if (customColumnData.TryGetValue(fieldName, out values))
                    {
                        object orgId = e.GetListSourceFieldValue("organization_id");

                        if (orgId is int)
                        {
                            decimal value = 0;

                            if (values.TryGetValue((int)orgId, out value))
                            {
                                return value;
                            }
                        }
                    }
                }
                break;

            case AggregateColumnType.Sum:
                return CalcColumnValue(field1, e, customColumnData, out isPercentage) + CalcColumnValue(field2, e, customColumnData, out isPercentage);

            case AggregateColumnType.Diff:
                return CalcColumnValue(field1, e, customColumnData, out isPercentage) - CalcColumnValue(field2, e, customColumnData, out isPercentage);

            case AggregateColumnType.AbsoluteComparison:
                return CalcColumnValue(field2, e, customColumnData, out isPercentage) - CalcColumnValue(field1, e, customColumnData, out isPercentage);

            case AggregateColumnType.RelativeComparison:
                {
                    decimal val1 = CalcColumnValue(field1, e, customColumnData, out isPercentage);
                    decimal val2 = CalcColumnValue(field2, e, customColumnData, out isPercentage);

                    isPercentage = true;

                    if (val1 != 0m)
                    {
                        return (val2 - val1) / val1;
                    }
                }
                break;

            default:
                throw new ArgumentException("Unknown aggregate column type in the field name: " + fieldName);
        }

        return 0m;
    }

    protected void RemoveAllSpecialColumns()
    {
        // Remove all total summaries for our special columns
        for (int i = PrimaryGridView.TotalSummary.Count - 1; i >= 0; i--)
        {
            if (PrimaryGridView.TotalSummary[i].FieldName.Contains(FinValueColumnInfo.fieldNameSeparator))
            {
                PrimaryGridView.TotalSummary.RemoveAt(i);
            }
        }

        // Remove all columns for which FieldName contains our special characters
        foreach (GridViewDataColumn column in PrimaryGridView.Columns)
        {
            if (column.FieldName.Contains(FinValueColumnInfo.fieldNameSeparator))
            {
                column.Visible = false;
                column.FieldName = "";
                column.Caption = "";
            }
        }

        // Remove definitions for all comparison columns
        Dictionary<int, FinValueColumnInfo> definitions = GetColumnDefinitions();

        if (definitions != null)
        {
            definitions.Clear();
        }
    }

    #endregion (Adding of custom columns)

    #region Presentation Filter

    protected string GetFinGridPreFilter()
    {
        string preFilter = "";

        Dictionary<int, FinValueColumnInfo> definitions = GetColumnDefinitions();

        if (definitions != null)
        {
            SortedDictionary<int, FinValueColumnInfo> sortedDefinitions = new SortedDictionary<int, FinValueColumnInfo>();

            foreach (KeyValuePair<int, FinValueColumnInfo> def in definitions)
            {
                sortedDefinitions.Add(def.Key, def.Value);
            }

            foreach (KeyValuePair<int, FinValueColumnInfo> def in sortedDefinitions)
            {
                if (preFilter.Length > 0)
                {
                    preFilter += "|";
                }

                bool columnVisible = true;

                if (def.Key < PrimaryGridView.Columns.Count)
                {
                    columnVisible = PrimaryGridView.Columns[def.Key].Visible;
                }

                preFilter += def.Value.FormatForPresentationFilter(columnVisible);
            }
        }

        return preFilter;
    }

    protected void RestoreFromPresentationFilter(string preFilter)
    {
        if (preFilter.Contains("|") || preFilter.Contains(FinValueColumnInfo.captionSeparator))
        {
            RemoveAllSpecialColumns();

            Dictionary<int, FinValueColumnInfo> definitions = GetColumnDefinitions();

            if (definitions != null)
            {
                string[] parts = preFilter.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                int gridColumn = 0;

                for (int i = 0; i < parts.Length; i++)
                {
                    string serializedField = parts[i];
                    bool columnVisible = true;

                    if (serializedField.EndsWith(VisibleColumnTag))
                    {
                        columnVisible = true;
                        serializedField = serializedField.Substring(0, serializedField.Length - VisibleColumnTag.Length);
                    }
                    else if (serializedField.EndsWith(InvisibleColumnTag))
                    {
                        columnVisible = false;
                        serializedField = serializedField.Substring(0, serializedField.Length - InvisibleColumnTag.Length);
                    }

                    FinValueColumnInfo definition = new FinValueColumnInfo(serializedField);

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
                                column.Visible = columnVisible;

                                // Save the column definition
                                definitions[gridColumn] = definition;
                                break;
                            }
                        }

                        gridColumn++;
                    }
                }

                GenerateCustomColumnData();
            }
        }
    }

    #endregion (Presentation Filter)

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

    protected Dictionary<string, Dictionary<int, decimal>> GetCustomColumnData()
    {
        string key = "FinCustomColumnData";

        object list = Session[key];

        if (list is Dictionary<string, Dictionary<int, decimal>>)
        {
            return list as Dictionary<string, Dictionary<int, decimal>>;
        }

        Dictionary<string, Dictionary<int, decimal>> columnData = new Dictionary<string, Dictionary<int, decimal>>();

        Session[key] = columnData;

        return columnData;
    }

    protected Dictionary<int, FinValueColumnInfo> GetColumnDefinitions()
    {
        string key = GetPageUniqueKey() + "_ColumnDefinitions";

        object list = Session[key];

        if (list is Dictionary<int, FinValueColumnInfo>)
        {
            return list as Dictionary<int, FinValueColumnInfo>;
        }

        Dictionary<int, FinValueColumnInfo> definitions = new Dictionary<int, FinValueColumnInfo>();

        Session[key] = definitions;

        return definitions;
    }

    #endregion (Working with Session)

    #region Form picker

    private int FormPickerPeriodID
    {
        get { return Session[GetPageUniqueKey() + "_FormPickerPeriodID"] is int ? (int)Session[GetPageUniqueKey() + "_FormPickerPeriodID"] : 0; }
        set { Session[GetPageUniqueKey() + "_FormPickerPeriodID"] = value; }
    }

    private int FormPickerReportTypeID
    {
        get { return Session[GetPageUniqueKey() + "_FormPickerRepTypeID"] is int ? (int)Session[GetPageUniqueKey() + "_FormPickerRepTypeID"] : 0; }
        set { Session[GetPageUniqueKey() + "_FormPickerRepTypeID"] = value; }
    }

    protected void SqlDataSourceDictForms_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@per_id"].Value = FormPickerPeriodID;
        e.Command.Parameters["@rep_t"].Value = FormPickerReportTypeID;
    }

    protected void ComboFormName_Callback(object source, CallbackEventArgsBase e)
    {
        try
        {
            string[] parts = e.Parameter.Split(new char[] { '|' });

            if (parts.Length == 2 && parts[0].Length > 0 && parts[1].Length > 0)
            {
                FormPickerPeriodID = int.Parse(parts[0]);
                FormPickerReportTypeID = int.Parse(parts[1]);

                (source as ASPxComboBox).DataBind();
            }
        }
        finally
        {
        }
    }

    #endregion (Form picker)
}