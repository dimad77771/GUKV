using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;


public partial class UserControls_FieldChooser : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	public class ParameterJson
	{
		public string gridId { get; set; }
		public string captionPattern { get; set; }
	}


	protected void CallbackPanelGridColumns_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
		var gridId = "";
        var captionPattern = e.Parameter ?? "";
        var s1 = "JSON=";
		if (captionPattern.StartsWith(s1))
		{
			var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ParameterJson>(captionPattern.Substring(s1.Length));
			gridId = data.gridId ?? "";
			captionPattern = data.captionPattern ?? "";
		}

		ASPxGridView grid;
		if (string.IsNullOrEmpty(gridId))
        {
			grid = FindGridRecursive(Page);
		}
        else
        {
			grid = FindAllGridsRecursive(Page).Where(x => x.ID == gridId).FirstOrDefault();
		}

		

        if (grid != null)
        {
            Utils.GenerateFieldChooserNodes(ListBoxGridColumns, grid, captionPattern);
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
                        Utils.GenerateFieldChooserNodes(ListBoxGridColumns, child as ASPxGridView, captionPattern);
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

	private List<ASPxGridView> FindAllGridsRecursive(Control root)
	{
		var result = new List<ASPxGridView>();

		FindAllGridsInternal(root, result);

		return result;
	}

	private void FindAllGridsInternal(Control root, List<ASPxGridView> result)
	{
		var grid = root as ASPxGridView;

		if (grid != null)
		{
			if (!(root is DevExpress.Web.Internal.FileManagerGridView))
			{
				result.Add(grid);
			}
		}

		foreach (Control c in root.Controls)
		{
			FindAllGridsInternal(c, result);
		}
	}
}