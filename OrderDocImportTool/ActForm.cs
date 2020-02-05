using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using GUKV.ImportToolUtils;

namespace GUKV.DataMigration
{
    public partial class ActForm : Form
    {
        public ImportedAct act = new ImportedAct();
        public DocumentNJF rishennya = null;

        private int makeChangesTo1NFColumnIndex = -1;
        private int transferColumnIndex = -1;

        public ActForm()
        {
            InitializeComponent();
            AddGridColumns();
        }

        public string RishNumber
        {
            set
            {
                editRishNumber.Text = value;
            }
        }

        public DateTime RishDate
        {
            set
            {
                editRishDate.Value = value;
            }
        }

        public void MatchActObjectsTo1NF(ObjectFinder objectFinder1NF, OrganizationFinder orgFinder1NF)
        {
            foreach (ActObject obj in act.actObjects)
            {
                obj.PerformMatchingTo1NF(objectFinder1NF, orgFinder1NF);
            }
        }

        private void AddGridColumns()
        {
            AddTextColumn("Адреса Об'єкту");
            AddTextColumn("Назва Об'єкту");
            AddTextColumn("Площа Об'єкту");
            AddTextColumn("Вид Об'єкту");
            AddTextColumn("Тип Об'єкту");
            AddTextColumn("Група Призначення");
            AddTextColumn("Призначення");

            DataGridViewCheckBoxColumn exportTo1NF = new DataGridViewCheckBoxColumn();

            exportTo1NF.HeaderText = "Внести зміни в 1НФ";
            exportTo1NF.FalseValue = 0;
            exportTo1NF.TrueValue = 1;

            gridObjects.Columns.Add(exportTo1NF);

            AddTextColumn("Передача прав");
        }

