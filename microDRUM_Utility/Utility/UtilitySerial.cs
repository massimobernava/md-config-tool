using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace microDrum
{
    public static class UtilitySerial
    {
        private static SerialPort serialPort = null;
        public  static SerialDataReceivedEventHandler DataReceived=null;
        public static EventHandler COMClosed = null;

        static internal int ReadByte()
        {
            return serialPort.ReadByte();
        }
        static internal string COM_Name
        {
            get { if (serialPort != null) return serialPort.PortName; return ""; }
        }
        static internal bool IsOpen
        {
            get { return (serialPort != null && serialPort.IsOpen); }
        }
        static internal int BytesToRead
        {
            get { return serialPort.BytesToRead; }
        }
        static internal void CloseCOM()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
                serialPort.DataReceived -= serialPort_DataReceived;

                serialPort.Close();
                serialPort = null;

                if (COMClosed != null) COMClosed(null,null);
            }
        }
        static internal bool OpenCOM(string selectedCOM)
        {
            if (String.IsNullOrEmpty(selectedCOM)) { return false; }

            try
            {
                serialPort = new SerialPort(selectedCOM, 31250, Parity.None, 8, StopBits.One);
                serialPort.Open();
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
            }
            catch (IOException ioe)
            {
                MessageBox.Show(ioe.Message);
                return false;
            }
            if (UtilityMONO.RunningOnMONO())
            {
                Thread PortListener = new Thread(ListenPort);
                PortListener.IsBackground = true;
                PortListener.Start();
            }
            else
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            return true;
        }
        static internal void ListenPort()
        {
            while(DataReceived == null);
            while(true)
                if (serialPort.BytesToRead > 0)
                {
                    DataReceived(null, null);
                }
        }

        static internal void DiscardAllBuffer()
        {
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
        }
        static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (DataReceived != null) DataReceived(sender, e);
        }
        static internal void Write(byte[] buffer)
        {
            try
            {
                if (IsOpen) serialPort.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }
    }
}
