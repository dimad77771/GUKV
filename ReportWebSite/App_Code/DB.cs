using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;
using GUKV.Conveyancing;
using GUKV.ImportToolUtils;
using System.IO;
using log4net;
using System.Configuration;
using GUKV.Common;
using System.Data;
using ExtDataEntry.Models;

namespace GUKV.Conveyancing
{
    public class DictionaryValue
    {
        public int key = -1;
        public int parentKey = -1;
        public string value = "";

        public DictionaryValue()
        {
        }

        public DictionaryValue(int k, string v)
        {
            key = k;
            value = v;
        }

        public DictionaryValue(int k, int pk, string v)
        {
            key = k;
            parentKey = pk;
            value = v;
        }

        public override string ToString()
        {
            return value;
        }
    }

    public class DictionaryData
    {
        public string Name = "";

        public Dictionary<int, DictionaryValue> ValuesSql = new Dictionary<int, DictionaryValue>();
        //public Dictionary<int, DictionaryValue> ValuesNJF = new Dictionary<int, DictionaryValue>();

        public Dictionary<string, int> KeysSql = new Dictionary<string, int>();
        //public Dictionary<string, int> KeysNJF = new Dictionary<string, int>();

        public int MaxKeySql = -1;
        //public int MaxKeyNJF = -1;

        public DictionaryData()
        {
        }

        public DictionaryData(string name,
            SqlConnection connectionSql, string tableNameSql, string keyNameSql, string valueNameSql, string parentKeyNameSql, string sortFieldsSql/*,
            FbConnection connectionNJF, string tableNameNJF, string keyNameNJF, string valueNameNJF, string parentKeyNameNJF, string sortFieldsNJF*/)
        {
            Name = name;

            if (connectionSql != null)
            {
                LoadValues(connectionSql, ValuesSql, KeysSql, tableNameSql, keyNameSql, valueNameSql, parentKeyNameSql, sortFieldsSql);
            }

            // Calculate the max. keys
            foreach (KeyValuePair<int, DictionaryValue> pair in ValuesSql)
            {
                if (MaxKeySql < pair.Key)
                {
                    MaxKeySql = pair.Key;
                }
            }
        }

        //public DictionaryData(string name,
        //    FbConnection connection1NF, string tableName1NF, string keyName1NF, string valueName1NF, string parentKeyName1NF, string sortFields1NF,
        //    FbConnection connectionNJF, string tableNameNJF, string keyNameNJF, string valueNameNJF, string parentKeyNameNJF, string sortFieldsNJF)
        //{
        //    Name = name;

        //    if (connection1NF != null)
        //    {
        //        LoadValues(connection1NF, Values1NF, Keys1NF, tableName1NF, keyName1NF, valueName1NF, parentKeyName1NF, sortFields1NF);
        //    }

        //    if (connectionNJF != null)
        //    {
        //        LoadValues(connectionNJF, ValuesNJF, KeysNJF, tableNameNJF, keyNameNJF, valueNameNJF, parentKeyNameNJF, sortFieldsNJF);
        //    }

        //    // Calculate the max. keys
        //    foreach (KeyValuePair<int, DictionaryValue> pair in Values1NF)
        //    {
        //        if (MaxKey1NF < pair.Key)
        //        {
        //            MaxKey1NF = pair.Key;
        //        }
        //    }

        //    foreach (KeyValuePair<int, DictionaryValue> pair in ValuesNJF)
        //    {
        //        if (MaxKeyNJF < pair.Key)
        //        {
        //            MaxKeyNJF = pair.Key;
        //        }
        //    }
        //}

