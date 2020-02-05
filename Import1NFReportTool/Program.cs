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

            Importer.WriteInfo("Starting import");

            // Read preferences from the INI files
            Preferences preferences = new Preferences();

            if (!preferences.ReadConnPreferencesFromFile())
            {
                Importer.WriteError("Could not read connection preferences from the file " + Preferences.iniConnFileName);

                return;
            }

            if (!preferences.ReadPathPreferencesFromFile())
            {
                Importer.WriteError("Could not read input-output folder preferences from the file " + Preferences.iniPathFileName);

                return;
            }

            // Make sure that all folders (input, output, error) exist
            if (!System.IO.Directory.Exists(preferences.input1NFFolder))
            {
                Importer.WriteError("The specified input directory " + preferences.input1NFFolder + " does not exist");
                return;
            }

            // Perform import
            Importer importer = new Importer();

            importer.Import(preferences);

            // Finished
            Importer.WriteInfo("Import finished.");
        }
    }
}
