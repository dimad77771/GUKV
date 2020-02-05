using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;

namespace GUKV.DataMigration
{
    /// <summary>
    /// Describes particular formula of financial reporting
    /// </summary>
    public class FormulaInfo
    {
        /// <summary>
        /// Formula ID, obtained from the Balans database
        /// </summary>
        public int id = -1;

        /// <summary>
        /// ID of the calendar period (a quarter) to which this formula applies
        /// </summary>
        public int period = -1;

        /// <summary>
        /// Formula name
        /// </summary>
        public string name = "";

        /// <summary>
        /// Formula string
        /// </summary>
        public string formula = "";

        /// <summary>
        /// Parsed formula string
        /// </summary>
        public List<string> tokens = new List<string>();

        /// <summary>
        /// Characters that separate the formula tokens
        /// </summary>
        public static char[] ValueTokenSeparators = new char[] { ',' };

        /// <summary>
        /// Default constructor
        /// </summary>
        public FormulaInfo()
        {
        }

        /// <summary>
        /// Splits the formula string into tokens (operators, constants, and references to values)
        /// </summary>
        public void Parse()
        {
            string token = "";

            for (int i = 0; i < formula.Length; i++)
            {
                if (IsOperatorOrBrace(formula[i]))
                {
                    token = token.Trim();

                    if (token.Length > 0)
                    {
                        tokens.Add(token);
                    }

                    tokens.Add(formula[i].ToString());

                    token = "";
                }
                else
                {
                    token += formula[i];
                }
            }

            token = token.Trim();

            if (token.Length > 0)
            {
                tokens.Add(token);
            }
        }

        /// <summary>
        /// Verifies if the given character is a valid operator, or a brace
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>TRUE if if the given character is a valid operator, or a brace</returns>
        public static bool IsOperatorOrBrace(char c)
        {
            return
                c == '+' ||
                c == '-' ||
                c == '/' ||
                c == '*' ||
                c == '(' ||
                c == ')';
        }

        /// <summary>
        /// Verifies if the given character is a valid operator
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>TRUE if if the given character is a valid operator</returns>
        public static bool IsOperator(char c)
        {
            return
                c == '+' ||
                c == '-' ||
                c == '/' ||
                c == '*';
        }

        /// <summary>
        /// Defines the operator priority
        /// </summary>
        /// <param name="c">Operator character</param>
        /// <returns>Operator priority; greater values mean higher priority</returns>
        public static int GetOperatorPriority(char c)
        {
            switch (c)
            {
                case '+':
                case '-':
                    return 1;

                case '*':
                case '/':
                    return 2;
            }

            return 0;
        }

        /// <summary>
        /// Returns the list of forms that are used in this formula
        /// </summary>
        /// <param name="forms">IDs of all the used forms are added to this list</param>
        public void GetUsedForms(HashSet<int> forms)
        {
            foreach (string token in tokens)
            {
                string[] valueInfo = token.Split(ValueTokenSeparators);

                if (valueInfo.Length >= 3)
                {
                    forms.Add(int.Parse(valueInfo[0]));
                }
            }
        }
    }

    /// <summary>
    /// Populates the SQL Server database from the Firebird databases.
    /// </summary>
//    public partial class Migrator
//    {
//        #region Member variables

//        /// <summary>
//        /// Forms and formulae related to the earlier periods are NOT imported.
//        /// 64 = 1st quarter of year 2010
//        /// </summary>
//        private int minBalansPeriodToImport = 1; // 64

//        private Dictionary<int, FormulaInfo> balansFormulae = new Dictionary<int, FormulaInfo>();

//        private HashSet<int> balansForms = new HashSet<int>();

//        private Dictionary<string, decimal> formValueCache = new Dictionary<string, decimal>();

//        /// <summary>
//        /// This seed is used to generate IDs of injected formulae. It is auto-decremented,
//        /// so IDs of injected formulae form a sequence of negative numbers: -1, -2, -3, etc.
//        /// </summary>
//        private int customFormulaIdSeed = 0;

//        /// <summary>
//        /// A special prefix for formulae generated by our system
//        /// </summary>
//        private string EIS = "[i] ";

//        #endregion (Member variables)

//        #region Migration of financial reports

//        private void MigrateBalansFinReports()
//        {
//            logger.WriteInfo("=== Migrating financial reports from the Balans database ===");

//            MigrateBalansFinDictionaries();
//            RetrieveAllUsedFinFormulae();

//            // Add some formulae which are not present in Balans datatabse (but we need them)
//            InjectBalansCustomFormulae();

//            // Parse all formulae
//            foreach (KeyValuePair<int, FormulaInfo> pair in balansFormulae)
//            {
//                pair.Value.Parse();
//            }

//            // Migrate all forms
//            MigrateBalansForms();

//            // Dump the formulae that we have migrated
//            CSVLog logFormulae = new CSVLog("CSV/BalansFormulae.csv");

//            logFormulae.WriteCell("ID");
//            logFormulae.WriteCell("Name");
//            logFormulae.WriteCell("Period");
//            logFormulae.WriteCell("Formula");
//            logFormulae.WriteEndOfLine();

//            foreach (KeyValuePair<int, FormulaInfo> pair in balansFormulae)
//            {
//                logFormulae.WriteCell(pair.Value.id.ToString());
//                logFormulae.WriteCell(pair.Value.name);
//                logFormulae.WriteCell(pair.Value.period.ToString());
//                logFormulae.WriteCell(pair.Value.formula);
//                logFormulae.WriteEndOfLine();
//            }

//            if (!testOnly)
//            {
//                MigrateFinReportValues();

//                PrecalculateFormulaeForAllOrganizations();
//            }

//            MigrateFormChanges();
//        }

//        private void MigrateBalansFinDictionaries()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // TYPE_ZVITNOST
//            fields.Clear();
//            fields.Add("KOD_TYPE_ZVITNOST", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connectionBalans, "TYPE_ZVITNOST", "dict_fin_report_type", fields, "KOD_TYPE_ZVITNOST", null);

//            // S_TYPE_FORM
//            fields.Clear();
//            fields.Add("KOD_TYPE_FORM", "id");
//            fields.Add("NAME_TYPE_FORM", "name");

//            MigrateTable(connectionBalans, "S_TYPE_FORM", "dict_fin_form_type", fields, "KOD_TYPE_FORM", null);

//            // S_PRIZNAKFORM
//            fields.Clear();
//            fields.Add("PRIZNAKFORMID", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connectionBalans, "S_PRIZNAKFORM", "dict_fin_form_kind", fields, "PRIZNAKFORMID", null);

//            // DANPER
//            fields.Clear();
//            fields.Add("KOD_DANPER", "id");
//            fields.Add("PERIOD", "name");
//            fields.Add("NACHALO", "period_start");
//            fields.Add("USER_KOREG", "modified_by");
//            fields.Add("DATE_KOREG", "modify_date");
//            fields.Add("FIN_PLAN", "is_fin_plan");
//            fields.Add("PARENT_KOD", "parent_id");
//            fields.Add("IS_DEL", "is_deleted");

//            MigrateTable(connectionBalans, "DANPER", "fin_report_periods", fields, "KOD_DANPER", null);
//        }

//        private void RetrieveAllUsedFinFormulae()
//        {
//            string querySelect = "SELECT RFORMUALID, KOD_DANPER, RFORMULA_NAME, RFORMULALINE FROM REP_FORMULA" +
//                " WHERE KOD_DANPER >= " + minBalansPeriodToImport.ToString();

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connectionBalans))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
//                            object dataPeriodId = reader.IsDBNull(1) ? null : reader.GetValue(1);
//                            object dataName = reader.IsDBNull(2) ? null : reader.GetValue(2);
//                            object dataFormula = reader.IsDBNull(3) ? null : reader.GetValue(3);

//                            if (dataId is int && dataPeriodId is int && dataName is string && dataFormula is string)
//                            {
//                                string formula = ((string)dataFormula).Trim();

//                                // There are some formulae in the Balans database that don't make any sense
//                                if (formula.EndsWith("+") || formula.EndsWith("-"))
//                                {
//                                    formula = formula.Substring(0, formula.Length - 1);
//                                }

//                                formula = formula.Replace("++", "+");

//                                if (formula != "/")
//                                {
//                                    FormulaInfo info = new FormulaInfo();

//                                    info.id = (int)dataId;
//                                    info.period = (int)dataPeriodId;
//                                    info.name = (string)dataName;
//                                    info.formula = formula;

//                                    balansFormulae[info.id] = info;
//                                }
//                            }
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }

//            // Load the same recordset into our database ('fin_report_formulae' table)
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            fields.Add("RFORMUALID", "id");
//            fields.Add("KOD_DANPER", "period_id");
//            fields.Add("RFORMULA_NAME", "name");
//            fields.Add("RFORMULALINE", "formula");

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connectionBalans))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        BulkMigrateDataReader("REP_FORMULA", "fin_report_formulae", reader, fields);

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }
//        }

