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
//    public partial class Migrator
//    {
//        #region Member variables

//        /// <summary>
//        /// Auto-incremented Id of organizations that are added from the Balans database
//        /// </summary>
//        private int balansOrganizationIdSeed = 300000;

//        /// <summary>
//        /// A mapping of organization IDs between Balans and 1NF
//        /// </summary>
//        private Dictionary<int, int> orgIdMappingBalansTo1NF = new Dictionary<int, int>();

//        /// <summary>
//        /// Mapping of Balans organization forms (S_FORM_GOSP) to 1NF organization forms (S_FORM_GOSP)
//        /// </summary>
//        private Dictionary<int, int> orgFormMappingBalansTo1NF = new Dictionary<int, int>();

//        /// <summary>
//        /// Mapping of Balans document kinds (SKINDDOC) to 1NF document kinds (S_VID_DOC)
//        /// </summary>
//        private Dictionary<int, int> docKindMappingBalansTo1NF = new Dictionary<int, int>();

//        /// <summary>
//        /// Mapping of Balans document IDs to 1NF document IDs
//        /// </summary>
//        private Dictionary<int, int> docIdMappingBalansTo1NF = new Dictionary<int, int>();

//        /// <summary>
//        /// Cache of registration organizations from the Balans database (by name, not by ID)
//        /// </summary>
//        private Dictionary<string, int> reesOrgCacheBalans = new Dictionary<string, int>();

//        /// <summary>
//        /// A CSV log that contains all organizations from the Balans database that were not
//        /// found in the 1NF database
//        /// </summary>
//        private CSVLog csvLogUnmatchedBalansOrg = new CSVLog("CSV/NotFoundBalansOrg.csv");

//        /// <summary>
//        /// A temporary table which is used to store organizations from databases other thatn 1NF
//        /// </summary>
//        private const string OrganizationsTempTable = "temp_balans_org";

//        #endregion (Member variables)

//        #region Balans database migration

//        private void MigrateBalans()
//        {
//            logger.WriteInfo("=== Migrating database Balans ===");

//            // Connect to Balans database
//            connectionBalans = ConnectToFirebirdDatabase("Balans", GetBalansConnectionString());

//            if (connectionBalans != null && connectionBalans.State == ConnectionState.Open)
//            {
//                // Prepare mapping for the Balans dictionaries that require mapping to 1NF dictionaries
//                PrepareBalansOrgFormMapping();
//                PrepareBalansDocKindMapping();

//                MigrateBalansOrgDictionaries();
//                MigrateBalansDocDictionaries();
//                PrepareBalansReesOrgCache();

//                MigrateBalansDocuments();
//                MigrateBalansOrganizations();
//                MigrateBalansDocRelations();
                
//                // Import financial data
//                MigrateBalansFinReports();

//                // Import budget payment rate coefficients
//                logger.WriteInfo("=== Importing financial coefficients for budget paymet rate calculation ===");
//                ImportFinCoefficients();

//                // Calculate the budget payment rates
//                logger.WriteInfo("=== Calculating budget paymet rates ===");
//                CalculateBudgetPaymentRates();

//                // Close connection
//                logger.WriteInfo("Disconnectiong from the Balans database");

//                connectionBalans.Close();
//                connectionBalans = null;
//            }
//        }

//        private void PrepareBalansOrgFormMapping()
//        {
//            orgFormMappingBalansTo1NF.Clear();

//            orgFormMappingBalansTo1NF.Add(1, 2); // БЮДЖЕТНА
//            orgFormMappingBalansTo1NF.Add(2, 1); // ГОСПРОЗРАХУНКОВА
//            orgFormMappingBalansTo1NF.Add(3, 5); // ЗМІШАНА
//        }

//        private void PrepareBalansDocKindMapping()
//        {
//            // Get the resource strings
//            string docTypeOrder = GUKV.DataMigration.Properties.Resources.DocTypeOrder;
//            string docTypeDecision = GUKV.DataMigration.Properties.Resources.DocTypeDecision;
//            string docTypeContract = GUKV.DataMigration.Properties.Resources.DocTypeContract;
//            string docTypePolojennya = GUKV.DataMigration.Properties.Resources.DocTypePolojennya;
//            string docTypeStatut = GUKV.DataMigration.Properties.Resources.DocTypeStatut;
//            string docTypePostanova = GUKV.DataMigration.Properties.Resources.DocTypePostanova;
//            string docTypePovidomlennya = GUKV.DataMigration.Properties.Resources.DocTypePovidomlennya;

//            int idRozporyadjennia = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docTypeOrder);

//            if (idRozporyadjennia < 0)
//            {
//                idRozporyadjennia = 7;

//                AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", idRozporyadjennia, docTypeOrder);
//            }

//            int idRishennia = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docTypeDecision);

//            if (idRishennia < 0)
//            {
//                idRishennia = 9;

//                AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", idRishennia, docTypeDecision);
//            }

//            int idContract = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docTypeContract);

//            if (idContract < 0)
//            {
//                idContract = 11;

//                AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", idContract, docTypeContract);
//            }

//            int idPolojennia = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docTypePolojennya);

//            if (idPolojennia < 0)
//            {
//                idPolojennia = 12;

//                AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", idPolojennia, docTypePolojennya);
//            }

//            int idStatut = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docTypeStatut);

//            if (idStatut < 0)
//            {
//                idStatut = 13;

//                AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", idStatut, docTypeStatut);
//            }

//            int idPostanova = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docTypePostanova);

//            if (idPostanova < 0)
//            {
//                idPostanova = 34;

