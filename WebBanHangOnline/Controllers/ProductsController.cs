using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
	public class ProductsController : Controller
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();
		// GET: Products
		public ActionResult Index()
		{
			var items = _db.Products.ToList();
			return View(items);
		}

		public ActionResult ProductsCategory(string alias, int id)
		{
			var items = _db.Products.Where(x => x.ProductCategory.Id == id && x.ProductCategory.Alias == alias).ToList();
			var cate = _db.ProductCategories.FirstOrDefault(x => x.Id == id);
			if (cate != null)
			{
				ViewBag.CateName = cate.Title;
			}
			ViewBag.CateId = id;
			return View(items);
		}

		public ActionResult Partial_Items()
		{
			var items = _db.Products.Where(x => x.IsHome && x.IsActive == true).ToList();
			return PartialView(items);
		}

		public ActionResult Partial_ProductsSales()
		{
			var items = _db.Products.Where(x => x.IsSale && x.IsActive == true).ToList();
			return PartialView(items);
		}

		public ActionResult Detail(int id)
		{
			var item = _db.Products.Find(id);
			if (item != null)
			{
				_db.Products.Attach(item);
				item.ViewCount += 1;
				_db.Entry(item).Property(x => x.ViewCount).IsModified = true;
				_db.SaveChanges();
			}
			return View(item);
		}

		
	}
}