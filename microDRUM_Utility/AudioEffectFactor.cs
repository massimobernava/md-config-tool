using System;
using System.Collections.Generic;
using System.Text;

namespace microDrum
{
    public class AudioEffectFactor
    {
        public AudioEffectFactor(float defaultValue, float minimum, float maximum, float increment, string description)
        {
            this.Default = defaultValue;
            this.Value = defaultValue;
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.Increment = increment;
            this.Description = description;
        }

        public string Description { get; set; }
        public float Default { get; private set; }
        public float Minimum { get; private set; }
        public float Maximum { get; private set; }
        public float Increment { get; private set; }

        public float Value { get; set; }

    }
}
