using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Data;

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

        public static void Segnala(string dataRapp, string numeRapp, DataRow drImpostazioni) {
            using (var ms = new MemoryStream())
            using (var client = new SmtpClient())
            using (var msg = new MailMessage()) {
                try {
                    msg.To.Add(new MailAddress("denise.colleoni@sigma-sistemi.it;chiara.annoni@sigma-sistemi.it"));

                    msg.From = new MailAddress(drImpostazioni["CMRGENFROM"].ToString(), drImpostazioni["CMRGENFRON"].ToString());
                    msg.Subject = "Rilevati problemi sul rapportino: " + drImpostazioni["CMRGENOGGE"].ToString();
                    msg.Subject = msg.Subject.Replace("§DATARAPP§", dataRapp);
                    msg.Subject = msg.Subject.Replace("§NUMERAPP§", numeRapp);

                    msg.Body = msg.Subject;
                    msg.IsBodyHtml = true;

                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(drImpostazioni["CMRGENFROM"].ToString(), drImpostazioni["CMRGENUPWD"].ToString());
                    client.Port = Convert.ToInt16(drImpostazioni["CMRGENPORT"]);
                    client.Host = drImpostazioni["CMRGENSMTP"].ToString();
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = true;
                    if (drImpostazioni["CMRGENMSSL"].ToString() == "0")
                        client.EnableSsl = false;


                    client.Send(msg);
                } catch (Exception ex) {

                }
            }
        }
    }
}





