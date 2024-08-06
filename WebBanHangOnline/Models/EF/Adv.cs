using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
	[Table("tbl_Adv")]
	public class Adv:CommonAbstract
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required(ErrorMessage = "Vui lòng không bỏ trống tiêu đề")]
		public string Tilte { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public string Link { get; set; }
		public int Type { get; set; }
	}
}