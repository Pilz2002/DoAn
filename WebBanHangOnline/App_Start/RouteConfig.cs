using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebBanHangOnline
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//thanh toan
			routes.MapRoute(
				name: "Checkout",
				url: "thanh-toan",
				defaults: new { controller = "ShoppingCart", action = "Checkout" },
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			//lien he
			routes.MapRoute(
				name: "Contact",
				url: "lien-he",
				defaults: new { controller = "Contact", action = "Index"},
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			//gio hang
			routes.MapRoute(
				name: "ShoppingCart",
				url: "gio-hang",
				defaults: new { controller = "ShoppingCart", action = "Index" },
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			//san pham theo danh muc
			routes.MapRoute(
				name: "CategoryProduct",
				url: "san-pham/{alias}-{id}",
				defaults: new { controller = "Products", action = "ProductsCategory", id = UrlParameter.Optional, alias = UrlParameter.Optional },
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			//tat ca san pham
			routes.MapRoute(
				name: "Products",
				url: "san-pham",
				defaults: new { controller = "Products", action = "Index" },
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			//chi tiet san pham
			routes.MapRoute(
				name: "detailProducts",
				url: "chi-tiet/{alias}-{id}",
				defaults: new { controller = "Products", action = "Detail", id = UrlParameter.Optional, alias = UrlParameter.Optional },
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			//tin tuc
			routes.MapRoute(
				name: "news",
				url: "tin-tuc",
				defaults: new { controller = "News", action = "Index" },
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			//chi tiet tin tuc
			routes.MapRoute(
				name: "detailNews",
				url: "{alias}-n{id}",
				defaults: new { controller = "News", action = "Detail", id = UrlParameter.Optional, alias = UrlParameter.Optional },
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			//chi tiet bai viet	
			routes.MapRoute(
				name: "detailPosts",
				url: "post/{alias}",
				defaults: new { controller = "Article", action = "Index", alias = UrlParameter.Optional },
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			//return sau khi thanh toan
			routes.MapRoute(
				name: "vnpay_return",
				url: "vnpay_return",
				defaults: new { controller = "ShoppingCart", action = "VnpayReturn"},
				namespaces: new[] { "WebBanHangOnline.Controllers" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				namespaces: new[] {"WebBanHangOnline.Controllers"} 
			);
		}
	}
}
