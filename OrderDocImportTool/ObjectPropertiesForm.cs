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
    public partial class ObjectPropertiesForm : Form
    {
        private AppendixObject appendixObject = null;

        private List<Transfer> transfers = new List<Transfer>();

        private Object1NF object1NF = null;

        public ObjectPropertiesForm()
        {
            InitializeComponent();
        }

        public void SetAppendixObject(AppendixObject appendixObj)
        {
            appendixObject = appendixObj;

            // Copy all the transfers to the temporary list, to support the Cancel button
            foreach (Transfer t in appendixObj.transfers)
            {
                transfers.Add(t.MakeCopy());
            }

            // Copy association to the 1NF object, to support the Cancel button
            object1NF = appendixObj.object1NF;

            FillComboBoxValues();
            FillControls();
            FillListOfTransfers();
        }

        private void FillComboBoxValues()
        {
            DB.FillComboBoxFromDictionaryNJF(comboObjectKind, DB.DICT_OBJ_KIND);
            DB.FillComboBoxFromDictionaryNJF(comboObjectType, DB.DICT_OBJ_TYPE);
            DB.FillComboBoxFromDictionaryNJF(comboPurposeGroup, DB.DICT_PURPOSE_GROUP);
            DB.FillComboBoxFromDictionaryNJF(comboPurpose, DB.DICT_PURPOSE);
        }

        private void FillControls()
        {
            SelectComboValueByCategory(comboObjectKind, ColumnCategory.ObjectKind);
            SelectComboValueByCategory(comboObjectType, ColumnCategory.ObjectType);
            SelectComboValueByCategory(comboPurposeGroup, ColumnCategory.PurposeGroup);

            // The list of items for comboPurpose control depends on the selected value in the comboPurposeGroup control
            comboPurposeGroup_SelectedIndexChanged(this, new EventArgs());

            SelectComboValueByCategory(comboPurpose, ColumnCategory.Purpose);

            SetEditTextByCategory(editAddress, ColumnCategory.Address);
            SetEditTextByCategory(editObjectName, ColumnCategory.ObjectName);
            SetEditTextByCategory(editBalansCost, ColumnCategory.BalansCost);
            SetEditTextByCategory(editFinalCost, ColumnCategory.FinalCost);
            SetEditTextByCategory(editObjLength, ColumnCategory.Length);
            SetEditTextByCategory(editObjSquare, ColumnCategory.Square);
            SetEditTextByCategory(editObjDiameter, ColumnCategory.Diameter);
            SetEditTextByCategory(editBuildYear, ColumnCategory.BuildYear);
            SetEditTextByCategory(editExplYear, ColumnCategory.ExplYear);
            SetEditTextByCategory(editCharacteristics, ColumnCategory.Characteristics);

            if (appendixObject.object1NF != null)
            {
                editObject1NF.Text = appendixObject.object1NF.ToString();
            }
            else
            {
                editObject1NF.Text = "";
            }
        }

        private void FillListOfTransfers()
        {
            listTransfers.Items.Clear();

            foreach (Transfer t in transfers)
            {
                ListViewItem lviOrgFrom = new ListViewItem();
                ListViewItem.ListViewSubItem lvsiOrgTo = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem lvsiRight = new ListViewItem.ListViewSubItem();

                lviOrgFrom.Text = t.orgNameFrom.Trim();
                lvsiOrgTo.Text = t.orgNameTo.Trim();
                lvsiRight.Text = DB.FindNameInDictionaryNJF(DB.DICT_RIGHTS, t.rightId);

                lviOrgFrom.SubItems.Add(lvsiOrgTo);
                lviOrgFrom.SubItems.Add(lvsiRight);

                listTransfers.Items.Add(lviOrgFrom);
            }
        }

        private void SetEditTextByCategory(TextBox edit, ColumnCategory category)
        {
            string value = "";

            if (appendixObject.properties.TryGetValue(category, out value))
            {
                edit.Text = value;
            }
            else
            {
                edit.Text = "";
            }
        }

        private void SelectComboValueByCategory(ComboBox combo, ColumnCategory category)
        {
            string value = "";

            if (appendixObject.properties.TryGetValue(category, out value))
            {
                value = value.ToUpper().Trim();

                for (int i = 0; i < combo.Items.Count; i++)
                {
                    DictionaryValue dv = combo.Items[i] as DictionaryValue;

                    if (dv.value == value)
                    {
                        combo.SelectedIndex = i;
                        return;
                    }
                }
            }

            // Value is absent, or not found in the dictionary
            combo.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            appendixObject.properties[ColumnCategory.Address] = editAddress.Text;
            appendixObject.properties[ColumnCategory.ObjectName] = editObjectName.Text;
            appendixObject.properties[ColumnCategory.BalansCost] = editBalansCost.Text;
            appendixObject.properties[ColumnCategory.FinalCost] = editFinalCost.Text;
            appendixObject.properties[ColumnCategory.Length] = editObjLength.Text;
            appendixObject.properties[ColumnCategory.Square] = editObjSquare.Text;
            appendixObject.properties[ColumnCategory.Diameter] = editObjDiameter.Text;
            appendixObject.properties[ColumnCategory.BuildYear] = editBuildYear.Text;
            appendixObject.properties[ColumnCategory.ExplYear] = editExplYear.Text;
            appendixObject.properties[ColumnCategory.Characteristics] = editCharacteristics.Text;

            appendixObject.properties[ColumnCategory.ObjectKind] = comboObjectKind.Text;
            appendixObject.properties[ColumnCategory.ObjectType] = comboObjectType.Text;
            appendixObject.properties[ColumnCategory.PurposeGroup] = comboPurposeGroup.Text;
            appendixObject.properties[ColumnCategory.Purpose] = comboPurpose.Text;

            // Save the list of transfers
            appendixObject.transfers.Clear();

            foreach (Transfer t in transfers)
            {
                appendixObject.transfers.Add(t);
            }

            // Save the associated 1NF object
            appendixObject.object1NF = object1NF;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboPurposeGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Fill the drop-down of object purposes
            comboPurpose.Items.Clear();

            if (comboPurposeGroup.SelectedItem is DictionaryValue)
            {
                DB.FillComboBoxFromDictionaryNJF_Hierarchical(comboPurpose, DB.DICT_PURPOSE, (comboPurposeGroup.SelectedItem as DictionaryValue).key);
            }
        }

        private void btnAddTransfer_Click(object sender, EventArgs e)
        {
            Transfer t = new Transfer();

            using (AddObjTransferForm form = new AddObjTransferForm())
            {
                form.SetTransfer(t);

                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    transfers.Add(t);

                    FillListOfTransfers();
                }
            }
        }

        private void btnDelTransfer_Click(object sender, EventArgs e)
        {
            if (listTransfers.SelectedIndices.Count > 0)
            {
                int row = listTransfers.SelectedIndices[0];

                transfers.RemoveAt(row);

                FillListOfTransfers();
            }
        }

        private void btnPickObject_Click(object sender, EventArgs e)
        {
            using (ObjectPickerForm picker = new ObjectPickerForm())
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (picker.selectedObject != null)
                    {
                        object1NF = picker.selectedObject;

                        editObject1NF.Text = object1NF.ToString();
                    }
                }
            }
        }

        private void btnNewObject_Click(object sender, EventArgs e)
        {
            using (CreateNewObjectForm form = new CreateNewObjectForm())
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (form.newObjectId > 0)
                    {
                        Object1NF obj = null;

                        if (DB.objects1NF.TryGetValue(form.newObjectId, out obj))
                        {
                            object1NF = obj;

                            editObject1NF.Text = object1NF.ToString();
                        }
                    }
                }
            }
        }
    }
}
