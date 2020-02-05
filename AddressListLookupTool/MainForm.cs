using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUKV.ImportToolUtils;
//using GUKV.ImportToolUtils;


namespace GUKV.DataMigration
{
    public partial class MainForm : Form
    {
        #region Member variables

        /// <summary>
        /// Connection settings and other preferences
        /// </summary>
        private Preferences preferences = new Preferences();

        /// <summary>
        /// Define an object to pass to the Word API for missing parameters
        /// </summary>
        private object missing = System.Type.Missing;

        /// <summary>
        /// The index of grid column that contains address
        /// </summary>
        private int gridAddressColumn = -1;

        /// <summary>
        /// Address lookup helper object
        /// </summary>
        //private ObjectFinder objectFinder = new ObjectFinder();

        /// <summary>
        /// Contains all data from 'view_buildings' View in SQL Server
        /// </summary>
        private Dictionary<int, List<Dictionary<string, object>>> cacheObjects = new Dictionary<int, List<Dictionary<string, object>>>();

        /// <summary>
        /// Contains all data from 'view_balans' View in SQL Server
        /// </summary>
        private Dictionary<int, List<Dictionary<string, object>>> cacheBalans = new Dictionary<int, List<Dictionary<string, object>>>();

        /// <summary>
        /// Contains all data from 'view_arenda' View in SQL Server
        /// </summary>
        private Dictionary<int, List<Dictionary<string, object>>> cacheArenda = new Dictionary<int, List<Dictionary<string, object>>>();

        /// <summary>
        /// Contains all data from 'view_privatization_objects' View in SQL Server
        /// </summary>
        private Dictionary<int, List<Dictionary<string, object>>> cachePrivat = new Dictionary<int, List<Dictionary<string, object>>>();

        /// <summary>
        /// Contains all available properties from 'view_buildings' View in SQL Server
        /// </summary>
        private Dictionary<string, string> objectProperties = new Dictionary<string, string>();

        /// <summary>
        /// Contains all available properties from 'view_balans' View in SQL Server
        /// </summary>
        private Dictionary<string, string> balansProperties = new Dictionary<string, string>();

        /// <summary>
        /// Contains all available properties from 'view_arenda' View in SQL Server
        /// </summary>
        private Dictionary<string, string> arendaProperties = new Dictionary<string, string>();

        /// <summary>
        /// Contains all available properties from 'view_privatization_objects' View in SQL Server
        /// </summary>
        private Dictionary<string, string> privatProperties = new Dictionary<string, string>();

        /// <summary>
        /// This prefix is added to all columns extracted from the source document
        /// </summary>
        private const string SourceColumnPrefix = "SrcColumn";

        #endregion (Member variables)

