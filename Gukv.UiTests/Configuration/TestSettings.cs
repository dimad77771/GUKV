using System.Globalization;

namespace Gukv.UiTests.Configuration;

public sealed class TestSettings
{
    public const string WriteConfirmation = "YES_I_AM_USING_A_TEST_DATABASE";

    public Uri BaseUri { get; } = ReadBaseUri();
    public string? Username { get; } = Read("GUKV_E2E_USERNAME");
    public string? Password { get; } = Read("GUKV_E2E_PASSWORD");
    public string? SqlConnectionString { get; } = Read("GUKV_E2E_SQL_CONNECTION");
    public string? ExpectedDatabase { get; } = Read("GUKV_E2E_EXPECT_DATABASE");
    public string WebConfigPath { get; } = ReadWebConfigPath();
    public bool RestoreAfterWrite { get; } = ReadBoolean("GUKV_E2E_RESTORE_AFTER_WRITE", true);
    public int TimeoutMilliseconds { get; } = ReadInteger("GUKV_E2E_TIMEOUT_MS") ?? 60_000;

    public int? ReportId => ReadInteger("GUKV_E2E_REPORT_ID");
    public int? BalansId => ReadInteger("GUKV_E2E_BALANS_ID");
    public int? GroupStreetId => ReadInteger("GUKV_E2E_GROUP_STREET_ID");
    public int? GroupRepresentativeId => ReadInteger("GUKV_E2E_GROUP_REPRESENTATIVE_ID");
    public string? GroupDisplay => Read("GUKV_E2E_GROUP_DISPLAY");
    public IReadOnlyList<int> GroupBalansIds => ReadIntegerList("GUKV_E2E_GROUP_BALANS_IDS");
    public int? WriteTargetBuildingId => ReadInteger("GUKV_E2E_WRITE_TARGET_BUILDING_ID");
    public int? GuardBalansId => ReadInteger("GUKV_E2E_GUARD_BALANS_ID");
    public bool WritesEnabled => string.Equals(
        Read("GUKV_E2E_ALLOW_WRITES"), WriteConfirmation, StringComparison.Ordinal);

    public (string Username, string Password) RequireCredentials()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrEmpty(Password))
        {
            Assert.Ignore("Set GUKV_E2E_USERNAME and GUKV_E2E_PASSWORD to run authenticated UI tests.");
        }

        return (Username!, Password!);
    }

    public (int ReportId, int BalansId) RequireObject()
    {
        if (ReportId is not > 0 || BalansId is not > 0)
        {
            Assert.Ignore("Set positive GUKV_E2E_REPORT_ID and GUKV_E2E_BALANS_ID values.");
        }

        return (ReportId.Value, BalansId.Value);
    }

    public GroupScenario RequireGroupScenario()
    {
        if (GroupStreetId is not > 0 || GroupRepresentativeId is not > 0 ||
            string.IsNullOrWhiteSpace(GroupDisplay) || GroupBalansIds.Count == 0)
        {
            Assert.Ignore(
                "Set GUKV_E2E_GROUP_STREET_ID, GUKV_E2E_GROUP_REPRESENTATIVE_ID, " +
                "GUKV_E2E_GROUP_DISPLAY and the complete GUKV_E2E_GROUP_BALANS_IDS list.");
        }

        return new GroupScenario(
            GroupStreetId.Value,
            GroupRepresentativeId.Value,
            GroupDisplay!,
            GroupBalansIds);
    }

    public WriteScenario RequireWriteScenario()
    {
        if (!WritesEnabled)
        {
            Assert.Ignore(
                $"Write test is disabled. Set GUKV_E2E_ALLOW_WRITES={WriteConfirmation} only for a disposable test database.");
        }

        (int reportId, int balansId) = RequireObject();

        if (WriteTargetBuildingId is not > 0 || string.IsNullOrWhiteSpace(SqlConnectionString) ||
            string.IsNullOrWhiteSpace(ExpectedDatabase))
        {
            Assert.Fail(
                "Write test requires GUKV_E2E_WRITE_TARGET_BUILDING_ID, GUKV_E2E_SQL_CONNECTION and GUKV_E2E_EXPECT_DATABASE.");
        }

        int targetBuildingId = WriteTargetBuildingId
            ?? throw new InvalidOperationException("Write target building ID was not configured.");

        return new WriteScenario(
            reportId,
            balansId,
            targetBuildingId,
            GuardBalansId,
            SqlConnectionString!,
            ExpectedDatabase!,
            RestoreAfterWrite);
    }

    private static Uri ReadBaseUri()
    {
        string value = Read("GUKV_E2E_BASE_URL") ?? "http://localhost:6670/";
        if (!Uri.TryCreate(value, UriKind.Absolute, out Uri? uri))
        {
            throw new InvalidOperationException("GUKV_E2E_BASE_URL must be an absolute URL.");
        }

        return uri.AbsoluteUri.EndsWith("/", StringComparison.Ordinal)
            ? uri
            : new Uri(uri.AbsoluteUri + "/", UriKind.Absolute);
    }

    private static string ReadWebConfigPath()
    {
        string? configuredPath = Read("GUKV_E2E_WEB_CONFIG");
        if (configuredPath is not null)
        {
            return Path.GetFullPath(configuredPath);
        }

        return Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "..",
            "ReportWebSite",
            "Web.config"));
    }

    private static string? Read(string name)
    {
        string? value = Environment.GetEnvironmentVariable(name);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private static int? ReadInteger(string name)
    {
        string? value = Read(name);
        if (value is null)
        {
            return null;
        }

        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
        {
            throw new InvalidOperationException($"{name} must be an integer.");
        }

        return result;
    }

    private static IReadOnlyList<int> ReadIntegerList(string name)
    {
        string? value = Read(name);
        if (value is null)
        {
            return Array.Empty<int>();
        }

        return value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(part =>
            {
                if (!int.TryParse(part.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
                {
                    throw new InvalidOperationException($"{name} contains a non-integer value: {part}.");
                }

                return result;
            })
            .ToArray();
    }

    private static bool ReadBoolean(string name, bool defaultValue)
    {
        string? value = Read(name);
        return value is null ? defaultValue : bool.Parse(value);
    }
}

public sealed record GroupScenario(
    int StreetId,
    int RepresentativeBuildingId,
    string DisplayNumber,
    IReadOnlyList<int> ExpectedBalansIds);

public sealed record WriteScenario(
    int ReportId,
    int BalansId,
    int TargetBuildingId,
    int? GuardBalansId,
    string SqlConnectionString,
    string ExpectedDatabase,
    bool RestoreAfterWrite);
