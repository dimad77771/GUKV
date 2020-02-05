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

public partial class Admin_Users : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(Utils.UsersAdmin))
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

            if (fieldName == "Email")
            {
                if (e.NewValues[dataColumn.FieldName] == null)
                    e.Errors[dataColumn] = "Заповніть адресу електронної пошти";
                if (e.NewValues[dataColumn.FieldName] != null && ((String)e.NewValues[dataColumn.FieldName]).Length > 256)
                    e.Errors[dataColumn] = "Адреса електронної пошти не може бути довше 256 знаків";
            }

            if (fieldName == "UserName")
            {
                if (e.NewValues[dataColumn.FieldName] == null)
                    e.Errors[dataColumn] = "Заповніть ПІБ користувача";
                if (e.NewValues[dataColumn.FieldName] != null && ((String)e.NewValues[dataColumn.FieldName]).Length > 256)
                    e.Errors[dataColumn] = "ПІБ користувача не може бути довше 256 знаків";
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


}