//        private void MigrateBalansForms()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // S_FORM
//            fields.Clear();
//            fields.Add("KOD_FORM", "id");
//            fields.Add("PRIZ_ACTIVE", "is_active");
//            fields.Add("KOD_TYPE_ZVITNOST", "report_type_id");
//            fields.Add("KOD_TYPE_FORM", "form_type_id");
//            fields.Add("NAME_FORM", "form_name");

//            fields.Add("USER_KOREG", "modified_by");
//            fields.Add("DATE_KOREG", "modify_date");
//            fields.Add("NOMER_FORM", "form_number");
//            fields.Add("FILE_NAME", "file_name1");
//            fields.Add("KOD_DANPER", "period_id");

//            fields.Add("PRIZNAK_BALANS", "form_kind_id");
//            fields.Add("FILE_NAME2", "file_name2");
//            fields.Add("FIXCOLS", "num_fixed_cols");
//            fields.Add("FIXROWS", "num_fixed_rows");

//            MigrateTable(connectionBalans, "S_FORM", "fin_report_forms", fields, "KOD_FORM", null);

//            // S_ROW
//            fields.Clear();
//            fields.Add("NOMER_ROW", "row_index");
//            fields.Add("KOD_FORM", "form_id");
//            fields.Add("KOD_PRIZN", "is_disabled");
//            fields.Add("USER_KOREG", "modified_by");
//            fields.Add("DATE_KOREG", "modify_date");
//            fields.Add("NAME_OUTPUT", "name");

//            MigrateTable(connectionBalans, "S_ROW", "fin_report_rows", fields, "NOMER_ROW", null);

//            // S_COLUMN
//            fields.Clear();
//            fields.Add("NOMER_COL", "col_index");
//            fields.Add("KOD_FORM", "form_id");
//            fields.Add("KOD_PRIZN", "is_disabled");
//            fields.Add("USER_KOREG", "modified_by");
//            fields.Add("DATE_KOREG", "modify_date");

//            MigrateTable(connectionBalans, "S_COLUMN", "fin_report_columns", fields, "NOMER_COL", null);

//            // DISABLE_CELL
//            fields.Clear();
//            fields.Add("KOD_FORM", "form_id");
//            fields.Add("NOMER_ROW", "row_index");
//            fields.Add("NOMER_COL", "col_index");
//            fields.Add("KOD_PRIZN", "is_disabled");
//            fields.Add("NAME_CELL", "cell_text");

//            MigrateTable(connectionBalans, "DISABLE_CELL", "fin_report_cells", fields, "NOMER_ROW", null);
//        }

//        private void MigrateFinReportValues()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            fields.Add("KOD_FORM", "form_id");
//            fields.Add("NOMER_ROW", "num_row");
//            fields.Add("NOMER_COL", "num_col");
//            fields.Add("KOD_DANPER", "period_id");
//            fields.Add("ZNACHENIE", "value");
//            fields.Add("KOD_OBJ", "organization_id");
//            fields.Add("KOD_KLASIFIK", "expense_code");

//            /*
//            string querySelect = "SELECT KOD_FORM, NOMER_ROW, NOMER_COL, KOD_DANPER, ZNACHENIE, KOD_OBJ, KOD_KLASIFIK" +
//                " FROM ZNACHENIE WHERE" +
//                " (ACTIVATE = 1) AND" +
//                " (KOD_DANPER >= " + minBalansPeriodToImport.ToString() + ") AND"
//                " KOD_FORM IN (" + finFormList + ")";
//            */

//            string querySelect = "SELECT KOD_FORM, NOMER_ROW, NOMER_COL, KOD_DANPER, ZNACHENIE, KOD_OBJ, KOD_KLASIFIK" +
//                " FROM ZNACHENIE WHERE" +
//                " (ACTIVATE = 1) AND" +
//                " (KOD_DANPER >= " + minBalansPeriodToImport.ToString() + ")";

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connectionBalans))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        DataReaderAdapter adapter = new DataReaderAdapter(reader);

//                        // Map organization IDs from Balans to 1NF
//                        adapter.AddColumnMapping("KOD_OBJ", orgIdMappingBalansTo1NF);

//                        BulkMigrateDataReader("ZNACHENIE", "fin_report_values", adapter, fields);

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }
//        }

//        private void MigrateFormChanges()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            fields.Add("DATE_ZM", "change_date");
//            fields.Add("NOMER_NAKAZ", "order_num");
//            fields.Add("USER_KOREG", "modified_by");
//            fields.Add("DATE_KOREG", "modify_date");
//            fields.Add("KOD_FORM", "form_id");
//            fields.Add("KOD_OBJ", "organization_id");
//            fields.Add("PRIM", "note");
//            fields.Add("ACTIVATE", "is_active");
//            fields.Add("STATUS", "form_status");
//            fields.Add("DATE_SAVE", "save_date");

//            string querySelect = GetSelectStatement("ZMIN", fields, "KOD_ZM", null);

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connectionBalans))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        DataReaderAdapter adapter = new DataReaderAdapter(reader);

//                        // Map organization IDs from Balans to 1NF
//                        adapter.AddColumnMapping("KOD_OBJ", orgIdMappingBalansTo1NF);

//                        BulkMigrateDataReader("ZMIN", "fin_form_changes", adapter, fields);

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }
//        }

//        private void PrecalculateFormulaeForAllOrganizations()
//        {
//            logger.WriteInfo("Precalculating formulae for organizations");

//            DataTable table = new DataTable("fin_report_formula_values");

//            table.Columns.Add("organization_id", typeof(int));
//            table.Columns.Add("formula_id", typeof(int));
//            table.Columns.Add("period_id", typeof(int));
//            table.Columns.Add("value", typeof(decimal));

//            object[] rowValues = new object[4];

//            // Create a cache of form values
//            string querySelect = "SELECT organization_id, form_id, period_id, num_row, num_col, [value] FROM fin_report_values ORDER BY id";

//            try
//            {
//                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
//                {
//                    using (SqlDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            object dataOrgId = reader.IsDBNull(0) ? null : reader.GetValue(0);
//                            object dataFormId = reader.IsDBNull(1) ? null : reader.GetValue(1);
//                            object dataPeriodId = reader.IsDBNull(2) ? null : reader.GetValue(2);
//                            object dataRowNum = reader.IsDBNull(3) ? null : reader.GetValue(3);
//                            object dataColNum = reader.IsDBNull(4) ? null : reader.GetValue(4);
//                            object dataValue = reader.IsDBNull(5) ? null : reader.GetValue(5);

//                            if (dataOrgId is int &&
//                                dataFormId is int &&
//                                dataPeriodId is int &&
//                                dataRowNum is int &&
//                                dataColNum is int &&
//                                dataValue is decimal)
//                            {
//                                string key = GetValueKey((int)dataOrgId, (int)dataFormId, (int)dataPeriodId,
//                                    (int)dataRowNum, (int)dataColNum);

//                                formValueCache[key] = (decimal)dataValue;

//                                // The old algorithm, which merges all values together
//                                /*
//                                decimal prevValue = 0;

//                                if (formValueCache.TryGetValue(key, out prevValue))
//                                {
//                                    // Multiple values for one row/cell are possible. We must merge them
//                                    formValueCache[key] = prevValue + (decimal)dataValue;
//                                }
//                                else
//                                {
//                                    formValueCache.Add(key, (decimal)dataValue);
//                                }
//                                */
//                            }
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }

//            // Get all organizations from the Balans database
//            List<int> organizations = new List<int>();
//            organizations.Capacity = 3000;

//            querySelect = "SELECT id FROM organizations WHERE origin_db = 2";

//            try
//            {
//                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
//                {
//                    using (SqlDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);

//                            if (dataId is int)
//                            {
//                                organizations.Add((int)dataId);
//                            }
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }

//            // Process all the organizations
//            for (int i = 0; i < organizations.Count; i++)
//            {
//                int organizationId = organizations[i];

//                // Enumerate all formulae
//                foreach (KeyValuePair<int, FormulaInfo> pair in balansFormulae)
//                {
//                    decimal value = 0;

//                    if (GetFinFormulaValue(pair.Value, organizationId, out value))
//                    {
//                        // Add the row to the table
//                        rowValues[0] = organizationId;
//                        rowValues[1] = pair.Key;
//                        rowValues[2] = pair.Value.period;
//                        rowValues[3] = value;

//                        table.Rows.Add(rowValues);
//                    }
//                }
//            }

//            BulkMigrateDataTable("fin_report_formula_values", table);

//            /*
//            // Dump the accumulated data into SQL Server
//            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionSqlClient))
//            {
//                // Initialize the BULK copy class
//                bulkCopy.DestinationTableName = "dbo.fin_report_formula_values";
//                bulkCopy.BatchSize = 1000; // This setting is optimal for migrating BLOBs from Firebird
//                bulkCopy.BulkCopyTimeout = 1200;

