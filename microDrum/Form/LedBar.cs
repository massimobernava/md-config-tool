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
    public partial class LedBar : UserControl
    {
        Graphics g = null;
        static Bitmap AnimatedImage = new Bitmap(global::microDrum.Properties.Resources.ProgressBar);
        Rectangle destRect = new Rectangle(0, 0, 128, 16);
        Rectangle srcRect = new Rectangle(0, 0, 128, 16);
        private int _Value = 0;
        public int Maximum = 99;
        public int Minimum = 0;

        public int Value
        {
            get { return _Value; }
            set { _Value = value; Refresh(); }
        }
        public LedBar()
        {
            InitializeComponent();
            g = CreateGraphics();
        }

        private void LedBar_Paint(object sender, PaintEventArgs e)
        {
            srcRect.Y = (50*Value/(Maximum-Minimum+1)) * 16;
            g.DrawImage(AnimatedImage, destRect, srcRect, GraphicsUnit.Pixel);
        }
    }
}
