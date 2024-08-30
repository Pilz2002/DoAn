using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
	[Table("tbl_ProductCategory")]
	public class ProductCategory : CommonAbstract
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required(ErrorMessage = "Tiêu đề không được để trống")]
		public string Title { get; set; }
		public string Alias { get; set; }
		public string Description { get; set; }
		public string Icon { get; set; }
		public string SeoTitle { get; set; }
		public string SeoDescription { get; set; }
		public string SeoKeywords { get; set; }
		public virtual ICollection<Product> Products { get; set; }
	}
}