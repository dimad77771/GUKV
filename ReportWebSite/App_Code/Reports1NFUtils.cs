using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using FirebirdSql.Data.FirebirdClient;
using DevExpress.Web;
using log4net;
using GUKV.Common;
using GUKV;
using GUKV.ImportToolUtils;
using ExtDataEntry.Models;
using System.Web.UI.WebControls;

public static class StringExtensions
{
    public static string Left(this string s, int left)
    {
        if (s.Length > left)
            return s.Substring(0, left);

        return s;
    }
}

/// <summary>
/// Contains all utility code for the web-based 1NF reporting system, including
/// the database update code
/// </summary>
public static class Reports1NFUtils
{
    private static readonly ILog log = LogManager.GetLogger("ReportWebSite");

    #region Nested types

    private class RentDecisionInfo
    {
        public int id = 0;
        public string docNum = "";
        public DateTime? docDate = null;
        public string purposeStr = "";
        public decimal rentSquare = 0m;
        public string pidstavaStr = "";
        public string modifiedBy = "";
        public DateTime? modifyDate = null;

        public bool isUpdated = false;

        public RentDecisionInfo()
        {
        }

        public SqlCommand CreateUpdateCmdSql(SqlConnection connection, int existingId)
        {
            string fields = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            AddUpdateQueryParameter(ref fields, "modified_by", modifiedBy.Length > 0 ? modifiedBy : null, parameters, 18);
            AddUpdateQueryParameter(ref fields, "modify_date", modifyDate.HasValue ? (object)modifyDate.Value : null, parameters, -1);
            AddUpdateQueryParameter(ref fields, "doc_num", docNum.Length > 0 ? docNum : null, parameters, 18);
            AddUpdateQueryParameter(ref fields, "doc_date", docDate.HasValue ? (object)docDate.Value : null, parameters, -1);
            AddUpdateQueryParameter(ref fields, "purpose_str", purposeStr.Length > 0 ? purposeStr : null, parameters, 255);
            AddUpdateQueryParameter(ref fields, "rent_square", rentSquare > 0m ? (object)rentSquare : null, parameters, -1);
            AddUpdateQueryParameter(ref fields, "pidstava", pidstavaStr.Length > 0 ? pidstavaStr : null, parameters, 255);

            SqlCommand cmd = new SqlCommand("UPDATE link_arenda_2_decisions SET " + fields.TrimStart(' ', ',') + " WHERE id = " + existingId.ToString(), connection);

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            return cmd;
        }

        public FbCommand CreateUpdateCmd1NF(FbConnection connection1NF, int existingId)
        {
            string fields = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            AddUpdateQueryParameter(ref fields, "ISP", modifiedBy.Length > 0 ? modifiedBy.Left(18) : null, parameters, 18);
            AddUpdateQueryParameter(ref fields, "DT", modifyDate.HasValue ? (object)modifyDate.Value : null, parameters, -1);
            AddUpdateQueryParameter(ref fields, "NOM", docNum.Length > 0 ? docNum : null, parameters, 18);
            AddUpdateQueryParameter(ref fields, "DT_DOC", docDate.HasValue ? (object)docDate.Value : null, parameters, -1);
            AddUpdateQueryParameter(ref fields, "PURP_STR", purposeStr.Length > 0 ? purposeStr : null, parameters, 255);
            AddUpdateQueryParameter(ref fields, "SQUARE", rentSquare > 0m ? (object)rentSquare : null, parameters, -1);
            AddUpdateQueryParameter(ref fields, "PIDSTAVA", pidstavaStr.Length > 0 ? pidstavaStr : null, parameters, 255);

            FbCommand cmd = new FbCommand("UPDATE ARRISH SET " + fields.TrimStart(' ', ',') + " WHERE ID = " + existingId.ToString(), connection1NF);

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new FbParameter(param.Key, param.Value));
            }

            return cmd;
        }

        private void AddUpdateQueryParameter(ref string fields, string fieldName, object value, Dictionary<string, object> parameters, int maxStringLen)
        {
            if (value != null)
            {
                if (value is string && ((string)value).Length > maxStringLen)
                {
                    value = ((string)value).Substring(0, maxStringLen);
                }

                fields += ", " + fieldName + " = @" + fieldName;
                parameters.Add(fieldName, value);
            }
            else
            {
                fields += ", " + fieldName + " = NULL";
            }
        }

        public SqlCommand CreateInsertCmdSql(SqlConnection connection, int arendaId)
        {
            string fields = "";
            string values = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            AddInsertQueryParameter(ref fields, ref values, "arenda_id", arendaId, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "modified_by", modifiedBy.Length > 0 ? modifiedBy : null, parameters, 18);
            AddInsertQueryParameter(ref fields, ref values, "modify_date", modifyDate.HasValue ? (object)modifyDate.Value : null, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "doc_num", docNum.Length > 0 ? docNum : null, parameters, 18);
            AddInsertQueryParameter(ref fields, ref values, "doc_date", docDate.HasValue ? (object)docDate.Value : null, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "purpose_str", purposeStr.Length > 0 ? purposeStr : null, parameters, 255);
            AddInsertQueryParameter(ref fields, ref values, "rent_square", rentSquare > 0m ? (object)rentSquare : null, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "pidstava", pidstavaStr.Length > 0 ? pidstavaStr : null, parameters, 255);

            SqlCommand cmd = new SqlCommand("INSERT INTO link_arenda_2_decisions (" + fields.TrimStart(' ', ',') + ") VALUES (" + values.TrimStart(' ', ',') + ")", connection);

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
            }

            return cmd;
        }

        public FbCommand CreateInsertCmd1NF(FbConnection connection1NF, int arendaId, int linkId)
        {
            string fields = "";
            string values = "";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            AddInsertQueryParameter(ref fields, ref values, "ID", linkId, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "ARENDA", arendaId, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "ISP", modifiedBy.Length > 0 ? modifiedBy.Left(18) : null, parameters, 18);
            AddInsertQueryParameter(ref fields, ref values, "DT", modifyDate.HasValue ? (object)modifyDate.Value : null, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "NOM", docNum.Length > 0 ? docNum : null, parameters, 18);
            AddInsertQueryParameter(ref fields, ref values, "DT_DOC", docDate.HasValue ? (object)docDate.Value : null, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "PURP_STR", purposeStr.Length > 0 ? purposeStr : null, parameters, 255);
            AddInsertQueryParameter(ref fields, ref values, "SQUARE", rentSquare > 0m ? (object)rentSquare : null, parameters, -1);
            AddInsertQueryParameter(ref fields, ref values, "PIDSTAVA", pidstavaStr.Length > 0 ? pidstavaStr : null, parameters, 255);

            FbCommand cmd = new FbCommand("INSERT INTO ARRISH (" + fields.TrimStart(' ', ',') + ") VALUES (" + values.TrimStart(' ', ',') + ")", connection1NF);

            foreach (KeyValuePair<string, object> param in parameters)
            {
                cmd.Parameters.Add(new FbParameter(param.Key, param.Value));
            }

            return cmd;
        }

        private void AddInsertQueryParameter(ref string fields, ref string values, string fieldName, object value, Dictionary<string, object> parameters, int maxStringLen)
        {
            if (value != null)
            {
                if (value is string && ((string)value).Length > maxStringLen)
                {
                    value = ((string)value).Substring(0, maxStringLen);
                }

                fields += ", " + fieldName;
                values += ", @" + fieldName;
                parameters.Add(fieldName, value);
            }
            else
            {
                fields += ", " + fieldName;
                values += ", NULL";
            }
        }
    }

    #endregion (Nested types)

    #region UI helper functions

    public static void GetAllControls(Control parent, Dictionary<string, Control> controls)
    {
        foreach (Control child in parent.Controls)
        {
            if (child.ID != null && child.ID.Length > 0)
            {
                controls[child.ID.ToLower()] = child;
            }

            GetAllControls(child, controls);
        }
    }

    public static string GetEditText(Dictionary<string, Control> controls, string controlID)
    {
        Control ctl = null;

        if (controls.TryGetValue(controlID.ToLower(), out ctl))
        {
            if (ctl is ASPxTextBox)
            {
                return (ctl as ASPxTextBox).Text.Trim();
            }
            else if (ctl is ASPxMemo)
            {
                return (ctl as ASPxMemo).Text.Trim();
            }
        }

        return "";
    }

    public static object GetEditNumeric(Dictionary<string, Control> controls, string controlID)
    {
        Control ctl = null;

        if (controls.TryGetValue(controlID.ToLower(), out ctl))
        {
            if (ctl is ASPxSpinEdit)
            {
                ASPxSpinEdit spin = ctl as ASPxSpinEdit;

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
			else if (ctl is HiddenField)
			{
				HiddenField spin = ctl as HiddenField;
				return int.Parse(spin.Value);
			}
		}

        return null;
    }

    public static int GetDropDownValue(Dictionary<string, Control> controls, string controlID)
    {
        Control ctl = null;

        if (controls.TryGetValue(controlID.ToLower(), out ctl))
        {
            if (ctl is ASPxComboBox)
            {
                object value = (ctl as ASPxComboBox).Value;

                if (value is int)
                {
                    return (int)value;
                }
            }
        }

        return -1;
    }

    public static string GetDropDownText(Dictionary<string, Control> controls, string controlID)
    {
        Control ctl = null;

        if (controls.TryGetValue(controlID.ToLower(), out ctl))
        {
            if (ctl is ASPxComboBox)
            {
                return (ctl as ASPxComboBox).Text.Trim();
            }
        }

        return "";
    }


    public static bool GetCheckBoxValue(Dictionary<string, Control> controls, string controlID)
    {
        Control ctl = null;

        if (controls.TryGetValue(controlID.ToLower(), out ctl))
        {
            if (ctl is ASPxCheckBox)
            {
                return (ctl as ASPxCheckBox).Checked;
            }
        }

        return false;
    }

    public static object GetDateValue(Dictionary<string, Control> controls, string controlID)
    {
        Control ctl = null;

        if (controls.TryGetValue(controlID.ToLower(), out ctl))
        {
            if (ctl is ASPxDateEdit)
            {
                return (ctl as ASPxDateEdit).Value;
            }
        }

        return null;
    }

    public static bool IsRadioButtonChecked(Dictionary<string, Control> controls, string controlID)
    {
        Control ctl = null;

        if (controls.TryGetValue(controlID.ToLower(), out ctl))
        {
            if (ctl is ASPxRadioButton)
            {
                return (ctl as ASPxRadioButton).Checked;
            }
        }

        return false;
    }

    public static string GetControlTitle(Dictionary<string, Control> controls, string controlID)
    {
        Control ctl = null;

        if (controls.TryGetValue(controlID.ToLower(), out ctl))
        {
            if (ctl is ASPxEdit)
            {
                object title = (ctl as ASPxEdit).Attributes["Title"];

                if (title is string)
                {
                    return (string)title;
                }
            }
        }

        return "";
    }

    public static void EnableDevExpressEditors(Control parent, bool enable, Dictionary<string, string> markedControls,
        Func<Control, bool> controlFilter = null)
    {
        bool controlEnable = controlFilter == null ? enable : enable && controlFilter(parent);

        if (parent is ASPxEdit)
        {
            if (controlEnable)
            {
                /* if ((parent as ASPxEdit).ReadOnly)
                    (parent as ASPxEdit).ClientEnabled = false; */
            }
            else
            {
                // (parent as ASPxEdit).ClientEnabled = false;
                (parent as ASPxEdit).ReadOnly = true;

                // Read-only (disabled) controls shouldn't cause validation errors
                (parent as ASPxEdit).ValidationSettings.RequiredField.IsRequired = false;
                (parent as ASPxEdit).ValidationSettings.RegularExpression.ValidationExpression = null;
            }

            string comment = "";

            if (markedControls.TryGetValue(parent.ID.ToLower(), out comment))
            {
                // This control is marked as having WRONG DATA
                (parent as ASPxEdit).ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                (parent as ASPxEdit).Border.BorderColor = System.Drawing.Color.FromArgb(255, 0, 0);
                (parent as ASPxEdit).ToolTip = comment;
                (parent as ASPxEdit).DisabledStyle.ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
            }
            else if ((parent as ASPxEdit).ReadOnly)
            {
                // Make display of disabled fields more clear
                (parent as ASPxEdit).ForeColor = System.Drawing.Color.FromArgb(92, 92, 92);
            }

            // Attach the event that allows to know the last focused control on the client side
            if (parent is ASPxTextEdit)
            {
                (parent as ASPxTextEdit).GetClientSideEvents().SetEventHandler("GotFocus", @"function (s, e) { lastFocusedControlId = '' + s.cp_ControlName; lastFocusedControlTitle = '' + s.cp_ControlTitle; }");
                (parent as ASPxTextEdit).JSProperties["cp_ControlName"] = parent.ID;

                object title = (parent as ASPxTextEdit).Attributes["Title"];

                if (title != null)
                {
                    if (title is string)
                    {
                        (parent as ASPxTextEdit).JSProperties["cp_ControlTitle"] = (string)title;
                    }
                    else
                    {
                        throw new InvalidOperationException("Title is not defined for the control: " + parent.ID);
                    }
                }
            }
            if (parent is ASPxSpinEdit)
            {
                (parent as ASPxSpinEdit).AllowMouseWheel = false;
            }
        }
        else
        {
            foreach (Control child in parent.Controls)
            {
                EnableDevExpressEditors(child, enable, markedControls, controlFilter);
            }
        }
    }

    public static Dictionary<string, string> GetMarkedControlIDs(int reportId, int? balanceId, int? balanceDeletedId, int? arendaId, int? arendaRentedId, int? organizationId)
    {
        Dictionary<string, string> currentStates = new Dictionary<string, string>();

        SqlConnection connection = Utils.ConnectToDatabase();

        if (connection != null)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Parameters.Add(new SqlParameter("rid", reportId));

                var sqlConditions = " ";

                if (arendaId == null)
                    sqlConditions = " AND arenda_id IS NULL";
                else
                    sqlConditions = " AND arenda_id = " + arendaId.ToString();

                if (balanceId == null)
                    sqlConditions = sqlConditions + " AND balans_id IS NULL";
                else
                    sqlConditions = sqlConditions + " AND balans_id = " + balanceId.ToString();

                if (balanceDeletedId == null)
                    sqlConditions = sqlConditions + " AND balans_deleted_id IS NULL";
                else
                    sqlConditions = sqlConditions + " AND balans_deleted_id = " + balanceDeletedId.ToString();

                if (arendaRentedId == null)
                    sqlConditions = sqlConditions + " AND arenda_rented_id IS NULL";
                else
                    sqlConditions = sqlConditions + " AND arenda_rented_id = " + arendaRentedId.ToString();

                if (organizationId == null)
                    sqlConditions = sqlConditions + " AND organization_id IS NULL";
                else
                    sqlConditions = sqlConditions + " AND organization_id = " + organizationId.ToString();

                cmd.Connection = connection;
                cmd.CommandText = "SELECT control_id, is_wrong_data, comment, comment_user FROM reports1nf_comments WHERE report_id = @rid " + sqlConditions + " ORDER BY comment_date ASC";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string controlId = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        int isWrongData = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        string comment = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string commentUser = reader.IsDBNull(3) ? "" : reader.GetString(3);

                        if (controlId.Length > 0)
                        {
                            if (isWrongData == 1)
                            {
                                currentStates[controlId.ToLower()] = commentUser + ": " + comment;
                            }
                            else
                            {
                                currentStates.Remove(controlId.ToLower());
                            }
                        }
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        return currentStates;
    }

    #endregion (UI helper functions)

    #region Misc DB-related functions

    /// <summary>
    /// Attempts to connect to the 1NF database
    /// </summary>
    /// <returns>Created connection object, or NULL if connection could not be established</returns>
    //public static FbConnection ConnectTo1NF()
    //{
    //    return Utils.ConnectTo1NF();

    //    /*
    //    string connectionString = ConfigurationManager.ConnectionStrings["1NFTestConnectionString"].ConnectionString;

    //    if (connectionString != null && connectionString.Length > 0)
    //    {
    //        FbConnection connection = new FbConnection(connectionString);

    //        try
    //        {
    //            connection.Open();

    //            return connection;
    //        }
    //        catch
    //        {
    //            //
    //        }
    //    }

    //    return null;
    //    */
    //}

    public static int GetReportOrganizationId(SqlConnection connection, int reportId, out string orgName)
    {
        int reportOrganizationId = -1;
        orgName = "";

        string query = "SELECT rep.organization_id, org.full_name FROM reports1nf rep INNER JOIN reports1nf_org_info org ON org.report_id = rep.id WHERE rep.id = @rid";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    reportOrganizationId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                    orgName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                }

                reader.Close();
            }
        }

        return reportOrganizationId;
    }

    public static int GetReportOrganizationId(int reportId, ref int reportRdaDistrictId)
    {
        int reportOrganizationId = -1;
        SqlConnection connection = Utils.ConnectToDatabase();
        reportRdaDistrictId = -1;

        if (connection != null)
        {
            string query = @"SELECT rep.organization_id, org.addr_distr_new_id,
                CASE WHEN org.form_ownership_id IN (select id from dict_org_ownership where is_rda = 1) THEN 1 ELSE 0 END 'is_rda'
                FROM reports1nf rep
                INNER JOIN organizations org ON org.id = rep.organization_id and (org.is_deleted is null or org.is_deleted = 0)
                WHERE rep.id = @rid";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("rid", reportId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reportOrganizationId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        int distr_id = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                        bool is_rda = reader.IsDBNull(2) ? false : reader.GetInt32(2) == 1;
                        if (is_rda)
                            reportRdaDistrictId = distr_id;
                    }

                    reader.Close();
                }
            }

            connection.Close();
        }

        return reportOrganizationId;
    }

    public static int GetBalansBuildingUniqueId(SqlConnection connection, int reportId, int balansId, Dictionary<string, object> buildingProperties)
    {
        if (buildingProperties != null)
            buildingProperties.Clear();

        int buildingUniqueId = -1;

        using (SqlCommand cmd = new SqlCommand("SELECT building_1nf_unique_id FROM reports1nf_balans WHERE id = @bid AND report_id = @rid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.Parameters.Add(new SqlParameter("bid", balansId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        buildingUniqueId = reader.GetInt32(0);
                }

                reader.Close();
            }
        }

        // Get the building properties
        if (buildingUniqueId > 0 && buildingProperties != null)
        {
            GetUniqueBuildingProperties(connection, reportId, buildingUniqueId, buildingProperties);
        }

        return buildingUniqueId;
    }

    public static void GetUniqueBuildingProperties(SqlConnection connection, int reportId, int uniqueBuildingId, Dictionary<string, object> buildingProperties)
    {
        buildingProperties.Clear();

//pgv        using (SqlCommand cmd = new SqlCommand("SELECT * FROM reports1nf_buildings WHERE unique_id = @bid AND report_id = @rid", connection))
        using (SqlCommand cmd = new SqlCommand("SELECT id,master_building_id,addr_street_name,addr_street_id,addr_street_name2,addr_street_id2,street_full_name,addr_distr_old_id,addr_distr_new_id,addr_nomer1,addr_nomer2,addr_nomer3,addr_nomer,addr_misc,addr_korpus_flag,addr_korpus,addr_zip_code,addr_address,tech_condition_id,date_begin,date_end,num_floors,construct_year,condition_year,is_condition_valid,bti_code,history_id,object_type_id,object_kind_id,is_land,modify_date,modified_by,kadastr_code,cost_balans,tbs.sqr_total,tbs.sqr_pidval,sqr_mk,sqr_dk,sqr_rk,sqr_other,sqr_rented,sqr_zagal,sqr_for_rent,tbs.sqr_habit,tbs.sqr_non_habit,additional_info,is_deleted,del_date,oatuu_id,updpr,arch_id,arch_flag,facade_id,nomer_int,characteristics,expl_enter_year,is_basement_exists,is_loft_exists,tbs.sqr_loft,unique_id,report_id FROM reports1nf_buildings rb outer apply (select sum(b.sqr_total) sqr_total, sum(b.sqr_non_habit) sqr_non_habit, sum(b.sqr_habit) sqr_habit, sum(b.sqr_pidval) sqr_pidval, sum(b.sqr_loft) sqr_loft from reports1nf_balans bal INNER JOIN reports1nf_buildings b ON bal.building_1nf_unique_id = b.unique_id where bal.building_id = rb.id ) tbs WHERE unique_id = @bid AND report_id = @rid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.Parameters.Add(new SqlParameter("bid", uniqueBuildingId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string fieldName = reader.GetName(i).ToLower();

                        if (reader.IsDBNull(i))
                        {
                            buildingProperties[fieldName] = System.DBNull.Value;
                        }
                        else
                        {
                            buildingProperties[fieldName] = reader.GetValue(i);
                        }
                    }
                }

                reader.Close();
            }
        }
    }

    public static int GenerateUniqueBuilding(SqlConnection connection, int buildingId, int reportId)
    {
        SqlParameter outputParam = new SqlParameter("BUILDING_UNIQUE_ID", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        using (SqlCommand cmd = new SqlCommand("dbo.fnCreateUniqueBuilding", connection))
        {
            cmd.Parameters.Add(new SqlParameter("BUILDING_ID", buildingId));
            cmd.Parameters.Add(new SqlParameter("REPORT_ID", reportId));
            cmd.Parameters.Add(outputParam);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
        }

        if (outputParam.Value is int)
        {
            return (int)outputParam.Value;
        }

        return -1;
    }

    #endregion Misc DB-related functions

    #region Updating the EIS database

    public static void SendOrganizationInfo(SqlConnection connection, int reportId)
    {
        // Get the field names from the 'organizations' table
        HashSet<string> orgFieldNames = new HashSet<string>();

        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM organizations", connection))
        {
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string fieldName = reader.GetName(i).ToLower();
                    orgFieldNames.Add(fieldName);
                }

                reader.Close();
            }
        }

        int organizationId = -1;
        string fieldList = "";
        SqlCommand cmdUpdate = new SqlCommand("", connection);

        using (SqlCommand cmd = new SqlCommand("SELECT * FROM reports1nf_org_info WHERE report_id = @rid and (is_deleted is null or is_deleted = 0)", connection))
