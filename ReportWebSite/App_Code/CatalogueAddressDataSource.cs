using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GUKV
{
    /// <summary>
    /// Request-local data source for the address catalogue. The database remains untouched:
    /// equivalent building numbers are collapsed only in the returned table.
    /// </summary>
    public static class CatalogueAddressDataSource
    {
        private const string SelectSql = @"
SELECT view_buildings.*,
    STUFF(
    (
        SELECT '<br/>' + CONCAT(org_holder.full_name, ': ', A.sqr_total)
        FROM balans A
        LEFT OUTER JOIN view_organizations org_holder ON A.organization_id = org_holder.organization_id
        LEFT OUTER JOIN view_buildings b ON A.building_id = b.building_id
        OUTER APPLY
        (
            SELECT TOP 1 bal2.id
            FROM balans bal2
            INNER JOIN buildings b2 ON b2.id = bal2.building_id
            WHERE b2.addr_street_id = b.addr_street_id
                AND b2.addr_nomer1 = b.addr_nomer1
                AND bal2.id + 10 != A.id + 10
                AND bal2.sqr_total = A.sqr_total
                AND ISNULL(bal2.is_deleted, 0) = 0
        ) twin
        WHERE A.building_id = view_buildings.building_id
            AND (0 = 1 OR (0 = 0 AND
                (is_deleted IS NULL OR is_deleted = 0 OR
                    CASE WHEN twin.id IS NULL AND A.is_deleted > 0
                        AND A.modified_by = 'Auto-import' THEN 1 ELSE 0 END = 1)))
        ORDER BY 1
        FOR XML PATH(''), TYPE
    ).value('text()[1]', 'nvarchar(max)'), 1, 5, '') AS balans_info
FROM view_buildings
WHERE (building_deleted IS NULL OR building_deleted = 0)
    AND LEN(COALESCE(street_full_name, '')) > 0";

        public static DataTable SelectBuildings()
        {
            DataTable source = new DataTable();

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            using (SqlCommand command = new SqlCommand(SelectSql, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    source.Load(reader);
                }
            }

            return GroupEquivalentAddresses(source);
        }

        public static int UpdateBuildingNumber(int building_id,
            string addr_nomer1, string addr_nomer2, string addr_nomer3)
        {
            const string updateSql = @"
UPDATE buildings
SET addr_nomer1 = @addr_nomer1,
    addr_nomer2 = @addr_nomer2,
    addr_nomer3 = @addr_nomer3
WHERE id = @building_id;

UPDATE reports1nf_buildings
SET addr_nomer1 = @addr_nomer1,
    addr_nomer2 = @addr_nomer2,
    addr_nomer3 = @addr_nomer3
WHERE id = @building_id;";

            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            using (SqlCommand command = new SqlCommand(updateSql, connection))
            {
                command.Parameters.Add("@building_id", SqlDbType.Int).Value = building_id;
                AddNumberParameter(command, "@addr_nomer1", addr_nomer1);
                AddNumberParameter(command, "@addr_nomer2", addr_nomer2);
                AddNumberParameter(command, "@addr_nomer3", addr_nomer3);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public static string NormalizeNumberForSearch(string value)
        {
            return string.Join(" ", AddressNumberGrouping.Tokenize(value)
                .Select(token => token.Value)
                .ToArray());
        }

        private static DataTable GroupEquivalentAddresses(DataTable source)
        {
            DataTable result = CreateWritableResultTable(source);

            if (!result.Columns.Contains("addr_nomer_normalized"))
            {
                result.Columns.Add("addr_nomer_normalized", typeof(string));
            }

            IEnumerable<DataRow> sourceRows = source.Rows.Cast<DataRow>();

            foreach (IGrouping<string, DataRow> streetRows in sourceRows.GroupBy(
                GetStreetScopeKey, StringComparer.Ordinal))
            {
                List<DataRow> rows = streetRows.ToList();
                Dictionary<int, DataRow> rowsByBuildingId = rows.ToDictionary(GetBuildingId);

                IList<AddressNumberGroup> numberGroups = AddressNumberGrouping.Group(rows.Select(row =>
                    new AddressNumberCandidate(
                        GetBuildingId(row),
                        GetText(row, "addr_nomer1"),
                        GetText(row, "addr_nomer2"),
                        GetText(row, "addr_nomer3"))));

                foreach (AddressNumberGroup numberGroup in numberGroups)
                {
                    DataRow representative = rowsByBuildingId[numberGroup.RepresentativeBuildingId];
                    DataRow groupedRow = result.NewRow();

                    foreach (DataColumn column in source.Columns)
                    {
                        groupedRow[column.ColumnName] = representative[column.ColumnName];
                    }

                    groupedRow["addr_nomer"] = numberGroup.DisplayNumber;
                    groupedRow["addr_nomer_normalized"] =
                        NormalizeNumberForSearch(numberGroup.DisplayNumber);
                    groupedRow["balans_info"] = MergeBalansInformation(
                        numberGroup.BuildingIds.Select(buildingId => rowsByBuildingId[buildingId]));

                    result.Rows.Add(groupedRow);
                }
            }

            return result;
        }

        private static DataTable CreateWritableResultTable(DataTable source)
        {
            DataTable result = new DataTable(source.TableName)
            {
                CaseSensitive = source.CaseSensitive,
                Locale = source.Locale
            };

            // Do not clone DataColumn.ReadOnly/Expression flags reported for SQL view
            // and SELECT-expression fields. This detached table intentionally replaces
            // addr_nomer and balans_info with their grouped presentation values.
            foreach (DataColumn sourceColumn in source.Columns)
            {
                result.Columns.Add(sourceColumn.ColumnName, sourceColumn.DataType);
            }

            return result;
        }

        private static string GetStreetScopeKey(DataRow row)
        {
            if (!row.IsNull("addr_street_id"))
            {
                return "STREET:" + Convert.ToString(row["addr_street_id"]);
            }

            // With no dictionary street ID there is no safe proof that two rows refer to
            // the same street, so such rows deliberately remain independent.
            return "BUILDING:" + GetBuildingId(row).ToString();
        }

        private static int GetBuildingId(DataRow row)
        {
            return Convert.ToInt32(row["building_id"]);
        }

        private static string GetText(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? string.Empty : Convert.ToString(row[columnName]);
        }

        private static string MergeBalansInformation(IEnumerable<DataRow> rows)
        {
            List<string> items = new List<string>();
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (DataRow row in rows.OrderBy(GetBuildingId))
            {
                string value = GetText(row, "balans_info");
                string[] parts = value.Split(
                    new[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string part in parts)
                {
                    string item = part.Trim();
                    if (item.Length > 0 && seen.Add(item))
                    {
                        items.Add(item);
                    }
                }
            }

            return string.Join("<br/>", items
                .OrderBy(item => item, StringComparer.OrdinalIgnoreCase)
                .ToArray());
        }

        private static void AddNumberParameter(SqlCommand command, string name, string value)
        {
            SqlParameter parameter = command.Parameters.Add(name, SqlDbType.VarChar, 32);
            parameter.Value = string.IsNullOrEmpty(value) ? (object)DBNull.Value : value;
        }

        private static string GetConnectionString()
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings["GUKVConnectionString"];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new InvalidOperationException(
                    "Connection string GUKVConnectionString is not configured.");
            }

            return settings.ConnectionString;
        }
    }
}
