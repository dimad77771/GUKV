using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.ImportToolUtils
{
    [Serializable()]
    public class AppendixObject
    {
        public int object1NFId = -1;

        public Dictionary<ColumnCategory, string> properties = new Dictionary<ColumnCategory, string>();

        public List<Transfer> transfers = new List<Transfer>();

        public AppendixObject()
        {
        }

        public bool IsEmpty
        {
            get
            {
                foreach (KeyValuePair<ColumnCategory, string> prop in properties)
                {
                    if (prop.Value.Length > 0)
                    {
                        return false;
                    }
                }

                if (transfers.Count > 0)
                {
                    return false;
                }

                return true;
            }
        }

        public Object1NF object1NF
        {
            get
            {
                Object1NF obj = null;

                if (DB.objects1NF.TryGetValue(object1NFId, out obj))
                {
                    return obj;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    object1NFId = value.objectId;
                }
                else
                {
                    object1NFId = -1;
                }
            }
        }

        public AppendixObject MakeCopy()
        {
            AppendixObject newObj = new AppendixObject();

            newObj.object1NF = object1NF;

            foreach (KeyValuePair<ColumnCategory, string> pair in properties)
            {
                newObj.properties.Add(pair.Key, pair.Value);
            }

            foreach (Transfer t in transfers)
            {
                newObj.transfers.Add(t.MakeCopy());
            }

            return newObj;
        }

        public string FormatTransfers()
        {
            string rights = "";

            foreach (Transfer t in transfers)
            {
                string rightName = DB.FindNameInDictionaryNJF(DB.DICT_RIGHTS, t.rightId);

                string transferStr = string.Format("{0} {1}\n{2} {3}\n{4} {5}",
                    GUKV.ImportToolUtils.AppResources.TransferTooltipFormatFrom, t.orgNameFrom,
                    GUKV.ImportToolUtils.AppResources.TransferTooltipFormatTo, t.orgNameTo,
                    GUKV.ImportToolUtils.AppResources.TransferTooltipFormatRight, rightName);

                if (rights.Length > 0)
                {
                    rights += "\n\n";
                }

                rights += transferStr;
            }

            return rights;
        }

        public bool SimilarTransferExists(Transfer transfer)
        {
            foreach (Transfer t in transfers)
            {
                if (t.Equals(transfer))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
