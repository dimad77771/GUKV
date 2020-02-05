using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using System.Web.Security;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using GUKV.Conveyancing;
using DevExpress.Web.ASPxPopupControl;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.Configuration;
using GUKV.Common;

public partial class Reports1NF_TransferRequestAdd : System.Web.UI.Page
{
    public bool readOnlyMode = false;
    protected int userOrgId = 0;
    protected int requestId = 0;

    protected int orgIdForConfirm = -1;

    protected bool isObjectExists = false;

    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");

    protected void Page_Load(object sender, EventArgs e)
    {
        log.InfoFormat("reqid={0}, t={1}, rid={2}, IsPostback={3}, IsCallback={4}",
            Request.Params["reqid"], Request.Params["t"], Request.Params["rid"],
            IsPostBack, IsCallback);
        
        userOrgId = Utils.GetUserOrganizationID(User.Identity.Name);

        var strRequestId = Request.Params["reqid"];

        if (!string.IsNullOrEmpty(strRequestId))
            requestId = int.Parse(strRequestId);

        var typeOfTransfer = Request.Params["t"];

        if (!string.IsNullOrEmpty(typeOfTransfer))
            ConveyancingType.Value = typeOfTransfer;

        if (string.IsNullOrEmpty(strRequestId) && string.IsNullOrEmpty(typeOfTransfer))
            Response.Redirect("ConveyancingRequestsList.aspx");

        string reportIdStr = Request.Params["rid"];

        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);
        }

        if (ReportID > 0)
        {
            int reportRdaDistrictId = -1;
            int reportOrganizationId = Reports1NFUtils.GetReportOrganizationId(ReportID, ref reportRdaDistrictId);

            for (int i = 0; i < SectionMenu.Items.Count; i++)
            {
                if (SectionMenu.Items[i].NavigateUrl.IndexOf('?') < 0)
                    SectionMenu.Items[i].NavigateUrl += "?rid=" + ReportID.ToString();
            }
        }

        if (!string.IsNullOrEmpty(strRequestId))
        {
            ConveyancingForm.DataBind();

            DataRowView tRow = (DataRowView)ConveyancingForm.DataItem;
            if (tRow != null)
            {
                ConveyancingType.Value = tRow["conveyancing_type"].ToString();

                orgIdForConfirm = tRow["org_id_for_confirm"] == DBNull.Value ? -1 : (int)tRow["org_id_for_confirm"];
                
                isObjectExists = tRow["is_object_exists"] == DBNull.Value ? false : (bool)tRow["is_object_exists"];
            }
        }

        if (orgIdForConfirm != userOrgId)
            ASPxPanelApplyButtons.Visible = false;

    }

    /*
    protected void FillObjectInfo()
    {
        SqlConnection connectionSql = Utils.ConnectToDatabase();
        var selectQuery = "";
        
        if (isObjectExists)
            selectQuery = "SELECT vb.district, vb.street_full_name, vb.addr_nomer, vb.sqr_total, COALESCE(vb.balans_obj_name, vb.purpose) AS purpose FROM transfer_requests req LEFT JOIN view_balans_all vb ON req.balans_id = vb.balans_id WHERE request_id = @request_id";
        else
            selectQuery = "SELECT district.name AS district, street.name AS street_full_name, (COALESCE(LTRIM(RTRIM(ub.addr_nomer1)) + ' ', '') + COALESCE(LTRIM(RTRIM(ub.addr_nomer2)) + ' ', '') + COALESCE(LTRIM(RTRIM(ub.addr_nomer3)), '')) AS addr_nomer, ub.sqr_total, ub.purpose_str as purpose "
                + "FROM transfer_requests req LEFT JOIN unverified_balans ub ON req.balans_id = ub.id LEFT JOIN dict_streets street ON street.id = ub.addr_street_id LEFT JOIN dict_districts2 district ON ub.addr_district_id = district.id WHERE request_id =  @request_id";
            
        using (SqlCommand cmd = new SqlCommand(selectQuery, connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("request_id", requestId));
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ((ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "ASPxLabelObjAddress")).Text = (string)rdr["district"] + " Р-Н, " + (string)rdr["street_full_name"] + ", " + (string)rdr["addr_nomer"];
                    ((ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "ASPxLabelObjPurpose")).Text = (string)rdr["purpose"];
                    ((ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "ASPxLabelObjTotalArea")).Text = ((decimal)rdr["sqr_total"]).ToString();
                }
            }
        }

    }
     * 

    protected void TuneAccess()
    {
        if (Roles.IsUserInRole(User.Identity.Name, "ConveyancingConfirmation") || (orgIdForConfirm == userOrgId))
        {
            readOnlyMode = true;
            ASPxPanelInitButtons.Visible = false;
        }
        else 
        {
            readOnlyMode = true;
            ASPxPanelInitButtons.Visible = false;
        }

    }*/

    protected void CPCreateRozpDoc_Callback(object sender, CallbackEventArgsBase e)
    {
        try
        {
            var doc_num = ((ASPxTextBox)Utils.FindControlRecursive(ConveyancingForm, "NewRozpNum")).Value.ToString();
            var doc_topic = ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "NewRozpName")).Value.ToString();
            var doc_date = (DateTime)(((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "NewRozpDate")).Value);
            var doc_type = (int)((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "NewRozpType")).Value;
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

            ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "TextBoxRishName")).Text = doc_topic;
            ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "DateEditRishDate")).Value = doc_date;
            ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRishRozpType")).Value = doc_type;
            //((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRishRozpType")).DataBind();
            ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Value = doc_id;
            ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).DataBindItems();
        }
        catch (Exception ex)
        {
            log.Error("General failure", ex);
        }
    }

    protected void CreateRozpDoc(string docNum, string docTopic, DateTime docDate, int docType)
    {
        //Preferences preferences = new Preferences();

        var doc = new ImportedDoc();
        doc.docDate = docDate;
        doc.docNum = docNum;
        doc.docTitle = docTopic;
        doc.docTypeId = 1; // кмда

        //var docId = GUKV.Conveyancing.DB.ExportDocument(preferences, doc);
        /*
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string query = @"INSERT INTO documents (id, kind_id, general_kind_id, doc_date, doc_num, topic, modified_by, modify_date)
                    VALUES (@id, @kind_id, @general_kind_id, @doc_date, @doc_num, @topic, @modified_by, @modify_date)";

            using (SqlCommand cmdInsert = new SqlCommand(query, connection))
            {
                cmdInsert.Parameters.Add("id", docId);
                cmdInsert.Parameters.Add("kind_id", docType);
                cmdInsert.Parameters.Add("general_kind_id", 7); // закріплення майна
                cmdInsert.Parameters.Add("doc_date", docDate);
                cmdInsert.Parameters.Add("doc_num", docNum);
                cmdInsert.Parameters.Add("topic", docTopic);
                cmdInsert.Parameters.Add("modified_by", ""); // выяснить, кто тут должен быть 
                cmdInsert.Parameters.Add("modify_date", DateTime.Now);
                cmdInsert.ExecuteNonQuery();
            }
            connection.Close();
        }
         */
    }

    protected void CPMainPanel_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("save:"))
        {
            int balans_id = -1;
            var str_balans_id = ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "HdnObjectId")).Value;
            int.TryParse(str_balans_id, out balans_id);
            var comboOrgFrom = ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboOrgFrom"));
            var orgFromId = comboOrgFrom.Value is int ? (int)comboOrgFrom.Value : -1;

            if (ConveyancingType.Value == "1")
            {
                if (!ConveyancingUtils.IsCorrectOwner(balans_id, orgFromId))
                {
                    CPMainPanel.JSProperties["cp_status"] = "Вказана організація не є балансоутримувачем даного об'єкту";
                    return;
                }
            }
            else if (ConveyancingType.Value != "5")
            {
                userOrgId = Utils.GetUserOrganizationID(User.Identity.Name);
                if (!ConveyancingUtils.IsCorrectOwner(balans_id, userOrgId))
                {
                    CPMainPanel.JSProperties["cp_status"] = "Ви не є балансоутримувачем даного об'єкту";
                    return;
                }
            }

            if (ASPxEdit.AreEditorsValid(ConveyancingForm))
            {
                SaveRequest();
            }
            CPMainPanel.JSProperties["cp_status"] = "OK";
        }
    }

    protected void CPObjSel_Callback(object sender, CallbackEventArgsBase e)
    {
        var cp_status = "";

        if (e.Parameter.StartsWith("sel_obj:"))
        {
            string strObjectID = e.Parameter.Substring(8);

            int objectID = int.Parse(strObjectID);


            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                string query = @"SELECT district, street_full_name, addr_nomer, sqr_total, COALESCE(balans_obj_name, purpose) AS 'purpose' FROM view_balans_all 
                                    WHERE balans_id = @objId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("objId", objectID));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string district = (reader.IsDBNull(reader.GetOrdinal("district")) ? null : (string)reader["district"]);
                            string streetFullName = (reader.IsDBNull(reader.GetOrdinal("street_full_name")) ? null : (string)reader["street_full_name"]);
                            string addrNomer = (reader.IsDBNull(reader.GetOrdinal("addr_nomer")) ? null : (string)reader["addr_nomer"]);
                            string purpose = (reader.IsDBNull(reader.GetOrdinal("purpose")) ? string.Empty : (string)reader["purpose"]);
                            decimal sqrTotal = (reader.IsDBNull(reader.GetOrdinal("sqr_total")) ? 0m : (decimal)reader["sqr_total"]);

                            ((ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "ASPxLabelObjAddress")).Text =
                                (district == null ? string.Empty : district + " Р-Н, ")
                                + (streetFullName == null ? string.Empty : streetFullName + ", ")
                                + (addrNomer ?? string.Empty);
                            ((ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "ASPxLabelObjPurpose")).Text = purpose;
                            ((ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "ASPxLabelObjTotalArea")).Text = sqrTotal.ToString();
                            ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "AreaForTransfer")).Value = sqrTotal;
                            ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "HdnObjectId")).Value = objectID.ToString();
                            ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "IsExistingObject")).Value = "1";
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }

            cp_status = "selobjok";
        }
        else if (e.Parameter.StartsWith("createbalans:"))
        {
            var ComboBalansStreetNewObj = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBalansStreetNewObj");
            var ComboBalansBuildingNewObj = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBalansBuildingNewObj");
            var EditBalansObjSquare = (ASPxEdit)Utils.FindControlRecursive(ConveyancingForm, "EditBalansObjSquare");
            var EditBalansObjCost = (ASPxEdit)Utils.FindControlRecursive(ConveyancingForm, "EditBalansObjCost");
            var ComboBoxBalansOwnership = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBoxBalansOwnership");
            var ComboBoxBalansObjKind = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBoxBalansObjKind");
            var ComboBoxBalansObjType = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBoxBalansObjType");
            var ComboBoxBalansPurposeGroup = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBoxBalansPurposeGroup");
            var ComboBoxBalansPurpose = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBoxBalansPurpose");

            var newBalansId = ConveyancingUtils.CreateUnverifiedBalansObject(
                ComboBalansBuildingNewObj.Value is int ? (int)ComboBalansBuildingNewObj.Value : -1,
                (decimal)EditBalansObjSquare.Value,
                EditBalansObjCost.Value is decimal ? (decimal)EditBalansObjCost.Value : 0,
                ComboBoxBalansOwnership.Value is int ? (int)ComboBoxBalansOwnership.Value : -1,
                ComboBoxBalansObjKind.Value is int ? (int)ComboBoxBalansObjKind.Value : -1,
                ComboBoxBalansObjType.Value is int ? (int)ComboBoxBalansObjType.Value : -1,
                ComboBoxBalansPurposeGroup.Value is int ? (int)ComboBoxBalansPurposeGroup.Value : -1,
                ComboBoxBalansPurpose.Value is int ? (int)ComboBoxBalansPurpose.Value : -1,
                "");

            ((ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "ASPxLabelObjAddress")).Text = ComboBalansStreetNewObj.Text + ", " + ComboBalansBuildingNewObj.Text;
            ((ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "ASPxLabelObjPurpose")).Text = ComboBoxBalansPurpose.Text;
            ((ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "ASPxLabelObjTotalArea")).Text = EditBalansObjSquare.Value.ToString();
            ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "AreaForTransfer")).Value = EditBalansObjSquare.Value;
            ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "HdnObjectId")).Value = newBalansId.ToString();
            ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "IsExistingObject")).Value = "0";

            cp_status = "createbalansok";
        }
        else if (e.Parameter.StartsWith("createbuilding:"))
        {
            //FbConnection connection = Utils.ConnectTo1NF();

            var LabelBuildingCreationError = (ASPxLabel)Utils.FindControlRecursive(ConveyancingForm, "LabelBuildingCreationError");

            //if (connection != null)
            {
                string errorMessage = "";

                var ComboBalansStreetNewObj = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBalansStreetNewObj");
                var ComboBoxDistrict = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBoxDistrict");
                var ComboBoxBuildingKind = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBoxBuildingKind");
                var ComboBoxBuildingType = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBoxBuildingType");
                var TextBoxNumber1 = (ASPxTextEdit)Utils.FindControlRecursive(ConveyancingForm, "TextBoxNumber1");
                var TextBoxNumber2 = (ASPxTextEdit)Utils.FindControlRecursive(ConveyancingForm, "TextBoxNumber2");
                var TextBoxNumber3 = (ASPxTextEdit)Utils.FindControlRecursive(ConveyancingForm, "TextBoxNumber3");
                var TextBoxMiscAddr = (ASPxTextEdit)Utils.FindControlRecursive(ConveyancingForm, "TextBoxMiscAddr");
                var ComboBalansBuildingNewObj = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBalansBuildingNewObj");


                int newBuildingId = ConveyancingUtils.CreateNew1NFBuilding(
                    //connection,
                    ComboBalansStreetNewObj.Value is int ? (int)ComboBalansStreetNewObj.Value : -1,
                    ComboBoxDistrict.Value is int ? (int)ComboBoxDistrict.Value : -1,
                    ComboBoxBuildingKind.Value is int ? (int)ComboBoxBuildingKind.Value : -1,
                    ComboBoxBuildingType.Value is int ? (int)ComboBoxBuildingType.Value : -1,
                    TextBoxNumber1.Text,
                    TextBoxNumber2.Text,
                    TextBoxNumber3.Text,
                    TextBoxMiscAddr.Text,
                    //true,
                    out errorMessage);

                //connection.Close();

                if (newBuildingId > 0)
                {
                    AddressStreetID = ComboBalansStreetNewObj.Value is int ? (int)ComboBalansStreetNewObj.Value : -1;
                    ComboBalansBuildingNewObj.DataBind();
                    ComboBalansBuildingNewObj.SelectedItem = ComboBalansBuildingNewObj.Items.FindByValue(newBuildingId);
                }

                LabelBuildingCreationError.Text = errorMessage;
                LabelBuildingCreationError.ClientVisible = (errorMessage.Length > 0);
            }
            //else
            //{
            //    LabelBuildingCreationError.Text = "Неможливо установити зв'язок з базою 1НФ.";
            //    LabelBuildingCreationError.ClientVisible = true;
            //}
            cp_status = "createbuildingok";
        }
        var CPObjSel = (ASPxCallbackPanel)Utils.FindControlRecursive(ConveyancingForm, "CPObjSel");
        CPObjSel.JSProperties["cp_status"] = cp_status;
    }

    protected int ReportID
    {
        get
        {
            object reportId = ViewState["REPORT_ID"];

            if (reportId is int)
            {
                return (int)reportId;
            }

            return 0;
        }

        set
        {
            ViewState["REPORT_ID"] = value;
        }
    }

    protected void SaveRequest()
    {
        using (SqlConnection connectionSql = Utils.ConnectToDatabase())
        {
            using (SqlTransaction transaction = connectionSql.BeginTransaction())
            {
                if (requestId > 0)
                {
                    // This is a repeated submission of the same transfer request. Let's delete
                    // the old request, and follow the standard create procedure.
                    using (SqlCommand command = connectionSql.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 600;
                        command.CommandText = "DELETE FROM transfer_requests WHERE request_id=@reqid";
                        command.Transaction = transaction;

                        command.Parameters.AddWithValue("reqid", requestId);

                        command.ExecuteNonQuery();
                    }
                }

                Dictionary<string, object> parameters = new Dictionary<string, object>();

                string fieldList = "", paramList = "";

                string str_balans_id = ""; int balans_id = -1, org_from_id = -1;

                userOrgId = Utils.GetUserOrganizationID(User.Identity.Name);

                switch (ConveyancingType.Value)
                {
                    case "1": // Прийняти об'єкт на баланс

                        fieldList = "created_by, create_date, conveyancing_type, status, org_to_id, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_from_id, org_id_for_confirm, balans_id, rishrozp_doc_id, rish_num, is_object_exists, conveyancing_area";
                        paramList = "@created_by, @create_date, 1, 1, @org_to_id, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_from_id, @org_id_for_confirm, @balans_id, @rishrozp_doc_id, @rish_num, @is_object_exists, @conveyancing_area";


                        org_from_id = (int)((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboOrgFrom")).Value;

                        str_balans_id = ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "HdnObjectId")).Value;
                        if (int.TryParse(str_balans_id, out balans_id))
                        {
                            var isCorrect = ConveyancingUtils.IsCorrectOwner(balans_id, org_from_id);
                        };

                        parameters.Clear();
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("org_to_id", userOrgId);
                        parameters.Add("new_akt_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "ActDate")).Value);
                        parameters.Add("new_akt_num", ((ASPxTextEdit)Utils.FindControlRecursive(ConveyancingForm, "ActNumber")).Value);
                        parameters.Add("new_akt_summa", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActSum")).Value);
                        parameters.Add("new_akt_summa_zalishkova", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActResidualSum")).Value);
                        parameters.Add("rishrozp_name", ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "TextBoxRishName")).Value);
                        parameters.Add("rishrozp_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "DateEditRishDate")).Value);
                        parameters.Add("rish_doc_kind_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRishRozpType")).Value);
                        parameters.Add("obj_right_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ASPxComboBoxRight")).Value);
                        parameters.Add("balans_id", balans_id);
                        parameters.Add("org_id_for_confirm", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboOrgFrom")).Value);
                        parameters.Add("org_from_id", org_from_id);
                        parameters.Add("rishrozp_doc_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Value);
                        parameters.Add("rish_num", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Text);
                        parameters.Add("conveyancing_area", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "AreaForTransfer")).Value);
                        parameters.Add("is_object_exists", true);
                        break;
                    case "2": // Передати об'єкт з балансу

                        fieldList = "created_by, create_date, balans_id, conveyancing_type, status, org_to_id, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_from_id, org_id_for_confirm, comment, is_object_exists, conveyancing_area, rishrozp_doc_id, rish_num";
                        paramList = "@created_by, @create_date, @balans_id, 2, 1, @org_to_id, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_from_id, @org_id_for_confirm, @comment, @is_object_exists, @conveyancing_area, @rishrozp_doc_id, @rish_num";

                        str_balans_id = ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "HdnObjectId")).Value;
                        if (int.TryParse(str_balans_id, out balans_id))
                        {
                            var isCorrect = ConveyancingUtils.IsCorrectOwner(balans_id, org_from_id);
                        };

                        parameters.Clear();
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("org_to_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboOrgTo")).Value);
                        parameters.Add("new_akt_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "ActDate")).Value);
                        parameters.Add("new_akt_num", ((ASPxTextEdit)Utils.FindControlRecursive(ConveyancingForm, "ActNumber")).Value);
                        parameters.Add("new_akt_summa", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActSum")).Value);
                        parameters.Add("new_akt_summa_zalishkova", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActResidualSum")).Value);
                        parameters.Add("rishrozp_name", ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "TextBoxRishName")).Value);
                        parameters.Add("rishrozp_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "DateEditRishDate")).Value);
                        parameters.Add("rish_doc_kind_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRishRozpType")).Value);
                        parameters.Add("obj_right_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ASPxComboBoxRight")).Value);
                        parameters.Add("comment", ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "Comment")).Text);
                        parameters.Add("rishrozp_doc_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Value);
                        parameters.Add("rish_num", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Text);
                        parameters.Add("conveyancing_area", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "AreaForTransfer")).Value);
                        parameters.Add("is_object_exists", true);
                        parameters.Add("balans_id", balans_id);

                        parameters.Add("org_id_for_confirm", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboOrgTo")).Value);
                        parameters.Add("org_from_id", userOrgId);

                        break;
                    case "3": // Списати об'єкту з балансу шляхом зносу
                        fieldList = "created_by, balans_id, create_date, conveyancing_type, status, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_from_id, comment, conveyancing_area, is_object_exists, rishrozp_doc_id, rish_num";
                        paramList = "@created_by, @balans_id, @create_date, 3, 1, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_from_id, @comment, @conveyancing_area, @is_object_exists, @rishrozp_doc_id, @rish_num";

                        str_balans_id = ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "HdnObjectId")).Value;
                        if (int.TryParse(str_balans_id, out balans_id))
                        {
                            var isCorrect = ConveyancingUtils.IsCorrectOwner(balans_id, org_from_id);
                        };

                        parameters.Clear();
                        parameters.Add("balans_id", balans_id);
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("new_akt_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "ActDate")).Value);
                        parameters.Add("new_akt_num", ((ASPxTextEdit)Utils.FindControlRecursive(ConveyancingForm, "ActNumber")).Value);
                        parameters.Add("new_akt_summa", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActSum")).Value);
                        parameters.Add("new_akt_summa_zalishkova", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActResidualSum")).Value);
                        parameters.Add("rishrozp_name", ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "TextBoxRishName")).Value);
                        parameters.Add("rishrozp_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "DateEditRishDate")).Value);
                        parameters.Add("rish_doc_kind_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRishRozpType")).Value);
                        parameters.Add("rishrozp_doc_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Value);
                        parameters.Add("rish_num", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Text);
                        parameters.Add("obj_right_id", -1);
                        parameters.Add("comment", ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "Comment")).Text);
                        parameters.Add("conveyancing_area", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "AreaForTransfer")).Value);
                        parameters.Add("org_from_id", userOrgId);
                        parameters.Add("is_object_exists", true);

                        break;
                    case "4": // Списати об'єкту з балансу шляхом приватизації
                        fieldList = "created_by, balans_id, create_date, conveyancing_type, status, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_from_id, comment, rishrozp_doc_id, rish_num, conveyancing_area, is_object_exists, org_to_id, org_id_for_confirm";
                        paramList = "@created_by, @balans_id, @create_date, 4, 1, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_from_id, @comment, @rishrozp_doc_id, @rish_num, @conveyancing_area, @is_object_exists, @org_to_id, @org_id_for_confirm";

                        str_balans_id = ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "HdnObjectId")).Value;
                        if (int.TryParse(str_balans_id, out balans_id))
                        {
                            var isCorrect = ConveyancingUtils.IsCorrectOwner(balans_id, org_from_id);
                        };

                        parameters.Clear();
                        parameters.Add("balans_id", balans_id);
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("new_akt_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "ActDate")).Value);
                        parameters.Add("new_akt_num", ((ASPxTextEdit)Utils.FindControlRecursive(ConveyancingForm, "ActNumber")).Value);
                        parameters.Add("new_akt_summa", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActSum")).Value);
                        parameters.Add("new_akt_summa_zalishkova", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActResidualSum")).Value);
                        parameters.Add("rishrozp_name", ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "TextBoxRishName")).Value);
                        parameters.Add("rishrozp_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "DateEditRishDate")).Value);
                        parameters.Add("rish_doc_kind_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRishRozpType")).Value);
                        parameters.Add("rishrozp_doc_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Value);
                        parameters.Add("rish_num", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Text);
                        parameters.Add("obj_right_id", -1);
                        parameters.Add("comment", ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "Comment")).Text);
                        parameters.Add("conveyancing_area", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "AreaForTransfer")).Value);
                        parameters.Add("org_from_id", userOrgId);
                        parameters.Add("is_object_exists", true);

                        ASPxComboBox CombOrgTo = (ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboOrgTo");
                        parameters.Add("org_to_id", CombOrgTo.Value != null ? (int)CombOrgTo.Value : -1);
                        parameters.Add("org_id_for_confirm", CombOrgTo.Value != null ? (int)CombOrgTo.Value : -1);
                        break;
                    case "5": // Постановка  новозбудованого об'єкту на баланс
                        fieldList = "created_by, create_date, balans_id, is_object_exists, conveyancing_type, status, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_to_id, comment, rishrozp_doc_id, rish_num, conveyancing_area";
                        paramList = "@created_by, @create_date, @balans_id, @is_object_exists, 5, 1, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_to_id, @comment, @rishrozp_doc_id, @rish_num, @conveyancing_area";


                        str_balans_id = ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "HdnObjectId")).Value;
                        int.TryParse(str_balans_id, out balans_id);
                        string IsExistingObject = ((HiddenField)Utils.FindControlRecursive(ConveyancingForm, "IsExistingObject")).Value;

                        parameters.Clear();
                        parameters.Add("balans_id", balans_id);
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("new_akt_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "ActDate")).Value);
                        parameters.Add("new_akt_num", ((ASPxTextEdit)Utils.FindControlRecursive(ConveyancingForm, "ActNumber")).Value);
                        parameters.Add("new_akt_summa", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActSum")).Value);
                        parameters.Add("new_akt_summa_zalishkova", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "ActResidualSum")).Value);
                        parameters.Add("rishrozp_name", ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "TextBoxRishName")).Value);
                        parameters.Add("rishrozp_date", ((ASPxDateEdit)Utils.FindControlRecursive(ConveyancingForm, "DateEditRishDate")).Value);
                        parameters.Add("rish_doc_kind_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRishRozpType")).Value);
                        parameters.Add("rishrozp_doc_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Value);
                        parameters.Add("rish_num", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboRozpDoc")).Text);
                        parameters.Add("obj_right_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ASPxComboBoxRight")).Value);
                        parameters.Add("comment", ((ASPxMemo)Utils.FindControlRecursive(ConveyancingForm, "Comment")).Text);
                        parameters.Add("conveyancing_area", ((ASPxSpinEdit)Utils.FindControlRecursive(ConveyancingForm, "AreaForTransfer")).Value);

                        if (IsExistingObject == "1") // объект существует
                            parameters.Add("is_object_exists", true); // для теста так, но нужно будет присваивать реальное значение
                        else
                            parameters.Add("is_object_exists", false);

                        parameters.Add("org_to_id", userOrgId);
                        //parameters.Add("org_to_id", ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboOrgTo")).Value);
                        break;
                }

                using (SqlCommand cmdInsert = new SqlCommand(
                    "INSERT INTO transfer_requests (" + fieldList + ") VALUES (" + paramList + "); SELECT SCOPE_IDENTITY()", 
                    connectionSql, 
                    transaction))
                {
                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        cmdInsert.Parameters.Add(new SqlParameter(param.Key, param.Value));
                    }

                    requestId = Convert.ToInt32(cmdInsert.ExecuteScalar());
                }

                transaction.Commit();

                SendBalansTransferNotifications(requestId);
            }
        }
    }

    protected void ComboOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] parts = e.Parameter.Split(new char[] { '|' });

        if (parts.Length == 2)
        {
            RenterOrgZkpoPattern = parts[0].Trim();
            RenterOrgNamePattern = parts[1].Trim().ToUpper();


            ASPxComboBox combobox = sender as ASPxComboBox;

            combobox.DataBind();

            if (combobox.Items.Count > 0)
                combobox.SelectedIndex = 0;
        }
    }

    protected void ComboOrg_ItemRequestedByValue(object sender, EventArgs e)
    {
        ASPxComboBox combobox = (ASPxComboBox)sender;

        combobox.DataBindItems();

        if (combobox.Items.Count > 0)
            combobox.SelectedIndex = 0;
    }

    protected void SqlDataSourceOrgFrom_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        log.InfoFormat("SqlDataSourceOrgFrom_Selecting: setting @org_id to {0}", userOrgId);
        e.Command.Parameters["@org_id"].Value = userOrgId;
    }

    protected void SqlDataSourceTransferRequest_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        log.InfoFormat("SqlDataSourceTransferRequest_Selecting: setting @req_id to {0}", requestId);
        e.Command.Parameters["@req_id"].Value = requestId;
    }

    protected void SqlDataSourceBalansOur_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        log.InfoFormat("SqlDataSourceBalansOur_Selecting: setting @org_id to {0}", userOrgId);
        e.Command.Parameters["@org_id"].Value = userOrgId;
    }

    protected void SqlDataSourceSearchOrgFrom_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string orgName = RenterOrgNamePattern;
        string zkpo = RenterOrgZkpoPattern;

        ASPxComboBox combobox = (ASPxComboBox)this.FindControlRecursive("ComboOrgFrom");
        if (combobox.Value != null)
            e.Command.Parameters["@org_id"].Value = combobox.Value;

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

    protected void SqlDataSourceSearchOrgTo_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string orgName = RenterOrgNamePattern;
        string zkpo = RenterOrgZkpoPattern;

        ASPxComboBox combobox = (ASPxComboBox)this.FindControlRecursive("ComboOrgTo");
        if (combobox.Value != null)
            e.Command.Parameters["@org_id"].Value = combobox.Value;

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

    protected void ComboRozpDoc_OnItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
    {
        log.InfoFormat("ComboRozpDoc_OnItemsRequestedByFilterCondition (e.Filter={0}, e.BeginIndex={1}, e.EndIndex={2})",
            e.Filter, e.BeginIndex, e.EndIndex);

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

    protected void ComboRozpDoc_OnItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
    {
        log.InfoFormat("ComboRozpDoc_OnItemRequestedByValue (e.Value={0})", e.Value);

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

    protected void ConveyancingForm_DataBound(object sender, EventArgs e)
    {
        if (ConveyancingForm.DataItemCount == 0)
        {
            log.Info("Switching ConveyancingForm to FormViewMode.Insert");
            ConveyancingForm.ChangeMode(FormViewMode.Insert);
        }
        else if (orgIdForConfirm != userOrgId)
        {
            log.Info("Switching ConveyancingForm to FormViewMode.Edit");
            ConveyancingForm.ChangeMode(FormViewMode.Edit);
        }
        else
        {
            log.Info("Switching ConveyancingForm to FormViewMode.ReadOnly");
            ConveyancingForm.ChangeMode(FormViewMode.ReadOnly);
        }
    }

    protected void ConveyancingForm_OnItemCreated(object sender, EventArgs e)
    {
        log.Info("ConveyancingForm_OnItemCreated");
    }

    protected void CreateRozpDoc(int requestId)
    {
        SqlConnection connection = Utils.ConnectToDatabase();
        if (connection != null)
        {
            string query = @"SELECT * FROM transfer_requests WHERE request_id = @request_id";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("request_id", requestId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var kind_id = reader["rish_doc_kind_id"];
                        var general_kind_id = 7; // закріплення майна
                        var doc_date = reader["rishrozp_date"];
                        var doc_num = reader["rish_num"];
                        var topic = reader["rishrozp_name"];

                        reader.Close();

                        query = @"INSERT INTO documents (id, kind_id, general_kind_id, doc_date, doc_num, topic, modified_by, modify_date)
                                  VALUES (@id, @kind_id, @general_kind_id, @doc_date, @doc_num, @topic, @modified_by, @modify_date)";


                        using (SqlCommand cmdInsert = new SqlCommand(query, connection))
                        {
                            cmdInsert.Parameters.Add("id", GetNewRozpDocID());
                            cmdInsert.Parameters.Add("kind_id", kind_id);
                            cmdInsert.Parameters.Add("general_kind_id", general_kind_id);
                            cmdInsert.Parameters.Add("doc_date", doc_date);
                            cmdInsert.Parameters.Add("doc_num", doc_num);
                            cmdInsert.Parameters.Add("topic", topic);
                            cmdInsert.Parameters.Add("modified_by", ""); // выяснить, кто тут должен быть 
                            cmdInsert.Parameters.Add("modify_date", DateTime.Now);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }

                }
            }
            connection.Close();
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

    protected void SendBalansTransferNotifications(int requestId)
    {
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string org_name = "";
            string akt_num = "";
            decimal area = 0m;
            string address = "";
            int req_id = 0;
            int org_id_for_confirm = -1;

            string query = @"SELECT req.request_id, req.org_id_for_confirm, req.new_akt_num, req.conveyancing_area,
                            CASE WHEN req.conveyancing_type IN (1,5) THEN orgt.full_name ELSE orgf.full_name END AS org_name,
                            CASE WHEN req.is_object_exists = 1 THEN vb.street_full_name ELSE bu.street_full_name END AS street_full_name,
                            CASE WHEN req.is_object_exists = 1 THEN vb.addr_nomer ELSE bu.addr_nomer END AS addr_nomer
                            FROM transfer_requests req 
                            LEFT JOIN view_balans_all vb ON req.balans_id = vb.balans_id
                            LEFT JOIN unverified_balans ub ON req.balans_id = ub.id
                            LEFT JOIN buildings bu ON ub.building_id = bu.id
                            LEFT JOIN organizations orgf ON req.org_from_id = orgf.id
                            LEFT JOIN organizations orgt ON req.org_to_id = orgt.id
                            WHERE request_id = @request_id";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("request_id", requestId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        org_name = (string)reader["org_name"];
                        akt_num = (string)reader["new_akt_num"];
                        area = (decimal)reader["conveyancing_area"];
                        address = (reader["street_full_name"] != DBNull.Value && reader["addr_nomer"] != DBNull.Value) ? (string)reader["street_full_name"] + ", " + (string)reader["addr_nomer"] : "";
                        req_id = (int)reader["request_id"];
                        org_id_for_confirm = reader["org_id_for_confirm"] is int ? (int)reader["org_id_for_confirm"] : -1;

                        //max_id = (int)reader["max_id"] + 1;
                    }
                    reader.Close();
                }
            }




            if (org_id_for_confirm > 0)
            {
                var msgTxt = @"Доброго дня. 

                               Організація {0} потребує вашого підтвердження або спростування інформації щодо акту № {1}, 
                               про передачу об'єкту за адресою {2} площею {3}. 
                               Перейдіть будь ласка за посиланням для уточнення вашої позиції щодо вищезгаданого обєкту. 
                               {4}";

                var link = Resources.Strings.GlobalWebSiteURL;

                string messageBody = string.Format(msgTxt, org_name, akt_num, address, area, link);

                string subject = "Повідомлення";

                query = @"SELECT u.UserName, m.Email from reports1nf_accounts a
                        LEFT JOIN aspnet_Users u
                        ON a.UserID = u.UserID
                        LEFT JOIN aspnet_Membership m
                        ON a.UserID = m.UserID
                        WHERE organization_id = @organization_id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("organization_id", org_id_for_confirm));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(1))
                            {
                                var email = (string)reader["Email"];
                                if (!string.IsNullOrEmpty(email))
                                    EmailNotifier.SendMailMessage(WebConfigurationManager.AppSettings["EmailFrom"], email, null, null, subject, messageBody);
                            }
                        }
                        reader.Close();
                    }

                }


            }
            connection.Close();
        }


    }

    protected void SqlDataSourceOrgSearch2_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string name = OrgSearchNamePattern;
        string zkpo = OrgSearchZkpoPattern;

        if (name.Length == 0 && zkpo.Length == 0)
        {
            e.Command.Parameters["@zkpo"].Value = "^";
            e.Command.Parameters["@fname"].Value = "^";
        }
        else
        {
            e.Command.Parameters["@zkpo"].Value = zkpo.Length > 0 ? "%" + zkpo + "%" : "%";
            e.Command.Parameters["@fname"].Value = name.Length > 0 ? "%" + name + "%" : "%";
        }
    }

    protected string OrgSearchZkpoPattern
    {
        set
        {
            Page.SetEditedCustomData("OrgSearchZkpoPattern", value);
        }

        get
        {
            return Page.GetEditedCustomData<string>("OrgSearchZkpoPattern") ?? string.Empty;
        }
    }

    protected string OrgSearchNamePattern
    {
        set
        {
            Page.SetEditedCustomData("OrgSearchNamePattern", value);
        }

        get
        {
            return Page.GetEditedCustomData<string>("OrgSearchNamePattern") ?? string.Empty;
        }
    }

    /*
    protected void ComboBalansOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] parts = e.Parameter.Split(new char[] { '|' });

        if (parts.Length == 2)
        {
            OrgSearchZkpoPattern = parts[0].Trim();
            OrgSearchNamePattern = parts[1].Trim().ToUpper();

            (sender as ASPxComboBox).DataBind();
        }
    }*/

    protected int AddressStreetID
    {
        get;
        set;
    }

    protected void SqlDataSourceDictBuildings_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@street_id"].Value = ((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBalansStreetNewObj")).Value;//AddressStreetID; 
    }

    protected void ComboAddressBuilding_Callback(object source, CallbackEventArgsBase e)
    {
        try
        {
            int streetId = int.Parse(e.Parameter);
            AddressStreetID = streetId;
            //((ASPxComboBox)Utils.FindControlRecursive(ConveyancingForm, "ComboBalansStreetNewObj")).Value = streetId;


            (source as ASPxComboBox).DataBind();
        }
        finally
        {
        }
    }

    protected void CPApply_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("approve:"))
        {
            AproveTransferRequest();
        }
        else if (e.Parameter.StartsWith("discard:"))
        {
            var comment = e.Parameter.Substring(8);
            DiscardTransferRequest(comment);
        }
    }



    protected void DiscardTransferRequest(string comment)
    {
        SqlConnection connection = Utils.ConnectToDatabase();
        if (connection != null)
        {
            string query = "UPDATE transfer_requests SET status = 2, comment = @comment WHERE request_id = @request_id";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add("request_id", requestId);
                cmd.Parameters.Add("comment", comment);
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }
        DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback("ConveyancingRequestsList.aspx");
    }

    protected void AproveTransferRequest()
    {
        try
        {
            BalansTransferUtils.AproveTransferRequest(User, requestId, true);
        }
        catch (Exception ex)
        {
            log.Info(ex.Message);
        }

        DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback("ConveyancingRequestsList.aspx");
    }

