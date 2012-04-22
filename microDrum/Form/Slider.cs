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
    public partial class Slider : UserControl, System.ComponentModel.ISupportInitialize
    {
        Graphics g = null;
        static Bitmap AnimatedImage = new Bitmap(global::microDrum.Properties.Resources.Slider);
        Rectangle destRect = new Rectangle(0, 0, 48, 20);
        Rectangle srcRect = new Rectangle(0, 0, 48, 20);
        private int _Value = 0;
        public int Value
        {
            get { return _Value; }
            set { 
                _Value = value; 
                lblValue.Text = _Value.ToString();
                Decimal tmp=(Decimal)value;
                if (tmp < Minimum) tmp = Minimum;
                if (tmp > Maximum) tmp = Maximum;
                nudBase.Value = tmp;
                if (ValueChanged != null) ValueChanged(this,new EventArgs()); 
                Refresh();
            }
        }
        public int Maximum
        {
            get { return _Maximum; }
            set { _Maximum = value; nudBase.Maximum = (decimal)value; }
        }
        private int _Maximum = 100;
        public int Minimum
        {
            get { return _Minimum; }
            set { _Minimum = value; nudBase.Minimum = (decimal)value; }
        }
        private int _Minimum = 0;

        int Status = -1;

        public event EventHandler ValueChanged;

        public Slider()
        {
            InitializeComponent();
            g = CreateGraphics();
            nudBase.ValueChanged += new EventHandler(nudBase_ValueChanged);
        }

        private bool _ShowSlider = false;
        public bool ShowSlider
        {
            get { return _ShowSlider; }
            set{ _ShowSlider=value; nudBase.Visible=!value;}

        }

        void nudBase_ValueChanged(object sender, EventArgs e)
        {
            _Value = (int)nudBase.Value;
            if (ValueChanged != null) ValueChanged(this, new EventArgs()); 
        }

        private void Slider_Paint(object sender, PaintEventArgs e)
        {
            srcRect.Y = (99 * _Value / (Maximum - Minimum + 1)) * 20;
            g.DrawImage(AnimatedImage, destRect, srcRect, GraphicsUnit.Pixel);
        }

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            if (Status != -1)
            {
                _Value += (e.X - Status) * (e.X - Status) * (e.X - Status);
                Status = e.X;
                if (_Value < 0) _Value = 0;
                else if (_Value > Maximum) _Value = Maximum;
                lblValue.Text = _Value.ToString();
                if (ValueChanged != null) ValueChanged(this, new EventArgs()); 

                this.Refresh();
            }
        }

        private void Slider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Status = e.X;
            }
        }

        private void Slider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Status = -1;
            }
        }


        #region ISupportInitialize Membri di

        public void BeginInit()
        {
            nudBase.BeginInit();
        }

        public void EndInit()
        {
            nudBase.EndInit();
        }

        #endregion
    }
}
