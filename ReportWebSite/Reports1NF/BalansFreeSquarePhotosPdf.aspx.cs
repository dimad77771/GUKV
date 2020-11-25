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
		//var rid = Int32.Parse(Request.QueryString["rid"]);
		//var bid = Int32.Parse(Request.QueryString["bid"]);
		var id = Int32.Parse(Request.QueryString["id"]);


		//var fname = @"info_" + bid + "_" + rid + "_" + id + ".pdf";
		var fname = @"info_" + "_" + id + ".pdf";
		var bytes = new BalansFreeSquarePhotosPdfBulder().Go(id);

		Response.Clear();
		Response.ContentType = "application/pdf";
		Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fname + "\"");
		Response.BinaryWrite(bytes);

		Response.Flush();
		Response.End();
	}







	public class BalansFreeSquarePhotosPdfBulder
	{
		Int32 Free_square_id;
		SqlConnection connectionSql;
		String photoRootPath, photo1NFPath;
		PdfDocument PdfDoc;

		Byte[] OutputPdfBytes;


		public Byte[] Go(int free_square_id)
		{
			Free_square_id = free_square_id;
			ConfigInit();
			PdfInit();
			connectionSql = Utils.ConnectToDatabase();

			var allfiles = GetAllFiles().ToArray();

			bool ex = false;
			foreach (var fi in allfiles)
			{
				if (ProcFile(fi))
				{
					ex = true;
				}
			}
			if (!ex)
			{
				BuildEmptyPdf();
			}


			connectionSql.Close();
			PdfSave();
			return OutputPdfBytes;
		}

		void ConfigInit()
		{
			photoRootPath = WebConfigurationManager.AppSettings["ImgFreeSquareRootFolder"];
			photo1NFPath = Path.Combine(photoRootPath, "1NF");
		}

		IEnumerable<FileInfo> GetAllFiles()
		{
			string query = @"select file_name, file_ext from reports1nf_balans_free_square_photos a where a.free_square_id = @free_square_id";
			using (SqlCommand cmd = new SqlCommand(query, connectionSql))
			{
				cmd.Parameters.AddWithValue("free_square_id", Free_square_id);
				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						string file_name = reader.IsDBNull(0) ? string.Empty : (string)reader.GetValue(0);
						string file_ext = reader.IsDBNull(1) ? string.Empty : (string)reader.GetValue(1);

						yield return new FileInfo
						{
							file_name = file_name,
							file_ext = file_ext,
							FullFilename = Path.Combine(photo1NFPath, "" + Free_square_id, file_name + file_ext),
						};
					}
					reader.Close();
				}
			}
		}

		void PdfInit()
		{
			PdfDoc = new PdfDocument();
		}

		void PdfSave()
		{
			var outstream = new MemoryStream();
			PdfDoc.Save(outstream);
			outstream.Dispose();
			OutputPdfBytes = outstream.ToArray();

			//File.WriteAllBytes(@"H:\DOWNLOADS\PDFBox.NET-1.8.9\aaa" + Guid.NewGuid() + ".pdf", OutputPdfBytes);
		}


		bool ProcFile(FileInfo fi)
		{
			if (!LLLLhotorowUtils.Exists(fi.FullFilename, connectionSql)) return false;

			if (ProcFileImage(fi)) return true;
			if (ProcFilePdf(fi)) return true;
			if (ProcFileDocx(fi)) return true;

			return false;
		}

		bool ProcFileImage(FileInfo fi)
		{
			System.Drawing.Image image;
			try
			{
				var mem = new MemoryStream(LLLLhotorowUtils.Read(fi.FullFilename, connectionSql));
				image = Bitmap.FromStream(mem);
			}
			catch (Exception ex)
			{
				return false;
			}

			var convertor = new PdfUnitConvertor();
			var pdfBitmap = new PdfBitmap(image);

			var width = convertor.ConvertFromPixels(image.Width, PdfGraphicsUnit.Point);
			var height = convertor.ConvertFromPixels(image.Height, PdfGraphicsUnit.Point);

			var section = PdfDoc.Sections.Add();
			section.PageSettings.Size = new SizeF(width, height);
			section.PageSettings.Orientation = (width > height ? PdfPageOrientation.Landscape : PdfPageOrientation.Portrait);
			section.PageSettings.Margins.Bottom = 0;
			section.PageSettings.Margins.Top = 0;
			section.PageSettings.Margins.Left = 0;
			section.PageSettings.Margins.Right = 0;
			var page = section.Pages.Add();

			var graphics = page.Graphics;
			graphics.DrawImage(pdfBitmap, new RectangleF(0, 0, width, height));

			image.Dispose();
			return true;
		}

		void BuildEmptyPdf()
		{
			var page = PdfDoc.Pages.Add();
			var graphics = page.Graphics;
			//var font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
			var font = new PdfTrueTypeFont(new System.Drawing.Font("Arial Unicode MS", 20), true);
			graphics.DrawString("Зображення відсутні", font, PdfBrushes.Black, new PointF(10, 10));
			//graphics.DrawString("Heloo", font, PdfBrushes.Black, new PointF(10, 10));
		}

		bool ProcFilePdf(FileInfo fi)
		{
			PdfLoadedDocument expdf;
			try
			{
				var bytes = LLLLhotorowUtils.Read(fi.FullFilename, connectionSql);
				expdf = new PdfLoadedDocument(bytes);
			}
			catch (Exception ex)
			{
				return false;
			}

			PdfDoc.Append(expdf);

			expdf.Dispose();
			return true;
		}

		bool ProcFileDocx(FileInfo fi)
		{
			WordDocument docx;
			try
			{
				var mem = new MemoryStream(LLLLhotorowUtils.Read(fi.FullFilename, connectionSql));
				docx = new WordDocument(mem);
			}
			catch (Exception ex)
			{
				return false;
			}

			var converter = new DocToPDFConverter();
			converter.Settings.EmbedFonts = true;
			converter.Settings.ImageQuality = 100;
			converter.Settings.ImageResolution = 640;
			converter.Settings.OptimizeIdenticalImages = true;
			var expdf = converter.ConvertToPDF(docx);

			var outstream = new MemoryStream();
			expdf.Save(outstream);
			var pdfBytes = outstream.ToArray();
			outstream.Dispose();
			expdf.Dispose();

			var pdfdoc = new PdfLoadedDocument(pdfBytes);
			PdfDoc.Append(pdfdoc);
			pdfdoc.Dispose();

			return true;
		}


		struct FileInfo
		{
			public string file_name;
			public string file_ext;
			public string FullFilename;
		}
	}
}