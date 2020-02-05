using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using GUKV.ImportToolUtils.Properties;

namespace GUKV.ImportToolUtils
{
    [Serializable()]
    public class ActObject
    {
        public int objectId_NJF = -1;
        public int objectDocsPropertiesId_NJF = -1;

        public string addrStreet_NJF = "";
        public string addrNomer1_NJF = "";
        public string addrNomer2_NJF = "";
        public string addrNomer3_NJF = "";
        public string addrMisc_NJF = "";
        public object districtId_NJF = null;

        public string objectName_NJF = "";
        public object objectSquare_NJF = null;
        public object objectLen_NJF = null;
        public object objectDiamTrub_NJF = null;
        public object objectBalansCost_NJF = null;
        public object objectFinalCost_NJF = null;

        public int objectFloorsInt_NJF = -1;
        public string objectFloorsStr_NJF = "";

        public int objectId_1NF = -1;

        public int purposeGroupIdNJF = -1;
        public int purposeIdNJF = -1;
        public int objectTypeIdNJF = -1;
        public int objectKindIdNJF = -1;

        public object characteristicNJF = null;
        public object yearBuildNJF = null;
        public object yearExplNJF = null;
        public object techStateIdNJF = null;

        public bool includedInAct = false;
        public bool makeChangesIn1NF = false;

        public List<BalansTransfer> balansTransfers = new List<BalansTransfer>();

        public ActObject()
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

        public override string ToString()
        {
            string address = addrNomer1_NJF.Trim();

            if (addrNomer2_NJF.Length > 0)
            {
                if (address.Length > 0)
                    address += " ";

                address += addrNomer2_NJF.Trim();
            }

            if (addrNomer3_NJF.Length > 0)
            {
                if (address.Length > 0)
                    address += " ";

                address += addrNomer3_NJF.Trim();
            }

            if (addrMisc_NJF.Length > 0)
            {
                if (address.Length > 0)
                    address += " ";

                address += addrMisc_NJF.Trim();
            }

            if (addrStreet_NJF.Length > 0 && address.Length > 0)
            {
                return addrStreet_NJF + ", " + address;
            }

            return address.Length > 0 ? address : addrStreet_NJF;
        }

        public string FormatBalansTransfers()
        {
            string transfers = "";

            foreach (BalansTransfer bt in balansTransfers)
            {
                string transferType = GUKV.ImportToolUtils.AppResources.TransferTypeTransfer;

                if (bt.transferType == ObjectTransferType.Create)
                    transferType = GUKV.ImportToolUtils.AppResources.TransferTypeCreate;

                if (bt.transferType == ObjectTransferType.Destroy)
                    transferType = GUKV.ImportToolUtils.AppResources.TransferTypeDestroy;

                string orgFrom = string.IsNullOrEmpty(bt.orgFromShortName_1NF) ? "?????" : bt.orgFromShortName_1NF;
                string orgTo = string.IsNullOrEmpty(bt.orgToShortName_1NF) ? "?????" : bt.orgToShortName_1NF;
                string objAddress = bt.object1NF != null ? bt.object1NF.ToString() : "?????";
                string objSquare = bt.sqr > 0m ? bt.sqr.ToString("F2") : "?????";

                string balansFound = bt.balansId_1NF > 0 ?
                    GUKV.ImportToolUtils.AppResources.TransferTooltipFormatBalansFound :
                    GUKV.ImportToolUtils.AppResources.TransferTooltipFormatBalansNotFound;

                string transferStr = "";

                switch (bt.transferType)
                {
                    case ObjectTransferType.Transfer:
                        transferStr = string.Format("{0}\n{1} {2}\n{3} {4}\n{5} {6}\n{7} {8}\n{9}",
                            transferType,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatAddr, objAddress,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatSqr, objSquare,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatFrom, orgFrom,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatTo, orgTo,
                            balansFound);
                        break;

                    case ObjectTransferType.Create:
                        transferStr = string.Format("{0}\n{1} {2}\n{3} {4}\n{5} {6}",
                            transferType,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatAddr, objAddress,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatSqr, objSquare,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatTo, orgTo);
                        break;

                    case ObjectTransferType.Destroy:
                        transferStr = string.Format("{0}\n{1} {2}\n{3} {4}\n{5} {6}\n{7}",
                            transferType,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatAddr, objAddress,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatSqr, objSquare,
                            GUKV.ImportToolUtils.AppResources.TransferTooltipFormatFrom, orgFrom,
                            balansFound);
                        break;
                }

                if (transfers.Length > 0)
                {
                    transfers += "\n\n";
                }

                transfers += transferStr;
            }

            return transfers;
        }