//                // Setup maping between source columns and destination columns
//                for (int i = 0; i < table.Columns.Count; i++)
//                {
//                    bulkCopy.ColumnMappings.Add(table.Columns[i].ColumnName, table.Columns[i].ColumnName);
//                }

//                // Write from the source to the destination
//                try
//                {
//                    bulkCopy.WriteToServer(table);
//                }
//                catch (Exception ex)
//                {
//                    ShowSqlErrorMessageDlg("<No SQL statement available for BULK insertion>", ex.Message);

//                    // Abort migration correctly
//                    throw new MigrationAbortedException();
//                }

//                bulkCopy.Close();
//            }
//            */
//        }

//        private string GetValueKey(int organizationId, int formId, int periodId, int rowNum, int colNum)
//        {
//            return
//                organizationId.ToString() + "_@_" +
//                periodId.ToString() + "_@_" +
//                formId.ToString() + "_@_" +
//                rowNum.ToString() + "_@_" +
//                colNum.ToString();
//        }

//        private bool GetFinFormulaValue(FormulaInfo info, int organizationId1NF, out decimal value)
//        {
//            value = 0;

//            List<object> values = new List<object>();
//            bool tokensFound = false;

//            // Precalculate the formula tokens
//            for (int i = 0; i < info.tokens.Count; i++)
//            {
//                string token = info.tokens[i];

//                if (token.StartsWith("c") && token.EndsWith("c") && token.Length > 2)
//                {
//                    // This is a constant
//                    token = StripConstant(token);

//                    values.Add(decimal.Parse(token));
//                }
//                else if (token.Contains(","))
//                {
//                    // This is a value from form
//                    decimal val = 0;

//                    if (GetFinFormulaTokenValue(token, organizationId1NF, info.period, out val))
//                    {
//                        tokensFound = true;
//                    }
//                    else
//                    {
//                        val = 0;
//                    }

//                    values.Add(val);
//                }
//                else
//                {
//                    // This is a special token
//                    values.Add(token);
//                }
//            }

//            if (values.Count > 0 && tokensFound)
//            {
//                // Convert the list of values into Polish notation
//                List<object> polish = new List<object>();
//                Stack<string> stack = new Stack<string>();

//                for (int i = 0; i < values.Count; i++)
//                {
//                    object token = values[i];

//                    if (token is decimal)
//                    {
//                        polish.Add(token);
//                    }
//                    else
//                    {
//                        string op = (string)token;

//                        if (FormulaInfo.IsOperator(op[0]))
//                        {
//                            while (stack.Count > 0)
//                            {
//                                string topOp = stack.Peek();

//                                if (!FormulaInfo.IsOperator(topOp[0]))
//                                {
//                                    break;
//                                }

//                                if (FormulaInfo.GetOperatorPriority(topOp[0]) >= FormulaInfo.GetOperatorPriority(op[0]))
//                                {
//                                    polish.Add(stack.Pop());
//                                }
//                                else
//                                {
//                                    break;
//                                }
//                            }

//                            stack.Push(op);
//                        }
//                        else if (op == "(")
//                        {
//                            stack.Push(op);
//                        }
//                        else if (op == ")")
//                        {
//                            while (stack.Count > 0)
//                            {
//                                if ("(" == stack.Peek())
//                                {
//                                    stack.Pop();
//                                    break;
//                                }
//                                else
//                                {
//                                    polish.Add(stack.Pop());
//                                }
//                            }
//                        }
//                    }
//                }

//                while (stack.Count > 0)
//                {
//                    polish.Add(stack.Pop());
//                }

//                // Calculate the expression written in Reverse Polish notation
//                Stack<decimal> calculationStack = new Stack<decimal>();

//                for (int i = 0; i < polish.Count; i++)
//                {
//                    if (polish[i] is decimal)
//                    {
//                        calculationStack.Push((decimal)polish[i]);
//                    }
//                    else
//                    {
//                        string op = (string)polish[i];

//                        if (calculationStack.Count == 1)
//                        {
//                            // This should be an unary operator
//                            if (op == "+" || op == "-")
//                            {
//                                decimal operand = calculationStack.Pop();

//                                calculationStack.Push((op == "-") ? -operand : operand);
//                            }
//                            else
//                            {
//                                throw new ArgumentException("Could not calculate formula + " + info.id.ToString() + ": " + info.formula);
//                            }
//                        }
//                        else
//                        {
//                            // There must be two operands on the stack
//                            if (calculationStack.Count < 2)
//                            {
//                                throw new ArgumentException("Could not calculate formula + " + info.id.ToString() + ": " + info.formula);
//                            }

//                            decimal operandR = calculationStack.Pop();
//                            decimal operandL = calculationStack.Pop();

//                            if (op == "+")
//                            {
//                                calculationStack.Push(operandL + operandR);
//                            }
//                            else if (op == "-")
//                            {
//                                calculationStack.Push(operandL - operandR);
//                            }
//                            else if (op == "*")
//                            {
//                                calculationStack.Push(operandL * operandR);
//                            }
//                            else if (op == "/")
//                            {
//                                if (operandR == 0)
//                                {
//                                    calculationStack.Push((decimal)0);
//                                }
//                                else
//                                {
//                                    calculationStack.Push(operandL / operandR);
//                                }
//                            }
//                            else
//                            {
//                                throw new ArgumentException("Could not calculate formula " +
//                                    info.id.ToString() + "[ " + info.formula + " ] - Unknown token " + op);
//                            }
//                        }
//                    }
//                }

//                if (calculationStack.Count > 0)
//                {
//                    value = calculationStack.Pop();
//                    return true;
//                }
//            }

//            return false;
//        }

//        private string StripConstant(string cons)
//        {
//            while (cons.StartsWith("c"))
//            {
//                cons = cons.Substring(1);
//            }

//            while (cons.EndsWith("c"))
//            {
//                cons = cons.Substring(0, cons.Length - 1);
//            }

//            return cons;
//        }

//        private bool GetFinFormulaTokenValue(string token, int organizationId1NF, int periodId, out decimal value)
//        {
//            string[] valueInfo = token.Split(FormulaInfo.ValueTokenSeparators);

//            if (valueInfo.Length >= 3)
//            {
//                int formId = int.Parse(valueInfo[0]);
//                int columnId = int.Parse(valueInfo[1]);
//                int rowId = int.Parse(valueInfo[2]);

//                return GetFinFormulaTokenValue(formId, rowId, columnId, organizationId1NF, periodId, out value);
//            }

//            value = 0;
//            return false;
//        }

//        private bool GetFinFormulaTokenValue(int formId, int rowId, int columnId, int organizationId1NF, int periodId, out decimal value)
//        {
//            string key = GetValueKey(organizationId1NF, formId, periodId, rowId, columnId);

//            if (formValueCache.TryGetValue(key, out value))
//            {
//                return true;
//            }

//            value = 0;
//            return false;
//        }

//        #endregion (Migration of financial reports)

//        #region Custom formulae

//        private void InjectBalansCustomFormulae()
//        {
//            // Clear income
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaClearIncome + "01.04.2010", "3373,3,24,+3372,3,45,-3372,3,46,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaClearIncome + "01.07.2010", "3401,3,24,+3400,3,45,-3400,3,46,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaClearIncome + "01.10.2010", "3425,3,24,+3424,3,45,-3424,3,46,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaClearIncome + "01.01.2011", "3484,3,22,+3483,3,45,-3483,3,46,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaClearIncome + "01.04.2011", "3588,3,22,+3597,3,45,-3597,3,46,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaClearIncome + "01.01.2011", "3633,3,17,+3635,3,46,-3635,3,47,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaClearIncome + "01.04.2012", "3840,3,17,+3839,3,45,-3839,3,46,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaClearIncome + "01.07.2012", "3879,3,17,+3878,3,45,-3878,3,46,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaClearIncome + "01.10.2012", "3897,3,17,+3896,3,45,-3896,3,46,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaClearIncome + "01.01.2013", "3938,3,17,+3937,3,45,-3937,3,46,");

//            /* Відрахування до бюджету */
//            /*
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaBudgPayments2010Q1, "3365,4,65,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaBudgPayments2010Q2, "3393,4,65,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaBudgPayments2010Q3, "3419,4,65,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaBudgPayments2010, "3559,4,65,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaBudgPayments2011Q1, "3582,4,64,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaBudgPayments2011Q2, "3636,4,64,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaBudgPayments2011Q3, "3651,4,64,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaBudgPayments2011, "3691,4,64,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaBudgPayments2012Q1, "3834,4,64,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaBudgPayments2012Q2, "3873,4,64,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaBudgPayments2012Q3, "3891,4,64,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaBudgPayments2012, "3932,4,64,");
//            */

//            /* Сукупний дохід (без ПДВ) */
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaTotalIncome2012,
//                "3937,3,9,+3937,3,14,+3937,3,23,+3937,3,24,+3937,3,25,+3938,3,8,");

