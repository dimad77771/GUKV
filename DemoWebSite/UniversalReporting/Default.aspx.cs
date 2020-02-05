using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using DevExpress.XtraPrinting;
using System.Text;
using System.IO;

public partial class UniversalReporting_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    #region Buildings report

    private int ExpandedBuildingID
    {
        get { return Session["ExpandedBuildingID"] is int ? (int)Session["ExpandedBuildingID"] : 0; }
        set { Session["ExpandedBuildingID"] = value; }
    }

    protected void ASPxGridViewObjects_BeforePerformDataSelect(object sender, EventArgs e)
    {
        ASPxGridView ASPxGridViewObjects = (ASPxGridView)sender;

        ExpandedBuildingID = (int)ASPxGridViewObjects.GetMasterRowKeyValue();
    }

    protected void SqlDataSourceBuildingDetail_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@building_id"].Value = ExpandedBuildingID;
    }

    protected void ASPxButton_Buildings_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterBuildings);
    }

    protected void ASPxButton_Buildings_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterBuildings);
    }

    protected void ASPxButton_Buildings_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterBuildings);
    }

    #endregion (Buildings report)

    #region Balans report

    private int ExpandedBalansOrgID
    {
        get { return Session["ExpandedBalansOrgID"] is int ? (int)Session["ExpandedBalansOrgID"] : 0; }
        set { Session["ExpandedBalansOrgID"] = value; }
    }

    protected void ASPxGridViewBalansDetail_BeforePerformDataSelect(object sender, EventArgs e)
    {
        ASPxGridView ASPxGridViewBalansDetail = (ASPxGridView)sender;

        ExpandedBalansOrgID = (int)ASPxGridViewBalansDetail.GetMasterRowKeyValue();
    }

    protected void SqlDataSourceBalansDetail_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@organization_id"].Value = ExpandedBalansOrgID;
    }

    protected void ASPxButton_Balans_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterBalans);
    }

    protected void ASPxButton_Balans_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterBalans);
    }

    protected void ASPxButton_Balans_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterBalans);
    }

    #endregion (Balans report)

    #region Arenda report

    private int ExpandedArendaOrgID
    {
        get { return Session["ExpandedArendaOrgID"] is int ? (int)Session["ExpandedArendaOrgID"] : 0; }
        set { Session["ExpandedArendaOrgID"] = value; }
    }

    protected void ASPxGridViewArendaDetail_BeforePerformDataSelect(object sender, EventArgs e)
    {
        ASPxGridView ASPxGridViewArendaDetail = (ASPxGridView)sender;

        ExpandedArendaOrgID = (int)ASPxGridViewArendaDetail.GetMasterRowKeyValue();
    }

    protected void SqlDataSourceArendaDetail_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@organization_id"].Value = ExpandedArendaOrgID;
    }

    protected void ASPxButton_Arenda_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterArenda);
    }

    protected void ASPxButton_Arenda_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterArenda);
    }

    protected void ASPxButton_Arenda_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterArenda);
    }

    #endregion (Arenda report)

    #region Global report

    protected void ASPxButton_Global_ExportXLS_Click(object sender, EventArgs e)
    {
        this.ExportGridToXLS(ASPxGridViewExporterGlobal);
    }

    protected void ASPxButton_Global_ExportPDF_Click(object sender, EventArgs e)
    {
        this.ExportGridToPDF(ASPxGridViewExporterGlobal);
    }

    protected void ASPxButton_Global_ExportCSV_Click(object sender, EventArgs e)
    {
        this.ExportGridToCSV(ASPxGridViewExporterGlobal);
    }

    #endregion (Global report)
}