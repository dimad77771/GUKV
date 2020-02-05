using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;

namespace TestLocalization {
    public partial class FormTreeList : DevExpress.XtraEditors.XtraForm {
        public FormTreeList() {
            InitializeComponent();
        }

        private void FormTreeList_Load(object sender, EventArgs e) {
            new DevExpress.XtraTreeList.Design.XViews(treeList1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            UserControl ctrl = ((IPrintable)treeList1).PropertyEditorControl;
            using(Form frm = new Form()) {
                frm.Size = new Size(600, 400);
                ctrl.Dock = DockStyle.Fill;
                frm.Controls.Add(ctrl);
                frm.ShowDialog();
            }
        }
    }
}