//            // Cost of Actives
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaCostActives2010Q1, "3367,4,53,+3371,4,34,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaCostActives2010Q2, "3395,4,53,+3399,4,34,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaCostActives2010Q3, "3420,4,53,+3423,4,34,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaCostActives2010, "3478,4,54,+3482,4,34,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaCostActives2011Q1, "3583,4,54,+3586,4,34,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaCostActives2011Q2, "3634,4,54,+3632,4,35,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaCostActives2011Q3, "3653,4,54,+3656,4,35,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaCostActives2011, "3666,4,54,+3694,4,35,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaCostActives2012Q1, "3835,4,54,+3838,4,35,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaCostActives2012Q2, "3874,4,54,+3877,4,35,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaCostActives2012Q3, "3892,4,54,+3895,4,35,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaCostActives2012, "3933,4,54,+3936,4,35,");

//            // Debit debt
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaDebitDebt + "01.04.2010",
//                "3367,4,20,+3367,4,34,+3367,4,36,+3367,4,40,+3367,4,41,+3367,4,42,+3367,4,43,+3367,4,44,+3371,4,21,+3371,4,25,+3371,4,24,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaDebitDebt + "01.07.2010",
//                "3395,4,20,+3395,4,34,+3395,4,36,+3395,4,40,+3395,4,41,+3395,4,42,+3395,4,43,+3395,4,44,+3399,4,21,+3399,4,25,+3399,4,24,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaDebitDebt + "01.10.2010",
//                "3420,4,20,+3420,4,34,+3420,4,36,+3420,4,40,+3420,4,41,+3420,4,42,+3420,4,43,+3420,4,44,+3423,4,21,+3423,4,25,+3423,4,24,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaDebitDebt + "01.01.2011",
//                "3478,4,20,+3478,4,34,+3478,4,36,+3478,4,40,+3478,4,41,+3478,4,42,+3478,4,43,+3478,4,44,+3482,4,21,+3482,4,25,+3482,4,24,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaDebitDebt + "01.04.2011",
//                "3583,4,20,+3583,4,34,+3583,4,36,+3583,4,40,+3583,4,41,+3583,4,42,+3583,4,43,+3583,4,44,+3586,4,21,+3586,4,25,+3586,4,24,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaDebitDebt + "01.07.2011",
//                "3634,4,20,+3634,4,34,+3634,4,36,+3634,4,40,+3634,4,41,+3634,4,42,+3634,4,43,+3634,4,44,+3632,4,21,+3632,4,25,+3632,4,24,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaDebitDebt + "01.10.2011",
//                "3653,4,20,+3653,4,34,+3653,4,36,+3653,4,40,+3653,4,41,+3653,4,42,+3653,4,43,+3653,4,44,+3656,4,21,+3656,4,25,+3656,4,24,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaDebitDebt + "01.01.2012",
//                "3666,4,20,+3666,4,34,+3666,4,36,+3666,4,40,+3666,4,41,+3666,4,42,+3666,4,43,+3666,4,44,+3694,4,21,+3694,4,25,+3694,4,24,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaDebitDebt + "01.04.2012",
//                "3835,4,20,+3835,4,34,+3835,4,36,+3835,4,40,+3835,4,41,+3835,4,42,+3835,4,43,+3835,4,44,+3838,4,21,+3838,4,25,+3838,4,24,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaDebitDebt + "01.07.2012",
//                "3874,4,20,+3874,4,34,+3874,4,36,+3874,4,40,+3874,4,41,+3874,4,42,+3874,4,43,+3874,4,44,+3877,4,21,+3877,4,25,+3877,4,24,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaDebitDebt + "01.10.2012",
//                "3892,4,20,+3892,4,34,+3892,4,36,+3892,4,40,+3892,4,41,+3892,4,42,+3892,4,43,+3892,4,44,+3895,4,21,+3895,4,25,+3895,4,24,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaDebitDebt + "01.01.2013",
//                "3933,4,20,+3933,4,34,+3933,4,36,+3933,4,40,+3933,4,41,+3933,4,42,+3933,4,43,+3933,4,44,+3936,4,21,+3936,4,25,+3936,4,24,");

//            // Long term debit debt
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaLongDebitDebt + "01.04.2010", "3367,4,20,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaLongDebitDebt + "01.07.2010", "3395,4,20,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaLongDebitDebt + "01.10.2010", "3420,4,20,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaLongDebitDebt + "01.01.2011", "3478,4,20,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaLongDebitDebt + "01.04.2011", "3583,4,20,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaLongDebitDebt + "01.07.2011", "3634,4,20,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaLongDebitDebt + "01.10.2011", "3653,4,20,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaLongDebitDebt + "01.01.2012", "3666,4,20,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaLongDebitDebt + "01.04.2012", "3835,4,20,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaLongDebitDebt + "01.07.2012", "3874,4,20,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaLongDebitDebt + "01.10.2012", "3892,4,20,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaLongDebitDebt + "01.01.2013", "3933,4,20,");

//            // Credit debt
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaCreditDebt + "01.04.2010", "3367,4,79,+3367,4,95,+3371,4,44,+3371,4,55,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaCreditDebt + "01.07.2010", "3395,4,79,+3395,4,95,+3399,4,44,+3399,4,55,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaCreditDebt + "01.10.2010", "3420,4,79,+3420,4,95,+3423,4,44,+3423,4,55,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaCreditDebt + "01.01.2011", "3478,4,80,+3478,4,96,+3482,4,44,+3482,4,55,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaCreditDebt + "01.04.2011", "3583,4,80,+3583,4,96,+3586,4,44,+3586,4,55,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaCreditDebt + "01.07.2011", "3634,4,80,+3634,4,96,+3632,4,45,+3632,4,55,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaCreditDebt + "01.10.2011", "3653,4,80,+3653,4,96,+3656,4,45,+3656,4,55,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaCreditDebt + "01.01.2012", "3666,4,80,+3666,4,96,+3694,4,45,+3694,4,55,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaCreditDebt + "01.04.2012", "3835,4,80,+3835,4,96,+3838,4,45,+3838,4,55,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaCreditDebt + "01.07.2012", "3874,4,80,+3874,4,96,+3877,4,45,+3877,4,55,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaCreditDebt + "01.10.2012", "3892,4,80,+3892,4,96,+3895,4,45,+3895,4,55,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaCreditDebt + "01.01.2013", "3933,4,80,+3933,4,96,+3936,4,45,+3936,4,55,");

//            // Dovgostrokovi zobovjazannia
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaDovgZobov + "01.04.2010", "3367,4,79,+3371,4,44,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaDovgZobov + "01.07.2010", "3395,4,79,+3399,4,44,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaDovgZobov + "01.10.2010", "3420,4,79,+3423,4,44,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaDovgZobov + "01.01.2011", "3478,4,80,+3482,4,44,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaDovgZobov + "01.04.2011", "3583,4,80,+3586,4,44,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaDovgZobov + "01.07.2011", "3634,4,80,+3632,4,45,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaDovgZobov + "01.10.2011", "3653,4,80,+3656,4,45,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaDovgZobov + "01.01.2012", "3666,4,80,+3694,4,45,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaDovgZobov + "01.04.2012", "3835,4,80,+3838,4,45,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaDovgZobov + "01.07.2012", "3874,4,80,+3877,4,45,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaDovgZobov + "01.10.2012", "3892,4,80,+3895,4,45,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaDovgZobov + "01.01.2013", "3933,4,80,+3936,4,45,");

//            // Dovgostrokovi credity bankiv
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaDovgBankCredit + "01.04.2010", "3367,4,75,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaDovgBankCredit + "01.07.2010", "3395,4,75,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaDovgBankCredit + "01.10.2010", "3420,4,75,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaDovgBankCredit + "01.01.2011", "3478,4,76,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaDovgBankCredit + "01.04.2011", "3583,4,76,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaDovgBankCredit + "01.07.2011", "3634,4,76,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaDovgBankCredit + "01.10.2011", "3653,4,76,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaDovgBankCredit + "01.01.2012", "3666,4,76,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaDovgBankCredit + "01.04.2012", "3835,4,76,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaDovgBankCredit + "01.07.2012", "3874,4,76,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaDovgBankCredit + "01.10.2012", "3892,4,76,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaDovgBankCredit + "01.01.2013", "3933,4,76,");

