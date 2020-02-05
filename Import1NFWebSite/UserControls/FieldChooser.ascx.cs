using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;

public partial class UserControls_FieldChooser : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void CallbackPanelGridColumns_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        ASPxGridView grid = FindGridView(Page.Master);

        if (grid != null)
        {
            Utils.GenerateFieldChooserNodes(ListBoxGridColumns, grid, e.Parameter);
        }
    }

    protected ASPxGridView FindGridView(Control c)
    {
        foreach (Control child in c.Controls)
        {
            if (child is ASPxGridView)
            {
                return child as ASPxGridView;
            }
            else
            {
                ASPxGridView grid = FindGridView(child);

                if (grid != null)
                {
                    return grid;
                }
            }
        }

        return null;
    }
}