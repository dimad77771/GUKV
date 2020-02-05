using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;

namespace GUKV.DataMigration
{
    /// <summary>
    /// Populates the SQL Server database from the Firebird databases.
    /// </summary>
    public partial class Migrator
    {
        #region Member variables

        /// <summary>
        /// A mapping of organization IDs between Privatizacia and 1NF
        /// </summary>
        //private Dictionary<int, int> orgIdMappingPrivatTo1NF = new Dictionary<int, int>();

        /// <summary>
        /// Mapping of UMS$FINANCING_FORM dictionary (Privatizacia) to dict_org_form_gosp
        /// </summary>
        //private Dictionary<int, int> finFormMappingPrivatTo1NF = new Dictionary<int, int>();

        /// <summary>
        /// Mapping of SSTATUS dictionary (Privatizacia) to dict_org_status
        /// </summary>
        //private Dictionary<int, int> orgStatusMappingPrivatTo1NF = new Dictionary<int, int>();

        /// <summary>
        /// Mapping of SVIDOBJ dictionary (Privatizacia) to dict_object_kind
        /// </summary>
        //private Dictionary<int, int> objKindMappingPrivatTo1NF = new Dictionary<int, int>();

        /// <summary>
        /// Mapping of STIPOBJ dictionary (Privatizacia) to dict_object_type
        /// </summary>
        //private Dictionary<int, int> objTypeMappingPrivatTo1NF = new Dictionary<int, int>();

        /// <summary>
        /// Relates the 'Privatizacia' objects to balans records in 1 NF
        /// </summary>
        //private Dictionary<int, int> privatBalansIdByObjectId = new Dictionary<int, int>();

        /// <summary>
        /// A cache that stores privatization state by object ID
        /// </summary>
        //private Dictionary<int, int> privatStateIdByObjectId = new Dictionary<int, int>();

        /// <summary>
        /// Contains ID of primary document for each Privatizacia object
        /// </summary>
        //private Dictionary<int, int> privatNormDocIdByObjectId = new Dictionary<int, int>();

        /// <summary>
        /// Auto-incremented Id of organizations that are added from the Privatizacia database
        /// </summary>
        //private int privatizationOrgIdSeed = 500000;

        /// <summary>
        /// Auto-incremented Id of documents that are added from the Privatizacia database
        /// </summary>
        //private int privatizationDocIdSeed = 50000;

        /// <summary>
        /// This is a cache for the MOBJ table of the Privatizacia database. A key in this
        /// dictionary is 'ID' column of the MOBJ table.
        /// </summary>
        //private Dictionary<int, PrivatizaciaObjInfo> privatizaciaObjects = new Dictionary<int, PrivatizaciaObjInfo>();

        /// <summary>
        /// This dictionary maps 'Privatizacia' objects to the '1NF' objects
        /// </summary>
        //private Dictionary<int, int> mapObjectPrivatTo1NF = new Dictionary<int, int>();

        /// <summary>
        /// This dictionary maps 'Privatizacia' documents to the '1NF' documents
        /// </summary>
        //private Dictionary<int, int> mapDocumentsPrivatTo1NF = new Dictionary<int, int>();

        /// <summary>
        /// A CSV log that contains all objects from the Privatizacia database that were not
        /// found in the 1NF database
        /// </summary>
        private CSVLog csvLogUnmatchedPrivatObj = new CSVLog("CSV/NotFoundPrivatObj.csv");

        /// <summary>
        /// Value of the general_kind_id column for all documents from the Privatizacia database
        /// </summary>
        //private int docGeneralCategoryPrivatization = 0;

        #endregion (Member variables)

        //#region Privatizacia database migration

        //private void MigratePrivatization()
        //{
        //    logger.WriteInfo("=== Migrating database Privatizacia ===");

        //    // Connect to Privatizacia database
        //    connectionPrivatizacia = ConnectToFirebirdDatabase("Privatizacia", GetPrivatConnectionString());

        //    if (connectionPrivatizacia != null && connectionPrivatizacia.State == ConnectionState.Open)
        //    {
        //        PreparePrivatizationOrgIdMapping();
        //        PreparePrivatizationFinFormMapping();
        //        PreparePrivatizationOrgFormMapping();
        //        PreparePrivatizationObjKindMapping();
        //        PreparePrivatizationObjTypeMapping();

        //        // Rebuild the cache of 1NF organizations before performing object matching
        //        organizationFinder.BuildOrganizationCacheFromSqlServer(connectionSqlClient, false);

        //        MigratePrivatizationObjDictionaries();
        //        MigratePrivatizationsOrgDictionaries();
        //        MigratePrivatizationsOrganizations();
        //        MigratePrivatizationObjects();

        //        MigratePrivatizationDocuments();
        //        MigratePrivatizationDocLinks();
        //        MigratePrivatizationObjDocLinks();
        //        MarkPrivatizationPrimaryDocs();
        //        MigratePrivatRishenDocsForObjects();

        //        GeneratePrivatizationAddrMismatchReport();

        //        // Close connection
        //        logger.WriteInfo("Disconnectiong from the Privatizacia database");

        //        connectionPrivatizacia.Close();
        //        connectionPrivatizacia = null;
        //    }
        //}

        //private void PreparePrivatizationOrgIdMapping()
        //{
        //    /////////////////////////////////////////////////////////////////////////////
        //    // 1) Use information from the SZKPO table: KOD_OBJ_1NF and ZKPO
        //    /////////////////////////////////////////////////////////////////////////////

        //    // Get the required columns from the SZKPO table
        //    string querySelect = "SELECT ID, ZKPO, KOD_OBJ_1NF FROM SZKPO";

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Get the organization data
        //                    object dataId = Short2Int(reader.IsDBNull(0) ? null : reader.GetValue(0));
        //                    object dataZkpo = reader.IsDBNull(1) ? null : reader.GetValue(1);
        //                    object dataId1NF = Short2Int(reader.IsDBNull(2) ? null : reader.GetValue(2));

        //                    if (dataId is int)
        //                    {
        //                        if (dataId1NF is int)
        //                        {
        //                            // Direct mapping exists
        //                            orgIdMappingPrivatTo1NF[(int)dataId] = (int)dataId1NF;
        //                        }
        //                        else if (dataZkpo is string)
        //                        {
        //                            // Try to find organization in 1NF by ZKPO code
        //                            bool forceCreate = false;
        //                            int orgIdIn1NF = organizationFinder.GetOrgIdByZKPO((string)dataZkpo, "", true, out forceCreate);

        //                            if (orgIdIn1NF > 0)
        //                            {
        //                                orgIdMappingPrivatTo1NF[(int)dataId] = orgIdIn1NF;
        //                            }
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

        //    /////////////////////////////////////////////////////////////////////////////
        //    // 2) Read the user-defined mapping
        //    /////////////////////////////////////////////////////////////////////////////

        //    // Read the 'mapping_org_privat_1nf' table from SQL Server
        //    querySelect = "SELECT org_id_privat, org_id_1nf FROM mapping_org_privat_1nf";

        //    try
        //    {
        //        using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
        //        {
        //            using (SqlDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Get the mapping entry
        //                    object dataOrgIdPrivat = reader.IsDBNull(0) ? null : reader.GetValue(0);
        //                    object dataOrgId1nf = reader.IsDBNull(1) ? null : reader.GetValue(1);

        //                    if (dataOrgIdPrivat is int && dataOrgId1nf is int)
        //                    {
        //                        orgIdMappingPrivatTo1NF[(int)dataOrgIdPrivat] = (int)dataOrgId1nf;
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

        //private void PreparePrivatizationFinFormMapping()
        //{
        //    finFormMappingPrivatTo1NF.Clear();

        //    finFormMappingPrivatTo1NF.Add(1, 2); // БЮДЖЕТНА
        //    finFormMappingPrivatTo1NF.Add(2, 1); // ГОСПРОЗРАХУНКОВА
        //    finFormMappingPrivatTo1NF.Add(3, 5); // ЗМІШАНА
        //}

        //private void PreparePrivatizationOrgFormMapping()
        //{
        //    orgStatusMappingPrivatTo1NF.Clear();

        //    orgStatusMappingPrivatTo1NF.Add(1, 2); // Фізична
        //    orgStatusMappingPrivatTo1NF.Add(2, 1); // Юридична
        //}

