﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;

public partial class UserControls_FieldChooser : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void CallbackPanelGridColumns_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        ASPxGridView grid = FindGridRecursive(Page);

        if (grid != null)
        {
            Utils.GenerateFieldChooserNodes(ListBoxGridColumns, grid, e.Parameter);
        }
        else
        {
            DevExpress.Web.ASPxPageControl pageControl = FindPageControl(Page.Master);

            if (pageControl != null)
            {
                // Find a grid in the active tab
                foreach (Control child in pageControl.ActiveTabPage.Controls)
                {
                    if (child is ASPxGridView)
                    {
                        Utils.GenerateFieldChooserNodes(ListBoxGridColumns, child as ASPxGridView, e.Parameter);
                        break;
                    }
                }
            }
        }
    }

    protected DevExpress.Web.ASPxPageControl FindPageControl(Control c)
    {
        foreach (Control child in c.Controls)
        {
            if (child is DevExpress.Web.ASPxPageControl)
            {
                return child as DevExpress.Web.ASPxPageControl;
            }
            else
            {
                DevExpress.Web.ASPxPageControl p = FindPageControl(child);

                if (p != null)
                {
                    return p;
                }
            }
        }

        return null;
    }

    private ASPxGridView FindGridRecursive(Control root)
    {
        if (root is ASPxGridView)
        {
            if (!(root is DevExpress.Web.Internal.FileManagerGridView))
            {
                return root as ASPxGridView;
            }
        }

        foreach (Control c in root.Controls)
        {
            ASPxGridView g = FindGridRecursive(c);

            if (g != null)
            {
                return g;
            }
        }

        return null;
    }
}