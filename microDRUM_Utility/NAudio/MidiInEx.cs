using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NAudio.Midi;
using NAudio;

namespace microDrum
{
    /// <summary>
    /// Represents a MIDI in device
    /// </summary>
    public class MidiInEx : IDisposable
    {
        public const int CALLBACK_FUNCTION = 0x00030000;

        public enum MidiInMessage
        {
            Open = 0x3C1,
            Close = 0x3C2,
            Data = 0x3C3,
            LongData = 0x3C4,
            Error = 0x3C5,
            LongError = 0x3C6,
            MoreData = 0x3CC,
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MIDIHDR
        {
            public string lpData;
            public int dwBufferLength;
            public int dwBytesRecorded;
            public int dwUser;
            public int dwFlags;
            public int lpNext;
            public int Reserved;
        }
        public delegate void MidiInCallback(IntPtr midiInHandle, MidiInMessage message, IntPtr userData, IntPtr messageParameter1, IntPtr messageParameter2);

        private IntPtr hMidiIn = IntPtr.Zero;
        private bool disposed = false;
        private MidiInCallback callback;

        /// <summary>
        /// Called when a MIDI message is received
        /// </summary>
        public event EventHandler<MidiInMessageEventArgs> MessageReceived;

        /// <summary>
        /// An invalid MIDI message
        /// </summary>
        public event EventHandler<MidiInMessageEventArgs> ErrorReceived;

        /// <summary>
        /// Gets the number of MIDI input devices available in the system
        /// </summary>
        public static int NumberOfDevices
        {
            get
            {
                return MidiInGetNumDevs();
            }
        }

        /// <summary>
        /// Opens a specified MIDI in device
        /// </summary>
        /// <param name="deviceNo">The device number</param>
        public MidiInEx(int deviceNo)
        {
            this.callback = new MidiInCallback(Callback);
            MmException.Try(MidiInOpen(out hMidiIn, deviceNo, this.callback, 0, CALLBACK_FUNCTION), "midiInOpen");
            //MidiInPrepareHeader(
            //MidiInAddBuffer(
        }

        /// <summary>
        /// Closes this MIDI in device
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Closes this MIDI in device
        /// </summary>
        public void Dispose()
        {
            GC.KeepAlive(callback);
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Start the MIDI in device
        /// </summary>
        public void Start()
        {
            MmException.Try(MidiInStart(hMidiIn), "midiInStart");
        }

        /// <summary>
        /// Stop the MIDI in device
        /// </summary>
        public void Stop()
        {
            MmException.Try(MidiInStop(hMidiIn), "midiInStop");
        }

        /// <summary>
        /// Reset the MIDI in device
        /// </summary>
        public void Reset()
        {
            MmException.Try(MidiInReset(hMidiIn), "midiInReset");
        }

        private void Callback(IntPtr midiInHandle, MidiInMessage message, IntPtr userData, IntPtr messageParameter1, IntPtr messageParameter2)
        {
            switch (message)
            {
                case MidiInMessage.Open:
                    // message Parameter 1 & 2 are not used
                    break;
                case MidiInMessage.Data:
                    // parameter 1 is packed MIDI message
                    // parameter 2 is milliseconds since MidiInStart
                    if (MessageReceived != null)
                    {
                        MessageReceived(this, new MidiInMessageEventArgs(messageParameter1.ToInt32(), messageParameter2.ToInt32()));
                    }
                    break;
                case MidiInMessage.Error:
                    // parameter 1 is invalid MIDI message
                    if (ErrorReceived != null)
                    {
                        ErrorReceived(this, new MidiInMessageEventArgs(messageParameter1.ToInt32(), messageParameter2.ToInt32()));
                    }
                    break;
                case MidiInMessage.Close:
                    // message Parameter 1 & 2 are not used
                    break;
                case MidiInMessage.LongData:
                    // parameter 1 is pointer to MIDI header
                    // parameter 2 is milliseconds since MidiInStart
                    /*
                    Me.MIDIInHdr.lpData = Me.ddd
Me.MIDIInHdr.dwBufferLength = Len(Me.ddd)
Me.MIDIInHdr.dwFlags = 0
Err = midiInPrepareHeader(hMidiIn, Me.MIDIInHdr, Len(Me.MIDIInHdr))
If Err <> C_MMSYSERR_NOERROR Then
Me.pLastError = Err
Debug.WriteLine(CStr(Err))
End If
Err = midiInAddBuffer(hMidiIn, Me.MIDIInHdr, Len(Me.MIDIInHdr))
If Err <> C_MMSYSERR_NOERROR Then
Me.pLastError = Err
Debug.WriteLine(CStr(Err))
End If*/
                    break;
                case MidiInMessage.LongError:
                    // parameter 1 is pointer to MIDI header
                    // parameter 2 is milliseconds since MidiInStart
                    break;
                case MidiInMessage.MoreData:
                    // parameter 1 is packed MIDI message
                    // parameter 2 is milliseconds since MidiInStart
                    break;
            }
        }

        /// <summary>
        /// Gets the MIDI in device info
        /// </summary>
        public static MidiInCapabilities DeviceInfo(int midiInDeviceNumber)
        {
            MidiInCapabilities caps = new MidiInCapabilities();
            int structSize = Marshal.SizeOf(caps);
            MmException.Try(MidiInGetDevCaps(midiInDeviceNumber, out caps, structSize), "midiInGetDevCaps");
            return caps;
        }

        /// <summary>
        /// Closes the MIDI out device
        /// </summary>
        /// <param name="disposing">True if called from Dispose</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                //if(disposing) Components.Dispose();
                MidiInClose(hMidiIn);
            }
            disposed = true;
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        ~MidiInEx()
        {
            System.Diagnostics.Debug.Assert(false, "MIDI In was not finalised");
            Dispose(false);
        }


