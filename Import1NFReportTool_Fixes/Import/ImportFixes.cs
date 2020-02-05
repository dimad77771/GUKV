using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace GUKV.DataMigration
{
    public class ImportFixes
    {
        /// <summary>
        /// Connection to the SQL Server
        /// </summary>
        private SqlConnection connectionSqlServer = null;

        /// <summary>
        /// Connection to the 1NF database in Firebird
        /// </summary>
        private FbConnection connection1NF = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImportFixes()
        {
        }

        #region Connecting to databases

        public bool TestConnections(Preferences preferences)
        {
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
                return false;
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
                return false;
            }

            return true;
        }

        public void OpenConnections()
        {
            connectionSqlServer.Open();
            connection1NF.Open();
        }

        public void CloseConnections()
        {
            connectionSqlServer.Close();
            connection1NF.Close();
        }

        #endregion (Connecting to databases)

        #region Rent square import failure fix

        public void FixRentSquareImportFailure()
        {
            //////////////////////////////////////////////////////////////////////////////////////
            // 1) Fix the rent agreement with existing ID (exported from 1NF)
            //////////////////////////////////////////////////////////////////////////////////////

            int numFixedRecords = 0;

            string query = "SELECT [id_arenda], [square] FROM arenda_1nf WHERE unloading = 1 AND edited = 1 AND deleted = 0 " +
                "AND (NOT [id_arenda] IS NULL) AND (NOT [square] IS NULL)";

            using (SqlCommand cmdSelect = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idArenda = reader.GetInt32(0);
                        decimal sqr = reader.GetDecimal(1);

                        string update = "UPDATE ARENDA1NF SET SQUARE = @psqr WHERE ID = @pid AND ISP = @pisp";

                        try
                        {
                            using (FbCommand cmdUpdate = new FbCommand(update, connection1NF))
                            {
                                cmdUpdate.Parameters.Add("psqr", sqr);
                                cmdUpdate.Parameters.Add("pid", idArenda);
                                cmdUpdate.Parameters.Add("pisp", "Auto-import");

                                cmdUpdate.ExecuteNonQuery();

                                numFixedRecords++;
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteError("SQL query failed: " + query);
                            WriteError("Message: " + ex.Message);
                            WriteError("psqr = " + sqr.ToString());
                            WriteError("pid = " + idArenda.ToString());
                            WriteError("pisp = Auto-import");
                        }
                    }

                    reader.Close();
                }
            }

            WriteInfo("Updated " + numFixedRecords.ToString() + " existing rent agreements");

            //////////////////////////////////////////////////////////////////////////////////////
            // 2) Fix the rent agreement that were added by user, and do not have an existing ID
            //////////////////////////////////////////////////////////////////////////////////////

            numFixedRecords = 0;

            query = "SELECT [dognum], [dogdate], [start_orenda], [finish_orenda], [plata_dogovor], [square] " +
                "FROM arenda_1nf WHERE unloading = 0 AND edited = 1 AND deleted = 0 " +
                "AND (NOT [dognum] IS NULL) " +
                "AND (NOT [dogdate] IS NULL) " +
                /* "AND (NOT [start_orenda] IS NULL) " + */
                "AND (NOT [finish_orenda] IS NULL) " +
                "AND (NOT [plata_dogovor] IS NULL) " +
                "AND (NOT [square] IS NULL) ";

            using (SqlCommand cmdSelect = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string dogNum = reader.GetString(0).Trim();
                        DateTime dogDate = reader.GetDateTime(1);
                        DateTime rentStart = reader.GetDateTime(2);
                        DateTime rentFinish = reader.GetDateTime(3);
                        decimal payment = reader.GetDecimal(4);
                        decimal sqr = reader.GetDecimal(5);

                        string update = "UPDATE ARENDA1NF SET SQUARE = @psqr WHERE " +
                            "DOGNUM = @pnum AND " +
                            "DOGDATE = @pdate AND " +
                            /* "START_ORENDA = @pstart AND " + */
                            "FINISH_ORENDA = @pfinish AND " +
                            "PLATA_DOGOVOR = @ppayment AND " +
                            "ISP = @pisp";

                        try
                        {
                            using (FbCommand cmdUpdate = new FbCommand(update, connection1NF))
                            {
                                cmdUpdate.Parameters.Add("psqr", sqr);
                                cmdUpdate.Parameters.Add("pnum", dogNum);
                                cmdUpdate.Parameters.Add("pdate", dogDate);
                                /* cmdUpdate.Parameters.Add("pstart", rentStart); */
                                cmdUpdate.Parameters.Add("pfinish", rentFinish);
                                cmdUpdate.Parameters.Add("ppayment", payment);
                                cmdUpdate.Parameters.Add("pisp", "Auto-import");

                                cmdUpdate.ExecuteNonQuery();

                                numFixedRecords++;
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteError("SQL query failed: " + update);
                            WriteError("Message: " + ex.Message);
                            WriteError("psqr = " + sqr.ToString());
                            WriteError("pnum = " + dogNum);
                            WriteError("pdate = " + dogDate.ToShortDateString());
                            WriteError("pstart = " + rentStart.ToShortDateString());
                            WriteError("pfinish = " + rentFinish.ToShortDateString());
                            WriteError("ppayment = " + payment.ToString());
                            WriteError("pisp = Auto-import");
                        }
                    }

                    reader.Close();
                }
            }

            WriteInfo("Updated " + numFixedRecords.ToString() + " new rent agreements");

            //////////////////////////////////////////////////////////////////////////////////////
            // 3) Fix the agreement notes with existing ID (exported from 1NF)
            //////////////////////////////////////////////////////////////////////////////////////

            numFixedRecords = 0;

            query = "SELECT [id_arenda_prim], [square] FROM arenda_prim WHERE unloading = 1 AND edited = 1 AND deleted = 0 " +
                "AND (NOT [id_arenda_prim] IS NULL) AND (NOT [square] IS NULL)";

            using (SqlCommand cmdSelect = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idArendaPrim = reader.GetInt32(0);
                        decimal sqr = reader.GetDecimal(1);

                        string update = "UPDATE ARENDA_PRIM SET SQUARE = @psqr WHERE ID = @pid AND ISP = @pisp";

                        try
                        {
                            using (FbCommand cmdUpdate = new FbCommand(update, connection1NF))
                            {
                                cmdUpdate.Parameters.Add("psqr", sqr);
                                cmdUpdate.Parameters.Add("pid", idArendaPrim);
                                cmdUpdate.Parameters.Add("pisp", "Auto-import");

                                cmdUpdate.ExecuteNonQuery();

                                numFixedRecords++;
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteError("SQL query failed: " + update);
                            WriteError("Message: " + ex.Message);
                            WriteError("psqr = " + sqr.ToString());
                            WriteError("pid = " + idArendaPrim.ToString());
                            WriteError("pisp = Auto-import");
                        }
                    }

                    reader.Close();
                }
            }

            WriteInfo("Updated " + numFixedRecords.ToString() + " existing rent notes");

            //////////////////////////////////////////////////////////////////////////////////////
            // 4) Fix the agreement notes that were added by user, and do not have an existing ID
            //////////////////////////////////////////////////////////////////////////////////////

            numFixedRecords = 0;

            query = "SELECT [descript], [purp_str], [ar_stavka], [price], [narah], [square] " +
                "FROM arenda_prim WHERE unloading = 0 AND edited = 1 AND deleted = 0 AND NOT ([square] IS NULL)";

            using (SqlCommand cmdSelect = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmdSelect.ExecuteReader())
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    while (reader.Read())
                    {
                        object description = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object purpose = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object stavka = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object price = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object narahovano = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        decimal sqr = reader.GetDecimal(5);

                        string update = "UPDATE ARENDA_PRIM SET SQUARE = @psqr WHERE ISP = @pisp";

                        parameters.Clear();

                        if (description is string)
                        {
                            update += " AND DESCRIPT = @descr";
                            parameters.Add("descr", description);
                        }

                        if (purpose is string)
                        {
                            update += " AND PURP_STR = @purp";
                            parameters.Add("purp", purpose);
                        }

                        if (stavka is decimal)
                        {
                            update += " AND AR_STAVKA = @stav";
                            parameters.Add("stav", stavka);
                        }

                        if (price is decimal)
                        {
                            update += " AND PRICE = @pri";
                            parameters.Add("pri", price);
                        }

                        if (narahovano is decimal)
                        {
                            update += " AND NARAH = @nar";
                            parameters.Add("nar", narahovano);
                        }

                        // Only make changes when search is reliable
                        if (parameters.Count >= 3)
                        {
                            try
                            {
                                using (FbCommand cmdUpdate = new FbCommand(update, connection1NF))
                                {
                                    cmdUpdate.Parameters.Add("psqr", sqr);
                                    cmdUpdate.Parameters.Add("pisp", "Auto-import");

                                    foreach (KeyValuePair<string, object> pair in parameters)
                                    {
                                        cmdUpdate.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                                    }

                                    cmdUpdate.ExecuteNonQuery();

                                    numFixedRecords++;
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteError("SQL query failed: " + update);
                                WriteError("Message: " + ex.Message);
                                WriteError("psqr = " + sqr.ToString());
                                WriteError("pisp = Auto-import");
                                DumpQueryParameters(parameters);
                            }
                        }
                    }

                    reader.Close();
                }
            }

            WriteInfo("Updated " + numFixedRecords.ToString() + " new rent notes");
        }

        #endregion (Rent square import failure fix)

        #region Balans duplicates fix for KZhSE

        public void FixKZhSEBalansDuplicates()
        {
            HashSet<int> IdsToDelete = new HashSet<int>();
            HashSet<string> uniqueKeys = new HashSet<string>();

            // Get all added entries for this organization
            string query = "SELECT ID, OBJ_ULNAME, OBJ_NOMER, SQR_ZAG, GRPURP, PURPOSE, PURP_STR, FLOATS, ZAL_VART FROM balans_1nf WHERE ORG = 1 AND ISP = 'Auto-import' ORDER BY ID";

            using (FbCommand cmdSelect = new FbCommand(query, connection1NF))
            {
                using (FbDataReader reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object balansId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object street = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object buildingNum = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object sqr = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object purposeGroup = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        object purposeCode = reader.IsDBNull(5) ? null : reader.GetValue(5);
                        object purposeStr = reader.IsDBNull(6) ? null : reader.GetValue(6);
                        object floors = reader.IsDBNull(7) ? null : reader.GetValue(7);
                        object cost = reader.IsDBNull(8) ? null : reader.GetValue(8);

                        if (balansId is int)
                        {
                            // Check if this is a duplicate entry
                            string key = GetBalansUniqueKey(street, buildingNum, sqr, purposeGroup, purposeCode, purposeStr, floors, cost);

                            if (uniqueKeys.Contains(key))
                            {
                                // Delete the duplicate entry
                                IdsToDelete.Add((int)balansId);
                            }
                            else
                            {
                                uniqueKeys.Add(key);
                            }
                        }
                    }

                    reader.Close();
                }
            }

            WriteInfo("Found " + IdsToDelete.Count.ToString() + " duplicate balans entries");

            // Delete all the found duplicates
            foreach (int balansId in IdsToDelete)
            {
                string delete = "UPDATE balans_1nf SET DELETED = 1 WHERE ID = " + balansId.ToString();

                using (FbCommand cmdDelete = new FbCommand(delete, connection1NF))
                {
                    cmdDelete.ExecuteNonQuery();
                }
            }

            WriteInfo("Deleted " + IdsToDelete.Count.ToString() + " duplicate balans entries");

            // Find the Arenda duplicate entries
            query = "SELECT ar.ID, ob.ulname, ob.nomer, ar.SQUARE, ar.org_kod, ar.start_orenda, ar.finish_orenda, ar.dognum, ar.dogdate, ar.vartist_exp " +
                "FROM arenda1nf ar LEFT OUTER join object_1nf ob ON ar.object_kod = ob.object_kod WHERE " +
                "ar.BALANS_KOD = 1 AND ar.ISP = 'Auto-import' ORDER BY ar.ID";

            uniqueKeys.Clear();
            IdsToDelete.Clear();

            using (FbCommand cmdSelect = new FbCommand(query, connection1NF))
            {
                using (FbDataReader reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object arendaId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object street = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object buildingNum = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object sqr = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object renterId = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        object rentStart = reader.IsDBNull(5) ? null : reader.GetValue(5);
                        object rentFinish = reader.IsDBNull(6) ? null : reader.GetValue(6);
                        object docNum = reader.IsDBNull(7) ? null : reader.GetValue(7);
                        object docDate = reader.IsDBNull(8) ? null : reader.GetValue(8);
                        object cost = reader.IsDBNull(9) ? null : reader.GetValue(9);

                        if (arendaId is int)
                        {
                            // Check if this is a duplicate entry
                            string key = GetArendaUniqueKey(street, buildingNum, sqr, renterId, rentStart, rentFinish, docNum, docDate, cost);

                            if (uniqueKeys.Contains(key))
                            {
                                // Delete the duplicate entry
                                IdsToDelete.Add((int)arendaId);
                            }
                            else
                            {
                                uniqueKeys.Add(key);
                            }
                        }
                    }

                    reader.Close();
                }
            }

            WriteInfo("Found " + IdsToDelete.Count.ToString() + " duplicate arenda entries");

            // Delete all the found duplicates
            foreach (int arendaId in IdsToDelete)
            {
                string delete = "UPDATE arenda1nf SET DELETED = 1 WHERE ID = " + arendaId.ToString();

                using (FbCommand cmdDelete = new FbCommand(delete, connection1NF))
                {
                    cmdDelete.ExecuteNonQuery();
                }
            }

            WriteInfo("Deleted " + IdsToDelete.Count.ToString() + " duplicate arenda entries");
        }

        private string GetBalansUniqueKey(object street, object buildingNum, object sqr, object purposeGroup,
            object purposeCode, object purposeStr, object floors, object cost)
        {
            string strStreet = (street == null) ? "NULL" : street.ToString().Trim();
            string strBuilding = (buildingNum == null) ? "NULL" : buildingNum.ToString().Trim();
            string strSqr = (sqr == null) ? "NULL" : sqr.ToString();
            string strPurposeGr = (purposeGroup == null) ? "NULL" : purposeGroup.ToString();
            string strPurposeCode = (purposeCode == null) ? "NULL" : purposeCode.ToString();
            string strPurposeStr = (purposeStr == null) ? "NULL" : purposeStr.ToString().Trim();
            string strFloors = (floors == null) ? "NULL" : floors.ToString().Trim();
            string strCost = (cost == null) ? "NULL" : cost.ToString();

            return strStreet + "_" + strBuilding + "_" + strSqr + "_" + strPurposeGr + "_" + strPurposeCode + "_" + strPurposeStr + "_" + strFloors + "_" + strCost;
        }

        private string GetArendaUniqueKey(object street, object buildingNum, object sqr, object renterId,
            object rentStart, object rentFinish, object docNum, object docDate, object cost)
        {
            string strStreet = (street == null) ? "NULL" : street.ToString().Trim();
            string strBuilding = (buildingNum == null) ? "NULL" : buildingNum.ToString().Trim();
            string strSqr = (sqr == null) ? "NULL" : sqr.ToString();
            string strRenter = (renterId == null) ? "NULL" : renterId.ToString();
            string strRentStart = (rentStart == null) ? "NULL" : ((DateTime)rentStart).ToShortDateString();
            string strRentFinish = (rentFinish == null) ? "NULL" : ((DateTime)rentFinish).ToShortDateString();
            string strDocNum = (docNum == null) ? "NULL" : docNum.ToString().Trim();
            string strDocDate = (docDate == null) ? "NULL" : ((DateTime)docDate).ToShortDateString();
            string strCost = (cost == null) ? "NULL" : cost.ToString();

            return strStreet + "_" + strBuilding + "_" + strSqr + "_" + strRenter + "_" + strRentStart + "_" + strRentFinish + "_" + strDocNum + "_" + strDocDate + "_" + strCost;
        }

        #endregion (Balans duplicates fix for KZhSE)

        #region Case sensitivity fix

        public void CaseSensitivityFix()
        {
            string query = "SELECT full_name FROM GUKV.dbo.organizations WHERE id = 8882";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlServer))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string nameLower = name.ToLower();
                    }

                    reader.Close();
                }
            }
        }

        #endregion (Case sensitivity fix)

        #region Testing and Debugging

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
