using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI;
using log4net;

/// <summary>
/// A single item in the Hierarchical data source for the Rishennya project document tree
/// </summary>
public class RishProjectDataSourceItem : IRishProjectNode
{
    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");

    public class ComparerByOrdinal : IComparer<RishProjectDataSourceItem>
    {
        public static readonly ComparerByOrdinal Comparer = new ComparerByOrdinal();

        private ComparerByOrdinal()
        {
        }

        public int Compare(RishProjectDataSourceItem x, RishProjectDataSourceItem y)
        {
            return x.OrdinalPos.CompareTo(y.OrdinalPos);
        }
    }

    protected readonly RishProjectDataSource owner;
    protected readonly DB.bp_rish_project_itemRow row;

    public DB.bp_rish_project_itemRow Row { get { return row; } }

    public string introText { get { return Row.Isintro_textNull() ? string.Empty : Row.intro_text; } set { Row.intro_text = value; } }
    public string outroText { get { return Row.Isoutro_textNull() ? string.Empty : Row.outro_text; } set { Row.outro_text = value; } }
    public int organizationFromId { get { return Row.Isorg_from_idNull() ? 0 : Row.org_from_id; } set { if (value == 0) Row.Setorg_from_idNull(); else Row.org_from_id = value; } }
    public int organizationToId { get { return Row.Isorg_to_idNull() ? 0 : Row.org_to_id; } set { if (value == 0) Row.Setorg_to_idNull(); else Row.org_to_id = value; } }
    public int rightId { get { return Row.Isright_idNull() ? 0 : Row.right_id; } set { if (value == 0) Row.Setright_idNull(); else Row.right_id = value; } }
    public string explanation { get { return Row.IsexplanationNull() ? string.Empty : Row.explanation; } set { Row.explanation = value; } }
    public bool isTable { get { return Row.is_table; } set { Row.is_table = value; } }
    public int? tableType { get { return Row.Istable_typeNull() ? (int?)null : Row.table_type; } set { if (value == null) Row.Settable_typeNull(); else Row.table_type = value.Value; } }
    public string arbitraryText { get { return Row.Isarbitrary_textNull() ? string.Empty : Row.arbitrary_text; } set { Row.arbitrary_text = value; } }
    public bool useExternalDocument { get { return Row.Isuse_external_documentNull() ? false : Row.use_external_document; } set { Row.use_external_document = value; } }
    public string externalDocumentName
    {
        get { return Row.Isexternal_document_idNull() ? string.Empty : Row.external_documentRow.name; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (!Row.Isexternal_document_idNull())
                {
                    Row.external_documentRow.Delete();
                    Row.Setexternal_document_idNull();
                }
            }
            else
            {
                if (Row.Isexternal_document_idNull())
                {
                    Row.external_documentRow =
                        ((DB)Row.Table.DataSet).external_document.Addexternal_documentRow(value, string.Empty);
                }
                else
                {
                    Row.external_documentRow.name = value;
                }
            }
        }
    }
    public string externalDocumentUniqueFileName
    {
        get { return Row.Isexternal_document_idNull() ? string.Empty : Row.external_documentRow.unique_filename; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (!Row.Isexternal_document_idNull())
                {
                    Row.external_documentRow.Delete();
                    Row.Setexternal_document_idNull();
                }
            }
            else
            {
                if (Row.Isexternal_document_idNull())
                {
                    Row.external_documentRow =
                        ((DB)Row.Table.DataSet).external_document.Addexternal_documentRow(string.Empty, value);
                }
                else
                {
                    Row.external_documentRow.unique_filename = value;
                }
            }
        }
    }

    /// <summary>
    /// Creates a brand new node (which does not exist in the database yet)
    /// </summary>
    /// <param name="owner">The owning data source</param>
    /// <param name="parentNodeID">ID of the parent node</param>
    /// <param name="isTbl">Indicates that a table is added to the parent item</param>
    public RishProjectDataSourceItem(RishProjectDataSource owner, int parentNodeID, bool isTbl)
    {
        this.owner = owner;

        // Set a very big ordinal, to place the item at the very end; it is expected that
        // the caller will use RefreshOrdinals() method of the data source to update
        // the ordinal number immediately
        this.row = owner.Context.bp_rish_project_item.Addbp_rish_project_itemRow(
            owner.ProjectRow, null, 1000000, null, null, 0, 0, 0, null, isTbl, null, null, isTbl, 0);
        this.row.SetexplanationNull();
        this.row.Setintro_textNull();
        this.row.Setorg_from_idNull();
        this.row.Setorg_to_idNull();
        this.row.Setoutro_textNull();
        this.row.Setparent_item_idNull();
        this.row.Setright_idNull();
        if (!isTbl)
            this.row.Settable_typeNull();

        // This will take care of proper alignment and positioning in the hierarchy of the newly created 
        // Typed Dataset record.
        this.ParentID = parentNodeID;
    }

    /// <summary>
    /// Performs de-serealization of the node from the database
    /// </summary>
    /// <param name="owner">The owning data source</param>
    /// <param name="row">Data row to deserialize from</param>
    public RishProjectDataSourceItem(RishProjectDataSource owner, DB.bp_rish_project_itemRow row)
    {
        this.owner = owner;
        this.row = row;
    }

    public RishProjectDataSource DataSource
    {
        get
        {
            return owner;
        }
    }

    /// <summary>
    /// Generates a templated item text, depending on the item content
    /// </summary>
    /// <returns></returns>
    public string GeneratePunktText(bool convertToText)
    {
        return convertToText ? Utils.ExtractTextFromHtml(outroText) : outroText;

        /*
        string strippedIntro = convertToText ? Utils.ExtractTextFromHtml(introText) : introText;
        string strippedOutro = convertToText ? Utils.ExtractTextFromHtml(outroText) : outroText;

        strippedIntro = strippedIntro.Trim();
        strippedOutro = strippedOutro.Trim();

        string dynamicPart = GetDynamicPunkTextPart();

        if (strippedIntro.Length > 0 && !strippedIntro.EndsWith("."))
        {
            dynamicPart = DecapitalizeFirstLetter(dynamicPart);
        }

        return strippedIntro + (strippedIntro.Length > 0 ? " " : string.Empty)
            + dynamicPart
            + (strippedOutro.Length > 0 ? " " : string.Empty) + strippedOutro;
        */
    }

    [Flags]
    public enum DataPiece
    {
        None = 0x00,
        ObjectOrAddress = 0x01,
        OrgFrom = 0x02,
        OrgTo = 0x04,
        Right = 0x08,
    }

    // For the purposes of template interpretation, the following positional parameters are available (actual text, not IDs):
    // {0} - ObjectOrAddress (if object is specified, address is ignored)
    // {1} - OrgFrom
    // {2} - OrgTo
    // {3} - Right
    // If the corresponding _item entry has no data for any given parameter, null is passed instead, so positional
    // parameter indexes remain intact.
    private static readonly Dictionary<DataPiece, string> _dynamicPunkTextTemplates = new Dictionary<DataPiece, string>()
    {
        {
            DataPiece.ObjectOrAddress,
            "Об'єкт {0}"
        },
        {
            DataPiece.ObjectOrAddress | DataPiece.OrgFrom, 
            "Об'єкт {0} від організації {1}"
        },
        {
            DataPiece.ObjectOrAddress | DataPiece.OrgTo, 
            "Об'єкт {0} до організації {2}"
        },
        {
            DataPiece.ObjectOrAddress | DataPiece.Right, 
            "Передати {3} щодо об'єкта {0}"
        },
        {
            DataPiece.ObjectOrAddress | DataPiece.OrgFrom | DataPiece.OrgTo, 
            "Від організації {1} до організації {2} щодо об'єкта: {0}"
        },
        {
            DataPiece.ObjectOrAddress | DataPiece.OrgFrom | DataPiece.Right,
            "Передати {3} від організації {1} щодо об'єкта: {0}"
        },

        { 
            DataPiece.ObjectOrAddress | DataPiece.OrgTo | DataPiece.Right, 
            "Передати {3} до організації {2} щодо об'єкта: {0}" 
        },
        { 
            DataPiece.ObjectOrAddress | DataPiece.OrgFrom | DataPiece.OrgTo | DataPiece.Right, 
            "Передати {3} від організації {1} до організації {2} щодо об'єкта: {0}"
        },
        
        {
            DataPiece.OrgFrom,
            "Від організації {1}"
        },
        {
            DataPiece.OrgFrom | DataPiece.OrgTo, 
            "Від організації {1} до організації {2}"
        },
        { 
            DataPiece.OrgFrom | DataPiece.Right, 
            "Передати {3} від організації {1}"
        },
        { 
            DataPiece.OrgFrom | DataPiece.OrgTo | DataPiece.Right, 
            "Передати {3} від організації {1} до організації {2}"
        },

        {
            DataPiece.OrgTo,
            "До організації {2}"
        },
        { 
            DataPiece.OrgTo | DataPiece.Right, 
            "Передати {3} до організації {2}"
        },
        
        {
            DataPiece.Right,
            "Передати {3}"
        },
    };

    public string GetDynamicPunkTextPart()
    {
        DataPiece mask = DataPiece.None;

        string objectOrAddress = string.Empty;
        string organizationFrom = string.Empty;
        string organizationTo = string.Empty;
        string right = string.Empty;

        if (!string.IsNullOrWhiteSpace(arbitraryText))
        {
            mask |= DataPiece.ObjectOrAddress;
            objectOrAddress = GetDynamicPunkTextPart(DataPiece.ObjectOrAddress);
        }

        if (organizationFromId > 0)
        {
            mask |= DataPiece.OrgFrom;
            organizationFrom = GetDynamicPunkTextPart(DataPiece.OrgFrom);
        }

        if (organizationToId > 0)
        {
            mask |= DataPiece.OrgTo;
            organizationTo = GetDynamicPunkTextPart(DataPiece.OrgTo);
        }

        if (rightId > 0)
        {
            mask |= DataPiece.Right;
            right = GetDynamicPunkTextPart(DataPiece.Right);
        }

        string template;

        if (!_dynamicPunkTextTemplates.TryGetValue(mask, out template))
            return string.Empty;

        if (mask == (DataPiece.ObjectOrAddress | DataPiece.OrgFrom | DataPiece.OrgTo | DataPiece.Right) &&
            (right.StartsWith("до сфери управ") || right.StartsWith("на баланс")))
        {
            template = "Передати {3} від організації {1} до організації {2} об'єкт: {0}";
        }

        return string.Format(template, objectOrAddress, organizationFrom, organizationTo, right);
    }

    public static string FormatOrganizationInfo(int organizationId)
    {
        if (organizationId > 0)
        {
            DB.organizationsRow data =
                (new DBTableAdapters.organizationsTableAdapter()).GetDataByID(organizationId).FirstOrDefault();

            if (data != null && !data.Isfull_nameNull())
                return CapitalizeAllWords(data.full_name);
        }
        return string.Empty;
    }

    public static string FormatObjectOrAddressInfo(int buildingId, int balansObjectId, string arbitraryText)
    {
        string objectOrAddress = string.Empty;

        if (balansObjectId <= 0 && buildingId <= 0 && !string.IsNullOrWhiteSpace(arbitraryText))
        {
            objectOrAddress = arbitraryText;
        }
        else 
        if (balansObjectId > 0)
        {
            //DB.view_balansRow data = (new DBTableAdapters.view_balansTableAdapter()).GetDataByBalansID(balansObjectId).FirstOrDefault();
            DB.view_balansRow data; 

            DB.view_balansDataTable dt = new DB.view_balansDataTable();
            try
            {
                (new DBTableAdapters.view_balansTableAdapter()).FillByBalansID(dt, balansObjectId);
                data = dt.FirstOrDefault();
            }
            catch
            {
                foreach (System.Data.DataRow row in dt.GetErrors())
                {
                    log.Error(row.RowError);
                }
                throw;
            }

            if (data != null)
            {

                /*
                if (!data.Isbalans_obj_nameNull())
                {
                    objectOrAddress += (objectOrAddress.Length > 0 ? " " : string.Empty)
                        + (!string.IsNullOrWhiteSpace(arbitraryText) ? string.Format("({0})", arbitraryText) : data.balans_obj_name.ToLower());
                }                
                
                if (!data.Issqr_totalNull())
                {
                    objectOrAddress += (objectOrAddress.Length > 0 ? " " : string.Empty)
                        + "площею " + data.sqr_total + " кв.м.";
                }
                
                objectOrAddress +=
                    (objectOrAddress.Length > 0 ? " за адресою " : string.Empty)
                    + (data.Isstreet_full_nameNull() ? string.Empty : CapitalizeAllWords(data.street_full_name))
                    + (!data.Isstreet_full_nameNull() && !data.Isaddr_nomerNull() ? ", " : "")
                    + (data.Isaddr_nomerNull() ? string.Empty : data.addr_nomer);
                */
                objectOrAddress +=
                    (data.Isstreet_full_nameNull() ? string.Empty : CapitalizeAllWords(data.street_full_name))
                    + (!data.Isstreet_full_nameNull() && !data.Isaddr_nomerNull() ? ", " : "")
                    + (data.Isaddr_nomerNull() ? string.Empty : data.addr_nomer);
            }
        }
        else if (buildingId > 0)
        {
            DB.view_buildingsRow data = (new DBTableAdapters.view_buildingsTableAdapter()).GetDataByBuildingID(buildingId).FirstOrDefault();
            if (data != null)
            {
                //objectOrAddress = !string.IsNullOrWhiteSpace(arbitraryText) ? arbitraryText + " " : string.Empty;
                //objectOrAddress += (data.Isstreet_full_nameNull() ? string.Empty : "за адресою " + CapitalizeAllWords(data.street_full_name));
                objectOrAddress += (data.Isstreet_full_nameNull() ? string.Empty : CapitalizeAllWords(data.street_full_name));

                if (!data.Isaddr_nomerNull())
                {
                    if (objectOrAddress.Length > 0)
                        objectOrAddress += ", ";

                    objectOrAddress += data.addr_nomer;
                }
            }
        }

        return objectOrAddress;
    }

    public string GetDynamicPunkTextPart(DataPiece dataPiece)
    {
        switch (dataPiece)
        {
            case DataPiece.ObjectOrAddress:
                {
                    //return FormatObjectOrAddressInfo(buildingId, balansObjectId, arbitraryText);
                    return FormatObjectOrAddressInfo(0, 0, "");
                }

            case DataPiece.OrgFrom:
                {
                    return FormatOrganizationInfo(organizationFromId);
                }

            case DataPiece.OrgTo:
                {
                    return FormatOrganizationInfo(organizationToId);
                }

            case DataPiece.Right:
                {
                    string right = string.Empty;

                    if (rightId > 0)
                    {
                        DB.dict_obj_rightsRow data = (new DBTableAdapters.dict_obj_rightsTableAdapter()).GetDataByID(rightId).FirstOrDefault();

                        if (data != null && !data.IsnameNull())
                        {
                            right = data.name;
                            string prefix = "право";

                            if (right == "ДО СФЕРИ УПРАВЛІННЯ" || right == "НА БАЛАНС")
                            {
                                prefix = string.Empty;
                            }
                            else if (right == "ПОЗИЧКУ")
                            {
                                prefix = "в";
                            }

                            if (prefix.Length > 0)
                            {
                                right = prefix + " " + right;
                            }
                        }
                    }

                    return right.ToLower();
                }
        }

        return "";
    }

    protected static string CapitalizeFirstLetter(string text)
    {
        text = text.Trim();

        if (text.Length > 0)
        {
            return text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
        }

        return text;
    }

    protected static string CapitalizeAllWords(string text)
    {
        string[] parts = text.Split(' ');
        string result = "";

        foreach (string part in parts)
        {
            if (part.StartsWith("\"") && part.EndsWith("\""))
            {
                result += " \"" + CapitalizeFirstLetter(part.Substring(1, part.Length - 2)) + "\"";
            }
            else
            {
                result += " " + CapitalizeFirstLetter(part);
            }
        }

        return result.TrimStart(' ');
    }

    protected static string DecapitalizeFirstLetter(string text)
    {
        text = text.Trim();

        if (text.Length > 0)
        {
            return text.Substring(0, 1).ToLower() + text.Substring(1);
        }

        return text;
    }

    #region IRishProjectNode Members

    public virtual int ID { get { return Row.id; } }

    public virtual int ParentID
    {
        get
        {
            return Row.Isparent_item_idNull() 
                ? Row.project_id + RishProjectAppendixItem.DocAndAppIdentifierPadding 
                : Row.parent_item_id;
        }
        set
        {
            int? newProjectID = null;

            IRishProjectNode parentNode = owner.First(x => x.ID == value);
            if (parentNode is RishProjectDataSourceItem)
            {
                // The node is dropped under an item node, and therefore is a subitem
                Row.parent_item_id = value;
                
                int projectID = ((RishProjectDataSourceItem)parentNode).Row.project_id;
                if (projectID != Row.project_id)
                {
                    Row.project_id = projectID;
                    newProjectID = projectID;
                }
            }
            else
            {
                // The node is dropped under the document/appendix node, and therefore
                // has no formal parent
                Row.Setparent_item_idNull();

                int projectID = ((RishProjectAppendixItem)parentNode).Row.id;
                if (projectID != Row.project_id)
                {
                    Row.project_id = projectID;
                    newProjectID = projectID;
                }
            }

            if (newProjectID != null)
            {
                // Switch all children of this node to the new Project ID as well.
                foreach (IRishProjectNode child0 in owner.GetAllChildrenOfID(this.ID))
                {
                    RishProjectDataSourceItem child = child0 as RishProjectDataSourceItem;
                    if (child == null)
                        continue;
                    child.Row.project_id = newProjectID.Value;
                }
            }
        }
    }

    public virtual string DisplayNodeType { get { return string.Empty; } }

    public virtual string DisplayNumber
    {
        get
        {
            if (isTable)
            {
                return "";
            }

            string value = "";
            IRishProjectNode node = this;

            while (node != null)
            {
                value = node.OrdinalPos.ToString() + "." + value;
                node = owner.FirstOrDefault(x => x.ID == node.ParentID && x is RishProjectDataSourceItem);
            }

            return value;
        }
    }

    public virtual string DisplayText
    {
        get
        {
            string text;

            if (isTable)
            {
                if (!string.IsNullOrEmpty(externalDocumentName))
                {
                    text = string.Format("Додаток; завантажено {0} об'єкт{1} з документу {2}",
                        this.Row.Getbp_rish_project_tableRows().Length, "ів", externalDocumentName);
                }
                else
                {
                    text = "Додаток; об'єктів не завантажено";
                }
            }
            else
            {
                if (useExternalDocument)
                {
                    if (!string.IsNullOrEmpty(externalDocumentName))
                    {
                        text = "Використання зовнішнього документа " + externalDocumentName;
                    }
                    else
                    {
                        text = "Зовнішнього документа не вказано";
                    }
                }
                else
                {
                    text = GeneratePunktText(true) ?? string.Empty;
                }
            }

            return (text.Length > 85) ? text.Substring(0, 82) + "..." : text;
        }
    }

    public virtual int OrdinalPos { get { return Row.Isordinal_posNull() ? 0 : Row.ordinal_pos; } set { Row.ordinal_pos = value; } }

    public void BeforeRemove()
    {
        // Mark the corresponding database row for removal (doesn't remove the row from the enclosing table, though)
        this.row.Delete();
    }

    #endregion
}