//                AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", idPostanova, docTypePostanova);
//            }

//            int idPovidomlennya = FindDictionaryEntryIdInSQLServer("dict_doc_kind", "id", "name", docTypePovidomlennya);

//            if (idPovidomlennya < 0)
//            {
//                idPovidomlennya = 54;

//                AddDictionaryEntryInSQLServer("dict_doc_kind", "id", "name", idPovidomlennya, docTypePovidomlennya);
//            }

//            // Create the mapping
//            docKindMappingBalansTo1NF.Clear();

//            docKindMappingBalansTo1NF.Add(0, 1);
//            docKindMappingBalansTo1NF.Add(1, idRozporyadjennia);
//            docKindMappingBalansTo1NF.Add(2, 44);
//            docKindMappingBalansTo1NF.Add(3, idRishennia);
//            docKindMappingBalansTo1NF.Add(4, 47);
//            docKindMappingBalansTo1NF.Add(5, 27);
//            docKindMappingBalansTo1NF.Add(6, 113);
//            docKindMappingBalansTo1NF.Add(7, 6);
//            docKindMappingBalansTo1NF.Add(8, idContract);
//            docKindMappingBalansTo1NF.Add(9, idPolojennia);
//            docKindMappingBalansTo1NF.Add(10, idStatut);
//            docKindMappingBalansTo1NF.Add(11, idPostanova);
//            docKindMappingBalansTo1NF.Add(12, idPovidomlennya);
//            docKindMappingBalansTo1NF.Add(13, 5);
//            docKindMappingBalansTo1NF.Add(14, 52);
//        }

//        private void MigrateBalansOrgDictionaries()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // S_VLASN_VIB
//            fields.Clear();
//            fields.Add("KOD_VLASN_VIB", "id");
//            fields.Add("NAME_VLASN_VIB", "name");

//            MigrateTable(connectionBalans, "S_VLASN_VIB", "dict_form_vlasn_vibuttya", fields, "KOD_VLASN_VIB", null);

//            // S_MAYNO
//            fields.Clear();
//            fields.Add("KOD_MAYNO", "id");
//            fields.Add("MAYNO", "name");

//            MigrateTable(connectionBalans, "S_MAYNO", "dict_org_mayno", fields, "KOD_MAYNO", null);

//            // S_POSADA
//            fields.Clear();
//            fields.Add("KOD_POSADA", "id");
//            fields.Add("POSADA", "name");

//            MigrateTable(connectionBalans, "S_POSADA", "dict_org_contact_posada", fields, "KOD_POSADA", null);

//            // S_PRIZ_NADH
//            fields.Clear();
//            fields.Add("KOD_PRIZ_NADH", "id");
//            fields.Add("NAME_PRIZ_NAMDH", "name");

//            MigrateTable(connectionBalans, "S_PRIZ_NADH", "dict_org_nadhodjennya", fields, "KOD_PRIZ_NADH", null);

//            // S_PRIZ_VIB
//            fields.Clear();
//            fields.Add("KOD_PRIZ_VIB", "id");
//            fields.Add("NAME_PRIZ_VIB", "name");

//            MigrateTable(connectionBalans, "S_PRIZ_VIB", "dict_org_vibuttya", fields, "KOD_PRIZ_VIB", null);

//            // S_PRIZ_PR_KOM
//            fields.Clear();
//            fields.Add("KOD_PRIZ_PR_KOM", "id");
//            fields.Add("NAME_PRIZ_PR_KOM", "name");

//            MigrateTable(connectionBalans, "S_PRIZ_PR_KOM", "dict_org_privat_status", fields, "KOD_PRIZ_PR_KOM", null);

//            // S_POTOCH_STAN
//            fields.Clear();
//            fields.Add("KOD_POTOCH_STAN", "id");
//            fields.Add("NAME_POTOCH_STAN", "name");

//            MigrateTable(connectionBalans, "S_POTOCH_STAN", "dict_org_cur_state", fields, "KOD_POTOCH_STAN", null);

//            // S_SFERA_UPR
//            fields.Clear();
//            fields.Add("KOD_SFERA_UPR", "id");
//            fields.Add("NAME_SFERA_UPR", "name");

//            MigrateTable(connectionBalans, "S_SFERA_UPR", "dict_org_sfera_upr", fields, "KOD_SFERA_UPR", null);

//            // S_EK_PLAN_ZONA
//            fields.Clear();
//            fields.Add("KOD_EK_PLAN_ZONA", "id");
//            fields.Add("NAME_EK_PLAN_ZONA", "name");

//            MigrateTable(connectionBalans, "S_EK_PLAN_ZONA", "dict_org_plan_zone", fields, "KOD_EK_PLAN_ZONA", null);

//            // S_REESORG
//            fields.Clear();
//            fields.Add("KOD_REESORG", "id");
//            fields.Add("REESORG", "name");

//            MigrateTable(connectionBalans, "S_REESORG", "dict_org_registr_org", fields, "KOD_REESORG", null);

//            // S_ORG_FORM
//            fields.Clear();
//            fields.Add("KOD_ORG_FORM", "id");
//            fields.Add("NAME_ORG_FORM", "name");

//            OverrideDictionary(connectionBalans, "S_ORG_FORM", "dict_org_form", fields,
//                "KOD_ORG_FORM", "id", "KOD_ORG_FORM", null, false);

//            // S_VIDOM_NAL
//            fields.Clear();
//            fields.Add("KOD_VIDOM_NAL", "id");
//            fields.Add("NAME_VIDOM_NAL", "name");
//            fields.Add("FULL_NAME", "full_name");

