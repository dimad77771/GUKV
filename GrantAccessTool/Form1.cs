using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;

namespace GrantAccessTool
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Connection to the NEWNJF database in Firebird
        /// </summary>
        private FbConnection connectionNEWNJF = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonGrant_Click(object sender, EventArgs e)
        {
            // Check that user name is entered
            string userNameToGrant = textBoxUserToGrant.Text;

            userNameToGrant = userNameToGrant.Trim().ToUpper();

            if (userNameToGrant.Length == 0)
            {
                MessageBox.Show(
                    "Please enter the user name to grant access to.",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Connect to NEWNJF database
            connectionNEWNJF = new FbConnection(GetConnectionString());

            try
            {
                connectionNEWNJF.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot conect to database NEWNJF. Message: " + ex.Message,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Get the SQL statement that grants access for the ADMINISTRATOR role
            string select = "SELECT SQL FROM USER_RIGHTS WHERE KIND_USER = 'ADMINISTRATOR'";

            FbDataReader reader = null;

            try
            {
                FbCommand commandSelect = new FbCommand(select, connectionNEWNJF);

                reader = commandSelect.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not execute the SQL Statement: " + select + " Error message: " + ex.Message,
                    "SQL Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                connectionNEWNJF.Close();
                return;
            }

            if (reader.Read())
            {
                string sqlStatement = reader.GetString(0);

                textBoxSQL.Text = sqlStatement;

                // Split the multi-line SQL statement into many independent statements
                char[] separators = new char[1];

                separators[0] = '\n';

                string[] statements = sqlStatement.Split(separators);

                // Execute each statement
                bool bFailure = false;

                for (int i = 0; i < statements.Length; i++)
                {
                    string statement = statements[i].Trim();

                    if (statement.Length > 0)
                    {
                        statement = statement.Replace("_USER_", userNameToGrant);

                        // statement = statement.Replace("GRANT ", "REVOKE ");
                        // statement = statement.Replace("TO _USER_", "FROM _USER_");

                        try
                        {
                            FbCommand commandExecute = new FbCommand(statement, connectionNEWNJF);

                            commandExecute.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                "Could not execute the SQL Statement: " + statement + " Error message: " + ex.Message,
                                "SQL Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            bFailure = true;
                            break;
                        }
                    }
                }

                // We're done!
                if (!bFailure)
                {
                    MessageBox.Show(
                        "Access to the user " + textBoxUserToGrant.Text + " granted.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(
                    "No SQL statement found in the USER_RIGHTS table for user ADMINISTRATOR",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            reader.Close();

            // Close the connection to database
            connectionNEWNJF.Close();
        }

        private string GetConnectionString()
        {
            return "Server=" + textBoxServerName.Text + ";" +
                "Database=" + textBoxDatabaseName.Text + ";" +
                "User=" + textBoxUserName.Text + ";" +
                "Password=" + textBoxPassword.Text;
        }
    }
}
