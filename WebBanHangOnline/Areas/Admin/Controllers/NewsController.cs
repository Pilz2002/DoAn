using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
		private readonly ApplicationDbContext _db = new ApplicationDbContext();
		// GET: Admin/News
		public ActionResult Index(int? page,string searchText)
        {
			var pageSize = 10;
			if(page == null)
			{
				page = 1;
			}
			IEnumerable<News> items = _db.News.OrderByDescending(x => x.Id);
			if(!string.IsNullOrEmpty(searchText))
			{
				items = items.Where(x => x.Alias.Contains(searchText) || x.Title.Contains(searchText));
			}
			var pageNumber = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageNumber,pageSize);
			ViewBag.PageNumber = pageNumber;
			ViewBag.PageSize = pageSize;
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
        public ActionResult Add(News model)
        {
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.IsActive = true;
                model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
                _db.News.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

		public ActionResult Edit(int Id)
		{
            var item = _db.News.Find(Id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(News model)
		{
			if (ModelState.IsValid)
			{
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
				_db.News.Attach(model);
                _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(model);
		}

        [HttpPost]
		public ActionResult Delete(int Id)
		{
			var item = _db.News.Find(Id);
            if(item != null)
            {
                _db.News.Remove(item);
                _db.SaveChanges();
				return Json(new {success = true});
			}
			return Json(new { success = false });
		}

		public ActionResult IsActive(int Id)
		{
			var item = _db.News.Find(Id);
			if (item != null)
			{
				item.IsActive = !item.IsActive;
				_db.Entry(item).State = System.Data.Entity.EntityState.Modified;
				_db.SaveChanges();
				return Json(new { success = true, isActive = item.IsActive });
			}
			return Json(new { success = false,isActive = item.IsActive });
		}

		[HttpPost]
		public ActionResult DeleteAll(string ids)
		{
			if(!string.IsNullOrEmpty(ids))
			{
				var listId = ids.Split(',');
				if(listId != null)
				{
					foreach(var id in listId)
					{
						var item = _db.News.Find(Convert.ToInt32(id));
						_db.News.Remove(item);
						_db.SaveChanges();
					}
				}
				return Json(new { success = true});
			}
			return Json(new { success = false });

		}
	}
}