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
//        #region Assessment database migration

//        private void MigrateAssessment()
//        {
//            logger.WriteInfo("=== Migrating database 'Assessment' ===");

//            MigrateAssessmentDictionaries();
//            MigrateAssessmentTables();

//            GenerateAssessmentDetailGrouping();
//            GenerateAssessmentInDocGrouping();
//            GenerateAssessmentOutDocGrouping();
//        }

//        private void MigrateAssessmentDictionaries()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // S_VID_OB_EXP_OC
//            fields.Clear();
//            fields.Add("S_VID_OB_EXP_OC_ID", "id");
//            fields.Add("NAME", "name");
//            fields.Add("NAME_SHORT", "short_name");

//            MigrateTable(connection1NF, "S_VID_OB_EXP_OC", "dict_expert_obj_type", fields, "S_VID_OB_EXP_OC_ID", null);

//            // S_REZENZ
//            fields.Clear();
//            fields.Add("S_REZENZ_ID", "id");
//            fields.Add("NAME", "name");
//            fields.Add("DELETED", "is_deleted");

//            MigrateTable(connection1NF, "S_REZENZ", "dict_expert_rezenz", fields, "S_REZENZ_ID", null);

//            // S_KORESPONDENT
//            fields.Clear();
//            fields.Add("S_KORESPONDENT_ID", "id");
//            fields.Add("NAME", "name");

//            MigrateTable(connection1NF, "S_KORESPONDENT", "dict_expert_korr", fields, "S_KORESPONDENT_ID", null);

//            // S_VID_REZENZ
//            fields.Clear();
//            fields.Add("S_VID_REZENZ_ID", "id");
//            fields.Add("NAME", "name");
//            fields.Add("COLOR", "color_rgb");

//            MigrateTable(connection1NF, "S_VID_REZENZ", "dict_expert_rezenz_type", fields, "S_VID_REZENZ_ID", null);

//            // S_EXPERT
//            fields.Clear();
//            fields.Add("KOD", "id");
//            fields.Add("FULL_NAME", "full_name");
//            fields.Add("SHORT_NAME", "short_name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");
//            fields.Add("FIO_BOSS", "fio_boss");
//            fields.Add("TEL_BOSS", "tel_boss");
//            fields.Add("KOD_RAYON2", "addr_district_id");
//            fields.Add("ULKOD", "addr_street_id");
//            fields.Add("POST_INDEX", "addr_zip_code");
//            fields.Add("NOMER_DOMA", "addr_number");
//            fields.Add("SERTIFIKAT_NOM", "certificate_num");
//            fields.Add("SERTIFIKAT_DT", "certificate_date");
//            fields.Add("SERTIFIKAT_FINISH_DT", "certificate_end_date");
//            fields.Add("SPRAVA_NOM", "case_num");
//            fields.Add("FULL_ADDRESS", "addr_full");
//            fields.Add("PRIMITKA", "note");
//            fields.Add("DELETED", "is_deleted");
//            fields.Add("DTDELETE", "del_date");

//            MigrateTable(connection1NF, "S_EXPERT", "dict_expert", fields, "KOD", null);
//        }

//        private void MigrateAssessmentTables()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // NO_PRIM_EXP
//            fields.Clear();
//            fields.Add("NO_PRIM_EXP_ID", "id");
//            fields.Add("ORG_AR", "org_renter_id");
//            fields.Add("ORG_AR_NAME", "org_renter_name");
//            fields.Add("ORG_BAL", "org_balans_id");
//            fields.Add("ORG_BAL_NAME", "org_balans_name");

//            fields.Add("OBJECT", "building_id");
//            fields.Add("BALID", "balans_id");
//            fields.Add("KOD_RAYON2", "addr_district_id");
//            fields.Add("S_VID_OB_EXP_OC_ID", "expert_obj_type_id");
//            fields.Add("SQUARE", "obj_square");

//            fields.Add("S_EXPERT_ID", "expert_id");
//            fields.Add("S_EXPERT_NAME", "expert_name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");
//            fields.Add("S_REZENZ_ID", "rezenz_id");

//            fields.Add("OBJECT_ULNAME", "addr_street_name");
//            fields.Add("VART1_USD", "cost_1_usd");
//            fields.Add("VART_PRIM", "cost_prim");
//            fields.Add("DT_EXP", "valuation_date");
//            fields.Add("DT_ITOG", "final_date");

//            fields.Add("ARCH_NOM", "arch_num");
//            fields.Add("ARCH_DT", "arch_date");
//            fields.Add("IN_ARCH", "is_archived");
//            fields.Add("IS_STAND_OC", "is_stand_oc");
//            fields.Add("DELETED", "is_deleted");