        //private void PreparePrivatizationObjKindMapping()
        //{
        //    objKindMappingPrivatTo1NF.Add(13, 1);
        //    objKindMappingPrivatTo1NF.Add(31, 2);
        //    objKindMappingPrivatTo1NF.Add(36, 2);
        //    objKindMappingPrivatTo1NF.Add(53, 20);
        //    objKindMappingPrivatTo1NF.Add(57, 20);

        //    /*
        //    // Find all entries from the SVIDOBJ dictionary that are really used
        //    HashSet<int> setUsedCodes = new HashSet<int>();

        //    string querySelect = "SELECT DISTINCT KOD_VIDOBJ FROM KMOBJ";

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    object dataCode = reader.IsDBNull(0) ? null : reader.GetValue(0);

        //                    if (dataCode is int)
        //                    {
        //                        int code = (int)dataCode;

        //                        if (code > 0)
        //                        {
        //                            setUsedCodes.Add(code);
        //                        }
        //                    }
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

        //    // Get the text for the found entries from SVIDOBJ table
        //    Dictionary<int, string> objectKinds = new Dictionary<int, string>();

        //    querySelect = "SELECT KOD_VIDOBJ, NAME FROM SVIDOBJ";

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    object dataCode = reader.IsDBNull(0) ? null : reader.GetValue(0);
        //                    object dataName = reader.IsDBNull(1) ? null : reader.GetValue(1);

        //                    if (dataCode is int && dataName is string)
        //                    {
        //                        int code = (int)dataCode;
        //                        string name = ((string)dataName).Trim();

        //                        if (setUsedCodes.Contains(code) && name.Length > 0)
        //                        {
        //                            objectKinds.Add(code, name);
        //                        }
        //                    }
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

        //    // Check if those entires are present in the 'dict_object_kind' table
        //    int newDictEntryId = 25;

        //    foreach (KeyValuePair<int, string> pair in objectKinds)
        //    {
        //        int existingId = FindDictionaryEntryIdInSQLServer("dict_object_kind", "id", "name", pair.Value);

        //        if (existingId > 0)
        //        {
        //            // Save the existing mapping
        //            objKindMappingPrivatTo1NF.Add(pair.Key, existingId);
        //        }
        //        else
        //        {
        //            AddDictionaryEntryInSQLServer("dict_object_kind", "id", "name", newDictEntryId, pair.Value);

        //            objKindMappingPrivatTo1NF.Add(pair.Key, newDictEntryId);

        //            newDictEntryId++;
        //        }
        //    }
        //    */
        //}

        //private void PreparePrivatizationObjTypeMapping()
        //{
        //    int objTypeInBuilt = FindDictionaryEntryIdInSQLServer("dict_object_type", "id", "name",
        //            GUKV.DataMigration.Properties.Resources.ObjectTypeInBuilt);

        //    if (objTypeInBuilt < 0)
        //    {
        //        objTypeInBuilt = 10;

        //        AddDictionaryEntryInSQLServer("dict_object_type", "id", "name",
        //            objTypeInBuilt, GUKV.DataMigration.Properties.Resources.ObjectTypeInBuilt);
        //    }

        //    objTypeMappingPrivatTo1NF.Add(6, objTypeInBuilt);
        //    objTypeMappingPrivatTo1NF.Add(11, 1);
        //    objTypeMappingPrivatTo1NF.Add(12, 9);
        //}

        //private void MigratePrivatizationsOrgDictionaries()
        //{
        //    Dictionary<string, string> fields = new Dictionary<string, string>();

        //    // S_VID_DIAL
        //    fields.Clear();
        //    fields.Add("KOD_VID_DIAL", "id");
        //    fields.Add("NAME_VID_DIAL", "name");
        //    fields.Add("KOD_GALUZ", "branch_code");

        //    OverrideDictionary(connectionPrivatizacia, "S_VID_DIAL", "dict_org_occupation", fields,
        //        "KOD_VID_DIAL", "id", "KOD_VID_DIAL", null, true);

        //    // SGALUZ
        //    fields.Clear();
        //    fields.Add("KOD_GALUZ", "id");
        //    fields.Add("NAME", "name");

        //    OverrideDictionary(connectionPrivatizacia, "SGALUZ", "dict_org_industry", fields,
        //        "KOD_GALUZ", "id", "KOD_GALUZ", null, true);

        //    // SKOPFG
        //    fields.Clear();
        //    fields.Add("KOD_KOPFG", "id");
        //    fields.Add("NAME", "name");

        //    OverrideDictionary(connectionPrivatizacia, "SKOPFG", "dict_org_form", fields,
        //        "KOD_KOPFG", "id", "KOD_KOPFG", null, true);
        //}

        //private void MigratePrivatizationsOrganizations()
        //{
        //    logger.WriteInfo("Migrating organizations from the Privatizacia database");

        //    Dictionary<string, string> fieldsAll = new Dictionary<string, string>();
        //    Dictionary<string, string> fieldsNew = new Dictionary<string, string>();

        //    CreatePrivatOrganizationsAllFieldsMapping(fieldsAll);
        //    CreatePrivatOrganizationsNewFieldsMapping(fieldsNew);

        //    // Dump all organizations from Balans database to our database
        //    ExecuteStatementInSQLServer("DELETE FROM " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable);

        //    string srcFieldList = "";

        //    foreach (KeyValuePair<string, string> entry in fieldsAll)
        //    {
        //        if (entry.Key != "KVED")
        //        {
        //            if (srcFieldList.Length > 0)
        //                srcFieldList += ", ";

        //            srcFieldList += "z." + entry.Key;
        //        }
        //    }

        //    string selectStatement = @"SELECT " + srcFieldList + ", kv.KVED FROM SZKPO z LEFT OUTER JOIN SKVED kv ON kv.KOD_KVED = z.KOD_KVED";

        //    using (FbCommand cmd = new FbCommand(selectStatement, connectionPrivatizacia))
        //    {
        //        using (FbDataReader r = cmd.ExecuteReader())
        //        {
        //            BulkMigrateDataReader("SZKPO", OrganizationsTempTable, r, fieldsAll);

        //            r.Close();
        //        }
        //    }

        //    // Perform mapping of some dictionaries
        //    ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable + " SET origin_db = 4");

        //    ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable + " SET industry_id = NULL WHERE industry_id = 0");
        //    ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable + " SET organ_id = NULL WHERE organ_id = 0");
        //    ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable + " SET form_id = NULL WHERE form_id = 0");
        //    ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable + " SET registr_org_id = NULL WHERE registr_org_id = 0");

        //    UpdateSqlServerDictionaryByMapping(OrganizationsTempTable, "status_id", orgStatusMappingPrivatTo1NF);
        //    UpdateSqlServerDictionaryByMapping(OrganizationsTempTable, "form_gosp_id", finFormMappingPrivatTo1NF);

        //    // Perform mapping of Privatization organizations to 1NF organizations
        //    string query = "SELECT id, zkpo_code, full_name, short_name FROM " + OrganizationsTempTable + " WHERE (is_deleted IS NULL) OR (is_deleted = 0)";

        //    using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
        //    {
        //        using (SqlDataReader r = cmd.ExecuteReader())
        //        {
        //            while (r.Read())
        //            {
        //                int id = r.GetInt32(0);
        //                string zkpo = r.IsDBNull(1) ? "" : r.GetString(1);
        //                string fullName = r.IsDBNull(2) ? "" : r.GetString(2);
        //                string shortName = r.IsDBNull(3) ? "" : r.GetString(3);

        //                if (!orgIdMappingPrivatTo1NF.ContainsKey(id))
        //                {
        //                    bool categorized = false;
        //                    int existingOrganizationID = organizationFinder.FindOrganization(zkpo, fullName, shortName, true, out categorized);

        //                    if (existingOrganizationID > 0)
        //                    {
        //                        orgIdMappingPrivatTo1NF.Add(id, existingOrganizationID);
        //                    }
        //                    else
        //                    {
        //                        orgIdMappingPrivatTo1NF.Add(id, id + privatizationOrgIdSeed);
        //                    }
        //                }
        //            }

        //            r.Close();
        //        }
        //    }

