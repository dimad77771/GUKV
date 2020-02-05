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
    public partial class CreateNewObjectForm : Form
    {
        public int newObjectId = -1;

        public CreateNewObjectForm()
        {
            InitializeComponent();

            DB.FillComboBoxFromDictionary1NF(comboStreet, DB.DICT_STREETS);
            DB.FillComboBoxFromDictionary1NF(comboDistrict, DB.DICT_DISTRICTS);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Perform some validations
            if (!(comboStreet.SelectedItem is DictionaryValue))
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть назву вулиці.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            if (!(comboDistrict.SelectedItem is DictionaryValue))
            {
                if (System.Windows.Forms.MessageBox.Show("Не вибрано район. Ви дійсно хочете створити новий об'єкт не вказуючи район?",
                        "Помилка",
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            string nomer1 = editNumber1.Text.Trim();
            string nomer2 = editNumber2.Text.Trim();
            string nomer3 = editNumber3.Text.Trim();
            string addrMisc = editAddrMisc.Text.Trim();

            if ((nomer1 + nomer2 + nomer3 + addrMisc).Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть номер будинку.",
                    "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // Korpus number must be prefixed
            string korpus = editNumber3.Text;

            if (korpus.Length > 0)
            {
                korpus = Properties.AppResources.KorpusPrefix + korpus;
            }

            // Get the street ID and district ID
            int streetId = (comboStreet.SelectedItem as DictionaryValue).key;
            string streetName = (comboStreet.SelectedItem as DictionaryValue).value;
            int districtId = -1;

            if (comboDistrict.SelectedItem is DictionaryValue)
            {
                districtId = (comboDistrict.SelectedItem as DictionaryValue).key;
            }

            newObjectId = DB.CreateNew1NFObject(MainForm.preferences, streetId, streetName, districtId,
                editNumber1.Text, editNumber2.Text, korpus, editAddrMisc.Text);

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
