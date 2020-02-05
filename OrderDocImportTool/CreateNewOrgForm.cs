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
    public partial class CreateNewOrgForm : Form
    {
        public int newOrganizationId = -1;

        public CreateNewOrgForm()
        {
            InitializeComponent();

            DB.FillComboBoxFromDictionary1NF(comboDistrict, DB.DICT_DISTRICTS);
            DB.FillComboBoxFromDictionary1NF(comboStreet, DB.DICT_STREETS);
            DB.FillComboBoxFromDictionary1NF(comboOrgType, DB.DICT_ORG_TYPE);
            DB.FillComboBoxFromDictionary1NF(comboIndustry, DB.DICT_ORG_INDUSTRY);
            DB.FillComboBoxFromDictionary1NF(comboOccupation, DB.DICT_ORG_OCCUPATION);
            DB.FillComboBoxFromDictionary1NF(comboFormGosp, DB.DICT_ORG_FORM_GOSP);
            DB.FillComboBoxFromDictionary1NF(comboFormVlasn, DB.DICT_ORG_OWNERSHIP);

            comboStatus.SelectedIndex = 0;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            string fullName = editFullName.Text.Trim().ToUpper();
            string shortName = editShortName.Text.Trim().ToUpper();
            string zkpo = editZkpo.Text.Trim().ToUpper();

            string nomer = editNomer1.Text.Trim();
            string korpus = editKorpus.Text.Trim();
            string addrMisc = editAddrMisc.Text.Trim();

            // Validate the required fields
            if (fullName.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть повну назву організації.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (shortName.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть скорочену назву організації.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (zkpo.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть код ЄДРПОУ організації.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (!(comboStreet.SelectedItem is DictionaryValue))
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть назву вулиці.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (nomer.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть номер будинку.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (!(comboOrgType.SelectedItem is DictionaryValue))
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть тип організації.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (!(comboFormGosp.SelectedItem is DictionaryValue))
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть форму господарювання організації.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (!(comboFormVlasn.SelectedItem is DictionaryValue))
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть форму власності організації.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (!(comboIndustry.SelectedItem is DictionaryValue))
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть галузь організації.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (!(comboOccupation.SelectedItem is DictionaryValue))
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть вид діяльності організації.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // Check if this ZKPO code is unique
            if (DB.FindOrganizationByZKPO(zkpo) > 0)
            {
                System.Windows.Forms.MessageBox.Show("Організація з таким кодом ЄДРПОУ вже існує в базі '1НФ'!",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (!(comboDistrict.SelectedItem is DictionaryValue))
            {
                if (System.Windows.Forms.MessageBox.Show("Не вибрано район. Ви дійсно хочете створити нову організацію, не вказуючи район?",
                        "Помилка",
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            newOrganizationId = DB.CreateNew1NFOrganization(MainForm.preferences,
                zkpo,
                fullName,
                shortName,
                comboDistrict.SelectedItem is DictionaryValue ? (int?)((comboDistrict.SelectedItem as DictionaryValue).key) : (int?)null,
                (comboStreet.SelectedItem as DictionaryValue).value,
                editNomer1.Text.Trim().ToUpper(),
                editKorpus.Text.Trim().ToUpper(),
                editAddrMisc.Text.Trim().ToUpper(),
                editZIP.Text.Trim().ToUpper(),
                (comboIndustry.SelectedItem as DictionaryValue).key,
                (comboOccupation.SelectedItem as DictionaryValue).key,
                (comboFormVlasn.SelectedItem as DictionaryValue).key,
                (comboFormGosp.SelectedItem as DictionaryValue).key,
                (comboOrgType.SelectedItem as DictionaryValue).key,
                comboStatus.SelectedIndex + 1,
                editDirectorName.Text.Trim().ToUpper(),
                editDirectorPhone.Text.Trim().ToUpper(),
                editBuhgalterName.Text.Trim().ToUpper(),
                editBuhgalterPhone.Text.Trim().ToUpper(),
                editFax.Text.Trim().ToUpper());

            if (newOrganizationId > 0)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void comboIndustry_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Fill the drop-down of occupations
            comboOccupation.Items.Clear();

            if (comboIndustry.SelectedItem is DictionaryValue)
            {
                DB.FillComboBoxFromDictionary1NF_Hierarchical(comboOccupation, DB.DICT_ORG_OCCUPATION, (comboIndustry.SelectedItem as DictionaryValue).key);
            }
        }
    }
}