//            fields.Add("DTDELETE", "del_date");
//            fields.Add("OBJECT_NOMER1", "addr_number1");
//            fields.Add("OBJECT_NOMER2", "addr_number2");
//            fields.Add("PRIMITKA_TEXT", "note_text");

//            MigrateTable(connection1NF, "NO_PRIM_EXP", "expert_note", fields, "NO_PRIM_EXP_ID", null);

//            // NO_PRIM_EXP_DETAILS
//            fields.Clear();
//            // fields.Add("NO_PRIM_EXP_DETAILS_ID", "id"); - Replaced by an IDENTITY field
//            fields.Add("NO_PRIM_EXP_ID", "expert_note_id");
//            fields.Add("SQUARE", "obj_square");
//            fields.Add("VART1_USD", "cost_1_usd");
//            fields.Add("VART1_USD_FLAG", "cost_1_usd_flag");
//            fields.Add("DT_EXP", "valuation_date");
//            fields.Add("POVERH", "floors");
//            fields.Add("NEV", "purpose");
//            fields.Add("PRIMITKA", "note");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");
//            fields.Add("DELETED", "is_deleted");
//            fields.Add("DTDELETE", "del_date");

//            MigrateTable(connection1NF, "NO_PRIM_EXP_DETAILS", "expert_note_detail", fields, "NO_PRIM_EXP_DETAILS_ID", null);

//            // NO_VHIDNI_DOK
//            fields.Clear();
//            // fields.Add("NO_VHIDNI_DOK_ID", "id"); - Replaced by an IDENTITY field
//            fields.Add("NO_PRIM_EXP_ID", "expert_note_id");
//            fields.Add("DTDOK", "doc_date");
//            fields.Add("NODOC", "doc_num");
//            fields.Add("DTKONTR", "control_date");
//            fields.Add("KORESPONDENT_ID", "korrespondent_id");
//            fields.Add("DT", "modify_date");
//            fields.Add("ISP", "modified_by");
//            fields.Add("MVD_DOK_ID", "mvd_doc_id");
//            fields.Add("DELETED", "is_deleted");
//            fields.Add("DTDELETE", "del_date");
//            fields.Add("REZENZ_NAME", "rezenz_name");

//            MigrateTable(connection1NF, "NO_VHIDNI_DOK", "expert_input_doc", fields, "NO_VHIDNI_DOK_ID", null);

//            // NO_VYHIDNI_DOK
//            fields.Clear();
//            // fields.Add("NO_VYHIDNI_DOK_ID", "id"); - Replaced by an IDENTITY field
//            fields.Add("NO_PRIM_EXP_ID", "expert_note_id");
//            fields.Add("DTDOK", "doc_date");
//            fields.Add("NODOC", "doc_num");
//            fields.Add("S_VID_REZENZ_ID", "rezenz_type_id");
//            fields.Add("DT", "modify_date");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DELETED", "is_deleted");
//            fields.Add("DTDELETE", "del_date");

//            MigrateTable(connection1NF, "NO_VYHIDNI_DOK", "expert_output_doc", fields, "NO_VYHIDNI_DOK_ID", null);
//        }

//        private void GenerateAssessmentDetailGrouping()
//        {
//            DataTable table = new DataTable("expert_note_detail_grouped");

//            table.Columns.Add("expert_note_id", typeof(int));
//            table.Columns.Add("obj_squares", typeof(string));
//            table.Columns.Add("costs_1_usd", typeof(string));
//            table.Columns.Add("valuation_dates", typeof(string));
//            table.Columns.Add("floors", typeof(string));
//            table.Columns.Add("purposes", typeof(string));

//            object[] rowValues = new object[6];

//            string query = @"SELECT
//                d.expert_note_id,
//                d.obj_square,
//                d.cost_1_usd,
//                d.valuation_date,
//                d.floors,
//                d.purpose
//            FROM
//                expert_note_detail d
//            WHERE
//                d.is_deleted = 0
//            ORDER BY
//                d.expert_note_id";

//            int expertNoteId = -1;
//            string objSquares = "";
//            string objCosts = "";
//            string valuationDates = "";
//            string objFloors = "";
//            string objPurposes = "";

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        int noteId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
//                        decimal sqr = reader.IsDBNull(1) ? -1m : reader.GetDecimal(1);
//                        decimal cost1m = reader.IsDBNull(2) ? -1m : reader.GetDecimal(2);
//                        DateTime evalDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
//                        string floor = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim();
//                        string purpose = reader.IsDBNull(5) ? "" : reader.GetString(5).Trim();

