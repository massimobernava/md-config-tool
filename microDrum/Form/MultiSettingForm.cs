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
    public partial class MultiSettingForm : Form
    {
        public MultiSettingForm()
        {
            InitializeComponent();

            cbTypeMulti.Items.Add(PinType.Piezo);
            cbTypeMulti.Items.Add(PinType.Switch);
            cbTypeMulti.Items.Add(PinType.HHC);
            cbTypeMulti.Items.Add(PinType.HH);
            cbTypeMulti.Items.Add(PinType.HHs);
            cbTypeMulti.Items.Add(PinType.YSwitch);
            cbTypeMulti.Items.Add(PinType.Disabled);

            for (byte n = 83; n > 0; n--)
            {
                dudNoteMulti.Items.Add(new Note(n));
            }
            dudNoteMulti.Items.Add(new Note(255));
        }

        private void btnSendMulti_Click(object sender, EventArgs e)
        {
            if (nudTo.Value - nudFrom.Value < 0) return;

            pbSendMulti.Maximum=(int)(nudTo.Value - nudFrom.Value);
            pbSendMulti.Value = 0;

            for (byte i = (byte)nudFrom.Value; i < nudTo.Value; i++)
            {
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (chkSave.Checked ? 0x01 : 0x00)), i, 0x0D, (byte)cbTypeMulti.SelectedIndex);
                System.Threading.Thread.Sleep(10);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (chkSave.Checked ? 0x01 : 0x00)), i, 0x00, ((Note)dudNoteMulti.SelectedItem).Value);
                System.Threading.Thread.Sleep(10);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (chkSave.Checked ? 0x01 : 0x00)), i, 0x01, (byte)nudThresoldMulti.Value);
                System.Threading.Thread.Sleep(10);

                pbSendMulti.Value = pbSendMulti.Value + 1;
            }
        }
    }
}
