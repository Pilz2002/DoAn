using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
		private readonly ApplicationDbContext _db = new ApplicationDbContext();
		// GET: Admin/ProductCategory
		public ActionResult Index()
        {
            var items = _db.ProductCategories.ToList();
            return View(items);
        }

		public ActionResult Add()
		{
			return View();
		}

		[HttpPost]
        public ActionResult Add(ProductCategory model)
        {
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _db.ProductCategories.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
			return View();
		}

		public ActionResult Edit(int Id)
		{
			var item = _db.ProductCategories.Find(Id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ProductCategory model)
		{
			if (ModelState.IsValid)
			{
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
				_db.ProductCategories.Attach(model);
				_db.Entry(model).State = System.Data.Entity.EntityState.Modified;
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int Id)
		{
			var item = _db.ProductCategories.Find(Id);
			if (item != null)
			{
				_db.ProductCategories.Remove(item);
				_db.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

	}
}
