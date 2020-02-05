using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI;
using System.Data;
using System.Collections;

/// <summary>
/// Represents the main document node of the Hierarchical data source for the Rishennya project document tree
/// </summary>
public class RishProjectMainDocItem : RishProjectAppendixItem
{
    private readonly DB.bp_rish_project_infoRow infoRow;
    private readonly ArrayList coverLettersGridDataSource = new ArrayList();

    public class CoverLettersGridItem
    {
        private readonly DB.bp_rish_project_stateRow state;

        public int id
        {
            get { return state.id; }
        }
        public string state_name
        {
            get { return ((DB)state.Table.DataSet).dict_rish_project_state.FindByid(state.state_id).name; }
        }
        public string cover_letter_no
        {
            get { return state.Iscover_letter_noNull() ? null : state.cover_letter_no; }
            set { if (value == null) state.Setcover_letter_noNull(); else state.cover_letter_no = value; }
        }
        public DateTime? cover_letter_date
        {
            get { return state.Iscover_letter_dateNull() ? (DateTime?)null : state.cover_letter_date; }
            set { if (value == null) state.Setcover_letter_dateNull(); else state.cover_letter_date = value.Value; }
        }

        public CoverLettersGridItem(DB.bp_rish_project_stateRow state)
        {
            this.state = state;
        }
    }

    public int? documentId { get { return infoRow.Isdocument_idNull() ? (int?)null : infoRow.document_id; } set { if (value == null) infoRow.Setdocument_idNull(); else infoRow.document_id = value.Value; } }
    public string documentNum { get { return infoRow.Isdocument_numNull() ? string.Empty : infoRow.document_num; } set { infoRow.document_num = value; } }
    public DateTime? documentDate { get { return infoRow.Isdocument_dateNull() ? null : (DateTime?)infoRow.document_date; } set { if (value == null) infoRow.Setdocument_dateNull(); else infoRow.document_date = value.Value; } }
    public string stateIds 
    { 
        get 
        {
            return string.Join(",",
                infoRow.bp_rish_projectRow.Getbp_rish_project_stateRows()
                    .Where(x => x.RowState != DataRowState.Deleted && x.RowState != DataRowState.Detached)
                    .Where(x => x.Isexited_onNull())
                    .Select(x => x.state_id.ToString())
                );
        } 
        set 
        {
            if (value == null)
                throw new ArgumentNullException();
            if (value.Length == 0)
                throw new ArgumentOutOfRangeException();
            int[] value0 = value
                .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Where(x => { int v; return int.TryParse(x, out v); })
                .Select(x => int.Parse(x))
                .ToArray();
            if (value0.Length == 0)
                throw new ArgumentOutOfRangeException();

            Guid currentUserId = (Guid)System.Web.Security.Membership.GetUser().ProviderUserKey;

            DB.bp_rish_project_stateRow[] stateRows = infoRow.bp_rish_projectRow.Getbp_rish_project_stateRows();
            foreach (DB.bp_rish_project_stateRow row2delete in stateRows.Where(x => !value0.Contains(x.state_id)))
            {
                if (row2delete.RowState == DataRowState.Added)
                {
                    // This was added, but hasn't been committed to the database yet - we just
                    // negate the addition operation, and be done with it.
                    row2delete.Delete();
                }
                else if (row2delete.RowState != DataRowState.Deleted && row2delete.RowState != DataRowState.Detached)
                {
                    // This was already committed to the database, therefore we just mark it
                    // as "exited" using the current date/time and user ID.
                    row2delete.exited_by = currentUserId;
                    row2delete.exited_on = DateTime.Now;
                }
            }

            foreach (int state2add in value0.Where(x => !stateRows
                .Where(y => y.RowState != DataRowState.Deleted && y.RowState != DataRowState.Detached)
                .Where(y => y.Isexited_onNull())
                .Any(y => y.state_id == x)))
            {
                DB.bp_rish_project_stateRow newStateRow = ((DB)infoRow.Table.DataSet).bp_rish_project_state.Addbp_rish_project_stateRow(
                    infoRow.bp_rish_projectRow,
                    state2add,
                    DateTime.Now,
                    currentUserId,
                    DateTime.Now,
                    currentUserId,
                    null,
                    DateTime.Now
                    );
                newStateRow.Setexited_byNull();
                newStateRow.Setexited_onNull();
                newStateRow.Setcover_letter_dateNull();
                newStateRow.Setcover_letter_noNull();
            }
        } 
    }
    public int projectTypeId { get { return infoRow.project_type_id; } set { infoRow.project_type_id = value; } }
    public int projectContactId { get { return infoRow.project_contact_id; } set { infoRow.project_contact_id = value; } }
    public string subject { get { return infoRow.IssubjectNull() ? string.Empty : infoRow.subject; } set { infoRow.subject = value; } }
    public Guid creatorId { get { return infoRow.created_by; } set { infoRow.created_by = value; } }
    public DateTime creationDate { get { return infoRow.create_date; } set { infoRow.create_date = value; } }
    public Guid lastSavedUserId { get { return infoRow.modified_by; } set { infoRow.modified_by = value; } }
    public DateTime lastSaveDate { get { return infoRow.modify_date; } set { infoRow.modify_date = value; } }
    public bool isExportedToNJF { get { return infoRow.Isis_exportedNull() ? false : infoRow.is_exported > 0; } set { infoRow.is_exported = value ? 1 : 0; } }

    public RishProjectMainDocItem(RishProjectDataSource owner, DB.bp_rish_projectRow row)
        : base(owner, row)
	{
        infoRow = row.Getbp_rish_project_infoRows().First();
    }

    public IEnumerable<CoverLettersGridItem> CoverLettersGridDataSource
    {
        get
        {
            DB context = (DB)infoRow.Table.DataSet;
            return context.bp_rish_project_state
                .Where(x => x.RowState != DataRowState.Deleted && x.RowState != DataRowState.Detached)
                .Where(x => x.Isexited_onNull())
                .Where(x => ((RishProjectState)context.dict_rish_project_state.FindByid(x.state_id).flags & RishProjectState.RequresCoverLetter) == RishProjectState.RequresCoverLetter)
                .Select(x => new CoverLettersGridItem(x));
        }
    }

    #region IRishProjectNode Members

    public override string DisplayNodeType
    {
        get
        {
            return "Документ";
        }
    }

    public override int OrdinalPos
    {
        get 
        { 
            // The document item is always first (followed by appendixes)
            return 0; 
        }

        set
        {
        }
    }

    public override void BeforeRemove()
    {
        // Main documents are not deleted this way
        throw new InvalidOperationException();
    }

    #endregion

    /// <summary>
    /// This function should determine the actual 'Rozporadjennia' DB document type
    /// for RISHENNYA and ROZPORADJENNIA projects
    /// </summary>
    /// <returns></returns>
    public string GetRozpDocumentTypeName()
    {
        DB context = (DB)infoRow.Table.DataSet;
        return context.dict_rish_project_type.FindByid(projectTypeId).name;
    }

    public int? GetRozpDocumentTypeID()
    {
        switch (projectTypeId)
        {
            case 1:
                return 5;
            case 2:
                return 1;
            case 3:
                return 44;
        }
        return null;
    }
}