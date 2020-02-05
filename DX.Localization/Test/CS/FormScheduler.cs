using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraScheduler.UI;

namespace TestLocalization
{
	/// <summary>
	/// Summary description for FormScheduler.
	/// </summary>
	public class FormScheduler : System.Windows.Forms.Form
	{
		private DevExpress.XtraScheduler.SchedulerControl schedulerControl1;
        private DevExpress.XtraScheduler.SchedulerStorage schedulerStorage1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private NewAppointmentItem newAppointmentItem1;
        private NewRecurringAppointmentItem newRecurringAppointmentItem1;
        private NavigateViewBackwardItem navigateViewBackwardItem1;
        private NavigateViewForwardItem navigateViewForwardItem1;
        private GotoTodayItem gotoTodayItem1;
        private ViewZoomInItem viewZoomInItem1;
        private ViewZoomOutItem viewZoomOutItem1;
        private SwitchToDayViewItem switchToDayViewItem1;
        private SwitchToWorkWeekViewItem switchToWorkWeekViewItem1;
        private SwitchToWeekViewItem switchToWeekViewItem1;
        private SwitchToMonthViewItem switchToMonthViewItem1;
        private SwitchToTimelineViewItem switchToTimelineViewItem1;
        private SwitchToGanttViewItem switchToGanttViewItem1;
        private GroupByNoneItem groupByNoneItem1;
        private GroupByDateItem groupByDateItem1;
        private GroupByResourceItem groupByResourceItem1;
        private HomeRibbonPage homeRibbonPage1;
        private AppointmentRibbonPageGroup appointmentRibbonPageGroup1;
        private NavigatorRibbonPageGroup navigatorRibbonPageGroup1;
        private ArrangeRibbonPageGroup arrangeRibbonPageGroup1;
        private GroupByRibbonPageGroup groupByRibbonPageGroup1;
        private SchedulerBarController schedulerBarController1;
		private System.ComponentModel.IContainer components;

