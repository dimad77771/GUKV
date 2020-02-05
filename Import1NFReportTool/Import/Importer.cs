using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace GUKV.DataMigration
{
    public class Importer
    {
        #region Nested classes

        /// <summary>
        /// Information about particular rent agreement
        /// </summary>
        private class ArendaAgreementInfo
        {
            /// <summary>
            /// ID of the Arenda object in 1NF
            /// </summary>
            public int arendaId = 0;

            /// <summary>
            /// Agreement number
            /// </summary>
            public object agreementNum = "";

            /// <summary>
            /// Agreement date
            /// </summary>
            public object agreementDate = DateTime.MinValue;

            /// <summary>
            /// Giver organization Id
            /// </summary>
            public object orgGiverId = 0;

            /// <summary>
            /// Renter organization Id
            /// </summary>
            public object orgRenterId = 0;

            /// <summary>
            /// Balance holder organization Id
            /// </summary>
            public object orgBalansId = 0;

            /// <summary>
            /// Building Id
            /// </summary>
            public object buildingId = 0;

            /// <summary>
            /// Rent start date
            /// </summary>
            public object rentStartDate = null;

            /// <summary>
            /// Rent finish date
            /// </summary>
            public object rentFinishDate = null;

            /// <summary>
            /// Rented square
            /// </summary>
            public object rentSquare = null;

            /// <summary>
            /// Indicates that rent agreement is marked as deleted
            /// </summary>
            public bool deleted = false;

            /// <summary>
            /// Default constructor
            /// </summary>
            public ArendaAgreementInfo()
            {
            }

            /// <summary>
            /// Makes sure that all properties have comparable values
            /// </summary>
            public void ValidateProperties()
            {
                if (agreementNum is string)
                {
                    string num = ((string)agreementNum).Trim();

                    if (num.Length > 0)
                    {
                        agreementNum = num.ToUpper();
                    }
                    else
                    {
                        agreementNum = null;
                    }
                }

                if (agreementDate is DateTime)
                {
                    agreementDate = ((DateTime)agreementDate).Date;
                }

                if (orgRenterId is int && (int)orgRenterId < 1)
                {
                    orgRenterId = null;
                }

                if (orgGiverId is int && (int)orgGiverId < 1)
                {
                    orgGiverId = null;
                }

                if (orgBalansId is int && (int)orgBalansId < 1)
                {
                    orgBalansId = null;
                }

                if (rentSquare is decimal && (decimal)rentSquare <= 0)
                {
                    rentSquare = null;
                }
            }

            /// <summary>
            /// Performs matching of another ArendaAgreementInfo object to this object
            /// </summary>
            /// <param name="info">Object to compare</param>
            /// <returns>TRUE if matching was successful</returns>
            public bool IsMatch(ArendaAgreementInfo info)
            {
                bool agreementNumOK = (agreementNum == null) || (info.agreementNum == null) ||
                    (agreementNum is string && info.agreementNum is string && (string)agreementNum == (string)info.agreementNum);

                bool agreementDateOK = (agreementDate == null) || (info.agreementDate == null) ||
                    (agreementDate is DateTime && info.agreementDate is DateTime && (DateTime)agreementDate == (DateTime)info.agreementDate);

                bool renterOK =
                    orgRenterId is int && info.orgRenterId is int && (int)orgRenterId == (int)info.orgRenterId;

                bool rentStartOK =
                    rentStartDate is DateTime && info.rentStartDate is DateTime && (DateTime)rentStartDate == (DateTime)info.rentStartDate;

                bool squareOK =
                    rentSquare is decimal && info.rentSquare is decimal && (decimal)rentSquare == (decimal)info.rentSquare;

                return agreementNumOK && agreementDateOK && renterOK && rentStartOK && squareOK;
            }
        }

        /// <summary>
        /// Information about particular object on balans
        /// </summary>
        public class BalansObjectInfo
        {
            /// <summary>
            /// ID of the Balans object in 1NF
            /// </summary>
            public int balansId = 0;

            /// <summary>
            /// Organization Id
            /// </summary>
            public object orgId = 0;

            /// <summary>
            /// Building Id
            /// </summary>
            public object buildingId = 0;

            /// <summary>
            /// Object total square
            /// </summary>
            public object totalSquare = null;

            /// <summary>
            /// Object non-habitable square
            /// </summary>
            // public object nonHabitSquare = null;

            /// <summary>
            /// Purpose group ID
            /// </summary>
            public object purposeGroupId = null;

            /// <summary>
            /// Purpose ID
            /// </summary>
            public object purposeId = null;

            /// <summary>
            /// Purpose as a string
            /// </summary>
            public object purposeStr = null;

            /// <summary>
            /// Floors
            /// </summary>
            public object floors = null;

            /// <summary>
            /// Indicates that object is marked as deleted
            /// </summary>
            public bool deleted = false;

            /// <summary>
            /// Default constructor
            /// </summary>
            public BalansObjectInfo()
            {
            }

            /// <summary>
            /// Makes sure that all properties have comparable values
            /// </summary>
            public void ValidateProperties()
            {
                if (orgId is int && (int)orgId < 1)
                {
                    orgId = null;
                }

                if (buildingId is int && (int)buildingId < 1)
                {
                    buildingId = null;
                }

                if (purposeGroupId is int && (int)purposeGroupId < 1)
                {
                    purposeGroupId = null;
                }

                if (purposeId is int && (int)purposeId < 1)
                {
                    purposeId = null;
                }

                if (purposeStr is string)
                {
                    string str = ((string)purposeStr).Trim();

                    if (str.Length > 0)
                    {
                        purposeStr = str.ToUpper();
                    }
                    else
                    {
                        purposeStr = null;
                    }
                }

                if (floors is string)
                {
                    string str = ((string)floors).Trim();

                    if (str.Length > 0)
                    {
                        floors = str.ToUpper();
                    }
                    else
                    {
                        floors = null;
                    }
                }

                if (totalSquare is decimal && (decimal)totalSquare <= 0)
                {
                    totalSquare = null;
                }

                /*
                if (nonHabitSquare is decimal && (decimal)nonHabitSquare <= 0)
                {
                    nonHabitSquare = null;
                }
                */
            }

            /// <summary>
            /// Performs matching of another BalansObjectInfo object to this object
            /// </summary>
            /// <param name="info">Object to compare</param>
            /// <returns>TRUE if matching was successful</returns>
            public bool IsMatch(BalansObjectInfo info)
            {
                bool floorsOK = (floors == null) || (info.floors == null) ||
                    (floors is string && info.floors is string && (string)floors == (string)info.floors);

                bool purposeGroupOK = (purposeGroupId == null) || (info.purposeGroupId == null) ||
                    (purposeGroupId is int && info.purposeGroupId is int && (int)purposeGroupId == (int)info.purposeGroupId);

                bool purposeIdOK = (purposeId == null) || (info.purposeId == null) ||
                    (purposeId is int && info.purposeId is int && (int)purposeId == (int)info.purposeId);

                bool purposeStrOK = (purposeStr == null) || (info.purposeStr == null) ||
                    (purposeStr is string && info.purposeStr is string && (string)purposeStr == (string)info.purposeStr);

                bool orgOK =
                    orgId is int && info.orgId is int && (int)orgId == (int)info.orgId;

                bool totalSquareOK =
                    totalSquare is decimal && info.totalSquare is decimal && (decimal)totalSquare == (decimal)info.totalSquare;

                /*
                bool nonHabitSquareOK = (nonHabitSquare == null) || (info.nonHabitSquare == null) ||
                    (nonHabitSquare is decimal && info.nonHabitSquare is decimal && (decimal)nonHabitSquare == (decimal)info.nonHabitSquare);
                */

                return purposeGroupOK && purposeIdOK && purposeStrOK && orgOK && totalSquareOK /* && nonHabitSquareOK */;
            }
        }

        /// <summary>
        /// Describes a validation error
        /// </summary>
        private class ValidationError
        {
            public int reportId = 0;

            public int errorCode = 0;

            public int objectId = 0;

            public string message = "";

            public string objectDescription = "";

            public string token = "";

            public ValidationError()
            {
            }

            public ValidationError(int repId, int objId, int err, string objDescr, string msg, string tok)
            {
                reportId = repId;
                errorCode = err;
                objectId = objId;
                objectDescription = objDescr;
                message = msg;
                token = tok;
            }
        }

        private class RentDecisionInfo
        {
            public int id = 0;
            public string docNum = "";
            public DateTime? docDate = null;
            public string pidstavaStr = "";

            public bool isUpdated = false;

            public RentDecisionInfo()
            {
            }

            public FbCommand CreateUpdateCmd1NF(FbConnection connection1NF, int existingId)
            {
                string fields = "";
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                AddUpdateQueryParameter(ref fields, "ISP", "Auto-import", parameters, 18);
                AddUpdateQueryParameter(ref fields, "DT", DateTime.Now, parameters, -1);
                AddUpdateQueryParameter(ref fields, "NOM", docNum.Length > 0 ? docNum : null, parameters, 18);
                AddUpdateQueryParameter(ref fields, "DT_DOC", docDate.HasValue ? (object)docDate.Value : null, parameters, -1);
                AddUpdateQueryParameter(ref fields, "PIDSTAVA", pidstavaStr.Length > 0 ? pidstavaStr : null, parameters, 255);

                FbCommand cmd = new FbCommand("UPDATE ARRISH SET " + fields.TrimStart(' ', ',') + " WHERE ID = " + existingId.ToString(), connection1NF);

                foreach (KeyValuePair<string, object> param in parameters)
                {
                    cmd.Parameters.Add(new FbParameter(param.Key, param.Value));
                }

                return cmd;
            }

            private void AddUpdateQueryParameter(ref string fields, string fieldName, object value, Dictionary<string, object> parameters, int maxStringLen)
            {
                if (value != null)
                {
                    if (value is string && ((string)value).Length > maxStringLen)
                    {
                        value = ((string)value).Substring(0, maxStringLen);
                    }

                    fields += ", " + fieldName + " = @" + fieldName;
                    parameters.Add(fieldName, value);
                }
                else
                {
                    fields += ", " + fieldName + " = NULL";
                }
            }

            public FbCommand CreateInsertCmd1NF(FbConnection connection1NF, int arendaId, int linkId)
            {
                string fields = "";
                string values = "";
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                AddInsertQueryParameter(ref fields, ref values, "ID", linkId, parameters, -1);
                AddInsertQueryParameter(ref fields, ref values, "ARENDA", arendaId, parameters, -1);
                AddInsertQueryParameter(ref fields, ref values, "ISP", "Auto-import", parameters, 18);
                AddInsertQueryParameter(ref fields, ref values, "DT", DateTime.Now, parameters, -1);
                AddInsertQueryParameter(ref fields, ref values, "NOM", docNum.Length > 0 ? docNum : null, parameters, 18);
                AddInsertQueryParameter(ref fields, ref values, "DT_DOC", docDate.HasValue ? (object)docDate.Value : null, parameters, -1);
                AddInsertQueryParameter(ref fields, ref values, "PIDSTAVA", pidstavaStr.Length > 0 ? pidstavaStr : null, parameters, 255);

                FbCommand cmd = new FbCommand("INSERT INTO ARRISH (" + fields.TrimStart(' ', ',') + ") VALUES (" + values.TrimStart(' ', ',') + ")", connection1NF);

                foreach (KeyValuePair<string, object> param in parameters)
                {
                    cmd.Parameters.Add(new FbParameter(param.Key, param.Value));
                }

                return cmd;
            }

            private void AddInsertQueryParameter(ref string fields, ref string values, string fieldName, object value, Dictionary<string, object> parameters, int maxStringLen)
            {
                if (value != null)
                {
                    if (value is string && ((string)value).Length > maxStringLen)
                    {
                        value = ((string)value).Substring(0, maxStringLen);
                    }

                    fields += ", " + fieldName;
                    values += ", @" + fieldName;
                    parameters.Add(fieldName, value);
                }
                else
                {
                    fields += ", " + fieldName;
                    values += ", NULL";
                }
            }
        }

        #endregion (Nested classes)

        #region Member variables

        /// <summary>
        /// The year for which reports are accepted
        /// </summary>
        int currentYear = DateTime.Now.Year;

        /// <summary>
        /// Current date, represented as string in Firebird format
        /// </summary>
        string curDateFirebird = GetCurDateForFirebird();

        /// <summary>
        /// Connection to the SQL Server
        /// </summary>
        private SqlConnection connectionSqlServer = null;

        /// <summary>
        /// Connection to the Paradox database
        /// </summary>
        private OleDbConnection connectionParadox = null;

        /// <summary>
        /// Connection to the 1NF database in Firebird
        /// </summary>
        private FbConnection connection1NF = null;

        /// <summary>
        /// Object lookup helper
        /// </summary>
        private ObjectFinder objectFinder = new ObjectFinder();

        /// <summary>
        /// Organization lookup helper
        /// </summary>
        private OrganizationFinder organizationFinder = new OrganizationFinder();

        /// <summary>
        /// Archive state creation helper
        /// </summary>
        private Archiver1NF archiver1NF = new Archiver1NF();

        /// <summary>
        /// Mapping of object Id's from the Paradox database to the 1NF database
        /// </summary>
        private Dictionary<int, int> objectIdMapping = new Dictionary<int, int>();

        /// <summary>
        /// Mapping of organization Id's from the Paradox database to the 1NF database
        /// </summary>
        private Dictionary<int, int> organizationIdMapping = new Dictionary<int, int>();

        /// <summary>
        /// Mapping of balans Id's from the Paradox database to the 1NF database
        /// </summary>
        private Dictionary<int, int> balansIdMapping = new Dictionary<int, int>();

        /// <summary>
        /// Mapping of arenda Id's from the Paradox database to the 1NF database
        /// </summary>
        private Dictionary<int, int> arendaIdMapping = new Dictionary<int, int>();

        /// <summary>
        /// Cache of 1NF document Id's by document tags (the tag includes doc number, date and kind)
        /// </summary>
        private Dictionary<string, int> docIdCache1NF = new Dictionary<string, int>();

        /// <summary>
        /// The list of valid document types in the 1 NF database
        /// </summary>
        private HashSet<int> validDocTypes1NF = new HashSet<int>();

        /// <summary>
        /// Auto-incremented ID of new organizations in 1NF
        /// </summary>
        private int organizationIdSeed1NF = 0;

        /// <summary>
        /// All Paradox files are copied to this folder before processing (because Paradox driver
        /// can not handlefolders with cyrillic names)
        /// </summary>
        private const string localDbFolderName = ".\\ParadoxDbFiles";

        /// <summary>
        /// Auto-incremented indexof a temporary DB sub-folder within 'ParadoxDbFiles'
        /// </summary>
        private int paradoxFolderIndex = 0;

        /// <summary>
        /// Contains all rent agreements from 1NF database. Key in this dictionary is a building ID
        /// </summary>
        private Dictionary<int, List<ArendaAgreementInfo>> rentAgreementCache = new Dictionary<int, List<ArendaAgreementInfo>>();

        /// <summary>
        /// Contains all 'balans' records from the 1NF database. Key in this dictionary is a building ID
        /// </summary>
        private Dictionary<int, List<BalansObjectInfo>> balansCacheByObject = new Dictionary<int, List<BalansObjectInfo>>();

        /// <summary>
        /// Contains all 'balans' records from the 1NF database. Key in this dictionary is an ID from 'balans_1nf' table
        /// </summary>
        private Dictionary<int, BalansObjectInfo> balansCacheByID = new Dictionary<int, BalansObjectInfo>();

        /// <summary>
        /// IDs of purpose groups that do not have any associated purpose values
        /// </summary>
        private HashSet<int> emptyPurposeGroups = new HashSet<int>();

        #endregion (Member variables)

        #region Logs

        /// <summary>
        /// Column names of the imported paradox tables are logged into this CSV file
        /// </summary>
        // private GUKV.DataMigration.CSVLog log = new GUKV.DataMigration.CSVLog("paradox_schema.csv");

        /// <summary>
        /// Contains all object addresses that could not be mapped to 1NF database
        /// </summary>
        private CSVLog logObjAddressNoMatch = new CSVLog("CSV/ObjectAddrNoMatch.csv");

        /// <summary>
        /// Contains all object addresses that could be successfully mapped to 1NF database
        /// </summary>
        private CSVLog logObjAddressMatched = new CSVLog("CSV/ObjectAddrMatched.csv");

        /// <summary>
        /// Contains addresses that match to a wrong Id in 1NF
        /// </summary>
        private CSVLog logObjAddressWrongMatch = new CSVLog("CSV/ObjectAddrWrongMatch.csv");

        /// <summary>
        /// Contains all organizations that could not be mapped to 1NF database
        /// </summary>
        private CSVLog logOrgNoMatch = new CSVLog("CSV/OrgNoMatch.csv");

        /// <summary>
        /// Contains all organizations that could be successfully mapped to 1NF database
        /// </summary>
        private CSVLog logOrgMatched = new CSVLog("CSV/OrgMatched.csv");

        /// <summary>
        /// This CSV log contains all organizations that could not be matched because of duplicate ZKPO codes
        /// </summary>
        private CSVLog logUnmatchedOrgsBecauseOfDuplicateZKPO = new CSVLog("CSV/DuplicateZKPOOrg.csv");

        #endregion (Logs)

        #region Construction

        public Importer()
        {
        }

        #endregion (Construction)

        #region Interface

        public void Import(Preferences preferences)
        {
            // Create a temporary folder for the database files
            if (!System.IO.Directory.Exists(localDbFolderName))
            {
                System.IO.Directory.CreateDirectory(localDbFolderName);
            }

            // Clear the temporary folder
            try
            {
                ClearFolder(localDbFolderName);
            }
            catch (Exception ex)
            {
                WriteError("Could not clear the temporary DB folder. Exception: " + ex.Message);
                return;
            }

            // Test that connection to SQL Server is valid
            string connString = preferences.GetImport1NFConnectionString();
            connectionSqlServer = new SqlConnection(connString);

            try
            {
                connectionSqlServer.Open();
                connectionSqlServer.Close();
            }
            catch
            {
                WriteError("Could not connect to SQL Server. Conection string: " + connString);
                return;
            }

            // Test that connection to 1 NF is valid
            connString = preferences.Get1NFConnectionString();
            connection1NF = new FbConnection(connString);

            try
            {
                connection1NF.Open();
                connection1NF.Close();
            }
            catch
            {
                WriteError("Could not connect to 1NF. Conection string: " + connString);
                return;
            }

            // Open connections to SQL Server and 1 NF
            connectionSqlServer.Open();
            connection1NF.Open();

            // Get the current ID seed for 1NF organizations
            organizationIdSeed1NF = organizationFinder.Get1NFOrganizationIdSeed(connection1NF);

            // Fill the list of empty purpose groups (for validation only)
            PrepareListOfEmptyPurposeGroups();

            // Populate the object finder and organization finder
            try
            {
                objectFinder.BuildObjectCacheFrom1NF(connection1NF);
                organizationFinder.BuildOrganizationCacheFrom1NF(connection1NF, false);
                organizationFinder.ReadManualZKPOMatch(connectionSqlServer);

                // organizationFinder.GenerateDuplicateZKPOReport(connectionSqlServer, "duplicate_zkpo");
            }
            catch (Exception ex)
            {
                WriteError("Exception while populating object and organization finders: " + ex.Message);

                connectionSqlServer.Close();
                connection1NF.Close();
                return;
            }

            // Populate the cache of all major entities
            try
            {
                PrepareRentAgreementCache();
                PrepareBalansCache();
            }
            catch (Exception ex)
            {
                WriteError("Exception while populating the cache of existing entities (balans and arenda): " + ex.Message);

                connectionSqlServer.Close();
                connection1NF.Close();
                return;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 1) Prepare the cache of folder names that are already processed
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            HashSet<string> processedFolders = new HashSet<string>();

            string query = "SELECT folder_name FROM reports";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string folderName = reader.IsDBNull(0) ? "" : reader.GetString(0);

                        if (folderName.Length > 0)
                        {
                            processedFolders.Add(folderName.ToLower());
                        }
                    }

                    reader.Close();
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 2) Find all newly added folders in the input folder
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            string inputFolder = preferences.input1NFFolder;

            if (!inputFolder.EndsWith("/") && !inputFolder.EndsWith("\\"))
            {
                inputFolder += "/";
            }

            IEnumerable<string> folders = System.IO.Directory.EnumerateDirectories(preferences.input1NFFolder);
            IEnumerator<string> enumerator = folders.GetEnumerator();

            while (enumerator.MoveNext())
            {
                string path = enumerator.Current;

                // Make sure that only the name of specific folder is saved (not the full path)
                string dirName = System.IO.Path.GetDirectoryName(path);

                if (dirName.Length > 0)
                {
                    path = path.Substring(dirName.Length);
                }

                while (path.StartsWith("/") || path.StartsWith("\\"))
                {
                    path = path.Substring(1);
                }

                // Process the folder only if it is not processed yet
                if (!processedFolders.Contains(path.ToLower()))
                {
                    // Generate full name of the folder
                    dirName = inputFolder + path;

                    if (System.IO.Directory.Exists(dirName))
                    {
                        WriteInfo("Importing data from folder: " + path);

                        ProcessInputFolder(dirName, path, preferences);
                    }
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 3) Select all reports that should be imported (in case there are multiple reports from one organization)
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ChooseReportsForProcessing();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 4) Validate the reports
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ValidateReports();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 5) Perform export to 1NF for all validated reports
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ExportReportsTo1NF();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 6) Shutdown
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Close all connections
            connectionSqlServer.Close();
            connection1NF.Close();

            // Clear the temporary folder on exit
            try
            {
                ClearFolder(localDbFolderName);
            }
            catch (Exception ex)
            {
                WriteError("Could not clear the temporary DB folder. Exception: " + ex.Message);
            }
        }

        private void ProcessInputFolder(string folderNameFull, string folderNameShort, Preferences preferences)
        {
            // If this directory contains Paradox files, process this directory and stop
            if (IsParadoxFolder(folderNameFull))
            {
                ImportDatabase(folderNameFull, folderNameShort, preferences);
            }
            else
            {
                // Check all sub-folders recursively
                IEnumerable<string> folders = System.IO.Directory.EnumerateDirectories(folderNameFull);

                IEnumerator<string> enumerator = folders.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    ProcessInputFolder(enumerator.Current, folderNameShort, preferences);
                }
            }
        }

        private bool IsParadoxFolder(string folderName)
        {
            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(folderName, "*.db");

            IEnumerator<string> enumerator = files.GetEnumerator();

            return enumerator.MoveNext();
        }

        private void ImportDatabase(string databaseFolder, string folderNameShort, Preferences preferences)
        {
            int reportId = -1;
            int organizationId = -1;

            // Get the save date and time for this report
            DateTime dtSave = GetParadoxFolderModifyDate(databaseFolder);

            // Copy all the database files to the local folder, because Paradox driver fails on
            // folder names with cyrillic symbols
            string localDbPath = CopyParadoxFolderToLocal(databaseFolder);

            // Check that Paradox connection can be opened
            connectionParadox = new OleDbConnection(preferences.GetParadoxConnectionString(localDbPath));

            try
            {
                connectionParadox.Open();
            }
            catch (Exception ex)
            {
                WriteInfo("Could not open Paradox connection for the input folder: " + databaseFolder);
                WriteException(ex);

                connectionParadox = null;
            }

            if (connectionParadox != null)
            {
                // Testing
                // DumpDataTable("sorg_1nf", connectionParadox);

                // Read the basic information about report
                if (ReadReportInfo(databaseFolder, folderNameShort, dtSave, out reportId, out organizationId))
                {
                    try
                    {
                        ImportObjects(reportId);
                        ImportOrganizations(reportId);
                        ImportBalans(reportId);
                        ImportArenda(reportId);
                    }
                    catch (Exception ex)
                    {
                        WriteError("Exception while reading data from Paradox database: " + ex.Message);

                        // Prevent further processing
                        reportId = -1;
                    }

                    // Testing
                    // OverrideDictionary("skinddok", "ID");
                }

                connectionParadox.Close();
                connectionParadox = null;
            }
        }

        private void ChooseReportsForProcessing()
        {
            WriteInfo("Selecting the most recent reports for import...");

            // Select the maximum save date for each organization
            Dictionary<int, DateTime> orgMaxSaveDate = new Dictionary<int, DateTime>();

            string query = "SELECT organization_id, save_date FROM reports";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                        {
                            int orgId = reader.GetInt32(0);
                            DateTime dtSave = reader.GetDateTime(1);

                            if (orgMaxSaveDate.ContainsKey(orgId))
                            {
                                DateTime dtMax = orgMaxSaveDate[orgId];

                                if (dtSave > dtMax)
                                {
                                    orgMaxSaveDate[orgId] = dtSave;
                                }
                            }
                            else
                            {
                                orgMaxSaveDate.Add(orgId, dtSave);
                            }
                        }
                    }

                    reader.Close();
                }
            }

            // Find the most recent report for each organization
            Dictionary<int, int> recentReports = new Dictionary<int, int>();

            query = "SELECT id, organization_id, save_date, load_status FROM reports";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                        {
                            int reportId = reader.GetInt32(0);
                            int orgId = reader.GetInt32(1);
                            DateTime dtSave = reader.GetDateTime(2);
                            int loadStatus = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);

                            /*
                            if (!recentReports.ContainsKey(orgId))
                            {
                                if (orgMaxSaveDate.ContainsKey(orgId))
                                {
                                    DateTime dtMax = orgMaxSaveDate[orgId];

                                    if (dtSave == dtMax)
                                    {
                                        recentReports.Add(orgId, reportId);
                                    }
                                }
                            }
                            */

                            if (orgMaxSaveDate.ContainsKey(orgId))
                            {
                                DateTime dtMax = orgMaxSaveDate[orgId];

                                if (dtSave >= dtMax)
                                {
                                    recentReports[orgId] = reportId;
                                }
                            }

                            // If there is a loaded report, always accept it as the most recent
                            /*
                            if (loadStatus == 1)
                            {
                                recentReports[orgId] = reportId;
                            }
                            */
                        }
                    }

                    reader.Close();
                }
            }

            // Set data status for all reports
            foreach (KeyValuePair<int, int> pair in recentReports)
            {
                int orgId = pair.Key;
                int reportId = pair.Value;

                // 1) Mark this report as the most recent
                using (SqlCommand cmd = new SqlCommand("UPDATE reports SET data_status = 1 WHERE id = @rid", connectionSqlServer))
                {
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));
                    cmd.ExecuteNonQuery();
                }

                // 2) Mark all other reports from this organization as NOT recent
                using (SqlCommand cmd = new SqlCommand("UPDATE reports SET data_status = 0 WHERE organization_id = @oid AND id <> @rid", connectionSqlServer))
                {
                    cmd.Parameters.Add(new SqlParameter("oid", orgId));
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private DateTime GetParadoxFolderModifyDate(string inputFolder)
        {
            DateTime dtMaxUpdateDate = DateTime.MinValue;

            System.IO.DirectoryInfo dirSource = new System.IO.DirectoryInfo(inputFolder);

            foreach (System.IO.FileInfo fi in dirSource.GetFiles())
            {
                if (fi.LastWriteTime > dtMaxUpdateDate)
                {
                    dtMaxUpdateDate = fi.LastWriteTime;
                }
            }

            return dtMaxUpdateDate;
        }

        private string CopyParadoxFolderToLocal(string inputFolder)
        {
            // Generate a new name for the temporary folder
            paradoxFolderIndex++;

            string tempfolder = localDbFolderName + "\\" + paradoxFolderIndex.ToString();

            // Create the temporary folder
            if (!System.IO.Directory.Exists(tempfolder))
            {
                System.IO.Directory.CreateDirectory(tempfolder);
            }

            // Copy all files from the source directory to the destination directory
            System.IO.DirectoryInfo dirSource = new System.IO.DirectoryInfo(inputFolder);

            foreach (System.IO.FileInfo fi in dirSource.GetFiles())
            {
                fi.CopyTo(tempfolder + "\\" + fi.Name);
            }

            return tempfolder;
        }

        private void ClearFolder(string folderName)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderName);

            foreach (System.IO.FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (System.IO.DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);

                di.Delete();
            }
        }

        #endregion (Interface)

        #region Import from Paradox DB

        private string convertToUnicode(string str)
        {
            Encoding utf8 = Encoding.Unicode;
            Encoding win1251 = Encoding.GetEncoding("windows-1251");

            // Convert the string into a byte[]
            byte[] utf8Bytes = utf8.GetBytes(str);

            // Remove every second character from the unicode bytes
            byte[] buffer = new byte[utf8Bytes.Length / 2];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = utf8Bytes[i * 2];
            }

            return win1251.GetString(buffer);
        }

        private object convertParamToUnicode(object param)
        {
            if (param is string)
            {
                return convertToUnicode((string)param);
            }

            return param;
        }

        private bool ReadReportInfo(string path, string folderNameShort, DateTime dtSave,
            out int reportId, out int organizationId)
        {
            reportId = -1;
            organizationId = -1;

            try
            {
                using (OleDbCommand cmd = new OleDbCommand("select DT, DT_UNLOAD, ORG_KOD from general_data", connectionParadox))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // object dataDtSave = reader.IsDBNull(0) ? null : reader.GetValue(0);
                            // object dataDtUnload = reader.IsDBNull(1) ? null : reader.GetValue(1);
                            object dataOrgId = reader.IsDBNull(2) ? null : reader.GetValue(2);

                            if (dataOrgId is int)
                            {
                                organizationId = (int)dataOrgId;

                                // [Stanislav] Override organization ID for the School 306! This fix is required for one particular report
                                if (organizationId == 141450 && path.Contains("_306"))
                                {
                                    organizationId = 141440;
                                }

                                string query = "insert into reports (organization_id, report_year, save_date, import_date, folder_name, folder_path)" +
                                    " values (@p1, @p2, @p3, @p4, @p5, @p6)";

                                using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
                                {
                                    command.Parameters.Add(new SqlParameter("p1", organizationId));
                                    command.Parameters.Add(new SqlParameter("p2", currentYear));
                                    command.Parameters.Add(new SqlParameter("p3", dtSave));
                                    command.Parameters.Add(new SqlParameter("p4", DateTime.Now.Date));
                                    command.Parameters.Add(new SqlParameter("p5", folderNameShort));
                                    command.Parameters.Add(new SqlParameter("p6", path));

                                    command.ExecuteNonQuery();
                                }

                                // Read the auto-generated ID of this report
                                query = "select max(id) from reports";

                                using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
                                {
                                    using (SqlDataReader r = command.ExecuteReader())
                                    {
                                        if (r.Read())
                                        {
                                            object dataReportId = r.IsDBNull(0) ? null : r.GetValue(0);

                                            if (dataReportId is int)
                                            {
                                                reportId = (int)dataReportId;
                                            }
                                        }

                                        r.Close();
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("Could not read report credentials (organization ID, etc.) from the database. Exception: " + ex.Message);
            }

            return reportId > 0 && organizationId > 0;
        }

        private void ImportObjects(int reportId)
        {
            // Create mapping for the fields that have different names in Paradox DB and our DB
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            mapping.Add("ADDRESS", "[ADDRESS]");

            ImportTable("object_1nf", "object_1nf", reportId, mapping);
        }

        private void ImportOrganizations(int reportId)
        {
            // Create mapping for the fields that have different names in Paradox DB and our DB
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            ImportTable("sorg_1nf", "sorg_1nf", reportId, mapping);
        }

        private void ImportBalans(int reportId)
        {
            // Create mapping for the fields that have different names in Paradox DB and our DB
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            mapping.Add("ID", "BALANS_ID");
            mapping.Add("OBJECT", "OBJECT_ID");
            mapping.Add("ORG", "organization_id");

            ImportTable("balans_1nf", "balans_1nf", reportId, mapping);
        }

        private void ImportArenda(int reportId)
        {
            // Create mapping for the fields that have different names in Paradox DB and our DB
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            mapping.Clear();
            mapping.Add("ID", "ID_ARENDA");
            mapping.Add("SQUARE", "[SQUARE]");

            ImportTable("arenda1nf", "arenda_1nf", reportId, mapping);

            mapping.Clear();
            mapping.Add("ID", "ID_ARENDA_PRIM");
            mapping.Add("SQUARE", "[SQUARE]");

            ImportTable("arenda_prim", "arenda_prim", reportId, mapping);

            mapping.Clear();
            mapping.Add("ID", "ID_ARRISH");

            ImportTable("arrish", "arrish", reportId, mapping);

            mapping.Clear();
            mapping.Add("OBJECT", "[OBJECT]");

            ImportTable("rishen", "rishen", reportId, mapping);
        }

        private void ImportTable(string sourceTable, string destTable, int reportId,
            Dictionary<string, string> mapping)
        {
            // Read all the columns from the source table
            using (OleDbCommand cmd = new OleDbCommand("select * from " + sourceTable, connectionParadox))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool valuesValid = true;

                        // Prepare an SQL statement for inserting this row into the destination table
                        string fields = "report_id";
                        string values = "@rid";

                        Dictionary<string, object> parameters = new Dictionary<string, object>();

                        parameters["@rid"] = reportId;

                        // List<object> debugValues = new List<object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            fields += ", ";
                            values += ", ";

                            // This field can have different name in our table
                            string fieldName = reader.GetName(i).ToUpper();

                            if (mapping.ContainsKey(fieldName))
                            {
                                fieldName = mapping[fieldName];
                            }

                            fields += fieldName;
                            values += "@p" + i.ToString();

                            object val = ValidateDate(reader.GetValue(i));

                            // Users may enter invalid dates. Validate them
                            if (val is DateTime && ((DateTime)val).Year < 1760)
                            {
                                valuesValid = false;
                            }

                            parameters["@p" + i.ToString()] = val;

                            /* object tempValue = reader.GetValue(i);
                            debugValues.Add(tempValue); */
                        }

                        if (valuesValid)
                        {
                            // Format an INSERT statement for the SQL Server
                            string query = "insert into " + destTable + " ( " + fields + " ) values ( " + values + " )";

                            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
                            {
                                foreach (KeyValuePair<string, object> pair in parameters)
                                {
                                    command.Parameters.Add(new SqlParameter(pair.Key, convertParamToUnicode(pair.Value)));
                                }

                                command.ExecuteNonQuery();
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        #endregion (Import from Paradox DB)

        #region Export to 1NF

        private void PreProcessObjects(int reportId)
        {
            // Make sure that building number is correctly entered
            Dictionary<int, string> numbers1 = new Dictionary<int, string>();
            Dictionary<int, string> numbers2 = new Dictionary<int, string>();

            string query = "select id, nomer1, nomer2 from object_1nf where report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        string dataNomer1 = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string dataNomer2 = reader.IsDBNull(2) ? "" : reader.GetString(2);

                        if (dataId is int)
                        {
                            int id = (int)dataId;

                            if (dataNomer1.Length > 9 || dataNomer2.Length > 18)
                            {
                                dataNomer1 = dataNomer1.Trim().Substring(0, 9);
                                dataNomer2 = dataNomer2.Trim().Substring(0, 18);

                                numbers1[id] = dataNomer1;
                                numbers2[id] = dataNomer2;
                            }

                            dataNomer1 = dataNomer1.Trim();
                            dataNomer2 = dataNomer2.Trim();

                            // Find any non-digit in the primary number
                            for (int i = 0; i < dataNomer1.Length; i++)
                            {
                                if (!char.IsDigit(dataNomer1[i]))
                                {
                                    string actualNomer1 = dataNomer1.Substring(0, i);
                                    string actualNomer2 = dataNomer1.Substring(i).Trim();

                                    string prevNomer2 = dataNomer2;

                                    if (prevNomer2.Length > 0)
                                    {
                                        actualNomer2 += " " + prevNomer2;
                                    }

                                    if (actualNomer2.Length > 18)
                                    {
                                        actualNomer2 = actualNomer2.Substring(0, 18);
                                    }

                                    numbers1[id] = actualNomer1;
                                    numbers2[id] = actualNomer2;
                                    break;
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }

            // Update the building numbers in our database
            foreach (KeyValuePair<int, string> pair in numbers1)
            {
                query = "update object_1nf set nomer1 = @num1, nomer2 = @num2 where id = @rowid";

                using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
                {
                    cmd.Parameters.Add(new SqlParameter("rowid", pair.Key));
                    cmd.Parameters.Add(new SqlParameter("num1", pair.Value));
                    cmd.Parameters.Add(new SqlParameter("num2", numbers2[pair.Key]));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ExportReportsTo1NF()
        {
            WriteInfo("Exporting validated reports to 1NF...");

            Dictionary<int, int> reports = new Dictionary<int, int>();

            string query = "SELECT id, organization_id FROM reports WHERE load_status = 0 AND data_status = 1 AND validation_status = 1";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                        {
                            reports.Add(reader.GetInt32(0), reader.GetInt32(1));
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, int> pair in reports)
            {
                ExportReportTo1NF(pair.Key, pair.Value);
            }

            // Update the "find" names for all exported organizations
            using (FbCommand command = new FbCommand("UPDATE SORG_1NF SET FIND_NAME_OBJ = SHORT_NAME_OBJ WHERE FIND_NAME_OBJ IS NULL", connection1NF))
            {
                command.ExecuteNonQuery();
            }
        }

        private void ExportReportTo1NF(int reportId, int organizationId)
        {
            WriteInfo("Exporting report " + reportId.ToString());

            objectIdMapping.Clear();
            organizationIdMapping.Clear();
            balansIdMapping.Clear();
            arendaIdMapping.Clear();

            // Prepare 1 NF cached data
            try
            {
                Prepare1NFDocumentCache();
                archiver1NF.PrepareArchiveObjectList(connection1NF);
                PrepareDocTypeList();
            }
            catch (Exception ex)
            {
                WriteError("Exception while preparing 1NF data cache: " + ex.Message);
                return;
            }

            // Perform pre-processing of imported data
            try
            {
                PreProcessObjects(reportId);
            }
            catch (Exception ex)
            {
                WriteError("Exception while pre-processing imported data: " + ex.Message);
                return;
            }

            // Make all changes to the 1 NF database in a transaction
            FbTransaction transaction = null;

            try
            {
                transaction = connection1NF.BeginTransaction();

                // Get the list of objects and organizations which are used in Balans and Arenda
                HashSet<int> requiredObjects = new HashSet<int>();
                HashSet<int> requiredOrganizations = new HashSet<int>();
                HashSet<int> buildingRentedByReportSubmitter = new HashSet<int>();
                HashSet<int> renters = new HashSet<int>();

                GetBalansObjectsAndOrganizations(reportId, requiredObjects, requiredOrganizations);

                GetArendaObjectsAndOrganizations(reportId, organizationId, requiredObjects,
                    requiredOrganizations, buildingRentedByReportSubmitter, renters);

                // Perform export
                ExportObjects(reportId, transaction, requiredObjects, buildingRentedByReportSubmitter);
                ExportOrganizations(reportId, organizationId, transaction, requiredOrganizations, renters);

                ExportBalans(reportId, organizationId, transaction);
                ExportBalansDocs(reportId, organizationId, transaction);
                ExportBalansStreetNames(reportId, organizationId, transaction);

                ExportArenda(reportId, transaction);
                // ExportArendaPrim(reportId, transaction);
                // ExportArendaRishen(reportId, transaction);

                // Commit the transaction
                transaction.Commit();

                // Mark the report as 'processed'
                string query = "UPDATE reports SET load_status = 1 WHERE id = @rid";

                using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
                {
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                WriteError("Exception while exporting report to 1NF: " + ex.Message);

                // Roll back the transaction
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
        }

        private void GetBalansObjectsAndOrganizations(int reportId, HashSet<int> requiredObjects,
            HashSet<int> requiredOrganizations)
        {
            // Enumerate all edited Balans entries, and collect all objects and organizations from them
            string query = "SELECT balans_id, [object_id], organization_id, sqr_zag, grpurp, purpose FROM balans_1nf WHERE edited <> 0 AND deleted = 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int balansId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        int objectId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                        int organizationId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                        decimal sqrTotal = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                        int purposeGroupId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                        int purposeId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);

                        // Save objects and organizations that we need to validate
                        if (balansId > 0 && objectId > 0 && organizationId > 0 && sqrTotal > 0 && purposeGroupId > 0 && purposeId > 0)
                        {
                            requiredObjects.Add(objectId);
                            requiredOrganizations.Add(organizationId);
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void GetArendaObjectsAndOrganizations(int reportId, int organizationId, HashSet<int> requiredObjects,
            HashSet<int> requiredOrganizations, HashSet<int> buildingRentedByReportSubmitter, HashSet<int> renters)
        {
            // Enumerate all edited Arenda entries, and collect all objects and organizations from them
            string query = "SELECT id_arenda, orgiv, org_kod, balans_kod, object_kod, [square], plata_dogovor, start_orenda" +
                " FROM arenda_1nf WHERE edited <> 0 AND deleted = 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int arendaId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        int orgGiverId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                        int orgRenterId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                        int orgBalansId = reader.IsDBNull(3) ? -1 : reader.GetInt32(3);
                        int objectId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                        decimal sqr = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5);
                        // decimal payment = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6);
                        object rentStart = reader.IsDBNull(7) ? null : reader.GetValue(7);

                        // Save objects and organizations that we need to validate
                        if (arendaId > 0 &&
                            orgGiverId > 0 &&
                            orgRenterId > 0 &&
                            orgBalansId > 0 &&
                            objectId > 0 &&
                            sqr > 0 &&
                            /* payment > 0 && */
                            rentStart is DateTime)
                        {
                            requiredObjects.Add(objectId);
                            requiredOrganizations.Add(orgRenterId);
                            requiredOrganizations.Add(orgGiverId);
                            requiredOrganizations.Add(orgBalansId);

                            renters.Add(orgRenterId);

                            if (orgRenterId == organizationId)
                            {
                                buildingRentedByReportSubmitter.Add(objectId);
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void CreateObjectFieldMapping(Dictionary<string, string> mapping)
        {
            mapping.Clear();

            mapping.Add("ulkod2", "ulkod2");
            mapping.Add("ikod", "ikod");
            mapping.Add("budyear", "budyear");
            mapping.Add("history", "history");
            mapping.Add("newdistr", "newdistr");
            mapping.Add("ulkod", "ulkod");
            mapping.Add("vidobj", "vidobj");
            mapping.Add("adrdop", "adrdop");
            mapping.Add("typobj", "typobj");
            mapping.Add("nomer1", "nomer1");
            mapping.Add("texstan_obj", "texstan");
            mapping.Add("nomer2", "nomer2");
            mapping.Add("oznaka_bud", "oznaka_bud");
            mapping.Add("pnum", "pnum");
            mapping.Add("nomer3", "nomer3");
            mapping.Add("dodat_vid", "dodat_vid");
            mapping.Add("szag", "szag");
            mapping.Add("v_nej", "v_nej");
            mapping.Add("v_jil", "v_jil");
            mapping.Add("s_gurt", "s_gurt");
        }

        private void ExportObjects(int reportId, FbTransaction transaction,
            HashSet<int> requiredObjects, HashSet<int> buildingRentedByReportSubmitter)
        {
            // Create mapping for the columns between our database and 1NF database
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            Dictionary<string, object> data = new Dictionary<string,object>();

            CreateObjectFieldMapping(mapping);
            string srcFieldList = GetSrcFieldList(mapping, true);

            // Create special mapping for UPDATE requests. We do not want to replace object address,
            // because this information is more valid in 1NF database than in user reports
            Dictionary<string, string> mappingUpdate = new Dictionary<string, string>(mapping);

            mappingUpdate.Remove("ulkod");
            mappingUpdate.Remove("ulkod2");
            mappingUpdate.Remove("newdistr");
            mappingUpdate.Remove("nomer1");
            mappingUpdate.Remove("nomer2");
            mappingUpdate.Remove("nomer3");
            mappingUpdate.Remove("adrdop");

            Dictionary<string, string> mappingUpdateNoSquare = new Dictionary<string, string>(mappingUpdate);

            mappingUpdateNoSquare.Remove("szag");
            mappingUpdateNoSquare.Remove("v_nej");
            mappingUpdateNoSquare.Remove("v_jil");
            mappingUpdateNoSquare.Remove("s_gurt");
            
            // Debug information
            Dictionary<int, int> matchedObjects = new Dictionary<int, int>();
            Dictionary<int, int> createdObjects = new Dictionary<int, int>();

            // Get the list of added or modified objects
            string query = "SELECT id, object_kod, edited, " + srcFieldList + " FROM object_1nf WHERE report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GetReaderRowData(reader, data, mapping);

                        object dataEntryId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataObjectId = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        int wasEdited = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);

                        if (dataObjectId is int && requiredObjects.Contains((int)dataObjectId))
                        {
                            // Try to find an existing object in 1NF
                            int existingId = -1;

                            int streetCode1 = data.ContainsKey("ulkod") ? (int)data["ulkod"] : -1;
                            int streetCode2 = data.ContainsKey("ulkod2") ? (int)data["ulkod2"] : -1;
                            string number1 = data.ContainsKey("nomer1") ? (string)data["nomer1"] : "";
                            string number2 = data.ContainsKey("nomer2") ? (string)data["nomer2"] : "";
                            string number3 = data.ContainsKey("nomer3") ? (string)data["nomer3"] : "";
                            string addrMisc = data.ContainsKey("adrdop") ? (string)data["adrdop"] : "";
                            object district = data.ContainsKey("newdistr") ? data["newdistr"] : null;
                            object sqr = data.ContainsKey("szag") ? data["szag"] : null;

                            if (streetCode1 == streetCode2)
                            {
                                streetCode2 = -1;
                            }

                            string street1 = "";
                            string street2 = "";

                            if (!objectFinder.GetStreetName(streetCode1, out street1))
                            {
                                street1 = "";
                            }

                            if (!objectFinder.GetStreetName(streetCode2, out street2))
                            {
                                street2 = "";
                            }

                            street1 = street1.Trim().ToUpper();
                            street2 = street2.Trim().ToUpper();
                            number1 = number1.Trim().ToUpper();
                            number2 = number2.Trim().ToUpper();
                            number3 = number3.Trim().ToUpper();
                            addrMisc = addrMisc.Trim().ToUpper();

                            // Correct some well-known mistakes
                            CorrectKnownAddrMisspellings(ref street1, ref street2, ref number1, ref number2, ref number3, ref addrMisc);

                            // Format the full address, for writing to the log
                            string fullSrcAddress = street1 + " / " + street2 + " " + number1 + " + " + number2 + " + " + number3 + " ~ " + addrMisc;

                            if (district is int)
                            {
                                fullSrcAddress += " District = ";
                                fullSrcAddress += district.ToString();
                            }

                            // If organization which submitted the report rents some objects in this building,
                            // ignore the square. The 'Obmin 1NF' tool used by report submitters does not allow
                            // to enter total square of buildings in this situation
                            Dictionary<string, string> mappingForUpdate = mappingUpdate;

                            if (buildingRentedByReportSubmitter.Contains((int)dataObjectId))
                            {
                                sqr = null;
                                mappingForUpdate = mappingUpdateNoSquare;
                            }

                            // Perform address matching
                            bool addressIsSimple = false;
                            bool similarAddressExists = false;

                            if ((streetCode1 > 0 || streetCode2 > 0) && (number1.Length > 0 || number2.Length > 0 || number3.Length > 0))
                            {
                                existingId = objectFinder.FindObject(street1, street2, number1, number2, number3,
                                    addrMisc, district, sqr, out addressIsSimple, out similarAddressExists);
                            }

                            if (existingId > 0)
                            {
                                // Save the mapping between proposed ID and the existing ID
                                objectIdMapping[(int)dataObjectId] = existingId;

                                if (wasEdited > 0)
                                {
                                    // Prepare an UPDATE statement
                                    string destFieldList = GetUpdateFieldList(reader, mappingForUpdate, parameters, null);

                                    if (parameters.Count > 0)
                                    {
                                        // Create an archived state if necessary
                                        int archRecordId = archiver1NF.CreateObjectArchiveRecord(connection1NF, existingId, transaction);

                                        // Prepare the UPDATE statement
                                        query = "UPDATE OBJECT_1NF SET ISP = @isp, EIS_MODIFIED_BY = @isp, DT = @dt, STAN_YEAR = @syear, ULNAME = @sname1, ULNAME2 = @sname2, ";

                                        if (archRecordId > 0)
                                        {
                                            query += "ARCH_KOD = @acod, ";
                                        }

                                        query += destFieldList + " WHERE OBJECT_KOD = @oid";

                                        try
                                        {
                                            using (FbCommand commandUpdate = new FbCommand(query, connection1NF))
                                            {
                                                commandUpdate.Parameters.Add(new FbParameter("oid", existingId));
                                                commandUpdate.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                                commandUpdate.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                                commandUpdate.Parameters.Add(new FbParameter("syear", DateTime.Now.Year));
                                                commandUpdate.Parameters.Add(new FbParameter("sname1", street1));
                                                commandUpdate.Parameters.Add(new FbParameter("sname2", street2));

                                                if (archRecordId > 0)
                                                {
                                                    commandUpdate.Parameters.Add(new FbParameter("acod", archRecordId));
                                                }

                                                foreach (KeyValuePair<string, object> pair in parameters)
                                                {
                                                    commandUpdate.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                                }

                                                commandUpdate.Transaction = transaction;
                                                commandUpdate.ExecuteNonQuery();
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            WriteError("SQL query failed: " + query);
                                            DumpQueryParameters(parameters);
                                            WriteError("Parameter oid = " + existingId.ToString());
                                            WriteError("Parameter acod = " + archRecordId.ToString());
                                            throw;
                                        }
                                    }
                                }

                                // Save the match to our database
                                matchedObjects[(int)dataEntryId] = existingId;
                            }
                            else
                            {
                                // Check if street names exist in 1 NF
                                if (street1.Length > 0 && !objectFinder.IsKnownStreetName(street1))
                                {
                                    throw new ArgumentException("Unknown street name: " + street1 + ". Aborting.");
                                }

                                if (street2.Length > 0 && !objectFinder.IsKnownStreetName(street2))
                                {
                                    throw new ArgumentException("Unknown street name: " + street2 + ". Aborting.");
                                }

                                string paramList = "";
                                string destFieldList = GetInsertFieldList(reader, out paramList, mapping, parameters, null);

                                if (parameters.Count > 0)
                                {
                                    // Generate new object Id
                                    int newObjectId = GenerateNewId1NF("OBJECT_1NF_GEN", transaction);

                                    if (newObjectId > 0)
                                    {
                                        // Prepare the INSERT statement
                                        query = "INSERT INTO OBJECT_1NF (OBJECT_KOD, OBJECT_KODSTAN, ISP, EIS_MODIFIED_BY, DT, STAN_YEAR, REALSTAN, ULNAME, ULNAME2, DELETED, KORPUS, "
                                            + destFieldList + ") VALUES (@oid, 1, @isp, @isp, @dt, @syear, 1, @sname1, @sname2, 0, 0, " + paramList + ")";

                                        try
                                        {
                                            using (FbCommand commandInsert = new FbCommand(query, connection1NF))
                                            {
                                                commandInsert.Parameters.Add(new FbParameter("oid", newObjectId));
                                                commandInsert.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                                commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                                commandInsert.Parameters.Add(new FbParameter("syear", DateTime.Now.Year));
                                                commandInsert.Parameters.Add(new FbParameter("sname1", street1));
                                                commandInsert.Parameters.Add(new FbParameter("sname2", street2));

                                                foreach (KeyValuePair<string, object> pair in parameters)
                                                {
                                                    commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                                }

                                                commandInsert.Transaction = transaction;
                                                commandInsert.ExecuteNonQuery();
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            WriteError("SQL query failed: " + query);
                                            DumpQueryParameters(parameters);
                                            WriteError("Parameter oid = " + newObjectId.ToString());
                                            throw;
                                        }

                                        // Save mapping between the old Id and the new Id
                                        objectIdMapping[(int)dataObjectId] = newObjectId;

                                        // Add the new object to the cache of ObjectFinder
                                        objectFinder.AddObjectToCache(newObjectId, street1, street2, number1, number2, number3, addrMisc, district,
                                            data.ContainsKey("ulkod") ? data["ulkod"] : null,
                                            data.ContainsKey("ulkod2") ? data["ulkod2"] : null,
                                            null,
                                            data.ContainsKey("szag") ? data["szag"] : null,
                                            data.ContainsKey("v_jil") ? data["v_jil"] : null,
                                            data.ContainsKey("v_nej") ? data["v_nej"] : null,
                                            1);

                                        // Save the new ID to our database
                                        createdObjects[(int)dataEntryId] = newObjectId;
                                    }
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }

            // Write the debug info
            foreach (KeyValuePair<int, int> pair in matchedObjects)
            {
                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE object_1nf SET match_id = @mid WHERE id = @eid", connectionSqlServer))
                {
                    cmdUpdate.Parameters.Add(new SqlParameter("eid", pair.Key));
                    cmdUpdate.Parameters.Add(new SqlParameter("mid", pair.Value));
                    cmdUpdate.ExecuteNonQuery();
                }
            }

            foreach (KeyValuePair<int, int> pair in createdObjects)
            {
                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE object_1nf SET create_id = @cid WHERE id = @eid", connectionSqlServer))
                {
                    cmdUpdate.Parameters.Add(new SqlParameter("eid", pair.Key));
                    cmdUpdate.Parameters.Add(new SqlParameter("cid", pair.Value));
                    cmdUpdate.ExecuteNonQuery();
                }
            }
        }

        private void CorrectKnownAddrMisspellings(ref string street1, ref string street2,
            ref string number1, ref string number2, ref string number3, ref string addrMisc)
        {
            if (addrMisc == "0")
            {
                addrMisc = "";
            }

            if (number1.Contains(GUKV.DataMigration.Properties.Resources.AddrNoInfo1) ||
                number1.Contains(GUKV.DataMigration.Properties.Resources.AddrNoInfo2))
            {
                number1 = "";
            }

            if (number2.Contains(GUKV.DataMigration.Properties.Resources.AddrNoInfo1) ||
                number2.Contains(GUKV.DataMigration.Properties.Resources.AddrNoInfo2))
            {
                number2 = "";
            }

            if (number3.Contains(GUKV.DataMigration.Properties.Resources.AddrNoInfo1) ||
                number3.Contains(GUKV.DataMigration.Properties.Resources.AddrNoInfo2))
            {
                number3 = "";
            }

            if (addrMisc.Contains(GUKV.DataMigration.Properties.Resources.AddrNoInfo1) ||
                addrMisc.Contains(GUKV.DataMigration.Properties.Resources.AddrNoInfo2) ||
                addrMisc.Contains(GUKV.DataMigration.Properties.Resources.AddrShutova) ||
                addrMisc.Contains(GUKV.DataMigration.Properties.Resources.AddrSmolicha))
            {
                addrMisc = "";
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetGlushkova) &&
                number2.Contains(GUKV.DataMigration.Properties.Resources.NumberAslashA))
            {
                number2 = GUKV.DataMigration.Properties.Resources.NumberAblockA;
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetDegtarivska) &&
                number1.Contains("25"))
            {
                if (number2.Length > 0 && number3.Length == 0)
                {
                    while (number2.StartsWith("0"))
                    {
                        number2 = number2.Remove(0, 1);
                    }

                    if (!number2.ToUpper().StartsWith(GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect))
                    {
                        number2 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + number2;
                    }
                }
                else if (number3.Length > 0 && number2.Length == 0)
                {
                    while (number3.StartsWith("0"))
                    {
                        number3 = number3.Remove(0, 1);
                    }

                    if (!number3.ToUpper().StartsWith(GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect))
                    {
                        number3 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + number3;
                    }
                }

                if (number3 == "01" && number2.Contains(". 1"))
                {
                    number3 = "";
                }

                if (number3 == "03" && number2.Contains(". 3"))
                {
                    number3 = "";
                }

                if (number3 == "07" && number2.Contains(". 7"))
                {
                    number3 = "";
                }
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetKurenivska) &&
                number1.Contains("16") &&
                number2.Length > 0 &&
                number3.Length > 0)
            {
                number3 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + number3;
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetZhukova) &&
                number1 == "2" && number2 == "" && number3 == "2")
            {
                number3 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + number3;
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetKomarova) &&
                number1 == "3" && number2 == "" && number3 == "2")
            {
                number3 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + number3;
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetIllinska) &&
                number1 == "3" && number2.Contains("/7") && number3.StartsWith("3 "))
            {
                number2 = GUKV.DataMigration.Properties.Resources.AddrIllinska37A3k;
                number3 = "";
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetHarkivske) &&
                number1.Contains("121"))
            {
                if (number3.Length > 0)
                {
                    number3 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + number3;
                }
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetRustaveli) &&
                number1.Contains("26"))
            {
                if (number3.StartsWith(GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefix4))
                {
                    number3 = number3.Insert(1, ".");
                }
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetKopylivska) &&
                number2.Contains(GUKV.DataMigration.Properties.Resources.AddrKopylivska1_7_k2_correct))
            {
                number2 = GUKV.DataMigration.Properties.Resources.AddrIllinska37A3k;
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetKopylivska) &&
                number1 == "67" && number2 == "" && number3 == "10")
            {
                number2 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + "10";
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetKotelnikova) &&
                number1 == "95" && number2 == "" && number3 == "3")
            {
                number3 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + number3;
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetKotelnikova) &&
                number1 == "51" && number2 == "" && number3 == "13")
            {
                number2 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + "13";
                number3 = "";
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetFranka) &&
                number1 == "24" && number2 == GUKV.DataMigration.Properties.Resources.AddrFranka24A_wrong)
            {
                number2 = GUKV.DataMigration.Properties.Resources.AddrFranka24A_correct;
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.Street40LetOkt) &&
                number1 == "118" && number2 == "" && number3 == "2")
            {
                number3 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + number3;
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.Street40LetOkt) &&
                number1 == "120" && number2 == "/1" && number3 == "")
            {
                number2 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + "1";
            }

            if (addrMisc.Contains(GUKV.DataMigration.Properties.Resources.StreetParkPushkina) &&
                number1 == "40" && number2 == "" && number3 == "")
            {
                number2 = GUKV.DataMigration.Properties.Resources.BuildingAddressNoNumber + "1";
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetBehterevskyy) && number1 == "15")
            {
                if (number2 == GUKV.DataMigration.Properties.Resources.AddrBehterevskyyLit2)
                {
                    number2 = "/2";
                }
                else if (number2 == GUKV.DataMigration.Properties.Resources.AddrBehterevskyyLit17)
                {
                    number2 = "/17";
                }
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetDniprodz) &&
                number1 == "130" && number2 == GUKV.DataMigration.Properties.Resources.AddrDniprodz130K10B_wrong)
            {
                number2 = GUKV.DataMigration.Properties.Resources.AddrDniprodz130K10B_correct;
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetNaberezhnoPechDoroga) &&
               number1 == "4" && number2 == "(2)")
            {
                number2 = GUKV.DataMigration.Properties.Resources.BuildingKorpusPrefixCorrect + "2";
            }

            if (street1.Contains(GUKV.DataMigration.Properties.Resources.StreetGorkogo) &&
                number1 == "160" && number2 == GUKV.DataMigration.Properties.Resources.AddrGorkogoLitB)
            {
                number2 = GUKV.DataMigration.Properties.Resources.BuildingLitPrefix +
                    GUKV.DataMigration.Properties.Resources.AddrGorkogoLitB;
            }

            if (street1 == GUKV.DataMigration.Properties.Resources.StreetVozduhoflotskyyProv &&
                number1 == "49")
            {
                street1 = GUKV.DataMigration.Properties.Resources.StreetVozduhoflotskyyProsp;
            }
        }

        private void CreateOrganizationFieldMapping(Dictionary<string, string> mapping)
        {
            mapping.Clear();

            mapping.Add("is_arend", "is_arend");
            mapping.Add("full_name_obj", "full_name_obj");
            mapping.Add("short_name_obj", "short_name_obj");
            mapping.Add("post_index", "post_index");
            mapping.Add("name_ul", "name_ul");
            mapping.Add("nomer_doma", "nomer_doma");
            mapping.Add("nomer_korpus", "nomer_korpus");
            mapping.Add("kod_zkpo", "kod_zkpo");
            mapping.Add("fio_boss", "fio_boss");
            mapping.Add("post_boss", "post_boss");
            mapping.Add("tel_boss", "tel_boss");
            mapping.Add("koatuu", "koatuu");
            mapping.Add("kod_rayon2", "kod_rayon2");
            mapping.Add("kod_status", "kod_status");
            mapping.Add("kod_form_vlasn", "kod_form_vlasn");
            mapping.Add("kod_vidom_nal", "kod_vidom_nal");
            mapping.Add("kod_organ", "kod_organ");
            mapping.Add("kod_galuz", "kod_galuz");
            mapping.Add("kod_vid_dial", "kod_vid_dial");
            mapping.Add("kod_kved", "kod_kved");
            mapping.Add("kod_org_form", "kod_org_form");
            mapping.Add("kod_form_gosp", "kod_form_gosp");
            mapping.Add("persent_akcia", "persent_akcia");
            mapping.Add("pl_zagal", "pl_zagal");
            mapping.Add("pl_balans", "pl_balans");
            mapping.Add("pl_virob_prizn", "pl_virob_prizn");
            mapping.Add("pl_not_virob_prizn", "pl_not_virob_prizn");
            mapping.Add("pl_arenduetsya", "pl_arenduetsya");
            mapping.Add("pl_nadana_v_a", "pl_nadana_v_a");
            mapping.Add("pl_zn_bal", "pl_znyata_z_balansu");
            mapping.Add("prodaj", "pl_prodaj");
            mapping.Add("znes_bud", "pl_spisani_zneseni");
            mapping.Add("pered_na_bal", "pl_peredana_inshum");
        }

        private void ExportOrganizations(int reportId, int organizationId, FbTransaction transaction,
            HashSet<int> requiredOrganizations, HashSet<int> renters)
        {
            // Create mapping for the columns between our database and 1NF database
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            CreateOrganizationFieldMapping(mapping);
            string srcFieldList = GetSrcFieldList(mapping, true);

            // Create special mapping for UPDATE requests. We do not want to replace organization name and ZKPO,
            // because this information is more valid in 1NF database than in user reports
            Dictionary<string, string> mappingUpdate = new Dictionary<string, string>(mapping);

            mappingUpdate.Remove("full_name_obj");
            mappingUpdate.Remove("short_name_obj");
            mappingUpdate.Remove("kod_zkpo");

            // Debug information
            Dictionary<int, int> matchedOrganizations = new Dictionary<int, int>();
            Dictionary<int, int> createdOrganizations = new Dictionary<int, int>();

            // Get the list of modified or added organizations
            string query = "SELECT id, kod_obj, edited, " + srcFieldList + " FROM sorg_1nf WHERE report_id = @rid";

            int indexFullName = GetSrcFieldIndexFromMapping(mapping, "full_name_obj") + 3; // 3 is added because there are also "id", "kod_obj" and "edited" fields in the SELECT list
            int indexShortName = GetSrcFieldIndexFromMapping(mapping, "short_name_obj") + 3;
            int indexZkpo = GetSrcFieldIndexFromMapping(mapping, "kod_zkpo") + 3;
            int indexIndustry = GetSrcFieldIndexFromMapping(mapping, "kod_galuz") + 3;
            int indexSubIndustry = GetSrcFieldIndexFromMapping(mapping, "kod_vid_dial") + 3;
            int indexOwnership = GetSrcFieldIndexFromMapping(mapping, "kod_form_vlasn") + 3;

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataEntryId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataOrgId = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        int wasEdited = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);

                        if (dataOrgId is int && requiredOrganizations.Contains((int)dataOrgId))
                        {
                            // Try to find this organization in 1NF
                            string fullName = reader.IsDBNull(indexFullName) ? "" : reader.GetString(indexFullName).Trim().ToUpper();
                            string shortName = reader.IsDBNull(indexShortName) ? "" : reader.GetString(indexShortName).Trim().ToUpper();
                            string zkpo = reader.IsDBNull(indexZkpo) ? "" : reader.GetString(indexZkpo).Trim().ToUpper();
                            bool categorized = false;

                            int existingId = organizationFinder.FindOrganization(zkpo, fullName, shortName, false, out categorized);

                            if (existingId > 0)
                            {
                                // An existing organization found; save the mapping to it
                                organizationIdMapping[(int)dataOrgId] = existingId;

                                // Save the matched ID to our database
                                matchedOrganizations[(int)dataEntryId] = existingId;

                                if (wasEdited > 0)
                                {
                                    // Prepare the list of UPDATE parameters
                                    Dictionary<string, string> mappingForUpdate = new Dictionary<string, string>(mappingUpdate);

                                    if ((int)dataOrgId != organizationId)
                                    {
                                        // Remove the fields that can be entered by the report submitter only for themselves
                                        mappingForUpdate.Remove("kod_galuz");
                                        mappingForUpdate.Remove("kod_vid_dial");
                                    }

                                    if (!renters.Contains((int)dataOrgId))
                                    {
                                        // Remove the fields that can be entered by the report submitter only for renters
                                        mappingForUpdate.Remove("kod_form_vlasn");
                                    }

                                    string destFieldList = GetUpdateFieldList(reader, mappingForUpdate, parameters, null);

                                    if (parameters.Count > 1) // "is_arend" parameter is always present
                                    {
                                        // Create an archived state if necessary
                                        int archRecordId = archiver1NF.CreateOrganizationArchiveRecord(connection1NF, (int)dataOrgId, transaction);

                                        // Prepare the UPDATE statement
                                        query = "UPDATE SORG_1NF SET ISP = @isp, EIS_MODIFIED_BY = @isp, DT = @dt, USER_KOREG = @ukor, DATE_KOREG = @dtkor, ";

                                        if (archRecordId > 0)
                                        {
                                            query += "ARCH_KOD = @acod, ";
                                        }

                                        query += destFieldList + " WHERE KOD_OBJ = @orgid";

                                        try
                                        {
                                            using (FbCommand commandUpdate = new FbCommand(query, connection1NF))
                                            {
                                                commandUpdate.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                                commandUpdate.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                                commandUpdate.Parameters.Add(new FbParameter("ukor", "Auto-import"));
                                                commandUpdate.Parameters.Add(new FbParameter("dtkor", DateTime.Now.Date));
                                                commandUpdate.Parameters.Add(new FbParameter("orgid", dataOrgId));

                                                if (archRecordId > 0)
                                                {
                                                    commandUpdate.Parameters.Add(new FbParameter("acod", archRecordId));
                                                }

                                                foreach (KeyValuePair<string, object> pair in parameters)
                                                {
                                                    commandUpdate.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                                }

                                                commandUpdate.Transaction = transaction;
                                                commandUpdate.ExecuteNonQuery();
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            WriteError("SQL query failed: " + query);
                                            DumpQueryParameters(parameters);
                                            WriteError("Parameter orgid = " + dataOrgId.ToString());
                                            WriteError("Parameter acod = " + archRecordId.ToString());
                                            throw;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // No match found; add a new organization
                                string paramList = "";
                                string destFieldList = GetInsertFieldList(reader, out paramList, mapping, parameters, null);

                                if (parameters.Count > 1 && (fullName.Length > 0 || shortName.Length > 0) && organizationFinder.IsValidZKPO(zkpo))
                                {
                                    // Generate new organization Id
                                    int newOrgId = GenerateOrganizationId1NF(transaction);

                                    if (newOrgId > 0)
                                    {
                                        // Prepare the INSERT statement
                                        query = "INSERT INTO SORG_1NF (KOD_OBJ, KOD_STAN, LAST_SOST, ISP, EIS_MODIFIED_BY, DT, USER_KOREG, DATE_KOREG, DELETED, " + destFieldList +
                                            ") VALUES (@orgd, 1, 1, @isp, @isp, @dt, @ukor, @dtkor, 0, " + paramList + ")";

                                        try
                                        {
                                            using (FbCommand commandInsert = new FbCommand(query, connection1NF))
                                            {
                                                commandInsert.Parameters.Add(new FbParameter("orgd", newOrgId));
                                                commandInsert.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                                commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                                commandInsert.Parameters.Add(new FbParameter("ukor", "Auto-import"));
                                                commandInsert.Parameters.Add(new FbParameter("dtkor", DateTime.Now.Date));

                                                foreach (KeyValuePair<string, object> pair in parameters)
                                                {
                                                    commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                                }

                                                commandInsert.Transaction = transaction;
                                                commandInsert.ExecuteNonQuery();
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            WriteError("SQL query failed: " + query);
                                            DumpQueryParameters(parameters);
                                            WriteError("Parameter orgd = " + newOrgId.ToString());
                                            throw;
                                        }

                                        // Save mapping between the old Id and the new Id
                                        organizationIdMapping[(int)dataOrgId] = newOrgId;

                                        // Save the new ID to our database
                                        createdOrganizations[(int)dataEntryId] = newOrgId;

                                        // Add the inserted organization to the OrganizationFinder
                                        organizationFinder.AddOrganizationToCache(newOrgId, zkpo, fullName, shortName,
                                            reader.IsDBNull(indexIndustry) ? null : reader.GetValue(indexIndustry),
                                            reader.IsDBNull(indexSubIndustry) ? null : reader.GetValue(indexSubIndustry),
                                            reader.IsDBNull(indexOwnership) ? null : reader.GetValue(indexOwnership));
                                    }
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }

            // Write the debug info
            foreach (KeyValuePair<int, int> pair in matchedOrganizations)
            {
                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE sorg_1nf SET match_id = @mid WHERE id = @eid", connectionSqlServer))
                {
                    cmdUpdate.Parameters.Add(new SqlParameter("eid", pair.Key));
                    cmdUpdate.Parameters.Add(new SqlParameter("mid", pair.Value));
                    cmdUpdate.ExecuteNonQuery();
                }
            }

            foreach (KeyValuePair<int, int> pair in createdOrganizations)
            {
                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE sorg_1nf SET create_id = @cid WHERE id = @eid", connectionSqlServer))
                {
                    cmdUpdate.Parameters.Add(new SqlParameter("eid", pair.Key));
                    cmdUpdate.Parameters.Add(new SqlParameter("cid", pair.Value));
                    cmdUpdate.ExecuteNonQuery();
                }
            }
        }

        private void CreateBalansFieldMapping(Dictionary<string, string> mapping)
        {
            mapping.Clear();

            mapping.Add("form_vlasn", "form_vlasn");
            mapping.Add("pravo", "pravo");
            mapping.Add("kindobj", "kindobj");
            mapping.Add("typeobj", "typeobj");
            mapping.Add("date_bti", "date_bti");
            mapping.Add("obj_kodbti", "obj_kodbti");
            mapping.Add("reesno", "reesno");
            mapping.Add("grpurp", "grpurp");
            mapping.Add("purpose", "purpose");
            mapping.Add("purp_str", "purp_str");
            mapping.Add("floats", "floats");
            mapping.Add("texstan_bal", "texstan");
            mapping.Add("sqr_zag", "sqr_zag");
            mapping.Add("sqr_nej", "sqr_nej");
            mapping.Add("sqr_gurtoj", "sqr_gurtoj");
            mapping.Add("sqr_kor", "sqr_kor");
            mapping.Add("sqr_pidv", "sqr_pidv");
            mapping.Add("sqr_vlas", "sqr_vlas");
            mapping.Add("sqr_free", "sqr_free");
            mapping.Add("v_dog_orenda", "v_dog_orenda");
            mapping.Add("v_sqr_orenda", "v_sqr_orenda");
            mapping.Add("balans_vartist", "balans_vartist");
            mapping.Add("zal_vart", "zal_vart");
            mapping.Add("znos", "znos");
            mapping.Add("znos_date", "znos_date");
            mapping.Add("vartist_rink", "vartist_rink");
            mapping.Add("rink_vart_dt", "rink_vart_dt");
            mapping.Add("vartist_exp_v", "vartist_exp_v");
            mapping.Add("dtexp", "dtexp");
        }

        private void ExportBalans(int reportId, int organizationId, FbTransaction transaction)
        {
            // Get the list of deleted balans entries
            string query = "SELECT balans_id, [object_id] FROM balans_1nf WHERE unloading <> 0 AND deleted <> 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataBalansId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataBuldingId = reader.IsDBNull(1) ? null : reader.GetValue(1);

                        if (dataBalansId is int && dataBuldingId is int)
                        {
                            // Prepare the UPDATE statement
                            query = "UPDATE BALANS_1NF SET DELETED = 1, DTDELETE = @dtdel, ISP = @isp, EIS_MODIFIED_BY = @isp, DT = @dt, EDT = @edt WHERE ID = @bid";

                            try
                            {
                                using (FbCommand commandDelete = new FbCommand(query, connection1NF))
                                {
                                    commandDelete.Parameters.Add(new FbParameter("dtdel", DateTime.Now.Date));
                                    commandDelete.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                    commandDelete.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                    commandDelete.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
                                    commandDelete.Parameters.Add(new FbParameter("bid", dataBalansId));

                                    commandDelete.Transaction = transaction;
                                    commandDelete.ExecuteNonQuery();

                                    // Unregister this balans object from our local cache
                                    UnregisterBalansObject((int)dataBalansId, (int)dataBuldingId);
                                }
                            }
                            catch (Exception)
                            {
                                WriteError("SQL query failed: " + query);
                                WriteError("Parameter bid = " + dataBalansId.ToString());
                                throw;
                            }
                        }
                    }

                    reader.Close();
                }
            }

            // Create mapping for the columns between our database and 1NF database
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            CreateBalansFieldMapping(mapping);
            string srcFieldList = GetSrcFieldList(mapping, true);

            // Debug information
            Dictionary<int, int> matchedBalans = new Dictionary<int, int>();
            Dictionary<int, int> createdBalans = new Dictionary<int, int>();

            // Get the list of modified or added balans entries
            query = "SELECT id, balans_id, [object_id], organization_id, " + srcFieldList + " FROM balans_1nf WHERE edited <> 0 AND deleted = 0 AND report_id = @rid";

            int indexPurposeGroup = GetSrcFieldIndexFromMapping(mapping, "grpurp") + 4; // 4 is added because there are 4 additional fields in the SELECT list
            int indexPurpose = GetSrcFieldIndexFromMapping(mapping, "purpose") + 4;
            int indexPurposeStr = GetSrcFieldIndexFromMapping(mapping, "purp_str") + 4;
            int indexSqrZag = GetSrcFieldIndexFromMapping(mapping, "sqr_zag") + 4;
            int indexSqrNonHabit = GetSrcFieldIndexFromMapping(mapping, "sqr_nej") + 4;
            int indexFloors = GetSrcFieldIndexFromMapping(mapping, "floats") + 4;
            int indexZalCost = GetSrcFieldIndexFromMapping(mapping, "zal_vart") + 4;

            // We need to override 'sqr_nej' parameter - make it identical to the 'sqr_zag' parameter
            Dictionary<string, object> overrides = new Dictionary<string, object>();

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int entryId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        int balansId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                        int objectId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                        int orgId = reader.IsDBNull(3) ? -1 : reader.GetInt32(3);

                        object dataPurposeGroup = reader.IsDBNull(indexPurposeGroup) ? null : reader.GetValue(indexPurposeGroup);
                        object dataPurpose = reader.IsDBNull(indexPurpose) ? null : reader.GetValue(indexPurpose);
                        object dataPurposeStr = reader.IsDBNull(indexPurposeStr) ? null : reader.GetValue(indexPurposeStr);
                        object dataSqrZag = reader.IsDBNull(indexSqrZag) ? null : reader.GetValue(indexSqrZag);
                        // object dataSqrNonHabit = reader.IsDBNull(indexSqrNonHabit) ? null : reader.GetValue(indexSqrNonHabit);
                        object dataFloors = reader.IsDBNull(indexFloors) ? null : reader.GetValue(indexFloors);
                        object dataZalCost = reader.IsDBNull(indexZalCost) ? null : reader.GetValue(indexZalCost);

                        // Check if object and organization were successfully exported
                        objectId = objectIdMapping.ContainsKey(objectId) ? objectIdMapping[objectId] : -1;
                        orgId = organizationIdMapping.ContainsKey(orgId) ? organizationIdMapping[orgId] : -1;

                        // Validate the balans object according to validatin criteria
                        if (entryId > 0 && balansId > 0 && objectId > 0 && orgId > 0 &&
                            dataSqrZag is decimal && ((decimal)dataSqrZag) > 0 &&
                            dataPurposeGroup is int && ((int)dataPurposeGroup) > 0 &&
                            dataPurpose is int && ((int)dataPurpose) > 0)
                        {
                            // Override the 'sqr_nej' parameter
                            overrides["sqr_nej"] = dataSqrZag;

                            // Try to find a matching Balans record
                            BalansObjectInfo info = GetExistingBalansObject(orgId, objectId, dataPurposeGroup, dataPurpose,
                                dataPurposeStr, dataSqrZag/*, dataSqrNonHabit*/, dataFloors);

                            if (info != null)
                            {
                                // Save mapping between the old Id and the found Id
                                balansIdMapping[balansId] = info.balansId;

                                // Save mapping to the debug database
                                matchedBalans[entryId] = info.balansId;

                                string destFieldList = GetUpdateFieldList(reader, mapping, parameters, overrides);

                                if (parameters.Count > 0)
                                {
                                    // Create an archived state if necessary
                                    int archRecordId = archiver1NF.CreateBalansArchiveRecord(connection1NF, info.balansId, transaction);

                                    // Prepare the UPDATE statement
                                    // No need to update object ID - it can not change within an existing BALANS_1NF record
                                    query = "UPDATE BALANS_1NF SET ISP = @isp, EIS_MODIFIED_BY = @isp, DT = @dt, EDT = @edt, org = @org," +
                                        " ARCH_KOD = @acod, PRIZNAK1NF = 1, UPD_SOURCE = 2, DELETED = 0, " + destFieldList + " WHERE ID = @bid";

                                    try
                                    {
                                        using (FbCommand commandUpdate = new FbCommand(query, connection1NF))
                                        {
                                            // commandUpdate.Parameters.Add(new FbParameter("obj", dataObjectId)); - See comment above
                                            commandUpdate.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                            commandUpdate.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                            commandUpdate.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
                                            commandUpdate.Parameters.Add(new FbParameter("org", orgId));
                                            commandUpdate.Parameters.Add(new FbParameter("bid", info.balansId));
                                            commandUpdate.Parameters.Add(new FbParameter("acod", archRecordId));

                                            foreach (KeyValuePair<string, object> pair in parameters)
                                            {
                                                commandUpdate.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                            }

                                            commandUpdate.Transaction = transaction;
                                            commandUpdate.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        WriteError("SQL query failed: " + query);
                                        DumpQueryParameters(parameters);
                                        WriteError("Parameter org = " + orgId.ToString());
                                        WriteError("Parameter bid = " + info.balansId.ToString());
                                        WriteError("Parameter acod = " + archRecordId.ToString());
                                        throw;
                                    }
                                }
                            }
                            else
                            {
                                // No match found; create a new Balans entry
                                string paramList = "";
                                string destFieldList = GetInsertFieldList(reader, out paramList, mapping, parameters, overrides);

                                if (parameters.Count > 0)
                                {
                                    // Generate new balans Id
                                    int newBalansId = GenerateNewId1NF("GEN_BALANS_1NF", transaction);

                                    if (newBalansId > 0)
                                    {
                                        // Prepare the INSERT statement
                                        query = "INSERT INTO BALANS_1NF (ID, STAN, OBJECT, OBJECT_STAN, ORG, ORG_STAN, ISP, EIS_MODIFIED_BY, DT, EDT, LYEAR, PRIZNAK1NF, UPD_SOURCE, DELETED, " +
                                            destFieldList + ") VALUES (@bid, @stan, @obj, 1, @org, 1, @isp, @isp, @dt, @edt, @lyear, 1, 2, 0, " + paramList + ")";

                                        try
                                        {
                                            using (FbCommand commandInsert = new FbCommand(query, connection1NF))
                                            {
                                                commandInsert.Parameters.Add(new FbParameter("bid", newBalansId));
                                                commandInsert.Parameters.Add(new FbParameter("stan", DateTime.Now.Date));
                                                commandInsert.Parameters.Add(new FbParameter("obj", objectId));
                                                commandInsert.Parameters.Add(new FbParameter("org", orgId));
                                                commandInsert.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                                commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                                commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
                                                commandInsert.Parameters.Add(new FbParameter("lyear", DateTime.Now.Year));

                                                foreach (KeyValuePair<string, object> pair in parameters)
                                                {
                                                    commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                                }

                                                commandInsert.Transaction = transaction;
                                                commandInsert.ExecuteNonQuery();
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            WriteError("SQL query failed: " + query);
                                            DumpQueryParameters(parameters);
                                            WriteError("Parameter bid = " + newBalansId.ToString());
                                            WriteError("Parameter obj = " + objectId.ToString());
                                            WriteError("Parameter org = " + orgId.ToString());
                                            throw;
                                        }

                                        // Save mapping between the old Id and the new Id
                                        balansIdMapping[balansId] = newBalansId;

                                        // Save the produced ID to the debug database
                                        createdBalans[entryId] = newBalansId;

                                        // Add the new entry to the cache
                                        RegisterBalansObject(newBalansId, orgId, objectId, dataPurposeGroup, dataPurpose, dataPurposeStr,
                                            dataSqrZag/*, dataSqrNonHabit*/, dataFloors);
                                    }
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }

            // Write the debug info
            foreach (KeyValuePair<int, int> pair in matchedBalans)
            {
                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE balans_1nf SET match_id = @mid WHERE id = @eid", connectionSqlServer))
                {
                    cmdUpdate.Parameters.Add(new SqlParameter("eid", pair.Key));
                    cmdUpdate.Parameters.Add(new SqlParameter("mid", pair.Value));
                    cmdUpdate.ExecuteNonQuery();
                }
            }

            foreach (KeyValuePair<int, int> pair in createdBalans)
            {
                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE balans_1nf SET create_id = @cid WHERE id = @eid", connectionSqlServer))
                {
                    cmdUpdate.Parameters.Add(new SqlParameter("eid", pair.Key));
                    cmdUpdate.Parameters.Add(new SqlParameter("cid", pair.Value));
                    cmdUpdate.ExecuteNonQuery();
                }
            }
        }

        private void ExportBalansDocs(int reportId, int organizationId, FbTransaction transaction)
        {
            // Process documents for each balans entry (added or modified)
            string query = "SELECT id, balans_id, [object_id], dockind_korist, docnum_korist, docdate_korist," +
                " dockind_bal, docnum_bal, docdate_bal, dockind_vidch, docnum_vidch, docdate_vidch" +
                " FROM balans_1nf WHERE edited <> 0 AND deleted = 0 AND report_id = @rid";

            Dictionary<int, int> mapRowId2BalansId = new Dictionary<int, int>();
            Dictionary<int, int> mapRowId2ObjectId = new Dictionary<int, int>();
            Dictionary<int, int> mapRowId2DocIdKorist = new Dictionary<int, int>();
            Dictionary<int, int> mapRowId2DocIdBalans = new Dictionary<int, int>();
            Dictionary<int, int> mapRowId2DocIdDismiss = new Dictionary<int, int>();

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataRowId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataBalansId = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object dataObjectId = reader.IsDBNull(2) ? null : reader.GetValue(2);

                        if (dataRowId is int && dataBalansId is int && dataObjectId is int)
                        {
                            // Check if object and balans were successfully exported
                            int objectId = objectIdMapping.ContainsKey((int)dataObjectId) ? objectIdMapping[(int)dataObjectId] : -1;
                            int balansId = balansIdMapping.ContainsKey((int)dataBalansId) ? balansIdMapping[(int)dataBalansId] : -1;

                            if (objectId > 0 && balansId > 0)
                            {
                                mapRowId2BalansId.Add((int)dataRowId, balansId);
                                mapRowId2ObjectId.Add((int)dataRowId, objectId);

                                // Process the document about 'pravo na koristuvannia'
                                int docIdKorist = FindOrCreateDocument1NF(reader, 3, 4, 5, transaction);

                                if (docIdKorist > 0)
                                {
                                    mapRowId2DocIdKorist.Add((int)dataRowId, docIdKorist);
                                }

                                // Process the document about 'peredacha na balans'
                                int docIdBalans = FindOrCreateDocument1NF(reader, 6, 7, 8, transaction);

                                if (docIdBalans > 0)
                                {
                                    mapRowId2DocIdBalans.Add((int)dataRowId, docIdBalans);
                                }

                                // Process the document about 'vidchuzhennia'
                                int docIdDismiss = FindOrCreateDocument1NF(reader, 9, 10, 11, transaction);

                                if (docIdDismiss > 0)
                                {
                                    mapRowId2DocIdDismiss.Add((int)dataRowId, docIdDismiss);
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }

            foreach (KeyValuePair<int, int> pair in mapRowId2BalansId)
            {
                int rowId = pair.Key;
                int balansId = pair.Value;
                int objectId = mapRowId2ObjectId[pair.Key];
                int docId = -1;

                if (mapRowId2DocIdKorist.TryGetValue(rowId, out docId))
                {
                    int relationId = AddDocRelationToObject(rowId, balansId, organizationId, objectId, docId, transaction);

                    if (relationId > 0)
                    {
                        AddDocRelationToBalans(balansId, objectId, relationId, transaction);
                    }
                }

                if (mapRowId2DocIdBalans.TryGetValue(rowId, out docId))
                {
                    int relationId = AddDocRelationToObject(rowId, balansId, organizationId, objectId, docId, transaction);

                    if (relationId > 0)
                    {
                        AddDocRelationToBalans(balansId, objectId, relationId, transaction);
                    }
                }

                if (mapRowId2DocIdDismiss.TryGetValue(rowId, out docId))
                {
                    int relationId = AddDocRelationToObject(rowId, balansId, organizationId, objectId, docId, transaction);

                    if (relationId > 0)
                    {
                        AddDocRelationToBalans(balansId, objectId, relationId, transaction);
                    }
                }
            }
        }

        private void ExportBalansStreetNames(int reportId, int organizationId, FbTransaction transaction)
        {
            // Update street names for all modified and added objects
            string query = "SELECT balans_id, [object_id] FROM balans_1nf WHERE edited <> 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataBalansId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataObjectId = reader.IsDBNull(1) ? null : reader.GetValue(1);

                        if (dataBalansId is int && dataObjectId is int)
                        {
                            // Check if object and balans were successfully exported
                            int objectId = objectIdMapping.ContainsKey((int)dataObjectId) ? objectIdMapping[(int)dataObjectId] : -1;
                            int balansId = balansIdMapping.ContainsKey((int)dataBalansId) ? balansIdMapping[(int)dataBalansId] : -1;

                            if (objectId > 0 && balansId > 0)
                            {
                                // Get the object properties from ObjectFinder
                                ObjectInfo info = objectFinder.GetObjectInfo(objectId);

                                if (info != null)
                                {
                                    if (info.street is string ||
                                        info.nomer1 is string ||
                                        info.nomer2 is string ||
                                        info.nomer3 is string ||
                                        info.addrMiscInfo is string)
                                    {
                                        string streetName = info.street is string ? ((string)info.street).Trim() : "";
                                        string nomer1 = info.nomer1 is string ? ((string)info.nomer1).Trim() : "";
                                        string nomer2 = info.nomer2 is string ? ((string)info.nomer2).Trim() : "";
                                        string nomer3 = info.nomer3 is string ? ((string)info.nomer3).Trim() : "";
                                        string addrMisc = info.addrMiscInfo is string ? ((string)info.addrMiscInfo).Trim() : "";

                                        string paramStreetCode = (info.streetId is int) ? ", OBJ_KODUL = @p6" : "";

                                        string update = "UPDATE BALANS_1NF SET OBJ_ULNAME = @p1, OBJ_NOMER1 = @p2, OBJ_NOMER2 = @p3, OBJ_NOMER3 = @p4, " +
                                            "OBJ_ULADRDOP = @p1, OBJ_ADRDOP = @p5 " + paramStreetCode + " WHERE ID = " + balansId.ToString();

                                        using (FbCommand fbCmd = new FbCommand(update, connection1NF))
                                        {
                                            fbCmd.Transaction = transaction;

                                            fbCmd.Parameters.Add(new FbParameter("p1", streetName));
                                            fbCmd.Parameters.Add(new FbParameter("p2", nomer1));
                                            fbCmd.Parameters.Add(new FbParameter("p3", nomer2));
                                            fbCmd.Parameters.Add(new FbParameter("p4", nomer3));
                                            fbCmd.Parameters.Add(new FbParameter("p5", addrMisc));

                                            if (info.streetId is int)
                                            {
                                                fbCmd.Parameters.Add(new FbParameter("p6", info.streetId));
                                            }

                                            fbCmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateArendaFieldMapping(Dictionary<string, string> mapping)
        {
            mapping.Clear();

            mapping.Add("dogkind", "dogkind");
            mapping.Add("subar", "subar");
            mapping.Add("dognum", "dognum");
            mapping.Add("dogdate", "dogdate");
            mapping.Add("start_orenda", "start_orenda");
            mapping.Add("finish_orenda", "finish_orenda");
            mapping.Add("finish_orenda_fact", "finish_orenda_fact");
            mapping.Add("pidstava_fact", "pidstava_fact");
            mapping.Add("square", "square");
            mapping.Add("plata_dogovor", "plata_dogovor");
            mapping.Add("vartist_exp", "vartist_exp");
            mapping.Add("dtexp_dog", "dtexp");
            mapping.Add("kilk_prim", "kilk_prim");
        }

        private void ExportArenda(int reportId, FbTransaction transaction)
        {
            // Create mapping for the columns between our database and 1NF database
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            CreateArendaFieldMapping(mapping);
            string srcFieldList = GetSrcFieldList(mapping, true);

            // Debug information
            Dictionary<int, int> matchedArenda = new Dictionary<int, int>();
            Dictionary<int, int> createdArenda = new Dictionary<int, int>();
            Dictionary<int, int> agreementIdMap = new Dictionary<int, int>();

            // Get the list of modified or added arenda entries
            string query = "SELECT id, id_arenda, orgiv, balid, org_kod, balans_kod, object_kod, " + srcFieldList +
                " FROM arenda_1nf WHERE edited <> 0 AND deleted = 0 AND report_id = @rid";

            int indexAgrNum = GetSrcFieldIndexFromMapping(mapping, "dognum") + 7; // 7 is added because there are 7 additional fields in the SELECT list
            int indexAgrDate = GetSrcFieldIndexFromMapping(mapping, "dogdate") + 7;
            int indexStartDate = GetSrcFieldIndexFromMapping(mapping, "start_orenda") + 7;
            int indexFinishDate = GetSrcFieldIndexFromMapping(mapping, "finish_orenda") + 7;
            int indexSquare = GetSrcFieldIndexFromMapping(mapping, "square") + 7;
            int indexPayment = GetSrcFieldIndexFromMapping(mapping, "plata_dogovor") + 7;

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int entryId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        int arendaId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                        int orgGiverId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                        int orgRenterId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                        int orgBalansId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                        int objectId = reader.IsDBNull(6) ? -1 : reader.GetInt32(6);

                        object dataBalansId = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object dataAgrNum = reader.IsDBNull(indexAgrNum) ? null : reader.GetValue(indexAgrNum);
                        object dataAgrDate = reader.IsDBNull(indexAgrDate) ? null : reader.GetValue(indexAgrDate);
                        object dataStartDate = reader.IsDBNull(indexStartDate) ? null : reader.GetValue(indexStartDate);
                        object dataFinishDate = reader.IsDBNull(indexFinishDate) ? null : reader.GetValue(indexFinishDate);
                        object dataSquare = reader.IsDBNull(indexSquare) ? null : reader.GetValue(indexSquare);
                        object dataPayment = reader.IsDBNull(indexPayment) ? null : reader.GetValue(indexPayment);

                        // Object, organizations and balans can be mapped
                        objectId = objectIdMapping.ContainsKey(objectId) ? objectIdMapping[objectId] : -1;
                        orgRenterId = organizationIdMapping.ContainsKey(orgRenterId) ? organizationIdMapping[orgRenterId] : -1;
                        orgGiverId = organizationIdMapping.ContainsKey(orgGiverId) ? organizationIdMapping[orgGiverId] : -1;
                        orgBalansId = organizationIdMapping.ContainsKey(orgBalansId) ? organizationIdMapping[orgBalansId] : -1;

                        if (dataBalansId is int)
                        {
                            if (balansIdMapping.ContainsKey((int)dataBalansId))
                            {
                                dataBalansId = balansIdMapping[(int)dataBalansId];
                            }

                            // Avoid zero value, it is not a legal balans ID
                            if ((int)dataBalansId == 0)
                            {
                                dataBalansId = null;
                            }
                        }

                        if (arendaId > 0 &&
                            orgGiverId > 0 &&
                            orgRenterId > 0 &&
                            orgBalansId > 0 &&
                            objectId > 0 &&
                            dataSquare is decimal && ((decimal)dataSquare) > 0 &&
                            /* dataPayment is decimal && ((decimal)dataPayment) > 0 && */
                            dataStartDate is DateTime)
                        {
                            int oldArendaId = arendaId;
                            query = "";

                            // Try to find a matching Balans record
                            ArendaAgreementInfo info = GetExistingRentAgreement(dataAgrNum, dataAgrDate, objectId,
                                orgRenterId, orgGiverId, orgBalansId, dataStartDate, dataFinishDate, dataSquare);

                            if (info != null)
                            {
                                // Save mapping between the old Id and the found Id
                                arendaIdMapping[arendaId] = info.arendaId;

                                // Save mapping to the debug database
                                matchedArenda[entryId] = info.arendaId;

                                string destFieldList = GetUpdateFieldList(reader, mapping, parameters, null);

                                if (parameters.Count > 0)
                                {
                                    // Create an archived state if necessary
                                    int archRecordId = archiver1NF.CreateArendaArchiveRecord(connection1NF, info.arendaId, transaction);

                                    string balansUpdate = (dataBalansId is int) ? ", BALID = @balid"  : "";

                                    query = "UPDATE ARENDA1NF SET ORGIV = @orgiv" + balansUpdate + ", ORG_KOD = @orgkod, BALANS_KOD = @balanskod," +
                                        " OBJECT_KOD = @objkod, ISP = @isp, EIS_MODIFIED_BY = @isp, DT = @dt, EDT = @dt, SYSTEM_DT = @dt, SYSTEM_ISP = @isp, " +
                                        "ARCH_KOD = @acod, UPDT_SOURCE = 2, PRIZNAK_1NF = 1, DELETED = 0, " + destFieldList + " WHERE ID = @aid";

                                    parameters.Add("acod", archRecordId);

                                    if (dataBalansId is int)
                                        parameters.Add("balid", dataBalansId);
                                }

                                arendaId = info.arendaId;
                            }
                            else
                            {
                                // No match found; this is a new rent agreement
                                string paramList = "";
                                string destFieldList = GetInsertFieldList(reader, out paramList, mapping, parameters, null);

                                if (parameters.Count > 0)
                                {
                                    int newArendaId = GenerateNewId1NF("GEN_ARENDA1NF", transaction);

                                    if (newArendaId > 0)
                                    {
                                        // Save the mapping between old Arenda ID and new Arenda ID
                                        arendaIdMapping[arendaId] = newArendaId;

                                        // Save the produced ID to the debug database
                                        createdArenda[entryId] = newArendaId;

                                        string balansInsert = (dataBalansId is int) ? ", BALID" : "";
                                        string balansInsertParam = (dataBalansId is int) ? ", @balid" : "";

                                        query = "INSERT INTO ARENDA1NF (ID, OBJECT_KOD, OBJECT_KODSTAN, BALANS_KOD, BALANS_KODSTAN," +
                                            " ORG_KOD, ORG_KODSTAN, ORGIV, ORGIV_STAN" + balansInsert + ", NOACTIVE, DELETED," +
                                            " ISP, EIS_MODIFIED_BY, DT, EDT, SYSTEM_DT, SYSTEM_ISP, UPDT_SOURCE, PRIZNAK_1NF, " + destFieldList +
                                            ") VALUES (@aid, @objkod, 1, @balanskod, 1, @orgkod, 1, @orgiv, 1" + balansInsertParam + ", 0, 0," +
                                            " @isp, @isp, @dt, @dt, @dt, @isp, 2, 1, " + paramList + ")";

                                        if (dataBalansId is int)
                                            parameters.Add("balid", dataBalansId);

                                        // Register the new rent agreement in the cache
                                        RegisterRentAgreement(newArendaId, dataAgrNum, dataAgrDate, objectId, orgRenterId, orgGiverId,
                                            orgBalansId, dataStartDate, dataFinishDate, dataSquare);

                                        arendaId = newArendaId;
                                    }
                                }
                            }

                            if (query.Length > 0)
                            {
                                try
                                {
                                    using (FbCommand cmd = new FbCommand(query, connection1NF))
                                    {
                                        cmd.Parameters.Add(new FbParameter("orgiv", orgGiverId));
                                        cmd.Parameters.Add(new FbParameter("orgkod", orgRenterId));
                                        cmd.Parameters.Add(new FbParameter("balanskod", orgBalansId));
                                        cmd.Parameters.Add(new FbParameter("objkod", objectId));
                                        cmd.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                        cmd.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                        cmd.Parameters.Add(new FbParameter("aid", arendaId));

                                        foreach (KeyValuePair<string, object> pair in parameters)
                                        {
                                            cmd.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                        }

                                        cmd.Transaction = transaction;
                                        cmd.ExecuteNonQuery();

                                        // Notes and Decisions for this rent agreement will be processed later.
                                        // Save the ID mapping to a special map, for later processing
                                        agreementIdMap.Add(oldArendaId, arendaId);
                                    }
                                }
                                catch (Exception)
                                {
                                    WriteError("SQL query failed: " + query);
                                    DumpQueryParameters(parameters);

                                    WriteError("Parameter orgiv = " + orgGiverId.ToString());
                                    WriteError("Parameter orgkod = " + orgRenterId.ToString());
                                    WriteError("Parameter balanskod = " + orgBalansId.ToString());
                                    WriteError("Parameter objkod = " + objectId.ToString());
                                    WriteError("Parameter aid = " + arendaId.ToString());
                                    throw;
                                }
                            }
                        }
                    }
                }
            }

            // Export Notes and Decisions for all processed rent agreement
            foreach (KeyValuePair<int, int> pair in agreementIdMap)
            {
                ExportArendaPrimEx(reportId, pair.Key, pair.Value, transaction);
                ExportArendaRishenEx(reportId, pair.Key, pair.Value, transaction);
            }

            // Write the debug info
            foreach (KeyValuePair<int, int> pair in matchedArenda)
            {
                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE arenda_1nf SET match_id = @mid WHERE id = @eid", connectionSqlServer))
                {
                    cmdUpdate.Parameters.Add(new SqlParameter("eid", pair.Key));
                    cmdUpdate.Parameters.Add(new SqlParameter("mid", pair.Value));
                    cmdUpdate.ExecuteNonQuery();
                }
            }

            foreach (KeyValuePair<int, int> pair in createdArenda)
            {
                using (SqlCommand cmdUpdate = new SqlCommand("UPDATE arenda_1nf SET create_id = @cid WHERE id = @eid", connectionSqlServer))
                {
                    cmdUpdate.Parameters.Add(new SqlParameter("eid", pair.Key));
                    cmdUpdate.Parameters.Add(new SqlParameter("cid", pair.Value));
                    cmdUpdate.ExecuteNonQuery();
                }
            }

            // Get the list of deleted arenda entries
            query = "SELECT id_arenda FROM arenda_1nf WHERE unloading <> 0 AND deleted <> 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataArendaId = reader.IsDBNull(0) ? null : reader.GetValue(0);

                        if (dataArendaId is int)
                        {
                            // Prepare the UPDATE statement
                            query = "UPDATE ARENDA1NF SET DELETED = 1, DTDELETE = @dtdel, ISP = @isp, EIS_MODIFIED_BY = @isp, DT = @dt WHERE ID = @aid";

                            try
                            {
                                using (FbCommand commandDelete = new FbCommand(query, connection1NF))
                                {
                                    commandDelete.Parameters.Add(new FbParameter("dtdel", DateTime.Now.Date));
                                    commandDelete.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                    commandDelete.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                    commandDelete.Parameters.Add(new FbParameter("aid", dataArendaId));

                                    commandDelete.Transaction = transaction;
                                    commandDelete.ExecuteNonQuery();
                                }
                            }
                            catch (Exception)
                            {
                                WriteError("SQL query failed: " + query);
                                WriteError("Parameter aid = " + dataArendaId.ToString());
                                throw;
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        /*
        private void ExportArendaPrim(int reportId, FbTransaction transaction)
        {
            // Create mapping for the columns between our database and 1NF database
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            mapping.Add("grpurp", "grpurp");
            mapping.Add("purp", "purp");
            mapping.Add("purp_str", "purp_str");
            mapping.Add("square", "square");
            mapping.Add("descript", "descript");
            mapping.Add("ar_stavka", "ar_stavka");
            mapping.Add("narah", "narah");
            mapping.Add("price", "price");
            mapping.Add("vartist_exp", "vartist_exp");
            mapping.Add("dtexp", "dtexp");
            mapping.Add("kod_vidplata", "kod_vidplata");

            string srcFieldList = GetSrcFieldList(mapping, true);

            // Get the list of modified or added arenda notes
            string query = "SELECT id_arenda_prim, arenda, unloading, " + srcFieldList +
                " FROM arenda_prim WHERE edited <> 0 AND deleted = 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int arendaPrimId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        int arendaId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                        int unloading = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);

                        // Arenda ID can be mapped
                        arendaId = arendaIdMapping.ContainsKey(arendaId) ? arendaIdMapping[arendaId] : -1;

                        if (arendaPrimId > 0 && arendaId > 0)
                        {
                            query = "";

                            // Prepare the UPDATE statement or INSERT statement
                            if (unloading != 0)
                            {
                                // This is an edited entry
                                string destFieldList = GetUpdateFieldList(reader, mapping, parameters, null);

                                query = "UPDATE ARENDA_PRIM SET ARENDA = @arid, ISP = @isp, DT = @dt, SYS_ISP = @isp, SYS_DT = @dt, " +
                                    destFieldList + " WHERE ID = @apid";
                            }
                            else
                            {
                                // This is an added entry
                                int newArendaPrimId = GenerateNewId1NF("GEN_ARENDA_PRIM_ID", transaction);

                                if (newArendaPrimId > 0)
                                {
                                    string paramList = "";
                                    string destFieldList = GetInsertFieldList(reader, out paramList, mapping, parameters, null);

                                    query = "INSERT INTO ARENDA_PRIM (ID, ARENDA, ISP, DT, SYS_ISP, SYS_DT, DELETED, " + destFieldList +
                                        ") VALUES (@apid, @arid, @isp, @dt, @isp, @dt, 0, " + paramList + ")";

                                    arendaPrimId = newArendaPrimId;
                                }
                            }

                            if (query.Length > 0 && parameters.Count > 0)
                            {
                                try
                                {
                                    using (FbCommand commandUpdate = new FbCommand(query, connection1NF))
                                    {
                                        commandUpdate.Parameters.Add(new FbParameter("arid", arendaId));
                                        commandUpdate.Parameters.Add(new FbParameter("apid", arendaPrimId));
                                        commandUpdate.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                        commandUpdate.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));

                                        foreach (KeyValuePair<string, object> pair in parameters)
                                        {
                                            commandUpdate.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                        }

                                        commandUpdate.Transaction = transaction;
                                        commandUpdate.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception)
                                {
                                    WriteError("SQL query failed: " + query);
                                    DumpQueryParameters(parameters);
                                    WriteError("Parameter arid = " + arendaId.ToString());
                                    WriteError("Parameter apid = " + arendaPrimId.ToString());
                                    throw;
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }

            // Get the list of deleted arenda notes
            query = "SELECT id_arenda_prim FROM arenda_prim WHERE unloading <> 0 AND deleted <> 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataArendaPrimId = reader.IsDBNull(0) ? null : reader.GetValue(0);

                        if (dataArendaPrimId is int)
                        {
                            // Prepare the UPDATE statement
                            query = "UPDATE ARENDA_PRIM SET DELETED = 1, DTDELETE = @dtdel, ISP = @isp, DT = @dt, " +
                                "SYS_ISP = @isp, SYS_DT = @dt WHERE ID = @aid";

                            try
                            {
                                using (FbCommand commandDelete = new FbCommand(query, connection1NF))
                                {
                                    commandDelete.Parameters.Add(new FbParameter("dtdel", DateTime.Now.Date));
                                    commandDelete.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                    commandDelete.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                                    commandDelete.Parameters.Add(new FbParameter("aid", dataArendaPrimId));

                                    commandDelete.Transaction = transaction;
                                    commandDelete.ExecuteNonQuery();
                                }
                            }
                            catch (Exception)
                            {
                                WriteError("SQL query failed: " + query);
                                WriteError("Parameter aid = " + dataArendaPrimId.ToString());
                                throw;
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }
        */

        private void ExportArendaPrimEx(int reportId, int agreementIdSql, int agreementId1NF, FbTransaction transaction)
        {
            // Select IDs of all notes for this rent ageement (from Sql Server)
            List<int> noteIdsSqlServer = new List<int>();

            using (SqlCommand cmd = new SqlCommand("SELECT id_arenda_prim FROM arenda_prim WHERE (arenda = @aid) AND (report_id = @rid) AND (deleted = 0 OR deleted IS NULL) ORDER BY id_arenda_prim", connectionSqlServer))
            {
                cmd.Parameters.Add(new SqlParameter("aid", agreementIdSql));
                cmd.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            noteIdsSqlServer.Add(reader.GetInt32(0));
                    }

                    reader.Close();
                }
            }

            // Select IDs of all notes for this rent ageement (from 1NF)
            List<int> noteIds1NF = new List<int>();

            using (FbCommand cmd = new FbCommand("SELECT ID FROM ARENDA_PRIM WHERE (ARENDA = @aid) AND (DELETED = 0 OR DELETED IS NULL) ORDER BY ID", connection1NF))
            {
                cmd.Parameters.Add(new FbParameter("aid", agreementId1NF));
                cmd.Transaction = transaction;

                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            noteIds1NF.Add(reader.GetInt32(0));
                    }

                    reader.Close();
                }
            }

            // Delete all leftover notes in 1NF
            while (noteIds1NF.Count > noteIdsSqlServer.Count)
            {
                using (FbCommand cmd = new FbCommand("UPDATE ARENDA_PRIM SET DELETED = 1, DTDELETE = @ddt, ISP = @isp, DT = @ddt WHERE ID = @aid", connection1NF))
                {
                    cmd.Parameters.Add(new FbParameter("aid", noteIds1NF[noteIds1NF.Count - 1]));
                    cmd.Parameters.Add(new FbParameter("ddt", DateTime.Now));
                    cmd.Parameters.Add(new FbParameter("isp", "Auto-import"));

                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                }

                noteIds1NF.RemoveAt(noteIds1NF.Count - 1);
            }

            // Add some new notes to 1NF, if there is not enough
            while (noteIds1NF.Count < noteIdsSqlServer.Count)
            {
                int newArendaPrimId = GenerateNewId1NF("GEN_ARENDA_PRIM_ID", transaction);

                if (newArendaPrimId > 0)
                {
                    using (FbCommand cmd = new FbCommand("INSERT INTO ARENDA_PRIM (ID, ARENDA, DELETED, DT, ISP) VALUES (@noteid, @aid, 0, @dt, @isp)", connection1NF))
                    {
                        cmd.Parameters.Add(new FbParameter("noteid", newArendaPrimId));
                        cmd.Parameters.Add(new FbParameter("aid", agreementId1NF));
                        cmd.Parameters.Add(new FbParameter("dt", DateTime.Now));
                        cmd.Parameters.Add(new FbParameter("isp", "Auto-import"));

                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    noteIds1NF.Add(newArendaPrimId);
                }
                else
                {
                    break;
                }
            }

            if (noteIds1NF.Count == noteIdsSqlServer.Count)
            {
                Dictionary<string, string> mapping = new Dictionary<string, string>();

                mapping.Add("grpurp", "grpurp");
                mapping.Add("purp", "purp");
                mapping.Add("purp_str", "purp_str");
                mapping.Add("square", "square");
                mapping.Add("descript", "descript");
                mapping.Add("ar_stavka", "ar_stavka");
                mapping.Add("narah", "narah");
                mapping.Add("price", "price");
                mapping.Add("vartist_exp", "vartist_exp");
                mapping.Add("dtexp", "dtexp");
                mapping.Add("kod_vidplata", "kod_vidplata");

                // Perform UPDATE for each 1NF note from the corresponding Sql Server note
                for (int n = 0; n < noteIds1NF.Count; n++)
                {
                    int noteIdSql = noteIdsSqlServer[n];
                    int noteId1NF = noteIds1NF[n];

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM arenda_prim WHERE id_arenda_prim = @noteid AND report_id = @rid", connectionSqlServer))
                    {
                        cmd.Parameters.Add(new SqlParameter("noteid", noteIdSql));
                        cmd.Parameters.Add(new SqlParameter("rid", reportId));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fieldList = "";
                                FbCommand cmdUpdate1NF = new FbCommand("", connection1NF);

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string fieldNameSql = reader.GetName(i).ToLower();
                                    string fieldName1NF = "";

                                    if (fieldNameSql != "id" && mapping.TryGetValue(fieldNameSql, out fieldName1NF))
                                    {
                                        if (reader.IsDBNull(i))
                                        {
                                            fieldList += ", " + fieldName1NF + " = NULL";
                                        }
                                        else
                                        {
                                            string paramName = "param" + i.ToString();
                                            fieldList += ", " + fieldName1NF + " = @" + paramName;

                                            cmdUpdate1NF.Parameters.Add(new FbParameter(paramName, reader.GetValue(i)));
                                        }
                                    }
                                }

                                if (fieldList.Length > 0)
                                {
                                    cmdUpdate1NF.CommandText = "UPDATE ARENDA_PRIM SET ISP = @isp, DT = @dt, " + fieldList.TrimStart(' ', ',') + " WHERE ID = @noteid";

                                    cmdUpdate1NF.Parameters.Add(new FbParameter("noteid", noteId1NF));
                                    cmdUpdate1NF.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                    cmdUpdate1NF.Parameters.Add(new FbParameter("dt", DateTime.Now));

                                    cmdUpdate1NF.Transaction = transaction;
                                    cmdUpdate1NF.ExecuteNonQuery();
                                }
                            }

                            reader.Close();
                        }
                    }
                }
            }
        }

        /*
        private void ExportArendaRishen(int reportId, FbTransaction transaction)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            // Get the list of modified arenda decisions
            string query = "SELECT id_arrish, arenda, rishen, pidstava, nom, dt_doc" +
                " FROM arrish WHERE unloading <> 0 AND edited <> 0 AND deleted = 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataArrishId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataArendaId = reader.IsDBNull(1) ? null : reader.GetValue(2);
                        object dataRishenId = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object dataPidstava = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object dataDocNum = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        object dataDocDate = reader.IsDBNull(5) ? null : reader.GetValue(5);

                        if (dataArrishId is int && dataArendaId is int && arendaIdMapping.ContainsKey((int)dataArendaId))
                        {
                            // Validate the RISHEN id; sometimes 0 is entered into this field
                            if (dataRishenId is int && (int)dataRishenId == 0)
                            {
                                dataRishenId = null;
                            }

                            // Arenda ID is mapped
                            dataArendaId = arendaIdMapping[(int)dataArendaId];

                            // Prepare the UPDATE query
                            string update = "UPDATE ARRISH SET ARENDA = @arendaid, ISP = @isp, DT = @dt, SYSISP = @isp, SYSDT = @dt";

                            parameters.Clear();

                            if (dataRishenId is int)
                            {
                                update += ", RISHEN = @prishen";
                                parameters.Add("prishen", dataRishenId);
                            }

                            if (dataPidstava is string)
                            {
                                update += ", PIDSTAVA = @pids";
                                parameters.Add("pids", dataPidstava);
                            }

                            if (dataDocNum is string)
                            {
                                update += ", NOM = @docnum";
                                parameters.Add("docnum", dataDocNum);
                            }

                            if (dataDocDate is DateTime)
                            {
                                update += ", DT_DOC = @docdate";
                                parameters.Add("docdate", dataDocDate);
                            }

                            update += " WHERE ID = @arrishid";

                            if (parameters.Count > 0)
                            {
                                try
                                {
                                    using (FbCommand commandUpdate = new FbCommand(update, connection1NF))
                                    {
                                        commandUpdate.Parameters.Add(new FbParameter("arendaid", dataArendaId));
                                        commandUpdate.Parameters.Add(new FbParameter("arrishid", dataArrishId));
                                        commandUpdate.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                        commandUpdate.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));

                                        foreach (KeyValuePair<string, object> pair in parameters)
                                        {
                                            commandUpdate.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                        }

                                        commandUpdate.Transaction = transaction;
                                        commandUpdate.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception)
                                {
                                    WriteError("SQL query failed: " + update);
                                    DumpQueryParameters(parameters);
                                    WriteError("Parameter arendaid = " + dataArendaId.ToString());
                                    WriteError("Parameter arrishid = " + dataArrishId.ToString());
                                    throw;
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }

            // Get the list of added arenda decisions
            query = "SELECT id_arrish, arenda, rishen, pidstava, nom, dt_doc" +
                " FROM arrish WHERE unloading = 0 AND edited <> 0 AND deleted = 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataArrishId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataArendaId = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object dataRishenId = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object dataPidstava = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object dataDocNum = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        object dataDocDate = reader.IsDBNull(5) ? null : reader.GetValue(5);

                        if (dataArrishId is int && dataArendaId is int && arendaIdMapping.ContainsKey((int)dataArendaId))
                        {
                            // Arenda ID is mapped
                            dataArendaId = arendaIdMapping[(int)dataArendaId];

                            // Validate the RISHEN id; sometimes 0 is entered into this field
                            if (dataRishenId is int && (int)dataRishenId == 0)
                            {
                                dataRishenId = null;
                            }

                            // Prepare the INSERT statement
                            string fieldList = "ID, ARENDA, ISP, DT, SYSISP, SYSDT";
                            string paramList = "@arrishid, @arendaid, @isp, @dt, @isp, @dt";

                            parameters.Clear();

                            if (dataRishenId is int)
                            {
                                fieldList += ", RISHEN";
                                paramList += ", @prishen";
                                parameters.Add("prishen", dataRishenId);
                            }

                            if (dataPidstava is string)
                            {
                                fieldList += ", PIDSTAVA";
                                paramList += ", @pids";
                                parameters.Add("pids", dataPidstava);
                            }

                            if (dataDocNum is string)
                            {
                                fieldList += ", NOM";
                                paramList += ", @docnum";
                                parameters.Add("docnum", dataDocNum);
                            }

                            if (dataDocDate is DateTime)
                            {
                                fieldList += ", DT_DOC";
                                paramList += ", @docdate";
                                parameters.Add("docdate", dataDocDate);
                            }

                            if (parameters.Count > 0)
                            {
                                string insert = "INSERT INTO ARRISH (" + fieldList + ") VALUES (" + paramList + ")";

                                int newArrishd = GenerateNewId1NF("GEN_ARRISH_ID", transaction);

                                if (newArrishd > 0)
                                {
                                    try
                                    {
                                        using (FbCommand commandUpdate = new FbCommand(insert, connection1NF))
                                        {
                                            commandUpdate.Parameters.Add(new FbParameter("arendaid", dataArendaId));
                                            commandUpdate.Parameters.Add(new FbParameter("arrishid", newArrishd));
                                            commandUpdate.Parameters.Add(new FbParameter("isp", "Auto-import"));
                                            commandUpdate.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));

                                            foreach (KeyValuePair<string, object> pair in parameters)
                                            {
                                                commandUpdate.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                            }

                                            commandUpdate.Transaction = transaction;
                                            commandUpdate.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        WriteError("SQL query failed: " + insert);
                                        DumpQueryParameters(parameters);
                                        WriteError("Parameter arendaid = " + dataArendaId.ToString());
                                        WriteError("Parameter arrishid = " + newArrishd.ToString());
                                        throw;
                                    }
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }
        */

        private void ExportArendaRishenEx(int reportId, int agreementIdSql, int agreementId1NF, FbTransaction transaction)
        {
            // Get information about existing decisions from 1NF
            Dictionary<int, RentDecisionInfo> existingDecisions1NF = new Dictionary<int, RentDecisionInfo>();

            using (FbCommand cmd = new FbCommand("SELECT ID, NOM, DT_DOC FROM ARRISH WHERE ARENDA = @aid", connection1NF))
            {
                cmd.Parameters.Add(new FbParameter("aid", agreementId1NF));
                cmd.Transaction = transaction;

                using (FbDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        RentDecisionInfo info = new RentDecisionInfo();

                        info.id = r.GetInt32(0);
                        info.docNum = r.IsDBNull(1) ? "" : r.GetString(1).Trim().ToUpper();

                        if (!r.IsDBNull(2))
                            info.docDate = r.GetDateTime(2).Date;

                        existingDecisions1NF[info.id] = info;
                    }

                    r.Close();
                }
            }

            // Get all the current decisions (defined in the user report)
            Dictionary<int, RentDecisionInfo> newDecisions = new Dictionary<int, RentDecisionInfo>();

            using (SqlCommand cmd = new SqlCommand("SELECT id_arrish, nom, dt_doc, pidstava FROM arrish WHERE arenda = @aid AND report_id = @rid AND (deleted = 0 OR deleted IS NULL)", connectionSqlServer))
            {
                cmd.Parameters.Add(new SqlParameter("aid", agreementIdSql));
                cmd.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        RentDecisionInfo info = new RentDecisionInfo();

                        info.id = r.GetInt32(0);
                        info.docNum = r.IsDBNull(1) ? "" : r.GetString(1).Trim().ToUpper();

                        if (!r.IsDBNull(2))
                            info.docDate = r.GetDateTime(2).Date;

                        info.pidstavaStr = r.IsDBNull(3) ? "" : r.GetString(3).Trim();

                        newDecisions[info.id] = info;
                    }

                    r.Close();
                }
            }

            // Execute INSERT or UPDATE for all new decisions in the Sql Server and in 1NF
            foreach (KeyValuePair<int, RentDecisionInfo> newDecision in newDecisions)
            {
                RentDecisionInfo existingDecision = null;

                foreach (KeyValuePair<int, RentDecisionInfo> oldDecision in existingDecisions1NF)
                {
                    if (oldDecision.Value.docNum.Length > 0 && oldDecision.Value.docDate.HasValue)
                    {
                        if (newDecision.Value.docNum == oldDecision.Value.docNum &&
                            newDecision.Value.docDate == oldDecision.Value.docDate)
                        {
                            existingDecision = oldDecision.Value;
                            break;
                        }
                    }
                }

                if (existingDecision != null)
                {
                    FbCommand cmd = newDecision.Value.CreateUpdateCmd1NF(connection1NF, existingDecision.id);

                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();

                    // Mark the existing decision as processed
                    existingDecision.isUpdated = true;
                }
                else
                {
                    // Generate ID for a new ARRISH entry
                    int linkId = GenerateNewId1NF("GEN_ARRISH_ID", transaction);

                    if (linkId > 0)
                    {
                        using (FbCommand cmd = newDecision.Value.CreateInsertCmd1NF(connection1NF, agreementId1NF, linkId))
                        {
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            // Delete all the old decisions that could not be updated
            foreach (KeyValuePair<int, RentDecisionInfo> pair in existingDecisions1NF)
            {
                if (!pair.Value.isUpdated)
                {
                    using (FbCommand cmd = new FbCommand("DELETE FROM ARRISH WHERE ID = @did", connection1NF))
                    {
                        cmd.Parameters.Add(new FbParameter("did", pair.Key));
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private string GetSrcFieldList(Dictionary<string, string> mapping, bool forSqlServer)
        {
            string srcFieldList = "";

            foreach (KeyValuePair<string, string> pair in mapping)
            {
                if (srcFieldList.Length > 0)
                {
                    srcFieldList += ", ";
                }

                if (forSqlServer)
                {
                    srcFieldList += "[" + pair.Key + "]";
                }
                else
                {
                    srcFieldList += pair.Key;
                }
            }

            return srcFieldList;
        }

        private int GenerateNewId1NF(string generatorName, FbTransaction transaction)
        {
            int objectId = -1;

            string query = "SELECT GEN_ID(" + generatorName + ", 1) FROM RDB$DATABASE";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                command.Transaction = transaction;

                using (FbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            objectId = reader.GetInt32(0);
                        }
                    }

                    reader.Close();
                }
            }

            return objectId;
        }

        private int GenerateOrganizationId1NF(FbTransaction transaction)
        {
            if (organizationIdSeed1NF > 0)
            {
                organizationIdSeed1NF++;

                return organizationIdSeed1NF;
            }
            else
            {
                return GenerateNewId1NF("SORG_1NF_GEN", transaction);
            }
        }

        private int FindOrCreateDocument1NF(SqlDataReader reader, int columnIndexDocKind,
            int columnIndexDocNum, int columnIndexDocDate, FbTransaction transaction)
        {
            int docId = -1;

            object dataDocKind = reader.IsDBNull(columnIndexDocKind) ? null : reader.GetValue(columnIndexDocKind);
            object dataDocNum = reader.IsDBNull(columnIndexDocNum) ? null : reader.GetValue(columnIndexDocNum);
            object dataDocDate = reader.IsDBNull(columnIndexDocDate) ? null : reader.GetValue(columnIndexDocDate);

            if (dataDocKind is int && dataDocNum is string && dataDocDate is DateTime)
            {
                // First, try to find the document in 1NF, if it exists
                string tag = GetDocumentKey((int)dataDocKind, (DateTime)dataDocDate, (string)dataDocNum);

                if (docIdCache1NF.TryGetValue(tag, out docId))
                {
                    // Found an existing document; nothing to do
                }
                else
                {
                    // Generate a new ID for the document
                    docId = GenerateNewId1NF("GEN_ROZP_DOK_ID", transaction);

                    // Make sure that document has correct type
                    if (!validDocTypes1NF.Contains((int)dataDocKind))
                    {
                        dataDocKind = 113; // "Other"
                    }

                    // Execute INSERT into the table of documents
                    if (docId > 0)
                    {
                        string query = "insert into ROZP_DOK (DOK_ID, DOKKIND, DOKDATA, DOKNUM, DT, ISP) values" +
                            " (@did, @kind, @dtdoc, @num, @dt, @isp)";

                        using (FbCommand command = new FbCommand(query, connection1NF))
                        {
                            command.Transaction = transaction;

                            command.Parameters.Add(new FbParameter("did", docId));
                            command.Parameters.Add(new FbParameter("kind", dataDocKind));
                            command.Parameters.Add(new FbParameter("dtdoc", dataDocDate));
                            command.Parameters.Add(new FbParameter("num", ((string)dataDocNum).Trim().ToUpper()));
                            command.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                            command.Parameters.Add(new FbParameter("isp", "Auto-import"));

                            command.ExecuteNonQuery();
                        }

                        // Save this document ID for future lookup
                        docIdCache1NF[tag] = docId;
                    }
                }
            }

            return docId;
        }

        private int AddDocRelationToObject(int rowId, int balansId, int organizationId, int objectId, int docId,
            FbTransaction transaction)
        {
            int relationId = -1;

            // If relation already exists, we are not going to change anything
            string query = "select first 1 ID from OBJECT_DOKS_PROPERTIES where OBJECT_KOD = @oid and DOK_ID = @did";

            using (FbCommand cmd = new FbCommand(query, connection1NF))
            {
                cmd.Transaction = transaction;

                cmd.Parameters.Add(new FbParameter("oid", objectId));
                cmd.Parameters.Add(new FbParameter("did", docId));

                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read() && !reader.IsDBNull(0))
                    {
                        relationId = reader.GetInt32(0);
                    }
                }
            }

            if (relationId < 0)
            {
                // Generate a new ID for relation
                relationId = GenerateNewId1NF("OBJECT_DOKS_GEN", transaction);

                if (relationId > 0)
                {
                    // Insert a new record into OBJECT_DOKS_PROPERTIES table
                    query = "insert into OBJECT_DOKS_PROPERTIES (ID, OBJECT_KOD, OBJECT_KODSTAN, DOK_ID, ORG, DT, ISP, BAL_FLAG)" +
                        " values (@odpid, @objid, 1, @did, @orgid, @dt, @isp, 1)";

                    using (FbCommand cmd = new FbCommand(query, connection1NF))
                    {
                        cmd.Transaction = transaction;

                        cmd.Parameters.Add(new FbParameter("odpid", relationId));
                        cmd.Parameters.Add(new FbParameter("objid", objectId));
                        cmd.Parameters.Add(new FbParameter("did", docId));
                        cmd.Parameters.Add(new FbParameter("orgid", organizationId));
                        cmd.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                        cmd.Parameters.Add(new FbParameter("isp", "Auto-import"));

                        cmd.ExecuteNonQuery();
                    }

                    // Get all the data that we can add to the relation from the 'balans_1nf' table
                    Dictionary<string, string> mapping = new Dictionary<string, string>();
                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    mapping.Add("form_vlasn", "FORM_VLASN");
                    mapping.Add("kindobj", "OBJKIND");
                    mapping.Add("typeobj", "OBJTYPE");
                    mapping.Add("grpurp", "GRPURP");
                    mapping.Add("purpose", "PURPOSE");
                    mapping.Add("texstan_bal", "TEXSTAN");
                    mapping.Add("sqr_zag", "SQUARE");
                    mapping.Add("sqr_nej", "NLSQARE");
                    mapping.Add("sqr_free", "CLR_SQUARE");
                    mapping.Add("balans_vartist", "SUMMA_BALANS");
                    mapping.Add("zal_vart", "SUMMA_ZAL");
                    mapping.Add("znos", "SUMMA_ZNOS");
                    mapping.Add("vartist_exp_v", "EXP_VART");
                    mapping.Add("dtexp", "EXP_DATE");

                    query = "select " + GetSrcFieldList(mapping, true) + " from balans_1nf where id = @rid";

                    using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
                    {
                        command.Parameters.Add(new SqlParameter("rid", rowId));

                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                // Generate the UPDATE statement
                                string destFieldList = GetUpdateFieldList(r, mapping, parameters, null);

                                if (parameters.Count > 0)
                                {
                                    query = "UPDATE OBJECT_DOKS_PROPERTIES SET " + destFieldList + " WHERE ID = @odpid";

                                    using (FbCommand commandUpdate = new FbCommand(query, connection1NF))
                                    {
                                        commandUpdate.Transaction = transaction;
                                        commandUpdate.Parameters.Add(new FbParameter("odpid", relationId));

                                        foreach (KeyValuePair<string, object> pair in parameters)
                                        {
                                            commandUpdate.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                        }

                                        commandUpdate.ExecuteNonQuery();
                                    }
                                }
                            }

                            r.Close();
                        }
                    }

                    // Get all the data that we can add to the relation from the 'objects_1nf' table
                    mapping.Clear();

                    mapping.Add("ulname", "OBJ_ULNAME");
                    mapping.Add("ulname2", "OBJ_ULNAME2");
                    mapping.Add("nomer1", "OBJ_NOMER1");
                    mapping.Add("nomer2", "OBJ_NOMER2");
                    mapping.Add("nomer3", "OBJ_NOMER3");
                    mapping.Add("adrdop", "ADRDOP");
                    mapping.Add("budyear", "YEAR_BUILD");

                    query = "select " + GetSrcFieldList(mapping, false) + " from OBJECT_1NF where OBJECT_KOD = @oid";

                    using (FbCommand command = new FbCommand(query, connection1NF))
                    {
                        command.Transaction = transaction;
                        command.Parameters.Add(new FbParameter("oid", objectId));

                        using (FbDataReader r = command.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                // Generate the UPDATE statement
                                string destFieldList = GetUpdateFieldList(r, mapping, parameters, null);

                                if (parameters.Count > 0)
                                {
                                    query = "UPDATE OBJECT_DOKS_PROPERTIES SET " + destFieldList + " WHERE ID = @odpid";

                                    using (FbCommand commandUpdate = new FbCommand(query, connection1NF))
                                    {
                                        commandUpdate.Transaction = transaction;
                                        commandUpdate.Parameters.Add(new FbParameter("odpid", relationId));

                                        foreach (KeyValuePair<string, object> pair in parameters)
                                        {
                                            commandUpdate.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                        }

                                        commandUpdate.ExecuteNonQuery();
                                    }
                                }
                            }

                            r.Close();
                        }
                    }    
                }
            }

            return relationId;
        }

        private int AddDocRelationToBalans(int balansId, int objectId, int objectDocsPropertiesId, FbTransaction transaction)
        {
            int relationId = -1;

            // If relation already exists, we are not going to add it
            string query = "select first 1 ID from BALANS_DOK_PROP where BALID = @bid AND OBJECT_KOD = @oid and DOK_ID = @did";

            using (FbCommand cmd = new FbCommand(query, connection1NF))
            {
                cmd.Transaction = transaction;

                cmd.Parameters.Add(new FbParameter("bid", balansId));
                cmd.Parameters.Add(new FbParameter("oid", objectId));
                cmd.Parameters.Add(new FbParameter("did", objectDocsPropertiesId));

                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read() && !reader.IsDBNull(0))
                    {
                        relationId = reader.GetInt32(0);
                    }
                }
            }

            if (relationId < 0)
            {
                // Generate a new ID for relation
                relationId = GenerateNewId1NF("GEN_BALANS_DOK_PROP", transaction);

                if (relationId > 0)
                {
                    // Insert a new record into OBJECT_DOKS_PROPERTIES table
                    query = "insert into BALANS_DOK_PROP (ID, BALID, OBJECT_KOD, FLAG, DOK_ID, SORT_FLD)" +
                        " values (@rid, @bid, @objid, 1, @did, 0)";

                    using (FbCommand cmd = new FbCommand(query, connection1NF))
                    {
                        cmd.Transaction = transaction;

                        cmd.Parameters.Add(new FbParameter("rid", relationId));
                        cmd.Parameters.Add(new FbParameter("bid", balansId));
                        cmd.Parameters.Add(new FbParameter("objid", objectId));
                        cmd.Parameters.Add(new FbParameter("did", objectDocsPropertiesId));

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            return relationId;
        }

        private string GetUpdateFieldList(System.Data.IDataReader reader,
            Dictionary<string, string> mapping,
            Dictionary<string, object> parameters,
            Dictionary<string, object> overrides)
        {
            parameters.Clear();

            string destFieldList = "";

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.IsDBNull(i))
                {
                    string srcFieldName = reader.GetName(i).ToLower();

                    if (mapping.ContainsKey(srcFieldName))
                    {
                        string destFieldName = mapping[srcFieldName];
                        string paramName = "p" + i.ToString();

                        if (destFieldList.Length > 0)
                        {
                            destFieldList += ", ";
                        }

                        destFieldList += destFieldName + " = @" + paramName;

                        // Perform value pre-processing
                        object val = reader.GetValue(i);

                        if (overrides != null && !overrides.TryGetValue(srcFieldName, out val))
                        {
                            val = reader.GetValue(i);
                        }

                        if (val is string)
                        {
                            val = ((string)val).Trim();
                        }

                        parameters[paramName] = val;
                    }
                }
            }

            return destFieldList;
        }

        private string GetInsertFieldList(System.Data.IDataReader reader, out string paramList,
            Dictionary<string, string> mapping,
            Dictionary<string, object> parameters,
            Dictionary<string, object> overrides)
        {
            paramList = "";

            parameters.Clear();

            string destFieldList = "";

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.IsDBNull(i))
                {
                    string srcFieldName = reader.GetName(i).ToLower();

                    if (mapping.ContainsKey(srcFieldName))
                    {
                        string destFieldName = mapping[srcFieldName];
                        string paramName = "p" + i.ToString();

                        if (destFieldList.Length > 0)
                        {
                            destFieldList += ", ";
                            paramList += ", ";
                        }

                        destFieldList += destFieldName;
                        paramList += "@" + paramName;

                        // Perform value pre-processing
                        object val = reader.GetValue(i);

                        if (overrides != null && !overrides.TryGetValue(srcFieldName, out val))
                        {
                            val = reader.GetValue(i);
                        }

                        if (val is string)
                        {
                            val = ((string)val).Trim();
                        }

                        parameters[paramName] = val;
                    }
                }
            }

            return destFieldList;
        }

        private void GetReaderRowData(System.Data.IDataReader reader, Dictionary<string, object> data, Dictionary<string, string> mapping)
        {
            data.Clear();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.IsDBNull(i))
                {
                    string srcFieldName = reader.GetName(i).ToLower().Trim();

                    if (srcFieldName.Length > 0 && mapping.ContainsKey(srcFieldName))
                    {
                        // Perform value pre-processing
                        object val = reader.GetValue(i);

                        if (val is string)
                        {
                            string str = ((string)val).Trim();

                            val = (str.Length > 0) ? str : null;
                        }

                        if (val != null)
                        {
                            data[srcFieldName] = val;
                        }
                    }
                }
            }
        }

        private object Short2Int(object number)
        {
            if (number is short)
            {
                int i = (short)number;

                return i;
            }

            return number;
        }

        private object ValidateDate(object val)
        {
            if (val is DateTime)
            {
                DateTime dt = (DateTime)val;

                if (dt.Year < 1760)
                {
                    // This is possible an entering error
                    string str = dt.Year.ToString();

                    if (str.Length == 1)
                    {
                        str = "200" + str;
                    }
                    else if (str.Length == 2)
                    {
                        str = "20" + str;
                    }
                    else if (str.Length == 3)
                    {
                        if (str.StartsWith("2"))
                        {
                            str = str.Insert(1, "0");
                        }
                        else if (str.StartsWith("1"))
                        {
                            str = str.Insert(1, "9");
                        }
                        else
                        {
                            str = "2" + str;
                        }
                    }

                    int yearCorrected = int.Parse(str);

                    return new DateTime(yearCorrected, dt.Month, dt.Day);
                }
            }

            return val;
        }

        private static string GetCurDateForFirebird()
        {
            DateTime dt = DateTime.Now;

            string day = dt.Day.ToString();
            string month = dt.Month.ToString();
            string year = dt.Year.ToString();

            while (day.Length < 2)
            {
                day = "0" + day;
            }

            while (month.Length < 2)
            {
                month = "0" + month;
            }

            return "\"" + month + "-" + day + "-" + year + "\"";
        }

        private int GetSrcFieldIndexFromMapping(Dictionary<string, string> fields, string srcFieldName)
        {
            int fieldIndex = 0;

            foreach (KeyValuePair<string, string> entry in fields)
            {
                if (entry.Key.ToUpper() == srcFieldName.ToUpper())
                {
                    return fieldIndex;
                }

                fieldIndex++;
            }

            return -1;
        }

        #endregion (Export to 1NF)

        #region Caching of some 1NF dictionaries

        private void Prepare1NFDocumentCache()
        {
            docIdCache1NF.Clear();

            string query = "SELECT DOK_ID, DOKKIND, DOKDATA, DOKNUM FROM ROZP_DOK";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                using (FbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataDocId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataDocKind = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object dataDocDate = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object dataDocNum = reader.IsDBNull(3) ? null : reader.GetValue(3);

                        if (dataDocId is int && dataDocKind is int && dataDocDate is DateTime && dataDocNum is string)
                        {
                            string tag = GetDocumentKey((int)dataDocKind, (DateTime)dataDocDate, (string)dataDocNum);

                            docIdCache1NF[tag] = (int)dataDocId;
                        }
                    }

                    reader.Close();
                }
            }
        }

        private string GetDocumentKey(int docKind, DateTime docDate, string docNum)
        {
            return docKind.ToString() + "_" + docDate.ToShortDateString() + "_" + docNum.Trim().ToUpper();
        }

        private string GetArrishKey(DateTime docDate, string docNum)
        {
            return docDate.ToShortDateString() + "_" + docNum.Trim().ToUpper();
        }

        private void PrepareDocTypeList()
        {
            validDocTypes1NF.Clear();

            string query = "SELECT ID FROM SKINDDOK";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                using (FbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataDocKindId = reader.IsDBNull(0) ? null : reader.GetValue(0);

                        if (dataDocKindId is int)
                        {
                            validDocTypes1NF.Add((int)dataDocKindId);
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void PrepareListOfEmptyPurposeGroups()
        {
            using (FbCommand cmd = new FbCommand("SELECT gr.KOD FROM SGRPURPOSE gr WHERE NOT gr.KOD IN (select distinct gr FROM SPURPOSE)", connection1NF))
            {
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        emptyPurposeGroups.Add(reader.GetInt32(0));
                    }

                    reader.Close();
                }
            }
        }

        #endregion (Caching of some 1NF dictionaries)

        #region Working with dictionaries

        protected void OverrideDictionary(string tableName, string primaryKeyField)
        {
            using (OleDbCommand cmd = new OleDbCommand("select * from " + tableName, connectionParadox))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    int keyFieldIndex = -1;

                    while (reader.Read())
                    {
                        // Determine the index of the primary key field
                        if (keyFieldIndex < 0)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (reader.GetName(i).ToUpper() == primaryKeyField.ToUpper())
                                {
                                    keyFieldIndex = i;
                                    break;
                                }
                            }
                        }

                        if (keyFieldIndex >= 0)
                        {
                            object dataKey = reader.GetValue(keyFieldIndex);

                            if (dataKey is int)
                            {
                                if (DictionaryEntryExistsInSqlServer(tableName, primaryKeyField, (int)dataKey))
                                {
                                    UpdateDictionaryEntryInSqlServer(tableName, reader, primaryKeyField, (int)dataKey);
                                }
                                else
                                {
                                    InsertDictionaryEntryInSqlServer(tableName, reader);
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        protected bool DictionaryEntryExistsInSqlServer(string tableName, string primaryKeyField, int key)
        {
            bool result = false;

            string query = "select top 1 * from " + tableName + " where " + primaryKeyField + " = @k_id";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                cmd.Parameters.Add(new SqlParameter("k_id", key));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = true;
                    }

                    reader.Close();
                }
            }

            return result;
        }

        protected void UpdateDictionaryEntryInSqlServer(string tableName, OleDbDataReader reader,
            string primaryKeyField, int key)
        {
            string values = "";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).ToUpper() != primaryKeyField.ToUpper())
                {
                    if (values.Length > 0)
                    {
                        values += ", ";
                    }

                    values += reader.GetName(i);
                    values += " = @p" + i.ToString();

                    parameters["@p" + i.ToString()] = reader.GetValue(i);
                }
            }

            string query = "update " + tableName + " set " + values + " where " + primaryKeyField + " = @k_id";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                cmd.Parameters.Add(new SqlParameter("k_id", key));

                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(pair.Key, convertParamToUnicode(pair.Value)));
                }

                cmd.ExecuteNonQuery();
            }
        }

        protected void InsertDictionaryEntryInSqlServer(string tableName, OleDbDataReader reader)
        {
            string fields = "";
            string values = "";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (fields.Length > 0)
                {
                    fields += ", ";

                }
                if (values.Length > 0)
                {
                    values += ", ";
                }

                fields += reader.GetName(i);
                values += "@p" + i.ToString();

                parameters["@p" + i.ToString()] = reader.GetValue(i);
            }

            string query = "insert into " + tableName + " ( " + fields + " ) values ( " + values + " )";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(pair.Key, convertParamToUnicode(pair.Value)));
                }

                cmd.ExecuteNonQuery();
            }
        }

        #endregion (Working with dictionaries)

        #region Rent agreement identification

        private void PrepareRentAgreementCache()
        {
            rentAgreementCache.Clear();

            string query = "SELECT ID, OBJECT_KOD, ORG_KOD, ORGIV, BALANS_KOD, DOGDATE, DOGNUM, START_ORENDA, FINISH_ORENDA, SQUARE, DELETED FROM ARENDA1NF";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                using (FbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataBuildingId = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object dataRenterId = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object dataGiverId = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object dataBalansId = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        object dataDate = reader.IsDBNull(5) ? null : reader.GetValue(5);
                        object dataNum = reader.IsDBNull(6) ? null : reader.GetValue(6);
                        object dataRentStart = reader.IsDBNull(7) ? null : reader.GetValue(7);
                        object dataRentFinish = reader.IsDBNull(8) ? null : reader.GetValue(8);
                        object dataSquare = reader.IsDBNull(9) ? null : reader.GetValue(9);
                        object dataDeleted = reader.IsDBNull(10) ? null : reader.GetValue(10);

                        if (dataId is int && dataBuildingId is int)
                        {
                            ArendaAgreementInfo info = new ArendaAgreementInfo();

                            info.arendaId = (int)dataId;
                            info.buildingId = dataBuildingId;
                            info.orgGiverId = dataGiverId;
                            info.orgRenterId = dataRenterId;
                            info.orgBalansId = dataRenterId;
                            info.agreementNum = dataNum;
                            info.agreementDate = dataDate;
                            info.rentStartDate = dataRentStart;
                            info.rentFinishDate = dataRentFinish;
                            info.rentSquare = dataSquare;
                            info.deleted = (dataDeleted is int) ? ((int)dataDeleted == 1) : false;

                            info.ValidateProperties();

                            List<ArendaAgreementInfo> list = null;

                            if (rentAgreementCache.TryGetValue((int)dataBuildingId, out list))
                            {
                                list.Add(info);
                            }
                            else
                            {
                                list = new List<ArendaAgreementInfo>();

                                list.Add(info);

                                rentAgreementCache.Add((int)dataBuildingId, list);
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        private ArendaAgreementInfo GetExistingRentAgreement(object agreementNum, object agreementDate,
            object buildingId, object orgRenterId, object orgGiverId, object orgBalansId,
            object rentStartDate, object rentFinishDate, object rentSquare)
        {
            ArendaAgreementInfo infoValidArenda = null;
            ArendaAgreementInfo infoDeletedArenda = null;

            if (buildingId is int)
            {
                ArendaAgreementInfo info = new ArendaAgreementInfo();

                info.buildingId = buildingId;
                info.orgGiverId = orgGiverId;
                info.orgRenterId = orgRenterId;
                info.orgBalansId = orgBalansId;
                info.agreementNum = agreementNum;
                info.agreementDate = agreementDate;
                info.rentStartDate = rentStartDate;
                info.rentFinishDate = rentFinishDate;
                info.rentSquare = rentSquare;

                info.ValidateProperties();

                // Get the cache of existing rent agreements for this building
                List<ArendaAgreementInfo> list = null;

                if (rentAgreementCache.TryGetValue((int)buildingId, out list))
                {
                    foreach (ArendaAgreementInfo infoExisting in list)
                    {
                        if (infoExisting.IsMatch(info))
                        {
                            if (infoExisting.deleted)
                            {
                                infoDeletedArenda = infoExisting;
                            }
                            else
                            {
                                infoValidArenda = infoExisting;
                            }
                        }
                    }
                }
            }

            return (infoValidArenda != null) ? infoValidArenda : infoDeletedArenda;
        }

        private void RegisterRentAgreement(int arendaId, object agrNum, object agrDate,
            object buildingId, object orgRenterId, object orgGiverId, object orgBalansId,
            object rentStartDate, object rentFinishDate, object rentSquare)
        {
            if (buildingId is int)
            {
                ArendaAgreementInfo info = new ArendaAgreementInfo();

                info.arendaId = arendaId;
                info.buildingId = buildingId;
                info.orgGiverId = orgGiverId;
                info.orgRenterId = orgRenterId;
                info.orgBalansId = orgBalansId;
                info.agreementNum = agrNum;
                info.agreementDate = agrDate;
                info.rentStartDate = rentStartDate;
                info.rentFinishDate = rentFinishDate;
                info.rentSquare = rentSquare;

                info.ValidateProperties();

                List<ArendaAgreementInfo> list = null;

                if (rentAgreementCache.TryGetValue((int)buildingId, out list))
                {
                    list.Add(info);
                }
                else
                {
                    list = new List<ArendaAgreementInfo>();

                    list.Add(info);

                    rentAgreementCache.Add((int)buildingId, list);
                }
            }
        }

        #endregion (Rent agreement identification)

        #region Balans entry identification

        private void PrepareBalansCache()
        {
            balansCacheByObject.Clear();

            // Get all records from 'BALANS_1NF' table
            string query = "SELECT ID, OBJECT, SQR_ZAG, GRPURP, PURPOSE, PURP_STR, FLOATS, ORG, DELETED, SQR_NEJ FROM balans_1nf ORDER BY ID";

            using (FbCommand cmdSelect = new FbCommand(query, connection1NF))
            {
                using (FbDataReader reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object balansId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object objectId = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object sqrTotal = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object purposeGroup = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object purposeCode = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        object purposeStr = reader.IsDBNull(5) ? null : reader.GetValue(5);
                        object floors = reader.IsDBNull(6) ? null : reader.GetValue(6);
                        object org = reader.IsDBNull(7) ? null : reader.GetValue(7);
                        object deleted = reader.IsDBNull(8) ? null : reader.GetValue(8);
                        object sqrNonHabit = reader.IsDBNull(9) ? null : reader.GetValue(9);

                        if (balansId is int && objectId is int)
                        {
                            BalansObjectInfo info = new BalansObjectInfo();

                            info.balansId = (int)balansId;
                            info.buildingId = objectId;
                            info.orgId = org;
                            info.purposeGroupId = purposeGroup;
                            info.purposeId = purposeCode;
                            info.purposeStr = purposeStr;
                            info.floors = floors;
                            info.totalSquare = sqrTotal;
                            // info.nonHabitSquare = sqrNonHabit;
                            info.deleted = (deleted is int) ? ((int)deleted == 1) : false;

                            info.ValidateProperties();

                            // Fill the simple cache by balans ID
                            balansCacheByID.Add((int)balansId, info);

                            // Fill the lookup cache by object ID
                            List<BalansObjectInfo> list = null;

                            if (balansCacheByObject.TryGetValue((int)objectId, out list))
                            {
                                list.Add(info);
                            }
                            else
                            {
                                list = new List<BalansObjectInfo>();

                                list.Add(info);

                                balansCacheByObject.Add((int)objectId, list);
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        private BalansObjectInfo GetExistingBalansObject(object orgId, object buildingId,
            object purposeGroup, object purposeCode, object purposeStr, object sqrTotal/*, object sqrNonHabit*/, object floors)
        {
            BalansObjectInfo infoValidBalans = null;
            BalansObjectInfo infoDeletedBalans = null;

            if (buildingId is int)
            {
                BalansObjectInfo info = new BalansObjectInfo();

                info.buildingId = buildingId;
                info.orgId = orgId;
                info.purposeGroupId = purposeGroup;
                info.purposeId = purposeCode;
                info.purposeStr = purposeStr;
                info.floors = floors;
                info.totalSquare = sqrTotal;
                // info.nonHabitSquare = sqrNonHabit;

                info.ValidateProperties();

                // Get the cache of existing balans objects for this building
                List<BalansObjectInfo> list = null;

                if (balansCacheByObject.TryGetValue((int)buildingId, out list))
                {
                    foreach (BalansObjectInfo infoExisting in list)
                    {
                        if (infoExisting.IsMatch(info))
                        {
                            if (infoExisting.deleted)
                            {
                                infoDeletedBalans = infoExisting;
                            }
                            else
                            {
                                infoValidBalans = infoExisting;
                            }
                        }
                    }
                }
            }

            return (infoValidBalans != null) ? infoValidBalans : infoDeletedBalans;
        }

        private void RegisterBalansObject(int balansId, object orgId, object buildingId,
            object purposeGroup, object purposeCode, object purposeStr, object sqrTotal/*, object sqrNonHabit*/, object floors)
        {
            if (buildingId is int)
            {
                BalansObjectInfo info = new BalansObjectInfo();

                info.balansId = (int)balansId;
                info.buildingId = buildingId;
                info.orgId = orgId;
                info.purposeGroupId = purposeGroup;
                info.purposeId = purposeCode;
                info.purposeStr = purposeStr;
                info.floors = floors;
                info.totalSquare = sqrTotal;
                // info.nonHabitSquare = sqrNonHabit;

                info.ValidateProperties();

                // Fill the simple cache by balans ID
                balansCacheByID.Add((int)balansId, info);

                // Fill the lookup cache by object ID
                List<BalansObjectInfo> list = null;

                if (balansCacheByObject.TryGetValue((int)buildingId, out list))
                {
                    list.Add(info);
                }
                else
                {
                    list = new List<BalansObjectInfo>();

                    list.Add(info);

                    balansCacheByObject.Add((int)buildingId, list);
                }
            }
        }

        private void UnregisterBalansObject(int balansId, int buildingId)
        {
            // Get the cache of existing balans objects for this building
            List<BalansObjectInfo> list = null;

            if (balansCacheByObject.TryGetValue((int)buildingId, out list))
            {
                BalansObjectInfo toDelete = null;

                foreach (BalansObjectInfo infoExisting in list)
                {
                    if (infoExisting.balansId == balansId)
                    {
                        toDelete = infoExisting;
                        break;
                    }
                }

                if (toDelete != null)
                {
                    list.Remove(toDelete);
                }
            }
        }

        #endregion (Balans entry identification)

        #region Report validation

        private int ERR_ObjIncomplete = 101;
        private int ERR_ObjUnknownStreet = 102;
        private int ERR_ObjDifferentSquare = 103;

        private int ERR_OrgNoPrimaryInfo = 201;
        private int ERR_OrgNoAdditionalInfo = 202;
        private int ERR_OrgDuplicateZKPO = 203;
        private int ERR_OrgDataMismatch = 204;

        private int ERR_BalansIncomplete = 301;
        private int ERR_BalansDifferentTotalSquare = 302;
        // private int ERR_BalansDifferentNonHabitSquare = 303;
        private int ERR_ArendaIncomplete = 401;

        /// <summary>
        /// Contains all encountered validation errors
        /// </summary>
        private List<ValidationError> validationErrors = new List<ValidationError>();

        /// <summary>
        /// This set contains tokens for all validation errors that should be ignored
        /// </summary>
        private HashSet<string> overriddenValidationErrors = new HashSet<string>();

        private void ValidateReports()
        {
            WriteInfo("Validating the selected reports...");

            HashSet<int> organizationsOutOfCity = GetOrganizationsOutOfCity();

            // Get all the overridden errors from the database
            string query = "SELECT token FROM validation_errors WHERE is_overridden = 1";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string token = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();

                        if (token.Length > 0)
                        {
                            overriddenValidationErrors.Add(token);
                        }
                    }

                    reader.Close();
                }
            }

            // Get all reports that must be validated
            Dictionary<int, int> reports = new Dictionary<int, int>();

            query = "SELECT id, organization_id FROM reports WHERE load_status = 0 AND data_status = 1 AND validation_status = 0";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reports.Add(reader.GetInt32(0), reader.GetInt32(1));
                    }
                }
            }

            HashSet<int> validatedReports = new HashSet<int>();

            foreach (KeyValuePair<int, int> pair in reports)
            {
                if (ValidateReport(pair.Key, pair.Value, organizationsOutOfCity))
                {
                    validatedReports.Add(pair.Key);
                }
            }

            // Set the validation flag for all unprocessed reports
            foreach (KeyValuePair<int, int> pair in reports)
            {
                query = "UPDATE reports SET validation_status = @vs WHERE id = @rid";

                using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
                {
                    cmd.Parameters.Add(new SqlParameter("rid", pair.Key));

                    if (validatedReports.Contains(pair.Key))
                    {
                        cmd.Parameters.Add(new SqlParameter("vs", 1));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("vs", -1));
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool ValidateReport(int reportId, int organizationId, HashSet<int> organizationsOutOfCity)
        {
            WriteInfo("Validating report " + reportId.ToString());

            int errorCount = 0;

            // Delete all errors for this report from previous validations; leave the overridden errors
            string query = "DELETE FROM validation_errors WHERE report_id = @rid AND is_overridden = 0";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                cmd.Parameters.Add(new SqlParameter("rid", reportId));
                cmd.ExecuteNonQuery();
            }

            // Process objects that were deleted in 1NF
            CheckDeletedObjects(reportId);

            // Process balans objects with changed address
            CheckBalansObjAddressChange(reportId);

            // Perform validation
            Dictionary<int, string> objDescriptions = new Dictionary<int, string>();
            Dictionary<int, string> orgDescriptions = new Dictionary<int, string>();

            PrepareValidationObjDescriptions(reportId, objDescriptions);
            PrepareValidationOrgDescriptions(reportId, orgDescriptions);

            HashSet<int> requiredObjects = new HashSet<int>();
            HashSet<int> requiredOrganizations = new HashSet<int>();
            HashSet<int> buildingsRentedByReportSubmitter = new HashSet<int>();
            HashSet<int> renters = new HashSet<int>();
            HashSet<int> nonStandardRentAgreements = new HashSet<int>();

            GetNonStandardRentAgreementsByNote(reportId, nonStandardRentAgreements);

            ValidateReportBalans(reportId, requiredObjects, requiredOrganizations, ref errorCount, objDescriptions, orgDescriptions);

            ValidateReportArenda(reportId, organizationId, requiredObjects, requiredOrganizations, ref errorCount,
                objDescriptions, orgDescriptions, buildingsRentedByReportSubmitter, renters, nonStandardRentAgreements);

            ValidateReportObjects(reportId, requiredObjects, ref errorCount, objDescriptions, buildingsRentedByReportSubmitter,
                organizationsOutOfCity.Contains(organizationId));

            ValidateReportOrganizations(reportId, organizationId, requiredOrganizations, ref errorCount, orgDescriptions, renters);

            // Write all the found validation errors to the database
            foreach (ValidationError err in validationErrors)
            {
                query = "INSERT INTO validation_errors (report_id, error_kind, error_msg, obj_id, token, object_descr) VALUES (@rep, @err, @msg, @obj, @tok, @descr)";

                using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
                {
                    cmd.Parameters.Add(new SqlParameter("rep", err.reportId));
                    cmd.Parameters.Add(new SqlParameter("obj", err.objectId));
                    cmd.Parameters.Add(new SqlParameter("err", err.errorCode));
                    cmd.Parameters.Add(new SqlParameter("msg", err.message));
                    cmd.Parameters.Add(new SqlParameter("tok", err.token));
                    cmd.Parameters.Add(new SqlParameter("descr", err.objectDescription));

                    cmd.ExecuteNonQuery();
                }
            }

            validationErrors.Clear();

            return errorCount == 0;
        }

        private void PrepareValidationObjDescriptions(int reportId, Dictionary<int, string> objDescriptions)
        {
            // Create mapping for the columns between our database and 1NF database
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            Dictionary<string, object> data = new Dictionary<string, object>();

            CreateObjectFieldMapping(mapping);

            string srcFieldList = GetSrcFieldList(mapping, true);

            // Validate all the added and modified objects
            string query = "SELECT object_kod, unloading, " + srcFieldList + " FROM object_1nf WHERE edited <> 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int objectId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);

                        if (objectId > 0)
                        {
                            GetReaderRowData(reader, data, mapping);

                            int streetCode1 = data.ContainsKey("ulkod") ? (int)data["ulkod"] : -1;
                            int streetCode2 = data.ContainsKey("ulkod2") ? (int)data["ulkod2"] : -1;
                            decimal sqr = data.ContainsKey("szag") ? (decimal)data["szag"] : 0;
                            string number1 = data.ContainsKey("nomer1") ? (string)data["nomer1"] : "";
                            string number2 = data.ContainsKey("nomer2") ? (string)data["nomer2"] : "";
                            string number3 = data.ContainsKey("nomer3") ? (string)data["nomer3"] : "";
                            string miscInfo = data.ContainsKey("adrdop") ? (string)data["adrdop"] : "";

                            // Prepare object description, to display it on the error reporting web-site
                            string description = number1 + " " + number2 + " " + number3;

                            if (miscInfo.Length > 0)
                            {
                                description += "(" + miscInfo + ")";
                            }

                            /* if (sqr > 0)
                            {
                                description += ", " + sqr.ToString() + " " + Properties.Resources.SquareMeters;
                            } */

                            string street1 = "";
                            string street2 = "";

                            if (!objectFinder.GetStreetName(streetCode1, out street1))
                            {
                                street1 = "";
                            }

                            if (!objectFinder.GetStreetName(streetCode2, out street2))
                            {
                                street2 = "";
                            }

                            if (street2.Length > 0)
                            {
                                description = street1 + " / " + street2 + " " + description;
                            }
                            else
                            {
                                description = street1 + " " + description;
                            }

                            if (description.Trim().Length == 0)
                            {
                                description = Properties.Resources.ValidationNoObjectInfo;
                            }

                            objDescriptions[objectId] = description;
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void PrepareValidationOrgDescriptions(int reportId, Dictionary<int, string> orgDescriptions)
        {
            // Create mapping for the columns between our database and 1NF database
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            Dictionary<string, object> data = new Dictionary<string, object>();

            CreateOrganizationFieldMapping(mapping);

            string srcFieldList = GetSrcFieldList(mapping, true);

            // Validate all the added and modified organizations
            string query = "SELECT kod_obj, " + srcFieldList + " FROM sorg_1nf WHERE edited <> 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int organizationId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);

                        if (organizationId > 0)
                        {
                            GetReaderRowData(reader, data, mapping);

                            string zkpo = data.ContainsKey("kod_zkpo") ? (string)data["kod_zkpo"] : "";
                            string full_name = data.ContainsKey("full_name_obj") ? (string)data["full_name_obj"] : "";

                            zkpo = zkpo.Trim();
                            full_name = full_name.Trim();

                            // Prepare organizations description, to display it on the error reporting web-site
                            string description = full_name;

                            if (zkpo.Length > 0)
                            {
                                description += " (" + zkpo + ")";
                            }

                            if (description.Trim().Length == 0)
                            {
                                description = Properties.Resources.ValidationNoOrgInfo;
                            }

                            orgDescriptions[organizationId] = description;
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void ValidateReportObjects(int reportId, HashSet<int> requiredObjects, ref int errorCount,
            Dictionary<int, string> objDescriptions, HashSet<int> buildingsRentedByReportSubmitter, bool allowObjectsWithoutDistricts)
        {
            Dictionary<int, int> objectMapTo1NF = new Dictionary<int, int>();
            Dictionary<int, decimal> objectSquare = new Dictionary<int, decimal>();

            // Create mapping for the columns between our database and 1NF database
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            Dictionary<string, object> data = new Dictionary<string, object>();

            CreateObjectFieldMapping(mapping);

            string srcFieldList = GetSrcFieldList(mapping, true);

            // Validate all the added and modified objects
            string query = "SELECT object_kod, unloading, " + srcFieldList + " FROM object_1nf WHERE edited <> 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int objectId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        int unloading = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);

                        if (objectId > 0 && requiredObjects.Contains(objectId))
                        {
                            int objectErrorCount = 0;
                            GetReaderRowData(reader, data, mapping);

                            int streetCode1 = data.ContainsKey("ulkod") ? (int)data["ulkod"] : -1;
                            int streetCode2 = data.ContainsKey("ulkod2") ? (int)data["ulkod2"] : -1;
                            int districtCode = data.ContainsKey("newdistr") ? (int)data["newdistr"] : -1;
                            decimal sqr = data.ContainsKey("szag") ? (decimal)data["szag"] : 0;
                            string number1 = data.ContainsKey("nomer1") ? (string)data["nomer1"] : "";
                            string number2 = data.ContainsKey("nomer2") ? (string)data["nomer2"] : "";
                            string number3 = data.ContainsKey("nomer3") ? (string)data["nomer3"] : "";
                            string miscInfo = data.ContainsKey("adrdop") ? (string)data["adrdop"] : "";

                            // Get the object description, prepared earlier
                            string description = "";
                            objDescriptions.TryGetValue(objectId, out description);

                            // Check if both street names are known
                            string street1 = "";
                            string street2 = "";

                            if (streetCode1 > 0 && !objectFinder.GetStreetName(streetCode1, out street1))
                            {
                                SubmitValidationError(reportId, objectId, ERR_ObjUnknownStreet, description,
                                    Properties.Resources.ValidationUnknownStreetCode, ref objectErrorCount);
                            }

                            if (objectErrorCount == 0 && streetCode2 > 0 && !objectFinder.GetStreetName(streetCode2, out street2))
                            {
                                SubmitValidationError(reportId, objectId, ERR_ObjUnknownStreet, description,
                                    Properties.Resources.ValidationUnknownStreetCode, ref objectErrorCount);
                            }

                            // Correct some known misspellings
                            number1 = number1.Trim().ToUpper();
                            number2 = number2.Trim().ToUpper();
                            number3 = number3.Trim().ToUpper();
                            miscInfo = miscInfo.Trim().ToUpper();

                            CorrectKnownAddrMisspellings(ref street1, ref street2, ref number1, ref number2, ref number3, ref miscInfo);

                            // Check if all required properties for this object are defined
                            bool objectCanBeMatched = true;
                            string requiredProperties = "";
                            string divider = "";

                            if (streetCode1 <= 0)
                            {
                                requiredProperties += divider + Properties.Resources.ValidationAddrStreet;
                                divider = ", ";
                                objectCanBeMatched = false;
                            }

                            if (!IsVillage(street1) && !IsVillage(street2) && districtCode <= 0 && !allowObjectsWithoutDistricts) // District codes do not apply to suburbs
                            {
                                requiredProperties += divider + Properties.Resources.ValidationAddrDistrict;
                                divider = ", ";
                            }

                            if (sqr <= 0)
                            {
                                // Building square is not required if this building is rented by the report submitter
                                if (!buildingsRentedByReportSubmitter.Contains(objectId))
                                {
                                    requiredProperties += divider + Properties.Resources.ValidationAddrSquare;
                                    divider = ", ";
                                }
                            }

                            if (number1.Trim() + number2.Trim() + number3.Trim() + miscInfo.Trim() == "")
                            {
                                requiredProperties += divider + Properties.Resources.ValidationAddrNumber;
                                divider = ", ";
                                objectCanBeMatched = false;
                            }

                            if (objectCanBeMatched)
                            {
                                // Try to find this object in the database, and check the square
                                bool addressIsSimple = false;
                                bool similarAddressExists = false;

                                int existingId = objectFinder.FindObject(street1, street2, number1, number2, number3, miscInfo,
                                    districtCode > 0 ? (object)districtCode : null,
                                    sqr > 0 ? (object)sqr : null,
                                    out addressIsSimple,
                                    out similarAddressExists);

                                if (existingId > 0)
                                {
                                    ObjectInfo info = objectFinder.GetObjectInfo(existingId);

                                    // Save the object for future validation
                                    objectMapTo1NF.Add(objectId, existingId);
                                    objectSquare.Add(objectId, sqr);

                                    // Write the match to the log
                                    if (info != null)
                                    {
                                        logObjAddressMatched.WriteCell(description);
                                        logObjAddressMatched.WriteCell(info.FormatToStr());
                                        logObjAddressMatched.WriteEndOfLine();

                                        // If this match is suspicious, write it to the special log
                                        if (unloading != 0 && existingId != objectId)
                                        {
                                            logObjAddressWrongMatch.WriteCell(description);
                                            logObjAddressWrongMatch.WriteCell(info.FormatToStr());
                                            logObjAddressWrongMatch.WriteEndOfLine();
                                        }
                                    }
                                }
                                else
                                {
                                    // Write the unmatched objects to the log
                                    logObjAddressNoMatch.WriteCell(description);
                                    logObjAddressNoMatch.WriteCell("NO MATCH");
                                    logObjAddressNoMatch.WriteEndOfLine();

                                    // No match for this object; we need all properties to be filled
                                    if (requiredProperties.Length > 0)
                                    {
                                        SubmitValidationError(reportId, objectId, ERR_ObjIncomplete, description,
                                            String.Format(Properties.Resources.ValidationMsgObjIncomplete, requiredProperties),
                                            ref objectErrorCount);
                                    }
                                }
                            }
                            else
                            {
                                // Some properties required for matching are missing
                                if (requiredProperties.Length > 0)
                                {
                                    SubmitValidationError(reportId, objectId, ERR_ObjIncomplete, description,
                                        String.Format(Properties.Resources.ValidationMsgObjIncomplete, requiredProperties),
                                        ref objectErrorCount);
                                }
                            }

                            errorCount += objectErrorCount;
                        }
                    }

                    reader.Close();
                }
            }

            ValidateReportObjectsThatCanBeMatched(reportId, objectMapTo1NF, objectSquare,
                ref errorCount, objDescriptions, buildingsRentedByReportSubmitter);
        }

        private void ValidateReportObjectsThatCanBeMatched(int reportId,
            Dictionary<int, int> objectMapTo1NF,
            Dictionary<int, decimal> objectSquare,
            ref int errorCount,
            Dictionary<int, string> objDescriptions,
            HashSet<int> buildingsRentedByReportSubmitter)
        {
            /*
            // If there are objects with identical address - group them
            Dictionary<int, List<int>> objectMap1NFToReport = new Dictionary<int, List<int>>();

            foreach (KeyValuePair<int, int> pair in objectMapTo1NF)
            {
                List<int> list = null;

                if (objectMap1NFToReport.TryGetValue(pair.Value, out list))
                {
                    list.Add(pair.Key);
                }
                else
                {
                    list = new List<int>();

                    list.Add(pair.Key);

                    objectMap1NFToReport.Add(pair.Value, list);
                }
            }

            foreach (KeyValuePair<int, List<int>> pair in objectMap1NFToReport)
            {
                ObjectInfo info = objectFinder.GetObjectInfo(pair.Key);

                if (info != null && info.totalSquare is decimal)
                {
                    // Find an object with exact match by square
                    int primaryObjectInReport = -1;
                    decimal bestDiff = -1m;

                    foreach (int objectId in pair.Value)
                    {
                        decimal sqr = 0m;

                        if (objectSquare.TryGetValue(objectId, out sqr))
                        {
                            decimal diff = Math.Abs(sqr - (decimal)info.totalSquare);

                            if (primaryObjectInReport < 0 || diff < bestDiff)
                            {
                                primaryObjectInReport = objectId;
                                bestDiff = diff;
                            }
                        }
                    }

                    if (primaryObjectInReport > 0 && bestDiff < 20m)
                    {
                        // Replace all objects in this group with the primary object (with best square match)
                        foreach (int objectId in pair.Value)
                        {
                            if (objectId != primaryObjectInReport)
                            {
                                Replace1NFReportObject(objectId, primaryObjectInReport, reportId);

                                // No need to further validate such objects
                                objectMapTo1NF.Remove(objectId);
                            }
                        }
                    }
                }
            }
            */

            // Perform validation of the remaining objects
            foreach (KeyValuePair<int, int> pair in objectMapTo1NF)
            {
                int objectId = pair.Key;
                int existingId = pair.Value;
                decimal sqr = 0m;

                ObjectInfo info = objectFinder.GetObjectInfo(existingId);

                if (info != null && info.totalSquare is decimal && objectSquare.TryGetValue(objectId, out sqr))
                {
                    decimal prevSquare = (decimal)info.totalSquare;

                    if (Math.Abs(prevSquare - sqr) >= 20)
                    {
                        // Building square mismatch is allowed if this building is rented by the report submitter
                        if (!buildingsRentedByReportSubmitter.Contains(objectId))
                        {
                            // Get the object description, prepared earlier
                            string description = "";

                            if (!objDescriptions.TryGetValue(objectId, out description))
                            {
                                description = "";
                            }

                            SubmitValidationError(reportId, objectId, ERR_ObjDifferentSquare, description,
                                String.Format(Properties.Resources.ValidationMsgObjDiffSqr, prevSquare, sqr),
                                ref errorCount);
                        }
                    }
                }
            }
        }

        private void Replace1NFReportObject(int oldObjectId, int newObjectId, int reportId)
        {
            string query = @"UPDATE arenda_1nf SET object_kod = @newobj WHERE report_id = @rid AND object_kod = @oldobj";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                cmd.Parameters.Add(new SqlParameter("rid", reportId));
                cmd.Parameters.Add(new SqlParameter("oldobj", oldObjectId));
                cmd.Parameters.Add(new SqlParameter("newobj", newObjectId));

                cmd.ExecuteNonQuery();
            }

            query = @"UPDATE balans_1nf SET object_id = @newobj WHERE report_id = @rid AND object_id = @oldobj";

            using (SqlCommand cmd2 = new SqlCommand(query, connectionSqlServer))
            {
                cmd2.Parameters.Add(new SqlParameter("rid", reportId));
                cmd2.Parameters.Add(new SqlParameter("oldobj", oldObjectId));
                cmd2.Parameters.Add(new SqlParameter("newobj", newObjectId));

                cmd2.ExecuteNonQuery();
            }
        }

        private void ValidateReportOrganizations(int reportId, int orgIdReportSubmitter, HashSet<int> requiredOrganizations,
            ref int errorCount, Dictionary<int, string> orgDescriptions, HashSet<int> renters)
        {
            // Create mapping for the columns between our database and 1NF database
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            Dictionary<string, object> data = new Dictionary<string, object>();

            CreateOrganizationFieldMapping(mapping);

            string srcFieldList = GetSrcFieldList(mapping, true);

            // Validate all the added and modified organizations
            string query = "SELECT kod_obj, " + srcFieldList + " FROM sorg_1nf WHERE edited <> 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int organizationId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);

                        if (organizationId > 0 && requiredOrganizations.Contains(organizationId))
                        {
                            int objectErrorCount = 0;
                            GetReaderRowData(reader, data, mapping);

                            string zkpo = data.ContainsKey("kod_zkpo") ? (string)data["kod_zkpo"] : "";
                            string full_name = data.ContainsKey("full_name_obj") ? (string)data["full_name_obj"] : "";
                            int industryCode = data.ContainsKey("kod_galuz") ? (int)data["kod_galuz"] : -1;
                            int subIndustryCode = data.ContainsKey("kod_vid_dial") ? (int)data["kod_vid_dial"] : -1;
                            int ownershipCode = data.ContainsKey("kod_form_vlasn") ? (int)data["kod_form_vlasn"] : -1;

                            zkpo = zkpo.Trim();
                            full_name = full_name.Trim();

                            // Get the organization description, prepared earlier
                            string description = "";
                            orgDescriptions.TryGetValue(organizationId, out description);

                            // Check if all required properties for this organization are defined
                            string requiredProperties = "";
                            string divider = "";

                            if (zkpo.Length == 0)
                            {
                                requiredProperties += divider + Properties.Resources.ValidationOrgZKPO;
                                divider = ", ";
                            }

                            if (full_name.Length == 0)
                            {
                                requiredProperties += divider + Properties.Resources.ValidationOrgName;
                                divider = ", ";
                            }

                            if (requiredProperties.Length > 0)
                            {
                                SubmitValidationError(reportId, organizationId, ERR_OrgNoPrimaryInfo, description,
                                    String.Format(Properties.Resources.ValidationMsgOrgIncomplete, requiredProperties),
                                    ref objectErrorCount);
                            }

                            // Check if we can find organization by this ZKPO code
                            bool forceCreate = false;
                            int existingId = (zkpo.Length > 0) ? organizationFinder.GetOrgIdByZKPO(zkpo, full_name, false, out forceCreate) : -1;

                            if (existingId < 0 && zkpo.Length > 0 && full_name.Length > 0 && !organizationFinder.IsUniqueZKPO(zkpo) && !forceCreate)
                            {
                                SubmitValidationError(reportId, organizationId, ERR_OrgDuplicateZKPO, description,
                                    String.Format(Properties.Resources.ValidationOrgDuplicateZKPO, zkpo),
                                    ref objectErrorCount);

                                // Write this organization to the log
                                logUnmatchedOrgsBecauseOfDuplicateZKPO.WriteCell(zkpo);
                                logUnmatchedOrgsBecauseOfDuplicateZKPO.WriteCell(full_name);
                                logUnmatchedOrgsBecauseOfDuplicateZKPO.WriteEndOfLine();
                            }

                            if (existingId > 0)
                            {
                                OrganizationInfo info = organizationFinder.GetOrgInfoById(existingId);

                                if (info != null)
                                {
                                    requiredProperties = "";
                                    divider = "";

                                    // Check industry
                                    if (industryCode > 0 && info.industryCode is int && (int)info.industryCode > 0 && organizationId == orgIdReportSubmitter)
                                    {
                                        if (industryCode != (int)info.industryCode)
                                        {
                                            requiredProperties += divider + Properties.Resources.ValidationOrgIndustry;
                                            divider = ", ";
                                        }
                                    }

                                    // Check activity
                                    if (subIndustryCode > 0 && info.subIndustryCode is int && (int)info.subIndustryCode > 0 && organizationId == orgIdReportSubmitter)
                                    {
                                        if (subIndustryCode != (int)info.subIndustryCode)
                                        {
                                            requiredProperties += divider + Properties.Resources.ValidationOrgActivity;
                                            divider = ", ";
                                        }
                                    }

                                    // Check form of ownership
                                    if (ownershipCode > 0 && info.ownershipCode is int && (int)info.ownershipCode > 0 && renters.Contains(organizationId))
                                    {
                                        if (ownershipCode != (int)info.ownershipCode)
                                        {
                                            requiredProperties += divider + Properties.Resources.ValidationOrgOwnership;
                                            divider = ", ";
                                        }
                                    }

                                    if (requiredProperties.Length > 0)
                                    {
                                        SubmitValidationError(reportId, organizationId, ERR_OrgDataMismatch, description,
                                            String.Format(Properties.Resources.ValidationMsgOrgMismatch, zkpo, requiredProperties),
                                            ref objectErrorCount);
                                    }

                                    // Write the organization match to the log
                                    logOrgMatched.WriteCell(description);
                                    logOrgMatched.WriteCell(info.zkpoCode);
                                    logOrgMatched.WriteCell(info.fullName);
                                    logOrgMatched.WriteEndOfLine();
                                }
                            }

                            if (existingId < 0 && zkpo.Length > 0)
                            {
                                // Write the unmatched organization to the log
                                logOrgNoMatch.WriteCell(description);
                                logOrgNoMatch.WriteCell("NO MATCH");
                                logOrgNoMatch.WriteEndOfLine();

                                // Check if all additional properties for this organization are defined
                                requiredProperties = "";
                                divider = "";

                                if (industryCode <= 0 && organizationId == orgIdReportSubmitter)
                                {
                                    requiredProperties += divider + Properties.Resources.ValidationOrgIndustry;
                                    divider = ", ";
                                }

                                if (subIndustryCode <= 0 && organizationId == orgIdReportSubmitter)
                                {
                                    requiredProperties += divider + Properties.Resources.ValidationOrgActivity;
                                    divider = ", ";
                                }

                                if (ownershipCode <= 0 && renters.Contains(organizationId))
                                {
                                    requiredProperties += divider + Properties.Resources.ValidationOrgOwnership;
                                    divider = ", ";
                                }

                                if (requiredProperties.Length > 0)
                                {
                                    SubmitValidationError(reportId, organizationId, ERR_OrgNoAdditionalInfo, description,
                                        String.Format(Properties.Resources.ValidationMsgOrgNoAdditionalInfo, requiredProperties),
                                        ref objectErrorCount);
                                }
                            }

                            errorCount += objectErrorCount;
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void ValidateReportBalans(int reportId, HashSet<int> requiredObjects, HashSet<int> requiredOrganizations,
            ref int errorCount, Dictionary<int, string> objDescriptions, Dictionary<int, string> orgDescriptions)
        {
            Dictionary<int, string> balansObjDescriptions = new Dictionary<int, string>();

            // Enumerate all edited Balans entries, and collect all objects and organizations from them
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            Dictionary<string, object> data = new Dictionary<string, object>();

            CreateBalansFieldMapping(mapping);

            string srcFieldList = GetSrcFieldList(mapping, true);
            string query = "SELECT balans_id, [object_id], organization_id, " + srcFieldList + " FROM balans_1nf WHERE edited <> 0 AND deleted = 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GetReaderRowData(reader, data, mapping);

                        int balansId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        int objectId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                        int organizationId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                        decimal sqrTotal = data.ContainsKey("sqr_zag") ? (decimal)data["sqr_zag"] : 0m;
                        // decimal sqrNonHabit = data.ContainsKey("sqr_nej") ? (decimal)data["sqr_nej"] : 0;
                        int purposeGroupCode = data.ContainsKey("grpurp") ? (int)data["grpurp"] : -1;
                        int purposeCode = data.ContainsKey("purpose") ? (int)data["purpose"] : -1;
                        string purposeStr = data.ContainsKey("purp_str") ? (string)data["purp_str"] : "";

                        // Ignore the balans object with missing square
                        if (sqrTotal > 0)
                        {
                            // Prepare balans object description, to display it on the error reporting web-site
                            string description = purposeStr;

                            if (sqrTotal > 0)
                            {
                                description += ", " + sqrTotal.ToString() + " " + Properties.Resources.SquareMeters;
                            }

                            string orgDescription = "";

                            if (orgDescriptions.TryGetValue(organizationId, out orgDescription))
                            {
                                description = orgDescription + ", " + description;
                            }

                            string objDescription = "";

                            if (objDescriptions.TryGetValue(objectId, out objDescription))
                            {
                                description = objDescription + ", " + description;
                            }

                            balansObjDescriptions.Add(balansId, description);

                            // Check that all required properties for Balans entry are present
                            string requiredProperties = "";
                            string divider = "";

                            if (objectId <= 0)
                            {
                                requiredProperties += divider + Properties.Resources.ValidationBalansObject;
                                divider = ", ";
                            }

                            if (organizationId <= 0)
                            {
                                requiredProperties += divider + Properties.Resources.ValidationBalansOrg;
                                divider = ", ";
                            }

                            if (purposeGroupCode <= 0)
                            {
                                requiredProperties += divider + Properties.Resources.ValidationBalansPurposeGroup;
                                divider = ", ";
                            }

                            if (purposeCode <= 0)
                            {
                                if (purposeGroupCode > 0 && emptyPurposeGroups.Contains(purposeGroupCode))
                                {
                                    // Some purpose groups are empty; we should not require a purpose code for such purpose groups
                                }
                                else
                                {
                                    requiredProperties += divider + Properties.Resources.ValidationBalansPurpose;
                                    divider = ", ";
                                }
                            }

                            if (sqrTotal <= 0)
                            {
                                requiredProperties += divider + Properties.Resources.ValidationBalansTotalSqr;
                                divider = ", ";
                            }

                            if (requiredProperties.Length > 0)
                            {
                                // This check is disabled - there is no way to enter non-habitable square in the report module
                                /*
                                // If some errors are already generated, ask for non-habitable square as well
                                if (sqrNonHabit <= 0)
                                {
                                    requiredProperties += divider + Properties.Resources.ValidationBalansNonHabitSqr;
                                    divider = ", ";
                                }
                                */

                                SubmitValidationError(reportId, balansId, ERR_BalansIncomplete, description,
                                    String.Format(Properties.Resources.ValidationMsgBalansIncomplete, requiredProperties),
                                    ref errorCount);
                            }

                            // Save objects and organizations that we need to validate
                            if (objectId > 0)
                            {
                                requiredObjects.Add(objectId);
                            }

                            if (organizationId > 0)
                            {
                                requiredOrganizations.Add(organizationId);
                            }
                        }
                    }

                    reader.Close();
                }
            }

            // Check deviation of square for existing Balans entries
            query = "SELECT balans_id, sqr_zag, sqr_nej FROM balans_1nf WHERE unloading <> 0 AND edited <> 0 AND deleted = 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int balansId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        decimal sqrTotal = reader.IsDBNull(1) ? -1 : reader.GetDecimal(1);
                        decimal sqrNonHabit = reader.IsDBNull(2) ? -1 : reader.GetDecimal(2);

                        // Skip objects with zero square - they are not imported
                        if (sqrTotal > 0m)
                        {
                            // Get balans object description (prepared earlier)
                            string description = "";
                            balansObjDescriptions.TryGetValue(balansId, out description);

                            // Compare to existing values
                            BalansObjectInfo info = null;

                            if (balansCacheByID.TryGetValue(balansId, out info))
                            {
                                if (sqrTotal >= 0 && info.totalSquare is decimal)
                                {
                                    decimal diff = Math.Abs((decimal)info.totalSquare - sqrTotal);

                                    if (diff >= 20)
                                    {
                                        SubmitValidationError(reportId, balansId, ERR_BalansDifferentTotalSquare, description,
                                            String.Format(Properties.Resources.ValidationMsgBalansTotalSquareMismatch, (decimal)info.totalSquare, sqrTotal),
                                            ref errorCount);
                                    }
                                }

                                // This check is disabled - there is no way to enter non-habitable square in the report module
                                /*
                                if (sqrNonHabit >= 0 && info.nonHabitSquare is decimal)
                                {
                                    decimal diff = Math.Abs((decimal)info.nonHabitSquare - sqrNonHabit);

                                    if (diff >= 20)
                                    {
                                        SubmitValidationError(reportId, balansId, ERR_BalansDifferentNonHabitSquare, description,
                                            String.Format(Properties.Resources.ValidationMsgBalansNonHabitSquareMismatch, (decimal)info.nonHabitSquare, sqrNonHabit),
                                            ref errorCount);
                                    }
                                }
                                */
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void ValidateReportArenda(int reportId, int organizationId, HashSet<int> requiredObjects,
            HashSet<int> requiredOrganizations, ref int errorCount, Dictionary<int, string> objDescriptions,
            Dictionary<int, string> orgDescriptions, HashSet<int> buildingsRentedByReportSubmitter, HashSet<int> renters,
            HashSet<int> nonStandardRentAgreements)
        {
            // Enumerate all edited Arenda entries, and collect all objects and organizations from them
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            HashSet<int> paymentTypes = new HashSet<int>();

            CreateArendaFieldMapping(mapping);

            string srcFieldList = GetSrcFieldList(mapping, true);
            string query = "SELECT id_arenda, orgiv, org_kod, balans_kod, object_kod, " + srcFieldList + " FROM arenda_1nf WHERE edited <> 0 AND deleted = 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int objectErrorCount = 0;
                        GetReaderRowData(reader, data, mapping);

                        int arendaId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        int orgGiverId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                        int orgRenterId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                        int orgBalansId = reader.IsDBNull(3) ? -1 : reader.GetInt32(3);
                        int objectId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);

                        string agreementNum = data.ContainsKey("dognum") ? (string)data["dognum"] : "";
                        object agreementDate = data.ContainsKey("dogdate") ? data["dogdate"] : null;
                        int agreementKind = data.ContainsKey("dogkind") ? (int)data["dogkind"] : -1;
                        decimal sqr = data.ContainsKey("square") ? (decimal)data["square"] : 0;
                        decimal payment = data.ContainsKey("plata_dogovor") ? (decimal)data["plata_dogovor"] : 0;

                        // If renter is an organization which submitted this report, save the building ID
                        if (objectId > 0 && orgRenterId > 0 && orgRenterId == organizationId)
                        {
                            buildingsRentedByReportSubmitter.Add(objectId);
                        }

                        // If agreement kind is not standard, we do not require 'payment' to be filled
                        bool paymentIsRequired = true;

                        if (agreementKind > 0 && agreementKind != 1)
                        {
                            paymentIsRequired = false;
                        }

                        if (nonStandardRentAgreements.Contains(arendaId))
                        {
                            paymentIsRequired = false;
                        }

                        // Prepare rent agreement description, to display it on the error reporting web-site
                        string description = "";

                        if (agreementNum.Length > 0 && agreementDate is DateTime)
                        {
                            description = String.Format(Properties.Resources.ValidationRentAgreementDescr,
                                agreementNum, ((DateTime)agreementDate).ToShortDateString());
                        }

                        if (sqr > 0)
                        {
                            description += " " + sqr.ToString() + " " + Properties.Resources.SquareMeters;
                        }

                        string orgDescription = "";

                        if (orgDescriptions.TryGetValue(orgRenterId, out orgDescription))
                        {
                            description = Properties.Resources.ValidationRentAgreementRenter + " " + orgDescription + ", " + description;
                        }

                        string objDescription = "";

                        if (objDescriptions.TryGetValue(objectId, out objDescription))
                        {
                            description = objDescription + ", " + description;
                        }

                        // Check that all required properties for Rent agreement are present
                        string requiredProperties = "";
                        string divider = "";

                        if (objectId <= 0)
                        {
                            requiredProperties += divider + Properties.Resources.ValidationArendaObject;
                            divider = ", ";
                        }

                        if (orgRenterId <= 0)
                        {
                            requiredProperties += divider + Properties.Resources.ValidationArendaRenter;
                            divider = ", ";
                        }

                        if (orgGiverId <= 0)
                        {
                            requiredProperties += divider + Properties.Resources.ValidationArendaGiver;
                            divider = ", ";
                        }

                        if (orgBalansId <= 0)
                        {
                            requiredProperties += divider + Properties.Resources.ValidationArendaBalansHolder;
                            divider = ", ";
                        }

                        if (sqr <= 0)
                        {
                            requiredProperties += divider + Properties.Resources.ValidationArendaSquare;
                            divider = ", ";
                        }

                        if (payment <= 0)
                        {
                            // Payment is not required if 'renter' organization is the one that submitted the report
                            if (orgRenterId != organizationId && paymentIsRequired)
                            {
                                requiredProperties += divider + Properties.Resources.ValidationArendaPayment;
                                divider = ", ";
                            }
                        }

                        if (!data.ContainsKey("start_orenda"))
                        {
                            requiredProperties += divider + Properties.Resources.ValidationArendaStart;
                            divider = ", ";
                        }

                        if (requiredProperties.Length > 0)
                        {
                            SubmitValidationError(reportId, arendaId, ERR_ArendaIncomplete, description,
                                String.Format(Properties.Resources.ValidationMsgArendaIncomplete, requiredProperties),
                                ref objectErrorCount);
                        }

                        errorCount += objectErrorCount;

                        // Save objects and organizations that we need to validate
                        if (objectId > 0)
                        {
                            requiredObjects.Add(objectId);
                        }

                        if (orgRenterId > 0)
                        {
                            requiredOrganizations.Add(orgRenterId);
                            renters.Add(orgRenterId);
                        }

                        if (orgGiverId > 0)
                        {
                            requiredOrganizations.Add(orgGiverId);
                        }

                        if (orgBalansId > 0)
                        {
                            requiredOrganizations.Add(orgBalansId);
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void CheckDeletedObjects(int reportId)
        {
            // Get all objects that were unloaded from 1NF, and not deleted
            Dictionary<int, int> objects = new Dictionary<int, int>();

            string query = "SELECT id, object_kod FROM object_1nf WHERE unloading <> 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                            objects.Add(reader.GetInt32(0), reader.GetInt32(1));
                    }

                    reader.Close();
                }
            }

            // All those objects must be present in the ObjectFinder
            foreach (KeyValuePair<int, int> pair in objects)
            {
                ObjectInfo info = objectFinder.GetObjectInfo(pair.Value);

                if (info == null)
                {
                    // Mark this object as 'new' in the report
                    using (SqlCommand command = new SqlCommand("UPDATE object_1nf SET unloading = 0 WHERE id = @oid AND report_id = @rid", connectionSqlServer))
                    {
                        command.Parameters.Add(new SqlParameter("oid", pair.Key));
                        command.Parameters.Add(new SqlParameter("rid", reportId));

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void CheckBalansObjAddressChange(int reportId)
        {
            // Get all balans objects that were modified
            Dictionary<int, int> balansObjects = new Dictionary<int, int>();

            string query = "SELECT id, object_id FROM balans_1nf WHERE unloading <> 0 AND edited <> 0 AND deleted = 0 AND report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                        {
                            balansObjects.Add(reader.GetInt32(0), reader.GetInt32(1));
                        }
                    }

                    reader.Close();
                }
            }

            // For each balans object see if its address was changed
            foreach (KeyValuePair<int, int> pair in balansObjects)
            {
                int rowId = pair.Key;
                int objectId = pair.Value;

                // Build unified address from the 1NF information
                string oldFullAddress = "";
                string oldSimpleAddress = "";
                bool oldAddressIsSimple = false;

                ObjectInfo info = objectFinder.GetObjectInfo(objectId);

                if (info == null)
                {
                    continue;
                }

                ObjectFinder.UnifiedAddress uniAddrOld = ObjectFinder.GetUnifiedAddressEx(
                    info.street is string ? (string)info.street : "",
                    info.nomer1 is string ? (string)info.nomer1 : "",
                    info.nomer2 is string ? (string)info.nomer2 : "",
                    info.nomer3 is string ? (string)info.nomer3 : "",
                    info.addrMiscInfo is string ? (string)info.addrMiscInfo : "",
                    out oldFullAddress, out oldSimpleAddress, out oldAddressIsSimple);

                // Build unified address from report
                string reportStreet = "";
                string reportNomer1 = "";
                string reportNomer2 = "";
                string reportNomer3 = "";
                string reportAddrDopInfo = "";

                string reportFullAddress = "";
                string reportSimpleAddress = "";
                bool reportAddressIsSimple = false;

                using (SqlCommand cmd = new SqlCommand("SELECT ulkod, nomer1, nomer2, nomer3, dodat_vid FROM object_1nf WHERE object_kod = @oid AND report_id = @rid", connectionSqlServer))
                {
                    cmd.Parameters.Add(new SqlParameter("oid", objectId));
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int reportStreetId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                            reportNomer1 = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            reportNomer2 = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            reportNomer3 = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            reportAddrDopInfo = reader.IsDBNull(4) ? "" : reader.GetString(4);

                            objectFinder.GetStreetName(reportStreetId, out reportStreet);
                        }

                        reader.Close();
                    }
                }

                ObjectFinder.UnifiedAddress uniAddrReport = ObjectFinder.GetUnifiedAddressEx(reportStreet,
                    reportNomer1, reportNomer2, reportNomer3, reportAddrDopInfo,
                    out reportFullAddress, out reportSimpleAddress, out reportAddressIsSimple);

                // Both addresses must match!
                oldFullAddress = oldFullAddress.Trim().ToUpper();
                reportFullAddress = reportFullAddress.Trim().ToUpper();

                if (oldFullAddress.Length > 0 && reportFullAddress.Length > 0 && oldFullAddress != reportFullAddress)
                {
                    try
                    {
                        // Create a copy of this balans object in our temporary database
                        SqlCommand cmdInsert = new SqlCommand("", connectionSqlServer);

                        string fieldList = "";
                        string valueList = "";

                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM balans_1nf WHERE id = @rowid AND report_id = @rid", connectionSqlServer))
                        {
                            cmd.Parameters.Add(new SqlParameter("rowid", rowId));
                            cmd.Parameters.Add(new SqlParameter("rid", reportId));

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        string columnName = reader.GetName(i).ToLower();

                                        if (columnName != "id")
                                        {
                                            if (reader.IsDBNull(i))
                                            {
                                                fieldList += ", " + columnName;
                                                valueList += ", NULL";
                                            }
                                            else
                                            {
                                                string paramName = "param" + i.ToString();

                                                fieldList += ", " + columnName;
                                                valueList += ", @" + paramName;

                                                cmdInsert.Parameters.Add(new SqlParameter(paramName, reader.GetValue(i)));
                                            }
                                        }
                                    }
                                }

                                reader.Close();
                            }
                        }

                        if (fieldList.Length > 0 && valueList.Length > 0)
                        {
                            // Perform UPDATE
                            cmdInsert.CommandText = "INSERT INTO balans_1nf (" + fieldList.TrimStart(' ', ',') + ") VALUES (" + valueList.TrimStart(' ', ',') + ")";
                            cmdInsert.ExecuteNonQuery();
                        }

                        // Get the ID of newly inserted entry
                        int newRowId = -1;

                        using (SqlCommand cmd = new SqlCommand("SELECT MAX(id) FROM balans_1nf WHERE report_id = @rid", connectionSqlServer))
                        {
                            cmd.Parameters.Add(new SqlParameter("rid", reportId));

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (!reader.IsDBNull(0))
                                        newRowId = reader.GetInt32(0);
                                }

                                reader.Close();
                            }
                        }

                        if (newRowId > 0)
                        {
                            // Mark the new object as 'new'
                            using (SqlCommand cmd = new SqlCommand("UPDATE balans_1nf SET unloading = 0, edited = 1, deleted = 0 WHERE id = @rowid AND report_id = @rid", connectionSqlServer))
                            {
                                cmd.Parameters.Add(new SqlParameter("rowid", newRowId));
                                cmd.Parameters.Add(new SqlParameter("rid", reportId));

                                cmd.ExecuteNonQuery();
                            }

                            // Mark the old object as 'deleted'
                            using (SqlCommand cmd = new SqlCommand("UPDATE balans_1nf SET unloading = 1, edited = 0, deleted = 1 WHERE id = @rowid AND report_id = @rid", connectionSqlServer))
                            {
                                cmd.Parameters.Add(new SqlParameter("rowid", rowId));
                                cmd.Parameters.Add(new SqlParameter("rid", reportId));

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Just skip this object
                    }
                }
            }
        }

        private void GetNonStandardRentAgreementsByNote(int reportId, HashSet<int> nonStandardRentAgreements)
        {
            nonStandardRentAgreements.Clear();

            // Find all rent agreements for this report
            HashSet<int> rentAgreements = new HashSet<int>();

            string query = "SELECT id_arenda FROM arenda_1nf WHERE report_id = @rid";

            using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
            {
                command.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            rentAgreements.Add(reader.GetInt32(0));
                        }
                    }

                    reader.Close();
                }
            }

            // Check which agreements have non-standard payment
            foreach (int arendaId in rentAgreements)
            {
                query = "SELECT kod_vidplata, ar_stavka, price FROM arenda_prim WHERE report_id = @rid AND arenda = @aid";

                using (SqlCommand command = new SqlCommand(query, connectionSqlServer))
                {
                    command.Parameters.Add(new SqlParameter("rid", reportId));
                    command.Parameters.Add(new SqlParameter("aid", arendaId));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                int paymentType = reader.GetInt32(0);

                                if (paymentType == 2 ||
                                    paymentType == 3 ||
                                    paymentType == 4 ||
                                    paymentType == 6 ||
                                    paymentType == 8 ||
                                    paymentType == 9)
                                {
                                    nonStandardRentAgreements.Add(arendaId);
                                    break;
                                }
                            }
                        }

                        reader.Close();
                    }
                }
            }
        }

        private bool SubmitValidationError(int reportId, int objectId, int errorCode, string objectDescr, string message, ref int errorCount)
        {
            // Generate the error token
            string token = reportId.ToString() + "_" + objectId.ToString() + "_" + errorCode.ToString();

            // Check if this error is overridden
            if (overriddenValidationErrors.Contains(token))
            {
                return false;
            }
            else
            {
                validationErrors.Add(new ValidationError(reportId, objectId, errorCode, objectDescr, message, token));

                errorCount++;

                return true;
            }
        }

        private bool IsVillage(string streetName)
        {
            string street = streetName.ToUpper();

            foreach (string village in Properties.Settings.Default.AddrKnownVillages)
            {
                if (street.Contains(village))
                {
                    return true;
                }
            }

            return false;
        }

        private HashSet<int> GetOrganizationsOutOfCity()
        {
            HashSet<int> result = new HashSet<int>();

            using (SqlCommand cmd = new SqlCommand("SELECT organization_id FROM [GUKV].[dbo].out_of_city_organizations", connectionSqlServer))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            result.Add(reader.GetInt32(0));
                        }
                    }

                    reader.Close();
                }
            }

            return result;
        }

        #endregion (Report validation)

        #region Testing and Debugging

        /*
        private void DumpDataTable(string tableName, OleDbConnection connection)
        {
            using (OleDbCommand cmd = new OleDbCommand("select * from " + tableName, connection))
            {
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    log.WriteCell(tableName);
                    log.WriteEndOfLine();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string fieldName = reader.GetName(i);
                        string fieldType = reader.GetDataTypeName(i);

                        log.WriteCell(fieldName);
                        log.WriteCell(fieldType);
                        log.WriteEndOfLine();
                    }

                    reader.Close();

                    log.WriteEndOfLine();
                }
            }
        }
        */

        /// <summary>
        /// Returns the root logger of the log4net hierarchy
        /// </summary>
        /// <returns>The root logger of the log4net hierarchy</returns>
        private static Logger GetRootLog()
        {
            Hierarchy h = (Hierarchy)log4net.LogManager.GetRepository();

            return h.Root;
        }

        /// <summary>
        /// Adds a new message to the log
        /// </summary>
        /// <param name="message">The message text</param>
        public static void WriteInfo(string message)
        {
            Logger logger = GetRootLog();

            logger.Log(Level.Info, message, null);
        }

        /// <summary>
        /// Writes an error message to the log
        /// </summary>
        /// <param name="message">The message text</param>
        public static void WriteError(string message)
        {
            Logger logger = GetRootLog();

            logger.Log(Level.Error, message, null);
        }

        /// <summary>
        /// Writes the provided exception to the log
        /// </summary>
        /// <param name="ex">Exception to write</param>
        public static void WriteException(Exception ex)
        {
            Logger logger = GetRootLog();

            logger.Log(Level.Error, "Exception occured", ex);
        }

        /// <summary>
        /// Writes the query parameters to error log
        /// </summary>
        /// <param name="parameters">Query parameters</param>
        private void DumpQueryParameters(Dictionary<string, object> parameters)
        {
            foreach (KeyValuePair<string, object> pair in parameters)
            {
                if (pair.Value != null)
                {
                    WriteError("Parameter " + pair.Key + " = " + pair.Value.ToString());
                }
                else
                {
                    WriteError("Parameter " + pair.Key + " = NULL");
                }
            }
        }

        #endregion (Testing and Debugging)
    }
}
