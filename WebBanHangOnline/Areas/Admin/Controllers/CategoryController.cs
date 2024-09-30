using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin,Employee")]
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();
		// GET: Admin/Category
		public ActionResult Index()
		{
			var items = _db.Categories.OrderByDescending(x => x.Id).ToList();
			return View(items);
		}

		public ActionResult Add()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(Category model)
		{
			if (ModelState.IsValid)
			{
				model.CreatedDate = DateTime.Now;
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
				_db.Categories.Add(model);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}

		public ActionResult Edit(int Id)
		{
			var item = _db.Categories.Find(Id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Category model)
		{
			if (ModelState.IsValid)
			{
				_db.Categories.Attach(model);
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
				_db.Entry(model).State = System.Data.Entity.EntityState.Modified;
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int Id)
		{
			var item = _db.Categories.Find(Id);
			if(item != null)
			{
				_db.Categories.Remove(item);
				_db.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		public ActionResult IsActive(int Id)
		{
			var item = _db.Categories.Find(Id);
			if (item != null)
			{
				item.IsActive = !item.IsActive;
				_db.Entry(item).State = System.Data.Entity.EntityState.Modified;
				_db.SaveChanges();
				return Json(new { success = true, isActive = item.IsActive });
			}
			return Json(new { success = false, isActive = item.IsActive });
		}
	}
}