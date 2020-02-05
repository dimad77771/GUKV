using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using GUKV.Common;
using GUKV.DataMigration.Properties;

namespace GUKV.ImportToolUtils
{
    /// <summary>
    /// Describes a particular object in 1NF database
    /// </summary>
    public class ObjectInfo
    {
        /// <summary>
        /// Building Id
        /// </summary>
        public int objectId = 0;

        /// <summary>
        /// Street name
        /// </summary>
        public object street = null;

        /// <summary>
        /// Street Id
        /// </summary>
        public object streetId = null;

        /// <summary>
        /// Name of the second street
        /// </summary>
        public object street2 = null;

        /// <summary>
        /// Id of the second street
        /// </summary>
        public object streetId2 = null;

        /// <summary>
        /// Building number (1)
        /// </summary>
        public object nomer1 = null;

        /// <summary>
        /// Building number (2)
        /// </summary>
        public object nomer2 = null;

        /// <summary>
        /// Building number (3)
        /// </summary>
        public object nomer3 = null;

        /// <summary>
        /// Additional information
        /// </summary>
        public object addrMiscInfo = null;

        /// <summary>
        /// District Id
        /// </summary>
        public object districtId = null;

        /// <summary>
        /// Total square
        /// </summary>
        public object totalSquare = null;

        /// <summary>
        /// Non-habitable square
        /// </summary>
        public object nonHabitSquare = null;

        /// <summary>
        /// Habitable square
        /// </summary>
        public object habitSquare = null;

        /// <summary>
        /// Indicates that object is deleted in the 1NF database
        /// </summary>
        public bool deleted = false;

        /// <summary>
        /// Indicates that address is not disabled in 1NF
        /// </summary>
        public bool addressIsActive = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ObjectInfo()
        {
        }

        /// <summary>
        /// Builds a string presentation of this object
        /// </summary>
        /// <returns>Full address formatted as a string</returns>
        public string FormatToStr()
        {
            string address = "";

            if (street is string)
            {
                address += street.ToString() + " ";
            }

            if (street2 is string)
            {
                address += " / " + street2.ToString() + " ";
            }

            if (nomer1 is string)
            {
                address += nomer1.ToString();
            }

            if (nomer2 is string)
            {
                address += " " + nomer2.ToString();
            }

            if (nomer3 is string)
            {
                address += " " + nomer3.ToString();
            }

            if (addrMiscInfo is string)
            {
                address += ", " + addrMiscInfo.ToString();
            }

            /*
            if (districtId is int)
            {
                address += " District = " + districtId.ToString();
            }
            */

            return address;
        }

        /// <summary>
        /// Makes sure that all properties have comparable values
        /// </summary>
        public void ValidateProperties()
        {
            street = ValidateStringProp(street);
            street2 = ValidateStringProp(street2);
            nomer1 = ValidateStringProp(nomer1);
            nomer2 = ValidateStringProp(nomer2);
            nomer3 = ValidateStringProp(nomer3);
            addrMiscInfo = ValidateStringProp(addrMiscInfo);

            if (totalSquare is decimal && (decimal)totalSquare <= 0)
            {
                totalSquare = null;
            }

            if (nonHabitSquare is decimal && (decimal)nonHabitSquare <= 0)
            {
                nonHabitSquare = null;
            }

            if (habitSquare is decimal && (decimal)habitSquare <= 0)
            {
                habitSquare = null;
            }

            if (streetId is int && (int)streetId <= 0)
            {
                streetId = null;
            }

            if (streetId2 is int && (int)streetId2 <= 0)
            {
                streetId2 = null;
            }

            if (districtId is int && (int)districtId <= 0)
            {
                districtId = null;
            }
        }

        private object ValidateStringProp(object property)
        {
            if (property is string)
            {
                string str = ((string)property).Trim();

                if (str.Length > 0)
                {
                    return str.ToUpper();
                }
            }

            return null;
        }
    }

    /// <summary>
    /// This helper class is used to match objects from various databases
    /// (Balans, Rosporadjennia, etc) to the 1 NF objects.
    /// </summary>
    public class ObjectFinder
    {
        private static readonly object _initLocked = new object();
        private static ObjectFinder _instance;
        private static DateTime lastReload = DateTime.MinValue;
        private static int minutesInterval = 5;

        public static ObjectFinder Instance
        {
            get
            {
                if ((_instance == null) || (lastReload.AddMinutes(minutesInterval) < DateTime.Now))
                    lock (_initLocked)
                    {
                        if ((_instance == null) || (lastReload.AddMinutes(minutesInterval) < DateTime.Now))
                        {
                            _instance = new ObjectFinder();
                            using (SqlConnection conn = CommonUtils.ConnectToDatabase())
                            {
                                _instance.BuildObjectCacheFromSqlServer(conn);
                            }
                        }

                    }
                return _instance;
            }
        }


        #region Nested classes

        /// <summary>
        /// Street types
        /// </summary>
        public enum StreetType
        {
            Street,
            Avenue, // Prospect
            Lane, // Provulok
            Square, // Ploscha
            Park,
            Boulevard
        }

        /// <summary>
        /// Describes a street name in a manner suitable for fuzzy comparisons
        /// </summary>
        public class StreetName
        {
            /// <summary>
            /// Name of the city; if empty, we assume that it is Kiev
            /// </summary>
            public List<string> city = null;

            /// <summary>
            /// Name of the village; if empty, we assume that it is Kiev
            /// </summary>
            public List<string> village = null;

            /// <summary>
            /// Street name in the text form (unparsed)
            /// </summary>
            public string street = "";

            /// <summary>
            /// Street name split into separate words (for fuzzy comparisons)
            /// </summary>
            public List<string> streetParts = null;

            /// <summary>
            /// Street type
            /// </summary>
            public StreetType streetType = StreetType.Street;

            /// <summary>
            /// Default constructor
            /// </summary>
            public StreetName()
            {
            }

            /// <summary>
            /// Creates a StreetName object for the specified street name
            /// </summary>
            /// <param name="streetName">A name of street to parse</param>
            public StreetName(string streetName)
            {
                Parse(streetName);
            }

            /// <summary>
            /// Splits the street name into words, and analyzes the parts to make them suitable for fuzzy comparison
            /// </summary>
            /// <param name="streetName">Street anme to process</param>
            public void Parse(string streetName)
            {
                this.street = streetName;

                string[] addrParts = street.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // Process each part separately; it may be a city name, a village name, or a street address
                foreach (string part in addrParts)
                {
                    // Remove all punctiation characters, and split the address part by spaces
                    string[] words = part.Replace(".", "").Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    // Try to determine the address type
                    List<string> actualAddressWords = new List<string>();

                    bool isCity = false;
                    bool isVillage = false;
                    StreetType streetTp = StreetType.Street;

                    foreach (string word in words)
                    {
                        string w = word.ToUpper();

                        if (GUKV.DataMigration.Properties.Settings.Default.AddrTypeVillage.Contains(w))
                        {
                            isVillage = true;
                        }
                        else if (GUKV.DataMigration.Properties.Settings.Default.AddrTypeCity.Contains(w))
                        {
                            isCity = true;
                        }
                        else if (GUKV.DataMigration.Properties.Settings.Default.AddrTypeAvenue.Contains(w))
                        {
                            streetTp = StreetType.Avenue;
                        }
                        else if (GUKV.DataMigration.Properties.Settings.Default.AddrTypeBoulevard.Contains(w))
                        {
                            streetTp = StreetType.Boulevard;
                        }
                        else if (GUKV.DataMigration.Properties.Settings.Default.AddrTypeLane.Contains(w))
                        {
                            streetTp = StreetType.Lane;
                        }
                        else if (GUKV.DataMigration.Properties.Settings.Default.AddrTypeSquare.Contains(w))
                        {
                            streetTp = StreetType.Square;
                        }
                        else if (GUKV.DataMigration.Properties.Settings.Default.AddrTypePark.Contains(w))
                        {
                            streetTp = StreetType.Park;
                        }
                        else
                        {
                            actualAddressWords.Add(w);
                        }
                    }

                    // Check what we've found
                    actualAddressWords.Sort();

                    if (isCity)
                    {
                        city = actualAddressWords;
                    }
                    else if (isVillage)
                    {
                        village = actualAddressWords;
                    }
                    else
                    {
                        streetParts = actualAddressWords;
                        streetType = streetTp;
                    }
                }

                // Make sure that this object contains no NULLs
                if (city == null)
                    city = new List<string>();

                if (village == null)
                    village = new List<string>();

                if (streetParts == null)
                    streetParts = new List<string>();
            }

            /// <summary>
            /// Overrides the ToString() methodto return the unparsed (pure) string name, as specified in 1NF
            /// </summary>
            /// <returns>Object state converted to string</returns>
            public override string ToString()
            {
                return street;
            }

            /// <summary>
            /// Overrides the Equals() method to compare StreetName objects by the 1NF street name
            /// </summary>
            /// <param name="obj">Object to compare with. Must be an instance of StreetName class.</param>
            /// <returns>TRUE if objects are equal</returns>
            public override bool Equals(object obj)
            {
                return ((StreetName)obj).street == this.street;
            }

            /// <summary>
            /// Overrides GetHashCode() method to store StreetName objects in dictionaries by the 1NF street name
            /// </summary>
            /// <returns>Hash code</returns>
            public override int GetHashCode()
            {
                return street.GetHashCode();
            }
        }

        /// <summary>
        /// Represents an address in a unified form
        /// </summary>
        public class UnifiedAddress
        {
            /// <summary>
            /// Name of the street
            /// </summary>
            public StreetName streetName = null;

            /// <summary>
            /// The primary number of the building
            /// </summary>
            public string number = "";

            /// <summary>
            /// If the building number looks like "X-Y", the Y part is stored in this variable
            /// </summary>
            public string rangeNumber = "";

            /// <summary>
            /// If the building number looks like "X/Y", the Y part is stored in this variable
            /// </summary>
            public string slashNumber = "";

            /// <summary>
            /// If the building number looks like "X K.Y", the Y part is stored in this variable
            /// </summary>
            public string korpus = "";

            /// <summary>
            /// If the building number looks like "X LIT.Y", the Y part is stored in this variable
            /// </summary>
            public string litera = "";

            /// <summary>
            /// If the building number looks like "X DOK-Y", the Y part is stored in this variable
            /// </summary>
            public string dock = "";

            /// <summary>
            /// If the building number looks like "X Ch.Y", the Y part is stored in this variable
            /// </summary>
            public string part = "";

            /// <summary>
            /// Misc. part of the address
            /// </summary>
            public string misc = "";

            /// <summary>
            /// Indicates that 'LIT' prefix was encountered in the address
            /// </summary>
            public bool literaSpecified = false;

            /// <summary>
            /// Default constructor
            /// </summary>
            public UnifiedAddress()
            {
            }

