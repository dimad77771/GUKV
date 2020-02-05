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
//        #region Rent Payments database migration

//        private void MigrateRentPayments()
//        {
//            logger.WriteInfo("=== Migrating database 'Orendna Plata' ===");

//            MigrateRentPaymentDictionaries();
//            MigrateRentPaymentData();

//            // Perform matching of 'tech state' values for the 'rent_object' table
//            ExecuteStatementInSQLServer("UPDATE rent_object SET tech_state_id = 5 WHERE tech_state_id = 2");
//            ExecuteStatementInSQLServer("UPDATE rent_object SET tech_state_id = 2 WHERE tech_state_id = 1");

//            MatchRentPaymentRentersTo1NF();
//            MatchRentPaymentObjectsTo1NF();
//            UpdateObjectFreeSquareFromRentPayments();
//        }

//        private void GetRentPeriodsInfo(out int lastID, out DateTime lastQuarterBegin)
//        {
//            string statement = "SELECT MAX(id) id, MAX(period_start) period_start FROM dict_rent_period";

//            try
//            {
//                using (SqlCommand command = connectionSqlClient.CreateCommand())
//                {
//                    command.CommandType = CommandType.Text;
//                    command.CommandTimeout = 600;
//                    command.CommandText = statement;

//                    using (IDataReader reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            lastID = (int)reader["id"];
//                            lastQuarterBegin = (DateTime)reader["period_start"];
//                        }
//                        else
//                        {
//                            lastID = 0;
//                            lastQuarterBegin = default(DateTime);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowSqlErrorMessageDlg(statement, ex.Message);

//                // Abort migration correctly
//                throw new MigrationAbortedException();
//            }
//        }

//        private void CreateRentPeriodEntry(int id, DateTime quarterBegin)
//        {
//            ExecuteStatementInSQLServer(GetInsertStatement("dict_rent_period", new Dictionary<string, object>
//                    {
//                        {"id", id},
//                        {"name", string.Format("{0} квартал {1}", (quarterBegin.Month + 2) / 3, quarterBegin.Year)},
//                        {"period_start", quarterBegin},
//                        {"period_end", quarterBegin.AddMonths(3).AddDays(-1)},
//                        {"period_quarter", (quarterBegin.Month + 2) / 3},
//                        {"period_year", quarterBegin.Year},
//                        {"is_active", 0},
//                    }));
//        }

//        private void UpdateRentPeriodsDictionary()
//        {
//            // What's the current quarter?
//            DateTime currentQuarterBegin =
//                new DateTime(DateTime.Now.Year, DateTime.Now.Month - ((DateTime.Now.Month - 1) % 3), 1);

//            int lastID;
//            DateTime lastQuarterBegin;

//            GetRentPeriodsInfo(out lastID, out lastQuarterBegin);

//            if (lastID == 0)
//            {
//                // There's no data in this table. We might want to migrate whatever data is in the
//                // externa data source before creating the "current" quarter.

//                Dictionary<string, string> fields = new Dictionary<string, string>();

//                fields.Add("S_PERIOD_OR_PLATA_ID", "id");
//                fields.Add("NAME", "name");
//                fields.Add("DT_START", "period_start");
//                fields.Add("DT_END", "period_end");
//                fields.Add("KVART", "period_quarter");
//                fields.Add("SYEAR", "period_year");
//                fields.Add("PRIZNAK", "is_active");

//                MigrateTable(connection1NF, "ZAV_S_PERIOD_OR_PLATA", "dict_rent_period", fields, "S_PERIOD_OR_PLATA_ID", null);

//                // Re-load the last [id] and [period_end] data
//                GetRentPeriodsInfo(out lastID, out lastQuarterBegin);
//            }

//            if (lastQuarterBegin == default(DateTime))
//            {
//                // The table is empty, we shall create a single record corresponding
//                // to the current quarter.
//                CreateRentPeriodEntry(1, currentQuarterBegin);
//            }
//            else
//            {
//                // The table is not empty, therefore we shall create as many quarters
//                // as necessary to fill the gap between the last one and the current one.
//                while (lastQuarterBegin < currentQuarterBegin)
//                {
//                    lastQuarterBegin = lastQuarterBegin.AddMonths(3);