        private void LoadValues(SqlConnection connection, Dictionary<int, DictionaryValue> values, Dictionary<string, int> keys,
            string tableName, string keyName, string valueName, string parentKeyName, string sortFields)
        {
            string orderBy = sortFields;

            if (orderBy.Length == 0)
            {
                orderBy = keyName;
            }

            if (parentKeyName.Length > 0)
            {
                // Hierarchical dictionary
                using (SqlCommand cmd = new SqlCommand("SELECT " + keyName + ", " + valueName + ", " + parentKeyName + " FROM " + tableName +
                    " WHERE (NOT " + valueName + " IS NULL) AND (" + valueName + " <> '') ORDER BY " + orderBy, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                int key = reader.GetInt32(0);
                                string value = reader.GetString(1).Trim().ToUpper();
                                int parentKey = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);

                                values[key] = new DictionaryValue(key, parentKey, value);
                                keys[value] = key;
                            }
                        }

                        reader.Close();
                    }
                }
            }
            else
            {
                // Simple dictionary
                using (SqlCommand cmd = new SqlCommand("SELECT " + keyName + ", " + valueName + " FROM " + tableName +
                    " WHERE (NOT " + valueName + " IS NULL) AND (" + valueName + " <> '') ORDER BY " + orderBy, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                int key = reader.GetInt32(0);
                                string value = reader.GetString(1).Trim().ToUpper();

                                values[key] = new DictionaryValue(key, value);
                                keys[value] = key;
                            }
                        }

                        reader.Close();
                    }
                }
            }
        }

        //private void LoadValues(FbConnection connection, Dictionary<int, DictionaryValue> values, Dictionary<string, int> keys,
        //    string tableName, string keyName, string valueName, string parentKeyName, string sortFields)
        //{
        //    string orderBy = sortFields;

        //    if (orderBy.Length == 0)
        //    {
        //        orderBy = keyName;
        //    }

        //    if (parentKeyName.Length > 0)
        //    {
        //        // Hierarchical dictionary
        //        using (FbCommand cmd = new FbCommand("SELECT " + keyName + ", " + valueName + ", " + parentKeyName + " FROM " + tableName +
        //            " WHERE (NOT " + valueName + " IS NULL) AND (" + valueName + " <> '') ORDER BY " + orderBy, connection))
        //        {
        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    if (!reader.IsDBNull(0))
        //                    {
        //                        int key = reader.GetInt32(0);
        //                        string value = reader.GetString(1).Trim().ToUpper();
        //                        int parentKey = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);

        //                        values[key] = new DictionaryValue(key, parentKey, value);
        //                        keys[value] = key;
        //                    }
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Simple dictionary
        //        using (FbCommand cmd = new FbCommand("SELECT " + keyName + ", " + valueName + " FROM " + tableName +
        //            " WHERE (NOT " + valueName + " IS NULL) AND (" + valueName + " <> '') ORDER BY " + orderBy, connection))
        //        {
        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    if (!reader.IsDBNull(0))
        //                    {
        //                        int key = reader.GetInt32(0);
        //                        string value = reader.GetString(1).Trim().ToUpper();

        //                        values[key] = new DictionaryValue(key, value);
        //                        keys[value] = key;
        //                    }
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
        //}
    }

    public static class DB
    {
        private static readonly ILog log = LogManager.GetLogger("ReportWebSite");

        public static string UserName = "SYSDBA";

        public static string UserPassword = "masterkey";

        //public static Dictionary<string, string> users = new Dictionary<string, string>();

        public static Dictionary<int, Organization1NF> organizations1NF = new Dictionary<int, Organization1NF>();

        public static Dictionary<int, Object1NF> objects1NF = new Dictionary<int, Object1NF>();

        public static Dictionary<int, Dictionary<int, BalansObject1NF>> balans1NFByAddress = new Dictionary<int, Dictionary<int, BalansObject1NF>>();

        public static Dictionary<int, BalansObject1NF> balans1NFByID = new Dictionary<int, BalansObject1NF>();

        public static Dictionary<int, Document> documentsNJF = new Dictionary<int, Document>();

        public static Dictionary<int, Document> actsNJF = new Dictionary<int, Document>();

        public static Dictionary<int, Organization1NF> organizationsNJF = new Dictionary<int, Organization1NF>();

        public static Dictionary<string, DictionaryData> dictionaries = new Dictionary<string, DictionaryData>();

        private static Dictionary<int, int> manualStreetCodeMappingNJFto1NF = null;

        private static Dictionary<int, int> objectTypeMappingNJFto1NF = GUKV.ImportToolUtils.FieldMappings.CreateObjectTypeMappingNJFto1NF(-1, -1, -1);

        public const int MAX_DOC_NUMBER_LEN = 20;
        public const int MAX_DOC_TITLE_LEN = 255;
        public const int MAX_OBJECT_NAME_LEN = 255;
        public const int MAX_DIAMETER_LEN = 20;
        public const int MAX_CHARACTERISTICS_LEN = 100;

        //public const string DICT_PURPOSE = "PURPOSE";
        //public const string DICT_PURPOSE_GROUP = "PURPOSE_GROUP";
        //public const string DICT_OBJ_KIND = "OBJ_KIND";
        //public const string DICT_OBJ_TYPE = "OBJ_TYPE";
        //public const string DICT_STREETS = "STREETS";
        //public const string DICT_DISTRICTS = "DISTRICTS";
        //public const string DICT_ORG_INDUSTRY = "INDUSTRY";
        //public const string DICT_ORG_OCCUPATION = "OCCUPATION";
        //public const string DICT_ORG_TYPE = "ORG_TYPE";
        //public const string DICT_ORG_FORM_GOSP = "FIN_FORM";
        //public const string DICT_ORG_OWNERSHIP = "OWNERSHIP";
        //public const string DICT_RIGHTS = "RIGHTS";
        //public const string DICT_DOC_TYPES = "DOC_TYPES";
        //public const string DICT_TECH_STATE = "OBJ_TECH_STATE";


        #region Interface

        public static decimal ConvertStrToDecimal(ref string value)
        {
            // Remove all unnecessary characters; leave just the number
            string strippedValue = "";
            bool commaFound = false;
            bool dotFound = false;

            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsDigit(value[i]))
                {
                    strippedValue += value[i];
                }
                else if (value[i] == ',')
                {
                    strippedValue += ',';
                    commaFound = true;
                }
                else if (value[i] == '.')
                {
                    strippedValue += '.';
                    dotFound = true;
                }
            }

            try
            {
                decimal d = decimal.Parse(strippedValue);
            }
            catch (Exception)
            {
                if (dotFound)
                {
                    strippedValue = strippedValue.Replace('.', ',');
                }
                else if (commaFound)
                {
                    strippedValue = strippedValue.Replace(',', '.');
                }
            }

            value = strippedValue;

            return decimal.Parse(strippedValue);
        }


        //public static int CreateBalansObjectNew(Preferences preferences, int objectId, decimal sqr, int orgToId)
        //{
        //    int newBalansId = -1;
        //    Dictionary<string, object> values = new Dictionary<string, object>();
        //    FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString());
        //    try
        //    {
        //        connection1NF.Open();

        //        FbTransaction transaction = null;
        //        try
        //        {
        //            transaction = connection1NF.BeginTransaction();

        //            // Get information about the object
        //            string query = @"SELECT FULL_ULNAME, ULKOD, ULNAME2, ULKOD2, NOMER1, NOMER2, NOMER3, ADRDOP FROM OBJECT_1NF WHERE OBJECT_KOD = @bid";
        //            using (FbCommand cmd = new FbCommand(query, connection1NF))
        //            {
        //                try
        //                {
        //                    cmd.Parameters.Add(new FbParameter("bid", objectId));
        //                    cmd.Transaction = transaction;

        //                    using (FbDataReader reader = cmd.ExecuteReader())
        //                    {
        //                        if (reader.Read())
        //                        {
        //                            if (!reader.IsDBNull(1))
        //                                values.Add("OBJ_KODUL", reader.GetValue(1));

        //                            if (!reader.IsDBNull(3))
        //                                values.Add("ULKOD2", reader.GetValue(3));

        //                            if (!reader.IsDBNull(0))
        //                                values.Add("OBJ_ULNAME", reader.GetValue(0));

        //                            if (!reader.IsDBNull(2))
        //                                values.Add("ULNAME2", reader.GetValue(2));

        //                            if (!reader.IsDBNull(4))
        //                                values.Add("OBJ_NOMER1", reader.GetValue(4));

        //                            if (!reader.IsDBNull(5))
        //                                values.Add("OBJ_NOMER2", reader.GetValue(5));

        //                            if (!reader.IsDBNull(6))
        //                                values.Add("OBJ_NOMER3", reader.GetValue(6));

        //                            if (!reader.IsDBNull(7))
        //                                values.Add("OBJ_ADRDOP", reader.GetValue(7));
        //                        }

        //                        reader.Close();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    log.Info(ex.Message);
        //                    throw;
        //                }


        //                // Generate new balans Id
        //                newBalansId = GenerateNewFirebirdId(connection1NF, "GEN_BALANS_1NF", transaction);

        //                if (newBalansId > 0)
        //                {
        //                    values.Add("ID", newBalansId);
        //                    values.Add("ORG", orgToId);
        //                    values.Add("LYEAR", DateTime.Now.Year);
        //                    values.Add("SQR_ZAG", sqr);

        //                    // Generate parameters for INSERT statement
        //                    Dictionary<string, object> parameters1NF = new Dictionary<string, object>();
        //                    Dictionary<string, object> parametersSQL = new Dictionary<string, object>();

        //                    int paramIndex = 1;
        //                    string insertFieldList1NF = "";
        //                    string insertParamList1NF = "";

        //                    foreach (KeyValuePair<string, object> pair in values)
        //                    {
        //                        string paramName = "p" + paramIndex.ToString();

        //                        if (paramIndex > 1)
        //                        {
        //                            insertFieldList1NF += ", ";
        //                            insertParamList1NF += ", ";
        //                        }

        //                        insertFieldList1NF += pair.Key;
        //                        insertParamList1NF += "@" + paramName;

        //                        parameters1NF[paramName] = pair.Value;

        //                        paramIndex++;
        //                    }

        //                    // Prepare the INSERT statement
        //                    query = "INSERT INTO BALANS_1NF (" + insertFieldList1NF + ") VALUES (" + insertParamList1NF + ")";

        //                    using (FbCommand commandInsert = new FbCommand(query, connection1NF))
        //                    {
        //                        try
        //                        {
        //                            foreach (KeyValuePair<string, object> pair in parameters1NF)
        //                            {
        //                                commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
        //                            }

        //                            commandInsert.Transaction = transaction;
        //                            commandInsert.ExecuteNonQuery();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            log.Info(ex.Message);
        //                            throw;
        //                        }
        //                    }
        //                }

        //            }

        //            transaction.Commit();
        //        }
        //        catch (Exception e)
        //        {
        //            if (transaction != null)
        //            {
        //                transaction.Rollback();
        //            }
        //        }
        //        connection1NF.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info(ex.Message);
        //    }

        //    return newBalansId;
        //}

        //public static void LoadUsers(Preferences preferences)
        //{
        //    FbConnection connection = new FbConnection(preferences.GetUsersConnectionString());

        //    try
        //    {
        //        connection.Open();

        //        using (FbCommand cmd = new FbCommand("SELECT SYS_USER, REAL_USER FROM SYSTEM_USER", connection))
        //        {
        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    users.Add(reader.GetString(0), reader.GetString(1));
        //                }

        //                reader.Close();
        //            }
        //        }

        //        connection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних користувачів",
        //            System.Windows.Forms.MessageBoxButtons.OK,
        //            System.Windows.Forms.MessageBoxIcon.Error);
        //    }
        //}

        //public static void LoadData(Preferences preferences)
        //{
        //    // Test connections
        //    //FbConnection connectionNJF = new FbConnection(preferences.GetNJFConnectionString(UserName, UserPassword));

        //    //try
        //    //{
        //    //    connectionNJF.Open();
        //    //    connectionNJF.Close();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    log.Info("Помилка доступу до бази даних 'Розпорядження': " + ex.Message);
        //    //    return;
        //    //}

        //    //FbConnection connection1NF = null;// new FbConnection(preferences.Get1NFConnectionString());

        //    //try
        //    //{
        //    //    //connection1NF.Open();
        //    //    connection1NF.Close();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    log.Info("Помилка доступу до бази даних '1НФ': " + ex.Message);
        //    //    return;
        //    //}

        //    // Load data from both databases
        //    try
        //    {
        //        //connection1NF.Open();
        //        //connectionNJF.Open();

        //        ///////////////////////////////////////////////////////////////////////////////////
        //        // Load dictionaries

        //        // Purpose
        //        dictionaries.Add(DICT_PURPOSE, new DictionaryData(DICT_PURPOSE,
        //            connection1NF, "SPURPOSE", "KOD", "NAME", "GR", "",
        //            connectionNJF, "SPURPOSE", "KOD", "NAME", "GR", ""));

        //        // Purpose Group
        //        dictionaries.Add(DICT_PURPOSE_GROUP, new DictionaryData(DICT_PURPOSE_GROUP,
        //            connection1NF, "SGRPURPOSE", "KOD", "NAME", "", "",
        //            connectionNJF, "SGRPURPOSE", "KOD", "NAME", "", ""));

        //        // Object kind
        //        dictionaries.Add(DICT_OBJ_KIND, new DictionaryData(DICT_OBJ_KIND,
        //            connection1NF, "SKINDOBJ", "KOD", "NAME", "", "",
        //            connectionNJF, "SKINDOBJ", "KOD", "NAME", "", ""));

        //        // Object type
        //        dictionaries.Add(DICT_OBJ_TYPE, new DictionaryData(DICT_OBJ_TYPE,
        //            connection1NF, "STYPEOBJ", "KOD", "NAME", "", "",
        //            connectionNJF, "STYPEOBJ", "KOD", "NAME", "", ""));

        //        // Transfer rights (exists in NJF only)
        //        dictionaries.Add(DICT_RIGHTS, new DictionaryData(DICT_RIGHTS,
        //            null, "", "", "", "", "",
        //            connectionNJF, "SPRAVO", "KOD", "NAME", "", ""));

        //        // Document types (exists in NJF only)
        //        dictionaries.Add(DICT_DOC_TYPES, new DictionaryData(DICT_DOC_TYPES,
        //            null, "", "", "", "", "",
        //            connectionNJF, "SKINDDOK", "KOD", "NAME", "", ""));

        //        // Street names
        //        dictionaries.Add(DICT_STREETS, new DictionaryData(DICT_STREETS,
        //            connection1NF, "SUL", "KOD", "NAME", "", "KOD, STAN",
        //            connectionNJF, "SUL", "KOD", "NAME", "", ""));

        //        // Districts
        //        dictionaries.Add(DICT_DISTRICTS, new DictionaryData(DICT_DISTRICTS,
        //            connection1NF, "S_RAYON2", "KOD_RAYON2", "NAME_RAYON2", "", "",
        //            connectionNJF, "SRA", "KOD", "NAME", "", ""));

        //        // Organization types
        //        dictionaries.Add(DICT_ORG_TYPE, new DictionaryData(DICT_ORG_TYPE,
        //            connection1NF, "S_ORG_FORM", "KOD_ORG_FORM", "NAME_ORG_FORM", "", "",
        //            connectionNJF, "SPV", "KOD", "NAME", "", ""));

        //        // Industry
        //        dictionaries.Add(DICT_ORG_INDUSTRY, new DictionaryData(DICT_ORG_INDUSTRY,
        //            connection1NF, "S_GALUZ", "KOD_GALUZ", "NAME_GALUZ", "", "",
        //            connectionNJF, "SOT", "KOD", "NAME", "", ""));

        //        // Occupation (exists only in 1NF)
        //        dictionaries.Add(DICT_ORG_OCCUPATION, new DictionaryData(DICT_ORG_OCCUPATION,
        //            connection1NF, "S_VID_DIAL", "KOD_VID_DIAL", "NAME_VID_DIAL", "KOD_GALUZ", "",
        //            null, "", "", "", "", ""));

        //        // Organization finance types
        //        dictionaries.Add(DICT_ORG_FORM_GOSP, new DictionaryData(DICT_ORG_FORM_GOSP,
        //            connection1NF, "S_FORM_GOSP", "KOD_FORM_GOSP", "NAME_FORM_GOSP", "", "",
        //            connectionNJF, "SFG", "KOD", "NAME", "", ""));

        //        // Organization form of ownership
        //        dictionaries.Add(DICT_ORG_OWNERSHIP, new DictionaryData(DICT_ORG_OWNERSHIP,
        //            connection1NF, "S_FORM_VLASN", "KOD_FORM_VLASN", "NAME_FORM_VLASN", "", "",
        //            connectionNJF, "SFORMVL", "KOD", "NAME", "", ""));

        //        // Technical state
        //        dictionaries.Add(DICT_TECH_STATE, new DictionaryData(DICT_TECH_STATE,
        //            connection1NF, "STEXSTAN", "KOD", "NAME", "", "",
        //            connectionNJF, "STEXSTAN", "KOD", "NAME", "", ""));

        //        RemoveOldDistricts();

        //        ///////////////////////////////////////////////////////////////////////////////////
        //        // Load database objects

        //        LoadOrganizationsFrom1NF(connection1NF);
        //        //LoadObjectsFrom1NF(connection1NF);
        //        LoadBalansObjectsFrom1NF(connection1NF);
        //        LoadDocumentsFromNJF(connectionNJF);

        //        organizationsNJF = LoadOrganizationsFromNJF(connectionNJF);

        //        //connection1NF.Close();
        //        //connectionNJF.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info("Помилка доступу до бази даних: " + ex.Message);
        //    }
        //}

        public static int FindCodeInDictionary1NF_Hierarchical(string dictionaryName, string name, int parentKey)
        {
            name = name.Trim().ToUpper();

            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Value.value.Trim().ToUpper() == name && pair.Value.parentKey == parentKey)
                    {
                        return pair.Key;
                    }
                }
            }

            return -1;
        }

        public static int FindCodeInDictionaryNJF(string dictionaryName, string name)
        {
            name = name.Trim().ToUpper();

            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Value.value.Trim().ToUpper() == name)
                    {
                        return pair.Key;
                    }
                }
            }

            return -1;
        }

        public static int FindCodeInDictionaryNJF_Hierarchical(string dictionaryName, string name, int parentKey)
        {
            name = name.Trim().ToUpper();

            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Value.value.Trim().ToUpper() == name && pair.Value.parentKey == parentKey)
                    {
                        return pair.Key;
                    }
                }
            }

            return -1;
        }

        public static string FindNameInDictionary1NF(string dictionaryName, int id)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Key == id)
                    {
                        return pair.Value.value.Trim();
                    }
                }
            }

            return "";
        }

        public static string FindNameInDictionary1NF_Hierarchical(string dictionaryName, int id, int parentKey)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Key == id && pair.Value.parentKey == parentKey)
                    {
                        return pair.Value.value.Trim();
                    }
                }
            }

            return "";
        }

        public static string FindNameInDictionaryNJF(string dictionaryName, int id)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Key == id)
                    {
                        return pair.Value.value.Trim();
                    }
                }
            }

            return "";
        }

        public static string FindNameInDictionaryNJF_Hierarchical(string dictionaryName, int id, int parentKey)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Key == id && pair.Value.parentKey == parentKey)
                    {
                        return pair.Value.value.Trim();
                    }
                }
            }

            return "";
        }

        public static DictionaryValue FindValueInDictionary1NF(string dictionaryName, int id)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Key == id)
                    {
                        return pair.Value;
                    }
                }
            }

            return null;
        }

        public static DictionaryValue FindValueInDictionary1NF_Hierarchical(string dictionaryName, int id, int parentKey)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Key == id && pair.Value.parentKey == parentKey)
                    {
                        return pair.Value;
                    }
                }
            }

            return null;
        }

        public static DictionaryValue FindValueInDictionaryNJF(string dictionaryName, int id)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Key == id)
                    {
                        return pair.Value;
                    }
                }
            }

            return null;
        }

        public static DictionaryValue FindValueInDictionaryNJF_Hierarchical(string dictionaryName, int id, int parentKey)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Key == id && pair.Value.parentKey == parentKey)
                    {
                        return pair.Value;
                    }
                }
            }

            return null;
        }

        public static void FillComboBoxFromDictionary1NF(System.Windows.Forms.ComboBox combo, string dictionaryName)
        {
            combo.Items.Clear();

            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    combo.Items.Add(pair.Value);
                }
            }
        }

        public static void FillComboBoxFromDictionary1NF_Hierarchical(System.Windows.Forms.ComboBox combo,
            string dictionaryName, int parentKey)
        {
            combo.Items.Clear();

            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Value.parentKey == parentKey)
                    {
                        combo.Items.Add(pair.Value);
                    }
                }
            }
        }

        public static void FillComboBoxFromDictionaryNJF(System.Windows.Forms.ComboBox combo, string dictionaryName)
        {
            combo.Items.Clear();

            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    combo.Items.Add(pair.Value);
                }
            }
        }

        public static void FillComboBoxFromDictionaryNJF_Hierarchical(System.Windows.Forms.ComboBox combo,
            string dictionaryName, int parentKey)
        {
            combo.Items.Clear();

            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
                {
                    if (pair.Value.parentKey == parentKey)
                    {
                        combo.Items.Add(pair.Value);
                    }
                }
            }
        }

        public static void RemoveComboBoxValue(System.Windows.Forms.ComboBox combo, int key)
        {
            int indexToRemove = -1;

            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (combo.Items[i] is DictionaryValue && (combo.Items[i] as DictionaryValue).key == key)
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove >= 0)
            {
                combo.Items.RemoveAt(indexToRemove);
            }
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <param name="key1NF"></param>
        /// <returns></returns>
        //public static int MatchDictionary1NFtoNJF(string dictionaryName, int key1NF)
        //{
        //    DictionaryData data = null;

        //    if (dictionaries.TryGetValue(dictionaryName, out data))
        //    {
        //        DictionaryValue value1NF = null;

        //        if (data.ValuesSql.TryGetValue(key1NF, out value1NF))
        //        {
        //            foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
        //            {
        //                if (pair.Value.value == value1NF.value)
        //                {
        //                    return pair.Key;
        //                }
        //            }
        //        }
        //    }

        //    return -1;
        //}

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <param name="key1NF"></param>
        /// <returns></returns>
        //public static int MatchDictionaryNJFto1NF(string dictionaryName, int key1NF)
        //{
        //    DictionaryData data = null;

        //    if (dictionaries.TryGetValue(dictionaryName, out data))
        //    {
        //        DictionaryValue valueNJF = null;

        //        if (data.ValuesSql.TryGetValue(key1NF, out valueNJF))
        //        {
        //            foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
        //            {
        //                if (pair.Value.value == valueNJF.value)
        //                {
        //                    return pair.Key;
        //                }
        //            }
        //        }
        //    }

        //    return -1;
        //}

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="ownershipId1NF"></param>
        /// <returns></returns>
        //public static int MatchOrgOwnership1NFtoNJF(int ownershipId1NF)
        //{
        //    return ownershipId1NF;

        //    switch (ownershipId1NF)
        //    {
        //        case 10:
        //        case 20:
        //        case 31:
        //        case 33:
        //        case 34:
        //        case 35:
        //        case 40:
        //        case 50:
        //            return ownershipId1NF;

        //        case 38:
        //            return 34;

        //        case 99: // ВЛАСНІСТЬ НЕ ВИЗНАЧЕНА - ПРИ ЭТОМ ЗНАЧЕНИИ ПОЛЯ НЕВОЗМОЖНО ПЕРЕДАТЬ ОБЪЕКТ С БАЛАНСА НА БАЛАНС, ТАК КАК РУГАЕТСЯ НА FOREIGN KEY
        //            return -1;
        //    }

        //    return MatchDictionary1NFtoNJF(DICT_ORG_OWNERSHIP, ownershipId1NF);
        //}

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="formGospId1NF"></param>
        /// <returns></returns>
        //public static int MatchOrgFormGosp1NFtoNJF(int formGospId1NF)
        //{
        //    return formGospId1NF;
        //    switch (formGospId1NF)
        //    {
        //        case 1:
        //        case 2:
        //        case 3:
        //        case 4:
        //        case 5:
        //        case 6:
        //            return formGospId1NF;
        //    }

        //    return MatchDictionary1NFtoNJF(DICT_ORG_FORM_GOSP, formGospId1NF);
        //}

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="objectTypeIdNJF"></param>
        /// <returns></returns>
        //public static int MatchObjectTypeNJFto1NF(int objectTypeIdNJF)
        //{
        //    int objectTyp11NF = -1;

        //    if (objectTypeMappingNJFto1NF.TryGetValue(objectTypeIdNJF, out objectTyp11NF))
        //    {
        //        return objectTyp11NF;
        //    }

        //    return -1;
        //}

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="objectTypeIdNJF"></param>
        /// <returns></returns>
        //public static int MatchObjectKindNJFto1NF(int objectTypeIdNJF)
        //{
        //    if (objectTypeIdNJF >= 1 && objectTypeIdNJF <= 4)
        //    {
        //        return objectTypeIdNJF;
        //    }

        //    return -1;
        //}

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <param name="key1NF"></param>
        /// <param name="parentDictionaryName"></param>
        /// <param name="parentKey1NF"></param>
        /// <returns></returns>
        //public static int MatchDictionary1NFtoNJF_Hierarchical(string dictionaryName, int key1NF,
        //    string parentDictionaryName, int parentKey1NF)
        //{
        //    int parentKeyNJF = MatchDictionary1NFtoNJF(parentDictionaryName, parentKey1NF);

        //    if (parentKeyNJF >= 0)
        //    {
        //        DictionaryData data = null;

        //        if (dictionaries.TryGetValue(dictionaryName, out data))
        //        {
        //            DictionaryValue value1NF = new DictionaryValue();

        //            if (data.ValuesSql.TryGetValue(key1NF, out value1NF))
        //            {
        //                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
        //                {
        //                    if (pair.Value.parentKey == parentKeyNJF && pair.Value.value == value1NF.value)
        //                    {
        //                        return pair.Key;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return -1;
        //}

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <param name="keyNJF"></param>
        /// <param name="parentDictionaryName"></param>
        /// <param name="parentKeyNJF"></param>
        /// <returns></returns>
        //public static int MatchDictionaryNJFto1NF_Hierarchical(string dictionaryName, int keyNJF,
        //    string parentDictionaryName, int parentKeyNJF)
        //{
        //    int parentKey1NF = MatchDictionaryNJFto1NF(parentDictionaryName, parentKeyNJF);

        //    if (parentKey1NF >= 0)
        //    {
        //        DictionaryData data = null;

        //        if (dictionaries.TryGetValue(dictionaryName, out data))
        //        {
        //            DictionaryValue valueNJF = new DictionaryValue();

        //            if (data.ValuesSql.TryGetValue(keyNJF, out valueNJF))
        //            {
        //                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
        //                {
        //                    if (pair.Value.parentKey == parentKey1NF && pair.Value.value == valueNJF.value)
        //                    {
        //                        return pair.Key;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return -1;
        //}

        public static int FindDocument(string docNum, DateTime docDate, int docKind)
        {
            docNum = docNum.Trim().ToUpper();

            foreach (KeyValuePair<int, Document> doc in documentsNJF)
            {
                if (doc.Value.documentNumber == docNum && doc.Value.documentDate.Date == docDate.Date)
                {
                    if (docKind > 0)
                    {
                        if (doc.Value.documentKind == docKind)
                        {
                            return doc.Key;
                        }
                    }
                    else
                    {
                        return doc.Key;
                    }
                }
            }

            return -1;
        }

        public static int FindAct(string actNum, DateTime actDate, string rishNum, DateTime rishDate)
        {
            actNum = actNum.Trim().ToUpper();
            rishNum = rishNum.Trim().ToUpper();

            int parentId = FindDocument(rishNum, rishDate, -1);

            if (parentId > 0)
            {
                Document parentDoc = null;

                if (documentsNJF.TryGetValue(parentId, out parentDoc))
                {
                    if (parentDoc.dependentDocuments != null)
                    {
                        foreach (int childId in parentDoc.dependentDocuments)
                        {
                            Document childDoc = null;

                            if (actsNJF.TryGetValue(childId, out childDoc))
                            {
                                if (childDoc.documentNumber.Trim().ToUpper() == actNum && childDoc.documentDate.Date == actDate.Date)
                                {
                                    return childId;
                                }
                            }
                        }
                    }
                }
            }

            return -1;
        }

        public static int FindActByNumber(string actNum, DateTime actDate)
        {
            actNum = actNum.Trim().ToUpper();

            foreach (KeyValuePair<int, Document> doc in actsNJF)
            {
                if (doc.Value.documentNumber == actNum && doc.Value.documentDate.Date == actDate.Date)
                {
                    return doc.Key;
                }
            }

            return -1;
        }

        //public static int CreateNewNJFObject(/*FbConnection connectionNJF, FbTransaction transaction,*/
        //    string street, string district, string nomer1, string nomer2, string nomer3, string addrMisc,
        //    out int streetId, out int districtId)
        //{
        //    int objectId = -1;

        //    street = street.Trim().ToUpper();
        //    district = district.Trim().ToUpper();
        //    nomer1 = nomer1.Trim().ToUpper();
        //    nomer2 = nomer2.Trim().ToUpper();
        //    nomer3 = nomer3.Trim().ToUpper();
        //    addrMisc = addrMisc.Trim().ToUpper();

        //    if (nomer3.StartsWith("К.")) //if (nomer3.StartsWith(Properties.AppResources.KorpusPrefix))
        //    {
        //        nomer3 = nomer3.Substring(2);
        //    }

        //    if (street.Length <= 0)
        //    {
        //        throw new ArgumentException("Для створення нового об'єкту необхідно вказати вулицю.");
        //    }

        //    if ((nomer1 + nomer2 + nomer3 + addrMisc).Length == 0)
        //    {
        //        throw new ArgumentException("Для створення нового об'єкту необхідно вказати номер будинку.");
        //    }

        //    streetId = FindStreetMatchInNJF(street/*, connectionNJF, transaction*/);

        //    if (streetId <= 0)
        //    {
        //        throw new ArgumentException("Вулицю " + street + " не знайдено в базі Розпорядження");
        //    }

        //    districtId = -1;

        //    if (district.Length > 0)
        //    {
        //        districtId = FindDistrictMatchInNJF(district);
        //    }

        //    // If address can not fit into narrow columns, try to fix it
        //    if (nomer3.Length > 3)
        //    {
        //        addrMisc = nomer3 + ((addrMisc.Length > 0) ? " " + addrMisc : "");
        //        nomer3 = "";
        //    }

        //    if (nomer2.Length > 4)
        //    {
        //        addrMisc = nomer2 + ((addrMisc.Length > 0) ? " " + addrMisc : "");
        //        nomer2 = "";
        //    }

        //    // Generate the new address code
        //    int addrId = GenerateNewFirebirdId(connectionNJF, "ADR_REES_GEN", transaction);

        //    if (addrId > 0)
        //    {
        //        // Insert the new record into ADR_REES table
        //        string fields = "ADR_REES_KOD, ADR_REES_KODSTAN, ULNAME, DT_BEG, ADRDOP, NOMER1, NOMER2, NOMER3, ULKOD, DT, ISP";
        //        string parameters = "@kod, 0, @strt, @edt, @misc, @nom1, @nom2, @nom3, @strtid, @edt, @usr";

        //        if (districtId > 0)
        //        {
        //            fields += ", NEWDISTR";
        //            parameters += ", @distrid";
        //        }

        //        string query = "INSERT INTO ADR_REES (" + fields + ") VALUES (" + parameters + ")";

        //        using (FbCommand commandInsert = new FbCommand(query, connectionNJF))
        //        {
        //            try
        //            {
        //                commandInsert.Parameters.Add(new FbParameter("kod", addrId));
        //                commandInsert.Parameters.Add(new FbParameter("strt", street.Length > 60 ? street.Substring(0, 60) : street));
        //                commandInsert.Parameters.Add(new FbParameter("misc", addrMisc.Length > 60 ? addrMisc.Substring(0, 60) : addrMisc));
        //                commandInsert.Parameters.Add(new FbParameter("nom1", nomer1.Length > 4 ? nomer1.Substring(0, 4) : nomer1));
        //                commandInsert.Parameters.Add(new FbParameter("nom2", nomer2.Length > 4 ? nomer2.Substring(0, 4) : nomer2));
        //                commandInsert.Parameters.Add(new FbParameter("nom3", nomer3.Length > 3 ? nomer3.Substring(0, 3) : nomer3));
        //                commandInsert.Parameters.Add(new FbParameter("strtid", streetId));
        //                commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
        //                commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));

        //                if (districtId > 0)
        //                {
        //                    commandInsert.Parameters.Add(new FbParameter("distrid", districtId));
        //                }

        //                commandInsert.Transaction = transaction;
        //                commandInsert.ExecuteNonQuery();
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Info(ex.Message);
        //                throw;
        //            }
        //        }

        //        // Get the ID of object (which is created automatically via Trigger)
        //        query = "SELECT MAX(OBJECT_KOD) FROM OBJECT";

        //        using (FbCommand cmd = new FbCommand(query, connectionNJF))
        //        {
        //            try
        //            {
        //                cmd.Transaction = transaction;

        //                using (FbDataReader r = cmd.ExecuteReader())
        //                {
        //                    if (r.Read())
        //                    {
        //                        objectId = r.GetInt32(0);
        //                    }

        //                    r.Close();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Info(ex.Message);
        //                throw;
        //            }
        //        }
        //    }

        //    return objectId;
        //}

        //public static int CreateNew1NFObject(Preferences preferences, int streetId, string streetName, int districtId,
        //    string number1, string number2, string number3, string addrMisc)
        //{
        //    int newBuildingId = -1;

        //    number1 = number1.Trim().ToUpper();
        //    number2 = number2.Trim().ToUpper();
        //    number3 = number3.Trim().ToUpper();
        //    addrMisc = addrMisc.Trim().ToUpper();

        //    ObjectFinder.PreProcessBuildingNumbers(ref number1, ref number2);

        //    // Verify parameters
        //    if ((number1 + number2 + number3 + addrMisc).Length == 0)
        //    {
        //        throw new ArgumentException("Необхідно заповнити номер будинку.");
        //    }

        //    if (streetId < 0)
        //    {
        //        throw new ArgumentException("Необхідно вибрати вулицю.");
        //    }

        //    using (FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString()))
        //    {
        //        try
        //        {
        //            connection1NF.Open();
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
        //                System.Windows.Forms.MessageBoxButtons.OK,
        //                System.Windows.Forms.MessageBoxIcon.Error);

        //            return -1;
        //        }

        //        // Make all changes to the Firebird database in a transaction
        //        FbTransaction transaction = null;

        //        try
        //        {
        //            transaction = connection1NF.BeginTransaction();

        //            // Generate new building Id
        //            newBuildingId = GenerateNewFirebirdId(connection1NF, "OBJECT_1NF_GEN", transaction);

        //            Dictionary<string, object> parameters = new Dictionary<string, object>();

        //            if (newBuildingId < 0)
        //            {
        //                throw new InvalidOperationException("Помилка при створенні ідентифікатора нового будинку в базі 1НФ.");
        //            }

        //            // Get the street state from 1NF
        //            /*
        //            int streetState = -1;

        //            using (FbCommand cmdTest = new FbCommand("SELECT STAN, NAME FROM SUL WHERE KOD = @strid", connection1NF))
        //            {
        //                cmdTest.Transaction = transaction;
        //                cmdTest.Parameters.Add(new FbParameter("strid", streetId));

        //                using (FbDataReader r = cmdTest.ExecuteReader())
        //                {
        //                    while (r.Read())
        //                    {
        //                        int stan = r.IsDBNull(0) ? -1 : r.GetInt32(0);
        //                        string name = r.IsDBNull(1) ? "" : r.GetString(1).Trim().ToUpper();

        //                        if (streetName == name)
        //                        {
        //                            streetState = stan;
        //                            break;
        //                        }
        //                    }

        //                    r.Close();
        //                }
        //            }
        //            */

        //            streetName = streetName.Trim().ToUpper();

        //            if (streetName.Length > 100)
        //                streetName = streetName.Substring(0, 100);

        //            // Prepare the INSERT statement
        //            string fieldList = "OBJECT_KOD, OBJECT_KODSTAN, ISP, EIS_MODIFIED_BY, DT, STAN_YEAR, REALSTAN, ULNAME, FULL_ULNAME, DELETED, KORPUS";
        //            string paramList = "@oid, 1, @isp, @isp, @dt, @syear, 1, @sname, @sname, 0, 0";

        //            AddQueryParam("scod", "ULKOD", streetId, ref fieldList, ref paramList, parameters, -1);
        //            AddQueryParam("num1", "NOMER1", number1, ref fieldList, ref paramList, parameters, 9);
        //            AddQueryParam("num2", "NOMER2", number2, ref fieldList, ref paramList, parameters, 18);
        //            AddQueryParam("num3", "NOMER3", number3, ref fieldList, ref paramList, parameters, 10);
        //            AddQueryParam("amsc", "ADRDOP", addrMisc, ref fieldList, ref paramList, parameters, 100);
        //            AddQueryParam("distr", "NEWDISTR", districtId, ref fieldList, ref paramList, parameters, -1);

        //            string query = "INSERT INTO OBJECT_1NF (" + fieldList + ") VALUES (" + paramList + ")";

        //            using (FbCommand commandInsert = new FbCommand(query, connection1NF))
        //            {
        //                try
        //                {
        //                    commandInsert.Parameters.Add(new FbParameter("oid", newBuildingId));
        //                    commandInsert.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
        //                    commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
        //                    commandInsert.Parameters.Add(new FbParameter("syear", DateTime.Now.Year));
        //                    commandInsert.Parameters.Add(new FbParameter("sname", streetName));

        //                    foreach (KeyValuePair<string, object> pair in parameters)
        //                    {
        //                        commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
        //                    }

        //                    commandInsert.Transaction = transaction;
        //                    commandInsert.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    log.Info(ex.Message);
        //                    throw;
        //                }
        //            }

        //            // Commit the transaction
        //            transaction.Commit();
        //            transaction = null; // This will prevent an undesired Rollback() in the catch{} section

        //            // Save the created building to our local cache
        //            Object1NF obj = new Object1NF();

        //            obj.objectId = newBuildingId;
        //            obj.streetName = FindNameInDictionary1NF(DICT_STREETS, streetId);
        //            obj.addrNomer1 = number1;
        //            obj.addrNomer2 = number2;
        //            obj.addrNomer3 = number3;
        //            obj.addrMisc = addrMisc;
        //            obj.techStateId = null;
        //            obj.buildYear = null;
        //            obj.districtId = districtId > 0 ? (object)districtId : null;
        //            obj.objTypeId = null;
        //            obj.objKindId = null;
        //            obj.streetId = streetId;
        //            obj.totalSqr = null;

        //            objects1NF.Add(newBuildingId, obj);
        //        }
        //        catch (Exception ex)
        //        {
        //            newBuildingId = -1;

        //            // Roll back the transaction
        //            if (transaction != null)
        //            {
        //                transaction.Rollback();
        //            }

        //            System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
        //                System.Windows.Forms.MessageBoxButtons.OK,
        //                System.Windows.Forms.MessageBoxIcon.Error);
        //        }

        //        connection1NF.Close();
        //    }

        //    return newBuildingId;
        //}

        private static void AddQueryParam(string paramName, string fieldName, object value,
            ref string fieldList, ref string paramList, Dictionary<string, object> parameters, int textLimit)
        {
            bool valueIsValid = (value != null);

            if (value is int)
            {
                valueIsValid = ((int)value >= 0);
            }
            else if (value is string)
            {
                string s = (string)value;

                s = s.Trim();

                if (textLimit > 0 && s.Length > textLimit)
                {
                    s = s.Substring(0, textLimit);
                }

                value = s;
                valueIsValid = s.Length > 0;
            }

            if (valueIsValid)
            {
                fieldList += ", " + fieldName;
                paramList += ", @" + paramName;
                parameters.Add(paramName, value);
            }
        }

        //public static int CreateNewNJFOrganization(FbConnection connection, FbTransaction transaction,
        //    string zkpo, string fullName, string shortName,
        //    int? districtId, int streetId, string nomer1, string nomer2, string addrMisc, int? flat, string zipCode,
        //    int industryId, int ownershipId, int formGospId, int orgTypeId, int orgStatusId,
        //    string directorFIO, string directorTel, string buhgalterFIO, string buhgalterTel, string fax)
        //{
        //    // Generate the new organization code
        //    int newNjfOrgId = GenerateNewFirebirdId(connection, "ZKPO_GEN", transaction);

        //    if (newNjfOrgId > 0)
        //    {
        //        string fieldList = "KOD, STAN, ZKPO, NAMEP, NAME, ISACTIVE, ISP, DTISP, DT_BEG, POSTIND, NOMER1, NOMER2, ADRDOP, FIO, TEL, FAX, FIOB, TELB";
        //        string paramList = "@kod, 0, @zkpo, @namep, @name, 1, @usr, @mdt, @mdt, @postal, @nom1, @nom2, @adrdop, @dirf, @dirt, @fax, @buhf, @buht";

        //        if (districtId.HasValue)
        //        {
        //            fieldList += ", RA";
        //            paramList += ", @distr";
        //        }

        //        if (flat.HasValue)
        //        {
        //            fieldList += ", KVARTIRA";
        //            paramList += ", @kva";
        //        }

        //        if (orgStatusId > 0)
        //        {
        //            fieldList += ", TYPE_ORG";
        //            paramList += ", @orgt";
        //        }

        //        if (industryId > 0)
        //        {
        //            fieldList += ", OT";
        //            paramList += ", @ind";
        //        }

        //        if (formGospId > 0)
        //        {
        //            fieldList += ", FG";
        //            paramList += ", @fgosp";
        //        }

        //        if (ownershipId > 0)
        //        {
        //            fieldList += ", FORMVL";
        //            paramList += ", @fvl";
        //        }

        //        if (orgTypeId > 0)
        //        {
        //            fieldList += ", PV";
        //            paramList += ", @orgform";
        //        }

        //        if (streetId > 0)
        //        {
        //            fieldList += ", UL";
        //            paramList += ", @strid";
        //        }

        //        string query = "INSERT INTO SZKPO (" + fieldList + ") VALUES (" + paramList + ")";

        //        using (FbCommand command = connection.CreateCommand())
        //        {
        //            try
        //            {
        //                command.Transaction = transaction;
        //                command.CommandType = System.Data.CommandType.Text;
        //                command.CommandTimeout = 600;
        //                command.CommandText = query;

        //                command.Parameters.Add(new FbParameter("kod", newNjfOrgId));
        //                command.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 10 ? zkpo.Substring(0, 10) : zkpo));
        //                command.Parameters.Add(new FbParameter("namep", fullName.Length > 160 ? fullName.Substring(0, 160) : fullName));
        //                command.Parameters.Add(new FbParameter("name", shortName.Length > 160 ? shortName.Substring(0, 160) : shortName));
        //                command.Parameters.Add(new FbParameter("usr", UserName.Length > 20 ? UserName.Substring(0, 20) : UserName));
        //                command.Parameters.Add(new FbParameter("mdt", DateTime.Now.Date));

        //                command.Parameters.Add(new FbParameter("postal", zipCode.Length > 15 ? zipCode.Substring(0, 15) : zipCode));
        //                command.Parameters.Add(new FbParameter("nom1", nomer1.Length > 4 ? nomer1.Substring(0, 4) : nomer1));
        //                command.Parameters.Add(new FbParameter("nom2", nomer2.Length > 4 ? nomer2.Substring(0, 4) : nomer2));
        //                command.Parameters.Add(new FbParameter("adrdop", addrMisc.Length > 160 ? addrMisc.Substring(0, 160) : addrMisc));

        //                command.Parameters.Add(new FbParameter("dirf", directorFIO.Length > 160 ? directorFIO.Substring(0, 160) : directorFIO));
        //                command.Parameters.Add(new FbParameter("dirt", directorTel.Length > 15 ? directorTel.Substring(0, 15) : directorTel));
        //                command.Parameters.Add(new FbParameter("fax", fax.Length > 15 ? fax.Substring(0, 15) : fax));
        //                command.Parameters.Add(new FbParameter("buhf", buhgalterFIO.Length > 160 ? buhgalterFIO.Substring(0, 160) : buhgalterFIO));
        //                command.Parameters.Add(new FbParameter("buht", buhgalterTel.Length > 18 ? buhgalterTel.Substring(0, 18) : buhgalterTel));

        //                if (orgStatusId > 0)
        //                {
        //                    command.Parameters.Add(new FbParameter("orgt", orgStatusId));
        //                }

        //                if (industryId > 0)
        //                {
        //                    command.Parameters.Add(new FbParameter("ind", industryId));
        //                }

        //                if (formGospId > 0)
        //                {
        //                    command.Parameters.Add(new FbParameter("fgosp", formGospId));
        //                }

        //                if (ownershipId > 0)
        //                {
        //                    command.Parameters.Add(new FbParameter("fvl", ownershipId));
        //                }

        //                if (orgTypeId > 0)
        //                {
        //                    command.Parameters.Add(new FbParameter("orgform", orgTypeId));
        //                }

        //                if (districtId.HasValue)
        //                {
        //                    command.Parameters.Add(new FbParameter("distr", districtId.Value));
        //                }

        //                if (flat.HasValue)
        //                {
        //                    command.Parameters.Add(new FbParameter("kva", flat.Value));
        //                }

        //                if (streetId > 0)
        //                {
        //                    command.Parameters.Add(new FbParameter("strid", streetId));
        //                }

        //                command.ExecuteNonQuery();
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Info(ex.Message);
        //                throw;
        //            }
        //        }
        //    }

        //    return newNjfOrgId;
        //}

        //public static int CreateNew1NFOrganization(Preferences preferences,
        //    string zkpo, string fullName, string shortName,
        //    int? districtId, string street, string nomer, string korpus, string addrMisc, string zipCode,
        //    int industryId, int occupationId, int ownershipId, int formGospId, int orgTypeId, int orgStatusId,
        //    string directorFIO, string directorTel, string buhgalterFIO, string buhgalterTel, string fax)
        //{
        //    int newOrgId = -1;

        //    fullName = fullName.Trim().ToUpper();
        //    shortName = shortName.Trim().ToUpper();
        //    zkpo = zkpo.Trim();

        //    nomer = nomer.Trim().ToUpper();
        //    korpus = korpus.Trim().ToUpper();
        //    addrMisc = addrMisc.Trim().ToUpper();
        //    zipCode = zipCode.Trim().ToUpper();

        //    directorFIO = directorFIO.Trim().ToUpper();
        //    directorTel = directorTel.Trim().ToUpper();
        //    buhgalterFIO = buhgalterFIO.Trim().ToUpper();
        //    buhgalterTel = buhgalterTel.Trim().ToUpper();
        //    fax = fax.Trim().ToUpper();

        //    // Verify parameters
        //    if (fullName.Length == 0)
        //    {
        //        throw new ArgumentException("Необхідно заповнити повну назву організації.");
        //    }

        //    if (zkpo.Length == 0)
        //    {
        //        throw new ArgumentException("Необхідно заповнити Код ЄДРПОУ.");
        //    }

        //    using (FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString()))
        //    {
        //        try
        //        {
        //            connection1NF.Open();
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
        //                System.Windows.Forms.MessageBoxButtons.OK,
        //                System.Windows.Forms.MessageBoxIcon.Error);

        //            return -1;
        //        }

        //        // Make all changes to the Firebird database in a transaction
        //        FbTransaction transaction = null;

        //        try
        //        {
        //            transaction = connection1NF.BeginTransaction();

        //            // Check if ZKPO code already exists
        //            using (FbCommand cmdTest = new FbCommand("SELECT FIRST 1 KOD_OBJ FROM SORG_1NF WHERE KOD_ZKPO = @zkpo", connection1NF))
        //            {
        //                cmdTest.Transaction = transaction;
        //                cmdTest.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 14 ? zkpo.Substring(0, 14) : zkpo));

        //                using (FbDataReader r = cmdTest.ExecuteReader())
        //                {
        //                    if (r.Read())
        //                    {
        //                        throw new InvalidOperationException("В базі 1НФ вже існує організація з таким кодом ЄДРПОУ.");
        //                    }

        //                    r.Close();
        //                }
        //            }

        //            // Generate new organization Id
        //            newOrgId = Get1NFNewOrganizationId(connection1NF, transaction);

        //            Dictionary<string, object> parameters = new Dictionary<string, object>();

        //            if (newOrgId < 0)
        //            {
        //                throw new InvalidOperationException("Помилка при створенні ідентифікатора нової організації в базі 1НФ.");
        //            }

        //            // Prepare the INSERT statement
        //            string fieldList = "KOD_OBJ, KOD_STAN, LAST_SOST, ISP, EIS_MODIFIED_BY, DT, USER_KOREG, DATE_KOREG, DELETED, KOD_ZKPO, FULL_NAME_OBJ, SHORT_NAME_OBJ";
        //            string paramList = "@orgd, 1, 1, @isp, @isp, @dt, @isp, @dt, 0, @zkpo, @fname, @sname";

        //            AddQueryParam("gal", "KOD_GALUZ", industryId, ref fieldList, ref paramList, parameters, -1);
        //            AddQueryParam("viddial", "KOD_VID_DIAL", occupationId, ref fieldList, ref paramList, parameters, -1);
        //            AddQueryParam("fvl", "KOD_FORM_VLASN", ownershipId, ref fieldList, ref paramList, parameters, -1);
        //            AddQueryParam("sta", "KOD_STATUS", orgStatusId, ref fieldList, ref paramList, parameters, -1);
        //            AddQueryParam("gosp", "KOD_FORM_GOSP", formGospId, ref fieldList, ref paramList, parameters, -1);
        //            AddQueryParam("orgf", "KOD_ORG_FORM", orgTypeId, ref fieldList, ref paramList, parameters, -1);
        //            AddQueryParam("dirfio", "FIO_BOSS", directorFIO, ref fieldList, ref paramList, parameters, 70);
        //            AddQueryParam("dirtel", "TEL_BOSS", directorTel, ref fieldList, ref paramList, parameters, 23);
        //            AddQueryParam("buhfio", "FIO_BUH", buhgalterFIO, ref fieldList, ref paramList, parameters, 70);
        //            AddQueryParam("buhtel", "TEL_BUH", buhgalterTel, ref fieldList, ref paramList, parameters, 23);
        //            AddQueryParam("distr", "KOD_RAYON2", districtId.HasValue ? districtId.Value : -1, ref fieldList, ref paramList, parameters, -1);
        //            AddQueryParam("strt", "NAME_UL", street, ref fieldList, ref paramList, parameters, 100);
        //            AddQueryParam("nom", "NOMER_DOMA", nomer, ref fieldList, ref paramList, parameters, 30);
        //            AddQueryParam("korp", "NOMER_KORPUS", korpus, ref fieldList, ref paramList, parameters, 20);
        //            AddQueryParam("zcod", "POST_INDEX", zipCode, ref fieldList, ref paramList, parameters, 18);

        //            string query = "INSERT INTO SORG_1NF (" + fieldList + ") VALUES (" + paramList + ")";

        //            using (FbCommand commandInsert = new FbCommand(query, connection1NF))
        //            {
        //                try
        //                {
        //                    commandInsert.Parameters.Add(new FbParameter("orgd", newOrgId));
        //                    commandInsert.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
        //                    commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
        //                    commandInsert.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 14 ? zkpo.Substring(0, 14) : zkpo));
        //                    commandInsert.Parameters.Add(new FbParameter("fname", fullName.Length > 252 ? fullName.Substring(0, 252) : fullName));
        //                    commandInsert.Parameters.Add(new FbParameter("sname", shortName.Length > 100 ? shortName.Substring(0, 100) : shortName));

        //                    foreach (KeyValuePair<string, object> pair in parameters)
        //                    {
        //                        commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
        //                    }

        //                    commandInsert.Transaction = transaction;
        //                    commandInsert.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    log.Info(ex.Message);
        //                    throw;
        //                }
        //            }

        //            // Commit the transaction
        //            transaction.Commit();
        //            transaction = null; // This will prevent an undesired Rollback() in the catch{} section

        //            // Save the created organization in our local cache
        //            Organization1NF org = new Organization1NF();

        //            org.organizationId = newOrgId;
        //            org.zkpo = zkpo;
        //            org.fullName = fullName;
        //            org.shortName = shortName;
        //            org.industryId = industryId;
        //            org.occupationId = occupationId;
        //            org.statusId = orgStatusId;
        //            org.orgTypeId = orgTypeId;
        //            org.formGospId = formGospId;
        //            org.ownershipFormId = ownershipId;
        //            org.addrStreet = street;
        //            org.addrNomer = nomer;
        //            org.addrKorpus = korpus;
        //            org.addrZipCode = zipCode;
        //            org.addrDistrictId = districtId.HasValue ? districtId.Value : -1;
        //            org.directorFIO = directorFIO;
        //            org.directorTel = directorTel;
        //            org.buhgalterFIO = buhgalterFIO;
        //            org.buhgalterTel = buhgalterTel;
        //            org.fax = fax;

        //            organizations1NF.Add(newOrgId, org);
        //        }
        //        catch (Exception ex)
        //        {
        //            newOrgId = -1;

        //            // Roll back the transaction
        //            if (transaction != null)
        //            {
        //                transaction.Rollback();
        //            }

        //            System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
        //                System.Windows.Forms.MessageBoxButtons.OK,
        //                System.Windows.Forms.MessageBoxIcon.Error);
        //        }

        //        connection1NF.Close();
        //    }

        //    return newOrgId;
        //}

