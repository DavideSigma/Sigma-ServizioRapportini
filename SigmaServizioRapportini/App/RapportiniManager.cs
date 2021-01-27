using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;

namespace SigmaServizioRapportini.App {

    class RapportiniManager {

        public void Elabora() {
            try {
                DatabaseManager dbMgr = new DatabaseManager();

                DataTable dtImpostazioni = dbMgr.GetDataTableSP("CMRGEN_impostazioni", null);
                if (dtImpostazioni.Rows.Count == 0)
                    return;

                ReportLoader rptLoadr;
                Byte[] bytesReport;

                DataTable dt = dbMgr.GetDataTableSP("CMPIAN_elencoRapportiniDaInviare", null);
                foreach (DataRow dr in dt.Rows) {
                    SqlParameter[] parametri = new SqlParameter[1];
                    SqlParameter prm1 = new SqlParameter("CMPIANUPRO", dr["CMPIANUPRO"].ToString());
                    parametri[0] = prm1;

                    DataTable dtRapp = dbMgr.GetDataTableSP("CMPIAN_stampaRapportino", parametri);
                    rptLoadr = new ReportLoader();
                    bytesReport = rptLoadr.GetBytesReport(new SectionReport1(), dtRapp);

                    if (sendMail(bytesReport, dr["CMDIPDEMAI"].ToString(), dtRapp.Rows[0]["CMPIANDATA"].ToString(), dr["CMPIANUPRO"].ToString(), dtImpostazioni.Rows[0])) {
                        flaggaRapportinoInviato(dr["CMPIANUPRO"].ToString());
                    }
                }
            } catch (Exception ex) {
                Globale.ScriviFileLog(ex.Message.ToString());
            }
        }

        private bool sendMail(Byte[] bytesReport, string mailDestinatario, string dataRapp, string numeRapp, DataRow drImpostazioni) {
            using (var ms = new MemoryStream())
            using (var client = new SmtpClient())
            using (var msg = new MailMessage()) {
                try {
                    mailDestinatario = mailDestinatario.Replace(";", "");

                    msg.To.Add(new MailAddress(mailDestinatario));
                    msg.From = new MailAddress(drImpostazioni["CMRGENFROM"].ToString(), drImpostazioni["CMRGENFRON"].ToString());
                    msg.Subject = drImpostazioni["CMRGENOGGE"].ToString();
                    msg.Subject = msg.Subject.Replace("§DATARAPP§", dataRapp);
                    msg.Subject = msg.Subject.Replace("§NUMERAPP§", numeRapp);

                    msg.Body = drImpostazioni["CMRGENTMAI"].ToString() + drImpostazioni["CMRGENFIRM"].ToString();
                    msg.IsBodyHtml = true;

                    MemoryStream msReport = new MemoryStream(bytesReport);
                    msReport.Position = 0;

                    string nomeFileRapp = "Rapportino" + numeRapp + ".pdf";

                    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf); // System.Net.Mime.MediaTypeNames.Text.Plain 
                    Attachment allegato = new System.Net.Mail.Attachment(msReport, ct);
                    allegato.ContentDisposition.FileName = nomeFileRapp;
                    msg.Attachments.Add(allegato);

                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(drImpostazioni["CMRGENFROM"].ToString(), drImpostazioni["CMRGENUPWD"].ToString());
                    client.Port = Convert.ToInt16(drImpostazioni["CMRGENPORT"]);
                    client.Host = drImpostazioni["CMRGENSMTP"].ToString();
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = true;
                    if (drImpostazioni["CMRGENMSSL"].ToString() == "0")
                        client.EnableSsl = false;


                    client.Send(msg);
                    return true;
                } catch (Exception ex) {
                    Globale.ScriviFileLog(ex.Message.ToString());
                    return false;
                }
            }
        }

        private void flaggaRapportinoInviato(string numeRapp) {
            try { 
                DatabaseManager dbMgr = new DatabaseManager();

                SqlParameter[] parametri = new SqlParameter[1];
                SqlParameter prm1 = new SqlParameter("CMPIANUPRO", numeRapp);
                parametri[0] = prm1;

                dbMgr.EseguiSP("CMPIAN_setRapportinoInviato", parametri);
            } catch (Exception ex) {
                Globale.ScriviFileLog(ex.Message.ToString());
            }
        }

    }
}
