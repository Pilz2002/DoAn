using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.Common
{
	public class SettingHelper
	{
		private static ApplicationDbContext _db = new ApplicationDbContext();

		public static string GetValue(string key)
		{
			var item = _db.SystemSettings.SingleOrDefault(x => x.SettingKey == key);
			if (item != null)
			{
				return item.SettingValue;
			}
			return "";
		}
	}
}