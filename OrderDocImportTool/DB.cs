using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;
using GUKV.Common;
using GUKV.ImportToolUtils;

namespace GUKV.DataMigration
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

        public Dictionary<int, DictionaryValue> Values1NF = new Dictionary<int, DictionaryValue>();
        public Dictionary<int, DictionaryValue> ValuesNJF = new Dictionary<int, DictionaryValue>();

        public Dictionary<string, int> Keys1NF = new Dictionary<string, int>();
        public Dictionary<string, int> KeysNJF = new Dictionary<string, int>();

        public int MaxKey1NF = -1;
        public int MaxKeyNJF = -1;

        public DictionaryData()
        {
        }

        public DictionaryData(string name,
            FbConnection connection1NF, string tableName1NF, string keyName1NF, string valueName1NF, string parentKeyName1NF, string sortFields1NF,
            FbConnection connectionNJF, string tableNameNJF, string keyNameNJF, string valueNameNJF, string parentKeyNameNJF, string sortFieldsNJF)
        {
            Name = name;

            if (connection1NF != null)
            {
                LoadValues(connection1NF, Values1NF, Keys1NF, tableName1NF, keyName1NF, valueName1NF, parentKeyName1NF, sortFields1NF);
            }

            if (connectionNJF != null)
            {
                LoadValues(connectionNJF, ValuesNJF, KeysNJF, tableNameNJF, keyNameNJF, valueNameNJF, parentKeyNameNJF, sortFieldsNJF);
            }

            // Calculate the max. keys
            foreach (KeyValuePair<int, DictionaryValue> pair in Values1NF)
            {
                if (MaxKey1NF < pair.Key)
                {
                    MaxKey1NF = pair.Key;
                }
            }

            foreach (KeyValuePair<int, DictionaryValue> pair in ValuesNJF)
            {
                if (MaxKeyNJF < pair.Key)
                {
                    MaxKeyNJF = pair.Key;
                }
            }
        }

        private void LoadValues(FbConnection connection, Dictionary<int, DictionaryValue> values, Dictionary<string, int> keys,
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
                using (FbCommand cmd = new FbCommand("SELECT " + keyName + ", " + valueName + ", " + parentKeyName + " FROM " + tableName +
                    " WHERE (NOT " + valueName + " IS NULL) AND (" + valueName + " <> '') ORDER BY " + orderBy, connection))
                {
                    using (FbDataReader reader = cmd.ExecuteReader())
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
                using (FbCommand cmd = new FbCommand("SELECT " + keyName + ", " + valueName + " FROM " + tableName +
                    " WHERE (NOT " + valueName + " IS NULL) AND (" + valueName + " <> '') ORDER BY " + orderBy, connection))
                {
                    using (FbDataReader reader = cmd.ExecuteReader())
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
    }

    public static class DB
    {
        public static string UserName = "";

        public static string UserPassword = "";

        public static Dictionary<string, string> users = new Dictionary<string, string>();

        public static Dictionary<int, Organization1NF> organizations1NF = new Dictionary<int, Organization1NF>();

        public static Dictionary<int, Object1NF> objects1NF = new Dictionary<int, Object1NF>();

        public static Dictionary<int, Dictionary<int, BalansObject1NF>> balans1NFByAddress = new Dictionary<int, Dictionary<int, BalansObject1NF>>();

        public static Dictionary<int, BalansObject1NF> balans1NFByID = new Dictionary<int, BalansObject1NF>();

        public static Dictionary<int, DocumentNJF> documentsNJF = new Dictionary<int, DocumentNJF>();

        public static Dictionary<int, DocumentNJF> actsNJF = new Dictionary<int, DocumentNJF>();

        public static Dictionary<int, Organization1NF> organizationsNJF = new Dictionary<int,Organization1NF>();

        public static Dictionary<string, DictionaryData> dictionaries = new Dictionary<string, DictionaryData>();

        private static Dictionary<int, int> manualStreetCodeMappingNJFto1NF = null;

        private static Dictionary<int, int> objectTypeMappingNJFto1NF = GUKV.ImportToolUtils.FieldMappings.CreateObjectTypeMappingNJFto1NF(-1, -1, -1);

        public const int MAX_DOC_NUMBER_LEN = 20;
        public const int MAX_DOC_TITLE_LEN = 255;
        public const int MAX_OBJECT_NAME_LEN = 255;
        public const int MAX_DIAMETER_LEN = 20;
        public const int MAX_CHARACTERISTICS_LEN = 100;

        public const string DICT_PURPOSE = "PURPOSE";
        public const string DICT_PURPOSE_GROUP = "PURPOSE_GROUP";
        public const string DICT_OBJ_KIND = "OBJ_KIND";
        public const string DICT_OBJ_TYPE = "OBJ_TYPE";
        public const string DICT_STREETS = "STREETS";
        public const string DICT_DISTRICTS = "DISTRICTS";
        public const string DICT_ORG_INDUSTRY = "INDUSTRY";
        public const string DICT_ORG_OCCUPATION = "OCCUPATION";
        public const string DICT_ORG_TYPE = "ORG_TYPE";
        public const string DICT_ORG_FORM_GOSP = "FIN_FORM";
        public const string DICT_ORG_OWNERSHIP = "OWNERSHIP";
        public const string DICT_RIGHTS = "RIGHTS";
        public const string DICT_DOC_TYPES = "DOC_TYPES";
        public const string DICT_TECH_STATE = "OBJ_TECH_STATE";

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

        public static void LoadUsers(Preferences preferences)
        {
            FbConnection connection = new FbConnection(preferences.GetUsersConnectionString());

            try
            {
                connection.Open();

#if DEBUG
                users.Add("SYSDBA", "<Administrator>");
#endif

                using (FbCommand cmd = new FbCommand("SELECT SYS_USER, REAL_USER FROM SYSTEM_USER", connection))
                {
                    using (FbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(reader.GetString(0), reader.GetString(1));
                        }

                        reader.Close();
                    }
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних користувачів",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static void LoadData(Preferences preferences)
        {
            // Test connections
            FbConnection connectionNJF = new FbConnection(preferences.GetNJFConnectionString(UserName, UserPassword));

            try
            {
                connectionNJF.Open();
                connectionNJF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних 'Розпорядження': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString());

            try
            {
                connection1NF.Open();
                connection1NF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return;
            }

            // Load data from both databases
            try
            {
                connection1NF.Open();
                connectionNJF.Open();

                ///////////////////////////////////////////////////////////////////////////////////
                // Load dictionaries

                // Purpose
                dictionaries.Add(DICT_PURPOSE, new DictionaryData(DICT_PURPOSE,
                    connection1NF, "SPURPOSE", "KOD", "NAME", "GR", "",
                    connectionNJF, "SPURPOSE", "KOD", "NAME", "GR", ""));

                // Purpose Group
                dictionaries.Add(DICT_PURPOSE_GROUP, new DictionaryData(DICT_PURPOSE_GROUP,
                    connection1NF, "SGRPURPOSE", "KOD", "NAME", "", "",
                    connectionNJF, "SGRPURPOSE", "KOD", "NAME", "", ""));

                // Object kind
                dictionaries.Add(DICT_OBJ_KIND, new DictionaryData(DICT_OBJ_KIND,
                    connection1NF, "SKINDOBJ", "KOD", "NAME", "", "",
                    connectionNJF, "SKINDOBJ", "KOD", "NAME", "", ""));

                // Object type
                dictionaries.Add(DICT_OBJ_TYPE, new DictionaryData(DICT_OBJ_TYPE,
                    connection1NF, "STYPEOBJ", "KOD", "NAME", "", "",
                    connectionNJF, "STYPEOBJ", "KOD", "NAME", "", ""));

                // Transfer rights (exists in NJF only)
                dictionaries.Add(DICT_RIGHTS, new DictionaryData(DICT_RIGHTS,
                    null, "", "", "", "", "",
                    connectionNJF, "SPRAVO", "KOD", "NAME", "", ""));

                // Document types (exists in NJF only)
                dictionaries.Add(DICT_DOC_TYPES, new DictionaryData(DICT_DOC_TYPES,
                    null, "", "", "", "", "",
                    connectionNJF, "SKINDDOK", "KOD", "NAME", "", ""));

                // Street names
                dictionaries.Add(DICT_STREETS, new DictionaryData(DICT_STREETS,
                    connection1NF, "SUL", "KOD", "NAME", "", "KOD, STAN",
                    connectionNJF, "SUL", "KOD", "NAME", "", ""));

                // Districts
                dictionaries.Add(DICT_DISTRICTS, new DictionaryData(DICT_DISTRICTS,
                    connection1NF, "S_RAYON2", "KOD_RAYON2", "NAME_RAYON2", "", "",
                    connectionNJF, "SRA", "KOD", "NAME", "", ""));

                // Organization types
                dictionaries.Add(DICT_ORG_TYPE, new DictionaryData(DICT_ORG_TYPE,
                    connection1NF, "S_ORG_FORM", "KOD_ORG_FORM", "NAME_ORG_FORM", "", "",
                    connectionNJF, "SPV", "KOD", "NAME", "", ""));

                // Industry
                dictionaries.Add(DICT_ORG_INDUSTRY, new DictionaryData(DICT_ORG_INDUSTRY,
                    connection1NF, "S_GALUZ", "KOD_GALUZ", "NAME_GALUZ", "", "",
                    connectionNJF, "SOT", "KOD", "NAME", "", ""));

                // Occupation (exists only in 1NF)
                dictionaries.Add(DICT_ORG_OCCUPATION, new DictionaryData(DICT_ORG_OCCUPATION,
                    connection1NF, "S_VID_DIAL", "KOD_VID_DIAL", "NAME_VID_DIAL", "KOD_GALUZ", "",
                    null, "", "", "", "", ""));

                // Organization finance types
                dictionaries.Add(DICT_ORG_FORM_GOSP, new DictionaryData(DICT_ORG_FORM_GOSP,
                    connection1NF, "S_FORM_GOSP", "KOD_FORM_GOSP", "NAME_FORM_GOSP", "", "",
                    connectionNJF, "SFG", "KOD", "NAME", "", ""));

                // Organization form of ownership
                dictionaries.Add(DICT_ORG_OWNERSHIP, new DictionaryData(DICT_ORG_OWNERSHIP,
                    connection1NF, "S_FORM_VLASN", "KOD_FORM_VLASN", "NAME_FORM_VLASN", "", "",
                    connectionNJF, "SFORMVL", "KOD", "NAME", "", ""));

                // Technical state
                dictionaries.Add(DICT_TECH_STATE, new DictionaryData(DICT_TECH_STATE,
                    connection1NF, "STEXSTAN", "KOD", "NAME", "", "",
                    connectionNJF, "STEXSTAN", "KOD", "NAME", "", ""));

                RemoveOldDistricts();

                ///////////////////////////////////////////////////////////////////////////////////
                // Load database objects

                LoadOrganizationsFrom1NF(connection1NF);
                LoadObjectsFrom1NF(connection1NF);
                LoadBalansObjectsFrom1NF(connection1NF);
                LoadDocumentsFromNJF(connectionNJF);

                organizationsNJF = LoadOrganizationsFromNJF(connectionNJF);

                connection1NF.Close();
                connectionNJF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних: " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static void InitObjectFinderFromNJF(Preferences preferences, ObjectFinder objectFinder)
        {
            FbConnection connection = new FbConnection(preferences.GetNJFConnectionString(UserName, UserPassword));

            try
            {
                connection.Open();

                objectFinder.BuildObjectCacheFromNJF(connection);

                connection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних 'Розпорядження': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static void InitObjectFinderFrom1NF(Preferences preferences, ObjectFinder objectFinder)
        {
            FbConnection connection = new FbConnection(preferences.Get1NFConnectionString());

            try
            {
                connection.Open();

                objectFinder.BuildObjectCacheFrom1NF(connection);

                connection.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static int FindCodeInDictionary1NF(string dictionaryName, string name)
        {
            name = name.Trim().ToUpper();

            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
                {
                    if (pair.Value.value.Trim().ToUpper() == name)
                    {
                        return pair.Key;
                    }
                }
            }

            return -1;
        }

        public static int FindCodeInDictionary1NF_Hierarchical(string dictionaryName, string name, int parentKey)
        {
            name = name.Trim().ToUpper();

            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
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
                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
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

        public static int MatchDictionary1NFtoNJF(string dictionaryName, int key1NF)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                DictionaryValue value1NF = null;

                if (data.Values1NF.TryGetValue(key1NF, out value1NF))
                {
                    foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
                    {
                        if (pair.Value.value == value1NF.value)
                        {
                            return pair.Key;
                        }
                    }
                }
            }

            return -1;
        }

        public static int MatchDictionaryNJFto1NF(string dictionaryName, int key1NF)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(dictionaryName, out data))
            {
                DictionaryValue valueNJF = null;

                if (data.ValuesNJF.TryGetValue(key1NF, out valueNJF))
                {
                    foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
                    {
                        if (pair.Value.value == valueNJF.value)
                        {
                            return pair.Key;
                        }
                    }
                }
            }

            return -1;
        }

        public static int MatchOrgOwnership1NFtoNJF(int ownershipId1NF)
        {
            switch (ownershipId1NF)
            {
                case 10:
                case 20:
                case 31:
                case 33:
                case 34:
                case 35:
                case 40:
                case 50:
                    return ownershipId1NF;

                case 38:
                    return 34;

                case 99:
                    return -1;
            }

            return MatchDictionary1NFtoNJF(DICT_ORG_OWNERSHIP, ownershipId1NF);
        }

        public static int MatchOrgFormGosp1NFtoNJF(int formGospId1NF)
        {
            switch (formGospId1NF)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    return formGospId1NF;
            }

            return MatchDictionary1NFtoNJF(DICT_ORG_FORM_GOSP, formGospId1NF);
        }

        public static int MatchObjectTypeNJFto1NF(int objectTypeIdNJF)
        {
            int objectTyp11NF = -1;

            if (objectTypeMappingNJFto1NF.TryGetValue(objectTypeIdNJF, out objectTyp11NF))
            {
                return objectTyp11NF;
            }

            return -1;
        }

        public static int MatchObjectKindNJFto1NF(int objectTypeIdNJF)
        {
            if (objectTypeIdNJF >= 1 && objectTypeIdNJF <= 4)
            {
                return objectTypeIdNJF;
            }

            return -1;
        }

        public static int MatchDictionary1NFtoNJF_Hierarchical(string dictionaryName, int key1NF,
            string parentDictionaryName, int parentKey1NF)
        {
            int parentKeyNJF = MatchDictionary1NFtoNJF(parentDictionaryName, parentKey1NF);

            if (parentKeyNJF >= 0)
            {
                DictionaryData data = null;

                if (dictionaries.TryGetValue(dictionaryName, out data))
                {
                    DictionaryValue value1NF = new DictionaryValue();

                    if (data.Values1NF.TryGetValue(key1NF, out value1NF))
                    {
                        foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
                        {
                            if (pair.Value.parentKey == parentKeyNJF && pair.Value.value == value1NF.value)
                            {
                                return pair.Key;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        public static int MatchDictionaryNJFto1NF_Hierarchical(string dictionaryName, int keyNJF,
            string parentDictionaryName, int parentKeyNJF)
        {
            int parentKey1NF = MatchDictionaryNJFto1NF(parentDictionaryName, parentKeyNJF);

            if (parentKey1NF >= 0)
            {
                DictionaryData data = null;

                if (dictionaries.TryGetValue(dictionaryName, out data))
                {
                    DictionaryValue valueNJF = new DictionaryValue();

                    if (data.ValuesNJF.TryGetValue(keyNJF, out valueNJF))
                    {
                        foreach (KeyValuePair<int, DictionaryValue> pair in data.Values1NF)
                        {
                            if (pair.Value.parentKey == parentKey1NF && pair.Value.value == valueNJF.value)
                            {
                                return pair.Key;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        public static int FindDocument(string docNum, DateTime docDate, int docKind)
        {
            docNum = docNum.Trim().ToUpper();

            foreach (KeyValuePair<int, DocumentNJF> doc in documentsNJF)
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
                DocumentNJF parentDoc = null;

                if (documentsNJF.TryGetValue(parentId, out parentDoc))
                {
                    if (parentDoc.dependentDocuments != null)
                    {
                        foreach (int childId in parentDoc.dependentDocuments)
                        {
                            DocumentNJF childDoc = null;

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

            foreach (KeyValuePair<int, DocumentNJF> doc in actsNJF)
            {
                if (doc.Value.documentNumber == actNum && doc.Value.documentDate.Date == actDate.Date)
                {
                    return doc.Key;
                }
            }

            return -1;
        }

        public static int CreateNewNJFObject(FbConnection connectionNJF, FbTransaction transaction,
            string street, string district, string nomer1, string nomer2, string nomer3, string addrMisc,
            out int streetId, out int districtId)
        {
            int objectId = -1;

            street = street.Trim().ToUpper();
            district = district.Trim().ToUpper();
            nomer1 = nomer1.Trim().ToUpper();
            nomer2 = nomer2.Trim().ToUpper();
            nomer3 = nomer3.Trim().ToUpper();
            addrMisc = addrMisc.Trim().ToUpper();

            if (nomer3.StartsWith(Properties.AppResources.KorpusPrefix))
            {
                nomer3 = nomer3.Substring(2);
            }

            if (street.Length <= 0)
            {
                throw new ArgumentException("Для створення нового об'єкту необхідно вказати вулицю.");
            }

            if ((nomer1 + nomer2 + nomer3 + addrMisc).Length == 0)
            {
                throw new ArgumentException("Для створення нового об'єкту необхідно вказати номер будинку.");
            }

            streetId = FindStreetMatchInNJF(street, connectionNJF, transaction);

            if (streetId <= 0)
            {
                throw new ArgumentException("Вулицю " + street + " не знайдено в базі Розпорядження");
            }

            districtId = -1;

            if (district.Length > 0)
            {
                districtId = FindDistrictMatchInNJF(district);
            }

            // If address can not fit into narrow columns, try to fix it
            if (nomer3.Length > 3)
            {
                addrMisc = nomer3 + ((addrMisc.Length > 0) ? " " + addrMisc : "");
                nomer3 = "";
            }

            if (nomer2.Length > 4)
            {
                addrMisc = nomer2 + ((addrMisc.Length > 0) ? " " + addrMisc : "");
                nomer2 = "";
            }

            // Generate the new address code
            int addrId = GenerateNewFirebirdId(connectionNJF, "ADR_REES_GEN", transaction);

            if (addrId > 0)
            {
                // Insert the new record into ADR_REES table
                string fields = "ADR_REES_KOD, ADR_REES_KODSTAN, ULNAME, DT_BEG, ADRDOP, NOMER1, NOMER2, NOMER3, ULKOD, DT, ISP";
                string parameters = "@kod, 0, @strt, @edt, @misc, @nom1, @nom2, @nom3, @strtid, @edt, @usr";

                if (districtId > 0)
                {
                    fields += ", NEWDISTR";
                    parameters += ", @distrid";
                }

                string query = "INSERT INTO ADR_REES (" + fields + ") VALUES (" + parameters + ")";

                using (FbCommand commandInsert = new FbCommand(query, connectionNJF))
                {
                    try
                    {
                        commandInsert.Parameters.Add(new FbParameter("kod", addrId));
                        commandInsert.Parameters.Add(new FbParameter("strt", street.Length > 60 ? street.Substring(0, 60) : street));
                        commandInsert.Parameters.Add(new FbParameter("misc", addrMisc.Length > 60 ? addrMisc.Substring(0, 60) : addrMisc));
                        commandInsert.Parameters.Add(new FbParameter("nom1", nomer1.Length > 4 ? nomer1.Substring(0, 4) : nomer1));
                        commandInsert.Parameters.Add(new FbParameter("nom2", nomer2.Length > 4 ? nomer2.Substring(0, 4) : nomer2));
                        commandInsert.Parameters.Add(new FbParameter("nom3", nomer3.Length > 3 ? nomer3.Substring(0, 3) : nomer3));
                        commandInsert.Parameters.Add(new FbParameter("strtid", streetId));
                        commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
                        commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));

                        if (districtId > 0)
                        {
                            commandInsert.Parameters.Add(new FbParameter("distrid", districtId));
                        }

                        commandInsert.Transaction = transaction;
                        commandInsert.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                        throw;
                    }
                }

                // Get the ID of object (which is created automatically via Trigger)
                query = "SELECT MAX(OBJECT_KOD) FROM OBJECT";

                using (FbCommand cmd = new FbCommand(query, connectionNJF))
                {
                    try
                    {
                        cmd.Transaction = transaction;

                        using (FbDataReader r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                objectId = r.GetInt32(0);
                            }

                            r.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(query, cmd, ex.Message);
                        throw;
                    }
                }
            }

            return objectId;
        }

        public static int CreateNew1NFObject(Preferences preferences, int streetId, string streetName, int districtId,
            string number1, string number2, string number3, string addrMisc)
        {
            int newBuildingId = -1;

            number1 = number1.Trim().ToUpper();
            number2 = number2.Trim().ToUpper();
            number3 = number3.Trim().ToUpper();
            addrMisc = addrMisc.Trim().ToUpper();

            ObjectFinder.PreProcessBuildingNumbers(ref number1, ref number2);

            // Verify parameters
            if ((number1 + number2 + number3 + addrMisc).Length == 0)
            {
                throw new ArgumentException("Необхідно заповнити номер будинку.");
            }

            if (streetId < 0)
            {
                throw new ArgumentException("Необхідно вибрати вулицю.");
            }

            using (FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString()))
            {
                try
                {
                    connection1NF.Open();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return -1;
                }

                // Make all changes to the Firebird database in a transaction
                FbTransaction transaction = null;

                try
                {
                    transaction = connection1NF.BeginTransaction();

                    // Generate new building Id
                    newBuildingId = GenerateNewFirebirdId(connection1NF, "OBJECT_1NF_GEN", transaction);

                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    if (newBuildingId < 0)
                    {
                        throw new InvalidOperationException("Помилка при створенні ідентифікатора нового будинку в базі 1НФ.");
                    }

                    // Get the street state from 1NF
                    /*
                    int streetState = -1;

                    using (FbCommand cmdTest = new FbCommand("SELECT STAN, NAME FROM SUL WHERE KOD = @strid", connection1NF))
                    {
                        cmdTest.Transaction = transaction;
                        cmdTest.Parameters.Add(new FbParameter("strid", streetId));

                        using (FbDataReader r = cmdTest.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                int stan = r.IsDBNull(0) ? -1 : r.GetInt32(0);
                                string name = r.IsDBNull(1) ? "" : r.GetString(1).Trim().ToUpper();

                                if (streetName == name)
                                {
                                    streetState = stan;
                                    break;
                                }
                            }

                            r.Close();
                        }
                    }
                    */

                    streetName = streetName.Trim().ToUpper();

                    if (streetName.Length > 100)
                        streetName = streetName.Substring(0, 100);

                    // Prepare the INSERT statement
                    string fieldList = "OBJECT_KOD, OBJECT_KODSTAN, ISP, EIS_MODIFIED_BY, DT, STAN_YEAR, REALSTAN, ULNAME, FULL_ULNAME, DELETED, KORPUS";
                    string paramList = "@oid, 1, @isp, @isp, @dt, @syear, 1, @sname, @sname, 0, 0";

                    AddQueryParam("scod", "ULKOD", streetId, ref fieldList, ref paramList, parameters, -1);
                    AddQueryParam("num1", "NOMER1", number1, ref fieldList, ref paramList, parameters, 9);
                    AddQueryParam("num2", "NOMER2", number2, ref fieldList, ref paramList, parameters, 18);
                    AddQueryParam("num3", "NOMER3", number3, ref fieldList, ref paramList, parameters, 10);
                    AddQueryParam("amsc", "ADRDOP", addrMisc, ref fieldList, ref paramList, parameters, 100);
                    AddQueryParam("distr", "NEWDISTR", districtId, ref fieldList, ref paramList, parameters, -1);

                    string query = "INSERT INTO OBJECT_1NF (" + fieldList + ") VALUES (" + paramList + ")";

                    using (FbCommand commandInsert = new FbCommand(query, connection1NF))
                    {
                        try
                        {
                            commandInsert.Parameters.Add(new FbParameter("oid", newBuildingId));
                            commandInsert.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
                            commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                            commandInsert.Parameters.Add(new FbParameter("syear", DateTime.Now.Year));
                            commandInsert.Parameters.Add(new FbParameter("sname", streetName));

                            foreach (KeyValuePair<string, object> pair in parameters)
                            {
                                commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                            }

                            commandInsert.Transaction = transaction;
                            commandInsert.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                            throw;
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();
                    transaction = null; // This will prevent an undesired Rollback() in the catch{} section

                    // Save the created building to our local cache
                    Object1NF obj = new Object1NF();

                    obj.objectId = newBuildingId;
                    obj.streetName = FindNameInDictionary1NF(DICT_STREETS, streetId);
                    obj.addrNomer1 = number1;
                    obj.addrNomer2 = number2;
                    obj.addrNomer3 = number3;
                    obj.addrMisc = addrMisc;
                    obj.techStateId = null;
                    obj.buildYear = null;
                    obj.districtId = districtId > 0 ? (object)districtId : null;
                    obj.objTypeId = null;
                    obj.objKindId = null;
                    obj.streetId = streetId;
                    obj.totalSqr = null;

                    objects1NF.Add(newBuildingId, obj);
                }
                catch (Exception ex)
                {
                    newBuildingId = -1;

                    // Roll back the transaction
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }

                connection1NF.Close();
            }

            return newBuildingId;
        }

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

        public static int CreateNewNJFOrganization(FbConnection connection, FbTransaction transaction,
            string zkpo, string fullName, string shortName,
            int? districtId, int streetId, string nomer1, string nomer2, string addrMisc, int? flat, string zipCode,
            int industryId, int ownershipId, int formGospId, int orgTypeId, int orgStatusId,
            string directorFIO, string directorTel, string buhgalterFIO, string buhgalterTel, string fax)
        {
            // Generate the new organization code
            int newNjfOrgId = GenerateNewFirebirdId(connection, "ZKPO_GEN", transaction);

            if (newNjfOrgId > 0)
            {
                string fieldList = "KOD, STAN, ZKPO, NAMEP, NAME, ISACTIVE, ISP, DTISP, DT_BEG, POSTIND, NOMER1, NOMER2, ADRDOP, FIO, TEL, FAX, FIOB, TELB";
                string paramList = "@kod, 0, @zkpo, @namep, @name, 1, @usr, @mdt, @mdt, @postal, @nom1, @nom2, @adrdop, @dirf, @dirt, @fax, @buhf, @buht";

                if (districtId.HasValue)
                {
                    fieldList += ", RA";
                    paramList += ", @distr";
                }

                if (flat.HasValue)
                {
                    fieldList += ", KVARTIRA";
                    paramList += ", @kva";
                }

                if (orgStatusId > 0)
                {
                    fieldList += ", TYPE_ORG";
                    paramList += ", @orgt";
                }

                if (industryId > 0)
                {
                    fieldList += ", OT";
                    paramList += ", @ind";
                }

                if (formGospId > 0)
                {
                    fieldList += ", FG";
                    paramList += ", @fgosp";
                }

                if (ownershipId > 0)
                {
                    fieldList += ", FORMVL";
                    paramList += ", @fvl";
                }

                if (orgTypeId > 0)
                {
                    fieldList += ", PV";
                    paramList += ", @orgform";
                }

                if (streetId > 0)
                {
                    fieldList += ", UL";
                    paramList += ", @strid";
                }

                string query = "INSERT INTO SZKPO (" + fieldList + ") VALUES (" + paramList + ")";

                using (FbCommand command = connection.CreateCommand())
                {
                    try
                    {
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = query;

                        command.Parameters.Add(new FbParameter("kod", newNjfOrgId));
                        command.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 10 ? zkpo.Substring(0, 10) : zkpo));
                        command.Parameters.Add(new FbParameter("namep", fullName.Length > 160 ? fullName.Substring(0, 160) : fullName));
                        command.Parameters.Add(new FbParameter("name", shortName.Length > 160 ? shortName.Substring(0, 160) : shortName));
                        command.Parameters.Add(new FbParameter("usr", UserName.Length > 20 ? UserName.Substring(0, 20) : UserName));
                        command.Parameters.Add(new FbParameter("mdt", DateTime.Now.Date));

                        command.Parameters.Add(new FbParameter("postal", zipCode.Length > 15 ? zipCode.Substring(0, 15) : zipCode));
                        command.Parameters.Add(new FbParameter("nom1", nomer1.Length > 4 ? nomer1.Substring(0, 4) : nomer1));
                        command.Parameters.Add(new FbParameter("nom2", nomer2.Length > 4 ? nomer2.Substring(0, 4) : nomer2));
                        command.Parameters.Add(new FbParameter("adrdop", addrMisc.Length > 160 ? addrMisc.Substring(0, 160) : addrMisc));

                        command.Parameters.Add(new FbParameter("dirf", directorFIO.Length > 160 ? directorFIO.Substring(0, 160) : directorFIO));
                        command.Parameters.Add(new FbParameter("dirt", directorTel.Length > 15 ? directorTel.Substring(0, 15) : directorTel));
                        command.Parameters.Add(new FbParameter("fax", fax.Length > 15 ? fax.Substring(0, 15) : fax));
                        command.Parameters.Add(new FbParameter("buhf", buhgalterFIO.Length > 160 ? buhgalterFIO.Substring(0, 160) : buhgalterFIO));
                        command.Parameters.Add(new FbParameter("buht", buhgalterTel.Length > 18 ? buhgalterTel.Substring(0, 18) : buhgalterTel));

                        if (orgStatusId > 0)
                        {
                            command.Parameters.Add(new FbParameter("orgt", orgStatusId));
                        }

                        if (industryId > 0)
                        {
                            command.Parameters.Add(new FbParameter("ind", industryId));
                        }

                        if (formGospId > 0)
                        {
                            command.Parameters.Add(new FbParameter("fgosp", formGospId));
                        }

                        if (ownershipId > 0)
                        {
                            command.Parameters.Add(new FbParameter("fvl", ownershipId));
                        }

                        if (orgTypeId > 0)
                        {
                            command.Parameters.Add(new FbParameter("orgform", orgTypeId));
                        }

                        if (districtId.HasValue)
                        {
                            command.Parameters.Add(new FbParameter("distr", districtId.Value));
                        }

                        if (flat.HasValue)
                        {
                            command.Parameters.Add(new FbParameter("kva", flat.Value));
                        }

                        if (streetId > 0)
                        {
                            command.Parameters.Add(new FbParameter("strid", streetId));
                        }

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(query, command, ex.Message);
                        throw;
                    }
                }
            }

            return newNjfOrgId;
        }

        public static int CreateNew1NFOrganization(Preferences preferences,
            string zkpo, string fullName, string shortName,
            int? districtId, string street, string nomer, string korpus, string addrMisc, string zipCode,
            int industryId, int occupationId, int ownershipId, int formGospId, int orgTypeId, int orgStatusId,
            string directorFIO, string directorTel, string buhgalterFIO, string buhgalterTel, string fax)
        {
            int newOrgId = -1;

            fullName = fullName.Trim().ToUpper();
            shortName = shortName.Trim().ToUpper();
            zkpo = zkpo.Trim();

            nomer = nomer.Trim().ToUpper();
            korpus = korpus.Trim().ToUpper();
            addrMisc = addrMisc.Trim().ToUpper();
            zipCode = zipCode.Trim().ToUpper();

            directorFIO = directorFIO.Trim().ToUpper();
            directorTel = directorTel.Trim().ToUpper();
            buhgalterFIO = buhgalterFIO.Trim().ToUpper();
            buhgalterTel = buhgalterTel.Trim().ToUpper();
            fax = fax.Trim().ToUpper();

            // Verify parameters
            if (fullName.Length == 0)
            {
                throw new ArgumentException("Необхідно заповнити повну назву організації.");
            }

            if (zkpo.Length == 0)
            {
                throw new ArgumentException("Необхідно заповнити Код ЄДРПОУ.");
            }

            using (FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString()))
            {
                try
                {
                    connection1NF.Open();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return -1;
                }

                // Make all changes to the Firebird database in a transaction
                FbTransaction transaction = null;

                try
                {
                    transaction = connection1NF.BeginTransaction();

                    // Check if ZKPO code already exists
                    using (FbCommand cmdTest = new FbCommand("SELECT FIRST 1 KOD_OBJ FROM SORG_1NF WHERE KOD_ZKPO = @zkpo", connection1NF))
                    {
                        cmdTest.Transaction = transaction;
                        cmdTest.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 14 ? zkpo.Substring(0, 14) : zkpo));

                        using (FbDataReader r = cmdTest.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                throw new InvalidOperationException("В базі 1НФ вже існує організація з таким кодом ЄДРПОУ.");
                            }

                            r.Close();
                        }
                    }

                    // Generate new organization Id
                    newOrgId = Get1NFNewOrganizationId(connection1NF, transaction);

                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    if (newOrgId < 0)
                    {
                        throw new InvalidOperationException("Помилка при створенні ідентифікатора нової організації в базі 1НФ.");
                    }

                    // Prepare the INSERT statement
                    string fieldList = "KOD_OBJ, KOD_STAN, LAST_SOST, ISP, EIS_MODIFIED_BY, DT, USER_KOREG, DATE_KOREG, DELETED, KOD_ZKPO, FULL_NAME_OBJ, SHORT_NAME_OBJ";
                    string paramList = "@orgd, 1, 1, @isp, @isp, @dt, @isp, @dt, 0, @zkpo, @fname, @sname";

                    AddQueryParam("gal", "KOD_GALUZ", industryId, ref fieldList, ref paramList, parameters, -1);
                    AddQueryParam("viddial", "KOD_VID_DIAL", occupationId, ref fieldList, ref paramList, parameters, -1);
                    AddQueryParam("fvl", "KOD_FORM_VLASN", ownershipId, ref fieldList, ref paramList, parameters, -1);
                    AddQueryParam("sta", "KOD_STATUS", orgStatusId, ref fieldList, ref paramList, parameters, -1);
                    AddQueryParam("gosp", "KOD_FORM_GOSP", formGospId, ref fieldList, ref paramList, parameters, -1);
                    AddQueryParam("orgf", "KOD_ORG_FORM", orgTypeId, ref fieldList, ref paramList, parameters, -1);
                    AddQueryParam("dirfio", "FIO_BOSS", directorFIO, ref fieldList, ref paramList, parameters, 70);
                    AddQueryParam("dirtel", "TEL_BOSS", directorTel, ref fieldList, ref paramList, parameters, 23);
                    AddQueryParam("buhfio", "FIO_BUH", buhgalterFIO, ref fieldList, ref paramList, parameters, 70);
                    AddQueryParam("buhtel", "TEL_BUH", buhgalterTel, ref fieldList, ref paramList, parameters, 23);
                    AddQueryParam("distr", "KOD_RAYON2", districtId.HasValue ? districtId.Value : -1, ref fieldList, ref paramList, parameters, -1);
                    AddQueryParam("strt", "NAME_UL", street, ref fieldList, ref paramList, parameters, 100);
                    AddQueryParam("nom", "NOMER_DOMA", nomer, ref fieldList, ref paramList, parameters, 30);
                    AddQueryParam("korp", "NOMER_KORPUS", korpus, ref fieldList, ref paramList, parameters, 20);
                    AddQueryParam("zcod", "POST_INDEX", zipCode, ref fieldList, ref paramList, parameters, 18);

                    string query = "INSERT INTO SORG_1NF (" + fieldList + ") VALUES (" + paramList + ")";

                    using (FbCommand commandInsert = new FbCommand(query, connection1NF))
                    {
                        try
                        {
                            commandInsert.Parameters.Add(new FbParameter("orgd", newOrgId));
                            commandInsert.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
                            commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                            commandInsert.Parameters.Add(new FbParameter("zkpo", zkpo.Length > 14 ? zkpo.Substring(0, 14) : zkpo));
                            commandInsert.Parameters.Add(new FbParameter("fname", fullName.Length > 252 ? fullName.Substring(0, 252) : fullName));
                            commandInsert.Parameters.Add(new FbParameter("sname", shortName.Length > 100 ? shortName.Substring(0, 100) : shortName));

                            foreach (KeyValuePair<string, object> pair in parameters)
                            {
                                commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                            }

                            commandInsert.Transaction = transaction;
                            commandInsert.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                            throw;
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();
                    transaction = null; // This will prevent an undesired Rollback() in the catch{} section

                    // Save the created organization in our local cache
                    Organization1NF org = new Organization1NF();

                    org.organizationId = newOrgId;
                    org.zkpo = zkpo;
                    org.fullName = fullName;
                    org.shortName = shortName;
                    org.industryId = industryId;
                    org.occupationId = occupationId;
                    org.statusId = orgStatusId;
                    org.orgTypeId = orgTypeId;
                    org.formGospId = formGospId;
                    org.ownershipFormId = ownershipId;
                    org.addrStreet = street;
                    org.addrNomer = nomer;
                    org.addrKorpus = korpus;
                    org.addrZipCode = zipCode;
                    org.addrDistrictId = districtId.HasValue ? districtId.Value : -1;
                    org.directorFIO = directorFIO;
                    org.directorTel = directorTel;
                    org.buhgalterFIO = buhgalterFIO;
                    org.buhgalterTel = buhgalterTel;
                    org.fax = fax;

                    organizations1NF.Add(newOrgId, org);
                }
                catch (Exception ex)
                {
                    newOrgId = -1;

                    // Roll back the transaction
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }

                connection1NF.Close();
            }

            return newOrgId;
        }

        public static List<ActObject> GetDocObjects(Preferences preferences, int documentId)
        {
            List<ActObject> list = new List<ActObject>();

            // Test connections
            FbConnection connectionNJF = new FbConnection(preferences.GetNJFConnectionString(UserName, UserPassword));

            try
            {
                connectionNJF.Open();
                connectionNJF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних 'Розпорядження': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return list;
            }

            FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString());

            try
            {
                connection1NF.Open();
                connection1NF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return list;
            }

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                connectionNJF.Open();
                connection1NF.Open();

                // Get all records from OBJECT_DOKS_PROPERTIES table related to this document
                using (FbCommand command = connectionNJF.CreateCommand())
                {
                    try
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = @"SELECT 
                            odp.ID, odp.OBJECT_KOD, odp.NAME, odp.SQUARE, obj.ULNAME, obj.NOMER1, obj.NOMER2, obj.NOMER3, obj.ADRDOP, obj.NEWDISTR, 
                            odp.GRPURP, odp.PURPOSE, odp.OBJKIND, odp.OBJTYPE, odp.SUMMA_BALANS, odp.SUMMA_ZAL, odp.FLOORS,
                            odp.CHARACTERISTIC, odp.YEAR_BUILD, odp.YEAR_EXPL, odp.TEXSTAN, odp.LEN, odp.DIAM_TRUB
                            FROM OBJECT_DOKS_PROPERTIES odp INNER JOIN OBJECT obj ON obj.OBJECT_KOD = odp.OBJECT_KOD AND obj.OBJECT_KODSTAN = 1
                            WHERE odp.DOK_ID = @docid";

                        command.Parameters.Add(new FbParameter("docid", documentId));

                        using (FbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    ActObject actObj = new ActObject();

                                    actObj.objectDocsPropertiesId_NJF = reader.GetInt32(0);
                                    actObj.objectId_NJF = reader.GetInt32(1);
                                    actObj.objectName_NJF = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
                                    actObj.objectSquare_NJF = reader.IsDBNull(3) ? null : reader.GetValue(3);

                                    actObj.addrStreet_NJF = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim().ToUpper();
                                    actObj.addrNomer1_NJF = reader.IsDBNull(5) ? "" : reader.GetString(5).Trim().ToUpper();
                                    actObj.addrNomer2_NJF = reader.IsDBNull(6) ? "" : reader.GetString(6).Trim().ToUpper();
                                    actObj.addrNomer3_NJF = reader.IsDBNull(7) ? "" : reader.GetString(7).Trim().ToUpper();
                                    actObj.addrMisc_NJF = reader.IsDBNull(8) ? "" : reader.GetString(8).Trim().ToUpper();
                                    actObj.districtId_NJF = reader.IsDBNull(9) ? null : reader.GetValue(9);

                                    actObj.purposeGroupIdNJF = reader.IsDBNull(10) ? -1 : reader.GetInt32(10);
                                    actObj.purposeIdNJF = reader.IsDBNull(11) ? -1 : reader.GetInt32(11);
                                    actObj.objectKindIdNJF = reader.IsDBNull(12) ? -1 : reader.GetInt32(12);
                                    actObj.objectTypeIdNJF = reader.IsDBNull(13) ? -1 : reader.GetInt32(13);

                                    actObj.objectBalansCost_NJF = reader.IsDBNull(14) ? null : reader.GetValue(14);
                                    actObj.objectFinalCost_NJF = reader.IsDBNull(15) ? null : reader.GetValue(15);

                                    actObj.objectFloorsInt_NJF = reader.IsDBNull(16) ? -1 : reader.GetInt32(16);

                                    if (actObj.objectFloorsInt_NJF >= 0)
                                        actObj.objectFloorsStr_NJF = actObj.objectFloorsInt_NJF.ToString();

                                    actObj.characteristicNJF = reader.IsDBNull(17) ? null : reader.GetValue(17);
                                    actObj.yearBuildNJF = reader.IsDBNull(18) ? null : reader.GetValue(18);
                                    actObj.yearExplNJF = reader.IsDBNull(19) ? null : reader.GetValue(19);
                                    actObj.techStateIdNJF = reader.IsDBNull(20) ? null : reader.GetValue(20);
                                    actObj.objectLen_NJF = reader.IsDBNull(21) ? null : reader.GetValue(21);
                                    actObj.objectDiamTrub_NJF = reader.IsDBNull(22) ? null : reader.GetValue(22);

                                    list.Add(actObj);
                                }
                            }

                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(command.CommandText, command, ex.Message);
                        throw;
                    }
                }

                foreach (ActObject actObj in list)
                {
                    actObj.DeduceObjectTypeFor1NF();
                }

                // Build transfers for each Act Object
                foreach (ActObject actObj in list)
                {
                    string query = @"SELECT FROM_ORG, TOORG, PRAVO FROM PRAVA_PROECT WHERE RD = @rdoc AND PRAVO IN (2, 3, 6, 7, 8, 9)";

                    using (FbCommand cmd = new FbCommand(query, connectionNJF))
                    {
                        try
                        {
                            cmd.Parameters.Add(new FbParameter("rdoc", actObj.objectDocsPropertiesId_NJF));

                            using (FbDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int organizationFromId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                                    int organizationToId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                                    int rightId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);

                                    if (organizationFromId > 0 || organizationToId > 0)
                                    {
                                        BalansTransfer balTransfer = new BalansTransfer();

                                        balTransfer.objectId_NJF = actObj.objectId_NJF;
                                        balTransfer.objectId_1NF = actObj.objectId_1NF;
                                        balTransfer.organizationFromId_NJF = organizationFromId;
                                        balTransfer.organizationToId_NJF = organizationToId;

                                        Organization1NF org = null;

                                        if (organizationFromId > 0 && organizationsNJF.TryGetValue(organizationFromId, out org))
                                        {
                                            balTransfer.orgFromZkpo_NJF = org.zkpo;
                                            balTransfer.orgFromFullName_NJF = org.fullName;
                                            balTransfer.orgFromShortName_NJF = org.shortName;
                                        }

                                        if (organizationToId > 0 && organizationsNJF.TryGetValue(organizationToId, out org))
                                        {
                                            balTransfer.orgToZkpo_NJF = org.zkpo;
                                            balTransfer.orgToFullName_NJF = org.fullName;
                                            balTransfer.orgToShortName_NJF = org.shortName;
                                        }

                                        if (actObj.objectSquare_NJF is decimal)
                                        {
                                            balTransfer.sqr = (decimal)actObj.objectSquare_NJF;
                                        }

                                        // Deduce the type of balans transfer
                                        if (rightId == 7) // Znesennya
                                        {
                                            balTransfer.transferType = ObjectTransferType.Destroy;
                                        }
                                        else if (organizationToId < 0)
                                        {
                                            balTransfer.transferType = ObjectTransferType.Destroy;
                                        }
                                        else if (organizationFromId < 0)
                                        {
                                            balTransfer.transferType = ObjectTransferType.Create;
                                        }

                                        actObj.balansTransfers.Add(balTransfer);
                                    }
                                }

                                reader.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            SqlErrorForm.ShowSqlErrorMessageDlg(query, cmd, ex.Message);
                            throw;
                        }
                    }
                }

                connection1NF.Close();
                connectionNJF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних: " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            return list;
        }

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

        public static void ExportDocument(Preferences preferences, ImportedDoc doc)
        {
            manualStreetCodeMappingNJFto1NF = ObjectFinder.GetStreetCodeMatchNJFto1NF();

            FbConnection connectionNJF = new FbConnection(preferences.GetNJFConnectionString(UserName, UserPassword));

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                connectionNJF.Open();

                // Initialize object and organization finders
                ObjectFinder objectFinder = ObjectFinder.Instance;
                OrganizationFinder orgFinder = new OrganizationFinder();

                objectFinder.BuildObjectCacheFromNJF(connectionNJF);
                orgFinder.BuildOrganizationCacheFromNJF(connectionNJF, false);

                // Make all changes to the Firebird database in a transaction
                FbTransaction transaction = null;

                try
                {
                    transaction = connectionNJF.BeginTransaction();

                    // Create the document
                    int documentId = CreateDocumentNJF(connectionNJF, transaction,
                        doc.docTypeId, doc.docNum, doc.docTitle, doc.docDate, doc.docSum, doc.docFinalSum);

                    if (documentId > 0)
                    {
                        // Create relation to the main document
                        AddRelationToPrimaryDocNJF(connectionNJF, transaction, documentId, doc.masterDocNum, doc.masterDocDate);

                        // Create relations to all objects
                        ExportDocObjects(connectionNJF, transaction, doc, documentId, objectFinder, orgFinder);

                        // Save the created document in our local data structures
                        RegisterNJFDocument(documentId, doc);
                    }

                    // Commit the transaction
                    transaction.Commit();

                    // Tell user that everything is OK
                    System.Windows.Forms.MessageBox.Show("Завантаження даних до бази 'Розпорядження' завершено успішно.",
                        "Завантаження даних",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    // Roll back the transaction
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                }

                connectionNJF.Close();

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних 'Розпорядження'",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private static int CreateDocumentNJF(FbConnection connectionNJF, FbTransaction transaction,
            int docTypeId, string docNum, string docTitle, DateTime docDate, decimal docSum, decimal docFinalSum)
        {
            // Generate a new ID for the document
            int documentId = GenerateNewFirebirdId(connectionNJF, "ROZP_DOK_GEN", transaction);

            // Prepare the INSERT statement
            string query = "";

            if (docTypeId == 3) // Act
            {
                query = "INSERT INTO ACTS (DOK_ID, DOKKIND, PIDROZDIL, STATUS, DOKDATA, DOKNUM, DOKTEMA, PRIZNAK, DT, ISP, DT_INSERT, ISP_INSERT, SUMMA, SUMMA_ZAL)" +
                    " VALUES (@docid, @doctype, 1, 2, @docdt, @docnum, @topic, 1, @edt, @usr, @edt, @usr, @sumtot, @sumfin)";
            }
            else
            {
                query = "INSERT INTO ROZP_DOK (DOK_ID, DOKKIND, PIDROZDIL, STATUS, DOKDATA, DOKNUM, DOKTEMA, PRIZNAK, DT, ISP, DT_INSERT, ISP_INSERT, SUMMA, SUMMA_ZAL)" +
                    " VALUES (@docid, @doctype, 1, 2, @docdt, @docnum, @topic, 1, @edt, @usr, @edt, @usr, @sumtot, @sumfin)";
            }

            using (FbCommand commandInsert = new FbCommand(query, connectionNJF))
            {
                try
                {
                    commandInsert.Parameters.Add(new FbParameter("docid", documentId));
                    commandInsert.Parameters.Add(new FbParameter("doctype", docTypeId));
                    commandInsert.Parameters.Add(new FbParameter("docdt", docDate.Date));
                    commandInsert.Parameters.Add(new FbParameter("docnum", (docNum.Length > 20) ? docNum.Substring(0, 20) : docNum));
                    commandInsert.Parameters.Add(new FbParameter("topic", (docTitle.Length > 255) ? docTitle.Substring(0, 255) : docTitle));
                    commandInsert.Parameters.Add(new FbParameter("sumtot", docSum));
                    commandInsert.Parameters.Add(new FbParameter("sumfin", docFinalSum));
                    commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
                    commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));

                    commandInsert.Transaction = transaction;
                    commandInsert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                    throw;
                }
            }

            return documentId;
        }

        private static void AddRelationToPrimaryDocNJF(FbConnection connectionNJF, FbTransaction transaction,
            int documentId, string masterDocNum, DateTime masterDocDate)
        {
            if (masterDocNum.Length > 0)
            {
                int parentDocId = DB.FindDocument(masterDocNum, masterDocDate.Date, -1);

                if (parentDocId >= 0)
                {
                    string query = "INSERT INTO DOK_DEPEND (DOK_KOD1, DOK_KOD2, KIND, DT, ISP) VALUES (@chi, @pare, 1, @edt, @usr)";

                    using (FbCommand commandInsert = new FbCommand(query, connectionNJF))
                    {
                        try
                        {
                            commandInsert.Parameters.Add(new FbParameter("chi", documentId));
                            commandInsert.Parameters.Add(new FbParameter("pare", parentDocId));
                            commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
                            commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));

                            commandInsert.Transaction = transaction;
                            commandInsert.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                            throw;
                        }
                    }
                }
            }
        }

        private static void ExportDocObjects(FbConnection connection, FbTransaction transaction, ImportedDoc doc, int documentId,
            ObjectFinder objectFinder, OrganizationFinder orgFinder)
        {
            foreach (Appendix appendix in doc.appendices)
            {
                foreach (AppendixObject obj in appendix.objects)
                {
                    if (obj.object1NF != null)
                    {
                        int matchObjectId = FindObjectMatchInNJF(connection, transaction, obj.object1NF, objectFinder);

                        // Generate a new object-doc relation ID
                        int relationId = GenerateNewFirebirdId(connection, "OBJECT_DOKS_GEN", transaction);

                        // Insert the relation into OBJECT_DOKS table
                        string query = "INSERT INTO OBJECT_DOKS (ID, DOK_ID, OBJECT_KOD, OBJECT_KODSTAN, DTISP, ISP) VALUES (@relid, @docid, @objid, 0, @edt, @usr)";

                        using (FbCommand commandInsert = new FbCommand(query, connection))
                        {
                            try
                            {
                                commandInsert.Parameters.Add(new FbParameter("relid", relationId));
                                commandInsert.Parameters.Add(new FbParameter("docid", documentId));
                                commandInsert.Parameters.Add(new FbParameter("objid", matchObjectId));
                                commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
                                commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

                                commandInsert.Transaction = transaction;
                                commandInsert.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                                throw;
                            }
                        }

                        // At this point a new record might be already added to the OBJECT_DOKS_PROPERTIES table via a trigger. Check if this is so
                        bool objectDocPropertiesExists = false;

                        using (FbCommand cmd = new FbCommand("SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE ID = @relid", connection))
                        {
                            try
                            {
                                cmd.Parameters.Add(new FbParameter("relid", relationId));
                                cmd.Transaction = transaction;

                                using (FbDataReader r = cmd.ExecuteReader())
                                {
                                    if (r.Read())
                                    {
                                        objectDocPropertiesExists = true;
                                    }

                                    r.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                SqlErrorForm.ShowSqlErrorMessageDlg(query, cmd, ex.Message);
                                throw;
                            }
                        }

                        Dictionary<string, object> parameters = new Dictionary<string, object>();

                        parameters["@relid"] = relationId;
                        parameters["@edt"] = DateTime.Now.Date;
                        parameters["@usr"] = UserName.Length > 18 ? UserName.Substring(0, 18) : UserName;

                        string fieldList = "";
                        string paramList = "";

                        if (objectDocPropertiesExists)
                        {
                            // Prepare an UPDATE query for the OBJECT_DOKS_PROPERTIES table
                            fieldList = "TEXSTAN = 1, DT = @edt, ISP = @usr";
                           
                        }
                        else
                        {
                            // Prepare an INSERT query for the OBJECT_DOKS_PROPERTIES table
                            fieldList = "ID, OBJECT_KOD, OBJECT_KODSTAN, DOK_ID, TEXSTAN, DT, ISP";
                            paramList = "@relid, @objid, 0, @docid, 1, @edt, @usr";

                            parameters["@objid"] = matchObjectId;
                            parameters["@docid"] = documentId;

                            query = "INSERT INTO OBJECT_DOKS_PROPERTIES (" + fieldList + ") VALUES (" + paramList + ")";
                        }

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "CHARACTERISTIC", "@char", obj,
                            ColumnCategory.Characteristics, FbDbType.VarChar, "", objectDocPropertiesExists, 100);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "GRPURP", "@purpgr", obj,
                            ColumnCategory.PurposeGroup, FbDbType.Integer, DICT_PURPOSE_GROUP, objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "PURPOSE", "@purp", obj,
                            ColumnCategory.Purpose, FbDbType.Integer, DICT_PURPOSE, objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "SQUARE", "@sqre", obj,
                            ColumnCategory.Square, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "OBJKIND", "@knd", obj,
                            ColumnCategory.ObjectKind, FbDbType.Integer, DICT_OBJ_KIND, objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "OBJTYPE", "@typ", obj,
                            ColumnCategory.ObjectType, FbDbType.Integer, DICT_OBJ_TYPE, objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "SUMMA_BALANS", "@sumbal", obj,
                            ColumnCategory.BalansCost, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "SUMMA_ZAL", "@sumzal", obj,
                            ColumnCategory.FinalCost, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "SUMMA_BALANS_0", "@sumbal0", obj,
                            ColumnCategory.BalansCost, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "SUMMA_ZAL_0", "@sumzal0", obj,
                            ColumnCategory.FinalCost, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "NAME", "@objname", obj,
                            ColumnCategory.ObjectName, FbDbType.VarChar, "", objectDocPropertiesExists, 255);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "YEAR_BUILD", "@yearb", obj,
                            ColumnCategory.BuildYear, FbDbType.Integer, "", objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "YEAR_EXPL", "@yearexp", obj,
                            ColumnCategory.ExplYear, FbDbType.Integer, "", objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "LEN", "@objlen", obj,
                            ColumnCategory.Length, FbDbType.Decimal, "", objectDocPropertiesExists, -1);

                        AddQueryParameter(parameters, ref fieldList, ref paramList, "DIAM_TRUB", "@diam", obj,
                            ColumnCategory.Diameter, FbDbType.VarChar, "", objectDocPropertiesExists, 20);

                        if (objectDocPropertiesExists)
                        {
                            query = "UPDATE OBJECT_DOKS_PROPERTIES SET " + fieldList + " WHERE ID = @relid";
                        }
                        else
                        {
                            query = "INSERT INTO OBJECT_DOKS_PROPERTIES (" + fieldList + ") VALUES (" + paramList + ")";
                        }

                        using (FbCommand commandInsert = new FbCommand(query, connection))
                        {
                            try
                            {
                                foreach (KeyValuePair<string, object> param in parameters)
                                {
                                    if (param.Key.StartsWith("@"))
                                    {
                                        commandInsert.Parameters.Add(new FbParameter(param.Key.Substring(1), param.Value));
                                    }
                                    else
                                    {
                                        commandInsert.Parameters.Add(new FbParameter(param.Key, param.Value));
                                    }
                                }

                                commandInsert.Transaction = transaction;
                                commandInsert.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                                throw;
                            }
                        }

                        // Export the object rights (for Acts this is not needed)
                        if (!doc.IsAct())
                        {
                            ExportObjectRights(connection, transaction, doc, obj, matchObjectId, documentId, relationId, orgFinder);
                        }
                    }
                }
            }
        }

        private static void ExportObjectRights(FbConnection connection, FbTransaction transaction, ImportedDoc doc,
            AppendixObject obj, int matchObjectId, int documentId, int relationId, OrganizationFinder orgFinder)
        {
            foreach (Transfer t in obj.transfers)
            {
                if (t.rightId > 0)
                {
                    // Perform matching of 1NF organizations to the NJF organizations
                    int matchOrgFromId = -1;
                    int matchOrgToId = -1;

                    if (t.orgFrom != null)
                        matchOrgFromId = FindOrganizationMatchInNJF(connection, transaction, t.orgFrom, orgFinder);

                    if (t.orgTo != null)
                        matchOrgToId = FindOrganizationMatchInNJF(connection, transaction, t.orgTo, orgFinder);

                    // Add record to the PRAVA_PROECT table
                    if (matchOrgFromId > 0 || matchOrgToId > 0)
                    {
                        int pravaProjectId = GenerateNewFirebirdId(connection, "GEN_PRAVA_PROECT_ID", transaction);

                        if (pravaProjectId > 0)
                        {
                            string query = "INSERT INTO PRAVA_PROECT (ID, RD, FROM_ORG, FROM_ORG_STAN, TOORG, TOORG_STAN, PRAVO, DT, ISP) VALUES" +
                                " (@ppid, @relid, <FROMORG>, <FROMORGSTAN>, <TOORG>, <TOORGSTAN>, @rightid, @edt, @usr)";

                            if (t.orgFrom != null)
                            {
                                query = query.Replace("<FROMORG>", "@fromid");
                                query = query.Replace("<FROMORGSTAN>", "0");
                            }
                            else
                            {
                                query = query.Replace("<FROMORG>", "NULL");
                                query = query.Replace("<FROMORGSTAN>", "NULL");
                            }

                            if (t.orgTo != null)
                            {
                                query = query.Replace("<TOORG>", "@toid");
                                query = query.Replace("<TOORGSTAN>", "0");
                            }
                            else
                            {
                                query = query.Replace("<TOORG>", "NULL");
                                query = query.Replace("<TOORGSTAN>", "NULL");
                            }

                            using (FbCommand commandInsert = new FbCommand(query, connection))
                            {
                                try
                                {
                                    commandInsert.Parameters.Add(new FbParameter("ppid", pravaProjectId));
                                    commandInsert.Parameters.Add(new FbParameter("relid", relationId));
                                    commandInsert.Parameters.Add(new FbParameter("rightid", t.rightId));
                                    commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
                                    commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

                                    if (matchOrgFromId > 0)
                                    {
                                        commandInsert.Parameters.Add(new FbParameter("fromid", matchOrgFromId));
                                    }

                                    if (matchOrgToId > 0)
                                    {
                                        commandInsert.Parameters.Add(new FbParameter("toid", matchOrgToId));
                                    }

                                    commandInsert.Transaction = transaction;
                                    commandInsert.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                                    throw;
                                }
                            }
                        }
                    }

                    // Add records to the OBJECT_PRAVO table
                    int fromRecordId = -1;
                    int toRecordId = -1;

                    if (matchOrgFromId > 0)
                    {
                        fromRecordId = GenerateNewFirebirdId(connection, "OBJECT_PRAVO_GEN", transaction);

                        if (fromRecordId > 0)
                        {
                            // Create the "from" entry in the OBJECT_PRAVO table
                            string query = "INSERT INTO OBJECT_PRAVO (ID, OBJECT_KOD, OBJECT_KODSTAN, ZKPO, ZKPO_STAN, PRAVO, END_DOK, END_DATE, STDOKOBJID, DT, ISP)" +
                                " VALUES (@fromid, @objid, 0, @orgid, 0, @rid, @docid, @docdt, @relid, @edt, @usr)";

                            using (FbCommand commandInsert = new FbCommand(query, connection))
                            {
                                try
                                {
                                    commandInsert.Parameters.Add(new FbParameter("fromid", fromRecordId));
                                    commandInsert.Parameters.Add(new FbParameter("objid", matchObjectId));
                                    commandInsert.Parameters.Add(new FbParameter("orgid", matchOrgFromId));
                                    commandInsert.Parameters.Add(new FbParameter("rid", t.rightId));
                                    commandInsert.Parameters.Add(new FbParameter("docid", documentId));
                                    commandInsert.Parameters.Add(new FbParameter("docdt", doc.docDate.Date));
                                    commandInsert.Parameters.Add(new FbParameter("relid", relationId));
                                    commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
                                    commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

                                    commandInsert.Transaction = transaction;
                                    commandInsert.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                                    throw;
                                }
                            }
                        }
                    }

                    if (matchOrgToId > 0)
                    {
                        toRecordId = GenerateNewFirebirdId(connection, "OBJECT_PRAVO_GEN", transaction);

                        if (toRecordId > 0)
                        {
                            // Create the "to" entry in the OBJECT_PRAVO table
                            string query = "INSERT INTO OBJECT_PRAVO (ID, OBJECT_KOD, OBJECT_KODSTAN, ZKPO, ZKPO_STAN, PRAVO, START_DOK, START_DATE, STDOKOBJID, DT, ISP)" +
                                " VALUES (@toid, @objid, 0, @orgid, 0, @rid, @docid, @docdt, @relid, @edt, @usr)";

                            using (FbCommand commandInsert = new FbCommand(query, connection))
                            {
                                try
                                {
                                    commandInsert.Parameters.Add(new FbParameter("toid", toRecordId));
                                    commandInsert.Parameters.Add(new FbParameter("objid", matchObjectId));
                                    commandInsert.Parameters.Add(new FbParameter("orgid", matchOrgToId));
                                    commandInsert.Parameters.Add(new FbParameter("rid", t.rightId));
                                    commandInsert.Parameters.Add(new FbParameter("docid", documentId));
                                    commandInsert.Parameters.Add(new FbParameter("docdt", doc.docDate.Date));
                                    commandInsert.Parameters.Add(new FbParameter("relid", relationId));
                                    commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
                                    commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

                                    commandInsert.Transaction = transaction;
                                    commandInsert.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                                    throw;
                                }
                            }

                            // Update the "from" record - add relation to the "to" record
                            if (fromRecordId > 0)
                            {
                                query = "UPDATE OBJECT_PRAVO SET CLOSINGKARD = @toid WHERE ID = @fromid";

                                using (FbCommand commandInsert = new FbCommand(query, connection))
                                {
                                    try
                                    {
                                        commandInsert.Parameters.Add(new FbParameter("fromid", fromRecordId));
                                        commandInsert.Parameters.Add(new FbParameter("toid", toRecordId));

                                        commandInsert.Transaction = transaction;
                                        commandInsert.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void AddQueryParameter(Dictionary<string, object> parameters,
            ref string fieldList, ref string paramList, string fieldName, string paramName,
            AppendixObject obj, ColumnCategory category, FbDbType dbType, string dictionaryName, bool isUpdate, int maxStringLen)
        {
            string value = "";
            int code = -1;

            if (obj.properties.TryGetValue(category, out value))
            {
                object val = value;

                // Convert the object to the proper data type
                try
                {
                    switch (dbType)
                    {
                        case FbDbType.VarChar:
                            if (maxStringLen > 0 && value.Length > maxStringLen)
                            {
                                value = value.Substring(0, maxStringLen);
                            }
                            break;

                        case FbDbType.Decimal:
                            val = decimal.Parse(value);
                            break;

                        case FbDbType.Integer:
                            if (dictionaryName.Length > 0)
                            {
                                code = DB.FindCodeInDictionaryNJF(dictionaryName, value);

                                val = (code >= 0) ? (object)code : null;
                            }
                            else
                            {
                                val = int.Parse(value);
                            }
                            break;
                    }
                }
                catch (Exception)
                {
                    val = null;
                }

                if (val != null)
                {
                    parameters.Add(paramName, val);

                    if (fieldList.Length > 0)
                        fieldList += ", ";

                    if (isUpdate)
                    {
                        // Format for the UPDATE statement
                        fieldList += fieldName + " = " + paramName;
                    }
                    else
                    {
                        // Format for the INSERT statement
                        fieldList += fieldName;
                    }

                    if (paramList.Length > 0)
                        paramList += ", ";

                    paramList += paramName;
                }
            }
        }

        private static void RegisterNJFDocument(int documentId, ImportedDoc doc)
        {
            DocumentNJF doc1nf = new DocumentNJF();

            doc1nf.documentId = documentId;
            doc1nf.documentKind = doc.docTypeId;
            doc1nf.documentDate = doc.docDate;
            doc1nf.documentNumber = doc.docNum.Trim().ToUpper();
            doc1nf.documentTitle = doc.docTitle.Trim().ToUpper();

            if (doc.docTypeId == 3) // AKT
            {
                actsNJF.Add(documentId, doc1nf);
            }
            else
            {
                documentsNJF.Add(documentId, doc1nf);
            }
        }

        private static void RegisterNJFAct(int documentId, ImportedAct act, DocumentNJF rishennya)
        {
            DocumentNJF doc1nf = new DocumentNJF();

            doc1nf.documentId = documentId;
            doc1nf.documentKind = 3;
            doc1nf.documentDate = act.docDate;
            doc1nf.documentNumber = act.docNum.Trim().ToUpper();
            doc1nf.documentTitle = act.docTitle.Trim().ToUpper();

            actsNJF.Add(documentId, doc1nf);

            if (rishennya.dependentDocuments == null)
                rishennya.dependentDocuments = new List<int>();

            rishennya.dependentDocuments.Add(documentId);
        }

        private static int GenerateNewFirebirdId(FbConnection connection, string generatorName, FbTransaction transaction)
        {
            int objectId = -1;

            string query = "SELECT GEN_ID(" + generatorName + ", 1) FROM RDB$DATABASE";

            using (FbCommand command = new FbCommand(query, connection))
            {
                command.Transaction = transaction;

                using (FbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            objectId = reader.GetInt32(0);
                        }
                    }

                    reader.Close();
                }
            }

            return objectId;
        }

        public static bool ExportAct(Preferences preferences, ImportedAct act, DocumentNJF rishennya)
        {
            bool result = false;
            manualStreetCodeMappingNJFto1NF = ObjectFinder.GetStreetCodeMatchNJFto1NF();

            // Test connections
            FbConnection connectionNJF = new FbConnection(preferences.GetNJFConnectionString(UserName, UserPassword));

            try
            {
                connectionNJF.Open();
                connectionNJF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних 'Розпорядження': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return false;
            }


            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                connectionNJF.Open();

                // Make all changes to the Firebird database in a transaction
                FbTransaction transaction = null;

                try
                {
                    transaction = connectionNJF.BeginTransaction();

                    // Create the document
                    int documentId = CreateDocumentNJF(connectionNJF, transaction,
                        3, act.docNum, act.docTitle, act.docDate, act.docSum, act.docFinalSum);

                    if (documentId > 0)
                    {
                        // Create relation to the main document
                        AddRelationToPrimaryDocNJF(connectionNJF, transaction, documentId, rishennya.documentNumber, rishennya.documentDate);

                        // Create relations to all objects
                        ExportActObjects(connectionNJF, transaction, act, documentId);

                        // Save the created document in our local data structures
                        RegisterNJFAct(documentId, act, rishennya);
                    }

                    // Commit the transaction
                    transaction.Commit();
                    transaction = null;

                    result = true;
                }
                catch (Exception)
                {
                    // Roll back the transaction
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    throw;
                }

                connectionNJF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних 'Розпорядження': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            return result;
        }

        private static void ExportActObjects(FbConnection connection, FbTransaction transaction, ImportedAct act, int documentId)
        {
            foreach (ActObject obj in act.actObjects)
            {
                if (obj.objectId_NJF < 0)
                    continue;

                // If this object does not belong to Rishennya, add it to the Rishennya as well
                if (obj.objectDocsPropertiesId_NJF <= 0)
                {
                    AddActObjectToRishennya(obj, connection, transaction, documentId);
                }

                AddSingleObjectToDocument(obj, connection, transaction, documentId);
            }
        }

        private static void AddActObjectToRishennya(ActObject actObj, FbConnection connectionNJF, FbTransaction transaction, int rishennyaId)
        {
            // At this point a new record might be already added to the OBJECT_DOKS_PROPERTIES table via a trigger. Check if this is so
            FbCommand cmd = new FbCommand("", connectionNJF);

            if (actObj.objectSquare_NJF is decimal)
            {
                // If square is specified, it must match
                cmd.CommandText = "SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE DOK_ID = @doc AND OBJECT_KOD = @ob AND NAME = @nm AND SQUARE = @sq";
                cmd.Parameters.Add(new FbParameter("sq", actObj.objectSquare_NJF));
            }
            else if (actObj.objectLen_NJF is decimal)
            {
                // If length is specified, it must match
                cmd.CommandText = "SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE DOK_ID = @doc AND OBJECT_KOD = @ob AND NAME = @nm AND LEN = @l";
                cmd.Parameters.Add(new FbParameter("l", actObj.objectLen_NJF));
            }
            else
            {
                cmd.CommandText = "SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE DOK_ID = @doc AND OBJECT_KOD = @ob AND NAME = @nm";
            }

            cmd.Parameters.Add(new FbParameter("doc", rishennyaId));
            cmd.Parameters.Add(new FbParameter("ob", actObj.objectId_NJF));
            cmd.Parameters.Add(new FbParameter("nm", actObj.objectName_NJF));

            bool objectDocPropertiesExists = false;

            try
            {
                cmd.Transaction = transaction;

                using (FbDataReader r = cmd.ExecuteReader())
                {
                    objectDocPropertiesExists = r.Read();
                    r.Close();
                }
            }
            catch (Exception ex)
            {
                SqlErrorForm.ShowSqlErrorMessageDlg(cmd.CommandText, cmd, ex.Message);
                throw;
            }

            // If there is no such object in Rishennya, add it
            if (!objectDocPropertiesExists)
            {
                AddSingleObjectToDocument(actObj, connectionNJF, transaction, rishennyaId);
            }
        }

        private static void AddSingleObjectToDocument(ActObject obj, FbConnection connectionNJF, FbTransaction transaction, int documentId)
        {
            // Generate a new object-doc relation ID
            int relationId = GenerateNewFirebirdId(connectionNJF, "OBJECT_DOKS_GEN", transaction);

            // Insert the relation into OBJECT_DOKS table
            string query = "INSERT INTO OBJECT_DOKS (ID, DOK_ID, OBJECT_KOD, OBJECT_KODSTAN, DTISP, ISP) VALUES (@relid, @docid, @objid, 0, @edt, @usr)";

            using (FbCommand commandInsert = new FbCommand(query, connectionNJF))
            {
                try
                {
                    commandInsert.Parameters.Add(new FbParameter("relid", relationId));
                    commandInsert.Parameters.Add(new FbParameter("docid", documentId));
                    commandInsert.Parameters.Add(new FbParameter("objid", obj.objectId_NJF));
                    commandInsert.Parameters.Add(new FbParameter("edt", DateTime.Now.Date));
                    commandInsert.Parameters.Add(new FbParameter("usr", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

                    commandInsert.Transaction = transaction;
                    commandInsert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                    throw;
                }
            }

            // At this point a new record might be already added to the OBJECT_DOKS_PROPERTIES table via a trigger. Check if this is so
            bool objectDocPropertiesExists = false;

            query = @"SELECT FIRST 1 ID FROM OBJECT_DOKS_PROPERTIES WHERE ID = @relid";

            using (FbCommand cmd = new FbCommand(query, connectionNJF))
            {
                try
                {
                    cmd.Parameters.Add(new FbParameter("relid", relationId));
                    cmd.Transaction = transaction;

                    using (FbDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            objectDocPropertiesExists = true;
                        }

                        r.Close();
                    }
                }
                catch (Exception ex)
                {
                    SqlErrorForm.ShowSqlErrorMessageDlg(query, cmd, ex.Message);
                    throw;
                }
            }

            if (objectDocPropertiesExists)
            {
                // Update the existing record using INSERT
                using (FbCommand command = obj.PrepareUpdateStatement(connectionNJF, relationId, documentId))
                {
                    try
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(command.CommandText, command, ex.Message);
                        throw;
                    }
                }
            }
            else
            {
                // Add new record using INSERT
                using (FbCommand command = obj.PrepareInsertStatement(connectionNJF, relationId, documentId))
                {
                    try
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(command.CommandText, command, ex.Message);
                        throw;
                    }
                }
            }
        }

        public static bool TransferBalansObjects(Preferences preferences, ImportedAct act, DocumentNJF rishennya, bool modifyUserReportsImmediately)
        {
            bool result = false;
            FbConnection connection1NF = new FbConnection(preferences.Get1NFConnectionString());

            try
            {
                connection1NF.Open();
                connection1NF.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних '1НФ': " + ex.Message,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                return false;
            }

            SqlConnection connectionSql = null;

            if (modifyUserReportsImmediately)
            {
                connectionSql = new SqlConnection(preferences.GetSqlServerConnectionString());

                try
                {
                    connectionSql.Open();
                    connectionSql.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка доступу до бази даних ЕІС: " + ex.Message,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);

                    return false;
                }
            }

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                connection1NF.Open();

                if (connectionSql != null)
                    connectionSql.Open();

                foreach (ActObject obj in act.actObjects)
                {
                    if (obj.makeChangesIn1NF)
                    {
                        foreach (BalansTransfer bt in obj.balansTransfers)
                        {
                            if (bt.IsFullyDefined())
                            {
                                // Make the transition in Transaction
                                FbTransaction transaction = null;

                                try
                                {
                                    transaction = connection1NF.BeginTransaction();

                                    switch (bt.transferType)
                                    {
                                        case ObjectTransferType.Transfer:
                                            CutBalansObject(connection1NF, transaction, connectionSql, bt.balansId_1NF, bt.sqr, bt.organizationToId_1NF, rishennya);
                                            break;

                                        case ObjectTransferType.Create:
                                            CreateBalansObject(connection1NF, transaction, connectionSql, obj, bt, rishennya);
                                            break;

                                        case ObjectTransferType.Destroy:
                                            MarkBalansObjectAsDeleted(connection1NF, transaction, connectionSql, bt.balansId_1NF, rishennya, bt.organizationToId_1NF);
                                            break;
                                    }

                                    // Commit the transaction
                                    transaction.Commit();
                                }
                                catch (Exception)
                                {
                                    transaction.Rollback();
                                    throw;
                                }
                            }
                        }
                    }
                }

                if (connectionSql != null)
                    connectionSql.Close();

                connection1NF.Close();

                result = true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                System.Windows.Forms.MessageBox.Show(ex.Message, "Помилка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);

                result = false;
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            return result;
        }

        private static void MarkBalansObjectAsDeleted(FbConnection connection1NF, FbTransaction transaction, SqlConnection connectionSql,
            int balansId, DocumentNJF rishennya, int vidchOrgId)
        {
            // Mark object as DELETED in 1NF
            string fields = "DELETED = 1, DTDELETE = @dt, EDT = @dt, DT = @dt, ISP = @isp, EIS_MODIFIED_BY = @isp, " +
                "EIS_VIDCH_DOC_TYPE = @dtype, EIS_VIDCH_DOC_NUM = @dnum, EIS_VIDCH_DOC_DATE = @ddate";

            if (vidchOrgId > 0)
                fields += ", EIS_VIDCH_ORG_ID = @vorgid";

            string query = "UPDATE BALANS_1NF SET " + fields + " WHERE ID = @balid";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                try
                {
                    command.Parameters.Add(new FbParameter("balid", balansId));
                    command.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                    command.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

                    command.Parameters.Add(new FbParameter("dtype", rishennya.documentKind));
                    command.Parameters.Add(new FbParameter("dnum", rishennya.documentNumber));
                    command.Parameters.Add(new FbParameter("ddate", rishennya.documentDate.Date));

                    if (vidchOrgId > 0)
                        command.Parameters.Add(new FbParameter("vorgid", vidchOrgId));

                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SqlErrorForm.ShowSqlErrorMessageDlg(query, command, ex.Message);
                    throw;
                }
            }

            // Mark object as DELETED in Sql Server
            if (connectionSql != null)
            {
                fields = "is_deleted = 1, del_date = @dt, modify_date = @dt, modified_by = @isp, vidch_doc_type = @dtype, vidch_doc_num = @dnum, vidch_doc_date = @ddate";

                if (vidchOrgId > 0)
                    fields += ", vidch_org_id = @vorgid";

                query = "UPDATE balans SET " + fields + " WHERE id = @balid";

                using (SqlCommand command = new SqlCommand(query, connectionSql))
                {
                    bool deleteSucceeded = false;

                    try
                    {
                        command.Parameters.Add(new SqlParameter("balid", balansId));
                        command.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));
                        command.Parameters.Add(new SqlParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

                        command.Parameters.Add(new SqlParameter("dtype", rishennya.documentKind));
                        command.Parameters.Add(new SqlParameter("dnum", rishennya.documentNumber));
                        command.Parameters.Add(new SqlParameter("ddate", rishennya.documentDate.Date));

                        if (vidchOrgId > 0)
                            command.Parameters.Add(new SqlParameter("vorgid", vidchOrgId));

                        command.ExecuteNonQuery();
                        deleteSucceeded = true;
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(query, command, ex.Message);
                        throw;
                    }

                    if (deleteSucceeded)
                    {
                        // Delete the object from user report
                        using (SqlCommand cmd = new SqlCommand("dbo.fnDeleteBalansObjectInReport", connectionSql))
                        {
                            try
                            {
                                cmd.Parameters.Add(new SqlParameter("BALANS_ID", balansId));
                                cmd.Parameters.Add(new SqlParameter("COPY_TO_DELETED", 1));

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.ExecuteNonQuery();

                                Common.EmailNotifier.NotifyBalansObjectRemoved(connectionSql, null, balansId);
                            }
                            catch (Exception ex)
                            {
                                SqlErrorForm.ShowSqlErrorMessageDlg(query, cmd, ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
        }

        private static void ChangeBalansObjectSquare(FbConnection connection1NF, FbTransaction transaction, SqlConnection connectionSql,
            int balansId, decimal newSquare)
        {
            // Get the current square from 1NF
            decimal sqrTotal = -1m;
            decimal sqrNonHabit = -1m;

            string query = "SELECT SQR_ZAG, SQR_NEJ FROM BALANS_1NF WHERE ID = @balid";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                try
                {
                    command.Parameters.Add(new FbParameter("balid", balansId));
                    command.Transaction = transaction;

                    using (FbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sqrTotal = reader.IsDBNull(0) ? -1m : reader.GetDecimal(0);
                            sqrNonHabit = reader.IsDBNull(1) ? -1m : reader.GetDecimal(1);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    SqlErrorForm.ShowSqlErrorMessageDlg(query, command, ex.Message);
                    throw;
                }
            }

            // Validate parameters
            sqrTotal = newSquare;

            if (sqrNonHabit > sqrTotal)
            {
                sqrNonHabit = sqrTotal;
            }

            // Update the object in 1NF
            string fields = "SQR_ZAG = @szag, EDT = @dt, DT = @dt, ISP = @isp, EIS_MODIFIED_BY = @isp";

            if (sqrNonHabit >= 0)
                fields += ", SQR_NEJ = @snonh";

            query = "UPDATE BALANS_1NF SET " + fields + " WHERE ID = @balid";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                try
                {
                    command.Parameters.Add(new FbParameter("balid", balansId));
                    command.Parameters.Add(new FbParameter("szag", sqrTotal));
                    command.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
                    command.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

                    if (sqrNonHabit >= 0)
                        command.Parameters.Add(new FbParameter("snonh", sqrNonHabit));

                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SqlErrorForm.ShowSqlErrorMessageDlg(query, command, ex.Message);
                    throw;
                }
            }

            // Update the object in SQL Server
            if (connectionSql != null)
            {
                fields = "sqr_total = @szag, modify_date = @dt, modified_by = @isp";

                if (sqrNonHabit >= 0)
                    fields += ", sqr_non_habit = @snonh";

                query = "UPDATE balans SET " + fields + " WHERE id = @balid";

                using (SqlCommand command = new SqlCommand(query, connectionSql))
                {
                    try
                    {
                        command.Parameters.Add(new SqlParameter("balid", balansId));
                        command.Parameters.Add(new SqlParameter("szag", sqrTotal));
                        command.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));
                        command.Parameters.Add(new SqlParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));

                        if (sqrNonHabit >= 0)
                            command.Parameters.Add(new SqlParameter("snonh", sqrNonHabit));

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(query, command, ex.Message);
                        throw;
                    }
                }
            }
        }

        private static int CopyBalansObject(FbConnection connection1NF, FbTransaction transaction, SqlConnection connectionSql,
            int balansId, int newOwnerOrgId, DocumentNJF rishennya)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            GUKV.ImportToolUtils.FieldMappings.Create1NFBalansFieldMapping(mapping, false, true);

            // Generate new balans Id
            int newBalansId = GenerateNewFirebirdId(connection1NF, "GEN_BALANS_1NF", transaction);

            if (newBalansId > 0)
            {
                //  Generate parameters for the INSERT command
                FbCommand cmdInsert1NF = new FbCommand("", connection1NF);
                SqlCommand cmdInsertSql = new SqlCommand();

                string insertFields1NF = "";
                string insertFieldsSql = "";
                string insertParams1NF = "";
                string insertParamsSql = "";
                bool ISP_exists = false;

                string query = "SELECT * FROM BALANS_1NF WHERE ID = @balid";

                using (FbCommand command = new FbCommand(query, connection1NF))
                {
                    command.Parameters.Add(new FbParameter("balid", balansId));
                    command.Transaction = transaction;

                    using (FbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string paramName = "p" + i.ToString();
                                string fieldName1NF = reader.GetName(i).ToUpper();
                                object value = reader.IsDBNull(i) ? null : reader.GetValue(i);

                                // Skip computed columns
                                if (fieldName1NF != "OBJ_NOMER")
                                {
                                    // Some columns must be filled with new values
                                    if (fieldName1NF == "ID")
                                    {
                                        value = newBalansId;
                                    }
                                    else if (fieldName1NF == "ORG")
                                    {
                                        value = newOwnerOrgId;
                                    }
                                    else if (fieldName1NF == "ISP")
                                    {
                                        value = UserName.Length > 18 ? UserName.Substring(0, 18) : UserName;
                                        ISP_exists = true;
                                    }
                                    else if (fieldName1NF == "EIS_MODIFIED_BY")
                                    {
                                        value = UserName.Length > 64 ? UserName.Substring(0, 64) : UserName;
                                    }
                                    else if (fieldName1NF == "DT" || fieldName1NF == "EDT")
                                    {
                                        value = DateTime.Now.Date;
                                    }
                                    else if (fieldName1NF == "EIS_BALANS_DOC_TYPE")
                                    {
                                        value = rishennya.documentKind;
                                    }
                                    else if (fieldName1NF == "EIS_BALANS_DOC_NUM")
                                    {
                                        value = rishennya.documentNumber;
                                    }
                                    else if (fieldName1NF == "EIS_BALANS_DOC_DATE")
                                    {
                                        value = rishennya.documentDate.Date;
                                    }

                                    if (value != null)
                                    {
                                        cmdInsert1NF.Parameters.Add(new FbParameter(paramName, value));

                                        insertFields1NF += ", " + fieldName1NF;
                                        insertParams1NF += ", @" + paramName;

                                        // Get the Sql Server column by the name of 1NF column, and prepare INSERT for Sql Server
                                        string fieldNameSql = "";

                                        if (mapping.TryGetValue(fieldName1NF, out fieldNameSql))
                                        {
                                            cmdInsertSql.Parameters.Add(new SqlParameter(paramName, value));

                                            insertFieldsSql += ", " + fieldNameSql;
                                            insertParamsSql += ", @" + paramName;
                                        }
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }
                }

                // We need to update the ISP field in 1NF, even if it is not mapped
                if (!ISP_exists)
                {
                    insertFields1NF += ", ISP";
                    insertParams1NF += ", @isp";
                    cmdInsert1NF.Parameters.Add(new FbParameter("isp", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName));
                }

                if (insertFields1NF.StartsWith(", "))
                    insertFields1NF = insertFields1NF.Substring(2);

                if (insertParams1NF.StartsWith(", "))
                    insertParams1NF = insertParams1NF.Substring(2);

                if (insertFieldsSql.StartsWith(", "))
                    insertFieldsSql = insertFieldsSql.Substring(2);

                if (insertParamsSql.StartsWith(", "))
                    insertParamsSql = insertParamsSql.Substring(2);

                // Copy the balans object in 1NF
                try
                {
                    cmdInsert1NF.Transaction = transaction;
                    cmdInsert1NF.CommandText = "INSERT INTO BALANS_1NF (" + insertFields1NF + ") VALUES (" + insertParams1NF + ")";
                    cmdInsert1NF.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SqlErrorForm.ShowSqlErrorMessageDlg(cmdInsert1NF.CommandText, cmdInsert1NF, ex.Message);
                    throw;
                }

                if (connectionSql != null)
                {
                    // Copy the balans object in SQL Server
                    try
                    {
                        cmdInsertSql.Connection = connectionSql;
                        cmdInsertSql.CommandText = "INSERT INTO balans (" + insertFieldsSql + ") VALUES (" + insertParamsSql + ")";
                        cmdInsertSql.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(cmdInsertSql.CommandText, cmdInsertSql, ex.Message);
                        throw;
                    }
                }
            }

            return newBalansId;
        }

        private static int CreateBalansObject(FbConnection connection1NF, FbTransaction transaction, SqlConnection connectionSql,
            ActObject actObj, BalansTransfer bt, DocumentNJF rishennya)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            GUKV.ImportToolUtils.FieldMappings.Create1NFBalansFieldMapping(mapping, false, true);

            int purposeGroup1NF = MatchDictionaryNJFto1NF(DICT_PURPOSE_GROUP, actObj.purposeGroupIdNJF);
            int purpose1NF = MatchDictionaryNJFto1NF_Hierarchical(DICT_PURPOSE, actObj.purposeIdNJF, DICT_PURPOSE_GROUP, actObj.purposeGroupIdNJF);
            int objectType1NF = MatchObjectTypeNJFto1NF(actObj.objectTypeIdNJF);
            int objectKind1NF = MatchObjectKindNJFto1NF(actObj.objectKindIdNJF);

            // Add all the properties we can take from 'privatization' table
            values.Add("OBJECT", actObj.objectId_1NF);
            values.Add("SQR_ZAG", bt.sqr);
            values.Add("SQR_NEJ", bt.sqr);

            values.Add("PRIZNAK1NF", 0);
            values.Add("UPD_SOURCE", 10); // 10 = Bagriy
            values.Add("DELETED", 0);
            values.Add("ISP", UserName.Length > 18 ? UserName.Substring(0, 18) : UserName);
            values.Add("EIS_MODIFIED_BY", UserName.Length > 64 ? UserName.Substring(0, 64) : UserName);
            values.Add("DT", DateTime.Now.Date);
            values.Add("EDT", DateTime.Now.Date);
            values.Add("PRIV_COUNT", 0);
            values.Add("LYEAR", DateTime.Now.Year);
            values.Add("PRAVO", 1); // 'Ownership' right
            values.Add("STAN", DateTime.Now.Date);
            values.Add("OBJECT_STAN", 1);
            values.Add("ORG_STAN", 1);
            values.Add("TEXSTAN", 2); // Zadovilniy
            values.Add("HISTORY", 2); // Usual building

            values.Add("EIS_BALANS_DOC_TYPE", rishennya.documentKind);
            values.Add("EIS_BALANS_DOC_DATE", rishennya.documentDate.Date);
            values.Add("EIS_BALANS_DOC_NUM", rishennya.documentNumber);

            Organization1NF orgNewOwner = bt.orgTo1NF;

            if (orgNewOwner != null && orgNewOwner.ownershipFormId > 0)
                values.Add("FORM_VLASN", orgNewOwner.ownershipFormId);

            if (objectKind1NF > 0)
                values.Add("KINDOBJ", objectKind1NF);

            if (objectType1NF > 0)
                values.Add("TYPEOBJ", objectType1NF);

            if (purposeGroup1NF > 0)
                values.Add("GRPURP", purposeGroup1NF);

            if (purpose1NF > 0)
                values.Add("PURPOSE", purpose1NF);

            if (actObj.objectName_NJF.Length > 0)
                values.Add("PURP_STR", actObj.objectName_NJF);

            if (actObj.objectBalansCost_NJF is decimal)
                values.Add("BALANS_VARTIST", actObj.objectBalansCost_NJF);

            if (actObj.objectFinalCost_NJF is decimal)
                values.Add("ZAL_VART", actObj.objectFinalCost_NJF);

            if (actObj.objectFloorsStr_NJF.Length > 0)
                values.Add("FLOATS", actObj.objectFloorsStr_NJF);

            // Get information about the object
            string query = @"SELECT FULL_ULNAME, ULKOD, ULNAME2, ULKOD2, NOMER1, NOMER2, NOMER3, ADRDOP FROM OBJECT_1NF WHERE OBJECT_KOD = @bid";

            using (FbCommand cmd = new FbCommand(query, connection1NF))
            {
                try
                {
                    cmd.Parameters.Add(new FbParameter("bid", actObj.objectId_1NF));
                    cmd.Transaction = transaction;

                    using (FbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(1))
                                values.Add("OBJ_KODUL", reader.GetValue(1));

                            if (!reader.IsDBNull(3))
                                values.Add("ULKOD2", reader.GetValue(3));

                            if (!reader.IsDBNull(0))
                                values.Add("OBJ_ULNAME", reader.GetValue(0));

                            if (!reader.IsDBNull(2))
                                values.Add("ULNAME2", reader.GetValue(2));

                            if (!reader.IsDBNull(4))
                                values.Add("OBJ_NOMER1", reader.GetValue(4));

                            if (!reader.IsDBNull(5))
                                values.Add("OBJ_NOMER2", reader.GetValue(5));

                            if (!reader.IsDBNull(6))
                                values.Add("OBJ_NOMER3", reader.GetValue(6));

                            if (!reader.IsDBNull(7))
                                values.Add("OBJ_ADRDOP", reader.GetValue(7));
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    SqlErrorForm.ShowSqlErrorMessageDlg(query, cmd, ex.Message);
                    throw;
                }
            }

            // Generate new balans Id
            int newBalansId = GenerateNewFirebirdId(connection1NF, "GEN_BALANS_1NF", transaction);

            if (newBalansId > 0)
            {
                values.Add("ID", newBalansId);
                values.Add("ORG", bt.organizationToId_1NF);

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

                using (FbCommand commandInsert = new FbCommand(query, connection1NF))
                {
                    try
                    {
                        foreach (KeyValuePair<string, object> pair in parameters1NF)
                        {
                            commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
                        }

                        commandInsert.Transaction = transaction;
                        commandInsert.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(query, commandInsert, ex.Message);
                        throw;
                    }
                }

                // Make the same change in SQL Server
                if (connectionSql != null)
                {
                    bool insertedOK = false;
                    query = "INSERT INTO balans (" + insertFieldListSQL + ") VALUES (" + insertParamListSQL + ")";

                    using (SqlCommand cmd = new SqlCommand(query, connectionSql))
                    {
                        try
                        {
                            foreach (KeyValuePair<string, object> pair in parametersSQL)
                            {
                                cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                            }

                            cmd.ExecuteNonQuery();
                            insertedOK = true;
                        }
                        catch (Exception ex)
                        {
                            SqlErrorForm.ShowSqlErrorMessageDlg(query, cmd, ex.Message);
                            throw;
                        }
                    }

                    if (insertedOK)
                    {
                        // Add the object to user report
                        using (SqlCommand cmd = new SqlCommand("dbo.fnAddBalansObjectToReport", connectionSql))
                        {
                            try
                            {
                                cmd.Parameters.Add(new SqlParameter("BALANS_ID", newBalansId));

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.ExecuteNonQuery();

                                GUKV.Common.EmailNotifier.NotifyBalansObjectAdded(connectionSql, null, newBalansId);
                            }
                            catch (Exception ex)
                            {
                                SqlErrorForm.ShowSqlErrorMessageDlg(query, cmd, ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }

            return newBalansId;
        }

        private static int CutBalansObject(FbConnection connection1NF, FbTransaction transaction, SqlConnection connectionSql,
            int balansId, decimal cutSquare, int newOwnerOrgId, DocumentNJF rishennya)
        {
            // Get the current square of object from 1NF
            decimal sqrTotal = -1m;
            decimal sqrNonHabit = -1m;

            string query = "SELECT SQR_ZAG, SQR_NEJ FROM BALANS_1NF WHERE ID = @balid";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                try
                {
                    command.Parameters.Add(new FbParameter("balid", balansId));
                    command.Transaction = transaction;

                    using (FbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sqrTotal = reader.IsDBNull(0) ? -1m : reader.GetDecimal(0);
                            sqrNonHabit = reader.IsDBNull(1) ? -1m : reader.GetDecimal(1);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    SqlErrorForm.ShowSqlErrorMessageDlg(query, command, ex.Message);
                    throw;
                }
            }

            int newBalansObj = CopyBalansObject(connection1NF, transaction, connectionSql, balansId, newOwnerOrgId, rishennya);

            if (cutSquare >= sqrTotal || Math.Abs(sqrTotal - cutSquare) <= 1m)
            {
                // The object can be transferred completely
                MarkBalansObjectAsDeleted(connection1NF, transaction, connectionSql, balansId, rishennya, newOwnerOrgId);
            }
            else
            {
                // Part of the object must remain at the previous owner
                ChangeBalansObjectSquare(connection1NF, transaction, connectionSql, balansId, sqrTotal - cutSquare);
            }

            ChangeBalansObjectSquare(connection1NF, transaction, connectionSql, newBalansObj, cutSquare);

            // Add the object to user report
            if (connectionSql != null)
            {
                using (SqlCommand cmd = new SqlCommand("dbo.fnAddBalansObjectToReport", connectionSql))
                {
                    try
                    {
                        cmd.Parameters.Add(new SqlParameter("BALANS_ID", newBalansObj));

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        EmailNotifier.NotifyBalansObjectAdded(connectionSql, null, newBalansObj);
                    }
                    catch (Exception ex)
                    {
                        SqlErrorForm.ShowSqlErrorMessageDlg(query, cmd, ex.Message);
                        throw;
                    }
                }
            }

            return newBalansObj;
        }

        #endregion (Document export)

        #region Implementation

        private static void LoadOrganizationsFrom1NF(FbConnection connection)
        {
            string query = @"SELECT KOD_OBJ, KOD_ZKPO, FULL_NAME_OBJ, SHORT_NAME_OBJ, KOD_GALUZ, KOD_VID_DIAL, KOD_STATUS, KOD_ORG_FORM,
                KOD_FORM_GOSP, KOD_FORM_VLASN, NAME_UL, NOMER_DOMA, NOMER_KORPUS, POST_INDEX, ADDITIONAL_ADRESS, KOD_RAYON2,
                FIO_BOSS, TEL_BOSS, FIO_BUH, TEL_BUH, TELEFAX FROM SORG_1NF WHERE KOD_STAN = 1 AND (DELETED IS NULL OR DELETED = 0)";

            using (FbCommand cmd = new FbCommand(query, connection))
            {
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            int orgId = reader.GetInt32(0);
                            Organization1NF org = null;

                            if (!organizations1NF.TryGetValue(orgId, out org))
                            {
                                org = new Organization1NF();
                                organizations1NF.Add(orgId, org);
                            }

                            org.organizationId = reader.GetInt32(0);
                            org.zkpo = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim().ToUpper();
                            org.fullName = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
                            org.shortName = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim().ToUpper();
                            org.industryId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            org.occupationId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                            org.statusId = reader.IsDBNull(6) ? -1 : reader.GetInt32(6);
                            org.orgTypeId = reader.IsDBNull(7) ? -1 : reader.GetInt32(7);
                            org.formGospId = reader.IsDBNull(8) ? -1 : reader.GetInt32(8);
                            org.ownershipFormId = reader.IsDBNull(9) ? -1 : reader.GetInt32(9);
                            org.addrStreet = reader.IsDBNull(10) ? "" : reader.GetString(10).Trim().ToUpper();
                            org.addrNomer = reader.IsDBNull(11) ? "" : reader.GetString(11).Trim().ToUpper();
                            org.addrKorpus = reader.IsDBNull(12) ? "" : reader.GetString(12).Trim().ToUpper();
                            org.addrZipCode = reader.IsDBNull(13) ? "" : reader.GetString(13).Trim().ToUpper();
                            org.addrDistrictId = reader.IsDBNull(15) ? -1 : reader.GetInt32(15);
                            org.directorFIO = reader.IsDBNull(16) ? "" : reader.GetString(16).Trim().ToUpper();
                            org.directorTel = reader.IsDBNull(17) ? "" : reader.GetString(17).Trim().ToUpper();
                            org.buhgalterFIO = reader.IsDBNull(18) ? "" : reader.GetString(18).Trim().ToUpper();
                            org.buhgalterTel = reader.IsDBNull(19) ? "" : reader.GetString(19).Trim().ToUpper();
                            org.fax = reader.IsDBNull(20) ? "" : reader.GetString(20).Trim().ToUpper();

                            string addrMisc = reader.IsDBNull(14) ? "" : reader.GetString(14).Trim().ToUpper();

                            if (addrMisc.Length > 0)
                                org.addrNomer = addrMisc;
                        }
                    }

                    reader.Close();
                }
            }
        }

        private static void LoadObjectsFrom1NF(FbConnection connection)
        {
            using (FbCommand cmd = new FbCommand("SELECT OBJECT_KOD, FULL_ULNAME, NOMER1, NOMER2, NOMER3, ADRDOP, TEXSTAN, " +
                "BUDYEAR, NEWDISTR, TYPOBJ, VIDOBJ, ULKOD, SZAG FROM OBJECT_1NF WHERE DELETED = 0 ORDER BY OBJECT_KOD", connection))
            {
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            Object1NF obj = new Object1NF();

                            obj.objectId = reader.GetInt32(0);
                            obj.streetName = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim().ToUpper();
                            obj.addrNomer1 = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
                            obj.addrNomer2 = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim().ToUpper();
                            obj.addrNomer3 = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim().ToUpper();
                            obj.addrMisc = reader.IsDBNull(5) ? "" : reader.GetString(5).Trim().ToUpper();
                            obj.techStateId = reader.IsDBNull(6) ? null : reader.GetValue(6);
                            obj.buildYear = reader.IsDBNull(7) ? null : reader.GetValue(7);
                            obj.districtId = reader.IsDBNull(8) ? null : reader.GetValue(8);
                            obj.objTypeId = reader.IsDBNull(9) ? null : reader.GetValue(9);
                            obj.objKindId = reader.IsDBNull(10) ? null : reader.GetValue(10);
                            obj.streetId = reader.IsDBNull(11) ? -1 : reader.GetInt32(11);
                            obj.totalSqr = reader.IsDBNull(12) ? null : reader.GetValue(12);

                            objects1NF.Add(obj.objectId, obj);
                        }
                    }

                    reader.Close();
                }
            }
        }

        private static void LoadBalansObjectsFrom1NF(FbConnection connection)
        {
            balans1NFByID.Clear();
            balans1NFByAddress.Clear();

            using (FbCommand cmd = new FbCommand("SELECT ID, OBJECT, ORG, SQR_ZAG, GRPURP, PURPOSE, PURP_STR FROM BALANS_1NF WHERE (DELETED = 0) OR (DELETED IS NULL)", connection))
            {
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            BalansObject1NF obj = new BalansObject1NF();

                            obj.balansId = reader.GetInt32(0);
                            obj.objectId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                            obj.organizationId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                            obj.sqr = reader.IsDBNull(3) ? 0m : reader.GetDecimal(3);
                            obj.purposeGroupId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            obj.purposeId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                            obj.purpose = reader.IsDBNull(6) ? "" : reader.GetString(6).Trim().ToUpper();

                            // Balans objects are stored by object ID
                            Dictionary<int, BalansObject1NF> storage = null;

                            if (!balans1NFByAddress.TryGetValue(obj.objectId, out storage))
                            {
                                storage = new Dictionary<int, BalansObject1NF>();

                                balans1NFByAddress.Add(obj.objectId, storage);
                            }

                            storage.Add(obj.balansId, obj);
                            balans1NFByID.Add(obj.balansId, obj);
                        }
                    }

                    reader.Close();
                }
            }
        }

        private static void LoadDocumentsFromNJF(FbConnection connection)
        {
            using (FbCommand cmd = new FbCommand("SELECT DOK_ID, DOKKIND, DOKDATA, DOKNUM, DOKTEMA FROM ROZP_DOK WHERE NOT (DOK_ID IS NULL)", connection))
            {
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(2) && !reader.IsDBNull(3))
                        {
                            DocumentNJF doc = new DocumentNJF();

                            doc.documentId = reader.GetInt32(0);
                            doc.documentKind = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                            doc.documentDate = reader.GetDateTime(2).Date;
                            doc.documentNumber = reader.GetString(3);
                            doc.documentTitle = reader.IsDBNull(4) ? "" : reader.GetString(4);

                            doc.documentNumber = doc.documentNumber.Trim().ToUpper();
                            doc.documentTitle = doc.documentTitle.Trim().ToUpper();

                            documentsNJF.Add(doc.documentId, doc);
                        }
                    }

                    reader.Close();
                }
            }

            using (FbCommand cmd = new FbCommand("SELECT DOK_ID, DOKKIND, DOKDATA, DOKNUM, DOKTEMA FROM ACTS WHERE NOT (DOK_ID IS NULL)", connection))
            {
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(2) && !reader.IsDBNull(3))
                        {
                            DocumentNJF doc = new DocumentNJF();

                            doc.documentId = reader.GetInt32(0);
                            doc.documentKind = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                            doc.documentDate = reader.GetDateTime(2).Date;
                            doc.documentNumber = reader.GetString(3);
                            doc.documentTitle = reader.IsDBNull(4) ? "" : reader.GetString(4);

                            doc.documentNumber = doc.documentNumber.Trim().ToUpper();
                            doc.documentTitle = doc.documentTitle.Trim().ToUpper();

                            actsNJF.Add(doc.documentId, doc);
                        }
                    }

                    reader.Close();
                }
            }

            using (FbCommand cmd = new FbCommand("SELECT DOK_KOD1, DOK_KOD2 FROM DOK_DEPEND WHERE NOT (DOK_KOD1 IS NULL) AND NOT (DOK_KOD2 IS NULL)", connection))
            {
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int childId = reader.GetInt32(0);
                        int parentId = reader.GetInt32(1);

                        DocumentNJF parentDoc = null;

                        if (documentsNJF.TryGetValue(parentId, out parentDoc))
                        {
                            if (parentDoc.dependentDocuments == null)
                            {
                                parentDoc.dependentDocuments = new List<int>();
                            }

                            if (!parentDoc.dependentDocuments.Contains(childId))
                            {
                                parentDoc.dependentDocuments.Add(childId);
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        private static Dictionary<int, Organization1NF> LoadOrganizationsFromNJF(FbConnection connection)
        {
            Dictionary<int, Organization1NF> orgList = new Dictionary<int,Organization1NF>();

            using (FbCommand cmd = new FbCommand("SELECT KOD, STAN, ZKPO, NAME, NAMEP FROM SZKPO ORDER BY KOD, STAN", connection))
            {
                using (FbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int organizationId = reader.GetInt32(0);
                        Organization1NF org = null;

                        if (!orgList.TryGetValue(organizationId, out org))
                        {
                            org = new Organization1NF();
                            orgList.Add(organizationId, org);
                        }

                        org.organizationId = reader.GetInt32(0);
                        org.zkpo = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim().ToUpper();
                        org.shortName = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim().ToUpper();
                        org.fullName = reader.IsDBNull(4) ? "" : reader.GetString(4).Trim().ToUpper();
                    }

                    reader.Close();
                }
            }

            return orgList;
        }

        public static int FindDistrictMatchInNJF(string districtName)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(DICT_DISTRICTS, out data))
            {
                // Find a district in NJF by name
                districtName = districtName.Trim().ToUpper();

                foreach (KeyValuePair<int, DictionaryValue> pair in data.ValuesNJF)
                {
                    if (pair.Value.value == districtName)
                    {
                        return pair.Key;
                    }
                }
            }

            return -1;
        }

        public static int FindStreetMatchInNJF(string streetName, FbConnection connectionNJF, FbTransaction transaction)
        {
            DictionaryData data = null;

            if (dictionaries.TryGetValue(DICT_STREETS, out data))
            {
                streetName = streetName.Trim().ToUpper();

                // Find a street in NJF by name
                int streetIdNJF = -1;

                if (data.KeysNJF.TryGetValue(streetName, out streetIdNJF))
                {
                    return streetIdNJF;
                }

                // Get the ID of this street in 1NF
                int streetId1NF = -1;

                if (data.Keys1NF.TryGetValue(streetName, out streetId1NF))
                {
                    // Try to find a manual mapping
                    foreach (KeyValuePair<int, int> pair in manualStreetCodeMappingNJFto1NF)
                    {
                        if (pair.Value == streetId1NF)
                        {
                            return pair.Key;
                        }
                    }
                }

                // We need to create this street in NJF database
                // There is no Generator for this table, so we just have to find the maximum existing Id
                int newStreetIdNJF = data.MaxKeyNJF + 1;

                using (FbCommand cmd = new FbCommand("INSERT INTO SUL (KOD, NAME) VALUES (@sid, @nm)", connectionNJF))
                {
                    if (streetName.Length > 160)
                        streetName = streetName.Substring(0, 160);

                    cmd.Parameters.Add(new FbParameter("sid", newStreetIdNJF));
                    cmd.Parameters.Add(new FbParameter("nm", streetName));

                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                }

                // Update our cached dictionary
                data.MaxKeyNJF = newStreetIdNJF;
                data.KeysNJF[streetName] = newStreetIdNJF;
                data.ValuesNJF[newStreetIdNJF] = new DictionaryValue(newStreetIdNJF, streetName);

                return newStreetIdNJF;
            }

            return -1;
        }

        public static int FindObjectMatchInNJF(FbConnection connectionNJF, FbTransaction transaction,
            Object1NF obj, ObjectFinder objectFinder)
        {

            bool addressIsSimple = false;
            bool similarAddressExists = false;

            int existingId = objectFinder.FindObject(obj.streetName, "", obj.addrNomer1, obj.addrNomer2, obj.addrNomer3,
                obj.addrMisc, null, null, out addressIsSimple, out similarAddressExists)[0];

            if (existingId > 0)
            {
                return existingId;
            }

            // Get the district name
            DictionaryValue district = new DictionaryValue();

            if (obj.districtId is int)
            {
                DictionaryData data = null;

                if (dictionaries.TryGetValue(DB.DICT_DISTRICTS, out data))
                {
                    if (!data.Values1NF.TryGetValue((int)obj.districtId, out district))
                    {
                        district = new DictionaryValue();
                    }
                }
            }

            int streetId = -1;
            int districtId = -1;

            existingId = CreateNewNJFObject(connectionNJF, transaction, obj.streetName, district.value,
                obj.addrNomer1, obj.addrNomer2, obj.addrNomer3, obj.addrMisc, out streetId, out districtId);

            // Register the object in the Object Finder
            objectFinder.AddObjectToCache(existingId, obj.streetName, null, obj.addrNomer1, obj.addrNomer2, obj.addrNomer3,
                obj.addrMisc, districtId > 0 ? (object)districtId : null, streetId, null, 0, obj.totalSqr, null, null, 1);

            return existingId;
        }

        public static int FindOrganizationMatchInNJF(FbConnection connectionNJF, FbTransaction transaction,
            Organization1NF org, OrganizationFinder orgFinder)
        {
            bool categorized = false;

            int existingOrgId = orgFinder.FindOrganization(org.zkpo, org.fullName, org.shortName, false, out categorized);

            if (existingOrgId > 0)
            {
                return existingOrgId;
            }

            // Get the street id in NJF by street name
            int streetId = FindStreetMatchInNJF(org.addrStreet, connectionNJF, transaction);

            // Get the district id in NJF by district name
            int? districtId = null;
            DictionaryValue district = new DictionaryValue();

            if (org.addrDistrictId > 0)
            {
                DictionaryData data = null;

                if (dictionaries.TryGetValue(DB.DICT_DISTRICTS, out data))
                {
                    if (!data.Values1NF.TryGetValue(org.addrDistrictId, out district))
                    {
                        district = new DictionaryValue();
                    }
                }
            }

            if (district.value.Length > 0)
            {
                districtId = FindDistrictMatchInNJF(district.value);
            }

            // Determine organization status in NJF
            int statusId = 2;

            if (org.statusId == 2)
                statusId = 1;

            int industryId = MatchDictionary1NFtoNJF(DB.DICT_ORG_INDUSTRY, org.industryId);
            int ownershipId = MatchOrgOwnership1NFtoNJF(org.ownershipFormId);
            int formGospId = MatchOrgFormGosp1NFtoNJF(org.formGospId);
            int orgTypeId = MatchDictionary1NFtoNJF(DB.DICT_ORG_TYPE, org.orgTypeId);

            existingOrgId = CreateNewNJFOrganization(connectionNJF, transaction,
                org.zkpo,
                org.fullName,
                org.shortName,
                districtId,
                streetId,
                org.addrNomer,
                "",
                "",
                null,
                org.addrZipCode,
                industryId,
                ownershipId,
                formGospId,
                orgTypeId,
                statusId,
                org.directorFIO,
                org.directorTel,
                org.buhgalterFIO,
                org.buhgalterTel,
                org.fax);

            // Register organization in the Organization Finder
            orgFinder.AddOrganizationToCache(existingOrgId, org.zkpo, org.fullName, org.shortName,
                industryId > 0 ? (object)industryId : null,
                null,
                ownershipId > 0 ? (object)ownershipId : null);

            return existingOrgId;
        }

        private static int Get1NFNewOrganizationId(FbConnection connection1NF, FbTransaction transaction)
        {
            int seed = -1;

            string query = "select max(kod_obj) from sorg_1nf where kod_obj < 130000";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                command.Transaction = transaction;

                using (FbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        object dataSeed = reader.IsDBNull(0) ? null : reader.GetValue(0);

                        if (dataSeed is int)
                        {
                            seed = (int)dataSeed + 1;
                        }
                    }

                    reader.Close();
                }
            }

            if (seed < 0)
            {
                seed = GenerateNewFirebirdId(connection1NF, "SORG_1NF_GEN", transaction);
            }

            return seed;
        }

        private static void RemoveOldDistricts()
        {
            DictionaryData dictDistricts = null;

            if (dictionaries.TryGetValue(DICT_DISTRICTS, out dictDistricts))
            {
                List<int> keysToRemove = dictDistricts.Values1NF.Keys.Where(i => i >= 900).ToList();

                foreach (int key in keysToRemove)
                {
                    dictDistricts.Values1NF.Remove(key);
                }
            }
        }

        #endregion (Implementation)
    }
}