//    protected void CommitBalansObjectChanges(int requestId, bool notifyByEmail)
//    {
//        SqlConnection connectionSql = Utils.ConnectToDatabase();

//        if (connectionSql != null)
//        {
//            SqlTransaction transactionSql = connectionSql.BeginTransaction();
//            try
//            {
//                string query = @"SELECT req.*, doc.doc_num rozp_doc_num, doc.doc_date rozp_doc_date, doc.topic rozp_doc_name,ub.form_ownership_id,
//                                CASE WHEN is_object_exists = 1 THEN bal.sqr_total ELSE ub.sqr_total END AS sqr_total, 
//                                CASE WHEN is_object_exists = 1 THEN bal.building_id ELSE ub.building_id END AS building_id,
//                                CASE WHEN is_object_exists = 1 THEN bal.purpose_group_id ELSE ub.purpose_group_id END AS purpose_group_id,
//                                CASE WHEN is_object_exists = 1 THEN bal.purpose_id ELSE ub.purpose_id END AS purpose_id,
//                                CASE WHEN is_object_exists = 1 THEN bal.object_type_id ELSE ub.object_type_id END AS object_type_id,
//                                CASE WHEN is_object_exists = 1 THEN bal.object_kind_id ELSE ub.object_kind_id END AS object_kind_id
//                                FROM transfer_requests req 
//                                LEFT JOIN unverified_balans ub ON req.balans_id = ub.id
//                                LEFT JOIN balans bal ON req.balans_id = bal.id
//                                LEFT JOIN documents doc ON req.rishrozp_doc_id = doc.id
//                                WHERE request_id = @request_id";

