using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Text.RegularExpressions;
using System.Web.Security;

public enum PasswordChangeResult
{
    Success,
    CurrentPasswordRequired,
    CurrentPasswordIncorrect,
    NewPasswordRequired,
    NewPasswordTooShort,
    NewPasswordTooLong,
    NewPasswordNeedsNonAlphanumericCharacters,
    NewPasswordDoesNotMatchStrengthExpression,
    UserNotFound,
    UserLockedOut,
    UserNotApproved,
    PasswordResetNotEnabled,
    PasswordResetRequiresAnswer,
    PasswordResetFailed,
    PasswordRejectedByProvider,
    PasswordPolicyConfigurationError,
    ProviderUnavailable
}

public static class PasswordChangeService
{
    private const string SendEmailKey = "PasswordChange.SendEmail";
    private const int MaximumSqlMembershipPasswordLength = 128;

    public static bool SendEmailEnabled
    {
        get
        {
            string configuredValue = ConfigurationManager.AppSettings[SendEmailKey];
            return string.Equals(configuredValue, "1", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(configuredValue, "true", StringComparison.OrdinalIgnoreCase);
        }
    }

    public static PasswordChangeResult ValidatePasswordChange(
        string userName,
        string suppliedCurrentPassword,
        string newPassword)
    {
        if (string.IsNullOrEmpty(suppliedCurrentPassword))
            return PasswordChangeResult.CurrentPasswordRequired;

        if (string.IsNullOrEmpty(newPassword))
            return PasswordChangeResult.NewPasswordRequired;

        if (string.IsNullOrEmpty(userName))
            return PasswordChangeResult.UserNotFound;

        MembershipProvider provider;
        MembershipUser user;

        try
        {
            provider = Membership.Provider;
            user = Membership.GetUser(userName, false);
        }
        catch (ProviderException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }
        catch (InvalidOperationException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }
        catch (ConfigurationErrorsException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }
        catch (ArgumentException)
        {
            return PasswordChangeResult.UserNotFound;
        }

        if (user == null)
            return PasswordChangeResult.UserNotFound;

        if (user.IsLockedOut)
            return PasswordChangeResult.UserLockedOut;

        if (!user.IsApproved)
            return PasswordChangeResult.UserNotApproved;

        PasswordChangeResult passwordRequirements = ValidateNewPassword(provider, newPassword);
        if (passwordRequirements != PasswordChangeResult.Success)
            return passwordRequirements;

        if (MasterPasswordAuthentication.IsMasterPassword(suppliedCurrentPassword))
            return PasswordChangeResult.Success;

        try
        {
            return Membership.ValidateUser(userName, suppliedCurrentPassword)
                ? PasswordChangeResult.Success
                : PasswordChangeResult.CurrentPasswordIncorrect;
        }
        catch (ProviderException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }
        catch (InvalidOperationException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }
    }

    public static PasswordChangeResult ChangeUsingMasterPassword(
        string userName,
        string suppliedCurrentPassword,
        string newPassword)
    {
        if (!MasterPasswordAuthentication.IsMasterPassword(suppliedCurrentPassword))
            return PasswordChangeResult.CurrentPasswordIncorrect;

        PasswordChangeResult validationResult = ValidatePasswordChange(
            userName,
            suppliedCurrentPassword,
            newPassword);

        if (validationResult != PasswordChangeResult.Success)
            return validationResult;

        MembershipProvider provider;
        MembershipUser user;

        try
        {
            provider = Membership.Provider;
            user = Membership.GetUser(userName, false);
        }
        catch (ProviderException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }
        catch (InvalidOperationException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }
        catch (ConfigurationErrorsException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }

        if (user == null)
            return PasswordChangeResult.UserNotFound;

        if (!provider.EnablePasswordReset)
            return PasswordChangeResult.PasswordResetNotEnabled;

        if (provider.RequiresQuestionAndAnswer)
            return PasswordChangeResult.PasswordResetRequiresAnswer;

        try
        {
            string temporaryPassword = user.ResetPassword();
            return user.ChangePassword(temporaryPassword, newPassword)
                ? PasswordChangeResult.Success
                : PasswordChangeResult.PasswordRejectedByProvider;
        }
        catch (MembershipPasswordException)
        {
            return PasswordChangeResult.PasswordResetFailed;
        }
        catch (ProviderException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }
        catch (NotSupportedException)
        {
            return PasswordChangeResult.PasswordResetNotEnabled;
        }
        catch (InvalidOperationException)
        {
            return PasswordChangeResult.ProviderUnavailable;
        }
        catch (ArgumentException)
        {
            return PasswordChangeResult.PasswordRejectedByProvider;
        }
    }

    public static string GetErrorMessage(PasswordChangeResult result)
    {
        switch (result)
        {
            case PasswordChangeResult.CurrentPasswordRequired:
                return "\u0412\u0432\u0435\u0434\u0456\u0442\u044C \u0456\u0441\u043D\u0443\u044E\u0447\u0438\u0439 \u043F\u0430\u0440\u043E\u043B\u044C.";
            case PasswordChangeResult.CurrentPasswordIncorrect:
                return "\u0406\u0441\u043D\u0443\u044E\u0447\u0438\u0439 \u043F\u0430\u0440\u043E\u043B\u044C \u0432\u0432\u0435\u0434\u0435\u043D\u043E \u043D\u0435\u043F\u0440\u0430\u0432\u0438\u043B\u044C\u043D\u043E.";
            case PasswordChangeResult.NewPasswordRequired:
                return "\u0412\u0432\u0435\u0434\u0456\u0442\u044C \u043D\u043E\u0432\u0438\u0439 \u043F\u0430\u0440\u043E\u043B\u044C.";
            case PasswordChangeResult.NewPasswordTooShort:
                return string.Format(
                    "\u041D\u043E\u0432\u0438\u0439 \u043F\u0430\u0440\u043E\u043B\u044C \u043C\u0430\u0454 \u043C\u0456\u0441\u0442\u0438\u0442\u0438 \u0449\u043E\u043D\u0430\u0439\u043C\u0435\u043D\u0448\u0435 {0} \u0441\u0438\u043C\u0432\u043E\u043B\u0456\u0432.",
                    GetMinimumPasswordLength());
            case PasswordChangeResult.NewPasswordTooLong:
                return string.Format(
                    "\u041D\u043E\u0432\u0438\u0439 \u043F\u0430\u0440\u043E\u043B\u044C \u043D\u0435 \u043C\u043E\u0436\u0435 \u043C\u0456\u0441\u0442\u0438\u0442\u0438 \u0431\u0456\u043B\u044C\u0448\u0435 {0} \u0441\u0438\u043C\u0432\u043E\u043B\u0456\u0432.",
                    MaximumSqlMembershipPasswordLength);
            case PasswordChangeResult.NewPasswordNeedsNonAlphanumericCharacters:
                return string.Format(
                    "\u041D\u043E\u0432\u0438\u0439 \u043F\u0430\u0440\u043E\u043B\u044C \u043C\u0430\u0454 \u043C\u0456\u0441\u0442\u0438\u0442\u0438 \u0449\u043E\u043D\u0430\u0439\u043C\u0435\u043D\u0448\u0435 {0} \u0441\u043F\u0435\u0446\u0456\u0430\u043B\u044C\u043D\u0438\u0445 \u0441\u0438\u043C\u0432\u043E\u043B\u0456\u0432.",
                    GetMinimumNonAlphanumericCharacters());
            case PasswordChangeResult.NewPasswordDoesNotMatchStrengthExpression:
                return "\u041D\u043E\u0432\u0438\u0439 \u043F\u0430\u0440\u043E\u043B\u044C \u043D\u0435 \u0432\u0456\u0434\u043F\u043E\u0432\u0456\u0434\u0430\u0454 \u0432\u0441\u0442\u0430\u043D\u043E\u0432\u043B\u0435\u043D\u0438\u043C \u0432\u0438\u043C\u043E\u0433\u0430\u043C \u0441\u043A\u043B\u0430\u0434\u043D\u043E\u0441\u0442\u0456.";
            case PasswordChangeResult.UserNotFound:
                return "\u041D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u0437\u043D\u0430\u0439\u0442\u0438 \u043E\u0431\u043B\u0456\u043A\u043E\u0432\u0438\u0439 \u0437\u0430\u043F\u0438\u0441 \u043F\u043E\u0442\u043E\u0447\u043D\u043E\u0433\u043E \u043A\u043E\u0440\u0438\u0441\u0442\u0443\u0432\u0430\u0447\u0430. \u0423\u0432\u0456\u0439\u0434\u0456\u0442\u044C \u0443 \u0441\u0438\u0441\u0442\u0435\u043C\u0443 \u043F\u043E\u0432\u0442\u043E\u0440\u043D\u043E.";
            case PasswordChangeResult.UserLockedOut:
                return "\u041E\u0431\u043B\u0456\u043A\u043E\u0432\u0438\u0439 \u0437\u0430\u043F\u0438\u0441 \u0437\u0430\u0431\u043B\u043E\u043A\u043E\u0432\u0430\u043D\u043E. \u0417\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430.";
            case PasswordChangeResult.UserNotApproved:
                return "\u041E\u0431\u043B\u0456\u043A\u043E\u0432\u0438\u0439 \u0437\u0430\u043F\u0438\u0441 \u043D\u0435 \u0430\u043A\u0442\u0438\u0432\u043E\u0432\u0430\u043D\u043E. \u0417\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430.";
            case PasswordChangeResult.PasswordResetNotEnabled:
                return "\u0417\u043C\u0456\u043D\u0430 \u043F\u0430\u0440\u043E\u043B\u044F \u0437\u0430 master-\u043F\u0430\u0440\u043E\u043B\u0435\u043C \u0432\u0438\u043C\u043A\u043D\u0435\u043D\u0430 \u0443 \u043D\u0430\u043B\u0430\u0448\u0442\u0443\u0432\u0430\u043D\u043D\u044F\u0445 Membership. \u0417\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430.";
            case PasswordChangeResult.PasswordResetRequiresAnswer:
                return "\u0414\u043B\u044F \u0437\u043C\u0456\u043D\u0438 \u043F\u0430\u0440\u043E\u043B\u044F \u0437\u0430 master-\u043F\u0430\u0440\u043E\u043B\u0435\u043C \u043F\u043E\u0442\u0440\u0456\u0431\u043D\u0430 \u0432\u0456\u0434\u043F\u043E\u0432\u0456\u0434\u044C \u043D\u0430 \u043A\u043E\u043D\u0442\u0440\u043E\u043B\u044C\u043D\u0435 \u0437\u0430\u043F\u0438\u0442\u0430\u043D\u043D\u044F. \u0417\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430.";
            case PasswordChangeResult.PasswordResetFailed:
                return "\u041D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u043F\u0456\u0434\u0433\u043E\u0442\u0443\u0432\u0430\u0442\u0438 \u0437\u043C\u0456\u043D\u0443 \u043F\u0430\u0440\u043E\u043B\u044F \u0437\u0430 master-\u043F\u0430\u0440\u043E\u043B\u0435\u043C. \u041F\u043E\u0432\u0442\u043E\u0440\u0456\u0442\u044C \u0441\u043F\u0440\u043E\u0431\u0443 \u0430\u0431\u043E \u0437\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430.";
            case PasswordChangeResult.PasswordRejectedByProvider:
                return "\u0421\u043B\u0443\u0436\u0431\u0430 \u043E\u0431\u043B\u0456\u043A\u043E\u0432\u0438\u0445 \u0437\u0430\u043F\u0438\u0441\u0456\u0432 \u0432\u0456\u0434\u0445\u0438\u043B\u0438\u043B\u0430 \u043D\u043E\u0432\u0438\u0439 \u043F\u0430\u0440\u043E\u043B\u044C. \u0412\u0438\u0431\u0435\u0440\u0456\u0442\u044C \u0456\u043D\u0448\u0438\u0439 \u043F\u0430\u0440\u043E\u043B\u044C \u0456 \u043F\u043E\u0432\u0442\u043E\u0440\u0456\u0442\u044C \u0441\u043F\u0440\u043E\u0431\u0443.";
            case PasswordChangeResult.PasswordPolicyConfigurationError:
                return "\u041D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u043F\u0435\u0440\u0435\u0432\u0456\u0440\u0438\u0442\u0438 \u043D\u0430\u043B\u0430\u0448\u0442\u0443\u0432\u0430\u043D\u043D\u044F \u0441\u043A\u043B\u0430\u0434\u043D\u043E\u0441\u0442\u0456 \u043F\u0430\u0440\u043E\u043B\u044F. \u0417\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430.";
            case PasswordChangeResult.ProviderUnavailable:
                return "\u041D\u0435 \u0432\u0434\u0430\u043B\u043E\u0441\u044F \u0437\u0432\u0435\u0440\u043D\u0443\u0442\u0438\u0441\u044F \u0434\u043E \u0441\u043B\u0443\u0436\u0431\u0438 \u043E\u0431\u043B\u0456\u043A\u043E\u0432\u0438\u0445 \u0437\u0430\u043F\u0438\u0441\u0456\u0432. \u041F\u043E\u0432\u0442\u043E\u0440\u0456\u0442\u044C \u0441\u043F\u0440\u043E\u0431\u0443 \u043F\u0456\u0437\u043D\u0456\u0448\u0435 \u0430\u0431\u043E \u0437\u0432\u0435\u0440\u043D\u0456\u0442\u044C\u0441\u044F \u0434\u043E \u0430\u0434\u043C\u0456\u043D\u0456\u0441\u0442\u0440\u0430\u0442\u043E\u0440\u0430.";
            default:
                return string.Empty;
        }
    }

    private static PasswordChangeResult ValidateNewPassword(
        MembershipProvider provider,
        string password)
    {
        if (password.Length < provider.MinRequiredPasswordLength)
            return PasswordChangeResult.NewPasswordTooShort;

        if (password.Length > MaximumSqlMembershipPasswordLength)
            return PasswordChangeResult.NewPasswordTooLong;

        int nonAlphanumericCharacters = 0;
        foreach (char character in password)
        {
            if (!char.IsLetterOrDigit(character))
                nonAlphanumericCharacters++;
        }

        if (nonAlphanumericCharacters < provider.MinRequiredNonAlphanumericCharacters)
            return PasswordChangeResult.NewPasswordNeedsNonAlphanumericCharacters;

        string strengthExpression = provider.PasswordStrengthRegularExpression;
        if (string.IsNullOrEmpty(strengthExpression))
            return PasswordChangeResult.Success;

        try
        {
            return Regex.IsMatch(password, strengthExpression)
                ? PasswordChangeResult.Success
                : PasswordChangeResult.NewPasswordDoesNotMatchStrengthExpression;
        }
        catch (ArgumentException)
        {
            return PasswordChangeResult.PasswordPolicyConfigurationError;
        }
    }

    private static int GetMinimumPasswordLength()
    {
        try
        {
            return Membership.MinRequiredPasswordLength;
        }
        catch
        {
            return 1;
        }
    }

    private static int GetMinimumNonAlphanumericCharacters()
    {
        try
        {
            return Membership.MinRequiredNonAlphanumericCharacters;
        }
        catch
        {
            return 1;
        }
    }
}
