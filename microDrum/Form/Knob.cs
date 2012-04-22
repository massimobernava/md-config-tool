using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace microDrum
{
    public partial class Knob : UserControl
    {
        Graphics g = null;
        static Bitmap AnimatedImage = new Bitmap(global::microDrum.Properties.Resources.knob);
        Rectangle destRect = new Rectangle(0, 0, 32, 32);
        Rectangle srcRect = new Rectangle(0, 0, 32, 32);
        public int Value = 0;
        int Status = -1;

        public string ToolTipText
        {
            set { toolTip.SetToolTip(this, value); }
        }
        public readonly int Minimum = 0;
        public readonly int Maximum = (AnimatedImage.Height / AnimatedImage.Width) - 2;
        public event EventHandler ValueChanged;

        public Knob()
        {
            InitializeComponent();
            g = CreateGraphics();
            
        }

        private void Knob_Paint(object sender, PaintEventArgs e)
        {
            srcRect.Y = Value * 32 + 32;
            g.DrawImage(AnimatedImage, destRect, srcRect, GraphicsUnit.Pixel);
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (Status != -1)
            {
                Value += (e.X - Status);
                Status = e.X;
                if(Value<0)Value=0;
                else if (Value > Maximum) Value = Maximum;
                if (ValueChanged != null) ValueChanged(this, null);

                this.Refresh();
            }
        }


        
        private void Knob_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Status=e.X;
            }
        }

        private void Knob_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Status=-1;
            }
        }
    }
}