        //    // Set the master ID for each mapped organization
        //    foreach (KeyValuePair<int, int> pair in orgIdMappingPrivatTo1NF)
        //    {
        //        if (pair.Value < privatizationOrgIdSeed)
        //        {
        //            query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
        //                " SET master_org_id = @masterid WHERE id = @orgid";

        //            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
        //            {
        //                cmd.Parameters.Add(new SqlParameter("orgid", pair.Key));
        //                cmd.Parameters.Add(new SqlParameter("masterid", pair.Value));

        //                cmd.ExecuteNonQuery();
        //            }

        //            OverwritePrivatOrganization(pair.Key, pair.Value, fieldsNew);
        //        }
        //    }

        //    // Make sure that Balans organizations never mess up with 1NF organizations
        //    ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
        //        " SET id = id + " + privatizationOrgIdSeed);

        //    // Perform a BULK insert from the temporary table into the main table
        //    BulkMigrateNewOrganizations();
        //}

        //private void CreatePrivatOrganizationsNewFieldsMapping(Dictionary<string, string> fields)
        //{
        //    fields.Clear();

        //    fields.Add("SVIDDT", "registration_svid_date");
        //    fields.Add("KVARTIRA_FIZ", "addr_flat_num");
        //    fields.Add("PASP_SER_FIZ", "director_passp_seria");
        //    fields.Add("PASP_NOM_FIZ", "director_passp_num");
        //    fields.Add("PASP_VID_FIZ", "director_passp_auth");
        //    fields.Add("PASP_DT_FIZ", "director_passp_date");
        //    fields.Add("PASP_SER", "povnov_passp_seria");
        //    fields.Add("PASP_NOM", "povnov_passp_num");
        //    fields.Add("PASP_VID", "povnov_passp_auth");
        //    fields.Add("PASP_DT", "povnov_passp_date");
        //    fields.Add("FIO_OSOBA", "povnov_osoba_fio");
        //    fields.Add("POVNOV", "povnovajennia");
        //    fields.Add("CITY", "addr_city");
        //}

        //private void CreatePrivatOrganizationsAllFieldsMapping(Dictionary<string, string> fields)
        //{
        //    fields.Clear();

        //    CreatePrivatOrganizationsNewFieldsMapping(fields);

        //    fields.Add("ID", "id");
        //    fields.Add("KOD_STAN_1NF", "last_state");
        //    fields.Add("NAME", "short_name");
        //    fields.Add("FULL_NAME", "full_name");
        //    fields.Add("ZKPO", "zkpo_code");
        //    fields.Add("POSTIND", "addr_zip_code");

        //    fields.Add("NOMER", "addr_nomer");
        //    fields.Add("ADRDOP", "addr_misc");
        //    fields.Add("REESDT", "registration_date");
        //    fields.Add("REESNO", "registration_num");
        //    fields.Add("KOD_GALUZ", "industry_id");

        //    fields.Add("KOD_FORMVL", "form_ownership_id");
        //    fields.Add("KOD_ORGUPR", "organ_id");
        //    fields.Add("KOD_KOPFG", "form_id");
        //    fields.Add("KVED", "kved_code");
        //    fields.Add("KOD_REESORG", "registr_org_id");

        //    fields.Add("FIO", "director_fio");
        //    fields.Add("TEL", "director_phone");
        //    fields.Add("FAX", "fax");
        //    fields.Add("KOD_STATUS", "status_id");
        //    fields.Add("KOATUU", "koatuu");

        //    fields.Add("REKVIZIT_BANK", "bank_mfo");
        //    fields.Add("STREET", "addr_street_name");
        //    fields.Add("DTISP", "modify_date");
        //    fields.Add("ISP", "modified_by");
        //    fields.Add("ID_ACTIVITY_STATUS", "occupation_id");

        //    fields.Add("ID_FINANCING_FORM", "form_gosp_id");
        //    fields.Add("KOD_RA", "addr_distr_new_id");
        //}

        //private void OverwritePrivatOrganization(int sourceOrgId, int destinationOrgId, Dictionary<string, string> fieldsNew)
        //{
        //    string fieldList = "";
        //    string srcFieldList = "";
        //    SqlCommand cmdUpdate = new SqlCommand("", connectionSqlClient);

        //    foreach (KeyValuePair<string, string> entry in fieldsNew)
        //    {
        //        if (srcFieldList.Length > 0)
        //            srcFieldList += ", ";

        //        srcFieldList += entry.Value;
        //    }

        //    using (SqlCommand cmd = new SqlCommand("SELECT " + srcFieldList + " FROM " + Properties.Settings.Default.ConnectionSQLServerDatabase +
        //        ".dbo." + OrganizationsTempTable + " WHERE id = @srcid", connectionSqlClient))
        //    {
        //        cmd.Parameters.Add(new SqlParameter("srcid", sourceOrgId));

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                // Prepare the UPDATE query from the data reader
        //                for (int i = 0; i < reader.FieldCount; i++)
        //                {
        //                    string columnName = reader.GetName(i).ToLower();

        //                    if (!reader.IsDBNull(i))
        //                    {
        //                        cmdUpdate.Parameters.Add(new SqlParameter("param" + i.ToString(), reader.GetValue(i)));

        //                        fieldList += ", " + columnName + " = @param" + i.ToString();
        //                    }
        //                }
        //            }

        //            reader.Close();
        //        }
        //    }

        //    if (fieldList.Length > 0)
        //    {
        //       // Perform UPDATE
        //        cmdUpdate.CommandText = "UPDATE organizations SET " + fieldList.TrimStart(' ', ',') + " WHERE id = @destid";

        //        cmdUpdate.Parameters.Add(new SqlParameter("destid", destinationOrgId));

        //        try
        //        {
        //            cmdUpdate.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.WriteError("Privatization organization " + sourceOrgId.ToString() + " has some invalid data: " + ex.Message);
        //        }
        //    }
        //}

        //private void MigratePrivatizationObjDictionaries()
        //{
        //    Dictionary<string, string> fields = new Dictionary<string, string>();

        //    // S_OZN_PRIV
        //    fields.Clear();
        //    fields.Add("ID", "id");
        //    fields.Add("NAME", "name");

        //    MigrateTable(connectionPrivatizacia, "S_OZN_PRIV", "dict_privat_state", fields, "ID", null);

        //    // SKLGR
        //    fields.Clear();
        //    fields.Add("KOD_KLGR", "id");
        //    fields.Add("NAME", "name");

        //    MigrateTable(connectionPrivatizacia, "SKLGR", "dict_privat_obj_group", fields, "KOD_KLGR", null);

        //    DeleteDictionaryEntryInSQLServer("dict_privat_obj_group", "id", 0); // Avoid empty records in dictionaries

        //    // Replace the english codes in this dictionary with cyrillic codes
        //    UpdateDictionaryEntryInSQLServer("dict_privat_obj_group", "id" , "name", 1,
        //        GUKV.DataMigration.Properties.Resources.PrivatGroupCodeA);

        //    UpdateDictionaryEntryInSQLServer("dict_privat_obj_group", "id", "name", 8,
        //        GUKV.DataMigration.Properties.Resources.PrivatGroupCodeE);

        //    // SSPOSPRIV
        //    fields.Clear();
        //    fields.Add("KOD_SPOSPRIV", "id");
        //    fields.Add("NAME", "name");

        //    MigrateTable(connectionPrivatizacia, "SSPOSPRIV", "dict_privat_kind", fields, "KOD_SPOSPRIV", null);

        //    DeleteDictionaryEntryInSQLServer("dict_privat_kind", "id", 0); // Avoid empty records in dictionaries

        //    // SOZNOBJ
        //    fields.Clear();
        //    fields.Add("KOD_OZNOBJ", "id");
        //    fields.Add("NAME", "name");

        //    MigrateTable(connectionPrivatizacia, "SOZNOBJ", "dict_privat_complex", fields, "KOD_OZNOBJ", null);

        //    DeleteDictionaryEntryInSQLServer("dict_privat_complex", "id", 0); // Avoid empty records in dictionaries

        //    // SSFERUPR
        //    fields.Clear();
        //    fields.Add("ID", "id");
        //    fields.Add("NAME", "code");
        //    fields.Add("FULLNAME", "name");

