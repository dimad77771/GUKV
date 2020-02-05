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
    //public partial class Migrator
    //{
    //    #region Member variables

    //    /// <summary>
    //    /// Value of the general_kind_id column for all documents from the Arenda database
    //    /// </summary>
    //    private int docGeneralCategoryArenda = 0;

    //    /// <summary>
    //    /// Auto-incremented Id of 'protocol' documents that are added from the Arenda database
    //    /// </summary>
    //    private int protocolIdSeed = 5000;

    //    /// <summary>
    //    /// Auto-incremented Id of 'rasporadjennia' documents that are added from the Arenda database
    //    /// </summary>
    //    private int rasporIdSeed = 8000;

    //    /// <summary>
    //    /// Mapping of document IDs from the table RASPOR
    //    /// </summary>
    //    private Dictionary<int, int> arendaRasporDocIdMapping = new Dictionary<int, int>();

    //    #endregion (Member variables)

    //    #region Arenda database migration

    //    private void MigrateArenda()
    //    {
    //        logger.WriteInfo("=== Migrating database Arenda ===");

    //        MigrateArendaDictionaries();

    //        MigrateArendaProtocols();
    //        MigrateArendaRaspor();
            
    //        MigrateArendaDodatki("PROTOK", "SPKOD", "DTP", "NOMP", true, protocolIdSeed);
    //        MigrateArendaDodatki("RASPOR", "SRKOD", "DTR", "NOMR", false, rasporIdSeed);

    //        MigrateArendaApplications();
    //        MigrateArendaDecisions();
    //        MigrateArendaDecisionLinks();
    //    }

    //    private void MigrateArendaDictionaries()
    //    {
    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        // SVDOD
    //        fields.Clear();
    //        fields.Add("KOD", "id");
    //        fields.Add("NAME", "name");
    //        fields.Add("ISP", "modified_by");
    //        fields.Add("DT", "modify_date");
    //        fields.Add("VIDDIL", "otdel_gukv_id");

    //        MigrateTable(connection1NF, "SVDOD", "dict_doc_appendix_kind", fields, "KOD", null);

    //        // SSTADIYA
    //        fields.Clear();
    //        fields.Add("KOD", "id");
    //        fields.Add("NAME", "name");
    //        fields.Add("ISP", "modified_by");
    //        fields.Add("DT", "modify_date");

    //        MigrateTable(connection1NF, "SSTADIYA", "dict_doc_stadiya", fields, "KOD", null);

    //        // SOZR
    //        fields.Clear();
    //        fields.Add("KOD", "id");
    //        fields.Add("NAME", "name");
    //        fields.Add("NAMEK", "name_abbr");
    //        fields.Add("ISP", "modified_by");
    //        fields.Add("DT", "modify_date");
    //        fields.Add("NAMEREPORT", "name_report_form");
    //        fields.Add("NAME_EXP", "name_order_form");

    //        MigrateTable(connection1NF, "SOZR", "dict_rent_decisions", fields, "KOD", null);
    //    }

    //    private void MigrateArendaProtocols()
    //    {
    //        logger.WriteInfo("Migrating documents from the table SPROTOK");

    //        Dictionary<string, object> values = new Dictionary<string, object>();

    //        // Get the resource strings
    //        string docTypeProtocol = GUKV.DataMigration.Properties.Resources.DocTypeProtocol;
    //        string protocolTopicDefault = GUKV.DataMigration.Properties.Resources.ProtocolTopicDefault;
    //        string protocolTopicForCity = GUKV.DataMigration.Properties.Resources.ProtocolTopicForCity;
    //        string protocolTopicForConstCommitty = GUKV.DataMigration.Properties.Resources.ProtocolTopicForConstCommitty;
    //        string docNumString = GUKV.DataMigration.Properties.Resources.DocNum;
    //        string docDateString = GUKV.DataMigration.Properties.Resources.DocDate;
    //        string docArchFlagRus = GUKV.DataMigration.Properties.Resources.DocArchiveFlagRus;
    //        string docArchFlagEng = GUKV.DataMigration.Properties.Resources.DocArchiveFlagEng;

    //        // Prepare the field mapping for the source table
    //        Dictionary<string, string> fieldsSrc = new Dictionary<string, string>();

    //        fieldsSrc.Add("KOD", "id");
    //        fieldsSrc.Add("DT", "doc_date");
    //        fieldsSrc.Add("NOM", "doc_num");
    //        fieldsSrc.Add("KOMENTAR", "note");
    //        fieldsSrc.Add("ISP", "modified_by");
    //        fieldsSrc.Add("DTISP", "modify_date");
    //        fieldsSrc.Add("ARXIV", "is_archived");
    //        fieldsSrc.Add("VKOMIS", "commission_id");

    //        // Get the default type of the 'protokol' document
    //        int protocolDokKind = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docTypeProtocol);

    //        if (protocolDokKind < 0)
    //        {
    //            logger.WriteError("MigrateArendaProtocols() method requires " + docTypeProtocol + " document type.");

    //            // Abort migration correctly
    //            throw new MigrationAbortedException();
    //        }

    //        // Prepare the SQL statement to get all the protocols
    //        string querySelect = GetSelectStatement("SPROTOK", fieldsSrc, "KOD", null);

    //        // Get all the documents
    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the document data
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataDocDate = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataDocNum = reader.IsDBNull(2) ? null : reader.GetValue(2);
    //                        object dataNote = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                        object dataModifiedBy = reader.IsDBNull(4) ? null : reader.GetValue(4);
    //                        object dataModifyDate = reader.IsDBNull(5) ? null : reader.GetValue(5);
    //                        object dataArch = reader.IsDBNull(6) ? null : reader.GetValue(6);
    //                        object dataCommissionId = reader.IsDBNull(7) ? null : reader.GetValue(7);

    //                        if (dataId is int)
    //                        {
    //                            // Format the protocol topic
    //                            string topic = protocolTopicDefault;

    //                            if (dataCommissionId is int)
    //                            {
    //                                if ((int)dataCommissionId == 1)
    //                                {
    //                                    topic = protocolTopicForCity;
    //                                }
    //                                else
    //                                {
    //                                    topic = protocolTopicForConstCommitty;
    //                                }
    //                            }

    //                            if (dataDocNum is string)
    //                            {
    //                                topic += " " + docNumString + " " + (string)dataDocNum;
    //                            }

    //                            if (dataDocDate is DateTime)
    //                            {
    //                                DateTime dt = (DateTime)dataDocDate;

    //                                topic += " " + docDateString + " " + dt.ToShortDateString();
    //                            }

    //                            // Check if the document is archived
    //                            int archived = 0;

    //                            if (dataArch is string)
    //                            {
    //                                string isArch = ((string)dataArch).Trim().ToUpper();

    //                                if (isArch == docArchFlagRus || isArch == docArchFlagEng)
    //                                {
    //                                    archived = 1;
    //                                }
    //                            }

    //                            // Prepare values for inserting this document into SQL Server table
    //                            int newDocumentId = (int)dataId + protocolIdSeed;

    //                            values["id"] = newDocumentId;
    //                            values["kind_id"] = protocolDokKind;
    //                            values["general_kind_id"] = docGeneralCategoryArenda;
    //                            values["doc_date"] = dataDocDate;
    //                            values["doc_num"] = dataDocNum;
    //                            values["topic"] = topic;
    //                            values["note"] = dataNote;
    //                            values["modified_by"] = dataModifiedBy;
    //                            values["modify_date"] = dataModifyDate;
    //                            values["is_archived"] = archived;
    //                            values["commission_id"] = dataCommissionId;

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

    //                            // Register the inserted document in the Document Finder, for future lookup
    //                            docFinder.RegisterDocument(newDocumentId, dataDocDate, protocolDokKind, dataDocNum, topic, true);
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

    //    private void MigrateArendaRaspor()
    //    {
    //        logger.WriteInfo("Migrating documents from the table SRASPOR");

    //        Dictionary<string, object> values = new Dictionary<string, object>();

    //        // Get the resource strings
    //        string docTypeOrder = GUKV.DataMigration.Properties.Resources.DocTypeOrderKMDA;
    //        string orderTopicDefault = GUKV.DataMigration.Properties.Resources.OrderTopicKMDADefault;
    //        string docNumString = GUKV.DataMigration.Properties.Resources.DocNum;
    //        string docDateString = GUKV.DataMigration.Properties.Resources.DocDate;
    //        string docArchFlagRus = GUKV.DataMigration.Properties.Resources.DocArchiveFlagRus;
    //        string docArchFlagEng = GUKV.DataMigration.Properties.Resources.DocArchiveFlagEng;

    //        // Prepare the field mapping for the source table
    //        Dictionary<string, string> fieldsSrc = new Dictionary<string, string>();

    //        fieldsSrc.Add("KOD", "id");
    //        fieldsSrc.Add("DT", "doc_date");
    //        fieldsSrc.Add("NOM", "doc_num");
    //        fieldsSrc.Add("KOMENTAR", "note");
    //        fieldsSrc.Add("ISP", "modified_by");
    //        fieldsSrc.Add("DTISP", "modify_date");
    //        fieldsSrc.Add("ARXIV", "is_archived");

    //        // Get the default type of the 'rasporadjennia' document
    //        int rasporDokKind = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docTypeOrder);

    //        if (rasporDokKind < 0)
    //        {
    //            logger.WriteError("MigrateArendaRaspor() method requires " + docTypeOrder + " document type.");

    //            // Abort migration correctly
    //            throw new MigrationAbortedException();
    //        }

    //        // Prepare the SQL statement to get all the 'rasporadjennia' documents
    //        string querySelect = GetSelectStatement("SRASPOR", fieldsSrc, "KOD", null);

    //        // Get all the documents
    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the document data
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataDocDate = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataDocNum = reader.IsDBNull(2) ? null : reader.GetValue(2);
    //                        object dataNote = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                        object dataModifiedBy = reader.IsDBNull(4) ? null : reader.GetValue(4);
    //                        object dataModifyDate = reader.IsDBNull(5) ? null : reader.GetValue(5);
    //                        object dataArch = reader.IsDBNull(6) ? null : reader.GetValue(6);

    //                        if (dataId is int)
    //                        {
    //                            // Format the document topic
    //                            string topic = orderTopicDefault;

    //                            if (dataDocNum is string)
    //                            {
    //                                topic += " " + docNumString + " " + (string)dataDocNum;
    //                            }

    //                            if (dataDocDate is DateTime)
    //                            {
    //                                DateTime dt = (DateTime)dataDocDate;

    //                                topic += " " + docDateString + " " + dt.ToShortDateString();
    //                            }

    //                            // Check if the document is archived
    //                            int archived = 0;

    //                            if (dataArch is string)
    //                            {
    //                                string isArch = ((string)dataArch).Trim().ToUpper();

    //                                if (isArch == docArchFlagRus || isArch == docArchFlagEng)
    //                                {
    //                                    archived = 1;
    //                                }
    //                            }

    //                            // Prepare values for inserting this document into SQL Server table
    //                            int newDocumentId = (int)dataId + rasporIdSeed;

    //                            values["id"] = newDocumentId;
    //                            values["kind_id"] = rasporDokKind;
    //                            values["general_kind_id"] = docGeneralCategoryArenda;
    //                            values["doc_date"] = dataDocDate;
    //                            values["doc_num"] = dataDocNum;
    //                            values["topic"] = topic;
    //                            values["note"] = dataNote;
    //                            values["modified_by"] = dataModifiedBy;
    //                            values["modify_date"] = dataModifyDate;
    //                            values["is_archived"] = archived;

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

    //                            // Save the ID mapping
    //                            arendaRasporDocIdMapping[(int)dataId] = newDocumentId;

    //                            // Register the inserted document in the Document Finder, for future lookup
    //                            docFinder.RegisterDocument(newDocumentId, dataDocDate, dataDocNum, rasporDokKind, topic, true);
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

    //    private void MigrateArendaDodatki(
    //        string srcTableName, /* PROTOK, RASPOR */
    //        string docIdFieldName, /* SPKOD, SRKOD */
    //        string docDateFieldName, /* DTP, DTR */
    //        string docNumFieldName, /* NOMP, NOMR */
    //        bool includeStadiya,
    //        int docIdSeed)
    //    {
    //        logger.WriteInfo("Migrating appendices from the table " + srcTableName);

    //        Dictionary<string, object> values = new Dictionary<string, object>();

    //        // Get the resource strings
    //        string docArchFlagRus = GUKV.DataMigration.Properties.Resources.DocArchiveFlagRus;
    //        string docArchFlagEng = GUKV.DataMigration.Properties.Resources.DocArchiveFlagEng;

    //        // Prepare the field mapping for the source table
    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        fields.Add("KOD", "id");
    //        fields.Add("DT", "app_date");
    //        fields.Add("NOM", "app_doc_num");
    //        fields.Add("DODATOK", "app_num");
    //        fields.Add("DODATOK_USER", "app_num_user");
    //        fields.Add("ARXIV", "is_archived");
    //        fields.Add("VDOD", "kind_id");
    //        fields.Add("ISP", "modified_by");
    //        fields.Add("DTISP", "modify_date");
    //        fields.Add(docIdFieldName, "doc_id");
    //        fields.Add(docDateFieldName, "doc_date");
    //        fields.Add(docNumFieldName, "doc_num");

    //        if (includeStadiya)
    //        {
    //            fields.Add("STADIYA", "stadiya_id");
    //        }

    //        // Prepare the SQL statement to get all the 'rasporadjennia' documents
    //        string querySelect = GetSelectStatement(srcTableName, fields, "KOD", null);

    //        // Get all the appendices
    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        // Get the appendix data
    //                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
    //                        object dataAppDate = reader.IsDBNull(1) ? null : reader.GetValue(1);
    //                        object dataAppDocNum = reader.IsDBNull(2) ? null : reader.GetValue(2);
    //                        object dataAppNum = reader.IsDBNull(3) ? null : reader.GetValue(3);
    //                        object dataAppNumUser = reader.IsDBNull(4) ? null : reader.GetValue(4);
    //                        object dataArch = reader.IsDBNull(5) ? null : reader.GetValue(5);
    //                        object dataKindId = reader.IsDBNull(6) ? null : reader.GetValue(6);
    //                        object dataModifiedBy = reader.IsDBNull(7) ? null : reader.GetValue(7);
    //                        object dataModifyDate = reader.IsDBNull(8) ? null : reader.GetValue(8);
    //                        object dataDocId = reader.IsDBNull(9) ? null : reader.GetValue(9);
    //                        object dataDocDate = reader.IsDBNull(10) ? null : reader.GetValue(10);
    //                        object dataDocNum = reader.IsDBNull(11) ? null : reader.GetValue(11);
    //                        object dataStadiya = null;

    //                        if (includeStadiya)
    //                        {
    //                            dataStadiya = reader.IsDBNull(12) ? null : reader.GetValue(12);
    //                        }

    //                        if (dataId is int && dataDocId is int)
    //                        {
    //                            // Check if the appendix is archived
    //                            int archived = 0;

    //                            if (dataArch is string)
    //                            {
    //                                string isArch = ((string)dataArch).Trim().ToUpper();

    //                                if (isArch == docArchFlagRus || isArch == docArchFlagEng)
    //                                {
    //                                    archived = 1;
    //                                }
    //                            }

    //                            // Prepare values for inserting this document into SQL Server table
    //                            values["id"] = (int)dataId;
    //                            values["app_date"] = dataAppDate;
    //                            values["app_doc_num"] = dataAppDocNum;
    //                            values["app_num"] = dataAppNum;
    //                            values["app_num_user"] = dataAppNumUser;
    //                            values["is_archived"] = archived;
    //                            values["kind_id"] = dataKindId;
    //                            values["modified_by"] = dataModifiedBy;
    //                            values["modify_date"] = dataModifyDate;
    //                            values["doc_id"] = (int)dataDocId + docIdSeed;
    //                            values["doc_date"] = dataDocDate;
    //                            values["doc_num"] = dataDocNum;
    //                            values["stadiya_id"] = dataStadiya;

    //                            // Format the INSERT statement for adding this document to our database
    //                            string queryInsert = GetInsertStatement("doc_appendices", values);

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

    //    private void MigrateArendaApplications()
    //    {
    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        // ZAMOV
    //        fields.Clear();

    //        fields.Add("KOD", "id");
    //        fields.Add("ARNAME", "renter_name");
    //        fields.Add("NAZ", "rent_goal");
    //        fields.Add("STROK", "rent_term");
    //        fields.Add("OBJ", "building_id");

    //        fields.Add("NOMVLU", "appl_letter_num");
    //        fields.Add("DTVLU", "appl_letter_date");
    //        fields.Add("ISP", "modified_by");
    //        fields.Add("DTISP", "modify_date");

    //        fields.Add("S_ZAMOV", "appl_sqr");
    //        fields.Add("SYSTEM_AR", "org_renter_id");
    //        fields.Add("OZNSUBOREND", "subarenda_id");

    //        MigrateTable(connection1NF, "ZAMOV", "arenda_applications", fields, "KOD", null);
    //    }

    //    private void MigrateArendaDecisions()
    //    {
    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        // BULK migrate the table RISHEN, where all decisions for rent are stored
    //        fields.Add("KOD", "id");
    //        fields.Add("OZR", "decision_id");
    //        fields.Add("ETR", "etr");
    //        fields.Add("STROK", "rent_term");
    //        fields.Add("PR_STROK", "rent_term_note");
    //        fields.Add("NPP", "rishen_punkt");

    //        fields.Add("NAZVA", "title");
    //        fields.Add("RDNM", "rent_purpose");
    //        fields.Add("NOVPROT", "nov_prot");
    //        fields.Add("OBJ", "building_id");
    //        fields.Add("SYSTEM_AR", "org_renter_id");
    //        fields.Add("SYSTEM_ARQ", "org_balans_id");
    
    //        fields.Add("ORGIV", "org_giver_id");
    //        fields.Add("BALANS_ID", "balans_id");
    //        fields.Add("DTILU", "apply_date");
    //        fields.Add("KODILU", "apply_letter_id");
    //        fields.Add("NOMILU", "apply_letter_num");

    //        fields.Add("KODP", "appendix_prot_id");
    //        fields.Add("KODR", "appendix_rasp_id");
    //        fields.Add("KODZ", "application_id");
    //        fields.Add("K_KOMN", "num_rooms");
    //        fields.Add("S_ZAMOV", "sqr_applied");
    //        fields.Add("S_ROB", "sqr_robocha");

    //        fields.Add("VART_1M", "cost_1m");
    //        fields.Add("PLATA", "cost_rent");
    //        fields.Add("OZNSUBOREND", "is_subarenda");
    //        fields.Add("STADIYA", "stadiya_id");
    //        fields.Add("ISP", "modified_by");
    //        fields.Add("DTISP", "modify_date");

    //        if (!testOnly)
    //        {
    //            fields.Add("TRISH", "blob_rish_etap1");
    //            fields.Add("TRISHP", "blob_rish2_etap1");
    //            fields.Add("NAZ", "blob_nazva");
    //            fields.Add("TRISH1", "blob_rish_etap2");
    //            fields.Add("TRISHP1", "blob_rish2_etap2");
    //        }

    //        MigrateTable(connection1NF, "RISHEN", "arenda_decisions", fields, "KOD", null);

    //        // Some post-processing of the loaded data is performed in 'DataMigrationPostProcessing.sql' script

    //        // RISHOBJ
    //        fields.Clear();

    //        if (!testOnly)
    //        {
    //            fields.Add("NAZ", "blob_name");
    //            fields.Add("ROZTASH", "blob_location");
    //        }

    //        fields.Add("KODZ", "application_id");
    //        fields.Add("RISHEN", "rishen_id");
    //        fields.Add("OBJ", "building_id");
    //        fields.Add("POVERCH", "poverh");

    //        fields.Add("ISP", "modified_by");
    //        fields.Add("DTISP", "modify_date");
    //        fields.Add("PL", "obj_square");
    //        fields.Add("STAVKA", "stavka");
    //        fields.Add("STAVKAM", "stavka_m");

    //        MigrateTable(connection1NF, "RISHOBJ", "arenda_decision_objects", fields, "KOD", null);
    //    }

    //    private void MigrateArendaDecisionLinks()
    //    {
    //        logger.WriteInfo("Migrating links between ARENDA and RASPOR tables (from the ARRISH table)");

    //        Dictionary<string, string> fields = new Dictionary<string, string>();

    //        fields.Add("ARENDA", "arenda_id");
    //        fields.Add("RISHEN", "rishen_id");
    //        fields.Add("ORD", "ord");
    //        fields.Add("ISP", "modified_by");
    //        fields.Add("DT", "modify_date");

    //        fields.Add("NOM", "doc_num");
    //        fields.Add("DT_DOC", "doc_date");
    //        fields.Add("DODATOK", "doc_dodatok");
    //        fields.Add("PUNKT", "doc_punkt");
    //        fields.Add("PURP_STR", "purpose_str");

    //        fields.Add("SQUARE", "rent_square");
    //        fields.Add("OZR", "decision_id");
    //        fields.Add("RASPOR", "doc_raspor_id");
    //        fields.Add("PIDSTAVA", "pidstava");

    //        string querySelect = GetSelectStatement("ARRISH", fields, "ID", null);

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    DataReaderAdapter adapter = new DataReaderAdapter(reader);

    //                    // Map RASPOR IDs from Arenda to our database
    //                    adapter.AddColumnMapping("RASPOR", arendaRasporDocIdMapping);

    //                    BulkMigrateDataReader("ARRISH", "link_arenda_2_decisions", adapter, fields);

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

    //        // Migrate the TAR_ARRISH table - archive states for the ARRISH table
    //        fields.Remove("ARENDA");

    //        fields.Add("TAR_ARENDA", "archive_arenda_link_code");

    //        List<string> requiredFields = new List<string>();
    //        requiredFields.Add("TAR_ARENDA");

    //        querySelect = GetSelectStatement("TAR_ARRISH", fields, "ID", requiredFields);

    //        try
    //        {
    //            using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
    //            {
    //                using (FbDataReader reader = commandSelect.ExecuteReader())
    //                {
    //                    DataReaderAdapter adapter = new DataReaderAdapter(reader);

    //                    // Map RASPOR IDs from Arenda to our database
    //                    adapter.AddColumnMapping("RASPOR", arendaRasporDocIdMapping);

    //                    BulkMigrateDataReader("TAR_ARRISH", "arch_link_arenda_2_decisions", adapter, fields);

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

    //    #endregion (Arenda database migration)
    //}
}
