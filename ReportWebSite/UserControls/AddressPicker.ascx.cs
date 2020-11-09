using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web;

public partial class UserControls_AddressPicker : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private int AddressPickerStreetID
    {
        get { return Session["AddressPickerStreetID"] is int ? (int)Session["AddressPickerStreetID"] : 0; }
        set { Session["AddressPickerStreetID"] = value; }
    }

    protected void ComboAddrPickerBuilding_Callback(object source, CallbackEventArgsBase e)
    {
        try
        {
            int streetId = int.Parse(e.Parameter);

            AddressPickerStreetID = streetId;

            (source as ASPxComboBox).DataBind();
        }
        finally
        {
        }
    }

    protected void SqlDataSourceDictBuildings_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@street_id"].Value = AddressPickerStreetID;
    }
}