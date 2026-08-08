using DevExpress.Web;
using DevExpress.Web.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;

public partial class Admin_NormalizationTexts : Page
{
    private const string NormalizationCacheKey = "GUKV.NormalizationTexts";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(Utils.DictionartyAdmin))
        {
            string errorUrl = Page.ResolveClientUrl("~/Account/Restricted.aspx");

            if (Page.IsCallback)
                ASPxWebControl.RedirectOnCallback(errorUrl);
            else
                Response.Redirect(errorUrl);
        }
    }

    protected void ASPxGridViewNormalizationTexts_RowValidating(
        object sender,
        ASPxDataValidationEventArgs e)
    {
        GridViewDataColumn column =
            ASPxGridViewNormalizationTexts.Columns["txt"] as GridViewDataColumn;

        object value = e.NewValues["txt"];
        var text = value == null ? null : Convert.ToString(value);
        var error = "";

        if (string.IsNullOrWhiteSpace(text))
        {
            error = "Заповніть нормалізований текст.";
        }
        else if (text.Length > 8000)
        {
			error = "Текст не може бути довшим за 8000 знаків.";
        }
        else if (NormalizationTextExists(text, e.OldValues["txt"]))
        {
			error = "Такий нормалізований текст уже існує.";
        }

        if (!string.IsNullOrEmpty(error))
        {
            e.RowError = error;
        }
	}

    private bool NormalizationTextExists(string newText, object oldValue)
    {
        string oldText = oldValue == null ? null : Convert.ToString(oldValue);

        const string query = @"
SELECT CASE WHEN EXISTS
(
    SELECT 1
    FROM dbo.NormalizationTexts
    WHERE txt = @txt
      AND (@old_txt IS NULL OR txt <> @old_txt)
)
THEN 1 ELSE 0 END";

        using (SqlConnection connection = Utils.ConnectToDatabase())
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.Add("@txt", SqlDbType.VarChar, 8000).Value = newText;
            command.Parameters.Add("@old_txt", SqlDbType.VarChar, 8000).Value =
                oldText == null ? (object)DBNull.Value : oldText;

            object result = command.ExecuteScalar();
            return Convert.ToInt32(result) == 1;
        }
    }

    protected void SqlDataSourceNormalizationTexts_Changed(
        object sender,
        System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception == null)
            HttpRuntime.Cache.Remove(NormalizationCacheKey);
    }
}
