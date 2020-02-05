using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;


public class BalansEntry
{
    public int BalansID { get; set; }
    public int BuildingID { get; set; }
    public int OrgID { get; set; }
    public decimal SqrTotal { get; set; }
    public bool IsDeleted { get; set; }
    public string InvNumber { get; set; }
}

/// <summary>
/// Summary description for BalansFinder
/// </summary>
public class BalansFinder
{


    private readonly Dictionary<int, List<BalansEntry>> _entries;

    public BalansFinder(SqlConnection conn)
    {
        _entries = ReadEntries(conn)
            .GroupBy(x => x.BuildingID)
            .ToDictionary(x => x.Key, x => x.ToList());
    }

    private static IEnumerable<BalansEntry> ReadEntries(SqlConnection conn)
    {
        using (SqlCommand command = conn.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 600;
            command.CommandText = "SELECT id, building_id, organization_id, sqr_total, ISNULL(is_deleted, 0) is_deleted, reestr_no FROM balans WHERE building_id IS NOT NULL AND organization_id IS NOT NULL AND sqr_total IS NOT NULL";

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return new BalansEntry()
                    {
                        BalansID = (int)reader["id"],
                        BuildingID = (int)reader["building_id"],
                        OrgID = (int)reader["organization_id"],
                        SqrTotal = (decimal)reader["sqr_total"],
                        IsDeleted = Convert.ToInt32(reader["is_deleted"]) != 0,
                        InvNumber = (reader.IsDBNull(reader.GetOrdinal("reestr_no")) ? null : (string)reader["reestr_no"]),
                    };
                }
            }
        }
    }

    public BalansEntry FindBalansObject(List<int> buildingID, int organizationID, decimal sqrTotal, string invNumber,
        bool includeDeleted = false)
    {
        List<BalansEntry> balansInBuilding = new List<BalansEntry>();

        foreach (int i in buildingID)
        {
            List<BalansEntry> curBalansInBuilding;
            if (_entries.TryGetValue(i, out curBalansInBuilding))
                balansInBuilding.AddRange(curBalansInBuilding);
        }

        if (balansInBuilding.Count == 0)
            return null;

        // Use inventory number to disambiguate between multiple balans objects with the same area.

        BalansEntry balans = balansInBuilding
            .FirstOrDefault(x => x.OrgID == organizationID && x.SqrTotal == sqrTotal && x.InvNumber == invNumber && ((includeDeleted) || (x.IsDeleted == false)));
        if (balans != null)
            return balans;

        balans = balansInBuilding
            .FirstOrDefault(x => x.OrgID == organizationID && Math.Abs(x.SqrTotal - sqrTotal) < 0.03m && x.InvNumber == invNumber && ((includeDeleted) || (x.IsDeleted == false)));
        if (balans != null)
            return balans;

        balans = balansInBuilding
            .FirstOrDefault(x => x.OrgID == organizationID && x.SqrTotal == sqrTotal && ((includeDeleted) || (x.IsDeleted == false)));
        if (balans != null)
            return balans;

        balans = balansInBuilding
            .FirstOrDefault(x => x.OrgID == organizationID && Math.Abs(x.SqrTotal - sqrTotal) < 0.03m && ((includeDeleted) || (x.IsDeleted == false)));

        if (balans != null)
            return balans;

        return null;
    }
}