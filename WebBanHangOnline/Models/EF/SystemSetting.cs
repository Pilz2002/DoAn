using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
	[Table("tbl_SystemSetting")]
	public class SystemSetting
	{
		[Key]
		[StringLength(50)]
		public string SettingKey { get; set; }
		public string SettingValue { get; set; }
		public string SettingDescription { get; set; }
	}
}