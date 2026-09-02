using System;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;

/// <summary>
/// Optional emergency authentication using the master_password appSetting.
/// A master password never creates an identity: the requested membership user
/// must already exist. Missing or empty configuration keeps legacy behaviour.
/// </summary>
public static class MasterPasswordAuthentication
{
    private const string MasterPasswordKey = "master_password";

    public static bool IsMasterPassword(string password)
    {
        string masterPassword = WebConfigurationManager.AppSettings[MasterPasswordKey];

        return !string.IsNullOrEmpty(masterPassword) &&
            !string.IsNullOrEmpty(password) &&
            FixedTimeEquals(masterPassword, password);
    }

    public static bool IsValidForExistingUser(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || !IsMasterPassword(password))
        {
            return false;
        }

        return Membership.GetUser(username, false) != null;
    }

    private static bool FixedTimeEquals(string expected, string actual)
    {
        byte[] expectedBytes = Encoding.UTF8.GetBytes(expected ?? string.Empty);
        byte[] actualBytes = Encoding.UTF8.GetBytes(actual ?? string.Empty);
        int difference = expectedBytes.Length ^ actualBytes.Length;
        int maximumLength = Math.Max(expectedBytes.Length, actualBytes.Length);

        for (int index = 0; index < maximumLength; index++)
        {
            byte expectedByte = index < expectedBytes.Length ? expectedBytes[index] : (byte)0;
            byte actualByte = index < actualBytes.Length ? actualBytes[index] : (byte)0;
            difference |= expectedByte ^ actualByte;
        }

        return difference == 0;
    }
}
