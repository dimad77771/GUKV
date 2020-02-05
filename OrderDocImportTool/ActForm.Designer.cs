namespace GUKV.DataMigration
{
    partial class ActForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActForm));
            this.checkModify1NFImmediately = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.editActFinalSum = new System.Windows.Forms.TextBox();
            this.editActSum = new System.Windows.Forms.TextBox();
            this.editActTitle = new System.Windows.Forms.TextBox();
            this.btnExportData = new System.Windows.Forms.Button();
            this.editRishDate = new System.Windows.Forms.DateTimePicker();
            this.editActDate = new System.Windows.Forms.DateTimePicker();
            this.editRishNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.editActNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEditObject = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gridObjects = new System.Windows.Forms.DataGridView();
            this.btnAddObject = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridObjects)).BeginInit();
            this.SuspendLayout();
            // 
            // checkModify1NFImmediately
            // 
            this.checkModify1NFImmediately.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkModify1NFImmediately.AutoSize = true;
            this.checkModify1NFImmediately.Location = new System.Drawing.Point(157, 446);
            this.checkModify1NFImmediately.Name = "checkModify1NFImmediately";
            this.checkModify1NFImmediately.Size = new System.Drawing.Size(412, 17);
            this.checkModify1NFImmediately.TabIndex = 49;
            this.checkModify1NFImmediately.Text = "Одразу внести зміни у звіти балансоутримувачів, об\'єкти яких передаються";
            this.checkModify1NFImmediately.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(253, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(142, 13);
            this.label9.TabIndex = 48;
            this.label9.Text = "Залишкова сума за Актом";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 47;
            this.label8.Text = "Сума за Актом";
            // 
            // editActFinalSum
            // 
            this.editActFinalSum.Location = new System.Drawing.Point(401, 60);
            this.editActFinalSum.MaxLength = 50;
            this.editActFinalSum.Name = "editActFinalSum";
            this.editActFinalSum.Size = new System.Drawing.Size(142, 20);
            this.editActFinalSum.TabIndex = 46;
            // 
            // editActSum
            // 
            this.editActSum.Location = new System.Drawing.Point(106, 60);
            this.editActSum.MaxLength = 50;
            this.editActSum.Name = "editActSum";
            this.editActSum.Size = new System.Drawing.Size(141, 20);
            this.editActSum.TabIndex = 45;
            // 
            // editActTitle
            // 
            this.editActTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.editActTitle.Location = new System.Drawing.Point(106, 34);
            this.editActTitle.Name = "editActTitle";
            this.editActTitle.Size = new System.Drawing.Size(682, 20);
            this.editActTitle.TabIndex = 44;
            this.editActTitle.Text = "АКТ ПРИЙМАННЯ-ПЕРЕДАЧІ";
            // 
            // btnExportData
            // 
            this.btnExportData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportData.Image = ((System.Drawing.Image)(resources.GetObject("btnExportData.Image")));
            this.btnExportData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportData.Location = new System.Drawing.Point(14, 442);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(137, 23);
            this.btnExportData.TabIndex = 43;
            this.btnExportData.Text = "Завантажити дані";
            this.btnExportData.UseVisualStyleBackColor = true;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // editRishDate
            // 
            this.editRishDate.Enabled = false;
            this.editRishDate.Location = new System.Drawing.Point(401, 86);
            this.editRishDate.Name = "editRishDate";
            this.editRishDate.Size = new System.Drawing.Size(142, 20);
            this.editRishDate.TabIndex = 40;
            // 
            // editActDate
            // 
            this.editActDate.Location = new System.Drawing.Point(401, 8);
            this.editActDate.Name = "editActDate";
            this.editActDate.Size = new System.Drawing.Size(142, 20);
            this.editActDate.TabIndex = 41;
            // 
            // editRishNumber
            // 
            this.editRishNumber.Enabled = false;
            this.editRishNumber.Location = new System.Drawing.Point(106, 86);
            this.editRishNumber.MaxLength = 20;
            this.editRishNumber.Name = "editRishNumber";
            this.editRishNumber.Size = new System.Drawing.Size(141, 20);
            this.editRishNumber.TabIndex = 39;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(253, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Дата Рішення";
            // 
            // editActNumber
            // 
            this.editActNumber.Location = new System.Drawing.Point(106, 8);
            this.editActNumber.MaxLength = 20;
            this.editActNumber.Name = "editActNumber";
            this.editActNumber.Size = new System.Drawing.Size(141, 20);
            this.editActNumber.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Номер Рішення";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(253, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Дата Акту";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "Назва Акту";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Номер Акту";
            // 
            // btnEditObject
            // 
            this.btnEditObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditObject.Location = new System.Drawing.Point(300, 414);
            this.btnEditObject.Name = "btnEditObject";
            this.btnEditObject.Size = new System.Drawing.Size(137, 23);
            this.btnEditObject.TabIndex = 31;
            this.btnEditObject.Text = "Редагувати об\'єкт";
            this.btnEditObject.UseVisualStyleBackColor = true;
            this.btnEditObject.Click += new System.EventHandler(this.btnEditObject_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(14, 414);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(137, 23);
            this.btnDelete.TabIndex = 32;
            this.btnDelete.Text = "Видалити вибраний";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(651, 414);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(137, 23);
            this.btnClose.TabIndex = 30;
            this.btnClose.Text = "Закрити";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gridObjects
            // 
            this.gridObjects.AllowUserToAddRows = false;
            this.gridObjects.AllowUserToDeleteRows = false;
            this.gridObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridObjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridObjects.Location = new System.Drawing.Point(14, 112);
            this.gridObjects.MultiSelect = false;
            this.gridObjects.Name = "gridObjects";
            this.gridObjects.ReadOnly = true;
            this.gridObjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridObjects.Size = new System.Drawing.Size(774, 296);
            this.gridObjects.TabIndex = 29;
            this.gridObjects.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridObjects_CellContentClick);
            this.gridObjects.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridObjects_CellDoubleClick);
            // 
            // btnAddObject
            // 
            this.btnAddObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddObject.Location = new System.Drawing.Point(157, 414);
            this.btnAddObject.Name = "btnAddObject";
            this.btnAddObject.Size = new System.Drawing.Size(137, 23);
            this.btnAddObject.TabIndex = 32;
            this.btnAddObject.Text = "Додати об\'єкт";
            this.btnAddObject.UseVisualStyleBackColor = true;
            this.btnAddObject.Click += new System.EventHandler(this.btnAddObject_Click);
            // 
            // ActForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 477);
            this.Controls.Add(this.checkModify1NFImmediately);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.editActFinalSum);
            this.Controls.Add(this.editActSum);
            this.Controls.Add(this.editActTitle);
            this.Controls.Add(this.btnExportData);
            this.Controls.Add(this.editRishDate);
            this.Controls.Add(this.editActDate);
            this.Controls.Add(this.editRishNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.editActNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnEditObject);
            this.Controls.Add(this.btnAddObject);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridObjects);
            this.Name = "ActForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Перелік Об\'єктів за Актом";
            ((System.ComponentModel.ISupportInitialize)(this.gridObjects)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkModify1NFImmediately;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox editActFinalSum;
        private System.Windows.Forms.TextBox editActSum;
        private System.Windows.Forms.TextBox editActTitle;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.DateTimePicker editRishDate;
        private System.Windows.Forms.DateTimePicker editActDate;
        private System.Windows.Forms.TextBox editRishNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox editActNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnEditObject;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView gridObjects;
        private System.Windows.Forms.Button btnAddObject;
    }
}