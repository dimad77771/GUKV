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
//        /// Value of the general_kind_id column for all documents from the 1NF database
//        /// </summary>
//        private int docGeneralCategory1NF = 0;

//        /// <summary>
//        /// Mapping between VID_PREDPR and ORG_FORM columns in 1 NF database
//        /// </summary>
//        private Dictionary<int, int> mappingVidPredprToOrgForm = new Dictionary<int, int>();

//        /// <summary>
//        /// Mapping between old building_doc IDs and the new building_doc IDs
//        /// </summary>
//        private Dictionary<int, int> mappingBuildingDocIds = new Dictionary<int, int>();

//        #endregion (Member variables)

//        #region 1NF database migration

//        private void Migrate1NF()
//        {
//            logger.WriteInfo("=== Migrating database 1NF ===");

//            // Prepare mapping for the 1NF and Arenda document general kind
//            Prepare1NFDocKindMapping();

//            Migrate1NFObjects();
//            Migrate1NFOrganizations();
//            Migrate1NFBalans();
//            Migrate1NFArenda();
//            Migrate1NFArendaNotes();
//            Migrate1NFArendaPidstava();
//            Migrate1NFDocuments();
//            Migrate1NKvedDictionary();

//            Prepare1NFBuildingDocsIdMapping();
//            Migrate1NBalansDocs();

//            Migrate1NFCopiedDictionaries();
//        }

//        private void Prepare1NFDocKindMapping()
//        {
//            string docKind1NF = GUKV.DataMigration.Properties.Resources.DocKind1NFDocuments;
//            string docKindArenda = GUKV.DataMigration.Properties.Resources.DocKindArendaDocuments;
//            string docKindPrivatization = GUKV.DataMigration.Properties.Resources.DocKindPrivatDocuments;

//            // Add the "1nf documents" entry if it is missing
//            docGeneralCategory1NF = FindDictionaryEntryIdInSQLServer(
//                "dict_doc_general_kind", "id", "name", docKind1NF);

//            if (docGeneralCategory1NF < 0)
//            {
//                docGeneralCategory1NF = 121;

//                AddDictionaryEntryInSQLServer("dict_doc_general_kind", "id", "name",
//                    docGeneralCategory1NF, docKind1NF);
//            }

//            // Add the "rent documents" entry if it is missing
//            docGeneralCategoryArenda = FindDictionaryEntryIdInSQLServer(
//                "dict_doc_general_kind", "id", "name", docKindArenda);

//            if (docGeneralCategoryArenda < 0)
//            {
//                docGeneralCategoryArenda = 122;

//                AddDictionaryEntryInSQLServer("dict_doc_general_kind", "id", "name",
//                    docGeneralCategoryArenda, docKindArenda);
//            }

//            // Add the "privatization documents" entry if it is missing
//            docGeneralCategoryPrivatization = FindDictionaryEntryIdInSQLServer(
//                "dict_doc_general_kind", "id", "name", docKindPrivatization);

//            if (docGeneralCategoryPrivatization < 0)
//            {
//                docGeneralCategoryPrivatization = 123;

//                AddDictionaryEntryInSQLServer("dict_doc_general_kind", "id", "name",
//                    docGeneralCategoryPrivatization, docKindPrivatization);
//            }
//        }

//        private void Migrate1NFObjects()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // SUL
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("NAME_OUTPUT", "name_output");
//            fields.Add("KIND_NAME", "kind");
//            fields.Add("STAN", "stan");
//            fields.Add("ACTIVITY", "activity");
//            fields.Add("DOK_ID_FROM", "renamed_from_doc_id");
//            fields.Add("DOK_ID_TO", "renamed_to_doc_id");
//            fields.Add("PARENT_CODE", "parent_id");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SUL", "dict_streets", fields, "KOD", null);

//            RemoveDuplicateStreets();

//            // S_RAYON
//            fields.Clear();
//            fields.Add("KOD_RAYON", "id");
//            fields.Add("NAME_RAYON", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_RAYON", "dict_districts", fields, "KOD_RAYON", null);

//            // S_RAYON2
//            fields.Clear();
//            fields.Add("KOD_RAYON2", "id");
//            fields.Add("NAME_RAYON2", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_RAYON2", "dict_districts2", fields, "KOD_RAYON2", null);

//            // STEXSTAN
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "STEXSTAN", "dict_tech_state", fields, "KOD", null);

//            // SHISTORY
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SHISTORY", "dict_history", fields, "KOD", null);

//            // STYPEOBJ
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "STYPEOBJ", "dict_object_type", fields, "KOD", null);

