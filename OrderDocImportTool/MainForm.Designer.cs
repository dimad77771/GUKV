namespace GUKV.DataMigration
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnOpenFile = new System.Windows.Forms.ToolStripButton();
            this.btnEditAct = new System.Windows.Forms.ToolStripButton();
            this.btnSaveProject = new System.Windows.Forms.ToolStripButton();
            this.btnLoadProject = new System.Windows.Forms.ToolStripButton();
            this.comboDocType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.editDocNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.editDocDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.editDocTitle = new System.Windows.Forms.TextBox();
            this.listAppendices = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOpenAppendix = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnExportData = new System.Windows.Forms.Button();
            this.editMasterDocDate = new System.Windows.Forms.DateTimePicker();
            this.editMasterDocNumber = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.labelMasterDoc = new System.Windows.Forms.Label();
            this.openInputFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnAddAppendix = new System.Windows.Forms.Button();
            this.btnDelAppendix = new System.Windows.Forms.Button();
            this.editDocSum = new System.Windows.Forms.TextBox();
            this.editDocFinalSum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnUploadAdd = new System.Windows.Forms.Button();
            this.saveProjectDialog = new System.Windows.Forms.SaveFileDialog();
            this.openProjectDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenFile,
            this.btnEditAct,
            this.btnSaveProject,
            this.btnLoadProject});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(709, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenFile.Image")));
            this.btnOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(132, 22);
            this.btnOpenFile.Text = "Відкрити Документ";
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnEditAct
            // 
            this.btnEditAct.Image = ((System.Drawing.Image)(resources.GetObject("btnEditAct.Image")));
            this.btnEditAct.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditAct.Name = "btnEditAct";
            this.btnEditAct.Size = new System.Drawing.Size(101, 22);
            this.btnEditAct.Text = "Створити Акт";
            this.btnEditAct.Click += new System.EventHandler(this.btnEditAct_Click);
            // 
            // btnSaveProject
            // 
            this.btnSaveProject.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveProject.Image")));
            this.btnSaveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveProject.Name = "btnSaveProject";
            this.btnSaveProject.Size = new System.Drawing.Size(102, 22);
            this.btnSaveProject.Text = "Зберегти дані";
            this.btnSaveProject.Click += new System.EventHandler(this.btnSaveProject_Click);
            // 
            // btnLoadProject
            // 
            this.btnLoadProject.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadProject.Image")));
            this.btnLoadProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadProject.Name = "btnLoadProject";
            this.btnLoadProject.Size = new System.Drawing.Size(159, 22);
            this.btnLoadProject.Text = "Відкрити збережені дані";
            this.btnLoadProject.Click += new System.EventHandler(this.btnLoadProject_Click);
            // 
            // comboDocType
            // 
            this.comboDocType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboDocType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDocType.FormattingEnabled = true;
            this.comboDocType.Location = new System.Drawing.Point(169, 38);
            this.comboDocType.Name = "comboDocType";
            this.comboDocType.Size = new System.Drawing.Size(525, 21);
            this.comboDocType.Sorted = true;
            this.comboDocType.TabIndex = 1;
            this.comboDocType.SelectedIndexChanged += new System.EventHandler(this.comboDocType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Вид Документу";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Номер Документу";
            // 
            // editDocNumber
            // 
            this.editDocNumber.Location = new System.Drawing.Point(169, 65);
            this.editDocNumber.MaxLength = 20;
            this.editDocNumber.Name = "editDocNumber";
            this.editDocNumber.Size = new System.Drawing.Size(318, 20);
            this.editDocNumber.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(506, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "від";
            // 
            // editDocDate
            // 
            this.editDocDate.Location = new System.Drawing.Point(546, 65);
            this.editDocDate.Name = "editDocDate";
            this.editDocDate.Size = new System.Drawing.Size(148, 20);
            this.editDocDate.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Назва Документу";
            // 
            // editDocTitle
            // 
            this.editDocTitle.Location = new System.Drawing.Point(169, 91);
            this.editDocTitle.MaxLength = 255;
            this.editDocTitle.Multiline = true;
            this.editDocTitle.Name = "editDocTitle";
            this.editDocTitle.Size = new System.Drawing.Size(525, 61);
            this.editDocTitle.TabIndex = 3;
            // 
            // listAppendices
            // 
            this.listAppendices.FormattingEnabled = true;
            this.listAppendices.Location = new System.Drawing.Point(15, 265);
            this.listAppendices.Name = "listAppendices";
            this.listAppendices.Size = new System.Drawing.Size(679, 95);
            this.listAppendices.TabIndex = 6;
            this.listAppendices.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listAppendices_MouseDoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 249);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Додатки до Документу";
            // 
            // btnOpenAppendix
            // 
            this.btnOpenAppendix.Location = new System.Drawing.Point(561, 366);
            this.btnOpenAppendix.Name = "btnOpenAppendix";
            this.btnOpenAppendix.Size = new System.Drawing.Size(133, 23);
            this.btnOpenAppendix.TabIndex = 8;
            this.btnOpenAppendix.Text = "Відкрити Додаток";
            this.btnOpenAppendix.UseVisualStyleBackColor = true;
            this.btnOpenAppendix.Click += new System.EventHandler(this.btnOpenAppendix_Click);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(15, 402);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(679, 1);
            this.label6.TabIndex = 9;
            // 
            // btnExportData
            // 
            this.btnExportData.Image = ((System.Drawing.Image)(resources.GetObject("btnExportData.Image")));
            this.btnExportData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportData.Location = new System.Drawing.Point(15, 415);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(137, 23);
            this.btnExportData.TabIndex = 8;
            this.btnExportData.Text = "Завантажити дані";
            this.btnExportData.UseVisualStyleBackColor = true;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // editMasterDocDate
            // 
            this.editMasterDocDate.Location = new System.Drawing.Point(546, 158);
            this.editMasterDocDate.Name = "editMasterDocDate";
            this.editMasterDocDate.Size = new System.Drawing.Size(148, 20);
            this.editMasterDocDate.TabIndex = 13;
            // 
            // editMasterDocNumber
            // 
            this.editMasterDocNumber.Location = new System.Drawing.Point(169, 158);
            this.editMasterDocNumber.MaxLength = 20;
            this.editMasterDocNumber.Name = "editMasterDocNumber";
            this.editMasterDocNumber.Size = new System.Drawing.Size(318, 20);
            this.editMasterDocNumber.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(506, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "від";
            // 
            // labelMasterDoc
            // 
            this.labelMasterDoc.Location = new System.Drawing.Point(12, 155);
            this.labelMasterDoc.Name = "labelMasterDoc";
            this.labelMasterDoc.Size = new System.Drawing.Size(151, 26);
            this.labelMasterDoc.TabIndex = 11;
            this.labelMasterDoc.Text = "Номер Документу, в який вносяться зміни";
            // 
            // openInputFileDialog
            // 
            this.openInputFileDialog.DefaultExt = "doc";
            this.openInputFileDialog.Filter = "Microsoft Word 2003 files|*.doc|Microsoft Word 2007 files|*.docx";
            this.openInputFileDialog.Title = "Виберіть файл Рішення, Розпорядження або Акту";
            // 
            // btnAddAppendix
            // 
            this.btnAddAppendix.Location = new System.Drawing.Point(15, 366);
            this.btnAddAppendix.Name = "btnAddAppendix";
            this.btnAddAppendix.Size = new System.Drawing.Size(137, 23);
            this.btnAddAppendix.TabIndex = 18;
            this.btnAddAppendix.Text = "Створити Додаток";
            this.btnAddAppendix.UseVisualStyleBackColor = true;
            this.btnAddAppendix.Click += new System.EventHandler(this.btnAddAppendix_Click);
            // 
            // btnDelAppendix
            // 
            this.btnDelAppendix.Location = new System.Drawing.Point(158, 366);
            this.btnDelAppendix.Name = "btnDelAppendix";
            this.btnDelAppendix.Size = new System.Drawing.Size(137, 23);
            this.btnDelAppendix.TabIndex = 19;
            this.btnDelAppendix.Text = "Видалити Додаток";
            this.btnDelAppendix.UseVisualStyleBackColor = true;
            this.btnDelAppendix.Click += new System.EventHandler(this.btnDelAppendix_Click);
            // 
            // editDocSum
            // 
            this.editDocSum.Location = new System.Drawing.Point(169, 184);
            this.editDocSum.MaxLength = 50;
            this.editDocSum.Name = "editDocSum";
            this.editDocSum.Size = new System.Drawing.Size(318, 20);
            this.editDocSum.TabIndex = 20;
            // 
            // editDocFinalSum
            // 
            this.editDocFinalSum.Location = new System.Drawing.Point(169, 210);
            this.editDocFinalSum.MaxLength = 50;
            this.editDocFinalSum.Name = "editDocFinalSum";
            this.editDocFinalSum.Size = new System.Drawing.Size(318, 20);
            this.editDocFinalSum.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 188);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Сума за Документом";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(12, 207);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(151, 32);
            this.label9.TabIndex = 23;
            this.label9.Text = "Залишкова сума за Документом";
            // 
            // btnUploadAdd
            // 
            this.btnUploadAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUploadAdd.Location = new System.Drawing.Point(301, 366);
            this.btnUploadAdd.Name = "btnUploadAdd";
            this.btnUploadAdd.Size = new System.Drawing.Size(137, 23);
            this.btnUploadAdd.TabIndex = 24;
            this.btnUploadAdd.Text = " Завантажити додаток";
            this.btnUploadAdd.UseVisualStyleBackColor = true;
            this.btnUploadAdd.Click += new System.EventHandler(this.btnUploadAdd_Click);
            // 
            // saveProjectDialog
            // 
            this.saveProjectDialog.DefaultExt = "rdoc";
            this.saveProjectDialog.Filter = "Файли проектів (*.rdoc)|*.rdoc";
            this.saveProjectDialog.Title = "Зберегти завантажений документ для подальшої обробки";
            // 
            // openProjectDialog
            // 
            this.openProjectDialog.DefaultExt = "rdoc";
            this.openProjectDialog.Filter = "Файли проектів (*.rdoc)|*.rdoc";
            this.openProjectDialog.Title = "Відкрити збережені дані";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 450);
            this.Controls.Add(this.btnUploadAdd);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.editDocFinalSum);
            this.Controls.Add(this.editDocSum);
            this.Controls.Add(this.btnDelAppendix);
            this.Controls.Add(this.btnAddAppendix);
            this.Controls.Add(this.editMasterDocDate);
            this.Controls.Add(this.editMasterDocNumber);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelMasterDoc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnExportData);
            this.Controls.Add(this.btnOpenAppendix);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listAppendices);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.editDocDate);
            this.Controls.Add(this.editDocTitle);
            this.Controls.Add(this.editDocNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboDocType);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Завантаження Розпорядчих Документів";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnOpenFile;
        private System.Windows.Forms.ComboBox comboDocType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox editDocNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker editDocDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox editDocTitle;
        private System.Windows.Forms.ListBox listAppendices;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOpenAppendix;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.DateTimePicker editMasterDocDate;
        private System.Windows.Forms.TextBox editMasterDocNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelMasterDoc;
        private System.Windows.Forms.OpenFileDialog openInputFileDialog;
        private System.Windows.Forms.Button btnAddAppendix;
        private System.Windows.Forms.Button btnDelAppendix;
        private System.Windows.Forms.TextBox editDocSum;
        private System.Windows.Forms.TextBox editDocFinalSum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnUploadAdd;
        private System.Windows.Forms.ToolStripButton btnEditAct;
        private System.Windows.Forms.ToolStripButton btnSaveProject;
        private System.Windows.Forms.ToolStripButton btnLoadProject;
        private System.Windows.Forms.SaveFileDialog saveProjectDialog;
        private System.Windows.Forms.OpenFileDialog openProjectDialog;
    }
}

