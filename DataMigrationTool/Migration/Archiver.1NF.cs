using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace GUKV.DataMigration
{
    /// <summary>
    /// This helper class creates archive state in 1NF for objects, organizations,
    /// balans and rent agreements
    /// </summary>
    public class Archiver1NF
    {
        #region Member variables

        ///// <summary>
        ///// The list of 1 NF objects that have archived state
        ///// </summary>
        //private Dictionary<int, int> objectArchCodes = new Dictionary<int, int>();

        ///// <summary>
        ///// The list of 1 NF organizations that have archived state
        ///// </summary>
        //private Dictionary<int, int> orgArchCodes = new Dictionary<int, int>();

        ///// <summary>
        ///// The list of 1 NF arenda records that have archived state
        ///// </summary>
        //private Dictionary<int, int> arendaArchCodes = new Dictionary<int, int>();

        ///// <summary>
        ///// The list of 1 NF balans records that have archived state
        ///// </summary>
        //private Dictionary<int, int> balansArchCodes = new Dictionary<int, int>();

        #endregion (Member variables)

        /// <summary>
        /// Default constructor
        /// </summary>
        //public Archiver1NF()
        //{
        //}

        #region Interface

        /// <summary>
        /// This method must be called before Archiver1NF can be used
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        //public void PrepareArchiveObjectList(FbConnection connection1NF)
        //{
        //    PrepareArchiveList(connection1NF, "OBJECT_1NF", "OBJECT_KOD", "ARCH_KOD", objectArchCodes);

        //    PrepareArchiveList(connection1NF, "SORG_1NF", "KOD_OBJ", "ARCH_KOD", orgArchCodes);

        //    PrepareArchiveList(connection1NF, "ARENDA1NF", "ID", "ARCH_KOD", arendaArchCodes);

        //    PrepareArchiveList(connection1NF, "BALANS_1NF", "ID", "ARCH_KOD", balansArchCodes);
        //}

        /// <summary>
        /// Creates a new archive record for th specified object
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        /// <param name="objectId1NF">Object ID in 1NF</param>
        /// <param name="transaction">Transaction (may be 'null')</param>
        /// <returns>ID of the generated archive entry</returns>
//        public int CreateObjectArchiveRecord(FbConnection connection1NF, int objectId1NF, FbTransaction transaction)
//        {
//            int existingArchRecordId = -1;
//            object archRecordPrev = null;
//            object archRecordNext = null;

//            if (objectArchCodes.TryGetValue(objectId1NF, out existingArchRecordId))
//            {
//                // Cool ;)
//            }
//            else
//            {
//                // The cache may be empty; try to get the value from the table
//                using (FbCommand command = new FbCommand("SELECT OBJECT_KOD, ARCH_KOD FROM OBJECT_1NF WHERE OBJECT_KOD = " + objectId1NF.ToString(), connection1NF))
//                {
//                    command.Transaction = transaction;

//                    using (FbDataReader reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            if (!reader.IsDBNull(1))
//                                existingArchRecordId = reader.GetInt32(1);
//                        }

//                        reader.Close();
//                    }
//                }
//            }

//            // Get the properties of previous archive record
//            if (existingArchRecordId > 0)
//            {
//                using (FbCommand c = new FbCommand("SELECT PREV_KOD, NEXT_KOD FROM TAR_OBJECT_1NF WHERE C_KOD = " +
//                    existingArchRecordId.ToString(), connection1NF))
//                {
//                    c.Transaction = transaction;

//                    using (FbDataReader r = c.ExecuteReader())
//                    {
//                        if (r.Read())
//                        {
//                            archRecordPrev = r.IsDBNull(0) ? null : r.GetValue(0);
//                            archRecordNext = r.IsDBNull(1) ? null : r.GetValue(1);
//                        }

//                        r.Close();
//                    }
//                }
//            }

//            // Generate a new ID for archive entry
//            int newArchEntryId = GenerateNewId1NF(connection1NF, "GEN_TAR_OBJECT_1NF_ID", transaction);

//            // Format the SQL statement
//            string insertStatement = @"INSERT INTO TAR_OBJECT_1NF
//                (OBJECT_KOD, OBJECT_KODSTAN, MAIN_OBJECT_KOD, ADR_REES_KOD, ADR_REES_KODSTAN, REALSTAN, ULNAME, NOMER1, NOMER2, NOMER3,
//                ADRDOP, SZAG, SPIDV, TEXSTAN, DT_BEG, PNUM, DT_END, BUDYEAR, HISTORY, TYPOBJ, VIDOBJ, KODBTI, DISTR, IKOD, NS,
//                CHARACTERISTIC, STAN_YEAR, ZEMLIA, DT, ISP, KADASTR, BALANS_VARTIST, NEWDISTR, ULKOD, MAPKOD, PHOTO, OLD_DT, OLD_ISP,
//                NN, V_MK, V_DK, V_RK, V_IN, UPDPR, V_AR, V_OB, V_PR, DELETED, DTDELETE, OZNAKA_BUD, DODAT_VID, NOMER_INT, KOD_OATUU,
//                V_JIL, V_NEJ, ULNAME2, ULKOD2, FULL_ULNAME, EIS_EXPL_ENTER_YEAR, EIS_BASEMENT_EXISTS, EIS_LOFT_EXISTS, EIS_LOFT_SQUARE, EIS_MODIFIED_BY,
//                C_KOD, PREV_KOD, NEXT_KOD, CRE_DT, CRE_ISP)
//            SELECT
//                OBJECT_KOD, OBJECT_KODSTAN, MAIN_OBJECT_KOD, ADR_REES_KOD, ADR_REES_KODSTAN, REALSTAN, ULNAME, NOMER1, NOMER2, NOMER3,
//                ADRDOP, SZAG, SPIDV, TEXSTAN, DT_BEG, PNUM, DT_END, BUDYEAR, HISTORY, TYPOBJ, VIDOBJ, KODBTI, DISTR, IKOD, NS,
//                CHARACTERISTIC, STAN_YEAR, ZEMLIA, DT, ISP, KADASTR, BALANS_VARTIST, NEWDISTR, ULKOD, MAPKOD, PHOTO, OLD_DT, OLD_ISP,
//                NN, V_MK, V_DK, V_RK, V_IN, UPDPR, V_AR, V_OB, V_PR, DELETED, DTDELETE, OZNAKA_BUD, DODAT_VID, NOMER_INT, KOD_OATUU,
//                V_JIL, V_NEJ, ULNAME2, ULKOD2, FULL_ULNAME, EIS_EXPL_ENTER_YEAR, EIS_BASEMENT_EXISTS, EIS_LOFT_EXISTS, EIS_LOFT_SQUARE, EIS_MODIFIED_BY,
//                @archid, @previd, @nextid, @curdate, @isp
//            FROM
//                OBJECT_1NF
//            WHERE
//                OBJECT_KOD = @objid";

//            // Firebird 1.0 fails on INSERT - SELECT query with NULL parameters. So we have to emulate them
//            insertStatement = insertStatement.Replace("@archid", newArchEntryId.ToString());
//            insertStatement = insertStatement.Replace("@objid", objectId1NF.ToString());
//            insertStatement = insertStatement.Replace("@previd", (existingArchRecordId > 0) ? existingArchRecordId.ToString() : "NULL");
//            insertStatement = insertStatement.Replace("@nextid", (archRecordNext == null) ? "NULL" : archRecordNext.ToString());

//            using (FbCommand command = new FbCommand(insertStatement, connection1NF))
//            {
//                command.Transaction = transaction;
//                command.Parameters.Add(new FbParameter("curdate", DateTime.Now.Date));
//                command.Parameters.Add(new FbParameter("isp", "Auto-import"));
//                command.ExecuteNonQuery();
//            }

//            // Update the previous archive record
//            if (existingArchRecordId > 0)
//            {
//                using (FbCommand cmd = new FbCommand("UPDATE TAR_OBJECT_1NF SET NEXT_KOD = @nextid WHERE C_KOD = " +
//                    existingArchRecordId.ToString(), connection1NF))
//                {
//                    cmd.Transaction = transaction;
//                    cmd.Parameters.Add(new FbParameter("nextid", newArchEntryId));
//                    cmd.ExecuteNonQuery();
//                }
//            }

//            // Save the last archived state for this object
//            objectArchCodes[objectId1NF] = newArchEntryId;

//            return newArchEntryId;
//        }

        /// <summary>
        /// Creates a new archive record for organization
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        /// <param name="organizationId1NF">Organization ID in 1NF</param>
        /// <param name="transaction">Transaction (may be 'null')</param>
        /// <returns>ID of the generated archive entry</returns>
//        public int CreateOrganizationArchiveRecord(FbConnection connection1NF, int organizationId1NF, FbTransaction transaction)
//        {
//            int existingArchRecordId = -1;
//            object archRecordPrev = null;
//            object archRecordNext = null;

//            if (orgArchCodes.TryGetValue(organizationId1NF, out existingArchRecordId))
//            {
//                // Cool ;)
//            }
//            else
//            {
//                // The cache may be empty; try to get the value from the table
//                using (FbCommand command = new FbCommand("SELECT KOD_OBJ, ARCH_KOD FROM SORG_1NF WHERE KOD_OBJ = " + organizationId1NF.ToString(), connection1NF))
//                {
//                    command.Transaction = transaction;

//                    using (FbDataReader reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            if (!reader.IsDBNull(1))
//                                existingArchRecordId = reader.GetInt32(1);
//                        }

//                        reader.Close();
//                    }
//                }
//            }

//            // Get the properties of previous archive record
//            if (existingArchRecordId > 0)
//            {
//                using (FbCommand c = new FbCommand("SELECT PREV_KOD, NEXT_KOD FROM TAR_SORG_1NF WHERE C_KOD = " +
//                    existingArchRecordId.ToString(), connection1NF))
//                {
//                    c.Transaction = transaction;

//                    using (FbDataReader r = c.ExecuteReader())
//                    {
//                        if (r.Read())
//                        {
//                            archRecordPrev = r.IsDBNull(0) ? null : r.GetValue(0);
//                            archRecordNext = r.IsDBNull(1) ? null : r.GetValue(1);
//                        }

//                        r.Close();
//                    }
//                }
//            }

//            // Generate a new ID for archive entry
//            int newArchEntryId = GenerateNewId1NF(connection1NF, "GEN_TAR_SORG_1NF", transaction);

//            // Format the SQL statement
//            string insertStatement = @"INSERT INTO TAR_SORG_1NF
//                (KOD_OBJ, KOD_STAN, LAST_SOST, KOD_PRIZ_PR_KOM, KOD_VID_DIAL, KOD_STATUS, KOD_EK_PLAN_ZONA, KOD_FORM_GOSP,
//                KOD_VID_PREDPR, KOD_FORM_VLASN, KOD_VLASN_VIB, KOD_PIDVID_DIAL, KOD_SFERA_UPR, KOD_GOSP_STRUKT, KOD_ORGAN,
//                KOD_GALUZ, KOD_POTOCH_STAN, KOD_PRIZ_NADH, KOD_PRIZ_VIB, NOMER_OBJ, KOD_ZKPO, KOD_RAYON, NAME_UL, NOMER_DOMA,
//                NOMER_KORPUS, POST_INDEX, FIO_BOSS, FIO_BUH, DATE_NADHOD, DATE_VIBUT, KOLVO_BUD, USER_KOREG, DATE_KOREG,
//                TEL_BOSS, TEL_BUH, FULL_NAME_OBJ, SHORT_NAME_OBJ, KOD_DOCUM, KOD_OLD_GOSP_STRUKT, KOD_OLD_ORGAN,
//                KOD_ADDITION_PRIZNAK, KOD_OLD_PIDVID_DIAL, KOD_OLD_GALUZ, KOD_OLD_VID_DIAL, KOD_PRIKMET, KOD_PRIZNAK_1,
//                KOD_VIDOM_NAL, KOD_IMEN, KOD_ORG_FORM, KOD_VID_GOSP_STR, FIND_NAME_OBJ, NAME_DAVAL_VIDM, ADDITIONAL_ADRESS,
//                TELEFAX, REESORG, REESNO, REESDT, LYEAR, PL_BALANS, PL_VIROB_PRIZN, PL_NOT_VIROB_PRIZN, PL_ARENDA_VILNA,
//                PL_ZAGAL, PL_ARENDUETSYA, PL_PRIVAT, PL_NADANA_V_A, KOLVO_OBJ, NOMER_DOMA2, KOD_KVED, PERCENT_ACTIONS,
//                DT_STAT_DOVIDKA, KOD_RAYON2, DT_BEG_STAN, DT_END_STAN, KOD_AKCIA, DTLYEAR, NO_SVIDOT, KOD_REESORG, KOATUU,
//                DT, ISP, KOD_VID_AKCIA, PERSENT_AKCIA, NAME_BANK, MFO, OTDEL, ACCOUNT, DELETED, DTDELETE, TYPEORG, VIDDIL,
//                SYSTEM_DT, SYSTEM_ISP, FIO_BOSS_R_VIDM, POST_BOSS, POST_BOSS_R_VIDM, DOC_BOSS, DOC_BOSS_R_VIDM, PL_PRODAJ,
//                PL_ZNYATA_Z_BALANSU, PL_SPISANI_ZNESENI, PL_PEREDANA_INSHUM, LIKVID, LIKVID_DATE, PIDP_RDA,
//                EIS_ADDR_STREET_ID, EIS_DIRECTOR_EMAIL, EIS_BUH_EMAIL, EIS_PHYS_ADDR_STREET_ID, EIS_PHYS_ADDR_DISTRICT_ID, EIS_PHYS_ADDR_NOMER, EIS_PHYS_ADDR_ZIP_CODE, EIS_PHYS_ADDR_MISC, EIS_MODIFIED_BY,
//                EIS_BUDGET_NARAH_50_UAH, EIS_BUDGET_ZVIT_50_UAH, EIS_BUDGET_PREV_50_UAH, EIS_BUDGET_DEBT_30_50_UAH, EIS_IS_SPECIAL_ORG, EIS_PAYMENT_BUDGET_SPECIAL,
//                EIS_KONKURS_PAYMENTS, EIS_UNKNOWN_PAYMENTS, EIS_UNKNOWN_PAYMENT_NOTE,
//                C_KOD, PREV_KOD, NEXT_KOD, CRE_DT, CRE_ISP)
//            SELECT
//                KOD_OBJ, KOD_STAN, LAST_SOST, KOD_PRIZ_PR_KOM, KOD_VID_DIAL, KOD_STATUS, KOD_EK_PLAN_ZONA, KOD_FORM_GOSP,
//                KOD_VID_PREDPR, KOD_FORM_VLASN, KOD_VLASN_VIB, KOD_PIDVID_DIAL, KOD_SFERA_UPR, KOD_GOSP_STRUKT, KOD_ORGAN,
//                KOD_GALUZ, KOD_POTOCH_STAN, KOD_PRIZ_NADH, KOD_PRIZ_VIB, NOMER_OBJ, KOD_ZKPO, KOD_RAYON, NAME_UL, NOMER_DOMA,
//                NOMER_KORPUS, POST_INDEX, FIO_BOSS, FIO_BUH, DATE_NADHOD, DATE_VIBUT, KOLVO_BUD, USER_KOREG, DATE_KOREG,
//                TEL_BOSS, TEL_BUH, FULL_NAME_OBJ, SHORT_NAME_OBJ, KOD_DOCUM, KOD_OLD_GOSP_STRUKT, KOD_OLD_ORGAN,
//                KOD_ADDITION_PRIZNAK, KOD_OLD_PIDVID_DIAL, KOD_OLD_GALUZ, KOD_OLD_VID_DIAL, KOD_PRIKMET, KOD_PRIZNAK_1,
//                KOD_VIDOM_NAL, KOD_IMEN, KOD_ORG_FORM, KOD_VID_GOSP_STR, FIND_NAME_OBJ, NAME_DAVAL_VIDM, ADDITIONAL_ADRESS,
//                TELEFAX, REESORG, REESNO, REESDT, LYEAR, PL_BALANS, PL_VIROB_PRIZN, PL_NOT_VIROB_PRIZN, PL_ARENDA_VILNA,
//                PL_ZAGAL, PL_ARENDUETSYA, PL_PRIVAT, PL_NADANA_V_A, KOLVO_OBJ, NOMER_DOMA2, KOD_KVED, PERCENT_ACTIONS,
//                DT_STAT_DOVIDKA, KOD_RAYON2, DT_BEG_STAN, DT_END_STAN, KOD_AKCIA, DTLYEAR, NO_SVIDOT, KOD_REESORG, KOATUU,
//                DT, ISP, KOD_VID_AKCIA, PERSENT_AKCIA, NAME_BANK, MFO, OTDEL, ACCOUNT, DELETED, DTDELETE, TYPEORG, VIDDIL,
//                SYSTEM_DT, SYSTEM_ISP, FIO_BOSS_R_VIDM, POST_BOSS, POST_BOSS_R_VIDM, DOC_BOSS, DOC_BOSS_R_VIDM, PL_PRODAJ,
//                PL_ZNYATA_Z_BALANSU, PL_SPISANI_ZNESENI, PL_PEREDANA_INSHUM, LIKVID, LIKVID_DATE, PIDP_RDA,
//                EIS_ADDR_STREET_ID, EIS_DIRECTOR_EMAIL, EIS_BUH_EMAIL, EIS_PHYS_ADDR_STREET_ID, EIS_PHYS_ADDR_DISTRICT_ID, EIS_PHYS_ADDR_NOMER, EIS_PHYS_ADDR_ZIP_CODE, EIS_PHYS_ADDR_MISC, EIS_MODIFIED_BY,
//                EIS_BUDGET_NARAH_50_UAH, EIS_BUDGET_ZVIT_50_UAH, EIS_BUDGET_PREV_50_UAH, EIS_BUDGET_DEBT_30_50_UAH, EIS_IS_SPECIAL_ORG, EIS_PAYMENT_BUDGET_SPECIAL,
//                EIS_KONKURS_PAYMENTS, EIS_UNKNOWN_PAYMENTS, EIS_UNKNOWN_PAYMENT_NOTE,
//                @archid, @previd, @nextid, @curdate, @isp
//            FROM
//                SORG_1NF
//            WHERE
//                KOD_OBJ = @orgid";

//            // Firebird 1.0 does fails on INSERT - SELECT query with poarameters. So we have to emulate them
//            insertStatement = insertStatement.Replace("@archid", newArchEntryId.ToString());
//            insertStatement = insertStatement.Replace("@orgid", organizationId1NF.ToString());
//            insertStatement = insertStatement.Replace("@previd", (existingArchRecordId > 0) ? existingArchRecordId.ToString() : "NULL");
//            insertStatement = insertStatement.Replace("@nextid", (archRecordNext == null) ? "NULL" : archRecordNext.ToString());

//            using (FbCommand command = new FbCommand(insertStatement, connection1NF))
//            {
//                command.Transaction = transaction;
//                command.Parameters.Add("curdate", DateTime.Now.Date);
//                command.Parameters.Add("isp", "Auto-import");
//                command.ExecuteNonQuery();
//            }

//            // Update the previous archive record
//            if (existingArchRecordId > 0)
//            {
//                using (FbCommand cmd = new FbCommand("UPDATE TAR_SORG_1NF SET NEXT_KOD = @nextid WHERE C_KOD = " +
//                    existingArchRecordId.ToString(), connection1NF))
//                {
//                    cmd.Transaction = transaction;
//                    cmd.Parameters.Add(new FbParameter("nextid", newArchEntryId));
//                    cmd.ExecuteNonQuery();
//                }
//            }

//            // Save the last archived state for this organization
//            orgArchCodes[organizationId1NF] = newArchEntryId;

//            return newArchEntryId;
//        }

        /// <summary>
        /// Creates a new archive record for Arenda table entry
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        /// <param name="arendaId1NF">ID of the rent agreement in 1NF</param>
        /// <param name="transaction">Transaction (may be 'null')</param>
        /// <returns>ID of the generated archive entry</returns>
//        public int CreateArendaArchiveRecord(FbConnection connection1NF, int arendaId1NF, FbTransaction transaction)
//        {
//            int existingArchRecordId = -1;
//            object archRecordPrev = null;
//            object archRecordNext = null;

//            if (arendaArchCodes.TryGetValue(arendaId1NF, out existingArchRecordId))
//            {
//                // Cool ;)
//            }
//            else
//            {
//                // The cache may be empty; try to get the value from the table
//                using (FbCommand command = new FbCommand("SELECT ID, ARCH_KOD FROM ARENDA1NF WHERE ID = " + arendaId1NF.ToString(), connection1NF))
//                {
//                    command.Transaction = transaction;

//                    using (FbDataReader reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            if (!reader.IsDBNull(1))
//                                existingArchRecordId = reader.GetInt32(1);
//                        }

//                        reader.Close();
//                    }
//                }
//            }

//            // Get the properties of previous archive record
//            if (existingArchRecordId > 0)
//            {
//                using (FbCommand c = new FbCommand("SELECT PREV_KOD, NEXT_KOD FROM TAR_ARENDA1NF WHERE C_KOD = " +
//                    existingArchRecordId.ToString(), connection1NF))
//                {
//                    c.Transaction = transaction;

//                    using (FbDataReader r = c.ExecuteReader())
//                    {
//                        if (r.Read())
//                        {
//                            archRecordPrev = r.IsDBNull(0) ? null : r.GetValue(0);
//                            archRecordNext = r.IsDBNull(1) ? null : r.GetValue(1);
//                        }

//                        r.Close();
//                    }
//                }
//            }

//            // Generate a new ID for archive entry
//            int newArchEntryId = GenerateNewId1NF(connection1NF, "GEN_TAR_ARENDA1NF_ID", transaction);

//            // Format the SQL statement
//            string insertStatement = @"INSERT INTO TAR_ARENDA1NF
//                (ID, OBJECT_KOD, OBJECT_KODSTAN, BALANS_KOD, BALANS_KODSTAN, ORG_KOD, ORG_KODSTAN, ORGIV, ORGIV_STAN,
//                LYEAR, OBJKIND, GRPURPOSE, PURPOSE, NAME, DESCRIPTION, SQUARE, PRIVAT, UPDT_SOURCE, DOGKIND, DOGDATE,
//                DOGNUM, DOGSTR, POVERH, PPL_CNT, PLATA_NARAH, PLATA_SPLACH, PLATA_BORG, PLATA_DOGOVOR, AR_STAVKA,
//                PIDSTAVA_STR, PIDSTAVA_DATE, PIDSTAVA_NUM, DT, START_ORENDA, FINISH_ORENDA, FINISH_ORENDA_FACT,
//                PRIZNAK_1NF, ISP, SROK_BORG, ORDER_NO, ORDER_DATE, PURP_STR, EDT, RISHEN, NOACTIVE, NOACTIVE_DT,
//                OLD_ISP, OLD_DT, AR_STAVKA_UAH, VARTIST_EXP_V, DTEXP, SUBAR, KILK_PRIM, PIDSTAVA_DATE2, PIDSTAVA_NUM2,
//                PIDSTAVA_STR2, ORDER_NO2, KOD_VIDPLATA, DTDELETE, DELETED, VARTIST_EXP, PIDSTAVA_FACT,
//                EIS_AGREEMENT_STATE, EIS_MODIFIED_BY,
//                C_KOD, PREV_KOD, NEXT_KOD, CRE_DT, CRE_ISP)
//            SELECT
//                ID, OBJECT_KOD, OBJECT_KODSTAN, BALANS_KOD, BALANS_KODSTAN, ORG_KOD, ORG_KODSTAN, ORGIV, ORGIV_STAN,
//                LYEAR, OBJKIND, GRPURPOSE, PURPOSE, NAME, DESCRIPTION, SQUARE, PRIVAT, UPDT_SOURCE, DOGKIND, DOGDATE,
//                DOGNUM, DOGSTR, POVERH, PPL_CNT, PLATA_NARAH, PLATA_SPLACH, PLATA_BORG, PLATA_DOGOVOR, AR_STAVKA,
//                PIDSTAVA_STR, PIDSTAVA_DATE, PIDSTAVA_NUM, DT, START_ORENDA, FINISH_ORENDA, FINISH_ORENDA_FACT,
//                PRIZNAK_1NF, ISP, SROK_BORG, ORDER_NO, ORDER_DATE, PURP_STR, EDT, RISHEN, NOACTIVE, NOACTIVE_DT,
//                OLD_ISP, OLD_DT, AR_STAVKA_UAH, VARTIST_EXP_V, DTEXP, SUBAR, KILK_PRIM, PIDSTAVA_DATE2, PIDSTAVA_NUM2,
//                PIDSTAVA_STR2, ORDER_NO2, KOD_VIDPLATA, DTDELETE, DELETED, VARTIST_EXP, PIDSTAVA_FACT,
//                EIS_AGREEMENT_STATE, EIS_MODIFIED_BY,
//                @archid, @previd, @nextid, @curdate, @isp
//            FROM
//                ARENDA1NF
//            WHERE
//                ID = @arendaid";

//            // Firebird 1.0 fails on INSERT - SELECT query with parameters. So we have to emulate them
//            insertStatement = insertStatement.Replace("@archid", newArchEntryId.ToString());
//            insertStatement = insertStatement.Replace("@arendaid", arendaId1NF.ToString());
//            insertStatement = insertStatement.Replace("@previd", (existingArchRecordId > 0) ? existingArchRecordId.ToString() : "NULL");
//            insertStatement = insertStatement.Replace("@nextid", (archRecordNext == null) ? "NULL" : archRecordNext.ToString());

//            using (FbCommand command = new FbCommand(insertStatement, connection1NF))
//            {
//                command.Transaction = transaction;
//                command.Parameters.Add("curdate", DateTime.Now.Date);
//                command.Parameters.Add("isp", "Auto-import");
//                command.ExecuteNonQuery();
//            }

//            // Copy the ARRISH entries from the existing ARENDA1NF entry to the archive
//            string query = @"insert into tar_arrish (
//                ID, TAR_ARENDA, RISHEN, ORD, SYSISP, SYSDT, ISP, DT, NOM, DT_DOC, DODATOK,
//                PUNKT, PURP_STR, SQUARE, KIND_PIDSTAVA, OZR, RASPOR, PIDSTAVA)
//            select
//                GEN_ID(gen_tar_arrish_id, 1), @arenda_tar, RISHEN, ORD, SYSISP, SYSDT, ISP, DT, NOM, DT_DOC,
//                DODATOK, PUNKT, PURP_STR, SQUARE, KIND_PIDSTAVA, OZR, RASPOR, PIDSTAVA
//            from
//                arrish
//            where
//                arenda = @arenda_id";

//            query = query.Replace("@arenda_tar", newArchEntryId.ToString());
//            query = query.Replace("@arenda_id", arendaId1NF.ToString());

//            using (FbCommand command = new FbCommand(query, connection1NF))
//            {
//                command.Transaction = transaction;
//                command.ExecuteNonQuery();
//            }

//            // Copy the ARENDA_PRIM entries from the existing ARENDA1NF entry to the archive
//            query = @"insert into tar_arenda_prim (
//                ARCH_ID, ARENDA_ARCH_ID,
//                ID, ARENDA, GRPURP, PURP, PURP_STR, SQUARE, DT, ISP, SYS_DT, SYS_ISP, DESCRIPT,
//                AR_STAVKA, NARAH, PRICE, DELETED, DTDELETE, VARTIST_EXP, DTEXP, KOD_VIDPLATA,
//                EIS_INVENT_NO, EIS_PRIM_STATUS)
//            select
//                GEN_ID(gen_tar_arenda_prim, 1), @arenda_tar,
//                ID, ARENDA, GRPURP, PURP, PURP_STR, SQUARE, DT, ISP, SYS_DT, SYS_ISP, DESCRIPT,
//                AR_STAVKA, NARAH, PRICE, DELETED, DTDELETE, VARTIST_EXP, DTEXP, KOD_VIDPLATA,
//                EIS_INVENT_NO, EIS_PRIM_STATUS
//            from
//                arenda_prim
//            where
//                arenda = @arenda_id";

//            query = query.Replace("@arenda_tar", newArchEntryId.ToString());
//            query = query.Replace("@arenda_id", arendaId1NF.ToString());

//            using (FbCommand command = new FbCommand(query, connection1NF))
//            {
//                command.Transaction = transaction;
//                command.ExecuteNonQuery();
//            }

//            // Update the previous archive record
//            if (existingArchRecordId > 0)
//            {
//                using (FbCommand cmd = new FbCommand("UPDATE TAR_ARENDA1NF SET NEXT_KOD = @nextid WHERE C_KOD = " +
//                    existingArchRecordId.ToString(), connection1NF))
//                {
//                    cmd.Transaction = transaction;
//                    cmd.Parameters.Add(new FbParameter("nextid", newArchEntryId));
//                    cmd.ExecuteNonQuery();
//                }
//            }

//            // Save the last archived state for this rent agreement
//            arendaArchCodes[arendaId1NF] = newArchEntryId;

//            return newArchEntryId;
//        }

        /// <summary>
        /// Creates a new archive record for Balans table entry
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        /// <param name="balansId1NF">ID of the balans entry in 1NF</param>
        /// <param name="transaction">Transaction (may be 'null')</param>
        /// <returns>ID of the generated archive entry</returns>
//        public int CreateBalansArchiveRecord(FbConnection connection1NF, int balansId1NF, FbTransaction transaction)
//        {
//            int existingArchRecordId = -1;
//            object archRecordPrev = null;
//            object archRecordNext = null;

//            if (balansArchCodes.TryGetValue(balansId1NF, out existingArchRecordId))
//            {
//                // Cool ;)
//            }
//            else
//            {
//                // The cache may be empty; try to get the value from the table
//                using (FbCommand command = new FbCommand("SELECT ID, ARCH_KOD FROM BALANS_1NF WHERE ID = " + balansId1NF.ToString(), connection1NF))
//                {
//                    command.Transaction = transaction;

//                    using (FbDataReader reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            if (!reader.IsDBNull(1))
//                                existingArchRecordId = reader.GetInt32(1);
//                        }

//                        reader.Close();
//                    }
//                }
//            }

//            // Get the properties of previous archive record
//            if (existingArchRecordId > 0)
//            {
//                using (FbCommand c = new FbCommand("SELECT PREV_KOD, NEXT_KOD FROM TAR_BALANS_1NF WHERE C_KOD = " +
//                    existingArchRecordId.ToString(), connection1NF))
//                {
//                    c.Transaction = transaction;

//                    using (FbDataReader r = c.ExecuteReader())
//                    {
//                        if (r.Read())
//                        {
//                            archRecordPrev = r.IsDBNull(0) ? null : r.GetValue(0);
//                            archRecordNext = r.IsDBNull(1) ? null : r.GetValue(1);
//                        }

//                        r.Close();
//                    }
//                }
//            }

//            // Generate a new ID for archive entry
//            int newArchEntryId = GenerateNewId1NF(connection1NF, "GEN_TAR_BALANS_1NF_ID", transaction);

//            // Format the SQL statement
//            string insertStatement = @"INSERT INTO TAR_BALANS_1NF
//                (ID, STAN, LYEAR, OBJECT, OBJECT_STAN, ORG, ORG_STAN, SQR_ZAG, SQR_PIDV, VARTIST_EXP, PPL_NUM,
//                SQR_VLAS, SQR_FREE, V_SQR_ORENDA, V_DOG_ORENDA, SQR_PRIV, PRIV_COUNT, OREND_NARAH, OREND_SPLACH,
//                BORG, O26, OBTI, RISHEN, RNOMER, RDATA, MEMO, FORM_VLASN, KINDOBJ, TYPEOBJ, GRPURP, PURPOSE, PURP_STR,
//                TEXSTAN, HISTORY, FLOATS, DOKID, DT, LOCAL_DOG, LOCAL_DOGNUM, LOCAL_DOGDATE, LOCAL_DOGKIND, PRIZNAK1NF,
//                PRAVO, ISP, BALANS_VARTIST, OBJ_ULNAME, OBJ_NOMER1, OBJ_NOMER2, OBJ_NOMER3, OBJ_KODBTI, YEAR1, OBJ_KODUL,
//                PRYMITKA, EDT, FORM_GIV, ORG_EXPL, ORG_EXPL_STAN, UPD_SOURCE, SPRAV_VARTIST, OLD_ISP, OLD_DT, SQR_VLASN_N,
//                OTDEL, ORG_VLAS, ORG_VLAS_STAN, ZAL_VART, ZNOS, VARTIST_EXP_V, DTEXP, REESNO, SPR_VART_1KVM, DTSPRAV,
//                VIDDIL, DATE_BTI, OBJ_ADRDOP, ZNOS_DATE, VARTIST_RINK, DELETED, DTDELETE,
//                SQR_GURTOJ, RINK_VART_DT, SQR_NEJ, ULNAME2, ULKOD2 , OBJ_ULADRDOP, SQR_KOR,
//                EIS_FREE_SQR_EXISTS, EIS_FREE_SQR_USEFUL, EIS_FREE_SQR_CONDITION, EIS_FREE_SQR_LOCATION,
//                EIS_OWNERSHIP_DOC_TYPE, EIS_OWNERSHIP_DOC_NUM, EIS_OWNERSHIP_DOC_DATE, EIS_BALANS_DOC_TYPE,
//                EIS_BALANS_DOC_NUM, EIS_BALANS_DOC_DATE, EIS_VIDCH_DOC_TYPE, EIS_VIDCH_DOC_NUM,
//                EIS_VIDCH_DOC_DATE, EIS_VIDCH_TYPE_ID, EIS_VIDCH_ORG_ID, EIS_VIDCH_COST,
//                EIS_SQR_ENGINEERING, EIS_OBJ_STATUS_ID, EIS_MODIFIED_BY,
//                C_KOD, PREV_KOD, NEXT_KOD, CRE_DT, CRE_ISP)
//            SELECT
//                ID, STAN, LYEAR, OBJECT, OBJECT_STAN, ORG, ORG_STAN, SQR_ZAG, SQR_PIDV, VARTIST_EXP, PPL_NUM,
//                SQR_VLAS, SQR_FREE, V_SQR_ORENDA, V_DOG_ORENDA, SQR_PRIV, PRIV_COUNT, OREND_NARAH, OREND_SPLACH,
//                BORG, O26, OBTI, RISHEN, RNOMER, RDATA, MEMO, FORM_VLASN, KINDOBJ, TYPEOBJ, GRPURP, PURPOSE, PURP_STR,
//                TEXSTAN, HISTORY, FLOATS, DOKID, DT, LOCAL_DOG, LOCAL_DOGNUM, LOCAL_DOGDATE, LOCAL_DOGKIND, PRIZNAK1NF,
//                PRAVO, ISP, BALANS_VARTIST, OBJ_ULNAME, OBJ_NOMER1, OBJ_NOMER2, OBJ_NOMER3, OBJ_KODBTI, YEAR1, OBJ_KODUL,
//                PRYMITKA, EDT, FORM_GIV, ORG_EXPL, ORG_EXPL_STAN, UPD_SOURCE, SPRAV_VARTIST, OLD_ISP, OLD_DT, SQR_VLASN_N,
//                OTDEL, ORG_VLAS, ORG_VLAS_STAN, ZAL_VART, ZNOS, VARTIST_EXP_V, DTEXP, REESNO, SPR_VART_1KVM, DTSPRAV,
//                VIDDIL, DATE_BTI, OBJ_ADRDOP, ZNOS_DATE, VARTIST_RINK, DELETED, DTDELETE,
//                SQR_GURTOJ, RINK_VART_DT, SQR_NEJ, ULNAME2, ULKOD2 , OBJ_ULADRDOP, SQR_KOR,
//                EIS_FREE_SQR_EXISTS, EIS_FREE_SQR_USEFUL, EIS_FREE_SQR_CONDITION, EIS_FREE_SQR_LOCATION,
//                EIS_OWNERSHIP_DOC_TYPE, EIS_OWNERSHIP_DOC_NUM, EIS_OWNERSHIP_DOC_DATE, EIS_BALANS_DOC_TYPE,
//                EIS_BALANS_DOC_NUM, EIS_BALANS_DOC_DATE, EIS_VIDCH_DOC_TYPE, EIS_VIDCH_DOC_NUM,
//                EIS_VIDCH_DOC_DATE, EIS_VIDCH_TYPE_ID, EIS_VIDCH_ORG_ID, EIS_VIDCH_COST,
//                EIS_SQR_ENGINEERING, EIS_OBJ_STATUS_ID, EIS_MODIFIED_BY,
//                @archid, @previd, @nextid, @curdate, @isp
//            FROM
//                BALANS_1NF
//            WHERE
//                ID = @balansid";

//            // Firebird 1.0 does fails on INSERT - SELECT query with poarameters. So we have to emulate them
//            insertStatement = insertStatement.Replace("@archid", newArchEntryId.ToString());
//            insertStatement = insertStatement.Replace("@balansid", balansId1NF.ToString());
//            insertStatement = insertStatement.Replace("@previd", (existingArchRecordId > 0) ? existingArchRecordId.ToString() : "NULL");
//            insertStatement = insertStatement.Replace("@nextid", (archRecordNext == null) ? "NULL" : archRecordNext.ToString());

//            using (FbCommand command = new FbCommand(insertStatement, connection1NF))
//            {
//                command.Transaction = transaction;
//                command.Parameters.Add("curdate", DateTime.Now.Date);
//                command.Parameters.Add("isp", "Auto-import");
//                command.ExecuteNonQuery();
//            }

//            // Copy the BALANS_DOK_PROP entries from the existing BALANS_1NF entry to the archive
//            string query = @"insert into TAR_BALANS_DOK_PROP (
//                AR_ID, BAL_AR_ID,
//                ID, BALID, OBJECT_KOD,FLAG, DOK_ID)
//            select
//                GEN_ID(GEN_TAR_BALANS_DOK_PROP_ID, 1), @balans_tar,
//                ID, BALID, OBJECT_KOD,FLAG, DOK_ID
//            from
//                BALANS_DOK_PROP
//            where
//                BALID = @balans_id";

//            query = query.Replace("@balans_tar", newArchEntryId.ToString());
//            query = query.Replace("@balans_id", balansId1NF.ToString());

//            using (FbCommand command = new FbCommand(query, connection1NF))
//            {
//                command.Transaction = transaction;
//                command.ExecuteNonQuery();
//            }

//            // Update the previous archive record
//            if (existingArchRecordId > 0)
//            {
//                using (FbCommand cmd = new FbCommand("UPDATE TAR_BALANS_1NF SET NEXT_KOD = @nextid WHERE C_KOD = " +
//                    existingArchRecordId.ToString(), connection1NF))
//                {
//                    cmd.Transaction = transaction;
//                    cmd.Parameters.Add(new FbParameter("nextid", newArchEntryId));
//                    cmd.ExecuteNonQuery();
//                }
//            }

//            // Save the last archived state for this Balans object
//            balansArchCodes[balansId1NF] = newArchEntryId;

//            return newArchEntryId;
//        }

        #endregion (Interface)

        #region Implementation

        /// <summary>
        /// Generates a new ID using a Firebird GENERATOR
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        /// <param name="generatorName">Generator name</param>
        /// <param name="transaction">Transaction</param>
        /// <returns>The generated ID</returns>
        //private int GenerateNewId1NF(FbConnection connection1NF, string generatorName, FbTransaction transaction)
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

        /// <summary>
        /// Populates the cache of existing archive IDs
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        /// <param name="table">Name of the table to get archive IDs from</param>
        /// <param name="idField">Primary key field in the specified table</param>
        /// <param name="archCodeField">Archive ID field in the specified table</param>
        /// <param name="cache">The cache that must be populated</param>
        //private void PrepareArchiveList(FbConnection connection1NF, string table,
        //    string idField, string archCodeField, Dictionary<int, int> cache)
        //{
        //    cache.Clear();

        //    string query = "SELECT " + idField + ", " + archCodeField + " FROM " + table + " WHERE NOT " + archCodeField + " IS NULL";

        //    using (FbCommand command = new FbCommand(query, connection1NF))
        //    {
        //        using (FbDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                object dataObjectId = reader.IsDBNull(0) ? null : reader.GetValue(0);
        //                object dataArchKod = reader.IsDBNull(1) ? null : reader.GetValue(1);

        //                if (dataObjectId is int && dataArchKod is int && (int)dataArchKod > 0)
        //                {
        //                    cache.Add((int)dataObjectId, (int)dataArchKod);
        //                }
        //            }

        //            reader.Close();
        //        }
        //    }
        //}

        #endregion (Implementation)
    }
}
