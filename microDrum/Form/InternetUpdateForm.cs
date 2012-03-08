using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace microDrum
{
    public partial class InternetUpdateForm : Form
    {
        public InternetUpdateForm()
        {
            InitializeComponent();
            lblVersion.Text = Version.String;
        }

        private void InternetUpdateForm_Shown(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\Manifest.ini");

            Downloader.Download(@"http://microdrum.altervista.org/Manifest.ini", Path.GetDirectoryName(Application.ExecutablePath) + @"\Manifest.ini");

            //Controlla versione
            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\Manifest.ini"))
            {
                string Version = UtilityIniFile.GetIniString("Config", "LastVersion", Path.GetDirectoryName(Application.ExecutablePath) + @"\Manifest.ini");
                //Proponi pagina di download
                lblCurrentRelease.Invoke(new EventHandler(delegate{lblCurrentRelease.Text = Version;}));
                if (microDrum.Version.Int < Convert.ToInt32(Version))
                    btnGo.Invoke(new EventHandler(delegate { btnGo.Enabled = true; }));


            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"http://microdrum.altervista.org/blog/downloads/"));

        }
    }
}
