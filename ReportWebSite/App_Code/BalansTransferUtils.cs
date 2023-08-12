using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Security.Principal;
using GUKV.Common;
using System.Web.Configuration;
using GUKV.Conveyancing;
using log4net;
using System.Text;
using System.Web.Security;

/// <summary>
/// Summary description for BalansTransfer
/// </summary>
public class BalansTransferUtils
{
    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");

    public static int SaveRequest(
        IPrincipal User, 
        int ConveyancingType_Value,
        int OrgFromId, 
        int ComboOrgTo_Value, 
        int ObjectId_Value, 
        decimal AreaForTransfer_Value, 
        DateTime ActDate_Value, 
        object ActNumber_Value, 
        decimal ActSum_Value, 
        decimal ActResidualSum_Value, 
        object TextBoxRishName_Value, 
        DateTime DateEditRishDate_Value, 
        object ComboRishRozpType_Value, 
        object ASPxComboBoxRight_Value, 
        object ComboRozpDoc_Value, 
        object ComboRozpDoc_Text)
    {
        object Comment_Text = "";
        object IsExistingObject_Value = true;
        int requestId = 0;

        using (SqlConnection connectionSql = Utils.ConnectToDatabase())
        {
            using (SqlTransaction transaction = connectionSql.BeginTransaction())
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                string fieldList = "", paramList = "";

                string str_balans_id = ""; int balans_id = -1, org_from_id = -1;

                //OrgFromId = Utils.GetUserOrganizationID(User.Identity.Name);

                switch (ConveyancingType_Value.ToString())
                {
                    case "1": // Прийняти об'єкт на баланс

                        fieldList = "created_by, create_date, conveyancing_type, status, org_to_id, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_from_id, org_id_for_confirm, balans_id, rishrozp_doc_id, rish_num, is_object_exists, conveyancing_area";
                        paramList = "@created_by, @create_date, 1, 1, @org_to_id, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_from_id, @org_id_for_confirm, @balans_id, @rishrozp_doc_id, @rish_num, @is_object_exists, @conveyancing_area";


                        org_from_id = int.Parse(OrgFromId.ToString());

                        str_balans_id = ObjectId_Value.ToString();
                        if (int.TryParse(str_balans_id, out balans_id))
                        {
                            var isCorrect = ConveyancingUtils.IsCorrectOwner(balans_id, org_from_id);
                        };

                        parameters.Clear();
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("org_to_id", OrgFromId);
                        parameters.Add("new_akt_date", ActDate_Value);
                        parameters.Add("new_akt_num", ActNumber_Value);
                        parameters.Add("new_akt_summa", ActSum_Value);
                        parameters.Add("new_akt_summa_zalishkova", ActResidualSum_Value);
                        parameters.Add("rishrozp_name", TextBoxRishName_Value);
                        parameters.Add("rishrozp_date", DateEditRishDate_Value);
                        parameters.Add("rish_doc_kind_id", ComboRishRozpType_Value);
                        parameters.Add("obj_right_id", ASPxComboBoxRight_Value);
                        parameters.Add("balans_id", balans_id);
                        parameters.Add("org_id_for_confirm", ComboOrgTo_Value);
                        parameters.Add("org_from_id", ComboOrgTo_Value);
                        parameters.Add("rishrozp_doc_id", ComboRozpDoc_Value);
                        parameters.Add("rish_num", ComboRozpDoc_Text);
                        parameters.Add("conveyancing_area", AreaForTransfer_Value);
                        parameters.Add("is_object_exists", true);
                        //parameters.Add("vidch_org_id", ComboOrgTo_Value);
                        break;
                    case "2": // Передати об'єкт з балансу

                        fieldList = "created_by, create_date, balans_id, conveyancing_type, status, org_to_id, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_from_id, org_id_for_confirm, comment, is_object_exists, conveyancing_area, rishrozp_doc_id, rish_num";
                        paramList = "@created_by, @create_date, @balans_id, 2, 1, @org_to_id, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_from_id, @org_id_for_confirm, @comment, @is_object_exists, @conveyancing_area, @rishrozp_doc_id, @rish_num";

                        str_balans_id = ObjectId_Value.ToString();
                        if (int.TryParse(str_balans_id, out balans_id))
                        {
                            //var isCorrect = ConveyancingUtils.IsCorrectOwner(balans_id, org_from_id);
                        };

                        parameters.Clear();
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("org_to_id", ComboOrgTo_Value);
                        parameters.Add("new_akt_date", ActDate_Value);
                        parameters.Add("new_akt_num", ActNumber_Value);
                        parameters.Add("new_akt_summa", ActSum_Value);
                        parameters.Add("new_akt_summa_zalishkova", ActResidualSum_Value);
                        parameters.Add("rishrozp_name", TextBoxRishName_Value);
                        parameters.Add("rishrozp_date", DateEditRishDate_Value);
                        parameters.Add("rish_doc_kind_id", ComboRishRozpType_Value);
                        parameters.Add("obj_right_id", ASPxComboBoxRight_Value);
                        parameters.Add("comment", Comment_Text);
                        parameters.Add("rishrozp_doc_id", ComboRozpDoc_Value);
                        parameters.Add("rish_num", ComboRozpDoc_Text);
                        parameters.Add("conveyancing_area", AreaForTransfer_Value);
                        parameters.Add("is_object_exists", true);
                        parameters.Add("balans_id", balans_id);

                        parameters.Add("org_id_for_confirm", ComboOrgTo_Value);
                        parameters.Add("org_from_id", OrgFromId);
                        //parameters.Add("vidch_org_id", OrgFromId);

                        break;
                    case "3": // Списати об'єкту з балансу шляхом зносу
                        fieldList = "created_by, balans_id, create_date, conveyancing_type, status, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_from_id, comment, conveyancing_area, is_object_exists, rishrozp_doc_id, rish_num";
                        paramList = "@created_by, @balans_id, @create_date, 3, 1, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_from_id, @comment, @conveyancing_area, @is_object_exists, @rishrozp_doc_id, @rish_num";

                        str_balans_id = ObjectId_Value.ToString();
                        if (int.TryParse(str_balans_id, out balans_id))
                        {
                            var isCorrect = ConveyancingUtils.IsCorrectOwner(balans_id, org_from_id);
                        };

                        parameters.Clear();
                        parameters.Add("balans_id", balans_id);
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("new_akt_date", ActDate_Value);
                        parameters.Add("new_akt_num", ActNumber_Value);
                        parameters.Add("new_akt_summa", ActSum_Value);
                        parameters.Add("new_akt_summa_zalishkova", ActResidualSum_Value);
                        parameters.Add("rishrozp_name", TextBoxRishName_Value);
                        parameters.Add("rishrozp_date", DateEditRishDate_Value);
                        parameters.Add("rish_doc_kind_id", ComboRishRozpType_Value);
                        parameters.Add("rishrozp_doc_id", ComboRozpDoc_Value);
                        parameters.Add("rish_num", ComboRozpDoc_Text);
                        parameters.Add("obj_right_id", -1);
                        parameters.Add("comment", Comment_Text);
                        parameters.Add("conveyancing_area", AreaForTransfer_Value);
                        parameters.Add("org_from_id", OrgFromId);
                        parameters.Add("is_object_exists", true);

                        break;
                    case "4": // Списати об'єкту з балансу шляхом приватизації
                        fieldList = "created_by, balans_id, create_date, conveyancing_type, status, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_from_id, comment, rishrozp_doc_id, rish_num, conveyancing_area, is_object_exists, org_to_id, org_id_for_confirm";
                        paramList = "@created_by, @balans_id, @create_date, 4, 1, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_from_id, @comment, @rishrozp_doc_id, @rish_num, @conveyancing_area, @is_object_exists, @org_to_id, @org_id_for_confirm";

                        str_balans_id = ObjectId_Value.ToString();
                        if (int.TryParse(str_balans_id, out balans_id))
                        {
                            var isCorrect = ConveyancingUtils.IsCorrectOwner(balans_id, org_from_id);
                        };

                        parameters.Clear();
                        parameters.Add("balans_id", balans_id);
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("new_akt_date", ActDate_Value);
                        parameters.Add("new_akt_num", ActNumber_Value);
                        parameters.Add("new_akt_summa", ActSum_Value);
                        parameters.Add("new_akt_summa_zalishkova", ActResidualSum_Value);
                        parameters.Add("rishrozp_name", TextBoxRishName_Value);
                        parameters.Add("rishrozp_date", DateEditRishDate_Value);
                        parameters.Add("rish_doc_kind_id", ComboRishRozpType_Value);
                        parameters.Add("rishrozp_doc_id", ComboRozpDoc_Value);
                        parameters.Add("rish_num", ComboRozpDoc_Text);
                        parameters.Add("obj_right_id", -1);
                        parameters.Add("comment", Comment_Text);
                        parameters.Add("conveyancing_area", AreaForTransfer_Value);
                        parameters.Add("org_from_id", OrgFromId);
                        parameters.Add("is_object_exists", true);

                        parameters.Add("org_to_id", ComboOrgTo_Value);
                        parameters.Add("org_id_for_confirm", ComboOrgTo_Value);
                        break;
                    case "5": // Постановка  новозбудованого об'єкту на баланс
                        fieldList = "created_by, create_date, balans_id, is_object_exists, conveyancing_type, status, new_akt_date, new_akt_num, new_akt_summa, new_akt_summa_zalishkova, rishrozp_name, rishrozp_date, rish_doc_kind_id, obj_right_id, org_to_id, comment, rishrozp_doc_id, rish_num, conveyancing_area";
                        paramList = "@created_by, @create_date, @balans_id, @is_object_exists, 5, 1, @new_akt_date, @new_akt_num, @new_akt_summa, @new_akt_summa_zalishkova, @rishrozp_name, @rishrozp_date, @rish_doc_kind_id, @obj_right_id, @org_to_id, @comment, @rishrozp_doc_id, @rish_num, @conveyancing_area";


                        str_balans_id = ObjectId_Value.ToString();
                        int.TryParse(str_balans_id, out balans_id);
                        string IsExistingObject = IsExistingObject_Value.ToString();

                        parameters.Clear();
                        parameters.Add("balans_id", balans_id);
                        parameters.Add("created_by", User.Identity.Name);
                        parameters.Add("create_date", DateTime.Now);
                        parameters.Add("new_akt_date", ActDate_Value);
                        parameters.Add("new_akt_num", ActNumber_Value);
                        parameters.Add("new_akt_summa", ActSum_Value);
                        parameters.Add("new_akt_summa_zalishkova", ActResidualSum_Value);
                        parameters.Add("rishrozp_name", TextBoxRishName_Value);
                        parameters.Add("rishrozp_date", DateEditRishDate_Value);
                        parameters.Add("rish_doc_kind_id", ComboRishRozpType_Value);
                        parameters.Add("rishrozp_doc_id", ComboRozpDoc_Value);
                        parameters.Add("rish_num", ComboRozpDoc_Text);
                        parameters.Add("obj_right_id", ASPxComboBoxRight_Value);
                        parameters.Add("comment", Comment_Text);
                        parameters.Add("conveyancing_area", AreaForTransfer_Value);

                        if (IsExistingObject == "1") // объект существует
                            parameters.Add("is_object_exists", true); // для теста так, но нужно будет присваивать реальное значение
                        else
                            parameters.Add("is_object_exists", false);

                        //parameters.Add("org_to_id", OrgFromId);
                        parameters.Add("org_to_id", ComboOrgTo_Value);
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

                //SendBalansTransferNotifications();
            }
        }
        return requestId;
    }

