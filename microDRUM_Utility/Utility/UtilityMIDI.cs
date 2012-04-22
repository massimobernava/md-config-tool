using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using NAudio;
using NAudio.Midi;

namespace microDrum
{
    public struct Log
    {
        public byte Sensor;
        public int N;
        public uint Time;
        public int Reading;
        public int MaxReading;
        public int Y0;
        public byte State;

        public override string ToString()
        {
            string s = "N";
            if (State == 1) s = "S";
            else if (State == 2) s = "M";

            return N.ToString() + ".(" + Sensor.ToString() + "," + Time.ToString() + "," + s + ")->" + Reading.ToString() + ":" + Y0.ToString() + ":" + MaxReading.ToString();
        }
    }

    public enum MIDIType
    {
        Serial,
        MIDI_IN,
        MIDI_OUT
    }
    public static class UtilityMIDI
    {
        //private static MIDIType midiType = MIDIType.Null;
        public static EventHandler MIDIClosed = null;
        private static MidiInEx midiIn = null;
        private static string midiIn_Name = null;
        private static MidiOutEx midiOut = null;
        private static string midiOut_Name = null;

        /* public static MIDIType Type
         {
             get { return midiType; }
         }*/
        public class MIDIDataReceivedEventArgs : EventArgs
        {
            public byte Cmd;
            public byte Data1;
            public byte Data2;
            public byte Data3;
            public Log Log;
        }
        public delegate void MIDIDataReceivedEventHandler(MIDIDataReceivedEventArgs e);

        public static MIDIDataReceivedEventHandler MIDIReceived_SysEx;
        public static MIDIDataReceivedEventHandler MIDIReceived_NoteOn;
        public static MIDIDataReceivedEventHandler MIDIReceived_CC;

