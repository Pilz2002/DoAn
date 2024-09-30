using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
	[Authorize(Roles = "Admin,Employee")]
	public class ProductImagesController : Controller
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext(); 
        // GET: Admin/ProductImages
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var images = _db.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(images);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _db.ProductImages.Find(id);
            _db.ProductImages.Remove(item);
            _db.SaveChanges();
            return Json(new {success = true});
        }

        [HttpPost]
		public ActionResult AddImage(int productId,string url)
		{
            _db.ProductImages.Add(new Models.EF.ProductImage
            {
                ProductId = productId,
                Image = url,
                IsDefault = false
            });
            _db.SaveChanges();
			return Json(new { success = true });
		}


	}
}