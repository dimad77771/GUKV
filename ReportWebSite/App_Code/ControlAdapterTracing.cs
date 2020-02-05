using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

public class ControlAdapterTracing : System.Web.UI.Adapters.ControlAdapter
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        this.Page.Trace.Write("OnLoad of :" + this.Control.ID + " - " + this.Control.GetType().FullName);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.Page.Trace.Write("OnPreRender of :" + this.Control.ID + " - " + this.Control.GetType().FullName);
    }

    protected override void BeginRender(HtmlTextWriter writer)
    {
        this.Page.Trace.Write("Started Rendering of :" + this.Control.ID + " - " + this.Control.GetType().FullName);
        base.BeginRender(writer);
    }

    protected override void EndRender(HtmlTextWriter writer)
    {
        base.EndRender(writer);
        this.Page.Trace.Write("Rendering Complete for :" + this.Control.ID + " - " + this.Control.GetType().FullName);
    }
}