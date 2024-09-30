using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
	[Table("tbl_Review")]
	public class ReviewProduct
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string Username { get; set; }
		public string Fullname { get; set; }
		public string Email { get; set; }
		public string Content { get; set; }
		public int Rate { get; set; }
		public DateTime CreatedDate { get; set; }
		public string Avatar { get; set; }
		public virtual Product Product { get; set; }
	}
}