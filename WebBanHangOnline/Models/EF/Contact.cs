using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
	[Table("tbl_Contact")]
	public class Contact : CommonAbstract
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Website { get; set; }
		public string Message { get; set; }
		public bool IsRead { get; set; }
	}
}