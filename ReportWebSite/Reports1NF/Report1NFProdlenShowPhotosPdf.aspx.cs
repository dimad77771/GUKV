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
		var id = Int32.Parse(Request.QueryString["id"]);
		var isjpeg = (Request.QueryString["jpeg"] == "1");

		if (!isjpeg)
		{
		}
		else
		{
			var fname = @"free_jpeg_" + "_" + id + ".zip";
			var bytes = new JpegBulder().Go(id);

			Response.Clear();
			Response.ContentType = "application/zip";
			Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fname + "\"");
			Response.BinaryWrite(bytes);

			Response.Flush();
			Response.End();
		}
	}





	public class JpegBulder
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
			if (Free_square_id >= 1000000)
			{
				photo1NFPath = Path.Combine(photoRootPath, "reports1nf_arenda_dogcontinue_current_stage_documents");
			}
			else
			{
				photo1NFPath = Path.Combine(photoRootPath, "1nf");
			}
		}

		IEnumerable<FileInfo> GetAllFiles()
		{
			string query;
			if (Free_square_id >= 1000000)
			{
				query = @"select file_name, file_ext from reports1nf_arenda_dogcontinue_current_stage_documents a where a.free_square_id = @free_square_id";
			}
			else
			{
				query = @"select file_name, file_ext from reports1nf_balans_free_square_photos a where a.free_square_id = @free_square_id";
			}
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