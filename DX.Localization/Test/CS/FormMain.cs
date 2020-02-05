using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;

namespace TestLocalization {
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class FormMain : System.Windows.Forms.Form {
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem nbiCharts;
        private DevExpress.XtraNavBar.NavBarItem nbiGrid;
        private DevExpress.XtraNavBar.NavBarItem nbiLayoutAndEditors;
        private DevExpress.XtraNavBar.NavBarItem nbiScheduler;
        private DevExpress.XtraNavBar.NavBarItem nbiReportDesigner;
        private DevExpress.XtraNavBar.NavBarItem nbiWizard;
        private DevExpress.XtraNavBar.NavBarItem nbiPrintPreviewRibbon;
        private DevExpress.XtraNavBar.NavBarItem nbiPrintPreviewBars;
        private DevExpress.XtraNavBar.NavBarItem nbiPivot;
        private DevExpress.XtraNavBar.NavBarItem nbiTreeList;
        private GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraNavBar.NavBarItem nbiRichEdit;
        private DevExpress.XtraNavBar.NavBarItem nbiGauges;
        private IContainer components;

        public FormMain() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if(disposing) {
                if(components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.nbiCharts = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiGrid = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiLayoutAndEditors = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiPivot = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiPrintPreviewBars = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiPrintPreviewRibbon = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiReportDesigner = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiRichEdit = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiScheduler = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiTreeList = new DevExpress.XtraNavBar.NavBarItem();
            this.nbiWizard = new DevExpress.XtraNavBar.NavBarItem();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.nbiGauges = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroup1;
            this.navBarControl1.ContentButtonHint = null;
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1});
            this.navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.nbiGrid,
            this.nbiCharts,
            this.nbiScheduler,
            this.nbiLayoutAndEditors,
            this.nbiReportDesigner,
            this.nbiWizard,
            this.nbiPrintPreviewRibbon,
            this.nbiPrintPreviewBars,
            this.nbiPivot,
            this.nbiTreeList,
            this.nbiRichEdit,
            this.nbiGauges});
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 142;
            this.navBarControl1.Size = new System.Drawing.Size(230, 393);
            this.navBarControl1.TabIndex = 2;
            this.navBarControl1.Text = "navBarControl1";
            this.navBarControl1.View = new DevExpress.XtraNavBar.ViewInfo.NavigationPaneViewInfoRegistrator();
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "navBarGroup1";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiCharts),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiGrid),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiLayoutAndEditors),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiPivot),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiPrintPreviewBars),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiPrintPreviewRibbon),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiReportDesigner),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiRichEdit),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiScheduler),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiTreeList),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiWizard),
            new DevExpress.XtraNavBar.NavBarItemLink(this.nbiGauges)});
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // nbiCharts
            // 
            this.nbiCharts.Caption = "Charts";
            this.nbiCharts.Name = "nbiCharts";
            this.nbiCharts.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiCharts_LinkClicked);
            // 
            // nbiGrid
            // 
            this.nbiGrid.Caption = "Grid";
            this.nbiGrid.Name = "nbiGrid";
            this.nbiGrid.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiGrid_LinkClicked);
            // 
            // nbiLayoutAndEditors
            // 
            this.nbiLayoutAndEditors.Caption = "Layout & Editors & SpellChecker";
            this.nbiLayoutAndEditors.Name = "nbiLayoutAndEditors";
            this.nbiLayoutAndEditors.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiLayoutAndEditors_LinkClicked);
            // 
            // nbiPivot
            // 
            this.nbiPivot.Caption = "PivotGrid";
            this.nbiPivot.Name = "nbiPivot";
            this.nbiPivot.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiPivot_LinkClicked);
            // 
            // nbiPrintPreviewBars
            // 
            this.nbiPrintPreviewBars.Caption = "Print Preview with XtraBars";
            this.nbiPrintPreviewBars.Name = "nbiPrintPreviewBars";
            this.nbiPrintPreviewBars.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiPrintPreviewBars_LinkClicked);
            // 
            // nbiPrintPreviewRibbon
            // 
            this.nbiPrintPreviewRibbon.Caption = "Print Preview with Ribbon";
            this.nbiPrintPreviewRibbon.Name = "nbiPrintPreviewRibbon";
            this.nbiPrintPreviewRibbon.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiPrintPreviewRibbon_LinkClicked);
            // 
            // nbiReportDesigner
            // 
            this.nbiReportDesigner.Caption = "Report Designer";
            this.nbiReportDesigner.Name = "nbiReportDesigner";
            this.nbiReportDesigner.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiReportDesigner_LinkClicked);
            // 
            // nbiRichEdit
            // 
            this.nbiRichEdit.Caption = "RichEdit";
            this.nbiRichEdit.Name = "nbiRichEdit";
            this.nbiRichEdit.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiRichEdit_LinkClicked);
            // 
            // nbiScheduler
            // 
            this.nbiScheduler.Caption = "Scheduler";
            this.nbiScheduler.Name = "nbiScheduler";
            this.nbiScheduler.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiScheduler_LinkClicked);
            // 
            // nbiTreeList
            // 
            this.nbiTreeList.Caption = "TreeList";
            this.nbiTreeList.Name = "nbiTreeList";
            this.nbiTreeList.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiTreeList_LinkClicked);
            // 
            // nbiWizard
            // 
            this.nbiWizard.Caption = "Wizard";
            this.nbiWizard.Name = "nbiWizard";
            this.nbiWizard.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiWizard_LinkClicked);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(230, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(492, 393);
            this.gridControl1.TabIndex = 3;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // nbiGauges
            // 
            this.nbiGauges.Caption = "Gauges";
            this.nbiGauges.Name = "nbiGauges";
            this.nbiGauges.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.nbiGauges_LinkClicked);
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(722, 393);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.navBarControl1);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void nbiCharts_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            new FormChart().Show();
        }

        private void nbiLayoutAndEditors_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            new FormLayoutAndEditors().Show();
        }

        private void nbiGrid_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            new FormGrid().Show();
        }

        private void nbiScheduler_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            new FormScheduler().Show();
        }

        private void nbiReportDesigner_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            new XtraReport1().ShowDesigner();
        }

        private void nbiWizard_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            using(FormWizard wizard = new FormWizard()) {
                wizard.ShowDialog();
            }
        }

        private void nbiPrintPreviewRibbon_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            using(DevExpress.XtraPrinting.Preview.PrintPreviewRibbonFormEx form = new DevExpress.XtraPrinting.Preview.PrintPreviewRibbonFormEx()) {
                PrintingSystem ps = new PrintingSystem();
                form.PrintControl.PrintingSystem = ps;
                PrintableComponentLink link = new PrintableComponentLink(ps);
                link.Component = gridControl1;
                link.CreateDocument();
                form.ShowDialog();
            }
        }

        private void nbiPrintPreviewBars_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            using(DevExpress.XtraPrinting.Preview.PrintPreviewFormEx form = new DevExpress.XtraPrinting.Preview.PrintPreviewFormEx()) {
                XtraReport1 report = new XtraReport1();
                form.PrintingSystem = report.PrintingSystem;
                report.CreateDocument();
                form.ShowDialog();
            }
        }

        private void nbiPivot_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            new FormPivot().Show();
        }

        private void nbiTreeList_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            new FormTreeList().Show();
        }

        private void FormMain_Load(object sender, EventArgs e) {
            new DevExpress.XtraGrid.Design.XViewsPrinting(gridControl1);
            gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "");
            gridView1.OptionsView.ShowFooter = true;
            gridView1.ActiveFilterCriteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("[Discontinued] = False");
        }

        private void nbiRichEdit_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            new FormRichEdit().Show();
        }

        private void nbiGauges_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) {
            new FormGauge().Show();
        }
    }
}