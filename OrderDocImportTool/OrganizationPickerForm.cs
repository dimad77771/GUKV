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
    public partial class OrganizationPickerForm : Form
    {
        private List<Organization1NF> foundOrganizations = new List<Organization1NF>();

        public Organization1NF selectedOrganization = null;

        public OrganizationPickerForm(string namePattern)
        {
            InitializeComponent();

            if (namePattern.Length > 0)
            {
                editOrgName.Text = namePattern;

                btnFindOrganization_Click(this, new EventArgs());
            }
        }

        private void btnFindOrganization_Click(object sender, EventArgs e)
        {
            string zkpo = editOrgZkpo.Text.Trim();
            string name = editOrgName.Text.Trim().ToUpper();

            foundOrganizations.Clear();

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
                    foundOrganizations.Add(pair.Value);
                }
            }

            // Fill the List View
            listOrganizations.Items.Clear();

            foreach (Organization1NF org in foundOrganizations)
            {
                ListViewItem lviOrgZkpo = new ListViewItem();
                ListViewItem.ListViewSubItem lvsiOrgName = new ListViewItem.ListViewSubItem();

                lviOrgZkpo.Text = org.zkpo;
                lvsiOrgName.Text = org.fullName;

                lviOrgZkpo.SubItems.Add(lvsiOrgName);

                listOrganizations.Items.Add(lviOrgZkpo);
            }
        }

        private void btnSelectOrganization_Click(object sender, EventArgs e)
        {
            selectedOrganization = null;

            foreach (int index in listOrganizations.SelectedIndices)
            {
                if (foundOrganizations.Count > index)
                {
                    selectedOrganization = foundOrganizations[index];
                }
            }
        }

        private void editOrgZkpo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFindOrganization_Click(this, new EventArgs());
            }
        }

        private void editOrgName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFindOrganization_Click(this, new EventArgs());
            }
        }
    }
}
