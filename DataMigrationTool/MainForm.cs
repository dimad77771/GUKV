using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUKV.DataMigration
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Initialize the default preferences
            //textBoxServerName1NF.Text = Properties.Settings.Default.Connection1NFServer;
            //textBoxDatabaseName1NF.Text = Properties.Settings.Default.Connection1NFDatabase;
            //textBoxUserName1NF.Text = Properties.Settings.Default.Connection1NFUser;
            //textBoxPassword1NF.Text = Properties.Settings.Default.Connection1NFPassword;

            //textBoxServerNameBalans.Text = Properties.Settings.Default.ConnectionBalansServer;
            //textBoxDatabaseNameBalans.Text = Properties.Settings.Default.ConnectionBalansDatabase;
            //textBoxUserNameBalans.Text = Properties.Settings.Default.ConnectionBalansUser;
            //textBoxPasswordBalans.Text = Properties.Settings.Default.ConnectionBalansPassword;

            //textBoxServerNamePriv.Text = Properties.Settings.Default.ConnectionPrivatServer;
            //textBoxDatabaseNamePriv.Text = Properties.Settings.Default.ConnectionPrivatDatabase;
            //textBoxUserNamePriv.Text = Properties.Settings.Default.ConnectionPrivatUser;
            //textBoxPasswordPriv.Text = Properties.Settings.Default.ConnectionPrivatPassword;

            //textBoxServerNameNJF.Text = Properties.Settings.Default.ConnectionNJFServer;
            //textBoxDatabaseNameNJF.Text = Properties.Settings.Default.ConnectionNJFDatabase;
            //textBoxUserNameNJF.Text = Properties.Settings.Default.ConnectionNJFUser;
            //textBoxPasswordNJF.Text = Properties.Settings.Default.ConnectionNJFPassword;

            textBoxServerNameSQL.Text = Properties.Settings.Default.ConnectionSQLServerServer;
            textBoxDatabaseNameSQL.Text = Properties.Settings.Default.ConnectionSQLServerDatabase;
            textBoxUserNameSQL.Text = Properties.Settings.Default.ConnectionSQLServerUser;
            textBoxPasswordSQL.Text = Properties.Settings.Default.ConnectionSQLServerPassword;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonMigrate_Click(object sender, EventArgs e)
        {
            // Disable any activity until migration stops
            buttonMigrate.Enabled = false;
            buttonCancel.Enabled = false;

            // Create a logger
            MigrationLog log = new MigrationLog(textBoxMigrationLog);

            // Switch to the log page in the Tab control
            tabControlPreferences.SelectedTab = tabPageLog;

            // Perform data migration
            Migrator migrator = new Migrator(log);

            if (migrator.TestConnection())
            {
                migrator.Disconnect();
                migrator.PerformMigration();
            }

            migrator.Disconnect();

            log.WriteInfo("Finished");

            MessageBox.Show(
                "Data migration completed.",
                "Data Migration",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Enable the buttons back
            buttonMigrate.Enabled = true;
            buttonCancel.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}