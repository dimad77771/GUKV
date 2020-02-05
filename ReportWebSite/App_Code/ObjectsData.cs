using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ObjectsData
/// </summary>
public class ObjectsData
{
    public int ProjectID { get; set; }
    public string ActNumber { get; set; }
    public DateTime ActDate { get; set; }
    public int[] SelectedTableRowIDs { get; set; }
    public int BalansOwnershipTypeID { get; set; }
    public int RightID { get; set; }
    public bool RightRequiresOrgFromID { get; set; }
    public bool RightRequiresOrgToID { get; set; }
    public int OrgFromID { get; set; }
    public int OrgToID { get; set; }
    public int ObjectType { get; set; }
    public int ProjectDocumentID { get; set; }
    public string ProjectDocumentNumber { get; set; }
    public DateTime ProjectDocumentDate { get; set; }
    public int OwnershipDocType { get; set; }

    /// <summary>
    /// Get the most significant OrgID in this operation. If RightRequiresOrgToID, then OrgToID
    /// is the most significant, otherwise it is OrgFromID.
    /// </summary>
    /// <returns></returns>
    public int GetSignificantOrgID()
    {
        return RightRequiresOrgToID ? OrgToID : OrgFromID;
    }

    
}