//            OverrideDictionary(connectionBalans, "S_VIDOM_NAL", "dict_org_vedomstvo", fields,
//                "KOD_VIDOM_NAL", "id", "KOD_VIDOM_NAL", null, false);

//            // S_GALUZ
//            fields.Clear();
//            fields.Add("KOD_GALUZ", "id");
//            fields.Add("NAME_GALUZ", "name");

//            OverrideDictionary(connectionBalans, "S_GALUZ", "dict_org_industry", fields,
//                "KOD_GALUZ", "id", "KOD_GALUZ", null, false);

//            // S_ORGAN
//            fields.Clear();
//            fields.Add("KOD_ORGAN", "id");
//            fields.Add("NAME_ORGAN", "name");
//            fields.Add("KOD_GALUZ", "industry_code");

//            OverrideDictionary(connectionBalans, "S_ORGAN", "dict_org_organ", fields,
//                "KOD_ORGAN", "id", "KOD_ORGAN", null, false);

//            // S_GOSP_STRUKT
//            fields.Clear();
//            fields.Add("KOD_GOSP_STRUKT", "id");
//            fields.Add("NAME_GOSP_STRUKT", "name");
//            fields.Add("KOD_GALUZ", "industry_code");

//            OverrideDictionary(connectionBalans, "S_GOSP_STRUKT", "dict_org_gosp_struct", fields,
//                "KOD_GOSP_STRUKT", "id", "KOD_GOSP_STRUKT", null, false);

//            // S_RAYON
//            fields.Clear();
//            fields.Add("KOD_RAYON", "id");
//            fields.Add("NAME_RAYON", "name");

//            OverrideDictionary(connectionBalans, "S_RAYON", "dict_districts", fields,
//                "KOD_RAYON", "id", "KOD_RAYON", null, false);

//            // S_FORM_VLASN
//            fields.Clear();
//            fields.Add("KOD_FORM_VLASN", "id");
//            fields.Add("NAME_FORM_VLASN", "name");

//            OverrideDictionary(connectionBalans, "S_FORM_VLASN", "dict_org_ownership", fields,
//                "KOD_FORM_VLASN", "id", "KOD_FORM_VLASN", null, false);

//            // S_STATUS
//            fields.Clear();
//            fields.Add("KOD_STATUS", "id");
//            fields.Add("NAME_STATUS", "name");

//            OverrideDictionary(connectionBalans, "S_STATUS", "dict_org_status", fields,
//                "KOD_STATUS", "id", "KOD_STATUS", null, false);

//            // S_VID_DIAL
//            fields.Clear();
//            fields.Add("KOD_VID_DIAL", "id");
//            fields.Add("NAME_VID_DIAL", "name");
//            fields.Add("KOD_GALUZ", "branch_code");

//            OverrideDictionary(connectionBalans, "S_VID_DIAL", "dict_org_occupation", fields,
//                "KOD_VID_DIAL", "id", "KOD_VID_DIAL", null, false);

//            // S_OLD_GALUZ
//            fields.Clear();
//            fields.Add("KOD_OLD_GALUZ", "id");
//            fields.Add("NAME_OLD_GALUZ", "name");

//            MigrateTable(connectionBalans, "S_OLD_GALUZ", "dict_org_old_industry", fields, "KOD_OLD_GALUZ", null);

//            // S_OLD_VID_DIAL
//            fields.Clear();
//            fields.Add("KOD_OLD_VID_DIAL", "id");
//            fields.Add("NAME_OLD_VID_DIAL", "name");
//            fields.Add("KOD_OLD_GALUZ", "old_industry_id");

//            MigrateTable(connectionBalans, "S_OLD_VID_DIAL", "dict_org_old_occupation", fields, "KOD_OLD_VID_DIAL", null);

//            // S_OLD_ORGAN
//            fields.Clear();
//            fields.Add("KOD_OLD_ORGAN", "id");
//            fields.Add("NAME_OLD_ORGAN", "name");
//            fields.Add("KOD_OLD_GALUZ", "old_industry_id");

//            MigrateTable(connectionBalans, "S_OLD_ORGAN", "dict_org_old_organ", fields, "KOD_OLD_ORGAN", null);
//        }

//        private void MigrateBalansDocDictionaries()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();
//            List<string> requiredFields = new List<string>();

//            // S_UZAG_DOC
//            fields.Clear();
//            fields.Add("ID", "id");
//            fields.Add("NAZVA", "name");

//            requiredFields.Clear();
//            requiredFields.Add("NAZVA");

//            MigrateTable(connectionBalans, "S_UZAG_DOC", "dict_doc_general_kind", fields, "ID", requiredFields);
//        }

//        private void MigrateBalansDocuments()
//        {
//            logger.WriteInfo("Migrating documents from the Balans database");

//            Dictionary<string, string> fields = new Dictionary<string, string>();
//            Dictionary<string, object> dataOverride = new Dictionary<string, object>();

//            // Prepare a field mapping for Documents
//            fields.Add("ID", "id");
//            fields.Add("NOMER", "doc_num");
//            fields.Add("DT_DOC", "doc_date");
//            fields.Add("ID_VID_DOC", "kind_id");
//            fields.Add("ID_UZAG_DOC", "general_kind_id");
//            fields.Add("NAME_DOC_SHORT", "topic");

