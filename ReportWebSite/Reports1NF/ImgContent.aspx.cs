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

public partial class Reports1NF_Cabinet : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		var rootFolder = PhotorowUtils.ImgContentRootFolder;
		var photofilename = Path.Combine(rootFolder, Request.QueryString["photofilename"]);
		photofilename = photofilename.Replace(@"/", @"\");
		var filename = Path.GetFileName(photofilename);

		SqlConnection connectionSql = Utils.ConnectToDatabase();
		var bytes = PhotorowUtils.Read(photofilename, connectionSql);
		connectionSql.Close();

		Response.Clear();
		Response.ContentType = "application/jpeg";
		Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
		if (bytes != null)
		{
			Response.BinaryWrite(bytes);
		}

		Response.Flush();
		Response.End();
	}

}