using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Test_RemoveBuildingDuplicates : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlConnection conn = Utils.ConnectToDatabase();

        DB.view_buildingsDataTable tab = new DBTableAdapters.view_buildingsTableAdapter().GetData();
        foreach (DB.view_buildingsRow row in tab)
        {
            //"SELECT * FROM "
        }

        
    }

}