        public static bool OpenMIDI(MIDIType Type, string Data)
        {
            //midiType = Type;
            try
            {
                if (Type == MIDIType.Serial)
                {
                    UtilitySerial.OpenCOM(Data);

                    if (UtilitySerial.IsOpen)
                    {
                        UtilitySerial.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                        return true;
                    }
                }
                else if (Type == MIDIType.MIDI_IN)
                {
                    for (int i = 0; i < MidiInEx.NumberOfDevices; i++)
                        if (MidiInEx.DeviceInfo(i).ProductName == Data)
                        {
                            midiIn = new MidiInEx(i);
                            midiIn.ErrorReceived += new EventHandler<MidiInMessageEventArgs>(midiIn_ErrorReceived);
                            midiIn.MessageReceived += new EventHandler<MidiInMessageEventArgs>(midiIn_MessageReceived);
                            midiIn.Start();
                            midiIn_Name = MidiInEx.DeviceInfo(i).ProductName;
                            return true;
                        }
                }
                else if (Type == MIDIType.MIDI_OUT)
                {
                    for (int i = 0; i < MidiOutEx.NumberOfDevices; i++)
                        if (MidiOutEx.DeviceInfo(i).ProductName == Data)
                        {
                            midiOut = new MidiOutEx(i);
                            midiOut_Name = MidiOutEx.DeviceInfo(i).ProductName;
                            return true;
                        }
                }
                return false;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        static void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static string[] GetMIDIDevices(MIDIType Type)
        {
            List<string> Devices = new List<string>();
            if (Type == MIDIType.Serial) return UtilitySerial.GetPortNames();
            else if (Type == MIDIType.MIDI_IN)
            {
                for (int i = 0; i < MidiInEx.NumberOfDevices; i++)
                    Devices.Add(MidiInEx.DeviceInfo(i).ProductName);

                if (Devices.Count == 0) return null;
            }
            else if (Type == MIDIType.MIDI_OUT)
            {
                for (int i = 0; i < MidiOutEx.NumberOfDevices; i++)
                    Devices.Add(MidiOutEx.DeviceInfo(i).ProductName);

                if (Devices.Count == 0) return null;
            }

            return Devices.ToArray();
        }

        static void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
             if (e.MidiEvent is NAudio.Midi.NoteOnEvent && MIDIReceived_NoteOn != null)
            {
                MIDIDataReceivedEventArgs eMIDI = new MIDIDataReceivedEventArgs();
                eMIDI.Cmd = 0x99;
                eMIDI.Data1 = (byte)((NAudio.Midi.NoteOnEvent)e.MidiEvent).NoteNumber;
                eMIDI.Data2 = (byte)((NAudio.Midi.NoteOnEvent)e.MidiEvent).Velocity;
                MIDIReceived_NoteOn(eMIDI);
                UtilityAudio.MIDI_NoteOn(eMIDI.Data1, eMIDI.Data2);
            }
            else if (e.MidiEvent is NAudio.Midi.ControlChangeEvent && MIDIReceived_CC != null)
            {
                MIDIDataReceivedEventArgs eMIDI = new MIDIDataReceivedEventArgs();
                eMIDI.Cmd = 0xB9;
                eMIDI.Data1 = (byte)((NAudio.Midi.ControlChangeEvent)e.MidiEvent).Controller;
                eMIDI.Data2 = (byte)((NAudio.Midi.ControlChangeEvent)e.MidiEvent).ControllerValue;
                MIDIReceived_CC(eMIDI);
                UtilityAudio.MIDI_CC(eMIDI.Data1, eMIDI.Data2);
            }
            else if (e.MidiEvent is NAudio.Midi.SysexEvent && MIDIReceived_SysEx != null)
            {
                MIDIDataReceivedEventArgs eMIDI = new MIDIDataReceivedEventArgs();
                //e.RawMessage;//Meglio leggere direttamente questo...
                /*if(...)
                {
                    eMIDI.Cmd = 0xB9;
                    eMIDI.Data1 = (byte)((NAudio.Midi.SysexEvent)e.MidiEvent).;
                    eMIDI.Data2 = (byte)((NAudio.Midi.SysexEvent)e.MidiEvent).ControllerValue;
                    MIDIReceived_SysEx(eMIDI);
                }*/
            }
        }
        static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (midiIn != null) return;

            while (UtilitySerial.BytesToRead > 0)
            {
                byte Start = (byte)UtilitySerial.ReadByte();

                if (Start == 0xF0)//SysEx
                {
                    byte ID = (byte)UtilitySerial.ReadByte();
                    byte Cmd = (byte)UtilitySerial.ReadByte();

                    MIDIDataReceivedEventArgs eMIDI = new MIDIDataReceivedEventArgs();
                    eMIDI.Cmd = Cmd;

                    if (Cmd == 0x6E)//LOG
                    {
                        byte Size = (byte)UtilitySerial.ReadByte();
                        byte[] Message = new byte[Size];
                        for (int i = 0; i < Size; i++)
                            Message[i] = (byte)UtilitySerial.ReadByte();
                        if (Message.Length < 4) return;
                        Log log = new Log();
                        log.Time = (uint)(Message[0] | (Message[1] << 8) | (Message[2] << 16) | (Message[3] << 24));
                        log.Sensor = Message[4];
                        log.N = Message[5] | (Message[6] << 8);
                        log.Reading = Message[7] | (Message[8] << 8);
                        log.Y0 = Message[9] | (Message[10] << 8);
                        log.MaxReading = Message[11] | (Message[12] << 8);
                        log.State = Message[13];
                        eMIDI.Log = log;
                    }
                    else if (Cmd == 0x6D)//PROFILING
                    {
                        byte Size = (byte)UtilitySerial.ReadByte();
                        byte[] Message = new byte[Size];
                        for (int i = 0; i < Size; i++)
                            Message[i] = (byte)UtilitySerial.ReadByte();
                        uint Time = (uint)(Message[0] | (Message[1] << 8) | (Message[2] << 16) | (Message[3] << 24));
                        uint N = (uint)(Message[4] | (Message[5] << 8) | (Message[6] << 16) | (Message[7] << 24));

                        System.Windows.Forms.MessageBox.Show("Time:" + Time.ToString() + " / N:" + N.ToString() + " = "+ (Time/N).ToString(),"Profiling");
                    }
                    else
                    {
                        eMIDI.Data1 = (byte)UtilitySerial.ReadByte();
                        eMIDI.Data2 = (byte)UtilitySerial.ReadByte();
                        eMIDI.Data3 = (byte)UtilitySerial.ReadByte();

                    }
                    byte End = (byte)UtilitySerial.ReadByte();

#if DEBUG
                    if (Cmd < 128 && eMIDI.Data1 < 128 && eMIDI.Data2 < 128 && eMIDI.Data3 < 128)
                        System.Diagnostics.Debug.WriteLine("<- 0x" + Cmd.ToString("X2") + " 0x" + eMIDI.Data1.ToString("X2") + " 0x" + eMIDI.Data2.ToString("X2") + " 0x" + eMIDI.Data3.ToString("X2"));
                    else
                        System.Diagnostics.Debug.WriteLine("<- ATTENZIONE:0x" + Cmd.ToString("X2") + " 0x" + eMIDI.Data1.ToString("X2") + " 0x" + eMIDI.Data2.ToString("X2") + " 0x" + eMIDI.Data3.ToString("X2"));
#endif
                    if (MIDIReceived_SysEx != null && ID == 0x77 && End == 0xF7)
                        MIDIReceived_SysEx(eMIDI);
                }
                else if ((Start & 0xF0)== 0x90)//NoteOn
                {
                    MIDIDataReceivedEventArgs eMIDI = new MIDIDataReceivedEventArgs();
                    eMIDI.Cmd = Start;
                    eMIDI.Data1 = (byte)UtilitySerial.ReadByte();
                    eMIDI.Data2 = (byte)UtilitySerial.ReadByte();

                    if (MIDIReceived_NoteOn != null)
                        MIDIReceived_NoteOn(eMIDI);

                    UtilityAudio.MIDI_NoteOn(eMIDI.Data1, eMIDI.Data2);
                }
                else if (Start == 0xB9)//CC
                {
                    MIDIDataReceivedEventArgs eMIDI = new MIDIDataReceivedEventArgs();
                    eMIDI.Cmd = 0xB9;
                    eMIDI.Data1 = (byte)UtilitySerial.ReadByte();
                    eMIDI.Data2 = (byte)UtilitySerial.ReadByte();

                    if (MIDIReceived_CC != null)
                        MIDIReceived_CC(eMIDI);

                    UtilityAudio.MIDI_CC(eMIDI.Data1, eMIDI.Data2);
                }
                //else continue;
            }
        }
        public static void MIDI_SysEx(byte cmd, byte data1, byte data2, byte data3)
        {
#if DEBUG
            if(cmd<128 && data1<128 && data2<128 && data3<128)
                System.Diagnostics.Debug.WriteLine("-> 0x" + cmd.ToString("X2") + " 0x" + data1.ToString("X2") + " 0x" + data2.ToString("X2") + " 0x" + data3.ToString("X2"));
            else
                System.Diagnostics.Debug.WriteLine("-> ATTENZIONE:0x" + cmd.ToString("X2") + " 0x" + data1.ToString("X2") + " 0x" + data2.ToString("X2") + " 0x" + data3.ToString("X2"));
#endif
            if (cmd >127 && data1 > 127 && data2 > 127 && data3 > 127) return;

            if (UtilitySerial.IsOpen)
                UtilitySerial.Write(new byte[] { 0xF0, 0x77, cmd, data1, data2, data3, 0xF7 });
            else if (midiOut!=null)
            {
                midiOut.SendLongMessage(new byte[] { 0xF0, 0x77, cmd, data1, data2, data3, 0xF7 });
            }
        }

