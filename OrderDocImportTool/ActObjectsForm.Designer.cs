namespace GUKV.DataMigration
{
    partial class ActObjectsForm
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
            this.gridObjects = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCreateAct = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.editRishNumber = new System.Windows.Forms.TextBox();
            this.editRishDate = new System.Windows.Forms.DateTimePicker();
            this.btnImportRish = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridObjects)).BeginInit();
            this.SuspendLayout();
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
            this.gridObjects.Location = new System.Drawing.Point(12, 33);
            this.gridObjects.MultiSelect = false;
            this.gridObjects.Name = "gridObjects";
            this.gridObjects.ReadOnly = true;
            this.gridObjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridObjects.Size = new System.Drawing.Size(807, 387);
            this.gridObjects.TabIndex = 0;
            this.gridObjects.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridObjects_CellContentClick);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(682, 426);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(137, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Закрити";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCreateAct
            // 
            this.btnCreateAct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateAct.Location = new System.Drawing.Point(12, 426);
            this.btnCreateAct.Name = "btnCreateAct";
            this.btnCreateAct.Size = new System.Drawing.Size(137, 23);
            this.btnCreateAct.TabIndex = 9;
            this.btnCreateAct.Text = "Створити Акт";
            this.btnCreateAct.UseVisualStyleBackColor = true;
            this.btnCreateAct.Click += new System.EventHandler(this.btnCreateAct_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Номер Рішення";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(254, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Дата Рішення";
            // 
            // editRishNumber
            // 
            this.editRishNumber.Enabled = false;
            this.editRishNumber.Location = new System.Drawing.Point(107, 6);
            this.editRishNumber.MaxLength = 20;
            this.editRishNumber.Name = "editRishNumber";
            this.editRishNumber.Size = new System.Drawing.Size(141, 20);
            this.editRishNumber.TabIndex = 12;
            // 
            // editRishDate
            // 
            this.editRishDate.Enabled = false;
            this.editRishDate.Location = new System.Drawing.Point(337, 6);
            this.editRishDate.Name = "editRishDate";
            this.editRishDate.Size = new System.Drawing.Size(142, 20);
            this.editRishDate.TabIndex = 13;
            // 
            // btnImportRish
            // 
            this.btnImportRish.Location = new System.Drawing.Point(485, 4);
            this.btnImportRish.Name = "btnImportRish";
            this.btnImportRish.Size = new System.Drawing.Size(74, 23);
            this.btnImportRish.TabIndex = 14;
            this.btnImportRish.Text = "Вибрати";
            this.btnImportRish.UseVisualStyleBackColor = true;
            this.btnImportRish.Click += new System.EventHandler(this.btnImportRish_Click);
            // 
            // ActObjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 461);
            this.Controls.Add(this.btnImportRish);
            this.Controls.Add(this.editRishDate);
            this.Controls.Add(this.editRishNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCreateAct);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridObjects);
            this.Name = "ActObjectsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Перелік Об\'єктів за Розпорядчим Документом";
            ((System.ComponentModel.ISupportInitialize)(this.gridObjects)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridObjects;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCreateAct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox editRishNumber;
        private System.Windows.Forms.DateTimePicker editRishDate;
        private System.Windows.Forms.Button btnImportRish;
    }
}