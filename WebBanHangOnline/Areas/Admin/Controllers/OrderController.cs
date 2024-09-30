using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using PagedList;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin,Employee")]
	public class OrderController : Controller
	{
		// GET: Admin/Order
		private readonly ApplicationDbContext _db = new ApplicationDbContext();
		public ActionResult Index(int? page)
		{
			var items = _db.Orders.OrderByDescending(x => x.CreatedDate).ToList();
			if (page == null)
			{
				page = 1;
			}
			var pageNumber = page ?? 1;
			var pageSize = 15;
			return View(items.ToPagedList(pageNumber, pageSize));
		}

		public ActionResult View(int id)
		{
			var item = _db.Orders.Find(id);
			return View(item);
		}

		[HttpPost]
		public ActionResult Update(int id,string trangThai)
		{
			var item = _db.Orders.Find(id);
			if(item!=null)
			{
				_db.Orders.Attach(item);
				item.TrangThai = trangThai;
				_db.Entry(item).Property(x => x.TrangThai).IsModified = true;
				_db.SaveChanges();
				return Json(new { message="Success",success=true});
			}
			return Json(new { message = "Unsuccess", success = false });
		}
	}
}