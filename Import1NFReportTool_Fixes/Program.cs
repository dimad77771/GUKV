using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace GUKV.DataMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            // If the folder "Logs" does not exist, create it
            if (!System.IO.Directory.Exists(".\\Logs"))
            {
                System.IO.Directory.CreateDirectory(".\\Logs");
            }

            // Initialize log4net from the configuration file
            if (System.IO.File.Exists("Log4NetConfig.xml"))
            {
                XmlConfigurator.Configure(new System.IO.FileInfo("Log4NetConfig.xml"));
            }
            else if (System.IO.File.Exists("../../Log4NetConfig.xml"))
            {
                XmlConfigurator.Configure(new System.IO.FileInfo("../../Log4NetConfig.xml"));
            }

            // Read preferences from the INI files
            Preferences preferences = new Preferences();

            if (preferences.ReadConnPreferencesFromFile() &&
                preferences.ReadPathPreferencesFromFile())
            {
                ImportFixes fixes = new ImportFixes();

                if (fixes.TestConnections(preferences))
                {
                    ImportFixes.WriteInfo("Starting fixes");

                    fixes.OpenConnections();

                    try
                    {
                        // fixes.FixRentSquareImportFailure();
                        // fixes.FixKZhSEBalansDuplicates();
                        // fixes.CaseSensitivityFix();
                    }
                    catch (Exception ex)
                    {
                        ImportFixes.WriteError("Exception during fix execution: " + ex.Message);
                    }

                    fixes.CloseConnections();

                    ImportFixes.WriteInfo("Finished");
                }
            }
        }
    }
}
