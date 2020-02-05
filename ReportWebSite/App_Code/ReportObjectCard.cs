using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for XtraReport1
/// </summary>
public class ReportObjectCard : DevExpress.XtraReports.UI.XtraReport
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Contains a document list for each balans_id related to the displayed object
    /// </summary>
    private Dictionary<int, string> balansDocuments = new Dictionary<int, string>();

    /// <summary>
    /// Contains all information about rent applicants that have no agreement
    /// </summary>
    private Dictionary<int, RenterNoAgreementInfo> rentersInfo = new Dictionary<int, RenterNoAgreementInfo>();

    /// <summary>
    /// ID of the building which is displayed in this report
    /// </summary>
    private int buildingId = -1;

    /// <summary>
    /// Total square from all active rent agreements
    /// </summary>
    private decimal totalRentedSquare = 0m;

	private DevExpress.XtraReports.UI.DetailBand Detail;
	private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
	private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private ReportHeaderBand ReportHeader;
    private DB db1;
    private XRLabel xrLabel3;
    private DevExpress.XtraReports.Parameters.Parameter Date;
    private XRLabel xrLabel1;
    private XRLabel xrLabel4;
    private XRLabel xrLabel2;
    private XRLabel xrLabel8;
    private XRLabel xrLabel7;
    private XRLabel xrLabel6;
    private XRLabel xrLabel14;
    private XRLabel xrLabel13;
    private XRLabel xrLabel12;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private XRLabel xrLabel9;
    private XRLabel xrLabel18;
    private XRLabel xrLabel17;
    private XRLabel xrLabel16;
    private XRLabel xrLabel15;
    private XRLabel xrLabel19;
    private XRLabel xrLabel20;
    private XRLabel xrLabel22;
    private XRLabel xrLabel21;
    private XRLabel xrLabel24;
    private XRLabel xrLabel23;
    private XRLabel xrLabel25;
    private XRLabel xrLabel36;
    private XRLabel xrLabel34;
    private XRLabel xrLabel32;
    private XRLabel xrLabel31;
    private XRLabel xrLabel29;
    private XRLabel xrLabel37;
    private XRLabel xrLabel47;
    private XRLabel xrLabel46;
    private XRLabel xrLabel44;
    private XRLabel xrLabel40;
    private XRLabel xrLabel39;
    private XRLabel xrLabel38;
    private XRLabel xrLabel50;
    private CalculatedField FullStreetName;
    private CalculatedField AgreementNumberAndDate;
    private CalculatedField AgreementStartEndDates;
    private DetailReportBand DetailReport;
    private DetailBand Detail1;
    private XRLabel xrLabel5;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell12;
    private DetailReportBand DetailReport2;
    private DetailBand Detail3;
    private XRTable xrTable2;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private ReportHeaderBand ReportHeader1;
    private XRTable xrTable3;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell50;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell51;
    private XRTableCell xrTableCell52;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell55;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell58;
    private XRLabel xrLabel51;
    private GroupHeaderBand GroupHeader1;
    private XRLabel xrLabel52;
    private ReportHeaderBand ReportHeader2;
    private XRTable xrTable4;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell30;
    private DetailReportBand DetailReport1;
    private DetailBand Detail2;
    private ReportHeaderBand ReportHeader3;
    private XRTable xrTable5;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell7;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private XRLabel xrLabel53;
    private XRTable xrTable6;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell32;
    private CalculatedField BalansDocList;
    private DetailReportBand DetailReport3;
    private DetailBand Detail4;
    private XRTable xrTable8;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell65;
    private XRTableCell xrTableCell73;
    private XRTableCell xrTableCell74;
    private XRTableCell xrTableCell75;
    private XRTableCell xrTableCell76;
    private XRTableCell xrTableCell77;
    private XRTableCell xrTableCell78;
    private ReportHeaderBand ReportHeader4;
    private XRTable xrTable7;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell60;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell62;
    private XRTableCell xrTableCell63;
    private XRTableCell xrTableCell64;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell67;
    private XRTableCell xrTableCell68;
    private XRTableCell xrTableCell69;
    private XRTableCell xrTableCell70;
    private XRTableCell xrTableCell71;
    private XRTableCell xrTableCell72;
    private XRLabel xrLabel54;
    private CalculatedField RentersNAName;
    private CalculatedField RentersNALetter;
    private CalculatedField RentersNAPurpose;
    private CalculatedField RentersNASqr;
    private CalculatedField RentersNATerm;
    private CalculatedField RentersNADecision;
    private CalculatedField KorisnaPloscha;
    private CalculatedField PloschaInshVlasnikiv;
    private CalculatedField RentedSqrTotal;
    private CalculatedField RentersNARishen;

    public ReportObjectCard(string buildingIdStr, string balansIdStr)
	{
        InitializeComponent();

        if (!string.IsNullOrEmpty(buildingIdStr) && int.TryParse(buildingIdStr, out buildingId) && buildingId > 0)
        {
            DB dataset = db1;

            (new DBTableAdapters.ObjCardPropertiesTableAdapter()).FillByBID(dataset.ObjCardProperties, buildingId);
            (new DBTableAdapters.ObjCardArendaTableAdapter()).FillByBID(dataset.ObjCardArenda, buildingId);
            (new DBTableAdapters.ObjCardPurchaseTableAdapter()).FillByBID(dataset.ObjCardPurchase, buildingId);
            (new DBTableAdapters.ObjCardPrivatTableAdapter()).FillByBID(dataset.ObjCardPrivat, buildingId);
            (new DBTableAdapters.ObjCardBalansTableAdapter()).FillByBID(dataset.ObjCardBalans, buildingId);
            (new DBTableAdapters.ObjCardRentersNoAgreementsTableAdapter()).FillByBID(dataset.ObjCardRentersNoAgreements, buildingId);

            FillBalansDocLists();
            FillRentersNoAgreementsData();
            CalcTotalRentedSquare();
        }
    }
	
	/// <summary> 
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing) {
		if (disposing && (components != null)) {
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent() {
//        string resourceFileName = "ReportObjectCard.resx";
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.Date = new DevExpress.XtraReports.Parameters.Parameter();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.db1 = new DB();
        this.FullStreetName = new DevExpress.XtraReports.UI.CalculatedField();
        this.AgreementNumberAndDate = new DevExpress.XtraReports.UI.CalculatedField();
        this.AgreementStartEndDates = new DevExpress.XtraReports.UI.CalculatedField();
        this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
        this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader2 = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.DetailReport2 = new DevExpress.XtraReports.UI.DetailReportBand();
        this.Detail3 = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
        this.DetailReport1 = new DevExpress.XtraReports.UI.DetailReportBand();
        this.Detail2 = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader3 = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
        this.BalansDocList = new DevExpress.XtraReports.UI.CalculatedField();
        this.DetailReport3 = new DevExpress.XtraReports.UI.DetailReportBand();
        this.Detail4 = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader4 = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
        this.RentersNAName = new DevExpress.XtraReports.UI.CalculatedField();
        this.RentersNALetter = new DevExpress.XtraReports.UI.CalculatedField();
        this.RentersNAPurpose = new DevExpress.XtraReports.UI.CalculatedField();
        this.RentersNASqr = new DevExpress.XtraReports.UI.CalculatedField();
        this.RentersNATerm = new DevExpress.XtraReports.UI.CalculatedField();
        this.RentersNADecision = new DevExpress.XtraReports.UI.CalculatedField();
        this.RentersNARishen = new DevExpress.XtraReports.UI.CalculatedField();
        this.KorisnaPloscha = new DevExpress.XtraReports.UI.CalculatedField();
        this.PloschaInshVlasnikiv = new DevExpress.XtraReports.UI.CalculatedField();
        this.RentedSqrTotal = new DevExpress.XtraReports.UI.CalculatedField();
        ((System.ComponentModel.ISupportInitialize)(this.db1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel50,
            this.xrLabel47,
            this.xrLabel46,
            this.xrLabel44,
            this.xrLabel40,
            this.xrLabel39,
            this.xrLabel38,
            this.xrLabel37,
            this.xrLabel36,
            this.xrLabel34,
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel29,
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel15,
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6});
        this.Detail.HeightF = 201.125F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel50
        // 
        this.xrLabel50.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel50.Name = "xrLabel50";
        this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel50.SizeF = new System.Drawing.SizeF(278.125F, 23F);
        this.xrLabel50.StylePriority.UseFont = false;
        this.xrLabel50.StylePriority.UseTextAlignment = false;
        this.xrLabel50.Text = "I. Загальна  характеристика:";
        this.xrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel47
        // 
        this.xrLabel47.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel47.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel47.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.sqr_object_other")});
        this.xrLabel47.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(862.218F, 143.3749F);
        this.xrLabel47.Name = "xrLabel47";
        this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel47.SizeF = new System.Drawing.SizeF(96.78192F, 18F);
        this.xrLabel47.StylePriority.UseBorderDashStyle = false;
        this.xrLabel47.StylePriority.UseBorders = false;
        this.xrLabel47.StylePriority.UseFont = false;
        this.xrLabel47.Text = "xrLabel47";
        // 
        // xrLabel46
        // 
        this.xrLabel46.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel46.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.sqr_object_dk")});
        this.xrLabel46.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(862.218F, 125.3749F);
        this.xrLabel46.Name = "xrLabel46";
        this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel46.SizeF = new System.Drawing.SizeF(96.7821F, 17.99999F);
        this.xrLabel46.StylePriority.UseBorderDashStyle = false;
        this.xrLabel46.StylePriority.UseBorders = false;
        this.xrLabel46.StylePriority.UseFont = false;
        this.xrLabel46.Text = "xrLabel46";
        // 
        // xrLabel44
        // 
        this.xrLabel44.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel44.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel44.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.PloschaInshVlasnikiv")});
        this.xrLabel44.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(862.218F, 107.3749F);
        this.xrLabel44.Name = "xrLabel44";
        this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel44.SizeF = new System.Drawing.SizeF(96.78192F, 18F);
        this.xrLabel44.StylePriority.UseBorderDashStyle = false;
        this.xrLabel44.StylePriority.UseBorders = false;
        this.xrLabel44.StylePriority.UseFont = false;
        this.xrLabel44.Text = "xrLabel44";
        // 
        // xrLabel40
        // 
        this.xrLabel40.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel40.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.sqr_habit")});
        this.xrLabel40.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(862.218F, 89.37495F);
        this.xrLabel40.Name = "xrLabel40";
        this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel40.SizeF = new System.Drawing.SizeF(96.78192F, 18F);
        this.xrLabel40.StylePriority.UseBorderDashStyle = false;
        this.xrLabel40.StylePriority.UseBorders = false;
        this.xrLabel40.StylePriority.UseFont = false;
        this.xrLabel40.Text = "xrLabel40";
        // 
        // xrLabel39
        // 
        this.xrLabel39.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel39.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.KorisnaPloscha")});
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardBalans.sqr_gromady")});
        this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(862.2181F, 71.37495F);
        this.xrLabel39.Name = "xrLabel39";
        this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel39.SizeF = new System.Drawing.SizeF(96.78192F, 18.00001F);
        this.xrLabel39.StylePriority.UseBorderDashStyle = false;
        this.xrLabel39.StylePriority.UseBorders = false;
        this.xrLabel39.StylePriority.UseFont = false;
        this.xrLabel39.Text = "xrLabel39";
        // 
        // xrLabel38
        // 
        this.xrLabel38.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel38.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.RentedSqrTotal")});
        this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(862.2192F, 161.3749F);
        this.xrLabel38.Name = "xrLabel38";
        this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel38.SizeF = new System.Drawing.SizeF(95.46F, 18F);
        this.xrLabel38.StylePriority.UseBorderDashStyle = false;
        this.xrLabel38.StylePriority.UseBorders = false;
        this.xrLabel38.StylePriority.UseFont = false;
        this.xrLabel38.Text = "xrLabel38";
        // 
        // xrLabel37
        // 
        this.xrLabel37.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel37.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.sqr_not_for_rent")});
        this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(862.2181F, 179.375F);
        this.xrLabel37.Name = "xrLabel37";
        this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel37.SizeF = new System.Drawing.SizeF(96.78186F, 18F);
        this.xrLabel37.StylePriority.UseBorderDashStyle = false;
        this.xrLabel37.StylePriority.UseBorders = false;
        this.xrLabel37.StylePriority.UseFont = false;
        this.xrLabel37.Text = "xrLabel37";
        // 
        // xrLabel36
        // 
        this.xrLabel36.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel36.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(415.3381F, 179.375F);
        this.xrLabel36.Name = "xrLabel36";
        this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel36.SizeF = new System.Drawing.SizeF(446.88F, 18F);
        this.xrLabel36.StylePriority.UseBorderDashStyle = false;
        this.xrLabel36.StylePriority.UseBorders = false;
        this.xrLabel36.StylePriority.UseFont = false;
        this.xrLabel36.Text = "Площа, що не може бути надана в оренду, кв.м";
        // 
        // xrLabel34
        // 
        this.xrLabel34.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel34.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(415.3381F, 161.3749F);
        this.xrLabel34.Name = "xrLabel34";
        this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel34.SizeF = new System.Drawing.SizeF(446.88F, 18F);
        this.xrLabel34.StylePriority.UseBorderDashStyle = false;
        this.xrLabel34.StylePriority.UseBorders = false;
        this.xrLabel34.StylePriority.UseFont = false;
        this.xrLabel34.Text = "Площа тер. громади м.Києва, що надається в оренду, кв.м";
        // 
        // xrLabel32
        // 
        this.xrLabel32.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(415.3381F, 143.3749F);
        this.xrLabel32.Name = "xrLabel32";
        this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel32.SizeF = new System.Drawing.SizeF(446.88F, 18F);
        this.xrLabel32.StylePriority.UseBorderDashStyle = false;
        this.xrLabel32.StylePriority.UseBorders = false;
        this.xrLabel32.StylePriority.UseFont = false;
        this.xrLabel32.Text = "        інших форм власності, кв.м";
        // 
        // xrLabel31
        // 
        this.xrLabel31.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(415.3382F, 125.3749F);
        this.xrLabel31.Name = "xrLabel31";
        this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel31.SizeF = new System.Drawing.SizeF(446.88F, 18F);
        this.xrLabel31.StylePriority.UseBorderDashStyle = false;
        this.xrLabel31.StylePriority.UseBorders = false;
        this.xrLabel31.StylePriority.UseFont = false;
        this.xrLabel31.Text = "        державної форми власності, кв.м";
        // 
        // xrLabel29
        // 
        this.xrLabel29.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(415.3442F, 107.3749F);
        this.xrLabel29.Name = "xrLabel29";
        this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel29.SizeF = new System.Drawing.SizeF(446.88F, 18F);
        this.xrLabel29.StylePriority.UseBorderDashStyle = false;
        this.xrLabel29.StylePriority.UseBorders = false;
        this.xrLabel29.StylePriority.UseFont = false;
        this.xrLabel29.Text = "Площа інших власників, кв.м";
        // 
        // xrLabel25
        // 
        this.xrLabel25.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(415.3388F, 89.37495F);
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(446.88F, 18F);
        this.xrLabel25.StylePriority.UseBorderDashStyle = false;
        this.xrLabel25.StylePriority.UseBorders = false;
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.Text = "в т.ч. житлового фонду, кв.м";
        // 
        // xrLabel24
        // 
        this.xrLabel24.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(415.3391F, 71.37497F);
        this.xrLabel24.Name = "xrLabel24";
        this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel24.SizeF = new System.Drawing.SizeF(446.88F, 18F);
        this.xrLabel24.StylePriority.UseBorderDashStyle = false;
        this.xrLabel24.StylePriority.UseBorders = false;
        this.xrLabel24.StylePriority.UseFont = false;
        this.xrLabel24.Text = "        - з них площа територіальної громади м. Києва, кв.м";
        // 
        // xrLabel23
        // 
        this.xrLabel23.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.sqr_non_habit")});
        this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(862.2192F, 53.37503F);
        this.xrLabel23.Name = "xrLabel23";
        this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel23.SizeF = new System.Drawing.SizeF(96.78094F, 17.99999F);
        this.xrLabel23.StylePriority.UseBorderDashStyle = false;
        this.xrLabel23.StylePriority.UseBorders = false;
        this.xrLabel23.StylePriority.UseFont = false;
        this.xrLabel23.Text = "xrLabel23";
        // 
        // xrLabel22
        // 
        this.xrLabel22.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(415.3442F, 53.37499F);
        this.xrLabel22.Name = "xrLabel22";
        this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel22.SizeF = new System.Drawing.SizeF(446.88F, 18F);
        this.xrLabel22.StylePriority.UseBorderDashStyle = false;
        this.xrLabel22.StylePriority.UseBorders = false;
        this.xrLabel22.StylePriority.UseFont = false;
        this.xrLabel22.Text = "в т.ч. нежилих приміщень, кв.м";
        // 
        // xrLabel21
        // 
        this.xrLabel21.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.sqr_total")});
        this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(862.2192F, 35.37502F);
        this.xrLabel21.Name = "xrLabel21";
        this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel21.SizeF = new System.Drawing.SizeF(96.78094F, 18F);
        this.xrLabel21.StylePriority.UseBorderDashStyle = false;
        this.xrLabel21.StylePriority.UseBorders = false;
        this.xrLabel21.StylePriority.UseFont = false;
        this.xrLabel21.Text = "xrLabel21";
        // 
        // xrLabel20
        // 
        this.xrLabel20.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(415.3441F, 35.37502F);
        this.xrLabel20.Name = "xrLabel20";
        this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel20.SizeF = new System.Drawing.SizeF(446.88F, 18F);
        this.xrLabel20.StylePriority.UseBorderDashStyle = false;
        this.xrLabel20.StylePriority.UseBorders = false;
        this.xrLabel20.StylePriority.UseFont = false;
        this.xrLabel20.Text = "Загальна площа будинку, кв.м";
        // 
        // xrLabel19
        // 
        this.xrLabel19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.history")});
        this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(191.3858F, 143.3749F);
        this.xrLabel19.Name = "xrLabel19";
        this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel19.SizeF = new System.Drawing.SizeF(200F, 18F);
        this.xrLabel19.StylePriority.UseFont = false;
        this.xrLabel19.Text = "xrLabel19";
        // 
        // xrLabel18
        // 
        this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(38.2608F, 143.3749F);
        this.xrLabel18.Name = "xrLabel18";
        this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel18.SizeF = new System.Drawing.SizeF(153.125F, 18F);
        this.xrLabel18.StylePriority.UseFont = false;
        this.xrLabel18.Text = "Пам’ятка історії:";
        // 
        // xrLabel17
        // 
        this.xrLabel17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.construct_year")});
        this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(191.3858F, 125.3749F);
        this.xrLabel17.Name = "xrLabel17";
        this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel17.SizeF = new System.Drawing.SizeF(200F, 18F);
        this.xrLabel17.StylePriority.UseFont = false;
        this.xrLabel17.Text = "xrLabel17";
        // 
        // xrLabel16
        // 
        this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 125.3749F);
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(153.125F, 18F);
        this.xrLabel16.StylePriority.UseFont = false;
        this.xrLabel16.Text = "Рік будівництва:";
        // 
        // xrLabel15
        // 
        this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.condition")});
        this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(191.3858F, 107.3749F);
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(200F, 18F);
        this.xrLabel15.StylePriority.UseFont = false;
        this.xrLabel15.Text = "xrLabel15";
        // 
        // xrLabel14
        // 
        this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 107.3749F);
        this.xrLabel14.Name = "xrLabel14";
        this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel14.SizeF = new System.Drawing.SizeF(153.125F, 18F);
        this.xrLabel14.StylePriority.UseFont = false;
        this.xrLabel14.Text = "Технічний стан:";
        // 
        // xrLabel13
        // 
        this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.num_floors")});
        this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(191.3858F, 89.37495F);
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(200F, 18F);
        this.xrLabel13.StylePriority.UseFont = false;
        this.xrLabel13.Text = "xrLabel13";
        // 
        // xrLabel12
        // 
        this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 89.37498F);
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(153.125F, 18F);
        this.xrLabel12.StylePriority.UseFont = false;
        this.xrLabel12.Text = "Кількість поверхів:";
        // 
        // xrLabel11
        // 
        this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.object_type")});
        this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(191.3858F, 71.37498F);
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(200F, 18F);
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.Text = "xrLabel11";
        // 
        // xrLabel10
        // 
        this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 71.37498F);
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(153.125F, 18F);
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.Text = "Тип об’єкту:";
        // 
        // xrLabel9
        // 
        this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.object_kind")});
        this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(191.3858F, 53.37499F);
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(200F, 18F);
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.Text = "xrLabel9";
        // 
        // xrLabel8
        // 
        this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 53.37499F);
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(153.125F, 18F);
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.Text = "Вид об’єкту:";
        // 
        // xrLabel7
        // 
        this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.district")});
        this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(191.3858F, 35.37499F);
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(200F, 18F);
        this.xrLabel7.StylePriority.UseFont = false;
        this.xrLabel7.Text = "xrLabel7";
        // 
        // xrLabel6
        // 
        this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 35.37499F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(153.125F, 18F);
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.Text = "Район:";
        // 
        // TopMargin
        // 
        this.TopMargin.HeightF = 16F;
        this.TopMargin.Name = "TopMargin";
        this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // BottomMargin
        // 
        this.BottomMargin.HeightF = 6F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel2,
            this.xrLabel3,
            this.xrLabel1});
        this.ReportHeader.HeightF = 73.95834F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrLabel4
        // 
        this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardProperties.FullStreetName")});
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(150F, 45F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(659.375F, 23F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "xrLabel4";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(150F, 22F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(659.375F, 23F);
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "КАРТКА НА ОБ’ЄКТ НЕРУХОМОГО МАЙНА ЗА АДРЕСОЮ:";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel3
        // 
        this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.Date, "Text", "{0:d}")});
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 28.83336F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 18.83334F);
        this.xrLabel3.StylePriority.UseBorders = false;
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "xrLabel3";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // Date
        // 
        this.Date.Description = "The date the report was generated";
        this.Date.Name = "Date";
        this.Date.Type = typeof(System.DateTime);
