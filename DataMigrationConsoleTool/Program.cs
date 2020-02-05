using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using log4net.Config;

namespace GUKV.DataMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            DeleteOutdatedLogFiles();

            // Initialize log4net from the configuration file
            XmlConfigurator.Configure(new System.IO.FileInfo("Log4NetConfig.xml"));

            // Create a logger
            GUKV.DataMigration.Migration.IMigrationLog log = new DataMigrationConsoleTool.MigrationLog();

            // Perform data migration
            Migrator migrator = new Migrator(log);

            if (migrator.TestConnection())
            {
                migrator.Disconnect();
                migrator.PerformMigration();
            }

            migrator.Disconnect();
        }

        private static void DeleteOutdatedLogFiles()
        {
            // Find all outdated log files
            IEnumerable<string> files = Directory.EnumerateFiles(".\\Logs", "migration*");

            IEnumerator<string> filesEnumerator = files.GetEnumerator();

            List<string> filesToDelete = new List<string>();

            while (filesEnumerator.MoveNext())
            {
                string fileName = filesEnumerator.Current;

                FileInfo info = new FileInfo(fileName);

                TimeSpan time = DateTime.Now - info.CreationTime;

                if (time.Days > 5)
                {
                    // This file should be deleted
                    filesToDelete.Add(fileName);
                }
            }

            //if ((DateTime.Now - new DateTime(2000, 1, 1)).TotalDays - typeof(Program).Assembly.GetName().Version.Build > 150)
            //{
            //    Console.Error.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(
            //        "QnVpbGQgZXhwaXJhdGlvbiBjaGVjayBoYXMgZmFpbGVkLiBUaGlzIGJ1aWxkIGlzIG5vIGxvbmdlciBhY3RpdmUuIFBsZWFzZSwgZGVwbG95IGEgbW9yZSByZWNlbnQgYnVpbGQgdG8gcmVzdW1lIGZ1bmN0aW9uYWxpdHkgb2YgdGhpcyBjb21tYW5kLWxpbmUgdXRpbGl0eS4=")
            //    ));
            //    Environment.Exit(0);
            //}

            // Delete all outdated log files
            foreach (string file in filesToDelete)
            {
                File.Delete(file);
            }
        }
    }
}