    public static void SendBalansTransferNotifications(int ConveyancingType_Value, List<int> idList)
    {
        string msg = string.Empty;



        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            int org_id_for_confirm = -1;
            string org_name = "";
            string akt_num = "";

            StringBuilder objListText = new StringBuilder();
            objListText.Append("<table>");
            bool isFirst = true;
            foreach (int requestId in idList)
            {
                if (isFirst)
                {
                    objListText.AppendLine("<tr>Адреса<td></td><td>Площа</td>");
                    isFirst = false;
                }

                decimal area = 0m;
                string address = "";
                int req_id = 0;
                

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
                            
                            objListText.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", address, area);
                        }
                        reader.Close();
                    }
                }
            }
            objListText.Append("</table>");


            if (org_id_for_confirm > 0)
            {
                string messageBody = string.Empty;
                switch (ConveyancingType_Value)
                {

                    case 1:// Прийняти об'єкт на баланс
                        messageBody = String.Format("Доброго дня!<br/><br/>Повіломляємо, що балансоутримувач \"{0}\" прийняв на баланс наступний список обєктів:<br/>", org_name);
                        break;
                    case 2:// Передати об'єкт з балансу
                        messageBody = String.Format("Доброго дня!<br/><br/>Повіломляємо, що балансоутримувач \"{0}\" передав на баланс, наступний список обєктів:<br/>", org_name);
                        break;
                }
                messageBody += objListText.ToString();

                string subject = "Повідомлення";

                string queryAcc = @"SELECT u.UserName, m.Email from reports1nf_accounts a
                        LEFT JOIN aspnet_Users u
                        ON a.UserID = u.UserID
                        LEFT JOIN aspnet_Membership m
                        ON a.UserID = m.UserID
                        WHERE organization_id = @organization_id";

                using (SqlCommand cmd = new SqlCommand(queryAcc, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("organization_id", org_id_for_confirm));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(1))
                            {
                                var email = (string)reader["Email"];
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

    public static void AproveTransferRequest(IPrincipal User, int requestId, bool notifyByEmail)
    {
        try
        {
            CommitBalansObjectChanges(User, requestId, notifyByEmail);
            SqlConnection connection = Utils.ConnectToDatabase();
            if (connection != null)
            {
                string query = "UPDATE transfer_requests SET status = 3 WHERE request_id = @request_id";
                //string query = "DELETE transfer_requests WHERE request_id = @request_id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@request_id", requestId);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            log.Info(ex.Message);
            throw;
        }
    }

    public static void CommitBalansObjectChanges(IPrincipal User, int requestId, bool notifyByEmail)
    {
        SqlConnection connectionSql = Utils.ConnectToDatabase();

        if (connectionSql != null)
        {
            SqlTransaction transactionSql = connectionSql.BeginTransaction();
            try
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
                            var rishrozp_doc_id = (int)reader["rishrozp_doc_id"];
                            var rish_doc_kind_id = (int)reader["rish_doc_kind_id"];
                            var ownership_type_id = reader["obj_right_id"] is int ? (int)reader["obj_right_id"] : -1;
                            var form_ownership_id = reader["form_ownership_id"] is int ? (int)reader["form_ownership_id"] : -1;
                            var purpose_group_id = reader["purpose_group_id"] is int ? (int)reader["purpose_group_id"] : -1;
                            var purpose_id = reader["purpose_id"] is int ? (int)reader["purpose_id"] : -1;
                            var object_type_id = reader["object_type_id"] is int ? (int)reader["object_type_id"] : -1;
                            var object_kind_id = reader["object_kind_id"] is int ? (int)reader["object_kind_id"] : -1;

                            reader.Close();

                            ProcessTransfer(connectionSql, transactionSql, User, notifyByEmail, conveyancing_type, building_id, balans_id, org_from_id, org_to_id, conveyancing_area, new_akt_num, new_akt_date, new_akt_summa, new_akt_summa_zalishkova, rozp_doc_num, rozp_doc_date, rishrozp_doc_id, rish_doc_kind_id, ownership_type_id, form_ownership_id, purpose_group_id, purpose_id, object_type_id, object_kind_id, requestId);
                        }
                    }
                }

                transactionSql.Commit();
                transactionSql.Dispose();
                transactionSql = null;
            }
            catch (Exception ex)
            {
                log.Error("General failure", ex);

                if (transactionSql != null)
                {
                    try { transactionSql.Rollback(); }
                    catch { }
                    transactionSql.Dispose();
                }
                throw ex;
            }

            if (connectionSql.State == ConnectionState.Open)
                connectionSql.Close();

        }


        Utils.ReloadCacheForBalans();
    }

    public static void ProcessTransfer(SqlConnection connectionSql, SqlTransaction transactionSql, 
        IPrincipal User, bool notifyByEmail, int conveyancing_type, int building_id, int balans_id, 
        int org_from_id, int org_to_id, 
        decimal conveyancing_area, 
        string new_akt_num, DateTime new_akt_date, decimal new_akt_summa, decimal new_akt_summa_zalishkova, 
        string rozp_doc_num, DateTime rozp_doc_date, int rishrozp_doc_id, int rish_doc_kind_id, 
        int ownership_type_id, int form_ownership_id, int purpose_group_id, int purpose_id, 
        int object_type_id, int object_kind_id, int request_id)
    {
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
            //actObject.objectId = 22930; // FAKE!!!
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
            rish.documentKind = rish_doc_kind_id;
            rish.documentDate = DateTime.Now;
            rish.documentNumber = rozp_doc_num;
            rish.documentDate = rozp_doc_date;
            rish.modify_by = User.Identity.Name;
            rish.ownership_type_id = ownership_type_id;

            if (GUKV.Conveyancing.DB.ExportAct(importedAct, rish))
            {
                if (GUKV.Conveyancing.DB.TransferBalansObjects(connectionSql, transactionSql, importedAct, rish, notifyByEmail, 3, request_id: request_id))
                {
                    foreach (ActObject act in importedAct.actObjects)
                    {
                        foreach (BalansTransfer bt in act.balansTransfers)
                        {
                            CreateAktNew(connectionSql, transactionSql, bt.balansId, bt.objectId, importedAct.docDate, importedAct.docNum, importedAct.docSum, importedAct.docFinalSum, rishrozp_doc_id, request_id);
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
            balansTransfer.organizationToId = org_to_id;
            balansTransfer.balansId = balans_id;

            var actObject = new ActObject();
            actObject.makeChangesIn1NF = true;
            //actObject.objectId = 22930; // FAKE!!!
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
            rish.documentDate = rozp_doc_date;
            rish.modify_by = User.Identity.Name;
            rish.ownership_type_id = ownership_type_id;

            var vidch_type_id = 0;
            if (conveyancing_type == 3)
                vidch_type_id = 2;
            if (conveyancing_type == 4)
                vidch_type_id = 1;

            if (GUKV.Conveyancing.DB.ExportAct(importedAct, rish))
            {
                if (GUKV.Conveyancing.DB.TransferBalansObjects(connectionSql, transactionSql, importedAct, rish, notifyByEmail, vidch_type_id))
                {
                    foreach (ActObject act in importedAct.actObjects)
                    {
                        foreach (BalansTransfer bt in act.balansTransfers)
                        {
                            CreateAktNew(connectionSql, transactionSql, bt.balansId, bt.objectId, importedAct.docDate, importedAct.docNum, importedAct.docSum, importedAct.docFinalSum, rishrozp_doc_id);
                        }
                    }
                }
            }

            //ConveyancingUtils.DestroyBalansObj(balans_id, org_from_id, building_id);
            if (conveyancing_type == 3)
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
            //actObject.objectId = 22930; // FAKE!!!
            actObject.balansTransfers.Add(balansTransfer);
            actObject.purposeGroupId = purpose_group_id;
            actObject.purposeId = purpose_id;
            actObject.objectTypeId = object_type_id;
            actObject.objectKindId = object_kind_id;
            actObject.objectId = building_id;
            actObject.objectFinalCost = new_akt_summa_zalishkova;
            actObject.objectBalansCost = new_akt_summa;

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
            rish.documentDate = rozp_doc_date;
            rish.modify_by = User.Identity.Name;
            rish.ownership_type_id = ownership_type_id;

            if (GUKV.Conveyancing.DB.ExportAct(importedAct, rish))
            {
                if (GUKV.Conveyancing.DB.TransferBalansObjects(connectionSql, transactionSql, importedAct, rish, notifyByEmail, 0))
                {
                    foreach (ActObject act in importedAct.actObjects)
                    {
                        foreach (BalansTransfer bt in act.balansTransfers)
                        {
                            CreateAktNew(connectionSql, transactionSql, bt.balansId, bt.objectId, importedAct.docDate, importedAct.docNum, importedAct.docSum, importedAct.docFinalSum, rishrozp_doc_id);
                        }
                    }
                }
            }
        }
    }

    public static void AddObjectToAkt(SqlConnection connectionSql, SqlTransaction transactionSql, int balansId,
        int buildingId, int rozpDocId, int aktId)
    {
        Int32 building_docs_id = 0;

        if (buildingId > 0)
        {
            var query = @"INSERT INTO building_docs (building_id, document_id) OUTPUT INSERTED.ID VALUES (@building_id, @document_id)";
            using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
            {
                try
                {
                    cmdInsert.Parameters.Add("building_id", buildingId);
                    cmdInsert.Parameters.Add("document_id", rozpDocId);
                    building_docs_id = (Int32)cmdInsert.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Unable to insert into [building_docs] with paramters building_id={0}, document_id={1}", buildingId, rozpDocId), ex);
                    throw;
                }

            }
        }

        if (balansId > 0 && buildingId > 0 && building_docs_id > 0)
        {
            var query = @"INSERT INTO balans_docs (balans_id, building_id, building_docs_id) VALUES (@balans_id, @building_id, @building_docs_id)";
            using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
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
                    log.Error(string.Format("Unable to insert into [balans_docs] with parameters balans_id={0}, building_id={1}, building_docs_id={2}", balansId, buildingId, building_docs_id), ex);
                    throw;
                }
            }
        }
    }

    public static int CreateAktNew(SqlConnection connectionSql, SqlTransaction transactionSql, int balansId, 
        int buildingId, DateTime aktDate, string aktNum, decimal aktSum, decimal aktSumFinal, int rozpDocId, int? request_id = null)
    {
        if (connectionSql == null)
            return 0;

        var query = @"INSERT INTO documents (id, kind_id, general_kind_id, doc_date, doc_num, topic, summa, summa_zalishkova, modified_by, modify_date) OUTPUT INSERTED.ID
                                VALUES (@id, @kind_id, @general_kind_id, @doc_date, @doc_num, @topic, @summa, @summa_zalishkova, @modified_by, @modify_date)";

        int slave_doc_id = 0;

        using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
        {
            try
            {
                cmdInsert.Parameters.Add("id", GetNewAktID(connectionSql, transactionSql));
                cmdInsert.Parameters.Add("kind_id", 3); // АКТ ПРИЙМАННЯ-ПЕРЕДАЧІ
                cmdInsert.Parameters.Add("general_kind_id", 7); // закріплення майна
                cmdInsert.Parameters.Add("doc_date", aktDate);
                cmdInsert.Parameters.Add("doc_num", aktNum);
                cmdInsert.Parameters.Add("topic", "АКТ ПРИЙМАННЯ-ПЕРЕДАВАННЯ"); // Андрей сказал так делать
                cmdInsert.Parameters.Add("summa", aktSum);
                cmdInsert.Parameters.Add("summa_zalishkova", aktSumFinal);
                cmdInsert.Parameters.Add("modified_by", Membership.GetUser().UserName);
                cmdInsert.Parameters.Add("modify_date", DateTime.Now);
                slave_doc_id = (int)cmdInsert.ExecuteScalar();
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                throw;
            }
        }

        query = @"INSERT INTO doc_dependencies (master_doc_id, slave_doc_id, depend_kind_id) VALUES (@master_doc_id, @slave_doc_id, @depend_kind_id)";

        using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
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
                throw;
            }
        }

        Int32 building_docs_id = 0;

        if (buildingId > 0)
        {
            query = @"INSERT INTO building_docs (building_id, document_id) OUTPUT INSERTED.ID VALUES (@building_id, @document_id)";
            using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
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
                    throw;
                }

            }
        }

        if (balansId > 0 && buildingId > 0)
        {
            query = @"INSERT INTO balans_docs (balans_id, building_id, building_docs_id) VALUES (@balans_id, @building_id, @building_docs_id)";
            using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql, transactionSql))
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
                    throw;
                }
            }
        }

        return slave_doc_id;
    }

    protected static int GetNewAktID(SqlConnection connectionSql, SqlTransaction transactionSql)
    {
        var max_id = 0;

        if (connectionSql != null)
        {
            string query = @"SELECT Max(id) AS max_id FROM documents WHERE /* id > 20000 AND id < 40000 */  id < 1000000";

            using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
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
        }

        return max_id;
    }

    public static void ModifyTechCondition(SqlConnection connectionSql, SqlTransaction transactionSql, int balansId, int techConditionId)
    {
        if (connectionSql != null)
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

}