//                        if (noteId > 0)
//                        {
//                            // If a new note ID is reached, save the previos one
//                            if (noteId != expertNoteId)
//                            {
//                                if (expertNoteId > 0)
//                                {
//                                    rowValues[0] = expertNoteId;
//                                    rowValues[1] = objSquares;
//                                    rowValues[2] = objCosts;
//                                    rowValues[3] = valuationDates;
//                                    rowValues[4] = objFloors;
//                                    rowValues[5] = objPurposes;

//                                    table.Rows.Add(rowValues);
//                                }

//                                expertNoteId = noteId;
//                                objSquares = "";
//                                objCosts = "";
//                                valuationDates = "";
//                                objFloors = "";
//                                objPurposes = "";
//                            }

//                            // Save the new values
//                            if (sqr >= 0m)
//                            {
//                                if (objSquares.Length > 0)
//                                {
//                                    objSquares += "\n";
//                                }

//                                objSquares += sqr.ToString("########.##");
//                            }

//                            if (cost1m >= 0m)
//                            {
//                                if (objCosts.Length > 0)
//                                {
//                                    objCosts += "\n";
//                                }

//                                objCosts += cost1m.ToString("########.##");
//                            }

//                            if (evalDate > DateTime.MinValue)
//                            {
//                                if (valuationDates.Length > 0)
//                                {
//                                    valuationDates += "\n";
//                                }

//                                valuationDates += evalDate.ToShortDateString();
//                            }

//                            if (floor.Length > 0)
//                            {
//                                if (objFloors.Length > 0)
//                                {
//                                    objFloors += "\n";
//                                }

//                                objFloors += floor;
//                            }

//                            if (purpose.Length > 0)
//                            {
//                                if (objPurposes.Length > 0)
//                                {
//                                    objPurposes += "\n";
//                                }

//                                objPurposes += purpose;
//                            }
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            // Save the last portion of the data
//            if (expertNoteId > 0)
//            {
//                rowValues[0] = expertNoteId;
//                rowValues[1] = objSquares;
//                rowValues[2] = objCosts;
//                rowValues[3] = valuationDates;
//                rowValues[4] = objFloors;
//                rowValues[5] = objPurposes;

//                table.Rows.Add(rowValues);
//            }

//            // Store the table in SQL Server
//            BulkMigrateDataTable("expert_note_detail_grouped", table);
//        }

//        private void GenerateAssessmentInDocGrouping()
//        {
//            DataTable table = new DataTable("expert_input_doc_grouped");

//            table.Columns.Add("expert_note_id", typeof(int));
//            table.Columns.Add("doc_dates", typeof(string));
//            table.Columns.Add("doc_numbers", typeof(string));
//            table.Columns.Add("control_dates", typeof(string));
//            table.Columns.Add("korrespondents", typeof(string));
//            table.Columns.Add("rezenz_names", typeof(string));

//            object[] rowValues = new object[6];

//            string query = @"SELECT
//                eid.expert_note_id,
//                eid.doc_date,
//                eid.doc_num,
//                eid.control_date,
//                dict_expert_korr.name AS 'korrespondent',
//                eid.rezenz_name
//            FROM
//                expert_input_doc eid
//                LEFT OUTER JOIN dict_expert_korr ON dict_expert_korr.id = eid.korrespondent_id
//            WHERE
//                eid.is_deleted = 0
//            ORDER BY
//                eid.expert_note_id";

//            int expertNoteId = -1;
//            string docDates = "";
//            string docNumbers = "";
//            string controlDates = "";
//            string korrespondents = "";
//            string rezenzNames = "";

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        int noteId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
//                        DateTime docDate = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1);
//                        string docNum = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim();
//                        DateTime controlDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
//                        string korrespondent = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim();
//                        string rezenzName = reader.IsDBNull(5) ? "" : reader.GetString(5).Trim();

//                        if (noteId > 0)
//                        {
//                            // If a new note ID is reached, save the previos one
//                            if (noteId != expertNoteId)
//                            {
//                                if (expertNoteId > 0)
//                                {
//                                    rowValues[0] = expertNoteId;
//                                    rowValues[1] = docDates;
//                                    rowValues[2] = docNumbers;
//                                    rowValues[3] = controlDates;
//                                    rowValues[4] = korrespondents;
//                                    rowValues[5] = rezenzNames;

//                                    table.Rows.Add(rowValues);
//                                }

//                                expertNoteId = noteId;
//                                docDates = "";
//                                docNumbers = "";
//                                controlDates = "";
//                                korrespondents = "";
//                                rezenzNames = "";
//                            }