//            // SKINDOBJ
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("MNEMONIC", "mnemonic");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SKINDOBJ", "dict_object_kind", fields, "KOD", null);

//            // S_OATUU
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAZVA", "name");

//            MigrateTable(connection1NF, "S_OATUU", "dict_oatuu", fields, "KOD", null);

//            // S_OZNAKA_BUD
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_OZNAKA_BUD", "dict_facade", fields, "KOD", null);

//            // OBJECT_1NF
//            FieldMappings.Create1NFObjectFieldMapping(fields, testOnly, enable1NFNewFieldsImport);

//            //MigrateTableUsingINSERT(connection1NF, "OBJECT_1NF", "buildings", fields, "OBJECT_KOD", null);
//            MigrateTable(connection1NF, "OBJECT_1NF", "buildings", fields, "OBJECT_KOD", null);

//            if (!testOnly)
//            {
//                // TAR_OBJECT_1NF - archive states for buildings
//                fields.Remove("ARCH_KOD");
//                fields.Remove("IN_ARCH");
//                fields.Remove("KORPUS");
//                fields.Remove("ADDRESS_NO_KORPUS");

//                fields.Add("CRE_DT", "archive_create_date");
//                fields.Add("CRE_ISP", "archive_create_user");
//                fields.Add("C_KOD", "archive_link_code");
//                fields.Add("NEXT_KOD", "archive_link_code_next");
//                fields.Add("PREV_KOD", "archive_link_code_prev");

//                MigrateTable(connection1NF, "TAR_OBJECT_1NF", "arch_buildings", fields, "OBJECT_KOD", null);

//                // Correct archive state dates and user names
//                ExecuteStatementInSQLServer(@"UPDATE b_next SET modified_by = b_prev.archive_create_user, modify_date = b_prev.archive_create_date
//                   FROM arch_buildings b_next INNER JOIN arch_buildings b_prev ON b_next.archive_link_code = b_prev.archive_link_code_next");
//            }
//        }

//        private void RemoveDuplicateStreets()
//        {
//            Dictionary<int, int> streetStan = new Dictionary<int, int>();

//            using (SqlCommand cmd = new SqlCommand("SELECT id, stan FROM dict_streets", connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
//                        {
//                            int streetId = reader.GetInt32(0);
//                            int stan = reader.GetInt32(1);
//                            int curStan = 0;

//                            if (streetStan.TryGetValue(streetId, out curStan))
//                            {
//                                if (stan > curStan)
//                                {
//                                    streetStan[streetId] = stan;
//                                }
//                            }
//                            else
//                            {
//                                streetStan.Add(streetId, stan);
//                            }
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            foreach (KeyValuePair<int, int> pair in streetStan)
//            {
//                if (pair.Value > 1)
//                {
//                    using (SqlCommand cmd = new SqlCommand("DELETE FROM dict_streets WHERE id = @sid AND stan < @stn", connectionSqlClient))
//                    {
//                        cmd.Parameters.Add(new SqlParameter("sid", pair.Key));
//                        cmd.Parameters.Add(new SqlParameter("stn", pair.Value));

//                        cmd.ExecuteNonQuery();
//                    }
//                }
//            }
//        }

//        private void Migrate1NFOrganizations()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // S_VID_DIAL
//            fields.Clear();
//            fields.Add("KOD_VID_DIAL", "id");
//            fields.Add("NAME_VID_DIAL", "name");
//            fields.Add("KOD_GALUZ", "branch_code");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_VID_DIAL", "dict_org_occupation", fields, "KOD_VID_DIAL", null);

//            // S_STATUS
//            fields.Clear();
//            fields.Add("KOD_STATUS", "id");
//            fields.Add("NAME_STATUS", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_STATUS", "dict_org_status", fields, "KOD_STATUS", null);

//            // S_FORM_GOSP
//            fields.Clear();
//            fields.Add("KOD_FORM_GOSP", "id");
//            fields.Add("NAME_FORM_GOSP", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_FORM_GOSP", "dict_org_form_gosp", fields, "KOD_FORM_GOSP", null);

//            // S_FORM_VLASN
//            fields.Clear();
//            fields.Add("KOD_FORM_VLASN", "id");
//            fields.Add("NAME_FORM_VLASN", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");
//            fields.Add("NAME2", "display_name");

//            MigrateTable(connection1NF, "S_FORM_VLASN", "dict_org_ownership", fields, "KOD_FORM_VLASN", null);

//            // S_GOSP_STRUKT
//            fields.Clear();
//            fields.Add("KOD_GOSP_STRUKT", "id");
//            fields.Add("NAME_GOSP_STRUKT", "name");
//            fields.Add("KOD_GALUZ", "industry_code");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_GOSP_STRUKT", "dict_org_gosp_struct", fields, "KOD_GOSP_STRUKT", null);

