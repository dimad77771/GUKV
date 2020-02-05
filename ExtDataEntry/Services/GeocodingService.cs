using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExtDataEntry.Models;

namespace ExtDataEntry.Services
{
    public class GeocodingService
    {
        public static int? FindOrCreateGeocoding(string district, string address, string number)
        {
            if (string.IsNullOrWhiteSpace(address))
                return null;

            DB data = new DB();
            (new Models.DBTableAdapters.GeocodingTableAdapter()).Fill(data.Geocoding);

            DB.GeocodingRow entry;

            Dictionary<Address, DB.GeocodingRow> lookupWithDistrict = data.Geocoding
                .GroupBy(x => Address.FromData(x, true))
                .ToDictionary(x => x.Key, x => x.First());
            if (!lookupWithDistrict.TryGetValue(new Address() { District = district, Street = address, Nomer = number }, out entry))
            {
                Dictionary<Address, DB.GeocodingRow> lookupWithoutDistrict = data.Geocoding
                    .GroupBy(x => Address.FromData(x, false))
                    .ToDictionary(x => x.Key, x => x.First());
                if (!lookupWithoutDistrict.TryGetValue(new Address() { Street = address, Nomer = number }, out entry))
                {
                    entry = data.Geocoding.AddGeocodingRow(district, address, number, 0, null, DateTime.Now);
                    (new Models.DBTableAdapters.GeocodingTableAdapter()).Update(data);
                }
            }

            return entry.Id;
        }
    }
}
