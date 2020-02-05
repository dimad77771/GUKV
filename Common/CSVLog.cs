
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GUKV.DataMigration
{
    /// <summary>
    /// This log can be used to output CSV data to a file.
    /// </summary>
    public class CSVLog
    {
        #region Constants

        /// <summary>
        /// The separator character which is inserted between cells
        /// </summary>
        private string csvSeparator = ";";

        #endregion (Constants)

        #region Member variables

        /// <summary>
        /// Name of the CSV file we are writing to
        /// </summary>
        private string csvFileName = "";

        /// <summary>
        /// A temporary buffer that holds the current line (which is not written yet)
        /// </summary>
        private string curLine = "";

        #endregion (Member variables)

        #region Construction

        /// <summary>
        /// Creates a new CSV log and associates it with the specified file
        /// </summary>
        /// <param name="fileName">Name of file to write CSV data to</param>
        public CSVLog(string fileName)
        {
            // If the file name is local, we need to make sure
            // that it resides in the same folder with the main .EXE file
            if (fileName.IndexOf(':') >= 0)
            {
                // User specified a global file name
                csvFileName = fileName;
            }
            else
            {
                // User specified a local file name
                string exeFileName = AppDomain.CurrentDomain.BaseDirectory;

                csvFileName = Path.GetDirectoryName(exeFileName) + "/" + fileName;
            }

            // If the directory does not exist, create it
            string folder = Path.GetDirectoryName(csvFileName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // Reset the file contents, if the file exists
            FileInfo fileInfo = new FileInfo(csvFileName);

            using (StreamWriter writer = fileInfo.CreateText())
            {
                writer.Close();
            }
        }

        #endregion (Construction)

        #region Interface

        /// <summary>
        /// Creates a new cell in the CSV file and writes the specified data into this cell
        /// </summary>
        /// <param name="value">A data to write to the cell</param>
        /// /// <param name="formatAsString">If TRUE, the value will be wrapped into quotes</param>
        public void WriteCell(object value, bool formatAsString)
        {
            // Append the cell value to the current line
            if (curLine.Length > 0)
            {
                curLine += csvSeparator;
            }

            // All strings must be included into quotes
            if (value is string)
            {
                curLine += "\"" + FormatStringForCSV(value as string) + "\"";
            }
            else
            {
                if (formatAsString)
                {
                    curLine += "\"" + value.ToString() + "\"";
                }
                else
                {
                    curLine += value.ToString();
                }
            }
        }

        /// <summary>
        /// Creates a new cell in the CSV file and writes the specified data into this cell
        /// </summary>
        /// <param name="value">A data to write to the cell</param>
        public void WriteCell(object value)
        {
            WriteCell(value, false);
        }

        /// <summary>
        /// Writes the accumulated cells to the CSV file and starts a new line
        /// </summary>
        public void WriteEndOfLine()
        {
            // Dump the current line to the file
            using (StreamWriter writer = new StreamWriter(csvFileName, true, Encoding.GetEncoding("windows-1251")))
            {
                writer.WriteLine(curLine);
                writer.Close();
            }

            curLine = "";
        }

        #endregion (Interface)

        #region Implementation

        /// <summary>
        /// Replaces all " characters with the "" sequence.
        /// </summary>
        /// <param name="str">A string to format</param>
        /// <returns>Formatted string</returns>
        private string FormatStringForCSV(string str)
        {
            int quotePos = str.IndexOf('\"');

            if (quotePos < 0)
            {
                // No single quotes in the text; we can use it
                return str;
            }
            else
            {
                string result = "";

                // Find each " symbol and replace it with ""
                while (quotePos >= 0)
                {
                    if (quotePos > 0)
                    {
                        result += str.Substring(0, quotePos);
                    }

                    result += "\"\"";

                    str = str.Remove(0, quotePos + 1);

                    quotePos = str.IndexOf('\"');
                }

                // Do not forget about the remainder of the string
                result += str;

                return result;
            }
        }

        #endregion (Implementation)
    }
}