//        this.Date.ValueInfo = DateTime.Now.Date.ToShortDateString();
        this.Date.ValueInfo = System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        // 
        // xrLabel1
        // 
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 18.83334F);
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "Дата видачі:";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // db1
        // 
        this.db1.DataSetName = "DB";
        this.db1.EnforceConstraints = false;
        this.db1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // FullStreetName
        // 
        this.FullStreetName.DataMember = "ObjCardProperties";
        this.FullStreetName.Expression = "[street_full_name] + \' \' + [addr_nomer]";
        this.FullStreetName.FieldType = DevExpress.XtraReports.UI.FieldType.String;
        this.FullStreetName.Name = "FullStreetName";
        // 
        // AgreementNumberAndDate
        // 
        this.AgreementNumberAndDate.DataMember = "ObjCardArenda";
        this.AgreementNumberAndDate.Name = "AgreementNumberAndDate";
        this.AgreementNumberAndDate.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.AgreementNumberAndDate_GetValue);
        // 
        // AgreementStartEndDates
        // 
        this.AgreementStartEndDates.DataMember = "ObjCardArenda";
        this.AgreementStartEndDates.Name = "AgreementStartEndDates";
        this.AgreementStartEndDates.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.AgreementStartEndDates_GetValue);
        // 
        // DetailReport
        // 
        this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1,
            this.ReportHeader2});
        this.DetailReport.DataMember = "ObjCardBalans";
        this.DetailReport.DataSource = this.db1;
        this.DetailReport.Level = 0;
        this.DetailReport.Name = "DetailReport";
        // 
        // Detail1
        // 
        this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
        this.Detail1.HeightF = 25F;
        this.Detail1.Name = "Detail1";
        // 
        // xrTable1
        // 
        this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
        this.xrTable1.SizeF = new System.Drawing.SizeF(969F, 25F);
        this.xrTable1.StylePriority.UseBorders = false;
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell8,
            this.xrTableCell10,
            this.xrTableCell12});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardBalans.org_full_name")});
        this.xrTableCell1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell1.Multiline = true;
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UsePadding = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "xrTableCell1";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell1.Weight = 0.59252570898584356D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardBalans.org_ownership")});
        this.xrTableCell2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UsePadding = false;
        this.xrTableCell2.StylePriority.UseTextAlignment = false;
        this.xrTableCell2.Text = "xrTableCell2";
        this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell2.Weight = 0.34000643623342763D;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardBalans.BalansDocList")});
        this.xrTableCell3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell3.Multiline = true;
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell3.StylePriority.UseFont = false;
        this.xrTableCell3.StylePriority.UsePadding = false;
        this.xrTableCell3.StylePriority.UseTextAlignment = false;
        this.xrTableCell3.Text = "xrTableCell3";
        this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell3.Weight = 1.0674679171471089D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardBalans.sqr_total")});
        this.xrTableCell8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UsePadding = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        this.xrTableCell8.Text = "xrTableCell8";
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell8.Weight = 0.30785829841949575D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
 //           new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardBalans.sqr_non_habit")});
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardBalans.sqr_free_totalcnt")});
  this.xrTableCell10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.StylePriority.UsePadding = false;
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        this.xrTableCell10.Text = "xrTableCell10";
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell10.Weight = 0.285021325128952D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardBalans.purpose")});
        this.xrTableCell12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.StylePriority.UsePadding = false;
        this.xrTableCell12.StylePriority.UseTextAlignment = false;
        this.xrTableCell12.Text = "xrTableCell12";
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell12.Weight = 0.4071203140851718D;
        // 
        // ReportHeader2
        // 
        this.ReportHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrLabel5});
        this.ReportHeader2.HeightF = 68.25002F;
        this.ReportHeader2.KeepTogether = true;
        this.ReportHeader2.Name = "ReportHeader2";
        // 
        // xrTable4
        // 
        this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 20.00001F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4,
            this.xrTableRow5});
        this.xrTable4.SizeF = new System.Drawing.SizeF(968.9999F, 48.25001F);
        this.xrTable4.StylePriority.UseBorders = false;
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21,
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell24});
        this.xrTableRow4.Name = "xrTableRow4";
        this.xrTableRow4.Weight = 1D;
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell19.Multiline = true;
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.StylePriority.UseFont = false;
        this.xrTableCell19.StylePriority.UseTextAlignment = false;
        this.xrTableCell19.Text = "Назва організації-балансотримача,\r\nвид права";
        this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell19.Weight = 0.59252579249827453D;
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell20.Multiline = true;
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.StylePriority.UseFont = false;
        this.xrTableCell20.StylePriority.UseTextAlignment = false;
        this.xrTableCell20.Text = "Форма власності\r\nоб’єкта";
        this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell20.Weight = 0.340006385742435D;
        // 
        // xrTableCell21
        // 
        this.xrTableCell21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell21.Multiline = true;
        this.xrTableCell21.Name = "xrTableCell21";
        this.xrTableCell21.StylePriority.UseFont = false;
        this.xrTableCell21.StylePriority.UseTextAlignment = false;
        this.xrTableCell21.Text = "Рішення, розпорядження,\r\nінші документи (№, дата)";
        this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell21.Weight = 1.0674682641731548D;
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell22.Multiline = true;
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.StylePriority.UseTextAlignment = false;
        this.xrTableCell22.Text = "Загальна\r\nплоща на\r\nбалансі,\r\nкв.м";
        this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell22.Weight = 0.30785834494519493D;
        // 
        // xrTableCell23
        // 
        this.xrTableCell23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell23.Multiline = true;
        this.xrTableCell23.Name = "xrTableCell23";
        this.xrTableCell23.StylePriority.UseFont = false;
        this.xrTableCell23.StylePriority.UseTextAlignment = false;
