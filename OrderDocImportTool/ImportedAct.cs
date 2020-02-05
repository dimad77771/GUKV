using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
    [Serializable()]
    public class ImportedAct
    {
        public string docNum = "";

        public string docTitle = "";

        public DateTime docDate = DateTime.MinValue;

        public string masterDocNum = "";

        public DateTime masterDocDate = DateTime.MinValue;

        public List<ActObject> actObjects = new List<ActObject>();

        public decimal docSum = 0m;

        public decimal docFinalSum = 0m;

        public ImportedAct()
        {
        }
    }
}