//                    CreateRentPeriodEntry(++lastID, lastQuarterBegin);
//                }
//            }

//            // Set the quarter for which reporting is currently expected.
//            ExecuteStatementInSQLServer(string.Format("UPDATE dict_rent_period SET is_active = CASE WHEN CAST('{0}' AS DATE) BETWEEN DATEADD(d, 20, period_start) AND DATEADD(d, 20, period_end) THEN 1 ELSE 0 END", DateTime.Now.ToString("s")));
//        }

//        private void MigrateRentPaymentDictionaries()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // ZAV_S_PERIOD_OR_PLATA

//            // NOTE
//            // ----
//            // [dict_rent_period] table in EIS is now authoritative

//            UpdateRentPeriodsDictionary();

//            // ZAV_S_PRIMITKA
//            fields.Clear();
//            fields.Add("ID", "id");
//            fields.Add("NAZVA", "name");

//            MigrateTable(connection1NF, "ZAV_S_PRIMITKA", "dict_rent_note", fields, "ID", null);

//            // ZAV_DKK
//            fields.Clear();
//            fields.Add("ID", "id");
//            fields.Add("FULL_NAME", "name");
//            fields.Add("SHORT_NAME", "short_name");

//            MigrateTable(connection1NF, "ZAV_DKK", "dict_rent_dkk", fields, "ID", null);

//            // ZAV_S_VID_DIAL
//            fields.Clear();
//            fields.Add("VID_DIAL_ID", "id");
//            fields.Add("VID_DIAL_NAME", "name");

//            MigrateTable(connection1NF, "ZAV_S_VID_DIAL", "dict_rent_occupation", fields, "VID_DIAL_ID", null);

//            // ZAV_PLATA
//            fields.Clear();
//            fields.Add("ID", "id");
//            fields.Add("PLATA", "name");

//            MigrateTable(connection1NF, "ZAV_PLATA", "dict_rent_payment_type", fields, "ID", null);

//            // ZAV_S_RAYON
//            fields.Clear();
//            fields.Add("KOD_RAYON2", "id");
//            fields.Add("NAME_RAYON2", "name");
//            fields.Add("ISP", "modified_by");
//            fields.Add("DT", "modify_date");

//            OverrideDictionary(connection1NF, "ZAV_S_RAYON", "dict_districts2", fields,
//                "KOD_RAYON2", "id", "KOD_RAYON2", null, true);
//        }

//        private void MigrateRentPaymentData()
//        {
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            // ZAV_BALANS_ORG
//            fields.Clear();
//            fields.Add("KOD_OBJ", "organization_id");
//            fields.Add("VID_DIAL_ID", "rent_occupation_id");
//            fields.Add("ID_DKK", "rent_dkk_id");
//            fields.Add("P_ENERGO_KANAL", "p_energo_kanal");
//            fields.Add("P_ZVIT", "p_zvit");

//            MigrateTable(connection1NF, "ZAV_BALANS_ORG", "rent_balans_org", fields, "KOD_OBJ", null);

//            // ZAV_ORENDARI_OR_PLATA
//            fields.Clear();
//            fields.Add("ORENDARI_OR_PLATA_ID", "id");
//            fields.Add("NAME", "name");
//            fields.Add("EDRPOU", "zkpo_code");

//            MigrateTable(connection1NF, "ZAV_ORENDARI_OR_PLATA", "rent_renter_org", fields, "ORENDARI_OR_PLATA_ID", null);

//            // ZAVANTAZH_OR_PLATA
//            fields.Clear();
//            fields.Add("ZAVANTAZH_ID", "id");
//            fields.Add("KOD_BALANS_OR_PLATA", "org_balans_id");
//            fields.Add("S_PERIOD_OR_PLATA_ID", "rent_period_id");
//            fields.Add("SQUARE_BALANS", "sqr_balans");
//            fields.Add("PLATA_ZVIT_UAH", "payment_zvit_uah");