        //    MigrateTable(connectionPrivatizacia, "SSFERUPR", "dict_privat_subordination", fields, "ID", null);

        //    DeleteDictionaryEntryInSQLServer("dict_privat_subordination", "id", 0); // Avoid empty records in dictionaries
        //}

        //private void MigratePrivatizationObjects()
        //{
        //    /////////////////////////////////////////////////////////////////////////////
        //    // 1) Read in the table that relates Privatizacia objects to 1 NF objects
        //    /////////////////////////////////////////////////////////////////////////////

        //    string querySelect = "SELECT ID, ID_1NF_OBJ, POSTIND, NEWDISTR, NUL, NOMER, ADRDOP, KOD_ARHIT, DTISP, ISP FROM MOBJ";

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Create a cache entry for this MOBJ record
        //                    PrivatizaciaObjInfo info = new PrivatizaciaObjInfo();

        //                    info.objectId = (int)reader.GetValue(0);

        //                    info.objectId1NF = Short2Int(reader.IsDBNull(1) ? null : reader.GetValue(1));
        //                    info.addrZipCode = reader.IsDBNull(2) ? null : reader.GetValue(2);
        //                    info.districtId = Short2Int(reader.IsDBNull(3) ? null : reader.GetValue(3));
        //                    info.addrStreetName = reader.IsDBNull(4) ? null : reader.GetValue(4);
        //                    info.addrNomer = reader.IsDBNull(5) ? null : reader.GetValue(5);
        //                    info.addrMiscInfo = reader.IsDBNull(6) ? null : reader.GetValue(6);
        //                    info.historyId = Short2Int(reader.IsDBNull(7) ? null : reader.GetValue(7));
        //                    info.modifyDate = reader.IsDBNull(8) ? null : reader.GetValue(8);
        //                    info.modifiedBy = reader.IsDBNull(9) ? null : reader.GetValue(9);

        //                    // Avoid zero in the "KOD_ARHIT" column
        //                    if (info.historyId is int && ((int)info.historyId) == 0)
        //                    {
        //                        info.historyId = null;
        //                    }

        //                    privatizaciaObjects[info.objectId] = info;
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

        //    /////////////////////////////////////////////////////////////////////////////
        //    // 2) Read the mapping between 'Privatizacia' objects and 1 NF balans table
        //    /////////////////////////////////////////////////////////////////////////////

        //    querySelect = "SELECT ID, BALID, ID_OZN_PRIV FROM PRIVOBJ ORDER BY ID, ID_STAN";

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    object dataObjectId = Short2Int(reader.IsDBNull(0) ? null : reader.GetValue(0));
        //                    object dataBalansId = Short2Int(reader.IsDBNull(1) ? null : reader.GetValue(1));
        //                    object dataStateId = Short2Int(reader.IsDBNull(2) ? null : reader.GetValue(2));

        //                    if (dataObjectId is int && dataBalansId is int)
        //                    {
        //                        privatBalansIdByObjectId[(int)dataObjectId] = (int)dataBalansId;
        //                    }

        //                    if (dataObjectId is int && dataStateId is int)
        //                    {
        //                        if ((int)dataStateId > 0)
        //                        {
        //                            privatStateIdByObjectId[(int)dataObjectId] = (int)dataStateId;
        //                        }
        //                    }
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

        //    /////////////////////////////////////////////////////////////////////////////
        //    // 3) Read the mapping of organizations (buyers) to objects
        //    /////////////////////////////////////////////////////////////////////////////

        //    Dictionary<int, int> purchasers = new Dictionary<int, int>();

        //    querySelect = "SELECT ID_KMOBJ, ID_ZKPO FROM ZAJ";

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    object dataObjectId = Short2Int(reader.IsDBNull(0) ? null : reader.GetValue(0));
        //                    object dataOrgId = Short2Int(reader.IsDBNull(1) ? null : reader.GetValue(1));

        //                    if (dataObjectId is int && dataOrgId is int)
        //                    {
        //                        purchasers[(int)dataObjectId] = (int)dataOrgId;
        //                    }
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

        //    /////////////////////////////////////////////////////////////////////////////
        //    // 4) Migrate the objects from the KMOBJ table, and save a mapping between
        //    //   'Privatizacia' objects and '1 NF' objects
        //    /////////////////////////////////////////////////////////////////////////////

        //    HashSet<int> processedObjects = new HashSet<int>();
        //    HashSet<int> primaryObjects = new HashSet<int>();

        //    // Write a header to the CSV log that contains unmatched objects
        //    string cvsLogHeaderPrivatObjName = GUKV.DataMigration.Properties.Resources.CvsLogPrivatObjName;
        //    string cvsLogHeaderPrivatObjZIP = GUKV.DataMigration.Properties.Resources.CvsLogPrivatObjZIP;
        //    string cvsLogHeaderPrivatObjStreet = GUKV.DataMigration.Properties.Resources.CvsLogPrivatObjStreet;
        //    string cvsLogHeaderPrivatObjNumber = GUKV.DataMigration.Properties.Resources.CvsLogPrivatObjNumber;
        //    string cvsLogHeaderPrivatObjKorpus = GUKV.DataMigration.Properties.Resources.CvsLogPrivatObjKorpus;

        //    csvLogUnmatchedPrivatObj.WriteCell(cvsLogHeaderPrivatObjName);
        //    csvLogUnmatchedPrivatObj.WriteCell(cvsLogHeaderPrivatObjZIP);
        //    csvLogUnmatchedPrivatObj.WriteCell(cvsLogHeaderPrivatObjStreet);
        //    csvLogUnmatchedPrivatObj.WriteCell(cvsLogHeaderPrivatObjNumber);
        //    csvLogUnmatchedPrivatObj.WriteCell(cvsLogHeaderPrivatObjKorpus);
        //    csvLogUnmatchedPrivatObj.WriteEndOfLine();

        //    querySelect = "SELECT ID, ID_MOBJ, ID_ZKPO, NAZVA, POSTIND, NOMER, ADRDOP, NEWDISTR, S_ZAG, NUL, KOD_KLGR, " +
        //        "KOD_SPOSPRIV, DTISP, ISP, KOD_OZNOBJ, MISCE, CINA, EK_VART, EK_VARTD, ID_SFERUPR, ID_NORMDOC, " +
        //        "KOD_VIDOBJ, KOD_TIPOBJ, KOD_GRNAZN, KOD_POVERH, KOD_ARHIT FROM KMOBJ";

        //    MigratePivatizationObjectsFromTable(querySelect, purchasers, processedObjects, true, primaryObjects);

        //    querySelect = "SELECT ID, ID_MOBJ, ID_ZKPO, NAZVA, POSTIND, NOMER, ADRDOP, NEWDISTR, S_ZAG, NUL, KOD_KLGR," +
        //        " KOD_SPOSPRIV,  DTISP, ISP, NULL AS \"KOD_OZNOBJ\", NULL AS \"MISCE\", NULL AS \"CINA\", NULL AS \"EK_VART\"," +
        //        " NULL AS \"EK_VARTD\",  ID_SFERUPR, NULL AS \"ID_NORMDOC\", NULL AS \"KOD_VIDOBJ\", NULL AS \"KOD_TIPOBJ\"," +
        //        " NULL AS \"KOD_GRNAZN\", NULL AS \"KOD_POVERH\", NULL AS \"KOD_ARHIT\" FROM PRIVOBJ ORDER BY ID, ID_STAN";

        //    MigratePivatizationObjectsFromTable(querySelect, purchasers, processedObjects, false, primaryObjects);
        //}