        private void AddTextColumn(string caption)
        {
            gridObjects.Columns.Add(caption, caption);
            gridObjects.Columns[gridObjects.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridObjects.Columns[gridObjects.Columns.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        public void FillGrid()
        {
            gridObjects.Rows.Clear();

            // Add a row for each object
            object[] rowData = new object[gridObjects.Columns.Count];

            for (int i = 0; i < act.actObjects.Count; i++)
            {
                GetRowValuesForObject(act.actObjects[i], rowData);

                gridObjects.Rows.Add(rowData);

                UpdateRowColors(i, act.actObjects[i]);
            }
        }

        private void UpdateGridRow(int nRow)
        {
            if (gridObjects.Rows.Count > nRow && act.actObjects.Count > nRow)
            {
                ActObject obj = act.actObjects[nRow];

                object[] rowData = new object[gridObjects.Columns.Count];

                GetRowValuesForObject(obj, rowData);

                gridObjects.Rows[nRow].SetValues(rowData);

                UpdateRowColors(nRow, obj);
            }
        }

        private void GetRowValuesForObject(ActObject obj, object[] rowData)
        {
            for (int j = 0; j < rowData.Length; j++)
            {
                rowData[j] = null;
            }

            // Address
            rowData[0] = obj.ToString();

            // Object name
            rowData[1] = obj.objectName_NJF;

            // Square
            if (obj.objectSquare_NJF is decimal)
            {
                rowData[2] = ((decimal)obj.objectSquare_NJF).ToString("F2");
            }

            // Object Kind
            rowData[3] = DB.FindNameInDictionaryNJF(DB.DICT_OBJ_KIND, obj.objectKindIdNJF);

            // Object Type
            rowData[4] = DB.FindNameInDictionaryNJF(DB.DICT_OBJ_TYPE, obj.objectTypeIdNJF);

            // Purpose Group
            rowData[5] = DB.FindNameInDictionaryNJF(DB.DICT_PURPOSE_GROUP, obj.purposeGroupIdNJF);

            // Purpose
            rowData[6] = DB.FindNameInDictionaryNJF(DB.DICT_PURPOSE, obj.purposeIdNJF);

            // Make changes to 1NF ?
            rowData[7] = obj.makeChangesIn1NF ? 1 : 0;

            // Balans transfers
            rowData[8] = obj.FormatBalansTransfersShort();

            makeChangesTo1NFColumnIndex = 7;
            transferColumnIndex = 8;
        }

        private void UpdateRowColors(int nRow, ActObject actObj)
        {
            if (!actObj.AllTransfersAreValid())
            {
                gridObjects.Rows[nRow].Cells[transferColumnIndex].Style.ForeColor = Color.Red;
                gridObjects.Rows[nRow].Cells[transferColumnIndex].Style.SelectionForeColor = Color.Red;
            }
            else
            {
                gridObjects.Rows[nRow].Cells[transferColumnIndex].Style.ForeColor = Color.Black;
                gridObjects.Rows[nRow].Cells[transferColumnIndex].Style.SelectionForeColor = Color.Black;
            }

            gridObjects.Rows[nRow].Cells[transferColumnIndex].ToolTipText = actObj.FormatBalansTransfers();
        }

        private void gridObjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < gridObjects.Rows.Count)
            {
                if (e.ColumnIndex >= 0 && e.ColumnIndex < gridObjects.Columns.Count)
                {
                    object cell = gridObjects.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    if (cell is DataGridViewCheckBoxCell)
                    {
                        DataGridViewCheckBoxCell cbCell = cell as DataGridViewCheckBoxCell;

                        if (cbCell.Value.Equals(cbCell.TrueValue))
                        {
                            cbCell.Value = cbCell.FalseValue;
                        }
                        else
                        {
                            cbCell.Value = cbCell.TrueValue;
                        }

                        if (e.ColumnIndex == makeChangesTo1NFColumnIndex)
                        {
                            act.actObjects[e.RowIndex].makeChangesIn1NF = (cbCell.Value == cbCell.TrueValue);
                        }
                    }
                }
            }
        }

        private void gridObjects_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                using (ActObjectPropertiesForm propForm = new ActObjectPropertiesForm())
                {
                    propForm.SetActObject(act.actObjects[e.RowIndex]);

                    if (propForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        UpdateGridRow(e.RowIndex);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridObjects.SelectedRows.Count > 0)
            {
                if (System.Windows.Forms.MessageBox.Show("Видалити вибраний об'єкт?",
                        "Видалення Об'єкту",
                        System.Windows.Forms.MessageBoxButtons.OKCancel,
                        System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                {
                    int nRowIndex = -1;

                    foreach (DataGridViewRow row in gridObjects.SelectedRows)
                    {
                        nRowIndex = row.Index;
                        break;
                    }

                    if (nRowIndex >= 0)
                    {
                        gridObjects.Rows.RemoveAt(nRowIndex);
                        act.actObjects.RemoveAt(nRowIndex);
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("будь ласка, виберіть об'єкт, який необхідно видалити.",
                    "Видалення Об'єкту",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void btnAddObject_Click(object sender, EventArgs e)
        {
            using (AddActObjectForm addForm = new AddActObjectForm())
            {
                if (addForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    act.actObjects.Add(addForm.actObject);

                    FillGrid();
                }
            }
        }

        private void btnEditObject_Click(object sender, EventArgs e)
        {
            if (gridObjects.SelectedRows.Count > 0)
            {
                int nRowIndex = -1;

                foreach (DataGridViewRow row in gridObjects.SelectedRows)
                {
                    nRowIndex = row.Index;
                    break;
                }

                if (nRowIndex >= 0)
                {
                    using (ActObjectPropertiesForm propForm = new ActObjectPropertiesForm())
                    {
                        propForm.SetActObject(act.actObjects[nRowIndex]);

                        if (propForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            UpdateGridRow(nRowIndex);
                        }
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("будь ласка, виберіть об'єкт, який необхідно редагувати.",
                    "Редагування Об'єкту",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            if (act == null)
            {
                act = new ImportedAct();
            }

            // Validate the entered information
            act.docNum = editActNumber.Text.Trim().ToUpper();
            act.docTitle = editActTitle.Text.Trim().ToUpper();
            act.docDate = editActDate.Value.Date;

            if (act.docNum.Length > DB.MAX_DOC_NUMBER_LEN)
                act.docNum = act.docNum.Substring(0, DB.MAX_DOC_NUMBER_LEN);

            if (act.docTitle.Length > DB.MAX_DOC_TITLE_LEN)
                act.docTitle = act.docTitle.Substring(0, DB.MAX_DOC_TITLE_LEN);

            if (act.docNum.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть номер Акту.",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (act.docTitle.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть назву Акту.",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            string masterDocNum = editRishNumber.Text.Trim().ToUpper();
            DateTime masterDocDate = editRishDate.Value.Date;

            if (masterDocNum.Length == 0 || rishennya == null)
            {
                System.Windows.Forms.MessageBox.Show("При введенні Акту необхідно вибрати Рішення або Розпорядження, до якого відноситься Акт.",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // Check if there are some objects can not be exported to 1NF
            bool undefinedObjectsExist = false;

            foreach (ActObject obj in act.actObjects)
            {
                if (obj.makeChangesIn1NF)
                {
                    foreach (BalansTransfer bt in obj.balansTransfers)
                    {
                        if (!bt.IsFullyDefined())
                        {
                            undefinedObjectsExist = true;
                            break;
                        }
                    }
                }

                if (undefinedObjectsExist)
                {
                    break;
                }
            }

            if (undefinedObjectsExist)
            {
                System.Windows.Forms.MessageBox.Show("В Акті є об'єкти, які необхідно передати з балансу на баланс в базі '1НФ'. Всі такі об'єкти мають бути коректно заповнені. Експорт Акту не є можливим.",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // We can not load a document which already exists
            int existingDocId = DB.FindActByNumber(act.docNum, act.docDate);

            if (existingDocId >= 0)
            {
                System.Windows.Forms.MessageBox.Show("Акт зі вказаним номером вже введено в БД 'Розпорядження'. Повторне завантаження не є можливим.",
                    "Перевірка Документу",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // Parse the document cost values
            try
            {
                string docSumTxt = editActSum.Text.Trim();

                if (docSumTxt.Length > 0)
                {
                    act.docSum = DB.ConvertStrToDecimal(ref docSumTxt);
                }
                else
                {
                    act.docSum = 0m;
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Сума за Актом'",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            try
            {
                string docFinalSumTxt = editActFinalSum.Text.Trim();

                if (docFinalSumTxt.Length > 0)
                {
                    act.docFinalSum = DB.ConvertStrToDecimal(ref docFinalSumTxt);
                }
                else
                {
                    act.docFinalSum = 0m;
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Залишкова сума за Актом'",
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
                    if (DB.ExportAct(MainForm.preferences, act, rishennya))
                    {
                        if (DB.TransferBalansObjects(MainForm.preferences, act, rishennya, checkModify1NFImmediately.Checked))
                        {
                            System.Windows.Forms.MessageBox.Show("Завантаження даних до бази 'Розпорядження' завершено успішно.",
                                "Завантаження даних",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
    }
}
