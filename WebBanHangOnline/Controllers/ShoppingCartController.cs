using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
	public class ShoppingCartController : Controller
	{
		private readonly ApplicationDbContext _db = new ApplicationDbContext();
		// GET: ShoppingCart
		public ActionResult Index()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				return View(cart.items);
			}
			return View();
		}

		public ActionResult Checkout()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				ViewBag.CheckCart = cart;
			}
			return View();
		}

		public ActionResult CheckoutSuccess()
		{
			return View();
		}

		public ActionResult Partial_Checkout()
		{
			return PartialView();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Checkout(OrderViewModel req)
		{
			var code = new { failure = true, code = -1 };
			if (ModelState.IsValid)
			{
				ShoppingCart cart = (ShoppingCart)Session["Cart"];
				if (cart != null)
				{
					Order order = new Order();
					order.CustomerName = req.CustomerName;
					order.Phone = req.Phone;
					order.Address = req.Address;
					cart.items.ForEach(x => order.OrderDetails.Add(new OrderDetail
					{
						ProductId = x.ProductId,
						Quantity = x.Quantity,
						Price = x.Price
					}));
					order.CreatedDate = DateTime.Now;
					order.ModifiedDate = DateTime.Now;
					order.CreatedBy = req.Phone;
					order.TotalAmount = cart.items.Sum(x => (x.Price * x.Quantity));
					order.TypeOfPayment = req.TypeOfPayment;
					order.Email = req.Email;
					Random rd = new Random();
					order.Code = "DH_" + req.Email.Substring(0, req.Email.IndexOf('@')) + req.Phone.Substring(6, 4) + "_" + rd.Next(0, int.MaxValue);
					_db.Orders.Add(order);
					_db.SaveChanges();
					//send email
					string contentCustomer =  System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
					var strSanPham = "";
					var thanhTien = decimal.Zero;
					foreach (var sp in cart.items)
					{
						strSanPham += "<tr>";
						strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word\"><span>"+ sp.ProductName + "<span>đ</span> </span></td>";
						strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\">" + sp.Quantity + "</td>";
						strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\">" + WebBanHangOnline.Models.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
						strSanPham += "</tr>";
						thanhTien += sp.Price * sp.Quantity;
					}
					contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
					contentCustomer = contentCustomer.Replace("{{NgayDat}}", order.CreatedDate.ToString("dd/MM/yyyy"));
					contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
					contentCustomer = contentCustomer.Replace("{{ThanhTien}}", WebBanHangOnline.Models.Common.Common.FormatNumber(thanhTien, 0));
					contentCustomer = contentCustomer.Replace("{{TongTien}}", WebBanHangOnline.Models.Common.Common.FormatNumber(thanhTien, 0));
					contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
					contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
					contentCustomer = contentCustomer.Replace("{{Email}}", order.Email);
					contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
					bool check = WebBanHangOnline.Models.Common.Common.SendMail("ShopOnline", "Đơn hàng # " + order.Code, contentCustomer, order.Email);
					cart.ClearCart();
					return RedirectToAction("CheckoutSuccess");
				}
			}
			return Json(code);
		}

		public ActionResult ShowCount()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				return Json(new { count = cart.items.Count }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { count = 0, JsonRequestBehavior.AllowGet });
		}

		[HttpPost]
		public ActionResult AddToCart(int id, int quantity)
		{
			var code = new { success = false, msg = "", code = -1, count = 0 };
			ApplicationDbContext _db = new ApplicationDbContext();
			var checkProduct = _db.Products.FirstOrDefault(x => x.Id == id);
			if (checkProduct != null)
			{
				ShoppingCart cart = (ShoppingCart)Session["Cart"];
				if (cart == null)
				{
					cart = new ShoppingCart();
				}

				ShoppingCartItem item = new ShoppingCartItem
				{
					ProductId = checkProduct.Id,
					ProductName = checkProduct.Title,
					CategoryName = checkProduct.ProductCategory.Title,
					Alias = checkProduct.Alias,
					Quantity = quantity
				};

				if (checkProduct.ProductImages.FirstOrDefault(x => x.IsDefault) != null)
				{
					item.ProductImg = checkProduct.ProductImages.FirstOrDefault(x => x.IsDefault).Image;
				}

				item.Price = checkProduct.Price;

				if (checkProduct.PriceSale != checkProduct.Price)
				{
					item.Price = (decimal)checkProduct.PriceSale;
				}

				item.TotalPrice = item.Quantity * item.Price;
				cart.AddToCart(item, quantity);
				Session["Cart"] = cart;
				code = new { success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công", code = 1, count = cart.items.Count };
			}
			return Json(code);
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var code = new { success = false, msg = "", code = -1, count = 0 };
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				var checkProduct = cart.items.FirstOrDefault(x => x.ProductId == id);
				if (checkProduct != null)
				{
					cart.Remove(id);
					code = new { success = true, msg = "", code = 1, count = cart.items.Count };
				}
			}
			return Json(code);
		}

		public ActionResult Partial_Item_Cart()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				return PartialView(cart.items);
			}
			return PartialView();
		}

		public ActionResult Partial_Item_Pay()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				return PartialView(cart.items);
			}
			return PartialView();
		}

		[HttpPost]
		public ActionResult DeleteAll()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				cart.ClearCart();
				return Json(new { success = true });
			}
			return Json(new { success = false });

		}

		[HttpPost]
		public ActionResult Update(int id, int quantity)
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				cart.UpdateQuantity(id, quantity);
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}
	}
}