//        public static List<ActObject> GetDocObjects(Preferences preferences, int documentId)
//        {
//            List<ActObject> list = new List<ActObject>();

//            // Test connections
//            FbConnection connectionNJF = new FbConnection(preferences.GetNJFConnectionString(UserName, UserPassword));

//            try
//            {
//                connectionNJF.Open();
//                connectionNJF.Close();
//            }
//            catch (Exception ex)
//            {
//                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних 'Розпорядження': " + ex.Message,
//                    System.Windows.Forms.MessageBoxButtons.OK,
//                    System.Windows.Forms.MessageBoxIcon.Error);

//                return list;
//            }

//            FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString());

//            try
//            {
//                connection1NF.Open();
//                connection1NF.Close();
//            }
//            catch (Exception ex)
//            {
//                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
//                    System.Windows.Forms.MessageBoxButtons.OK,
//                    System.Windows.Forms.MessageBoxIcon.Error);

//                return list;
//            }

//            try
//            {
//                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

//                connectionNJF.Open();
//                connection1NF.Open();

//                // Get all records from OBJECT_DOKS_PROPERTIES table related to this document
//                using (FbCommand command = connectionNJF.CreateCommand())
//                {
//                    try
//                    {
//                        command.CommandType = System.Data.CommandType.Text;
//                        command.CommandTimeout = 600;
//                        command.CommandText = @"SELECT 
//                            odp.ID, odp.OBJECT_KOD, odp.NAME, odp.SQUARE, obj.ULNAME, obj.NOMER1, obj.NOMER2, obj.NOMER3, obj.ADRDOP, obj.NEWDISTR, 
//                            odp.GRPURP, odp.PURPOSE, odp.OBJKIND, odp.OBJTYPE, odp.SUMMA_BALANS, odp.SUMMA_ZAL, odp.FLOORS,
//                            odp.CHARACTERISTIC, odp.YEAR_BUILD, odp.YEAR_EXPL, odp.TEXSTAN, odp.LEN, odp.DIAM_TRUB
//                            FROM OBJECT_DOKS_PROPERTIES odp INNER JOIN OBJECT obj ON obj.OBJECT_KOD = odp.OBJECT_KOD AND obj.OBJECT_KODSTAN = 1
//                            WHERE odp.DOK_ID = @docid";

//                        command.Parameters.Add(new FbParameter("docid", documentId));

//                        using (FbDataReader reader = command.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                if (!reader.IsDBNull(0))
//                                {
//                                    ActObject actObj = new ActObject();

//                                    actObj.objectDocsPropertiesId_NJF = reader.GetInt32(0);
//                                    actObj.objectId_NJF = reader.GetInt32(1);
//                                    actObj.objectName_NJF = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
//                                    actObj.objectSquare_NJF = reader.IsDBNull(3) ? null : reader.GetValue(3);

//                                    actObj.addrStreet_NJF = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim().ToUpper();
//                                    actObj.addrNomer1_NJF = reader.IsDBNull(5) ? "" : reader.GetString(5).Trim().ToUpper();
//                                    actObj.addrNomer2_NJF = reader.IsDBNull(6) ? "" : reader.GetString(6).Trim().ToUpper();
//                                    actObj.addrNomer3_NJF = reader.IsDBNull(7) ? "" : reader.GetString(7).Trim().ToUpper();
//                                    actObj.addrMisc_NJF = reader.IsDBNull(8) ? "" : reader.GetString(8).Trim().ToUpper();
//                                    actObj.districtId_NJF = reader.IsDBNull(9) ? null : reader.GetValue(9);

//                                    actObj.purposeGroupIdNJF = reader.IsDBNull(10) ? -1 : reader.GetInt32(10);
//                                    actObj.purposeIdNJF = reader.IsDBNull(11) ? -1 : reader.GetInt32(11);
//                                    actObj.objectKindIdNJF = reader.IsDBNull(12) ? -1 : reader.GetInt32(12);
//                                    actObj.objectTypeIdNJF = reader.IsDBNull(13) ? -1 : reader.GetInt32(13);

//                                    actObj.objectBalansCost_NJF = reader.IsDBNull(14) ? null : reader.GetValue(14);
//                                    actObj.objectFinalCost_NJF = reader.IsDBNull(15) ? null : reader.GetValue(15);

//                                    actObj.objectFloorsInt_NJF = reader.IsDBNull(16) ? -1 : reader.GetInt32(16);

//                                    if (actObj.objectFloorsInt_NJF >= 0)
//                                        actObj.objectFloorsStr_NJF = actObj.objectFloorsInt_NJF.ToString();

//                                    actObj.characteristicNJF = reader.IsDBNull(17) ? null : reader.GetValue(17);
//                                    actObj.yearBuildNJF = reader.IsDBNull(18) ? null : reader.GetValue(18);
//                                    actObj.yearExplNJF = reader.IsDBNull(19) ? null : reader.GetValue(19);
//                                    actObj.techStateIdNJF = reader.IsDBNull(20) ? null : reader.GetValue(20);
//                                    actObj.objectLen_NJF = reader.IsDBNull(21) ? null : reader.GetValue(21);
//                                    actObj.objectDiamTrub_NJF = reader.IsDBNull(22) ? null : reader.GetValue(22);

//                                    list.Add(actObj);
//                                }
//                            }

//                            reader.Close();
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        log.Info(ex.Message);
//                        throw;
//                    }
//                }

//                foreach (ActObject actObj in list)
//                {
//                    actObj.DeduceObjectTypeFor1NF();
//                }

//                // Build transfers for each Act Object
//                foreach (ActObject actObj in list)
//                {
//                    string query = @"SELECT FROM_ORG, TOORG, PRAVO FROM PRAVA_PROECT WHERE RD = @rdoc AND PRAVO IN (2, 3, 6, 7, 8, 9)";

//                    using (FbCommand cmd = new FbCommand(query, connectionNJF))
//                    {
//                        try
//                        {
//                            cmd.Parameters.Add(new FbParameter("rdoc", actObj.objectDocsPropertiesId_NJF));

//                            using (FbDataReader reader = cmd.ExecuteReader())
//                            {
//                                while (reader.Read())
//                                {
//                                    int organizationFromId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
//                                    int organizationToId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
//                                    int rightId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);

//                                    if (organizationFromId > 0 || organizationToId > 0)
//                                    {
//                                        BalansTransfer balTransfer = new BalansTransfer();

//                                        balTransfer.objectId_NJF = actObj.objectId_NJF;
//                                        balTransfer.objectId_1NF = actObj.objectId_1NF;
//                                        balTransfer.organizationFromId_NJF = organizationFromId;
//                                        balTransfer.organizationToId_NJF = organizationToId;

//                                        Organization1NF org = null;

//                                        if (organizationFromId > 0 && organizationsNJF.TryGetValue(organizationFromId, out org))
//                                        {
//                                            balTransfer.orgFromZkpo_NJF = org.zkpo;
//                                            balTransfer.orgFromFullName_NJF = org.fullName;
//                                            balTransfer.orgFromShortName_NJF = org.shortName;
//                                        }

//                                        if (organizationToId > 0 && organizationsNJF.TryGetValue(organizationToId, out org))
//                                        {
//                                            balTransfer.orgToZkpo_NJF = org.zkpo;
//                                            balTransfer.orgToFullName_NJF = org.fullName;
//                                            balTransfer.orgToShortName_NJF = org.shortName;
//                                        }

//                                        if (actObj.objectSquare_NJF is decimal)
//                                        {
//                                            balTransfer.sqr = (decimal)actObj.objectSquare_NJF;
//                                        }

//                                        // Deduce the type of balans transfer
//                                        if (rightId == 7) // Znesennya
//                                        {
//                                            balTransfer.transferType = ObjectTransferType.Destroy;
//                                        }
//                                        else if (organizationToId < 0)
//                                        {
//                                            balTransfer.transferType = ObjectTransferType.Destroy;
//                                        }
//                                        else if (organizationFromId < 0)
//                                        {
//                                            balTransfer.transferType = ObjectTransferType.Create;
//                                        }

//                                        actObj.balansTransfers.Add(balTransfer);
//                                    }
//                                }

//                                reader.Close();
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            log.Info(ex.Message);
//                            throw;
//                        }
//                    }
//                }

//                connection1NF.Close();
//                connectionNJF.Close();
//            }
//            catch (Exception ex)
//            {
//                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних: " + ex.Message,
//                    System.Windows.Forms.MessageBoxButtons.OK,
//                    System.Windows.Forms.MessageBoxIcon.Error);
//            }

//            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

