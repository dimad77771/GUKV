using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUKV.BalansDataMappingTool
{
    public partial class SqlErrorForm : Form
    {
        public SqlErrorForm()
        {
            InitializeComponent();
        }

        public string SqlStatement
        {
            set
            {
                textBoxSqlStatement.Text = value;
            }

            get
            {
                return textBoxSqlStatement.Text;
            }
        }

        public string ErrorMessage
        {
            set
            {
                textBoxErrorMessage.Text = value;
            }

            get
            {
                return textBoxErrorMessage.Text;
            }
        }
    }
}
