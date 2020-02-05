using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace TestLocalization {
    public partial class FormChart : DevExpress.XtraBars.Ribbon.RibbonForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public FormChart() {
            // Required for Windows Form Designer support
            InitializeComponent();

            // TODO: Add any constructor code after InitializeComponent call
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e) {
            new DevExpress.XtraCharts.Wizard.ChartWizard(chartControl1).ShowDialog();
        }
    }
}