using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using GeocodingService.Properties;
using System.Threading;

namespace GeocodingService
{
    class Program
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(Program));

        private static readonly List<DB.geocodingRow> _queue = new List<DB.geocodingRow>();
        private static readonly object _queueLock = new object();

        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;

                DB data = new DB();

                //LOGGER.Info("Loading all referenced addresses...");
                //(new DBTableAdapters.view_balansTableAdapter()).Fill(data.view_balans);
                //LOGGER.InfoFormat("{0} unique addresses found", data.view_balans.Count);

                LOGGER.Info("Loading all cached geocoding results...");
                (new DBTableAdapters.geocodingTableAdapter()).Fill(data.geocoding);
                LOGGER.InfoFormat("{0} cached geocoding results found", data.geocoding.Count);

                //LOGGER.Info("Loading referenced addresses to boost priority...");
                //(new DBTableAdapters.referenced_addressesTableAdapter()).Fill(data.referenced_addresses);
                //Dictionary<Address, DB.referenced_addressesRow> referencedAddressesLookup = data.referenced_addresses
                //    .Select(x => new { Addr = (Address)x, Item = x })
                //    .Where(x => x.Addr.IsValid)
                //    .Distinct(x => x.Addr)
                //    .ToDictionary(x => x.Addr, x => x.Item);
                //LOGGER.InfoFormat("{0} unique ({1} valid) addresses are referenced across various objects", 
                //    data.referenced_addresses.Count, referencedAddressesLookup.Count);

                //LOGGER.Info("Queuing new addresses for geocoding...");
                //Dictionary<Address, DB.view_balansRow> viewBalansLookup = data.view_balans
                //    .Select(x => new { Addr = (Address)x, Item = x })
                //    .Where(x => x.Addr.IsValid)
                //    .Distinct(x => x.Addr)
                //    .ToDictionary(x => x.Addr, x => x.Item);
                //Dictionary<Address, DB.geocodingRow> geocodingLookup = data.geocoding.ToDictionary(x => (Address)x);

                //foreach (var entry in viewBalansLookup)
                //{
                //    if (geocodingLookup.ContainsKey(entry.Key))
                //        continue;

                //    DB.geocodingRow row = data.geocoding.AddgeocodingRow(
                //        (int)StatusCode.Queued, DateTime.Now,
                //        0, 0,
                //        entry.Key.District,
                //        entry.Key.Street,
                //        entry.Key.Nomer,
                //        null);

                //    row.Setloc_latNull();
                //    row.Setloc_lngNull();

                //    geocodingLookup.Add((Address)row, row);
                //}

                LOGGER.InfoFormat("{0} new addresses queued for geocoding", data.geocoding.Count(x => x.RowState == System.Data.DataRowState.Added));
                LOGGER.Info("Updating the database...");
                (new DBTableAdapters.geocodingTableAdapter()).Update(data.geocoding);
                LOGGER.Info("Done");

                _queue.AddRange(data.geocoding
                    .Where(x => x.status_code == (int)StatusCode.Queued)
                    .OrderBy(x => x.last_action_on));
                    //.OrderBy(x => referencedAddressesLookup.ContainsKey((Address)x) ? 0 : 1)
                    //.ThenBy(x => (!x.Isaddr_districtNull() && x.addr_district == "ШЕВЧЕНКІВСЬКИЙ") ? 0 : 1)
                    //.ThenBy(x => x.last_action_on));

                Geocoding.IGeocoder geocoder = CreateGeocoder();

                while (_queue.Count > 0)
                {
                    DB.geocodingRow row = _queue.First();
                    _queue.RemoveAt(0);

                    DateTime startTime = DateTime.Now;

                    ProcessAddress(geocoder, row);

                    int delay;
                    while ((delay = (int)Math.Round((DateTime.Now - startTime).TotalMilliseconds)) < Settings.Default.GoogleGeocodingRequestRate * 1000)
                    {
                        LOGGER.WarnFormat("Delaying requests for {0}ms to limit the request rate",
                            Math.Max(Settings.Default.GoogleGeocodingRequestRate * 1000 - delay, 150));
                        Thread.Sleep(Math.Max(Settings.Default.GoogleGeocodingRequestRate * 1000 - delay, 150));
                    }
                }
            }
            catch (Exception ex)
            {
                LOGGER.Fatal("General failure", ex);
            }
        }

        private static void ProcessAddress(Geocoding.IGeocoder geocoder, DB.geocodingRow row)
        {
            Address address = (Address)row;

            bool repeat = false;
            do
            {
                if (repeat)
                    LOGGER.InfoFormat("Retrying for {0} | {1} | {2}...", address.District, address.Street, address.Nomer);
                else
                    LOGGER.InfoFormat("Querying for {0} | {1} | {2}...", address.District, address.Street, address.Nomer);
                repeat = false;

                try
                {
                    var matches = geocoder.Geocode(address.ToString()).Where(x => IsValidMatch(x)).ToList();
                    if (matches.Count == 0)
                    {
                        LOGGER.Info("No matching address(es) found");
                        row.status_code = (int)StatusCode.NoMatch;
                    }
                    else if (matches.Count > 1)
                    {
                        LOGGER.InfoFormat("{0} ambiguous matching address(es) found", matches.Count);
                        row.status_code = (int)StatusCode.Ambiguous;
                        foreach (var match in matches)
                        {
                            LOGGER.InfoFormat("Ambiguous match: {0} at {1}, {2}", match.FormattedAddress, 
                                match.Coordinates.Longitude, match.Coordinates.Latitude);

                            ((DB)row.Table.DataSet).geocoding_matches
                                .Addgeocoding_matchesRow(row, match.FormattedAddress, match.Coordinates.Longitude, match.Coordinates.Latitude);
                        }
                    }
                    else
                    {
                        var match = matches.First();

                        LOGGER.InfoFormat("Exact match: {0} at {1}, {2}", match.FormattedAddress,
                            match.Coordinates.Longitude, match.Coordinates.Latitude);

                        row.status_code = (int)StatusCode.Success;
                        row.loc_lng = match.Coordinates.Longitude;
                        row.loc_lat = match.Coordinates.Latitude;

                        ((DB)row.Table.DataSet).geocoding_matches
                            .Addgeocoding_matchesRow(row, match.FormattedAddress, match.Coordinates.Longitude, match.Coordinates.Latitude);
                    }
                }
                catch (Geocoding.Google.GoogleGeocodingException ex)
                {
                    switch (ex.Status)
                    {
                        case Geocoding.Google.GoogleStatus.Ok:
                            throw new Exception("Unexpected response", ex);

                        case Geocoding.Google.GoogleStatus.Error:
                        case Geocoding.Google.GoogleStatus.InvalidRequest:
                            row.status_code = (int)StatusCode.Failure;
                            row.status_message = ex.ToString();
                            LOGGER.Warn("Query failure", ex);
                            break;

                        case Geocoding.Google.GoogleStatus.OverQueryLimit:
                            LOGGER.InfoFormat("Quota overdrawn. Sleeping for {0}...", Settings.Default.QuotaOverdraftSleep);
                            Thread.Sleep(Settings.Default.QuotaOverdraftSleep);
                            repeat = true;
                            break;

                        case Geocoding.Google.GoogleStatus.RequestDenied:
                            throw new Exception("Authorization problem", ex);

                        case Geocoding.Google.GoogleStatus.ZeroResults:
                            LOGGER.Info("No matching address(es) found");
                            row.status_code = (int)StatusCode.NoMatch;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    row.status_code = (int)StatusCode.Failure;
                    row.status_message = ex.ToString();
                    LOGGER.Warn("Query failure", ex);
                }

                row.last_action_on = DateTime.Now;
                (new DBTableAdapters.geocodingTableAdapter()).Update(row);
                (new DBTableAdapters.geocoding_matchesTableAdapter()).Update(row.Getgeocoding_matchesRows());
            } while (repeat);
        }

        private static Geocoding.IGeocoder CreateGeocoder()
        {
            Geocoding.Google.GoogleGeocoder geocoder = new Geocoding.Google.GoogleGeocoder(Settings.Default.GoogleApiKey);
            if (!string.IsNullOrWhiteSpace(Settings.Default.GoogleGeocodingLanguage))
                geocoder.Language = Settings.Default.GoogleGeocodingLanguage;
            return geocoder;
        }

        private static bool IsValidMatch(Geocoding.Address match)
        {
            if (match.FormattedAddress.Equals(Settings.Default.AddressSuffix, StringComparison.OrdinalIgnoreCase))
                return false;
            return true;
        }
    }
}