//                using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
//                {
//                    cmd.Parameters.Add(new SqlParameter("request_id", requestId));

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            int conveyancing_type = (int)reader["conveyancing_type"];

//                            var balans_id = (int)reader["balans_id"];
//                            var building_id = (int)reader["building_id"];
//                            var is_object_exists = (bool)reader["is_object_exists"];
//                            var org_to_id = reader["org_to_id"] is int ? (int)reader["org_to_id"] : -1;
//                            var org_from_id = reader["org_from_id"] is int ? (int)reader["org_from_id"] : -1;
//                            var conveyancing_area = (decimal)reader["conveyancing_area"];
//                            var sqr_total = (decimal)reader["sqr_total"];
//                            var new_akt_num = (string)reader["new_akt_num"];
//                            var new_akt_date = (DateTime)reader["new_akt_date"];
//                            var new_akt_summa = (decimal)reader["new_akt_summa"];
//                            var new_akt_summa_zalishkova = (decimal)reader["new_akt_summa_zalishkova"];
//                            var rozp_doc_num = (string)reader["rozp_doc_num"];
//                            var rozp_doc_date = (DateTime)reader["rozp_doc_date"];
//                            var rishrozp_doc_id = (int)reader["rishrozp_doc_id"];
//                            var rish_doc_kind_id = (int)reader["rish_doc_kind_id"];
//                            var ownership_type_id = reader["obj_right_id"] is int ? (int)reader["obj_right_id"] : -1;
//                            var form_ownership_id = reader["form_ownership_id"] is int ? (int)reader["form_ownership_id"] : -1;
//                            var purpose_group_id = reader["purpose_group_id"] is int ? (int)reader["purpose_group_id"] : -1;
//                            var purpose_id = reader["purpose_id"] is int ? (int)reader["purpose_id"] : -1;
//                            var object_type_id = reader["object_type_id"] is int ? (int)reader["object_type_id"] : -1;
//                            var object_kind_id = reader["object_kind_id"] is int ? (int)reader["object_kind_id"] : -1;