//                            // Save the new values
//                            if (docDate > DateTime.MinValue)
//                            {
//                                if (docDates.Length > 0)
//                                {
//                                    docDates += "\n";
//                                }

//                                docDates += docDate.ToShortDateString();
//                            }

//                            if (controlDate > DateTime.MinValue)
//                            {
//                                if (controlDates.Length > 0)
//                                {
//                                    controlDates += "\n";
//                                }

//                                controlDates += controlDate.ToShortDateString();
//                            }

//                            if (docNum.Length > 0)
//                            {
//                                if (docNumbers.Length > 0)
//                                {
//                                    docNumbers += "\n";
//                                }

//                                docNumbers += docNum;
//                            }

//                            if (korrespondent.Length > 0)
//                            {
//                                if (korrespondents.Length > 0)
//                                {
//                                    korrespondents += "\n";
//                                }

//                                korrespondents += korrespondent;
//                            }

//                            if (rezenzName.Length > 0)
//                            {
//                                if (rezenzNames.Length > 0)
//                                {
//                                    rezenzNames += "\n";
//                                }

//                                rezenzNames += rezenzName;
//                            }
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            // Save the last portion of the data
//            if (expertNoteId > 0)
//            {
//                rowValues[0] = expertNoteId;
//                rowValues[1] = docDates;
//                rowValues[2] = docNumbers;
//                rowValues[3] = controlDates;
//                rowValues[4] = korrespondents;
//                rowValues[5] = rezenzNames;

//                table.Rows.Add(rowValues);
//            }

//            // Store the table in SQL Server
//            BulkMigrateDataTable("expert_input_doc_grouped", table);
//        }

//        private void GenerateAssessmentOutDocGrouping()
//        {
//            DataTable table = new DataTable("expert_output_doc_grouped");

//            table.Columns.Add("expert_note_id", typeof(int));
//            table.Columns.Add("doc_dates", typeof(string));
//            table.Columns.Add("doc_numbers", typeof(string));
//            table.Columns.Add("rezenz_kinds", typeof(string));

//            object[] rowValues = new object[4];

//            string query = @"SELECT
//                eod.expert_note_id,
//                eod.doc_date,
//                eod.doc_num,
//                dict_expert_rezenz_type.name AS 'rezenz_kind'
//            FROM
//                expert_output_doc eod
//                LEFT OUTER JOIN dict_expert_rezenz_type ON dict_expert_rezenz_type.id = eod.rezenz_type_id
//            WHERE
//                eod.is_deleted = 0
//            ORDER BY
//                eod.expert_note_id";

//            int expertNoteId = -1;
//            string docDates = "";
//            string docNumbers = "";
//            string rezenzKinds = "";

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        int noteId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
//                        DateTime docDate = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1);
//                        string docNum = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim();
//                        string rezenzKind = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim();

//                        if (noteId > 0)
//                        {
//                            // If a new note ID is reached, save the previos one
//                            if (noteId != expertNoteId)
//                            {
//                                if (expertNoteId > 0)
//                                {
//                                    rowValues[0] = expertNoteId;
//                                    rowValues[1] = docDates;
//                                    rowValues[2] = docNumbers;
//                                    rowValues[3] = rezenzKinds;

//                                    table.Rows.Add(rowValues);
//                                }

//                                expertNoteId = noteId;
//                                docDates = "";
//                                docNumbers = "";
//                                rezenzKinds = "";
//                            }

//                            // Save the new values
//                            if (docDate > DateTime.MinValue)
//                            {
//                                if (docDates.Length > 0)
//                                {
//                                    docDates += "\n";
//                                }

//                                docDates += docDate.ToShortDateString();
//                            }

//                            if (docNum.Length > 0)
//                            {
//                                if (docNumbers.Length > 0)
//                                {
//                                    docNumbers += "\n";
//                                }

//                                docNumbers += docNum;
//                            }

//                            if (rezenzKind.Length > 0)
//                            {
//                                if (rezenzKinds.Length > 0)
//                                {
//                                    rezenzKinds += "\n";
//                                }

//                                rezenzKinds += rezenzKind;
//                            }
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            // Save the last portion of the data
//            if (expertNoteId > 0)
//            {
//                rowValues[0] = expertNoteId;
//                rowValues[1] = docDates;
//                rowValues[2] = docNumbers;
//                rowValues[3] = rezenzKinds;

//                table.Rows.Add(rowValues);
//            }

//            // Store the table in SQL Server
//            BulkMigrateDataTable("expert_output_doc_grouped", table);
//        }

//        #endregion (Assessment database migration)
//    }
}
