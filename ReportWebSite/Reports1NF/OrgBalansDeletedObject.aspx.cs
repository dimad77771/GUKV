using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using GUKV;

public partial class Reports1NF_OrgBalansDeletedObject : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");
    //private bool FormIsValid = true;
    //private static IList<string> ErrorMessages = new List<string>();
    //private static List<string> reqEditors = new List<string> { "ComboVidchType", "EditVidchSquare", "EditObjBtiReestrNoDeleted", "ComboVidchDocKind", "EditVidchDocNum", "EditVidchDocDate" };
    //private string mode = ""; // save or send? 

    private DeletedObjectValidator validator;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];
        string balansIdStr = Request.QueryString["bid"];

        if (reportIdStr != null && reportIdStr.Length > 0 && balansIdStr != null && balansIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);
            BalansObjectID = int.Parse(balansIdStr);

            // Set the parameters for SQL queries
            SqlDataSourceBuilding.SelectParameters["bal_id"].DefaultValue = BalansObjectID.ToString();
            SqlDataSourceBuilding.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();

            SqlDataSourceBalansObject.SelectParameters["bal_id"].DefaultValue = BalansObjectID.ToString();
            SqlDataSourceBalansObject.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();

            SqlDataSourceBalansObjectStatus.SelectParameters["bal_id"].DefaultValue = BalansObjectID.ToString();
            SqlDataSourceBalansObjectStatus.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
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
        if (!DeletedObjectExistsInReport)
        {
            Response.Redirect(Page.ResolveClientUrl("~/Account/RestrictedObject.aspx"));
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
        ReportCommentViewer1.BalansDeletedId = BalansObjectID;
        ReportCommentViewer1.AddNumberOfCommentsToButton(ButtonComments);

        // Enable / disable all controls depending on the report owner
        EnableControlsBasingOnUserRole();

        validator = new DeletedObjectValidator(this, "MainGroup");
    }

    protected void EnableControlsBasingOnUserRole()
    {
        bool userIsReportSubmitter = Roles.IsUserInRole(Utils.Report1NFSubmitterRole);
        bool reportBelongsToUser = ReportBelongsToThisUser.HasValue ? ReportBelongsToThisUser.Value : false;

        string strBid = Request.QueryString["bid"];
        int bId = int.Parse(strBid);

        Dictionary<string, string> markedControls = Reports1NFUtils.GetMarkedControlIDs(ReportID, null, bId, null, null, null);

        Reports1NFUtils.EnableDevExpressEditors(AddressForm, reportBelongsToUser && userIsReportSubmitter, markedControls);
        Reports1NFUtils.EnableDevExpressEditors(DelObjectForm, reportBelongsToUser && userIsReportSubmitter, markedControls);

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

    protected int BalansObjectID
    {
        get
        {
            object balansId = ViewState["BALANS_OBJECT_ID"];

            if (balansId is int)
            {
                return (int)balansId;
            }

            return 0;
        }

        set
        {
            ViewState["BALANS_OBJECT_ID"] = value;
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

    protected bool DeletedObjectExistsInReport
    {
        get
        {
            bool exists = false;
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 id FROM reports1nf_balans_deleted WHERE id = @bid AND report_id = @rep_id", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("bid", BalansObjectID));
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

    protected void SaveChanges(SqlConnection connection)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        Dictionary<string, Control> controls = new Dictionary<string, Control>();

        Reports1NFUtils.GetAllControls(AddressForm, controls);
        Reports1NFUtils.GetAllControls(DelObjectForm, controls);

        // Get the balans object properties
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string fieldList = "";

        AddQueryParameter(ref fieldList, "vidch_type_id", "vtid", Reports1NFUtils.GetDropDownValue(controls, "ComboVidchType"), parameters);
        AddQueryParameter(ref fieldList, "floors", "flrs", Reports1NFUtils.GetEditText(controls, "EditVidchFloor"), parameters);
        AddQueryParameter(ref fieldList, "sqr_total", "sqrtot", Reports1NFUtils.GetEditNumeric(controls, "EditVidchSquare"), parameters);
        AddQueryParameter(ref fieldList, "vidch_cost", "vct", Reports1NFUtils.GetEditNumeric(controls, "EditVidchCost"), parameters);
        AddQueryParameter(ref fieldList, "purpose_str", "purpstr", Reports1NFUtils.GetEditText(controls, "EditVidchPurpose"), parameters);
        AddQueryParameter(ref fieldList, "reestr_no", "reesno", Reports1NFUtils.GetEditText(controls, "EditObjBtiReestrNoDeleted"), parameters);

        AddQueryParameter(ref fieldList, "vidch_org_id", "vorg", Reports1NFUtils.GetDropDownValue(controls, "ComboVidchOrg"), parameters);
        AddQueryParameter(ref fieldList, "vidch_doc_type", "vdt", Reports1NFUtils.GetDropDownValue(controls, "ComboVidchDocKind"), parameters);
        AddQueryParameter(ref fieldList, "vidch_doc_num", "vdn", Reports1NFUtils.GetEditText(controls, "EditVidchDocNum"), parameters);
        AddQueryParameter(ref fieldList, "vidch_doc_date", "vdd", Reports1NFUtils.GetDateValue(controls, "EditVidchDocDate"), parameters);

        // System parameters
        AddQueryParameter(ref fieldList, "modify_date", "mdt", DateTime.Now, parameters);
        AddQueryParameter(ref fieldList, "modified_by", "mby", username.Left(64), parameters);

        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_balans_deleted SET " + fieldList + ", is_valid = @isValid, validation_errors = @errMsgs WHERE report_id = @rid AND id = @bid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.Parameters.Add(new SqlParameter("bid", BalansObjectID));
            cmd.Parameters.Add(new SqlParameter("isValid", validator.IsValid));
            cmd.Parameters.Add(new SqlParameter("errMsgs", string.Join("<br/>", validator.ValidationErrorMessages)));

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            cmd.ExecuteNonQuery();
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

            SqlConnection connectionSql = Utils.ConnectToDatabase();

            if (connectionSql != null)
            {
                // Save the form before sending it to DKV
                SaveChanges(connectionSql);

                validator.ValidateDB(connectionSql, "reports1nf_balans_deleted", string.Format("report_id = {0} and id = {1}", ReportID, BalansObjectID), true);
                if (validator.IsValid)
                {
                    Reports1NFUtils.SendBalansDeletedObject(connectionSql, /*connection1NF,*/ ReportID, BalansObjectID);

                    connectionSql.Close();
                }
                else
                {
                    StatusForm.DataBind();
                    connectionSql.Close();
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
        DelObjectForm.DataBind();
        FormViewState.DataBind();

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

    #region Organization picker

    private string OrgZkpoPattern
    {
        get
        {
            object pattern = Session["OrgBalansDeletedObject_OrgZkpoPattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["OrgBalansDeletedObject_OrgZkpoPattern"] = value;
        }
    }

    private string OrgNamePattern
    {
        get
        {
            object pattern = Session["OrgBalansDeletedObject_OrgNamePattern"];
            return (pattern is string) ? (string)pattern : "";
        }

        set
        {
            Session["OrgBalansDeletedObject_OrgNamePattern"] = value;
        }
    }

    protected void SqlDataSourceOrgSearch_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        //Dictionary<string, Control> controls = new Dictionary<string, Control>();

        //Reports1NFUtils.GetAllControls(AddressForm, controls);
        //Reports1NFUtils.GetAllControls(DelObjectForm, controls);
        //int orgId = Reports1NFUtils.GetDropDownValue(controls, "ComboVidchOrg");
        //e.Command.Parameters["@org_id"].Value = orgId;

        e.Command.Parameters["@bal_id"].Value = BalansObjectID;
        e.Command.Parameters["@rep_id"].Value = ReportID;

        string orgName = OrgNamePattern;
        string zkpo = OrgZkpoPattern;

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

    protected void ComboVidchOrg_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] parts = e.Parameter.Split(new char[] { '|' });

        if (parts.Length == 2)
        {
            OrgZkpoPattern = parts[0].Trim();
            OrgNamePattern = parts[1].Trim().ToUpper();

            (sender as ASPxComboBox).DataBind();
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

    #endregion (Organization picker)
}