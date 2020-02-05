using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;

namespace GUKV.DataMigration
{
    public partial class SqlErrorForm : Form
    {
        public SqlErrorForm()
        {
            InitializeComponent();
        }

        public static void ShowSqlErrorMessageDlg(string sqlStatement, FbCommand command, string errorMessage)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            // Display the error dialog
            using (SqlErrorForm errorForm = new SqlErrorForm())
            {
                // Append all parameters to the SQL statement
                if (command != null)
                {
                    string parameters = "";

                    foreach (FbParameter param in command.Parameters)
                    {
                        string paramDescr = param.ParameterName + " = " + ((param.Value == null) ? "null" : param.Value.ToString());

                        if (parameters.Length > 0)
                        {
                            parameters += "\n";
                        }

                        parameters += paramDescr;
                    }

                    sqlStatement += "\n\n" + parameters;
                }

                errorForm.textBoxSqlStatement.Lines = sqlStatement.Split('\n');
                errorForm.textBoxErrorMessage.Text = errorMessage;

                errorForm.ShowDialog();
            }
        }

        public static void ShowSqlErrorMessageDlg(string sqlStatement, SqlCommand command, string errorMessage)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            // Display the error dialog
            using (SqlErrorForm errorForm = new SqlErrorForm())
            {
                // Append all parameters to the SQL statement
                if (command != null)
                {
                    string parameters = "";

                    foreach (FbParameter param in command.Parameters)
                    {
                        string paramDescr = param.ParameterName + " = " + ((param.Value == null) ? "null" : param.Value.ToString());

                        if (parameters.Length > 0)
                        {
                            parameters += "\n";
                        }

                        parameters += paramDescr;
                    }

                    sqlStatement += "\n\n" + parameters;
                }

                errorForm.textBoxSqlStatement.Lines = sqlStatement.Split('\n');
                errorForm.textBoxErrorMessage.Text = errorMessage;

                errorForm.ShowDialog();
            }
        }
    }
}
