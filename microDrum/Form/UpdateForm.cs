using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace microDrum
{
    public partial class UpdateForm : Form
    {
        public string COM = null;

        public UpdateForm()
        {
            InitializeComponent();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string ArduinoPath = txtArduino.Text;
            string FirmwarePath = txtHex.Text;
            COM = nudCOM.Value.ToString();

            // They come from the Arduino installation directory arduino/hardware/tools/avr/bin
            if (!File.Exists(ArduinoPath + "hardware\\tools\\avr\\bin\\avrdude.exe"))
            {
                MessageBox.Show("avrdude tool not installed", "AVRUpdate error");
                return;
            }
            if (!File.Exists(ArduinoPath + "hardware\\tools\\avr\\etc\\avrdude.conf"))
            {
                MessageBox.Show("avrdude config file not installed", "AVRUpdate error");
                return;
            }
            if (!File.Exists(ArduinoPath + "hardware\\tools\\avr\\bin\\cygwin1.dll"))
            {
                MessageBox.Show("avrdude cygwin dll not installed", "AVRUpdate error");
                return;
            }
            if (!File.Exists(ArduinoPath + "hardware\\tools\\avr\\bin\\libusb0.dll"))
            {
                MessageBox.Show("avrdude usb dll not installed", "AVRUpdate error");
                return;
            }

            // THis file is the new image to be uploaded to the Arduino board...
            if (!File.Exists(FirmwarePath))
            {
                MessageBox.Show("microDRUM Firmware???", "Error");
                return;
            }

            txtTrace.Text += "(DO NOT RESET OR TURN OFF TILL THIS COMPLETES)\r\n";
            MessageBox.Show("Click OK to Start", "AVR Update");
            string avrport;
            if (Convert.ToInt32(nudCOM.Value) <= 9)
                avrport = "COM" + nudCOM.Value.ToString();
            else
                avrport = "\\\\.\\COM" + nudCOM.Value.ToString();

            //string dir = installDir.Replace("\\", "/");

            Process avrprog = new Process();
            StreamReader avrstdout, avrstderr;
            StreamWriter avrstdin;
            ProcessStartInfo psI = new ProcessStartInfo("cmd");


            psI.UseShellExecute = false;
            psI.RedirectStandardInput = true;
            psI.RedirectStandardOutput = true;
            psI.RedirectStandardError = true;
            psI.CreateNoWindow = true;
            avrprog.StartInfo = psI;
            avrprog.Start();
            avrstdin = avrprog.StandardInput;
            avrstdout = avrprog.StandardOutput;
            avrstderr = avrprog.StandardError;
            avrstdin.AutoFlush = true;
            string portSpeed = radioUNO.Checked ? "-b115200" : "-b57600";
            //avrstdin.WriteLine(installDir + "\\avr\\avrdude.exe -Cavr/avrdude.conf -patmega328p -cstk500v1 -P" + avrport + " -b57600 -D -Uflash:w:" + dir + "/avr/AVRImage.hex:i");
            avrstdin.WriteLine("\""+ArduinoPath + "hardware\\tools\\avr\\bin\\avrdude.exe\" -C \"" + ArduinoPath.Replace("\\", "/") + "hardware/tools/avr/etc/avrdude.conf\" -patmega328p -cstk500v1 -P" + avrport + " " + portSpeed + " -D -Uflash:w:\"" + FirmwarePath.Replace("\\", "/") + "\":i");
            avrstdin.Close();
            txtTrace.Text = avrstdout.ReadToEnd();
            txtTrace.Text += avrstderr.ReadToEnd();
        }

        private void UpdateForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {

                if (MainForm.LastDirectoryUsed.ContainsKey("ArduinoDir"))
                    txtArduino.Text = MainForm.LastDirectoryUsed["ArduinoDir"];
                //txtArduino.Text = UtilityIniFile.GetIniString("Setup", "ArduinoPath", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");

                if (!String.IsNullOrEmpty(COM))
                    nudCOM.Value = Convert.ToInt32(COM.Replace("COM", ""));
            }
        }

        private void btnArduino_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Arduino EXE|arduino.exe";

            if (MainForm.LastDirectoryUsed.ContainsKey("ArduinoDir"))
                openFileDialog1.InitialDirectory = MainForm.LastDirectoryUsed["ArduinoDir"];

            openFileDialog1.ShowDialog();

            if (MainForm.LastDirectoryUsed.ContainsKey("ArduinoDir"))
                MainForm.LastDirectoryUsed["ArduinoDir"] = openFileDialog1.FileName.Replace("arduino.exe", "");
            else
                MainForm.LastDirectoryUsed.Add("ArduinoDir", openFileDialog1.FileName.Replace("arduino.exe", ""));

            txtArduino.Text = openFileDialog1.FileName.Replace("arduino.exe", "");
        }

        private void btnHex_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "microDRUM Firmware (*.mhd)|*.mhd";

            openFileDialog1.ShowDialog();

            txtHex.Text = openFileDialog1.FileName;
        }
    }
}