//                            reader.Close();

//                            if (conveyancing_type == 1 || conveyancing_type == 2) // Прийняти об'єкт на баланс / Передати об'єкт з балансу
//                            {
//                                var balansTransfer = new BalansTransfer();
//                                balansTransfer.transferType = ObjectTransferType.Transfer;
//                                balansTransfer.objectId = building_id;
//                                balansTransfer.sqr = conveyancing_area;
//                                balansTransfer.organizationFromId = org_from_id;
//                                balansTransfer.organizationToId = org_to_id;
//                                balansTransfer.balansId = balans_id;

//                                var actObject = new ActObject();
//                                actObject.makeChangesIn1NF = true;
//                                //actObject.objectId = 22930; // FAKE!!!
//                                actObject.balansTransfers.Add(balansTransfer);

//                                var importedAct = new ImportedAct();
//                                importedAct.docTitle = "АКТ ПРИЙМАННЯ-ПЕРЕДАВАННЯ";
//                                importedAct.docDate = new_akt_date;
//                                importedAct.docNum = new_akt_num;
//                                importedAct.docSum = new_akt_summa;
//                                importedAct.docFinalSum = new_akt_summa_zalishkova;
//                                importedAct.masterDocDate = rozp_doc_date;
//                                importedAct.masterDocNum = rozp_doc_num;
//                                importedAct.actObjects.Add(actObject);

