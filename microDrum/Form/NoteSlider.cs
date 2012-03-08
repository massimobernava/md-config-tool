using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace microDrum
{
    public partial class NoteSlider : UserControl
    {
        Graphics g = null;
        static Bitmap AnimatedImage = new Bitmap(global::microDrum.Properties.Resources.NoteSlider);
        Rectangle destRect = new Rectangle(0, 0, 70, 20);
        Rectangle srcRect = new Rectangle(0, 0, 70, 20);
        private int ValueX = 0;
        private int ValueY = 0;
        private int StatusX = -1;
        private int StatusY = -1;

        System.Drawing.Drawing2D.LinearGradientBrush lgb1 = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, 20, 20), Color.FromArgb(60, 65, 104, 156), Color.FromArgb(250, 0, 0, 0), 180.0f);
        System.Drawing.Drawing2D.LinearGradientBrush lgb2 = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(-10, 0, 20, 20), Color.FromArgb(60, 65, 104, 156), Color.FromArgb(250, 0, 0, 0), 0.0f);


        public int SelectedIndex
        {
            set
            {
                dudNoteBase.SelectedIndex = value;
                if (value == -1) return;
                ValueX = ((Note)dudNoteBase.SelectedItem).NoteValue - 1;
                ValueY = ((Note)dudNoteBase.SelectedItem).OctaveValue + 2;
            }
            get
            {
                if (_ShowSlider) return ValueX + 1 + ValueY * 12;
                else return dudNoteBase.SelectedIndex;

            }
        }
        public Note SelectedItem
        {
            get
            {
                if (_ShowSlider)
                    return new Note((byte)(ValueX + 1 + ValueY * 12));
                else
                    return (Note)dudNoteBase.SelectedItem;
            }
        }
        public System.Windows.Forms.DomainUpDown.DomainUpDownItemCollection Items
        {
            get
            {
                return dudNoteBase.Items;
            }
        }
        public event EventHandler SelectedItemChanged;

        public NoteSlider()
        {
            InitializeComponent();
            g = CreateGraphics();
            g.CompositingQuality = CompositingQuality.HighQuality;
            for (byte n = 95; n >0; n--)
            {
                dudNoteBase.Items.Add(new Note(n));
            }
            //Rectangle rect = this.DisplayRectangle;
            //this.Region = new Region(GetRoundRect(rect, 2.0f));
        }

        private bool _ShowSlider = false;
        public bool ShowSlider
        {
            get { return _ShowSlider; }
            set { 
                _ShowSlider = value; 
                dudNoteBase.Visible = !value; 
                
                if (value)
                {
                    ValueX = ((Note)dudNoteBase.SelectedItem).NoteValue - 1;
                    ValueY = ((Note)dudNoteBase.SelectedItem).OctaveValue + 2;
                }
                else dudNoteBase.SelectedIndex = 95-(ValueX+1 + ValueY * 12);
                this.Refresh();
            }

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
        private void NoteSlider_Paint(object sender, PaintEventArgs e)
        {
            if (!_ShowSlider)
            {
                g.FillRectangle(Brushes.Transparent, 0, 0, 70, 20);
                return;
            }
            srcRect.X = /*(ValueX % 2==1) ? 10 + ValueX * 15 :*/ 10 + ValueX * 30;
            srcRect.Y = ValueY * 20;
            g.DrawImage(AnimatedImage, destRect, srcRect, GraphicsUnit.Pixel);
            g.DrawRectangle(new Pen(Color.FromArgb(50,Color.Black), 5), 0, 0, 69,19);
            g.FillRectangle(lgb1, 0, 0, 19, 19);
            g.FillRectangle(lgb2, 51, 0, 19, 19);
            g.DrawRectangle(new Pen(Color.FromArgb(200,255,0,0),3), 18, 0, 33, 19);
        }

        private void NoteSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                StatusX = e.X;
                StatusY = e.Y;
            }
        }

        private void NoteSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //if (ValueX % 2 == 1) { ValueX++; Refresh(); }
                StatusX = -1;
                StatusY = -1;
            }
        }

        private void NoteSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (StatusX != -1)
            {
                ValueX += (e.X - StatusX);
                StatusX = e.X;
                ValueY += (e.Y - StatusY);
                StatusY = e.Y;
                if (ValueX < -1)ValueX = -1;
                else if (ValueX > 10) ValueX = 10;
                if (ValueY < 0) ValueY = 0;
                else if (ValueY > 7) ValueY = 7;
                
                //if (ValueChanged != null) ValueChanged(this, new EventArgs());
                if (SelectedItemChanged != null) SelectedItemChanged(this, null);

                this.Refresh();
            }
        }

        private void dudNoteBase_SelectedItemChanged(object sender, EventArgs e)
        {
            if (SelectedItemChanged != null) SelectedItemChanged(this, null);
        }
    }
}
