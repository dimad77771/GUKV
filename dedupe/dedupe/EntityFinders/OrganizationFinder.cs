using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using GUKV.DataMigration;

namespace GUKV.ImportToolUtils
{
    /// <summary>
    /// This helper class is used to match organizations from various databases
    /// (Balans, Rosporadjennia, etc) to the 1 NF organizations.
    /// </summary>
    public class OrganizationFinder
    {
        #region Member variables

        /// <summary>
        /// This map is a cache for fast organization ID lookup by its ZKPO code
        /// </summary>
        private Dictionary<string, int> mapZkpoToId = new Dictionary<string, int>();

        /// <summary>
        /// Mapping of organizations by the short name
        /// </summary>
        private Dictionary<string, object> mapOrgByShortName = new Dictionary<string, object>();

        /// <summary>
        /// Mapping of organizations by the full name
        /// </summary>
        private Dictionary<string, object> mapOrgByFullName = new Dictionary<string, object>();

        /// <summary>
        /// Organizations in 1 NF that could not be categorized
        /// </summary>
        private List<OrganizationInfo> uncategorizedOrganizations = new List<OrganizationInfo>();

        /// <summary>
        /// Full cache of all 1NF organizations (by organization ID)
        /// </summary>
        private Dictionary<int, OrganizationInfo> mapOrgProperties = new Dictionary<int, OrganizationInfo>();

        /// <summary>
        /// Contains ZKPO codes whichare not unique in the 1NF database
        /// </summary>
        private Dictionary<string, List<int>> duplicateZkpo = new Dictionary<string, List<int>>();

        /// <summary>
        /// Contains manual overrides for organization lookup by ZKPO code
        /// </summary>
        private Dictionary<string, int> zkpoOverride = new Dictionary<string, int>();

        /// <summary>
        /// Contains manual overrides for organization lookup by ZKPO code and organization name
        /// </summary>
        private Dictionary<string, int> zkpoOverrideWithName = new Dictionary<string, int>();

        /// <summary>
        /// All organizations that could not be categorized are written to this log
        /// </summary>        

        #endregion (Member variables)

        #region Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrganizationFinder()
        {
        }

        #endregion (Construction)

        #region Interface

