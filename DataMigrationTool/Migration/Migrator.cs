using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Resources;
using FirebirdSql.Data.FirebirdClient;
using System.Net.Mail;
using GUKV.ImportToolUtils;
using GUKV.DataMigration.Migration;

namespace GUKV.DataMigration
{
    /// <summary>
    /// Populates the SQL Server database from the Firebird databases.
    /// </summary>
    public partial class Migrator
    {
        #region Constants

        /// <summary>
        /// Relative path to the folder that contains SQL scripts
        /// </summary>
        private const string ScriptFolder = "..\\..\\..\\DbScripts\\";

        /// <summary>
        /// Relative path to the folder that may contain data to import
        /// </summary>
        private const string DataFolder1 = "..\\..\\..\\ImportData\\";

        /// <summary>
        /// Another relative path to the folder that may contain data to import
        /// </summary>
        private const string DataFolder2 = "FinData\\";

        /// <summary>
        /// The local name of file that contains financial coefficients by organization
        /// </summary>
        private const string FinOrgCoefFileMask = "Balans_Org_Data.*.csv";

        /// <summary>
        /// The local name of file that contains financial coefficients by industry
        /// </summary>
        private const string FinIndustryCoefFileMask = "Balans_Industry_Data.*.csv";

        /// <summary>
        /// The local name of file that contains an SQL script used to erase the SQL database
        /// </summary>
        private const string EraserScriptFileName = "SqlServerDbSchema.sql";

        /// <summary>
        /// The local name of file that contains an SQL script which is executed before migration
        /// </summary>
        private const string PreProcessingScriptFileName = "DataMigrationPreProcessing.sql";

        /// <summary>
        /// The local name of file that contains an SQL script which is executed after migration
        /// </summary>
        private const string PostProcessingScriptFileName = "DataMigrationPostProcessing.sql";

        /// <summary>
        /// The local name of file that contains an SQL script used to create the views
        /// </summary>
        private const string ViewCreationScriptFileName = "SqlServerViews.sql";

        #endregion (Constants)

        #region Member variables

        /// <summary>
        /// Connection to the SQL Server
        /// </summary>
        private SqlConnection connectionSqlClient = null;

        /// <summary>
        /// Connection to the 1NF database in Firebird
        /// </summary>
        //private FbConnection connection1NF = null;

        /// <summary>
        /// Connection to the Balans database in Firebird
        /// </summary>
        //private FbConnection connectionBalans = null;

        /// <summary>
        /// Connection to the Privatizacia database in Firebird
        /// </summary>
        //private FbConnection connectionPrivatizacia = null;

        /// <summary>
        /// Connection to the Rosporadjennia database in Firebird
        /// </summary>
        //private FbConnection connectionNJF = null;

        /// <summary>
        /// Logger that should be used for debug output
        /// </summary>
        private IMigrationLog logger = null;

        /// <summary>
        /// Provides organization lookup in the 1 NF database
        /// </summary>
        private OrganizationFinder organizationFinder = new OrganizationFinder();

        /// <summary>
        /// Provides object lookup in the 1 NF database
        /// </summary>
        //private ObjectFinder objectFinder = new ObjectFinder();

        /// <summary>
        /// Provides document lookup in the 1 NF database
        /// </summary>
        //private DocumentFinder docFinder = null;

        /// <summary>
        /// Helper object for archive state creation in 1NF
        /// </summary>
        //private Archiver1NF archiver1NF = new Archiver1NF();

        /// <summary>
        /// A CSV log that contains all documents from various databases that were
        /// matched against some 1NF documents
        /// </summary>
        private CSVLog csvLogMatchedDocuments = new CSVLog("CSV/MatchedDocs.csv");

        /// <summary>
        /// Internal flag, used for debugging only. In normal operation shoud be
        /// set to FALSE. Setting it to TRUE greatly increases the speed of migration,
        /// but some information is ignored.
        /// </summary>
        private bool testOnly = false;

        /// <summary>
        /// This flag enables import of the new 1NF data columns, added for compatibility
        /// with web-based 1NF report submitting system. This flag is temporary, and should
        /// be removed once all the new data fields are added to the primary 1NF database
        /// </summary>
        private bool enable1NFNewFieldsImport = true;

        #endregion (Member variables)

        #region Construction

        /// <summary>
        /// Creates a Mirator object and associates it with the provided logger object
        /// </summary>
        /// <param name="log">Logger that should be used for debug output</param>
        public Migrator(IMigrationLog log)
        {
            logger = log;

            //docFinder = new DocumentFinder(logger);
        }

        #endregion (Construction)

        #region Interface

