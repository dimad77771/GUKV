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
    public partial class RishPickerForm : Form
    {
        public DocumentNJF selectedDocument = null;

        public RishPickerForm()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string mask = editMask.Text.Trim().ToUpper();

            if (mask.Length > 0)
            {
                List<DocumentNJF> foundDocs = new List<DocumentNJF>();

                foreach (KeyValuePair<int, DocumentNJF> doc in DB.documentsNJF)
                {
                    if (doc.Value.documentNumber.Contains(mask))
                    {
                        foundDocs.Add(doc.Value);
                    }
                }

                foundDocs.Sort(new DocumentNJFComparerByDate());

                listDocuments.Items.Clear();

                foreach (DocumentNJF doc in foundDocs)
                {
                    ListViewItem item = new ListViewItem(doc.documentNumber);
                    item.SubItems.Add(doc.documentDate.ToShortDateString());
                    item.SubItems.Add(doc.documentTitle);

                    item.Tag = doc;

                    listDocuments.Items.Add(item);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, введіть номер документу для пошуку (повністю або частково).",
                    "Пошук Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (listDocuments.SelectedItems.Count > 0)
            {
                selectedDocument = listDocuments.SelectedItems[0].Tag as DocumentNJF;

                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Будь ласка, виберіть документ зі списку.",
                    "Вибір Розпорядчого Документу",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }
    }

    public class DocumentNJFComparerByDate : IComparer<DocumentNJF>
    {
        public DocumentNJFComparerByDate()
        {
        }

        int IComparer<DocumentNJF>.Compare(DocumentNJF x, DocumentNJF y)
        {
            if (x.documentDate < y.documentDate)
                return -1;

            if (x.documentDate > y.documentDate)
                return 1;

            return x.documentNumber.CompareTo(y.documentNumber);
        }
    }
}
