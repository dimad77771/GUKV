using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxTreeList;
using System.Data.SqlClient;
using DevExpress.Web;
using System.IO;
using System.Data;
using DevExpress.Web.Data;
using log4net;
using Excel;

public partial class RishProjectTableEditor : System.Web.UI.UserControl
{

    private static readonly ILog log = LogManager.GetLogger(typeof(RishProjectTableEditor));

    protected void Page_Load(object sender, EventArgs e)
    {
        //GetPageUniqueKey();

        RishProjectDataSourceItem item = Page.GetEditedTreeItem<RishProjectDataSourceItem>();
        if (item == null)
            return;

        HtmlEditorCssFile cssFile = new HtmlEditorCssFile("~/CSS/ASPxHtmlEditor.css");
        MemoPunktOutro.CssFiles.Add(cssFile);

        if (!IsInitialized)
        {
            // Initialize the external document choices and settings
            if (!string.IsNullOrEmpty(item.externalDocumentName))
            {
                LinkFileName.Text = item.externalDocumentName;
                LinkFileName.NavigateUrl = ResolveClientUrl(
                    string.Format("~/Documents/ExternalDocument.aspx?id={0}&name={1}",
                        Path.GetFileName(item.externalDocumentUniqueFileName), item.externalDocumentName
                        )
                    );
            }

            ComboObjectType.Value = item.tableType ?? 0;
            EditAppendixName.Text = item.introText;
            MemoPunktOutro.Html = item.outroText.Length > 0 ? item.outroText : "<p style=\"font-family: Times New Roman; font-size: 14pt;\">&nbsp;</p>"; ;

            IsInitialized = true;
        }

        Utils.HideUnnecessaryHtmlEditorButtons(MemoPunktOutro, true);

        RebindTableGrid();

        if (ComboObjectType.SelectedItem != null)
        {
            UpdateTableGridLayout((int)ComboObjectType.SelectedItem.Value);
            ProcessCreateAct();
        }
    }

    private void RebindTableGrid(bool force = false)
    {
        if (GridViewTable.DataSource == null || force)
        {
            GridViewTable.DataSource = GetGridDataSource();
            GridViewTable.DataBind();
        }
    }

    private void UpdateTableGridLayout(int objectType)
    {
        if (GridViewTable.DataSource != null)
        {
            switch (objectType)
            {
                case 0: // Об’єкти нерухомості нежитлового фонду
                    GridViewTable.Columns["addr_street_name"].Visible = true;
                    GridViewTable.Columns["addr_nomer"].Visible = true;
                    GridViewTable.Columns["addr_misc"].Visible = true;
                    GridViewTable.Columns["addr_distr"].Visible = true;
                    GridViewTable.Columns["year_built"].Visible = true;
                    GridViewTable.Columns["sqr_total"].Visible = true;
                    GridViewTable.Columns["obj_kind"].Visible = true;
                    GridViewTable.Columns["obj_type"].Visible = true;
                    
                    GridViewTable.Columns["address"].Visible = false;
                    GridViewTable.Columns["location"].Visible = false;
                    GridViewTable.Columns["commissioned_date"].Visible = false;
                    break;
                case 1: // Інші об’єкти комунальної власності
                    GridViewTable.Columns["addr_street_name"].Visible = false;
                    GridViewTable.Columns["addr_nomer"].Visible = false;
                    GridViewTable.Columns["addr_misc"].Visible = false;
                    GridViewTable.Columns["addr_distr"].Visible = false;
                    GridViewTable.Columns["year_built"].Visible = false;
                    GridViewTable.Columns["sqr_total"].Visible = false;
                    GridViewTable.Columns["obj_kind"].Visible = false;
                    GridViewTable.Columns["obj_type"].Visible = false;

                    GridViewTable.Columns["address"].Visible = true;
                    GridViewTable.Columns["location"].Visible = true;
                    GridViewTable.Columns["commissioned_date"].Visible = true;
                    break;
            }
        }
    }
 
    private DataTable GetGridDataSource()
    {
        DataSet editorDatasource = GetEditorDataSource();
        if (editorDatasource == null)
            return null;
        return editorDatasource.Tables["rows"];
    }

    private DataSet GetEditorDataSource()
    {
        DataSet datasource = Page.GetEditedCustomData<DataSet>("RishProjectTableEditor_DataSource");
        if (datasource == null)
        {
            RishProjectDataSourceItem item = Page.GetEditedTreeItem<RishProjectDataSourceItem>();
            if (item == null)
                return null;

            datasource = item.DataSource.CreateTableDataSource(item.ID);

            Page.SetEditedCustomData("RishProjectTableEditor_DataSource", datasource);
         }

        datasource.EnforceConstraints = false;
        return datasource;
     }

