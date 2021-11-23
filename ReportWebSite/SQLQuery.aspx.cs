using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.Cryptography;
using System.Text;
using DevExpress.Web;
using System.IO;
using Syncfusion.Pdf;
using System.Web.Configuration;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Parsing;
using Syncfusion.DocToPDFConverter;
using Syncfusion.DocIO.DLS;
using ExtDataEntry.Models;
using GUKV.Common;
using System.Data.Common;
using Newtonsoft.Json;

public partial class Reports1NF_Cabinet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var sql = Request.Form["sql_query"];
		if (string.IsNullOrEmpty(sql))
		{
			sql = Request.QueryString["sql_query"];
		}

		var access_key = Request.Form["access_key"];
		if (string.IsNullOrEmpty(access_key))
		{
			access_key = Request.QueryString["access_key"];
		}
		access_key = (access_key ?? "").ToLower();

		if (access_key != "BB6F3F9DE99E469B8831DC4EB1E12F1F".ToLower())
		{
			Response.StatusCode = 403;
			return;
		}

		//var sql = "select top 100 * from reports1nf_balans";

		var connection = CommonUtils.ConnectToDatabase();
		if (connection == null) throw new Exception("Database GUKV2016 not found");
		var factory = DbProviderFactories.GetFactory(connection);
		var dataTable = new DataTable();
		using (var cmd = factory.CreateCommand())
		{
			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;
			cmd.Connection = connection;
			using (var adapter = factory.CreateDataAdapter())
			{
				adapter.SelectCommand = cmd;
				adapter.Fill(dataTable);
			}
		}


		var json = JsonConvert.SerializeObject(dataTable);
		var bytes = Encoding.UTF8.GetBytes(json);

		Response.Clear();
		Response.ContentType = "application/json";
		//Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fname + "\"");
		Response.BinaryWrite(bytes);

		Response.Flush();
		Response.End();
	}
}