using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SigmaServizioRapportini.App;

namespace SigmaServizioRapportini.App {
    class DatabaseManager {

        private static string prefixSP = "ServizioRapp_";

        public string LeggiTabellaServer(string nomeTabella, string condizione, string nomeCampo) {
            string risultato;
            string sSQL;
            DataSet dtsRICE = new DataSet();
            SqlDataAdapter adpRICE = new SqlDataAdapter();

            sSQL = "SELECT " + nomeCampo + " FROM " + nomeTabella + " WHERE " + condizione;
            adpRICE.SelectCommand = new SqlCommand(sSQL, new SqlConnection(getStringaCnnServer()));
            adpRICE.Fill(dtsRICE);
            risultato = "";
            if (dtsRICE.Tables[0].Rows.Count > 0)
                risultato = dtsRICE.Tables[0].Rows[0][nomeCampo].ToString();

            return risultato;
        }

        public string LeggiTabella(string nomeArchivio, string nomeTabella, string condizione, string nomeCampo) {
            string risultato;
            string sSQL;
            DataSet dtsRICE = new DataSet();
            SqlDataAdapter adpRICE = new SqlDataAdapter();

            sSQL = "SELECT " + nomeCampo + " FROM " + nomeTabella + " WHERE " + condizione;
            adpRICE.SelectCommand = new SqlCommand(sSQL, new SqlConnection(getStringaCnn(nomeArchivio)));
            adpRICE.Fill(dtsRICE);
            risultato = "";
            if (dtsRICE.Tables[0].Rows.Count > 0)
                risultato = dtsRICE.Tables[0].Rows[0][nomeCampo].ToString();

            return risultato;
        }

        public string LeggiTabella(string sSQL, string nomeCampo, string nomeArchivio = Globale.NomeDb) {
            string risultato;
            DataSet dtsRICE = new DataSet();
            SqlDataAdapter adpRICE = new SqlDataAdapter();

            adpRICE.SelectCommand = new SqlCommand(sSQL, new SqlConnection(getStringaCnn(nomeArchivio)));
            adpRICE.Fill(dtsRICE);
            risultato = "";
            if (dtsRICE.Tables[0].Rows.Count > 0)
                risultato = dtsRICE.Tables[0].Rows[0][nomeCampo].ToString();

            return risultato;
        }

        public DataTable GetDataTable(string stringaSQL, string nomeArchivio = Globale.NomeDb) {
            DataSet dtsRICE = new DataSet();
            SqlDataAdapter adpRICE = new SqlDataAdapter();

            adpRICE.SelectCommand = new SqlCommand(stringaSQL, new SqlConnection(getStringaCnn(nomeArchivio)));
            adpRICE.Fill(dtsRICE);
            return dtsRICE.Tables[0];
        }

        public DataTable GetDataTableSP(string nomeSP, SqlParameter[] parametri, string nomeArchivio = Globale.NomeDb) {
            SqlCommand cmd = new SqlCommand();

            try {
                using (SqlConnection cnn = new SqlConnection(getStringaCnn(nomeArchivio))) {
                    cmd.Connection = cnn;

                    if (parametri != null)
                        cmd.Parameters.AddRange(parametri);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = prefixSP + nomeSP;
                    cmd.Connection.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader()) {
                        var tb = new DataTable();
                        tb.Load(dr);
                        return tb;
                    }
                    //cmd.Connection.Close();
                }
            } catch (Exception ex) {
                Globale.ScriviFileLog(ex.Message.ToString());
                return null;
            }
        }

        public DataTable GetDataTableServer(string stringaSQL) {
            DataSet dtsRICE = new DataSet();
            SqlDataAdapter adpRICE = new SqlDataAdapter();

            adpRICE.SelectCommand = new SqlCommand(stringaSQL, new SqlConnection(getStringaCnnServer()));
            adpRICE.Fill(dtsRICE);
            return dtsRICE.Tables[0];
        }

        public void AggiornaDbServer(string stringaSQL, ref int numeroRighe) {
            SqlCommand cmd = new SqlCommand();

            try {
                cmd.Connection = new SqlConnection(getStringaCnnServer());
                cmd.CommandText = stringaSQL;
                cmd.Connection.Open();
                numeroRighe = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            } catch (Exception ex) {
                numeroRighe = 0;
            }
        }

        public void EseguiSP(string nomeSP, SqlParameter[] parametri, string nomeArchivio = Globale.NomeDb) {
            SqlCommand cmd = new SqlCommand();

            try {
                using (SqlConnection cnn = new SqlConnection(getStringaCnn(nomeArchivio))) {
                    cmd.Connection = cnn;
                    cmd.Parameters.AddRange(parametri);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = prefixSP + nomeSP;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            } catch (Exception ex) {
                Globale.ScriviFileLog(ex.Message.ToString());
            }
        }

        private string getStringaCnnServer() {
            try {
                return Globale.CnnString;
            } catch (Exception ex) {
                Globale.ScriviFileLog(ex.Message.ToString());
                return "";
            }
        }

        private string getStringaCnn(string nomeArchivio) {
            try {
                string cnns = Globale.CnnString;
                cnns = cnns.Replace("\r", "").Replace("\n", "");
                cnns = cnns.Replace(";Database=GiaNt-Server", ";Database=" + nomeArchivio);

                return cnns;
            } catch (Exception ex) {
                Globale.ScriviFileLog(ex.Message.ToString());
                return "";
            }
        }
    }
}
