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
    public partial class ObjectPickerForm : Form
    {
        private List<Object1NF> foundObjects = new List<Object1NF>();

        public Object1NF selectedObject = null;

        public ObjectPickerForm()
        {
            InitializeComponent();

            // Fill the drop-down of streets
            DB.FillComboBoxFromDictionary1NF(comboStreet, DB.DICT_STREETS);
        }

        private void comboStreet_SelectedIndexChanged(object sender, EventArgs e)
        {
            foundObjects.Clear();

            if (comboStreet.SelectedItem is DictionaryValue)
            {
                int streetId = (comboStreet.SelectedItem as DictionaryValue).key;

                if (streetId > 0)
                {
                    // Find all objects that correspond to the selected street
                    foreach (KeyValuePair<int, Object1NF> pair in DB.objects1NF)
                    {
                        if (pair.Value.streetId == streetId)
                        {
                            foundObjects.Add(pair.Value);
                        }
                    }
                }
            }

            // Sort the objects by address string
            foundObjects.Sort(new Object1NFAlphabeticalComparer());

            // Fill the List View
            listObjects.Items.Clear();

            foreach (Object1NF obj in foundObjects)
            {
                ListViewItem lviObject = new ListViewItem();

                lviObject.Text = obj.ToString();

                listObjects.Items.Add(lviObject);
            }
        }

        private void btnSelectObject_Click(object sender, EventArgs e)
        {
            selectedObject = null;

            foreach (int index in listObjects.SelectedIndices)
            {
                if (foundObjects.Count > index)
                {
                    selectedObject = foundObjects[index];
                }
            }
        }
    }
}
