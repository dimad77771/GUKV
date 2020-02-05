using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace PortalWebSite.Common
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

        public static explicit operator Address(geocoding item)
        {
            return new Address()
            {
                District = item.addr_district,
                Street = item.addr_street_full_name,
                Nomer = item.addr_nomer,
            };
        }

        public static explicit operator Address(view_balans_all item)
        {
            return new Address()
            {
                District = item.district,
                Street = item.street_full_name,
                Nomer = item.addr_nomer,
            };
        }
    }
}