        /// <summary>
        /// Connects to SQL Server and all the Firebird databases just to verify that
        /// all connection settings are valid
        /// </summary>
        /// <returns>'True' if connection to all databases was successful</returns>
        public bool TestConnection()
        {
            logger.WriteInfo("Testing connection settings");

            // Connect to SQL Server
            connectionSqlClient = ConnectToSQLServer(GetSQLClientConnectionString());

            if (connectionSqlClient == null)
            {
                return false;
            }

            // Connect to 1NF database
            //connection1NF = ConnectToFirebirdDatabase("1NF", Get1NFConnectionString());

            //if (connection1NF == null)
            //{
            //    return false;
            //}

            // Connect to Balans database
            //connectionBalans = ConnectToFirebirdDatabase("Balans", GetBalansConnectionString());

            //if (connectionBalans == null)
            //{
            //    return false;
            //}

            // Connect to Privatizacia database
            //connectionPrivatizacia = ConnectToFirebirdDatabase("Privatizacia", GetPrivatConnectionString());

            //if (connectionPrivatizacia == null)
            //{
            //    return false;
            //}

            // Connect to the Rosporadjennia database
            //connectionNJF = ConnectToFirebirdDatabase("Rosporadjennia", GetNJFConnectionString());

            //if (connectionNJF == null)
            //{
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// Disconnects from all the databases
        /// </summary>
        public void Disconnect()
        {
            if (connectionSqlClient != null && connectionSqlClient.State == ConnectionState.Open)
            {
                logger.WriteInfo("Disconnecting from SQL Server...");

                connectionSqlClient.Close();
                connectionSqlClient = null;
            }

            //if (connection1NF != null && connection1NF.State == ConnectionState.Open)
            //{
            //    logger.WriteInfo("Disconnecting from 1NF database...");

            //    connection1NF.Close();
            //    connection1NF = null;
            //}

            //if (connectionBalans != null && connectionBalans.State == ConnectionState.Open)
            //{
            //    logger.WriteInfo("Disconnecting from Balans database...");

            //    connectionBalans.Close();
            //    connectionBalans = null;
            //}

            //if (connectionPrivatizacia != null && connectionPrivatizacia.State == ConnectionState.Open)
            //{
            //    logger.WriteInfo("Disconnecting from Privatizacia database...");

            //    connectionPrivatizacia.Close();
            //    connectionPrivatizacia = null;
            //}

            //if (connectionNJF != null && connectionNJF.State == ConnectionState.Open)
            //{
            //    logger.WriteInfo("Disconnecting from Rosporadjennia database...");

            //    connectionNJF.Close();
            //    connectionNJF = null;
            //}
        }

        /// <summary>
        /// Performs the data migration from Firebird to SQL Server
        /// </summary>
        public void PerformMigration()
        {
            try
            {
                DateTime timeMigrationStart = DateTime.Now;

                // Connect to SQL Server
                connectionSqlClient = ConnectToSQLServer(GetSQLClientConnectionString());

                if (connectionSqlClient != null && connectionSqlClient.State == ConnectionState.Open)
                {
                    // Connect to 1NF database
                    //connection1NF = ConnectToFirebirdDatabase("1NF", Get1NFConnectionString());

                    //if (connection1NF != null && connection1NF.State == ConnectionState.Open)
                    //{
                        // Load manual mapping of ZKPO codes that are duplicated in 1NF
                        organizationFinder.ReadManualZKPOMatch(connectionSqlClient);

                        // Erase the destination database
                        logger.WriteInfo("Erasing the destination database");

                        RunScriptFromFile(EraserScriptFileName, connectionSqlClient);

                        // Erase the destination database
                        logger.WriteInfo("Executing the pre-processing script");

                        RunScriptFromFile(PreProcessingScriptFileName, connectionSqlClient);

                        // Migrate 1NF - the primary database
                        //Migrate1NF();

                        // Initialize the objects that provide object lookup and organization lookup
                        logger.WriteInfo("=== Preparing the cache of organizations by ZKPO code and name ===");

                        //organizationFinder.BuildOrganizationCacheFrom1NF(connection1NF, true);

                        logger.WriteInfo("=== Preparing the cache of 1 NF object addresses ===");

                        ObjectFinder.Instance.BuildObjectCacheFromSqlServer(connectionSqlClient);

                        logger.WriteInfo("=== Preparing the cache of 1 NF documents ===");

                        ExecuteStatementInSQLServer("UPDATE documents SET doc_num = RTRIM(LTRIM(doc_num))");

                        //docFinder.RefreshDocumentCache(connectionSqlClient);

                        // Migrate other databases
                        //MigrateRentPayments();
                        //MigrateBalans();
                        //MigrateArenda();
                        //MigrateNJF();
                        //MigratePrivatization();
                        //MigrateAssessment();

                        // Post-processing
                        logger.WriteInfo("=== Post-processing ===");
                        logger.WriteInfo("Executing the post-processing script");

                        RunScriptFromFile(PostProcessingScriptFileName, connectionSqlClient);

                        // Create views for the site
                        logger.WriteInfo("Creating views for the web site");

                        RunScriptFromFile(ViewCreationScriptFileName, connectionSqlClient);

                        // Perform data corrections
                        if (!testOnly)
                        {
                            organizationFinder.BuildOrganizationCacheFromSqlServer(connectionSqlClient, false);
                            ObjectFinder.Instance.BuildObjectCacheFromSqlServer(connectionSqlClient);

                            //archiver1NF.PrepareArchiveObjectList(connection1NF);

                            // PerformDataCorrections(); - Disabled for now

                            // Automatical transfer of objects from one owner to another
                            //PerformAutoTransfers();
                        }
                    //}
                }

                logger.WriteInfo("=== Finished ===");

                // Calculate the total time of migration
                DateTime timeMigrationEnd = DateTime.Now;

                TimeSpan migrationTime = timeMigrationEnd - timeMigrationStart;

                logger.WriteInfo("Total migration time = " + migrationTime.ToString());
            }
            catch (Exception ex)
            {
                logger.WriteInfo("Data migration ended with errors!");
                logger.WriteInfo("Exception: " + ex.Message);
            }
        }

        #endregion (Interface)

        #region Connection string generation

        /// <summary>
        /// Builds a valid Firebird connection string from the given DB conection settings
        /// </summary>
        /// <param name="server">Server name</param>
        /// <param name="database">Database name</param>
        /// <param name="user">User name</param>
        /// <param name="password">User password</param>
        /// <returns>Created connection string</returns>
        private string GetFirebirdConnectionString(string server, string database,
            string user, string password, bool useWin1251Encoding)
        {
            string connString = "Server=" + server + ";" +
                "Database=" + database + ";" +
                "User=" + user + ";" +
                "Password=" + password;

            if (useWin1251Encoding)
            {
                connString += ";Charset=WIN1251";
            }

            return connString;
        }

        /// <summary>
        /// Builds a valid OLEDB connection string from the given DB conection settings
        /// </summary>
        /// <param name="server">Server name</param>
        /// <param name="database">Database name</param>
        /// <param name="user">User name</param>
        /// <param name="password">User password</param>
        /// <returns>Created connection string</returns>
        //private string GetOLEDBConnectionString(string server, string database,
        //    string user, string password)
        //{
        //    return "Provider=SQLOLEDB;" +
        //        "DataSource=" + server + ";" +
        //        "InitialCatalog=" + database + ";" +
        //        "UID=" + user + ";" +
        //        "PWD=" + password;
        //}

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
        /// Returns the correct conection string to the SQL Server database
        /// </summary>
        /// <returns>Connection string</returns>
        private string GetSQLClientConnectionString()
        {
            //return "Data Source=(local);Initial Catalog=GUKV;Integrated Security=True;";
            return GetSQLClientConnectionString(
                Properties.Settings.Default.ConnectionSQLServerServer,
                Properties.Settings.Default.ConnectionSQLServerDatabase,
                Properties.Settings.Default.ConnectionSQLServerUser,
                Properties.Settings.Default.ConnectionSQLServerPassword);
        }

        /// <summary>
        /// Returns the correct conection string to the 1 NF database
        /// </summary>
        /// <returns>Connection string</returns>
        //private string Get1NFConnectionString()
        //{
        //    return GetFirebirdConnectionString(
        //        Properties.Settings.Default.Connection1NFServer,
        //        Properties.Settings.Default.Connection1NFDatabase,
        //        Properties.Settings.Default.Connection1NFUser,
        //        Properties.Settings.Default.Connection1NFPassword,
        //        true);

        //    /*
        //    return GetFirebirdConnectionString(
        //        "gukv_1nf",
        //        "c:\\database\\1nf_reserve\\1nf2.gdb",
        //        Properties.Settings.Default.Connection1NFUser,
        //        Properties.Settings.Default.Connection1NFPassword,
        //        true);
        //    */
        //}

        /// <summary>
        /// Returns the correct conection string to the Balans database
        /// </summary>
        /// <returns>Connection string</returns>
        //private string GetBalansConnectionString()
        //{
        //    return GetFirebirdConnectionString(
        //        Properties.Settings.Default.ConnectionBalansServer,
        //        Properties.Settings.Default.ConnectionBalansDatabase,
        //        Properties.Settings.Default.ConnectionBalansUser,
        //        Properties.Settings.Default.ConnectionBalansPassword,
        //        true);
        //}

        /// <summary>
        /// Returns the correct conection string to the Privatizacia database
        /// </summary>
        /// <returns>Connection string</returns>
        //private string GetPrivatConnectionString()
        //{
        //    return GetFirebirdConnectionString(
        //        Properties.Settings.Default.ConnectionPrivatServer,
        //        Properties.Settings.Default.ConnectionPrivatDatabase,
        //        Properties.Settings.Default.ConnectionPrivatUser,
        //        Properties.Settings.Default.ConnectionPrivatPassword,
        //        false);
        //}

        /// <summary>
        /// Returns the correct conection string to the Rosporadjennia database
        /// </summary>
        /// <returns>Connection string</returns>
        //private string GetNJFConnectionString()
        //{
        //    return GetFirebirdConnectionString(
        //        Properties.Settings.Default.ConnectionNJFServer,
        //        Properties.Settings.Default.ConnectionNJFDatabase,
        //        Properties.Settings.Default.ConnectionNJFUser,
        //        Properties.Settings.Default.ConnectionNJFPassword,
        //        true);
        //}

        #endregion (Connection string generation)

        #region Helper methods

        /// <summary>
        /// Connects to a Firebird database and returns the created connection.
        /// </summary>
        /// <param name="databaseName">Name of the database (for logging purposes only)</param>
        /// <param name="connectionString">Connection string</param>
        /// <returns>The created connection, or NULL if connection failed</returns>
        //private FbConnection ConnectToFirebirdDatabase(string databaseName, string connectionString)
        //{
        //    FbConnection connection = new FbConnection(connectionString);

        //    logger.WriteInfo("Connecting to " + databaseName + " database...");
        //    logger.WriteInfo("Connection string: " + connectionString);

        //    try
        //    {
        //        connection.Open();

        //        logger.WriteInfo("Connected");

        //        return connection;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.WriteError("Cannot conect to database " + databaseName + ". Message: " + ex.Message);

        //        return null;
        //    }
        //}

        /// <summary>
        /// Connects to the SQL Server and returns the created connection.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>The created connection, or NULL if connection failed</returns>
        private SqlConnection ConnectToSQLServer(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            logger.WriteInfo("Connecting to SQL Server using SQL Client...");
            logger.WriteInfo("Connection string: " + connectionString);

            try
            {
                connection.Open();

                logger.WriteInfo("Connected");

                return connection;
            }
            catch (Exception ex)
            {
                logger.WriteError("Cannot conect to SQL Server. Message: " + ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Returns an index of the source table column in the given column mapping
        /// </summary>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="srcFieldName">Name of the column in the source table</param>
        /// /// <param name="srcTableName">Name of the source table - for debugging purposes only</param>
        /// <returns>Index of the specified column within the mapping, or -1 if the column is not found</returns>
        //private int GetSrcFieldIndexFromMapping(Dictionary<string, string> fields, string srcFieldName, string srcTableName)
        //{
        //    int fieldIndex = 0;

        //    foreach (KeyValuePair<string, string> entry in fields)
        //    {
        //        if (entry.Key.ToUpper() == srcFieldName.ToUpper())
        //        {
        //            return fieldIndex;
        //        }

        //        fieldIndex++;
        //    }

        //    // This is not a valid condition - throw an exception
        //    logger.WriteInfo("The field " + srcFieldName + " was not found in the column mapping for " + srcTableName + " table");

        //    throw new MigrationAbortedException();
        //}

        /// <summary>
        /// Returns an index of the destination table column in the given column mapping
        /// </summary>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="destFieldName">Name of the column in the destination table</param>
        /// <returns>Index of the specified column within the mapping, or -1 if the column is not found</returns>
        private int GetDestFieldIndexFromMapping(Dictionary<string, string> fields, string destFieldName)
        {
            int fieldIndex = 0;

            foreach (KeyValuePair<string, string> entry in fields)
            {
                if (entry.Value.ToUpper() == destFieldName.ToUpper())
                {
                    return fieldIndex;
                }

                fieldIndex++;
            }

            return -1;
        }

        /// <summary>
        /// Migrates all the data from the source table (in Firebird)
        /// to the destination table (in SQL Server).
        /// </summary>
        /// <param name="sourceDbConnection">Connection to the source database</param>
        /// <param name="srcTableName">Source table name</param>
        /// <param name="destTableName">Destination table name</param>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="sortSrcField">Name of the "ORDER BY" column in the source table.
        /// This parameter can be an empty string if no sorting is required.</param>
        /// <param name="requiredFields">List of fields in the source table which
        /// are not allowed to be NULL.</param>
        //private void MigrateTable(
        //    FbConnection sourceDbConnection,
        //    string srcTableName,
        //    string destTableName,
        //    Dictionary<string, string> fields,
        //    string sortSrcField,
        //    List<string> requiredFields)
        //{
        //    BulkMigrateTable(sourceDbConnection, srcTableName, destTableName, fields, sortSrcField, requiredFields);

        //    /*
        //    if (Preferences.UseBulkCopy)
        //    {
        //        BulkMigrateTable(sourceDbConnection, srcTableName, destTableName, fields, sortSrcField, requiredFields);
        //    }
        //    else
        //    {
        //        MigrateTableUsingINSERT(sourceDbConnection, srcTableName, destTableName, fields, sortSrcField, requiredFields);
        //    }
        //    */
        //}

        /// <summary>
        /// Migrates all the data from the source table (in Firebird)
        /// to the destination table (in SQL Server) using simple INSERT statements.
        /// </summary>
        /// <param name="sourceDbConnection">Connection to the source database</param>
        /// <param name="srcTableName">Source table name</param>
        /// <param name="destTableName">Destination table name</param>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="sortSrcField">Name of the "ORDER BY" column in the source table.
        /// This parameter can be an empty string if no sorting is required.</param>
        /// <param name="requiredFields">List of fields in the source table which
        /// are not allowed to be NULL.</param>
        //private void MigrateTableUsingINSERT(
        //    FbConnection sourceDbConnection,
        //    string srcTableName,
        //    string destTableName,
        //    Dictionary<string, string> fields,
        //    string sortSrcField,
        //    List<string> requiredFields)
        //{
        //    logger.WriteInfo("Migrating table " + srcTableName + " into table " + destTableName + " using INSERT");

        //    object sortSrcFieldValue = null;
        //    while (true)
        //    {
        //        // Read the data
        //        string querySelect;

        //        if (sortSrcFieldValue == null)
        //            querySelect = GetSelectStatement(srcTableName, fields, sortSrcField, requiredFields, "", 1);
        //        else
        //            querySelect = GetSelectStatement(srcTableName, fields, sortSrcField, requiredFields, string.Format("{0} > {1}", sortSrcField, sortSrcFieldValue), 1);

        //        try
        //        {
        //            using (FbCommand commandSelect = new FbCommand(querySelect, sourceDbConnection))
        //            {
        //                Console.WriteLine("Executing {0}", querySelect);

        //                using (FbDataReader reader = commandSelect.ExecuteReader())
        //                {
        //                    sortSrcFieldValue = null;

        //                    while (reader.Read())
        //                    {
        //                        string queryInsert = GetInsertStatement(destTableName, fields, reader, null);

        //                        sortSrcFieldValue = reader[sortSrcField];

        //                        try
        //                        {
        //                            using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionSqlClient))
        //                            {
        //                                commandInsert.ExecuteNonQuery();
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            ShowSqlErrorMessageDlg(queryInsert, ex.Message);

        //                            // Abort migration correctly
        //                            throw new MigrationAbortedException();
        //                        }
        //                    }

        //                    reader.Close();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex is MigrationAbortedException)
        //            {
        //                throw;
        //            }
        //            {
        //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

        //                // Abort migration correctly
        //                throw new MigrationAbortedException();
        //            }
        //        }

        //        if (sortSrcFieldValue == null || sortSrcFieldValue is DBNull)
        //            break;
        //    }
        //}

        /// <summary>
        /// Migrates all the data from the source table (in Firebird)
        /// to the destination table (in SQL Server) using the BULK copy capabilities.
        /// </summary>
        /// <param name="sourceDbConnection">Connection to the source database</param>
        /// <param name="srcTableName">Source table name</param>
        /// <param name="destTableName">Destination table name</param>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="sortSrcField">Name of the "ORDER BY" column in the source table.
        /// This parameter can be an empty string if no sorting is required.</param>
        /// <param name="requiredFields">List of fields in the source table
        /// which are not allowed to be NULL.</param>
        /// <param name="whereClause">If this parameter is not empty, WHERE clause is generated.</param>
        //private void BulkMigrateTable(
        //    FbConnection sourceDbConnection,
        //    string srcTableName,
        //    string destTableName,
        //    Dictionary<string, string> fields,
        //    string sortSrcField,
        //    List<string> requiredFields,
        //    string whereClause)
        //{
        //    logger.WriteInfo("BULK migrating table " + srcTableName + " into table " + destTableName);

        //    // Read the data
        //    string querySelect = GetSelectStatement(srcTableName, fields, sortSrcField, requiredFields, whereClause);

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, sourceDbConnection))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionSqlClient))
        //                {
        //                    // Initialize the BULK copy class
        //                    bulkCopy.DestinationTableName = "dbo." + destTableName;
        //                    bulkCopy.BatchSize = 1000; // This setting is optimal for migrating BLOBs from Firebird
        //                    bulkCopy.BulkCopyTimeout = 1200;

