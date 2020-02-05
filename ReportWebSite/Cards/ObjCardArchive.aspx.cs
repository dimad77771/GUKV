using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Cards_ObjCardArchive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string archiveIdStr = Request.QueryString["arid"];

        if (!IsPostBack && archiveIdStr != null && archiveIdStr.Length > 0)
        {
            SqlDataSourceObjCardProperties.SelectParameters["arid"].DefaultValue = archiveIdStr.Trim();
        }

        // Check if user has permissions to view this address
        if (PermissionGranted == 0 && Utils.RdaDistrictID > 0 && !string.IsNullOrEmpty(archiveIdStr))
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                int buildingId = 0;

                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 building_id FROM view_arch_buildings WHERE archive_id = @arid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("arid", int.Parse(archiveIdStr)));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            buildingId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        }

                        reader.Close();
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT addr_distr_new_id FROM view_buildings WHERE building_id = @bid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("bid", buildingId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.IsDBNull(0))
                            {
                                PermissionGranted = -1;
                            }
                            else
                            {
                                PermissionGranted = (reader.GetInt32(0) == Utils.RdaDistrictID) ? 1 : -1;
                            }
                        }
                        else
                        {
                            PermissionGranted = -1;
                        }

                        reader.Close();
                    }
                }

                connection.Close();
            }
        }

        if (PermissionGranted < 0)
        {
            Response.Redirect(Page.ResolveClientUrl("~/Account/RestrictedObject.aspx"));
        }
    }

    protected int PermissionGranted
    {
        get
        {
            object val = ViewState["PERMISSION_GRANTED"];

            if (val is int)
            {
                return (int)val;
            }

            return 0;
        }

        set
        {
            ViewState["PERMISSION_GRANTED"] = value;
        }
    }

    public string EvaluateSignature(object modifiedBy, object modifyDate)
    {
        string userName = (modifiedBy is string) ? (string)modifiedBy : Resources.Strings.SignatureUnknownUser;
        string date = (modifyDate is DateTime) ? ((DateTime)modifyDate).ToShortDateString() : Resources.Strings.SignatureUnknownDate;

        return string.Format(Resources.Strings.SignatureObjCard, userName, date);
    }
}