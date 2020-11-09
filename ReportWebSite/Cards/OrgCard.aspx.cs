using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web;

public partial class Cards_OrgCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string orgIdStr = Request.QueryString["orgid"];

        orgIdStr = (orgIdStr == null) ? "" : orgIdStr.Trim();

        if (orgIdStr != null && orgIdStr.Length > 0)
        {
            int organizationId = int.Parse(orgIdStr);

            // Set the parameters for SQL queries
            SqlDataSourceOrgCardProperties.SelectParameters["org_id"].DefaultValue = organizationId.ToString();
            SqlDataSourceOrgCardBalans.SelectParameters["org_id"].DefaultValue = organizationId.ToString();
            SqlDataSourceOrgCardArenda.SelectParameters["org_id"].DefaultValue = organizationId.ToString();
            SqlDataSourceOrgCardArendaGiven.SelectParameters["org_id"].DefaultValue = organizationId.ToString();
            SqlDataSourceOrgCardPrivat.SelectParameters["org_id"].DefaultValue = organizationId.ToString();
            SqlDataSourceOrgCardOrendnaPlataByRenter.SelectParameters["org_id"].DefaultValue = organizationId.ToString();
            SqlDataSourceOrgCardOrendnaPlataByBalans.SelectParameters["org_id"].DefaultValue = organizationId.ToString();
            SqlDataSourceOrgArchive.SelectParameters["org_id"].DefaultValue = organizationId.ToString();
            SqlDataSourceNormativniDocumenti.SelectParameters["org_id"].DefaultValue = organizationId.ToString();
        }

        // Check if user has permissions to view this address
        if (PermissionGranted == 0 && Utils.RdaDistrictID > 0 && !string.IsNullOrEmpty(orgIdStr))
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                using (SqlCommand cmd = new SqlCommand("SELECT addr_distr_new_id FROM view_organizations WHERE organization_id = @org", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("org", int.Parse(orgIdStr)));

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
            Response.Redirect(Page.ResolveClientUrl("~/Account/RestrictedOrg.aspx"));
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

    protected void ASPxButtonPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect(Page.ResolveClientUrl("~/Cards/PrintOrgCard.aspx" + Request.Url.Query));
    }

    protected void SqlDataSourceOrgCardBalans_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@show_del"].Value = CheckBalansShowDeleted.Checked ? 1 : 0;
    }

    protected void SqlDataSourceOrgCardArenda_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@show_del"].Value = CheckArendaShowDeleted.Checked ? 1 : 0;
    }

    protected void SqlDataSourceOrgCardArendaGiven_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@show_del"].Value = CheckArendaGivenShowDeleted.Checked ? 1 : 0;
    }

    protected void GridViewOrgCardBalans_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        GridViewOrgCardBalans.DataSourceID = "SqlDataSourceOrgCardBalans";
        GridViewOrgCardBalans.DataBind();
    }

    protected void GridViewOrgCardBalans_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.Data)
        {
            object deleted = e.GetValue("is_deleted") != DBNull.Value ? Convert.ToInt32(e.GetValue("is_deleted")) : 0;

            if (deleted is int && (int)deleted == 1)
            {
                e.Row.BackColor = System.Drawing.Color.LightGray;
            }
        }
    }

    protected void GridViewObjCardArenda_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        GridViewOrgCardArenda.DataSourceID = "SqlDataSourceOrgCardArenda";
        GridViewOrgCardArenda.DataBind();
    }

    protected void GridViewOrgCardArenda_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.Data)
        {
            object active = Convert.ToInt32(e.GetValue("agreement_active_int"));

            if (active is int && (int)active == 0)
            {
                e.Row.BackColor = System.Drawing.Color.LightGray;
            }
        }
    }

    protected void GridViewOrgCardArendaGiven_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        GridViewOrgCardArendaGiven.DataSourceID = "SqlDataSourceOrgCardArendaGiven";
        GridViewOrgCardArendaGiven.DataBind();
    }

    protected void GridViewOrgCardArendaGiven_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.Data)
        {
            object active = Convert.ToInt32(e.GetValue("agreement_active_int"));

            if (active is int && (int)active == 0)
            {
                e.Row.BackColor = System.Drawing.Color.LightGray;
            }
        }
    }

    protected void GridViewNormativniDocumenti_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        GridViewOrgCardBalans.DataSourceID = "SqlDataSourceNormativniDocumenti";
        GridViewOrgCardBalans.DataBind();
    }

    /*
    protected void GridViewOrgCardRenterArchiveStates_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        SqlDataSourceArendaArchiveForRenter.SelectParameters["arid"].DefaultValue = e.Parameters.Trim();

        GridViewOrgCardRenterArchiveStates.DataSourceID = "SqlDataSourceArendaArchiveForRenter";
        GridViewOrgCardRenterArchiveStates.DataBind();
    }

    protected void GridViewOrgCardGiverArchiveStates_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        SqlDataSourceArendaArchiveForGiver.SelectParameters["arid"].DefaultValue = e.Parameters.Trim();

        GridViewOrgCardGiverArchiveStates.DataSourceID = "SqlDataSourceArendaArchiveForGiver";
        GridViewOrgCardGiverArchiveStates.DataBind();
    }

    protected void GridViewOrgCardBalansArchiveStates_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        SqlDataSourceBalansArchive.SelectParameters["balid"].DefaultValue = e.Parameters.Trim();

        GridViewOrgCardBalansArchiveStates.DataSourceID = "SqlDataSourceBalansArchive";
        GridViewOrgCardBalansArchiveStates.DataBind();
    }
    */

    #region Data binding support

    public string EvaluateObjectLink(object buildingId, object address)
    {
        if (!(address is string))
        {
            return "";
        }

        if (!(buildingId is int))
        {
            return address.ToString();
        }

        return "<a href=\"javascript:ShowObjectCardSimple(" + buildingId.ToString() + ")\">" + address.ToString() + "</a>";
    }

    public string EvaluateOrganizationLink(object organizationId, object name)
    {
        if (!(name is string))
        {
            return "";
        }

        if (!(organizationId is int))
        {
            return name.ToString();
        }

        return "<a href=\"javascript:ShowOrganizationCard(" + organizationId.ToString() + ")\">" + name.ToString() + "</a>";
    }

    public string EvaluateSignature(object modifiedBy, object modifyDate)
    {
        string userName = (modifiedBy is string) ? (string)modifiedBy : Resources.Strings.SignatureUnknownUser;
        string date = (modifyDate is DateTime) ? ((DateTime)modifyDate).ToShortDateString() : Resources.Strings.SignatureUnknownDate;

        return string.Format(Resources.Strings.SignatureObjCard, userName, date);
    }

    #endregion (Data binding support)
}