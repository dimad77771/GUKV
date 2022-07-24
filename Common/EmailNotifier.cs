using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using log4net;
using System.Net.Mail;
using System.Web.Configuration;

namespace GUKV.Common
{
    /// <summary>
    /// Summary description for EmailNotifier
    /// </summary>
    public static class EmailNotifier
    {
        public static void NotifyBalansObjectRemoved(SqlConnection connectionSqlClient, SqlTransaction transactionSql, int balansId)
        {
            string query = @"SELECT org.id, org.full_name, bal.sqr_total, b.street_full_name, b.addr_nomer, dict_doc_kind.name, vidch_doc_num, vidch_doc_date
                FROM balans bal
                INNER JOIN buildings b ON b.id = bal.building_id
                INNER JOIN organizations org ON org.id = bal.organization_id
                LEFT OUTER JOIN dict_doc_kind ON dict_doc_kind.id = bal.vidch_doc_type
                WHERE bal.id = @balid";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient, transactionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("balid", balansId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            int organizationId = reader.GetInt32(0);
                            string orgName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            decimal square = reader.IsDBNull(2) ? 0m : reader.GetDecimal(2);
                            string street = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            string buildingNumber = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            string docName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            string docNumber = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            DateTime? docDate = reader.IsDBNull(7) ? null : (DateTime?)reader.GetDateTime(7);

                            string address = street.Length > 0 ? street + " " + buildingNumber : buildingNumber;
                            string date = docDate.HasValue ? docDate.Value.ToShortDateString() : "";

                            string messageBody = string.Format(
                                Properties.Resources.Report1NFObjectRemovedBody,
                                new object[] { docName, docNumber, date, orgName, square.ToString("F2"), address, Properties.Resources.GlobalWebSiteURL });

                            reader.Close();

                            SendEmailToOrganization(connectionSqlClient, transactionSql, organizationId, Properties.Settings.Default.SmtpReplyTo, Properties.Resources.Report1NFObjectRemovedTitle, messageBody);
                        }
                        else
                        {
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.Error("Exception while sending email notification about balans object removal: " + ex.Message);
                throw;
            }
        }

