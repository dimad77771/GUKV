using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraPivotGrid;
using System.Data;
using DevExpress.Data.Filtering;

namespace TestLocalization
{
	/// <summary>
	/// Summary description for FormPivot.
	/// </summary>
	public class FormPivot : System.Windows.Forms.Form
	{
		private DevExpress.XtraPivotGrid.PivotGridControl pivotGridControl1;
		private DevExpress.XtraPivotGrid.PivotGridField pivotGridField1;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private IContainer components;

		public FormPivot()
		{
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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pivotGridControl1 = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.pivotGridField1 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField2 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // pivotGridControl1
            // 
            this.pivotGridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.pivotGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pivotGridControl1.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.pivotGridField1,
            this.pivotGridField2});
            this.pivotGridControl1.Location = new System.Drawing.Point(0, 24);
            this.pivotGridControl1.Name = "pivotGridControl1";
            this.pivotGridControl1.Size = new System.Drawing.Size(550, 337);
            this.pivotGridControl1.TabIndex = 0;
            // 
            // pivotGridField1
            // 
            this.pivotGridField1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField1.AreaIndex = 0;
            this.pivotGridField1.Name = "pivotGridField1";
            // 
            // pivotGridField2
            // 
            this.pivotGridField2.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField2.AreaIndex = 1;
            this.pivotGridField2.Name = "pivotGridField2";
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1});
            this.barManager1.MaxItemId = 1;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.bar1.Text = "Tools";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Print Preview...";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // FormPivot
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(550, 361);
            this.Controls.Add(this.pivotGridControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FormPivot";
            this.Text = "FormPivot";
            this.Load += new System.EventHandler(this.FormPivot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void FormPivot_Load(object sender, System.EventArgs e) {
            InitPivotGridControl();
		}

        DataTable table;
        PivotGridControl Pivot { get { return pivotGridControl1; } }
		DataTable Table { get { return table; } }
		
		void InitPivotGridControl() {
			this.table = new DataTable();
			CreateColumns();
			AddRecords();
            CreatePivotFields();
            Pivot.DataSource = new BindingSource(Table, "");
            Pivot.Prefilter.Criteria = new OperandProperty("Year") == 1995;
		}
		void CreatePivotFields() {
            Pivot.Fields.Clear();
			Pivot.Fields.Add("Category Name", PivotArea.RowArea);
			Pivot.Fields.Add("Product Name", PivotArea.RowArea);
			Pivot.Fields.Add("Year", PivotArea.ColumnArea);
			PivotGridField fieldSale = Pivot.Fields.Add("Sale", PivotArea.DataArea);
            fieldSale.Options.AllowRunTimeSummaryChange = true;
            fieldSale.Options.ShowSummaryTypeName = true;

			Pivot.Fields.Add("Employee Name", PivotArea.FilterArea);
			Pivot.Fields["Year"].Width = 70;
			Pivot.Fields["Sale"].CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			Pivot.Fields["Sale"].CellFormat.FormatString = "c";
		}
        
        void CreateColumns() {
			Table.Columns.Add("Category Name", typeof(string));
			Table.Columns.Add("Product Name", typeof(string));
			Table.Columns.Add("Year", typeof(int));
			Table.Columns.Add("Sale", typeof(float));
			Table.Columns.Add("Quantity", typeof(int));
			Table.Columns.Add("Employee Name", typeof(string));
		}
		void AddRecords() {
			Table.Rows.Add(new object[] {"Beverages", "Chai", 1995, 5070.60, 319, null});
			Table.Rows.Add(new object[] {"Beverages", "Chai", 1996, 6295.50, 399, null});
            Table.Rows.Add(new object[] {"Beverages", "Ipoh Coffee", 1995, 10034.90, 228, null});
            Table.Rows.Add(new object[] {"Beverages", "Ipoh Coffee", 1996, 8560.60, 216, null});
            Table.Rows.Add(new object[] {"Confections", "Chocolade", 1995, 1282.01, 130, null});
            Table.Rows.Add(new object[] {"Confections", "Chocolade", 1996, 86.70, 8, null});
            Table.Rows.Add(new object[] {"Confections", "Scottish Longbreads", 1995, 3909.00, 380, null});
            Table.Rows.Add(new object[] {"Confections", "Scottish Longbreads", 1996, 4175.00, 354, null});
		}

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            pivotGridControl1.ShowPrintPreview();
        }
	}
}