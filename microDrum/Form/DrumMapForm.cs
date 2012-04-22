using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace microDrum
{
    public partial class DrumMapForm : Form
    {
        private List<DMap> drumMap = new List<DMap>();

        public DMap[] DrumMap
        {
            get
            {
                return drumMap.ToArray();
            }
        }
        public DrumMapForm(DMap[] DrumMap)
        {
            InitializeComponent();

            foreach (DMap dm in DrumMap)
            {
                lbPads.Items.Add(dm);
            }
            lbPads.SelectedIndex = 0;
        }

        private void lbPads_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPads.SelectedIndex >= 0)
            {
                DMap dmap = (DMap)lbPads.SelectedItem;
                txtName.Text = dmap.Name;
                nudHead.Value = dmap.Head;
                nudRim.Value = dmap.Rim;
                cbSingle.Checked = dmap.Single;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            UpdateSelected();
        }
        private void UpdateSelected()
        {
            if (lbPads.SelectedIndex >= 0)
            {
                DMap dmap = (DMap)lbPads.SelectedItem;
                dmap.Name = txtName.Text;
                dmap.Head = (byte)nudHead.Value;
                dmap.Rim = (byte)nudRim.Value;
                dmap.Single = cbSingle.Checked;
                lbPads.Items[lbPads.SelectedIndex] = dmap;
            }
        }
        private void nudHead_ValueChanged(object sender, EventArgs e)
        {
            UpdateSelected();
        }

        private void nudRim_ValueChanged(object sender, EventArgs e)
        {
            UpdateSelected();
        }

        private void cbSingle_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSelected();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DMap dm in lbPads.Items)
                drumMap.Add(dm);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text)) return;

            DMap dmap = new DMap();
            dmap.Name = txtName.Text;
            dmap.Head = (byte)nudHead.Value;
            dmap.Rim = (byte)nudRim.Value;
            dmap.Single = cbSingle.Checked;
            lbPads.Items.Add(dmap);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbPads.SelectedIndex >= 0)
            lbPads.Items.RemoveAt(lbPads.SelectedIndex);
        }
    }
}