//            fields.Add("OTRYM_ZVIT_UAH", "received_zvit_uah");
//            fields.Add("OTRYM_NAR_ZVIT_UAH", "received_nar_zvit_uah");
//            fields.Add("BORG_ZAG_UAH", "debt_total_uah");
//            fields.Add("BORG_ZVIT_UAH", "debt_zvit_uah");
//            fields.Add("BUD_NAR_50_UAH", "budget_narah_50_uah");

//            fields.Add("BUD_ZVIT_50_UAH", "budget_zvit_50_uah");
//            fields.Add("BUD_POPER_50_UAH", "budget_prev_50_uah");
//            fields.Add("BUD_BORG_50_UAH", "budget_debt_50_uah");
//            fields.Add("BUD_BORG_30_50_UAH", "budget_debt_30_50_uah");
//            fields.Add("SQUARE_CMK", "sqr_cmk");

//            fields.Add("BUD_NAR_50_CMK_UAH", "budget_narah_50_cmk_uah");
//            fields.Add("BUD_ZVIT_50_CMK_UAH", "budget_zvit_50_cmk_uah");
//            fields.Add("BUD_BORG_50_CMK_UAH", "budget_debt_50_cmk_uah");
//            fields.Add("BUD_ZVIT_50_IN_UAH", "budget_zvit_50_in_uah");
//            fields.Add("KOD_ZKPO", "zkpo_code");

//            fields.Add("DT", "modify_date");
//            fields.Add("ISP", "modified_by");
//            fields.Add("MAIL", "email");
//            fields.Add("TELL", "phone");
//            fields.Add("VIDPOV", "responsible_fio");

//            fields.Add("DIRECTOR", "director_fio");
//            fields.Add("BUH", "buhgalter_fio");
//            fields.Add("KOL_DOG", "num_agreements");
//            fields.Add("KOL_ORENDAR", "num_renters");
//            fields.Add("BORG_VITRAT_NA_UTRIMAN", "debt_v_mezhah_vitrat");

//            fields.Add("BUD_ZVIT_50_UAH_OLD", "budget_zvit_50_old");
//            fields.Add("POST_ADDRESS", "post_address");
//            fields.Add("PERERAH_50_B", "pererahunok_50_b");

//            MigrateTable(connection1NF, "ZAVANTAZH_OR_PLATA", "rent_payment", fields, "ZAVANTAZH_ID", null);

//            // ZAV_DANI_OBJ_OR_PLATA
//            fields.Clear();

//            // fields.Add("ZAV_DANI_OBJ_OR_PLATA_ID", "id"); - Replaced by an IDENTITY field
//            fields.Add("ZAVANTAZH_ID", "rent_payment_id");
//            fields.Add("S_PERIOD_OR_PLATA_ID", "rent_period_id");
//            fields.Add("OBJ_VUL", "addr_street");

//            fields.Add("OBJ_TYPE", "addr_street_type");
//            fields.Add("OBJ_NUM", "addr_num");
//            fields.Add("ZAG_PLOSH", "sqr_total");
//            fields.Add("ZAG_PLOSH_OREND", "sqr_rented");
//            fields.Add("ZAG_PLOSH_VIL", "sqr_free");

//            fields.Add("PLOSH_VIL_KOR", "sqr_korysna");
//            fields.Add("PLOSH_VIL_MZK", "sqr_mzk");
//            fields.Add("POVERH", "floors");
//            fields.Add("STAN", "tech_state_id");
//            fields.Add("ENERGO", "is_energo");

//            fields.Add("VODO", "is_vodo");
//            fields.Add("TEPLO", "is_teplo");
//            fields.Add("KOD_RAYON", "district_id");
//            fields.Add("VYKORISTANNJA", "purpose");
//            fields.Add("PRYMITKA", "rent_note_id");

//            MigrateTable(connection1NF, "ZAV_DANI_OBJ_OR_PLATA", "rent_object", fields, "ZAV_DANI_OBJ_OR_PLATA_ID", null);

//            // ZAV_DANI_OR_PLATA
//            fields.Clear();