//                                var rish = new Document();
//                                rish.documentKind = rish_doc_kind_id;
//                                rish.documentDate = DateTime.Now;
//                                rish.documentNumber = rozp_doc_num;
//                                rish.documentDate = rozp_doc_date;
//                                rish.modify_by = User.Identity.Name;
//                                rish.ownership_type_id = ownership_type_id;

//                                if (GUKV.Conveyancing.DB.ExportAct(importedAct, rish))
//                                {
//                                    if (GUKV.Conveyancing.DB.TransferBalansObjects(connectionSql, transactionSql, importedAct, rish, notifyByEmail, 3))
//                                    {
//                                        foreach (ActObject act in importedAct.actObjects)
//                                        {
//                                            foreach (BalansTransfer bt in act.balansTransfers)
//                                            {
//                                                CreateAktNew(connectionSql, transactionSql, bt.balansId, bt.objectId, importedAct.docDate, importedAct.docNum, importedAct.docSum, importedAct.docFinalSum, rishrozp_doc_id);
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                            else if (conveyancing_type == 3 || conveyancing_type == 4) // Списати об'єкту з балансу шляхом зносу / Списати об'єкту з балансу шляхом приватизації
//                            {
//                                var balansTransfer = new BalansTransfer();
//                                balansTransfer.transferType = ObjectTransferType.Destroy;
//                                balansTransfer.objectId = building_id;
//                                balansTransfer.sqr = conveyancing_area;
//                                balansTransfer.organizationFromId = org_from_id;
//                                balansTransfer.organizationToId = org_to_id;
//                                balansTransfer.balansId = balans_id;

