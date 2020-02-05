using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUKV.DataMigration
{
    public partial class DataLoadingForm : Form
    {
        public DataLoadingForm()
        {
            InitializeComponent();
        }

        public ProgressBar Progress
        {
            get
            {
                return progressBar;
            }
        }
    }
}
