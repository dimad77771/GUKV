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
using Syncfusion.Compression.Zip;
using System.Drawing.Imaging;

public partial class Reports1NF_Cabinet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		//var rid = Int32.Parse(Request.QueryString["rid"]);
		//var bid = Int32.Parse(Request.QueryString["bid"]);
		var id = Int32.Parse(Request.QueryString["id"]);
		var isjpeg = (Request.QueryString["jpeg"] == "1");

		if (!isjpeg)
		{
			var fname = @"privat_" + "_" + id + ".pdf";
			var bytes = new PrivatisatPhotosPdfBulder().Go(id);

			Response.Clear();
			Response.ContentType = "application/pdf";
			Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fname + "\"");
			Response.BinaryWrite(bytes);

			Response.Flush();
			Response.End();
		}
		else
		{
			var fname = @"privat_jpeg_" + "_" + id + ".zip";
			var bytes = new PrivatisatPhotosJpegBulder().Go(id);

			Response.Clear();
			Response.ContentType = "application/zip";
			Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fname + "\"");
			Response.BinaryWrite(bytes);

			Response.Flush();
			Response.End();
		}
	}




	public class PrivatisatPhotosPdfBulder
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
			photo1NFPath = Path.Combine(photoRootPath, "privatisat_documents");
		}

		IEnumerable<FileInfo> GetAllFiles()
		{
			string query = @"select file_name, file_ext from privatisat_documents a where a.free_square_id = @free_square_id";
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
			if (!File.Exists(fi.FullFilename)) return false;

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
				image = Bitmap.FromFile(fi.FullFilename);
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
				expdf = new PdfLoadedDocument(fi.FullFilename);
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
				docx = new WordDocument(fi.FullFilename);
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


	public class PrivatisatPhotosJpegBulder
	{
		Int32 Free_square_id;
		SqlConnection connectionSql;
		String photoRootPath, photo1NFPath;
		String OutCatalog;
		Int32 ImageCount = 0;
		Byte[] OutputZipBytes;


		public Byte[] Go(int free_square_id)
		{
			Free_square_id = free_square_id;
			ConfigInit();
			CatalogInit();
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


			connectionSql.Close();
			CatalogSave();
			ClearTempFiles();
			return OutputZipBytes;
		}

		void ConfigInit()
		{
			photoRootPath = WebConfigurationManager.AppSettings["ImgFreeSquareRootFolder"];
			photo1NFPath = Path.Combine(photoRootPath, "privatisat_documents");
		}

		IEnumerable<FileInfo> GetAllFiles()
		{
			string query = @"select file_name, file_ext from privatisat_documents a where a.free_square_id = @free_square_id";
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

		void CatalogInit()
		{
			OutCatalog = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(OutCatalog);
		}

		void CatalogSave()
		{
			var outstream = new MemoryStream();

			var zipArchive = new ZipArchive();
			var files = Directory.GetFiles(OutCatalog);
			foreach (var file in files)
			{
				zipArchive.AddFile(file);
			}
			zipArchive.Save(outstream, true);
			zipArchive.Close();

			OutputZipBytes = outstream.ToArray();

			outstream.Dispose();
		}


		bool ProcFile(FileInfo fi)
		{
			if (!File.Exists(fi.FullFilename)) return false;

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
				image = Bitmap.FromFile(fi.FullFilename);
			}
			catch (Exception ex)
			{
				return false;
			}

			SaveImage(image);

			image.Dispose();
			return true;
		}

		void SaveImage(System.Drawing.Image image)
		{
			var myImageCodecInfo = GetEncoderInfo("image/jpeg");
			var myEncoder = System.Drawing.Imaging.Encoder.Quality;
			var myEncoderParameters = new EncoderParameters(1);

			var myEncoderParameter = new EncoderParameter(myEncoder, 90L);
			myEncoderParameters.Param[0] = myEncoderParameter;
			var filename = GetNextFilename();
			image.Save(filename, myImageCodecInfo, myEncoderParameters);
		}

		string GetNextFilename()
		{
			ImageCount++;
			var filename = Path.Combine(OutCatalog, ImageCount.ToString() + ".jpeg");
			return filename;
		}

		ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for (j = 0; j < encoders.Length; ++j)
			{
				if (encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}

		public const int PDF_IMAGE_RESOLUTION = 150;
		bool ProcFilePdf(FileInfo fi = default(FileInfo), byte[] pdfBytes = null)
		{
			var f = new SautinSoft.PdfFocus();
			f.Serial = "10003636879";

			if (pdfBytes != null)
			{
				f.OpenPdf(pdfBytes);
			}
			else
			{
				f.OpenPdf(fi.FullFilename);
			}

			if (f.PageCount <= 0)
			{
				return false;
			}

			f.ImageOptions.Dpi = PDF_IMAGE_RESOLUTION;

			for (int i = 1; i <= f.PageCount; i++)
			{
				var img = f.ToDrawingImage(i);
				if (img == null)
				{
					throw new Exception("Не можу розпізнати цей pdf-файл");
				}

				SaveImage(img);

				img.Dispose();
			}

			return true;
		}

		bool ProcFileDocx(FileInfo fi)
		{
			WordDocument docx;
			try
			{
				docx = new WordDocument(fi.FullFilename);
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

			ProcFilePdf(pdfBytes: pdfBytes);

			return true;
		}

		void ClearTempFiles()
		{
			try
			{
				Directory.Delete(OutCatalog, true);
			}
			catch (Exception ex)
			{

			}
		}

		struct FileInfo
		{
			public string file_name;
			public string file_ext;
			public string FullFilename;
		}
	}
}