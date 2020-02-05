namespace dedupe
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSaveGroup = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lvChildGroups = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lvStreetsDuplicates = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.maxDistance = new System.Windows.Forms.NumericUpDown();
            this.lvStreets = new System.Windows.Forms.ListView();
            this.GroupId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GroupName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CntInGroup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSaveBuildingsGroup = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lvBuildChildGroups = new System.Windows.Forms.ListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lvBuildingsDuplicates = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.maxDistanceBuildings = new System.Windows.Forms.NumericUpDown();
            this.lvBuildings = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BuildNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button3 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxDistance)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxDistanceBuildings)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1310, 758);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.lvStreetsDuplicates);
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.maxDistance);
            this.tabPage1.Controls.Add(this.lvStreets);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1302, 732);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Улицы";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(274, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(147, 37);
            this.button2.TabIndex = 13;
            this.button2.Text = "Выгрузить скрипт";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.UploadStreetdedupeScript_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSaveGroup);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lvChildGroups);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(542, 461);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(476, 258);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Группа дубликатов";
            // 
            // btnSaveGroup
            // 
            this.btnSaveGroup.Enabled = false;
            this.btnSaveGroup.Location = new System.Drawing.Point(325, 214);
            this.btnSaveGroup.Name = "btnSaveGroup";
            this.btnSaveGroup.Size = new System.Drawing.Size(129, 23);
            this.btnSaveGroup.TabIndex = 4;
            this.btnSaveGroup.Text = "Сохранить группу";
            this.btnSaveGroup.UseVisualStyleBackColor = true;
            this.btnSaveGroup.Click += new System.EventHandler(this.btnSaveStreetGroup_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(51, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 15);
            this.label3.TabIndex = 3;
            // 
            // lvChildGroups
            // 
            this.lvChildGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvChildGroups.Location = new System.Drawing.Point(25, 107);
            this.lvChildGroups.Name = "lvChildGroups";
            this.lvChildGroups.Size = new System.Drawing.Size(429, 101);
            this.lvChildGroups.TabIndex = 2;
            this.lvChildGroups.UseCompatibleStateImageBehavior = false;
            this.lvChildGroups.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Название улицы";
            this.columnHeader2.Width = 250;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Список дубликатов:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Главная запись:";
            // 
            // lvStreetsDuplicates
            // 
            this.lvStreetsDuplicates.CheckBoxes = true;
            this.lvStreetsDuplicates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.Name});
            this.lvStreetsDuplicates.FullRowSelect = true;
            this.lvStreetsDuplicates.GridLines = true;
            this.lvStreetsDuplicates.Location = new System.Drawing.Point(542, 52);
            this.lvStreetsDuplicates.Name = "lvStreetsDuplicates";
            this.lvStreetsDuplicates.Size = new System.Drawing.Size(476, 390);
            this.lvStreetsDuplicates.TabIndex = 11;
            this.lvStreetsDuplicates.UseCompatibleStateImageBehavior = false;
            this.lvStreetsDuplicates.View = System.Windows.Forms.View.Details;
            this.lvStreetsDuplicates.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvStreetsDuplicates_SelectedIndexChanged);
            this.lvStreetsDuplicates.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvStreetsDuplicates_SelectedIndexChanged);
            this.lvStreetsDuplicates.Click += new System.EventHandler(this.lvStreetsDuplicates_SelectedIndexChanged);
            // 
            // id
            // 
            this.id.Text = "Id улицы";
            // 
            // Name
            // 
            this.Name.Text = "Название улицы";
            this.Name.Width = 300;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(14, 6);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(84, 13);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Кол-во отличий";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // maxDistance
            // 
            this.maxDistance.Location = new System.Drawing.Point(14, 26);
            this.maxDistance.Name = "maxDistance";
            this.maxDistance.Size = new System.Drawing.Size(120, 20);
            this.maxDistance.TabIndex = 9;
            // 
            // lvStreets
            // 
            this.lvStreets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GroupId,
            this.GroupName,
            this.CntInGroup});
            this.lvStreets.FullRowSelect = true;
            this.lvStreets.GridLines = true;
            this.lvStreets.HideSelection = false;
            this.lvStreets.Location = new System.Drawing.Point(14, 52);
            this.lvStreets.Name = "lvStreets";
            this.lvStreets.Size = new System.Drawing.Size(499, 677);
            this.lvStreets.TabIndex = 8;
            this.lvStreets.UseCompatibleStateImageBehavior = false;
            this.lvStreets.View = System.Windows.Forms.View.Details;
            this.lvStreets.Click += new System.EventHandler(this.lvStreets_Click);
            // 
            // GroupId
            // 
            this.GroupId.Text = "Id группы";
            // 
            // GroupName
            // 
            this.GroupName.Text = "Название улицы";
            this.GroupName.Width = 292;
            // 
            // CntInGroup
            // 
            this.CntInGroup.Text = "Кол-во дубликатов";
            this.CntInGroup.Width = 116;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(140, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 37);
            this.button1.TabIndex = 7;
            this.button1.Text = "Загрузить список \r\nдубликатов улиц";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnLoadStreetGroups);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button8);
            this.tabPage2.Controls.Add(this.button7);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.button6);
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.lvBuildingsDuplicates);
            this.tabPage2.Controls.Add(this.linkLabel2);
            this.tabPage2.Controls.Add(this.maxDistanceBuildings);
            this.tabPage2.Controls.Add(this.lvBuildings);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1302, 732);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Здания";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(342, 10);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(109, 36);
            this.button7.TabIndex = 21;
            this.button7.Text = "Скрипт по всем!";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.UploadBuilddedupeAllScripts);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(457, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 13);
            this.label7.TabIndex = 20;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(691, 16);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(155, 23);
            this.button6.TabIndex = 19;
            this.button6.Text = "Просмотреть отмеченные";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(530, 16);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(155, 23);
            this.button5.TabIndex = 18;
            this.button5.Text = "Просмотреть группу";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(266, 9);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(70, 37);
            this.button4.TabIndex = 17;
            this.button4.Text = "Выгрузить скрипт";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.UploadBuilddedupeScript_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSaveBuildingsGroup);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lvBuildChildGroups);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(530, 471);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(476, 258);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Группа дубликатов";
            // 
            // btnSaveBuildingsGroup
            // 
            this.btnSaveBuildingsGroup.Enabled = false;
            this.btnSaveBuildingsGroup.Location = new System.Drawing.Point(325, 214);
            this.btnSaveBuildingsGroup.Name = "btnSaveBuildingsGroup";
            this.btnSaveBuildingsGroup.Size = new System.Drawing.Size(129, 23);
            this.btnSaveBuildingsGroup.TabIndex = 4;
            this.btnSaveBuildingsGroup.Text = "Сохранить группу";
            this.btnSaveBuildingsGroup.UseVisualStyleBackColor = true;
            this.btnSaveBuildingsGroup.Click += new System.EventHandler(this.btnSaveBuildingGroup_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(51, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 15);
            this.label4.TabIndex = 3;
            // 
            // lvBuildChildGroups
            // 
            this.lvBuildChildGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader11});
            this.lvBuildChildGroups.Location = new System.Drawing.Point(25, 107);
            this.lvBuildChildGroups.Name = "lvBuildChildGroups";
            this.lvBuildChildGroups.Size = new System.Drawing.Size(429, 101);
            this.lvBuildChildGroups.TabIndex = 2;
            this.lvBuildChildGroups.UseCompatibleStateImageBehavior = false;
            this.lvBuildChildGroups.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Id";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Название улицы";
            this.columnHeader9.Width = 250;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "№ дома";
            this.columnHeader11.Width = 100;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Список дубликатов:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Главная запись:";
            // 
            // lvBuildingsDuplicates
            // 
            this.lvBuildingsDuplicates.CheckBoxes = true;
            this.lvBuildingsDuplicates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader10,
            this.columnHeader12});
            this.lvBuildingsDuplicates.FullRowSelect = true;
            this.lvBuildingsDuplicates.GridLines = true;
            this.lvBuildingsDuplicates.Location = new System.Drawing.Point(530, 52);
            this.lvBuildingsDuplicates.Name = "lvBuildingsDuplicates";
            this.lvBuildingsDuplicates.Size = new System.Drawing.Size(748, 390);
            this.lvBuildingsDuplicates.TabIndex = 15;
            this.lvBuildingsDuplicates.UseCompatibleStateImageBehavior = false;
            this.lvBuildingsDuplicates.View = System.Windows.Forms.View.Details;
            this.lvBuildingsDuplicates.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvBuildingsDuplicates_SelectedIndexChanged);
            this.lvBuildingsDuplicates.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvBuildingsDuplicates_SelectedIndexChanged);
            this.lvBuildingsDuplicates.Click += new System.EventHandler(this.lvBuildingsDuplicates_SelectedIndexChanged);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Id строения";
            this.columnHeader6.Width = 84;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Название улицы";
            this.columnHeader7.Width = 300;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "№ дома";
            this.columnHeader10.Width = 100;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Норм. адрес";
            this.columnHeader12.Width = 275;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(6, 6);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(84, 13);
            this.linkLabel2.TabIndex = 14;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Кол-во отличий";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // maxDistanceBuildings
            // 
            this.maxDistanceBuildings.Location = new System.Drawing.Point(6, 26);
            this.maxDistanceBuildings.Name = "maxDistanceBuildings";
            this.maxDistanceBuildings.Size = new System.Drawing.Size(120, 20);
            this.maxDistanceBuildings.TabIndex = 13;
            // 
            // lvBuildings
            // 
            this.lvBuildings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.BuildNumber,
            this.columnHeader5});
            this.lvBuildings.FullRowSelect = true;
            this.lvBuildings.GridLines = true;
            this.lvBuildings.HideSelection = false;
            this.lvBuildings.Location = new System.Drawing.Point(6, 52);
            this.lvBuildings.Name = "lvBuildings";
            this.lvBuildings.Size = new System.Drawing.Size(499, 677);
            this.lvBuildings.TabIndex = 12;
            this.lvBuildings.UseCompatibleStateImageBehavior = false;
            this.lvBuildings.View = System.Windows.Forms.View.Details;
            this.lvBuildings.SelectedIndexChanged += new System.EventHandler(this.lvBuildings_SelectedIndexChanged_1);
            this.lvBuildings.Click += new System.EventHandler(this.lvBuildings_Click);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Id группы";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Название улицы";
            this.columnHeader4.Width = 200;
            // 
            // BuildNumber
            // 
            this.BuildNumber.Text = "№ дома";
            this.BuildNumber.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Кол-во дубликатов";
            this.columnHeader5.Width = 116;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(132, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(128, 37);
            this.button3.TabIndex = 11;
            this.button3.Text = "Загрузить список \r\nдубликатов зданий";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnLoadBuildingsGroups);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(852, 16);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(118, 23);
            this.button8.TabIndex = 22;
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1419, 782);
            this.Controls.Add(this.tabControl1);
            
            this.Text = "Дедубликатор улиц и зданий";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxDistance)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxDistanceBuildings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSaveGroup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView lvChildGroups;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvStreetsDuplicates;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader Name;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.NumericUpDown maxDistance;
        private System.Windows.Forms.ListView lvStreets;
        private System.Windows.Forms.ColumnHeader GroupId;
        private System.Windows.Forms.ColumnHeader GroupName;
        private System.Windows.Forms.ColumnHeader CntInGroup;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.NumericUpDown maxDistanceBuildings;
        private System.Windows.Forms.ListView lvBuildings;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ColumnHeader BuildNumber;
        private System.Windows.Forms.ListView lvBuildingsDuplicates;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSaveBuildingsGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lvBuildChildGroups;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;

    }
}