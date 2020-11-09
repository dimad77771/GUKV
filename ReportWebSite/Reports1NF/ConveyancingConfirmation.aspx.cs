using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using System.Web.Security;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using GUKV.Conveyancing;
using DevExpress.Web;
using log4net;
using FirebirdSql.Data.FirebirdClient;
using System.Web.Configuration;
using GUKV.Common;

public partial class Reports1NF_ConveyancingConfirmation : System.Web.UI.Page
{
    protected int requestId = 0;

    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");

    protected void Page_Load(object sender, EventArgs e)
    {
        var strRequestId = Request.QueryString["reqid"];

        if (!string.IsNullOrEmpty(strRequestId))
            requestId = int.Parse(strRequestId);

        if (string.IsNullOrEmpty(strRequestId))
            Response.Redirect("ConveyancingList.aspx");
    }

    protected void CPApply_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("approve:"))
        {
            //SendNotifications(requestId);
            //return;

            //ConveyancingUtils.TestDB();
            AproveTransferRequest();
        }
        else if (e.Parameter.StartsWith("discard:"))
        {
            var comment = e.Parameter.Substring(8);
            DiscardTransferRequest(comment);
        }
    }

    protected void AproveTransferRequest()
    {

        CommitBalansObjectChanges(requestId);
        SqlConnection connection = Utils.ConnectToDatabase();
        if (connection != null)
        {
            string query = "UPDATE transfer_requests SET status = 3 WHERE request_id = @request_id";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add("@request_id", requestId);
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }


        //SendNotifications(requestId);
        DevExpress.Web.ASPxWebControl.RedirectOnCallback("ConveyancingList.aspx");
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
        DevExpress.Web.ASPxWebControl.RedirectOnCallback("ConveyancingList.aspx");
    }

    protected int BalansOwnershipTypeToObjRight(int balansOwnershipType)
    {
        return balansOwnershipType;
        if (balansOwnershipType == 11)
            return 2;
        else if (balansOwnershipType == 2)
            return 3;
        else if (balansOwnershipType == 4)
            return 6;
        else
            return -1;

    }

    protected void CommitBalansObjectChanges(int requestId)
    {
        SqlConnection connectionSql = Utils.ConnectToDatabase();
        SqlTransaction transactionSql = connectionSql.BeginTransaction();

        try
        {
            if (connectionSql != null)
            {
                string query = @"SELECT req.*, doc.doc_num rozp_doc_num, doc.doc_date rozp_doc_date, doc.topic rozp_doc_name,ub.form_ownership_id,
                                CASE WHEN is_object_exists = 1 THEN bal.sqr_total ELSE ub.sqr_total END AS sqr_total, 
                                CASE WHEN is_object_exists = 1 THEN bal.building_id ELSE ub.building_id END AS building_id,
                                CASE WHEN is_object_exists = 1 THEN bal.purpose_group_id ELSE ub.purpose_group_id END AS purpose_group_id,
                                CASE WHEN is_object_exists = 1 THEN bal.purpose_id ELSE ub.purpose_id END AS purpose_id,
                                CASE WHEN is_object_exists = 1 THEN bal.object_type_id ELSE ub.object_type_id END AS object_type_id,
                                CASE WHEN is_object_exists = 1 THEN bal.object_kind_id ELSE ub.object_kind_id END AS object_kind_id
                                FROM transfer_requests req 
                                LEFT JOIN unverified_balans ub ON req.balans_id = ub.id
                                LEFT JOIN balans bal ON req.balans_id = bal.id
                                LEFT JOIN documents doc ON req.rishrozp_doc_id = doc.id
                                WHERE request_id = @request_id";

                using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("request_id", requestId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int conveyancing_type = (int)reader["conveyancing_type"];

                            var balans_id = (int)reader["balans_id"];
                            var building_id = (int)reader["building_id"];
                            var is_object_exists = (bool)reader["is_object_exists"];
                            var org_to_id = reader["org_to_id"] is int ? (int)reader["org_to_id"] : -1;
                            var org_from_id = reader["org_from_id"] is int ? (int)reader["org_from_id"] : -1;
                            var conveyancing_area = (decimal)reader["conveyancing_area"];
                            var sqr_total = (decimal)reader["sqr_total"];
                            var new_akt_num = (string)reader["new_akt_num"];
                            var new_akt_date = (DateTime)reader["new_akt_date"];
                            var new_akt_summa = (decimal)reader["new_akt_summa"];
                            var new_akt_summa_zalishkova = (decimal)reader["new_akt_summa_zalishkova"];
                            var rozp_doc_num = (string)reader["rozp_doc_num"];
                            var rozp_doc_date = (DateTime)reader["rozp_doc_date"];
                            var rozp_doc_name = (string)reader["rozp_doc_name"];
                            var rishrozp_doc_id = (int)reader["rishrozp_doc_id"];
                            var rish_doc_kind_id = (int)reader["rish_doc_kind_id"];
                            var ownership_type_id = reader["obj_right_id"] is int ? (int)reader["obj_right_id"] : -1;
                            var form_ownership_id = reader["form_ownership_id"] is int ? (int)reader["form_ownership_id"] : -1;

                            var purpose_group_id = reader["purpose_group_id"] is int ? (int)reader["purpose_group_id"] : -1;
                            var purpose_id = reader["purpose_id"] is int ? (int)reader["purpose_id"] : -1;
                            var object_type_id = reader["object_type_id"] is int ? (int)reader["object_type_id"] : -1;
                            var object_kind_id = reader["object_kind_id"] is int ? (int)reader["object_kind_id"] : 0;

                            reader.Close();

                            if (conveyancing_type == 1 || conveyancing_type == 2) // Прийняти об'єкт на баланс / Передати об'єкт з балансу
                            {
                                var balansTransfer = new BalansTransfer();
                                balansTransfer.transferType = ObjectTransferType.Transfer;
                                balansTransfer.objectId = building_id;
                                balansTransfer.sqr = conveyancing_area;
                                balansTransfer.organizationFromId = org_from_id;
                                balansTransfer.organizationToId = org_to_id;
                                balansTransfer.balansId = balans_id;

                                var actObject = new ActObject();
                                actObject.makeChangesIn1NF = true;
                                actObject.balansTransfers.Add(balansTransfer);

                                var importedAct = new ImportedAct();
                                //importedAct.docTitle = "АКТ ПРИЙМАННЯ-ПЕРЕДАВАННЯ";
                                importedAct.docDate = new_akt_date;
                                importedAct.docNum = new_akt_num;
                                importedAct.docSum = new_akt_summa;
                                importedAct.docFinalSum = new_akt_summa_zalishkova;
                                importedAct.masterDocDate = rozp_doc_date;
                                importedAct.masterDocNum = rozp_doc_num;
                                importedAct.actObjects.Add(actObject);


                                var rish = new Document();
                                rish.documentDate = DateTime.Now;
                                rish.documentNumber = rozp_doc_num;
                                rish.documentTitle = rozp_doc_name;
                                rish.documentKind = rish_doc_kind_id;
                                rish.modify_by = User.Identity.Name;
                                rish.ownership_type_id = ownership_type_id;

                                if (GUKV.Conveyancing.DB.ExportAct(/*preferences,*/ importedAct, rish))
                                {
                                    AppendixObject appObj = new AppendixObject();

                                    Transfer transfer = new Transfer();
                                    transfer.organizationIdFrom = org_from_id;
                                    transfer.rightId = BalansOwnershipTypeToObjRight(ownership_type_id);
                                    transfer.organizationIdTo = org_to_id;
                                    appObj.objectId = building_id;
                                    appObj.transfers.Add(transfer);

                                    Appendix app = new Appendix();
                                    app.objects.Add(appObj);

                                    ImportedDoc doc = new ImportedDoc();
                                    doc.docNum = rozp_doc_num;
                                    doc.docTitle = rozp_doc_name;
                                    doc.docTypeId = 1;
                                    doc.docDate = DateTime.Now;
                                    doc.appendices.Add(app);
                                    //GUKV.Conveyancing.DB.ExportDocument(preferences, doc);

                                    if (GUKV.Conveyancing.DB.TransferBalansObjects(connectionSql, transactionSql, importedAct, rish, true, 0))
                                    {
                                        foreach (ActObject act in importedAct.actObjects)
                                        {
                                            foreach (BalansTransfer bt in act.balansTransfers)
                                            {
                                                //CreateAktNew(bt.balansId_1NF, bt.objectId_1NF, importedAct.docDate, importedAct.docNum, importedAct.docSum, importedAct.docFinalSum, rishrozp_doc_id);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (conveyancing_type == 3 || conveyancing_type == 4) // Списати об'єкту з балансу шляхом зносу / Списати об'єкту з балансу шляхом приватизації
                            {
                                var balansTransfer = new BalansTransfer();
                                balansTransfer.transferType = ObjectTransferType.Destroy;
                                balansTransfer.objectId = building_id;
                                balansTransfer.sqr = conveyancing_area;
                                balansTransfer.organizationFromId = org_from_id;
                                //balansTransfer.organizationToId_1NF = org_to_id;
                                balansTransfer.balansId = balans_id;

                                var actObject = new ActObject();
                                actObject.makeChangesIn1NF = true;
                                actObject.balansTransfers.Add(balansTransfer);

                                var importedAct = new ImportedAct();
                                //importedAct.docTitle = "АКТ ПРИЙМАННЯ-ПЕРЕДАВАННЯ";
                                importedAct.docDate = new_akt_date;
                                importedAct.docNum = new_akt_num;
                                importedAct.docSum = new_akt_summa;
                                importedAct.docFinalSum = new_akt_summa_zalishkova;
                                importedAct.masterDocDate = rozp_doc_date;
                                importedAct.masterDocNum = rozp_doc_num;
                                importedAct.actObjects.Add(actObject);


                                var rish = new Document();
                                rish.documentDate = DateTime.Now;
                                rish.documentNumber = rozp_doc_num;
                                rish.documentTitle = rozp_doc_name;
                                rish.documentDate = rozp_doc_date;
                                rish.documentKind = rish_doc_kind_id;
                                rish.modify_by = User.Identity.Name;
                                rish.ownership_type_id = ownership_type_id;

                                var vidch_type_id = 0;
                                if (conveyancing_type == 3)
                                    vidch_type_id = 2;
                                if (conveyancing_type == 4)
                                    vidch_type_id = 1;

                                if (GUKV.Conveyancing.DB.ExportAct(/*preferences,*/ importedAct, rish))
                                {
                                    if (GUKV.Conveyancing.DB.TransferBalansObjects(connectionSql, transactionSql, importedAct, rish, true, vidch_type_id))
                                    {
                                        foreach (ActObject act in importedAct.actObjects)
                                        {
                                            foreach (BalansTransfer bt in act.balansTransfers)
                                            {
                                                //CreateAktNew(bt.balansId_1NF, bt.objectId_1NF, importedAct.docDate, importedAct.docNum, importedAct.docSum, importedAct.docFinalSum, rishrozp_doc_id);
                                            }
                                        }
                                    }
                                }

                                //ConveyancingUtils.DestroyBalansObj(balans_id, org_from_id, building_id);
                                //if (conveyancing_type == 3)
                                ModifyTechCondition(connectionSql, transactionSql, balans_id, 3); // make this balans object 'ЗНЕСЕНИЙ'
                            }
                            else if (conveyancing_type == 5)
                            {
                                var balansTransfer = new BalansTransfer();
                                balansTransfer.transferType = ObjectTransferType.Create;
                                balansTransfer.objectId = building_id;
                                balansTransfer.sqr = conveyancing_area;
                                balansTransfer.organizationFromId = org_from_id;
                                balansTransfer.organizationToId = org_to_id;
                                balansTransfer.balansId = balans_id;
                                balansTransfer.form_ownership_id = form_ownership_id;

                                var actObject = new ActObject();
                                actObject.makeChangesIn1NF = true;
                                actObject.balansTransfers.Add(balansTransfer);
                                actObject.purposeGroupId = purpose_group_id;
                                actObject.purposeId = purpose_id;
                                actObject.objectTypeId = object_type_id;
                                actObject.objectKindId = object_kind_id;
                                actObject.objectId = building_id;

                                var importedAct = new ImportedAct();
                                //importedAct.docTitle = "АКТ ПРИЙМАННЯ-ПЕРЕДАВАННЯ";
                                importedAct.docDate = new_akt_date;
                                importedAct.docNum = new_akt_num;
                                importedAct.docSum = new_akt_summa;
                                importedAct.docFinalSum = new_akt_summa_zalishkova;
                                importedAct.masterDocDate = rozp_doc_date;
                                importedAct.masterDocNum = rozp_doc_num;
                                importedAct.actObjects.Add(actObject);


                                var rish = new Document();
                                rish.documentDate = DateTime.Now;
                                rish.documentNumber = rozp_doc_num;
                                rish.documentTitle = rozp_doc_name;
                                rish.documentDate = rozp_doc_date;
                                rish.documentKind = rish_doc_kind_id;
                                rish.modify_by = User.Identity.Name;
                                rish.ownership_type_id = ownership_type_id;

                                if (GUKV.Conveyancing.DB.ExportAct(/*preferences,*/ importedAct, rish))
                                {
                                    AppendixObject appObj = new AppendixObject();

                                    Transfer transfer = new Transfer();
                                    transfer.organizationIdFrom = org_from_id;
                                    transfer.rightId = BalansOwnershipTypeToObjRight(ownership_type_id);
                                    transfer.organizationIdTo = org_to_id;
                                    appObj.objectId = building_id;
                                    appObj.transfers.Add(transfer);

                                    Appendix app = new Appendix();
                                    app.objects.Add(appObj);

                                    ImportedDoc doc = new ImportedDoc();
                                    doc.docTypeId = 1;
                                    doc.docDate = DateTime.Now;
                                    doc.appendices.Add(app);
                                    //GUKV.Conveyancing.DB.ExportDocument(preferences, doc);

                                    if (GUKV.Conveyancing.DB.TransferBalansObjects(connectionSql, transactionSql, importedAct, rish, true, 0))
                                    {
                                        foreach (ActObject act in importedAct.actObjects)
                                        {
                                            foreach (BalansTransfer bt in act.balansTransfers)
                                            {
                                                //CreateAktNew(bt.balansId_1NF, bt.objectId_1NF, importedAct.docDate, importedAct.docNum, importedAct.docSum, importedAct.docFinalSum, rishrozp_doc_id);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //connectionSql.Close();
            }
            
            //transaction1NF.Commit();
            transactionSql.Commit();

            //connection1NF.Close();
            connectionSql.Close();
        }
        catch
        {
            //transaction1NF.Rollback();
            transactionSql.Rollback();
            throw;
        }
        Utils.ReloadCacheForBalans();
    }

    protected void SqlDataSourceTransferRequest_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@req_id"].Value = requestId;
    }

    protected void CreateAktNew(int balansId, int buildingId, DateTime aktDate, string aktNum, decimal aktSum,
        decimal aktSumFinal, int rozpDocId)
    {
        SqlConnection connection = Utils.ConnectToDatabase();
        if (connection != null)
        {
            var query = @"INSERT INTO documents (id, kind_id, general_kind_id, doc_date, doc_num, topic, summa, summa_zalishkova, modified_by, modify_date) OUTPUT INSERTED.ID
                                  VALUES (@id, @kind_id, @general_kind_id, @doc_date, @doc_num, @topic, @summa, @summa_zalishkova, @modified_by, @modify_date)";

            Int32 slave_doc_id = 0;

            using (SqlCommand cmdInsert = new SqlCommand(query, connection))
            {
                try
                {
                    cmdInsert.Parameters.Add("id", GetNewAktID());
                    cmdInsert.Parameters.Add("kind_id", 3); // АКТ ПРИЙМАННЯ-ПЕРЕДАЧІ
                    cmdInsert.Parameters.Add("general_kind_id", 7); // закріплення майна
                    cmdInsert.Parameters.Add("doc_date", aktDate);
                    cmdInsert.Parameters.Add("doc_num", aktNum);
                    cmdInsert.Parameters.Add("topic", "АКТ ПРИЙМАННЯ-ПЕРЕДАВАННЯ"); // Андрей сказал так делать
                    cmdInsert.Parameters.Add("summa", aktSum);
                    cmdInsert.Parameters.Add("summa_zalishkova", aktSumFinal);
                    cmdInsert.Parameters.Add("modified_by", User.Identity.Name); // выяснить, кто тут должен быть 
                    cmdInsert.Parameters.Add("modify_date", DateTime.Now);
                    slave_doc_id = (Int32)cmdInsert.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    log.Info(ex.Message);
                }
            }

            query = @"INSERT INTO doc_dependencies (master_doc_id, slave_doc_id, depend_kind_id) VALUES (@master_doc_id, @slave_doc_id, @depend_kind_id)";

            using (SqlCommand cmdInsert = new SqlCommand(query, connection))
            {
                try
                {
                    cmdInsert.Parameters.Add("master_doc_id", rozpDocId);
                    cmdInsert.Parameters.Add("slave_doc_id", slave_doc_id);
                    cmdInsert.Parameters.Add("depend_kind_id", 1); // 1 = ПІДПОРЯДКОВАНИЙ ДОКУМЕНТ
                    cmdInsert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Info(ex.Message);
                }
            }

            Int32 building_docs_id = 0;

            query = @"INSERT INTO building_docs (building_id, document_id) OUTPUT INSERTED.ID VALUES (@building_id, @document_id)";
            using (SqlCommand cmdInsert = new SqlCommand(query, connection))
            {
                try
                {
                    cmdInsert.Parameters.Add("building_id", buildingId);
                    cmdInsert.Parameters.Add("document_id", rozpDocId);
                    building_docs_id = (Int32)cmdInsert.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    log.Info(ex.Message);
                }

            }

            query = @"INSERT INTO balans_docs (balans_id, building_id, building_docs_id) VALUES (@balans_id, @building_id, @building_docs_id)";
            using (SqlCommand cmdInsert = new SqlCommand(query, connection))
            {
                try
                {
                    cmdInsert.Parameters.Add("balans_id", balansId);
                    cmdInsert.Parameters.Add("building_id", buildingId);
                    cmdInsert.Parameters.Add("building_docs_id", building_docs_id);
                    cmdInsert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Info(ex.Message + " " + balansId.ToString() + " " + buildingId.ToString() + " " + building_docs_id.ToString());
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
            string query = @"SELECT Max(id) AS max_id FROM documents WHERE /*id > 15000 AND id < 20000 */ id < 1000000";

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

    protected int GetNewAktID()
    {
        var max_id = 0;
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string query = @"SELECT Max(id) AS max_id FROM documents WHERE /* id > 20000 AND id < 40000 */ id < 1000000";

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


    internal static void SendNotifications(int requestId)
    {
        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            string org_name = "";
            string akt_num = "";
            string area = "";
            string address = "";
            string req_id = "";
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
                        area = reader["conveyancing_area"] is decimal ? ((decimal)reader["conveyancing_area"]).ToString() : "0";
                        address = (string)reader["street_full_name"] + ", " + (string)reader["addr_nomer"];
                        req_id = reader["request_id"] is int ? ((int)reader["request_id"]).ToString() : "";
                        org_id_for_confirm = reader["org_id_for_confirm"] is int ? (int)reader["org_id_for_confirm"] : -1;

                        //max_id = (int)reader["max_id"] + 1;
                    }
                    reader.Close();
                }
            }


            var msgTxt = @"Доброго дня. 
Організація {0} потребує вашого підтвердження або спростування інформації щодо акту № {1}, 
про передачу об'єкту за адресою {2} площею {3} м². 
Перейдіть будь ласка за посиланням для уточнення вашої позиції щодо вищезгаданого обєкту. 
                                
{4}";

            var link = @"http://185.151.105.59/gukv/ReportWebSite/Reports1NF/ConveyancingConfirmation.aspx?reqid=" + req_id;
            var messageBody = string.Format(msgTxt, org_name, akt_num, address, area, link);
            var subject = "Повідомлення";

            //var list = Roles.GetUsersInRole("ConveyancingConfirmation").Select(Membership.GetUser).ToList();

            if (org_id_for_confirm > 0)
            {
                query = @"SELECT u.UserName, m.Email from reports1nf_accounts a
                        LEFT JOIN aspnet_Users u
                        ON a.UserID = u.UserID
                        LEFT JOIN aspnet_Membership m
                        ON a.UserID = m.UserID
                        WHERE organization_id = @organization_id AND m.Email IS NOT NULL";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("organization_id", org_id_for_confirm));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var email = (string)reader["Email"];
                                EmailNotifier.SendMailMessage(WebConfigurationManager.AppSettings["EmailFrom"], email, null, null, subject, messageBody);
                            }
                            reader.Close();
                        }
                        else
                        {
                            var userlist = Roles.GetUsersInRole("ConveyancingConfirmation").Select(Membership.GetUser).ToList();
                            foreach (var user in userlist)
                            {
                                var email = user.Email;
                                if (email != null)
                                {
                                   EmailNotifier.SendMailMessage(WebConfigurationManager.AppSettings["EmailFrom"], email, null, null, subject, messageBody);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var userlist = Roles.GetUsersInRole("ConveyancingConfirmation").Select(Membership.GetUser).ToList();
                foreach (var user in userlist)
                {
                    var email = user.Email;
                    if (email != null)
                    {
                        EmailNotifier.SendMailMessage(WebConfigurationManager.AppSettings["EmailFrom"], email, null, null, subject, messageBody);
                    }
                }
            }

            connection.Close();
        }


    }

    protected void ModifyTechCondition(SqlConnection connectionSql, SqlTransaction transactionSql, int balansId, int techConditionId)
    {
        string query = "UPDATE balans SET tech_condition_id = @tech_condition_id WHERE id = @balans_id";
        using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
        {
            cmd.Parameters.Add("@balans_id", balansId);
            cmd.Parameters.Add("@tech_condition_id", techConditionId);
            cmd.ExecuteNonQuery();
        }
    }
}