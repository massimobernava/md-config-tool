using System;
using System.Collections.Generic;
using System.Text;
using NAudio.Wave;
//using Un4seen.Bass;
//using Un4seen.Bass.Misc;
//using Un4seen.BassAsio;
using NAudio.Midi;
using Jacobi.Vst.Interop.Host;
using System.Windows.Forms;

namespace microDrum
{
    public enum AudioLibrary
    {
        Null,
        NAudio,
        BassNET
    }
    public static class UtilityAudio
    {
        private static AudioLibrary UsedLibrary = AudioLibrary.Null;
        private static SFZ GeneralSFZ = null;//Per adesso uno ma in teoria se ne potrebbero avere anche di più
        private static VST GeneralVST = null;//""
        private static MixerForm mixerForm = null;

        //NAUDIO
        private static List<WaveStream> Samples = new List<WaveStream>();
        private static IWavePlayer playbackDevice = null;
        private static RecordableMixerStream32 Mixer32 = null;

        private static VSTStream vstStream = null;
        private static WaveChannel32 mp3Channel = null;
        private static long mp3Position = 0;
        //private static MidiOut midiOut = null;
        
        //=============================================


        public static bool OpenAudio(AudioLibrary AL,AudioEffectChain General)
        {
            if (UsedLibrary != AudioLibrary.Null ||(playbackDevice != null && playbackDevice.PlaybackState == PlaybackState.Playing)) return false;

            UsedLibrary = AL;

            if (UsedLibrary == AudioLibrary.NAudio)
            {
                //if (midiOut != null) midiOut.Close();

                //NAUDIO
                Mixer32 = new RecordableMixerStream32();
                Mixer32.Chain = General;
                Mixer32.AutoStop = false;
                playbackDevice = new AsioOut(0);//new WaveOut();//
                if (playbackDevice == null) playbackDevice = new WaveOut();
                if (playbackDevice == null) return false;
                playbackDevice.Init(Mixer32);
                

                //=============================================
            }
            return true;
        }
        public static void StartAudio()
        {
            if (playbackDevice != null)  playbackDevice.Play();
        }
        public static void StopAudio()
        {
            if (playbackDevice != null) playbackDevice.Stop();
        }
        
