using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Documents_DoTextEmbedded : System.Web.UI.Page
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
                        // Return the file content
                        System.IO.FileInfo info = new System.IO.FileInfo(tempFile.FileName);

                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();

                        if (isHtml)
                        {
                            Response.ContentType = "text/html";
                        }
                        else
                        {
                            Response.ContentType = "text/plain";
                        }

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