//            // Get indices of the columns that we need for document lookup and dictionary mapping
//            int docKindFieldIndex = GetSrcFieldIndexFromMapping(fields, "ID_VID_DOC", "DOCUM");
//            int docNumFieldIndex = GetSrcFieldIndexFromMapping(fields, "NOMER", "DOCUM");
//            int docDateFieldIndex = GetSrcFieldIndexFromMapping(fields, "DT_DOC", "DOCUM");
//            int docGeneralKindFieldIndex = GetSrcFieldIndexFromMapping(fields, "ID_UZAG_DOC", "DOCUM");

//            // Get all the documents from the Balans database
//            List<string> requiredFields = new List<string>();

//            requiredFields.Add("NAME_DOC");

//            string querySelect = GetSelectStatement("DOCUM", fields, "ID", requiredFields);

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connectionBalans))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            dataOverride.Clear();

//                            object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
//                            object dataDocKind = reader.IsDBNull(docKindFieldIndex) ? null : reader.GetValue(docKindFieldIndex);
//                            object dataDocNum = reader.IsDBNull(docNumFieldIndex) ? null : reader.GetValue(docNumFieldIndex);
//                            object dataDocDate = reader.IsDBNull(docDateFieldIndex) ? null : reader.GetValue(docDateFieldIndex);

//                            if (dataId is int)
//                            {
//                                int documentIdBalans = (int)dataId;

//                                // Map the document kind to 1NF document kinds
//                                if (dataDocKind is short)
//                                {
//                                    dataDocKind = (int)((short)dataDocKind);
//                                }

//                                if (dataDocKind is int)
//                                {
//                                    int newDocKind = 0;

//                                    if (docKindMappingBalansTo1NF.TryGetValue((int)dataDocKind, out newDocKind))
//                                    {
//                                        dataDocKind = newDocKind;
//                                        dataOverride.Add("kind_id", newDocKind);
//                                    }
//                                }

//                                // Try to find an existing document
//                                int existingDocId = -1;

//                                if (dataDocNum is string && dataDocDate is DateTime && dataDocKind is int)
//                                {
//                                    existingDocId = docFinder.FindDocumentIn1NF(
//                                        (DateTime)dataDocDate, (string)dataDocNum, (int)dataDocKind, "");
//                                }

//                                if (existingDocId > 0)
//                                {
//                                    // Save the mapping
//                                    docIdMappingBalansTo1NF.Add(documentIdBalans, existingDocId);
//                                }
//                                else
//                                {
//                                    // Create the new document using INSERT
//                                    string queryInsert = GetInsertStatement("documents", fields, reader, dataOverride);

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

//                                    // Save the mapping; document is inserted with the same ID
//                                    docIdMappingBalansTo1NF.Add(documentIdBalans, documentIdBalans);

//                                    // Register the inserted document in the Document Finder, for future lookup
//                                    docFinder.RegisterDocument(documentIdBalans, dataDocDate, dataDocNum, dataDocKind, "",
//                                        !reader.IsDBNull(docGeneralKindFieldIndex));
//                                }
//                            }
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (ex is MigrationAbortedException)
//                {
//                    throw;
//                }
//                else
//                {
//                    ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                    // Abort migration correctly
//                    throw new MigrationAbortedException();
//                }
//            }
//        }

//        private void AddBalansDocRelation(int organizationId, int documentId, int linkKind)
//        {
//            // Get the correct organization Id and document Id (mapped to the 1NF range)
//            int actualOrgId = organizationId;
//            int actualDocId = documentId;

//            if (orgIdMappingBalansTo1NF.TryGetValue(organizationId, out actualOrgId) &&
//                docIdMappingBalansTo1NF.TryGetValue(documentId, out actualDocId))
//            {
//                // Check if this link is really new and if the document is present
//                if (!docFinder.DocLinkToOrgExistsInSQLServer(actualOrgId, actualDocId) &&
//                    docFinder.DocumentExistsInSQLServer(actualDocId))
//                {
//                    docFinder.AddDocLinkToOrganization(connectionSqlClient,
//                        actualOrgId, actualDocId, linkKind);
//                }
//            }
//        }

//        private void MigrateBalansDocRelations()
//        {
//            logger.WriteInfo("Migrating document relations from the Balans database");

//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            /////////////////////////////////////////////////////////////////////////////////
//            // 1) Migrate the DOC_SORG table - relations between documents and organizations
//            /////////////////////////////////////////////////////////////////////////////////

//            // Prepare a field mapping for the DOC_SORG table
//            fields.Clear();
//            fields.Add("KOD_OBJ", "organization_id");
//            fields.Add("KOD_DOC", "document_id");

//            // Get all the document relations from the Balans database
//            List<string> requiredFields = new List<string>();

//            requiredFields.Add("KOD_OBJ");
//            requiredFields.Add("KOD_DOC");

//            string querySelect = GetSelectStatement("DOC_SORG", fields, "KOD_OBJ, KOD_DOC", requiredFields);

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connectionBalans))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            object dataOrgId = reader.GetValue(0);
//                            object dataDocId = reader.GetValue(1);

//                            if (dataOrgId is int && dataDocId is int)
//                            {
//                                AddBalansDocRelation((int)dataOrgId, (int)dataDocId, 0);
//                            }
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (ex is MigrationAbortedException)
//                {
//                    throw;
//                }
//                else
//                {
//                    ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                    // Abort migration correctly
//                    throw new MigrationAbortedException();
//                }
//            }

//            /////////////////////////////////////////////////////////////////////////////////
//            // 2) Migrate the document relations from SORG table
//            /////////////////////////////////////////////////////////////////////////////////

