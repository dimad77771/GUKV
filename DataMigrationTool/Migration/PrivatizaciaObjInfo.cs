using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
    /// <summary>
    /// This data structure describes an object (building) from the 'Privatizacia' database.
    /// The member variables of this structure duplicate fields of the MOBJ table in the
    /// 'Privatizacia' database. This table is read into memory only once, and stored in the
    /// privatizaciaObjects variable of the Migrator class.
    /// </summary>
    public class PrivatizaciaObjInfo
    {
        /// <summary>
        /// Object ID in the 'Privatizacia' database
        /// </summary>
        public int objectId = 0;

        /// <summary>
        /// ID of the same object in the 1 NF database
        /// </summary>
        public object objectId1NF = null;

        /// <summary>
        /// Address: street name
        /// </summary>
        public object addrStreetName = null;

        /// <summary>
        /// Address: building number
        /// </summary>
        public object addrNomer = null;

        /// <summary>
        /// Address: building number suffix - 'korpus 1', for instance
        /// </summary>
        public object addrMiscInfo = null;

        /// <summary>
        /// Address: postal code
        /// </summary>
        public object addrZipCode = null;

        /// <summary>
        /// District code from the 'dict_districts2' table. This dictionary is the same
        /// in 1 NF database and 'Privatizacia' database
        /// </summary>
        public object districtId = null;

        /// <summary>
        /// Historical heritage code from the 'dict_history' table. This dictionary is the same
        /// in 1 NF database and 'Privatizacia' database
        /// </summary>
        public object historyId = null;

        /// <summary>
        /// Name of the user who created or modified this object in the 'Privatizacia' database
        /// </summary>
        public object modifiedBy = null;

        /// <summary>
        /// Date and time of the last object modification in the 'Privatizacia' database
        /// </summary>
        public object modifyDate = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PrivatizaciaObjInfo()
        {
        }
    }
}