//            // Potochni zobovjazannia
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaPotochZobov + "01.04.2010", "3367,4,95,+3371,4,55,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaPotochZobov + "01.07.2010", "3395,4,95,+3399,4,55,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaPotochZobov + "01.10.2010", "3420,4,95,+3423,4,55,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaPotochZobov + "01.01.2011", "3478,4,96,+3482,4,55,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaPotochZobov + "01.04.2011", "3583,4,96,+3586,4,55,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaPotochZobov + "01.07.2011", "3634,4,96,+3632,4,55,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaPotochZobov + "01.10.2011", "3653,4,96,+3656,4,55,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaPotochZobov + "01.01.2012", "3666,4,96,+3694,4,55,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaPotochZobov + "01.04.2012", "3835,4,96,+3838,4,55,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaPotochZobov + "01.07.2012", "3874,4,96,+3877,4,55,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaPotochZobov + "01.10.2012", "3892,4,96,+3895,4,55,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaPotochZobov + "01.01.2013", "3933,4,96,+3936,4,55,");

//            // Korotkostrokovi credity bankiv
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaShortBankCredit + "01.04.2010", "3367,4,81,+3371,4,46,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaShortBankCredit + "01.07.2010", "3395,4,81,+3399,4,46,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaShortBankCredit + "01.10.2010", "3420,4,81,+3423,4,46,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaShortBankCredit + "01.01.2011", "3478,4,82,+3482,4,46,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaShortBankCredit + "01.04.2011", "3583,4,82,+3586,4,46,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaShortBankCredit + "01.07.2011", "3634,4,82,+3632,4,47,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaShortBankCredit + "01.10.2011", "3653,4,82,+3656,4,47,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaShortBankCredit + "01.01.2012", "3666,4,82,+3694,4,47,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaShortBankCredit + "01.04.2012", "3835,4,82,+3838,4,47,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaShortBankCredit + "01.07.2012", "3874,4,82,+3877,4,47,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaShortBankCredit + "01.10.2012", "3892,4,82,+3895,4,47,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaShortBankCredit + "01.01.2013", "3933,4,82,+3936,4,47,");

//            // Zagalnyy obsyag dohodiv
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameQ1, 2010),
//                "3372,3,9,+3372,3,14,+3372,3,23,+3372,3,24,+3372,3,25,+3372,3,36,+3372,3,41,+3373,3,10,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameQ2, 2010),
//                "3400,3,9,+3400,3,14,+3400,3,23,+3400,3,24,+3400,3,25,+3400,3,36,+3400,3,41,+3401,3,10,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameQ3, 2010),
//                "3424,3,9,+3424,3,14,+3424,3,23,+3424,3,24,+3424,3,25,+3424,3,36,+3424,3,41,+3425,3,10,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameYear, 2010),
//                "3483,3,9,+3483,3,14,+3483,3,23,+3483,3,24,+3483,3,25,+3483,3,36,+3483,3,41,+3484,3,9,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameQ1, 2011),
//                "3597,3,9,+3597,3,14,+3597,3,23,+3597,3,24,+3597,3,25,+3597,3,36,+3597,3,41,+3588,3,9,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameQ2, 2011),
//                "3635,3,9,+3635,3,15,+3635,3,24,+3635,3,25,+3635,3,26,+3635,3,37,+3635,3,42,+3633,3,8,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameQ3, 2011),
//                "3663,3,9,+3663,3,15,+3663,3,24,+3663,3,25,+3663,3,26,+3663,3,37,+3663,3,42,+3657,3,8,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameYear, 2011),
//                "3692,3,9,+3692,3,14,+3692,3,23,+3692,3,24,+3692,3,25,+3692,3,36,+3692,3,41,+3695,3,8,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameQ1, 2012),
//                "3839,3,9,+3839,3,14,+3839,3,23,+3839,3,24,+3839,3,25,+3839,3,36,+3839,3,41,+3840,3,8,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameQ2, 2012),
//                "3878,3,9,+3878,3,14,+3878,3,23,+3878,3,24,+3878,3,25,+3878,3,36,+3878,3,41,+3879,3,8,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameQ3, 2012),
//                "3896,3,9,+3896,3,14,+3896,3,23,+3896,3,24,+3896,3,25,+3896,3,36,+3896,3,41,+3897,3,8,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaTotalDohod + string.Format(Properties.Resources.FormulaNameYear, 2012),
//                "3937,3,9,+3937,3,14,+3937,3,23,+3937,3,24,+3937,3,25,+3937,3,36,+3937,3,41,+3938,3,8,");

//            // Pributok-zbitok fact.
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameQ1, 2010), "3365,6,63,-3365,6,64,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameQ2, 2010), "3393,6,63,-3393,6,64,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameQ3, 2010), "3419,6,63,-3419,6,64,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameYear, 2010), "3559,6,63,-3559,6,64,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameQ1, 2011), "3582,6,62,-3582,6,63,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameQ2, 2011), "3636,6,62,-3636,6,63,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameQ3, 2011), "3651,6,62,-3651,6,63,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameYear, 2011), "3691,6,62,-3691,6,63,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameQ1, 2012), "3834,6,62,-3834,6,63,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameQ2, 2012), "3873,6,62,-3873,6,63,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameQ3, 2012), "3891,6,62,-3891,6,63,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaPributokZbitokFact +
//                string.Format(Properties.Resources.FormulaNameYear, 2012), "3932,6,62,-3932,6,63,");

//            // Serednya kilkist pracuu4ih
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameQ1, 2010), "3375,4,3,");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameQ2, 2010), "3398,4,3,");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameQ3, 2010), "3422,4,3,");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameYear, 2010), "3481,4,3,");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameQ1, 2011), "3585,4,3,");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameQ2, 2011), "3638,4,3,");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameQ3, 2011), "3655,4,3,");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameYear, 2011), "3668,4,3,");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameQ1, 2012), "3837,4,3,");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameQ2, 2012), "3876,4,3,");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameQ3, 2012), "3894,4,3,");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaAvgNumWorkers +
//                string.Format(Properties.Resources.FormulaNameYear, 2012), "3935,4,3,");

//            // Serednya zar. plata
//            InjectFormula((--customFormulaIdSeed), 64, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameQ1, 2010), "c1000c*3375,4,4,/3375,4,3,/c3c");
//            InjectFormula((--customFormulaIdSeed), 65, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameQ2, 2010), "c1000c*3398,4,4,/3398,4,3,/c6c");
//            InjectFormula((--customFormulaIdSeed), 66, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameQ3, 2010), "c1000c*3422,4,4,/3422,4,3,/c9c");
//            InjectFormula((--customFormulaIdSeed), 69, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameYear, 2010), "c1000c*3481,4,4,/3481,4,3,/c12c");

//            InjectFormula((--customFormulaIdSeed), 71, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameQ1, 2011), "c1000c*3585,4,4,/3585,4,3,/c3c");
//            InjectFormula((--customFormulaIdSeed), 72, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameQ2, 2011), "c1000c*3638,4,4,/3638,4,3,/c6c");
//            InjectFormula((--customFormulaIdSeed), 73, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameQ3, 2011), "c1000c*3655,4,4,/3655,4,3,/c9c");
//            InjectFormula((--customFormulaIdSeed), 74, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameYear, 2011), "c1000c*3668,4,4,/3668,4,3,/c12c");

//            InjectFormula((--customFormulaIdSeed), 77, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameQ1, 2012), "c1000c*3837,4,4,/3837,4,3,/c3c");
//            InjectFormula((--customFormulaIdSeed), 78, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameQ2, 2012), "c1000c*3876,4,4,/3876,4,3,/c6c");
//            InjectFormula((--customFormulaIdSeed), 79, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameQ3, 2012), "c1000c*3894,4,4,/3894,4,3,/c9c");
//            InjectFormula((--customFormulaIdSeed), 82, Properties.Resources.FormulaAvgSalary +
//                string.Format(Properties.Resources.FormulaNameYear, 2012), "c1000c*3935,4,4,/3935,4,3,/c12c");
//        }

//        private void InjectFormula(int id, int period, string name, string formula)
//        {
//            FormulaInfo info = new FormulaInfo();

//            info.id = id;
//            info.period = period;
//            info.name = EIS + name;
//            info.formula = formula;

//            balansFormulae[info.id] = info;

//            // Insert the formula into 'fin_report_formulae' table
//            InsertFormulaIntoSqlServer(id, period, name, formula);
//        }

//        private void InsertFormulaIntoSqlServer(int id, int period, string name, string formula)
//        {
//            // Insert the formula into 'fin_report_formulae' table
//            Dictionary<string, object> values = new Dictionary<string, object>();

//            values["id"] = id;
//            values["period_id"] = period;
//            values["name"] = EIS + name;
//            values["formula"] = formula;

//            string insertStatement = GetInsertStatement("fin_report_formulae", values);

//            try
//            {
//                using (SqlCommand cmd = new SqlCommand(insertStatement, connectionSqlClient))
//                {
//                    cmd.ExecuteNonQuery();
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(insertStatement, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }
//        }

//        #endregion (Custom formulae)

//        #region Budget payment rate calculation

//        /// <summary>
//        /// A cache of formula values relatedto budget payments
//        /// </summary>
//        private DataTable tableBudgetPaymentFormulaValues = new DataTable();