        //private void MigratePivatizationObjectsFromTable(string querySelect,
        //    Dictionary<int, int> purchasers, HashSet<int> processedObjects,
        //    bool processingPrimaryObjects, HashSet<int> primaryObjects)
        //{
        //    Dictionary<string, object> values = new Dictionary<string, object>();

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    object dataId = Short2Int(reader.IsDBNull(0) ? null : reader.GetValue(0));
        //                    object dataLinkedObj = Short2Int(reader.IsDBNull(1) ? null : reader.GetValue(1));
        //                    object dataIdOrg = Short2Int(reader.IsDBNull(2) ? null : reader.GetValue(2));
        //                    object dataName = reader.IsDBNull(3) ? "" : reader.GetValue(3);
        //                    object dataZipCode = reader.IsDBNull(4) ? "" : reader.GetValue(4);
        //                    object dataAddrNomer = reader.IsDBNull(5) ? "" : reader.GetValue(5);
        //                    object dataAddrMisc = reader.IsDBNull(6) ? "" : reader.GetValue(6);
        //                    object dataDistrId = Short2Int(reader.IsDBNull(7) ? null : reader.GetValue(7));
        //                    object dataSqr = reader.IsDBNull(8) ? null : reader.GetValue(8);
        //                    object dataAddrStreet = reader.IsDBNull(9) ? "" : reader.GetValue(9);
        //                    object dataGroupId = Short2Int(reader.IsDBNull(10) ? null : reader.GetValue(10));
        //                    object dataPrivKindId = Short2Int(reader.IsDBNull(11) ? null : reader.GetValue(11));
        //                    //object dataPrivStateId = reader.IsDBNull(12) ? null : reader.GetValue(12);
        //                    object dataModifyDate = reader.IsDBNull(12) ? null : reader.GetValue(12);
        //                    object dataModifiedBy = reader.IsDBNull(13) ? null : reader.GetValue(13);
        //                    object dataComplex = Short2Int(reader.IsDBNull(14) ? null : reader.GetValue(14));
        //                    object dataMiscInfo = reader.IsDBNull(15) ? null : reader.GetValue(15);
        //                    object dataCost = reader.IsDBNull(16) ? null : reader.GetValue(16);
        //                    object dataCostExpert = reader.IsDBNull(17) ? null : reader.GetValue(17);
        //                    object dataDateExpert = reader.IsDBNull(18) ? null : reader.GetValue(18);
        //                    object dataSubordination = Short2Int(reader.IsDBNull(19) ? null : reader.GetValue(19));
        //                    object dataDocId = Short2Int(reader.IsDBNull(20) ? null : reader.GetValue(20));
        //                    object dataObjKind = Short2Int(reader.IsDBNull(21) ? null : reader.GetValue(21));
        //                    object dataObjType = Short2Int(reader.IsDBNull(22) ? null : reader.GetValue(22));
        //                    object dataPurpose = Short2Int(reader.IsDBNull(23) ? null : reader.GetValue(23));
        //                    object dataFloor = Short2Int(reader.IsDBNull(24) ? null : reader.GetValue(24));
        //                    object dataArhit = Short2Int(reader.IsDBNull(25) ? null : reader.GetValue(25));

        //                    // Avoid zero entries in dictionaries
        //                    if (dataGroupId is int && (int)dataGroupId == 0)
        //                    {
        //                        dataGroupId = null;
        //                    }

        //                    if (dataPrivKindId is int && (int)dataPrivKindId == 0)
        //                    {
        //                        dataPrivKindId = null;
        //                    }

        //                    if (dataComplex is int && (int)dataComplex == 0)
        //                    {
        //                        dataComplex = null;
        //                    }

        //                    if (dataSubordination is int && (int)dataSubordination == 0)
        //                    {
        //                        dataSubordination = null;
        //                    }

        //                    if (dataObjKind is int && (int)dataObjKind == 0)
        //                    {
        //                        dataObjKind = null;
        //                    }

        //                    if (dataObjType is int && (int)dataObjType == 0)
        //                    {
        //                        dataObjType = null;
        //                    }

        //                    if (dataPurpose is int && (int)dataPurpose == 0)
        //                    {
        //                        dataPurpose = null;
        //                    }

        //                    if (dataFloor is int && (int)dataFloor == 0)
        //                    {
        //                        dataFloor = null;
        //                    }

        //                    if (dataArhit is int && (int)dataArhit == 0)
        //                    {
        //                        dataArhit = null;
        //                    }

        //                    if (dataDistrId is int && (int)dataDistrId == 0)
        //                    {
        //                        dataDistrId = null;
        //                    }

        //                    // Find the corresponding object in 1 NF
        //                    if (dataId is int && dataLinkedObj is int)
        //                    {
        //                        int objectId = (int)dataId;

        //                        // Do not overwrite the primary objects
        //                        if (!processingPrimaryObjects && primaryObjects.Contains(objectId))
        //                        {
        //                            continue;
        //                        }

        //                        // Save IDs of primary objects
        //                        if (processingPrimaryObjects)
        //                        {
        //                            primaryObjects.Add(objectId);
        //                        }

        //                        // Save the ID of related primary document
        //                        if (dataDocId is int)
        //                        {
        //                            privatNormDocIdByObjectId[objectId] = (int)dataDocId;
        //                        }

        //                        // Find the related 1 NF object
        //                        int objId1NF = FindPrivatObjectIn1NF((int)dataLinkedObj);

        //                        if (objId1NF > 0)
        //                        {
        //                            // Save the mapping beween objects in Privatizacia and 1 NF
        //                            mapObjectPrivatTo1NF[objectId] = objId1NF;

        //                            // Take the organization ID from ZAJ table (read in on step 3 above)
        //                            int actualPurchaserId = -1;

        //                            if (purchasers.TryGetValue(objectId, out actualPurchaserId))
        //                            {
        //                                dataIdOrg = actualPurchaserId;
        //                            }

        //                            // Find the organization Id in 1NF
        //                            object orgId = null;

        //                            if (dataIdOrg is int && orgIdMappingPrivatTo1NF.ContainsKey((int)dataIdOrg))
        //                            {
        //                                int orgId1NF = 0;

        //                                orgIdMappingPrivatTo1NF.TryGetValue((int)dataIdOrg, out orgId1NF);

        //                                orgId = orgId1NF;
        //                            }

        //                            if (dataIdOrg is int && orgId == null)
        //                            {
        //                                logger.WriteInfo("Organization " + dataIdOrg.ToString() + " is not mapped to 1NF");
        //                            }

        //                            // Map the object kind
        //                            if (dataObjKind is int && objKindMappingPrivatTo1NF.ContainsKey((int)dataObjKind))
        //                            {
        //                                dataObjKind = objKindMappingPrivatTo1NF[(int)dataObjKind];
        //                            }

        //                            // Map the object type
        //                            if (dataObjType is int && objTypeMappingPrivatTo1NF.ContainsKey((int)dataObjType))
        //                            {
        //                                dataObjType = objTypeMappingPrivatTo1NF[(int)dataObjType];
        //                            }

        //                            // Create a new entry in the 'privatization' table
        //                            values.Clear();

        //                            values["id"] = objectId;
        //                            values["building_id"] = objId1NF;
        //                            values["organization_id"] = orgId;
        //                            values["sqr_total"] = dataSqr;
        //                            values["obj_group_id"] = dataGroupId;
        //                            values["obj_name"] = dataName;
        //                            values["privat_kind_id"] = dataPrivKindId;
        //                            values["complex_id"] = dataComplex;
        //                            values["note"] = dataMiscInfo;
        //                            values["cost"] = dataCost;
        //                            values["cost_expert"] = dataCostExpert;
        //                            values["expert_date"] = dataDateExpert;
        //                            values["subordination_id"] = dataSubordination;
        //                            values["object_kind_id"] = dataObjKind;
        //                            values["object_type_id"] = dataObjType;
        //                            values["purpose_group_id"] = dataPurpose;
        //                            values["obj_floor"] = dataFloor;
        //                            values["history_id"] = dataArhit;
        //                            values["modified_by"] = dataModifiedBy;
        //                            values["modify_date"] = dataModifyDate;
        //                            values["addr_street_name"] = dataAddrStreet;
        //                            values["addr_number"] = dataAddrNomer;
        //                            values["addr_misc"] = dataAddrMisc;
        //                            values["addr_district_id"] = dataDistrId;
        //                            values["addr_zip_code"] = dataZipCode;

        //                            if (privatBalansIdByObjectId.ContainsKey((int)dataId))
        //                            {
        //                                values["balans_id"] = privatBalansIdByObjectId[(int)dataId];
        //                            }

        //                            if (privatStateIdByObjectId.ContainsKey((int)dataId))
        //                            {
        //                                values["privat_state_id"] = privatStateIdByObjectId[(int)dataId];
        //                            }

        //                            // Add the 'privatization' object
        //                            string queryInsertOrUpdate = "";

