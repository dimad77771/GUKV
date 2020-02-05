using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using GUKV;
using DevExpress.Web.ASPxGridView;
using GUKV.Conveyancing;
using System.Data.SqlClient;

public partial class Reports1NF_MassTransfer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsCallback)
            LoadBalans1();

        gridView1.DataSource = data1;
        gridView1.DataBind();

        gridView2.DataSource = data2;
        gridView2.DataBind();

        SqlDataSourceReport.SelectParameters["report_id"].DefaultValue = ReportID1.ToString();
        SqlDataSourceReport.DataBind();
    }

    private void LoadBalans1()
    {
        OrganizationID1 = Utils.GetUserOrganizationID(User.Identity.Name);
        ReportID1 = Utils.GetReportIdByOrganizationId(OrganizationID1);
        data1 = null;
        gridView1.Selection.UnselectAll();
        gridView1.PageIndex = 0;
        if ((OrganizationID1 > 0) && (ReportID1 > 0))
            data1 = GUKV.BalansObject.Select(OrganizationID1, ReportID1).ToList();
    }

    private void LoadBalans2(int orgId)
    {
        OrganizationID2 = orgId;
        ReportID2 = Utils.GetReportIdByOrganizationId(OrganizationID2);
        data2 = null;
        gridView2.Selection.UnselectAll();
        gridView2.PageIndex = 0;
        if ((OrganizationID2 > 0) && (ReportID2 > 0))
            data2 = GUKV.BalansObject.Select(OrganizationID2, ReportID2);
    }

    protected int OrganizationID1
    {
        get
        {
            object val = Session[GetPageUniqueKey() + "_ORGANIZATION_ID1"];

            if (val is int)
            {
                return (int)val;
            }

            return 0;
        }

        set
        {
            Session[GetPageUniqueKey() + "_ORGANIZATION_ID1"] = value;
        }
    }

    protected int ReportID1
    {
        get
        {
            object val = Session[GetPageUniqueKey() + "_REPORT_ID1"];

            if (val is int)
            {
                return (int)val;
            }

            return 0;
        }

        set
        {
            Session[GetPageUniqueKey() + "_REPORT_ID1"] = value;
        }
    }

    protected int OrganizationID2
    {
        get
        {
            object val = Session[GetPageUniqueKey() + "_ORGANIZATION_ID2"];

            if (val is int)
            {
                return (int)val;
            }

            return 0;
        }

        set
        {
            Session[GetPageUniqueKey() + "_ORGANIZATION_ID2"] = value;
        }
    }

    protected int ReportID2
    {
        get
        {
            object val = Session[GetPageUniqueKey() + "_REPORT_ID2"];

            if (val is int)
            {
                return (int)val;
            }

            return 0;
        }

        set
        {
            Session[GetPageUniqueKey() + "_REPORT_ID2"] = value;
        }
    }

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

    private IEnumerable<BalansObject> data1
    {
        get
        {
            object d = Session[GetPageUniqueKey() + "_Data1"];
            return (d is IEnumerable<BalansObject>) ? (IEnumerable<BalansObject>)d : null;
        }
        set
        {
            Session[GetPageUniqueKey() + "_Data1"] = value;
        }
    }

    private IEnumerable<BalansObject> data2
    {
        get
        {
            object d = Session[GetPageUniqueKey() + "_Data2"];
            return (d is IEnumerable<BalansObject>) ? (IEnumerable<BalansObject>)d : null;
        }
        set
        {
            Session[GetPageUniqueKey() + "_Data2"] = value;
        }
    }

    private string RenterOrgZkpoPattern
    {
        get
        {
            object pattern = Session[GetPageUniqueKey() + "_RenterOrgZkpoPattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session[GetPageUniqueKey() + "_RenterOrgZkpoPattern"] = value;
        }
    }

    private string RenterOrgNamePattern
    {
        get
        {
            object pattern = Session[GetPageUniqueKey() + "_RenterOrgNamePattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session[GetPageUniqueKey() + "_RenterOrgNamePattern"] = value;
        }
    }

    protected void ComboOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] parts = e.Parameter.Split(new char[] { '|' });

        if (parts.Length == 2)
        {
            RenterOrgZkpoPattern = parts[0].Trim();
            RenterOrgNamePattern = parts[1].Trim().ToUpper();

            if (string.IsNullOrEmpty(RenterOrgZkpoPattern) && string.IsNullOrEmpty(RenterOrgNamePattern))
            {
                LoadBalans2(0);
                gridView2.DataSource = data2;
                gridView2.DataBind();
            }

            ASPxComboBox combobox = sender as ASPxComboBox;
            combobox.DataBind();

            if (combobox.Items.Count > 0)
            {
                combobox.SelectedIndex = 0;
            }
        }


    }

    protected void SqlDataSourceSearchOrgFrom_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string orgName = RenterOrgNamePattern;
        string zkpo = RenterOrgZkpoPattern;

        if (string.IsNullOrEmpty(RenterOrgZkpoPattern) && string.IsNullOrEmpty(RenterOrgNamePattern))
        {
            LoadBalans2(0);
            gridView2.DataSource = data2;
            gridView2.DataBind();
        }

        ASPxComboBox combobox = (ASPxComboBox)this.FindControlRecursive("ComboOrgFrom");
        if ((combobox != null) && (combobox.ClientValue != null))
        {
            e.Command.Parameters["@org_id"].Value = combobox.ClientValue;
        }

        e.Command.Parameters["@rep_id"].Value = ReportID2;

        if (orgName.Length == 0 && zkpo.Length == 0)
        {
            e.Command.Parameters["@zkpo"].Value = "^";
            e.Command.Parameters["@fname"].Value = "^";
        }
        else
        {
            e.Command.Parameters["@zkpo"].Value = zkpo.Length > 0 ? "%" + zkpo + "%" : "%";
            e.Command.Parameters["@fname"].Value = orgName.Length > 0 ? "%" + orgName + "%" : "%";
        }
    }

    protected void ComboOrgFrom_ValueChanged(object sender, EventArgs e)
    {
        if (ComboOrgFrom.Value != null)
        {
            if (OrganizationID2 != (int)ComboOrgFrom.Value)
            {
                LoadBalans1();
                gridView1.DataSource = data1;
                gridView1.DataBind();

                LoadBalans2((int)ComboOrgFrom.Value);
                gridView2.DataSource = data2;
                gridView2.DataBind();
            }
        }
        else
        {
            LoadBalans2(0);
            gridView2.DataSource = data2;
            gridView2.DataBind();
        }
    }

    protected void SqlDataSourceReport_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }

    private void TransferUI(ref IEnumerable<BalansObject> source, ref IEnumerable<BalansObject> destination, ASPxGridView sourceGrid)
    {

        Dictionary<int, BalansObject> list1 = source == null ? new Dictionary<int, BalansObject>() : source.ToDictionary(x => x.BalansId);
        Dictionary<int, BalansObject> list2 = destination == null ? new Dictionary<int, BalansObject>() : destination.ToDictionary(x => x.BalansId);
        List<int> idList = new List<int>();

        Dictionary<int, BalansObject> list2result = new Dictionary<int,BalansObject>();

        foreach (KeyValuePair<int, BalansObject> b1 in list1)
        {
            if (sourceGrid.Selection.IsRowSelectedByKey(b1.Key) && !list2.ContainsKey(b1.Key) && !list2result.ContainsKey(b1.Key))
            {
                list2result.Add(b1.Key, b1.Value);
                idList.Add(b1.Key);
            }
        }

        foreach (KeyValuePair<int, BalansObject> b2 in list2)
        {
            list2result.Add(b2.Key, b2.Value);
        }
        
        sourceGrid.Selection.UnselectAll();

        foreach (int i in idList)
            list1.Remove(i);

        source = list1.Values.AsEnumerable();
        destination = list2result.Values.AsEnumerable();
    }

    protected void ContentCallback_Callback(object sender, CallbackEventArgsBase e)
    {
        if (string.IsNullOrEmpty(e.Parameter))
            return;

        if (e.Parameter.ToLower().StartsWith("selectall1:"))
        {
            gridView1.Selection.SelectAll();
            return;
        }
        if (e.Parameter.ToLower().StartsWith("selectall2:"))
        {
            gridView2.Selection.SelectAll();
            return;
        }

        if (e.Parameter.ToLower().StartsWith("sendselected"))
        {
            IEnumerable<BalansObject> d1 = data1;
            IEnumerable<BalansObject> d2 = data2;

            if (e.Parameter.ToLower().EndsWith("1:"))
                TransferUI(ref d1, ref d2, gridView1);

            if (e.Parameter.ToLower().EndsWith("2:"))
                TransferUI(ref d2, ref d1, gridView2);

            data1 = d1;
            gridView1.DataSource = data1;
            gridView1.DataBind();

            data2 = d2;
            gridView2.DataSource = data2;
            gridView2.DataBind();

            return;
        }

        if (e.Parameter.ToLower().StartsWith("transfer:"))
        {
            Transfer();
        }

        if (e.Parameter.ToLower().StartsWith("cancel:"))
        {
            DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback("ConveyancingRequestsList.aspx");
        }
    }

    private static void ColorRow(DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e, int gridOrgId)
    {
        if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
            return;

        int value = (int)e.GetValue("OrganizationId");
        if (value != gridOrgId)
            e.Row.ForeColor = System.Drawing.Color.GreenYellow;
    }

    protected void gridView1_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
    {
        ColorRow(e, OrganizationID1);
    }

    protected void gridView2_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
    {
        ColorRow(e, OrganizationID2);
    }

    protected void ComboRozpDoc_OnItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
    {
        //log.InfoFormat("ComboRozpDoc_OnItemRequestedByValue (e.Value={0})", e.Value);

        ASPxComboBox comboBox = (ASPxComboBox)source;
        SqlDataSource1.SelectCommand = @"SELECT d.id, doc_num, d.kind_id, k.name doc_kind, CONVERT(VARCHAR, doc_date, 104) doc_date, topic FROM documents d
                                         LEFT JOIN dict_doc_kind k ON d.kind_id = k.id
                                         WHERE d.id = @id";

        int value = 0;
        if (e.Value != null)
            int.TryParse(e.Value.ToString(), out value);

        SqlDataSource1.SelectParameters.Clear();
        SqlDataSource1.SelectParameters.Add("id", TypeCode.Int32, value.ToString());
        comboBox.DataSource = SqlDataSource1;
        comboBox.DataBindItems();
    }

    protected void ComboRozpDoc_OnItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
    {
        //log.InfoFormat("ComboRozpDoc_OnItemsRequestedByFilterCondition (e.Filter={0}, e.BeginIndex={1}, e.EndIndex={2})", e.Filter, e.BeginIndex, e.EndIndex);

        ASPxComboBox comboBox = (ASPxComboBox)source;

        SqlDataSource1.SelectParameters.Clear();

        string filterParam = string.Empty;
        string likeQuery = string.Empty;
        if ((!string.IsNullOrEmpty(e.Filter)) && (e.Filter.Length > 0))
        {
            if (e.Filter.Length >= 3)
            {
                SqlDataSource1.SelectCommand =
                       @"SELECT st.id, doc_num, st.kind_id, k.name doc_kind, CONVERT(VARCHAR, doc_date, 104) doc_date, topic 
                                 FROM (select id, t.kind_id, doc_num, doc_date, topic, row_number() over(order by t.doc_num)
                	             AS rn from documents t 
                                 INNER JOIN rozp_doc_kinds r ON t.kind_id = r.kind_id
                                 WHERE doc_num like @filter) as st                 
                                 LEFT JOIN dict_doc_kind k ON st.kind_id = k.id
                	             WHERE doc_num <> ''  AND st.rn between @startIndex and @endIndex";
                SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            }
            else
            {
                SqlDataSource1.SelectCommand =
                    @"SELECT st.id, doc_num, st.kind_id, k.name doc_kind, CONVERT(VARCHAR, doc_date, 104) doc_date, topic 
                    FROM (select id, t.kind_id, doc_num, doc_date, topic, row_number() over(order by t.doc_num)
	                AS rn from documents t 
                    INNER JOIN rozp_doc_kinds r ON t.kind_id = r.kind_id
                    WHERE doc_num = @filter) as st                 
                    LEFT JOIN dict_doc_kind k ON st.kind_id = k.id
	                WHERE doc_num <> ''  AND st.rn between @startIndex and @endIndex";
                SqlDataSource1.SelectParameters.Add("filter", TypeCode.String, e.Filter);
            }
        }
        else
        {
            SqlDataSource1.SelectCommand =
                @"SELECT st.id, doc_num, st.kind_id, k.name doc_kind, CONVERT(VARCHAR, doc_date, 104) doc_date, topic 
                    FROM (select id, t.kind_id, doc_num, doc_date, topic, row_number() over(order by t.doc_num)
                    AS rn from documents t 
                    INNER JOIN rozp_doc_kinds r ON t.kind_id = r.kind_id) as st                 
                    LEFT JOIN dict_doc_kind k ON st.kind_id = k.id
                    WHERE doc_num <> ''  AND st.rn between @startIndex and @endIndex";
        }


        SqlDataSource1.SelectParameters.Add("startIndex", TypeCode.Int32, (e.BeginIndex + 1).ToString());
        SqlDataSource1.SelectParameters.Add("endIndex", TypeCode.Int32, (e.EndIndex + 1).ToString());
        comboBox.DataSource = SqlDataSource1;
        comboBox.DataBindItems();
    }

    protected void CPCreateRozpDoc_Callback(object sender, CallbackEventArgsBase e)
    {
        try
        {
            var doc_num = ((ASPxTextBox)Utils.FindControlRecursive(Page, "NewRozpNum")).Value.ToString();
            var doc_topic = ((ASPxMemo)Utils.FindControlRecursive(Page, "NewRozpName")).Value.ToString();
            var doc_date = (DateTime)(((ASPxDateEdit)Utils.FindControlRecursive(Page, "NewRozpDate")).Value);
            var doc_type = (int)((ASPxComboBox)Utils.FindControlRecursive(Page, "NewRozpType")).Value;
            var general_kind_id = 7; // закріплення майна
            int doc_id = -1;

            //Preferences preferences = new Preferences();

            var doc = new ImportedDoc();
            doc.docDate = doc_date;
            doc.docNum = doc_num;
            doc.docTitle = doc_topic;
            doc.docTypeId = 7;
            //GUKV.Conveyancing.DB.ExportDocument(preferences, doc);

            // CreateRozpDoc(doc_num, doc_topic, doc_date, doc_type); ПОКА ВЫКЛЮЧИТЬ


            SqlConnection connection = Utils.ConnectToDatabase();
            if (connection != null)
            {
                var query = @"INSERT INTO documents (id, kind_id, general_kind_id, doc_date, doc_num, topic, modified_by, modify_date)  OUTPUT INSERTED.ID
                        VALUES (@id, @kind_id, @general_kind_id, @doc_date, @doc_num, @topic, @modified_by, @modify_date)";

                using (SqlCommand cmdInsert = new SqlCommand(query, connection))
                {
                    cmdInsert.Parameters.Add("id", GetNewRozpDocID());
                    cmdInsert.Parameters.Add("kind_id", doc_type);
                    cmdInsert.Parameters.Add("general_kind_id", general_kind_id);
                    cmdInsert.Parameters.Add("doc_date", doc_date);
                    cmdInsert.Parameters.Add("doc_num", doc_num);
                    cmdInsert.Parameters.Add("topic", doc_topic);
                    cmdInsert.Parameters.Add("modified_by", ""); // выяснить, кто тут должен быть 
                    cmdInsert.Parameters.Add("modify_date", DateTime.Now);
                    doc_id = (Int32)cmdInsert.ExecuteScalar();
                }
                connection.Close();
            }

            ((ASPxMemo)Utils.FindControlRecursive(Page, "TextBoxRishName")).Text = doc_topic;
            ((ASPxDateEdit)Utils.FindControlRecursive(Page, "DateEditRishDate")).Value = doc_date;
            ((ASPxComboBox)Utils.FindControlRecursive(Page, "ComboRishRozpType")).Value = doc_type;
            //((ASPxComboBox)Utils.FindControlRecursive(Page, "ComboRishRozpType")).DataBind();
            ((ASPxComboBox)Utils.FindControlRecursive(Page, "ComboRozpDoc")).Value = doc_id;
            ((ASPxComboBox)Utils.FindControlRecursive(Page, "ComboRozpDoc")).DataBindItems();
        }
        catch (Exception ex)
        {
            //log.Error("General failure", ex);
        }
    }

    protected int GetNewRozpDocID()
    {
        var max_id = 0;
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string query = @"SELECT Max(id) AS max_id FROM documents WHERE /*id > 15000 AND*/ id < 1000000";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        max_id = (int)reader["max_id"] + 1;
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }

        return max_id;
    }


    protected void Transfer()
    {

        if (OrganizationID2 <= 0)
            throw new Exception("Ви не вказали балансоутримувача для проведення передачі об'ектів");

        if (OrganizationID1 == OrganizationID2)
            throw new Exception("Ви не вірно вказали балансоутримувача");

        int cntToTransfer1 = 0;
        if (data1 != null)
            foreach (BalansObject b1 in data1)
            {
                if (b1.OrganizationId != OrganizationID1)
                    cntToTransfer1++;
            }

        int cntToTransfer2 = 0;
        if (data2 != null)
            foreach (BalansObject b2 in data2)
            {
                if (b2.OrganizationId != OrganizationID2)
                    cntToTransfer2++;
            }

        if ((cntToTransfer1 > 0) && (cntToTransfer2 > 0))
            throw new Exception("В межах однієї операції можна зробити одну дію (або прийняти або передати об’єкти). Перевірте чи правильно Ви обрали об’єкти для проведення операції!");

        if ((cntToTransfer1 == 0) && (cntToTransfer2 == 0))
            throw new Exception("Ви не вибрали жодного об'екту для передачі");

        if (!ValidateControls())
            throw new Exception("Ви не заповнили обов'язкові поля.");

        if (cntToTransfer1 > 0)
        {
            List<int> idList = new List<int>();
            foreach (BalansObject b1 in data1)
            {
                if (b1.OrganizationId != OrganizationID1)
                {
                    int requestId = ProcessTransfer(1, OrganizationID1, OrganizationID2, b1.BalansId, b1.SqrTotal);
                    idList.Add(requestId);
                }
            }
            BalansTransferUtils.SendBalansTransferNotifications(1, idList);
            DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback("ConveyancingRequestsList.aspx");
        }

        if (cntToTransfer2 > 0)
        {
            List<int> idList = new List<int>();
            foreach (BalansObject b2 in data2)
            {
                if (b2.OrganizationId != OrganizationID2)
                {
                    int requestId = ProcessTransfer(2, OrganizationID1, OrganizationID2, b2.BalansId, b2.SqrTotal);
                    idList.Add(requestId);
                }
            }
            BalansTransferUtils.SendBalansTransferNotifications(2, idList);
            DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback("ConveyancingRequestsList.aspx");
        }
    
    }

    private bool ValidateControls()
    {
        return 
            //((ASPxMemo)Utils.FindControlRecursive(this, "TextBoxRishName")).Value != null &&
            //((ASPxDateEdit)Utils.FindControlRecursive(this, "DateEditRishDate")).Value != null &&
            //((ASPxComboBox)Utils.FindControlRecursive(this, "ComboRishRozpType")).Value != null &&
            ((ASPxComboBox)Utils.FindControlRecursive(this, "ASPxComboBoxRight")).Value != null &&
            ((ASPxComboBox)Utils.FindControlRecursive(this, "ComboRozpDoc")).Value != null &&
            ((ASPxComboBox)Utils.FindControlRecursive(this, "ComboRozpDoc")).Text != null &&
            ((ASPxTextEdit)Utils.FindControlRecursive(this, "ActNumber")).Value != null &&
            ((ASPxDateEdit)Utils.FindControlRecursive(this, "ActDate")).Value != null;// &&
            //((ASPxSpinEdit)Utils.FindControlRecursive(this, "ActSum")).Value != null &&
            //((ASPxSpinEdit)Utils.FindControlRecursive(this, "ActResidualSum")).Value != null;
    }

    private int ProcessTransfer(int ConveyancingType_Value, int ComboOrgFrom_Value, int ComboOrgTo_Value, int ObjectId_Value, decimal AreaForTransfer_Value)
    {
        // Розпорядчі документи
        object TextBoxRishName_Value = ((ASPxMemo)Utils.FindControlRecursive(this, "TextBoxRishName")).Value;
        DateTime DateEditRishDate_Value = (DateTime)((ASPxDateEdit)Utils.FindControlRecursive(this, "DateEditRishDate")).Value;
        object ComboRishRozpType_Value = ((ASPxComboBox)Utils.FindControlRecursive(this, "ComboRishRozpType")).Value;
        object ASPxComboBoxRight_Value = ((ASPxComboBox)Utils.FindControlRecursive(this, "ASPxComboBoxRight")).Value;

        object ComboRozpDoc_Value = ((ASPxComboBox)Utils.FindControlRecursive(this, "ComboRozpDoc")).Value;
        object ComboRozpDoc_Text = ((ASPxComboBox)Utils.FindControlRecursive(this, "ComboRozpDoc")).Text;

        // Акт
        object ActNumber_Value = ((ASPxTextEdit)Utils.FindControlRecursive(this, "ActNumber")).Value;
        DateTime ActDate_Value = (DateTime)((ASPxDateEdit)Utils.FindControlRecursive(this, "ActDate")).Value;
        decimal ActSum_Value = 0;// (decimal)((ASPxSpinEdit)Utils.FindControlRecursive(this, "ActSum")).Value;
        decimal ActResidualSum_Value = 0;// (decimal)((ASPxSpinEdit)Utils.FindControlRecursive(this, "ActResidualSum")).Value;

        int requestId = BalansTransferUtils.SaveRequest(User,
            ConveyancingType_Value,
            ComboOrgFrom_Value,
            ComboOrgTo_Value,
            ObjectId_Value,
            AreaForTransfer_Value,
            ActDate_Value,
            ActNumber_Value,
            ActSum_Value,
            ActResidualSum_Value,
            TextBoxRishName_Value,
            DateEditRishDate_Value,
            ComboRishRozpType_Value,
            ASPxComboBoxRight_Value,
            ComboRozpDoc_Value,
            ComboRozpDoc_Text);

        BalansTransferUtils.AproveTransferRequest(User, requestId, false);

        return requestId;
    }

    protected void OnValidation(object sender, ValidationEventArgs e)
    {
        ASPxTextEdit edit = sender as ASPxTextEdit;
        if (edit.Text.Trim() == "")
            e.IsValid = false;
    }

 }