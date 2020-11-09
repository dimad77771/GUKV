using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using log4net;
using System.Xml;
using System.Xml.XPath;
using DevExpress.Web.Data;
using ExtDataEntry.Models;
using GUKV;
using System.Web.Configuration;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;


public partial class Reports1NF_OrgBalansObject : PhotoPage
{
    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");
    //private bool FormIsValid = true;
    //private static IList<string> ErrorMessages = new List<string>();
    //private static List<string> reqEditors = new List<string> { "EditBuildingSqrTotal", "ComboObjStatus", "EditObjBtiReestrNo", "EditObjSqrTotal", "ComboOwnershipDocKind", "EditOwnershipDocNum", "EditOwnershipDocDate", "ComboBalansDocKind", "EditBalansDocNum", "EditBalansDocDate" };
    //private string mode = ""; // save or send? 

    private BalansObjectValidator validator;

    public Control FindControlRecursive(Control control, string id)
    {
        //GridViewDataTextColumn aa;
        //aa.EditFormSettings

        //ASPxGridView aa;
        //aa.ClientSideEvents.RowExpanding

        //GridViewDataTextColumn a;
        //a.EditFormSettings

        //GridViewDataTextColumn a;
        //a.PropertiesEdit.ClientInstanceName

        if (control == null) return null;
        //try to find the control at the current level
        Control ctrl = control.FindControl(id);

        if (ctrl == null)
        {
            //search the children
            foreach (Control child in control.Controls)
            {
                ctrl = FindControlRecursive(child, id);

                if (ctrl != null) break;
            }
        }
        return ctrl;
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {

    }

	protected string ParamRid
	{
		get
		{
			return Request.QueryString["rid"];
		}
	}
	protected string ParamBid
	{
		get
		{
			return Request.QueryString["bid"];
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
    protected bool EditFreeSquareMode
    {
        get
        {
            return (ParamEditFreeSquareId != null);
        }
    }

    protected void ASPxButton_ArendaObjects_ExportPDF_Click(object sender, EventArgs e)
	{
		//this.ExportGridToPDF(ASPxGridViewExporterArendaObjects, PrimaryGridView, LabelReportTitle1.Text, ViewState["PrimaryGridView.DataSourceID"] as string);
		var b = 100;
	}


	protected void Page_Load(object sender, EventArgs e)
    {
//////
        try
		{
        GetPageUniqueKey();
        //Page.ClientScript.RegisterClientScriptInclude("jQuery-1.4.1", Page.ResolveClientUrl("~/Scripts/jquery-1.4.1.js"));

        string reportIdStr = Request.QueryString["rid"];
        string balansIdStr = Request.QueryString["bid"];

        //string tab = Request.QueryString["tab"];
        //if (!string.IsNullOrEmpty(tab))
            //CardPageControl.ActiveTabIndex = int.Parse(tab);


        if ((!IsCallback) && (reportIdStr != null && reportIdStr.Length > 0 && balansIdStr != null && balansIdStr.Length > 0))
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

            SqlDataSourcePhoto.SelectParameters["bal_id"].DefaultValue = BalansObjectID.ToString();
            SqlDataSourcePhoto.SelectParameters["folder_prefix"].DefaultValue = "~/ImgContent/1NF/";

            SqlDataSourceFreeSquare.SelectParameters["balans_id"].DefaultValue = BalansObjectID.ToString();
            SqlDataSourceFreeSquare.SelectParameters["report_id"].DefaultValue = ReportID.ToString();
            SqlDataSourceFreeSquare.SelectParameters["free_square_id"].DefaultValue = (EditFreeSquareMode ? ParamEditFreeSquareId : - 1).ToString();

            SqlDataSourceBalansArchive.SelectParameters["balid"].DefaultValue = balansIdStr.Trim();
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
        if (!BalansObjectExistsInReport)
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
        ReportCommentViewer1.BalansId = BalansObjectID;
        ReportCommentViewer1.AddNumberOfCommentsToButton(ButtonComments);

        // Enable / disable all controls depending on the report owner
        EnableControlsBasingOnUserRole();

        validator = new BalansObjectValidator(this, "MainGroup");

        PrepareTempPhotoFolder();


        if (!IsPostBack)
		{
            if (!EditFreeSquareMode)
            {
                ASPxGridViewFreeSquare.FilterExpression = "[is_included] = True";
            }
            foreach (var col in ASPxGridViewFreeSquare.Columns)
            {
                var vcol = col as GridViewEditDataColumn;
                if (vcol != null)
                {
                    vcol.PropertiesEdit.ClientInstanceName = "felm__" + (!string.IsNullOrEmpty(vcol.FieldName) ? vcol.FieldName : vcol.Name);
                }
            }

            if (EditFreeSquareMode)
			{
                foreach(TabPage tabpage in CardPageControl.TabPages)
				{
                    if (tabpage.Text != "Вільні приміщення")
					{
                        tabpage.Visible = false;
                    }
                }

                ASPxGridViewFreeSquare.StartEdit(0);
            }
        }
            //////
        }
        catch (Exception ex)
        {
            var lognet = log4net.LogManager.GetLogger("ReportWebSite");
            lognet.Debug("--------------- Orgbalansobject page load ----------------", ex);
            throw ex;
        }
    }

    protected void EnableControlsBasingOnUserRole()
    {
        bool userIsReportSubmitter = Roles.IsUserInRole(Utils.Report1NFSubmitterRole);
		bool userIsReportSubmitter0 = userIsReportSubmitter;
		bool reportBelongsToUser = ReportBelongsToThisUser.HasValue ? ReportBelongsToThisUser.Value : false;

		bool userIsReportReportCenterSubmit = Roles.IsUserInRole(Utils.Report1NFReportCenterSubmitRole);
		if (userIsReportReportCenterSubmit)
		{
			userIsReportSubmitter = true;
			reportBelongsToUser = true;
		}

		string strBid = Request.QueryString["bid"];
        int bId = int.Parse(strBid);

        Dictionary<string, string> markedControls = Reports1NFUtils.GetMarkedControlIDs(ReportID, bId, null, null, null, null);

        Reports1NFUtils.EnableDevExpressEditors(AddressForm, reportBelongsToUser && userIsReportSubmitter, markedControls);
        Reports1NFUtils.EnableDevExpressEditors(BalansObjForm, reportBelongsToUser && userIsReportSubmitter, markedControls);
        Reports1NFUtils.EnableDevExpressEditors(BalansDocsForm, reportBelongsToUser && userIsReportSubmitter, markedControls);
        Reports1NFUtils.EnableDevExpressEditors(BalansCostForm, reportBelongsToUser && userIsReportSubmitter, markedControls);

        ButtonSave.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
        ButtonSend.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
        ButtonClear.ClientVisible = reportBelongsToUser && userIsReportSubmitter;
        StatusForm.Visible = reportBelongsToUser && userIsReportSubmitter;
		if (!userIsReportSubmitter0)
		{
			ButtonSave.ClientVisible = false;
		}

		//pgv
		var enabledFreeSquare = reportBelongsToUser && userIsReportSubmitter;
        if (EditFreeSquareMode)
        {
            enabledFreeSquare = true;
            var col0 = ASPxGridViewFreeSquare.AllColumns[0] as GridViewCommandColumn;
            col0.ShowNewButton = false;
            col0.ShowDeleteButton = false;
            (col0.CustomButtons["btnPhoto"] as GridViewCommandColumnCustomButton).Visibility = GridViewCustomButtonVisibility.Invisible;
            (col0.CustomButtons["btnPdfBuild"] as GridViewCommandColumnCustomButton).Visibility = GridViewCustomButtonVisibility.Invisible;
            (col0.CustomButtons["btnFreeCycle"] as GridViewCommandColumnCustomButton).Visibility = GridViewCustomButtonVisibility.Invisible;
        }
        //ASPxGridViewFreeSquare.Enabled = enabledFreeSquare;
        if (!enabledFreeSquare)
		{
			var col0 = ASPxGridViewFreeSquare.AllColumns[0] as GridViewCommandColumn;
			col0.ShowEditButton = false;
			col0.ShowNewButton = false;
			col0.ShowDeleteButton = false;
			col0.ShowCancelButton = false;
			(col0.CustomButtons["btnPhoto"] as GridViewCommandColumnCustomButton).Visibility = GridViewCustomButtonVisibility.Invisible;
		}


		// Enable or disable BTI documentation controls
		Control panelBti = BalansDocsForm.FindControl("PanelObjBTI");

        if (panelBti is ASPxRoundPanel)
        {
            Control editBtiCondition = panelBti.FindControl("EditObjBTICondition");
            Control EditCaseNumber = panelBti.FindControl("EditCaseNumber");
            Control editObjBtiDate = panelBti.FindControl("EditObjBtiDate");

            if (editBtiCondition is ASPxTextEdit && EditCaseNumber is ASPxTextEdit && editObjBtiDate is ASPxTextEdit)
            {
                object value = (editBtiCondition as ASPxTextEdit).Value;

                (EditCaseNumber as ASPxTextEdit).ClientEnabled = value is int && (int)value == 1;
                (editObjBtiDate as ASPxTextEdit).ClientEnabled = value is int && (int)value == 1;
            }
        }

        // Enable or disable Basement and Loft controls
        Control panelBuildingSquare = AddressForm.FindControl("PanelBuildingSquare");

        if (panelBuildingSquare is ASPxRoundPanel)
        {
            Control checkBasementExists = panelBuildingSquare.FindControl("CheckBasementExists");
            Control checkLoftExists = panelBuildingSquare.FindControl("CheckLoftExists");
            Control editBasementSqr = panelBuildingSquare.FindControl("EditBuildingSqrBasement");
            Control editLoftSqr = panelBuildingSquare.FindControl("EditBuildingSqrLoft");

            if (checkBasementExists is ASPxCheckBox && editBasementSqr is ASPxSpinEdit)
            {
                (editBasementSqr as ASPxSpinEdit).ClientEnabled = (checkBasementExists as ASPxCheckBox).Checked;
            }

            if (checkLoftExists is ASPxCheckBox && editLoftSqr is ASPxSpinEdit)
            {
                (editLoftSqr as ASPxSpinEdit).ClientEnabled = (checkLoftExists as ASPxCheckBox).Checked;
            }
        }

        Control panelFreeSquare = BalansObjForm.FindControl("PanelFreeSquare");

        if (panelFreeSquare is ASPxRoundPanel)
        {
            Control checkFreeSquareExists = panelFreeSquare.FindControl("CheckFreeSquareExists");
            Control editObjFreeSquare = panelFreeSquare.FindControl("EditObjFreeSquare");
            Control comboFreeSqrTechState = panelFreeSquare.FindControl("ComboFreeSqrTechState");
            Control editObjFreeSqrLocation = panelFreeSquare.FindControl("EditObjFreeSqrLocation");

            if (checkFreeSquareExists is ASPxCheckBox && editObjFreeSquare is ASPxSpinEdit)
            {
                (editObjFreeSquare as ASPxSpinEdit).ClientEnabled = (checkFreeSquareExists as ASPxCheckBox).Checked;
            }

            if (checkFreeSquareExists is ASPxCheckBox && comboFreeSqrTechState is ASPxComboBox)
            {
                (comboFreeSqrTechState as ASPxComboBox).ClientEnabled = (checkFreeSquareExists as ASPxCheckBox).Checked;
            }

            if (checkFreeSquareExists is ASPxCheckBox && editObjFreeSqrLocation is ASPxTextBox)
            {
                (editObjFreeSqrLocation as ASPxTextBox).ClientEnabled = (checkFreeSquareExists as ASPxCheckBox).Checked;
            }
        }
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

    public bool IsHistoryButtonVisible()
    {
        return Request.QueryString["bid"] == null ? false : true;
    }


    protected bool BalansObjectExistsInReport
    {
        get
        {
            bool exists = false;
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 id FROM reports1nf_balans WHERE id = @bid AND report_id = @rep_id", connection))
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
		//!!!! -> нужно выставить в connection.string сменить 
		//		"MultipleActiveResultSets=True" 
		//			на
		//		"MultipleActiveResultSets=False" 
		//!!SqlCommand cmd22 = new SqlCommand("begin tran", connection);
		//!!cmd22.ExecuteNonQuery();

		System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        Dictionary<string, Control> controls = new Dictionary<string, Control>();

        Reports1NFUtils.GetAllControls(AddressForm, controls);
        Reports1NFUtils.GetAllControls(BalansObjForm, controls);
        Reports1NFUtils.GetAllControls(BalansDocsForm, controls);
        Reports1NFUtils.GetAllControls(BalansCostForm, controls);

        // Get the building ID in our temporary table
        int buildingUniqueId = Reports1NFUtils.GetBalansBuildingUniqueId(connection, ReportID, BalansObjectID, null);

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string fieldList = "";

        if (buildingUniqueId > 0)
        {
            // Update the building properties
            AddQueryParameter(ref fieldList, "addr_nomer1", "an1", Reports1NFUtils.GetEditText(controls, "EditBuildingNum1"), parameters);
            AddQueryParameter(ref fieldList, "addr_nomer2", "an2", Reports1NFUtils.GetEditText(controls, "EditBuildingNum2"), parameters);
            AddQueryParameter(ref fieldList, "addr_nomer3", "an3", Reports1NFUtils.GetEditText(controls, "EditBuildingNum3"), parameters);
            AddQueryParameter(ref fieldList, "addr_misc", "amsc", Reports1NFUtils.GetEditText(controls, "EditMiscAddr"), parameters);
            AddQueryParameter(ref fieldList, "addr_zip_code", "azip", Reports1NFUtils.GetEditText(controls, "EditZipCode"), parameters);

            AddQueryParameter(ref fieldList, "construct_year", "cy", Reports1NFUtils.GetEditNumeric(controls, "EditBuildYear"), parameters);
            AddQueryParameter(ref fieldList, "expl_enter_year", "eey", Reports1NFUtils.GetEditNumeric(controls, "EditExplYear"), parameters);
            AddQueryParameter(ref fieldList, "is_basement_exists", "ibe", Reports1NFUtils.GetCheckBoxValue(controls, "CheckBasementExists") ? 1 : 0, parameters);
            AddQueryParameter(ref fieldList, "is_loft_exists", "ile", Reports1NFUtils.GetCheckBoxValue(controls, "CheckLoftExists") ? 1 : 0, parameters);

            AddQueryParameter(ref fieldList, "sqr_total", "sqrtot", Reports1NFUtils.GetEditNumeric(controls, "EditBuildingSqrTotal"), parameters);
            AddQueryParameter(ref fieldList, "sqr_non_habit", "sqrnh", Reports1NFUtils.GetEditNumeric(controls, "EditBuildingSqrNonHabit"), parameters);
            AddQueryParameter(ref fieldList, "sqr_habit", "sqrh", Reports1NFUtils.GetEditNumeric(controls, "EditBuildingSqrHabit"), parameters);
            AddQueryParameter(ref fieldList, "sqr_pidval", "sqrpdv", Reports1NFUtils.GetEditNumeric(controls, "EditBuildingSqrBasement"), parameters);
            AddQueryParameter(ref fieldList, "sqr_loft", "sqrloft", Reports1NFUtils.GetEditNumeric(controls, "EditBuildingSqrLoft"), parameters);

            AddQueryParameter(ref fieldList, "facade_id", "fid", Reports1NFUtils.GetDropDownValue(controls, "ComboBuildingFacade"), parameters);
            AddQueryParameter(ref fieldList, "num_floors", "nfl", Reports1NFUtils.GetEditText(controls, "EditBuildingNumFloors"), parameters);
            AddQueryParameter(ref fieldList, "object_kind_id", "knd", Reports1NFUtils.GetDropDownValue(controls, "ComboBuildingKind"), parameters);
            AddQueryParameter(ref fieldList, "object_type_id", "type", Reports1NFUtils.GetDropDownValue(controls, "ComboBuildingType"), parameters);
            AddQueryParameter(ref fieldList, "tech_condition_id", "tech", Reports1NFUtils.GetDropDownValue(controls, "ComboBuildingTechState"), parameters);
            AddQueryParameter(ref fieldList, "history_id", "hist", Reports1NFUtils.GetDropDownValue(controls, "ComboBuildingHistory"), parameters);

            AddQueryParameter(ref fieldList, "modify_date", "mdt", DateTime.Now, parameters);
            AddQueryParameter(ref fieldList, "modified_by", "mby", username.Left(64), parameters);
            AddQueryParameter(ref fieldList, "addr_distr_new_id", "adnid", Reports1NFUtils.GetDropDownValue(controls, "ComboAddrDistrict"), parameters);
            AddQueryParameter(ref fieldList, "addr_street_id", "adsid", Reports1NFUtils.GetDropDownValue(controls, "ComboAddrStreet"), parameters);
            AddQueryParameter(ref fieldList, "addr_street_name", "adsnm", Reports1NFUtils.GetDropDownText(controls, "ComboAddrStreet"), parameters);
            AddQueryParameter(ref fieldList, "street_full_name", "strfm", Reports1NFUtils.GetDropDownText(controls, "ComboAddrStreet"), parameters);
            AddQueryParameter(ref fieldList, "addr_korpus", "adkorp", Reports1NFUtils.GetDropDownText(controls, "ComboAddrStreet")+" "+Reports1NFUtils.GetEditText(controls, "EditBuildingNum1")+Reports1NFUtils.GetEditText(controls, "EditBuildingNum2")+Reports1NFUtils.GetEditText(controls, "EditBuildingNum3"), parameters);

            using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_buildings SET " + fieldList + " WHERE unique_id = @uid", connection))
            {
                cmd.Parameters.Add(new SqlParameter("uid", buildingUniqueId));

                foreach (KeyValuePair<string, object> param in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                }

                cmd.ExecuteNonQuery();
            }
        }

        // Get the balans object properties
        parameters.Clear();
        fieldList = "";

        AddQueryParameter(ref fieldList, "object_kind_id", "knd", Reports1NFUtils.GetDropDownValue(controls, "ComboObjKind"), parameters);
        AddQueryParameter(ref fieldList, "object_type_id", "typ", Reports1NFUtils.GetDropDownValue(controls, "ComboObjType"), parameters);
        AddQueryParameter(ref fieldList, "tech_condition_id", "tech", Reports1NFUtils.GetDropDownValue(controls, "ComboObjTechState"), parameters);
        AddQueryParameter(ref fieldList, "obj_status_id", "ostat", Reports1NFUtils.GetDropDownValue(controls, "ComboObjStatus"), parameters);
        AddQueryParameter(ref fieldList, "floors", "flrs", Reports1NFUtils.GetEditText(controls, "EditObjFloors"), parameters);

        AddQueryParameter(ref fieldList, "sqr_total", "sqrtot", Reports1NFUtils.GetEditNumeric(controls, "EditObjSqrTotal"), parameters);
        AddQueryParameter(ref fieldList, "sqr_kor", "sqrkor", Reports1NFUtils.GetEditNumeric(controls, "EditObjSqrKor"), parameters);
        AddQueryParameter(ref fieldList, "sqr_vlas_potreb", "sqrvl", Reports1NFUtils.GetEditNumeric(controls, "EditObjSqrVlasn"), parameters);
        AddQueryParameter(ref fieldList, "sqr_engineering", "sqreng", Reports1NFUtils.GetEditNumeric(controls, "EditObjSqrEng"), parameters);
        AddQueryParameter(ref fieldList, "num_rent_agr", "numr", Reports1NFUtils.GetEditNumeric(controls, "EditObjNumRentAgreements"), parameters);
        AddQueryParameter(ref fieldList, "sqr_in_rent", "sqrrnt", Reports1NFUtils.GetEditNumeric(controls, "EditObjSqrInRent"), parameters);
        AddQueryParameter(ref fieldList, "sqr_pidval", "sqrpdv", Reports1NFUtils.GetEditNumeric(controls, "EditBuildingSqrBasement"), parameters);

        AddQueryParameter(ref fieldList, "is_free_sqr", "isfr", Reports1NFUtils.GetCheckBoxValue(controls, "CheckFreeSquareExists") ? 1 : 0, parameters);
        AddQueryParameter(ref fieldList, "free_sqr_useful", "sqrfr", Reports1NFUtils.GetEditNumeric(controls, "EditObjFreeSquare") ==  null ? 0 : Reports1NFUtils.GetEditNumeric(controls, "EditObjFreeSquare"), parameters);
        AddQueryParameter(ref fieldList, "free_sqr_condition_id", "frtech", Reports1NFUtils.GetEditText(controls, "EditFreeSqrCondition"), parameters);
        AddQueryParameter(ref fieldList, "free_sqr_location", "frloc", Reports1NFUtils.GetEditText(controls, "EditObjFreeSqrLocation"), parameters);

        AddQueryParameter(ref fieldList, "form_ownership_id", "ownr", Reports1NFUtils.GetDropDownValue(controls, "ComboObjOwnership"), parameters);
        AddQueryParameter(ref fieldList, "ownership_type_id", "right", Reports1NFUtils.GetDropDownValue(controls, "ComboObjRight"), parameters);

        AddQueryParameter(ref fieldList, "ownership_doc_type", "odtyp", Reports1NFUtils.GetDropDownValue(controls, "ComboOwnershipDocKind"), parameters);
        AddQueryParameter(ref fieldList, "ownership_doc_num", "odnum", Reports1NFUtils.GetEditText(controls, "EditOwnershipDocNum"), parameters);
        AddQueryParameter(ref fieldList, "ownership_doc_date", "oddt", Reports1NFUtils.GetDateValue(controls, "EditOwnershipDocDate"), parameters);

        AddQueryParameter(ref fieldList, "balans_doc_type", "bdtyp", Reports1NFUtils.GetDropDownValue(controls, "ComboBalansDocKind"), parameters);
        AddQueryParameter(ref fieldList, "balans_doc_num", "bdnum", Reports1NFUtils.GetEditText(controls, "EditBalansDocNum"), parameters);
        AddQueryParameter(ref fieldList, "balans_doc_date", "bddt", Reports1NFUtils.GetDateValue(controls, "EditBalansDocDate"), parameters);

        AddQueryParameter(ref fieldList, "purpose_group_id", "pgr", Reports1NFUtils.GetDropDownValue(controls, "ComboPurposeGroup"), parameters);
        AddQueryParameter(ref fieldList, "purpose_id", "purp", Reports1NFUtils.GetDropDownValue(controls, "ComboPurpose"), parameters);
        AddQueryParameter(ref fieldList, "purpose_str", "purpstr", Reports1NFUtils.GetEditText(controls, "EditPurposeStr"), parameters);

        AddQueryParameter(ref fieldList, "bti_id", "btiid", Reports1NFUtils.GetEditText(controls, "EditObjBTICondition"), parameters);
        AddQueryParameter(ref fieldList, "reestr_no", "reesno", Reports1NFUtils.GetEditText(controls, "EditObjBtiReestrNo"), parameters);
        AddQueryParameter(ref fieldList, "date_bti", "btidt", Reports1NFUtils.GetDateValue(controls, "EditObjBtiDate"), parameters);
        AddQueryParameter(ref fieldList, "obj_bti_code", "btic", Reports1NFUtils.GetEditText(controls, "EditCaseNumber"), parameters);

        AddQueryParameter(ref fieldList, "cost_balans", "cbal", Reports1NFUtils.GetEditNumeric(controls, "EditObjCostBalans"), parameters);
        AddQueryParameter(ref fieldList, "cost_zalishkova", "czal", Reports1NFUtils.GetEditNumeric(controls, "EditObjCostZalishkova"), parameters);
        AddQueryParameter(ref fieldList, "znos", "zn", Reports1NFUtils.GetEditNumeric(controls, "EditObjZnos"), parameters);
        AddQueryParameter(ref fieldList, "znos_date", "zndt", Reports1NFUtils.GetDateValue(controls, "EditObjZnosDate"), parameters);
        AddQueryParameter(ref fieldList, "cost_expert_total", "costexp", Reports1NFUtils.GetEditNumeric(controls, "EditCostExpertTotal"), parameters);
        AddQueryParameter(ref fieldList, "date_expert", "dtexp", Reports1NFUtils.GetDateValue(controls, "EditDateExpert"), parameters);
        AddQueryParameter(ref fieldList, "history_id", "hist", Reports1NFUtils.GetDropDownValue(controls, "ComboBuildingHistory"), parameters);

        // System parameters
        AddQueryParameter(ref fieldList, "modify_date", "mdt", DateTime.Now, parameters);
        AddQueryParameter(ref fieldList, "modified_by", "mby", username.Left(64), parameters);

        decimal totalFreeSquare = decimal.MinValue;
        using (SqlCommand cmdFreeSquareTotal = new SqlCommand("select sum(total_free_sqr) from reports1nf_balans_free_square where balans_id = @balans_id", connection))
        {
            cmdFreeSquareTotal.Parameters.AddWithValue("balans_id", BalansObjectID);
            using (SqlDataReader reader = cmdFreeSquareTotal.ExecuteReader())
            {
                if (reader.Read() && !reader.IsDBNull(0))
                    totalFreeSquare = (decimal)reader.GetValue(0);
                reader.Close();
            }
        }

        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_balans SET " + fieldList + ", is_valid = @isValid, validation_errors = @errMsgs WHERE report_id = @rid AND id = @bid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.Parameters.Add(new SqlParameter("bid", BalansObjectID));

            cmd.Parameters.Add(new SqlParameter("isValid", validator.IsValid));
            cmd.Parameters.Add(new SqlParameter("errMsgs", string.Join("<br/>", validator.ValidationErrorMessages)));

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            if (totalFreeSquare > decimal.MinValue)
            {
                cmd.Parameters["sqrfr"].Value = totalFreeSquare;
                cmd.Parameters["isfr"].Value = 1;
            }

            cmd.ExecuteNonQuery();
        }

        // Add all additional comments entered by user
        string buildingTotalSqrComment = Reports1NFUtils.GetEditText(controls, "EditBuildingSqrTotalComment");

        if (buildingTotalSqrComment.Length > 0)
        {
            Reports1NFUtils.AddComment(connection, ReportID, buildingTotalSqrComment, 0, BalansObjectID, 0, 0, 0, "EditBuildingSqrTotal",
                Reports1NFUtils.GetControlTitle(controls, "EditBuildingSqrTotal"), false, false);
        }

        string buildingNonHabitSqrComment = Reports1NFUtils.GetEditText(controls, "EditBuildingSqrNonHabitComment");

        if (buildingNonHabitSqrComment.Length > 0)
        {
            Reports1NFUtils.AddComment(connection, ReportID, buildingNonHabitSqrComment, 0, BalansObjectID, 0, 0, 0, "EditBuildingSqrNonHabit",
                Reports1NFUtils.GetControlTitle(controls, "EditBuildingSqrNonHabit"), false, false);
        }

        string buildingHabitSqrComment = Reports1NFUtils.GetEditText(controls, "EditBuildingSqrHabitComment");

        if (buildingHabitSqrComment.Length > 0)
        {
            Reports1NFUtils.AddComment(connection, ReportID, buildingHabitSqrComment, 0, BalansObjectID, 0, 0, 0, "EditBuildingSqrHabit",
                Reports1NFUtils.GetControlTitle(controls, "EditBuildingSqrHabit"), false, false);
        }

        string objectSqrComment = Reports1NFUtils.GetEditText(controls, "EditObjSqrTotalComment");

        if (objectSqrComment.Length > 0)
        {
            Reports1NFUtils.AddComment(connection, ReportID, objectSqrComment, 0, BalansObjectID, 0, 0, 0, "EditObjSqrTotal",
                Reports1NFUtils.GetControlTitle(controls, "EditObjSqrTotal"), false, false);
        }

        string objectKorSqrComment = Reports1NFUtils.GetEditText(controls, "EditObjSqrKorComment");

        if (objectKorSqrComment.Length > 0)
        {
            Reports1NFUtils.AddComment(connection, ReportID, objectKorSqrComment, 0, BalansObjectID, 0, 0, 0, "EditObjSqrKor",
                Reports1NFUtils.GetControlTitle(controls, "EditObjSqrKor"), false, false);
        }

        string freeSqrComment = Reports1NFUtils.GetEditText(controls, "EditObjSqrFreeComment");

        if (freeSqrComment.Length > 0)
        {
            Reports1NFUtils.AddComment(connection, ReportID, freeSqrComment, 0, BalansObjectID, 0, 0, 0, "EditObjFreeSquare",
                Reports1NFUtils.GetControlTitle(controls, "EditObjFreeSquare"), false, false);
        }

        object sqrTotal = Reports1NFUtils.GetEditNumeric(controls, "EditObjSqrTotal");
        if ((sqrTotal is decimal) && (sqrTotal != null) && (((decimal)sqrTotal) == -1)) //pgv   -1 - ознака для видалення об'єкту
        {
            GUKV.Conveyancing.DB.MarkBalansObjectAsDeleted(connection, null, BalansObjectID, null, null, -1, true, false, -1);
            DevExpress.Web.ASPxWebControl.RedirectOnCallback(Page.ResolveClientUrl("~/Reports1NF/OrgBalansList.aspx?rid=" + ReportID.ToString()));
        }


       SavePhotoChanges();

		//!!SqlCommand cmd23 = new SqlCommand("rollback", connection);
		//!!cmd23.ExecuteNonQuery();


		//// commit inserted items
		//using (SqlCommand cmd = new SqlCommand("update reports1nf_photos set status = 0 where status = 1 and bal_id = @bid", connection))
		//{
		//    cmd.Parameters.Add(new SqlParameter("bid", BalansObjectID));
		//    cmd.ExecuteNonQuery();
		//}

		//// remove deleted
		//using (SqlCommand cmd = new SqlCommand("select id, file_ext from reports1nf_photos where status = 2 and bal_id = @bid", connection))
		//{
		//    cmd.Parameters.AddWithValue("bid", BalansObjectID);
		//    using (SqlDataReader r = cmd.ExecuteReader())
		//    {
		//        while (r.Read())
		//            Reports1NFUtils.DeleteBalansPhotoFile(r.GetInt32(0), BalansObjectID, r.GetString(1), "1NF");

		//        r.Close();
		//    }
		//}

		//using (SqlCommand cmd = new SqlCommand("delete from reports1nf_photos where status = 2 and bal_id = @bid", connection))
		//{
		//    cmd.Parameters.Add(new SqlParameter("bid", BalansObjectID));
		//    cmd.ExecuteNonQuery();
		//}
	}

	private void SavePhotoChanges()
    {
        string balansIdStr = Request.QueryString["bid"];
        Int32 newId = 0;
        string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
        //string serverLocalBalansObjectFolder = Path.Combine(photoRootPath, "Balans", balansIdStr);
        string serverLocal1NFObjectFolder = Path.Combine(photoRootPath, "1NF", balansIdStr);

		//if (!Directory.Exists(TempPhotoFolder())) throw new Exception("Temp Photo Folder not found");

        if (Directory.Exists(serverLocal1NFObjectFolder))
        {
            foreach (string fileToDelete in Directory.GetFiles(serverLocal1NFObjectFolder))
            {
                File.Delete(fileToDelete);
            }

        }

        SqlConnection connection = Utils.ConnectToDatabase();
        SqlTransaction trans = connection.BeginTransaction();

        try
        {
            using (SqlCommand cmd = new SqlCommand("delete from reports1nf_photos where bal_id = @bid", connection, trans))
            {
                cmd.Parameters.AddWithValue("bid", int.Parse(balansIdStr));
                cmd.ExecuteNonQuery();
            }

			var allfiles = Directory.GetFiles(TempPhotoFolder());
            foreach (string filePath in allfiles)
            {
				var dbfile = PhotoUtils.LocalFilename2DbFilename(filePath);
				string fullPath = string.Empty;
				
				SqlCommand cmd = new SqlCommand("INSERT INTO reports1nf_photos (bal_id, file_name, file_ext, user_id, create_date) VALUES (@balid, @filename, @fileext, @usrid, @createdate); ; SELECT CAST(SCOPE_IDENTITY() AS int)", connection, trans);
                cmd.Parameters.Add(new SqlParameter("balid", int.Parse(balansIdStr)));
                cmd.Parameters.Add(new SqlParameter("filename", dbfile.file_name));
                cmd.Parameters.Add(new SqlParameter("fileext", dbfile.file_ext));
                cmd.Parameters.Add(new SqlParameter("usrid", (Guid)System.Web.Security.Membership.GetUser().ProviderUserKey));
                cmd.Parameters.Add(new SqlParameter("createdate", DateTime.Now));
				newId = (Int32)cmd.ExecuteScalar();

                fullPath = System.IO.Path.Combine(serverLocal1NFObjectFolder, newId.ToString() + System.IO.Path.GetExtension(filePath));
                //fullPath = Path.Combine(TempPhotoFolder(), e.UploadedFile.FileName);

                if (!System.IO.Directory.Exists(serverLocal1NFObjectFolder))
                    System.IO.Directory.CreateDirectory(serverLocal1NFObjectFolder);

                File.Copy(filePath, fullPath);
                //File.Delete(filePath);

                //System.IO.FileStream file = System.IO.File.Create(fullPath);
                //file.Write(e.UploadedFile.FileBytes, 0, (int)e.UploadedFile.ContentLength);
                //file.Flush();
                //file.Close();

            }

            trans.Commit();
            connection.Close();
        }
        catch
        {
            trans.Rollback();
            throw;
        }

		//pgv
		//if (Directory.Exists(TempPhotoFolder()))
		//	Directory.Delete(TempPhotoFolder(),true);
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
//////
        try
        {
			if (e.Parameter.StartsWith("save:"))
			{
				SqlConnection connectionSql = Utils.ConnectToDatabase();

				if (connectionSql != null)
				{
					SaveChanges(connectionSql);

					connectionSql.Close();
				}
			}
			else if (e.Parameter.StartsWith("send:"))
			{
				//mode = "send";
				validator.ValidateUI();
				this.errorForm.DataSource = validator.FormatErrorDataSource();
				this.errorForm.DataBind();

				SqlConnection connectionSql = Utils.ConnectToDatabase();

				if (connectionSql != null)
				{
					// Save the form before sending it to DKV
					SaveChanges(connectionSql);

					validator.ValidateDB(connectionSql, "reports1nf_balans", string.Format("report_id = {0} and id = {1}", ReportID, BalansObjectID), true);

					if (validator.IsValid)
					{
						System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
						string username = (user == null ? "System" : user.UserName);
						Reports1NFUtils.SendBalansObject(connectionSql, /*connection1NF,*/ ReportID, BalansObjectID, username);
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

				SqlConnection connectionSql = Utils.ConnectToDatabase();

				if (connectionSql != null)
				{
					DiscardChanges(connectionSql);

					connectionSql.Close();
				}

			}

			StatusForm.DataBind();
			AddressForm.DataBind();
			BalansObjForm.DataBind();
			BalansDocsForm.DataBind();
			BalansCostForm.DataBind();
			FormViewState.DataBind();
			EnableControlsBasingOnUserRole();
	//////
        }
        catch (Exception ex)
        {
            var lognet = log4net.LogManager.GetLogger("ReportWebSite");
            lognet.Debug("--------------- Orgbalansobject CPMainPanel_Callback ----------------", ex);
            throw ex;
        }

    }

    private void DiscardChanges(SqlConnection connectionSql)
    {

        foreach (string file in Directory.GetFiles(TempPhotoFolder()))
        {
            if (File.Exists(file))
                File.Delete(file);
        }

        CopySourceFiles(Request.QueryString["bid"]);

		//PrepareTempPhotoFolder();

		//!!! if (Directory.Exists(TempPhotoFolder()))
		//!!! Directory.Delete(TempPhotoFolder(),true);



		//using (SqlCommand cmd = new SqlCommand("select id, file_ext from reports1nf_photos where status = 1 and bal_id = @bid", connectionSql))
		//{
		//    cmd.Parameters.AddWithValue("bid", BalansObjectID);
		//    using (SqlDataReader r = cmd.ExecuteReader())
		//    {
		//        while (r.Read())
		//            Reports1NFUtils.DeleteBalansPhotoFile(r.GetInt32(0), BalansObjectID, r.GetString(1), "1NF");

		//        r.Close();
		//    }
		//}

		//using (SqlCommand cmd = new SqlCommand("delete from reports1nf_photos where status = 1 and bal_id = @bid", connectionSql))
		//{
		//    cmd.Parameters.AddWithValue("bid", BalansObjectID);
		//    cmd.ExecuteNonQuery();
		//}

		//using (SqlCommand cmd = new SqlCommand("update reports1nf_photos set status = 0 where status = 2 and bal_id = @bid", connectionSql))
		//{
		//    cmd.Parameters.AddWithValue("bid", BalansObjectID);
		//    cmd.ExecuteNonQuery();
		//}
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

    public string EvaluateTrimStr(object str)
    {
        if (str is string)
        {
            return ((string)str).Trim();
        }

        return "";
    }

    public string EvaluateSignature(object modifiedBy, object modifyDate)
    {
        string userName = (modifiedBy is string) ? (string)modifiedBy : Resources.Strings.SignatureUnknownUser;
        string date = (modifyDate is DateTime) ? ((DateTime)modifyDate).ToShortDateString() + " " + ((DateTime)modifyDate).ToShortTimeString() : Resources.Strings.SignatureUnknownDate;

        return string.Format(Resources.Strings.SignatureObjCard, userName, date);
    }

    //protected void ValidateEditors()
    //{
        //validator.ValidateUI();
        //validator.ValidateDB("reports1nf_balans", string.Format("report_id = {0} and id = {1}", ReportID, BalansObjectID), true);

        //ErrorMessages.Clear();
        //ASPxTextEdit.ValidateEditorsInContainer(CPMainPanel);
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

    protected void ASPxUploadPhotoControl_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {
        //this.LoadViewState("PageUniqueKey");

        string balansIdStr = Request.QueryString["bid"];
        //Int32 newId = 0;

        string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
        //string serverLocalBalansObjectFolder = Path.Combine(photoRootPath, "Balans", balansIdStr);
        string serverLocal1NFObjectFolder = Path.Combine(photoRootPath, "1NF", balansIdStr);


        //string serverLocalBalansObjectFolder = Server.MapPath(String.Format(@"~\ImgContent\1NF\{0}\", balansIdStr));

        string fullPath = string.Empty;

        //SqlConnection connection = Utils.ConnectToDatabase();
        //SqlTransaction trans = connection.BeginTransaction();
        try
        {
            //SqlCommand cmd = new SqlCommand("INSERT INTO reports1nf_photos (bal_id, file_name, file_ext, user_id, create_date, status) VALUES (@balid, @filename, @fileext, @usrid, @createdate, 1); ; SELECT CAST(SCOPE_IDENTITY() AS int)", connection, trans);
            //cmd.Parameters.Add(new SqlParameter("balid", int.Parse(balansIdStr)));
            //cmd.Parameters.Add(new SqlParameter("filename", System.IO.Path.GetFileNameWithoutExtension(e.UploadedFile.FileName)));
            //cmd.Parameters.Add(new SqlParameter("fileext", System.IO.Path.GetExtension(e.UploadedFile.FileName)));
            //cmd.Parameters.Add(new SqlParameter("usrid", (Guid)System.Web.Security.Membership.GetUser().ProviderUserKey));
            //cmd.Parameters.Add(new SqlParameter("createdate", DateTime.Now));
            //newId = (Int32)cmd.ExecuteScalar();

            //fullPath = System.IO.Path.Combine(serverLocal1NFObjectFolder, newId.ToString() + System.IO.Path.GetExtension(e.UploadedFile.FileName));
            

            if (!System.IO.Directory.Exists(serverLocal1NFObjectFolder))
                System.IO.Directory.CreateDirectory(serverLocal1NFObjectFolder);

			PhotoUtils.AddUploadedFile(TempPhotoFolder(), e.UploadedFile.FileName, e.UploadedFile.FileBytes);

			/*
			fullPath = Path.Combine(TempPhotoFolder(), e.UploadedFile.FileName);
			System.IO.FileStream file = System.IO.File.Create(fullPath);
            file.Write(e.UploadedFile.FileBytes, 0, (int)e.UploadedFile.ContentLength);
            file.Flush();
            file.Close();
			*/

			//////var fullPath2 = Path.Combine(TempPhotoFolder(), "qq" + Guid.NewGuid() + ".jpg");
			//////File.Copy(fullPath, fullPath2, true);


			//trans.Commit();
			//connection.Close();

		}
		catch(Exception ex)
        {
            //trans.Rollback();
            if ((!string.IsNullOrEmpty(fullPath)) && (System.IO.File.Exists(fullPath)))
                System.IO.File.Delete(fullPath);
			throw ex;
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


        string balId = Request.QueryString["bid"];

        if (imageGalleryDemo != null)
        {
            if (imageGalleryDemo.Items.Count == 0)
                BindImageGallery(balId);

            ImageGalleryItem item = imageGalleryDemo.Items[int.Parse(indexStr)];
            FileAttachment drv = (FileAttachment)item.DataItem;
            //int elementToDelete = int.Parse(drv.ID);
            string fileExt = Path.GetExtension(drv.Name);

            if (File.Exists(drv.Name))
                File.Delete(drv.Name);


            //Reports1NFUtils.DeleteBalansPhotoFileItem(elementToDelete, int.Parse(balId), fileExt, "1NF", "reports1nf_photos");
            imageUrl = item.ImageUrl;

        }
        return imageUrl;
    }

    public static DateTime AddBusinessDays(DateTime date, int days)
    {
        if (days < 0)
        {
            throw new ArgumentException("days cannot be negative", "days");
        }

        if (days == 0) return date;

        if (date.DayOfWeek == DayOfWeek.Saturday)
        {
            date = date.AddDays(2);
            days -= 1;
        }
        else if (date.DayOfWeek == DayOfWeek.Sunday)
        {
            date = date.AddDays(1);
            days -= 1;
        }

        date = date.AddDays(days / 5 * 7);
        int extraDays = days % 5;

        if ((int)date.DayOfWeek + extraDays > 5)
        {
            extraDays += 2;
        }

        return date.AddDays(extraDays);

    }

    protected void ASPxGridViewFreeSquare_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
    {
		e.NewValues["is_included"] = true;
	}

    protected void SqlDataSourceFreeSquare_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["rid"]))
            e.Command.Parameters["@report_id"].Value = int.Parse(Request.QueryString["rid"]);

        if (!string.IsNullOrEmpty(Request.QueryString["bid"]))
            e.Command.Parameters["@balans_id"].Value = int.Parse(Request.QueryString["bid"]);

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

        if (!string.IsNullOrEmpty(Request.QueryString["bid"]))
            e.Command.Parameters["@balans_id"].Value = int.Parse(Request.QueryString["bid"]);

        e.Command.Parameters["@modify_date"].Value = DateTime.Now;
        MembershipUser user = Membership.GetUser();
        e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;

		var b = 10;
    }

    //protected void ObjectDataSourcePhotoFiles_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    //{
    //    //if (Request.Cookies["RecordID"] != null)
    //    //    e.InputParameters["id"] = Request.Cookies["RecordID"].Value;
    //}

    protected void ObjectDataSourcePhotoFiles_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        if (Request.Cookies["RecordID"] != null)
            e.InputParameters["RecordID"] = Request.Cookies["RecordID"].Value;

        //if (Request.QueryString["bid"] != null)
        //    e.InputParameters["balans_id"] = int.Parse(Request.QueryString["bid"]);
    }

    //protected void ObjectDataSourcePhotoFiles_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    //{
    //    // SELECTing appears to bind its parameter values from cookies just fine
    //}

    //protected void ASPxGridViewFreeSquare_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    //{
    //    if (string.IsNullOrEmpty(e.NewValues["AddrStreet"] as string))
    //    {
    //        e.NewValues["GeocodingId"] = null;
    //    }
    //    else if ((e.OldValues["AddrStreet"] as string) != (e.NewValues["AddrStreet"] as string)
    //        || (e.OldValues["AddrNumber"] as string) != (e.NewValues["AddrNumber"] as string)
    //        || e.NewValues["GeocodingId"] == null)
    //    {
    //        e.NewValues["GeocodingId"] = 1;
    //    }
    //}

    //protected void SqlDataSourceFreeSquare_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    //{
    //    if (e.Exception == null || e.ExceptionHandled)
    //        FileAttachment.DeleteAll(FileAttachmentTag, (int)e.Command.Parameters["@Id"].Value);
    //}

    protected void ASPxGridViewFreeSquare_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        var form_of_ownership = (e.OldValues["form_of_ownership"] ?? "").ToString();

        var komis_protocol = (e.OldValues["komis_protocol"] == null ? "" : e.OldValues["komis_protocol"].ToString().Trim());
        if (komis_protocol != "" && !komis_protocol.StartsWith("0") && form_of_ownership != "КОМУНАЛЬНА (СФЕРА УПРАВЛІННЯ РДА)")
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

    protected void errorForm_DataBound(object sender, EventArgs e)
    {
        Repeater r = (Repeater)Utils.FindControlRecursive(errorForm, "errorList");
        if (r != null)
        {
            r.DataSource = errorForm.DataSource;
            r.DataBind();
        }

    }

    protected void ContentCallback_Callback(object sender, CallbackEventArgsBase e)
    {


        if (e.Parameter.ToLower().StartsWith("deleteimage:"))
        {
            DeleteImage(e.Parameter.Split(':')[1], string.Empty);
            BindImageGallery(Request.QueryString["bid"]);
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
                BindImageGallery(Request.QueryString["bid"]);

            }
        }
    }
    protected void CPMainPanel_Unload(object sender, EventArgs e)
    {

    }

    protected string GetPageUniqueKey()
    {
        string keyName = "PageUniqueKey_" + Request.QueryString["bid"];

        object key = ViewState[keyName];

        if (key is string)
        {
            return (string)key;
        }

        // Generate unique key
        Guid guid = Guid.NewGuid();

        string str = guid.ToString();

        ViewState[keyName] = str;

        return str;
    }

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
            string balansIdStr = Request.QueryString["bid"];
            string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
            string destFolder = Path.Combine(photoRootPath, "1NF_" + balansIdStr + "_" + PhotoFolderID.ToString()).ToLower();
//            string destFolder = Path.Combine(photoRootPath, "Tmp","1NF_" + balansIdStr + "_" + PhotoFolderID.ToString()).ToLower();


            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            return destFolder;
        }
        else
            return string.Empty;
    }

    private void PrepareTempPhotoFolder()
    {
        string balansIdStr = Request.QueryString["bid"];

        if (PhotoFolderID == Guid.Empty)
        {
            PhotoFolderID = Guid.NewGuid();

            CopySourceFiles(balansIdStr);
        }

        BindImageGallery(balansIdStr);
    }

    private void CopySourceFiles(string balansIdStr)
    {


        string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];

        string destFolder = TempPhotoFolder();

        if (!Directory.Exists(destFolder))
            Directory.CreateDirectory(destFolder);
        else
        {
            //Directory.
        }

        //var security = Directory.GetAccessControl(destFolder);
        //security.AddAccessRule(
        //    new FileSystemAccessRule(
        //        new SecurityIdentifier(WellKnownSidType.AnonymousSid, null),
        //        FileSystemRights.FullControl,
        //        InheritanceFlags.ObjectInherit,
        //        PropagationFlags.InheritOnly,
        //        AccessControlType.Allow
        //    )
        //);
        //Directory.SetAccessControl(destFolder, security);




        SqlConnection connection = Utils.ConnectToDatabase();
        using (SqlCommand cmd = new SqlCommand("select id, file_name, file_ext from reports1nf_photos where bal_id = @bid", connection))
        {
            cmd.Parameters.AddWithValue("bid", balansIdStr);
            using (SqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    int id = r.GetInt32(0);
                    string file_name = r.GetString(1);
                    string file_ext = r.GetString(2);

                    string sourceFileToCopy = Path.Combine(photoRootPath, "1NF", balansIdStr, id.ToString() + file_ext);
                    if (File.Exists(sourceFileToCopy))
                    {
                        string destFileToCopy = Path.Combine(destFolder, PhotoUtils.DbFilename2LocalFilename(file_name, file_ext));
                        if (File.Exists(destFileToCopy))
                            File.Delete(destFileToCopy);
                        File.Copy(sourceFileToCopy, destFileToCopy);
                    }
                }

                r.Close();
            }
        }
        connection.Close();
    }

    private void BindImageGallery(string balansIdStr)
    {
        ObjectDataSourceBalansPhoto.SelectParameters["recordID"].DefaultValue = balansIdStr;
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

   private void messageBox(string p)
    {
        //        throw new NotImplementedException();
        throw new Exception(p);
    }
}