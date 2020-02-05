using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.Data;
using DevExpress.XtraPrinting;
using System.IO;
using System.Text;
using System.Globalization;

public partial class Privatization_PrivatizationView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Build a simple dataset that holds objects which must be associated with the created document
        DataTable tableObjectsToAdd = (DataTable)Session["tableObjectsToAdd"];

        if (tableObjectsToAdd == null)
        {
            tableObjectsToAdd = new DataTable();

            tableObjectsToAdd.Columns.Add("balans_id", typeof(int));
            tableObjectsToAdd.Columns.Add("organization", typeof(string));
            tableObjectsToAdd.Columns.Add("district", typeof(string));
            tableObjectsToAdd.Columns.Add("street", typeof(string));
            tableObjectsToAdd.Columns.Add("number", typeof(string));
            tableObjectsToAdd.Columns.Add("object_group", typeof(string));
            tableObjectsToAdd.Columns.Add("sqr_balans", typeof(string));
            tableObjectsToAdd.Columns.Add("sqr_privat", typeof(string));
            tableObjectsToAdd.Columns.Add("floor", typeof(string));
            tableObjectsToAdd.Columns.Add("privat_kind", typeof(string));
            tableObjectsToAdd.Columns.Add("geo_lat", typeof(double));
            tableObjectsToAdd.Columns.Add("geo_lng", typeof(double));

            // Create a primary key in the table
            DataColumn[] keys = new DataColumn[1];
            keys[0] = tableObjectsToAdd.Columns[0];

            tableObjectsToAdd.PrimaryKey = keys;

            Session["tableObjectsToAdd"] = tableObjectsToAdd;
        }

        GridViewObjects.SettingsEditing.Mode = GridViewEditingMode.Inline;
        GridViewObjects.DataSource = tableObjectsToAdd;
        GridViewObjects.DataBind();
        UpdateAllCoordinates();

        // Create a cache for balans properties
        if (Session["balansOrganizations"] == null)
        {
            Session["balansOrganizations"] = new Dictionary<int, string>();
            Session["balansDistricts"] = new Dictionary<int, string>();
            Session["balansStreets"] = new Dictionary<int, string>();
            Session["balansNumbers"] = new Dictionary<int, string>();
            Session["balansSqr"] = new Dictionary<int, string>();
            Session["balansObjGroup"] = new Dictionary<int, string>();
            Session["balansObjName"] = new Dictionary<int, string>();
        }

        // Set the current date to the workflow date edit
        if (!IsPostBack)
        {
            EditWorkfowStartDate.Date = DateTime.Now;
            EditWorkfowEditDate.Date = DateTime.Now;
        }
    }

    protected void GridViewParentDocuments_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView grid = sender as ASPxGridView;

        object[] docIds = new object[grid.VisibleRowCount];
        object[] docNums = new object[grid.VisibleRowCount];
        object[] docDates = new object[grid.VisibleRowCount];
        object[] docNames = new object[grid.VisibleRowCount];
        object[] docDisplayNames = new object[grid.VisibleRowCount];

        for (int i = 0; i < grid.VisibleRowCount; i++)
        {
            docIds[i] = grid.GetRowValues(i, "id");
            docNums[i] = grid.GetRowValues(i, "doc_num");
            docDates[i] = grid.GetRowValues(i, "doc_date");
            docNames[i] = grid.GetRowValues(i, "topic");
            docDisplayNames[i] = grid.GetRowValues(i, "search_name");
        }

        e.Properties["cpDocIds"] = docIds;
        e.Properties["cpDocNums"] = docNums;
        e.Properties["cpDocDates"] = docDates;
        e.Properties["cpDocNames"] = docNames;
        e.Properties["cpDocDisplayNames"] = docDisplayNames;
    }

    protected void GridViewObjectsToAdd_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        Dictionary<int, string> balansOrganizations = (Dictionary<int, string>)Session["balansOrganizations"];
        Dictionary<int, string> balansDistricts = (Dictionary<int, string>)Session["balansDistricts"];
        Dictionary<int, string> balansStreets = (Dictionary<int, string>)Session["balansStreets"];
        Dictionary<int, string> balansNumbers = (Dictionary<int, string>)Session["balansNumbers"];
        Dictionary<int, string> balansSqr = (Dictionary<int, string>)Session["balansSqr"];
        Dictionary<int, string> balansObjGroup = (Dictionary<int, string>)Session["balansObjGroup"];
        Dictionary<int, string> balansObjName = (Dictionary<int, string>)Session["balansObjName"];

        ASPxGridView grid = sender as ASPxGridView;

        object[] balIds = new object[grid.VisibleRowCount];
        object[] balDisplayNames = new object[grid.VisibleRowCount];

        for (int i = 0; i < grid.VisibleRowCount; i++)
        {
            object id = grid.GetRowValues(i, "balans_id");
            object organization = grid.GetRowValues(i, "org_short_name");
            object district = grid.GetRowValues(i, "district");
            object street = grid.GetRowValues(i, "street_full_name");
            object number = grid.GetRowValues(i, "addr_nomer");
            object square = grid.GetRowValues(i, "sqr_balans");
            object objGroup = grid.GetRowValues(i, "object_group");
            object objName = grid.GetRowValues(i, "object_name");
            object objKind = grid.GetRowValues(i, "object_kind");

            #region Data validation

            if (organization == null || organization is System.DBNull)
            {
                organization = "";
            }

            if (district == null || district is System.DBNull)
            {
                district = "";
            }

            if (street == null || street is System.DBNull)
            {
                street = "";
            }

            if (number == null || number is System.DBNull)
            {
                number = "";
            }

            if (objGroup == null || objGroup is System.DBNull)
            {
                objGroup = "";
            }

            if (objName == null || objName is System.DBNull)
            {
                objName = "";
            }

            if (objKind == null || objKind is System.DBNull)
            {
                objKind = "";
            }

            #endregion (Data validation)

            balIds[i] = id;
            balDisplayNames[i] = street.ToString() + " " + number.ToString();

            // Update the local cache of object properties
            int balansId = (int)id;
            string objectName = objName as string;

            balansOrganizations[balansId] = (string)organization;
            balansDistricts[balansId] = (string)district;
            balansStreets[balansId] = (string)street;
            balansNumbers[balansId] = (string)number;
            balansSqr[balansId] = square.ToString();
            balansObjGroup[balansId] = (string)objGroup;
            balansObjName[balansId] = (objectName.Length > 0) ? objectName : (string)objKind;
        }

        e.Properties["cpBalansIds"] = balIds;
        e.Properties["cpBalansDisplayNames"] = balDisplayNames;
    }

    protected void GridViewObjects_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        DataTable tableObjectsToAdd = (DataTable) Session["tableObjectsToAdd"];

        if (tableObjectsToAdd != null)
        {
            if (e.Parameters is string)
            {
                string callbackParams = e.Parameters as string;

                string deleteRequest = "delete: ";
                string insertRequest = "insert";

                if (callbackParams.IndexOf(deleteRequest) == 0)
                {
                    // This is a request to delete the selected row
                    string rowIndexStr = callbackParams.Remove(0, deleteRequest.Length);

                    if (rowIndexStr.Length > 0)
                    {
                        int selRow = int.Parse(rowIndexStr);

                        if (selRow >= 0)
                        {
                            object balansId = GridViewObjects.GetRowValues(selRow, GridViewObjects.KeyFieldName);

                            if (balansId is int)
                            {
                                DataRow row = tableObjectsToAdd.Rows.Find((int)balansId);

                                if (row != null)
                                {
                                    tableObjectsToAdd.Rows.Remove(row);
                                }
                            }
                        }
                    }
                }
                else if (callbackParams.IndexOf(insertRequest) == 0)
                {
                    // This is a request to insert a new row
                    AddNewObjectToGrid();
                }
            }

            GridViewObjects.DataSource = tableObjectsToAdd;
            GridViewObjects.DataBind();
            UpdateAllCoordinates();
        }
    }

    protected void AddNewObjectToGrid()
    {
        Dictionary<int, string> balansOrganizations = (Dictionary<int, string>)Session["balansOrganizations"];
        Dictionary<int, string> balansDistricts = (Dictionary<int, string>)Session["balansDistricts"];
        Dictionary<int, string> balansStreets = (Dictionary<int, string>)Session["balansStreets"];
        Dictionary<int, string> balansNumbers = (Dictionary<int, string>)Session["balansNumbers"];
        Dictionary<int, string> balansSqr = (Dictionary<int, string>)Session["balansSqr"];
        Dictionary<int, string> balansObjGroup = (Dictionary<int, string>)Session["balansObjGroup"];
        Dictionary<int, string> balansObjName = (Dictionary<int, string>)Session["balansObjName"];

        DataTable tableObjectsToAdd = (DataTable)Session["tableObjectsToAdd"];

        if (DropDownObjectToAdd.KeyValue is string)
        {
            int balansId = int.Parse((string)DropDownObjectToAdd.KeyValue);

            // Get the properties of this balans entry
            string organization = "";
            string street = "";
            string district = "";
            string number = "";
            string square = "";
            string group = "";
            string name = "";

            if (balansOrganizations.ContainsKey(balansId))
            {
                organization = balansOrganizations[balansId];
            }

            if (balansStreets.ContainsKey(balansId))
            {
                street = balansStreets[balansId];
            }

            if (balansDistricts.ContainsKey(balansId))
            {
                district = balansDistricts[balansId];
            }

            if (balansNumbers.ContainsKey(balansId))
            {
                number = balansNumbers[balansId];
            }

            if (balansSqr.ContainsKey(balansId))
            {
                square = balansSqr[balansId];
            }

            if (balansObjGroup.ContainsKey(balansId))
            {
                group = balansObjGroup[balansId];
            }

            if (balansObjName.ContainsKey(balansId))
            {
                name = balansObjName[balansId];
            }

            // Do not add the row if we already have such balans_id
            DataRow row = tableObjectsToAdd.Rows.Find(balansId);

            if (row == null)
            {
                GeocodingAPI.Location location = GeocodingAPI.Resolve(
                    street + " " + number.Replace("ЛІТ.", " ") + ", Киев, Украина");

                object[] values = new object[] {
                    balansId, organization, district, street, number, group, square, square, "",
                    ComboPrivatizationKind.Value.ToString(), 
                    (location == null ? 0.0 : location.Lat), 
                    (location == null ? 0.0 : location.Lng) };

                tableObjectsToAdd.Rows.Add(values);
            }

            // Re-bind the grid of objects
            GridViewObjects.DataSource = tableObjectsToAdd;
            GridViewObjects.DataBind();
            UpdateAllCoordinates();
        }
    }

    protected void GridViewObjects_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        DataTable tableObjectsToAdd = (DataTable) Session["tableObjectsToAdd"];

        if (tableObjectsToAdd != null)
        {
            // Find the balans_id of the updated row
            int balansId = -1;

            foreach (DictionaryEntry keyEntry in e.Keys)
            {
                if (keyEntry.Key.ToString().ToLower() == "balans_id")
                {
                    balansId = (int)keyEntry.Value;
                    break;
                }
            }

            if (balansId >= 0)
            {
                // Find the row in our local data table
                DataRow row = tableObjectsToAdd.Rows.Find(balansId);

                if (row != null)
                {
                    // Modify the row values
                    object[] rowItems = row.ItemArray;

                    foreach (DictionaryEntry valueEntry in e.NewValues)
                    {
                        string fieldName = valueEntry.Key.ToString().ToLower();

                        if (fieldName == "object_group")
                        {
                            if (valueEntry.Value != null)
                                rowItems[5] = valueEntry.Value.ToString();
                        }
                        else if (fieldName == "sqr_privat")
                        {
                            if (valueEntry.Value != null)
                                rowItems[7] = valueEntry.Value.ToString();
                        }
                        else if (fieldName == "floor")
                        {
                            if (valueEntry.Value != null)
                                rowItems[8] = valueEntry.Value.ToString();
                        }
                        else if (fieldName == "privat_kind")
                        {
                            if (valueEntry.Value != null)
                                rowItems[9] = valueEntry.Value.ToString();
                        }
                    }

                    row.ItemArray = rowItems;

                    // Re-bind the grid of objects
                    GridViewObjects.CancelEdit();
                    GridViewObjects.DataSource = tableObjectsToAdd;
                    GridViewObjects.DataBind();
                    UpdateAllCoordinates();
                }
            }
        }

        e.Cancel = true;
    }

    protected void ButtonCreateDocument_Click(object sender, EventArgs e)
    {
        using (TempFile tempFile = new TempFile())
        {
            string templateName = "RishenTemplate.docx";
            string templatePath = "DocTemplates/" + templateName;
            string templateFullName = Server.MapPath(templatePath);

            GenerateRishenProjectInWord(templateFullName, tempFile.FileName);

            System.IO.FileInfo info = new System.IO.FileInfo(templateFullName);

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/ms-word";
            Response.AddHeader("content-disposition", "attachment; filename=ProektRishennia.docx; size=" + info.Length.ToString());

            // Pipe the stream contents to the output stream
            using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName,
                System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
            {
                stream.CopyTo(Response.OutputStream);
            }

            Response.End();
        }
    }

    protected void GridViewParentDocuments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ((ASPxGridView)sender).FilterExpression = "StartsWith([doc_num], '" + e.Parameters + "')";
    }

    protected void GridViewObjectsToAdd_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ((ASPxGridView)sender).FilterExpression = "StartsWith([street_full_name], '" + e.Parameters + "')";
    }

    #region Generation of the Word document from template

    /// <summary>
    /// Define an object to pass to the API for missing parameters
    /// </summary>
    private object missing = System.Type.Missing;

    protected void GenerateRishenProjectInWord(string templateFileName, string outputFileName)
    {
        Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

        app.Visible = false;

        Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(templateFileName);

        if (doc != null)
        {
            GenerateRishenSectionsInWord(doc, "%SELL_START%", "%SELL_END%", Resources.Strings.PrivatKindSell);

            GenerateRishenSectionsInWord(doc, "%AUCT_START%", "%AUCT_END%", Resources.Strings.PrivatKindAuction);

            doc.SaveAs(outputFileName);
            doc.Close();
        }

        app.Quit();
    }

    private void FilterRishObjects(string group, string privatizationKind,
        List<string> objNames, List<string> objOrganizations,
        List<string> objAddresses, List<string> objSquare, List<string> objFloor)
    {
        objNames.Clear();
        objOrganizations.Clear();
        objAddresses.Clear();
        objSquare.Clear();
        objFloor.Clear();

        DataTable tableObjectsToAdd = (DataTable) Session["tableObjectsToAdd"];
        Dictionary<int, string> balansObjName = (Dictionary<int, string>)Session["balansObjName"];

        if (tableObjectsToAdd != null && balansObjName != null)
        {
            for (int n = 0; n < tableObjectsToAdd.Rows.Count; n++)
            {
                object[] rowItems = tableObjectsToAdd.Rows[n].ItemArray;

                int balansId = (int)rowItems[0];
                string organization = rowItems[1] as string;
                string street = rowItems[3] as string;
                string number = rowItems[4] as string;
                string objGroup = rowItems[5] as string;
                string square = rowItems[7] as string;
                object floor = rowItems[8];
                string privKind = rowItems[9] as string;

                string name = "";

                if (balansObjName.ContainsKey(balansId))
                {
                    name = balansObjName[balansId];
                }

                // Check the values against the filter
                if (privKind.Trim().ToLower() == privatizationKind.Trim().ToLower() &&
                    objGroup.Trim().ToLower() == group.Trim().ToLower())
                {
                    // Return this row to the caller
                    objNames.Add(name);
                    objOrganizations.Add(organization);
                    objAddresses.Add(street + " " + number);
                    objSquare.Add(square);

                    if (floor is string && ((string)floor).Length > 0)
                    {
                        objFloor.Add((string)floor);
                    }
                    else
                    {
                        objFloor.Add("");
                    }
                }
            }
        }
    }

    private void GenerateRishenSectionsInWord(Microsoft.Office.Interop.Word.Document doc,
        string startTag, string endTag, string privatizationKind)
    {
        List<string> objNames = new List<string>();
        List<string> objOrganizations = new List<string>();
        List<string> objAddresses = new List<string>();
        List<string> objSquare = new List<string>();
        List<string> objFloor = new List<string>();

        // Load strings from resources
        string objectGroupsStr = Resources.Strings.ObjectGroups;
        string cityStr = Resources.Strings.RishenCityName;
        string streetStr = Resources.Strings.RishenStreet;
        string floorStr = Resources.Strings.RishenPoverh;

        // 1) Find the range that contains the table with objects of this privatization kind
        int templateRangeStart = -1;
        int templateRangeEnd = -1;

        foreach (Microsoft.Office.Interop.Word.Range r in doc.StoryRanges)
        {
            foreach (Microsoft.Office.Interop.Word.Paragraph para in r.Paragraphs)
            {
                if (para.Range.Text.IndexOf(startTag) >= 0)
                {
                    templateRangeStart = para.Range.Start;
                }

                if (para.Range.Text.IndexOf(endTag) >= 0)
                {
                    templateRangeEnd = para.Range.End;
                }
            }
        }

        if (templateRangeStart >= 0 && templateRangeEnd > templateRangeStart)
        {
            int insertionPos = templateRangeEnd;

            // Process each object group separately
            foreach (char groupCode in objectGroupsStr)
            {
                string group = "" + groupCode;

                FilterRishObjects(group, privatizationKind, objNames, objOrganizations, objAddresses, objSquare, objFloor);

                if (objNames.Count > 0)
                {
                    // Generate a new table for each group of objects
                    Microsoft.Office.Interop.Word.Range rngSource = doc.Range(templateRangeStart, templateRangeEnd);

                    rngSource.Copy();

                    Microsoft.Office.Interop.Word.Range rngDestination = doc.Range(insertionPos, insertionPos);

                    rngDestination.Paste();

                    // There must be a table in the pasted region
                    if (rngDestination.Tables.Count > 0)
                    {
                        Microsoft.Office.Interop.Word.Table table = rngDestination.Tables[1];

                        // Add rows to the table
                        for (int i = 0; i < objNames.Count; i++)
                        {
                            Microsoft.Office.Interop.Word.Row row = table.Rows.Add();

                            if (row.Cells.Count == 5)
                            {
                                Microsoft.Office.Interop.Word.Cell cell1 = row.Cells[1];
                                Microsoft.Office.Interop.Word.Cell cell2 = row.Cells[2];
                                Microsoft.Office.Interop.Word.Cell cell3 = row.Cells[3];
                                Microsoft.Office.Interop.Word.Cell cell4 = row.Cells[4];
                                Microsoft.Office.Interop.Word.Cell cell5 = row.Cells[5];

                                cell1.Range.Text = (i + 1).ToString();
                                cell2.Range.Text = objNames[i] + objOrganizations[i];
                                cell3.Range.Text = cityStr + streetStr + " " + objAddresses[i];
                                cell4.Range.Text = objSquare[i];

                                string floor = objFloor[i].Trim();

                                if (floor.Length > 0)
                                {
                                    cell5.Range.Text = floorStr + " " + floor;
                                }

                                // Insert some paragraphs, to make text more readable
                                Microsoft.Office.Interop.Word.Range rngCell2 = doc.Range(
                                    cell2.Range.Start, cell2.Range.Start + objNames[i].Length);

                                rngCell2.InsertParagraphAfter();

                                Microsoft.Office.Interop.Word.Range rngCell3 = doc.Range(
                                    cell3.Range.Start, cell3.Range.Start + cityStr.Length);

                                rngCell3.InsertParagraphAfter();
                            }
                        }

                        // The fourth row in the table is just a template; remove it
                        if (table.Rows.Count > 3)
                        {
                            Microsoft.Office.Interop.Word.Row row = table.Rows[4];

                            row.Delete();
                        }

                        // Insert the group code to all places where it is required
                        ReplaceTextInWord(rngDestination, "%GROUP%", group);

                        // Delete the starting and ending tag in the copied range
                        ReplaceTextInWord(rngDestination, startTag, "");
                        ReplaceTextInWord(rngDestination, endTag, "");

                        // We need to insert the next table after the pasted one
                        insertionPos = rngDestination.End;
                    }
                }
            }

            // Delete the template table
            Microsoft.Office.Interop.Word.Range rngToDelete = doc.Range(templateRangeStart, templateRangeEnd);

            rngToDelete.Delete();
        }
    }

    private void ReplaceTextInWord(Microsoft.Office.Interop.Word.Range range,
        string textToFind, string textToReplace)
    {
        // Set the text to find and replace
        range.Find.Text = textToFind;
        range.Find.Replacement.Text = textToReplace;

        // Set the Find.Wrap property to continue (so it doesn't
        // prompt the user or stop when it hits the end of
        // the section)
        range.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindStop;

        // Declare an object to pass as a parameter that sets
        // the Replace parameter to the "wdReplaceAll" enum
        object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;

        // Execute the Find and Replace -- notice that the
        // 11th parameter is the "replaceAll" enum object
        range.Find.Execute(ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing, ref replaceAll,
            ref missing, ref missing, ref missing, ref missing);
    }

    #endregion (Generation of the Word document from template)
    
    protected void ASPxButton_Privatization_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(GridViewPrivatizationExporter);
    }
    protected void ASPxButton_Privatization_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(GridViewPrivatizationExporter);
    }
    protected void ASPxButton_Privatization_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(GridViewPrivatizationExporter);
    }

    private void UpdateAllCoordinates()
    {
        DataTable table = (DataTable)GridViewObjects.DataSource;
        GridViewObjects.JSProperties["cpAllCoordinates"] =
            "[" + string.Join(",", table.Rows.Cast<DataRow>().Select(
                x => string.Format("{{ \"lat\":{0}, \"lng\":{1}, \"tooltip\":\"{2}\", \"html\":\"{3}\" }}",
                    ((double)x["geo_lat"]).ToString(NumberFormatInfo.InvariantInfo),
                    ((double)x["geo_lng"]).ToString(NumberFormatInfo.InvariantInfo),
                    (x["street"] + " " + ((string)x["number"]).Replace("ЛІТ.", " ")).EscapeJSONParam(),
                    ("<strong>" + x["street"] + " " + x["number"] + "</strong><br /><br />"
                        + "Балансоутримувач:<br /><strong>" + x["organization"] + "</strong><br />"
                        + "Площа на балансі: <strong>" + x["sqr_balans"] + "</strong> кв.м.").EscapeJSONParam())
                ).ToArray()
            ) + "]";
        GridViewObjects.JSProperties["cpAllCoordinatesCenter"] =
            string.Format("{{ \"lat\":{0}, \"lng\":{1} }}",
                (table.Rows.Count == 0 ? 0.0 : table.Rows.Cast<DataRow>().Average(x => (double)x["geo_lat"])).ToString(NumberFormatInfo.InvariantInfo),
                (table.Rows.Count == 0 ? 0.0 : table.Rows.Cast<DataRow>().Average(x => (double)x["geo_lng"])).ToString(NumberFormatInfo.InvariantInfo)
            );
    }

    private int ExpandedPrivatMasterDocID
    {
        get { return Session["ExpandedPrivatMasterDocID"] is int ? (int)Session["ExpandedPrivatMasterDocID"] : 0; }
        set { Session["ExpandedPrivatMasterDocID"] = value; }
    }

    private int ExpandedPrivatSlaveDocID
    {
        get { return Session["ExpandedPrivatSlaveDocID"] is int ? (int)Session["ExpandedPrivatSlaveDocID"] : 0; }
        set { Session["ExpandedPrivatSlaveDocID"] = value; }
    }

    protected void GridViewPrivatizationDetail_BeforePerformDataSelect(object sender, EventArgs e)
    {
        ASPxGridView GridViewPrivatizationDetail = (ASPxGridView)sender;

        object masterDocId = GridViewPrivatizationDetail.GetMasterRowFieldValues("master_doc_id");
        object slaveDocId = GridViewPrivatizationDetail.GetMasterRowFieldValues("slave_doc_id");

        if (masterDocId is int)
        {
            ExpandedPrivatMasterDocID = (int)masterDocId;
        }
        else
        {
            ExpandedPrivatMasterDocID = 0;
        }

        if (slaveDocId is int)
        {
            ExpandedPrivatSlaveDocID = (int)slaveDocId;
        }
        else
        {
            ExpandedPrivatSlaveDocID = 0;
        }
    }

    protected void SqlDataSourceViewPrivatDetail_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@p_master_doc_id"].Value = ExpandedPrivatMasterDocID;
        e.Command.Parameters["@p_slave_doc_id"].Value = ExpandedPrivatSlaveDocID;
    }
}