        public string FormatBalansTransfersShort()
        {
            string transfers = "";

            foreach (BalansTransfer bt in balansTransfers)
            {
                string transferType = GUKV.ImportToolUtils.AppResources.TransferTypeTransfer;

                if (bt.transferType == ObjectTransferType.Create)
                    transferType = GUKV.ImportToolUtils.AppResources.TransferTypeCreate;

                if (bt.transferType == ObjectTransferType.Destroy)
                    transferType = GUKV.ImportToolUtils.AppResources.TransferTypeDestroy;

                if (transfers.Length > 0)
                {
                    transfers += ", ";
                }

                transfers += transferType;
            }

            return transfers;
        }

        public bool AllTransfersAreValid()
        {
            foreach (BalansTransfer bt in balansTransfers)
            {
                if (!bt.IsFullyDefined())
                {
                    return false;
                }
            }

            return true;
        }

        public void DeduceObjectTypeFor1NF()
        {
            makeChangesIn1NF =
                objectKindIdNJF == 1 ||
                objectKindIdNJF == 2 ||
                objectKindIdNJF == 3 ||
                objectKindIdNJF == 18;
        }

        public void CopyTo(ActObject other)
        {
            other.objectId_NJF = objectId_NJF;
            other.objectDocsPropertiesId_NJF = objectDocsPropertiesId_NJF;
            other.addrStreet_NJF = addrStreet_NJF;
            other.addrNomer1_NJF = addrNomer1_NJF;
            other.addrNomer2_NJF = addrNomer2_NJF;
            other.addrNomer3_NJF = addrNomer3_NJF;
            other.addrMisc_NJF = addrMisc_NJF;
            other.districtId_NJF = districtId_NJF;
            other.objectName_NJF = objectName_NJF;
            other.objectSquare_NJF = objectSquare_NJF;
            other.objectId_1NF = objectId_1NF;

            other.purposeGroupIdNJF = purposeGroupIdNJF;
            other.purposeIdNJF = purposeIdNJF;
            other.objectTypeIdNJF = objectTypeIdNJF;
            other.objectKindIdNJF = objectKindIdNJF;

            other.objectBalansCost_NJF = objectBalansCost_NJF;
            other.objectFinalCost_NJF = objectFinalCost_NJF;
            other.objectFloorsInt_NJF = objectFloorsInt_NJF;
            other.objectFloorsStr_NJF = objectFloorsStr_NJF;

            other.characteristicNJF = characteristicNJF;
            other.yearBuildNJF = yearBuildNJF;
            other.yearExplNJF = yearExplNJF;
            other.techStateIdNJF = techStateIdNJF;

            other.includedInAct = includedInAct;
            other.makeChangesIn1NF = makeChangesIn1NF;

            other.balansTransfers.Clear();

            foreach (BalansTransfer bt in balansTransfers)
            {
                other.balansTransfers.Add(bt.MakeCopy());
            }
        }

        //public void PerformMatchingTo1NF(ObjectFinder objectFinder1NF, OrganizationFinder orgFinder1NF)
        //{
        //    DictionaryData dictDistricts = null;

        //    if (!DB.dictionaries.TryGetValue(DB.DICT_DISTRICTS, out dictDistricts))
        //    {
        //        dictDistricts = null;
        //    }

        //    // Map the district code from NJF to 1NF
        //    object district1NF = null;

        //    if (districtId_NJF is int && dictDistricts != null)
        //    {
        //        DictionaryValue districtNJF = null;

