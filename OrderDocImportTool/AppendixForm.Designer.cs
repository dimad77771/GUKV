namespace GUKV.DataMigration
{
    partial class AppendixForm
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
            this.editAppendixNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.gridAppendixObjects = new System.Windows.Forms.DataGridView();
            this.btnAddObject = new System.Windows.Forms.Button();
            this.btnCopyObject = new System.Windows.Forms.Button();
            this.btnDelObject = new System.Windows.Forms.Button();
            this.btnEditObjects = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridAppendixObjects)).BeginInit();
            this.SuspendLayout();
            // 
            // editAppendixNumber
            // 
            this.editAppendixNumber.Location = new System.Drawing.Point(105, 17);
            this.editAppendixNumber.Name = "editAppendixNumber";
            this.editAppendixNumber.ReadOnly = true;
            this.editAppendixNumber.Size = new System.Drawing.Size(155, 20);
            this.editAppendixNumber.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Номер Додатку";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Перелік Об\'єктів Додатку";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(664, 490);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(123, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Закрити";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gridAppendixObjects
            // 
            this.gridAppendixObjects.AllowUserToAddRows = false;
            this.gridAppendixObjects.AllowUserToDeleteRows = false;
            this.gridAppendixObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAppendixObjects.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridAppendixObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAppendixObjects.Location = new System.Drawing.Point(12, 68);
            this.gridAppendixObjects.Name = "gridAppendixObjects";
            this.gridAppendixObjects.ReadOnly = true;
            this.gridAppendixObjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAppendixObjects.Size = new System.Drawing.Size(775, 416);
            this.gridAppendixObjects.TabIndex = 25;
            this.gridAppendixObjects.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridAppendixObjects_CellDoubleClick);
            // 
            // btnAddObject
            // 
            this.btnAddObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddObject.Location = new System.Drawing.Point(12, 490);
            this.btnAddObject.Name = "btnAddObject";
            this.btnAddObject.Size = new System.Drawing.Size(123, 23);
            this.btnAddObject.TabIndex = 26;
            this.btnAddObject.Text = "Додати Об\'єкт";
            this.btnAddObject.UseVisualStyleBackColor = true;
            this.btnAddObject.Click += new System.EventHandler(this.btnAddObject_Click);
            // 
            // btnCopyObject
            // 
            this.btnCopyObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopyObject.Location = new System.Drawing.Point(141, 490);
            this.btnCopyObject.Name = "btnCopyObject";
            this.btnCopyObject.Size = new System.Drawing.Size(123, 23);
            this.btnCopyObject.TabIndex = 26;
            this.btnCopyObject.Text = "Копіювати Об\'єкти";
            this.btnCopyObject.UseVisualStyleBackColor = true;
            this.btnCopyObject.Click += new System.EventHandler(this.btnCopyObject_Click);
            // 
            // btnDelObject
            // 
            this.btnDelObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelObject.Location = new System.Drawing.Point(399, 490);
            this.btnDelObject.Name = "btnDelObject";
            this.btnDelObject.Size = new System.Drawing.Size(123, 23);
            this.btnDelObject.TabIndex = 26;
            this.btnDelObject.Text = "Видалити Об\'єкти";
            this.btnDelObject.UseVisualStyleBackColor = true;
            this.btnDelObject.Click += new System.EventHandler(this.btnDelObject_Click);
            // 
            // btnEditObjects
            // 
            this.btnEditObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditObjects.Location = new System.Drawing.Point(270, 490);
            this.btnEditObjects.Name = "btnEditObjects";
            this.btnEditObjects.Size = new System.Drawing.Size(123, 23);
            this.btnEditObjects.TabIndex = 26;
            this.btnEditObjects.Text = "Редагувати Об\'єкти";
            this.btnEditObjects.UseVisualStyleBackColor = true;
            this.btnEditObjects.Click += new System.EventHandler(this.btnEditObject_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Location = new System.Drawing.Point(664, 39);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(123, 23);
            this.btnSelectAll.TabIndex = 27;
            this.btnSelectAll.Text = "Виділити Всі";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // AppendixForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 521);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnEditObjects);
            this.Controls.Add(this.btnDelObject);
            this.Controls.Add(this.btnCopyObject);
            this.Controls.Add(this.btnAddObject);
            this.Controls.Add(this.gridAppendixObjects);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editAppendixNumber);
            this.Controls.Add(this.label2);
            this.Name = "AppendixForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Додаток до Розпорядчого Документу";
            ((System.ComponentModel.ISupportInitialize)(this.gridAppendixObjects)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox editAppendixNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView gridAppendixObjects;
        private System.Windows.Forms.Button btnAddObject;
        private System.Windows.Forms.Button btnCopyObject;
        private System.Windows.Forms.Button btnDelObject;
        private System.Windows.Forms.Button btnEditObjects;
        private System.Windows.Forms.Button btnSelectAll;
    }
}