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
    public partial class AddColumnForm : Form
    {
        public static string ObjectFieldPrefix = "object-";
        public static string BalansFieldPrefix = "balans-";
        public static string ArendaFieldPrefix = "arenda-";
        public static string PrivatFieldPrefix = "privat-";

        public Dictionary<string, string> ObjectProperties = null;
        public Dictionary<string, string> BalansProperties = null;
        public Dictionary<string, string> ArendaProperties = null;
        public Dictionary<string, string> PrivatProperties = null;

        public string SelectedColumnTitle = "";
        public string SelectedColumnFieldName = "";

        public AddColumnForm()
        {
            InitializeComponent();
        }

        private void AddColumnForm_Load(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;

            radioButtonBalansField.Checked = false;
            radioButtonArendaField.Checked = false;
            radioButtonPrivatField.Checked = false;
            radioButtonObjectField.Checked = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string fields = "";
            string title = "";

            for (int i = 0; i < listBoxFields.CheckedItems.Count; i++)
            {
                object item = listBoxFields.CheckedItems[i];

                if (item is FieldItemInfo)
                {
                    if (fields.Length > 0)
                    {
                        fields += "|";
                        title += ", ";
                    }

                    fields += (item as FieldItemInfo).fieldName;
                    title += (item as FieldItemInfo).displayName;
                }
            }

            SelectedColumnTitle = title;
            SelectedColumnFieldName = fields;
        }

        private void radioButtonObjectField_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonObjectField.Checked)
            {
                FillListOfFields(ObjectProperties, ObjectFieldPrefix);
            }
        }

        private void radioButtonBalansField_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonBalansField.Checked)
            {
                FillListOfFields(BalansProperties, BalansFieldPrefix);
            }
        }

        private void radioButtonArendaField_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonArendaField.Checked)
            {
                FillListOfFields(ArendaProperties, ArendaFieldPrefix);
            }
        }

        private void radioButtonPrivatField_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonPrivatField.Checked)
            {
                FillListOfFields(PrivatProperties, PrivatFieldPrefix);
            }
        }

        private void FillListOfFields(Dictionary<string, string> fields, string prefix)
        {
            listBoxFields.Items.Clear();

            foreach (KeyValuePair<string, string> pair in fields)
            {
                listBoxFields.Items.Add(new FieldItemInfo(prefix + pair.Key, pair.Value), false);
            }

            buttonOK.Enabled = false;
        }

        private void listBoxFields_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                buttonOK.Enabled = true;
            }
            else
            {
                // We need to ensure that there is at least one checked item in the list
                buttonOK.Enabled = (listBoxFields.CheckedItems.Count > 1);
            }
        }
    }

    public class FieldItemInfo
    {
        public string fieldName = "";
        public string displayName = "";

        public FieldItemInfo()
        {
        }

        public FieldItemInfo(string field, string title)
        {
            fieldName = field;
            displayName = title;
        }

        public override string ToString()
        {
            return displayName;
        }
    }
}