        //        if (dictDistricts.ValuesNJF.TryGetValue((int)districtId_NJF, out districtNJF))
        //        {
        //            foreach (KeyValuePair<int, DictionaryValue> pair in dictDistricts.Values1NF)
        //            {
        //                if (districtNJF.value == pair.Value.value)
        //                {
        //                    district1NF = pair.Key;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    bool addressIsSimple = false;
        //    bool similarAddressExists = false;

        //    objectId_1NF = objectFinder1NF.FindObject(addrStreet_NJF, "",
        //        addrNomer1_NJF, addrNomer2_NJF, addrNomer2_NJF, addrMisc_NJF,
        //        district1NF, null, out addressIsSimple, out similarAddressExists);

        //    DeduceObjectTypeFor1NF();

        //    // Perform matching of balans transfers
        //    foreach (BalansTransfer bal in balansTransfers)
        //    {
        //        bal.objectId_1NF = objectId_1NF;

        //        bool categorized = false;
        //        Organization1NF org1NF = null;

        //        // Match the From organization
        //        bal.organizationFromId_1NF = orgFinder1NF.FindOrganization(bal.orgFromZkpo_NJF, bal.orgFromFullName_NJF, bal.orgFromShortName_NJF, false, out categorized);

        //        if (bal.organizationFromId_1NF > 0)
        //        {
        //            if (DB.organizations1NF.TryGetValue(bal.organizationFromId_1NF, out org1NF))
        //            {
        //                bal.orgFrom1NF = org1NF;
        //            }
        //        }

        //        // Match the To organization
        //        bal.organizationToId_1NF = orgFinder1NF.FindOrganization(bal.orgToZkpo_NJF, bal.orgToFullName_NJF, bal.orgToShortName_NJF, false, out categorized);

        //        if (bal.organizationToId_1NF > 0)
        //        {
        //            if (DB.organizations1NF.TryGetValue(bal.organizationToId_1NF, out org1NF))
        //            {
        //                bal.orgTo1NF = org1NF;
        //            }
        //        }

        //        bal.FindBalansObjectIn1NF();
        //    }
        //}

        public FbCommand PrepareInsertStatement(FbConnection connectionNJF, int newRelId, int documentId)
        {
            string fields = "";
            string values = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            AddInsertQueryParameter(ref fields, ref values, "ID", newRelId, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "ISP", DB.UserName, parameters, 18);
            AddInsertQueryParameter(ref fields, ref values, "DT", DateTime.Now, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "DOK_ID", documentId, parameters, -1);

            AddInsertQueryParameter(ref fields, ref values, "OBJECT_KOD", objectId_NJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "OBJECT_KODSTAN", 1, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "NAME", objectName_NJF, parameters, 255);
            AddInsertQueryParameter(ref fields, ref values, "CHARACTERISTIC", characteristicNJF, parameters, 100);
            AddInsertQueryParameter(ref fields, ref values, "SUMMA_BALANS", objectBalansCost_NJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "SUMMA_BALANS_0", objectBalansCost_NJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "SUMMA_ZAL", objectFinalCost_NJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "SUMMA_ZAL_0", objectFinalCost_NJF, parameters, -1);

            AddInsertQueryParameter(ref fields, ref values, "SQUARE", objectSquare_NJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "GRPURP", purposeGroupIdNJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "PURPOSE", purposeIdNJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "TEXSTAN", techStateIdNJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "OBJKIND", objectKindIdNJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "OBJTYPE", objectTypeIdNJF, parameters, -1);

            AddInsertQueryParameter(ref fields, ref values, "YEAR_BUILD", yearBuildNJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "YEAR_EXPL", yearExplNJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "LEN", objectLen_NJF, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "DIAM_TRUB", objectDiamTrub_NJF, parameters, 20);
            AddInsertQueryParameter(ref fields, ref values, "FLOORS", objectFloorsInt_NJF, parameters, -1);

            FbCommand cmd = new FbCommand("INSERT INTO OBJECT_DOKS_PROPERTIES (" + fields.TrimStart(' ', ',') + ") VALUES (" + values.TrimStart(' ', ',') + ")", connectionNJF);

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new FbParameter(param.Key, param.Value));
            }

