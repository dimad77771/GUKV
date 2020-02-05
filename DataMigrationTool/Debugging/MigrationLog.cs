using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GUKV.DataMigration.Migration;

namespace GUKV.DataMigration
{
    /// <summary>
    /// Provides a logging interface for the Migrator class.
    /// </summary>
    public class MigrationLog : IMigrationLog
    {
        #region Member variables

        /// <summary>
        /// An output window for the log messages
        /// </summary>
        private TextBox textBoxOutput = null;

        /// <summary>
        /// All the log messages are stored in this list
        /// </summary>
        private List<string> messageList = new List<string>();

        #endregion (Member variables)

        #region Construction

        /// <summary>
        /// Creates a logger and associates it with the provided output window
        /// </summary>
        /// <param name="textBox">A text box to write the log messages to</param>
        public MigrationLog(TextBox textBox)
        {
            textBoxOutput = textBox;

            Clear();
        }

        #endregion (Construction)

        #region Interface

        /// <summary>
        /// Removes all messages from the log
        /// </summary>
        public void Clear()
        {
            messageList.Clear();

            if (textBoxOutput != null)
            {
                messageList.Clear();

                textBoxOutput.Lines = messageList.ToArray();

                textBoxOutput.Invalidate();
                textBoxOutput.Update();
            }
        }

        /// <summary>
        /// Adds a new message to the log
        /// </summary>
        /// <param name="message">The message text</param>
        public void WriteInfo(string message)
        {
            if (textBoxOutput != null)
            {
                messageList.Add(message);

                textBoxOutput.Lines = messageList.ToArray();

                // Scroll the text box down
                textBoxOutput.Select(textBoxOutput.Text.Length - 1, 0);
                textBoxOutput.ScrollToCaret();

                // Redraw the text box
                textBoxOutput.Invalidate();
                textBoxOutput.Update();
            }
        }

        /// <summary>
        /// Writes an error message to the log
        /// </summary>
        /// <param name="message">The message text</param>
        public void WriteError(string message)
        {
            // MessageBox.Show(message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            WriteInfo("ERROR: " + message);
        }

        /// <summary>
        /// Writes an SQL statement error message to the log
        /// </summary>
        public void WriteSQLError(string sqlStatement, string errorMessage)
        {
            WriteInfo("Error in sql statement: " + sqlStatement);
            WriteInfo("Message: " + errorMessage);

            SqlErrorForm.ShowSqlErrorMessageDlg(sqlStatement, errorMessage);
        }

        #endregion (Interface)
    }
}
