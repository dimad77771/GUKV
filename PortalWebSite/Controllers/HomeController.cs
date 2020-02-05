using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PortalWebSite.Common;
using PortalWebSite.Models;

namespace PortalWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly GUKVEntities _context;

        public HomeController(GUKVEntities context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Objects()
        {
            int[] ownership_int_filter = new int[] { 32, 33, 34 };

            List<view_balans_all> objects = _context.view_balans_all
                .Where(x => (x.org_ownership_int != null && ownership_int_filter.Contains((int)x.org_ownership_int))
                    || (x.form_ownership_int != null && ownership_int_filter.Contains((int)x.form_ownership_int)))
                .Where(x => (x.is_deleted ?? 0) == 0 || x.is_not_accepted == 1)
                //.Where(x => (x.sqr_free ?? 0m) - (x.sqr_not_for_rent ?? 0m) > 1 || (x.sqr_in_rent ?? 0m) > 0)
                .Where(x => (x.sqr_free ?? 0m) - (x.sqr_not_for_rent ?? 0m) > 1)
                .ToList();

            List<geocoding> geocoding = _context.geocodings
                .Where(x => x.status_code == 1)
                .ToList();
            Dictionary<Address, geocoding> geocodingLookup = geocoding
                .Select(x => new { Addr = (Address)x, Item = x })
                .Where(x => x.Addr.IsValid)
                .ToDictionary(x => x.Addr, x => x.Item);


            ObjectsModel model = new ObjectsModel();

            model.Objects = objects
                .Select(x => new { Addr = (Address)x, Item = x })
                .Where(x => x.Addr.IsValid)
                .Select(x => new OneObjectModel
                {
                    Obj = (Obj)x.Item,
                    Loc = (Loc)geocodingLookup.GetValue(x.Addr)
                })
                .Where(x => x.Loc != null)
                .ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
