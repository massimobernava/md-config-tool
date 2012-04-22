using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace microDrum
{
    public partial class LogForm : Form
    {
        internal List<Log> Logs = new List<Log>();

        public LogForm()
        {
            InitializeComponent();
        }

        private void pbLog_Paint(object sender, PaintEventArgs e)
        {
            uint iTime=Logs[0].Time;
            uint fTime = Logs[Logs.Count-1].Time;
            for(int i=0;i<Logs.Count-1;i++)
            {
                e.Graphics.DrawLine(Pens.Black, new Point((int)(((Logs[i].Time - iTime) * 1000) / (fTime-iTime)), 256-(Logs[i].Y0/4)), new Point((int)(((Logs[i + 1].Time - iTime) * 1000) / (fTime-iTime)), 256-(Logs[i + 1].Y0/4)));
            }
        }
    }
}