        public MainForm()
        {
            InitializeComponent();

            ObjectFinder.Instance.AllowLiteraAComparisonWithoutSquareMatch(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!preferences.ReadConnPreferencesFromFile())
            {
                MessageBox.Show(
                    Properties.AppResources.ErrorNoIniFile,
                    Properties.AppResources.ErrorCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Application.Exit();
            }

            using (DataLoadingForm form = new DataLoadingForm())
            {
                form.Show();
                form.Refresh();

                // Try to connect to SQL Server
                string connString = preferences.GetGUKVConnectionString();

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    try
                    {
                        connection.Open();

                        form.Progress.Value = 20;
                        form.Progress.Refresh();
                        form.Progress.Refresh();

                        // Prepare the cache of object information
                        CacheDataFromView(connection, cacheObjects, "SELECT * FROM view_buildings", "building_id", AddColumnForm.ObjectFieldPrefix);

                        form.Progress.Value = 40;
                        form.Progress.Refresh();
                        form.Progress.Refresh();

                        // Prepare the cache of Balans information
                        CacheDataFromView(connection, cacheBalans, "SELECT * FROM view_balans", "building_id", AddColumnForm.BalansFieldPrefix);

                        form.Progress.Value = 60;
                        form.Progress.Refresh();
                        form.Progress.Refresh();

                        // Prepare the cache of Arenda information
                        CacheDataFromView(connection, cacheArenda, "SELECT * FROM view_arenda WHERE agreement_active_int = 1", "building_id", AddColumnForm.ArendaFieldPrefix);

                        form.Progress.Value = 80;
                        form.Progress.Refresh();
                        form.Progress.Refresh();

                        // Prepare the cache of Privatization information
                        CacheDataFromView(connection, cachePrivat, "SELECT * FROM view_privatization_objects", "building_id", AddColumnForm.PrivatFieldPrefix);

                        form.Progress.Value = 100;
                        form.Progress.Refresh();
                        form.Progress.Refresh();

                        // Populate the object finder from SQL Server
                        ObjectFinder.Instance.BuildObjectCacheFromSqlServer(connection);

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            Properties.AppResources.ErrorNoDbConnection + ": " + ex.Message,
                            Properties.AppResources.ErrorCaption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }

                form.Refresh();
                form.Hide();
            }

            PopulateProperties(objectProperties, Properties.AppSettings.Default.ObjectPropList);
            PopulateProperties(balansProperties, Properties.AppSettings.Default.BalansPropList);
            PopulateProperties(arendaProperties, Properties.AppSettings.Default.ArendaPropList);
            PopulateProperties(privatProperties, Properties.AppSettings.Default.PrivatPropList);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolButtonOpen_Click(object sender, EventArgs e)
        {
            if (openInputFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Check the document format
                string fileName = openInputFileDialog.FileName.ToLower();

                if (fileName.EndsWith(".doc") || fileName.EndsWith(".docx"))
                {
                    //ReadInputWordDoc(openInputFileDialog.FileName);
                }
                else if (fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx"))
                {
                   // ReadInputExcelDoc(openInputFileDialog.FileName);
                }
            }
        }

        private void toolButtonWrite_Click(object sender, EventArgs e)
        {
            if (saveOutputFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Check the document format
                string fileName = saveOutputFileDialog.FileName.ToLower();

                if (fileName.EndsWith(".doc") || fileName.EndsWith(".docx"))
                {
                    WriteOutputWordDoc(saveOutputFileDialog.FileName);
                }
                else if (fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx"))
                {
                    WriteOutputExcelDoc(saveOutputFileDialog.FileName);
                }
            }
        }

        private void toolButtonAddColumn_Click(object sender, EventArgs e)
        {
            if (gridAddressTable.Rows.Count > 0)
            {
                if (gridAddressColumn >= 0)
                {
                    AddColumnForm form = new AddColumnForm();

                    form.BalansProperties = balansProperties;
                    form.ArendaProperties = arendaProperties;
                    form.ObjectProperties = objectProperties;
                    form.PrivatProperties = privatProperties;

                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        gridAddressTable.Columns.Add(form.SelectedColumnFieldName, "");

                        gridAddressTable.Columns[gridAddressTable.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                        gridAddressTable.Columns[gridAddressTable.Columns.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                        // Set the column title
                        DataGridViewRow row = gridAddressTable.Rows[0];
                        row.Cells[gridAddressTable.Columns.Count - 1].Value = form.SelectedColumnTitle;

                        // Fill the column data
                        FillColumn(gridAddressTable.Columns.Count - 1, form.SelectedColumnFieldName);
                    }
                }
                else
                {
                    MessageBox.Show(
                        Properties.AppResources.ErrorNoAddressColumn,
                        Properties.AppResources.ErrorCaption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(
                    Properties.AppResources.ErrorNoData,
                    Properties.AppResources.ErrorCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void toolButtonDelColumn_Click(object sender, EventArgs e)
        {
            if (gridAddressTable.SelectedCells.Count != 1)
            {
                MessageBox.Show(
                    Properties.AppResources.ErrorMultipleDelCells,
                    Properties.AppResources.ErrorCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int columnToDelete = gridAddressTable.SelectedCells[0].ColumnIndex;

            // Ask user if he wants to delete the original column
            if (gridAddressTable.Columns[columnToDelete].Name.StartsWith(SourceColumnPrefix))
            {
                if (MessageBox.Show(Properties.AppResources.ConfirmationDelSrcColumn, "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            if (columnToDelete == gridAddressColumn)
            {
                gridAddressColumn = -1;
                toolEditAddressColumn.Text = "";
            }
            else if (columnToDelete < gridAddressColumn)
            {
                gridAddressColumn--;
            }

            gridAddressTable.Columns.RemoveAt(columnToDelete);
        }

        private void toolButtonAddrColumn_Click(object sender, EventArgs e)
        {
            if (gridAddressTable.SelectedCells.Count != 1)
            {
                MessageBox.Show(
                    Properties.AppResources.ErrorMultipleSelCells,
                    Properties.AppResources.ErrorCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            gridAddressColumn = gridAddressTable.SelectedCells[0].ColumnIndex;

            if (gridAddressTable.SelectedCells[0].Value != null)
            {
                string columnName = gridAddressTable.SelectedCells[0].Value.ToString().Trim();

                if (columnName.Length > 0)
                {
                    toolEditAddressColumn.Text = columnName;
                }
                else
                {
                    toolEditAddressColumn.Text = Properties.AppResources.Column + " " + (gridAddressColumn + 1).ToString();
                }
            }
            else
            {
                toolEditAddressColumn.Text = Properties.AppResources.Column + " " + (gridAddressColumn + 1).ToString();
            }
        }

        #region Document reading

        //private void ReadInputWordDoc(string fileName)
        //{
        //    DeleteCurrentGridData();

        //    // Create the Word application
        //    Microsoft.Office.Interop.Word._Application app = new Microsoft.Office.Interop.Word.Application();

        //    app.Visible = false;

        //    try
        //    {
        //        // Open the source document
        //        Microsoft.Office.Interop.Word._Document doc = app.Documents.Open(fileName);

        //        if (doc != null)
        //        {
        //            try
        //            {
        //                // Find the first table in the document
        //                if (doc.Tables.Count > 0)
        //                {
        //                    // Take the table with non-zero row count
        //                    Microsoft.Office.Interop.Word.Table table = null;

        //                    for (int n = 1; n <= doc.Tables.Count; n++)
        //                    {
        //                        if (doc.Tables[n].Rows.Count > 0)
        //                        {
        //                            table = doc.Tables[n];
        //                            break;
        //                        }
        //                    }

        //                    if (table != null)
        //                    {
        //                        // Duplicate all columns in our Grid
        //                        for (int nCol = 1; nCol <= table.Columns.Count; nCol++)
        //                        {
        //                            gridAddressTable.Columns.Add(SourceColumnPrefix + nCol.ToString(), "");
        //                            gridAddressTable.Columns[gridAddressTable.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
        //                            gridAddressTable.Columns[gridAddressTable.Columns.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        //                        }

        //                        // Copy data from all the Table rows to our Grid
        //                        object[] rowData = new object[table.Columns.Count];

        //                        for (int nRow = 1; nRow <= table.Rows.Count; nRow++)
        //                        {
        //                            // Copy the table row to our grid
        //                            Microsoft.Office.Interop.Word.Row row = table.Rows[nRow];

        //                            if (row.Cells.Count == table.Columns.Count)
        //                            {
        //                                for (int nCell = 1; nCell <= row.Cells.Count; nCell++)
        //                                {
        //                                    Microsoft.Office.Interop.Word.Cell cell = row.Cells[nCell];

        //                                    rowData[nCell - 1] = RemoveSpecialChars(cell.Range.Text);
        //                                }

        //                                gridAddressTable.Rows.Add(rowData);
        //                            }

        //                            // Update the progress bar
        //                            double progress = (double)nRow / (double)table.Rows.Count;
        //                            int newValue = (int)((double)readingProgressBar.Maximum * progress) + 1;

        //                            readingProgressBar.Value = (newValue > readingProgressBar.Maximum) ? readingProgressBar.Maximum : newValue;
        //                            readingProgressBar.Update();
        //                        }
        //                    }
        //                }
        //            }
        //            finally
        //            {
        //                doc.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(
        //            Properties.AppResources.DocOpenError + ": " + ex.Message,
        //            Properties.AppResources.ErrorCaption,
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Error);

        //        DeleteCurrentGridData();
        //    }

        //    readingProgressBar.Value = 0;
        //    readingProgressBar.Update();

        //    app.Quit();
        //}

        //private void ReadInputExcelDoc(string fileName)
        //{
        //    DeleteCurrentGridData();

        //    // Create the Excel application
        //    Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

        //    app.Visible = false;

        //    try
        //    {
        //        // Open the source document
        //        Microsoft.Office.Interop.Excel._Workbook book = app.Workbooks.Open(fileName);

        //        if (book != null)
        //        {
        //            try
        //            {
        //                // Assume that table is located at the first sheet
        //                if (book.Sheets.Count > 0)
        //                {
        //                    Microsoft.Office.Interop.Excel.Worksheet sheet = book.Sheets[1];

        //                    // Import all the used cells
        //                    Microsoft.Office.Interop.Excel.Range range = sheet.UsedRange;

        //                    object[,] valueArray = (object[,])range.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);

        //                    int nRowCount = valueArray.GetLength(0);
        //                    int nColCount = valueArray.GetLength(1);

        //                    if (nRowCount > 0 && nColCount > 0)
        //                    {
        //                        // Duplicate all columns in our Grid
        //                        for (int nCol = 1; nCol <= nColCount; nCol++)
        //                        {
        //                            gridAddressTable.Columns.Add(SourceColumnPrefix + nCol.ToString(), "");
        //                            gridAddressTable.Columns[gridAddressTable.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
        //                            gridAddressTable.Columns[gridAddressTable.Columns.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        //                        }

        //                        // Copy data from all the Table rows to our Grid
        //                        object[] rowData = new object[nColCount];

        //                        for (int nRow = 0; nRow < nRowCount; nRow++)
        //                        {
        //                            for (int nCell = 0; nCell < nColCount; nCell++)
        //                            {
        //                                object value = valueArray[nRow + 1, nCell + 1];

        //                                rowData[nCell] = value != null ? value.ToString() : "";
        //                            }

        //                            gridAddressTable.Rows.Add(rowData);

        //                            // Update the progress bar
        //                            double progress = (double)nRow / (double)nRowCount;
        //                            int newValue = (int)((double)readingProgressBar.Maximum * progress) + 1;

        //                            readingProgressBar.Value = (newValue > readingProgressBar.Maximum) ? readingProgressBar.Maximum : newValue;
        //                            readingProgressBar.Update();
        //                        }
        //                    }
        //                }
        //            }
        //            finally
        //            {
        //                book.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(
        //            Properties.AppResources.DocOpenError + ": " + ex.Message,
        //            Properties.AppResources.ErrorCaption,
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Error);

        //        DeleteCurrentGridData();
        //    }

        //    readingProgressBar.Value = 0;
        //    readingProgressBar.Update();

        //    app.Quit();
        //}

        private string RemoveSpecialChars(string source)
        {
            source = source.Replace("\r", " ");
            source = source.Replace("\n", " ");
            source = source.Replace("\a", " ");

            while (source.IndexOf("  ") >= 0)
            {
                source = source.Replace("  ", " ");
            }

            return source.Trim();
        }

        private void DeleteCurrentGridData()
        {
            gridAddressTable.Rows.Clear();
            gridAddressTable.Columns.Clear();

            gridAddressColumn = -1;
            toolEditAddressColumn.Text = "";
        }

        #endregion (Document reading)

        #region Document writing

        private void WriteOutputWordDoc(string fileName)
        {
            // Create the Word application
            //Microsoft.Office.Interop.Word._Application app = new Microsoft.Office.Interop.Word.Application();

            //app.Visible = false;

            //try
            //{
            //    // Create a new Word document
            //    Microsoft.Office.Interop.Word._Document doc = app.Documents.Add(ref missing, ref missing, ref missing, ref missing);

            //    if (doc != null)
            //    {
            //        try
            //        {
            //            // Insert a default text into the new document
            //            Microsoft.Office.Interop.Word.Range range = doc.Range(0, 0);

            //            if (range != null)
            //            {
            //                // Insert a default text
            //                range.InsertAfter("\n000\n\n");

            //                // Now there must be 3 paragraphs in the document
            //                if (doc.Paragraphs.Count > 2)
            //                {
            //                    Microsoft.Office.Interop.Word.Range rangeForTable = doc.Paragraphs[2].Range;

            //                    if (rangeForTable != null)
            //                    {
            //                        Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(rangeForTable,
            //                            gridAddressTable.Rows.Count, gridAddressTable.Columns.Count, ref missing, ref missing);

            //                        if (table != null)
            //                        {
            //                            // Fill all cells of the table with data from our grid
            //                            for (int nRow = 0; nRow < gridAddressTable.Rows.Count && (nRow + 1) <= table.Rows.Count; nRow++)
            //                            {
            //                                Microsoft.Office.Interop.Word.Row row = table.Rows[nRow + 1];

            //                                for (int nCol = 0; nCol < gridAddressTable.Columns.Count && (nCol + 1) <= row.Cells.Count; nCol++)
            //                                {
            //                                    Microsoft.Office.Interop.Word.Cell cell = row.Cells[nCol + 1];

            //                                    cell.Range.Font.Size = 9;

            //                                    cell.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderTop].Visible = true;
            //                                    cell.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderLeft].Visible = true;
            //                                    cell.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderRight].Visible = true;
            //                                    cell.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].Visible = true;

            //                                    object value = gridAddressTable.Rows[nRow].Cells[nCol].Value;

            //                                    if (value != null)
            //                                    {
            //                                        cell.Range.Text = value.ToString();
            //                                    }

            //                                    if (gridAddressTable.Rows[nRow].Cells[nCol].Style.ForeColor == Color.Red)
            //                                    {
            //                                        cell.Range.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorRed;
            //                                    }
            //                                }

            //                                // Update the progress bar
            //                                double progress = (double)(nRow + 1) / (double)gridAddressTable.Rows.Count;
            //                                int newValue = (int)((double)readingProgressBar.Maximum * progress) + 1;

            //                                readingProgressBar.Value = (newValue > readingProgressBar.Maximum) ? readingProgressBar.Maximum : newValue;
            //                                readingProgressBar.Update();
            //                            }
            //                        }
            //                    }
            //                }
            //            }

            //            MessageBox.Show(
            //                Properties.AppResources.MsgDataSaved,
            //                "",
            //                MessageBoxButtons.OK,
            //                MessageBoxIcon.Information);
            //        }
            //        finally
            //        {
            //            doc.SaveAs(fileName);
            //            doc.Close();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(
            //        Properties.AppResources.DocSaveError + ": " + ex.Message,
            //        Properties.AppResources.ErrorCaption,
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);
            //}

            //readingProgressBar.Value = 0;
            //readingProgressBar.Update();

            //app.Quit();
        }

        private void WriteOutputExcelDoc(string fileName)
        {
            // Delete the output file, if it exists
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    Properties.AppResources.ErrorCannotSaveDoc,
                    Properties.AppResources.ErrorCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Create the Excel application
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

            app.Visible = false;

            try
            {
                // Create a new Workbook
                Microsoft.Office.Interop.Excel._Workbook book = app.Workbooks.Add();

                if (book != null)
                {
                    try
                    {
                        // Create a single sheet in the Workbook
                        if (book.Sheets.Count == 0)
                        {
                            book.Sheets.Add();
                        }

                        Microsoft.Office.Interop.Excel.Worksheet sheet = book.Sheets[1];

                        if (sheet != null)
                        {
                            // Set column width for all columns we are going to use
                            for (int nCol = 0; nCol < gridAddressTable.Columns.Count; nCol++)
                            {
                                // Build the column signature in Excel format
                                char column = (char)((int)'A' + nCol);
                                string signature = column.ToString() + ":" + column.ToString();

                                sheet.Range[signature, missing].EntireColumn.ColumnWidth = 30;
                            }

                            // Fill all cells of the sheet with data from our grid
                            for (int nRow = 0; nRow < gridAddressTable.Rows.Count; nRow++)
                            {
                                for (int nCol = 0; nCol < gridAddressTable.Columns.Count; nCol++)
                                {
                                    // Build the cell signature in Excel format
                                    char column = (char)((int)'A' + nCol);
                                    string cell = column.ToString() + (nRow + 1).ToString();

                                    sheet.Range[cell, missing].Font.Size = 9;

                                    object value = gridAddressTable.Rows[nRow].Cells[nCol].Value;

                                    if (value != null)
                                    {
                                        sheet.Range[cell, missing].Value2 = value.ToString();
                                    }

                                    if (gridAddressTable.Rows[nRow].Cells[nCol].Style.ForeColor == Color.Red)
                                    {
                                        sheet.Range[cell, missing].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    }

                                    sheet.Range[cell, missing].WrapText = true;

                                    sheet.Range[cell, missing].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle =
                                        Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                    sheet.Range[cell, missing].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle =
                                        Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                    sheet.Range[cell, missing].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle =
                                        Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                    sheet.Range[cell, missing].Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
                                        Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                }

                                // Update the progress bar
                                double progress = (double)(nRow + 1) / (double)gridAddressTable.Rows.Count;
                                int newValue = (int)((double)readingProgressBar.Maximum * progress) + 1;

                                readingProgressBar.Value = (newValue > readingProgressBar.Maximum) ? readingProgressBar.Maximum : newValue;
                                readingProgressBar.Update();
                            }
                        }

                        MessageBox.Show(
                            Properties.AppResources.MsgDataSaved,
                            "",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    finally
                    {
                        book.SaveAs(fileName);
                        book.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Properties.AppResources.DocSaveError + ": " + ex.Message,
                    Properties.AppResources.ErrorCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            readingProgressBar.Value = 0;
            readingProgressBar.Update();

            app.Quit();
        }

        #endregion (Document writing)

        #region Data caching

        private void CacheDataFromView(SqlConnection connection,
            Dictionary<int, List<Dictionary<string, object>>> cache,
            string sqlQuery,
            string objectFieldName,
            string fieldNamePrefix)
        {
            objectFieldName = objectFieldName.ToLower();

            Dictionary<string, object> data = new Dictionary<string, object>();

            using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GetReaderRowData(reader, data);

                        if (data.ContainsKey(objectFieldName))
                        {
                            object dataObjectId = data[objectFieldName];

                            if (dataObjectId is int)
                            {
                                int objectId = (int)dataObjectId;
                                List<Dictionary<string, object>> list = null;

                                if (cache.TryGetValue(objectId, out list))
                                {
                                    list.Add(CloneRowData(data, fieldNamePrefix));
                                }
                                else
                                {
                                    list = new List<Dictionary<string, object>>();

                                    list.Add(CloneRowData(data, fieldNamePrefix));

                                    cache.Add(objectId, list);
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void GetReaderRowData(System.Data.IDataReader reader, Dictionary<string, object> data)
        {
            data.Clear();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.IsDBNull(i))
                {
                    string srcFieldName = reader.GetName(i).ToLower().Trim();

                    if (srcFieldName.Length > 0)
                    {
                        // Perform value pre-processing
                        object val = reader.GetValue(i);

                        if (val is string)
                        {
                            string str = ((string)val).Trim();

                            val = (str.Length > 0) ? str : null;
                        }

                        if (val != null)
                        {
                            data[srcFieldName.ToLower()] = val;
                        }
                    }
                }
            }
        }

        private Dictionary<string, object> CloneRowData(Dictionary<string, object> data, string fieldNamePrefix)
        {
            Dictionary<string, object> clone = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> pair in data)
            {
                clone.Add(fieldNamePrefix + pair.Key, pair.Value);
            }

            return clone;
        }

        #endregion (Data caching)

        #region Column data population

        private void FillColumn(int columnIndex, string fieldName)
        {
            List<int> objectIDs = new List<int>();
            bool preciseMatch = true;

            // Determine the cache from which to take column data
            if (gridAddressColumn >= 0 && gridAddressColumn < gridAddressTable.Columns.Count)
            {
                // Fill the specified column in all rows of the Grid; starting from the second row
                for (int nRow = 1; nRow < gridAddressTable.Rows.Count; nRow++)
                {
                    DataGridViewRow row = gridAddressTable.Rows[nRow];

                    if (columnIndex < row.Cells.Count && gridAddressColumn < row.Cells.Count)
                    {
                        // Try to find an ID of object by the address
                        object cellValue = row.Cells[gridAddressColumn].Value;

                        if (cellValue is string)
                        {
                            ObjectFinder.Instance.FindObjectIDsByInpreciseAddress(cellValue.ToString(),
                                objectIDs, menuItemAllowInpreciseAddrMatch.Checked, out preciseMatch);

                            string lookup = "";

                            if (!preciseMatch)
                            {
                                lookup = Properties.AppResources.ErrorInpreciseMatch;
                            }

                            for (int i = 0; i < objectIDs.Count; i++)
                            {
                                // Dump the object address, because it may be inprecise
                                if (menuItemDisplayFoundAddress.Checked)
                                {
                                    ObjectInfo info = ObjectFinder.Instance.GetObjectInfo(objectIDs[i]);

                                    if (lookup.Length > 0)
                                        lookup += "\n\n";

                                    lookup += "=== " + info.FormatToStr() + " ===";
                                }

                                // Dump the requested information
                                string columnText = FindObjectColumnText(objectIDs[i], fieldName);

                                if (columnText.Length > 0)
                                {
                                    if (lookup.Length > 0)
                                        lookup += "\n\n";

                                    lookup += columnText;
                                }
                            }

                            row.Cells[columnIndex].Value = lookup;

                            if (!preciseMatch)
                            {
                                row.Cells[columnIndex].Style.ForeColor = Color.Red;
                            }
                        }
                    }

                    // Update the progress bar
                    double progress = (double)(nRow + 1) / (double)gridAddressTable.Rows.Count;
                    int newValue = (int)((double)readingProgressBar.Maximum * progress) + 1;

                    readingProgressBar.Value = (newValue > readingProgressBar.Maximum) ? readingProgressBar.Maximum : newValue;
                    readingProgressBar.Update();
                }
            }

            readingProgressBar.Value = 0;
            readingProgressBar.Update();
        }

        private string FindObjectColumnText(int objectId, string fieldName)
        {
            string result = "";
            List<Dictionary<string, object>> list = null;
            Dictionary<int, List<Dictionary<string, object>>> cache = null;

            if (fieldName.StartsWith(AddColumnForm.ObjectFieldPrefix))
            {
                cache = cacheObjects;
            }
            else if (fieldName.StartsWith(AddColumnForm.BalansFieldPrefix))
            {
                cache = cacheBalans;
            }
            else if (fieldName.StartsWith(AddColumnForm.ArendaFieldPrefix))
            {
                cache = cacheArenda;
            }
            else if (fieldName.StartsWith(AddColumnForm.PrivatFieldPrefix))
            {
                cache = cachePrivat;
            }

            // Field name is actually a set of fields, separated with the | character
            string[] fields = fieldName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (cache != null && cache.TryGetValue(objectId, out list))
            {
                foreach (Dictionary<string, object> data in list)
                {
                    string resultForOneObject = "";
                    object value = null;

                    foreach (string field in fields)
                    {
                        if (data.TryGetValue(field, out value) && value != null)
                        {
                            if (resultForOneObject.Length > 0)
                            {
                                resultForOneObject += ", ";
                            }

                            if (value is DateTime)
                            {
                                resultForOneObject += ((DateTime)value).ToShortDateString();
                            }
                            else
                            {
                                resultForOneObject += value.ToString();
                            }
                        }
                    }

                    if (result.Length > 0)
                    {
                        result += "\n\n";
                    }

                    result += resultForOneObject;
                }
            }

            return result;
        }

        #endregion (Column data population)

        #region Property lists

        private void PopulateProperties(Dictionary<string, string> properties,
            System.Collections.Specialized.StringCollection collection)
        {
            properties.Clear();

            foreach (string s in collection)
            {
                int nDivider = s.IndexOf('=');

                if (nDivider > 0)
                {
                    string fieldName = s.Substring(0, nDivider);
                    string title = s.Substring(nDivider + 1);

                    if (fieldName.Length > 0 && title.Length > 0)
                    {
                        properties[fieldName] = title;
                    }
                }
            }
        }

        #endregion (Property lists)
    }
}