//        /// <summary>
//        /// A temporary bufer to insert rows into tableBudgetPaymentFormulaValues table
//        /// </summary>
//        private object[] rowBudgetPaymentFormulaValues = new object[4];

//        private void CalculateBudgetPaymentRates()
//        {
//            // Prepare the list of Fin Plans (key is a form ID, value is a period ID)
//            Dictionary<int, int> finPlans = new Dictionary<int, int>();
//            Dictionary<int, int> finPlanPaymentRows = new Dictionary<int, int>();
//            Dictionary<int, int> finPlan1stQuarterCol = new Dictionary<int, int>();
//            Dictionary<int, int> finPlan2ndQuarterCol = new Dictionary<int, int>();
//            Dictionary<int, int> finPlan3rdQuarterCol = new Dictionary<int, int>();
//            Dictionary<int, int> finPlan4thQuarterCol = new Dictionary<int, int>();

//            FindFinPlans(finPlans, finPlanPaymentRows, finPlan1stQuarterCol, finPlan2ndQuarterCol, finPlan3rdQuarterCol, finPlan4thQuarterCol);

//            // Prepare the list of Fin Results (key is a form ID, value is a period ID)
//            Dictionary<int, int> finResults = new Dictionary<int, int>();
//            Dictionary<int, int> finResultIncomeRows = new Dictionary<int, int>();

//            FindFinResults(finResults, finResultIncomeRows);

//            // We need to perform this calculation for all periods
//            string query = "SELECT id, period_start FROM fin_report_periods WHERE is_deleted = 0";

//            // select, filter, sort periods. define prev periods for each period
//            IList<ReportPeriod> periodsList = new List<ReportPeriod>();

//            using (SqlCommand cmdPeriods = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader r = cmdPeriods.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                        {
//                            periodsList.Add(new ReportPeriod { Id = r.GetInt32(0), Date = r.GetDateTime(1) });
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            var goodMonthes = new List<int>() { 1, 4, 7, 10 };

//            var filteredPeriodList = from period in periodsList
//                                     where period.Date.Day == 1
//                                     && goodMonthes.Contains(period.Date.Month)
//                                     select period;

//            var sortedPeriodList = filteredPeriodList.OrderBy(p => p.Date).ToList();

//            foreach (var curPeriod in sortedPeriodList)
//            {
//                IEnumerable<ReportPeriod> prevPeriods = null;
//                if (curPeriod.Date.Month != 4)
//                {
//                    prevPeriods = from period in sortedPeriodList
//                                  where period.Date == curPeriod.Date.AddMonths(-3)
//                                  select period;
//                }                
//                if (prevPeriods != null)
//                    curPeriod.PreviousPeriods = prevPeriods.ToList();
//            }

//            // We need to perform this calculation for all organizations
//            Dictionary<int, KeyValuePair<int, int>> organizations = new Dictionary<int, KeyValuePair<int, int>>();

//            query = "SELECT id, old_industry_id, old_occupation_id FROM organizations WHERE origin_db = 2 AND is_deleted = 0";

//            using (SqlCommand cmdOrg = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader r = cmdOrg.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0))
//                        {
//                            int oldIndustryId = r.IsDBNull(1) ? 0 : r.GetInt32(1);
//                            int oldOccupationId = r.IsDBNull(2) ? 0 : r.GetInt32(2);

//                            organizations.Add(r.GetInt32(0), new KeyValuePair<int, int>(oldIndustryId, oldOccupationId));
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            // Prepare a cache for formula values, to use BULK insert
//            tableBudgetPaymentFormulaValues.Columns.Add("organization_id", typeof(int));
//            tableBudgetPaymentFormulaValues.Columns.Add("formula_id", typeof(int));
//            tableBudgetPaymentFormulaValues.Columns.Add("period_id", typeof(int));
//            tableBudgetPaymentFormulaValues.Columns.Add("value", typeof(decimal));

//            // Calculate for each period
//            HashSet<int> budgetPaymentFormulae = new HashSet<int>();
//            Dictionary<int, decimal> rates = new Dictionary<int, decimal>();

//            foreach (var period in sortedPeriodList)
//            {
//                rates.Clear();

//                // Get budget payment rates for each industry
//                Dictionary<string, decimal> industryRates = new Dictionary<string, decimal>();

//                query = "SELECT old_industry_id, old_occupation_id, payments_rate FROM fin_budget_rate_by_industry WHERE period_id = @pid";

//                using (SqlCommand cmdIndustryRate = new SqlCommand(query, connectionSqlClient))
//                {
//                    cmdIndustryRate.Parameters.Add(new SqlParameter("pid", period.Id));

//                    using (SqlDataReader r = cmdIndustryRate.ExecuteReader())
//                    {
//                        while (r.Read())
//                        {
//                            if (!r.IsDBNull(0) && !r.IsDBNull(2))
//                            {
//                                int industryId = r.GetInt32(0);
//                                int occupationId = r.IsDBNull(1) ? 0 : r.GetInt32(1);

//                                if (occupationId > 0)
//                                {
//                                    industryRates[industryId.ToString() + "_" + occupationId.ToString()] = r.GetDecimal(2);
//                                }
//                                else
//                                {
//                                    industryRates[industryId.ToString()] = r.GetDecimal(2);
//                                }
//                            }
//                        }

//                        r.Close();
//                    }
//                }

//                // Get budget payment rates for individual organizations
//                Dictionary<int, decimal> orgRates = new Dictionary<int, decimal>();

//                query = "SELECT organization_id, payments_rate FROM fin_budget_rate_by_org WHERE period_id = @pid";

//                using (SqlCommand cmdOrgRate = new SqlCommand(query, connectionSqlClient))
//                {
//                    cmdOrgRate.Parameters.Add(new SqlParameter("pid", period.Id));

//                    using (SqlDataReader r = cmdOrgRate.ExecuteReader())
//                    {
//                        while (r.Read())
//                        {
//                            if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                            {
//                                orgRates[r.GetInt32(0)] = r.GetDecimal(1);
//                            }
//                        }

//                        r.Close();
//                    }
//                }

//                // Calculate only if we have some data for this period
//                if (industryRates.Count > 0 || orgRates.Count > 0)
//                {
//                    // Generate ID and name for the new formula
//                    int formulaId = (--customFormulaIdSeed);
//                    string formulaName = string.Format(Properties.Resources.FormulaBudgPaymentsRate, period.Date.ToString("dd.MM.yyyy"));

//                    // Save the formula ID and Name to SQL Server
//                    InsertFormulaIntoSqlServer(formulaId, period.Id, formulaName, "");

//                    // Calculate the payment rate for each organization
//                    foreach (KeyValuePair<int, KeyValuePair<int, int>> org in organizations)
//                    {
//                        int organizationId = org.Key;
//                        int industryId = org.Value.Key;
//                        int occupationId = org.Value.Value;

//                        decimal rate = 0m;
//                        bool createEntry = false;

//                        if (orgRates.TryGetValue(organizationId, out rate))
//                        {
//                            createEntry = true;
//                        }
//                        else
//                        {
//                            // Check for specific payment rate for OCCUPATION and INDUSTRY
//                            string key = industryId.ToString() + "_" + occupationId.ToString();

//                            if (industryRates.TryGetValue(key, out rate))
//                            {
//                                createEntry = true;
//                            }
//                            else
//                            {
//                                // Check for the general payment rate forINDUSTRY
//                                key = industryId.ToString();

//                                if (industryRates.TryGetValue(key, out rate))
//                                {
//                                    createEntry = true;
//                                }
//                            }
//                        }

//                        if (createEntry)
//                        {
//                            // Save the formula value
//                            rowBudgetPaymentFormulaValues[0] = org.Key;
//                            rowBudgetPaymentFormulaValues[1] = formulaId;
//                            rowBudgetPaymentFormulaValues[2] = period.Id;
//                            rowBudgetPaymentFormulaValues[3] = 1000m * rate;

//                            tableBudgetPaymentFormulaValues.Rows.Add(rowBudgetPaymentFormulaValues);

//                            // Save the payment ratefor this organization for future processing
//                            rates[org.Key] = rate;
//                        }
//                    }
//                }

//                // Calculate the actual budget payments
//                if (rates.Count > 0)
//                {
//                    CalculateBudgetPayments(period, sortedPeriodList, organizations, finPlans, finPlanPaymentRows,
//                        finPlan1stQuarterCol, finPlan2ndQuarterCol, finPlan3rdQuarterCol, finPlan4thQuarterCol,
//                        finResults, finResultIncomeRows, rates, budgetPaymentFormulae);
//                }
//            }

//            // Dump the accumulated values
//            BulkMigrateDataTable("fin_report_formula_values", tableBudgetPaymentFormulaValues);

//            // Save ID of all auto-generated budget payment formulae into a special table
//            foreach (int formulaId in budgetPaymentFormulae)
//            {
//                query = "INSERT INTO fin_budget_formula (formula_id) VALUES (@fid)";

