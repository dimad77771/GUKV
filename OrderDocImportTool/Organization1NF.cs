using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
    public class Organization1NF
    {
        public int organizationId = -1;

        public string zkpo = "";
        public string fullName = "";
        public string shortName = "";

        public int industryId = -1;
        public int occupationId = -1;
        public int statusId = -1;
        public int orgTypeId = -1;
        public int ownershipFormId = -1;
        public int formGospId = -1;

        public int addrDistrictId = -1;
        public string addrStreet = "";
        public string addrNomer = "";
        public string addrKorpus = "";
        public string addrZipCode = "";
        
        public string directorFIO = "";
        public string directorTel = "";
        public string buhgalterFIO = "";
        public string buhgalterTel = "";
        public string fax = "";

        public Organization1NF()
        {
        }
    }
}
