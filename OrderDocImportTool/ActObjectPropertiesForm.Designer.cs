namespace GUKV.DataMigration
{
    partial class ActObjectPropertiesForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActObjectPropertiesForm));
            this.editAddressNJF = new System.Windows.Forms.TextBox();
            this.editAddress1NF = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPickObject = new System.Windows.Forms.Button();
            this.listBalansTransfers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageListTransferStates = new System.Windows.Forms.ImageList(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.editObjectName = new System.Windows.Forms.TextBox();
            this.editObjectSquare = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.editTransferSqr = new System.Windows.Forms.TextBox();
            this.editTransferOrgFromNJF = new System.Windows.Forms.TextBox();
            this.editTransferOrgFrom1NF = new System.Windows.Forms.TextBox();
            this.editTransferOrgToNJF = new System.Windows.Forms.TextBox();
            this.editTransferOrgTo1NF = new System.Windows.Forms.TextBox();
            this.editBalansObject1NF = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnPickOrgFrom = new System.Windows.Forms.Button();
            this.btnPickOrgTo = new System.Windows.Forms.Button();
            this.btnPickBalansObject = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioDestroy = new System.Windows.Forms.RadioButton();
            this.radioCreate = new System.Windows.Forms.RadioButton();
            this.radioTransfer = new System.Windows.Forms.RadioButton();
            this.labelRequiredSqr = new System.Windows.Forms.Label();
            this.labelRequiredFrom = new System.Windows.Forms.Label();
            this.labelRequiredTo = new System.Windows.Forms.Label();
            this.labelRequiredBalansObj = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnDelTransfer = new System.Windows.Forms.Button();
            this.btnAddTransfer = new System.Windows.Forms.Button();
            this.btnAcceptSquare = new System.Windows.Forms.Button();
            this.btnApplyTotalSquare = new System.Windows.Forms.Button();
            this.btnNewObject = new System.Windows.Forms.Button();
            this.btnNewOrgFrom = new System.Windows.Forms.Button();
            this.btnNewOrgTo = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // editAddressNJF
            // 
            this.editAddressNJF.Enabled = false;
            this.editAddressNJF.Location = new System.Drawing.Point(219, 12);
            this.editAddressNJF.Name = "editAddressNJF";
            this.editAddressNJF.Size = new System.Drawing.Size(446, 20);
            this.editAddressNJF.TabIndex = 0;
            // 
            // editAddress1NF
            // 
            this.editAddress1NF.Enabled = false;
            this.editAddress1NF.Location = new System.Drawing.Point(219, 38);
            this.editAddress1NF.Name = "editAddress1NF";
            this.editAddress1NF.Size = new System.Drawing.Size(446, 20);
            this.editAddress1NF.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Адреса Об\'єкту в БД \'Розпорядження\'";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Адреса Об\'єкту в БД \'1НФ\'";
            // 
            // btnPickObject
            // 
            this.btnPickObject.Image = ((System.Drawing.Image)(resources.GetObject("btnPickObject.Image")));
            this.btnPickObject.Location = new System.Drawing.Point(671, 38);
            this.btnPickObject.Name = "btnPickObject";
            this.btnPickObject.Size = new System.Drawing.Size(22, 20);
            this.btnPickObject.TabIndex = 74;
            this.btnPickObject.UseVisualStyleBackColor = true;
            this.btnPickObject.Click += new System.EventHandler(this.btnPickObject_Click);
            // 
            // listBalansTransfers
            // 
            this.listBalansTransfers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listBalansTransfers.FullRowSelect = true;
            this.listBalansTransfers.GridLines = true;
            this.listBalansTransfers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listBalansTransfers.HideSelection = false;
            this.listBalansTransfers.Location = new System.Drawing.Point(15, 142);
            this.listBalansTransfers.MultiSelect = false;
            this.listBalansTransfers.Name = "listBalansTransfers";
            this.listBalansTransfers.ShowItemToolTips = true;
            this.listBalansTransfers.Size = new System.Drawing.Size(706, 97);
            this.listBalansTransfers.SmallImageList = this.imageListTransferStates;
            this.listBalansTransfers.StateImageList = this.imageListTransferStates;
            this.listBalansTransfers.TabIndex = 75;
            this.listBalansTransfers.UseCompatibleStateImageBehavior = false;
            this.listBalansTransfers.View = System.Windows.Forms.View.Details;
            this.listBalansTransfers.SelectedIndexChanged += new System.EventHandler(this.listBalansTransfers_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 52;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Тип передачі";
            this.columnHeader2.Width = 130;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Площа, кв.м.";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Від кого";
            this.columnHeader4.Width = 160;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Кому";
            this.columnHeader5.Width = 160;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Об\'єкт в БД \'1НФ\'";
            this.columnHeader6.Width = 280;
            // 
            // imageListTransferStates
            // 
            this.imageListTransferStates.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTransferStates.ImageStream")));
            this.imageListTransferStates.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTransferStates.Images.SetKeyName(0, "Accept_12_12.png");
            this.imageListTransferStates.Images.SetKeyName(1, "Block_12_12.png");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(253, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Передачі з балансу на баланс по даному об\'єкту";
            // 
            // editObjectName
            // 
            this.editObjectName.Enabled = false;
            this.editObjectName.Location = new System.Drawing.Point(219, 64);
            this.editObjectName.Name = "editObjectName";
            this.editObjectName.Size = new System.Drawing.Size(446, 20);
            this.editObjectName.TabIndex = 1;
            // 
            // editObjectSquare
            // 
            this.editObjectSquare.Location = new System.Drawing.Point(219, 90);
            this.editObjectSquare.Name = "editObjectSquare";
            this.editObjectSquare.Size = new System.Drawing.Size(120, 20);
            this.editObjectSquare.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Назва Об\'єкту";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Площа Об\'єкту";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(373, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "кв.м.";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(646, 457);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 76;
            this.btnCancel.Text = "Відмінити";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(565, 457);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 77;
            this.btnOK.Text = "Зберегти";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // editTransferSqr
            // 
            this.editTransferSqr.Enabled = false;
            this.editTransferSqr.Location = new System.Drawing.Point(219, 300);
            this.editTransferSqr.Name = "editTransferSqr";
            this.editTransferSqr.Size = new System.Drawing.Size(120, 20);
            this.editTransferSqr.TabIndex = 78;
            // 
            // editTransferOrgFromNJF
            // 
            this.editTransferOrgFromNJF.Enabled = false;
            this.editTransferOrgFromNJF.Location = new System.Drawing.Point(219, 326);
            this.editTransferOrgFromNJF.Name = "editTransferOrgFromNJF";
            this.editTransferOrgFromNJF.Size = new System.Drawing.Size(421, 20);
            this.editTransferOrgFromNJF.TabIndex = 79;
            // 
            // editTransferOrgFrom1NF
            // 
            this.editTransferOrgFrom1NF.Enabled = false;
            this.editTransferOrgFrom1NF.Location = new System.Drawing.Point(219, 352);
            this.editTransferOrgFrom1NF.Name = "editTransferOrgFrom1NF";
            this.editTransferOrgFrom1NF.Size = new System.Drawing.Size(421, 20);
            this.editTransferOrgFrom1NF.TabIndex = 80;
            // 
            // editTransferOrgToNJF
            // 
            this.editTransferOrgToNJF.Enabled = false;
            this.editTransferOrgToNJF.Location = new System.Drawing.Point(219, 378);
            this.editTransferOrgToNJF.Name = "editTransferOrgToNJF";
            this.editTransferOrgToNJF.Size = new System.Drawing.Size(421, 20);
            this.editTransferOrgToNJF.TabIndex = 81;
            // 
            // editTransferOrgTo1NF
            // 
            this.editTransferOrgTo1NF.Enabled = false;
            this.editTransferOrgTo1NF.Location = new System.Drawing.Point(219, 404);
            this.editTransferOrgTo1NF.Name = "editTransferOrgTo1NF";
            this.editTransferOrgTo1NF.Size = new System.Drawing.Size(421, 20);
            this.editTransferOrgTo1NF.TabIndex = 82;
            // 
            // editBalansObject1NF
            // 
            this.editBalansObject1NF.Enabled = false;
            this.editBalansObject1NF.Location = new System.Drawing.Point(219, 430);
            this.editBalansObject1NF.Name = "editBalansObject1NF";
            this.editBalansObject1NF.Size = new System.Drawing.Size(421, 20);
            this.editBalansObject1NF.TabIndex = 83;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 303);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Площа, що передається";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(373, 303);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "кв.м.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 329);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(161, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Від кого (БД \'Розпорядження\')";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 355);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Від кого (БД \'1НФ\')";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 381);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(146, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Кому (БД \'Розпорядження\')";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 407);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(90, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Кому (БД \'1НФ\')";
            // 
            // btnPickOrgFrom
            // 
            this.btnPickOrgFrom.Enabled = false;
            this.btnPickOrgFrom.Image = ((System.Drawing.Image)(resources.GetObject("btnPickOrgFrom.Image")));
            this.btnPickOrgFrom.Location = new System.Drawing.Point(672, 352);
            this.btnPickOrgFrom.Name = "btnPickOrgFrom";
            this.btnPickOrgFrom.Size = new System.Drawing.Size(22, 20);
            this.btnPickOrgFrom.TabIndex = 74;
            this.btnPickOrgFrom.UseVisualStyleBackColor = true;
            this.btnPickOrgFrom.Click += new System.EventHandler(this.btnPickOrgFrom_Click);
            // 
            // btnPickOrgTo
            // 
            this.btnPickOrgTo.Enabled = false;
            this.btnPickOrgTo.Image = ((System.Drawing.Image)(resources.GetObject("btnPickOrgTo.Image")));
            this.btnPickOrgTo.Location = new System.Drawing.Point(672, 404);
            this.btnPickOrgTo.Name = "btnPickOrgTo";
            this.btnPickOrgTo.Size = new System.Drawing.Size(22, 20);
            this.btnPickOrgTo.TabIndex = 74;
            this.btnPickOrgTo.UseVisualStyleBackColor = true;
            this.btnPickOrgTo.Click += new System.EventHandler(this.btnPickOrgTo_Click);
            // 
            // btnPickBalansObject
            // 
            this.btnPickBalansObject.Enabled = false;
            this.btnPickBalansObject.Image = ((System.Drawing.Image)(resources.GetObject("btnPickBalansObject.Image")));
            this.btnPickBalansObject.Location = new System.Drawing.Point(672, 430);
            this.btnPickBalansObject.Name = "btnPickBalansObject";
            this.btnPickBalansObject.Size = new System.Drawing.Size(22, 20);
            this.btnPickBalansObject.TabIndex = 74;
            this.btnPickBalansObject.UseVisualStyleBackColor = true;
            this.btnPickBalansObject.Click += new System.EventHandler(this.btnPickBalansObject_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 433);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(153, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Об\'єкт на балансі (БД \'1НФ\')";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioDestroy);
            this.groupBox1.Controls.Add(this.radioCreate);
            this.groupBox1.Controls.Add(this.radioTransfer);
            this.groupBox1.Location = new System.Drawing.Point(15, 245);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(706, 49);
            this.groupBox1.TabIndex = 84;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тип передачі";
            // 
            // radioDestroy
            // 
            this.radioDestroy.AutoSize = true;
            this.radioDestroy.Enabled = false;
            this.radioDestroy.Location = new System.Drawing.Point(569, 19);
            this.radioDestroy.Name = "radioDestroy";
            this.radioDestroy.Size = new System.Drawing.Size(121, 17);
            this.radioDestroy.TabIndex = 2;
            this.radioDestroy.TabStop = true;
            this.radioDestroy.Text = "Відчуження об\'єкту";
            this.radioDestroy.UseVisualStyleBackColor = true;
            this.radioDestroy.CheckedChanged += new System.EventHandler(this.radioDestroy_CheckedChanged);
            // 
            // radioCreate
            // 
            this.radioCreate.AutoSize = true;
            this.radioCreate.Enabled = false;
            this.radioCreate.Location = new System.Drawing.Point(320, 19);
            this.radioCreate.Name = "radioCreate";
            this.radioCreate.Size = new System.Drawing.Size(210, 17);
            this.radioCreate.TabIndex = 1;
            this.radioCreate.TabStop = true;
            this.radioCreate.Text = "Прийняття на баланс нового об\'єкту";
            this.radioCreate.UseVisualStyleBackColor = true;
            this.radioCreate.CheckedChanged += new System.EventHandler(this.radioCreate_CheckedChanged);
            // 
            // radioTransfer
            // 
            this.radioTransfer.AutoSize = true;
            this.radioTransfer.Enabled = false;
            this.radioTransfer.Location = new System.Drawing.Point(17, 19);
            this.radioTransfer.Name = "radioTransfer";
            this.radioTransfer.Size = new System.Drawing.Size(272, 17);
            this.radioTransfer.TabIndex = 0;
            this.radioTransfer.TabStop = true;
            this.radioTransfer.Text = "Передача існуючого об\'єкту з балансу на баланс";
            this.radioTransfer.UseVisualStyleBackColor = true;
            this.radioTransfer.CheckedChanged += new System.EventHandler(this.radioTransfer_CheckedChanged);
            // 
            // labelRequiredSqr
            // 
            this.labelRequiredSqr.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelRequiredSqr.ForeColor = System.Drawing.Color.Red;
            this.labelRequiredSqr.Location = new System.Drawing.Point(406, 302);
            this.labelRequiredSqr.Name = "labelRequiredSqr";
            this.labelRequiredSqr.Size = new System.Drawing.Size(18, 18);
            this.labelRequiredSqr.TabIndex = 85;
            this.labelRequiredSqr.Text = "*";
            this.labelRequiredSqr.Visible = false;
            // 
            // labelRequiredFrom
            // 
            this.labelRequiredFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelRequiredFrom.ForeColor = System.Drawing.Color.Red;
            this.labelRequiredFrom.Location = new System.Drawing.Point(647, 354);
            this.labelRequiredFrom.Name = "labelRequiredFrom";
            this.labelRequiredFrom.Size = new System.Drawing.Size(18, 18);
            this.labelRequiredFrom.TabIndex = 85;
            this.labelRequiredFrom.Text = "*";
            this.labelRequiredFrom.Visible = false;
            // 
            // labelRequiredTo
            // 
            this.labelRequiredTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelRequiredTo.ForeColor = System.Drawing.Color.Red;
            this.labelRequiredTo.Location = new System.Drawing.Point(647, 406);
            this.labelRequiredTo.Name = "labelRequiredTo";
            this.labelRequiredTo.Size = new System.Drawing.Size(18, 18);
            this.labelRequiredTo.TabIndex = 85;
            this.labelRequiredTo.Text = "*";
            this.labelRequiredTo.Visible = false;
            // 
            // labelRequiredBalansObj
            // 
            this.labelRequiredBalansObj.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelRequiredBalansObj.ForeColor = System.Drawing.Color.Red;
            this.labelRequiredBalansObj.Location = new System.Drawing.Point(647, 432);
            this.labelRequiredBalansObj.Name = "labelRequiredBalansObj";
            this.labelRequiredBalansObj.Size = new System.Drawing.Size(18, 18);
            this.labelRequiredBalansObj.TabIndex = 85;
            this.labelRequiredBalansObj.Text = "*";
            this.labelRequiredBalansObj.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(12, 465);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(492, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Поля, позначені зірочкою, є обов\'язковими для заповнення для обраного типу переда" +
                "чі об\'єкту";
            // 
            // btnDelTransfer
            // 
            this.btnDelTransfer.Image = ((System.Drawing.Image)(resources.GetObject("btnDelTransfer.Image")));
            this.btnDelTransfer.Location = new System.Drawing.Point(699, 116);
            this.btnDelTransfer.Name = "btnDelTransfer";
            this.btnDelTransfer.Size = new System.Drawing.Size(22, 20);
            this.btnDelTransfer.TabIndex = 74;
            this.btnDelTransfer.UseVisualStyleBackColor = true;
            this.btnDelTransfer.Click += new System.EventHandler(this.btnDelTransfer_Click);
            // 
            // btnAddTransfer
            // 
            this.btnAddTransfer.Image = ((System.Drawing.Image)(resources.GetObject("btnAddTransfer.Image")));
            this.btnAddTransfer.Location = new System.Drawing.Point(671, 116);
            this.btnAddTransfer.Name = "btnAddTransfer";
            this.btnAddTransfer.Size = new System.Drawing.Size(22, 20);
            this.btnAddTransfer.TabIndex = 74;
            this.btnAddTransfer.UseVisualStyleBackColor = true;
            this.btnAddTransfer.Click += new System.EventHandler(this.btnAddTransfer_Click);
            // 
            // btnAcceptSquare
            // 
            this.btnAcceptSquare.Enabled = false;
            this.btnAcceptSquare.Image = ((System.Drawing.Image)(resources.GetObject("btnAcceptSquare.Image")));
            this.btnAcceptSquare.Location = new System.Drawing.Point(345, 300);
            this.btnAcceptSquare.Name = "btnAcceptSquare";
            this.btnAcceptSquare.Size = new System.Drawing.Size(22, 20);
            this.btnAcceptSquare.TabIndex = 74;
            this.btnAcceptSquare.UseVisualStyleBackColor = true;
            this.btnAcceptSquare.Click += new System.EventHandler(this.btnAcceptSquare_Click);
            // 
            // btnApplyTotalSquare
            // 
            this.btnApplyTotalSquare.Image = ((System.Drawing.Image)(resources.GetObject("btnApplyTotalSquare.Image")));
            this.btnApplyTotalSquare.Location = new System.Drawing.Point(345, 90);
            this.btnApplyTotalSquare.Name = "btnApplyTotalSquare";
            this.btnApplyTotalSquare.Size = new System.Drawing.Size(22, 20);
            this.btnApplyTotalSquare.TabIndex = 74;
            this.btnApplyTotalSquare.UseVisualStyleBackColor = true;
            this.btnApplyTotalSquare.Click += new System.EventHandler(this.btnApplyTotalSquare_Click);
            // 
            // btnNewObject
            // 
            this.btnNewObject.Image = ((System.Drawing.Image)(resources.GetObject("btnNewObject.Image")));
            this.btnNewObject.Location = new System.Drawing.Point(699, 38);
            this.btnNewObject.Name = "btnNewObject";
            this.btnNewObject.Size = new System.Drawing.Size(22, 20);
            this.btnNewObject.TabIndex = 86;
            this.btnNewObject.UseVisualStyleBackColor = true;
            this.btnNewObject.Click += new System.EventHandler(this.btnNewObject_Click);
            // 
            // btnNewOrgFrom
            // 
            this.btnNewOrgFrom.Enabled = false;
            this.btnNewOrgFrom.Image = ((System.Drawing.Image)(resources.GetObject("btnNewOrgFrom.Image")));
            this.btnNewOrgFrom.Location = new System.Drawing.Point(699, 352);
            this.btnNewOrgFrom.Name = "btnNewOrgFrom";
            this.btnNewOrgFrom.Size = new System.Drawing.Size(22, 20);
            this.btnNewOrgFrom.TabIndex = 87;
            this.btnNewOrgFrom.UseVisualStyleBackColor = true;
            this.btnNewOrgFrom.Click += new System.EventHandler(this.btnNewOrgFrom_Click);
            // 
            // btnNewOrgTo
            // 
            this.btnNewOrgTo.Enabled = false;
            this.btnNewOrgTo.Image = ((System.Drawing.Image)(resources.GetObject("btnNewOrgTo.Image")));
            this.btnNewOrgTo.Location = new System.Drawing.Point(699, 404);
            this.btnNewOrgTo.Name = "btnNewOrgTo";
            this.btnNewOrgTo.Size = new System.Drawing.Size(22, 20);
            this.btnNewOrgTo.TabIndex = 88;
            this.btnNewOrgTo.UseVisualStyleBackColor = true;
            this.btnNewOrgTo.Click += new System.EventHandler(this.btnNewOrgTo_Click);
            // 
            // ActObjectPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 490);
            this.Controls.Add(this.btnNewOrgTo);
            this.Controls.Add(this.btnNewOrgFrom);
            this.Controls.Add(this.btnNewObject);
            this.Controls.Add(this.labelRequiredBalansObj);
            this.Controls.Add(this.labelRequiredTo);
            this.Controls.Add(this.labelRequiredFrom);
            this.Controls.Add(this.labelRequiredSqr);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.editBalansObject1NF);
            this.Controls.Add(this.editTransferOrgTo1NF);
            this.Controls.Add(this.editTransferOrgToNJF);
            this.Controls.Add(this.editTransferOrgFrom1NF);
            this.Controls.Add(this.editTransferOrgFromNJF);
            this.Controls.Add(this.editTransferSqr);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.listBalansTransfers);
            this.Controls.Add(this.btnPickBalansObject);
            this.Controls.Add(this.btnPickOrgTo);
            this.Controls.Add(this.btnAddTransfer);
            this.Controls.Add(this.btnDelTransfer);
            this.Controls.Add(this.btnApplyTotalSquare);
            this.Controls.Add(this.btnAcceptSquare);
            this.Controls.Add(this.btnPickOrgFrom);
            this.Controls.Add(this.btnPickObject);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editObjectSquare);
            this.Controls.Add(this.editObjectName);
            this.Controls.Add(this.editAddress1NF);
            this.Controls.Add(this.editAddressNJF);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActObjectPropertiesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Властивості Об\'єкту";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox editAddressNJF;
        private System.Windows.Forms.TextBox editAddress1NF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPickObject;
        private System.Windows.Forms.ListView listBalansTransfers;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editObjectName;
        private System.Windows.Forms.TextBox editObjectSquare;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox editTransferSqr;
        private System.Windows.Forms.TextBox editTransferOrgFromNJF;
        private System.Windows.Forms.TextBox editTransferOrgFrom1NF;
        private System.Windows.Forms.TextBox editTransferOrgToNJF;
        private System.Windows.Forms.TextBox editTransferOrgTo1NF;
        private System.Windows.Forms.TextBox editBalansObject1NF;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnPickOrgFrom;
        private System.Windows.Forms.Button btnPickOrgTo;
        private System.Windows.Forms.Button btnPickBalansObject;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioDestroy;
        private System.Windows.Forms.RadioButton radioCreate;
        private System.Windows.Forms.RadioButton radioTransfer;
        private System.Windows.Forms.Label labelRequiredSqr;
        private System.Windows.Forms.Label labelRequiredFrom;
        private System.Windows.Forms.Label labelRequiredTo;
        private System.Windows.Forms.Label labelRequiredBalansObj;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btnDelTransfer;
        private System.Windows.Forms.Button btnAddTransfer;
        private System.Windows.Forms.Button btnAcceptSquare;
        private System.Windows.Forms.ImageList imageListTransferStates;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btnApplyTotalSquare;
        private System.Windows.Forms.Button btnNewObject;
        private System.Windows.Forms.Button btnNewOrgFrom;
        private System.Windows.Forms.Button btnNewOrgTo;
    }
}