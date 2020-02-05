using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GUKV.DataMigration
{
    /// <summary>
    /// This class reads application preferences from the INI file
    /// </summary>
    public class Preferences
    {
        #region Preferences

        /// <summary>
        /// Name of the folder that contains global configuration settings
        /// </summary>
        public const string iniFolderName = "K:\\ITG\\ToolSettings";

        /// <summary>
        /// Name of the INI file with connection settings
        /// </summary>
        public const string iniConnFileName = "ConnectionSettings.ini";

        /// <summary>
        /// Name of the INI file with Users database connection settings
        /// </summary>
        public const string iniUsersFileName = "Users.ini";

        /// <summary>
        /// Name of the INI file with NJF database connection settings
        /// </summary>
        public const string iniNJFFileName = "NJF.ini";

        /// <summary>
        /// User login to the NJF database
        /// </summary>
        public string userNameNJF = "SYSDBA";

        /// <summary>
        /// User password to the NJF database
        /// </summary>
        public string userPasswordNJF = "masterkey";

        /// <summary>
        /// User login to the Users database
        /// </summary>
        public string userNameUsers = "SYSDBA";

        /// <summary>
        /// User password to the Users database
        /// </summary>
        public string userPasswordUsers = "masterkey";

        /// <summary>
        /// User login to the 1NF database
        /// </summary>
        public string userName1NF = "SYSDBA";

        /// <summary>
        /// User password to the 1NF database
        /// </summary>
        public string userPassword1NF = "masterkey";

        /// <summary>
        /// User login to the Sql Server
        /// </summary>
        public string userNameSql = "sa";

        /// <summary>
        /// User password to the Sql Server
        /// </summary>
        public string userPasswordSql = "13579";

        /// <summary>
        /// Server name of the NJF connection string
        /// </summary>
        public string serverNameNJF = "gukv_1nf";

        /// <summary>
        /// Database name of the NJF connection string
        /// </summary>
        public string databaseNameNJF = "c:\\database\\1nf_reserve\\newnjf.gdb";

        /// <summary>
        /// Server name of the Users database connection string
        /// </summary>
        public string serverNameUsers = "gukv";

        /// <summary>
        /// Database name of the Users database connection string
        /// </summary>
        public string databaseNameUsers = "d:\\database\\users\\users.gdb";

        /// <summary>
        /// Server name of the 1NF database connection string
        /// </summary>
        public string serverName1NF = "gukv_1nf";

        /// <summary>
        /// Database name of the 1NF database connection string
        /// </summary>
        public string databaseName1NF = "c:\\database\\1nf\\1nf.gdb";

        /// <summary>
        /// Server name of the SQL Server connection string
        /// </summary>
        public string serverNameSql = "localhost";

        /// <summary>
        /// Database name of the SQL Server connection string
        /// </summary>
        public string databaseNameSql = "GUKV";

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
        /// <param name="server">Server name</param>
        /// <param name="database">Database name</param>
        /// <param name="user">User name</param>
        /// <param name="password">User password</param>
        /// <returns>Created connection string</returns>
        private string GetSQLClientConnectionString(string server, string database,
            string user, string password)
        {
            return
                "Server=" + server + ";" +
                "Database=" + database + ";" +
                "UID=" + user + ";" +
                "PWD=" + password;
        }

        /// <summary>
        /// Builds a connection string for NJF database
        /// </summary>
        /// <returns>Connection string for NJF database</returns>
        public string GetNJFConnectionString(string userName, string password)
        {
            return GetFirebirdConnectionString(
                serverNameNJF,
                databaseNameNJF,
                userName,
                password);
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
        /// Builds a connection string for Users database
        /// </summary>
        /// <returns>Connection string for Users database</returns>
        public string GetUsersConnectionString()
        {
            return GetFirebirdConnectionString(
                serverNameUsers,
                databaseNameUsers,
                userNameUsers,
                userPasswordUsers);
        }

        /// <summary>
        /// Builds a connection string to the SQL Server
        /// </summary>
        /// <returns>Connection string to the SQL Server</returns>
        public string GetSqlServerConnectionString()
        {
            return GetSQLClientConnectionString(
                serverNameSql,
                databaseNameSql,
                userNameSql,
                userPasswordSql);
        }

        #endregion (Database connection string support)

        #region INI file support

        /// <summary>
        /// Reads preferences from the INI file
        /// </summary>
        /// <returns>TRUE if the INI file was located and parsed</returns>
        public bool ReadConnPreferencesFromFile()
        {
            if (!ReadConnPreferencesFromFile(iniNJFFileName))
            {
                return false;
            }

            if (!ReadConnPreferencesFromFile(iniUsersFileName))
            {
                return false;
            }

            if (!ReadConnPreferencesFromFile(iniConnFileName))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reads the connection preferences from the specified file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool ReadConnPreferencesFromFile(string fileName)
        {
            // Try local file first
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName, Encoding.ASCII))
                {
                    ReadPreferencesFromStream(reader);

                    reader.Close();
                }

                return true;
            }

            // Try the global path
            if (File.Exists(iniFolderName + "\\" + fileName))
            {
                using (StreamReader reader = new StreamReader(iniFolderName + "\\" + fileName, Encoding.ASCII))
                {
                    ReadPreferencesFromStream(reader);

                    reader.Close();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Reads connection preferences from the provided ASCII file stream
        /// </summary>
        /// <param name="reader">The input stream</param>
        private void ReadPreferencesFromStream(StreamReader reader)
        {
            string line = "";

            string tempServerName = "";
            string tempDatabaseName = "";
            string tempUserName = "";
            string tempUserPwd = "";

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

                    if (line == "NJF")
                    {
                        line = ReadSectionFromStream(reader,
                            ref serverNameNJF, ref databaseNameNJF,
                            ref userNameNJF, ref userPasswordNJF);
                    }
                    else if (line == "USERS")
                    {
                        line = ReadSectionFromStream(reader,
                            ref serverNameUsers, ref databaseNameUsers,
                            ref userNameUsers, ref userPasswordUsers);
                    }
                    else if (line == "1NF")
                    {
                        line = ReadSectionFromStream(reader,
                            ref serverName1NF, ref databaseName1NF,
                            ref userName1NF, ref userPassword1NF);
                    }
                    else if (line == "GUKV")
                    {
                        line = ReadSectionFromStream(reader,
                            ref serverNameSql, ref databaseNameSql,
                            ref userNameSql, ref userPasswordSql);
                    }
                    else
                    {
                        line = ReadSectionFromStream(reader,
                            ref tempServerName, ref tempDatabaseName,
                            ref tempUserName, ref tempUserPwd);
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