        //                            if (processedObjects.Contains(objectId))
        //                            {
        //                                // This is a new state of an existing object; UPDATE it
        //                                queryInsertOrUpdate = GetUpdateStatement("privatization", values, "id", objectId);
        //                            }
        //                            else
        //                            {
        //                                // This is a new object; INSERT it
        //                                queryInsertOrUpdate = GetInsertStatement("privatization", values);

        //                                // Make a note that this object is processed
        //                                processedObjects.Add(objectId);
        //                            }

        //                            // Execute INSERT or UPDATE
        //                            try
        //                            {
        //                                using (SqlCommand command = new SqlCommand(queryInsertOrUpdate, connectionSqlClient))
        //                                {
        //                                    command.ExecuteNonQuery();
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                ShowSqlErrorMessageDlg(queryInsertOrUpdate, ex.Message);

        //                                // Abort migration correctly
        //                                throw new MigrationAbortedException();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            // Object not found; write it to the CVS log (if it is a new object)
        //                            if (!processedObjects.Contains(objectId))
        //                            {
        //                                csvLogUnmatchedPrivatObj.WriteCell((dataName is string) ? (string)dataName : "");
        //                                csvLogUnmatchedPrivatObj.WriteCell((dataZipCode is string) ? (string)dataZipCode : "");
        //                                csvLogUnmatchedPrivatObj.WriteCell((dataAddrStreet is string) ? (string)dataAddrStreet : "");
        //                                csvLogUnmatchedPrivatObj.WriteCell((dataAddrNomer is string) ? (string)dataAddrNomer : "");
        //                                csvLogUnmatchedPrivatObj.WriteCell((dataAddrMisc is string) ? (string)dataAddrMisc : "");
        //                                csvLogUnmatchedPrivatObj.WriteEndOfLine();

        //                                processedObjects.Add(objectId);
        //                            }
        //                        }
        //                    }
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
        //}

        //private int FindPrivatObjectIn1NF(int linkedObjId)
        //{
        //    // If mapping is specified in the 'Privatizacia' database, use this mapping
        //    if (privatizaciaObjects.ContainsKey(linkedObjId))
        //    {
        //        PrivatizaciaObjInfo info = privatizaciaObjects[linkedObjId];

        //        if (info.objectId1NF is int)
        //        {
        //            return (int)info.objectId1NF;
        //        }
        //    }

        //    // Object not found
        //    return -1;
        //}

        //private int GetNewPrivatizationDocId(int documentId)
        //{
        //    return documentId + privatizationDocIdSeed;
        //}

        //private void MigratePrivatizationDocuments()
        //{
        //    ///////////////////////////////////////////////////////////////////
        //    // 1) Migrate primary documents from the NORMDOC table
        //    ///////////////////////////////////////////////////////////////////

        //    logger.WriteInfo("Migrating documents from the table NORMDOC");

        //    Dictionary<string, object> values = new Dictionary<string, object>();

        //    // Prepare the SQL statement to get all the documents
        //    string querySelect = "SELECT ID, NAME_DOC, NOMER, DT_DOC, ID_VID_DOC, ISP, DTISP FROM NORMDOC";

        //    // Get all the documents
        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Get the document data
        //                    object dataId = Short2Int(reader.IsDBNull(0) ? null : reader.GetValue(0));
        //                    object dataName = reader.IsDBNull(1) ? null : reader.GetValue(1);
        //                    object dataDocNum = reader.IsDBNull(2) ? null : reader.GetValue(2);
        //                    object dataDocDate = reader.IsDBNull(3) ? null : reader.GetValue(3);
        //                    object dataDocKind = Short2Int(reader.IsDBNull(4) ? null : reader.GetValue(4));
        //                    object dataModifiedBy = reader.IsDBNull(5) ? null : reader.GetValue(5);
        //                    object dataModifyDate = reader.IsDBNull(6) ? null : reader.GetValue(6);

        //                    if (dataId is int)
        //                    {
        //                        // Try to find an existing document
        //                        int existingDocId = -1;

        //                        if (dataDocDate is DateTime && dataDocNum is string && dataDocKind is int)
        //                        {
        //                            existingDocId = docFinder.FindDocumentIn1NF((DateTime)dataDocDate, (string)dataDocNum,
        //                                (int)dataDocKind, (dataName is string) ? (string)dataName : "");
        //                        }

        //                        if (existingDocId > 0)
        //                        {
        //                            // Just save the mapping
        //                            mapDocumentsPrivatTo1NF.Add((int)dataId, existingDocId);

        //                            // Set the document category to 'Privatization'
        //                            docFinder.ModifyDocGeneralKind(connectionSqlClient, existingDocId,
        //                                docGeneralCategoryPrivatization);
        //                        }
        //                        else
        //                        {
        //                            // Prepare values for inserting this document into SQL Server table
        //                            int newDocumentId = GetNewPrivatizationDocId((int)dataId);

        //                            values["id"] = newDocumentId;
        //                            values["kind_id"] = dataDocKind;
        //                            values["general_kind_id"] = docGeneralCategoryPrivatization;
        //                            values["doc_date"] = dataDocDate;
        //                            values["doc_num"] = dataDocNum;
        //                            values["topic"] = dataName;
        //                            values["modified_by"] = dataModifiedBy;
        //                            values["modify_date"] = dataModifyDate;

        //                            // Format the INSERT statement for adding this document to our database
        //                            string queryInsert = GetInsertStatement("documents", values);

        //                            // Execute the INSERT statement
        //                            try
        //                            {
        //                                using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionSqlClient))
        //                                {
        //                                    commandInsert.ExecuteNonQuery();
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                ShowSqlErrorMessageDlg(queryInsert, ex.Message);

        //                                // Abort migration correctly
        //                                throw new MigrationAbortedException();
        //                            }

        //                            // Save the mapping between old ID and the new ID
        //                            mapDocumentsPrivatTo1NF.Add((int)dataId, newDocumentId);

        //                            // Register the inserted document in the Document Finder, for future lookup
        //                            docFinder.RegisterDocument(newDocumentId, dataDocDate, dataDocNum, dataDocKind, dataName, true);
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

        //    ///////////////////////////////////////////////////////////////////
        //    // 2) Migrate Agreements and Acts from the DOC table
        //    ///////////////////////////////////////////////////////////////////

        //    logger.WriteInfo("Migrating documents from the table DOC");

        //    values.Clear();

        //    // Prepare the SQL statement to get all the documents
        //    querySelect = "SELECT ID, ID_KMOBJ, NAME_DOC, NOMER, DT_DOC, SERIA, ID_VID_DOC, ISP, DTISP FROM DOC";
        //    querySelect += " WHERE NOT (DT_DOC IS NULL)";

        //    // Get all the documents
        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Get the document data
        //                    object dataId = Short2Int(reader.IsDBNull(0) ? null : reader.GetValue(0));
        //                    object dataObjectId = Short2Int(reader.IsDBNull(1) ? null : reader.GetValue(1));
        //                    object dataName = reader.IsDBNull(2) ? null : reader.GetValue(2);
        //                    object dataDocNum = reader.IsDBNull(3) ? null : reader.GetValue(3);
        //                    object dataDocDate = reader.IsDBNull(4) ? null : reader.GetValue(4);
        //                    object dataSerial = reader.IsDBNull(5) ? null : reader.GetValue(5);
        //                    object dataDocKind = Short2Int(reader.IsDBNull(6) ? null : reader.GetValue(6));
        //                    object dataModifiedBy = reader.IsDBNull(7) ? null : reader.GetValue(7);
        //                    object dataModifyDate = reader.IsDBNull(8) ? null : reader.GetValue(8);

        //                    if (dataId is int)
        //                    {
        //                        // If document contains no title, set the default title
        //                        if (dataName is string && ((string)dataName).Trim().Length == 0)
        //                        {
        //                            dataName = null;
        //                        }

        //                        if (dataName == null && dataDocKind is int)
        //                        {
        //                            object title = FindDictionaryEntryNameInSQLServer("dict_doc_kind", "id", "name", (int)dataDocKind);

        //                            if (title is string)
        //                            {
        //                                dataName = title;
        //                            }
        //                        }

        //                        // Try to find an existing document
        //                        int existingDocId = -1;

