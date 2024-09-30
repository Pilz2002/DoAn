using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
	[Table("tbl_Order")]
	public class Order:CommonAbstract
	{
		public Order()
		{
			this.OrderDetails = new HashSet<OrderDetail>();
		}
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string Code { get; set; }
		[Required]
		public string CustomerName { get; set; }
		[Required]
		public string Phone { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string Email { get; set; }
		public string TrangThai { get; set; }
		public decimal TotalAmount { get; set; }
		public int Quantity { get; set; }
		public int TypeOfPayment { get; set; }
		public string CustomerId { get; set; }

		public virtual ICollection<OrderDetail> OrderDetails { get; set; }
	}
}