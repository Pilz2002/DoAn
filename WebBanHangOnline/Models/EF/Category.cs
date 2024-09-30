using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
	[Table("tbl_Category")]
	public class Category : CommonAbstract
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required(ErrorMessage ="Tiêu đề không được để trống")]
		public string Title { get; set; }
		public string Alias { get; set; }
		public string Link { get; set; }
		public string Description { get; set; }
		public string SeoTitle { get; set; }
		public string SeoDescription { get; set; }
		public string SeoKeywords { get; set; }
		public int Position { get; set; }
		public bool IsActive { get; set; }

		public virtual ICollection<News> News { get; set; }
		public virtual ICollection<Post> Posts { get; set; }
	}
}