//                                var actObject = new ActObject();
//                                actObject.makeChangesIn1NF = true;
//                                //actObject.objectId = 22930; // FAKE!!!
//                                actObject.balansTransfers.Add(balansTransfer);

//                                var importedAct = new ImportedAct();
//                                importedAct.docTitle = "АКТ ПРИЙМАННЯ-ПЕРЕДАВАННЯ";
//                                importedAct.docDate = new_akt_date;
//                                importedAct.docNum = new_akt_num;
//                                importedAct.docSum = new_akt_summa;
//                                importedAct.docFinalSum = new_akt_summa_zalishkova;
//                                importedAct.masterDocDate = rozp_doc_date;
//                                importedAct.masterDocNum = rozp_doc_num;
//                                importedAct.actObjects.Add(actObject);

//                                var rish = new Document();
//                                rish.documentDate = DateTime.Now;
//                                rish.documentNumber = rozp_doc_num;
//                                rish.documentDate = rozp_doc_date;
//                                rish.modify_by = User.Identity.Name;
//                                rish.ownership_type_id = ownership_type_id;

//                                var vidch_type_id = 0;
//                                if (conveyancing_type == 3)
//                                    vidch_type_id = 2;
//                                if (conveyancing_type == 4)
//                                    vidch_type_id = 1;

