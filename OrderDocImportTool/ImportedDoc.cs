using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
    [Serializable()]
    public class ImportedDoc
    {
        public string docNum = "";

        public string docTitle = "";

        public DateTime docDate = DateTime.MinValue;

        public int docTypeId = 0;

        public string masterDocNum = "";

        public DateTime masterDocDate = DateTime.MinValue;

        public List<Appendix> appendices = new List<Appendix>();

        public decimal docSum = 0m;

        public decimal docFinalSum = 0m;

        public ImportedDoc()
        {
        }

        public bool IsAct()
        {
            return docTypeId == 3;
        }
    }
}
