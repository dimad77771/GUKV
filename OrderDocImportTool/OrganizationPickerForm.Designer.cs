namespace GUKV.DataMigration
{
    partial class OrganizationPickerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrganizationPickerForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.editOrgZkpo = new System.Windows.Forms.TextBox();
            this.editOrgName = new System.Windows.Forms.TextBox();
            this.btnFindOrganization = new System.Windows.Forms.Button();
            this.btnSelectOrganization = new System.Windows.Forms.Button();
            this.listOrganizations = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Код ЄДРПОУ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Назва Організації";
            // 
            // editOrgZkpo
            // 
            this.editOrgZkpo.Location = new System.Drawing.Point(114, 20);
            this.editOrgZkpo.Name = "editOrgZkpo";
            this.editOrgZkpo.Size = new System.Drawing.Size(452, 20);
            this.editOrgZkpo.TabIndex = 2;
            this.editOrgZkpo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editOrgZkpo_KeyPress);
            // 
            // editOrgName
            // 
            this.editOrgName.Location = new System.Drawing.Point(114, 50);
            this.editOrgName.Name = "editOrgName";
            this.editOrgName.Size = new System.Drawing.Size(452, 20);
            this.editOrgName.TabIndex = 3;
            this.editOrgName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editOrgName_KeyPress);
            // 
            // btnFindOrganization
            // 
            this.btnFindOrganization.Image = ((System.Drawing.Image)(resources.GetObject("btnFindOrganization.Image")));
            this.btnFindOrganization.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFindOrganization.Location = new System.Drawing.Point(475, 76);
            this.btnFindOrganization.Name = "btnFindOrganization";
            this.btnFindOrganization.Size = new System.Drawing.Size(91, 23);
            this.btnFindOrganization.TabIndex = 4;
            this.btnFindOrganization.Text = "Знайти";
            this.btnFindOrganization.UseVisualStyleBackColor = true;
            this.btnFindOrganization.Click += new System.EventHandler(this.btnFindOrganization_Click);
            // 
            // btnSelectOrganization
            // 
            this.btnSelectOrganization.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelectOrganization.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectOrganization.Image")));
            this.btnSelectOrganization.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSelectOrganization.Location = new System.Drawing.Point(475, 284);
            this.btnSelectOrganization.Name = "btnSelectOrganization";
            this.btnSelectOrganization.Size = new System.Drawing.Size(91, 23);
            this.btnSelectOrganization.TabIndex = 6;
            this.btnSelectOrganization.Text = "Вибрати";
            this.btnSelectOrganization.UseVisualStyleBackColor = true;
            this.btnSelectOrganization.Click += new System.EventHandler(this.btnSelectOrganization_Click);
            // 
            // listOrganizations
            // 
            this.listOrganizations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listOrganizations.FullRowSelect = true;
            this.listOrganizations.Location = new System.Drawing.Point(12, 115);
            this.listOrganizations.MultiSelect = false;
            this.listOrganizations.Name = "listOrganizations";
            this.listOrganizations.Size = new System.Drawing.Size(554, 163);
            this.listOrganizations.TabIndex = 7;
            this.listOrganizations.UseCompatibleStateImageBehavior = false;
            this.listOrganizations.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Код ЄДРПОУ";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Назва";
            this.columnHeader2.Width = 430;
            // 
            // OrganizationPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 315);
            this.Controls.Add(this.listOrganizations);
            this.Controls.Add(this.btnSelectOrganization);
            this.Controls.Add(this.btnFindOrganization);
            this.Controls.Add(this.editOrgName);
            this.Controls.Add(this.editOrgZkpo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrganizationPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вибір Організації";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox editOrgZkpo;
        private System.Windows.Forms.TextBox editOrgName;
        private System.Windows.Forms.Button btnFindOrganization;
        private System.Windows.Forms.Button btnSelectOrganization;
        private System.Windows.Forms.ListView listOrganizations;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}