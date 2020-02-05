using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_CustomReportCreator : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void SqlDataSourceUserFolders_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        MembershipUser user = Membership.GetUser();
        object uid = (user != null) ? user.ProviderUserKey : null;

        if (uid is System.Guid)
        {
            e.Command.Parameters["@uid"].Value = (System.Guid)uid;
        }
        else
        {
            e.Command.Parameters["@uid"].Value = System.Guid.Empty;
        }
    }

    protected void CallbackPanelUserFolders_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        // This callback is always called to refresh the drop-down of folders

        object oldValue = ComboFolders.Value;

        ComboFolders.DataBind();

        // Try to select the item with the same value
        int indexToSelect = 0;

        if (oldValue is int)
        {
            for (int i = 0; i < ComboFolders.Items.Count; i++)
            {
                if ((int)ComboFolders.Items[i].Value == (int)oldValue)
                {
                    indexToSelect = i;
                    break;
                }
            }
        }

        ComboFolders.SelectedIndex = indexToSelect;
    }
}