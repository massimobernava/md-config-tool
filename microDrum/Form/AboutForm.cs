using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Management;

namespace microDrum
{
    public partial class AboutForm : Form
    {
        public static AboutForm Singleton = null;

        private const string PublicKey = "<RSAKeyValue><Modulus>odG/XBfzkOTENNcBcdbTN6vBnJLGf3CEFJmW/VxSDWs/T76pTBuAxPtk1D0X6/SqllO6y7N03GELDa9D7M1MKXMbl5TZL8GBWjueEZiKgQgcCKgX8RMqscZlNHwwlnU95NbnuBQQpK62edZps5natxHenksSb71s69DsA545Gm8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public AboutForm()
        {
            InitializeComponent();

            Singleton = this;
            lblVersion.Text = "v"+Version.String;

        }

        private GraphicsPath GetRoundRect(Rectangle rect, float radius)
        {

            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(rect.X + radius, rect.Y, rect.X + rect.Width - (radius * 2), rect.Y);
            gp.AddArc(rect.X + rect.Width - (radius * 2), rect.Y, radius * 2, radius * 2, 270, 90);

            gp.AddLine(rect.X + rect.Width, rect.Y + radius, rect.X + rect.Width, rect.Y + rect.Height - (radius * 2));
            gp.AddArc(rect.X + rect.Width - (radius * 2), rect.Y + rect.Height - (radius * 2), radius * 2, radius * 2, 0, 90);

            gp.AddLine(rect.X + rect.Width - (radius * 2), rect.Y + rect.Height, rect.X + radius, rect.Y + rect.Height);
            gp.AddArc(rect.X, rect.Y + rect.Height - (radius * 2), radius * 2, radius * 2, 90, 90);

            gp.AddLine(rect.X, rect.Y + rect.Height - (radius * 2), rect.X, rect.Y + radius);
            gp.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);


            gp.CloseFigure();

            return (gp);
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public bool IsValid()
        {
            string LicenseKey = GetLicenseKey();
            if (String.IsNullOrEmpty(LicenseKey)) return false;

            return UtilityCryptography.AsymmetricVerify(GetSoftwareKey(), LicenseKey, PublicKey);
        }

        private void AboutForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                this.Size = new Size(this.Size.Width, IsValid() ? 200 : 400);
                btnPayPal.Visible = !IsValid();
                Rectangle rect = this.DisplayRectangle;
                this.Region = new Region(GetRoundRect(rect, 10.0f));

                tbSoftwareKey.Text = GetSoftwareKey();
                tbLicenseKey.Text = "";// UtilityCryptography.AsymmetricSign(tbSoftwareKey.Text, PrivateKey);
            }
        }

        public static string GetCPUId()
        {
            string cpuInfo = String.Empty;
            string temp = String.Empty;

            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == String.Empty)
                {// only return cpuInfo from first CPU
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
            }
            return cpuInfo;
        }
        public static string GetVolumeSerial(string strDriveLetter)
        {
            if (strDriveLetter == "" || strDriveLetter == null) strDriveLetter = "C";
            ManagementObject disk =
                new ManagementObject("win32_logicaldisk.deviceid=\"" + strDriveLetter + ":\"");
            disk.Get();
            return disk["VolumeSerialNumber"].ToString();
        }
        public static string GetHDSerial()
        {

           	object so;
          	string sn = String.Empty;

			try
            {
				
            	ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_PhysicalMedia");

				ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject wml_HD in collection)
                {
                    if ((so = wml_HD["SerialNumber"]) != null && so is string)
                    {
                        sn = ((string)so).Trim();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
				return "";
            }
            return (sn);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (UtilityCryptography.AsymmetricVerify(tbSoftwareKey.Text, tbLicenseKey.Text, PublicKey))
            {
                MessageBox.Show("LicenseKey is valid");
                SetLicenseKey(tbLicenseKey.Text);
                DialogResult = DialogResult.OK;
                UtilityMIDI.MIDI_SysEx(0x01, 0x00, 0x00, 0x00);//SetState Off
                Close();
            }
            else MessageBox.Show("LicenseKey is not valid");
        }

        public string GetSoftwareKey()
        {
            return UtilityCryptography.GetMD5Hash(/*GetCPUId() +*/ "microDRUM" + GetHDSerial())+Version.String.Replace(".",""); ;
        }
        private string GetLicenseKey()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("microDRUM");

            if (key != null && key.GetValue("LicenseKey")!=null) return key.GetValue("LicenseKey").ToString();

            return null;
        }
        private void SetLicenseKey(string SoftwareKey)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE",true).CreateSubKey("microDRUM");
            if (key != null) key.SetValue("LicenseKey", SoftwareKey);

        }

        private void btnHomePage_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"http://microdrum.altervista.org/"));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnPayPal_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(@"https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=7YV276FU22H2W"));
        }

    }
}
