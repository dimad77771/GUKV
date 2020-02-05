using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtDataEntry.Services;
using DevExpress.Web.Data;

namespace ExtDataEntry.Arenda
{
    public partial class Contest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@LastModifiedOn"].Value = DateTime.Now;
            e.Command.Parameters["@LastModifiedBy"].Value = AuthService.CurrentUserID;
            if (!string.IsNullOrEmpty(e.Command.Parameters["@AddrStreet"].Value as string))
            {
                e.Command.Parameters["@GeocodingId"].Value = GeocodingService.FindOrCreateGeocoding(
                    null,
                    e.Command.Parameters["@AddrStreet"].Value as string,
                    e.Command.Parameters["@AddrNumber"].Value as string
                    );
            }
        }

        protected void SqlDataSource1_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@LastModifiedOn"].Value = DateTime.Now;
            e.Command.Parameters["@LastModifiedBy"].Value = AuthService.CurrentUserID;
        }

        protected void ASPxGridView1_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewValues["AddrStreet"] as string))
            {
                e.NewValues["GeocodingId"] = null;
            }
            else if ((e.OldValues["AddrStreet"] as string) != (e.NewValues["AddrStreet"] as string)
                || (e.OldValues["AddrNumber"] as string) != (e.NewValues["AddrNumber"] as string)
                || e.NewValues["GeocodingId"] == null)
            {
                e.NewValues["GeocodingId"] = GeocodingService.FindOrCreateGeocoding(
                    null, e.NewValues["AddrStreet"] as string, e.NewValues["AddrNumber"] as string);
            }
        }
    }
}