using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
using WebBanHangOnline.Models.Payments;

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
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("login", "account");
			}
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

		public ActionResult VnpayReturn()
		{
			if (Request.QueryString.Count > 0)
			{
				string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
				var vnpayData = Request.QueryString;
				VnPayLibrary vnpay = new VnPayLibrary();

				foreach (string s in vnpayData)
				{
					//get all querystring data
					if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
					{
						vnpay.AddResponseData(s, vnpayData[s]);
					}
				}

				string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
				long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
				string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
				string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
				String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
				String TerminalID = Request.QueryString["vnp_TmnCode"];
				long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
				String bankCode = Request.QueryString["vnp_BankCode"];

				bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
				if (checkSignature)
				{
					if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
					{
						//Thanh toan thanh cong
						ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
						var itemOrder = _db.Orders.FirstOrDefault(x => x.Code == orderCode);
						if (itemOrder != null)
						{
							itemOrder.TrangThai = "complete";
							_db.Orders.Attach(itemOrder);
							_db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
							_db.SaveChanges();
						}
					}
					else
					{
						//Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
						ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
					}
					ViewBag.MaThanhToan = "Mã đơn hàng thanh toán:" + orderCode;
					ViewBag.MaGiaoDich = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
					ViewBag.TongTien = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
					ViewBag.NganHang = "Ngân hàng thanh toán:" + bankCode;
				}
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Checkout(OrderViewModel req)
		{
			var code = new { failure = true, code = -1, url = "" };
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
					order.TrangThai = "pending";
					if (User.Identity.IsAuthenticated)
					{
						order.CustomerId = User.Identity.GetUserId();
					}
					Random rd = new Random();
					order.Code = "DH_" + req.Email.Substring(0, req.Email.IndexOf('@')) + req.Phone.Substring(6, 4) + "_" + rd.Next(0, int.MaxValue);
					_db.Orders.Add(order);
					_db.SaveChanges();
					//send email
					string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
					var strSanPham = "";
					var thanhTien = decimal.Zero;
					foreach (var sp in cart.items)
					{
						strSanPham += "<tr>";
						strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word\"><span>" + sp.ProductName + "<span>đ</span> </span></td>";
						strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\">" + sp.Quantity + "</td>";
						strSanPham += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\">" + WebBanHangOnline.Models.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
						strSanPham += "</tr>";
						thanhTien += sp.Price * sp.Quantity;
					}
					//email khách hàng
					contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
					contentCustomer = contentCustomer.Replace("{{NgayDat}}", order.CreatedDate.ToString("dd/MM/yyyy"));
					contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
					contentCustomer = contentCustomer.Replace("{{ThanhTien}}", WebBanHangOnline.Models.Common.Common.FormatNumber(thanhTien, 0));
					contentCustomer = contentCustomer.Replace("{{TongTien}}", WebBanHangOnline.Models.Common.Common.FormatNumber(thanhTien, 0));
					contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
					contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
					contentCustomer = contentCustomer.Replace("{{Email}}", order.Email);
					contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
					WebBanHangOnline.Models.Common.Common.SendMail("ShopOnline", "Đơn hàng # " + order.Code, contentCustomer, order.Email);

					//email admin
					string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
					contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
					contentAdmin = contentAdmin.Replace("{{NgayDat}}", order.CreatedDate.ToString("dd/MM/yyyy"));
					contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
					contentAdmin = contentAdmin.Replace("{{ThanhTien}}", WebBanHangOnline.Models.Common.Common.FormatNumber(thanhTien, 0));
					contentAdmin = contentAdmin.Replace("{{TongTien}}", WebBanHangOnline.Models.Common.Common.FormatNumber(thanhTien, 0));
					contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
					contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
					contentAdmin = contentAdmin.Replace("{{Email}}", order.Email);
					contentAdmin = contentAdmin.Replace("{{PhuongThucThanhToan}}", order.TypeOfPayment == 1 ? "COD" : "Chuyển khoản online");
					contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
					WebBanHangOnline.Models.Common.Common.SendMail("ShopOnline", "Xác nhận đơn hàng # " + order.Code, contentAdmin, ConfigurationManager.AppSettings["EmailAdmin"]);

					cart.ClearCart();
					code = new { failure = false, code = req.TypeOfPayment, url = "" };
					if (req.TypeOfPayment == 2)
					{
						var url = UrlPayment(req.TypePaymentVN, order.Code);
						code = new { failure = false, code = req.TypeOfPayment, url = url };
						return Json(code, JsonRequestBehavior.AllowGet);
					}
				}
			}
			return Json(code, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ShowCount()
		{
			ShoppingCart cart = (ShoppingCart)Session["Cart"];
			if (cart != null)
			{
				return Json(new { count = cart.items.Count }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
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

		#region Thanh toán VnPay
		public string UrlPayment(int typePaymentVN, string orderCode)
		{
			var urlPayment = "";
			var order = _db.Orders.FirstOrDefault(x => x.Code == orderCode);
			//Get Config Info
			string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
			string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
			string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
			string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

			//Build URL for VNPAY
			VnPayLibrary vnpay = new VnPayLibrary();

			var price = (long)order.TotalAmount * 100;
			vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
			vnpay.AddRequestData("vnp_Command", "pay");
			vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
			vnpay.AddRequestData("vnp_Amount", price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
			if (typePaymentVN == 1)
			{
				vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
			}
			else if (typePaymentVN == 2)
			{
				vnpay.AddRequestData("vnp_BankCode", "VNBANK");
			}
			else if (typePaymentVN == 3)
			{
				vnpay.AddRequestData("vnp_BankCode", "INTCARD");
			}

			vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
			vnpay.AddRequestData("vnp_CurrCode", "VND");
			vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
			vnpay.AddRequestData("vnp_Locale", "vn");
			vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng:" + order.Code);
			vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

			vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
			vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

			//Add Params of 2.1.0 Version
			//Billing

			urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
			//log.InfoFormat("VNPAY URL: {0}", paymentUrl);
			return urlPayment;
		}
		#endregion
	}
}