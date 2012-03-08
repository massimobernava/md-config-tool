using System;
using System.Collections.Generic;
using System.Text;

namespace microDrum
{
    public abstract class AudioEffect
    {
        public bool Enabled { get; set; }
        public abstract string Name { get; }
        public List<AudioEffectFactor> Factors = new List<AudioEffectFactor>();
        public float SampleRate { get; set; }
        public float Tempo { get; set; }

        public AudioEffect()
        {
            Enabled = true;
            Tempo = 120;
            SampleRate = 44100;
        }
        public AudioEffectFactor AddFactor(float defaultValue, float minimum, float maximum, float increment, string description)
        {
            AudioEffectFactor factor = new AudioEffectFactor(defaultValue, minimum, maximum, increment, description);
            Factors.Add(factor);
            return factor;
        }
        protected float Factor1 { get { return Factors[0].Value; } }
        protected float Factor2 { get { return Factors[1].Value; } }
        protected float Factor3 { get { return Factors[2].Value; } }
        protected float Factor4 { get { return Factors[3].Value; } }
        protected float Factor5 { get { return Factors[4].Value; } }
        protected float Factor6 { get { return Factors[5].Value; } }
        protected float Factor7 { get { return Factors[6].Value; } }
        protected float Factor8 { get { return Factors[7].Value; } }

        protected float Min(float a, float b) { return Math.Min(a, b); }
        protected float Max(float a, float b) { return Math.Max(a, b); }
        protected float Abs(float a) { return Math.Abs(a); }
        protected float Exp(float a) { return (float)Math.Exp(a); }
        protected float Sqrt(float a) { return (float)Math.Sqrt(a); }
        protected float Sin(float a) { return (float)Math.Sin(a); }
        protected float Tan(float a) { return (float)Math.Tan(a); }
        protected float Cos(float a) { return (float)Math.Cos(a); }
        protected float Pow(float a, float b) { return (float)Math.Pow(a, b); }
        protected float Sign(float a) { return Math.Sign(a); }
        protected float Log(float a) { return (float)Math.Log(a); }
        //protected float PI { get { return (float)Math.PI; } }

        protected const float Db2log = 0.11512925464970228420089957273422f; // ln(10) / 20 
        protected const float PI = 3.1415926535f;
        protected const float HalfPi = 1.57079632675f; // pi / 2;
        protected const float HalfPiScaled = 2.218812643387445f; // halfpi * 1.41254f;

        public abstract void Sample(ref float LeftSample, ref float RightSample);
        public virtual void Init()
        {
        }
        public abstract void OnFactorChanges();
        public virtual void Block(int samplesblock)
        {

        }

    }

    public class EmptyEffect : AudioEffect
    {
        public EmptyEffect()
        {
            AddFactor(0, -50, 50, 1, "1");
            AddFactor(0, -50, 50, 1, "2");
            AddFactor(0, -50, 50, 1, "3");
            AddFactor(0, -50, 50, 1, "4");
            AddFactor(0, -50, 50, 1, "5");
            AddFactor(0, -50, 50, 1, "6");
            AddFactor(0, -50, 50, 1, "7");
            AddFactor(0, -50, 50, 1, "8");
        }

        public override string Name
        {
            get { return "Empty"; }
        }

        public override void Sample(ref float LeftSample, ref float RightSample)
        {

        }
        public override void OnFactorChanges()
        {

        }
    }
    
    public class VolumeEffect : AudioEffect
    {
        public VolumeEffect()
        {
            AddFactor(0, -50, 50, 1, "adjustment (dB)");
            AddFactor(0, -50, 50, 1, "max volume (dB)");
        }

        public override string Name
        {
            get { return "Volume adjustment"; }
        }

        float adj1;
        float adj2;
        float adj1_s;
        float dadj;
        float doseek;

        public override void OnFactorChanges()
        {
            adj1 = Pow(2, (Factor1 / 6));
            adj2 = Pow(2, (Factor2 / 6));
            doseek = 1;
        }

        public override void Block(int samplesblock)
        {
            if (doseek != 0)
            {
                dadj = (adj1 - adj1_s) / samplesblock;
                doseek = 0;
            }
            else
            {
                dadj = 0;
                adj1_s = adj1;
            }
        }

