using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GeocodingService.Properties;

namespace GeocodingService
{
    public class Address
    {
        private string _district;
        private string _street;
        private string _nomer;

        public string District { get { return _district; } set { _district = Normalize(value); } }
        public string Street { get { return _street; } set { _street = Normalize(value); } }
        public string Nomer { get { return _nomer; } set { _nomer = Normalize(value); } }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Street);
            }
        }

        public override string ToString()
        {
            return Street
                + (Nomer == null ? "" : ", " + Nomer)
                + (District == null ? "" : ", " + District)
                + (string.IsNullOrWhiteSpace(Settings.Default.AddressSuffix) ? "" : ", " + Settings.Default.AddressSuffix);
        }

        public override int GetHashCode()
        {
            return (District ?? "<no district>").GetHashCode()
                ^ (Street ?? "<no street>").GetHashCode()
                ^ (Nomer ?? "<no nomer>").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Address other = obj as Address;
            if (object.ReferenceEquals(other, null))
                return base.Equals(obj);

            return this.District == other.District
                && this.Street == other.Street
                && this.Nomer == other.Nomer;
        }

        private static string CollapseWhitespace(string text)
        {
            return Regex.Replace(text, @"\s{2,}", " ");
        }

        private static string Normalize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;
            return CollapseWhitespace(text.Trim()).ToUpper();
        }

        public static explicit operator Address(DB.geocodingRow item)
        {
            return new Address()
            {
                District = item.Isaddr_districtNull() ? null : item.addr_district,
                Street = item.addr_street_full_name,
                Nomer = item.Isaddr_nomerNull() ? null : item.addr_nomer,
            };
        }

        public static explicit operator Address(DB.view_balansRow item)
        {
            return new Address()
            {
                District = item.IsdistrictNull() ? null : item.district,
                Street = item.Isstreet_full_nameNull() ? null : item.street_full_name,
                Nomer = item.Isaddr_nomerNull() ? null : item.addr_nomer,
            };
        }

        public static explicit operator Address(DB.referenced_addressesRow item)
        {
            return new Address()
            {
                District = item.IsdistrictNull() ? null : item.district,
                Street = item.Isstreet_full_nameNull() ? null : item.street_full_name,
                Nomer = item.Isaddr_nomerNull() ? null : item.addr_nomer,
            };
        }
    }
}