        //                        if (dataDocDate is DateTime && dataDocNum is string && dataDocKind is int)
        //                        {
        //                            existingDocId = docFinder.FindDocumentIn1NF((DateTime)dataDocDate, (string)dataDocNum,
        //                                (int)dataDocKind, (dataName is string) ? (string)dataName : "");
        //                        }

        //                        if (existingDocId > 0)
        //                        {
        //                            // Just save the mapping
        //                            mapDocumentsPrivatTo1NF.Add((int)dataId, existingDocId);

        //                            // Set the document category to 'Privatization'
        //                            docFinder.ModifyDocGeneralKind(connectionSqlClient, existingDocId,
        //                                docGeneralCategoryPrivatization);
        //                        }
        //                        else
        //                        {
        //                            // Prepare values for inserting this document into SQL Server table
        //                            existingDocId = GetNewPrivatizationDocId((int)dataId) - 9000;

        //                            values["id"] = existingDocId;
        //                            values["kind_id"] = dataDocKind;
        //                            values["general_kind_id"] = docGeneralCategoryPrivatization;
        //                            values["doc_date"] = dataDocDate;
        //                            values["doc_num"] = dataDocNum;
        //                            values["serial_num"] = dataSerial;
        //                            values["topic"] = dataName;
        //                            values["modified_by"] = dataModifiedBy;
        //                            values["modify_date"] = dataModifyDate;

        //                            // Format the INSERT statement for adding this document to our database
        //                            string queryInsert = GetInsertStatement("documents", values);

        //                            // Execute the INSERT statement
        //                            try
        //                            {
        //                                using (SqlCommand commandInsert = new SqlCommand(queryInsert, connectionSqlClient))
        //                                {
        //                                    commandInsert.ExecuteNonQuery();
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                ShowSqlErrorMessageDlg(queryInsert, ex.Message);

        //                                // Abort migration correctly
        //                                throw new MigrationAbortedException();
        //                            }

        //                            // Save the mapping between old ID and the new ID
        //                            mapDocumentsPrivatTo1NF.Add((int)dataId, existingDocId);

        //                            // Register the inserted document in the Document Finder, for future lookup
        //                            docFinder.RegisterDocument(existingDocId, dataDocDate, dataDocNum, dataDocKind, dataName, true);
        //                        }

        //                        // Save a relation between object and document
        //                        if (dataObjectId is int && mapObjectPrivatTo1NF.ContainsKey((int)dataObjectId))
        //                        {
        //                            docFinder.AddPrivatizationDocLinkToObject(connectionSqlClient,
        //                                (int)dataObjectId, existingDocId, null, dataModifiedBy, dataModifyDate);
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

        //private void MigratePrivatizationDocLinks()
        //{
        //    logger.WriteInfo("Migrating document links from the table NDOC_INHERITED");

        //    // Prepare the SQL statement to get all the documents
        //    string querySelect = "SELECT ID_PARENT, ID_CHILD, ISP, DTISP FROM NDOC_INHERITED";

        //    // Get all the document links
        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Get the document data
        //                    object dataParentId = Short2Int(reader.IsDBNull(0) ? null : reader.GetValue(0));
        //                    object dataChildId = Short2Int(reader.IsDBNull(1) ? null : reader.GetValue(1));
        //                    object dataModifiedBy = reader.IsDBNull(2) ? null : reader.GetValue(2);
        //                    object dataModifyDate = reader.IsDBNull(3) ? null : reader.GetValue(3);

        //                    if (dataParentId is int && dataChildId is int)
        //                    {
        //                        int parentId1NF = -1;
        //                        int childId1NF = -1;

        //                        if (mapDocumentsPrivatTo1NF.TryGetValue((int)dataParentId, out parentId1NF) &&
        //                            mapDocumentsPrivatTo1NF.TryGetValue((int)dataChildId, out childId1NF))
        //                        {
        //                            docFinder.AddDocLinkToDoc(connectionSqlClient,
        //                                parentId1NF, childId1NF, dataModifiedBy, dataModifyDate, 1); // dependent document
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

        //private void MigratePrivatizationObjDocLinks()
        //{
        //    logger.WriteInfo("Migrating links between objects and documents from the table OBJDOC");

        //    /*
        //    foreach (KeyValuePair<int, int> pair in privatNormDocIdByObjectId)
        //    {
        //        int documentId1NF = -1;

        //        if (mapDocumentsPrivatTo1NF.TryGetValue(pair.Value, out documentId1NF))
        //        {
        //            if (mapObjectPrivatTo1NF.ContainsKey(pair.Key))
        //            {
        //                docFinder.AddPrivatizationDocLinkToObject(connectionSqlClient,
        //                    pair.Key, documentId1NF, null, null);
        //            }
        //        }
        //    }
        //    */

        //    Dictionary<string, object> values = new Dictionary<string, object>();

        //    // Prepare the SQL statement to get all the document objects
        //    string querySelect = "SELECT ID, ID_PRIVOBJ, ID_NORMDOC, PARENTDOCID, PRIM, DODNO, ISP, DTISP FROM OBJDOC";