        public override void Sample(ref float spl0, ref float spl1)
        {
            spl0 = Min(Max(spl0 * adj1_s, -adj2), adj2);
            spl1 = Min(Max(spl1 * adj1_s, -adj2), adj2);
            adj1_s += dadj;
        }
    }

    public class ThreeBandEQEffect : AudioEffect
    {
        public ThreeBandEQEffect()
        {
            AddFactor(0, 0, 100, 0.1f, "lo drive%");
            AddFactor(0, -12, 12, 0.1f, "lo gain");
            AddFactor(0, 0, 100, 0.1f, "mid drive%");
            AddFactor(0, -12, 12, 0.1f, "mid gain");
            AddFactor(0, 0, 100, 0.1f, "hi drive%");
            AddFactor(0, -12, 12, 0.1f, "hi gain");
            AddFactor(240, 60, 680, 1, "low-mid freq");
            AddFactor(2400, 720, 12000, 10, "mid-high freq");
        }
        public override string Name
        {
            get { return "3 Band EQ"; }
        }

        float db2log;
        float pi;
        float halfpi;
        float halfpiscaled;


        public override void Init()
        {
            db2log = 0.11512925464970228420089957273422f; // ln(10) / 20 
            pi = 3.1415926535f;
            halfpi = pi / 2;
            halfpiscaled = halfpi * 1.41254f;
        }
        float mixl;
        float mixm;
        float mixh;
        float al;
        float ah;
        float mixl1;
        float mixm1;
        float mixh1;
        float gainl;
        float gainm;
        float gainh;
        float mixlg;
        float mixmg;
        float mixhg;
        float mixlg1;
        float mixmg1;
        float mixhg1;

        public override void Sample(ref float spl0, ref float spl1)
        {
            float dry0 = spl0;
            float dry1 = spl1;

            float lf1h = lfh;
            lfh = dry0 + lfh - ah * lf1h;
            float high_l = dry0 - lfh * ah;

            float lf1l = lfl;
            lfl = dry0 + lfl - al * lf1l;
            float low_l = lfl * al;

            float mid_l = dry0 - low_l - high_l;

            float rf1h = rfh;
            rfh = dry1 + rfh - ah * rf1h;
            float high_r = dry1 - rfh * ah;

            float rf1l = rfl;
            rfl = dry1 + rfl - al * rf1l;
            float low_r = rfl * al;

            float mid_r = dry1 - low_r - high_r;

            float wet0_l = mixlg * Sin(low_l * halfpiscaled);
            float wet0_m = mixmg * Sin(mid_l * halfpiscaled);
            float wet0_h = mixhg * Sin(high_l * halfpiscaled);
            float wet0 = (wet0_l + wet0_m + wet0_h);

            float dry0_l = low_l * mixlg1;
            float dry0_m = mid_l * mixmg1;
            float dry0_h = high_l * mixhg1;
            dry0 = (dry0_l + dry0_m + dry0_h);

            float wet1_l = mixlg * Sin(low_r * halfpiscaled);
            float wet1_m = mixmg * Sin(mid_r * halfpiscaled);
            float wet1_h = mixhg * Sin(high_r * halfpiscaled);
            float wet1 = (wet1_l + wet1_m + wet1_h);

            float dry1_l = low_r * mixlg1;
            float dry1_m = mid_r * mixmg1;
            float dry1_h = high_r * mixhg1;
            dry1 = (dry1_l + dry1_m + dry1_h);

            spl0 = dry0 + wet0;
            spl1 = dry1 + wet1;
        }

        float lfl;
        float lfh;
        float rfh;
        float rfl;

        public override void OnFactorChanges()
        {
            mixl = Factor1 / 100;
            mixm = Factor3 / 100;
            mixh = Factor5 / 100;
            al = Min(Factor7, SampleRate) / SampleRate;
            ah = Max(Min(Factor8, SampleRate) / SampleRate, al);
            mixl1 = 1 - mixl;
            mixm1 = 1 - mixm;
            mixh1 = 1 - mixh;
            gainl = Exp(Factor2 * db2log);
            gainm = Exp(Factor4 * db2log);
            gainh = Exp(Factor6 * db2log);
            mixlg = mixl * gainl;
            mixmg = mixm * gainm;
            mixhg = mixh * gainh;
            mixlg1 = mixl1 * gainl;
            mixmg1 = mixm1 * gainm;
            mixhg1 = mixh1 * gainh;
        }
    }