		public FormScheduler()
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
            DevExpress.XtraScheduler.TimeRuler timeRuler1 = new DevExpress.XtraScheduler.TimeRuler();
            DevExpress.XtraScheduler.TimeRuler timeRuler2 = new DevExpress.XtraScheduler.TimeRuler();
            this.schedulerControl1 = new DevExpress.XtraScheduler.SchedulerControl();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.schedulerStorage1 = new DevExpress.XtraScheduler.SchedulerStorage(this.components);
            this.schedulerBarController1 = new DevExpress.XtraScheduler.UI.SchedulerBarController();
            this.appointmentRibbonPageGroup1 = new DevExpress.XtraScheduler.UI.AppointmentRibbonPageGroup();
            this.homeRibbonPage1 = new DevExpress.XtraScheduler.UI.HomeRibbonPage();
            this.newAppointmentItem1 = new DevExpress.XtraScheduler.UI.NewAppointmentItem();
            this.newRecurringAppointmentItem1 = new DevExpress.XtraScheduler.UI.NewRecurringAppointmentItem();
            this.navigatorRibbonPageGroup1 = new DevExpress.XtraScheduler.UI.NavigatorRibbonPageGroup();
            this.navigateViewBackwardItem1 = new DevExpress.XtraScheduler.UI.NavigateViewBackwardItem();
            this.navigateViewForwardItem1 = new DevExpress.XtraScheduler.UI.NavigateViewForwardItem();
            this.gotoTodayItem1 = new DevExpress.XtraScheduler.UI.GotoTodayItem();
            this.viewZoomInItem1 = new DevExpress.XtraScheduler.UI.ViewZoomInItem();
            this.viewZoomOutItem1 = new DevExpress.XtraScheduler.UI.ViewZoomOutItem();
            this.arrangeRibbonPageGroup1 = new DevExpress.XtraScheduler.UI.ArrangeRibbonPageGroup();
            this.switchToDayViewItem1 = new DevExpress.XtraScheduler.UI.SwitchToDayViewItem();
            this.switchToWorkWeekViewItem1 = new DevExpress.XtraScheduler.UI.SwitchToWorkWeekViewItem();
            this.switchToWeekViewItem1 = new DevExpress.XtraScheduler.UI.SwitchToWeekViewItem();
            this.switchToMonthViewItem1 = new DevExpress.XtraScheduler.UI.SwitchToMonthViewItem();
            this.switchToTimelineViewItem1 = new DevExpress.XtraScheduler.UI.SwitchToTimelineViewItem();
            this.switchToGanttViewItem1 = new DevExpress.XtraScheduler.UI.SwitchToGanttViewItem();
            this.groupByRibbonPageGroup1 = new DevExpress.XtraScheduler.UI.GroupByRibbonPageGroup();
            this.groupByNoneItem1 = new DevExpress.XtraScheduler.UI.GroupByNoneItem();
            this.groupByDateItem1 = new DevExpress.XtraScheduler.UI.GroupByDateItem();
            this.groupByResourceItem1 = new DevExpress.XtraScheduler.UI.GroupByResourceItem();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerBarController1)).BeginInit();
            this.SuspendLayout();
            // 
            // schedulerControl1
            // 
            this.schedulerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schedulerControl1.Location = new System.Drawing.Point(0, 142);
            this.schedulerControl1.MenuManager = this.ribbonControl1;
            this.schedulerControl1.Name = "schedulerControl1";
            this.schedulerControl1.Size = new System.Drawing.Size(783, 370);
            this.schedulerControl1.Start = new System.DateTime(2006, 1, 31, 0, 0, 0, 0);
            this.schedulerControl1.Storage = this.schedulerStorage1;
            this.schedulerControl1.TabIndex = 0;
            this.schedulerControl1.Text = "schedulerControl1";
            this.schedulerControl1.Views.DayView.TimeRulers.Add(timeRuler1);
            this.schedulerControl1.Views.WorkWeekView.TimeRulers.Add(timeRuler2);
            this.schedulerControl1.EditAppointmentFormShowing += new DevExpress.XtraScheduler.AppointmentFormEventHandler(this.schedulerControl1_EditAppointmentFormShowing);
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ApplicationButtonText = null;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.ExpandCollapseItem.Name = "";
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barButtonItem1,
            this.barCheckItem1,
            this.newAppointmentItem1,
            this.newRecurringAppointmentItem1,
            this.navigateViewBackwardItem1,
            this.navigateViewForwardItem1,
            this.gotoTodayItem1,
            this.viewZoomInItem1,
            this.viewZoomOutItem1,
            this.switchToDayViewItem1,
            this.switchToWorkWeekViewItem1,
            this.switchToWeekViewItem1,
            this.switchToMonthViewItem1,
            this.switchToTimelineViewItem1,
            this.switchToGanttViewItem1,
            this.groupByNoneItem1,
            this.groupByDateItem1,
            this.groupByResourceItem1});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 29;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1,
            this.homeRibbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(783, 142);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Show Print Options...";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barCheckItem1
            // 
            this.barCheckItem1.Caption = "RibbonForm";
            this.barCheckItem1.Checked = true;
            this.barCheckItem1.Id = 12;
            this.barCheckItem1.Name = "barCheckItem1";
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
            this.ribbonPageGroup1.ItemLinks.Add(this.barCheckItem1);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // schedulerBarController1
            // 
            this.schedulerBarController1.BarItems.Add(this.newAppointmentItem1);
            this.schedulerBarController1.BarItems.Add(this.newRecurringAppointmentItem1);
            this.schedulerBarController1.BarItems.Add(this.navigateViewBackwardItem1);
            this.schedulerBarController1.BarItems.Add(this.navigateViewForwardItem1);
            this.schedulerBarController1.BarItems.Add(this.gotoTodayItem1);
            this.schedulerBarController1.BarItems.Add(this.viewZoomInItem1);
            this.schedulerBarController1.BarItems.Add(this.viewZoomOutItem1);
            this.schedulerBarController1.BarItems.Add(this.switchToDayViewItem1);
            this.schedulerBarController1.BarItems.Add(this.switchToWorkWeekViewItem1);
            this.schedulerBarController1.BarItems.Add(this.switchToWeekViewItem1);
            this.schedulerBarController1.BarItems.Add(this.switchToMonthViewItem1);
            this.schedulerBarController1.BarItems.Add(this.switchToTimelineViewItem1);
            this.schedulerBarController1.BarItems.Add(this.switchToGanttViewItem1);
            this.schedulerBarController1.BarItems.Add(this.groupByNoneItem1);
            this.schedulerBarController1.BarItems.Add(this.groupByDateItem1);
            this.schedulerBarController1.BarItems.Add(this.groupByResourceItem1);
            this.schedulerBarController1.Control = this.schedulerControl1;
            // 
            // appointmentRibbonPageGroup1
            // 
            this.appointmentRibbonPageGroup1.ItemLinks.Add(this.newAppointmentItem1);
            this.appointmentRibbonPageGroup1.ItemLinks.Add(this.newRecurringAppointmentItem1);
            this.appointmentRibbonPageGroup1.Name = "appointmentRibbonPageGroup1";
            // 
            // homeRibbonPage1
            // 
            this.homeRibbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.appointmentRibbonPageGroup1,
            this.navigatorRibbonPageGroup1,
            this.arrangeRibbonPageGroup1,
            this.groupByRibbonPageGroup1});
            this.homeRibbonPage1.Name = "homeRibbonPage1";
            // 
            // newAppointmentItem1
            // 
            this.newAppointmentItem1.Id = 13;
            this.newAppointmentItem1.Name = "newAppointmentItem1";
            // 
            // newRecurringAppointmentItem1
            // 
            this.newRecurringAppointmentItem1.Id = 14;
            this.newRecurringAppointmentItem1.Name = "newRecurringAppointmentItem1";
            // 
            // navigatorRibbonPageGroup1
            // 
            this.navigatorRibbonPageGroup1.ItemLinks.Add(this.navigateViewBackwardItem1);
            this.navigatorRibbonPageGroup1.ItemLinks.Add(this.navigateViewForwardItem1);
            this.navigatorRibbonPageGroup1.ItemLinks.Add(this.gotoTodayItem1);
            this.navigatorRibbonPageGroup1.ItemLinks.Add(this.viewZoomInItem1);
            this.navigatorRibbonPageGroup1.ItemLinks.Add(this.viewZoomOutItem1);
            this.navigatorRibbonPageGroup1.Name = "navigatorRibbonPageGroup1";
            // 
            // navigateViewBackwardItem1
            // 
            this.navigateViewBackwardItem1.Id = 15;
            this.navigateViewBackwardItem1.Name = "navigateViewBackwardItem1";
            // 
            // navigateViewForwardItem1
            // 
            this.navigateViewForwardItem1.Id = 16;
            this.navigateViewForwardItem1.Name = "navigateViewForwardItem1";
            // 
            // gotoTodayItem1
            // 
            this.gotoTodayItem1.Id = 17;
            this.gotoTodayItem1.Name = "gotoTodayItem1";
            // 
            // viewZoomInItem1
            // 
            this.viewZoomInItem1.Id = 18;
            this.viewZoomInItem1.Name = "viewZoomInItem1";
            // 
            // viewZoomOutItem1
            // 
            this.viewZoomOutItem1.Id = 19;
            this.viewZoomOutItem1.Name = "viewZoomOutItem1";
            // 
            // arrangeRibbonPageGroup1
            // 
            this.arrangeRibbonPageGroup1.ItemLinks.Add(this.switchToDayViewItem1);
            this.arrangeRibbonPageGroup1.ItemLinks.Add(this.switchToWorkWeekViewItem1);
            this.arrangeRibbonPageGroup1.ItemLinks.Add(this.switchToWeekViewItem1);
            this.arrangeRibbonPageGroup1.ItemLinks.Add(this.switchToMonthViewItem1);
            this.arrangeRibbonPageGroup1.ItemLinks.Add(this.switchToTimelineViewItem1);
            this.arrangeRibbonPageGroup1.ItemLinks.Add(this.switchToGanttViewItem1);
            this.arrangeRibbonPageGroup1.Name = "arrangeRibbonPageGroup1";
            // 
            // switchToDayViewItem1
            // 
            this.switchToDayViewItem1.Id = 20;
            this.switchToDayViewItem1.Name = "switchToDayViewItem1";
            // 
            // switchToWorkWeekViewItem1
            // 
            this.switchToWorkWeekViewItem1.Id = 21;
            this.switchToWorkWeekViewItem1.Name = "switchToWorkWeekViewItem1";
            // 
            // switchToWeekViewItem1
            // 
            this.switchToWeekViewItem1.Id = 22;
            this.switchToWeekViewItem1.Name = "switchToWeekViewItem1";
            // 
            // switchToMonthViewItem1
            // 
            this.switchToMonthViewItem1.Id = 23;
            this.switchToMonthViewItem1.Name = "switchToMonthViewItem1";
            // 
            // switchToTimelineViewItem1
            // 
            this.switchToTimelineViewItem1.Id = 24;
            this.switchToTimelineViewItem1.Name = "switchToTimelineViewItem1";
            // 
            // switchToGanttViewItem1
            // 
            this.switchToGanttViewItem1.Id = 25;
            this.switchToGanttViewItem1.Name = "switchToGanttViewItem1";
            // 
            // groupByRibbonPageGroup1
            // 
            this.groupByRibbonPageGroup1.ItemLinks.Add(this.groupByNoneItem1);
            this.groupByRibbonPageGroup1.ItemLinks.Add(this.groupByDateItem1);
            this.groupByRibbonPageGroup1.ItemLinks.Add(this.groupByResourceItem1);
            this.groupByRibbonPageGroup1.Name = "groupByRibbonPageGroup1";
            // 
            // groupByNoneItem1
            // 
            this.groupByNoneItem1.Id = 26;
            this.groupByNoneItem1.Name = "groupByNoneItem1";
            // 
            // groupByDateItem1
            // 
            this.groupByDateItem1.Id = 27;
            this.groupByDateItem1.Name = "groupByDateItem1";
            // 
            // groupByResourceItem1
            // 
            this.groupByResourceItem1.Id = 28;
            this.groupByResourceItem1.Name = "groupByResourceItem1";
            // 
            // FormScheduler
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(783, 512);
            this.Controls.Add(this.schedulerControl1);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "FormScheduler";
            this.Text = "FormScheduler";
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerBarController1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            schedulerControl1.ShowPrintOptionsForm();
        }

        private void schedulerControl1_EditAppointmentFormShowing(object sender, DevExpress.XtraScheduler.AppointmentFormEventArgs e) {
            if (barCheckItem1.Checked) {
                XtraForm form;
                // Create a form.
                form = new AppointmentFormRibbonStyle(schedulerControl1, e.Appointment, e.OpenRecurrenceForm);
                // Comply with restrictions.
                ((AppointmentFormRibbonStyle)form).ReadOnly = e.ReadOnly;
                e.DialogResult = form.ShowDialog(e.Parent);
                e.Handled = true;
            }
        }
	}
}