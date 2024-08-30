﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
		private readonly ApplicationDbContext _db = new ApplicationDbContext();
		// GET: Admin/Post
		public ActionResult Index()
        {
			var items = _db.Posts.OrderByDescending(x => x.Id).ToList();
			return View(items);
		}

		public ActionResult Add()
		{
			var categories = _db.Categories.ToList();
			ViewBag.CategoryList = categories.Select(c => new SelectListItem
			{
				Value = c.Id.ToString(),
				Text = c.Title
			}).ToList();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(Post model)
		{
			if (ModelState.IsValid)
			{
				model.CreatedDate = DateTime.Now;
				model.ModifiedDate = DateTime.Now;
				model.IsActive = true;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
				_db.Posts.Add(model);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}

		public ActionResult Edit(int Id)
		{
			var item = _db.Posts.Find(Id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Post model)
		{
			if (ModelState.IsValid)
			{
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
				_db.Posts.Attach(model);
				_db.Entry(model).State = System.Data.Entity.EntityState.Modified;
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int Id)
		{
			var item = _db.Posts.Find(Id);
			if (item != null)
			{
				_db.Posts.Remove(item);
				_db.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		public ActionResult IsActive(int Id)
		{
			var item = _db.Posts.Find(Id);
			if (item != null)
			{
				item.IsActive = !item.IsActive;
				_db.Entry(item).State = System.Data.Entity.EntityState.Modified;
				_db.SaveChanges();
				return Json(new { success = true, isActive = item.IsActive });
			}
			return Json(new { success = false, isActive = item.IsActive });
		}

		[HttpPost]
		public ActionResult DeleteAll(string ids)
		{
			if (!string.IsNullOrEmpty(ids))
			{
				var listId = ids.Split(',');
				if (listId != null)
				{
					foreach (var id in listId)
					{
						var item = _db.Posts.Find(Convert.ToInt32(id));
						_db.Posts.Remove(item);
						_db.SaveChanges();
					}
				}
				return Json(new { success = true });
			}
			return Json(new { success = false });

		}
	}
}