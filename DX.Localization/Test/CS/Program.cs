using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TestLocalization {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            if(args.Length > 0) {
                try {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(args[0]);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(args[0]);
                    Application.Run(new FormMain());
                    return;
                }
                catch {
                }
            }
            else {
                // 1. Uncomment code and change the culture name if necessary. The list of available 
                //    culture names can be found in the "CultureInfo Class" topic in the MSDN Library
                // 2. Copy the satellite assemblies you are testing to the subfolder of the project's bin
                //    directory.  The subfolder name must correspond to the culture name, 
                //    e.g. Dutch assemblies must be copied to bin\Debug\nl

                // Catalan (ca)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ca-ES");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ca");

                // Czech (cs)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("cs-CZ");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("cs");

                // Dutch (nl)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nl-NL");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl");

                // French (fr)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-FR");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");

                // German (de)
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");

                // Greek (el)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("el-GR");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("el");

                // Hungarian (hu)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("hu-HU");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("hu");

                // Italian (it)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("it-IT");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("it");

                // Norwegian (no)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nb-NO");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("no");

                // Polish (pl)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pl-PL");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pl");

                // Portuguese (pt)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-PT");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt");

                // Russian (ru)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");

                // Spanish(es)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es");

                // Slovak (sk)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sk-SK");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sk");

                // Slovenian(sl)
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("sl-SI");
                //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sl");
            }

            Application.Run(new FormMain());
        }
    }
}