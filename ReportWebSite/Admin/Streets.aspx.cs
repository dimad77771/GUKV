using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Streets : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(Utils.DictionartyAdmin))
        {
            string errorurl = Page.ResolveClientUrl("~/Account/Restricted.aspx");
            if (Page.IsCallback)
                DevExpress.Web.ASPxClasses.ASPxWebControl.RedirectOnCallback(errorurl);
            else
                Response.Redirect(errorurl);
        }
    }

    protected void ASPxGridViewFreeSquare_RowValidating(object sender, ASPxDataValidationEventArgs e)
    {
        foreach (GridViewColumn column in ASPxGridViewFreeSquare.Columns)
        {
            GridViewDataColumn dataColumn = column as GridViewDataColumn;
            if (dataColumn == null) continue;
            string fieldName = dataColumn.FieldName.ToLower();

            if (fieldName == "name")
            { 
                if (e.NewValues[dataColumn.FieldName] == null)
                    e.Errors[dataColumn] = "Заповніть назву вулиці великими літерами";
                if (e.NewValues[dataColumn.FieldName] != null && ((String)e.NewValues[dataColumn.FieldName]).Length > 160)
                    e.Errors[dataColumn] = "Назва вулиці не може бути довше 160 знаків";
            }

            if (fieldName == "name_output")
            {
                if (e.NewValues[dataColumn.FieldName] == null)
                    e.Errors[dataColumn] = "Заповніть назву вулиці";
                if (e.NewValues[dataColumn.FieldName] != null && ((String)e.NewValues[dataColumn.FieldName]).Length > 160)
                    e.Errors[dataColumn] = "Назва вулиці не може бути довше 160 знаків";
            }

            if (fieldName == "kind")
            {
                if  (e.NewValues[dataColumn.FieldName] == null)
                    e.Errors[dataColumn] = "Заповніть тип вулиці (вул. пров. ";
                if (e.NewValues[dataColumn.FieldName] != null && ((String)e.NewValues[dataColumn.FieldName]).Length > 50)
                    e.Errors[dataColumn] = "Тип вулиці не може бути довше 50 знаків";
            }

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

    protected void SqlDataSourceFreeSquare_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {
        string query = "select max(id)+1 from [dict_streets]";
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
    protected void SqlDataSourceFreeSquare_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        e.Command.Parameters["@modify_date"].Value = DateTime.Now;
        MembershipUser user = Membership.GetUser();
        e.Command.Parameters["@modified_by"].Value = user == null ? String.Empty : (String)user.UserName;
    }
}