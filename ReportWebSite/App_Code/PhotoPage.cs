using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExtDataEntry.Models;

/// <summary>
/// Summary description for PhotoPage
/// </summary>
public class PhotoPage : System.Web.UI.Page
{
    protected string GetPageUniqueKey()
    {
        object key = ViewState["PageUniqueKey"];

        if (key is string)
        {
            return (string)key;
        }

        // Generate unique key
        Guid guid = Guid.NewGuid();

        string str = guid.ToString();

        ViewState["PageUniqueKey"] = str;

        return str;
    }

    protected IEnumerable<FileAttachment> PhotoData
    {
        get
        {
            object d = Session[GetPageUniqueKey() + "_PhotoData"];
            return (d is IEnumerable<FileAttachment>) ? (IEnumerable<FileAttachment>)d : null;
        }
        set
        {
            Session[GetPageUniqueKey() + "_PhotoData"] = value;
        }
    }
}