    public class BadBussMojo : AudioEffect
    {
        public BadBussMojo()
        {
            AddFactor(0, -60, 0, 0.01f, "Pos. Thresh (dB)");
            AddFactor(0, -60, 0, 0.01f, "Neg. Thresh (dB)");
            AddFactor(1, 1, 2, 0.001f, "Pos. Nonlinearity");
            AddFactor(1, 1, 2, 0.001f, "Neg. Nonlinearity");
            AddFactor(0, 0, 6, 0.01f, "Pos. Knee (dB)");
            AddFactor(0, 0, 6, 0.01f, "Neg. Knee (dB)");
            AddFactor(0, 0, 100, 0.1f, "Mod A");
            AddFactor(0, 0, 100, 0.1f, "Mod B");
        }
        public override string Name
        {
            get { return "BadBussMojo"; }
        }

        float log2db;
        float db2log;
        float pi;
        float halfpi;

        public override void Init()
        {
            log2db = 8.6858896380650365530225783783321f; // 20 / ln(10)
            db2log = 0.11512925464970228420089957273422f; // ln(10) / 20 
            pi = 3.1415926535f;
            halfpi = pi / 2;
        }
        float pt;
        float nt;
        float pl;
        float nl;
        float mixa;
        float mixb;
        float drivea;
        float mixa1;
        float drivea1;
        float drivea2;
        float mixb1;
        float pts;
        float nts;
        float ptt;
        float ntt;
        float ptsv;
        float ntsv;
        float drive = 0; // never assigned to

        public override void Sample(ref float spl0, ref float spl1)
        {
            if (mixa > 0)
            {
                wet0 = drivea1 * spl0 * (1 - Abs(spl0 * drivea2));
                wet1 = drivea1 * spl1 * (1 - Abs(spl1 * drivea2));
                spl0 = mixa1 * spl0 + (mixa) * wet0;
                spl1 = mixa1 * spl1 + (mixa) * wet1;
            }

            if (mixb > 0)
            {
                wet0 = Sin(spl0 * halfpi);
                wet1 = Sin(spl1 * halfpi);
                spl0 = mixb1 * spl0 + (mixb) * wet0;
                spl1 = mixb1 * spl1 + (mixb) * wet1;
            }

            float db0 = Log(Abs(spl0)) * log2db;
            float db1 = Log(Abs(spl1)) * log2db;

            if (spl0 > ptsv)
            {
                diff = Max(Min((db0 - ptt), 0), pts);
                if (pts == 0) mult = 0; else mult = diff / pts;
                spl0 = ptsv + ((spl0 - ptsv) / (1 + (pl * mult)));
            }
            if (spl0 < ntsv)
            {
                diff = Max(Min((db0 - ntt), 0), nts);
                if (nts == 0) mult = 0; else mult = diff / nts;
                spl0 = ntsv + ((spl0 - ntsv) / (1 + (nl * mult)));
            }
            if (spl1 > ptsv)
            {
                diff = Max(Min((db1 - ptt), 0), pts);
                if (pts == 0) mult = 0; else mult = diff / pts;
                spl1 = ptsv + ((spl1 - ptsv) / (1 + (pl * mult)));
            }
            if (spl1 < ntsv)
            {
                diff = Max(Min((db1 - ntt), 0), nts);
                if (nts == 0) mult = 0; else mult = diff / nts;
                spl1 = ntsv + ((spl1 - ntsv) / (1 + (nl * mult)));
            }
        }

        float wet0;
        float wet1;
        float diff;
        float mult;

        public override void OnFactorChanges()
        {
            pt = Factor1;
            nt = Factor2;
            pl = Factor3 - 1;
            nl = Factor4 - 1;
            mixa = Factor7 / 100;
            mixb = Factor8 / 100;
            drivea = 1;
            mixa1 = 1 - mixa;
            drivea1 = 1 / (1 - (drivea / 2));
            drivea2 = drive / 2;
            mixb1 = 1 - mixb;
            pts = Factor5;
            nts = Factor6;
            ptt = pt - pts;
            ntt = nt - nts;

            ptsv = Exp(ptt * db2log);
            ntsv = -Exp(ntt * db2log);
        }
    }