        public static void NotifyBalansObjectAdded(SqlConnection connectionSqlClient, SqlTransaction transactionSql, int balansId)
        {
            string query = @"SELECT org.id, org.full_name, bal.sqr_total, b.street_full_name, b.addr_nomer, dict_doc_kind.name, balans_doc_num, balans_doc_date
                FROM balans bal
                INNER JOIN buildings b ON b.id = bal.building_id
                INNER JOIN organizations org ON org.id = bal.organization_id
                LEFT OUTER JOIN dict_doc_kind ON dict_doc_kind.id = bal.balans_doc_type
                WHERE bal.id = @balid";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient, transactionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("balid", balansId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            int organizationId = reader.GetInt32(0);
                            string orgName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            decimal square = reader.IsDBNull(2) ? 0m : reader.GetDecimal(2);
                            string street = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            string buildingNumber = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            string docName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            string docNumber = reader.IsDBNull(6) ? "" : reader.GetString(6);
                            DateTime? docDate = reader.IsDBNull(7) ? null : (DateTime?)reader.GetDateTime(7);

                            string address = street.Length > 0 ? street + " " + buildingNumber : buildingNumber;
                            string date = docDate.HasValue ? docDate.Value.ToShortDateString() : "";

                            string messageBody = string.Format(Properties.Resources.Report1NFObjectAddedBody,
                                new object[] { docName, docNumber, date, orgName, square.ToString("F2"), address, Properties.Resources.GlobalWebSiteURL });

                            reader.Close();

                            SendEmailToOrganization(connectionSqlClient, transactionSql, organizationId, Properties.Settings.Default.SmtpReplyTo, Properties.Resources.Report1NFObjectAddedTitle, messageBody);
                        }
                        else
                        {
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                //logger.Error("Exception while sending email notification about balans object creation: " + ex.Message);
            }
        }

        public static void SendEmailToOrganization(SqlConnection connectionSqlClient, SqlTransaction transactionSql, int organizationId, string from, string subject, string body)
        {
            // Select all emails
            string query = @"SELECT mem.Email FROM reports1nf_accounts acc INNER JOIN aspnet_Membership mem ON mem.UserId = acc.UserId WHERE acc.organization_id = @org";

            using (SqlCommand cmd = new SqlCommand(query, connectionSqlClient, transactionSql))
            {
                cmd.Parameters.Add(new SqlParameter("org", organizationId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            string email = reader.GetString(0).Trim();

                            if (email.Length > 0 && email.Contains("@"))
                            {
                                try
                                {
                                    SendMailMessage(from, email, null, null, subject, body);
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                    //logger.Error("Exception while sending email notification: " + ex.Message);
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        public static void SendEmailToUser(SqlConnection connection, SqlTransaction transactionSql, string userName, string from, string subject, string body)
        {
            // Select email of particular user
            string query = @"SELECT mem.Email FROM aspnet_Membership mem
            INNER JOIN aspnet_Users usr ON mem.UserId = usr.UserId
            WHERE LTRIM(RTRIM(LOWER(usr.UserName))) = LTRIM(RTRIM(LOWER(@usrname)))";

            using (SqlCommand cmd = new SqlCommand(query, connection, transactionSql))
            {
                cmd.Parameters.Add(new SqlParameter("usrname", userName));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            string email = reader.GetString(0).Trim();

                            if (email.Length > 0)
                            {
                                try
                                {
                                    SendMailMessage(from, email, null, null, subject, body);
                                }
                                finally
                                {
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        public static void SendEmailToRole(SqlConnection connectionSql, SqlTransaction transactionSql, string roleName, string from, string subject, string body, int organizationFromId)
        {
			if (String.IsNullOrEmpty(from)) return;

            // Read all notification preferences for this sender organization
            Dictionary<Guid, int> settings = new Dictionary<Guid, int>();
            string query = "SELECT UserId, is_notify FROM user_notification_settings WHERE organization_id = @orgid";

            using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
            {
                cmd.Parameters.Add(new SqlParameter("orgid", organizationFromId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                        {
                            settings[reader.GetGuid(0)] = reader.GetInt32(1);
                        }
                    }

                    reader.Close();
                }
            }

            // Select all emails of users that belong to particular role
            query = @"SELECT mem.UserId, mem.Email FROM aspnet_Roles rol
            INNER JOIN aspnet_UsersInRoles uir ON uir.RoleId = rol.RoleId
            INNER JOIN aspnet_Membership mem ON mem.UserId = uir.UserId
            WHERE rol.RoleName = @rname";

            using (SqlCommand cmd = new SqlCommand(query, connectionSql, transactionSql))
            {
                cmd.Parameters.Add(new SqlParameter("rname", roleName));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                        {
                            Guid userId = reader.GetGuid(0);
                            string email = reader.GetString(1).Trim();

                            if (email.Length > 0)
                            {
                                // Check if notification is enabled for this user
                                int is_enabled = 1;

                                if (!settings.TryGetValue(userId, out is_enabled))
                                {
                                    // By default, user MUST receive the notification
                                    is_enabled = 1;
                                }

                                if (is_enabled == 1)
                                {
                                    try
                                    {
                                        SendMailMessage(from, email, null, null, subject, body);
                                    }
                                    finally
                                    {
                                    }
                                }
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Send email.
        /// Configuration must be in config file in section
        /// <configuration>
        ///   <system.net>
        ///     <mailSettings>
        ///       <smtp deliveryMethod="Network" from="support@itgukraine.com">
        ///         <network host="192.168.10.4" port="25" userName="GUM-KMDA\a" password="" />
        ///       </smtp>
        ///     </mailSettings>
        ///   </system.net>
        /// </configuration>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void SendMailMessage(string from, string to, string cc, string bcc, string subject, string body)
        {
            if (WebConfigurationManager.AppSettings["NoSendEmail"] == "1")
			{
                return;
            }
            if (string.IsNullOrEmpty(from))
            {
                return;
            }

            bool isTest = false;

            body = body.Replace("\r", "");
            body = body.Replace("\n", "<br/>");

            // Instantiate a new instance of MailMessage
            MailMessage mMailMessage = new MailMessage();
            mMailMessage.IsBodyHtml = true;

            // Set the sender address of the mail message
            mMailMessage.From = new MailAddress(from);
            // Set the recepient address of the mail message
            mMailMessage.To.Add(new MailAddress(to));
            //mMailMessage.To.Add(new MailAddress("ILazarieva@itgukraine.com"));
            mMailMessage.Bcc.Add(new MailAddress("pul@ukr.net"));

            // Check if the cc value is null or an empty value
            if ((cc != null) && (cc != string.Empty))
            {
                // Set the CC address of the mail message
                mMailMessage.CC.Add(new MailAddress(cc));
            }

            // Check if the bcc value is null or an empty string
            if ((bcc != null) && (bcc != string.Empty))
            {
                // Set the Bcc address of the mail message
                mMailMessage.Bcc.Add(new MailAddress(bcc));
            }

            // Set the subject of the mail message
            mMailMessage.Subject = subject;
            if (isTest)
                mMailMessage.Subject = "ТЕСТОВАЯ ВЕРСИЯ " + mMailMessage.Subject;
            // Set the body of the mail message
            
            mMailMessage.Body = body;
            if (isTest)
                mMailMessage.Body = "ТЕСТОВАЯ ВЕРСИЯ. ЕСЛИ ВЫ НЕ ЯВЛЯЕТЕСЬ ТЕСТИРОВЩИКОМ, ПРОСТО ИГНОРИРУЙТЕ ДАННОЕ ПИСЬМО<BR/>" + mMailMessage.Body;

            // Set the priority of the mail message to normal
            mMailMessage.Priority = MailPriority.Normal;

			System.Net.ServicePointManager.ServerCertificateValidationCallback = (x, y, z, t) => true;

			// Instantiate a new instance of SmtpClient
			SmtpClient mSmtpClient = new SmtpClient();

            // Send the mail message
            mSmtpClient.Send(mMailMessage);
        }

    }
}