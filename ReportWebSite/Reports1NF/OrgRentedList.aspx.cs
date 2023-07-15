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


public partial class Reports1NF_OrgRentedList : System.Web.UI.Page
{
    private class GridCallbackData
    {
        public string AgreementNum { get; set; }

        public int AgreementDateYear { get; set; }
        public int AgreementDateMonth { get; set; }
        public int AgreementDateDay { get; set; }

        public int AgreementStartYear { get; set; }
        public int AgreementStartMonth { get; set; }
        public int AgreementStartDay { get; set; }

        public int AgreementFinishYear { get; set; }
        public int AgreementFinishMonth { get; set; }
        public int AgreementFinishDay { get; set; }

        public int AgreementToDeleteID { get; set; }

        public int BuildingID { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];

        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);

            // Set the parameters for SQL queries
            SqlDataSourceReportRented.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
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

        if (!Roles.IsUserInRole(Utils.RDAControllerRole))
        {
            SectionMenu.Visible = (Utils.GetLastReportId() <= 0);
        }
	}

    protected int ReportID
    {
        get
        {
            object reportId = ViewState["REPORT_ID"];

            return (reportId is int) ? (int)reportId : 0;
        }

        set
        {
            ViewState["REPORT_ID"] = value;
        }
    }

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
                e.Visible = (int)deleted == 0 ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
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
            GridCallbackData data = e.Parameters.FromJSON<GridCallbackData>();

            if (data.AgreementToDeleteID != 0)
            {
                // Mark the rent agreement as deleted
                SqlConnection connection = Utils.ConnectToDatabase();

                if (connection != null)
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_rented SET is_deleted = 1, modified_by = @usr, modify_date = @dt WHERE report_id = @repid AND id = @arid", connection))
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
                DateTime agreementStart = new DateTime(data.AgreementStartYear, data.AgreementStartMonth + 1, data.AgreementStartDay);
                DateTime agreementFinish = new DateTime(data.AgreementFinishYear, data.AgreementFinishMonth + 1, data.AgreementFinishDay);

                SqlConnection connection = Utils.ConnectToDatabase();

                if (connection != null)
                {
                    // Get the ID of organization that submits the report
                    int organizationId = 0;

                    using (SqlCommand cmd = new SqlCommand("SELECT organization_id FROM reports1nf WHERE id = @repid", connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("repid", ReportID));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                organizationId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            }

                            reader.Close();
                        }
                    }

                    // Copy the building information to report
                    int uniqueBuildingId = Reports1NFUtils.GenerateUniqueBuilding(connection, data.BuildingID, ReportID);

                    if (uniqueBuildingId > 0)
                    {
                        // Create a new rent agreement
                        string query = @"INSERT INTO reports1nf_arenda_rented
                            (report_id, org_renter_id, building_id, building_1nf_unique_id, agreement_date, agreement_num, rent_start_date, rent_finish_date, is_deleted, modified_by, modify_date)
                            VALUES
                            (@repid, @oid, @bid, @buid, @adt, @anum, @dtstart, @dtend, 0, @usr, @dt)";

                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.Add(new SqlParameter("repid", ReportID));
                            cmd.Parameters.Add(new SqlParameter("oid", organizationId));
                            cmd.Parameters.Add(new SqlParameter("bid", data.BuildingID));
                            cmd.Parameters.Add(new SqlParameter("buid", uniqueBuildingId));
                            cmd.Parameters.Add(new SqlParameter("adt", agreementDate));
                            cmd.Parameters.Add(new SqlParameter("anum", data.AgreementNum));
                            cmd.Parameters.Add(new SqlParameter("dtstart", agreementStart));
                            cmd.Parameters.Add(new SqlParameter("dtend", agreementFinish));
                            cmd.Parameters.Add(new SqlParameter("usr", username));
                            cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));

                            cmd.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }
            }
        }

        // Rebind the grid of rent agreements
        PrimaryGridView.DataSourceID = "SqlDataSourceReportRented";
        PrimaryGridView.DataBind();
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
            ReportID, new SendAllRentedObjectsWorkItem(), "RENTED_OBJ_WORK_ITEM_", Resources.Strings.ProcessingRentedObjects);
    }

    protected void ASPxButton_RentedObjects_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterRentedObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string);
    }

    protected void ASPxButton_RentedObjects_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterRentedObjects, PrimaryGridView, "", ViewState["PrimaryGridView.DataSourceID"] as string);
    }
    #region Address picker

    private int AddressStreetID
    {
        get
        {
            object streetId = Session["OrgRentedList_AddressStreetID"];
            return (streetId is int) ? (int)streetId : 0;
        }

        set
        {
            Session["OrgRentedList_AddressStreetID"] = value;
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

    #endregion (Address picker)
}