//                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                {
//                    cmd.Parameters.Add(new SqlParameter("fid", formulaId));
//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }

//        private void FindFinPlans(Dictionary<int, int> finPlans, Dictionary<int, int> finPlanPaymentRows,
//            Dictionary<int, int> finPlan1stQuarterCol,
//            Dictionary<int, int> finPlan2ndQuarterCol,
//            Dictionary<int, int> finPlan3rdQuarterCol,
//            Dictionary<int, int> finPlan4thQuarterCol)
//        {
//            // Get all Fin Plans
//            string query = "SELECT id, period_id FROM fin_report_forms WHERE UPPER(form_name) like @finplan AND RTRIM(LTRIM(form_number)) = '01' AND UPPER(is_active) <> 'N'";

//            using (SqlCommand cmdFinPlans = new SqlCommand(query, connectionSqlClient))
//            {
//                cmdFinPlans.Parameters.Add(new SqlParameter("finplan", Properties.Resources.FinPlan));

//                using (SqlDataReader r = cmdFinPlans.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                        {
//                            finPlans[r.GetInt32(0)] = r.GetInt32(1);
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            // Prepare the list of Form IDs, to get all rows of Fin Plans with a single query
//            string formIDs = "";

//            foreach (KeyValuePair<int, int> plan in finPlans)
//            {
//                if (formIDs.Length > 0)
//                {
//                    formIDs += ", ";
//                }

//                formIDs += plan.Key.ToString();
//            }

//            // Get all matching rows (by row name) for all Fin Plans
//            query = "SELECT form_id, row_index FROM fin_report_rows WHERE form_id IN (" + formIDs + ") AND UPPER(name) like @vidr1 AND UPPER(name) like @vidr2 AND UPPER(name) like @vidr3";

//            using (SqlCommand cmdFinPlanRows = new SqlCommand(query, connectionSqlClient))
//            {
//                cmdFinPlanRows.Parameters.Add(new SqlParameter("vidr1", Properties.Resources.FinPlanVidrahuvannia1));
//                cmdFinPlanRows.Parameters.Add(new SqlParameter("vidr2", Properties.Resources.FinPlanVidrahuvannia2));
//                cmdFinPlanRows.Parameters.Add(new SqlParameter("vidr3", Properties.Resources.FinPlanVidrahuvannia3));

//                using (SqlDataReader r = cmdFinPlanRows.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                        {
//                            finPlanPaymentRows[r.GetInt32(0)] = r.GetInt32(1);
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            // Get all fin plan columns that represent particular quarters of the year
//            query = "SELECT form_id, col_index, cell_text FROM fin_report_cells WHERE form_id IN (" + formIDs + ") AND row_index = 1 AND (UPPER(cell_text) LIKE @quart)";

//            using (SqlCommand cmdFinPlanRows = new SqlCommand(query, connectionSqlClient))
//            {
//                cmdFinPlanRows.Parameters.Add(new SqlParameter("quart", Properties.Resources.FinPlanQuarterCol));

//                using (SqlDataReader r = cmdFinPlanRows.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1) && !r.IsDBNull(2))
//                        {
//                            int formId = r.GetInt32(0);
//                            int colIndex = r.GetInt32(1);
//                            string cellText = r.GetString(2);

//                            cellText = cellText.ToUpper();
//                            cellText = cellText.Replace((char)1030, 'I');

//                            if (cellText.Contains("IV"))
//                            {
//                                finPlan4thQuarterCol[formId] = colIndex;
//                            }
//                            else if (cellText.Contains("III"))
//                            {
//                                finPlan3rdQuarterCol[formId] = colIndex;
//                            }
//                            else if (cellText.Contains("II"))
//                            {
//                                finPlan2ndQuarterCol[formId] = colIndex;
//                            }
//                            else if (cellText.Contains("I"))
//                            {
//                                finPlan1stQuarterCol[formId] = colIndex;
//                            }
//                        }
//                    }

//                    r.Close();
//                }
//            }
//        }

//        private void FindFinResults(Dictionary<int, int> finResults, Dictionary<int, int> finResultIncomeRows)
//        {
//            Dictionary<int, int> finResultsForm2 = new Dictionary<int, int>();
//            Dictionary<int, int> finResultsForm2M = new Dictionary<int, int>();

//            // Get all Fin Results (Form 2)
//            string query = @"SELECT id, period_id FROM fin_report_forms WHERE form_name like @fname1 AND form_name like @fname2 AND
//                UPPER(RTRIM(LTRIM(form_number))) = @fnum1 AND UPPER(is_active) <> 'N'";

//            using (SqlCommand cmdFinResults = new SqlCommand(query, connectionSqlClient))
//            {
//                cmdFinResults.Parameters.Add(new SqlParameter("fname1", Properties.Resources.FinResults1));
//                cmdFinResults.Parameters.Add(new SqlParameter("fname2", Properties.Resources.FinResults2));
//                cmdFinResults.Parameters.Add(new SqlParameter("fnum1", Properties.Resources.FinResultsForm1));

//                using (SqlDataReader r = cmdFinResults.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                        {
//                            finResultsForm2[r.GetInt32(0)] = r.GetInt32(1);
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            // Get all Fin Results (Form 2-m)
//             query = @"SELECT id, period_id FROM fin_report_forms WHERE form_name like @fname1 AND form_name like @fname2 AND
//                UPPER(RTRIM(LTRIM(form_number))) = @fnum2 AND UPPER(is_active) <> 'N'";

//            using (SqlCommand cmdFinResults = new SqlCommand(query, connectionSqlClient))
//            {
//                cmdFinResults.Parameters.Add(new SqlParameter("fname1", Properties.Resources.FinResults1));
//                cmdFinResults.Parameters.Add(new SqlParameter("fname2", Properties.Resources.FinResults2));
//                cmdFinResults.Parameters.Add(new SqlParameter("fnum2", Properties.Resources.FinResultsForm2));

//                using (SqlDataReader r = cmdFinResults.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                        {
//                            finResultsForm2M[r.GetInt32(0)] = r.GetInt32(1);
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            // Merge both dictionaries into one result
//            foreach (KeyValuePair<int, int> finRes in finResultsForm2)
//            {
//                finResults.Add(finRes.Key, finRes.Value);
//            }

//            foreach (KeyValuePair<int, int> finRes in finResultsForm2M)
//            {
//                finResults.Add(finRes.Key, finRes.Value);
//            }

//            // Prepare the list of Form IDs, to get all rows of Fin Results with a single query
//            string formIDs2 = "";
//            string formIDs2m = "";

//            foreach (KeyValuePair<int, int> finRes in finResultsForm2)
//            {
//                if (formIDs2.Length > 0)
//                {
//                    formIDs2 += ", ";
//                }

//                formIDs2 += finRes.Key.ToString();
//            }

//            foreach (KeyValuePair<int, int> finRes in finResultsForm2M)
//            {
//                if (formIDs2m.Length > 0)
//                {
//                    formIDs2m += ", ";
//                }

//                formIDs2m += finRes.Key.ToString();
//            }

//            // Get all matching rows (by row name) for all 'Form 2' financial results
//            query = @"SELECT
//                rowsA.form_id,
//                rowsB.row_index
//            FROM
//                fin_report_rows rowsA
//                LEFT OUTER JOIN fin_report_rows rowsB ON rowsB.form_id = rowsA.form_id AND rowsB.row_index = rowsA.row_index + 1
//            WHERE
//                rowsA.form_id IN ($) AND
//                UPPER(rowsA.name) like @res1 AND
//                UPPER(rowsB.name) like @res2";

//            query = query.Replace("$", formIDs2);

//            using (SqlCommand cmdFinPlanRows = new SqlCommand(query, connectionSqlClient))
//            {
//                cmdFinPlanRows.Parameters.Add(new SqlParameter("res1", Properties.Resources.FinResultsPureIncome1));
//                cmdFinPlanRows.Parameters.Add(new SqlParameter("res2", Properties.Resources.FinResultsPureIncome2));

//                using (SqlDataReader r = cmdFinPlanRows.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                        {
//                            int formId = r.GetInt32(0);
//                            int rowIndex = r.GetInt32(1);
//                            int existing = 0;

//                            if (finResultIncomeRows.TryGetValue(formId, out existing))
//                            {
//                                // Take the minimal value
//                                finResultIncomeRows[formId] = rowIndex < existing ? rowIndex : existing;
//                            }
//                            else
//                            {
//                                finResultIncomeRows[formId] = rowIndex;
//                            }
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            // Get all matching rows (by row name) for all 'Form 2-m' financial results
//            query = @"SELECT
//                r.form_id,
//                r.row_index
//            FROM
//                fin_report_rows r
//            WHERE
//                r.form_id IN ($) AND
//                UPPER(r.name) like @res1 AND
//                UPPER(r.name) like @res2 AND
//                UPPER(r.name) like @res3";

