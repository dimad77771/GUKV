using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using FirebirdSql.Data.FirebirdClient;

namespace GUKV.DataMigration
{
    /// <summary>
    /// Populates the SQL Server database from the Firebird databases.
    /// </summary>
    //public partial class Migrator
    //{
    //    #region Nested classes

    //    /// <summary>
    //    /// Describes a single entry of the PRAVA_PROECT table of the 'Rosporadjennia' database
    //    /// </summary>
    //    private class NJFSpecialRightInfo
    //    {
    //        /// <summary>
    //        /// ID column
    //        /// </summary>
    //        public int transferId = 0;

    //        /// <summary>
    //        /// FROM_ORG column
    //        /// </summary>
    //        public int organizationFromId = 0;

    //        /// <summary>
    //        /// TOORG column
    //        /// </summary>
    //        public int organizationToId = 0;

    //        /// <summary>
    //        /// PRAVO column
    //        /// </summary>
    //        public int rightId = 0;

    //        public NJFSpecialRightInfo()
    //        {
    //        }

    //        public NJFSpecialRightInfo(int id, int orgFrom, int orgTo, int right)
    //        {
    //            transferId = id;
    //            organizationFromId = orgFrom;
    //            organizationToId = orgTo;
    //            rightId = right;
    //        }
    //    }

    //    /// <summary>
    //    /// Describes a single entry of the OBJECT_DOKS_PROPERTIES table of the 'Rosporadjennia' database
    //    /// </summary>
    //    private class NJFObjectDocsProperties
    //    {
    //        public int documentId = 0;
    //        public int objectId = 0;
    //        public object objectName = null;
    //        public object characteristics = null;
    //        public object miscInfo = null;
    //        public object sumBalans = null;
    //        public object sumZalishkova = null;
    //        public object square = null;
    //        public object length = null;
    //        public object modifiedBy = null;
    //        public object modifyDate = null;

    //        public NJFObjectDocsProperties()
    //        {
    //        }
    //    }

    //    #endregion (Nested classes)

    //    #region Member variables

    //    /// <summary>
    //    /// Mapping between NJF objects and 1 NF objects
    //    /// </summary>
    //    private Dictionary<int, int> objIdMappingNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Mapping between NJF organizations and 1 NF organizations
    //    /// </summary>
    //    private Dictionary<int, int> orgIdMappingNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Mapping between NJF documents and 1 NF documents
    //    /// </summary>
    //    private Dictionary<int, int> docIdMappingNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Contains IDs of NJF objects that have some documents related to them
    //    /// </summary>
    //    private HashSet<int> objectsNJFRelatedToDocuments = new HashSet<int>();

    //    /// <summary>
    //    /// Contains IDs of NJF organizations that have some documents related to them
    //    /// </summary>
    //    private HashSet<int> orgNJFRelatedToObjects = new HashSet<int>();

    //    /// <summary>
    //    /// A CSV log that contains all objects from the Rosporadjennia database that were not
    //    /// found in the 1NF database
    //    /// </summary>
    //    private CSVLog csvLogUnmatchedNJFObjects = new CSVLog("CSV/NotFoundNJFObj.csv");

    //    /// <summary>
    //    /// A CSV log that contains all streets from the Rosporadjennia database that were not
    //    /// found in the 1NF database
    //    /// </summary>
    //    private CSVLog csvLogUnmatchedNJFStreets = new CSVLog("CSV/NotFoundNJFStreets.csv");

    //    /// <summary>
    //    /// A CSV log that contains all organizations from the Rosporadjennia database that were not
    //    /// found in the 1NF database
    //    /// </summary>
    //    private CSVLog csvLogUnmatchedNJFOrganizations = new CSVLog("CSV/NotFoundNJFOrg.csv");

    //    /// <summary>
    //    /// Mapping of the STEXSTAN table in Rosporadjennia to 1 NF technical states
    //    /// </summary>
    //    private Dictionary<int, int> mappingTechStateNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Mapping of the SRA table in Rosporadjennia to 1 NF districts
    //    /// </summary>
    //    private Dictionary<int, int> mappingDistrictNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Mapping of the STYPEOBJ table in Rosporadjennia to 1 NF object types
    //    /// </summary>
    //    private Dictionary<int, int> mappingObjTypeNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Mapping of the SKINDOBJ table in Rosporadjennia to 1 NF object kinds
    //    /// </summary>
    //    private Dictionary<int, int> mappingObjKindNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Mapping of the SKINDDOC table in Rosporadjennia to 1 NF document kinds
    //    /// </summary>
    //    private Dictionary<int, int> mappingDocKindNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Mapping of the SPURPOSE table in Rosporadjennia to 1 NF object purpose
    //    /// </summary>
    //    private Dictionary<int, int> mappingObjPurposeNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Mapping of the SGRPURPOSE table in Rosporadjennia to 1 NF object purpose groups
    //    /// </summary>
    //    private Dictionary<int, int> mappingObjPurposeGroupNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Mapping of the organization status from Rosporadjennia to 1 NF
    //    /// </summary>
    //    private Dictionary<int, int> mappingOrgStatusNJFto1NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// Street names from the 'Rosporadjennia' database stored by the street code
    //    /// </summary>
    //    private Dictionary<int, string> streetNamesNJF = new Dictionary<int, string>();

    //    /// <summary>
    //    /// Street codes from the 1NF database
    //    /// </summary>
    //    private Dictionary<string, int> streetCodes1NF = new Dictionary<string, int>();

    //    /// <summary>
    //    /// Mapping of street codes from NJF dictionary (table 'SUL') to 1NF dictionary (table 'SUL')
    //    /// </summary>
    //    private Dictionary<int, int> streetCodeMappingNJF21NF = new Dictionary<int, int>();

    //    /// <summary>
    //    /// The starting Id of all NJF objects which are inserted into 'buildings' table
    //    /// </summary>
    //    private int njfObjectIdSeed = 100000;

    //    /// <summary>
    //    /// Auto-incremented Id of organizations that are added from the Rosporadjennia database
    //    /// </summary>
    //    private int njfOrgIdSeed = 400000;

    //    /// <summary>
    //    /// Auto-incremented Id of 'pozoradjennia' documents that are added from the Rosporadjennia database
    //    /// </summary>
    //    private int njfDocIdSeedRaspor = 15000;

    //    /// <summary>
    //    /// Auto-incremented Id of 'act' documents that are added from the Rosporadjennia database
    //    /// </summary>
    //    private int njfDocIdSeedAct = 20000;

    //    /// <summary>
    //    /// This set contains unique keys for all transfers that were already exported to SQL Server
    //    /// </summary>
    //    private HashSet<string> existingNJFObjRightTransfers = new HashSet<string>();

    //    /// <summary>
    //    /// This cache allows to find an ID of OBJECT_DOKS table entry by object Id and document Id
    //    /// </summary>
    //    private Dictionary<string, int> njfReverseCacheObjectDocs = new Dictionary<string, int>();

    //    /// <summary>
    //    /// Stores all related documents for a given document. For a child document it allows to
    //    /// find a parent, and for a parent document it allows to find a child.
    //    /// </summary>
    //    private Dictionary<int, object> njfDocRelationCache = new Dictionary<int, object>();

    //    /// <summary>
    //    /// A full cache of the PRAVA_PROECT table
    //    /// </summary>
    //    private Dictionary<int, object> njfSpecialRightsCache = new Dictionary<int, object>();

    //    /// <summary>
    //    /// A full cache of the OBJECT_DOKS_PROPERTIES table
    //    /// </summary>
    //    private Dictionary<int, NJFObjectDocsProperties> njfObjectDoksProperties = new Dictionary<int, NJFObjectDocsProperties>();

    //    /// <summary>
    //    /// Holds a document date for every 'Akt' document from the NJF database.
    //    /// </summary>
    //    private Dictionary<int, DateTime> aktDatesNJF = new Dictionary<int, DateTime>();

    //    /// <summary>
    //    /// Holds a document date for every 'Rozporadjennia' document from the NJF database.
    //    /// </summary>
    //    private Dictionary<int, DateTime> rozpDatesNJF = new Dictionary<int, DateTime>();

    //    /// <summary>
    //    /// IDs of organizations from the 'Rosporadjennia' database that could not be mapped
    //    /// </summary>
    //    private HashSet<int> unmappedNJFOrganizations = new HashSet<int>();

    //    /// <summary>
    //    /// Contains a special string tag for each processed rozporadjennia-to-object relation
    //    /// </summary>
    //    private HashSet<string> njfRozpAndObjectsRelationCache = new HashSet<string>();

    //    /// <summary>
    //    /// Data table that contains cached entries for 'object_rights' table
    //    /// </summary>
    //    private DataTable tableNjfRights = new DataTable();

    //    /// <summary>
    //    /// A temporary buffer used for inserting new records into tableNjfRights DataTable
    //    /// </summary>
    //    private object[] tableNjfRightsRow = null;

    //    /// <summary>
    //    /// Data table that contains cached entries for 'building_docs' table
    //    /// </summary>
    //    private DataTable tableNjfBuildingDocs = new DataTable();

    //    /// <summary>
    //    /// Data table that contains cached entries for 'object_rights_building_docs' table
    //    /// </summary>
    //    private DataTable tableNjfObjRightsBuildingDocs = new DataTable();

    //    /// <summary>
    //    /// A temporary buffer used for inserting new records into tableNjfBuildingDocs
    //    /// and tableNjfObjRightsBuildingDocs DataTable
    //    /// </summary>
    //    private object[] tableNjfBuildingDocsRow = null;

    //    /// <summary>
    //    /// A temporary table which is used to store buildings from databases other thatn 1NF
    //    /// </summary>
    //    private const string BuildingsTempTable = "temp_buildings";

    //    #endregion (Member variables)

    //    #region 'Rosporadjennia' database migration

    //    private void MigrateNJF()
    //    {
    //        logger.WriteInfo("=== Migrating database 'Rosporadjennia' ===");

    //        // Connect to the Rosporadjennia database
    //        connectionNJF = ConnectToFirebirdDatabase("Rosporadjennia", GetNJFConnectionString());

    //        if (connectionNJF != null && connectionNJF.State == ConnectionState.Open)
    //        {
    //            InitNJFDataTables();

    //            streetCodeMappingNJF21NF = ObjectFinder.GetStreetCodeMatchNJFto1NF();

    //            PrepareNJFTechStateMapping();
    //            PrepareNJFDistrictMapping();
    //            PrepareNJFObjTypeMapping();
    //            PrepareNJFObjKindMapping();
    //            PrepareNJFDocKindMapping();
    //            PrepareNJFObjPurposeGroupMapping();
    //            PrepareNJFObjPurposeMapping();

    //            PrepareObjToDocRelationCache();
    //            PrepareOrgToObjectRelationCache();
    //            PrepareDocToDocRelationCache();
    //            PrepareProjectRightsCache();
    //            PrepareNJFStreetCache();

    //            // Rebuild the cache of 1NF objects before performing object matching
    //            objectFinder.BuildObjectCacheFromSqlServer(connectionSqlClient);
    //            organizationFinder.BuildOrganizationCacheFromSqlServer(connectionSqlClient, false);

    //            MigrateNJFObjects();
    //            MigrateNJFOrganizations();

    //            MigrateNJFDocDictionaries();
    //            MigrateNJFDocuments("ROZP_DOK", njfDocIdSeedRaspor);
    //            MigrateNJFDocuments("ACTS", njfDocIdSeedAct);

    //            MigrateNJFObjectRights();
    //            MigrateNJFDocRights();
    //            MigrateNJFDocLinks();

    //            if (!testOnly)
    //            {
    //                MigrateNJFDocRestrictions();
    //            }

    //            MigrateNJFObjectToDocLinks();

    //            // Dump the data from cached tables
    //            BulkMigrateDataTable("object_rights", tableNjfRights);
    //            BulkMigrateDataTable("building_docs", tableNjfBuildingDocs);
    //            BulkMigrateDataTable("object_rights_building_docs", tableNjfObjRightsBuildingDocs);

    //            // Close connection
    //            logger.WriteInfo("Disconnectiong from the Rosporadjennia database");

    //            connectionNJF.Close();
    //            connectionNJF = null;
    //        }
    //    }

    //    private void InitNJFDataTables()
    //    {
    //        // Add columns for 'object_rights' data table
    //        tableNjfRights.Columns.Add("building_id", typeof(int));
    //        tableNjfRights.Columns.Add("org_from_id", typeof(int));
    //        tableNjfRights.Columns.Add("org_to_id", typeof(int));
    //        tableNjfRights.Columns.Add("right_id", typeof(int));
    //        tableNjfRights.Columns.Add("akt_id", typeof(int));
    //        tableNjfRights.Columns.Add("rozp_id", typeof(int));
    //        tableNjfRights.Columns.Add("transfer_date", typeof(DateTime));
    //        tableNjfRights.Columns.Add("modified_by", typeof(string));
    //        tableNjfRights.Columns.Add("modify_date", typeof(DateTime));
    //        tableNjfRights.Columns.Add("name", typeof(string));
    //        tableNjfRights.Columns.Add("characteristic", typeof(string));
    //        tableNjfRights.Columns.Add("misc_info", typeof(string));
    //        tableNjfRights.Columns.Add("sum_balans", typeof(decimal));
    //        tableNjfRights.Columns.Add("sum_zalishkova", typeof(decimal));
    //        tableNjfRights.Columns.Add("sqr_transferred", typeof(decimal));
    //        tableNjfRights.Columns.Add("len_transferred", typeof(decimal));

    //        tableNjfRightsRow = new object[tableNjfRights.Columns.Count];