//            // Get the document relations from the SORG table
//            querySelect = "SELECT KOD_OBJ, ID_DOC_PIDST, ID_DOC_KONTR FROM SORG WHERE " +
//                "(NOT ID_DOC_PIDST IS NULL) OR (NOT ID_DOC_KONTR IS NULL)";

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connectionBalans))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            object dataOrgId = reader.GetValue(0);
//                            object dataDocIdPidst = reader.GetValue(1);
//                            object dataDocIdKontr = reader.GetValue(2);

//                            if (dataOrgId is int && dataDocIdPidst is int)
//                            {
//                                AddBalansDocRelation((int)dataOrgId, (int)dataDocIdPidst, 0);
//                            }

//                            if (dataOrgId is int && dataDocIdKontr is int)
//                            {
//                                AddBalansDocRelation((int)dataOrgId, (int)dataDocIdKontr, 0);
//                            }
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (ex is MigrationAbortedException)
//                {
//                    throw;
//                }
//                else
//                {
//                    ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                    // Abort migration correctly
//                    throw new MigrationAbortedException();
//                }
//            }
//        }

//        private void MigrateBalansOrganizations()
//        {
//            logger.WriteInfo("Migrating organizations from the Balans database");

//            Dictionary<string, string> fields = new Dictionary<string, string>();
//            Dictionary<string, object> dataOverride = new Dictionary<string, object>();

//            string fieldBalansId = "KOD_OBJ";
//            string fieldFormGosp = "KOD_FORM_GOSP";
//            string fieldKodReesOrg = "KOD_REESORG";
//            string fieldREESORG = "REESORG";
//            string fieldKodVidPidpr = "KOD_VID_PREDPR";

//            string aliasId = "id";
//            string aliasFormGosp = "form_gosp_id";
//            string aliasOriginDb = "origin_db";
//            string aliasReesOrg = "registr_org_id";
//            string aliasKodVidPidpr = "is_under_closing";

//            // Prepare a field mapping for Organizations
//            CreateBalansOrganizationsFieldMapping(fields);

//            // Get indices of some columns that we need for searching and dictionary mapping
//            int kodFieldIndex = GetSrcFieldIndexFromMapping(fields, fieldBalansId, "SORG");
//            int formGospFieldIndex = GetSrcFieldIndexFromMapping(fields, fieldFormGosp, "SORG");
//            int reesOrgCodeFieldIndex = GetSrcFieldIndexFromMapping(fields, fieldKodReesOrg, "SORG");
//            int reesorgFieldIndex = GetSrcFieldIndexFromMapping(fields, fieldREESORG, "SORG");
//            int kodVidPidprFieldIndex = GetSrcFieldIndexFromMapping(fields, fieldKodVidPidpr, "SORG");

//            // Write a header to the CSV log that contains unmatched organization
//            csvLogUnmatchedBalansOrg.WriteCell(GUKV.DataMigration.Properties.Resources.CvsLogBalansOrgOKPO);
//            csvLogUnmatchedBalansOrg.WriteCell(GUKV.DataMigration.Properties.Resources.CvsLogBalansOrgFullName);
//            csvLogUnmatchedBalansOrg.WriteCell(GUKV.DataMigration.Properties.Resources.CvsLogBalansOrgShortName);
//            csvLogUnmatchedBalansOrg.WriteEndOfLine();

//            #region Not archive organizations

//            // Dump all organizations from Balans database to our database
//            ExecuteStatementInSQLServer("DELETE FROM " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable);

//            string selectStatement = GetSelectStatement("SORG", fields, "", null);

//            selectStatement += " s1 WHERE KOD_STAN = (SELECT MAX(KOD_STAN) FROM sorg s2 WHERE s2.KOD_OBJ = s1.KOD_OBJ) ORDER BY KOD_OBJ";

//            using (FbCommand cmd = new FbCommand(selectStatement, connectionBalans))
//            {
//                using (FbDataReader r = cmd.ExecuteReader())
//                {
//                    BulkMigrateDataReader("SORG", OrganizationsTempTable, r, fields);

//                    r.Close();
//                }
//            }

//            // Erase immediately all organizations that are not used in the Balans database
//            string query = @"UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
//                @" SET is_deleted = 1 WHERE
//                    (NOT end_state_date IS NULL) OR
//                    (COALESCE(status_id, 0) <> 1) OR
//                    (form_gosp_id IS NULL) OR
//                    (COALESCE(form_ownership_id, 0) NOT IN (32, 34))";

//            ExecuteStatementInSQLServer(query);

//            // Perform mapping of some dictionaries
//            ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable + " SET origin_db = 2");

//            UpdateSqlServerDictionaryByMapping(OrganizationsTempTable, "form_gosp_id", orgFormMappingBalansTo1NF);

//            ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
//                " SET is_under_closing = 0 WHERE (is_under_closing IS NULL) OR (is_under_closing <> 15)");

//            ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
//                " SET is_under_closing = 1 WHERE is_under_closing = 15");

//            foreach (KeyValuePair<string, int> pair in reesOrgCacheBalans)
//            {
//                string reesOrgName = pair.Key.Trim();

//                if (reesOrgName.Length > 0)
//                {
//                    query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
//                        " SET registr_org_id = @orgid WHERE LTRIM(RTRIM(registration_auth)) = @orgstr";

//                    using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                    {
//                        cmd.Parameters.Add(new SqlParameter("orgid", pair.Value));
//                        cmd.Parameters.Add(new SqlParameter("orgstr", reesOrgName));

//                        cmd.ExecuteNonQuery();
//                    }
//                }
//            }