//            // S_ORGAN
//            fields.Clear();
//            fields.Add("KOD_ORGAN", "id");
//            fields.Add("NAME_ORGAN", "name");
//            fields.Add("KOD_GALUZ", "industry_code");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_ORGAN", "dict_org_organ", fields, "KOD_ORGAN", null);

//            // S_GALUZ
//            fields.Clear();
//            fields.Add("KOD_GALUZ", "id");
//            fields.Add("NAME_GALUZ", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_GALUZ", "dict_org_industry", fields, "KOD_GALUZ", null);

//            // S_ADDITION_PRIZNAK
//            fields.Clear();
//            fields.Add("KOD_ADDITION_PRIZNAK", "id");
//            fields.Add("NAME_ADDITION_PRIZNAK", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_ADDITION_PRIZNAK", "dict_org_priznak", fields, "KOD_ADDITION_PRIZNAK", null);

//            // S_PRIKMET
//            fields.Clear();
//            fields.Add("KOD_PRIKMET", "id");
//            fields.Add("NAME_PRIKM_UKR_M", "name_ukr_male");
//            fields.Add("NAME_PRIKM_UKR_W", "name_ukr_fem");
//            fields.Add("NAME_PRIKM_UKR_O", "name_ukr_single");
//            fields.Add("NAME_PRIKM_UKR_MN", "name_ukr_mult");
//            fields.Add("NAME_PRIKM_RUS_M", "name_rus_male");
//            fields.Add("NAME_PRIKM_RUS_W", "name_rus_fem");
//            fields.Add("NAME_PRIKM_RUS_O", "name_rus_single");
//            fields.Add("NAME_PRIKM_RUS_MN", "name_rus_mult");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_PRIKMET", "dict_org_title_form", fields, "KOD_PRIKMET", null);

//            // S_PRIZNAK_1
//            fields.Clear();
//            fields.Add("KOD_PRIZNAK_1", "id");
//            fields.Add("NAME_PRIZNAK_1", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_PRIZNAK_1", "dict_org_form_1nf", fields, "KOD_PRIZNAK_1", null);

//            // S_VIDOM_NAL
//            fields.Clear();
//            fields.Add("KOD_VIDOM_NAL", "id");
//            fields.Add("NAME_VIDOM_NAL", "name");
//            fields.Add("FULL_NAME", "full_name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_VIDOM_NAL", "dict_org_vedomstvo", fields, "KOD_VIDOM_NAL", null);

//            // S_IMEN
//            fields.Clear();
//            fields.Add("KOD_IMEN", "id");
//            fields.Add("NAME_IMEN_RUS", "name_rus");
//            fields.Add("NAME_IMEN_UKR", "name_ukr");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_IMEN", "dict_org_title", fields, "KOD_IMEN", null);

//            // S_ORG_FORM
//            fields.Clear();
//            fields.Add("KOD_ORG_FORM", "id");
//            fields.Add("NAME_ORG_FORM", "name");
//            fields.Add("KOD_ORG_FORM_STAT", "stat_code");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_ORG_FORM", "dict_org_form", fields, "KOD_ORG_FORM", null);

//            // S_VID_GOSP_STR
//            fields.Clear();
//            fields.Add("KOD_VID_GOSP_STR", "id");
//            fields.Add("NAME_VID_GOSP_STR", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_VID_GOSP_STR", "dict_org_gosp_struct_type", fields, "KOD_VID_GOSP_STR", null);

//            // S_AKCIA
//            fields.Clear();
//            fields.Add("KOD_AKCIA", "id");
//            fields.Add("NAME_AKCIA", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_AKCIA", "dict_org_share_type", fields, "KOD_AKCIA", null);

//            // S_VIDDIL
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("NAME_OUTPUT", "display_name");
//            fields.Add("SHORT_NAME_OUTPUT", "short_name");

//            MigrateTable(connection1NF, "S_VIDDIL", "dict_otdel_gukv", fields, "KOD", null);

//            // SORG_1NF
//            FieldMappings.Create1NFOrgFieldMapping(fields, enable1NFNewFieldsImport);

//            MigrateTable(connection1NF, "SORG_1NF", "organizations", fields, "KOD_OBJ", null);

//            if (!testOnly)
//            {
//                // TAR_SORG_1NF - archive states for 1NF organizations
//                fields.Remove("ARCH_KOD");
//                fields.Remove("IN_ARCH");
//                fields.Remove("IS_AREND");