    public class PitchDown : AudioEffect
    {
        public PitchDown()
        {
            AddFactor(1, 0, 6, 1, "Octaves Down");
            AddFactor(0, 0, 11, 1, "Semitones Down");
            AddFactor(0, 0, 99, 1, "Cents Down");
            AddFactor(100, 4, 500, 10, "Chunk Size (ms)");
            AddFactor(1, 0.001f, 1, 0.1f, "Overlap Size");
            AddFactor(0, -120, 6, 1, "Wet Mix (dB)");
            AddFactor(-120, -120, 6, 1, "Dry Mix (dB)");
        }
        public override string Name
        {
            get { return "PitchDown"; }
        }

        int bufferSize;
        int bufferPosition;
        float scl;
        float rspos;
        float drymix;
        float wetmix;
        int rrilen;
        float[] buffer = new float[65534];
        float invbs;

        public override void Init()
        {
            
        }
        
        public override void Sample(ref float spl0, ref float spl1)
        {
            //ss0=spl0; ss1=spl1;
            float ss0 = spl0;
            float ss1 = spl1;

            //hbp=((bufpos * scl)|0)*2;
            int hbp = ((int)(bufferPosition * scl)) * 2;

            // pre read these before writing
            //s0r=rrilen[hbp];
            //s1r=rrilen[hbp+1];
            float s0r = buffer[rrilen + hbp];
            float s1r = buffer[rrilen + hbp + 1];

            //(bufpos*2)[0]=spl0;
            //(bufpos*2)[1]=spl1;
            buffer[bufferPosition * 2 + 0] = spl0;
            buffer[bufferPosition * 2 + 1] = spl1;

            //bufpos < rspos ? 
            if (bufferPosition < rspos)
            {
                // mix
                // sc=bufpos*invbs;
                // spl0=hbp[0]*sc + s0r*(1-sc);
                // spl1=hbp[1]*sc + s1r*(1-sc);
                float sc = bufferPosition * invbs;
                spl0 = buffer[hbp + 0] * sc + s0r * (1 - sc);
                spl1 = buffer[hbp + 1] * sc + s1r * (1 - sc);
            }
            else
            {
                // straight resample
                //     spl0=hbp[0]; 
                //     spl1=hbp[1];
                spl0 = buffer[hbp + 0];
                spl1 = buffer[hbp + 1];
            }

            //(bufpos+=1) >= bufsize ? bufpos=0;
            if (++bufferPosition >= bufferSize)
            {
                bufferPosition = 0;
            }

            //spl0 = spl0*wetmix + ss0*drymix;
            //spl1 = spl1*wetmix + ss1*drymix;
            spl0 = spl0 * wetmix + ss0 * drymix;
            spl1 = spl1 * wetmix + ss1 * drymix;
        }

        public override void OnFactorChanges()
        {
            //lbufsize=bufsize; 
            int lbufsize = bufferSize;
            //bufsize=(srate*0.001*slider4)&65534; 
            bufferSize = (int)(SampleRate * 0.001 * Factor4) & 65534;
            //lbufsize!=bufsize ? bufpos=0;
            if (lbufsize != bufferSize)
            {
                bufferPosition = 0;
            }

            //scl=2 ^ (-(slider1 + slider2/12 + slider3/1200)); 
            scl = Pow(2, (-(Factor1 + Factor2 / 12.0f + Factor3 / 1200.0f)));
            //rilen=(max(scl,0.5)*bufsize)|0; 
            int rilen = (int)(Max(scl, 0.5f) * bufferSize);
            //rrilen=((scl*bufsize)|0)*2; 
            rrilen = ((int)(scl * bufferSize)) * 2;
            //slider5=min(1,max(slider5,0)); 
            float overlap = Min(1, Max(Factor5, 0));
            //rspos=slider5*(bufsize-rilen); 
            rspos = overlap * (bufferSize - rilen);
            //invbs=1/rspos;
            invbs = 1.0f / rspos;
            //drymix=2 ^ (slider7/6); 
            drymix = Pow(2, Factor7 / 6);
            //wetmix=2 ^ (slider6/6);
            wetmix = Pow(2, Factor6 / 6);
        }
    }
}
