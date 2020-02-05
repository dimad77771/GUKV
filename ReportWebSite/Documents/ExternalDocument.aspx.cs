using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Documents_ExternalDocument : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Request.Params["id"];
        if (string.IsNullOrEmpty(id) || Path.IsPathRooted(id) || id[0] == '.')
            return;

        string documentFileName = Path.Combine(ExternalDocument.GetExternalDocumentStorePath(true), id);
        if (!File.Exists(documentFileName))
        {
            documentFileName = Path.Combine(ExternalDocument.GetExternalDocumentStorePath(false), id);
            if (!File.Exists(documentFileName))
                return;
        }

        string name = Request.Params["name"];

        Response.Clear();
        Response.ClearHeaders();
        Response.ClearContent();

        switch (Path.GetExtension(documentFileName).ToLower())
        {
            case ".docx":
                Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;
            default:
                Response.ContentType = "application/octet-stream";
                break;
        }

        string contentDisposition = "attachment; filename"; // +Uri.EscapeDataString(name ?? id);
        if (Request.Browser.Browser == "IE" && (Request.Browser.Version == "7.0" || Request.Browser.Version == "8.0"))
            contentDisposition += "=" + Uri.EscapeDataString(name ?? id);
        else if (Request.Browser.Browser == "Safari")
            contentDisposition += "=" + (name ?? id);
        else
            contentDisposition += "*=UTF-8''" + Uri.EscapeDataString(name ?? id);
        Response.AddHeader("Content-Disposition", contentDisposition);

        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        // Pipe the stream contents to the output stream
        using (FileStream stream = new FileStream(documentFileName, FileMode.Open, FileAccess.Read))
        {
            stream.CopyTo(Response.OutputStream);
        }

        Response.End();
    }
}