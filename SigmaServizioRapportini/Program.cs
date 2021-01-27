using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SigmaServizioRapportini.App;

namespace SigmaServizioRapportini {

    class Program {

        static void Main(string[] args) {
            Globale.CnnString = args[0];

            string AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            Globale.PercorsoFileLog = AppPath + "\\SigmaServizioRapportini.log";

            SigmaServizioRapportini.App.RapportiniManager rappMgr = new SigmaServizioRapportini.App.RapportiniManager();
            rappMgr.Elabora();
        }

    }
}
