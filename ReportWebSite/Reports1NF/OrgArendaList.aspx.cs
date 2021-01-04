using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Data;
using FirebirdSql.Data.FirebirdClient;

public partial class Reports1NF_OrgArendaList : System.Web.UI.Page
{
    private class GridCallbackData
    {
        public string AgreementNum { get; set; }
        public int AgreementDateYear { get; set; }
        public int AgreementDateMonth { get; set; }
        public int AgreementDateDay { get; set; }
        public int BuildingID { get; set; }
        public int OrgBalansID { get; set; }
        public int OrgRenterID { get; set; }
        public int OrgGiverID { get; set; }
        public string OrgGiverComment { get; set; }

        public int AgreementToDeleteID { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];

        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);

            // Set the parameters for SQL queries
            SqlDataSourceReportArenda.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
            SqlDataSourceArendaStatus.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
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

            // Save the user organization ID
            if (ViewState["OrgArendaList_UserOrgID"] == null)
            {
                ViewState["OrgArendaList_UserOrgID"] = reportOrganizationId;
            }

            if (Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
            {
                if (Utils.UserOrganizationID != reportOrganizationId)
                {
                    Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
                }
            }
            else if (Roles.IsUserInRole(Utils.RDAControllerRole))
            {
                if (Utils.RdaDistrictID != reportRdaDistrictId)
                {
                    Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
                }
            }
        }
        else
        {
            Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
        }