    //        // Add columns for 'building_docs' data table
    //        tableNjfBuildingDocs.Columns.Add("building_id", typeof(int));
    //        tableNjfBuildingDocs.Columns.Add("document_id", typeof(int));
    //        tableNjfBuildingDocs.Columns.Add("obj_name", typeof(string));
    //        tableNjfBuildingDocs.Columns.Add("obj_description", typeof(string));
    //        tableNjfBuildingDocs.Columns.Add("obj_build_year", typeof(int));

    //        tableNjfBuildingDocs.Columns.Add("obj_expl_enter_year", typeof(int));
    //        tableNjfBuildingDocs.Columns.Add("obj_length", typeof(decimal));
    //        tableNjfBuildingDocs.Columns.Add("cost_balans", typeof(decimal));
    //        tableNjfBuildingDocs.Columns.Add("cost_znos", typeof(decimal));
    //        tableNjfBuildingDocs.Columns.Add("cost_zalishkova", typeof(decimal));

    //        tableNjfBuildingDocs.Columns.Add("num_rooms", typeof(int));
    //        tableNjfBuildingDocs.Columns.Add("num_floors", typeof(int));
    //        tableNjfBuildingDocs.Columns.Add("obj_location", typeof(string));
    //        tableNjfBuildingDocs.Columns.Add("sqr_obj", typeof(decimal));
    //        tableNjfBuildingDocs.Columns.Add("sqr_free", typeof(decimal));

    //        tableNjfBuildingDocs.Columns.Add("sqr_habit", typeof(decimal));
    //        tableNjfBuildingDocs.Columns.Add("sqr_non_habit", typeof(decimal));
    //        tableNjfBuildingDocs.Columns.Add("modified_by", typeof(string));

    //        tableNjfBuildingDocs.Columns.Add("modify_date", typeof(DateTime));
    //        tableNjfBuildingDocs.Columns.Add("doc_pos", typeof(string));
    //        tableNjfBuildingDocs.Columns.Add("pipe_diameter", typeof(string));
    //        tableNjfBuildingDocs.Columns.Add("pipe_material", typeof(string));

    //        tableNjfBuildingDocs.Columns.Add("purpose_group_id", typeof(int));
    //        tableNjfBuildingDocs.Columns.Add("purpose_id", typeof(int));
    //        tableNjfBuildingDocs.Columns.Add("tech_condition_id", typeof(int));
    //        tableNjfBuildingDocs.Columns.Add("object_kind_id", typeof(int));
    //        tableNjfBuildingDocs.Columns.Add("object_type_id", typeof(int));

    //        if (!testOnly)
    //        {
    //            tableNjfBuildingDocs.Columns.Add("note", typeof(string));
    //        }

    //        // Copy the columns to the tableNjfObjRightsBuildingDocs table
    //        for (int i = 0; i < tableNjfBuildingDocs.Columns.Count; i++)
    //        {
    //            tableNjfObjRightsBuildingDocs.Columns.Add(
    //                tableNjfBuildingDocs.Columns[i].ColumnName,
    //                tableNjfBuildingDocs.Columns[i].DataType);
    //        }
            
    //        tableNjfBuildingDocsRow = new object[tableNjfBuildingDocs.Columns.Count];
    //    }

    //    private void MigrateNJFObjects()
    //    {
    //        logger.WriteInfo("Matching 'Rosporadjennia' objects against 1NF objects");

    //        // Write a header to the CSV log that contains unmatched objects
    //        csvLogUnmatchedNJFObjects.WriteCell(GUKV.DataMigration.Properties.Resources.NJFUnmatchedObjID);
    //        csvLogUnmatchedNJFObjects.WriteCell(GUKV.DataMigration.Properties.Resources.NJFUnmatchedObjStreet);
    //        csvLogUnmatchedNJFObjects.WriteCell(GUKV.DataMigration.Properties.Resources.NJFUnmatchedObjNomer1);
    //        csvLogUnmatchedNJFObjects.WriteCell(GUKV.DataMigration.Properties.Resources.NJFUnmatchedObjNomer2);
    //        csvLogUnmatchedNJFObjects.WriteCell(GUKV.DataMigration.Properties.Resources.NJFUnmatchedObjNomer3);
    //        csvLogUnmatchedNJFObjects.WriteCell(GUKV.DataMigration.Properties.Resources.NJFUnmatchedObjAddrMisc);
    //        csvLogUnmatchedNJFObjects.WriteEndOfLine();

    //        // Create field mapping for the OBJECTS table
    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        fields.Add("OBJECT_KOD", "id");
    //        fields.Add("ULNAME", "addr_street_name");
    //        fields.Add("NOMER1", "addr_nomer1");
    //        fields.Add("NOMER2", "addr_nomer2");
    //        fields.Add("NOMER3", "addr_nomer3");
    //        fields.Add("ADRDOP", "addr_misc");
    //        fields.Add("TEXSTAN", "tech_condition_id");
    //        fields.Add("BUDYEAR", "construct_year");
    //        fields.Add("NEWDISTR", "addr_distr_new_id");
    //        fields.Add("TYPOBJ", "object_type_id");
    //        fields.Add("VIDOBJ", "object_kind_id");
    //        fields.Add("ULKOD", "addr_street_id");
    //        fields.Add("SZAG", "sqr_total");

    //        // Dump all organizations from NJF database to our database
    //        ExecuteStatementInSQLServer("DELETE FROM " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable);

    //        string selectStatement = "SELECT OBJECT_KOD, ULNAME, NOMER1, NOMER2, NOMER3, ADRDOP," +
    //            " TEXSTAN, BUDYEAR, NEWDISTR, TYPOBJ, VIDOBJ, ULKOD, SZAG FROM OBJECT WHERE OBJECT_KODSTAN = 1";

    //        using (FbCommand cmd = new FbCommand(selectStatement, connectionNJF))
    //        {
    //            using (FbDataReader reader = cmd.ExecuteReader())
    //            {
    //                DataReaderAdapter adapter = new DataReaderAdapter(reader);
    //                adapter.SkipUnmappedRows = false;

    //                // Map street IDs from NJF to 1NF
    //                adapter.AddColumnMapping("ULKOD", streetCodeMappingNJF21NF);

    //                BulkMigrateDataReader("OBJECT", BuildingsTempTable, adapter, fields);

    //                reader.Close();
    //            }
    //        }