        /*public static void SetMIDI(int device)
        {
            if (midiOut != null) midiOut.Close();
            midiOut = new NAudio.Midi.MidiOut(device);
        }*/
        public static int LoadSample(string SamplePath)
        {
            //Console.WriteLine("load:"+SamplePath);
            try
            {
                if (UsedLibrary == AudioLibrary.NAudio)
                {
                    //NAUDIO
                    WaveFileReader reader = new WaveFileReader(SamplePath);
                    WaveStream Stream = null;
                    //Console.WriteLine("CreatedReader");
                    if (reader.WaveFormat.BitsPerSample == 16) Stream = new WaveChannel16To32(reader);//Volume e Pan da aggiungere
                    else if (reader.WaveFormat.BitsPerSample == 24) Stream = new WaveChannel24To32(reader);
                    else if (reader.WaveFormat.BitsPerSample == 32 && reader.WaveFormat.Encoding == WaveFormatEncoding.Pcm) Stream = new WaveChannel32To32(reader);
                    else if (reader.WaveFormat.BitsPerSample == 32 && reader.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat) { MessageBox.Show("Attenzione"); Stream = new WaveChannel32(new Wave32To16Stream(reader)); }
                    //Console.WriteLine("CreatedChannel");
                    if (Stream != null)
                    {
                        if (Stream is WaveChannel16To32) ((WaveChannel16To32)Stream).Volume = 0.0f;
                        else if (Stream is WaveChannel24To32) ((WaveChannel24To32)Stream).Volume = 0.0f;
                        else if (Stream is WaveChannel32To32) ((WaveChannel32To32)Stream).Volume = 0.0f;
                        else if (Stream is WaveChannel32) ((WaveChannel32)Stream).Volume = 0.0f;
                        //Console.WriteLine("Volume");
                        Samples.Add(Stream);
                        Mixer32.AddInputStream(Stream);
                        //Console.WriteLine("Mixer");
                        return Samples.Count - 1;
                    }
                    //=============================================
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;

        }

        public static void PlaySample(int ID, byte Volume)
        {
            if (UsedLibrary == AudioLibrary.NAudio)
            {
                //NAUDIO
                Samples[ID].Position = 0;
                if (Samples[ID] is WaveChannel16To32) ((WaveChannel16To32)Samples[ID]).Volume = (float)Volume / 127.0f;
                else if (Samples[ID] is WaveChannel24To32) ((WaveChannel24To32)Samples[ID]).Volume = (float)Volume / 127.0f;
                else if (Samples[ID] is WaveChannel32To32) ((WaveChannel32To32)Samples[ID]).Volume = (float)Volume / 127.0f;
                else if (Samples[ID] is WaveChannel32) ((WaveChannel32)Samples[ID]).Volume = (float)Volume / 127.0f;
                //=============================================
            }
        }
        public static void EffectSample(int ID, AudioEffectChain Chain)
        {
            if (UsedLibrary == AudioLibrary.NAudio)
            {
                if (Samples[ID] is WaveChannel16To32) { ((WaveChannel16To32)Samples[ID]).Chain = Chain; if (Chain != null) Chain.SampleRate = ((WaveChannel16To32)Samples[ID]).WaveFormat.SampleRate; }
                else if (Samples[ID] is WaveChannel24To32) { ((WaveChannel24To32)Samples[ID]).Chain = Chain; if(Chain!=null) Chain.SampleRate = ((WaveChannel24To32)Samples[ID]).WaveFormat.SampleRate; }
                else if (Samples[ID] is WaveChannel32To32) { ((WaveChannel32To32)Samples[ID]).Chain = Chain; if (Chain != null) Chain.SampleRate = ((WaveChannel32To32)Samples[ID]).WaveFormat.SampleRate; }
                //else if (Samples[ID] is WaveChannel32) ((WaveChannel32)Samples[ID]).Chain = Chain;
                //=============================================
            }
        }

        public static void Dispose()
        {
            if (mp3Channel != null) mp3Channel.Dispose();
            if (GeneralSFZ != null) GeneralSFZ.Dispose();
            DisposeVST();

            if (UsedLibrary == AudioLibrary.NAudio)
            {
                //NAUDIO
                Samples.Clear();
                if (playbackDevice != null)
                {
                    playbackDevice.Stop();
                    playbackDevice.Dispose();
                    playbackDevice = null;
                }
                if (Mixer32 != null) { Mixer32.Dispose(); Mixer32 = null; }

                
                //=============================================
            }
            UsedLibrary = AudioLibrary.Null;
            //if (midiOut != null) midiOut.Close();
        }

        public static string GetSampleInfo(int ID)
        {
            if (ID > 0 && ID < Samples.Count)
                return Samples[ID].WaveFormat.BitsPerSample + "bit@" + Samples[ID].WaveFormat.SampleRate;
            else return "";
        }

        public static void MIDI_NoteOn(byte Note, byte Velocity)
        {
            //SFZ
            if (GeneralSFZ != null) GeneralSFZ.MIDI_NoteOn(Note, Velocity);

            //VST
            if (GeneralVST != null) GeneralVST.MIDI_NoteOn(Note, Velocity);

            //MIDI
            /*if (midiOut != null)
            {
                NAudio.Midi.NoteOnEvent noteOn = new NAudio.Midi.NoteOnEvent(10000, 10, Note, 100, 100);
                midiOut.Send(noteOn.GetAsShortMessage());
            }*/
        }

        public static void MIDI_CC(byte Number, byte Value)
        {
            //VST
            if (GeneralVST != null) GeneralVST.MIDI_CC(Number, Value);
            //SFZ
            if (GeneralSFZ != null) GeneralSFZ.MIDI_CC(Number, Value);
            //MIDI
        }

        public static SFZ LoadSFZ(string SFZPath)
        {
            try
            {
                if (GeneralSFZ != null) GeneralSFZ.Dispose();
                GeneralSFZ = new SFZ();
                GeneralSFZ.Load(SFZPath);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + ex.Source + ex.StackTrace);
            }

            return GeneralSFZ;
        }

        public static VST LoadVST(string VSTPath, IntPtr hWnd)
        {
            DisposeVST();

            GeneralVST = new VST();

            HostCommandStub hcs = new HostCommandStub();
            hcs.Directory = System.IO.Path.GetDirectoryName(VSTPath);

            try
            {
                GeneralVST.pluginContext = VstPluginContext.Create(VSTPath, hcs);
                GeneralVST.pluginContext.PluginCommandStub.Open();
                //pluginContext.PluginCommandStub.SetProgram(0);
                GeneralVST.pluginContext.PluginCommandStub.EditorOpen(hWnd);
                GeneralVST.pluginContext.PluginCommandStub.MainsChanged(true);

                vstStream = new VSTStream();
                vstStream.ProcessCalled += new EventHandler<VSTStreamEventArgs>(GeneralVST.Stream_ProcessCalled);
                vstStream.pluginContext = GeneralVST.pluginContext;
                vstStream.SetWaveFormat(44100, 2);

                Mixer32.AddInputStream(vstStream);
                //playbackDevice = new AsioOut(0);//new WaveOut();//
                //playbackDevice.Init(mixer);

                return GeneralVST;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        public static void LoadMP3(string MP3Path)
        {
            Mp3FileReader mp3Reader = new Mp3FileReader(MP3Path);
            WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(mp3Reader);
            WaveStream blockAlignedStream = new BlockAlignReductionStream(pcmStream);

            if (mp3Channel != null) mp3Channel.Dispose();
            mp3Channel = new WaveChannel32(blockAlignedStream);
            mp3Position = 0;
            //mp3Channel.Position = mp3Channel.Length;
            mixerForm = new MixerForm(mp3Channel);
        }

        public static void PlayMP3()
        {
            if (mp3Channel == null || Mixer32.ContainsInputStream(mp3Channel)) return;

            Mixer32.AddInputStream(mp3Channel);
            mp3Channel.Position = mp3Position;
        }
        public static void PauseMP3()
        {
            mp3Position = mp3Channel.Position;
            Mixer32.RemoveInputStream(mp3Channel);
        }
        public static void StopMp3()
        {
            mp3Position = 0;
            mp3Channel.CurrentTime = TimeSpan.Zero;
            Mixer32.RemoveInputStream(mp3Channel);
        }
        public static TimeSpan GetMp3TotalTime()
        {
            if (mp3Channel != null) return mp3Channel.TotalTime;
            return TimeSpan.Zero;
        }
        public static TimeSpan GetMp3CurrentTime()
        {
            if (mp3Channel != null) return mp3Channel.CurrentTime;
            return TimeSpan.Zero;
        }

        public static string GetVSTDirectory()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("VST");
            if (key != null) return key.GetValue("VstPluginsPath").ToString();
            else
            {
                key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("XLN Audio\\Addictive Drums");
                if (key != null) return key.GetValue("VSTPath").ToString();
            }
            return null;
        }

        public static void DisposeVST()
        {
            if (Mixer32 != null) Mixer32.RemoveInputStream(vstStream);
            if (vstStream != null) vstStream.Dispose();
            vstStream = null;
            if (GeneralVST != null) GeneralVST.Dispose();
            GeneralVST = null;
        }


        public static bool IsMP3Played()
        {
            return Mixer32.ContainsInputStream(mp3Channel);
        }

        public static void ShowMixer()
        {
            if (mixerForm != null)
                mixerForm.Show();
        }

        public static void SaveStream(string SavePath)
        {
            Mixer32.StreamMixToDisk(SavePath);
        }

        public static void StartStreamingToDisk()
        {
            Mixer32.StartStreamingToDisk();
        }

        public static void StopStreamingToDisk()
        {
            Mixer32.StopStreamingToDisk();
        }

    }
}
