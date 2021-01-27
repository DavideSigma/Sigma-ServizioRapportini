using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SigmaServizioRapportini.App {

    public static class Globale {
        public static string PercorsoFileLog = "";
        public static string CnnString = "";
        public const string NomeDb = "Giant-SigmaSistemi";

        public static void ScriviFileLog(string testoMessaggio) {
            try {
                File.AppendAllText(Globale.PercorsoFileLog, DateTime.Now.ToString() + "\t" + testoMessaggio + "\r\n");
            } catch (Exception ex1) {

            }
        }
    }
}


