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
public class ReportOrgCard : DevExpress.XtraReports.UI.XtraReport
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private int organizationId = -1;

	private DevExpress.XtraReports.UI.DetailBand Detail;
	private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
	private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private ReportHeaderBand ReportHeader;
    private OrgCardDB orgCardDB;
    private XRLabel xrLabel3;
    private DevExpress.XtraReports.Parameters.Parameter Date;
    private XRLabel xrLabel1;
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
    private XRLabel xrLabel17;
    private XRLabel xrLabel16;
    private XRLabel xrLabel15;
    private XRLabel xrLabel50;
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
    private XRLabel xrLabel64;
    private XRLabel xrLabel63;
    private XRLabel xrLabel62;
    private XRLabel xrLabel67;
    private XRLabel xrLabel66;
    private XRLabel xrLabel65;
    private XRLabel xrLabel61;
    private XRLabel xrLabel57;
    private XRLabel xrLabel56;
    private XRLabel xrLabel55;
    private XRLabel xrLabel60;
    private XRLabel xrLabel59;
    private XRLabel xrLabel58;
    private XRLabel xrLabel52;
    private XRLabel xrLabel54;
    private XRLabel xrLabel49;
    private XRLabel xrLabel43;
    private XRLabel xrLabel45;
    private XRLabel xrLabel48;
    private XRLabel xrLabel35;
    private XRLabel xrLabel41;
    private XRLabel xrLabel42;
    private XRLabel xrLabel33;
    private XRLabel xrLabel27;
    private XRLabel xrLabel28;
    private XRLabel xrLabel30;
    private XRLabel xrLabel26;
    private XRLabel xrLabel39;
    private XRLabel xrLabel38;
    private XRLabel xrLabel37;
    private XRLabel xrLabel46;
    private XRLabel xrLabel44;
    private XRLabel xrLabel40;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel4;
    private XRLabel xrLabel36;
    private XRLabel xrLabel34;
    private XRLabel xrLabel32;
    private XRLabel xrLabel25;
    private XRLabel xrLabel24;
    private XRLabel xrLabel31;
    private XRLabel xrLabel29;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel23;
    private XRLabel xrLabel22;
    private XRTableCell xrTableCell79;
    private XRTableCell xrTableCell81;
    private XRTableCell xrTableCell80;
    private XRTableCell xrTableCell82;
    private XRTableCell xrTableCell84;
    private XRTableCell xrTableCell83;
    private XRLabel xrLabel47;
    private XRTableCell xrTableCell87;
    private XRTableCell xrTableCell85;
    private XRTableCell xrTableCell86;
    private CalculatedField FieldGiverNameZKPO;
    private CalculatedField FieldObjAddress;
    private CalculatedField FieldRentAgreement;
    private CalculatedField FieldRenterNameZKPO;
    private CalculatedField FieldObjAddress2;
    private CalculatedField FieldRentAgreement2;
    private XRTableCell xrTableCell96;
    private XRTableCell xrTableCell94;
    private XRTableCell xrTableCell95;
    private XRTableCell xrTableCell92;
    private XRTableCell xrTableCell88;
    private XRTableCell xrTableCell90;
    private XRTableCell xrTableCell93;
    private XRTableCell xrTableCell89;
    private XRTableCell xrTableCell91;
    private CalculatedField FieldRentStart;
    private CalculatedField FieldRentFinish;
    private CalculatedField FieldRentStart2;
    private CalculatedField FieldRentFinish2;
    private XRTableCell xrTableCell72;

    public ReportOrgCard(string organizationIdStr)
	{
        InitializeComponent();

        if (!string.IsNullOrEmpty(organizationIdStr) && int.TryParse(organizationIdStr, out organizationId) && organizationId > 0)
        {
            OrgCardDB dataset = orgCardDB;

            (new OrgCardDBTableAdapters.OrgCardPropertiesTableAdapter()).FillByBID(dataset.OrgCardProperties, organizationId);
            (new OrgCardDBTableAdapters.OrgCardBalansTableAdapter()).FillByBID(dataset.OrgCardBalans, organizationId);
            (new OrgCardDBTableAdapters.OrgCardRentTableAdapter()).FillByBID(dataset.OrgCardRent, organizationId);
            (new OrgCardDBTableAdapters.OrgCardGiven4RentTableAdapter()).FillByBID(dataset.OrgCardGiven4Rent, organizationId);
            (new OrgCardDBTableAdapters.OrgCardPrivatTableAdapter()).FillByBID(dataset.OrgCardPrivat, organizationId);
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
        string resourceFileName = "ReportOrgCard.resx";
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel67 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel66 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
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
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.Date = new DevExpress.XtraReports.Parameters.Parameter();
        this.orgCardDB = new OrgCardDB();
        this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
        this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader2 = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
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
        this.DetailReport1 = new DevExpress.XtraReports.UI.DetailReportBand();
        this.Detail2 = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell96 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell94 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader3 = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
        this.DetailReport3 = new DevExpress.XtraReports.UI.DetailReportBand();
        this.Detail4 = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader4 = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
        this.FieldGiverNameZKPO = new DevExpress.XtraReports.UI.CalculatedField();
        this.FieldObjAddress = new DevExpress.XtraReports.UI.CalculatedField();
        this.FieldRentAgreement = new DevExpress.XtraReports.UI.CalculatedField();
        this.FieldRenterNameZKPO = new DevExpress.XtraReports.UI.CalculatedField();
        this.FieldObjAddress2 = new DevExpress.XtraReports.UI.CalculatedField();
        this.FieldRentAgreement2 = new DevExpress.XtraReports.UI.CalculatedField();
        this.FieldRentStart = new DevExpress.XtraReports.UI.CalculatedField();
        this.FieldRentFinish = new DevExpress.XtraReports.UI.CalculatedField();
        this.FieldRentStart2 = new DevExpress.XtraReports.UI.CalculatedField();
        this.FieldRentFinish2 = new DevExpress.XtraReports.UI.CalculatedField();
        ((System.ComponentModel.ISupportInitialize)(this.orgCardDB)).BeginInit();
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
            this.xrLabel39,
            this.xrLabel38,
            this.xrLabel37,
            this.xrLabel46,
            this.xrLabel44,
            this.xrLabel40,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel4,
            this.xrLabel36,
            this.xrLabel34,
            this.xrLabel32,
            this.xrLabel64,
            this.xrLabel63,
            this.xrLabel62,
            this.xrLabel67,
            this.xrLabel66,
            this.xrLabel65,
            this.xrLabel61,
            this.xrLabel57,
            this.xrLabel56,
            this.xrLabel55,
            this.xrLabel60,
            this.xrLabel59,
            this.xrLabel58,
            this.xrLabel52,
            this.xrLabel54,
            this.xrLabel49,
            this.xrLabel43,
            this.xrLabel45,
            this.xrLabel48,
            this.xrLabel35,
            this.xrLabel41,
            this.xrLabel42,
            this.xrLabel33,
            this.xrLabel27,
            this.xrLabel28,
            this.xrLabel30,
            this.xrLabel26,
            this.xrLabel50,
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
        this.Detail.HeightF = 423.7915F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel39
        // 
        this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(517.7302F, 107.3749F);
        this.xrLabel39.Name = "xrLabel39";
        this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel39.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel39.StylePriority.UseFont = false;
        this.xrLabel39.Text = "Факс:";
        // 
        // xrLabel38
        // 
        this.xrLabel38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.buhgalter_phone")});
        this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(681.2717F, 89.37495F);
        this.xrLabel38.Name = "xrLabel38";
        this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel38.SizeF = new System.Drawing.SizeF(274.6268F, 18F);
        this.xrLabel38.StylePriority.UseFont = false;
        this.xrLabel38.Text = "xrLabel38";
        // 
        // xrLabel37
        // 
        this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(517.7302F, 89.37495F);
        this.xrLabel37.Name = "xrLabel37";
        this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel37.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel37.StylePriority.UseFont = false;
        this.xrLabel37.Text = "Тел. Бухгалтера:";
        // 
        // xrLabel46
        // 
        this.xrLabel46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.contact_email")});
        this.xrLabel46.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(681.2717F, 125.3749F);
        this.xrLabel46.Name = "xrLabel46";
        this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel46.SizeF = new System.Drawing.SizeF(274.6268F, 18F);
        this.xrLabel46.StylePriority.UseFont = false;
        this.xrLabel46.Text = "xrLabel46";
        // 
        // xrLabel44
        // 
        this.xrLabel44.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(517.7302F, 125.3749F);
        this.xrLabel44.Name = "xrLabel44";
        this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel44.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel44.StylePriority.UseFont = false;
        this.xrLabel44.Text = "Email:";
        // 
        // xrLabel40
        // 
        this.xrLabel40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.fax")});
        this.xrLabel40.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(681.2717F, 107.3749F);
        this.xrLabel40.Name = "xrLabel40";
        this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel40.SizeF = new System.Drawing.SizeF(274.6268F, 18F);
        this.xrLabel40.StylePriority.UseFont = false;
        this.xrLabel40.Text = "xrLabel40";
        // 
        // xrLabel19
        // 
        this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(517.7302F, 53.37502F);
        this.xrLabel19.Name = "xrLabel19";
        this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel19.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel19.StylePriority.UseFont = false;
        this.xrLabel19.Text = "Тел. Директора:";
        // 
        // xrLabel18
        // 
        this.xrLabel18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.director_fio")});
        this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(681.2717F, 35.37499F);
        this.xrLabel18.Name = "xrLabel18";
        this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel18.SizeF = new System.Drawing.SizeF(274.6268F, 18F);
        this.xrLabel18.StylePriority.UseFont = false;
        this.xrLabel18.Text = "xrLabel18";
        // 
        // xrLabel4
        // 
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(517.7302F, 35.37499F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.Text = "Директор:";
        // 
        // xrLabel36
        // 
        this.xrLabel36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.buhgalter_fio")});
        this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(681.2717F, 71.37498F);
        this.xrLabel36.Name = "xrLabel36";
        this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel36.SizeF = new System.Drawing.SizeF(274.6268F, 18F);
        this.xrLabel36.StylePriority.UseFont = false;
        this.xrLabel36.Text = "xrLabel36";
        // 
        // xrLabel34
        // 
        this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(517.7302F, 71.37498F);
        this.xrLabel34.Name = "xrLabel34";
        this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel34.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel34.StylePriority.UseFont = false;
        this.xrLabel34.Text = "Бухгалтер:";
        // 
        // xrLabel32
        // 
        this.xrLabel32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.director_phone")});
        this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(681.2717F, 53.37502F);
        this.xrLabel32.Name = "xrLabel32";
        this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel32.SizeF = new System.Drawing.SizeF(274.6268F, 17.99999F);
        this.xrLabel32.StylePriority.UseFont = false;
        this.xrLabel32.Text = "xrLabel32";
        // 
        // xrLabel64
        // 
        this.xrLabel64.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.gosp_struct_type")});
        this.xrLabel64.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 309.4166F);
        this.xrLabel64.Name = "xrLabel64";
        this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel64.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel64.StylePriority.UseFont = false;
        this.xrLabel64.Text = "xrLabel64";
        // 
        // xrLabel63
        // 
        this.xrLabel63.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.kved_code")});
        this.xrLabel63.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 327.4166F);
        this.xrLabel63.Name = "xrLabel63";
        this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel63.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel63.StylePriority.UseFont = false;
        this.xrLabel63.Text = "xrLabel63";
        // 
        // xrLabel62
        // 
        this.xrLabel62.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.org_form")});
        this.xrLabel62.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 345.4165F);
        this.xrLabel62.Name = "xrLabel62";
        this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel62.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel62.StylePriority.UseFont = false;
        this.xrLabel62.Text = "xrLabel62";
        // 
        // xrLabel67
        // 
        this.xrLabel67.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.vedomstvo")});
        this.xrLabel67.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel67.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 381.4164F);
        this.xrLabel67.Name = "xrLabel67";
        this.xrLabel67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel67.SizeF = new System.Drawing.SizeF(755.8767F, 18F);
        this.xrLabel67.StylePriority.UseFont = false;
        this.xrLabel67.Text = "xrLabel67";
        // 
        // xrLabel66
        // 
        this.xrLabel66.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.old_organ")});
        this.xrLabel66.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel66.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 399.4165F);
        this.xrLabel66.Name = "xrLabel66";
        this.xrLabel66.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel66.SizeF = new System.Drawing.SizeF(755.8767F, 18F);
        this.xrLabel66.StylePriority.UseFont = false;
        this.xrLabel66.Text = "xrLabel66";
        // 
        // xrLabel65
        // 
        this.xrLabel65.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.sfera_upr")});
        this.xrLabel65.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 363.4165F);
        this.xrLabel65.Name = "xrLabel65";
        this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel65.SizeF = new System.Drawing.SizeF(755.8767F, 18F);
        this.xrLabel65.StylePriority.UseFont = false;
        this.xrLabel65.Text = "xrLabel65";
        // 
        // xrLabel61
        // 
        this.xrLabel61.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.form_gosp")});
        this.xrLabel61.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 255.4165F);
        this.xrLabel61.Name = "xrLabel61";
        this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel61.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel61.StylePriority.UseFont = false;
        this.xrLabel61.Text = "xrLabel61";
        // 
        // xrLabel57
        // 
        this.xrLabel57.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.industry")});
        this.xrLabel57.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 183.4167F);
        this.xrLabel57.Name = "xrLabel57";
        this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel57.SizeF = new System.Drawing.SizeF(755.8767F, 18F);
        this.xrLabel57.StylePriority.UseFont = false;
        this.xrLabel57.Text = "xrLabel57";
        // 
        // xrLabel56
        // 
        this.xrLabel56.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.occupation")});
        this.xrLabel56.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 201.4167F);
        this.xrLabel56.Name = "xrLabel56";
        this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel56.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel56.StylePriority.UseFont = false;
        this.xrLabel56.Text = "xrLabel56";
        // 
        // xrLabel55
        // 
        this.xrLabel55.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.old_industry")});
        this.xrLabel55.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 219.4167F);
        this.xrLabel55.Name = "xrLabel55";
        this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel55.SizeF = new System.Drawing.SizeF(755.8768F, 18.00002F);
        this.xrLabel55.StylePriority.UseFont = false;
        this.xrLabel55.Text = "xrLabel55";
        // 
        // xrLabel60
        // 
        this.xrLabel60.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.form_of_ownership")});
        this.xrLabel60.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 273.4165F);
        this.xrLabel60.Name = "xrLabel60";
        this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel60.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel60.StylePriority.UseFont = false;
        this.xrLabel60.Text = "xrLabel60";
        // 
        // xrLabel59
        // 
        this.xrLabel59.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.gosp_struct")});
        this.xrLabel59.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 291.4166F);
        this.xrLabel59.Name = "xrLabel59";
        this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel59.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel59.StylePriority.UseFont = false;
        this.xrLabel59.Text = "xrLabel59";
        // 
        // xrLabel58
        // 
        this.xrLabel58.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.old_occupation")});
        this.xrLabel58.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 237.4166F);
        this.xrLabel58.Name = "xrLabel58";
        this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel58.SizeF = new System.Drawing.SizeF(755.8768F, 17.99997F);
        this.xrLabel58.StylePriority.UseFont = false;
        this.xrLabel58.Text = "xrLabel58";
        // 
        // xrLabel52
        // 
        this.xrLabel52.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 381.4164F);
        this.xrLabel52.Name = "xrLabel52";
        this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel52.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel52.StylePriority.UseFont = false;
        this.xrLabel52.Text = "Орган управління:";
        // 
        // xrLabel54
        // 
        this.xrLabel54.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 399.4165F);
        this.xrLabel54.Name = "xrLabel54";
        this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel54.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel54.StylePriority.UseFont = false;
        this.xrLabel54.Text = "Орган госп. упр.:";
        // 
        // xrLabel49
        // 
        this.xrLabel49.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 363.4165F);
        this.xrLabel49.Name = "xrLabel49";
        this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel49.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel49.StylePriority.UseFont = false;
        this.xrLabel49.Text = "Сфера управління:";
        // 
        // xrLabel43
        // 
        this.xrLabel43.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 309.4166F);
        this.xrLabel43.Name = "xrLabel43";
        this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel43.SizeF = new System.Drawing.SizeF(163.5416F, 18F);
        this.xrLabel43.StylePriority.UseFont = false;
        this.xrLabel43.Text = "Вид Госп. Структури:";
        // 
        // xrLabel45
        // 
        this.xrLabel45.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 327.4166F);
        this.xrLabel45.Name = "xrLabel45";
        this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel45.SizeF = new System.Drawing.SizeF(163.5417F, 18F);
        this.xrLabel45.StylePriority.UseFont = false;
        this.xrLabel45.Text = "КВЕД:";
        // 
        // xrLabel48
        // 
        this.xrLabel48.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 345.4166F);
        this.xrLabel48.Name = "xrLabel48";
        this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel48.SizeF = new System.Drawing.SizeF(163.5417F, 18F);
        this.xrLabel48.StylePriority.UseFont = false;
        this.xrLabel48.Text = "Орг.-правова форма госп.:";
        // 
        // xrLabel35
        // 
        this.xrLabel35.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 255.4165F);
        this.xrLabel35.Name = "xrLabel35";
        this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel35.SizeF = new System.Drawing.SizeF(163.5416F, 18F);
        this.xrLabel35.StylePriority.UseFont = false;
        this.xrLabel35.Text = "Форма фінансування:";
        // 
        // xrLabel41
        // 
        this.xrLabel41.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 273.4165F);
        this.xrLabel41.Name = "xrLabel41";
        this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel41.SizeF = new System.Drawing.SizeF(163.5416F, 18F);
        this.xrLabel41.StylePriority.UseFont = false;
        this.xrLabel41.Text = "Форма Власності:";
        // 
        // xrLabel42
        // 
        this.xrLabel42.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 291.4166F);
        this.xrLabel42.Name = "xrLabel42";
        this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel42.SizeF = new System.Drawing.SizeF(163.5416F, 18F);
        this.xrLabel42.StylePriority.UseFont = false;
        this.xrLabel42.Text = "Госп. Структура:";
        // 
        // xrLabel33
        // 
        this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 237.4166F);
        this.xrLabel33.Name = "xrLabel33";
        this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel33.SizeF = new System.Drawing.SizeF(163.5416F, 17.99997F);
        this.xrLabel33.StylePriority.UseFont = false;
        this.xrLabel33.Text = "Вид Діяльності (Баланс):";
        // 
        // xrLabel27
        // 
        this.xrLabel27.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 183.4167F);
        this.xrLabel27.Name = "xrLabel27";
        this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel27.SizeF = new System.Drawing.SizeF(163.5416F, 18F);
        this.xrLabel27.StylePriority.UseFont = false;
        this.xrLabel27.Text = "Галузь:";
        // 
        // xrLabel28
        // 
        this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 201.4167F);
        this.xrLabel28.Name = "xrLabel28";
        this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel28.SizeF = new System.Drawing.SizeF(163.5416F, 18F);
        this.xrLabel28.StylePriority.UseFont = false;
        this.xrLabel28.Text = "Вид Діяльності:";
        // 
        // xrLabel30
        // 
        this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 219.4167F);
        this.xrLabel30.Name = "xrLabel30";
        this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel30.SizeF = new System.Drawing.SizeF(163.5416F, 18.00002F);
        this.xrLabel30.StylePriority.UseFont = false;
        this.xrLabel30.Text = "Галузь (Баланс):";
        // 
        // xrLabel26
        // 
        this.xrLabel26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(0F, 152F);
        this.xrLabel26.Name = "xrLabel26";
        this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel26.SizeF = new System.Drawing.SizeF(278.125F, 23F);
        this.xrLabel26.StylePriority.UseFont = false;
        this.xrLabel26.StylePriority.UseTextAlignment = false;
        this.xrLabel26.Text = "II. Додаткові відомості";
        this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel50
        // 
        this.xrLabel50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(0.4167557F, 0F);
        this.xrLabel50.Name = "xrLabel50";
        this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel50.SizeF = new System.Drawing.SizeF(278.125F, 23F);
        this.xrLabel50.StylePriority.UseFont = false;
        this.xrLabel50.StylePriority.UseTextAlignment = false;
        this.xrLabel50.Text = "I. Адреса та контактна інформація:";
        this.xrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel17
        // 
        this.xrLabel17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.addr_zip_code")});
        this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 125.3749F);
        this.xrLabel17.Name = "xrLabel17";
        this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel17.SizeF = new System.Drawing.SizeF(282.6976F, 18F);
        this.xrLabel17.StylePriority.UseFont = false;
        this.xrLabel17.Text = "xrLabel17";
        // 
        // xrLabel16
        // 
        this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 125.3749F);
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel16.StylePriority.UseFont = false;
        this.xrLabel16.Text = "Поштовий Індекс:";
        // 
        // xrLabel15
        // 
        this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.addr_korpus")});
        this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 107.3749F);
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(282.6976F, 18F);
        this.xrLabel15.StylePriority.UseFont = false;
        this.xrLabel15.Text = "xrLabel15";
        // 
        // xrLabel14
        // 
        this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 107.3749F);
        this.xrLabel14.Name = "xrLabel14";
        this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel14.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel14.StylePriority.UseFont = false;
        this.xrLabel14.Text = "Корпус:";
        // 
        // xrLabel13
        // 
        this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.addr_nomer")});
        this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 89.37495F);
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(282.6976F, 18F);
        this.xrLabel13.StylePriority.UseFont = false;
        this.xrLabel13.Text = "xrLabel13";
        // 
        // xrLabel12
        // 
        this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 89.37498F);
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel12.StylePriority.UseFont = false;
        this.xrLabel12.Text = "Номер Будинку:";
        // 
        // xrLabel11
        // 
        this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.addr_street_name")});
        this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 71.37498F);
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(282.6976F, 18F);
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.Text = "xrLabel11";
        // 
        // xrLabel10
        // 
        this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 71.37498F);
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.Text = "Назва Вулиці:";
        // 
        // xrLabel9
        // 
        this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.addr_district")});
        this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 53.37499F);
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(282.6976F, 18F);
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.Text = "xrLabel9";
        // 
        // xrLabel8
        // 
        this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 53.37499F);
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.Text = "Район:";
        // 
        // xrLabel7
        // 
        this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.addr_city")});
        this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(201.8024F, 35.37499F);
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(282.6976F, 18F);
        this.xrLabel7.StylePriority.UseFont = false;
        this.xrLabel7.Text = "xrLabel7";
        // 
        // xrLabel6
        // 
        this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(38.26081F, 35.37499F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.Text = "Місто:";
        // 
        // TopMargin
        // 
        this.TopMargin.HeightF = 8.708334F;
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
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel31,
            this.xrLabel29,
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel2,
            this.xrLabel3,
            this.xrLabel1});
        this.ReportHeader.HeightF = 117.7084F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrLabel25
        // 
        this.xrLabel25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.short_name")});
        this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(201.8023F, 76.83333F);
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.Text = "xrLabel25";
        // 
        // xrLabel24
        // 
        this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 76.83333F);
        this.xrLabel24.Name = "xrLabel24";
        this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel24.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel24.StylePriority.UseFont = false;
        this.xrLabel24.Text = "Коротка Назва:";
        // 
        // xrLabel31
        // 
        this.xrLabel31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.status")});
        this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(201.8023F, 94.83333F);
        this.xrLabel31.Name = "xrLabel31";
        this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel31.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel31.StylePriority.UseFont = false;
        this.xrLabel31.Text = "xrLabel31";
        // 
        // xrLabel29
        // 
        this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(38.26094F, 94.83333F);
        this.xrLabel29.Name = "xrLabel29";
        this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel29.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel29.StylePriority.UseFont = false;
        this.xrLabel29.Text = "Фіз. / Юр. Особа:";
        // 
        // xrLabel21
        // 
        this.xrLabel21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.zkpo_code")});
        this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(201.8023F, 40.83333F);
        this.xrLabel21.Name = "xrLabel21";
        this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel21.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel21.StylePriority.UseFont = false;
        this.xrLabel21.Text = "xrLabel21";
        // 
        // xrLabel20
        // 
        this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 40.83333F);
        this.xrLabel20.Name = "xrLabel20";
        this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel20.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel20.StylePriority.UseFont = false;
        this.xrLabel20.Text = "Код ЄДРПОУ:";
        // 
        // xrLabel23
        // 
        this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardProperties.full_name")});
        this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(201.8023F, 58.83333F);
        this.xrLabel23.Name = "xrLabel23";
        this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel23.SizeF = new System.Drawing.SizeF(755.8768F, 18F);
        this.xrLabel23.StylePriority.UseFont = false;
        this.xrLabel23.Text = "xrLabel23";
        // 
        // xrLabel22
        // 
        this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(38.26072F, 58.83333F);
        this.xrLabel22.Name = "xrLabel22";
        this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel22.SizeF = new System.Drawing.SizeF(163.5415F, 18F);
        this.xrLabel22.StylePriority.UseFont = false;
        this.xrLabel22.Text = "Повна Назва:";
        // 
        // xrLabel2
        // 
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(150F, 20.95833F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(659.375F, 19.875F);
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "КАРТКА ОРГАНІЗАЦІЇ";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel3
        // 
        this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.Date, "Text", "")});
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(819.375F, 18.83334F);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(780F, 18.83334F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
//        this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 18.83334F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(200F, 20F);
        this.xrLabel3.StylePriority.UseBorders = false;
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "xrLabel3";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel1
        // 
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(780F, 0F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 18.83334F);
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "Дата видачі:";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // Date
        // 
        this.Date.Description = "The date the report was generated";
        this.Date.Name = "Date";
        this.Date.Type = typeof(System.DateTime);
        this.Date.ValueInfo = System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        // 
        // orgCardDB
        // 
        this.orgCardDB.DataSetName = "OrgCardDB";
        this.orgCardDB.EnforceConstraints = false;
        this.orgCardDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // DetailReport
        // 
        this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1,
            this.ReportHeader2});
        this.DetailReport.DataMember = "OrgCardBalans";
        this.DetailReport.DataSource = this.orgCardDB;
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
        this.xrTable1.SizeF = new System.Drawing.SizeF(955.8986F, 25F);
        this.xrTable1.StylePriority.UseBorders = false;
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell8,
            this.xrTableCell84,
            this.xrTableCell83,
            this.xrTableCell10,
            this.xrTableCell12});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardBalans.district")});
        this.xrTableCell1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell1.Multiline = true;
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UsePadding = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "xrTableCell1";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell1.Weight = 0.39580236150797493D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardBalans.street_full_name")});
        this.xrTableCell2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UsePadding = false;
        this.xrTableCell2.StylePriority.UseTextAlignment = false;
        this.xrTableCell2.Text = "xrTableCell2";
        this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell2.Weight = 0.53672964198879725D;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardBalans.addr_nomer")});
        this.xrTableCell3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell3.Multiline = true;
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell3.StylePriority.UseFont = false;
        this.xrTableCell3.StylePriority.UsePadding = false;
        this.xrTableCell3.StylePriority.UseTextAlignment = false;
        this.xrTableCell3.Text = "xrTableCell3";
        this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell3.Weight = 0.2837993282194532D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardBalans.sqr_total")});
        this.xrTableCell8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UsePadding = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        this.xrTableCell8.Text = "xrTableCell8";
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell8.Weight = 0.34655525357280148D;
        // 
        // xrTableCell84
        // 
        this.xrTableCell84.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardBalans.object_type")});
        this.xrTableCell84.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell84.Name = "xrTableCell84";
        this.xrTableCell84.StylePriority.UseFont = false;
        this.xrTableCell84.StylePriority.UseTextAlignment = false;
        this.xrTableCell84.Text = "xrTableCell84";
        this.xrTableCell84.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell84.Weight = 0.37503410630838585D;
        // 
        // xrTableCell83
        // 
        this.xrTableCell83.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardBalans.object_kind")});
        this.xrTableCell83.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell83.Name = "xrTableCell83";
        this.xrTableCell83.StylePriority.UseFont = false;
        this.xrTableCell83.StylePriority.UseTextAlignment = false;
        this.xrTableCell83.Text = "xrTableCell83";
        this.xrTableCell83.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell83.Weight = 0.36993766918846305D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardBalans.purpose")});
        this.xrTableCell10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.StylePriority.UsePadding = false;
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        this.xrTableCell10.Text = "xrTableCell10";
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell10.Weight = 0.368870402383209D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardBalans.history")});
        this.xrTableCell12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.StylePriority.UsePadding = false;
        this.xrTableCell12.StylePriority.UseTextAlignment = false;
        this.xrTableCell12.Text = "xrTableCell12";
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell12.Weight = 0.282709690717815D;
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
        this.xrTable4.SizeF = new System.Drawing.SizeF(955.8986F, 48.25001F);
        this.xrTable4.StylePriority.UseBorders = false;
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21,
            this.xrTableCell22,
            this.xrTableCell79,
            this.xrTableCell81,
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
        this.xrTableCell19.Text = "Район";
        this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell19.Weight = 0.39580237299723103D;
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell20.Multiline = true;
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.StylePriority.UseFont = false;
        this.xrTableCell20.StylePriority.UseTextAlignment = false;
        this.xrTableCell20.Text = "Назва Вулиці";
        this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell20.Weight = 0.53672980524347857D;
        // 
        // xrTableCell21
        // 
        this.xrTableCell21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell21.Multiline = true;
        this.xrTableCell21.Name = "xrTableCell21";
        this.xrTableCell21.StylePriority.UseFont = false;
        this.xrTableCell21.StylePriority.UseTextAlignment = false;
        this.xrTableCell21.Text = "Номер";
        this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell21.Weight = 0.28379934031820653D;
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell22.Multiline = true;
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.StylePriority.UseTextAlignment = false;
        this.xrTableCell22.Text = "Площа На Балансі, кв.м.";
        this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell22.Weight = 0.34655501608375272D;
        // 
        // xrTableCell79
        // 
        this.xrTableCell79.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell79.Name = "xrTableCell79";
        this.xrTableCell79.StylePriority.UseFont = false;
        this.xrTableCell79.StylePriority.UseTextAlignment = false;
        this.xrTableCell79.Text = "Тип Об\'єкту";
        this.xrTableCell79.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell79.Weight = 0.37503443893432381D;
        // 
        // xrTableCell81
        // 
        this.xrTableCell81.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell81.Name = "xrTableCell81";
        this.xrTableCell81.StylePriority.UseFont = false;
        this.xrTableCell81.StylePriority.UseTextAlignment = false;
        this.xrTableCell81.Text = "Вид Об\'єкту";
        this.xrTableCell81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell81.Weight = 0.36993781378206664D;
        // 
        // xrTableCell23
        // 
        this.xrTableCell23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell23.Multiline = true;
        this.xrTableCell23.Name = "xrTableCell23";
        this.xrTableCell23.StylePriority.UseFont = false;
        this.xrTableCell23.StylePriority.UseTextAlignment = false;
        this.xrTableCell23.Text = "Призначення";
        this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell23.Weight = 0.36887045132843038D;
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell24.Multiline = true;
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.StylePriority.UseFont = false;
        this.xrTableCell24.StylePriority.UseTextAlignment = false;
        this.xrTableCell24.Text = "Історична\r\nЦінність";
        this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell24.Weight = 0.28270939905300224D;
        // 
        // xrTableRow5
        // 
        this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCell28,
            this.xrTableCell80,
            this.xrTableCell82,
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
        this.xrTableCell25.Weight = 0.39580239661765049D;
        // 
        // xrTableCell26
        // 
        this.xrTableCell26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell26.Name = "xrTableCell26";
        this.xrTableCell26.StylePriority.UseFont = false;
        this.xrTableCell26.StylePriority.UseTextAlignment = false;
        this.xrTableCell26.Text = "2";
        this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell26.Weight = 0.53672978162305907D;
        // 
        // xrTableCell27
        // 
        this.xrTableCell27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell27.Name = "xrTableCell27";
        this.xrTableCell27.StylePriority.UseFont = false;
        this.xrTableCell27.StylePriority.UseTextAlignment = false;
        this.xrTableCell27.Text = "3";
        this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell27.Weight = 0.28379934031820642D;
        // 
        // xrTableCell28
        // 
        this.xrTableCell28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell28.Name = "xrTableCell28";
        this.xrTableCell28.StylePriority.UseFont = false;
        this.xrTableCell28.StylePriority.UseTextAlignment = false;
        this.xrTableCell28.Text = "4";
        this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell28.Weight = 0.3465550160837525D;
        // 
        // xrTableCell80
        // 
        this.xrTableCell80.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell80.Name = "xrTableCell80";
        this.xrTableCell80.StylePriority.UseFont = false;
        this.xrTableCell80.StylePriority.UseTextAlignment = false;
        this.xrTableCell80.Text = "5";
        this.xrTableCell80.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell80.Weight = 0.37503443893432381D;
        // 
        // xrTableCell82
        // 
        this.xrTableCell82.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell82.Name = "xrTableCell82";
        this.xrTableCell82.StylePriority.UseFont = false;
        this.xrTableCell82.StylePriority.UseTextAlignment = false;
        this.xrTableCell82.Text = "6";
        this.xrTableCell82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell82.Weight = 0.36993781378206664D;
        // 
        // xrTableCell29
        // 
        this.xrTableCell29.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell29.Name = "xrTableCell29";
        this.xrTableCell29.StylePriority.UseFont = false;
        this.xrTableCell29.StylePriority.UseTextAlignment = false;
        this.xrTableCell29.Text = "7";
        this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell29.Weight = 0.36887045132843038D;
        // 
        // xrTableCell30
        // 
        this.xrTableCell30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell30.Name = "xrTableCell30";
        this.xrTableCell30.StylePriority.UseFont = false;
        this.xrTableCell30.StylePriority.UseTextAlignment = false;
        this.xrTableCell30.Text = "8";
        this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell30.Weight = 0.2827093990530023D;
        // 
        // xrLabel5
        // 
        this.xrLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0.4167557F, 0F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(526.0417F, 20F);
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.Text = "III. Перелік об\'єктів на балансі:";
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // DetailReport2
        // 
        this.DetailReport2.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail3,
            this.ReportHeader1});
        this.DetailReport2.DataMember = "OrgCardRent";
        this.DetailReport2.DataSource = this.orgCardDB;
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
        this.xrTable2.SizeF = new System.Drawing.SizeF(955.8986F, 25F);
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
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardRent.FieldGiverNameZKPO")});
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
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardRent.FieldObjAddress")});
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
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardRent.rent_square")});
        this.xrTableCell37.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell37.Multiline = true;
        this.xrTableCell37.Name = "xrTableCell37";
        this.xrTableCell37.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell37.StylePriority.UseFont = false;
        this.xrTableCell37.StylePriority.UsePadding = false;
        this.xrTableCell37.StylePriority.UseTextAlignment = false;
        this.xrTableCell37.Text = "xrTableCell37";
        this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell37.Weight = 0.34365365108732038D;
        // 
        // xrTableCell38
        // 
        this.xrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardRent.object_name")});
        this.xrTableCell38.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell38.Multiline = true;
        this.xrTableCell38.Name = "xrTableCell38";
        this.xrTableCell38.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell38.StylePriority.UseFont = false;
        this.xrTableCell38.StylePriority.UsePadding = false;
        this.xrTableCell38.StylePriority.UseTextAlignment = false;
        this.xrTableCell38.Text = "xrTableCell38";
        this.xrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell38.Weight = 0.43944327194722227D;
        // 
        // xrTableCell39
        // 
        this.xrTableCell39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardRent.FieldRentAgreement")});
        this.xrTableCell39.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell39.Multiline = true;
        this.xrTableCell39.Name = "xrTableCell39";
        this.xrTableCell39.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell39.StylePriority.UseFont = false;
        this.xrTableCell39.StylePriority.UsePadding = false;
        this.xrTableCell39.StylePriority.UseTextAlignment = false;
        this.xrTableCell39.Text = "xrTableCell39";
        this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell39.Weight = 0.42781611475250453D;
        // 
        // xrTableCell40
        // 
        this.xrTableCell40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardRent.FieldRentStart")});
        this.xrTableCell40.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell40.Multiline = true;
        this.xrTableCell40.Name = "xrTableCell40";
        this.xrTableCell40.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell40.StylePriority.UseFont = false;
        this.xrTableCell40.StylePriority.UsePadding = false;
        this.xrTableCell40.StylePriority.UseTextAlignment = false;
        this.xrTableCell40.Text = "xrTableCell40";
        this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell40.Weight = 0.29238961044742123D;
        // 
        // xrTableCell41
        // 
        this.xrTableCell41.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardRent.FieldRentFinish")});
        this.xrTableCell41.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell41.Multiline = true;
        this.xrTableCell41.Name = "xrTableCell41";
        this.xrTableCell41.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell41.StylePriority.UseFont = false;
        this.xrTableCell41.StylePriority.UsePadding = false;
        this.xrTableCell41.StylePriority.UseTextAlignment = false;
        this.xrTableCell41.Text = "xrTableCell41";
        this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell41.Weight = 0.2890871315609192D;
        // 
        // xrTableCell42
        // 
        this.xrTableCell42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardRent.is_subarenda")});
        this.xrTableCell42.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell42.Multiline = true;
        this.xrTableCell42.Name = "xrTableCell42";
        this.xrTableCell42.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell42.StylePriority.UseFont = false;
        this.xrTableCell42.StylePriority.UsePadding = false;
        this.xrTableCell42.StylePriority.UseTextAlignment = false;
        this.xrTableCell42.Text = "xrTableCell42";
        this.xrTableCell42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell42.Weight = 0.2145114885337879D;
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
        this.xrTable3.SizeF = new System.Drawing.SizeF(955.8986F, 50F);
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
        this.xrTableCell43.Multiline = true;
        this.xrTableCell43.Name = "xrTableCell43";
        this.xrTableCell43.StylePriority.UseFont = false;
        this.xrTableCell43.StylePriority.UseTextAlignment = false;
        this.xrTableCell43.Text = "Назва орендодавця,\r\nкод ЄДРПОУ";
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
        this.xrTableCell44.Text = "Адреса об\'єкту";
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
        this.xrTableCell45.Text = "Орендована площа, кв.м.";
        this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell45.Weight = 0.33552650475280571D;
        // 
        // xrTableCell46
        // 
        this.xrTableCell46.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell46.Multiline = true;
        this.xrTableCell46.Name = "xrTableCell46";
        this.xrTableCell46.StylePriority.UseFont = false;
        this.xrTableCell46.StylePriority.UseTextAlignment = false;
        this.xrTableCell46.Text = "Використання приміщення";
        this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell46.Weight = 0.42905031564434992D;
        // 
        // xrTableCell47
        // 
        this.xrTableCell47.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell47.Multiline = true;
        this.xrTableCell47.Name = "xrTableCell47";
        this.xrTableCell47.StylePriority.UseFont = false;
        this.xrTableCell47.StylePriority.UseTextAlignment = false;
        this.xrTableCell47.Text = "Договір (номер, дата)";
        this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell47.Weight = 0.41769872139851005D;
        // 
        // xrTableCell48
        // 
        this.xrTableCell48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell48.Multiline = true;
        this.xrTableCell48.Name = "xrTableCell48";
        this.xrTableCell48.StylePriority.UseFont = false;
        this.xrTableCell48.StylePriority.UseTextAlignment = false;
        this.xrTableCell48.Text = "Початок оренди";
        this.xrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell48.Weight = 0.28547465321449306D;
        // 
        // xrTableCell49
        // 
        this.xrTableCell49.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell49.Multiline = true;
        this.xrTableCell49.Name = "xrTableCell49";
        this.xrTableCell49.StylePriority.UseFont = false;
        this.xrTableCell49.StylePriority.UseTextAlignment = false;
        this.xrTableCell49.Text = "Закінчення оренди";
        this.xrTableCell49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell49.Weight = 0.28224980498984131D;
        // 
        // xrTableCell50
        // 
        this.xrTableCell50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell50.Multiline = true;
        this.xrTableCell50.Name = "xrTableCell50";
        this.xrTableCell50.StylePriority.UseFont = false;
        this.xrTableCell50.StylePriority.UseTextAlignment = false;
        this.xrTableCell50.Text = "Суб-\r\nоренда";
        this.xrTableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell50.Weight = 0.20943826492356812D;
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
        this.xrTableCell53.Weight = 0.33552650475280577D;
        // 
        // xrTableCell54
        // 
        this.xrTableCell54.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell54.Name = "xrTableCell54";
        this.xrTableCell54.StylePriority.UseFont = false;
        this.xrTableCell54.StylePriority.UseTextAlignment = false;
        this.xrTableCell54.Text = "4";
        this.xrTableCell54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell54.Weight = 0.42905031564434981D;
        // 
        // xrTableCell55
        // 
        this.xrTableCell55.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell55.Name = "xrTableCell55";
        this.xrTableCell55.StylePriority.UseFont = false;
        this.xrTableCell55.StylePriority.UseTextAlignment = false;
        this.xrTableCell55.Text = "5";
        this.xrTableCell55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell55.Weight = 0.41769872139851005D;
        // 
        // xrTableCell56
        // 
        this.xrTableCell56.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell56.Name = "xrTableCell56";
        this.xrTableCell56.StylePriority.UseFont = false;
        this.xrTableCell56.StylePriority.UseTextAlignment = false;
        this.xrTableCell56.Text = "6";
        this.xrTableCell56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell56.Weight = 0.28547465321449306D;
        // 
        // xrTableCell57
        // 
        this.xrTableCell57.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell57.Name = "xrTableCell57";
        this.xrTableCell57.StylePriority.UseFont = false;
        this.xrTableCell57.StylePriority.UseTextAlignment = false;
        this.xrTableCell57.Text = "7";
        this.xrTableCell57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell57.Weight = 0.28224980498984131D;
        // 
        // xrTableCell58
        // 
        this.xrTableCell58.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell58.Name = "xrTableCell58";
        this.xrTableCell58.StylePriority.UseFont = false;
        this.xrTableCell58.StylePriority.UseTextAlignment = false;
        this.xrTableCell58.Text = "8";
        this.xrTableCell58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell58.Weight = 0.20943826492356812D;
        // 
        // xrLabel51
        // 
        this.xrLabel51.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.999987F);
        this.xrLabel51.Name = "xrLabel51";
        this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel51.SizeF = new System.Drawing.SizeF(301.2078F, 18F);
        this.xrLabel51.StylePriority.UseFont = false;
        this.xrLabel51.StylePriority.UseTextAlignment = false;
        this.xrLabel51.Text = "IV. Перелік орендованих об\'єктів:";
        this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // DetailReport1
        // 
        this.DetailReport1.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail2,
            this.ReportHeader3});
        this.DetailReport1.DataMember = "OrgCardPrivat";
        this.DetailReport1.DataSource = this.orgCardDB;
        this.DetailReport1.Level = 3;
        this.DetailReport1.Name = "DetailReport1";
        // 
        // Detail2
        // 
        this.Detail2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
        this.Detail2.HeightF = 25F;
        this.Detail2.Name = "Detail2";
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
        this.xrTable6.SizeF = new System.Drawing.SizeF(955.8985F, 25F);
        this.xrTable6.StylePriority.UseBorders = false;
        // 
        // xrTableRow9
        // 
        this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell11,
            this.xrTableCell13,
            this.xrTableCell96,
            this.xrTableCell94,
            this.xrTableCell95,
            this.xrTableCell14,
            this.xrTableCell33});
        this.xrTableRow9.Name = "xrTableRow9";
        this.xrTableRow9.Weight = 1D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardPrivat.district")});
        this.xrTableCell9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell9.Multiline = true;
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UsePadding = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.Text = "xrTableCell9";
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell9.Weight = 0.27025984875631148D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardPrivat.street_full_name")});
        this.xrTableCell11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell11.Multiline = true;
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UsePadding = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "xrTableCell11";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell11.Weight = 0.36648725037376506D;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardPrivat.addr_nomer")});
        this.xrTableCell13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell13.Multiline = true;
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell13.StylePriority.UseFont = false;
        this.xrTableCell13.StylePriority.UsePadding = false;
        this.xrTableCell13.StylePriority.UseTextAlignment = false;
        this.xrTableCell13.Text = "xrTableCell13";
        this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell13.Weight = 0.19378234675579653D;
        // 
        // xrTableCell96
        // 
        this.xrTableCell96.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardPrivat.obj_name")});
        this.xrTableCell96.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell96.Name = "xrTableCell96";
        this.xrTableCell96.StylePriority.UseFont = false;
        this.xrTableCell96.StylePriority.UseTextAlignment = false;
        this.xrTableCell96.Text = "xrTableCell96";
        this.xrTableCell96.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell96.Weight = 0.33744381795466344D;
        // 
        // xrTableCell94
        // 
        this.xrTableCell94.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardPrivat.sqr_total")});
        this.xrTableCell94.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell94.Name = "xrTableCell94";
        this.xrTableCell94.StylePriority.UseFont = false;
        this.xrTableCell94.StylePriority.UseTextAlignment = false;
        this.xrTableCell94.Text = "xrTableCell94";
        this.xrTableCell94.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell94.Weight = 0.20591579076473571D;
        // 
        // xrTableCell95
        // 
        this.xrTableCell95.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardPrivat.obj_group")});
        this.xrTableCell95.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell95.Name = "xrTableCell95";
        this.xrTableCell95.StylePriority.UseFont = false;
        this.xrTableCell95.StylePriority.UseTextAlignment = false;
        this.xrTableCell95.Text = "xrTableCell95";
        this.xrTableCell95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell95.Weight = 0.20195174165432139D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardPrivat.privat_kind")});
        this.xrTableCell14.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell14.Multiline = true;
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell14.StylePriority.UseFont = false;
        this.xrTableCell14.StylePriority.UsePadding = false;
        this.xrTableCell14.StylePriority.UseTextAlignment = false;
        this.xrTableCell14.Text = "xrTableCell14";
        this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell14.Weight = 0.21927958664327871D;
        // 
        // xrTableCell33
        // 
        this.xrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardPrivat.cost")});
        this.xrTableCell33.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell33.Name = "xrTableCell33";
        this.xrTableCell33.StylePriority.UseFont = false;
        this.xrTableCell33.StylePriority.UseTextAlignment = false;
        this.xrTableCell33.Text = "xrTableCell33";
        this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell33.Weight = 0.22562886179399541D;
        // 
        // ReportHeader3
        // 
        this.ReportHeader3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable5,
            this.xrLabel53});
        this.ReportHeader3.HeightF = 72.99999F;
        this.ReportHeader3.Name = "ReportHeader3";
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
        this.xrTable5.SizeF = new System.Drawing.SizeF(955.8986F, 50F);
        this.xrTable5.StylePriority.UseBorders = false;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell92,
            this.xrTableCell88,
            this.xrTableCell90,
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
        this.xrTableCell4.Text = "Район";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell4.Weight = 0.26386822364893731D;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell5.Multiline = true;
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "Назва Вулиці";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell5.Weight = 0.35781983177728316D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell6.Multiline = true;
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "Номер";
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell6.Weight = 0.18919953227166175D;
        // 
        // xrTableCell92
        // 
        this.xrTableCell92.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell92.Name = "xrTableCell92";
        this.xrTableCell92.StylePriority.UseFont = false;
        this.xrTableCell92.StylePriority.UseTextAlignment = false;
        this.xrTableCell92.Text = "Назва Об\'єкту";
        this.xrTableCell92.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell92.Weight = 0.32946342334412693D;
        // 
        // xrTableCell88
        // 
        this.xrTableCell88.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell88.Name = "xrTableCell88";
        this.xrTableCell88.StylePriority.UseFont = false;
        this.xrTableCell88.StylePriority.UseTextAlignment = false;
        this.xrTableCell88.Text = "Площа, кв.м.";
        this.xrTableCell88.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell88.Weight = 0.20104564355745896D;
        // 
        // xrTableCell90
        // 
        this.xrTableCell90.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell90.Name = "xrTableCell90";
        this.xrTableCell90.StylePriority.UseFont = false;
        this.xrTableCell90.StylePriority.UseTextAlignment = false;
        this.xrTableCell90.Text = "Група Об\'єкту";
        this.xrTableCell90.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell90.Weight = 0.19717558003554525D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell7.Multiline = true;
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UseTextAlignment = false;
        this.xrTableCell7.Text = "Спосіб Приватизації";
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell7.Weight = 0.21409384901678608D;
        // 
        // xrTableCell31
        // 
        this.xrTableCell31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell31.Name = "xrTableCell31";
        this.xrTableCell31.StylePriority.UseFont = false;
        this.xrTableCell31.StylePriority.UseTextAlignment = false;
        this.xrTableCell31.Text = "Вартість, грн.";
        this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell31.Weight = 0.22029275963057926D;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell93,
            this.xrTableCell89,
            this.xrTableCell91,
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
        this.xrTableCell15.Weight = 0.26386823939588172D;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseFont = false;
        this.xrTableCell16.StylePriority.UseTextAlignment = false;
        this.xrTableCell16.Text = "2";
        this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell16.Weight = 0.35781980028339455D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseFont = false;
        this.xrTableCell17.StylePriority.UseTextAlignment = false;
        this.xrTableCell17.Text = "3";
        this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell17.Weight = 0.18919947322062053D;
        // 
        // xrTableCell93
        // 
        this.xrTableCell93.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell93.Name = "xrTableCell93";
        this.xrTableCell93.StylePriority.UseFont = false;
        this.xrTableCell93.StylePriority.UseTextAlignment = false;
        this.xrTableCell93.Text = "4";
        this.xrTableCell93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell93.Weight = 0.32946330130530832D;
        // 
        // xrTableCell89
        // 
        this.xrTableCell89.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell89.Name = "xrTableCell89";
        this.xrTableCell89.StylePriority.UseFont = false;
        this.xrTableCell89.StylePriority.UseTextAlignment = false;
        this.xrTableCell89.Text = "5";
        this.xrTableCell89.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell89.Weight = 0.20104577346974978D;
        // 
        // xrTableCell91
        // 
        this.xrTableCell91.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell91.Name = "xrTableCell91";
        this.xrTableCell91.StylePriority.UseFont = false;
        this.xrTableCell91.StylePriority.UseTextAlignment = false;
        this.xrTableCell91.Text = "6";
        this.xrTableCell91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell91.Weight = 0.1971756784539474D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseFont = false;
        this.xrTableCell18.StylePriority.UseTextAlignment = false;
        this.xrTableCell18.Text = "7";
        this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell18.Weight = 0.2140938175228973D;
        // 
        // xrTableCell32
        // 
        this.xrTableCell32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell32.Name = "xrTableCell32";
        this.xrTableCell32.StylePriority.UseFont = false;
        this.xrTableCell32.StylePriority.UseTextAlignment = false;
        this.xrTableCell32.Text = "8";
        this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell32.Weight = 0.22029275963057918D;
        // 
        // xrLabel53
        // 
        this.xrLabel53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5.000051F);
        this.xrLabel53.Name = "xrLabel53";
        this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel53.SizeF = new System.Drawing.SizeF(323.0001F, 18F);
        this.xrLabel53.StylePriority.UseFont = false;
        this.xrLabel53.StylePriority.UseTextAlignment = false;
        this.xrLabel53.Text = "VI. Перелік приватизованих об’єктів:";
        this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // DetailReport3
        // 
        this.DetailReport3.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail4,
            this.ReportHeader4});
        this.DetailReport3.DataMember = "OrgCardGiven4Rent";
        this.DetailReport3.DataSource = this.orgCardDB;
        this.DetailReport3.Level = 2;
        this.DetailReport3.Name = "DetailReport3";
        // 
        // Detail4
        // 
        this.Detail4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable8});
        this.Detail4.HeightF = 25F;
        this.Detail4.Name = "Detail4";
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
        this.xrTable8.SizeF = new System.Drawing.SizeF(955.4818F, 25F);
        this.xrTable8.StylePriority.UseBorders = false;
        // 
        // xrTableRow12
        // 
        this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell65,
            this.xrTableCell87,
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
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardGiven4Rent.FieldRenterNameZKPO")});
        this.xrTableCell65.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell65.Multiline = true;
        this.xrTableCell65.Name = "xrTableCell65";
        this.xrTableCell65.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell65.StylePriority.UseFont = false;
        this.xrTableCell65.StylePriority.UsePadding = false;
        this.xrTableCell65.StylePriority.UseTextAlignment = false;
        this.xrTableCell65.Text = "xrTableCell65";
        this.xrTableCell65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell65.Weight = 0.57193677905621576D;
        // 
        // xrTableCell87
        // 
        this.xrTableCell87.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardGiven4Rent.FieldObjAddress2")});
        this.xrTableCell87.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell87.Name = "xrTableCell87";
        this.xrTableCell87.StylePriority.UseFont = false;
        this.xrTableCell87.StylePriority.UseTextAlignment = false;
        this.xrTableCell87.Text = "xrTableCell87";
        this.xrTableCell87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell87.Weight = 0.45202077507375615D;
        // 
        // xrTableCell73
        // 
        this.xrTableCell73.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardGiven4Rent.rent_square")});
        this.xrTableCell73.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell73.Multiline = true;
        this.xrTableCell73.Name = "xrTableCell73";
        this.xrTableCell73.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell73.StylePriority.UseFont = false;
        this.xrTableCell73.StylePriority.UsePadding = false;
        this.xrTableCell73.StylePriority.UseTextAlignment = false;
        this.xrTableCell73.Text = "xrTableCell73";
        this.xrTableCell73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell73.Weight = 0.34400916430299339D;
        // 
        // xrTableCell74
        // 
        this.xrTableCell74.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardGiven4Rent.object_name")});
        this.xrTableCell74.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell74.Multiline = true;
        this.xrTableCell74.Name = "xrTableCell74";
        this.xrTableCell74.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell74.StylePriority.UseFont = false;
        this.xrTableCell74.StylePriority.UsePadding = false;
        this.xrTableCell74.StylePriority.UseTextAlignment = false;
        this.xrTableCell74.Text = "xrTableCell74";
        this.xrTableCell74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell74.Weight = 0.43989724238611255D;
        // 
        // xrTableCell75
        // 
        this.xrTableCell75.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardGiven4Rent.FieldRentAgreement2")});
        this.xrTableCell75.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell75.Multiline = true;
        this.xrTableCell75.Name = "xrTableCell75";
        this.xrTableCell75.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell75.StylePriority.UseFont = false;
        this.xrTableCell75.StylePriority.UsePadding = false;
        this.xrTableCell75.StylePriority.UseTextAlignment = false;
        this.xrTableCell75.Text = "xrTableCell75";
        this.xrTableCell75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell75.Weight = 0.42825795817585233D;
        // 
        // xrTableCell76
        // 
        this.xrTableCell76.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardGiven4Rent.FieldRentStart2")});
        this.xrTableCell76.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell76.Multiline = true;
        this.xrTableCell76.Name = "xrTableCell76";
        this.xrTableCell76.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell76.StylePriority.UseFont = false;
        this.xrTableCell76.StylePriority.UsePadding = false;
        this.xrTableCell76.StylePriority.UseTextAlignment = false;
        this.xrTableCell76.Text = "xrTableCell76";
        this.xrTableCell76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell76.Weight = 0.29269170112760268D;
        // 
        // xrTableCell77
        // 
        this.xrTableCell77.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardGiven4Rent.FieldRentFinish2")});
        this.xrTableCell77.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell77.Multiline = true;
        this.xrTableCell77.Name = "xrTableCell77";
        this.xrTableCell77.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell77.StylePriority.UseFont = false;
        this.xrTableCell77.StylePriority.UsePadding = false;
        this.xrTableCell77.StylePriority.UseTextAlignment = false;
        this.xrTableCell77.Text = "xrTableCell77";
        this.xrTableCell77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell77.Weight = 0.28938542075913709D;
        // 
        // xrTableCell78
        // 
        this.xrTableCell78.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OrgCardGiven4Rent.is_subarenda")});
        this.xrTableCell78.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell78.Multiline = true;
        this.xrTableCell78.Name = "xrTableCell78";
        this.xrTableCell78.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
        this.xrTableCell78.StylePriority.UseFont = false;
        this.xrTableCell78.StylePriority.UsePadding = false;
        this.xrTableCell78.StylePriority.UseTextAlignment = false;
        this.xrTableCell78.Text = "xrTableCell78";
        this.xrTableCell78.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell78.Weight = 0.21473325920127992D;
        // 
        // ReportHeader4
        // 
        this.ReportHeader4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel47,
            this.xrTable7});
        this.ReportHeader4.HeightF = 76F;
        this.ReportHeader4.Name = "ReportHeader4";
        // 
        // xrLabel47
        // 
        this.xrLabel47.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.999987F);
        this.xrLabel47.Name = "xrLabel47";
        this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel47.SizeF = new System.Drawing.SizeF(411.6245F, 18F);
        this.xrLabel47.StylePriority.UseFont = false;
        this.xrLabel47.StylePriority.UseTextAlignment = false;
        this.xrLabel47.Text = "V. Перелік об\'єктів, що надаються в оренду:";
        this.xrLabel47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrTable7
        // 
        this.xrTable7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
        this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 26F);
        this.xrTable7.Name = "xrTable7";
        this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow10,
            this.xrTableRow11});
        this.xrTable7.SizeF = new System.Drawing.SizeF(955.8986F, 50F);
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
            this.xrTableCell64,
            this.xrTableCell85});
        this.xrTableRow10.Name = "xrTableRow10";
        this.xrTableRow10.Weight = 1D;
        // 
        // xrTableCell34
        // 
        this.xrTableCell34.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell34.Name = "xrTableCell34";
        this.xrTableCell34.StylePriority.UseFont = false;
        this.xrTableCell34.StylePriority.UseTextAlignment = false;
        this.xrTableCell34.Text = "Назва орендаря, код ЄДРПОУ";
        this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell34.Weight = 0.55912455781514758D;
        // 
        // xrTableCell59
        // 
        this.xrTableCell59.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell59.Multiline = true;
        this.xrTableCell59.Name = "xrTableCell59";
        this.xrTableCell59.StylePriority.UseFont = false;
        this.xrTableCell59.StylePriority.UseTextAlignment = false;
        this.xrTableCell59.Text = "Адреса об\'єкту";
        this.xrTableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell59.Weight = 0.44087514623497259D;
        // 
        // xrTableCell60
        // 
        this.xrTableCell60.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell60.Multiline = true;
        this.xrTableCell60.Name = "xrTableCell60";
        this.xrTableCell60.StylePriority.UseFont = false;
        this.xrTableCell60.StylePriority.UseTextAlignment = false;
        this.xrTableCell60.Text = "Орендована площа, кв.м.";
        this.xrTableCell60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell60.Weight = 0.33552653690483203D;
        // 
        // xrTableCell61
        // 
        this.xrTableCell61.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell61.Multiline = true;
        this.xrTableCell61.Name = "xrTableCell61";
        this.xrTableCell61.StylePriority.UseFont = false;
        this.xrTableCell61.StylePriority.UseTextAlignment = false;
        this.xrTableCell61.Text = "Використання приміщення";
        this.xrTableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell61.Weight = 0.42905023506882539D;
        // 
        // xrTableCell62
        // 
        this.xrTableCell62.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell62.Multiline = true;
        this.xrTableCell62.Name = "xrTableCell62";
        this.xrTableCell62.StylePriority.UseFont = false;
        this.xrTableCell62.StylePriority.UseTextAlignment = false;
        this.xrTableCell62.Text = "Договір (номер, дата)";
        this.xrTableCell62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell62.Weight = 0.41769884236520688D;
        // 
        // xrTableCell63
        // 
        this.xrTableCell63.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell63.Multiline = true;
        this.xrTableCell63.Name = "xrTableCell63";
        this.xrTableCell63.StylePriority.UseFont = false;
        this.xrTableCell63.StylePriority.UseTextAlignment = false;
        this.xrTableCell63.Text = "Початок оренди";
        this.xrTableCell63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell63.Weight = 0.28547461682110553D;
        // 
        // xrTableCell64
        // 
        this.xrTableCell64.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell64.Multiline = true;
        this.xrTableCell64.Name = "xrTableCell64";
        this.xrTableCell64.StylePriority.UseFont = false;
        this.xrTableCell64.StylePriority.UseTextAlignment = false;
        this.xrTableCell64.Text = "Закінчення оренди";
        this.xrTableCell64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell64.Weight = 0.28224993184280672D;
        // 
        // xrTableCell85
        // 
        this.xrTableCell85.Name = "xrTableCell85";
        this.xrTableCell85.StylePriority.UseTextAlignment = false;
        this.xrTableCell85.Text = "Суб-оренда";
        this.xrTableCell85.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell85.Weight = 0.20943783864535573D;
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
            this.xrTableCell72,
            this.xrTableCell86});
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
        this.xrTableCell66.Weight = 0.55912446333349941D;
        // 
        // xrTableCell67
        // 
        this.xrTableCell67.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell67.Name = "xrTableCell67";
        this.xrTableCell67.StylePriority.UseFont = false;
        this.xrTableCell67.StylePriority.UseTextAlignment = false;
        this.xrTableCell67.Text = "2";
        this.xrTableCell67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell67.Weight = 0.44087561864322244D;
        // 
        // xrTableCell68
        // 
        this.xrTableCell68.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell68.Name = "xrTableCell68";
        this.xrTableCell68.StylePriority.UseFont = false;
        this.xrTableCell68.StylePriority.UseTextAlignment = false;
        this.xrTableCell68.Text = "3";
        this.xrTableCell68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell68.Weight = 0.33552634794156244D;
        // 
        // xrTableCell69
        // 
        this.xrTableCell69.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell69.Name = "xrTableCell69";
        this.xrTableCell69.StylePriority.UseFont = false;
        this.xrTableCell69.StylePriority.UseTextAlignment = false;
        this.xrTableCell69.Text = "4";
        this.xrTableCell69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell69.Weight = 0.4290500461055291D;
        // 
        // xrTableCell70
        // 
        this.xrTableCell70.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell70.Name = "xrTableCell70";
        this.xrTableCell70.StylePriority.UseFont = false;
        this.xrTableCell70.StylePriority.UseTextAlignment = false;
        this.xrTableCell70.Text = "5";
        this.xrTableCell70.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell70.Weight = 0.41769865340187479D;
        // 
        // xrTableCell71
        // 
        this.xrTableCell71.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell71.Name = "xrTableCell71";
        this.xrTableCell71.StylePriority.UseFont = false;
        this.xrTableCell71.StylePriority.UseTextAlignment = false;
        this.xrTableCell71.Text = "6";
        this.xrTableCell71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell71.Weight = 0.28547461682110553D;
        // 
        // xrTableCell72
        // 
        this.xrTableCell72.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.xrTableCell72.Name = "xrTableCell72";
        this.xrTableCell72.StylePriority.UseFont = false;
        this.xrTableCell72.StylePriority.UseTextAlignment = false;
        this.xrTableCell72.Text = "7";
        this.xrTableCell72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell72.Weight = 0.28224993184280667D;
        // 
        // xrTableCell86
        // 
        this.xrTableCell86.Name = "xrTableCell86";
        this.xrTableCell86.StylePriority.UseTextAlignment = false;
        this.xrTableCell86.Text = "8";
        this.xrTableCell86.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell86.Weight = 0.20943802760865207D;
        // 
        // FieldGiverNameZKPO
        // 
        this.FieldGiverNameZKPO.DataMember = "OrgCardRent";
        this.FieldGiverNameZKPO.Expression = "[org_giver_full_name] + \', \' + [org_giver_zkpo]";
        this.FieldGiverNameZKPO.Name = "FieldGiverNameZKPO";
        // 
        // FieldObjAddress
        // 
        this.FieldObjAddress.DataMember = "OrgCardRent";
        this.FieldObjAddress.Name = "FieldObjAddress";
        this.FieldObjAddress.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.FieldObjAddress_GetValue);
        // 
        // FieldRentAgreement
        // 
        this.FieldRentAgreement.DataMember = "OrgCardRent";
        this.FieldRentAgreement.Name = "FieldRentAgreement";
        this.FieldRentAgreement.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.FieldRentAgreement_GetValue);
        // 
        // FieldRenterNameZKPO
        // 
        this.FieldRenterNameZKPO.DataMember = "OrgCardGiven4Rent";
        this.FieldRenterNameZKPO.Expression = "[org_renter_full_name] + \', \' + [org_renter_zkpo]";
        this.FieldRenterNameZKPO.Name = "FieldRenterNameZKPO";
        // 
        // FieldObjAddress2
        // 
        this.FieldObjAddress2.DataMember = "OrgCardGiven4Rent";
        this.FieldObjAddress2.Name = "FieldObjAddress2";
        this.FieldObjAddress2.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.FieldObjAddress2_GetValue);
        // 
        // FieldRentAgreement2
        // 
        this.FieldRentAgreement2.DataMember = "OrgCardGiven4Rent";
        this.FieldRentAgreement2.Name = "FieldRentAgreement2";
        this.FieldRentAgreement2.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.FieldRentAgreement2_GetValue);
        // 
        // FieldRentStart
        // 
        this.FieldRentStart.DataMember = "OrgCardRent";
        this.FieldRentStart.DisplayName = "FieldRentStart";
        this.FieldRentStart.Name = "FieldRentStart";
        this.FieldRentStart.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.FieldRentStart_GetValue);
        // 
        // FieldRentFinish
        // 
        this.FieldRentFinish.DataMember = "OrgCardRent";
        this.FieldRentFinish.Name = "FieldRentFinish";
        this.FieldRentFinish.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.FieldRentFinish_GetValue);
        // 
        // FieldRentStart2
        // 
        this.FieldRentStart2.DataMember = "OrgCardGiven4Rent";
        this.FieldRentStart2.Name = "FieldRentStart2";
        this.FieldRentStart2.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.FieldRentStart2_GetValue);
        // 
        // FieldRentFinish2
        // 
        this.FieldRentFinish2.DataMember = "OrgCardGiven4Rent";
        this.FieldRentFinish2.Name = "FieldRentFinish2";
        this.FieldRentFinish2.GetValue += new DevExpress.XtraReports.UI.GetValueEventHandler(this.FieldRentFinish2_GetValue);
        // 
        // ReportOrgCard
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
            this.FieldGiverNameZKPO,
            this.FieldObjAddress,
            this.FieldRentAgreement,
            this.FieldRenterNameZKPO,
            this.FieldObjAddress2,
            this.FieldRentAgreement2,
            this.FieldRentStart,
            this.FieldRentFinish,
            this.FieldRentStart2,
            this.FieldRentFinish2});
        this.DataMember = "OrgCardProperties";
        this.DataSource = this.orgCardDB;
        this.Landscape = true;
        this.Margins = new System.Drawing.Printing.Margins(35, 25, 9, 6);
        this.PageHeight = 827;
        this.PageWidth = 1169;
        this.PaperKind = System.Drawing.Printing.PaperKind.A4;
        this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Date});
        this.Version = "12.1";
        ((System.ComponentModel.ISupportInitialize)(this.orgCardDB)).EndInit();
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

    private void FieldObjAddress_GetValue(object sender, GetValueEventArgs e)
    {
        string district = "";
        string street = "";
        string number = "";

        if (!((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).IsdistrictNull())
        {
            district = ((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).district;
        }

        if (!((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).Isstreet_full_nameNull())
        {
            street = ((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).street_full_name;
        }

        if (!((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).Isaddr_nomerNull())
        {
            number = ((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).addr_nomer;
        }

        e.Value = street + " " + number;

        if (district.Length > 0)
        {
            e.Value += ", " + district + " " + Resources.Strings.RentAddressDistrict;
        }
    }

    private void FieldObjAddress2_GetValue(object sender, GetValueEventArgs e)
    {
        string district = "";
        string street = "";
        string number = "";

        if (!((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).IsdistrictNull())
        {
            district = ((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).district;
        }

        if (!((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).Isstreet_full_nameNull())
        {
            street = ((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).street_full_name;
        }

        if (!((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).Isaddr_nomerNull())
        {
            number = ((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).addr_nomer;
        }

        e.Value = street + " " + number;

        if (district.Length > 0)
        {
            e.Value += ", " + district + " " + Resources.Strings.RentAddressDistrict;
        }
    }

    private void FieldRentAgreement_GetValue(object sender, GetValueEventArgs e)
    {
        string agreementNum = "";

        if (!((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).Isagreement_numNull())
        {
            agreementNum = ((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).agreement_num;
        }

        if (!((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).Isagreement_dateNull())
        {
            DateTime agreementDate = ((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).agreement_date;

            e.Value = Resources.Strings.RentAgreementNum + agreementNum + Resources.Strings.RentAgreementDate + agreementDate.ToShortDateString();
        }
        else
        {
            e.Value = agreementNum.Length > 0 ? Resources.Strings.RentAgreementNum + agreementNum : "";
        }
    }

    private void FieldRentAgreement2_GetValue(object sender, GetValueEventArgs e)
    {
        string agreementNum = "";

        if (!((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).Isagreement_numNull())
        {
            agreementNum = ((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).agreement_num;
        }

        if (!((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).Isagreement_dateNull())
        {
            DateTime agreementDate = ((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).agreement_date;

            e.Value = Resources.Strings.RentAgreementNum + agreementNum + Resources.Strings.RentAgreementDate + agreementDate.ToShortDateString();
        }
        else
        {
            e.Value = agreementNum.Length > 0 ? Resources.Strings.RentAgreementNum + agreementNum : "";
        }
    }

    private void FieldRentStart_GetValue(object sender, GetValueEventArgs e)
    {
        if (!((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).Isrent_start_dateNull())
        {
            DateTime rentStartDate = ((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).rent_start_date;

            e.Value = rentStartDate.ToShortDateString();
        }
        else
        {
            e.Value = "";
        }
    }

    private void FieldRentFinish_GetValue(object sender, GetValueEventArgs e)
    {
        if (!((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).Isrent_finish_dateNull())
        {
            DateTime rentFinishDate = ((OrgCardDB.OrgCardRentRow)((DataRowView)e.Row).Row).rent_finish_date;

            e.Value = rentFinishDate.ToShortDateString();
        }
        else
        {
            e.Value = "";
        }
    }

    private void FieldRentStart2_GetValue(object sender, GetValueEventArgs e)
    {
        if (!((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).Isrent_start_dateNull())
        {
            DateTime rentStartDate = ((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).rent_start_date;

            e.Value = rentStartDate.ToShortDateString();
        }
        else
        {
            e.Value = "";
        }
    }

    private void FieldRentFinish2_GetValue(object sender, GetValueEventArgs e)
    {
        if (!((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).Isrent_finish_dateNull())
        {
            DateTime rentFinishDate = ((OrgCardDB.OrgCardGiven4RentRow)((DataRowView)e.Row).Row).rent_finish_date;

            e.Value = rentFinishDate.ToShortDateString();
        }
        else
        {
            e.Value = "";
        }
    }
}
