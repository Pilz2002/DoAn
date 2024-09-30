using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models
{
	public class OrderViewModel
	{
		[Required]
		public string CustomerName { get; set; }
		[Required]
		public string Phone { get; set; }
		[Required]
		public string Address { get; set; }
		public int TypeOfPayment { get; set; }
		public int TypePaymentVN { get; set; }
		public string Email { get; set; }
		public int CustomerId { get; set; }
	}
}