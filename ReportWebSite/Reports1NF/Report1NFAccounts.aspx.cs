using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using GUKV.Common;

public partial class Reports1NF_Report1NFAccounts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SectionMenu.Visible = Roles.IsUserInRole(Utils.Report1NFReviewerRole);

        PrimaryGridView.Columns[0].Visible = Roles.IsUserInRole(Utils.AdministratorRole);

        // The 'Notifications' page must be visible only to users that can receive some notifications
        if (Roles.IsUserInRole(Utils.DKVOrganizationControllerRole) ||
            Roles.IsUserInRole(Utils.DKVObjectControllerRole) ||
            Roles.IsUserInRole(Utils.DKVArendaControllerRole) ||
            Roles.IsUserInRole(Utils.DKVArendaPaymentsControllerRole))
        {
            SectionMenu.Items[2].Visible = true;
        }
        else
        {
            SectionMenu.Items[2].Visible = false;
        }
    }

    protected void GridViewAccounts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        if (e.Parameters.StartsWith("delete:"))
        {
            int accountId = int.Parse(e.Parameters.Substring(7));

            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // Get the user Id
                Guid userId = Guid.Empty;

                using (SqlCommand cmd = new SqlCommand("SELECT UserId FROM reports1nf_accounts WHERE id = @aid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("aid", accountId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                object val = reader.GetValue(0);

                                if (val is Guid)
                                    userId = (Guid)val;
                            }
                        }

                        reader.Close();
                    }
                }

                // Get the name of this user
                string userName = "";

                using (SqlCommand cmd = new SqlCommand("SELECT UserName FROM aspnet_Users WHERE UserId = @usrid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("usrid", userId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                                userName = reader.GetString(0);
                        }

                        reader.Close();
                    }
                }

                if (userName.Length > 0)
                {
                    try
                    {
                        // Delete the user from the 'reports1nf_accounts' table
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM reports1nf_accounts WHERE UserId = @usrid", connection))
                        {
                            cmd.Parameters.Add(new SqlParameter("usrid", userId));
                            cmd.ExecuteNonQuery();
                        }

                        Membership.DeleteUser(userName);
                    }
                    catch (Exception)
                    {
                    }
                }

                connection.Close();
            }
        }

        PrimaryGridView.DataBind();
    }

    private string BalansOrgZkpoPattern
    {
        get
        {
            object pattern = Session["Accounts_BalansOrgZkpoPattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["Accounts_BalansOrgZkpoPattern"] = value;
        }
    }

    private string BalansOrgNamePattern
    {
        get
        {
            object pattern = Session["Accounts_BalansOrgNamePattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["Accounts_BalansOrgNamePattern"] = value;
        }
    }

    protected void SqlDataSourceOrgSearchBalans_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        HandleOrgSearchSelectingEvent(e, BalansOrgNamePattern, BalansOrgZkpoPattern);
    }

    protected void HandleOrgSearchSelectingEvent(SqlDataSourceSelectingEventArgs e, string orgName, string zkpo)
    {
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

    protected void ComboBalansOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] parts = e.Parameter.Split(new char[] { '|' });

        if (parts.Length == 2)
        {
            BalansOrgZkpoPattern = parts[0].Trim();
            BalansOrgNamePattern = parts[1].Trim().ToUpper();

            (sender as ASPxComboBox).DataBind();
            (sender as ASPxComboBox).SelectedIndex = 0;

        }
    }

    protected void CPAddBalansAcc_Callback(object sender, CallbackEventArgsBase e)
    {
        string firstName = EditFirstNameOrg.Text.Trim();
        string lastName = EditLastNameOrg.Text.Trim();
        string userName = firstName + "." + lastName;
        string email = EditEmailOrg.Text.Trim();

        object orgId = ComboBalansOrg.Value;

        if (orgId is int)
        {
            int organizationId = (int)orgId;

            // Generate password for the new user
            string password = System.Web.Security.Membership.GeneratePassword(System.Web.Security.Membership.MinRequiredPasswordLength + 2, System.Web.Security.Membership.MinRequiredNonAlphanumericCharacters);

            System.Web.Security.MembershipUser user = System.Web.Security.Membership.CreateUser(userName, password, email);

            if (user != null)
            {
                // Add the user to the 'report submitter' role
                System.Web.Security.Roles.AddUserToRole(user.UserName, Utils.Report1NFSubmitterRole);

                // Relate the user to organization
                SqlConnection connection = Utils.ConnectToDatabase();

                if (connection != null)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO reports1nf_accounts (UserId, organization_id) VALUES (@usr, @org)", connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("usr", user.ProviderUserKey));
                        cmd.Parameters.Add(new SqlParameter("org", organizationId));

                        cmd.ExecuteNonQuery();
                    }

                    // Send a notification to the user
                    string messageBody = string.Format(Resources.Strings.Report1NFNewPasswordBody,
                        new object[] { firstName + " " + lastName, Resources.Strings.GlobalWebSiteURL, userName, password });

                    EmailNotifier.SendEmailToUser(connection, null, userName, WebConfigurationManager.AppSettings["EmailFrom"], Resources.Strings.Report1NFNewPasswordTitle, messageBody);

                    connection.Close();
                }
            }
        }
    }


    protected void CPAddMultipleBalansAcc_Callback(object sender, CallbackEventArgsBase e)
    {
        var csvData = ASPxMemoCSVData.Text;
        List<FutureAccount> ourFutureAccounts = GetFutureAccounts(csvData);

        // now we're going to fill orgId's
        SqlConnection connection = Utils.ConnectToDatabase();
        foreach (var fa in ourFutureAccounts)
        {
            try
            {
                string query = @"select id from organizations
                                 where zkpo_code = @zkpo_code";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("zkpo_code", fa.Edrpou));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fa.OrgId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        }
                        reader.Close();
                    }
                }   
            }
            catch (SqlException exp)
            {
                // write to log (later)
                throw new InvalidOperationException("Data could not be read", exp);
            }            
        }
        
        // register accounts
        var registeredCount = 0;
        var errors = "";

        foreach (var fa in ourFutureAccounts.Where(a => a.OrgId != 0))
        {
            string password = System.Web.Security.Membership.GeneratePassword(System.Web.Security.Membership.MinRequiredPasswordLength + 2, System.Web.Security.Membership.MinRequiredNonAlphanumericCharacters);

            try
            {
                System.Web.Security.MembershipUser user = System.Web.Security.Membership.CreateUser(fa.Pib, password, fa.Email);
                if (user != null)
                {
                    // Add the user to the 'report submitter' role
                    System.Web.Security.Roles.AddUserToRole(user.UserName, Utils.Report1NFSubmitterRole);

                    // Relate the user to organization

                    if (connection != null)
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO reports1nf_accounts (UserId, organization_id) VALUES (@usr, @org)", connection))
                        {
                            cmd.Parameters.Add(new SqlParameter("usr", user.ProviderUserKey));
                            cmd.Parameters.Add(new SqlParameter("org", fa.OrgId));

                            cmd.ExecuteNonQuery();
                        }

                        // Send a notification to the user
                        string messageBody = string.Format(Resources.Strings.Report1NFNewPasswordBody,
                            new object[] { fa.Pib, Resources.Strings.GlobalWebSiteURL, fa.Pib, password });

                        EmailNotifier.SendEmailToUser(connection, null,  fa.Pib, WebConfigurationManager.AppSettings["EmailFrom"], Resources.Strings.Report1NFNewPasswordTitle, messageBody);
                        registeredCount++;


                    }
                }
            }
            catch(Exception ex)
            {
                errors = errors + fa.Pib + ": " + ex.Message + "\r\n";
            }
        }
        
        if (connection != null) connection.Close();

        // show log
        
        string importLog = "Было импортировано: " + registeredCount.ToString() + " аккаунтов \r\n";
        importLog = importLog + "Не были импортированы:\r\n";
        foreach (var fa in ourFutureAccounts.Where(a => a.OrgId == 0))
        {
            importLog = importLog + fa.Pib + ": не найдена организация с таким ЕДРПОУ \r\n";
        }
        importLog = importLog + "Ошибки: \r\n" + errors;
        ASPxMemoLog.Text = importLog;


    }

    protected void CPAddRdaAcc_Callback(object sender, CallbackEventArgsBase e)
    {
        string firstName = EditFirstNameRda.Text.Trim();
        string lastName = EditLastNameRda.Text.Trim();
        string userName = firstName + "." + lastName;
        string email = EditEmailRda.Text.Trim();

        object orgId = ComboRdaOrg.Value;

        SqlConnection connection = Utils.ConnectToDatabase();

        if (orgId is int)
        {
            int organizationId = (int)orgId;
            int districtId = GetDistrictIdByRdaId(connection, organizationId);

            // Generate password for the new user
            string password = System.Web.Security.Membership.GeneratePassword(10, 0);

            System.Web.Security.MembershipUser user = System.Web.Security.Membership.CreateUser(userName, password, email);

            if (user != null)
            {
                // Add the user to the 'report submitter' role
                System.Web.Security.Roles.AddUserToRole(user.UserName, Utils.RDAControllerRole);

                // Relate the user to organization
                

                if (connection != null)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO reports1nf_accounts (UserId, organization_id, rda_district_id) VALUES (@usr, @org, @rda)", connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("usr", user.ProviderUserKey));
                        cmd.Parameters.Add(new SqlParameter("org", organizationId));
                        cmd.Parameters.Add(new SqlParameter("rda", districtId));

                        cmd.ExecuteNonQuery();
                    }

                    // Send a notification to the user
                    string messageBody = string.Format(Resources.Strings.Report1NFRdaNewPasswordBody,
                        new object[] { firstName + " " + lastName, Resources.Strings.GlobalWebSiteURL, userName, password });

                    EmailNotifier.SendEmailToUser(connection, null, userName, WebConfigurationManager.AppSettings["EmailFrom"], Resources.Strings.Report1NFRdaNewPasswordTitle, messageBody);

                    connection.Close();
                }
            }
        }
    }

    protected int GetDistrictIdByRdaId(SqlConnection connection, int organizationRdaId)
    {
        int[] allid = new int[]
            { 8515,8400,1728,9826,15130,141824,137676,8566,308543,99405315 };

//pgv        if (!allid.Contains(organizationRdaId))
//            throw new ArgumentException("Unexpected RDA organization ID");

        //switch (organizationRdaId)
        //{
        //    case 8515: return 361;
        //    case 8400: return 363;
        //    case 1728: return 364;
        //    case 8109: return 366;
        //    case 9826: return 380;
        //    case 15130: return 382;
        //    case 141824: return 385;
        //    case 137676: return 386;
        //    case 8566: return 389;
        //    case 13128: return 391;
        //}
        int res = 0;
        using (SqlCommand cmd = new SqlCommand("select addr_distr_new_id from organizations where id = @orgid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("orgid", organizationRdaId));
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        res = reader.GetInt32(0);
                }
                reader.Close();
            }
        }

        if (res == 0)
            throw new ArgumentException("У вибраної РДА відсутній код addr_distr_new_id");

        return res;
    }

    public List<FutureAccount> GetFutureAccounts(string csvContent)
    {
        List<FutureAccount> futureAccounts = new List<FutureAccount>();
        CSVHelper csv = new CSVHelper(csvContent);
        foreach (string[] line in csv)
        {
            FutureAccount futureAccount = new FutureAccount();
            futureAccount.Pib = line[0];
            futureAccount.Edrpou = line[1];
            futureAccount.Email = line[2];
            futureAccounts.Add(futureAccount);
        }
        return futureAccounts;
    }
}

public class FutureAccount
{
    public string Pib { get; set; }
    public string Edrpou { get; set; }
    public string Email { get; set; }
    public int OrgId { get; set; }
}



public class CSVHelper : List<string[]>
{
    protected string csv = string.Empty;
    protected char separator = ';';

    public CSVHelper(string csv, char separator = ';')
    {
        this.csv = csv;
        this.separator = separator;

        if (!csv.Contains(separator))
        {
            separator = '\t';
        }

        foreach (string line in csv.Split('\n').ToList().Where(s => !string.IsNullOrEmpty(s)))
        {
            string[] values = line.Split(separator);

            for (int i = 0; i < values.Length; i++)
            {
                //Trim values
                values[i] = values[i].Trim('\"');
            }

            this.Add(values);
        }
    }
}