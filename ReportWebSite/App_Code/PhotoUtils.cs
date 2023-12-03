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

public static class PhotoUtils
{
	public const int MAX_IMAGE_LEN = 204800;
	public const int PDF_IMAGE_RESOLUTION = 150;

	public static string DbFilename2LocalFilename(string file_name, string file_ext)
	{
		return file_name + file_ext;
	}

	public struct DbFilename
	{
		public string file_name { get; set; }
		public string file_ext { get; set; }
	}
	public static DbFilename LocalFilename2DbFilename(string localname)
	{
		var filename = Path.GetFileNameWithoutExtension(localname);
		var fileext = Path.GetExtension(localname);

		return new DbFilename
		{
			file_name = filename,
			file_ext = fileext,
		};
	}

	public static void AddUploadedFile(string tempPhotoFolder, string uploadedFileName, byte[] uploadedFileBytes)
	{
		var connectionSql = Utils.ConnectToDatabase();
		var sqlTransaction = connectionSql.BeginTransaction();

		var file_name = Path.GetFileNameWithoutExtension(uploadedFileName);
		var file_ext = Path.GetExtension(uploadedFileName);
		var newfilename = Path.Combine(tempPhotoFolder, DbFilename2LocalFilename(file_name, file_ext));

		var jpeginfo = ProcImage(uploadedFileBytes);
		if (jpeginfo.IsImage)
		{
			LLLLhotorowUtils.Write(newfilename, jpeginfo.Jpegbytes, connectionSql, sqlTransaction);
		}
		else
		{
			var pdfinfo = ProcPdf(uploadedFileName, uploadedFileBytes);
			if (pdfinfo.IsPdf)
			{
				for(int i = 0; i < pdfinfo.ListFileNames.Count; i++)
				{
					var pdfImageFilename = Path.Combine(tempPhotoFolder, DbFilename2LocalFilename(pdfinfo.ListFileNames[i], ".jpg"));
					LLLLhotorowUtils.Write(pdfImageFilename, pdfinfo.ListJpegbytes[i], connectionSql, sqlTransaction);
				}
			}
			else
			{
				throw new Exception("Підтримуються тільки файли зображень (jpg/jpeg/png/bmp/gif) або pdf-файли");
			}
		}

		sqlTransaction.Commit();
		connectionSql.Close();
	}

	public struct ProcImageReturn
	{
		public bool IsImage { get; set; }
		public byte[] Jpegbytes { get; set; }
	}
	public static ProcImageReturn ProcImage(byte[] imageBytes)
	{
		using (MemoryStream memstr = new MemoryStream(imageBytes))
		{
			Image img = null;
			try
			{
				img = Image.FromStream(memstr);
			}
			catch(Exception ex)
			{
				if (ex is ArgumentException)
				{
					return new ProcImageReturn
					{
						IsImage = false,
					};
				}
				throw ex;
			}
			var curkoef = 1.0;
			var kolcycles = 0;
			while (true)
			{
				kolcycles++;
				var curimg = new Bitmap(img, new Size((int)(img.Width * curkoef), (int)(img.Height * curkoef)));
				var jpegbytes = MakeJpeg(curimg);
				var jpeglen = jpegbytes.Length;
				if (jpeglen <= MAX_IMAGE_LEN)
				{
					return new ProcImageReturn
					{
						IsImage = true,
						Jpegbytes = jpegbytes,
					};
				}
				curimg.Dispose();

				double lesskoef;
				if (jpeglen > MAX_IMAGE_LEN * 10)
				{
					lesskoef = 0.5;
				}
				else if (jpeglen > MAX_IMAGE_LEN * 5)
				{
					lesskoef = 0.75;
				}
				else if (jpeglen > MAX_IMAGE_LEN * 3)
				{
					lesskoef = 0.85;
				}
				else if (jpeglen > MAX_IMAGE_LEN * 2)
				{
					lesskoef = 0.90;
				}
				else
				{
					lesskoef = 0.95;
				}
				curkoef = curkoef * lesskoef;
			}
		}
	}

	static byte[] MakeJpeg(Image image)
	{
		var codecs = ImageCodecInfo.GetImageEncoders();
		ImageCodecInfo ici = null;
		foreach (ImageCodecInfo codec in codecs)
		{
			if (codec.MimeType == "image/jpeg")
			{
				ici = codec;
				break;
			}
		}
		if (ici == null) throw new Exception("image/jpeg codec not found");

		EncoderParameters ep = new EncoderParameters();
		ep.Param[0] = new EncoderParameter(Encoder.Quality, (long)95);
		using (var memstream = new MemoryStream())
		{
			image.Save(memstream, ici, ep);
			var jpegbytes = memstream.ToArray();
			return jpegbytes;
		}
	}


	public struct ProcPdfReturn
	{
		public bool IsPdf { get; set; }
		public List<string> ListFileNames { get; set; }
		public List<byte[]> ListJpegbytes { get; set; }
	}
	public static ProcPdfReturn ProcPdf(string pdfFilename, byte[] pdfbytes)
	{
		var ret = new ProcPdfReturn
		{
			IsPdf = true,
			ListFileNames = new List<string>(),
			ListJpegbytes = new List<byte[]>(),
		};
		pdfFilename = Path.GetFileNameWithoutExtension(pdfFilename);

		var f = new SautinSoft.PdfFocus();
		f.Serial = "10003636879";

		f.OpenPdf(pdfbytes);

		if (f.PageCount <= 0) throw new Exception("Pdf error pagecount");
		f.ImageOptions.Dpi = PDF_IMAGE_RESOLUTION;

		for(int i = 1; i <= f.PageCount; i++)
		{
			var img = f.ToDrawingImage(i);
			if (img == null)
			{
				throw new Exception("Не можу розпізнати цей pdf-файл");
			}

			var stream = new MemoryStream();
			img.Save(stream, ImageFormat.Png);
			var pngbytes = stream.ToArray();
			stream.Dispose();

			var jpeginfo = ProcImage(pngbytes);
			if (!jpeginfo.IsImage) throw new ArgumentException();

			ret.ListFileNames.Add(pdfFilename + "_" + i.ToString("0000"));
			ret.ListJpegbytes.Add(jpeginfo.Jpegbytes);

			img.Dispose();
		}
		return ret;
	}

}