//            // fields.Add("DANI_OR_PLATA_ID", "id"); - Replaced by an IDENTITY field
//            fields.Add("ORENDARI_OR_PLATA_ID", "rent_renter_org_id");
//            fields.Add("SQUARE_ORENDA", "sqr_total_rent");
//            fields.Add("SQUARE_PLATA_VIDS_VART", "sqr_payed_by_percent");
//            fields.Add("SQUARE_PLATA_1_UAH", "sqr_payed_by_1uah");

//            fields.Add("SQUARE_HOUR", "sqr_payed_hourly");
//            fields.Add("PAY_NAR_UAH", "payment_narah");
//            fields.Add("PAY_OTR_UAH", "payment_received");
//            fields.Add("BUD_50_UAH", "payment_budget_50_uah");
//            fields.Add("ZAVANTAZH_ID", "rent_payment_id");

//            fields.Add("KOD_OREND_SORG_1NF", "renter_organization_id");
//            fields.Add("S_PERIOD_OR_PLATA_ID", "rent_period_id");
//            fields.Add("ORNDODAV_ID", "giver_organization_id");
//            fields.Add("PRIZNAK_DOG", "agreement_flag");
//            fields.Add("PAY_NAR_ZVIT", "payment_nar_zvit");

//            MigrateTable(connection1NF, "ZAV_DANI_OR_PLATA", "rent_payment_by_renter", fields, "DANI_OR_PLATA_ID", null);

//            // ZAV_DANI_BORG_OR_PLATA
//            fields.Clear();

//            // fields.Add("DANI_BORG_OR_PLATA_ID", "id"); - Replaced by an IDENTITY field
//            fields.Add("ZAVANTAZH_ID", "rent_payment_id");
//            fields.Add("ORENDARI_OR_PLATA_ID", "rent_renter_org_id");
//            fields.Add("S_PERIOD_OR_PLATA_ID", "rent_period_id");
//            fields.Add("ORNDODAV_ID", "giver_organization_id");

//            fields.Add("BORG_ALL_UAH", "debt_total");
//            fields.Add("BORG_3_UAH", "debt_3_month");
//            fields.Add("BORG_3_36_UAH", "debt_12_month");
//            fields.Add("BORG_3_36_UAH2", "debt_3_years");
//            fields.Add("BORG_3_YEAR_UAH", "debt_over_3_years");

//            fields.Add("BORG_SPYSANO_UAH", "debt_spysano");
//            fields.Add("QUAN_ZAH", "num_zahodiv_total");
//            fields.Add("QUAN_ZAH_ZVIT", "num_zahodiv_zvit");
//            fields.Add("QUAN_POZOV", "num_pozov_total");
//            fields.Add("QUAN_POZOV_ZVIT", "num_pozov_zvit");

//            fields.Add("QUAN_POZOV_DOZV", "num_pozov_zadov_total");
//            fields.Add("QUAN_POZOV_DOZV_ZVIT", "num_pozov_zadov_zvit");
//            fields.Add("QUAN_POZOV_VIK", "num_pozov_vikon_total");
//            fields.Add("QUAN_POZOV_VIK_ZVIT", "num_pozov_vikon_zvit");
//            fields.Add("BORG_POGASH_UAH", "debt_pogasheno_total");

//            fields.Add("BORG_POGASH_ZVIT_UAH", "debt_pogasheno_zvit");
//            fields.Add("PRIZNAK_DOG", "agreement_flag");
//            fields.Add("BORG_VITRAT_NA_UTRIMAN", "debt_v_mezhah_vitrat");

//            MigrateTable(connection1NF, "ZAV_DANI_BORG_OR_PLATA", "rent_payment_debt", fields, "DANI_BORG_OR_PLATA_ID", null);

//            // ZAV_KAZNA
//            fields.Clear();

//            // fields.Add("ID", "id"); - Replaced by an IDENTITY field
//            fields.Add("KOD_INN", "zkpo_code");
//            fields.Add("DT", "payment_date");
//            fields.Add("SUMMA", "payment_sum");
//            fields.Add("ID_PLATA", "rent_payment_type_id");
//            fields.Add("KOD_BALANS", "org_balans_id");
//            fields.Add("OBJ", "organization_id");

