using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.ImportToolUtils
{
    public enum ObjectTransferType
    {
        Transfer,
        Create,
        Destroy
    }

    [Serializable()]
    public class BalansTransfer
    {
        public ObjectTransferType transferType = ObjectTransferType.Transfer;

        public int objectId_1NF = -1;
        public int objectId_NJF = -1;

        public int organizationFromId_1NF = -1;
        public int organizationFromId_NJF = -1;

        public int organizationToId_1NF = -1;
        public int organizationToId_NJF = -1;

        public int balansId_1NF = -1;

        public decimal sqr = 0m;

        public string orgFromZkpo_NJF = "";
        public string orgFromFullName_NJF = "";
        public string orgFromShortName_NJF = "";

        public string orgToZkpo_NJF = "";
        public string orgToFullName_NJF = "";
        public string orgToShortName_NJF = "";

        public string orgFromZkpo_1NF = "";
        public string orgFromFullName_1NF = "";
        public string orgFromShortName_1NF = "";

        public string orgToZkpo_1NF = "";
        public string orgToFullName_1NF = "";
        public string orgToShortName_1NF = "";

        public BalansTransfer()
        {
        }

        public Object1NF object1NF
        {
            get
            {
                Object1NF obj = null;

                if (DB.objects1NF.TryGetValue(objectId_1NF, out obj))
                {
                    return obj;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    objectId_1NF = value.objectId;
                }
                else
                {
                    objectId_1NF = -1;
                }
            }
        }

        public Organization1NF orgFrom1NF
        {
            get
            {
                Organization1NF org = null;

                if (DB.organizations1NF.TryGetValue(organizationFromId_1NF, out org))
                {
                    return org;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    organizationFromId_1NF = value.organizationId;
                    orgFromZkpo_1NF = value.zkpo;
                    orgFromFullName_1NF = value.fullName;
                    orgFromShortName_1NF = value.shortName;
                }
                else
                {
                    organizationFromId_1NF = -1;
                    orgFromZkpo_1NF = "";
                    orgFromFullName_1NF = "";
                    orgFromShortName_1NF = "";
                }
            }
        }

        public Organization1NF orgTo1NF
        {
            get
            {
                Organization1NF org = null;

                if (DB.organizations1NF.TryGetValue(organizationToId_1NF, out org))
                {
                    return org;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    organizationToId_1NF = value.organizationId;
                    orgToZkpo_1NF = value.zkpo;
                    orgToFullName_1NF = value.fullName;
                    orgToShortName_1NF = value.shortName;
                }
                else
                {
                    organizationToId_1NF = -1;
                    orgToZkpo_1NF = "";
                    orgToFullName_1NF = "";
                    orgToShortName_1NF = "";
                }
            }
        }

        public BalansObject1NF balansObject1NF
        {
            get
            {
                BalansObject1NF obj = null;

                if (DB.balans1NFByID.TryGetValue(balansId_1NF, out obj))
                {
                    return obj;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    balansId_1NF = value.balansId;

                    // Assign the transferred square as well
                    if (value.sqr > 0m && sqr == 0m)
                    {
                        sqr = value.sqr;
                    }

                    // Assign the owner organization as well
                    if (value.organizationId > 0)
                    {
                        Organization1NF org = null;

                        if (DB.organizations1NF.TryGetValue(value.organizationId, out org))
                        {
                            this.orgFrom1NF = org;
                        }
                    }
                }
                else
                {
                    balansId_1NF = -1;
                }
            }
        }

        public void FindBalansObjectIn1NF()
        {
            balansId_1NF = -1;

            if (objectId_1NF > 0 && organizationFromId_1NF > 0 && sqr > 0m)
            {
                Dictionary<int, BalansObject1NF> storage = null;

                if (DB.balans1NFByAddress.TryGetValue(objectId_1NF, out storage))
                {
                    // Select all balans objects that belong to the 'FROM' organization
                    List<BalansObject1NF> objectsEqualSquare = new List<BalansObject1NF>();
                    List<BalansObject1NF> objectsGreaterSquare = new List<BalansObject1NF>();

                    foreach (KeyValuePair<int, BalansObject1NF> pair in storage)
                    {
                        if (pair.Value.organizationId == organizationFromId_1NF && pair.Value.sqr > 0m)
                        {
                            // Check if square of both objects can be considered identical
                            decimal diff = Math.Abs(pair.Value.sqr - sqr);

                            if ((diff / sqr < 0.03m) && (diff / pair.Value.sqr < 0.03m))
                            {
                                objectsEqualSquare.Add(pair.Value);
                            }
                            else if (pair.Value.sqr > sqr)
                            {
                                objectsGreaterSquare.Add(pair.Value);
                            }
                        }
                    }

                    // If there is one object with the same square - great
                    if (objectsEqualSquare.Count == 1)
                    {
                        balansId_1NF = objectsEqualSquare[0].balansId;
                    }
                    else if (objectsEqualSquare.Count == 0 && objectsGreaterSquare.Count == 1)
                    {
                        balansId_1NF = objectsGreaterSquare[0].balansId;
                    }
                }
            }
        }

        public BalansTransfer MakeCopy()
        {
            BalansTransfer other = new BalansTransfer();

            other.objectId_1NF = objectId_1NF;
            other.objectId_NJF = objectId_NJF;
            other.organizationFromId_1NF = organizationFromId_1NF;
            other.organizationFromId_NJF = organizationFromId_NJF;
            other.organizationToId_1NF = organizationToId_1NF;
            other.organizationToId_NJF = organizationToId_NJF;
            other.balansId_1NF = balansId_1NF;

            other.sqr = sqr;

            other.orgFromZkpo_NJF = orgFromZkpo_NJF;
            other.orgFromFullName_NJF = orgFromFullName_NJF;
            other.orgFromShortName_NJF = orgFromShortName_NJF;

            other.orgToZkpo_NJF = orgToZkpo_NJF;
            other.orgToFullName_NJF = orgToFullName_NJF;
            other.orgToShortName_NJF = orgToShortName_NJF;

            other.orgFromZkpo_1NF = orgFromZkpo_1NF;
            other.orgFromFullName_1NF = orgFromFullName_1NF;
            other.orgFromShortName_1NF = orgFromShortName_1NF;

            other.orgToZkpo_1NF = orgToZkpo_1NF;
            other.orgToFullName_1NF = orgToFullName_1NF;
            other.orgToShortName_1NF = orgToShortName_1NF;

            other.transferType = transferType;

            return other;
        }

        public bool IsFullyDefined()
        {
            switch (transferType)
            {
                case ObjectTransferType.Transfer:
                    return (objectId_1NF > 0) && (sqr > 0m) && (organizationFromId_1NF > 0) && (organizationToId_1NF > 0) && (balansId_1NF > 0);

                case ObjectTransferType.Create:
                    return (objectId_1NF > 0) && (sqr > 0m) && (organizationToId_1NF > 0);

                case ObjectTransferType.Destroy:
                    return (objectId_1NF > 0) && (organizationFromId_1NF > 0) && (balansId_1NF > 0);
            }

            return false;
        }
    }
}
