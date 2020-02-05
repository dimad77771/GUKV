using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GUKV.DataMigration
{
    public partial class MainForm : Form
    {
        #region Nested classes

        private class CustomTableRow : List<string>
        {
            public bool RowIsFull = true;

            public CustomTableRow()
            {
            }
        }

        #endregion (Nested classes)

        public static Preferences preferences = new Preferences();

        private ImportedDoc document = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (preferences.ReadConnPreferencesFromFile())
            {
                DB.LoadUsers(preferences);

                bool loggedIn = false;

                using (LoginForm loginForm = new LoginForm())
                {
                    loginForm.preferences = preferences;

                    if (loginForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        loggedIn = true;
                    }
                }

                if (!loggedIn)
                {
                    Application.Exit();
                }

                DB.LoadData(preferences);

                // Fill the drop-down of document types
                DB.FillComboBoxFromDictionaryNJF(comboDocType, DB.DICT_DOC_TYPES);

                // Not all types of documents must show up in this drop-down
                DB.RemoveComboBoxValue(comboDocType, 3); // Act
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Не знайдено файл з налаштуваннями системи.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        #region Document reading

        //private void ReadInputWordDoc(string fileName, bool replaceCurrentDoc)
        //{
        //    if (replaceCurrentDoc)
        //    {
        //        document = new ImportedDoc();
        //    }

        //    GUKV.DataMigration.ObjectFinder objectFinder = new DataMigration.ObjectFinder();

        //    DB.InitObjectFinderFrom1NF(preferences, objectFinder);

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
        //                Cursor.Current = Cursors.WaitCursor;

        //                // Read the document properties
        //                if (replaceCurrentDoc)
        //                {
        //                    ReadInputDocProperties(doc, document);
        //                }

        //                // Process each table in the document
        //                int appendixIndex = document.appendices.Count;

        //                for (int n = 1; n <= doc.Tables.Count; n++)
        //                {
        //                    if (doc.Tables[n].Rows.Count > 1)
        //                    {
        //                        appendixIndex++;

        //                        List<CustomTableRow> reconstructed = ReconstructTable(doc.Tables[n]);

        //                        ReadInputDocTableEx(reconstructed, appendixIndex, objectFinder);
        //                    }
        //                }
        //            }
        //            finally
        //            {
        //                Cursor.Current = Cursors.Default;

        //                doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(
        //            ex.Message,
        //            "Помилка при відкритті документу",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Error);
        //    }

        //    app.Quit();
        //}

        //private void ReadInputDocProperties(Microsoft.Office.Interop.Word._Document doc, ImportedDoc document)
        //{
        //    int paragraphIndex = 1;

        //    bool kmrFound = false;
        //    bool kmdaFound = false;
        //    bool sessionFound = false;
        //    bool rishennyaFound = false;
        //    bool rozpFound = false;

        //    while (paragraphIndex <= doc.Paragraphs.Count)
        //    {
        //        Microsoft.Office.Interop.Word.Paragraph para = doc.Paragraphs[paragraphIndex];

        //        string paraText = para.Range.Text.Trim().ToUpper();

        //        if (paraText.Length > 0)
        //        {
        //            if (paraText == Properties.AppResources.DocTextRishennya)
        //            {
        //                rishennyaFound = true;
        //            }
        //            else if (paraText == Properties.AppResources.DocTextRozp)
        //            {
        //                rozpFound = true;
        //            }
        //            else if (paraText.Contains(Properties.AppResources.DocTextKMDA))
        //            {
        //                kmdaFound = true;
        //            }
        //            else if (paraText.Contains(Properties.AppResources.DocTextKMR))
        //            {
        //                kmrFound = true;

        //                if (paraText.Contains(Properties.AppResources.DocTextSession))
        //                {
        //                    sessionFound = true;
        //                }
        //            }
        //            else if (paraText.Contains(Properties.AppResources.DocTextSession))
        //            {
        //                sessionFound = true;
        //            }
        //            else if (paraText.StartsWith(Properties.AppResources.DocTextDatePrefix))
        //            {
        //                ParseDocumentDate(document, paraText);

        //                // Read the document title
        //                int titleParagraph = paragraphIndex + 1;

        //                // Skip the 'KYIV' line, if it is present
        //                if (titleParagraph <= doc.Paragraphs.Count)
        //                {
        //                    Microsoft.Office.Interop.Word.Paragraph p = doc.Paragraphs[titleParagraph];

        //                    string txt = p.Range.Text.Trim().ToUpper();

        //                    if (txt == Properties.AppResources.DocTextKyiv)
        //                    {
        //                        titleParagraph++;
        //                    }
        //                }

        //                if (titleParagraph <= doc.Paragraphs.Count)
        //                {
        //                    Microsoft.Office.Interop.Word.Paragraph p = doc.Paragraphs[titleParagraph];

        //                    document.docTitle = p.Range.Text.Trim().ToUpper();

        //                    if (document.docTitle.Length > DB.MAX_DOC_TITLE_LEN)
        //                        document.docTitle = document.docTitle.Substring(0, DB.MAX_DOC_TITLE_LEN);
        //                }

        //                break;
        //            }
        //        }

        //        paragraphIndex++;
        //    }

        //    // Determine the document type
        //    if (rozpFound)
        //    {
        //        if (kmrFound && sessionFound)
        //        {
        //            document.docTypeId = 7;
        //        }
        //        else if (kmdaFound)
        //        {
        //            document.docTypeId = 1;
        //        }
        //    }
        //    else if (rishennyaFound)
        //    {
        //        if (kmrFound && sessionFound)
        //        {
        //            document.docTypeId = 5;
        //        }
        //        else if (kmdaFound)
        //        {
        //            document.docTypeId = 2;
        //        }
        //    }
        //    else
        //    {
        //        document.docTypeId = -1;
        //    }
        //}

        private void ParseDocumentDate(ImportedDoc document, string dateAndNumber)
        {
            // Remove the 'vid ' prefix
            dateAndNumber = dateAndNumber.Substring(4);

            // Extract the document number
            int numberDividerPos = dateAndNumber.IndexOf(Properties.AppResources.DocTextNumPrefix);

            if (numberDividerPos >= 0)
            {
                document.docNum = dateAndNumber.Substring(numberDividerPos + Properties.AppResources.DocTextNumPrefix.Length).Trim();

                dateAndNumber = dateAndNumber.Substring(0, numberDividerPos).Trim();

                if (document.docNum.Length > DB.MAX_DOC_NUMBER_LEN)
                    document.docNum = document.docNum.Substring(0, DB.MAX_DOC_NUMBER_LEN);
            }

            // Parse the date
            string[] dateParts = dateAndNumber.Split(new char[] { ' ', '.' });

            if (dateParts.Length >= 3)
            {
                try
                {
                    int day = int.Parse(dateParts[0]);
                    int month = DetermineMonthFromString(dateParts[1]);
                    int year = int.Parse(dateParts[2]);

                    document.docDate = new DateTime(year, month, day);
                }
                catch (Exception)
                {
                }
            }
        }

        private int DetermineMonthFromString(string monthStr)
        {
            for (int i = 0; i < Properties.AppSettings.Default.MonthNames.Count; i++)
            {
                if (monthStr.Contains(Properties.AppSettings.Default.MonthNames[i]))
                {
                    return i + 1;
                }
            }

            return int.Parse(monthStr);
        }

        private List<CustomTableRow> ReconstructTable(Microsoft.Office.Interop.Word.Table table)
        {
            List<CustomTableRow> reconstructed = new List<CustomTableRow>();

            // Get the table XML
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(table.Range.XML);

            // Find the table node
            System.Xml.XmlNode documentNode = null;
            System.Xml.XmlNode bodyNode = null;
            System.Xml.XmlNode sectionNode = null;
            System.Xml.XmlNode tableNode = null;

            foreach (System.Xml.XmlNode child in xmlDoc.ChildNodes)
            {
                if (child.NodeType == System.Xml.XmlNodeType.Element && child.Name.ToLower() == "w:worddocument")
                {
                    documentNode = child;
                    break;
                }
            }

            if (documentNode != null)
            {
                foreach (System.Xml.XmlNode child in documentNode.ChildNodes)
                {
                    if (child.NodeType == System.Xml.XmlNodeType.Element && child.Name.ToLower() == "w:body")
                    {
                        bodyNode = child;
                        break;
                    }
                }
            }

            if (bodyNode != null)
            {
                foreach (System.Xml.XmlNode child in bodyNode.ChildNodes)
                {
                    if (child.NodeType == System.Xml.XmlNodeType.Element && child.Name.ToLower() == "wx:sect")
                    {
                        sectionNode = child;
                        break;
                    }
                }
            }

            if (sectionNode != null)
            {
                foreach (System.Xml.XmlNode child in sectionNode.ChildNodes)
                {
                    if (child.NodeType == System.Xml.XmlNodeType.Element && child.Name.ToLower() == "w:tbl")
                    {
                        tableNode = child;
                        break;
                    }
                }
            }

            if (tableNode != null)
            {
                foreach (System.Xml.XmlNode node in tableNode.ChildNodes)
                {
                    if (node.NodeType == System.Xml.XmlNodeType.Element && node.Name.ToLower() == "w:tr")
                    {
                        // Create a new row
                        CustomTableRow row = new CustomTableRow();

                        foreach (System.Xml.XmlNode n in node.ChildNodes)
                        {
                            if (n.NodeType == System.Xml.XmlNodeType.Element && n.Name.ToLower() == "w:tc")
                            {
                                // Get the cell properties and inner text
                                string cellText = "";
                                int colSpan = 1;

                                foreach (System.Xml.XmlNode c in n.ChildNodes)
                                {
                                    if (c.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (c.Name.ToLower() == "w:tcpr")
                                        {
                                            foreach (System.Xml.XmlNode x in c.ChildNodes)
                                            {
                                                if (x.NodeType == System.Xml.XmlNodeType.Element && x.Name.ToLower() == "w:gridspan")
                                                {
                                                    System.Xml.XmlAttribute attrValue = x.Attributes["w:val"];

                                                    if (attrValue != null && attrValue.Value.Length > 0)
                                                    {
                                                        colSpan = int.Parse(attrValue.Value);
                                                    }
                                                }
                                            }
                                        }
                                        else if (c.Name.ToLower() == "w:p")
                                        {
                                            foreach (System.Xml.XmlNode rng in c.ChildNodes)
                                            {
                                                if (rng.NodeType == System.Xml.XmlNodeType.Element && rng.Name.ToLower() == "w:r")
                                                {
                                                    if (cellText.Length > 0)
                                                        cellText += "\n";

                                                    cellText += rng.InnerText;
                                                }
                                            }
                                        }
                                    }
                                }

                                // Add the cell value independently for each column
                                for (int i = 0; i < colSpan; i++)
                                {
                                    row.Add(cellText);
                                }

                                if (colSpan > 1)
                                {
                                    row.RowIsFull = false;
                                }
                            }
                        }

                        reconstructed.Add(row);
                    }
                }
            }

            return reconstructed;
        }

        //private void ReadInputDocTableEx(List<CustomTableRow> table, int appendixIndex,
        //    GUKV.DataMigration.ObjectFinder objectFinder)
        //{
        //    int tableColumnCount = 0;

        //    if (table.Count > 0)
        //    {
        //        tableColumnCount = table[0].Count;
        //    }

        //    if (tableColumnCount == 0)
        //        return;

        //    // If a sub-header row exists, we can use it to divide the table header from the data
        //    int firstDataRow = -1;
        //    int lastHeaderRow = -1;

        //    int rowWithNumbers = FindRowWithNumbers(table);

        //    if (rowWithNumbers > 0)
        //    {
        //        firstDataRow = rowWithNumbers + 1;
        //        lastHeaderRow = rowWithNumbers - 1;
        //    }
        //    else
        //    {
        //        firstDataRow = FindFirstIndexedRow(table, 0);

        //        if (firstDataRow > 0)
        //        {
        //            lastHeaderRow = firstDataRow - 1;
        //        }
        //        else
        //        {
        //            firstDataRow = FindFirstFullRow(table);

        //            if (firstDataRow > 0)
        //            {
        //                lastHeaderRow = firstDataRow - 1;
        //            }
        //        }
        //    }

        //    if (firstDataRow > 0 && lastHeaderRow >= 0)
        //    {
        //        // Build the column names
        //        Dictionary<int, string> inputColumnNames = new Dictionary<int, string>();

        //        for (int r = 0; r <= lastHeaderRow; r++)
        //        {
        //            for (int c = 0; c < table[r].Count; c++)
        //            {
        //                string cellText = table[r][c];

        //                if (cellText.Length > 0)
        //                {
        //                    string curValue = "";

        //                    if (inputColumnNames.TryGetValue(c, out curValue))
        //                    {
        //                        inputColumnNames[c] = curValue + " - " + cellText;
        //                    }
        //                    else
        //                    {
        //                        inputColumnNames[c] = cellText;
        //                    }
        //                }
        //            }
        //        }

        //        // Determine the meaning of each column by its title
        //        Dictionary<ColumnCategory, int> columnTypes = new Dictionary<ColumnCategory, int>();

        //        for (int nCol = 0; nCol < tableColumnCount; nCol++)
        //        {
        //            string columnTitle = "";

        //            if (inputColumnNames.TryGetValue(nCol, out columnTitle))
        //            {
        //                columnTitle = columnTitle.ToLower();

        //                if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeRowIndex) && nCol == 0)
        //                {
        //                    columnTypes[ColumnCategory.RowIndex] = nCol;
        //                }
        //                else if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeAddress))
        //                {
        //                    columnTypes[ColumnCategory.Address] = nCol;
        //                }
        //                else if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeLength))
        //                {
        //                    columnTypes[ColumnCategory.Length] = nCol;
        //                }
        //                else if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeSquare))
        //                {
        //                    columnTypes[ColumnCategory.Square] = nCol;
        //                }
        //                else if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeDiameter))
        //                {
        //                    columnTypes[ColumnCategory.Diameter] = nCol;
        //                }
        //                else if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeFinalCost))
        //                {
        //                    columnTypes[ColumnCategory.FinalCost] = nCol;
        //                }
        //                else if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeBalansCost))
        //                {
        //                    columnTypes[ColumnCategory.BalansCost] = nCol;
        //                }
        //                else if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeBuildYear))
        //                {
        //                    columnTypes[ColumnCategory.BuildYear] = nCol;
        //                }
        //                else if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeExplYear))
        //                {
        //                    columnTypes[ColumnCategory.ExplYear] = nCol;
        //                }
        //                else if (IsColumnTitleMatch(columnTitle, Properties.AppSettings.Default.ColTypeObjectName))
        //                {
        //                    columnTypes[ColumnCategory.ObjectName] = nCol;
        //                }
        //            }
        //        }

        //        // Read the table data
        //        RemoveSuspiciousRows(table, columnTypes, firstDataRow);
        //        ProcessInputDocTable(table, tableColumnCount, appendixIndex, objectFinder, columnTypes, firstDataRow);
        //    }
        //}

        private void RemoveSuspiciousRows(List<CustomTableRow> table, Dictionary<ColumnCategory, int> columnTypes, int firstDataRow)
        {
            int buildYearColumn = -1;

            if (columnTypes.TryGetValue(ColumnCategory.BuildYear, out buildYearColumn))
            {
                // Check if there are some rows where Address and/or Build Year is defined
                bool rowsWithYearExist = false;

                for (int nRow = firstDataRow; nRow < table.Count; nRow++)
                {
                    if (table[nRow][buildYearColumn].Length > 0 && StringHasOnlyDigits(table[nRow][buildYearColumn]))
                    {
                        rowsWithYearExist = true;
                        break;
                    }
                }

                if (rowsWithYearExist)
                {
                    HashSet<CustomTableRow> rowsToRemove = new HashSet<CustomTableRow>();

                    for (int nRow = firstDataRow; nRow < table.Count; nRow++)
                    {
                        if (!table[nRow].RowIsFull && (table[nRow][buildYearColumn].Length == 0 || !StringHasDigits(table[nRow][buildYearColumn])))
                        {
                            rowsToRemove.Add(table[nRow]);
                        }
                    }

                    foreach (CustomTableRow row in rowsToRemove)
                    {
                        table.Remove(row);
                    }
                }
            }
        }

        //private void ProcessInputDocTable(List<CustomTableRow> table,
        //    int tableColumnCount,
        //    int appendixIndex,
        //    GUKV.DataMigration.ObjectFinder objectFinder,
        //    Dictionary<ColumnCategory, int> columnTypes,
        //    int firstRowIndex)
        //{
        //    int indexCol = columnTypes.ContainsKey(ColumnCategory.RowIndex) ? columnTypes[ColumnCategory.RowIndex] : -1;
        //    int addressCol = columnTypes.ContainsKey(ColumnCategory.Address) ? columnTypes[ColumnCategory.Address] : -1;

        //    // Get the data from table rows
        //    if (firstRowIndex > 0)
        //    {
        //        Appendix appendix = new Appendix();

        //        // appendix.columnTypes = columnTypes;
        //        appendix.appendixNum = appendixIndex.ToString();

        //        int prevObjectIndex = -1;
        //        string prevAddress = "";
        //        string prevObjectName = "";
        //        string[] rowValues = new string[tableColumnCount];
        //        List<string>[] rowValueGroups = new List<string>[tableColumnCount];
        //        List<AppendixObject> curObjects = new List<AppendixObject>();

        //        for (int nRow = firstRowIndex; nRow < table.Count; nRow++)
        //        {
        //            // If the rows in the table are numbered, determine the row index
        //            int objectIndex = 0;

        //            if (indexCol >= 0)
        //            {
        //                string idx = GetRowIndexFromTable(table, nRow, indexCol);

        //                idx = idx.Replace(".", "");

        //                if (idx.Length > 0)
        //                {
        //                    try
        //                    {
        //                        objectIndex = int.Parse(idx);
        //                    }
        //                    catch (Exception)
        //                    {
        //                        objectIndex = 0;
        //                    }
        //                }
        //            }

        //            // If this is a new object, clear the cache of values
        //            if (objectIndex != prevObjectIndex)
        //            {
        //                // Add all accumulated objects to the appendix
        //                ValidateAppendixObjects(curObjects);
        //                appendix.objects.AddRange(curObjects);

        //                // Clear the cache
        //                curObjects.Clear();
        //                prevAddress = "";
        //                prevObjectName = "";

        //                for (int nCol = 0; nCol < tableColumnCount; nCol++)
        //                {
        //                    rowValues[nCol] = "";
        //                }
        //            }

        //            // Get the column values of the current row
        //            int maxValueCount = 0;
        //            bool addressExtracted = false;

        //            for (int nCol = 0; nCol < tableColumnCount; nCol++)
        //            {
        //                if (columnTypes.ContainsValue(nCol))
        //                {
        //                    List<string> values = GetAppendixTableCellMultipleValues(table, nRow, nCol);

        //                    if (values != null)
        //                    {
        //                        if (values.Count > 1 && nCol == addressCol)
        //                        {
        //                            if (values[0].Length > 0)
        //                            {
        //                                prevAddress = values[0];
        //                                values.RemoveAt(0);
        //                                addressExtracted = true;
        //                            }
        //                        }

        //                        if (addressExtracted && nCol != addressCol && values.Count > 1)
        //                        {
        //                            if (values[0].Length == 0)
        //                            {
        //                                values.RemoveAt(0);
        //                            }
        //                        }

        //                        if (maxValueCount < values.Count)
        //                        {
        //                            maxValueCount = values.Count;
        //                        }
        //                    }

        //                    rowValueGroups[nCol] = values;
        //                }
        //            }

        //            for (int nCol = 0; nCol < tableColumnCount; nCol++)
        //            {
        //                if (columnTypes.ContainsValue(nCol))
        //                {
        //                    List<string> values = rowValueGroups[nCol];

        //                    if (values != null)
        //                    {
        //                        if (values.Count == 1)
        //                        {
        //                            // This value is the same for all rows in the group
        //                            while (values.Count < maxValueCount)
        //                            {
        //                                values.Add(values[0]);
        //                            }
        //                        }
        //                        else if (values.Count > 0 && values.Count < maxValueCount)
        //                        {
        //                            // Propagate the last cell value till the end of the group
        //                            while (values.Count < maxValueCount)
        //                            {
        //                                values.Add(values[values.Count - 1]);
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            for (int group = 0; group < maxValueCount; group++)
        //            {
        //                for (int nCol = 0; nCol < tableColumnCount; nCol++)
        //                {
        //                    string val = "";

        //                    List<string> values = rowValueGroups[nCol];

        //                    if (values != null && group < values.Count)
        //                    {
        //                        val = values[group];
        //                    }

        //                    if (val.Length > 0)
        //                    {
        //                        rowValues[nCol] = val.Trim();
        //                    }
        //                }

        //                curObjects.Add(CreateAppendixObject(columnTypes, rowValues, ref prevAddress, ref prevObjectName));
        //            }

        //            prevObjectIndex = objectIndex;
        //        }

        //        // Add the last accumulated objects to the appendix
        //        ValidateAppendixObjects(curObjects);
        //        appendix.objects.AddRange(curObjects);

        //        PostProcessAppendix(appendix, objectFinder);

        //        document.appendices.Add(appendix);
        //    }
        //}

        private AppendixObject CreateAppendixObject(Dictionary<ColumnCategory, int> columnTypes, string[] rowValues,
            ref string prevAddress, ref string prevObjectName)
        {
            AppendixObject obj = new AppendixObject();

            // Try to parse the object address
            int addressCol = columnTypes.ContainsKey(ColumnCategory.Address) ? columnTypes[ColumnCategory.Address] : -1;

            if (addressCol < 0)
            {
                addressCol = columnTypes.ContainsKey(ColumnCategory.ObjectName) ? columnTypes[ColumnCategory.ObjectName] : -1;
            }

            if (addressCol >= 0)
            {
                string cellText = rowValues[addressCol];

                string address = "";
                string objectName = "";

                TryParseAddress(cellText, ref address, ref objectName);

                address = address.Replace(":", "");

                if (address.Length > 0 && prevAddress.Length == 0)
                {
                    prevAddress = address;
                }

                if (objectName.Length > 0 && prevObjectName.Length == 0)
                {
                    prevObjectName = objectName;
                }

                if (address.Length == 0 && prevAddress.Length > 0)
                {
                    address = prevAddress;
                }

                if (objectName.Length == 0 && prevObjectName.Length > 0)
                {
                    objectName = prevObjectName;
                }

                obj.properties[ColumnCategory.Address] = address.ToUpper();
                obj.properties[ColumnCategory.ObjectName] = objectName.ToUpper();
            }

            // Get the other known properties
            IEnumerable<ColumnCategory> categories = Enum.GetValues(typeof(ColumnCategory)).Cast<ColumnCategory>();

            foreach (ColumnCategory category in categories)
            {
                if (category != ColumnCategory.RowIndex &&
                    category != ColumnCategory.Address)
                {
                    int columnIndex = 0;

                    if (columnTypes.TryGetValue(category, out columnIndex))
                    {
                        if (category == ColumnCategory.ObjectName)
                        {
                            // Do not overwrite the object name, if it already exists
                            if (!obj.properties.ContainsKey(ColumnCategory.ObjectName))
                            {
                                obj.properties[ColumnCategory.ObjectName] = rowValues[columnIndex].ToUpper();
                            }
                        }
                        else
                        {
                            obj.properties[category] = rowValues[columnIndex].ToUpper();
                        }
                    }
                }
            }

            return obj;
        }

        private void ValidateAppendixObjects(List<AppendixObject> objects)
        {
            if (objects != null && objects.Count > 1)
            {
                string address1 = "";
                string objectName1 = "";
                string address2 = "";
                string objectName2 = "";

                objects[0].properties.TryGetValue(ColumnCategory.Address, out address1);
                objects[0].properties.TryGetValue(ColumnCategory.ObjectName, out objectName1);

                objects[1].properties.TryGetValue(ColumnCategory.Address, out address2);
                objects[1].properties.TryGetValue(ColumnCategory.ObjectName, out objectName2);

                if (address1 == null) address1 = "";
                if (objectName1 == null) objectName1 = "";
                if (address2 == null) address2 = "";
                if (objectName2 == null) objectName2 = "";

                if (address1 == address2 && objectName2.Length > 0 && objectName1.Length == 0)
                {
                    objects.RemoveAt(0);
                }
            }
        }

        private void RemoveTotals(Appendix appendix)
        {
            List<AppendixObject> objectsToRemove = new List<AppendixObject>();

            // Check if there are objects with Address and Build Year defined
            bool definedObjectsExist = false;

            foreach (AppendixObject obj in appendix.objects)
            {
                string addr = "";
                string buildYear = "";

                if (obj.properties.TryGetValue(ColumnCategory.Address, out addr) &&
                    obj.properties.TryGetValue(ColumnCategory.BuildYear, out buildYear))
                {
                    if (addr.Length > 0 && buildYear.Length > 0 && StringHasDigits(addr) && StringHasOnlyDigits(buildYear))
                    {
                        definedObjectsExist = true;
                        break;
                    }
                }
            }

            if (definedObjectsExist)
            {
                foreach (AppendixObject obj in appendix.objects)
                {
                    string addr = "";
                    string buildYear = "";

                    if (!obj.properties.TryGetValue(ColumnCategory.Address, out addr))
                        addr = "";

                    if (!obj.properties.TryGetValue(ColumnCategory.BuildYear, out buildYear))
                        buildYear = "";

                    if (addr.Length == 0 && buildYear.Length == 0)
                    {
                        objectsToRemove.Add(obj);
                    }
                }
            }
            else
            {
                // Repeat the process with another type of year column
                foreach (AppendixObject obj in appendix.objects)
                {
                    string addr = "";
                    string explYear = "";

                    if (obj.properties.TryGetValue(ColumnCategory.Address, out addr) &&
                        obj.properties.TryGetValue(ColumnCategory.ExplYear, out explYear))
                    {
                        if (addr.Length > 0 && explYear.Length > 0 && StringHasDigits(addr) && StringHasOnlyDigits(explYear))
                        {
                            definedObjectsExist = true;
                            break;
                        }
                    }
                }

                if (definedObjectsExist)
                {
                    foreach (AppendixObject obj in appendix.objects)
                    {
                        string addr = "";
                        string explYear = "";

                        if (!obj.properties.TryGetValue(ColumnCategory.Address, out addr))
                            addr = "";

                        if (!obj.properties.TryGetValue(ColumnCategory.ExplYear, out explYear))
                            explYear = "";

                        if (addr.Length == 0 && explYear.Length == 0)
                        {
                            objectsToRemove.Add(obj);
                        }
                    }
                }
            }

            foreach (AppendixObject obj in objectsToRemove)
            {
                appendix.objects.Remove(obj);
            }
        }

        //private void PostProcessAppendix(Appendix appendix, ObjectFinder objectFinder)
        //{
        //    List<int> objectIDs = new List<int>();
        //    bool preciceMatch = false;

        //    RemoveTotals(appendix);

        //    foreach (AppendixObject obj in appendix.objects)
        //    {
        //        // Set the purpose and object kind automatically
        //        string objectName = "";

        //        if (obj.properties.TryGetValue(ColumnCategory.ObjectName, out objectName))
        //        {
        //            foreach (string objTypePattern in Properties.AppSettings.Default.ObjectTypeCodes)
        //            {
        //                // Those patterns are in the form PATTERN=PURPOSE_GROUP_ID/PURPOSE_ID/OBJ_TYPE_ID/OBJ_KIND_ID
        //                // All codes are taken from the NJF database

        //                int indexOfDivider = objTypePattern.IndexOf('=');

        //                if (indexOfDivider > 0)
        //                {
        //                    string pattern = objTypePattern.Substring(0, indexOfDivider);
        //                    string codesStr = objTypePattern.Substring(indexOfDivider + 1);

        //                    string[] codes = codesStr.Split(new char[] { '/' });

        //                    if (codes.Length == 4 && objectName.Contains(pattern))
        //                    {
        //                        int purposeGroupId = int.Parse(codes[0]);
        //                        int purposeId = int.Parse(codes[1]);
        //                        int objTypeId = int.Parse(codes[2]);
        //                        int objKindId = int.Parse(codes[3]);

        //                        obj.properties[ColumnCategory.PurposeGroup] = DB.FindNameInDictionaryNJF(DB.DICT_PURPOSE_GROUP, purposeGroupId);
        //                        obj.properties[ColumnCategory.Purpose] = DB.FindNameInDictionaryNJF(DB.DICT_PURPOSE, purposeId);
        //                        obj.properties[ColumnCategory.ObjectType] = DB.FindNameInDictionaryNJF(DB.DICT_OBJ_TYPE, objTypeId);
        //                        obj.properties[ColumnCategory.ObjectKind] = DB.FindNameInDictionaryNJF(DB.DICT_OBJ_KIND, objKindId);

        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        // Try to find the object in 1NF database
        //        string address = "";

        //        if (obj.properties.TryGetValue(ColumnCategory.Address, out address))
        //        {
        //            objectIDs.Clear();

        //            objectFinder.FindObjectIDsByInpreciseAddress(address, objectIDs, false, out preciceMatch);

        //            if (objectIDs.Count > 0)
        //            {
        //                Object1NF obj1nf = null;

        //                if (DB.objects1NF.TryGetValue(objectIDs[0], out obj1nf))
        //                {
        //                    obj.object1NF = obj1nf;
        //                }
        //            }
        //        }

        //        // Validate the numeric values
        //        ValidateDecimalProperty(obj, ColumnCategory.BalansCost);
        //        ValidateDecimalProperty(obj, ColumnCategory.FinalCost);
        //        ValidateDecimalProperty(obj, ColumnCategory.Square);
        //        ValidateDecimalProperty(obj, ColumnCategory.Length);

        //        ValidateIntegerProperty(obj, ColumnCategory.BuildYear);
        //        ValidateIntegerProperty(obj, ColumnCategory.ExplYear);

        //        // Validate the string values
        //        if (obj.properties.TryGetValue(ColumnCategory.ObjectName, out objectName))
        //        {
        //            if (objectName.Length > DB.MAX_OBJECT_NAME_LEN)
        //            {
        //                obj.properties[ColumnCategory.ObjectName] = objectName.Substring(0, DB.MAX_OBJECT_NAME_LEN);
        //            }
        //        }

        //        string diameter = "";

        //        if (obj.properties.TryGetValue(ColumnCategory.Diameter, out diameter))
        //        {
        //            if (diameter.Length > DB.MAX_DIAMETER_LEN)
        //            {
        //                obj.properties[ColumnCategory.Diameter] = diameter.Substring(0, DB.MAX_DIAMETER_LEN);
        //            }
        //        }
        //    }

        //    // Remove all empty objects
        //    int idx = 0;

        //    while (idx < appendix.objects.Count)
        //    {
        //        if (appendix.objects[idx].IsEmpty)
        //        {
        //            appendix.objects.RemoveAt(idx);
        //        }
        //        else
        //        {
        //            idx++;
        //        }
        //    }
        //}

        private void ValidateDecimalProperty(AppendixObject obj, ColumnCategory category)
        {
            string value = "";

            if (obj.properties.TryGetValue(category, out value) && value.Length > 0)
            {
                try
                {
                    DB.ConvertStrToDecimal(ref value);
                }
                catch (Exception)
                {
                    value = "";
                }

                obj.properties[category] = value;
            }
        }

        private void ValidateIntegerProperty(AppendixObject obj, ColumnCategory category)
        {
            string value = "";

            if (obj.properties.TryGetValue(category, out value) && value.Length > 0)
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
                    int i = int.Parse(strippedValue);
                }
                catch (Exception)
                {
                    strippedValue = "";
                }

                obj.properties[category] = strippedValue;
            }
        }

        private string GetRowIndexFromTable(List<CustomTableRow> table, int nRow, int nCol)
        {
            while (nRow >= 0)
            {
                string cellText = table[nRow][nCol];

                if (cellText.Length > 0)
                {
                    return cellText;
                }

                nRow--;
            }

            return "";
        }

        private List<string> GetAppendixTableCellMultipleValues(List<CustomTableRow> table, int nRow, int nCol)
        {
            try
            {
                // Check how many rows are in the cell
                string cellText = table[nRow][nCol];

                if (cellText.EndsWith("\r\a"))
                {
                    cellText = cellText.Substring(0, cellText.Length - 2);
                }

                cellText = cellText.Replace("\r", "\n");
                cellText = cellText.Replace("\v", "\n");
                cellText = cellText.Replace("\a", "");

                string[] rows = cellText.Split(new char[] { '\n' });

                if (rows.Length > 0)
                {
                    List<string> list = new List<string>();

                    foreach (string str in rows)
                    {
                        string rowText = str.Trim();

                        while (rowText.IndexOf("  ") >= 0)
                        {
                            rowText = rowText.Replace("  ", " ");
                        }

                        list.Add(rowText);
                    }

                    return list;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        private int FindRowWithNumbers(List<CustomTableRow> table)
        {
            int nRow = 1;

            while (nRow < table.Count)
            {
                bool rowContainsLetters = false;

                for (int nCol = 0; nCol < table[nRow].Count; nCol++)
                {
                    foreach (char ch in table[nRow][nCol])
                    {
                        if (!char.IsDigit(ch) && ch != '.')
                        {
                            rowContainsLetters = true;
                            break;
                        }
                    }
                }

                if (!rowContainsLetters)
                {
                    return nRow;
                }

                nRow++;
            }

            return -1;
        }

        private int FindFirstIndexedRow(List<CustomTableRow> table, int indexColumn)
        {
            int nRow = 1;

            while (nRow < table.Count)
            {
                string cellData = table[nRow][indexColumn].Trim();

                cellData = cellData.Replace(".", "");

                if (cellData.Length > 0 && StringHasOnlyDigits(cellData))
                {
                    return nRow;
                }

                nRow++;
            }

            return -1;
        }

        private int FindFirstFullRow(List<CustomTableRow> table)
        {
            int nRow = 1;

            while (nRow < table.Count)
            {
                if (table[nRow].RowIsFull)
                {
                    return nRow;
                }

                nRow++;
            }

            return -1;
        }

        private bool IsColumnTitleMatch(string columnTitle, System.Collections.Specialized.StringCollection patterns)
        {
            foreach (string pattern in patterns)
            {
                if (columnTitle.Contains(pattern))
                {
                    return true;
                }
            }

            return false;
        }

        private void TryParseAddress(string cellText, ref string address, ref string objName)
        {
            address = "";
            objName = "";

            cellText = cellText.ToUpper();

            foreach (string addrDivider in Properties.AppSettings.Default.AddressDividers)
            {
                int dividerPos = cellText.IndexOf(addrDivider);

                if (dividerPos > 0)
                {
                    objName = cellText.Substring(0, dividerPos).Trim();
                    address = RemoveAddressTail(cellText.Substring(dividerPos + addrDivider.Length)).Trim();

                    // The building number may be embedded in the object name
                    string buildingNumberDivider = " N";
                    int pos = objName.IndexOf(buildingNumberDivider);

                    if (pos >= 0)
                    {
                        string buildingNumber = objName.Substring(pos + buildingNumberDivider.Length);

                        objName = (pos > 0) ? objName.Substring(0, pos).Trim() : "";

                        if (buildingNumber.Length > 0)
                        {
                            address += ", ";
                            address += buildingNumber.Trim();
                        }
                    }

                    return;
                }
            }

            // No object description found; maybe it is a pure address
            if (IsAddress(cellText))
            {
                address = cellText;
            }
            else
            {
                // Not an address
                objName = cellText;
            }
        }

        private bool IsAddress(string str)
        {
            if (ContainsAddressPart(str, Properties.Settings.Default.StreetAvenuePrefixList))
            {
                return StringHasDigits(str);
            }
            else if (ContainsAddressPart(str, Properties.Settings.Default.StreetBoulevardPrefixList))
            {
                return StringHasDigits(str);
            }
            else if (ContainsAddressPart(str, Properties.Settings.Default.StreetLanePrefixList))
            {
                return StringHasDigits(str);
            }
            else if (ContainsAddressPart(str, Properties.Settings.Default.StreetSquarePrefixList))
            {
                return StringHasDigits(str);
            }
            else if (ContainsAddressPart(str, Properties.AppSettings.Default.StreetHighwayPrefixList))
            {
                return StringHasDigits(str);
            }
            else if (ContainsAddressPart(str, Properties.AppSettings.Default.StreetRiverBankPrefixList))
            {
                return StringHasDigits(str);
            }
            else if (str.Contains(Properties.AppResources.StreetAddrPrefix))
            {
                return StringHasDigits(str);
            }

            return false;
        }

        private bool ContainsAddressPart(string str, System.Collections.Specialized.StringCollection prefixes)
        {
            foreach (string prefix in prefixes)
            {
                if (str.Contains(prefix))
                {
                    return true;
                }
            }

            return false;
        }

        private string RemoveAddressTail(string address)
        {
            if (address.StartsWith("."))
            {
                address = address.Substring(1);
            }

            string[] parts = address.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Find the first part with digit; this is going to be a start of building number
            int buildingNumPart = 0;

            while (buildingNumPart < parts.Length)
            {
                if (StringHasDigits(parts[buildingNumPart]))
                {
                    break;
                }

                buildingNumPart++;
            }

            if (buildingNumPart < parts.Length)
            {
                // Find the first word after the building number
                int pos = buildingNumPart + 1;

                while (pos < parts.Length)
                {
                    if (StringHasOnlyLetters(parts[pos]) && parts[pos].Length > 1)
                    {
                        // Return all the parts before the first word
                        string result = "";

                        for (int i = 0; i < pos; i++)
                        {
                            if (result.Length > 0)
                                result += " ";

                            result += parts[i];
                        }

                        return result;
                    }

                    pos++;
                }
            }

            return address;
        }

        private bool StringHasDigits(string str)
        {
            foreach (char ch in str)
            {
                if (char.IsDigit(ch))
                {
                    return true;
                }
            }

            return false;
        }

        private bool StringHasOnlyLetters(string str)
        {
            foreach (char ch in str)
            {
                if (!char.IsLetter(ch))
                {
                    return false;
                }
            }

            return true;
        }

        private bool StringHasOnlyDigits(string str)
        {
            foreach (char ch in str)
            {
                if (!char.IsDigit(ch))
                {
                    return false;
                }
            }

            return true;
        }

        private void ReadInputExcelDoc(string fileName)
        {
            /*
            // Create the Excel application
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

            app.Visible = false;
            
            try
            {
                // Open the source document
                Microsoft.Office.Interop.Excel._Workbook book = app.Workbooks.Open(fileName);

                if (book != null)
                {
                    try
                    {
                        // Assume that table is located at the first sheet
                        if (book.Sheets.Count > 0)
                        {
                            Microsoft.Office.Interop.Excel.Worksheet sheet = book.Sheets[1];

                            // Import all the used cells
                            Microsoft.Office.Interop.Excel.Range range = sheet.UsedRange;

                            object[,] valueArray = (object[,])range.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);

                            int nRowCount = valueArray.GetLength(0);
                            int nColCount = valueArray.GetLength(1);

                            if (nRowCount > 0 && nColCount > 0)
                            {
                                // Duplicate all columns in our Grid
                                for (int nCol = 1; nCol <= nColCount; nCol++)
                                {
                                    gridAddressTable.Columns.Add(SourceColumnPrefix + nCol.ToString(), "");
                                    gridAddressTable.Columns[gridAddressTable.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                                    gridAddressTable.Columns[gridAddressTable.Columns.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                                }

                                // Copy data from all the Table rows to our Grid
                                object[] rowData = new object[nColCount];

                                for (int nRow = 0; nRow < nRowCount; nRow++)
                                {
                                    for (int nCell = 0; nCell < nColCount; nCell++)
                                    {
                                        object value = valueArray[nRow + 1, nCell + 1];

                                        rowData[nCell] = value != null ? value.ToString() : "";
                                    }

                                    gridAddressTable.Rows.Add(rowData);

                                    // Update the progress bar
                                    double progress = (double)nRow / (double)nRowCount;
                                    int newValue = (int)((double)readingProgressBar.Maximum * progress) + 1;

                                    readingProgressBar.Value = (newValue > readingProgressBar.Maximum) ? readingProgressBar.Maximum : newValue;
                                    readingProgressBar.Update();
                                }
                            }
                        }
                    }
                    finally
                    {
                        book.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Properties.AppResources.DocOpenError + ": " + ex.Message,
                    Properties.AppResources.ErrorCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                DeleteCurrentGridData();
            }

            readingProgressBar.Value = 0;
            readingProgressBar.Update();

            app.Quit();
            */
        }

        private string RemoveSpecialChars(string source)
        {
            source = source.Replace("\r", " ");
            source = source.Replace("\n", " ");
            source = source.Replace("\a", " ");
            source = source.Replace("\v", " ");

            while (source.IndexOf("  ") >= 0)
            {
                source = source.Replace("  ", " ");
            }

            return source.Trim();
        }

        #endregion (Document reading)

        #region User Interface

        private void FillPrimaryControls()
        {
            listAppendices.Items.Clear();

            DictionaryData dictDocTypes = null;
            DB.dictionaries.TryGetValue(DB.DICT_DOC_TYPES, out dictDocTypes);

            // Clear all the fields
            editDocNumber.Text = "";
            editDocTitle.Text = "";
            editDocDate.Value = DateTime.Now;
            comboDocType.SelectedIndex = -1;
            listAppendices.Items.Clear();

            editMasterDocNumber.Text = "";
            editMasterDocDate.Value = DateTime.Now;

            if (document != null)
            {
                editDocNumber.Text = document.docNum;
                editDocTitle.Text = document.docTitle;

                if (document.docDate >= editDocDate.MinDate)
                    editDocDate.Value = document.docDate;

                // Select the document type
                if (document.docTypeId > 0 && dictDocTypes != null)
                {
                    DictionaryValue docTypeText = new DictionaryValue();

                    if (dictDocTypes.ValuesNJF.TryGetValue(document.docTypeId, out docTypeText))
                    {
                        comboDocType.SelectedItem = docTypeText;
                    }
                }

                comboDocType_SelectedIndexChanged(this, new EventArgs());

                // Fill the list of apendices
                foreach (Appendix app in document.appendices)
                {
                    listAppendices.Items.Add(app);
                }
            }
        }

        private void comboDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboDocType.SelectedItem is DictionaryValue)
            {
                int docKind = (comboDocType.SelectedItem as DictionaryValue).key;

                if (docKind == 3)
                {
                    labelMasterDoc.Text = Properties.AppResources.LabelMasterDocAct;
                }
                else
                {
                    labelMasterDoc.Text = Properties.AppResources.LabelMasterDocRozp;
                }
            }
        }

        private void btnOpenAppendix_Click(object sender, EventArgs e)
        {
            if (listAppendices.SelectedIndex >= 0)
            {
                AppendixForm form = new AppendixForm();

                form.SetAppendix(listAppendices.SelectedItem as Appendix);

                form.ShowDialog();
            }
            else
            {
                MessageBox.Show(
                    "Будь ласка, виберіть Додаток.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void listAppendices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnOpenAppendix_Click(sender, e);
        }

        private void btnAddAppendix_Click(object sender, EventArgs e)
        {
            if (document == null)
            {
                document = new ImportedDoc();
            }

            // Determine a unique number for the new appendix
            int appNum = 1;

            while (true)
            {
                // Check if Appendix with such number already exists
                bool appendixExists = false;

                foreach (Appendix app in document.appendices)
                {
                    if (app.appendixNum == appNum.ToString())
                    {
                        appendixExists = true;
                        break;
                    }
                }

                if (!appendixExists)
                {
                    break;
                }

                appNum++;
            }

            // Create a new appendix
            Appendix appendix = new Appendix();

            appendix.appendixNum = appNum.ToString();

            document.appendices.Add(appendix);

            listAppendices.Items.Add(appendix);
        }

        private void btnDelAppendix_Click(object sender, EventArgs e)
        {
            if (document == null)
            {
                document = new ImportedDoc();
            }

            if (listAppendices.SelectedItem is Appendix)
            {
                Appendix appendix = listAppendices.SelectedItem as Appendix;

                if (MessageBox.Show(
                    "Видалити Додаток " + appendix.appendixNum + "?",
                    "Видалення Додатку",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    document.appendices.Remove(appendix);

                    listAppendices.SelectedIndex = -1;
                    listAppendices.Items.Remove(appendix);
                }
            }
            else
            {
                MessageBox.Show(
                    "Будь ласка, виберіть Додаток, який необхідно видалити.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            if (document == null)
            {
                document = new ImportedDoc();
            }

            // Check if there are some objects which are not associated to 1NF
            bool unassignedObjectsExist = false;
            bool objectsWithoutTransfersExist = false;

            foreach (Appendix app in document.appendices)
            {
                foreach (AppendixObject obj in app.objects)
                {
                    if (obj.object1NF == null)
                    {
                        unassignedObjectsExist = true;
                    }

                    if (obj.transfers.Count == 0)
                    {
                        objectsWithoutTransfersExist = true;
                    }

                    if (unassignedObjectsExist && objectsWithoutTransfersExist)
                    {
                        break;
                    }
                }

                if (unassignedObjectsExist && objectsWithoutTransfersExist)
                {
                    break;
                }
            }

            if (unassignedObjectsExist)
            {
                if (System.Windows.Forms.MessageBox.Show("В документі є об'єкти, які не привязані до БД Розпорядження." +
                        " Під час завантаження такі об'єкти будуть пропущені. Ви дійсно хочете завантажити документ до бази даних?",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            if (objectsWithoutTransfersExist)
            {
                if (System.Windows.Forms.MessageBox.Show("В документі є об'єкти, для яких не вказані права. Ви дійсно хочете завантажити документ до бази даних?",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            // Validate the entered information
            document.docNum = editDocNumber.Text.Trim().ToUpper();
            document.docTitle = editDocTitle.Text.Trim().ToUpper();
            document.docDate = editDocDate.Value.Date;
            document.docTypeId = -1;

            if (document.docNum.Length > DB.MAX_DOC_NUMBER_LEN)
                document.docNum = document.docNum.Substring(0, DB.MAX_DOC_NUMBER_LEN);

            if (document.docTitle.Length > DB.MAX_DOC_TITLE_LEN)
                document.docTitle = document.docTitle.Substring(0, DB.MAX_DOC_TITLE_LEN);

            if (comboDocType.SelectedItem is DictionaryValue)
            {
                document.docTypeId = (comboDocType.SelectedItem as DictionaryValue).key;
            }

            if (document.docNum.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть номер документу.",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (document.docTitle.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть назву документу.",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (document.docTypeId < 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть тип документу.",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // If Act is loaded, the primary document must be specified
            document.masterDocNum = editMasterDocNumber.Text.Trim().ToUpper();
            document.masterDocDate = editMasterDocDate.Value.Date;

            if (document.docTypeId == 3)
            {
                if (document.masterDocNum.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("При введенні Акту необхідно вказати номер та дату Рішення або Розпорядження, до якого відноситься Акт.",
                        "Перевірка Документу",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // Check the document dependencies
            if (document.masterDocNum.Length > 0)
            {
                int masterDocId = DB.FindDocument(document.masterDocNum, document.masterDocDate, -1);

                if (masterDocId < 0)
                {
                    string msg = string.Format("Документ № {0} від {1} не існує в БД 'Розпорядження'." +
                        " Для завантаження підпорядкованого документу (або Акту) необхідно спочатку завантажити головний документ.",
                        document.masterDocNum, document.masterDocDate.ToShortDateString());

                    System.Windows.Forms.MessageBox.Show(msg,
                        "Перевірка Документу",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // We can not load a document which already exists
            if (document.docTypeId == 3)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, для введення Актів користуйтесь пунктом меню 'Створити Акт'.",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;

                /*
                int existingDocId = DB.FindActByNumber(document.docNum, document.docDate);

                if (existingDocId >= 0)
                {
                    System.Windows.Forms.MessageBox.Show("Акт зі вказаним номером вже введено в БД 'Розпорядження'. Повторне завантаження не є можливим.",
                        "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
                */
            }
            else
            {
                int existingDocId = DB.FindDocument(document.docNum, document.docDate, -1);

                if (existingDocId >= 0)
                {
                    string msg = string.Format("Документ № {0} від {1} вже введено в БД 'Розпорядження'. Повторне завантаження не є можливим.",
                        document.docNum, document.docDate.ToShortDateString());

                    System.Windows.Forms.MessageBox.Show(msg,
                        "Перевірка Документу",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // Parse the document cost values
            try
            {
                string docSumTxt = editDocSum.Text.Trim();

                if (docSumTxt.Length > 0)
                {
                    document.docSum = DB.ConvertStrToDecimal(ref docSumTxt);
                }
                else
                {
                    document.docSum = 0m;
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Сума за Документом'",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            try
            {
                string docFinalSumTxt = editDocFinalSum.Text.Trim();

                if (docFinalSumTxt.Length > 0)
                {
                    document.docFinalSum = DB.ConvertStrToDecimal(ref docFinalSumTxt);
                }
                else
                {
                    document.docFinalSum = 0m;
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Залишкова сума за Документом'",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // All verifications done; export the document
            using (ConfirmUploadForm confirmForm = new ConfirmUploadForm())
            {
                if (confirmForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DB.ExportDocument(preferences, document);
                }
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (document != null)
            {
                if (System.Windows.Forms.MessageBox.Show("При відкритті нового документу усі дані з поточного документу будуть втрачені. Продовжувати?",
                        "Відкриття розпорядчого документу",
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            if (openInputFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Check the document format
                string fileName = openInputFileDialog.FileName.ToLower();

                if (fileName.EndsWith(".doc") || fileName.EndsWith(".docx"))
                {
                    //ReadInputWordDoc(openInputFileDialog.FileName, true);
                }
                else if (fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx"))
                {
                    ReadInputExcelDoc(openInputFileDialog.FileName);
                }

                FillPrimaryControls();
            }
        }

        private void btnUploadAdd_Click(object sender, EventArgs e)
        {
            if (openInputFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Check the document format
                string fileName = openInputFileDialog.FileName.ToLower();

                if (fileName.EndsWith(".doc") || fileName.EndsWith(".docx"))
                {
                    //ReadInputWordDoc(openInputFileDialog.FileName, false);
                }
                else if (fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx"))
                {
                    ReadInputExcelDoc(openInputFileDialog.FileName);
                }

                // Update the list of appendices
                listAppendices.Items.Clear();

                foreach (Appendix app in document.appendices)
                {
                    listAppendices.Items.Add(app);
                }
            }
        }

        private void btnEditAct_Click(object sender, EventArgs e)
        {
            using (ActObjectsForm actForm = new ActObjectsForm())
            {
                actForm.ShowDialog();
            }
        }

        private void btnSaveProject_Click(object sender, EventArgs e)
        {
            if (document != null)
            {
                if (saveProjectDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Stream stream = File.Open(saveProjectDialog.FileName, FileMode.Create);

                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, document);

                    stream.Close();

                    System.Windows.Forms.MessageBox.Show("Дані збережено.",
                        "Збереження даних",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Дані для збереження відсутні. Спочатку необхідно відкрити розпорядчий документ.",
                    "Збереження даних",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        private void btnLoadProject_Click(object sender, EventArgs e)
        {
            if (document != null)
            {
                if (System.Windows.Forms.MessageBox.Show("При відкритті збережених даних будуть втачені усі дані з поточного документу. Продовжувати?",
                        "Відкриття збережених даних",
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            if (openProjectDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Stream stream = File.Open(openProjectDialog.FileName, FileMode.Open);

                BinaryFormatter formatter = new BinaryFormatter();

                document = formatter.Deserialize(stream) as ImportedDoc;

                stream.Close();
            }

            FillPrimaryControls();
        }

        #endregion (User Interface)
    }
}