//                fields.Add("CRE_DT", "archive_create_date");
//                fields.Add("CRE_ISP", "archive_create_user");
//                fields.Add("C_KOD", "archive_link_code");
//                fields.Add("NEXT_KOD", "archive_link_code_next");
//                fields.Add("PREV_KOD", "archive_link_code_prev");

//                MigrateTable(connection1NF, "TAR_SORG_1NF", "arch_organizations", fields, "KOD_OBJ", null);
//            }

//            // Merge columns KOD_VID_PREDPR and KOD_ORG_FORM, because they contain the same data
//            Merge1NFOrgFormToSingleColumn();

//            // Correct archive state dates and user names
//            ExecuteStatementInSQLServer(@"UPDATE org_next SET modified_by = org_prev.archive_create_user, modify_date = org_prev.archive_create_date
//                   FROM arch_organizations org_next INNER JOIN arch_organizations org_prev ON org_next.archive_link_code = org_prev.archive_link_code_next");
//        }

//        /// <summary>
//        /// Merge columns KOD_VID_PREDPR and KOD_ORG_FORM from the SORG_1NF table into a single column
//        /// </summary>
//        private void Merge1NFOrgFormToSingleColumn()
//        {
//            string orgKindSmallBusiness = GUKV.DataMigration.Properties.Resources.OrgKindSmallBusiness;
//            string orgKindDeputy = GUKV.DataMigration.Properties.Resources.OrgKindDeputy;

//            // Add the missing values from S_VID_PREDPR to dict_org_form
//            int idMalePidpr = FindDictionaryEntryIdInSQLServer("dict_org_form", "id", "name", orgKindSmallBusiness);

//            if (idMalePidpr < 0)
//            {
//                idMalePidpr = 3;

//                AddDictionaryEntryInSQLServer("dict_org_form", "id", "name", idMalePidpr, orgKindSmallBusiness);
//            }

//            int idDeputy = FindDictionaryEntryIdInSQLServer("dict_org_form", "id", "name", orgKindDeputy);

//            if (idDeputy < 0)
//            {
//                idDeputy = 11;

//                AddDictionaryEntryInSQLServer("dict_org_form", "id", "name", idDeputy, orgKindDeputy);
//            }

//            // Save mapping for future use (the same dictionary is used in 'Rosporadjennia' database)
//            mappingVidPredprToOrgForm.Add(2, 1100);
//            mappingVidPredprToOrgForm.Add(4, 150);
//            mappingVidPredprToOrgForm.Add(5, 830);
//            mappingVidPredprToOrgForm.Add(6, 845);
//            mappingVidPredprToOrgForm.Add(8, 540);
//            mappingVidPredprToOrgForm.Add(9, 520);
//            mappingVidPredprToOrgForm.Add(10, 840);

//            // Update the values in the 'temp_org_kind_id' to match the S_ORG_FORM values
//            foreach (KeyValuePair<int, int> mapping in mappingVidPredprToOrgForm)
//            {
//                Replace1NFOrgFormDictionaryValue(mapping.Key, mapping.Value);
//            }

//            // Tempoary column 'temp_org_kind_id' is removed in the 'DataMigrationPostProcessing.sql' script
//        }

//        private void Replace1NFOrgFormDictionaryValue(int valueFrom, int valueTo)
//        {
//            ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase +
//                ".dbo.organizations SET temp_org_kind_id = " + valueTo.ToString() +
//                " WHERE temp_org_kind_id = " + valueFrom.ToString());

//            ExecuteStatementInSQLServer("UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase +
//                ".dbo.arch_organizations SET temp_org_kind_id = " + valueTo.ToString() +
//                " WHERE temp_org_kind_id = " + valueFrom.ToString());
//        }

//        private void Migrate1NFBalans()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // SO26
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SO26", "dict_balans_o26", fields, "KOD", null);

//            // SOBTI
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SOBTI", "dict_balans_bti", fields, "KOD", null);

//            // SGRPURPOSE
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SGRPURPOSE", "dict_balans_purpose_group", fields, "KOD", null);

//            // SPURPOSE
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("GR", "group_code");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SPURPOSE", "dict_balans_purpose", fields, "KOD", null);

//            // SPRAVO
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SPRAVO", "dict_balans_ownership_type", fields, "KOD", null);

//            // S_FORM_GIV
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_FORM_GIV", "dict_balans_form_giver", fields, "KOD", null);

//            // S_UPD_SOURCE
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_UPD_SOURCE", "dict_balans_update_src", fields, "KOD", null);

//            // S_VIDCH_TYPE
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "S_VIDCH_TYPE", "dict_balans_vidch_type", fields, "KOD", null);

