using System;
using System.Collections.Generic;
using System.Text;
using NAudio.Wave;
using microDrum;

namespace NAudio.Wave
{
    public abstract class AudioEffectWaveStream : WaveStream
    {
        private AudioEffectChain audioEffects = null;

        public AudioEffectChain Chain
        {
            set
            {
                audioEffects = value;
            }
        }

        public void BlockEffects(int samplesblock)
        {
            if (audioEffects != null)
            {
                foreach (AudioEffect ae in audioEffects)
                {
                    ae.Block(samplesblock);
                }
            }
        }
        public void ApplyEffects(ref float LeftSample, ref float RightSample)
        {
            if (audioEffects != null)
            {
                foreach (AudioEffect ae in audioEffects)
                {
                    if(ae.Enabled)
                        ae.Sample(ref LeftSample, ref RightSample);
                }
            }
        }

    }
}
