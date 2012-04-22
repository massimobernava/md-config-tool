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
    public partial class ChannelForm : Form
    {
        private PinSetting[] Setting = null;
        private DMap[] DrumMap = null;

        public void Set(PinSetting[] setting, DMap[] drummap)
        {
            Setting = setting;
            DrumMap = drummap;

            ShowChannels(-1);

        }
        private void ShowChannels(int SelectedIndex)
        {
            lstChannel.Items.Clear();
            for (int i = 0; i < Setting.Length; i++)
            {
                foreach (DMap dmap in DrumMap)
                    if (dmap.Head == i)
                        lstChannel.Items.Add(i + ") " + dmap.Name + " Head Ch:" + (Setting[i].Channel+1));
                    else if (dmap.Rim == i)
                        lstChannel.Items.Add(i + ") " + dmap.Name + " Rim Ch:" + (Setting[i].Channel+1));

            }

            if (SelectedIndex >= 0) lstChannel.SelectedIndex = SelectedIndex;
        }
        public ChannelForm()
        {
            InitializeComponent();


        }

        private void nudChannel_ValueChanged(object sender, EventArgs e)
        {
            if (lstChannel.SelectedIndex >= 0)
                Setting[lstChannel.SelectedIndex].Channel = (byte)(nudChannel.Value-1);

            ShowChannels(lstChannel.SelectedIndex);
        }

        private void lstChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstChannel.SelectedIndex >= 0)
            {
                nudChannel.Enabled = true;
                nudChannel.Value = Setting[lstChannel.SelectedIndex].Channel + 1;
            }
            else nudChannel.Enabled = false;
        }

        private void sendChannels_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Setting.Length; i++)
            {
                UtilitySetting.SetPinParam((byte)i, uDrumParam.Channel, Setting[i].Channel);
                lbChannels.Maximum = Setting.Length;
                lbChannels.Value = i;
            }
        }
    }
}