//                                if (GUKV.Conveyancing.DB.ExportAct(importedAct, rish))
//                                {
//                                    if (GUKV.Conveyancing.DB.TransferBalansObjects(connectionSql, transactionSql, importedAct, rish, notifyByEmail, vidch_type_id))
//                                    {
//                                        foreach (ActObject act in importedAct.actObjects)
//                                        {
//                                            foreach (BalansTransfer bt in act.balansTransfers)
//                                            {
//                                                CreateAktNew(connectionSql, transactionSql, bt.balansId, bt.objectId, importedAct.docDate, importedAct.docNum, importedAct.docSum, importedAct.docFinalSum, rishrozp_doc_id);
//                                            }
//                                        }
//                                    }
//                                }

//                                //ConveyancingUtils.DestroyBalansObj(balans_id, org_from_id, building_id);
//                                //if (conveyancing_type == 3)
//                                ModifyTechCondition(connectionSql, transactionSql, balans_id, 3); // make this balans object 'ЗНЕСЕНИЙ'
//                            }
//                            else if (conveyancing_type == 5)
//                            {
//                                var balansTransfer = new BalansTransfer();
//                                balansTransfer.transferType = ObjectTransferType.Create;
//                                balansTransfer.objectId = building_id;
//                                balansTransfer.sqr = conveyancing_area;
//                                balansTransfer.organizationFromId = org_from_id;
//                                balansTransfer.organizationToId = org_to_id;
//                                balansTransfer.balansId = balans_id;
//                                balansTransfer.form_ownership_id = form_ownership_id;

//                                var actObject = new ActObject();
//                                actObject.makeChangesIn1NF = true;
//                                //actObject.objectId = 22930; // FAKE!!!
//                                actObject.balansTransfers.Add(balansTransfer);
//                                actObject.purposeGroupId = purpose_group_id;
//                                actObject.purposeId = purpose_id;
//                                actObject.objectTypeId = object_type_id;
//                                actObject.objectKindId = object_kind_id;
//                                actObject.objectId = building_id;
//                                actObject.objectFinalCost = new_akt_summa_zalishkova;
//                                actObject.objectBalansCost = new_akt_summa;

