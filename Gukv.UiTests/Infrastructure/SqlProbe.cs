using Microsoft.Data.SqlClient;
using System.Xml.Linq;

namespace Gukv.UiTests.Infrastructure;

public sealed class SqlProbe(string connectionString)
{
    public async Task<SqlTarget> GetTargetAsync()
    {
        await using SqlConnection connection = await OpenAsync();
        await using SqlCommand command = new(
            "SELECT CONVERT(nvarchar(128), SERVERPROPERTY('ServerName')), DB_NAME()",
            connection);
        await using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            throw new InvalidOperationException("SQL target query returned no row.");
        }

        return new SqlTarget(reader.GetString(0), reader.GetString(1));
    }

    public async Task<string> GetDatabaseNameAsync()
    {
        await using SqlConnection connection = await OpenAsync();
        await using SqlCommand command = new("SELECT DB_NAME()", connection);
        return Convert.ToString(await command.ExecuteScalarAsync()) ?? string.Empty;
    }

    public async Task<BalansLinkState> GetBalansLinkStateAsync(int reportId, int balansId)
    {
        const string query = @"
SELECT
    reportBalans.building_id,
    reportBalans.building_1nf_unique_id,
    reportBuilding.id,
    centralBalans.building_id,
    reportBalans.submit_date
FROM reports1nf_balans reportBalans
LEFT JOIN reports1nf_buildings reportBuilding
    ON reportBuilding.unique_id = reportBalans.building_1nf_unique_id
   AND reportBuilding.report_id = reportBalans.report_id
LEFT JOIN balans centralBalans ON centralBalans.id = reportBalans.id
WHERE reportBalans.report_id = @reportId AND reportBalans.id = @balansId";

        await using SqlConnection connection = await OpenAsync();
        await using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("reportId", reportId);
        command.Parameters.AddWithValue("balansId", balansId);

        await using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            throw new InvalidOperationException(
                $"reports1nf_balans row was not found for report {reportId}, balans {balansId}.");
        }

        return new BalansLinkState(
            ReadNullableInt32(reader, 0),
            ReadNullableInt32(reader, 1),
            ReadNullableInt32(reader, 2),
            ReadNullableInt32(reader, 3),
            reader.IsDBNull(4) ? null : reader.GetDateTime(4));
    }

    public async Task<VisualAddress> GetBuildingAddressAsync(int buildingId)
    {
        const string query = @"
SELECT
    COALESCE(district.name, ''),
    COALESCE(street.name, building.addr_street_name, ''),
    COALESCE(building.addr_nomer1, ''),
    COALESCE(building.addr_nomer2, ''),
    COALESCE(building.addr_nomer3, '')
FROM buildings building
LEFT JOIN dict_streets street ON street.id = building.addr_street_id
LEFT JOIN dict_districts2 district ON district.id = building.addr_distr_new_id
WHERE building.id = @buildingId";

        await using SqlConnection connection = await OpenAsync();
        await using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("buildingId", buildingId);

        await using SqlDataReader reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            throw new InvalidOperationException($"Building {buildingId} was not found.");
        }

        return new VisualAddress(
            reader.GetString(0),
            reader.GetString(1),
            VisualAddress.JoinNumber(reader.GetString(2), reader.GetString(3), reader.GetString(4)));
    }

    private async Task<SqlConnection> OpenAsync()
    {
        SqlConnection connection = new(connectionString);
        await connection.OpenAsync();
        return connection;
    }

    private static int? ReadNullableInt32(SqlDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
    }
}

public static class WebConfigProbe
{
    public static string ReadGukvConnectionString(string webConfigPath)
    {
        if (!File.Exists(webConfigPath))
        {
            throw new FileNotFoundException(
                "The local Web.config was not found. Set GUKV_E2E_WEB_CONFIG explicitly.",
                webConfigPath);
        }

        XDocument document = XDocument.Load(webConfigPath);
        XElement? entry = document
            .Descendants("connectionStrings")
            .Elements("add")
            .SingleOrDefault(element => string.Equals(
                (string?)element.Attribute("name"),
                "GUKVConnectionString",
                StringComparison.Ordinal));

        string? connectionString = (string?)entry?.Attribute("connectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"GUKVConnectionString was not found in {webConfigPath}.");
        }

        return connectionString;
    }
}

public sealed record SqlTarget(string ServerName, string DatabaseName);

public sealed record BalansLinkState(
    int? ReportBuildingId,
    int? ReportBuildingUniqueId,
    int? SnapshotBuildingId,
    int? CentralBuildingId,
    DateTime? SubmitDate);
