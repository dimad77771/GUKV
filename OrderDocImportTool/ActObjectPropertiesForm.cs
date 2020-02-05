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
    public partial class ActObjectPropertiesForm : Form
    {
        public ActObject actObject = null;

        public ActObject actObjectWorkingCopy = null;

        private bool isFillingTransferProperties = false;

        public ActObjectPropertiesForm()
        {
            InitializeComponent();
        }

        public void SetActObject(ActObject actObj)
        {
            actObject = actObj;

            actObjectWorkingCopy = new ActObject();
            actObject.CopyTo(actObjectWorkingCopy);

            UpdateAllControls();
        }

        private void UpdateAllControls()
        {
            editAddressNJF.Text = actObjectWorkingCopy.ToString();

            if (actObjectWorkingCopy.object1NF != null)
                editAddress1NF.Text = actObjectWorkingCopy.object1NF.ToString();
            else
                editAddress1NF.Text = "";

            editObjectName.Text = actObjectWorkingCopy.objectName_NJF;

            if (actObjectWorkingCopy.objectSquare_NJF is decimal)
                editObjectSquare.Text = ((decimal)actObjectWorkingCopy.objectSquare_NJF).ToString("F2");
            else
                editObjectSquare.Text = "";

            editTransferSqr.Text = "";
            editTransferOrgFromNJF.Text = "";
            editTransferOrgFrom1NF.Text = "";
            editTransferOrgToNJF.Text = "";
            editTransferOrgTo1NF.Text = "";
            editBalansObject1NF.Text = "";

            FillGridOfTransfers(true);
        }

        private void FillGridOfTransfers(bool keepSelection)
        {
            // Save the selected index, if it exists
            int selectedIndex = -1;

            if (listBalansTransfers.SelectedIndices.Count > 0)
            {
                selectedIndex = listBalansTransfers.SelectedIndices[0];
            }

            listBalansTransfers.Items.Clear();

            foreach (BalansTransfer bt in actObjectWorkingCopy.balansTransfers)
            {
                BalansObject1NF balansObj = bt.balansObject1NF;

                string transferType = Properties.AppResources.TransferTypeTransfer;

                if (bt.transferType == ObjectTransferType.Create)
                    transferType = Properties.AppResources.TransferTypeCreate;

                if (bt.transferType == ObjectTransferType.Destroy)
                    transferType = Properties.AppResources.TransferTypeDestroy;

                string sqr = bt.sqr > 0m ? bt.sqr.ToString("F2") : (bt.transferType != ObjectTransferType.Destroy ? "?????" : "");
                string from = bt.organizationFromId_1NF > 0 ? bt.orgFromFullName_1NF : (bt.transferType != ObjectTransferType.Create ? "?????" : "");
                string to = bt.organizationToId_1NF > 0 ? bt.orgToFullName_1NF : (bt.transferType != ObjectTransferType.Destroy ? "?????" : "");
                string balans = balansObj != null ? balansObj.ToString() : (bt.transferType != ObjectTransferType.Create ? "?????" : "");

                if (bt.transferType == ObjectTransferType.Create)
                {
                    from = "";
                    balans = "";
                }

                if (bt.transferType == ObjectTransferType.Destroy)
                {
                    to = "";
                }

                ListViewItem item = new ListViewItem("");
                item.SubItems.Add(transferType);
                item.SubItems.Add(sqr);
                item.SubItems.Add(from);
                item.SubItems.Add(to);
                item.SubItems.Add(balans);

                item.IndentCount = 0;
                item.ImageIndex = bt.IsFullyDefined() ? 0 : 1;
                item.Tag = bt;

                listBalansTransfers.Items.Add(item);
            }

            // Restore the selected index, if possible
            if (keepSelection && selectedIndex >= 0 && selectedIndex < listBalansTransfers.Items.Count)
            {
                listBalansTransfers.SelectedIndices.Add(selectedIndex);
            }
            else
            {
                FillBalansTransferProperties(null);
            }
        }

        private void FillBalansTransferProperties(BalansTransfer bt)
        {
            isFillingTransferProperties = true;

            editTransferSqr.Text = "";
            editTransferOrgFromNJF.Text = "";
            editTransferOrgFrom1NF.Text = "";
            editTransferOrgToNJF.Text = "";
            editTransferOrgTo1NF.Text = "";
            editBalansObject1NF.Text = "";

            radioTransfer.Checked = false;
            radioCreate.Checked = false;
            radioDestroy.Checked = false;

            if (bt != null)
            {
                if (bt.sqr > 0m)
                    editTransferSqr.Text = bt.sqr.ToString("F2");

                string zkpoFromNJF = bt.orgFromZkpo_NJF.Length > 0 ? bt.orgFromZkpo_NJF : "????????";
                string zkpoFrom1NF = bt.orgFromZkpo_1NF.Length > 0 ? bt.orgFromZkpo_1NF : "????????";
                string zkpoToNJF = bt.orgToZkpo_NJF.Length > 0 ? bt.orgToZkpo_NJF : "????????";
                string zkpoTo1NF = bt.orgToZkpo_1NF.Length > 0 ? bt.orgToZkpo_1NF : "????????";

                editTransferOrgFromNJF.Text = bt.orgFromFullName_NJF.Length > 0 ? zkpoFromNJF + " - " + bt.orgFromFullName_NJF : "";
                editTransferOrgFrom1NF.Text = bt.orgFromFullName_1NF.Length > 0 ? zkpoFrom1NF + " - " + bt.orgFromFullName_1NF : "";
                editTransferOrgToNJF.Text = bt.orgToFullName_NJF.Length > 0 ? zkpoToNJF + " - " + bt.orgToFullName_NJF : "";
                editTransferOrgTo1NF.Text = bt.orgToFullName_1NF.Length > 0 ? zkpoTo1NF + " - " + bt.orgToFullName_1NF : "";

                BalansObject1NF balansObj = bt.balansObject1NF;

                if (balansObj != null)
                    editBalansObject1NF.Text = balansObj.ToString();

                switch (bt.transferType)
                {
                    case ObjectTransferType.Transfer:
                        radioTransfer.Checked = true;
                        break;

                    case ObjectTransferType.Create:
                        radioCreate.Checked = true;
                        editTransferOrgFromNJF.Text = Properties.AppResources.Inaplicable;
                        editTransferOrgFrom1NF.Text = Properties.AppResources.Inaplicable;
                        editBalansObject1NF.Text = Properties.AppResources.Inaplicable;
                        break;

                    case ObjectTransferType.Destroy:
                        radioDestroy.Checked = true;
                        editTransferOrgToNJF.Text = Properties.AppResources.Inaplicable;
                        editTransferOrgTo1NF.Text = Properties.AppResources.Inaplicable;
                        break;
                }
            }

            ShowRequiredLabels();

            isFillingTransferProperties = false;
        }

        private void ShowRequiredLabels()
        {
            labelRequiredSqr.Visible = radioTransfer.Checked || radioCreate.Checked;
            labelRequiredFrom.Visible = radioTransfer.Checked || radioDestroy.Checked;
            labelRequiredTo.Visible = radioTransfer.Checked || radioCreate.Checked;
            labelRequiredBalansObj.Visible = radioTransfer.Checked || radioDestroy.Checked;
        }

        private void btnPickObject_Click(object sender, EventArgs e)
        {
            using (ObjectPickerForm picker = new ObjectPickerForm())
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (picker.selectedObject != null)
                    {
                        actObjectWorkingCopy.object1NF = picker.selectedObject;

                        editAddress1NF.Text = picker.selectedObject.ToString();

                        // Update all balans transfers as well
                        foreach (BalansTransfer bt in actObjectWorkingCopy.balansTransfers)
                        {
                            bt.objectId_1NF = picker.selectedObject.objectId;
                        }
                    }
                }
            }
        }

        private void listBalansTransfers_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enableButtons = false;

            if (listBalansTransfers.SelectedItems.Count > 0 && listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
            {
                FillBalansTransferProperties(listBalansTransfers.SelectedItems[0].Tag as BalansTransfer);

                enableButtons = true;
            }
            else
            {
                FillBalansTransferProperties(null);
            }

            btnPickBalansObject.Enabled = enableButtons;
            btnPickOrgFrom.Enabled = enableButtons;
            btnPickOrgTo.Enabled = enableButtons;
            btnAcceptSquare.Enabled = enableButtons;

            btnNewOrgFrom.Enabled = enableButtons;
            btnNewOrgTo.Enabled = enableButtons;

            radioTransfer.Enabled = enableButtons;
            radioCreate.Enabled = enableButtons;
            radioDestroy.Enabled = enableButtons;

            editTransferSqr.Enabled = enableButtons;
        }

        private void btnPickOrgFrom_Click(object sender, EventArgs e)
        {
            if (listBalansTransfers.SelectedItems.Count > 0 && listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
            {
                BalansTransfer bt = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;

                using (OrganizationPickerForm picker = new OrganizationPickerForm(""))
                {
                    if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (picker.selectedOrganization != null)
                        {
                            bt.orgFrom1NF = picker.selectedOrganization;

                            FillGridOfTransfers(true);
                            FillBalansTransferProperties(bt);
                        }
                    }
                }
            }
        }

        private void btnPickOrgTo_Click(object sender, EventArgs e)
        {
            if (listBalansTransfers.SelectedItems.Count > 0 && listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
            {
                BalansTransfer bt = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;

                using (OrganizationPickerForm picker = new OrganizationPickerForm(""))
                {
                    if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (picker.selectedOrganization != null)
                        {
                            bt.orgTo1NF = picker.selectedOrganization;

                            FillGridOfTransfers(true);
                            FillBalansTransferProperties(bt);
                        }
                    }
                }
            }
        }

        private void btnPickBalansObject_Click(object sender, EventArgs e)
        {
            if (listBalansTransfers.SelectedItems.Count > 0 && listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
            {
                BalansTransfer bt = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;

                using (BalansObjectPickerForm picker = new BalansObjectPickerForm())
                {
                    if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (picker.selectedBalansId > 0)
                        {
                            BalansObject1NF bal = null;

                            if (DB.balans1NFByID.TryGetValue(picker.selectedBalansId, out bal))
                            {
                                bt.balansObject1NF = bal;
                            }

                            FillGridOfTransfers(true);
                            FillBalansTransferProperties(bt);
                        }
                    }
                }
            }
        }

        private void radioTransfer_CheckedChanged(object sender, EventArgs e)
        {
            if (!isFillingTransferProperties &&
                listBalansTransfers.SelectedItems.Count > 0 &&
                listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
            {
                BalansTransfer bt = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;

                bt.transferType = ObjectTransferType.Transfer;

                FillGridOfTransfers(true);
                FillBalansTransferProperties(bt);
            }
        }

        private void radioCreate_CheckedChanged(object sender, EventArgs e)
        {
            if (!isFillingTransferProperties &&
                listBalansTransfers.SelectedItems.Count > 0 &&
                listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
            {
                BalansTransfer bt = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;

                bt.transferType = ObjectTransferType.Create;

                FillGridOfTransfers(true);
                FillBalansTransferProperties(bt);
            }
        }

        private void radioDestroy_CheckedChanged(object sender, EventArgs e)
        {
            if (!isFillingTransferProperties &&
                listBalansTransfers.SelectedItems.Count > 0 &&
                listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
            {
                BalansTransfer bt = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;

                bt.transferType = ObjectTransferType.Destroy;

                FillGridOfTransfers(true);
                FillBalansTransferProperties(bt);
            }
        }

        private void btnAddTransfer_Click(object sender, EventArgs e)
        {
            BalansTransfer bt = new BalansTransfer();

            bt.objectId_1NF = actObjectWorkingCopy.objectId_1NF;
            bt.objectId_NJF = actObjectWorkingCopy.objectId_NJF;

            actObjectWorkingCopy.balansTransfers.Add(bt);

            FillGridOfTransfers(false);
        }

        private void btnDelTransfer_Click(object sender, EventArgs e)
        {
            if (listBalansTransfers.SelectedItems.Count > 0)
            {
                actObjectWorkingCopy.balansTransfers.RemoveAt(listBalansTransfers.SelectedIndices[0]);

                listBalansTransfers.Items.RemoveAt(listBalansTransfers.SelectedIndices[0]);
            }
            else
            {
                MessageBox.Show(
                    "Будь ласка, виберіть передачу права зі списку.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnAcceptSquare_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBalansTransfers.SelectedItems.Count > 0 && listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
                {
                    BalansTransfer bt = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;

                    string text = editTransferSqr.Text.Trim();

                    bt.sqr = DB.ConvertStrToDecimal(ref text);

                    FillGridOfTransfers(true);
                    FillBalansTransferProperties(bt);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Будь ласка, введіть числове значення в поле 'Площа'.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            actObjectWorkingCopy.CopyTo(actObject);

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnApplyTotalSquare_Click(object sender, EventArgs e)
        {
            try
            {
                string text = editObjectSquare.Text.Trim();
                decimal sqr = DB.ConvertStrToDecimal(ref text);

                actObjectWorkingCopy.objectSquare_NJF = sqr;

                foreach (BalansTransfer bt in actObjectWorkingCopy.balansTransfers)
                {
                    bt.sqr = sqr;
                }

                BalansTransfer btSelected = null;

                if (listBalansTransfers.SelectedItems.Count > 0 && listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
                {
                    btSelected = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;
                }

                FillGridOfTransfers(true);
                FillBalansTransferProperties(btSelected);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Будь ласка, введіть числове значення в поле 'Площа'.",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
                            actObjectWorkingCopy.object1NF = obj;

                            editAddress1NF.Text = obj.ToString();

                            // Update all balans transfers as well
                            foreach (BalansTransfer bt in actObjectWorkingCopy.balansTransfers)
                            {
                                bt.objectId_1NF = obj.objectId;
                            }
                        }
                    }
                }
            }
        }

        private void btnNewOrgFrom_Click(object sender, EventArgs e)
        {
            if (listBalansTransfers.SelectedItems.Count > 0 && listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
            {
                BalansTransfer btSelected = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;

                using (CreateNewOrgForm form = new CreateNewOrgForm())
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (form.newOrganizationId > 0)
                        {
                            Organization1NF org = null;

                            if (DB.organizations1NF.TryGetValue(form.newOrganizationId, out org))
                            {
                                btSelected.orgFrom1NF = org;

                                FillGridOfTransfers(true);
                                FillBalansTransferProperties(btSelected);
                            }
                        }
                    }
                }
            }
        }

        private void btnNewOrgTo_Click(object sender, EventArgs e)
        {
            if (listBalansTransfers.SelectedItems.Count > 0 && listBalansTransfers.SelectedItems[0].Tag is BalansTransfer)
            {
                BalansTransfer btSelected = listBalansTransfers.SelectedItems[0].Tag as BalansTransfer;

                using (CreateNewOrgForm form = new CreateNewOrgForm())
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (form.newOrganizationId > 0)
                        {
                            Organization1NF org = null;

                            if (DB.organizations1NF.TryGetValue(form.newOrganizationId, out org))
                            {
                                btSelected.orgTo1NF = org;

                                FillGridOfTransfers(true);
                                FillBalansTransferProperties(btSelected);
                            }
                        }
                    }
                }
            }
        }
    }
}
