using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using ExtDataEntry.Models;
using System.Data.SqlClient;

public static class ZvitDbUtils
{
	public static void DbFilename2LocalFilename(SqlConnection connection, int reportID, string user)
	{
		WorkItem workItem = null;

		workItem.Init(reportID, user);
		workItem.DoWork();

		//var trans = connection.BeginTransaction();

		//Reports1NFUtils.SendRentedObject(connection, reportID, id);

		//trans.Commit();
	}

}