using DevExpress.Web;
using DevExpress.Web.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_DicOrgOccupation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(Utils.DictionartyAdmin))
        {
            string errorurl = Page.ResolveClientUrl("~/Account/Restricted.aspx");
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(errorurl);
            else
                Response.Redirect(errorurl);
        }
    }

    protected void ASPxGridViewDicOrgOccupation_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        foreach (GridViewColumn column in ASPxGridViewDicOrgOccupation.Columns)
        {
            GridViewDataColumn dataColumn = column as GridViewDataColumn;
            if (dataColumn == null) continue;
            string fieldName = dataColumn.FieldName.ToLower();

            if (fieldName == "name")
            {
                if (e.NewValues[dataColumn.FieldName] == null)
                    e.Errors[dataColumn] = "Заповніть назву виду діяльності";
                if (e.NewValues[dataColumn.FieldName] != null && ((String)e.NewValues[dataColumn.FieldName]).Length > 50)
                    e.Errors[dataColumn] = "Назва виду діяльності не може бути довше 50 знаків";
            }

            if (fieldName == "branch_code")
            {
                if (e.NewValues[dataColumn.FieldName] == null)
                    e.Errors[dataColumn] = "Заповніть цифровий код галузі";
            }

        }

        if (e.Errors.Count > 0)
            e.RowError = "Заповніть обов'язкові поля.";
    }

    protected void ASPxGridViewDicOrgOccupation_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        if (!ASPxGridViewDicOrgOccupation.IsNewRowEditing)
        {
            ASPxGridViewDicOrgOccupation.DoRowValidation();
        }
    }

    protected void SqlDataSourceDicOrgOccupation_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {
        string query = "select max(id)+1 from [dict_org_occupation]";
        int maxID = -1;

        SqlConnection connectionSql = Utils.ConnectToDatabase();
        using (SqlCommand cmd = new SqlCommand(query, connectionSql))
        {
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    maxID = reader.GetInt32(0);
                }

                reader.Close();
            }
        }

        e.Command.Parameters["@id"].Value = maxID;
        e.Command.Parameters["@modify_date"].Value = DateTime.Now;
        MembershipUser user = Membership.GetUser();
        e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;
    }
    protected void SqlDataSourceDicOrgOccupation_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        e.Command.Parameters["@modify_date"].Value = DateTime.Now;
        MembershipUser user = Membership.GetUser();
        e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;
    }
}