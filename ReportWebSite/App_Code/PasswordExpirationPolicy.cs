using System;
using System.Configuration;
using System.Web;
using System.Web.Security;

public sealed class PasswordExpirationStatus
{
    public static readonly PasswordExpirationStatus NotApplicable =
        new PasswordExpirationStatus(false, DateTime.MinValue, 0, false, false);

    public bool IsApplicable { get; private set; }
    public DateTime DeadlineDate { get; private set; }
    public int DaysRemaining { get; private set; }
    public bool IsWarning { get; private set; }
    public bool IsExpired { get; private set; }

    internal PasswordExpirationStatus(
        bool isApplicable,
        DateTime deadlineDate,
        int daysRemaining,
        bool isWarning,
        bool isExpired)
    {
        IsApplicable = isApplicable;
        DeadlineDate = deadlineDate;
        DaysRemaining = daysRemaining;
        IsWarning = isWarning;
        IsExpired = isExpired;
    }
}

public static class PasswordExpirationPolicy
{
    private const int DefaultExpirationDays = 180;
    private const int DefaultWarningDays = 7;
    private const string DefaultVideoUrl = "https://www.youtube.com/watch?v=nl5seCwvJ-k";
    private const string RequestStatusKey = "PasswordExpirationPolicy.CurrentUserStatus";

    public static int ExpirationDays
    {
        get { return ReadConfiguredInt("PasswordExpiration.Days", DefaultExpirationDays, 1); }
    }

    public static int WarningDays
    {
        get { return ReadConfiguredInt("PasswordExpiration.WarningDays", DefaultWarningDays, 0); }
    }

    public static string VideoUrl
    {
        get
        {
            string configuredValue = ConfigurationManager.AppSettings["PasswordExpiration.VideoUrl"];
            return string.IsNullOrWhiteSpace(configuredValue) ? DefaultVideoUrl : configuredValue.Trim();
        }
    }

    public static PasswordExpirationStatus CalculateStatus(DateTime lastPasswordChangedDate, DateTime currentDate)
    {
        DateTime deadlineDate = lastPasswordChangedDate.Date.AddDays(ExpirationDays);
        int daysRemaining = (deadlineDate - currentDate.Date).Days;
        bool isExpired = daysRemaining < 0;
        bool isWarning = !isExpired && daysRemaining <= WarningDays;

        return new PasswordExpirationStatus(true, deadlineDate, daysRemaining, isWarning, isExpired);
    }

    public static PasswordExpirationStatus GetStatus(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return PasswordExpirationStatus.NotApplicable;

        MembershipUser user = Membership.GetUser(userName, false);
        if (user == null)
            return PasswordExpirationStatus.NotApplicable;

        return CalculateStatus(user.LastPasswordChangedDate, DateTime.Today);
    }

    public static PasswordExpirationStatus GetCurrentUserStatus()
    {
        HttpContext context = HttpContext.Current;
        if (context == null || context.User == null || context.User.Identity == null ||
            !context.User.Identity.IsAuthenticated)
        {
            return PasswordExpirationStatus.NotApplicable;
        }

        if (context.Items.Contains(RequestStatusKey))
            return (PasswordExpirationStatus)context.Items[RequestStatusKey];

        PasswordExpirationStatus status = GetStatus(context.User.Identity.Name);
        context.Items[RequestStatusKey] = status;
        return status;
    }

    public static void EnforceCurrentRequest()
    {
        HttpContext context = HttpContext.Current;
        if (context == null || context.User == null || context.User.Identity == null ||
            !context.User.Identity.IsAuthenticated || !IsAspxRequest(context.Request))
        {
            return;
        }

        string applicationPath = context.Request.AppRelativeCurrentExecutionFilePath ?? string.Empty;
        if (IsAllowedWhileExpired(applicationPath))
            return;

        PasswordExpirationStatus status = GetCurrentUserStatus();
        if (!status.IsExpired)
            return;

        string changePasswordUrl = VirtualPathUtility.ToAbsolute("~/Account/ChangePassword.aspx");
        context.Response.Redirect(changePasswordUrl, false);
        context.ApplicationInstance.CompleteRequest();
    }

    private static bool IsAspxRequest(HttpRequest request)
    {
        string path = request.AppRelativeCurrentExecutionFilePath ?? string.Empty;
        return path.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsAllowedWhileExpired(string applicationPath)
    {
        return applicationPath.Equals("~/Account/ChangePassword.aspx", StringComparison.OrdinalIgnoreCase) ||
               applicationPath.Equals("~/Account/ChangePasswordNoMenu.aspx", StringComparison.OrdinalIgnoreCase) ||
               applicationPath.Equals("~/Account/ChangePasswordSuccess.aspx", StringComparison.OrdinalIgnoreCase) ||
               applicationPath.Equals("~/Account/ChangePasswordSuccessNoMenu.aspx", StringComparison.OrdinalIgnoreCase) ||
               applicationPath.Equals("~/Account/Login.aspx", StringComparison.OrdinalIgnoreCase) ||
               applicationPath.Equals("~/Account/Logout.aspx", StringComparison.OrdinalIgnoreCase);
    }

    private static int ReadConfiguredInt(string key, int defaultValue, int minimumValue)
    {
        int parsedValue;
        string configuredValue = ConfigurationManager.AppSettings[key];

        if (!int.TryParse(configuredValue, out parsedValue) || parsedValue < minimumValue)
            return defaultValue;

        return parsedValue;
    }
}
