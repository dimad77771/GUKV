using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI;

/// <summary>
/// Represents an Appendix node of the Hierarchical data source for the Rishennya project document tree
/// </summary>
public class RishProjectAppendixItem : IRishProjectNode
{
    public const int DocAndAppIdentifierPadding = 1000000;

    protected readonly RishProjectDataSource owner;
    protected readonly DB.bp_rish_projectRow projRow;

    public DB.bp_rish_projectRow Row { get { return projRow; } }

    public string name { get { return Row.name; } set { Row.name = value; } }
    public string introText { get { return Row.Isintro_textNull() ? string.Empty : Row.intro_text; } set { Row.intro_text = value; } }
    public string outroText { get { return Row.Isoutro_textNull() ? string.Empty : Row.outro_text; } set { Row.outro_text = value; } }
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
    /// Creates a brand new appendix (which does not exist in the database yet)
    /// </summary>
    /// <param name="owner">The owning data source</param>
    public RishProjectAppendixItem(RishProjectDataSource owner)
    {
        this.owner = owner;
        // Set a very big ordinal, to place the item at the very end; it is expected that
        // the caller will use RefreshOrdinals() method of the data source to update
        // the ordinal number immediately
        this.projRow = owner.Context.bp_rish_project
            .Addbp_rish_projectRow(string.Empty, null, null, owner.ProjectRow, 1000000, null, false);
    }

    /// <summary>
    /// Attaches to the given Typed Dataset table row
    /// </summary>
    /// <param name="owner">The owning data source</param>
    /// <param name="row">Data row to deserialize from</param>
    public RishProjectAppendixItem(RishProjectDataSource owner, DB.bp_rish_projectRow row)
	{
        this.owner = owner;
        this.projRow = row;
    }

    #region IRishProjectNode Members

    public virtual int ID { get { return projRow.id + DocAndAppIdentifierPadding; } }

    public virtual int ParentID { get; set; }

    public virtual string DisplayNodeType
    {
        get
        {
            return "Додаток " + (OrdinalPos - 1).ToString();
        }
    }

    public virtual string DisplayNumber
    {
        get
        {
            return string.Empty;
        }
    }

    public virtual string DisplayText
    {
        get
        {
            if (useExternalDocument)
            {
                if (!string.IsNullOrEmpty(externalDocumentName))
                {
                    return "Використання зовнішнього документа " + externalDocumentName;
                }
                else
                {
                    return "Зовнішнього документа не вказано";
                }
            }
            return (name.Length > 85) ? name.Substring(0, 82) + "..." : name;
        }
    }

    public virtual int OrdinalPos
    {
        get { return projRow.Isordinal_posNull() ? 0 : projRow.ordinal_pos; }
        set { projRow.ordinal_pos = value; }
    }

    public virtual void BeforeRemove()
    {
        // Mark the corresponding database row for removal (doesn't remove the row from the enclosing table, though)
        this.projRow.Delete();
    }

    #endregion
}
