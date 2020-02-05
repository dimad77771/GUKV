using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
    [Serializable()]
    public class Appendix
    {
        public string appendixNum = "";

        public List<AppendixObject> objects = new List<AppendixObject>();

        public Appendix()
        {
        }

        public override string ToString()
        {
            return "Додаток " + appendixNum;
        }
    }
}
