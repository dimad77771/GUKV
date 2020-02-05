using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
    public class BalansObject1NF
    {
        public int balansId = -1;

        public int organizationId = -1;

        public int objectId = -1;

        public decimal sqr = 0m;

        public int purposeId = -1;

        public int purposeGroupId = -1;

        public string purpose = "";

        public BalansObject1NF()
        {
        }

        public override string ToString()
        {
            string description = purpose;

            if (description.Length == 0)
            {
                description = DB.FindNameInDictionary1NF(DB.DICT_PURPOSE, purposeId);
            }

            if (description.Length == 0)
            {
                description = DB.FindNameInDictionary1NF(DB.DICT_PURPOSE_GROUP, purposeGroupId);
            }

            if (description.Length == 0)
            {
                description = "НЕЖИЛЕ ПРИМІЩЕННЯ";
            }

            if (sqr > 0m)
            {
                description += " площею " + sqr.ToString("F2") + " кв.м.";
            }

            Object1NF obj = null;

            if (DB.objects1NF.TryGetValue(objectId, out obj))
            {
                description += " за адресою " + obj.ToString();
            }

            Organization1NF org = null;

            if (DB.organizations1NF.TryGetValue(organizationId, out org))
            {
                description += " на балансі " + org.fullName;
            }

            return description;
        }
    }
}