//            MigrateTable(connection1NF, "ZAV_KAZNA", "rent_vipiski", fields, "ID", null);
//        }

//        private void MatchRentPaymentRentersTo1NF()
//        {
//            logger.WriteInfo("=== Matching 'Orendna Plata' organizations to 1NF organizations ===");

//            // Perform matching by ZKPO code
//            Dictionary<int, int> renterIDs1NF = new Dictionary<int, int>();

//            using (SqlCommand cmd = new SqlCommand("SELECT id, zkpo_code, name FROM rent_renter_org WHERE NOT (id IS NULL) AND NOT (zkpo_code IS NULL)", connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        int id = reader.GetInt32(0);
//                        string zkpo = reader.GetString(1).Trim();
//                        string orgName = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();

//                        bool forceCreate = false;
//                        int orgId1NF = organizationFinder.GetOrgIdByZKPO(zkpo, orgName, true, out forceCreate);

//                        if (orgId1NF > 0 && !forceCreate)
//                        {
//                            renterIDs1NF[id] = orgId1NF;
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            // Use existing mapping from 'rent_payment_by_renter' table
//            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT rent_renter_org_id, renter_organization_id FROM rent_payment_by_renter " +
//                "WHERE rent_renter_org_id > 0 AND renter_organization_id > 0", connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        renterIDs1NF[reader.GetInt32(0)] = reader.GetInt32(1);
//                    }

//                    reader.Close();
//                }
//            }

//            // Save the produced matching to our database
//            foreach (KeyValuePair<int, int> pair in renterIDs1NF)
//            {
//                using (SqlCommand cmd = new SqlCommand("UPDATE rent_renter_org SET organization_id = @orgid WHERE id = @rid", connectionSqlClient))
//                {
//                    cmd.Parameters.Add(new SqlParameter("rid", pair.Key));
//                    cmd.Parameters.Add(new SqlParameter("orgid", pair.Value ));

//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }

//        private void MatchRentPaymentObjectsTo1NF()
//        {
//            logger.WriteInfo("=== Matching 'Orendna Plata' objects to 1NF objects ===");

//            // We need to tolerate LITERA A addresses in this case
//            objectFinder.AllowLiteraAComparisonWithoutSquareMatch(true);

//            Dictionary<int, int> objectMatch = new Dictionary<int, int>();

//            using (SqlCommand cmd = new SqlCommand("SELECT id, addr_street, addr_street_type, addr_num FROM rent_object WHERE NOT (id IS NULL)", connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        int id = reader.GetInt32(0);
//                        string street = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim().ToUpper();
//                        string streetType = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
//                        string addrNum = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim().ToUpper();

//                        if (street.Length > 0 && addrNum.Length > 0)
//                        {
//                            // Perform necessary corrections
//                            ValidateRentPaymentAddrStreet(ref street, streetType);

//                            // Perform street name validation
//                            if (!objectFinder.IsKnownStreetName(street))
//                            {
//                                string actualStreet = "";

//                                if (objectFinder.FindSimilarStreetName(street, out actualStreet))
//                                {
//                                    street = actualStreet;
//                                }
//                                else
//                                {
//                                    street = "";
//                                }
//                            }

//                            // Perform address matching
//                            if (street.Length > 0)
//                            {
//                                bool addressIsSimple = false;
//                                bool similarAddressExists = false;

//                                int existingObject = objectFinder.FindObject(street, "", addrNum, "", "", "", null, null,
//                                    out addressIsSimple, out similarAddressExists);

//                                if (existingObject > 0)
//                                {
//                                    objectMatch[id] = existingObject;
//                                }
//                            }
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            objectFinder.AllowLiteraAComparisonWithoutSquareMatch(false);

//            // Save the produced matching to our database
//            foreach (KeyValuePair<int, int> pair in objectMatch)
//            {
//                using (SqlCommand cmd = new SqlCommand("UPDATE rent_object SET building_id = @bid WHERE id = @oid", connectionSqlClient))
//                {
//                    cmd.Parameters.Add(new SqlParameter("oid", pair.Key));
//                    cmd.Parameters.Add(new SqlParameter("bid", pair.Value));

