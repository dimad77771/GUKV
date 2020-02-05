
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace GUKV.DataMigrationConsoleTool
{
    public class MigrationLog : GUKV.DataMigration.Migration.IMigrationLog
    {
        public MigrationLog()
        {
        }

        #region Interface

        /// <summary>
        /// Removes all messages from the log
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Adds a new message to the log
        /// </summary>
        /// <param name="message">The message text</param>
        public void WriteInfo(string message)
        {
            Logger logger = GetRootLog();

            logger.Log(Level.Info, message, null);
        }

        /// <summary>
        /// Writes an error message to the log
        /// </summary>
        /// <param name="message">The message text</param>
        public void WriteError(string message)
        {
            Logger logger = GetRootLog();

            logger.Log(Level.Error, message, null);
        }

        /// <summary>
        /// Writes an SQL statement error message to the log
        /// </summary>
        public void WriteSQLError(string sqlStatement, string errorMessage)
        {
            Logger logger = GetRootLog();

            logger.Log(Level.Error, "SQL Error: " + errorMessage, null);
            logger.Log(Level.Error, "SQL Statement: " + sqlStatement, null);
        }

        /// <summary>
        /// Writes the provided exception to the log
        /// </summary>
        /// <param name="ex">Exception to write</param>
        private void WriteException(Exception ex)
        {
            Logger logger = GetRootLog();

            logger.Log(Level.Error, "Exception occured", ex);
        }

        #endregion (Interface)

        #region Implementation

        /// <summary>
        /// Returns the root logger of the log4net hierarchy
        /// </summary>
        /// <returns>The root logger of the log4net hierarchy</returns>
        private Logger GetRootLog()
        {
            Hierarchy h = (Hierarchy)log4net.LogManager.GetRepository();

            return h.Root;
        }

        #endregion (Implementation)
    }
}
