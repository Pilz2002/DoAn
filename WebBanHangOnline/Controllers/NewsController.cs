using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
			IEnumerable<News> items = _db.News.OrderByDescending(x => x.Id);
			var pageSize = 15;
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

        public ActionResult Partial_News_Home()
        {
            var items = _db.News.Take(3).ToList();
            return PartialView(items);
        }

        public ActionResult Detail(int id)
        {
            var item = _db.News.Find(id);
            return View(item);
        }


    }
}