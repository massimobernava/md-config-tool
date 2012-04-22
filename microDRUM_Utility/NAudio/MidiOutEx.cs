using System;
using System.Runtime.InteropServices;
using NAudio.Wave;
using System.Text;
using NAudio.Midi;
using NAudio;

namespace microDrum
{
    /// <summary>
    /// Summary description for midi.
    /// </summary>
    unsafe public class MidiOutEx
    {
        
        public const int CALLBACK_FUNCTION = 0x00030000;

        public enum MidiOutMessage
        {
            Open = 0x3C7,
            Close = 0x3C8,
            Done = 0x3C9
        }
        public delegate void MidiOutCallback(IntPtr midiInHandle, MidiOutMessage message, IntPtr userData, IntPtr messageParameter1, IntPtr messageParameter2);

        private IntPtr hMidiOut = IntPtr.Zero;
		private bool disposed = false;
        MidiOutCallback callback;

		public static int NumberOfDevices 
		{
			get 
			{
				return MidiOutGetNumDevs();
			}
		}

        public static MidiOutCapabilities DeviceInfo(int midiOutDeviceNumber)
        {
            MidiOutCapabilities caps = new MidiOutCapabilities();
            int structSize = Marshal.SizeOf(caps);
            MmException.Try(MidiOutGetDevCaps(midiOutDeviceNumber, out caps, structSize), "midiOutGetDevCaps");
            return caps;
        }

		
		/// <summary>
		/// Opens a specified MIDI out device
		/// </summary>
		/// <param name="deviceNo">The device number</param>
		public MidiOutEx(int deviceNo) 
		{
            this.callback = new MidiOutCallback(Callback);
            MmException.Try(MidiOutOpen(out hMidiOut, deviceNo, callback, 0, CALLBACK_FUNCTION), "midiOutOpen");
		}
		
		/// <summary>
		/// Closes this MIDI out device
		/// </summary>
		public void Close() 
		{
			Dispose();
		}

		/// <summary>
		/// Closes this MIDI out device
		/// </summary>
		public void Dispose() 
		{
            GC.KeepAlive(callback);
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Gets or sets the volume for this MIDI out device
		/// </summary>
		public int Volume 
		{
			// TODO: Volume can be accessed by device ID
			get 
			{
				int volume = 0;
				MmException.Try(MidiOutGetVolume(hMidiOut,ref volume),"midiOutGetVolume");
				return volume;
			}
			set 
			{
				MmException.Try(MidiOutSetVolume(hMidiOut,value),"midiOutSetVolume");
			}
		}

		/// <summary>
		/// Resets the MIDI out device
		/// </summary>
		public void Reset() 
		{
			MmException.Try(MidiOutReset(hMidiOut),"midiOutReset");
		}
		protected virtual void Dispose(bool disposing) 
		{
			if(!this.disposed) 
			{
				//if(disposing) Components.Dispose();
				MidiOutClose(hMidiOut);
			}
			disposed = true;         
		}

        private void Callback(IntPtr midiInHandle, MidiOutMessage message, IntPtr userData, IntPtr messageParameter1, IntPtr messageParameter2)
        {
        }
        public void SendMessage(int message)
        {
            MmException.Try(MidiOutShortMessage(hMidiOut, message), "midiOutShortMsg");
        }
        public int SendLongMessage(byte[] data)
        {
            /*Debug.Write("Midi Out: ");
            for(int i=0; i<data.Length; i++)
                Debug.Write(data[i].ToString("X")+" ");
            Debug.WriteLine("");*/

            int result;
            IntPtr ptr;
            int size = Marshal.SizeOf(typeof(MidiHeader));
            MidiHeader header = new MidiHeader();
            header.data = Marshal.AllocHGlobal(data.Length);
            for (int i = 0; i < data.Length; i++)
                Marshal.WriteByte(header.data, i, data[i]);
            header.bufferLength = data.Length;
            header.bytesRecorded = data.Length;
            header.flags = 0;

            try
            {
                ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MidiHeader)));
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(header.data);
                throw;
            }

            try
            {
                Marshal.StructureToPtr(header, ptr, false);
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(header.data);
                Marshal.FreeHGlobal(ptr);
                throw;
            }

            result = MidiOutPrepareHeader(hMidiOut, ptr, size);
            if (result == 0) result = MidiOutLongMessage(hMidiOut, ptr, size);
            if (result == 0) result = MidiOutUnprepareHeader(hMidiOut, ptr, size);

            Marshal.FreeHGlobal(header.data);
            Marshal.FreeHGlobal(ptr);

            return result;
        }

        #region Misc Declarations

        public const int MAXPNAMELEN = 32;

        [StructLayout(LayoutKind.Sequential)]
        public struct MidiHeader
        {
            public IntPtr data;
            public int bufferLength;
            public int bytesRecorded;
            public int user;
            public int flags;
            public IntPtr lpNext;
            public int reserved;
            public int offset;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] dwReserved;
        }

        #endregion

                #region MidiOut Declarations

        [DllImport("winmm.dll", EntryPoint = "midiOutGetNumDevs")]
        public static extern int MidiOutGetNumDevs();

        [DllImport("winmm.dll", CharSet = CharSet.Auto, EntryPoint = "midiOutGetDevCaps")]
        public static extern MmResult MidiOutGetDevCaps(int uDeviceID, out MidiOutCapabilities caps, int cbMidiOutCaps);

        [DllImport("winmm.dll", EntryPoint = "midiOutOpen")]
        public static extern MmResult MidiOutOpen(out IntPtr lphMidiOut, int uDeviceID, MidiOutCallback dwCallback, int dwInstance, int dwFlags);

        [DllImport("winmm.dll", EntryPoint = "midiOutClose")]
        public static extern int MidiOutClose(IntPtr hMidiOut);

        [DllImport("winmm.dll", EntryPoint = "midiOutShortMsg")]
        public static extern MmResult MidiOutShortMessage(IntPtr hMidiOut, int dwMsg);

        [DllImport("winmm.dll", EntryPoint = "midiOutLongMsg")]
        public static extern int MidiOutLongMessage(IntPtr handle, IntPtr headerPtr, int sizeOfMidiHeader);

        [DllImport("winmm.dll", EntryPoint = "midiOutPrepareHeader")]
        public static extern int MidiOutPrepareHeader(IntPtr handle, IntPtr headerPtr, int sizeOfMidiHeader);

        [DllImport("winmm.dll", EntryPoint = "midiOutUnprepareHeader")]
        public static extern int MidiOutUnprepareHeader(IntPtr handle, IntPtr headerPtr, int sizeOfMidiHeader);

        [DllImport("winmm.dll")]
        public static extern MmResult MidiOutGetVolume(IntPtr uDeviceID, ref int lpdwVolume);

        [DllImport("winmm.dll")]
        public static extern MmResult MidiOutSetVolume(IntPtr hMidiOut, int dwVolume);

        [DllImport("winmm.dll")]
        public static extern MmResult MidiOutReset(IntPtr hMidiOut);


        #endregion
    }
}

