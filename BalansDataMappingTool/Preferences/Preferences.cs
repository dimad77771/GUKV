
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GUKV.BalansDataMappingTool
{
    /// <summary>
    /// This class reads application preferences from the INI file
    /// </summary>
    public class Preferences
    {
        #region Preferences

        /// <summary>
        /// A global path to the INI file. This is the primary place to search for the INI file
        /// </summary>
        public const string iniFileNameGlobal = "K:/SoftBlue/ToolSettings/BalansDataMappingTool.ini";

        /// <summary>
        /// If the global INI file is not found, the local name is used
        /// </summary>
        public const string iniFileNameLocal = "BalansDataMappingTool.ini";

        /// <summary>
        /// Login to the SQL Server
        /// </summary>
        public string userNameSqlServer = "sa";

        /// <summary>
        /// Password of the SQL Server user
        /// </summary>
        public string userPasswordSqlServer = "13579";

        /// <summary>
        /// User login to the 1NF database
        /// </summary>
        public string userName1NF = "SYSDBA";

        /// <summary>
        /// User password to the 1NF database
        /// </summary>
        public string userPassword1NF = "masterkey";

        /// <summary>
        /// User login to the Balans database
        /// </summary>
        public string userNameBalans = "SYSDBA";

        /// <summary>
        /// User password to the Balans database
        /// </summary>
        public string userPasswordBalans = "masterkey";

        /// <summary>
        /// Server name of the SQL Server connection string
        /// </summary>
        public string serverNameSqlServer = "";

        /// <summary>
        /// Database name of the SQL Server connection string
        /// </summary>
        public string databaseNameSqlServer = "";

        /// <summary>
        /// Server name of the 1 NF connection string
        /// </summary>
        public string serverName1NF = "";

        /// <summary>
        /// Database name of the 1 NF connection string
        /// </summary>
        public string databaseName1NF = "";

        /// <summary>
        /// Server name of the Balans connection string
        /// </summary>
        public string serverNameBalans = "";

        /// <summary>
        /// Database name of the Balans connection string
        /// </summary>
        public string databaseNameBalans = "";

        #endregion (Preferences)

        #region Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        public Preferences()
        {
        }

        #endregion (Construction)

        #region Database connection string support

        /// <summary>
        /// Builds a valid Firebird connection string from the given DB conection settings
        /// </summary>
        /// <param name="serverName">Name of the Firebird server</param>
        /// <param name="databaseName">Database name on the server</param>
        /// <param name="userName">User name</param>
        /// <param name="userPassword">User password</param>
        /// <returns>Created connection string</returns>
        private string GetFirebirdConnectionString(
            string serverName,
            string databaseName,
            string userName,
            string userPassword)
        {
            return "Server=" + serverName + ";" +
                "Database=" + databaseName + ";" +
                "User=" + userName + ";" +
                "Password=" + userPassword + ";" +
                "Charset=WIN1251";
        }

        /// <summary>
        /// Builds a valid SQL Client connection string from the given DB conection settings
        /// </summary>
        /// <param name="serverName">Name of the SQL Server</param>
        /// <param name="databaseName">Database name on the server</param>
        /// <param name="userName">User name</param>
        /// <param name="userPassword">User password</param>
        /// <returns>Created connection string</returns>
        private string GetSQLClientConnectionString(
            string serverName,
            string databaseName,
            string userName,
            string userPassword)
        {
            return
                "Server=" + serverName + ";" +
                "Database=" + databaseName + ";" +
                "UID=" + userName + ";" +
                "PWD=" + userPassword;
        }

        /// <summary>
        /// Builds a connection string for SQL Server
        /// </summary>
        /// <returns>SQL Server connection string</returns>
        public string GetSqlServerConnectionString()
        {
            return GetSQLClientConnectionString(
                serverNameSqlServer,
                databaseNameSqlServer,
                userNameSqlServer,
                userPasswordSqlServer);
        }

        /// <summary>
        /// Builds a connection string for 1NF database
        /// </summary>
        /// <returns>Connection string for 1NF database</returns>
        public string Get1NFConnectionString()
        {
            return GetFirebirdConnectionString(
                serverName1NF,
                databaseName1NF,
                userName1NF,
                userPassword1NF);
        }

        /// <summary>
        /// Builds a connection string for Balans database
        /// </summary>
        /// <returns>Connection string for Balans database</returns>
        public string GetBalansConnectionString()
        {
            return GetFirebirdConnectionString(
                serverNameBalans,
                databaseNameBalans,
                userNameBalans,
                userPasswordBalans);
        }

        #endregion (Database connection string support)

        #region INI file support

        /// <summary>
        /// Reads preferences from the INI file
        /// </summary>
        /// <returns>TRUE if the INI file was located and parsed</returns>
        public bool ReadPreferencesFromFile()
        {
            if (File.Exists(iniFileNameGlobal))
            {
                ReadPreferencesFromStream(new StreamReader(iniFileNameGlobal, Encoding.ASCII));

                return true;
            }

            if (File.Exists(iniFileNameLocal))
            {
                ReadPreferencesFromStream(new StreamReader(iniFileNameLocal, Encoding.ASCII));

                return true;
            }

            MessageBox.Show(
                "Неможливо відкрити файл налаштувань " + iniFileNameGlobal +
                    ". Програма не може бути запущена.",
                "Помилка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return false;
        }

        /// <summary>
        /// Reads preferences from the provided ASCII file stream
        /// </summary>
        /// <param name="reader">The input stream</param>
        private void ReadPreferencesFromStream(StreamReader reader)
        {
            string line = "";

            while (!reader.EndOfStream)
            {
                line = line.Trim().ToUpper();

                if (line.Length == 0)
                {
                    line = reader.ReadLine().Trim().ToUpper();
                }

                // Skip comments
                if (line.Length > 0 && !line.StartsWith(";"))
                {
                    // Remove the section header braces
                    if (line.StartsWith("["))
                    {
                        line = line.Remove(0, 1);
                    }

                    if (line.EndsWith("]"))
                    {
                        line = line.Remove(line.Length - 1);
                    }

                    // Parse the section header
                    line = line.Trim();

                    if (line == "SQLSERVER")
                    {
                        line = ReadSectionFromStream(reader,
                            ref serverNameSqlServer, ref databaseNameSqlServer,
                            ref userNameSqlServer, ref userPasswordSqlServer);
                    }
                    else if (line == "1NF")
                    {
                        line = ReadSectionFromStream(reader,
                            ref serverName1NF, ref databaseName1NF,
                            ref userName1NF, ref userPassword1NF);
                    }
                    else if (line == "BALANS")
                    {
                        line = ReadSectionFromStream(reader,
                            ref serverNameBalans, ref databaseNameBalans,
                            ref userNameBalans, ref userPasswordBalans);
                    }
                }
            }
        }

        /// <summary>
        /// Reads preferences from particular section of an INI file.
        /// </summary>
        /// <param name="reader">The input stream</param>
        /// <param name="serverName">If server name is specified in the section
        /// preferences, it is returned in this variable</param>
        /// <param name="databaseName">If database name is specified in the section
        /// preferences, it is returned in this variable</param>
        /// <param name="userName">If user name is specified in the section
        /// preferences, it is returned in this variable</param>
        /// <param name="userPassword">If user password is specified in the section
        /// preferences, it is returned in this variable</param>
        /// <returns>The first line of the INI file after the parsed section</returns>
        private string ReadSectionFromStream(StreamReader reader,
            ref string serverName, ref string databaseName,
            ref string userName, ref string userPassword)
        {
            char[] separator = new char[1];
            separator[0] = '=';

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().Trim();

                if (line.Length > 0)
                {
                    // If another section is reached, stop processing
                    if (line.StartsWith("["))
                    {
                        return line;
                    }

                    // Skip comments
                    if (!line.StartsWith(";"))
                    {
                        string[] statements = line.Split(separator);

                        if (statements.Length == 2)
                        {
                            string param = statements[0].Trim().ToUpper();
                            string value = statements[1].Trim();

                            if (param == "SERVER")
                            {
                                serverName = value;
                            }
                            else if (param == "DATABASE")
                            {
                                databaseName = value;
                            }
                            else if (param == "LOGIN")
                            {
                                userName = value;
                            }
                            else if (param == "PASSWORD")
                            {
                                userPassword = value;
                            }
                        }
                    }
                }
            }

            return "";
        }

        #endregion (INI file support)
    }
}
