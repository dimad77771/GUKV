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
    public partial class ObjGroupPropertiesForm : Form
    {
        private List<AppendixObject> objects = new List<AppendixObject>();

        private List<Transfer> transfers = new List<Transfer>();

        private Object1NF object1NF = null;

        public ObjGroupPropertiesForm()
        {
            InitializeComponent();
        }

        public void SetObjects(List<AppendixObject> objList)
        {
            objects.Clear();

            foreach (AppendixObject obj in objList)
            {
                objects.Add(obj);
            }

            DB.FillComboBoxFromDictionaryNJF(comboObjectKind, DB.DICT_OBJ_KIND);
            DB.FillComboBoxFromDictionaryNJF(comboObjectType, DB.DICT_OBJ_TYPE);
            DB.FillComboBoxFromDictionaryNJF(comboPurposeGroup, DB.DICT_PURPOSE_GROUP);
            DB.FillComboBoxFromDictionaryNJF(comboPurpose, DB.DICT_PURPOSE);

            // Get the common values from all objects, if they exist
            if (objects.Count > 0)
            {
                string commonPurpose = null;
                string commonPurposeGroup = null;
                string commonObjType = null;
                string commonObjKind = null;
                string commonObjName = null;
                string commonCharacteristics = null;

                foreach (AppendixObject obj in objects)
                {
                    GetCommonValue(obj, ColumnCategory.Purpose, ref commonPurpose);
                    GetCommonValue(obj, ColumnCategory.PurposeGroup, ref commonPurposeGroup);
                    GetCommonValue(obj, ColumnCategory.ObjectType, ref commonObjType);
                    GetCommonValue(obj, ColumnCategory.ObjectKind, ref commonObjKind);
                    GetCommonValue(obj, ColumnCategory.ObjectName, ref commonObjName);
                    GetCommonValue(obj, ColumnCategory.Characteristics, ref commonCharacteristics);
                }

                SelectComboValue(comboObjectType, commonObjType);
                SelectComboValue(comboObjectKind, commonObjKind);
                SelectComboValue(comboPurposeGroup, commonPurposeGroup);

                // The list of items for comboPurpose control depends on the selected value in the comboPurposeGroup control
                comboPurposeGroup_SelectedIndexChanged(this, new EventArgs());

                SelectComboValue(comboPurpose, commonPurpose);

                if (commonObjName != null)
                    editObjectName.Text = commonObjName;

                if (commonCharacteristics != null)
                    editCharacteristics.Text = commonCharacteristics;

                // Get the common transfers
                foreach (Transfer t in objects[0].transfers)
                {
                    bool existsInAllObjects = true;

                    for (int k = 1; k < objects.Count; k++)
                    {
                        if (!objects[k].SimilarTransferExists(t))
                        {
                            existsInAllObjects = false;
                            break;
                        }
                    }

                    if (existsInAllObjects)
                    {
                        transfers.Add(t.MakeCopy());
                    }
                }

                FillListOfTransfers();
            }
        }

        private void SelectComboValue(ComboBox combo, string name)
        {
            if (name != null && name.Length > 0)
            {
                for (int i = 0; i < combo.Items.Count; i++)
                {
                    DictionaryValue dv = combo.Items[i] as DictionaryValue;

                    if (dv.value == name)
                    {
                        combo.SelectedIndex = i;
                        return;
                    }
                }
            }

            // Value is absent, or not found in the dictionary
            combo.SelectedIndex = -1;
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

        private void GetCommonValue(AppendixObject obj, ColumnCategory category, ref string commonValue)
        {
            string value = "";

            if (!obj.properties.TryGetValue(category, out value))
            {
                value = "";
            }

            if (commonValue == null)
            {
                commonValue = value;
            }
            else if (commonValue.Length > 0 && commonValue != value)
            {
                commonValue = "";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Apply properties to all the provided objects
            foreach (AppendixObject obj in objects)
            {
                if (comboPurpose.SelectedItem is DictionaryValue)
                {
                    obj.properties[ColumnCategory.Purpose] = (comboPurpose.SelectedItem as DictionaryValue).value;
                }

                if (comboPurposeGroup.SelectedItem is DictionaryValue)
                {
                    obj.properties[ColumnCategory.PurposeGroup] = (comboPurposeGroup.SelectedItem as DictionaryValue).value;
                }

                if (comboObjectType.SelectedItem is DictionaryValue)
                {
                    obj.properties[ColumnCategory.ObjectType] = (comboObjectType.SelectedItem as DictionaryValue).value;
                }

                if (comboObjectKind.SelectedItem is DictionaryValue)
                {
                    obj.properties[ColumnCategory.ObjectKind] = (comboObjectKind.SelectedItem as DictionaryValue).value;
                }

                string objectName = editObjectName.Text.Trim().ToUpper();

                if (objectName.Length > DB.MAX_OBJECT_NAME_LEN)
                    objectName = objectName.Substring(0, DB.MAX_OBJECT_NAME_LEN);

                if (objectName.Length > 0)
                {
                    obj.properties[ColumnCategory.ObjectName] = objectName;
                }

                string characteristics = editCharacteristics.Text.Trim().ToUpper();

                if (characteristics.Length > DB.MAX_CHARACTERISTICS_LEN)
                    characteristics = characteristics.Substring(0, DB.MAX_CHARACTERISTICS_LEN);

                if (characteristics.Length > 0)
                {
                    obj.properties[ColumnCategory.Characteristics] = characteristics;
                }

                // Copy all our transfers to each object
                obj.transfers.Clear();

                // Save the associated 1NF object
                if (this.object1NF != null)
                    obj.object1NF = this.object1NF;

                foreach (Transfer t in transfers)
                {
                    obj.transfers.Add(t.MakeCopy());
                }
            }
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
