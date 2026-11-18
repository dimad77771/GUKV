using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Data;

public partial class RentInspections : UserControl
{
    public int ReportId { get; set; }
    public int AgreementId { get; set; }
    public bool CanEdit { get; set; }
    public Action<ASPxDataInitNewRowEventArgs> FillDefaults { get; set; }

    protected override void OnPreRender(EventArgs e)
    {
        var actions = (GridViewCommandColumn)Grid.Columns["Actions"];
        actions.ShowNewButton = actions.ShowEditButton = actions.ShowDeleteButton = CanEdit;
        Grid.JSProperties["cpActUrl"] = "OrgRentAgreement.aspx?rid=" + ReportId + "&aid=" + AgreementId + "&inspectionAct=";
        base.OnPreRender(e);
    }

    protected void InitInspection(object sender, ASPxDataInitNewRowEventArgs e)
    {
        if (CanEdit) FillDefaults(e);
    }

    private void AddScope(SqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@rid", ReportId);
        cmd.Parameters.AddWithValue("@aid", AgreementId);
    }

    protected void SelectInspection(object sender, SqlDataSourceSelectingEventArgs e)
    {
        AddScope((SqlCommand)e.Command);
    }

    protected void WriteInspection(object sender, SqlDataSourceCommandEventArgs e)
    {
        if (!CanEdit)
            throw new System.Web.HttpException(403, "Редагування заборонено");
        AddScope((SqlCommand)e.Command);
    }
}
