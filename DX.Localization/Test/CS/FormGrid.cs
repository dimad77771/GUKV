using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using System.Collections;

namespace TestLocalization {
    public partial class FormGrid : DevExpress.XtraEditors.XtraForm {
        public FormGrid() {
            InitializeComponent();
        }

        private void FormGrid_Load(object sender, EventArgs e) {
            gridControl1.DataSource = new ArrayList(Application.OpenForms);
            cardView1.PopulateColumns();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            UserControl ctrl = ((IPrintable)gridControl1).PropertyEditorControl;
            using(Form frm = new Form()) {
                frm.Size = new Size(600, 400);
                ctrl.Dock = DockStyle.Fill;
                frm.Controls.Add(ctrl);
                frm.ShowDialog();
            }
        }
    }
}