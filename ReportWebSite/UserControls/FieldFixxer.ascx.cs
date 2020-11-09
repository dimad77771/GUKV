using System;
using System.Web.UI;
using DevExpress.Web;
using DevExpress.Web;

public partial class UserControls_FieldFixxer : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void CallbackPanelGridColumns_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        ASPxGridView grid = FindGridRecursive(Page);

        if (grid != null)
        {
            Utils.GenerateFieldFixxerNodes(ListBoxGridColumns1, grid, e.Parameter);
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
                        Utils.GenerateFieldFixxerNodes(ListBoxGridColumns1, child as ASPxGridView, e.Parameter);
                        break;
                    }
                }
            }
        }
    }

    protected void SaveFixxedColumns_Click(object sender, EventArgs e)
    {
        ASPxGridView grid = FindGridRecursive(Page);

        if (grid != null)
        {
            Utils.ProcessDataGridSaveLayoutCallback(hdnSelectedFieldsUnderFix_FieldFixxer.Value, grid, 0, string.Empty);
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
                        Utils.ProcessDataGridSaveLayoutCallback(hdnSelectedFieldsUnderFix_FieldFixxer.Value, child as ASPxGridView, 0, string.Empty);
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

            DevExpress.Web.ASPxPageControl p = FindPageControl(child);

            if (p != null)
            {
                return p;
            }
        }

        return null;
    }

    private ASPxGridView FindGridRecursive(Control root)
    {
        if (root is ASPxGridView)
        {
            return root as ASPxGridView;
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