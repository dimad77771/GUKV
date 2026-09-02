using System;
using System.Globalization;
using System.Web;
using System.Web.UI;

public partial class UserControls_PasswordExpirationNotice : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PasswordExpirationStatus status = PasswordExpirationPolicy.GetCurrentUserStatus();
        if (!status.IsApplicable || (!status.IsWarning && !status.IsExpired))
        {
            NoticePanel.Visible = false;
            return;
        }

        string deadline = status.DeadlineDate.ToString(
            "dd.MM.yyyy",
            CultureInfo.GetCultureInfo("uk-UA"));

        NoticePanel.Visible = true;
        NoticePanel.CssClass = status.IsExpired
            ? "password-expiration-notice password-expiration-notice--expired"
            : "password-expiration-notice password-expiration-notice--warning";

        NoticeText.Text = status.IsExpired
            ? string.Format(
                "<span class=\"password-expiration-notice__message\">\u0421\u0442\u0440\u043E\u043A \u0434\u0456\u0457 \u043F\u0430\u0440\u043E\u043B\u044F \u0437\u0430\u0432\u0435\u0440\u0448\u0438\u0432\u0441\u044F. \u041F\u0430\u0440\u043E\u043B\u044C \u043F\u043E\u0442\u0440\u0456\u0431\u043D\u043E \u0431\u0443\u043B\u043E \u0437\u043C\u0456\u043D\u0438\u0442\u0438 \u0434\u043E <strong>{0}</strong> \u0432\u043A\u043B\u044E\u0447\u043D\u043E.</span>",
                HttpUtility.HtmlEncode(deadline))
            : string.Format(
                "<span class=\"password-expiration-notice__message\">\u0423\u0432\u0430\u0433\u0430: \u043F\u0430\u0440\u043E\u043B\u044C \u043F\u043E\u0442\u0440\u0456\u0431\u043D\u043E \u0437\u043C\u0456\u043D\u0438\u0442\u0438 \u0434\u043E <strong>{0}</strong> \u0432\u043A\u043B\u044E\u0447\u043D\u043E.</span>",
                HttpUtility.HtmlEncode(deadline));

        ChangePasswordLink.Text = "\u0417\u043C\u0456\u043D\u0438\u0442\u0438 \u043F\u0430\u0440\u043E\u043B\u044C";
        ChangePasswordLink.NavigateUrl = ResolveUrl("~/Account/ChangePassword.aspx");
        VideoLink.Text = "\u0412\u0456\u0434\u0435\u043E\u0456\u043D\u0441\u0442\u0440\u0443\u043A\u0446\u0456\u044F";
        VideoLink.NavigateUrl = PasswordExpirationPolicy.VideoUrl;
        VideoLink.Attributes["rel"] = "noopener noreferrer";
    }
}
