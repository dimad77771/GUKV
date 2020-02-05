using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUKV.DataMigration
{
    public partial class SqlErrorForm : Form
    {
        public SqlErrorForm()
        {
            InitializeComponent();
        }

        public string SqlStatement
        {
            set
            {
                textBoxSqlStatement.Text = value;
            }

            get
            {
                return textBoxSqlStatement.Text;
            }
        }

        public string ErrorMessage
        {
            set
            {
                textBoxErrorMessage.Text = value;
            }

            get
            {
                return textBoxErrorMessage.Text;
            }
        }

        /// <summary>
        /// Displays an error message window for an invalid SQL statement
        /// </summary>
        /// <param name="sqlStatement">The SQL statement that caused an error</param>
        /// <param name="errorMessage">The error message</param>
        public static void ShowSqlErrorMessageDlg(string sqlStatement, string errorMessage)
        {
            // Display the error dialog
            using (SqlErrorForm errorForm = new SqlErrorForm())
            {
                errorForm.SqlStatement = sqlStatement;
                errorForm.ErrorMessage = errorMessage;

                errorForm.ShowDialog();
            }
        }
    }
}
