using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace microDrum
{
    public partial class EffectsChainControl : UserControl
    {
        public AudioEffectChain EffectChain=new AudioEffectChain();
        List<EffectControl> EffectControls = new List<EffectControl>();
        EffectControl empty = new EffectControl(new EmptyEffect());

        public EffectsChainControl()
        {
            InitializeComponent();
            
            empty.AutoSize = true;
            empty.Location = new System.Drawing.Point(3, 3);//+120
            empty.Size = new System.Drawing.Size(442, 106);
            empty.TabIndex = 0;
            empty.IsEmpty = true;
            empty.AddRemoveButton += new EventHandler(empty_AddRemoveButton);
            
            this.Controls.Add(empty);
            EffectControls.Add(empty);
        }

        void empty_AddRemoveButton(object sender, EventArgs e)
        {
            //Visualizza lista
            contextMenuEffects.Show(((Control)sender).Controls["btnAddRemove"], 0, 0);
        }

        public void AddEffect(AudioEffect ae)
        {

            EffectControl ec = new EffectControl(ae);
            ec.AutoSize = true;
            
            ec.Size = new System.Drawing.Size(442, 106);
            ec.TabIndex = 0;
            ec.IsEmpty = false;
            ec.AddRemoveButton += new EventHandler(ec_AddRemoveButton);
            ec.MoveUpButton += new EventHandler(ec_MoveUpButton);
            ec.MoveDownButton += new EventHandler(ec_MoveDownButton);

            EffectChain.AddEffect(ae);
            this.Controls.Add(ec);
            EffectControls.Add(ec);
            
            for (int i = 0; i < EffectControls.Count;i++)
              EffectControls[i].Location = new System.Drawing.Point(3, 3 + 110 * (EffectControls.Count-i-1));
        }

        void ec_MoveDownButton(object sender, EventArgs e)
        {
            int index = EffectControls.IndexOf((EffectControl)sender);
            if (index == -1)
            {
                throw new ArgumentException("The specified effect is not part of this effect chain");
            }
            if (index > 1)
            {
                EffectControls.RemoveAt(index);
                EffectControls.Insert(index - 1, (EffectControl)sender);

                for (int i = 0; i < EffectControls.Count; i++)
                    EffectControls[i].Location = new System.Drawing.Point(3, 3 + 110 * (EffectControls.Count - i - 1));

                EffectChain.MoveDown(((EffectControl)sender).Effect);
            }
        }

        void ec_MoveUpButton(object sender, EventArgs e)
        {
            int index = EffectControls.IndexOf((EffectControl)sender);
            if (index == -1)
            {
                throw new ArgumentException("The specified effect is not part of this effect chain");
            }
            if (index < EffectControls.Count-1)
            {
                EffectControls.RemoveAt(index);
                EffectControls.Insert(index + 1, (EffectControl)sender);

                for (int i = 0; i < EffectControls.Count; i++)
                    EffectControls[i].Location = new System.Drawing.Point(3, 3 + 110 * (EffectControls.Count - i - 1));

                EffectChain.MoveUp(((EffectControl)sender).Effect);
            }
        }

        void ec_AddRemoveButton(object sender, EventArgs e)
        {
            EffectChain.Remove(((EffectControl)sender).Effect);
            EffectControls.Remove((EffectControl)sender);
            Controls.Remove((EffectControl)sender);

            for (int i = 0; i < EffectControls.Count;i++)
                EffectControls[i].Location = new System.Drawing.Point(3, 3 + 110 * (EffectControls.Count-i-1));
        }
        private void emptyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEffect(new EmptyEffect());
        }

        private void volumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEffect(new VolumeEffect());
        }

        private void threeBandEQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEffect(new ThreeBandEQEffect());
        }

        private void badBussMojoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEffect(new BadBussMojo());
        }

        private void pitchDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEffect(new PitchDown());
        }
    }
}
