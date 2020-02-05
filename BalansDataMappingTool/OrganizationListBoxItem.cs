using System;
using System.Text;

namespace GUKV.BalansDataMappingTool
{
    /// <summary>
    /// This class holds information about particular organization of '1 NF' or 'Balans' database.
    /// </summary>
    public class OrganizationListBoxItem
    {
        /// <summary>
        /// Organization Id
        /// </summary>
        public int organizationId = 0;

        /// <summary>
        /// Organization name
        /// </summary>
        public string organizationName = "";

        /// <summary>
        /// Creates a new object
        /// </summary>
        /// <param name="id">Organization Id</param>
        /// <param name="name">Organization name</param>
        public OrganizationListBoxItem(int id, string name)
        {
            organizationId = id;
            organizationName = name;
        }

        /// <summary>
        /// This method is overridden to allow ListBox to display OrganizationListBoxItem items
        /// </summary>
        /// <returns>String presentation of the OrganizationListBoxItem object</returns>
        public override string ToString()
        {
            return organizationName;
        }
    }
}