//            // Perform mapping of Balans organizations to 1NF organizations
//            query = "SELECT id, zkpo_code, full_name, short_name FROM " + OrganizationsTempTable + " WHERE (is_deleted IS NULL) OR (is_deleted = 0)";

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader r = cmd.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        int id = r.GetInt32(0);
//                        string zkpo = r.IsDBNull(1) ? "" : r.GetString(1);
//                        string fullName = r.IsDBNull(2) ? "" : r.GetString(2);
//                        string shortName = r.IsDBNull(3) ? "" : r.GetString(3);

//                        bool categorized = false;

//                        int existingOrganizationID = organizationFinder.FindOrganization(
//                            zkpo, fullName, shortName, true, out categorized);

//                        if (existingOrganizationID > 0)
//                        {
//                            orgIdMappingBalansTo1NF.Add(id, existingOrganizationID);
//                        }
//                        else
//                        {
//                            orgIdMappingBalansTo1NF.Add(id, GetBalansOrganizationIdIn1NF(id));

//                            // Organization not found; write it to the log
//                            csvLogUnmatchedBalansOrg.WriteCell(id);
//                            csvLogUnmatchedBalansOrg.WriteCell(zkpo);
//                            csvLogUnmatchedBalansOrg.WriteCell(fullName);
//                            csvLogUnmatchedBalansOrg.WriteCell(shortName);
//                            csvLogUnmatchedBalansOrg.WriteEndOfLine();
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            // Set the master ID for each mapped organization
//            foreach (KeyValuePair<int, int> pair in orgIdMappingBalansTo1NF)
//            {
//                if (pair.Value < balansOrganizationIdSeed)
//                {
//                    query = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
//                        " SET master_org_id = @masterid WHERE id = @orgid";

//                    using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                    {
//                        cmd.Parameters.Add(new SqlParameter("orgid", pair.Key));
//                        cmd.Parameters.Add(new SqlParameter("masterid", pair.Value));

//                        cmd.ExecuteNonQuery();
//                    }

//                    // Overwrite the master organization, because Balans is the primary source for organization information
//                    OverwriteOrganization(pair.Key, pair.Value);
//                }
//            }

//            // Make sure that Balans organizations never mess up with 1NF organizations
//            ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
//                " SET id = id + " + balansOrganizationIdSeed);

//            // Perform a BULK insert from the temporary table into the main table
//            BulkMigrateNewOrganizations();

//            #endregion Not archive organizations

//            #region Archive organizations

//            // Import the archive states; they are located in the same SORG table
//            logger.WriteInfo("Importing organization archive states from the SORG table");

//            string querySelect = GetSelectStatement("SORG", fields, "", null);

//            querySelect += " WHERE (NOT DT_END_STAN IS NULL) AND (DT_END_STAN < CURRENT_TIMESTAMP) ORDER BY KOD_OBJ, KOD_STAN";

//            FbCommand commandSelect = new FbCommand(querySelect, connectionBalans);
//            FbDataReader reader = null;

//            try
//            {
//                reader = commandSelect.ExecuteReader();
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }

//            while (reader.Read())
//            {
//                // Get Id of the organization in Balans database, and values of the search fields
//                object dataBalansId = reader.GetValue(kodFieldIndex);
//                object dataFormGosp = reader.GetValue(formGospFieldIndex);
//                object dataReesOrgCode = reader.IsDBNull(reesOrgCodeFieldIndex) ? null : reader.GetValue(reesOrgCodeFieldIndex);
//                object dataReesorg = reader.GetValue(reesorgFieldIndex);
//                var dataKodVidPidprId = (int?)(reader.IsDBNull(kodVidPidprFieldIndex) ? null : reader.GetValue(kodVidPidprFieldIndex));

//                if (dataBalansId is int)
//                {
//                    // Process only organizations that are mapped to our database
//                    int orgMappedId = 0;

//                    if (orgIdMappingBalansTo1NF.TryGetValue((int)dataBalansId, out orgMappedId))
//                    {
//                        // Generate mapping for the dictionaries
//                        dataOverride.Clear();

//                        if (dataFormGosp is int)
//                        {
//                            int newFormGosp = 0;

//                            if (orgFormMappingBalansTo1NF.TryGetValue((int)dataFormGosp, out newFormGosp))
//                            {
//                                dataOverride.Add(aliasFormGosp, newFormGosp);
//                            }
//                        }

//                        // Map registration authority code
//                        if (dataReesOrgCode == null && dataReesorg is string && reesOrgCacheBalans.ContainsKey((string)dataReesorg))
//                        {
//                            dataOverride.Add(aliasReesOrg, reesOrgCacheBalans[(string)dataReesorg]);
//                        }

//                        // Kod_Vid_Pidpr -> to (bool)is_under_closing
//                        dataOverride.Add(aliasKodVidPidpr, (dataKodVidPidprId.HasValue && dataKodVidPidprId.Value == 15) ? 1 : 0);

//                        dataOverride.Add(aliasId, orgMappedId);
//                        dataOverride.Add(aliasOriginDb, 2); // 2 ~ Balans database

//                        // INSERT this entry into archive state table (arch_organizations)
//                        string queryInsertOrUpdate = GetInsertStatement("arch_organizations", fields, reader, dataOverride);

//                        try
//                        {
//                            using (SqlCommand command = new SqlCommand(queryInsertOrUpdate, connectionSqlClient))
//                            {
//                                command.ExecuteNonQuery();
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
//            }

//            reader.Close();

//            #endregion Archive organizations

//            // Fill the fields in the archive state storage that can not be imported
//            ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase +
//                ".dbo.arch_organizations SET archive_create_date = end_state_date WHERE archive_create_date IS NULL");

