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
    public partial class BalansObjectPickerForm : Form
    {
        public int selectedBalansId = -1;

        public BalansObjectPickerForm()
        {
            InitializeComponent();

            // Fill the drop-down of streets
            DB.FillComboBoxFromDictionary1NF(comboStreet, DB.DICT_STREETS);
        }

        private void comboStreet_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBuilding.Items.Clear();

            if (comboStreet.SelectedItem is DictionaryValue)
            {
                int streetId = (comboStreet.SelectedItem as DictionaryValue).key;

                if (streetId > 0)
                {
                    // Find all objects that correspond to the selected street
                    foreach (KeyValuePair<int, Object1NF> pair in DB.objects1NF)
                    {
                        if (pair.Value.streetId == streetId)
                        {
                            comboBuilding.Items.Add(new DictionaryValue(pair.Value.objectId, pair.Value.FormatBuildingNumber()));
                        }
                    }
                }
            }
        }

        private void comboBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillListOfObjects();
        }

        private void comboOrganizations_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillListOfObjects();
        }

        private void btnFindOrg_Click(object sender, EventArgs e)
        {
            comboOrganizations.Items.Clear();

            string zkpo = editOrgZkpoPattern.Text.Trim();
            string name = editOrgNamePattern.Text.Trim().ToUpper();

            if (zkpo.Length > 0 || name.Length > 0)
            {
                foreach (KeyValuePair<int, Organization1NF> pair in DB.organizations1NF)
                {
                    if (zkpo.Length > 0)
                    {
                        if (!pair.Value.zkpo.Contains(zkpo))
                        {
                            continue;
                        }
                    }

                    if (name.Length > 0)
                    {
                        if (!pair.Value.fullName.Contains(name) && !pair.Value.shortName.Contains(name))
                        {
                            continue;
                        }
                    }

                    // Match found
                    comboOrganizations.Items.Add(new DictionaryValue(pair.Key, pair.Value.zkpo + " - " + pair.Value.fullName));
                }
            }

            FillListOfObjects();
        }

        private void FillListOfObjects()
        {
            listObjects.Items.Clear();

            if (comboBuilding.SelectedItem is DictionaryValue)
            {
                int objectId1NF = (comboBuilding.SelectedItem as DictionaryValue).key;
                int organizationId1NF = -1;

                if (comboOrganizations.SelectedItem is DictionaryValue)
                {
                    organizationId1NF = (comboOrganizations.SelectedItem as DictionaryValue).key;
                }

                Dictionary<int, BalansObject1NF> storage = null;

                if (DB.balans1NFByAddress.TryGetValue(objectId1NF, out storage))
                {
                    foreach (KeyValuePair<int, BalansObject1NF> pair in storage)
                    {
                        if (organizationId1NF < 0 || pair.Value.organizationId == organizationId1NF)
                        {
                            Organization1NF org = null;

                            if (DB.organizations1NF.TryGetValue(pair.Value.organizationId, out org))
                            {
                                ListViewItem item = new ListViewItem(pair.Value.sqr.ToString("F2"));
                                item.SubItems.Add(org.fullName);
                                item.SubItems.Add(DB.FindNameInDictionary1NF(DB.DICT_PURPOSE, pair.Value.purposeId));
                                item.SubItems.Add(pair.Value.purpose);

                                item.Tag = pair.Value.balansId;

                                listObjects.Items.Add(item);
                            }
                        }
                    }
                }
            }
        }

        private void btnSelectObject_Click(object sender, EventArgs e)
        {
            if (listObjects.SelectedItems.Count > 0)
            {
                selectedBalansId = (int)listObjects.SelectedItems[0].Tag;

                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show(
                    "Будь ласка, виберіть об'єкт зі списку.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}
