using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.IO;
using System.Web.Security;

/// <summary>
/// Implements a Hierarchical data source for the Rishennya project document tree
/// </summary>
public class RishProjectDataSource : List<IRishProjectNode>, IListSource
{
    private readonly DB dataset = new DB();

    public DB Context { get { return dataset; } }

    public DB.bp_rish_projectRow ProjectRow { get { return ((RishProjectMainDocItem)this.First()).Row; } }

    public new void Remove(IRishProjectNode node)
    {
        node.BeforeRemove();
        base.Remove(node);
    }

    public void LoadFromDatabase(int projectId)
    {
        dataset.Clear();

        (new DBTableAdapters.external_documentTableAdapter()).Fill(dataset.external_document);
        (new DBTableAdapters.bp_rish_projectTableAdapter()).FillByIDOrProjectID(dataset.bp_rish_project, projectId);
        (new DBTableAdapters.bp_rish_project_infoTableAdapter()).FillByProjectID(dataset.bp_rish_project_info, projectId);
        (new DBTableAdapters.bp_rish_project_stateTableAdapter()).FillByProjectID(dataset.bp_rish_project_state, projectId);
        (new DBTableAdapters.bp_rish_project_itemTableAdapter()).FillByProjectID(dataset.bp_rish_project_item, projectId);
        (new DBTableAdapters.bp_rish_project_tableTableAdapter()).FillByProjectID(dataset.bp_rish_project_table, projectId);
        (new DBTableAdapters.dict_rish_project_stateTableAdapter()).Fill(dataset.dict_rish_project_state);
        (new DBTableAdapters.dict_obj_rightsTableAdapter()).Fill(dataset.dict_obj_rights);
        (new DBTableAdapters.dict_rish_project_typeTableAdapter()).Fill(dataset.dict_rish_project_type);

        foreach (DB.bp_rish_projectRow projectOrAppendixRow in dataset.bp_rish_project)
        {
            if (projectOrAppendixRow.Isapp_for_project_idNull())
                this.Add(new RishProjectMainDocItem(this, projectOrAppendixRow));
            else
                this.Add(new RishProjectAppendixItem(this, projectOrAppendixRow));
        }

        foreach (DB.bp_rish_project_itemRow itemRow in dataset.bp_rish_project_item)
        {
            // Table contents are not shown in the main tree
            if (!itemRow.Isparent_item_idNull() && itemRow.bp_rish_project_itemRowParent.is_table)
                continue;

            this.Add(new RishProjectDataSourceItem(this, itemRow));
        }

        RefreshOrdinals();
    }

