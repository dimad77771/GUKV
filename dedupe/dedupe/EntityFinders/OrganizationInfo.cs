using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GUKV.ImportToolUtils
{
    #region Organization categories

    /// <summary>
    /// Lists al known types of organizations
    /// </summary>
    public enum OrgCategory
    {
        Undefined,
        Komunalna, // Komunalne pidpriemstvo
        TOV, // Tovaristvo z obmejenou vidpovidalnistu
        FOP, // Fizichna osoba - pidpriemec
        Privatna, // Privatne pidpriemstvo
        Akcionerna, // Akcionerne tovaristvo
        PAT, // Publichne akcionerne tovaristvo
        ZAT, // Zakrite akcionerne tovaristvo
        Gromadska, // Gromadska organizacia
        Bank, // Comerciyniy bank
        Advocate, // Advocate
        AdvocateBureau, // Advocate Bureau
        OSBB, // OSBB
        Kooperativ, // Kooperativ
        Spilna, // Spilne pedpriemstvo
        Painter, // Painter
        Blagodiyna, // Blagodiyna organizacia
        Association, // Associaciya
        Upravlinnya, // Upravlinnya
        ZHBK, // Zhitlovo-budivelniy kooperativ
        ZHEK, // Zhitlovo-ekspluataciyna kontora
        Pobutova // Budinok pobutu
    }

    #endregion (Organization categories)

    /// <summary>
    /// This data structure describes an organization from the '1NF' database.
    /// </summary>
    public class OrganizationInfo
    {
        /// <summary>
        /// Organization ID in 1NF
        /// </summary>
        public int organizationId1NF = 0;

        /// <summary>
        /// ZKPO code
        /// </summary>
        public string zkpoCode = "";

        /// <summary>
        /// Full name, as specified in the 1NF database
        /// </summary>
        public string fullName = "";

        /// <summary>
        /// Extracted full name
        /// </summary>
        public string fullNameStripped = "";

        /// <summary>
        /// Short name, as specified in the 1NF database
        /// </summary>
        public string shortName = "";

        /// <summary>
        /// Extracted short name
        /// </summary>
        public string shortNameStripped = "";

        /// <summary>
        /// Deduced organization category
        /// </summary>
        public OrgCategory category = OrgCategory.Undefined;

        /// <summary>
        /// Industry ID, as specified in 1NF database
        /// </summary>
        public object industryCode = null;

        /// <summary>
        /// Sub-industry ID, as specified in 1NF database
        /// </summary>
        public object subIndustryCode = null;

        /// <summary>
        /// Form of ownership ID, as specified in 1NF database
        /// </summary>
        public object ownershipCode = null;

        /// <summary>
        /// Indicates that organization is deleted in 1NF
        /// </summary>
        public bool deleted = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrganizationInfo()
        {
        }

        /// <summary>
        /// Performs matching of another OrganizationInfo object to this object
        /// </summary>
        /// <param name="info">Object to compare</param>
        /// <returns>TRUE if matching was successful</returns>
        public bool IsMatch(OrganizationInfo info)
        {
            // ZKPO codes must match
            if (zkpoCode.Length > 0 && info.zkpoCode.Length > 0 && zkpoCode == info.zkpoCode)
            {
                // If industry or ownership form is specified, they must match
                bool industryOK = (industryCode == null) || (info.industryCode == null) ||
                    (industryCode is int && info.industryCode is int && (int)industryCode == (int)info.industryCode);

                bool subIndustryOK = (subIndustryCode == null) || (info.subIndustryCode == null) ||
                    (subIndustryCode is int && info.subIndustryCode is int && (int)subIndustryCode == (int)info.subIndustryCode);

                bool ownershipOK = (ownershipCode == null) || (info.ownershipCode == null) ||
                    (ownershipCode is int && info.ownershipCode is int && (int)ownershipCode == (int)info.ownershipCode);

                return industryOK && subIndustryOK && ownershipOK;
            }

            return false;
        }
    }
}