//            query = query.Replace("$", formIDs2m);

//            using (SqlCommand cmdFinPlanRows = new SqlCommand(query, connectionSqlClient))
//            {
//                cmdFinPlanRows.Parameters.Add(new SqlParameter("res1", Properties.Resources.FinResultsPureIncome1));
//                cmdFinPlanRows.Parameters.Add(new SqlParameter("res2", Properties.Resources.FinResultsPureIncome2));
//                cmdFinPlanRows.Parameters.Add(new SqlParameter("res3", Properties.Resources.FinResultsPureIncome3));

//                using (SqlDataReader r = cmdFinPlanRows.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                        {
//                            int formId = r.GetInt32(0);
//                            int rowIndex = r.GetInt32(1);
//                            int existing = 0;

//                            if (finResultIncomeRows.TryGetValue(formId, out existing))
//                            {
//                                // Take the minimal value
//                                finResultIncomeRows[formId] = rowIndex < existing ? rowIndex : existing;
//                            }
//                            else
//                            {
//                                finResultIncomeRows[formId] = rowIndex;
//                            }
//                        }
//                    }

//                    r.Close();
//                }
//            }
//        }

//        private void CalculateBudgetPayments(ReportPeriod period,
//            IList<ReportPeriod> periods,
//            Dictionary<int, KeyValuePair<int, int>> organizations,
//            Dictionary<int, int> finPlans,
//            Dictionary<int, int> finPlanPaymentRows,
//            Dictionary<int, int> finPlan1stQuarterCol,
//            Dictionary<int, int> finPlan2ndQuarterCol,
//            Dictionary<int, int> finPlan3rdQuarterCol,
//            Dictionary<int, int> finPlan4thQuarterCol,
//            Dictionary<int, int> finResults,
//            Dictionary<int, int> finResultIncomeRows,
//            Dictionary<int, decimal> rates,
//            HashSet<int> budgetPaymentFormulae)
//        {
//            // Get all fin plans for this period
//            Dictionary<int, int> finPlanForms = new Dictionary<int, int>();
//            int finPlanPeriod = period.Id - 1;

//            while (finPlanPeriod > 0 && finPlanForms.Count == 0)
//            {
//                foreach (KeyValuePair<int, int> plan in finPlans)
//                {
//                    if (plan.Value == finPlanPeriod)
//                    {
//                        finPlanForms.Add(plan.Key, plan.Value);
//                    }
//                }

//                finPlanPeriod--;
//            }

//            // Get all fin results for this period
//            Dictionary<int, int> finResultForms = new Dictionary<int, int>();

//            foreach (KeyValuePair<int, int> res in finResults)
//            {
//                if (res.Value == period.Id)
//                {
//                    finResultForms.Add(res.Key, res.Value);
//                }
//            }


//            // Generate ID and name for the new formula
//            int formulaId = (--customFormulaIdSeed);
//            string formulaName = string.Format(Properties.Resources.FormulaBudgPayments, period.Date.ToString("dd.MM.yyyy"));

//            budgetPaymentFormulae.Add(formulaId);

//            // Save the formula ID and Name to SQL Server
//            InsertFormulaIntoSqlServer(formulaId, period.Id, formulaName, "");

//            foreach (KeyValuePair<int, KeyValuePair<int, int>> org in organizations)
//            {
//                decimal rate = 0m;
//                decimal actualPayment = 0m;

//                if (rates.TryGetValue(org.Key, out rate))
//                {
//                    // 1) Calculate the actual payment (from 'financial result' form)                    

//                    foreach (KeyValuePair<int, int> resForm in finResultForms)
//                    {
//                        int rowId = 0;
//                        decimal val = 0;

//                        if (finResultIncomeRows.TryGetValue(resForm.Key, out rowId))
//                        {
//                            if (GetFinFormulaTokenValue(resForm.Key, rowId, 3, org.Key, resForm.Value, out val))
//                            {

//                                decimal prevVal = 0;
//                                decimal prevActualPayment = 0;

//                                if (period.PreviousPeriods != null)
//                                {
//                                    foreach (var prevPeriod in period.PreviousPeriods)
//                                    {
//                                        // Get all fin results for this period
//                                        foreach (KeyValuePair<int, int> res in finResults)
//                                        {
//                                            if (res.Value == prevPeriod.Id)
//                                            {
//                                                if (finResultIncomeRows.TryGetValue(res.Key, out rowId))
//                                                {
//                                                    if (GetFinFormulaTokenValue(res.Key, rowId, 3, org.Key, prevPeriod.Id, out prevVal))
//                                                    {
//                                                        prevActualPayment = prevActualPayment + prevVal;
//                                                        break;
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                }

//                                val = val - prevActualPayment;
//                                // Negative values mean negative income; we do not take such cases into consideration
//                                if (val > 0)
//                                {                                        
//                                    actualPayment = rate * val;
//                                }
//                            }
//                        }
//                    }
//                }

//                decimal proposedPayment = -1m;

//                if (finPlanForms.Count > 0)
//                {
//                    // Get all organizations for which financial plan is approved (for this period)
//                    HashSet<int> orgApproved = new HashSet<int>();

//                    GetOrganizationsWithApprovedFinPlan(orgApproved, finPlanForms);

//                    // 2) Calculate the proposed payment (from fin. plan)
//                    if (orgApproved.Contains(org.Key))
//                    {
//                        foreach (KeyValuePair<int, int> finPlanForm in finPlanForms)
//                        {
//                            // We need to take the value from the column which corresponds to the processed period
//                            Dictionary<int, int> finPlanCol = null;

//                            switch (getPeriodQuarterIndex(period.Date))
//                            {
//                                case 1:
//                                    finPlanCol = finPlan1stQuarterCol;
//                                    break;

//                                case 2:
//                                    finPlanCol = finPlan2ndQuarterCol;
//                                    break;

//                                case 3:
//                                    finPlanCol = finPlan3rdQuarterCol;
//                                    break;

//                                case 4:
//                                    finPlanCol = finPlan4thQuarterCol;
//                                    break;
//                            }

//                            int columnIndex = 0;

//                            if (finPlanCol != null && finPlanCol.TryGetValue(finPlanForm.Key, out columnIndex))
//                            {
//                                int rowId = 0;
//                                decimal val = 0;

//                                if (finPlanPaymentRows.TryGetValue(finPlanForm.Key, out rowId))
//                                {
//                                    if (GetFinFormulaTokenValue(finPlanForm.Key, rowId, columnIndex, org.Key, finPlanForm.Value, out val))
//                                    {
//                                        proposedPayment = val;
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    if (actualPayment >= 0 || proposedPayment >= 0)
//                    {
//                        // Calculate the final value
//                        decimal payment = Math.Max(actualPayment, proposedPayment);

//                        /*
//                        // If this value is NOT taken from fin. plan, make it negative - to display on the web site as 'not confirmed'
//                        if (proposedPayment < 0)
//                        {
//                            payment = -payment;
//                        }
//                        */

//                        // Save the formula value
//                        rowBudgetPaymentFormulaValues[0] = org.Key;
//                        rowBudgetPaymentFormulaValues[1] = formulaId;
//                        rowBudgetPaymentFormulaValues[2] = period.Id;
//                        rowBudgetPaymentFormulaValues[3] = payment;

//                        tableBudgetPaymentFormulaValues.Rows.Add(rowBudgetPaymentFormulaValues);
//                    }   
//                }
//            }
//        }

//        private void GetOrganizationsWithApprovedFinPlan(HashSet<int> orgApproved, Dictionary<int, int> finPlanForms)
//        {
//            // Prepare the list of Form IDs, to get all organizations with a single query
//            string formIDs = "";

//            foreach (KeyValuePair<int, int> form in finPlanForms)
//            {
//                if (formIDs.Length > 0)
//                {
//                    formIDs += ", ";
//                }

//                formIDs += form.Key.ToString();
//            }

//            // Get the list of organizations
//            string query = "SELECT organization_id FROM fin_form_changes WHERE form_id IN ($) AND form_status = 2";

//            query = query.Replace("$", formIDs);

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader r = cmd.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0))
//                        {
//                            orgApproved.Add(r.GetInt32(0));
//                        }
//                    }

//                    r.Close();
//                }
//            }
//        }

//        int getPeriodQuarterIndex(DateTime periodStartDate)
//        {
//            switch (periodStartDate.Month)
//            {
//                case 2:
//                case 3:
//                case 4:
//                    return 1;

//                case 5:
//                case 6:
//                case 7:
//                    return 2;

//                case 8:
//                case 9:
//                case 10:
//                    return 3;

//                case 11:
//                case 12:
//                case 1:
//                    return 4;
//            }

//            return -1;
//        }

//        #endregion (Budget payment rate calculation)
//    }

    public class ReportPeriod
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public IList<ReportPeriod> PreviousPeriods { get; set; }
    }

}
