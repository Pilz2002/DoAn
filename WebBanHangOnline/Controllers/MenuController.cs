using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuTop()
        {
            var items = _db.Categories.OrderBy(x => x.Position).Where(x=>x.IsActive == true).ToList();
            return PartialView("_MenuTop",items);
        }

        public ActionResult MenuProductCategory()
        {
            var items = _db.ProductCategories.ToList();
			return PartialView("_MenuProductCategory", items);
		}

		public ActionResult MenuArrivals()
		{
			var items = _db.ProductCategories.ToList();
			return PartialView("_MenuArrivals", items);
		}

		public ActionResult MenuLeft(int? id)
		{
            if(id != null )
            {
                ViewBag.CateId = id;
            }
			var items = _db.ProductCategories.ToList();
			return PartialView("_MenuLeft", items);
		}

	}
}