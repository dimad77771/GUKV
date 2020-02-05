using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration.Migration
{
    public interface IMigrationLog
    {
        void WriteInfo(string p);

        void WriteSQLError(string sqlStatement, string errorMessage);

        void WriteError(string p);
    }
}