//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }

//        protected void ValidateRentPaymentAddrStreet(ref string street, string streetType)
//        {
//            // Remove unnecessary prefixes and suffixes
//            foreach (string s in Properties.Settings.Default.RentPaymentStreetSuffix)
//            {
//                string suffix = s;
//                string actualStreetType = "";

//                int dividerPos = suffix.IndexOf('=');

//                if (dividerPos > 0)
//                {
//                    actualStreetType = suffix.Substring(dividerPos + 1);
//                    suffix = suffix.Substring(0, dividerPos);
//                }

//                if (street.EndsWith(suffix + "."))
//                {
//                    street = street.Substring(0, street.Length - suffix.Length - 1).Trim();

//                    if (actualStreetType.Length > 0)
//                        streetType = actualStreetType;
//                }
//                else if (street.EndsWith(suffix))
//                {
//                    street = street.Substring(0, street.Length - suffix.Length).Trim();

//                    if (actualStreetType.Length > 0)
//                        streetType = actualStreetType;
//                }

//                if (street.StartsWith(suffix + "."))
//                {
//                    street = street.Substring(suffix.Length + 1).Trim();

//                    if (actualStreetType.Length > 0)
//                        streetType = actualStreetType;
//                }
//                else if (street.StartsWith(suffix))
//                {
//                    street = street.Substring(suffix.Length).Trim();

//                    if (actualStreetType.Length > 0)
//                        streetType = actualStreetType;
//                }
//            }

//            // Consider street type
//            if (streetType.StartsWith(Properties.Resources.RentPaymentStreetTypeSquare1))
//            {
//                street += " " + Properties.Resources.RentPaymentStreetTypeSquare;
//            }
//            else if (streetType.StartsWith(Properties.Resources.RentPaymentStreetTypeAvenue1) ||
//                streetType.StartsWith(Properties.Resources.RentPaymentStreetTypeAvenue2) ||
//                streetType.StartsWith(Properties.Resources.RentPaymentStreetTypeAvenue3) ||
//                streetType.StartsWith(Properties.Resources.RentPaymentStreetTypeAvenue4) ||
//                streetType.StartsWith(Properties.Resources.RentPaymentStreetTypeAvenue5))
//            {
//                street += " " + Properties.Resources.RentPaymentStreetTypeAvenue;
//            }
//            else if (streetType.StartsWith(Properties.Resources.RentPaymentStreetTypeLane1))
//            {
//                street += " " + Properties.Resources.RentPaymentStreetTypeLane;
//            }
//            else if (streetType.StartsWith(Properties.Resources.RentPaymentStreetTypeBoulevard1))
//            {
//                street += " " + Properties.Resources.RentPaymentStreetTypeBoulevard;
//            }
//            else if (streetType.StartsWith(Properties.Resources.RentPaymentStreetTypeUzviz1))
//            {
//                street += " " + Properties.Resources.RentPaymentStreetTypeUzviz;
//            }
//        }

//        protected void UpdateObjectFreeSquareFromRentPayments()
//        {
//            DataTable table = new DataTable("rent_free_square");

//            table.Columns.Add("building_id", typeof(int));
//            table.Columns.Add("rent_period_id", typeof(int));
//            table.Columns.Add("organization_id", typeof(int));
//            table.Columns.Add("sqr_free_total", typeof(decimal));
//            table.Columns.Add("sqr_free_korysna", typeof(decimal));
//            table.Columns.Add("sqr_free_mzk", typeof(decimal));
//            table.Columns.Add("free_sqr_floors", typeof(string));
//            table.Columns.Add("free_sqr_purpose", typeof(string));

//            object[] rowValues = new object[8];

//            int buildingId = -1;
//            int periodId = -1;
//            int organizationId = -1;

//            decimal freeSqr = 0m;
//            decimal freeKorysna = 0m;
//            decimal freeMzk = 0m;
//            HashSet<string> floors = new HashSet<string>();
//            HashSet<string> purpose = new HashSet<string>();

