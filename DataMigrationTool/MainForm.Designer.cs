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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonMigrate = new System.Windows.Forms.Button();
            this.tabControlPreferences = new System.Windows.Forms.TabControl();
            this.tabPage1NF = new System.Windows.Forms.TabPage();
            this.textBoxPassword1NF = new System.Windows.Forms.TextBox();
            this.textBoxUserName1NF = new System.Windows.Forms.TextBox();
            this.textBoxDatabaseName1NF = new System.Windows.Forms.TextBox();
            this.textBoxServerName1NF = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageBalans = new System.Windows.Forms.TabPage();
            this.textBoxPasswordBalans = new System.Windows.Forms.TextBox();
            this.textBoxUserNameBalans = new System.Windows.Forms.TextBox();
            this.textBoxDatabaseNameBalans = new System.Windows.Forms.TextBox();
            this.textBoxServerNameBalans = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPagePrivatization = new System.Windows.Forms.TabPage();
            this.textBoxPasswordPriv = new System.Windows.Forms.TextBox();
            this.textBoxUserNamePriv = new System.Windows.Forms.TextBox();
            this.textBoxDatabaseNamePriv = new System.Windows.Forms.TextBox();
            this.textBoxServerNamePriv = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tabPageNJF = new System.Windows.Forms.TabPage();
            this.textBoxPasswordNJF = new System.Windows.Forms.TextBox();
            this.textBoxUserNameNJF = new System.Windows.Forms.TextBox();
            this.textBoxDatabaseNameNJF = new System.Windows.Forms.TextBox();
            this.textBoxServerNameNJF = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.tabPageSQLServer = new System.Windows.Forms.TabPage();
            this.textBoxPasswordSQL = new System.Windows.Forms.TextBox();
            this.textBoxUserNameSQL = new System.Windows.Forms.TextBox();
            this.textBoxDatabaseNameSQL = new System.Windows.Forms.TextBox();
            this.textBoxServerNameSQL = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.textBoxMigrationLog = new System.Windows.Forms.TextBox();
            this.tabControlPreferences.SuspendLayout();
            this.tabPage1NF.SuspendLayout();
            this.tabPageBalans.SuspendLayout();
            this.tabPagePrivatization.SuspendLayout();
            this.tabPageNJF.SuspendLayout();
            this.tabPageSQLServer.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(713, 380);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 24);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonMigrate
            // 
            this.buttonMigrate.Location = new System.Drawing.Point(607, 380);
            this.buttonMigrate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonMigrate.Name = "buttonMigrate";
            this.buttonMigrate.Size = new System.Drawing.Size(100, 24);
            this.buttonMigrate.TabIndex = 1;
            this.buttonMigrate.Text = "Migrate data";
            this.buttonMigrate.UseVisualStyleBackColor = true;
            this.buttonMigrate.Click += new System.EventHandler(this.buttonMigrate_Click);
            // 
            // tabControlPreferences
            // 
            this.tabControlPreferences.Controls.Add(this.tabPage1NF);
            this.tabControlPreferences.Controls.Add(this.tabPageBalans);
            this.tabControlPreferences.Controls.Add(this.tabPagePrivatization);
            this.tabControlPreferences.Controls.Add(this.tabPageNJF);
            this.tabControlPreferences.Controls.Add(this.tabPageSQLServer);
            this.tabControlPreferences.Controls.Add(this.tabPageLog);
            this.tabControlPreferences.Location = new System.Drawing.Point(12, 13);
            this.tabControlPreferences.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlPreferences.Name = "tabControlPreferences";
            this.tabControlPreferences.SelectedIndex = 0;
            this.tabControlPreferences.Size = new System.Drawing.Size(805, 359);
            this.tabControlPreferences.TabIndex = 2;
            // 
            // tabPage1NF
            // 
            this.tabPage1NF.Controls.Add(this.textBoxPassword1NF);
            this.tabPage1NF.Controls.Add(this.textBoxUserName1NF);
            this.tabPage1NF.Controls.Add(this.textBoxDatabaseName1NF);
            this.tabPage1NF.Controls.Add(this.textBoxServerName1NF);
            this.tabPage1NF.Controls.Add(this.label4);
            this.tabPage1NF.Controls.Add(this.label3);
            this.tabPage1NF.Controls.Add(this.label2);
            this.tabPage1NF.Controls.Add(this.label1);
            this.tabPage1NF.Location = new System.Drawing.Point(4, 23);
            this.tabPage1NF.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1NF.Name = "tabPage1NF";
            this.tabPage1NF.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1NF.Size = new System.Drawing.Size(797, 332);
            this.tabPage1NF.TabIndex = 0;
            this.tabPage1NF.Text = "1NF";
            this.tabPage1NF.UseVisualStyleBackColor = true;
            // 
            // textBoxPassword1NF
            // 
            this.textBoxPassword1NF.Enabled = false;
            this.textBoxPassword1NF.Location = new System.Drawing.Point(6, 226);
            this.textBoxPassword1NF.Name = "textBoxPassword1NF";
            this.textBoxPassword1NF.Size = new System.Drawing.Size(785, 22);
            this.textBoxPassword1NF.TabIndex = 7;
            // 
            // textBoxUserName1NF
            // 
            this.textBoxUserName1NF.Enabled = false;
            this.textBoxUserName1NF.Location = new System.Drawing.Point(6, 161);
            this.textBoxUserName1NF.Name = "textBoxUserName1NF";
            this.textBoxUserName1NF.Size = new System.Drawing.Size(785, 22);
            this.textBoxUserName1NF.TabIndex = 6;
            // 
            // textBoxDatabaseName1NF
            // 
            this.textBoxDatabaseName1NF.Enabled = false;
            this.textBoxDatabaseName1NF.Location = new System.Drawing.Point(6, 96);
            this.textBoxDatabaseName1NF.Name = "textBoxDatabaseName1NF";
            this.textBoxDatabaseName1NF.Size = new System.Drawing.Size(785, 22);
            this.textBoxDatabaseName1NF.TabIndex = 5;
            // 
            // textBoxServerName1NF
            // 
            this.textBoxServerName1NF.Enabled = false;
            this.textBoxServerName1NF.Location = new System.Drawing.Point(6, 36);
            this.textBoxServerName1NF.Name = "textBoxServerName1NF";
            this.textBoxServerName1NF.Size = new System.Drawing.Size(785, 22);
            this.textBoxServerName1NF.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "User Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Database Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Name";
            // 
            // tabPageBalans
            // 
            this.tabPageBalans.Controls.Add(this.textBoxPasswordBalans);
            this.tabPageBalans.Controls.Add(this.textBoxUserNameBalans);
            this.tabPageBalans.Controls.Add(this.textBoxDatabaseNameBalans);
            this.tabPageBalans.Controls.Add(this.textBoxServerNameBalans);
            this.tabPageBalans.Controls.Add(this.label9);
            this.tabPageBalans.Controls.Add(this.label10);
            this.tabPageBalans.Controls.Add(this.label11);
            this.tabPageBalans.Controls.Add(this.label12);
            this.tabPageBalans.Location = new System.Drawing.Point(4, 23);
            this.tabPageBalans.Name = "tabPageBalans";
            this.tabPageBalans.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBalans.Size = new System.Drawing.Size(797, 332);
            this.tabPageBalans.TabIndex = 3;
            this.tabPageBalans.Text = "Balans";
            this.tabPageBalans.UseVisualStyleBackColor = true;
            // 
            // textBoxPasswordBalans
            // 
            this.textBoxPasswordBalans.Enabled = false;
            this.textBoxPasswordBalans.Location = new System.Drawing.Point(6, 226);
            this.textBoxPasswordBalans.Name = "textBoxPasswordBalans";
            this.textBoxPasswordBalans.Size = new System.Drawing.Size(785, 22);
            this.textBoxPasswordBalans.TabIndex = 15;
            // 
            // textBoxUserNameBalans
            // 
            this.textBoxUserNameBalans.Enabled = false;
            this.textBoxUserNameBalans.Location = new System.Drawing.Point(6, 161);
            this.textBoxUserNameBalans.Name = "textBoxUserNameBalans";
            this.textBoxUserNameBalans.Size = new System.Drawing.Size(785, 22);
            this.textBoxUserNameBalans.TabIndex = 14;
            // 
            // textBoxDatabaseNameBalans
            // 
            this.textBoxDatabaseNameBalans.Enabled = false;
            this.textBoxDatabaseNameBalans.Location = new System.Drawing.Point(6, 96);
            this.textBoxDatabaseNameBalans.Name = "textBoxDatabaseNameBalans";
            this.textBoxDatabaseNameBalans.Size = new System.Drawing.Size(785, 22);
            this.textBoxDatabaseNameBalans.TabIndex = 13;
            // 
            // textBoxServerNameBalans
            // 
            this.textBoxServerNameBalans.Enabled = false;
            this.textBoxServerNameBalans.Location = new System.Drawing.Point(6, 36);
            this.textBoxServerNameBalans.Name = "textBoxServerNameBalans";
            this.textBoxServerNameBalans.Size = new System.Drawing.Size(785, 22);
            this.textBoxServerNameBalans.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 209);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 14);
            this.label9.TabIndex = 11;
            this.label9.Text = "Password";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 144);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 14);
            this.label10.TabIndex = 10;
            this.label10.Text = "User Name";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 14);
            this.label11.TabIndex = 9;
            this.label11.Text = "Database Name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 14);
            this.label12.TabIndex = 8;
            this.label12.Text = "Server Name";
            // 
            // tabPagePrivatization
            // 
            this.tabPagePrivatization.Controls.Add(this.textBoxPasswordPriv);
            this.tabPagePrivatization.Controls.Add(this.textBoxUserNamePriv);
            this.tabPagePrivatization.Controls.Add(this.textBoxDatabaseNamePriv);
            this.tabPagePrivatization.Controls.Add(this.textBoxServerNamePriv);
            this.tabPagePrivatization.Controls.Add(this.label13);
            this.tabPagePrivatization.Controls.Add(this.label14);
            this.tabPagePrivatization.Controls.Add(this.label15);
            this.tabPagePrivatization.Controls.Add(this.label16);
            this.tabPagePrivatization.Location = new System.Drawing.Point(4, 23);
            this.tabPagePrivatization.Name = "tabPagePrivatization";
            this.tabPagePrivatization.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePrivatization.Size = new System.Drawing.Size(797, 332);
            this.tabPagePrivatization.TabIndex = 4;
            this.tabPagePrivatization.Text = "Privatizaciya";
            this.tabPagePrivatization.UseVisualStyleBackColor = true;
            // 
            // textBoxPasswordPriv
            // 
            this.textBoxPasswordPriv.Enabled = false;
            this.textBoxPasswordPriv.Location = new System.Drawing.Point(6, 226);
            this.textBoxPasswordPriv.Name = "textBoxPasswordPriv";
            this.textBoxPasswordPriv.Size = new System.Drawing.Size(785, 22);
            this.textBoxPasswordPriv.TabIndex = 15;
            // 
            // textBoxUserNamePriv
            // 
            this.textBoxUserNamePriv.Enabled = false;
            this.textBoxUserNamePriv.Location = new System.Drawing.Point(6, 161);
            this.textBoxUserNamePriv.Name = "textBoxUserNamePriv";
            this.textBoxUserNamePriv.Size = new System.Drawing.Size(785, 22);
            this.textBoxUserNamePriv.TabIndex = 14;
            // 
            // textBoxDatabaseNamePriv
            // 
            this.textBoxDatabaseNamePriv.Enabled = false;
            this.textBoxDatabaseNamePriv.Location = new System.Drawing.Point(6, 96);
            this.textBoxDatabaseNamePriv.Name = "textBoxDatabaseNamePriv";
            this.textBoxDatabaseNamePriv.Size = new System.Drawing.Size(785, 22);
            this.textBoxDatabaseNamePriv.TabIndex = 13;
            // 
            // textBoxServerNamePriv
            // 
            this.textBoxServerNamePriv.Enabled = false;
            this.textBoxServerNamePriv.Location = new System.Drawing.Point(6, 36);
            this.textBoxServerNamePriv.Name = "textBoxServerNamePriv";
            this.textBoxServerNamePriv.Size = new System.Drawing.Size(785, 22);
            this.textBoxServerNamePriv.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 209);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 14);
            this.label13.TabIndex = 11;
            this.label13.Text = "Password";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 144);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 14);
            this.label14.TabIndex = 10;
            this.label14.Text = "User Name";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 79);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(108, 14);
            this.label15.TabIndex = 9;
            this.label15.Text = "Database Name";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 19);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(88, 14);
            this.label16.TabIndex = 8;
            this.label16.Text = "Server Name";
            // 
            // tabPageNJF
            // 
            this.tabPageNJF.Controls.Add(this.textBoxPasswordNJF);
            this.tabPageNJF.Controls.Add(this.textBoxUserNameNJF);
            this.tabPageNJF.Controls.Add(this.textBoxDatabaseNameNJF);
            this.tabPageNJF.Controls.Add(this.textBoxServerNameNJF);
            this.tabPageNJF.Controls.Add(this.label17);
            this.tabPageNJF.Controls.Add(this.label18);
            this.tabPageNJF.Controls.Add(this.label19);
            this.tabPageNJF.Controls.Add(this.label20);
            this.tabPageNJF.Location = new System.Drawing.Point(4, 23);
            this.tabPageNJF.Name = "tabPageNJF";
            this.tabPageNJF.Size = new System.Drawing.Size(797, 332);
            this.tabPageNJF.TabIndex = 5;
            this.tabPageNJF.Text = "NJF";
            this.tabPageNJF.UseVisualStyleBackColor = true;
            // 
            // textBoxPasswordNJF
            // 
            this.textBoxPasswordNJF.Enabled = false;
            this.textBoxPasswordNJF.Location = new System.Drawing.Point(6, 226);
            this.textBoxPasswordNJF.Name = "textBoxPasswordNJF";
            this.textBoxPasswordNJF.Size = new System.Drawing.Size(785, 22);
            this.textBoxPasswordNJF.TabIndex = 15;
            // 
            // textBoxUserNameNJF
            // 
            this.textBoxUserNameNJF.Enabled = false;
            this.textBoxUserNameNJF.Location = new System.Drawing.Point(6, 161);
            this.textBoxUserNameNJF.Name = "textBoxUserNameNJF";
            this.textBoxUserNameNJF.Size = new System.Drawing.Size(785, 22);
            this.textBoxUserNameNJF.TabIndex = 14;
            // 
            // textBoxDatabaseNameNJF
            // 
            this.textBoxDatabaseNameNJF.Enabled = false;
            this.textBoxDatabaseNameNJF.Location = new System.Drawing.Point(6, 96);
            this.textBoxDatabaseNameNJF.Name = "textBoxDatabaseNameNJF";
            this.textBoxDatabaseNameNJF.Size = new System.Drawing.Size(785, 22);
            this.textBoxDatabaseNameNJF.TabIndex = 13;
            // 
            // textBoxServerNameNJF
            // 
            this.textBoxServerNameNJF.Enabled = false;
            this.textBoxServerNameNJF.Location = new System.Drawing.Point(6, 36);
            this.textBoxServerNameNJF.Name = "textBoxServerNameNJF";
            this.textBoxServerNameNJF.Size = new System.Drawing.Size(785, 22);
            this.textBoxServerNameNJF.TabIndex = 12;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 209);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(69, 14);
            this.label17.TabIndex = 11;
            this.label17.Text = "Password";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 144);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(76, 14);
            this.label18.TabIndex = 10;
            this.label18.Text = "User Name";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 79);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(108, 14);
            this.label19.TabIndex = 9;
            this.label19.Text = "Database Name";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 19);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(88, 14);
            this.label20.TabIndex = 8;
            this.label20.Text = "Server Name";
            // 
            // tabPageSQLServer
            // 
            this.tabPageSQLServer.Controls.Add(this.textBoxPasswordSQL);
            this.tabPageSQLServer.Controls.Add(this.textBoxUserNameSQL);
            this.tabPageSQLServer.Controls.Add(this.textBoxDatabaseNameSQL);
            this.tabPageSQLServer.Controls.Add(this.textBoxServerNameSQL);
            this.tabPageSQLServer.Controls.Add(this.label5);
            this.tabPageSQLServer.Controls.Add(this.label6);
            this.tabPageSQLServer.Controls.Add(this.label7);
            this.tabPageSQLServer.Controls.Add(this.label8);
            this.tabPageSQLServer.Location = new System.Drawing.Point(4, 23);
            this.tabPageSQLServer.Name = "tabPageSQLServer";
            this.tabPageSQLServer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSQLServer.Size = new System.Drawing.Size(797, 332);
            this.tabPageSQLServer.TabIndex = 1;
            this.tabPageSQLServer.Text = "SQL Server";
            this.tabPageSQLServer.UseVisualStyleBackColor = true;
            // 
            // textBoxPasswordSQL
            // 
            this.textBoxPasswordSQL.Enabled = false;
            this.textBoxPasswordSQL.Location = new System.Drawing.Point(6, 226);
            this.textBoxPasswordSQL.Name = "textBoxPasswordSQL";
            this.textBoxPasswordSQL.Size = new System.Drawing.Size(785, 22);
            this.textBoxPasswordSQL.TabIndex = 15;
            // 
            // textBoxUserNameSQL
            // 
            this.textBoxUserNameSQL.Enabled = false;
            this.textBoxUserNameSQL.Location = new System.Drawing.Point(6, 161);
            this.textBoxUserNameSQL.Name = "textBoxUserNameSQL";
            this.textBoxUserNameSQL.Size = new System.Drawing.Size(785, 22);
            this.textBoxUserNameSQL.TabIndex = 14;
            // 
            // textBoxDatabaseNameSQL
            // 
            this.textBoxDatabaseNameSQL.Enabled = false;
            this.textBoxDatabaseNameSQL.Location = new System.Drawing.Point(6, 96);
            this.textBoxDatabaseNameSQL.Name = "textBoxDatabaseNameSQL";
            this.textBoxDatabaseNameSQL.Size = new System.Drawing.Size(785, 22);
            this.textBoxDatabaseNameSQL.TabIndex = 13;
            // 
            // textBoxServerNameSQL
            // 
            this.textBoxServerNameSQL.Enabled = false;
            this.textBoxServerNameSQL.Location = new System.Drawing.Point(6, 36);
            this.textBoxServerNameSQL.Name = "textBoxServerNameSQL";
            this.textBoxServerNameSQL.Size = new System.Drawing.Size(785, 22);
            this.textBoxServerNameSQL.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 209);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 14);
            this.label5.TabIndex = 11;
            this.label5.Text = "Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 14);
            this.label6.TabIndex = 10;
            this.label6.Text = "User Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 14);
            this.label7.TabIndex = 9;
            this.label7.Text = "Database Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 14);
            this.label8.TabIndex = 8;
            this.label8.Text = "Server Name";
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.textBoxMigrationLog);
            this.tabPageLog.Location = new System.Drawing.Point(4, 23);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(797, 332);
            this.tabPageLog.TabIndex = 2;
            this.tabPageLog.Text = "Migration Log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // textBoxMigrationLog
            // 
            this.textBoxMigrationLog.Location = new System.Drawing.Point(6, 6);
            this.textBoxMigrationLog.Multiline = true;
            this.textBoxMigrationLog.Name = "textBoxMigrationLog";
            this.textBoxMigrationLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMigrationLog.Size = new System.Drawing.Size(785, 320);
            this.textBoxMigrationLog.TabIndex = 0;
            this.textBoxMigrationLog.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 417);
            this.Controls.Add(this.tabControlPreferences);
            this.Controls.Add(this.buttonMigrate);
            this.Controls.Add(this.buttonCancel);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GUKV Data Migration Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControlPreferences.ResumeLayout(false);
            this.tabPage1NF.ResumeLayout(false);
            this.tabPage1NF.PerformLayout();
            this.tabPageBalans.ResumeLayout(false);
            this.tabPageBalans.PerformLayout();
            this.tabPagePrivatization.ResumeLayout(false);
            this.tabPagePrivatization.PerformLayout();
            this.tabPageNJF.ResumeLayout(false);
            this.tabPageNJF.PerformLayout();
            this.tabPageSQLServer.ResumeLayout(false);
            this.tabPageSQLServer.PerformLayout();
            this.tabPageLog.ResumeLayout(false);
            this.tabPageLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonMigrate;
        private System.Windows.Forms.TabControl tabControlPreferences;
        private System.Windows.Forms.TabPage tabPage1NF;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPassword1NF;
        private System.Windows.Forms.TextBox textBoxUserName1NF;
        private System.Windows.Forms.TextBox textBoxDatabaseName1NF;
        private System.Windows.Forms.TextBox textBoxServerName1NF;
        private System.Windows.Forms.TabPage tabPageSQLServer;
        private System.Windows.Forms.TextBox textBoxPasswordSQL;
        private System.Windows.Forms.TextBox textBoxUserNameSQL;
        private System.Windows.Forms.TextBox textBoxDatabaseNameSQL;
        private System.Windows.Forms.TextBox textBoxServerNameSQL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.TextBox textBoxMigrationLog;
        private System.Windows.Forms.TabPage tabPageBalans;
        private System.Windows.Forms.TextBox textBoxPasswordBalans;
        private System.Windows.Forms.TextBox textBoxUserNameBalans;
        private System.Windows.Forms.TextBox textBoxDatabaseNameBalans;
        private System.Windows.Forms.TextBox textBoxServerNameBalans;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPagePrivatization;
        private System.Windows.Forms.TextBox textBoxPasswordPriv;
        private System.Windows.Forms.TextBox textBoxUserNamePriv;
        private System.Windows.Forms.TextBox textBoxDatabaseNamePriv;
        private System.Windows.Forms.TextBox textBoxServerNamePriv;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabPage tabPageNJF;
        private System.Windows.Forms.TextBox textBoxPasswordNJF;
        private System.Windows.Forms.TextBox textBoxUserNameNJF;
        private System.Windows.Forms.TextBox textBoxDatabaseNameNJF;
        private System.Windows.Forms.TextBox textBoxServerNameNJF;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
    }
}