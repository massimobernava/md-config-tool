using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace microDrum
{
    public partial class ReportErrorForm : Form
    {
        public ReportErrorForm()
        {
            InitializeComponent();
        }

        public bool IsEmail(string Email)
        {
            //versione semplificata dell'espressione regolare corretta
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(Email))
                return (true);
            else
                return (false);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string filename = Path.GetDirectoryName(Application.ExecutablePath);
            filename += "\\BugReport.txt";
            if (!File.Exists(filename)) return;
            if (!IsEmail(txtEmail.Text)) { MessageBox.Show("Use your email!", "Please..."); txtEmail.Focus(); return; }

            foreach (Control c in Controls) c.Enabled = false;

            //Meglio farlo con un thread separato

            System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            mailClient.EnableSsl = true;

            System.Net.NetworkCredential cred = new System.Net.NetworkCredential(
                "username",
                "password");
            mailClient.Credentials = cred;
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(txtEmail.Text, "massimo.bernava@gmail.com", AboutForm.Singleton.GetSoftwareKey(), "Email:" + txtEmail.Text + Environment.NewLine + "Note:" + Environment.NewLine + txtNote.Text);
            message.Attachments.Add(new System.Net.Mail.Attachment(filename));

            //mailClient.Send(message);

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
