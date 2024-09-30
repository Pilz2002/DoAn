using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();
		public ActionResult Index()
		{
			ViewBag.Title = "Kappa Shop";
			ViewBag.SeoDescription = "Kappa Shop online";
			return View();
		}

		public ActionResult Partial_Subscribe()
		{
			return PartialView();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Subscribe(Subscribe req)
		{
			if(ModelState.IsValid)
			{
				_db.Subscribes.Add(new Subscribe { Email = req.Email, CreatedDate = DateTime.Now });
				_db.SaveChanges();
				return Json(true);
			}
			return View("Partial_Subscribe");
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult Refresh()
		{
			var item = new ThongKeStringViewModel();
			ViewBag.visitors_online = HttpContext.Application["visitors_online"];
			item.HomNay = HttpContext.Application["HomNay"].ToString();
			item.HomQua = HttpContext.Application["HomQua"].ToString();
			item.TuanNay = HttpContext.Application["TuanNay"].ToString();
			item.TuanTruoc = HttpContext.Application["TuanTruoc"].ToString();
			item.ThangNay = HttpContext.Application["ThangNay"].ToString();
			item.ThangTruoc = HttpContext.Application["ThangTruoc"].ToString();
			item.TatCa = HttpContext.Application["TatCa"].ToString();
			return PartialView(item);
		}
	}
}