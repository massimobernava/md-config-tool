using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace microDrum
{
    public partial class EffectControl : UserControl
    {
        public AudioEffect Effect;

        public event EventHandler AddRemoveButton;
        public event EventHandler MoveUpButton;
        public event EventHandler MoveDownButton;
        private bool isEmpty = false;

        public EffectControl(AudioEffect ae)
        {
            InitializeComponent();
            lblEffectName.Text = ae.Name;
            Effect = ae;

            int i=0;
            foreach (AudioEffectFactor Factor in Effect.Factors)
            {
                /*TrackBar tb = new TrackBar();
                tb.Tag = Factor;
                tb.Location = new System.Drawing.Point(55+i*45, 3);
                tb.Orientation = System.Windows.Forms.Orientation.Vertical;
                tb.Size = new System.Drawing.Size(45, 65);
                tb.TickStyle = System.Windows.Forms.TickStyle.Both;
                tb.Minimum = 0;
                tb.Maximum = 1000;
                tb.SmallChange = 50;
                tb.LargeChange = 100;
                tb.TickFrequency = 50;
                tb.Value = FactorToTrackBar(tb);
                tb.ValueChanged += new EventHandler(tb_ValueChanged);*/
                //panelEffect.Controls.Add(tb);

                Knob bn = new Knob();
                bn.Tag = Factor;
                bn.Location = new System.Drawing.Point(75 + (i/2) * 45, 3+(i%2)*50);
                bn.Size = new System.Drawing.Size(32, 32);
                bn.Value = FactorToKnob(bn);
                bn.ValueChanged += new EventHandler(bn_ValueChanged);
                bn.ToolTipText = Factor.Description;
                panelEffect.Controls.Add(bn);

                Panel pan = new Panel();
                pan.BackColor = System.Drawing.Color.SteelBlue;
                pan.Location = new System.Drawing.Point(110+(i/2)*45, 5);
                pan.Size = new System.Drawing.Size(1, 90);
                panelEffect.Controls.Add(pan);

                Label lblV = new Label();
                lblV.Location = new System.Drawing.Point(65 + (i/2) * 45, 35 + (i % 2) * 50);
                lblV.Size = new System.Drawing.Size(45, 15);
                lblV.Font = new Font(lblV.Font.FontFamily, 7.0f);
                lblV.Text = Factor.Value.ToString("0.00");
                lblV.Tag = bn;
                panelEffect.Controls.Add(lblV);

                /*Label lbl = new Label();
                lbl.Location = new System.Drawing.Point(65 + i * 45, 55);
                lbl.Size = new System.Drawing.Size(45, 50);
                lbl.Font = new Font(lbl.Font.FontFamily, 7.0f);
                lbl.Text = Factor.Description;
                panelEffect.Controls.Add(lbl);*/

                i++;
            }
        }

        void bn_ValueChanged(object sender, EventArgs e)
        {
            ((AudioEffectFactor)((Knob)sender).Tag).Value = KnobToFactor((Knob)sender);
            Effect.OnFactorChanges();
            foreach(Control c in panelEffect.Controls)
                if (c is Label && c.Tag == sender) c.Text = ((AudioEffectFactor)((Knob)sender).Tag).Value.ToString("0.00");
        }
        float KnobToFactor(Knob kn)
        {
            float value = 0;

            if (kn.Value == kn.Maximum)
            {
                value = ((AudioEffectFactor)kn.Tag).Maximum;
            }
            else
            {
                value = ((AudioEffectFactor)kn.Tag).Minimum + (kn.Value * (((AudioEffectFactor)kn.Tag).Maximum - ((AudioEffectFactor)kn.Tag).Minimum) / (float)kn.Maximum);
                value -= value % ((AudioEffectFactor)kn.Tag).Increment;
            }

            return value;
        }
        float TrackBarToFactor(TrackBar tb)
        {
            float value = 0;

            if (tb.Value == tb.Maximum)
            {
                value = ((AudioEffectFactor)tb.Tag).Maximum;
            }
            else
            {
                value = ((AudioEffectFactor)tb.Tag).Minimum + (tb.Value * (((AudioEffectFactor)tb.Tag).Maximum - ((AudioEffectFactor)tb.Tag).Minimum) / 1000.0f);
                value -= value % ((AudioEffectFactor)tb.Tag).Increment;
            }

            return value;
        }
        int FactorToKnob(Knob kn)
        {
            return (int)(((((AudioEffectFactor)kn.Tag).Value - ((AudioEffectFactor)kn.Tag).Minimum) / (((AudioEffectFactor)kn.Tag).Maximum - ((AudioEffectFactor)kn.Tag).Minimum)) * (float)kn.Maximum);

        }
        int FactorToTrackBar(TrackBar tb)
        {
            return (int)(((((AudioEffectFactor)tb.Tag).Value - ((AudioEffectFactor)tb.Tag).Minimum) / (((AudioEffectFactor)tb.Tag).Maximum - ((AudioEffectFactor)tb.Tag).Minimum)) * 1000.0f);
        }
        void tb_ValueChanged(object sender, EventArgs e)
        {
            ((AudioEffectFactor)((TrackBar)sender).Tag).Value = TrackBarToFactor((TrackBar)sender);
            Effect.OnFactorChanges();
        }

        private void btnAddRemove_Click(object sender, EventArgs e)
        {
            if (AddRemoveButton != null) AddRemoveButton(this, e);
        }

        public bool IsEmpty
        {
            get { return isEmpty; }
            set
            {
                isEmpty = value;

                if (isEmpty)
                {
                    btnAddRemove.Text = "Add";
                    btnUp.Visible = false;
                    btnDown.Visible = false;
                    panelEffect.Visible = false;
                }
                else
                {
                    btnAddRemove.Text = "Remove";
                    btnUp.Visible = true;
                    btnDown.Visible = true;
                    panelEffect.Visible = true;
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (MoveUpButton != null) MoveUpButton(this, e);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (MoveDownButton != null) MoveDownButton(this, e);
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Effect.Enabled = chkEnabled.Checked;
        }
    }
}
