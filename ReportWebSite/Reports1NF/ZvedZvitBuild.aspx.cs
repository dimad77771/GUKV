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

public partial class Reports1NF_Cabinet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		var builder = new ZvedZvitBuilder
		{
			Page = this,
		};
		builder.Go();

		//var page = Page;
		////var bytes = new BalansFreeSquarePhotosPdfBulder().Go(id);

		//Response.Clear();
		//Response.ContentType = "application/pdf";
		//Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fname + "\"");
		//Response.BinaryWrite(bytes);

		//Response.Flush();
		//Response.End();
	}
}