    public void SaveToDatabase()
    {
        if (!dataset.HasChanges())
            return;

        MembershipUser membershipUser = Membership.GetUser();
        Guid currentUserID = (Guid)membershipUser.ProviderUserKey;
        string currentUserName = membershipUser.UserName;

        var mainDoc = GetMainDoc();
        if (!string.IsNullOrEmpty(mainDoc.documentNum) && mainDoc.documentDate != null)
        {
            (new DBTableAdapters.documentsTableAdapter()).FillByDocumentIdentity(
                dataset.documents,
                mainDoc.GetRozpDocumentTypeID(),
                mainDoc.documentNum.Trim(),
                mainDoc.documentDate
                );

            if (mainDoc.documentId == null)
            {
                if (dataset.documents.Count > 0)
                {
                    mainDoc.documentId = dataset.documents[0].id;
                }
                else
                {
                    // Create a new document
                    DB.documentsRow documentRow = dataset.documents.AdddocumentsRow(
                        (new DBTableAdapters.documentsTableAdapter()).GetNextAvailableID() ?? 1,
                        mainDoc.GetRozpDocumentTypeID().Value,
                        7, // general_kind_id = 7; // закріплення майна
                        mainDoc.documentDate.Value,
                        mainDoc.documentNum,
                        mainDoc.subject,
                        currentUserName,
                        DateTime.Now);
                    mainDoc.documentId = documentRow.id;
                }
            }
            else
            {
                if (dataset.documents.FindByid(mainDoc.documentId.Value) == null)
                {
                    // Load the currently referenced document and mercilessly mangle it
                    // to conform to the entered parameters.
                    dataset.documents.Clear();
                    (new DBTableAdapters.documentsTableAdapter()).FillByID(dataset.documents, mainDoc.documentId.Value);

                    // There must be a document (database referential integrity takes care of that)
                    DB.documentsRow documentRow = dataset.documents[0];

                    documentRow.doc_num = mainDoc.documentNum;
                    documentRow.doc_date = mainDoc.documentDate.Value;
                    documentRow.modified_by = currentUserName;
                    documentRow.modify_date = DateTime.Now;
                }
            }
        }
        else if (mainDoc.documentId != null)
        {
            // Reset the document ID, since the document identity is no longer available
            mainDoc.documentId = null;
        }

        using (DisposableScope s = new DisposableScope())
        {
            // Schedule external document file deletion for rows that are no longer in use. This applies also
            // to rows which were updated, and obviously the old file will no longer be referenced. To ensure
            // that the on-commit handler works with the correct data, we're going to evaluate the file name
            // at the DataRowVersion.Original data position presently, and pass it as a paramter to the on-
            // commit action.
            foreach (string fileName in dataset.external_document
                .Where(x => x.RowState == DataRowState.Deleted || x.RowState == DataRowState.Detached || x.RowState == DataRowState.Modified)
                .Where(x => x.RowState != DataRowState.Modified || (string)x[dataset.external_document.unique_filenameColumn, DataRowVersion.Original] != (string)x[dataset.external_document.unique_filenameColumn, DataRowVersion.Current])
                .Select(x => (string)x[dataset.external_document.unique_filenameColumn, DataRowVersion.Original]))
            {
                s.OnCommit += delegate()
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch
                    {
                    }
                };
            }

            // The external documents that are new or remain referenced, we "commit" to the main storage.
            foreach (DB.external_documentRow row in
                dataset.external_document.Where(x => x.RowState == DataRowState.Added || x.RowState == DataRowState.Modified))
            {
                if (string.IsNullOrEmpty(row.unique_filename))
                    continue;

                string updatedTargetFile = ExternalDocument.Commit(row.unique_filename);
                if (row.unique_filename != updatedTargetFile)
                    row.unique_filename = updatedTargetFile;
            }

            using (SqlConnection conn = Utils.ConnectToDatabase())
            {
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    ProjectRow.Getbp_rish_project_infoRows().First().modified_by = currentUserID;
                    ProjectRow.Getbp_rish_project_infoRows().First().modify_date = DateTime.Now;

                    transaction.NewAdapter<DBTableAdapters.documentsTableAdapter>().Update(dataset);

                    transaction.NewAdapter<DBTableAdapters.external_documentTableAdapter>().Update(dataset);
                    transaction.NewAdapter<DBTableAdapters.bp_rish_projectTableAdapter>().Update(dataset);
                    transaction.NewAdapter<DBTableAdapters.bp_rish_project_infoTableAdapter>().Update(dataset);
                    transaction.NewAdapter<DBTableAdapters.bp_rish_project_stateTableAdapter>().Update(dataset);
                    
                    transaction.NewAdapter<DBTableAdapters.bp_rish_project_itemTableAdapter>().Update(dataset);
                    transaction.NewAdapter<DBTableAdapters.bp_rish_project_tableTableAdapter>().Update(dataset);

                    transaction.Commit();
                    s.Commit();
                }
            }
        }
    }

    /// <summary>
    /// Get the status whether the document contains any Tables that are populated with data.
    /// </summary>
    public bool HasTables
    {
        get
        {
            return Context.bp_rish_project_table.Any();
        }
    }

    #region IListSource Members

    public bool ContainsListCollection
    {
        get { return this.Count > 0; }
    }

    public System.Collections.IList GetList()
    {
        return this;
    }

    #endregion

    public RishProjectMainDocItem GetMainDoc()
    {
        foreach (IRishProjectNode node in this)
        {
            if (node is RishProjectMainDocItem)
            {
                return node as RishProjectMainDocItem;
            }
        }

        return null;
    }

    /// <summary>
    /// Sort by ordinal position and regenerate ordinal position numbers
    /// </summary>
    public void RefreshOrdinals()
    {
        foreach (IGrouping<int, IRishProjectNode> group in this.GroupBy(x => x.ParentID))
        {
            IRishProjectNode[] sorted = group.OrderBy(x => x.OrdinalPos).ToArray();

            for (int index = 0; index < sorted.Length; index++)
            {
                sorted[index].OrdinalPos = index + 1;
            }
        }

        this.Sort(1, this.Count - 1, IRishProjectNode_Comparer.Comparer);
    }

    private class IRishProjectNode_Comparer : IComparer<IRishProjectNode>
    {
        public static readonly IRishProjectNode_Comparer Comparer = new IRishProjectNode_Comparer();

        private IRishProjectNode_Comparer()
        {
        }

        #region IComparer<IRishProjectNode> Members

        public int Compare(IRishProjectNode x, IRishProjectNode y)
        {
            int parentComparison = x.ParentID.CompareTo(y.ParentID);
            return parentComparison == 0 ? x.OrdinalPos.CompareTo(y.OrdinalPos) : parentComparison;
        }

        #endregion
    }

    public IEnumerable<IRishProjectNode> GetAllChildrenOfID(int id)
    {
        List<int> scope = new List<int> { id };
        while (scope.Count > 0)
        {
            List<int> newScope = new List<int>();
            foreach (int parentID in scope)
            {
                foreach (IRishProjectNode child in this.Where(x => x.ParentID == parentID))
                {
                    yield return child;
                    newScope.Add(child.ID);
                }
            }
            scope = newScope;
        }
    }

    public RishProjectDataSourceItem GetItem(int id)
    {
        foreach (IRishProjectNode node in this)
        {
            if (node is RishProjectDataSourceItem)
            {
                RishProjectDataSourceItem item = node as RishProjectDataSourceItem;

                if (item.ID == id)
                {
                    return item;
                }
            }
        }

        return null;
    }

    public RishProjectDataSourceItem GetTable(int parentId)
    {
        foreach (IRishProjectNode node in this)
        {
            if (node is RishProjectDataSourceItem)
            {
                RishProjectDataSourceItem item = node as RishProjectDataSourceItem;

                if (item.ParentID == parentId)
                {
                    return item;
                }
            }
        }

        return null;
    }

    public IEnumerable<RishProjectDataSourceItem> GetAllTables(bool skipEmpty = false)
    {
        foreach (IRishProjectNode node in this)
        {
            if (node is RishProjectDataSourceItem)
            {
                RishProjectDataSourceItem item = node as RishProjectDataSourceItem;

                if (item.isTable)
                {
                    if (skipEmpty && item.Row.Getbp_rish_project_tableRows().Length == 0)
                        continue;

                    yield return item;
                }
            }
        }
    }
    
    public sealed class OrganizationDesc
    {
        public static readonly OrganizationDesc Empty = new OrganizationDesc();

        public interface IEditorState
        {
            void Commit();
            string GetDisplayText();
        }

        public enum Mode
        {
            From,
            To,
        }

        public int? OrganizationID { get; set; }
        public Mode OpMode { get; set; }

        /// <summary>
        /// Editors are encouraged to use this reference in order to store
        /// their intermediate state until such time a "save changes" operation is
        /// requested.
        /// </summary>
        public IEditorState EditorState { get; set; }

        public static OrganizationDesc FromData(DB.bp_rish_project_itemRow row, Mode mode)
        {
            return new OrganizationDesc()
                {
                    OrganizationID = (mode == Mode.From
                        ? row.Isorg_from_idNull() ? null : (int?)row.org_from_id
                        : row.Isorg_to_idNull() ? null : (int?)row.org_to_id),
                    OpMode = mode,
                };
        }

        public override string ToString()
        {
            return (EditorState == null ? GetDisplayText() : EditorState.GetDisplayText());
        }

        public string GetDisplayText()
        {
            if (OrganizationID == null)
                return string.Empty;
            return RishProjectDataSourceItem.FormatOrganizationInfo(OrganizationID.Value);
        }
    }

    public sealed class ObjectOrAddressDesc
    {
        public static readonly ObjectOrAddressDesc Empty = new ObjectOrAddressDesc();

        public interface IEditorState
        {
            void Commit();
            string GetDisplayText();
        }

        public int? BuildingID { get; set; }
        public int? BalansID { get; set; }
        public string ArbitraryText { get; set; }

        /// <summary>
        /// Editors are encouraged to use this reference in order to store
        /// their intermediate state until such time a "save changes" operation is
        /// requested.
        /// </summary>
        public IEditorState EditorState { get; set; }

        public override string ToString()
        {
            return (EditorState == null ? GetDisplayText() : EditorState.GetDisplayText());
        }

        public string GetDisplayText()
        {
            return RishProjectDataSourceItem.FormatObjectOrAddressInfo(BuildingID ?? 0, BalansID ?? 0, ArbitraryText);
        }
    }

    /// <summary>
    /// Create a datasource suitable for the table editor functionality.
    /// </summary>
    /// <remarks>
    /// The resulting datasource is completely independent from the main datasource, and can be
    /// changed freely without concern for contaminating the main datasource with unwanted changes.
    /// This allows to properly support cancelation of table editing activities.
    /// </remarks>
    public DataSet CreateTableDataSource(int tableItemID)
    {
        DataSet datasource = new DataSet();

        DataTable rowsTable = datasource.Tables.Add("rows");

        DataColumn rowsTableIdColumn = rowsTable.Columns.Add("id", typeof(int));
        rowsTableIdColumn.AutoIncrement = true;
        rowsTableIdColumn.AutoIncrementSeed = -1;
        rowsTableIdColumn.AutoIncrementStep = -1;
        rowsTableIdColumn.AllowDBNull = false;
        rowsTableIdColumn.Unique = true;

        rowsTable.PrimaryKey = new DataColumn[] { rowsTableIdColumn };
        rowsTable.Columns.Add("name", typeof(string));
        rowsTable.Columns.Add("address", typeof(string));
        rowsTable.Columns.Add("addr_street_name", typeof(string));
        rowsTable.Columns.Add("addr_nomer", typeof(string));
        rowsTable.Columns.Add("addr_misc", typeof(string));
        rowsTable.Columns.Add("addr_distr", typeof(string));
        rowsTable.Columns.Add("year_built", typeof(int));
        rowsTable.Columns.Add("sqr_total", typeof(decimal));
        rowsTable.Columns.Add("inv_number", typeof(string));
        rowsTable.Columns.Add("location", typeof(string));
        rowsTable.Columns.Add("commissioned_date", typeof(DateTime));
        rowsTable.Columns.Add("initial_cost", typeof(decimal));
        rowsTable.Columns.Add("remaining_cost", typeof(decimal));
        rowsTable.Columns.Add("obj_kind", typeof(string));
        rowsTable.Columns.Add("obj_type", typeof(string));

        foreach (DB.bp_rish_project_tableRow row in dataset.bp_rish_project_table
            .ActiveRows<DB.bp_rish_project_tableRow>()
            .Where(x => x.bp_rish_project_item_id == tableItemID))
        {
            DataRow rowsTableRow = rowsTable.NewRow();
            rowsTableRow["id"] = row.id;
            if (!row.IsaddressNull())
                rowsTableRow["address"] = row.address;
            if (!row.Isaddr_street_nameNull())
                rowsTableRow["addr_street_name"] = row.addr_street_name;
            if (!row.Isaddr_nomerNull())
                rowsTableRow["addr_nomer"] = row.addr_nomer;
            if (!row.Isaddr_miscNull())
                rowsTableRow["addr_misc"] = row.addr_misc;
            if (!row.Isaddr_distrNull())
                rowsTableRow["addr_distr"] = row.addr_distr;
            if (!row.Iscommissioned_dateNull())
                rowsTableRow["commissioned_date"] = row.commissioned_date;
            if (!row.Isinitial_costNull())
                rowsTableRow["initial_cost"] = row.initial_cost;
            if (!row.Isinv_numberNull())
                rowsTableRow["inv_number"] = row.inv_number;
            if (!row.IslocationNull())
                rowsTableRow["location"] = row.location;
            if (!row.IsnameNull())
                rowsTableRow["name"] = row.name;
            if (!row.Isremaining_costNull())
                rowsTableRow["remaining_cost"] = row.remaining_cost;
            if (!row.Issqr_totalNull())
                rowsTableRow["sqr_total"] = row.sqr_total;
            if (!row.Isyear_builtNull())
                rowsTableRow["year_built"] = row.year_built;
            if (!row.Isobj_kindNull())
                rowsTableRow["obj_kind"] = row.obj_kind;
            if (!row.Isobj_typeNull())
                rowsTableRow["obj_type"] = row.obj_type;
            rowsTable.Rows.Add(rowsTableRow);
        }

        // In order to be able identify changes introduced to the datasource, we shall want to
        // mark the baseline state as "unmodified", therefore everything that is not "unmodified"
        // found at the commit stage is a change candidate.
        datasource.AcceptChanges();

        return datasource;
    }

    /// <summary>
    /// Commit the datasource obtained from CreateTableDataSource() call back to the main datasource.
    /// </summary>
    public void CommitTableDataSource(int tableItemID, DataSet datasource)
    {
        DB.bp_rish_project_itemRow tableItem = dataset.bp_rish_project_item.FindByid(tableItemID);

        if (tableItem == null)
            return;

        DataTable rowsTable = datasource.Tables["rows"];
        foreach (DataRow rowsTableRow in rowsTable.Rows.Cast<DataRow>())
        {
            if (rowsTableRow.RowState == DataRowState.Deleted || rowsTableRow.RowState == DataRowState.Detached)
            {
                foreach (DB.bp_rish_project_tableRow row in
                    tableItem.Getbp_rish_project_tableRows().Where(x => x.id == (int)rowsTableRow["id", DataRowVersion.Original]))
                {
                    row.Delete();
                }
            }
            else if (rowsTableRow.RowState == DataRowState.Modified)
            {
                DB.bp_rish_project_tableRow row =
                    tableItem.Getbp_rish_project_tableRows().First(x => x.id == (int)rowsTableRow["id"]);
                if (rowsTableRow.IsNull("name"))
                    row.SetnameNull();
                else
                    row.name = (string)rowsTableRow["name"];
                if (rowsTableRow.IsNull("address"))
                    row.SetaddressNull();
                else
                    row.address = (string)rowsTableRow["address"];
                if (rowsTableRow.IsNull("addr_street_name"))
                    row.Setaddr_street_nameNull();
                else
                    row.addr_street_name = (string)rowsTableRow["addr_street_name"];
                if (rowsTableRow.IsNull("addr_nomer"))
                    row.Setaddr_nomerNull();
                else
                    row.addr_nomer = (string)rowsTableRow["addr_nomer"];
                if (rowsTableRow.IsNull("addr_misc"))
                    row.Setaddr_miscNull();
                else
                    row.addr_misc = (string)rowsTableRow["addr_misc"];
                if (rowsTableRow.IsNull("addr_distr"))
                    row.Setaddr_distrNull();
                else
                    row.addr_distr = (string)rowsTableRow["addr_distr"];
                if (rowsTableRow.IsNull("year_built"))
                    row.Setyear_builtNull();
                else
                    row.year_built = (int)rowsTableRow["year_built"];
                if (rowsTableRow.IsNull("sqr_total"))
                    row.Setsqr_totalNull();
                else
                    row.sqr_total = (decimal)rowsTableRow["sqr_total"];
                if (rowsTableRow.IsNull("inv_number"))
                    row.Setinv_numberNull();
                else
                    row.inv_number = (string)rowsTableRow["inv_number"];
                if (rowsTableRow.IsNull("initial_cost"))
                    row.Setinitial_costNull();
                else
                    row.initial_cost = (decimal)rowsTableRow["initial_cost"];
                if (rowsTableRow.IsNull("remaining_cost"))
                    row.Setremaining_costNull();
                else
                    row.remaining_cost = (decimal)rowsTableRow["remaining_cost"];
                if (rowsTableRow.IsNull("location"))
                    row.SetlocationNull();
                else
                    row.location = (string)rowsTableRow["location"];
                if (rowsTableRow.IsNull("commissioned_date"))
                    row.Setcommissioned_dateNull();
                else
                    row.commissioned_date = (DateTime)rowsTableRow["commissioned_date"];
                if (rowsTableRow.IsNull("obj_kind"))
                    row.Setobj_kindNull();
                else
                    row.obj_kind = (string)rowsTableRow["obj_kind"];
                if (rowsTableRow.IsNull("obj_type"))
                    row.Setobj_typeNull();
                else
                    row.obj_type = (string)rowsTableRow["obj_type"];
            }
            else if (rowsTableRow.RowState == DataRowState.Added)
            {
                DB.bp_rish_project_tableRow row = ((DB)tableItem.Table.DataSet).bp_rish_project_table.Addbp_rish_project_tableRow(
                    tableItem,
                    rowsTableRow.IsNull("name") ? null : (string)rowsTableRow["name"],
                    rowsTableRow.IsNull("address") ? null : (string)rowsTableRow["address"],
                    rowsTableRow.IsNull("year_built") ? 0 : (int)rowsTableRow["year_built"],
                    rowsTableRow.IsNull("sqr_total") ? 0m : (decimal)rowsTableRow["sqr_total"],
                    rowsTableRow.IsNull("inv_number") ? null : (string)rowsTableRow["inv_number"],
                    rowsTableRow.IsNull("initial_cost") ? 0m : (decimal)rowsTableRow["initial_cost"],
                    rowsTableRow.IsNull("remaining_cost") ? 0m : (decimal)rowsTableRow["remaining_cost"],
                    rowsTableRow.IsNull("location") ? null : (string)rowsTableRow["location"],
                    rowsTableRow.IsNull("commissioned_date") ? DateTime.Now : (DateTime)rowsTableRow["commissioned_date"],
                    false,
                    rowsTableRow.IsNull("addr_nomer") ? null : (string)rowsTableRow["addr_nomer"],
                    rowsTableRow.IsNull("addr_misc") ? null : (string)rowsTableRow["addr_misc"],
                    rowsTableRow.IsNull("addr_street_name") ? null : (string)rowsTableRow["addr_street_name"],
                    rowsTableRow.IsNull("addr_distr") ? null : (string)rowsTableRow["addr_distr"],
                    rowsTableRow.IsNull("obj_kind") ? null : (string)rowsTableRow["obj_kind"],
                    rowsTableRow.IsNull("obj_type") ? null : (string)rowsTableRow["obj_type"]
                    );
                if (rowsTableRow.IsNull("year_built"))
                    row.Setyear_builtNull();
                if (rowsTableRow.IsNull("sqr_total"))
                    row.Setsqr_totalNull();
                if (rowsTableRow.IsNull("initial_cost"))
                    row.Setinitial_costNull();
                if (rowsTableRow.IsNull("remaining_cost"))
                    row.Setremaining_costNull();
                if (rowsTableRow.IsNull("commissioned_date"))
                    row.Setcommissioned_dateNull();
                row.Setis_acted_onNull();
            }
        }

        // As we have finished synchronizing the changes, we are moving the baseline mark to the current state.
        datasource.AcceptChanges();
    }
}
