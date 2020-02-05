using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace TestLocalization {
    public partial class FormLayoutAndEditors : DevExpress.XtraEditors.XtraForm {
        public FormLayoutAndEditors() {
            InitializeComponent();
        }

        private void simpleButton6_Click(object sender, EventArgs e) {
            spellChecker1.Check(memoEdit1);
        }
    }
}