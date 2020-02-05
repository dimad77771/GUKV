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
    public partial class AddActObjectForm : Form
    {
        private Object1NF object1NF = null;

        public ActObject actObject = null;

        public AddActObjectForm()
        {
            InitializeComponent();

            DB.FillComboBoxFromDictionaryNJF(comboObjType, DB.DICT_OBJ_TYPE);
            DB.FillComboBoxFromDictionaryNJF(comboObjKind, DB.DICT_OBJ_KIND);
            DB.FillComboBoxFromDictionaryNJF(comboPurposeGroup, DB.DICT_PURPOSE_GROUP);
            DB.FillComboBoxFromDictionaryNJF(comboPurpose, DB.DICT_PURPOSE);
            DB.FillComboBoxFromDictionaryNJF(comboTechState, DB.DICT_TECH_STATE);
        }

        private void btnPickAddr_Click(object sender, EventArgs e)
        {
            using (ObjectPickerForm picker = new ObjectPickerForm())
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (picker.selectedObject != null)
                    {
                        object1NF = picker.selectedObject;

                        editAddress.Text = picker.selectedObject.ToString();
                    }
                }
            }
        }

        private void btnCreateAddr_Click(object sender, EventArgs e)
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

                            editAddress.Text = obj.ToString();
                        }
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ActObject actObj = new ActObject();

            // Address validation
            if (object1NF == null)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть адресу об'єкту.",
                    "Перевірка Даних",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (editName.Text.Trim().Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть назву об'єкту.",
                    "Перевірка Даних",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (editSquare.Text.Trim().Length == 0 && editLength.Text.Trim().Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть площу або довжину об'єкту.",
                    "Перевірка Даних",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // Drop-down values
            if (comboObjType.SelectedItem is DictionaryValue)
            {
                actObj.objectTypeIdNJF = (comboObjType.SelectedItem as DictionaryValue).key;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, вкажіть тип об'єкту.",
                    "Перевірка Даних",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (comboObjKind.SelectedItem is DictionaryValue)
            {
                actObj.objectKindIdNJF = (comboObjKind.SelectedItem as DictionaryValue).key;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, вкажіть вид об'єкту.",
                    "Перевірка Даних",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (comboPurposeGroup.SelectedItem is DictionaryValue)
            {
                actObj.purposeGroupIdNJF = (comboPurposeGroup.SelectedItem as DictionaryValue).key;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, вкажіть групу призначення об'єкту.",
                    "Перевірка Даних",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (comboPurpose.SelectedItem is DictionaryValue)
            {
                actObj.purposeIdNJF = (comboPurpose.SelectedItem as DictionaryValue).key;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, вкажіть призначення об'єкту.",
                    "Перевірка Даних",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (comboTechState.SelectedItem is DictionaryValue)
            {
                actObj.techStateIdNJF = (comboTechState.SelectedItem as DictionaryValue).key;
            }

            // Build Year
            if (editBuildYear.Text.Length > 0)
            {
                try
                {
                    actObj.yearBuildNJF = int.Parse(editBuildYear.Text);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Рік побудови'.",
                        "Перевірка Даних",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // Exploit Year
            if (editExplYear.Text.Length > 0)
            {
                try
                {
                    actObj.yearExplNJF = int.Parse(editExplYear.Text);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Рік введення в експлуатацію'.",
                        "Перевірка Даних",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // Floor
            if (editFloor.Text.Length > 0)
            {
                try
                {
                    actObj.objectFloorsInt_NJF = int.Parse(editFloor.Text);
                    actObj.objectFloorsStr_NJF = actObj.objectFloorsInt_NJF.ToString();
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Поверх'.",
                        "Перевірка Даних",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // Square
            if (editSquare.Text.Length > 0)
            {
                try
                {
                    string sqr = editSquare.Text;
                    actObj.objectSquare_NJF = DB.ConvertStrToDecimal(ref sqr);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Площа'.",
                        "Перевірка Даних",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // Length
            if (editLength.Text.Length > 0)
            {
                try
                {
                    string len = editLength.Text;
                    actObj.objectLen_NJF = DB.ConvertStrToDecimal(ref len);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Довжина'.",
                        "Перевірка Даних",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // Balans cost
            if (editBalansCost.Text.Length > 0)
            {
                try
                {
                    string balansCost = editBalansCost.Text;
                    actObj.objectBalansCost_NJF = DB.ConvertStrToDecimal(ref balansCost);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Балансова вартість'.",
                        "Перевірка Даних",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // Final cost
            if (editFinalCost.Text.Length > 0)
            {
                try
                {
                    string finalCost = editFinalCost.Text;
                    actObj.objectFinalCost_NJF = DB.ConvertStrToDecimal(ref finalCost);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Будь ласка, введіть числове значення в поле 'Залишкова вартість'.",
                        "Перевірка Даних",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }
            }

            // Text properties
            actObj.objectName_NJF = editName.Text.Trim().ToUpper();
            actObj.characteristicNJF = editCharacteristic.Text.Trim().ToUpper();

            // Address
            actObj.objectId_1NF = object1NF.objectId;

            // Test connection to NJF
            FbConnection connectionNJF = new FbConnection(MainForm.preferences.GetNJFConnectionString(DB.UserName, DB.UserPassword));

            try
            {
                connectionNJF.Open();
                connectionNJF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних 'Розпорядження': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            connectionNJF.Open();
            Cursor.Current = Cursors.WaitCursor;

            ObjectFinder objectFinder = ObjectFinder.Instance;
            objectFinder.BuildObjectCacheFromNJF(connectionNJF);

            FbTransaction transaction = connectionNJF.BeginTransaction();

            try
            {
                actObj.objectId_NJF = DB.FindObjectMatchInNJF(connectionNJF, transaction, object1NF, objectFinder);

                transaction.Commit();

                actObj.addrStreet_NJF = object1NF.streetName;
                actObj.addrNomer1_NJF = object1NF.addrNomer1;
                actObj.addrNomer2_NJF = object1NF.addrNomer2;
                actObj.addrNomer3_NJF = object1NF.addrNomer3;
                actObj.addrMisc_NJF = object1NF.addrMisc;
                actObj.districtId_NJF = (object1NF.districtId is int) ? (object)DB.MatchDictionary1NFtoNJF(DB.DICT_DISTRICTS, (int)object1NF.districtId) : null;
            }
            catch (Exception)
            {
                transaction.Rollback();
            }

            Cursor.Current = Cursors.Default;
            connectionNJF.Close();

            // All verifications performed; return OK
            actObject = actObj;
            DialogResult = System.Windows.Forms.DialogResult.OK;
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
    }
}
