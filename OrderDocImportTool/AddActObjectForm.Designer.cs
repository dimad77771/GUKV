namespace GUKV.DataMigration
{
    partial class AddActObjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddActObjectForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.editAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreateAddr = new System.Windows.Forms.Button();
            this.btnPickAddr = new System.Windows.Forms.Button();
            this.editName = new System.Windows.Forms.TextBox();
            this.editCharacteristic = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboObjKind = new System.Windows.Forms.ComboBox();
            this.comboObjType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboPurposeGroup = new System.Windows.Forms.ComboBox();
            this.comboPurpose = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboTechState = new System.Windows.Forms.ComboBox();
            this.editBalansCost = new System.Windows.Forms.TextBox();
            this.editFinalCost = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.editFloor = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.editBuildYear = new System.Windows.Forms.TextBox();
            this.editExplYear = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.editSquare = new System.Windows.Forms.TextBox();
            this.editDiameter = new System.Windows.Forms.TextBox();
            this.editLength = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(669, 303);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Відмінити";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(588, 303);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Створити";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // editAddress
            // 
            this.editAddress.Enabled = false;
            this.editAddress.Location = new System.Drawing.Point(123, 12);
            this.editAddress.Name = "editAddress";
            this.editAddress.Size = new System.Drawing.Size(565, 20);
            this.editAddress.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Адреса Об\'єкту";
            // 
            // btnCreateAddr
            // 
            this.btnCreateAddr.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateAddr.Image")));
            this.btnCreateAddr.Location = new System.Drawing.Point(722, 11);
            this.btnCreateAddr.Name = "btnCreateAddr";
            this.btnCreateAddr.Size = new System.Drawing.Size(22, 20);
            this.btnCreateAddr.TabIndex = 45;
            this.btnCreateAddr.UseVisualStyleBackColor = true;
            this.btnCreateAddr.Click += new System.EventHandler(this.btnCreateAddr_Click);
            // 
            // btnPickAddr
            // 
            this.btnPickAddr.Image = ((System.Drawing.Image)(resources.GetObject("btnPickAddr.Image")));
            this.btnPickAddr.Location = new System.Drawing.Point(694, 11);
            this.btnPickAddr.Name = "btnPickAddr";
            this.btnPickAddr.Size = new System.Drawing.Size(22, 20);
            this.btnPickAddr.TabIndex = 44;
            this.btnPickAddr.UseVisualStyleBackColor = true;
            this.btnPickAddr.Click += new System.EventHandler(this.btnPickAddr_Click);
            // 
            // editName
            // 
            this.editName.Location = new System.Drawing.Point(123, 38);
            this.editName.MaxLength = 255;
            this.editName.Name = "editName";
            this.editName.Size = new System.Drawing.Size(565, 20);
            this.editName.TabIndex = 46;
            // 
            // editCharacteristic
            // 
            this.editCharacteristic.Location = new System.Drawing.Point(123, 64);
            this.editCharacteristic.MaxLength = 100;
            this.editCharacteristic.Name = "editCharacteristic";
            this.editCharacteristic.Size = new System.Drawing.Size(565, 20);
            this.editCharacteristic.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Назва Об\'єкту";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Характеристика";
            // 
            // comboObjKind
            // 
            this.comboObjKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboObjKind.FormattingEnabled = true;
            this.comboObjKind.Location = new System.Drawing.Point(123, 105);
            this.comboObjKind.Name = "comboObjKind";
            this.comboObjKind.Size = new System.Drawing.Size(221, 21);
            this.comboObjKind.TabIndex = 47;
            // 
            // comboObjType
            // 
            this.comboObjType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboObjType.FormattingEnabled = true;
            this.comboObjType.Location = new System.Drawing.Point(467, 105);
            this.comboObjType.Name = "comboObjType";
            this.comboObjType.Size = new System.Drawing.Size(221, 21);
            this.comboObjType.TabIndex = 47;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Вид Об\'єкту";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(353, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Тип Об\'єкту";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Група Призначення";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(353, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Призначення";
            // 
            // comboPurposeGroup
            // 
            this.comboPurposeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPurposeGroup.FormattingEnabled = true;
            this.comboPurposeGroup.Location = new System.Drawing.Point(123, 132);
            this.comboPurposeGroup.Name = "comboPurposeGroup";
            this.comboPurposeGroup.Size = new System.Drawing.Size(221, 21);
            this.comboPurposeGroup.TabIndex = 47;
            this.comboPurposeGroup.SelectedIndexChanged += new System.EventHandler(this.comboPurposeGroup_SelectedIndexChanged);
            // 
            // comboPurpose
            // 
            this.comboPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPurpose.FormattingEnabled = true;
            this.comboPurpose.Location = new System.Drawing.Point(467, 132);
            this.comboPurpose.Name = "comboPurpose";
            this.comboPurpose.Size = new System.Drawing.Size(221, 21);
            this.comboPurpose.TabIndex = 47;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Технічний Стан";
            // 
            // comboTechState
            // 
            this.comboTechState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTechState.FormattingEnabled = true;
            this.comboTechState.Location = new System.Drawing.Point(123, 159);
            this.comboTechState.Name = "comboTechState";
            this.comboTechState.Size = new System.Drawing.Size(221, 21);
            this.comboTechState.TabIndex = 47;
            // 
            // editBalansCost
            // 
            this.editBalansCost.Location = new System.Drawing.Point(123, 240);
            this.editBalansCost.Name = "editBalansCost";
            this.editBalansCost.Size = new System.Drawing.Size(221, 20);
            this.editBalansCost.TabIndex = 48;
            // 
            // editFinalCost
            // 
            this.editFinalCost.Location = new System.Drawing.Point(467, 240);
            this.editFinalCost.Name = "editFinalCost";
            this.editFinalCost.Size = new System.Drawing.Size(221, 20);
            this.editFinalCost.TabIndex = 49;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 243);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Балансова Вартість";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(353, 243);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(110, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Залишкова Вартість";
            // 
            // editFloor
            // 
            this.editFloor.Location = new System.Drawing.Point(467, 160);
            this.editFloor.Name = "editFloor";
            this.editFloor.Size = new System.Drawing.Size(221, 20);
            this.editFloor.TabIndex = 49;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(353, 162);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Поверх";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 269);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Рік Побудови";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(353, 263);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 27);
            this.label13.TabIndex = 3;
            this.label13.Text = "Рік Введення в Експлуатацію";
            // 
            // editBuildYear
            // 
            this.editBuildYear.Location = new System.Drawing.Point(123, 266);
            this.editBuildYear.Name = "editBuildYear";
            this.editBuildYear.Size = new System.Drawing.Size(221, 20);
            this.editBuildYear.TabIndex = 48;
            // 
            // editExplYear
            // 
            this.editExplYear.Location = new System.Drawing.Point(467, 266);
            this.editExplYear.Name = "editExplYear";
            this.editExplYear.Size = new System.Drawing.Size(221, 20);
            this.editExplYear.TabIndex = 49;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 190);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Площа, кв.м.";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(353, 190);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 13);
            this.label15.TabIndex = 3;
            this.label15.Text = "Довжина, м.";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 216);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(101, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "Діаметр Труб, мм.";
            // 
            // editSquare
            // 
            this.editSquare.Location = new System.Drawing.Point(123, 187);
            this.editSquare.Name = "editSquare";
            this.editSquare.Size = new System.Drawing.Size(221, 20);
            this.editSquare.TabIndex = 48;
            // 
            // editDiameter
            // 
            this.editDiameter.Location = new System.Drawing.Point(123, 213);
            this.editDiameter.MaxLength = 20;
            this.editDiameter.Name = "editDiameter";
            this.editDiameter.Size = new System.Drawing.Size(221, 20);
            this.editDiameter.TabIndex = 48;
            // 
            // editLength
            // 
            this.editLength.Location = new System.Drawing.Point(467, 187);
            this.editLength.Name = "editLength";
            this.editLength.Size = new System.Drawing.Size(221, 20);
            this.editLength.TabIndex = 49;
            // 
            // AddActObjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 335);
            this.Controls.Add(this.editFloor);
            this.Controls.Add(this.editExplYear);
            this.Controls.Add(this.editLength);
            this.Controls.Add(this.editFinalCost);
            this.Controls.Add(this.editDiameter);
            this.Controls.Add(this.editBuildYear);
            this.Controls.Add(this.editSquare);
            this.Controls.Add(this.editBalansCost);
            this.Controls.Add(this.comboPurpose);
            this.Controls.Add(this.comboObjType);
            this.Controls.Add(this.comboTechState);
            this.Controls.Add(this.comboPurposeGroup);
            this.Controls.Add(this.comboObjKind);
            this.Controls.Add(this.editCharacteristic);
            this.Controls.Add(this.editName);
            this.Controls.Add(this.btnCreateAddr);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnPickAddr);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editAddress);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddActObjectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Створити Новий Об\'єкт за Актом";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox editAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreateAddr;
        private System.Windows.Forms.Button btnPickAddr;
        private System.Windows.Forms.TextBox editName;
        private System.Windows.Forms.TextBox editCharacteristic;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboObjKind;
        private System.Windows.Forms.ComboBox comboObjType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboPurposeGroup;
        private System.Windows.Forms.ComboBox comboPurpose;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboTechState;
        private System.Windows.Forms.TextBox editBalansCost;
        private System.Windows.Forms.TextBox editFinalCost;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox editFloor;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox editBuildYear;
        private System.Windows.Forms.TextBox editExplYear;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox editSquare;
        private System.Windows.Forms.TextBox editDiameter;
        private System.Windows.Forms.TextBox editLength;
    }
}