    public void SaveChanges()
    {
        RishProjectDataSourceItem item = Page.GetEditedTreeItem<RishProjectDataSourceItem>();
        if (item == null)
            return;

        item.tableType = Convert.ToInt32(ComboObjectType.Value);
        item.introText = EditAppendixName.Text;
        item.outroText = MemoPunktOutro.Html;

        item.DataSource.CommitTableDataSource(item.ID, GetEditorDataSource());

        // Only when the temporary file name is initialized we can be certain
        // that a new file was uploaded. Only then we should touch the external_document
        // reference of the main document (which would cause .Modified status on the
        // affected rows).
        if (!string.IsNullOrEmpty(TempFileName.Value))
        {
            item.externalDocumentName = OrigFileName.Value;
            item.externalDocumentUniqueFileName =
                Path.Combine(ExternalDocument.GetExternalDocumentStorePath(true), TempFileName.Value);
        }
    }

    private bool IsInitialized
    {
        get { return Page.GetEditorFormInitialized(); }
        set { Page.SetEditorFormInitialized(value); }
    }

    public IRishProjectNode EditedTableItem
    {
        set
        {
            Session["EditedTableItem"] = value;
            Session["RISH_PROJECT_TABLE_ROW_EDIT_FORM_INIT_DONE"] = false;
        }

        get
        {
            object val = Session["EditedTableItem"];

            if (val is IRishProjectNode)
            {
                return val as IRishProjectNode;
            }

            return null;
        }
    }

    protected void UploadFile_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        string attachmentFileName = ExternalDocument.Copy(e.UploadedFile.FileContent, e.UploadedFile.FileName, true);

        try
        {
            ParseDocument(attachmentFileName);
            ProcessCreateAct();

        }
        catch (Exception ex)
        {
            e.CallbackData =
                (new
                {
                    ImportError = ex.Message,
                }).ToJSON();
            
            try { File.Delete(attachmentFileName); }
            catch { }
            
            return;
        }