//            ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase +
//                ".dbo.arch_organizations SET archive_create_user = modified_by WHERE archive_create_user IS NULL");
//        }

//        private void BulkMigrateNewOrganizations()
//        {
//            // Read the data
//            string querySelect = "SELECT * FROM " + OrganizationsTempTable;

//            try
//            {
//                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
//                {
//                    using (SqlDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        using (SqlConnection destinationConnection = new SqlConnection(GetSQLClientConnectionString()))
//                        {
//                            destinationConnection.Open();

//                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
//                            {
//                                // Initialize the BULK copy class
//                                bulkCopy.DestinationTableName = "dbo.organizations";

//                                // Write from the source to the destination
//                                try
//                                {
//                                    bulkCopy.WriteToServer(reader);
//                                }
//                                catch (Exception ex)
//                                {
//                                    ShowSqlErrorMessageDlg("<No SQL statement available for BULK insertion>", ex.Message);

//                                    // Abort migration correctly
//                                    throw new MigrationAbortedException();
//                                }

//                                // Close the objects
//                                bulkCopy.Close();
//                            }

//                            destinationConnection.Close();
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (ex is MigrationAbortedException)
//                {
//                    throw;
//                }
//                else
//                {
//                    ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                    // Abort migration correctly
//                    throw new MigrationAbortedException();
//                }
//            }
//        }

//        private void OverwriteOrganization(int sourceOrgId, int destinationOrgId)
//        {
//#if ENABLE_1NF_ORG_OVERWRITE
//            string fieldList = "";
//            SqlCommand cmdUpdate = new SqlCommand("", connectionSqlClient);

//            using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + OrganizationsTempTable +
//                " WHERE id = @srcid", connectionSqlClient))
//            {
//                cmd.Parameters.Add(new SqlParameter("srcid", sourceOrgId));

//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    if (reader.Read())
//                    {
//                        // Prepare the UPDATE query from the data reader
//                        for (int i = 0; i < reader.FieldCount; i++)
//                        {
//                            string columnName = reader.GetName(i).ToLower();

//                            if (columnName != "id" && columnName != "master_org_id" && !reader.IsDBNull(i))
//                            {
//                                object value = reader.GetValue(i);

//                                if (value is DateTime)
//                                {
//                                    value = CheckDateTime((DateTime)value);
//                                }

//                                cmdUpdate.Parameters.Add(new SqlParameter("param" + i.ToString(), value));

//                                fieldList += ", " + columnName + " = @param" + i.ToString();
//                            }
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            if (fieldList.Length > 0)
//            {
//                // Perform UPDATE
//                cmdUpdate.CommandText = "UPDATE organizations SET " + fieldList.TrimStart(' ', ',') + " WHERE id = @destid";

//                cmdUpdate.Parameters.Add(new SqlParameter("destid", destinationOrgId));

//                try
//                {
//                    cmdUpdate.ExecuteNonQuery();
//                }
//                catch (Exception ex)
//                {
//                    logger.WriteError("Balans organization " + sourceOrgId.ToString() + " has some invalid data: " + ex.Message);
//                }
//            }
//#endif // ENABLE_1NF_ORG_OVERWRITE
//        }

//        private void UpdateSqlServerDictionaryByMapping(string tableName, string fieldName, Dictionary<int, int> mapping)
//        {
//            // Generate a temporary unique key for each mapping entry
//            int key = 100000;
//            Dictionary<int, int> newMapping = new Dictionary<int, int>();

//            foreach (KeyValuePair<int, int> pair in mapping)
//            {
//                if (pair.Key != pair.Value)
//                {
//                    key++;

//                    newMapping.Add(key, pair.Value);

//                    ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + tableName +
//                        " SET " + fieldName + " = " + key.ToString() + " WHERE " + fieldName + " = " + pair.Key.ToString());
//                }
//            }

//            foreach (KeyValuePair<int, int> pair in newMapping)
//            {
//                ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase + ".dbo." + tableName +
//                    " SET " + fieldName + " = " + pair.Value.ToString() + " WHERE " + fieldName + " = " + pair.Key.ToString());
//            }
//        }

//        private int GetBalansOrganizationIdIn1NF(int orgIdInBalans)
//        {
//            return orgIdInBalans + balansOrganizationIdSeed;
//        }

//        private void CreateBalansOrganizationsFieldMapping(Dictionary<string, string> fields)
//        {
//            fields.Clear();

//            // Fields from the 1NF database (SORG_1NF table)

//            fields.Add("KOD_OBJ", "id");

//            // The KOD_STAN is not imported. It is used only to set proper value for origin_db column
//            fields.Add("KOD_STAN", "origin_db");

//            fields.Add("LAST_SOST", "last_state");
//            fields.Add("KOD_VID_DIAL", "occupation_id");
//            fields.Add("KOD_STATUS", "status_id");
//            fields.Add("KOD_FORM_GOSP", "form_gosp_id");

//            fields.Add("KOD_VID_PREDPR", "is_under_closing");

//            fields.Add("KOD_VLASN_VIB", "form_vlasn_vibuttya_id");

//            fields.Add("KOD_FORM_VLASN", "form_ownership_id");
//            fields.Add("KOD_GOSP_STRUKT", "gosp_struct_id");
//            fields.Add("KOD_ORGAN", "organ_id");
//            fields.Add("KOD_GALUZ", "industry_id");

//            fields.Add("NOMER_OBJ", "nomer_obj");
//            fields.Add("KOD_ZKPO", "zkpo_code");
//            fields.Add("KOD_RAYON", "addr_distr_old_id");
//            fields.Add("KOD_RAYON2", "addr_distr_new_id");
//            fields.Add("NAME_UL", "addr_street_name");