        /// <summary>
        /// Reads the manual overrides for organization matching from SQL Server database
        /// </summary>
        /// <param name="connection">Connection to SQL Server</param>
        public void ReadManualZKPOMatch(SqlConnection connection)
        {
            string query = "SELECT zkpo, full_name, org_id_1nf FROM Import1NF.dbo.manual_zkpo_match";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(2))
                        {
                            string zkpo = reader.GetString(0).Trim();
                            string orgFullName = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim().ToUpper();
                            int orgId1NF = reader.GetInt32(2);

                            // Zkpo must be padded to 10 digits
                            while (zkpo.Length < 10)
                            {
                                zkpo = "0" + zkpo;
                            }

                            if (orgFullName.Length == 0)
                            {
                                zkpoOverride[zkpo] = orgId1NF;
                            }
                            else
                            {
                                zkpoOverrideWithName[zkpo + "_" + orgFullName] = orgId1NF;
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Adds the specified organization to the cache of OrganizationFinder
        /// </summary>
        /// <param name="organizationId">Organization Id</param>
        /// <param name="zkpo">ZKPO code</param>
        /// <param name="fullName">Full name of the organization</param>
        /// <param name="shortName">Short name of the organization</param>
        public void AddOrganizationToCache(int organizationId, string zkpo, string fullName, string shortName,
            object industryCode, object subIndustryCode, object ownershipCode)
        {
            zkpo = zkpo.Trim();
            fullName = fullName.Trim().ToUpper();
            shortName = shortName.Trim().ToUpper();

            // Zkpo must be padded to 10 digits
            if (IsValidZKPO(zkpo))
            {
                while (zkpo.Length < 10)
                {
                    zkpo = "0" + zkpo;
                }

                // Check if ZKPO is unique
                int anotherOrg = 0;

                if (mapZkpoToId.TryGetValue(zkpo, out anotherOrg))
                {
                    AddDuplicateZKPO(zkpo, anotherOrg);
                    AddDuplicateZKPO(zkpo, organizationId);
                }

                // Create a cache by ZKPO code
                mapZkpoToId[zkpo] = organizationId;
            }

            OrganizationInfo info = CreateOrgInfo(organizationId, zkpo, fullName, shortName);

            info.industryCode = industryCode;
            info.subIndustryCode = subIndustryCode;
            info.ownershipCode = ownershipCode;

            mapOrgProperties[organizationId] = info;

            if (info.fullNameStripped.Length > 0)
            {
                AddOrgCacheEntry(info.fullNameStripped, info, mapOrgByFullName);
            }

            if (info.shortNameStripped.Length > 0)
            {
                AddOrgCacheEntry(info.shortNameStripped, info, mapOrgByShortName);
            }

            // If organization is not categorized, write it to the log
            if (info.category == OrgCategory.Undefined)
            {
                uncategorizedOrganizations.Add(info);
            }
        }

        /// <summary>
        /// Verifies if the specified ZKPO code is valid
        /// </summary>
        /// <param name="zkpoCode">ZKPO code</param>
        /// <returns>TRUE if this ZKPO code contains some non-zero digits</returns>
        public bool IsValidZKPO(string zkpoCode)
        {
            // To be valid, ZKPO code must contain some non-zero digits
            foreach (char ch in zkpoCode)
            {
                if (char.IsDigit(ch) && ch != '0')
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verifies if given ZKPO code is unique in 1NF
        /// </summary>
        /// <param name="zkpoCode">ZKPO code</param>
        /// <returns>TRUE if ZKPO code is unique</returns>
        public bool IsUniqueZKPO(string zkpoCode)
        {
            zkpoCode = zkpoCode.Trim();

            if (IsValidZKPO(zkpoCode))
            {
                // Zkpo must be padded to 10 digits
                while (zkpoCode.Length < 10)
                {
                    zkpoCode = "0" + zkpoCode;
                }

                return !duplicateZkpo.ContainsKey(zkpoCode);
            }

            return true;
        }

        /// <summary>
        /// Returns ID of organization in 1 NF by the ZKPO code. If ZKPO code is
        /// not unique in 1NF, this function will try to use the manual match table,
        /// and return -1 if there is no manual match.
        /// </summary>
        /// <param name="zkpoCode">ZKPO code</param>
        /// <param name="orgFullName">Full name of the organization</param>
        /// <param name="allowDuplicateZkpoCodes">This parameter defines behavior of the function
        /// when the specified ZKPO code is not unique. If no precise match by ZKPO code is possible,
        /// and allowDuplicateZkpoCodes is FALSE, this function will return -1. If no precise match
        /// by ZKPO code is possible, and allowDuplicateZkpoCodes is TRUE, this function will return
        /// ID of any matching organization.</param>
        /// <param name="forceCreate">If this parameter is set to TRUE upon exit,
        /// the caller should create a new organization</param>
        /// <returns>Organization ID, or -1 if organization is not found</returns>
        public int GetOrgIdByZKPO(string zkpoCode, string orgFullName, bool allowDuplicateZkpoCodes, out bool forceCreate)
        {
            zkpoCode = zkpoCode.Trim();
            orgFullName = orgFullName.Trim().ToUpper();
            int organizationId = -1;

            forceCreate = false;

            if (IsValidZKPO(zkpoCode))
            {
                // Zkpo must be padded to 10 digits
                while (zkpoCode.Length < 10)
                {
                    zkpoCode = "0" + zkpoCode;
                }

                // Special processing for ZKPO codes which are not unique
                List<int> organizations = null;

                if (duplicateZkpo.TryGetValue(zkpoCode, out organizations))
                {
                    // Try to find the organization by full name
                    foreach (int orgId in organizations)
                    {
                        OrganizationInfo info = GetOrgInfoById(orgId);

                        if (info != null && info.fullName.Trim().ToUpper() == orgFullName)
                        {
                            return orgId;
                        }
                    }

                    // Try to find a manual override
                    if (orgFullName.Length > 0)
                    {
                        string key = zkpoCode + "_" + orgFullName;

                        if (zkpoOverrideWithName.TryGetValue(key, out organizationId))
                        {
                            // Manual match can be set to -1. It indicates that organization is not known
                            if (organizationId < 0)
                            {
                                forceCreate = true;
                            }

                            return organizationId;
                        }
                    }

                    if (zkpoOverride.TryGetValue(zkpoCode, out organizationId))
                    {
                        // Manual match can be set to -1. It indicates that organization is not known
                        if (organizationId < 0)
                        {
                            forceCreate = true;
                        }

                        return organizationId;
                    }

                    if (allowDuplicateZkpoCodes)
                    {
                        // Just return any of the organizations
                        if (organizations.Count > 0)
                            return organizations[0];
                    }
                }
                else
                {
                    if (mapZkpoToId.TryGetValue(zkpoCode, out organizationId))
                    {
                        return organizationId;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns full information about organization by its ID
        /// </summary>
        /// <param name="organizationId">Organization ID</param>
        /// <returns>Information about this organization, or NULL if the given ID is not found</returns>
        public OrganizationInfo GetOrgInfoById(int organizationId)
        {
            OrganizationInfo info = null;

            if (mapOrgProperties.TryGetValue(organizationId, out info))
            {
                return info;
            }

            return null;
        }

        /// <summary>
        /// Returns full name of the organization by its ID
        /// </summary>
        /// <param name="organizationId">Organization ID</param>
        /// <returns>Full name of the organization</returns>
        public string GetOrgFullNameById(int organizationId)
        {
            OrganizationInfo info = null;

            if (mapOrgProperties.TryGetValue(organizationId, out info))
            {
                return info.fullName;
            }

            return "";
        }

        /// <summary>
        /// Tries to find an organization in 1NF that matches the specified parameters
        /// </summary>
        /// <param name="zkpo">ZKPO code</param>
        /// <param name="fullName">Full name of the organization</param>
        /// <param name="shortName">Short name of the organization</param>
        /// <param name="allowDuplicateZkpoCodes">This parameter defines behavior of the function
        /// when the specified ZKPO code is not unique. If no precise match by ZKPO code is possible,
        /// and allowDuplicateZkpoCodes is FALSE, this function will return -1. If no precise match
        /// by ZKPO code is possible, and allowDuplicateZkpoCodes is TRUE, this function will return
        /// ID of any matching organization.</param>
        /// <param name="categorized">Indicates that category could be deduced for this organization</param>
        /// <returns>Organization ID in 1NF, or -1 if organization was not found</returns>
        public int FindOrganization(string zkpo, string fullName, string shortName, bool allowDuplicateZkpoCodes, out bool categorized)
        {
            categorized = false;

            zkpo = zkpo.Trim();
            fullName = fullName.Trim().ToUpper();
            shortName = shortName.Trim().ToUpper();

            ///////////////////////////////////////////////////////////////////
            // 1) Find by ZKPO code - this is the most reliable search
            ///////////////////////////////////////////////////////////////////

            if (IsValidZKPO(zkpo))
            {
                // Zkpo must be padded to 10 digits
                while (zkpo.Length < 10)
                {
                    zkpo = "0" + zkpo;
                }

                bool forceCreate = false;
                int organizationId = GetOrgIdByZKPO(zkpo, fullName, allowDuplicateZkpoCodes, out forceCreate);

                if (organizationId > 0 || forceCreate)
                {
                    return organizationId;
                }
            }

            ///////////////////////////////////////////////////////////////////
            // 2) Find by name and category
            ///////////////////////////////////////////////////////////////////

            OrganizationInfo info = CreateOrgInfo(0, zkpo, fullName, shortName);

            if (info.category != OrgCategory.Undefined)
            {
                categorized = true;
            }

            List<OrganizationInfo> organizations = new List<OrganizationInfo>();

            FindOrganizationsInCache(info.fullNameStripped, mapOrgByFullName, organizations);
            FindOrganizationsInCache(info.shortNameStripped, mapOrgByShortName, organizations);

            if (organizations.Count == 1)
            {
                // Exactly one match found
                return organizations[0].organizationId1NF;
            }
            else if (organizations.Count > 1 && info.category != OrgCategory.Undefined)
            {
                // Try to find a match by category
                int numOrgWithTheSameCategory = 0;
                OrganizationInfo orgWithTheSameCategory = null;

                foreach (OrganizationInfo org in organizations)
                {
                    if (org.category == info.category)
                    {
                        orgWithTheSameCategory = org;
                        numOrgWithTheSameCategory++;
                    }
                }

                if (numOrgWithTheSameCategory == 1 && orgWithTheSameCategory != null)
                {
                    return orgWithTheSameCategory.organizationId1NF;
                }
            }

            ///////////////////////////////////////////////////////////////////
            // 3) Find by name only
            ///////////////////////////////////////////////////////////////////

            // Disabled so far: not reliable enough
            /*
            if (organizations.Count > 0)
            {
                int orgId = FindOrganizationByName(organizations, fullName, zkpo, false);

                if (orgId <= 0)
                {
                    orgId = FindOrganizationByName(organizations, shortName, zkpo, true);
                }

                return orgId;
            }
            */

            return -1;
        }

        /// <summary>
        /// Generates the list oforganizations with duplicate ZKPO codes, and stores
        /// this report to SQL Server
        /// </summary>
        /// <param name="connection">Connection to SQL Server</param>
        /// <param name="tableName">The table to store report to</param>
        public void GenerateDuplicateZKPOReport(SqlConnection connection, string tableName)
        {
            using (SqlCommand cmdClear = new SqlCommand("DELETE FROM " + tableName, connection))
            {
                cmdClear.ExecuteNonQuery();
            }

            foreach (KeyValuePair<string, List<int>> pair in duplicateZkpo)
            {
                string zkpo = pair.Key;

                foreach (int orgId in pair.Value)
                {
                    using (SqlCommand cmdInsert = new SqlCommand("INSERT INTO " + tableName + " (zkpo, org_id) VALUES (@p1, @p2)", connection))
                    {
                        cmdInsert.Parameters.Add(new SqlParameter("p1", zkpo));
                        cmdInsert.Parameters.Add(new SqlParameter("p2", orgId));

                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Returns a valid unused organization ID in 1NF
        /// </summary>
        /// <param name="connection1NF">Connection to 1NF database</param>
        /// <returns>Generated organization ID</returns>
        public int Get1NFOrganizationIdSeed(FbConnection connection1NF)
        {
            int seed = -1;

            string query = "select max(kod_obj) from sorg_1nf where kod_obj < 130000";

            using (FbCommand command = new FbCommand(query, connection1NF))
            {
                using (FbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        object dataSeed = reader.IsDBNull(0) ? null : reader.GetValue(0);

                        if (dataSeed is int)
                        {
                            seed = (int)dataSeed;
                        }
                    }

                    reader.Close();
                }
            }

            return seed;
        }

        #endregion (Interface)

        #region Implementation

        private void AddDuplicateZKPO(string zkpo, int organizationId)
        {
            List<int> organizations = null;

            if (!duplicateZkpo.TryGetValue(zkpo, out organizations))
            {
                organizations = new List<int>();

                duplicateZkpo.Add(zkpo, organizations);
            }

            if (!organizations.Contains(organizationId))
            {
                organizations.Add(organizationId);
            }
        }

        private OrgCategory CategorizeOrganization(ref string orgName)
        {
            orgName = orgName.Trim();

            if (OrgNameMatchesCategory(ref orgName,  GUKV.DataMigration.Properties.Settings.Default.OrgNameZHBK))
            {
                return OrgCategory.ZHBK;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameZHEK))
            {
                // Special processing is required to extract the ZHEK number and use it as organization name
                ExtractZHEKNumber(ref orgName);

                return OrgCategory.ZHEK;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNamePobut))
            {
                return OrgCategory.Pobutova;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameUpr))
            {
                return OrgCategory.Upravlinnya;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameAssoc))
            {
                return OrgCategory.Association;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameBlago))
            {
                return OrgCategory.Blagodiyna;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameFOP))
            {
                return OrgCategory.FOP;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameTOV))
            {
                return OrgCategory.TOV;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNamePainters))
            {
                return OrgCategory.Painter;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameAdvBureau))
            {
                return OrgCategory.AdvocateBureau;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameOSBB))
            {
                return OrgCategory.OSBB;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameComBank))
            {
                return OrgCategory.Bank;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNamePAT))
            {
                return OrgCategory.PAT;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameZAT))
            {
                return OrgCategory.ZAT;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameAdvocate))
            {
                return OrgCategory.Advocate;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameGromad))
            {
                return OrgCategory.Gromadska;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameKP))
            {
                return OrgCategory.Komunalna;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameKoop))
            {
                return OrgCategory.Kooperativ;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameSP))
            {
                return OrgCategory.Spilna;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNameAT))
            {
                return OrgCategory.Akcionerna;
            }

            if (OrgNameMatchesCategory(ref orgName, GUKV.DataMigration.Properties.Settings.Default.OrgNamePP))
            {
                return OrgCategory.Privatna;
            }

            return OrgCategory.Undefined;
        }

        private string StripOrgName(string orgName)
        {
            string name = orgName.Trim();

            // If the name starts with a quotem or ends with a quote - maybe it is a quoted name
            if (name.StartsWith("\""))
            {
                int pos = name.IndexOf('\"', 1);

                if (pos > 0)
                {
                    name = name.Substring(1, pos - 1);
                }
            }
            else if (name.EndsWith("\""))
            {
                int pos = name.IndexOf('\"');

                if (pos < (name.Length - 1))
                {
                    name = name.Substring(pos + 1, name.Length - pos - 2);
                }
            }

            char[] prefixes = new char[] { '\'', '\"', '*' };

            int idx = name.IndexOfAny(prefixes);

            while (idx >= 0)
            {
                name = name.Remove(idx, 1);

                idx = name.IndexOfAny(prefixes);
            }

            return name.Trim();
        }

        private bool OrgNameMatchesCategory(ref string name, StringCollection prefixes)
        {
            foreach (string prefix in prefixes)
            {
                if (name.StartsWith(prefix))
                {
                    name = name.Remove(0, prefix.Length);

                    return true;
                }
            }

            return false;
        }

        private string PreprocessOrgName(string orgName)
        {
            string name = orgName;

            while (name.StartsWith("*"))
            {
                name = name.Substring(1);
            }

            while (name.EndsWith("*"))
            {
                name = name.Substring(0, name.Length - 1);
            }

            return name.Trim();
        }

        private void ExtractActualOrgName(ref string extractedName)
        {
            // Find positions of all quotes
            int firstQuotePos = -1;
            int lastQuotePos = -1;
            int numQuotes = 0;

            for (int i = 0; i < extractedName.Length; i++)
            {
                if (extractedName[i] == '\"')
                {
                    numQuotes++;

                    if (firstQuotePos < 0)
                    {
                        firstQuotePos = i;
                    }

                    lastQuotePos = i;
                }
            }

            if (numQuotes > 1)
            {
                // Leave only the part of name which is within quotes
                extractedName = extractedName.Substring(0, lastQuotePos);
                extractedName = extractedName.Remove(0, firstQuotePos + 1);
            }

            // Remove all special characters from the name
            char[] separators = new char[] { '\'', '\"', '*' };

            int idx = extractedName.IndexOfAny(separators);

            while (idx >= 0)
            {
                extractedName = extractedName.Remove(idx, 1);

                idx = extractedName.IndexOfAny(separators);
            }
        }

        private OrgCategory GetUnifiedCategory(OrgCategory category1, OrgCategory category2)
        {
            if (category1 == category2)
            {
                return category2;
            }

            if (category1 == OrgCategory.Undefined)
            {
                return category2;
            }

            if (category2 == OrgCategory.Undefined)
            {
                return category1;
            }

            // Categories are different; apply some logic
            if ((category1 == OrgCategory.FOP && category2 == OrgCategory.Privatna) ||
                (category2 == OrgCategory.FOP && category1 == OrgCategory.Privatna))
            {
                return OrgCategory.FOP;
            }

            if ((category1 == OrgCategory.Painter && category2 == OrgCategory.Privatna) ||
                (category2 == OrgCategory.Painter && category1 == OrgCategory.Privatna))
            {
                return OrgCategory.Painter;
            }

            if ((category1 == OrgCategory.Painter && category2 == OrgCategory.FOP) ||
                (category2 == OrgCategory.Painter && category1 == OrgCategory.FOP))
            {
                return OrgCategory.Painter;
            }

            if ((category1 == OrgCategory.AdvocateBureau && category2 == OrgCategory.Akcionerna) ||
                (category2 == OrgCategory.AdvocateBureau && category1 == OrgCategory.Akcionerna))
            {
                return OrgCategory.AdvocateBureau;
            }

            if ((category1 == OrgCategory.PAT && category2 == OrgCategory.TOV) ||
                (category2 == OrgCategory.PAT && category1 == OrgCategory.TOV))
            {
                return OrgCategory.PAT;
            }

            if ((category1 == OrgCategory.PAT && category2 == OrgCategory.Akcionerna) ||
                (category2 == OrgCategory.PAT && category1 == OrgCategory.Akcionerna))
            {
                return OrgCategory.PAT;
            }

            if ((category1 == OrgCategory.ZAT && category2 == OrgCategory.TOV) ||
                (category2 == OrgCategory.ZAT && category1 == OrgCategory.TOV))
            {
                return OrgCategory.ZAT;
            }

            if ((category1 == OrgCategory.ZAT && category2 == OrgCategory.Akcionerna) ||
                (category2 == OrgCategory.ZAT && category1 == OrgCategory.Akcionerna))
            {
                return OrgCategory.ZAT;
            }

            // The categories are too different; can not decide, what it is
            return OrgCategory.Undefined;
        }

        private OrganizationInfo CreateOrgInfo(int orgId, string zkpo, string fullName, string shortName)
        {
            fullName = PreprocessOrgName(fullName.Trim().ToUpper());
            shortName = PreprocessOrgName(shortName.Trim().ToUpper());

            OrganizationInfo info = new OrganizationInfo();

            OrgCategory categoryFromFullName = OrgCategory.Undefined;
            OrgCategory categoryFromShortName = OrgCategory.Undefined;

            // Set the default organization data
            info.organizationId1NF = orgId;
            info.zkpoCode = zkpo;
            info.fullName = fullName;
            info.shortName = shortName;

            // Process the full name
            if (fullName.Length > 0)
            {
                string extractedFullName = fullName;

                categoryFromFullName = CategorizeOrganization(ref extractedFullName);

                ExtractActualOrgName(ref extractedFullName);

                info.fullNameStripped = extractedFullName.Trim();
            }

            // Process the short name
            if (shortName.Length > 0)
            {
                string extractedShortName = shortName;

                categoryFromShortName = CategorizeOrganization(ref extractedShortName);

                ExtractActualOrgName(ref extractedShortName);

                info.shortNameStripped = extractedShortName.Trim();
            }

            // Try to produce a unified category
            info.category = GetUnifiedCategory(categoryFromFullName, categoryFromShortName);

            return info;
        }

        private void AddOrgCacheEntry(string orgName, OrganizationInfo info, Dictionary<string, object> cache)
        {
            object existingInfo = null;

            if (cache.TryGetValue(orgName, out existingInfo))
            {
                // It may be a list, or a single entry
                if (existingInfo is List<OrganizationInfo>)
                {
                    // Add new info to existing list
                    List<OrganizationInfo> list = existingInfo as List<OrganizationInfo>;

                    list.Add(info);
                }
                else
                {
                    // Create a new list
                    List<OrganizationInfo> list = new List<OrganizationInfo>();

                    list.Add(existingInfo as OrganizationInfo);
                    list.Add(info);

                    cache[orgName] = list;
                }
            }
            else
            {
                // No data; add new entry
                cache[orgName] = info;
            }
        }

        private bool IsOrganizationInList(List<OrganizationInfo> list, int organizationId)
        {
            foreach (OrganizationInfo info in list)
            {
                if (info.organizationId1NF == organizationId)
                {
                    return true;
                }
            }

            return false;
        }

        private void FindOrganizationsInCache(string orgName, Dictionary<string, object> cache,
            List<OrganizationInfo> results)
        {
            object data = null;

            if (cache.TryGetValue(orgName, out data))
            {
                if (data is OrganizationInfo)
                {
                    OrganizationInfo info = data as OrganizationInfo;

                    if (!IsOrganizationInList(results, info.organizationId1NF))
                    {
                        results.Add(info);
                    }
                }
                else if (data is List<OrganizationInfo>)
                {
                    List<OrganizationInfo> list = data as List<OrganizationInfo>;

                    foreach (OrganizationInfo info in list)
                    {
                        if (!IsOrganizationInList(results, info.organizationId1NF))
                        {
                            results.Add(info);
                        }
                    }
                }
            }
        }

        private void ExtractZHEKNumber(ref string orgName)
        {
            // Get all digits from the organization name
            StringBuilder strBuilder = new StringBuilder();

            bool digitsFound = false;

            for (int i = 0; i < orgName.Length; i++)
            {
                if (char.IsDigit(orgName[i]))
                {
                    digitsFound = true;

                    strBuilder.Append(orgName[i]);
                }
                else
                {
                    // If some number is already found - stop
                    if (digitsFound)
                    {
                        break;
                    }
                }
            }

            if (digitsFound)
            {
                orgName = strBuilder.ToString();
            }
        }

        private void BuildOrganizationKeywords(string orgName, List<string> keywords)
        {
            keywords.Clear();

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < orgName.Length; i++)
            {
                if (!char.IsLetterOrDigit(orgName[i]))
                {
                    string keyword = builder.ToString().Trim();

                    if (keyword.Length > 0)
                    {
                        keywords.Add(keyword.ToUpper());
                    }

                    builder.Clear();
                }
                else
                {
                    builder.Append(orgName[i]);
                }
            }

            // The last keyword might be still in the StringBuilder
            string remainder = builder.ToString().Trim();

            if (remainder.Length > 0)
            {
                keywords.Add(remainder.ToUpper());
            }
        }

        private int FindOrganizationByName(List<OrganizationInfo> searchList,
            string orgName, string zkpo, bool useShortName)
        {
            List<string> keywords = new List<string>();
            List<string> searchKeywords = new List<string>();

            BuildOrganizationKeywords(orgName, keywords);

            if (keywords.Count > 0)
            {
                List<OrganizationInfo> foundOrganizations = new List<OrganizationInfo>();
                int keywordsInMatch = 1; // No need to consider organization names without any match

                // Search for matches in the uncategorized organizations
                foreach (OrganizationInfo info in searchList)
                {
                    // If ZKPO is defined, skip organizations with different ZKPO
                    if (zkpo.Length > 0 && info.zkpoCode.Length > 0 && zkpo != info.zkpoCode)
                    {
                        continue;
                    }

                    // Split the name of organization
                    if (useShortName)
                    {
                        BuildOrganizationKeywords(info.shortName, searchKeywords);
                    }
                    else
                    {
                        BuildOrganizationKeywords(info.fullName, searchKeywords);
                    }

                    // Calculate how many keywords match
                    int keywordsFound = 0;

                    foreach (string keyword in keywords)
                    {
                        if (searchKeywords.Contains(keyword))
                        {
                            keywordsFound++;
                        }
                    }

                    // Check if this organization is a good match
                    if (keywordsFound >= keywordsInMatch)
                    {
                        if (keywordsFound > keywordsInMatch)
                        {
                            keywordsInMatch = keywordsFound;

                            // This is a better match; remove all prevous findings
                            foundOrganizations.Clear();
                        }

                        foundOrganizations.Add(info);
                    }
                }

                // If there is one exact match - return it
                if (foundOrganizations.Count == 1)
                {
                    return foundOrganizations[0].organizationId1NF;
                }
            }

            return -1;

        }

        #endregion (Implementation)
    }
}