//pgv        using (SqlCommand cmd = new SqlCommand("SELECT * FROM reports1nf_org_info WHERE report_id = @rid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    // Prepare the UPDATE query from the data reader
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i).ToLower();

                        if (columnName == "id")
                        {
                            organizationId = reader.GetInt32(i);
                        }
                        else if (orgFieldNames.Contains(columnName))
                        {
                            if (reader.IsDBNull(i))
                            {
                                fieldList += ", " + columnName + " = NULL";
                            }
                            else
                            {
                                cmdUpdate.Parameters.Add(new SqlParameter("param" + i.ToString(), reader.GetValue(i)));

                                fieldList += ", " + columnName + " = @param" + i.ToString();
                            }
                        }
                    }
                }

                reader.Close();
            }
        }

        if (organizationId > 0 && fieldList.Length > 0)
        {
            System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
            string username = (user == null ? "Auto-import" : user.UserName);

            ArchiverSql.CreateOrganizationArchiveRecord(connection, organizationId, username, null);

            // Perform UPDATE
            cmdUpdate.CommandText = "UPDATE organizations SET " + fieldList.TrimStart(' ', ',') + " WHERE id = @orgid";

            cmdUpdate.Parameters.Add(new SqlParameter("orgid", organizationId));

            cmdUpdate.ExecuteNonQuery();

            // Modify the submit date
            using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_org_info SET submit_date = @sdt WHERE report_id = @rid", connection))
            {
                cmd.Parameters.Add(new SqlParameter("sdt", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("rid", reportId));
                cmd.ExecuteNonQuery();
            }

            // Mark the report as 'not viewed'
            using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf SET is_reviewed = 0 WHERE id = @rid", connection))
            {
                cmd.Parameters.Add(new SqlParameter("rid", reportId));
                cmd.ExecuteNonQuery();
            }

            if (organizationId < 300000)
            {
                // Update the same organization in 1NF database; organizations with ID >= 300000
                // belong to [Balans] database and therefore are not updated.
                //SendOrganizationInfoTo1NF(connection, new Dictionary<string, HashSet<int>>(), reportId, organizationId);
            }
        }
    }

    public static void SendBalansObject(SqlConnection connectionSql, /*FbConnection connection1NF,*/
        int reportId, int balansId, string user)
    {
        BalansObjectValidator validator = new BalansObjectValidator();
        validator.ValidateDB(connectionSql, "reports1nf_balans", string.Format("report_id = {0} and id = {1}", reportId, balansId), true);

        if (!validator.IsValid)
        {
            return;
        }

        // Get the ID of the temporary building information placeholder
        Dictionary<string, object> buildingProperties = new Dictionary<string, object>();
        int buildingUniqueId = GetBalansBuildingUniqueId(connectionSql, reportId, balansId, buildingProperties);

        if (buildingUniqueId > 0)
        {
            string number1 = "";
            string number2 = "";
            string number3 = "";
            string addrMisc = "";

            int matchBuildingId = FindObjectMatchForBalansBuilding(connectionSql, /*connection1NF,*/ buildingProperties,
                ref number1, ref number2, ref number3, ref addrMisc, true, user);

            if (matchBuildingId > 0)
            {
                // Update the associated building Id in the 'reports1nf_buildings' table
                using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_buildings SET id = @matchid WHERE unique_id = @unid AND report_id = @rid", connectionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("matchid", matchBuildingId));
                    cmd.Parameters.Add(new SqlParameter("unid", buildingUniqueId));
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));
                    cmd.ExecuteNonQuery();
                }

                // Update the associated building Id in the 'reports1nf_balans' table
                using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_balans SET building_id = @matchid, obj_nomer1 = @on1, obj_nomer2 = @on2," +
                    " obj_nomer3 = @on3, obj_addr_misc = @oad WHERE id = @bid AND report_id = @rid", connectionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("matchid", matchBuildingId));
                    cmd.Parameters.Add(new SqlParameter("bid", balansId));
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));

                    cmd.Parameters.Add(new SqlParameter("on1", number1));
                    cmd.Parameters.Add(new SqlParameter("on2", number2));
                    cmd.Parameters.Add(new SqlParameter("on3", number3));
                    cmd.Parameters.Add(new SqlParameter("oad", addrMisc));

                    cmd.ExecuteNonQuery();
                }

                // Update the Balans object information in Sql Server, and in 1NF
                SendBalansObjProperties(connectionSql, /*connection1NF,*/ reportId, balansId, "reports1nf_balans");
                
                SendBalansObjectFreeSquare(connectionSql, reportId, balansId);

                SendBalansObjectPhotos(connectionSql, reportId, balansId);
            }
            else
            {
                // ERROR: There must always be some match, found or created
            }
        }
        else
        {
            // ERROR: Invalid state of the database
        }

        // Modify the submit date for the balans object
        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_balans SET submit_date = @sdt WHERE report_id = @rid AND id = @bid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("sdt", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.Parameters.Add(new SqlParameter("bid", balansId));
            cmd.ExecuteNonQuery();
        }

        // Mark the report as 'not viewed'
        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf SET is_reviewed = 0 WHERE id = @rid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.ExecuteNonQuery();
        }
    }

    public static void SendBalansDeletedObject(SqlConnection connectionSql, /*FbConnection connection1NF,*/
        int reportId, int balansId)
    {
        // Update the Balans object information in Sql Server, and in 1NF
        SendBalansObjProperties(connectionSql, /*connection1NF,*/ reportId, balansId, "reports1nf_balans_deleted");

        // Modify the submit date for the balans object
        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_balans_deleted SET submit_date = @sdt WHERE report_id = @rid AND id = @bid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("sdt", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.Parameters.Add(new SqlParameter("bid", balansId));
            cmd.ExecuteNonQuery();
        }

        // Mark the report as 'not viewed'
        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf SET is_reviewed = 0 WHERE id = @rid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.ExecuteNonQuery();
        }
    }

    private static void SendBalansObjProperties(SqlConnection connectionSql, /*FbConnection connection1NF,*/
        int reportId, int balansId, string sourceBalansTable)
    {
        // Get the field names from the 'balans' table
        HashSet<string> balansFieldNames = new HashSet<string>();

        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM balans", connectionSql))
        {
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    balansFieldNames.Add(reader.GetName(i).ToLower());
                }

                reader.Close();
            }
        }

        Dictionary<string, string> mapping = new Dictionary<string, string>();
        GUKV.ImportToolUtils.FieldMappings.Create1NFBalansFieldMapping(mapping, false, true);

        mapping = GUKV.ImportToolUtils.FieldMappings.ReverseMapping(mapping);

        // Remove the fields that we are going to set manually
        mapping.Remove("arch_id");

        string fieldListSql = "";
        string fieldList1NF = "";
        SqlCommand cmdUpdateSql = new SqlCommand("", connectionSql);
        //FbCommand cmdUpdate1NF = new FbCommand("", connection1NF);

        using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + sourceBalansTable + " WHERE id = @bid AND report_id = @rid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("bid", balansId));
            cmd.Parameters.Add(new SqlParameter("rid", reportId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    // Prepare two UPDATE queries from the data reader
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnNameSql = reader.GetName(i).ToLower();

                        if (balansFieldNames.Contains(columnNameSql) && columnNameSql != "id" && columnNameSql != "obj_nomer") // Skip COMPUTED columns
                        {
                            // Try to find this column in the field mapping
                            string columnName1NF = "";

                            if (mapping.TryGetValue(columnNameSql, out columnName1NF))
                            {
                                if (reader.IsDBNull(i))
                                {
                                    fieldListSql += ", " + columnNameSql + " = NULL";
                                    fieldList1NF += ", " + columnName1NF + " = NULL";
                                }
                                else
                                {
                                    object value = reader.GetValue(i);

                                    cmdUpdateSql.Parameters.Add(new SqlParameter("param" + i.ToString(), reader.GetValue(i)));
                                    fieldListSql += ", " + columnNameSql + " = @param" + i.ToString();

                                    //if (CanMapBalansValueTo1NF(connection1NF, dictPool, columnName1NF, ref value))
                                    //{
                                    //    cmdUpdate1NF.Parameters.Add(new FbParameter("param" + i.ToString(), reader.GetValue(i)));
                                    //    fieldList1NF += ", " + columnName1NF + " = @param" + i.ToString();

                                    //    // The ISP field in 1NF must have the same value as EIS_MODIFIED_BY field
                                    //    if (columnName1NF == "EIS_MODIFIED_BY" && value is string)
                                    //    {
                                    //        cmdUpdate1NF.Parameters.Add(new FbParameter("isp", ((string)value).Left(18)));

                                    //        fieldList1NF += ", ISP = @isp";
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    fieldList1NF += ", " + columnName1NF + " = NULL";
                                    //}
                                }
                            }
                        }
                    }
                }

                reader.Close();
            }
        }

        // Perform UPDATE in the SQL Server
        if (fieldListSql.Length > 0)
        {
            System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
            string username = (user == null ? "Auto-import" : user.UserName);

            ArchiverSql.CreateBalansArchiveRecord(connectionSql, balansId, username, null);

            cmdUpdateSql.CommandText = "UPDATE balans SET " + fieldListSql.TrimStart(' ', ',') + " WHERE id = @bid";

            cmdUpdateSql.Parameters.Add(new SqlParameter("bid", balansId));
            cmdUpdateSql.ExecuteNonQuery();
        }

        // Perform UPDATE in the 1NF database
        //if (fieldList1NF.Length > 0)
        //{
        //    using (FbTransaction transaction = connection1NF.BeginTransaction())
        //    {
        //        try
        //        {
        //            // Create the Balans archive state before updating it
        //            GUKV.DataMigration.Archiver1NF archiver = new GUKV.DataMigration.Archiver1NF();
        //            int archiveId = archiver.CreateBalansArchiveRecord(connection1NF, balansId, transaction);

        //            if (archiveId > 0)
        //            {
        //                fieldList1NF += ", ARCH_KOD = @parch";
        //                cmdUpdate1NF.Parameters.Add(new FbParameter("parch", archiveId));
        //            }

        //            cmdUpdate1NF.CommandText = "UPDATE BALANS_1NF SET " + fieldList1NF.TrimStart(' ', ',') + " WHERE id = @bid";

        //            cmdUpdate1NF.Parameters.Add(new FbParameter("bid", balansId));
        //            cmdUpdate1NF.Transaction = transaction;
        //            cmdUpdate1NF.ExecuteNonQuery();

        //            // Commit the transaction
        //            transaction.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            // Roll back the transaction
        //            transaction.Rollback();
        //        }
        //    }
        //}
    }

    private static void SendBalansObjectFreeSquare(SqlConnection connectionSql, int reportId, int balansId)
    {
        string photoRootPath = LLLLhotorowUtils.ImgFreeSquareRootFolder;
        string photo1NFPath = Path.Combine(photoRootPath, "1NF");
        string photoBalansPath = Path.Combine(photoRootPath, "Balans");

        string query = "SELECT bsp.id,bsp.free_square_id,bsp.file_name,bsp.file_ext from balans_free_square_photos bsp where bsp.free_square_id in (select bs.id from [balans_free_square] bs where bs.balans_id = @balans_id)";
        using (SqlCommand cmd = new SqlCommand(query, connectionSql))
        {
            cmd.Parameters.AddWithValue("balans_id", balansId);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    int free_square_id = (int)reader.GetValue(1);
                    string file_name = (string)reader.GetValue(2);
                    string file_ext = (string)reader.GetValue(3);
                    string photoFolder = Path.Combine(photoBalansPath, free_square_id.ToString());
                    string fileFullPath = Path.Combine(photoFolder, file_name + file_ext);
                    LLLLhotorowUtils.Delete(fileFullPath, connectionSql);
                }
                reader.Close();
            }
        }

        query = "DELETE from balans_free_square_photos where free_square_id in (select bs.id from [balans_free_square] bs where bs.balans_id = @balans_id)";
        using (SqlCommand cmd = new SqlCommand(query, connectionSql))
        {
            cmd.Parameters.AddWithValue("balans_id", balansId);
            cmd.ExecuteNonQuery();
        }

        query = "DELETE from balans_free_square where balans_id = @balans_id";
        using (SqlCommand cmd = new SqlCommand(query, connectionSql))
        {
            cmd.Parameters.AddWithValue("balans_id", balansId);
            cmd.ExecuteNonQuery();
        }

        query = @"SELECT [id],[balans_id],[total_free_sqr],[free_sqr_korysna],[free_sqr_condition_id],[floor],[possible_using],[water],[heating],[power],[gas],[note],[modify_date],[modified_by],[is_solution],[initiator],[zgoda_control_id],[zgoda_renter_id]
            FROM [reports1nf_balans_free_square]
            WHERE [balans_id] = @balans_id AND [report_id] = @report_id";
        
        string queryInsert = @"INSERT INTO [balans_free_square] 
            ([balans_id],[total_free_sqr],[free_sqr_korysna],[free_sqr_condition_id],[floor],[possible_using],[water],[heating],[power],[gas],[note],[modify_date],[modified_by],[original_id],[is_solution],[initiator],[zgoda_control_id],[zgoda_renter_id])
            VALUES (@balans_id,@total_free_sqr,@free_sqr_korysna,@free_sqr_condition_id,@floor,@possible_using,@water,@heating,@power,@gas,@note,@modify_date,@modified_by,@original_id,@is_solution,@initiator,@zgoda_control_id,@zgoda_renter_id);
            SELECT CAST(SCOPE_IDENTITY() AS int)";

        using (SqlCommand cmdSelect = new SqlCommand(query, connectionSql))
        {
            cmdSelect.Parameters.AddWithValue("balans_id", balansId);
            cmdSelect.Parameters.AddWithValue("report_id", reportId);
            using (SqlDataReader reader = cmdSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    int balans_id = (int)reader.GetValue(1);
//                    decimal total_free_sqr = (decimal)reader.GetValue(2);
//                    decimal free_sqr_korysna = (decimal)reader.GetValue(3);
//                    int free_sqr_condition_id = (int)reader.GetValue(4);
//                    string floor = (string)reader.GetValue(5);
//                    string possible_using = (string)reader.GetValue(6);
                    bool water = (bool)reader.GetValue(7);
                    bool heating = (bool)reader.GetValue(8);
                    bool power = (bool)reader.GetValue(9);
                    bool gas = (bool)reader.GetValue(10);
//                    string note = (string)reader.GetValue(11);
                    DateTime modify_date = (DateTime)reader.GetValue(12);
                    string modified_by = (string)reader.GetValue(13);

					//var zzz = reader.GetValue(14);
					//bool is_solution = (bool)reader.GetValue(14);
					bool is_solution = true;

//                    string initiator = (string)reader.GetValue(15);
//                    int zgoda_control_id = (int)reader.GetValue(16);
//                    int zgoda_renter_id = (int)reader.GetValue(17);

                    decimal total_free_sqr = reader.IsDBNull(2) ? 0 : (decimal)reader.GetValue(2);
                    decimal free_sqr_korysna = reader.IsDBNull(3) ? 0 : (decimal)reader.GetValue(3);
                    int free_sqr_condition_id = reader.IsDBNull(4) ? 0 :  (int)reader.GetValue(4);
                    string floor = reader.IsDBNull(5) ? "" : (string)reader.GetValue(5);
                    string possible_using = reader.IsDBNull(6) ? "" : (string)reader.GetValue(6);
                    string note = reader.IsDBNull(11) ? "" : (string)reader.GetValue(11);
                    string initiator = reader.IsDBNull(15) ? "" : (string)reader.GetValue(15);
                    int zgoda_control_id = reader.IsDBNull(16) ? 0 : (int)reader.GetValue(16);
                    int zgoda_renter_id = reader.IsDBNull(17) ? 0 : (int)reader.GetValue(17);


                    int newId = 0;
                    using (SqlCommand cmdInsertBalans = new SqlCommand(queryInsert, Utils.ConnectToDatabase()))
                    {
                        cmdInsertBalans.Parameters.AddWithValue("balans_id", balansId);
                        cmdInsertBalans.Parameters.AddWithValue("total_free_sqr", total_free_sqr);
                        cmdInsertBalans.Parameters.AddWithValue("free_sqr_korysna", free_sqr_korysna);
                        cmdInsertBalans.Parameters.AddWithValue("free_sqr_condition_id", free_sqr_condition_id);
                        cmdInsertBalans.Parameters.AddWithValue("floor", floor);
                        cmdInsertBalans.Parameters.AddWithValue("possible_using", possible_using);
                        cmdInsertBalans.Parameters.AddWithValue("water", water);
                        cmdInsertBalans.Parameters.AddWithValue("heating", heating);
                        cmdInsertBalans.Parameters.AddWithValue("power", power);
                        cmdInsertBalans.Parameters.AddWithValue("gas", gas);
                        cmdInsertBalans.Parameters.AddWithValue("note", note);
                        cmdInsertBalans.Parameters.AddWithValue("modify_date", modify_date);
                        cmdInsertBalans.Parameters.AddWithValue("modified_by", modified_by);
                        cmdInsertBalans.Parameters.AddWithValue("original_id", id);
                        cmdInsertBalans.Parameters.AddWithValue("is_solution", is_solution);
                        cmdInsertBalans.Parameters.AddWithValue("initiator", initiator);
                        cmdInsertBalans.Parameters.AddWithValue("zgoda_control_id", zgoda_control_id);
                        cmdInsertBalans.Parameters.AddWithValue("zgoda_renter_id", zgoda_renter_id);

                        newId = (int)cmdInsertBalans.ExecuteScalar();
                    }

                    string querySelectPhoto = @"SELECT [id],[free_square_id],[file_name],[file_ext],[modify_date],[modified_by]
                        FROM [reports1nf_balans_free_square_photos]
                        WHERE [free_square_id] = @free_square_id";

                    string queryInsertPhoto = @"INSERT INTO [balans_free_square_photos]
                        ([free_square_id],[file_name],[file_ext],[modify_date],[modified_by],[original_id])
                        VALUES
                        (@free_square_id,@file_name,@file_ext,@modify_date,@modified_by,@original_id);
                        SELECT CAST(SCOPE_IDENTITY() AS int)";

                    using (SqlCommand cmpSelectPhoto = new SqlCommand(querySelectPhoto, Utils.ConnectToDatabase()))
                    {
                        cmpSelectPhoto.Parameters.AddWithValue("free_square_id", id);
                        using (SqlDataReader readerPhoto = cmpSelectPhoto.ExecuteReader())
                        {
                            while (readerPhoto.Read())
                            {
                                int photo_id = (int)readerPhoto.GetValue(0);
                                int free_square_id = (int)readerPhoto.GetValue(1);
                                string file_name = (string)readerPhoto.GetValue(2);
                                string file_ext = (string)readerPhoto.GetValue(3);
                                DateTime photo_modify_date = (DateTime)readerPhoto.GetValue(4);
                                string photo_modified_by = (string)readerPhoto.GetValue(5);


                                int newPhotoId = 0;
                                using (SqlCommand cmdInsertPhoto = new SqlCommand(queryInsertPhoto, Utils.ConnectToDatabase()))
                                {
                                    cmdInsertPhoto.Parameters.AddWithValue("free_square_id", newId);
                                    cmdInsertPhoto.Parameters.AddWithValue("file_name", file_name);
                                    cmdInsertPhoto.Parameters.AddWithValue("file_ext", file_ext);
                                    cmdInsertPhoto.Parameters.AddWithValue("modify_date", photo_modify_date);
                                    cmdInsertPhoto.Parameters.AddWithValue("modified_by", photo_modified_by);
                                    cmdInsertPhoto.Parameters.AddWithValue("original_id", photo_id);

                                    newPhotoId = (Int32)cmdInsertPhoto.ExecuteScalar();
                                }

                                string sourceFile = String.Format(@"{0}\{1}\{2}", photo1NFPath, free_square_id, file_name + file_ext);
                                string destFolder = Path.Combine(photoBalansPath, newId.ToString());
                                string destFile = Path.Combine(destFolder, file_name + file_ext);
                                LLLLhotorowUtils.Delete(destFile, Utils.ConnectToDatabase());
                                LLLLhotorowUtils.Copy(sourceFile, destFile, Utils.ConnectToDatabase());
                            }
                            readerPhoto.Close();
                        }
                    }

                }
                reader.Close();
            }

        }
    }

    private static void SendBalansObjectPhotos(SqlConnection connectionSql, int reportId, int balansId)
    {
        string photoRootPath = LLLLhotorowUtils.ImgContentRootFolder;
        string photo1NFPath = Path.Combine(photoRootPath, "1NF", balansId.ToString());
        string photoBalansPath = Path.Combine(photoRootPath, "Balans", balansId.ToString());

        string query = "SELECT id,file_name,file_ext from balans_photos where bal_id = @balans_id";
        using (SqlCommand cmd = new SqlCommand(query, connectionSql))
        {
            cmd.Parameters.AddWithValue("balans_id", balansId);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = (int)reader.GetValue(0);
                    string file_name = (string)reader.GetValue(1);
                    string file_ext = (string)reader.GetValue(2);
                    string fileFullPath = Path.Combine(photoBalansPath, id.ToString() + file_ext);
                    LLLLhotorowUtils.Delete(fileFullPath, connectionSql);
                }
                reader.Close();
            }
        }

        query = "DELETE from balans_photos where bal_id = @balans_id";
        using (SqlCommand cmd = new SqlCommand(query, connectionSql))
        {
            cmd.Parameters.AddWithValue("balans_id", balansId);
            cmd.ExecuteNonQuery();
        }

        query = @"SELECT [id],[file_name],[file_ext] FROM [reports1nf_photos] WHERE [bal_id] = @balans_id";
        using (SqlCommand cmd = new SqlCommand(query, connectionSql))
        {
            cmd.Parameters.AddWithValue("balans_id", balansId);
            using (SqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    int id = (int)r.GetValue(0);
                    string file_name = (string)r.GetValue(1);
                    string file_ext = (string)r.GetValue(2);

                    string sourceFile = Path.Combine(photo1NFPath, id.ToString() + file_ext);
                    string destFile = Path.Combine(photoBalansPath, id.ToString() + file_ext);
                    LLLLhotorowUtils.Delete(destFile, connectionSql);
                    LLLLhotorowUtils.Copy(sourceFile, destFile, connectionSql);
                }
                r.Close();
            }
        }

        query = @"INSERT INTO [balans_photos] ([id],[bal_id],[file_name],[file_ext],[user_id],[create_date])
            SELECT [id],[bal_id],[file_name],[file_ext],[user_id],[create_date] FROM [reports1nf_photos] WHERE [bal_id] = @balans_id";
        using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql))
        {
            cmdInsert.Parameters.AddWithValue("balans_id", balansId);
            cmdInsert.ExecuteNonQuery();
        }


    }

    public static void DeleteBalansPhotoFileItem(int elementToDelete, int balId, string fileExt, string scope, string tableName)
    {
        string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
        string serverLocalBalansObjectFolder = Path.Combine(photoRootPath, "Balans", balId.ToString());
        string serverLocal1NFObjectFolder = Path.Combine(photoRootPath, "1NF", balId.ToString());

        string fileToDelete = Path.Combine(photoRootPath, scope, balId.ToString(), elementToDelete.ToString() + fileExt);

        string thumbFolderToDelete = Path.Combine(photoRootPath, "Thumb", balId.ToString());

        SqlConnection connection = Utils.ConnectToDatabase();
        SqlTransaction trans = connection.BeginTransaction();
        try
        {
            SqlCommand cmd = new SqlCommand("UPDATE " + tableName + " SET status = 2 WHERE id = @elementToDelete AND bal_id = @balId", connection, trans);
            cmd.Parameters.Add(new SqlParameter("elementToDelete", elementToDelete));
            cmd.Parameters.Add(new SqlParameter("balId", balId));
            cmd.ExecuteNonQuery();

            trans.Commit();
            connection.Close();
        }
        catch
        {
            trans.Rollback();
            connection.Close();
        }
    }

    public static void DeleteBalansPhotoFile(int elementToDelete, int balId, string fileExt, string scope)
    {
        string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
        string fileToDelete = Path.Combine(photoRootPath, scope, balId.ToString(), elementToDelete.ToString() + fileExt);
        var conn = Utils.ConnectToDatabase();
        LLLLhotorowUtils.Delete(fileToDelete, conn);
    }

    public static void DeleteFolderRecursive(string folderToDelete)
    {
        var conn = Utils.ConnectToDatabase();
        LLLLhotorowUtils.DeleteAll(folderToDelete, conn);
    }

    private static int FindObjectMatchForBalansBuilding(SqlConnection connectionSql, /*FbConnection connection1NF,*/
        Dictionary<string, object> buildingProperties, ref string number1, ref string number2,
        ref string number3, ref string addrMisc, bool copyBuildingProperties, string user)
    {
        // Get the street ID and building numbers
        int streetId = -1;
        object value = null;

        if (buildingProperties.TryGetValue("addr_street_id", out value) && value is int)
            streetId = (int)value;

        if (buildingProperties.TryGetValue("addr_nomer1", out value) && value is string)
            number1 = ((string)value).Trim().ToUpper();

        if (buildingProperties.TryGetValue("addr_nomer2", out value) && value is string)
            number2 = ((string)value).Trim().ToUpper();

        if (buildingProperties.TryGetValue("addr_nomer3", out value) && value is string)
            number3 = ((string)value).Trim().ToUpper();

        if (buildingProperties.TryGetValue("addr_misc", out value) && value is string)
            addrMisc = ((string)value).Trim().ToUpper();

//pgv визначаємо код будівлі
        int existingObjectId = -1;
        if (buildingProperties.TryGetValue("id", out value) && value is int)
            existingObjectId = (int)value;

        // Try to find existing object with the same address

//pgv    якщо не визначено код будівлі, то шукаємо його по коду вулиці і номеру будинку
//        int existingObjectId = ObjectFinder.Instance.FindObjectByStreetId(connectionSql, streetId, number1, number2, number3, addrMisc);

        if (existingObjectId == -1)
            existingObjectId = ObjectFinder.Instance.FindObjectByStreetId(connectionSql, streetId, number1, number2, number3, addrMisc);

        // We need to create a new object in both 1NF and Sql Server
        if (existingObjectId <= 0)
            existingObjectId = CreateObjectIn1NF(connectionSql, /*connection1NF,*/ buildingProperties);

        if (existingObjectId > 0)
        {
            if (copyBuildingProperties)
            {
                // Update this existing object with the new data (from 'reports1nf_buildings' table)

                // Get the field names from the 'buildings' table
                HashSet<string> buildingFieldNames = new HashSet<string>();

                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM buildings", connectionSql))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            buildingFieldNames.Add(reader.GetName(i).ToLower());
                        }

                        reader.Close();
                    }
                }

                string fieldList = "";
                SqlCommand cmdUpdateBuilding = new SqlCommand("", connectionSql);

                foreach (KeyValuePair<string, object> pair in buildingProperties)
                {
                    if (pair.Key != "id" && buildingFieldNames.Contains(pair.Key) && pair.Key != "addr_nomer" && pair.Key != "addr_address") // Skip COMPUTED columns
                    {
                        if (pair.Value is System.DBNull)
                        {
                            fieldList += ", " + pair.Key + " = NULL";
                        }
                        else
                        {
                            string paramName = "p_" + pair.Key;

                            cmdUpdateBuilding.Parameters.Add(new SqlParameter(paramName, pair.Value));

                            fieldList += ", " + pair.Key + " = @" + paramName;
                        }
                    }
                }

                if (fieldList.Length > 0)
                {
                    ArchiverSql.CreateObjectArchiveRecord(connectionSql, existingObjectId, user, null);

                    // Perform UPDATE
                    cmdUpdateBuilding.CommandText = "UPDATE buildings SET " + fieldList.TrimStart(' ', ',') + " WHERE id = @bid";

                    cmdUpdateBuilding.Parameters.Add(new SqlParameter("bid", existingObjectId));

                    cmdUpdateBuilding.ExecuteNonQuery();
                }
            }
        }

        return existingObjectId;
    }

    private static int FindObjectMatch(SqlConnection connectionSql, /*FbConnection connection1NF,*/
        int uniqueBuildingId, int reportId, bool copyBuildingProperties, string user)
    {
        Dictionary<string, object> buildingProperties = new Dictionary<string, object>();

        GetUniqueBuildingProperties(connectionSql, reportId, uniqueBuildingId, buildingProperties);

        string number1 = "";
        string number2 = "";
        string number3 = "";
        string addrMisc = "";

        return FindObjectMatchForBalansBuilding(connectionSql, /*connection1NF,*/ buildingProperties,
            ref number1, ref number2, ref number3, ref addrMisc, copyBuildingProperties, user);
    }

    public static void SendRentedObject(SqlConnection connection, /*FbConnection connection1NF,*/ int reportId, int rentedObjectId)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "Auto-import" : user.UserName);

        // Check the object status
        int idInPrimaryDb = 0;
        bool objectDeleted = false;
        int uniqueBuildingId = -1;

        using (SqlCommand cmd = new SqlCommand("SELECT arenda_rented_id, is_deleted, building_1nf_unique_id FROM reports1nf_arenda_rented WHERE id = @aid AND report_id = @rid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("aid", rentedObjectId));
            cmd.Parameters.Add(new SqlParameter("rid", reportId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    idInPrimaryDb = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    objectDeleted = reader.IsDBNull(1) ? false : (reader.GetInt32(1) == 1);
                    uniqueBuildingId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                }

                reader.Close();
            }
        }

        if (objectDeleted)
        {
            // Deleted rented object description; delete in the primary database as well
            if (idInPrimaryDb > 0)
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE arenda_rented SET is_deleted = 1, del_date = @mdt, modified_by = @usr, modify_date = @mdt WHERE id = @aid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("aid", idInPrimaryDb));
                    cmd.Parameters.Add(new SqlParameter("mdt", DateTime.Now));
                    cmd.Parameters.Add(new SqlParameter("usr", username.Left(64)));

                    cmd.ExecuteNonQuery();
                }

                // After this operation the rent agreement will no longer show in the UI; see the SQL query in OrgRentedList.aspx
            }
            else
            {
                // This rented object was never submitted to DKV; delete it
                using (SqlCommand cmd = new SqlCommand("DELETE FROM reports1nf_arenda_rented WHERE id = @aid AND report_id = @rid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("aid", rentedObjectId));
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));

                    cmd.ExecuteNonQuery();
                }
            }
        }
        else
        {
            // Get the ID of this rent agreement in the primary database
            if (idInPrimaryDb <= 0)
            {
                // New rented object description; create an entry in the 'arenda_rented' table
                SqlParameter outputIdParam = new SqlParameter("ARENDA_RENTED_ID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                using (SqlCommand cmd = new SqlCommand("dbo.fnCreateRentedObject", connection))
                {
                    cmd.Parameters.Add(outputIdParam);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                if (outputIdParam.Value is int)
                {
                    idInPrimaryDb = (int)outputIdParam.Value;
                }
            }

            if (idInPrimaryDb > 0)
            {
                // Perform address matching
                if (uniqueBuildingId > 0)
                {
                    int newBuildingId = FindObjectMatch(connection, /*connection1NF,*/ uniqueBuildingId, reportId, false, username);

//pgv                    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_rented SET building_id = @bid WHERE id = @aid AND report_id = @rid", connection))
                    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_rented SET building_id = @bid, arenda_rented_id = @arid WHERE id = @aid AND report_id = @rid", connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("aid", rentedObjectId));
                        cmd.Parameters.Add(new SqlParameter("rid", reportId));
                        cmd.Parameters.Add(new SqlParameter("bid", newBuildingId));
//pgv
                        cmd.Parameters.Add(new SqlParameter("arid", idInPrimaryDb));

                        cmd.ExecuteNonQuery();
                    }
                }

                // Get the field names from the 'arenda_rented' table
                HashSet<string> rentedFieldNames = new HashSet<string>();

                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM arenda_rented", connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            rentedFieldNames.Add(reader.GetName(i).ToLower());
                        }

                        reader.Close();
                    }
                }

                string fieldList = "";
                SqlCommand cmdUpdateRentedObj = new SqlCommand("", connection);

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM reports1nf_arenda_rented WHERE id = @aid AND report_id = @rid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("aid", rentedObjectId));
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Prepare the UPDATE query from the data reader
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i).ToLower();

                                if (rentedFieldNames.Contains(columnName) && columnName != "id" && columnName != "arenda_rented_id")
                                {
                                    if (reader.IsDBNull(i))
                                    {
                                        fieldList += ", " + columnName + " = NULL";
                                    }
                                    else
                                    {
                                        cmdUpdateRentedObj.Parameters.Add(new SqlParameter("param" + i.ToString(), reader.GetValue(i)));

                                        fieldList += ", " + columnName + " = @param" + i.ToString();
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }
                }

                if (fieldList.Length > 0 && idInPrimaryDb > 0)
                {
                    // Perform UPDATE
                    cmdUpdateRentedObj.CommandText = "UPDATE arenda_rented SET " + fieldList.TrimStart(' ', ',') + " WHERE id = @aid";

                    cmdUpdateRentedObj.Parameters.Add(new SqlParameter("aid", idInPrimaryDb));

                    cmdUpdateRentedObj.ExecuteNonQuery();

                    // Generate an archive state for the rented object
                    using (SqlCommand cmd = new SqlCommand("dbo.fnPushRentedObjectToArchive", connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("ARENDA_RENTED_ID", idInPrimaryDb));
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                }

                // Copy the rent payment information to a separate table, under the current reporting period
                SendRentedObjectPayments(connection, idInPrimaryDb);
            }
        }

        // Modify the submit date for the rented object
        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_rented SET submit_date = @sdt WHERE report_id = @rid AND id = @aid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("sdt", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.Parameters.Add(new SqlParameter("aid", rentedObjectId));
            cmd.ExecuteNonQuery();
        }

        // Mark the report as 'not viewed'
        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf SET is_reviewed = 0 WHERE id = @rid", connection))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.ExecuteNonQuery();
        }
    }

    public static void SendRentAgreement(SqlConnection connectionSql, /*FbConnection connection1NF,*/
        int reportId, ref int agreementId)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        // Check the agreement status
        bool agreementDeleted = false;
        int uniqueBuildingId = -1;

        using (SqlCommand cmd = new SqlCommand("SELECT is_deleted, building_1nf_unique_id FROM reports1nf_arenda WHERE id = @aid AND report_id = @rid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("aid", agreementId));
            cmd.Parameters.Add(new SqlParameter("rid", reportId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    agreementDeleted = reader.IsDBNull(0) ? false : (reader.GetInt32(0) == 1);
                    uniqueBuildingId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                }

                reader.Close();
            }
        }

        if (agreementDeleted)
        {
            // Deleted agreement; delete in the primary database as well
            if (agreementId > 0)
            {
                DeleteRentAgreementInAllDatabases(connectionSql, /*connection1NF,*/ agreementId);

                // After this operation the rent agreement will no longer show in the UI; see the SQL query in OrgArendaList.aspx
            }
            else
            {
                // This rent agreement was never submitted to DKV; delete it. No need to update the 1NF database
                using (SqlCommand cmd = new SqlCommand("DELETE FROM reports1nf_arenda WHERE id = @aid AND report_id = @rid", connectionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("aid", agreementId));
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));

                    cmd.ExecuteNonQuery();
                }
            }
        }
        else
        {
            // Perform address matching
            //if (uniqueBuildingId > 0)
            //{
            //    int newBuildingId = FindObjectMatch(connectionSql, /*connection1NF,*/ uniqueBuildingId, reportId, false, username);

            //    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda SET building_id = @bid WHERE id = @aid AND report_id = @rid", connectionSql))
            //    {
            //        cmd.Parameters.Add(new SqlParameter("aid", agreementId));
            //        cmd.Parameters.Add(new SqlParameter("rid", reportId));
            //        cmd.Parameters.Add(new SqlParameter("bid", newBuildingId));

            //        cmd.ExecuteNonQuery();
            //    }
            //}

            // Check if we need to create a new agreement
            bool created = false;

            if (agreementId <= 0)
            {
                int newAgreementId = CreateRentAgreementIn1NF(connectionSql, /*connection1NF,*/ reportId, agreementId);

                if (newAgreementId > 0)
                {
                    created = true;

                    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda SET id = @newid WHERE id = @oldid AND report_id = @rid", connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("oldid", agreementId));
                        cmd.Parameters.Add(new SqlParameter("newid", newAgreementId));
                        cmd.Parameters.Add(new SqlParameter("rid", reportId));

                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_decisions SET arenda_id = @newid WHERE arenda_id = @oldid AND report_id = @rid", connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("oldid", agreementId));
                        cmd.Parameters.Add(new SqlParameter("newid", newAgreementId));
                        cmd.Parameters.Add(new SqlParameter("rid", reportId));

                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_notes SET arenda_id = @newid WHERE arenda_id = @oldid AND report_id = @rid", connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("oldid", agreementId));
                        cmd.Parameters.Add(new SqlParameter("newid", newAgreementId));
                        cmd.Parameters.Add(new SqlParameter("rid", reportId));

                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_payments SET arenda_id = @newid WHERE arenda_id = @oldid AND report_id = @rid", connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("oldid", agreementId));
                        cmd.Parameters.Add(new SqlParameter("newid", newAgreementId));
                        cmd.Parameters.Add(new SqlParameter("rid", reportId));

                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_payment_documents SET arenda_id = @newid WHERE arenda_id = @oldid AND report_id = @rid", connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("oldid", agreementId));
                        cmd.Parameters.Add(new SqlParameter("newid", newAgreementId));
                        cmd.Parameters.Add(new SqlParameter("rid", reportId));

                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_comments SET arenda_id = @newid WHERE arenda_id = @oldid AND report_id = @rid", connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("oldid", agreementId));
                        cmd.Parameters.Add(new SqlParameter("newid", newAgreementId));
                        cmd.Parameters.Add(new SqlParameter("rid", reportId));

                        cmd.ExecuteNonQuery();
                    }

                    agreementId = newAgreementId;
                }
                else
                {
                    // ERROR: Invalid state of the database
                }
            }

            if (agreementId > 0 && !created)
            {
                // Prepare field mapping for the ARENDA1NF table
                Dictionary<string, string> mapping = new Dictionary<string, string>();
                GUKV.ImportToolUtils.FieldMappings.Create1NFArendaFieldMapping(mapping, false, true);

                // Remove the fields that we are going to set manually
                mapping.Remove("ARCH_KOD");

                mapping = GUKV.ImportToolUtils.FieldMappings.ReverseMapping(mapping);

                // Update the rent agreement information
                //string fieldList1NF = "";
                string fieldListSql = "";
                //FbCommand cmdUpdate1NF = new FbCommand("", connection1NF);
                SqlCommand cmdUpdateSql = new SqlCommand("", connectionSql);

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM reports1nf_arenda WHERE id = @aid AND report_id = @rid", connectionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("aid", agreementId));
                    cmd.Parameters.Add(new SqlParameter("rid", reportId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Prepare two UPDATE queries from the data reader
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnNameSql = reader.GetName(i).ToLower();
                                string columnName1NF = "";

                                if (columnNameSql != "id" && mapping.TryGetValue(columnNameSql, out columnName1NF))
                                {
                                    if (reader.IsDBNull(i))
                                    {
                                        fieldListSql += ", " + columnNameSql + " = NULL";

                                        // The NOACTIVE and SUBAR fields can not be NULL in 1NF
                                        //if (columnName1NF == "NOACTIVE" || columnName1NF == "SUBAR")
                                        //{
                                        //    fieldList1NF += ", " + columnName1NF + " = 0";
                                        //}
                                        //else
                                        //{
                                        //    fieldList1NF += ", " + columnName1NF + " = NULL";
                                        //}
                                    }
                                    else
                                    {
                                        string paramName = "param" + i.ToString();
                                        object value = reader.GetValue(i);

                                        cmdUpdateSql.Parameters.Add(new SqlParameter(paramName, value));
                                        fieldListSql += ", " + columnNameSql + " = @" + paramName;

                                        //if (CanMapArendaValueTo1NF(connection1NF, dictPool, columnName1NF, ref value))
                                        //{
                                        //    if (columnName1NF == "EIS_MODIFIED_BY")
                                        //    {
                                        //        cmdUpdate1NF.Parameters.Add(new FbParameter(paramName, value.ToString().Left(64)));

                                        //        // The ISP field in 1NF must have the same value as EIS_MODIFIED_BY field
                                        //        cmdUpdate1NF.Parameters.Add(new FbParameter("isp", ((string)value).Left(18)));
                                        //        fieldList1NF += ", ISP = @isp";
                                        //    }
                                        //    else
                                        //    {
                                        //        cmdUpdate1NF.Parameters.Add(new FbParameter(paramName, value));
                                        //    }

                                        //    fieldList1NF += ", " + columnName1NF + " = @" + paramName;
                                        //}
                                        //else
                                        //{
                                        //    fieldList1NF += ", " + columnName1NF + " = NULL";
                                        //}
                                    }
                                }
                            }
                        }

                        reader.Close();
                    }
                }

                // Perform UPDATE in 1NF
                //if (fieldList1NF.Length > 0)
                //{
                //    using (FbTransaction transaction = connection1NF.BeginTransaction())
                //    {
                //        try
                //        {
                //            // Create the ARENDA1NF archive state before updating it
                //            GUKV.DataMigration.Archiver1NF archiver = new GUKV.DataMigration.Archiver1NF();
                //            int archiveId = archiver.CreateArendaArchiveRecord(connection1NF, agreementId, transaction);

                //            if (archiveId > 0)
                //            {
                //                fieldList1NF += ", ARCH_KOD = @parch";
                //                cmdUpdate1NF.Parameters.Add(new FbParameter("parch", archiveId));
                //            }

                //            cmdUpdate1NF.CommandText = "UPDATE ARENDA1NF SET " + fieldList1NF.TrimStart(' ', ',') + " WHERE ID = @aid";
                //            log.Info("agreementId = " + agreementId + " ===== " + cmdUpdate1NF.CommandText);
                //            cmdUpdate1NF.Parameters.Add(new FbParameter("aid", agreementId));

                //            var parInfo = "";
                //            foreach (FbParameter par in cmdUpdate1NF.Parameters)
                //            {
                //                parInfo = parInfo + " ||| " + par.ParameterName + " = " + par.Value;
                //            }

                //            log.Debug(parInfo);
                //            cmdUpdate1NF.Transaction = transaction;
                //            cmdUpdate1NF.ExecuteNonQuery();

                //            // Commit the transaction
                //            transaction.Commit();
                //        }
                //        catch (Exception ex)
                //        {
                //            // Roll back the transaction
                //            transaction.Rollback();
                //            log.Error("agreementId = " + agreementId + " ===== " + ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
                //        }
                //    }
                //}


                int new_archive_link_code = ArchiverSql.CreateArendaArchiveRecord(connectionSql, agreementId, username, null);

                // Perform UPDATE in the Sql Server
                if (fieldListSql.Length > 0)
                {
                    cmdUpdateSql.CommandText = "UPDATE arenda SET " + fieldListSql.TrimStart(' ', ',') + " WHERE id = @aid";
                    cmdUpdateSql.Parameters.Add(new SqlParameter("aid", agreementId));
                    cmdUpdateSql.ExecuteNonQuery();
                }

                // Copy the rent decisions
                SendRentAgreementDecisions(connectionSql, /*connection1NF,*/ agreementId);

                // Copy the agreement notes
                SendRentAgreementNotes(connectionSql, /*connection1NF,*/ agreementId, reportId);

                // Copy the rent payment information
                SendRentPayments(connectionSql, agreementId, reportId, new_archive_link_code);

				SendRentAgreementPhotos(connectionSql, reportId, agreementId);
			}
        }

        // Modify the submit date for the rent agreement
        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda SET submit_date = @sdt WHERE report_id = @rid AND id = @aid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("sdt", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.Parameters.Add(new SqlParameter("aid", agreementId));
            cmd.ExecuteNonQuery();
        }

        // Mark the report as 'not viewed'
        using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf SET is_reviewed = 0 WHERE id = @rid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.ExecuteNonQuery();
        }
    }

    private static void SendRentAgreementDecisions(SqlConnection connectionSql, /*FbConnection connection1NF,*/ int agreementId)
    {
        // Get information about existing decisions from the Sql Server
        Dictionary<int, RentDecisionInfo> existingDecisionsSql = new Dictionary<int, RentDecisionInfo>();

        using (SqlCommand cmd = new SqlCommand("SELECT id, doc_num, doc_date FROM link_arenda_2_decisions WHERE arenda_id = @aid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("aid", agreementId));

            using (SqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    RentDecisionInfo info = new RentDecisionInfo();

                    info.id = r.GetInt32(0);
                    info.docNum = r.IsDBNull(1) ? "" : r.GetString(1).Trim().ToUpper();

                    if (!r.IsDBNull(2))
                        info.docDate = r.GetDateTime(2).Date;

                    existingDecisionsSql[info.id] = info;
                }

                r.Close();
            }
        }

        // Get information about existing decisions from 1NF
        //Dictionary<int, RentDecisionInfo> existingDecisions1NF = new Dictionary<int, RentDecisionInfo>();

        //using (FbCommand cmd = new FbCommand("SELECT ID, NOM, DT_DOC FROM ARRISH WHERE ARENDA = @aid", connection1NF))
        //{
        //    cmd.Parameters.Add(new FbParameter("aid", agreementId));

        //    using (FbDataReader r = cmd.ExecuteReader())
        //    {
        //        while (r.Read())
        //        {
        //            RentDecisionInfo info = new RentDecisionInfo();

        //            info.id = r.GetInt32(0);
        //            info.docNum = r.IsDBNull(1) ? "" : r.GetString(1).Trim().ToUpper();

        //            if (!r.IsDBNull(2))
        //                info.docDate = r.GetDateTime(2).Date;

        //            existingDecisions1NF[info.id] = info;
        //        }

        //        r.Close();
        //    }
        //}

        // Get all the current decisions (defined in the user report)
        Dictionary<int, RentDecisionInfo> newDecisions = new Dictionary<int, RentDecisionInfo>();

        using (SqlCommand cmd = new SqlCommand("SELECT id, modified_by, modify_date, doc_num, doc_date, purpose_str, " +
            "rent_square, pidstava FROM reports1nf_arenda_decisions WHERE arenda_id = @aid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("aid", agreementId));

            using (SqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    RentDecisionInfo info = new RentDecisionInfo();

                    info.id = r.GetInt32(0);
                    info.modifiedBy = r.IsDBNull(1) ? "" : r.GetString(1).Trim().Left(18);

                    if (!r.IsDBNull(2))
                        info.modifyDate = r.GetDateTime(2);

                    info.docNum = r.IsDBNull(3) ? "" : r.GetString(3).Trim().ToUpper();

                    if (!r.IsDBNull(4))
                        info.docDate = r.GetDateTime(4).Date;

                    info.purposeStr = r.IsDBNull(5) ? "" : r.GetString(5).Trim();
                    info.rentSquare = r.IsDBNull(6) ? 0m : r.GetDecimal(6);
                    info.pidstavaStr = r.IsDBNull(7) ? "" : r.GetString(7).Trim();

                    newDecisions[info.id] = info;
                }

                r.Close();
            }
        }

        // Execute INSERT or UPDATE for all new decisions in the Sql Server and in 1NF
        foreach (KeyValuePair<int, RentDecisionInfo> newDecision in newDecisions)
        {
            RentDecisionInfo existingDecisionSql = null;
            //RentDecisionInfo existingDecision1NF = null;

            // Sql Server update
            foreach (KeyValuePair<int, RentDecisionInfo> oldDecision in existingDecisionsSql)
            {
                if (oldDecision.Value.docNum.Length > 0 && oldDecision.Value.docDate.HasValue)
                {
                    if (newDecision.Value.docNum == oldDecision.Value.docNum &&
                        newDecision.Value.docDate == oldDecision.Value.docDate)
                    {
                        existingDecisionSql = oldDecision.Value;
                        break;
                    }
                }
            }

            if (existingDecisionSql != null)
            {
                SqlCommand cmd = newDecision.Value.CreateUpdateCmdSql(connectionSql, existingDecisionSql.id);
                cmd.ExecuteNonQuery();

                // Mark the existing decision as processed
                existingDecisionSql.isUpdated = true;
            }
            else
            {
                SqlCommand cmd = newDecision.Value.CreateInsertCmdSql(connectionSql, agreementId);
                cmd.ExecuteNonQuery();
            }

            // 1NF update
            //foreach (KeyValuePair<int, RentDecisionInfo> oldDecision in existingDecisions1NF)
            //{
            //    if (oldDecision.Value.docNum.Length > 0 && oldDecision.Value.docDate.HasValue)
            //    {
            //        if (newDecision.Value.docNum == oldDecision.Value.docNum &&
            //            newDecision.Value.docDate == oldDecision.Value.docDate)
            //        {
            //            existingDecision1NF = oldDecision.Value;
            //            break;
            //        }
            //    }
            //}

            //if (existingDecision1NF != null)
            //{
            //    FbCommand cmd = newDecision.Value.CreateUpdateCmd1NF(connection1NF, existingDecision1NF.id);
            //    cmd.ExecuteNonQuery();

            //    // Mark the existing decision as processed
            //    existingDecision1NF.isUpdated = true;
            //}
            //else
            //{
            //    using (FbTransaction transaction = connection1NF.BeginTransaction())
            //    {
            //        try
            //        {
            //            // Generate ID for a new ARRISH entry
            //            int linkId = GenerateId1NFUsingGenerator("GEN_ARRISH_ID", connection1NF, transaction);

            //            if (linkId > 0)
            //            {
            //                using (FbCommand cmd = newDecision.Value.CreateInsertCmd1NF(connection1NF, agreementId, linkId))
            //                {
            //                    cmd.Transaction = transaction;
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }

            //            // Commit the transaction
            //            transaction.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            // Roll back the transaction
            //            transaction.Rollback();
            //            log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
            //        }
            //    }
            //}
        }

        // Delete all the old decisions that could not be updated
        foreach (KeyValuePair<int, RentDecisionInfo> pair in existingDecisionsSql)
        {
            if (!pair.Value.isUpdated)
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM link_arenda_2_decisions WHERE id = @did", connectionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("did", pair.Key));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //foreach (KeyValuePair<int, RentDecisionInfo> pair in existingDecisions1NF)
        //{
        //    if (!pair.Value.isUpdated)
        //    {
        //        using (FbCommand cmd = new FbCommand("DELETE FROM ARRISH WHERE ID = @did", connection1NF))
        //        {
        //            cmd.Parameters.Add(new FbParameter("did", pair.Key));
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
    }

    private static void SendRentAgreementNotes(SqlConnection connectionSql, /*FbConnection connection1NF,*/ int agreementId, int reportId)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        // Delete all notes related to this agreement in the primary database