//            // BALANS_1NF
//            FieldMappings.Create1NFBalansFieldMapping(fields, testOnly, enable1NFNewFieldsImport);

//            MigrateTable(connection1NF, "BALANS_1NF", "balans", fields, "ID", null);

//            // Update [balans] table to mark objects with sqr_total = 0 as deleted. This is done
//            // in accordance with the business logic of balance holders disposing of duplicate
//            // objects (most of which were created earlier by the 1NF import utility back in 2013)
//            // by resetting sqr_total field to zero instead of going through the full-blown
//            // object disposal process.

//            int objectsMarkedDeleted = ExecuteStatementInSQLServer(
//                "UPDATE balans SET is_deleted = 1 WHERE sqr_total = 0 AND ISNULL(is_deleted, 0) = 0"
//                );
//            if (objectsMarkedDeleted > 0)
//            {
//                logger.WriteInfo(
//                    string.Format("{0} об'єктів відмічено видаленими завдяки sqr_total=0", objectsMarkedDeleted)
//                    );
//            }

//            if (!testOnly)
//            {
//                // TAR_BALANS_1NF - balans archive states
//                fields.Remove("IN_ARCH");
//                fields.Remove("ARCH_KOD");

//                fields.Add("CRE_DT", "archive_create_date");
//                fields.Add("CRE_ISP", "archive_create_user");
//                fields.Add("C_KOD", "archive_link_code");
//                fields.Add("NEXT_KOD", "archive_link_code_next");
//                fields.Add("PREV_KOD", "archive_link_code_prev");

//                MigrateTable(connection1NF, "TAR_BALANS_1NF", "arch_balans", fields, "C_KOD", null);

//                // Correct archive state dates and user names
//                ExecuteStatementInSQLServer(@"UPDATE bal_next SET modified_by = bal_prev.archive_create_user, modify_date = bal_prev.archive_create_date
//                    FROM arch_balans bal_next INNER JOIN arch_balans bal_prev ON bal_next.archive_link_code = bal_prev.archive_link_code_next");
//            }
//        }

//        private void Migrate1NFArenda()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // SKINDDOG
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SKINDDOG", "dict_arenda_agreement_kind", fields, "KOD", null);

//            // S_WAYPRIVAT
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "S_WAYPRIVAT", "dict_arenda_privat_kind", fields, "KOD", null);

//            // S_VIDPLATA
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "S_VIDPLATA", "dict_arenda_payment_type", fields, "KOD", null);

//            // ARENDA1NF
//            FieldMappings.Create1NFArendaFieldMapping(fields, testOnly, enable1NFNewFieldsImport);

//            MigrateTable(connection1NF, "ARENDA1NF", "arenda", fields, "ID", null);

//            if (!testOnly)
//            {
//                // TAR_ARENDA1NF - arenda archive states
//                fields.Remove("ARCH_KOD");
//                fields.Remove("BALID");
//                fields.Remove("WAYPRIVAT");
//                fields.Remove("DATE_EXP");
//                fields.Remove("NUM_AKT");
//                fields.Remove("DATE_AKT");
//                fields.Remove("NUM_BTI");
//                fields.Remove("DATE_BTI");
//                fields.Remove("SER_SVID");
//                fields.Remove("NUM_SVID");
//                fields.Remove("DATE_SVID");

//                fields.Add("CRE_DT", "archive_create_date");
//                fields.Add("CRE_ISP", "archive_create_user");
//                fields.Add("C_KOD", "archive_link_code");
//                fields.Add("NEXT_KOD", "archive_link_code_next");
//                fields.Add("PREV_KOD", "archive_link_code_prev");

//                MigrateTable(connection1NF, "TAR_ARENDA1NF", "arch_arenda", fields, "C_KOD", null);

//                // Correct archive state dates and user names
//                ExecuteStatementInSQLServer(@"UPDATE ar_next SET modified_by = ar_prev.archive_create_user, modify_date = ar_prev.archive_create_date
//                   FROM arch_arenda ar_next INNER JOIN arch_arenda ar_prev ON ar_next.archive_link_code = ar_prev.archive_link_code_next");
//            }
//        }

//        private void Migrate1NFArendaNotes()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // ARENDA_PRIM
//            FieldMappings.Create1NFArendaNotesFieldMapping(fields, enable1NFNewFieldsImport);

//            MigrateTable(connection1NF, "ARENDA_PRIM", "arenda_notes", fields, "ID", null);

//            if (!testOnly)
//            {
//                // TAR_ARENDA_PRIM
//                fields.Add("ARENDA_ARCH_ID", "archive_arenda_link_code");

