namespace GUKV.DataMigration
{
    partial class BalansObjectPickerForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBuilding = new System.Windows.Forms.ComboBox();
            this.comboStreet = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboOrganizations = new System.Windows.Forms.ComboBox();
            this.btnFindOrg = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.editOrgNamePattern = new System.Windows.Forms.TextBox();
            this.editOrgZkpoPattern = new System.Windows.Forms.TextBox();
            this.btnSelectObject = new System.Windows.Forms.Button();
            this.listObjects = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBuilding);
            this.groupBox1.Controls.Add(this.comboStreet);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(659, 51);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Адреса";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Назва Вулиці";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(394, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Номер Будинку";
            // 
            // comboBuilding
            // 
            this.comboBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBuilding.FormattingEnabled = true;
            this.comboBuilding.Location = new System.Drawing.Point(485, 19);
            this.comboBuilding.Name = "comboBuilding";
            this.comboBuilding.Size = new System.Drawing.Size(168, 21);
            this.comboBuilding.Sorted = true;
            this.comboBuilding.TabIndex = 1;
            this.comboBuilding.SelectedIndexChanged += new System.EventHandler(this.comboBuilding_SelectedIndexChanged);
            // 
            // comboStreet
            // 
            this.comboStreet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboStreet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboStreet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStreet.FormattingEnabled = true;
            this.comboStreet.Location = new System.Drawing.Point(96, 19);
            this.comboStreet.Name = "comboStreet";
            this.comboStreet.Size = new System.Drawing.Size(292, 21);
            this.comboStreet.Sorted = true;
            this.comboStreet.TabIndex = 0;
            this.comboStreet.SelectedIndexChanged += new System.EventHandler(this.comboStreet_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboOrganizations);
            this.groupBox2.Controls.Add(this.btnFindOrg);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.editOrgNamePattern);
            this.groupBox2.Controls.Add(this.editOrgZkpoPattern);
            this.groupBox2.Location = new System.Drawing.Point(12, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(659, 75);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Балансоутримувач";
            // 
            // comboOrganizations
            // 
            this.comboOrganizations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboOrganizations.FormattingEnabled = true;
            this.comboOrganizations.Location = new System.Drawing.Point(6, 45);
            this.comboOrganizations.Name = "comboOrganizations";
            this.comboOrganizations.Size = new System.Drawing.Size(647, 21);
            this.comboOrganizations.Sorted = true;
            this.comboOrganizations.TabIndex = 4;
            this.comboOrganizations.SelectedIndexChanged += new System.EventHandler(this.comboOrganizations_SelectedIndexChanged);
            // 
            // btnFindOrg
            // 
            this.btnFindOrg.Location = new System.Drawing.Point(582, 17);
            this.btnFindOrg.Name = "btnFindOrg";
            this.btnFindOrg.Size = new System.Drawing.Size(71, 23);
            this.btnFindOrg.TabIndex = 3;
            this.btnFindOrg.Text = "Знайти";
            this.btnFindOrg.UseVisualStyleBackColor = true;
            this.btnFindOrg.Click += new System.EventHandler(this.btnFindOrg_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(221, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Назва Організації";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Код ЄДРПОУ";
            // 
            // editOrgNamePattern
            // 
            this.editOrgNamePattern.Location = new System.Drawing.Point(325, 19);
            this.editOrgNamePattern.Name = "editOrgNamePattern";
            this.editOrgNamePattern.Size = new System.Drawing.Size(251, 20);
            this.editOrgNamePattern.TabIndex = 1;
            // 
            // editOrgZkpoPattern
            // 
            this.editOrgZkpoPattern.Location = new System.Drawing.Point(96, 19);
            this.editOrgZkpoPattern.Name = "editOrgZkpoPattern";
            this.editOrgZkpoPattern.Size = new System.Drawing.Size(119, 20);
            this.editOrgZkpoPattern.TabIndex = 0;
            // 
            // btnSelectObject
            // 
            this.btnSelectObject.Location = new System.Drawing.Point(600, 322);
            this.btnSelectObject.Name = "btnSelectObject";
            this.btnSelectObject.Size = new System.Drawing.Size(71, 23);
            this.btnSelectObject.TabIndex = 4;
            this.btnSelectObject.Text = "Вибрати";
            this.btnSelectObject.UseVisualStyleBackColor = true;
            this.btnSelectObject.Click += new System.EventHandler(this.btnSelectObject_Click);
            // 
            // listObjects
            // 
            this.listObjects.AutoArrange = false;
            this.listObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader6,
            this.columnHeader7});
            this.listObjects.FullRowSelect = true;
            this.listObjects.GridLines = true;
            this.listObjects.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listObjects.HideSelection = false;
            this.listObjects.Location = new System.Drawing.Point(12, 171);
            this.listObjects.MultiSelect = false;
            this.listObjects.Name = "listObjects";
            this.listObjects.Size = new System.Drawing.Size(659, 145);
            this.listObjects.TabIndex = 5;
            this.listObjects.UseCompatibleStateImageBehavior = false;
            this.listObjects.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Площа (кв.м.)";
            this.columnHeader1.Width = 86;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Об\'єкти на балансі";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Балансоутримувач";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Призначення";
            this.columnHeader6.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Призначення за уточненням Балансоутримувача";
            this.columnHeader7.Width = 260;
            // 
            // BalansObjectPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 352);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listObjects);
            this.Controls.Add(this.btnSelectObject);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BalansObjectPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вибір Об\'єкту на балансі в БД \'1НФ\'";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBuilding;
        private System.Windows.Forms.ComboBox comboStreet;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboOrganizations;
        private System.Windows.Forms.Button btnFindOrg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editOrgNamePattern;
        private System.Windows.Forms.TextBox editOrgZkpoPattern;
        private System.Windows.Forms.Button btnSelectObject;
        private System.Windows.Forms.ListView listObjects;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}