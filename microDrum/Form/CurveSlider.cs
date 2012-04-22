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
    public partial class CurveSlider : UserControl
    {
        Graphics g = null;
        static Bitmap AnimatedImage = new Bitmap(global::microDrum.Properties.Resources.CurveSlider);
        Rectangle destRect = new Rectangle(0, 0, 100, 20);
        Rectangle srcRect = new Rectangle(0, 0, 100, 20);
        private int Value = 0;
        private int Status = 0;

        public event EventHandler SelectedIndexChanged;
        public int SelectedIndex
        {
            get { return Status; }
            set { 
                Status = value; 
                cbBase.SelectedIndex = value;
                if (SelectedIndexChanged != null) SelectedIndexChanged(this, new EventArgs());
            }
        }

        public CurveSlider()
        {
            InitializeComponent();
            g = CreateGraphics();
        }

        private void CurveSlider_Paint(object sender, PaintEventArgs e)
        {
            srcRect.Y = Value * 20;
            g.DrawImage(AnimatedImage, destRect, srcRect, GraphicsUnit.Pixel);
        }

        private void CurveSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Status = (e.X/20);
                if (SelectedIndexChanged != null) SelectedIndexChanged(this, new EventArgs());

            }
        }

        private void CurveSlider_MouseUp(object sender, MouseEventArgs e)
        {
            /*if (e.Button == MouseButtons.Left)
            {
                Status =-1;
            }*/
        }

        private void timerPaint_Tick(object sender, EventArgs e)
        {
            if (Status*6 != Value)
            {
                Value += Math.Sign(Status*6-Value);
                Refresh();
            }
        }

        private void cbBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            Status = cbBase.SelectedIndex;
            if (SelectedIndexChanged != null) SelectedIndexChanged(this, new EventArgs());

        }
        private bool _ShowSlider = false;
        public bool ShowSlider
        {
            get { return _ShowSlider; }
            set { _ShowSlider = value; cbBase.Visible = !value; }

        }
    }
}