//            string query = @"SELECT
//                obj.building_id,
//                obj.rent_period_id,
//                pay.org_balans_id,
//                obj.floors,
//                obj.purpose,
//                SUM(obj.sqr_free) AS 'sqr_free',
//                SUM(obj.sqr_korysna) AS 'sqr_korysna',
//                SUM(obj.sqr_mzk) AS 'sqr_mzk'
//            FROM
//                rent_object obj
//                LEFT OUTER JOIN rent_payment pay ON pay.id = obj.rent_payment_id
//            WHERE
//                NOT (obj.building_id IS NULL) AND NOT (obj.rent_period_id IS NULL) AND NOT (pay.org_balans_id IS NULL)
//            GROUP BY
//                obj.building_id, obj.rent_period_id, pay.org_balans_id, obj.floors, obj.purpose";

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        int curBuildingId = reader.GetInt32(0);
//                        int curPeriodId = reader.GetInt32(1);
//                        int curOrgId = reader.GetInt32(2);
//                        string curFloors = reader.IsDBNull(3) ? "" : reader.GetString(3);
//                        string curPurpose = reader.IsDBNull(4) ? "" : reader.GetString(4);
//                        decimal curFree = reader.IsDBNull(5) ? 0m : reader.GetDecimal(5);
//                        decimal curKorysna = reader.IsDBNull(6) ? 0m : reader.GetDecimal(6);
//                        decimal curMzk = reader.IsDBNull(7) ? 0m : reader.GetDecimal(7);

//                        curFloors = curFloors.Trim().ToLower();
//                        curPurpose = curPurpose.Trim().ToLower();

//                        if (curBuildingId != buildingId || curPeriodId != periodId || curOrgId != organizationId)
//                        {
//                            // Dump the accumulated values
//                            if (buildingId > 0 && periodId > 0 && organizationId > 0)
//                            {
//                                rowValues[0] = buildingId;
//                                rowValues[1] = periodId;
//                                rowValues[2] = organizationId;
//                                rowValues[3] = freeSqr;
//                                rowValues[4] = freeKorysna;
//                                rowValues[5] = freeMzk;
//                                rowValues[6] = ConcatenateStrSet(floors);
//                                rowValues[7] = ConcatenateStrSet(purpose);

//                                table.Rows.Add(rowValues);
//                            }

//                            buildingId = curBuildingId;
//                            periodId = curPeriodId;
//                            organizationId = curOrgId;

//                            freeSqr = 0m;
//                            freeKorysna = 0m;
//                            freeMzk = 0m;
//                            floors.Clear();
//                            purpose.Clear();
//                        }

//                        freeSqr += curFree;
//                        freeKorysna += curKorysna;
//                        freeMzk += curMzk;

//                        if (curFloors.Length > 0 && !floors.Contains(curFloors))
//                        {
//                            floors.Add(curFloors);
//                        }

//                        if (curPurpose.Length > 0 && !purpose.Contains(curPurpose))
//                        {
//                            purpose.Add(curPurpose);
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            // Dump the last portion of data
//            if (buildingId > 0 && periodId > 0 && organizationId > 0)
//            {
//                rowValues[0] = buildingId;
//                rowValues[1] = periodId;
//                rowValues[2] = organizationId;
//                rowValues[3] = freeSqr;
//                rowValues[4] = freeKorysna;
//                rowValues[5] = freeMzk;
//                rowValues[6] = ConcatenateStrSet(floors);
//                rowValues[7] = ConcatenateStrSet(purpose);

//                table.Rows.Add(rowValues);
//            }

//            // Store the table in SQL Server
//            BulkMigrateDataTable("rent_free_square", table);
//        }

//        protected string ConcatenateStrSet(HashSet<string> set)
//        {
//            string res = "";

//            foreach (string s in set)
//            {
//                if (res.Length > 0)
//                {
//                    res += ", ";
//                }

//                res += s;
//            }

//            return res;
//        }

//        #endregion (Rent Payments database migration)
//    }
}