        [DllImport("winmm.dll", EntryPoint = "midiConnect")]
        public static extern MmResult MidiConnect(IntPtr hMidiIn, IntPtr hMidiOut, int pReserved);

        [DllImport("winmm.dll", EntryPoint = "midiDisconnect")]
        public static extern MmResult MidiDisconnect(IntPtr hMidiIn, int hmo, int pReserved);

        [DllImport("winmm.dll", EntryPoint = "midiInAddBuffer")]
        public static extern MmResult MidiInAddBuffer(IntPtr hMidiIn, [MarshalAs(UnmanagedType.Struct)] ref microDrum.MidiInEx.MIDIHDR lpMidiInHdr, int uSize);

        [DllImport("winmm.dll", EntryPoint = "midiInClose")]
        public static extern MmResult MidiInClose(IntPtr hMidiIn);

        [DllImport("winmm.dll", EntryPoint = "midiInGetDevCaps", CharSet = CharSet.Auto)]
        public static extern MmResult MidiInGetDevCaps(int deviceId, out MidiInCapabilities capabilities, int size);

        [DllImport("winmm.dll", EntryPoint = "midiInGetErrorText")]
        public static extern MmResult MidiInGetErrorText(int err, string lpText, int uSize);

        [DllImport("winmm.dll", EntryPoint = "midiInGetID")]
        public static extern MmResult MidiInGetID(IntPtr hMidiIn, int lpuDeviceId);

        [DllImport("winmm.dll", EntryPoint = "midiInGetNumDevs")]
        public static extern int MidiInGetNumDevs();

        [DllImport("winmm.dll", EntryPoint = "midiInMsg")]
        public static extern MmResult MidiInMsg(IntPtr hMidiIn, int msg, int dw1, int dw2);

        [DllImport("winmm.dll", EntryPoint = "midiInOpen")]
        public static extern MmResult MidiInOpen(out IntPtr hMidiIn, int uDeviceID, microDrum.MidiInEx.MidiInCallback callback, int dwInstance, int dwFlags);

        [DllImport("winmm.dll", EntryPoint = "midiInOpen")]
        public static extern MmResult MidiInOpenWindow(out IntPtr hMidiIn, int uDeviceID, IntPtr callbackWindowHandle, int dwInstance, int dwFlags);

        [DllImport("winmm.dll", EntryPoint = "midiInPrepareHeader")]
        public static extern MmResult MidiInPrepareHeader(IntPtr hMidiIn, [MarshalAs(UnmanagedType.Struct)] ref microDrum.MidiInEx.MIDIHDR lpMidiInHdr, int uSize);

        [DllImport("winmm.dll", EntryPoint = "midiInReset")]
        public static extern MmResult MidiInReset(IntPtr hMidiIn);

        [DllImport("winmm.dll", EntryPoint = "midiInStart")]
        public static extern MmResult MidiInStart(IntPtr hMidiIn);

        [DllImport("winmm.dll", EntryPoint = "midiInStop")]
        public static extern MmResult MidiInStop(IntPtr hMidiIn);

        [DllImport("winmm.dll", EntryPoint = "midiInUnprepareHeader")]
        public static extern MmResult MidiInUnprepareHeader(IntPtr hMidiIn, [MarshalAs(UnmanagedType.Struct)] ref microDrum.MidiInEx.MIDIHDR lpMidiInHdr, int uSize);

    }

    /// <summary>
    /// MIDI In Message Information
    /// </summary>
    public class MidiInMessageEventArgs : EventArgs
    {
        int message;
        MidiEvent midiEvent;
        int timestamp;

        /// <summary>
        /// Create a new MIDI In Message EventArgs
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timestamp"></param>
        public MidiInMessageEventArgs(int message, int timestamp)
        {
            this.message = message;
            this.timestamp = timestamp;
            try
            {
                this.midiEvent = MidiEvent.FromRawMessage(message);
            }
            catch (Exception)
            {
                // don't worry too much - might be an invalid message
            }
        }

        /// <summary>
        /// The Raw message received from the MIDI In API
        /// </summary>
        public int RawMessage
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// The raw message interpreted as a MidiEvent
        /// </summary>
        public MidiEvent MidiEvent
        {
            get
            {
                return midiEvent;
            }
        }

        /// <summary>
        /// The timestamp in milliseconds for this message
        /// </summary>
        public int Timestamp
        {
            get
            {
                return timestamp;
            }
        }


    }

}