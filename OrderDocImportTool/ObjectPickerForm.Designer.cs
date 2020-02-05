namespace GUKV.DataMigration
{
    partial class ObjectPickerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectPickerForm));
            this.listObjects = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSelectObject = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboStreet = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // listObjects
            // 
            this.listObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listObjects.FullRowSelect = true;
            this.listObjects.Location = new System.Drawing.Point(12, 37);
            this.listObjects.MultiSelect = false;
            this.listObjects.Name = "listObjects";
            this.listObjects.Size = new System.Drawing.Size(462, 163);
            this.listObjects.TabIndex = 14;
            this.listObjects.UseCompatibleStateImageBehavior = false;
            this.listObjects.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Адреса Об\'єкту";
            this.columnHeader1.Width = 455;
            // 
            // btnSelectObject
            // 
            this.btnSelectObject.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelectObject.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectObject.Image")));
            this.btnSelectObject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSelectObject.Location = new System.Drawing.Point(383, 206);
            this.btnSelectObject.Name = "btnSelectObject";
            this.btnSelectObject.Size = new System.Drawing.Size(91, 23);
            this.btnSelectObject.TabIndex = 13;
            this.btnSelectObject.Text = "Вибрати";
            this.btnSelectObject.UseVisualStyleBackColor = true;
            this.btnSelectObject.Click += new System.EventHandler(this.btnSelectObject_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Назва Вулиці";
            // 
            // comboStreet
            // 
            this.comboStreet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboStreet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboStreet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStreet.FormattingEnabled = true;
            this.comboStreet.Location = new System.Drawing.Point(92, 10);
            this.comboStreet.Name = "comboStreet";
            this.comboStreet.Size = new System.Drawing.Size(382, 21);
            this.comboStreet.Sorted = true;
            this.comboStreet.TabIndex = 15;
            this.comboStreet.SelectedIndexChanged += new System.EventHandler(this.comboStreet_SelectedIndexChanged);
            // 
            // ObjectPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 236);
            this.Controls.Add(this.comboStreet);
            this.Controls.Add(this.listObjects);
            this.Controls.Add(this.btnSelectObject);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ObjectPickerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listObjects;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnSelectObject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboStreet;
    }
}