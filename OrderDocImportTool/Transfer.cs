using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
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
