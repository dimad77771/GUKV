using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using System.Data.SqlClient;
using DevExpress.Web;
using System.IO;

public partial class BPRishProject_RishProjectList : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NewRishContact.JSProperties["cpSelectedIndex"] = -1;
        NewContactOrganization.JSProperties["cpSelectedIndex"] = -1;

        if (!IsPostBack)
        {
            NewRishState.Value = string.Join(",",
                (new DBTableAdapters.dict_rish_project_stateTableAdapter()).GetData()
                    .Where(x => (RishProjectState)x.flags == RishProjectState.None)
                    .Select(x => x.id.ToString())
                );
        }
    }

    private class NewDocData
    {
        public string Name { get; set; }
        public int ContactID { get; set; }
        public int DocTypeID { get; set; }
    }

    protected void GridViewRishProjects_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Parameters))
            return;

        if (e.Parameters.StartsWith("init:"))
            return;

        NewDocData data = e.Parameters.FromJSON<NewDocData>();

        if (string.IsNullOrEmpty(data.Name))
            return;
        if (data.ContactID <= 0)
            return;
        if (data.DocTypeID <= 0)
            return;

        MembershipUser currentUser = Membership.GetUser();
        Guid currentUserGUID = (Guid)currentUser.ProviderUserKey;

        DB dataset = new DB();

        DB.bp_rish_projectRow projRow = dataset.bp_rish_project
            .Addbp_rish_projectRow(data.Name, null, null, null, 0, null, false);
        projRow.Setordinal_posNull();

        (new DBTableAdapters.dict_rish_project_org_contactTableAdapter()).Fill(dataset.dict_rish_project_org_contact);
        DB.dict_rish_project_org_contactRow contactRow = dataset.dict_rish_project_org_contact.FindByid(data.ContactID);
        
        DB.bp_rish_project_infoRow infoRow = dataset.bp_rish_project_info.Addbp_rish_project_infoRow(
            projRow, null, default(DateTime), currentUserGUID, DateTime.Now, 
            currentUserGUID, DateTime.Now, data.DocTypeID, contactRow, null, 0, 0);
        infoRow.Setdocument_dateNull();
        infoRow.Setdocument_idNull();

        // Assign the well-known initial state
        (new DBTableAdapters.dict_rish_project_stateTableAdapter()).Fill(dataset.dict_rish_project_state);
        foreach (int stateID in dataset
            .dict_rish_project_state.Where(x => (RishProjectState)x.flags == RishProjectState.None)
            .Select(x => x.id))
        {
            DB.bp_rish_project_stateRow stateRow = dataset.bp_rish_project_state.Addbp_rish_project_stateRow(
                projRow, stateID, DateTime.Now, currentUserGUID, DateTime.Now, currentUserGUID, null, DateTime.Now
                );
            stateRow.Setexited_byNull();
            stateRow.Setexited_onNull();
            stateRow.Setcover_letter_dateNull();
            stateRow.Setcover_letter_noNull();
        }

        (new DBTableAdapters.bp_rish_projectTableAdapter()).Update(dataset);
        (new DBTableAdapters.bp_rish_project_infoTableAdapter()).Update(dataset);
        (new DBTableAdapters.bp_rish_project_stateTableAdapter()).Update(dataset);

        GridViewRishProjects.DataSourceID = "SqlDataSourceRishProjects";
        GridViewRishProjects.DataBind();
    }

    protected void GridViewRishProjects_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        if (e.ButtonID == "Delete")
        {
            object id = GridViewRishProjects.GetRowValues(e.VisibleIndex, "id");

            if (id is int)
            {
                SqlConnection connection = Utils.ConnectToDatabase();

                if (connection != null)
                {
                    Utils.RishProjectDelete((int)id, connection);

                    connection.Close();
                }

                GridViewRishProjects.DataSourceID = "SqlDataSourceRishProjects";
                GridViewRishProjects.DataBind();
            }
        }
        else if (e.ButtonID == "Clone")
        {
            object key = GridViewRishProjects.GetRowValues(e.VisibleIndex, "id");

            if (key is int)
            {
                int projectID = (int)key;

                DB originalData = new DB();
                (new DBTableAdapters.dict_rish_project_org_contactTableAdapter())
                    .Fill(originalData.dict_rish_project_org_contact);
                (new DBTableAdapters.external_documentTableAdapter())
                    .Fill(originalData.external_document);
                (new DBTableAdapters.bp_rish_projectTableAdapter())
                    .FillByIDOrProjectID(originalData.bp_rish_project, projectID);
                (new DBTableAdapters.bp_rish_project_infoTableAdapter())
                    .FillByProjectID(originalData.bp_rish_project_info, projectID);
                (new DBTableAdapters.bp_rish_project_itemTableAdapter())
                    .FillByProjectID(originalData.bp_rish_project_item, projectID);
                (new DBTableAdapters.dict_rish_project_stateTableAdapter())
                    .Fill(originalData.dict_rish_project_state);

                DB.bp_rish_projectRow projectRow = 
                    originalData.bp_rish_project.First(x => x.Isapp_for_project_idNull());
                
                // A unique name must be generated for the cloned document, using
                // the following format:
                // (Копія #) <original document name>
                string newDocumentName = null;

                string[] existingDocumentNames = (new DBTableAdapters.bp_rish_projectTableAdapter()).GetData()
                    .Where(x => x.Isapp_for_project_idNull()).Select(x => x.name).ToArray();
                for (int index = 1; index < int.MaxValue; index++)
                {
                    newDocumentName = string.Format("(Копія {0}) {1}", index, projectRow.name);
                    if (!existingDocumentNames.Any(x => x.Equals(newDocumentName, StringComparison.OrdinalIgnoreCase)))
                        break;
                }

                DB cloneData = new DB();
                CloneMapping mapping = new CloneMapping();
                Guid currentUserID = (Guid)Membership.GetUser().ProviderUserKey;

                int clonedProjectId = -1;

                using (DisposableScope s = new DisposableScope())
                {
                    foreach (DB.bp_rish_projectRow row in Order_bp_rish_project_ForCloning(originalData.bp_rish_project))
                    {
                        bool isProjectRow = row.Isapp_for_project_idNull();

                        DB.external_documentRow cloneAttachment = null;

                        if (!row.Isuse_external_documentNull()
                            && row.use_external_document
                            && row.external_documentRow != null)
                        {
                            string cloneAttachmentName = ExternalDocument.Copy(row.external_documentRow.unique_filename, false);

                            // We don't want to leave any copies of external document files lying around should
                            // the database transaction fail to complete. Therefore, we shall enqueue the copy
                            // file deletion on rollback.
                            s.OnRollback += delegate() { try { File.Delete(cloneAttachmentName); } catch { } };

                            cloneAttachment = cloneData.external_document
                                .Addexternal_documentRow(row.external_documentRow.name, cloneAttachmentName);
                        }

                        DB.bp_rish_projectRow cloneRow = cloneData.bp_rish_project.Addbp_rish_projectRow(
                            isProjectRow ? newDocumentName : row.name,
                            row.Isintro_textNull() ? null : row.intro_text,
                            row.Isoutro_textNull() ? null : row.outro_text,
                            row.Isapp_for_project_idNull() ? null : mapping.Get<DB.bp_rish_projectRow>(row.app_for_project_id),
                            row.Isordinal_posNull() ? 0 : row.ordinal_pos,
                            cloneAttachment,
                            row.Isuse_external_documentNull() ? false : row.use_external_document);
                        if (row.Isordinal_posNull())
                            cloneRow.Setordinal_posNull();
                        if (row.Isuse_external_documentNull())
                            cloneRow.Setuse_external_documentNull();
                        mapping.Set(row.id, cloneRow);

                        if (isProjectRow)
                        {
                            DB.bp_rish_project_infoRow infoRow = originalData.bp_rish_project_info.First();

                            DB.dict_rish_project_org_contactRow contactRow = originalData.dict_rish_project_org_contact.FindByid(infoRow.project_contact_id);

                            DB.bp_rish_project_infoRow cloneInfoRow = cloneData.bp_rish_project_info.Addbp_rish_project_infoRow(
                                cloneRow,
                                infoRow.Isdocument_numNull() ? null : infoRow.document_num,
                                infoRow.Isdocument_dateNull() ? default(DateTime) : infoRow.document_date,
                                infoRow.created_by,
                                infoRow.create_date,
                                currentUserID,
                                DateTime.Now,
                                infoRow.project_type_id,
                                contactRow,
                                infoRow.IssubjectNull() ? null : infoRow.subject,
                                infoRow.is_exported,
                                infoRow.Isdocument_idNull() ? 0 : infoRow.document_id);

                            if (infoRow.Isdocument_dateNull())
                                cloneInfoRow.Setdocument_dateNull();
                            if (infoRow.Isdocument_idNull())
                                cloneInfoRow.Setdocument_idNull();

                            // We shall assign the cloned Project the initial state(s).
                            foreach (DB.dict_rish_project_stateRow stateRow in
                                originalData.dict_rish_project_state.Where(x => (RishProjectState)x.flags == RishProjectState.None))
                            {
                                DB.bp_rish_project_stateRow cloneStateRow = cloneData.bp_rish_project_state.Addbp_rish_project_stateRow(
                                    cloneRow,
                                    stateRow.id,
                                    DateTime.Now,
                                    currentUserID,
                                    DateTime.Now,
                                    currentUserID,
                                    null,
                                    DateTime.Now
                                    );
                                cloneStateRow.Setexited_byNull();
                                cloneStateRow.Setexited_onNull();
                                cloneStateRow.Setcover_letter_noNull();
                                cloneStateRow.Setcover_letter_dateNull();
                            }
                        }
                    }

                    foreach (DB.bp_rish_project_itemRow row in Order_bp_rish_project_item_ForCloning(originalData.bp_rish_project_item))
                    {
                        DB.external_documentRow cloneAttachment = null;

                        if (!row.Isuse_external_documentNull()
                            && row.use_external_document
                            && row.external_documentRow != null)
                        {
                            string cloneAttachmentName = ExternalDocument.Copy(row.external_documentRow.unique_filename, false);

                            // We don't want to leave any copies of external document files lying around should
                            // the database transaction fail to complete. Therefore, we shall enqueue the copy
                            // file deletion on rollback.
                            s.OnRollback += delegate() { try { File.Delete(cloneAttachmentName); } catch { } };

                            cloneAttachment = cloneData.external_document
                                .Addexternal_documentRow(row.external_documentRow.name, cloneAttachmentName);
                        }

                        DB.bp_rish_project_itemRow cloneRow = cloneData.bp_rish_project_item.Addbp_rish_project_itemRow(
                            mapping.Get<DB.bp_rish_projectRow>(row.project_id),
                            row.Isparent_item_idNull() ? null : mapping.Get<DB.bp_rish_project_itemRow>(row.parent_item_id),
                            row.Isordinal_posNull() ? 0 : row.ordinal_pos,
                            row.Isintro_textNull() ? null : row.intro_text,
                            row.Isoutro_textNull() ? null : row.outro_text,
                            row.Isorg_from_idNull() ? 0 : row.org_from_id,
                            row.Isorg_to_idNull() ? 0 : row.org_to_id,
                            row.Isright_idNull() ? 0 : row.right_id,
                            row.IsexplanationNull() ? null : row.explanation,
                            row.is_table,
                            row.Isarbitrary_textNull() ? null : row.arbitrary_text,
                            cloneAttachment,
                            row.Isuse_external_documentNull() ? false : row.use_external_document,
                            row.Istable_typeNull() ? 0 : row.table_type);
                        if (row.Isordinal_posNull())
                            cloneRow.Setordinal_posNull();
                        if (row.Isorg_from_idNull())
                            cloneRow.Setorg_from_idNull();
                        if (row.Isorg_to_idNull())
                            cloneRow.Setorg_to_idNull();
                        if (row.Isright_idNull())
                            cloneRow.Setright_idNull();
                        if (row.Isuse_external_documentNull())
                            cloneRow.Setuse_external_documentNull();
                        if (row.Istable_typeNull())
                            cloneRow.Settable_typeNull();
                        mapping.Set(row.id, cloneRow);
                    }

                    using (SqlConnection conn = Utils.ConnectToDatabase())
                    {
                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            transaction.NewAdapter<DBTableAdapters.external_documentTableAdapter>().Update(cloneData);
                            transaction.NewAdapter<DBTableAdapters.bp_rish_projectTableAdapter>().Update(cloneData);
                            transaction.NewAdapter<DBTableAdapters.bp_rish_project_infoTableAdapter>().Update(cloneData);
                            transaction.NewAdapter<DBTableAdapters.bp_rish_project_stateTableAdapter>().Update(cloneData);
                            transaction.NewAdapter<DBTableAdapters.bp_rish_project_itemTableAdapter>().Update(cloneData);

                            clonedProjectId = cloneData.bp_rish_project.First().id;

                            transaction.Commit();
                            
                            // "Committing" the disposable scope in this context simply negates all
                            // instructions to delete copies of external documents.
                            s.Commit();
                        }
                    }
                }

                if (clonedProjectId == -1)
                {
                    GridViewRishProjects.DataSourceID = "SqlDataSourceRishProjects";
                    GridViewRishProjects.DataBind();
                }
                else
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback(Page.ResolveClientUrl("~/BPRishProject/RishProjectForm.aspx?projid=" + clonedProjectId.ToString()));
            }
        }
    }

    private static IEnumerable<DB.bp_rish_projectRow> Order_bp_rish_project_ForCloning(IEnumerable<DB.bp_rish_projectRow> rows)
    {
        yield return rows.First(x => x.Isapp_for_project_idNull());

        // Only one layer is supported: project <- appendix
        foreach (DB.bp_rish_projectRow row in rows.Where(x => !x.Isapp_for_project_idNull()))
            yield return row;
    }

    private static IEnumerable<DB.bp_rish_project_itemRow> Order_bp_rish_project_item_ForCloning(IEnumerable<DB.bp_rish_project_itemRow> rows)
    {
        List<int> processedIDs = new List<int>();

        // Start with the records that have no parent IDs (e.g. items)
        foreach (DB.bp_rish_project_itemRow row in rows.Where(x => x.Isparent_item_idNull()))
        {
            processedIDs.Add(row.id);
            yield return row;
        }

        // Keep processing sub-items, tables, etc. layer by layer until no layers left
        while (processedIDs.Count != rows.Count())
        {
            foreach (DB.bp_rish_project_itemRow row in rows.Where(x => !processedIDs.Contains(x.id) && processedIDs.Contains(x.parent_item_id)))
            {
                processedIDs.Add(row.id);
                yield return row;
            }
        }
    }

    private class CloneMapping
    {
        private readonly Dictionary<Type, Dictionary<int, object>> _mapping = 
            new Dictionary<Type, Dictionary<int, object>>();

        public void Set<T>(int id, T value)
        {
            Dictionary<int, object> idMap;
            if (!_mapping.TryGetValue(typeof(T), out idMap))
                _mapping.Add(typeof(T), idMap = new Dictionary<int, object>());
            idMap[id] = value;
        }

        public T Get<T>(int id)
        {
            Dictionary<int, object> idMap;
            if (!_mapping.TryGetValue(typeof(T), out idMap))
                return default(T);
            object value;
            if (!idMap.TryGetValue(id, out value))
                return default(T);
            return (T)value;
        }
    }

    protected void GridViewRishProjects_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView grid = sender as ASPxGridView;

        object[] projectIds = new object[grid.VisibleRowCount];

        for (int i = grid.VisibleStartIndex; i < grid.VisibleRowCount; i++)
        {
            projectIds[i] = grid.GetRowValues(i, "id");
        }

        e.Properties["cpProjectIds"] = projectIds;
    }

    private class NewProjectContactData
    {
        public int OrganizationID { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
    }

    protected void NewRishContact_Callback(object sender, CallbackEventArgsBase e)
    {
        if (string.IsNullOrEmpty(e.Parameter))
            return;

        NewProjectContactData data = e.Parameter.FromJSON<NewProjectContactData>();
        if (data.OrganizationID <= 0)
            return;
        if (string.IsNullOrEmpty(data.ContactName))
            return;

        DB.dict_rish_project_org_contactDataTable table = new DB.dict_rish_project_org_contactDataTable();
        DB.dict_rish_project_org_contactRow row = table
            .Adddict_rish_project_org_contactRow(data.OrganizationID, data.ContactName, data.ContactTitle, data.ContactPhone);
        try
        {
            (new DBTableAdapters.dict_rish_project_org_contactTableAdapter()).Update(table);
            // Reload the combobox contents and select the newly crerated item
            NewRishContact.DataBind();
            NewRishContact.JSProperties["cpSelectedIndex"] = NewRishContact.Items.IndexOfValue(row.id.ToString());
        }
        catch (Exception ex)
        {
            NewRishContact.DataBind();
            NewRishContact.JSProperties["cpSelectedIndex"] = -1;
            NewRishContact.JSProperties["cpErrorText"] = "Вказана відповідальна особа вже існує.";
        }
        
    }

    private class NewProjectOrgData
    {
        public string OrganizationName { get; set; }
    }

    protected void NewContactOrganization_Callback(object sender, CallbackEventArgsBase e)
    {
        if (string.IsNullOrEmpty(e.Parameter))
            return;

        NewProjectOrgData data = e.Parameter.FromJSON<NewProjectOrgData>();
        if (string.IsNullOrEmpty(data.OrganizationName))
            return;

        DB.dict_rish_project_orgDataTable table = new DB.dict_rish_project_orgDataTable();
        DB.dict_rish_project_orgRow row = table.Adddict_rish_project_orgRow(data.OrganizationName);
        (new DBTableAdapters.dict_rish_project_orgTableAdapter()).Update(table);

        // Reload the combobox contents and select the newly crerated item
        NewContactOrganization.DataBind();
        NewContactOrganization.JSProperties["cpSelectedIndex"] = NewContactOrganization.Items.IndexOfValue(row.id.ToString());
    }

    private const string ExportTitle = "Реєстр проектів Рішень та Розпоряджень";

    protected void ButtonSaveAs_ExportXLS_Click(object sender, EventArgs e)
    {
        Page.ExportGridToXLS(GridViewRishProjectsExporter, GridViewRishProjects, ExportTitle, string.Empty);
    }

    protected void ButtonSaveAs_ExportPDF_Click(object sender, EventArgs e)
    {
        Page.ExportGridToPDF(GridViewRishProjectsExporter, GridViewRishProjects, ExportTitle, string.Empty);
    }

    protected void ButtonSaveAs_ExportCSV_Click(object sender, EventArgs e)
    {
        Page.ExportGridToCSV(GridViewRishProjectsExporter, GridViewRishProjects, ExportTitle, string.Empty);
    }

    public string CutRishennyaName(object name)
    {
        if (name is string)
        {
            string str = (string)name;

            return str.Length > 85 ? str.Substring(0, 82) + "..." : str;
        }

        return "";
    }
}