//            fields.Add("NOMER_DOMA", "addr_nomer");
//            fields.Add("NOMER_KORPUS", "addr_korpus");
//            fields.Add("POST_INDEX", "addr_zip_code");
//            fields.Add("ADDITIONAL_ADRESS", "addr_misc");

//            fields.Add("FIO_BOSS", "director_fio");
//            fields.Add("TEL_BOSS", "director_phone");
//            fields.Add("FIO_BUH", "buhgalter_fio");
//            fields.Add("TEL_BUH", "buhgalter_phone");
//            fields.Add("KOLVO_BUD", "num_buildings");

//            fields.Add("FULL_NAME_OBJ", "full_name");
//            fields.Add("SHORT_NAME_OBJ", "short_name");
//            fields.Add("KOD_ADDITION_PRIZNAK", "priznak_id");
//            fields.Add("KOD_PRIKMET", "title_form_id");
//            fields.Add("KOD_PRIZNAK_1", "form_1nf_id");

//            fields.Add("KOD_VIDOM_NAL", "vedomstvo_id");
//            fields.Add("KOD_IMEN", "title_id");
//            fields.Add("KOD_ORG_FORM", "form_id");
//            fields.Add("KOD_VID_GOSP_STR", "gosp_struct_type_id");
//            fields.Add("FIND_NAME_OBJ", "search_name");

//            fields.Add("NAME_DAVAL_VIDM", "name_komu");
//            fields.Add("TELEFAX", "fax");
//            fields.Add("REESORG", "registration_auth");
//            fields.Add("REESNO", "registration_num");
//            fields.Add("REESDT", "registration_date");

//            fields.Add("NO_SVIDOT", "registration_svidot");
//            // fields.Add("YEAR", "l_year"); - always NULL in the source table
//            fields.Add("PL_BALANS", "sqr_on_balance");
//            fields.Add("PL_VIROB_PRIZN", "sqr_manufact");
//            fields.Add("PL_NOT_VIROB_PRIZN", "sqr_non_manufact");

//            fields.Add("PL_ARENDA_VILNA", "sqr_free_for_rent");
//            fields.Add("PL_ZAGAL", "sqr_total");
//            fields.Add("PL_ARENDUETSYA", "sqr_rented");
//            fields.Add("PL_PRIVAT", "sqr_privat");
//            fields.Add("PL_NADANA_V_A", "sqr_given_for_rent");

//            fields.Add("KOLVO_OBJ", "num_objects");
//            fields.Add("KOD_KVED", "kved_code");
//            fields.Add("DT_STAT_DOVIDKA", "date_stat_spravka");

//            fields.Add("KOATUU", "koatuu");
//            fields.Add("USER_KOREG", "modified_by");
//            fields.Add("DATE_KOREG", "modify_date");
//            fields.Add("KOD_VID_AKCIA", "share_type_id");
//            fields.Add("PERSENT_AKCIA", "share");

//            // New fields (added in Balans database) from SORG table

//            fields.Add("KOD_MAYNO", "mayno_id");
//            fields.Add("EMAIL", "contact_email");
//            fields.Add("KOD_POSADA", "contact_posada_id");
//            fields.Add("KOD_PRIZ_NADH", "nadhodjennya_id");
//            fields.Add("KOD_PRIZ_VIB", "vibuttya_id");

//            fields.Add("KOD_PRIZ_PR_KOM", "privat_status_id");
//            fields.Add("KOD_POTOCH_STAN", "cur_state_id");
//            fields.Add("KOD_SFERA_UPR", "sfera_upr_id");
//            fields.Add("KOD_EK_PLAN_ZONA", "plan_zone_id");
//            fields.Add("KOD_REESORG", "registr_org_id");

//            fields.Add("DATE_NADHOD", "nadhodjennya_date");
//            fields.Add("DATE_VIBUT", "vibuttya_date");
//            fields.Add("CHASTKA", "chastka");
//            fields.Add("REES_RISH", "registration_rish");
//            fields.Add("REES_DOV", "registration_dov_date");

//            fields.Add("REESNO_CORP", "registration_corp");
//            // fields.Add("FULL_NAME_OBJBLOB", "full_name_detailed"); - do we really need a THIRD name for organization?
//            fields.Add("BEGIN_STROK", "strok_start_date");
//            fields.Add("END_STROK", "strok_end_date");
//            fields.Add("STAT_FOND", "stat_fond");

//            fields.Add("SIZE_PLUS", "size_plus");
//            fields.Add("POST_INDEX3", "addr_zip_code_3");

//            fields.Add("KOD_OLD_GALUZ", "old_industry_id");
//            fields.Add("KOD_OLD_VID_DIAL", "old_occupation_id");
//            fields.Add("KOD_OLD_ORGAN", "old_organ_id");

//            fields.Add("DT_BEG_STAN", "beg_state_date");
//            fields.Add("DT_END_STAN", "end_state_date");
//        }

//        private void PrepareBalansReesOrgCache()
//        {
//            string query = "SELECT id, name FROM dict_org_registr_org";

//            try
//            {
//                using (SqlCommand commandSelect = new SqlCommand(query, connectionSqlClient))
//                {
//                    using (SqlDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            int id = (int)reader.GetValue(0);
//                            string name = (string)reader.GetValue(1);

//                            reesOrgCacheBalans.Add(name, id);
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(query, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }
//        }

//        #endregion (Balans database migration)
//    }
}
