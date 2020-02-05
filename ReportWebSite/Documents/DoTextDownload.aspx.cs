using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using FirebirdSql.Data.FirebirdClient;

public partial class Documents_DoTextDownload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string docIdStr = Request.QueryString["docid"];

        if (docIdStr != null && docIdStr.Length > 0)
        {
            int documentId = int.Parse(docIdStr);

            using (TempFile tempFile = new TempFile())
            {
                object docNumber = null;
                object docDate = null;
                object docTitle = null;
                int externalDocId = -1;

                if (Utils.GetDocProperties(documentId, out docNumber, out docDate, out docTitle, out externalDocId))
                {
                    bool isHtml = false;

                    if (Utils.GetExternalDocText(externalDocId, tempFile, out isHtml))
                    {
                        // Generate document title
                        string title = Resources.Strings.DocDownloadTitleSimple;

                        if (docNumber is string)
                        {
                            title = Resources.Strings.DocDownloadTitleWithNum + ((string)docNumber).Trim();
                        }

                        if (docDate is DateTime)
                        {
                            title += Resources.Strings.DocDownloadTitleDate;
                            title += ((DateTime)docDate).ToShortDateString();
                        }

                        // Return the file content
                        System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();

                        if (isHtml)
                        {
                            Response.ContentType = "text/html";
                            title += ".html";
                        }
                        else
                        {
                            Response.ContentType = "text/plain";
                            title += ".txt";
                        }

                        Response.AddHeader("content-disposition", "attachment; filename=" + title + "; size=" + info.Length.ToString());

                        Response.Cache.SetCacheability(HttpCacheability.NoCache);

                        // Pipe the stream contents to the output stream
                        using (System.IO.FileStream stream = System.IO.File.Open(tempFile.FileName,
                            System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite))
                        {
                            stream.CopyTo(Response.OutputStream);
                        }

                        Response.End();
                    }
                }
            }
        }
    }
}