namespace GUKV.DataMigration
{
    partial class AddObjTransferForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddObjTransferForm));
            this.comboRight = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnPickOrgTo = new System.Windows.Forms.Button();
            this.editOrgTo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnPickOrgFrom = new System.Windows.Forms.Button();
            this.editOrgFrom = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCreateOrgFrom = new System.Windows.Forms.Button();
            this.btnCreateOrgTo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboRight
            // 
            this.comboRight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRight.FormattingEnabled = true;
            this.comboRight.Location = new System.Drawing.Point(110, 64);
            this.comboRight.Name = "comboRight";
            this.comboRight.Size = new System.Drawing.Size(378, 21);
            this.comboRight.TabIndex = 48;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 47;
            this.label6.Text = "Право";
            // 
            // btnPickOrgTo
            // 
            this.btnPickOrgTo.Image = ((System.Drawing.Image)(resources.GetObject("btnPickOrgTo.Image")));
            this.btnPickOrgTo.Location = new System.Drawing.Point(494, 38);
            this.btnPickOrgTo.Name = "btnPickOrgTo";
            this.btnPickOrgTo.Size = new System.Drawing.Size(22, 20);
            this.btnPickOrgTo.TabIndex = 46;
            this.btnPickOrgTo.UseVisualStyleBackColor = true;
            this.btnPickOrgTo.Click += new System.EventHandler(this.btnPickOrgTo_Click);
            // 
            // editOrgTo
            // 
            this.editOrgTo.Location = new System.Drawing.Point(110, 38);
            this.editOrgTo.Name = "editOrgTo";
            this.editOrgTo.Size = new System.Drawing.Size(378, 20);
            this.editOrgTo.TabIndex = 45;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 44;
            this.label7.Text = "Передається до";
            // 
            // btnPickOrgFrom
            // 
            this.btnPickOrgFrom.Image = ((System.Drawing.Image)(resources.GetObject("btnPickOrgFrom.Image")));
            this.btnPickOrgFrom.Location = new System.Drawing.Point(494, 11);
            this.btnPickOrgFrom.Name = "btnPickOrgFrom";
            this.btnPickOrgFrom.Size = new System.Drawing.Size(22, 20);
            this.btnPickOrgFrom.TabIndex = 43;
            this.btnPickOrgFrom.UseVisualStyleBackColor = true;
            this.btnPickOrgFrom.Click += new System.EventHandler(this.btnPickOrgFrom_Click);
            // 
            // editOrgFrom
            // 
            this.editOrgFrom.Location = new System.Drawing.Point(110, 12);
            this.editOrgFrom.Name = "editOrgFrom";
            this.editOrgFrom.Size = new System.Drawing.Size(378, 20);
            this.editOrgFrom.TabIndex = 42;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "Передається від";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(388, 102);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 49;
            this.btnOK.Text = "Зберегти";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(469, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 50;
            this.btnCancel.Text = "Відмінити";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnCreateOrgFrom
            // 
            this.btnCreateOrgFrom.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateOrgFrom.Image")));
            this.btnCreateOrgFrom.Location = new System.Drawing.Point(522, 11);
            this.btnCreateOrgFrom.Name = "btnCreateOrgFrom";
            this.btnCreateOrgFrom.Size = new System.Drawing.Size(22, 20);
            this.btnCreateOrgFrom.TabIndex = 43;
            this.btnCreateOrgFrom.UseVisualStyleBackColor = true;
            this.btnCreateOrgFrom.Click += new System.EventHandler(this.btnCreateOrgFrom_Click);
            // 
            // btnCreateOrgTo
            // 
            this.btnCreateOrgTo.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateOrgTo.Image")));
            this.btnCreateOrgTo.Location = new System.Drawing.Point(522, 38);
            this.btnCreateOrgTo.Name = "btnCreateOrgTo";
            this.btnCreateOrgTo.Size = new System.Drawing.Size(22, 20);
            this.btnCreateOrgTo.TabIndex = 46;
            this.btnCreateOrgTo.UseVisualStyleBackColor = true;
            this.btnCreateOrgTo.Click += new System.EventHandler(this.btnCreateOrgTo_Click);
            // 
            // AddObjTransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 134);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.comboRight);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCreateOrgTo);
            this.Controls.Add(this.btnPickOrgTo);
            this.Controls.Add(this.editOrgTo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnCreateOrgFrom);
            this.Controls.Add(this.btnPickOrgFrom);
            this.Controls.Add(this.editOrgFrom);
            this.Controls.Add(this.label8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddObjTransferForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Передача Права на Об\'єкт";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboRight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnPickOrgTo;
        private System.Windows.Forms.TextBox editOrgTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnPickOrgFrom;
        private System.Windows.Forms.TextBox editOrgFrom;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCreateOrgFrom;
        private System.Windows.Forms.Button btnCreateOrgTo;
    }
}