        //                    // Setup maping between source columns and destination columns
        //                    if (fields != null)
        //                    {
        //                        foreach (KeyValuePair<string, string> entry in fields)
        //                        {
        //                            bulkCopy.ColumnMappings.Add(entry.Key, entry.Value);
        //                        }
        //                    }

        //                    // Write from the source to the destination
        //                    try
        //                    {
        //                        bulkCopy.WriteToServer(reader);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        ShowSqlErrorMessageDlg("<No SQL statement available for BULK insertion>", ex.Message);

        //                        // Abort migration correctly
        //                        throw new MigrationAbortedException();
        //                    }

        //                    // Close the objects
        //                    bulkCopy.Close();
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is MigrationAbortedException)
        //        {
        //            throw;
        //        }
        //        else
        //        {
        //            ShowSqlErrorMessageDlg(querySelect, ex.Message);

        //            // Abort migration correctly
        //            throw new MigrationAbortedException();
        //        }
        //    }
        //}

        /// <summary>
        /// Migrates all the data from the source table (in Firebird)
        /// to the destination table (in SQL Server) using the BULK copy capabilities.
        /// </summary>
        /// <param name="sourceDbConnection">Connection to the source database</param>
        /// <param name="srcTableName">Source table name</param>
        /// <param name="destTableName">Destination table name</param>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="sortSrcField">Name of the "ORDER BY" column in the source table.
        /// This parameter can be an empty string if no sorting is required.</param>
        /// <param name="requiredFields">List of fields in the source table
        /// which are not allowed to be NULL.</param>
        //private void BulkMigrateTable(
        //    FbConnection sourceDbConnection,
        //    string srcTableName,
        //    string destTableName,
        //    Dictionary<string, string> fields,
        //    string sortSrcField,
        //    List<string> requiredFields)
        //{
        //    BulkMigrateTable(sourceDbConnection, srcTableName, destTableName, fields, sortSrcField, requiredFields, "");
        //}

        //private void BulkMigrateDataReader(
        //    string srcTableName,
        //    string destTableName,
        //    IDataReader reader,
        //    Dictionary<string, string> fields)
        //{
        //    logger.WriteInfo("Adapted BULK migrating table " + srcTableName + " into table " + destTableName);

        //    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionSqlClient))
        //    {
        //        // Initialize the BULK copy class
        //        bulkCopy.DestinationTableName = "dbo." + destTableName;
        //        bulkCopy.BatchSize = 1000; // This setting is optimal for migrating BLOBs from Firebird
        //        bulkCopy.BulkCopyTimeout = 1200;

        //        // Setup maping between source columns and destination columns
        //        if (fields != null)
        //        {
        //            foreach (KeyValuePair<string, string> entry in fields)
        //            {
        //                bulkCopy.ColumnMappings.Add(entry.Key, entry.Value);
        //            }
        //        }

        //        // Write from the source to the destination
        //        try
        //        {
        //            bulkCopy.WriteToServer(reader);
        //        }
        //        catch (Exception ex)
        //        {
        //            ShowSqlErrorMessageDlg("<No SQL statement available for BULK insertion>", ex.Message);

        //            // Abort migration correctly
        //            throw new MigrationAbortedException();
        //        }

        //        // Close the objects
        //        bulkCopy.Close();
        //    }
        //}