            return cmd;
        }

        private void AddInsertQueryParameter(ref string fields, ref string values, string fieldName, object value, Dictionary<string, object> parameters, int maxStringLen)
        {
            if (value is int && (int)value < 0)
            {
                value = null;
            }

            if (value is string && ((string)value).Length == 0)
            {
                value = null;
            }

            if (value != null)
            {
                if (value is string && ((string)value).Length > maxStringLen)
                {
                    value = ((string)value).Substring(0, maxStringLen);
                }

                fields += ", " + fieldName;
                values += ", @" + fieldName;
                parameters.Add(fieldName, value);
            }
            else
            {
                fields += ", " + fieldName;
                values += ", NULL";
            }
        }

        public FbCommand PrepareUpdateStatement(FbConnection connectionNJF, int existingRelId, int documentId)
        {
            string fields = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            AddUpdateQueryParameter(ref fields, "ISP", DB.UserName, parameters, 18);
            AddUpdateQueryParameter(ref fields, "DT", DateTime.Now, parameters, -1);
            AddUpdateQueryParameter(ref fields, "DOK_ID", documentId, parameters, -1);

            AddUpdateQueryParameter(ref fields, "OBJECT_KOD", objectId_NJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "NAME", objectName_NJF, parameters, 255);
            AddUpdateQueryParameter(ref fields, "CHARACTERISTIC", characteristicNJF, parameters, 100);
            AddUpdateQueryParameter(ref fields, "SUMMA_BALANS", objectBalansCost_NJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "SUMMA_BALANS_0", objectBalansCost_NJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "SUMMA_ZAL", objectFinalCost_NJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "SUMMA_ZAL_0", objectFinalCost_NJF, parameters, -1);

            AddUpdateQueryParameter(ref fields, "SQUARE", objectSquare_NJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "GRPURP", purposeGroupIdNJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "PURPOSE", purposeIdNJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "TEXSTAN", techStateIdNJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "OBJKIND", objectKindIdNJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "OBJTYPE", objectTypeIdNJF, parameters, -1);

            AddUpdateQueryParameter(ref fields, "YEAR_BUILD", yearBuildNJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "YEAR_EXPL", yearExplNJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "LEN", objectLen_NJF, parameters, -1);
            AddUpdateQueryParameter(ref fields, "DIAM_TRUB", objectDiamTrub_NJF, parameters, 20);
            AddUpdateQueryParameter(ref fields, "FLOORS", objectFloorsInt_NJF, parameters, -1);

            FbCommand cmd = new FbCommand("UPDATE OBJECT_DOKS_PROPERTIES SET " + fields.TrimStart(' ', ',') + " WHERE ID = @relid", connectionNJF);

            cmd.Parameters.Add(new FbParameter("relid", existingRelId));

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new FbParameter(param.Key, param.Value));
            }

            return cmd;
        }

        private void AddUpdateQueryParameter(ref string fields, string fieldName, object value, Dictionary<string, object> parameters, int maxStringLen)
        {
            if (value is int && (int)value < 0)
            {
                value = null;
            }

            if (value is string && ((string)value).Length == 0)
            {
                value = null;
            }

            if (value != null)
            {
                if (value is string && ((string)value).Length > maxStringLen)
                {
                    value = ((string)value).Substring(0, maxStringLen);
                }

                fields += ", " + fieldName + " = @" + fieldName;
                parameters.Add(fieldName, value);
            }
            else
            {
                fields += ", " + fieldName + " = NULL";
            }
        }
    }

    public class ActObjectAlphabeticalComparer : IComparer<ActObject>
    {
        public ActObjectAlphabeticalComparer()
        {
        }

        int IComparer<ActObject>.Compare(ActObject x, ActObject y)
        {
            string str1 = x != null ? x.ToString() : "";
            string str2 = y != null ? y.ToString() : "";

            return str1.CompareTo(str2);
        }
    }
}
