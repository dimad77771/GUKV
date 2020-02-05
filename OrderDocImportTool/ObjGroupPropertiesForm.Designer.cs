namespace GUKV.DataMigration
{
    partial class ObjGroupPropertiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjGroupPropertiesForm));
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.comboPurpose = new System.Windows.Forms.ComboBox();
            this.comboPurposeGroup = new System.Windows.Forms.ComboBox();
            this.comboObjectKind = new System.Windows.Forms.ComboBox();
            this.comboObjectType = new System.Windows.Forms.ComboBox();
            this.btnDelTransfer = new System.Windows.Forms.Button();
            this.btnAddTransfer = new System.Windows.Forms.Button();
            this.listTransfers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNewObject = new System.Windows.Forms.Button();
            this.btnPickObject = new System.Windows.Forms.Button();
            this.editObject1NF = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.editObjectName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.editCharacteristics = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(345, 42);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 13);
            this.label14.TabIndex = 61;
            this.label14.Text = "Призначення";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(345, 15);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(106, 13);
            this.label13.TabIndex = 59;
            this.label13.Text = "Група Призначення";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 43);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 58;
            this.label12.Text = "Вид Об\'єкту";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 60;
            this.label11.Text = "Тип Об\'єкту";
            // 
            // comboPurpose
            // 
            this.comboPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPurpose.FormattingEnabled = true;
            this.comboPurpose.Location = new System.Drawing.Point(457, 39);
            this.comboPurpose.Name = "comboPurpose";
            this.comboPurpose.Size = new System.Drawing.Size(223, 21);
            this.comboPurpose.Sorted = true;
            this.comboPurpose.TabIndex = 57;
            // 
            // comboPurposeGroup
            // 
            this.comboPurposeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPurposeGroup.FormattingEnabled = true;
            this.comboPurposeGroup.Location = new System.Drawing.Point(457, 12);
            this.comboPurposeGroup.Name = "comboPurposeGroup";
            this.comboPurposeGroup.Size = new System.Drawing.Size(223, 21);
            this.comboPurposeGroup.Sorted = true;
            this.comboPurposeGroup.TabIndex = 56;
            this.comboPurposeGroup.SelectedIndexChanged += new System.EventHandler(this.comboPurposeGroup_SelectedIndexChanged);
            // 
            // comboObjectKind
            // 
            this.comboObjectKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboObjectKind.FormattingEnabled = true;
            this.comboObjectKind.Location = new System.Drawing.Point(109, 40);
            this.comboObjectKind.Name = "comboObjectKind";
            this.comboObjectKind.Size = new System.Drawing.Size(224, 21);
            this.comboObjectKind.Sorted = true;
            this.comboObjectKind.TabIndex = 55;
            // 
            // comboObjectType
            // 
            this.comboObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboObjectType.FormattingEnabled = true;
            this.comboObjectType.Location = new System.Drawing.Point(109, 12);
            this.comboObjectType.Name = "comboObjectType";
            this.comboObjectType.Size = new System.Drawing.Size(224, 21);
            this.comboObjectType.Sorted = true;
            this.comboObjectType.TabIndex = 54;
            // 
            // btnDelTransfer
            // 
            this.btnDelTransfer.Location = new System.Drawing.Point(135, 365);
            this.btnDelTransfer.Name = "btnDelTransfer";
            this.btnDelTransfer.Size = new System.Drawing.Size(117, 23);
            this.btnDelTransfer.TabIndex = 75;
            this.btnDelTransfer.Text = "Видалити Право";
            this.btnDelTransfer.UseVisualStyleBackColor = true;
            this.btnDelTransfer.Click += new System.EventHandler(this.btnDelTransfer_Click);
            // 
            // btnAddTransfer
            // 
            this.btnAddTransfer.Location = new System.Drawing.Point(12, 365);
            this.btnAddTransfer.Name = "btnAddTransfer";
            this.btnAddTransfer.Size = new System.Drawing.Size(117, 23);
            this.btnAddTransfer.TabIndex = 74;
            this.btnAddTransfer.Text = "Додати Право";
            this.btnAddTransfer.UseVisualStyleBackColor = true;
            this.btnAddTransfer.Click += new System.EventHandler(this.btnAddTransfer_Click);
            // 
            // listTransfers
            // 
            this.listTransfers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listTransfers.FullRowSelect = true;
            this.listTransfers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listTransfers.Location = new System.Drawing.Point(12, 163);
            this.listTransfers.MultiSelect = false;
            this.listTransfers.Name = "listTransfers";
            this.listTransfers.ShowItemToolTips = true;
            this.listTransfers.Size = new System.Drawing.Size(668, 193);
            this.listTransfers.TabIndex = 73;
            this.listTransfers.UseCompatibleStateImageBehavior = false;
            this.listTransfers.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Від Кого";
            this.columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Кому";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Право";
            this.columnHeader3.Width = 160;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(496, 365);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 23);
            this.btnSave.TabIndex = 71;
            this.btnSave.Text = "Зберегти";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(591, 365);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 23);
            this.btnClose.TabIndex = 72;
            this.btnClose.Text = "Закрити";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 76;
            this.label1.Text = "Права на об\'єкти";
            // 
            // btnNewObject
            // 
            this.btnNewObject.Image = ((System.Drawing.Image)(resources.GetObject("btnNewObject.Image")));
            this.btnNewObject.Location = new System.Drawing.Point(658, 93);
            this.btnNewObject.Name = "btnNewObject";
            this.btnNewObject.Size = new System.Drawing.Size(22, 20);
            this.btnNewObject.TabIndex = 80;
            this.btnNewObject.UseVisualStyleBackColor = true;
            this.btnNewObject.Click += new System.EventHandler(this.btnNewObject_Click);
            // 
            // btnPickObject
            // 
            this.btnPickObject.Image = ((System.Drawing.Image)(resources.GetObject("btnPickObject.Image")));
            this.btnPickObject.Location = new System.Drawing.Point(630, 93);
            this.btnPickObject.Name = "btnPickObject";
            this.btnPickObject.Size = new System.Drawing.Size(22, 20);
            this.btnPickObject.TabIndex = 79;
            this.btnPickObject.UseVisualStyleBackColor = true;
            this.btnPickObject.Click += new System.EventHandler(this.btnPickObject_Click);
            // 
            // editObject1NF
            // 
            this.editObject1NF.Location = new System.Drawing.Point(109, 93);
            this.editObject1NF.Name = "editObject1NF";
            this.editObject1NF.ReadOnly = true;
            this.editObject1NF.Size = new System.Drawing.Size(511, 20);
            this.editObject1NF.TabIndex = 78;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 77;
            this.label3.Text = "Об\'єкт в БД";
            // 
            // editObjectName
            // 
            this.editObjectName.Location = new System.Drawing.Point(109, 67);
            this.editObjectName.MaxLength = 255;
            this.editObjectName.Name = "editObjectName";
            this.editObjectName.Size = new System.Drawing.Size(571, 20);
            this.editObjectName.TabIndex = 82;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 13);
            this.label10.TabIndex = 81;
            this.label10.Text = "Назва Об\'єкту";
            // 
            // editCharacteristics
            // 
            this.editCharacteristics.Location = new System.Drawing.Point(109, 119);
            this.editCharacteristics.MaxLength = 100;
            this.editCharacteristics.Name = "editCharacteristics";
            this.editCharacteristics.Size = new System.Drawing.Size(571, 20);
            this.editCharacteristics.TabIndex = 84;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 83;
            this.label4.Text = "Характеристика";
            // 
            // ObjGroupPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 396);
            this.Controls.Add(this.editCharacteristics);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.editObjectName);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnNewObject);
            this.Controls.Add(this.btnPickObject);
            this.Controls.Add(this.editObject1NF);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDelTransfer);
            this.Controls.Add(this.btnAddTransfer);
            this.Controls.Add(this.listTransfers);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.comboPurpose);
            this.Controls.Add(this.comboPurposeGroup);
            this.Controls.Add(this.comboObjectKind);
            this.Controls.Add(this.comboObjectType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjGroupPropertiesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Параметри Групи Об\'єктів";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboPurpose;
        private System.Windows.Forms.ComboBox comboPurposeGroup;
        private System.Windows.Forms.ComboBox comboObjectKind;
        private System.Windows.Forms.ComboBox comboObjectType;
        private System.Windows.Forms.Button btnDelTransfer;
        private System.Windows.Forms.Button btnAddTransfer;
        private System.Windows.Forms.ListView listTransfers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNewObject;
        private System.Windows.Forms.Button btnPickObject;
        private System.Windows.Forms.TextBox editObject1NF;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editObjectName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox editCharacteristics;
        private System.Windows.Forms.Label label4;
    }
}