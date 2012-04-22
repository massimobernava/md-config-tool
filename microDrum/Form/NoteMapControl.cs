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
    public partial class NoteMapControl : UserControl
    {
        public NoteMapControl()
        {
            InitializeComponent();
        }
        public NoteMapControl(int FromNote,int ToNote)
        {
            InitializeComponent();

            slFromNote.SelectedIndex = 95- (byte)(FromNote & 0x00FF);
            rFromNote.Checked = (byte)(FromNote >> 8)==0x90;
            rFromCC.Checked = (byte)(FromNote >> 8) == 0xB0;

            slToNote.SelectedIndex = 95 - (byte)(ToNote & 0x00FF);
            rToNote.Checked = (byte)(ToNote >> 8) == 0x90;
            rToCC.Checked = (byte)(ToNote >> 8) == 0xB0;

            this.Size = new Size(this.Size.Width, 36);
        }
        public int FromNote
        {
            get { return (rFromNote.Checked?0x9000:0xB000) | (95-slFromNote.SelectedIndex); }
            //set { slFromNote.SelectedIndex = value; }
        }

        public int ToNote
        {
            get { return (rToNote.Checked ? 0x9000 : 0xB000) | (95-slToNote.SelectedIndex); }
            //set { slToNote.SelectedIndex = value; }
        }

        public bool Selected
        {
            get { return chkSelected.Checked; }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            btnOpen.Text = this.Size.Height == 108 ? "_" : "^";
            this.Size = new Size(this.Size.Width, this.Size.Height == 108 ? 36 : 108);
        }
    }
}
