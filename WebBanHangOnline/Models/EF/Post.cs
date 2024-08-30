using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHangOnline.Models.EF
{
	[Table("tbl_Post")]
	public class Post:CommonAbstract
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required(ErrorMessage = "Tiêu đề không được để trống")]
		public string Title { get; set; }
		public string Alias { get; set; }
		public string Description { get; set; }
		[AllowHtml]
		public string Detail { get; set; }
		public string Image { get; set; }
		public int CategoryId { get; set; }
		public virtual Category Category { get; set; }
		public string SeoTitle { get; set; }
		public string SeoDescription { get; set; }
		public string SeoKeyWords { get; set; }
		public bool IsActive { get; set; }

	}
}