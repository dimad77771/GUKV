using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web;

public partial class Cards_ObjCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string buildingIdStr = Request.QueryString["bid"];
        string balansIdStr = Request.QueryString["balid"];

        buildingIdStr = (buildingIdStr == null) ? "" : buildingIdStr.Trim();
        balansIdStr = (balansIdStr == null) ? "" : balansIdStr.Trim();

        if (!IsPostBack && buildingIdStr != null && buildingIdStr.Length > 0)
        {
            SqlDataSourceObjCardProperties.SelectParameters["bid"].DefaultValue = buildingIdStr;
            SqlDataSourceObjCardArenda.SelectParameters["bid"].DefaultValue = buildingIdStr;
            SqlDataSourceObjCardBalans.SelectParameters["bid"].DefaultValue = buildingIdStr;
            SqlDataSourceObjCardPurchase.SelectParameters["bid"].DefaultValue = buildingIdStr;
            SqlDataSourceObjCardPrivat.SelectParameters["bid"].DefaultValue = buildingIdStr;
            SqlDataSourceObjCardTransfer.SelectParameters["bid"].DefaultValue = buildingIdStr;
            SqlDataSourceObjArchive.SelectParameters["bid"].DefaultValue = buildingIdStr;
        }

        // Check if user has permissions to view this address
        if (PermissionGranted == 0 && Utils.RdaDistrictID > 0 && !string.IsNullOrEmpty(buildingIdStr))
        {
            SqlConnection connection = Utils.ConnectToDatabase();

            if (connection != null)
            {
                using (SqlCommand cmd = new SqlCommand("SELECT addr_distr_new_id FROM view_buildings WHERE building_id = @bid", connection))
                {
                    cmd.Parameters.Add(new SqlParameter("bid", int.Parse(buildingIdStr)));

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

    protected void ASPxButtonPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect(Page.ResolveClientUrl("~/Cards/PrintObjCard.aspx" + Request.Url.Query));
    }

    public string EvaluateSignature(object modifiedBy, object modifyDate)
    {
        string userName = (modifiedBy is string) ? (string)modifiedBy : Resources.Strings.SignatureUnknownUser;
        string date = (modifyDate is DateTime) ? ((DateTime)modifyDate).ToShortDateString() : Resources.Strings.SignatureUnknownDate;

        return string.Format(Resources.Strings.SignatureObjCard, userName, date);
    }

    protected void SqlDataSourceObjCardBalans_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@show_del"].Value = CheckBalansShowDeleted.Checked ? 1 : 0;
    }

    protected void SqlDataSourceObjCardArenda_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@show_del"].Value = CheckArendaShowDeleted.Checked ? 1 : 0;
    }

    protected void GridViewObjCardBalans_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        GridViewObjCardBalans.DataSourceID = "SqlDataSourceObjCardBalans";
        GridViewObjCardBalans.DataBind();
    }

    protected void GridViewObjCardBalans_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.Data)
        {
            object deleted = Convert.ToInt32(e.GetValue("is_deleted"));

            if (deleted is int && (int)deleted == 1)
            {
                e.Row.BackColor = System.Drawing.Color.LightGray;
            }
        }
    }

    protected void GridViewObjCardArenda_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        GridViewObjCardArenda.DataSourceID = "SqlDataSourceObjCardArenda";
        GridViewObjCardArenda.DataBind();
    }

    protected void GridViewObjCardArenda_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
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

    protected void GridViewObjCardArenda_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
    {
        Utils.CustomSummaryExcludeSubarenda(e);
    }
}