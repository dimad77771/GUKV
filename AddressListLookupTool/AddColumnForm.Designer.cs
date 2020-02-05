namespace GUKV.DataMigration
{
    partial class AddColumnForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.radioButtonObjectField = new System.Windows.Forms.RadioButton();
            this.radioButtonBalansField = new System.Windows.Forms.RadioButton();
            this.radioButtonArendaField = new System.Windows.Forms.RadioButton();
            this.listBoxFields = new System.Windows.Forms.CheckedListBox();
            this.radioButtonPrivatField = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(553, 464);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(104, 25);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Відмінити";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(443, 464);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(104, 25);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "Додати";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // radioButtonObjectField
            // 
            this.radioButtonObjectField.AutoSize = true;
            this.radioButtonObjectField.Location = new System.Drawing.Point(12, 12);
            this.radioButtonObjectField.Name = "radioButtonObjectField";
            this.radioButtonObjectField.Size = new System.Drawing.Size(155, 17);
            this.radioButtonObjectField.TabIndex = 5;
            this.radioButtonObjectField.TabStop = true;
            this.radioButtonObjectField.Text = "Інформація щодо будинку";
            this.radioButtonObjectField.UseVisualStyleBackColor = true;
            this.radioButtonObjectField.CheckedChanged += new System.EventHandler(this.radioButtonObjectField_CheckedChanged);
            // 
            // radioButtonBalansField
            // 
            this.radioButtonBalansField.AutoSize = true;
            this.radioButtonBalansField.Location = new System.Drawing.Point(12, 35);
            this.radioButtonBalansField.Name = "radioButtonBalansField";
            this.radioButtonBalansField.Size = new System.Drawing.Size(217, 17);
            this.radioButtonBalansField.TabIndex = 5;
            this.radioButtonBalansField.TabStop = true;
            this.radioButtonBalansField.Text = "Інформація щодо балансоутримувачів";
            this.radioButtonBalansField.UseVisualStyleBackColor = true;
            this.radioButtonBalansField.CheckedChanged += new System.EventHandler(this.radioButtonBalansField_CheckedChanged);
            // 
            // radioButtonArendaField
            // 
            this.radioButtonArendaField.AutoSize = true;
            this.radioButtonArendaField.Location = new System.Drawing.Point(12, 58);
            this.radioButtonArendaField.Name = "radioButtonArendaField";
            this.radioButtonArendaField.Size = new System.Drawing.Size(165, 17);
            this.radioButtonArendaField.TabIndex = 5;
            this.radioButtonArendaField.TabStop = true;
            this.radioButtonArendaField.Text = "Інформація щодо орендарів";
            this.radioButtonArendaField.UseVisualStyleBackColor = true;
            this.radioButtonArendaField.CheckedChanged += new System.EventHandler(this.radioButtonArendaField_CheckedChanged);
            // 
            // listBoxFields
            // 
            this.listBoxFields.CheckOnClick = true;
            this.listBoxFields.FormattingEnabled = true;
            this.listBoxFields.Location = new System.Drawing.Point(12, 105);
            this.listBoxFields.Name = "listBoxFields";
            this.listBoxFields.Size = new System.Drawing.Size(645, 349);
            this.listBoxFields.TabIndex = 6;
            this.listBoxFields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listBoxFields_ItemCheck);
            // 
            // radioButtonPrivatField
            // 
            this.radioButtonPrivatField.AutoSize = true;
            this.radioButtonPrivatField.Location = new System.Drawing.Point(12, 81);
            this.radioButtonPrivatField.Name = "radioButtonPrivatField";
            this.radioButtonPrivatField.Size = new System.Drawing.Size(179, 17);
            this.radioButtonPrivatField.TabIndex = 7;
            this.radioButtonPrivatField.TabStop = true;
            this.radioButtonPrivatField.Text = "Інформація щодо приватизації";
            this.radioButtonPrivatField.UseVisualStyleBackColor = true;
            this.radioButtonPrivatField.CheckedChanged += new System.EventHandler(this.radioButtonPrivatField_CheckedChanged);
            // 
            // AddColumnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 498);
            this.Controls.Add(this.radioButtonPrivatField);
            this.Controls.Add(this.listBoxFields);
            this.Controls.Add(this.radioButtonArendaField);
            this.Controls.Add(this.radioButtonBalansField);
            this.Controls.Add(this.radioButtonObjectField);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddColumnForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Додати Колонку";
            this.Load += new System.EventHandler(this.AddColumnForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.RadioButton radioButtonObjectField;
        private System.Windows.Forms.RadioButton radioButtonBalansField;
        private System.Windows.Forms.RadioButton radioButtonArendaField;
        private System.Windows.Forms.CheckedListBox listBoxFields;
        private System.Windows.Forms.RadioButton radioButtonPrivatField;
    }
}