//        this.xrTableCell23.Text = "Площа\r\nнежилих\r\nприміщень на\r\nбалансі, кв.м";
        this.xrTableCell23.Text = "Вільні\r\nприміщення";
        this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell23.Weight = 0.28502117454788223D;
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell24.Multiline = true;
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.StylePriority.UseFont = false;
        this.xrTableCell24.StylePriority.UseTextAlignment = false;
        //        this.xrTableCell24.Text = "Використання\r\nприміщення\r\n(цільове\r\nпризначення)\r\n";
                this.xrTableCell24.Text = "Цільове\r\nпризначення\r\n";
        this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell24.Weight = 0.4071200380930583D;
        // 
        // xrTableRow5
        // 
        this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCell28,
            this.xrTableCell29,
            this.xrTableCell30});
        this.xrTableRow5.Name = "xrTableRow5";
        this.xrTableRow5.Weight = 1D;
        // 
        // xrTableCell25
        // 
        this.xrTableCell25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell25.Name = "xrTableCell25";
        this.xrTableCell25.StylePriority.UseFont = false;
        this.xrTableCell25.StylePriority.UseTextAlignment = false;
        this.xrTableCell25.Text = "1";
        this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell25.Weight = 0.59252579249827453D;
        // 
        // xrTableCell26
        // 
        this.xrTableCell26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell26.Name = "xrTableCell26";
        this.xrTableCell26.StylePriority.UseFont = false;
        this.xrTableCell26.StylePriority.UseTextAlignment = false;
        this.xrTableCell26.Text = "2";
        this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell26.Weight = 0.340006385742435D;
        // 
        // xrTableCell27
        // 
        this.xrTableCell27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell27.Name = "xrTableCell27";
        this.xrTableCell27.StylePriority.UseFont = false;
        this.xrTableCell27.StylePriority.UseTextAlignment = false;
        this.xrTableCell27.Text = "3";
        this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell27.Weight = 1.0674680752097987D;
        // 
        // xrTableCell28
        // 
        this.xrTableCell28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell28.Name = "xrTableCell28";
        this.xrTableCell28.StylePriority.UseFont = false;
        this.xrTableCell28.StylePriority.UseTextAlignment = false;
        this.xrTableCell28.Text = "4";
        this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell28.Weight = 0.30785853390855072D;
        // 
        // xrTableCell29
        // 
        this.xrTableCell29.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell29.Name = "xrTableCell29";
        this.xrTableCell29.StylePriority.UseFont = false;
        this.xrTableCell29.StylePriority.UseTextAlignment = false;
        this.xrTableCell29.Text = "5";
        this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell29.Weight = 0.28502117454788223D;
        // 
        // xrTableCell30
        // 
        this.xrTableCell30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell30.Name = "xrTableCell30";
        this.xrTableCell30.StylePriority.UseFont = false;
        this.xrTableCell30.StylePriority.UseTextAlignment = false;
        this.xrTableCell30.Text = "6";
        this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell30.Weight = 0.40712003809305836D;
        // 
        // xrLabel5
        // 
        this.xrLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0.4167557F, 0F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(278.125F, 20F);
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.Text = "II. Перелік балансотримачів:";
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // DetailReport2
        // 
        this.DetailReport2.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail3,
            this.ReportHeader1,
            this.GroupHeader1});
        this.DetailReport2.DataMember = "ObjCardArenda";
        this.DetailReport2.DataSource = this.db1;
        this.DetailReport2.Level = 1;
        this.DetailReport2.Name = "DetailReport2";
        // 
        // Detail3
        // 
        this.Detail3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
        this.Detail3.HeightF = 25F;
        this.Detail3.Name = "Detail3";
        // 
        // xrTable2
        // 
        this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
        this.xrTable2.SizeF = new System.Drawing.SizeF(969F, 25F);
        this.xrTable2.StylePriority.UseBorders = false;
        // 
        // xrTableRow6
        // 
        this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell35,
            this.xrTableCell36,
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell40,
            this.xrTableCell41,
            this.xrTableCell42});
        this.xrTableRow6.Name = "xrTableRow6";
        this.xrTableRow6.Weight = 1D;
        // 
        // xrTableCell35
        // 
        this.xrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.org_renter_full_name")});
        this.xrTableCell35.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell35.Multiline = true;
        this.xrTableCell35.Name = "xrTableCell35";
        this.xrTableCell35.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell35.StylePriority.UseFont = false;
        this.xrTableCell35.StylePriority.UsePadding = false;
        this.xrTableCell35.StylePriority.UseTextAlignment = false;
        this.xrTableCell35.Text = "xrTableCell35";
        this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell35.Weight = 0.5726680833403599D;
        // 
        // xrTableCell36
        // 
        this.xrTableCell36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.pidstava_display")});
        this.xrTableCell36.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell36.Multiline = true;
        this.xrTableCell36.Name = "xrTableCell36";
        this.xrTableCell36.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell36.StylePriority.UseFont = false;
        this.xrTableCell36.StylePriority.UsePadding = false;
        this.xrTableCell36.StylePriority.UseTextAlignment = false;
        this.xrTableCell36.Text = "xrTableCell36";
        this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell36.Weight = 0.45155467561993773D;
        // 
        // xrTableCell37
        // 
        this.xrTableCell37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.AgreementNumberAndDate")});
        this.xrTableCell37.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell37.Multiline = true;
        this.xrTableCell37.Name = "xrTableCell37";
        this.xrTableCell37.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell37.StylePriority.UseFont = false;
        this.xrTableCell37.StylePriority.UsePadding = false;
        this.xrTableCell37.StylePriority.UseTextAlignment = false;
        this.xrTableCell37.Text = "xrTableCell37";
        this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell37.Weight = 0.512111234324749D;
        // 
        // xrTableCell38
        // 
        this.xrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.AgreementStartEndDates")});
        this.xrTableCell38.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell38.Multiline = true;
        this.xrTableCell38.Name = "xrTableCell38";
        this.xrTableCell38.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell38.StylePriority.UseFont = false;
        this.xrTableCell38.StylePriority.UsePadding = false;
        this.xrTableCell38.StylePriority.UseTextAlignment = false;
        this.xrTableCell38.Text = "xrTableCell38";
        this.xrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell38.Weight = 0.51211154882811527D;
        // 
        // xrTableCell39
        // 
        this.xrTableCell39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.rent_square")});
        this.xrTableCell39.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell39.Multiline = true;
        this.xrTableCell39.Name = "xrTableCell39";
        this.xrTableCell39.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell39.StylePriority.UseFont = false;
        this.xrTableCell39.StylePriority.UsePadding = false;
        this.xrTableCell39.StylePriority.UseTextAlignment = false;
        this.xrTableCell39.Text = "xrTableCell39";
        this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell39.Weight = 0.25605547200697465D;
        // 
        // xrTableCell40
        // 
        this.xrTableCell40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.floor_number")});
        this.xrTableCell40.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell40.Multiline = true;
        this.xrTableCell40.Name = "xrTableCell40";
        this.xrTableCell40.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell40.StylePriority.UseFont = false;
        this.xrTableCell40.StylePriority.UsePadding = false;
        this.xrTableCell40.StylePriority.UseTextAlignment = false;
        this.xrTableCell40.Text = "xrTableCell40";
        this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell40.Weight = 0.2560556655475078D;
        // 
        // xrTableCell41
        // 
        this.xrTableCell41.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.purpose")});
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.purpose_str")});
        this.xrTableCell41.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell41.Multiline = true;
        this.xrTableCell41.Name = "xrTableCell41";
        this.xrTableCell41.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell41.StylePriority.UseFont = false;
        this.xrTableCell41.StylePriority.UsePadding = false;
        this.xrTableCell41.StylePriority.UseTextAlignment = false;
        this.xrTableCell41.Text = "xrTableCell41";
        this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell41.Weight = 0.25605585908804085D;
        // 
        // xrTableCell42
        // 
        this.xrTableCell42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.is_subarenda")});
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.objpayment_type")});
        this.xrTableCell42.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell42.Multiline = true;
        this.xrTableCell42.Name = "xrTableCell42";
        this.xrTableCell42.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell42.StylePriority.UseFont = false;
        this.xrTableCell42.StylePriority.UsePadding = false;
        this.xrTableCell42.StylePriority.UseTextAlignment = false;
        this.xrTableCell42.Text = "xrTableCell42";
        this.xrTableCell42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell42.Weight = 0.25605554458467461D;
        // 
        // ReportHeader1
        // 
        this.ReportHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3,
            this.xrLabel51});
        this.ReportHeader1.HeightF = 72.99999F;
        this.ReportHeader1.KeepTogether = true;
        this.ReportHeader1.Name = "ReportHeader1";
        // 
        // xrTable3
        // 
        this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 22.99999F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow8});
        this.xrTable3.SizeF = new System.Drawing.SizeF(969F, 50F);
        this.xrTable3.StylePriority.UseBorders = false;
        // 
        // xrTableRow7
        // 
        this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell43,
            this.xrTableCell44,
            this.xrTableCell45,
            this.xrTableCell46,
            this.xrTableCell47,
            this.xrTableCell48,
            this.xrTableCell49,
            this.xrTableCell50});
        this.xrTableRow7.Name = "xrTableRow7";
        this.xrTableRow7.Weight = 1D;
        // 
        // xrTableCell43
        // 
        this.xrTableCell43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell43.Name = "xrTableCell43";
        this.xrTableCell43.StylePriority.UseFont = false;
        this.xrTableCell43.StylePriority.UseTextAlignment = false;
        this.xrTableCell43.Text = "Назва орендаря, керівник , телефон";
        this.xrTableCell43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell43.Weight = 0.55912450075887676D;
        // 
        // xrTableCell44
        // 
        this.xrTableCell44.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell44.Multiline = true;
        this.xrTableCell44.Name = "xrTableCell44";
        this.xrTableCell44.StylePriority.UseFont = false;
        this.xrTableCell44.StylePriority.UseTextAlignment = false;
        this.xrTableCell44.Text = "Підстава на орен-\r\nду (вид докумен-\r\nту, номер, дата,\r\nдодаток, пункт)";
        this.xrTableCell44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell44.Weight = 0.44087549924112324D;
        // 
        // xrTableCell45
        // 
        this.xrTableCell45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell45.Multiline = true;
        this.xrTableCell45.Name = "xrTableCell45";
        this.xrTableCell45.StylePriority.UseFont = false;
        this.xrTableCell45.StylePriority.UseTextAlignment = false;
        this.xrTableCell45.Text = "Договір\r\n оренди \r\n(№, дата)\r\n";
        this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell45.Weight = 0.5D;
        // 
        // xrTableCell46
        // 
        this.xrTableCell46.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell46.Multiline = true;
        this.xrTableCell46.Name = "xrTableCell46";
        this.xrTableCell46.StylePriority.UseFont = false;
        this.xrTableCell46.StylePriority.UseTextAlignment = false;
        this.xrTableCell46.Text = "Строки\r\nдоговору\r\n(початок,\r\nзакінчення)\r\n";
        this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell46.Weight = 0.5D;
        // 
        // xrTableCell47
        // 
        this.xrTableCell47.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell47.Multiline = true;
        this.xrTableCell47.Name = "xrTableCell47";
        this.xrTableCell47.StylePriority.UseFont = false;
        this.xrTableCell47.StylePriority.UseTextAlignment = false;
        this.xrTableCell47.Text = "Загальна\r\nорендо-\r\nвана\r\nплоща,\r\nкв.м";
        this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell47.Weight = 0.25D;
        // 
        // xrTableCell48
        // 
        this.xrTableCell48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell48.Multiline = true;
        this.xrTableCell48.Name = "xrTableCell48";
        this.xrTableCell48.StylePriority.UseFont = false;
        this.xrTableCell48.StylePriority.UseTextAlignment = false;
        this.xrTableCell48.Text = "Поверх\r\n(підвал,\r\nцоколь)";
        this.xrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell48.Weight = 0.25D;
        // 
        // xrTableCell49
        // 
        this.xrTableCell49.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell49.Multiline = true;
        this.xrTableCell49.Name = "xrTableCell49";
        this.xrTableCell49.StylePriority.UseFont = false;
        this.xrTableCell49.StylePriority.UseTextAlignment = false;
