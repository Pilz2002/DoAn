using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Article
        public ActionResult Index(string alias)
        {
            var item = _db.Posts.FirstOrDefault(x => x.Alias == alias);
            return View(item);
        }
    }
}