            /// <summary>
            /// Produces a unique key for the full address identified by this object
            /// </summary>
            /// <returns>The full address converted to string in a standard fashion</returns>
            public string FormatFullAddress()
            {
                string fullAddress = streetName.street + "_N_" + number;

                if (rangeNumber.Length > 0)
                {
                    fullAddress += "_R_" + rangeNumber;
                }

                if (slashNumber.Length > 0)
                {
                    fullAddress += "_S_" + slashNumber;
                }

                if (korpus.Length > 0)
                {
                    fullAddress += "_K_" + korpus;
                }

                if (litera.Length > 0)
                {
                    fullAddress += "_LIT_" + litera;
                }

                if (part.Length > 0)
                {
                    fullAddress += "_P_" + part;
                }

                if (dock.Length > 0)
                {
                    fullAddress += "_D_" + dock;
                }

                if (misc.Length > 0)
                {
                    fullAddress += "_M_" + misc;
                }

                return fullAddress;
            }

            /// <summary>
            /// Produces a unique key for the basic address (street and building number) identified by this object
            /// </summary>
            /// <returns>The full address converted to string in a standard fashion</returns>
            public string FormatBasicAddress()
            {
                string fullAddress = streetName.street + "_N_" + number;

                if (rangeNumber.Length > 0)
                {
                    fullAddress += "_R_" + rangeNumber;
                }

                return fullAddress;
            }

            /// <summary>
            /// Indicates that this address has a form "Street NUM", with no suffixes and prefixes
            /// </summary>
            /// <returns></returns>
            public bool IsSimple()
            {
                return
                    number.Length > 0 &&
                    !literaSpecified &&
                    rangeNumber.Length == 0 &&
                    slashNumber.Length == 0 &&
                    korpus.Length == 0 &&
                    dock.Length == 0 &&
                    part.Length == 0 &&
                    misc.Length == 0;
            }

            /// <summary>
            /// Creates a full copy of the unified address
            /// </summary>
            /// <returns>The created copy of this object</returns>
            public UnifiedAddress MakeCopy()
            {
                UnifiedAddress addr = new UnifiedAddress();

                addr.streetName = this.streetName;
                addr.number = this.number;
                addr.rangeNumber = this.rangeNumber;
                addr.slashNumber = this.slashNumber;

                addr.litera = this.litera;
                addr.literaSpecified = this.literaSpecified;

                addr.korpus = this.korpus;
                addr.dock = this.dock;
                addr.part = this.part;
                addr.misc = this.misc;

                return addr;
            }
        }

        #endregion (Nested classes)

        #region Member variables

        /// <summary>
        /// This parameter regulates precision of fuzzy comparisons
        /// </summary>
        private const int maxAllowedFuzzyErrors = 2;

        /// <summary>
        /// If this flag is set, addresses that contain 'LITERA A' can be matched to similar addresses
        /// without any LITERA, and square of such addresses is not taken into consideration
        /// </summary>
        private bool allowLiterAComparisonWithoutSquareMatch = false;

        /// <summary>
        /// Mapping between object address and object Id in 1NF
        /// </summary>
        private Dictionary<string, int> objAddrMapToId1NF = new Dictionary<string, int>();

        /// <summary>
        /// Mapping between object address and object Id in 1NF. This map is generated
        /// only by street and the first building number.
        /// </summary>
        private Dictionary<string, List<int>> objAddrMapToId1NF_By1stNumber = new Dictionary<string, List<int>>();

        /// <summary>
        /// Mapping between object address and object Id in 1NF, including the district code
        /// </summary>
        private Dictionary<string, List<int>> objAddrMapToId1NF_WithDistrict = new Dictionary<string, List<int>>();

        /// <summary>
        /// The street names from the 1NF 'SUL' dictionary table
        /// </summary>
        private Dictionary<int, StreetName> streetNamesById = new Dictionary<int, StreetName>();

        /// <summary>
        /// Full list of 1 NF street names
        /// </summary>
        private Dictionary<string, int> streetNames = new Dictionary<string, int>();

        /// <summary>
        /// Full cache of all 1NF objects
        /// </summary>
        private Dictionary<int, ObjectInfo> objectProperties = new Dictionary<int, ObjectInfo>();

        /// <summary>
        /// Prefix 'LIT.' for a building number
        /// </summary>
        private static string litPrefixLITd = Resources.BuildingLitPrefix;

        /// <summary>
        /// Prefix 'LITER.' for a building number
        /// </summary>
        private static string litPrefixLITERd = Resources.BuildingLitPrefix2;

        /// <summary>
        /// Prefix 'LITERA' for a building number
        /// </summary>
        private static string litPrefixLITERA = Resources.BuildingLitPrefix3;

        /// <summary>
        /// Prefix 'LITER' for a building number
        /// </summary>
        private static string litPrefixLITER = Resources.BuildingLitPrefix4;

        /// <summary>
        /// Prefix 'LITER .' for a building number
        /// </summary>
        private static string litPrefixLITER_d = Resources.BuildingLitPrefix5;

        /// <summary>
        /// Prefix 'LITER  .' for a building number
        /// </summary>
        private static string litPrefixLITER__d = Resources.BuildingLitPrefix6;

        /// <summary>
        /// Prefix 'LITERA -' for a building number
        /// </summary>
        private static string litPrefixLITER_sh = Resources.BuildingLitPrefix7;

        /// <summary>
        /// Prefix 'LIT' for a building number
        /// </summary>
        private static string litPrefixLIT = Resources.BuildingLitPrefix8;

        /// <summary>
        /// 'PROV.' suffix for a street name
        /// </summary>
        private static string streetSuffixProv = Resources.StreetSuffixProv;

        /// <summary>
        /// 'PROSP.' suffix for a street name
        /// </summary>
        private static string streetSuffixProsp = Resources.StreetSuffixProsp;

        /// <summary>
        /// 'BULV.' suffix for a street name
        /// </summary>
        private static string streetSuffixBulv = Resources.StreetSuffixBulv;

        /// <summary>
        /// 'K.' prefix for a building number
        /// </summary>
        private static string korpusPrefixCorrect = Resources.BuildingKorpusPrefixCorrect;

        /// <summary>
        /// 'KORP.' prefix for a building number
        /// </summary>
        private static string korpusPrefix1 = Resources.BuildingKorpusPrefix1;

        /// <summary>
        /// 'korp.' prefix for a building number
        /// </summary>
        private static string korpusPrefix2 = Resources.BuildingKorpusPrefix2;

        /// <summary>
        /// 'K-' prefix for a building number
        /// </summary>
        private static string korpusPrefix3 = Resources.BuildingKorpusPrefix3;

        /// <summary>
        /// 'DOK-' prefix for a building number
        /// </summary>
        private static string dockPrefix1 = Resources.BuildingDockPrefix1;

        /// <summary>
        /// 'Ch.' prefix for a building number
        /// </summary>
        private static string partPrefix1 = Resources.BuildingPartPrefix1;

        /// <summary>
        /// Quotes that serve as separators
        /// </summary>
        private static char[] btiSuffixSeparators = new char[] { '\'', '\"' };

        /// <summary>
        /// A single space as a separator
        /// </summary>
        private static char[] spaceSeparator = new char[] { ' ' };

        #endregion (Member variables)

        #region Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        private ObjectFinder()
        {
        }

        #endregion (Construction)

        #region Interface

        /// <summary>
        /// Prepares the cache of objects for future lookup, using the SQL Server database as a source
        /// </summary>
        /// <param name="connectionSqlClient">Connection to the SQL Server</param>
        public void BuildObjectCacheFromSqlServer(SqlConnection connectionSqlClient)
        {
            objAddrMapToId1NF.Clear();
            objAddrMapToId1NF_By1stNumber.Clear();
            objAddrMapToId1NF_WithDistrict.Clear();

            // Select all objects from the OBJECTS table
            string querySelect = "SELECT id, addr_street_name, addr_street_name2, addr_nomer1, addr_nomer2, addr_nomer3, addr_misc, " +
                "addr_distr_new_id, addr_street_id, addr_street_id2, is_deleted, sqr_total, sqr_habit, sqr_non_habit FROM buildings " +
                "WHERE (is_deleted IS NULL) OR (is_deleted = 0) ORDER BY id";

            using (SqlCommand commandSelect = new SqlCommand(querySelect, connectionSqlClient))
            {
                using (SqlDataReader reader = commandSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Get the object data
                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataStreet1 = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object dataStreet2 = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object dataNomer1 = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object dataNomer2 = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        object dataNomer3 = reader.IsDBNull(5) ? null : reader.GetValue(5);
                        object dataMisc = reader.IsDBNull(6) ? null : reader.GetValue(6);
                        object dataDistrict = reader.IsDBNull(7) ? null : reader.GetValue(7);
                        object dataStreetId1 = reader.IsDBNull(8) ? null : reader.GetValue(8);
                        object dataStreetId2 = reader.IsDBNull(9) ? null : reader.GetValue(9);
                        object dataDeleted = reader.IsDBNull(10) ? null : reader.GetValue(10);
                        object dataSqrTotal = reader.IsDBNull(11) ? null : reader.GetValue(11);
                        object dataSqrHabit = reader.IsDBNull(12) ? null : reader.GetValue(12);
                        object dataSqrNonHabit = reader.IsDBNull(13) ? null : reader.GetValue(13);

                        // Add this address to the cache
                        AddObjectToCache(dataId, dataStreet1, dataStreet2, dataNomer1, dataNomer2, dataNomer3, dataMisc,
                            dataDistrict, dataStreetId1, dataStreetId2, dataDeleted, dataSqrTotal, dataSqrHabit, dataSqrNonHabit, 1);
                    }

                    reader.Close();
                }
            }

            // Prepare the cache of street names
            streetNamesById.Clear();
            streetNames.Clear();

            string query = "SELECT id, name FROM dict_streets";

