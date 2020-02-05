using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxRoundPanel;
using DevExpress.Web.ASPxClasses;
using GUKV.Common;

public partial class Reports1NF_OrgInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string reportIdStr = Request.QueryString["rid"];

        if (reportIdStr != null && reportIdStr.Length > 0)
        {
            ReportID = int.Parse(reportIdStr);

            // Set the parameters for SQL queries
            SqlDataSourceOrgProperties.SelectParameters["rep_id"].DefaultValue = ReportID.ToString();
        }

        // Check if report belongs to this user
        int reportRdaDistrictId = -1;
        int reportOrganizationId = Reports1NFUtils.GetReportOrganizationId(ReportID, ref reportRdaDistrictId);

        if (ReportID > 0)
        {
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

            int userOrg = Utils.UserOrganizationID; // Defined in Cabinet.aspx.cs

            if (userOrg > 0)
            {
                if (!ReportBelongsToThisUser.HasValue)
                {
                    ReportBelongsToThisUser = (reportOrganizationId == userOrg);
                }
            }
            else
            {
                ReportBelongsToThisUser = false;
            }
        }

        ReportCommentViewer1.ReportId = ReportID;
        ReportCommentViewer1.OrganizationId = reportOrganizationId;
        ReportCommentViewer1.AddNumberOfCommentsToButton(ButtonComments);

        // Enable / disable all controls depending on the report owner
        EnableControlsBasingOnUserRole();
    }

    protected void EnableControlsBasingOnUserRole()
    {
        bool userIsReportSubmitter = Roles.IsUserInRole(Utils.Report1NFSubmitterRole);

        Control pageControl = OrgDetails.FindControl("CardPageControl");
        ASPxRoundPanel panelPayments = null;
        if (pageControl != null)
            panelPayments = pageControl.FindControl("PanelPayments") as ASPxRoundPanel;

        if (ReportBelongsToThisUser.HasValue)
        {
            Dictionary<string, string> markedControls = Reports1NFUtils.GetMarkedControlIDs(ReportID, null, null, null, null, Utils.UserOrganizationID);

            Reports1NFUtils.EnableDevExpressEditors(OrgDetails, ReportBelongsToThisUser.Value && userIsReportSubmitter,
                markedControls);

            ButtonSave.ClientVisible = ReportBelongsToThisUser.Value & userIsReportSubmitter;
            ButtonSend.ClientVisible = ReportBelongsToThisUser.Value & userIsReportSubmitter;
            ButtonClear.ClientVisible = ReportBelongsToThisUser.Value & userIsReportSubmitter;
        }

        if (ReportID > 0)
        {
            for (int i = 0; i < SectionMenu.Items.Count; i++)
            {
                if (SectionMenu.Items[i].NavigateUrl.IndexOf('?') < 0)
                    SectionMenu.Items[i].NavigateUrl += "?rid=" + ReportID.ToString();
            }
        }

        if (!userIsReportSubmitter)
        {
            if (SectionMenu.Items.Count > 6)
                SectionMenu.Items[6].ClientVisible = false;

            Control labelNotSubmitted = OrgDetails.FindControl("LabelNotSubmitted");
            Control labelAllSubmitted = OrgDetails.FindControl("LabelAllSubmitted");
            Control labelPlaceholder = OrgDetails.FindControl("LabelPlaceholder");

            if (labelNotSubmitted is ASPxLabel)
                (labelNotSubmitted as ASPxLabel).ClientVisible = false;

            if (labelAllSubmitted is ASPxLabel)
                (labelAllSubmitted as ASPxLabel).ClientVisible = false;

            if (labelPlaceholder is ASPxLabel)
                (labelPlaceholder as ASPxLabel).ClientVisible = true;
        }

        // Enable or disable 'special payments' field
        if (panelPayments != null)
        {
            Control checkIsSpecialOrganization = panelPayments.FindControl("CheckIsSpecialOrganization");
            Control editPaymentSpecial = panelPayments.FindControl("EditPaymentSpecial_orndpymnt");

            if (checkIsSpecialOrganization is ASPxCheckBox && editPaymentSpecial is ASPxSpinEdit)
            {
                (editPaymentSpecial as ASPxSpinEdit).ClientEnabled = (checkIsSpecialOrganization as ASPxCheckBox).Checked;
            }
        }

        ReplaceContributionRateInLabels();
    }

    protected void ReplaceContributionRateInLabels()
    {
        Control pageControl = OrgDetails.FindControl("CardPageControl");

        if (pageControl != null)
        {
            Control panelPayments = pageControl.FindControl("PanelPayments");

            if (panelPayments is ASPxRoundPanel)
            {
                int rate = GetOrgContributionRate();
                string rateStr = rate.ToString() + "%";

                Control labelBudgetNarah50uah = panelPayments.FindControl("LabelBudgetNarah50UAH");
                Control labelBudgetZvit50uah = panelPayments.FindControl("LabelZvitNarah50UAH");
                Control labelBudgetPrev50uah = panelPayments.FindControl("LabelBudgetPrev50UAH");
                Control labelBudgetDebt50uah = panelPayments.FindControl("LabelBudgetDebt50UAH");

                if (labelBudgetNarah50uah is ASPxLabel)
                    (labelBudgetNarah50uah as ASPxLabel).Text = (labelBudgetNarah50uah as ASPxLabel).Text.Replace("50%", rateStr);

                if (labelBudgetZvit50uah is ASPxLabel)
                    (labelBudgetZvit50uah as ASPxLabel).Text = (labelBudgetZvit50uah as ASPxLabel).Text.Replace("50%", rateStr);

                if (labelBudgetPrev50uah is ASPxLabel)
                    (labelBudgetPrev50uah as ASPxLabel).Text = (labelBudgetPrev50uah as ASPxLabel).Text.Replace("50%", rateStr);

                if (labelBudgetDebt50uah is ASPxLabel)
                    (labelBudgetDebt50uah as ASPxLabel).Text = (labelBudgetDebt50uah as ASPxLabel).Text.Replace("50%", rateStr);
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

    protected string GetEditText(string panelID, string controlID)
    {
        Control pageControl = OrgDetails.FindControl("CardPageControl");

        if (pageControl != null)
        {
            Control panel = pageControl.FindControl(panelID);

            if (panel is ASPxRoundPanel)
            {
                Control edit = panel.FindControl(controlID);

                if (edit is ASPxTextBox)
                {
                    return (edit as ASPxTextBox).Text.Trim();
                }
                else if (edit is ASPxMemo)
                {
                    return (edit as ASPxMemo).Text.Trim();
                }
            }
        }

        return "";
    }

    protected int GetDropDownValue(string panelID, string controlID)
    {
        Control pageControl = OrgDetails.FindControl("CardPageControl");

        if (pageControl != null)
        {
            Control panel = pageControl.FindControl(panelID);

            if (panel is ASPxRoundPanel)
            {
                Control dropDown = panel.FindControl(controlID);

                if (dropDown is ASPxComboBox)
                {
                    object value = (dropDown as ASPxComboBox).Value;

                    if (value is int)
                    {
                        return (int)value;
                    }
                }
            }
        }

        return -1;
    }

    protected object GetEditNumeric(string panelID, string controlID)
    {
        Control pageControl = OrgDetails.FindControl("CardPageControl");

        if (pageControl != null)
        {
            Control panel = pageControl.FindControl(panelID);

            if (panel is ASPxRoundPanel)
            {
                Control numericEdit = panel.FindControl(controlID);

                if (numericEdit is ASPxSpinEdit)
                {
                    ASPxSpinEdit spin = numericEdit as ASPxSpinEdit;

                    if (spin.Value is decimal)
                    {
                        decimal val = (decimal)spin.Value;

                        if (spin.NumberType == SpinEditNumberType.Integer)
                        {
                            return (int)val;
                        }

                        return val;
                    }
                }
            }
        }

        return null;
    }

    protected bool GetCheckBoxValue(string panelID, string controlID)
    {
        Control pageControl = OrgDetails.FindControl("CardPageControl");

        if (pageControl != null)
        {
            Control panel = pageControl.FindControl(panelID);

            if (panel is ASPxRoundPanel)
            {
                Control checkBox = panel.FindControl(controlID);

                if (checkBox is ASPxCheckBox)
                {
                    return (checkBox as ASPxCheckBox).Checked;
                }
            }
        }

        return false;
    }

    protected int GetOrgContributionRate()
    {
        int rate = 50;

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT contribution_rate FROM reports1nf_org_info WHERE report_id = @rid", connection))
            {
                cmd.Parameters.Add(new SqlParameter("rid", ReportID));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            rate = reader.GetInt32(0);
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        return rate;
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
        else if (value is DateTime)
        {
            valueExists = true;
        }
        else if (value is decimal)
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

    protected void SaveChanges(SqlConnection connection)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        string fieldList = "";

        string zkpo = GetEditText("PanelBasics", "EditZKPO");
        string fullName = GetEditText("PanelBasics", "EditFullName");
        string shortName = GetEditText("PanelBasics", "EditShortName");

        int addrDistrictId = GetDropDownValue("PanelAddr", "ComboAddrDistrict");
        int addrStreetId = GetDropDownValue("PanelAddr", "ComboAddrStreet");
        string addrBuildingNum = GetEditText("PanelAddr", "EditAddrNum");
        string addrZipCode = GetEditText("PanelAddr", "EditAddrZipCode");
        string addrMisc = GetEditText("PanelAddr", "EditAddrMisc");

        string directorName = GetEditText("PanelContacts", "EditDirectorName");
        string directorTitle = GetEditText("PanelContacts", "EditDirectorTitle");
        string directorPhone = GetEditText("PanelContacts", "EditDirectorPhone");

        string buhgalterName = GetEditText("PanelContacts", "EditBuhgalterName");
        string buhgalterPhone = GetEditText("PanelContacts", "EditBuhgalterPhone");

        int ownershipId = GetDropDownValue("PanelInfo", "ComboOwnership");
        int formGospId = GetDropDownValue("PanelInfo", "ComboFormGosp");
        int industryId = GetDropDownValue("PanelInfo", "ComboIndustry");
        int occupationId = GetDropDownValue("PanelInfo", "ComboOccupation");
        string kved = GetEditText("PanelInfo", "EditKVED");
        int orgFormId = GetDropDownValue("PanelInfo", "ComboOrgForm");
        int vedomstvoId = GetDropDownValue("PanelInfo", "ComboVedomstvo");
        int oldOrganId = GetDropDownValue("PanelInfo", "ComboOldOrgan");

        //AddQueryParameter(ref fieldList, "zkpo_code", "zkpo", zkpo, parameters);
        AddQueryParameter(ref fieldList, "full_name", "fname", fullName, parameters);
        AddQueryParameter(ref fieldList, "short_name", "sname", shortName, parameters);

        AddQueryParameter(ref fieldList, "addr_distr_new_id", "distr", addrDistrictId, parameters);
        AddQueryParameter(ref fieldList, "addr_street_id", "strid", addrStreetId, parameters);
        AddQueryParameter(ref fieldList, "addr_nomer", "num", addrBuildingNum, parameters);
        AddQueryParameter(ref fieldList, "addr_zip_code", "zip", addrZipCode, parameters);
        AddQueryParameter(ref fieldList, "addr_misc", "amsc", addrMisc, parameters);

        AddQueryParameter(ref fieldList, "director_fio", "dfio", directorName, parameters);
        AddQueryParameter(ref fieldList, "director_phone", "dphone", directorPhone, parameters);
        AddQueryParameter(ref fieldList, "director_title", "dtitle", directorTitle, parameters);

        AddQueryParameter(ref fieldList, "buhgalter_fio", "bfio", buhgalterName, parameters);
        AddQueryParameter(ref fieldList, "buhgalter_phone", "bphone", buhgalterPhone, parameters);

        AddQueryParameter(ref fieldList, "form_ownership_id", "fown", ownershipId, parameters);
        AddQueryParameter(ref fieldList, "form_gosp_id", "fgosp", formGospId, parameters);
        AddQueryParameter(ref fieldList, "industry_id", "ind", industryId, parameters);
        AddQueryParameter(ref fieldList, "occupation_id", "occ", occupationId, parameters);
        AddQueryParameter(ref fieldList, "kved_code", "kved", kved, parameters);
        AddQueryParameter(ref fieldList, "form_id", "form", orgFormId, parameters);
        AddQueryParameter(ref fieldList, "vedomstvo_id", "ved", vedomstvoId, parameters);
        AddQueryParameter(ref fieldList, "old_organ_id", "oorg", oldOrganId, parameters);

        int physAddrDistrictId = GetDropDownValue("PanelPhysAddr", "ComboPhysAddrDistrict");
        int physAddrStreetId = GetDropDownValue("PanelPhysAddr", "ComboPhysAddrStreet");
        string physAddrBuildingNum = GetEditText("PanelPhysAddr", "EditPhysAddrNum");
        string physAddrZipCode = GetEditText("PanelPhysAddr", "EditPhysAddrZipCode");
        string physAddrMisc = GetEditText("PanelPhysAddr", "EditPhysAddrMisc");

        string directorEmail = GetEditText("PanelContacts", "EditDirectorEmail");
        string buhgalterEmail = GetEditText("PanelContacts", "EditBuhgalterEmail");

        int contribution_rate = GetDropDownValue("PanelInfo", "contribution_rate");

        AddQueryParameter(ref fieldList, "director_email", "demail", directorEmail, parameters);
        AddQueryParameter(ref fieldList, "buhgalter_email", "bemail", buhgalterEmail, parameters);

        AddQueryParameter(ref fieldList, "phys_addr_district_id", "pdistr", physAddrDistrictId, parameters);
        AddQueryParameter(ref fieldList, "phys_addr_street_id", "pstrid", physAddrStreetId, parameters);
        AddQueryParameter(ref fieldList, "phys_addr_nomer", "pnum", physAddrBuildingNum, parameters);
        AddQueryParameter(ref fieldList, "phys_addr_zip_code", "pzip", physAddrZipCode, parameters);
        AddQueryParameter(ref fieldList, "phys_addr_misc", "pamsc", physAddrMisc, parameters);

        AddQueryParameter(ref fieldList, "contribution_rate", "contribution_rate", contribution_rate, parameters);

        AddQueryParameter(ref fieldList, "payment_budget_special", "paysp", GetEditNumeric("PanelPayments", "EditPaymentSpecial_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "budget_narah_50_uah", "budg50n", GetEditNumeric("PanelPayments", "EditBudgetNarah50_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "budget_zvit_50_uah", "budg50z", GetEditNumeric("PanelPayments", "EditBudgetZvit50_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "budget_prev_50_uah", "budg50p", GetEditNumeric("PanelPayments", "EditBudgetPrev50_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "budget_debt_30_50_uah", "budg50o", GetEditNumeric("PanelPayments", "EditBudgetDebtOld50_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "is_special_organization", "isSpecOrg", GetCheckBoxValue("PanelPayments", "CheckIsSpecialOrganization") ? 1 : 0, parameters);

        AddQueryParameter(ref fieldList, "konkurs_payments", "paykon", GetEditNumeric("PanelUnknownPayments", "EditKonkursPayments_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "unknown_payments", "payun", GetEditNumeric("PanelUnknownPayments", "EditUnknownPayments_orndpymnt"), parameters);
        AddQueryParameter(ref fieldList, "unknown_payment_note", "payunnote", GetEditText("PanelUnknownPayments", "MemoUnknownPayments_orndpymnt"), parameters);

        // System parameters
        AddQueryParameter(ref fieldList, "modify_date", "mdt", DateTime.Now, parameters);
        AddQueryParameter(ref fieldList, "modified_by", "mby", username.Left(64), parameters);

        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_org_info SET " + fieldList + " WHERE report_id = @rid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            cmd.ExecuteNonQuery();
        }

        // Update the street names by the street code
        string query = @"UPDATE org_info SET org_info.addr_street_name = streets.name
            FROM reports1nf_org_info AS org_info
            INNER JOIN dict_streets AS streets ON org_info.addr_street_id = streets.id
            WHERE org_info.report_id = @rid";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", ReportID));
            cmd.ExecuteNonQuery();
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
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                // Save changes before sending them to DKV
                SaveChanges(connection);

                Reports1NFUtils.SendOrganizationInfo(connection, ReportID);

                connection.Close();
            }
        }
        else if (e.Parameter.StartsWith("clear:"))
        {
            // No need to do anything. OrgDetails.DataBind() will do the trick
        }

        OrgDetails.DataBind();
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
}