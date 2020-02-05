using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dedupe
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public void LoadBuildingsGroup(List<int> buildIds) {
            var data = new Context().BuildingsFull.Where(b => buildIds.Contains(b.Id)).ToList();
            dataGridView1.DataSource = data;        
        }
    }
}
