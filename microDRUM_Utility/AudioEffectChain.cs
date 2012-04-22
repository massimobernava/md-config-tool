using System;
using System.Collections.Generic;
using System.Text;

namespace microDrum
{
    public class AudioEffectChain : IEnumerable<AudioEffect>
    {
        public List<AudioEffect> Effects=new List<AudioEffect>();

        public float SampleRate 
        { 
            set
            {
                foreach(AudioEffect ae in Effects) ae.SampleRate=value;
            }
        }

        public void AddEffect(AudioEffect ae)
        {
            Effects.Add(ae);
        }
        public bool Remove(AudioEffect effect)
        {
            return Effects.Remove(effect);
        }
        
        public bool MoveUp(AudioEffect effect)
        {
            int index = Effects.IndexOf(effect);
            if (index == -1)
            {
                throw new ArgumentException("The specified effect is not part of this effect chain");
            }
            if (index > 0)
            {
                Effects.RemoveAt(index);
                Effects.Insert(index - 1, effect);
                return true;
            }
            return false;
        }

        public bool MoveDown(AudioEffect effect)
        {
            int index = Effects.IndexOf(effect);
            if (index == -1)
            {
                throw new ArgumentException("The specified effect is not part of this effect chain");
            }
            if (index < Effects.Count - 1)
            {
                Effects.RemoveAt(index);
                Effects.Insert(index + 1, effect);
                return true;
            }
            return false;
        }

        IEnumerator<AudioEffect> IEnumerable<AudioEffect>.GetEnumerator()
        {
            return Effects.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Effects.GetEnumerator();
        }
    }
}
