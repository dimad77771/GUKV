using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GUKV.DataMigration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            char[] separators = new char[] { ';' };

            // Try to connect to the SQL Server
            SqlConnection connection = ConnectToSQLServer(GetSQLClientConnectionString());

            if (connection != null)
            {
                try
                {
                    // Load the industry coefficients
                    StreamReader reader = new StreamReader("../../Data/Balans_Industry_Data.csv", Encoding.ASCII);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine().Trim();

                        if (line.Length == 0)
                        {
                            break;
                        }

                        string[] parts = line.Split(separators);

                        if (parts.Length > 2 && parts[0].Length > 0 && parts[2].Length > 0)
                        {
                            int industryCode = int.Parse(parts[0]);
                            double koef = double.Parse(parts[2]);

                            // Update the industry coefficient in our DB
                            string statement = "UPDATE dbo.dict_org_old_industry SET budg_payments_rate = @rate WHERE id = @oid";

                            using (SqlCommand cmd = new SqlCommand(statement, connection))
                            {
                                cmd.Parameters.Add(new SqlParameter("oid", industryCode));
                                cmd.Parameters.Add(new SqlParameter("rate", koef / 100.0));

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    reader.Close();

                    // Load the organization coefficients
                    reader = new StreamReader("../../Data/Balans_Org_Data.csv", Encoding.ASCII);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine().Trim();

                        if (line.Length == 0)
                        {
                            break;
                        }

                        string[] parts = line.Split(separators);

                        if (parts.Length > 2 && parts[0].Length > 0 && parts[2].Length > 0)
                        {
                            string zkpo = parts[0].Trim();
                            double koef = double.Parse(parts[2]);

                            // Update the organization coefficient in our DB
                            string statement = "UPDATE dbo.organizations SET budg_payments_rate = @rate WHERE RTRIM(LTRIM(zkpo_code)) = @zkpo";

                            using (SqlCommand cmd = new SqlCommand(statement, connection))
                            {
                                cmd.Parameters.Add(new SqlParameter("zkpo", zkpo));
                                cmd.Parameters.Add(new SqlParameter("rate", koef / 100.0));

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    reader.Close();
                }
                finally
                {
                    connection.Close();
                }

                MessageBox.Show("Completed.");
            }
        }

        private SqlConnection ConnectToSQLServer(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect to the SQL Server. Message: " + ex.Message);

                return null;
            }
        }

        private string GetSQLClientConnectionString()
        {
            return GetSQLClientConnectionString(
                Properties.Settings.Default.ConnectionSQLServerServer,
                Properties.Settings.Default.ConnectionSQLServerDatabase,
                Properties.Settings.Default.ConnectionSQLServerUser,
                Properties.Settings.Default.ConnectionSQLServerPassword);
        }

        private string GetSQLClientConnectionString(string server, string database,
            string user, string password)
        {
            return
                "Server=" + server + ";" +
                "Database=" + database + ";" +
                "UID=" + user + ";" +
                "PWD=" + password;
        }
    }
}
