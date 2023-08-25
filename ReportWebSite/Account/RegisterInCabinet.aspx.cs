using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

public partial class Account_Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
    }

    protected void RegisterUser_CreatedUser(object sender, EventArgs e)
    {
        var newUser = Membership.GetUser(RegisterUser.UserName);
        var newUserId = (Guid)newUser.ProviderUserKey;

        var nameF = ((TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("NameF")).Text;
        var nameI = ((TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("NameI")).Text;
        var nameO = ((TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("NameO")).Text;
        var phone = ((TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Phone")).Text;
        var userName = ((TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("UserName")).Text;

        var db = Utils.ConnectToDatabase();

        var myCommand = new SqlCommand("update aspnet_Membership set IsCabinet = 1, nameF = @nameF, nameI = @nameI, nameO = @nameO, Phone = @phone, Email = @email where UserId = @userId", db);
        myCommand.Parameters.AddWithValue("@nameF", nameF);
        myCommand.Parameters.AddWithValue("@nameI", nameI);
        myCommand.Parameters.AddWithValue("@nameO", nameO);
        myCommand.Parameters.AddWithValue("@phone", phone);
        myCommand.Parameters.AddWithValue("@userId", newUserId);
        myCommand.Parameters.AddWithValue("@email", userName);
        myCommand.ExecuteNonQuery();
        db.Close();

        FormsAuthentication.SetAuthCookie(RegisterUser.UserName, true);

        object returnUrl = Request.QueryString["ReturnUrl"];
        if (returnUrl != null)
        {
            Response.Redirect(returnUrl.ToString());
        }
    }
}