            using (SqlCommand commandSelect = new SqlCommand(query, connectionSqlClient))
            {
                using (SqlDataReader reader = commandSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataStreet = reader.IsDBNull(1) ? null : reader.GetValue(1);

                        if (dataId is int && dataStreet is string)
                        {
                            AddStreetToCache((int)dataId, (string)dataStreet);
                        }
                    }

                    reader.Close();
                }
            }
            lastReload = DateTime.Now;
        }

        /// <summary>
        /// Prepares the cache of objects for future lookup, using the 1NF database as a source
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        public void BuildObjectCacheFrom1NF(FbConnection connection1NF)
        {
            objAddrMapToId1NF.Clear();
            objAddrMapToId1NF_By1stNumber.Clear();
            objAddrMapToId1NF_WithDistrict.Clear();

            // Select all objects from the OBJECTS table
            string querySelect = "SELECT OBJECT_KOD, ULNAME, ULNAME2, NOMER1, NOMER2, NOMER3, ADRDOP, NEWDISTR, " +
                "ULKOD, ULKOD2, DELETED, SZAG, V_JIL, V_NEJ, ADR_REES_KODSTAN FROM OBJECT_1NF WHERE (DELETED IS NULL) OR (DELETED = 0) ORDER BY OBJECT_KOD";

            using (FbCommand commandSelect = new FbCommand(querySelect, connection1NF))
            {
                using (FbDataReader reader = commandSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Get the object data
                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataStreet1 = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object dataStreet2 = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object dataNomer1 = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object dataNomer2 = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        object dataNomer3 = reader.IsDBNull(5) ? null : reader.GetValue(5);
                        object dataMisc = reader.IsDBNull(6) ? null : reader.GetValue(6);
                        object dataDistrict = reader.IsDBNull(7) ? null : reader.GetValue(7);
                        object dataStreetId1 = reader.IsDBNull(8) ? null : reader.GetValue(8);
                        object dataStreetId2 = reader.IsDBNull(9) ? null : reader.GetValue(9);
                        object dataDeleted = reader.IsDBNull(10) ? null : reader.GetValue(10);
                        object dataSqrTotal = reader.IsDBNull(11) ? null : reader.GetValue(11);
                        object dataSqrHabit = reader.IsDBNull(12) ? null : reader.GetValue(12);
                        object dataSqrNonHabit = reader.IsDBNull(13) ? null : reader.GetValue(13);
                        object dataAddressIsActive = reader.IsDBNull(14) ? null : reader.GetValue(14);

                        // Add this address to the cache
                        AddObjectToCache(dataId, dataStreet1, dataStreet2, dataNomer1, dataNomer2, dataNomer3, dataMisc, dataDistrict,
                            dataStreetId1, dataStreetId2, dataDeleted, dataSqrTotal, dataSqrHabit, dataSqrNonHabit, dataAddressIsActive);
                    }

                    reader.Close();
                }
            }

            // Prepare the cache of street names
            streetNamesById.Clear();
            streetNames.Clear();

            string query = "SELECT KOD, REPRESENT FROM SUL";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                using (FbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataKod = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataStreet = reader.IsDBNull(1) ? null : reader.GetValue(1);

                        if (dataKod is int && dataStreet is string)
                        {
                            AddStreetToCache((int)dataKod, (string)dataStreet);
                        }
                    }

                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Prepares the cache of objects for future lookup, using the Rozporadjennia database as a source
        /// </summary>
        /// <param name="connectionNJF">Connection to the Rozporadjennia database</param>
        public void BuildObjectCacheFromNJF(FbConnection connectionNJF)
        {
            objAddrMapToId1NF.Clear();
            objAddrMapToId1NF_By1stNumber.Clear();
            objAddrMapToId1NF_WithDistrict.Clear();

            // Select all objects from the OBJECTS table
            string querySelect = "SELECT OBJECT_KOD, ULNAME, NOMER1, NOMER2, NOMER3, ADRDOP, NEWDISTR, " +
                "ULKOD, SZAG FROM OBJECT WHERE OBJECT_KODSTAN = 1 ORDER BY OBJECT_KOD";

            using (FbCommand commandSelect = new FbCommand(querySelect, connectionNJF))
            {
                using (FbDataReader reader = commandSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Get the object data
                        object dataId = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataStreet = reader.IsDBNull(1) ? null : reader.GetValue(1);
                        object dataNomer1 = reader.IsDBNull(2) ? null : reader.GetValue(2);
                        object dataNomer2 = reader.IsDBNull(3) ? null : reader.GetValue(3);
                        object dataNomer3 = reader.IsDBNull(4) ? null : reader.GetValue(4);
                        object dataMisc = reader.IsDBNull(5) ? null : reader.GetValue(5);
                        object dataDistrict = reader.IsDBNull(6) ? null : reader.GetValue(6);
                        object dataStreetId = reader.IsDBNull(7) ? null : reader.GetValue(7);
                        object dataSqrTotal = reader.IsDBNull(8) ? null : reader.GetValue(8);

                        // Add this address to the cache
                        AddObjectToCache(dataId, dataStreet, null, dataNomer1, dataNomer2, dataNomer3, dataMisc, dataDistrict,
                            dataStreetId, null, 0, dataSqrTotal, null, null, 1);
                    }

                    reader.Close();
                }
            }

            // Prepare the cache of street names
            streetNamesById.Clear();
            streetNames.Clear();

            string query = "SELECT KOD, NAME FROM SUL";

            using (FbCommand command = new FbCommand(query, connectionNJF))
            {
                using (FbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        object dataKod = reader.IsDBNull(0) ? null : reader.GetValue(0);
                        object dataStreet = reader.IsDBNull(1) ? null : reader.GetValue(1);

                        if (dataKod is int && dataStreet is string)
                        {
                            AddStreetToCache((int)dataKod, (string)dataStreet);
                        }
                    }

                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Adds the specifid object to the cache of ObjectFinder
        /// </summary>
        /// <param name="id">Object ID</param>
        /// <param name="dataStreet1">Name of the primary street</param>
        /// <param name="dataStreet2">Name of the secondary street</param>
        /// <param name="dataNomer1">Primary building number</param>
        /// <param name="dataNomer2">Secondary building number</param>
        /// <param name="dataDistrict">District code</param>
        public void AddObjectToCache(object id, object dataStreet1, object dataStreet2,
            object dataNomer1, object dataNomer2, object dataNomer3, object dataMiscInfo, object dataDistrict,
            object dataStreetId1, object dataStreetId2, object dataDeleted,
            object dataSqrTotal, object dataSqrHabit, object dataSqrNonHabit, object dataAddressIsActive)
        {
            if (id is int)
            {
                // Produce full cache of object properties
                ObjectInfo info = new ObjectInfo();

                info.objectId = (int)id;
                info.street = dataStreet1;
                info.street2 = dataStreet2;
                info.nomer1 = dataNomer1;
                info.nomer2 = dataNomer2;
                info.nomer3 = dataNomer3;
                info.addrMiscInfo = dataMiscInfo;
                info.streetId = dataStreetId1;
                info.streetId2 = dataStreetId2;
                info.totalSquare = dataSqrTotal;
                info.habitSquare = dataSqrHabit;
                info.nonHabitSquare = dataSqrNonHabit;
                info.districtId = dataDistrict;

                if (dataDeleted is int)
                {
                    info.deleted = ((int)dataDeleted == 1);
                }

                info.ValidateProperties();

                objectProperties[(int)id] = info;

                if (dataStreet1 is string && (dataNomer1 is string || dataNomer2 is string || dataNomer3 is string))
                {
                    int buildingId = (int)id;
                    string street = ((string)dataStreet1).Trim();
                    string nomer1 = (dataNomer1 is string) ? ((string)dataNomer1).Trim() : "";
                    string nomer2 = (dataNomer2 is string) ? ((string)dataNomer2).Trim() : "";
                    string nomer3 = (dataNomer3 is string) ? ((string)dataNomer3).Trim() : "";
                    string addInfo = (dataMiscInfo is string) ? ((string)dataMiscInfo).Trim() : "";

                    bool objectDeleted = (dataDeleted is int) ? (int)dataDeleted > 0 : false;
                    bool addressIsActive = (dataAddressIsActive is int) ? (int)dataAddressIsActive == 1 : true;

                    // Make sure that district code is valid
                    if (dataDistrict is int && ((int)dataDistrict) == 0)
                    {
                        dataDistrict = null;
                    }

                    if (street.Length > 0 && (nomer1.Length > 0 || nomer2.Length > 0))
                    {
                        string fullAddress = "";
                        string simpleAddress = "";
                        bool addressIsSimple = false;
                        string street2 = (dataStreet2 is string) ? ((string)dataStreet2).Trim() : "";

                        if (street2.Length > 0 && street2 != street)
                        {
                            // Address includes two street names. add both of them, and all combinations
                            GetUnifiedAddressEx(street, nomer1, nomer2, nomer3, addInfo,
                                out fullAddress, out simpleAddress, out addressIsSimple);

                            if (fullAddress.Length > 0 && simpleAddress.Length > 0)
                            {
                                AddObjectToCacheByFullAddress(buildingId, fullAddress, simpleAddress, dataDistrict, objectDeleted, addressIsActive);
                            }

                            GetUnifiedAddressEx(street2, nomer1, nomer2, nomer3, addInfo,
                                out fullAddress, out simpleAddress, out addressIsSimple);

                            if (fullAddress.Length > 0 && simpleAddress.Length > 0)
                            {
                                AddObjectToCacheByFullAddress(buildingId, fullAddress, simpleAddress, dataDistrict, objectDeleted, addressIsActive);
                            }

                            GetUnifiedAddressEx(street + "+" + street2, nomer1, nomer2, nomer3, addInfo,
                                out fullAddress, out simpleAddress, out addressIsSimple);

                            if (fullAddress.Length > 0 && simpleAddress.Length > 0)
                            {
                                AddObjectToCacheByFullAddress(buildingId, fullAddress, simpleAddress, dataDistrict, objectDeleted, addressIsActive);
                            }

                            GetUnifiedAddressEx(street2 + "+" + street, nomer1, nomer2, nomer3, addInfo,
                                out fullAddress, out simpleAddress, out addressIsSimple);

                            if (fullAddress.Length > 0 && simpleAddress.Length > 0)
                            {
                                AddObjectToCacheByFullAddress(buildingId, fullAddress, simpleAddress, dataDistrict, objectDeleted, addressIsActive);
                            }
                        }
                        else
                        {
                            GetUnifiedAddressEx(street, nomer1, nomer2, nomer3, addInfo,
                                out fullAddress, out simpleAddress, out addressIsSimple);

                            if (fullAddress.Length > 0 && simpleAddress.Length > 0)
                            {
                                AddObjectToCacheByFullAddress(buildingId, fullAddress, simpleAddress, dataDistrict, objectDeleted, addressIsActive);
                            }
                        }
                    }
                }
            }
        }

        private void AddObjectToCacheDictionary(Dictionary<string, List<int>> dic, string key, int value)
        {
            List<int> list = null;

            if (dic.TryGetValue(key, out list))
            {
                if (!list.Contains(value))
                {
                    list.Add(value);
                }
            }
            else
            {
                list = new List<int>();
                list.Add(value);
                dic.Add(key, list);
            }
        }

        private void AddObjectToCacheByFullAddress(int buildingId, string fullAddress, string simpleAddress, object district,
            bool objectDeleted, bool addressIsActive)
        {
            // Do not override an existing object by deleted object
            if (objAddrMapToId1NF.ContainsKey(fullAddress))
            {
                if (!objectDeleted && addressIsActive)
                    objAddrMapToId1NF[fullAddress] = buildingId;
            }
            else
            {
                objAddrMapToId1NF[fullAddress] = buildingId;
            }

            // Prepare the partial cache as well
            AddObjectToCacheByFirstNumber(simpleAddress, buildingId);

            if (district is int)
            {
                string key = AddDistrictCodetoAddressKey(fullAddress, (int)district);

                AddObjectToCacheDictionary(objAddrMapToId1NF_WithDistrict, key, buildingId);
                //// Do not override an existing object by deleted object
                //if (objAddrMapToId1NF_WithDistrict.ContainsKey(key))
                //{
                //    if (!objectDeleted && addressIsActive)
                //        objAddrMapToId1NF_WithDistrict[key] = buildingId;
                //}
                //else
                //{
                //    objAddrMapToId1NF_WithDistrict[key] = buildingId;
                //}
            }
        }

        /// <summary>
        /// Helper function; adds the specified building ID to the cache by simple address
        /// </summary>
        /// <param name="simpleAddress">Simple address of the building (which ignores additional information: Korpus, Litera, etc)</param>
        /// <param name="buildingId">Building ID</param>
        private void AddObjectToCacheByFirstNumber(string simpleAddress, int buildingId)
        {
            List<int> list = null;

            if (objAddrMapToId1NF_By1stNumber.TryGetValue(simpleAddress, out list))
            {
                if (!list.Contains(buildingId))
                {
                    list.Add(buildingId);
                }
            }
            else
            {
                list = new List<int>();

                list.Add(buildingId);

                objAddrMapToId1NF_By1stNumber.Add(simpleAddress, list);
            }
        }

        /// <summary>
        /// Produces a unified address from the provided data
        /// </summary>
        /// <param name="street">Street name</param>
        /// <param name="nomer1">Building number</param>
        /// <param name="nomer2">Building number (2)</param>
        /// <param name="nomer3">Building number (3)</param>
        /// <param name="addrDopInfo">Additional address information</param>
        /// <param name="fullAddress">Full unified address is returned in this variable</param>
        /// <param name="simpleAddress">Short unified address is returned in this variable</param>
        /// <param name="addressIsSimple">Indicates that unified address has a form "street NNN", with no suffixes</param>
        public static UnifiedAddress GetUnifiedAddressEx(string street, string nomer1, string nomer2, string nomer3,
            string addrDopInfo, out string fullAddress, out string simpleAddress, out bool addressIsSimple)
        {
            fullAddress = "";
            simpleAddress = "";
            addressIsSimple = false;

            CorrectKnownStreetMisspellings(ref street);

            // Preprocess the address
            string ul = StripStreetName(street.ToUpper());
            string num1 = nomer1.Trim().ToUpper();
            string num2 = nomer2.Trim().ToUpper();
            string num3 = nomer3.Trim().ToUpper();
            string misc = addrDopInfo.Trim().ToUpper();

            // Add all non-empty numbers to an array (for unified processing)
            List<string> numbers = new List<string>();

            if (num1.Length > 0)
            {
                numbers.Add(num1);
            }

            if (num2.Length > 0)
            {
                numbers.Add(num2);
            }

            if (num3.Length > 0)
            {
                numbers.Add(num3);
            }

            if (misc.Length > 0)
            {
                numbers.Add(misc);
            }

            if (numbers.Count == 0)
            {
                return null;
            }
            else
            {
                UnifiedAddress uniAddr = new UnifiedAddress();
                uniAddr.streetName = new StreetName(ul.Trim().ToUpper());

                // Merge al the numbers into a single string
                string numString = "";

                for (int i = 0; i < numbers.Count; i++)
                {
                    if (i > 0)
                    {
                        numString += " ";
                    }

                    numString += numbers[i];
                }

                ParseAddressNumbers(numString, uniAddr);

                addressIsSimple = uniAddr.IsSimple();
                fullAddress = uniAddr.FormatFullAddress();
                simpleAddress = uniAddr.FormatBasicAddress();

                return uniAddr;
            }
        }

        public void AddStreetToCache(int streetId, string streetName)
        {
            streetNamesById[streetId] = new StreetName(streetName);
            streetNames[streetName.Trim().ToLower()] = streetId;
        }

        public bool FindStreetId(string streetName, out int streetId, bool useSimilar = false)
        {
            bool res = streetNames.TryGetValue(streetName.Trim().ToLower(), out streetId);
            if ((!res) && (useSimilar))
            {
                string similarStreetName = string.Empty;
                res = FindSimilarStreetName(streetName, out similarStreetName);
            }
            return res;
        }

        /// <summary>
        /// Finds the object in 1 NF database by address
        /// </summary>
        /// <param name="street">Street name</param>
        /// <param name="nomer1">Building number</param>
        /// <param name="nomer2">Building number (2)</param>
        /// <param name="nomer3">Building number (3)</param>
        /// <param name="addrDopInfo">Additional address information</param>
        /// <param name="district">District code (if NULL, no matching by district is performed)</param>
        /// <param name="square">Object square (if NULL, no matching by object square is performed)</param>
        /// <param name="addressIsSimple">Indicates that provided address is simple (only street and
        /// building number is specified)</param>
        /// <param name="similarAddressExists">Indicates that similar address exists in 1 NF</param>
        /// <returns>ID of the found object, or -1 if object is not found</returns>
        public List<int> FindObject(string street1, string street2, string nomer1, string nomer2, string nomer3,
            string addrDopInfo, object district, object square, out bool addressIsSimple, out bool similarAddressExists)
        {
            similarAddressExists = false;
            addressIsSimple = false;

            string fullAddress = "";
            string simpleAddress = "";
            int buildingId = -1;
            List<int> res = new List<int>();


            // Make sure that district code is valid
            if (district is int && ((int)district) == 0)
            {
                district = null;
            }

            // If street names are not known, try fuzzy matching
            street1 = street1.Trim();
            street2 = street2.Trim();
            string foundStreet = "";

            CorrectKnownStreetMisspellings(ref street1);
            CorrectKnownStreetMisspellings(ref street2);

            if (street1.Length > 0 && !IsKnownStreetName(street1))
            {
                if (FindSimilarStreetName(street1, out foundStreet))
                {
                    street1 = foundStreet;
                }
            }

            if (street2.Length > 0 && !IsKnownStreetName(street2))
            {
                if (FindSimilarStreetName(street2, out foundStreet))
                {
                    street2 = foundStreet;
                }
            }

            if (street1.Length > 0 && street2.Length > 0)
            {
                // Two streets are specified; try combination A+B
                UnifiedAddress uniAddr = GetUnifiedAddressEx(street1 + "+" + street2, nomer1, nomer2, nomer3, addrDopInfo,
                    out fullAddress, out simpleAddress, out addressIsSimple);

                if (uniAddr != null && fullAddress.Length > 0 && simpleAddress.Length > 0)
                {
                    similarAddressExists = objAddrMapToId1NF_By1stNumber.ContainsKey(simpleAddress);
                }

                res = FindObjectByUnifiedAddress(uniAddr, district, square);

                //buildingId = FindObjectByUnifiedAddress(uniAddr, district, square);

                if (res.Count == 0)
                {
                    // Try combination B+A
                    uniAddr = GetUnifiedAddressEx(street2 + "+" + street1, nomer1, nomer2, nomer3, addrDopInfo,
                        out fullAddress, out simpleAddress, out addressIsSimple);

                    if (uniAddr != null && fullAddress.Length > 0 && simpleAddress.Length > 0)
                    {
                        res = FindObjectByUnifiedAddress(uniAddr, district, square);
                    }
                }
            }
            else
            {
                // A single street name is specified
                string street = (street1.Length > 0) ? street1 : street2;

                UnifiedAddress uniAddr = GetUnifiedAddressEx(street, nomer1, nomer2, nomer3, addrDopInfo,
                    out fullAddress, out simpleAddress, out addressIsSimple);

                if (uniAddr != null && fullAddress.Length > 0 && simpleAddress.Length > 0)
                {
                    similarAddressExists = objAddrMapToId1NF_By1stNumber.ContainsKey(simpleAddress);

                    res = FindObjectByUnifiedAddress(uniAddr, district, square);
                }
            }

            return res;
        }

        /// <summary>
        /// Finds an object in 1NF by the street ID and building numbers. This function is lightweight,
        /// and it does not require ObjectFinder to be fully initialized. This is why this function
        /// is static.
        /// </summary>
        /// <param name="connection1NF">Connection to the 1NF database</param>
        /// <param name="streetId">ID of the street in 1NF database</param>
        /// <param name="nomer1">First building number</param>
        /// <param name="nomer2">Second building number</param>
        /// <param name="nomer3">Third building number</param>
        /// <param name="addrDopInfo">Additional address</param>
        /// <returns>Object ID in 1NF, or -1 if the object was not found</returns>
        public static int FindObjectByStreetIdIn1NF(FbConnection connection1NF, int streetId,
            string nomer1, string nomer2, string nomer3, string addrDopInfo)
        {
            int foundObjectId = -1;

            // Prepare the unified address; we can ignore the street name here, because lookup
            // is performed within a single street
            string fullAddress = "";
            string simpleAddress = "";
            bool addressIsSimple = false;

            UnifiedAddress uniAddr = GetUnifiedAddressEx("STREET", nomer1, nomer2, nomer3, addrDopInfo,
                out fullAddress, out simpleAddress, out addressIsSimple);

            if (uniAddr != null)
            {
                // Get all objects related to this street
                using (FbCommand cmd = new FbCommand("SELECT OBJECT_KOD, NOMER1, NOMER2, NOMER3, ADRDOP FROM OBJECT_1NF" +
                    " WHERE ULKOD = @ucod AND ULKOD2 IS NULL AND (DELETED IS NULL OR DELETED = 0)", connection1NF))
                {
                    cmd.Parameters.Add(new FbParameter("ucod", streetId));

                    using (FbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                            string nom1 = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string nom2 = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            string nom3 = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            string misc = reader.IsDBNull(4) ? "" : reader.GetString(4);

                            string fullAddress2 = "";
                            string simpleAddress2 = "";
                            bool addressIsSimple2 = false;

                            UnifiedAddress uniAddr2 = GetUnifiedAddressEx("STREET", nom1, nom2, nom3, misc,
                                out fullAddress2, out simpleAddress2, out addressIsSimple2);

                            if (uniAddr2 != null && fullAddress2 == fullAddress)
                            {
                                foundObjectId = id;
                                break;
                            }
                        }

                        reader.Close();
                    }
                }
            }

            return foundObjectId;
        }

        /// <summary>
        /// Finds an object in SQL Server by the street ID and building numbers. This function is lightweight,
        /// and it does not require ObjectFinder to be fully initialized. This is why this function
        /// is static.
        /// </summary>
        /// <param name="connection">Connection to the SQL Server</param>
        /// <param name="streetId">ID of the street in SQL Server database</param>
        /// <param name="nomer1">First building number</param>
        /// <param name="nomer2">Second building number</param>
        /// <param name="nomer3">Third building number</param>
        /// <param name="addrDopInfo">Additional address</param>
        /// <param name="useOnlyObjectsFrom1NF">If this parameter is TRUE, the function will only return objects
        /// that exist in the 1NF database</param>
        /// <returns>Object ID in SQL Server, or -1 if the object was not found</returns>
        public int FindObjectByStreetId(SqlConnection connection, int streetId,
            string nomer1, string nomer2, string nomer3, string addrDopInfo/*, bool useOnlyObjectsFrom1NF*/)
        {
            int foundObjectId = -1;

            // Prepare the unified address; we can ignore the street name here, because lookup
            // is performed within a single street
            string fullAddress = "";
            string simpleAddress = "";
            bool addressIsSimple = false;

            UnifiedAddress uniAddr = GetUnifiedAddressEx("STREET", nomer1, nomer2, nomer3, addrDopInfo,
                out fullAddress, out simpleAddress, out addressIsSimple);

            if (uniAddr != null)
            {
                // Get all objects related to this street
                string query = "SELECT id, addr_nomer1, addr_nomer2, addr_nomer3, addr_misc FROM buildings" +
                    " WHERE addr_street_id = @ucod AND addr_street_id2 IS NULL AND (is_deleted IS NULL OR is_deleted = 0)";

                //if (useOnlyObjectsFrom1NF)
                //{
                //    query += " AND id < 100000";
                //}

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("ucod", streetId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                            string nom1 = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string nom2 = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            string nom3 = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            string misc = reader.IsDBNull(4) ? "" : reader.GetString(4);

                            string fullAddress2 = "";
                            string simpleAddress2 = "";
                            bool addressIsSimple2 = false;

                            UnifiedAddress uniAddr2 = GetUnifiedAddressEx("STREET", nom1, nom2, nom3, misc,
                                out fullAddress2, out simpleAddress2, out addressIsSimple2);

                            if (uniAddr2 != null && fullAddress2 == fullAddress)
                            {
                                foundObjectId = id;
                                break;
                            }
                        }

                        reader.Close();
                    }
                }
            }

            return foundObjectId;
        }

        /// <summary>
        /// Finds object ID in 1NF by the unified (pre-processed) address
        /// </summary>
        /// <param name="uniAddr">The preprocessed address (does not include district)</param>
        /// <param name="district">District code (optional, if it is known)</param>
        /// <param name="square">Object square (optional, if it is known)</param>
        /// <returns>Object ID, or -1 if specified address was not found</returns>
        private List<int> FindObjectByUnifiedAddress(UnifiedAddress uniAddr, object district, object square)
        {
            int buildingId = -1;
            string fullAddress = uniAddr.FormatFullAddress();

            List<int> outList = null;

            List<int> res = new List<int>();

            // If the district is specified, we need to take it into consideration
            if (district is int)
            {
                if (objAddrMapToId1NF_WithDistrict.TryGetValue(AddDistrictCodetoAddressKey(fullAddress, (int)district), out outList))
                {
                    res.AddRange(outList);
                }
                return res;
            }

            // Could not find by district; search for any matching address
            if (objAddrMapToId1NF.TryGetValue(fullAddress, out buildingId))
            {
                res.Add(buildingId);
                return res;
                //return buildingId;
            }

            // Not found; try to add 'LITERA A', if it is missing
            // We can perform this search only of object square is known
            if (square is decimal || allowLiterAComparisonWithoutSquareMatch)
            {
                decimal sqr = allowLiterAComparisonWithoutSquareMatch ? 0 : (decimal)square;

                if (sqr > 0 || allowLiterAComparisonWithoutSquareMatch)
                {
                    bool repeatSearch = false;

                    UnifiedAddress addrCopy = uniAddr.MakeCopy();

                    if (addrCopy.literaSpecified)
                    {
                        if (addrCopy.litera.Trim() == Resources.LiteraA)
                        {
                            addrCopy.litera = "";
                            repeatSearch = true;
                        }
                    }
                    else
                    {
                        addrCopy.litera = Resources.LiteraA;
                        repeatSearch = true;
                    }

                    if (repeatSearch)
                    {
                        fullAddress = addrCopy.FormatFullAddress();

                        // If the district is specified, we need to take it into consideration
                        if (district is int)
                        {
                            if (objAddrMapToId1NF_WithDistrict.TryGetValue(AddDistrictCodetoAddressKey(fullAddress, (int)district), out outList))
                            {
                                foreach (int i in outList)
                                {
                                    // Object square must match
                                    if (allowLiterAComparisonWithoutSquareMatch || ObjectSquareMatch(i, sqr))
                                    {
                                        res.Add(i);
                                        //return buildingId;
                                    }
                                }
                                if (res.Count > 0)
                                    return res;
                            }
                        }

                        // Could not find by district; search for any matching address
                        if (objAddrMapToId1NF.TryGetValue(fullAddress, out buildingId))
                        {
                            // Object square must match
                            if (allowLiterAComparisonWithoutSquareMatch || ObjectSquareMatch(buildingId, sqr))
                            {
                                res.Add(buildingId);
                                return res;
                            }
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Verifies if object square is equal to the provided value
        /// </summary>
        /// <param name="objectId">Object ID</param>
        /// <param name="sqr">Square to compare to</param>
        /// <returns>TRUE if the given object has the specified square</returns>
        private bool ObjectSquareMatch(int objectId, decimal sqr)
        {
            ObjectInfo info = null;

            if (objectProperties.TryGetValue(objectId, out info))
            {
                if (info.totalSquare is decimal)
                {
                    decimal totalSqr = (decimal)info.totalSquare;

                    return totalSqr == sqr;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the street name by the street ID from the SUL table in 1NF.
        /// </summary>
        /// <param name="id">Street Id</param>
        /// <param name="name">Street name</param>
        /// <returns>TRUE if the specified Id is present in the dictionary</returns>
        public bool GetStreetName(int id, out string name)
        {
            StreetName sn = null;

            if (streetNamesById.TryGetValue(id, out sn))
            {
                name = sn.street;
                return true;
            }

            name = "";
            return false;
        }

        /// <summary>
        /// Returns information stored in 1NF for the specified object
        /// </summary>
        /// <param name="objectId">Object ID</param>
        /// <returns>Object properties</returns>
        public ObjectInfo GetObjectInfo(int objectId)
        {
            ObjectInfo info = null;

            if (objectProperties.TryGetValue(objectId, out info))
            {
                return info;
            }

            return null;
        }

        /// <summary>
        /// Returns meaningful description of the specified object
        /// </summary>
        /// <param name="objectId">Object ID</param>
        /// <returns>Object description</returns>
        public string GetObjectDescription(int objectId)
        {
            ObjectInfo info = null;

            if (objectProperties.TryGetValue(objectId, out info))
            {
                return info.FormatToStr();
            }

            return "";
        }

        /// <summary>
        /// Verifies if the specified street name is present in the 1NF 'SUL' table
        /// </summary>
        /// <param name="streetName">Street name</param>
        /// <returns>TRUE is this street name is known</returns>
        public bool IsKnownStreetName(string streetName)
        {
            return streetNames.ContainsKey(streetName.Trim().ToLower());
        }

        /// <summary>
        /// Tries to find a known valid street name which is close to the specified pattern
        /// </summary>
        /// <param name="pattern">A possible misspelled name of street</param>
        /// <param name="streetName"></param>
        /// <returns></returns>
        public bool FindSimilarStreetName(string pattern, out string streetName)
        {
            if (CorrectKnownStreetMisspellings(ref pattern))
            {
                streetName = pattern;
                return true;
            }

            // Parse the provided pattern
            StreetName snPattern = new StreetName(pattern);

            // Try to find a similar street name in the full list, obtained from 1 NF
            StreetName bestMatch = null;
            int bestMatchNumErrors = int.MaxValue;

            foreach (KeyValuePair<int, StreetName> pair in streetNamesById)
            {
                StreetName sn = pair.Value;

                // If city is specified in any pattern, we must search for an address in the same city
                if ((snPattern.city.Count > 0 || sn.city.Count > 0) && !FuzzyMatchLists(snPattern.city, sn.city, maxAllowedFuzzyErrors))
                {
                    continue;
                }

                // If village is specified in the pattern, we must search for an address in the same village
                if ((snPattern.village.Count > 0 || sn.village.Count > 0) && !FuzzyMatchLists(snPattern.village, sn.village, maxAllowedFuzzyErrors))
                {
                    continue;
                }

                // If street type is specified for both streets, it must match
                if ((snPattern.streetType != StreetType.Street || sn.streetType != StreetType.Street) && snPattern.streetType != sn.streetType)
                {
                    continue;
                }

                // If this is a pure city name (without street name), match it to another pure city name
                if ((snPattern.city.Count > 0 || snPattern.village.Count > 0))
                {
                    if (snPattern.streetParts.Count == 0 && sn.streetParts.Count == 0)
                    {
                        bestMatch = sn;
                        break;
                    }
                }

                // Match the street names
                int numErrors = FuzzyMatchListsNumErrors(snPattern.streetParts, sn.streetParts, maxAllowedFuzzyErrors);

                if (numErrors >= 0 && numErrors < bestMatchNumErrors)
                {
                    bestMatchNumErrors = numErrors;
                    bestMatch = sn;
                }
            }

            if (bestMatch != null)
            {
                streetName = bestMatch.street;
                return true;
            }

            streetName = "";
            return false;
        }

        /// <summary>
        /// Performs fuzzy comparison of two string lists
        /// </summary>
        /// <param name="list1">The first list</param>
        /// <param name="list2">The second list</param>
        /// <param name="maxAllowedErrors">Maximum number of allowed errors</param>
        /// <returns>TRUE if lists can be considered equal</returns>
        private bool FuzzyMatchLists(List<string> list1, List<string> list2, int maxAllowedErrors)
        {
            return FuzzyMatchListsNumErrors(list1, list2, maxAllowedErrors) <= maxAllowedErrors;
        }

        /// <summary>
        /// Returns amount of errors found in fuzzy match of two lists
        /// </summary>
        /// <param name="list1">The first list</param>
        /// <param name="list2">The second list</param>
        /// <param name="maxAllowedErrors">Maximum number of allowed errors</param>
        /// <returns>Amount of found errors</returns>
        private int FuzzyMatchListsNumErrors(List<string> list1, List<string> list2, int maxAllowedErrors)
        {
            if (list1.Count == list2.Count && list1.Count > 0)
            {
                // Remove the identical words from the lists
                HashSet<string> identicals = new HashSet<string>();

                foreach (string s in list1)
                {
                    if (list2.Contains(s))
                    {
                        identicals.Add(s);
                    }
                }

                // If all words are identical, we have an exact match
                if (identicals.Count == list1.Count)
                {
                    return 0;
                }

                // We can not modify the provided lists; produce two copies
                List<string> copy1 = new List<string>();
                List<string> copy2 = new List<string>();

                copy1.AddRange(list1);
                copy2.AddRange(list2);

                // Remove the words which are present in both lists
                foreach (string s in identicals)
                {
                    copy1.Remove(s);
                    copy2.Remove(s);
                }

                // Try to match the remaining words against each other
                int totalErrors = 0;

                foreach (string s1 in copy1)
                {
                    // Find the best match in the other list (with minimum count of errors)
                    int minFoundErrors = int.MaxValue;

                    foreach (string s2 in copy2)
                    {
                        int numErrors = Itg.Utility.Fuzzy.LevenshteinDistanceReference.CalculateDistance(s1, s2, maxAllowedErrors + 1);

                        if (numErrors >= 0 && numErrors < minFoundErrors)
                        {
                            minFoundErrors = numErrors;
                        }
                    }

                    totalErrors += minFoundErrors;

                    // Stop as soon as allowed amount of errors is reached
                    if (totalErrors > maxAllowedErrors)
                    {
                        return int.MaxValue;
                    }
                }

                return totalErrors;
            }

            return int.MaxValue;
        }

        /// <summary>
        /// Enables or disables square check when matching addresses with 'LITERA A' to addresses
        /// with no LITERA at all
        /// </summary>
        /// <param name="allow">TRUE if square check should be disabled</param>
        public void AllowLiteraAComparisonWithoutSquareMatch(bool allow)
        {
            allowLiterAComparisonWithoutSquareMatch = allow;
        }

        /// <summary>
        /// Finds all objects that match the simple address: street and number
        /// </summary>
        /// <param name="street">Street name</param>
        /// <param name="buildingNumber">Building number</param>
        /// <param name="buildings">The list of found building IDs</param>
        public void FindAllObjectsByPrimaryAddress(string street, string buildingNumber, List<int> buildings)
        {
            buildings.Clear();

            CorrectKnownStreetMisspellings(ref street);

            street = street.Trim().ToUpper();
            buildingNumber = buildingNumber.Trim().ToUpper();

            if (street.Length > 0 && buildingNumber.Length > 0)
            {
                // Make sure that street is known
                if (!IsKnownStreetName(street))
                {
                    string foundStreet = "";

                    if (FindSimilarStreetName(street, out foundStreet))
                    {
                        street = foundStreet;
                    }
                    else
                    {
                        street = "";
                    }
                }

                if (street.Length > 0)
                {
                    // Create a unified address
                    string fullAddress = "";
                    string simpleAddress = "";
                    bool addressIsSimple = false;

                    UnifiedAddress uniAddr = GetUnifiedAddressEx(street, buildingNumber, "", "", "",
                        out fullAddress, out simpleAddress, out addressIsSimple);

                    // Get all buildings by this simple address
                    List<int> list = null;

                    if (objAddrMapToId1NF_By1stNumber.TryGetValue(simpleAddress, out list))
                    {
                        buildings.AddRange(list);
                    }

                    // If building number looks like '50A', try to search without the letter as well
                    if (uniAddr.number.Length > 1 &&
                        char.IsLetter(uniAddr.number[uniAddr.number.Length - 1]) &&
                        IsNumberSequence(uniAddr.number.Substring(0, uniAddr.number.Length - 1)))
                    {
                        uniAddr.number = uniAddr.number.Substring(0, uniAddr.number.Length - 1);

                        if (objAddrMapToId1NF_By1stNumber.TryGetValue(uniAddr.FormatBasicAddress(), out list))
                        {
                            foreach (int buildingId in list)
                            {
                                if (!buildings.Contains(buildingId))
                                {
                                    buildings.Add(buildingId);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Replaces some known street misspellings with correct street names from 1NF
        /// </summary>
        /// <param name="street">Street name</param>
        /// <returns>TRUE if street name was corrected</returns>
        public static bool CorrectKnownStreetMisspellings(ref string street)
        {
            // Special cases
            if (street.Contains(Resources.StreetAntonovaShort))
            {
                if (street.Contains(Resources.StreetOvsienkaShort))
                {
                    street = Resources.StreetAntonovaOvs;
                }
                else
                {
                    street = Resources.StreetAntonovaAvia;
                }

                return true;
            }

            // General cases
            foreach (string correction in GUKV.DataMigration.Properties.Settings.Default.StreetNameCorrections)
            {
                string[] parts = correction.Split(new char[] { '=' });

                if (parts.Length == 2)
                {
                    string pattern = parts[0].Trim();
                    string value = parts[1].Trim();

                    if (pattern.Length > 0 && value.Length > 0)
                    {
                        // If pattern looks like "xxx"=yyy, then we need to search for the exact word
                        if (pattern.StartsWith("\"") && pattern.EndsWith("\""))
                        {
                            string patternWord = pattern.Substring(1, pattern.Length - 2);
                            string[] words = street.Split(' ');

                            foreach (string word in words)
                            {
                                if (word == patternWord)
                                {
                                    street = value;
                                    return true;
                                }
                            }
                        }
                        else if (street.Contains(pattern))
                        {
                            street = value;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Removes all prefixes from the building number (LIT., K., etc)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string StripBuildingNumber(string num)
        {
            string[] prefixes = new string[]
            {
                litPrefixLITd,
                litPrefixLITERd,
                litPrefixLITERA,
                litPrefixLITER,
                litPrefixLITER_d,
                litPrefixLITER__d,
                litPrefixLITER_sh,
                litPrefixLIT,
                korpusPrefixCorrect,
                korpusPrefix1,
                korpusPrefix2,
                korpusPrefix3,
            };

            foreach (string prefix in prefixes)
            {
                if (num.StartsWith(prefix))
                {
                    return num.Substring(prefix.Length).Trim();
                }
            }

            return num;
        }

        /// <summary>
        /// Use this function to correct building numbers before adding new objects
        /// (or updating existing objects) in 1NF. The first building number must always
        /// be a pure number, no letters allowed.
        /// </summary>
        /// <param name="number1">The first building number</param>
        /// <param name="number2">The second building number</param>
        public static void PreProcessBuildingNumbers(ref string number1, ref string number2)
        {
            if (number1.Length > 9 || number2.Length > 18)
            {
                number1 = number1.Trim().Substring(0, 9);
                number2 = number2.Trim().Substring(0, 18);
            }

            number1 = number1.Trim();
            number2 = number2.Trim();

            // Find any non-digit in the primary number
            for (int i = 0; i < number1.Length; i++)
            {
                if (!char.IsDigit(number1[i]))
                {
                    string actualNumber1 = number1.Substring(0, i);
                    string actualNumber2 = number1.Substring(i).Trim();

                    if (number2.Length > 0)
                    {
                        actualNumber2 += " " + number2;
                    }

                    if (actualNumber2.Length > 18)
                    {
                        actualNumber2 = actualNumber2.Substring(0, 18);
                    }

                    number1 = actualNumber1;
                    number2 = actualNumber2;
                    break;
                }
            }
        }

        /// <summary>
        /// Generates maunal mapping of some well-known street codes from NJF database to 1NF database
        /// </summary>
        /// <returns>Created manual mapping of IDs</returns>
        public static Dictionary<int, int> GetStreetCodeMatchNJFto1NF()
        {
            Dictionary<int, int> streetCodeMappingNJF21NF = new Dictionary<int, int>();

            streetCodeMappingNJF21NF.Add(69, 69);
            streetCodeMappingNJF21NF.Add(8025, 285);
            streetCodeMappingNJF21NF.Add(8027, 8131);
            streetCodeMappingNJF21NF.Add(98, 98);
            streetCodeMappingNJF21NF.Add(105, 8131);
            streetCodeMappingNJF21NF.Add(173, 173);
            streetCodeMappingNJF21NF.Add(235, 235);
            streetCodeMappingNJF21NF.Add(256, 256);
            streetCodeMappingNJF21NF.Add(278, 278);
            streetCodeMappingNJF21NF.Add(291, 2372);
            streetCodeMappingNJF21NF.Add(320, 320);
            streetCodeMappingNJF21NF.Add(346, 346);
            streetCodeMappingNJF21NF.Add(429, 429);
            streetCodeMappingNJF21NF.Add(478, 478);
            streetCodeMappingNJF21NF.Add(531, 531);
            streetCodeMappingNJF21NF.Add(551, 551);
            streetCodeMappingNJF21NF.Add(563, 563);
            streetCodeMappingNJF21NF.Add(597, 597);
            streetCodeMappingNJF21NF.Add(657, 657);
            streetCodeMappingNJF21NF.Add(658, 889);
            streetCodeMappingNJF21NF.Add(667, 667);
            streetCodeMappingNJF21NF.Add(683, 683);
            streetCodeMappingNJF21NF.Add(694, 694);
            streetCodeMappingNJF21NF.Add(723, 723);
            streetCodeMappingNJF21NF.Add(761, 1203);
            streetCodeMappingNJF21NF.Add(770, 770);
            streetCodeMappingNJF21NF.Add(784, 8268);
            streetCodeMappingNJF21NF.Add(816, 57);
            streetCodeMappingNJF21NF.Add(828, 828);
            streetCodeMappingNJF21NF.Add(855, 855);
            streetCodeMappingNJF21NF.Add(878, 878);
            streetCodeMappingNJF21NF.Add(881, 881);
            streetCodeMappingNJF21NF.Add(882, 882);
            streetCodeMappingNJF21NF.Add(949, 949);
            streetCodeMappingNJF21NF.Add(1050, 1050);
            streetCodeMappingNJF21NF.Add(1060, 1060);
            streetCodeMappingNJF21NF.Add(1061, 1061);
            streetCodeMappingNJF21NF.Add(1107, 1107);
            streetCodeMappingNJF21NF.Add(1137, 1137);
            streetCodeMappingNJF21NF.Add(1145, 1145);
            streetCodeMappingNJF21NF.Add(1149, 1149);
            streetCodeMappingNJF21NF.Add(1150, 1150);
            streetCodeMappingNJF21NF.Add(1151, 1151);
            streetCodeMappingNJF21NF.Add(1152, 1152);
            streetCodeMappingNJF21NF.Add(1154, 1137);
            streetCodeMappingNJF21NF.Add(1155, 1155);
            streetCodeMappingNJF21NF.Add(1204, 1204);
            streetCodeMappingNJF21NF.Add(1347, 1347);
            streetCodeMappingNJF21NF.Add(1379, 1379);
            streetCodeMappingNJF21NF.Add(1478, 1478);
            streetCodeMappingNJF21NF.Add(1598, 1598);
            streetCodeMappingNJF21NF.Add(1604, 566);
            streetCodeMappingNJF21NF.Add(1607, 1607);
            streetCodeMappingNJF21NF.Add(1820, 1820);
            streetCodeMappingNJF21NF.Add(1872, 1872);
            streetCodeMappingNJF21NF.Add(2642, 275);
            streetCodeMappingNJF21NF.Add(2705, 2705);
            streetCodeMappingNJF21NF.Add(2720, 2720);
            streetCodeMappingNJF21NF.Add(4010, 4010);
            streetCodeMappingNJF21NF.Add(5010, 5010);
            streetCodeMappingNJF21NF.Add(5014, 5014);
            streetCodeMappingNJF21NF.Add(5015, 704);
            streetCodeMappingNJF21NF.Add(8006, 1879);
            streetCodeMappingNJF21NF.Add(8014, 8119);
            streetCodeMappingNJF21NF.Add(8057, 8119);
            streetCodeMappingNJF21NF.Add(8018, 1820);
            streetCodeMappingNJF21NF.Add(8019, 788);
            streetCodeMappingNJF21NF.Add(8021, 2062);
            streetCodeMappingNJF21NF.Add(8028, 1478);
            streetCodeMappingNJF21NF.Add(8031, 8226);
            streetCodeMappingNJF21NF.Add(8032, 1065);
            streetCodeMappingNJF21NF.Add(8034, 1042);
            streetCodeMappingNJF21NF.Add(8040, 107);
            streetCodeMappingNJF21NF.Add(8041, 969);
            streetCodeMappingNJF21NF.Add(8046, 5043);
            streetCodeMappingNJF21NF.Add(8049, 1694);
            streetCodeMappingNJF21NF.Add(8053, 8028);
            streetCodeMappingNJF21NF.Add(8060, 969);
            streetCodeMappingNJF21NF.Add(8071, 832);
            streetCodeMappingNJF21NF.Add(8078, 8229);
            streetCodeMappingNJF21NF.Add(8079, 1137);
            streetCodeMappingNJF21NF.Add(8082, 431);
            streetCodeMappingNJF21NF.Add(8092, 8261);
            streetCodeMappingNJF21NF.Add(8106, 8232);
            streetCodeMappingNJF21NF.Add(8107, 8179);
            streetCodeMappingNJF21NF.Add(8108, 8218);
            streetCodeMappingNJF21NF.Add(8109, 8214);
            streetCodeMappingNJF21NF.Add(8111, 8270);

            return streetCodeMappingNJF21NF;
        }

        #endregion (Interface)

        #region Implementation

        private static string StripStreetName(string street)
        {
            string str = street.Trim();

            if (str == Resources.LesiUkrainkiShort)
            {
                str = Resources.LesiUkrainkiCorrect;
            }

            return str;
        }

        public static void ParseAddressNumbers(string numbers, UnifiedAddress address)
        {
            numbers = numbers.Trim().ToUpper();

            if (numbers == ".")
            {
                return;
            }

            numbers = numbers.Replace(Resources.BuildingAddressNoNumber,
                Resources.BuildingAddressNoNumberCorrect);

            numbers = numbers.Replace(Resources.BuildingPrefixNumber, "");

            // Remove the duplicate KORPOS prefix
            int dupCorpusPos = numbers.IndexOf(Resources.BuildingKorpusDuplicate);

            while (dupCorpusPos >= 0)
            {
                numbers = numbers.Remove(dupCorpusPos, Resources.BuildingKorpusDuplicate.Length);

                dupCorpusPos = numbers.IndexOf(Resources.BuildingKorpusDuplicate);
            }

            numbers = numbers.Trim();

            // Check special suffixes
            ParseMiscPartOfAddress(ref numbers, address);

            // Remove multiple quotes
            int posQuote1 = numbers.IndexOf('"');

            if (posQuote1 >= 0)
            {
                int posQuote2 = numbers.IndexOf('"', posQuote1 + 1);

                if (posQuote2 == posQuote1 + 2 && (char.IsLetter(numbers[posQuote1 + 1])))
                {
                    numbers = numbers.Remove(posQuote2, 1);
                    numbers = numbers.Remove(posQuote1, 1);
                }
            }

            // Remove all DOK suffixes
            int pos = numbers.IndexOf(dockPrefix1);

            if (pos >= 0)
            {
                address.dock = numbers.Substring(pos + dockPrefix1.Length).Trim();
                numbers = numbers.Substring(0, pos);
            }

            // Remove all Part suffixes
            pos = numbers.IndexOf(partPrefix1);

            if (pos >= 0)
            {
                address.part = numbers.Substring(pos + partPrefix1.Length).Trim();
                numbers = numbers.Substring(0, pos);
            }

            // Remove all Lit. suffixes
            pos = numbers.IndexOf(litPrefixLITER_sh);

            if (pos >= 0)
            {
                address.litera = numbers.Substring(pos + litPrefixLITER_sh.Length).Trim();
                address.literaSpecified = true;
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(litPrefixLITd);

            if (pos >= 0)
            {
                address.litera = numbers.Substring(pos + litPrefixLITd.Length).Trim();
                address.literaSpecified = true;
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(litPrefixLITERd);

            if (pos >= 0)
            {
                address.litera = numbers.Substring(pos + litPrefixLITERd.Length).Trim();
                address.literaSpecified = true;
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(litPrefixLITER__d);

            if (pos >= 0)
            {
                address.litera = numbers.Substring(pos + litPrefixLITER__d.Length).Trim();
                address.literaSpecified = true;
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(litPrefixLITER_d);

            if (pos >= 0)
            {
                address.litera = numbers.Substring(pos + litPrefixLITER_d.Length).Trim();
                address.literaSpecified = true;
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(litPrefixLITERA);

            if (pos >= 0)
            {
                address.litera = numbers.Substring(pos + litPrefixLITERA.Length).Trim();
                address.literaSpecified = true;
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(litPrefixLITER);

            if (pos >= 0)
            {
                address.litera = numbers.Substring(pos + litPrefixLITER.Length).Trim();
                address.literaSpecified = true;
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(litPrefixLIT);

            if (pos >= 0)
            {
                address.litera = numbers.Substring(pos + litPrefixLIT.Length).Trim();
                address.literaSpecified = true;
                numbers = numbers.Substring(0, pos);
            }

            // Consider all special BTI suffixes
            pos = numbers.IndexOfAny(btiSuffixSeparators);

            if (pos > 0)
            {
                pos--;

                while (pos > 0 && (char.IsLetter(numbers[pos - 1]) || numbers[pos - 1] == ','))
                {
                    pos--;
                }

                if (pos >= 0 && char.IsLetter(numbers[pos]))
                {
                    address.litera = numbers.Substring(pos).Trim();
                    numbers = (pos > 0) ? numbers.Substring(0, pos) : "";
                }
            }

            // Remove all KORP. suffixes
            pos = numbers.IndexOf(korpusPrefix1);

            if (pos >= 0)
            {
                address.korpus = numbers.Substring(pos + korpusPrefix1.Length).Trim();
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(korpusPrefix2);

            if (pos >= 0)
            {
                address.korpus = numbers.Substring(pos + korpusPrefix2.Length).Trim();
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(korpusPrefixCorrect);

            if (pos >= 0)
            {
                address.korpus = numbers.Substring(pos + korpusPrefixCorrect.Length).Trim();
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(korpusPrefix3);

            if (pos >= 0)
            {
                address.korpus = numbers.Substring(pos + korpusPrefix3.Length).Trim();
                numbers = numbers.Substring(0, pos);
            }

            pos = numbers.IndexOf(".");

            if (pos >= 0)
            {
                address.korpus = numbers.Substring(pos + 1).Trim();
                numbers = numbers.Substring(0, pos);
            }

            // If there is a slash, split the address into two
            pos = numbers.IndexOf('/');

            if (pos > 0)
            {
                address.slashNumber = numbers.Substring(pos + 1).Trim();
                numbers = numbers.Substring(0, pos);
            }

            // If there is a dash, split the address into two
            pos = numbers.IndexOf('-');

            if (pos > 0)
            {
                address.rangeNumber = numbers.Substring(pos + 1).Trim();
                numbers = numbers.Substring(0, pos);
            }

            // What remains is a number
            address.number = numbers;

            // Post-processing 1: check if there is a korpus string in the litera
            pos = address.litera.IndexOf(korpusPrefix1);

            if (pos >= 0)
            {
                address.korpus = address.litera.Substring(pos + korpusPrefix1.Length).Trim();
                address.litera = address.litera.Substring(0, pos);
            }

            pos = address.litera.IndexOf(korpusPrefix2);

            if (pos >= 0)
            {
                address.korpus = address.litera.Substring(pos + korpusPrefix2.Length).Trim();
                address.litera = address.litera.Substring(0, pos);
            }

            pos = address.litera.IndexOf(korpusPrefixCorrect);

            if (pos >= 0)
            {
                address.korpus = address.litera.Substring(pos + korpusPrefixCorrect.Length).Trim();
                address.litera = address.litera.Substring(0, pos);
            }

            pos = address.litera.IndexOf(korpusPrefix3);

            if (pos >= 0)
            {
                address.korpus = address.litera.Substring(pos + korpusPrefix3.Length).Trim();
                address.litera = address.litera.Substring(0, pos);
            }

            // Post-processing 2: remove unnecessary characters
            address.slashNumber = address.slashNumber.Replace(" ", "");
            address.slashNumber = address.slashNumber.Replace("(", "");
            address.slashNumber = address.slashNumber.Replace(")", "");
            address.slashNumber = address.slashNumber.Replace(",", "");

            address.rangeNumber = address.rangeNumber.Replace(" ", "");
            address.rangeNumber = address.rangeNumber.Replace("(", "");
            address.rangeNumber = address.rangeNumber.Replace(")", "");
            address.rangeNumber = address.rangeNumber.Replace(",", "");

            address.korpus = address.korpus.Replace(" ", "");
            address.korpus = address.korpus.Replace("(", "");
            address.korpus = address.korpus.Replace(")", "");
            address.korpus = address.korpus.Replace(",", "");

            address.litera = address.litera.Replace(" ", "");
            address.litera = address.litera.Replace("(", "");
            address.litera = address.litera.Replace(")", "");

            if (address.litera.StartsWith("-"))
            {
                address.litera = address.litera.Substring(1);
            }

            // Convert addresses like "1 A" to "1A"
            address.number = address.number.Replace("(", "");
            address.number = address.number.Replace(")", "");
            address.number = address.number.Replace(',', ' ');

            string[] tokens = address.number.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);

            // Re-build the address from the parts (this removes double spaces)
            address.number = "";

            for (int n = 0; n < tokens.Length; n++)
            {
                if (n > 0)
                {
                    address.number += " ";
                }

                address.number += tokens[n];
            }

            if (tokens.Length == 2)
            {
                if (IsNumberSequence(tokens[1]))
                {
                    // Two numbers in a row: make it a slash number
                    if (address.slashNumber.Length == 0)
                    {
                        address.number = tokens[0];
                        address.slashNumber = tokens[1];
                    }
                    else
                    {
                        // If the numbers are equal, ignore one of them
                        if (tokens[0] == tokens[1] && tokens[0].Length > 1)
                        {
                            address.number = tokens[0];
                        }
                    }
                }
                else
                {
                    // Remove the unnecessary separator
                    address.number = tokens[0] + tokens[1];
                }
            }

            // Special processing for numbers like "1-A"
            if (IsNumberSequence(address.number) && IsLetterSequence(address.rangeNumber))
            {
                address.number += address.rangeNumber;
                address.rangeNumber = "";
            }
        }

        private static void ParseMiscPartOfAddress(ref string numbers, UnifiedAddress address)
        {
            foreach (string suffix in GUKV.DataMigration.Properties.Settings.Default.AddressMiscSuffixes)
            {
                if (numbers.EndsWith(suffix))
                {
                    address.misc = suffix;

                    numbers = numbers.Substring(0, numbers.Length - suffix.Length).Trim();

                    break;
                }
            }
        }

        private static bool IsNumberSequence(string str)
        {
            foreach (char ch in str)
            {
                if (!char.IsNumber(ch))
                {
                    return false;
                }
            }

            return str.Length > 0;
        }

        private static bool IsLetterSequence(string str)
        {
            foreach (char ch in str)
            {
                if (!char.IsLetter(ch))
                {
                    return false;
                }
            }

            return str.Length > 0;
        }

        /// <summary>
        /// Extracts the letter from a building number which looks like "41A"
        /// </summary>
        /// <param name="number">Building number</param>
        /// <param name="letter">The extracted letter is returned in this parameter</param>
        /// <returns>TRUE if letter was found in the number</returns>
        private bool GetBuildingNumberLetter(string number, out char letter)
        {
            letter = ' ';

            if (number.Length > 1 &&
                char.IsLetter(number[number.Length - 1]) &&
                IsNumberSequence(number.Substring(0, number.Length - 1)))
            {
                letter = number[number.Length - 1];
                return true;
            }

            return false;
        }

        private string AddDistrictCodetoAddressKey(string addressKey, int district)
        {
            return district.ToString() + "D_" + addressKey;
        }

        #endregion (Implementation)

        #region Inprecise address lookup

        public bool FindObjectIDsByInpreciseAddress(string address, List<int> objectIDs,
            bool allowInpreciseAddressMath, out bool preciseMatch)
        {
            objectIDs.Clear();
            preciseMatch = true;

            // Remove some unnecessary characters
            address = address.Replace("\"", "");
            address = address.Replace("\xAB", ""); // Left-pointing double angle quotation mark
            address = address.Replace("\xBB", ""); // Right-pointing double angle quotation mark
            address = address.Replace("\x201C", ""); // Left double quotation mark (Unicode)
            address = address.Replace("\x201D", ""); // Right double quotation mark (Unicode)
            address = address.Replace(":", "");

            while (address.IndexOf("..") >= 0)
            {
                address = address.Replace("..", ".");
            }

            // Split the address into parts
            string[] parts = address.ToUpper().Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            // Remove all tokens that indicate crossroads - we can not use this information
            List<string> tokens = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                string pt = parts[i].Trim();

                if (!IsCrossRoadToken(pt))
                {
                    tokens.Add(pt);
                }
            }

            // Comma may be skipped
            if (tokens.Count == 1)
            {
                string[] parts2 = tokens[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts2.Length > 1 && IsValidNumberToken(parts2[parts2.Length - 1]))
                {
                    string street = "";

                    for (int i = 0; i < parts2.Length - 1; i++)
                    {
                        street += " " + parts2[i];
                    }

                    tokens.Clear();

                    tokens.Add(street);
                    tokens.Add(parts2[parts2.Length - 1]);
                }
            }

            // Individual LITERA tokens should be appended to their respective "number" tokens
            CheckHangingLiteraTokens(tokens);

            // Perform address matching
            if (tokens.Count > 1)
            {
                // Trim all parts of the address
                for (int i = 0; i < tokens.Count; i++)
                {
                    tokens[i] = tokens[i].Trim();
                }

                string street = "";
                string number = "";
                int prevReplacedPartOfNumberLen = 0;

                for (int i = 0; i < tokens.Count; i++)
                {
                    string token = tokens[i];

                    if (IsStreetToken(ref token) || IsKnownStreetName(token))
                    {
                        // Start a new street
                        street = token;
                        number = "";
                        prevReplacedPartOfNumberLen = 0;

                        // Try to find a similar street in 1NF
                        if (!IsKnownStreetName(street))
                        {
                            string actualStreet = "";

                            if (FindSimilarStreetName(street, out actualStreet))
                            {
                                street = actualStreet;
                            }
                            else
                            {
                                street = "";
                                preciseMatch = false; // Notify user that match must be verified
                            }
                        }
                    }
                    else
                    {
                        // This is a number token
                        if (street.Length > 0)
                        {
                            if (!IsValidNumberToken(token) && number.Length > token.Length)
                            {
                                number = ReplaceNumberSuffix(number, token, ref prevReplacedPartOfNumberLen);
                            }
                            else
                            {
                                number = token;
                                prevReplacedPartOfNumberLen = 0;
                            }

                            // Perform lookup
                            if (number.Length > 0)
                            {
                                if (allowInpreciseAddressMath)
                                {
                                    List<int> foundIDs = new List<int>();

                                    FindAllObjectsByPrimaryAddress(street, number, foundIDs);

                                    foreach (int objectId in foundIDs)
                                    {
                                        if (objectId > 0 && !objectIDs.Contains(objectId))
                                        {
                                            objectIDs.Add(objectId);
                                        }
                                    }
                                }
                                else
                                {
                                    bool addressIsSimple = false;
                                    bool similarAddressExists = false;

                                    List<int> foundIDs = FindObject(street, "", number, "", "", "", null, null,
                                        out addressIsSimple, out similarAddressExists);

                                    foreach (int objectId in foundIDs)
                                    {
                                        if (objectId > 0 && !objectIDs.Contains(objectId))
                                        {
                                            objectIDs.Add(objectId);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Something went wrong; notify user that match must be verified
                            preciseMatch = false;
                        }
                    }
                }
            }

            return objectIDs.Count > 0;
        }

        private void CheckHangingLiteraTokens(List<string> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];

                if (IsLiteraToken(token))
                {
                    // Check if we can append it to a previous token
                    if (i > 0)
                    {
                        tokens[i - 1] = tokens[i - 1] + " " + token;

                        tokens[i] = "";
                    }
                }
            }

            tokens.RemoveAll(IsEmptyString);
        }

        private static bool IsEmptyString(string s)
        {
            return s.Length == 0;
        }

        private string ReplaceNumberSuffix(string number, string newSuffix, ref int prevReplacedPartOfNumberLen)
        {
            int numCharsToRemove = prevReplacedPartOfNumberLen;
            prevReplacedPartOfNumberLen = newSuffix.Length;

            if (numCharsToRemove <= 0)
            {
                // Check how many characters we can remove from the end of number
                int maxAllowedCharsToRemove = 0;
                int i = number.Length - 1;
                bool letterFound = false;

                while (i >= 0 && char.IsDigit(number[i]))
                {
                    maxAllowedCharsToRemove++;
                    i--;
                }

                while (i >= 0 && char.IsLetter(number[i]))
                {
                    letterFound = true;
                    maxAllowedCharsToRemove++;
                    i--;
                }

                if (letterFound)
                {
                    numCharsToRemove = newSuffix.Length <= maxAllowedCharsToRemove ? newSuffix.Length : maxAllowedCharsToRemove;
                }
            }

            string newNumber = "";

            if (numCharsToRemove > 0)
            {
                // Remove the previous suffix from the building number
                newNumber = number.Substring(0, number.Length - numCharsToRemove) + newSuffix;
            }
            else
            {
                // Invalid condition; perform default replacement
                newNumber = number.Substring(0, number.Length - newSuffix.Length) + newSuffix; ;
            }

            return newNumber.Trim();
        }

        private bool IsCrossRoadToken(string token)
        {
            return token.StartsWith(Resources.CrossRoadPrefix + " ");
        }

        private bool IsStreetToken(ref string token)
        {
            // We do not process cross roads
            if (token.Contains('\\') || token.Contains('/'))
            {
                return false;
            }

            // Try some known street names
            if (CorrectKnownStreetMisspellings(ref token))
            {
                return true;
            }

            bool isStreet = false;

            // Convert different street types to 1NF standard
            isStreet |= CheckStringTokenPrefix(ref token, GUKV.DataMigration.Properties.Settings.Default.StreetAvenuePrefixList,
                Resources.StreetKindAvenueCorrect);

            isStreet |= CheckStringTokenPrefix(ref token, GUKV.DataMigration.Properties.Settings.Default.StreetBoulevardPrefixList,
                Resources.StreetKindBoulevardCorrect);

            isStreet |= CheckStringTokenPrefix(ref token, GUKV.DataMigration.Properties.Settings.Default.StreetLanePrefixList,
                Resources.StreetKindLaneCorrect);

            isStreet |= CheckStringTokenPrefix(ref token, GUKV.DataMigration.Properties.Settings.Default.StreetSquarePrefixList,
                Resources.StreetKindSquareCorrect);

            // Remove unnecessary street suffixes
            if (token.EndsWith(" " + Resources.StreetSuffix1))
            {
                token = token.Substring(0, token.Length - Resources.StreetSuffix1.Length);
                isStreet = true;
            }

            if (token.EndsWith(" " + Resources.StreetSuffix2))
            {
                token = token.Substring(0, token.Length - Resources.StreetSuffix2.Length);
                isStreet = true;
            }

            if (token.StartsWith(Resources.StreetSuffix1))
            {
                token = token.Substring(Resources.StreetSuffix1.Length);
                isStreet = true;
            }

            token = token.Trim();

            while (token.IndexOf("  ") >= 0)
            {
                token = token.Replace("  ", " ");
            }

            return isStreet;
        }

        private bool CheckStringTokenPrefix(ref string token, System.Collections.Specialized.StringCollection prefixes,
            string correctPrefix)
        {
            foreach (string prefix in prefixes)
            {
                if (token.StartsWith(prefix))
                {
                    token = token.Substring(prefix.Length) + " " + correctPrefix;
                    return true;
                }

                if (token.EndsWith(" " + prefix))
                {
                    token = token.Substring(0, token.Length - prefix.Length) + " " + correctPrefix;
                    return true;
                }
            }

            return false;
        }

        private bool IsValidNumberToken(string token)
        {
            string t = token.Trim();

            // If there are digits at the beginning of the "number" token, assume that this is a valid number
            foreach (char ch in token)
            {
                if (char.IsDigit(ch))
                {
                    return true;
                }
                else if (char.IsLetter(ch))
                {
                    return false;
                }
            }

            return false;
        }

        private bool IsLiteraToken(string token)
        {
            return
                token.StartsWith(Resources.BuildingLitPrefix) ||
                token.StartsWith(Resources.BuildingLitPrefix2) ||
                token.StartsWith(Resources.BuildingLitPrefix3) ||
                token.StartsWith(Resources.BuildingLitPrefix4) ||
                token.StartsWith(Resources.BuildingLitPrefix5) ||
                token.StartsWith(Resources.BuildingLitPrefix6) ||
                token.StartsWith(Resources.BuildingLitPrefix7) ||
                token.StartsWith(Resources.BuildingLitPrefix8);
        }

        #endregion (Inprecise address lookup)
    }
}
