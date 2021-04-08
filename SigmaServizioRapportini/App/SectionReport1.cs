using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace SigmaServizioRapportini.App {
    /// <summary>
    /// Summary description for SectionReport1.
    /// </summary>
    public partial class SectionReport1 : GrapeCity.ActiveReports.SectionReport {

        public Hashtable Opzioni;

        public SectionReport1() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private void Detail1_Format(object sender, EventArgs e) {
            LineSX.Height = shpTOTA.Top + shpTOTA.Height - labDATA.Top; // Detail.Height
            LineDX.Height = shpTOTA.Top + shpTOTA.Height - labDATA.Top; // Detail.Height

            if (txtCMPIANNORE.Text.Trim() == "")
                txtCMPIANNORE.Text = "0";

            if (txtCMPIANOVIA.Text.Trim() == "")
                txtCMPIANOVIA.Text = "0";

            txtCMPIANNORE.Text = (Convert.ToDecimal(txtCMPIANNORE.Text) + Convert.ToDecimal(txtCMPIANOVIA.Text)).ToString();
        }
    }
}
