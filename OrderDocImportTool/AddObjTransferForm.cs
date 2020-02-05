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
    public partial class AddObjTransferForm : Form
    {
        public Transfer transfer = null;

        public AddObjTransferForm()
        {
            InitializeComponent();

            DB.FillComboBoxFromDictionaryNJF(comboRight, DB.DICT_RIGHTS);
        }

        public void SetTransfer(Transfer t)
        {
            transfer = t;

            // Fill the user interface
            editOrgFrom.Text = t.orgNameFrom;
            editOrgTo.Text = t.orgNameTo;

            comboRight.SelectedItem = DB.FindValueInDictionaryNJF(DB.DICT_RIGHTS, t.rightId);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Save properties of the transfer
            transfer.orgNameFrom = editOrgFrom.Text.Trim().ToUpper();
            transfer.orgNameTo = editOrgTo.Text.Trim().ToUpper();

            if (comboRight.SelectedItem is DictionaryValue)
            {
                transfer.rightId = (comboRight.SelectedItem as DictionaryValue).key;
            }
            else
            {
                transfer.rightId = -1;
            }
        }

        private void btnPickOrgFrom_Click(object sender, EventArgs e)
        {
            using (OrganizationPickerForm picker = new OrganizationPickerForm(editOrgFrom.Text))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (picker.selectedOrganization != null)
                    {
                        transfer.orgFrom = picker.selectedOrganization;
                        transfer.orgNameFrom = picker.selectedOrganization.fullName;

                        editOrgFrom.Text = picker.selectedOrganization.fullName;
                    }
                }
            }
        }

        private void btnPickOrgTo_Click(object sender, EventArgs e)
        {
            using (OrganizationPickerForm picker = new OrganizationPickerForm(editOrgTo.Text))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (picker.selectedOrganization != null)
                    {
                        transfer.orgTo = picker.selectedOrganization;
                        transfer.orgNameTo = picker.selectedOrganization.fullName;

                        editOrgTo.Text = picker.selectedOrganization.fullName;
                    }
                }
            }
        }

        private void btnCreateOrgFrom_Click(object sender, EventArgs e)
        {
            using (CreateNewOrgForm form = new CreateNewOrgForm())
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (form.newOrganizationId > 0)
                    {
                        Organization1NF org = null;

                        if (DB.organizations1NF.TryGetValue(form.newOrganizationId, out org))
                        {
                            transfer.orgFrom = org;
                            transfer.orgNameFrom = org.fullName;

                            editOrgFrom.Text = org.fullName;
                        }
                    }
                }
            }
        }

        private void btnCreateOrgTo_Click(object sender, EventArgs e)
        {
            using (CreateNewOrgForm form = new CreateNewOrgForm())
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (form.newOrganizationId > 0)
                    {
                        Organization1NF org = null;

                        if (DB.organizations1NF.TryGetValue(form.newOrganizationId, out org))
                        {
                            transfer.orgTo = org;
                            transfer.orgNameTo = org.fullName;

                            editOrgTo.Text = org.fullName;
                        }
                    }
                }
            }
        }
    }
}