        e.CallbackData =
            (new
            {
                OriginalFileName = Path.GetFileName(e.UploadedFile.FileName),

                // Passing the full path back to the client side is not such a good idea. Upon save,
                // we can always assume that the file was uploaded into the temporary storage and
                // rebuild its path correspondingly.
                TempFileName = Path.GetFileName(attachmentFileName),

                ViewDocumentUrl = ResolveClientUrl(
                    string.Format("~/Documents/ExternalDocument.aspx?id={0}&name={1}",
                        Path.GetFileName(attachmentFileName), Path.GetFileName(e.UploadedFile.FileName))
                    ),
            }).ToJSON();
    }

    private void ParseDocument(string fileName)
    {
        DataSet importData;

        using (Stream stream = File.OpenRead(fileName))
        {
            IExcelDataReader reader;
            if (Path.GetExtension(fileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            using (reader)
            {
                reader.IsFirstRowAsColumnNames = true;
                importData = reader.AsDataSet();
            }
        }

        DataTable table = GetGridDataSource();
        table.Rows.Clear();
        //foreach (DataRow row in table.Rows)
            //row.Delete();

        if (importData.Tables.Count == 0
            || !importData.Tables.Cast<DataTable>().Any(x => x.Rows.Count > 0))
        {
            throw new InvalidOperationException("Завантажений файл здається порожнім");
        }

        // This tricks ComboObjectType into setting up its .Value property in accordance
        // with the current selection; otherwise, the property remains NULL.
        ((IPostBackDataHandler)ComboObjectType).LoadPostData(ComboObjectType.UniqueID, Request.Params);
        bool processUnmappedColumns = (Convert.ToInt32(ComboObjectType.Value) == 1);

        foreach (DataTable importTable in importData.Tables)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            for (int columnIndex = 0; columnIndex < importTable.Columns.Count; columnIndex++)
            {
                string inputFieldName = importTable.Columns[columnIndex].ColumnName.Trim();
                string tableFieldName = EnumerateAllColumns<GridViewDataColumn>(GridViewTable.Columns)
                    .Where(x => x.Visible)
                    .Where(x => inputFieldName.Equals(x.Caption, StringComparison.OrdinalIgnoreCase)
                        || inputFieldName.Equals(FullCaption(x), StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.FieldName)
                    .FirstOrDefault();
                if (tableFieldName != null && !mapping.ContainsKey(inputFieldName))
                    mapping.Add(inputFieldName, tableFieldName);
            }
            if (mapping.Count == 0)
                continue;

            foreach(DataRow importRow in importTable.Rows)
            {
                DataRow row = table.NewRow();
                bool hasData = false;
                foreach (KeyValuePair<string, string> entry in mapping)
                {
                    if (!importRow.IsNull(entry.Key))
                    {
                        row[entry.Value] = importRow[entry.Key];
                        if (!hasData && Convert.ToString(importRow[entry.Key]).Trim().Length > 0)
                            hasData = true;
                    }
                }
                if (processUnmappedColumns && importTable.Columns.Count > mapping.Count)
                {
                    foreach (DataColumn importColumn in importTable.Columns.Cast<DataColumn>()
                        .Where(x => !mapping.ContainsKey(x.ColumnName)))
                    {
                        string importValue = Convert.ToString(importRow[importColumn]).Trim();
                        if (importValue.Length == 0)
                            continue;

                        hasData = true;

                        string tableValue = (row["name"] as string ?? string.Empty);
                        if (tableValue.Length > 0)
                            tableValue += "; ";
                        row["name"] = tableValue + importValue;
                    }
                }
                if (hasData)
                    table.Rows.Add(row);
            }
        }

        if (table.Rows.Count == 0)
            throw new InvalidOperationException("Немає очікуваних колонок");
    }

    private static string FullCaption(GridViewColumn column)
    {
        if (column.ParentBand == null)
            return null;
        return column.ParentBand.Caption + " " + column.Caption;
    }

    private IEnumerable<T> EnumerateAllColumns<T>(GridViewColumnCollection columns)
        where T: GridViewColumn
    {
        foreach (GridViewColumn column in columns)
        {
            if (column is GridViewBandColumn)
            {
                foreach (T subColumn in EnumerateAllColumns<T>(((GridViewBandColumn)column).Columns))
                    yield return subColumn;
            }
            else
            {
                if (column is T)
                    yield return (T)column;
            }
        }
    }

    private class GridViewTable_CustomCallbackParams
    {
        public int ObjectType { get; set; }
    }

    protected void GridViewTable_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        GridViewTable_CustomCallbackParams p = e.Parameters.FromJSON<GridViewTable_CustomCallbackParams>();
        if (p != null)
        {
            UpdateTableGridLayout(p.ObjectType);
            ProcessCreateAct();
        }
    }

    public string PageUniqueKey { get; set; }

    protected string GetPageUniqueKey()
    {
        return PageUniqueKey;

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

    public int ProjectID
    {

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

    protected List<DataError> Errors
    {
        get
        {
            List<DataError> value = Session[GetPageUniqueKey() + "_Errors"] as List<DataError>;
            if (value == null)
                Session[GetPageUniqueKey() + "_Errors"] = (value = new List<DataError>());
            return value;
        }
        set
        {
            Session[GetPageUniqueKey() + "_Errors"] = value;
        }
    }

    protected void GridViewTable_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        DataError error = Errors.FirstOrDefault(x => x.RowID == (int)e.KeyValue);
        if (error == null)
            return;

        e.Cell.ToolTip = error.What;

        if (error.FieldName == null || error.FieldName == e.DataColumn.FieldName)
        {
            e.Cell.BackColor = System.Drawing.Color.Red;
            e.Cell.ForeColor = System.Drawing.Color.White;
        }
        else if (error.FieldName != null)
        {
            e.Cell.BackColor = System.Drawing.Color.LightYellow;
        }
    }

    private bool ProcessCreateAct()
    {
        string actNumber = string.Empty;
        DateTime actDate = DateTime.MinValue;

        int[] selectedTableRowIDs = new int[0]; 
        int rightID = 1;// ConvertTo<int>(ComboBoxAppendix.SelectedItem.GetValue("right_id"));
        int orgFromID = -1;// ConvertTo<int>(ComboBoxAppendix.SelectedItem.GetValue("org_from_id"));
        int orgToID = -1;// ConvertTo<int>(ComboBoxAppendix.SelectedItem.GetValue("org_to_id"));
        int objectType = ComboObjectType.SelectedItem == null ? -1 : (int)ComboObjectType.SelectedItem.Value;

        //return false;
        return RishProjectTableProcessor.ProcessCreateAct(Errors, ProjectID, Page.User, actNumber, actDate, GetEditorDataSource(), selectedTableRowIDs, rightID, orgFromID, orgToID, objectType, true);
    }


}