//pgv        using (SqlCommand cmd = new SqlCommand("UPDATE arenda_notes SET is_deleted = 1, del_date = @ddt, modified_by = @isp, modify_date = @ddt WHERE arenda_id = @aid", connectionSql))
        using (SqlCommand cmd = new SqlCommand("UPDATE arenda_notes SET is_deleted = 1, del_date = @ddt, modified_by = @isp, modify_date = @ddt WHERE arenda_id = @aid and is_deleted = 0", connectionSql))

        {
            cmd.Parameters.Add(new SqlParameter("aid", agreementId));
            cmd.Parameters.Add(new SqlParameter("ddt", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("isp", username.Left(64)));
            cmd.ExecuteNonQuery();
        }

        // Dump all notes from the report to the primary database
        string query = @"INSERT INTO arenda_notes
            SELECT arenda_id, purpose_group_id, purpose_id, purpose_str, rent_square, modify_date, modified_by, note,
            rent_rate, rent_rate_uah, cost_narah, cost_agreement, COALESCE(is_deleted, 0), del_date, cost_expert_total,
            date_expert, payment_type_id, invent_no, note_status_id, zapezh_deposit, ref_balans_id FROM reports1nf_arenda_notes WHERE arenda_id = @aid AND report_id = @rid";

        using (SqlCommand cmd = new SqlCommand(query, connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("aid", agreementId));
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.ExecuteNonQuery();
        }

        // Select IDs of all notes for this rent ageement (from Sql Server)
        List<int> noteIdsSqlServer = new List<int>();

        using (SqlCommand cmd = new SqlCommand("SELECT id FROM arenda_notes WHERE (arenda_id = @aid) AND (is_deleted = 0 OR is_deleted IS NULL) ORDER BY id", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("aid", agreementId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        noteIdsSqlServer.Add(reader.GetInt32(0));
                }

                reader.Close();
            }
        }

        // Select IDs of all notes for this rent ageement (from 1NF)
        //List<int> noteIds1NF = new List<int>();

        //try
        //{
        //    using (FbCommand cmd = new FbCommand("SELECT ID FROM ARENDA_PRIM WHERE (ARENDA = @aid) AND (DELETED = 0 OR DELETED IS NULL) ORDER BY ID", connection1NF))
        //    {
        //        cmd.Parameters.Add(new FbParameter("aid", agreementId));

        //        // Dump the command
        //        log.Debug(cmd.CommandText);
        //        var parInfo = "";

        //        foreach (FbParameter par in cmd.Parameters)
        //        {
        //            parInfo = parInfo + " ||||| " + par.ParameterName + " = " + par.Value;
        //        }

        //        log.Debug(parInfo);


        //        using (FbDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                if (!reader.IsDBNull(0))
        //                    noteIds1NF.Add(reader.GetInt32(0));
        //            }

        //            reader.Close();
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
        //    return;
        //}

        // Delete all leftover notes in 1NF
        //while (noteIds1NF.Count > noteIdsSqlServer.Count)
        //{
        //    try
        //    {
        //        using (FbCommand cmd = new FbCommand("UPDATE ARENDA_PRIM SET DELETED = 1, DTDELETE = @ddt, ISP = @isp, DT = @ddt WHERE ID = @aid", connection1NF))
        //        {
        //            cmd.Parameters.Add(new FbParameter("aid", noteIds1NF[noteIds1NF.Count - 1]));
        //            cmd.Parameters.Add(new FbParameter("ddt", DateTime.Now));
        //            cmd.Parameters.Add(new FbParameter("isp", username.Left(18)));

        //            // Dump the command
        //            log.Debug(cmd.CommandText);
        //            var parInfo = "";

        //            foreach (FbParameter par in cmd.Parameters)
        //            {
        //                parInfo = parInfo + " ||||| " + par.ParameterName + " = " + par.Value;
        //            }

        //            log.Debug(parInfo);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
        //        break;
        //    }

        //    noteIds1NF.RemoveAt(noteIds1NF.Count - 1);
        //}

        // Add some new notes to 1NF, if there is not enough
        //while (noteIds1NF.Count < noteIdsSqlServer.Count)
        //{
        //    using (FbTransaction transaction = connection1NF.BeginTransaction())
        //    {
        //        int newArendaPrimId = GenerateId1NFUsingGenerator("GEN_ARENDA_PRIM_ID", connection1NF, transaction);

        //        if (newArendaPrimId > 0)
        //        {
        //            try
        //            {
        //                using (FbCommand cmd = new FbCommand("INSERT INTO ARENDA_PRIM (ID, ARENDA, DELETED, DT, ISP) VALUES (@noteid, @aid, 0, @dt, @isp)", connection1NF))
        //                {
        //                    cmd.Parameters.Add(new FbParameter("noteid", newArendaPrimId));
        //                    cmd.Parameters.Add(new FbParameter("aid", agreementId));
        //                    cmd.Parameters.Add(new FbParameter("dt", DateTime.Now));
        //                    cmd.Parameters.Add(new FbParameter("isp", username.Left(18)));

        //                    // Dump the command
        //                    log.Debug(cmd.CommandText);
        //                    var parInfo = "";

        //                    foreach (FbParameter par in cmd.Parameters)
        //                    {
        //                        parInfo = parInfo + " ||||| " + par.ParameterName + " = " + par.Value;
        //                    }

        //                    log.Debug(parInfo);


        //                    cmd.Transaction = transaction;
        //                    cmd.ExecuteNonQuery();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);

        //                transaction.Rollback();
        //                break;
        //            }

        //            transaction.Commit();

        //            noteIds1NF.Add(newArendaPrimId);
        //        }
        //        else
        //        {
        //            transaction.Rollback();
        //            break;
        //        }
        //    }
        //}

        //if (noteIds1NF.Count == noteIdsSqlServer.Count)
        //{
        //    Dictionary<string, string> mapping = new Dictionary<string, string>();
        //    GUKV.DataMigration.FieldMappings.Create1NFArendaNotesFieldMapping(mapping, true);

        //    mapping = GUKV.DataMigration.FieldMappings.ReverseMapping(mapping);

        //    // Perform UPDATE for each 1NF note from the corresponding Sql Server note
        //    for (int n = 0; n < noteIds1NF.Count; n++)
        //    {
        //        int noteIdSql = noteIdsSqlServer[n];
        //        int noteId1NF = noteIds1NF[n];

        //        using (SqlCommand cmd = new SqlCommand("SELECT * FROM arenda_notes WHERE id = @noteid", connectionSql))
        //        {
        //            cmd.Parameters.Add(new SqlParameter("noteid", noteIdSql));

        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    using (FbTransaction transaction = connection1NF.BeginTransaction())
        //                    {
        //                        try
        //                        {
        //                            string fieldList = "";
        //                            FbCommand cmdUpdate1NF = new FbCommand("", connection1NF);

        //                            for (int i = 0; i < reader.FieldCount; i++)
        //                            {
        //                                string fieldNameSql = reader.GetName(i).ToLower();
        //                                string fieldName1NF = "";

        //                                if (fieldNameSql != "id" && mapping.TryGetValue(fieldNameSql, out fieldName1NF))
        //                                {
        //                                    if (reader.IsDBNull(i))
        //                                    {
        //                                        fieldList += ", " + fieldName1NF + " = NULL";
        //                                    }
        //                                    else
        //                                    {
        //                                        string paramName = "param" + i.ToString();
        //                                        fieldList += ", " + fieldName1NF + " = @" + paramName;

        //                                        if (fieldName1NF == "ISP")
        //                                        {
        //                                            cmdUpdate1NF.Parameters.Add(new FbParameter(paramName, reader.GetValue(i).ToString().Left(18)));
        //                                        }
        //                                        else
        //                                        {
        //                                            cmdUpdate1NF.Parameters.Add(new FbParameter(paramName, reader.GetValue(i)));
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                            if (fieldList.Length > 0)
        //                            {
        //                                cmdUpdate1NF.CommandText = "UPDATE ARENDA_PRIM SET " + fieldList.TrimStart(' ', ',') + " WHERE ID = @noteid";
        //                                cmdUpdate1NF.Parameters.Add(new FbParameter("noteid", noteId1NF));

        //                                log.Debug(cmdUpdate1NF.CommandText);
        //                                var parInfo = "";

        //                                foreach (FbParameter par in cmdUpdate1NF.Parameters)
        //                                {
        //                                    parInfo = parInfo + " ||||| " + par.ParameterName + " = " + par.Value;
        //                                }

        //                                log.Debug(parInfo);

        //                                cmdUpdate1NF.Transaction = transaction;
        //                                cmdUpdate1NF.ExecuteNonQuery();
        //                            }

        //                            // Commit the transaction
        //                            transaction.Commit();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            // Roll back the transaction
        //                            transaction.Rollback();
        //                            log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
        //                        }
        //                    }
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
        //}
    }

    private static void SendRentPayments(SqlConnection connectionSql, int agreementId, int reportId, int new_archive_link_code)
    {
        // Update the period ID in the payment table (always use the active reporting period)
        
        // NOTE
        // ----
        // As end users are being allowed to submit reporting for arbitrary periods (selectable via the UI),
        // it is no longer desirable to overwrite user-specified information here.

        //using (SqlCommand cmd = new SqlCommand("UPDATE reports1nf_arenda_payments SET rent_period_id = (SELECT MAX(id) FROM dict_rent_period WHERE is_active = 1)" +
        //    " WHERE report_id = @rid AND arenda_id = @aid", connectionSql))
        //{
        //    cmd.Parameters.Add(new SqlParameter("rid", reportId));
        //    cmd.Parameters.Add(new SqlParameter("aid", agreementId));
        //    cmd.ExecuteNonQuery();
        //}

        // Get the properties of rent payment related to the rent agreement
        Dictionary<string, object> paymentProperties = new Dictionary<string, object>();
        int paymentIdInReport = -1;
        int periodId = -1;

        using (SqlCommand cmd = new SqlCommand("SELECT * FROM reports1nf_arenda_payments WHERE report_id = @rid AND arenda_id = @aid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.Parameters.Add(new SqlParameter("aid", agreementId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string fieldName = reader.GetName(i).ToLower();

                        if (fieldName == "id")
                        {
                            paymentIdInReport = reader.GetInt32(i);
                        }
                        else if (fieldName == "rent_period_id")
                        {
                            periodId = reader.IsDBNull(i) ? 0 : reader.GetInt32(i);
                        }

                        if (fieldName != "report_id" && fieldName != "id")
                        {
                            if (reader.IsDBNull(i))
                                paymentProperties.Add(fieldName, System.DBNull.Value);
                            else
                                paymentProperties.Add(fieldName, reader.GetValue(i));
                        }
                    }
                }

                reader.Close();
            }
        }

        if (paymentIdInReport > 0 && periodId > 0)
        {
            // Get the ID of rent payment (related to the same reporting period) in the primary database
            int paymentIdPrimary = -1;

            using (SqlCommand cmd = new SqlCommand("SELECT id FROM arenda_payments WHERE arenda_id = @aid AND rent_period_id = @rpid", connectionSql))
            {
                cmd.Parameters.Add(new SqlParameter("rpid", periodId));
                cmd.Parameters.Add(new SqlParameter("aid", agreementId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            paymentIdPrimary = reader.GetInt32(0);
                    }

                    reader.Close();
                }
            }

            // Prepare the UPDATE or INSERT statement
            string fieldList = "";
            string paramList = "";
            SqlCommand cmdInsertOrUpdate = new SqlCommand("", connectionSql);

            foreach (KeyValuePair<string, object> pair in paymentProperties)
            {
                string paramName = "p_" + pair.Key;

                if (paymentIdPrimary > 0)
                {
                    // Use UPDATE syntax
                    if (pair.Value is System.DBNull)
                    {
                        fieldList += ", " + pair.Key + " = NULL";
                    }
                    else
                    {
                        cmdInsertOrUpdate.Parameters.Add(new SqlParameter(paramName, pair.Value));
                        fieldList += ", " + pair.Key + " = @" + paramName;
                    }
                }
                else
                {
                    // Use INSERT syntax
                    fieldList += ", " + pair.Key;

                    if (pair.Value is System.DBNull)
                    {
                        paramList += ", NULL";
                    }
                    else
                    {
                        cmdInsertOrUpdate.Parameters.Add(new SqlParameter(paramName, pair.Value));
                        paramList += ", @" + paramName;
                    }
                }
            }

            if (paymentIdPrimary > 0)
            {
                cmdInsertOrUpdate.CommandText = "UPDATE arenda_payments SET " + fieldList.TrimStart(' ', ',') + ", arenda_archive_link_code = " + new_archive_link_code.ToString() + " WHERE id = @payid";
                cmdInsertOrUpdate.Parameters.Add(new SqlParameter("payid", paymentIdPrimary));
            }
            else
            {
                cmdInsertOrUpdate.CommandText = "INSERT INTO arenda_payments (" + fieldList.TrimStart(' ', ',') + ", arenda_archive_link_code) VALUES (" + paramList.TrimStart(' ', ',') + ", " + new_archive_link_code.ToString() + ")";
            }

            cmdInsertOrUpdate.ExecuteNonQuery();
        }
    }

    private static void SendRentedObjectPayments(SqlConnection connectionSql, int arendaRentedId)
    {
        // Get ID of the current reporting period
        int periodId = -1;

        using (SqlCommand cmd = new SqlCommand("SELECT MAX(id) FROM dict_rent_period WHERE is_active = 1", connectionSql))
        {
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                        periodId = reader.GetInt32(0);
                }

                reader.Close();
            }
        }

        if (periodId > 0)
        {
            // Check if this rented object has CMK properties
            bool isCmk = false;

            using (SqlCommand cmd = new SqlCommand("SELECT is_cmk FROM arenda_rented WHERE id = @aid", connectionSql))
            {
                cmd.Parameters.Add(new SqlParameter("aid", arendaRentedId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            isCmk = reader.GetInt32(0) > 0;
                    }

                    reader.Close();
                }
            }

            if (isCmk)
            {
                // Get the ID of rent payment (related to the same reporting period) in the primary database
                int paymentIdPrimary = -1;

                using (SqlCommand cmd = new SqlCommand("SELECT id FROM arenda_payments_cmk WHERE arenda_rented_id = @aid AND rent_period_id = @rpid", connectionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("rpid", periodId));
                    cmd.Parameters.Add(new SqlParameter("aid", arendaRentedId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                                paymentIdPrimary = reader.GetInt32(0);
                        }

                        reader.Close();
                    }
                }

                if (paymentIdPrimary > 0)
                {
                    // UPDATE an existing record in the 'arenda_payments_cmk' table
                    string query = @"UPDATE acmk SET
	                    acmk.cmk_sqr_rented = ar.cmk_sqr_rented,
	                    acmk.cmk_payment_narah = ar.cmk_payment_narah,
	                    acmk.cmk_payment_to_budget = ar.cmk_payment_to_budget,
	                    acmk.cmk_rent_debt = ar.cmk_rent_debt,
	                    acmk.modify_date = ar.modify_date,
	                    acmk.modified_by = ar.modified_by
                    FROM arenda_payments_cmk acmk INNER JOIN arenda_rented ar ON ar.id = @aid WHERE acmk.id = @cmkid";

                    using (SqlCommand cmd = new SqlCommand(query, connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("aid", arendaRentedId));
                        cmd.Parameters.Add(new SqlParameter("cmkid", paymentIdPrimary));
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // INSERT a new record into 'arenda_payments_cmk' table
                    string query = @"INSERT INTO arenda_payments_cmk (
	                    arenda_rented_id,
	                    rent_period_id,
	                    cmk_sqr_rented,
	                    cmk_payment_narah,
	                    cmk_payment_to_budget,
	                    cmk_rent_debt,
	                    modify_date,
	                    modified_by)
                    SELECT
	                    @aid AS 'arenda_rented_id',
	                    @rpid AS 'rent_period_id',
	                    ar.cmk_sqr_rented,
	                    ar.cmk_payment_narah,
	                    ar.cmk_payment_to_budget,
	                    ar.cmk_rent_debt,
	                    ar.modify_date,
	                    ar.modified_by
                    FROM arenda_rented ar WHERE ar.id = @aid";

                    using (SqlCommand cmd = new SqlCommand(query, connectionSql))
                    {
                        cmd.Parameters.Add(new SqlParameter("aid", arendaRentedId));
                        cmd.Parameters.Add(new SqlParameter("rpid", periodId));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                // Delete all CMK reporting for this rent agreement and reporting period
                using (SqlCommand cmd = new SqlCommand("DELETE FROM arenda_payments_cmk WHERE arenda_rented_id = @aid AND rent_period_id = @rpid", connectionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("aid", arendaRentedId));
                    cmd.Parameters.Add(new SqlParameter("rpid", periodId));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    private static void DeleteRentAgreementInAllDatabases(SqlConnection connectionSql, /*FbConnection connection1NF,*/ int agreementId)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        using (SqlCommand cmd = new SqlCommand("UPDATE arenda SET is_deleted = 1, del_date = @mdt, modified_by = @usr, modify_date = @mdt WHERE id = @aid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("aid", agreementId));
            cmd.Parameters.Add(new SqlParameter("mdt", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("usr", username.Left(64)));

            cmd.ExecuteNonQuery();
        }

        //if (connection1NF != null)
        //{
        //    using (FbCommand cmd = new FbCommand("UPDATE ARENDA1NF SET DELETED = 1, DTDELETE = @mdt, ISP = @isp, EIS_MODIFIED_BY = @usr, DT = @mdt WHERE ID = @aid", connection1NF))
        //    {
        //        log.Info(cmd.CommandText);
        //        cmd.Parameters.Add(new FbParameter("aid", agreementId));
        //        cmd.Parameters.Add(new FbParameter("mdt", DateTime.Now));
        //        cmd.Parameters.Add(new FbParameter("isp", username.Left(18)));
        //        cmd.Parameters.Add(new FbParameter("usr", username.Left(64)));

        //        cmd.ExecuteNonQuery();
        //    }
        //}
    }

	private static void SendRentAgreementPhotos(SqlConnection connectionSql, int reportId, int agreementId)
	{
		string photoRootPath = WebConfigurationManager.AppSettings["ImgContentRootFolder"];
		string photo1NFARENDAPath = Path.Combine(photoRootPath, "1NFARENDA", agreementId.ToString());
		string photoArendaPath = Path.Combine(photoRootPath, "Arenda", agreementId.ToString());

		string query = "SELECT id,file_name,file_ext from arenda_photos where arenda_id = @arenda_id";
		using (SqlCommand cmd = new SqlCommand(query, connectionSql))
		{
			cmd.Parameters.AddWithValue("arenda_id", agreementId);
			using (SqlDataReader reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					int id = (int)reader.GetValue(0);
					string file_name = (string)reader.GetValue(1);
					string file_ext = (string)reader.GetValue(2);
					string fileFullPath = Path.Combine(photoArendaPath, id.ToString() + file_ext);
                    LLLLhotorowUtils.Delete(fileFullPath, connectionSql);
				}
				reader.Close();
			}
		}

		query = "DELETE from arenda_photos where arenda_id = @arenda_id";
		using (SqlCommand cmd = new SqlCommand(query, connectionSql))
		{
			cmd.Parameters.AddWithValue("arenda_id", agreementId);
			cmd.ExecuteNonQuery();
		}

		query = @"SELECT [id],[file_name],[file_ext] FROM [reports1nf_arendaphotos] WHERE [arenda_id] = @arenda_id";
		using (SqlCommand cmd = new SqlCommand(query, connectionSql))
		{
			cmd.Parameters.AddWithValue("arenda_id", agreementId);
			using (SqlDataReader r = cmd.ExecuteReader())
			{
				while (r.Read())
				{
					int id = (int)r.GetValue(0);
					string file_name = (string)r.GetValue(1);
					string file_ext = (string)r.GetValue(2);

					string sourceFile = Path.Combine(photo1NFARENDAPath, id.ToString() + file_ext);
					string destFile = Path.Combine(photoArendaPath, id.ToString() + file_ext);
                    LLLLhotorowUtils.Delete(destFile, connectionSql);
                    LLLLhotorowUtils.Copy(sourceFile, destFile, connectionSql);
				}
				r.Close();
			}
		}

		query = @"INSERT INTO [arenda_photos] ([id],[arenda_id],[file_name],[file_ext],[user_id],[create_date])
            SELECT [id],[arenda_id],[file_name],[file_ext],[user_id],[create_date] FROM [reports1nf_arendaphotos] WHERE [arenda_id] = @arenda_id";
		using (SqlCommand cmdInsert = new SqlCommand(query, connectionSql))
		{
			cmdInsert.Parameters.AddWithValue("arenda_id", agreementId);
			cmdInsert.ExecuteNonQuery();
		}
	}

	#endregion (Updating the EIS database)

	#region Updating the 1NF database

	/// <summary>
	/// Generates a new ID in 1NF database using the specified GENERATOR
	/// </summary>
	/// <param name="generatorName">Generator name</param>
	/// <param name="transaction">Transaction (may be NULL)</param>
	/// <returns>Generated ID, or -1 in case of any error</returns>
	private static int GenerateId1NFUsingGenerator(string generatorName, FbConnection connection1NF, FbTransaction transaction)
    {
        int objectId = -1;

        string query = "SELECT GEN_ID(" + generatorName + ", 1) FROM RDB$DATABASE";

        using (FbCommand command = new FbCommand(query, connection1NF))
        {
            command.Transaction = transaction;

            using (FbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        objectId = reader.GetInt32(0);
                    }
                }

                reader.Close();
            }
        }

        return objectId;
    }

    private static Dictionary<string, int> Get1NFOrgFieldLimits()
    {
        Dictionary<string, int> limits = new Dictionary<string, int>();

        limits.Add("KOD_ZKPO", 14);
        limits.Add("NAME_UL", 100);
        limits.Add("NOMER_DOMA", 30);
        limits.Add("NOMER_KORPUS", 20);
        limits.Add("POST_INDEX", 18);
        limits.Add("FIO_BOSS", 70);
        limits.Add("FIO_BUH", 70);
        limits.Add("USER_KOREG", 20);
        limits.Add("TEL_BOSS", 23);
        limits.Add("TEL_BUH", 23);
        limits.Add("FULL_NAME_OBJ", 252);
        limits.Add("SHORT_NAME_OBJ", 100);
        limits.Add("FIND_NAME_OBJ", 100);
        limits.Add("NAME_DAVAL_VIDM", 150);
        limits.Add("ADDITIONAL_ADRESS", 150);
        limits.Add("TELEFAX", 23);
        limits.Add("REESORG", 40);
        limits.Add("REESNO", 15);
        limits.Add("NOMER_DOMA2", 18);
        limits.Add("KOD_KVED", 7);
        limits.Add("NO_SVIDOT", 10);
        limits.Add("KOATUU", 15);
        limits.Add("ISP", 18);
        limits.Add("PERSENT_AKCIA", 50);
        limits.Add("NAME_BANK", 10);
        limits.Add("MFO", 200);
        limits.Add("ACCOUNT", 18);
        limits.Add("SYSTEM_ISP", 18);
        limits.Add("FIO_BOSS_R_VIDM", 70);
        limits.Add("POST_BOSS", 70);
        limits.Add("POST_BOSS_R_VIDM", 70);
        limits.Add("DOC_BOSS", 100);
        limits.Add("DOC_BOSS_R_VIDM", 100);
        limits.Add("EIS_DIRECTOR_EMAIL", 100);
        limits.Add("EIS_PHYS_ADDR_NOMER", 60);
        limits.Add("EIS_PHYS_ADDR_ZIP_CODE", 24);
        limits.Add("EIS_PHYS_ADDR_MISC", 150);
        limits.Add("EIS_MODIFIED_BY", 64);
        limits.Add("EIS_BUH_EMAIL", 100);
        limits.Add("EIS_UNKNOWN_PAYMENT_NOTE", 510);

        return limits;
    }

    //private static void SendOrganizationInfoTo1NF(SqlConnection connection, Dictionary<string, HashSet<int>> dictPool, int reportId, int organizationId)
    //{
    //    FbConnection connection1NF = ConnectTo1NF();

    //    if (connection1NF != null)
    //    {
    //        Dictionary<string, int> fieldLimits = Get1NFOrgFieldLimits();

    //        // Use the same field mapping that is used in Data Migration tool
    //        Dictionary<string, string> mapping = new Dictionary<string, string>();
    //        GUKV.DataMigration.FieldMappings.Create1NFOrgFieldMapping(mapping, true);

    //        mapping = GUKV.DataMigration.FieldMappings.ReverseMapping(mapping);

    //        // Remove the fields that we are going to set manually
    //        mapping.Remove("arch_id");

    //        // Prepare an UPDATE command for 1NF
    //        FbCommand cmdUpdate = new FbCommand("", connection1NF);
    //        string fieldList = "";

    //        using (SqlCommand cmd = new SqlCommand("SELECT * FROM organizations WHERE id = @oid", connection))
    //        {
    //            cmd.Parameters.Add(new SqlParameter("oid", organizationId));

    //            using (SqlDataReader reader = cmd.ExecuteReader())
    //            {
    //                if (reader.Read())
    //                {
    //                    // Prepare the UPDATE query from the data reader
    //                    for (int i = 0; i < reader.FieldCount; i++)
    //                    {
    //                        string columnName = reader.GetName(i).ToLower();
    //                        string fieldName1NF = "";

    //                        if (columnName != "id" && mapping.TryGetValue(columnName, out fieldName1NF))
    //                        {
    //                            fieldName1NF = fieldName1NF.ToUpper();

    //                            if (reader.IsDBNull(i))
    //                            {
    //                                fieldList += ", " + fieldName1NF + " = NULL";
    //                            }
    //                            else
    //                            {
    //                                object value = reader.GetValue(i);

    //                                if (CanMapOrganizationValueTo1NF(connection1NF, dictPool, fieldName1NF, value))
    //                                {
    //                                    // If this is a string field, check its length limit
    //                                    int limit = 0;

    //                                    if (value is string && fieldLimits.TryGetValue(fieldName1NF, out limit))
    //                                    {
    //                                        value = ((string)value).Left(limit);
    //                                    }

    //                                    cmdUpdate.Parameters.Add(new FbParameter("param" + i.ToString(), value));

    //                                    fieldList += ", " + fieldName1NF + " = @param" + i.ToString();

    //                                    // The ISP field in 1NF must have the same value as EIS_MODIFIED_BY field
    //                                    if (fieldName1NF == "EIS_MODIFIED_BY" && value is string)
    //                                    {
    //                                        cmdUpdate.Parameters.Add(new FbParameter("isp", ((string)value).Left(18)));

    //                                        fieldList += ", ISP = @isp";
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }

    //                reader.Close();
    //            }
    //        }

    //        if (fieldList.Length > 0)
    //        {
    //            using (FbTransaction transaction = connection1NF.BeginTransaction())
    //            {
    //                try
    //                {
    //                    // Create the organization archive state before updating it
    //                    GUKV.DataMigration.Archiver1NF archiver = new GUKV.DataMigration.Archiver1NF();
    //                    int archiveId = archiver.CreateOrganizationArchiveRecord(connection1NF, organizationId, transaction);

    //                    if (archiveId > 0)
    //                    {
    //                        fieldList += ", ARCH_KOD = @parch";
    //                        cmdUpdate.Parameters.Add(new FbParameter("parch", archiveId));
    //                    }

    //                    cmdUpdate.CommandText = "UPDATE SORG_1NF SET " + fieldList.TrimStart(' ', ',') + " WHERE KOD_OBJ = @orgid";
    //                    cmdUpdate.Parameters.Add(new FbParameter("orgid", organizationId));

    //                    var parInfo = "";
    //                    foreach (FbParameter par in cmdUpdate.Parameters)
    //                    {
    //                        parInfo = parInfo + " ||| " + par.ParameterName + " = " + par.Value;
    //                    }
    //                    log.Debug(cmdUpdate.CommandText);
    //                    log.Debug(parInfo);

    //                    cmdUpdate.Transaction = transaction;
    //                    cmdUpdate.ExecuteNonQuery();

    //                    // Commit the transaction
    //                    transaction.Commit();
    //                }
    //                catch (Exception ex)
    //                {
    //                    // Roll back the transaction
    //                    transaction.Rollback();
    //                    log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
    //                }
    //            }
    //        }

    //        connection1NF.Close();
    //    }
    //}

    private static bool CanMapOrganizationValueTo1NF(FbConnection connection, Dictionary<string, HashSet<int>> dictPool, string fieldName, object value)
    {
        if (value is int)
        {
            fieldName = fieldName.Trim().ToUpper();

            if (fieldName == "KOD_VID_DIAL")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_VID_DIAL", "KOD_VID_DIAL", (int)value);

            if (fieldName == "KOD_STATUS")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_STATUS", "KOD_STATUS", (int)value);

            if (fieldName == "KOD_FORM_GOSP")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_FORM_GOSP", "KOD_FORM_GOSP", (int)value);

            if (fieldName == "KOD_FORM_VLASN")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_FORM_VLASN", "KOD_FORM_VLASN", (int)value);

            if (fieldName == "KOD_GOSP_STRUKT")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_GOSP_STRUKT", "KOD_GOSP_STRUKT", (int)value);

            if (fieldName == "KOD_ORGAN")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_ORGAN", "KOD_ORGAN", (int)value);

            if (fieldName == "KOD_GALUZ")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_GALUZ", "KOD_GALUZ", (int)value);

            if (fieldName == "KOD_ADDITION_PRIZNAK")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_ADDITION_PRIZNAK", "KOD_ADDITION_PRIZNAK", (int)value);

            if (fieldName == "KOD_PRIKMET")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_PRIKMET", "KOD_PRIKMET", (int)value);

            if (fieldName == "KOD_PRIZNAK_1")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_PRIZNAK_1", "KOD_PRIZNAK_1", (int)value);

            if (fieldName == "KOD_VIDOM_NAL")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_VIDOM_NAL", "KOD_VIDOM_NAL", (int)value);

            if (fieldName == "KOD_IMEN")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_IMEN", "KOD_IMEN", (int)value);

            if (fieldName == "KOD_ORG_FORM")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_ORG_FORM", "KOD_ORG_FORM", (int)value);

            if (fieldName == "KOD_VID_GOSP_STR")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_VID_GOSP_STR", "KOD_VID_GOSP_STR", (int)value);

            if (fieldName == "KOD_VID_AKCIA")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_AKCIA", "KOD_AKCIA", (int)value);

            if (fieldName == "VIDDIL")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_VIDDIL", "KOD", (int)value);

            if (fieldName == "KOD_RAYON")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_RAYON", "KOD_RAYON", (int)value);

            if (fieldName == "KOD_RAYON2")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_RAYON2", "KOD_RAYON2", (int)value);

            if (fieldName == "EIS_ADDR_STREET_ID")
                return CanMapDictionaryValue1NF(connection, dictPool, "SUL", "KOD", (int)value);

            if (fieldName == "EIS_PHYS_ADDR_STREET_ID")
                return CanMapDictionaryValue1NF(connection, dictPool, "SUL", "KOD", (int)value);

            if (fieldName == "EIS_PHYS_ADDR_DISTRICT_ID")
                return CanMapDictionaryValue1NF(connection, dictPool, "S_RAYON2", "KOD_RAYON2", (int)value);
        }

        return true;
    }

    //private static bool CanMapBalansValueTo1NF(FbConnection connection, Dictionary<string, HashSet<int>> dictPool, string fieldName, ref object value)
    //{
    //    if (value is int)
    //    {
    //        fieldName = fieldName.Trim().ToUpper();

    //        if (fieldName == "FORM_VLASN")
    //        {
    //            int mappedValue = MapOrgOwnershipTo1NF((int)value);

    //            if (mappedValue > 0)
    //            {
    //                value = mappedValue;
    //                return true;
    //            }

    //            return false;
    //        }

    //        if (fieldName == "KINDOBJ")
    //            return CanMapDictionaryValue1NF(connection, dictPool, "SKINDOBJ", "KOD", (int)value);

    //        if (fieldName == "TYPEOBJ")
    //            return CanMapDictionaryValue1NF(connection, dictPool, "STYPEOBJ", "KOD", (int)value);

    //        if (fieldName == "HISTORY")
    //            return CanMapDictionaryValue1NF(connection, dictPool, "SHISTORY", "KOD", (int)value);

    //        if (fieldName == "TEXSTAN")
    //            return CanMapDictionaryValue1NF(connection, dictPool, "STEXSTAN", "KOD", (int)value);

    //        if (fieldName == "GRPURP")
    //            return CanMapDictionaryValue1NF(connection, dictPool, "SGRPURPOSE", "KOD", (int)value);

    //        if (fieldName == "PURPOSE")
    //            return CanMapDictionaryValue1NF(connection, dictPool, "SPURPOSE", "KOD", (int)value);

    //        if (fieldName == "OBJ_KODUL")
    //            return CanMapDictionaryValue1NF(connection, dictPool, "SUL", "KOD", (int)value);

    //        if (fieldName == "ULKOD2")
    //            return CanMapDictionaryValue1NF(connection, dictPool, "SUL", "KOD", (int)value);
    //    }

    //    return true;
    //}

    private static bool CanMapArendaValueTo1NF(FbConnection connection, Dictionary<string, HashSet<int>> dictPool, string fieldName, ref object value)
    {
        if (value is int)
        {
            fieldName = fieldName.Trim().ToUpper();

            if (fieldName == "OBJKIND")
                return CanMapDictionaryValue1NF(connection, dictPool, "SKINDOBJ", "KOD", (int)value);

            if (fieldName == "GRPURPOSE")
                return CanMapDictionaryValue1NF(connection, dictPool, "SGRPURPOSE", "KOD", (int)value);

            if (fieldName == "PURPOSE")
                return CanMapDictionaryValue1NF(connection, dictPool, "SPURPOSE", "KOD", (int)value);
        }

        return true;
    }

    //private static int MapOrgOwnershipTo1NF(int orgOwnership)
    //{
    //    switch (orgOwnership)
    //    {
    //        case 10:
    //        case 20:
    //        case 31:
    //        case 33:
    //        case 34:
    //        case 35:
    //        case 40:
    //        case 50:
    //        case 99:
    //            return orgOwnership;

    //        case 32:
    //            return 33;
    //    }

    //    return -1;
    //}

    private static int CreateObjectIn1NF(SqlConnection connectionSql, /*FbConnection connection1NF,*/ Dictionary<string, object> buildingProperties)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        // Pre-process the building numbers, because 1NF has some restrictions for them
        object val = null;

        if (buildingProperties.TryGetValue("addr_nomer1", out val) && val is string)
        {
            string number1 = (string)val;
            string number2 = "";

            if (buildingProperties.TryGetValue("addr_nomer2", out val) && val is string)
            {
                number2 = (string)val;
            }

            ObjectFinder.PreProcessBuildingNumbers(ref number1, ref number2);

            buildingProperties["addr_nomer1"] = number1;
            buildingProperties["addr_nomer2"] = number2;
        }

        // Remove the fields that we are going to set manually
        Dictionary<string, string> fields = new Dictionary<string, string>();
        GUKV.ImportToolUtils.FieldMappings.Create1NFObjectFieldMapping(fields, false, true);

        fields.Remove("OBJECT_KOD");
        fields.Remove("DT");
        fields.Remove("EIS_MODIFIED_BY");
        fields.Remove("STAN_YEAR");
        fields.Remove("DT_BEG");
        fields.Remove("DT_END");
        fields.Remove("DELETED");
        fields.Remove("DTDELETE");
        fields.Remove("ARCH_KOD");
        fields.Remove("IN_ARCH");
        fields.Remove("CHARACTERISTIC");
        fields.Remove("KORPUS");
        fields.Remove("REALSTAN");

        // Prepare the INSERT parameters for 1NF
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        int paramIndex = 0;
        string insertFieldList1NF = "";
        string insertFieldListSql = "";
        string insertParamList = "";

        foreach (KeyValuePair<string, string> mappingEntry in fields)
        {
            string fieldNameSqlServer = mappingEntry.Value.ToLower();
            object value = null;

            if (buildingProperties.TryGetValue(fieldNameSqlServer, out value))
            {
                if (insertFieldList1NF.Length > 0)
                {
                    insertFieldList1NF += ", ";
                    insertFieldListSql += ", ";
                    insertParamList += ", ";
                }

                insertFieldList1NF += mappingEntry.Key;
                insertFieldListSql += fieldNameSqlServer;

                if (value is System.DBNull)
                {
                    insertParamList += "NULL";
                }
                else
                {
                    paramIndex++;

                    string paramName = "p" + paramIndex.ToString();

                    insertParamList += "@" + paramName;

                    parameters[paramName] = value;
                }
            }
        }

        int newObjectId = -1;

        using (SqlCommand cmdNewObjectId = new SqlCommand("select MAX(ID) + 1 FROM buildings", connectionSql))
        {
            using (SqlDataReader r = cmdNewObjectId.ExecuteReader())
            {
                if (r.Read())
                    newObjectId = (int)r.GetValue(0);
                r.Close();
            }
        }

        //if (parameters.Count > 0)
        //{
        //    using (FbTransaction transaction = connection1NF.BeginTransaction())
        //    {
        //        try
        //        {
        //            // Generate ID for a new object
        //            newObjectId = GenerateId1NFUsingGenerator("OBJECT_1NF_GEN", connection1NF, transaction);

        //            if (newObjectId > 0)
        //            {
        //                // Format the INSERT statement
        //                string query = "INSERT INTO OBJECT_1NF (OBJECT_KOD, OBJECT_KODSTAN, ISP, EIS_MODIFIED_BY, DT, STAN_YEAR, REALSTAN, DT_BEG, DELETED, KORPUS, "
        //                    + insertFieldList1NF + ") VALUES (@oid, 1, @isp, @mby, @dt, @syear, 1, @dt, 0, 0, " + insertParamList + ")";

        //                using (FbCommand commandInsert = new FbCommand(query, connection1NF))
        //                {
        //                    commandInsert.Parameters.Add(new FbParameter("oid", newObjectId));
        //                    commandInsert.Parameters.Add(new FbParameter("isp", username.Left(18)));
        //                    commandInsert.Parameters.Add(new FbParameter("mby", username.Left(64)));
        //                    commandInsert.Parameters.Add(new FbParameter("dt", DateTime.Now));
        //                    commandInsert.Parameters.Add(new FbParameter("syear", DateTime.Now.Year));

        //                    foreach (KeyValuePair<string, object> pair in parameters)
        //                    {
        //                        commandInsert.Parameters.Add(new FbParameter(pair.Key, pair.Value));
        //                    }

        //                    commandInsert.Transaction = transaction;
        //                    commandInsert.ExecuteNonQuery();
        //                }
        //            }

        //            // Commit the transaction
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Roll back the transaction
        //            transaction.Rollback();
        //            log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
        //            newObjectId = -1;
        //        }
        //    }
        //}

        // Create the same object in Sql Server
        if (newObjectId > 0)
        {
            string query = "INSERT INTO buildings (id, modified_by, modify_date, condition_year, is_condition_valid, date_begin, is_deleted, addr_korpus_flag" + 
            (!string.IsNullOrEmpty(insertFieldListSql) ? "," +insertFieldListSql : "") + ") VALUES (@oid, @isp, @dt, @syear, 1, @dt, 0, 0" + 
            (!string.IsNullOrEmpty(insertParamList) ?  ", " + insertParamList  : "") + ")";

            using (SqlCommand cmd = new SqlCommand(query, connectionSql))
            {
                cmd.Parameters.Add(new SqlParameter("oid", newObjectId));
                cmd.Parameters.Add(new SqlParameter("isp", username.Left(64)));
                cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));
                cmd.Parameters.Add(new SqlParameter("syear", DateTime.Now.Year));

                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                cmd.ExecuteNonQuery();
            }
        }

        return newObjectId;
    }

    private static int CreateRentAgreementIn1NF(SqlConnection connectionSql, /*FbConnection connection1NF,*/
        int reportId, int agreementIdInReport)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);

        // Get the field mapping for ARENDA1NF table
        Dictionary<string, string> mapping = new Dictionary<string, string>();
        GUKV.ImportToolUtils.FieldMappings.Create1NFArendaFieldMapping(mapping, false, true);

        // Remove the fields that we are going to set manually
        mapping.Remove("ID");
        mapping.Remove("OBJECT_KODSTAN");
        mapping.Remove("BALANS_KODSTAN");
        mapping.Remove("ORG_KODSTAN");
        mapping.Remove("ORGIV_STAN");
        mapping.Remove("NOACTIVE");
        mapping.Remove("DELETED");
        mapping.Remove("EIS_MODIFIED_BY");
        mapping.Remove("DT");
        mapping.Remove("EDT");
        mapping.Remove("SYSTEM_DT");
        mapping.Remove("SYSTEM_ISP");
        mapping.Remove("UPDT_SOURCE");
        mapping.Remove("PRIZNAK_1NF");

        mapping = GUKV.ImportToolUtils.FieldMappings.ReverseMapping(mapping);

        string destFieldList1NF = "";
        string destFieldListSql = "";
        string paramList1NF = "";
        string paramListSql = "";
        //Dictionary<string, object> parameters1NF = new Dictionary<string, object>();
        Dictionary<string, object> parametersSql = new Dictionary<string, object>();

        // Get the rent agreement properties from Sql Server
        using (SqlCommand cmd = new SqlCommand("SELECT * FROM reports1nf_arenda WHERE report_id = @rid AND id = @aid", connectionSql))
        {
            cmd.Parameters.Add(new SqlParameter("rid", reportId));
            cmd.Parameters.Add(new SqlParameter("aid", agreementIdInReport));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string fieldNameSql = reader.GetName(i).ToLower();
                        string fieldName1NF = "";

                        if (mapping.TryGetValue(fieldNameSql, out fieldName1NF))
                        {
                            fieldName1NF = fieldName1NF.ToUpper();

                            if (reader.IsDBNull(i))
                            {
                                // Some fields are not allowed to be NULL in 1NF
                                if (fieldName1NF == "SUBAR")
                                {
                                    destFieldList1NF += ", " + fieldName1NF;
                                    paramList1NF += ", @sbr";
                                    //parameters1NF["sbr"] = 0;
                                }
                            }
                            else
                            {
                                string paramName = "param" + i.ToString();
                                object value = reader.GetValue(i);

                                destFieldListSql += ", " + fieldNameSql;
                                paramListSql += ", @" + paramName;
                                parametersSql[paramName] = value;

                                //if (CanMapArendaValueTo1NF(connection1NF, dictPool, fieldName1NF, ref value))
                                //{
                                //    destFieldList1NF += ", " + fieldName1NF;
                                //    paramList1NF += ", @" + paramName;
                                //    parameters1NF[paramName] = value;
                                //}
                            }
                        }
                    }
                }

                reader.Close();
            }
        }

        // Create the rent agreement in 1NF
        int newArendaId = -1;

        using (SqlCommand cmdNewArendaId = new SqlCommand("select MAX(ID)+1 FROM arenda", connectionSql))
        {
            using (SqlDataReader r = cmdNewArendaId.ExecuteReader())
            {
                if (r.Read())
                    newArendaId = (int)r.GetValue(0);
                r.Close();
            }
        }

        //string query = "INSERT INTO ARENDA1NF (ID, OBJECT_KODSTAN, BALANS_KODSTAN, ORG_KODSTAN, ORGIV_STAN, NOACTIVE, DELETED," +
        //    " ISP, EIS_MODIFIED_BY, DT, EDT, SYSTEM_DT, SYSTEM_ISP, UPDT_SOURCE, PRIZNAK_1NF, " + destFieldList1NF.TrimStart(',', ' ') +
        //    ") VALUES (@aid, 1, 1, 1, 1, 0, 0, @isp, @mby, @dt, @dt, @dt, @isp, 2, 1, " + paramList1NF.TrimStart(',', ' ') + ")";

        //if (parameters1NF.Count > 0)
        //{
        //    using (FbTransaction transaction = connection1NF.BeginTransaction())
        //    {
        //        try
        //        {
        //            // Generate ID for a new object
        //            newArendaId = GenerateId1NFUsingGenerator("GEN_ARENDA1NF", connection1NF, transaction);

        //            if (newArendaId > 0)
        //            {
        //                using (FbCommand cmd = new FbCommand(query, connection1NF))
        //                {
        //                    cmd.Parameters.Add(new FbParameter("isp", username.Left(18)));
        //                    cmd.Parameters.Add(new FbParameter("mby", username.Left(64)));
        //                    cmd.Parameters.Add(new FbParameter("dt", DateTime.Now));
        //                    cmd.Parameters.Add(new FbParameter("aid", newArendaId));

        //                    foreach (KeyValuePair<string, object> pair in parameters1NF)
        //                    {
        //                        cmd.Parameters.Add(new FbParameter(pair.Key, pair.Value));
        //                    }

        //                    log.Info(cmd.CommandText);
        //                    cmd.Transaction = transaction;
        //                    cmd.ExecuteNonQuery();
        //                }
        //            }

        //            // Commit the transaction
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Roll back the transaction
        //            transaction.Rollback();
        //            log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
        //            newArendaId = -1;
        //        }
        //    }
        //}

        // If everything went OK, create the same agreement in Sql Server
        if (newArendaId > 0 && parametersSql.Count > 0)
        {
            string query = "INSERT INTO arenda (id, is_inactive, is_deleted, modified_by, modify_date, update_src_id, priznak_1nf, " +
                destFieldListSql.TrimStart(',', ' ') + ") VALUES (@aid, 0, 0, @isp, @dt, 2, 1, " + paramListSql.TrimStart(',', ' ') + ")";

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, connectionSql))
                {
                    cmd.Parameters.Add(new SqlParameter("isp", username.Left(64)));
                    cmd.Parameters.Add(new SqlParameter("dt", DateTime.Now));
                    cmd.Parameters.Add(new SqlParameter("aid", newArendaId));

                    foreach (KeyValuePair<string, object> pair in parametersSql)
                    {
                        cmd.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }

                    log.Info(cmd.CommandText);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " ===== " + ex.Source + " ===== " + ex.TargetSite + " ==== " + ex.StackTrace);
            }
        }

        return newArendaId;
    }

    #endregion (Updating the 1NF database)

    #region Caching of the 1NF dictionaries

    public static bool CanMapDictionaryValue1NF(FbConnection connection1NF, Dictionary<string, HashSet<int>> dictPool, string tableName, string keyField, int value)
    {
        HashSet<int> dict = Get1NFDictionary(dictPool, tableName, keyField, connection1NF);

        if (dict != null)
        {
            return dict.Contains(value);
        }

        return false;
    }

    public static HashSet<int> Get1NFDictionary(Dictionary<string, HashSet<int>> dictPool, string tableName, string keyField, FbConnection connection1NF)
    {
        HashSet<int> dict = null;

        if (dictPool.TryGetValue(tableName, out dict))
        {
            return dict;
        }

        // Not found in the cache; read the dictionary from 1NF
        dict = Read1NFDictionary(tableName, keyField, connection1NF);

        if (dict != null)
        {
            dictPool.Add(tableName, dict);

            return dict;
        }

        return null;
    }

    private static HashSet<int> Read1NFDictionary(string tableName, string keyField, FbConnection connection1NF)
    {
        HashSet<int> keys = new HashSet<int>();

        using (FbCommand cmd = new FbCommand("SELECT " + keyField + " FROM " + tableName, connection1NF))
        {
            using (FbDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    if (!r.IsDBNull(0))
                    {
                        int key = r.GetInt32(0);

                        if (key > 0)
                        {
                            keys.Add(key);
                        }
                    }
                }

                r.Close();
            }
        }

        return keys;
    }

    #endregion (Caching of the 1NF dictionaries)

    #region Comments

    public static void AddComment(SqlConnection connection, int reportId, string comment,
        int organizationId, int balansId, int balansDeletedId, int rentAgreementId, int rentedObjectId,
        string controlId, string controlTitle, bool isWrongData, bool sendNotification)
    {
        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
        string username = (user == null ? "System" : user.UserName);
        string usernameForComment = username;

        // Check if user belongs to DKV
        bool userIsReportSubmitter = true;
        bool userIsRda = false;

        if (Roles.IsUserInRole(Utils.Report1NFReviewerRole))
        {
            usernameForComment += " (ДКВ)";
            userIsReportSubmitter = false;
        }
        else if (Roles.IsUserInRole(Utils.RDAControllerRole))
        {
            usernameForComment += " (РДА)";
            userIsReportSubmitter = false;
            userIsRda = true;
        }

        SqlCommand cmdInsert = new SqlCommand("", connection);
        string fieldList = "report_id, comment, comment_date, comment_user";
        string paramList = "@rep, @txt, @dt, @usr";

        cmdInsert.Parameters.Add(new SqlParameter("rep", reportId));
        cmdInsert.Parameters.Add(new SqlParameter("txt", comment));
        cmdInsert.Parameters.Add(new SqlParameter("dt", DateTime.Now));
        cmdInsert.Parameters.Add(new SqlParameter("usr", usernameForComment.Left(64)));

        if (controlId.Length > 0)
        {
            fieldList += ", control_id";
            paramList += ", @ctlid";

            cmdInsert.Parameters.Add(new SqlParameter("ctlid", controlId.Left(64)));
        }

        if (controlTitle.Length > 0)
        {
            fieldList += ", control_title";
            paramList += ", @ctltitle";

            cmdInsert.Parameters.Add(new SqlParameter("ctltitle", controlTitle.Left(256)));
        }

        if (organizationId != 0)
        {
            fieldList += ", organization_id";
            paramList += ", @org";

            cmdInsert.Parameters.Add(new SqlParameter("org", organizationId));
        }

        if (balansId != 0)
        {
            fieldList += ", balans_id";
            paramList += ", @bal";

            cmdInsert.Parameters.Add(new SqlParameter("bal", balansId));
        }

        if (balansDeletedId != 0)
        {
            fieldList += ", balans_deleted_id";
            paramList += ", @baldel";

            cmdInsert.Parameters.Add(new SqlParameter("baldel", balansDeletedId));
        }

        if (rentAgreementId != 0)
        {
            fieldList += ", arenda_id";
            paramList += ", @ar";

            cmdInsert.Parameters.Add(new SqlParameter("ar", rentAgreementId));
        }

        if (rentedObjectId != 0)
        {
            fieldList += ", arenda_rented_id";
            paramList += ", @robj";

            cmdInsert.Parameters.Add(new SqlParameter("robj", rentedObjectId));
        }

        if (isWrongData)
        {
            fieldList += ", is_wrong_data";
            paramList += ", @wrng";

            cmdInsert.Parameters.Add(new SqlParameter("wrng", 1));
        }

        cmdInsert.CommandText = @"INSERT INTO reports1nf_comments (" + fieldList + ") VALUES (" + paramList + ")";
        cmdInsert.ExecuteNonQuery();

        // Send a email notification about the comment
        if (sendNotification)
        {
            if (userIsReportSubmitter)
            {
                // - закомментировано по указанию Синенко от 11.07.2020 
                //Reports1NFUtils.SendNewOrgCommentNotification(connection, reportId, Utils.UserOrganizationID, comment, controlTitle, controlId, username,
                //    organizationId > 0, balansId, balansDeletedId, rentAgreementId, rentedObjectId);
            }
            else
            {
                if (userIsRda)
                {
                    Reports1NFUtils.SendNewRDACommentNotification(connection, reportId, comment, controlTitle, controlId, username,
                        organizationId > 0, balansId, balansDeletedId, rentAgreementId, rentedObjectId);
                }
                else
                {
                    Reports1NFUtils.SendNewDKVCommentNotification(connection, reportId, comment, controlTitle, controlId, username,
                        organizationId > 0, balansId, balansDeletedId, rentAgreementId, rentedObjectId);
                }
            }
        }
    }

    #endregion (Comments)

    #region Email notifications

    //public static void SendEmailToOrganization(SqlConnection connection, int organizationId, string subject, string body)
    //{
    //    // Select all emails
    //    string query = @"SELECT mem.Email FROM reports1nf_accounts acc INNER JOIN aspnet_Membership mem ON mem.UserId = acc.UserId WHERE acc.organization_id = @org";

    //    using (SqlCommand cmd = new SqlCommand(query, connection))
    //    {
    //        cmd.Parameters.Add(new SqlParameter("org", organizationId));

    //        using (SqlDataReader reader = cmd.ExecuteReader())
    //        {
    //            while (reader.Read())
    //            {
    //                if (!reader.IsDBNull(0))
    //                {
    //                    string email = reader.GetString(0).Trim();

    //                    if (email.Length > 0 && email.Contains('@'))
    //                    {
    //                        try
    //                        {
    //                            EmailNotifier.SendMailMessage(WebConfigurationManager.AppSettings["EmailFrom"], email, null, null, subject, body);
    //                        }
    //                        finally
    //                        {
    //                        }
    //                    }
    //                }
    //            }

    //            reader.Close();
    //        }
    //    }
    //}





    public static void SendNewOrgCommentNotification(SqlConnection connection, int reportId, int organizationFromId,
        string comment, string infoArea, string controlId, string userName,
        bool organizationInfo, int balansId, int balansDeletedId, int arendaId, int rentedObjectId)
    {
        string roleName = "";
        string subject = "";
        string body = "";

        string orgName = "";
        int organizationId = GetReportOrganizationId(connection, reportId, out orgName);

        if (organizationInfo)
        {
            if (controlId.Contains("_orndpymnt"))
            {
                roleName = Utils.DKVArendaPaymentsControllerRole;

                FormatOrgInfoNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectOrgOrgArendaPayment, Resources.Strings.Report1NFCommentBodyOrgOrgArendaPayment,
                    userName, orgName, "", infoArea, comment, Resources.Strings.GlobalWebSiteURL, reportId);
            }
            else
            {
                roleName = Utils.DKVOrganizationControllerRole;

                FormatOrgInfoNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectOrgOrg, Resources.Strings.Report1NFCommentBodyOrgOrg,
                    userName, orgName, "", infoArea, comment, Resources.Strings.GlobalWebSiteURL, reportId);
            }
        }
        else if (balansId > 0)
        {
            roleName = Utils.DKVObjectControllerRole;

            FormatBalansObjNotification(out subject, out body,
                Resources.Strings.Report1NFCommentSubjectOrgBalans, Resources.Strings.Report1NFCommentBodyOrgBalans,
                userName, orgName, GetBalansObjectDescription(connection, reportId, balansId), infoArea, comment,
                Resources.Strings.GlobalWebSiteURL, balansId, reportId);
        }
        else if (balansDeletedId > 0)
        {
            roleName = Utils.DKVObjectControllerRole;

            FormatDeletedObjNotification(out subject, out body,
                Resources.Strings.Report1NFCommentSubjectOrgBalansDel, Resources.Strings.Report1NFCommentBodyOrgBalansDel,
                userName, orgName, GetBalansDeletedObjectDescription(connection, reportId, balansDeletedId), infoArea, comment,
                Resources.Strings.GlobalWebSiteURL, balansDeletedId, reportId);
        }
        else if (arendaId > 0)
        {
            if (controlId.Contains("_orndpymnt"))
            {
                roleName = Utils.DKVArendaPaymentsControllerRole;

                FormatRentAgreementNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectOrgArendaPayment, Resources.Strings.Report1NFCommentBodyOrgArendaPayment,
                    userName, orgName, GetRentAgreementDescription(connection, reportId, arendaId), infoArea, comment,
                    Resources.Strings.GlobalWebSiteURL, arendaId, reportId);
            }
            else
            {
                roleName = Utils.DKVArendaControllerRole;

                FormatRentAgreementNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectOrgArenda, Resources.Strings.Report1NFCommentBodyOrgArenda,
                    userName, orgName, GetRentAgreementDescription(connection, reportId, arendaId), infoArea, comment,
                    Resources.Strings.GlobalWebSiteURL, arendaId, reportId);
            }
        }
        else if (rentedObjectId > 0)
        {
            roleName = Utils.DKVArendaControllerRole;

            FormatRentedObjectNotification(out subject, out body,
                Resources.Strings.Report1NFCommentSubjectOrgRented, Resources.Strings.Report1NFCommentBodyOrgRented,
                userName, orgName, GetRentedObjectDescription(connection, reportId, rentedObjectId), infoArea, comment,
                Resources.Strings.GlobalWebSiteURL, rentedObjectId, reportId);
        }

        EmailNotifier.SendEmailToRole(connection, null, roleName, WebConfigurationManager.AppSettings["EmailFrom"], subject, body, organizationFromId);
    }

    public static void SendNewDKVCommentNotification(SqlConnection connection, int reportId,
        string comment, string infoArea, string controlId, string userName,
        bool organizationInfo, int balansId, int balansDeletedId, int arendaId, int rentedObjectId)
    {
        string subject = "";
        string body = "";

        string orgName = "";
        int organizationId = GetReportOrganizationId(connection, reportId, out orgName);

        if (organizationInfo)
        {
            if (controlId.Contains("_orndpymnt"))
            {
                FormatOrgInfoNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectDkvOrgArendaPayment, Resources.Strings.Report1NFCommentBodyDkvOrgArendaPayment,
                    userName, orgName, "", infoArea, comment, Resources.Strings.GlobalWebSiteURL, reportId);
            }
            else
            {
                FormatOrgInfoNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectDkvOrg, Resources.Strings.Report1NFCommentBodyDkvOrg,
                    userName, orgName, "", infoArea, comment, Resources.Strings.GlobalWebSiteURL, reportId);
            }
        }
        else if (balansId > 0)
        {
            FormatBalansObjNotification(out subject, out body,
                Resources.Strings.Report1NFCommentSubjectDkvBalans, Resources.Strings.Report1NFCommentBodyDkvBalans,
                userName, orgName, GetBalansObjectDescription(connection, reportId, balansId), infoArea, comment,
                Resources.Strings.GlobalWebSiteURL, balansId, reportId);
        }
        else if (balansDeletedId > 0)
        {
            FormatDeletedObjNotification(out subject, out body,
                Resources.Strings.Report1NFCommentSubjectDkvBalansDel, Resources.Strings.Report1NFCommentBodyDkvBalansDel,
                userName, orgName, GetBalansDeletedObjectDescription(connection, reportId, balansDeletedId), infoArea, comment,
                Resources.Strings.GlobalWebSiteURL, balansDeletedId, reportId);
        }
        else if (arendaId > 0)
        {
            if (controlId.Contains("_orndpymnt"))
            {
                FormatRentAgreementNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectDkvArendaPayment, Resources.Strings.Report1NFCommentBodyDkvArendaPayment,
                    userName, orgName, GetRentAgreementDescription(connection, reportId, arendaId), infoArea, comment,
                    Resources.Strings.GlobalWebSiteURL, arendaId, reportId);
            }
            else
            {
                FormatRentAgreementNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectDkvArenda, Resources.Strings.Report1NFCommentBodyDkvArenda,
                    userName, orgName, GetRentAgreementDescription(connection, reportId, arendaId), infoArea, comment,
                    Resources.Strings.GlobalWebSiteURL, arendaId, reportId);
            }
        }
        else if (rentedObjectId > 0)
        {
            FormatRentedObjectNotification(out subject, out body,
                Resources.Strings.Report1NFCommentSubjectDkvRented, Resources.Strings.Report1NFCommentBodyDkvRented,
                userName, orgName, GetRentedObjectDescription(connection, reportId, rentedObjectId), infoArea, comment,
                Resources.Strings.GlobalWebSiteURL, rentedObjectId, reportId);
        }

        EmailNotifier.SendEmailToOrganization(connection, null, organizationId, WebConfigurationManager.AppSettings["EmailFrom"], subject, body);
    }

    public static void SendNewRDACommentNotification(SqlConnection connection, int reportId,
        string comment, string infoArea, string controlId, string userName,
        bool organizationInfo, int balansId, int balansDeletedId, int arendaId, int rentedObjectId)
    {
        string subject = "";
        string body = "";

        string orgName = "";
        int organizationId = GetReportOrganizationId(connection, reportId, out orgName);

        if (organizationInfo)
        {
            if (controlId.Contains("_orndpymnt"))
            {
                FormatOrgInfoNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectRdaOrgArendaPayment, Resources.Strings.Report1NFCommentBodyRdaOrgArendaPayment,
                    userName, orgName, "", infoArea, comment, Resources.Strings.GlobalWebSiteURL, reportId);
            }
            else
            {
                FormatOrgInfoNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectRdaOrg, Resources.Strings.Report1NFCommentBodyRdaOrg,
                    userName, orgName, "", infoArea, comment, Resources.Strings.GlobalWebSiteURL, reportId);
            }
        }
        else if (balansId > 0)
        {
            FormatBalansObjNotification(out subject, out body,
                Resources.Strings.Report1NFCommentSubjectRdaBalans, Resources.Strings.Report1NFCommentBodyRdaBalans,
                userName, orgName, GetBalansObjectDescription(connection, reportId, balansId), infoArea, comment,
                Resources.Strings.GlobalWebSiteURL, balansId, reportId);
        }
        else if (balansDeletedId > 0)
        {
            FormatDeletedObjNotification(out subject, out body,
                Resources.Strings.Report1NFCommentSubjectRdaBalansDel, Resources.Strings.Report1NFCommentBodyRdaBalansDel,
                userName, orgName, GetBalansDeletedObjectDescription(connection, reportId, balansDeletedId), infoArea, comment,
                Resources.Strings.GlobalWebSiteURL, balansDeletedId, reportId);
        }
        else if (arendaId > 0)
        {
            if (controlId.Contains("_orndpymnt"))
            {
                FormatRentAgreementNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectRdaArendaPayment, Resources.Strings.Report1NFCommentBodyRdaArendaPayment,
                    userName, orgName, GetRentAgreementDescription(connection, reportId, arendaId), infoArea, comment,
                    Resources.Strings.GlobalWebSiteURL, arendaId, reportId);
            }
            else
            {
                FormatRentAgreementNotification(out subject, out body,
                    Resources.Strings.Report1NFCommentSubjectRdaArenda, Resources.Strings.Report1NFCommentBodyRdaArenda,
                    userName, orgName, GetRentAgreementDescription(connection, reportId, arendaId), infoArea, comment,
                    Resources.Strings.GlobalWebSiteURL, arendaId, reportId);
            }
        }
        else if (rentedObjectId > 0)
        {
            FormatRentedObjectNotification(out subject, out body,
                Resources.Strings.Report1NFCommentSubjectRdaRented, Resources.Strings.Report1NFCommentBodyRdaRented,
                userName, orgName, GetRentedObjectDescription(connection, reportId, rentedObjectId), infoArea, comment,
                Resources.Strings.GlobalWebSiteURL, rentedObjectId, reportId);
        }

        EmailNotifier.SendEmailToOrganization(connection, null, organizationId, WebConfigurationManager.AppSettings["EmailFrom"], subject, body);
    }

    private static string GetBalansObjectDescription(SqlConnection connection, int reportId, int balansId)
    {
        string description = "";

        string query = @"SELECT b.street_full_name, b.addr_nomer, bal.sqr_total, COALESCE(bal.purpose_str, dict_balans_purpose.name) AS 'purpose'
            FROM reports1nf_balans bal
            INNER JOIN reports1nf_buildings b ON b.unique_id = bal.building_1nf_unique_id
            LEFT OUTER JOIN dict_balans_purpose ON dict_balans_purpose.id = bal.purpose_id
            WHERE bal.id = @bal_id AND bal.report_id = @rep_id";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("bal_id", balansId));
            cmd.Parameters.Add(new SqlParameter("rep_id", reportId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string street = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();
                    string number = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim();
                    decimal sqr = reader.IsDBNull(2) ? -1m : reader.GetDecimal(2);
                    string purpose = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim();

                    if (purpose.Length > 0)
                    {
                        description += "Приміщення: " + purpose;
                    }
                    else
                    {
                        description += "Приміщення";
                    }

                    if (sqr > 0m)
                    {
                        description += " площею " + sqr.ToString("F2") + " кв.м.";
                    }

                    if (street.Length > 0)
                        street += " ";

                    street += number;

                    description += " за адресою " + street;
                }

                reader.Close();
            }
        }

        return description;
    }

    private static string GetBalansDeletedObjectDescription(SqlConnection connection, int reportId, int balansDeletedId)
    {
        string description = "";

        string query = @"SELECT b.street_full_name, b.addr_nomer, bal.sqr_total, COALESCE(bal.purpose_str, dict_balans_purpose.name) AS 'purpose'
            FROM reports1nf_balans_deleted bal
            INNER JOIN buildings b ON b.id = bal.building_id
            LEFT OUTER JOIN dict_balans_purpose ON dict_balans_purpose.id = bal.purpose_id
            WHERE bal.id = @bal_id AND bal.report_id = @rep_id";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("bal_id", balansDeletedId));
            cmd.Parameters.Add(new SqlParameter("rep_id", reportId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string street = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();
                    string number = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim();
                    decimal sqr = reader.IsDBNull(2) ? -1m : reader.GetDecimal(2);
                    string purpose = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim();

                    if (purpose.Length > 0)
                    {
                        description += "Приміщення: " + purpose;
                    }
                    else
                    {
                        description += "Приміщення";
                    }

                    if (sqr > 0m)
                    {
                        description += " площею " + sqr.ToString("F2") + " кв.м.";
                    }

                    if (street.Length > 0)
                        street += " ";

                    street += number;

                    description += " за адресою " + street;
                }

                reader.Close();
            }
        }

        return description;
    }

    private static string GetRentAgreementDescription(SqlConnection connection, int reportId, int arendaId)
    {
        string description = "";

        string query = @"SELECT ar.agreement_num, ar.agreement_date, org_renter.full_name
            FROM reports1nf_arenda ar LEFT OUTER JOIN organizations org_renter ON org_renter.id = ar.org_renter_id and (org_renter.is_deleted is null or org_renter.is_deleted = 0)
            WHERE ar.id = @ar_id AND ar.report_id = @rep_id";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("ar_id", arendaId));
            cmd.Parameters.Add(new SqlParameter("rep_id", reportId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string agreementNum = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();
                    object agreementDate = reader.IsDBNull(1) ? null : (object)reader.GetDateTime(1);
                    string renterName = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim();

                    string dateStr = "(не вказано)";

                    if (agreementDate is DateTime)
                        dateStr = ((DateTime)agreementDate).ToShortDateString();

                    if (agreementNum.Length == 0)
                        agreementNum = "(не вказано)";

                    if (renterName.Length == 0)
                        renterName = "(не вказано)";

                    description = string.Format(Resources.Strings.Reports1NFRentFormat, agreementNum, dateStr, renterName);
                }

                reader.Close();
            }
        }

        return description;
    }

    private static string GetRentedObjectDescription(SqlConnection connection, int reportId, int arendaRentedId)
    {
        string description = "";

        string query = @"SELECT ar.agreement_num, ar.agreement_date
            FROM reports1nf_arenda_rented ar
            WHERE ar.id = @ar_id AND ar.report_id = @rep_id";

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.Parameters.Add(new SqlParameter("ar_id", arendaRentedId));
            cmd.Parameters.Add(new SqlParameter("rep_id", reportId));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string agreementNum = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();
                    object agreementDate = reader.IsDBNull(1) ? null : (object)reader.GetDateTime(1);

                    string dateStr = "(не вказано)";

                    if (agreementDate is DateTime)
                        dateStr = ((DateTime)agreementDate).ToShortDateString();

                    if (agreementNum.Length == 0)
                        agreementNum = "(не вказано)";

                    description = string.Format(Resources.Strings.Reports1NFRentedFormat, agreementNum, dateStr);
                }

                reader.Close();
            }
        }

        return description;
    }

    private static void FormatEmailNotification(out string subject, out string body, string subjectTemplate, string bodyTemplate,
        string userName, string orgName, string objectDescription, string infoArea, string comment, string url)
    {
        subject = subjectTemplate;

        body = string.Format(bodyTemplate, new object[] { userName, orgName, objectDescription, infoArea, comment, url });
    }

    private static void FormatOrgInfoNotification(out string subject, out string body, string subjectTemplate, string bodyTemplate,
        string userName, string orgName, string objectDescription, string infoArea, string comment, string url, int reportId)
    {
        // Build a correct url for Organization Info page
        if (url.EndsWith("/"))
        {
            url = url.Substring(0, url.Length - 1);
        }

        string cardUrl = string.Format("{0}/Reports1NF/OrgInfo.aspx?rid={1}", url, reportId);

        FormatEmailNotification(out subject, out body, subjectTemplate, bodyTemplate,
            userName, orgName, objectDescription, infoArea, comment, cardUrl);
    }

    private static void FormatBalansObjNotification(out string subject, out string body, string subjectTemplate, string bodyTemplate,
        string userName, string orgName, string objectDescription, string infoArea, string comment, string url, int balansId, int reportId)
    {
        // Build a correct url for Balans Object page
        if (url.EndsWith("/"))
        {
            url = url.Substring(0, url.Length - 1);
        }

        string cardUrl = string.Format("{0}/Reports1NF/OrgBalansObject.aspx?rid={1}&bid={2}", url, reportId, balansId);

        FormatEmailNotification(out subject, out body, subjectTemplate, bodyTemplate,
            userName, orgName, objectDescription, infoArea, comment, cardUrl);
    }

    private static void FormatDeletedObjNotification(out string subject, out string body, string subjectTemplate, string bodyTemplate,
        string userName, string orgName, string objectDescription, string infoArea, string comment, string url, int balansDeletedId, int reportId)
    {
        // Build a correct url for Deleted Balans Object page
        if (url.EndsWith("/"))
        {
            url = url.Substring(0, url.Length - 1);
        }

        string cardUrl = string.Format("{0}/Reports1NF/OrgBalansDeletedObject.aspx?rid={1}&bid={2}", url, reportId, balansDeletedId);

        FormatEmailNotification(out subject, out body, subjectTemplate, bodyTemplate,
            userName, orgName, objectDescription, infoArea, comment, cardUrl);
    }

    private static void FormatRentAgreementNotification(out string subject, out string body, string subjectTemplate, string bodyTemplate,
        string userName, string orgName, string objectDescription, string infoArea, string comment, string url, int agreementId, int reportId)
    {
        // Build a correct url for Rent Agreement page
        if (url.EndsWith("/"))
        {
            url = url.Substring(0, url.Length - 1);
        }

        string cardUrl = string.Format("{0}/Reports1NF/OrgRentAgreement.aspx?rid={1}&aid={2}", url, reportId, agreementId);

        FormatEmailNotification(out subject, out body, subjectTemplate, bodyTemplate,
            userName, orgName, objectDescription, infoArea, comment, cardUrl);
    }

    private static void FormatRentedObjectNotification(out string subject, out string body, string subjectTemplate, string bodyTemplate,
        string userName, string orgName, string objectDescription, string infoArea, string comment, string url, int rentedObjectId, int reportId)
    {
        // Build a correct url for Rented Object page
        if (url.EndsWith("/"))
        {
            url = url.Substring(0, url.Length - 1);
        }

        string cardUrl = string.Format("{0}/Reports1NF/OrgRentedObject.aspx?rid={1}&aid={2}", url, reportId, rentedObjectId);

        FormatEmailNotification(out subject, out body, subjectTemplate, bodyTemplate,
            userName, orgName, objectDescription, infoArea, comment, cardUrl);
    }

    #endregion (Email notifications)

    #region Multithreading support for lengthy operations

    public static object thisLock = new object();
    public static object thisDisplayLock = new object();

    private static Dictionary<string, WorkItem> WorkItems
    {
        get
        {
            object val = HttpContext.Current.Session["REPORTS_1NF_WORK_ITEMS"];

            if (val is Dictionary<string, WorkItem>)
            {
                return val as Dictionary<string, WorkItem>;
            }

            Dictionary<string, WorkItem> workItems = new Dictionary<string, WorkItem>();

            HttpContext.Current.Session["REPORTS_1NF_WORK_ITEMS"] = workItems;

            return workItems;
        }

        set
        {
            HttpContext.Current.Session["REPORTS_1NF_WORK_ITEMS"] = value;
        }
    }


    public static string StartWorkItem(WorkItem item, int reportId, string user)
    {
        string id = "";

        lock (thisDisplayLock)
        {
            item.Init(reportId, user);
        }

        lock (thisLock)
        {
            item.id = Guid.NewGuid().ToString();

            id = item.id;

            Dictionary<string, WorkItem> workItems = WorkItems;

            workItems.Add(id, item);

            item.StartProcessing();
        }

        return id;
    }

    public static void GetWorkItemStatistics(string workItemId, ref int percentComplete, ref string message, ref bool finished)
    {
        lock (thisLock)
        {
            Dictionary<string, WorkItem> workItems = WorkItems;
            WorkItem item = null;

            if (workItems != null && workItems.TryGetValue(workItemId, out item))
            {
                if (item.numObjectsToProcess > 0)
                {
                    percentComplete = (int)(100.0 * (double)item.numObjectsProcessed / (double)item.numObjectsToProcess);
                }

                message = item.message;
                finished = item.finished;
            }
        }
    }

    public static void StopWorkItem(string workItemId)
    {
        lock (thisLock)
        {
            Dictionary<string, WorkItem> workItems = WorkItems;

            if (workItems != null)
            {
                workItems.Remove(workItemId);
            }
        }
    }

    public static void ProcessProgressPanelCallback(ASPxCallbackPanel panel, string callbackParam,
        ASPxProgressBar progressBar, ASPxLabel label, int reportId, WorkItem workItem, string sessionKey, string processingMessage)
    {
        if (callbackParam.StartsWith("init:"))
        {
            System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser();
            string username = (user == null ? "Auto-import" : user.UserName);

            string workItemId = Reports1NFUtils.StartWorkItem(workItem, reportId, username);

            panel.JSProperties["cpWorkItemId"] = workItemId;

            HttpContext.Current.Session[sessionKey + reportId.ToString()] = workItemId;

            label.Text = processingMessage;
            progressBar.Position = 0;
        }
        else if (callbackParam.StartsWith("timer:"))
        {
//            string workItemId = (string)HttpContext.Current.Session[sessionKey + reportId.ToString()];
            string workItemId = (string)HttpContext.Current.Session[sessionKey + reportId.ToString()] ?? "";//@@@@@@
            panel.JSProperties["cpWorkItemId"] = workItemId;

            if (workItemId.Length > 0)
            {
                bool finished = false;
                string message = "";
                int progress = 0;

                Reports1NFUtils.GetWorkItemStatistics(workItemId, ref progress, ref message, ref finished);

                label.Text = message.Length > 0 ? message : processingMessage;

                if (finished)
                {
                    progressBar.Position = 100;
                    HttpContext.Current.Session[sessionKey + reportId.ToString()] = "";
                    panel.JSProperties["cpWorkItemId"] = "";

                    Reports1NFUtils.StopWorkItem(workItemId);
                }
                else
                {
                    progressBar.Position = progress;
                }
            }
        }
    }

    #endregion (Multithreading support for lengthy operations)

    public static int DEBTKVART_COUNT = 16;
}