//                List<string> requiredFields = new List<string>();

//                requiredFields.Add("ARENDA_ARCH_ID");

//                MigrateTable(connection1NF, "TAR_ARENDA_PRIM", "arch_arenda_notes", fields, "ARCH_ID", requiredFields);
//            }
//        }

//        private void Migrate1NFArendaPidstava()
//        {
//            if (testOnly)
//            {
//                return;
//            }

//            logger.WriteInfo("Migrating reasons for rent from the ARRISH table");

//            Dictionary<int, string> pidstavaCache = new Dictionary<int, string>();

//            string querySelect = "SELECT ARENDA, DOCUMENT_NAME FROM ARRISH";

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            object dataArendaId = reader.IsDBNull(0) ? null : reader.GetValue(0);
//                            object dataDocName = reader.IsDBNull(1) ? null : reader.GetValue(1);

//                            if (dataArendaId is int && dataDocName is string)
//                            {
//                                int arendaId = (int)dataArendaId;
//                                string docName = (string)dataDocName;

//                                docName = docName.Trim();

//                                if (docName.Length > 0)
//                                {
//                                    string existingDocs = "";

//                                    if (pidstavaCache.TryGetValue(arendaId, out existingDocs))
//                                    {
//                                        existingDocs += ", ";
//                                        existingDocs += docName;

//                                        pidstavaCache[arendaId] = existingDocs;
//                                    }
//                                    else
//                                    {
//                                        pidstavaCache[arendaId] = docName;
//                                    }
//                                }
//                            }
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }

//            // Update the 'arenda' table using gathered information
//            Dictionary<string, object> values = new Dictionary<string, object>();

//            foreach (KeyValuePair<int, string> pair in pidstavaCache)
//            {
//                values["pidstava_display"] = pair.Value;

//                string queryUpdate = GetUpdateStatement("arenda", values, "id", pair.Key);

//                try
//                {
//                    using (SqlCommand cmd = new SqlCommand(queryUpdate, connectionSqlClient))
//                    {
//                        cmd.ExecuteNonQuery();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ShowSqlErrorMessageDlg(queryUpdate, ex.Message);

//                    // Abort migration correctly
//                    throw new MigrationAbortedException();
//                }
//            }
//        }

//        private void Migrate1NFDocuments()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();
//            List<string> requiredFields = new List<string>();

//            // SKINDDOK
//            fields.Clear();
//            fields.Add("ID", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "SKINDDOK", "dict_doc_kind", fields, "ID", null);

//            // S_DEPEND_KIND
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "S_DEPEND_KIND", "dict_doc_depend_kind", fields, "KOD", null);

//            // SVKOMIS
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            requiredFields.Clear();
//            requiredFields.Add("NAME");

//            MigrateTable(connection1NF, "SVKOMIS", "dict_doc_commission", fields, "KOD", requiredFields);

//            // ROZP_DOK
//            fields.Clear();
//            fields.Add("DOK_ID", "id");
//            fields.Add("DOKKIND", "kind_id");
//            fields.Add("DOKDATA", "doc_date");
//            fields.Add("DOKNUM", "doc_num");
//            fields.Add("DOKTEMA", "topic");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");
//            fields.Add("WORKED", "is_not_in_work");
//            fields.Add("IS_REC", "is_reconstruct");
//            fields.Add("DATA_NADH", "receive_date");
//            fields.Add("DATE_PRIV", "priv_date");

//            MigrateTable(connection1NF, "ROZP_DOK", "documents", fields, "DOK_ID", null);

//            // Set the default "general document kind" for all documents from 1NF
//            string statement = "UPDATE " + Properties.Settings.Default.ConnectionSQLServerDatabase +
//                ".dbo.documents SET general_kind_id = " + docGeneralCategory1NF.ToString() +
//                " WHERE (general_kind_id IS NULL)";

//            ExecuteStatementInSQLServer(statement);

//            // DOK_DEPEND
//            fields.Clear();

//            fields.Add("MASTER", "master_doc_id");
//            fields.Add("SLAVE", "slave_doc_id");
//            fields.Add("VID", "depend_kind_id");
//            fields.Add("ISP", "modified_by");

//            requiredFields.Clear();
//            requiredFields.Add("MASTER");
//            requiredFields.Add("SLAVE");

//            MigrateTable(connection1NF, "DOK_DEPEND", "doc_dependencies", fields, "ID", requiredFields);

//            // S_DOK_CHANGE
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "S_DOK_CHANGE", "dict_doc_change_type", fields, "KOD", null);

//            // OBJECT_DOKS_PROPERTIES
//            fields.Clear();

