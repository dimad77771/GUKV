using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;

namespace GUKV.DataMigration
{
    public partial class Migrator
    {
//        #region Member variables

//        /// <summary>
//        /// All changes are written to the database on behalf of this user 
//        /// </summary>
//        public const string AutoUserName = "Auto-correct";

//        /// <summary>
//        /// Values from 'dict_org_ownership' dictionary which represent all kinds of private property
//        /// </summary>
//        private static int[] ownershipTypesPrivatAll = new int[] { 10, 20, 40, 50 };

//        /// <summary>
//        /// Values from 'dict_org_ownership' dictionary which represent all kinds of community property
//        /// </summary>
//        private static int[] ownershipTypesKomunalnaAll = new int[] { 32, 33, 34, 35 };

//        /// <summary>
//        /// The value from 'dict_org_ownership' dictionary which represents private property
//        /// </summary>
//        private const int ownershipTypePrivat = 10;

//        /// <summary>
//        /// The value from 'dict_org_ownership' dictionary which represents state property
//        /// </summary>
//        private const int ownershipTypeDerzhavna = 31;

//        /// <summary>
//        /// Values from 'dict_balans_purpose' dictionary which represent various types of buildings
//        /// </summary>
//        private static int[] transferPurposeBuildings = new int[] { 1, 514, 516, 518, 542, 1001 };

//        /// <summary>
//        /// Values from 'dict_balans_purpose' dictionary which represent various types of living buildings
//        /// </summary>
//        private static int[] transferPurposeLivingBuildings = new int[] { 1 };

//        /// <summary>
//        /// Allowed deviation of the square (in %) when comparing two objects
//        /// </summary>
//        private decimal allowedSquareDeviationPercent = 0.03m;

//        /// <summary>
//        /// Contains ID's of organizations which are possible duplicates in 1NF
//        /// </summary>
//        private Dictionary<int, HashSet<int>> orgSubstitutions = new Dictionary<int, HashSet<int>>();

//        /// <summary>
//        /// A valid (unused) organization ID in 1NF. Can be auto-incremented.
//        /// </summary>
//        private int organizationIdSeed1NF = -1;

//        /// <summary>
//        /// Contains all valid key values from some 1NF dictionaries. Key is a dictionary
//        /// name (in upper case), value is a set of valid dictionary IDs
//        /// </summary>
//        private Dictionary<string, HashSet<int>> dictionaries1NF = new Dictionary<string, HashSet<int>>();

//        /// <summary>
//        /// Contains all values from 'dict_org_ownership' table
//        /// </summary>
//        private static Dictionary<int, string> dictFormOfOwnership = new Dictionary<int, string>();

//        /// <summary>
//        /// Contains all values from 'dict_obj_rights' table
//        /// </summary>
//        private static Dictionary<int, string> dictTransferRight = new Dictionary<int, string>();

//        /// <summary>
//        /// Mapping of processed SQL Server objects to 1NF object
//        /// </summary>
//        private Dictionary<int, int> correctionObjectIdMapping = new Dictionary<int, int>();

//        /// <summary>
//        /// Mapping of processed SQL Server organizations to 1NF organizations
//        /// </summary>
//        private Dictionary<int, int> correctionOrgIdMapping = new Dictionary<int, int>();

//        #endregion (Member variables)

//        #region Nested types

//        /// <summary>
//        /// Describes an object transfer record from 'Rozporadjennia'
//        /// </summary>
//        private class ObjTransfer
//        {
//            public int orgFromId = 0;
//            public int orgToId = 0;
//            public int buildingId = 0;
//            public int rightId = 0;
//            public string name = "";

//            public decimal objSquare = 0m;
//            public DateTime aktDate = DateTime.MinValue;

//            public object objectType = null;
//            public object objectKind = null;
//            public object purposeGroup = null;
//            public object purpose = null;
//            public object condition = null;
//            public object sumBalans = null;
//            public object sumZalishkova = null;
//            public object miscInfo = null;

//            public ObjTransfer()
//            {
//            }

//            public bool IsBalansTransfer
//            {
//                get
//                {
//                    return
//                        rightId == 2 ||
//                        rightId == 3 ||
//                        rightId == 6 ||
//                        rightId == 8 ||
//                        rightId == 9;
//                }
//            }

//            public bool IsOwnershipTransfer
//            {
//                get
//                {
//                    return
//                        rightId == 1 ||
//                        rightId == 5;
//                }
//            }

//            public int GetReceiverFormOfOwnership(OrganizationFinder orgFinder)
//            {
//                if (orgFinder != null)
//                {
//                    OrganizationInfo info = orgFinder.GetOrgInfoById(orgToId);

//                    if (info != null && info.ownershipCode is int)
//                    {
//                        return (int)info.ownershipCode;
//                    }
//                }

//                return -1;
//            }

//            /// <summary>
//            /// Relates the 'dict_obj_rights' dictionary and 'dict_balans_ownership_type' dictionary
//            /// </summary>
//            /// <returns>Value from the 'dict_balans_ownership_type' dictionary that corresponds to
//            /// the value from the 'dict_obj_rights' dictionary</returns>
//            public int GetBalansOwnershipType()
//            {
//                switch (rightId)
//                {
//                    case 1: return 1;
//                    case 3: return 2;
//                    case 5: return 1;
//                    case 6: return 4;
//                    case 8: return 6;
//                    case 10: return 8;
//                }

//                return -1;
//            }

//            /// <summary>
//            /// Generates a detailed description of this object
//            /// </summary>
//            /// <param name="objectFinder">Object finder helper</param>
//            /// <param name="orgFinder">Organization finder helper</param>
//            /// <returns>A meaningful description of this object</returns>
//            public string FormatToString(ObjectFinder objectFinder, OrganizationFinder orgFinder)
//            {
//                string addr = (objectFinder != null) ? objectFinder.GetObjectDescription(buildingId) : "";
//                string orgFromName = (orgFinder != null) ? orgFinder.GetOrgFullNameById(orgFromId) : "";
//                string orgToName = (orgFinder != null) ? orgFinder.GetOrgFullNameById(orgToId) : "";

//                string right = "";

//                if (!Migrator.dictTransferRight.TryGetValue(rightId, out right))
//                {
//                    right = "";
//                }

//                return string.Format(Properties.Resources.CorrectionTransferInfo, new object[] {
//                    name,
//                    objSquare.ToString(),
//                    addr,
//                    orgFromName,
//                    orgToName,
//                    right
//                } );
//            }
//        }

//        /// <summary>
//        /// Describes a group of object transfers, related to a single document
//        /// </summary>
//        private class ObjTransferByDoc
//        {
//            public int rozpDocId = 0;

//            public Dictionary<int, List<ObjTransfer>> transfersByBuildingId = new Dictionary<int, List<ObjTransfer>>();

//            public ObjTransferByDoc()
//            {
//            }

//            public void AddObjectTransfer(ObjTransfer t)
//            {
//                List<ObjTransfer> list = null;

//                if (transfersByBuildingId.TryGetValue(t.buildingId, out list))
//                {
//                    list.Add(t);
//                }
//                else
//                {
//                    list = new List<ObjTransfer>();

//                    list.Add(t);

//                    transfersByBuildingId.Add(t.buildingId, list);
//                }
//            }
//        }

//        /// <summary>
//        /// Describes an object from the 'Privatization' database
//        /// </summary>
//        private class PrivatObjectInfo
//        {
//            public int privatizationId = 0;
//            public int organizationId = 0;
//            public int buildingId = 0;
//            public decimal objSquare = 0;
//            public object name = null;
//            public object objectKind = null;
//            public object objectType = null;
//            public object purposeGroup = null;
//            public object history = null;
//            public object floor = null;
//            public object note = null;
//            public object cost = null;
//            public object costExpert = null;
//            public object expertDate = null;
//            public object aktDate = null;

//            /// <summary>
//            /// Default constructor
//            /// </summary>
//            public PrivatObjectInfo()
//            {
//            }

//            /// <summary>
//            /// Converts the aktDate member variable to DateTime
//            /// </summary>
//            public DateTime AktDate
//            {
//                get
//                {
//                    return (DateTime)aktDate;
//                }
//            }

//            /// <summary>
//            /// Generates a detailed description of this object
//            /// </summary>
//            /// <param name="objectFinder"></param>
//            /// <returns></returns>
//            public string FormatToString(ObjectFinder objectFinder, OrganizationFinder orgFinder)
//            {
//                string addr = (objectFinder != null) ? objectFinder.GetObjectDescription(buildingId) : "";
//                string orgName = (orgFinder != null) ? orgFinder.GetOrgFullNameById(organizationId) : "";

//                return string.Format(Properties.Resources.CorrectionPrivatInfo, new object[] { note, addr, objSquare.ToString(), orgName });
//            }
//        }

//        /// <summary>
//        /// Describes an object on balans from the 1NF database
//        /// </summary>
//        private class BalansObjectInfo
//        {
//            public int balansId = 0;
//            public int organizationId = 0;
//            public int buildingId = 0;
//            public decimal objSquare = 0;
//            public object formOfOwnership = null;
//            public object modifyDate = null;

//            public BalansObjectInfo()
//            {
//            }

//            public bool IsPrivatObject()
//            {
//                if (formOfOwnership is int)
//                {
//                    return Migrator.IsPrivateOwnership((int)formOfOwnership);
//                }

//                return false;
//            }

//            public bool IsCommunityObject()
//            {
//                if (formOfOwnership is int)
//                {
//                    return Migrator.IsCommunityOwnership((int)formOfOwnership);
//                }

//                return false;
//            }

//            public bool IsFormOwnershipEqual(int form)
//            {
//                if (formOfOwnership is int)
//                {
//                    return (int)formOfOwnership == form;
//                }

//                return false;
//            }

//            public string FormatToString(ObjectFinder objectFinder, OrganizationFinder orgFinder)
//            {
//                string addr = (objectFinder != null) ? objectFinder.GetObjectDescription(buildingId) : "";
//                string orgName = (orgFinder != null) ? orgFinder.GetOrgFullNameById(organizationId) : "";

//                if (modifyDate is DateTime)
//                {
//                    return string.Format(Properties.Resources.CorrectionBalansInfoWithDate,
//                        new object[] { objSquare.ToString(), orgName, addr, ((DateTime)modifyDate).ToShortDateString() });
//                }
//                else
//                {
//                    return string.Format(Properties.Resources.CorrectionBalansInfoNoDate, objSquare.ToString(), orgName, addr);
//                }
//            }
//        }

//        /// <summary>
//        /// Describes a rent agreement from the 1NF database
//        /// </summary>
//        private class RentAgreementInfo
//        {
//            public int arendaId = 0;
//            public int orgRenterId = 0;
//            public int orgBalansId = 0;
//            public int buildingId = 0;
//            public int balansId = 0;
//            public decimal rentSquare = 0;
//            public object agreementNum = null;
//            public object agreementDate = null;

//            public RentAgreementInfo()
//            {
//            }

//            public string FormatToString(ObjectFinder objectFinder, OrganizationFinder orgFinder)
//            {
//                string addr = (objectFinder != null) ? objectFinder.GetObjectDescription(buildingId) : "";
//                string orgNameRenter = (orgFinder != null) ? orgFinder.GetOrgFullNameById(orgRenterId) : "";
//                string orgNameBalans = (orgFinder != null) ? orgFinder.GetOrgFullNameById(orgBalansId) : "";

//                // Prepare the agreement number and date
//                string numberAndDate = (agreementNum is string) ? Properties.Resources.DocNum + " " + (string)agreementNum : "";

//                if (agreementDate is DateTime)
//                {
//                    numberAndDate += " " + Properties.Resources.DocDate + " " + ((DateTime)agreementDate).ToShortDateString();
//                }

//                return string.Format(Properties.Resources.CorrectionRentInfo,
//                    new object[] { numberAndDate, rentSquare.ToString(), orgNameBalans, orgNameRenter, addr });
//            }
//        }

//        #endregion (Nested types)

//        #region Data corrections

//        private void PerformDataCorrections()
//        {
//            // Register some allowed organization substitutions
//            addOrgSubstitution(1464, 6139);
//            addOrgSubstitution(4684, 21918);
//            addOrgSubstitution(5289, 5059);
//            addOrgSubstitution(8640, 133608);
//            addOrgSubstitution(8895, 4201);
//            addOrgSubstitution(8895, 155);
//            addOrgSubstitution(9012, 768);
//            addOrgSubstitution(10218, 133666);
//            addOrgSubstitution(10647, 20329);
//            addOrgSubstitution(13808, 133518);
//            addOrgSubstitution(16717, 8504);
//            addOrgSubstitution(22841, 132432);
//            addOrgSubstitution(136831, 138680);
//            addOrgSubstitution(14354, 516);

//            // Get the current ID seed for 1NF organizations
//            organizationIdSeed1NF = organizationFinder.Get1NFOrganizationIdSeed(connection1NF);

//            // Get some 1NF dictionaries
//            Extract1NFDictionary("S_VID_DIAL", "KOD_VID_DIAL");
//            Extract1NFDictionary("S_STATUS", "KOD_STATUS");
//            Extract1NFDictionary("S_FORM_GOSP", "KOD_FORM_GOSP");
//            Extract1NFDictionary("S_FORM_VLASN", "KOD_FORM_VLASN");
//            Extract1NFDictionary("S_GOSP_STRUKT", "KOD_GOSP_STRUKT");
//            Extract1NFDictionary("S_ORGAN", "KOD_ORGAN");
//            Extract1NFDictionary("S_GALUZ", "KOD_GALUZ");
//            Extract1NFDictionary("S_ADDITION_PRIZNAK", "KOD_ADDITION_PRIZNAK");
//            Extract1NFDictionary("S_PRIKMET", "KOD_PRIKMET");
//            Extract1NFDictionary("S_PRIZNAK_1", "KOD_PRIZNAK_1");
//            Extract1NFDictionary("S_VIDOM_NAL", "KOD_VIDOM_NAL");
//            Extract1NFDictionary("S_IMEN", "KOD_IMEN");
//            Extract1NFDictionary("S_ORG_FORM", "KOD_ORG_FORM");
//            Extract1NFDictionary("S_VID_GOSP_STR", "KOD_VID_GOSP_STR");
//            Extract1NFDictionary("S_AKCIA", "KOD_AKCIA");
//            Extract1NFDictionary("S_VIDDIL", "KOD");

//            Extract1NFDictionary("SKINDOBJ", "KOD");
//            Extract1NFDictionary("STYPEOBJ", "KOD");
//            Extract1NFDictionary("STEXSTAN", "KOD");
//            Extract1NFDictionary("SGRPURPOSE", "KOD");
//            Extract1NFDictionary("SPURPOSE", "KOD");
//            Extract1NFDictionary("SHISTORY", "KOD");

//            // Get some dictionaries from SQL Server
//            PopulateOwnershipDictionary();
//            PopulateTransferRightsDictionary();

//            // Delete all previously reported errors that could not be corrected
//            using (SqlCommand cmd = new SqlCommand("DELETE FROM sys_report_corrections WHERE is_corrected = 0", connectionSqlClient))
//            {
//                cmd.ExecuteNonQuery();
//            }

//            // Perform data corrections
//            CorrectObjTransferData();
//            // CorrectPrivatizationData(); - Disabled, because address mapping defined in the Privatization database is incorrect
//        }

//        private void addOrgSubstitution(int orgId, int orgMatchId)
//        {
//            HashSet<int> substitutions = null;

//            if (orgSubstitutions.TryGetValue(orgId, out substitutions))
//            {
//                substitutions.Add(orgMatchId);
//            }
//            else
//            {
//                substitutions = new HashSet<int>();

//                substitutions.Add(orgMatchId);

//                orgSubstitutions.Add(orgId, substitutions);
//            }

//            // Add the reverse pair as well
//            if (orgSubstitutions.TryGetValue(orgMatchId, out substitutions))
//            {
//                substitutions.Add(orgId);
//            }
//            else
//            {
//                substitutions = new HashSet<int>();

//                substitutions.Add(orgId);

//                orgSubstitutions.Add(orgMatchId, substitutions);
//            }
//        }

//        private void AddCorrectionLogEntry(bool isCorrected, int modificationKind, string objectDesc, string actionDescr, string reasonDescr)
//        {
//            string query = @"INSERT INTO sys_report_corrections (is_corrected, correction_kind, obj_descr, action_descr,
//                reason_descr, modify_date) VALUES (@ic, @ck, @objd, @actd, @reasd, @mdt)";

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                cmd.Parameters.Add(new SqlParameter("ic", isCorrected ? 1 : 0));
//                cmd.Parameters.Add(new SqlParameter("ck", modificationKind));
//                cmd.Parameters.Add(new SqlParameter("objd", objectDesc));
//                cmd.Parameters.Add(new SqlParameter("actd", actionDescr));
//                cmd.Parameters.Add(new SqlParameter("reasd", reasonDescr));
//                cmd.Parameters.Add(new SqlParameter("mdt", DateTime.Now.Date));

//                cmd.ExecuteNonQuery();
//            }
//        }

//        private string ConvertIntArrayToStrList(int[] arr)
//        {
//            string result = "";

//            foreach (int value in arr)
//            {
//                if (result.Length > 0)
//                {
//                    result += ", ";
//                }

//                result += value.ToString();
//            }

//            return result;
//        }

//        private bool IntArrayContains(int[] arr, int value)
//        {
//            foreach (int val in arr)
//            {
//                if (val == value)
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        #endregion (Data corrections)

//        #region Object transfer data validation

//        private const int TransferCorrectionOwnership = 101;
//        private const int TransferCorrectionAddBalansObject = 102;
//        private const int TransferCorrectionCutBalansObject = 103;
//        private const int TransferCorrectionReplaceBalansHolder = 104;

//        private const int TransferSuspectTooManyObjForReceiver = 151;
//        private const int TransferSuspectWrongSquareForReceiver = 152;
//        private const int TransferSuspectTooManyObjForGiver = 153;
//        private const int TransferSuspectNoObjForGiver = 154;
//        private const int TransferSuspectBalansIsUpToDate = 155;
//        private const int TransferSuspectDiferentHolder = 156;
//        private const int TransferSuspectBalansAmbiguity = 157;

//        private void CorrectObjTransferData()
//        {
//            logger.WriteInfo("Performing auto-corrections in 1NF using Rozporadjennia data");

//            Dictionary<int, ObjTransferByDoc> transfers = new Dictionary<int, ObjTransferByDoc>();
//            Dictionary<int, List<BalansObjectInfo>> balans = new Dictionary<int, List<BalansObjectInfo>>();

//            PrepareTransferCorrectionData(transfers, balans);

//            // Process all transfer documents
//            foreach (KeyValuePair<int, ObjTransferByDoc> transferDoc in transfers)
//            {
//                foreach (KeyValuePair<int, List<ObjTransfer>> transfersByBuilding in transferDoc.Value.transfersByBuildingId)
//                {
//                    foreach (ObjTransfer transfer in transfersByBuilding.Value)
//                    {
//                        if (transfer.IsBalansTransfer)
//                        {
//                            // Find another transfer of the same building, with different right ID
//                            ObjTransfer transferFormOfOwnership = null;

//                            foreach (ObjTransfer t in transfersByBuilding.Value)
//                            {
//                                if (OrgMatch(t.orgFromId, transfer.orgFromId) &&
//                                    t.objSquare == transfer.objSquare &&
//                                    t.IsOwnershipTransfer)
//                                {
//                                    transferFormOfOwnership = t;
//                                    break;
//                                }
//                            }

//                            // Check if the 'receiver' organization has something on balans
//                            List<BalansObjectInfo> balansObjectsAll = null;
//                            List<BalansObjectInfo> balansObjects = new List<BalansObjectInfo>();
//                            bool objectWithTheSameSquareExists = false;

//                            if (balans.TryGetValue(transfer.buildingId, out balansObjectsAll))
//                            {
//                                foreach (BalansObjectInfo balObj in balansObjectsAll)
//                                {
//                                    if (OrgMatch(balObj.organizationId, transfer.orgToId))
//                                    {
//                                        balansObjects.Add(balObj);
//                                    }

//                                    if (SquareMatch(balObj.objSquare, transfer.objSquare))
//                                    {
//                                        objectWithTheSameSquareExists = true;
//                                    }
//                                }
//                            }

//                            if (balansObjects.Count > 0)
//                            {
//                                // This organization has some objects on balans; try to find an object with the same square
//                                List<BalansObjectInfo> sameSquareObjects = new List<BalansObjectInfo>();
//                                List<BalansObjectInfo> similarSquareObjects = new List<BalansObjectInfo>();
//                                List<BalansObjectInfo> greaterSquareObjects = new List<BalansObjectInfo>();

//                                foreach (BalansObjectInfo balObj in balansObjects)
//                                {
//                                    if (balObj.objSquare == transfer.objSquare)
//                                    {
//                                        sameSquareObjects.Add(balObj);
//                                    }
//                                    else if (SquareMatch(balObj.objSquare, transfer.objSquare))
//                                    {
//                                        similarSquareObjects.Add(balObj);
//                                    }
//                                    else if (balObj.objSquare > transfer.objSquare)
//                                    {
//                                        greaterSquareObjects.Add(balObj);
//                                    }
//                                }

//                                if (sameSquareObjects.Count > 0)
//                                {
//                                    if (sameSquareObjects.Count == 1)
//                                    {
//                                        // We may need to correct the form of ownership
//                                        ProcessTransferExistingObject(transfer, sameSquareObjects[0], transferFormOfOwnership);
//                                    }
//                                    else
//                                    {
//                                        AddCorrectionLogEntry(false,
//                                            TransferSuspectTooManyObjForReceiver,
//                                            transfer.FormatToString(objectFinder, organizationFinder),
//                                            "",
//                                            Properties.Resources.CorrectionTransferTooManyObjForReceiver);
//                                    }
//                                }
//                                else if (similarSquareObjects.Count > 0)
//                                {
//                                    if (similarSquareObjects.Count == 1)
//                                    {
//                                        // We may need to correct the form of ownership
//                                        ProcessTransferExistingObject(transfer, similarSquareObjects[0], transferFormOfOwnership);
//                                    }
//                                    else
//                                    {
//                                        AddCorrectionLogEntry(false,
//                                            TransferSuspectTooManyObjForReceiver,
//                                            transfer.FormatToString(objectFinder, organizationFinder),
//                                            "",
//                                            Properties.Resources.CorrectionTransferTooManyObjForReceiver);
//                                    }
//                                }
//                                else
//                                {
//                                    // Do not throw an error if there is an object with bigger square
//                                    if (greaterSquareObjects.Count == 0)
//                                    {
//                                        if (balansObjects.Count > 1)
//                                        {
//                                            AddCorrectionLogEntry(false,
//                                                TransferSuspectTooManyObjForReceiver,
//                                                transfer.FormatToString(objectFinder, organizationFinder),
//                                                "",
//                                                Properties.Resources.CorrectionTransferTooManyObjForReceiver);
//                                        }
//                                        else
//                                        {
//                                            AddCorrectionLogEntry(false,
//                                                TransferSuspectWrongSquareForReceiver,
//                                                transfer.FormatToString(objectFinder, organizationFinder),
//                                                "",
//                                                Properties.Resources.CorrectionTransferSqrMismatch);
//                                        }
//                                    }
//                                }
//                            }
//                            else if (objectWithTheSameSquareExists)
//                            {
//                                // There is a similar object in 1NF, but it has a different owner
//                                AddCorrectionLogEntry(false,
//                                    TransferSuspectDiferentHolder,
//                                    transfer.FormatToString(objectFinder, organizationFinder),
//                                    "",
//                                    Properties.Resources.CorrectionTransferOwnerMismatch);
//                            }
//                            else
//                            {
//                                // Check if the 'giver' organization has something on balans
//                                balansObjects.Clear();

//                                if (balansObjectsAll != null)
//                                {
//                                    foreach (BalansObjectInfo balObj in balansObjectsAll)
//                                    {
//                                        if (OrgMatch(balObj.organizationId, transfer.orgFromId))
//                                        {
//                                            balansObjects.Add(balObj);
//                                        }
//                                    }
//                                }

//                                if (balansObjects.Count > 0)
//                                {
//                                    // Separate the balans objects into 3 categories: objects with equal square, similar square, and greater square
//                                    List<BalansObjectInfo> balansEqualSqr = new List<BalansObjectInfo>();
//                                    List<BalansObjectInfo> balansSimilarSqr = new List<BalansObjectInfo>();
//                                    List<BalansObjectInfo> balansGreaterSqr = new List<BalansObjectInfo>();

//                                    foreach (BalansObjectInfo balObj in balansObjects)
//                                    {
//                                        if (balObj.objSquare == transfer.objSquare)
//                                        {
//                                            balansEqualSqr.Add(balObj);
//                                        }
//                                        else if (SquareMatch(balObj.objSquare, transfer.objSquare))
//                                        {
//                                            balansSimilarSqr.Add(balObj);
//                                        }
//                                        else if (balObj.objSquare > transfer.objSquare)
//                                        {
//                                            balansGreaterSqr.Add(balObj);
//                                        }
//                                    }

//                                    List<BalansObjectInfo> foundObjects = balansEqualSqr;

//                                    if (foundObjects.Count == 0)
//                                        foundObjects = balansSimilarSqr;

//                                    if (foundObjects.Count == 0)
//                                        foundObjects = balansGreaterSqr;

//                                    if (foundObjects.Count == 1)
//                                    {
//                                        ProcessTransfer(transfer, foundObjects[0], transferFormOfOwnership);
//                                    }
//                                    else
//                                    {
//                                        // No automatic corrections can be made; report a problem
//                                        if (foundObjects.Count > 1)
//                                        {
//                                            AddCorrectionLogEntry(false,
//                                                TransferSuspectTooManyObjForGiver,
//                                                transfer.FormatToString(objectFinder, organizationFinder),
//                                                "",
//                                                Properties.Resources.CorrectionTransferTooManyObjForGiver);
//                                        }
//                                        else
//                                        {
//                                            AddCorrectionLogEntry(false,
//                                                TransferSuspectNoObjForGiver,
//                                                transfer.FormatToString(objectFinder, organizationFinder),
//                                                "",
//                                                Properties.Resources.CorrectionTransferNoSuitableObjForGiver);
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    // None of the organizations has something on balans; create a new object in 1NF
//                                    if (balansObjectsAll != null && balansObjectsAll.Count > 0)
//                                    {
//                                        // Ambiguity exists; requires manual actions
//                                        AddCorrectionLogEntry(false,
//                                            TransferSuspectBalansAmbiguity,
//                                            transfer.FormatToString(objectFinder, organizationFinder),
//                                            "",
//                                            Properties.Resources.CorrectionTransferAmbiguity);
//                                    }
//                                    else
//                                    {
//                                        DateTime actualizationDate = GetInfoDateForBalansObject(transfer.buildingId,
//                                            transfer.orgToId, transfer.objSquare, balans);

//                                        if (actualizationDate < transfer.aktDate)
//                                        {
//                                            using (FbTransaction transaction = connection1NF.BeginTransaction())
//                                            {
//                                                try
//                                                {
//                                                    AddBalansObjFromTransfer(transfer,
//                                                        transferFormOfOwnership != null ? transferFormOfOwnership.GetReceiverFormOfOwnership(organizationFinder) : -1,
//                                                        transaction);

//                                                    // Commit the transaction
//                                                    transaction.Commit();
//                                                }
//                                                catch (Exception ex)
//                                                {
//                                                    logger.WriteError("Exception in AddBalansObjFromTransfer() function: " + ex.Message);

//                                                    // Roll back the transaction
//                                                    transaction.Rollback();
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        private void PrepareTransferCorrectionData(Dictionary<int, ObjTransferByDoc> transfers,
//            Dictionary<int, List<BalansObjectInfo>> balans)
//        {
//            string query = @"SELECT
//                    transfer_id,
//                    building_id,
//                    org_from_id,
//                    org_to_id,
//                    right_id,
//                    rozp_doc_id,
//                    akt_date,
//                    sqr_transferred,
//                    name,
//                    object_type_int,
//                    object_kind_int,
//                    purpose_group_int,
//                    purpose_int,
//                    condition_int,
//                    sum_balans,
//                    sum_zalishkova,
//                    misc_info
//                FROM view_object_rights
//                WHERE
//                    NOT sqr_transferred IS NULL AND
//                    NOT akt_date IS NULL AND
//                    NOT rozp_doc_id IS NULL AND
//                    purpose_int IN ($)";

//            query = query.Replace("$", ConvertIntArrayToStrList(transferPurposeBuildings));

//            HashSet<int> transferBuildings = new HashSet<int>();

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        ObjTransfer info = new ObjTransfer();

//                        info.buildingId = reader.GetInt32(1);
//                        info.orgFromId = reader.GetInt32(2);
//                        info.orgToId = reader.GetInt32(3);
//                        info.rightId = reader.GetInt32(4);
//                        info.aktDate = reader.GetDateTime(6);
//                        info.objSquare = reader.GetDecimal(7);

//                        info.name = reader.IsDBNull(8) ? "" : reader.GetString(8);
//                        info.objectType = reader.IsDBNull(9) ? null : reader.GetValue(9);
//                        info.objectKind = reader.IsDBNull(10) ? null : reader.GetValue(10);
//                        info.purposeGroup = reader.IsDBNull(11) ? null : reader.GetValue(11);
//                        info.purpose = reader.IsDBNull(12) ? null : reader.GetValue(12);
//                        info.condition = reader.IsDBNull(13) ? null : reader.GetValue(13);
//                        info.sumBalans = reader.IsDBNull(14) ? null : reader.GetValue(14);
//                        info.sumZalishkova = reader.IsDBNull(15) ? null : reader.GetValue(15);
//                        info.miscInfo = reader.IsDBNull(16) ? null : reader.GetValue(16);

//                        // Check if the building information is correct
//                        ObjectInfo objInfo = objectFinder.GetObjectInfo(info.buildingId);
//                        bool objectInfoExists = false;

//                        if (objInfo != null)
//                        {
//                            if (objInfo.street is string && ((string)objInfo.street).Length > 0 &&
//                                objInfo.nomer1 is string && ((string)objInfo.nomer1).Length > 0)
//                            {
//                                objectInfoExists = true;
//                            }
//                        }

//                        if (objectInfoExists)
//                        {
//                            // Limit the misc. information to 100 characters; this is a limitation of 1NF "BALANS_1NF" table
//                            if (info.miscInfo is string)
//                            {
//                                string s = (string)info.miscInfo;

//                                if (s.Length > 100)
//                                {
//                                    info.miscInfo = s.Substring(0, 100);
//                                }
//                            }

//                            // Relate this transfer to its document
//                            int rozpDocId = reader.GetInt32(5);

//                            ObjTransferByDoc transferByDoc = null;

//                            if (transfers.TryGetValue(rozpDocId, out transferByDoc))
//                            {
//                                transferByDoc.AddObjectTransfer(info);
//                            }
//                            else
//                            {
//                                transferByDoc = new ObjTransferByDoc();

//                                transferByDoc.rozpDocId = rozpDocId;
//                                transferByDoc.AddObjectTransfer(info);

//                                transfers.Add(rozpDocId, transferByDoc);
//                            }

//                            // We also need to know the set of all used buildings
//                            transferBuildings.Add(info.buildingId);

//                            List<int> similarBuildings = FindAllBuildingsByPrimaryAddress(info.buildingId);

//                            foreach (int similarId in similarBuildings)
//                            {
//                                transferBuildings.Add(similarId);
//                            }
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            // Prepare the cache of balans holders for each building we are interested in
//            List<string> buildingLists = SplitIDsIntoSetsOf100(transferBuildings);

//            foreach (string buildingIdList in buildingLists)
//            {
//                query = @"SELECT
//                    b.balans_id,
//                    b.organization_id,
//                    b.building_id,
//                    b.sqr_total,
//                    b.org_ownership_int,
//                    b.input_date
//                FROM
//                    view_balans b
//                WHERE
//                    b.building_id IN ($) AND NOT (b.sqr_total IS NULL)";

//                query = query.Replace("$", buildingIdList);

//                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                {
//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            BalansObjectInfo info = new BalansObjectInfo();

//                            info.balansId = reader.GetInt32(0);
//                            info.organizationId = reader.GetInt32(1);
//                            info.buildingId = reader.GetInt32(2);
//                            info.objSquare = reader.GetDecimal(3);
//                            info.formOfOwnership = reader.IsDBNull(4) ? null : reader.GetValue(4);
//                            info.modifyDate = reader.IsDBNull(5) ? null : reader.GetValue(5);

//                            List<BalansObjectInfo> list = null;

//                            if (balans.TryGetValue(info.buildingId, out list))
//                            {
//                                list.Add(info);
//                            }
//                            else
//                            {
//                                list = new List<BalansObjectInfo>();

//                                list.Add(info);

//                                balans.Add(info.buildingId, list);
//                            }
//                        }

//                        reader.Close();
//                    }
//                }
//            }
//        }

//        private List<string> SplitIDsIntoSetsOf100(HashSet<int> idSet)
//        {
//            List<string> result = new List<string>();

//            int curCount = 0;
//            string curSet = "";

//            while (idSet.Count > 0)
//            {
//                int id = idSet.First();
//                idSet.Remove(id);

//                if (curSet.Length > 0)
//                    curSet += ", ";

//                curSet += id.ToString();
//                curCount++;

//                if (curCount == 100)
//                {
//                    if (curSet.Length > 0)
//                        result.Add(curSet);

//                    curSet = "";
//                    curCount = 0;
//                }
//            }

//            if (curSet.Length > 0)
//                result.Add(curSet);

//            return result;
//        }

//        private void ProcessTransfer(ObjTransfer transfer, BalansObjectInfo balansObj,
//            ObjTransfer transferOwnership)
//        {
//            // Calculate the difference between transferred square and object square
//            decimal diff = balansObj.objSquare - transfer.objSquare;

//            if (diff >= 50m)
//            {
//                // The source object is much bigger; cut a part from it
//                using (FbTransaction transaction = connection1NF.BeginTransaction())
//                {
//                    try
//                    {
//                        bool dateMismatch = false;

//                        if (CutBalansObject(balansObj, transfer.objSquare, transfer.aktDate,
//                                transfer.FormatToString(objectFinder, organizationFinder),
//                                Properties.Resources.CorrectionReasonTransfer,
//                                transaction, out dateMismatch))
//                        {
//                            AddBalansObjFromTransfer(transfer,
//                                transferOwnership != null ? transferOwnership.GetReceiverFormOfOwnership(organizationFinder) : -1,
//                                transaction);
//                        }
//                        else
//                        {
//                            if (dateMismatch)
//                            {
//                                AddCorrectionLogEntry(false,
//                                    TransferSuspectBalansIsUpToDate,
//                                    transfer.FormatToString(objectFinder, organizationFinder),
//                                    "",
//                                    string.Format(
//                                        Properties.Resources.CorrectionTransferBalansUpToDate,
//                                        balansObj.FormatToString(objectFinder, organizationFinder)));
//                            }
//                            else
//                            {
//                                ReplaceBalansHolder(balansObj,
//                                    transfer.orgToId,
//                                    transferOwnership != null ? transferOwnership.GetReceiverFormOfOwnership(organizationFinder) : -1,
//                                    transfer.FormatToString(objectFinder, organizationFinder),
//                                    Properties.Resources.CorrectionReasonTransfer,
//                                    transaction,
//                                    transfer.aktDate,
//                                    out dateMismatch);
//                            }
//                        }

//                        // Commit the transaction
//                        transaction.Commit();
//                    }
//                    catch (Exception ex)
//                    {
//                        logger.WriteError("Exception in ProcessTransfer() function: " + ex.Message);

//                        // Roll back the transaction
//                        transaction.Rollback();
//                    }
//                }
//            }
//            else if (SquareMatch(balansObj.objSquare, transfer.objSquare))
//            {
//                // Assign a new owner for this object
//                using (FbTransaction transaction = connection1NF.BeginTransaction())
//                {
//                    try
//                    {
//                        bool dateMismatch = false;

//                        if (ReplaceBalansHolder(balansObj,
//                                transfer.orgToId,
//                                transferOwnership != null ? transferOwnership.GetReceiverFormOfOwnership(organizationFinder) : -1,
//                                transfer.FormatToString(objectFinder, organizationFinder),
//                                Properties.Resources.CorrectionReasonTransfer,
//                                transaction,
//                                transfer.aktDate,
//                                out dateMismatch))
//                        {
//                            // Commit the transaction
//                            transaction.Commit();
//                        }
//                        else
//                        {
//                            // Roll back the transaction
//                            transaction.Rollback();

//                            AddCorrectionLogEntry(false,
//                                TransferSuspectBalansIsUpToDate,
//                                transfer.FormatToString(objectFinder, organizationFinder),
//                                "",
//                                string.Format(
//                                    Properties.Resources.CorrectionTransferBalansUpToDate,
//                                    balansObj.FormatToString(objectFinder, organizationFinder)));
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        logger.WriteError("Exception in ReplaceBalansHolder() function: " + ex.Message);

//                        // Roll back the transaction
//                        transaction.Rollback();
//                    }
//                }
//            }
//            else
//            {
//                AddCorrectionLogEntry(false,
//                    TransferSuspectWrongSquareForReceiver,
//                    transfer.FormatToString(objectFinder, organizationFinder),
//                    "",
//                    Properties.Resources.CorrectionTransferSqrMismatch);
//            }
//        }

//        private void ProcessTransferExistingObject(ObjTransfer transfer, BalansObjectInfo balansObj,
//            ObjTransfer transferOwnership)
//        {
//            // All we can do is correct form of ownership
//            if (transferOwnership != null)
//            {
//                CorrectBalansOwnership(balansObj, transferOwnership.aktDate,
//                    transferOwnership.GetReceiverFormOfOwnership(organizationFinder),
//                    transferOwnership.FormatToString(objectFinder, organizationFinder), false);
//            }
//        }

//        #endregion (Object transfer data validation)

//        #region Privatization data validation

//        private const int PrivatCorrectionOwnership = 201;
//        private const int PrivatCorrectionAddBalansObject = 202;
//        private const int PrivatCorrectionCutBalansObject = 203;
//        private const int PrivatCorrectionReplaceBalansHolder = 204;

//        private const int PrivatSuspectTooManyObjects = 251;
//        private const int PrivatSuspectWrongSquare = 252;
//        private const int PrivatSuspectNo1NFData = 253;
//        private const int PrivatSuspectTooManyRentAgreements = 254;
//        private const int PrivatSuspectNoRentAgreements = 255;
//        private const int PrivatSuspectRentAgreementsUndefined = 256;
//        private const int PrivatSuspectRentAgreementsNoBalans = 257;
//        private const int PrivatSuspectDifferentOrg = 258;
//        private const int PrivatSuspectNoAkt = 259;
//        private const int PrivatSuspectBalansIsUpToDate = 260;

//        private void CorrectPrivatizationData()
//        {
//            logger.WriteInfo("Performing auto-corrections in 1NF using Privatization data");

//            List<PrivatObjectInfo> privObjects = new List<PrivatObjectInfo>();
//            Dictionary<int, List<BalansObjectInfo>> balans = new Dictionary<int, List<BalansObjectInfo>>();
//            Dictionary<int, List<RentAgreementInfo>> arenda = new Dictionary<int, List<RentAgreementInfo>>();

//            PreparePrivatCorrectionData(privObjects, balans, arenda);

//            // Remove privatization objects which are in fact valid
//            FilterPrivatObjects(privObjects, balans);

//            // Process each privatization object
//            foreach (PrivatObjectInfo privObject in privObjects)
//            {
//                // Check if this organization has something on balans
//                List<BalansObjectInfo> balansObjects = new List<BalansObjectInfo>();
//                List<BalansObjectInfo> balansObjectsAll = null;

//                if (balans.TryGetValue(privObject.buildingId, out balansObjectsAll))
//                {
//                    foreach (BalansObjectInfo balObj in balansObjectsAll)
//                    {
//                        if (OrgMatch(balObj.organizationId, privObject.organizationId))
//                        {
//                            balansObjects.Add(balObj);
//                        }
//                    }
//                }

//                if (balansObjects.Count > 0)
//                {
//                    // This organization has some objects on balans; try to find an object with the same square
//                    List<BalansObjectInfo> sameSquareObjects = new List<BalansObjectInfo>();
//                    List<BalansObjectInfo> similarSquareObjects = new List<BalansObjectInfo>();

//                    foreach (BalansObjectInfo balObj in balansObjects)
//                    {
//                        if (balObj.objSquare == privObject.objSquare)
//                        {
//                            sameSquareObjects.Add(balObj);
//                        }
//                        else if (SquareMatch(balObj.objSquare, privObject.objSquare))
//                        {
//                            similarSquareObjects.Add(balObj);
//                        }
//                    }

//                    if (sameSquareObjects.Count == 1)
//                    {
//                        // Probably we need to correct ownership type
//                        CorrectBalansOwnership(sameSquareObjects[0], privObject.aktDate, ownershipTypePrivat,
//                            privObject.FormatToString(objectFinder, organizationFinder), true);
//                    }
//                    else if (sameSquareObjects.Count == 0 && similarSquareObjects.Count == 1)
//                    {
//                        // Probably we need to correct ownership type
//                        CorrectBalansOwnership(similarSquareObjects[0], privObject.aktDate, ownershipTypePrivat,
//                            privObject.FormatToString(objectFinder, organizationFinder), true);
//                    }
//                    else
//                    {
//                        // No automatic corrections can be made; report a suspect
//                        if (balansObjects.Count > 1)
//                        {
//                            AddCorrectionLogEntry(false,
//                                PrivatSuspectTooManyObjects,
//                                privObject.FormatToString(objectFinder, organizationFinder),
//                                "",
//                                string.Format(Properties.Resources.CorrectionMsgTooManyObjOnBalans,
//                                    organizationFinder.GetOrgFullNameById(privObject.organizationId)));
//                        }
//                        else
//                        {
//                            AddCorrectionLogEntry(false,
//                                PrivatSuspectWrongSquare,
//                                privObject.FormatToString(objectFinder, organizationFinder),
//                                "",
//                                Properties.Resources.CorrectionMsgIncorrectPrivatSquare);
//                        }
//                    }
//                }
//                else
//                {
//                    // No objects on balans for this organization; try to find its rent agreements
//                    if (balansObjectsAll != null)
//                    {
//                        List<RentAgreementInfo> rentAgreements = FindRentAgreements(privObject, balansObjectsAll, arenda);

//                        // Separate the rented objects into 3 categories: rent agreements with equal square, similar square, and greater square
//                        List<RentAgreementInfo> rentAgreementsEqualSqr = new List<RentAgreementInfo>();
//                        List<RentAgreementInfo> rentAgreementsSimilarSqr = new List<RentAgreementInfo>();
//                        List<RentAgreementInfo> rentAgreementsGreaterSqr = new List<RentAgreementInfo>();

//                        foreach (RentAgreementInfo agreement in rentAgreements)
//                        {
//                            if (agreement.rentSquare == privObject.objSquare)
//                            {
//                                rentAgreementsEqualSqr.Add(agreement);
//                            }
//                            else if (SquareMatch(agreement.rentSquare, privObject.objSquare))
//                            {
//                                rentAgreementsSimilarSqr.Add(agreement);
//                            }
//                            else if (agreement.rentSquare > privObject.objSquare)
//                            {
//                                rentAgreementsGreaterSqr.Add(agreement);
//                            }
//                        }

//                        List<RentAgreementInfo> foundAgreements = rentAgreementsEqualSqr;

//                        if (foundAgreements.Count == 0)
//                            foundAgreements = rentAgreementsSimilarSqr;

//                        if (foundAgreements.Count == 0)
//                            foundAgreements = rentAgreementsGreaterSqr;

//                        if (foundAgreements.Count > 0)
//                        {
//                            int balansObjCount = CalculateBalansObjCountForRentAgreements(foundAgreements);

//                            if (balansObjCount == 1)
//                            {
//                                UpdatePrivatFromRentAgreement(balans, privObject, foundAgreements[0]);
//                            }
//                            else
//                            {
//                                AddCorrectionLogEntry(false,
//                                    PrivatSuspectTooManyRentAgreements,
//                                    privObject.FormatToString(objectFinder, organizationFinder),
//                                    "",
//                                    string.Format(Properties.Resources.CorrectionMsgTooManyRentAgreements,
//                                        organizationFinder.GetOrgFullNameById(privObject.organizationId)));
//                            }
//                        }
//                        else
//                        {
//                            // No automatic corrections can be made; report a suspect
//                            if (rentAgreements.Count == 0)
//                            {
//                                AddCorrectionLogEntry(false,
//                                    PrivatSuspectNoRentAgreements,
//                                    privObject.FormatToString(objectFinder, organizationFinder),
//                                    "",
//                                    string.Format(Properties.Resources.CorrectionMsgNoRentAgreements,
//                                        organizationFinder.GetOrgFullNameById(privObject.organizationId)));
//                            }
//                            else if (rentAgreements.Count > 1)
//                            {
//                                AddCorrectionLogEntry(false,
//                                    PrivatSuspectTooManyRentAgreements,
//                                    privObject.FormatToString(objectFinder, organizationFinder),
//                                    "",
//                                    string.Format(Properties.Resources.CorrectionMsgTooManyRentAgreements,
//                                        organizationFinder.GetOrgFullNameById(privObject.organizationId)));
//                            }
//                            else
//                            {
//                                AddCorrectionLogEntry(false,
//                                    PrivatSuspectRentAgreementsUndefined,
//                                    privObject.FormatToString(objectFinder, organizationFinder),
//                                    "",
//                                    string.Format(Properties.Resources.CorrectionMsgWrongRentAgreement,
//                                        organizationFinder.GetOrgFullNameById(privObject.organizationId)));
//                            }
//                        }
//                    }
//                    else
//                    {
//                        // Nothing on balans for this address in 1NF; report a problem
//                        AddCorrectionLogEntry(false,
//                            PrivatSuspectNo1NFData,
//                            privObject.FormatToString(objectFinder, organizationFinder),
//                            "",
//                            string.Format(Properties.Resources.CorrectionMsgNoDataByAddress,
//                                objectFinder.GetObjectDescription(privObject.buildingId)));
//                    }
//                }
//            }
//        }

//        private void PreparePrivatCorrectionData(List<PrivatObjectInfo> privObjects,
//            Dictionary<int, List<BalansObjectInfo>> balans,
//            Dictionary<int, List<RentAgreementInfo>> arenda)
//        {
//            // Prepare the list of objects from Privatization database that must be verified
//            string query = @"SELECT
//                p.id,
//                p.organization_id,
//                p.building_id,
//                p.sqr_total,
//                p.obj_name,
//                p.object_kind_id,
//                p.object_type_id,
//                p.purpose_group_id,
//                p.history_id,
//                p.obj_floor,
//                p.note,
//                p.cost,
//                p.cost_expert,
//                p.expert_date,
//                akt.doc_date
//            FROM
//                privatization p
//                LEFT OUTER JOIN view_privatization_akts akt ON akt.privatization_id = p.id
//            WHERE
//                p.privat_state_id IN (2, 6) AND
//                NOT (p.organization_id IS NULL) AND
//                NOT (p.sqr_total IS NULL) AND
//                NOT EXISTS (SELECT id FROM balans b WHERE
//		            b.building_id = p.building_id AND
//		            b.organization_id = p.organization_id AND
//		            b.sqr_total >= p.sqr_total AND
//		            b.form_ownership_id IN ($))";

//            query = query.Replace("$", ConvertIntArrayToStrList(ownershipTypesPrivatAll));

//            HashSet<int> privatBuildings = new HashSet<int>();

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        PrivatObjectInfo info = new PrivatObjectInfo();

//                        info.privatizationId = reader.GetInt32(0);
//                        info.organizationId = reader.GetInt32(1);
//                        info.buildingId = reader.GetInt32(2);
//                        info.objSquare = reader.GetDecimal(3);
//                        info.name = reader.IsDBNull(4) ? null : reader.GetValue(4);
//                        info.objectKind = reader.IsDBNull(5) ? null : reader.GetValue(5);
//                        info.objectType = reader.IsDBNull(6) ? null : reader.GetValue(6);
//                        info.purposeGroup = reader.IsDBNull(7) ? null : reader.GetValue(7);
//                        info.history = reader.IsDBNull(8) ? null : reader.GetValue(8);
//                        info.floor = reader.IsDBNull(9) ? null : reader.GetValue(9);
//                        info.note = reader.IsDBNull(10) ? null : reader.GetValue(10);
//                        info.cost = reader.IsDBNull(11) ? null : reader.GetValue(11);
//                        info.costExpert = reader.IsDBNull(12) ? null : reader.GetValue(12);
//                        info.expertDate = reader.IsDBNull(13) ? null : reader.GetValue(13);
//                        info.aktDate = reader.IsDBNull(14) ? null : reader.GetValue(14);

//                        privObjects.Add(info);

//                        // We also need to know the set of all used buildings
//                        privatBuildings.Add(info.buildingId);
//                    }

//                    reader.Close();
//                }
//            }

//            // Prepare the list of building IDs
//            string buildingIdList = "";

//            foreach (int buildingId in privatBuildings)
//            {
//                if (buildingIdList.Length > 0)
//                {
//                    buildingIdList += ", ";
//                }

//                buildingIdList += buildingId.ToString();
//            }

//            // Prepare the cache of balans holders for each building we are interested in
//            query = @"SELECT
//                b.balans_id,
//                b.organization_id,
//                b.building_id,
//                b.sqr_total,
//                b.org_ownership_int,
//                b.input_date
//            FROM
//                view_balans b
//            WHERE
//                b.building_id IN ($) AND NOT (b.sqr_total IS NULL)";

//            query = query.Replace("$", buildingIdList);

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        BalansObjectInfo info = new BalansObjectInfo();

//                        info.balansId = reader.GetInt32(0);
//                        info.organizationId = reader.GetInt32(1);
//                        info.buildingId = reader.GetInt32(2);
//                        info.objSquare = reader.GetDecimal(3);
//                        info.formOfOwnership = reader.IsDBNull(4) ? null : reader.GetValue(4);
//                        info.modifyDate = reader.IsDBNull(5) ? null : reader.GetValue(5);

//                        List<BalansObjectInfo> list = null;

//                        if (balans.TryGetValue(info.buildingId, out list))
//                        {
//                            list.Add(info);
//                        }
//                        else
//                        {
//                            list = new List<BalansObjectInfo>();

//                            list.Add(info);

//                            balans.Add(info.buildingId, list);
//                        }
//                    }

//                    reader.Close();
//                }
//            }

//            // Prepare the cache of rent agreements for each building we are interested in
//            query = @"SELECT
//                ar.id,
//                ar.org_renter_id,
//                ar.org_balans_id,
//                ar.building_id,
//                ar.rent_square,
//                ar.balans_id,
//                ar.agreement_date,
//                ar.agreement_num
//            FROM
//                arenda ar
//            WHERE
//                ar.building_id IN ($) AND
//                NOT (ar.org_renter_id IS NULL) AND
//                NOT (ar.org_balans_id IS NULL) AND
//                NOT (ar.building_id IS NULL) AND
//                NOT (ar.rent_square IS NULL) AND
//                NOT (ar.balans_id IS NULL) AND
//                NOT (ar.balans_id IN (SELECT id FROM balans WHERE is_deleted = 1))";

//            query = query.Replace("$", buildingIdList);

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        RentAgreementInfo info = new RentAgreementInfo();

//                        info.arendaId = reader.GetInt32(0);
//                        info.orgRenterId = reader.GetInt32(1);
//                        info.orgBalansId = reader.GetInt32(2);
//                        info.buildingId = reader.GetInt32(3);
//                        info.rentSquare = reader.GetDecimal(4);
//                        info.balansId = reader.GetInt32(5);
//                        info.agreementDate = reader.IsDBNull(6) ? null : reader.GetValue(6);
//                        info.agreementNum = reader.IsDBNull(7) ? null : reader.GetValue(7);

//                        List<RentAgreementInfo> list = null;

//                        if (arenda.TryGetValue(info.buildingId, out list))
//                        {
//                            list.Add(info);
//                        }
//                        else
//                        {
//                            list = new List<RentAgreementInfo>();

//                            list.Add(info);

//                            arenda.Add(info.buildingId, list);
//                        }
//                    }

//                    reader.Close();
//                }
//            }
//        }

//        private void FilterPrivatObjects(List<PrivatObjectInfo> privObjects,
//            Dictionary<int, List<BalansObjectInfo>> balans)
//        {
//            List<PrivatObjectInfo> privObjectsToProcess = new List<PrivatObjectInfo>();

//            foreach (PrivatObjectInfo privObject in privObjects)
//            {
//                bool matchingObjectFound = false;

//                // Check if this organization has an object with a similar square
//                List<BalansObjectInfo> balansObjects = null;

//                if (balans.TryGetValue(privObject.buildingId, out balansObjects))
//                {
//                    foreach (BalansObjectInfo balObj in balansObjects)
//                    {
//                        if (balObj.organizationId == privObject.organizationId &&
//                            (balObj.objSquare >= privObject.objSquare || SquareMatch(balObj.objSquare, privObject.objSquare)) &&
//                            balObj.IsPrivatObject())
//                        {
//                            matchingObjectFound = true;
//                            break;
//                        }

//                        // If there is a private object with exactly the same square - everything is OK
//                        if (balObj.objSquare == privObject.objSquare &&
//                            balObj.IsPrivatObject())
//                        {
//                            if (balObj.organizationId != privObject.organizationId)
//                            {
//                                AddCorrectionLogEntry(false,
//                                    PrivatSuspectDifferentOrg,
//                                    privObject.FormatToString(objectFinder, organizationFinder),
//                                    "",
//                                    Properties.Resources.CorrectionMsgWrongBalansHolder);
//                            }

//                            matchingObjectFound = true;
//                            break;
//                        }
//                    }
//                }

//                // Check if this organization has substitutions
//                if (!matchingObjectFound)
//                {
//                    HashSet<int> substitutions = null;

//                    if (orgSubstitutions.TryGetValue(privObject.organizationId, out substitutions))
//                    {
//                        List<BalansObjectInfo> balObjects = null;

//                        if (balans.TryGetValue(privObject.buildingId, out balObjects))
//                        {
//                            // For each substitution, try to find a corresponding object on balans
//                            foreach (int orgId in substitutions)
//                            {
//                                foreach (BalansObjectInfo balObj in balObjects)
//                                {
//                                    if (balObj.organizationId == orgId &&
//                                        balObj.IsPrivatObject() &&
//                                        (balObj.objSquare >= privObject.objSquare || SquareMatch(balObj.objSquare, privObject.objSquare)))
//                                    {
//                                        matchingObjectFound = true;
//                                        break;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }

//                // If no object with a similar organization found, save this PrivatObjectInfo for processing
//                if (!matchingObjectFound)
//                {
//                    // We can not process privatization objects that have no Akt
//                    if (privObject.aktDate is DateTime)
//                    {
//                        privObjectsToProcess.Add(privObject);
//                    }
//                    else
//                    {
//                        AddCorrectionLogEntry(false,
//                            PrivatSuspectNoAkt,
//                            privObject.FormatToString(objectFinder, organizationFinder),
//                            "",
//                            Properties.Resources.CorrectionMsgNoPrivatAkt);
//                    }
//                }
//            }

//            // Returns all the objects that passed validation
//            privObjects.Clear();
//            privObjects.AddRange(privObjectsToProcess);
//        }

//        private List<RentAgreementInfo> FindRentAgreements(PrivatObjectInfo privatObj,
//            List<BalansObjectInfo> balansHolders, Dictionary<int, List<RentAgreementInfo>> arenda)
//        {
//            List<RentAgreementInfo> agreements = new List<RentAgreementInfo>();
//            List<RentAgreementInfo> list = null;

//            if (arenda.TryGetValue(privatObj.buildingId, out list))
//            {
//                foreach (RentAgreementInfo agreement in list)
//                {
//                    // We need only objects rented by our organization, and
//                    // rented square must be equal or greater than obtained square
//                    if (OrgMatch(agreement.orgRenterId, privatObj.organizationId) &&
//                        (agreement.rentSquare >= privatObj.objSquare ||
//                         SquareMatch(agreement.rentSquare, privatObj.objSquare)))
//                    {
//                        // We need only balans holders listed in the provided array
//                        foreach (BalansObjectInfo balObj in balansHolders)
//                        {
//                            if (OrgMatch(agreement.orgBalansId, balObj.organizationId))
//                            {
//                                agreements.Add(agreement);
//                                break;
//                            }
//                        }
//                    }
//                }
//            }

//            return agreements;
//        }

//        private int CalculateBalansObjCountForRentAgreements(List<RentAgreementInfo> rentAgreements)
//        {
//            HashSet<int> balansObjects = new HashSet<int>();

//            foreach (RentAgreementInfo agreement in rentAgreements)
//            {
//                balansObjects.Add(agreement.balansId);
//            }

//            return balansObjects.Count;
//        }

//        private void UpdatePrivatFromRentAgreement(Dictionary<int, List<BalansObjectInfo>> balans,
//            PrivatObjectInfo privatObj, RentAgreementInfo agreement)
//        {
//            using (FbTransaction transaction = connection1NF.BeginTransaction())
//            {
//                try
//                {
//                    bool dateMismatch = false;

//                    BalansObjectInfo balansObj = FindBalansInfo(balans, privatObj.buildingId, agreement.balansId);

//                    if (balansObj == null)
//                    {
//                        balansObj = GetBalansObjectInfo(agreement.balansId);
//                    }

//                    if (balansObj != null)
//                    {
//                        decimal squareDiff = balansObj.objSquare - privatObj.objSquare;

//                        if (squareDiff >= 50m)
//                        {
//                            // The source object is big enough to cut some square from it
//                            if (CutBalansObject(balansObj, privatObj.objSquare, privatObj.AktDate,
//                                    privatObj.FormatToString(objectFinder, organizationFinder),
//                                    agreement.FormatToString(objectFinder, organizationFinder),
//                                    transaction, out dateMismatch))
//                            {
//                                AddBalansObjFromPrivatObj(privatObj, agreement, transaction);
//                            }
//                            else
//                            {
//                                if (dateMismatch)
//                                {
//                                    AddCorrectionLogEntry(false,
//                                        PrivatSuspectBalansIsUpToDate,
//                                        privatObj.FormatToString(objectFinder, organizationFinder),
//                                        "",
//                                        string.Format(
//                                            Properties.Resources.CorrectionMsgBalansUpToDate,
//                                            balansObj.FormatToString(objectFinder, organizationFinder)));
//                                }
//                                else
//                                {
//                                    ReplaceBalansHolder(balansObj, privatObj.organizationId, ownershipTypePrivat,
//                                        privatObj.FormatToString(objectFinder, organizationFinder),
//                                        agreement.FormatToString(objectFinder, organizationFinder),
//                                        transaction,
//                                        privatObj.AktDate,
//                                        out dateMismatch);
//                                }
//                            }
//                        }
//                        else if (SquareMatch(balansObj.objSquare, privatObj.objSquare))
//                        {
//                            if (!ReplaceBalansHolder(balansObj, privatObj.organizationId, ownershipTypePrivat,
//                                    privatObj.FormatToString(objectFinder, organizationFinder),
//                                    agreement.FormatToString(objectFinder, organizationFinder),
//                                    transaction,
//                                    privatObj.AktDate,
//                                    out dateMismatch))
//                            {
//                                AddCorrectionLogEntry(false,
//                                    PrivatSuspectBalansIsUpToDate,
//                                    privatObj.FormatToString(objectFinder, organizationFinder),
//                                    "",
//                                    string.Format(
//                                        Properties.Resources.CorrectionMsgBalansUpToDate,
//                                        balansObj.FormatToString(objectFinder, organizationFinder)));
//                            }
//                        }
//                        else
//                        {
//                            AddCorrectionLogEntry(false,
//                                PrivatSuspectRentAgreementsUndefined,
//                                privatObj.FormatToString(objectFinder, organizationFinder),
//                                "",
//                                string.Format(Properties.Resources.CorrectionMsgWrongRentAgreement,
//                                    organizationFinder.GetOrgFullNameById(privatObj.organizationId)));
//                        }
//                    }

//                    // Commit the transaction
//                    transaction.Commit();
//                }
//                catch (Exception ex)
//                {
//                    logger.WriteError("Exception in UpdatePrivatFromRentAgreement() function: " + ex.Message);

//                    // Roll back the transaction
//                    transaction.Rollback();
//                }
//            }
//        }

//        private BalansObjectInfo FindBalansInfo(Dictionary<int, List<BalansObjectInfo>> balans, int buildingId, int balansId)
//        {
//            List<BalansObjectInfo> balansObjects = null;

//            if (balans.TryGetValue(buildingId, out balansObjects))
//            {
//                foreach (BalansObjectInfo info in balansObjects)
//                {
//                    if (info.balansId == balansId)
//                    {
//                        return info;
//                    }
//                }
//            }

//            return null;
//        }

//        private bool SquareMatch(object square1, object square2)
//        {
//            if (square1 is decimal && square2 is decimal)
//            {
//                decimal s1 = (decimal)square1;
//                decimal s2 = (decimal)square2;

//                if (s1 == 0)
//                {
//                    return s2 == 0;
//                }
//                else if (s2 == 0)
//                {
//                    return false; // we know that s1 is non-zero; this if verified earlier
//                }
//                else
//                {
//                    decimal diff = Math.Abs(s1 - s2);

//                    return (diff / s1 < allowedSquareDeviationPercent) && (diff / s2 < allowedSquareDeviationPercent);
//                }
//            }

//            return false;
//        }

//        private bool OrgMatch(int organization1, int organization2)
//        {
//            if (organization1 == organization2)
//            {
//                return true;
//            }

//            // Thre may be substitutions for this organization
//            HashSet<int> substitutions = null;

//            if (orgSubstitutions.TryGetValue(organization1, out substitutions))
//            {
//                foreach (int substOrg in substitutions)
//                {
//                    if (substOrg == organization2)
//                    {
//                        return true;
//                    }
//                }
//            }

//            return false;
//        }

//        private BalansObjectInfo GetBalansObjectInfo(int balansId)
//        {
//            BalansObjectInfo info = null;

//            string query = @"SELECT
//                b.balans_id,
//                b.organization_id,
//                b.building_id,
//                b.sqr_total,
//                b.org_ownership_int,
//                b.input_date
//            FROM
//                view_balans b
//            WHERE
//                NOT (b.sqr_total IS NULL) AND b.balans_id = " + balansId.ToString();

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    if (reader.Read())
//                    {
//                        info = new BalansObjectInfo();

//                        info.balansId = reader.GetInt32(0);
//                        info.organizationId = reader.GetInt32(1);
//                        info.buildingId = reader.GetInt32(2);
//                        info.objSquare = reader.GetDecimal(3);
//                        info.formOfOwnership = reader.IsDBNull(4) ? null : reader.GetValue(4);
//                        info.modifyDate = reader.IsDBNull(5) ? null : reader.GetValue(5);
//                    }

//                    reader.Close();
//                }
//            }

//            return info;
//        }

//        private void DumpQueryParameters(Dictionary<string, object> parameters)
//        {
//            foreach (KeyValuePair<string, object> pair in parameters)
//            {
//                if (pair.Value != null)
//                {
//                    logger.WriteError("Parameter " + pair.Key + " = " + pair.Value.ToString());
//                }
//                else
//                {
//                    logger.WriteError("Parameter " + pair.Key + " = NULL");
//                }
//            }
//        }

//        #endregion (Privatization data validation)

//        #region Data corrections

//        private void AddBalansObjFromPrivatObj(PrivatObjectInfo privatObj, RentAgreementInfo agreement, FbTransaction transaction)
//        {
//            // Make sure that organization is well-defined (it has ZKPO code and full name)
//            OrganizationInfo orgInfo = organizationFinder.GetOrgInfoById(privatObj.organizationId);

//            bool organizationValid = orgInfo != null && orgInfo.zkpoCode.Length > 0 && orgInfo.fullName.Length > 0;

//            if (!organizationValid)
//                return;

//            Dictionary<string, object> values = new Dictionary<string, object>();
//            Dictionary<string, string> mapping = new Dictionary<string, string>();

//            FieldMappings.Create1NFBalansFieldMapping(mapping, testOnly, enable1NFNewFieldsImport);

//            // Add all the properties we can take from 'privatization' table
//            values.Add("OBJECT", privatObj.buildingId);
//            values.Add("SQR_ZAG", privatObj.objSquare);
//            values.Add("SQR_NEJ", privatObj.objSquare);

//            if (privatObj.cost != null)
//            {
//                values.Add("BALANS_VARTIST", privatObj.cost);
//                values.Add("ZAL_VART", privatObj.cost);
//            }

//            if (privatObj.costExpert != null)
//                values.Add("VARTIST_EXP_V", privatObj.costExpert);

//            if (privatObj.floor != null)
//                values.Add("FLOATS", privatObj.floor.ToString());

//            values.Add("PRIZNAK1NF", 0);
//            values.Add("UPD_SOURCE", 8); // 8 = Privatization
//            values.Add("DELETED", 0);
//            values.Add("ISP", AutoUserName);
//            values.Add("DT", DateTime.Now.Date);
//            values.Add("EDT", DateTime.Now.Date);
//            values.Add("PRIV_COUNT", 1);
//            values.Add("LYEAR", DateTime.Now.Year);
//            values.Add("FORM_VLASN", ownershipTypePrivat);
//            values.Add("PRAVO", 1); // 'Ownership' right
//            values.Add("STAN", DateTime.Now.Date);
//            values.Add("OBJECT_STAN", 1);
//            values.Add("ORG_STAN", 1);

//            if (privatObj.objectKind is int && CanMapDictionaryValue1NF("SKINDOBJ", (int)privatObj.objectKind))
//                values.Add("KINDOBJ", privatObj.objectKind);

//            if (privatObj.objectType is int && CanMapDictionaryValue1NF("STYPEOBJ", (int)privatObj.objectType))
//                values.Add("TYPEOBJ", privatObj.objectType);

//            if (privatObj.history is int && CanMapDictionaryValue1NF("SHISTORY", (int)privatObj.history))
//                values.Add("HISTORY", privatObj.history);

//            if (privatObj.purposeGroup is int && CanMapDictionaryValue1NF("SGRPURPOSE", (int)privatObj.purposeGroup))
//                values.Add("GRPURP", privatObj.purposeGroup);

//            if (privatObj.name != null)
//                values.Add("PURP_STR", privatObj.name);

//            // Get information about the object
//            string query = @"SELECT
//                addr_street_name,
//                addr_street_id,
//                addr_street_name2,
//                addr_street_id2,
//                addr_nomer1,
//                addr_nomer2,
//                addr_nomer3,
//                addr_misc,
//                tech_condition_id,
//                object_type_id,
//                object_kind_id
//            FROM buildings WHERE id = @bid";

//            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//            {
//                cmd.Parameters.Add(new SqlParameter("bid", privatObj.buildingId));

//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    if (reader.Read())
//                    {
//                        if (!reader.IsDBNull(1))
//                            values.Add("OBJ_KODUL", reader.GetValue(1));

//                        if (!reader.IsDBNull(3))
//                            values.Add("ULKOD2", reader.GetValue(3));

//                        if (!reader.IsDBNull(0))
//                            values.Add("OBJ_ULNAME", reader.GetValue(0));

//                        if (!reader.IsDBNull(2))
//                            values.Add("ULNAME2", reader.GetValue(2));

//                        if (!reader.IsDBNull(4))
//                            values.Add("OBJ_NOMER1", reader.GetValue(4));

//                        if (!reader.IsDBNull(5))
//                            values.Add("OBJ_NOMER2", reader.GetValue(5));

//                        if (!reader.IsDBNull(6))
//                            values.Add("OBJ_NOMER3", reader.GetValue(6));

//                        if (!reader.IsDBNull(7))
//                            values.Add("OBJ_ADRDOP", reader.GetValue(7));

//                        if (!reader.IsDBNull(8) && CanMapDictionaryValue1NF("STEXSTAN", reader.GetInt32(8)))
//                            values.Add("TEXSTAN", reader.GetValue(8));
//                    }

//                    reader.Close();
//                }
//            }

//            try
//            {
//                // Generate new balans Id
//                int newBalansId = GenerateId1NFUsingGenerator("GEN_BALANS_1NF", transaction);

//                if (newBalansId > 0)
//                {
//                    values.Add("ID", newBalansId);

//                    // Make sure that organization exists in 1NF
//                    int newOrgId = GetOrCreateOrganization1NF(privatObj.organizationId, transaction);

//                    if (newOrgId >= 0)
//                    {
//                        values.Add("ORG", newOrgId);

//                        // Generate parameters for INSERT statement
//                        Dictionary<string, object> parameters1NF = new Dictionary<string, object>();
//                        Dictionary<string, object> parametersSQL = new Dictionary<string, object>();

//                        int paramIndex = 1;
//                        string insertFieldList1NF = "";
//                        string insertFieldListSQL = "";
//                        string insertParamList1NF = "";
//                        string insertParamListSQL = "";

//                        foreach (KeyValuePair<string, object> pair in values)
//                        {
//                            string paramName = "p" + paramIndex.ToString();

//                            if (paramIndex > 1)
//                            {
//                                insertFieldList1NF += ", ";
//                                insertParamList1NF += ", ";
//                            }

//                            insertFieldList1NF += pair.Key;
//                            insertParamList1NF += "@" + paramName;

//                            parameters1NF[paramName] = pair.Value;

//                            // Generate the list of fields for Sql Server INSERT statement as well
//                            foreach (KeyValuePair<string, string> mappingPair in mapping)
//                            {
//                                if (mappingPair.Key == pair.Key)
//                                {
//                                    if (insertFieldListSQL.Length > 0)
//                                    {
//                                        insertFieldListSQL += ", ";
//                                        insertParamListSQL += ", ";
//                                    }

//                                    insertFieldListSQL += mappingPair.Value;
//                                    insertParamListSQL += "@" + paramName;

//                                    if (pair.Key == "ORG")
//                                    {
//                                        parametersSQL[paramName] = privatObj.organizationId;
//                                    }
//                                    else
//                                    {
//                                        parametersSQL[paramName] = pair.Value;
//                                    }

//                                    break;
//                                }
//                            }

//                            paramIndex++;
//                        }

//                        // Prepare the INSERT statement
//                        query = "INSERT INTO BALANS_1NF (" + insertFieldList1NF + ") VALUES (" + insertParamList1NF + ")";

//                        try
//                        {
//                            using (FbCommand commandInsert = new FbCommand(query, connection1NF))
//                            {
//                                foreach (KeyValuePair<string, object> pair in parameters1NF)
//                                {
//                                    commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
//                                }

//                                commandInsert.Transaction = transaction;
//                                commandInsert.ExecuteNonQuery();
//                            }

//                            // Make the same change in SQL Server
//                            query = "INSERT INTO balans (" + insertFieldListSQL + ") VALUES (" + insertParamListSQL + ")";

//                            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                            {
//                                foreach (KeyValuePair<string, object> pair in parametersSQL)
//                                {
//                                    cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
//                                }

//                                cmd.ExecuteNonQuery();
//                            }

//                            // Add the change to the log
//                            AddCorrectionLogEntry(true,
//                                PrivatCorrectionAddBalansObject,
//                                privatObj.FormatToString(objectFinder, organizationFinder),
//                                Properties.Resources.CorrectionMsgBalansObjCreatedFromPrivat,
//                                agreement.FormatToString(objectFinder, organizationFinder));
//                        }
//                        catch (Exception)
//                        {
//                            logger.WriteError("SQL query failed: " + query);
//                            DumpQueryParameters(parameters1NF);
//                            throw;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                logger.WriteError("Exception in PrivatAddBalansObjFromPrivatObj() function: " + ex.Message);
//                throw;
//            }
//        }

//        private void AddBalansObjFromTransfer(ObjTransfer transfer, int formOfOwnership, FbTransaction transaction)
//        {
//            formOfOwnership = GetMapped1NFOwnershipType(formOfOwnership);

//            // Make sure that receiving organization is well-defined (it has ZKPO code and full name)
//            OrganizationInfo orgInfo = organizationFinder.GetOrgInfoById(transfer.orgToId);

//            bool organizationValid = orgInfo != null && orgInfo.zkpoCode.Length > 0 && orgInfo.fullName.Length > 0;

//            if (!organizationValid)
//                return;

//            // Check if we need to create the object in 1NF
//            int buildingId1NF = GetOrCreateObject1NF(transfer.buildingId, transaction);

//            if (buildingId1NF > 0)
//            {
//                Dictionary<string, object> values = new Dictionary<string, object>();
//                Dictionary<string, string> mapping = new Dictionary<string, string>();

//                FieldMappings.Create1NFBalansFieldMapping(mapping, testOnly, enable1NFNewFieldsImport);

//                // Add all the properties we can take from 'privatization' table
//                values.Add("OBJECT", buildingId1NF);
//                values.Add("SQR_ZAG", transfer.objSquare);

//                // This may be a habitable or non-habitable building
//                if (transfer.purpose is int)
//                {
//                    if (IntArrayContains(transferPurposeLivingBuildings, (int)transfer.purpose))
//                    {
//                        // This is a living building; no non-habitable square
//                        values.Add("SQR_NEJ", 0m);
//                    }
//                    else
//                    {
//                        values.Add("SQR_NEJ", transfer.objSquare);
//                    }
//                }
//                else
//                {
//                    values.Add("SQR_NEJ", transfer.objSquare);
//                }

//                if (transfer.sumBalans != null)
//                    values.Add("BALANS_VARTIST", transfer.sumBalans);

//                if (transfer.sumZalishkova != null)
//                    values.Add("ZAL_VART", transfer.sumZalishkova);

//                if (transfer.miscInfo != null)
//                    values.Add("FLOATS", transfer.miscInfo);

//                values.Add("PRIZNAK1NF", 0);
//                values.Add("UPD_SOURCE", 10); // 10 = 'Rozporadjennia' viddil
//                values.Add("DELETED", 0);
//                values.Add("ISP", AutoUserName);
//                values.Add("DT", DateTime.Now.Date);
//                values.Add("EDT", DateTime.Now.Date);
//                values.Add("PRIV_COUNT", 1);
//                values.Add("LYEAR", DateTime.Now.Year);
//                values.Add("FORM_VLASN", formOfOwnership);
//                values.Add("STAN", DateTime.Now.Date);
//                values.Add("OBJECT_STAN", 1);
//                values.Add("ORG_STAN", 1);

//                int ownershipType = transfer.GetBalansOwnershipType();

//                if (ownershipType > 0)
//                    values.Add("PRAVO", ownershipType);

//                if (transfer.objectKind is int && CanMapDictionaryValue1NF("SKINDOBJ", (int)transfer.objectKind))
//                    values.Add("KINDOBJ", transfer.objectKind);

//                if (transfer.objectType is int && CanMapDictionaryValue1NF("STYPEOBJ", (int)transfer.objectType))
//                    values.Add("TYPEOBJ", transfer.objectType);

//                if (transfer.condition is int && CanMapDictionaryValue1NF("STEXSTAN", (int)transfer.condition))
//                    values.Add("TEXSTAN", transfer.condition);

//                if (transfer.purposeGroup is int && CanMapDictionaryValue1NF("SGRPURPOSE", (int)transfer.purposeGroup))
//                    values.Add("GRPURP", transfer.purposeGroup);

//                if (transfer.purpose is int && CanMapDictionaryValue1NF("SPURPOSE", (int)transfer.purpose))
//                    values.Add("PURPOSE", transfer.purpose);

//                if (transfer.name != null)
//                    values.Add("PURP_STR", transfer.name);

//                // Get information about the object
//                string query = @"SELECT
//                    addr_street_name,
//                    addr_street_id,
//                    addr_street_name2,
//                    addr_street_id2,
//                    addr_nomer1,
//                    addr_nomer2,
//                    addr_nomer3,
//                    addr_misc,
//                    tech_condition_id,
//                    object_type_id,
//                    object_kind_id
//                FROM buildings WHERE id = @bid";

//                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                {
//                    cmd.Parameters.Add(new SqlParameter("bid", transfer.buildingId));

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            if (!reader.IsDBNull(1))
//                                values.Add("OBJ_KODUL", reader.GetValue(1));

//                            if (!reader.IsDBNull(3))
//                                values.Add("ULKOD2", reader.GetValue(3));

//                            if (!reader.IsDBNull(0))
//                                values.Add("OBJ_ULNAME", reader.GetValue(0));

//                            if (!reader.IsDBNull(2))
//                                values.Add("ULNAME2", reader.GetValue(2));

//                            if (!reader.IsDBNull(4))
//                                values.Add("OBJ_NOMER1", reader.GetValue(4));

//                            if (!reader.IsDBNull(5))
//                                values.Add("OBJ_NOMER2", reader.GetValue(5));

//                            if (!reader.IsDBNull(6))
//                                values.Add("OBJ_NOMER3", reader.GetValue(6));

//                            if (!reader.IsDBNull(7))
//                                values.Add("OBJ_ADRDOP", reader.GetValue(7));

//                            if (transfer.condition == null)
//                            {
//                                if (!reader.IsDBNull(8) && CanMapDictionaryValue1NF("STEXSTAN", reader.GetInt32(8)))
//                                    values.Add("TEXSTAN", reader.GetValue(8));
//                            }
//                        }

//                        reader.Close();
//                    }
//                }

//                try
//                {
//                    // Generate new balans Id
//                    int newBalansId = GenerateId1NFUsingGenerator("GEN_BALANS_1NF", transaction);

//                    if (newBalansId > 0)
//                    {
//                        values.Add("ID", newBalansId);

//                        // Make sure that organization exists in 1NF
//                        int newOrgId = GetOrCreateOrganization1NF(transfer.orgToId, transaction);

//                        if (newOrgId >= 0)
//                        {
//                            values.Add("ORG", newOrgId);

//                            // Generate parameters for INSERT statement
//                            Dictionary<string, object> parameters1NF = new Dictionary<string, object>();
//                            Dictionary<string, object> parametersSQL = new Dictionary<string, object>();

//                            int paramIndex = 1;
//                            string insertFieldList1NF = "";
//                            string insertFieldListSQL = "";
//                            string insertParamList1NF = "";
//                            string insertParamListSQL = "";

//                            foreach (KeyValuePair<string, object> pair in values)
//                            {
//                                string paramName = "p" + paramIndex.ToString();

//                                if (paramIndex > 1)
//                                {
//                                    insertFieldList1NF += ", ";
//                                    insertParamList1NF += ", ";
//                                }

//                                insertFieldList1NF += pair.Key;
//                                insertParamList1NF += "@" + paramName;

//                                parameters1NF[paramName] = pair.Value;

//                                // Generate the list of fields for Sql Server INSERT statement as well
//                                foreach (KeyValuePair<string, string> mappingPair in mapping)
//                                {
//                                    if (mappingPair.Key == pair.Key)
//                                    {
//                                        if (insertFieldListSQL.Length > 0)
//                                        {
//                                            insertFieldListSQL += ", ";
//                                            insertParamListSQL += ", ";
//                                        }

//                                        insertFieldListSQL += mappingPair.Value;
//                                        insertParamListSQL += "@" + paramName;

//                                        if (pair.Key == "ORG")
//                                        {
//                                            parametersSQL[paramName] = transfer.orgToId;
//                                        }
//                                        else if (pair.Key == "OBJECT")
//                                        {
//                                            parametersSQL[paramName] = transfer.buildingId;
//                                        }
//                                        else
//                                        {
//                                            parametersSQL[paramName] = pair.Value;
//                                        }

//                                        break;
//                                    }
//                                }

//                                paramIndex++;
//                            }

//                            // Prepare the INSERT statement
//                            query = "INSERT INTO BALANS_1NF (" + insertFieldList1NF + ") VALUES (" + insertParamList1NF + ")";

//                            try
//                            {
//                                using (FbCommand commandInsert = new FbCommand(query, connection1NF))
//                                {
//                                    foreach (KeyValuePair<string, object> pair in parameters1NF)
//                                    {
//                                        commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
//                                    }

//                                    commandInsert.Transaction = transaction;
//                                    commandInsert.ExecuteNonQuery();
//                                }

//                                // Make the same change in our database
//                                query = "INSERT INTO balans (" + insertFieldListSQL + ") VALUES (" + insertParamListSQL + ")";

//                                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                                {
//                                    foreach (KeyValuePair<string, object> pair in parametersSQL)
//                                    {
//                                        cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
//                                    }

//                                    cmd.ExecuteNonQuery();
//                                }

//                                // Write the change to the log
//                                AddCorrectionLogEntry(true,
//                                    TransferCorrectionAddBalansObject,
//                                    transfer.FormatToString(objectFinder, organizationFinder),
//                                    Properties.Resources.CorrectionMsgBalansObjCreatedFromTransfer,
//                                    Properties.Resources.CorrectionReasonTransfer);
//                            }
//                            catch (Exception ex)
//                            {
//                                logger.WriteError("SQL query failed: " + query);
//                                logger.WriteError("Message: " + ex.Message);
//                                DumpQueryParameters(parameters1NF);
//                                throw;
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    logger.WriteError("Exception in AddBalansObjFromTransfer() function: " + ex.Message);
//                    throw;
//                }
//            }
//        }

//        private bool CutBalansObject(BalansObjectInfo balansObj, decimal squareToCut, object infoDate,
//            string sourceObjDescr, string reasonDescr, FbTransaction transaction, out bool dateMismatch)
//        {
//            dateMismatch = false;

//            // Get the current square for this object
//            decimal totalSqr = 0;
//            decimal nonHabitSqr = 0;
//            object balansObjDate = null;

//            string query = @"SELECT SQR_ZAG, SQR_NEJ, EDT FROM BALANS_1NF WHERE ID = @bid";

//            using (FbCommand cmd = new FbCommand(query, connection1NF))
//            {
//                cmd.Parameters.Add(new FbParameter("bid", balansObj.balansId));
//                cmd.Transaction = transaction;

//                using (FbDataReader reader = cmd.ExecuteReader())
//                {
//                    if (reader.Read())
//                    {
//                        totalSqr = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
//                        nonHabitSqr = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
//                        balansObjDate = reader.IsDBNull(2) ? null : reader.GetValue(2);
//                    }

//                    reader.Close();
//                }
//            }

//            // Check the dates; balans object must be older than privatization object
//            if (balansObjDate is DateTime && infoDate is DateTime)
//            {
//                if ((DateTime)balansObjDate >= (DateTime)infoDate)
//                {
//                    dateMismatch = true;
//                    return false;
//                }
//            }

//            // Update the square
//            if (nonHabitSqr >= totalSqr)
//            {
//                totalSqr -= squareToCut;
//                nonHabitSqr -= squareToCut;
//            }
//            else
//            {
//                totalSqr -= squareToCut;
//            }

//            totalSqr = (totalSqr < 0) ? 0 : totalSqr;
//            nonHabitSqr = (nonHabitSqr < 0) ? 0 : nonHabitSqr;

//            if (totalSqr >= 1m)
//            {
//                // Create an archived state if necessary
//                int archRecordId = archiver1NF.CreateBalansArchiveRecord(connection1NF, balansObj.balansId, transaction);

//                if (archRecordId > 0)
//                {
//                    // Update the square of this object
//                    query = @"UPDATE BALANS_1NF SET SQR_ZAG = @stot, SQR_NEJ = @snon, ARCH_KOD = @acod, ISP = @isp, DT = @dt, EDT = @dt, UPD_SOURCE = 8 WHERE ID = @bid";

//                    using (FbCommand cmd = new FbCommand(query, connection1NF))
//                    {
//                        cmd.Parameters.Add(new FbParameter("stot", totalSqr));
//                        cmd.Parameters.Add(new FbParameter("snon", nonHabitSqr));
//                        cmd.Parameters.Add(new FbParameter("acod", archRecordId));
//                        cmd.Parameters.Add(new FbParameter("isp", AutoUserName));
//                        cmd.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
//                        cmd.Parameters.Add(new FbParameter("bid", balansObj.balansId));

//                        cmd.Transaction = transaction;
//                        cmd.ExecuteNonQuery();
//                    }

//                    // Make the same change in our database
//                    query = @"UPDATE balans SET sqr_total = @stot, sqr_non_habit = @snon, arch_id = @acod, modified_by = @isp, modify_date = @dt, update_src_id = 8 WHERE id = @bid";

//                    using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                    {
//                        cmd.Parameters.Add(new SqlParameter("stot", totalSqr));
//                        cmd.Parameters.Add(new SqlParameter("snon", nonHabitSqr));
//                        cmd.Parameters.Add(new SqlParameter("acod", archRecordId));
//                        cmd.Parameters.Add(new SqlParameter("isp", AutoUserName));
//                        cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));
//                        cmd.Parameters.Add(new SqlParameter("bid", balansObj.balansId));

//                        cmd.ExecuteNonQuery();
//                    }

//                    // Write the change to the log
//                    AddCorrectionLogEntry(true, PrivatCorrectionCutBalansObject,
//                        sourceObjDescr,
//                        string.Format(Properties.Resources.CorrectionMsgBalansSqrCorrected,
//                            balansObj.FormatToString(objectFinder, organizationFinder),
//                            totalSqr.ToString()),
//                        reasonDescr);
//                }

//                return true;
//            }

//            return false;
//        }

//        private void CorrectBalansOwnership(BalansObjectInfo balansObj, object infoDate, int newFormOfOwnership, string sourceObjDescr, bool fromPrivatizationDB)
//        {
//            newFormOfOwnership = GetMapped1NFOwnershipType(newFormOfOwnership);

//            // Get description of the new form of ownership
//            string newFormOfOwnershipText = "";
//            dictFormOfOwnership.TryGetValue(newFormOfOwnership, out newFormOfOwnershipText);

//            if (newFormOfOwnershipText.Length == 0)
//                newFormOfOwnershipText = newFormOfOwnership.ToString();

//            // Maybe there is no need to correct anything
//            if (IsPrivateOwnership(newFormOfOwnership) && balansObj.IsPrivatObject())
//            {
//                // No need to correct
//            }
//            else if (IsCommunityOwnership(newFormOfOwnership) && balansObj.IsCommunityObject())
//            {
//                // No need to correct
//            }
//            else if (balansObj.IsFormOwnershipEqual(newFormOfOwnership) || balansObj.IsFormOwnershipEqual(ownershipTypeDerzhavna))
//            {
//                // No need to correct
//            }
//            else
//            {
//                // Check the dates; balans object must be older than privatization object
//                bool dateMismatch = false;

//                if (balansObj.modifyDate is DateTime && infoDate is DateTime)
//                {
//                    if ((DateTime)balansObj.modifyDate >= (DateTime)infoDate)
//                    {
//                        dateMismatch = true;
//                    }
//                }

//                if (!dateMismatch)
//                {
//                    using (FbTransaction transaction = connection1NF.BeginTransaction())
//                    {
//                        try
//                        {
//                            // Create an archived state if necessary
//                            int archRecordId = archiver1NF.CreateBalansArchiveRecord(connection1NF, balansObj.balansId, transaction);

//                            if (archRecordId > 0)
//                            {
//                                string query = @"UPDATE BALANS_1NF SET FORM_VLASN = @fvl, ARCH_KOD = @acod, ISP = @isp, DT = @dt, EDT = @dt, UPD_SOURCE = 8 WHERE ID = @bid";

//                                using (FbCommand cmd = new FbCommand(query, connection1NF))
//                                {
//                                    cmd.Parameters.Add(new FbParameter("fvl", newFormOfOwnership));
//                                    cmd.Parameters.Add(new FbParameter("acod", archRecordId));
//                                    cmd.Parameters.Add(new FbParameter("isp", AutoUserName));
//                                    cmd.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
//                                    cmd.Parameters.Add(new FbParameter("bid", balansObj.balansId));

//                                    cmd.Transaction = transaction;
//                                    cmd.ExecuteNonQuery();
//                                }

//                                // Make the same change in our database (SQL Server)
//                                query = @"UPDATE balans SET form_ownership_id = @fvl, arch_id = @acod, modified_by = @isp, modify_date = @dt, update_src_id = 8 WHERE id = @bid";

//                                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                                {
//                                    cmd.Parameters.Add(new SqlParameter("fvl", newFormOfOwnership));
//                                    cmd.Parameters.Add(new SqlParameter("acod", archRecordId));
//                                    cmd.Parameters.Add(new SqlParameter("isp", AutoUserName));
//                                    cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));
//                                    cmd.Parameters.Add(new SqlParameter("bid", balansObj.balansId));

//                                    cmd.ExecuteNonQuery();
//                                }

//                                // Write the change to the log
//                                AddCorrectionLogEntry(true,
//                                    PrivatCorrectionOwnership,
//                                    sourceObjDescr,
//                                    Properties.Resources.CorrectionMsgOwnershipChanged,
//                                    Properties.Resources.CorrectionMsgFromPrivatization);
//                            }

//                            // Commit the transaction
//                            transaction.Commit();
//                        }
//                        catch (Exception ex)
//                        {
//                            logger.WriteError("Exception in PrivatCorrectOwnership() function: " + ex.Message);

//                            // Rollback the transaction
//                            transaction.Rollback();
//                        }
//                    }
//                }
//            }
//        }

//        private bool ReplaceBalansHolder(BalansObjectInfo balansObj, int organizationId, int newFormOfOwnership,
//            string objectDescr, string reasonDescr, FbTransaction transaction, object infoDate, out bool dateMismatch)
//        {
//            dateMismatch = false;

//            newFormOfOwnership = GetMapped1NFOwnershipType(newFormOfOwnership);

//            // Get the modify date for this Balans object
//            object balansObjDate = null;

//            using (FbCommand cmd = new FbCommand("SELECT EDT FROM BALANS_1NF WHERE ID = @bid", connection1NF))
//            {
//                cmd.Parameters.Add(new FbParameter("bid", balansObj.balansId));
//                cmd.Transaction = transaction;

//                using (FbDataReader r = cmd.ExecuteReader())
//                {
//                    if (r.Read())
//                    {
//                        balansObjDate = r.IsDBNull(0) ? null : r.GetValue(0);
//                    }

//                    r.Close();
//                }
//            }

//            // Check the dates; balans object must be older than privatization object
//            if (balansObjDate is DateTime && infoDate is DateTime)
//            {
//                if ((DateTime)balansObjDate >= (DateTime)infoDate)
//                {
//                    dateMismatch = true;
//                    return false;
//                }
//            }

//            try
//            {
//                // Make sure that organization is well-defined (it has ZKPO code and full name)
//                OrganizationInfo orgInfo = organizationFinder.GetOrgInfoById(organizationId);

//                if (orgInfo != null && orgInfo.zkpoCode.Length > 0 && orgInfo.fullName.Length > 0)
//                {
//                    // Generate this organization in 1NF, if needed
//                    int newOrgId = GetOrCreateOrganization1NF(organizationId, transaction);

//                    if (newOrgId > 0)
//                    {
//                        // Create an archived state if necessary
//                        int archRecordId = archiver1NF.CreateBalansArchiveRecord(connection1NF, balansObj.balansId, transaction);

//                        if (archRecordId > 0)
//                        {
//                            // Update the balans entry
//                            string query = @"UPDATE BALANS_1NF SET ORG = @orgid, $ ARCH_KOD = @acod, ISP = @isp, DT = @dt, EDT = @dt, UPD_SOURCE = 8 WHERE ID = @bid";

//                            if (newFormOfOwnership > 0)
//                            {
//                                query = query.Replace("$", "FORM_VLASN = " + newFormOfOwnership.ToString() + ", ");
//                            }
//                            else
//                            {
//                                query = query.Replace("$", "");
//                            }

//                            using (FbCommand cmd = new FbCommand(query, connection1NF))
//                            {
//                                cmd.Parameters.Add(new FbParameter("orgid", newOrgId));
//                                cmd.Parameters.Add(new FbParameter("acod", archRecordId));
//                                cmd.Parameters.Add(new FbParameter("isp", AutoUserName));
//                                cmd.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
//                                cmd.Parameters.Add(new FbParameter("bid", balansObj.balansId));

//                                cmd.Transaction = transaction;
//                                cmd.ExecuteNonQuery();
//                            }

//                            // Make the same change in our database
//                            query = @"UPDATE balans SET organization_id = @orgid, $ arch_id = @acod, modified_by = @isp, modify_date = @dt, update_src_id = 8 WHERE id = @bid";

//                            if (newFormOfOwnership > 0)
//                            {
//                                query = query.Replace("$", "form_ownership_id = " + newFormOfOwnership.ToString() + ", ");
//                            }
//                            else
//                            {
//                                query = query.Replace("$", "");
//                            }

//                            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient))
//                            {
//                                cmd.Parameters.Add(new SqlParameter("orgid", organizationId));
//                                cmd.Parameters.Add(new SqlParameter("acod", archRecordId));
//                                cmd.Parameters.Add(new SqlParameter("isp", AutoUserName));
//                                cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));
//                                cmd.Parameters.Add(new SqlParameter("bid", balansObj.balansId));

//                                cmd.ExecuteNonQuery();
//                            }

//                            // Write the change to the log
//                            if (newFormOfOwnership > 0)
//                            {
//                                string ownership = "";

//                                if (!dictFormOfOwnership.TryGetValue(newFormOfOwnership, out ownership))
//                                {
//                                    ownership = "???";
//                                }

//                                AddCorrectionLogEntry(true,
//                                    PrivatCorrectionReplaceBalansHolder,
//                                    objectDescr,
//                                    string.Format(Properties.Resources.CorrectionMsgBalansObjNewHolderAndOwnership,
//                                        balansObj.FormatToString(objectFinder, organizationFinder),
//                                        organizationFinder.GetOrgFullNameById(organizationId),
//                                        ownership),
//                                    reasonDescr);
//                            }
//                            else
//                            {
//                                AddCorrectionLogEntry(true,
//                                    PrivatCorrectionReplaceBalansHolder,
//                                    objectDescr,
//                                    string.Format(Properties.Resources.CorrectionMsgBalansObjNewHolder,
//                                        balansObj.FormatToString(objectFinder, organizationFinder),
//                                        organizationFinder.GetOrgFullNameById(organizationId)),
//                                    reasonDescr);
//                            }

//                            return true;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                logger.WriteError("Exception in PrivatReplaceBalansHolder() function: " + ex.Message);
//                throw;
//            }

//            return false;
//        }

//        private int GetOrCreateObject1NF(int buildingId, FbTransaction transaction)
//        {
//            // If this building exists in 1NF, we can use it (it would remain unchanged in our database)
//            bool buildingExists = false;

//            using (FbCommand cmd = new FbCommand("SELECT FIRST 1 OBJECT_KOD FROM OBJECT_1NF WHERE OBJECT_KOD = " +
//                buildingId.ToString(), connection1NF))
//            {
//                cmd.Transaction = transaction;

//                using (FbDataReader r = cmd.ExecuteReader())
//                {
//                    buildingExists = r.Read();

//                    r.Close();
//                }
//            }

//            if (buildingExists)
//            {
//                return buildingId;
//            }

//            // If this object was already processed, there must be a mapping for it
//            int existingId = 0;

//            if (correctionObjectIdMapping.TryGetValue(buildingId, out existingId))
//            {
//                return existingId;
//            }

//            // We have to create a new object from our database
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            FieldMappings.Create1NFObjectFieldMapping(fields, testOnly, enable1NFNewFieldsImport);

//            // Remove the fields that we are going to set manually
//            fields.Remove("OBJECT_KOD");
//            fields.Remove("DT");
//            fields.Remove("ISP");
//            fields.Remove("STAN_YEAR");
//            fields.Remove("DT_BEG");
//            fields.Remove("DT_END");
//            fields.Remove("DELETED");
//            fields.Remove("DTDELETE");
//            fields.Remove("ARCH_KOD");
//            fields.Remove("IN_ARCH");
//            fields.Remove("CHARACTERISTIC");

//            // Prepare the SELECT statement for SQL Server
//            string srcFieldList = "";

//            foreach (KeyValuePair<string, string> entry in fields)
//            {
//                if (srcFieldList.Length > 0)
//                {
//                    srcFieldList += ", ";
//                }

//                srcFieldList += entry.Value;
//            }

//            // Get the data from our database
//            Dictionary<string, object> parameters = new Dictionary<string, object>();

//            string insertFieldList = "";
//            string insertParamList = "";
//            string specialParamNomer1 = "pnum1";
//            string specialParamNomer2 = "pnum2";

//            using (SqlCommand cmdGet = new SqlCommand("SELECT " + srcFieldList + " FROM buildings WHERE id = " +
//                buildingId.ToString(), connectionSqlClient))
//            {
//                using (SqlDataReader r = cmdGet.ExecuteReader())
//                {
//                    if (r.Read())
//                    {
//                        // Prepare INSERT parameters
//                        for (int i = 0; i < r.FieldCount; i++)
//                        {
//                            if (!r.IsDBNull(i))
//                            {
//                                string fieldName = r.GetName(i).ToLower();

//                                foreach (KeyValuePair<string, string> entry in fields)
//                                {
//                                    if (entry.Value.ToLower() == fieldName)
//                                    {
//                                        object value = r.GetValue(i);
//                                        string paramName = "p" + i.ToString();

//                                        if (entry.Value.ToLower() == "addr_nomer1")
//                                        {
//                                            paramName = specialParamNomer1;
//                                        }
//                                        else if (entry.Value.ToLower() == "addr_nomer2")
//                                        {
//                                            paramName = specialParamNomer2;
//                                        }

//                                        if (insertFieldList.Length > 0)
//                                            insertFieldList += ", ";

//                                        insertFieldList += entry.Key;

//                                        if (insertParamList.Length > 0)
//                                            insertParamList += ", ";

//                                        insertParamList += "@" + paramName;

//                                        parameters[paramName] = value;
//                                        break;
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            if (parameters.Count > 0)
//            {
//                // Preprocess parameters
//                object paramNumber1 = "";
//                object paramNumber2 = "";

//                if (!parameters.TryGetValue(specialParamNomer1, out paramNumber1))
//                {
//                    paramNumber1 = "";
//                }

//                if (!parameters.TryGetValue(specialParamNomer2, out paramNumber2))
//                {
//                    paramNumber2 = "";
//                }

//                if (paramNumber1 is string && paramNumber2 is string)
//                {
//                    string number1 = (string)paramNumber1;
//                    string number2 = (string)paramNumber2;

//                    ObjectFinder.PreProcessBuildingNumbers(ref number1, ref number2);

//                    parameters[specialParamNomer1] = number1;
//                    parameters[specialParamNomer2] = number2;
//                }

//                // Generate ID for a new object
//                int newObjectId = GenerateId1NFUsingGenerator("OBJECT_1NF_GEN", transaction);

//                if (newObjectId > 0)
//                {
//                    // Format the INSERT statement
//                    string query = "INSERT INTO OBJECT_1NF (OBJECT_KOD, OBJECT_KODSTAN, ISP, DT, STAN_YEAR, REALSTAN, DT_BEG, DELETED, KORPUS, "
//                        + insertFieldList + ") VALUES (@oid, 1, @isp, @dt, @syear, 1, @dt, 0, 0, " + insertParamList + ")";

//                    try
//                    {
//                        using (FbCommand commandInsert = new FbCommand(query, connection1NF))
//                        {
//                            commandInsert.Parameters.Add(new FbParameter("oid", newObjectId));
//                            commandInsert.Parameters.Add(new FbParameter("isp", AutoUserName));
//                            commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));
//                            commandInsert.Parameters.Add(new FbParameter("syear", DateTime.Now.Year));

//                            foreach (KeyValuePair<string, object> pair in parameters)
//                            {
//                                commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
//                            }

//                            commandInsert.Transaction = transaction;
//                            commandInsert.ExecuteNonQuery();
//                        }

//                        // Save the mapping, to avoid creatingthe same object twice
//                        correctionObjectIdMapping[buildingId] = newObjectId;

//                        return newObjectId;
//                    }
//                    catch (Exception ex)
//                    {
//                        logger.WriteError("SQL query failed: " + query);
//                        logger.WriteError("Message: " + ex.Message);
//                        DumpQueryParameters(parameters);
//                        logger.WriteError("Parameter oid = " + newObjectId.ToString());
//                    }
//                }
//            }

//            return -1;
//        }

//        private int GetOrCreateOrganization1NF(int organizationId, FbTransaction transaction)
//        {
//            // If this organization exists in 1NF, we can use it (it would remain unchanged in our database)
//            bool orgExists = false;

//            using (FbCommand cmd = new FbCommand("SELECT FIRST 1 KOD_OBJ FROM SORG_1NF WHERE KOD_OBJ = " +
//                organizationId.ToString(), connection1NF))
//            {
//                cmd.Transaction = transaction;

//                using (FbDataReader r = cmd.ExecuteReader())
//                {
//                    orgExists = r.Read();

//                    r.Close();
//                }
//            }

//            if (orgExists)
//            {
//                return organizationId;
//            }

//            // If this organization was already processed, there must be a mapping for it
//            int existingId = 0;

//            if (correctionOrgIdMapping.TryGetValue(organizationId, out existingId))
//            {
//                return existingId;
//            }

//            // We have to create a new organization from our database
//            Dictionary<string, string> fields = new Dictionary<string, string>();

//            FieldMappings.Create1NFOrgFieldMapping(fields, enable1NFNewFieldsImport);

//            // Remove the fields that we are going to set manually
//            fields.Remove("KOD_OBJ");
//            fields.Remove("LAST_SOST");
//            fields.Remove("ISP");
//            fields.Remove("DT");
//            fields.Remove("DELETED");
//            fields.Remove("DTDELETE");
//            fields.Remove("ARCH_KOD");
//            fields.Remove("IN_ARCH");
//            fields.Remove("DT_BEG_STAN");
//            fields.Remove("DT_END_STAN");
//            fields.Remove("KOD_VID_PREDPR");

//            // Prepare the SELECT statement for SQL Server
//            string srcFieldList = "";

//            foreach (KeyValuePair<string, string> entry in fields)
//            {
//                if (srcFieldList.Length > 0)
//                {
//                    srcFieldList += ", ";
//                }

//                srcFieldList += entry.Value;
//            }

//            // Get the data from our database
//            Dictionary<string, object> parameters = new Dictionary<string, object>();

//            string insertFieldList = "";
//            string insertParamList = "";

//            using (SqlCommand cmdGet = new SqlCommand("SELECT " + srcFieldList + " FROM organizations WHERE id = " +
//                organizationId.ToString(), connectionSqlClient))
//            {
//                using (SqlDataReader r = cmdGet.ExecuteReader())
//                {
//                    if (r.Read())
//                    {
//                        // Prepare INSERT parameters
//                        for (int i = 0; i < r.FieldCount; i++)
//                        {
//                            if (!r.IsDBNull(i))
//                            {
//                                string fieldName = r.GetName(i).ToLower();

//                                foreach (KeyValuePair<string, string> entry in fields)
//                                {
//                                    if (entry.Value.ToLower() == fieldName)
//                                    {
//                                        object value = r.GetValue(i);

//                                        // First check if we can map the value contained in this dictionary
//                                        if (CanMapOrganizationValueTo1NF(entry.Key.ToUpper(), value))
//                                        {
//                                            string paramName = "p" + i.ToString();

//                                            if (insertFieldList.Length > 0)
//                                                insertFieldList += ", ";

//                                            insertFieldList += entry.Key;

//                                            if (insertParamList.Length > 0)
//                                                insertParamList += ", ";

//                                            insertParamList += "@" + paramName;

//                                            parameters[paramName] = value;
//                                        }

//                                        break;
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            if (parameters.Count > 0 && organizationIdSeed1NF > 0)
//            {
//                // Generate ID for a new organization
//                organizationIdSeed1NF++;

//                int newOrgId = organizationIdSeed1NF;

//                // Format the INSERT statement
//                string query = "INSERT INTO SORG_1NF (KOD_OBJ, KOD_STAN, LAST_SOST, ISP, DT, USER_KOREG, DATE_KOREG, DELETED, DT_BEG_STAN, "
//                    + insertFieldList + ") VALUES (@orgd, 1, 1, @isp, @dt, @isp, @dt, 0, @dt, " + insertParamList + ")";

//                try
//                {
//                    using (FbCommand commandInsert = new FbCommand(query, connection1NF))
//                    {
//                        commandInsert.Parameters.Add(new FbParameter("orgd", newOrgId));
//                        commandInsert.Parameters.Add(new FbParameter("isp", AutoUserName));
//                        commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now.Date));

//                        foreach (KeyValuePair<string, object> pair in parameters)
//                        {
//                            commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
//                        }

//                        commandInsert.Transaction = transaction;
//                        commandInsert.ExecuteNonQuery();
//                    }

//                    // Save the mapping, to avoid creatingthe same object twice
//                    correctionOrgIdMapping[organizationId] = newOrgId;

//                    return newOrgId;
//                }
//                catch (Exception ex)
//                {
//                    logger.WriteError("SQL query failed: " + query);
//                    logger.WriteError("Message: " + ex.Message);
//                    DumpQueryParameters(parameters);
//                    logger.WriteError("Parameter orgd = " + newOrgId.ToString());
//                }
//            }

//            return -1;
//        }

//        #endregion (Data corrections)

//        #region Working with dictionaries

//        private void Extract1NFDictionary(string dictTableName, string keyField)
//        {
//            HashSet<int> keys = new HashSet<int>();

//            using (FbCommand cmd = new FbCommand("SELECT " + keyField + " FROM " + dictTableName, connection1NF))
//            {
//                using (FbDataReader r = cmd.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0))
//                        {
//                            int key = r.GetInt32(0);

//                            if (key > 0)
//                            {
//                                keys.Add(key);
//                            }
//                        }
//                    }

//                    r.Close();
//                }
//            }

//            dictionaries1NF[dictTableName.Trim().ToUpper()] = keys;
//        }

//        private bool CanMapOrganizationValueTo1NF(string fieldName, object value)
//        {
//            if (value is int)
//            {
//                fieldName = fieldName.Trim().ToUpper();

//                if (fieldName == "KOD_VID_DIAL")
//                    return CanMapDictionaryValue1NF("S_VID_DIAL", (int)value);

//                if (fieldName == "KOD_STATUS")
//                    return CanMapDictionaryValue1NF("S_STATUS", (int)value);

//                if (fieldName == "KOD_FORM_GOSP")
//                    return CanMapDictionaryValue1NF("S_FORM_GOSP", (int)value);

//                if (fieldName == "KOD_FORM_VLASN")
//                    return CanMapDictionaryValue1NF("S_FORM_VLASN", (int)value);

//                if (fieldName == "KOD_GOSP_STRUKT")
//                    return CanMapDictionaryValue1NF("S_GOSP_STRUKT", (int)value);

//                if (fieldName == "KOD_ORGAN")
//                    return CanMapDictionaryValue1NF("S_ORGAN", (int)value);

//                if (fieldName == "KOD_GALUZ")
//                    return CanMapDictionaryValue1NF("S_GALUZ", (int)value);

//                if (fieldName == "KOD_ADDITION_PRIZNAK")
//                    return CanMapDictionaryValue1NF("S_ADDITION_PRIZNAK", (int)value);

//                if (fieldName == "KOD_PRIKMET")
//                    return CanMapDictionaryValue1NF("S_PRIKMET", (int)value);

//                if (fieldName == "KOD_PRIZNAK_1")
//                    return CanMapDictionaryValue1NF("S_PRIZNAK_1", (int)value);

//                if (fieldName == "KOD_VIDOM_NAL")
//                    return CanMapDictionaryValue1NF("S_VIDOM_NAL", (int)value);

//                if (fieldName == "KOD_IMEN")
//                    return CanMapDictionaryValue1NF("S_IMEN", (int)value);

//                if (fieldName == "KOD_ORG_FORM")
//                    return CanMapDictionaryValue1NF("S_ORG_FORM", (int)value);

//                if (fieldName == "KOD_VID_GOSP_STR")
//                    return CanMapDictionaryValue1NF("S_VID_GOSP_STR", (int)value);

//                if (fieldName == "KOD_VID_AKCIA")
//                    return CanMapDictionaryValue1NF("S_AKCIA", (int)value);

//                if (fieldName == "VIDDIL")
//                    return CanMapDictionaryValue1NF("S_VIDDIL", (int)value);
//            }

//            return true;
//        }

//        private bool CanMapDictionaryValue1NF(string dictionaryName, int value)
//        {
//            HashSet<int> dictionary = null;

//            if (dictionaries1NF.TryGetValue(dictionaryName.Trim().ToUpper(), out dictionary))
//            {
//                return dictionary.Contains(value);
//            }

//            return false;
//        }

//        private void PopulateOwnershipDictionary()
//        {
//            using (SqlCommand cmd = new SqlCommand("SELECT id, name FROM dict_org_ownership", connectionSqlClient))
//            {
//                using (SqlDataReader r = cmd.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                        {
//                            int key = r.GetInt32(0);
//                            string value = r.GetString(1);

//                            if (key > 0 && value.Length > 0)
//                            {
//                                dictFormOfOwnership.Add(key, value);
//                            }
//                        }
//                    }

//                    r.Close();
//                }
//            }
//        }

//        private void PopulateTransferRightsDictionary()
//        {
//            using (SqlCommand cmd = new SqlCommand("SELECT id, name FROM dict_obj_rights", connectionSqlClient))
//            {
//                using (SqlDataReader r = cmd.ExecuteReader())
//                {
//                    while (r.Read())
//                    {
//                        if (!r.IsDBNull(0) && !r.IsDBNull(1))
//                        {
//                            int key = r.GetInt32(0);
//                            string value = r.GetString(1);

//                            if (key > 0 && value.Length > 0)
//                            {
//                                dictTransferRight.Add(key, value);
//                            }
//                        }
//                    }

//                    r.Close();
//                }
//            }
//        }

//        private int GetMapped1NFOwnershipType(int formOfOwnership)
//        {
//            if (formOfOwnership == 3)
//            {
//                return 99;
//            }
//            else if (formOfOwnership == 32)
//            {
//                return 33;
//            }

//            return formOfOwnership;
//        }

//        #endregion (Working with dictionaries)

//        #region Helper functions

//        private List<int> FindAllBuildingsByPrimaryAddress(int buildingId)
//        {
//            List<int> buildings = new List<int>();

//            ObjectInfo info = objectFinder.GetObjectInfo(buildingId);

//            if (info != null && info.street is string)
//            {
//                string fullAddress = "";
//                string simpleAddress = "";
//                bool addressIsSimple = false;

//                ObjectFinder.UnifiedAddress uniAddr = ObjectFinder.GetUnifiedAddressEx(
//                    (string) info.street,
//                    info.nomer1 is string ? (string)info.nomer1 : "",
//                    info.nomer2 is string ? (string)info.nomer2 : "",
//                    info.nomer3 is string ? (string)info.nomer3 : "",
//                    info.addrMiscInfo is string ? (string)info.addrMiscInfo : "",
//                    out fullAddress,
//                    out simpleAddress,
//                    out addressIsSimple);

//                if (uniAddr != null && uniAddr.streetName != null)
//                {
//                    // Correct some known misspellings
//                    if (uniAddr.streetName.street.Contains(Properties.Resources.StreetBoryspil) &&
//                        uniAddr.number == "23" && uniAddr.misc == Properties.Resources.AddrKM)
//                    {
//                        uniAddr.number = Properties.Resources.AddrBoryspilske23KMK1;
//                        uniAddr.misc = "";
//                    }

//                    if (uniAddr.streetName.street.Length > 0 && uniAddr.number.Length > 0)
//                    {
//                        objectFinder.FindAllObjectsByPrimaryAddress(uniAddr.streetName.street, uniAddr.number, buildings);
//                    }
//                }
//            }

//            return buildings;
//        }

//        /// <summary>
//        /// Finds the last information actualization date for particular organization (balans holder) and address
//        /// </summary>
//        /// <param name="buildingId"></param>
//        /// <param name="organizationId"></param>
//        /// <param name="balans"></param>
//        /// <returns></returns>
//        private DateTime GetInfoDateForBalansObject(int buildingId, int organizationId, object square, Dictionary<int, List<BalansObjectInfo>> balans)
//        {
//            // Check if building address can be matched successfully
//            ObjectInfo info = objectFinder.GetObjectInfo(buildingId);

//            if (info != null)
//            {
//                // If building is located on multiple streets, it can't be matched
//                if (info.streetId2 is int)
//                    return DateTime.MaxValue;

//                if (info.addrMiscInfo is string)
//                {
//                    string miscAddress = (string)info.addrMiscInfo;

//                    if (miscAddress.Contains('/') || miscAddress.Contains('\\'))
//                        return DateTime.MaxValue;
//                }
//            }

//            // Find all similar addresses
//            List<int> buildings = FindAllBuildingsByPrimaryAddress(buildingId);

//            DateTime bestDate = DateTime.MinValue;

//            foreach (int building in buildings)
//            {
//                List<BalansObjectInfo> balansObjects = null;

//                if (balans.TryGetValue(building, out balansObjects))
//                {
//                    foreach (BalansObjectInfo bal in balansObjects)
//                    {
//                        if (bal.modifyDate is DateTime && (DateTime)bal.modifyDate > bestDate)
//                        {
//                            if (OrgMatch(bal.organizationId, organizationId))
//                            {
//                                bestDate = (DateTime)bal.modifyDate;
//                            }

//                            if (SquareMatch(bal.objSquare, square))
//                            {
//                                bestDate = (DateTime)bal.modifyDate;
//                            }
//                        }
//                    }
//                }
//            }

//            return bestDate;
//        }

//        public static bool IsPrivateOwnership(int ownershipType)
//        {
//            foreach (int ownType in Migrator.ownershipTypesPrivatAll)
//            {
//                if (ownType == ownershipType)
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        public static bool IsCommunityOwnership(int ownershipType)
//        {
//            foreach (int ownType in Migrator.ownershipTypesKomunalnaAll)
//            {
//                if (ownType == ownershipType)
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        #endregion Helper functions
    }
}
