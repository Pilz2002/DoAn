using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace WebBanHangOnline.Models.Common
{
	public class ThongKeTruyCap
	{
		public static string strConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
		public static ThongKeViewModel ThongKe()
		{
			using (var connect = new SqlConnection(strConnection) )
			{
				var item = connect.QueryFirstOrDefault<ThongKeViewModel>("sp_ThongKe", commandType: System.Data.CommandType.StoredProcedure);
				return item;
			}
		}
	}
}