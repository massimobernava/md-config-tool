using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace microDrum
{
    public partial class VSTForm : Form
    {
        public static VSTForm Singleton = null;
        public static VST vst = null;

        //VstPluginContext pluginContext = null;
        //VSTStream vstStream = null;
        //IWavePlayer playbackDevice = null;
        //EditParametersForm edit = new EditParametersForm();

        //RecordableMixerStream32 mixer=new RecordableMixerStream32();
        //WaveChannel32 mp3Channel = null;

        //long mp3Position = 0;

        //MixerForm mixerForm = null;

        public VSTForm(string VSTPath)
        {
            Singleton = this;
            UtilityAudio.OpenAudio(AudioLibrary.NAudio, null);

            InitializeComponent();
            vst = UtilityAudio.LoadVST(VSTPath, this.Handle);
            this.Text = vst.pluginContext.PluginCommandStub.GetProgramName();
            Rectangle rect = new Rectangle();
            vst.pluginContext.PluginCommandStub.EditorGetRect(out rect);
            this.SetClientSizeCore(rect.Width, rect.Height + 125);
            vst.StreamCall += new EventHandler<VSTStreamEventArgs>(vst_StreamCall);
            
            UtilityAudio.StartAudio();

            /*HostCommandStub hcs = new HostCommandStub();
            hcs.Directory = System.IO.Path.GetDirectoryName(VSTPath);

            try
            {
                pluginContext = VstPluginContext.Create(VSTPath, hcs);
                pluginContext.PluginCommandStub.Open();
                this.Text= pluginContext.PluginCommandStub.GetProgramName();
                //pluginContext.PluginCommandStub.SetProgram(0);
                pluginContext.PluginCommandStub.EditorOpen(this.Handle);
                Rectangle rect = new Rectangle();
                pluginContext.PluginCommandStub.EditorGetRect(out rect);
                this.SetClientSizeCore(rect.Width, rect.Height+125);
                pluginContext.PluginCommandStub.MainsChanged(true);

                vstStream = new VSTStream();
                vstStream.ProcessCalled += new EventHandler<ProcessCalledEventArgs>(vstStream_ProcessCalled);
                vstStream.pluginContext = pluginContext;
                vstStream.SetWaveFormat(44100, 2);

                mixer.AddInputStream(vstStream);
                playbackDevice = new AsioOut(0);//new WaveOut();//
                playbackDevice.Init(mixer);
           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/

        }

        void vst_StreamCall(object sender, VSTStreamEventArgs e)
        {
            waveformPainter1.AddMax(e.MaxL);
            waveformPainter2.AddMax(e.MaxR);
        }

        public new void Dispose()
        {
            /*edit.Close();

            playbackDevice.Stop();
            playbackDevice.Dispose();
            pluginContext.Dispose();
            if(mp3Channel!=null)
                mp3Channel.Dispose();
            if(mixerForm!=null)
                mixerForm.Close();
            */
            UtilityAudio.DisposeVST();
            vst = null;
            base.Dispose();

            Singleton = null;
        }

        /*void vstStream_ProcessCalled(object sender, ProcessCalledEventArgs e)
        {
            waveformPainter1.AddMax(e.MaxL);
            waveformPainter2.AddMax(e.MaxR);
        }*/
        private void VSTForm_Shown(object sender, EventArgs e)
        {
            /*if (Visible)
            {
                try
                {
                    playbackDevice.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }*/
            /*else
            {
                playbackDevice.Stop();

                playbackDevice.Dispose();
                pluginContext.Dispose();
            }*/
        }

        private void VSTForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            /*playbackDevice.Stop();

            playbackDevice.Dispose();
            pluginContext.Dispose();
            mp3Channel.Dispose();

            mixerForm.Close();*/

            Singleton = null;
        }

        internal void ShowEditParameters()
        {
            //edit.AddParameters(pluginContext);
            //edit.Show();
        }

        private void VSTForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        //public void MIDI(byte Note,byte Velocity)
        //{
        //    byte[] midiData = new byte[4];
        //    midiData[0] = 144; // Send note on midi channel 1 
        //    midiData[1] = Note;   // Midi note 
        //    midiData[2] = Velocity; // Note strike velocity 
        //    midiData[3] = 0;    // Reserved, unused 

        //    VstMidiEvent vse = new VstMidiEvent(/*DeltaFrames*/ 0,
        //        /*NoteLength*/ 0,
        //        /*NoteOffset*/  0,
        //         midiData,
        //        /*Detune*/        0,
        //        /*NoteOffVelocity*/ 127);

        //    VstEvent[] ve = new VstEvent[1];
        //    ve[0] = vse;

        //    pluginContext.PluginCommandStub.ProcessEvents(ve);

        //}
        //public void CC(byte Number, byte Value)
        //{
        //    byte[] midiData = new byte[4];
        //    midiData[0] = 176; //0xB0
        //    midiData[1] = Number;
        //    midiData[2] = Value;
        //    midiData[3] = 0;    // Reserved, unused 

        //    VstMidiEvent vse = new VstMidiEvent(/*DeltaFrames*/ 0,
        //        /*NoteLength*/ 0,
        //        /*NoteOffset*/  0,
        //         midiData,
        //        /*Detune*/        0,
        //        /*NoteOffVelocity*/ 127);

        //    VstEvent[] ve = new VstEvent[1];
        //    ve[0] = vse;

        //    pluginContext.PluginCommandStub.ProcessEvents(ve);

        //}

        private void tsbPlay_Click(object sender, EventArgs e)
        {
            UtilityAudio.PlayMP3();
            tsbStop.Image = global::microDrum.Properties.Resources.pause;

        }

        private void tsbLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select MP3 file:";
            fileDialog.Filter = "MP3 Files (*.mp3)|*.mp3";
            fileDialog.ShowDialog();

            if (String.IsNullOrEmpty(fileDialog.FileName)) return;

            UtilityAudio.LoadMP3(fileDialog.FileName);

            tslTotalTime.Text = " / " + UtilityAudio.GetMp3TotalTime().ToString();
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            /*try
            {
                if (mixer.InputCount == 2)
                {
                    mp3Position = mp3Channel.Position;
                    mixer.RemoveInputStream(mp3Channel);
                    tsbStop.Image=global::microDrum.Properties.Resources.stop;
                }
                else
                {
                    mp3Position = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/

            try
            {
                if (UtilityAudio.IsMP3Played())
                {
                    UtilityAudio.PauseMP3();
                    tsbStop.Image = global::microDrum.Properties.Resources.stop;
                }
                else
                {
                    UtilityAudio.StopMp3();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tslNowTime.Text = UtilityAudio.GetMp3CurrentTime().ToString();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (!AboutForm.Singleton.IsValid()) { MessageBox.Show("Unregistred version don't support this!"); return; }

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Select output file:";
            saveFile.Filter = "WAV Files (*.wav)|*.wav";
            saveFile.ShowDialog();

            UtilityAudio.SaveStream(saveFile.FileName);

        }

        private void tsbRec_CheckedChanged(object sender, EventArgs e)
        {
            if (!AboutForm.Singleton.IsValid()) { MessageBox.Show("Unregistred version don't support this!"); return; }
            if (tsbRec.Checked)
            {
                tsbRec.BackColor = Color.Red;
                UtilityAudio.StartStreamingToDisk(); 
                
            }
            else
            {
                tsbRec.BackColor = Color.Transparent;
                UtilityAudio.StopStreamingToDisk();
            }
        }

        private void tsbMixer_Click(object sender, EventArgs e)
        {
            UtilityAudio.ShowMixer();
        }
    }
}
