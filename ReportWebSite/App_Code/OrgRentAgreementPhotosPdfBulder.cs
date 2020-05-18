using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using DevExpress.XtraReports.UI;
using DevExpress.Web.ASPxGridView;
using System.Data.SqlClient;
using Syncfusion.Pdf;
using System.IO;
using System.Drawing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;

public class OrgRentAgreementPhotosPdfBulder
{
    Int32 AgreementId;
    string PhotoFolder;

    SqlConnection connectionSql;
    PdfDocument PdfDoc;

    Byte[] OutputPdfBytes;


    public Byte[] Go(int agreementId, string photoFolder)
    {
        this.AgreementId = agreementId;
        this.PhotoFolder = photoFolder;

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


    IEnumerable<FileInfo> GetAllFiles()
    {
        string query = @"select file_name, file_ext, id from reports1nf_arendaphotos a where a.arenda_id = @arenda_id";
        using (SqlCommand cmd = new SqlCommand(query, connectionSql))
        {
            cmd.Parameters.AddWithValue("arenda_id", AgreementId);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var file_name = reader.IsDBNull(0) ? string.Empty : (string)reader.GetValue(0);
                    var file_ext = reader.IsDBNull(1) ? string.Empty : (string)reader.GetValue(1);
                    var file_id = reader.GetInt32(2);

                    yield return new FileInfo
                    {
                        file_name = file_name,
                        file_ext = file_ext,
                        file_id = file_id,
                        FullFilename = Path.Combine(PhotoFolder, file_name + file_ext),
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
        public int file_id;
        public string FullFilename;
    }
}