//        this.xrTableCell49.Text = "Цільове\r\nпризначення";
        this.xrTableCell49.Text = "Використання\r\nприміщення";
        this.xrTableCell49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell49.Weight = 0.25D;
        // 
        // xrTableCell50
        // 
        this.xrTableCell50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell50.Multiline = true;
        this.xrTableCell50.Name = "xrTableCell50";
        this.xrTableCell50.StylePriority.UseFont = false;
        this.xrTableCell50.StylePriority.UseTextAlignment = false;
//        this.xrTableCell50.Text = "Суб-\r\nоренда";
        this.xrTableCell50.Text = "Вид\r\nоплати";
        this.xrTableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell50.Weight = 0.25D;
        // 
        // xrTableRow8
        // 
        this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell51,
            this.xrTableCell52,
            this.xrTableCell53,
            this.xrTableCell54,
            this.xrTableCell55,
            this.xrTableCell56,
            this.xrTableCell57,
            this.xrTableCell58});
        this.xrTableRow8.Name = "xrTableRow8";
        this.xrTableRow8.Weight = 1D;
        // 
        // xrTableCell51
        // 
        this.xrTableCell51.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell51.Name = "xrTableCell51";
        this.xrTableCell51.StylePriority.UseFont = false;
        this.xrTableCell51.StylePriority.UseTextAlignment = false;
        this.xrTableCell51.Text = "1";
        this.xrTableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell51.Weight = 0.55912454799970979D;
        // 
        // xrTableCell52
        // 
        this.xrTableCell52.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell52.Name = "xrTableCell52";
        this.xrTableCell52.StylePriority.UseFont = false;
        this.xrTableCell52.StylePriority.UseTextAlignment = false;
        this.xrTableCell52.Text = "2";
        this.xrTableCell52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell52.Weight = 0.44087545200029027D;
        // 
        // xrTableCell53
        // 
        this.xrTableCell53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell53.Name = "xrTableCell53";
        this.xrTableCell53.StylePriority.UseFont = false;
        this.xrTableCell53.StylePriority.UseTextAlignment = false;
        this.xrTableCell53.Text = "3";
        this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell53.Weight = 0.5D;
        // 
        // xrTableCell54
        // 
        this.xrTableCell54.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell54.Name = "xrTableCell54";
        this.xrTableCell54.StylePriority.UseFont = false;
        this.xrTableCell54.StylePriority.UseTextAlignment = false;
        this.xrTableCell54.Text = "4";
        this.xrTableCell54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell54.Weight = 0.5D;
        // 
        // xrTableCell55
        // 
        this.xrTableCell55.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell55.Name = "xrTableCell55";
        this.xrTableCell55.StylePriority.UseFont = false;
        this.xrTableCell55.StylePriority.UseTextAlignment = false;
        this.xrTableCell55.Text = "5";
        this.xrTableCell55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell55.Weight = 0.25D;
        // 
        // xrTableCell56
        // 
        this.xrTableCell56.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell56.Name = "xrTableCell56";
        this.xrTableCell56.StylePriority.UseFont = false;
        this.xrTableCell56.StylePriority.UseTextAlignment = false;
        this.xrTableCell56.Text = "6";
        this.xrTableCell56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell56.Weight = 0.25D;
        // 
        // xrTableCell57
        // 
        this.xrTableCell57.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell57.Name = "xrTableCell57";
        this.xrTableCell57.StylePriority.UseFont = false;
        this.xrTableCell57.StylePriority.UseTextAlignment = false;
        this.xrTableCell57.Text = "7";
        this.xrTableCell57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell57.Weight = 0.25D;
        // 
        // xrTableCell58
        // 
        this.xrTableCell58.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell58.Name = "xrTableCell58";
        this.xrTableCell58.StylePriority.UseFont = false;
        this.xrTableCell58.StylePriority.UseTextAlignment = false;
        this.xrTableCell58.Text = "8";
        this.xrTableCell58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell58.Weight = 0.25D;
        // 
        // xrLabel51
        // 
        this.xrLabel51.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5F);
        this.xrLabel51.Name = "xrLabel51";
        this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel51.SizeF = new System.Drawing.SizeF(278.125F, 18F);
        this.xrLabel51.StylePriority.UseFont = false;
        this.xrLabel51.StylePriority.UseTextAlignment = false;
        this.xrLabel51.Text = "III. Перелік орендарів:";
        this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel52});
        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("org_giver_full_name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader1.HeightF = 23F;
        this.GroupHeader1.KeepTogether = true;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // xrLabel52
        // 
        this.xrLabel52.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel52.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
//            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.org_balans_full_name")});
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardArenda.org_giver_full_name")});
        this.xrLabel52.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel52.Name = "xrLabel52";
        this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel52.SizeF = new System.Drawing.SizeF(969F, 23F);
        this.xrLabel52.StylePriority.UseBorders = false;
        this.xrLabel52.StylePriority.UseFont = false;
        this.xrLabel52.StylePriority.UseTextAlignment = false;
        this.xrLabel52.Text = "xrLabel52";
        this.xrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // DetailReport1
        // 
        this.DetailReport1.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail2,
            this.ReportHeader3});
        this.DetailReport1.DataMember = "ObjCardPrivat";
        this.DetailReport1.DataSource = this.db1;
        this.DetailReport1.Level = 3;
        this.DetailReport1.Name = "DetailReport1";
        // 
        // Detail2
        // 
        this.Detail2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
        this.Detail2.HeightF = 25F;
        this.Detail2.Name = "Detail2";
        this.Detail2.Visible = false;
        // 
        // xrTable6
        // 
        this.xrTable6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable6.Name = "xrTable6";
        this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow9});
        this.xrTable6.SizeF = new System.Drawing.SizeF(968.4167F, 25F);
        this.xrTable6.StylePriority.UseBorders = false;
        // 
        // xrTableRow9
        // 
        this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell11,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell33});
        this.xrTableRow9.Name = "xrTableRow9";
        this.xrTableRow9.Weight = 1D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardPrivat.org_name")});
        this.xrTableCell9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell9.Multiline = true;
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UsePadding = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.Text = "xrTableCell9";
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell9.Weight = 0.63674710066889684D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardPrivat.rishennya_search_name")});
        this.xrTableCell11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell11.Multiline = true;
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UsePadding = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "xrTableCell11";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell11.Weight = 0.53259345759179444D;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardPrivat.sqr_total")});
        this.xrTableCell13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell13.Multiline = true;
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell13.StylePriority.UseFont = false;
        this.xrTableCell13.StylePriority.UsePadding = false;
        this.xrTableCell13.StylePriority.UseTextAlignment = false;
        this.xrTableCell13.Text = "xrTableCell13";
        this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell13.Weight = 0.27085355505780118D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardPrivat.purpose_group")});
        this.xrTableCell14.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell14.Multiline = true;
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell14.StylePriority.UseFont = false;
        this.xrTableCell14.StylePriority.UsePadding = false;
        this.xrTableCell14.StylePriority.UseTextAlignment = false;
        this.xrTableCell14.Text = "xrTableCell14";
        this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell14.Weight = 0.27080774138931152D;
        // 
        // xrTableCell33
        // 
        this.xrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardPrivat.privat_state")});
        this.xrTableCell33.Name = "xrTableCell33";
        this.xrTableCell33.StylePriority.UseTextAlignment = false;
        this.xrTableCell33.Text = "xrTableCell33";
        this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell33.Weight = 0.33621057614088967D;
        // 
        // ReportHeader3
        // 
        this.ReportHeader3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable5,
            this.xrLabel53});
        this.ReportHeader3.HeightF = 73.95834F;
        this.ReportHeader3.Name = "ReportHeader3";
        this.ReportHeader3.Visible = false;
        // 
        // xrTable5
        // 
        this.xrTable5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 22.99999F);
        this.xrTable5.Name = "xrTable5";
        this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2,
            this.xrTableRow3});
        this.xrTable5.SizeF = new System.Drawing.SizeF(968.9998F, 50F);
        this.xrTable5.StylePriority.UseBorders = false;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell31});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.Text = "Назва орендаря";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell4.Weight = 0.621688118413998D;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell5.Multiline = true;
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "Підстава";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell5.Weight = 0.51999767449126022D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell6.Multiline = true;
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "Площа, кв.м";
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell6.Weight = 0.2644476167188709D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell7.Multiline = true;
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UseTextAlignment = false;
        this.xrTableCell7.Text = "Призначення";
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell7.Weight = 0.26053332003031482D;
        // 
        // xrTableCell31
        // 
        this.xrTableCell31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell31.Name = "xrTableCell31";
        this.xrTableCell31.StylePriority.UseFont = false;
        this.xrTableCell31.StylePriority.UseTextAlignment = false;
        this.xrTableCell31.Text = "Стан";
        this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell31.Weight = 0.33333289241889197D;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell32});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.StylePriority.UseFont = false;
        this.xrTableCell15.StylePriority.UseTextAlignment = false;
        this.xrTableCell15.Text = "1";
        this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell15.Weight = 0.62168810266705365D;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseFont = false;
        this.xrTableCell16.StylePriority.UseTextAlignment = false;
        this.xrTableCell16.Text = "2";
        this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell16.Weight = 0.51999767449126033D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseFont = false;
        this.xrTableCell17.StylePriority.UseTextAlignment = false;
        this.xrTableCell17.Text = "3";
        this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell17.Weight = 0.26444750649026066D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseFont = false;
        this.xrTableCell18.StylePriority.UseTextAlignment = false;
        this.xrTableCell18.Text = "4";
        this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell18.Weight = 0.26053350899364675D;
        // 
        // xrTableCell32
        // 
        this.xrTableCell32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell32.Name = "xrTableCell32";
        this.xrTableCell32.StylePriority.UseFont = false;
        this.xrTableCell32.StylePriority.UseTextAlignment = false;
        this.xrTableCell32.Text = "5";
        this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell32.Weight = 0.33333282943111453D;
        // 
        // xrLabel53
        // 
        this.xrLabel53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5F);
        this.xrLabel53.Name = "xrLabel53";
        this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel53.SizeF = new System.Drawing.SizeF(600.0217F, 18F);
        this.xrLabel53.StylePriority.UseFont = false;
        this.xrLabel53.StylePriority.UseTextAlignment = false;
        this.xrLabel53.Text = "V. Перелік об’єктів, які включені в програму приватизації";
        this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // BalansDocList
        // 
        this.BalansDocList.DataMember = "ObjCardBalans";
        this.BalansDocList.DisplayName = "BalansDocList";
        this.BalansDocList.Name = "BalansDocList";
        this.BalansDocList.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.BalansDocList_GetValue);
        // 
        // DetailReport3
        // 
        this.DetailReport3.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail4,
            this.ReportHeader4});
        this.DetailReport3.DataMember = "ObjCardRentersNoAgreements";
        this.DetailReport3.DataSource = this.db1;
        this.DetailReport3.Level = 2;
        this.DetailReport3.Name = "DetailReport3";
        // 
        // Detail4
        // 
        this.Detail4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable8});
        this.Detail4.HeightF = 25F;
        this.Detail4.Name = "Detail4";
        this.Detail4.Visible = false;
        // 
        // xrTable8
        // 
        this.xrTable8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable8.LocationFloat = new DevExpress.Utils.PointFloat(0.4167557F, 0F);
        this.xrTable8.Name = "xrTable8";
        this.xrTable8.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow12});
        this.xrTable8.SizeF = new System.Drawing.SizeF(968F, 25F);
        this.xrTable8.StylePriority.UseBorders = false;
        // 
        // xrTableRow12
        // 
        this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell65,
            this.xrTableCell73,
            this.xrTableCell74,
            this.xrTableCell75,
            this.xrTableCell76,
            this.xrTableCell77,
            this.xrTableCell78});
        this.xrTableRow12.Name = "xrTableRow12";
        this.xrTableRow12.Weight = 1D;
        // 
        // xrTableCell65
        // 
        this.xrTableCell65.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardRentersNoAgreements.RentersNAName")});
        this.xrTableCell65.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell65.Multiline = true;
        this.xrTableCell65.Name = "xrTableCell65";
        this.xrTableCell65.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell65.StylePriority.UseFont = false;
        this.xrTableCell65.StylePriority.UsePadding = false;
        this.xrTableCell65.StylePriority.UseTextAlignment = false;
        this.xrTableCell65.Text = "xrTableCell65";
        this.xrTableCell65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell65.Weight = 0.88060399032839243D;
        // 
        // xrTableCell73
        // 
        this.xrTableCell73.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardRentersNoAgreements.RentersNALetter")});
        this.xrTableCell73.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell73.Multiline = true;
        this.xrTableCell73.Name = "xrTableCell73";
        this.xrTableCell73.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell73.StylePriority.UseFont = false;
        this.xrTableCell73.StylePriority.UsePadding = false;
        this.xrTableCell73.StylePriority.UseTextAlignment = false;
        this.xrTableCell73.Text = "xrTableCell73";
        this.xrTableCell73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell73.Weight = 0.33057321415795449D;
        // 
        // xrTableCell74
        // 
        this.xrTableCell74.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardRentersNoAgreements.RentersNAPurpose")});
        this.xrTableCell74.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell74.Multiline = true;
        this.xrTableCell74.Name = "xrTableCell74";
        this.xrTableCell74.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell74.StylePriority.UseFont = false;
        this.xrTableCell74.StylePriority.UsePadding = false;
        this.xrTableCell74.StylePriority.UseTextAlignment = false;
        this.xrTableCell74.Text = "xrTableCell74";
        this.xrTableCell74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell74.Weight = 0.38989716159260374D;
        // 
        // xrTableCell75
        // 
        this.xrTableCell75.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardRentersNoAgreements.RentersNASqr")});
        this.xrTableCell75.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell75.Multiline = true;
        this.xrTableCell75.Name = "xrTableCell75";
        this.xrTableCell75.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell75.StylePriority.UseFont = false;
        this.xrTableCell75.StylePriority.UsePadding = false;
        this.xrTableCell75.StylePriority.UseTextAlignment = false;
        this.xrTableCell75.Text = "xrTableCell75";
        this.xrTableCell75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell75.Weight = 0.38678950544523061D;
        // 
        // xrTableCell76
        // 
        this.xrTableCell76.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardRentersNoAgreements.RentersNATerm")});
        this.xrTableCell76.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell76.Multiline = true;
        this.xrTableCell76.Name = "xrTableCell76";
        this.xrTableCell76.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell76.StylePriority.UseFont = false;
        this.xrTableCell76.StylePriority.UsePadding = false;
        this.xrTableCell76.StylePriority.UseTextAlignment = false;
        this.xrTableCell76.Text = "xrTableCell76";
        this.xrTableCell76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell76.Weight = 0.31531564783596844D;
        // 
        // xrTableCell77
        // 
        this.xrTableCell77.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardRentersNoAgreements.RentersNADecision")});
        this.xrTableCell77.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell77.Multiline = true;
        this.xrTableCell77.Name = "xrTableCell77";
        this.xrTableCell77.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell77.StylePriority.UseFont = false;
        this.xrTableCell77.StylePriority.UsePadding = false;
        this.xrTableCell77.StylePriority.UseTextAlignment = false;
        this.xrTableCell77.Text = "xrTableCell77";
        this.xrTableCell77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell77.Weight = 0.35118509545644888D;
        // 
        // xrTableCell78
        // 
        this.xrTableCell78.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ObjCardRentersNoAgreements.RentersNARishen")});
        this.xrTableCell78.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell78.Multiline = true;
        this.xrTableCell78.Name = "xrTableCell78";
        this.xrTableCell78.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell78.StylePriority.UseFont = false;
        this.xrTableCell78.StylePriority.UsePadding = false;
        this.xrTableCell78.StylePriority.UseTextAlignment = false;
        this.xrTableCell78.Text = "xrTableCell78";
        this.xrTableCell78.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell78.Weight = 0.41830346852376121D;
        // 
        // ReportHeader4
        // 
        this.ReportHeader4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable7,
            this.xrLabel54});
        this.ReportHeader4.HeightF = 90.54165F;
        this.ReportHeader4.Name = "ReportHeader4";
        this.ReportHeader4.Visible = false;
        // 
        // xrTable7
        // 
        this.xrTable7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
        this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 40.54165F);
        this.xrTable7.Name = "xrTable7";
        this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow10,
            this.xrTableRow11});
        this.xrTable7.SizeF = new System.Drawing.SizeF(968.4168F, 50F);
        this.xrTable7.StylePriority.UseBorders = false;
        this.xrTable7.StylePriority.UseFont = false;
        // 
        // xrTableRow10
        // 
        this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell34,
            this.xrTableCell59,
            this.xrTableCell60,
            this.xrTableCell61,
            this.xrTableCell62,
            this.xrTableCell63,
            this.xrTableCell64});
        this.xrTableRow10.Name = "xrTableRow10";
        this.xrTableRow10.Weight = 1D;
        // 
        // xrTableCell34
        // 
        this.xrTableCell34.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell34.Name = "xrTableCell34";
        this.xrTableCell34.StylePriority.UseFont = false;
        this.xrTableCell34.StylePriority.UseTextAlignment = false;
        this.xrTableCell34.Text = "Назва орендаря, керівник, телефон";
        this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell34.Weight = 0.86106811145510842D;
        // 
        // xrTableCell59
        // 
        this.xrTableCell59.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell59.Multiline = true;
        this.xrTableCell59.Name = "xrTableCell59";
        this.xrTableCell59.StylePriority.UseFont = false;
        this.xrTableCell59.StylePriority.UseTextAlignment = false;
        this.xrTableCell59.Text = "Номер, дата листа-звернення";
        this.xrTableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell59.Weight = 0.32275532347499031D;
        // 
        // xrTableCell60
        // 
        this.xrTableCell60.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell60.Multiline = true;
        this.xrTableCell60.Name = "xrTableCell60";
        this.xrTableCell60.StylePriority.UseFont = false;
        this.xrTableCell60.StylePriority.UseTextAlignment = false;
        this.xrTableCell60.Text = "Цільове призначення приміщення";
        this.xrTableCell60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell60.Weight = 0.38067608056791791D;
        // 
        // xrTableCell61
        // 
        this.xrTableCell61.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell61.Multiline = true;
        this.xrTableCell61.Name = "xrTableCell61";
        this.xrTableCell61.StylePriority.UseFont = false;
        this.xrTableCell61.StylePriority.UseTextAlignment = false;
        this.xrTableCell61.Text = "Площа приміщення, кв.м, розміщення в будинку";
        this.xrTableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell61.Weight = 0.37764199082696404D;
        // 
        // xrTableCell62
        // 
        this.xrTableCell62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell62.Multiline = true;
        this.xrTableCell62.Name = "xrTableCell62";
        this.xrTableCell62.StylePriority.UseFont = false;
        this.xrTableCell62.StylePriority.UseTextAlignment = false;
        this.xrTableCell62.Text = "Термін дії договору оренди";
        this.xrTableCell62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell62.Weight = 0.30785849367501933D;
        // 
        // xrTableCell63
        // 
        this.xrTableCell63.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell63.Multiline = true;
        this.xrTableCell63.Name = "xrTableCell63";
        this.xrTableCell63.StylePriority.UseFont = false;
        this.xrTableCell63.StylePriority.UseTextAlignment = false;
        this.xrTableCell63.Text = "Результат розгляду";
        this.xrTableCell63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell63.Weight = 0.34287963489260836D;
        // 
        // xrTableCell64
        // 
        this.xrTableCell64.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell64.Multiline = true;
        this.xrTableCell64.Name = "xrTableCell64";
        this.xrTableCell64.StylePriority.UseFont = false;
        this.xrTableCell64.StylePriority.UseTextAlignment = false;
        this.xrTableCell64.Text = "№, дата рішення (розпорядження), додаток, пункт";
        this.xrTableCell64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell64.Weight = 0.40531425392090303D;
        // 
        // xrTableRow11
        // 
        this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell66,
            this.xrTableCell67,
            this.xrTableCell68,
            this.xrTableCell69,
            this.xrTableCell70,
            this.xrTableCell71,
            this.xrTableCell72});
        this.xrTableRow11.Name = "xrTableRow11";
        this.xrTableRow11.Weight = 1D;
        // 
        // xrTableCell66
        // 
        this.xrTableCell66.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell66.Name = "xrTableCell66";
        this.xrTableCell66.StylePriority.UseFont = false;
        this.xrTableCell66.StylePriority.UseTextAlignment = false;
        this.xrTableCell66.Text = "1";
        this.xrTableCell66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell66.Weight = 0.86106811145510842D;
        // 
        // xrTableCell67
        // 
        this.xrTableCell67.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell67.Name = "xrTableCell67";
        this.xrTableCell67.StylePriority.UseFont = false;
        this.xrTableCell67.StylePriority.UseTextAlignment = false;
        this.xrTableCell67.Text = "2";
        this.xrTableCell67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell67.Weight = 0.32275537071582328D;
        // 
        // xrTableCell68
        // 
        this.xrTableCell68.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell68.Name = "xrTableCell68";
        this.xrTableCell68.StylePriority.UseFont = false;
        this.xrTableCell68.StylePriority.UseTextAlignment = false;
        this.xrTableCell68.Text = "3";
        this.xrTableCell68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell68.Weight = 0.380676222290417D;
        // 
        // xrTableCell69
        // 
        this.xrTableCell69.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell69.Name = "xrTableCell69";
        this.xrTableCell69.StylePriority.UseFont = false;
        this.xrTableCell69.StylePriority.UseTextAlignment = false;
        this.xrTableCell69.Text = "4";
        this.xrTableCell69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell69.Weight = 0.37764199082696404D;
        // 
        // xrTableCell70
        // 
        this.xrTableCell70.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell70.Name = "xrTableCell70";
        this.xrTableCell70.StylePriority.UseFont = false;
        this.xrTableCell70.StylePriority.UseTextAlignment = false;
        this.xrTableCell70.Text = "5";
        this.xrTableCell70.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell70.Weight = 0.30785830471168729D;
        // 
        // xrTableCell71
        // 
        this.xrTableCell71.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell71.Name = "xrTableCell71";
        this.xrTableCell71.StylePriority.UseFont = false;
        this.xrTableCell71.StylePriority.UseTextAlignment = false;
        this.xrTableCell71.Text = "6";
        this.xrTableCell71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell71.Weight = 0.34287963489260836D;
        // 
        // xrTableCell72
        // 
        this.xrTableCell72.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell72.Name = "xrTableCell72";
        this.xrTableCell72.StylePriority.UseFont = false;
        this.xrTableCell72.StylePriority.UseTextAlignment = false;
        this.xrTableCell72.Text = "7";
        this.xrTableCell72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell72.Weight = 0.40531425392090303D;
        // 
        // xrLabel54
        // 
        this.xrLabel54.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.999987F);
        this.xrLabel54.Name = "xrLabel54";
        this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel54.SizeF = new System.Drawing.SizeF(945.4584F, 35.54166F);
        this.xrLabel54.StylePriority.UseFont = false;
        this.xrLabel54.StylePriority.UseTextAlignment = false;
        this.xrLabel54.Text = "IV. Перелік суб’єктів, по яких прийнято рішення Київради щодо надання приміщень в" +
            " оренду, але інформація щодо укладання договорів оренди до Головного управління " +
            "не надійшла:";
        this.xrLabel54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // RentersNAName
        // 
        this.RentersNAName.DataMember = "ObjCardRentersNoAgreements";
        this.RentersNAName.Name = "RentersNAName";
        this.RentersNAName.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.RentersNAName_GetValue);
        // 
        // RentersNALetter
        // 
        this.RentersNALetter.DataMember = "ObjCardRentersNoAgreements";
        this.RentersNALetter.Name = "RentersNALetter";
        this.RentersNALetter.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.RentersNALetter_GetValue);
        // 
        // RentersNAPurpose
        // 
        this.RentersNAPurpose.DataMember = "ObjCardRentersNoAgreements";
        this.RentersNAPurpose.Name = "RentersNAPurpose";
        this.RentersNAPurpose.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.RentersNAPurpose_GetValue);
        // 
        // RentersNASqr
        // 
        this.RentersNASqr.DataMember = "ObjCardRentersNoAgreements";
        this.RentersNASqr.Name = "RentersNASqr";
        this.RentersNASqr.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.RentersNASqr_GetValue);
        // 
        // RentersNATerm
        // 
        this.RentersNATerm.DataMember = "ObjCardRentersNoAgreements";
        this.RentersNATerm.Name = "RentersNATerm";
        this.RentersNATerm.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.RentersNATerm_GetValue);
        // 
        // RentersNADecision
        // 
        this.RentersNADecision.DataMember = "ObjCardRentersNoAgreements";
        this.RentersNADecision.Name = "RentersNADecision";
        this.RentersNADecision.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.RentersNADecision_GetValue);
        // 
        // RentersNARishen
        // 
        this.RentersNARishen.DataMember = "ObjCardRentersNoAgreements";
        this.RentersNARishen.Name = "RentersNARishen";
        this.RentersNARishen.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.RentersNARishen_GetValue);
        // 
        // KorisnaPloscha
        // 
        this.KorisnaPloscha.DataMember = "ObjCardProperties";
        this.KorisnaPloscha.Expression = "[sqr_object_mk] + [sqr_object_rk]";
        this.KorisnaPloscha.Name = "KorisnaPloscha";
        // 
        // PloschaInshVlasnikiv
        // 
        this.PloschaInshVlasnikiv.DataMember = "ObjCardProperties";
        this.PloschaInshVlasnikiv.Expression = "[sqr_object_dk] + [sqr_object_other]";
        this.PloschaInshVlasnikiv.Name = "PloschaInshVlasnikiv";
        // 
        // RentedSqrTotal
        // 
        this.RentedSqrTotal.DataMember = "ObjCardArenda";
        this.RentedSqrTotal.Name = "RentedSqrTotal";
        this.RentedSqrTotal.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.RentedSqrTotal_GetValue);
        // 
        // ReportObjectCard
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.DetailReport,
            this.DetailReport2,
            this.DetailReport3,
            this.DetailReport1});
        this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.FullStreetName,
            this.AgreementNumberAndDate,
            this.AgreementStartEndDates,
            this.BalansDocList,
            this.RentersNAName,
            this.RentersNALetter,
            this.RentersNAPurpose,
            this.RentersNASqr,
            this.RentersNATerm,
            this.RentersNADecision,
            this.RentersNARishen,
            this.KorisnaPloscha,
            this.PloschaInshVlasnikiv,
            this.RentedSqrTotal});
        this.DataMember = "ObjCardProperties";
        this.DataSource = this.db1;
        this.Landscape = true;
        this.Margins = new System.Drawing.Printing.Margins(35, 25, 16, 6);
        this.PageHeight = 827;
        this.PageWidth = 1169;
        this.PaperKind = System.Drawing.Printing.PaperKind.A4;
        this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Date});
        this.Version = "12.1";
        ((System.ComponentModel.ISupportInitialize)(this.db1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

	}

	#endregion

    private void AgreementNumberAndDate_GetValue(object sender, GetValueEventArgs e)
    {
        string agreementNum = "";
        string agreementDate = "";

        if (!((DB.ObjCardArendaRow)((DataRowView)e.Row).Row).Isagreement_numNull())
        {
            agreementNum = ((DB.ObjCardArendaRow)((DataRowView)e.Row).Row).agreement_num;
        }

        if (!((DB.ObjCardArendaRow)((DataRowView)e.Row).Row).Isagreement_dateNull())
        {
            DateTime dt = ((DB.ObjCardArendaRow)((DataRowView)e.Row).Row).agreement_date;

            agreementDate = dt.ToString("d");
        }

        if (agreementNum.Length > 0)
        {
            if (agreementDate.Length > 0)
            {
                e.Value = string.Format("№ {0}\nвід {1}", agreementNum, agreementDate);
            }
            else
            {
                e.Value = string.Format("№ {0}", agreementNum);
            }
        }
        else
        {
            e.Value = "";
        }
    }

    private void AgreementStartEndDates_GetValue(object sender, GetValueEventArgs e)
    {
        e.Value = string.Format("{0:d}-\n{1:d}",
            ((DB.ObjCardArendaRow)((DataRowView)e.Row).Row).Isrent_start_dateNull()
                ? null : (DateTime?)((DB.ObjCardArendaRow)((DataRowView)e.Row).Row).rent_start_date,
            ((DB.ObjCardArendaRow)((DataRowView)e.Row).Row).Isrent_finish_dateNull() 
                ? null : (DateTime?)((DB.ObjCardArendaRow)((DataRowView)e.Row).Row).rent_finish_date);
    }

    private void BalansDocList_GetValue(object sender, GetValueEventArgs e)
    {
        string docList = "";

        if (!balansDocuments.TryGetValue(((DB.ObjCardBalansRow)((DataRowView)e.Row).Row).balans_id, out docList))
        {
            docList = "";
        }

        e.Value = docList;
    }

    private void FillBalansDocLists()
    {
        // Get all the balans IDs that we are going to display
        List<int> balansIDs = new List<int>();
        DataTableReader reader = db1.ObjCardBalans.CreateDataReader();

        if (reader != null)
        {
            while (reader.Read())
            {
                object balansId = reader.IsDBNull(0) ? null : reader.GetValue(0);

                if (balansId is int)
                {
                    balansIDs.Add((int)balansId);
                }
            }

            reader.Close();
        }

        // Open connection to the database
        string connString = (new DBTableAdapters.ObjCardPropertiesTableAdapter()).Connection.ConnectionString;

        SqlConnection connection = new SqlConnection(connString);

        try
        {
            connection.Open();

            foreach (int balansId in balansIDs)
            {
                balansDocuments[(int)balansId] = GetBalansDocList((int)balansId, connection);
            }
        }
        finally
        {
            connection.Close();
        }
    }

    private string GetBalansDocList(int balansId, SqlConnection connection)
    {
        string querySelect = "SELECT docs.display_name from balans_docs bd " +
            "INNER JOIN building_docs bdocs ON bd.building_docs_id = bdocs.id " +
            "INNER JOIN view_documents docs ON bdocs.document_id = docs.id " +
            "WHERE bd.balans_id = @balid";

        string docList = "";

        using (SqlCommand command = new SqlCommand(querySelect, connection))
        {
            command.Parameters.Add(new SqlParameter("balid", balansId));

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    object docName = reader.IsDBNull(0) ? null : reader.GetValue(0);

                    if (docName is string)
                    {
                        string name = ((string)docName).Trim();

                        if (name.Length > 0)
                        {
                            if (docList.Length > 0)
                            {
                                docList += "\n\n";
                            }

                            docList += name;
                        }
                    }
                }

                reader.Close();
            }
        }

        return docList;
    }

    private void FillRentersNoAgreementsData()
    {
        string statement =

        @"SELECT
            decs.id AS 'decision_id',
            decs.rent_term,
            decs.note_nazva,
            appl.appl_letter_num,
            appl.appl_letter_date,
            appl.appl_sqr,
            decision.name AS 'decision',
            renter.id AS 'renter_id',
            renter.full_name AS 'renter_name',
            renter.director_fio AS 'renter_fio',
            renter.director_phone AS 'renter_phone',
            doc.doc_num AS 'rishen_num',
            doc.doc_date AS 'rishen_date',
            app.app_num_user AS 'rishen_dodatok',
            decs.rishen_punkt
        FROM
            arenda_decisions decs
            INNER JOIN arenda_applications appl ON appl.id = decs.application_id
            INNER JOIN doc_appendices app ON app.id = decs.appendix_rasp_id
            INNER JOIN documents doc ON doc.id = app.doc_id
            LEFT OUTER JOIN dict_rent_decisions decision ON decision.id = decs.decision_id
            LEFT OUTER JOIN organizations renter ON renter.id = decs.org_renter_id and (renter.is_deleted is null or renter.is_deleted = 0)
        WHERE
            decs.building_id = @bid AND
            decs.is_subarenda <> 2 AND
            decs.decision_id IN (21, 22, 24) AND
            NOT (appl.appl_letter_date IS NULL) AND
            NOT EXISTS (SELECT ar.id FROM arenda ar INNER JOIN link_arenda_2_decisions lnk ON lnk.arenda_id = ar.id WHERE
                ar.org_renter_id = renter.id AND RTRIM(LTRIM(lnk.doc_num)) = RTRIM(LTRIM(doc.doc_num)))
        ORDER BY
            renter.id,
            appl.appl_letter_date";

        // Open connection to the database
        string connString = (new DBTableAdapters.ObjCardPropertiesTableAdapter()).Connection.ConnectionString;

        SqlConnection connection = new SqlConnection(connString);

        try
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand(statement, connection))
            {
                cmd.Parameters.Add(new SqlParameter("bid", buildingId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int renterId = reader.IsDBNull(7) ? -1 : reader.GetInt32(7);

                        if (renterId > 0)
                        {
                            RenterNoAgreementInfo info = new RenterNoAgreementInfo();

                            info.rentTerm = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            info.purpose = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            info.square = reader.IsDBNull(5) ? "" : reader.GetDecimal(5).ToString();
                            info.decision = reader.IsDBNull(6) ? "" : reader.GetString(6);

                            string letterNum = reader.IsDBNull(3) ? "" : reader.GetString(3);

                            if (letterNum.Length > 0 && !reader.IsDBNull(4))
                            {
                                info.letterNumAndDate = Resources.Strings.RenterLetterNum + letterNum + "\n" +
                                    Resources.Strings.RenterLetterDate + reader.GetDateTime(4).ToShortDateString();
                            }

                            string renterName = reader.IsDBNull(8) ? "" : reader.GetString(8);
                            string renterFIO = reader.IsDBNull(9) ? "" : reader.GetString(9);
                            string renterPhone = reader.IsDBNull(10) ? "" : reader.GetString(10);

                            if (renterFIO.Length > 0)
                            {
                                renterName += "\n" + Resources.Strings.RenterDirector + renterFIO;
                            }

                            if (renterPhone.Length > 0)
                            {
                                renterName += "\n" + Resources.Strings.RenterPhone + renterPhone;
                            }

                            info.renterName = renterName;

                            string rishNum = reader.IsDBNull(11) ? "" : reader.GetString(11);
                            string rishDate = reader.IsDBNull(12) ? "" : reader.GetDateTime(12).ToShortDateString();
                            string rishDodatok = reader.IsDBNull(13) ? "" : reader.GetString(13);
                            string rishPunkt = reader.IsDBNull(14) ? "" : reader.GetInt32(14).ToString();

                            if (rishNum.Length > 0)
                            {
                                rishNum = Resources.Strings.RishennyaNum + rishNum;

                                if (rishDate.Length > 0)
                                {
                                    rishNum += "\n" + Resources.Strings.RishennyaDate + rishDate;
                                }

                                if (rishDodatok.Length > 0 || rishPunkt.Length > 0)
                                {
                                    rishNum += "\n";

                                    if (rishDodatok.Length > 0)
                                    {
                                        rishNum += Resources.Strings.RishennyaDodatok + rishDodatok;
                                    }

                                    if (rishPunkt.Length > 0)
                                    {
                                        rishNum += " " + Resources.Strings.RishennyaPunkt + rishPunkt;
                                    }
                                }
                            }

                            info.rishennya = rishNum;

                            // Save the information
                            rentersInfo[renterId] = info;
                        }
                    }

                    reader.Close();
                }
            }
        }
        finally
        {
            connection.Close();
        }
    }

    private void CalcTotalRentedSquare()
    {
/*        string statement = @"SELECT SUM(rent_square) FROM arenda WHERE
            building_id = @bid AND
            (is_deleted IS NULL OR is_deleted = 0) AND
            (is_subarenda IS NULL OR is_subarenda = 0) AND
            (is_privat IS NULL OR is_privat = 0)
*/
        string statement = @"SELECT SUM(rent_square) FROM dbo.reports1nf_arenda WHERE
            building_id = @bid AND
            isnull(is_deleted, 0) = 0 AND
            isnull(is_subarenda, 0) = 0 AND
            isnull(is_privat, 0) = 0
            and agreement_state = 1";
        
        // Open connection to the database
        string connString = (new DBTableAdapters.ObjCardPropertiesTableAdapter()).Connection.ConnectionString;

        SqlConnection connection = new SqlConnection(connString);

        try
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand(statement, connection))
            {
                cmd.Parameters.Add(new SqlParameter("bid", buildingId));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        object value = reader.GetValue(0);

                        if (value is decimal)
                        {
                            totalRentedSquare = (decimal)value;
                        }
                    }

                    reader.Close();
                }
            }
        }
        finally
        {
            connection.Close();
        }
    }

    #region Internal classes

    private class RenterNoAgreementInfo
    {
        public string renterName = "";
        public string rentTerm = "";
        public string purpose = "";
        public string letterNumAndDate = "";
        public string square = "";
        public string decision = "";
        public string rishennya = "";

        public RenterNoAgreementInfo()
        {
        }
    }

    #endregion (Internal classes)

    private void RentersNADecision_GetValue(object sender, GetValueEventArgs e)
    {
        string res = "";
        RenterNoAgreementInfo info = null;

        if (rentersInfo.TryGetValue(((DB.ObjCardRentersNoAgreementsRow)((DataRowView)e.Row).Row).org_renter_id, out info))
        {
            res = info.decision;
        }

        e.Value = res;
    }

    private void RentersNALetter_GetValue(object sender, GetValueEventArgs e)
    {
        string res = "";
        RenterNoAgreementInfo info = null;

        if (rentersInfo.TryGetValue(((DB.ObjCardRentersNoAgreementsRow)((DataRowView)e.Row).Row).org_renter_id, out info))
        {
            res = info.letterNumAndDate;
        }

        e.Value = res;
    }

    private void RentersNATerm_GetValue(object sender, GetValueEventArgs e)
    {
        string res = "";
        RenterNoAgreementInfo info = null;

        if (rentersInfo.TryGetValue(((DB.ObjCardRentersNoAgreementsRow)((DataRowView)e.Row).Row).org_renter_id, out info))
        {
            res = info.rentTerm;
        }

        e.Value = res;
    }

    private void RentersNASqr_GetValue(object sender, GetValueEventArgs e)
    {
        string res = "";
        RenterNoAgreementInfo info = null;

        if (rentersInfo.TryGetValue(((DB.ObjCardRentersNoAgreementsRow)((DataRowView)e.Row).Row).org_renter_id, out info))
        {
            res = info.square;
        }

        e.Value = res;
    }

    private void RentersNARishen_GetValue(object sender, GetValueEventArgs e)
    {
        string res = "";
        RenterNoAgreementInfo info = null;

        if (rentersInfo.TryGetValue(((DB.ObjCardRentersNoAgreementsRow)((DataRowView)e.Row).Row).org_renter_id, out info))
        {
            res = info.rishennya;
        }

        e.Value = res;
    }

    private void RentersNAPurpose_GetValue(object sender, GetValueEventArgs e)
    {
        string res = "";
        RenterNoAgreementInfo info = null;

        if (rentersInfo.TryGetValue(((DB.ObjCardRentersNoAgreementsRow)((DataRowView)e.Row).Row).org_renter_id, out info))
        {
            res = info.purpose;
        }

        e.Value = res;
    }

    private void RentersNAName_GetValue(object sender, GetValueEventArgs e)
    {
        string res = "";
        RenterNoAgreementInfo info = null;

        if (rentersInfo.TryGetValue(((DB.ObjCardRentersNoAgreementsRow)((DataRowView)e.Row).Row).org_renter_id, out info))
        {
            res = info.renterName;
        }

        e.Value = res;
    }

    private void RentedSqrTotal_GetValue(object sender, GetValueEventArgs e)
    {
        e.Value = totalRentedSquare;
    }
}
