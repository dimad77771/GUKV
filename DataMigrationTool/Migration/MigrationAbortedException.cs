using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUKV.DataMigration
{
    /// <summary>
    /// This exception is thrown when data migration should be aborted with no error message.
    /// </summary>
    public class MigrationAbortedException : Exception
    {
        public MigrationAbortedException()
        {
        }

        public MigrationAbortedException(string message)
            : base(message)
        {
        }
    }
}
