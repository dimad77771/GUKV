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
    public partial class ActObjectsForm : Form
    {
        private ImportedAct act = new ImportedAct();
        private DocumentNJF rishennya = null;

        public ActObjectsForm()
        {
            InitializeComponent();
            AddGridColumns();
        }

        private void btnImportRish_Click(object sender, EventArgs e)
        {
            using (RishPickerForm picker = new RishPickerForm())
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (picker.selectedDocument != null)
                    {
                        rishennya = picker.selectedDocument;

                        // Set the Rishennya properties
                        editRishNumber.Text = rishennya.documentNumber;
                        editRishDate.Value = rishennya.documentDate;

                        // Import all objects from the Rishennya
                        act.actObjects = DB.GetDocObjects(MainForm.preferences, rishennya.documentId);

                        act.actObjects.Sort(new ActObjectAlphabeticalComparer());

                        FillGrid();
                    }
                }
            }
        }

        private void AddGridColumns()
        {
            DataGridViewCheckBoxColumn useObject = new DataGridViewCheckBoxColumn();

            useObject.HeaderText = "Включено до Акту";
            useObject.FalseValue = 0;
            useObject.TrueValue = 1;

            gridObjects.Columns.Add(useObject);

            AddTextColumn("Адреса Об'єкту");
            AddTextColumn("Назва Об'єкту");
            AddTextColumn("Площа Об'єкту");
            AddTextColumn("Вид Об'єкту");
            AddTextColumn("Тип Об'єкту");
            AddTextColumn("Група Призначення");
            AddTextColumn("Призначення");
        }

        private void AddTextColumn(string caption)
        {
            gridObjects.Columns.Add(caption, caption);
            gridObjects.Columns[gridObjects.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridObjects.Columns[gridObjects.Columns.Count - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void FillGrid()
        {
            gridObjects.Rows.Clear();

            // Add a row for each object
            object[] rowData = new object[gridObjects.Columns.Count];

            for (int i = 0; i < act.actObjects.Count; i++)
            {
                GetRowValuesForObject(act.actObjects[i], rowData);

                gridObjects.Rows.Add(rowData);
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
            }
        }

        private void GetRowValuesForObject(ActObject obj, object[] rowData)
        {
            for (int j = 0; j < rowData.Length; j++)
            {
                rowData[j] = null;
            }

            // Included ?
            rowData[0] = obj.includedInAct ? 1 : 0;

            // Address
            rowData[1] = obj.ToString();

            // Object name
            rowData[2] = obj.objectName_NJF;

            // Square
            if (obj.objectSquare_NJF is decimal)
            {
                rowData[3] = ((decimal)obj.objectSquare_NJF).ToString("F2");
            }

            // Object Kind
            rowData[4] = DB.FindNameInDictionaryNJF(DB.DICT_OBJ_KIND, obj.objectKindIdNJF);

            // Object Type
            rowData[5] = DB.FindNameInDictionaryNJF(DB.DICT_OBJ_TYPE, obj.objectTypeIdNJF);

            // Purpose Group
            rowData[6] = DB.FindNameInDictionaryNJF(DB.DICT_PURPOSE_GROUP, obj.purposeGroupIdNJF);

            // Purpose
            rowData[7] = DB.FindNameInDictionaryNJF(DB.DICT_PURPOSE, obj.purposeIdNJF);
        }

        private void gridObjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < gridObjects.Rows.Count)
            {
                if (e.ColumnIndex == 0)
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

                        act.actObjects[e.RowIndex].includedInAct = (cbCell.Value == cbCell.TrueValue);
                    }
                }
            }
        }

        private void btnCreateAct_Click(object sender, EventArgs e)
        {
            if (rishennya == null)
            {
                System.Windows.Forms.MessageBox.Show("Для створення Акту необхідно спочатку вибрати Рішення або Розпорядження, до якого відноситься Акт.",
                    "Перевірка Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // Dump all selected objects to Act
            ImportedAct newAct = new ImportedAct();

            for (int nRow = 0; nRow < gridObjects.Rows.Count; nRow++)
            {
                object cell = gridObjects.Rows[nRow].Cells[0];

                if (cell is DataGridViewCheckBoxCell)
                {
                    DataGridViewCheckBoxCell cbCell = cell as DataGridViewCheckBoxCell;

                    if (cbCell.Value.Equals(cbCell.TrueValue))
                    {
                        ActObject newObj = new ActObject();

                        act.actObjects[nRow].CopyTo(newObj);

                        newAct.actObjects.Add(newObj);
                    }
                }
            }

            if (newAct.actObjects.Count == 0)
            {
                if (System.Windows.Forms.MessageBox.Show("Не вибрано жодного об'єкту для Акту. Створити порожній Акт?",
                    "Створення Акту",
                    System.Windows.Forms.MessageBoxButtons.OKCancel,
                    System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
            }

            // Test connection to 1NF
            FbConnection connection1NF = null;

            if (newAct.actObjects.Count > 0)
            {
                connection1NF = new FbConnection(MainForm.preferences.Get1NFConnectionString());

                try
                {
                    connection1NF.Open();
                    connection1NF.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            newAct.masterDocDate = editRishDate.Value;
            newAct.masterDocNum = editRishNumber.Text;

            using (ActForm actForm = new ActForm())
            {
                actForm.act = newAct;
                actForm.rishennya = rishennya;
                actForm.RishNumber = editRishNumber.Text;
                actForm.RishDate = editRishDate.Value;

                if (newAct.actObjects.Count > 0)
                {
                    // Perform matching of organizations and objects to 1NF
                    Cursor.Current = Cursors.WaitCursor;

                    connection1NF.Open();

                    ObjectFinder objectFinder1NF = ObjectFinder.Instance;
                    objectFinder1NF.BuildObjectCacheFrom1NF(connection1NF);

                    OrganizationFinder orgFinder1NF = new OrganizationFinder();
                    orgFinder1NF.BuildOrganizationCacheFrom1NF(connection1NF, false);

                    actForm.MatchActObjectsTo1NF(objectFinder1NF, orgFinder1NF);

                    connection1NF.Close();
                }

                actForm.FillGrid();

                Cursor.Current = Cursors.Default;

                actForm.ShowDialog();
            }
        }
    }
}