//            fields.Add("OBJECT_KOD", "building_id");
//            fields.Add("DOK_ID", "document_id");
//            fields.Add("NAME", "obj_name");
//            fields.Add("CHARACTERISTIC", "obj_description");
//            fields.Add("YEAR_BUILD", "obj_build_year");

//            fields.Add("YEAR_EXPL", "obj_expl_enter_year");
//            fields.Add("LEN", "obj_length");
//            fields.Add("SUMMA_BALANS", "cost_balans");
//            fields.Add("SUMMA_ZNOS", "cost_znos");

//            fields.Add("SUMMA_ZAL", "cost_zalishkova");
//            fields.Add("EXP_VART", "cost_expert");
//            fields.Add("ROOM_COUNT", "num_rooms");
//            fields.Add("FLOORS", "num_floors");
//            fields.Add("ARRANGEMENT", "obj_location");

//            fields.Add("GRPURP", "purpose_group_id");
//            fields.Add("PURPOSE", "purpose_id");
//            fields.Add("TEXSTAN", "tech_condition_id");
//            fields.Add("OBJKIND", "object_kind_id");
//            fields.Add("OBJTYPE", "object_type_id");

//            fields.Add("SQUARE", "sqr_obj");
//            fields.Add("CLR_SQUARE", "sqr_free");
//            fields.Add("LSQUARE", "sqr_habit");
//            fields.Add("NLSQARE", "sqr_non_habit");
//            fields.Add("WORKED", "is_not_in_work");

//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");
//            fields.Add("DODATOK", "doc_dodatok");
//            fields.Add("TAB", "doc_tab");
//            fields.Add("POS", "doc_pos");

//            /*
//            // Those fields duplicate the corresponding fields in the 'buildings' table
//            fields.Add("OBJ_ULNAME", "addr_street_name");
//            fields.Add("OBJ_ULNAME2", "addr_street_name2");
//            fields.Add("OBJ_NOMER1", "addr_nomer1");
//            fields.Add("OBJ_NOMER2", "addr_nomer2");
//            fields.Add("OBJ_NOMER3", "addr_nomer3");
//            fields.Add("ADRDOP", "addr_misc");
//            */

//            fields.Add("DOK_CHANGE", "doc_change_type_id");
//            fields.Add("DOK_CH_NUM", "doc_change_num");
//            fields.Add("DOK_CH_DATE", "doc_change_date");
//            fields.Add("CH_DOK_ID", "doc_change_doc_id");

//            fields.Add("FORM_VLASN", "form_ownership_id");
//            fields.Add("BAL_FLAG", "is_on_balans");
//            fields.Add("ORG", "org_id");
//            fields.Add("LAST_ORG", "last_org_id");
//            fields.Add("EXP_DATE", "date_expert");

//            fields.Add("DATE_PRIV", "date_priv");
//            fields.Add("ID_OZN_PRIV", "ozn_priv_id");

//            // This is a rempoary field; it is removed in the post-processing script
//            fields.Add("ID", "old_id");

//            if (!testOnly)
//            {
//                fields.Add("DESCRIPTION", "note");
//            }

//            MigrateTable(connection1NF, "OBJECT_DOKS_PROPERTIES", "building_docs", fields, "ID", null);
//        }

//        private void Migrate1NKvedDictionary()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // S_KVED
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("KOD_KVED", "code");
//            fields.Add("NU", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            MigrateTable(connection1NF, "S_KVED", "dict_kved", fields, "KOD", null);
//        }

//        private void Migrate1NBalansDocs()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            fields.Add("BALID", "balans_id");
//            fields.Add("OBJECT_KOD", "building_id");
//            fields.Add("FLAG", "link_kind");
//            fields.Add("DOK_ID", "building_docs_id");
//            fields.Add("SORT_FLD", "sort_field");

//            string querySelect = GetSelectStatement("BALANS_DOK_PROP", fields, "", null);

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        DataReaderAdapter adapter = new DataReaderAdapter(reader);

//                        // Map building_docs IDs
//                        adapter.AddColumnMapping("DOK_ID", mappingBuildingDocIds);

//                        BulkMigrateDataReader("BALANS_DOK_PROP", "balans_docs", adapter, fields);

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }

//            // Migrate archive states from TAR_BALANS_DOK_PROP table
//            fields.Remove("SORT_FLD");
//            fields.Add("BAL_AR_ID", "archive_balans_link_code");

//            List<string> requiredFields = new List<string>();
//            requiredFields.Add("BAL_AR_ID");

//            querySelect = GetSelectStatement("TAR_BALANS_DOK_PROP", fields, "AR_ID", requiredFields);