    //        // Perform mapping of some dictionaries
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable + " SET addr_street_id = NULL WHERE addr_street_id = 0");
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable + " SET addr_distr_new_id = NULL WHERE addr_distr_new_id = 0");
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable + " SET tech_condition_id = NULL WHERE tech_condition_id = 0");
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable + " SET object_type_id = NULL WHERE object_type_id = 0");
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable + " SET object_kind_id = NULL WHERE object_kind_id = 0");
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable + " SET sqr_total = NULL WHERE sqr_total <= 0");

    //        UpdateSqlServerDictionaryByMapping(BuildingsTempTable, "addr_distr_new_id", mappingDistrictNJFto1NF);
    //        UpdateSqlServerDictionaryByMapping(BuildingsTempTable, "object_type_id", mappingObjTypeNJFto1NF);
    //        UpdateSqlServerDictionaryByMapping(BuildingsTempTable, "object_kind_id", mappingObjKindNJFto1NF);

    //        // Nomer 3 is always a 'korpus' in NJF database
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable +
    //            " SET addr_nomer3 = REPLACE(addr_nomer3, N'№', '')");

    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable +
    //            " SET addr_nomer3 = N'К.' + addr_nomer3 WHERE LEN(LTRIM(RTRIM(addr_nomer3))) > 0");

    //        // Perform mapping of NJF objects to 1NF objects
    //        string query = "SELECT id, addr_street_name, addr_nomer1, addr_nomer2, addr_nomer3, addr_misc, addr_distr_new_id, sqr_total FROM " +
    //            BuildingsTempTable + " WHERE (is_deleted IS NULL) OR (is_deleted = 0)";

    //        using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
    //        {
    //            using (SqlDataReader reader = cmd.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    int id = reader.GetInt32(0);
    //                    string street = reader.IsDBNull(1) ? "" : reader.GetString(1);
    //                    string nomer1 = reader.IsDBNull(2) ? "" : reader.GetString(2);
    //                    string nomer2 = reader.IsDBNull(3) ? "" : reader.GetString(3);
    //                    string nomer3 = reader.IsDBNull(4) ? "" : reader.GetString(4);
    //                    string miscAddr = reader.IsDBNull(5) ? "" : reader.GetString(5);
    //                    object district = reader.IsDBNull(6) ? null : reader.GetValue(6);
    //                    object square = reader.IsDBNull(7) ? null : reader.GetValue(7);

    //                    if (district is int && (int)district == 0)
    //                    {
    //                        district = null;
    //                    }

    //                    if (square is decimal && (decimal)square <= 0)
    //                    {
    //                        square = null;
    //                    }

    //                    // Try to find this object in 1 NF
    //                    bool addressIsSimple = false;
    //                    bool similarAddressExists = false;

    //                    int buildingId = objectFinder.FindObject(street, "", nomer1, nomer2, nomer3, miscAddr,
    //                        district, square, out addressIsSimple, out similarAddressExists);

    //                    if (buildingId > 0)
    //                    {
    //                        // Object found; save the object mapping
    //                        objIdMappingNJFto1NF[id] = buildingId;
    //                    }
    //                    else
    //                    {
    //                        objIdMappingNJFto1NF.Add(id, id + njfObjectIdSeed);

    //                        // Address not found; write it to the log
    //                        csvLogUnmatchedNJFObjects.WriteCell(id.ToString());
    //                        csvLogUnmatchedNJFObjects.WriteCell(street);
    //                        csvLogUnmatchedNJFObjects.WriteCell(nomer1);
    //                        csvLogUnmatchedNJFObjects.WriteCell(nomer2);
    //                        csvLogUnmatchedNJFObjects.WriteCell(nomer3);
    //                        csvLogUnmatchedNJFObjects.WriteCell(miscAddr);
    //                        csvLogUnmatchedNJFObjects.WriteEndOfLine();
    //                    }
    //                }

    //                reader.Close();
    //            }
    //        }

    //        // Set the master ID for each mapped object
    //        foreach (KeyValuePair<int, int> pair in objIdMappingNJFto1NF)
    //        {
    //            if (pair.Value < njfObjectIdSeed)
    //            {
    //                query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable +
    //                    " SET master_building_id = @masterid WHERE id = @bid";

    //                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
    //                {
    //                    cmd.Parameters.Add(new SqlParameter("bid", pair.Key));
    //                    cmd.Parameters.Add(new SqlParameter("masterid", pair.Value));
    //                    cmd.ExecuteNonQuery();
    //                }
    //            }
    //        }

    //        // Make sure that NJF objects never mess up with 1NF objects
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + BuildingsTempTable +
    //            " SET id = id + " + njfObjectIdSeed);

    //        // Perform a BULK insert from the temporary table into the main table
    //        BulkMigrateNewObjects();
    //    }

    //    private void MigrateNJFOrganizations()
    //    {
    //        logger.WriteInfo("Matching 'Rosporadjennia' organizations against 1NF organizations");

    //        // Prepare the field mapping for SZKPO table
    //        Dictionary<string, string> fields = new Dictionary<string, string>();
    //        Dictionary<string, object> dataOverride = new Dictionary<string, object>();

    //        CreateNJFOrganizationsFieldMapping(fields);

    //        // Write a header to the CSV log that contains unmatched organizations
    //        csvLogUnmatchedNJFOrganizations.WriteCell(GUKV.DataMigration.Properties.Resources.UncategorizedOrgID);
    //        csvLogUnmatchedNJFOrganizations.WriteCell(GUKV.DataMigration.Properties.Resources.UncategorizedOrgZKPO);
    //        csvLogUnmatchedNJFOrganizations.WriteCell(GUKV.DataMigration.Properties.Resources.UncategorizedOrgFullName);
    //        csvLogUnmatchedNJFOrganizations.WriteCell(GUKV.DataMigration.Properties.Resources.UncategorizedOrgShortName);
    //        csvLogUnmatchedNJFOrganizations.WriteCell(GUKV.DataMigration.Properties.Resources.UncategorizedOrgIsCategorized);
    //        csvLogUnmatchedNJFOrganizations.WriteEndOfLine();

    //        // Dump all organizations from NJF database to our database
    //        ExecuteStatementInSQLServer("DELETE FROM " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable);

    //        string selectStatement = GetSelectStatement("SZKPO", fields, "", null) + " s1 WHERE STAN = (SELECT MAX(STAN) FROM SZKPO s2 WHERE s2.KOD = s1.KOD) ORDER BY KOD";

    //        using (FbCommand cmd = new FbCommand(selectStatement, connectionNJF))
    //        {
    //            using (FbDataReader r = cmd.ExecuteReader())
    //            {
    //                DataReaderAdapter adapter = new DataReaderAdapter(r);
    //                adapter.SkipUnmappedRows = false;

    //                // Map street IDs from NJF to 1NF
    //                adapter.AddColumnMapping("UL", streetCodeMappingNJF21NF);

    //                BulkMigrateDataReader("SZKPO", OrganizationsTempTable, adapter, fields);

    //                r.Close();
    //            }
    //        }

    //        // Erase immediately all organizations that are not used in the NJF database
    //        string query = @"UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
    //            @" SET is_deleted = 1 WHERE (short_name IS NULL)";

    //        ExecuteStatementInSQLServer(query);

    //        // Perform mapping of some dictionaries
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable + " SET origin_db = 3");

    //        mappingOrgStatusNJFto1NF.Add(1, 2);
    //        mappingOrgStatusNJFto1NF.Add(2, 1);

    //        UpdateSqlServerDictionaryByMapping(OrganizationsTempTable, "status_id", mappingOrgStatusNJFto1NF);
    //        UpdateSqlServerDictionaryByMapping(OrganizationsTempTable, "addr_distr_new_id", mappingDistrictNJFto1NF);
    //        UpdateSqlServerDictionaryByMapping(OrganizationsTempTable, "form_id", mappingVidPredprToOrgForm);

    //        // Update street names (we have only street code in the SZKPO table)
    //        query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
    //            " SET addr_street_name = s.name FROM " + OrganizationsTempTable + " AS org INNER JOIN dict_streets AS s ON org.addr_street_id = s.id";

    //        ExecuteStatementInSQLServer(query);

    //        // Perform mapping of NJF organizations to 1NF organizations
    //        query = "SELECT id, zkpo_code, full_name, short_name FROM " + OrganizationsTempTable + " WHERE (is_deleted IS NULL) OR (is_deleted = 0)";

    //        using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
    //        {
    //            using (SqlDataReader r = cmd.ExecuteReader())
    //            {
    //                while (r.Read())
    //                {
    //                    int id = r.GetInt32(0);
    //                    string zkpo = r.IsDBNull(1) ? "" : r.GetString(1);
    //                    string fullName = r.IsDBNull(2) ? "" : r.GetString(2);
    //                    string shortName = r.IsDBNull(3) ? "" : r.GetString(3);

    //                    bool categorized = false;

    //                    int existingOrganizationID = organizationFinder.FindOrganization(
    //                        zkpo, fullName, shortName, true, out categorized);

    //                    if (existingOrganizationID > 0)
    //                    {
    //                        orgIdMappingNJFto1NF.Add(id, existingOrganizationID);
    //                    }
    //                    else
    //                    {
    //                        orgIdMappingNJFto1NF.Add(id, id + njfOrgIdSeed);

    //                        // Organization not found; write it to the log
    //                        csvLogUnmatchedNJFOrganizations.WriteCell(id);
    //                        csvLogUnmatchedNJFOrganizations.WriteCell(zkpo);
    //                        csvLogUnmatchedNJFOrganizations.WriteCell(fullName);
    //                        csvLogUnmatchedNJFOrganizations.WriteCell(shortName);
    //                        csvLogUnmatchedNJFOrganizations.WriteCell(categorized ? "YES" : "NO");
    //                        csvLogUnmatchedNJFOrganizations.WriteEndOfLine();
    //                    }
    //                }

    //                r.Close();
    //            }
    //        }

    //        // Set the master ID for each mapped organization. This process is optimized, basing on the assumption
    //        // that many 'master' IDs are equal to the primary organization IDs
    //        query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable + " SET master_org_id = id";

    //        ExecuteStatementInSQLServer(query);

    //        foreach (KeyValuePair<int, int> pair in orgIdMappingNJFto1NF)
    //        {
    //            if (pair.Key != pair.Value)
    //            {
    //                if (pair.Value < njfOrgIdSeed)
    //                {
    //                    query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
    //                        " SET master_org_id = @masterid WHERE id = @orgid";

    //                    using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
    //                    {
    //                        cmd.Parameters.Add(new SqlParameter("orgid", pair.Key));
    //                        cmd.Parameters.Add(new SqlParameter("masterid", pair.Value));
    //                        cmd.ExecuteNonQuery();
    //                    }
    //                }
    //                else
    //                {
    //                    query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
    //                        " SET master_org_id = NULL WHERE id = @orgid";

    //                    using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
    //                    {
    //                        cmd.Parameters.Add(new SqlParameter("orgid", pair.Key));
    //                        cmd.ExecuteNonQuery();
    //                    }
    //                }
    //            }
    //        }

    //        // Make sure that NJF organizations never mess up with 1NF organizations
    //        ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
    //            " SET id = id + " + njfOrgIdSeed);

    //        // Perform a BULK insert from the temporary table into the main table
    //        BulkMigrateNewOrganizations();
    //    }

    //    private void CreateNJFOrganizationsFieldMapping(Dictionary<string, string> fields)
    //    {
    //        fields.Clear();

    //        fields.Add("KOD", "id");
    //        fields.Add("STAN", "last_state");
    //        fields.Add("ZKPO", "zkpo_code");
    //        fields.Add("NAME", "short_name");
    //        fields.Add("NAMEP", "full_name");

    //        fields.Add("VN", "vedomstvo_id");
    //        fields.Add("RA", "addr_distr_new_id");
    //        fields.Add("VO", "gosp_struct_type_id");
    //        fields.Add("FG", "form_gosp_id");
    //        fields.Add("OT", "industry_id");
    //        fields.Add("FORMVL", "form_ownership_id");
    //        fields.Add("UL", "addr_street_id");
    //        fields.Add("TYPE_ORG", "status_id");
    //        fields.Add("PV", "form_id");

    //        fields.Add("POSTIND", "addr_zip_code");
    //        fields.Add("NOMER1", "addr_nomer");
    //        fields.Add("NOMER2", "addr_nomer2");
    //        fields.Add("ADRDOP", "addr_misc");
    //        fields.Add("KVARTIRA", "addr_flat_num");

    //        fields.Add("FIO", "director_fio");
    //        fields.Add("TEL", "director_phone");
    //        fields.Add("FAX", "fax");
    //        fields.Add("FIOB", "buhgalter_fio");
    //        fields.Add("TELB", "buhgalter_phone");

    //        fields.Add("REESORG", "registration_auth");
    //        fields.Add("REESNO", "registration_num");
    //        fields.Add("REESDT", "registration_date");
    //        fields.Add("PASP_SER", "director_passp_seria");
    //        fields.Add("PASP_NOM", "director_passp_num");
    //        fields.Add("PASP_VID", "director_passp_auth");
    //        fields.Add("PASP_DT", "director_passp_date");

    //        fields.Add("ISP", "modified_by");
    //        fields.Add("DTISP", "modify_date");
    //        fields.Add("RIK", "l_year");
    //        fields.Add("DT_BEG", "beg_state_date");
    //        fields.Add("DT_END", "end_state_date");
            
    //    }

    //    private void BulkMigrateNewObjects()
    //    {
    //        // Read the data
    //        string querySelect = "SELECT * FROM " + BuildingsTempTable;

    //        try
    //        {
    //            using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
    //            {
    //                using (SqlDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    using (SqlConnection destinationConnection = new SqlConnection(GetSQLClientConnectionString()))
    //                    {
    //                        destinationConnection.Open();

    //                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
    //                        {
    //                            // Initialize the BULK copy class
    //                            bulkCopy.DestinationTableName = "dbo.buildings";

    //                            for (int i = 0; i < reader.FieldCount; i++)
    //                            {
    //                                string columnName = reader.GetName(i);

    //                                bulkCopy.ColumnMappings.Add(columnName, columnName);
    //                            }

    //                            // Write from the source to the destination
    //                            try
    //                            {
    //                                bulkCopy.WriteToServer(reader);
    //                            }
    //                            catch (Exception ex)
    //                            {
    //                                ShowSqlErrorMessageDlg("<No SQL statement available for BULK insertion>", ex.Message);

    //                                // Abort migration correctly
    //                                throw new MigrationAbortedException();
    //                            }

    //                            // Close the objects
    //                            bulkCopy.Close();
    //                        }

    //                        destinationConnection.Close();
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void MigrateNJFDocuments(string srcTableName, int idSeed)
    //    {
    //        logger.WriteInfo("Migrating documents from the table " + srcTableName);

    //        // Prepare field mapping for ROZP_DOK table
    //        Dictionary<string, string> fields = new Dictionary<string, string>();
    //        Dictionary<string, object> dataOverride = new Dictionary<string, object>();

    //        fields.Add("DOK_ID", "id");
    //        fields.Add("DOKKIND", "kind_id");
    //        fields.Add("STATUS", "state_id");
    //        fields.Add("DOKDATA", "doc_date");
    //        fields.Add("DOKNUM", "doc_num");
    //        fields.Add("PIDROZDIL", "source_id");
    //        fields.Add("DOKTEMA", "topic");
    //        fields.Add("PRIZNAK", "is_not_in_work");
    //        fields.Add("SUMMA", "summa");
    //        fields.Add("SUMMA_ZAL", "summa_zalishkova");
    //        fields.Add("DT", "modify_date");
    //        fields.Add("ISP", "modified_by");

    //        // The table ROZP_DOK contains one additional field
    //        bool migratingRozpDoc = (srcTableName == "ROZP_DOK");

    //        int docIdFieldIndex = GetSrcFieldIndexFromMapping(fields, "DOK_ID", srcTableName);
    //        int docKindFieldIndex = GetSrcFieldIndexFromMapping(fields, "DOKKIND", srcTableName);
    //        int docDateFieldIndex = GetSrcFieldIndexFromMapping(fields, "DOKDATA", srcTableName);
    //        int docNumFieldIndex = GetSrcFieldIndexFromMapping(fields, "DOKNUM", srcTableName);
    //        int docTopicFieldIndex = GetSrcFieldIndexFromMapping(fields, "DOKTEMA", srcTableName);
    //        int docInWorkFieldIndex = GetSrcFieldIndexFromMapping(fields, "PRIZNAK", srcTableName);

    //        int textExistsIndex = 0;
    //        int externDocIdIndex = 0;

    //        if (migratingRozpDoc)
    //        {
    //            fields.Add("ISTEXT", "is_text_exists");
    //            fields.Add("RDOC_ID", "extern_doc_id");

    //            textExistsIndex = GetSrcFieldIndexFromMapping(fields, "ISTEXT", srcTableName);
    //            externDocIdIndex = GetSrcFieldIndexFromMapping(fields, "RDOC_ID", srcTableName);
    //        }

    //        // This column is actually not imported. It is added to the mapping just to
    //        // override its value, and set the correct 'general document kind'
    //        fields.Add("ORG", "general_kind_id");

    //        // Get all documents from the table ROZP_DOK
    //        string querySelect = GetSelectStatement(srcTableName, fields, "", null);

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    int newDocIdSeed = idSeed;

    //                    while (reader.Read())
    //                    {
    //                        dataOverride.Clear();

    //                        // Get the object data
    //                        object dataId = reader.IsDBNull(docIdFieldIndex) ? null : reader.GetValue(docIdFieldIndex);
    //                        object dataDocKind = reader.IsDBNull(docKindFieldIndex) ? null : reader.GetValue(docKindFieldIndex);
    //                        object dataDocDate = reader.IsDBNull(docDateFieldIndex) ? null : reader.GetValue(docDateFieldIndex);
    //                        object dataDocNum = reader.IsDBNull(docNumFieldIndex) ? null : reader.GetValue(docNumFieldIndex);
    //                        object dataDocTopic = reader.IsDBNull(docTopicFieldIndex) ? null : reader.GetValue(docTopicFieldIndex);
    //                        object dataIsInWork = reader.IsDBNull(docInWorkFieldIndex) ? null : reader.GetValue(docInWorkFieldIndex);

    //                        if (dataId is int)
    //                        {
    //                            // Save the document date for future lookup
    //                            if (dataDocDate is DateTime)
    //                            {
    //                                if (migratingRozpDoc)
    //                                {
    //                                    rozpDatesNJF[(int)dataId] = (DateTime)dataDocDate;
    //                                }
    //                                else
    //                                {
    //                                    aktDatesNJF[(int)dataId] = (DateTime)dataDocDate;
    //                                }
    //                            }

    //                            // Map the document kind to 1 NF
    //                            if (dataDocKind is int)
    //                            {
    //                                int newDocKind = -1;

    //                                if (mappingDocKindNJFto1NF.TryGetValue((int)dataDocKind, out newDocKind))
    //                                {
    //                                    dataDocKind = newDocKind;
    //                                    dataOverride.Add("kind_id", newDocKind);
    //                                }
    //                            }

    //                            // Try to find a document with the same properties
    //                            int existingDocId = -1;

    //                            if (dataDocDate is DateTime && dataDocNum is string && dataDocKind is int)
    //                            {
    //                                string topic = (dataDocTopic is string) ? (string)dataDocTopic : "";

    //                                existingDocId = docFinder.FindDocumentIn1NF(
    //                                    (DateTime)dataDocDate, (string)dataDocNum, (int)dataDocKind, topic);
    //                            }

    //                            if (existingDocId > 0)
    //                            {
    //                                // Just save the mapping betwen the found document and NJF document
    //                                docIdMappingNJFto1NF[(int)dataId] = existingDocId;

    //                                // Modify the general document kind; it must be equal to 'zakriplennya mayna'
    //                                docFinder.ModifyDocGeneralKind(connectionSqlClient, existingDocId, 7);

    //                                if (migratingRozpDoc)
    //                                {
    //                                    ModifyNjfExistingDocProperties(existingDocId, 7,
    //                                        reader.IsDBNull(externDocIdIndex) ? null : reader.GetValue(externDocIdIndex),
    //                                        reader.IsDBNull(textExistsIndex) ? null : reader.GetValue(textExistsIndex));
    //                                }
    //                                else
    //                                {
    //                                    ModifyNjfExistingDocProperties(existingDocId, 7, null, null);
    //                                }
    //                            }
    //                            else
    //                            {
    //                                // Generate new document Id
    //                                int newDocId = newDocIdSeed++;

    //                                dataOverride.Add("id", newDocId);

    //                                // Invert the "is in work" attribute
    //                                if (dataIsInWork is int)
    //                                {
    //                                    dataOverride.Add("is_not_in_work", ((int)dataIsInWork == 1) ? 0 : 1);
    //                                }

    //                                // Trim all string values - document number and topic 
    //                                if (dataDocNum is string)
    //                                {
    //                                    dataOverride.Add("doc_num", ((string)dataDocNum).Trim());
    //                                }

    //                                if (dataDocTopic is string)
    //                                {
    //                                    dataOverride.Add("topic", ((string)dataDocTopic).Trim());
    //                                }

    //                                // General document kind if always the same
    //                                dataOverride.Add("general_kind_id", 7);

    //                                // Save mapping betwen old document Id and new document Id
    //                                docIdMappingNJFto1NF[(int)dataId] = newDocId;

    //                                // Generate the INSERT statement
    //                                string queryInsert = GetInsertStatement("documents", fields, reader, dataOverride);

    //                                try
    //                                {
    //                                    using (SqlCommand command = new SqlCommand(queryInsert, connectionSqlClient))
    //                                    {
    //                                        command.ExecuteNonQuery();
    //                                    }
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    ShowSqlErrorMessageDlg(queryInsert, ex.Message);

    //                                    // Abort migration correctly
    //                                    throw new MigrationAbortedException();
    //                                }

    //                                // Register the inserted document in the Document Finder, for future lookup
    //                                docFinder.RegisterDocument(newDocId, dataDocDate, dataDocNum, dataDocKind, dataDocTopic, true);
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void ModifyNjfExistingDocProperties(int documentId, int generalKindId,
    //        object externDocId, object textExists)
    //    {
    //        string query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo.documents SET" +
    //            " general_kind_id = " + generalKindId.ToString();

    //        if (externDocId is int)
    //        {
    //            query += ", extern_doc_id = " + externDocId.ToString();
    //        }

    //        if (externDocId is int)
    //        {
    //            query += ", is_text_exists = " + textExists.ToString();
    //        }

    //        query += " WHERE id = " + documentId.ToString();

    //        try
    //        {
    //            // Execute UPDATE
    //            using (SqlCommand commandUpdate = new SqlCommand(query, connectionSqlClient))
    //            {
    //                commandUpdate.ExecuteNonQuery();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.WriteSQLError(query, ex.Message);

    //            // Abort migration correctly
    //            throw new MigrationAbortedException();
    //        }
    //    }

    //    private void MigrateNJFDocDictionaries()
    //    {
    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        // SPIDROZDIL
    //        fields.Clear();
    //        fields.Add("KOD", "id");
    //        fields.Add("NAME", "name");

    //        MigrateTable(connectionNJF, "SPIDROZDIL", "dict_doc_source", fields, "KOD", null);

    //        // SSTATUS
    //        fields.Clear();
    //        fields.Add("KOD", "id");
    //        fields.Add("NAME", "name");

    //        MigrateTable(connectionNJF, "SSTATUS", "dict_doc_state", fields, "KOD", null);
    //    }

    //    private void MigrateNJFObjectRights()
    //    {
    //        ///////////////////////////////////////////////////////////////////
    //        // 1) Import the required dictionaries
    //        ///////////////////////////////////////////////////////////////////

    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        // SPIDROZDIL
    //        fields.Clear();
    //        fields.Add("KOD", "id");
    //        fields.Add("NAME", "name");

    //        MigrateTable(connectionNJF, "SPRAVO", "dict_obj_rights", fields, "KOD", null);

    //        ///////////////////////////////////////////////////////////////////
    //        // 2) Read in the entire table OBJECT_PRAVO_PROPERTIES
    //        ///////////////////////////////////////////////////////////////////

    //        logger.WriteInfo("Extracting data from the table OBJECT_PRAVO_PROPERTIES");

    //        Dictionary<int, NJFRightMiscInfo> rightPropCache = new Dictionary<int, NJFRightMiscInfo>();

    //        string querySelect = "SELECT ID, NAME, CHARACTERISTIC, ARRANGEMENT, SQUARE, " +
    //            "SUMMA_BALANS, SUMMA_ZAL, LEN FROM OBJECT_PRAVO_PROPERTIES";

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);

    //                        if (dataId is int && (int)dataId > 0)
    //                        {
    //                            // Add this information to the cache
    //                            NJFRightMiscInfo info = new NJFRightMiscInfo();

    //                            info.objectName = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                            info.characteristics = reader.IsDBNull(2) ? null : reader.GetValue(2);
    //                            info.miscInfo = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                            info.square = reader.IsDBNull(4) ? null : reader.GetValue(4);
    //                            info.sumBalans = reader.IsDBNull(5) ? null : reader.GetValue(5);
    //                            info.sumZalishkova = reader.IsDBNull(6) ? null : reader.GetValue(6);
    //                            info.length = reader.IsDBNull(7) ? null : reader.GetValue(7);

    //                            rightPropCache.Add((int)dataId, info);
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }

    //        ///////////////////////////////////////////////////////////////////
    //        // 3) Read in the entire table OBJECT_PRAVO
    //        ///////////////////////////////////////////////////////////////////

    //        logger.WriteInfo("Extracting data from the table OBJECT_PRAVO");

    //        Dictionary<int, NJFRightTransferInfo> rightCache = new Dictionary<int, NJFRightTransferInfo>();
    //        HashSet<int> closingCards = new HashSet<int>();
    //        HashSet<int> closedCards = new HashSet<int>();

    //        querySelect = "SELECT ID, OBJECT_KOD, ZKPO, PRAVO, START_DOK, START_DATE, " +
    //            "END_DOK, END_DATE, ISP, DT, CLOSINGKARD, CLOSEDKARD, STDOKOBJID FROM OBJECT_PRAVO"; // WHERE OBJECT_KODSTAN = 1

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataObjectId = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataOrgId = reader.IsDBNull(2) ? null : reader.GetValue(2);

    //                        if (dataId is int && dataObjectId is int && dataOrgId is int)
    //                        {
    //                            // Add this information to the cache
    //                            NJFRightTransferInfo info = new NJFRightTransferInfo();

    //                            info.transferId = (int)dataId;
    //                            info.objectId = (int)dataObjectId;
    //                            info.organizationId = (int)dataOrgId;

    //                            info.rightId = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                            info.addDocId = reader.IsDBNull(4) ? null : reader.GetValue(4);
    //                            info.addDocDate = reader.IsDBNull(5) ? null : reader.GetValue(5);
    //                            info.removeDocId = reader.IsDBNull(6) ? null : reader.GetValue(6);
    //                            info.removeDocDate = reader.IsDBNull(7) ? null : reader.GetValue(7);
    //                            info.modifiedBy = reader.IsDBNull(8) ? null : reader.GetValue(8);
    //                            info.modifyDate = reader.IsDBNull(9) ? null : reader.GetValue(9);
    //                            info.closingCard = reader.IsDBNull(10) ? null : reader.GetValue(10);
    //                            info.closedCard = reader.IsDBNull(11) ? null : reader.GetValue(11);
    //                            info.primaryDocId = reader.IsDBNull(12) ? null : reader.GetValue(12);

    //                            rightCache.Add((int)dataId, info);

    //                            // Populate additional caches
    //                            if (info.closingCard is int)
    //                            {
    //                                closingCards.Add((int)info.closingCard);
    //                            }

    //                            if (info.closedCard is int)
    //                            {
    //                                closedCards.Add((int)info.closedCard);
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }

    //        ///////////////////////////////////////////////////////////////////
    //        // 4) Match all rights to each other
    //        ///////////////////////////////////////////////////////////////////

    //        logger.WriteInfo("Matching object rights to each other");

    //        Dictionary<string, object> values = new Dictionary<string, object>();
    //        Dictionary<int, NJFRightTransferInfo> transfersByPrimaryDocFrom = new Dictionary<int, NJFRightTransferInfo>();
    //        Dictionary<int, NJFRightTransferInfo> transfersByPrimaryDocTo = new Dictionary<int, NJFRightTransferInfo>();
    //        List<NJFRightTransferInfo> unprocessedRows = new List<NJFRightTransferInfo>();

    //        querySelect = "SELECT ID FROM OBJECT_PRAVO ORDER BY ID"; // WHERE OBJECT_KODSTAN = 1

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);

    //                        if (dataId is int)
    //                        {
    //                            int transferId = (int)dataId;
    //                            NJFRightTransferInfo info = null;
    //                            NJFRightTransferInfo infoRelated = null;
    //                            NJFRightTransferInfo infoFrom = null;
    //                            NJFRightTransferInfo infoTo = null;

    //                            if (rightCache.TryGetValue(transferId, out info))
    //                            {
    //                                // Skip rows that can be matched to another rows
    //                                if (!closingCards.Contains(transferId) &&
    //                                    !closedCards.Contains(transferId))
    //                                {
    //                                    // Try to find the matching row
    //                                    if (info.closingCard is int)
    //                                    {
    //                                        if (rightCache.TryGetValue((int)info.closingCard, out infoRelated))
    //                                        {
    //                                            infoFrom = info;
    //                                            infoTo = infoRelated;
    //                                        }
    //                                    }
    //                                    else if (info.closedCard is int)
    //                                    {
    //                                        if (rightCache.TryGetValue((int)info.closedCard, out infoRelated))
    //                                        {
    //                                            infoFrom = infoRelated;
    //                                            infoTo = info;
    //                                        }
    //                                    }

    //                                    if (infoFrom != null && infoTo != null)
    //                                    {
    //                                        AddNJFRightsTransfer(infoFrom, infoTo, rightPropCache, values, null, false);

    //                                        // Save this transfer by the primary document
    //                                        if (infoFrom.primaryDocId is int)
    //                                        {
    //                                            transfersByPrimaryDocFrom[(int)infoFrom.primaryDocId] = infoFrom;
    //                                            transfersByPrimaryDocTo[(int)infoFrom.primaryDocId] = infoTo;
    //                                        }
    //                                        else if (infoTo.primaryDocId is int)
    //                                        {
    //                                            transfersByPrimaryDocFrom[(int)infoTo.primaryDocId] = infoFrom;
    //                                            transfersByPrimaryDocTo[(int)infoTo.primaryDocId] = infoTo;
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        // This row may have a special binding; save this row for future processing
    //                                        if (info.primaryDocId is int)
    //                                        {
    //                                            unprocessedRows.Add(info);
    //                                        }
    //                                    }
    //                                }
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }

    //        // Process the rows that could not be processed
    //        int numUnprocessedRows = 0;

    //        for (int n = 0; n < unprocessedRows.Count; n++)
    //        {
    //            NJFRightTransferInfo info = unprocessedRows[n];

    //            // Try to find the matching row using the "primary document" link
    //            if (info.primaryDocId is int)
    //            {
    //                int primaryDocId = (int)info.primaryDocId;
    //                NJFRightTransferInfo match = null;

    //                if (info.addDocId is int)
    //                {
    //                    if (transfersByPrimaryDocFrom.TryGetValue(primaryDocId, out match))
    //                    {
    //                        AddNJFRightsTransfer(match, info, rightPropCache, values, null, false);
    //                    }
    //                }
    //                else if (info.removeDocId is int)
    //                {
    //                    if (transfersByPrimaryDocTo.TryGetValue(primaryDocId, out match))
    //                    {
    //                        AddNJFRightsTransfer(info, match, rightPropCache, values, null, false);
    //                    }
    //                }

    //                if (match == null)
    //                {
    //                    // Try to use information from the PRAVA_PROECT table for this row
    //                    if (!PerformSpecialRightMatch(info, rightPropCache, values))
    //                    {
    //                        numUnprocessedRows++;
    //                    }
    //                }
    //            }
    //        }

    //        logger.WriteInfo("Could not match " + numUnprocessedRows + " object transfer records");
    //    }

    //    private void AddNJFRightsTransfer(NJFRightTransferInfo from, NJFRightTransferInfo to,
    //        Dictionary<int, NJFRightMiscInfo> rightPropCache, Dictionary<string, object> values,
    //        NJFObjectDocsProperties objProperties, bool processingRozpWithoutActs)
    //    {
    //        // Map IDs of organizations, objects and documents to the 1 NF entities
    //        int objectId = (to.objectId is int) ? (int)to.objectId : (int)from.objectId;

    //        object organizationIdFrom = from.organizationId;
    //        object organizationIdTo = to.organizationId;

    //        object rightId = (to.rightId != null) ? to.rightId : from.rightId;
    //        object modifiedBy = (to.modifiedBy != null) ? to.modifiedBy : from.modifiedBy;
    //        object modifyDate = (to.modifyDate != null) ? to.modifyDate : from.modifyDate;

    //        object docId = (to.addDocId != null) ? to.addDocId : to.removeDocId;

    //        if (docId == null)
    //        {
    //            docId = (from.addDocId != null) ? from.addDocId : from.removeDocId;
    //        }

    //        object docDate = (to.addDocDate != null) ? to.addDocDate : to.removeDocDate;

    //        if (docDate == null)
    //        {
    //            docDate = (from.addDocDate != null) ? from.addDocDate : from.removeDocDate;
    //        }

    //        // If the document date is not found, try to use our cache
    //        if (docDate == null && docId is int)
    //        {
    //            DateTime dt;

    //            if (rozpDatesNJF.TryGetValue((int)docId, out dt))
    //            {
    //                docDate = dt;
    //            }
    //            else if (aktDatesNJF.TryGetValue((int)docId, out dt))
    //            {
    //                docDate = dt;
    //            }
    //        }

    //        // Determine the document type (Akt or Rozporadjennia)
    //        object aktId = null;
    //        object rozpDocId = null;

    //        if (docId is int)
    //        {
    //            if (rozpDatesNJF.ContainsKey((int)docId))
    //            {
    //                rozpDocId = docId;
    //                aktId = FindNJFAktByRozpDoc((int)docId, objectId); // There may be many Akts in a single Rozporadjennia
    //            }
    //            else if (aktDatesNJF.ContainsKey((int)docId))
    //            {
    //                aktId = docId;
    //                rozpDocId = FindNJFRozpDocByAct((int)docId);
    //            }
    //        }

    //        // Get additional information from the OBJECT_PRAVO_PROPERTIES table
    //        object objectName = null;
    //        object characteristics = null;
    //        object miscInfo = null;
    //        object sumBalans = null;
    //        object sumZalishkova = null;
    //        object square = null;
    //        object length = null;

    //        if (objProperties != null)
    //        {
    //            objectName = objProperties.objectName;
    //            characteristics = objProperties.characteristics;
    //            miscInfo = objProperties.miscInfo;
    //            sumBalans = objProperties.sumBalans;
    //            sumZalishkova = objProperties.sumZalishkova;
    //            square = objProperties.square;
    //            length = objProperties.length;
    //            modifiedBy = objProperties.modifiedBy;
    //            modifyDate = objProperties.modifyDate;
    //        }
    //        else
    //        {
    //            NJFRightMiscInfo info = null;

    //            if (rightPropCache.TryGetValue(to.transferId, out info))
    //            {
    //                objectName = info.objectName;
    //                characteristics = info.characteristics;
    //                miscInfo = info.miscInfo;
    //                sumBalans = info.sumBalans;
    //                sumZalishkova = info.sumZalishkova;
    //                square = info.square;
    //                length = info.length;
    //            }

    //            if (rightPropCache.TryGetValue(from.transferId, out info))
    //            {
    //                if (objectName == null) objectName = info.objectName;
    //                if (characteristics == null) characteristics = info.characteristics;
    //                if (miscInfo == null) miscInfo = info.miscInfo;
    //                if (sumBalans == null) sumBalans = info.sumBalans;
    //                if (sumZalishkova == null) sumZalishkova = info.sumZalishkova;
    //                if (square == null) square = info.square;
    //                if (length == null) length = info.length;
    //            }
    //        }

    //        // Perform mapping of IDs
    //        int buildingId1NF = 0;
    //        int orgFromId1NF = 0;
    //        int orgToId1NF = 0;
    //        int docId1NF = 0;

    //        if (!objIdMappingNJFto1NF.TryGetValue(objectId, out buildingId1NF))
    //        {
    //            logger.WriteError("Object " + objectId.ToString() + " was not mapped correctly!");
    //            return;
    //        }

    //        if (organizationIdFrom is int)
    //        {
    //            if (orgIdMappingNJFto1NF.TryGetValue((int)organizationIdFrom, out orgFromId1NF))
    //            {
    //                organizationIdFrom = orgFromId1NF;
    //            }
    //            else
    //            {
    //                if (!unmappedNJFOrganizations.Contains((int)organizationIdFrom))
    //                {
    //                    unmappedNJFOrganizations.Add((int)organizationIdFrom);
    //                    logger.WriteInfo("Organization " + organizationIdFrom.ToString() + " was not mapped correctly");
    //                }

    //                organizationIdFrom = null;
    //            }
    //        }

    //        if (organizationIdTo is int)
    //        {
    //            if (orgIdMappingNJFto1NF.TryGetValue((int)organizationIdTo, out orgToId1NF))
    //            {
    //                organizationIdTo = orgToId1NF;
    //            }
    //            else
    //            {
    //                if (!unmappedNJFOrganizations.Contains((int)organizationIdTo))
    //                {
    //                    unmappedNJFOrganizations.Add((int)organizationIdTo);
    //                    logger.WriteInfo("Organization " + organizationIdTo.ToString() + " was not mapped correctly");
    //                }

    //                organizationIdTo = null;
    //            }
    //        }

    //        if (docId is int)
    //        {
    //            if (docIdMappingNJFto1NF.TryGetValue((int)docId, out docId1NF))
    //            {
    //                docId = docId1NF;
    //            }
    //            else
    //            {
    //                logger.WriteError("Document " + docId.ToString() + " was not mapped correctly!");
    //                return;
    //            }
    //        }

    //        if (rozpDocId is int)
    //        {
    //            if (docIdMappingNJFto1NF.TryGetValue((int)rozpDocId, out docId1NF))
    //            {
    //                rozpDocId = docId1NF;
    //            }
    //            else
    //            {
    //                logger.WriteError("Document (ROZP_DOK) " + rozpDocId.ToString() + " was not mapped correctly!");
    //                return;
    //            }
    //        }

    //        if (aktId is int)
    //        {
    //            if (docIdMappingNJFto1NF.TryGetValue((int)aktId, out docId1NF))
    //            {
    //                aktId = docId1NF;
    //            }
    //            else
    //            {
    //                logger.WriteError("Document (AKTS) " + aktId.ToString() + " was not mapped correctly!");
    //                return;
    //            }
    //        }

    //        // If the object-to-rozporadjennia relation was already processed, no need to do it twice
    //        if (rozpDocId is int)
    //        {
    //            string rozpToObjectKey = rozpDocId.ToString() + "|" + buildingId1NF.ToString();

    //            if (!njfRozpAndObjectsRelationCache.Add(rozpToObjectKey) && processingRozpWithoutActs)
    //            {
    //                return;
    //            }
    //        }

    //        // Check if this transfer was already added
    //        string key = GetNJFRightTransferKey(buildingId1NF, organizationIdFrom, organizationIdTo,
    //            rightId, rozpDocId, aktId,  objectName);

    //        bool keyExists = existingNJFObjRightTransfers.Contains(key);

    //        if (key.Length == 0 || !keyExists)
    //        {
    //            // Save the valid keys, to avoid adding duplicate transfers
    //            if (key.Length > 0)
    //            {
    //                existingNJFObjRightTransfers.Add(key);
    //            }

    //            // Generate an INSERT statement
    //            /*
    //            values.Clear();

    //            values.Add("building_id", buildingId1NF);

    //            if (organizationIdFrom is int)
    //            {
    //                values.Add("org_from_id", organizationIdFrom);
    //            }

    //            if (organizationIdTo is int)
    //            {
    //                values.Add("org_to_id", organizationIdTo);
    //            }

    //            values.Add("right_id", rightId);
    //            values.Add("akt_id", aktId);
    //            values.Add("rozp_id", rozpDocId);
    //            values.Add("transfer_date", docDate);
    //            values.Add("modified_by", modifiedBy);
    //            values.Add("modify_date", modifyDate);
    //            values.Add("name", objectName);
    //            values.Add("characteristic", characteristics);
    //            values.Add("misc_info", miscInfo);
    //            values.Add("sum_balans", sumBalans);
    //            values.Add("sum_zalishkova", sumZalishkova);
    //            values.Add("sqr_transferred", square);
    //            values.Add("len_transferred", length);

    //            string queryInsert = GetInsertStatement("object_rights", values);

    //            // Execute the INSERT statement
    //            try
    //            {
    //                using (SqlCommand command = new SqlCommand(queryInsert, connectionSqlClient))
    //                {
    //                    command.ExecuteNonQuery();
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                ShowSqlErrorMessageDlg(queryInsert, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //            */

    //            tableNjfRightsRow[0] = buildingId1NF;
    //            tableNjfRightsRow[1] = organizationIdFrom;
    //            tableNjfRightsRow[2] = organizationIdTo;
    //            tableNjfRightsRow[3] = rightId;
    //            tableNjfRightsRow[4] = aktId;
    //            tableNjfRightsRow[5] = rozpDocId;
    //            tableNjfRightsRow[6] = docDate;
    //            tableNjfRightsRow[7] = modifiedBy;
    //            tableNjfRightsRow[8] = modifyDate;
    //            tableNjfRightsRow[9] = objectName;
    //            tableNjfRightsRow[10] = characteristics;
    //            tableNjfRightsRow[11] = miscInfo;
    //            tableNjfRightsRow[12] = sumBalans;
    //            tableNjfRightsRow[13] = sumZalishkova;
    //            tableNjfRightsRow[14] = square;
    //            tableNjfRightsRow[15] = length;

    //            tableNjfRights.Rows.Add(tableNjfRightsRow);
    //        }
    //    }

    //    private bool PerformSpecialRightMatch(NJFRightTransferInfo info,
    //        Dictionary<int, NJFRightMiscInfo> rightPropCache, Dictionary<string, object> values)
    //    {
    //        if (info.objectId is int && info.addDocId is int)
    //        {
    //            string key = GetNjfObjToDocRelationKey((int)info.objectId, (int)info.addDocId);

    //            HashSet<int> objectDocsIds = new HashSet<int>();
    //            int objectDocsId = -1;

    //            if (njfReverseCacheObjectDocs.TryGetValue(key, out objectDocsId))
    //            {
    //                objectDocsIds.Add(objectDocsId);
    //            }

    //            // Perform the same search for all related documents
    //            object relatedDocs = null;

    //            if (njfDocRelationCache.TryGetValue((int)info.addDocId, out relatedDocs))
    //            {
    //                if (relatedDocs is int)
    //                {
    //                    key = GetNjfObjToDocRelationKey((int)info.objectId, (int)relatedDocs);

    //                    if (njfReverseCacheObjectDocs.TryGetValue(key, out objectDocsId))
    //                    {
    //                        objectDocsIds.Add(objectDocsId);
    //                    }
    //                }
    //                else if (relatedDocs is List<int>)
    //                {
    //                    foreach (int relDocId in (relatedDocs as List<int>))
    //                    {
    //                        key = GetNjfObjToDocRelationKey((int)info.objectId, relDocId);

    //                        if (njfReverseCacheObjectDocs.TryGetValue(key, out objectDocsId))
    //                        {
    //                            objectDocsIds.Add(objectDocsId);
    //                        }
    //                    }
    //                }
    //            }

    //            foreach (int objDocsId in objectDocsIds)
    //            {
    //                object value = null;

    //                if (njfSpecialRightsCache.TryGetValue(objDocsId, out value))
    //                {
    //                    List<NJFSpecialRightInfo> list = null;

    //                    if (value is List<NJFSpecialRightInfo>)
    //                    {
    //                        list = value as List<NJFSpecialRightInfo>;
    //                    }
    //                    else
    //                    {
    //                        list = new List<NJFSpecialRightInfo>();

    //                        list.Add((NJFSpecialRightInfo)value);
    //                    }

    //                    if (info.removeDocId == null)
    //                    {
    //                        info.removeDocId = info.addDocId;
    //                        info.removeDocDate = info.addDocDate;
    //                    }

    //                    if (info.addDocId == null)
    //                    {
    //                        info.addDocId = info.removeDocId;
    //                        info.addDocDate = info.removeDocDate;
    //                    }

    //                    NJFRightTransferInfo infoFrom = new NJFRightTransferInfo(info);
    //                    NJFRightTransferInfo infoTo = new NJFRightTransferInfo(info);

    //                    foreach (NJFSpecialRightInfo specialRight in list)
    //                    {
    //                        infoFrom.organizationId = specialRight.organizationFromId;
    //                        infoFrom.rightId = specialRight.rightId;

    //                        infoTo.organizationId = specialRight.organizationToId;
    //                        infoTo.rightId = specialRight.rightId;

    //                        AddNJFRightsTransfer(infoFrom, infoTo, rightPropCache, values, null, false);
    //                    }

    //                    return true;
    //                }
    //            }
    //        }

    //        return false;
    //    }

    //    private string GetNJFRightTransferKey(int buildingId1NF,
    //        object organizationIdFrom,
    //        object organizationIdTo,
    //        object rightId,
    //        object rozpDocId,
    //        object aktId,
    //        object objectName)
    //    {
    //        if (organizationIdFrom is int &&
    //            organizationIdTo is int &&
    //            rightId is int &&
    //            (rozpDocId is int || aktId is int) &&
    //            objectName is string)
    //        {
    //            if (rozpDocId is int)
    //            {
    //                return
    //                    "@FROM@" + (organizationIdFrom is int ? organizationIdFrom.ToString() : "NULL") +
    //                    "@TO@" + (organizationIdTo is int ? organizationIdTo.ToString() : "NULL") +
    //                    "@RIGHT@" + rightId.ToString() +
    //                    "@ROZPDOC@" + (rozpDocId is int ? rozpDocId.ToString() : "NULL") +
    //                    "@BUILDING@" + buildingId1NF.ToString() +
    //                    "@NAME@" + ((string)objectName).Trim().ToUpper();
    //            }
    //            else
    //            {
    //                return
    //                    "@FROM@" + (organizationIdFrom is int ? organizationIdFrom.ToString() : "NULL") +
    //                    "@TO@" + (organizationIdTo is int ? organizationIdTo.ToString() : "NULL") +
    //                    "@RIGHT@" + rightId.ToString() +
    //                    "@AKT@" + (aktId is int ? aktId.ToString() : "NULL") +
    //                    "@BUILDING@" + buildingId1NF.ToString() +
    //                    "@NAME@" + ((string)objectName).Trim().ToUpper();
    //            }
    //        }

    //        return "";
    //    }

    //    private void MigrateNJFDocRights()
    //    {
    //        logger.WriteInfo("Migrating object rights from the PRAVA_PROECT table");

    //        Dictionary<string, object> values = new Dictionary<string, object>();

    //        string querySelect = "SELECT ID, RD, FROM_ORG, TOORG, PRAVO FROM PRAVA_PROECT";

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    NJFRightTransferInfo from = new NJFRightTransferInfo();
    //                    NJFRightTransferInfo to = new NJFRightTransferInfo();

    //                    while (reader.Read())
    //                    {
    //                        // Get the document data
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataObjectDocsId = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataOrgFrom = reader.IsDBNull(2) ? null : reader.GetValue(2);
    //                        object dataOrgTo = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                        object dataPravo = reader.IsDBNull(4) ? null : reader.GetValue(4);

    //                        if (dataId is int && dataObjectDocsId is int && (dataOrgFrom is int || dataOrgTo is int))
    //                        {
    //                            NJFObjectDocsProperties objectDokProperties = null;

    //                            if (njfObjectDoksProperties.TryGetValue((int)dataObjectDocsId, out objectDokProperties))
    //                            {
    //                                from.removeDocId = objectDokProperties.documentId;
    //                                from.rightId = dataPravo;
    //                                from.organizationId = dataOrgFrom;
    //                                from.objectId = objectDokProperties.objectId;

    //                                to.addDocId = objectDokProperties.documentId;
    //                                to.rightId = dataPravo;
    //                                to.organizationId = dataOrgTo;
    //                                to.objectId = objectDokProperties.objectId;

    //                                AddNJFRightsTransfer(from, to, null, values, objectDokProperties, true);
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void MigrateNJFDocLinks()
    //    {
    //        logger.WriteInfo("Migrating document links from the table DOC_DEPEND");

    //        Dictionary<string, object> values = new Dictionary<string, object>();

    //        // Prepare the SQL statement to get all the documents
    //        string querySelect = "SELECT DOK_KOD1, DOK_KOD2, KIND, DT, ISP FROM DOK_DEPEND";

    //        // Get all the document links
    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the document data
    //                        object dataChildId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataParentId = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataLinkKind = reader.IsDBNull(2) ? null : reader.GetValue(2);
    //                        object dataModifyDate = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                        object dataModifiedBy = reader.IsDBNull(4) ? null : reader.GetValue(4);

    //                        if (dataParentId is int && dataChildId is int)
    //                        {
    //                            int parentDocId1NF = 0;
    //                            int childDocId1NF = 0;

    //                            if (docIdMappingNJFto1NF.TryGetValue((int)dataParentId, out parentDocId1NF) &&
    //                                docIdMappingNJFto1NF.TryGetValue((int)dataChildId, out childDocId1NF))
    //                            {
    //                                if (!docFinder.DocLinkToDocExistsInSQLServer(parentDocId1NF, childDocId1NF))
    //                                {
    //                                    docFinder.AddDocLinkToDoc(connectionSqlClient,
    //                                        parentDocId1NF, childDocId1NF, dataModifiedBy, dataModifyDate, dataLinkKind);
    //                                }
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void MigrateNJFDocRestrictions()
    //    {
    //        ///////////////////////////////////////////////////////////////////
    //        // 1) Migrate the required dictionaries
    //        ///////////////////////////////////////////////////////////////////

    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        // SGRRESTRICTIONS
    //        fields.Clear();
    //        fields.Add("KOD", "id");
    //        fields.Add("NAME", "name");

    //        MigrateTable(connectionNJF, "SGRRESTRICTIONS", "dict_restriction_group", fields, "KOD", null);

    //        // SRESTRICTION
    //        fields.Clear();
    //        fields.Add("KOD", "id");
    //        fields.Add("NAME", "name");
    //        fields.Add("GR", "group_id");

    //        MigrateTable(connectionNJF, "SRESTRICTION", "dict_restriction", fields, "KOD", null);

    //        ///////////////////////////////////////////////////////////////////
    //        // 2) Migrate restrictions from the RESTRICTIONS table
    //        ///////////////////////////////////////////////////////////////////

    //        logger.WriteInfo("Migrating document restrictions from the table RESTRICTIONS");

    //        Dictionary<string, object> values = new Dictionary<string, object>();

    //        // Prepare the SQL statement to get all the documents
    //        string querySelect = "SELECT DOK_ID, GR, RESTRICTION, DT, ISP FROM RESTRICTIONS";

    //        // Get all the document restrictions
    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the document data
    //                        object dataDocId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataRestrGroupId = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataRestrictionId = reader.IsDBNull(2) ? null : reader.GetValue(2);
    //                        object dataModifyDate = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                        object dataModifiedBy = reader.IsDBNull(4) ? null : reader.GetValue(4);

    //                        if (dataDocId is int && dataRestrGroupId is int && dataRestrictionId is int)
    //                        {
    //                            int docId1NF = 0;

    //                            if (docIdMappingNJFto1NF.TryGetValue((int)dataDocId, out docId1NF))
    //                            {
    //                                // Prepare values for inserting this document into SQL Server table
    //                                values["document_id"] = docId1NF;
    //                                values["restriction_id"] = dataRestrictionId;
    //                                values["restr_group_id"] = dataRestrGroupId;
    //                                values["modified_by"] = dataModifiedBy;
    //                                values["modify_date"] = dataModifyDate;

    //                                // Format the INSERT statement for adding this document to our database
    //                                string queryInsert = GetInsertStatement("restrictions", values);

    //                                // Execute the INSERT statement
    //                                try
    //                                {
    //                                    using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionSqlClient))
    //                                    {
    //                                        commandInsert.ExecuteNonQuery();
    //                                    }
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    ShowSqlErrorMessageDlg(queryInsert, ex.Message);

    //                                    // Abort migration correctly
    //                                    throw new MigrationAbortedException();
    //                                }
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void MigrateNJFObjectToDocLinks()
    //    {
    //        logger.WriteInfo("Migrating links between objects and documents from the table OBJECT_DOKS_PROPERTIES");

    //        // Create field mapping for the table OBJECT_DOKS_PROPERTIES
    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        fields.Add("OBJECT_KOD", "building_id");
    //        fields.Add("DOK_ID", "document_id");
    //        fields.Add("NAME", "obj_name");
    //        fields.Add("CHARACTERISTIC", "obj_description");
    //        fields.Add("YEAR_BUILD", "obj_build_year");

    //        fields.Add("YEAR_EXPL", "obj_expl_enter_year");
    //        fields.Add("LEN", "obj_length");
    //        fields.Add("SUMMA_BALANS", "cost_balans");
    //        fields.Add("SUMMA_ZNOS", "cost_znos");
    //        fields.Add("SUMMA_ZAL", "cost_zalishkova");

    //        fields.Add("ROOM_COUNT", "num_rooms");
    //        fields.Add("FLOORS", "num_floors");
    //        fields.Add("ARRANGEMENT", "obj_location");
    //        fields.Add("SQUARE", "sqr_obj");
    //        fields.Add("CLR_SQUARE", "sqr_free");

    //        fields.Add("LSQUARE", "sqr_habit");
    //        fields.Add("NLSQARE", "sqr_non_habit");
    //        //fields.Add("WORKED", "is_not_in_work");
    //        fields.Add("ISP", "modified_by");

    //        fields.Add("DT", "modify_date");
    //        fields.Add("POS", "doc_pos");
    //        fields.Add("DIAM_TRUB", "pipe_diameter");
    //        fields.Add("MAT_TRUB", "pipe_material");

    //        fields.Add("GRPURP", "purpose_group_id");
    //        fields.Add("PURPOSE", "purpose_id");
    //        fields.Add("TEXSTAN", "tech_condition_id");
    //        fields.Add("OBJKIND", "object_kind_id");
    //        fields.Add("OBJTYPE", "object_type_id");

    //        if (!testOnly)
    //        {
    //            fields.Add("DESCRIPTION", "note");
    //        }

    //        int purposeGroupIndex = GetSrcFieldIndexFromMapping(fields, "GRPURP", "OBJECT_DOKS_PROPERTIES");
    //        int purposeIndex = GetSrcFieldIndexFromMapping(fields, "PURPOSE", "OBJECT_DOKS_PROPERTIES");
    //        int techStateIndex = GetSrcFieldIndexFromMapping(fields, "TEXSTAN", "OBJECT_DOKS_PROPERTIES");
    //        int objKindIndex = GetSrcFieldIndexFromMapping(fields, "OBJKIND", "OBJECT_DOKS_PROPERTIES");
    //        int objTypeIndex = GetSrcFieldIndexFromMapping(fields, "OBJTYPE", "OBJECT_DOKS_PROPERTIES");

    //        // Select all records
    //        // Dictionary<string, object> dataOverride = new Dictionary<string, object>();

    //        string querySelect = GetSelectStatement("OBJECT_DOKS_PROPERTIES", fields, "ID", null);

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the document data
    //                        object dataObjectId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataDocId = reader.IsDBNull(1) ? null : reader.GetValue(1);

    //                        object dataPurposeGroup = reader.IsDBNull(purposeGroupIndex) ? null : reader.GetValue(purposeGroupIndex);
    //                        object dataPurpose = reader.IsDBNull(purposeIndex) ? null : reader.GetValue(purposeIndex);
    //                        object dataTechState = reader.IsDBNull(techStateIndex) ? null : reader.GetValue(techStateIndex);
    //                        object dataObjKind = reader.IsDBNull(objKindIndex) ? null : reader.GetValue(objKindIndex);
    //                        object dataObjType = reader.IsDBNull(objTypeIndex) ? null : reader.GetValue(objTypeIndex);

    //                        if (dataObjectId is int && dataDocId is int)
    //                        {
    //                            int objectId1NF = 0;
    //                            int docId1NF = 0;

    //                            if (objIdMappingNJFto1NF.TryGetValue((int)dataObjectId, out objectId1NF) &&
    //                                docIdMappingNJFto1NF.TryGetValue((int)dataDocId, out docId1NF))
    //                            {
    //                                // Prepare the array of objects for inserting into DataTable
    //                                for (int i = 0; i < reader.FieldCount; i++)
    //                                {
    //                                    tableNjfBuildingDocsRow[i] = reader.IsDBNull(i) ? null : reader.GetValue(i);
    //                                }

    //                                // Prepare the data override
    //                                // dataOverride.Clear();
    //                                // dataOverride.Add("building_id", objectId1NF);
    //                                // dataOverride.Add("document_id", docId1NF);
    //                                tableNjfBuildingDocsRow[0] = objectId1NF;
    //                                tableNjfBuildingDocsRow[1] = docId1NF;

    //                                if (dataPurposeGroup is int)
    //                                {
    //                                    if (mappingObjPurposeGroupNJFto1NF.ContainsKey((int)dataPurposeGroup))
    //                                    {
    //                                        // dataOverride.Add("purpose_group_id", mappingObjPurposeGroupNJFto1NF[(int)dataPurposeGroup]);
    //                                        tableNjfBuildingDocsRow[purposeGroupIndex] = mappingObjPurposeGroupNJFto1NF[(int)dataPurposeGroup];
    //                                    }
    //                                }

    //                                if (dataPurpose is int)
    //                                {
    //                                    if (mappingObjPurposeNJFto1NF.ContainsKey((int)dataPurpose))
    //                                    {
    //                                        // dataOverride.Add("purpose_id", mappingObjPurposeNJFto1NF[(int)dataPurpose]);
    //                                        tableNjfBuildingDocsRow[purposeIndex] = mappingObjPurposeNJFto1NF[(int)dataPurpose];
    //                                    }
    //                                }

    //                                if (dataTechState is int)
    //                                {
    //                                    if (mappingTechStateNJFto1NF.ContainsKey((int)dataTechState))
    //                                    {
    //                                        // dataOverride.Add("tech_condition_id", mappingTechStateNJFto1NF[(int)dataTechState]);
    //                                        tableNjfBuildingDocsRow[techStateIndex] = mappingTechStateNJFto1NF[(int)dataTechState];
    //                                    }
    //                                }

    //                                if (dataObjKind is int)
    //                                {
    //                                    if (mappingObjKindNJFto1NF.ContainsKey((int)dataObjKind))
    //                                    {
    //                                        // dataOverride.Add("object_kind_id", mappingObjKindNJFto1NF[(int)dataObjKind]);
    //                                        tableNjfBuildingDocsRow[objKindIndex] = mappingObjKindNJFto1NF[(int)dataObjKind];
    //                                    }
    //                                }

    //                                if (dataObjType is int)
    //                                {
    //                                    if (mappingObjTypeNJFto1NF.ContainsKey((int)dataObjType))
    //                                    {
    //                                        // dataOverride.Add("object_type_id", mappingObjTypeNJFto1NF[(int)dataObjType]);
    //                                        tableNjfBuildingDocsRow[objTypeIndex] = mappingObjTypeNJFto1NF[(int)dataObjType];
    //                                    }
    //                                }

    //                                tableNjfBuildingDocs.Rows.Add(tableNjfBuildingDocsRow);
    //                                tableNjfObjRightsBuildingDocs.Rows.Add(tableNjfBuildingDocsRow);

    //                                /*
    //                                // Create the new link using INSERT
    //                                string queryInsert = GetInsertStatement("object_rights_building_docs", fields, reader, dataOverride);

    //                                try
    //                                {
    //                                    using (SqlCommand command = new SqlCommand(queryInsert, connectionSqlClient))
    //                                    {
    //                                        command.ExecuteNonQuery();
    //                                    }
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    ShowSqlErrorMessageDlg(queryInsert, ex.Message);

    //                                    // Abort migration correctly
    //                                    throw new MigrationAbortedException();
    //                                }
    //                                */

    //                                // Add document-to-object link to the primary table as well (building_docs)
    //                                if (true /*!docFinder.DocLinkToObjExistsInSQLServer(objectId1NF, docId1NF)*/)
    //                                {
    //                                    /*
    //                                    queryInsert = GetInsertStatement("building_docs", fields, reader, dataOverride);

    //                                    try
    //                                    {
    //                                        using (SqlCommand command = new SqlCommand(queryInsert, connectionSqlClient))
    //                                        {
    //                                            command.ExecuteNonQuery();
    //                                        }
    //                                    }
    //                                    catch (Exception ex)
    //                                    {
    //                                        ShowSqlErrorMessageDlg(queryInsert, ex.Message);

    //                                        // Abort migration correctly
    //                                        throw new MigrationAbortedException();
    //                                    }
    //                                    */

    //                                    // Register the link for future lookup
    //                                    // docFinder.RegisterDocLinkToObject(objectId1NF, docId1NF);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            logger.WriteInfo("Object " + dataObjectId.ToString() + " was not mapped correctly");
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void PrepareNJFDocKindMapping()
    //    {
    //        string docKindRozpKyivRadi = Properties.Resources.DocKindRozpKyivRadi;
    //        string docKindNakazMinEnergetiki = Properties.Resources.DocKindNakazMinEnergetiki;
    //        string docKindRozpRadyanskAdmin = Properties.Resources.DocKindRozpRadyanskAdmin;
    //        string docKindRozpZaliznAdmin = Properties.Resources.DocKindRozpZaliznAdmin;
    //        string docKindRozpMoskAdmin = Properties.Resources.DocKindRozpMoskAdmin;

    //        // Add the missing document types
    //        int idRozpKyivRadi = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docKindRozpKyivRadi);

    //        if (idRozpKyivRadi < 0)
    //        {
    //            idRozpKyivRadi = 14;

    //            AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", idRozpKyivRadi, docKindRozpKyivRadi);
    //        }

    //        int idNakazMinEnergetiki = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docKindNakazMinEnergetiki);

    //        if (idNakazMinEnergetiki < 0)
    //        {
    //            idNakazMinEnergetiki = 53;

    //            AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", idNakazMinEnergetiki, docKindNakazMinEnergetiki);
    //        }

    //        if (FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docKindRozpRadyanskAdmin) < 0)
    //        {
    //            AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", 24, docKindRozpRadyanskAdmin);
    //        }

    //        if (FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docKindRozpZaliznAdmin) < 0)
    //        {
    //            AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", 26, docKindRozpZaliznAdmin);
    //        }

    //        if (FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docKindRozpMoskAdmin) < 0)
    //        {
    //            AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", 32, docKindRozpMoskAdmin);
    //        }

    //        mappingDocKindNJFto1NF.Add(7, idRozpKyivRadi);
    //        mappingDocKindNJFto1NF.Add(33, 35);
    //        mappingDocKindNJFto1NF.Add(35, idNakazMinEnergetiki);
    //        mappingDocKindNJFto1NF.Add(38, 106);
    //        mappingDocKindNJFto1NF.Add(39, 100);
    //        mappingDocKindNJFto1NF.Add(40, 48);
    //    }

    //    private void PrepareNJFObjPurposeGroupMapping()
    //    {
    //        string purposeGroupNezavershene = Properties.Resources.NJFPurposeGroupNezavershene;
    //        string purposeGroupNezhili = Properties.Resources.NJFPurposeGroupNezhili;
    //        string purposeGroupNezhitlovi = Properties.Resources.NJFPurposeGroupNezhitlovi;
    //        string purposeGroupPidpr = Properties.Resources.NJFPurposeGroupPidpr;
    //        string purposeGroupComplex = Properties.Resources.NJFPurposeGroupComplex;

    //        // Add the missing purpose groups
    //        int idNezavershene = FindDictionaryEntryIdInSQLServer("dict_balans_purpose_group", "id", "name", purposeGroupNezavershene);

    //        if (idNezavershene < 0)
    //        {
    //            idNezavershene = 32;

    //            AddDictionaryEntryInSQLServer("dict_balans_purpose_group", "id", "name", idNezavershene, purposeGroupNezavershene);
    //        }

    //        int idNezhili = FindDictionaryEntryIdInSQLServer("dict_balans_purpose_group", "id", "name", purposeGroupNezhili);

    //        if (idNezhili < 0)
    //        {
    //            idNezhili = 33;

    //            AddDictionaryEntryInSQLServer("dict_balans_purpose_group", "id", "name", idNezhili, purposeGroupNezhili);
    //        }

    //        int idNezhitlovi = FindDictionaryEntryIdInSQLServer("dict_balans_purpose_group", "id", "name", purposeGroupNezhitlovi);

    //        if (idNezhitlovi < 0)
    //        {
    //            idNezhitlovi = 34;

    //            AddDictionaryEntryInSQLServer("dict_balans_purpose_group", "id", "name", idNezhitlovi, purposeGroupNezhitlovi);
    //        }

    //        int idPidpr = FindDictionaryEntryIdInSQLServer("dict_balans_purpose_group", "id", "name", purposeGroupPidpr);

    //        if (idPidpr < 0)
    //        {
    //            idPidpr = 35;

    //            AddDictionaryEntryInSQLServer("dict_balans_purpose_group", "id", "name", idPidpr, purposeGroupPidpr);
    //        }

    //        int idComplex = FindDictionaryEntryIdInSQLServer("dict_balans_purpose_group", "id", "name", purposeGroupComplex);

    //        if (idComplex < 0)
    //        {
    //            idComplex = 36;

    //            AddDictionaryEntryInSQLServer("dict_balans_purpose_group", "id", "name", idComplex, purposeGroupComplex);
    //        }

    //        mappingObjPurposeGroupNJFto1NF.Add(30, 32);
    //        mappingObjPurposeGroupNJFto1NF.Add(31, 33);
    //        mappingObjPurposeGroupNJFto1NF.Add(32, 34);
    //        mappingObjPurposeGroupNJFto1NF.Add(33, 35);
    //        mappingObjPurposeGroupNJFto1NF.Add(34, 36);
    //    }

    //    private void PrepareNJFObjPurposeMapping()
    //    {
    //        Dictionary<string, object> values = new Dictionary<string,object>();

    //        // Get the entire dictionary from the SPURPOSE table
    //        string query = "SELECT KOD, GR, NAME FROM SPURPOSE";

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(query, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataGroupId = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataName = reader.IsDBNull(2) ? null : reader.GetValue(2);

    //                        if (dataId is int && dataGroupId is int && dataName is string)
    //                        {
    //                            int purposeId = (int)dataId;
    //                            int groupId = (int)dataGroupId;
    //                            string purpose = (string)dataName;

    //                            // Map the purpose group
    //                            if (mappingObjPurposeGroupNJFto1NF.ContainsKey(groupId))
    //                            {
    //                                groupId = mappingObjPurposeGroupNJFto1NF[groupId];
    //                            }

    //                            // Try to find a purpose with the same name and group in the existing database
    //                            query = "SELECT TOP 1 id FROM dict_balans_purpose WHERE " + 
    //                                "group_code = " + groupId.ToString() + " AND " +
    //                                "name = '" + FormatStringForSQLServer(purpose) + "'";

    //                            int existingPurposeId = -1;

    //                            using (SqlCommand command = new SqlCommand(query, connectionSqlClient))
    //                            {
    //                                using (SqlDataReader readerPurpose = command.ExecuteReader())
    //                                {
    //                                    if (readerPurpose.Read())
    //                                    {
    //                                        existingPurposeId = (int)readerPurpose.GetValue(0);
    //                                    }

    //                                    readerPurpose.Close();
    //                                }
    //                            }

    //                            if (existingPurposeId > 0)
    //                            {
    //                                // Found an existing extry in the dictionary; save the mapping
    //                                mappingObjPurposeNJFto1NF[purposeId] = existingPurposeId;
    //                            }
    //                            else
    //                            {
    //                                // Need to add a new entry to the dictionary
    //                                values.Clear();

    //                                values.Add("id", purposeId);
    //                                values.Add("group_code", groupId);
    //                                values.Add("name", purpose);

    //                                query = GetInsertStatement("dict_balans_purpose", values);

    //                                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
    //                                {
    //                                    cmd.ExecuteNonQuery();
    //                                }

    //                                // Save the mapping
    //                                mappingObjPurposeNJFto1NF[purposeId] = purposeId;
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(query, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void PrepareObjToDocRelationCache()
    //    {
    //        // Extract relations between objects and documents from the OBJECT_DOKS table
    //        logger.WriteInfo("Building the cache of OBJECT_DOKS table");

    //        string querySelect = "SELECT ID, DOK_ID, OBJECT_KOD FROM OBJECT_DOKS";

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataDocId = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataObjectId = reader.IsDBNull(2) ? null : reader.GetValue(2);

    //                        if (dataObjectId is int)
    //                        {
    //                            objectsNJFRelatedToDocuments.Add((int)dataObjectId);

    //                            // Build a cache that allows to find an ID by document ID and object ID
    //                            if (dataId is int && dataDocId is int)
    //                            {
    //                                string key = GetNjfObjToDocRelationKey((int)dataObjectId, (int)dataDocId);

    //                                njfReverseCacheObjectDocs[key] = (int)dataId;
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }

    //        // Extract relations between objects and documents from the OBJECT_DOKS_PROPERTIES table
    //        logger.WriteInfo("Building the cache of OBJECT_DOKS_PROPERTIES table");

    //        querySelect = "SELECT ID, OBJECT_KOD, DOK_ID, NAME, CHARACTERISTIC, DESCRIPTION, " +
    //            "SUMMA_BALANS, SUMMA_ZAL, SQUARE, LEN, DT, ISP FROM OBJECT_DOKS_PROPERTIES";

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataObjectId = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataDocId = reader.IsDBNull(2) ? null : reader.GetValue(2);

    //                        if (dataObjectId is int)
    //                        {
    //                            objectsNJFRelatedToDocuments.Add((int)dataObjectId);
    //                        }

    //                        if (dataId is int && dataObjectId is int && dataDocId is int)
    //                        {
    //                            NJFObjectDocsProperties info = new NJFObjectDocsProperties();

    //                            info.objectId = (int)dataObjectId;
    //                            info.documentId = (int)dataDocId;
    //                            info.objectName = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                            info.characteristics = reader.IsDBNull(4) ? null : reader.GetValue(4);
    //                            info.miscInfo = reader.IsDBNull(5) ? null : reader.GetValue(5);
    //                            info.sumBalans = reader.IsDBNull(6) ? null : reader.GetValue(6);
    //                            info.sumZalishkova = reader.IsDBNull(7) ? null : reader.GetValue(7);
    //                            info.square = reader.IsDBNull(8) ? null : reader.GetValue(8);
    //                            info.length = reader.IsDBNull(9) ? null : reader.GetValue(9);
    //                            info.modifyDate = reader.IsDBNull(10) ? null : reader.GetValue(10);
    //                            info.modifiedBy = reader.IsDBNull(11) ? null : reader.GetValue(11);

    //                            njfObjectDoksProperties[(int)dataId] = info;
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private string GetNjfObjToDocRelationKey(int objectId, int documentId)
    //    {
    //        return objectId.ToString() + "_" + documentId.ToString();
    //    }

    //    private void PrepareOrgToObjectRelationCache()
    //    {
    //        // Extract relations between objects and organizations from the OBJECT_PRAVO table
    //        logger.WriteInfo("Extracting used objects and organizations from the OBJECT_PRAVO table");

    //        string querySelect = "SELECT ID, OBJECT_KOD, ZKPO FROM OBJECT_PRAVO"; // WHERE OBJECT_KODSTAN = 1

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        object dataObjectId = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataOrgId = reader.IsDBNull(2) ? null : reader.GetValue(2);

    //                        if (dataObjectId is int && dataOrgId is int)
    //                        {
    //                            objectsNJFRelatedToDocuments.Add((int)dataObjectId);
    //                            orgNJFRelatedToObjects.Add((int)dataOrgId);

    //                            /*
    //                            // Consider only objects that are going to be imported
    //                            if (objectsNJFRelatedToDocuments.Contains((int)dataObjectId))
    //                            {
    //                                orgNJFRelatedToObjects.Add((int)dataOrgId);
    //                            }
    //                            */
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }

    //        // Extract all the used organizations from the PRAVA_PROECT table
    //        logger.WriteInfo("Extracting used objects and organizations from the PRAVA_PROECT table");

    //        querySelect = "SELECT FROM_ORG, TOORG FROM PRAVA_PROECT";

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        object dataOrgFromId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataOrgToId = reader.IsDBNull(1) ? null : reader.GetValue(1);

    //                        if (dataOrgFromId is int)
    //                        {
    //                            orgNJFRelatedToObjects.Add((int)dataOrgFromId);
    //                        }

    //                        if (dataOrgToId is int)
    //                        {
    //                            orgNJFRelatedToObjects.Add((int)dataOrgToId);
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void PrepareDocToDocRelationCache()
    //    {
    //        Dictionary<string, object> values = new Dictionary<string, object>();

    //        // Prepare the SQL statement to get all the documents
    //        logger.WriteInfo("Building the cache of DOK_DEPEND table");

    //        string querySelect = "SELECT DOK_KOD1, DOK_KOD2 FROM DOK_DEPEND";

    //        // Get all the document links
    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the document data
    //                        object dataChildId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataParentId = reader.IsDBNull(1) ? null : reader.GetValue(1);

    //                        if (dataParentId is int && dataChildId is int)
    //                        {
    //                            int parentDocId = (int)dataParentId;
    //                            int childDocId = (int)dataChildId;

    //                            AddNjfDocRelationPair(parentDocId, childDocId);
    //                            AddNjfDocRelationPair(childDocId, parentDocId);
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void AddNjfDocRelationPair(int parentDocId, int childDocId)
    //    {
    //        object relChild = null;

    //        if (njfDocRelationCache.TryGetValue(parentDocId, out relChild))
    //        {
    //            if (relChild is List<int>)
    //            {
    //                if (!(relChild as List<int>).Contains(childDocId))
    //                {
    //                    (relChild as List<int>).Add(childDocId);
    //                }
    //            }
    //            else
    //            {
    //                int existingChild = (int)relChild;

    //                if (existingChild != childDocId)
    //                {
    //                    List<int> list = new List<int>();

    //                    list.Add((int)relChild);
    //                    list.Add(childDocId);

    //                    njfDocRelationCache[parentDocId] = list;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            njfDocRelationCache.Add(parentDocId, childDocId);
    //        }
    //    }

    //    private object FindNJFRozpDocByAct(int aktId)
    //    {
    //        object relDocs = null;

    //        if (njfDocRelationCache.TryGetValue(aktId, out relDocs))
    //        {
    //            if (relDocs is List<int>)
    //            {
    //                foreach (int relatedDoc in (relDocs as List<int>))
    //                {
    //                    // This must be a document from ROZP_DOK table
    //                    if (rozpDatesNJF.ContainsKey(relatedDoc))
    //                    {
    //                        return relatedDoc;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                int relatedDoc = (int)relDocs;

    //                // This must be a document from ROZP_DOK table
    //                if (rozpDatesNJF.ContainsKey(relatedDoc))
    //                {
    //                    return relatedDoc;
    //                }
    //            }
    //        }

    //        return null;
    //    }

    //    private object FindNJFAktByRozpDoc(int rozpDocId, int objectId)
    //    {
    //        object relDocs = null;

    //        if (njfDocRelationCache.TryGetValue(rozpDocId, out relDocs))
    //        {
    //            if (relDocs is List<int>)
    //            {
    //                foreach (int relatedDoc in (relDocs as List<int>))
    //                {
    //                    // This must be a document from AKTS table
    //                    if (aktDatesNJF.ContainsKey(relatedDoc))
    //                    {
    //                        // Check if Akt is related to the specified object
    //                        string key = GetNjfObjToDocRelationKey(objectId, relatedDoc);

    //                        if (njfReverseCacheObjectDocs.ContainsKey(key))
    //                            return relatedDoc;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                int relatedDoc = (int)relDocs;

    //                // This must be a document from AKTS table
    //                if (aktDatesNJF.ContainsKey(relatedDoc))
    //                {
    //                    // Check if Akt is related to the specified object
    //                    string key = GetNjfObjToDocRelationKey(objectId, relatedDoc);

    //                    if (njfReverseCacheObjectDocs.ContainsKey(key))
    //                        return relatedDoc;
    //                }
    //            }
    //        }

    //        return null;
    //    }

    //    private void PrepareProjectRightsCache()
    //    {
    //        logger.WriteInfo("Building the cache of PRAVA_PROECT table");

    //        string querySelect = "SELECT ID, RD, FROM_ORG, TOORG, PRAVO FROM PRAVA_PROECT";

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the document data
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataObjectDocsId = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataOrgFrom = reader.IsDBNull(2) ? null : reader.GetValue(2);
    //                        object dataOrgTo = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                        object dataPravo = reader.IsDBNull(4) ? null : reader.GetValue(4);

    //                        if (dataId is int && dataObjectDocsId is int && dataOrgFrom is int && dataOrgTo is int && dataPravo is int)
    //                        {
    //                            int objectDocsId = (int)dataObjectDocsId;

    //                            NJFSpecialRightInfo info = new NJFSpecialRightInfo(
    //                                (int)dataId, (int)dataOrgFrom, (int)dataOrgTo, (int)dataPravo);

    //                            object value = null;

    //                            if (njfSpecialRightsCache.TryGetValue(objectDocsId, out value))
    //                            {
    //                                if (value is List<NJFSpecialRightInfo>)
    //                                {
    //                                    (value as List<NJFSpecialRightInfo>).Add(info);
    //                                }
    //                                else
    //                                {
    //                                    List<NJFSpecialRightInfo> list = new List<NJFSpecialRightInfo>();

    //                                    list.Add((NJFSpecialRightInfo)value);
    //                                    list.Add(info);

    //                                    njfSpecialRightsCache[objectDocsId] = list;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                njfSpecialRightsCache[objectDocsId] = new NJFSpecialRightInfo(
    //                                    (int)dataId, (int)dataOrgFrom, (int)dataOrgTo, (int)dataPravo);
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    private void PrepareNJFStreetCache()
    //    {
    //        // Get all street names from the 1 NF database
    //        string querySelect = "SELECT KOD, NAME FROM SUL";
    //        int maxStreetCode1NF = -1;

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the object data
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataStreet = reader.IsDBNull(1) ? null : reader.GetValue(1);

    //                        if (dataId is int)
    //                        {
    //                            string street = (dataStreet is string) ? ((string)dataStreet).Trim() : "";

    //                            if (street.Length > 0)
    //                            {
    //                                streetCodes1NF[street] = (int)dataId;
    //                            }

    //                            // Find the maximum existing street code in 1NF
    //                            if ((int)dataId > maxStreetCode1NF)
    //                            {
    //                                maxStreetCode1NF = (int)dataId;
    //                            }
    //                        }
    //                    }

    //                    reader.Close();
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //            // Abort migration correctly
    //            throw new MigrationAbortedException();
    //        }

    //        // Prepare the log of unmatched NJF streets
    //        csvLogUnmatchedNJFStreets.WriteCell("ID");
    //        csvLogUnmatchedNJFStreets.WriteCell("Street name");
    //        csvLogUnmatchedNJFStreets.WriteEndOfLine();

    //        // Get all street names from the NJF database and store them in the streetNamesNJF dictionary
    //        querySelect = "SELECT KOD, NAME FROM SUL";

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the object data
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataStreet = reader.IsDBNull(1) ? null : reader.GetValue(1);

    //                        if (dataId is int && !streetCodeMappingNJF21NF.ContainsKey((int)dataId))
    //                        {
    //                            string street = (dataStreet is string) ? ((string)dataStreet).Trim() : "";

    //                            if (street.Length > 0)
    //                            {
    //                                streetNamesNJF[(int)dataId] = street;
    //                            }

    //                            // Prepare the mapping between NJF street codes and 1NF street codes
    //                            int streetCode1NF = 0;

    //                            if (streetCodes1NF.TryGetValue(street, out streetCode1NF))
    //                            {
    //                                streetCodeMappingNJF21NF[(int)dataId] = streetCode1NF;
    //                            }
    //                            else
    //                            {
    //                                // No such street name in 1NF; we have to add it
    //                                string insert = "insert into dict_streets (id, name, name_output, stan, activity, modified_by, modify_date)" +
    //                                    " values (@p1, @p2, @p3, 1, 1, @p4, @p5)";

    //                                using (SqlCommand cmd = new SqlCommand(insert, connectionSqlClient))
    //                                {
    //                                    maxStreetCode1NF++;

    //                                    cmd.Parameters.Add(new SqlParameter("p1", maxStreetCode1NF));
    //                                    cmd.Parameters.Add(new SqlParameter("p2", street));
    //                                    cmd.Parameters.Add(new SqlParameter("p3", street));
    //                                    cmd.Parameters.Add(new SqlParameter("p4", "Auto-migration"));
    //                                    cmd.Parameters.Add(new SqlParameter("p5", DateTime.Now));

    //                                    cmd.ExecuteNonQuery();
    //                                }

    //                                // Save the mapping between NJF code and new 1NF code
    //                                streetCodeMappingNJF21NF[(int)dataId] = maxStreetCode1NF;

    //                                // Write the log entry
    //                                csvLogUnmatchedNJFStreets.WriteCell(dataId.ToString());
    //                                csvLogUnmatchedNJFStreets.WriteCell(street);
    //                                csvLogUnmatchedNJFStreets.WriteEndOfLine();
    //                            }
    //                        }
    //                    }

    //                    reader.Close();
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //            // Abort migration correctly
    //            throw new MigrationAbortedException();
    //        }
    //    }

    //    private void PrepareNJFTechStateMapping()
    //    {
    //        mappingTechStateNJFto1NF.Add(1, 2);
    //        mappingTechStateNJFto1NF.Add(2, 7);
    //        mappingTechStateNJFto1NF.Add(3, 5);
    //        mappingTechStateNJFto1NF.Add(4, 1);
    //    }

    //    private void PrepareNJFDistrictMapping()
    //    {
    //        mappingDistrictNJFto1NF.Add(1, 902);
    //        mappingDistrictNJFto1NF.Add(2, 902);
    //        mappingDistrictNJFto1NF.Add(3, 903);
    //        mappingDistrictNJFto1NF.Add(4, 904);
    //        mappingDistrictNJFto1NF.Add(5, 905);
    //        mappingDistrictNJFto1NF.Add(6, 906);
    //        mappingDistrictNJFto1NF.Add(7, 907);
    //        mappingDistrictNJFto1NF.Add(8, 908);
    //        mappingDistrictNJFto1NF.Add(9, 909);

    //        mappingDistrictNJFto1NF.Add(10, 389);
    //        mappingDistrictNJFto1NF.Add(11, 364);
    //        mappingDistrictNJFto1NF.Add(12, 912);
    //        mappingDistrictNJFto1NF.Add(13, 386);
    //        mappingDistrictNJFto1NF.Add(14, 361);
    //        mappingDistrictNJFto1NF.Add(15, 380);
    //        mappingDistrictNJFto1NF.Add(17, 364);
    //        mappingDistrictNJFto1NF.Add(18, 918);

    //        mappingDistrictNJFto1NF.Add(24, 386);
    //        mappingDistrictNJFto1NF.Add(31, 931);
    //        mappingDistrictNJFto1NF.Add(62, 364);
    //        mappingDistrictNJFto1NF.Add(63, 363);
    //        mappingDistrictNJFto1NF.Add(66, 366);
    //        mappingDistrictNJFto1NF.Add(69, 386);
    //        mappingDistrictNJFto1NF.Add(72, 389);
    //        mappingDistrictNJFto1NF.Add(75, 386);
    //        mappingDistrictNJFto1NF.Add(76, 391);
    //        mappingDistrictNJFto1NF.Add(78, 380);
    //        mappingDistrictNJFto1NF.Add(79, 361);
    //        mappingDistrictNJFto1NF.Add(82, 382);
    //        mappingDistrictNJFto1NF.Add(85, 385);
    //        mappingDistrictNJFto1NF.Add(88, 389);
    //        mappingDistrictNJFto1NF.Add(90, 363);
    //        mappingDistrictNJFto1NF.Add(91, 391);
    //    }

    //    private void PrepareNJFObjTypeMapping()
    //    {
    //        int objTypeInBuilt = FindDictionaryEntryIdInSQLServer("dict_object_type", "id", "name",
    //                GUKV.DataMigration.Properties.Resources.ObjectTypeInBuilt);

    //        int objTypeInBlock = FindDictionaryEntryIdInSQLServer("dict_object_type", "id", "name",
    //                GUKV.DataMigration.Properties.Resources.ObjectTypeInBlock);

    //        int objTypeOther = FindDictionaryEntryIdInSQLServer("dict_object_type", "id", "name",
    //                GUKV.DataMigration.Properties.Resources.ObjectTypeOther);

    //        if (objTypeInBuilt < 0)
    //        {
    //            objTypeInBuilt = 10;

    //            AddDictionaryEntryInSQLServer("dict_object_type", "id", "name",
    //                objTypeInBuilt, GUKV.DataMigration.Properties.Resources.ObjectTypeInBuilt);
    //        }

    //        if (objTypeInBlock < 0)
    //        {
    //            objTypeInBlock = 11;

    //            AddDictionaryEntryInSQLServer("dict_object_type", "id", "name",
    //                objTypeInBlock, GUKV.DataMigration.Properties.Resources.ObjectTypeInBlock);
    //        }

    //        if (objTypeOther < 0)
    //        {
    //            objTypeOther = 12;

    //            AddDictionaryEntryInSQLServer("dict_object_type", "id", "name",
    //                objTypeOther, GUKV.DataMigration.Properties.Resources.ObjectTypeOther);
    //        }

    //        mappingObjTypeNJFto1NF = FieldMappings.CreateObjectTypeMappingNJFto1NF(objTypeInBuilt, objTypeInBlock, objTypeOther);
    //    }

    //    private void PrepareNJFObjKindMapping()
    //    {
    //        // The first 4 entries of NJF SKINDOBJ table are identical to the 1NF SKINDOBJ table
    //        mappingObjKindNJFto1NF.Add(1, 1);
    //        mappingObjKindNJFto1NF.Add(2, 2);
    //        mappingObjKindNJFto1NF.Add(3, 3);
    //        mappingObjKindNJFto1NF.Add(4, 4);

    //        // All other entries must be mapped to the new range of IDs
    //        string querySelect = "SELECT KOD, NAME, MNEMONIC FROM SKINDOBJ ORDER BY KOD";

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the object data
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataName = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataMnemonic = reader.IsDBNull(2) ? null : reader.GetValue(2);

    //                        int idNJF = (dataId is int) ? (int)dataId : 0;
    //                        string name = (dataName is string) ? (string)dataName : "";
    //                        string mnemonic = (dataMnemonic is string) ? (string)dataMnemonic : "";

    //                        if (idNJF > 0)
    //                        {
    //                            // Determine ID of this entry in 1 NF
    //                            if (idNJF >= 5)
    //                            {
    //                                int id1NF = idNJF + 2;

    //                                AddDictionaryEntryInSQLServer("dict_object_kind", "id", "name", id1NF, name);
    //                                UpdateDictionaryEntryInSQLServer("dict_object_kind", "id", "mnemonic", id1NF, mnemonic);

    //                                mappingObjKindNJFto1NF.Add(idNJF, id1NF);
    //                            }
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
    //            else
    //            {
    //                ShowSqlErrorMessageDlg(querySelect, ex.Message);

    //                // Abort migration correctly
    //                throw new MigrationAbortedException();
    //            }
    //        }
    //    }

    //    #endregion (Rosporadjennia database migration)
    //}
}
