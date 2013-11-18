using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO.Ports;
using System.IO;
using System.Threading;

namespace microDrum
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        mDMode Mode = mDMode.Off;
        PinSetting[] Setting = new PinSetting[48];
        byte[] Diagnostic = new byte[48];
        DMap[] DrumMap = null;
        Graphics grap = null;
        Graphics grapPB = null;
        Bitmap grapBitmap = null;
        GeneralSetting gSetting = new GeneralSetting();
        HHSetting hhSetting = new HHSetting();

        VSTForm vstForm = null;
        TabForm tabForm = null;
        AboutForm aboutForm = new AboutForm();
        KeyboardForm keyForm = new KeyboardForm();
        NoteMapForm nmForm = new NoteMapForm();


        TabPage[] TabPages = new TabPage[5];


        private int fHeight, fWidth;
        private Random RandomClass = new Random();
        private FastBitmap fb;

        private Brush rightBrush = null;
        private Brush leftBrush = null;

        SFZ sfz = null;

        public static Dictionary<string, string> LastDirectoryUsed = new Dictionary<string, string>();

        private List<Log> LogRecording = new List<Log>();

        public MainForm()
        {
            InitializeComponent();


            //=================================================
            // NAMED PIPE
            //=================================================
            backgroundWorker1.RunWorkerAsync();
            //=================================================

            TabPages[(int)Tabs.Configuration] = tpStandby;
            TabPages[(int)Tabs.Tools] = tpTools;
            TabPages[(int)Tabs.Monitor] = tpMIDI;
            TabPages[(int)Tabs.Sfz] = tpSFZ;
            TabPages[(int)Tabs.Effects] = tpEffects;

            grapBitmap = new Bitmap(pbMIDI.Width, pbMIDI.Height);
            grapPB = pbMIDI.CreateGraphics();
            grap = Graphics.FromImage(grapBitmap);
            fHeight = grapBitmap.Height;
            fWidth = grapBitmap.Width;

            rightBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((pbMIDI.Height / 2) - 20, 0),
                new Point((pbMIDI.Height / 2) + 200, 0), Color.FromArgb(70, 130, 180), Color.FromArgb(176, 196, 222));
            leftBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((pbMIDI.Height / 2) + 20, 0),
                new Point((pbMIDI.Height / 2) - 200, 0), Color.FromArgb(70, 130, 180), Color.FromArgb(176, 196, 222));

            //XX//UtilitySerial.COMClosed += new EventHandler(COMClosed);
            UtilityMIDI.MIDIClosed += new EventHandler(COMClosed);
            //XX//UtilitySerial.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            UtilityMIDI.MIDIReceived_NoteOn += new UtilityMIDI.MIDIDataReceivedEventHandler(MIDI_NoteOn);
            UtilityMIDI.MIDIReceived_CC += new UtilityMIDI.MIDIDataReceivedEventHandler(MIDI_CC);
            UtilityMIDI.MIDIReceived_SysEx += new UtilityMIDI.MIDIDataReceivedEventHandler(MIDI_SysEx);

            cbTypeHead.Items.Add(PinType.Piezo);
            cbTypeHead.Items.Add(PinType.Switch);
            cbTypeHead.Items.Add(PinType.HHC);
            cbTypeHead.Items.Add(PinType.HH);
            cbTypeHead.Items.Add(PinType.HHs);
            cbTypeHead.Items.Add(PinType.YSwitch);
            cbTypeHead.Items.Add(PinType.Disabled);

            cbTypeHHC.Items.Add(PinType.Piezo);
            cbTypeHHC.Items.Add(PinType.Switch);
            cbTypeHHC.Items.Add(PinType.HHC);
            cbTypeHHC.Items.Add(PinType.HH);
            cbTypeHHC.Items.Add(PinType.HHs);
            cbTypeHHC.Items.Add(PinType.YSwitch);
            cbTypeHHC.Items.Add(PinType.Disabled);

            cbTypeRim.Items.Add(PinType.Piezo);
            cbTypeRim.Items.Add(PinType.Switch);
            cbTypeRim.Items.Add(PinType.HHs);
            cbTypeRim.Items.Add(PinType.YSwitch);
            cbTypeRim.Items.Add(PinType.Disabled);

            gbHHCtrl.Location = gbHead.Location;
            gbHHE.Location = gbDualPP.Location;
            gbDualPS.Location = gbDualPP.Location;

            tscCOM.Items.AddRange(UtilitySerial.GetPortNames());
            if (UtilityMIDI.GetMIDIDevices(MIDIType.MIDI_IN) != null)
                tscMIDIIN.Items.AddRange(UtilityMIDI.GetMIDIDevices(MIDIType.MIDI_IN));
            if (UtilityMIDI.GetMIDIDevices(MIDIType.MIDI_OUT) != null)
                tscMIDIOUT.Items.AddRange(UtilityMIDI.GetMIDIDevices(MIDIType.MIDI_OUT));

            LoadSetting(Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");

            //Controlliamo se c'è una porta di default aperta
            string COM = UtilityIniFile.GetIniString("Setup", "COM", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
            //se c'è apriamola
            tscCOM.SelectedIndex = -1;
            tscCOM.Text = "Select COM...";
            selectComToolStripMenuItem.Checked = false;
            if (!String.IsNullOrEmpty(COM))
            {
                for (int i = 0; i < tscCOM.Items.Count; i++)
                    if (tscCOM.Items[i].ToString().ToUpper() == COM.ToUpper())
                    {
                        selectComToolStripMenuItem.Checked = true;
                        tscCOM.SelectedIndex = i;
                        break;
                    }
            }
            string MIDIIN = UtilityIniFile.GetIniString("Setup", "MIDI_IN", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
            tscMIDIIN.SelectedIndex = -1;
            tscMIDIIN.Text = "Select MIDI IN...";
            mIDIINToolStripMenuItem.Checked = false;
            if (!String.IsNullOrEmpty(MIDIIN))
            {
                for (int i = 0; i < tscMIDIIN.Items.Count; i++)
                    if (tscMIDIIN.Items[i].ToString().ToUpper() == MIDIIN.ToUpper())
                    {
                        mIDIINToolStripMenuItem.Checked = true;
                        tscMIDIIN.SelectedIndex = i;
                        break;
                    }
            }
            string MIDIOUT = UtilityIniFile.GetIniString("Setup", "MIDI_OUT", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
            tscMIDIOUT.SelectedIndex = -1;
            tscMIDIOUT.Text = "Select MIDI OUT...";
            mIDIOUTToolStripMenuItem.Checked = false;
            if (!String.IsNullOrEmpty(MIDIOUT))
            {
                for (int i = 0; i < tscMIDIOUT.Items.Count; i++)
                    if (tscMIDIOUT.Items[i].ToString().ToUpper() == MIDIOUT.ToUpper())
                    {
                        mIDIOUTToolStripMenuItem.Checked = true;
                        tscMIDIOUT.SelectedIndex = i;
                        break;
                    }
            }
            for (byte n = 95; n > 0; n--)
            {
                //dudNoteHead.Items.Add(new Note(n));
                //dudNoteRim.Items.Add(new Note(n));
                dudChokeHead.Items.Add(new Note(n));
                dudChokeRim.Items.Add(new Note(n));
                dudDualA.Items.Add(new Note(n));
                dudDualB.Items.Add(new Note(n));
                dudDualC.Items.Add(new Note(n));
                dudDualD.Items.Add(new Note(n));

                dudHH_A.Items.Add(new Note(n));
                dudHH_B.Items.Add(new Note(n));
                dudHH_C.Items.Add(new Note(n));
                dudHH_D.Items.Add(new Note(n));

                dudHH_FootCloseNote.Items.Add(new Note(n));
                dudHH_FootSplashNote.Items.Add(new Note(n));

                dudAlternativeNote.Items.Add(new Note(n));
                dudOpenNoteRim.Items.Add(new Note(n));
                dudOpenNoteHead.Items.Add(new Note(n));
            }
            dudChokeHead.Items.Add(new Note(127));
            dudChokeRim.Items.Add(new Note(127));

            /*dudNoteHead.Items.Reverse();
            dudNoteRim.Items.Reverse();
            dudChokeHead.Items.Reverse();
            dudChokeRim.Items.Reverse();
            dudDualA.Items.Reverse();
            dudDualB.Items.Reverse();
            dudDualC.Items.Reverse();
            dudDualD.Items.Reverse();*/

            if (lbPads.Items.Count > 0)
                lbPads.SelectedIndex = 0;

            /*zedLog.GraphPane.AddCurve("Log1", log1, Color.Red);
            zedLog.GraphPane.AddCurve("Log2", log2, Color.Blue);
            zedLog.GraphPane.YAxis.Scale.Max = 255;*/

            configurationToolStripMenuItem.Checked = UtilityIniFile.GetIniString("Setup", "Configuration", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini") == "Show";
            toolToolStripMenuItem.Checked = UtilityIniFile.GetIniString("Setup", "Tool", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini") == "Show";
            monitorToolStripMenuItem.Checked = UtilityIniFile.GetIniString("Setup", "Monitor", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini") == "Show";
            sFZToolStripMenuItem.Checked = UtilityIniFile.GetIniString("Setup", "SFZ", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini") == "Show";
            effectsToolStripMenuItem.Checked = UtilityIniFile.GetIniString("Setup", "Effects", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini") == "Show";

            nudNSensor.Enabled = btnNSensor.Enabled = AboutForm.Singleton.IsValid();

            string[] LastDir = UtilityIniFile.GetKeys("LastDirectory", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
            foreach (string ld in LastDir)
                LastDirectoryUsed.Add(ld, UtilityIniFile.GetIniString("LastDirectory", ld, Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini"));

            //tcMain.Enabled = false;
        }
        void MIDI_NoteOn(UtilityMIDI.MIDIDataReceivedEventArgs e)
        {
            if (Mode == mDMode.MIDI)
            {
                if (chkEnableMonitor.Checked) UpdateMIDI(e.Cmd, e.Data1, e.Data2);
                if (TabForm.Singleton != null) tabForm.MIDI(e.Data1, e.Data2);
            }
        }
        public void MIDI_NoteOn(byte Note, byte Velocity)
        {
            if (Mode == mDMode.MIDI)
            {
                if (chkEnableMonitor.Checked) UpdateMIDI(0x99, Note, Velocity);
                if (TabForm.Singleton != null) tabForm.MIDI(Note, Velocity);
            }
        }
        void MIDI_CC(UtilityMIDI.MIDIDataReceivedEventArgs e)
        {
            if (Mode == mDMode.MIDI)
            {
                if (chkEnableMonitor.Checked) UpdateMIDI(e.Cmd, e.Data1, e.Data2);
            }
        }

        void MIDI_SysEx(UtilityMIDI.MIDIDataReceivedEventArgs e)
        {
            if (e.Cmd == 0x60) //License
            {
                byte[] lic = new byte[] { (byte)e.Data1, (byte)e.Data2 };
                if (aboutForm.IsValid())
                    UtilityMIDI.MIDI_SysEx(0x60, (byte)e.Data1, (byte)e.Data2, UtilityCryptography.GetPearsonHash(lic));
                else
                    UtilityMIDI.MIDI_SysEx(0x60, (byte)e.Data1, (byte)e.Data2, 0x00);
            }
            if (Mode == mDMode.Off)
            {
                OffMode(e.Cmd, e.Data1, e.Data2, e.Data3);
            }
            else if (Mode == mDMode.Tool)
            {
                LogMode(e.Cmd, e.Data1, e.Data2, e.Data3, e.Log);
            }
            else if (Mode == mDMode.StandBy)
            {
                StandbyMode(e.Cmd, e.Data1, e.Data2, e.Data3);
            }
        }

        #region Setting
        private void LoadSetting(string SettingPath)
        {
            //MessageBox.Show(SettingPath);
            if (!File.Exists(SettingPath)) { return; }

            gSetting.DelayTime = Convert.ToByte(UtilityIniFile.GetIniString("General", "Delay", SettingPath));
            //gSetting.HHC1 = Convert.ToByte(UtilityIniFile.GetIniString("General", "HHC1", SettingPath));
            gSetting.NSensor = Convert.ToByte(UtilityIniFile.GetIniString("General", "NSensor", SettingPath));
            gSetting.Xtalk = Convert.ToByte(UtilityIniFile.GetIniString("General", "Xtalk", SettingPath));

            //LoadPinSetting
            for (int i = 0; i < Setting.Length; i++)
            {
                string tmp = UtilityIniFile.GetIniString("PinSetting", "Pin" + i.ToString(), SettingPath);
                if (!String.IsNullOrEmpty(tmp))
                    Setting[i].FromString(tmp);
            }

            //Load DrumMap
            string[] Pads = UtilityIniFile.GetKeys("DrumMap", SettingPath);
            lbPads.Items.Clear();
            cbHH_HHC.Items.Clear();
            DrumMap = new DMap[Pads.Length];
            for (int i = 0; i < Pads.Length; i++)
            {
                string tmp = UtilityIniFile.GetIniString("DrumMap", Pads[i], SettingPath);
                if (!String.IsNullOrEmpty(tmp))
                {
                    string[] split = tmp.Split(';');
                    DrumMap[i].Name = Pads[i];
                    lbPads.Items.Add(Pads[i]);

                    DrumMap[i].Head = Convert.ToByte(split[0]);
                    DrumMap[i].Rim = Convert.ToByte(split[1]);
                    DrumMap[i].Single = (split[2] == "Y");

                    if (Setting[DrumMap[i].Head].Type == PinType.HHC)
                        cbHH_HHC.Items.Add(Pads[i]);
                }
            }
            string[] A = UtilityIniFile.GetIniString("HH", "A", SettingPath).Split(';');
            hhSetting.A_Note = Convert.ToByte(A[0]);
            hhSetting.A_Thresold = Convert.ToByte(A[1]);
            string[] B = UtilityIniFile.GetIniString("HH", "B", SettingPath).Split(';');
            hhSetting.B_Note = Convert.ToByte(B[0]);
            hhSetting.B_Thresold = Convert.ToByte(B[1]);
            string[] C = UtilityIniFile.GetIniString("HH", "C", SettingPath).Split(';');
            hhSetting.C_Note = Convert.ToByte(C[0]);
            hhSetting.C_Thresold = Convert.ToByte(C[1]);
            string[] D = UtilityIniFile.GetIniString("HH", "D", SettingPath).Split(';');
            hhSetting.D_Note = Convert.ToByte(D[0]);
            hhSetting.D_Thresold = Convert.ToByte(D[1]);
            string[] FootClose = UtilityIniFile.GetIniString("HH", "FootClose", SettingPath).Split(';');
            hhSetting.FootCloseNote = Convert.ToByte(FootClose[0]);
            hhSetting.FootCloseThresold = Convert.ToByte(FootClose[1]);
            string[] FootSplash = UtilityIniFile.GetIniString("HH", "FootSplash", SettingPath).Split(';');
            hhSetting.FootSplashNote = Convert.ToByte(FootSplash[0]);
            hhSetting.FootSplashThresold = Convert.ToByte(FootSplash[1]);
        }
        private void SaveSetting(string SettingPath)
        {
            UtilityIniFile.SetIniString("General", "Delay", gSetting.DelayTime.ToString(), SettingPath);
            //UtilityIniFile.SetIniString("General", "HHC1", gSetting.HHC1.ToString(), SettingPath);
            UtilityIniFile.SetIniString("General", "NSensor", gSetting.NSensor.ToString(), SettingPath);
            UtilityIniFile.SetIniString("General", "Xtalk", gSetting.Xtalk.ToString(), SettingPath);


            UtilityIniFile.SetIniString("HH", "A", hhSetting.A_Note.ToString() + ";" + hhSetting.A_Thresold.ToString(), SettingPath);
            UtilityIniFile.SetIniString("HH", "B", hhSetting.B_Note.ToString() + ";" + hhSetting.B_Thresold.ToString(), SettingPath);
            UtilityIniFile.SetIniString("HH", "C", hhSetting.C_Note.ToString() + ";" + hhSetting.C_Thresold.ToString(), SettingPath);
            UtilityIniFile.SetIniString("HH", "D", hhSetting.D_Note.ToString() + ";" + hhSetting.D_Thresold.ToString(), SettingPath);

            UtilityIniFile.SetIniString("HH", "FootClose", hhSetting.FootCloseNote.ToString() + ";" + hhSetting.FootCloseThresold.ToString(), SettingPath);
            UtilityIniFile.SetIniString("HH", "FootSplash", hhSetting.FootSplashNote.ToString() + ";" + hhSetting.FootSplashThresold.ToString(), SettingPath);


            UtilityIniFile.DeleteSection("DrumMap", SettingPath);
            for (int i = 0; i < DrumMap.Length; i++)
            {
                UtilityIniFile.SetIniString("DrumMap", DrumMap[i].Name, DrumMap[i].Head.ToString() + ";" + DrumMap[i].Rim.ToString() + ";" + (DrumMap[i].Single ? "Y" : "N"), SettingPath);
            }
            for (int i = 0; i < Setting.Length; i++)
            {
                UtilityIniFile.SetIniString("PinSetting", "Pin" + i.ToString(), Setting[i].ToString(), SettingPath);
            }

            foreach (string k in LastDirectoryUsed.Keys)
                UtilityIniFile.SetIniString("LastDirectory", k, LastDirectoryUsed[k], SettingPath);

        }
        #endregion

        #region COM
        /*void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (Mode == mDMode.Off)
            {
                OffMode();
            }
            else if (Mode == mDMode.Tool)
            {
                LogMode();
            }
            else if (Mode == mDMode.StandBy)
            {
                StandbyMode();
            }
            else if (Mode == mDMode.MIDI)
            {
                MidiMode();
            }
        }*/
        void COMClosed(object sender, EventArgs e)
        {
            Mode = mDMode.Off;
            tcMain.Enabled = false;
        }
        #endregion

        #region Mode

        //Cmd=0x00
        private void AskState()
        {
            //domandiamo alla microDRUM in che stato si trova
            UtilityMIDI.MIDI_SysEx(0x00, 0x00, 0x00, 0x00);

        }
        //Cmd=0x01
        private void SetState(mDMode State)
        {
            byte SendState = 0x00;
            switch (State)
            {
                case mDMode.Off: SendState = 0x00; break;
                case mDMode.StandBy: SendState = 0x01; break;
                case mDMode.MIDI: SendState = 0x02; break;
                case mDMode.Tool: SendState = 0x03; break;
            }
            UtilityMIDI.MIDI_SysEx(0x01, SendState, 0x00, 0x00);
            //serialPort.Write(new byte[] { 0xF0, 0x77, 0x01, SendState,0x00,0x00, 0xF7 }, 0, 7);
        }
        void OffMode(byte Cmd, byte Data1, byte Data2, byte Data3)
        {
            //Cmd=0x00 AskState
            if (Cmd == 0x00)
            {
                //Off
                if (Data1 == 0x00)
                {
                    // Mode = 0;
                    //MessageBox.Show("OK");

                    //Se la risposta della microDRUM è corretta
                    //Passa in standby
                    /*SetState(mDMode.StandBy);
                    //imposta la tab StandBy
                    tcMain.Enabled = true;
                    UpdateTab(Tabs.Configuration);
                    UpdateMenu(true);
                    tcMain.Enabled = false;*/
                }
                else if (Data1 == 0x01)//StandBy
                {
                    //imposta la tab StandBy
                    UpdateTab(Tabs.Configuration);
                    UpdateMenu(true);
                }
                else if (Data1 == 0x02)//MIDI
                {
                    //imposta la tab MIDI
                    UpdateTab(Tabs.Monitor);
                    UpdateMenu(false);
                }
                else if (Data1 == 0x03)//Log
                {
                    //imposta la tab Tools
                    UpdateTab(Tabs.Tools);
                    UpdateMenu(true);
                }
            }

                /*if (aboutForm.IsValid())
                    UtilityMIDI.MIDI_SysEx(0x60, (byte)Data1, (byte)Data2, UtilityCryptography.GetPearsonHash(lic));
                else
                {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    UtilityMIDI.MIDI_SysEx(0x60, (byte)rand.Next(127), 0x00, 0x00);
                }*/

        }
        void LogMode(byte Cmd, byte Data1, byte Data2, byte Data3, Log Log)
        {
            /*while (UtilitySerial.BytesToRead > 0)
            {
                byte Start = (byte)UtilitySerial.ReadByte();
                byte ID = (byte)UtilitySerial.ReadByte();
                byte Cmd = (byte)UtilitySerial.ReadByte();*/

            if (Cmd == 0x61) //EEPROM
            {
                /*byte Data1 = (byte)UtilitySerial.ReadByte();
                byte Data2 = (byte)UtilitySerial.ReadByte();
                byte Data3 = (byte)UtilitySerial.ReadByte();
                byte End = (byte)UtilitySerial.ReadByte();*/

                UpdateEEPROM((Data1 * 256) + Data2, Data3);
            }
            else if (Cmd == 0x6E) //LOG
            {
                /* byte Size = (byte)UtilitySerial.ReadByte();
                 byte[] Message = new byte[Size];
                 for (int i = 0; i < Size; i++)
                     Message[i] = (byte)UtilitySerial.ReadByte();
                 byte End = (byte)UtilitySerial.ReadByte();
                 Log log = new Log();
                 log.Time = (uint)(Message[0] | (Message[1] << 8) | (Message[2] << 16) | (Message[3] << 24));
                 log.Sensor = Message[4];
                 log.N = Message[5] | (Message[6] << 8);
                 log.Reading = Message[7] | (Message[8] << 8);
                 log.Y0 = Message[9] | (Message[10] << 8);
                 log.MaxReading = Message[11] | (Message[12] << 8);
                 log.State = Message[13];*/
                if (chkRecordLog.Checked)
                    LogRecording.Add(Log);

                UpdateLog(Log);
            }
            else if (Cmd == 0x6F) //DIAGNOSTIC
            {
                /*byte Data1 = (byte)UtilitySerial.ReadByte();
                byte Data2 = (byte)UtilitySerial.ReadByte();
                byte Data3 = (byte)UtilitySerial.ReadByte();
                byte End = (byte)UtilitySerial.ReadByte();*/

                UpdateDiagnostic(Data1, Data2);
            }
            //}
        }

        private void UpdateEEPROM(int Address, byte Value)
        {
            txtEEPROM.Invoke(new EventHandler(delegate
            {
                txtEEPROM.Text += Address.ToString() + "=" + Value.ToString() + ",";
            }));
        }

        void StandbyMode(byte Cmd, byte Data1, byte Data2, byte Data3)
        {
            /*while (UtilitySerial.BytesToRead > 0)
            {
                byte Start = (byte)UtilitySerial.ReadByte();
                if (Start != 0xF0) continue; //Forziamo la seriale a svuotarsi finchè non trova l'inizio di un sysex

                byte ID = (byte)UtilitySerial.ReadByte();
                byte Cmd = (byte)UtilitySerial.ReadByte();
                byte Data1 = (byte)UtilitySerial.ReadByte();
                byte Data2 = (byte)UtilitySerial.ReadByte();
                byte Data3 = (byte)UtilitySerial.ReadByte();
                byte End = (byte)UtilitySerial.ReadByte();

                if (Start == 0xF0 && ID == 0x77 && End == 0xF7)
                {*/
            if (Cmd == 0x02)//AskParam
            {
                if (Data1 == 0x7E)//General
                {
                    switch (Data2)
                    {
                        case 0x00: //Delay
                            gSetting.DelayTime = Data3;
                            break;
                        /*case 0x01: //HHC1
                            gSetting.HHC1 = Data3;
                            break;*/
                        case 0x02: //NSensor
                            gSetting.NSensor = (byte)(Data3 * 8);
                            break;
                        case 0x03: //Xtalk
                            gSetting.Xtalk = Data3;
                            break;
                    }
                }
                else if (Data1 == 0x4C)//HH
                {

                    switch (Data2)
                    {
                        case 0x00: hhSetting.A_Note = Data3; break;//NOTE A
                        case 0x01: hhSetting.B_Note = Data3; break;//NOTE B
                        case 0x02: hhSetting.C_Note = Data3; break;//NOTE C
                        case 0x03: hhSetting.D_Note = Data3; break;//NOTE D
                        case 0x04: hhSetting.A_Thresold = Data3; break;//THRESOLD A
                        case 0x05: hhSetting.B_Thresold = Data3; break;//THRESOLD B
                        case 0x06: hhSetting.C_Thresold = Data3; break;//THRESOLD C
                        case 0x07: hhSetting.D_Thresold = Data3; break;//THRESOLD D
                        case 0x08: hhSetting.FootSplashNote = Data3; break;//FOOTSPLASH NOTE
                        case 0x09: hhSetting.FootCloseNote = Data3; break;//FOOTCLOSE NOTE
                        case 0x0A: hhSetting.FootSplashThresold = Data3; break;//FOOTSPLASH THRESOLD
                        case 0x0B: hhSetting.FootCloseThresold = Data3; break;//FOOTCLOSE THRESOLD
                    }
                }
                else if (Data1 == 0x7F)
                {
                    if (Data2 == 0x7F && Data3 == 0x7F)
                        UpdateProgressBar(true);
                }
                else
                {
                    switch (Data2)
                    {
                        case 0x00: //Note
                            Setting[Data1].Note = Data3;
                            break;
                        case 0x01: //Thresold
                            Setting[Data1].Thresold = Data3;
                            break;
                        case 0x02: //ScanTime
                            Setting[Data1].ScanTime = Data3;
                            break;
                        case 0x03: //MaskTime
                            Setting[Data1].MaskTime = Data3;
                            break;
                        case 0x04: //Retrigger
                            Setting[Data1].Retrigger = Data3;
                            break;
                        case 0x05: //Curve
                            Setting[Data1].Curve = Data3;
                            break;
                        case 0x06: //Xtalk
                            Setting[Data1].Xtalk = Data3;
                            break;
                        case 0x07: //XtalkGroup
                            Setting[Data1].XtalkGroup = Data3;
                            break;
                        case 0x08: //CurveForm
                            Setting[Data1].CurveForm = Data3;
                            break;
                        case 0x09: //Choke
                            Setting[Data1].Choke = Data3;
                            break;
                        case 0x0A: //Dual
                            Setting[Data1].Dual = Data3;
                            break;
                        case 0x0B: //DualNote
                            Setting[Data1].DualNote = Data3;
                            break;
                        case 0x0C: //DualThresold
                            Setting[Data1].DualThresold = Data3;
                            break;
                        case 0x0D: //Type
                            Setting[Data1].Type = (PinType)Data3;
                            break;
                        case 0x0E: //Channel
                            Setting[Data1].Channel = Data3;
                            break;
                    }
                }
                UpdateProgressBar(false);
            }
           /* else if (Cmd == 0x60) //License
            {
                byte[] lic = new byte[] { (byte)Data1, (byte)Data2, (byte)Data3 };
                if (aboutForm.IsValid())
                    UtilityMIDI.MIDI_SysEx(0x60, UtilityCryptography.GetPearsonHash(lic), 0x00, 0x00);
                else
                {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    UtilityMIDI.MIDI_SysEx(0x60, (byte)rand.Next(127), 0x00, 0x00);
                }
            }*/
            //      }
            //  }
        }
        /*void MidiMode()
        {
            while (UtilitySerial.BytesToRead > 0)
            {
                try
                {
                    int Cmd = UtilitySerial.ReadByte();
                    if (Cmd != 0x99 && Cmd != 0xB9) continue; //Forziamo la seriale a svuotarsi finchè non trova l'inizio di una nota

                    byte Data1 = (byte)UtilitySerial.ReadByte();
                    byte Data2 = (byte)UtilitySerial.ReadByte();

                    UpdateMIDI(Cmd, Data1, Data2);
                }
                catch (IOException)
                {
                    MessageBox.Show("Reset???");
                }

            }
        }*/
        #endregion

        #region Update
        private void UpdateMenu(bool Enable)
        {
            menuStrip1.Invoke(new EventHandler(delegate
            {
                sendToMicroDrumToolStripMenuItem.Enabled = Enable;
                loadFromMicroDrumToolStripMenuItem.Enabled = Enable;

            }));
        }
        private Brush GetBrush(int Value, bool Dual)
        {
            if (Dual) return Brushes.White;// return new System.Drawing.Drawing2D.LinearGradientBrush(new Point((pbMIDI.Height / 2) - 20, 0), new Point((pbMIDI.Height / 2) + Value - 20, 0), Color.Black, Color.Red); ;

            if (Value < 16) return new SolidBrush(Color.FromArgb(70, 130, 180));
            else if (Value < 32) return new SolidBrush(Color.FromArgb(85, 139, 186));
            else if (Value < 48) return new SolidBrush(Color.FromArgb(100, 148, 192));
            else if (Value < 64) return new SolidBrush(Color.FromArgb(115, 158, 198));
            else if (Value < 80) return new SolidBrush(Color.FromArgb(130, 167, 204));
            else if (Value < 96) return new SolidBrush(Color.FromArgb(145, 177, 210));
            else if (Value < 112) return new SolidBrush(Color.FromArgb(160, 186, 216));
            else return new SolidBrush(Color.FromArgb(176, 196, 222));

            //return new System.Drawing.Drawing2D.LinearGradientBrush(new Point((pbMIDI.Height / 2) - 20, 0), new Point((pbMIDI.Height / 2) + Value - 20, 0), Color.FromArgb(70, 130, 180), Color.FromArgb(176, 196, 222));
        }
        private void Draw(int Data1, int Data2)
        {
            for (int i = 0; i < Setting.Length; i++)
                if (Setting[i].Note == Data1 || Setting[i].DualNote == Data1)
                {
                    for (int j = 0; j < DrumMap.Length; j++)
                    {
                        if (DrumMap[j].Head == i)
                        {
                            grap.FillRectangle(rightBrush, pbMIDI.Width / 2, 20 + j * 10, Data2, 10);
                            grap.DrawString((string)lbPads.Items[j] + ((Setting[i].Type == PinType.HHC) ? "" : " Head"), this.Font, Brushes.White, pbMIDI.Width - 20 - grap.MeasureString((string)lbPads.Items[j] + " Head", this.Font).Width, 20 + j * 10);
                        }
                        else if (!DrumMap[j].Single && DrumMap[j].Rim == i)
                        {
                            grap.FillRectangle(leftBrush, pbMIDI.Width / 2 - Data2, 20 + j * 10, Data2, 10);
                            grap.DrawString((string)lbPads.Items[j] + " Rim", this.Font, Brushes.White, 20, 20 + j * 10);
                        }
                    }
                    //break;
                }

        }
        internal void UpdateMIDI(int Cmd, byte Data1, byte Data2)
        {
            lbMIDI.Invoke(new EventHandler(delegate
                {

                    Draw(Data1, Data2);
                    lbMIDI.Items.Add(new MIDI(Cmd, Data1, Data2));
                    lbMIDI.TopIndex = lbMIDI.Items.Count - 20;
                }));
            //txtData.Invoke(new EventHandler(delegate { txtData.Text += Data+ "\r\n"; txtData.SelectionStart = txtData.Text.Length; txtData.ScrollToCaret(); }));
        }
        private void UpdateTab(Tabs TabPage)
        {
            try
            {
                tcMain.Invoke(new EventHandler(delegate { tcMain.SelectTab(TabPages[(int)TabPage]); }));
            }
            catch
            {

            }
        }
        private void UpdateProgressBar(bool End)
        {
            pbLoad.Invoke(new EventHandler(delegate
                {
                    if (End)
                    {
                        pbLoad.Value = pbLoad.Maximum;
                        tcMain.Enabled = true;
                        UpdateSetting();
                    }

                    if (pbLoad.Value < pbLoad.Maximum)
                        pbLoad.Value++;
                }));
        }
        private void UpdateLog(Log Log)
        {
            lbLog.Invoke(new EventHandler(delegate
            {
                lbLog.Items.Add(Log);
                lbLog.TopIndex = lbLog.Items.Count - 20;
                pbCurve.Refresh();
                float Zoom = chkZoom.Checked ? 5.0f : 1.0f;
                wfpLog.AddMax((float)Log.Reading * Zoom / 1024.0f);
            }));
        }
        private void UpdateDiagnostic(byte Pin, byte Velocity)
        {
            if (Pin > Diagnostic.Length) return;

            Diagnostic[Pin] = (byte)((Diagnostic[Pin] + Velocity) / 2);
            pbDiagnostic.Invoke(new EventHandler(delegate
            {
                pbDiagnostic.Refresh();
            }));
        }
        private void UpdatePiezo(bool Head, PinSetting PS)
        {
            string HR = Head ? "Head" : "Rim";
            GroupBox GB = Head ? gbHead : gbRim;

            GB.Controls["lbl" + HR + "Note"].Text = "Note:";
            GB.Controls["lbl" + HR + "ScanTime"].Text = "Scan Time:";
            GB.Controls["lbl" + HR + "MaskTime"].Text = "Mask Time:";
            GB.Controls["lblOpenThresold" + HR].Visible = false;
            GB.Controls["lblOpenNote" + HR].Visible = false;
            GB.Controls["nudOpenThresold" + HR].Visible = false;
            GB.Controls["dudOpenNote" + HR].Visible = false;
            GB.Controls["btnOpenThresold" + HR].Visible = false;
            GB.Controls["btnOpenNote" + HR].Visible = false;
            GB.Controls["lblChoke" + HR].Text="Numb:";
            GB.Controls["lblChoke" + HR].Visible = true;
            GB.Controls["dudChoke" + HR].Visible = false;
            GB.Controls["nudSensibility" + HR].Visible = true;
            GB.Controls["btnChoke" + HR].Visible = true;

            //Dual Piezo-Piezo
            /*if (PS.Dual == 255)
            {
                GB.Controls["btnNote" + HR].Enabled = true;
                GB.Controls["dudNote" + HR].Enabled = true;
                chkDualPiezo.Checked = false;
                dudDualA.Text = "---";
                dudDualB.Text = "---";
                dudDualC.Text = "---";
                dudDualD.Text = "---";


                tbHead.Value = 128;
                tbRim.Value = 128;

                labelHR.Text = "H:--- R:---";

                if (Head) chkPiezoSuppression.Checked = false;
                return;
            }
            if (Head) chkPiezoSuppression.Checked = true;
            for (int n = 0; n < dudDualA.Items.Count; n++)
            {
                if (Head)
                {
                    if (((Note)dudDualD.Items[n]).Value == PS.DualNote)
                        dudDualD.SelectedIndex = n;
                }
                else
                {
                    if (((Note)dudDualA.Items[n]).Value == PS.DualNote)
                        dudDualA.SelectedIndex = n;
                }
            }
            chkDualPiezo.Checked = true;
            GB.Controls["dudNote" + HR].Text = "---";
            GB.Controls["btnNote" + HR].Enabled = false;
            GB.Controls["dudNote" + HR].Enabled = false;

            //Dual Piezo-Switch
            chkPiezoSuppression.Checked = true;

            if (Head)
                tbHead.Value = PS.DualThresold;
            else
                tbRim.Value = 255 - PS.DualThresold;

            pbDualZone.Refresh();*/
        }
        private void UpdateSwitch(bool Head, PinSetting PS)
        {
            string HR = Head ? "Head" : "Rim";
            GroupBox GB = Head ? gbHead : gbRim;

            GB.Controls["lbl" + HR + "Note"].Text = "Note:";
            GB.Controls["lbl" + HR + "ScanTime"].Text = "Switch Time:";
            GB.Controls["lbl" + HR + "MaskTime"].Text = "Choke Time:";
            GB.Controls["lblOpenThresold" + HR].Visible = false;
            GB.Controls["lblOpenNote" + HR].Visible = false;
            GB.Controls["nudOpenThresold" + HR].Visible = false;
            GB.Controls["dudOpenNote" + HR].Visible = false;
            GB.Controls["btnOpenThresold" + HR].Visible = false;
            GB.Controls["btnOpenNote" + HR].Visible = false;
            GB.Controls["lblChoke" + HR].Visible = true;
            GB.Controls["lblChoke" + HR].Text = "Choke:";
            GB.Controls["dudChoke" + HR].Visible = true;
            GB.Controls["nudSensibility" + HR].Visible = false;
            GB.Controls["btnChoke" + HR].Visible = true;

            /*lblAlternative.Visible = true;
            lblAlternativeThresold.Visible = true;
            lblAlternativeNote.Visible = true;
            dudAlternativeNote.Visible = true;
            nudAlternativeThresold.Visible = true;

            //Dual
            if (PS.Dual == 255)
            {
                if (Head) chkPiezoSuppression.Checked = false;
                chkDualPiezoSwitch.Checked = false;
                dudAlternativeNote.Text = "---";
                return;
            }
            if (Head) chkPiezoSuppression.Checked = true;
            chkDualPiezoSwitch.Checked = true;//DA RIVEDERE
            for (int n = 0; n < dudAlternativeNote.Items.Count; n++)
            {
                if (((Note)dudAlternativeNote.Items[n]).Value == PS.DualNote)
                    dudAlternativeNote.SelectedIndex = n;
            }

            nudAlternativeThresold.Value = PS.DualThresold;*/
        }
        private void UpdateHH(PinSetting PS)
        {
            string HR = "Head";
            GroupBox GB = gbHead;

            GB.Controls["lbl" + HR + "Note"].Text = "Close:";
            GB.Controls["lbl" + HR + "ScanTime"].Text = "Scan Time:";
            GB.Controls["lbl" + HR + "MaskTime"].Text = "Mask Time:";
            GB.Controls["lblOpenThresold" + HR].Text = "Open Thresold:";
            GB.Controls["lblOpenNote" + HR].Text = "Open Note:";
            GB.Controls["lblOpenThresold" + HR].Visible = true;
            GB.Controls["lblOpenNote" + HR].Visible = true;
            GB.Controls["nudOpenThresold" + HR].Visible = true;
            GB.Controls["dudOpenNote" + HR].Visible = true;
            GB.Controls["btnOpenThresold" + HR].Visible = true;
            GB.Controls["btnOpenNote" + HR].Visible = true;
            GB.Controls["lblChoke" + HR].Visible = true;
            GB.Controls["lblChoke" + HR].Text = "HHC:";
            GB.Controls["dudChoke" + HR].Visible = false;
            GB.Controls["nudSensibility" + HR].Visible = false;
            GB.Controls["btnChoke" + HR].Visible = true;
            cbHH_HHC.Visible = true;

            //Dual
            /*if (PS.Dual == 255) return;
            for (int i = 0; i < DrumMap.Length; i++)
            {
                if (PS.Dual == DrumMap[i].Head)
                    cbHH_HHC.SelectedItem = DrumMap[i].Name;
            }

            nudHH_AThresold.Value = hhSetting.A_Thresold;
            nudHH_BThresold.Value = hhSetting.B_Thresold;
            nudHH_CThresold.Value = hhSetting.C_Thresold;
            nudHH_DThresold.Value = hhSetting.D_Thresold;
            nudHH_FootCloseThresold.Value = hhSetting.FootCloseThresold;
            nudHH_FootSplashThresold.Value = hhSetting.FootSplashThresold;

            for (int n = 0; n < dudHH_A.Items.Count; n++)
            {
                if (((Note)dudHH_A.Items[n]).Value == hhSetting.A_Note)
                    dudHH_A.SelectedIndex = n;
                if (((Note)dudHH_B.Items[n]).Value == hhSetting.B_Note)
                    dudHH_B.SelectedIndex = n;
                if (((Note)dudHH_C.Items[n]).Value == hhSetting.C_Note)
                    dudHH_C.SelectedIndex = n;
                if (((Note)dudHH_D.Items[n]).Value == hhSetting.D_Note)
                    dudHH_D.SelectedIndex = n;

                if (((Note)dudHH_FootCloseNote.Items[n]).Value == hhSetting.FootCloseNote)
                    dudHH_FootCloseNote.SelectedIndex = n;
                if (((Note)dudHH_FootSplashNote.Items[n]).Value == hhSetting.FootSplashNote)
                    dudHH_FootSplashNote.SelectedIndex = n;
            }*/
        }
        private void UpdateHHC(PinSetting PS)
        {
            /*nudHH_AThresold.Value = hhSetting.A_Thresold;
            nudHH_BThresold.Value = hhSetting.B_Thresold;
            nudHH_CThresold.Value = hhSetting.C_Thresold;
            nudHH_DThresold.Value = hhSetting.D_Thresold;
            nudHH_FootCloseThresold.Value = hhSetting.FootCloseThresold;
            nudHH_FootSplashThresold.Value = hhSetting.FootSplashThresold;

            for (int n = 0; n < dudHH_A.Items.Count; n++)
            {
                if (((Note)dudHH_A.Items[n]).Value == hhSetting.A_Note)
                    dudHH_A.SelectedIndex = n;
                if (((Note)dudHH_B.Items[n]).Value == hhSetting.B_Note)
                    dudHH_B.SelectedIndex = n;
                if (((Note)dudHH_C.Items[n]).Value == hhSetting.C_Note)
                    dudHH_C.SelectedIndex = n;
                if (((Note)dudHH_D.Items[n]).Value == hhSetting.D_Note)
                    dudHH_D.SelectedIndex = n;

                if (((Note)dudHH_FootCloseNote.Items[n]).Value == hhSetting.FootCloseNote)
                    dudHH_FootCloseNote.SelectedIndex = n;
                if (((Note)dudHH_FootSplashNote.Items[n]).Value == hhSetting.FootSplashNote)
                    dudHH_FootSplashNote.SelectedIndex = n;
            }
            */

        }
        private void UpdateHHs(bool Head, PinSetting PS)
        {
            string HR = Head ? "Head" : "Rim";
            GroupBox GB = Head ? gbHead : gbRim;

            GB.Controls["lbl" + HR + "Note"].Text = "Close:";
            GB.Controls["lbl" + HR + "ScanTime"].Text = "Switch Time:";
            GB.Controls["lbl" + HR + "MaskTime"].Text = "Choke Time:";
            GB.Controls["lblOpenThresold" + HR].Text = "Open Thresold:";
            GB.Controls["lblOpenNote" + HR].Text = "Open Note:";
            GB.Controls["lblOpenThresold" + HR].Visible = true;
            GB.Controls["lblOpenNote" + HR].Visible = true;
            GB.Controls["nudOpenThresold" + HR].Visible = true;
            GB.Controls["dudOpenNote" + HR].Visible = true;
            GB.Controls["btnOpenThresold" + HR].Visible = true;
            GB.Controls["btnOpenNote" + HR].Visible = true;
            GB.Controls["lblChoke" + HR].Visible = true;
            GB.Controls["lblChoke" + HR].Text = "Choke:";
            GB.Controls["dudChoke" + HR].Visible = true;
            GB.Controls["nudSensibility" + HR].Visible = false;
            GB.Controls["btnChoke" + HR].Visible = true;


        }
        private void UpdateYSwitch(bool Head, PinSetting PS)
        {
            string HR = Head ? "Head" : "Rim";
            GroupBox GB = Head ? gbHead : gbRim;

            GB.Controls["lbl" + HR + "Note"].Text = "Shaft Note:";
            GB.Controls["lbl" + HR + "ScanTime"].Text = "Switch Time:";
            GB.Controls["lbl" + HR + "MaskTime"].Text = "Choke Time:";
            GB.Controls["lblOpenThresold" + HR].Text = "Bell Thresold:";
            GB.Controls["lblOpenNote" + HR].Text = "Bell Note:";
            GB.Controls["lblOpenThresold" + HR].Visible = true;
            GB.Controls["lblOpenNote" + HR].Visible = true;
            GB.Controls["nudOpenThresold" + HR].Visible = true;
            GB.Controls["dudOpenNote" + HR].Visible = true;
            GB.Controls["btnOpenThresold" + HR].Visible = true;
            GB.Controls["btnOpenNote" + HR].Visible = true;
            GB.Controls["lblChoke" + HR].Visible = true;
            GB.Controls["lblChoke" + HR].Text = "Choke:";
            GB.Controls["dudChoke" + HR].Visible = true;
            GB.Controls["nudSensibility" + HR].Visible = false;
            GB.Controls["btnChoke" + HR].Visible = true;

            /*lblAlternative.Visible = false;
            lblAlternativeThresold.Visible = false;
            lblAlternativeNote.Visible = false;
            dudAlternativeNote.Visible = false;
            nudAlternativeThresold.Visible = false;

            GB.Controls["btnNote" + HR].Enabled = true;
            GB.Controls["dudNote" + HR].Enabled = true;

            for (int n = 0; n < dudOpenNoteRim.Items.Count; n++)
            {
                if (((Note)dudOpenNoteRim.Items[n]).Value == PS.DualNote)
                    dudOpenNoteRim.SelectedIndex = n;
            }
            nudOpenThresoldRim.Value = PS.DualThresold;

            //Dual
            if (PS.Dual == 255)
            {
                if (Head) chkPiezoSuppression.Checked = false;

                chkDualPiezoSwitch.Checked = false;
                return;
            }
            if (Head) chkPiezoSuppression.Checked = true;

            chkDualPiezoSwitch.Checked = true;*/
        }
        private void UpdateDual(PinType HeadType, PinType RimType, bool DualHead, bool DualRim)
        {
            gbDualPP.Visible = HeadType == PinType.Piezo && RimType == PinType.Piezo;
            gbDualPS.Visible = (HeadType == PinType.Piezo && (RimType == PinType.Switch || RimType == PinType.YSwitch))
                || ((HeadType == PinType.Switch || HeadType == PinType.YSwitch) && RimType == PinType.Piezo);
            gbHHCtrl.Visible = HeadType == PinType.HHC;
            gbHHE.Visible = HeadType == PinType.HHC || HeadType == PinType.HH;
            gbHead.Visible = HeadType != PinType.HHC;
            gbHHCtrl.Visible = HeadType == PinType.HHC;
            cbHH_HHC.Visible = HeadType == PinType.HH;

            //Dual Piezo-Piezo
            if (gbDualPP.Visible)
            {
                if (DualHead && DualRim)
                {
                    dudNoteHead.Text = "---";
                    btnNoteHead.Enabled = false;
                    dudNoteHead.Enabled = false;
                    dudNoteRim.Text = "---";
                    btnNoteRim.Enabled = false;
                    dudNoteRim.Enabled = false;
                    chkDualPiezo.Checked = true;
                }
                else
                {
                    btnNoteHead.Enabled = true;
                    dudNoteHead.Enabled = true;
                    btnNoteRim.Enabled = true;
                    dudNoteRim.Enabled = true;
                    chkDualPiezo.Checked = false;
                    dudDualA.Text = "---";
                    dudDualB.Text = "---";
                    dudDualC.Text = "---";
                    dudDualD.Text = "---";

                    tbHead.Value = 128;
                    tbRim.Value = 128;

                    labelHR.Text = "H:--- R:---";
                }
            }

            if (gbDualPS.Visible)
            {
                if (HeadType == PinType.YSwitch || RimType == PinType.YSwitch)
                {
                    lblAlternative.Visible = false;
                    lblAlternativeThresold.Visible = false;
                    lblAlternativeNote.Visible = false;
                    dudAlternativeNote.Visible = false;
                    nudAlternativeThresold.Visible = false;
                }
                else
                {
                    lblAlternative.Visible = true;
                    lblAlternativeThresold.Visible = true;
                    lblAlternativeNote.Visible = true;
                    dudAlternativeNote.Visible = true;
                    nudAlternativeThresold.Visible = true;
                }

                chkPiezoSuppression.Checked = DualHead;
                chkDualPiezoSwitch.Checked = DualRim;

                if (!DualHead)
                    dudAlternativeNote.Text = "---";

                btnNoteHead.Enabled = true;
                dudNoteHead.Enabled = true;
                btnNoteRim.Enabled = true;
                dudNoteRim.Enabled = true;

            }
        }
        private void UpdateSetting()
        {
            int PinHead = DrumMap[lbPads.SelectedIndex].Head;
            int PinRim = DrumMap[lbPads.SelectedIndex].Rim;

            gbHead.Visible = true;
            gbRim.Visible = !DrumMap[lbPads.SelectedIndex].Single;
            /*gbDualPP.Visible = Setting[PinHead].Type == PinType.Piezo && Setting[PinRim].Type == PinType.Piezo;
            gbDualPS.Visible = (Setting[PinHead].Type == PinType.Piezo && (Setting[PinRim].Type == PinType.Switch || Setting[PinRim].Type == PinType.YSwitch))
                || ((Setting[PinHead].Type == PinType.Switch || Setting[PinHead].Type == PinType.YSwitch) && Setting[PinRim].Type == PinType.Piezo);
            gbHHCtrl.Visible = false;
            gbHHE.Visible = false;
            cbHH_HHC.Visible = false;*/

            gbHead.Text = lbPads.SelectedItem.ToString() + " Head";
            gbRim.Text = lbPads.SelectedItem.ToString() + " Rim";
            if (DrumMap[lbPads.SelectedIndex].Single || Setting[PinHead].Type == PinType.HHC)
                gbHead.Text = lbPads.SelectedItem.ToString();
            labelHR.Text = "H:" + Setting[PinHead].DualThresold.ToString() + "   R:" + Setting[PinRim].DualThresold.ToString();

            if (Setting[PinHead].Type == PinType.Piezo) UpdatePiezo(true, Setting[PinHead]);
            else if (Setting[PinHead].Type == PinType.Switch) UpdateSwitch(true, Setting[PinHead]);
            else if (Setting[PinHead].Type == PinType.HH) UpdateHH(Setting[PinHead]);
            else if (Setting[PinHead].Type == PinType.HHC) UpdateHHC(Setting[PinHead]);
            else if (Setting[PinHead].Type == PinType.HHs) UpdateHHs(true, Setting[PinHead]);
            else if (Setting[PinHead].Type == PinType.YSwitch) UpdateYSwitch(true, Setting[PinHead]);

            if (Setting[PinRim].Type == PinType.Piezo) UpdatePiezo(false, Setting[PinRim]);
            else if (Setting[PinRim].Type == PinType.Switch) UpdateSwitch(false, Setting[PinRim]);
            else if (Setting[PinRim].Type == PinType.HHs) UpdateHHs(false, Setting[PinRim]);
            else if (Setting[PinRim].Type == PinType.YSwitch) UpdateYSwitch(false, Setting[PinRim]);

            UpdateDual(Setting[PinHead].Type, Setting[PinRim].Type, Setting[PinHead].Dual != 127, Setting[PinRim].Dual != 127);

            //Note===========================================================================
            //nudNoteHead.Value = Setting[PinHead].Note;
            //nudNoteRim.Value = Setting[PinRim].Note;

            bool HeadChanged = false;
            bool RimChanged = false;
            for (int n = 0; n < dudNoteHead.Items.Count; n++)
            {
                if (((Note)dudNoteHead.Items[n]).Value == Setting[PinHead].Note)
                {
                    dudNoteHead.SelectedIndex = n;
                    dudDualB.SelectedIndex = n;
                    HeadChanged = true;
                }
                if (((Note)dudNoteRim.Items[n]).Value == Setting[PinRim].Note)
                {
                    dudNoteRim.SelectedIndex = n;
                    dudDualC.SelectedIndex = n;
                    RimChanged = true;
                }
                if (((Note)dudDualD.Items[n]).Value == Setting[PinHead].DualNote)
                    dudDualD.SelectedIndex = n;
                if (((Note)dudDualA.Items[n]).Value == Setting[PinRim].DualNote)
                    dudDualA.SelectedIndex = n;
                if (((Note)dudAlternativeNote.Items[n]).Value == Setting[PinRim].DualNote)
                    dudAlternativeNote.SelectedIndex = n;
                if (((Note)dudOpenNoteRim.Items[n]).Value == Setting[PinRim].DualNote)
                    dudOpenNoteRim.SelectedIndex = n;
                if (((Note)dudOpenNoteHead.Items[n]).Value == Setting[PinHead].DualNote)
                    dudOpenNoteHead.SelectedIndex = n;
            }
            if (!HeadChanged)
            {
                dudNoteHead.SelectedIndex = dudNoteHead.Items.Count - 1;
                dudDualB.SelectedIndex = dudDualB.Items.Count - 1;
            }
            if (!RimChanged)
            {
                dudNoteRim.SelectedIndex = dudNoteRim.Items.Count - 1;
                dudDualC.SelectedIndex = dudDualC.Items.Count - 1;
            }

            nudCCHHC.Value = Setting[PinHead].Note;

            //Thresold===========================================================================
            nudThresoldHead.Value = Setting[PinHead].Thresold;
            nudThresoldRim.Value = Setting[PinRim].Thresold;

            //ScanTime===========================================================================
            nudScanTimeHead.Value = Setting[PinHead].ScanTime;
            nudScanTimeRim.Value = Setting[PinRim].ScanTime;

            //MaskTime===========================================================================
            nudMaskTimeHead.Value = Setting[PinHead].MaskTime;
            nudMaskTimeRim.Value = Setting[PinRim].MaskTime;
            nudMaskTimeHHC.Value = nudMaskTimeHead.Value;

            //Retrigger===========================================================================
            nudRetriggerHead.Value = Setting[PinHead].Retrigger;
            nudRetriggerRim.Value = Setting[PinRim].Retrigger;

            //Curve===========================================================================
            cbCurveHead.SelectedIndex = Setting[PinHead].Curve;
            cbCurveRim.SelectedIndex = Setting[PinRim].Curve;

            //Xtalk===========================================================================
            nudXtalkHead.Value = Setting[PinHead].Xtalk;
            nudXtalkRim.Value = Setting[PinRim].Xtalk;

            //XtalkGroup===========================================================================
            nudXtalkGroupHead.Value = Setting[PinHead].XtalkGroup;
            nudXtalkGroupRim.Value = Setting[PinRim].XtalkGroup;

            //CurveForm===========================================================================
            nudCurveFormHead.Value = Setting[PinHead].CurveForm;
            nudCurveFormRim.Value = Setting[PinRim].CurveForm;

            //Type===========================================================================
            cbTypeHead.SelectedIndex = (int)Setting[PinHead].Type;
            cbTypeRim.SelectedItem = Setting[PinRim].Type;
            cbTypeHHC.SelectedIndex = cbTypeHead.SelectedIndex;

            //Choke===========================================================================
            for (int n = 0; n < dudChokeRim.Items.Count; n++)
            {
                if (((Note)dudChokeRim.Items[n]).Value == Setting[PinRim].Choke)
                    dudChokeRim.SelectedIndex = n;
            }
            for (int n = 0; n < dudChokeHead.Items.Count; n++)
            {
                if (((Note)dudChokeHead.Items[n]).Value == Setting[PinHead].Choke)
                    dudChokeHead.SelectedIndex = n;
            }
            //Numb===========================================================================
            nudSensibilityHead.Value = Setting[PinHead].Choke;
            nudSensibilityRim.Value = Setting[PinRim].Choke;

            //DualThresold===========================================================================
            nudAlternativeThresold.Value = Setting[PinRim].DualThresold;
            tbHead.Value = Setting[PinHead].DualThresold;
            tbRim.Value = 255 - Setting[PinRim].DualThresold;
            nudOpenThresoldRim.Value = Setting[PinRim].DualThresold;
            nudOpenThresoldHead.Value = Setting[PinHead].DualThresold;

            pbDualZone.Refresh();

            //Dual
            for (int i = 0; i < DrumMap.Length; i++)
            {
                if (Setting[PinHead].Dual == DrumMap[i].Head)
                    cbHH_HHC.SelectedItem = DrumMap[i].Name;
            }

            //General===========================================================================

            nudHH_AThresold.Value = hhSetting.A_Thresold;
            nudHH_BThresold.Value = hhSetting.B_Thresold;
            nudHH_CThresold.Value = hhSetting.C_Thresold;
            nudHH_DThresold.Value = hhSetting.D_Thresold;
            nudHH_FootCloseThresold.Value = hhSetting.FootCloseThresold;
            nudHH_FootSplashThresold.Value = hhSetting.FootSplashThresold;

            for (int n = 0; n < dudHH_A.Items.Count; n++)
            {
                if (((Note)dudHH_A.Items[n]).Value == hhSetting.A_Note)
                    dudHH_A.SelectedIndex = n;
                if (((Note)dudHH_B.Items[n]).Value == hhSetting.B_Note)
                    dudHH_B.SelectedIndex = n;
                if (((Note)dudHH_C.Items[n]).Value == hhSetting.C_Note)
                    dudHH_C.SelectedIndex = n;
                if (((Note)dudHH_D.Items[n]).Value == hhSetting.D_Note)
                    dudHH_D.SelectedIndex = n;

                if (((Note)dudHH_FootCloseNote.Items[n]).Value == hhSetting.FootCloseNote)
                    dudHH_FootCloseNote.SelectedIndex = n;
                if (((Note)dudHH_FootSplashNote.Items[n]).Value == hhSetting.FootSplashNote)
                    dudHH_FootSplashNote.SelectedIndex = n;
            }
            nudDelay.Value = gSetting.DelayTime;
            //nudHHC1.Value = gSetting.HHC1;
            nudNSensor.Value = gSetting.NSensor < nudNSensor.Minimum ? nudNSensor.Minimum : gSetting.NSensor;
            nudGeneralXtalk.Value = gSetting.Xtalk;
        }
        #endregion

        #region Form Event
        private void tscCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            //se la porta selezionata è diversa da quella aperta chiudi e riapri
            if (/*XXUtilitySerial.COM_Name*/UtilityMIDI.PortName(MIDIType.Serial) != (string)tscCOM.SelectedItem)
            {
                //XX//UtilitySerial.CloseCOM();
                UtilityMIDI.Close(MIDIType.Serial);

                //XX//if (UtilitySerial.OpenCOM((string)tscCOM.SelectedItem))
                if (UtilityMIDI.OpenMIDI(MIDIType.Serial, (string)tscCOM.SelectedItem))
                    SetState(mDMode.Off);
                else
                    tscCOM.Text = "Select COM...";
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //File.Delete(Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");

            SaveSetting(Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");

            //Salviamo la porta aperta
            if (UtilityMIDI.IsOpen(MIDIType.Serial))
            {
                UtilityIniFile.SetIniString("Setup", "COM", UtilityMIDI.PortName(MIDIType.Serial), Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
                UtilityMIDI.Close(MIDIType.Serial);
            }
            if (UtilityMIDI.IsOpen(MIDIType.MIDI_IN))
            {
                UtilityIniFile.SetIniString("Setup", "MIDI_IN", UtilityMIDI.PortName(MIDIType.MIDI_IN), Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
                UtilityMIDI.Close(MIDIType.MIDI_IN);
            }
            else UtilityIniFile.SetIniString("Setup", "MIDI_IN", "", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");

            if (UtilityMIDI.IsOpen(MIDIType.MIDI_OUT))
            {
                UtilityIniFile.SetIniString("Setup", "MIDI_OUT", UtilityMIDI.PortName(MIDIType.MIDI_OUT), Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
                UtilityMIDI.Close(MIDIType.MIDI_OUT);
            }
            else UtilityIniFile.SetIniString("Setup", "MIDI_OUT", "", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");


            UtilityIniFile.SetIniString("Setup", "Configuration", configurationToolStripMenuItem.Checked ? "Show" : "Hide", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
            UtilityIniFile.SetIniString("Setup", "Tool", toolToolStripMenuItem.Checked ? "Show" : "Hide", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
            UtilityIniFile.SetIniString("Setup", "Monitor", monitorToolStripMenuItem.Checked ? "Show" : "Hide", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
            UtilityIniFile.SetIniString("Setup", "SFZ", sFZToolStripMenuItem.Checked ? "Show" : "Hide", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");
            UtilityIniFile.SetIniString("Setup", "Effects", effectsToolStripMenuItem.Checked ? "Show" : "Hide", Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini");

            if (vstForm != null)
            {
                vstForm.Dispose();
                vstForm = null;
            }
            if (sfz != null) sfz.Dispose();
            UtilityAudio.Dispose();

        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < TabPages.Length; i++)
                if (TabPages[i] == tcMain.SelectedTab)
                {
                    if (i == (int)Tabs.Configuration)
                    {
                        if (!chkAlwaysMIDI.Checked) SetState(mDMode.StandBy);
                        Mode = mDMode.StandBy;
                    }
                    else if (i == (int)Tabs.Tools)
                    {
                        if (!chkAlwaysMIDI.Checked) SetState(mDMode.Tool);
                        Mode = mDMode.Tool;
                    }
                    else if (i == (int)Tabs.Monitor || i == (int)Tabs.Sfz || i == (int)Tabs.Effects)
                    {
                        SetState(mDMode.MIDI);
                        Mode = mDMode.MIDI;
                    }
                    return;
                }
        }
        private void lbPads_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSetting();
        }
        private void loadAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Metto il modulo in standby
            tcMain.SelectedIndex = 0;
            pbLoad.Value = 0;
            tcMain.Enabled = false;

            //richiedo tutti i setting
            UtilityMIDI.MIDI_SysEx(0x02, 0x7F, 0x7F, 0x00);
        }
        private void loadSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Metto il modulo in standby
            tcMain.SelectedIndex = 0;
            pbLoad.Value = 0;

            //richiedo tutti i setting per il pad selezionato
            UtilityMIDI.MIDI_SysEx(0x02, (byte)DrumMap[lbPads.SelectedIndex].Head, 0x7F, 0x00);
            UtilityMIDI.MIDI_SysEx(0x02, (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x7F, 0x00);
        }
        private void loadGeneralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Metto il modulo in standby
            tcMain.SelectedIndex = 0;
            pbLoad.Value = 0;

            //richiedo tutti i setting per le impostazioni generali
            UtilityMIDI.MIDI_SysEx(0x02, 0x7E, 0x7F, 0x00);
        }
        private void sendAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (byte i = 0; i < Setting.Length; i++)
            {
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x00, Setting[i].Note);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x01, Setting[i].Thresold);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x02, Setting[i].ScanTime);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x03, Setting[i].MaskTime);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x04, Setting[i].Retrigger);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x05, Setting[i].Curve);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x06, Setting[i].Xtalk);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x07, Setting[i].XtalkGroup);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x08, Setting[i].CurveForm);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x09, Setting[i].Choke);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x0A, Setting[i].Dual);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x0B, Setting[i].DualNote);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x0C, Setting[i].DualThresold);
                UtilityMIDI.MIDI_SysEx(0x03, i, 0x0D, (byte)Setting[i].Type);
            }

            UtilityMIDI.MIDI_SysEx(0x03, 0x7E, 0x00, gSetting.DelayTime);
            //HHC1
            UtilityMIDI.MIDI_SysEx(0x03, 0x7E, 0x02, (byte)(gSetting.NSensor / 8));
            UtilityMIDI.MIDI_SysEx(0x03, 0x7E, 0x03, gSetting.Xtalk);
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select VST:";
            ofd.Filter = "Settings Files (*.smd)|*.smd";
            ofd.InitialDirectory = Application.StartupPath;
            DialogResult res = ofd.ShowDialog();

            if (res != DialogResult.OK || !File.Exists(ofd.FileName)) return;

            LoadSetting(ofd.FileName);
            UpdateSetting();
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Select setting file:";
            saveFile.Filter = "Settimg Files (*.smd)|*.smd";
            saveFile.AddExtension = true;
            DialogResult res = saveFile.ShowDialog();

            if (res != DialogResult.OK) return;
            SaveSetting(saveFile.FileName);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //XX//UtilitySerial.DiscardAllBuffer();
            UtilityMIDI.DiscardAllBuffer(MIDIType.Serial);
            UtilityMIDI.MIDI_SysEx(0x7F, 0x00, 0x00, 0x00);//RESET
            Mode = mDMode.Off;
        }
        private void tFader_Tick(object sender, EventArgs e)
        {
            grap.DrawLine(Pens.White, pbMIDI.Width / 2, 0, pbMIDI.Width / 2, 300);
            if (!cbFire.Checked) grap.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.Black)), new Rectangle(0, 0, pbMIDI.Width, pbMIDI.Height));
            grapPB.DrawImage(grapBitmap, 0, 0, fWidth, fHeight);
            if (cbFire.Checked) DoFlames();

            //già che ci siamo....
            tslNowTime.Text = UtilityAudio.GetMp3CurrentTime().ToString();
        }

        //VALUES
        private void dudNoteHead_SelectedItemChanged(object sender, EventArgs e)
        {
            if (dudNoteHead.SelectedIndex >= 0)
                Setting[DrumMap[lbPads.SelectedIndex].Head].Note = ((Note)dudNoteHead.SelectedItem).Value;
        }
        private void dudNoteRim_SelectedItemChanged(object sender, EventArgs e)
        {
            if (dudNoteRim.SelectedIndex >= 0)
                Setting[DrumMap[lbPads.SelectedIndex].Rim].Note = ((Note)dudNoteRim.SelectedItem).Value;
        }
        private void cbCurveHead_SelectedItemChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].Curve = (byte)cbCurveHead.SelectedIndex;
        }
        private void cbCurveRim_SelectedItemChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].Curve = (byte)cbCurveRim.SelectedIndex;
        }
        private void nudThresoldHead_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].Thresold = (byte)nudThresoldHead.Value;
        }
        private void nudThresoldRim_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].Thresold = (byte)nudThresoldRim.Value;
        }
        private void nudScanTimeHead_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].ScanTime = (byte)nudScanTimeHead.Value;
        }
        private void nudScanTimeRim_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].ScanTime = (byte)nudScanTimeRim.Value;
        }
        private void nudMaskTimeHead_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].MaskTime = (byte)nudMaskTimeHead.Value;
        }
        private void nudMaskTimeRim_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].MaskTime = (byte)nudMaskTimeRim.Value;
        }
        private void nudRetriggerHead_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].Retrigger = (byte)nudRetriggerHead.Value;
        }
        private void nudRetriggerRim_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].Retrigger = (byte)nudRetriggerRim.Value;
        }
        private void dudChokeRim_SelectedItemChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].Choke = ((Note)dudChokeRim.SelectedItem).Value;
        }
        private void dudChokeHead_SelectedItemChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].Choke = ((Note)dudChokeHead.SelectedItem).Value;
        }
        private void nudSensibilityHead_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].Choke = (byte)nudSensibilityHead.Value;
        }
        private void nudSensibilityRim_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].Choke = (byte)nudSensibilityRim.Value;
        }
        private void cbTypeHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].Type = (PinType)cbTypeHead.SelectedIndex;
            UpdateSetting();
        }
        private void cbTypeRim_SelectedIndexChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].Type = (PinType)cbTypeRim.SelectedItem;
            UpdateSetting();
        }
        private void cbTypeHHC_SelectedIndexChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].Type = (PinType)cbTypeHHC.SelectedIndex;

            UpdateSetting();
        }
        private void nudCurveFormHead_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].CurveForm = (byte)nudCurveFormHead.Value;
        }

        private void nudCurveFormRim_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].CurveForm = (byte)nudCurveFormRim.Value;
        }
        private void dudDualA_SelectedItemChanged(object sender, EventArgs e)
        {
            if (dudDualA.SelectedItem != null)
                Setting[DrumMap[lbPads.SelectedIndex].Rim].DualNote = ((Note)dudDualA.SelectedItem).Value;
        }
        private void dudDualB_SelectedItemChanged(object sender, EventArgs e)
        {
            if (dudDualB.SelectedItem != null)
                Setting[DrumMap[lbPads.SelectedIndex].Head].Note = ((Note)dudDualB.SelectedItem).Value;
        }
        private void dudDualC_SelectedItemChanged(object sender, EventArgs e)
        {
            if (dudDualC.SelectedItem != null)
                Setting[DrumMap[lbPads.SelectedIndex].Rim].Note = ((Note)dudDualC.SelectedItem).Value;
        }
        private void dudDualD_SelectedItemChanged(object sender, EventArgs e)
        {
            if (dudDualD.SelectedItem != null)
                Setting[DrumMap[lbPads.SelectedIndex].Head].DualNote = ((Note)dudDualD.SelectedItem).Value;
        }
        private void nudXtalkHead_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].Xtalk = (byte)nudXtalkHead.Value;
        }
        private void nudXtalkRim_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].Xtalk = (byte)nudXtalkRim.Value;
        }
        private void nudXtalkGroupHead_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].XtalkGroup = (byte)nudXtalkGroupHead.Value;
        }
        private void nudXtalkGroupRim_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].XtalkGroup = (byte)nudXtalkGroupRim.Value;
        }
        //===================================================================
        //BUTTON
        private void btnNoteHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x00, ((Note)dudNoteHead.SelectedItem).Value);
        }
        private void btnNoteRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x00, ((Note)dudNoteRim.SelectedItem).Value);
        }
        private void btnThresoldHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x01, (byte)nudThresoldHead.Value);
        }
        private void btnThresoldRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x01, (byte)nudThresoldRim.Value);
        }
        private void btnScanTimeHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x02, (byte)nudScanTimeHead.Value);
        }
        private void btnScanTimeRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x02, (byte)nudScanTimeRim.Value);
        }
        private void btnMaskTimeHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x03, (byte)nudMaskTimeHead.Value);
        }
        private void btnMaskTimeRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x03, (byte)nudMaskTimeRim.Value);
        }
        private void btnRetriggerHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x04, (byte)nudRetriggerHead.Value);
        }
        private void btnRetriggerRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x04, (byte)nudRetriggerRim.Value);
        }
        private void btnCurveHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x05, (byte)cbCurveHead.SelectedIndex);
        }
        private void btnCurveRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x05, (byte)cbCurveRim.SelectedIndex);
        }
        private void btnXtalkHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x06, (byte)nudXtalkHead.Value);
        }
        private void btnXtalkRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x06, (byte)nudXtalkHead.Value);
        }

        private void btnXtalkGroupHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x07, (byte)nudXtalkGroupHead.Value);
        }
        private void btnXtalkGroupRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x07, (byte)nudXtalkGroupHead.Value);
        }
        private void btnChokeRim_Click(object sender, EventArgs e)
        {
            if (Setting[DrumMap[lbPads.SelectedIndex].Rim].Type == PinType.Piezo)
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x09, (byte)nudSensibilityRim.Value);
            else
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x09, ((Note)dudChokeRim.SelectedItem).Value);
        }
        private void btnChokeHead_Click(object sender, EventArgs e)
        {
            if (Setting[DrumMap[lbPads.SelectedIndex].Head].Type == PinType.HH)
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0A, Setting[DrumMap[lbPads.SelectedIndex].Head].Dual); //HHC
            else if (Setting[DrumMap[lbPads.SelectedIndex].Head].Type == PinType.Piezo)
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x09, (byte)nudSensibilityHead.Value);
            else
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x09, ((Note)dudChokeHead.SelectedItem).Value);
        }
        private void btnTypeHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0D, (byte)cbTypeHead.SelectedIndex);
        }
        private void btnTypeRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0D, (byte)(PinType)cbTypeRim.SelectedItem);
        }
        private void btnCurveFormHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x08, (byte)nudCurveFormHead.Value);
        }
        private void btnCurveFormRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x08, (byte)nudCurveFormRim.Value);
        }
        private void btnDualPiezo_Click(object sender, EventArgs e)
        {
            if (chkDualPiezo.Checked)
            {
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x00, ((Note)dudDualB.SelectedItem).Value);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0A, (byte)DrumMap[lbPads.SelectedIndex].Rim);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0B, ((Note)dudDualD.SelectedItem).Value);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0C, (byte)tbHead.Value);

                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x00, ((Note)dudDualC.SelectedItem).Value);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0A, (byte)DrumMap[lbPads.SelectedIndex].Head);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0B, ((Note)dudDualA.SelectedItem).Value);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0C, (byte)(255 - (byte)tbRim.Value));
            }
            else
            {
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0A, 127);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0A, 127);
            }
        }
        //=======================================================

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string COM = null;
            if (/*XX UtilitySerial.IsOpen*/ UtilityMIDI.IsOpen(MIDIType.Serial))
            {
                COM = UtilityMIDI.PortName(MIDIType.Serial);//XX//UtilitySerial.COM_Name;
                SetState(mDMode.Off);
            }
            //XX//UtilitySerial.CloseCOM();
            UtilityMIDI.Close(MIDIType.Serial);

            UpdateForm update = new UpdateForm();

            update.COM = COM;
            update.ShowDialog();

            //XX//if (UtilitySerial.OpenCOM((string)tscCOM.SelectedItem))
            if (UtilityMIDI.OpenMIDI(MIDIType.Serial, (string)tscCOM.SelectedItem))
                SetState(mDMode.Off);
            else
                tscCOM.Text = "Select COM...";
        }
        private void tcMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > tcMain.TabPages.Count) return;

            TabPage CurrentTab = tcMain.TabPages[e.Index];
            Rectangle ItemRect = tcMain.GetTabRect(e.Index);
            SolidBrush FillBrush = new SolidBrush(Color.SteelBlue);
            SolidBrush BackBrush = new SolidBrush(Color.Transparent);
            SolidBrush TextBrush = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            //If we are currently painting the Selected TabItem we'll
            //change the brush colors and inflate the rectangle.
            if (System.Convert.ToBoolean(e.State & DrawItemState.Selected))
            {
                FillBrush.Color = Color.LightSteelBlue;
                TextBrush.Color = Color.White;
                ItemRect.Inflate(2, 2);
            }

            //Set up rotation for left and right aligned tabs
            if (tcMain.Alignment == TabAlignment.Left || tcMain.Alignment == TabAlignment.Right)
            {
                float RotateAngle = 90;
                if (tcMain.Alignment == TabAlignment.Left)
                    RotateAngle = 270;
                PointF cp = new PointF(ItemRect.Left + (ItemRect.Width / 2), ItemRect.Top + (ItemRect.Height / 2));
                e.Graphics.TranslateTransform(cp.X, cp.Y);
                e.Graphics.RotateTransform(RotateAngle);
                ItemRect = new Rectangle(-(ItemRect.Height / 2), -(ItemRect.Width / 2), ItemRect.Height, ItemRect.Width);
            }

            if (e.Index == this.tcMain.TabCount - 1)
            {
                Rectangle backRect = new Rectangle();
                backRect = ItemRect;
                backRect.X = this.tcMain.TabCount * this.tcMain.ItemSize.Width;
                backRect.Width = this.tcMain.Width - backRect.X;
                //backRect.Height += 2;
                backRect.Y -= 2;
                e.Graphics.FillRectangle(BackBrush, backRect);
            }

            //Next we'll paint the TabItem with our Fill Brush

            Rectangle backItemRect = ItemRect;
            backItemRect.Y -= 2;
            backItemRect.X -= 2;
            e.Graphics.FillRectangle(BackBrush, backItemRect);
            e.Graphics.FillRectangle(FillBrush, ItemRect);

            //Now draw the text.
            e.Graphics.DrawString(CurrentTab.Text, e.Font, TextBrush, ItemRect, sf);

            //Reset any Graphics rotation
            e.Graphics.ResetTransform();

            //Finally, we should Dispose of our brushes.
            FillBrush.Dispose();
            TextBrush.Dispose();
        }
        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (vstForm != null)
            {
                vstForm.Dispose();
                vstForm = null;

                showToolStripMenuItem.Enabled = false;
                editParametersToolStripMenuItem.Enabled = false;
                noteMapToolStripMenuItem.Enabled = false;

                loadToolStripMenuItem1.Text = "Load...";
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Select VST:";
                ofd.Filter = "VST Files (*.dll)|*.dll";
                if (LastDirectoryUsed.ContainsKey("VSTDir"))
                    ofd.InitialDirectory = LastDirectoryUsed["VSTDir"];
                else
                    ofd.InitialDirectory = UtilityAudio.GetVSTDirectory();
                DialogResult res = ofd.ShowDialog();

                if (res != DialogResult.OK || !File.Exists(ofd.FileName)) return;

                try
                {
                    if (LastDirectoryUsed.ContainsKey("VSTDir"))
                        LastDirectoryUsed["VSTDir"] = Directory.GetParent(ofd.FileName).FullName;
                    else
                        LastDirectoryUsed.Add("VSTDir", Directory.GetParent(ofd.FileName).FullName);
                    vstForm = new VSTForm(ofd.FileName);
                    vstForm.Show();

                    showToolStripMenuItem.Enabled = true;
                    editParametersToolStripMenuItem.Enabled = true;
                    noteMapToolStripMenuItem.Enabled = true;

                    loadToolStripMenuItem1.Text = "Unload...";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void editParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vstForm != null)
                vstForm.ShowEditParameters();
        }
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vstForm != null)
            {
                if (vstForm.Visible) vstForm.BringToFront();
                else vstForm.Visible = true;
            }
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nudNSensor.Enabled = false;
            btnNSensor.Enabled = false;
            if (aboutForm.ShowDialog() == DialogResult.OK)
            {
                nudNSensor.Enabled = true;
                btnNSensor.Enabled = true;
            }
        }
        private void drumMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrumMapForm dmForm = new DrumMapForm(DrumMap);
            if (dmForm.ShowDialog() == DialogResult.OK)
            {
                DrumMap = dmForm.DrumMap;
                lbPads.Items.Clear();
                foreach (DMap dm in DrumMap)
                    lbPads.Items.Add(dm.Name);
            }
        }
        private void lbLog_MouseDown(object sender, MouseEventArgs e)
        {
            Clipboard.Clear();
            string Data = "";
            uint startTime = ((Log)lbLog.Items[0]).Time;

            foreach (Log s in lbLog.Items)
                Data += ((float)s.Time - startTime) + " " + ((float)s.Reading) + "\r\n";

            Clipboard.SetText(Data);
        }
        private void nudDelay_ValueChanged(object sender, EventArgs e)
        {
            gSetting.DelayTime = (byte)nudDelay.Value;
        }
        private void btnGeneral_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x7E, 0x00, (byte)nudDelay.Value);
        }
        #endregion

        private void pbDualZone_Paint(object sender, PaintEventArgs e)
        {
            Rectangle A = new Rectangle(0, 0, e.ClipRectangle.Width * tbHead.Value / tbHead.Maximum, e.ClipRectangle.Height - e.ClipRectangle.Height * tbRim.Value / tbRim.Maximum);
            Rectangle B = new Rectangle(e.ClipRectangle.Width * tbHead.Value / tbHead.Maximum, 0, e.ClipRectangle.Width - e.ClipRectangle.Width * tbHead.Value / tbHead.Maximum, e.ClipRectangle.Height - e.ClipRectangle.Height * tbRim.Value / tbRim.Maximum);
            Rectangle C = new Rectangle(0, e.ClipRectangle.Height - e.ClipRectangle.Height * tbRim.Value / tbRim.Maximum, e.ClipRectangle.Width * tbHead.Value / tbHead.Maximum, e.ClipRectangle.Height * tbRim.Value / tbRim.Maximum);
            Rectangle D = new Rectangle(e.ClipRectangle.Width * tbHead.Value / tbHead.Maximum, e.ClipRectangle.Height - e.ClipRectangle.Height * tbRim.Value / tbRim.Maximum, e.ClipRectangle.Width - e.ClipRectangle.Width * tbHead.Value / tbHead.Maximum, e.ClipRectangle.Height * tbRim.Value / tbRim.Maximum);

            DrawLetterInsideDualPiezoGraph(e.Graphics, A, "A");
            DrawLetterInsideDualPiezoGraph(e.Graphics, B, "B");
            DrawLetterInsideDualPiezoGraph(e.Graphics, C, "C");
            DrawLetterInsideDualPiezoGraph(e.Graphics, D, "D");
        }
        private void DrawLetterInsideDualPiezoGraph(Graphics g, Rectangle r, string s)
        {
            byte gray = (byte)(r.Width * r.Height * 256 / (64 * 64));
            g.FillRectangle(new SolidBrush(Color.FromArgb(gray, gray, gray)), r);
            g.DrawRectangle(Pens.SteelBlue, r);
            g.DrawString(s, this.Font, Brushes.SteelBlue, -(g.MeasureString(s, this.Font).Width / 2) + r.X + r.Width / 2, -(g.MeasureString(s, this.Font).Height / 2) + r.Y + r.Height / 2);

        }
        private void chkDualPiezo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDualPiezo.Checked)
            {
                tbHead.Enabled = true;
                tbRim.Enabled = true;
                dudDualA.Enabled = true;
                dudDualB.Enabled = true;
                dudDualC.Enabled = true;
                dudDualD.Enabled = true;
                //btnDualPiezo.Enabled = true;

                Setting[DrumMap[lbPads.SelectedIndex].Head].Dual = DrumMap[lbPads.SelectedIndex].Rim;
                Setting[DrumMap[lbPads.SelectedIndex].Rim].Dual = DrumMap[lbPads.SelectedIndex].Head;
            }
            else
            {
                //btnDualPiezo.Enabled = false;
                tbHead.Enabled = false;
                tbRim.Enabled = false;
                dudDualA.Enabled = false;
                dudDualB.Enabled = false;
                dudDualC.Enabled = false;
                dudDualD.Enabled = false;
                dudDualA.Text = "---";
                dudDualB.Text = "---";
                dudDualC.Text = "---";
                dudDualD.Text = "---";

                Setting[DrumMap[lbPads.SelectedIndex].Head].Dual = 127;
                Setting[DrumMap[lbPads.SelectedIndex].Rim].Dual = 127;

            }

            UpdateSetting();
        }

        private void tbHeadRim_Scroll(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].DualThresold = (byte)tbHead.Value;
            Setting[DrumMap[lbPads.SelectedIndex].Rim].DualThresold = (byte)(255 - (byte)tbRim.Value);
            pbDualZone.Refresh();

            labelHR.Text = "H:" + Setting[DrumMap[lbPads.SelectedIndex].Head].DualThresold.ToString() +
                "   R:" + Setting[DrumMap[lbPads.SelectedIndex].Rim].DualThresold.ToString();
        }

        private void nudLog_ValueChanged(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx(0x6E, (byte)nudLog.Value, (byte)nudThresold.Value, 0x00);
            cbCurveViewer.SelectedIndex = Setting[(byte)nudLog.Value].Curve;
            nudFormCurveViewer.Value = Setting[(byte)nudLog.Value].CurveForm;
        }

        private void nudThresold_ValueChanged(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx(0x6E, (byte)nudLog.Value, (byte)nudThresold.Value, 0x00);
        }

        /*private void nudHHC1_ValueChanged(object sender, EventArgs e)
        {
            gSetting.HHC1 = (byte)nudHHC1.Value;
        }*/

        private void nudCCHHC_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].Note = (byte)nudCCHHC.Value;
        }

        /*private void btnHHC1_Click(object sender, EventArgs e)
        {
            SendSysEx(0x03, 0x7E, 0x01, (byte)nudHHC1.Value);
        }*/

        private void nudGeneralXtalk_ValueChanged(object sender, EventArgs e)
        {
            gSetting.Xtalk = (byte)nudGeneralXtalk.Value;
        }

        private void btnGeneralXtalk_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x7E, 0x03, (byte)nudGeneralXtalk.Value);
        }

        private void pbCurve_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            float f = (float)nudFormCurveViewer.Value / 128.0f;

            int CurvePass = 25;
            for (int i = 0; i < 128; i += 10)
            {
                g.DrawLine(Pens.LightGray, 0, i, 256, i);
            }
            for (int i = 0; i < 5; i++)
            {
                if (lbLog.Items.Count - i - 1 < 0) break;
                Log l = (Log)lbLog.Items[lbLog.Items.Count - i - 1];
                float y = Curve(cbCurveViewer.SelectedIndex, l.Reading, f);
                if (!float.IsNaN(y))
                {
                    g.DrawLine(Pens.Red, l.Reading / 4, 128 - y, l.Reading / 4, 128);
                    g.DrawLine(Pens.Red, 0, 128 - y, l.Reading / 4, 128 - y);
                }
            }

            for (int i = CurvePass; i < 1024; i += CurvePass)
            {
                float x1 = i - CurvePass;
                float x2 = i;
                float y1 = Curve(cbCurveViewer.SelectedIndex, x1, f);
                float y2 = Curve(cbCurveViewer.SelectedIndex, x2, f);

                g.DrawLine(Pens.LightGray, x1 / 4, 0, x1 / 4, 128);
                if (float.IsNaN(y1) || float.IsNaN(y2)) continue;
                g.DrawLine(Pens.Black, x1 / 4, 128 - y1, x2 / 4, 128 - y2);
            }

        }
        float Curve(int index, float x, float form)
        {
            if (index == 0) { return form * x / 8; }
            else if (index == 1) { return (127.0f / ((float)Math.Exp(2.0f * form) - 1.0f)) * ((float)Math.Exp(form * x / 512.0f) - 1.0f); }
            else if (index == 2) { return (float)Math.Log(1.0f + (form * x / 128.0f)) * (127.0f / (float)Math.Log((8.0f * form) + 1.0f)); }
            else if (index == 3) { return (127.0f / (1.0f + (float)Math.Exp(form * (512.0f - x) / 64.0f))); }
            else if (index == 4) { return (64.0f - ((8.0f / form) * (float)Math.Log((1024.0f / (1.0f + x)) - 1.0f))); }

            return float.NaN;
        }

        private void cbCurveViewer_SelectedIndexChanged(object sender, EventArgs e)
        {
            pbCurve.Refresh();
        }

        private void btnLoadSelectedPad_Click(object sender, EventArgs e)
        {
            pbLoad.Value = 0;

            //richiedo tutti i setting per il pad selezionato
            UtilityMIDI.MIDI_SysEx(0x02, (byte)DrumMap[lbPads.SelectedIndex].Head, 0x7F, 0x00);
            UtilityMIDI.MIDI_SysEx(0x02, (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x7F, 0x00);
        }

        private void btnLoadGeneral_Click(object sender, EventArgs e)
        {
            pbLoad.Value = 0;

            //richiedo tutti i setting per le impostazioni generali
            UtilityMIDI.MIDI_SysEx(0x02, 0x7E, 0x7F, 0x00);
        }

        private void loadTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabForm.Singleton != null) { TabForm.Singleton.BringToFront(); return; }

            tabForm = new TabForm();
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Select TAB:";
            openFile.Filter = "TAB Files (*.tab)|*.tab";

            if (MainForm.LastDirectoryUsed.ContainsKey("TabDir"))
                openFile.InitialDirectory = MainForm.LastDirectoryUsed["TabDir"];

            openFile.ShowDialog();

            if (File.Exists(openFile.FileName))
            {

                if (MainForm.LastDirectoryUsed.ContainsKey("TabDir"))
                    MainForm.LastDirectoryUsed["TabDir"] = Directory.GetParent(openFile.FileName).FullName;
                else
                    MainForm.LastDirectoryUsed.Add("TabDir", Directory.GetParent(openFile.FileName).FullName);


                tabForm.LoadTab(openFile.FileName);
                tabForm.Show();
            }
        }

        private void virtualInstrumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VirtualForm virtualForm = new VirtualForm(this);
            virtualForm.Show();
        }

        private void cbxOn_CheckedChanged(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx(0x6F, (byte)(cbxOn.Checked ? 0x01 : 0x00), 0x00, 0x00);
            cbxOn.Text = cbxOn.Checked ? "OFF" : "ON";
        }

        private void pbDiagnostic_Paint(object sender, PaintEventArgs e)
        {
            //Random r = new Random();
            for (int i = 0; i < 48; i++)
            {
                //Diagnostic[i] = (byte)r.Next(256);
                e.Graphics.FillRectangle(Brushes.SteelBlue, i * 8, 128 - Diagnostic[i] / 2, 8, Diagnostic[i] / 2);
                e.Graphics.DrawRectangle(Pens.SteelBlue, i * 8, 0, 8, 128);
                e.Graphics.DrawString(i.ToString(), new Font(this.Font.FontFamily, 5), Brushes.Red, i * 8, 117);
            }
        }

        private void btnResetDiagnostic_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 48; i++)
                Diagnostic[i] = 0;
        }

        private void btnHH_Click(object sender, EventArgs e)
        {
            /*for (int i = 0; i < DrumMap.Length; i++)
            {
                if (Setting[DrumMap[i].Head].Type == PinType.HHC && ((string)cbHH_HHC.SelectedItem) == DrumMap[i].Name)
                {
                    UtilityMIDI.SendSysEx(0x03, (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0A, DrumMap[i].Head);
                    break;
                }
            }*/
            //UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0A, Setting[DrumMap[lbPads.SelectedIndex].Head].Dual); //HHC

            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x00, ((Note)dudHH_A.SelectedItem).Value);
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x01, ((Note)dudHH_B.SelectedItem).Value);
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x02, ((Note)dudHH_C.SelectedItem).Value);
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x03, ((Note)dudHH_D.SelectedItem).Value);

            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x04, (byte)nudHH_AThresold.Value);
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x05, (byte)nudHH_BThresold.Value);
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x06, (byte)nudHH_CThresold.Value);
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x07, (byte)nudHH_DThresold.Value);

            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x08, ((Note)dudHH_FootSplashNote.SelectedItem).Value);
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x09, ((Note)dudHH_FootCloseNote.SelectedItem).Value);
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x0A, (byte)nudHH_FootSplashThresold.Value);
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), 0x4C, 0x0B, (byte)nudHH_FootCloseThresold.Value);
        }

        private void cbHH_HHC_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < DrumMap.Length; i++)
            {
                if (Setting[DrumMap[i].Head].Type == PinType.HHC && ((string)cbHH_HHC.SelectedItem) == DrumMap[i].Name)
                    Setting[DrumMap[lbPads.SelectedIndex].Head].Dual = DrumMap[i].Head;
            }
            //Setting[DrumMap[lbPads.SelectedIndex].Head].Dual
        }

        private void cbHH_HHC_DropDown(object sender, EventArgs e)
        {
            cbHH_HHC.Items.Clear();
            for (int i = 0; i < DrumMap.Length; i++)
            {
                if (Setting[DrumMap[i].Head].Type == PinType.HHC)
                    cbHH_HHC.Items.Add(DrumMap[i].Name);
            }

        }
        private void dudHH_A_SelectedItemChanged(object sender, EventArgs e)
        {
            hhSetting.A_Note = ((Note)dudHH_A.SelectedItem).Value;
        }

        private void dudHH_B_SelectedItemChanged(object sender, EventArgs e)
        {
            hhSetting.B_Note = ((Note)dudHH_B.SelectedItem).Value;
        }

        private void dudHH_C_SelectedItemChanged(object sender, EventArgs e)
        {
            hhSetting.C_Note = ((Note)dudHH_C.SelectedItem).Value;
        }

        private void dudHH_D_SelectedItemChanged(object sender, EventArgs e)
        {
            hhSetting.D_Note = ((Note)dudHH_D.SelectedItem).Value;
        }

        private void nudHH_AThresold_ValueChanged(object sender, EventArgs e)
        {
            hhSetting.A_Thresold = (byte)nudHH_AThresold.Value;
        }

        private void nudHH_BThresold_ValueChanged(object sender, EventArgs e)
        {
            hhSetting.B_Thresold = (byte)nudHH_BThresold.Value;
        }

        private void nudHH_CThresold_ValueChanged(object sender, EventArgs e)
        {
            hhSetting.C_Thresold = (byte)nudHH_CThresold.Value;
        }

        private void nudHH_DThresold_ValueChanged(object sender, EventArgs e)
        {
            hhSetting.D_Thresold = (byte)nudHH_DThresold.Value;
        }


        private void DoFlames()
        {
            // Seeding part. When we draw the initial red/yellow pixels
            // at the bottom of the screen, we need to delete them each
            // time through so that we don't end up with a single red
            // line at the bottom.
            grap.DrawLine(Pens.Black, 0, fHeight - 1, grapBitmap.Width, fHeight - 1);

            // Initialize the FastBitmap class
            fb = new FastBitmap(grapBitmap);

            // Draw our initial pixels. To make the fire look a little
            // more interesting, we'll mix red and yellow. We'll draw two
            // right next to each other, then move up one pixel and draw
            // them again. So each 'coal' will be a four pixel box.
            // Start at fHeight - 5 for no good reason!
            for (int x = 0; x < fWidth - 1; x += 4)
            {
                int RandomNumber = RandomClass.Next(0, fWidth - 1);
                fb.SetPixel(RandomNumber, fHeight - 1, Color.Yellow);
                fb.SetPixel(RandomNumber + 1, fHeight - 1, Color.Red);
                fb.SetPixel(RandomNumber, fHeight - 2, Color.Yellow);
                fb.SetPixel(RandomNumber + 1, fHeight - 2, Color.Red);
            }

            // Get and set each pixel on the screen.
            for (int y = fHeight - 1; y > 1; y--)
            {
                //Application.DoEvents();

                for (int x = 1; x < fWidth - 1; x++)
                {
                    // Get the color of the pixels. We'll grab
                    // four at a time and average them. Our fire
                    // will look different depending on how many
                    // we're averaging at a time.
                    Color c = fb.GetPixel(x, y);
                    Color d = fb.GetPixel(x, y - 1);
                    Color e = fb.GetPixel(x - 1, y);
                    Color r = fb.GetPixel(x + 1, y - 1);

                    // Add 'em and divide by four. Our RGB is
                    // a set of integer values we'll use in the
                    // FromArgb call below.
                    int rR = ((c.R + d.R + e.R + r.R) / 4);
                    int rG = ((c.G + d.G + e.G + r.G) / 4);
                    int rB = ((c.B + d.B + e.B + r.B) / 4);

                    // Now put them back one row up from where we
                    // got them!
                    fb.SetPixel(x, y - 1, Color.FromArgb(rR, rG, rB));
                }
            }

            // Unlock the FastBitmap since we're done with our pixel work
            fb.Release();

        }

        private void cbFire_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFire.Checked)
            {
                rightBrush = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Wave, Color.Yellow, Color.Red);
                leftBrush = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Wave, Color.Red, Color.Yellow);
            }
            else
            {
                rightBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((pbMIDI.Height / 2) - 20, 0),
                    new Point((pbMIDI.Height / 2) + 200, 0), Color.FromArgb(70, 130, 180), Color.FromArgb(176, 196, 222));
                leftBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point((pbMIDI.Height / 2) + 20, 0),
                    new Point((pbMIDI.Height / 2) - 200, 0), Color.FromArgb(70, 130, 180), Color.FromArgb(176, 196, 222));
            }

        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Metto il modulo in standby
            tcMain.SelectedIndex = 0;
            pbLoad.Value = 0;
            tcMain.Enabled = false;

            for (byte i = 0; i < Setting.Length; i++)
            {
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x00, Setting[i].Note);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x01, Setting[i].Thresold);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x02, Setting[i].ScanTime);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x03, Setting[i].MaskTime);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x04, Setting[i].Retrigger);
                Thread.Sleep(100);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x05, Setting[i].Curve);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x06, Setting[i].Xtalk);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x07, Setting[i].XtalkGroup);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x08, Setting[i].CurveForm);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x09, Setting[i].Choke);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x0A, Setting[i].Dual);
                Thread.Sleep(100);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x0B, Setting[i].DualNote);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x0C, Setting[i].DualThresold);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x0D, (byte)Setting[i].Type);
                UtilityMIDI.MIDI_SysEx(0x04, i, 0x0E, Setting[i].Channel);

                Thread.Sleep(100);
                UpdateProgressBar(false);
            }

            UtilityMIDI.MIDI_SysEx(0x04, 0x7E, 0x00, gSetting.DelayTime);
            UtilityMIDI.MIDI_SysEx(0x04, 0x7E, 0x02, (byte)(gSetting.NSensor / 8));
            UtilityMIDI.MIDI_SysEx(0x04, 0x7E, 0x03, gSetting.Xtalk);

            Thread.Sleep(100);
            UpdateProgressBar(false);

            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x00, hhSetting.A_Note);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x01, hhSetting.B_Note);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x02, hhSetting.C_Note);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x03, hhSetting.D_Note);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x04, hhSetting.A_Thresold);
            Thread.Sleep(100);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x05, hhSetting.B_Thresold);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x06, hhSetting.C_Thresold);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x07, hhSetting.D_Thresold);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x08, hhSetting.FootSplashNote);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x09, hhSetting.FootCloseNote);
            Thread.Sleep(100);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x0A, hhSetting.FootSplashThresold);
            UtilityMIDI.MIDI_SysEx(0x04, 0x4C, 0x0B, hhSetting.FootCloseThresold);

            UpdateProgressBar(false);

            //Questa chiamata permette di abilitare il loadAll se stiamo facendo il primo salvataggio o se abbiamo aggiornato la versione
            UtilityMIDI.MIDI_SysEx(0x04, 0x7E, 0x01, 0x08);//Version

            tcMain.Enabled = true;
            UpdateProgressBar(true);
        }

        private void saveSelectedPadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveGeneralToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void selectedToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnPadSend_Click(object sender, EventArgs e)
        {

        }

        private void nudHH_FootCloseThresold_ValueChanged(object sender, EventArgs e)
        {
            hhSetting.FootCloseThresold = (byte)nudHH_FootCloseThresold.Value;
        }

        private void nudHH_FootSplashThresold_ValueChanged(object sender, EventArgs e)
        {
            hhSetting.FootSplashThresold = (byte)nudHH_FootSplashThresold.Value;
        }

        private void dudHH_FootCloseNote_SelectedItemChanged(object sender, EventArgs e)
        {
            hhSetting.FootCloseNote = ((Note)dudHH_FootCloseNote.SelectedItem).Value;
        }

        private void dudHH_FootSplashNote_SelectedItemChanged(object sender, EventArgs e)
        {
            hhSetting.FootSplashNote = ((Note)dudHH_FootSplashNote.SelectedItem).Value;
        }

        private void btnEEPROMBackup1_Click(object sender, EventArgs e)
        {
            txtEEPROM.Text = "";

            for (int i = 0; i < 256; i++)
            {
                UtilityMIDI.MIDI_SysEx(0x61, /*(byte)j*/0, (byte)i, 0x00);
                System.Threading.Thread.Sleep(100);
            }
        }

        private void btnLoadSFZ_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Select SFZ:";
            open.Filter = "SFZ Files (*.sfz)|*.sfz";
            open.ShowDialog();

            if (!File.Exists(open.FileName)) return;

            tvSFZ.Nodes.Clear();

            try
            {
                UtilityAudio.OpenAudio(AudioLibrary.NAudio, eccGeneral.EffectChain);
                UtilityAudio.StopAudio();

                sfz = UtilityAudio.LoadSFZ(open.FileName);
                sfz.SFZUpdated += new SFZ.SFZUpdatedEventHandler(SFZUpdated);

                txtSFZFile.Text = open.FileName;

                foreach (SFZGroup Group in sfz.Groups)
                {
                    TreeNode node = new TreeNode();
                    node.Tag = Group;
                    node.Text = Group.ToString();
                    foreach (SFZRegion Region in Group.Regions)
                    {
                        TreeNode child = new TreeNode();
                        child.Tag = Region;
                        child.Text = Region.ToString();
                        if (Region.IDEffectChain == 0)
                            child.BackColor = Color.White;
                        else child.BackColor = Color.FromArgb(255, 255, 50 * Region.IDEffectChain);

                        node.Nodes.Add(child);
                    }
                    tvSFZ.Nodes.Add(node);
                }
                UtilityAudio.StartAudio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (tvSFZ.SelectedNode == null) return;
            tvSFZ.BackColor = Color.Red;
            sfz.Learn((SFZRegion)tvSFZ.SelectedNode.Tag);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            txtSFZFile.Text = "";
            tvSFZ.Nodes.Clear();
            sfz.Dispose();
        }

        private void tvSFZ_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is SFZGroup) { lblFilePath.Text = "---"; lblInfo.Text = "---"; cbEffectChain.SelectedIndex = -1; }
            else if (e.Node.Tag is SFZRegion)
            {
                lblFilePath.Text = ((SFZRegion)e.Node.Tag).SamplePath;
                lblInfo.Text = UtilityAudio.GetSampleInfo(((SFZRegion)e.Node.Tag).ID);
                cbEffectChain.SelectedIndex = ((SFZRegion)e.Node.Tag).IDEffectChain;
            }
        }

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

            if (MainForm.LastDirectoryUsed.ContainsKey("MP3Dir"))
                fileDialog.InitialDirectory = MainForm.LastDirectoryUsed["MP3Dir"];

            fileDialog.ShowDialog();

            if (String.IsNullOrEmpty(fileDialog.FileName)) return;

            if (MainForm.LastDirectoryUsed.ContainsKey("MP3Dir"))
                MainForm.LastDirectoryUsed["MP3Dir"] = Directory.GetParent(fileDialog.FileName).FullName;
            else
                MainForm.LastDirectoryUsed.Add("MP3Dir", Directory.GetParent(fileDialog.FileName).FullName);

            UtilityAudio.OpenAudio(AudioLibrary.NAudio, eccGeneral.EffectChain);
            UtilityAudio.LoadMP3(fileDialog.FileName);
            UtilityAudio.StartAudio();

            tslTotalTime.Text = " / " + UtilityAudio.GetMp3TotalTime().ToString();
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
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

        private void tvSFZ_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!(e.Node.Tag is SFZGroup))
            {
                ((SFZRegion)e.Node.Tag).Play(((SFZRegion)e.Node.Tag).hivel);
            }
        }

        private void keyboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            keyForm.Draw(DrumMap, Setting, hhSetting);
            keyForm.Show();
            keyForm.Location = new Point(this.Location.X + this.Size.Width + 10, this.Location.Y);
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            //keyForm.Location = new Point(this.Location.X + this.Size.Width+10, this.Location.Y);

        }

        private void SFZUpdated()
        {
            tvSFZ.BackColor = Color.White;
            tvSFZ.Nodes.Clear();
            foreach (SFZGroup Group in sfz.Groups)
            {
                TreeNode node = new TreeNode();
                node.Tag = Group;
                node.Text = Group.ToString();
                foreach (SFZRegion Region in Group.Regions)
                {
                    TreeNode child = new TreeNode();
                    child.Tag = Region;
                    child.Text = Region.ToString();
                    node.Nodes.Add(child);
                }
                tvSFZ.Nodes.Add(node);
            }
        }

        private void tscMIDIIN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UtilityMIDI.PortName(MIDIType.MIDI_IN) != (string)tscMIDIIN.SelectedItem)
            {
                UtilityMIDI.Close(MIDIType.MIDI_IN);

                if (!UtilityMIDI.OpenMIDI(MIDIType.MIDI_IN, (string)tscMIDIIN.SelectedItem))
                    tscMIDIIN.Text = "Select MIDI_IN...";
            }
        }

        private void ViewToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            tcMain.TabPages.Clear();

            if (configurationToolStripMenuItem.Checked)
                tcMain.TabPages.Add(TabPages[(int)Tabs.Configuration]);

            if (toolToolStripMenuItem.Checked)
                tcMain.TabPages.Add(TabPages[(int)Tabs.Tools]);

            if (monitorToolStripMenuItem.Checked)
                tcMain.TabPages.Add(TabPages[(int)Tabs.Monitor]);

            if (sFZToolStripMenuItem.Checked)
                tcMain.TabPages.Add(TabPages[(int)Tabs.Sfz]);

            if (effectsToolStripMenuItem.Checked)
                tcMain.TabPages.Add(TabPages[(int)Tabs.Effects]);
        }

        private void cbEffectChain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tvSFZ.SelectedNode == null || cbEffectChain.SelectedIndex < 0) return;

            if (tvSFZ.SelectedNode.Tag is SFZGroup)
            {
                foreach (TreeNode node in tvSFZ.SelectedNode.Nodes)
                {
                    ((SFZRegion)node.Tag).IDEffectChain = cbEffectChain.SelectedIndex;
                    if (cbEffectChain.SelectedIndex != 0)
                        node.BackColor = Color.FromArgb(86 - 10 * cbEffectChain.SelectedIndex, 154, 50 * cbEffectChain.SelectedIndex);
                    else
                        node.BackColor = Color.White;

                    switch (cbEffectChain.SelectedIndex)
                    {
                        case 0: UtilityAudio.EffectSample(((SFZRegion)node.Tag).ID, null); break;
                        case 1: UtilityAudio.EffectSample(((SFZRegion)node.Tag).ID, eccEffect1.EffectChain); break;
                        case 2: UtilityAudio.EffectSample(((SFZRegion)node.Tag).ID, eccEffect2.EffectChain); break;
                        case 3: UtilityAudio.EffectSample(((SFZRegion)node.Tag).ID, eccEffect3.EffectChain); break;
                        case 4: UtilityAudio.EffectSample(((SFZRegion)node.Tag).ID, eccEffect4.EffectChain); break;
                        case 5: UtilityAudio.EffectSample(((SFZRegion)node.Tag).ID, eccEffect5.EffectChain); break;
                    }
                }
            }
            else
            {
                ((SFZRegion)tvSFZ.SelectedNode.Tag).IDEffectChain = cbEffectChain.SelectedIndex;
                if (cbEffectChain.SelectedIndex != 0)
                    tvSFZ.SelectedNode.BackColor = Color.FromArgb(86 - 10 * cbEffectChain.SelectedIndex, 154, 50 * cbEffectChain.SelectedIndex);
                else
                    tvSFZ.SelectedNode.BackColor = Color.White;

                switch (cbEffectChain.SelectedIndex)
                {
                    case 0: UtilityAudio.EffectSample(((SFZRegion)tvSFZ.SelectedNode.Tag).ID, null); break;
                    case 1: UtilityAudio.EffectSample(((SFZRegion)tvSFZ.SelectedNode.Tag).ID, eccEffect1.EffectChain); break;
                    case 2: UtilityAudio.EffectSample(((SFZRegion)tvSFZ.SelectedNode.Tag).ID, eccEffect2.EffectChain); break;
                    case 3: UtilityAudio.EffectSample(((SFZRegion)tvSFZ.SelectedNode.Tag).ID, eccEffect3.EffectChain); break;
                    case 4: UtilityAudio.EffectSample(((SFZRegion)tvSFZ.SelectedNode.Tag).ID, eccEffect4.EffectChain); break;
                    case 5: UtilityAudio.EffectSample(((SFZRegion)tvSFZ.SelectedNode.Tag).ID, eccEffect5.EffectChain); break;
                }
            }
        }

        private void tscMIDIOUT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UtilityMIDI.PortName(MIDIType.MIDI_OUT) != (string)tscMIDIOUT.SelectedItem)
            {
                UtilityMIDI.Close(MIDIType.MIDI_OUT);

                if (!UtilityMIDI.OpenMIDI(MIDIType.MIDI_OUT, (string)tscMIDIOUT.SelectedItem))
                    tscMIDIOUT.Text = "Select MIDI_OUT...";
            }
        }

        private void selectComToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (selectComToolStripMenuItem.Checked)
            {
                tscCOM.Enabled = true;
            }
            else
            {
                UtilityMIDI.Close(MIDIType.Serial);
                tscCOM.Enabled = false;
            }
        }

        private void mIDIINToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (mIDIINToolStripMenuItem.Checked)
                tscMIDIIN.Enabled = true;
            else
            {
                UtilityMIDI.Close(MIDIType.MIDI_IN);
                tscMIDIIN.Enabled = false;
            }
        }

        private void mIDIOUTToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (mIDIOUTToolStripMenuItem.Checked)
                tscMIDIOUT.Enabled = true;
            else
            {
                UtilityMIDI.Close(MIDIType.MIDI_OUT);
                tscMIDIOUT.Enabled = false;
            }
        }

        private void WriteSysEx(FileStream fs, byte[] Sysex)
        {
            foreach (byte b in Sysex)
                fs.WriteByte(b);
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Select export file:";
            saveFile.Filter = "SysEx Files (*.syx)|*.syx";
            saveFile.AddExtension = true;
            DialogResult res = saveFile.ShowDialog();

            if (res != DialogResult.OK) return;
            if (saveFile.FilterIndex == 0) //SYX
            {
                FileStream fs = File.Create(saveFile.FileName);
                WriteSysEx(fs, new byte[] { 0xF0, 0x77, 0x03, 0x7E, 0x00, gSetting.DelayTime, 0xF7 });
                /*UtilityIniFile.SetIniString("General", "Delay", gSetting.DelayTime.ToString(), SettingPath);
                //UtilityIniFile.SetIniString("General", "HHC1", gSetting.HHC1.ToString(), SettingPath);
                //UtilityIniFile.SetIniString("General", "HHC2", gSetting.HHC2.ToString(), SettingPath);
                UtilityIniFile.SetIniString("General", "Xtalk", gSetting.Xtalk.ToString(), SettingPath);


                UtilityIniFile.SetIniString("HH", "A", hhSetting.A_Note.ToString() + ";" + hhSetting.A_Thresold.ToString(), SettingPath);
                UtilityIniFile.SetIniString("HH", "B", hhSetting.B_Note.ToString() + ";" + hhSetting.B_Thresold.ToString(), SettingPath);
                UtilityIniFile.SetIniString("HH", "C", hhSetting.C_Note.ToString() + ";" + hhSetting.C_Thresold.ToString(), SettingPath);
                UtilityIniFile.SetIniString("HH", "D", hhSetting.D_Note.ToString() + ";" + hhSetting.D_Thresold.ToString(), SettingPath);

                UtilityIniFile.SetIniString("HH", "FootClose", hhSetting.FootCloseNote.ToString() + ";" + hhSetting.FootCloseThresold.ToString(), SettingPath);
                UtilityIniFile.SetIniString("HH", "FootSplash", hhSetting.FootSplashNote.ToString() + ";" + hhSetting.FootSplashThresold.ToString(), SettingPath);


                UtilityIniFile.DeleteSection("DrumMap", SettingPath);
                for (int i = 0; i < DrumMap.Length; i++)
                {
                    UtilityIniFile.SetIniString("DrumMap", DrumMap[i].Name, DrumMap[i].Head.ToString() + ";" + DrumMap[i].Rim.ToString() + ";" + (DrumMap[i].Single ? "Y" : "N"), SettingPath);
                }
                for (int i = 0; i < Setting.Length; i++)
                {
                    UtilityIniFile.SetIniString("PinSetting", "Pin" + i.ToString(), Setting[i].ToString(), SettingPath);
                }*/
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lbLog.Items.Clear();
        }

        private void btnDualPiezoSwitch_Click(object sender, EventArgs e)
        {

            if (chkDualPiezoSwitch.Checked)
            {
                //Se attivo abilita il Piezo-Switch lato Piezo altrimenti il piezo suonerà come uno normale
                if (chkPiezoSuppression.Checked)
                {
                    UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0A, (byte)DrumMap[lbPads.SelectedIndex].Rim);
                    UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0A, (byte)DrumMap[lbPads.SelectedIndex].Head);
                }
                else
                {
                    UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0A, 127);
                    UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0A, 127);
                }

                //UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0B, ((Note)dudDualD.SelectedItem).Value);
                //UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0C, (byte)tbHead.Value);

                //se è un YSwitch B e C sono impostati da un'altra parte mentre A non credo serva ma devo controllare
                if (lblAlternative.Visible)//Se è visibile ci troviamo in un Piezo-Switch altrimenti è un Piezo-YSwitch
                {
                    UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0B, ((Note)dudAlternativeNote.SelectedItem).Value);
                    UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0C, (byte)(nudAlternativeThresold.Value));
                }
            }
            else
            {
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0A, 127);
                UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0A, 127);
            }
        }

        private void dudAlternativeNote_SelectedItemChanged(object sender, EventArgs e)
        {
            if (dudAlternativeNote.SelectedItem != null)
                Setting[DrumMap[lbPads.SelectedIndex].Rim].DualNote = ((Note)dudAlternativeNote.SelectedItem).Value;
        }

        private void chkDualPiezoSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDualPiezoSwitch.Checked)
            {
                nudAlternativeThresold.Enabled = true;
                dudAlternativeNote.Enabled = true;
                chkPiezoSuppression.Enabled = true;

                //Setting[DrumMap[lbPads.SelectedIndex].Head].Dual = DrumMap[lbPads.SelectedIndex].Rim;
                Setting[DrumMap[lbPads.SelectedIndex].Rim].Dual = DrumMap[lbPads.SelectedIndex].Head;
            }
            else
            {
                nudAlternativeThresold.Enabled = false;
                dudAlternativeNote.Enabled = false;
                chkPiezoSuppression.Enabled = false;

                Setting[DrumMap[lbPads.SelectedIndex].Head].Dual = 127;
                Setting[DrumMap[lbPads.SelectedIndex].Rim].Dual = 127;

            }

            UpdateSetting();
        }

        private void nudAlternativeThresold_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].DualThresold = (byte)nudAlternativeThresold.Value;
        }

        private void btnOpenThresoldRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0C, (byte)nudOpenThresoldRim.Value);
        }

        private void btnOpenNoteRim_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Rim, 0x0B, ((Note)dudOpenNoteRim.SelectedItem).Value);
        }

        private void dudOpenNoteRim_SelectedItemChanged(object sender, EventArgs e)
        {
            if (dudOpenNoteRim.SelectedIndex >= 0)
                Setting[DrumMap[lbPads.SelectedIndex].Rim].DualNote = ((Note)dudOpenNoteRim.SelectedItem).Value;
        }

        private void nudOpenThresoldRim_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Rim].DualThresold = (byte)nudOpenThresoldRim.Value;
        }

        private void btnOpenThresoldHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0C, (byte)nudOpenThresoldHead.Value);
        }

        private void btnOpenNoteHead_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x0B, ((Note)dudOpenNoteHead.SelectedItem).Value);
        }

        private void nudOpenThresoldHead_ValueChanged(object sender, EventArgs e)
        {
            Setting[DrumMap[lbPads.SelectedIndex].Head].DualThresold = (byte)nudOpenThresoldHead.Value;
        }

        private void dudOpenNoteHead_SelectedItemChanged(object sender, EventArgs e)
        {
            if (dudOpenNoteHead.SelectedIndex >= 0)
                Setting[DrumMap[lbPads.SelectedIndex].Head].DualNote = ((Note)dudOpenNoteHead.SelectedItem).Value;

        }

        private void multiSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MultiSettingForm msf = new MultiSettingForm();
            msf.ShowDialog();
        }

        private void chkPiezoSuppression_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPiezoSuppression.Checked)
                Setting[DrumMap[lbPads.SelectedIndex].Head].Dual = DrumMap[lbPads.SelectedIndex].Rim;
            else
                Setting[DrumMap[lbPads.SelectedIndex].Head].Dual = 127;
        }

        private void btnCCHHC_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x00, (byte)nudCCHHC.Value);

        }

        private void btnMaskTimeHHC_Click(object sender, EventArgs e)
        {
            UtilityMIDI.MIDI_SysEx((byte)(0x03 + (alwaySaveToolStripMenuItem.Checked ? 0x01 : 0x00)), (byte)DrumMap[lbPads.SelectedIndex].Head, 0x03, (byte)nudMaskTimeHHC.Value);

        }

        private void btnExportSFZ_Click(object sender, EventArgs e)
        {
            /*string[] SDCards = UtilitySDCard.GetSDCards();
            UtilitySDCard.Open(SDCards[0]);
            byte[] buffer = UtilitySDCard.ReadSector(1);

            string str_buffer = "";
            for (int i = 0; i < buffer.Length; i++)
                str_buffer += String.Format("{0:X2} ", buffer[i]);

            MessageBox.Show(str_buffer.ToString());
            */
            /*for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)(Math.Sin(i)*byte.MaxValue);
            UtilitySDCard.WriteSector(buffer, 1);
            */
            /*buffer = UtilitySDCard.ReadSector(1);

            str_buffer = "";
            for (int i = 0; i < buffer.Length; i++)
                str_buffer += String.Format("{0:X2} ", buffer[i]);

            MessageBox.Show(str_buffer.ToString());*/

        }

        private void errorReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportErrorForm reperr = new ReportErrorForm();
            reperr.Show();
        }

        private void checkVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternetUpdateForm internet = new InternetUpdateForm();
            internet.ShowDialog();

        }

        private void showNewControlToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (showNewControlToolStripMenuItem.Checked) ActivateNewControl(this);
            else DeactivateNewControl(this);
        }
        private void ActivateNewControl(Control Parent)
        {
            foreach (Control c in Parent.Controls)
            {
                if (c is microDrum.Slider) ((microDrum.Slider)c).ShowSlider = true;
                else if (c is microDrum.CurveSlider) ((microDrum.CurveSlider)c).ShowSlider = true;
                else if (c is microDrum.NoteSlider) ((microDrum.NoteSlider)c).ShowSlider = true;
                ActivateNewControl(c);
            }
        }
        private void DeactivateNewControl(Control Parent)
        {
            foreach (Control c in Parent.Controls)
            {
                if (c is microDrum.Slider) ((microDrum.Slider)c).ShowSlider = false;
                else if (c is microDrum.CurveSlider) ((microDrum.CurveSlider)c).ShowSlider = false;
                else if (c is microDrum.NoteSlider) ((microDrum.NoteSlider)c).ShowSlider = false;
                DeactivateNewControl(c);
            }
        }

        private void channelMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChannelForm cf = new ChannelForm();
            cf.Set(Setting, DrumMap);
            cf.ShowDialog();
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            LogForm lf = new LogForm();
            lf.Logs.Clear();

            foreach (Log s in lbLog.Items)
            {
                lf.Logs.Add(s);
            }

            lf.ShowDialog();
        }

        private void lbMIDI_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((((MIDI)lbMIDI.SelectedItem).Cmd & 0xF0) == 0x90) UtilityAudio.MIDI_NoteOn(((MIDI)lbMIDI.SelectedItem).Data1, ((MIDI)lbMIDI.SelectedItem).Data2);
        }

        private void noteMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nmForm.Show();
        }

        private void btnGetProfiling_Click(object sender, EventArgs e)
        {
            UtilitySetting.Profiling(false);
        }

        private void btnResetProfiling_Click(object sender, EventArgs e)
        {
            UtilitySetting.Profiling(true);
        }

        private void nudNSensor_ValueChanged(object sender, EventArgs e)
        {
            gSetting.NSensor = (byte)nudNSensor.Value;
        }

        private void generalToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnNSensor_Click(object sender, EventArgs e)
        {
            if (alwaySaveToolStripMenuItem.Checked)
                UtilitySetting.SaveGeneralParam(uDrumGeneralParam.NSensor, (byte)(gSetting.NSensor / 8));
            else
                UtilitySetting.SetGeneralParam(uDrumGeneralParam.NSensor, (byte)(gSetting.NSensor / 8));
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            System.IO.Pipes.NamedPipeClientStream MIDIPipe = new System.IO.Pipes.NamedPipeClientStream("MIDIPipe");
            MIDIPipe.Connect();

            int MIDI = 0;
            while ((MIDI=MIDIPipe.ReadByte()) > 0)
            {
                if ((MIDI & 0xF0) == 0x90)//NoteOn
                {
                    UtilityAudio.MIDI_NoteOn((byte)MIDIPipe.ReadByte(),(byte) MIDIPipe.ReadByte());
                }
            }
        }

        private void dkitdsndSFZToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void chkRecordLog_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecordLog.Checked)
            {
                LogRecording.Clear();

            }
            else
            {
                uint startTime = LogRecording[0].Time;
                foreach(Log l in LogRecording)
                {
                    File.AppendAllText(Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "LogData.txt", ((float)l.Time-startTime) + " " + ((float)l.Reading) / 21.0f + " " + ((float)l.Y0)/21.0f+"\r\n");
                }


            }
        }



    }
}
