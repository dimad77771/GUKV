using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
    public class Object1NF
    {
        public int objectId = -1;

        public string streetName = "";

        public int streetId = -1;

        public string addrNomer1 = "";

        public string addrNomer2 = "";

        public string addrNomer3 = "";

        public string addrMisc = "";

        public object techStateId = null;

        public object buildYear = null;

        public object districtId = null;

        public object objTypeId = null;

        public object objKindId = null;

        public object totalSqr = null;

        public string objectName = "";

        public Object1NF()
        {
        }

        public override string ToString()
        {
            string street = streetName.Trim();

            if (street.Length == 0 && streetId > 0)
            {
                street = DB.FindNameInDictionary1NF(DB.DICT_STREETS, streetId);
            }

            string address = FormatBuildingNumber();

            if (street.Length > 0 && address.Length > 0)
            {
                return street + ", " + address;
            }

            return address.Length > 0 ? address : street;
        }

        public string FormatBuildingNumber()
        {
            string address = addrNomer1.Trim();

            if (addrNomer2.Length > 0)
            {
                if (address.Length > 0)
                    address += " ";

                address += addrNomer2.Trim();
            }

            if (addrNomer3.Length > 0)
            {
                if (address.Length > 0)
                    address += " ";

                address += addrNomer3.Trim();
            }

            if (addrMisc.Length > 0)
            {
                if (address.Length > 0)
                    address += " ";

                address += addrMisc.Trim();
            }

            return address;
        }

        public virtual Object1NF Clone()
        {
            Object1NF clone = new Object1NF();

            clone.addrMisc = addrMisc;
            clone.addrNomer1 = addrNomer1;
            clone.addrNomer2 = addrNomer2;
            clone.addrNomer3 = addrNomer3;
            clone.buildYear = buildYear;
            clone.districtId = districtId;
            clone.objectId = objectId;
            clone.objectName = objectName;
            clone.objKindId = objKindId;
            clone.objTypeId = objTypeId;
            clone.streetId = streetId;
            clone.streetName = streetName;
            clone.techStateId = techStateId;
            clone.totalSqr = totalSqr;

            return clone;
        }
    }

    public class Object1NFAlphabeticalComparer : IComparer<Object1NF>
    {
        public Object1NFAlphabeticalComparer()
        {
        }

        int IComparer<Object1NF>.Compare(Object1NF x, Object1NF y)
        {
            string str1 = x.ToString();
            string str2 = y.ToString();

            return str1.CompareTo(str2);
        }
    }
}
