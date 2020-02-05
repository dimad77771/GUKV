using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace ExtDataEntry.Models
{
    public class Address
    {
        public const string AddressSuffix = "Київ, Україна";

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
                + (string.IsNullOrWhiteSpace(AddressSuffix) ? "" : ", " + AddressSuffix);
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

        public static Address FromData(DB.GeocodingRow item, bool withDistrict = true)
        {
            return new Address()
            {
                District = !withDistrict || item.IsAddrDistrictNull() ? null : item.AddrDistrict,
                Street = item.AddrStreet,
                Nomer = item.IsAddrNumberNull() ? null : item.AddrNumber,
            };
        }
    }
}
