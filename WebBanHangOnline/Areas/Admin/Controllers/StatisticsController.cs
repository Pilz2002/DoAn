using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin,Employee")]
	public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Admin/Statistics
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate,string toDate)
        {
            var query = from o in _db.Orders
                        join od in _db.OrderDetails on o.Id equals od.OrderId
                        join p in _db.Products on od.ProductId equals p.Id
                        select new {
                            CreateDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            if(!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy",null);
                query = query.Where(x => x.CreateDate >= startDate);
            }

			if (!string.IsNullOrEmpty(toDate))
			{
				DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
				query = query.Where(x => x.CreateDate < endDate);
			}

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreateDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y=>y.Quantity*y.OriginalPrice),
                TotalSell = x.Sum(y=>y.Quantity*y.Price)
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
			return Json(new {Data=result},JsonRequestBehavior.AllowGet);
        }
    }
}