using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using DevExpress.XtraGauges.Core;

namespace TestLocalization {
    public partial class FormGauge : DevExpress.XtraBars.Ribbon.RibbonForm {
        public FormGauge() {
            InitializeComponent();
        }
        void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            StyleManager.Show(gaugeControl1);        
        }
    }
}