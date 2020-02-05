using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
    public class DocumentNJF
    {
        public int documentId = -1;

        public int documentKind = -1;

        public string documentNumber = "";

        public string documentTitle = "";

        public DateTime documentDate = DateTime.MinValue;

        public List<int> dependentDocuments = null;

        public DocumentNJF()
        {
        }

        public override string ToString()
        {
            return "N " + documentNumber + " від " + documentDate.ToShortDateString() + ": " + documentTitle;
        }
    }
}
