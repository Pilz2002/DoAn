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
    public class ProductController : Controller
    {
		private readonly ApplicationDbContext _db = new ApplicationDbContext();
		// GET: Admin/Product
		public ActionResult Index(int? page)
        {
			IEnumerable<Product> items = _db.Products.OrderByDescending(x => x.Id);
			var pageSize = 10;
			if (page == null)
			{
				page = 1;
			}
			var pageNumber = page.HasValue ? Convert.ToInt32(page) : 1;
			items = items.ToPagedList(pageNumber, pageSize);
			ViewBag.PageNumber = pageNumber;
			ViewBag.PageSize = pageSize;
			return View(items);
        }

		public ActionResult Add()
		{
			ViewBag.CategoryId = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Add(Product model,List<string> Images,List<int> rDefault)
		{

			if (ModelState.IsValid)
			{
				if(Images!=null && Images.Count>0)
				{
					for(int i=0;i<Images.Count;i++)
					{
						if( i+1 == rDefault[0])
						{
							model.Image = Images[i];
							model.ProductImages.Add(new ProductImage
							{
								ProductId = model.Id,
								Image = Images[i],
								IsDefault = true
							});
						}
						else
						{
							model.ProductImages.Add(new ProductImage
							{
								ProductId = model.Id,
								Image = Images[i],
								IsDefault = false
							});
						}
					}
				}
				if (string.IsNullOrEmpty(model.SeoTitle))
				{
					model.SeoTitle = model.Title;
				}
				model.CreatedDate = DateTime.Now;
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
				_db.Products.Add(model);
				_db.SaveChanges();
				return RedirectToAction("index");
			}
			ViewBag.CategoryId = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
			return View();
		}

		public ActionResult Edit(int id)
		{
			ViewBag.CategoryId = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
			var item = _db.Products.Find(id);
			return View(item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Product model)
		{
			if(ModelState.IsValid)
			{
				model.ModifiedDate = DateTime.Now;
				model.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(model.Title);
				_db.Products.Attach(model);
				_db.Entry(model).State = System.Data.Entity.EntityState.Modified;
				_db.SaveChanges();
				return RedirectToAction("index");
			}
			ViewBag.CategoryId = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var item = _db.Products.Find(id);
			if(item != null)
			{
				_db.Products.Remove(item);
				_db.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}
	}
}