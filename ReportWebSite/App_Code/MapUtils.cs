using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;

public static class MapUtils
{
	public const string GIS = "GIS";
	public const string MBK = "MBK";

	const string CookSystem = "GUKV.Map.System";
	public static void SetSystem(string system)
    {
		var userInfo = new HttpCookie(CookSystem);
		userInfo["System"] = system;
		userInfo.Expires.Add(TimeSpan.FromDays(365 * 10));
		//HttpContext.Current.Request.Cookies.Add(userInfo);
		HttpContext.Current.Response.Cookies.Add(userInfo);
	}

	public static string GetSystem()
	{
		var system = GIS;
		var userInfo = HttpContext.Current.Request.Cookies[CookSystem];
		if (userInfo != null)
		{
			if (userInfo["System"] != null)
			{
				system = userInfo["System"].ToString();
			}
		}
		return system;
	}

}