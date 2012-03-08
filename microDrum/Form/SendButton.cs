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
    public partial class SendButton : UserControl
    {
        Graphics g = null;
        static Bitmap AnimatedImage = new Bitmap(global::microDrum.Properties.Resources.SendButton);
        Rectangle destRect = new Rectangle(0, 0, 27, 27);
        Rectangle srcRect = new Rectangle(0, 0, 27, 27);
        private int Value = 0;

        private void SendButton_Paint(object sender, PaintEventArgs e)
        {
            srcRect.Y = Value * 27;
            g.DrawImage(AnimatedImage, destRect, srcRect, GraphicsUnit.Pixel);
        }
        public SendButton()
        {
            InitializeComponent();
            g = CreateGraphics();
        }

        private void SendButton_MouseEnter(object sender, EventArgs e)
        {
            Value = 1;
            Refresh();
        }

        private void SendButton_MouseLeave(object sender, EventArgs e)
        {
            Value = 0;
            Refresh();
        }

        private void SendButton_MouseDown(object sender, MouseEventArgs e)
        {
            Value = 4;
            Refresh();
        }

        private void SendButton_MouseHover(object sender, EventArgs e)
        {
            Value = 2;
            Refresh();
        }

        private void SendButton_MouseUp(object sender, MouseEventArgs e)
        {
            Value = 3;
            Refresh();
        }
    }
}