        if (!Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
        {
            if (SectionMenu.Items.Count > 6)
                SectionMenu.Items[6].ClientVisible = false;

            ButtonAddAgreement.ClientVisible = false;
            ButtonSendAll.ClientVisible = false;
            CPStatus.ClientVisible = false;
        }

        // Bind data to the grid dynamically
        this.ProcessGridDataFetch(ViewState, PrimaryGridView);

        if (ReportID > 0)
        {
            if (!IsPostBack)
            {
                String ErrorMessage = String.Empty;

                //По кожному договору, що має статус «Договір діє» та “Вид оплати”  - «ГРОШОВА ОПЛАТА»,  «ПОГОДИННО»  повинен бути встановлений діючий Звітний період
                //По кожному договору, що має статус «Договір діє» повинен бути встановлений діючий Звітний період
                SqlConnection connection = Utils.ConnectToDatabase();
                string query = @"SELECT 
                dbo.efn_concat_string(ar.agreement_num, ', ', ar.agreement_num, 1) as [Договір]
                FROM reports1nf rep
                join reports1nf_arenda ar on rep.id  = ar.report_id
                LEFT JOIN arenda a ON a.id = ar.id
--                join dbo.organizations o on rep.organization_id = o.id
                join dbo.reports1nf_arenda_payments ap on ap.arenda_id = ar.id and ap.id = (select id from dbo.reports1nf_arenda_payments t1 where t1.arenda_id = ap.arenda_id and t1.report_id = ar.report_id and t1.rent_period_id = (select MAX(rent_period_id) from dbo.reports1nf_arenda_payments t2 where t2.arenda_id = t1.arenda_id and t2.report_id = t1.report_id ))
                left join [dbo].[dict_rent_period] p on ap.rent_period_id = p.id
                where ar.agreement_state = 1    
                and (ap.rent_period_id is null or ap.rent_period_id <> (select id from [dbo].[dict_rent_period]  where is_active = 1))
--              and ar.payment_type_id in (7,11)
                and isnull(ar.is_deleted, 0) = 0
                and isnull(a.is_deleted, 0) = 0
                and ar.report_id = @rep_id";

                String data = String.Empty;
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("rep_id", ReportID));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data = reader.GetString(0);
                        }

                        reader.Close();
                    }
                }
                if (data != String.Empty)
                {
//                    ErrorMessage = "По кожному договору, що має статус «Договір діє» та “Вид оплати”  - «ГРОШОВА ОПЛАТА», «ПОГОДИННО»  повинен бути встановлений діючий Звітний період: " + data;
                    ErrorMessage = "По кожному договору, що має статус «Договір діє» повинен бути встановлений діючий Звітний період: " + data;
                }
                // --По договору, що має статус «Договір діє» та «Вид оплати»  - «ГРОШОВА ОПЛАТА», «ПОГОДИННО», значення поля «Нараховано орендної плати за звітний період, грн. (без ПДВ)» повинно дорівнювати сумі значень полів  «- у тому числі, з нарахованої за звітний період (без боргів та переплат)» та  «Заборгованість по орендній платі, грн. - (без ПДВ): - за звітний період» та «Сальдо на початок року (незмінна впродовж року величина)
                if (ErrorMessage == String.Empty)
                {

                    connection = Utils.ConnectToDatabase();
                    query = @"SELECT 
                                dbo.efn_concat_string(ar.agreement_num, ', ', ar.agreement_num, 1) as [Договір]
                                FROM reports1nf_arenda ar
                                join dbo.reports1nf_arenda_payments ap on ap.arenda_id = ar.id and ap.id = (select id from dbo.reports1nf_arenda_payments t1 where t1.arenda_id = ap.arenda_id and t1.rent_period_id = (select MAX(rent_period_id) from dbo.reports1nf_arenda_payments t2 where t2.arenda_id = ap.arenda_id))
--                                join dbo.organizations o on ar.org_balans_id = o.id
                                left join [dbo].[dict_rent_period] p on ap.rent_period_id = p.id
                                LEFT JOIN arenda a ON a.id = ar.id
                                where ar.agreement_state = 1 
                                and ar.payment_type_id in (7, 11)
                                and isnull(ar.is_deleted, 0) = 0
                                and isnull(a.is_deleted, 0) = 0
                                and ap.payment_narah > ap.debt_zvit + ap.payment_nar_zvit + ap.last_year_saldo
                                and ar.report_id = @rep_id";

                    data = String.Empty;
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("rep_id", ReportID));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data = reader.GetString(0);
                            }

                            reader.Close();
                        }
                    }
                    if (data != String.Empty)
                    {
                        ErrorMessage = "По договору, що має статус «Договір діє» та «Вид оплати»  - «ГРОШОВА ОПЛАТА», «ПОГОДИННО», значення поля «Нараховано орендної плати за звітний період, грн. (без ПДВ)» не повинно перевищувати суму значень полів «- у тому числі, з нарахованої за звітний період (без боргів та переплат)» та  «Заборгованість по орендній платі, грн. - (без ПДВ): - за звітний період» та «Сальдо на початок року (незмінна впродовж року величина): " + data;
                    }
                }
                // --По договору, що має статус «Договір діє», значення поля «Фактична дата закінчення договору» повинно бути порожнім; 
                // --По договору, що має статус «Договір закінчився», значення поля «Фактична дата закінчення договору» НЕ повинно бути порожнім

                if (ErrorMessage == String.Empty)
                {

                    connection = Utils.ConnectToDatabase();
                    query = @"SELECT 
                                dbo.efn_concat_string(ar.agreement_num, ', ', ar.agreement_num, 1) as [Договір]
                                FROM reports1nf_arenda ar
--                                join dbo.organizations o on ar.org_balans_id = o.id
                                LEFT JOIN arenda a ON a.id = ar.id
                                where (ar.agreement_state = 1 and ar.rent_actual_finish_date is not Null or ar.agreement_state = 3 and ar.rent_actual_finish_date is Null )
                                and isnull(ar.is_deleted, 0) = 0
                                and isnull(a.is_deleted, 0) = 0
                                and ar.report_id = @rep_id";

                    data = String.Empty;
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("rep_id", ReportID));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data = reader.GetString(0);
                            }

                            reader.Close();
                        }
                    }
                    if (data != String.Empty)
                    {
                        ErrorMessage = "По договору, що має статус «Договір діє», значення поля «Фактична дата закінчення договору» повинно бути порожнім; По договору, що має статус «Договір закінчився», значення поля «Фактична дата закінчення договору» НЕ повинно бути порожнім: " + data;
                    }
                }
                //По договору, що має статус «Договір діє», значення поля «Площа що використовується всього, кв. м» не повинно дорівнювати 0
                if (ErrorMessage == String.Empty)
                {

                    connection = Utils.ConnectToDatabase();
                    query = @"SELECT 
                                dbo.efn_concat_string(ar.agreement_num, ', ', ar.agreement_num, 1) as [Договір]
                                FROM reports1nf_arenda ar
                                 INNER JOIN reports1nf rep ON rep.id = ar.report_id
                                 LEFT JOIN arenda a ON a.id = ar.id
                                where ar.agreement_state = 1 
                                and isnull(ar.rent_square, 0) = 0 
                                and isnull(ar.is_deleted, 0) = 0 
                                and isnull(a.is_deleted, 0) = 0
                                and ar.report_id = @rep_id";

                    data = String.Empty;
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("rep_id", ReportID));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data = reader.GetString(0);
                            }

                            reader.Close();
                        }
                    }
                    if (data != String.Empty)
                    {
                        ErrorMessage = "По договору, що має статус «Договір діє», значення поля «Площа що використовується всього, кв. м» не повинно дорівнювати 0: " + data;
                    }
                }
                //По договору, що має статус «Договір діє», «Договір закінчився, але заборгованість не погашено» дата модифікації звіту повинна бути в межах поточного Звітного періоду + 20 днів 
                if (ErrorMessage == String.Empty)
                {

                    connection = Utils.ConnectToDatabase();
                    query = @"SELECT 
                                dbo.efn_concat_string(ar.agreement_num, ', ', ar.agreement_num, 1) as [Договір]
                                FROM reports1nf_arenda ar 
                                 LEFT JOIN arenda a ON a.id = ar.id
                                 join dbo.dict_rent_period per on per.is_active = 1
		where ar.agreement_state in (1, 2) 
--		and (ar.submit_date not between per.period_start and DATEADD(day, 31, per.period_end) or ar.modify_date > ar.submit_date)
		and (ar.modify_date not between per.period_start and DATEADD(day, 31, per.period_end) )
		and isnull(ar.is_deleted, 0) = 0 
		and isnull(a.is_deleted, 0) = 0
                                    and ar.report_id = @rep_id";

                    data = String.Empty;
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("rep_id", ReportID));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data = reader.GetString(0);
                            }

                            reader.Close();
                        }
                    }
                    if (data != String.Empty)
                    {
                        ErrorMessage = "Неактуалізовано інформацію про договори оренди протягом поточного Звітного періоду: " + data;
                    }
                }

                if (ErrorMessage != String.Empty)
                {
                     ASPxLabelState.Text = ErrorMessage;

                     ButtonSendAll_Doble.Visible = true;
                     ButtonSendAll.Visible = false;
                }
            }
        }
        PrimaryGridView.Settings.VerticalScrollBarMode = DevExpress.Web.ScrollBarMode.Visible;
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

    #region Address and organization pickers

    private int AddressStreetID
    {
        get
        {
            object streetId = Session["OrgArendaList_AddressStreetID"];
            return (streetId is int) ? (int)streetId : 0;
        }

        set
        {
            Session["OrgArendaList_AddressStreetID"] = value;
        }
    }

    protected void ComboBuilding_Callback(object source, CallbackEventArgsBase e)
    {
        try
        {
            int streetId = int.Parse(e.Parameter);

            AddressStreetID = streetId;

            (source as ASPxComboBox).DataBind();
        }
        finally
        {
        }
    }

    protected void SqlDataSourceDictBuildings_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@street_id"].Value = AddressStreetID;
    }

    private string BalansOrgZkpoPattern
    {
        get
        {
            object pattern = Session["OrgArendaList_BalansOrgZkpoPattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["OrgArendaList_BalansOrgZkpoPattern"] = value;
        }
    }

    private string BalansOrgNamePattern
    {
        get
        {
            object pattern = Session["OrgArendaList_BalansOrgNamePattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["OrgArendaList_BalansOrgNamePattern"] = value;
        }
    }

    private string GiverOrgZkpoPattern
    {
        get
        {
            object pattern = Session["OrgArendaList_GiverOrgZkpoPattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["OrgArendaList_GiverOrgZkpoPattern"] = value;
        }
    }

    private string GiverOrgNamePattern
    {
        get
        {
            object pattern = Session["OrgArendaList_GiverOrgNamePattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["OrgArendaList_GiverOrgNamePattern"] = value;
        }
    }

    private string RenterOrgZkpoPattern
    {
        get
        {
            object pattern = Session["OrgArendaList_RenterOrgZkpoPattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["OrgArendaList_RenterOrgZkpoPattern"] = value;
        }
    }

    private string RenterOrgNamePattern
    {
        get
        {
            object pattern = Session["OrgArendaList_RenterOrgNamePattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["OrgArendaList_RenterOrgNamePattern"] = value;
        }
    }

    protected void SqlDataSourceOrgSearchBalans_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        HandleOrgSearchSelectingEvent(e, BalansOrgNamePattern, BalansOrgZkpoPattern);
    }

    protected void SqlDataSourceOrgSearchGiver_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string orgName = GiverOrgNamePattern;
        string zkpo = GiverOrgZkpoPattern;

        if (orgName.Length == 0 && zkpo.Length == 0)
        {
            e.Command.Parameters["@zkpo"].Value = "^";
            e.Command.Parameters["@fname"].Value = "^";

            object userOrgId = ViewState["OrgArendaList_UserOrgID"];

            if (userOrgId is int)
            {
                e.Command.Parameters["@balans_org"].Value = userOrgId;
            }
            else
            {
                int reportRdaDistrictId = -1;
                e.Command.Parameters["@balans_org"].Value = Reports1NFUtils.GetReportOrganizationId(ReportID, ref reportRdaDistrictId);
            }
        }
        else
        {
            e.Command.Parameters["@zkpo"].Value = zkpo.Length > 0 ? "%" + zkpo + "%" : "%";
            e.Command.Parameters["@fname"].Value = orgName.Length > 0 ? "%" + orgName + "%" : "%";
            e.Command.Parameters["@balans_org"].Value = 0;
        }
    }

    protected void SqlDataSourceOrgSearchRenter_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        HandleOrgSearchSelectingEvent(e, RenterOrgNamePattern, RenterOrgZkpoPattern);
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

    protected void ComboGiverOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter == "set_default_giver")
        {
            // GUKV is the default renter
            if ((sender as ASPxComboBox).Value == null)
            {
                GiverOrgZkpoPattern = "";
                GiverOrgNamePattern = "";

                (sender as ASPxComboBox).DataBind();
                (sender as ASPxComboBox).Value = Utils.GUKVOrganizationID;
            }
        }
        else
        {
            string[] parts = e.Parameter.Split(new char[] { '|' });

            if (parts.Length == 2)
            {
                GiverOrgZkpoPattern = parts[0].Trim();
                GiverOrgNamePattern = parts[1].Trim().ToUpper();

                (sender as ASPxComboBox).DataBind();
                (sender as ASPxComboBox).SelectedIndex = 0;
            }
        }
    }

    protected void ComboRenterOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] parts = e.Parameter.Split(new char[] { '|' });

        if (parts.Length == 2)
        {
            RenterOrgZkpoPattern = parts[0].Trim();
            RenterOrgNamePattern = parts[1].Trim().ToUpper();

            (sender as ASPxComboBox).DataBind();
            (sender as ASPxComboBox).SelectedIndex = 0;
        }
    }

    #endregion (Address and organization pickers)

    protected void CPStatus_Callback(object sender, CallbackEventArgsBase e)
    {
        ArendaStatusForm.DataBind();
    }

    protected void PrimaryGridView_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.Data)
        {
            object deletedInReport = e.GetValue("is_deleted");
            object deletedInEis = e.GetValue("is_deleted_in_eis");

            int isDeletedInReport = deletedInReport is int ? (int)deletedInReport : 0;
            int isDeletedInEis = deletedInEis is int ? (int)deletedInEis : 0;

            if (isDeletedInReport > 0 && isDeletedInEis == 0)
            {
                e.Row.BackColor = System.Drawing.Color.LightGray;
            }
        }
    }

    protected void PrimaryGridView_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
    {
        if (e.CellType == GridViewTableCommandCellType.Data)
        {
            object deleted = PrimaryGridView.GetRowValues(e.VisibleIndex, "is_deleted");

            if (deleted is int)
            {
  /*              e.Visible = (int)deleted == 0 ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;  */
            }
            else
            {
                e.Visible = DevExpress.Utils.DefaultBoolean.True;
            }

            // Hide the "delete" button is user is not allowed to edit the report
            if (e.ButtonID == "btnDeleteAgreement")
            {
                if (!Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
            }   
        }   
    }

    protected void PrimaryGridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        if (e.Parameters.StartsWith("init:"))
        {
            // Fake callback; do nothing
        }
        else
        {
            //pgv            
            if (e.Parameters.StartsWith("columns:"))
            {
                Utils.ProcessDataGridSaveLayoutCallback(e.Parameters, PrimaryGridView, Utils.GridIDArenda_Agreements, "");
                return;
            }
            else
            {
                GridCallbackData data = e.Parameters.FromJSON<GridCallbackData>();

                if (data.AgreementToDeleteID != 0)
                {
                    // Mark the rent agreement as deleted
                    SqlConnection connection = Utils.ConnectToDatabase();

                    if (connection != null)
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda SET is_deleted = 1, modified_by = @usr, modify_date = @dt WHERE report_id = @repid AND id = @arid", connection))
                        {
                            cmd.Parameters.Add(new SqlParameter("repid", ReportID));
                            cmd.Parameters.Add(new SqlParameter("arid", data.AgreementToDeleteID));
                            cmd.Parameters.Add(new SqlParameter("usr", username));
                            cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));

                            cmd.ExecuteNonQuery();
                        }

                        connection.Close();
                    }
                }
                else
                {
                    DateTime agreementDate = new DateTime(data.AgreementDateYear, data.AgreementDateMonth + 1, data.AgreementDateDay);

                    SqlConnection connection = Utils.ConnectToDatabase();

                    if (connection != null)
                    {
                        // Copy the building information to report
                        //using (SqlCommand cmd = new SqlCommand("INSERT INTO reports1nf_buildings SELECT b.*, @repid AS 'report_id' FROM buildings b WHERE b.id = @bid", connection))
                        //{
                        //    cmd.Parameters.Add(new SqlParameter("repid", ReportID));
                        //    cmd.Parameters.Add(new SqlParameter("bid", data.BuildingID));

                        //    cmd.ExecuteNonQuery();
                        //}

                        // Get the generated building ID from the 'reports1nf_buildings' table
                        int uniqueBuildingId = Reports1NFUtils.GenerateUniqueBuilding(connection, data.BuildingID, ReportID);

                        if (uniqueBuildingId > 0)
                        {
                            // Get the available ID for the new rent agreement
                            int minExistingId = 0;

                            using (SqlCommand cmd = new SqlCommand("SELECT MIN(id) FROM reports1nf_arenda WHERE report_id = @repid", connection))
                            {
                                cmd.Parameters.Add(new SqlParameter("repid", ReportID));

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        if (!reader.IsDBNull(0))
                                            minExistingId = reader.GetInt32(0);
                                    }

                                    reader.Close();
                                }
                            }

                            int newAgreementId = (minExistingId < 0) ? minExistingId - 1 : -1;

                            // Create a new rent agreement
                            string query = @"INSERT INTO reports1nf_arenda
                            (id, report_id, building_id, org_balans_id, org_renter_id, org_giver_id, agreement_date, agreement_num,
                                building_1nf_unique_id, agreement_state, is_deleted, update_src_id, modified_by, modify_date)
                            VALUES
                            (@aid, @repid, @bid, @orgbal, @orgrent, @orggiv, @adt, @anum, @buid, 1, 0, 2, @usr, @dt)";

                            using (SqlCommand cmd = new SqlCommand(query, connection))
                            {
                                cmd.Parameters.Add(new SqlParameter("aid", newAgreementId));
                                cmd.Parameters.Add(new SqlParameter("repid", ReportID));
                                cmd.Parameters.Add(new SqlParameter("bid", data.BuildingID));
                                cmd.Parameters.Add(new SqlParameter("orgbal", data.OrgBalansID));
                                cmd.Parameters.Add(new SqlParameter("orgrent", data.OrgRenterID));
                                cmd.Parameters.Add(new SqlParameter("orggiv", data.OrgGiverID));
                                cmd.Parameters.Add(new SqlParameter("adt", agreementDate));
                                cmd.Parameters.Add(new SqlParameter("anum", data.AgreementNum));
                                cmd.Parameters.Add(new SqlParameter("buid", uniqueBuildingId));
                                cmd.Parameters.Add(new SqlParameter("usr", username));
                                cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));

                                cmd.ExecuteNonQuery();
                            }

                            // Create an entry for rent payment information
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO reports1nf_arenda_payments (report_id, arenda_id) VALUES (@repid, @aid)", connection))
                            {
                                cmd.Parameters.Add(new SqlParameter("aid", newAgreementId));
                                cmd.Parameters.Add(new SqlParameter("repid", ReportID));

                                cmd.ExecuteNonQuery();
                            }

                            // If comment is specified, add the default comment
                            if (data.OrgGiverID != Utils.GUKVOrganizationID && data.OrgGiverComment.Length > 0)
                            {
                                Reports1NFUtils.AddComment(connection, ReportID, data.OrgGiverComment,
                                    0, 0, 0, newAgreementId, 0, "EditGiverName", Resources.Strings.RentAgreementGiver, false, false);
                            }
                        }

                        connection.Close();
                    }
                }
            }

            // Rebind the grid of rent agreements
            PrimaryGridView.DataSourceID = "SqlDataSourceReportArenda";
            PrimaryGridView.DataBind();
        }
    }

    protected void PrimaryGridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "is_submitted")
        {
            if (e.CellValue is string && ((string)e.CellValue).Length < 3)
            {
                e.Cell.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void PrimaryGridView_CustomColumnSort(object sender,
        DevExpress.Web.CustomColumnSortEventArgs e)
    {
        Utils.ProcessGridSortByBuildingNumber(e);
    }

    protected void CPProgress_Callback(object sender, CallbackEventArgsBase e)
    {
        Reports1NFUtils.ProcessProgressPanelCallback(sender as ASPxCallbackPanel, e.Parameter, ProgressSendAll, LabelProgress,
            ReportID, new SendAllRentAgreementsWorkItem(), "ARENDA_WORK_ITEM_", Resources.Strings.ProcessingArendaObjects);
    }

    protected string LimitString(string str, int maxLength)
    {
        return (str.Length <= maxLength) ? str : str.Substring(0, maxLength);
    }

    protected void ASPxButton_ArendaObjects_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterArendaObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_ArendaObjects_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterArendaObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string);
    }


    protected void CPOrgEditor_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("create_org:"))
        {

            string errorMessage = "";

            string fullName = TextBoxFullNameOrg.Text.Trim();
            string zkpo = TextBoxZkpoCodeOrg.Text.Trim();
            string shortName = TextBoxShortNameOrg.Text.Trim();
            int district = ComboBoxDistrictOrg.Value is int ? (int)ComboBoxDistrictOrg.Value : -1;
            string street = TextBoxStreetNameOrg.Text.Trim();
            string numOfHouse = TextBoxAddrNomerOrg.Text.Trim();
            string zip = TextBoxAddrZipCodeOrg.Text.Trim();
            int status = ComboBoxStatusOrg.Value is int ? (int)ComboBoxStatusOrg.Value : -1;
            int formVlasn = ComboBoxFormVlasnOrg.Value is int ? (int)ComboBoxFormVlasnOrg.Value : -1;
            string directorFio = TextBoxDirectorFioOrg.Text.Trim();
            string directorPhone = TextBoxDirectorPhoneOrg.Text.Trim();
            string directorEmail = TextBoxDirectorEmailOrg.Text.Trim();


            if (fullName == "")
                errorMessage = "Необхідно заповнити повну назву організації.";
            else if (zkpo == "")
                errorMessage = "Необхідно заповнити Код ЄДРПОУ.";
            else if (shortName == "")
                errorMessage = "Необхідно заповнити коротку назву організації.";
            else if (district == -1)
                errorMessage = "Необхідно вибрати район.";
            else if (street == "")
                errorMessage = "Необхідно вибрати вулицю.";
            else if (street == "")
                errorMessage = "Необхідно вибрати вулицю.";
            else if (numOfHouse == "")
                errorMessage = "Необхідно заповнити номер будинку.";
            else if (zip == "")
                errorMessage = "Необхідно заповнити поштовий індекс.";
            else if (status == -1)
                errorMessage = "Необхідно вибрати статус.";
            else if (status == 1 && zkpo.Length != 8)
                errorMessage = "Код ЄДРПОУ має бути 8 символів у довжину.";
            else if (status == 2 && zkpo.Length != 10)
                errorMessage = "Код ЄДРПОУ має бути 10 символів у довжину.";
            else if (formVlasn == -1)
                errorMessage = "Необхідно вибрати форму власності.";
            else if (directorFio == "")
                errorMessage = "Необхідно заповнити ПІБ директора.";
            else if (directorPhone == "")
                errorMessage = "Необхідно заповнити телефон директора.";
            else if (directorEmail == "")
                errorMessage = "Необхідно заповнити Email директора.";

            //Does it have Organization with 
            if (zkpo != "")
            {
                string full_name = string.Empty;
                SqlConnection connection = Utils.ConnectToDatabase();
                if (connection != null)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT full_name FROM organizations WHERE zkpo_code = @zkpo", connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("zkpo", zkpo));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    full_name = reader.GetString(0);
                            }

                            reader.Close();
                        }
                    }
                    connection.Close();
                }
                if (full_name != String.Empty)
                    errorMessage = "Організація « " + zkpo.ToString()+ "»-«" + full_name + "» вже існує у базі даних. Повторне створення заборонено.";
            }


            if (errorMessage == "")
            {
                //FbConnection connection = Utils.ConnectTo1NF();

                //if (connection != null)
                //{


                    int newOrdId = RishProjectExport.CreateNew1NFOrganization(
                        //connection,
                        fullName,
                        TextBoxShortNameOrg.Text,

                        zkpo,
                        ComboBoxIndustryFrom.Value is int ? (int)ComboBoxIndustryFrom.Value : -1,
                        ComboBoxOccupationFrom.Value is int ? (int)ComboBoxOccupationFrom.Value : -1,
                        ComboBoxFormVlasnOrg.Value is int ? (int)ComboBoxFormVlasnOrg.Value : -1,
                        ComboBoxStatusOrg.Value is int ? (int)ComboBoxStatusOrg.Value : -1,
                        -1,
                        -1,
                        -1,
                        TextBoxDirectorFioOrg.Text,
                        TextBoxDirectorPhoneOrg.Text,
                        TextBoxDirectorEmailOrg.Text,
                        TextBoxBuhgalterFioFrom.Text,
                        TextBoxBuhgalterPhoneFrom.Text,
                        TextBoxFax.Text,
                        "", // kved
                        ComboBoxDistrictOrg.Value is int ? (int)ComboBoxDistrictOrg.Value : -1,
                        street,
                        TextBoxAddrNomerOrg.Text,
                        TextBoxAddrKorpusFrom.Text,
                        TextBoxAddrZipCodeOrg.Text,
                        //true,
                        out errorMessage);

                    //connection.Close();

                    if (newOrdId > 0)
                    {

                        EditRenterOrgZKPO.Value = zkpo;
                        RenterOrgZkpoPattern = zkpo;
                        EditRenterOrgName.Value = fullName;
                        RenterOrgNamePattern = fullName;
                        ComboRenterOrg.DataBind();
                        ComboRenterOrg.SelectedItem = ComboRenterOrg.Items.FindByValue(newOrdId);
                    }
                //}
            }
            LabelOrgCreationError.Text = errorMessage;
            LabelOrgCreationError.ClientVisible = (errorMessage.Length > 0);
        }
    }
}