namespace GUKV.DataMigration
{
    partial class CreateNewObjectForm
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
            this.comboStreet = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.editNumber1 = new System.Windows.Forms.TextBox();
            this.editNumber3 = new System.Windows.Forms.TextBox();
            this.editNumber2 = new System.Windows.Forms.TextBox();
            this.editAddrMisc = new System.Windows.Forms.TextBox();
            this.comboDistrict = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboStreet
            // 
            this.comboStreet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboStreet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboStreet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStreet.FormattingEnabled = true;
            this.comboStreet.Location = new System.Drawing.Point(120, 42);
            this.comboStreet.Name = "comboStreet";
            this.comboStreet.Size = new System.Drawing.Size(401, 21);
            this.comboStreet.Sorted = true;
            this.comboStreet.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Назва вулиці";
            // 
            // editNumber1
            // 
            this.editNumber1.Location = new System.Drawing.Point(120, 69);
            this.editNumber1.MaxLength = 4;
            this.editNumber1.Name = "editNumber1";
            this.editNumber1.Size = new System.Drawing.Size(79, 20);
            this.editNumber1.TabIndex = 2;
            // 
            // editNumber3
            // 
            this.editNumber3.Location = new System.Drawing.Point(442, 69);
            this.editNumber3.MaxLength = 3;
            this.editNumber3.Name = "editNumber3";
            this.editNumber3.Size = new System.Drawing.Size(79, 20);
            this.editNumber3.TabIndex = 3;
            // 
            // editNumber2
            // 
            this.editNumber2.Location = new System.Drawing.Point(298, 69);
            this.editNumber2.MaxLength = 4;
            this.editNumber2.Name = "editNumber2";
            this.editNumber2.Size = new System.Drawing.Size(79, 20);
            this.editNumber2.TabIndex = 4;
            // 
            // editAddrMisc
            // 
            this.editAddrMisc.Location = new System.Drawing.Point(120, 95);
            this.editAddrMisc.MaxLength = 60;
            this.editAddrMisc.Name = "editAddrMisc";
            this.editAddrMisc.Size = new System.Drawing.Size(401, 20);
            this.editAddrMisc.TabIndex = 5;
            // 
            // comboDistrict
            // 
            this.comboDistrict.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboDistrict.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboDistrict.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDistrict.FormattingEnabled = true;
            this.comboDistrict.Location = new System.Drawing.Point(120, 15);
            this.comboDistrict.Name = "comboDistrict";
            this.comboDistrict.Size = new System.Drawing.Size(401, 21);
            this.comboDistrict.Sorted = true;
            this.comboDistrict.TabIndex = 6;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(365, 134);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "Створити";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(446, 134);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Відмінити";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Район";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Номер будинку";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Додаткова адреса";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(208, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 26);
            this.label5.TabIndex = 1;
            this.label5.Text = "Номер будинку (додатково)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(394, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Корпус";
            // 
            // CreateNewObjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 166);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.comboDistrict);
            this.Controls.Add(this.editAddrMisc);
            this.Controls.Add(this.editNumber2);
            this.Controls.Add(this.editNumber3);
            this.Controls.Add(this.editNumber1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboStreet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateNewObjectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Створити новий об\'єкт";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboStreet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editNumber1;
        private System.Windows.Forms.TextBox editNumber3;
        private System.Windows.Forms.TextBox editNumber2;
        private System.Windows.Forms.TextBox editAddrMisc;
        private System.Windows.Forms.ComboBox comboDistrict;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}