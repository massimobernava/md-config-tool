using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace microDrum
{
    public struct Note
    {
        static private string[] NOTES = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        public byte Value;
        public int NoteValue
        {
            get { return Value % 12; }
        }
        public int OctaveValue
        {
            get { return (Value / 12) - 2; }
        }
        public long Time;
        public bool Hit;
        public long Error;

        public Note(byte value)
        {
            Value = value;
            Time = System.Environment.TickCount;
            Hit = false;
            Error = 0;
        }

        private static string getStringForNote(byte noteValue)
        {
            int note = noteValue % 12;
            int octave = (noteValue / 12) - 2;
            StringBuilder buddy = new StringBuilder();

            return NOTES[note] + octave.ToString();
        }
        public static int getNoteFromString(string N)
        {
            for (int i = NOTES.Length - 1; i >= 0; i--)//Per evitare problemi con i semitoni # e lo StartWith
                if (N.Trim().StartsWith(NOTES[i]))
                {
                    return (24 + 12 * Convert.ToInt32(N.Replace(NOTES[i], "").Trim()) + i);
                }

            return -1;
        }

        public override string ToString()
        {
            if (Value == 127) return "---";
            return getStringForNote(Value) + " (" + Value.ToString() + ")";
        }
    }

    public class SFZGroup : SFZRegion
    {
        public List<SFZRegion> Regions = new List<SFZRegion>();
        public byte seq_counter = 1;

        public override string ToString()
        {
            string IDRegions = "";
            if (Regions.Count != 0) IDRegions = "[" + Regions[0].ID + "-" + Regions[Regions.Count - 1].ID + "]";
            string ret = "Group" + IDRegions + "-(";
            if (lokey != 0) ret += " LK:" + lokey.ToString();
            if (hikey != 127) ret += " HK:" + hikey.ToString();
            if (lovel != 0) ret += " LV:" + lovel.ToString();
            if (hivel != 127) ret += " HV:" + hivel.ToString();
            if (lorand != 0.0f) ret += " LR:" + lorand.ToString("0.00");
            if (hirand != 1.0f) ret += " HR:" + hirand.ToString("0.00");
            if (seq_length != 1) ret += " SL:" + seq_length.ToString();
            if (seq_length != 1) ret += " SP:" + seq_position.ToString();
            for (int i = 0; i < 127; i++)
            {
                if (locc[i] != 0 || hicc[i] != 127)
                    ret += " CC" + i.ToString() + ":[" + locc[i].ToString() + "-" + hicc[i].ToString() + "]";
            }
            ret += ")";
            return ret;
        }
    }
    public class SFZRegion
    {
        public int ID = 0;

        public string SamplePath;
        public int IDEffectChain = 0;

        public byte lokey;
        public byte hikey;
        public byte lovel;
        public byte hivel;
        public float lorand;
        public float hirand;
        public bool cc;
        public byte[] locc = new byte[127];
        public byte[] hicc = new byte[127];
        public byte seq_length;
        public byte seq_position;
        public int group;
        public int off_by;
        public float volume;
        public float pan;

        public SFZGroup Group;

        public SFZRegion()
        {
            SetDefault();
        }

        public SFZRegion(SFZGroup Group)
        {
            SetDefault();
            this.Group = Group;

            /*lokey = Group.lokey;
            hikey = Group.hikey;
            lovel = Group.lovel;
            hivel = Group.hivel;
            lorand = Group.lorand;
            hirand = Group.hirand;*/

            seq_length = Group.seq_length;
            seq_position = Group.seq_position;
        }
        private void SetDefault()
        {
            Group = null;

            ID = -1;

            lokey = 0;
            hikey = 127;
            lovel = 0;
            hivel = 127;
            lorand = 0.0f;
            hirand = 1.0f;

            seq_length = 1;
            seq_position = 1;
            //seq_counter = 1;

            for (int i = 0; i < 127; i++)
            {
                locc[i] = 0;
                hicc[i] = 127;
            }

            cc = true;
        }
        /*public void Play()
        {
            Stream.Position = 0;
        }*/
        public void Play(byte Volume)
        {
            UtilityAudio.PlaySample(ID, Volume);
        }
        /*public int Load(string Path)
        {
            try
            {
                SamplePath = Path;
                WaveFileReader reader = new WaveFileReader(Path);
                Stream = new WaveChannel32(reader);

                return 0;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(Path + ":" + e.Message);
                return 1;
            }
        }*/

        public int GenerateStream()
        {
            string _SamplePath = Path.GetFullPath(SamplePath);

            if (String.IsNullOrEmpty(_SamplePath) || !File.Exists(_SamplePath))
            {
                if (Group != null && Group.SamplePath != null) _SamplePath = Path.GetFullPath(Group.SamplePath);
                else return 1;
            }
            try
            {
                ID = UtilityAudio.LoadSample(_SamplePath);
                return 0;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(_SamplePath + ":" + e.Message);
                return 1;
            }
        }


        public override string ToString()
        {
            string ret = "Region ";
            ret += ID.ToString() + "-(";
            if (lokey != 0) ret += " LK:" + lokey.ToString();
            if (hikey != 127) ret += " HK:" + hikey.ToString();
            if (lovel != 0) ret += " LV:" + lovel.ToString();
            if (hivel != 127) ret += " HV:" + hivel.ToString();
            if (lorand != 0.0f) ret += " LR:" + lorand.ToString("0.00");
            if (hirand != 1.0f) ret += " HR:" + hirand.ToString("0.00");
            if (seq_length != 1) ret += " SL:" + seq_length.ToString();
            if (seq_length != 1) ret += " SP:" + seq_position.ToString();
            ret += ")";
            return ret;
        }
    }

    public class SFZ
    {
        public delegate void SFZUpdatedEventHandler();
        public SFZUpdatedEventHandler SFZUpdated;

        public List<SFZRegion> Regions = new List<SFZRegion>();
        public List<SFZGroup> Groups = new List<SFZGroup>();

        Random random = new Random();

        public void Learn(SFZRegion L)
        {
            LearnRegion = L;
        }
        private SFZRegion LearnRegion = null;


        public int Load(string FilePath)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(FilePath));

            int LastOpcode = 0;//0=Null,1=Group,2=Region
            SFZGroup Group = null;
            SFZRegion Region = null;

            string[] Lines = File.ReadAllLines(FilePath);
            foreach (string line in Lines)
            {
                string[] Opcodes = line.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < Opcodes.Length; i++)
                {
                    if (Opcodes[i].Trim().StartsWith("//")) break;
                    else if (Opcodes[i].Trim() == "<group>")
                    {
                        if (Region != null)
                        {
                            if (Region.GenerateStream() == 0)
                            {
                                if (Group != null) Group.Regions.Add(Region);
                                else Regions.Add(Region);
                            }

                            Region = null;
                        }
                        if (Group != null) Groups.Add(Group);
                        Group = new SFZGroup();
                        LastOpcode = 1;
                    }
                    else if (Opcodes[i].Trim() == "<region>")
                    {
                        if (Region != null)
                        {
                            if (Region.GenerateStream() == 0)
                            {
                                if (Group != null) Group.Regions.Add(Region);
                                else Regions.Add(Region);
                            }
                        }
                        Region = new SFZRegion(Group);
                        LastOpcode = 2;
                    }
                    else
                    {
                        string tmp = Opcodes[i];
                        while (Opcodes.Length > (i + 1) && !Opcodes[i + 1].Contains("=")) tmp += (" " + Opcodes[++i]);
                        Opcodes[i] = tmp;

                        if (Opcodes[i].Trim().StartsWith("sample="))
                        {
                            if (LastOpcode == 1) Group.SamplePath = GetOpcodeValue(Opcodes[i]);
                            else if (LastOpcode == 2) Region.SamplePath = GetOpcodeValue(Opcodes[i]);
                        }
                        else if (Opcodes[i].Trim().StartsWith("key="))
                        {
                            byte key = 0;
                            if (!Byte.TryParse(GetOpcodeValue(Opcodes[i]), out key))
                                key = (byte)Note.getNoteFromString(GetOpcodeValue(Opcodes[i]));

                            /*try
                        {
                            key = Convert.ToByte(GetOpcodeValue(Opcodes[i]));
                        }
                        catch (Exception)
                        {
                            key = (byte)Note.getNoteFromString(GetOpcodeValue(Opcodes[i]));
                        }*/

                            if (LastOpcode == 1) { Group.lokey = Group.hikey = key; }
                            else if (LastOpcode == 2) { Region.lokey = Region.hikey = key; }

                        }
                        else if (Opcodes[i].Trim().StartsWith("lokey="))
                        {
                            byte lokey = 0;
                            if (!Byte.TryParse(GetOpcodeValue(Opcodes[i]), out lokey))
                                lokey = (byte)Note.getNoteFromString(GetOpcodeValue(Opcodes[i]));

                            /*try
                        {
                            lokey = Convert.ToByte(GetOpcodeValue(Opcodes[i]));
                        }
                        catch (Exception)
                        {
                            lokey = (byte)Note.getNoteFromString(GetOpcodeValue(Opcodes[i]));
                        }*/
                            if (LastOpcode == 1) { Group.lokey = lokey; }
                            else if (LastOpcode == 2) { Region.lokey = lokey; }

                        }
                        else if (Opcodes[i].Trim().StartsWith("hikey="))
                        {
                            byte hikey = 0;
                            if (!Byte.TryParse(GetOpcodeValue(Opcodes[i]), out hikey))
                                hikey = (byte)Note.getNoteFromString(GetOpcodeValue(Opcodes[i]));

                            /*try
                            {
                                hikey = Byte.TryParse(Convert.ToByte(GetOpcodeValue(Opcodes[i]));
                            }
                            catch (Exception)
                            {
                                hikey = (byte)Note.getNoteFromString(GetOpcodeValue(Opcodes[i]));
                            }*/
                            if (LastOpcode == 1) { Group.hikey = hikey; }
                            else if (LastOpcode == 2) { Region.hikey = hikey; }

                        }
                        else if (Opcodes[i].Trim().StartsWith("lovel="))
                        {
                            byte lovel = Convert.ToByte(GetOpcodeValue(Opcodes[i]));
                            if (LastOpcode == 1) { Group.lovel = lovel; }
                            else if (LastOpcode == 2) { Region.lovel = lovel; }
                        }
                        else if (Opcodes[i].Trim().StartsWith("hivel="))
                        {
                            byte hivel = Convert.ToByte(GetOpcodeValue(Opcodes[i]));
                            if (LastOpcode == 1) { Group.hivel = hivel; }
                            else if (LastOpcode == 2) { Region.hivel = hivel; }
                        }
                        else if (Opcodes[i].Trim().StartsWith("lorand="))
                        {
                            float lorand = (float)Convert.ToDouble(GetOpcodeValue(Opcodes[i]), System.Globalization.CultureInfo.InvariantCulture);
                            if (LastOpcode == 1) { Group.lorand = lorand; }
                            else if (LastOpcode == 2) { Region.lorand = lorand; }
                        }
                        else if (Opcodes[i].Trim().StartsWith("hirand="))
                        {
                            float hirand = (float)Convert.ToDouble(GetOpcodeValue(Opcodes[i]), System.Globalization.CultureInfo.InvariantCulture);
                            if (LastOpcode == 1) { Group.hirand = hirand; }
                            else if (LastOpcode == 2) { Region.hirand = hirand; }
                        }
                        else if (Opcodes[i].Trim().StartsWith("seq_length="))
                        {
                            byte seq_length = Convert.ToByte(GetOpcodeValue(Opcodes[i]));
                            if (LastOpcode == 1) { Group.seq_length = seq_length; }
                            else if (LastOpcode == 2) { Region.seq_length = seq_length; }
                        }
                        else if (Opcodes[i].Trim().StartsWith("seq_position="))
                        {
                            byte seq_position = Convert.ToByte(GetOpcodeValue(Opcodes[i]));
                            if (LastOpcode == 1) { Group.seq_position = seq_position; }
                            else if (LastOpcode == 2) { Region.seq_position = seq_position; }
                        }
                        else if (Opcodes[i].Trim().StartsWith("locc"))
                        {
                            byte locc = Convert.ToByte(GetOpcodeValue(Opcodes[i]));
                            if (LastOpcode == 1) { Group.locc[Convert.ToByte(GetOpcodeCC(Opcodes[i]))] = locc; }
                            else if (LastOpcode == 2) { Region.locc[Convert.ToByte(GetOpcodeCC(Opcodes[i]))] = locc; }
                        }
                        else if (Opcodes[i].Trim().StartsWith("hicc"))
                        {
                            byte hicc = Convert.ToByte(GetOpcodeValue(Opcodes[i]));
                            if (LastOpcode == 1) { Group.hicc[Convert.ToByte(GetOpcodeCC(Opcodes[i]))] = hicc; }
                            else if (LastOpcode == 2) { Region.hicc[Convert.ToByte(GetOpcodeCC(Opcodes[i]))] = hicc; }
                        }
                        //else Console.WriteLine(Opcodes[i].Trim());
                    }
                }
            }
            if (Region != null)
            {
                if (Region.GenerateStream() == 0)
                {
                    if (Group != null) { Group.Regions.Add(Region); Groups.Add(Group); }
                    else Regions.Add(Region);

                }
            }

            return (0);
        }
        public int Save(string FilePath)
        {

            return (0);
        }
        public void MIDI_NoteOn(byte Note, byte Velocity)
        {
            float Rand = (float)random.NextDouble();

            if (LearnRegion != null)
            {
                LearnRegion.lokey = LearnRegion.hikey = Note;
                LearnRegion = null;
                if (SFZUpdated != null) SFZUpdated();
            }
            foreach (SFZRegion r in Regions)
                if (r.cc &&
                    Note >= r.lokey && Note <= r.hikey &&
                    Velocity >= r.lovel && Velocity <= r.hivel &&
                    Rand >= r.lorand && Rand <= r.hirand)
                {
                    if (r.seq_length + 1 == r.Group.seq_counter)
                    {
                        r.Group.seq_counter = 1;
                    }

                    if (r.Group.seq_counter == r.seq_position) { r.Play(Velocity); r.Group.seq_counter++; }
                }

            foreach (SFZGroup g in Groups)
            {
                if (g.cc &&
                    Note >= g.lokey && Note <= g.hikey &&
                    Velocity >= g.lovel && Velocity <= g.hivel &&
                    Rand >= g.lorand && Rand <= g.hirand)
                {
                    foreach (SFZRegion r in g.Regions)
                        if (r.cc &&
                            Note >= r.lokey && Note <= r.hikey &&
                            Velocity >= r.lovel && Velocity <= r.hivel &&
                            Rand >= r.lorand && Rand <= r.hirand)
                        {
                            if (r.seq_length + 1 == r.Group.seq_counter)
                            {
                                r.Group.seq_counter = 1;
                            }
                            if (r.Group.seq_counter == r.seq_position) { r.Play(Velocity); r.Group.seq_counter++; }
                        }
                }
            }
        }
        public void MIDI_CC(byte Number, byte Value)
        {
            foreach (SFZRegion r in Regions)
                r.cc = (Value >= r.locc[Number] && Value <= r.hicc[Number]);

            foreach (SFZGroup g in Groups)
            {
                g.cc = (Value >= g.locc[Number] && Value <= g.hicc[Number]);
                foreach (SFZRegion r in g.Regions)
                    r.cc = (Value >= r.locc[Number] && Value <= r.hicc[Number]);
            }
        }

        private string GetOpcodeValue(string OpcodeWithValue)
        {
            return OpcodeWithValue.Split('=')[1].Trim();
        }
        private string GetOpcodeCC(string CCOpcode)
        {
            CCOpcode = CCOpcode.Split('=')[0].Trim();
            return CCOpcode.Substring(CCOpcode.LastIndexOf("cc") + 2);
        }
        public void Dispose()
        {
            Regions.Clear();
            Groups.Clear();
        }
    }
}