//            return list;
//        }

        public static int FindOrganizationByZKPO(string zkpo)
        {
            zkpo = zkpo.Trim().ToUpper();

            foreach (KeyValuePair<int, Organization1NF> pair in organizations1NF)
            {
                if (pair.Value.zkpo == zkpo)
                {
                    return pair.Key;
                }
            }

            return -1;
        }

        #endregion (Interface)

        #region Document export

        ///test!!!
        //public static int ExportDocument(Preferences preferences, ImportedDoc doc)
        //{
        //    var docId = -1;
        //    //manualStreetCodeMappingNJFto1NF = ObjectFinder.GetStreetCodeMatchNJFto1NF();



        //    FbConnection connectionNJF = new FbConnection(preferences.GetNJFConnectionString(UserName, UserPassword));
        //    try
        //    {
        //        //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

        //        connectionNJF.Open();

        //        // Initialize object and organization finders
        //        ObjectFinder objectFinder = new ObjectFinder();
        //        OrganizationFinder orgFinder = new OrganizationFinder();

        //        objectFinder.BuildObjectCacheFromNJF(connectionNJF);
        //        orgFinder.BuildOrganizationCacheFromNJF(connectionNJF, false);

        //        // Make all changes to the Firebird database in a transaction
        //        FbTransaction transaction = null;

        //        try
        //        {
        //            transaction = connectionNJF.BeginTransaction();

        //            // Create the document
        //            docId = CreateDocumentNJF(connectionNJF, transaction,
        //                doc.docTypeId, doc.docNum, doc.docTitle, doc.docDate, doc.docSum, doc.docFinalSum);


        //            if (docId > 0)
        //            {
        //                // Create relation to the main document
        //                AddRelationToPrimaryDocNJF(connectionNJF, transaction, docId, doc.masterDocNum, doc.masterDocDate);

        //                // Create relations to all objects
        //                ExportDocObjects(/*connectionNJF, transaction,*/ doc, docId, objectFinder, orgFinder);

        //                // Save the created document in our local data structures
        //                // RegisterNJFDocument(docId, doc);
        //            }

        //            // Commit the transaction
        //            transaction.Commit();

        //            // Tell user that everything is OK
        //            // write to log "Завантаження даних до бази 'Розпорядження' завершено успішно.",                        
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Info(ex.Message);
        //            // Roll back the transaction
        //            if (transaction != null)
        //            {
        //                transaction.Rollback();
        //            }
        //        }

        //        connectionNJF.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info("Помилка доступу до бази даних 'Розпорядження'" + ex.Message);
        //    }

        //    return docId;
        //}

        //public static int CreateDocumentNJF(FbConnection connectionNJF, FbTransaction transaction,
        //    int docTypeId, string docNum, string docTitle, DateTime docDate, decimal docSum, decimal docFinalSum)
        //{
        //    // Generate a new ID for the document
        //    int documentId = GenerateNewFirebirdId(connectionNJF, "ROZP_DOK_GEN", transaction);

        //    // Prepare the INSERT statement
        //    string query = "";

        //    if (docTypeId == 3) // Act
        //    {
        //        query = "INSERT INTO ACTS (DOK_ID, DOKKIND, PIDROZDIL, STATUS, DOKDATA, DOKNUM, DOKTEMA, PRIZNAK, DT, ISP, DT_INSERT, ISP_INSERT, SUMMA, SUMMA_ZAL)" +
        //            " VALUES (@docid, @doctype, 1, 2, @docdt, @docnum, @topic, 1, @edt, @usr, @edt, @usr, @sumtot, @sumfin)";
        //    }
        //    else
        //    {
        //        query = "INSERT INTO ROZP_DOK (DOK_ID, DOKKIND, PIDROZDIL, STATUS, DOKDATA, DOKNUM, DOKTEMA, PRIZNAK, DT, ISP, DT_INSERT, ISP_INSERT, SUMMA, SUMMA_ZAL)" +
        //            " VALUES (@docid, @doctype, 1, 2, @docdt, @docnum, @topic, 1, @edt, @usr, @edt, @usr, @sumtot, @sumfin)";
        //    }

        //    using (FbCommand commandInsert = new FbCommand(query, connectionNJF))
        //    {
        //        try
        //        {
        //            commandInsert.Parameters.Add(new FbParameter("docid", documentId));
        //            commandInsert.Parameters.Add(new FbParameter("doctype", docTypeId));
        //            commandInsert.Parameters.Add(new FbParameter("docdt", docDate.Date));
        //            commandInsert.Parameters.Add(new FbParameter("docnum", (docNum.Length > 20) ? docNum.Substring(0, 20) : docNum));
        //            commandInsert.Parameters.Add(new FbParameter("topic", (docTitle.Length > 255) ? docTitle.Substring(0, 255) : docTitle));
        //            commandInsert.Parameters.Add(new FbParameter("sumtot", docSum));
        //            commandInsert.Parameters.Add(new FbParameter("sumfin", docFinalSum));
        //            commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
        //            commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));

        //            commandInsert.Transaction = transaction;
        //            commandInsert.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Info(ex.Message);
        //            throw;
        //        }
        //    }

        //    return documentId;
        //}

        //private static void AddRelationToPrimaryDocNJF(FbConnection connectionNJF, FbTransaction transaction,
        //    int documentId, string masterDocNum, DateTime masterDocDate)
        //{
        //    if (masterDocNum.Length > 0)
        //    {
        //        int parentDocId = DB.FindDocument(masterDocNum, masterDocDate.Date, -1);

        //        if (parentDocId >= 0)
        //        {
        //            string query = "INSERT INTO DOK_DEPEND (DOK_KOD1, DOK_KOD2, KIND, DT, ISP) VALUES (@chi, @pare, 1, @edt, @usr)";

        //            using (FbCommand commandInsert = new FbCommand(query, connectionNJF))
        //            {
        //                try
        //                {
        //                    commandInsert.Parameters.Add(new FbParameter("chi", documentId));
        //                    commandInsert.Parameters.Add(new FbParameter("pare", parentDocId));
        //                    commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
        //                    commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));

        //                    commandInsert.Transaction = transaction;
        //                    commandInsert.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    log.Info(ex.Message);
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //}

        //private static void ExportDocObjects(/*FbConnection connection, FbTransaction transaction, */ImportedDoc doc, int documentId,
        //    ObjectFinder objectFinder, OrganizationFinder orgFinder)
        //{
        //    foreach (Appendix appendix in doc.appendices)
        //    {
        //        foreach (AppendixObject obj in appendix.objects)
        //        {
        //            //FbConnection connection1NF = Utils.ConnectTo1NF();
        //            LoadOneBalansObjectFrom1NF(connection1NF, obj.object1NFId);
        //            LoadOrganizationsFrom1NF(connection1NF);
        //            connection1NF.Close();

        //            if (obj.object1NF != null)
        //            {
        //                int matchObjectId = FindObjectMatchInNJF(/*connection, transaction,*/ obj.object1NF, objectFinder);

        //                // Generate a new object-doc relation ID
        //                int relationId = GenerateNewFirebirdId(connection, "OBJECT_DOKS_GEN", transaction);

        //                // Insert the relation into OBJECT_DOKS table
        //                string query = "INSERT INTO OBJECT_DOKS (ID, DOK_ID, OBJECT_KOD, OBJECT_KODSTAN, DTISP, ISP) VALUES (@relid, @docid, @objid, 0, @edt, @usr)";

        //                using (FbCommand commandInsert = new FbCommand(query, connection))
        //                {
        //                    try
        //                    {
        //                        commandInsert.Parameters.Add(new FbParameter("relid", relationId));
        //                        commandInsert.Parameters.Add(new FbParameter("docid", documentId));
        //                        commandInsert.Parameters.Add(new FbParameter("objid", matchObjectId));
        //                        commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
        //                        commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

        //                        commandInsert.Transaction = transaction;
        //                        commandInsert.ExecuteNonQuery();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        log.Info(ex.Message);
        //                        throw;
        //                    }
        //                }

        //                // At this point a new record might be already added to the OBJECT_DOKS_PROPERTIES table via a trigger. Check if this is so
        //                bool objectDocPropertiesExists = false;

        //                using (FbCommand cmd = new FbCommand("SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE ID = @relid", connection))
        //                {
        //                    try
        //                    {
        //                        cmd.Parameters.Add(new FbParameter("relid", relationId));
        //                        cmd.Transaction = transaction;

        //                        using (FbDataReader r = cmd.ExecuteReader())
        //                        {
        //                            if (r.Read())
        //                            {
        //                                objectDocPropertiesExists = true;
        //                            }

        //                            r.Close();
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        log.Info(ex.Message);
        //                        throw;
        //                    }
        //                }

        //                Dictionary<string, object> parameters = new Dictionary<string, object>();

        //                parameters["@relid"] = relationId;
        //                parameters["@edt"] = DateTime.Now.Date;
        //                parameters["@usr"] = UserName.Length > 18 ? UserName.Substring(0, 18) : UserName;

        //                string fieldList = "";
        //                string paramList = "";

        //                if (objectDocPropertiesExists)
        //                {
        //                    // Prepare an UPDATE query for the OBJECT_DOKS_PROPERTIES table
        //                    fieldList = "TEXSTAN = 1, DT = @edt, ISP = @usr";

        //                }
        //                else
        //                {
        //                    // Prepare an INSERT query for the OBJECT_DOKS_PROPERTIES table
        //                    fieldList = "ID, OBJECT_KOD, OBJECT_KODSTAN, DOK_ID, TEXSTAN, DT, ISP";
        //                    paramList = "@relid, @objid, 0, @docid, 1, @edt, @usr";

        //                    parameters["@objid"] = matchObjectId;
        //                    parameters["@docid"] = documentId;

        //                    query = "INSERT INTO OBJECT_DOKS_PROPERTIES (" + fieldList + ") VALUES (" + paramList + ")";
        //                }

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "CHARACTERISTIC", "@char", obj,
        //                    ColumnCategory.Characteristics, FbDbType.VarChar, "", objectDocPropertiesExists, 100);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "GRPURP", "@purpgr", obj,
        //                    ColumnCategory.PurposeGroup, FbDbType.Integer, DICT_PURPOSE_GROUP, objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "PURPOSE", "@purp", obj,
        //                    ColumnCategory.Purpose, FbDbType.Integer, DICT_PURPOSE, objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "SQUARE", "@sqre", obj,
        //                    ColumnCategory.Square, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "OBJKIND", "@knd", obj,
        //                    ColumnCategory.ObjectKind, FbDbType.Integer, DICT_OBJ_KIND, objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "OBJTYPE", "@typ", obj,
        //                    ColumnCategory.ObjectType, FbDbType.Integer, DICT_OBJ_TYPE, objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "SUMMA_BALANS", "@sumbal", obj,
        //                    ColumnCategory.BalansCost, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "SUMMA_ZAL", "@sumzal", obj,
        //                    ColumnCategory.FinalCost, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "SUMMA_BALANS_0", "@sumbal0", obj,
        //                    ColumnCategory.BalansCost, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "SUMMA_ZAL_0", "@sumzal0", obj,
        //                    ColumnCategory.FinalCost, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "NAME", "@objname", obj,
        //                    ColumnCategory.ObjectName, FbDbType.VarChar, "", objectDocPropertiesExists, 255);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "YEAR_BUILD", "@yearb", obj,
        //                    ColumnCategory.BuildYear, FbDbType.Integer, "", objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "YEAR_EXPL", "@yearexp", obj,
        //                    ColumnCategory.ExplYear, FbDbType.Integer, "", objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "LEN", "@objlen", obj,
        //                    ColumnCategory.Length, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

        //                AddQueryParameter(parameters, ref fieldList, ref paramList, "DIAM_TRUB", "@diam", obj,
        //                    ColumnCategory.Diameter, FbDbType.VarChar, "", objectDocPropertiesExists, 20);

        //                if (objectDocPropertiesExists)
        //                {
        //                    query = "UPDATE OBJECT_DOKS_PROPERTIES SET " + fieldList + " WHERE ID = @relid";
        //                }
        //                else
        //                {
        //                    query = "INSERT INTO OBJECT_DOKS_PROPERTIES (" + fieldList + ") VALUES (" + paramList + ")";
        //                }

        //                using (FbCommand commandInsert = new FbCommand(query, connection))
        //                {
        //                    try
        //                    {
        //                        foreach (KeyValuePair<string, object> param in parameters)
        //                        {
        //                            if (param.Key.StartsWith("@"))
        //                            {
        //                                commandInsert.Parameters.Add(new FbParameter(param.Key.Substring(1), param.Value));
        //                            }
        //                            else
        //                            {
        //                                commandInsert.Parameters.Add(new FbParameter(param.Key, param.Value));
        //                            }
        //                        }

        //                        commandInsert.Transaction = transaction;
        //                        commandInsert.ExecuteNonQuery();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        log.Info(ex.Message);
        //                        throw;
        //                    }
        //                }

        //                // Export the object rights (for Acts this is not needed)
        //                if (!doc.IsAct())
        //                {
        //                    ExportObjectRights(/*connection, transaction,*/ doc, obj, matchObjectId, documentId, relationId, orgFinder);
        //                }
        //            }
        //        }
        //    }
        //}

        //private static void ExportObjectRights(/*FbConnection connection, FbTransaction transaction,*/ ImportedDoc doc,
        //    AppendixObject obj, int matchObjectId, int documentId, int relationId, OrganizationFinder orgFinder)
        //{
        //    foreach (Transfer t in obj.transfers)
        //    {
        //        if (t.rightId > 0)
        //        {
        //            // Perform matching of 1NF organizations to the NJF organizations
        //            int matchOrgFromId = -1;
        //            int matchOrgToId = -1;

        //            if (t.orgFrom != null)
        //                matchOrgFromId = FindOrganizationMatchInNJF(/*connection, transaction,*/ t.orgFrom, orgFinder);

        //            if (t.orgTo != null)
        //                matchOrgToId = FindOrganizationMatchInNJF(/*connection, transaction,*/ t.orgTo, orgFinder);

        //            // Add record to the PRAVA_PROECT table
        //            if (matchOrgFromId > 0 || matchOrgToId > 0)
        //            {
        //                int pravaProjectId = GenerateNewFirebirdId(connection, "GEN_PRAVA_PROECT_ID", transaction);

        //                if (pravaProjectId > 0)
        //                {
        //                    string query = "INSERT INTO PRAVA_PROECT (ID, RD, FROM_ORG, FROM_ORG_STAN, TOORG, TOORG_STAN, PRAVO, DT, ISP) VALUES" +
        //                        " (@ppid, @relid, <FROMORG>, <FROMORGSTAN>, <TOORG>, <TOORGSTAN>, @rightid, @edt, @usr)";

        //                    if (t.orgFrom != null)
        //                    {
        //                        query = query.Replace("<FROMORG>", "@fromid");
        //                        query = query.Replace("<FROMORGSTAN>", "0");
        //                    }
        //                    else
        //                    {
        //                        query = query.Replace("<FROMORG>", "NULL");
        //                        query = query.Replace("<FROMORGSTAN>", "NULL");
        //                    }

        //                    if (t.orgTo != null)
        //                    {
        //                        query = query.Replace("<TOORG>", "@toid");
        //                        query = query.Replace("<TOORGSTAN>", "0");
        //                    }
        //                    else
        //                    {
        //                        query = query.Replace("<TOORG>", "NULL");
        //                        query = query.Replace("<TOORGSTAN>", "NULL");
        //                    }

        //                    using (FbCommand commandInsert = new FbCommand(query, connection))
        //                    {
        //                        try
        //                        {
        //                            commandInsert.Parameters.Add(new FbParameter("ppid", pravaProjectId));
        //                            commandInsert.Parameters.Add(new FbParameter("relid", relationId));
        //                            commandInsert.Parameters.Add(new FbParameter("rightid", t.rightId));
        //                            commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
        //                            commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

        //                            if (matchOrgFromId > 0)
        //                            {
        //                                commandInsert.Parameters.Add(new FbParameter("fromid", matchOrgFromId));
        //                            }

        //                            if (matchOrgToId > 0)
        //                            {
        //                                commandInsert.Parameters.Add(new FbParameter("toid", matchOrgToId));
        //                            }

        //                            commandInsert.Transaction = transaction;
        //                            commandInsert.ExecuteNonQuery();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            log.Info(ex.Message);
        //                            throw;
        //                        }
        //                    }
        //                }
        //            }

        //            // Add records to the OBJECT_PRAVO table
        //            int fromRecordId = -1;
        //            int toRecordId = -1;

        //            if (matchOrgFromId > 0)
        //            {
        //                fromRecordId = GenerateNewFirebirdId(connection, "OBJECT_PRAVO_GEN", transaction);

        //                if (fromRecordId > 0)
        //                {
        //                    // Create the "from" entry in the OBJECT_PRAVO table
        //                    string query = "INSERT INTO OBJECT_PRAVO (ID, OBJECT_KOD, OBJECT_KODSTAN, ZKPO, ZKPO_STAN, PRAVO, END_DOK, END_DATE, STDOKOBJID, DT, ISP)" +
        //                        " VALUES (@fromid, @objid, 0, @orgid, 0, @rid, @docid, @docdt, @relid, @edt, @usr)";

        //                    using (FbCommand commandInsert = new FbCommand(query, connection))
        //                    {
        //                        try
        //                        {
        //                            commandInsert.Parameters.Add(new FbParameter("fromid", fromRecordId));
        //                            commandInsert.Parameters.Add(new FbParameter("objid", matchObjectId));
        //                            commandInsert.Parameters.Add(new FbParameter("orgid", matchOrgFromId));
        //                            commandInsert.Parameters.Add(new FbParameter("rid", t.rightId));
        //                            commandInsert.Parameters.Add(new FbParameter("docid", documentId));
        //                            commandInsert.Parameters.Add(new FbParameter("docdt", doc.docDate.Date));
        //                            commandInsert.Parameters.Add(new FbParameter("relid", relationId));
        //                            commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
        //                            commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

        //                            commandInsert.Transaction = transaction;
        //                            commandInsert.ExecuteNonQuery();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            log.Info(ex.Message);
        //                            throw;
        //                        }
        //                    }
        //                }
        //            }

        //            if (matchOrgToId > 0)
        //            {
        //                toRecordId = GenerateNewFirebirdId(connection, "OBJECT_PRAVO_GEN", transaction);

        //                if (toRecordId > 0)
        //                {
        //                    // Create the "to" entry in the OBJECT_PRAVO table
        //                    string query = "INSERT INTO OBJECT_PRAVO (ID, OBJECT_KOD, OBJECT_KODSTAN, ZKPO, ZKPO_STAN, PRAVO, START_DOK, START_DATE, STDOKOBJID, DT, ISP)" +
        //                        " VALUES (@toid, @objid, 0, @orgid, 0, @rid, @docid, @docdt, @relid, @edt, @usr)";

        //                    using (FbCommand commandInsert = new FbCommand(query, connection))
        //                    {
        //                        try
        //                        {
        //                            commandInsert.Parameters.Add(new FbParameter("toid", toRecordId));
        //                            commandInsert.Parameters.Add(new FbParameter("objid", matchObjectId));
        //                            commandInsert.Parameters.Add(new FbParameter("orgid", matchOrgToId));
        //                            commandInsert.Parameters.Add(new FbParameter("rid", t.rightId));
        //                            commandInsert.Parameters.Add(new FbParameter("docid", documentId));
        //                            commandInsert.Parameters.Add(new FbParameter("docdt", doc.docDate.Date));
        //                            commandInsert.Parameters.Add(new FbParameter("relid", relationId));
        //                            commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
        //                            commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

        //                            commandInsert.Transaction = transaction;
        //                            commandInsert.ExecuteNonQuery();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            log.Info(ex.Message);
        //                            throw;
        //                        }
        //                    }

        //                    // Update the "from" record - add relation to the "to" record
        //                    if (fromRecordId > 0)
        //                    {
        //                        query = "UPDATE OBJECT_PRAVO SET CLOSINGKARD = @toid WHERE ID = @fromid";

        //                        using (FbCommand commandInsert = new FbCommand(query, connection))
        //                        {
        //                            try
        //                            {
        //                                commandInsert.Parameters.Add(new FbParameter("fromid", fromRecordId));
        //                                commandInsert.Parameters.Add(new FbParameter("toid", toRecordId));

        //                                commandInsert.Transaction = transaction;
        //                                commandInsert.ExecuteNonQuery();
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                log.Info(ex.Message);
        //                                throw;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private static void AddQueryParameter(Dictionary<string, object> parameters,
        //    ref string fieldList, ref string paramList, string fieldName, string paramName,
        //    AppendixObject obj, ColumnCategory category, FbDbType dbType, string dictionaryName, bool isUpdate, int maxStringLen)
        //{
        //    string value = "";
        //    int code = -1;

        //    if (obj.properties.TryGetValue(category, out value))
        //    {
        //        object val = value;

        //        // Convert the object to the proper data type
        //        try
        //        {
        //            switch (dbType)
        //            {
        //                case FbDbType.VarChar:
        //                    if (maxStringLen > 0 && value.Length > maxStringLen)
        //                    {
        //                        value = value.Substring(0, maxStringLen);
        //                    }
        //                    break;

        //                case FbDbType.Decimal:
        //                    val = decimal.Parse(value);
        //                    break;

        //                case FbDbType.Integer:
        //                    if (dictionaryName.Length > 0)
        //                    {
        //                        code = DB.FindCodeInDictionaryNJF(dictionaryName, value);

        //                        val = (code >= 0) ? (object)code : null;
        //                    }
        //                    else
        //                    {
        //                        val = int.Parse(value);
        //                    }
        //                    break;
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            val = null;
        //        }

        //        if (val != null)
        //        {
        //            parameters.Add(paramName, val);

        //            if (fieldList.Length > 0)
        //                fieldList += ", ";

        //            if (isUpdate)
        //            {
        //                // Format for the UPDATE statement
        //                fieldList += fieldName + " = " + paramName;
        //            }
        //            else
        //            {
        //                // Format for the INSERT statement
        //                fieldList += fieldName;
        //            }

        //            if (paramList.Length > 0)
        //                paramList += ", ";

        //            paramList += paramName;
        //        }
        //    }
        //}

        //private static void RegisterNJFDocument(int documentId, ImportedDoc doc)
        //{
        //    DocumentNJF doc1nf = new DocumentNJF();

        //    doc1nf.documentId = documentId;
        //    doc1nf.documentKind = doc.docTypeId;
        //    doc1nf.documentDate = doc.docDate;
        //    doc1nf.documentNumber = doc.docNum.Trim().ToUpper();
        //    doc1nf.documentTitle = doc.docTitle.Trim().ToUpper();

        //    if (doc.docTypeId == 3) // AKT
        //    {
        //        actsNJF.Add(documentId, doc1nf);
        //    }
        //    else
        //    {
        //        documentsNJF.Add(documentId, doc1nf);
        //    }
        //}

        //private static void RegisterNJFAct(int documentId, ImportedAct act, DocumentNJF rishennya)
        //{
        //    DocumentNJF doc1nf = new DocumentNJF();

        //    doc1nf.documentId = documentId;
        //    doc1nf.documentKind = 3;
        //    doc1nf.documentDate = act.docDate;
        //    doc1nf.documentNumber = act.docNum.Trim().ToUpper();
        //    doc1nf.documentTitle = act.docTitle.Trim().ToUpper();

        //    actsNJF.Add(documentId, doc1nf);

        //    if (rishennya.dependentDocuments == null)
        //        rishennya.dependentDocuments = new List<int>();

        //    rishennya.dependentDocuments.Add(documentId);
        //}

        //private static int GenerateNewFirebirdId(FbConnection connection, string generatorName, FbTransaction transaction)
        //{
        //    int objectId = -1;

        //    string query = "SELECT GEN_ID(" + generatorName + ", 1) FROM RDB$DATABASE";

        //    using (FbCommand command = new FbCommand(query, connection))
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

        public static bool ExportAct(/*Preferences preferences,*/ ImportedAct act, Document rishennya)
        {
            return true;


            //bool result = false;
            //manualStreetCodeMappingNJFto1NF = ObjectFinder.GetStreetCodeMatchNJFto1NF();

            //// Test connections
            //FbConnection connectionNJF = new FbConnection(preferences.GetNJFConnectionString(UserName, UserPassword));

            //try
            //{
            //    connectionNJF.Open();
            //    connectionNJF.Close();
            //}
            //catch (Exception ex)
            //{
            //    log.Info("Помилка доступу до бази даних 'Розпорядження'" + ex.Message);
            //    return false;
            //}


            //try
            //{
            //    connectionNJF.Open();
            //    LoadDocumentsFromNJF(connectionNJF);
            //    // Make all changes to the Firebird database in a transaction
            //    FbTransaction transaction = null;

            //    try
            //    {
            //        transaction = connectionNJF.BeginTransaction();

            //        // Create the document
            //        int documentId = CreateDocumentNJF(connectionNJF, transaction,
            //            3, act.docNum, act.docTitle, act.docDate, act.docSum, act.docFinalSum);

            //        if (documentId > 0)
            //        {
            //            // Create relation to the main document

            //            AddRelationToPrimaryDocNJF(connectionNJF, transaction, documentId, rishennya.documentNumber, rishennya.documentDate);

            //            // Create relations to all objects
            //            //ExportActObjects(connectionNJF, transaction, act, documentId);

            //            // Save the created document in our local data structures
            //            RegisterNJFAct(documentId, act, rishennya);
            //        }

            //        // Commit the transaction
            //        transaction.Commit();
            //        transaction = null;

            //        result = true;
            //    }
            //    catch (Exception)
            //    {
            //        // Roll back the transaction
            //        if (transaction != null)
            //        {
            //            transaction.Rollback();
            //        }

            //        throw;
            //    }

            //    connectionNJF.Close();
            //}
            //catch (Exception ex)
            //{
            //    log.Info("Помилка доступу до бази даних 'Розпорядження'" + ex.Message);
            //}

            //return result;
        }

        //private static void ExportActObjects(FbConnection connection, FbTransaction transaction, ImportedAct act, int documentId)
        //{
        //    foreach (ActObject obj in act.actObjects)
        //    {
        //        if (obj.objectId < 0)
        //            continue;

        //        // If this object does not belong to Rishennya, add it to the Rishennya as well
        //        if (obj.objectDocsPropertiesId <= 0)
        //        {
        //            AddActObjectToRishennya(obj, connection, transaction, documentId);
        //        }

        //        AddSingleObjectToDocument(obj, connection, transaction, documentId);
        //    }
        //}

        //private static void AddActObjectToRishennya(ActObject actObj, FbConnection connectionNJF, FbTransaction transaction, int rishennyaId)
        //{
        //    // At this point a new record might be already added to the OBJECT_DOKS_PROPERTIES table via a trigger. Check if this is so
        //    FbCommand cmd = new FbCommand("", connectionNJF);

        //    if (actObj.objectSquare is decimal)
        //    {
        //        // If square is specified, it must match
        //        cmd.CommandText = "SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE DOK_ID = @doc AND OBJECT_KOD = @ob AND NAME = @nm AND SQUARE = @sq";
        //        cmd.Parameters.Add(new FbParameter("sq", actObj.objectSquare));
        //    }
        //    else if (actObj.objectLen is decimal)
        //    {
        //        // If length is specified, it must match
        //        cmd.CommandText = "SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE DOK_ID = @doc AND OBJECT_KOD = @ob AND NAME = @nm AND LEN = @l";
        //        cmd.Parameters.Add(new FbParameter("l", actObj.objectLen));
        //    }
        //    else
        //    {
        //        cmd.CommandText = "SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE DOK_ID = @doc AND OBJECT_KOD = @ob AND NAME = @nm";
        //    }

        //    cmd.Parameters.Add(new FbParameter("doc", rishennyaId));
        //    cmd.Parameters.Add(new FbParameter("ob", actObj.objectId));
        //    cmd.Parameters.Add(new FbParameter("nm", actObj.objectName));

        //    bool objectDocPropertiesExists = false;

        //    try
        //    {
        //        cmd.Transaction = transaction;

        //        using (FbDataReader r = cmd.ExecuteReader())
        //        {
        //            objectDocPropertiesExists = r.Read();
        //            r.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info(ex.Message);
        //        throw;
        //    }

        //    // If there is no such object in Rishennya, add it
        //    if (!objectDocPropertiesExists)
        //    {
        //        AddSingleObjectToDocument(actObj, connectionNJF, transaction, rishennyaId);
        //    }
        //}

        //private static void AddSingleObjectToDocument(ActObject obj, FbConnection connectionNJF, FbTransaction transaction, int documentId)
        //{
        //    // Generate a new object-doc relation ID
        //    int relationId = GenerateNewFirebirdId(connectionNJF, "OBJECT_DOKS_GEN", transaction);

        //    // Insert the relation into OBJECT_DOKS table
        //    string query = "INSERT INTO OBJECT_DOKS (ID, DOK_ID, OBJECT_KOD, OBJECT_KODSTAN, DTISP, ISP) VALUES (@relid, @docid, @objid, 0, @edt, @usr)";

        //    using (FbCommand commandInsert = new FbCommand(query, connectionNJF))
        //    {
        //        try
        //        {
        //            commandInsert.Parameters.Add(new FbParameter("relid", relationId));
        //            commandInsert.Parameters.Add(new FbParameter("docid", documentId));
        //            commandInsert.Parameters.Add(new FbParameter("objid", obj.objectId));
        //            commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
        //            commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

        //            commandInsert.Transaction = transaction;
        //            commandInsert.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Info(ex.Message);
        //            throw;
        //        }
        //    }

        //    // At this point a new record might be already added to the OBJECT_DOKS_PROPERTIES table via a trigger. Check if this is so
        //    bool objectDocPropertiesExists = false;

        //    query = @"SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE ID = @relid";

        //    using (FbCommand cmd = new FbCommand(query, connectionNJF))
        //    {
        //        try
        //        {
        //            cmd.Parameters.Add(new FbParameter("relid", relationId));
        //            cmd.Transaction = transaction;

        //            using (FbDataReader r = cmd.ExecuteReader())
        //            {
        //                if (r.Read())
        //                {
        //                    objectDocPropertiesExists = true;
        //                }

        //                r.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Info(ex.Message);
        //            throw;
        //        }
        //    }

        //    if (objectDocPropertiesExists)
        //    {
        //        // Update the existing record using INSERT
        //        using (FbCommand command = obj.PrepareUpdateStatement(connectionNJF, relationId, documentId))
        //        {
        //            try
        //            {
        //                command.Transaction = transaction;
        //                command.ExecuteNonQuery();
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Info(ex.Message);
        //                throw;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Add new record using INSERT
        //        using (FbCommand command = obj.PrepareInsertStatement(connectionNJF, relationId, documentId))
        //        {
        //            try
        //            {
        //                command.Transaction = transaction;
        //                command.ExecuteNonQuery();
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Info(ex.Message);
        //                throw;
        //            }
        //        }
        //    }
        //}

        public static bool TransferBalansObjects(SqlConnection connectionSql, SqlTransaction transactionSql, ImportedAct act, Document rishennya, bool notifyByEmail, int vidch_type_id, int? request_id = null)
        {
            bool result = false;

            try
            {
                foreach (ActObject obj in act.actObjects)
                {
                    if (obj.makeChangesIn1NF)
                    {
                        foreach (BalansTransfer bt in obj.balansTransfers)
                        {
                            if (bt.IsFullyDefined())
                            {
                                try
                                {
                                    switch (bt.transferType)
                                    {
                                        case ObjectTransferType.Transfer:
                                            bt.balansId = CutBalansObject(connectionSql, transactionSql, bt, rishennya, act, notifyByEmail);
                                            break;

                                        case ObjectTransferType.Create:
                                            
                                            bt.balansId = CreateBalansObject(connectionSql, transactionSql, obj, bt, rishennya, act, notifyByEmail, request_id);
                                            break;

                                        case ObjectTransferType.Destroy:
                                            MarkBalansObjectAsDeleted(connectionSql, transactionSql, bt.balansId, rishennya, act, bt.organizationToId, notifyByEmail, true, vidch_type_id);
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                    throw;
                                }
                            }
                        }
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                result = false;
                throw;
            }

            return result;
        }

        ///TEST!!!
        //private static void CreateDocuments1NF(/*FbConnection connection1NF, FbTransaction transaction,*/ DocumentNJF rishennya, ImportedAct act, BalansTransfer bt)
        //{
        //    var docId = -1;

        //    log.Info("CreateDocuments1NF == " + rishennya.documentDate.ToString() + " = " + act.docDate.ToString());

        //    var query = "select first 1 DOK_ID from ROZP_DOK where cast(DOKDATA as date) = @dok_date and DOKNUM = @dok_num and DOKKIND = @dok_kind";

        //    try
        //    {
        //        log.Info("try to find ROZP_DOK with dok_num = " + rishennya.documentNumber.Trim().ToUpper() + "        " + rishennya.documentDate.Date);
        //        using (FbCommand cmd = new FbCommand(query, connection1NF))
        //        {
        //            cmd.Transaction = transaction;

        //            cmd.Parameters.Clear();
        //            cmd.Parameters.Add(new FbParameter("dok_date", rishennya.documentDate.Date));
        //            cmd.Parameters.Add(new FbParameter("dok_num", rishennya.documentNumber.Trim().ToUpper()));
        //            cmd.Parameters.Add(new FbParameter("dok_kind", rishennya.documentKind));

        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    docId = reader.GetInt32(0);
        //                }
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message + "   " + query + rishennya.documentDate.ToString() + rishennya.documentKind.ToString());
        //        throw;
        //    }

        //    if (docId < 0)
        //    {
        //        docId = GenerateNewFirebirdId(connection1NF, "GEN_ROZP_DOK_ID", transaction);
        //        query = "insert into ROZP_DOK (DOK_ID, DOKKIND, DOKDATA, DOKNUM, DT, ISP) values" +
        //                        " (@did, @kind, @dtdoc, @num, @dt, @isp)";
        //        try
        //        {
        //            using (FbCommand command = new FbCommand(query, connection1NF))
        //            {
        //                command.Transaction = transaction;

        //                command.Parameters.Clear();
        //                command.Parameters.Add(new FbParameter("did", docId));
        //                command.Parameters.Add(new FbParameter("kind", rishennya.documentKind));
        //                command.Parameters.Add(new FbParameter("dtdoc", rishennya.documentDate));
        //                command.Parameters.Add(new FbParameter("num", rishennya.documentNumber.Trim().ToUpper()));
        //                command.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
        //                command.Parameters.Add(new FbParameter("isp", rishennya.modify_by));

        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message + "   " + query);
        //            //throw;
        //        }

        //    }

        //    var actId = -1;

        //    try
        //    {
        //        log.Info("try to find ROZP_DOK AKT with doc_number = " + act.docNum.Trim().ToUpper() + "    " + act.docDate.Date.ToString());
        //        var selActQuery = "select first 1 DOK_ID from ROZP_DOK where cast(DOKDATA as date) = @dt and DOKNUM = @doc_number and DOKKIND = @dok_kind";
        //        using (FbCommand selCmd = new FbCommand(selActQuery, connection1NF))
        //        {
        //            selCmd.Transaction = transaction;

        //            selCmd.Parameters.Clear();
        //            selCmd.Parameters.Add(new FbParameter("dt", act.docDate.Date));
        //            selCmd.Parameters.AddWithValue("doc_number", act.docNum.Trim().ToUpper());
        //            selCmd.Parameters.Add(new FbParameter("dok_kind", 3));

        //            using (FbDataReader reader = selCmd.ExecuteReader())
        //            {
        //                if (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    actId = reader.GetInt32(0);
        //                }
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        log.Error(ex.Message + "   " + query + act.docDate.ToString() + "3");
        //        //throw;
        //    }

        //    if (actId < 0)
        //    {
        //        actId = GenerateNewFirebirdId(connection1NF, "GEN_ROZP_DOK_ID", transaction);

        //        try
        //        {
        //            using (FbCommand command = new FbCommand(query, connection1NF))
        //            {
        //                command.Transaction = transaction;

        //                command.Parameters.Clear();
        //                command.Parameters.Add(new FbParameter("did", actId));
        //                command.Parameters.Add(new FbParameter("kind", 3)); // 3 = AKT
        //                command.Parameters.Add(new FbParameter("dtdoc", act.docDate));
        //                command.Parameters.Add(new FbParameter("num", act.docNum.Trim().ToUpper()));
        //                command.Parameters.Add(new FbParameter("dt", act.docDate));
        //                command.Parameters.Add(new FbParameter("isp", rishennya.modify_by));

        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message + "   " + query);
        //            //throw;
        //        }
        //    }

        //    query = "insert into DOK_DEPEND (MASTER, SLAVE, VID) values (@par_doc_id, @chi_doc_id, 1)";

        //    try
        //    {
        //        using (FbCommand command = new FbCommand(query, connection1NF))
        //        {
        //            command.Transaction = transaction;

        //            command.Parameters.Clear();
        //            command.Parameters.Add(new FbParameter("par_doc_id", docId));
        //            command.Parameters.Add(new FbParameter("chi_doc_id", actId));

        //            command.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message + "   " + query);
        //        //throw;
        //    }

        //    var objRozpDocRelationId = -1;

        //    query = "select first 1 ID from OBJECT_DOKS_PROPERTIES where OBJECT_KOD = @oid and DOK_ID = @did";

        //    try
        //    {
        //        using (FbCommand cmd = new FbCommand(query, connection1NF))
        //        {
        //            cmd.Transaction = transaction;

        //            cmd.Parameters.Clear();
        //            cmd.Parameters.Add(new FbParameter("oid", bt.objectId_1NF));
        //            cmd.Parameters.Add(new FbParameter("did", docId));

        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    objRozpDocRelationId = reader.GetInt32(0);
        //                }
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message + "   " + query);
        //        //throw;
        //    }



        //    if (objRozpDocRelationId < 0)
        //    {
        //        objRozpDocRelationId = GenerateNewFirebirdId(connection1NF, "OBJECT_DOKS_GEN", transaction);

        //        query = "insert into OBJECT_DOKS_PROPERTIES (ID, OBJECT_KOD, OBJECT_KODSTAN, DOK_ID, DT, ISP, BAL_FLAG)" +
        //                   " values (@odpid, @objid, 1, @did, @dt, @isp, 1)";

        //        try
        //        {
        //            using (FbCommand cmd = new FbCommand(query, connection1NF))
        //            {
        //                cmd.Transaction = transaction;

        //                cmd.Parameters.Clear();
        //                cmd.Parameters.Add(new FbParameter("odpid", objRozpDocRelationId));
        //                cmd.Parameters.Add(new FbParameter("objid", bt.objectId_1NF));
        //                cmd.Parameters.Add(new FbParameter("did", docId));
        //                cmd.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
        //                cmd.Parameters.Add(new FbParameter("isp", rishennya.modify_by));

        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message + "   " + query);
        //            //throw;
        //        }
        //    }

        //    var balRozpDocRelationId = -1;

        //    query = "select first 1 ID from BALANS_DOK_PROP where BALID = @bid AND OBJECT_KOD = @oid and DOK_ID = @did";

        //    try
        //    {
        //        using (FbCommand cmd = new FbCommand(query, connection1NF))
        //        {
        //            cmd.Transaction = transaction;

        //            cmd.Parameters.Clear();
        //            cmd.Parameters.Add(new FbParameter("bid", bt.balansId_1NF));
        //            cmd.Parameters.Add(new FbParameter("oid", bt.objectId_1NF));
        //            cmd.Parameters.Add(new FbParameter("did", objRozpDocRelationId));

        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    balRozpDocRelationId = reader.GetInt32(0);
        //                }
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message + "   " + query);
        //        //throw;
        //    }


        //    if (balRozpDocRelationId < 0)
        //    {
        //        balRozpDocRelationId = GenerateNewFirebirdId(connection1NF, "GEN_BALANS_DOK_PROP", transaction);

        //        query = "insert into BALANS_DOK_PROP (ID, BALID, OBJECT_KOD, FLAG, DOK_ID, SORT_FLD)" +
        //                    " values (@rid, @bid, @objid, 1, @did, 0)";

        //        try
        //        {
        //            using (FbCommand cmd = new FbCommand(query, connection1NF))
        //            {
        //                cmd.Transaction = transaction;

        //                cmd.Parameters.Clear();
        //                cmd.Parameters.Add(new FbParameter("rid", balRozpDocRelationId));
        //                cmd.Parameters.Add(new FbParameter("bid", bt.balansId_1NF));
        //                cmd.Parameters.Add(new FbParameter("objid", bt.objectId_1NF));
        //                cmd.Parameters.Add(new FbParameter("did", objRozpDocRelationId));

        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message + "   " + query);
        //            //throw;
        //        }
        //    }

        //    var objDocRelationId = -1;

        //    query = "select first 1 ID from OBJECT_DOKS_PROPERTIES where OBJECT_KOD = @oid and DOK_ID = @did";

        //    try
        //    {
        //        using (FbCommand cmd = new FbCommand(query, connection1NF))
        //        {
        //            cmd.Transaction = transaction;

        //            cmd.Parameters.Clear();
        //            cmd.Parameters.Add(new FbParameter("oid", bt.objectId_1NF));
        //            cmd.Parameters.Add(new FbParameter("did", actId));

        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    objDocRelationId = reader.GetInt32(0);
        //                }
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message + "   " + query);
        //        //throw;
        //    }



        //    if (objDocRelationId < 0)
        //    {
        //        objDocRelationId = GenerateNewFirebirdId(connection1NF, "OBJECT_DOKS_GEN", transaction);

        //        query = "insert into OBJECT_DOKS_PROPERTIES (ID, OBJECT_KOD, OBJECT_KODSTAN, DOK_ID, DT, ISP, BAL_FLAG)" +
        //                   " values (@odpid, @objid, 1, @did, @dt, @isp, 1)";

        //        try
        //        {
        //            using (FbCommand cmd = new FbCommand(query, connection1NF))
        //            {
        //                cmd.Transaction = transaction;

        //                cmd.Parameters.Clear();
        //                cmd.Parameters.Add(new FbParameter("odpid", objDocRelationId));
        //                cmd.Parameters.Add(new FbParameter("objid", bt.objectId_1NF));
        //                cmd.Parameters.Add(new FbParameter("did", actId));
        //                cmd.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
        //                cmd.Parameters.Add(new FbParameter("isp", rishennya.modify_by));

        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message + "   " + query);
        //            //throw;
        //        }
        //    }

        //    var balDocRelationId = -1;

        //    query = "select first 1 ID from BALANS_DOK_PROP where BALID = @bid AND OBJECT_KOD = @oid and DOK_ID = @did";

        //    try
        //    {
        //        using (FbCommand cmd = new FbCommand(query, connection1NF))
        //        {
        //            cmd.Transaction = transaction;

        //            cmd.Parameters.Clear();
        //            cmd.Parameters.Add(new FbParameter("bid", bt.balansId_1NF));
        //            cmd.Parameters.Add(new FbParameter("oid", bt.objectId_1NF));
        //            cmd.Parameters.Add(new FbParameter("did", objDocRelationId));

        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read() && !reader.IsDBNull(0))
        //                {
        //                    balDocRelationId = reader.GetInt32(0);
        //                }
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message + "   " + query);
        //        //throw;
        //    }


        //    if (balDocRelationId < 0)
        //    {
        //        balDocRelationId = GenerateNewFirebirdId(connection1NF, "GEN_BALANS_DOK_PROP", transaction);

        //        query = "insert into BALANS_DOK_PROP (ID, BALID, OBJECT_KOD, FLAG, DOK_ID, SORT_FLD)" +
        //                    " values (@rid, @bid, @objid, 1, @did, 0)";

        //        try
        //        {
        //            using (FbCommand cmd = new FbCommand(query, connection1NF))
        //            {
        //                cmd.Transaction = transaction;

        //                cmd.Parameters.Clear();
        //                cmd.Parameters.Add(new FbParameter("rid", balDocRelationId));
        //                cmd.Parameters.Add(new FbParameter("bid", bt.balansId_1NF));
        //                cmd.Parameters.Add(new FbParameter("objid", bt.objectId_1NF));
        //                cmd.Parameters.Add(new FbParameter("did", objDocRelationId));

        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex.Message + "   " + query);
        //            //throw;
        //        }
        //    }
        //}

        public static void MarkBalansObjectAsDeleted(SqlConnection connectionSql, SqlTransaction transactionSql,
            int balansId, Document rishennya, ImportedAct act, int vidchOrgId, bool notifyByEmail, bool copyToDeleted, int vidch_type_id)
        {
            // Mark object as DELETED in 1NF
            string fields = "DELETED = 1, DTDELETE = @dt, EDT = @dt, DT = @dt, ISP = @isp, EIS_MODIFIED_BY = @isp, " +
                "EIS_VIDCH_DOC_TYPE = @dtype, EIS_VIDCH_DOC_NUM = @dnum, EIS_VIDCH_DOC_DATE = @ddate";

            if (vidchOrgId > 0)
                fields += ", EIS_VIDCH_ORG_ID = @vorgid";

            string query = "UPDATE BALANS_1NF SET " + fields + " WHERE ID = @balid";

            //using (FbCommand command = new FbCommand(query, connection1NF))
            //{
            //    try
            //    {
            //        command.Parameters.Add(new FbParameter("balid", balansId));
            //        command.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
            //        command.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

            //        command.Parameters.Add(new FbParameter("dtype", rishennya.documentKind));
            //        command.Parameters.Add(new FbParameter("dnum", rishennya.documentNumber));
            //        command.Parameters.Add(new FbParameter("ddate", rishennya.documentDate.Date));

            //        if (vidchOrgId > 0)
            //            command.Parameters.Add(new FbParameter("vorgid", vidchOrgId));

            //        command.Transaction = transaction;
            //        command.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Info(ex.Message);
            //        throw;
            //    }
            //}

            // Mark object as DELETED in Sql Server
            if (connectionSql != null)
            {
                fields = "is_deleted = 1, del_date = @dt, modify_date = @dt, modified_by = @isp";

                if (rishennya != null)
                    fields += ", vidch_doc_type = @dtype, vidch_doc_num = @dnum, vidch_doc_date = @ddate";

                if (vidchOrgId > 0)
                    fields += ", vidch_org_id = @vorgid";

                query = "UPDATE balans SET " + fields + " WHERE id = @balid";

                using (SqlCommand command = new SqlCommand(query, connectionSql, transactionSql))
                {
                    bool deleteSucceeded = false;

                    try
                    {
                        command.Parameters.Add(new SqlParameter("balid", balansId));
                        command.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));
                        command.Parameters.Add(new SqlParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
                        if (rishennya != null)
                        {
                            command.Parameters.Add(new SqlParameter("dtype", rishennya.documentKind));
                            command.Parameters.Add(new SqlParameter("dnum", rishennya.documentNumber));
                            command.Parameters.Add(new SqlParameter("ddate", rishennya.documentDate.Date));
                        }

                        if (vidchOrgId > 0)
                            command.Parameters.Add(new SqlParameter("vorgid", vidchOrgId));

                        command.ExecuteNonQuery();
                        deleteSucceeded = true;
                    }
                    catch (Exception ex)
                    {
                        log.Info(ex.Message);
                        throw;
                    }

                    if (deleteSucceeded)
                    {
                        // Delete the object from user report
                        using (SqlCommand cmd = new SqlCommand("dbo.fnDeleteBalansObjectInReport", connectionSql, transactionSql))
                        {
                            try
                            {
                                cmd.Parameters.Add(new SqlParameter("BALANS_ID", balansId));
                                cmd.Parameters.Add(new SqlParameter("COPY_TO_DELETED", copyToDeleted ? 1 : 0));
                                cmd.Parameters.Add(new SqlParameter("VIDCH_TYPE_ID", vidch_type_id));
                                cmd.Parameters.Add(new SqlParameter("VIDCH_DOC_NUM", act == null ? string.Empty : act.docNum));
                                cmd.Parameters.Add(new SqlParameter("VIDCH_DOC_DATE", act == null ? DateTime.Now : act.docDate));
                                cmd.Parameters.Add(new SqlParameter("VIDCH_ORG_ID", vidchOrgId));

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.ExecuteNonQuery();

                                if (notifyByEmail)
                                    EmailNotifier.NotifyBalansObjectRemoved(connectionSql, transactionSql, balansId);
                            }
                            catch (Exception ex)
                            {
                                log.Info(ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
        }

        private static void ChangeBalansObjectSquare(/*FbConnection connection1NF, FbTransaction transaction,*/ SqlConnection connectionSql, SqlTransaction transactionSql,
            int balansId, decimal newSquare)
        {
            // Get the current square from 1NF
            //decimal sqrTotal = -1m;
            //decimal sqrNonHabit = -1m;

            //int organization_id = -1;

            //string query = "SELECT SQR_ZAG, SQR_NEJ, ORG FROM BALANS_1NF WHERE ID = @balid";

            //using (FbCommand command = new FbCommand(query, connection1NF))
            //{
            //    try
            //    {
            //        command.Parameters.Add(new FbParameter("balid", balansId));
            //        command.Transaction = transaction;

            //        using (FbDataReader reader = command.ExecuteReader())
            //        {
            //            if (reader.Read())
            //            {
            //                sqrTotal = reader.IsDBNull(0) ? -1m : reader.GetDecimal(0);
            //                sqrNonHabit = reader.IsDBNull(1) ? -1m : reader.GetDecimal(1);
            //                organization_id = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
            //            }

            //            reader.Close();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Info(ex.Message);
            //        throw;
            //    }
            //}

            // Validate parameters


            // Update the object in 1NF
            //string fields = "SQR_ZAG = @szag, EDT = @dt, DT = @dt, ISP = @isp";

            //if (sqrNonHabit >= 0)
            //    fields += ", SQR_NEJ = @snonh";

            //query = "UPDATE BALANS_1NF SET " + fields + " WHERE ID = @balid";

            //using (FbCommand command = new FbCommand(query, connection1NF))
            //{
            //    try
            //    {
            //        command.Parameters.Add(new FbParameter("balid", balansId));
            //        command.Parameters.Add(new FbParameter("szag", sqrTotal));
            //        command.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
            //        command.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

            //        if (sqrNonHabit >= 0)
            //            command.Parameters.Add(new FbParameter("snonh", sqrNonHabit));

            //        command.Transaction = transaction;
            //        command.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Info(ex.Message);
            //        throw;
            //    }
            //}

            // Update the object in SQL Server
            if (connectionSql != null)
            {
                decimal sqrTotal = newSquare;
                //int organization_id = -1;
                decimal sqrNonHabit = -1m;

                using (SqlCommand cmdSource = new SqlCommand("select sqr_non_habit from balans where id = @balid", connectionSql, transactionSql))
                {
                    cmdSource.Parameters.AddWithValue("balid", balansId);
                    using (SqlDataReader r = cmdSource.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            sqrNonHabit = r.IsDBNull(0) ? -1m : r.GetDecimal(0);
                        }
                        r.Close();
                    }

                }

                

                if (sqrNonHabit > sqrTotal)
                {
                    sqrNonHabit = sqrTotal;
                }

                
                string fields = "sqr_total = @szag, modify_date = @dt";

                if (sqrNonHabit >= 0)
                    fields += ", sqr_non_habit = @snonh";

                string query = "UPDATE balans SET " + fields + " WHERE id = @balid";

                using (SqlCommand command = new SqlCommand(query, connectionSql, transactionSql))
                {
                    try
                    {
                        command.Parameters.Add(new SqlParameter("balid", balansId));
                        command.Parameters.Add(new SqlParameter("szag", sqrTotal));
                        command.Parameters.Add(new SqlParameter("dt", DateTime.Now));

                        if (sqrNonHabit >= 0)
                            command.Parameters.Add(new SqlParameter("snonh", sqrNonHabit));

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log.Info(ex.Message);
                        throw;
                    }
                }

                //if (organization_id > 0)
                //{
                    var queryForExternalInput = "UPDATE reports1nf_balans SET sqr_total = @szag, sqr_non_habit = @snonh, modify_date = @dt WHERE " + /*"organization_id = @org_id AND " +*/ "id = @balid";
                    using (SqlCommand command = new SqlCommand(queryForExternalInput, connectionSql, transactionSql))
                    {
                        try
                        {
                            command.Parameters.Add(new SqlParameter("balid", balansId));
                            //command.Parameters.Add(new SqlParameter("org_id", organization_id));
                            command.Parameters.Add(new SqlParameter("szag", sqrTotal));
                            command.Parameters.Add(new SqlParameter("dt", DateTime.Now));

                            if (sqrNonHabit >= 0)
                                command.Parameters.Add(new SqlParameter("snonh", sqrNonHabit));

                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            log.Info(ex.Message);
                            throw;
                        }
                    }
                //}
            }
        }

        private static int CopyBalansObject(/*FbConnection connection1NF, FbTransaction transaction,*/ SqlConnection connectionSql, SqlTransaction transactionSql,
            int balansId, int newOwnerOrgId, Document rishennya, ImportedAct act)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            GUKV.ImportToolUtils.FieldMappings.Create1NFBalansFieldMapping(mapping, false, true);

            // Generate new balans Id
            int newBalansId = -1;// GenerateNewFirebirdId(connection1NF, "GEN_BALANS_1NF", transaction);

            using (SqlCommand cmdNewId = new SqlCommand("select MAX(id) + 1 from balans", connectionSql, transactionSql))
            {
                using (SqlDataReader reader = cmdNewId.ExecuteReader())
                {
                    if (reader.Read())
                        newBalansId = (int)reader.GetValue(0);
                    reader.Close();
                }
            }

            SqlCommand cmdInsertSql = new SqlCommand();
            string insertFieldsSql = "";
            string insertParamsSql = "";
            using (SqlCommand cmdSourceData = new SqlCommand("select * from balans where id = @balansId", connectionSql, transactionSql))
            {
                cmdSourceData.Parameters.AddWithValue("balansId", balansId);
                using (SqlDataReader reader = cmdSourceData.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string paramName = "p" + i.ToString();
                            string fieldNameSql = reader.GetName(i);
                            string fieldName = fieldNameSql.ToLower();

                            if (fieldName == "obj_nomer") // computed column
                                continue;

                            object value = reader.IsDBNull(i) ? null : reader.GetValue(i);

                            if (fieldName == "id")
                            {
                                value = newBalansId;
                            }
                            else if (fieldName == "organization_id")
                            {
                                value = newOwnerOrgId;
                            }
                            else if (fieldName == "modified_by")
                            {
                                value = rishennya.modify_by;
                            }
                            else if (fieldName == "modify_date")
                            {
                                value = DateTime.Now;
                            }
                            else if (fieldName == "balans_doc_type")
                            {
                                value = rishennya.documentKind;
                            }
                            else if (fieldName == "balans_doc_num")
                            {
                                value = rishennya.documentNumber;
                            }
                            else if (fieldName == "balans_doc_date")
                            {
                                value = rishennya.documentDate.Date;
                            }
                            else if (fieldName == "ownership_type_id")
                            {
                                value = rishennya.ownership_type_id;
                            }
                            else if (fieldName == "form_ownership_id")
                            {
                                //value = value is int ? MatchOrgOwnership1NFtoNJF((int)value) : null;
                                //value = value;
                            }
                            else if (fieldName == "ownership_doc_type")
                            {
                                value = rishennya.ownership_doc_type_id > 0 ? rishennya.ownership_doc_type_id : 3;
                            }
                            else if (fieldName == "ownership_doc_num")
                            {
                                value = act.docNum;
                            }
                            else if (fieldName == "ownership_doc_date")
                            {
                                value = act.docDate;
                            }
                            else if (fieldName == "reestr_no") // Обнуляем инвентарный номер при передаче объекта
                            {
                                value = null;
                            }
                            else if ((fieldName == "cost_balans") && (act.docSum > 0))
                            {
                                value = act.docSum;
                            }
                            else if ((fieldName == "cost_zalishkova")  && (act.docFinalSum > 0))
                            {
                                value = act.docFinalSum;
                            }


                            if (value == null)
                                continue;

                            insertFieldsSql += ", " + fieldNameSql;
                            insertParamsSql += ", @" + paramName;

                            cmdInsertSql.Parameters.AddWithValue(paramName, value);
                        }

                    }
                    reader.Close();
                }
            }


            //if (newBalansId > 0)
            //{
                //  Generate parameters for the INSERT command
                //FbCommand cmdInsert1NF = new FbCommand("", connection1NF);
                //SqlCommand cmdInsertSql = new SqlCommand();

                //string insertFields1NF = "";
                
                //string insertParams1NF = "";
                
                //bool ISP_exists = false;

                //string query = "SELECT * FROM BALANS_1NF WHERE ID = @balid";

                //using (FbCommand command = new FbCommand(query, connection1NF))
                //{
                //    command.Parameters.Add(new FbParameter("balid", balansId));
                //    command.Transaction = transaction;

                //    using (FbDataReader reader = command.ExecuteReader())
                //    {
                //        if (reader.Read())
                //        {
                //            for (int i = 0; i < reader.FieldCount; i++)
                //            {
                //                string paramName = "p" + i.ToString();
                //                string fieldName1NF = reader.GetName(i).ToUpper();
                //                object value = reader.IsDBNull(i) ? null : reader.GetValue(i);

                //                // Skip computed columns
                //                if (fieldName1NF != "OBJ_NOMER")
                //                {
                //                    // Some columns must be filled with new values
                //                    if (fieldName1NF == "ID")*
                //                    {
                //                        value = newBalansId;
                //                    }
                //                    else if (fieldName1NF == "ORG")*
                //                    {
                //                        value = newOwnerOrgId;
                //                    }
                //                    else if (fieldName1NF == "ISP")*
                //                    {
                //                        value = UserName.Length > 18 ? UserName.Substring(0, 18) : UserName;
                //                        ISP_exists = true;
                //                    }
                //                    else if (fieldName1NF == "EIS_MODIFIED_BY")*
                //                    {
                //                        value = rishennya.modify_by.Length > 64 ? rishennya.modify_by.Substring(0, 64) : rishennya.modify_by;
                //                    }
                //                    else if (fieldName1NF == "DT" || fieldName1NF == "EDT")*
                //                    {
                //                        value = DateTime.Now.Date;
                //                    }
                //                    else if (fieldName1NF == "EIS_BALANS_DOC_TYPE")*
                //                    {
                //                        value = rishennya.documentKind;
                //                    }
                //                    else if (fieldName1NF == "EIS_BALANS_DOC_NUM")*
                //                    {
                //                        value = rishennya.documentNumber;
                //                    }
                //                    else if (fieldName1NF == "EIS_BALANS_DOC_DATE")*
                //                    {
                //                        value = rishennya.documentDate.Date;
                //                    }
                //                    else if (fieldName1NF == "PRAVO")*
                //                    {
                //                        value = rishennya.ownership_type_id;
                //                    }

                //                    if (value != null)
                //                    {
                //                        //cmdInsert1NF.Parameters.Add(new FbParameter(paramName, value));

                //                        //insertFields1NF += ", " + fieldName1NF;
                //                        //insertParams1NF += ", @" + paramName;

                //                        // Get the Sql Server column by the name of 1NF column, and prepare INSERT for Sql Server
                //                        string fieldNameSql = "";

                //                        if (mapping.TryGetValue(fieldName1NF, out fieldNameSql))
                //                        {
                //                            if (fieldName1NF == "FORM_VLASN")*
                //                            {
                //                                var dict_org_ownership = value is int ? MatchOrgOwnership1NFtoNJF((int)value) : -1;
                //                                cmdInsertSql.Parameters.Add(new SqlParameter(paramName, dict_org_ownership));
                //                            }
                //                            else
                //                            {
                //                                cmdInsertSql.Parameters.Add(new SqlParameter(paramName, value));
                //                            }

                //                            insertFieldsSql += ", " + fieldNameSql;
                //                            insertParamsSql += ", @" + paramName;
                //                        }
                //                    }
                //                }
                //            }
                //        }

                //        reader.Close();
                //    }
                //}

                // We need to update the ISP field in 1NF, even if it is not mapped
                //if (!ISP_exists)
                //{
                //    insertFields1NF += ", ISP";
                //    insertParams1NF += ", @isp";
                //    cmdInsert1NF.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
                //}

                //if (insertFields1NF.StartsWith(", "))
                //    insertFields1NF = insertFields1NF.Substring(2);

                //if (insertParams1NF.StartsWith(", "))
                //    insertParams1NF = insertParams1NF.Substring(2);

                if (insertFieldsSql.StartsWith(", "))
                    insertFieldsSql = insertFieldsSql.Substring(2);

                if (insertParamsSql.StartsWith(", "))
                    insertParamsSql = insertParamsSql.Substring(2);

                // Copy the balans object in 1NF
                //try
                //{
                //    cmdInsert1NF.Transaction = transaction;
                //    cmdInsert1NF.CommandText = "INSERT INTO BALANS_1NF (" + insertFields1NF + ") VALUES (" + insertParams1NF + ")";
                //    cmdInsert1NF.ExecuteNonQuery();
                //}
                //catch (Exception ex)
                //{
                //    log.Info(ex.Message);
                //    throw;
                //}

                if (connectionSql != null)
                {
                    // Copy the balans object in SQL Server
                    try
                    {
                        cmdInsertSql.Connection = connectionSql;
                        cmdInsertSql.Transaction = transactionSql;
                        cmdInsertSql.CommandText = "INSERT INTO balans (" + insertFieldsSql + ") VALUES (" + insertParamsSql + ")";
                        cmdInsertSql.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log.Info(ex.Message);
                        throw;
                    }
                }
            //}

            return newBalansId;
        }

        private static Organization1NF GetOrganization(int id, SqlConnection connectionSql, SqlTransaction transactionSql)
        {
            Organization1NF res = null;
            using (SqlCommand cmd = new SqlCommand(@"select 
                    full_name,
                    form_ownership_id,
                    short_name,
                    zkpo_code
                from organizations where id = @org_id", connectionSql, transactionSql))
            {
                cmd.Parameters.AddWithValue("org_id", id);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        res = new Organization1NF();
                        //res.addrDistrictId = r.GetValue(0);
                        //res.addrKorpus = r.GetValue(1);
                        //res.addrNomer = r.GetValue(2);
                        //res.addrStreet = r.GetValue(3);
                        //res.addrZipCode = r.GetValue(4);
                        //res.buhgalterFIO = r.GetValue(5);
                        //res.buhgalterTel = r.GetValue(6);
                        //res.directorFIO = r.GetValue(7);
                        //res.directorTel = r.GetValue(8);
                        //res.fax = r.GetValue(9);
                        //res.formGospId = r.GetValue(10);
                        res.fullName = r.IsDBNull(0) ? "" : r.GetString(0);
                        //res.industryId = r.IsDBNull(1) r.GetValue(12);
                        //res.occupationId = r.GetValue(13);
                        res.organizationId = id;
                        res.ownershipFormId = r.IsDBNull(1) ? -1 : r.GetInt32(1);
                        res.shortName = r.IsDBNull(2) ? "" : r.GetString(2);
                        res.zkpo = r.IsDBNull(3) ? "" : r.GetString(3);
                    }
                    r.Close();
                }
            }
            return res;
            
        }

        private static int CreateBalansObject(/*FbConnection connection1NF, FbTransaction transaction,*/ SqlConnection connectionSql, SqlTransaction transactionSql,
            ActObject actObj, BalansTransfer bt, Document rishennya, ImportedAct act, bool notifyByEmail, int? request_id = null)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            GUKV.ImportToolUtils.FieldMappings.Create1NFBalansFieldMapping(mapping, false, true);

            int purposeGroup1NF = actObj.purposeGroupId; // MatchDictionaryNJFto1NF(DICT_PURPOSE_GROUP, actObj.purposeGroupIdNJF);
            int purpose1NF = actObj.purposeId;// MatchDictionaryNJFto1NF_Hierarchical(DICT_PURPOSE, actObj.purposeIdNJF, DICT_PURPOSE_GROUP, actObj.purposeGroupIdNJF);
            int objectType1NF = actObj.objectTypeId;// MatchObjectTypeNJFto1NF(actObj.objectTypeIdNJF);
            int objectKind1NF = actObj.objectKindId;// MatchObjectKindNJFto1NF(actObj.objectKindIdNJF);

            // Add all the properties we can take from 'privatization' table
            values.Add("OBJECT", actObj.objectId);
            values.Add("SQR_ZAG", bt.sqr);
            values.Add("SQR_NEJ", bt.sqr);

            values.Add("PRIZNAK1NF", 0);
            values.Add("UPD_SOURCE", 10); // 10 = Bagriy
            values.Add("DELETED", 0);
            values.Add("ISP", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName);
            values.Add("EIS_MODIFIED_BY", rishennya.modify_by.Length > 64 ? rishennya.modify_by.Substring(0, 64) : rishennya.modify_by);
            values.Add("DT", rishennya.modify_date);
            values.Add("EDT", DateTime.Now.Date);
            values.Add("PRIV_COUNT", 0);
            values.Add("LYEAR", DateTime.Now.Year);
            //values.Add("PRAVO", 1); // 'Ownership' right
            values.Add("STAN", DateTime.Now.Date);
            values.Add("OBJECT_STAN", 1);
            values.Add("ORG_STAN", 1);
            values.Add("TEXSTAN", 2); // Zadovilniy
            values.Add("HISTORY", 2); // Usual building

            values.Add("EIS_BALANS_DOC_TYPE", rishennya.documentKind); // balans_doc_type
            values.Add("EIS_BALANS_DOC_DATE", rishennya.documentDate.Date); // balans_doc_num
            values.Add("EIS_BALANS_DOC_NUM", rishennya.documentNumber); // balans_doc_date
            
            //values.Add("EIS_OWNERSHIP_DOC_TYPE", 3); // ownership_doc_type

            if (rishennya.ownership_doc_type_id > 0)
                values.Add("EIS_OWNERSHIP_DOC_TYPE", rishennya.ownership_doc_type_id);
            else
                values.Add("EIS_OWNERSHIP_DOC_TYPE", 3);


            if (!string.IsNullOrEmpty(act.docNum))
                values.Add("EIS_OWNERSHIP_DOC_NUM", act.docNum); // ownership_doc_num
            if (act.docDate != DateTime.MinValue)
                values.Add("EIS_OWNERSHIP_DOC_DATE", act.docDate); // ownership_doc_date

            //Organization1NF orgNewOwner = bt.orgTo1NF;
            //Organization1NF orgNewOwner = GetOrganization(bt.organizationToId_1NF, connectionSql, transactionSql);

            if (rishennya.ownership_type_id > 0)
                values.Add("PRAVO", rishennya.ownership_type_id); //ownership_type_id

            if (bt.form_ownership_id > 0)
                values.Add("FORM_VLASN", bt.form_ownership_id); // form_ownership_id

            if (act.docDate != DateTime.MinValue)
                values.Add("ZNOS_DATE", act.docDate);

            if (objectKind1NF > 0)
                values.Add("KINDOBJ", objectKind1NF);

            if (objectType1NF > 0)
                values.Add("TYPEOBJ", objectType1NF);

            if (purposeGroup1NF > 0)
                values.Add("GRPURP", purposeGroup1NF);

            if (purpose1NF > 0)
                values.Add("PURPOSE", purpose1NF);

            if (actObj.objectName.Length > 0)
                values.Add("PURP_STR", actObj.objectName);

            if (act.docSum > 0)
                values.Add("BALANS_VARTIST", act.docSum);

            if (act.docFinalSum > 0)
                values.Add("ZAL_VART", act.docFinalSum);

            if (actObj.objectFloorsStr.Length > 0)
                values.Add("FLOATS", actObj.objectFloorsStr);

            // Get information about the object
            string query = @"SELECT FULL_ULNAME, ULKOD, ULNAME2, ULKOD2, NOMER1, NOMER2, NOMER3, ADRDOP FROM OBJECT_1NF WHERE OBJECT_KOD = @bid";

            //using (FbCommand cmd = new FbCommand(query, connection1NF))
            //{
                //try
                //{
                //    cmd.Parameters.Add(new FbParameter("bid", actObj.objectId_1NF));
                //    cmd.Transaction = transaction;

                //    using (FbDataReader reader = cmd.ExecuteReader())
                //    {
                //        if (reader.Read())
                //        {
                //            if (!reader.IsDBNull(1))
                //                values.Add("OBJ_KODUL", reader.GetValue(1));

                //            if (!reader.IsDBNull(3))
                //                values.Add("ULKOD2", reader.GetValue(3));

                //            if (!reader.IsDBNull(0))
                //                values.Add("OBJ_ULNAME", reader.GetValue(0));

                //            if (!reader.IsDBNull(2))
                //                values.Add("ULNAME2", reader.GetValue(2));

                //            if (!reader.IsDBNull(4))
                //                values.Add("OBJ_NOMER1", reader.GetValue(4));

                //            if (!reader.IsDBNull(5))
                //                values.Add("OBJ_NOMER2", reader.GetValue(5));

                //            if (!reader.IsDBNull(6))
                //                values.Add("OBJ_NOMER3", reader.GetValue(6));

                //            if (!reader.IsDBNull(7))
                //                values.Add("OBJ_ADRDOP", reader.GetValue(7));
                //        }

                //        reader.Close();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    log.Info(ex.Message);
                //    throw;
                //}
            //}

            // Generate new balans Id
            //int newBalansId = GenerateNewFirebirdId(connection1NF, "GEN_BALANS_1NF", transaction);

            int newBalansId = 0;
            using (SqlCommand cmdNewBalansId = new SqlCommand("select max(id) + 1 from balans", connectionSql, transactionSql))
            {
                using (SqlDataReader r = cmdNewBalansId.ExecuteReader())
                {
                    if (r.Read())
                        newBalansId = (int)r.GetValue(0);
                    r.Close();
                }
            }

            if (newBalansId > 0)
            {
                values.Add("ID", newBalansId);
                values.Add("ORG", bt.organizationToId);

                // Generate parameters for INSERT statement
                Dictionary<string, object> parameters1NF = new Dictionary<string, object>();
                Dictionary<string, object> parametersSQL = new Dictionary<string, object>();

                int paramIndex = 1;
                string insertFieldList1NF = "";
                string insertFieldListSQL = "";
                string insertParamList1NF = "";
                string insertParamListSQL = "";

                foreach (KeyValuePair<string, object> pair in values)
                {
                    string paramName = "p" + paramIndex.ToString();

                    if (paramIndex > 1)
                    {
                        insertFieldList1NF += ", ";
                        insertParamList1NF += ", ";
                    }

                    insertFieldList1NF += pair.Key;
                    insertParamList1NF += "@" + paramName;

                    parameters1NF[paramName] = pair.Value;

                    // Generate the list of fields for Sql Server INSERT statement as well
                    string fieldNameSql = "";

                    if (mapping.TryGetValue(pair.Key, out fieldNameSql))
                    {
                        if (insertFieldListSQL.Length > 0)
                        {
                            insertFieldListSQL += ", ";
                            insertParamListSQL += ", ";
                        }

                        insertFieldListSQL += fieldNameSql;
                        insertParamListSQL += "@" + paramName;

                        parametersSQL[paramName] = pair.Value;
                    }

                    paramIndex++;
                }

                // Prepare the INSERT statement
                query = "INSERT INTO BALANS_1NF (" + insertFieldList1NF + ") VALUES (" + insertParamList1NF + ")";

                //using (FbCommand commandInsert = new FbCommand(query, connection1NF))
                //{
                //    try
                //    {
                //        foreach (KeyValuePair<string, object> pair in parameters1NF)
                //        {
                //            commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                //        }

                //        commandInsert.Transaction = transaction;
                //        commandInsert.ExecuteNonQuery();
                //    }
                //    catch (Exception ex)
                //    {
                //        log.Info(ex.Message);
                //        throw;
                //    }
                //}

                // Make the same change in SQL Server
                if (connectionSql != null)
                {
                    bool insertedOK = false;
                    query = "INSERT INTO balans (" + insertFieldListSQL + ") VALUES (" + insertParamListSQL + ")";

                    using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
                    {
                        try
                        {
                            foreach (KeyValuePair<string, object> pair in parametersSQL)
                            {
                                cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                            }

                            cmd.ExecuteNonQuery();

							if (request_id != null)
							{
								CopyTransferRequestPhoto(request_id.Value, newBalansId, connectionSql, transactionSql);
                                CopyTransferRishAttachfiles(request_id.Value, newBalansId, connectionSql, transactionSql);
                                CopyTransferAktAttachfiles(request_id.Value, newBalansId, connectionSql, transactionSql);
                            }

							insertedOK = true;
                        }
                        catch (Exception ex)
                        {
                            log.Info(ex.Message);
                            throw;
                        }
                    }

                    if (insertedOK)
                    {
                        // Add the object to user report

                        log.Info("dbo.fnAddBalansObjectToReport" + newBalansId.ToString());
                        using (SqlCommand cmd = new SqlCommand("dbo.fnAddBalansObjectToReport", connectionSql, transactionSql))
                        {
                            try
                            {
                                cmd.Parameters.Add(new SqlParameter("BALANS_ID", newBalansId));

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.ExecuteNonQuery();

                                if (notifyByEmail)
                                    EmailNotifier.NotifyBalansObjectAdded(connectionSql, transactionSql, newBalansId);
                            }
                            catch (Exception ex)
                            {
                                log.Info(ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }

            return newBalansId;
        }

		private static void CopyTransferRequestPhoto(int request_id, int newBalansId, SqlConnection connectionSql, SqlTransaction transactionSql)
		{
			string photoRootPath = LLLLhotorowUtils.ImgContentRootFolder;

			using (SqlCommand cmd = new SqlCommand("select id, file_name, file_ext, user_id, create_date from transfer_requests_photos where request_id = @reqid", connectionSql, transactionSql))
			{
				cmd.Parameters.AddWithValue("reqid", request_id);
				using (SqlDataReader r = cmd.ExecuteReader())
				{
					while (r.Read())
					{
						var id = r.GetInt32(0);
						var file_name = r.GetString(1);
						var file_ext = r.GetString(2);
						var user_id = r.GetGuid(3);
						var create_date = r.GetDateTime(4);

						var icmd = new SqlCommand("INSERT INTO reports1nf_photos (bal_id, file_name, file_ext, user_id, create_date) VALUES (@balid, @filename, @fileext, @usrid, @createdate); ; SELECT CAST(SCOPE_IDENTITY() AS int)", connectionSql, transactionSql);
						icmd.Parameters.Add(new SqlParameter("balid", newBalansId));
						icmd.Parameters.Add(new SqlParameter("filename", file_name));
						icmd.Parameters.Add(new SqlParameter("fileext", file_ext));
						icmd.Parameters.Add(new SqlParameter("usrid", user_id));
						icmd.Parameters.Add(new SqlParameter("createdate", create_date));
						var newId = (Int32)icmd.ExecuteScalar();

						var sourceFileToCopy = Path.Combine(photoRootPath, "TransferRequest", request_id.ToString(), id.ToString() + file_ext);
						var destFileToCopy = Path.Combine(photoRootPath, "1NF", newBalansId.ToString(), newId.ToString() + file_ext);
						if (LLLLhotorowUtils.Exists(sourceFileToCopy, connectionSql, transactionSql))
						{
							LLLLhotorowUtils.Copy(sourceFileToCopy, destFileToCopy, connectionSql, transactionSql);
						}
					}

					r.Close();
				}
			}
		}

        private static void CopyTransferRishAttachfiles(int request_id, int newBalansId, SqlConnection connectionSql, SqlTransaction transactionSql)
        {
            string photoRootPath = LLLLhotorowUtils.ImgFreeSquareRootFolder;

            using (SqlCommand cmd = new SqlCommand(@"
                select 
                    id, file_name, file_ext, modified_by, modify_date
                    ,(SELECT rep.id FROM reports1nf rep INNER JOIN balans bal ON bal.organization_id = rep.organization_id WHERE bal.id = @newBalansId) as report_id
                 from transfer_requests_rish_attachfiles where free_square_id = @reqid", connectionSql, transactionSql))
            {
                cmd.Parameters.AddWithValue("reqid", request_id);
                cmd.Parameters.AddWithValue("newBalansId", newBalansId);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var id = r.GetInt32(0);
                        var file_name = r.GetString(1);
                        var file_ext = r.GetString(2);
                        var modified_by = r.GetString(3);
                        var modify_date = r.GetDateTime(4);
                        var report_id = r.GetInt32(5);

                        int codBalansId = newBalansId + report_id * 500000;
                        var icmd = new SqlCommand("INSERT INTO reports1nf_balans_rish_attachfiles (free_square_id, file_name, file_ext, modified_by, modify_date) VALUES (@balid, @filename, @fileext, @modified_by, @modify_date); SELECT CAST(SCOPE_IDENTITY() AS int)", connectionSql, transactionSql);
                        icmd.Parameters.Add(new SqlParameter("balid", codBalansId));
                        icmd.Parameters.Add(new SqlParameter("filename", file_name));
                        icmd.Parameters.Add(new SqlParameter("fileext", file_ext));
                        icmd.Parameters.Add(new SqlParameter("modified_by", modified_by));
                        icmd.Parameters.Add(new SqlParameter("modify_date", modify_date));
                        var newId = (Int32)icmd.ExecuteScalar();

                        var sourceFileToCopy = Path.Combine(photoRootPath, "transfer_requests_rish_attachfiles", request_id.ToString(), file_name + file_ext);
                        var destFileToCopy = Path.Combine(photoRootPath, "reports1nf_balans_rish_attachfiles", codBalansId.ToString(), file_name + file_ext);
                        if (LLLLhotorowUtils.Exists(sourceFileToCopy, connectionSql, transactionSql))
                        {
                            LLLLhotorowUtils.Copy(sourceFileToCopy, destFileToCopy, connectionSql, transactionSql);
                        }
                    }

                    r.Close();
                }
            }
        }

        private static void CopyTransferAktAttachfiles(int request_id, int newBalansId, SqlConnection connectionSql, SqlTransaction transactionSql)
        {
            string photoRootPath = LLLLhotorowUtils.ImgFreeSquareRootFolder;

            using (SqlCommand cmd = new SqlCommand(@"
                select 
                    id, file_name, file_ext, modified_by, modify_date
                    ,(SELECT rep.id FROM reports1nf rep INNER JOIN balans bal ON bal.organization_id = rep.organization_id WHERE bal.id = @newBalansId) as report_id
                 from transfer_requests_akt_attachfiles where free_square_id = @reqid", connectionSql, transactionSql))
            {
                cmd.Parameters.AddWithValue("reqid", request_id);
                cmd.Parameters.AddWithValue("newBalansId", newBalansId);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var id = r.GetInt32(0);
                        var file_name = r.GetString(1);
                        var file_ext = r.GetString(2);
                        var modified_by = r.GetString(3);
                        var modify_date = r.GetDateTime(4);
                        var report_id = r.GetInt32(5);

                        int codBalansId = newBalansId + report_id * 500000;
                        var icmd = new SqlCommand("INSERT INTO reports1nf_balans_akt_attachfiles (free_square_id, file_name, file_ext, modified_by, modify_date) VALUES (@balid, @filename, @fileext, @modified_by, @modify_date); SELECT CAST(SCOPE_IDENTITY() AS int)", connectionSql, transactionSql);
                        icmd.Parameters.Add(new SqlParameter("balid", codBalansId));
                        icmd.Parameters.Add(new SqlParameter("filename", file_name));
                        icmd.Parameters.Add(new SqlParameter("fileext", file_ext));
                        icmd.Parameters.Add(new SqlParameter("modified_by", modified_by));
                        icmd.Parameters.Add(new SqlParameter("modify_date", modify_date));
                        var newId = (Int32)icmd.ExecuteScalar();

                        var sourceFileToCopy = Path.Combine(photoRootPath, "transfer_requests_akt_attachfiles", request_id.ToString(), file_name + file_ext);
                        var destFileToCopy = Path.Combine(photoRootPath, "reports1nf_balans_akt_attachfiles", codBalansId.ToString(), file_name + file_ext);
                        if (LLLLhotorowUtils.Exists(sourceFileToCopy, connectionSql, transactionSql))
                        {
                            LLLLhotorowUtils.Copy(sourceFileToCopy, destFileToCopy, connectionSql, transactionSql);
                        }
                    }

                    r.Close();
                }
            }
        }

        private static int CutBalansObject(/*FbConnection connection1NF, FbTransaction transaction,*/ SqlConnection connectionSql, SqlTransaction transactionSql, BalansTransfer bt,
            /*int balansId, decimal cutSquare, int newOwnerOrgId,*/ Document rishennya, ImportedAct act, bool notifyByEmail)
        {
            int balansId = bt.balansId;
            decimal cutSquare = bt.sqr;
            int oldOwnerOrgId = bt.organizationFromId;
            int newOwnerOrgId = bt.organizationToId;

            // Get the current square of object from 1NF
            decimal sqrTotal = -1m;
            decimal sqrNonHabit = -1m;

            //string query = "SELECT SQR_ZAG, SQR_NEJ FROM BALANS_1NF WHERE ID = @balid";

            //using (FbCommand command = new FbCommand(query, connection1NF))
            //{
            //    try
            //    {
            //        command.Parameters.Add(new FbParameter("balid", balansId));
            //        command.Transaction = transaction;

            //        using (FbDataReader reader = command.ExecuteReader())
            //        {
            //            if (reader.Read())
            //            {
            //                sqrTotal = reader.IsDBNull(0) ? -1m : reader.GetDecimal(0);
            //                sqrNonHabit = reader.IsDBNull(1) ? -1m : reader.GetDecimal(1);
            //            }

            //            reader.Close();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Info(ex.Message);
            //        throw;
            //    }
            //}

            using (SqlCommand cmdSource = new SqlCommand("select sqr_non_habit,sqr_total from balans where id = @balid", connectionSql, transactionSql))
            {
                cmdSource.Parameters.AddWithValue("balid", balansId);
                using (SqlDataReader r = cmdSource.ExecuteReader())
                {
                    if (r.Read())
                    {
                        sqrNonHabit = r.IsDBNull(0) ? -1m : r.GetDecimal(0);
                        sqrTotal = r.IsDBNull(1) ? -1m : r.GetDecimal(1);
                    }
                    r.Close();
                }

            }



            int newBalansObj = CopyBalansObject(connectionSql, transactionSql, balansId, newOwnerOrgId, rishennya, act);

            if (cutSquare >= sqrTotal || Math.Abs(sqrTotal - cutSquare) <= 1m)
            {
                System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
                string username = (user == null ? "Auto-import" : user.UserName);
                // The object can be transferred completely
                try
                {
                    //var arch_id = CreateBalansArchiveRecord(/*connection1NF,*/ balansId, newBalansObj/*, transaction*/);

                    ArchiverSql.CreateBalansArchiveRecord(connectionSql, balansId, username, transactionSql);
                }
                catch { }

                TransferArendaAgreements(connectionSql, transactionSql, balansId, newBalansObj, bt.organizationFromId, bt.organizationToId, username);
                TransferDocumentsToNewBalansObject(connectionSql, transactionSql, balansId, newBalansObj);
                //TransferBalansDokProp(/*connection1NF,*/ balansId, newBalansObj/*, transaction*/);
                MarkBalansObjectAsDeleted(connectionSql, transactionSql, balansId, rishennya, act, newOwnerOrgId, notifyByEmail, true, 3);
            }
            else
            {
                // Part of the object must remain at the previous owner
                ChangeBalansObjectSquare(connectionSql, transactionSql, balansId, sqrTotal - cutSquare);
            }

            ChangeBalansObjectSquare(connectionSql, transactionSql, newBalansObj, cutSquare);

            // Add the object to user report
            if (connectionSql != null)
            {
                log.Info("dbo.fnAddBalansObjectToReport" + newBalansObj.ToString());
                using (SqlCommand cmd = new SqlCommand("dbo.fnAddBalansObjectToReport", connectionSql, transactionSql))
                {
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("BALANS_ID", newBalansObj));
                        log.Info("dbo.fnAddBalansObjectToReport " + newBalansObj.ToString());
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        if (notifyByEmail)
                            EmailNotifier.NotifyBalansObjectAdded(connectionSql, transactionSql, newBalansObj);
                    }
                    catch (Exception ex)
                    {
                        log.Info(ex.Message);
                        throw;
                    }
                }
            }

            return newBalansObj;
        }

        private static void TransferArendaAgreements(SqlConnection connectionSql, SqlTransaction transactionSql, int balansId, int newBalansObj, int curOrgId, int newOrgId, string username)
        {
            if (connectionSql == null)
                return;
            log.Info(String.Format("TransferArendaAgreements from {0} to {1}", balansId, newBalansObj));


            int oldReportId = -1;
            using (SqlCommand cmdNewReportId = new SqlCommand("select id from reports1nf where organization_id=@old_org_id", connectionSql, transactionSql))
            {
                cmdNewReportId.Parameters.AddWithValue("old_org_id", curOrgId);
                using (SqlDataReader rdNewReportId = cmdNewReportId.ExecuteReader())
                {
                    if (rdNewReportId.Read())
                    {
                        oldReportId = (int)rdNewReportId["id"];
                    }
                }
            }

            int newReportId = -1;
            using (SqlCommand cmdNewReportId = new SqlCommand("select id from reports1nf where organization_id=@new_org_id", connectionSql, transactionSql))
            {
                cmdNewReportId.Parameters.AddWithValue("new_org_id", newOrgId);
                using (SqlDataReader rdNewReportId = cmdNewReportId.ExecuteReader())
                {
                    if (rdNewReportId.Read())
                    {
                        newReportId = (int)rdNewReportId["id"];
                    }
                }
            }


            //Dictionary<int, int> idListToUpdate = new Dictionary<int, int>();
            List<int> idListToUpdate = new List<int>();

            using (SqlCommand cmdArendaAgreements = new SqlCommand("select * FROM [arenda] an where an.[balans_id] = @balans_id and (is_deleted is null or is_deleted=0)", connectionSql, transactionSql))
            {
                //cmdArendaAgreements.Parameters.Add("org_id", balansId);
                cmdArendaAgreements.Parameters.AddWithValue("balans_id", balansId);

                using (SqlDataReader agreementsReader = cmdArendaAgreements.ExecuteReader())
                {
                    while (agreementsReader.Read())
                    {
                        int agreement_state = (int)agreementsReader["agreement_state"];
                        bool agr_active = agreement_state == 1; // Договір діє
                        bool agr_toxic = agreement_state == 2; // Договір закінчився, але заборгованість не погашено
                        bool agr_continued_by_another = agreement_state == 3; // Договір закінчився, оренда продовжена іншим договором

                        if ((agr_toxic) || (agr_continued_by_another)) // не передаем договор
                            continue;

                        DateTime rent_finish_date = agreementsReader["rent_finish_date"] != System.DBNull.Value ? (DateTime)agreementsReader["rent_finish_date"] : DateTime.MinValue;

                        if ((rent_finish_date != DateTime.MinValue) && (rent_finish_date < DateTime.Now.Date)) // дата окончания договора устарела
                            continue;

                        DateTime rent_actual_finish_date = agreementsReader["rent_actual_finish_date"] != System.DBNull.Value ? (DateTime)agreementsReader["rent_actual_finish_date"] : DateTime.MinValue;
                        if (rent_actual_finish_date != DateTime.MinValue)
                            continue;

                        int id = (int)agreementsReader["id"];
                        //int building_1nf_unique_id = (int)agreementsReader["building_1nf_unique_id"];
                        //idListToUpdate.Add(id, building_1nf_unique_id);
                        idListToUpdate.Add(id);
                    }
                }
            }

            foreach (int id in idListToUpdate)
            {
                ArchiverSql.CreateArendaArchiveRecord(connectionSql, id, username, transactionSql);

                using (SqlCommand cmdUpdateArenda = new SqlCommand("UPDATE [arenda] SET [org_balans_id]= @new_org_id, [balans_id] = @new_balans_id where [id]= @id", connectionSql, transactionSql))
                {
                    cmdUpdateArenda.Parameters.AddWithValue("new_org_id", newOrgId);
                    cmdUpdateArenda.Parameters.AddWithValue("new_balans_id", newBalansObj);
                    cmdUpdateArenda.Parameters.AddWithValue("id", id);
                    cmdUpdateArenda.ExecuteNonQuery();
                }

                if (newReportId > 0)
                {
                    using (SqlCommand cmdUpdateArenda = new SqlCommand("UPDATE [reports1nf_arenda] SET [org_balans_id]= @new_org_id, [balans_id] = @new_balans_id, [report_id] = @new_report_id where [id]= @id", connectionSql, transactionSql))
                    {
                        cmdUpdateArenda.Parameters.AddWithValue("new_org_id", newOrgId);
                        cmdUpdateArenda.Parameters.AddWithValue("new_balans_id", newBalansObj);
                        cmdUpdateArenda.Parameters.AddWithValue("new_report_id", newReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("id", id);
                        cmdUpdateArenda.ExecuteNonQuery();
                    }

                    //using (SqlCommand cmdUpdateArenda = new SqlCommand("UPDATE [reports1nf_buildings] SET report_id = @new_report_id where unique_id = @building_1nf_unique_id AND report_id = @old_report_id", connectionSql, transactionSql))
                    //{
                    //    cmdUpdateArenda.Parameters.AddWithValue("new_report_id", newReportId);
                    //    cmdUpdateArenda.Parameters.AddWithValue("old_report_id", oldReportId);
                    //    cmdUpdateArenda.Parameters.AddWithValue("building_1nf_unique_id", idListToUpdate[id]);
                    //    cmdUpdateArenda.ExecuteNonQuery();
                    //}

                    using (SqlCommand cmdUpdateArenda = new SqlCommand("UPDATE [reports1nf_arenda_decisions] SET report_id = @new_report_id WHERE arenda_id = @id AND report_id = @old_report_id", connectionSql, transactionSql))
                    {
                        cmdUpdateArenda.Parameters.AddWithValue("new_report_id", newReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("old_report_id", oldReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("id", id);
                        cmdUpdateArenda.ExecuteNonQuery();
                    }

                    using (SqlCommand cmdUpdateArenda = new SqlCommand("UPDATE [reports1nf_arenda_notes] SET report_id = @new_report_id WHERE arenda_id = @id AND report_id = @old_report_id", connectionSql, transactionSql))
                    {
                        cmdUpdateArenda.Parameters.AddWithValue("new_report_id", newReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("old_report_id", oldReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("id", id);
                        cmdUpdateArenda.ExecuteNonQuery();
                    }

                    using (SqlCommand cmdUpdateArenda = new SqlCommand("UPDATE [reports1nf_arenda_payments] SET report_id = @new_report_id WHERE arenda_id =  @id AND report_id = @old_report_id", connectionSql, transactionSql))
                    {
                        cmdUpdateArenda.Parameters.AddWithValue("new_report_id", newReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("old_report_id", oldReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("id", id);
                        cmdUpdateArenda.ExecuteNonQuery();
                    }

                    using (SqlCommand cmdUpdateArenda = new SqlCommand("UPDATE [reports1nf_payment_documents] SET report_id = @new_report_id WHERE arenda_id =  @id AND report_id = @old_report_id", connectionSql, transactionSql))
                    {
                        cmdUpdateArenda.Parameters.AddWithValue("new_report_id", newReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("old_report_id", oldReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("id", id);
                        cmdUpdateArenda.ExecuteNonQuery();
                    }

                    using (SqlCommand cmdUpdateArenda = new SqlCommand("UPDATE [reports1nf_comments] SET report_id = @new_report_id WHERE arenda_id =  @id AND report_id = @old_report_id", connectionSql, transactionSql))
                    {
                        cmdUpdateArenda.Parameters.AddWithValue("new_report_id", newReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("old_report_id", oldReportId);
                        cmdUpdateArenda.Parameters.AddWithValue("id", id);
                        cmdUpdateArenda.ExecuteNonQuery();
                    }

                }
            }
        }


        private static void TransferDocumentsToNewBalansObject(SqlConnection connectionSql, SqlTransaction transactionSql, int oldBalansId, int newBalansId)
        {

            if (connectionSql != null)
            {
                try
                {
                    string query = @"UPDATE balans_docs
                                 SET balans_id = @new_balans_id
                                 WHERE balans_id = @old_balans_id";

                    using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("old_balans_id", oldBalansId));
                        cmd.Parameters.Add(new SqlParameter("new_balans_id", newBalansId));
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    log.Info(ex.Message);
                    throw;
                }
            }
        }

        //private static void TransferBalansDokProp(/*FbConnection connection1NF,*/ int oldBalansId, int newBalansId/*, FbTransaction transaction*/)
        //{
        //    var query = "UPDATE balans_dok_prop SET BALID = @new_id WHERE BALID = @old_id";
        //    using (FbCommand command = new FbCommand(query, connection1NF))
        //    {
        //        command.Transaction = transaction;
        //        command.Parameters.Add("new_id", newBalansId);
        //        command.Parameters.Add("old_id", oldBalansId);
        //        command.ExecuteNonQuery();
        //    }
        //}


        /// <summary>
        /// Creates a new archive record for Balans table entry
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        /// <param name="balansId1NF">ID of the balans entry in 1NF</param>
        /// <param name="transaction">Transaction (may be 'null')</param>
        /// <returns>ID of the generated archive entry</returns>
//        private static int CreateBalansArchiveRecord(FbConnection connection1NF, int balansId1NF, int newBalansId1NF, FbTransaction transaction)
//        {
//            int existingArchRecordId = -1;
//            object archRecordPrev = null;
//            object archRecordNext = null;

//            // try to get the value from the table
//            using (FbCommand command = new FbCommand("SELECT ID, ARCH_KOD FROM BALANS_1NF WHERE ID = " + balansId1NF.ToString(), connection1NF))
//            {
//                command.Transaction = transaction;

//                using (FbDataReader reader = command.ExecuteReader())
//                {
//                    if (reader.Read())
//                    {
//                        if (!reader.IsDBNull(1))
//                            existingArchRecordId = reader.GetInt32(1);
//                    }

//                    reader.Close();
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
//            int newArchEntryId = GenerateNewFirebirdId(connection1NF, "GEN_TAR_BALANS_1NF_ID", transaction);

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

//            query = "UPDATE TAR_BALANS_1NF SET ID = @new_id WHERE ID = @old_id";
//            using (FbCommand command = new FbCommand(query, connection1NF))
//            {
//                command.Transaction = transaction;
//                command.Parameters.Add("new_id", newBalansId1NF);
//                command.Parameters.Add("old_id", balansId1NF);
//                command.ExecuteNonQuery();
//            }

//            return newArchEntryId;
//        }

        #endregion (Document export)

        #region Implementation

//        private static void LoadOrganizationsFrom1NF(FbConnection connection)
//        {
//            if (connection == null)
//                return;

//            string query = @"SELECT KOD_OBJ, KOD_ZKPO, FULL_NAME_OBJ, SHORT_NAME_OBJ, KOD_GALUZ, KOD_VID_DIAL, KOD_STATUS, KOD_ORG_FORM,
//                KOD_FORM_GOSP, KOD_FORM_VLASN, NAME_UL, NOMER_DOMA, NOMER_KORPUS, POST_INDEX, ADDITIONAL_ADRESS, KOD_RAYON2,
//                FIO_BOSS, TEL_BOSS, FIO_BUH, TEL_BUH, TELEFAX FROM SORG_1NF WHERE KOD_STAN = 1 AND (DELETED IS NULL OR DELETED = 0)";

//            using (FbCommand cmd = new FbCommand(query, connection))
//            {
//                using (FbDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        if (!reader.IsDBNull(0))
//                        {
//                            int orgId = reader.GetInt32(0);
//                            Organization1NF org = null;

//                            if (!organizations1NF.TryGetValue(orgId, out org))
//                            {
//                                org = new Organization1NF();
//                                organizations1NF.Add(orgId, org);
//                            }

//                            org.organizationId = reader.GetInt32(0);
//                            org.zkpo = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim().ToUpper();
//                            org.fullName = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
//                            org.shortName = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim().ToUpper();
//                            org.industryId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
//                            org.occupationId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
//                            org.statusId = reader.IsDBNull(6) ? -1 : reader.GetInt32(6);
//                            org.orgTypeId = reader.IsDBNull(7) ? -1 : reader.GetInt32(7);
//                            org.formGospId = reader.IsDBNull(8) ? -1 : reader.GetInt32(8);
//                            org.ownershipFormId = reader.IsDBNull(9) ? -1 : reader.GetInt32(9);
//                            org.addrStreet = reader.IsDBNull(10) ? "" : reader.GetString(10).Trim().ToUpper();
//                            org.addrNomer = reader.IsDBNull(11) ? "" : reader.GetString(11).Trim().ToUpper();
//                            org.addrKorpus = reader.IsDBNull(12) ? "" : reader.GetString(12).Trim().ToUpper();
//                            org.addrZipCode = reader.IsDBNull(13) ? "" : reader.GetString(13).Trim().ToUpper();
//                            org.addrDistrictId = reader.IsDBNull(15) ? -1 : reader.GetInt32(15);
//                            org.directorFIO = reader.IsDBNull(16) ? "" : reader.GetString(16).Trim().ToUpper();
//                            org.directorTel = reader.IsDBNull(17) ? "" : reader.GetString(17).Trim().ToUpper();
//                            org.buhgalterFIO = reader.IsDBNull(18) ? "" : reader.GetString(18).Trim().ToUpper();
//                            org.buhgalterTel = reader.IsDBNull(19) ? "" : reader.GetString(19).Trim().ToUpper();
//                            org.fax = reader.IsDBNull(20) ? "" : reader.GetString(20).Trim().ToUpper();

//                            string addrMisc = reader.IsDBNull(14) ? "" : reader.GetString(14).Trim().ToUpper();

//                            if (addrMisc.Length > 0)
//                                org.addrNomer = addrMisc;
//                        }
//                    }

//                    reader.Close();
//                }
//            }
//        }

        //private static void GetObjectFromINF(FbConnection connection, int object1NFId)
        //{
        //    using (FbCommand cmd = new FbCommand("SELECT OBJECT_KOD, FULL_ULNAME, NOMER1, NOMER2, NOMER3, ADRDOP, TEXSTAN, " +
        //        "BUDYEAR, NEWDISTR, TYPOBJ, VIDOBJ, ULKOD, SZAG FROM OBJECT_1NF WHERE DELETED = 0 ORDER BY OBJECT_KOD", connection))
        //    {
        //        using (FbDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                if (!reader.IsDBNull(0))
        //                {
        //                    Object1NF obj = new Object1NF();

        //                    obj.objectId = reader.GetInt32(0);
        //                    obj.streetName = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim().ToUpper();
        //                    obj.addrNomer1 = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
        //                    obj.addrNomer2 = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim().ToUpper();
        //                    obj.addrNomer3 = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim().ToUpper();
        //                    obj.addrMisc = reader.IsDBNull(5) ? "" : reader.GetString(5).Trim().ToUpper();
        //                    obj.techStateId = reader.IsDBNull(6) ? null : reader.GetValue(6);
        //                    obj.buildYear = reader.IsDBNull(7) ? null : reader.GetValue(7);
        //                    obj.districtId = reader.IsDBNull(8) ? null : reader.GetValue(8);
        //                    obj.objTypeId = reader.IsDBNull(9) ? null : reader.GetValue(9);
        //                    obj.objKindId = reader.IsDBNull(10) ? null : reader.GetValue(10);
        //                    obj.streetId = reader.IsDBNull(11) ? -1 : reader.GetInt32(11);
        //                    obj.totalSqr = reader.IsDBNull(12) ? null : reader.GetValue(12);

        //                    objects1NF.Add(obj.objectId, obj);
        //                }
        //            }

        //            reader.Close();
        //        }
        //    }
        //}

        //private static void LoadOneBalansObjectFrom1NF(FbConnection connection, int object1NFId)
        //{
        //    try
        //    {
        //        using (FbCommand cmd = new FbCommand("SELECT OBJECT_KOD, FULL_ULNAME, NOMER1, NOMER2, NOMER3, ADRDOP, TEXSTAN, " +
        //            "BUDYEAR, NEWDISTR, TYPOBJ, VIDOBJ, ULKOD, SZAG FROM OBJECT_1NF WHERE DELETED = 0 AND OBJECT_KOD = @ID", connection))
        //        {
        //            cmd.Parameters.Add(new FbParameter("ID", object1NFId));
        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    if (!reader.IsDBNull(0))
        //                    {
        //                        Object1NF obj = new Object1NF();

        //                        obj.objectId = reader.GetInt32(0);
        //                        obj.streetName = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim().ToUpper();
        //                        obj.addrNomer1 = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
        //                        obj.addrNomer2 = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim().ToUpper();
        //                        obj.addrNomer3 = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim().ToUpper();
        //                        obj.addrMisc = reader.IsDBNull(5) ? "" : reader.GetString(5).Trim().ToUpper();
        //                        obj.techStateId = reader.IsDBNull(6) ? null : reader.GetValue(6);
        //                        obj.buildYear = reader.IsDBNull(7) ? null : reader.GetValue(7);
        //                        obj.districtId = reader.IsDBNull(8) ? null : reader.GetValue(8);
        //                        obj.objTypeId = reader.IsDBNull(9) ? null : reader.GetValue(9);
        //                        obj.objKindId = reader.IsDBNull(10) ? null : reader.GetValue(10);
        //                        obj.streetId = reader.IsDBNull(11) ? -1 : reader.GetInt32(11);
        //                        obj.totalSqr = reader.IsDBNull(12) ? null : reader.GetValue(12);

        //                        objects1NF.Add(obj.objectId, obj);
        //                    }
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //    }
        //}

        //private static void LoadBalansObjectsFrom1NF(FbConnection connection)
        //{
        //    balans1NFByID.Clear();
        //    balans1NFByAddress.Clear();

        //    using (FbCommand cmd = new FbCommand("SELECT ID, OBJECT, ORG, SQR_ZAG, GRPURP, PURPOSE, PURP_STR FROM BALANS_1NF WHERE (DELETED = 0) OR (DELETED IS NULL)", connection))
        //    {
        //        using (FbDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                if (!reader.IsDBNull(0))
        //                {
        //                    BalansObject1NF obj = new BalansObject1NF();

        //                    obj.balansId = reader.GetInt32(0);
        //                    obj.objectId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
        //                    obj.organizationId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
        //                    obj.sqr = reader.IsDBNull(3) ? 0m : reader.GetDecimal(3);
        //                    obj.purposeGroupId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
        //                    obj.purposeId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
        //                    obj.purpose = reader.IsDBNull(6) ? "" : reader.GetString(6).Trim().ToUpper();

        //                    // Balans objects are stored by object ID
        //                    Dictionary<int, BalansObject1NF> storage = null;

        //                    if (!balans1NFByAddress.TryGetValue(obj.objectId, out storage))
        //                    {
        //                        storage = new Dictionary<int, BalansObject1NF>();

        //                        balans1NFByAddress.Add(obj.objectId, storage);
        //                    }

        //                    storage.Add(obj.balansId, obj);
        //                    balans1NFByID.Add(obj.balansId, obj);
        //                }
        //            }

        //            reader.Close();
        //        }
        //    }
        //}

        //private static void LoadDocumentsFromNJF(FbConnection connection)
        //{
        //    try
        //    {
        //        using (FbCommand cmd = new FbCommand("SELECT DOK_ID, DOKKIND, DOKDATA, DOKNUM, DOKTEMA FROM ROZP_DOK WHERE NOT (DOK_ID IS NULL)", connection))
        //        {
        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    if (!reader.IsDBNull(0) && !reader.IsDBNull(2) && !reader.IsDBNull(3))
        //                    {
        //                        Document doc = new Document();

        //                        doc.documentId = reader.GetInt32(0);
        //                        doc.documentKind = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
        //                        doc.documentDate = reader.GetDateTime(2).Date;
        //                        doc.documentNumber = reader.GetString(3);
        //                        doc.documentTitle = reader.IsDBNull(4) ? "" : reader.GetString(4);

        //                        doc.documentNumber = doc.documentNumber.Trim().ToUpper();
        //                        doc.documentTitle = doc.documentTitle.Trim().ToUpper();

        //                        documentsNJF.Add(doc.documentId, doc);
        //                    }
        //                }

        //                reader.Close();
        //            }
        //        }

        //        using (FbCommand cmd = new FbCommand("SELECT DOK_ID, DOKKIND, DOKDATA, DOKNUM, DOKTEMA FROM ACTS WHERE NOT (DOK_ID IS NULL)", connection))
        //        {
        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    if (!reader.IsDBNull(0) && !reader.IsDBNull(2) && !reader.IsDBNull(3))
        //                    {
        //                        Document doc = new Document();

        //                        doc.documentId = reader.GetInt32(0);
        //                        doc.documentKind = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
        //                        doc.documentDate = reader.GetDateTime(2).Date;
        //                        doc.documentNumber = reader.GetString(3);
        //                        doc.documentTitle = reader.IsDBNull(4) ? "" : reader.GetString(4);

        //                        doc.documentNumber = doc.documentNumber.Trim().ToUpper();
        //                        doc.documentTitle = doc.documentTitle.Trim().ToUpper();

        //                        actsNJF.Add(doc.documentId, doc);
        //                    }
        //                }

        //                reader.Close();
        //            }
        //        }

        //        using (FbCommand cmd = new FbCommand("SELECT DOK_KOD1, DOK_KOD2 FROM DOK_DEPEND WHERE NOT (DOK_KOD1 IS NULL) AND NOT (DOK_KOD2 IS NULL)", connection))
        //        {
        //            using (FbDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    int childId = reader.GetInt32(0);
        //                    int parentId = reader.GetInt32(1);

        //                    Document parentDoc = null;

        //                    if (documentsNJF.TryGetValue(parentId, out parentDoc))
        //                    {
        //                        if (parentDoc.dependentDocuments == null)
        //                        {
        //                            parentDoc.dependentDocuments = new List<int>();
        //                        }

        //                        if (!parentDoc.dependentDocuments.Contains(childId))
        //                        {
        //                            parentDoc.dependentDocuments.Add(childId);
        //                        }
        //                    }
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Info(ex.Message);
        //    }
        //}

        //private static Dictionary<int, Organization1NF> LoadOrganizationsFromNJF(FbConnection connection)
        //{
        //    Dictionary<int, Organization1NF> orgList = new Dictionary<int, Organization1NF>();

        //    using (FbCommand cmd = new FbCommand("SELECT KOD, STAN, ZKPO, NAME, NAMEP FROM SZKPO ORDER BY KOD, STAN", connection))
        //    {
        //        using (FbDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                int organizationId = reader.GetInt32(0);
        //                Organization1NF org = null;

        //                if (!orgList.TryGetValue(organizationId, out org))
        //                {
        //                    org = new Organization1NF();
        //                    orgList.Add(organizationId, org);
        //                }

        //                org.organizationId = reader.GetInt32(0);
        //                org.zkpo = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
        //                org.shortName = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim().ToUpper();
        //                org.fullName = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim().ToUpper();
        //            }

        //            reader.Close();
        //        }
        //    }

        //    return orgList;
        //}

        //public static int FindDistrictMatchInNJF(string districtName)
        //{
        //    DictionaryData data = null;

        //    if (dictionaries.TryGetValue(DICT_DISTRICTS, out data))
        //    {
        //        // Find a district in NJF by name
        //        districtName = districtName.Trim().ToUpper();

        //        foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesSql)
        //        {
        //            if (pair.Value.value == districtName)
        //            {
        //                return pair.Key;
        //            }
        //        }
        //    }

        //    return -1;
        //}

        //public static int FindStreetMatchInNJF(string streetName/*, FbConnection connectionNJF, FbTransaction transaction*/)
        //{
        //    DictionaryData data = null;

        //    if (dictionaries.TryGetValue(DICT_STREETS, out data))
        //    {
        //        streetName = streetName.Trim().ToUpper();

        //        // Find a street in NJF by name
        //        int streetIdNJF = -1;

        //        if (data.KeysSql.TryGetValue(streetName, out streetIdNJF))
        //        {
        //            return streetIdNJF;
        //        }

        //        // Get the ID of this street in 1NF
        //        int streetId1NF = -1;

        //        if (data.KeysSql.TryGetValue(streetName, out streetId1NF))
        //        {
        //            // Try to find a manual mapping
        //            foreach (KeyValuePair<int, int> pair in manualStreetCodeMappingNJFto1NF)
        //            {
        //                if (pair.Value == streetId1NF)
        //                {
        //                    return pair.Key;
        //                }
        //            }
        //        }

        //        // We need to create this street in NJF database
        //        // There is no Generator for this table, so we just have to find the maximum existing Id
        //        int newStreetIdNJF = data.MaxKeySql + 1;

        //        using (FbCommand cmd = new FbCommand("INSERT INTO SUL (KOD, NAME) VALUES (@sid, @nm)", connectionNJF))
        //        {
        //            if (streetName.Length > 160)
        //                streetName = streetName.Substring(0, 160);

        //            cmd.Parameters.Add(new FbParameter("sid", newStreetIdNJF));
        //            cmd.Parameters.Add(new FbParameter("nm", streetName));

        //            cmd.Transaction = transaction;
        //            cmd.ExecuteNonQuery();
        //        }

        //        // Update our cached dictionary
        //        data.MaxKeySql = newStreetIdNJF;
        //        data.KeysSql[streetName] = newStreetIdNJF;
        //        data.ValuesSql[newStreetIdNJF] = new DictionaryValue(newStreetIdNJF, streetName);

        //        return newStreetIdNJF;
        //    }

        //    return -1;
        //}

        //public static int FindObjectMatchInNJF(/*FbConnection connectionNJF, FbTransaction transaction,*/
        //    Object1NF obj, ObjectFinder objectFinder)
        //{

        //    bool addressIsSimple = false;
        //    bool similarAddressExists = false;

        //    int existingId = objectFinder.FindObject(obj.streetName, "", obj.addrNomer1, obj.addrNomer2, obj.addrNomer3,
        //        obj.addrMisc, null, null, out addressIsSimple, out similarAddressExists);

        //    if (existingId > 0)
        //    {
        //        return existingId;
        //    }

        //    // Get the district name
        //    DictionaryValue district = new DictionaryValue();

        //    if (obj.districtId is int)
        //    {
        //        DictionaryData data = null;

        //        if (dictionaries.TryGetValue(DB.DICT_DISTRICTS, out data))
        //        {
        //            if (!data.ValuesSql.TryGetValue((int)obj.districtId, out district))
        //            {
        //                district = new DictionaryValue();
        //            }
        //        }
        //    }

        //    int streetId = -1;
        //    int districtId = -1;

        //    existingId = CreateNewNJFObject(/*connectionNJF, transaction,*/ obj.streetName, district.value,
        //        obj.addrNomer1, obj.addrNomer2, obj.addrNomer3, obj.addrMisc, out streetId, out districtId);

        //    // Register the object in the Object Finder
        //    objectFinder.AddObjectToCache(existingId, obj.streetName, null, obj.addrNomer1, obj.addrNomer2, obj.addrNomer3,
        //        obj.addrMisc, districtId > 0 ? (object)districtId : null, streetId, null, 0, obj.totalSqr, null, null, 1);

        //    return existingId;
        //}

        //public static int FindOrganizationMatchInNJF(/*FbConnection connectionNJF, FbTransaction transaction,*/
        //    Organization1NF org, OrganizationFinder orgFinder)
        //{
        //    bool categorized = false;

        //    int existingOrgId = orgFinder.FindOrganization(org.zkpo, org.fullName, org.shortName, false, out categorized);

        //    if (existingOrgId > 0)
        //    {
        //        return existingOrgId;
        //    }

        //    // Get the street id in NJF by street name
        //    int streetId = FindStreetMatchInNJF(org.addrStreet/*, connectionNJF, transaction*/);

        //    // Get the district id in NJF by district name
        //    int? districtId = null;
        //    DictionaryValue district = new DictionaryValue();

        //    if (org.addrDistrictId > 0)
        //    {
        //        DictionaryData data = null;

        //        if (dictionaries.TryGetValue(DB.DICT_DISTRICTS, out data))
        //        {
        //            if (!data.ValuesSql.TryGetValue(org.addrDistrictId, out district))
        //            {
        //                district = new DictionaryValue();
        //            }
        //        }
        //    }

        //    if (district.value.Length > 0)
        //    {
        //        districtId = FindDistrictMatchInNJF(district.value);
        //    }

        //    // Determine organization status in NJF
        //    int statusId = 2;

        //    if (org.statusId == 2)
        //        statusId = 1;

        //    int industryId = MatchDictionary1NFtoNJF(DB.DICT_ORG_INDUSTRY, org.industryId);
        //    int ownershipId = MatchOrgOwnership1NFtoNJF(org.ownershipFormId);
        //    int formGospId = MatchOrgFormGosp1NFtoNJF(org.formGospId);
        //    int orgTypeId = MatchDictionary1NFtoNJF(DB.DICT_ORG_TYPE, org.orgTypeId);

        //    existingOrgId = CreateNewNJFOrganization(connectionNJF, transaction,
        //        org.zkpo,
        //        org.fullName,
        //        org.shortName,
        //        districtId,
        //        streetId,
        //        org.addrNomer,
        //        "",
        //        "",
        //        null,
        //        org.addrZipCode,
        //        industryId,
        //        ownershipId,
        //        formGospId,
        //        orgTypeId,
        //        statusId,
        //        org.directorFIO,
        //        org.directorTel,
        //        org.buhgalterFIO,
        //        org.buhgalterTel,
        //        org.fax);

        //    // Register organization in the Organization Finder
        //    orgFinder.AddOrganizationToCache(existingOrgId, org.zkpo, org.fullName, org.shortName,
        //        industryId > 0 ? (object)industryId : null,
        //        null,
        //        ownershipId > 0 ? (object)ownershipId : null);

        //    return existingOrgId;
        //}

        //private static int Get1NFNewOrganizationId(FbConnection connection1NF, FbTransaction transaction)
        //{
        //    int seed = -1;

        //    string query = "select max(kod_obj) from sorg_1nf where kod_obj < 130000";

        //    using (FbCommand command = new FbCommand(query, connection1NF))
        //    {
        //        command.Transaction = transaction;

        //        using (FbDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                object dataSeed = reader.IsDBNull(0) ? null : reader.GetValue(0);

        //                if (dataSeed is int)
        //                {
        //                    seed = (int)dataSeed + 1;
        //                }
        //            }

        //            reader.Close();
        //        }
        //    }

        //    if (seed < 0)
        //    {
        //        seed = GenerateNewFirebirdId(connection1NF, "SORG_1NF_GEN", transaction);
        //    }

        //    return seed;
        //}

        //private static void RemoveOldDistricts()
        //{
        //    DictionaryData dictDistricts = null;

        //    if (dictionaries.TryGetValue(DICT_DISTRICTS, out dictDistricts))
        //    {
        //        List<int> keysToRemove = dictDistricts.ValuesSql.Keys.Where(i => i >= 900).ToList();

        //        foreach (int key in keysToRemove)
        //        {
        //            dictDistricts.ValuesSql.Remove(key);
        //        }
        //    }
        //}


        #endregion (Implementation)
    }

    //public class Preferences
    //{
    //    #region Preferences

    //    /// <summary>
    //    /// Name of the folder that contains global configuration settings
    //    /// </summary>
    //    //public const string iniFolderName = "K:\\ITG\\ToolSettings";

    //    /// <summary>
    //    /// Name of the INI file with connection settings
    //    /// </summary>
    //    //public const string iniConnFileName = "ConnectionSettings.ini";

    //    /// <summary>
    //    /// Name of the INI file with Users database connection settings
    //    /// </summary>
    //    //public const string iniUsersFileName = "Users.ini";

    //    /// <summary>
    //    /// Name of the INI file with NJF database connection settings
    //    /// </summary>
    //    //public const string iniNJFFileName = "NJF.ini";

    //    /// <summary>
    //    /// User login to the NJF database
    //    /// </summary>
    //    //public string userNameNJF = "SYSDBA";

    //    /// <summary>
    //    /// User password to the NJF database
    //    /// </summary>
    //    //public string userPasswordNJF = "masterkey";

    //    /// <summary>
    //    /// User login to the Users database
    //    /// </summary>
    //    //public string userNameUsers = "SYSDBA";

    //    /// <summary>
    //    /// User password to the Users database
    //    /// </summary>
    //    //public string userPasswordUsers = "masterkey";

    //    /// <summary>
    //    /// User login to the 1NF database
    //    /// </summary>
    //    //public string userName1NF = "SYSDBA";

    //    /// <summary>
    //    /// User password to the 1NF database
    //    /// </summary>
    //    //public string userPassword1NF = "masterkey";

    //    /// <summary>
    //    /// User login to the Sql Server
    //    /// </summary>
    //    //public string userNameSql = "sa";

    //    /// <summary>
    //    /// User password to the Sql Server
    //    /// </summary>
    //    //public string userPasswordSql = "13579";

    //    /// <summary>
    //    /// Server name of the NJF connection string
    //    /// </summary>
    //    //public string serverNameNJF = "gukv_1nf";

    //    /// <summary>
    //    /// Database name of the NJF connection string
    //    /// </summary>
    //    //public string databaseNameNJF = "c:\\database\\1nf_reserve\\newnjf.gdb";

    //    /// <summary>
    //    /// Server name of the Users database connection string
    //    /// </summary>
    //    //public string serverNameUsers = "gukv";

    //    /// <summary>
    //    /// Database name of the Users database connection string
    //    /// </summary>
    //    //public string databaseNameUsers = "d:\\database\\users\\users.gdb";

    //    /// <summary>
    //    /// Server name of the 1NF database connection string
    //    /// </summary>
    //    //public string serverName1NF = "gukv_1nf";

    //    /// <summary>
    //    /// Database name of the 1NF database connection string
    //    /// </summary>
    //    //public string databaseName1NF = "c:\\database\\1nf_reserve\\1nf2.gdb";

    //    /// <summary>
    //    /// Server name of the SQL Server connection string
    //    /// </summary>
    //    //public string serverNameSql = "localhost";

    //    /// <summary>
    //    /// Database name of the SQL Server connection string
    //    /// </summary>
    //    //public string databaseNameSql = "GUKVTest";

    //    #endregion (Preferences)

    //    #region Construction

    //    /// <summary>
    //    /// Default constructor
    //    /// </summary>
    //    public Preferences()
    //    {
    //    }

    //    #endregion (Construction)

    //    #region Database connection string support

    //    /// <summary>
    //    /// Builds a valid Firebird connection string from the given DB conection settings
    //    /// </summary>
    //    /// <param name="serverName">Name of the Firebird server</param>
    //    /// <param name="databaseName">Database name on the server</param>
    //    /// <param name="userName">User name</param>
    //    /// <param name="userPassword">User password</param>
    //    /// <returns>Created connection string</returns>
    //    //private string GetFirebirdConnectionString(
    //    //    string serverName,
    //    //    string databaseName,
    //    //    string userName,
    //    //    string userPassword)
    //    //{
    //    //    return "Server=" + serverName + ";" +
    //    //        "Database=" + databaseName + ";" +
    //    //        "User=" + userName + ";" +
    //    //        "Password=" + userPassword + ";" +
    //    //        "Charset=WIN1251";
    //    //}

    //    /// <summary>
    //    /// Builds a valid SQL Client connection string from the given DB conection settings
    //    /// </summary>
    //    /// <param name="server">Server name</param>
    //    /// <param name="database">Database name</param>
    //    /// <param name="user">User name</param>
    //    /// <param name="password">User password</param>
    //    /// <returns>Created connection string</returns>
    //    //private string GetSQLClientConnectionString(string server, string database,
    //    //    string user, string password)
    //    //{
    //    //    return ConfigurationManager.ConnectionStrings["GUKVConnectionString"].ConnectionString;
    //    //    /*return "Data Source=localhost;Initial Catalog=GUKV;Integrated Security=True";
    //    //    return
    //    //        "Server=" + server + ";" +
    //    //        "Database=" + database + ";" +
    //    //        "UID=" + user + ";" +
    //    //        "PWD=" + password;*/
    //    //}

    //    /// <summary>
    //    /// Builds a connection string for NJF database
    //    /// </summary>
    //    /// <returns>Connection string for NJF database</returns>
    //    //public string GetNJFConnectionString(string userName, string password)
    //    //{
    //    //    return ConfigurationManager.ConnectionStrings["NJFconnectionString"].ConnectionString;
    //    //    /*
    //    //    return GetFirebirdConnectionString(
    //    //        serverNameNJF,
    //    //        databaseNameNJF,
    //    //        userName,
    //    //        password);*/
    //    //}

    //    /// <summary>
    //    /// Builds a connection string for 1NF database
    //    /// </summary>
    //    /// <returns>Connection string for 1NF database</returns>
    //    //public string Get1NFConnectionString()
    //    //{
    //    //    return ConfigurationManager.ConnectionStrings["1NFconnectionString"].ConnectionString;
    //    //    /*
    //    //    return GetFirebirdConnectionString(
    //    //        serverName1NF,
    //    //        databaseName1NF,
    //    //        userName1NF,
    //    //        userPassword1NF);*/
    //    //}

    //    /// <summary>
    //    /// Builds a connection string for Users database
    //    /// </summary>
    //    /// <returns>Connection string for Users database</returns>
    //    //public string GetUsersConnectionString()
    //    //{
    //    //    return GetFirebirdConnectionString(
    //    //        serverNameUsers,
    //    //        databaseNameUsers,
    //    //        userNameUsers,
    //    //        userPasswordUsers);
    //    //}

    //    /// <summary>
    //    /// Builds a connection string to the SQL Server
    //    /// </summary>
    //    /// <returns>Connection string to the SQL Server</returns>
    //    //public string GetSqlServerConnectionString()
    //    //{
    //    //    return GetSQLClientConnectionString(
    //    //        serverNameSql,
    //    //        databaseNameSql,
    //    //        userNameSql,
    //    //        userPasswordSql);
    //    //}

    //    #endregion (Database connection string support)

    //    #region INI file support

    //    /// <summary>
    //    /// Reads preferences from the INI file
    //    /// </summary>
    //    /// <returns>TRUE if the INI file was located and parsed</returns>
    //    //public bool ReadConnPreferencesFromFile()
    //    //{
    //    //    if (!ReadConnPreferencesFromFile(iniNJFFileName))
    //    //    {
    //    //        return false;
    //    //    }

    //    //    if (!ReadConnPreferencesFromFile(iniUsersFileName))
    //    //    {
    //    //        return false;
    //    //    }

    //    //    if (!ReadConnPreferencesFromFile(iniConnFileName))
    //    //    {
    //    //        return false;
    //    //    }

    //    //    return true;
    //    //}

    //    /// <summary>
    //    /// Reads the connection preferences from the specified file
    //    /// </summary>
    //    /// <param name="fileName"></param>
    //    /// <returns></returns>
    //    //private bool ReadConnPreferencesFromFile(string fileName)
    //    //{
    //    //    // Try local file first
    //    //    if (File.Exists(fileName))
    //    //    {
    //    //        using (StreamReader reader = new StreamReader(fileName, Encoding.ASCII))
    //    //        {
    //    //            ReadPreferencesFromStream(reader);

    //    //            reader.Close();
    //    //        }

    //    //        return true;
    //    //    }

    //    //    // Try the global path
    //    //    if (File.Exists(iniFolderName + "\\" + fileName))
    //    //    {
    //    //        using (StreamReader reader = new StreamReader(iniFolderName + "\\" + fileName, Encoding.ASCII))
    //    //        {
    //    //            ReadPreferencesFromStream(reader);

    //    //            reader.Close();
    //    //        }

    //    //        return true;
    //    //    }

    //    //    return false;
    //    //}

    //    /// <summary>
    //    /// Reads connection preferences from the provided ASCII file stream
    //    /// </summary>
    //    /// <param name="reader">The input stream</param>
    //    //private void ReadPreferencesFromStream(StreamReader reader)
    //    //{
    //    //    string line = "";

    //    //    string tempServerName = "";
    //    //    string tempDatabaseName = "";
    //    //    string tempUserName = "";
    //    //    string tempUserPwd = "";

    //    //    while (!reader.EndOfStream)
    //    //    {
    //    //        line = line.Trim().ToUpper();

    //    //        if (line.Length == 0)
    //    //        {
    //    //            line = reader.ReadLine().Trim().ToUpper();
    //    //        }

    //    //        // Skip comments
    //    //        if (line.Length > 0 && !line.StartsWith(";"))
    //    //        {
    //    //            // Remove the section header braces
    //    //            if (line.StartsWith("["))
    //    //            {
    //    //                line = line.Remove(0, 1);
    //    //            }

    //    //            if (line.EndsWith("]"))
    //    //            {
    //    //                line = line.Remove(line.Length - 1);
    //    //            }

    //    //            // Parse the section header
    //    //            line = line.Trim();

    //    //            //if (line == "NJF")
    //    //            //{
    //    //            //    line = ReadSectionFromStream(reader,
    //    //            //        ref serverNameNJF, ref databaseNameNJF,
    //    //            //        ref userNameNJF, ref userPasswordNJF);
    //    //            //}
    //    //            //else if (line == "USERS")
    //    //            //{
    //    //            //    line = ReadSectionFromStream(reader,
    //    //            //        ref serverNameUsers, ref databaseNameUsers,
    //    //            //        ref userNameUsers, ref userPasswordUsers);
    //    //            //}
    //    //            //else if (line == "1NF")
    //    //            //{
    //    //            //    line = ReadSectionFromStream(reader,
    //    //            //        ref serverName1NF, ref databaseName1NF,
    //    //            //        ref userName1NF, ref userPassword1NF);
    //    //            //}
    //    //            //else 
    //    //            if (line == "GUKV")
    //    //            {
    //    //                line = ReadSectionFromStream(reader,
    //    //                    ref serverNameSql, ref databaseNameSql,
    //    //                    ref userNameSql, ref userPasswordSql);
    //    //            }
    //    //            else
    //    //            {
    //    //                line = ReadSectionFromStream(reader,
    //    //                    ref tempServerName, ref tempDatabaseName,
    //    //                    ref tempUserName, ref tempUserPwd);
    //    //            }
    //    //        }
    //    //    }
    //    //}

    //    /// <summary>
    //    /// Reads preferences from particular section of an INI file.
    //    /// </summary>
    //    /// <param name="reader">The input stream</param>
    //    /// <param name="serverName">If server name is specified in the section
    //    /// preferences, it is returned in this variable</param>
    //    /// <param name="databaseName">If database name is specified in the section
    //    /// preferences, it is returned in this variable</param>
    //    /// <param name="userName">If user name is specified in the section
    //    /// preferences, it is returned in this variable</param>
    //    /// <param name="userPassword">If user password is specified in the section
    //    /// preferences, it is returned in this variable</param>
    //    /// <returns>The first line of the INI file after the parsed section</returns>
    //    //private string ReadSectionFromStream(StreamReader reader,
    //    //    ref string serverName, ref string databaseName,
    //    //    ref string userName, ref string userPassword)
    //    //{
    //    //    char[] separator = new char[1];
    //    //    separator[0] = '=';

    //    //    while (!reader.EndOfStream)
    //    //    {
    //    //        string line = reader.ReadLine().Trim();

    //    //        if (line.Length > 0)
    //    //        {
    //    //            // If another section is reached, stop processing
    //    //            if (line.StartsWith("["))
    //    //            {
    //    //                return line;
    //    //            }

    //    //            // Skip comments
    //    //            if (!line.StartsWith(";"))
    //    //            {
    //    //                string[] statements = line.Split(separator);

    //    //                if (statements.Length == 2)
    //    //                {
    //    //                    string param = statements[0].Trim().ToUpper();
    //    //                    string value = statements[1].Trim();

    //    //                    if (param == "SERVER")
    //    //                    {
    //    //                        serverName = value;
    //    //                    }
    //    //                    else if (param == "DATABASE")
    //    //                    {
    //    //                        databaseName = value;
    //    //                    }
    //    //                    else if (param == "LOGIN")
    //    //                    {
    //    //                        userName = value;
    //    //                    }
    //    //                    else if (param == "PASSWORD")
    //    //                    {
    //    //                        userPassword = value;
    //    //                    }
    //    //                }
    //    //            }
    //    //        }
    //    //    }

    //    //    return "";
    //    //}

    //    #endregion (INI file support)
    //}

    [Serializable()]
    public class ImportedDoc
    {
        public string docNum = "";

        public string docTitle = "";

        public DateTime docDate = DateTime.MinValue;

        public int docTypeId = 0;

        public string masterDocNum = "";

        public DateTime masterDocDate = DateTime.MinValue;

        public List<Appendix> appendices = new List<Appendix>();

        public decimal docSum = 0m;

        public decimal docFinalSum = 0m;

        public ImportedDoc()
        {
        }

        public bool IsAct()
        {
            return docTypeId == 3;
        }
    }

    [Serializable()]
    public class ImportedAct
    {
        public string docNum = "";

        //public string docTitle = "";

        public DateTime docDate = DateTime.MinValue;

        public string masterDocNum = "";

        public DateTime masterDocDate = DateTime.MinValue;

        public List<ActObject> actObjects = new List<ActObject>();

        public decimal docSum = 0m;

        public decimal docFinalSum = 0m;

        public ImportedAct()
        {
        }
    }

    public class Document
    {
        public int documentId = -1;

        public int documentKind = -1;

        public string documentNumber = "";

        public string documentTitle = "";

        public DateTime documentDate = DateTime.MinValue;

        public int ownership_type_id = -1;
        public string modify_by = "";
        public DateTime modify_date = DateTime.Now;

        public int ownership_doc_type_id = -1;

        public List<int> dependentDocuments = null;

        public Document()
        {
        }

        public override string ToString()
        {
            return "N " + documentNumber + " від " + documentDate.ToShortDateString() + ": " + documentTitle;
        }

        
    }

    [Serializable()]
    public class ActObject
    {
        //public int objectId = -1;
        public int objectDocsPropertiesId = -1;

        public string addrStreet = "";
        public string addrNomer1 = "";
        public string addrNomer2 = "";
        public string addrNomer3 = "";
        public string addrMisc = "";
        public object districtId = null;

        public string objectName = "";
        public object objectSquare = null;
        public object objectLen = null;
        public object objectDiamTrub = null;
        public object objectBalansCost = null;
        public object objectFinalCost = null;

        public int objectFloorsInt = -1;
        public string objectFloorsStr = "";

        public int objectId = -1;

        public int purposeGroupId = -1;
        public int purposeId = -1;
        public int objectTypeId = -1;
        public int objectKindId = -1;

        public object characteristic = null;
        public object yearBuild = null;
        public object yearExpl = null;
        public object techStateId = null;

        public bool includedInAct = false;
        public bool makeChangesIn1NF = false;

        public List<BalansTransfer> balansTransfers = new List<BalansTransfer>();
        public ActObject()
        {
        }

        //public Object1NF object1NF
        //{
        //    get
        //    {
        //        Object1NF obj = null;

        //        if (DB.objects1NF.TryGetValue(objectId, out obj))
        //        {
        //            return obj;
        //        }

        //        return null;
        //    }

        //    set
        //    {
        //        if (value != null)
        //        {
        //            objectId = value.objectId;
        //        }
        //        else
        //        {
        //            objectId = -1;
        //        }
        //    }
        //}

        public override string ToString()
        {
            string address = addrNomer1.Trim();

            if (addrNomer2.Length > 0)
            {
                if (address.Length > 0)
                    address += " ";

                address += addrNomer2.Trim();
            }

            if (addrNomer3.Length > 0)
            {
                if (address.Length > 0)
                    address += " ";

                address += addrNomer3.Trim();
            }

            if (addrMisc.Length > 0)
            {
                if (address.Length > 0)
                    address += " ";

                address += addrMisc.Trim();
            }

            if (addrStreet.Length > 0 && address.Length > 0)
            {
                return addrStreet + ", " + address;
            }

            return address.Length > 0 ? address : addrStreet;
        }

        //public bool AllTransfersAreValid()
        //{
        //    foreach (BalansTransfer bt in balansTransfers)
        //    {
        //        if (!bt.IsFullyDefined())
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //public void DeduceObjectTypeFor1NF()
        //{
        //    makeChangesIn1NF =
        //        objectKindId == 1 ||
        //        objectKindId == 2 ||
        //        objectKindId == 3 ||
        //        objectKindId == 18;
        //}

        //public void CopyTo(ActObject other)
        //{
        //    other.objectId_NJF = objectId_NJF;
        //    other.objectDocsPropertiesId_NJF = objectDocsPropertiesId_NJF;
        //    other.addrStreet_NJF = addrStreet_NJF;
        //    other.addrNomer1_NJF = addrNomer1_NJF;
        //    other.addrNomer2_NJF = addrNomer2_NJF;
        //    other.addrNomer3_NJF = addrNomer3_NJF;
        //    other.addrMisc_NJF = addrMisc_NJF;
        //    other.districtId_NJF = districtId_NJF;
        //    other.objectName_NJF = objectName_NJF;
        //    other.objectSquare_NJF = objectSquare_NJF;
        //    other.objectId_1NF = objectId_1NF;

        //    other.purposeGroupIdNJF = purposeGroupIdNJF;
        //    other.purposeIdNJF = purposeIdNJF;
        //    other.objectTypeIdNJF = objectTypeIdNJF;
        //    other.objectKindIdNJF = objectKindIdNJF;

        //    other.objectBalansCost_NJF = objectBalansCost_NJF;
        //    other.objectFinalCost_NJF = objectFinalCost_NJF;
        //    other.objectFloorsInt_NJF = objectFloorsInt_NJF;
        //    other.objectFloorsStr_NJF = objectFloorsStr_NJF;

        //    other.characteristicNJF = characteristicNJF;
        //    other.yearBuildNJF = yearBuildNJF;
        //    other.yearExplNJF = yearExplNJF;
        //    other.techStateIdNJF = techStateIdNJF;

        //    other.includedInAct = includedInAct;
        //    other.makeChangesIn1NF = makeChangesIn1NF;

        //    other.balansTransfers.Clear();

        //    foreach (BalansTransfer bt in balansTransfers)
        //    {
        //        other.balansTransfers.Add(bt.MakeCopy());
        //    }
        //}

        //public void PerformMatchingTo1NF(ObjectFinder objectFinder1NF, OrganizationFinder orgFinder1NF)
        //{
        //    DictionaryData dictDistricts = null;

        //    if (!DB.dictionaries.TryGetValue(DB.DICT_DISTRICTS, out dictDistricts))
        //    {
        //        dictDistricts = null;
        //    }

        //    // Map the district code from NJF to 1NF
        //    object district1NF = null;

        //    if (districtId_NJF is int && dictDistricts != null)
        //    {
        //        DictionaryValue districtNJF = null;

        //        if (dictDistricts.ValuesSql.TryGetValue((int)districtId_NJF, out districtNJF))
        //        {
        //            foreach (KeyValuePair<int, DictionaryValue> pair in dictDistricts.ValuesSql)
        //            {
        //                if (districtNJF.value == pair.Value.value)
        //                {
        //                    district1NF = pair.Key;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    bool addressIsSimple = false;
        //    bool similarAddressExists = false;

        //    objectId_1NF = objectFinder1NF.FindObject(addrStreet_NJF, "",
        //        addrNomer1_NJF, addrNomer2_NJF, addrNomer2_NJF, addrMisc_NJF,
        //        district1NF, null, out addressIsSimple, out similarAddressExists);

        //    DeduceObjectTypeFor1NF();

        //    // Perform matching of balans transfers
        //    foreach (BalansTransfer bal in balansTransfers)
        //    {
        //        bal.objectId_1NF = objectId_1NF;

        //        bool categorized = false;
        //        Organization1NF org1NF = null;

        //        // Match the From organization
        //        bal.organizationFromId_1NF = orgFinder1NF.FindOrganization(bal.orgFromZkpo_NJF, bal.orgFromFullName_NJF, bal.orgFromShortName_NJF, false, out categorized);

        //        if (bal.organizationFromId_1NF > 0)
        //        {
        //            if (DB.organizations1NF.TryGetValue(bal.organizationFromId_1NF, out org1NF))
        //            {
        //                bal.orgFrom1NF = org1NF;
        //            }
        //        }

        //        // Match the To organization
        //        bal.organizationToId_1NF = orgFinder1NF.FindOrganization(bal.orgToZkpo_NJF, bal.orgToFullName_NJF, bal.orgToShortName_NJF, false, out categorized);

        //        if (bal.organizationToId_1NF > 0)
        //        {
        //            if (DB.organizations1NF.TryGetValue(bal.organizationToId_1NF, out org1NF))
        //            {
        //                bal.orgTo1NF = org1NF;
        //            }
        //        }

        //        bal.FindBalansObjectIn1NF();
        //    }
        //}

        //public FbCommand PrepareInsertStatement(FbConnection connectionNJF, int newRelId, int documentId)
        //{
        //    string fields = "";
        //    string values = "";
        //    Dictionary<string, object> parameters = new Dictionary<string, object>();

        //    AddInsertQueryParameter(ref fields, ref values, "ID", newRelId, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "ISP", DB.UserName, parameters, 18);
        //    AddInsertQueryParameter(ref fields, ref values, "DT", DateTime.Now, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "DOK_ID", documentId, parameters, -1);

        //    AddInsertQueryParameter(ref fields, ref values, "OBJECT_KOD", objectId, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "OBJECT_KODSTAN", 1, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "NAME", objectName, parameters, 255);
        //    AddInsertQueryParameter(ref fields, ref values, "CHARACTERISTIC", characteristicNJF, parameters, 100);
        //    AddInsertQueryParameter(ref fields, ref values, "SUMMA_BALANS", objectBalansCost, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "SUMMA_BALANS_0", objectBalansCost, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "SUMMA_ZAL", objectFinalCost, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "SUMMA_ZAL_0", objectFinalCost, parameters, -1);

        //    AddInsertQueryParameter(ref fields, ref values, "SQUARE", objectSquare, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "GRPURP", purposeGroupIdNJF, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "PURPOSE", purposeIdNJF, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "TEXSTAN", techStateIdNJF, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "OBJKIND", objectKindIdNJF, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "OBJTYPE", objectTypeIdNJF, parameters, -1);

        //    AddInsertQueryParameter(ref fields, ref values, "YEAR_BUILD", yearBuildNJF, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "YEAR_EXPL", yearExplNJF, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "LEN", objectLen, parameters, -1);
        //    AddInsertQueryParameter(ref fields, ref values, "DIAM_TRUB", objectDiamTrub, parameters, 20);
        //    AddInsertQueryParameter(ref fields, ref values, "FLOORS", objectFloorsInt, parameters, -1);

        //    FbCommand cmd = new FbCommand("INSERT INTO OBJECT_DOKS_PROPERTIES (" + fields.TrimStart(' ', ',') + ") VALUES (" + values.TrimStart(' ', ',') + ")", connectionNJF);

        //    foreach (KeyValuePair<string, object> param in parameters)
        //    {
        //        cmd.Parameters.Add(new FbParameter(param.Key, param.Value));
        //    }

        //    return cmd;
        //}

        private void AddInsertQueryParameter(ref string fields, ref string values, string fieldName, object value, Dictionary<string, object> parameters, int maxStringLen)
        {
            if (value is int && (int)value < 0)
            {
                value = null;
            }

            if (value is string && ((string)value).Length == 0)
            {
                value = null;
            }

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

        //public FbCommand PrepareUpdateStatement(FbConnection connectionNJF, int existingRelId, int documentId)
        //{
        //    string fields = "";
        //    Dictionary<string, object> parameters = new Dictionary<string, object>();

        //    AddUpdateQueryParameter(ref fields, "ISP", DB.UserName, parameters, 18);
        //    AddUpdateQueryParameter(ref fields, "DT", DateTime.Now, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "DOK_ID", documentId, parameters, -1);

        //    AddUpdateQueryParameter(ref fields, "OBJECT_KOD", objectId, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "NAME", objectName, parameters, 255);
        //    AddUpdateQueryParameter(ref fields, "CHARACTERISTIC", characteristicNJF, parameters, 100);
        //    AddUpdateQueryParameter(ref fields, "SUMMA_BALANS", objectBalansCost, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "SUMMA_BALANS_0", objectBalansCost, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "SUMMA_ZAL", objectFinalCost, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "SUMMA_ZAL_0", objectFinalCost, parameters, -1);

        //    AddUpdateQueryParameter(ref fields, "SQUARE", objectSquare, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "GRPURP", purposeGroupIdNJF, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "PURPOSE", purposeIdNJF, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "TEXSTAN", techStateIdNJF, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "OBJKIND", objectKindIdNJF, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "OBJTYPE", objectTypeIdNJF, parameters, -1);

        //    AddUpdateQueryParameter(ref fields, "YEAR_BUILD", yearBuildNJF, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "YEAR_EXPL", yearExplNJF, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "LEN", objectLen, parameters, -1);
        //    AddUpdateQueryParameter(ref fields, "DIAM_TRUB", objectDiamTrub, parameters, 20);
        //    AddUpdateQueryParameter(ref fields, "FLOORS", objectFloorsInt, parameters, -1);

        //    FbCommand cmd = new FbCommand("UPDATE OBJECT_DOKS_PROPERTIES SET " + fields.TrimStart(' ', ',') + " WHERE ID = @relid", connectionNJF);

        //    cmd.Parameters.Add(new FbParameter("relid", existingRelId));

        //    foreach (KeyValuePair<string, object> param in parameters)
        //    {
        //        cmd.Parameters.Add(new FbParameter(param.Key, param.Value));
        //    }

        //    return cmd;
        //}

        private void AddUpdateQueryParameter(ref string fields, string fieldName, object value, Dictionary<string, object> parameters, int maxStringLen)
        {
            if (value is int && (int)value < 0)
            {
                value = null;
            }

            if (value is string && ((string)value).Length == 0)
            {
                value = null;
            }

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

    }

    public enum ObjectTransferType
    {
        Transfer,
        Create,
        Destroy
    }

    [Serializable()]
    public class Appendix
    {
        public string appendixNum = "";

        public List<AppendixObject> objects = new List<AppendixObject>();

        public Appendix()
        {
        }

        public override string ToString()
        {
            return "Додаток " + appendixNum;
        }
    }

    [Serializable()]
    public class AppendixObject
    {
        public int objectId = -1;

        //public Dictionary<ColumnCategory, string> properties = new Dictionary<ColumnCategory, string>();

        public List<Transfer> transfers = new List<Transfer>();

        public AppendixObject()
        {
        }

        //public bool IsEmpty
        //{
        //    get
        //    {
        //        foreach (KeyValuePair<ColumnCategory, string> prop in properties)
        //        {
        //            if (prop.Value.Length > 0)
        //            {
        //                return false;
        //            }
        //        }

        //        if (transfers.Count > 0)
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //}

        //public Object1NF object1NF
        //{
        //    get
        //    {
        //        Object1NF obj = null;

        //        if (DB.objects1NF.TryGetValue(object1NFId, out obj))
        //        {
        //            return obj;
        //        }

        //        return null;
        //    }

        //    set
        //    {
        //        if (value != null)
        //        {
        //            object1NFId = value.objectId;
        //        }
        //        else
        //        {
        //            object1NFId = -1;
        //        }
        //    }
        //}

        //public AppendixObject MakeCopy()
        //{
        //    AppendixObject newObj = new AppendixObject();

        //    newObj.object1NF = object1NF;

        //    foreach (KeyValuePair<ColumnCategory, string> pair in properties)
        //    {
        //        newObj.properties.Add(pair.Key, pair.Value);
        //    }

        //    foreach (Transfer t in transfers)
        //    {
        //        newObj.transfers.Add(t.MakeCopy());
        //    }

        //    return newObj;
        //}

        public bool SimilarTransferExists(Transfer transfer)
        {
            foreach (Transfer t in transfers)
            {
                if (t.Equals(transfer))
                {
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable()]
    public class BalansTransfer
    {
        public ObjectTransferType transferType = ObjectTransferType.Transfer;

        public int objectId = -1;
        public int organizationFromId = -1;
        public int organizationToId = -1;

        public int balansId = -1;

        public string orgFromZkpo = "";
        public string orgFromFullName = "";
        public string orgFromShortName = "";

        public string orgToZkpo = "";
        public string orgToFullName = "";
        public string orgToShortName = "";
        
        //public int objectId_1NF = -1;
        //public int objectId_NJF = -1;

        //public int organizationFromId_1NF = -1;
        //public int organizationFromId_NJF = -1;

        //public int organizationToId_1NF = -1;
        //public int organizationToId_NJF = -1;

        //public int balansId_1NF = -1;

        public decimal sqr = 0m;

        //public string orgFromZkpo_NJF = "";
        //public string orgFromFullName_NJF = "";
        //public string orgFromShortName_NJF = "";

        //public string orgToZkpo_NJF = "";
        //public string orgToFullName_NJF = "";
        //public string orgToShortName_NJF = "";

        //public string orgFromZkpo_1NF = "";
        //public string orgFromFullName_1NF = "";
        //public string orgFromShortName_1NF = "";

        //public string orgToZkpo_1NF = "";
        //public string orgToFullName_1NF = "";
        //public string orgToShortName_1NF = "";

        public int form_ownership_id = -1;

        public BalansTransfer()
        {
            
        }

        //public Object1NF object1NF
        //{
        //    get
        //    {
        //        Object1NF obj = null;

        //        if (DB.objects1NF.TryGetValue(objectId_1NF, out obj))
        //        {
        //            return obj;
        //        }

        //        return null;
        //    }

        //    set
        //    {
        //        if (value != null)
        //        {
        //            objectId_1NF = value.objectId;
        //        }
        //        else
        //        {
        //            objectId_1NF = -1;
        //        }
        //    }
        //}

        //public Organization1NF orgFrom1NF
        //{
        //    get
        //    {
        //        Organization1NF org = null;

        //        if (DB.organizations1NF.TryGetValue(organizationFromId_1NF, out org))
        //        {
        //            return org;
        //        }

        //        return null;
        //    }

        //    set
        //    {
        //        if (value != null)
        //        {
        //            organizationFromId_1NF = value.organizationId;
        //            orgFromZkpo_1NF = value.zkpo;
        //            orgFromFullName_1NF = value.fullName;
        //            orgFromShortName_1NF = value.shortName;
        //        }
        //        else
        //        {
        //            organizationFromId_1NF = -1;
        //            orgFromZkpo_1NF = "";
        //            orgFromFullName_1NF = "";
        //            orgFromShortName_1NF = "";
        //        }
        //    }
        //}

        //public Organization1NF orgTo1NF
        //{
        //    get
        //    {
        //        Organization1NF org = null;

        //        if (DB.organizations1NF.TryGetValue(organizationToId_1NF, out org))
        //        {
        //            return org;
        //        }

        //        return null;
        //    }

        //    set
        //    {
        //        if (value != null)
        //        {
        //            organizationToId_1NF = value.organizationId;
        //            orgToZkpo_1NF = value.zkpo;
        //            orgToFullName_1NF = value.fullName;
        //            orgToShortName_1NF = value.shortName;
        //        }
        //        else
        //        {
        //            organizationToId_1NF = -1;
        //            orgToZkpo_1NF = "";
        //            orgToFullName_1NF = "";
        //            orgToShortName_1NF = "";
        //        }
        //    }
        //}

        //public BalansObject1NF balansObject1NF
        //{
        //    get
        //    {
        //        BalansObject1NF obj = null;

        //        if (DB.balans1NFByID.TryGetValue(balansId_1NF, out obj))
        //        {
        //            return obj;
        //        }

        //        return null;
        //    }

        //    set
        //    {
        //        if (value != null)
        //        {
        //            balansId_1NF = value.balansId;

        //            // Assign the transferred square as well
        //            if (value.sqr > 0m && sqr == 0m)
        //            {
        //                sqr = value.sqr;
        //            }

        //            // Assign the owner organization as well
        //            if (value.organizationId > 0)
        //            {
        //                Organization1NF org = null;

        //                if (DB.organizations1NF.TryGetValue(value.organizationId, out org))
        //                {
        //                    this.orgFrom1NF = org;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            balansId_1NF = -1;
        //        }
        //    }
        //}

        //public void FindBalansObjectIn1NF()
        //{
        //    balansId_1NF = -1;

        //    if (objectId_1NF > 0 && organizationFromId_1NF > 0 && sqr > 0m)
        //    {
        //        Dictionary<int, BalansObject1NF> storage = null;

        //        if (DB.balans1NFByAddress.TryGetValue(objectId_1NF, out storage))
        //        {
        //            // Select all balans objects that belong to the 'FROM' organization
        //            List<BalansObject1NF> objectsEqualSquare = new List<BalansObject1NF>();
        //            List<BalansObject1NF> objectsGreaterSquare = new List<BalansObject1NF>();

        //            foreach (KeyValuePair<int, BalansObject1NF> pair in storage)
        //            {
        //                if (pair.Value.organizationId == organizationFromId_1NF && pair.Value.sqr > 0m)
        //                {
        //                    // Check if square of both objects can be considered identical
        //                    decimal diff = Math.Abs(pair.Value.sqr - sqr);

        //                    if ((diff / sqr < 0.03m) && (diff / pair.Value.sqr < 0.03m))
        //                    {
        //                        objectsEqualSquare.Add(pair.Value);
        //                    }
        //                    else if (pair.Value.sqr > sqr)
        //                    {
        //                        objectsGreaterSquare.Add(pair.Value);
        //                    }
        //                }
        //            }

        //            // If there is one object with the same square - great
        //            if (objectsEqualSquare.Count == 1)
        //            {
        //                balansId_1NF = objectsEqualSquare[0].balansId;
        //            }
        //            else if (objectsEqualSquare.Count == 0 && objectsGreaterSquare.Count == 1)
        //            {
        //                balansId_1NF = objectsGreaterSquare[0].balansId;
        //            }
        //        }
        //    }
        //}

        //public BalansTransfer MakeCopy()
        //{
        //    BalansTransfer other = new BalansTransfer();

        //    other.objectId_1NF = objectId_1NF;
        //    other.objectId_NJF = objectId_NJF;
        //    other.organizationFromId_1NF = organizationFromId_1NF;
        //    other.organizationFromId_NJF = organizationFromId_NJF;
        //    other.organizationToId_1NF = organizationToId_1NF;
        //    other.organizationToId_NJF = organizationToId_NJF;
        //    other.balansId_1NF = balansId_1NF;

        //    other.sqr = sqr;

        //    other.orgFromZkpo_NJF = orgFromZkpo_NJF;
        //    other.orgFromFullName_NJF = orgFromFullName_NJF;
        //    other.orgFromShortName_NJF = orgFromShortName_NJF;

        //    other.orgToZkpo_NJF = orgToZkpo_NJF;
        //    other.orgToFullName_NJF = orgToFullName_NJF;
        //    other.orgToShortName_NJF = orgToShortName_NJF;

        //    other.orgFromZkpo_1NF = orgFromZkpo_1NF;
        //    other.orgFromFullName_1NF = orgFromFullName_1NF;
        //    other.orgFromShortName_1NF = orgFromShortName_1NF;

        //    other.orgToZkpo_1NF = orgToZkpo_1NF;
        //    other.orgToFullName_1NF = orgToFullName_1NF;
        //    other.orgToShortName_1NF = orgToShortName_1NF;

        //    other.transferType = transferType;

        //    return other;
        //}

        public bool IsFullyDefined()
        {
            switch (transferType)
            {
                case ObjectTransferType.Transfer:
                    return (objectId > 0) && (sqr > 0m) && (organizationFromId > 0) && (organizationToId > 0) && (balansId > 0);

                case ObjectTransferType.Create:
                    return (objectId > 0) && (sqr > 0m) && (organizationToId > 0);

                case ObjectTransferType.Destroy:
                    return (objectId > 0) && (organizationFromId > 0) && (balansId > 0);
            }

            return false;
        }
    }

    [Serializable()]
    public class Transfer
    {
        public string orgNameFrom = "";

        public string orgNameTo = "";

        public int organizationIdFrom = -1;

        public int organizationIdTo = -1;

        public int rightId = -1;

        public Transfer()
        {
        }

        public Transfer MakeCopy()
        {
            Transfer t = new Transfer();

            t.orgNameFrom = orgNameFrom;
            t.orgNameTo = orgNameTo;
            t.rightId = rightId;
            t.orgFrom = orgFrom;
            t.orgTo = orgTo;

            return t;
        }

        public Organization1NF orgFrom
        {
            get
            {
                Organization1NF org = null;

                if (DB.organizations1NF.TryGetValue(organizationIdFrom, out org))
                {
                    return org;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    organizationIdFrom = value.organizationId;
                    orgNameFrom = value.fullName;
                }
                else
                {
                    organizationIdFrom = -1;
                    orgNameFrom = "";
                }
            }
        }

        public Organization1NF orgTo
        {
            get
            {
                Organization1NF org = null;

                if (DB.organizations1NF.TryGetValue(organizationIdTo, out org))
                {
                    return org;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    organizationIdTo = value.organizationId;
                    orgNameTo = value.fullName;
                }
                else
                {
                    organizationIdTo = -1;
                    orgNameTo = "";
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Transfer)
            {
                Transfer t = obj as Transfer;

                if (t.orgFrom != orgFrom)
                {
                    return false;
                }

                if (t.orgTo != orgTo)
                {
                    return false;
                }

                if (t.organizationIdFrom != organizationIdFrom)
                {
                    return false;
                }

                if (t.organizationIdTo != organizationIdTo)
                {
                    return false;
                }

                return t.rightId == rightId;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


}
