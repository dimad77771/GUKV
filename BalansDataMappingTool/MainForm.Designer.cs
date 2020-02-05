namespace GUKV.BalansDataMappingTool
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageOrganizations = new System.Windows.Forms.TabPage();
            this.panelRight = new System.Windows.Forms.Panel();
            this.buttonDeassociate = new System.Windows.Forms.Button();
            this.buttonAssociate = new System.Windows.Forms.Button();
            this.buttonApplyFilter1NF = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFilter1NF = new System.Windows.Forms.TextBox();
            this.checkBoxUseOrgSmartFilter = new System.Windows.Forms.CheckBox();
            this.listBox1NFOrganizations = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSelectedOrg1NF = new System.Windows.Forms.TextBox();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.buttonApplyFilterBalans = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFilterBalans = new System.Windows.Forms.TextBox();
            this.checkBoxShowOnlyUnmapped = new System.Windows.Forms.CheckBox();
            this.listBoxBalansOrganizations = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSelectedOrgBalans = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSaveChanges = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageOrganizations.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageOrganizations);
            this.tabControlMain.Location = new System.Drawing.Point(12, 12);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(885, 430);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageOrganizations
            // 
            this.tabPageOrganizations.Controls.Add(this.panelRight);
            this.tabPageOrganizations.Controls.Add(this.panelLeft);
            this.tabPageOrganizations.Location = new System.Drawing.Point(4, 22);
            this.tabPageOrganizations.Name = "tabPageOrganizations";
            this.tabPageOrganizations.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOrganizations.Size = new System.Drawing.Size(877, 404);
            this.tabPageOrganizations.TabIndex = 1;
            this.tabPageOrganizations.Text = "Організації";
            this.tabPageOrganizations.UseVisualStyleBackColor = true;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.panelRight.Controls.Add(this.buttonDeassociate);
            this.panelRight.Controls.Add(this.buttonAssociate);
            this.panelRight.Controls.Add(this.buttonApplyFilter1NF);
            this.panelRight.Controls.Add(this.label4);
            this.panelRight.Controls.Add(this.textBoxFilter1NF);
            this.panelRight.Controls.Add(this.checkBoxUseOrgSmartFilter);
            this.panelRight.Controls.Add(this.listBox1NFOrganizations);
            this.panelRight.Controls.Add(this.label2);
            this.panelRight.Controls.Add(this.textBoxSelectedOrg1NF);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(444, 3);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(430, 398);
            this.panelRight.TabIndex = 1;
            // 
            // buttonDeassociate
            // 
            this.buttonDeassociate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeassociate.Image = global::GUKV.BalansDataMappingTool.Properties.Resources.delete_16x;
            this.buttonDeassociate.Location = new System.Drawing.Point(404, 20);
            this.buttonDeassociate.Name = "buttonDeassociate";
            this.buttonDeassociate.Size = new System.Drawing.Size(23, 23);
            this.buttonDeassociate.TabIndex = 15;
            this.buttonDeassociate.UseVisualStyleBackColor = true;
            this.buttonDeassociate.Click += new System.EventHandler(this.buttonDeassociate_Click);
            // 
            // buttonAssociate
            // 
            this.buttonAssociate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAssociate.Location = new System.Drawing.Point(313, 370);
            this.buttonAssociate.Name = "buttonAssociate";
            this.buttonAssociate.Size = new System.Drawing.Size(114, 23);
            this.buttonAssociate.TabIndex = 14;
            this.buttonAssociate.Text = "Прив\'язати";
            this.buttonAssociate.UseVisualStyleBackColor = true;
            this.buttonAssociate.Click += new System.EventHandler(this.buttonAssociate_Click);
            // 
            // buttonApplyFilter1NF
            // 
            this.buttonApplyFilter1NF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApplyFilter1NF.Location = new System.Drawing.Point(329, 47);
            this.buttonApplyFilter1NF.Name = "buttonApplyFilter1NF";
            this.buttonApplyFilter1NF.Size = new System.Drawing.Size(98, 23);
            this.buttonApplyFilter1NF.TabIndex = 13;
            this.buttonApplyFilter1NF.Text = "Фільтрувати";
            this.buttonApplyFilter1NF.UseVisualStyleBackColor = true;
            this.buttonApplyFilter1NF.Click += new System.EventHandler(this.buttonApplyFilter1NF_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-3, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Фільтр назви:";
            // 
            // textBoxFilter1NF
            // 
            this.textBoxFilter1NF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilter1NF.Location = new System.Drawing.Point(94, 48);
            this.textBoxFilter1NF.Name = "textBoxFilter1NF";
            this.textBoxFilter1NF.Size = new System.Drawing.Size(229, 21);
            this.textBoxFilter1NF.TabIndex = 11;
            // 
            // checkBoxUseOrgSmartFilter
            // 
            this.checkBoxUseOrgSmartFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxUseOrgSmartFilter.AutoSize = true;
            this.checkBoxUseOrgSmartFilter.Checked = true;
            this.checkBoxUseOrgSmartFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseOrgSmartFilter.Location = new System.Drawing.Point(0, 376);
            this.checkBoxUseOrgSmartFilter.Name = "checkBoxUseOrgSmartFilter";
            this.checkBoxUseOrgSmartFilter.Size = new System.Drawing.Size(96, 17);
            this.checkBoxUseOrgSmartFilter.TabIndex = 9;
            this.checkBoxUseOrgSmartFilter.Text = "Авто-фільтр";
            this.checkBoxUseOrgSmartFilter.UseVisualStyleBackColor = true;
            this.checkBoxUseOrgSmartFilter.CheckedChanged += new System.EventHandler(this.checkBoxUseOrgSmartFilter_CheckedChanged);
            // 
            // listBox1NFOrganizations
            // 
            this.listBox1NFOrganizations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1NFOrganizations.FormattingEnabled = true;
            this.listBox1NFOrganizations.HorizontalScrollbar = true;
            this.listBox1NFOrganizations.Location = new System.Drawing.Point(0, 74);
            this.listBox1NFOrganizations.Name = "listBox1NFOrganizations";
            this.listBox1NFOrganizations.Size = new System.Drawing.Size(427, 290);
            this.listBox1NFOrganizations.TabIndex = 8;
            this.listBox1NFOrganizations.SelectedIndexChanged += new System.EventHandler(this.listBox1NFOrganizations_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Відповідна організація з бази \'1 НФ\'";
            // 
            // textBoxSelectedOrg1NF
            // 
            this.textBoxSelectedOrg1NF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSelectedOrg1NF.Location = new System.Drawing.Point(0, 21);
            this.textBoxSelectedOrg1NF.Name = "textBoxSelectedOrg1NF";
            this.textBoxSelectedOrg1NF.Size = new System.Drawing.Size(398, 21);
            this.textBoxSelectedOrg1NF.TabIndex = 6;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelLeft.Controls.Add(this.buttonApplyFilterBalans);
            this.panelLeft.Controls.Add(this.label3);
            this.panelLeft.Controls.Add(this.textBoxFilterBalans);
            this.panelLeft.Controls.Add(this.checkBoxShowOnlyUnmapped);
            this.panelLeft.Controls.Add(this.listBoxBalansOrganizations);
            this.panelLeft.Controls.Add(this.label1);
            this.panelLeft.Controls.Add(this.textBoxSelectedOrgBalans);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(3, 3);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(430, 398);
            this.panelLeft.TabIndex = 0;
            // 
            // buttonApplyFilterBalans
            // 
            this.buttonApplyFilterBalans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApplyFilterBalans.Location = new System.Drawing.Point(332, 47);
            this.buttonApplyFilterBalans.Name = "buttonApplyFilterBalans";
            this.buttonApplyFilterBalans.Size = new System.Drawing.Size(98, 23);
            this.buttonApplyFilterBalans.TabIndex = 16;
            this.buttonApplyFilterBalans.Text = "Фільтрувати";
            this.buttonApplyFilterBalans.UseVisualStyleBackColor = true;
            this.buttonApplyFilterBalans.Click += new System.EventHandler(this.buttonApplyFilterBalans_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Фільтр назви:";
            // 
            // textBoxFilterBalans
            // 
            this.textBoxFilterBalans.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilterBalans.Location = new System.Drawing.Point(97, 48);
            this.textBoxFilterBalans.Name = "textBoxFilterBalans";
            this.textBoxFilterBalans.Size = new System.Drawing.Size(229, 21);
            this.textBoxFilterBalans.TabIndex = 14;
            // 
            // checkBoxShowOnlyUnmapped
            // 
            this.checkBoxShowOnlyUnmapped.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowOnlyUnmapped.AutoSize = true;
            this.checkBoxShowOnlyUnmapped.Checked = true;
            this.checkBoxShowOnlyUnmapped.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowOnlyUnmapped.Location = new System.Drawing.Point(6, 376);
            this.checkBoxShowOnlyUnmapped.Name = "checkBoxShowOnlyUnmapped";
            this.checkBoxShowOnlyUnmapped.Size = new System.Drawing.Size(379, 17);
            this.checkBoxShowOnlyUnmapped.TabIndex = 9;
            this.checkBoxShowOnlyUnmapped.Text = "Відображати лише організації для яких нема співставлення";
            this.checkBoxShowOnlyUnmapped.UseVisualStyleBackColor = true;
            this.checkBoxShowOnlyUnmapped.CheckedChanged += new System.EventHandler(this.checkBoxShowOnlyUnmapped_CheckedChanged);
            // 
            // listBoxBalansOrganizations
            // 
            this.listBoxBalansOrganizations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxBalansOrganizations.FormattingEnabled = true;
            this.listBoxBalansOrganizations.HorizontalScrollbar = true;
            this.listBoxBalansOrganizations.Location = new System.Drawing.Point(6, 74);
            this.listBoxBalansOrganizations.Name = "listBoxBalansOrganizations";
            this.listBoxBalansOrganizations.Size = new System.Drawing.Size(421, 290);
            this.listBoxBalansOrganizations.TabIndex = 8;
            this.listBoxBalansOrganizations.SelectedIndexChanged += new System.EventHandler(this.listBoxBalansOrganizations_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Організація з бази \'Баланс\'";
            // 
            // textBoxSelectedOrgBalans
            // 
            this.textBoxSelectedOrgBalans.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSelectedOrgBalans.Location = new System.Drawing.Point(6, 21);
            this.textBoxSelectedOrgBalans.Name = "textBoxSelectedOrgBalans";
            this.textBoxSelectedOrgBalans.Size = new System.Drawing.Size(421, 21);
            this.textBoxSelectedOrgBalans.TabIndex = 6;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(753, 448);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(140, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Закрити";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSaveChanges
            // 
            this.buttonSaveChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveChanges.Location = new System.Drawing.Point(607, 448);
            this.buttonSaveChanges.Name = "buttonSaveChanges";
            this.buttonSaveChanges.Size = new System.Drawing.Size(140, 23);
            this.buttonSaveChanges.TabIndex = 2;
            this.buttonSaveChanges.Text = "Зберегти зміни";
            this.buttonSaveChanges.UseVisualStyleBackColor = true;
            this.buttonSaveChanges.Click += new System.EventHandler(this.buttonSaveChanges_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 483);
            this.Controls.Add(this.buttonSaveChanges);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.tabControlMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Співставлення даних бази Баланс та 1 НФ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageOrganizations.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSaveChanges;
        private System.Windows.Forms.TabPage tabPageOrganizations;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.CheckBox checkBoxShowOnlyUnmapped;
        private System.Windows.Forms.ListBox listBoxBalansOrganizations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSelectedOrgBalans;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSelectedOrg1NF;
        private System.Windows.Forms.Button buttonApplyFilter1NF;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFilter1NF;
        private System.Windows.Forms.CheckBox checkBoxUseOrgSmartFilter;
        private System.Windows.Forms.ListBox listBox1NFOrganizations;
        private System.Windows.Forms.Button buttonApplyFilterBalans;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFilterBalans;
        private System.Windows.Forms.Button buttonAssociate;
        private System.Windows.Forms.Button buttonDeassociate;
    }
}