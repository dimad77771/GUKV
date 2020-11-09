using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using DevExpress.Web;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using GUKV;


public partial class Reports1NF_OrgRentedObject : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");
    //private bool FormIsValid = true;
    //private static IList<string> ErrorMessages = new List<string>();
    //private static List<string> reqEditors = new List<string> { "EditAgrNum", "EditAgrDate", "EditAgrStartDate" };
    //private string mode = ""; // save or send? 

    private RentedObjectValidator validator;

    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];
        string objectIdStr = Request.QueryString["aid"];

        if (reportIdStr != null && reportIdStr.Length > 0 && objectIdStr != null && objectIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);
            RentedObjectID = int.Parse(objectIdStr);

            // Set the parameters for SQL queries
            SqlDataSourceBuilding.SelectParameters["aid"].DefaultValue = RentedObjectID.ToString();
            SqlDataSourceBuilding.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();

            SqlDataSourceRentedObj.SelectParameters["aid"].DefaultValue = RentedObjectID.ToString();
            SqlDataSourceRentedObj.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();

            SqlDataSourceAgreementStatus.SelectParameters["aid"].DefaultValue = RentedObjectID.ToString();
            SqlDataSourceAgreementStatus.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
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
        if (!RentedObjectExistsInReport)
        {
            Response.Redirect(Page.ResolveClientUrl("~/Account/RestrictedRentAgreement.aspx"));
        }

        if (ReportID > 0)
        {
            for (int i = 0; i < SectionMenu.Items.Count; i++)
            {
                if (SectionMenu.Items[i].NavigateUrl.IndexOf('?') < 0)
                    SectionMenu.Items[i].NavigateUrl += "?rid=" + ReportID.ToString();
            }
        }

        ReportCommentViewer1.ReportId = ReportID;
        ReportCommentViewer1.RentedObjectId = RentedObjectID;
        ReportCommentViewer1.AddNumberOfCommentsToButton(ButtonComments);

        EnableCmkControls();

        // Enable / disable all controls depending on the report owner
        EnableControlsBasingOnUserRole();

        validator = new RentedObjectValidator(this, "MainGroup");
    }

    protected void EnableControlsBasingOnUserRole()
    {
        bool userIsReportSubmitter = Roles.IsUserInRole(Utils.Report1NFSubmitterRole);
        bool reportBelongsToUser = ReportBelongsToThisUser.HasValue ? ReportBelongsToThisUser.Value : false;

        string strAid = Request.QueryString["aid"];
        int aId = int.Parse(strAid);

        Dictionary<string, string> markedControls = Reports1NFUtils.GetMarkedControlIDs(ReportID, null, null, null, aId, null);

        Reports1NFUtils.EnableDevExpressEditors(AddressForm, reportBelongsToUser && userIsReportSubmitter, markedControls);
        Reports1NFUtils.EnableDevExpressEditors(RentedObjectForm, reportBelongsToUser && userIsReportSubmitter, markedControls);

        ButtonSave.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
        ButtonSend.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
        ButtonClear.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
        StatusForm.Visible = reportBelongsToUser && userIsReportSubmitter;
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

    protected int RentedObjectID
    {
        get
        {
            object rentedObjId = ViewState["RENTED_OBJECT_ID"];

            if (rentedObjId is int)
            {
                return (int)rentedObjId;
            }

            return 0;
        }

        set
        {
            ViewState["RENTED_OBJECT_ID"] = value;
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

    protected bool RentedObjectExistsInReport
    {
        get
        {
            bool exists = false;
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 id FROM reports1nf_arenda_rented WHERE id = @aid AND report_id = @rep_id", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("aid", RentedObjectID));
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
        e.Command.Parameters["@aid"].Value = RentedObjectID;
        e.Command.Parameters["@rep_id"].Value = ReportID;
    }

    protected void EnableCmkControls()
    {
        Control panelCmk = RentedObjectForm.FindControl("PanelCmk");

        if (panelCmk is ASPxRoundPanel)
        {
            Control checkBox = panelCmk.FindControl("CheckIsCmk");

            if (checkBox is ASPxCheckBox)
            {
                bool enable = (checkBox as ASPxCheckBox).Checked;

                EnableCmkControl(panelCmk, enable, "EditCmkSqrRented");
                EnableCmkControl(panelCmk, enable, "EditCmkPaymentNarah");
                EnableCmkControl(panelCmk, enable, "EditCmkPaymentToBudget");
                EnableCmkControl(panelCmk, enable, "EditCmkRentDebt");
            }
        }
    }

    protected void EnableCmkControl(Control panelCollection, bool enable, string controlID)
    {
        Control ctl = panelCollection.FindControl(controlID);

        if (ctl is ASPxSpinEdit)
        {
            (ctl as ASPxSpinEdit).ClientEnabled = enable;
        }
    }

    public static void MarkControlAsValid(Dictionary<string, Control> controls, string controlID)
    {
        Control ctl = null;

        if (controls.TryGetValue(controlID.ToLower(), out ctl))
        {
            if (ctl is ASPxEdit)
            {
                (ctl as ASPxEdit).IsValid = true;
            }
        }
    }

    protected void SaveChanges(SqlConnection connection)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        Dictionary<string, Control> controls = new Dictionary<string, Control>();

        Reports1NFUtils.GetAllControls(AddressForm, controls);
        Reports1NFUtils.GetAllControls(RentedObjectForm, controls);

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string fieldList = "";

        // Get the rent agreement properties
        AddQueryParameter(ref fieldList, "agreement_num", "anum", Reports1NFUtils.GetEditText(controls, "EditAgrNum"), parameters);
        AddQueryParameter(ref fieldList, "agreement_date", "adt", Reports1NFUtils.GetDateValue(controls, "EditAgrDate"), parameters);
        AddQueryParameter(ref fieldList, "rent_start_date", "dtstart", Reports1NFUtils.GetDateValue(controls, "EditAgrStartDate"), parameters);
        AddQueryParameter(ref fieldList, "rent_finish_date", "dtfin", Reports1NFUtils.GetDateValue(controls, "EditAgrFinishDate"), parameters);
        AddQueryParameter(ref fieldList, "payment_type_id", "payid", Reports1NFUtils.GetDropDownValue(controls, "ComboPaymentTypeRented"), parameters);
        AddQueryParameter(ref fieldList, "rent_square", "rsqr", Reports1NFUtils.GetEditNumeric(controls, "EditRentedSquare"), parameters);
        AddQueryParameter(ref fieldList, "is_subarenda", "issub", Reports1NFUtils.GetCheckBoxValue(controls, "CheckRentedSubarenda") ? 1 : 0, parameters);

        AddQueryParameter(ref fieldList, "is_cmk", "iscmk", Reports1NFUtils.GetCheckBoxValue(controls, "CheckIsCmk") ? 1 : 0, parameters);
        AddQueryParameter(ref fieldList, "cmk_sqr_rented", "cmksqr", Reports1NFUtils.GetEditNumeric(controls, "EditCmkSqrRented"), parameters);
        AddQueryParameter(ref fieldList, "cmk_payment_narah", "cmkpayn", Reports1NFUtils.GetEditNumeric(controls, "EditCmkPaymentNarah"), parameters);
        AddQueryParameter(ref fieldList, "cmk_payment_to_budget", "cmkpayb", Reports1NFUtils.GetEditNumeric(controls, "EditCmkPaymentToBudget"), parameters);
        AddQueryParameter(ref fieldList, "cmk_rent_debt", "cmkdebt", Reports1NFUtils.GetEditNumeric(controls, "EditCmkRentDebt"), parameters);

        // System parameters
        AddQueryParameter(ref fieldList, "modify_date", "mdt", DateTime.Now, parameters);
        AddQueryParameter(ref fieldList, "modified_by", "mby", username.Left(64), parameters);

        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_rented SET " + fieldList + ", is_valid = @isValid, validation_errors = @errMsgs WHERE report_id = @rid AND id = @aid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.Parameters.Add(new SqlParameter("aid", RentedObjectID));
            cmd.Parameters.Add(new SqlParameter("isValid", validator.IsValid));
            cmd.Parameters.Add(new SqlParameter("errMsgs", string.Join("<br/>", validator.ValidationErrorMessages)));

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            cmd.ExecuteNonQuery();
        }

        MarkControlAsValid(controls, "EditCmkSqrRented");
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

    protected void CPMainPanel_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.StartsWith("save:"))
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                SaveChanges(connection);

                connection.Close();
            }
        }
        else if (e.Parameter.StartsWith("send:"))
        {
            validator.ValidateUI();
            this.errorForm.DataSource = validator.FormatErrorDataSource();
            this.errorForm.DataBind();

            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // Save the form before sending it to DKV
                SaveChanges(connection);

                if (validator.ValidateDB(connection, "reports1nf_arenda_rented", string.Format("report_id = {0} and id = {1}", ReportID, RentedObjectID), true))
                {
                    Reports1NFUtils.SendRentedObject(connection, /*connection1NF,*/ ReportID, RentedObjectID);

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
        RentedObjectForm.DataBind();
        FormViewState.DataBind();

        EnableCmkControls();
        EnableControlsBasingOnUserRole();
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

    protected void errorForm_DataBound(object sender, EventArgs e)
    {
        Repeater r = (Repeater)Utils.FindControlRecursive(errorForm, "errorList");
        if (r != null)
        {
            r.DataSource = errorForm.DataSource;
            r.DataBind();
        }
    }
}