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
            this.gridAddressTable = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolButtonWrite = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonAddColumn = new System.Windows.Forms.ToolStripButton();
            this.toolButtonDelColumn = new System.Windows.Forms.ToolStripButton();
            this.toolButtonAddrColumn = new System.Windows.Forms.ToolStripButton();
            this.toolEditAddressColumn = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuItemAllowInpreciseAddrMatch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDisplayFoundAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonExit = new System.Windows.Forms.Button();
            this.openInputFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveOutputFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.readingProgressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.gridAddressTable)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridAddressTable
            // 
            this.gridAddressTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAddressTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridAddressTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAddressTable.Location = new System.Drawing.Point(14, 30);
            this.gridAddressTable.Name = "gridAddressTable";
            this.gridAddressTable.ReadOnly = true;
            this.gridAddressTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridAddressTable.Size = new System.Drawing.Size(938, 504);
            this.gridAddressTable.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolButtonOpen,
            this.toolButtonWrite,
            this.toolStripSeparator1,
            this.toolButtonAddColumn,
            this.toolButtonDelColumn,
            this.toolButtonAddrColumn,
            this.toolEditAddressColumn,
            this.toolStripSeparator2,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(966, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolButtonOpen
            // 
            this.toolButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonOpen.Image")));
            this.toolButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonOpen.Name = "toolButtonOpen";
            this.toolButtonOpen.Size = new System.Drawing.Size(23, 22);
            this.toolButtonOpen.Text = "toolStripButton1";
            this.toolButtonOpen.ToolTipText = "Відкрити файл з переліком адрес";
            this.toolButtonOpen.Click += new System.EventHandler(this.toolButtonOpen_Click);
            // 
            // toolButtonWrite
            // 
            this.toolButtonWrite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolButtonWrite.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonWrite.Image")));
            this.toolButtonWrite.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonWrite.Name = "toolButtonWrite";
            this.toolButtonWrite.Size = new System.Drawing.Size(23, 22);
            this.toolButtonWrite.Text = "toolStripButton1";
            this.toolButtonWrite.ToolTipText = "Зберегти результати в форматі MS Word";
            this.toolButtonWrite.Click += new System.EventHandler(this.toolButtonWrite_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolButtonAddColumn
            // 
            this.toolButtonAddColumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolButtonAddColumn.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonAddColumn.Image")));
            this.toolButtonAddColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAddColumn.Name = "toolButtonAddColumn";
            this.toolButtonAddColumn.Size = new System.Drawing.Size(23, 22);
            this.toolButtonAddColumn.Text = "toolStripButton1";
            this.toolButtonAddColumn.ToolTipText = "Додати нову колонку";
            this.toolButtonAddColumn.Click += new System.EventHandler(this.toolButtonAddColumn_Click);
            // 
            // toolButtonDelColumn
            // 
            this.toolButtonDelColumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolButtonDelColumn.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonDelColumn.Image")));
            this.toolButtonDelColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonDelColumn.Name = "toolButtonDelColumn";
            this.toolButtonDelColumn.Size = new System.Drawing.Size(23, 22);
            this.toolButtonDelColumn.Text = "toolStripButton1";
            this.toolButtonDelColumn.ToolTipText = "Видалити колонку";
            this.toolButtonDelColumn.Click += new System.EventHandler(this.toolButtonDelColumn_Click);
            // 
            // toolButtonAddrColumn
            // 
            this.toolButtonAddrColumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolButtonAddrColumn.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonAddrColumn.Image")));
            this.toolButtonAddrColumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAddrColumn.Name = "toolButtonAddrColumn";
            this.toolButtonAddrColumn.Size = new System.Drawing.Size(23, 22);
            this.toolButtonAddrColumn.Text = "toolStripButton1";
            this.toolButtonAddrColumn.ToolTipText = "Вибрати колонку з адресою";
            this.toolButtonAddrColumn.Click += new System.EventHandler(this.toolButtonAddrColumn_Click);
            // 
            // toolEditAddressColumn
            // 
            this.toolEditAddressColumn.Name = "toolEditAddressColumn";
            this.toolEditAddressColumn.ReadOnly = true;
            this.toolEditAddressColumn.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAllowInpreciseAddrMatch,
            this.menuItemDisplayFoundAddress});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // menuItemAllowInpreciseAddrMatch
            // 
            this.menuItemAllowInpreciseAddrMatch.Checked = true;
            this.menuItemAllowInpreciseAddrMatch.CheckOnClick = true;
            this.menuItemAllowInpreciseAddrMatch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemAllowInpreciseAddrMatch.Name = "menuItemAllowInpreciseAddrMatch";
            this.menuItemAllowInpreciseAddrMatch.Size = new System.Drawing.Size(255, 22);
            this.menuItemAllowInpreciseAddrMatch.Text = "Дозволити пошук схожих адрес";
            // 
            // menuItemDisplayFoundAddress
            // 
            this.menuItemDisplayFoundAddress.Checked = true;
            this.menuItemDisplayFoundAddress.CheckOnClick = true;
            this.menuItemDisplayFoundAddress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemDisplayFoundAddress.Name = "menuItemDisplayFoundAddress";
            this.menuItemDisplayFoundAddress.Size = new System.Drawing.Size(273, 22);
            this.menuItemDisplayFoundAddress.Text = "Відображати знайдену в 1НФ адресу";
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExit.Location = new System.Drawing.Point(848, 543);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(104, 25);
            this.buttonExit.TabIndex = 2;
            this.buttonExit.Text = "Вихід";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // openInputFileDialog
            // 
            this.openInputFileDialog.DefaultExt = "doc";
            this.openInputFileDialog.Filter = "Microsoft Word 2003 files|*.doc|Microsoft Word 2007 files|*.docx|Microsoft Excel " +
                "2003 files|*.xls|Microsoft Excel 2007 files|*.xlsx";
            this.openInputFileDialog.Title = "Виберіть файл для обробки";
            // 
            // saveOutputFileDialog
            // 
            this.saveOutputFileDialog.Filter = "Microsoft Word 2007 files|*.docx|Microsoft Excel 2007 files|*.xlsx";
            this.saveOutputFileDialog.Title = "Зберегти таблицю в файл";
            // 
            // readingProgressBar
            // 
            this.readingProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.readingProgressBar.Location = new System.Drawing.Point(14, 545);
            this.readingProgressBar.Name = "readingProgressBar";
            this.readingProgressBar.Size = new System.Drawing.Size(818, 20);
            this.readingProgressBar.Step = 1;
            this.readingProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.readingProgressBar.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 577);
            this.Controls.Add(this.readingProgressBar);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.gridAddressTable);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Пошук за Адресами";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridAddressTable)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridAddressTable;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolButtonOpen;
        private System.Windows.Forms.ToolStripButton toolButtonWrite;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.OpenFileDialog openInputFileDialog;
        private System.Windows.Forms.SaveFileDialog saveOutputFileDialog;
        private System.Windows.Forms.ProgressBar readingProgressBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolButtonAddColumn;
        private System.Windows.Forms.ToolStripButton toolButtonDelColumn;
        private System.Windows.Forms.ToolStripButton toolButtonAddrColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox toolEditAddressColumn;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem menuItemAllowInpreciseAddrMatch;
        private System.Windows.Forms.ToolStripMenuItem menuItemDisplayFoundAddress;
    }
}

