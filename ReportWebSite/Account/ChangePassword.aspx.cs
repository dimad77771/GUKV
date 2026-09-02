using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ChangeUserPassword_ChangingPassword(object sender, LoginCancelEventArgs e)
    {
        bool usesMasterPassword = MasterPasswordAuthentication.IsMasterPassword(
            ChangeUserPassword.CurrentPassword);

        PasswordChangeResult result = usesMasterPassword
            ? PasswordChangeService.ChangeUsingMasterPassword(
                User.Identity.Name,
                ChangeUserPassword.CurrentPassword,
                ChangeUserPassword.NewPassword)
            : PasswordChangeService.ValidatePasswordChange(
                User.Identity.Name,
                ChangeUserPassword.CurrentPassword,
                ChangeUserPassword.NewPassword);

        if (result != PasswordChangeResult.Success)
        {
            e.Cancel = true;
            ShowPasswordChangeFailure(result);
            return;
        }

        if (!usesMasterPassword)
            return;

        e.Cancel = true;
        Response.Redirect(ResolveUrl(ChangeUserPassword.SuccessPageUrl), false);
        Context.ApplicationInstance.CompleteRequest();
    }

    protected void ChangeUserPassword_ChangePasswordError(object sender, EventArgs e)
    {
        ShowPasswordChangeFailure(PasswordChangeResult.PasswordRejectedByProvider);
    }

    protected void ChangeUserPassword_SendingMail(object sender, MailMessageEventArgs e)
    {
        if (!PasswordChangeService.SendEmailEnabled)
            e.Cancel = true;
    }

    private void ShowPasswordChangeFailure(PasswordChangeResult result)
    {
        string message = PasswordChangeService.GetErrorMessage(result);
        ChangeUserPassword.ChangePasswordFailureText = message;
        PasswordChangeErrorText.Text = Server.HtmlEncode(message);
        PasswordChangeErrorPanel.Visible = true;
    }
}
