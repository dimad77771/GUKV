using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web.Data;
using System.Web.Security;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DevExpress.Web.ASPxTreeList;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using NotesFor.HtmlToOpenXml;
using System.Xml.Linq;
using System.Data;
using DevExpress.Web;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;

using v = DocumentFormat.OpenXml.Vml;
using ovml = DocumentFormat.OpenXml.Vml.Office;


public partial class BPRishProject_RishProjectForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Generate unique key for the page
        GetPageUniqueKey();

        string projectIdStr = Request.QueryString["projid"];

        if (projectIdStr != null && projectIdStr.Length > 0)
        {
            ProjectID = int.Parse(projectIdStr.Trim());

            SqlDataSourceHistory.SelectParameters["projid"].DefaultValue = projectIdStr.Trim();
        }

        // Create and load the in-memory data source, if required
        if (ProjectID > 0)
        {
            // Bind the tree list to its data source
            RishProjectDataSource ds = ProjectDataSource;

            TreeListDoc.DataSource = ds;
            TreeListDoc.DataBind();
            TreeListDoc.ExpandAll();

            // Fill other form controls
            RishProjectMainDocItem doc = (RishProjectMainDocItem)ds.First();

            WorkflowStates = ds.Context.dict_rish_project_state.ToArray();
            ObjectRights = ds.Context.dict_obj_rights.ToArray();

            CoverLettersGrid.DataSource = doc.CoverLettersGridDataSource;
            CoverLettersGrid.DataBind();
            CoverLettersGrid.JSProperties["cpMissingRequiredData"] =
                doc.CoverLettersGridDataSource.Any(x => x.cover_letter_date == null || x.cover_letter_no == null);

            CoverLettersGridPanel.ClientVisible = doc.CoverLettersGridDataSource.Any();

            if (!IsCallback && !IsPostBack)
            {
                TextBoxRishName.Value = doc.name;
                TextBoxRishNum.Value = doc.documentNum;
                DateEditRishDate.Value = doc.documentDate;
                ComboProjectState.Value = doc.stateIds;
                ComboProjectType.DataBind();
                ComboProjectType.SelectedIndex = ComboProjectType.Items.IndexOfValue(doc.projectTypeId);
                TextCreatedBy.Text = Membership.GetUser(doc.creatorId).UserName;
                DateCreatedOn.Value = doc.creationDate;
                TextModifiedBy.Text = Membership.GetUser(doc.lastSavedUserId).UserName;
                DateModifiedOn.Value = doc.lastSaveDate;

                
            }
            else if (IsPostBack)
            {
                // Saving of the modified values
                doc.name = TextBoxRishName.Text;
                doc.documentNum = TextBoxRishNum.Text;
                doc.documentDate = DateEditRishDate.Value is DateTime ? (DateTime?)DateEditRishDate.Value : null;
                doc.stateIds = ComboProjectState.Value;
                doc.projectTypeId = Convert.ToInt32(ComboProjectType.SelectedItem.Value);
            }

            if (!IsCallback)
            {
                ButtonCreateNewAct.ClientEnabled = false;
                ButtonCreateNewAct.ToolTip = "Новий акт може бути створено лише коли розпорядчий документ досягне кінцевого стану та будуть заповнені його номер і дата";

                // Disable the state selection if the document has reached the final state
                if (doc.Row.Getbp_rish_project_stateRows()
                    .Where(x => x.RowState != DataRowState.Deleted && x.RowState != DataRowState.Detached)
                    .Any(x => WorkflowStates.Any(y =>
                        ((RishProjectState)y.flags & RishProjectState.Final) == RishProjectState.Final
                        && y.id == x.state_id)))
                {
                    ComboProjectState.ReadOnly = true;
                    if (TextBoxRishNum.Text.Trim().Length > 0 && DateEditRishDate.Text.Trim().Length > 0)
                    {
                        ButtonCreateNewAct.ClientEnabled = true;
                        ButtonCreateNewAct.ToolTip = "Створити новий акт";
                    }
                }

                // Also disable all controls if the document has emerged from the initial
                // (editable) state into an intermediary, or final state
                bool controlsEditable = true;
                if (doc.Row.Getbp_rish_project_stateRows()
                    .Where(x => x.RowState != DataRowState.Deleted && x.RowState != DataRowState.Detached)
                    .Where(x => x.Isexited_onNull())
                    .Any(x => !WorkflowStates.Any(y =>
                        (RishProjectState)y.flags == RishProjectState.None && y.id == x.state_id)))
                {
                    controlsEditable = false;
                }
                TextBoxRishName.ClientEnabled = 
                ComboProjectType.ClientEnabled = 
                TreeListDoc.Enabled = controlsEditable;
            }

            TreeListDoc.Templates.EditForm = new EditFormTemplateSelector(this);

            //ButtonExportToNJF.ClientEnabled = !doc.isExportedToNJF && doc.stateId == 16;
        }
        else
        {
            WorkflowStates = new DB.dict_rish_project_stateRow[0];
            ObjectRights = new DB.dict_obj_rightsRow[0];
        }
    }

    protected DB.dict_obj_rightsRow[] ObjectRights { get; set; }

    protected DB.dict_rish_project_stateRow[] WorkflowStates { get; set; }

    protected bool lastAppIsExternal = false;

    private class EditFormTemplateSelector : ITemplate
    {
        private readonly BPRishProject_RishProjectForm page;

        public EditFormTemplateSelector(BPRishProject_RishProjectForm page)
        {
            this.page = page;
        }

        #region ITemplate Members

        public void InstantiateIn(System.Web.UI.Control container)
        {
            IRishProjectNode item = page.EditedTreeItem;

            System.Web.UI.Control editor = null;

            if (item is RishProjectMainDocItem)
            {
                editor = page.LoadControl("~/BPRishProject/RishProjectMainDocEditor.ascx");
            }
            else if (item is RishProjectAppendixItem)
            {
                editor = page.LoadControl("~/BPRishProject/RishProjectAppendixEditor.ascx");
            }
            else if (item is RishProjectDataSourceItem)
            {
                if (((RishProjectDataSourceItem)item).isTable)
                {
                    editor = page.LoadControl("~/BPRishProject/RishProjectTableEditor.ascx");
                    ((RishProjectTableEditor)editor).PageUniqueKey = page.GetPageUniqueKey();
                }
                else
                {
                    editor = page.LoadControl("~/BPRishProject/RishProjectItemEditor.ascx");
                }
            }

            if (editor != null)
            {
                editor.ID = "RishProjectItemEditor1";
                container.Controls.Add(editor);
            }
        }

        #endregion
    }

    #region Tree View of the document structure

    protected void TreeListDoc_CustomCallback(object sender, TreeListCustomCallbackEventArgs e)
    {
        RishProjectDataSource ds = ProjectDataSource;

        if (ds != null)
        {
            int nodeId = int.Parse(e.Argument.Substring(8));

            // Find the node in the datasource
            IRishProjectNode node = null;

            foreach (IRishProjectNode n in ds)
            {
                if (n.ID == nodeId)
                {
                    node = n;
                    break;
                }
            }

            string editNodeKey = null;

            if (node != null)
            {
                if (e.Argument.StartsWith("delitem:"))
                {
                    if (node is RishProjectDataSourceItem && (node as RishProjectDataSourceItem).isTable)
                    {
                        DeleteTableValuesAndColumns(node as RishProjectDataSourceItem);
                    }

                    DeleteNodeRecursively(ds, node.ID);
                }
                else if (e.Argument.StartsWith("additem:"))
                {
                    RishProjectDataSourceItem newItem = new RishProjectDataSourceItem(ds, nodeId, false);
                    ds.Add(newItem);
                    editNodeKey = newItem.ID.ToString();
                }
                else if (e.Argument.StartsWith("addtble:"))
                {
                    RishProjectDataSourceItem newItem = new RishProjectDataSourceItem(ds, nodeId, true);
                    ds.Add(newItem);
                    editNodeKey = newItem.ID.ToString();
                }

                ds.RefreshOrdinals();
            }

            (sender as ASPxTreeList).DataSource = ds;
            (sender as ASPxTreeList).DataBind();
            (sender as ASPxTreeList).ExpandAll();

            if (editNodeKey != null)
                (sender as ASPxTreeList).StartEdit(editNodeKey);
        }
    }

    protected void DeleteNodeRecursively(RishProjectDataSource ds, int nodeId)
    {
        // Find all the child nodes (and the node with the specified ID)
        IRishProjectNode node = null;
        List<IRishProjectNode> nodesToDelete = new List<IRishProjectNode>();

        foreach (IRishProjectNode n in ds)
        {
            if (n.ID == nodeId)
            {
                node = n;
            }
            else if (n.ParentID == nodeId)
            {
                nodesToDelete.Add(n);
            }
        }

        // Delete the child nodes
        foreach (IRishProjectNode n in nodesToDelete)
        {
            DeleteNodeRecursively(ds, n.ID);
        }

        // Delete the rows that were not affected by the code above (because they are not represented by IRishNodeProject objects)
        foreach (DB.bp_rish_project_itemRow row in ds.Context.bp_rish_project_item)
        {
            if (row.RowState != System.Data.DataRowState.Deleted && !row.Isparent_item_idNull() && row.parent_item_id == nodeId)
            {
                row.Delete();
            }
        }

        if (node != null)
            ds.Remove(node);
    }

    protected void DeleteTableValuesAndColumns(RishProjectDataSourceItem tableItem)
    {
        // Get IDs of all the table items
        HashSet<int> tableRowIDs = new HashSet<int>();

        foreach (DB.bp_rish_project_itemRow row in tableItem.DataSource.Context.bp_rish_project_item)
        {
            if (row.RowState != System.Data.DataRowState.Deleted && !row.Isparent_item_idNull() && row.parent_item_id == tableItem.ID)
            {
                tableRowIDs.Add(row.id);
            }
        }

        tableRowIDs.Add(tableItem.ID);
    }

    private const int DocAddItemButtonIndex = 0;
    private const int DocAddTableIndex = 1;
    private const int DocDelNodeButtonIndex = 2;

    protected void TreeListDoc_CommandColumnButtonInitialize(object sender, TreeListCommandColumnButtonEventArgs e)
    {
        RishProjectDataSource ds = ProjectDataSource;

        if (ds != null && e.ButtonType == TreeListCommandColumnButtonType.Custom)
        {
            int itemId = int.Parse(e.NodeKey);

            // Find the item in the data source
            foreach (IRishProjectNode node in ds)
            {
                if (node.ID == itemId)
                {
                    if (node is RishProjectMainDocItem)
                    {
                        // Main document: show only 'Add punkt' button
                        e.Visible = (e.CustomButtonIndex == DocAddItemButtonIndex) ?
                            DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                    }
                    else if (node is RishProjectDataSourceItem)
                    {
                        RishProjectDataSourceItem item = node as RishProjectDataSourceItem;

                        if (item.isTable)
                        {
                            // Table item: show only 'Delete' button
                            e.Visible = (e.CustomButtonIndex == DocDelNodeButtonIndex) ?
                                DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                        }
                        else
                        {
                            if (ds.GetTable(item.ID) != null)
                            {
                                // Simple item with a table: show only 'Delete' button
                                e.Visible = (e.CustomButtonIndex == DocDelNodeButtonIndex) ?
                                    DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                            }
                            else
                            {
                                // Simple item without table: show 'Add table' and 'Delete' buttons
                                e.Visible = (e.CustomButtonIndex == DocAddTableIndex || e.CustomButtonIndex == DocDelNodeButtonIndex) ?
                                    DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                            }
                        }
                    }

                    break;
                }
            }
        }
    }

    protected void TreeListDoc_StartNodeEditing(object sender, TreeListNodeEditingEventArgs e)
    {
        EditedTreeItem = null;

        int itemId = int.Parse(e.NodeKey);

        RishProjectDataSource ds = ProjectDataSource;

        foreach (IRishProjectNode node in ds)
        {
            if (node.ID == itemId)
            {
                EditedTreeItem = node;
                break;
            }
        }
    }

    protected void TreeListDoc_NodeUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        // Save the user-entered values to the data source
        System.Web.UI.Control ctl = TreeListDoc.FindEditFormTemplateControl("RishProjectItemEditor1");

        // In order for the type to be recognized by the compiler, the .aspx page must include
        // <%@ Register src="RishProjectItemEditor.ascx" ...> instructions. Naturally, all flavors
        // of edit controls must be supported here.
        if (ctl is RishProjectItemEditor)
        {
            (ctl as RishProjectItemEditor).SaveChanges();
        }
        else if (ctl is RishProjectMainDocEditor)
        {
            (ctl as RishProjectMainDocEditor).SaveChanges();
        }
        else if (ctl is RishProjectAppendixEditor)
        {
            (ctl as RishProjectAppendixEditor).SaveChanges();
        }
        else if (ctl is RishProjectTableEditor)
        {
            (ctl as RishProjectTableEditor).SaveChanges();
        }

        // Stop any further processing
        (sender as ASPxTreeList).CancelEdit();
        e.Cancel = true;
    }

    protected void TreeListDoc_ProcessDragNode(object sender, TreeListNodeDragEventArgs e)
    {
        IRishProjectNode child = e.Node.DataItem as IRishProjectNode;
        IRishProjectNode newParent = e.NewParentNode.DataItem as IRishProjectNode;

        child.ParentID = newParent.ID;

        ProjectDataSource.RefreshOrdinals();

        e.Handled = true;
    }

    #endregion (Tree View of the document structure)

    #region History management

    /*
    protected void WriteHistoryEntry(SqlConnection connection, string change, string comment)
    {
        using (SqlCommand cmd = new SqlCommand("INSERT INTO bp_rish_project_history (project_id, change_text, change_note, modified_by, modify_date)" +
            " VALUES (@projid, @txt, @comm, @usr, @dt)", connection))
        {
            cmd.Parameters.Add(new SqlParameter("projid", ProjectID));
            cmd.Parameters.Add(new SqlParameter("txt", change));
            cmd.Parameters.Add(new SqlParameter("comm", comment));
            cmd.Parameters.Add(new SqlParameter("usr", Membership.GetUser().UserName));
            cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));

            cmd.ExecuteNonQuery();
        }

        // Mark the project as 'modified'
        using (SqlCommand cmd = new SqlCommand("UPDATE bp_rish_project SET modified_by = @usr, modify_date = @dt WHERE id = @projid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("projid", ProjectID));
            cmd.Parameters.Add(new SqlParameter("usr", Membership.GetUser().UserName));
            cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now.Date));

            cmd.ExecuteNonQuery();
        }
    }
    */

    protected void GridViewHistory_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        GridViewHistory.DataSourceID = "SqlDataSourceHistory";
        GridViewHistory.DataBind();
    }

    #endregion (History management)

    #region Commit to the database

    protected void ButtonSaveRishProject_Click(object sender, EventArgs e)
    {
        ProjectDataSource.SaveToDatabase();
    }

    #endregion (Commit to the database)

    #region Word report generation

    protected const string tagDocType = "{DOCTYPE}";
    protected const string tagDocTitle = "{DOCTITLE}";
    protected const string tagDocDate = "{DOCDATE}";
    protected const string tagDocNum = "{DOCNUM}";
    protected const string tagDocIntro = "{DOCINTRO}";
    protected const string tagDocOutro = "{DOCOUTRO}";
    protected const string tagDocItems = "{DOCITEMS}";

    protected const string tagAppendixStart = "{APPENDIXSTART}";
    protected const string tagAppendixTitle1 = "{APPENDIXTITLE1}";
    protected const string tagAppendixTitle = "{APPENDIXTITLE}";
    protected const string tagAppendixIntro = "{APPENDIXINTRO}";
    protected const string tagAppendixOutro = "{APPENDIXOUTRO}";
    protected const string tagAppendixItems = "{APPENDIXITEMS}";
    protected const string tagAppendixEnd = "{APPENDIXEND}";

    protected int altChunkId = 0;

    protected SectionProperties primarySectionProperties = null;

    protected const string StyleIdTableCell = "S1TABLECELL";
    protected const string StyleIdBold = "S1BOLD";

    protected void ButtonExportRishProject_Click(object sender, EventArgs e)
    {
        // Reset the auto-generated IDs of alternative document chunks
        altChunkId = 0;

        // Get the main document node
        RishProjectDataSource ds = ProjectDataSource;
        RishProjectMainDocItem mainDoc = null;

        foreach (IRishProjectNode node in ds)
        {
            if (node is RishProjectMainDocItem)
            {
                mainDoc = node as RishProjectMainDocItem;
                break;
            }
        }

        if (mainDoc != null)
        {
            if (mainDoc.useExternalDocument)
            {
                // The main document is represented by an external document. Technically speaking, the
                // "export to word" operation is invalid in this context. However, instead of generating
                // an error, we are going to yield a copy of the external document.

                // Dump the document contents to the output stream
                System.IO.FileInfo info = new System.IO.FileInfo(mainDoc.externalDocumentUniqueFileName);

                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                Response.AddHeader("content-disposition", "attachment; filename=RishProject.docx; size=" + info.Length.ToString());

                // Pipe the stream contents to the output stream
                using (System.IO.FileStream stream = System.IO.File.Open(mainDoc.externalDocumentUniqueFileName,
                    System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
                {
                    stream.CopyTo(Response.OutputStream);
                }

                Response.End();
            }
            else
            {
                // Select the proper template file
                string templateFileName = GetTemplateFileName(mainDoc);

                if (templateFileName.Length > 0)
                {
                    using (TempFile tempFile = TempFile.FromExistingFile(templateFileName))
                    {
                        using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(tempFile.FileName, true))
                        {
                            MainDocumentPart mainPart = wordDocument.MainDocumentPart;

                            if (mainPart != null)
                            {
                                AddDocumentStyles(mainPart);
                                UpdateTemplateFile(mainPart, mainDoc, ds, wordDocument);
                                
                                //string embeddedExcelPath = @"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\GUKV Import - БУД (2).xlsx";

                                //ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);
                                //imagePart.FeedData(File.Open(Server.MapPath(@"~/BPRishProject/Templates/BlankClick.png"), FileMode.Open));
                                //EmbeddedPackagePart embeddedObjectPart = mainPart.AddEmbeddedPackagePart(@"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                                //embeddedObjectPart.FeedData(File.Open(embeddedExcelPath, FileMode.Open));
                                //Paragraph p = DocUtils.CreateEmbeddedObjectParagraph(mainPart.GetIdOfPart(imagePart), mainPart.GetIdOfPart(embeddedObjectPart));
                                //Run sdt = mainPart.Document.Descendants<Run>().Where(s => s.InnerText.ToUpper().Contains("XLS_TABLE")).First();
                                //OpenXmlElement parent = sdt.Parent.Parent;
                                //parent.InsertAfter(p, sdt.Parent);
                                //sdt.Parent.Remove();
                            }

                            wordDocument.Close();

#region TEMP
                            //string containingDocumentPath = @"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\Doc2.docx";
                            //string embeddedExcelPath = @"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\GUKV Import - БУД (2).xlsx";

                            //using (WordprocessingDocument myDoc = WordprocessingDocument.Open(@"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\Doc2.docx", true))
                            //{
                            //    MainDocumentPart mainPart2 = myDoc.MainDocumentPart;
                            //    var embeddedPackagePart = mainPart2.AddNewPart<EmbeddedPackagePart>("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "rId1UUU9098");

                            //    DocUtils.GenerateEmbeddedPackagePart(embeddedPackagePart, embeddedExcelPath);
                            //}



                            //using (WordprocessingDocument myDoc = WordprocessingDocument.Open(containingDocumentPath, true))
                            //{
                            //    MainDocumentPart mainPart2 = myDoc.MainDocumentPart;
                            //    ImagePart imagePart = mainPart2.AddImagePart(ImagePartType.Png);
                            //    imagePart.FeedData(File.Open(@"d:\12494669_1024778694231558_1292043138065265713_n.png", FileMode.Open));
                            //    EmbeddedPackagePart embeddedObjectPart = mainPart2.AddEmbeddedPackagePart(@"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                            //    //embeddedObjectPart.FeedData(File.Open(embeddedExcelPath, FileMode.Open));

                            //    Paragraph p = DocUtils.CreateEmbeddedObjectParagraph(mainPart2.GetIdOfPart(imagePart), mainPart2.GetIdOfPart(embeddedObjectPart));
                            //    //SdtBlock sdt = mainPart2.Document.Descendants<SdtBlock>().Where(s => s.GetFirstChild<SdtProperties>().GetFirstChild<Aliases>().Val.Value.Equals("EmbedObject")).First();

                            //    //mainPart2.Document.Body.Append(p);

                            //    //string ss = p.OuterXml;
                            //    //ReplaceDocTag(mainPart2, null, "EmbedObject", ss, false);

                            //    //OpenXmlElement parent = sdt.Parent;
                            //    //parent.InsertAfter(p, sdt);
                            //    //sdt.Remove();
                            //    //mainPart2.Document.Save();
                            //}

                            //DocUtils.CreatePackage(containingDocumentPath, embeddedExcelPath);

                //            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(@"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\Doc2.docx", true))
                //            {
                //                MainDocumentPart mainPart2 = myDoc.MainDocumentPart;

                //                String id = "sdfsdfsdfsdf";
                //                EmbeddedObjectPart embeddedPackagePart1 = mainPart2.AddEmbeddedObjectPart("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //                embeddedPackagePart1.FeedData(File.Open(@"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\GUKV Import - БУД (2).xlsx", FileMode.Open));
                //                mainPart2.ChangeIdOfPart(embeddedPackagePart1, id);
                //                //GenerateEmbeddedPackagePart1Content(embeddedPackagePart1);


                //                Paragraph P = new Paragraph(
                //new Run(
                //  new EmbeddedObject(
                //    new v.Shape(
                //      new v.ImageData()
                //      {
                //          Title = "",
                //          RelationshipId = id
                //      }
                //    )
                //    {
                //        Id = "_x0000_i1025",
                //        Style = "width:76.5pt;height:49.5pt",
                //    },
                //    new ovml.OleObject()
                //    {
                //        Type = ovml.OleValues.Embed,
                //        ProgId = "Excel.Sheet.12",
                //        ShapeId = "_x0000_i1025",
                //        DrawAspect = ovml.OleDrawAspectValues.Icon,
                //        ObjectId = "_1299573545",
                //        Id = "rId1"
                //    }
                //  )));

                //                //ImagePart imagePart = mainPart2.AddImagePart(ImagePartType.Png);
                //                //imagePart.FeedData(File.Open("placeholder.png", FileMode.Open));
                //                //EmbeddedPackagePart embeddedObjectPart = mainPart2.AddEmbeddedPackagePart(@"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",);
                //                //embeddedObjectPart.FeedData(File.Open(@"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\GUKV Import - БУД (2).xlsx", FileMode.Open));

                //                //Paragraph P = new Paragraph(embeddedObjectPart);
                //                mainPart2.Document.Body.AppendChild(P);
                //                mainPart2.Document.Save();
                                
                //            }

                            //string containingDocumentPath = @"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\RishProject (7).docx";
                            //string embeddedExcelPath = @"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\GUKV Import - БУД (2).xlsx";
                            //DocUtils.CreatePackage(containingDocumentPath, embeddedExcelPath);



                            //EmbeddedPackagePart embeddedObjectPart = mainPart.AddEmbeddedPackagePart(@"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                            //embeddedObjectPart.FeedData(File.Open(@"C:\Users\a\Downloads\GUKV Import - БУД (2).xlsx", FileMode.Open));
                            ////mainPart.AddEmbeddedObjectPart(embeddedObjectPart);


                            //new Paragraph(new Run(embeddedObjectPart));

                            //ReplaceDocTagInElements(mainPart, null, "{XLS_TABLE}", embeddedObjectPart., false);


                            //mainPart.Document.Body.InsertBefore<OpenXmlCompositeElement>(embeddedObjectPart, mainPart.Document.Body.GET)

                            //mainPart.Document.InsertAt(embeddedObjectPart, 0);

                            //ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);
                            //imagePart.FeedData(File.Open("placeholder.png", FileMode.Open));

                            //Paragraph p = CreateEmbeddedObjectParagraph(mainPart.GetIdOfPart(imagePart),
                            //mainPart.GetIdOfPart(embeddedObjectPart);
                            //SdtBlock sdt = mainPart.Document.Descendants<SdtBlock>().Where(s => s.GetFirstChild<SdtProperties>().GetFirstChild<Alias>().Val.Value.Equals("EmbedObject")).First();
                            //OpenXmlElement parent = sdt.Parent;
                            //parent.InsertAfter(p, sdt);
                            //sdt.Remove();

                            //mainPart.Document.Save();

                            //wordDocument.Close();


//MainDocumentPart mainPart = myDoc.MainDocumentPart;
//ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);
                            //imagePart.FeedData(File.Open("placeholder.png", FileMode.Open));
                            #endregion TEMP

                            // See if there are any tables in this document that are populated
                            // with data (that is, have external documents attached to them). If
                            // that is the case, we shall form a ZIP archive including the main
                            // Word document, and all tables in their original format.
                            if (ds.HasTables)
                            {
                                using (TempFile zipTempFile = new TempFile())
                                {
                                    using (ZipFile zip = ZipFile.Create(zipTempFile.FileName))
                                    {
                                        zip.BeginUpdate();

                                        zip.Add(new StaticDiskDataSource(tempFile.FileName), 
                                            ds.GetMainDoc().GetRozpDocumentTypeName() + ".docx",
                                            CompressionMethod.Deflated,
                                            true);

                                        List<string> listToDel = new List<string>();

                                        foreach (var tableItem in ds.GetAllTables(true))
                                        {
                                            string name = string.Format("Додаток до пункту {0} ({1}).docx",
                                                ds.GetItem(tableItem.ParentID).DisplayNumber.TrimEnd('.'),
                                                Path.GetFileNameWithoutExtension(tableItem.externalDocumentName));

                                            string tempAppendixItemFileName = CreateRishProjectItemAppendix(tableItem.externalDocumentUniqueFileName, ds, tableItem);
                                            listToDel.Add(tempAppendixItemFileName);

                                            zip.Add(
                                                new StaticDiskDataSource(
                                                        //tableItem.externalDocumentUniqueFileName
                                                        tempAppendixItemFileName
                                                    ),
                                                    name,
                                                    CompressionMethod.Deflated,
                                                    true);
                                        }

                                        zip.CommitUpdate();
                                        zip.Close();

                                        foreach (string fileToDel in listToDel)
                                            File.Delete(fileToDel);
                                    }

                                    // Dump the ZIP archive contents to the output stream
                                    System.IO.FileInfo info = new System.IO.FileInfo(zipTempFile.FileName);

                                    Response.Clear();
                                    Response.ClearHeaders();
                                    Response.ClearContent();
                                    Response.ContentType = "application/zip";
                                    Response.AddHeader("content-disposition", "attachment; filename=RishProject.zip; size=" + info.Length.ToString());

                                    // Pipe the stream contents to the output stream
                                    using (System.IO.FileStream stream = System.IO.File.Open(zipTempFile.FileName,
                                        System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
                                    {
                                        stream.CopyTo(Response.OutputStream);
                                    }
                                }
                            }
                            else
                            {
                                // Dump the document contents to the output stream
                                System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

                                Response.Clear();
                                Response.ClearHeaders();
                                Response.ClearContent();
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                                Response.AddHeader("content-disposition", "attachment; filename=RishProject.docx; size=" + info.Length.ToString());

                                // Pipe the stream contents to the output stream
                                using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName,
                                    System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
                                {
                                    stream.CopyTo(Response.OutputStream);
                                }
                            }
                        }

                        Response.End();
                    }
                }
            }
        }
    }

    private string CreateRishProjectItemAppendix(string extenalFileName, RishProjectDataSource ds, RishProjectDataSourceItem tableItem)
    {
        string templateFileName = Server.MapPath(@"~/BPRishProject/Templates/AppendixTemplate.docx");

        RishProjectMainDocItem mainDoc = ds.GetMainDoc();



        string res = string.Empty;

        string tempFileName = Path.GetTempFileName();

        File.Copy(templateFileName, tempFileName, true);

        using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(tempFileName, true))
        {
            MainDocumentPart mainPart = wordDocument.MainDocumentPart;

            if (mainPart != null)
            {


                AddDocumentStyles(mainPart);

                // Find and store the SectionProperties entry of the template document
                primarySectionProperties = FindPrimarySectionElement(mainPart);

                if (primarySectionProperties != null)
                    primarySectionProperties.Remove(); // We will add it back later, when the new end of the document is determined

                

                //string embeddedExcelPath = @"d:\Work\gukv\ReportWebSite\BPRishProject\Templates\GUKV Import - БУД (2).xlsx";
                string embeddedExcelPath = tableItem.externalDocumentUniqueFileName;

                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);
                imagePart.FeedData(File.Open(Server.MapPath(@"~/BPRishProject/Templates/BlankClick.png"), FileMode.Open));
                EmbeddedPackagePart embeddedObjectPart = mainPart.AddEmbeddedPackagePart(@"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                embeddedObjectPart.FeedData(File.Open(embeddedExcelPath, FileMode.Open));
                Paragraph p = DocUtils.CreateEmbeddedObjectParagraph(mainPart.GetIdOfPart(imagePart), mainPart.GetIdOfPart(embeddedObjectPart));

                //mainPart.Document.Body.OfType<OpenXmlElement>().ToList()

                Run sdt = mainPart.Document.Descendants<Run>().Where(s => s.InnerText.ToUpper().Contains("XLS_TABLE")).First();
                OpenXmlElement parent = sdt.Parent.Parent;
                parent.InsertAfter(p, sdt.Parent);
                sdt.Parent.Remove();

                // Document title
                ReplaceDocTagInElements(mainPart, null, tagDocTitle, mainDoc.name, false);

                // Document date
                if (mainDoc.documentDate.HasValue)
                    ReplaceDocTagInElements(mainPart, null, tagDocDate, mainDoc.documentDate.Value.ToShortDateString(), false);
                else
                    ReplaceDocTagInElements(mainPart, null, tagDocDate, "", false);

                // Document number
                ReplaceDocTagInElements(mainPart, null, tagDocNum, mainDoc.documentNum, false);

                // Document intro
                ReplaceDocTagInElements(mainPart, null, tagAppendixIntro, tableItem.introText, false);

                // Document outro
                ReplaceDocTagInElements(mainPart, null, tagAppendixOutro, tableItem.outroText, true);

                // Add the last SectionProperties
                AddLastSectionProperties(mainPart);

                primarySectionProperties = null;
                //UpdateTemplateFile(mainPart, mainDoc, ds, wordDocument);
            }

            wordDocument.Close();
        }

        res = tempFileName;

        return res;
    }


    protected string GetTemplateFileName(RishProjectMainDocItem mainDoc)
    {
        switch (mainDoc.projectTypeId)
        {
            case 1:
                return Server.MapPath("Templates/RishTemplate.docx");

            case 2:
                return Server.MapPath("Templates/RozpTemplate.docx");

            case 3:
                return Server.MapPath("Templates/OrderTemplate.docx");
        }

        return "";
    }

    protected void UpdateTemplateFile(MainDocumentPart mainPart, RishProjectMainDocItem mainDoc, RishProjectDataSource ds, WordprocessingDocument doc)
    {
        // Find and store the SectionProperties entry of the template document
        primarySectionProperties = FindPrimarySectionElement(mainPart);

        if (primarySectionProperties != null)
            primarySectionProperties.Remove(); // We will add it back later, when the new end of the document is determined

        // Document type
        switch (mainDoc.projectTypeId)
        {
            case 1:
                ReplaceDocTagInElements(mainPart, null, tagDocType, "РІШЕННЯ", false);
                break;

            case 2:
                ReplaceDocTagInElements(mainPart, null, tagDocType, "РОЗПОРЯДЖЕННЯ", false);
                break;

            case 3:
                ReplaceDocTagInElements(mainPart, null, tagDocType, "НАКАЗ", false);
                break;
        }

        // Document title
        ReplaceDocTagInElements(mainPart, null, tagDocTitle, mainDoc.name, false);

        // Document date
        if (mainDoc.documentDate.HasValue)
            ReplaceDocTagInElements(mainPart, null, tagDocDate, mainDoc.documentDate.Value.ToShortDateString(), false);
        else
            ReplaceDocTagInElements(mainPart, null, tagDocDate, "", false);

        // Document number
        ReplaceDocTagInElements(mainPart, null, tagDocNum, mainDoc.documentNum, false);

        // Document intro
        ReplaceDocTagInElements(mainPart, null, tagDocIntro, mainDoc.introText, true);

        // Document outro
        ReplaceDocTagInElements(mainPart, null, tagDocOutro, mainDoc.outroText, true);

        // Find the tag that marks the place for document punkts
        List<Paragraph> list = mainPart.Document.Body.OfType<Paragraph>().ToList();

        foreach (Paragraph para in list)
        {
            string text = GetParagraphText(para);

            if (text.Contains(tagDocItems))
            {
                GeneratePunkts(ds, mainPart, para, mainDoc.ID, doc);

                // Delete the 'reference' paragraph
                para.Remove();
                break;
            }
        }

        // Generate the appendices
        List<OpenXmlElement> appendixElements = GetElementsBetweenTags(mainPart, tagAppendixStart, tagAppendixEnd, false);

        foreach (IRishProjectNode node in ds)
        {
            if (node is RishProjectAppendixItem && !(node is RishProjectMainDocItem))
            {
                GenerateAppendix(ds, mainPart, node as RishProjectAppendixItem, appendixElements, doc);
            }
        }

        // Remove the template part between {APPENDIXSTART} and {APPENDIXEND} tags
        List<OpenXmlElement> elementsToRemove = GetElementsBetweenTags(mainPart, tagAppendixStart, tagAppendixEnd, true);

        foreach (OpenXmlCompositeElement element in elementsToRemove)
        {
            element.Remove();
        }

        // Add the last SectionProperties
        AddLastSectionProperties(mainPart);

        primarySectionProperties = null;

        // now we're going to remove all empty pages
        /*
        List<Break> breaks = mainPart.Document.Descendants<Break>().ToList();
        foreach (Break b in breaks)
        {
            b.Remove();
        }
        mainPart.Document.Save();

        List<ParagraphProperties> paraProps =
            mainPart.Document.Descendants<ParagraphProperties>().Where(pPr => IsSectionProps(pPr)).ToList();
        foreach (ParagraphProperties pPr in paraProps)
        {
            pPr.RemoveChild<SectionProperties>
                (pPr.GetFirstChild<SectionProperties>());
        }
        mainPart.Document.Save();
        */
    }
    /*
    static bool IsSectionProps(ParagraphProperties pPr)
    {
        SectionProperties sectPr = pPr.GetFirstChild<SectionProperties>();
        if (sectPr == null)
            return false;
        else
            return true;
    }
    */
    protected void ReplaceDocTagInElements(MainDocumentPart mainPart,
        List<OpenXmlElement> elements, string tag, string replacement, bool isHtml)
    {
        if (elements == null)
        {
            // Take all elements of the document
            elements = mainPart.Document.Body.OfType<OpenXmlElement>().ToList();
        }

        foreach (OpenXmlElement element in elements)
        {
            if (element is Paragraph)
            {
                ReplaceDocTag(mainPart, element as Paragraph, tag, replacement, isHtml);
            }
        }
    }

    protected void ReplaceDocTag(MainDocumentPart mainPart, Paragraph para, string tag, string replacement, bool isHtml)
    {
        string paragraphText = GetParagraphText(para);

        if (isHtml)
        {
            if (paragraphText.Contains(tag))
            {
                //StripHtml(ref replacement, "");

                //altChunkId++;
                //string chunkId = "AltChunkId" + altChunkId.ToString();
                //AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, chunkId);

                replacement = "<html><head><meta charset=\"utf-8\"></head><body>" + replacement + "</body>";
                var openXmlElements = ConvertHtmlToOpenXml(mainPart, replacement);

                for (var i = 0; i < openXmlElements.Count; i++)
                {
                    mainPart.Document.Body.InsertBefore<OpenXmlCompositeElement>(openXmlElements[i], para);
                }

                /*
                string chunkId = AddHtmlToAltChunk(mainPart, replacement);

                AltChunk chunk = new AltChunk() { Id = chunkId };

                mainPart.Document.Body.InsertAfter<AltChunk>(chunk, para);
                */
                para.Remove();
            }
        }
        else
        {
            // Replace the tag
            bool replaced = false;
            int pos = paragraphText.IndexOf(tag);

            while (pos >= 0)
            {
                replaced = true;

                paragraphText = paragraphText.Replace(tag, replacement);

                // Search once again
                pos = paragraphText.IndexOf(tag);
            }

            if (replaced)
            {
                Run firstRun = para.OfType<Run>().First<Run>();

                if (firstRun == null)
                {
                    firstRun = new Run();
                }

                // Delete all paragraph sub-items
                para.RemoveAllChildren<Run>();

                // Add the text to the first Run
                firstRun.RemoveAllChildren<Text>();
                firstRun.AppendChild(new Text(paragraphText));

                // Create a new run with the modified text
                para.AppendChild(firstRun);
            }
        }
    }

    protected string GetParagraphText(Paragraph para)
    {
        string paragraphText = "";

        List<Run> runs = para.OfType<Run>().ToList();

        foreach (Run run in runs)
        {
            List<Text> texts = run.OfType<Text>().ToList();

            foreach (Text text in texts)
            {
                paragraphText += text.Text;
            }
        }

        return paragraphText;
    }

    protected List<OpenXmlElement> GetElementsBetweenTags(MainDocumentPart mainPart,
        string startTag, string endTag, bool includeTags)
    {
        List<OpenXmlElement> elementsToReturns = new List<OpenXmlElement>();
        List<OpenXmlElement> elements = mainPart.Document.Body.OfType<OpenXmlElement>().ToList();

        bool startTagEncountered = false;

        foreach (OpenXmlElement element in elements)
        {
            if (element is Paragraph)
            {
                Paragraph para = element as Paragraph;
                string paragraphText = GetParagraphText(para);

                if (startTagEncountered)
                {
                    if (paragraphText.Contains(endTag))
                    {
                        if (includeTags)
                            elementsToReturns.Add(element);

                        break;
                    }
                    else
                    {
                        elementsToReturns.Add(element);
                    }
                }
                else
                {
                    if (paragraphText.Contains(startTag))
                    {
                        if (includeTags)
                            elementsToReturns.Add(element);

                        startTagEncountered = true;
                    }
                }
            }
            else
            {
                if (startTagEncountered)
                {
                    elementsToReturns.Add(element);
                }
            }
        }

        return elementsToReturns;
    }

    protected void GeneratePunkts(RishProjectDataSource ds, MainDocumentPart mainPart, Paragraph afterThisParagraph, int appendixId, WordprocessingDocument doc)
    {
        int numberingInstanceId = AddNumberingStyleInstance(mainPart);

        List<OpenXmlElement> punkts = new List<OpenXmlElement>();

        GeneratePunktsRecursive(mainPart, ds, appendixId, punkts, string.Empty, doc);

        // Add the generated paragraphs to the document, after the specified paragraph
        OpenXmlElement prevPara = afterThisParagraph;

        foreach (OpenXmlElement para in punkts)
        {
            //var punktParagraph = new Paragraph(para);
            if (para is Paragraph)
                OpenXmlWord.ApplyStyleToParagraph(doc, "NormalItem", "Normal Item", (Paragraph)para);
            mainPart.Document.Body.InsertAfter<OpenXmlElement>(para, prevPara);
            prevPara = para;
        }
    }

    protected void GeneratePunktsRecursive(MainDocumentPart mainPart, RishProjectDataSource ds,
        int parentItemId, List<OpenXmlElement> punkts, string punktPrefix, WordprocessingDocument doc)
    {
        int punktIndex = 0;

        foreach (IRishProjectNode node in ds)
        {
            if (node is RishProjectDataSourceItem)
            {
                RishProjectDataSourceItem item = node as RishProjectDataSourceItem;

                if (item.ParentID == parentItemId)
                {
                    string punktNumber = string.Format("{0}{1}.", punktPrefix, ++punktIndex);
                    string chunkId = null;

                    if (!item.isTable)
                    {
                        // Generate an AltChunk for the item text
                        string itemText = item.outroText;

                        Utils.ProcessHyperlinks(ref itemText, OnChangeHyperlinkToSpan, null);

                        //StripHtml(ref itemText, punktNumber);

                        //chunkId = AddHtmlToAltChunk(mainPart, itemText);

                        itemText = "<html><head><meta charset=\"utf-8\"></head><body>" + itemText + "</body>";
                        InsertNumberOfArticleInHtmlDoc(ref itemText, punktNumber);

                        //itemText = "<div><span><p>Title <i> курсив </i></p> span text</span> text after span</div>";
                        //itemText = " &nbsp;";
                        //InsertNumberOfArticleInHtmlDoc(ref itemText, "777. ");


                        // Add the punkt text as a paragraph
                        /*
                        Paragraph para = new Paragraph(
                            new ParagraphProperties(
                                new NumberingProperties(
                                    new NumberingLevelReference() { Val = indentLevel },
                                    new NumberingId() { Val = numberingInstanceId }
                                )
                            ),
                            new Run(new Text(itemText))
                        );

                        punkts.Add(para);
                        */
                        var openXmlElements = ConvertHtmlToOpenXml(mainPart, itemText);

                        for (var i = 0; i < openXmlElements.Count; i++)
                        {
                            punkts.Add(openXmlElements[i]);
                        }                        


                        //punkts.Add(new AltChunk() { Id = chunkId });

                        // Add an empty paragraph between punkts
                        punkts.Add(new Paragraph(new Run(new Text(""))));

                        // If there is an explanation, add it as well
                        if (item.explanation.Length > 0)
                        {
                            /*
                            punkts.Add(
                                new Paragraph(
                                    new ParagraphProperties(
                                        new Indentation() { Left = "2880", FirstLine = "0" }),
                                    new Run(new Text(item.explanation))));
                            */

                            string explanation = item.explanation;

                            //StripHtml(ref explanation, "");

                            // Make the explanation occupy 50% of the page width
                            explanation = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\"><tr><td width=\"50%\"></td><td width=\"50%\">" +
                                explanation + "</td></tr></table>";

                            //chunkId = AddHtmlToAltChunk(mainPart, explanation);

                            //punkts.Add(new AltChunk() { Id = chunkId });
                            
                            openXmlElements = ConvertHtmlToOpenXml(mainPart, explanation);

                            for (var i = 0; i < openXmlElements.Count; i++)
                            {
                                //var punktParagraph = new Paragraph(openXmlElements[i]);
                                //OpenXmlWord.ApplyStyleToParagraph(doc, "NormalItem", "Normal Item", punktParagraph);
                                punkts.Add(openXmlElements[i]);
                            }              


                            // Add an empty line after the explanation (just for decoration)
                            punkts.Add(new Paragraph(new Run(new Text(""))));
                        }
                    }

                    // If there is a Table, add it as well
                    //
                    // ATTENTION:
                    // Tables are now considered Appendixes, while full-blown Appendixes no longer exist.
                    // HOWEVER, Tables (new Appendixes) are not exported now for the reason that we can't
                    // process their format into the final document efficiently (import Excel spreadsheet
                    // data into a Word document).
                    //
                    //if (item.isTable)
                    //{
                    //    if (item.useExternalDocument)
                    //    {
                    //        if (string.IsNullOrEmpty(item.externalDocumentName))
                    //        {
                    //            // This is an error - the external document option is selected, but the
                    //            // external document itself was never supplied.

                    //            // TODO: implement proper diagnostics
                    //        }
                    //        else
                    //        {
                    //            // Load the external document and insert its contents at the specified position in the output document.
                    //            string altChunkId = "Z" + Guid.NewGuid().ToString("N");

                    //            AlternativeFormatImportPart chunk = mainPart
                    //                .AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML, altChunkId);

                    //            using (FileStream stream = new FileStream(item.externalDocumentUniqueFileName, FileMode.Open, FileAccess.Read))
                    //            {
                    //                chunk.FeedData(stream);
                    //            }

                    //            // Wrap the inserted document into a separate section
                    //            if (!SectionPropertiesExists(mainPart, punkts) && primarySectionProperties != null)
                    //            {
                    //                punkts.Add(new Paragraph(new ParagraphProperties(primarySectionProperties.CloneNode(true))));
                    //            }

                    //            var altChunk = new AltChunk { Id = altChunkId };
                    //            MatchSource matchSrc = new MatchSource();
                    //            matchSrc.Val = OnOffValue.FromBoolean(true);
                    //            AltChunkProperties altProp = new AltChunkProperties(matchSrc);
                    //            altChunk.AltChunkProperties = altProp;
                    //            punkts.Add(altChunk);

                    //            SectionProperties sp = FindDocumentSectionElement(item.externalDocumentUniqueFileName);

                    //            if (sp != null)
                    //                punkts.Add(new Paragraph(new ParagraphProperties(sp)));
                    //        }
                    //    }
                    //    else
                    //    {
                    //        OpenXmlElement table = GenerateWordTable(item);

                    //        if (table != null)
                    //        {
                    //            punkts.Add(table);

                    //            // Add an empty line after table (just for decoration)
                    //            punkts.Add(new Paragraph(new Run(new Text(""))));
                    //        }
                    //    }
                    //}

                    // Table sub-items are, in fact, table rows and therefore don't get
                    // the GeneratePunktsRecursive treatment.
                    if (!item.isTable)
                    {
                        // Generate sub-items
                        GeneratePunktsRecursive(mainPart, ds, item.ID, punkts, punktNumber, doc);
                    }
                }
            }
        }
    }

    public bool OnChangeHyperlinkToSpan(ref string openingTag, ref string innerText, ref string closingTag, object parameter)
    {
        // Check the value of 'rel' attribute
        string relValue = Utils.GetRelAttributeFromHyperlink(openingTag);

        if (relValue == "gukv_addr" ||
            relValue == "gukv_obj" ||
            relValue == "gukv_org_from" ||
            relValue == "gukv_org_to")
        {
            openingTag = openingTag.Replace("<a", "<span");
            closingTag = closingTag.Replace("</a", "</span");
        }

        return true;
    }
  
    protected bool InsertNumberOfArticleInNode(HtmlAgilityPack.HtmlNode root, string punktNumber)
    {
        bool isDone = false; 
        for (int i = 0; i < root.ChildNodes.Count; i++)
        {
            HtmlAgilityPack.HtmlNode node = root.ChildNodes[i];

            if (node.Name == "#text")
            {
                node.InnerHtml = punktNumber + " " + node.InnerHtml;
                isDone = true;
            }
            else
            {
                isDone = InsertNumberOfArticleInNode(node, punktNumber);
            }
            
            if (isDone) break;
        }
        return isDone;
    }

    protected void InsertNumberOfArticleInHtmlDoc(ref string html, string punktNumber)
    {
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

        doc.LoadHtml(html);

        HtmlAgilityPack.HtmlNode root = doc.DocumentNode;

        if (root != null)
        {
            InsertNumberOfArticleInNode(root, punktNumber);

            html = root.OuterHtml;
        }
    }

    protected void GenerateAppendix(RishProjectDataSource ds, MainDocumentPart mainPart,
        RishProjectAppendixItem appendixItem, List<OpenXmlElement> appendixElements, WordprocessingDocument doc)
    {
        // Add a page break to the document
        //mainPart.Document.Body.AppendChild(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));

        if (appendixItem.useExternalDocument)
        {
            if (string.IsNullOrEmpty(appendixItem.externalDocumentName))
            {
                // This is an error - the external document option is selected, but the
                // external document itself was never supplied.

                // TODO: implement proper diagnostics
            }
            else
            {
                // Load the external document and insert its contents at the specified position in the output document.
                string altChunkId = "Z" + Guid.NewGuid().ToString("N");

                AlternativeFormatImportPart chunk = mainPart
                    .AddAlternativeFormatImportPart(AlternativeFormatImportPartType.WordprocessingML, altChunkId);

                using (FileStream stream = new FileStream(appendixItem.externalDocumentUniqueFileName, FileMode.Open, FileAccess.Read))
                {
                    chunk.FeedData(stream);
                }

                MatchSource matchSrc = new MatchSource();
                matchSrc.Val = OnOffValue.FromBoolean(true);
                AltChunkProperties altProp = new AltChunkProperties(matchSrc);

                var altChunk = new AltChunk { Id = altChunkId, AltChunkProperties = altProp };
                
                // Wrap the inserted document into a separate section
                if (!SectionPropertiesExists(mainPart, null) && primarySectionProperties != null)
                {
                    mainPart.Document.Body.AppendChild(new Paragraph(new ParagraphProperties(primarySectionProperties.CloneNode(true))));
                }

                mainPart.Document.Body.AppendChild(altChunk);

                SectionProperties sp = FindDocumentSectionElement(appendixItem.externalDocumentUniqueFileName);

                if (sp != null)
                    mainPart.Document.Body.AppendChild(new Paragraph(new ParagraphProperties(sp)));

                lastAppIsExternal = true;
            }
        }
        else
        {
            if (!lastAppIsExternal)
                // Add a page break to the document
                mainPart.Document.Body.AppendChild(new Paragraph(new Run(new  Break() { Type = BreakValues.Page })));

            // Add all elements of the appendix, processing the tags
            foreach (OpenXmlElement element in appendixElements)
            {
                OpenXmlElement copy = element.CloneNode(true);

                mainPart.Document.Body.AppendChild(copy);

                ProcessAppendixTags(mainPart, copy, appendixItem);

                if (copy is Paragraph)
                {
                    // If this is an {APPENDIXITEMS} tag, generate appendix items now
                    string paragraphText = GetParagraphText(copy as Paragraph);

                    if (paragraphText.Contains(tagAppendixItems))
                    {
                        // Generate the appendix items
                        GeneratePunkts(ds, mainPart, copy as Paragraph, appendixItem.ID, doc);

                        // Remove the unnecessary {APPENDIXITEMS} tag
                        copy.Remove();
                    }
                }
            }

            lastAppIsExternal = false;
        }
    }

    protected void ProcessAppendixTags(MainDocumentPart mainPart, OpenXmlElement element, RishProjectAppendixItem appendixItem)
    {
        if (element is Paragraph)
        {
            Paragraph para = element as Paragraph;

            var appendixTitle = appendixItem.name;
            var indexOfWhiteSpace = appendixTitle.IndexOf(' ');
            var appendixTitleFirstWord = "";
            if (indexOfWhiteSpace > 0)
                appendixTitleFirstWord = "<div style='text-align: center; font-weight: bold;'>" + appendixTitle.Substring(0, indexOfWhiteSpace).ToUpper() + "</div>";

            appendixTitle = appendixTitle.Substring(indexOfWhiteSpace+1);

            // Appendix title
            ReplaceDocTag(mainPart, para, tagAppendixTitle1, appendixTitleFirstWord, true);

            // Appendix title
            ReplaceDocTag(mainPart, para, tagAppendixTitle, appendixTitle, false);

            // Appendix intro
            ReplaceDocTag(mainPart, para, tagAppendixIntro, appendixItem.introText, true);

            // Appendix outro
            ReplaceDocTag(mainPart, para, tagAppendixOutro, appendixItem.outroText, true);
        }
    }

    protected int AddNumberingStyleInstance(MainDocumentPart mainPart)
    {
        NumberingDefinitionsPart numberingPart = mainPart.NumberingDefinitionsPart;

        if (numberingPart == null)
        {
            numberingPart = mainPart.AddNewPart<NumberingDefinitionsPart>("id1");
        }

        Numbering numbering = numberingPart.Numbering;

        if (numbering == null)
        {
            numbering = new Numbering();
        }

        // Get all abstract numbers
        List<AbstractNum> abstractNumbers = numbering.OfType<AbstractNum>().ToList();

        // Find maximum used Id of an abstract number
        int maxUsedAbstractNumberId = 0;

        foreach (AbstractNum absNum in abstractNumbers)
        {
            if (absNum.AbstractNumberId > maxUsedAbstractNumberId)
            {
                maxUsedAbstractNumberId = absNum.AbstractNumberId;
            }
        }

        // Get all numbering instances
        List<NumberingInstance> instances = numbering.OfType<NumberingInstance>().ToList();

        // Find maximum used Id of a numbering instance
        int maxUsedInstanceId = 0;

        foreach (NumberingInstance inst in instances)
        {
            if (inst.NumberID > maxUsedInstanceId)
            {
                maxUsedInstanceId = inst.NumberID;
            }
        }

        // Generate new IDs for the abstract number and its instance
        int newAbstractNumberId = maxUsedAbstractNumberId + 1;
        int newInstanceId = maxUsedInstanceId + 1;

        abstractNumbers.Add(
            new AbstractNum(
                new MultiLevelType() { Val = MultiLevelValues.Multilevel },

                new Level(
                    new NumberingFormat() { Val = NumberFormatValues.Decimal },
                    new LevelText() { Val = "%1." },
                    new LevelJustification() { Val = LevelJustificationValues.Left },
                    new StartNumberingValue() { Val = 1 },

                    new ParagraphProperties(
                        new Indentation() { Left = "360", Hanging = "360" }
                    )
                ) { LevelIndex = 0 },

                new Level(
                    new NumberingFormat() { Val = NumberFormatValues.Decimal },
                    new LevelText() { Val = "%1.%2." },
                    new LevelJustification() { Val = LevelJustificationValues.Left },
                    new StartNumberingValue() { Val = 1 },
                    new IsLegalNumberingStyle(),

                    new ParagraphProperties(
                        new Indentation() { Left = "792", Hanging = "432" }
                    )
                ) { LevelIndex = 1 },

                new Level(
                    new NumberingFormat() { Val = NumberFormatValues.Decimal },
                    new LevelText() { Val = "%1.%2.%3." },
                    new LevelJustification() { Val = LevelJustificationValues.Left },
                    new StartNumberingValue() { Val = 1 },
                    new IsLegalNumberingStyle(),

                    new ParagraphProperties(
                        new Indentation() { Left = "1224", Hanging = "504" }
                    )
                ) { LevelIndex = 2 },

                new Level(
                    new NumberingFormat() { Val = NumberFormatValues.Decimal },
                    new LevelText() { Val = "%1.%2.%3.%4." },
                    new LevelJustification() { Val = LevelJustificationValues.Left },
                    new StartNumberingValue() { Val = 1 },
                    new IsLegalNumberingStyle(),

                    new ParagraphProperties(
                        new Indentation() { Left = "1728", Hanging = "648" }
                    )
                ) { LevelIndex = 3 }

            ) { AbstractNumberId = newAbstractNumberId }
        );

        instances.Add(
            new NumberingInstance(
                new AbstractNumId() { Val = newAbstractNumberId }
            ) { NumberID = newInstanceId }
        );

        // Recreate the Numbering node
        numbering.RemoveAllChildren();

        foreach (AbstractNum absNum in abstractNumbers)
        {
            numbering.AppendChild(absNum);
        }

        foreach (NumberingInstance inst in instances)
        {
            numbering.AppendChild(inst);
        }

        numbering.Save(numberingPart);

        return newInstanceId;
    }

    protected IList<OpenXmlCompositeElement> ConvertHtmlToOpenXml(MainDocumentPart mainPart, string htmlText)
    {
        HtmlConverter converter = new HtmlConverter(mainPart);
        Body body = mainPart.Document.Body;
        var paragraphs = converter.Parse(htmlText);
        /*for (int i = 0; i < paragraphs.Count; i++)
        {
            body.Append(paragraphs[i]);
        }*/
        return paragraphs;
    }

    protected string AddHtmlToAltChunk(MainDocumentPart mainPart, string htmlText)
    {
        // Format the html to form a full document
        htmlText = "<html><head><meta charset=\"utf-8\"></head><body>" + htmlText + "</body>";

        // Generate a unique ID for the chunk
        altChunkId++;

        string chunkId = "AltChunkId" + altChunkId.ToString();

        // Create an AltChunk part in the document
        AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, chunkId);

        using (System.IO.Stream chunkStream = chunk.GetStream(System.IO.FileMode.Create, System.IO.FileAccess.Write))
        {
            using (System.IO.StreamWriter stringStream = new System.IO.StreamWriter(chunkStream))
            {
                stringStream.Write(htmlText);
            }
        }

        return chunkId;
    }

    protected OpenXmlElement GenerateWordTable(RishProjectDataSourceItem tableItem)
    {
        return null;

        //List<RishProjectTableColumn> columns = new List<RishProjectTableColumn>();
        //List<RishProjectDataSourceItem> rows = new List<RishProjectDataSourceItem>();

        //tableItem.DataSource.GetTableColumns(tableItem.ID, columns);
        //tableItem.DataSource.GetTableRows(tableItem.ID, rows);

        //Dictionary<int, Dictionary<int, RishProjectTableValue>> valueCache = tableItem.DataSource.GetTableValues(tableItem.ID);

        //DocumentFormat.OpenXml.Wordprocessing.Table table = null;

        //if (columns.Count > 0)
        //{
        //    table = new DocumentFormat.OpenXml.Wordprocessing.Table();

        //    // Create a TableProperties object and specify its border information
        //    TableProperties tblProp = new TableProperties(
        //        new TableBorders(
        //            new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
        //            new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
        //            new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
        //            new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
        //            new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
        //            new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
        //        ),
        //        new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto },
        //        new TableCellMarginDefault(
        //            new TableCellLeftMargin() { Width = 57, Type = TableWidthValues.Dxa },
        //            new TableCellRightMargin() { Width = 57, Type = TableWidthValues.Dxa })
        //    );

        //    // Append the TableProperties object to the empty table.
        //    table.AppendChild<TableProperties>(tblProp);

        //    // Add the column titles
        //    DocumentFormat.OpenXml.Wordprocessing.TableRow trTitles = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

        //    foreach (RishProjectTableColumn col in columns)
        //    {
        //        DocumentFormat.OpenXml.Wordprocessing.TableCell cellColumnTitle = new DocumentFormat.OpenXml.Wordprocessing.TableCell();

        //        cellColumnTitle.Append(new Paragraph(
        //            new ParagraphProperties(new Justification() { Val = JustificationValues.Center } , new ParagraphStyleId() { Val = StyleIdBold }),
        //            new Run(new Text(col.columnName))));

        //        trTitles.Append(cellColumnTitle);
        //    }

        //    table.Append(trTitles);

        //    // Add each table item as a table row
        //    foreach (RishProjectDataSourceItem row in rows)
        //    {
        //        DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

        //        // Add cells for each column in the appendix
        //        foreach (RishProjectTableColumn col in columns)
        //        {
        //            string value = row.DataSource.GetTableCellValue(col, row, valueCache);

        //            DocumentFormat.OpenXml.Wordprocessing.TableCell cellValue = new DocumentFormat.OpenXml.Wordprocessing.TableCell();

        //            cellValue.Append(new Paragraph(
        //                new ParagraphProperties(new Justification() { Val = JustificationValues.Left }, new ParagraphStyleId() { Val = StyleIdTableCell }), 
        //                new Run(new Text(value))));

        //            tr.Append(cellValue);
        //        }

        //        // Append the table row to the table.
        //        table.Append(tr);
        //    }
        //}

        //return table;
    }

    protected void AddDocumentStyles(MainDocumentPart mainPart)
    {
        // Add the text styles
        AddDocumentStyle(mainPart, StyleIdTableCell, "Table Cell", false, 0, 0);
        AddDocumentStyle(mainPart, StyleIdBold, "Bold Text", true, 0, 0);
    }

    private void AddDocumentStyle(MainDocumentPart mainPart, string styleId, string styleName,
        bool isBold, int indentLeft, int indentFirstLine)
    {
        StyleDefinitionsPart styleDefinitionsPart = mainPart.StyleDefinitionsPart;

        if (styleDefinitionsPart == null)
        {
            styleDefinitionsPart = mainPart.AddNewPart<StyleDefinitionsPart>();

            Styles root = new Styles();

            root.Save(styleDefinitionsPart);
        }

        Styles styles = styleDefinitionsPart.Styles;

        if (styles != null)
        {
            // Create a style definitions
            DocumentFormat.OpenXml.Wordprocessing.Style style = new DocumentFormat.OpenXml.Wordprocessing.Style()
            {
                Type = StyleValues.Paragraph,
                StyleId = styleId,
                CustomStyle = true
            };

            style.Append(new BasedOn() { Val = "Normal" });
            style.Append(new StyleName() { Val = styleName });
            style.Append(new NextParagraphStyle() { Val = "Normal" });

            if (isBold)
            {
                StyleRunProperties styleRunProperties = new StyleRunProperties();

                if (isBold)
                {
                    styleRunProperties.Append(new Bold());
                }

                style.Append(styleRunProperties);
            }

            if (indentLeft >= 0 || indentFirstLine >= 0)
            {
                StyleParagraphProperties styleParaProperties = new StyleParagraphProperties();

                if (indentLeft >= 0 || indentFirstLine >= 0)
                {
                    Indentation indentation = new Indentation();

                    if (indentLeft >= 0)
                        indentation.Left = indentLeft.ToString();

                    if (indentFirstLine >= 0)
                        indentation.FirstLine = indentFirstLine.ToString();

                    styleParaProperties.Append(indentation);
                }

                style.Append(styleParaProperties);
            }

            styles.Append(style);
        }
    }

    private SectionProperties FindPrimarySectionElement(MainDocumentPart mainPart)
    {
        List<SectionProperties> sections = mainPart.Document.Body.OfType<SectionProperties>().ToList();

        return sections.Count > 0 ? sections[sections.Count - 1] : null;
    }

    private SectionProperties FindDocumentSectionElement(string docFileName)
    {
        SectionProperties sectionProperties = null;

        docFileName = docFileName.Trim().ToLower();

        if (docFileName.EndsWith(".docx"))
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(docFileName, true))
            {
                MainDocumentPart mainPart = wordDocument.MainDocumentPart;

                if (mainPart != null)
                {
                    SectionProperties sp = FindPrimarySectionElement(mainPart);

                    sectionProperties = sp.CloneNode(true) as SectionProperties;
                }

                wordDocument.Close();
            }
        }

        // Remove any references to entities from the other document
        List<OpenXmlElement> elementsToRemove = new List<OpenXmlElement>();

        foreach (OpenXmlElement element in sectionProperties.ChildElements)
        {
            string name = element.LocalName;

            if (name != "pgSz" && name != "pgMar" && name != "cols" && name != "docGrid")
            {
                elementsToRemove.Add(element);
            }
        }

        foreach (OpenXmlElement element in elementsToRemove)
        {
            element.Remove();
        }

        return sectionProperties;
    }

    private bool SectionPropertiesExists(MainDocumentPart mainPart, List<OpenXmlElement> elements)
    {
        if (elements == null)
        {
            // Take all elements of the document
            elements = mainPart.Document.Body.OfType<OpenXmlElement>().ToList();
        }

        if (elements != null && elements.Count > 0)
        {
            OpenXmlElement lastElement = elements[elements.Count - 1];

            if (lastElement is SectionProperties)
            {
                return true;
            }
            else if (lastElement is Paragraph)
            {
                ParagraphProperties pPr = lastElement.ChildElements.First<ParagraphProperties>();

                if (pPr != null)
                {
                    SectionProperties sp = pPr.ChildElements.First<SectionProperties>();

                    return sp != null;
                }
            }
        }

        return false;
    }

    private void AddLastSectionProperties(MainDocumentPart mainPart)
    {
        // First check if there is already some SectionProperties at the end of the document
        List<OpenXmlElement> elements = mainPart.Document.Body.OfType<OpenXmlElement>().ToList();

        if (elements != null && elements.Count > 0)
        {
            OpenXmlElement lastElement = elements[elements.Count - 1];

            if (lastElement is SectionProperties)
            {
                // Everything is OK; last element of the document is SectionProperties
            }
            else if (lastElement is Paragraph)
            {
                ParagraphProperties pPr = lastElement.ChildElements.First<ParagraphProperties>();

                if (pPr != null)
                {
                    SectionProperties sp = pPr.ChildElements.First<SectionProperties>();

                    if (sp != null)
                    {
                        // Extract this SectionProperties from the paragraph, and put it on top level
                        sp.Remove();
                        lastElement.Remove();

                        mainPart.Document.Body.AppendChild(sp);
                        return;
                    }
                }
            }
        }

        // We need to add a new SectionProperties at the end of file
        if (primarySectionProperties != null)
            mainPart.Document.Body.AppendChild(primarySectionProperties);
    }

    #endregion (Word report generation)

    #region Export to the 'Rozporadjennia' DB

    protected void CPFormButtons_Callback(object sender, CallbackEventArgsBase e)
    {
        //if (e.Parameter.StartsWith("export:"))
        //{
        //    // Get the main document node
        //    RishProjectDataSource ds = ProjectDataSource;
        //    RishProjectMainDocItem mainDoc = null;

        //    foreach (IRishProjectNode node in ds)
        //    {
        //        if (node is RishProjectMainDocItem)
        //        {
        //            mainDoc = node as RishProjectMainDocItem;
        //            break;
        //        }
        //    }

        //    // Check the document status before exporting it
        //    if (mainDoc != null && !mainDoc.isExportedToNJF && mainDoc.stateId == 16)
        //    {
        //        //FbConnection connectionNJF = Utils.ConnectToNJF();

        //        //if (connectionNJF != null)
        //        //{
        //        //    string errorMessage = "";

        //        //    RishProjectExport.ExportDocument(connectionNJF, ds, out errorMessage);

        //        //    connectionNJF.Close();

        //        //    LabelNJFExportError.Text = errorMessage;
        //        //    LabelNJFExportError.ClientVisible = (errorMessage.Length > 0);

        //        //    // If everything went OK, no second export should be allowed
        //        //    if (errorMessage.Length == 0)
        //        //    {
        //        //        mainDoc.isExportedToNJF = true;
        //        //        ButtonExportToNJF.ClientEnabled = false;

        //        //        ds.SaveToDatabase();
        //        //    }
        //        //}
        //    }
        //}
    }

    #endregion (Export to the 'Rozporadjennia' DB)

    #region Session management

    protected string GetPageUniqueKey()
    {
        object key = ViewState["PageUniqueKey"];

        if (key is string)
        {
            return (string)key;
        }

        // Generate unique key
        Guid guid = Guid.NewGuid();

        string str = guid.ToString();

        ViewState["PageUniqueKey"] = str;

        return str;
    }

    protected RishProjectDataSource ProjectDataSource
    {
        get
        {
            RishProjectDataSource ds = Session[GetPageUniqueKey() + "_ProjectDataSource"] as RishProjectDataSource;

            if (ds == null)
            {
                ds = new RishProjectDataSource();
                ds.LoadFromDatabase(ProjectID);
                Session[GetPageUniqueKey() + "_ProjectDataSource"] = ds;
            }

            return ds;
        }
    }

    protected int ProjectID
    {
        set
        {
            Session[GetPageUniqueKey() + "_ProjectID"] = value;
        }

        get
        {
            object val = Session[GetPageUniqueKey() + "_ProjectID"];

            if (val is int)
            {
                return (int)val;
            }

            return -1;
        }
    }

    public IRishProjectNode EditedTreeItem
    {
        set
        {
            Page.SetEditedTreeItem(value);

            if (value is RishProjectMainDocItem)
            {
                TreeListDoc.SettingsPopupEditForm.Caption = "Параметри розпорядчого документу";
            }
            else if (value is RishProjectDataSourceItem)
            {
                if ((value as RishProjectDataSourceItem).isTable)
                {
                    TreeListDoc.SettingsPopupEditForm.Caption = "Параметри додатку до пункту розпорядчого документу";
                }
                else
                {
                    TreeListDoc.SettingsPopupEditForm.Caption = "Параметри пункту розпорядчого документу";
                }
            }
        }

        get
        {
            return Page.GetEditedTreeItem<IRishProjectNode>();
        }
    }

    #endregion (Session management)

    private class ComboProjectStateCallback_CallbackParams
    {
        public int[] AddedValues { get; set; }
        public int[] RemovedValues { get; set; }
    }

    private class ComboProjectStateCallback_CallbackResult
    {
        /// <summary>
        /// Signals that the grid must be refreshed. This is the general "something happened"
        /// status, and bears no indication whether the grid needs to be shown or hidden.
        /// </summary>
        public bool RefreshCoverLetterGrid { get; set; }

        /// <summary>
        /// Signals whether the grid needs to be shown or hidden. If RefreshCoverLetterGrid is
        /// set to true, then this property is evaluated to determine if the grid also needs to
        /// be shown or hidden.
        /// </summary>
        public bool ShowCoverLetterGrid { get; set; }

        //public object Diags { get; set; }
    }

    protected void ComboProjectStateCallback_Callback(object source, CallbackEventArgs e)
    {
        // By the time control reaches this callback function, the from has already had
        // the chance to process the new State ID values, and update the model. Which is
        // great, as it spares us from lots of custom model updates here.
        var result = new ComboProjectStateCallback_CallbackResult();

        ComboProjectStateCallback_CallbackParams p = e.Parameter.FromJSON<ComboProjectStateCallback_CallbackParams>();
        if (p != null && ((p.AddedValues != null && p.AddedValues.Length > 0) 
            || (p.RemovedValues != null && p.RemovedValues.Length > 0)))
        {

            int[] affectedStates = (p.AddedValues ?? new int[0])
                .Concat(p.RemovedValues ?? new int[0])
                .ToArray();

            int[] coverLetterStateIDs = WorkflowStates
                .Where(x => ((RishProjectState)x.flags & RishProjectState.RequresCoverLetter) == RishProjectState.RequresCoverLetter)
                .Select(x => x.id)
                .ToArray();

            int[] finalStateIDs = WorkflowStates
                .Where(x => ((RishProjectState)x.flags & RishProjectState.Final) == RishProjectState.Final)
                .Select(x => x.id)
                .ToArray();

            // RefreshCoverLetterGrid signal is raised whenever *any* operation is performed with a
            // state that requires a cover letter. Whether the grid needs to be also shown or hidden
            // is decided by the result of the current state selection operation later in this routine.
            result.RefreshCoverLetterGrid = coverLetterStateIDs.Any(affectedStates.Contains) || finalStateIDs.Any(affectedStates.Contains);

            RishProjectDataSource ds = ProjectDataSource;
            RishProjectMainDocItem doc = (RishProjectMainDocItem)ds.First();

            if (doc.Row.Getbp_rish_project_stateRows()
                .Where(x => x.RowState != DataRowState.Deleted && x.RowState != DataRowState.Detached)
                .Where(x => coverLetterStateIDs.Contains(x.state_id))
                .Where(x => x.Isexited_onNull())
                //.Where(x => x.Iscover_letter_noNull() || x.Iscover_letter_dateNull())
                .Any())
            {
                // ShowCoverLetterGrid signal is raised whenever the result of the current state
                // selection operation has at least one current state that requires the cover letter.
                // Whether the cover letter information is filled out is irrelevant at this point,
                // we simply must display the grid.
                result.ShowCoverLetterGrid = true;
            }

            //result.Diags = new
            //{
            //    ComboProjectState_Value = ComboProjectState.Value,
            //    ComboProjectState_ASPxListBox1_SelectedValues = ComboProjectState.ASPxListBox1.SelectedValues,
            //    doc_stateIds = doc.stateIds,
            //    doc_stateRows = doc.Row.Getbp_rish_project_stateRows().Select(x => new { x.id, x.state_id, x.entered_on, x.entered_by, x.RowState }),
            //};

        }

        e.Result = result.ToJSON();
    }

    protected void CoverLettersGrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        RishProjectMainDocItem doc = (RishProjectMainDocItem)ProjectDataSource.First();

        List<RishProjectMainDocItem.CoverLettersGridItem> items = doc.CoverLettersGridDataSource.ToList();
        RishProjectMainDocItem.CoverLettersGridItem item = items.FirstOrDefault(
            x => x.id == Convert.ToInt32(e.Keys[0])
            );
        if (item != null)
        {
            item.cover_letter_no = e.NewValues["cover_letter_no"] as string;
            item.cover_letter_date = e.NewValues["cover_letter_date"] is DateTime ? (DateTime?)e.NewValues["cover_letter_date"] : null;
        }

        CoverLettersGrid.JSProperties["cpMissingRequiredData"] =
            doc.CoverLettersGridDataSource.Any(x => x.cover_letter_date == null || x.cover_letter_no == null);

        ((ASPxGridView)sender).CancelEdit();
        e.Cancel = true;
    }

    protected void CoverLettersGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "cover_letter_no" || e.DataColumn.FieldName == "cover_letter_date")
        {
            if (e.CellValue == null)
            {
                e.Cell.ForeColor = System.Drawing.Color.FromArgb(255, 190, 190);
                e.Cell.Font.Italic = true;
                e.Cell.Text = "обов'язкове поле";
            }
        }
    }

    private class CallbackCheckDocumentIdentity_CallbackParams
    {
        public string TextBoxRishNum { get; set; }
        public DateTime DateEditRishDate { get; set; }
    }

    protected void CallbackCheckDocumentIdentity_Callback(object source, CallbackEventArgs e)
    {
        CallbackCheckDocumentIdentity_CallbackParams p =
            e.Parameter.FromJSON<CallbackCheckDocumentIdentity_CallbackParams>();


        bool alreadyExists = false;

        if (p != null
            && !string.IsNullOrEmpty(p.TextBoxRishNum)
            && p.DateEditRishDate != default(DateTime))
        {
            RishProjectDataSource ds = ProjectDataSource;
            if ((new DBTableAdapters.documentsTableAdapter()).GetDataByDocumentIdentity(
                ds.GetMainDoc().GetRozpDocumentTypeID(), p.TextBoxRishNum.Trim(), p.DateEditRishDate.Date).Any())
            {
                alreadyExists = true;
            }
        }

        e.Result = (new { AlreadyExists = alreadyExists }).ToJSON();
    }
}