//                                var importedAct = new ImportedAct();
//                                importedAct.docTitle = "АКТ ПРИЙМАННЯ-ПЕРЕДАВАННЯ";
//                                importedAct.docDate = new_akt_date;
//                                importedAct.docNum = new_akt_num;
//                                importedAct.docSum = new_akt_summa;
//                                importedAct.docFinalSum = new_akt_summa_zalishkova;
//                                importedAct.masterDocDate = rozp_doc_date;
//                                importedAct.masterDocNum = rozp_doc_num;
//                                importedAct.actObjects.Add(actObject);

//                                var rish = new Document();
//                                rish.documentDate = DateTime.Now;
//                                rish.documentNumber = rozp_doc_num;
//                                rish.documentDate = rozp_doc_date;
//                                rish.modify_by = User.Identity.Name;
//                                rish.ownership_type_id = ownership_type_id;

//                                if (GUKV.Conveyancing.DB.ExportAct(importedAct, rish))
//                                {
//                                    if (GUKV.Conveyancing.DB.TransferBalansObjects(connectionSql, transactionSql, importedAct, rish, notifyByEmail, 0))
//                                    {
//                                        foreach (ActObject act in importedAct.actObjects)
//                                        {
//                                            foreach (BalansTransfer bt in act.balansTransfers)
//                                            {
//                                                CreateAktNew(connectionSql, transactionSql, bt.balansId, bt.objectId, importedAct.docDate, importedAct.docNum, importedAct.docSum, importedAct.docFinalSum, rishrozp_doc_id);
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }

//                transactionSql.Commit();
//                transactionSql.Dispose();
//                transactionSql = null;
//            }
//            catch (Exception ex)
//            {
//                log.Error("General failure", ex);

//                if (transactionSql != null)
//                {
//                    try { transactionSql.Rollback(); }
//                    catch { }
//                    transactionSql.Dispose();
//                }
//                throw;
//            }

//            if (connectionSql.State == ConnectionState.Open)
//                connectionSql.Close();

//        }


//        Utils.ReloadCacheForBalans();
//    }


//    protected void CreateAktNew(SqlConnection connectionSql, SqlTransaction transactionSql, int balansId, int buildingId, DateTime aktDate, string aktNum, decimal aktSum,
//        decimal aktSumFinal, int rozpDocId)
//    {
//        //SqlConnection connection = Utils.ConnectToDatabase();
//        if (connectionSql != null)
//        {
//            var query = @"INSERT INTO documents (id, kind_id, general_kind_id, doc_date, doc_num, topic, summa, summa_zalishkova, modified_by, modify_date) OUTPUT INSERTED.ID
//                                  VALUES (@id, @kind_id, @general_kind_id, @doc_date, @doc_num, @topic, @summa, @summa_zalishkova, @modified_by, @modify_date)";

//            Int32 slave_doc_id = 0;

//            using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
//            {
//                try
//                {
//                    cmdInsert.Parameters.Add("id", GetNewAktID(connectionSql, transactionSql));
//                    cmdInsert.Parameters.Add("kind_id", 3); // АКТ ПРИЙМАННЯ-ПЕРЕДАЧІ
//                    cmdInsert.Parameters.Add("general_kind_id", 7); // закріплення майна
//                    cmdInsert.Parameters.Add("doc_date", aktDate);
//                    cmdInsert.Parameters.Add("doc_num", aktNum);
//                    cmdInsert.Parameters.Add("topic", "АКТ ПРИЙМАННЯ-ПЕРЕДАВАННЯ"); // Андрей сказал так делать
//                    cmdInsert.Parameters.Add("summa", aktSum);
//                    cmdInsert.Parameters.Add("summa_zalishkova", aktSumFinal);
//                    cmdInsert.Parameters.Add("modified_by", ""); // выяснить, кто тут должен быть 
//                    cmdInsert.Parameters.Add("modify_date", DateTime.Now);
//                    slave_doc_id = (Int32)cmdInsert.ExecuteScalar();
//                }
//                catch (Exception ex)
//                {
//                    log.Info(ex.Message);
//                    throw;
//                }
//            }

//            query = @"INSERT INTO doc_dependencies (master_doc_id, slave_doc_id, depend_kind_id) VALUES (@master_doc_id, @slave_doc_id, @depend_kind_id)";

//            using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
//            {
//                try
//                {
//                    cmdInsert.Parameters.Add("master_doc_id", rozpDocId);
//                    cmdInsert.Parameters.Add("slave_doc_id", slave_doc_id);
//                    cmdInsert.Parameters.Add("depend_kind_id", 1); // 1 = ПІДПОРЯДКОВАНИЙ ДОКУМЕНТ
//                    cmdInsert.ExecuteNonQuery();
//                }
//                catch (Exception ex)
//                {
//                    log.Info(ex.Message);
//                    throw;
//                }
//            }

//            Int32 building_docs_id = 0;

//            query = @"INSERT INTO building_docs (building_id, document_id) OUTPUT INSERTED.ID VALUES (@building_id, @document_id)";
//            using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
//            {
//                try
//                {
//                    cmdInsert.Parameters.Add("building_id", buildingId);
//                    cmdInsert.Parameters.Add("document_id", rozpDocId);
//                    building_docs_id = (Int32)cmdInsert.ExecuteScalar();
//                }
//                catch (Exception ex)
//                {
//                    log.Info(ex.Message);
//                    throw;
//                }

//            }

//            query = @"INSERT INTO balans_docs (balans_id, building_id, building_docs_id) VALUES (@balans_id, @building_id, @building_docs_id)";
//            using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
//            {
//                try
//                {
//                    cmdInsert.Parameters.Add("balans_id", balansId);
//                    cmdInsert.Parameters.Add("building_id", buildingId);
//                    cmdInsert.Parameters.Add("building_docs_id", building_docs_id);
//                    cmdInsert.ExecuteNonQuery();
//                }
//                catch (Exception ex)
//                {
//                    log.Info(ex.Message + " " + balansId.ToString() + " " + buildingId.ToString() + " " + building_docs_id.ToString());
//                    throw;
//                }
//            }

//            //connection.Close();
//        }

//    }

    //protected void ModifyTechCondition(SqlConnection connectionSql, SqlTransaction transactionSql, int balansId, int techConditionId)
    //{
    //    if (connectionSql != null)
    //    {
    //        string query = "UPDATE balans SET tech_condition_id = @tech_condition_id WHERE id = @balans_id";
    //        using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
    //        {
    //            cmd.Parameters.Add("@balans_id", balansId);
    //            cmd.Parameters.Add("@tech_condition_id", techConditionId);
    //            cmd.ExecuteNonQuery();
    //        }
    //    }
    //}

    //protected int GetNewAktID(SqlConnection connectionSql, SqlTransaction transactionSql)
    //{
    //    var max_id = 0;

    //    if (connectionSql != null)
    //    {
    //        string query = @"SELECT Max(id) AS max_id FROM documents --WHERE id > 20000 AND id < 40000";

    //        using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
    //        {
    //            using (SqlDataReader reader = cmd.ExecuteReader())
    //            {
    //                if (reader.Read())
    //                {
    //                    max_id = (int)reader["max_id"] + 1;
    //                }
    //                reader.Close();
    //            }
    //        }
    //    }

    //    return max_id;
    //}

}