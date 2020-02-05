using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalWebSite.Common;

namespace PortalWebSite.Models
{
    public class Obj : PropertyCopier<Obj>
    {
        public string org_full_name { get; set; }
        public string org_short_name { get; set; }
        public string district { get; set; }
        public string street_full_name { get; set; }
        public string addr_nomer { get; set; }
        public string addr_misc { get; set; }
        public decimal? sqr_total { get; set; }
        public decimal? sqr_free { get; set; }
        public decimal? sqr_in_rent { get; set; }
        public decimal? sqr_not_for_rent { get; set; }
        public string object_kind { get; set; }
        public string object_type { get; set; }
        public string condition { get; set; }
        public string floors { get; set; }
        public string purpose_group { get; set; }

        public static explicit operator Obj(view_balans_all item) { return FromItem(item); }
    }

    public class Loc : PropertyCopier<Loc>
    {
        public double? loc_lat { get; set; }
        public double? loc_lng { get; set; }

        public static explicit operator Loc(geocoding item) { return FromItem(item); }
    }

    public class OneObjectModel
    {
        public Obj Obj { get; set; }
        public Loc Loc { get; set; }
    }

    public class ObjectsModel
    {
        public List<OneObjectModel> Objects { get; set; }
    }
}
