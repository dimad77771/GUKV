using DevExpress.Web;
using StackExchange.Profiling.Helpers.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NoMenu : System.Web.UI.MasterPage
{
    Color RED_COLOR = Color.FromArgb(255, 0, 0);
    Color GREEN_COLOR = Color.FromArgb(0, 255, 0);

    protected void Page_Load(object sender, EventArgs e)
    {
        Utils.FindUserOrganizationAndRda("");

        CustomizePhoneText();
    }

    protected void MainHelperPanel_Callback(object sender, CallbackEventArgsBase e)
    {
        var user = Utils.GetUser();
        if (user != @"О.Синенко") return;

        var connection = Utils.ConnectToDatabase(); 
        if (connection == null) return;

        if (e.Parameter == "MainHelperPanel_PhoneText_1")
        {
            connection.Execute("update etc_phone_info set good_phone_1 = case when good_phone_1 = 1 then 0 else 1 end");
        }
        else if (e.Parameter == "MainHelperPanel_PhoneText_2")
        {
            connection.Execute("update etc_phone_info set good_phone_2 = case when good_phone_2 = 1 then 0 else 1 end");
        }
        else if (e.Parameter == "MainHelperPanel_PhoneText_3")
        {
            connection.Execute("update etc_phone_info set good_phone_3 = case when good_phone_3 = 1 then 0 else 1 end");
        }

        CustomizePhoneText();
    }

    void CustomizePhoneText()
	{
        var connection = Utils.ConnectToDatabase();
        if (connection == null) return;

        using (var cmd = new SqlCommand("SELECT * FROM etc_phone_info", connection))
        {
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var phone_1 = reader.GetBoolean(0);
                    var phone_2 = reader.GetBoolean(1);
                    var phone_3 = reader.GetBoolean(2);

                    PhoneText_1.ForeColor = phone_1 ? GREEN_COLOR : RED_COLOR;
                    PhoneText_2.ForeColor = phone_2 ? GREEN_COLOR : RED_COLOR;
                    PhoneText_3.ForeColor = phone_3 ? GREEN_COLOR : RED_COLOR;
                }

                reader.Close();
            }
        }

        connection.Dispose();
    }
}