        /// <summary>
        /// Inserts rows from the provided DataTable into the specified table within SQL Server
        /// </summary>
        /// <param name="destTableName">Name of the destination table in SQL Server</param>
        /// <param name="table">The table that contains data</param>
        private void BulkMigrateDataTable(
            string destTableName,
            DataTable table)
        {
            logger.WriteInfo("BULK migrating DataTable into table " + destTableName);

            // Dump the accumulated data into SQL Server
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionSqlClient))
            {
                // Initialize the BULK copy class
                bulkCopy.DestinationTableName = "dbo." + destTableName;
                bulkCopy.BatchSize = 1000; // This setting is optimal for migrating BLOBs from Firebird
                bulkCopy.BulkCopyTimeout = 1200;

                // Setup maping between source columns and destination columns
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    bulkCopy.ColumnMappings.Add(table.Columns[i].ColumnName, table.Columns[i].ColumnName);
                }

                // Write from the source to the destination
                try
                {
                    bulkCopy.WriteToServer(table);
                }
                catch (Exception ex)
                {
                    ShowSqlErrorMessageDlg("<No SQL statement available for BULK insertion>", ex.Message);

                    // Abort migration correctly
                    throw new MigrationAbortedException();
                }

                bulkCopy.Close();
            }
        }

        /// <summary>
        /// Verifies if the specified table in SQL Server contains a row with the specified ID.
        /// </summary>
        /// <param name="destTableName">Name of table in the SQL Server</param>
        /// <param name="destKeyField">Name of the primary key column within the table</param>
        /// <param name="ID">Value to find in the primary key clumn</param>
        /// <returns>TRUE if the specified value was found</returns>
        //private bool SqlServerTableContainsValue(string destTableName, string destKeyField, object value)
        //{
        //    bool tableContainsID = false;

        //    // Format the value that we need to find
        //    string key = "";

        //    if (value is int)
        //    {
        //        key = ((int)value).ToString();
        //    }
        //    else if (value is string)
        //    {
        //        key = "'" + FormatStringForSQLServer((string)value) + "'";
        //    }
        //    else
        //    {
        //        logger.WriteError("SqlServerTableContainsValue() method can find only an integer or string value. " +
        //            "The type " + value.GetType().FullName + " is not supported.");

        //        // Abort migration correctly
        //        throw new MigrationAbortedException();
        //    }

        //    // Prepare a simple SELECT statement for testing
        //    string querySelect = "SELECT TOP 1 " + destKeyField + " FROM " + destTableName +
        //        " WHERE " + destKeyField + " = " + key;

        //    try
        //    {
        //        using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
        //        {
        //            using (SqlDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                // Check if there are any data in the response
        //                if (reader.Read())
        //                {
        //                    tableContainsID = true;
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowSqlErrorMessageDlg(querySelect, ex.Message);

        //        // Abort migration correctly
        //        throw new MigrationAbortedException();
        //    }

        //    return tableContainsID;
        //}

        /// <summary>
        /// Finds a name of particular dictionary entry by the ID of this dictionary entry
        /// </summary>
        /// <param name="tableName">Name of the table that holds the dictionary</param>
        /// <param name="destKeyField">Name of the primary key field in the table</param>
        /// <param name="destNameField">Name of the 'name' field in the table</param>
        /// <param name="id">ID of the dictionary entry</param>
        /// <returns>Name of the dictionary entry, or 'null' if the entry does not exist</returns>
        private object FindDictionaryEntryNameInSQLServer(
            string tableName,
            string destKeyField,
            string destNameField,
            int id)
        {
            object entryName = null;

            // Prepare a simple SELECT statement for testing
            string querySelect = "SELECT TOP 1 " + destNameField + " FROM " + tableName +
                " WHERE " + destKeyField + " = " + id.ToString();

            try
            {
                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
                {
                    using (SqlDataReader reader = commandSelect.ExecuteReader())
                    {
                        // Check if there are any data in the response
                        if (reader.Read())
                        {
                            entryName = reader.GetValue(0);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);

                // Abort migration correctly
                throw new MigrationAbortedException();
            }

            return entryName;
        }

        /// <summary>
        /// Finds an ID of particular dictionary entry by the name of this dictionary entry
        /// </summary>
        /// <param name="tableName">Name of the table that holds the dictionary</param>
        /// <param name="destKeyField">Name of the primary key field in the table</param>
        /// <param name="destNameField">Name of the 'name' field in the table</param>
        /// <param name="name">Name of the dictionary entry that must be located</param>
        /// <returns>ID of the found dictionary entry, or -1 of the entry with
        /// the given name is not found</returns>
        private int FindDictionaryEntryIdInSQLServer(
            string tableName,
            string destKeyField,
            string destNameField,
            string name)
        {
            int entryId = -1;

            // Prepare a simple SELECT statement for testing
            string querySelect = "SELECT TOP 1 " + destKeyField + " FROM " + tableName +
                " WHERE " + destNameField + " = '" + FormatStringForSQLServer(name) + "'";

            try
            {
                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
                {
                    using (SqlDataReader reader = commandSelect.ExecuteReader())
                    {
                        // Check if there are any data in the response
                        if (reader.Read())
                        {
                            entryId = reader.GetInt32(0);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);

                // Abort migration correctly
                throw new MigrationAbortedException();
            }

            return entryId;
        }

        /// <summary>
        /// Adds a new entry to a dictionary table in the SQL Server
        /// </summary>
        /// <param name="tableName">Name of the table that holds the dictionary</param>
        /// <param name="destKeyField">Name of the primary key field in the table</param>
        /// <param name="destNameField">Name of the 'name' field in the table</param>
        /// <param name="id">Id of the inserted entry</param>
        /// <param name="name">Name of the inserted entry</param>
        //private void AddDictionaryEntryInSQLServer(
        //    string tableName,
        //    string destKeyField,
        //    string destNameField,
        //    int id,
        //    string name)
        //{
        //    // Use an INSERT statement to add a new entry to the dictionary
        //    string queryInsert = "INSERT INTO " + tableName +
        //        " (" + destKeyField + ", " + destNameField + ") VALUES (" +
        //        id.ToString() + ", '" + FormatStringForSQLServer(name) + "')";

        //    // Execute INSERT
        //    try
        //    {
        //        using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionSqlClient))
        //        {
        //            commandInsert.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowSqlErrorMessageDlg(queryInsert, ex.Message);

        //        // Abort migration correctly
        //        throw new MigrationAbortedException();
        //    }
        //}

        /// <summary>
        /// Sets a new value for an existing entry in the SQL Server dictionary table
        /// </summary>
        /// <param name="tableName">Name of the table that holds the dictionary</param>
        /// <param name="destKeyField">Name of the primary key field in the table</param>
        /// <param name="destNameField">Name of the 'name' field in the table</param>
        /// <param name="id">Id of the existing dictionary entry</param>
        /// <param name="name">The new value for the updated entry</param>
        //private void UpdateDictionaryEntryInSQLServer(
        //    string tableName,
        //    string destKeyField,
        //    string destNameField,
        //    int id,
        //    string name)
        //{
        //    // Use an UPDATE statement to change the dictionary entry
        //    string queryUpdate = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase +
        //        ".dbo." + tableName + " SET " + destNameField + " = '" +
        //        FormatStringForSQLServer(name) + "' WHERE " + destKeyField + " = " + id.ToString();

        //    // Execute UPDATE
        //    try
        //    {
        //        using (SqlCommand commandUpdate = new SqlCommand(queryUpdate, connectionSqlClient))
        //        {
        //            commandUpdate.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowSqlErrorMessageDlg(queryUpdate, ex.Message);

        //        // Abort migration correctly
        //        throw new MigrationAbortedException();
        //    }
        //}

        /// <summary>
        /// Deletes an entry from a dictionary table in SQL Server
        /// </summary>
        /// <param name="tableName">Name of the dictionary table</param>
        /// <param name="keyFieldName">Name of the primary key field in the dictionary table</param>
        /// <param name="id">Id of the entry that must be deleted</param>
        //private void DeleteDictionaryEntryInSQLServer(
        //    string tableName, string keyFieldName, int id)
        //{
        //    // Use a DELETE statement to delete an entry in the dictionary
        //    string queryInsert = "DELETE FROM " + tableName + " WHERE " + keyFieldName + " = " + id.ToString();

        //    // Execute DELETE
        //    try
        //    {
        //        using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionSqlClient))
        //        {
        //            commandInsert.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowSqlErrorMessageDlg(queryInsert, ex.Message);

        //        // Abort migration correctly
        //        throw new MigrationAbortedException();
        //    }
        //}

        /// <summary>
        /// Overrides rows in the destination dictionary with the rows from the source
        /// dictionary. If the row from the source dictionary is not found in the
        /// destination dictionary, it is added to the destination dictionary.
        /// </summary>
        /// <param name="sourceDbConnection">Connection to the source database</param>
        /// <param name="srcTableName">Source table name</param>
        /// <param name="destTableName">Destination table name</param>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="srcKeyField">Name of the primary key column in the source table</param>
        /// <param name="destKeyField">Name of the primary key column in the source table</param>
        /// <param name="sortSrcField">Name of the "ORDER BY" column in the source table.
        /// This parameter can be an empty string if no sorting is required.</param>
        /// <param name="requiredFields">List of fields in the source table
        /// which are not allowed to be NULL.</param>
        /// <param name="onlyNewRows">If this flag is set, only new rows from the source
        /// dictionary are added to the destination dictionary. Existing rows in the
        /// destination dictionary are not overridden</param>
        //private void OverrideDictionary(
        //    FbConnection sourceDbConnection,
        //    string srcTableName,
        //    string destTableName,
        //    Dictionary<string, string> fields,
        //    string srcKeyField,
        //    string destKeyField,
        //    string sortSrcField,
        //    List<string> requiredFields,
        //    bool onlyNewRows)
        //{
        //    logger.WriteInfo("Overriding table " + srcTableName + " with the table " + destTableName + " using INSERT");

        //    // Find the index of the source key column
        //    int srcKeyFieldIndex = GetSrcFieldIndexFromMapping(fields, srcKeyField, srcTableName);

        //    // Read the data
        //    string querySelect = GetSelectStatement(srcTableName, fields, sortSrcField, requiredFields);

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, sourceDbConnection))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string queryInsertOrUpdate = "";

        //                    // Check if a row with this ID is present in the destination table
        //                    object sourceRowID = reader.GetValue(srcKeyFieldIndex);

        //                    // We skip the dictionary entries with id "0"
        //                    bool skipRow = false;

        //                    if (sourceRowID is int && (int)sourceRowID == 0)
        //                    {
        //                        skipRow = true;
        //                    }

        //                    if (!skipRow)
        //                    {
        //                        if (SqlServerTableContainsValue(destTableName, destKeyField, sourceRowID))
        //                        {
        //                            // No need to UPDATE if only new rows from the source dictionary should be added
        //                            if (!onlyNewRows)
        //                            {
        //                                queryInsertOrUpdate = GetUpdateStatement(destTableName, fields,
        //                                    destKeyField, sourceRowID, reader, null);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            queryInsertOrUpdate = GetInsertStatement(destTableName, fields, reader, null);
        //                        }

        //                        // Execute INSERT or UPDATE
        //                        try
        //                        {
        //                            if (queryInsertOrUpdate.Length > 0)
        //                            {
        //                                using (SqlCommand command = new SqlCommand(queryInsertOrUpdate, connectionSqlClient))
        //                                {
        //                                    command.ExecuteNonQuery();
        //                                }
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            ShowSqlErrorMessageDlg(queryInsertOrUpdate, ex.Message);

        //                            // Abort migration correctly
        //                            throw new MigrationAbortedException();
        //                        }
        //                    }
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is MigrationAbortedException)
        //        {
        //            throw;
        //        }
        //        else
        //        {
        //            ShowSqlErrorMessageDlg(querySelect, ex.Message);

        //            // Abort migration correctly
        //            throw new MigrationAbortedException();
        //        }
        //    }
        //}

        /// <summary>
        /// Executes the given SQL statement against the SQL Server database
        /// </summary>
        /// <param name="statement">The SQL statement to execute</param>
        private int ExecuteStatementInSQLServer(string statement)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(statement, connectionSqlClient))
                {
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(statement, ex.Message);

                // Abort migration correctly
                throw new MigrationAbortedException();
            }
        }

        /// <summary>
        /// Generates a new ID in 1NF database using the specified GENERATOR
        /// </summary>
        /// <param name="generatorName">Generator name</param>
        /// <param name="transaction">Transaction (may be NULL)</param>
        /// <returns>Generated ID, or -1 in case of any error</returns>
        //private int GenerateId1NFUsingGenerator(string generatorName, FbTransaction transaction)
        //{
        //    int objectId = -1;

        //    string query = "SELECT GEN_ID(" + generatorName + ", 1) FROM RDB$DATABASE";

        //    using (FbCommand command = new FbCommand(query, connection1NF))
        //    {
        //        command.Transaction = transaction;

        //        using (FbDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                if (!reader.IsDBNull(0))
        //                {
        //                    objectId = reader.GetInt32(0);
        //                }
        //            }

        //            reader.Close();
        //        }
        //    }

        //    return objectId;
        //}

        #endregion (Helper methods)

        #region Document lookup

        /*
         * 
        /// <summary>
        /// Tries to find an existing document with the specified properties
        /// </summary>
        /// <param name="docDate">Document date</param>
        /// <param name="docNum">Document number</param>
        /// <param name="docTopic">Document title</param>
        /// <returns>Document ID, or -1 of document is not found</returns>
        private int FindDocumentIn1NF(DateTime docDate, string docNum, string docTopic)
        {
            int docId = -1;

            // First try to find by all three attributes
            string querySelect = "SELECT TOP 1 id, doc_num, doc_date, topic FROM documents WHERE " +
                "(doc_date = '" + FormatDateForSQLServer(docDate) + "') AND " +
                "(RTRIM(LTRIM(doc_num)) = '" + FormatStringForSQLServer(docNum.Trim().ToUpper()) + "')";

            try
            {
                using (SqlCommand command = new SqlCommand(querySelect, connectionSqlClient))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            docId = (int)reader.GetValue(0);

                            string num = reader.IsDBNull(1) ? "" : (string)reader.GetValue(1);
                            string dt = reader.IsDBNull(2) ? "" : ((DateTime)reader.GetValue(2)).ToShortDateString();
                            string topic = reader.IsDBNull(3) ? "" : (string)reader.GetValue(3);

                            // A match found; write it to the log
                            csvLogMatchedDocuments.WriteCell(docNum);
                            csvLogMatchedDocuments.WriteCell(docDate.ToShortDateString());
                            csvLogMatchedDocuments.WriteCell(docTopic);
                            csvLogMatchedDocuments.WriteCell(" = ");
                            csvLogMatchedDocuments.WriteCell(num);
                            csvLogMatchedDocuments.WriteCell(dt);
                            csvLogMatchedDocuments.WriteCell(topic);
                            csvLogMatchedDocuments.WriteEndOfLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);

                // Abort migration correctly
                throw new MigrationAbortedException();
            }

            return docId;
        }

        /// <summary>
        /// Verifiesif document with the given ID exists in SQL Server
        /// </summary>
        /// <param name="documentId">Document ID</param>
        /// <returns>TRUE if a document with the given ID exists</returns>
        private bool DocumentExistsInSQLServer(int documentId)
        {
            bool linkExists = false;

            // Prepare a simple SELECT statement for getting the document record
            string querySelect = "SELECT TOP 1 id FROM documents WHERE id = " + documentId.ToString();

            try
            {
                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
                {
                    using (SqlDataReader reader = commandSelect.ExecuteReader())
                    {
                        // Check if there are any data in the response
                        if (reader.Read())
                        {
                            linkExists = true;
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);

                // Abort migration correctly
                throw new MigrationAbortedException();
            }

            return linkExists;
        }

        /// <summary>
        /// Verifies if the given organization is linked to the given document in SQL Server
        /// </summary>
        /// <param name="organizationId">Organization ID</param>
        /// <param name="documentId">Document ID</param>
        /// <returns>TRUE if the link between organization and document exists</returns>
        private bool DocLinkToOrgExistsInSQLServer(int organizationId, int documentId)
        {
            bool linkExists = false;

            // Prepare a simple SELECT statement for getting the link record
            string querySelect = "SELECT TOP 1 organization_id FROM org_docs WHERE " +
                "organization_id = " + organizationId.ToString() + " AND " +
                "document_id = " + documentId.ToString();

            try
            {
                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
                {
                    using (SqlDataReader reader = commandSelect.ExecuteReader())
                    {
                        // Check if there are any data in the response
                        if (reader.Read())
                        {
                            linkExists = true;
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);

                // Abort migration correctly
                throw new MigrationAbortedException();
            }

            return linkExists;
        }

        /// <summary>
        /// Verifies if the given object is linked to the given document in SQL Server
        /// </summary>
        /// <param name="objectId">Object ID</param>
        /// <param name="documentId">Linked document ID</param>
        /// <param name="masterDocumentId">Master document ID (optional)</param>
        /// <returns>TRUE if the given object is linked to the given document in SQL Server</returns>
        private bool DocLinkToObjExistsInSQLServer(int objectId, int documentId, object masterDocumentId)
        {
            bool relationFound = false;

            // Prepare a simple SELECT statement for getting the link ID
            string querySelect = "";

            if (masterDocumentId is int)
            {
                querySelect = "SELECT TOP 1 id FROM building_docs WHERE " +
                    "building_id = " + objectId.ToString() + " AND " +
                    "document_id = " + documentId.ToString() + " AND " +
                    "master_doc_id = " + ((int)masterDocumentId).ToString();
            }
            else
            {
                querySelect = "SELECT TOP 1 id FROM building_docs WHERE " +
                    "building_id = " + objectId.ToString() + " AND " +
                    "document_id = " + documentId.ToString() + " AND " +
                    "(master_doc_id IS NULL)";
            }

            try
            {
                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
                {
                    using (SqlDataReader reader = commandSelect.ExecuteReader())
                    {
                        // Check if there are any data in the response
                        if (reader.Read())
                        {
                            relationFound = true;
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);

                // Abort migration correctly
                throw new MigrationAbortedException();
            }

            return relationFound;
        }

        /// <summary>
        /// Verifies if link between two documents exists in SQL Server
        /// </summary>
        /// <param name="parentDocId">Parent document ID</param>
        /// <param name="childDocId">Child document ID</param>
        /// <returns>TRUE if the link betwen two documents exists</returns>
        private bool DocLinkToDocExistsInSQLServer(int parentDocId, int childDocId)
        {
            bool linkExists = false;

            // Prepare a simple SELECT statement for getting the link record
            string querySelect = "SELECT TOP 1 id FROM doc_dependencies WHERE " +
                "master_doc_id = " + parentDocId.ToString() + " AND " +
                "slave_doc_id = " + childDocId.ToString();

            try
            {
                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
                {
                    using (SqlDataReader reader = commandSelect.ExecuteReader())
                    {
                        // Check if there are any data in the response
                        if (reader.Read())
                        {
                            linkExists = true;
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowSqlErrorMessageDlg(querySelect, ex.Message);

                // Abort migration correctly
                throw new MigrationAbortedException();
            }

            return linkExists;
        }

        */

        #endregion (Document lookup)

        #region SQL statement generation

        private DateTime CheckDateTime(DateTime dt)
        {
            if (dt.Year < 1900)
            {
                if (dt.Year >= 200 && dt.Year < 300)
                {
                    dt = new DateTime(dt.Year + 2000 - 200, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                }
            }

            return dt;
        }

        /// <summary>
        /// Converts a string constant to the form acceptable by SQL Server.
        /// All ' characters are replaced with the '' sequence.
        /// </summary>
        /// <param name="str">A string to convert</param>
        /// <returns>Converted string</returns>
        private static string FormatStringForSQLServer(string str)
        {
            int quotePos = str.IndexOf('\'');

            if (quotePos < 0)
            {
                // No single quotes in the text; we can use it
                return str;
            }
            else
            {
                string result = "";

                // Find each ' symbol and replace it with ''
                while (quotePos >= 0)
                {
                    if (quotePos > 0)
                    {
                        result += str.Substring(0, quotePos);
                    }

                    result += "''";

                    str = str.Remove(0, quotePos + 1);

                    quotePos = str.IndexOf('\'');
                }

                // Do not forget about the remainder of the string
                result += str;

                return result;
            }
        }

        /// <summary>
        /// Converts the DateTime value to a string that can be accepted by SQL Server
        /// </summary>
        /// <param name="date">The date value</param>
        /// <returns>A string that represents the provided date</returns>
        private static string FormatDateForSQLServer(DateTime date)
        {
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            string year = date.Year.ToString();

            if (date.Year < 1900)
            {
                year = "1900";
            }

            while (month.Length < 2)
            {
                month = "0" + month;
            }

            while (day.Length < 2)
            {
                day = "0" + day;
            }

            return month + "/" + day + "/" + year;
        }

        /// <summary>
        /// Converts the Decimal value to a string that can be accepted by SQL Server
        /// </summary>
        /// <param name="num">An input number</param>
        /// <returns>A string that represents the provided decimal number</returns>
        private static string FormatDecimalForSQLServer(Decimal num)
        {
            string numConverted = num.ToString();

            // If the decimal number is converted with "," separator, replace it with "."
            numConverted = numConverted.Replace(',', '.');

            return numConverted;
        }

        /// <summary>
        /// Converts an arbitrary SQL data type to string, acceptable for SQL Server.
        /// </summary>
        /// <param name="reader">The IDataReader to read the data from</param>
        /// <param name="columnIndex">The column index within the provided IDataReader</param>
        /// <returns>Formatted value</returns>
        private static string FormatColumnValue(IDataReader reader, int columnIndex)
        {
            string formattedValue = "";

            object value = reader.GetValue(columnIndex);

            string dataType = reader.GetDataTypeName(columnIndex).ToUpper();

            if (reader.IsDBNull(columnIndex))
            {
                formattedValue = "NULL";
            }
            else if (dataType == "VARCHAR" || dataType == "CHAR" || dataType == "NVARCHAR" || dataType == "NCHAR")
            {
                formattedValue = "'" + FormatStringForSQLServer(value.ToString()) + "'";
            }
            else if (dataType == "NUMERIC" || dataType == "DECIMAL")
            {
                formattedValue = "'" + FormatDecimalForSQLServer((Decimal)value) + "'";
            }
            else if (dataType == "TIMESTAMP" || dataType == "DATE")
            {
                formattedValue = "'" + FormatDateForSQLServer((DateTime)value) + "'";
            }
            else if (dataType == "BLOB")
            {
                // Convert an array of bytes to an array of characters using the "windows-1251" encoding
                byte[] bytes = value as byte[];

                formattedValue = "'" + FormatStringForSQLServer(Encoding.GetEncoding("windows-1251").GetString(bytes)) + "'";
            }
            else if (value is string) // Sometimes BLOBs are returned as strings
            {
                formattedValue = "'" + FormatStringForSQLServer(value.ToString()) + "'";
            }
            else
            {
                formattedValue = value.ToString();

                // logger.WriteInfo(value.GetType().FullName + "(" + dataType + ") converted as " + formattedValue);
            }

            return formattedValue;
        }

        /// <summary>
        /// Converts an arbitrary value to string, acceptable for SQL Server.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>Formatted value</returns>
        private static string FormatValueForSQLServer(object value)
        {
            string formattedValue = "";

            if (value == null)
            {
                formattedValue = "NULL";
            }
            else if (value is string)
            {
                formattedValue = "'" + FormatStringForSQLServer(value as string) + "'";
            }
            else if (value is double)
            {
                formattedValue = ((double)value).ToString("F",
                    System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            }
            else if (value is decimal)
            {
                formattedValue = ((decimal)value).ToString("F",
                    System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            }
            else if (value is DateTime)
            {
                formattedValue = "'" + FormatDateForSQLServer((DateTime)value) + "'";
            }
            else if (value is byte[])
            {
                // Convert an array of bytes to an array of characters using the "windows-1251" encoding
                byte[] bytes = value as byte[];

                formattedValue = "'" + FormatStringForSQLServer(Encoding.GetEncoding("windows-1251").GetString(bytes)) + "'";
            }
            else
            {
                formattedValue = value.ToString();

                // logger.Write(value.GetType().FullName + " converted as " + formattedValue);
            }

            return formattedValue;
        }

        /// <summary>
        /// Builds a SELECT query string from the provided data.
        /// </summary>
        /// <param name="srcTableName">Source table name</param>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="sortSrcField">Name of the "ORDER BY" column in the source table.
        /// This parameter can be an empty string if no sorting is required.</param>
        /// <returns>The prepared SELECT statement</returns>
        /// <param name="requiredFields">List of fields in the source table
        /// which are not allowed to be NULL.</param>
        /// <param name="whereClause">If this parameter is not empty, WHERE clause is generated.</param>
        public static string GetSelectStatement(string srcTableName, Dictionary<string, string> fields,
            string sortSrcField, List<string> requiredFields, string whereClause = "", int first = 0)
        {
            string srcFieldList = "";

            if (fields != null)
            {
                foreach (KeyValuePair<string, string> entry in fields)
                {
                    if (srcFieldList.Length > 0)
                    {
                        srcFieldList += ", ";
                    }

                    srcFieldList += entry.Key;
                }
            }
            else
            {
                // No fields are specified - select all data
                srcFieldList = "*";
            }

            // Prepare the SELECT query to get all the required columns from the dictionary
            string querySelect = "SELECT "
                + (first > 0 ? "FIRST " + first + " " : "")
                + srcFieldList + " FROM " + srcTableName;

            // Add the WHERE clause
            if (requiredFields != null && requiredFields.Count > 0)
            {
                string srcReqFieldList = "";

                foreach (string requiredFieldName in requiredFields)
                {
                    string whereNotNULL = "(NOT " + requiredFieldName + " IS NULL)";

                    if (srcReqFieldList.Length > 0)
                    {
                        srcReqFieldList += " AND ";
                    }

                    srcReqFieldList += whereNotNULL;
                }

                querySelect += " WHERE " + srcReqFieldList;

                if (whereClause.Length > 0)
                {
                    querySelect += " AND (" + whereClause + ") ";
                }
            }
            else
            {
                if (whereClause.Length > 0)
                {
                    querySelect += " WHERE (" + whereClause + ") ";
                }
            }

            // Add the ORDER BY clause if needed
            if (sortSrcField.Length > 0)
            {
                querySelect += " ORDER BY " + sortSrcField;
            }

            return querySelect;
        }

        /// <summary>
        /// Builds an INSERT query string from the provided data.
        /// </summary>
        /// <param name="destTableName">Destination table name</param>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="reader">The data reader to take the inserted values from</param>
        /// <param name="fieldDataOverride">This map allows caller to override some values
        /// from the IDataReader. A 'key' of this map is a column name in the destination (!) table,
        /// and 'value' is a data to insert into this column.</param>
        /// <returns>The generated INSERT statement</returns>
        public static string GetInsertStatement(
            string destTableName,
            Dictionary<string, string> fields,
            IDataReader reader,
            Dictionary<string, object> fieldDataOverride)
        {
            // Format the list of fields that will be updated
            string destFieldList = "";

            foreach (KeyValuePair<string, string> entry in fields)
            {
                if (entry.Key.Length > 0 && entry.Value.Length > 0)
                {
                    if (destFieldList.Length > 0)
                    {
                        destFieldList += ", ";
                    }

                    destFieldList += entry.Value;
                }
            }

            // Format the data row for INSERT statement
            string dataList = "";

            Dictionary<string, string>.ValueCollection.Enumerator valueEnumerator = fields.Values.GetEnumerator();

            for (int columnIndex = 0; columnIndex < fields.Count; columnIndex++)
            {
                // Get the name of the current column in the destination table
                if (!valueEnumerator.MoveNext())
                {
                    break;
                }

                if (valueEnumerator.Current.Length > 0)
                {
                    // Check if data for this column must be overridden
                    object dataToOverride = null;
                    bool overrideValue = false;

                    if (fieldDataOverride != null && fieldDataOverride.ContainsKey(valueEnumerator.Current))
                    {
                        fieldDataOverride.TryGetValue(valueEnumerator.Current, out dataToOverride);
                        overrideValue = true;
                    }

                    // Append the value to the list
                    if (dataList.Length > 0)
                    {
                        dataList += ", ";
                    }

                    if (overrideValue)
                    {
                        // We support only integer and string data for an override
                        if (dataToOverride == null)
                        {
                            dataList += "NULL";
                        }
                        else if (dataToOverride is int)
                        {
                            dataList += ((int)dataToOverride).ToString();
                        }
                        else if (dataToOverride is string)
                        {
                            dataList += "'" + FormatStringForSQLServer((string)dataToOverride) + "'";
                        }
                        else
                        {
                            // Abort migration correctly
                            throw new MigrationAbortedException("GetInsertStatement() method support only integer" +
                                " and string data for the data override. The type " +
                                dataToOverride.GetType().FullName + " is not supported.");
                        }
                    }
                    else
                    {
                        dataList += FormatColumnValue(reader, columnIndex);
                    }
                }
            }

            // Prepare and execute the INSERT statement
            string queryInsert = "INSERT INTO " + Properties.Settings.Default.ConnectionSQLServerDatabase +
                ".dbo." + destTableName + " (" + destFieldList + ") VALUES (" + dataList + ")";

            return queryInsert;
        }

        /// <summary>
        /// Generates an INSERT statement for key-value pairs where 'key' is the name
        /// of column, and 'value' is the column value.
        /// </summary>
        /// <param name="destTableName">Name of the destination table</param>
        /// <param name="values">The map of column values, where 'key' is the name
        /// of column, and 'value' is the column value</param>
        /// <returns>The generated INSERT statement</returns>
        public static string GetInsertStatement(
            string destTableName,
            Dictionary<string, object> values)
        {
            // Format the list of fields that will be updated, and the list of values
            string fieldList = "";
            string valueList = "";

            foreach (KeyValuePair<string, object> entry in values)
            {
                if (entry.Key.Length > 0)
                {
                    if (fieldList.Length > 0)
                    {
                        fieldList += ", ";
                        valueList += ", ";
                    }

                    fieldList += entry.Key;
                    valueList += FormatValueForSQLServer(entry.Value);
                }
            }

            // Prepare and execute the INSERT statement
            string queryInsert = "INSERT INTO " + Properties.Settings.Default.ConnectionSQLServerDatabase +
                ".dbo." + destTableName + " (" + fieldList + ") VALUES (" + valueList + ")";

            return queryInsert;
        }

        /// <summary>
        /// Generates an UPDATE statement for key-value pairs where 'key' is the name
        /// of column, and 'value' is the column value.
        /// </summary>
        /// <param name="destTableName">Name of the destination table</param>
        /// <param name="values">The map of column values, where 'key' is the name
        /// of column, and 'value' is the column value</param>
        /// <param name="destKeyField">Name of the primary key column in the destination table</param>
        /// <param name="destKeyValue">Value of the primary key for the row that must be updated</param>
        /// <returns>The generated UPDATE statement</returns>
        //public static string GetUpdateStatement(
        //    string destTableName,
        //    Dictionary<string, object> values,
        //    string destKeyField,
        //    object destKeyValue)
        //{
        //    // Format the value of the destination table key
        //    string key = "";

        //    if (destKeyValue is int)
        //    {
        //        key = ((int)destKeyValue).ToString();
        //    }
        //    else if (destKeyValue is string)
        //    {
        //        key = "'" + FormatStringForSQLServer((string)destKeyValue) + "'";
        //    }
        //    else
        //    {
        //        // Abort migration correctly
        //        throw new MigrationAbortedException("GetUpdateStatement() method can update the table only by" +
        //            " an integer or string key value. The type " +
        //            destKeyValue.GetType().FullName + " is not supported.");
        //    }

        //    // Format the UPDATE statement
        //    string queryUpdate = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase +
        //        ".dbo." + destTableName + " SET ";

        //    bool commaRequired = false;

        //    foreach (KeyValuePair<string, object> entry in values)
        //    {
        //        string destFieldName = entry.Key;

        //        if (destFieldName.Length > 0)
        //        {
        //            // We are not going to update the primary key; ignore it for the UPDATE statement
        //            if (destFieldName.ToUpper() != destKeyField.ToUpper())
        //            {
        //                string formattedValue = FormatValueForSQLServer(entry.Value);

        //                // Append the formatted value to the UPDATE query
        //                if (commaRequired)
        //                {
        //                    queryUpdate += ", ";
        //                }
        //                else
        //                {
        //                    commaRequired = true;
        //                }

        //                queryUpdate += destFieldName + " = " + formattedValue;
        //            }
        //        }
        //    }

        //    queryUpdate += " WHERE " + destKeyField + " = " + key;

        //    return queryUpdate;
        //}

        /// <summary>
        /// Builds an UPDATE query string from the provided data.
        /// </summary>
        /// <param name="destTableName">Destination table name</param>
        /// <param name="fields">The map of column names, where key is a column name
        /// in the source table, and value is a column name in the destination table</param>
        /// <param name="destKeyField">Name of the primary key column in the destination table</param>
        /// <param name="destKeyValue">Value of the primary key for the row that must be updated</param>
        /// <param name="reader">The data reader to take the UPDATE values from</param>
        /// <param name="fieldDataOverride">This map allows caller to override some values
        /// from the IDataReader. A 'key' of this map is a column name in the destination (!) table,
        /// and 'value' is a data to insert into this column.</param>
        /// <returns>The generated UPDATE statement</returns>
        //public static string GetUpdateStatement(
        //    string destTableName,
        //    Dictionary<string, string> fields,
        //    string destKeyField,
        //    object destKeyValue,
        //    IDataReader reader,
        //    Dictionary<string, object> fieldDataOverride)
        //{
        //    // Format the value of the destination table key
        //    string key = "";

        //    if (destKeyValue is int)
        //    {
        //        key = ((int)destKeyValue).ToString();
        //    }
        //    else if (destKeyValue is string)
        //    {
        //        key = "'" + FormatStringForSQLServer((string)destKeyValue) + "'";
        //    }
        //    else
        //    {
        //        // Abort migration correctly
        //        throw new MigrationAbortedException("GetUpdateStatement() method can update the table only" +
        //            " by an integer or string key value. The type " +
        //            destKeyValue.GetType().FullName + " is not supported.");
        //    }

        //    // Format the UPDATE statement
        //    string queryUpdate = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase +
        //        ".dbo." + destTableName + " SET ";

        //    bool commaRequired = false;

        //    for (int fieldIndex = 0; fieldIndex < reader.FieldCount; fieldIndex++)
        //    {
        //        // Check if this field should be included into the UPDATE statement
        //        string fieldName = reader.GetName(fieldIndex);

        //        if (fields.ContainsKey(fieldName))
        //        {
        //            string destFieldName = fields[fieldName];

        //            if (destFieldName.Length > 0)
        //            {
        //                // We are not going to update the primary key; ignore it for the UPDATE statement
        //                if (destFieldName.ToUpper() != destKeyField.ToUpper())
        //                {
        //                    string formattedValue = "";

        //                    // Check if data for this column must be overridden
        //                    object dataToOverride = null;
        //                    bool overrideValue = false;

        //                    if (fieldDataOverride != null && fieldDataOverride.ContainsKey(destFieldName))
        //                    {
        //                        fieldDataOverride.TryGetValue(destFieldName, out dataToOverride);
        //                        overrideValue = true;
        //                    }

        //                    if (overrideValue)
        //                    {
        //                        // We support only integer and string data for an override
        //                        if (dataToOverride == null)
        //                        {
        //                            formattedValue = "NULL";
        //                        }
        //                        else if (dataToOverride is int)
        //                        {
        //                            formattedValue = ((int)dataToOverride).ToString();
        //                        }
        //                        else if (dataToOverride is string)
        //                        {
        //                            formattedValue = "'" + FormatStringForSQLServer((string)dataToOverride) + "'";
        //                        }
        //                        else
        //                        {
        //                            // Abort migration correctly
        //                            throw new MigrationAbortedException("GetUpdateStatement() method support only integer " +
        //                                "and string data for the data override. The type " +
        //                                dataToOverride.GetType().FullName + " is not supported.");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        formattedValue = FormatColumnValue(reader, fieldIndex);
        //                    }

        //                    // Append the formatted value to the UPDATE query
        //                    if (commaRequired)
        //                    {
        //                        queryUpdate += ", ";
        //                    }
        //                    else
        //                    {
        //                        commaRequired = true;
        //                    }

        //                    queryUpdate += destFieldName + " = " + formattedValue;
        //                }
        //            }
        //        }
        //    }

        //    queryUpdate += " WHERE " + destKeyField + " = " + key;

        //    return queryUpdate;
        //}

        #endregion (SQL statement generation)

        #region SQL script execution methods

        /// <summary>
        /// Executes an SQL script from the specified file against the destination database.
        /// </summary>
        /// <param name="fileName">Name of the file that contains the SQL script</param>
        /// <param name="connection">Connection to the destination database</param>
        private void RunScriptFromFile(string fileName, SqlConnection connection)
        {
            string scriptFileName = ScriptFolder + fileName;

            if (!File.Exists(scriptFileName))
            {
                scriptFileName = fileName;
            }

            if (!File.Exists(scriptFileName))
            {
                logger.WriteError("Cannot find file " + fileName);

                // Abort migration correctly
                throw new MigrationAbortedException();
            }

            logger.WriteInfo("Running SQL script from the file " + scriptFileName);

            string scriptText = File.ReadAllText(scriptFileName);


            // The SqlCommand object can not handle the GO statement,
            // so we need to split the script and execute each part individually
            string[] separators = new string[] { "\nGO\n", "\rGO\r", "\r\nGO\r\n" };

            string[] statements = scriptText.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < statements.Length; i++)
            {
                string statement = statements[i];

                try
                {
                    using (SqlCommand command = new SqlCommand(statement, connection))
                    {
                        command.CommandTimeout = 600; // 10 minutes
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    ShowSqlErrorMessageDlg(statement, ex.Message);

                    // Abort migration correctly
                    throw new MigrationAbortedException();
                }
            }
        }

        #endregion (SQL script execution methods)

        #region Financial coefficient migration

        /*
        /// <summary>
        /// Imports user-defined financial coefficients into SQL Server database
        /// </summary>
        private void ImportFinCoefficients()
        {
            try
            {
                int maxIndustryPeriod = ImportFinCoefByIndustry();
                int maxOrgPeriod = ImportFinCoefByOrganization();

                // Get the maximum period ID from al periods
                string query = "SELECT MAX(id) FROM fin_report_periods";
                int maxPeriod = -1;

                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            maxPeriod = reader.GetInt32(0);
                        }

                        reader.Close();
                    }
                }

                if (maxPeriod > 0)
                {
                    // Propagate the coefficients to the later periods
                    for (int period = maxIndustryPeriod + 1; period <= maxPeriod; period++)
                    {
                        // Check if period ID is valid
                        bool periodValid = false;

                        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM fin_report_periods WHERE id = @pid", connectionSqlClient))
                        {
                            cmd.Parameters.Add(new SqlParameter("pid", period));

                            using (SqlDataReader r = cmd.ExecuteReader())
                            {
                                periodValid = r.Read();
                                r.Close();
                            }
                        }

                        if (periodValid)
                        {
                            query = @"INSERT INTO fin_budget_rate_by_industry (period_id, old_industry_id, payments_rate)
                            SELECT @per_to AS 'period_id', old_industry_id, payments_rate FROM fin_budget_rate_by_industry WHERE period_id = @per_from";

                            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
                            {
                                cmd.Parameters.Add(new SqlParameter("per_from", maxIndustryPeriod));
                                cmd.Parameters.Add(new SqlParameter("per_to", period));

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    for (int period = maxOrgPeriod + 1; period <= maxPeriod; period++)
                    {
                        // Check if period ID is valid
                        bool periodValid = false;

                        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM fin_report_periods WHERE id = @pid", connectionSqlClient))
                        {
                            cmd.Parameters.Add(new SqlParameter("pid", period));

                            using (SqlDataReader r = cmd.ExecuteReader())
                            {
                                periodValid = r.Read();
                                r.Close();
                            }
                        }

                        if (periodValid)
                        {
                            query = @"INSERT INTO fin_budget_rate_by_org (period_id, organization_id, payments_rate)
                            SELECT @per_to AS 'period_id', organization_id, payments_rate FROM fin_budget_rate_by_org WHERE period_id = @per_from";

                            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
                            {
                                cmd.Parameters.Add(new SqlParameter("per_from", maxOrgPeriod));
                                cmd.Parameters.Add(new SqlParameter("per_to", period));

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteInfo("Error: could not import financial coefficients");
                logger.WriteInfo("Error Message: " + ex.Message);
            }
        }

        private int ImportFinCoefByOrganization()
        {
            int maxProcessedPeriod = -1;

            char[] separators = new char[] { ';' };
            DataTable table = new DataTable();
            object[] rowData = new object[3];

            table.Columns.Add("period_id", typeof(int));
            table.Columns.Add("organization_id", typeof(int));
            table.Columns.Add("payments_rate", typeof(decimal));

            // Find all files that match the specified mask
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder2))
            {
                dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + DataFolder2);
            }
            else if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder1))
            {
                dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + DataFolder1);
            }

            FileInfo[] files = dirInfo.GetFiles(FinOrgCoefFileMask);

            foreach (FileInfo fi in files)
            {
                // Get the period code from file name
                int periodCode = 0;
                int firstDotPos = fi.Name.IndexOf('.');
                int lastDotPos = fi.Name.LastIndexOf('.');

                if (firstDotPos > 0 && lastDotPos > firstDotPos + 1)
                {
                    periodCode = int.Parse(fi.Name.Substring(firstDotPos + 1, lastDotPos - firstDotPos - 1));
                }

                // Read the file into temporary DataTable
                if (periodCode > 0)
                {
                    // Save the maximum processed period
                    if (maxProcessedPeriod < periodCode)
                    {
                        maxProcessedPeriod = periodCode;
                    }

                    using (StreamReader reader = new StreamReader(fi.FullName, Encoding.ASCII))
                    {
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
                                decimal koef = decimal.Parse(parts[2]);
                                bool forceCreate = false;

                                int organizationId = organizationFinder.GetOrgIdByZKPO(parts[0], "", true, out forceCreate);

                                if (organizationId > 0)
                                {
                                    // Save the row value
                                    rowData[0] = periodCode;
                                    rowData[1] = organizationId;
                                    rowData[2] = koef / 100m;

                                    table.Rows.Add(rowData);
                                }
                            }
                        }

                        reader.Close();
                    }
                }
            }

            // Dump the values into specified table
            BulkMigrateDataTable("fin_budget_rate_by_org", table);

            return maxProcessedPeriod;
        }

        private int ImportFinCoefByIndustry()
        {
            int maxProcessedPeriod = -1;

            char[] separators = new char[] { ';' };
            DataTable table = new DataTable();
            object[] rowData = new object[4];

            table.Columns.Add("period_id", typeof(int));
            table.Columns.Add("old_industry_id", typeof(int));
            table.Columns.Add("old_occupation_id", typeof(int));
            table.Columns.Add("payments_rate", typeof(decimal));

            // Find all files that match the specified mask
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder2))
            {
                dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + DataFolder2);
            }
            else if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder1))
            {
                dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + DataFolder1);
            }

            FileInfo[] files = dirInfo.GetFiles(FinIndustryCoefFileMask);

            foreach (FileInfo fi in files)
            {
                // Get the period code from file name
                int periodCode = 0;
                int firstDotPos = fi.Name.IndexOf('.');
                int lastDotPos = fi.Name.LastIndexOf('.');

                if (firstDotPos > 0 && lastDotPos > firstDotPos + 1)
                {
                    periodCode = int.Parse(fi.Name.Substring(firstDotPos + 1, lastDotPos - firstDotPos - 1));
                }

                // Read the file into temporary DataTable
                if (periodCode > 0)
                {
                    // Save the maximum processed period
                    if (maxProcessedPeriod < periodCode)
                    {
                        maxProcessedPeriod = periodCode;
                    }

                    using (StreamReader reader = new StreamReader(fi.FullName, Encoding.ASCII))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine().Trim();

                            if (line.Length == 0)
                            {
                                break;
                            }

                            string[] parts = line.Split(separators);

                            if (parts.Length > 3 && parts[0].Length > 0 && parts[1].Length > 0 && parts[3].Length > 0)
                            {
                                int industryCode = int.Parse(parts[0]);
                                int occupationCode = int.Parse(parts[1]);
                                decimal koef = decimal.Parse(parts[3]);

                                if (industryCode > 0)
                                {
                                    // Save the row value
                                    rowData[0] = periodCode;
                                    rowData[1] = industryCode;
                                    rowData[2] = occupationCode;
                                    rowData[3] = koef / 100m;

                                    table.Rows.Add(rowData);
                                }
                            }
                        }

                        reader.Close();
                    }
                }
            }

            // Dump the values into specified table
            BulkMigrateDataTable("fin_budget_rate_by_industry", table);

            return maxProcessedPeriod;
        }
        */

        private void ImportFinCoefficients()
        {
            OrganizationFinder orgFinderSql = new OrganizationFinder();

            logger.WriteInfo("=== Updating the cache of organizations by ZKPO code and name ===");
            orgFinderSql.BuildOrganizationCacheFromSqlServer(connectionSqlClient, false);

            // Determine the current period
            int periodId = -1;
            string query = "SELECT TOP 1 id FROM fin_report_periods WHERE ((is_deleted IS NULL) OR (is_deleted = 0)) AND (period_start <= GETDATE()) ORDER BY period_start DESC, id DESC";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read() && ! reader.IsDBNull(0))
                    {
                        periodId = reader.GetInt32(0);
                    }

                    reader.Close();
                }
            }

            if (periodId > 0)
            {
                // Delete all previously imported coefficients for this period
                using (SqlCommand cmd = new SqlCommand("DELETE FROM fin_budget_rate_by_org WHERE period_id = @pid", connectionSqlClient))
                {
                    cmd.Parameters.Add(new SqlParameter("pid", periodId));
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand("DELETE FROM fin_budget_rate_by_industry WHERE period_id = @pid", connectionSqlClient))
                {
                    cmd.Parameters.Add(new SqlParameter("pid", periodId));
                    cmd.ExecuteNonQuery();
                }

                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder2))
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder2 + Properties.Resources.FinCoefWordFileName1))
                    {
                        ImportFinCoefFromWord(AppDomain.CurrentDomain.BaseDirectory + DataFolder2 + Properties.Resources.FinCoefWordFileName1, periodId, orgFinderSql);
                    }

                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder2 + Properties.Resources.FinCoefWordFileName2))
                    {
                        ImportFinCoefFromWord(AppDomain.CurrentDomain.BaseDirectory + DataFolder2 + Properties.Resources.FinCoefWordFileName2, periodId, orgFinderSql);
                    }
                }
                else if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder1))
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder1 + Properties.Resources.FinCoefWordFileName1))
                    {
                        ImportFinCoefFromWord(AppDomain.CurrentDomain.BaseDirectory + DataFolder1 + Properties.Resources.FinCoefWordFileName1, periodId, orgFinderSql);
                    }

                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + DataFolder1 + Properties.Resources.FinCoefWordFileName2))
                    {
                        ImportFinCoefFromWord(AppDomain.CurrentDomain.BaseDirectory + DataFolder1 + Properties.Resources.FinCoefWordFileName2, periodId, orgFinderSql);
                    }
                }

                // Propagate the same coefficients to well-known periods of 2013
                if (periodId > 82)
                    PropagateFinCoefs(periodId, 82);

                if (periodId > 92)
                    PropagateFinCoefs(periodId, 92);

                if (periodId > 93)
                    PropagateFinCoefs(periodId, 93);

                if (periodId > 94)
                    PropagateFinCoefs(periodId, 94);

                // Get the maximum period ID from al periods
                query = "SELECT MAX(id) FROM fin_report_periods";
                int maxPeriod = -1;

                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            maxPeriod = reader.GetInt32(0);
                        }

                        reader.Close();
                    }
                }

                if (maxPeriod > periodId)
                {
                    // Propagate the coefficients to the later periods
                    for (int period = periodId + 1; period <= maxPeriod; period++)
                    {
                        PropagateFinCoefs(periodId, period);
                    }
                }
            }
        }

        private void PropagateFinCoefs(int periodIdFrom, int periodIdTo)
        {
            if (periodIdFrom == periodIdTo)
                return;

            // Check if period ID is valid
            bool periodValid = false;

            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM fin_report_periods WHERE id = @pid", connectionSqlClient))
            {
                cmd.Parameters.Add(new SqlParameter("pid", periodIdTo));

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    periodValid = r.Read();
                    r.Close();
                }
            }

            if (periodValid)
            {
                // Delete all previously imported coefficients for this period
                using (SqlCommand cmd = new SqlCommand("DELETE FROM fin_budget_rate_by_org WHERE period_id = @pid", connectionSqlClient))
                {
                    cmd.Parameters.Add(new SqlParameter("pid", periodIdTo));
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand("DELETE FROM fin_budget_rate_by_industry WHERE period_id = @pid", connectionSqlClient))
                {
                    cmd.Parameters.Add(new SqlParameter("pid", periodIdTo));
                    cmd.ExecuteNonQuery();
                }

                // Copy the coefficients from the source period
                string query = @"INSERT INTO fin_budget_rate_by_industry (period_id, old_industry_id, payments_rate)
                    SELECT @per_to AS 'period_id', old_industry_id, payments_rate FROM fin_budget_rate_by_industry WHERE period_id = @per_from";

                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
                {
                    cmd.Parameters.Add(new SqlParameter("per_from", periodIdFrom));
                    cmd.Parameters.Add(new SqlParameter("per_to", periodIdTo));

                    cmd.ExecuteNonQuery();
                }

                query = @"INSERT INTO fin_budget_rate_by_org (period_id, organization_id, payments_rate)
                    SELECT @per_to AS 'period_id', organization_id, payments_rate FROM fin_budget_rate_by_org WHERE period_id = @per_from";

                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
                {
                    cmd.Parameters.Add(new SqlParameter("per_from", periodIdFrom));
                    cmd.Parameters.Add(new SqlParameter("per_to", periodIdTo));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ImportFinCoefFromWord(string fileName, int periodId, OrganizationFinder orgFinderSql)
        {
            // Create the Word application
            Microsoft.Office.Interop.Word._Application app = new Microsoft.Office.Interop.Word.Application();

            app.Visible = false;

            try
            {
                // Open the source document
                Microsoft.Office.Interop.Word._Document doc = app.Documents.Open(fileName);

                if (doc != null)
                {
                    try
                    {
                        DataTable table = new DataTable();
                        object[] rowData = new object[3];

                        table.Columns.Add("period_id", typeof(int));
                        table.Columns.Add("organization_id", typeof(int));
                        table.Columns.Add("payments_rate", typeof(decimal));

                        for (int n = 1; n <= doc.Tables.Count; n++)
                        {
                            ImportFinCoefFromTable(doc.Tables[n], table, rowData, periodId, orgFinderSql);
                        }

                        // Dump the values into specified table
                        BulkMigrateDataTable("fin_budget_rate_by_org", table);
                    }
                    finally
                    {
                        doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
                    }
                }
            }
            catch (Exception)
            {
                // Okay...
            }

            app.Quit();
        }

        private void ImportFinCoefFromTable(Microsoft.Office.Interop.Word.Table wordTable, DataTable dataTable, object[] rowData, int periodId, OrganizationFinder orgFinderSql)
        {
            if (wordTable.Columns.Count > 1)
            {
                for (int nRow = 1; nRow <= wordTable.Rows.Count; nRow++)
                {
                    try
                    {
                        string cell1text = wordTable.Cell(nRow, 1).Range.Text;
                        string cell2text = wordTable.Cell(nRow, 2).Range.Text;

                        int pos = cell1text.IndexOf(Properties.Resources.FinCoefDocFileZKPOPattern);

                        if (pos >= 0)
                        {
                            string zkpo = "";

                            // Find a first digit after the pattern
                            pos += Properties.Resources.FinCoefDocFileZKPOPattern.Length;

                            while (pos < cell1text.Length && !char.IsDigit(cell1text[pos]))
                            {
                                pos++;
                            }

                            // Dump all digits to a separate string
                            while (pos < cell1text.Length && char.IsDigit(cell1text[pos]))
                            {
                                zkpo += cell1text[pos];
                                pos++;
                            }

                            if (zkpo.Length > 0)
                            {
                                // Get the coefficient (an integer number from the second cell)
                                string coefStr = "";

                                for (int i = 0; i < cell2text.Length; i++)
                                {
                                    if (char.IsDigit(cell2text[i]))
                                    {
                                        coefStr += cell2text[i];
                                    }
                                }

                                if (coefStr.Length > 0)
                                {
                                    decimal koef = decimal.Parse(coefStr);
                                    bool forceCreate = false;

                                    int organizationId = orgFinderSql.GetOrgIdByZKPO(zkpo, "", true, out forceCreate);

                                    if (organizationId > 0)
                                    {
                                        // Save the row value
                                        rowData[0] = periodId;
                                        rowData[1] = organizationId;
                                        rowData[2] = koef / 100m;

                                        dataTable.Rows.Add(rowData);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Some cells in the row are missing; okay...
                    }
                }
            }
        }

        #endregion (Financial coefficient migration)

        #region Debugging

        /// <summary>
        /// Displays an error message window for an invalid SQL statement
        /// </summary>
        /// <param name="sqlStatement">The SQL statement that caused an error</param>
        /// <param name="errorMessage">The error message</param>
        private void ShowSqlErrorMessageDlg(string sqlStatement, string errorMessage)
        {
            // Write a log message
            logger.WriteSQLError(sqlStatement, errorMessage);
        }

        #endregion (Debugging)

        #region Email notifications



        #endregion (Email notifications)
    }
}