        //    // Get all the links
        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(querySelect, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Get the document data
        //                    object dataLinkId = reader.IsDBNull(0) ? null : reader.GetValue(0);
        //                    object dataObjectId = reader.IsDBNull(1) ? null : reader.GetValue(1);
        //                    object dataDocId = reader.IsDBNull(2) ? null : reader.GetValue(2);
        //                    object dataMasterDocId = reader.IsDBNull(3) ? null : reader.GetValue(3);
        //                    object dataNote = reader.IsDBNull(4) ? null : reader.GetValue(4);
        //                    object dataDodatok = reader.IsDBNull(5) ? null : reader.GetValue(5);
        //                    object dataModifiedBy = reader.IsDBNull(6) ? null : reader.GetValue(6);
        //                    object dataModifyDate = reader.IsDBNull(7) ? null : reader.GetValue(7);

        //                    if (dataLinkId is int && dataObjectId is int && dataDocId is int)
        //                    {
        //                        int documentId = (int)dataDocId;

        //                        // Check if object and the document exist
        //                        int docId1NF = 0;
        //                        int masterDocId1NF = 0;

        //                        if (mapDocumentsPrivatTo1NF.TryGetValue(documentId, out docId1NF) &&
        //                            mapObjectPrivatTo1NF.ContainsKey((int)dataObjectId))
        //                        {
        //                            if (dataMasterDocId is int)
        //                            {
        //                                if (mapDocumentsPrivatTo1NF.TryGetValue((int)dataMasterDocId, out masterDocId1NF))
        //                                {
        //                                    dataMasterDocId = masterDocId1NF;
        //                                }
        //                            }

        //                            docFinder.AddPrivatizationDocLinkToObject(connectionSqlClient,
        //                                (int)dataObjectId, docId1NF, dataMasterDocId, dataModifiedBy, dataModifyDate);
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

        //private void MarkDocumentAsPrivatizationPrimary(int docId)
        //{
        //    Dictionary<string, object> values = new Dictionary<string, object>();

        //    values["is_priv_rishen"] = 1;

        //    string statement = GetUpdateStatement("documents", values, "id", docId);

        //    // Execute the UPDATE statement
        //    try
        //    {
        //        using (SqlCommand commandUpdate = new SqlCommand(statement, connectionSqlClient))
        //        {
        //            commandUpdate.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowSqlErrorMessageDlg(statement, ex.Message);

        //        // Abort migration correctly
        //        throw new MigrationAbortedException();
        //    }
        //}

        //private void MigratePrivatRishenDocsForObjects()
        //{
        //    Dictionary<string, object> values = new Dictionary<string, object>();

        //    foreach (KeyValuePair<int, int> pair in privatNormDocIdByObjectId)
        //    {
        //        int documentId1NF = -1;

        //        if (mapDocumentsPrivatTo1NF.TryGetValue(pair.Value, out documentId1NF))
        //        {
        //            if (mapObjectPrivatTo1NF.ContainsKey(pair.Key))
        //            {
        //                values["rishen_doc_id"] = documentId1NF;

        //                string statement = GetUpdateStatement("privatization", values, "id", pair.Key);

        //                // Execute the UPDATE statement
        //                try
        //                {
        //                    using (SqlCommand commandUpdate = new SqlCommand(statement, connectionSqlClient))
        //                    {
        //                        commandUpdate.ExecuteNonQuery();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    ShowSqlErrorMessageDlg(statement, ex.Message);

        //                    // Abort migration correctly
        //                    throw new MigrationAbortedException();
        //                }
        //            }
        //        }
        //    }
        //}

        //private void MarkPrivatizationPrimaryDocs()
        //{
        //    HashSet<int> primaryDocuments = new HashSet<int>();

        //    string query = "SELECT ID FROM MAINDOCVIEW";

        //    try
        //    {
        //        using (FbCommand commandSelect = new FbCommand(query, connectionPrivatizacia))
        //        {
        //            using (FbDataReader reader = commandSelect.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    object dataDocId = Short2Int(reader.IsDBNull(0) ? null : reader.GetValue(0));

        //                    if (dataDocId is int)
        //                    {
        //                        int docId1NF = 0;

        //                        if (mapDocumentsPrivatTo1NF.TryGetValue((int)dataDocId, out docId1NF))
        //                        {
        //                            primaryDocuments.Add(docId1NF);
        //                        }
        //                    }
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowSqlErrorMessageDlg(query, ex.Message);

        //        // Abort migration correctly
        //        throw new MigrationAbortedException();
        //    }

        //    // Set the "is_priv_rishen" flag for all such documents
        //    foreach (int docId in primaryDocuments)
        //    {
        //        query = "UPDATE documents SET is_priv_rishen = 1 WHERE id = " + docId.ToString();

        //        try
        //        {
        //            using (SqlCommand commandUpdate = new SqlCommand(query, connectionSqlClient))
        //            {
        //                commandUpdate.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ShowSqlErrorMessageDlg(query, ex.Message);

        //            // Abort migration correctly
        //            throw new MigrationAbortedException();
        //        }
        //    }
        //}

        //private object Short2Int(object number)
        //{
        //    if (number is short)
        //    {
        //        int i = (short)number;

        //        return i;
        //    }

        //    return number;
        //}

        //#endregion (Privatizacia database migration)

//        #region Address mismatch report generation

//        private void GeneratePrivatizationAddrMismatchReport()
//        {
//            logger.WriteInfo("Generating the address mismatch report for Privatizacia database");

//            CSVLog csvLogByNumber = new CSVLog("CSV/PrivatAddrNumMismatch.csv");
//            CSVLog csvLogByStreet = new CSVLog("CSV/PrivatAddrStreetMismatch.csv");

//            // Write a header to the CSV logs
//            csvLogByNumber.WriteCell(GUKV.DataMigration.Properties.Resources.PrivatCSVLogAddr1NF);
//            csvLogByNumber.WriteCell(GUKV.DataMigration.Properties.Resources.PrivatCSVLogAddrPrivat);
//            csvLogByNumber.WriteEndOfLine();

//            csvLogByStreet.WriteCell(GUKV.DataMigration.Properties.Resources.PrivatCSVLogAddr1NF);
//            csvLogByStreet.WriteCell(GUKV.DataMigration.Properties.Resources.PrivatCSVLogAddrPrivat);
//            csvLogByStreet.WriteEndOfLine();

//            string query = @"SELECT
//                p.id,
//                b.addr_street_name AS 'street1_1nf',
//                b.addr_street_name2 AS 'street2_1nf',
//                b.addr_nomer1 AS 'nomer1_1nf',
//                b.addr_nomer2 AS 'nomer1_2nf',
//                b.addr_nomer3 AS 'nomer1_3nf',
//                b.addr_misc AS 'misc_1nf',
//                p.addr_street_name,
//                p.addr_number,
//                p.addr_misc
//            FROM
//                privatization p
//                LEFT OUTER JOIN buildings b ON b.id = p.building_id";

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        int privatId = reader.GetInt32(0);

//                        string street1NF1 = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim().ToUpper();
//                        string street1NF2 = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
//                        string nomer1NF1 = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim().ToUpper();
//                        string nomer1NF2 = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim().ToUpper();
//                        string nomer1NF3 = reader.IsDBNull(5) ? "" : reader.GetString(5).Trim().ToUpper();
//                        string misc1NF = reader.IsDBNull(6) ? "" : reader.GetString(6).Trim().ToUpper();

//                        string streetPrivat = reader.IsDBNull(7) ? "" : reader.GetString(7).Trim().ToUpper();
//                        string nomerPrivat = reader.IsDBNull(8) ? "" : reader.GetString(8).Trim().ToUpper();
//                        string miscPrivat = reader.IsDBNull(9) ? "" : reader.GetString(9).Trim().ToUpper();

//                        // Format the addresses for the log
//                        if (miscPrivat.Length > 0)
//                        {
//                            if (!miscPrivat.StartsWith(Properties.Resources.BuildingLitPrefix))
//                            {
//                                miscPrivat = Properties.Resources.BuildingLitPrefix + " " + miscPrivat;
//                            }
//                        }

//                        string street1NF = street1NF1;

//                        if (street1NF2.Length > 0)
//                        {
//                            street1NF += "/" + street1NF2;
//                        }

//                        string address1NF = street1NF + " " + nomer1NF1 + " " + nomer1NF2 + " " + nomer1NF3 + " " + misc1NF;
//                        string addressPrivat = streetPrivat + " " + nomerPrivat + " " + miscPrivat;

//                        // Check streets
//                        string street1NFStripped = street1NF.Replace(" ", "");
//                        string streetPrivatStripped = streetPrivat.Replace(" ", "");

//                        if (street1NFStripped != streetPrivatStripped)
//                        {
//                            // Streets are different; write to thelog of streets
//                            csvLogByStreet.WriteCell(address1NF);
//                            csvLogByStreet.WriteCell(addressPrivat);
//                            csvLogByStreet.WriteEndOfLine();
//                        }
//                        else
//                        {
//                            // Check building numbers
//                            string fullAddress1NF = "";
//                            string simpleAddress1NF = "";
//                            bool address1NFIsSimple = false;

//                            ObjectFinder.UnifiedAddress uniAddr1NF = ObjectFinder.GetUnifiedAddressEx("STREET", nomer1NF1,
//                                nomer1NF2, nomer1NF3, "", out fullAddress1NF, out simpleAddress1NF, out address1NFIsSimple);

//                            string fullAddressPrivat = "";
//                            string simpleAddressPrivat = "";
//                            bool addressPrivatIsSimple = false;

//                            ObjectFinder.UnifiedAddress uniAddrPrivat = ObjectFinder.GetUnifiedAddressEx("STREET", nomerPrivat,
//                                miscPrivat, "", "", out fullAddressPrivat, out simpleAddressPrivat, out addressPrivatIsSimple);

//                            if (uniAddr1NF == null || uniAddrPrivat == null)
//                            {
//                                // Missing building number; write it to the log
//                                csvLogByNumber.WriteCell(address1NF);
//                                csvLogByNumber.WriteCell(addressPrivat);
//                                csvLogByNumber.WriteEndOfLine();
//                            }
//                            else
//                            {
//                                if (fullAddress1NF != fullAddressPrivat)
//                                {
//                                    // The suffix LIT.A may be missing; it is normal
//                                    bool literaAMissing1 = (uniAddr1NF.litera.Length == 0) && (uniAddrPrivat.litera == Properties.Resources.BuildingLetterA);
//                                    bool literaAMissing2 = (uniAddrPrivat.litera.Length == 0) && (uniAddr1NF.litera == Properties.Resources.BuildingLetterA);

//                                    if (literaAMissing1 || literaAMissing2)
//                                    {
//                                        uniAddr1NF.litera = "";
//                                        uniAddrPrivat.litera = "";

//                                        fullAddress1NF = uniAddr1NF.FormatFullAddress();
//                                        fullAddressPrivat = uniAddrPrivat.FormatFullAddress();
//                                    }

//                                    if (fullAddress1NF != fullAddressPrivat)
//                                    {
//                                        // Building number mismatch; write it to the log
//                                        csvLogByNumber.WriteCell(address1NF);
//                                        csvLogByNumber.WriteCell(addressPrivat);
//                                        csvLogByNumber.WriteEndOfLine();
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    reader.Close();
//                }
//            }
//        }

//        #endregion (Address mismatch report generation)
    }
}
