using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml;
using System.Globalization;

public static class GeocodingAPI
{
    public sealed class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public static Location Resolve(string address)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                string responseXml = client.DownloadString(
                    string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false",
                        Uri.EscapeUriString(address))
                    );
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseXml);
                XmlNode statusNode = doc.SelectSingleNode("//GeocodeResponse/status");
                if (statusNode == null || statusNode.FirstChild.Value != "OK")
                    return null;

                XmlNode locationNode = doc.SelectSingleNode("//GeocodeResponse/result/geometry/location");
                if (locationNode == null)
                    return null;

                return new Location()
                    {
                        Lat = double.Parse(locationNode.SelectSingleNode("lat").FirstChild.Value, NumberFormatInfo.InvariantInfo),
                        Lng = double.Parse(locationNode.SelectSingleNode("lng").FirstChild.Value, NumberFormatInfo.InvariantInfo),
                    };
            }
        }
        catch
        {
            return null;
        }
    }
}