        public static void MIDI_NoteOn(byte Note, byte Velocity)
        {
            if (UtilitySerial.IsOpen)
                UtilitySerial.Write(new byte[] { 0x90, Note, Velocity });
            else if (midiOut != null)
            {
                NAudio.Midi.NoteOnEvent noteOn = new NAudio.Midi.NoteOnEvent(10000, 10, Note, Velocity, 100);
                midiOut.SendMessage(noteOn.GetAsShortMessage());
            }

        }
        public static void DiscardAllBuffer(MIDIType Type)
        {
            if (Type == MIDIType.Serial)
                UtilitySerial.DiscardAllBuffer();
            else if (Type == MIDIType.MIDI_IN) midiIn.Reset();
            else if (Type == MIDIType.MIDI_OUT) midiOut.Reset();
        }
        public static bool IsOpen(MIDIType Type)
        {
            if (Type == MIDIType.Serial) return UtilitySerial.IsOpen;
            else if (Type == MIDIType.MIDI_IN) return midiIn != null;
            else if (Type == MIDIType.MIDI_OUT) return midiOut != null;
            return false;
        }
        public static string PortName(MIDIType Type)
        {
            if (Type == MIDIType.Serial) return UtilitySerial.COM_Name;
            else if (Type == MIDIType.MIDI_IN) return midiIn_Name;
            else if (Type == MIDIType.MIDI_OUT) return midiOut_Name;
            return string.Empty;
        }
        public static void Close(MIDIType Type)
        {
            if (Type == MIDIType.Serial) UtilitySerial.CloseCOM();
            else if (Type == MIDIType.MIDI_IN && midiIn != null) { midiIn.Stop(); midiIn.Close(); midiIn.Dispose(); midiIn = null; midiIn_Name = null; }
            else if (Type == MIDIType.MIDI_OUT && midiOut != null) { midiOut.Close(); midiOut.Dispose(); midiOut = null; midiOut_Name = null; }
        }
    }
}