//            try
//            {
//                using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
//                {
//                    using (FbDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        DataReaderAdapter adapter = new DataReaderAdapter(reader);

//                        // Map building_docs IDs
//                        adapter.AddColumnMapping("DOK_ID", mappingBuildingDocIds);

//                        BulkMigrateDataReader("TAR_BALANS_DOK_PROP", "arch_balans_docs", adapter, fields);

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }
//        }

//        private void Prepare1NFBuildingDocsIdMapping()
//        {
//            string querySelect = "SELECT id, old_id FROM building_docs";

//            try
//            {
//                using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
//                {
//                    using (SqlDataReader reader = commandSelect.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
//                            object dataOldId = reader.IsDBNull(1) ? null : reader.GetValue(1);

//                            if (dataId is int && dataOldId is int)
//                            {
//                                mappingBuildingDocIds[(int)dataOldId] = (int)dataId;
//                            }
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(querySelect, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }
//        }

//        private void Migrate1NFCopiedDictionaries()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // SUL
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");
//            fields.Add("STAN", "stan");

//            MigrateTable(connection1NF, "SUL", "dict_1nf_streets", fields, "KOD", null);

//            // S_RAYON2
//            fields.Clear();
//            fields.Add("KOD_RAYON2", "id");
//            fields.Add("NAME_RAYON2", "name");

//            MigrateTable(connection1NF, "S_RAYON2", "dict_1nf_districts2", fields, "KOD_RAYON2", null);

//            // STEXSTAN
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "STEXSTAN", "dict_1nf_tech_state", fields, "KOD", null);

//            // SHISTORY
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "SHISTORY", "dict_1nf_history", fields, "KOD", null);

//            // STYPEOBJ
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "STYPEOBJ", "dict_1nf_object_type", fields, "KOD", null);

//            // SKINDOBJ
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "SKINDOBJ", "dict_1nf_object_kind", fields, "KOD", null);

//            // S_VID_DIAL
//            fields.Clear();
//            fields.Add("KOD_VID_DIAL", "id");
//            fields.Add("NAME_VID_DIAL", "name");
//            fields.Add("KOD_GALUZ", "branch_code");

//            MigrateTable(connection1NF, "S_VID_DIAL", "dict_1nf_org_occupation", fields, "KOD_VID_DIAL", null);

//            // S_GALUZ
//            fields.Clear();
//            fields.Add("KOD_GALUZ", "id");
//            fields.Add("NAME_GALUZ", "name");

//            MigrateTable(connection1NF, "S_GALUZ", "dict_1nf_org_industry", fields, "KOD_GALUZ", null);

//            // S_STATUS
//            fields.Clear();
//            fields.Add("KOD_STATUS", "id");
//            fields.Add("NAME_STATUS", "name");

//            MigrateTable(connection1NF, "S_STATUS", "dict_1nf_org_status", fields, "KOD_STATUS", null);

//            // S_FORM_GOSP
//            fields.Clear();
//            fields.Add("KOD_FORM_GOSP", "id");
//            fields.Add("NAME_FORM_GOSP", "name");

//            MigrateTable(connection1NF, "S_FORM_GOSP", "dict_1nf_org_form_gosp", fields, "KOD_FORM_GOSP", null);

//            // S_FORM_VLASN
//            fields.Clear();
//            fields.Add("KOD_FORM_VLASN", "id");
//            fields.Add("NAME_FORM_VLASN", "name");

//            MigrateTable(connection1NF, "S_FORM_VLASN", "dict_1nf_org_ownership", fields, "KOD_FORM_VLASN", null);

//            // SGRPURPOSE
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "SGRPURPOSE", "dict_1nf_balans_purpose_group", fields, "KOD", null);

//            // SPURPOSE
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("GR", "group_code");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "SPURPOSE", "dict_1nf_balans_purpose", fields, "KOD", null);

//            // S_VIDOM_NAL
//            fields.Clear();
//            fields.Add("KOD_VIDOM_NAL", "id");
//            fields.Add("NAME_VIDOM_NAL", "name");

//            MigrateTable(connection1NF, "S_VIDOM_NAL", "dict_1nf_org_vedomstvo", fields, "KOD_VIDOM_NAL", null);

//            // SKINDDOK
//            fields.Clear();
//            fields.Add("ID", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "SKINDDOK", "dict_1nf_doc_kind", fields, "ID", null);

//            // S_OZNAKA_BUD
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "S_OZNAKA_BUD", "dict_1nf_facade", fields, "KOD", null);
//        }

//        #endregion (1NF database migration)
//    }
}
