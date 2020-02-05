namespace TestLocalization {
    partial class FormGauge {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.clientPanel = new DevExpress.XtraEditors.PanelControl();
            this.gaugeControl1 = new DevExpress.XtraGauges.Win.GaugeControl();
            this.circularGauge1 = new DevExpress.XtraGauges.Win.Gauges.Circular.CircularGauge();
            this.arcScaleBackgroundLayerComponent1 = new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleBackgroundLayerComponent();
            this.arcScaleComponent1 = new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleComponent();
            this.arcScaleEffectLayerComponent1 = new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleEffectLayerComponent();
            this.arcScaleNeedleComponent1 = new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleNeedleComponent();
            this.linearGauge1 = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearGauge();
            this.linearScaleBackgroundLayerComponent1 = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleBackgroundLayerComponent();
            this.linearScaleComponent1 = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleComponent();
            this.linearScaleLevelComponent1 = new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleLevelComponent();
            this.digitalGauge1 = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge();
            this.digitalBackgroundLayerComponent1 = new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientPanel)).BeginInit();
            this.clientPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.circularGauge1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcScaleBackgroundLayerComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcScaleComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcScaleEffectLayerComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcScaleNeedleComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linearGauge1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linearScaleBackgroundLayerComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linearScaleComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linearScaleLevelComponent1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalGauge1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 451);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(602, 31);
            // 
            // ribbon
            // 
            this.ribbon.ApplicationButtonText = null;
            // 
            // 
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.ExpandCollapseItem.Name = "";
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.barButtonItem1});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 2;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.SelectedPage = this.ribbonPage1;
            this.ribbon.Size = new System.Drawing.Size(602, 147);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Style Manager...";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // clientPanel
            // 
            this.clientPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.clientPanel.Controls.Add(this.gaugeControl1);
            this.clientPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientPanel.Location = new System.Drawing.Point(0, 147);
            this.clientPanel.Name = "clientPanel";
            this.clientPanel.Size = new System.Drawing.Size(602, 304);
            this.clientPanel.TabIndex = 2;
            // 
            // gaugeControl1
            // 
            this.gaugeControl1.AutoLayout = false;
            this.gaugeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gaugeControl1.Gauges.AddRange(new DevExpress.XtraGauges.Base.IGauge[] {
            this.circularGauge1,
            this.linearGauge1,
            this.digitalGauge1});
            this.gaugeControl1.Location = new System.Drawing.Point(0, 0);
            this.gaugeControl1.Name = "gaugeControl1";
            this.gaugeControl1.Size = new System.Drawing.Size(602, 304);
            this.gaugeControl1.TabIndex = 0;
            // 
            // circularGauge1
            // 
            this.circularGauge1.BackgroundLayers.AddRange(new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleBackgroundLayerComponent[] {
            this.arcScaleBackgroundLayerComponent1});
            this.circularGauge1.Bounds = new System.Drawing.Rectangle(6, 6, 323, 190);
            this.circularGauge1.EffectLayers.AddRange(new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleEffectLayerComponent[] {
            this.arcScaleEffectLayerComponent1});
            this.circularGauge1.Name = "circularGauge1";
            this.circularGauge1.Needles.AddRange(new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleNeedleComponent[] {
            this.arcScaleNeedleComponent1});
            this.circularGauge1.Scales.AddRange(new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleComponent[] {
            this.arcScaleComponent1});
            // 
            // arcScaleBackgroundLayerComponent1
            // 
            this.arcScaleBackgroundLayerComponent1.ArcScale = this.arcScaleComponent1;
            this.arcScaleBackgroundLayerComponent1.Name = "bg1";
            this.arcScaleBackgroundLayerComponent1.ShapeType = DevExpress.XtraGauges.Core.Model.BackgroundLayerShapeType.CircularFull_Style7;
            this.arcScaleBackgroundLayerComponent1.ZOrder = 1000;
            // 
            // arcScaleComponent1
            // 
            this.arcScaleComponent1.Center = new DevExpress.XtraGauges.Core.Base.PointF2D(125F, 125F);
            this.arcScaleComponent1.EndAngle = 60F;
            this.arcScaleComponent1.MajorTickmark.FormatString = "{0:F0}";
            this.arcScaleComponent1.MajorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Circular_Style7_2;
            this.arcScaleComponent1.MajorTickmark.TextOffset = 22F;
            this.arcScaleComponent1.MajorTickmark.TextOrientation = DevExpress.XtraGauges.Core.Model.LabelOrientation.LeftToRight;
            this.arcScaleComponent1.MaxValue = 100F;
            this.arcScaleComponent1.MinorTickCount = 4;
            this.arcScaleComponent1.MinorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Circular_Style7_1;
            this.arcScaleComponent1.Name = "scale1";
            this.arcScaleComponent1.RadiusX = 83F;
            this.arcScaleComponent1.RadiusY = 83F;
            this.arcScaleComponent1.StartAngle = -240F;
            // 
            // arcScaleEffectLayerComponent1
            // 
            this.arcScaleEffectLayerComponent1.ArcScale = this.arcScaleComponent1;
            this.arcScaleEffectLayerComponent1.Name = "effect1";
            this.arcScaleEffectLayerComponent1.ScaleCenterPos = new DevExpress.XtraGauges.Core.Base.PointF2D(0.5F, 1.06F);
            this.arcScaleEffectLayerComponent1.Shader = new DevExpress.XtraGauges.Core.Drawing.OpacityShader("Opacity[0.75]");
            this.arcScaleEffectLayerComponent1.ShapeType = DevExpress.XtraGauges.Core.Model.EffectLayerShapeType.CircularFull_Style7;
            this.arcScaleEffectLayerComponent1.Size = new System.Drawing.SizeF(235F, 110F);
            this.arcScaleEffectLayerComponent1.ZOrder = -1000;
            // 
            // arcScaleNeedleComponent1
            // 
            this.arcScaleNeedleComponent1.ArcScale = this.arcScaleComponent1;
            this.arcScaleNeedleComponent1.EndOffset = -25F;
            this.arcScaleNeedleComponent1.Name = "needle1";
            this.arcScaleNeedleComponent1.ShapeType = DevExpress.XtraGauges.Core.Model.NeedleShapeType.CircularFull_Style7;
            this.arcScaleNeedleComponent1.StartOffset = -21F;
            this.arcScaleNeedleComponent1.ZOrder = -50;
            // 
            // linearGauge1
            // 
            this.linearGauge1.BackgroundLayers.AddRange(new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleBackgroundLayerComponent[] {
            this.linearScaleBackgroundLayerComponent1});
            this.linearGauge1.Bounds = new System.Drawing.Rectangle(337, 6, 259, 287);
            this.linearGauge1.Levels.AddRange(new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleLevelComponent[] {
            this.linearScaleLevelComponent1});
            this.linearGauge1.Name = "linearGauge1";
            this.linearGauge1.Scales.AddRange(new DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleComponent[] {
            this.linearScaleComponent1});
            // 
            // linearScaleBackgroundLayerComponent1
            // 
            this.linearScaleBackgroundLayerComponent1.LinearScale = this.linearScaleComponent1;
            this.linearScaleBackgroundLayerComponent1.Name = "Gauge0_BackgroundLayer1";
            this.linearScaleBackgroundLayerComponent1.ZOrder = 1000;
            // 
            // linearScaleComponent1
            // 
            this.linearScaleComponent1.AppearanceTickmarkText.TextBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:Black");
            this.linearScaleComponent1.EndPoint = new DevExpress.XtraGauges.Core.Base.PointF2D(62.5F, 35F);
            this.linearScaleComponent1.MajorTickCount = 6;
            this.linearScaleComponent1.MajorTickmark.FormatString = "{0:F0}";
            this.linearScaleComponent1.MajorTickmark.ShapeOffset = -20F;
            this.linearScaleComponent1.MajorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Linear_Style1_1;
            this.linearScaleComponent1.MajorTickmark.TextOffset = -32F;
            this.linearScaleComponent1.MaxValue = 100F;
            this.linearScaleComponent1.MinorTickCount = 4;
            this.linearScaleComponent1.MinorTickmark.ShapeOffset = -14F;
            this.linearScaleComponent1.MinorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Linear_Style1_2;
            this.linearScaleComponent1.Name = "Gauge0_Scale1";
            this.linearScaleComponent1.StartPoint = new DevExpress.XtraGauges.Core.Base.PointF2D(62.5F, 215F);
            // 
            // linearScaleLevelComponent1
            // 
            this.linearScaleLevelComponent1.LinearScale = this.linearScaleComponent1;
            this.linearScaleLevelComponent1.Name = "Gauge0_Level1";
            this.linearScaleLevelComponent1.ZOrder = -50;
            // 
            // digitalGauge1
            // 
            this.digitalGauge1.AppearanceOff.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#00FFFFFF");
            this.digitalGauge1.AppearanceOn.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:Black");
            this.digitalGauge1.BackgroundLayers.AddRange(new DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent[] {
            this.digitalBackgroundLayerComponent1});
            this.digitalGauge1.Bounds = new System.Drawing.Rectangle(6, 196, 316, 100);
            this.digitalGauge1.Name = "digitalGauge1";
            this.digitalGauge1.Text = "00.000";
            // 
            // digitalBackgroundLayerComponent1
            // 
            this.digitalBackgroundLayerComponent1.BottomRight = new DevExpress.XtraGauges.Core.Base.PointF2D(259.8125F, 99.9625F);
            this.digitalBackgroundLayerComponent1.Name = "Gauge1_BackgroundLayer1";
            this.digitalBackgroundLayerComponent1.TopLeft = new DevExpress.XtraGauges.Core.Base.PointF2D(20F, 0F);
            this.digitalBackgroundLayerComponent1.ZOrder = 1000;
            // 
            // FormGauge
            // 
            this.ClientSize = new System.Drawing.Size(602, 482);
            this.Controls.Add(this.clientPanel);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Name = "FormGauge";
            this.Ribbon = this.ribbon;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "FormGauge";
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientPanel)).EndInit();
            this.clientPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.circularGauge1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcScaleBackgroundLayerComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcScaleComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcScaleEffectLayerComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arcScaleNeedleComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linearGauge1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linearScaleBackgroundLayerComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linearScaleComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linearScaleLevelComponent1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalGauge1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalBackgroundLayerComponent1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraEditors.PanelControl clientPanel;
        private DevExpress.XtraGauges.Win.GaugeControl gaugeControl1;
        private DevExpress.XtraGauges.Win.Gauges.Circular.CircularGauge circularGauge1;
        private DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleBackgroundLayerComponent arcScaleBackgroundLayerComponent1;
        private DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleComponent arcScaleComponent1;
        private DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleEffectLayerComponent arcScaleEffectLayerComponent1;
        private DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleNeedleComponent arcScaleNeedleComponent1;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearGauge linearGauge1;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleBackgroundLayerComponent linearScaleBackgroundLayerComponent1;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleComponent linearScaleComponent1;
        private DevExpress.XtraGauges.Win.Gauges.Linear.LinearScaleLevelComponent linearScaleLevelComponent1;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalGauge digitalGauge1;
        private DevExpress.XtraGauges.Win.Gauges.Digital.DigitalBackgroundLayerComponent digitalBackgroundLayerComponent1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
    }
}