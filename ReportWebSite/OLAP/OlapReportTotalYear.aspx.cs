using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using DevExpress.Web;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;

public partial class Assessment_AssessmentObjects : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
	    // Generate a unique key for this instance of the page
        GetPageUniqueKey();

		if (!IsPostBack)
		{
			pivotGrid.CollapseAll();
		}
	}


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


}