using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUKV.DataMigration
{
    public partial class AppendixForm : Form
    {
        private Appendix appendix = null;

        private Dictionary<ColumnCategory, int> columnIndices = new Dictionary<ColumnCategory, int>();

        public AppendixForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void SetAppendix(Appendix app)
        {
            appendix = app;

            editAppendixNumber.Text = appendix.appendixNum;

            AddGridColumns();
            FillGridOfObjects();
        }

        private void AddGridColumns()
        {
            columnIndices.Clear();

            IEnumerable<ColumnCategory> categories = Enum.GetValues(typeof(ColumnCategory)).Cast<ColumnCategory>();

            foreach (ColumnCategory category in categories)
            {
                if (category != ColumnCategory.RowIndex)
                {
                    string caption = GetColumnTitle(category);

                    gridAppendixObjects.Columns.Add(caption, caption);
                    gridAppendixObjects.Columns[gridAppendixObjects.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    gridAppendixObjects.Columns[gridAppendixObjects.Columns.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    columnIndices[category] = gridAppendixObjects.Columns.Count - 1;
                }
            }
        }

        private void FillGridOfObjects()
        {
            gridAppendixObjects.Rows.Clear();

            // Add a row for each object
            object[] rowData = new object[gridAppendixObjects.Columns.Count];

            for (int i = 0; i < appendix.objects.Count; i++)
            {
                GetRowValuesForObject(appendix.objects[i], rowData);

                gridAppendixObjects.Rows.Add(rowData);

                UpdateRowColors(gridAppendixObjects.Rows.Count - 1, appendix.objects[i]);
            }
        }

        private void UpdateGridRow(int rowIndex)
        {
            if (gridAppendixObjects.Rows.Count > rowIndex &&
                appendix.objects.Count > rowIndex)
            {
                AppendixObject obj = appendix.objects[rowIndex];

                object[] rowData = new object[gridAppendixObjects.Columns.Count];

                GetRowValuesForObject(obj, rowData);

                gridAppendixObjects.Rows[rowIndex].SetValues(rowData);

                UpdateRowColors(rowIndex, obj);
            }
        }

        private void GetRowValuesForObject(AppendixObject obj, object[] rowData)
        {
            for (int j = 0; j < rowData.Length; j++)
            {
                rowData[j] = null;
            }

            foreach (KeyValuePair<ColumnCategory, int> category in columnIndices)
            {
                string value = "";

                if (obj.properties.TryGetValue(category.Key, out value))
                {
                    rowData[category.Value] = value;
                }
                else if (category.Key == ColumnCategory.Associated1NFObject)
                {
                    if (obj.object1NF != null)
                    {
                        rowData[category.Value] = obj.object1NF.ToString();
                    }
                    else
                    {
                        rowData[category.Value] = Properties.AppResources.AppendixUnmatchedObject;
                    }
                }
                else if (category.Key == ColumnCategory.Transfers)
                {
                    if (obj.transfers.Count > 0)
                    {
                        rowData[category.Value] = Properties.AppResources.AppendixTransfersExist;
                    }
                    else
                    {
                        rowData[category.Value] = Properties.AppResources.AppendixTransfersMissing;
                    }
                }
            }
        }

        private void UpdateRowColors(int nRow, AppendixObject obj)
        {
            int nCol = 0;

            if (columnIndices.TryGetValue(ColumnCategory.Associated1NFObject, out nCol))
            {
                if (gridAppendixObjects.Rows[nRow].Cells[nCol].Value is string)
                {
                    string cellText = (string)gridAppendixObjects.Rows[nRow].Cells[nCol].Value;

                    if (cellText == Properties.AppResources.AppendixUnmatchedObject)
                    {
                        gridAppendixObjects.Rows[nRow].Cells[nCol].Style.ForeColor = Color.Red;
                        gridAppendixObjects.Rows[nRow].Cells[nCol].Style.SelectionForeColor = Color.Red;
                    }
                    else
                    {
                        gridAppendixObjects.Rows[nRow].Cells[nCol].Style.ForeColor = Color.Black;
                        gridAppendixObjects.Rows[nRow].Cells[nCol].Style.SelectionForeColor = Color.Black;
                    }
                }
            }

            if (columnIndices.TryGetValue(ColumnCategory.Transfers, out nCol))
            {
                if (gridAppendixObjects.Rows[nRow].Cells[nCol].Value is string)
                {
                    string cellText = (string)gridAppendixObjects.Rows[nRow].Cells[nCol].Value;

                    if (cellText == Properties.AppResources.AppendixTransfersMissing)
                    {
                        gridAppendixObjects.Rows[nRow].Cells[nCol].Style.ForeColor = Color.Red;
                        gridAppendixObjects.Rows[nRow].Cells[nCol].Style.SelectionForeColor = Color.Red;

                        gridAppendixObjects.Rows[nRow].Cells[nCol].ToolTipText = "";
                    }
                    else
                    {
                        gridAppendixObjects.Rows[nRow].Cells[nCol].Style.ForeColor = Color.Black;
                        gridAppendixObjects.Rows[nRow].Cells[nCol].Style.SelectionForeColor = Color.Black;

                        gridAppendixObjects.Rows[nRow].Cells[nCol].ToolTipText = obj.FormatTransfers();
                    }
                }
            }
        }

        private static string GetColumnTitle(ColumnCategory category)
        {
            switch (category)
            {
                case ColumnCategory.Address:
                    return Properties.AppResources.AppendixColTitleAddress;

                case ColumnCategory.Associated1NFObject:
                    return Properties.AppResources.AppendixColTitleObject1NF;

                case ColumnCategory.Transfers:
                    return Properties.AppResources.AppendixColTitleTransfers;

                case ColumnCategory.ObjectName:
                    return Properties.AppResources.AppendixColTitleObjName;

                case ColumnCategory.ObjectType:
                    return Properties.AppResources.AppendixColTitleObjType;

                case ColumnCategory.ObjectKind:
                    return Properties.AppResources.AppendixColTitleObjKind;

                case ColumnCategory.Purpose:
                    return Properties.AppResources.AppendixColTitlePurpose;

                case ColumnCategory.PurposeGroup:
                    return Properties.AppResources.AppendixColTitlePurposeGroup;

                case ColumnCategory.BalansCost:
                    return Properties.AppResources.AppendixColTitleBalansCost;

                case ColumnCategory.FinalCost:
                    return Properties.AppResources.AppendixColTitleFinalCost;

                case ColumnCategory.Square:
                    return Properties.AppResources.AppendixColTitleSquare;

                case ColumnCategory.Length:
                    return Properties.AppResources.AppendixColTitleLength;

                case ColumnCategory.Diameter:
                    return Properties.AppResources.AppendixColTitleDiameter;

                case ColumnCategory.Characteristics:
                    return Properties.AppResources.AppendixColTitleCharacteristics;

                case ColumnCategory.BuildYear:
                    return Properties.AppResources.AppendixColTitleBuildYear;

                case ColumnCategory.ExplYear:
                    return Properties.AppResources.AppendixColTitleExplYear;
            }

            return "";
        }

        private void gridAppendixObjects_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            using (ObjectPropertiesForm objPropertiesForm = new ObjectPropertiesForm())
            {
                objPropertiesForm.SetAppendixObject(appendix.objects[e.RowIndex]);

                if (objPropertiesForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UpdateGridRow(e.RowIndex);
                }
            }
        }

        private void btnAddObject_Click(object sender, EventArgs e)
        {
            appendix.objects.Add(new AppendixObject());

            gridAppendixObjects.Rows.Add();
        }

        private void btnCopyObject_Click(object sender, EventArgs e)
        {
            if (gridAppendixObjects.SelectedCells.Count > 0)
            {
                List<int> rows = GetSelectedRows();

                rows.Sort();

                for (int i = 0; i < rows.Count; i++)
                {
                    AppendixObject obj = appendix.objects[rows[i]];

                    appendix.objects.Add(obj.MakeCopy());

                    gridAppendixObjects.Rows.Add();

                    UpdateGridRow(gridAppendixObjects.Rows.Count - 1);
                }
            }
            else
            {
                MessageBox.Show(
                    "Будь ласка, виберіть об'єкти, які необхідно скопіювати.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDelObject_Click(object sender, EventArgs e)
        {
            if (gridAppendixObjects.SelectedCells.Count > 0)
            {
                if (MessageBox.Show(
                    "Видалити обрані об'єкти?",
                    "Видалення Об'єктів",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    List<int> rows = GetSelectedRows();

                    rows.Sort();

                    // Delete the rows from bottom to top
                    for (int i = rows.Count - 1; i >= 0; i--)
                    {
                        appendix.objects.RemoveAt(rows[i]);

                        gridAppendixObjects.Rows.RemoveAt(rows[i]);
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "Будь ласка, виберіть об'єкти, які необхідно видалити.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnEditObject_Click(object sender, EventArgs e)
        {
            List<int> rows = GetSelectedRows();

            if (rows.Count == 1)
            {
                using (ObjectPropertiesForm objPropertiesForm = new ObjectPropertiesForm())
                {
                    objPropertiesForm.SetAppendixObject(appendix.objects[rows[0]]);

                    if (objPropertiesForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        UpdateGridRow(rows[0]);
                    }
                }
            }
            else if (rows.Count > 1)
            {
                List<AppendixObject> objList = new List<AppendixObject>();

                for (int i = 0; i < rows.Count; i++)
                {
                    objList.Add(appendix.objects[rows[i]]);
                }

                using (ObjGroupPropertiesForm form = new ObjGroupPropertiesForm())
                {
                    form.SetObjects(objList);

                    form.ShowDialog();
                }

                for (int i = 0; i < rows.Count; i++)
                {
                    UpdateGridRow(rows[i]);
                }
            }
            else
            {
                MessageBox.Show(
                    "Будь ласка, виберіть об'єкти, які необхідно редагувати.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private List<int> GetSelectedRows()
        {
            List<int> rows = new List<int>();

            foreach (DataGridViewRow row in gridAppendixObjects.SelectedRows)
            {
                if (!rows.Contains(row.Index))
                {
                    rows.Add(row.Index);
                }
            }

            return rows;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            gridAppendixObjects.SelectAll();
        }
    }
}
