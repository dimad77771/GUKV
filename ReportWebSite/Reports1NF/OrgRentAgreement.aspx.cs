using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Data;
using System.Data;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using GUKV;
using System.Web.Configuration;
using System.IO;
using ExtDataEntry.Models;
using Syncfusion.Pdf;
using System.Drawing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.DocToPDFConverter;
using Syncfusion.DocIO.DLS;

public partial class Reports1NF_OrgRentAgreement : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");
    //private bool FormIsValid = true;
    //private static IList<string> ErrorMessages = new List<string>();
    //private static List<string> reqEditors = new List<string> { "EditAgreementNum", "EditAgreementDate", "EditStartDate", "ComboPaymentType" };
    //private string mode = ""; // save or send? 

    private RentingAgreementValidator validator;

    protected void Page_Load(object sender, EventArgs e)
    {
//////
        try
	{
        string reportIdStr = Request.QueryString["rid"];
        string agreementIdStr = Request.QueryString["aid"];
        string copyIdStr = Request.QueryString["copyid"];

        if (!string.IsNullOrEmpty(copyIdStr))
        {
            CopyCard(Int32.Parse(copyIdStr), Int32.Parse(reportIdStr));
        }

        GetPageUniqueKey();

        if (reportIdStr != null && reportIdStr.Length > 0 && agreementIdStr != null && agreementIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);

            if (RentAgreementID == 0)
                RentAgreementID = int.Parse(agreementIdStr);
        }

        // Check if report belongs to this user
        int reportRdaDistrictId = -1;
        int reportOrganizationId = Reports1NFUtils.GetReportOrganizationId(ReportID, ref reportRdaDistrictId);

        if (ReportID > 0 && !ReportBelongsToThisUser.HasValue)
        {
            int userOrg = Utils.UserOrganizationID; // Defined in Cabinet.aspx.cs

            if (userOrg > 0)
            {
                ReportBelongsToThisUser = (reportOrganizationId == userOrg);
            }
            else
            {
                ReportBelongsToThisUser = false;
            }

            // Save the user organization ID
            ViewState["OrgRentAgreement_UserOrgID"] = reportOrganizationId;
        }

        // Restrict access to card if report belongs to another organization
        if (ReportBelongsToThisUser.HasValue && !ReportBelongsToThisUser.Value)
        {
            if (Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
            {
                Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
            }
            else if (Roles.IsUserInRole(Utils.RDAControllerRole))
            {
                if (Utils.RdaDistrictID != reportRdaDistrictId)
                {
                    Response.Redirect(Page.ResolveClientUrl("~/Account/Restricted.aspx"));
                }
            }
        }

        // Restrict access to card if this rent agreement does not belong to the report
        if (!RentAgreementExistsInReport)
        {
            if (RentAgreementID < 0)
            {
                Response.Redirect(Page.ResolveClientUrl("~/Reports1NF/Cabinet.aspx?rid=" + ReportID.ToString()));
            }
            else
            {
                Response.Redirect(Page.ResolveClientUrl("~/Account/RestrictedRentAgreement.aspx"));
            }
        }

        if (ReportID > 0)
        {
            for (int i = 0; i < SectionMenu.Items.Count; i++)
            {
                if (SectionMenu.Items[i].NavigateUrl.IndexOf('?') < 0)
                    SectionMenu.Items[i].NavigateUrl += "?rid=" + ReportID.ToString();
            }
        }

        DataTable tableDecisions = DecisionsDataSource;
        GridViewDecisions.DataSource = tableDecisions;
        GridViewDecisions.DataBind();

		DataTable tableSubleases = SubleasesDataSource;
		GridViewSubleases.DataSource = tableSubleases;
		GridViewSubleases.DataBind();


		DataTable tableNotes = NotesDataSource;
        GridViewNotes.DataSource = tableNotes;
        GridViewNotes.DataBind();

        DataTable tablePaymentDocuments = PaymentDocumentsDataSource;
        GridViewPaymentDocuments.DataSource = tablePaymentDocuments;
        GridViewPaymentDocuments.DataBind();

        // Set the auto-calculated fields
        CalculateTotals();

        ReportCommentViewer1.ReportId = ReportID;
        ReportCommentViewer1.RentAgreementId = RentAgreementID;
        ReportCommentViewer1.AddNumberOfCommentsToButton(ButtonComments);


        ASPxComboBox periodCombo = ((ASPxComboBox)Utils.FindControlRecursive(PaymentForm, "ReportingPeriodCombo"));
        if (periodCombo != null)
        {
            object value = periodCombo.Value;
            if ((value == null) || ((value is int) && ((int)value <= 0)))
                periodCombo.Value = ActiveRentPeriodID;
        }

        // Enable / disable all controls depending on the report owner
        EnableControlsBasingOnUserRole();

        if (!Roles.IsUserInRole(Utils.Report1NFSubmitterRole))
        {
            ASPxButton btn = ((ASPxButton)Utils.FindControlRecursive(OrganizationsForm, "BtnCreateRenterOrg"));
            if (btn != null)
                btn.ClientVisible = false;
            btn = ((ASPxButton)Utils.FindControlRecursive(OrganizationsForm, "BtnEditRenterOrg"));
            if (btn != null)
                btn.ClientVisible = false;
        }

        validator = new RentingAgreementValidator(this, "MainGroup");

		PrepareTempPhotoFolder();

		if (!IsPostBack)
        {
            SqlConnection connection = Utils.ConnectToDatabase();
            string query = @"SELECT id, name FROM dict_rent_period where is_active = 1";

            string name = String.Empty;
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = reader.GetString(1);
                    }

                    reader.Close();
                }
            }
            ASPxLabel lbl = ((ASPxLabel)Utils.FindControlRecursive(PaymentForm, "NeededPeriodCombo"));
            lbl.Text = name;

            foreach (var col in ASPxGridViewFreeSquare.Columns)
            {
                var vcol = col as GridViewEditDataColumn;
                if (vcol != null)
                {
                    vcol.PropertiesEdit.ClientInstanceName = "felm__" + (!string.IsNullOrEmpty(vcol.FieldName) ? vcol.FieldName : vcol.Name);
                }
            }

        }

        if ((!IsCallback) && (reportIdStr != null && reportIdStr.Length > 0 && agreementIdStr != null && agreementIdStr.Length > 0))
        {
            SqlDataSourceFreeSquare.SelectParameters["arenda_id"].DefaultValue = agreementIdStr.ToString();
            SqlDataSourceFreeSquare.SelectParameters["report_id"].DefaultValue = ReportID.ToString();
            SqlDataSourceFreeSquare.SelectParameters["free_square_id"].DefaultValue = (EditFreeSquareMode ? ParamEditFreeSquareId : -1).ToString();

            SqlDataSourceArendaArchive.SelectParameters["arid"].DefaultValue = agreementIdStr.Trim();
        }

            //////
        }
        catch (Exception ex)
        {
            var lognet = log4net.LogManager.GetLogger("ReportWebSite");
            lognet.Debug("--------------- OrgRentAgreement page load ----------------", ex);
            throw ex;
        }
    }

    protected bool EditFreeSquareMode
    {
        get
        {
            return (ParamEditFreeSquareId != null);
        }
    }
    protected int? ParamEditFreeSquareId
    {
        get
        {
            var val = Request.QueryString["edit_free_square_id"];
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }
            else
            {
                return int.Parse(val);
            }
        }
    }



    protected void EnableControlsBasingOnUserRole()
    {
        bool userIsReportSubmitter = Roles.IsUserInRole(Utils.Report1NFSubmitterRole);
        bool reportBelongsToUser = ReportBelongsToThisUser.HasValue ? ReportBelongsToThisUser.Value : false;

        string strAid = Request.QueryString["aid"];
        int arendaId = int.Parse(strAid);

        Dictionary<string, string> markedControls = Reports1NFUtils.GetMarkedControlIDs(ReportID, null, null, arendaId, null, null);

        Reports1NFUtils.EnableDevExpressEditors(AddressForm, reportBelongsToUser && userIsReportSubmitter, markedControls);
        Reports1NFUtils.EnableDevExpressEditors(OrganizationsForm, reportBelongsToUser && userIsReportSubmitter, markedControls);
        Reports1NFUtils.EnableDevExpressEditors(PaymentForm, reportBelongsToUser && userIsReportSubmitter, markedControls);
        Reports1NFUtils.EnableDevExpressEditors(CollectionForm, reportBelongsToUser && userIsReportSubmitter, markedControls);
        Reports1NFUtils.EnableDevExpressEditors(InsuranceForm, reportBelongsToUser && userIsReportSubmitter, markedControls);

        GridViewDecisions.Enabled = reportBelongsToUser && userIsReportSubmitter;
		//!!!! GridViewSubleases.Enabled = reportBelongsToUser && userIsReportSubmitter;
		GridViewNotes.Enabled = reportBelongsToUser && userIsReportSubmitter;
        StatusForm.Visible = reportBelongsToUser && userIsReportSubmitter;

		ButtonSave.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
		//!!!! ButtonSend.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
		ButtonClear.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
        ButtonAddDecision.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
		//!!!! ButtonAddSublease.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
		ButtonAddNote.ClientVisible = reportBelongsToUser && userIsReportSubmitter;

        //BtnRecalcPaymentDocument.Visible = reportBelongsToUser && userIsReportSubmitter;

        IsReadOnlyForm = !(reportBelongsToUser && userIsReportSubmitter);

        // Enable or disable 'special payments' field
        /*
        Control panelAdditionalInfo = PaymentForm.FindControl("AdditionalInfoPanel");

        if (panelAdditionalInfo is ASPxRoundPanel)
        {
            Control checkIsSpecialOrganizatio = panelAdditionalInfo.FindControl("CheckIsSpecialOrganization");
            Control editPaymentSpecial = panelAdditionalInfo.FindControl("EditPaymentSpecial_orndpymnt");

            if (checkIsSpecialOrganizatio is ASPxCheckBox && editPaymentSpecial is ASPxSpinEdit)
            {
                (editPaymentSpecial as ASPxSpinEdit).ClientEnabled = (checkIsSpecialOrganizatio as ASPxCheckBox).Checked;
            }
        }
        */

        new[]
        { 
            //"edit_povidoleno1_date", "edit_povidoleno1_num",
            "edit_povidoleno2_date", "edit_povidoleno2_num",
            //"edit_povidoleno3_date", "edit_povidoleno3_num",
            "edit_povidoleno4_date", "edit_povidoleno4_num"
        }.ToList().ForEach(q => (PaymentForm.FindControlRecursive(q) as ASPxEdit).ReadOnly = false);

//        EnableCollectionControls();
        EnableInsuranceControls();
    }

    //bool a(Control q)
    //{
    //    
    //}

    protected bool IsReadOnlyForm { get; set; }

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

    protected int RentAgreementID
    {
        get
        {
            object agreementId = Session[GetPageUniqueKey() + "_RENT_AGREEMENT_ID"];

            if (agreementId is int)
            {
                return (int)agreementId;
            }

            return 0;
        }

        set
        {
            Session[GetPageUniqueKey() + "_RENT_AGREEMENT_ID"] = value;
        }
    }

    protected int ActiveRentPeriodID
    {
        get
        {
            var active_rent_period_id = 0;
            var connection = Utils.ConnectToDatabase();
            if (connection == null)
            {
                return 0;
            }
            string queryString = "select * from dict_rent_period where is_active = 1";
            var sqlCmd = new SqlCommand(queryString, connection);
            var reader = sqlCmd.ExecuteReader();
            if (reader.Read())
            {
                active_rent_period_id = Convert.ToInt32(reader["id"]);
            }
            connection.Close();
            return active_rent_period_id;
        }
    }

    protected bool? ReportBelongsToThisUser
    {
        get
        {
            object val = ViewState["REPORT_BELONGS_TO_USER"];

            if (val is bool)
            {
                return (bool)val;
            }

            return null;
        }

        set
        {
            ViewState["REPORT_BELONGS_TO_USER"] = value;
        }
    }

    protected bool RentAgreementExistsInReport
    {
        get
        {
            bool exists = false;
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 id FROM reports1nf_arenda WHERE id = @aid AND report_id = @rep_id", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
                    cmd.Parameters.Add(new SqlParameter("rep_id", ReportID));

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        exists = r.Read();
                        r.Close();
                    }
                }

                connection.Close();
            }

            return exists;
        }
    }

    protected void SqlDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@aid"].Value = RentAgreementID;
        e.Command.Parameters["@rep_id"].Value = ReportID;
    }

    protected void SqlDataSourceRenter_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@aid"].Value = RentAgreementID;
        e.Command.Parameters["@rep_id"].Value = ReportID;
    }

    protected void CalculateTotals()
    {
        decimal totalRentedSquare = 0m;
        Control panelAgreement = OrganizationsForm.FindControl("PanelAgreement");

        DataTable tableNotes = NotesDataSource;

        if (panelAgreement != null && tableNotes != null)
        {
            Control editNumObjects = panelAgreement.FindControl("EditNumApt");
            Control editRentSquare = panelAgreement.FindControl("EditRentSquare");

            if (editNumObjects is ASPxTextBox)
                (editNumObjects as ASPxTextBox).Text = tableNotes.Rows.Count.ToString();

            totalRentedSquare = GetTotalRentedSquare(tableNotes);

            if (editRentSquare is ASPxTextBox)
                (editRentSquare as ASPxTextBox).Text = totalRentedSquare.ToString();
        }

        Control panelRentPayment = PaymentForm.FindControl("PanelRentPayment");

        if (panelRentPayment != null)
        {
            Control editPaymentSqrTotal = panelRentPayment.FindControl("EditPaymentSqrTotal_orndpymnt");

            if (editPaymentSqrTotal is ASPxTextBox)
                (editPaymentSqrTotal as ASPxTextBox).Text = totalRentedSquare.ToString();
        }
    }

    #region Working with the table of Decisions

    protected DataTable DecisionsDataSource
    {
        get
        {
            string key = GetPageUniqueKey();

            object ds = Session[key + "_DECISIONS_DATA_SOURCE"];

            if (ds is DataTable)
            {
                return ds as DataTable;
            }

            object view = SqlDataSourceDecisions.Select(new DataSourceSelectArguments());

            if (view is DataView)
            {
                DataTable dt = (view as DataView).ToTable();

                Session[key + "_DECISIONS_DATA_SOURCE"] = dt;

                return dt;
            }

            return null;
        }

        set
        {
            Session[GetPageUniqueKey() + "_DECISIONS_DATA_SOURCE"] = value;
        }
    }

    protected void CPDecisions_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("add:"))
        {
            DataTable table = DecisionsDataSource;

            if (table != null)
            {
                // Look through all the rows in the table, and find a minimum existing id
                int minExistingId = 0;

                for (int row = 0; row < table.Rows.Count; row++)
                {
                    object id = table.Rows[row]["id"];

                    if (id is int && (int)id < minExistingId)
                    {
                        minExistingId = (int)id;
                    }
                }

                int newRowId = (minExistingId < 0) ? minExistingId - 1 : -1;

                // id, arenda_id, doc_num, doc_date, doc_dodatok, doc_punkt, purpose_str, rent_square, pidstava

                object[] values = new object[] { newRowId, RentAgreementID, "", null, null, null, "", 0m, "" };

                table.Rows.Add(values);

                GridViewDecisions.DataBind();
            }
        }
    }

    protected void GridViewDecisions_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
    {
        object rowKey = e.Keys[GridViewDecisions.KeyFieldName];

        if (rowKey is int)
        {
            int decisionId = (int)rowKey;

            // Find a row in the DataTable, and delete it
            DataTable table = DecisionsDataSource;

            if (table != null)
            {
                for (int row = 0; row < table.Rows.Count; row++)
                {
                    object id = table.Rows[row]["id"];

                    if (id is int && (int)id == decisionId)
                    {
                        table.Rows.RemoveAt(row);
                        break;
                    }
                }

                GridViewDecisions.DataBind();
            }
        }

        e.Cancel = true;
    }

    protected void GridViewDecisions_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        ASPxComboBox comboDocName = GridViewDecisions.FindEditFormTemplateControl("ComboPidstavaDocKind") as ASPxComboBox;
        ASPxTextBox editDocNum = GridViewDecisions.FindEditFormTemplateControl("EditPidstavaDocNum") as ASPxTextBox;
        ASPxDateEdit editDocDate = GridViewDecisions.FindEditFormTemplateControl("EditPidstavaDocDate") as ASPxDateEdit;
        ASPxTextBox editDocSquare = GridViewDecisions.FindEditFormTemplateControl("EditPidstavaDocSquare") as ASPxTextBox;
        ASPxTextBox editDocPurpose = GridViewDecisions.FindEditFormTemplateControl("EditPidstavaDocPurpose") as ASPxTextBox;

        if (comboDocName != null && editDocNum != null && editDocDate != null && editDocSquare != null && editDocPurpose != null)
        {
            object rowKey = e.Keys[GridViewDecisions.KeyFieldName];

            if (rowKey is int)
            {
                int decisionId = (int)rowKey;

                // Find a row in the DataTable, and modify it
                DataTable table = DecisionsDataSource;

                if (table != null)
                {
                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        object id = table.Rows[row]["id"];

                        if (id is int && (int)id == decisionId)
                        {
                            object[] values = table.Rows[row].ItemArray;

                            // id, arenda_id, doc_num, doc_date, doc_dodatok, doc_punkt, purpose_str, rent_square, pidstava

                            values[2] = editDocNum.Text.Trim().ToUpper().Left(18);
                            values[3] = editDocDate.Date.Year >= 1800 ? (object)editDocDate.Date : null;
                            values[6] = editDocPurpose.Text.Trim().ToUpper().Left(255);
                            values[7] = Utils.ConvertStrToDecimal(editDocSquare.Text);
                            values[8] = comboDocName.Text.Trim().ToUpper().Left(255);

                            table.Rows[row].ItemArray = values;
                            break;
                        }
                    }

                    GridViewDecisions.DataBind();
                }
            }
        }

        e.Cancel = true;
        GridViewDecisions.CancelEdit();
    }

    protected void SaveDecisions(SqlConnection connection)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        // Delete all rent decisions related to this agreement
        using (SqlCommand cmd = new SqlCommand("DELETE FROM reports1nf_arenda_decisions WHERE report_id = @rid AND arenda_id = @aid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
            cmd.ExecuteNonQuery();
        }

        // Save each decision as a new one
        DataTable table = DecisionsDataSource;

        if (table != null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            for (int row = 0; row < table.Rows.Count; row++)
            {
                object[] values = table.Rows[row].ItemArray;

                // id, arenda_id, doc_num, doc_date, doc_dodatok, doc_punkt, purpose_str, rent_square, pidstava

                string fieldList = "report_id, arenda_id, modify_date, modified_by";
                string paramList = "@rid, @aid, @mdt, @mby";

                parameters.Clear();
                parameters.Add("rid", ReportID);
                parameters.Add("aid", RentAgreementID);
                parameters.Add("mdt", DateTime.Now);
                parameters.Add("mby", username.Left(18));

                if (values[2] is string)
                {
                    fieldList += ", doc_num";
                    paramList += ", @dnum";
                    parameters.Add("dnum", values[2]);
                }

                if (values[3] is DateTime)
                {
                    fieldList += ", doc_date";
                    paramList += ", @ddate";
                    parameters.Add("ddate", values[3]);
                }

                if (values[6] is string)
                {
                    fieldList += ", purpose_str";
                    paramList += ", @pstr";
                    parameters.Add("pstr", values[6]);
                }

                if (values[7] is decimal)
                {
                    fieldList += ", rent_square";
                    paramList += ", @rsqr";
                    parameters.Add("rsqr", values[7]);
                }

                if (values[8] is string)
                {
                    fieldList += ", pidstava";
                    paramList += ", @pdst";
                    parameters.Add("pdst", values[8]);
                }

                using (SqlCommand cmdInsert = new SqlCommand("INSERT INTO reports1nf_arenda_decisions (" + fieldList + ") VALUES (" + paramList + ")", connection))
                {
                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        cmdInsert.Parameters.Add(new SqlParameter(param.Key, param.Value));
                    }

                    cmdInsert.ExecuteNonQuery();
                }
            }
        }
    }

	#endregion Working with the table of Decisions

	#region Working with the table of Subleases

	protected DataTable SubleasesDataSource
	{
		get
		{
			string key = GetPageUniqueKey();

			object ds = Session[key + "_SUBLEASES_DATA_SOURCE"];

			if (ds is DataTable)
			{
				return ds as DataTable;
			}

			object view = SqlDataSourceSubleases.Select(new DataSourceSelectArguments());

			if (view is DataView)
			{
				DataTable dt = (view as DataView).ToTable();

				Session[key + "_SUBLEASES_DATA_SOURCE"] = dt;

				return dt;
			}

			return null;
		}

		set
		{
			Session[GetPageUniqueKey() + "_SUBLEASES_DATA_SOURCE"] = value;
		}
	}

	protected void CPSubleases_Callback(object sender, CallbackEventArgsBase e)
	{
		if (e.Parameter.StartsWith("add:"))
		{
			DataTable table = SubleasesDataSource;

			if (table != null)
			{
				// Look through all the rows in the table, and find a minimum existing id
				int minExistingId = 0;

				for (int row = 0; row < table.Rows.Count; row++)
				{
					object id = table.Rows[row]["id"];

					if (id is int && (int)id < minExistingId)
					{
						minExistingId = (int)id;
					}
				}

				int newRowId = (minExistingId < 0) ? minExistingId - 1 : -1;

				// id, arenda_id, payment_type_id, agreement_date, agreement_num, rent_start_date, rent_finish_date, rent_square, rent_payment_month

				object[] values = new object[] { newRowId, RentAgreementID, null, null, null, null, null, null, null };

				table.Rows.Add(values);

				GridViewSubleases.DataBind();
			}
		}
	}

	protected void GridViewSubleases_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
	{
		object rowKey = e.Keys[GridViewSubleases.KeyFieldName];

		if (rowKey is int)
		{
			int subleaseId = (int)rowKey;

			// Find a row in the DataTable, and delete it
			DataTable table = SubleasesDataSource;

			if (table != null)
			{
				for (int row = 0; row < table.Rows.Count; row++)
				{
					object id = table.Rows[row]["id"];

					if (id is int && (int)id == subleaseId)
					{
						table.Rows.RemoveAt(row);
						break;
					}
				}

				GridViewSubleases.DataBind();
			}
		}

		e.Cancel = true;
	}

	protected void GridViewSubleases_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
	{
		var edit_payment_type_id = GridViewSubleases.FindEditFormTemplateControl("edit_payment_type_id") as ASPxComboBox;
		var edit_agreement_num = GridViewSubleases.FindEditFormTemplateControl("edit_agreement_num") as ASPxTextBox;
		var edit_agreement_date = GridViewSubleases.FindEditFormTemplateControl("edit_agreement_date") as ASPxDateEdit;
		var edit_rent_start_date = GridViewSubleases.FindEditFormTemplateControl("edit_rent_start_date") as ASPxDateEdit;
		var edit_rent_finish_date = GridViewSubleases.FindEditFormTemplateControl("edit_rent_finish_date") as ASPxDateEdit;
		var edit_rent_square = GridViewSubleases.FindEditFormTemplateControl("edit_rent_square") as ASPxSpinEdit;
		var edit_rent_payment_month = GridViewSubleases.FindEditFormTemplateControl("edit_rent_payment_month") as ASPxSpinEdit;
		var edit_using_possible_id = GridViewSubleases.FindEditFormTemplateControl("edit_using_possible_id") as ASPxComboBox;


		if (edit_agreement_num != null && edit_agreement_date != null && edit_rent_start_date != null && 
			edit_rent_finish_date != null && edit_payment_type_id != null && edit_rent_square != null && edit_rent_payment_month != null &&
			edit_using_possible_id != null)
		{
			object rowKey = e.Keys[GridViewSubleases.KeyFieldName];

			if (rowKey is int)
			{
				int subleaseId = (int)rowKey;

				// Find a row in the DataTable, and modify it
				DataTable table = SubleasesDataSource;

				if (table != null)
				{
					for (int row = 0; row < table.Rows.Count; row++)
					{
						object id = table.Rows[row]["id"];

						if (id is int && (int)id == subleaseId)
						{
							object[] values = table.Rows[row].ItemArray;

							// id, arenda_id, payment_type_id, agreement_date, agreement_num, rent_start_date, rent_finish_date, rent_square, rent_payment_month

							values[2] = edit_payment_type_id.Value == null ? DBNull.Value : (object)edit_payment_type_id.Value;
							values[3] = edit_agreement_date.Date.Year >= 1900 ? (object)edit_agreement_date.Date : DBNull.Value;
							values[4] = edit_agreement_num.Text.Trim();
							values[5] = edit_rent_start_date.Date.Year >= 1900 ? (object)edit_rent_start_date.Date : DBNull.Value;
							values[6] = edit_rent_finish_date.Date.Year >= 1900 ? (object)edit_rent_finish_date.Date : DBNull.Value;
							values[7] = Utils.ConvertStrToDecimal(edit_rent_square.Text);
							values[8] = Utils.ConvertStrToDecimal(edit_rent_payment_month.Text);
							values[9] = edit_using_possible_id.Value == null ? DBNull.Value : (object)edit_using_possible_id.Value;
							table.Rows[row].ItemArray = values;
							break;
						}
					}

					GridViewSubleases.DataBind();
				}
			}
		}

		e.Cancel = true;
		GridViewSubleases.CancelEdit();
	}

	protected void SaveSubleases(SqlConnection connection)
	{
		System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
		string username = (user == null ? "System" : user.UserName);

		// Delete all rent subleases related to this agreement
		using (SqlCommand cmd = new SqlCommand("DELETE FROM reports1nf_arenda_subleases WHERE report_id = @rid AND arenda_id = @aid", connection))
		{
			cmd.Parameters.Add(new SqlParameter("rid", ReportID));
			cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
			cmd.ExecuteNonQuery();
		}

		// Save each sublease as a new one
		DataTable table = SubleasesDataSource;

		if (table != null)
		{
			Dictionary<string, object> parameters = new Dictionary<string, object>();

			for (int row = 0; row < table.Rows.Count; row++)
			{
				object[] values = table.Rows[row].ItemArray;

				// id, arenda_id, payment_type_id, agreement_date, agreement_num, rent_start_date, rent_finish_date, rent_square, rent_payment_month

				string fieldList = "report_id, arenda_id, modify_date, modified_by";
				string paramList = "@rid, @aid, @mdt, @mby";

				parameters.Clear();
				parameters.Add("rid", ReportID);
				parameters.Add("aid", RentAgreementID);
				parameters.Add("mdt", DateTime.Now);
				parameters.Add("mby", username.Left(18));

				if (values[2] is int)
				{
					fieldList += ", payment_type_id";
					paramList += ", @payment_type_id";
					parameters.Add("payment_type_id", values[2]);
				}


				if (values[3] is DateTime)
				{
					fieldList += ", agreement_date";
					paramList += ", @agreement_date";
					parameters.Add("agreement_date", values[3]);
				}

				if (values[4] is string)
				{
					fieldList += ", agreement_num";
					paramList += ", @agreement_num";
					parameters.Add("agreement_num", values[4]);
				}

				if (values[5] is DateTime)
				{
					fieldList += ", rent_start_date";
					paramList += ", @rent_start_date";
					parameters.Add("rent_start_date", values[5]);
				}

				if (values[6] is DateTime)
				{
					fieldList += ", rent_finish_date";
					paramList += ", @rent_finish_date";
					parameters.Add("rent_finish_date", values[6]);
				}

				if (values[7] is decimal)
				{
					fieldList += ", rent_square";
					paramList += ", @rent_square";
					parameters.Add("rent_square", values[7]);
				}

				if (values[8] is decimal)
				{
					fieldList += ", rent_payment_month";
					paramList += ", @rent_payment_month";
					parameters.Add("rent_payment_month", values[8]);
				}

				if (values[9] is int)
				{
					fieldList += ", using_possible_id";
					paramList += ", @using_possible_id";
					parameters.Add("using_possible_id", values[9]);
				}


				using (SqlCommand cmdInsert = new SqlCommand("INSERT INTO reports1nf_arenda_subleases (" + fieldList + ") VALUES (" + paramList + ")", connection))
				{
					foreach (KeyValuePair<string, object> param in parameters)
					{
						cmdInsert.Parameters.Add(new SqlParameter(param.Key, param.Value));
					}

					cmdInsert.ExecuteNonQuery();
				}
			}
		}
	}

	#endregion Working with the table of Subleases

	#region Working with the table of Notes

	protected DataTable NotesDataSource
    {
        get
        {
            string key = GetPageUniqueKey();

            object ds = Session[key + "_NOTES_DATA_SOURCE"];

            if (ds is DataTable)
            {
                return ds as DataTable;
            }

            object view = SqlDataSourceNotes.Select(new DataSourceSelectArguments());

            if (view is DataView)
            {
                DataTable dt = (view as DataView).ToTable();

                Session[key + "_NOTES_DATA_SOURCE"] = dt;

                return dt;
            }

            return null;
        }

        set
        {
            Session[GetPageUniqueKey() + "_NOTES_DATA_SOURCE"] = value;
        }
    }

    protected void CPNotes_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("add:"))
        {
            DataTable table = NotesDataSource;

            if (table != null)
            {
                // Look through all the rows in the table, and find a minimum existing id
                int minExistingId = 0;

                for (int row = 0; row < table.Rows.Count; row++)
                {
                    object id = table.Rows[row]["id"];

                    if (id is int && (int)id < minExistingId)
                    {
                        minExistingId = (int)id;
                    }
                }

                int newRowId = (minExistingId < 0) ? minExistingId - 1 : -1;

                // id, purpose_group_id, purpose_id, purpose_str, rent_square, note, rent_rate, cost_narah, cost_agreement, cost_expert_total, date_expert, payment_type_id, invent_no, note_status_id

                object[] values = new object[] { newRowId, null, null, "", 0m, "", 0m, 0m, 0m, 0m, null, null, "", null };

                table.Rows.Add(values);

                GridViewNotes.DataBind();
            }
        }
    }

    protected void GridViewNotes_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
    {
        object rowKey = e.Keys[GridViewNotes.KeyFieldName];

        if (rowKey is int)
        {
            int decisionId = (int)rowKey;

            // Find a row in the DataTable, and delete it
            DataTable table = NotesDataSource;

            if (table != null)
            {
                for (int row = 0; row < table.Rows.Count; row++)
                {
                    object id = table.Rows[row]["id"];

                    if (id is int && (int)id == decisionId)
                    {
                        table.Rows.RemoveAt(row);
                        break;
                    }
                }

                GridViewNotes.DataBind();
            }
        }

        e.Cancel = true;
    }

    protected void GridViewNotes_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        ASPxTextBox editNoteSquare = GridViewNotes.FindEditFormTemplateControl("EditNoteSqr") as ASPxTextBox;
        ASPxTextBox editNoteFloor = GridViewNotes.FindEditFormTemplateControl("EditNoteFloor") as ASPxTextBox;
        ASPxTextBox editNoteInventNo = GridViewNotes.FindEditFormTemplateControl("EditNoteInventNo") as ASPxTextBox;
        ASPxComboBox comboNoteCurState = GridViewNotes.FindEditFormTemplateControl("ComboNoteCurState") as ASPxComboBox;
        ASPxComboBox comboNotePurposeGroup = GridViewNotes.FindEditFormTemplateControl("ComboNotePurposeGroup") as ASPxComboBox;
        ASPxComboBox comboNotePurpose = GridViewNotes.FindEditFormTemplateControl("ComboNotePurpose") as ASPxComboBox;
        ASPxTextBox editNotePurposeStr = GridViewNotes.FindEditFormTemplateControl("EditNotePurposeStr") as ASPxTextBox;
        ASPxTextBox editNoteCostExpert = GridViewNotes.FindEditFormTemplateControl("EditNoteCostExpert") as ASPxTextBox;
        ASPxDateEdit editNoteDateExpert = GridViewNotes.FindEditFormTemplateControl("EditNoteDateExpert") as ASPxDateEdit;
        ASPxTextBox editNoteCostNarah = GridViewNotes.FindEditFormTemplateControl("EditNoteCostNarah") as ASPxTextBox;
        ASPxTextBox editNoteRentRate = GridViewNotes.FindEditFormTemplateControl("EditNoteRentRate") as ASPxTextBox;
        ASPxTextBox editNoteCostAgreement = GridViewNotes.FindEditFormTemplateControl("EditNoteCostAgreement") as ASPxTextBox;
        ASPxComboBox comboNotePaymentType = GridViewNotes.FindEditFormTemplateControl("ComboNotePaymentType1") as ASPxComboBox;

        if (editNoteSquare != null && editNoteFloor != null && editNoteInventNo != null &&
            comboNoteCurState != null && comboNotePurposeGroup != null &&
            comboNotePurpose != null && editNotePurposeStr != null && editNoteCostExpert != null &&
            editNoteDateExpert != null && editNoteCostNarah != null && editNoteRentRate != null &&
            editNoteCostAgreement != null && comboNotePaymentType != null)
        {
            object rowKey = e.Keys[GridViewNotes.KeyFieldName];

            if (rowKey is int)
            {
                int decisionId = (int)rowKey;

                // Find a row in the DataTable, and modify it
                DataTable table = NotesDataSource;

                if (table != null)
                {
                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        object id = table.Rows[row]["id"];

                        if (id is int && (int)id == decisionId)
                        {
                            object[] values = table.Rows[row].ItemArray;

                            // id, purpose_group_id, purpose_id, purpose_str, rent_square, note, rent_rate, cost_narah, cost_agreement, cost_expert_total, date_expert, payment_type_id, invent_no, note_status_id

                            values[1] = comboNotePurposeGroup.Value is int ? comboNotePurposeGroup.Value : null;
                            values[2] = comboNotePurpose.Value is int ? comboNotePurpose.Value : null;
                            values[3] = editNotePurposeStr.Text.Trim().ToUpper().Left(250);
                            values[4] = Utils.ConvertStrToDecimal(editNoteSquare.Text);
                            values[5] = editNoteFloor.Text.Trim().ToUpper().Left(250);
                            values[6] = Utils.ConvertStrToDecimal(editNoteRentRate.Text);
                            values[7] = Utils.ConvertStrToDecimal(editNoteCostNarah.Text);
                            values[8] = Utils.ConvertStrToDecimal(editNoteCostAgreement.Text);
                            values[9] = Utils.ConvertStrToDecimal(editNoteCostExpert.Text);
                            values[10] = editNoteDateExpert.Date.Year >= 1800 ? (object)editNoteDateExpert.Date : null;
                            values[11] = comboNotePaymentType.Value is int ? comboNotePaymentType.Value : null;
                            values[12] = editNoteInventNo.Text.Trim().ToUpper().Left(128);
                            values[13] = comboNoteCurState.Value is int ? comboNoteCurState.Value : null;

                            table.Rows[row].ItemArray = values;
                            break;
                        }
                    }

                    GridViewNotes.DataBind();
                }
            }
        }

        e.Cancel = true;
        GridViewNotes.CancelEdit();
    }

    protected decimal GetTotalRentedSquare(DataTable tableNotes)
    {
        decimal total = 0m;

        for (int row = 0; row < tableNotes.Rows.Count; row++)
        {
            object square = tableNotes.Rows[row]["rent_square"];

            if (square is decimal)
            {
                total += (decimal)square;
            }
        }

        return total;
    }

    protected void SaveNotes(SqlConnection connection)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        // Delete all notes related to this agreement
        using (SqlCommand cmd = new SqlCommand("DELETE FROM reports1nf_arenda_notes WHERE report_id = @rid AND arenda_id = @aid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
            cmd.ExecuteNonQuery();
        }

        // Save each note as a new one
        DataTable table = NotesDataSource;

        if (table != null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            for (int row = 0; row < table.Rows.Count; row++)
            {
                object[] values = table.Rows[row].ItemArray;

                // id, purpose_group_id, purpose_id, purpose_str, rent_square, note, rent_rate, cost_narah, cost_agreement, cost_expert_total, date_expert, payment_type_id, invent_no, note_status_id

                string fieldList = "report_id, arenda_id, modify_date, modified_by";
                string paramList = "@rid, @aid, @mdt, @mby";

                parameters.Clear();
                parameters.Add("rid", ReportID);
                parameters.Add("aid", RentAgreementID);
                parameters.Add("mdt", DateTime.Now);
                parameters.Add("mby", username.Left(18));

                if (values[1] is int)
                {
                    fieldList += ", purpose_group_id";
                    paramList += ", @pgr";
                    parameters.Add("pgr", values[1]);
                }

                if (values[2] is int)
                {
                    fieldList += ", purpose_id";
                    paramList += ", @pid";
                    parameters.Add("pid", values[2]);
                }

                if (values[3] is string)
                {
                    fieldList += ", purpose_str";
                    paramList += ", @pstr";
                    parameters.Add("pstr", values[3]);
                }

                if (values[4] is decimal)
                {
                    fieldList += ", rent_square";
                    paramList += ", @rsqr";
                    parameters.Add("rsqr", values[4]);
                }

                if (values[5] is string)
                {
                    fieldList += ", note";
                    paramList += ", @note";
                    parameters.Add("note", values[5]);
                }

                if (values[6] is decimal)
                {
                    fieldList += ", rent_rate";
                    paramList += ", @rrate";
                    parameters.Add("rrate", values[6]);
                }

                if (values[7] is decimal)
                {
                    fieldList += ", cost_narah";
                    paramList += ", @costnar";
                    parameters.Add("costnar", values[7]);
                }

                if (values[8] is decimal)
                {
                    fieldList += ", cost_agreement";
                    paramList += ", @costagr";
                    parameters.Add("costagr", values[8]);
                }

                if (values[9] is decimal)
                {
                    fieldList += ", cost_expert_total";
                    paramList += ", @costexp";
                    parameters.Add("costexp", values[9]);
                }

                if (values[10] is DateTime)
                {
                    fieldList += ", date_expert";
                    paramList += ", @dtex";
                    parameters.Add("dtex", values[10]);
                }

                if (values[11] is int)
                {
                    fieldList += ", payment_type_id";
                    paramList += ", @payid";
                    parameters.Add("payid", values[11]);
                }

                if (values[12] is string)
                {
                    fieldList += ", invent_no";
                    paramList += ", @ino";
                    parameters.Add("ino", values[12]);
                }

                if (values[13] is int)
                {
                    fieldList += ", note_status_id";
                    paramList += ", @stid";
                    parameters.Add("stid", values[13]);
                }

                using (SqlCommand cmdInsert = new SqlCommand("INSERT INTO reports1nf_arenda_notes (" + fieldList + ") VALUES (" + paramList + ")", connection))
                {
                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        cmdInsert.Parameters.Add(new SqlParameter(param.Key, param.Value));
                    }

                    cmdInsert.ExecuteNonQuery();
                }
            }
        }
    }

    #endregion Working with the table of Notes

    #region Working with the table of Payment Documents

    protected DataTable PaymentDocumentsDataSource
    {
        get
        {
            string key = GetPageUniqueKey();

            object ds = Session[key + "_PAYMENT_DOCUMENTS_DATA_SOURCE"];

            if (ds is DataTable)
            {
                return ds as DataTable;
            }

            object view = SqlDataSourcePaymentDocuments.Select(new DataSourceSelectArguments());

            if (view is DataView)
            {
                DataTable dt = (view as DataView).ToTable();

                Session[key + "_PAYMENT_DOCUMENTS_DATA_SOURCE"] = dt;

                return dt;
            }

            return null;
        }

        set
        {
            Session[GetPageUniqueKey() + "_PAYMENT_DOCUMENTS_DATA_SOURCE"] = value;
        }
    }

    protected void CPPaymentDocuments_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("add:"))
        {
            DataTable table = PaymentDocumentsDataSource;

            if (table != null)
            {
                // Look through all the rows in the table, and find a minimum existing id
                int minExistingId = 0;

                for (int row = 0; row < table.Rows.Count; row++)
                {
                    object id = table.Rows[row]["id"];

                    if (id is int && (int)id < minExistingId)
                    {
                        minExistingId = (int)id;
                    }
                }

                int newRowId = (minExistingId < 0) ? minExistingId - 1 : -1;

                // id, report_id, arenda_id, payment_date, payment_number, payment_sum, payment_purpose

                object[] values = new object[] { newRowId, ReportID, RentAgreementID, null, "", 0m, "", null, null, ActiveRentPeriodID };

                table.Rows.Add(values);

                GridViewPaymentDocuments.DataBind();
            }
        }
    }

    protected void CPRentPayment_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("calc:"))
        {
            decimal total_income = 0;
            int active_rent_period_id;

            ASPxComboBox reportingPeriodCombo = PaymentForm.FindControl("ReportingPeriodCombo") as ASPxComboBox;
            if (reportingPeriodCombo != null)
            {
                active_rent_period_id = (int)reportingPeriodCombo.Value;
            }
            else
            {
                active_rent_period_id = ActiveRentPeriodID;
            }
            DataTable table = PaymentDocumentsDataSource;

            if (table != null)
            {
                for (int row = 0; row < table.Rows.Count; row++)
                {
                    var rent_period_id = (int)table.Rows[row]["rent_period_id"];

                    if (rent_period_id == active_rent_period_id)
                    {
                        total_income = total_income + (decimal)table.Rows[row]["payment_sum"];
                    }
                }
            }
            Control panelRentPayment = PaymentForm.FindControl("PanelRentPaymentDocuments");
            if (panelRentPayment != null)
            {
                Control cpRentPayment = panelRentPayment.FindControl("CPRentPayment");
                if (cpRentPayment != null)
                {
                    Control editPaymentReceived = cpRentPayment.FindControl("EditPaymentReceived_orndpymnt");

                    if (editPaymentReceived is ASPxSpinEdit)
                        (editPaymentReceived as ASPxSpinEdit).Value = total_income;
                }
            }
        }
    }

    protected void GridViewPaymentDocuments_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
    {
        object rowKey = e.Keys[GridViewPaymentDocuments.KeyFieldName];

        if (rowKey is int)
        {
            int paymentDocumentId = (int)rowKey;

            // Find a row in the DataTable, and delete it
            DataTable table = PaymentDocumentsDataSource;

            if (table != null)
            {
                for (int row = 0; row < table.Rows.Count; row++)
                {
                    object id = table.Rows[row]["id"];

                    if (id is int && (int)id == paymentDocumentId)
                    {
                        table.Rows.RemoveAt(row);
                        break;
                    }
                }
                GridViewPaymentDocuments.DataBind();
            }
        }

        e.Cancel = true;
    }

    protected void GridViewPaymentDocuments_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        ASPxDateEdit editPaymentDate = GridViewPaymentDocuments.FindEditFormTemplateControl("EditPaymentDate") as ASPxDateEdit;
        ASPxTextBox editPaymentNumber = GridViewPaymentDocuments.FindEditFormTemplateControl("EditPaymentNumber") as ASPxTextBox;
        ASPxSpinEdit editPaymentSum = GridViewPaymentDocuments.FindEditFormTemplateControl("EditPaymentSum") as ASPxSpinEdit;
        ASPxMemo editPaymentPurpose = GridViewPaymentDocuments.FindEditFormTemplateControl("EditPaymentPurpose") as ASPxMemo;
        ASPxComboBox editPaymentPeriod = GridViewPaymentDocuments.FindEditFormTemplateControl("EditPaymentPeriod") as ASPxComboBox;

        if (editPaymentDate != null && editPaymentNumber != null && editPaymentSum != null && editPaymentPurpose != null && editPaymentPeriod != null)
        {
            object rowKey = e.Keys[GridViewPaymentDocuments.KeyFieldName];

            if (rowKey is int)
            {
                int paymentDocumentId = (int)rowKey;

                // Find a row in the DataTable, and modify it
                DataTable table = PaymentDocumentsDataSource;

                if (table != null)
                {
                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        object id = table.Rows[row]["id"];

                        if (id is int && (int)id == paymentDocumentId)
                        {
                            object[] values = table.Rows[row].ItemArray;

                            // id, report_id, arenda_id, payment_date, payment_number, payment_sum, payment_purpose, modify_date, modified_by, rent_period_id
                            values[1] = ReportID;
                            values[2] = RentAgreementID;
                            values[3] = editPaymentDate.Date.Year >= 1800 ? (object)editPaymentDate.Date : null;
                            values[4] = editPaymentNumber.Text.Trim().ToUpper().Left(64);
                            values[5] = Utils.ConvertStrToDecimal(editPaymentSum.Text);
                            values[6] = editPaymentPurpose.Text.Trim().ToUpper().Left(256);
                            values[9] = editPaymentPeriod.SelectedItem.Value;

                            table.Rows[row].ItemArray = values;
                            break;
                        }
                    }

                    GridViewPaymentDocuments.DataBind();
                }
            }
        }

        e.Cancel = true;
        GridViewPaymentDocuments.CancelEdit();
    }

    protected void SavePaymentDocuments(SqlConnection connection)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        // Delete all notes related to this agreement
        using (SqlCommand cmd = new SqlCommand("DELETE FROM reports1nf_payment_documents WHERE report_id = @rid AND arenda_id = @aid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
            cmd.ExecuteNonQuery();
        }

        // Save each note as a new one
        DataTable table = PaymentDocumentsDataSource;

        if (table != null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            for (int row = 0; row < table.Rows.Count; row++)
            {
                object[] values = table.Rows[row].ItemArray;

                // id, report_id, arenda_id, payment_date, payment_number, payment_sum, payment_purpose, modify_date, modified_by, rent_period_id

                string fieldList = "report_id, arenda_id, modify_date, modified_by";
                string paramList = "@rid, @aid, @mdt, @mby";

                parameters.Clear();
                parameters.Add("rid", ReportID);
                parameters.Add("aid", RentAgreementID);
                parameters.Add("mdt", DateTime.Now);
                parameters.Add("mby", username.Left(64));

                if (values[3] is DateTime)
                {
                    fieldList += ", payment_date";
                    paramList += ", @payment_date";
                    parameters.Add("payment_date", values[3]);
                }

                if (values[4] is string)
                {
                    fieldList += ", payment_number";
                    paramList += ", @payment_number";
                    parameters.Add("payment_number", values[4]);
                }

                if (values[5] is decimal)
                {
                    fieldList += ", payment_sum";
                    paramList += ", @payment_sum";
                    parameters.Add("payment_sum", values[5]);
                }

                if (values[6] is string)
                {
                    fieldList += ", payment_purpose";
                    paramList += ", @payment_purpose";
                    parameters.Add("payment_purpose", values[6]);
                }

                if (values[9] is Int32)
                {
                    fieldList += ", rent_period_id";
                    paramList += ", @rent_period_id";
                    parameters.Add("rent_period_id", values[9]);
                }

                using (SqlCommand cmdInsert = new SqlCommand("INSERT INTO reports1nf_payment_documents (" + fieldList + ") VALUES (" + paramList + ")", connection))
                {
                    foreach (KeyValuePair<string, object> param in parameters)
                    {
                        cmdInsert.Parameters.Add(new SqlParameter(param.Key, param.Value));
                    }

                    cmdInsert.ExecuteNonQuery();
                }
            }
        }
    }

    #endregion Working with the table of Payment Documents

    #region Collection tab

    protected void EnableCollectionControls()
    {

        Control panelCollection = CollectionForm.FindControl("PanelCollection");
        Control panelCollection2 = CollectionForm.FindControl("PanelCollection2");
        if (panelCollection is ASPxRoundPanel)
        {
            Control checkBox = CollectionForm.FindControl("CheckNoDebt");

            if (checkBox is ASPxCheckBox)
            {
                bool enable = !((checkBox as ASPxCheckBox).Checked);

                EnableCollectionControl(panelCollection, enable, "EditCollectionDebtTotal");
                EnableCollectionControl(panelCollection, enable, "EditCollectionDebtZvit");
                EnableCollectionControl(panelCollection, enable, "EditCollectionDebt3Month");
                EnableCollectionControl(panelCollection, enable, "EditCollectionDebt12Month");
                EnableCollectionControl(panelCollection, enable, "EditCollectionDebt3Years");
                EnableCollectionControl(panelCollection, enable, "EditCollectionDebtOver3Years");
                EnableCollectionControl(panelCollection, enable, "EditCollectionDebtVMezhahVitrat");
                EnableCollectionControl(panelCollection, enable, "EditCollectionDebtSpysano");
            }
        }
        if (panelCollection2 is ASPxRoundPanel)
        {
            Control checkBox = CollectionForm.FindControl("CheckNoDebt");

            if (checkBox is ASPxCheckBox)
            {
                bool enable = !((checkBox as ASPxCheckBox).Checked);

                EnableCollectionControl(panelCollection2, enable, "EditCollectionNumZahodivTotal");
                EnableCollectionControl(panelCollection2, enable, "EditCollectionNumZahodivZvit");
                EnableCollectionControl(panelCollection2, enable, "EditCollectionNumPozovTotal");
                EnableCollectionControl(panelCollection2, enable, "EditCollectionNumPozovZvit");
                EnableCollectionControl(panelCollection2, enable, "EditCollectionPozovZadovTotal");
                EnableCollectionControl(panelCollection2, enable, "EditCollectionPozovZadovZvit");
                EnableCollectionControl(panelCollection2, enable, "EditCollectionPozovVikonTotal");
                EnableCollectionControl(panelCollection2, enable, "EditCollectionPozovVikonZvit");
                EnableCollectionControl(panelCollection2, enable, "EditCollectionDebtPayedTotal");
                EnableCollectionControl(panelCollection2, enable, "EditCollectionDebtPayedZvit");
            }
        }
    }

    protected void EnableCollectionControl(Control panelCollection, bool enable, string controlID)
    {
        Control ctl = panelCollection.FindControl(controlID);

        if (ctl is ASPxSpinEdit)
        {
            (ctl as ASPxSpinEdit).ClientEnabled = enable;
        }
    }

    protected void EnableInsuranceControls()
    {
        Control panelInsurance = InsuranceForm.FindControl("InsurancePanel");

        if (panelInsurance is ASPxRoundPanel)
        {
            Control isInsured = panelInsurance.FindControl("is_insured");

            if (isInsured is ASPxCheckBox)
            {
                //bool visibility = (isInsured as ASPxCheckBox).Checked;
                bool enable = (isInsured as ASPxCheckBox).Checked;
                EnableInsuranceControl(panelInsurance, enable, "insurance_start");
                EnableInsuranceControl(panelInsurance, enable, "insurance_end");
                EnableInsuranceControl(panelInsurance, enable, "insurance_sum");
                /*
                panelInsurance.FindControl("lbl_insurance_start").Visible = visibility;
                panelInsurance.FindControl("insurance_start").Visible = visibility;
                panelInsurance.FindControl("lbl_insurance_end").Visible = visibility;
                panelInsurance.FindControl("insurance_end").Visible = visibility;
                panelInsurance.FindControl("lbl_insurance_sum").Visible = visibility;
                panelInsurance.FindControl("insurance_sum").Visible = visibility;*/
            }
        }
    }

    protected void EnableInsuranceControl(Control panelInsurance, bool enable, string controlID)
    {
        Control ctl = panelInsurance.FindControl(controlID);

        if (ctl is ASPxSpinEdit)
        {
            (ctl as ASPxSpinEdit).ClientEnabled = enable;
        }
        else if (ctl is ASPxDateEdit)
        {
            (ctl as ASPxDateEdit).ClientEnabled = enable;
        }
    }

    #endregion (Collection tab)

    void AddressChange(SqlConnection connection)
    {
        using (var transaction = connection.BeginTransaction())
        {
            Dictionary<string, Control> allcontrols = new Dictionary<string, Control>();
            Reports1NFUtils.GetAllControls(AddressForm, allcontrols);
            var combostreet = (ASPxComboBox)allcontrols["combostreet"];
            var addr_street_id = (int?)combostreet.Value;
            if (addr_street_id == null) throw new Exception("Внутрішня помилка 1001.4");

            var new_building_id = (int?)null;
            var combobuilding = (ASPxComboBox)allcontrols["combobuilding"];
            var nomer = (combobuilding.Text ?? "").Trim();
            using (SqlCommand cmd = new SqlCommand(
                    @"select id from buildings where 
                        (is_deleted IS NULL OR is_deleted = 0) AND
                        (master_building_id IS NULL) AND
                        (addr_street_id = @street_id) AND
                        (RTRIM(LTRIM(addr_nomer)) = @nomer)", connection, transaction))
            {
                cmd.Parameters.Add(new SqlParameter("street_id", addr_street_id));
                cmd.Parameters.Add(new SqlParameter("nomer", nomer));
                new_building_id = (int?)cmd.ExecuteScalar();
            }
            if (new_building_id == null)
            {
                throw new Exception("Номер будинку повинен бути заповнений");
            }

            var cntrow = 0;
            int? building_id = null;
            int? building_1nf_unique_id = null;
            using (SqlCommand cmd = new SqlCommand(
                    @"SELECT ar.building_id, ar.building_1nf_unique_id FROM reports1nf_arenda ar INNER JOIN reports1nf_buildings b 
                        ON b.unique_id = ar.building_1nf_unique_id WHERE ar.id = @id AND ar.report_id = @report_id and ar.building_id = b.id", connection, transaction))
            {
                cmd.Parameters.Add(new SqlParameter("report_id", ReportID));
                cmd.Parameters.Add(new SqlParameter("id", RentAgreementID));
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cntrow++;
                        if (!reader.IsDBNull(0)) building_id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1)) building_1nf_unique_id = reader.GetInt32(1);
                    }
                    reader.Close();
                }
            }

            if (cntrow != 1) throw new Exception("Внутрішня помилка 1001.1");
            if (building_id == null) throw new Exception("Внутрішня помилка 1001.2");
            if (building_1nf_unique_id == null) throw new Exception("Внутрішня помилка 1001.3");

            //если поменяли адрес
            if (new_building_id != building_id)
            {
                var sql = "UPDATE reports1nf_arenda SET building_id = @building_id WHERE id = @id AND report_id = @report_id";
                using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
                {
                    cmd.Parameters.Add(new SqlParameter("building_id", new_building_id));
                    cmd.Parameters.Add(new SqlParameter("report_id", ReportID));
                    cmd.Parameters.Add(new SqlParameter("id", RentAgreementID));
                    cmd.ExecuteNonQuery();
                }

                sql = @"DELETE FROM reports1nf_buildings WHERE unique_id = @unique_id";
                using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
                {
                    cmd.Parameters.Add(new SqlParameter("unique_id", building_1nf_unique_id));
                    cmd.ExecuteNonQuery();
                }

                sql = @"SET IDENTITY_INSERT dbo.reports1nf_buildings ON;  
                        INSERT INTO reports1nf_buildings(id,master_building_id,addr_street_name,addr_street_id,addr_street_name2,addr_street_id2,street_full_name,addr_distr_old_id,addr_distr_new_id,addr_nomer1,addr_nomer2,addr_nomer3,addr_nomer,addr_misc,addr_korpus_flag,addr_korpus,addr_zip_code,addr_address,tech_condition_id,date_begin,date_end,num_floors,construct_year,condition_year,is_condition_valid,bti_code,history_id,object_type_id,object_kind_id,is_land,modify_date,modified_by,kadastr_code,cost_balans,sqr_total,sqr_pidval,sqr_mk,sqr_dk,sqr_rk,sqr_other,sqr_rented,sqr_zagal,sqr_for_rent,sqr_habit,sqr_non_habit,additional_info,is_deleted,del_date,oatuu_id,updpr,arch_id,arch_flag,facade_id,nomer_int,characteristics,expl_enter_year,is_basement_exists,is_loft_exists,sqr_loft,unique_id,report_id)
                        SELECT id,master_building_id,addr_street_name,addr_street_id,addr_street_name2,addr_street_id2,street_full_name,addr_distr_old_id,addr_distr_new_id,addr_nomer1,addr_nomer2,addr_nomer3,addr_nomer,addr_misc,addr_korpus_flag,addr_korpus,addr_zip_code,addr_address,tech_condition_id,date_begin,date_end,num_floors,construct_year,condition_year,is_condition_valid,bti_code,history_id,object_type_id,object_kind_id,is_land,modify_date,modified_by,kadastr_code,cost_balans,sqr_total,sqr_pidval,sqr_mk,sqr_dk,sqr_rk,sqr_other,sqr_rented,sqr_zagal,sqr_for_rent,sqr_habit,sqr_non_habit,additional_info,is_deleted,del_date,oatuu_id,updpr,arch_id,arch_flag,facade_id,nomer_int,characteristics,expl_enter_year,is_basement_exists,is_loft_exists,sqr_loft,
                                    @unique_id, @report_id FROM buildings b WHERE b.id = @new_building_id;
                        SET IDENTITY_INSERT dbo.reports1nf_buildings OFF";
                using (SqlCommand cmd = new SqlCommand(sql, connection, transaction))
                {
                    cmd.Parameters.Add(new SqlParameter("new_building_id", new_building_id));
                    cmd.Parameters.Add(new SqlParameter("unique_id", building_1nf_unique_id));
                    cmd.Parameters.Add(new SqlParameter("report_id", ReportID));
                    cmd.ExecuteNonQuery();
                }

            }

            transaction.Commit();
            //transaction.Rollback();
        }
    }

    protected void SaveChanges(SqlConnection connection)
    {
//////
        try
	{

        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        Dictionary<string, Control> controls = new Dictionary<string, Control>();

        Reports1NFUtils.GetAllControls(OrganizationsForm, controls);
        Reports1NFUtils.GetAllControls(PaymentForm, controls);
        Reports1NFUtils.GetAllControls(CollectionForm, controls);
        Reports1NFUtils.GetAllControls(InsuranceForm, controls);




            //////
        var rent_start_date = Reports1NFUtils.GetDateValue(controls, "EditStartDate");
	    if (rent_start_date == null)
	    {
		    var lognet = log4net.LogManager.GetLogger("ReportWebSite");
		    lognet.Debug("--------------- SaveChanges Error 77777 ----------------");
		    throw new ArgumentException("Помилка збереження даних. Спробуйте пізніше.");
	    }

        // Update the rent decisions
        SaveDecisions(connection);

		// Update the rent subleases
		SaveSubleases(connection);

		// Update the agreement notes
		SaveNotes(connection);

        // Update the payment documents
        SavePaymentDocuments(connection);

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string fieldList = "";

        // Get the rent agreement properties
        AddQueryParameter(ref fieldList, "agreement_num", "anum", Reports1NFUtils.GetEditText(controls, "EditAgreementNum"), parameters);
        AddQueryParameter(ref fieldList, "agreement_date", "adt", Reports1NFUtils.GetDateValue(controls, "EditAgreementDate"), parameters);
        AddQueryParameter(ref fieldList, "rent_start_date", "dtstart", Reports1NFUtils.GetDateValue(controls, "EditStartDate"), parameters);
        AddQueryParameter(ref fieldList, "rent_finish_date", "dtfin", Reports1NFUtils.GetDateValue(controls, "EditFinishDate"), parameters);
        AddQueryParameter(ref fieldList, "rent_actual_finish_date", "dtafin", Reports1NFUtils.GetDateValue(controls, "EditActualFinishDate"), parameters);
        AddQueryParameter(ref fieldList, "payment_type_id", "payid", Reports1NFUtils.GetDropDownValue(controls, "ComboPaymentType"), parameters);

        AddQueryParameter(ref fieldList, "rent_square", "rsqr", Utils.ConvertStrToDecimal(Reports1NFUtils.GetEditText(controls, "EditRentSquare"), -1m), parameters);
        AddQueryParameter(ref fieldList, "cost_expert_total", "sqrexp", Reports1NFUtils.GetEditNumeric(controls, "EditCostExpert"), parameters);
        AddQueryParameter(ref fieldList, "date_expert", "dtexp", Reports1NFUtils.GetDateValue(controls, "EditDateExpert"), parameters);
        AddQueryParameter(ref fieldList, "note", "note", Reports1NFUtils.GetEditText(controls, "MemoProlongationComment"), parameters);

        //AddQueryParameter(ref fieldList, "is_subarenda", "issub", Reports1NFUtils.GetCheckBoxValue(controls, "CheckSubarenda") ? 1 : 0, parameters);
		AddQueryParameter(ref fieldList, "is_subarenda", "issub", (SubleasesDataSource.Rows.Count > 0 ? 1 : 0), parameters);
				
        AddQueryParameter(ref fieldList, "is_loan_agreement", "isloanagr", Reports1NFUtils.GetCheckBoxValue(controls, "CheckLoanAgreement") ? 1 : 0, parameters);
        
        AddQueryParameter(ref fieldList, "agreement_state", "ast", 
            Reports1NFUtils.IsRadioButtonChecked(controls, "RadioAgreementActive") ? 1 : 
            Reports1NFUtils.IsRadioButtonChecked(controls, "RadioAgreementToxic") ? 2 : 
            Reports1NFUtils.IsRadioButtonChecked(controls, "RadioAgreementContinuedByAnother") ? 3 : 0, 
            parameters);


        AddQueryParameter(ref fieldList, "update_src_id", "usrc", (int)2, parameters); // 2 ~ from the balans holder
        AddQueryParameter(ref fieldList, "agreement_kind_id", "agrkind", (int)1, parameters); // 1 ~ Arenda

        AddQueryParameter(ref fieldList, "org_renter_id", "renid", Reports1NFUtils.GetDropDownValue(controls, "ComboRenterOrg"), parameters);
        AddQueryParameter(ref fieldList, "org_giver_id", "givid", Reports1NFUtils.GetDropDownValue(controls, "ComboGiverOrg"), parameters);

        // System parameters
        AddQueryParameter(ref fieldList, "modify_date", "mdt", DateTime.Now, parameters);
        AddQueryParameter(ref fieldList, "modified_by", "mby", username.Left(64), parameters);

        // insurance
        AddQueryParameter(ref fieldList, "is_insured", "is_insured", Reports1NFUtils.GetCheckBoxValue(controls, "is_insured") ? 1 : 0, parameters);
        AddQueryParameter(ref fieldList, "insurance_start", "insurance_start", Reports1NFUtils.GetDateValue(controls, "insurance_start"), parameters);
        AddQueryParameter(ref fieldList, "insurance_end", "insurance_end", Reports1NFUtils.GetDateValue(controls, "insurance_end"), parameters);
        AddQueryParameter(ref fieldList, "insurance_sum", "insurance_sum", Reports1NFUtils.GetEditNumeric(controls, "insurance_sum"), parameters);

        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda SET " + fieldList + ", is_valid = @isValid, validation_errors = @errMsgs WHERE report_id = @rid AND id = @aid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
            cmd.Parameters.Add(new SqlParameter("isValid", validator.IsValid));
            cmd.Parameters.Add(new SqlParameter("errMsgs", string.Join("<br/>", validator.ValidationErrorMessages)));

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            cmd.ExecuteNonQuery();
        }

        // If comment is specified, add the default comment
        int newGiverId = Reports1NFUtils.GetDropDownValue(controls, "ComboGiverOrg");
        string comment = Reports1NFUtils.GetEditText(controls, "EditGiverComment");

        if (newGiverId != Utils.GUKVOrganizationID && comment.Length > 0)
        {
            Reports1NFUtils.AddComment(connection, ReportID, comment,
                0, 0, 0, RentAgreementID, 0, "ComboGiverOrg", Resources.Strings.RentAgreementGiver, false, false);
        }

        // Update the payment information
        parameters.Clear();
        fieldList = "";

        AddQueryParameter(ref fieldList, "rent_period_id", "period", Reports1NFUtils.GetDropDownValue(controls, "ReportingPeriodCombo"), parameters);

        AddQueryParameter(ref fieldList, "sqr_total_rent", "sqrtot", Utils.ConvertStrToDecimal(Reports1NFUtils.GetEditText(controls, "EditPaymentSqrTotal_orndpymnt"), -1m), parameters);
        AddQueryParameter(ref fieldList, "sqr_payed_by_percent", "sqrper", Reports1NFUtils.GetEditNumeric(controls, "EditPaymentSqrByPercent_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "sqr_payed_by_1uah", "sqr1uah", Reports1NFUtils.GetEditNumeric(controls, "EditPaymentSqr1UAH_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "sqr_payed_hourly", "sqrhourly", Reports1NFUtils.GetEditNumeric(controls, "EditPaymentSqrHourly_orndpymnt"), parameters);

        AddQueryParameter(ref fieldList, "payment_narah", "paynar", Reports1NFUtils.GetEditNumeric(controls, "EditPaymentNarah_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "znyato_nadmirno_narah", "znyatonadmir", Reports1NFUtils.GetEditNumeric(controls, "edit_znyato_nadmirno_narah"), parameters);
        AddQueryParameter(ref fieldList, "znyato_from_avance", "znyatoavance", Reports1NFUtils.GetEditNumeric(controls, "edit_znyato_from_avance"), parameters);
        AddQueryParameter(ref fieldList, "last_year_saldo", "saldo", Reports1NFUtils.GetEditNumeric(controls, "EditPaymentSaldo_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "avance_saldo", "avancesaldo", Reports1NFUtils.GetEditNumeric(controls, "Edit_avance_saldo"), parameters);
        AddQueryParameter(ref fieldList, "payment_received", "payre", Reports1NFUtils.GetEditNumeric(controls, "EditPaymentReceived_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "payment_nar_zvit", "payzv", Reports1NFUtils.GetEditNumeric(controls, "EditPaymentNarZvit_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "avance_paymentnar", "avancepaymentnar", Reports1NFUtils.GetEditNumeric(controls, "Edit_avance_paymentnar"), parameters);
        AddQueryParameter(ref fieldList, "old_debts_payed", "odbtp", Reports1NFUtils.GetEditNumeric(controls, "EditPaymentOldDebtsPayed_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "return_orend_payed", "returnorendpayed", Reports1NFUtils.GetEditNumeric(controls, "edit_return_orend_payed"), parameters);
        AddQueryParameter(ref fieldList, "return_all_orend_payed", "returnallorendpayed", Reports1NFUtils.GetEditNumeric(controls, "edit_return_all_orend_payed"), parameters);
        AddQueryParameter(ref fieldList, "use_calc_debt", "usecalcdebt", Reports1NFUtils.GetCheckBoxValue(controls, "edit_use_calc_debt") ? 1 : 0, parameters);
        AddQueryParameter(ref fieldList, "avance_plat", "avanceplat", Reports1NFUtils.GetEditNumeric(controls, "edit_avance_plat"), parameters);

        AddQueryParameter(ref fieldList, "zabezdepoz_narah", "zabezdepoznarah", Reports1NFUtils.GetEditNumeric(controls, "edit_zabezdepoz_narah"), parameters);
        AddQueryParameter(ref fieldList, "zabezdepoz_saldo", "zabezdepozsaldo", Reports1NFUtils.GetEditNumeric(controls, "edit_zabezdepoz_saldo"), parameters);
        AddQueryParameter(ref fieldList, "zabezdepoz_nadhod", "zabezdepoznadhod", Reports1NFUtils.GetEditNumeric(controls, "edit_zabezdepoz_nadhod"), parameters);
        AddQueryParameter(ref fieldList, "use_zabezdepoz", "usezabezdepoz", Reports1NFUtils.GetCheckBoxValue(controls, "edit_use_zabezdepoz") ? 1 : 0, parameters);
        AddQueryParameter(ref fieldList, "zabezdepoz_debt", "zabezdepozdebt", Reports1NFUtils.GetEditNumeric(controls, "edit_zabezdepoz_debt"), parameters);
        AddQueryParameter(ref fieldList, "total_pereplata", "totalpereplata", Reports1NFUtils.GetEditNumeric(controls, "edit_total_pereplata"), parameters);

        for (   int k = 0; k <= 13; k++)
        {
            AddQueryParameter(ref fieldList, "debtkvart_" + k, "vdebtkvart_" + k, Reports1NFUtils.GetEditNumeric(controls, "edit_debtkvart_" + k), parameters);
        }

        //AddQueryParameter(ref fieldList, "budget_narah_50_uah", "budg50n", Reports1NFUtils.GetEditNumeric(controls, "EditBudgetNarah50_orndpymnt"), parameters);
        //AddQueryParameter(ref fieldList, "budget_zvit_50_uah", "budg50z", Reports1NFUtils.GetEditNumeric(controls, "EditBudgetZvit50_orndpymnt"), parameters);
        //AddQueryParameter(ref fieldList, "budget_prev_50_uah", "budg50p", Reports1NFUtils.GetEditNumeric(controls, "EditBudgetPrev50_orndpymnt"), parameters);
        //AddQueryParameter(ref fieldList, "budget_debt_50_uah", "budg50d", Reports1NFUtils.GetEditNumeric(controls, "EditBudgetDebt50_orndpymnt"), parameters);
        //AddQueryParameter(ref fieldList, "budget_debt_30_50_uah", "budg50o", Reports1NFUtils.GetEditNumeric(controls, "EditBudgetDebtOld50_orndpymnt"), parameters);

        AddQueryParameter(ref fieldList, "is_discount", "isout", Reports1NFUtils.GetCheckBoxValue(controls, "CheckRenterIsOut") ? 1 : 0, parameters);
        AddQueryParameter(ref fieldList, "is_debt_exists", "isdebt", Reports1NFUtils.GetCheckBoxValue(controls, "CheckNoDebt") ? 0 : 1, parameters);
        //AddQueryParameter(ref fieldList, "is_special_organization", "isSpecOrg", Reports1NFUtils.GetCheckBoxValue(controls, "CheckIsSpecialOrganization") ? 1 : 0, parameters);

		AddQueryParameter(ref fieldList, "zvilneno_percent", "zvilnenopercent", Reports1NFUtils.GetEditNumeric(controls, "edit_zvilneno_percent"), parameters);
		AddQueryParameter(ref fieldList, "zvilneno_date1", "zvilnenodate1", Reports1NFUtils.GetDateValue(controls, "edit_zvilneno_date1"), parameters);
		AddQueryParameter(ref fieldList, "zvilneno_date2", "zvilnenodate2", Reports1NFUtils.GetDateValue(controls, "edit_zvilneno_date2"), parameters);

		AddQueryParameter(ref fieldList, "zvilbykmp_percent", "zvilbykmppercent", Reports1NFUtils.GetEditNumeric(controls, "edit_zvilbykmp_percent"), parameters);
		AddQueryParameter(ref fieldList, "zvilbykmp_date1", "zvilbykmpdate1", Reports1NFUtils.GetDateValue(controls, "edit_zvilbykmp_date1"), parameters);
		AddQueryParameter(ref fieldList, "zvilbykmp_date2", "zvilbykmpdate2", Reports1NFUtils.GetDateValue(controls, "edit_zvilbykmp_date2"), parameters);

        AddQueryParameter(ref fieldList, "povidoleno1_date", "povidoleno1date", Reports1NFUtils.GetDateValue(controls, "edit_povidoleno1_date"), parameters);
        AddQueryParameter(ref fieldList, "povidoleno1_num", "povidoleno1num", Reports1NFUtils.GetEditText(controls, "edit_povidoleno1_num"), parameters);
        AddQueryParameter(ref fieldList, "povidoleno2_date", "povidoleno2date", Reports1NFUtils.GetDateValue(controls, "edit_povidoleno2_date"), parameters);
        AddQueryParameter(ref fieldList, "povidoleno2_num", "povidoleno2num", Reports1NFUtils.GetEditText(controls, "edit_povidoleno2_num"), parameters);
        AddQueryParameter(ref fieldList, "povidoleno3_date", "povidoleno3date", Reports1NFUtils.GetDateValue(controls, "edit_povidoleno3_date"), parameters);
        AddQueryParameter(ref fieldList, "povidoleno3_num", "povidoleno3num", Reports1NFUtils.GetEditText(controls, "edit_povidoleno3_num"), parameters);
        AddQueryParameter(ref fieldList, "povidoleno4_date", "povidoleno4date", Reports1NFUtils.GetDateValue(controls, "edit_povidoleno4_date"), parameters);
        AddQueryParameter(ref fieldList, "povidoleno4_num", "povidoleno4num", Reports1NFUtils.GetEditText(controls, "edit_povidoleno4_num"), parameters);

        AddQueryParameter(ref fieldList, "debt_total", "debttot", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebtTotal"), parameters);
        AddQueryParameter(ref fieldList, "debt_zvit", "debtzvit", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebtZvit"), parameters);
        AddQueryParameter(ref fieldList, "debt_3_month", "debt3m", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebt3Month"), parameters);
        AddQueryParameter(ref fieldList, "debt_12_month", "debt12m", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebt12Month"), parameters);
        AddQueryParameter(ref fieldList, "debt_3_years", "debt3y", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebt3Years"), parameters);
        AddQueryParameter(ref fieldList, "debt_over_3_years", "debtover", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebtOver3Years"), parameters);
        AddQueryParameter(ref fieldList, "debt_v_mezhah_vitrat", "debtmv", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebtVMezhahVitrat"), parameters);
        AddQueryParameter(ref fieldList, "debt_spysano", "debtsp", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebtSpysano"), parameters);
        AddQueryParameter(ref fieldList, "avance_debt", "avancedebt", Reports1NFUtils.GetEditNumeric(controls, "Edit_avance_debt"), parameters);

        AddQueryParameter(ref fieldList, "num_zahodiv_total", "numzaht", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionNumZahodivTotal"), parameters);
        AddQueryParameter(ref fieldList, "num_zahodiv_zvit", "numzahz", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionNumZahodivZvit"), parameters);
        AddQueryParameter(ref fieldList, "num_pozov_total", "numpozt", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionNumPozovTotal"), parameters);
        AddQueryParameter(ref fieldList, "num_pozov_zvit", "numpozz", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionNumPozovZvit"), parameters);
        AddQueryParameter(ref fieldList, "num_pozov_zadov_total", "zadovt", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionPozovZadovTotal"), parameters);
        AddQueryParameter(ref fieldList, "num_pozov_zadov_zvit", "zadovz", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionPozovZadovZvit"), parameters);
        AddQueryParameter(ref fieldList, "num_pozov_vikon_total", "vikont", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionPozovVikonTotal"), parameters);
        AddQueryParameter(ref fieldList, "num_pozov_vikon_zvit", "vikonz", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionPozovVikonZvit"), parameters);

        AddQueryParameter(ref fieldList, "debt_pogasheno_total", "dpaytot", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebtPayedTotal"), parameters);
        AddQueryParameter(ref fieldList, "debt_pogasheno_zvit", "dpayzvit", Reports1NFUtils.GetEditNumeric(controls, "EditCollectionDebtPayedZvit"), parameters);

        // System parameters
        AddQueryParameter(ref fieldList, "modify_date", "mdt", DateTime.Now, parameters);
        AddQueryParameter(ref fieldList, "modified_by", "mby", username.Left(64), parameters);



        int idInReport = 0;
        using (SqlCommand cmd = new SqlCommand("SELECT id FROM reports1nf_arenda_payments WHERE report_id = @rid AND arenda_id = @aid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
            using (SqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                    idInReport = r.IsDBNull(0) ? 0 : (int)r.GetValue(0);
                r.Close();
            }
        }




        //int cnt = 0;
        //using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM reports1nf_arenda_payments WHERE report_id = @rid AND arenda_id = @aid", connection))
        //{
        //    cmd.Parameters.Add(new SqlParameter("rid", ReportID));
        //    cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
        //    using (SqlDataReader r = cmd.ExecuteReader())
        //    {
        //        if (r.Read())
        //            cnt = r.IsDBNull(0) ? 0 : (int)r.GetValue(0);
        //        r.Close();
        //    }
        //}
        //if (cnt > 1)
        //{
        //    List<string> toDel = new List<string>();
        //    using (SqlCommand cmd = new SqlCommand("SELECT id FROM reports1nf_arenda_payments WHERE report_id = @rid AND arenda_id = @aid", connection))
        //    {
        //        cmd.Parameters.Add(new SqlParameter("rid", ReportID));
        //        cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
        //        using (SqlDataReader r = cmd.ExecuteReader())
        //        {
        //            while (r.Read())
        //            {
        //                int id = r.IsDBNull(0) ? 0 : (int)r.GetValue(0);
        //                if (id > 0)
        //                    toDel.Add(id.ToString());
        //                cnt--;
        //                if (cnt == 1)
        //                    break;
        //            }
        //            r.Close();
        //        }
        //    }
        //    using (SqlCommand cmd = new SqlCommand("DELETE FROM reports1nf_arenda_payments WHERE id in (" + string.Join(",", toDel.ToArray()) + ")"))
        //    {
        //        cmd.ExecuteNonQuery();
        //    }


        //}

        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_payments SET " +
            fieldList + " WHERE report_id = @rid AND arenda_id = @aid AND id = @id", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.Parameters.Add(new SqlParameter("aid", RentAgreementID));
            cmd.Parameters.Add(new SqlParameter("id", idInReport));

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            cmd.ExecuteNonQuery();
        }

		SavePhotoChanges();
//////
		}
        catch (Exception ex)
        {
            var lognet = log4net.LogManager.GetLogger("ReportWebSite");
            lognet.Debug("--------------- OrgRentAgreement SaveChanges ----------------", ex);
            throw ex;
        }


    }

    protected void AddQueryParameter(ref string fieldList, string fieldName, string paramName,
        object value, Dictionary<string, object> parameters)
    {
        bool valueExists = false;

        if (value is int)
        {
            valueExists = ((int)value) >= 0;
        }
        else if (value is string)
        {
            valueExists = ((string)value).Length > 0;
        }
        else if (value is decimal)
        {
            valueExists = true;
        }
        else if (value is DateTime)
        {
            valueExists = true;
        }

        if (fieldList.Length > 0)
        {
            fieldList += ", ";
        }

        if (valueExists)
        {
            fieldList += fieldName + " = @" + paramName;
            parameters[paramName] = value;
        }
        else
        {
            fieldList += fieldName + " = NULL";
        }
    }

    //protected void ValidateEditors()
    //{
    //    ErrorMessages.Clear();
    //    ASPxTextEdit.ValidateEditorsInContainer(CPMainPanel);
    //}

    //protected void OnValidation(object sender, ValidationEventArgs e)
    //{

    //    ASPxTextEdit edit = sender as ASPxTextEdit;
    //    if (reqEditors.Contains(edit.ID))
    //    {
    //        if (edit.Text.Trim() == "" || edit.Text == "<НЕ ЗАДАНО>")
    //        {
    //            if (mode == "send") e.IsValid = false;
    //            FormIsValid = false;
    //            ErrorMessages.Add(e.ErrorText);
    //        }
    //    }
    //}

   

    protected void CPMainPanel_Callback(object sender, CallbackEventArgsBase e)
    {
//////
        try
        {
            if (e.Parameter.StartsWith("save:"))
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                AddressChange(connection);

                SaveChanges(connection);

                connection.Close();
            }
        }
        else if (e.Parameter.StartsWith("send:"))
        {
            validator.ValidateUI();
            //!!ValidatePaymentForm();
            this.errorForm.DataSource = validator.FormatErrorDataSource();
            this.errorForm.DataBind();

            // Copy all information about address and object to the primary database
            SqlConnection connection = Utils.ConnectToDatabase();
            if (connection != null)
            {
                AddressChange(connection);

                // Save the form before sending it to DKV
                SaveChanges(connection);

                int agreementId = RentAgreementID;

                if (validator.ValidateDB(connection, "reports1nf_arenda", string.Format("report_id = {0} and id = {1}", ReportID, agreementId), true))
                {
                    Reports1NFUtils.SendRentAgreement(connection, /*connection1NF,*/ ReportID, ref agreementId);

                    RentAgreementID = agreementId;

                    connection.Close();
                }
                else
                {
                    StatusForm.DataBind();
                    connection.Close();
                    return;
                }
            }
            else
            {
                return;
            }



        }
        else if (e.Parameter.StartsWith("clear:"))
        {
            // No need to do anything. DataBind() will do the trick
        }

        StatusForm.DataBind();
        AddressForm.DataBind();
        OrganizationsForm.DataBind();
        PaymentForm.DataBind();
        CollectionForm.DataBind();
        InsuranceForm.DataBind();
        FormViewState.DataBind();

        // Rebind the grid of rent decisions
        object view = SqlDataSourceDecisions.Select(new DataSourceSelectArguments());

        if (view is DataView)
        {
            DecisionsDataSource = (view as DataView).ToTable();
        }

        GridViewDecisions.DataSource = DecisionsDataSource;
        GridViewDecisions.DataBind();

		// Rebind the grid of rent subleases
		view = SqlDataSourceSubleases.Select(new DataSourceSelectArguments());

		if (view is DataView)
		{
			SubleasesDataSource = (view as DataView).ToTable();
		}

		GridViewSubleases.DataSource = SubleasesDataSource;
		GridViewSubleases.DataBind();

		// Rebind the grid of objects
		view = SqlDataSourceNotes.Select(new DataSourceSelectArguments());

        if (view is DataView)
        {
            NotesDataSource = (view as DataView).ToTable();
        }

        GridViewNotes.DataSource = NotesDataSource;
        GridViewNotes.DataBind();

        CalculateTotals();
        EnableControlsBasingOnUserRole();
//////
        }
        catch (Exception ex)
        {
            var lognet = log4net.LogManager.GetLogger("ReportWebSite");
            lognet.Debug("--------------- OrgRentAgreement CPMainPanel_Callback ----------------", ex);
            throw ex;
        }
    }

    void ValidatePaymentForm()
    {
        Dictionary<string, Control> controls = new Dictionary<string, Control>();
        Reports1NFUtils.GetAllControls(PaymentForm, controls);

        var is_discount = Reports1NFUtils.GetCheckBoxValue(controls, "CheckRenterIsOut");
        var zvilneno_percent = Reports1NFUtils.GetEditNumeric(controls, "edit_zvilneno_percent");
        var zvilneno_date1 = Reports1NFUtils.GetDateValue(controls, "edit_zvilneno_date1");
        var zvilneno_date2 = Reports1NFUtils.GetDateValue(controls, "edit_zvilneno_date2");
        if (is_discount)
        {
            if (zvilneno_percent == null || zvilneno_date1 == null || zvilneno_date2 == null)
            {
                throw new Exception("Всі поля \"Звільнено від сплати орендної плати на\" повинні бути заповнені");
            }
        }
    }

    protected void CPCommentViewerPanel_Callback(object sender, CallbackEventArgsBase e)
    {
        // Get the commented control ID and Title
        int dividerPos = e.Parameter.IndexOf(';');

        if (dividerPos > 0)
        {
            ReportCommentViewer1.SetCommentTargetText(e.Parameter.Substring(dividerPos + 1));
        }
    }

    public string EvaluateSignature(object modifiedBy, object modifyDate)
    {
        string userName = (modifiedBy is string) ? (string)modifiedBy : Resources.Strings.SignatureUnknownUser;
        string date = (modifyDate is DateTime) ? ((DateTime)modifyDate).ToShortDateString() + " " + ((DateTime)modifyDate).ToShortTimeString() : Resources.Strings.SignatureUnknownDate;

        return string.Format(Resources.Strings.SignatureObjCard, userName, date);
    }

    public bool IsHistoryButtonVisible()
    {
        return Request.QueryString["aid"] == null ? false : true;
    }

    #region Organization pickers

    private string GiverOrgZkpoPattern
    {
        get
        {
            object pattern = Session[GetPageUniqueKey() + "_GiverOrgZkpoPattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session[GetPageUniqueKey() + "_GiverOrgZkpoPattern"] = value;
        }
    }

    private string GiverOrgNamePattern
    {
        get
        {
            object pattern = Session[GetPageUniqueKey() + "_GiverOrgNamePattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session[GetPageUniqueKey() + "_GiverOrgNamePattern"] = value;
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

    protected void SqlDataSourceOrgSearchGiver_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string orgName = GiverOrgNamePattern;
        string zkpo = GiverOrgZkpoPattern;

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

        object userOrgId = ViewState["OrgRentAgreement_UserOrgID"];

        if (userOrgId is int)
        {
            e.Command.Parameters["@balans_org"].Value = userOrgId;
        }
        else
        {
            int reportRdaDistrictId = -1;
            e.Command.Parameters["@balans_org"].Value = Reports1NFUtils.GetReportOrganizationId(ReportID, ref reportRdaDistrictId);
        }

        e.Command.Parameters["@aid"].Value = RentAgreementID;
        e.Command.Parameters["@rep_id"].Value = ReportID;
    }

    protected void SqlDataSourceOrgSearchRenter_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string orgName = RenterOrgNamePattern;
        string zkpo = RenterOrgZkpoPattern;

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

        e.Command.Parameters["@aid"].Value = RentAgreementID;
        e.Command.Parameters["@rep_id"].Value = ReportID;
    }

    protected void ComboGiverOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] parts = e.Parameter.Split(new char[] { '|' });

        if (parts.Length == 2)
        {
            GiverOrgZkpoPattern = parts[0].Trim();
            GiverOrgNamePattern = parts[1].Trim().ToUpper();

            (sender as ASPxComboBox).DataBind();

            if ((sender as ASPxComboBox).Items.Count > 0)
                (sender as ASPxComboBox).SelectedIndex = 0;
        }
    }

    protected void ComboRenterOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("create_org:") || e.Parameter.StartsWith("edit_org:"))
        {
            int newId = 0;
            Control ourPopUp;
            var new_id_str = "";
            if (e.Parameter.StartsWith("create_org:"))
            {
                ourPopUp = (Control)Utils.FindControlRecursive(OrganizationsForm, "PopupAddRenterOrg");
                new_id_str = ((HiddenField)Utils.FindControlRecursive(ourPopUp, "NewOrgId")).Value;
                Int32.TryParse(new_id_str, out newId);
            }
            else
            {
                ourPopUp = (Control)Utils.FindControlRecursive(OrganizationsForm, "PopupEditRenterOrg");
                new_id_str = ((HiddenField)Utils.FindControlRecursive(ourPopUp, "OldOrgId")).Value;
                Int32.TryParse(new_id_str, out newId);
            }

            ASPxComboBox ComboRenterOrg = (ASPxComboBox)Utils.FindControlRecursive(OrganizationsForm, "ComboRenterOrg");
            ComboRenterOrg.DataBind();
            ComboRenterOrg.SelectedItem = ComboRenterOrg.Items.FindByValue(newId);

        }
        else
        {
            string[] parts = e.Parameter.Split(new char[] { '|' });

            if (parts.Length == 2)
            {
                RenterOrgZkpoPattern = parts[0].Trim();
                RenterOrgNamePattern = parts[1].Trim().ToUpper();

                (sender as ASPxComboBox).DataBind();

                if ((sender as ASPxComboBox).Items.Count > 0)
                    (sender as ASPxComboBox).SelectedIndex = 0;
            }
        }
    }

    protected void CPPopUp_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("create_org:") || e.Parameter.StartsWith("edit_org:"))
        {
            ASPxLabel label;
            Control ourPopUp;
            HiddenField HdnNewOrgId;
            if (e.Parameter.StartsWith("create_org:"))
            {
                ourPopUp = (Control)Utils.FindControlRecursive(OrganizationsForm, "PopupAddRenterOrg");
                HdnNewOrgId = (HiddenField)Utils.FindControlRecursive(ourPopUp, "NewOrgId");
                label = (ASPxLabel)Utils.FindControlRecursive(ourPopUp, "LabelOrgCreationError");
            }
            else
            {
                ourPopUp = (Control)Utils.FindControlRecursive(OrganizationsForm, "PopupEditRenterOrg");
                HdnNewOrgId = (HiddenField)Utils.FindControlRecursive(ourPopUp, "OldOrgId");
                label = (ASPxLabel)Utils.FindControlRecursive(ourPopUp, "LabelOrgEditError");
            }

            int newOrgId = 0;
            Int32.TryParse(HdnNewOrgId.Value, out newOrgId);

            //HdnNewOrgId.Value = "";

            string errorMessage = "";

            string fullName = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxFullNameOrg")).Text.Trim();
            string zkpo = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxZkpoCodeOrg")).Text.Trim();
            string shortName = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxShortNameOrg")).Text.Trim();

            int district = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxDistrictOrg")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxDistrictOrg")).Value : -1;
            string street = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxStreetNameOrg")).Text; // Элемент в интерфейсе на самом деле не ASPxComboBox, а ASPxTextBox. Из-за этого сохранить данные невозможно.
            string numOfHouse = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxAddrNomerOrg")).Text.Trim();
            string zip = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxAddrZipCodeOrg")).Text.Trim();
            string korpus = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxAddrKorpusFrom")).Text.Trim();

            int status = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxStatusOrg")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxStatusOrg")).Value : -1;
            int formVlasn = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxFormVlasnOrg")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxFormVlasnOrg")).Value : -1;
            int industry = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxIndustryOrg")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxIndustryOrg")).Value : -1;
            int occupation = ((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxOccupationFrom")).Value is int ? (int)((ASPxComboBox)Utils.FindControlRecursive(ourPopUp, "ComboBoxOccupationFrom")).Value : -1;

            string directorFio = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxDirectorFioOrg")).Text;
            string directorPhone = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxDirectorPhoneOrg")).Text;
            string directorEmail = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxDirectorEmailOrg")).Text;
            string buhgalterFio = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxBuhgalterFioOrg")).Text;
            string buhgalterPhone = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxBuhgalterPhoneOrg")).Text;
            string fax = ((ASPxTextBox)Utils.FindControlRecursive(ourPopUp, "TextBoxFax")).Text;

            if (fullName == "")
                errorMessage = "Необхідно заповнити повну назву організації.";
            else if (zkpo == "")
                errorMessage = "Необхідно заповнити Код ЄДРПОУ.";
            else if (shortName == "")
                errorMessage = "Необхідно заповнити коротку назву організації.";
            else if (district == -1)
                errorMessage = "Необхідно вибрати район.";
            else if (street == "")
                errorMessage = "Необхідно заповнити вулицю.";
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

            if (errorMessage == "")
            {
                //var newOrgId = 0;

                //FbConnection connection = Utils.ConnectTo1NF();

                //if (connection != null)
                //{
                    if (newOrgId != 0)
                    {
                        newOrgId = RishProjectExport.Update1NFOrganization(
                            //connection,
                            newOrgId,
                            fullName,
                            shortName,
                            zkpo,
                            industry,
                            occupation,
                            formVlasn,
                            status,
                            -1,
                            -1,
                            -1,
                            directorFio,
                            directorPhone,
                            directorEmail,
                            buhgalterFio,
                            buhgalterPhone,
                            fax,
                            "", // kved
                            district,
                            street,
                            numOfHouse,
                            korpus,
                            zip,
                            //true,
                            out errorMessage);
                    }
                    else
                    {
                        newOrgId = RishProjectExport.CreateNew1NFOrganization(
                            //connection,
                            fullName,
                            shortName,
                            zkpo,
                            industry,
                            occupation,
                            formVlasn,
                            status,
                            -1,
                            -1,
                            -1,
                            directorFio,
                            directorPhone,
                            directorEmail,
                            buhgalterFio,
                            buhgalterPhone,
                            fax,
                            "", // kved
                            district,
                            street,
                            numOfHouse,
                            korpus,
                            zip,
                            //true,
                            out errorMessage);
                    }

                    //connection.Close();

                    if (newOrgId > 0)
                    {
                        HdnNewOrgId.Value = newOrgId.ToString();

                        ASPxTextBox EditRenterOrgZKPO = (ASPxTextBox)Utils.FindControlRecursive(OrganizationsForm, "EditRenterOrgZKPO");
                        EditRenterOrgZKPO.Text = zkpo;
                        RenterOrgZkpoPattern = zkpo;

                        ASPxTextBox EditRenterOrgName = (ASPxTextBox)Utils.FindControlRecursive(OrganizationsForm, "EditRenterOrgName");
                        EditRenterOrgName.Text = fullName;
                        RenterOrgNamePattern = fullName;
                    }
                //}
            }
            else
            {
                label.Text = errorMessage;
                label.ClientVisible = (errorMessage.Length > 0);
            }
        }
    }

    protected void errorForm_DataBound(object sender, EventArgs e)
    {
        Repeater r = (Repeater)Utils.FindControlRecursive(errorForm, "errorList");
        if (r != null)
        {
            r.DataSource = errorForm.DataSource;
            r.DataBind();
        }
    }

	#endregion (Organization pickers)


	#region Photos

	protected Guid PhotoFolderID
	{
		get
		{
			//return string.IsNullOrEmpty(TempFolderIDField.Value) ? Guid.Empty : new Guid(TempFolderIDField.Value);

			object val = Session[GetPageUniqueKey() + "_PHOTO_GUID"];

			if (val is Guid)
			{
				return (Guid)val;
			}

			return Guid.Empty;
		}

		set
		{
			Session[GetPageUniqueKey() + "_PHOTO_GUID"] = value;
			//TempFolderIDField.Value = value.ToString(); ;
		}
	}

	private string TempPhotoFolder()
	{
		if (PhotoFolderID != Guid.Empty)
		{
			string agreementIdStr = Request.QueryString["aid"];
			string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
			string destFolder = Path.Combine(photoRootPath, "1NF_" + agreementIdStr + "_" + PhotoFolderID.ToString()).ToLower();
			return destFolder;
		}
		else
			return string.Empty;
	}

	private void PrepareTempPhotoFolder()
	{
		string agreementIdStr = Request.QueryString["aid"];

		if (PhotoFolderID == Guid.Empty)
		{
			PhotoFolderID = Guid.NewGuid();

			CopySourceFiles(agreementIdStr);
		}

		BindImageGallery(agreementIdStr);
	}

	private void CopySourceFiles(string agreementIdStr)
	{
		string photoRootPath = LLLLhotorowUtils.ImgContentRootFolder;
		string destFolder = TempPhotoFolder();

		var connection = Utils.ConnectToDatabase();
        var transaction = connection.BeginTransaction();
        using (var cmd = new SqlCommand("select id, file_name, file_ext from reports1nf_arendaphotos where arenda_id = @aid", connection, transaction))
		{
			cmd.Parameters.AddWithValue("aid", agreementIdStr);
			using (SqlDataReader r = cmd.ExecuteReader())
			{
				while (r.Read())
				{
					int id = r.GetInt32(0);
					string file_name = r.GetString(1);
					string file_ext = r.GetString(2);

					string sourceFileToCopy = Path.Combine(photoRootPath, "1NFARENDA", agreementIdStr, id.ToString() + file_ext);
                    if (LLLLhotorowUtils.Exists(sourceFileToCopy, connection, transaction))
                    {
						string destFileToCopy = Path.Combine(destFolder, PhotoUtils.DbFilename2LocalFilename(file_name, file_ext));
                        LLLLhotorowUtils.Delete(destFileToCopy, connection, transaction);
                        LLLLhotorowUtils.Copy(sourceFileToCopy, destFileToCopy, connection, transaction);
					}
				}

				r.Close();
			}
		}
        transaction.Commit();
        connection.Close();
    }

    private void BindImageGallery(string agreementIdStr)
	{
		ObjectDataSourceBalansPhoto.SelectParameters["recordID"].DefaultValue = agreementIdStr;
		ObjectDataSourceBalansPhoto.SelectParameters["tempGuid"].DefaultValue = PhotoFolderID.ToString();
		imageGalleryDemo.DataSourceID = "ObjectDataSourceBalansPhoto";
		imageGalleryDemo.DataBind();

	}
	protected void TempFolderIDField_ValueChanged(object sender, EventArgs e)
	{
		//PhotoFolderID = new Guid(TempFolderIDField.Value);
	}
	protected void btnUpload_Click(object sender, EventArgs e)
	{

	}

	protected void ContentCallback_Callback(object sender, CallbackEventArgsBase e)
	{


		if (e.Parameter.ToLower().StartsWith("deleteimage:"))
		{
			DeleteImage(e.Parameter.Split(':')[1], string.Empty);
			BindImageGallery(Request.QueryString["aid"]);
			//imageGalleryDemo.DataBind();
			//ASPxFileManagerPhotoFiles.Refresh();
		}
	}
	protected void imageGalleryDemo_DataBound(object sender, EventArgs e)
	{
		if ((imageGalleryDemo.PageIndex > imageGalleryDemo.PageCount) && (IsCallback))
			imageGalleryDemo.PageIndex = 0;
	}

	protected void imageGalleryDemo_CustomCallback(object sender, CallbackEventArgsBase e)
	{
		ContentCallback_Callback(sender, e);
	}

	protected void ASPxCallbackPanelImageGallery_Callback(object sender, CallbackEventArgsBase e)
	{
		//if (!IsCallback)
		{
			if (e.Parameter.ToLower() == "refreshphoto:")
			{
				//imageGalleryDemo.DataBind();
				BindImageGallery(Request.QueryString["aid"]);

			}
		}
	}

	protected void delete_Callback(object source, CallbackEventArgs e)
	{
		string indexStr = e.Parameter;
		string imageUrl = string.Empty;

		imageUrl = DeleteImage(indexStr, imageUrl);
		e.Result = imageUrl;
	}

	private string DeleteImage(string indexStr, string imageUrl)
	{
		string archiveIdStr = Request.QueryString["arid"];
		if (archiveIdStr != null && archiveIdStr.Length > 0)
			throw new Exception("Ви не можете видалити фото з архівного стану.");

        var connection = Utils.ConnectToDatabase();
        var transaction = connection.BeginTransaction();
        string arendaId = Request.QueryString["aid"];

		if (imageGalleryDemo != null)
		{
			if (imageGalleryDemo.Items.Count == 0)
				BindImageGallery(arendaId);

			ImageGalleryItem item = imageGalleryDemo.Items[int.Parse(indexStr)];
			FileAttachment drv = (FileAttachment)item.DataItem;
			string fileExt = Path.GetExtension(drv.Name);
            LLLLhotorowUtils.Delete(drv.Name, connection, transaction);

			imageUrl = item.ImageUrl;
		}

        transaction.Commit();
        connection.Close();

        return imageUrl;
	}

	protected void ASPxUploadPhotoControl_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
	{
		string agreementIdStr = Request.QueryString["aid"];
		string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
		string serverLocal1NFObjectFolder = Path.Combine(photoRootPath, "1NFARENDA", agreementIdStr);
		string fullPath = string.Empty;
        PhotoUtils.AddUploadedFile(TempPhotoFolder(), e.UploadedFile.FileName, e.UploadedFile.FileBytes);
	}


	private void SavePhotoChanges()
	{
		string agreementIdStr = Request.QueryString["aid"];
		Int32 newId = 0;
		string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
        string local1NFObjectFolder = Path.Combine(photoRootPath, "1NFARENDA", agreementIdStr);

        SqlConnection connection = Utils.ConnectToDatabase();
        SqlTransaction trans = connection.BeginTransaction();

        foreach (string fileToDelete in LLLLhotorowUtils.GetFiles(local1NFObjectFolder, connection, trans))
        {
            LLLLhotorowUtils.Delete(fileToDelete, connection, trans);
        }

        try
        {
			using (SqlCommand cmd = new SqlCommand("delete from reports1nf_arendaphotos where arenda_id = @aid", connection, trans))
			{
				cmd.Parameters.AddWithValue("aid", int.Parse(agreementIdStr));
				cmd.ExecuteNonQuery();
			}

            var allfiles = LLLLhotorowUtils.GetFiles(TempPhotoFolder(), connection, trans);
            foreach (string filePath in allfiles)
			{
				var dbfile = PhotoUtils.LocalFilename2DbFilename(filePath);
				string fullPath = string.Empty;

				SqlCommand cmd = new SqlCommand("INSERT INTO reports1nf_arendaphotos (arenda_id, file_name, file_ext, user_id, create_date) VALUES (@aid, @filename, @fileext, @usrid, @createdate); ; SELECT CAST(SCOPE_IDENTITY() AS int)", connection, trans);
				cmd.Parameters.Add(new SqlParameter("aid", int.Parse(agreementIdStr)));
				cmd.Parameters.Add(new SqlParameter("filename", dbfile.file_name));
				cmd.Parameters.Add(new SqlParameter("fileext", dbfile.file_ext));
				cmd.Parameters.Add(new SqlParameter("usrid", (Guid)Membership.GetUser().ProviderUserKey));
				cmd.Parameters.Add(new SqlParameter("createdate", DateTime.Now));
				newId = (Int32)cmd.ExecuteScalar();

				fullPath = Path.Combine(local1NFObjectFolder, newId.ToString() + Path.GetExtension(filePath));
                LLLLhotorowUtils.Copy(filePath, fullPath, connection, trans);
			}

			trans.Commit();
			connection.Close();
		}
		catch
		{
			trans.Rollback();
			throw;
		}
	}


    protected void PdfImageBuild_Click(object sender, EventArgs e)
    {
        var agreementId = Int32.Parse(Request.QueryString["aid"]);

        var fname = @"фото_" + agreementId + ".pdf";
        var bytes = new OrgRentAgreementPhotosPdfBulder().Go(agreementId, TempPhotoFolder());

        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fname + "\"");
        Response.BinaryWrite(bytes);

        Response.Flush();
        Response.End();
    }

    #endregion

    protected void SqlDataSourceFreeSquare_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["rid"]))
            e.Command.Parameters["@report_id"].Value = int.Parse(Request.QueryString["rid"]);

        if (!string.IsNullOrEmpty(Request.QueryString["aid"]))
            e.Command.Parameters["@arenda_id"].Value = int.Parse(Request.QueryString["aid"]);

        e.Command.Parameters["@modify_date"].Value = DateTime.Now;
        MembershipUser user = Membership.GetUser();
        e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;

        if (e.Command.Parameters["@water"].Value == null)
            e.Command.Parameters["@water"].Value = 0;

        if (e.Command.Parameters["@heating"].Value == null)
            e.Command.Parameters["@heating"].Value = 0;

        //if (e.Command.Parameters["@power"].Value == null)
        //e.Command.Parameters["@power"].Value = 0;

        if (e.Command.Parameters["@gas"].Value == null)
            e.Command.Parameters["@gas"].Value = 0;

        if (e.Command.Parameters["@is_solution"].Value == null)
            e.Command.Parameters["@is_solution"].Value = 0;

        if (e.Command.Parameters["@is_included"].Value == null)
            e.Command.Parameters["@is_included"].Value = 0;
    }

    protected void SqlDataSourceFreeSquare_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["rid"]))
            e.Command.Parameters["@report_id"].Value = int.Parse(Request.QueryString["rid"]);

        if (!string.IsNullOrEmpty(Request.QueryString["aid"]))
            e.Command.Parameters["@arenda_id"].Value = int.Parse(Request.QueryString["aid"]);

        e.Command.Parameters["@modify_date"].Value = DateTime.Now;
        MembershipUser user = Membership.GetUser();
        e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;

        var b = 10;
    }

    protected void ASPxGridViewFreeSquare_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        var komis_protocol = (e.OldValues["komis_protocol"] == null ? "" : e.OldValues["komis_protocol"].ToString().Trim());
        if (komis_protocol != "" && !komis_protocol.StartsWith("0"))
        {
            e.RowError = "Об'єкт погоджено орендодавцем! Усі зміни ТІЛЬКИ з його дозволу за тел: 202-61-76, 202-61-77, 202-61-96 !";
            //e.Errors.Add(ASPxGridViewFreeSquare.Columns["total_free_sqr"], "AAAAAAAAA");
            //var ggg = e.HasErrors;
            return;
        }

        foreach (GridViewColumn column in ASPxGridViewFreeSquare.Columns)
        {
            GridViewDataColumn dataColumn = column as GridViewDataColumn;
            if (dataColumn == null) continue;
            string fieldName = dataColumn.FieldName.ToLower();

            if (fieldName == "total_free_sqr" && e.NewValues[dataColumn.FieldName] == null)
            {
                e.Errors[dataColumn] = "Заповніть загальну площу вільного приміщення";
            }

            if (fieldName == "total_free_sqr")
            {
                var svalue = e.NewValues[dataColumn.FieldName];
                if (svalue != null)
                {
                    var val = (decimal)svalue;
                    if (val < 2.0M)
                    {
                        e.Errors[dataColumn] = "Загальна площа об’єкта не може бути менше 2 кв.м.";
                    }
                }
            }

            if (fieldName == "free_sqr_condition_id" && e.NewValues[dataColumn.FieldName] == null)
            {
                e.Errors[dataColumn] = "Вкажіть стан вільного приміщення";
            }

            //if (fieldName == "free_sqr_condition_id")
            //{
            //	var svalue = e.NewValues[dataColumn.FieldName];
            //	if (svalue != null)
            //	{
            //		var val = (int)svalue;
            //		if (!(new[] { 2, 7, 11 }.Contains(val)))
            //		{
            //			e.Errors[dataColumn] = "Стан вільного приміщення може мати тільки значення: ЗАДОВІЛЬНИЙ, ДОБРИЙ, ПОТРЕБУЄ РЕМОНТУ";
            //		}
            //	}
            //}


            if (fieldName == "floor" && e.NewValues[dataColumn.FieldName] == null)
            {
                e.Errors[dataColumn] = "Заповніть місце розташування вільного приміщення (поверх)";
            }

            if (fieldName == "possible_using" && e.NewValues[dataColumn.FieldName] == null)
            {
                e.Errors[dataColumn] = "Заповніть можливе використання вільного приміщення";
            }

            //if (fieldName == "using_possible_id" && e.NewValues[dataColumn.FieldName] == null)
            //{
            //	e.Errors[dataColumn] = "Заповніть можливе використання вільного приміщення";
            //}
        }

        if (e.Errors.Count > 0)
            e.RowError = "Заповніть обов'язкові поля.";
    }

    protected void ASPxGridViewFreeSquare_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        if (!ASPxGridViewFreeSquare.IsNewRowEditing)
        {
            ASPxGridViewFreeSquare.DoRowValidation();
        }
    }

    protected void ASPxGridViewFreeSquare_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
    {
        e.NewValues["is_included"] = true;
        
        //var edit = CollectionForm.FindControl("EditCollectionDebtTotal") as ASPxSpinEdit;
        //e.NewValues["orend_plat_borg"] = edit.Value;
    }

    protected void ObjectDataSourcePhotoFiles_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        if (Request.Cookies["RecordID"] != null)
            e.InputParameters["RecordID"] = Request.Cookies["RecordID"].Value;

        //if (Request.QueryString["bid"] != null)
        //    e.InputParameters["balans_id"] = int.Parse(Request.QueryString["bid"]);
    }

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

    protected void SqlDataSourceDictBuildings_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("SqlDataSourceDictBuildings_Selecting=" + AddressStreetID);
		e.Command.Parameters["@street_id"].Value = AddressStreetID;
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

    void CopyCard(int copyId, int reportId)
	{
        var connection = Utils.ConnectToDatabase();
        var transaction = connection.BeginTransaction();

        SqlParameter outputParam = new SqlParameter("new_arenda_id", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        using (SqlCommand cmd = new SqlCommand("dbo.fnArendaClone", connection, transaction))
        {
            var user = Membership.GetUser();
            var username = (user == null ? String.Empty : (String)user.UserName);

            cmd.Parameters.Add(new SqlParameter("arenda_id", copyId));
            cmd.Parameters.Add(new SqlParameter("report_id", reportId));
            cmd.Parameters.Add(new SqlParameter("modified_by", username));
            cmd.Parameters.Add(new SqlParameter("modify_date", DateTime.Now));
            cmd.Parameters.Add(outputParam);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
        }

        var new_arenda_id = (int)outputParam.Value;

        //transaction.Rollback();
        transaction.Commit();

        var url = "~/Reports1NF/OrgRentAgreement.aspx?rid=" + reportId + "&aid=" + new_arenda_id;
        Response.Redirect(Page.ResolveClientUrl(url));
    }

}




