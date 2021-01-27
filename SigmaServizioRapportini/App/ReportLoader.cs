using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Data;
using GrapeCity.ActiveReports.Export.Pdf.Section;

namespace SigmaServizioRapportini.App {
    class ReportLoader {

        //        public string anteprima(string nome, SectionReport1 report, Hashtable opzioni = null) {

        //            DataTable tb = new DataTable();

        ////tb = sp.Selezione("CMPIAN_stampa")
        //        //Return rptLoader.anteprima("CM001_R1", New CM001_R1, tb, Me.Context, htaParametri)

        //            return elaboraReport(nome, report, opzioni);
        //        }

        //        private string elaboraReport(string nome, SectionReport1 report, Hashtable opzioni = null) {
        //            try {
        //                report.Opzioni = opzioni;
        //                report.Run();

        //                return "";
        //            } catch (Exception ex1) {
        //                return ex1.Source;
        //            }
        //        }

        //public void WriteReport(SectionReport1 report, DataTable dt, Hashtable opzioni = null) {
        //    try {
        //        report.Opzioni = opzioni;
        //        report.DataSource = dt;
        //        report.Run(false);

        //        byte[] bReport = export(report);

        //        string fileName = "B:\\temp\\aaaaRapportino.pdf";
        //        using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write)) {
        //            fs.Write(bReport, 0, bReport.Length);
        //        }
        //    } catch (Exception ex) {
        //        Console.WriteLine("Exception caught in process: {0}", ex);
        //    }
        //}

        public byte[] GetBytesReport(SectionReport1 report, DataTable dt, Hashtable opzioni = null) {
            try {
                report.Opzioni = opzioni;
                report.DataSource = dt;
                report.Run();

                return export(report);
            } catch (Exception ex1) {
                Globale.ScriviFileLog(ex1.Message.ToString());
                return null;
            }
        }

        private byte[] export(SectionReport1 report) {
            PdfExport pdfReportExp = new PdfExport();

            using (MemoryStream ms = new MemoryStream()) {
                try {

                    pdfReportExp.Export(report.Document, ms);

                    byte[] byteReport = new byte[ms.Length];
                    //var byteReport = new byte[ms.Length];

                    ms.Position = 0;
                    ms.Read(byteReport, 0, System.Convert.ToInt32(ms.Length));

                    ms.Close();
                    ms.Dispose();

                    return byteReport;
                } catch (Exception ex1) {
                    Globale.ScriviFileLog(ex1.Message.ToString());
                    return null;
                }
            }
        }
    }
}
