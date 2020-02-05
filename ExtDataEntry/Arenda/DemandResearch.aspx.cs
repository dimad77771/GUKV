using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using System.IO;
using ExtDataEntry.Models;
using DevExpress.Web.Data;
using ExtDataEntry.Services;

namespace ExtDataEntry.Arenda
{
    public partial class DemandResearch : System.Web.UI.Page
    {
        public const string FileAttachmentTag = "ExtRentDemandResearch";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ObjectDataSource1_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (Request.Cookies["RecordID"] != null)
                e.InputParameters["RecordID"] = Request.Cookies["RecordID"].Value;
        }

        protected void ObjectDataSource1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            // SELECTing appears to bind its parameter values from cookies just fine
        }

        protected void ObjectDataSource1_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            //if (Request.Cookies["RecordID"] != null)
            //    e.InputParameters["id"] = Request.Cookies["RecordID"].Value;
        }

        protected void SqlDataSource1_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null || e.ExceptionHandled)
                FileAttachment.DeleteAll(FileAttachmentTag, (int)e.Command.Parameters["@Id"].Value);
        }

        protected void ASPxGridView1_InitNewRow(object sender, ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ExpirationDate"] = AddBusinessDays(DateTime.Now.Date, 10);
        }
    
        public static DateTime AddBusinessDays(DateTime date, int days)
        {
            if (days < 0)
            {
                throw new ArgumentException("days cannot be negative", "days");
            }

            if (days == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            return date.AddDays(extraDays);

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