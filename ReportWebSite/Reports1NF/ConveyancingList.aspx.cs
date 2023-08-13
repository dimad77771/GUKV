using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports1NF_ConveyancingList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		IsBigBossUser = Utils.IsBigBossUserToString();
		PrimaryGridView.AllColumns["balans_id"].SetGridViewColumnCssClass("bigBossUserStyle");
	}
	protected string IsBigBossUser { get; set; }

    protected void ObjectDataSourcePhotoFiles_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        if (Request.Cookies["RecordID"] != null)
            e.InputParameters["RecordID"] = Request.Cookies["RecordID"].Value;
    }
}
