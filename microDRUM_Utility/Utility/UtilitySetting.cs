using System;
using System.Collections.Generic;
using System.Text;

namespace microDrum
{
    public enum uDrumMode
    {
        Off,
        StandBy,
        MIDI,
        Tool
    }

    public enum uDrumGeneralParam
    {
        Delay,
        Version,
        NSensor,
        XTalk
    }
    public enum uDrumParam
    {
        Note,
        CC,
        CloseNote,
        Thresold,
        ScanTime,
        SwitchTime,
        ChokeTime,
        MaskTime,
        Retrigger,
        Curve,
        XTalk,
        XTalkGroup,
        CurveForm,
        ChokeNote,
        Velocity,
        Choke,
        Dual,
        HHC,
        DualNote,
        AlternativeNote,
        OpenNote,
        DualThresold,
        AlternativeThresold,
        OThresold,
        Type,
        Channel
    }

    public static class UtilitySetting
    {
		public class SettingDataReceivedEventArgs : EventArgs
        {
            public byte Cmd;
            public byte Pin;
            public uDrumParam Param;
            public byte Value;
            public uDrumMode State;
        }
        public delegate void SettingDataReceivedEventHandler(SettingDataReceivedEventArgs e);
        public static SettingDataReceivedEventHandler SettingReceived;
		
		static UtilitySetting()
		{
			UtilityMIDI.MIDIReceived_SysEx+= new UtilityMIDI.MIDIDataReceivedEventHandler(MIDI_SysEx);
		}
		
		static void MIDI_SysEx(UtilityMIDI.MIDIDataReceivedEventArgs e)
        {
			if(SettingReceived!=null)
            {
                if(e.Cmd==0x02)
			    {
				    SettingDataReceivedEventArgs eSetting=new SettingDataReceivedEventArgs();

                    eSetting.Cmd = 0x02;
				    eSetting.Pin=e.Data1;
				    switch(e.Data2)
				    {
					    case 0x00: eSetting.Param=uDrumParam.Note; break;
					    case 0x01: eSetting.Param=uDrumParam.Thresold; break;
				    }
				    eSetting.Value=e.Data3;
			
				    SettingReceived(eSetting);
			    }
                else if (e.Cmd == 0x00)
                {
                    SettingDataReceivedEventArgs eSetting = new SettingDataReceivedEventArgs();

                    eSetting.Cmd = 0x00;
                    eSetting.State = uDrumMode.Off;
                    switch (e.Data1)
                    {
                        case 0x00: eSetting.State = uDrumMode.Off; break;
                        case 0x01: eSetting.State = uDrumMode.StandBy; break;
                        case 0x02: eSetting.State = uDrumMode.MIDI; break;
                        case 0x03: eSetting.State = uDrumMode.Tool; break;
                    }
                    SettingReceived(eSetting);

                }
            }
			
		}
        static public bool SetPinParam( byte Pin,uDrumParam Param, byte Value)
        {
            if (Pin > 0x30) return false;
            return SetSavePinParam(false,  Pin,Param, Value);
        }
        static public bool SavePinParam( byte Pin,uDrumParam Param, byte Value)
        {
            if (Pin > 0x30) return false;
            return SetSavePinParam(true,  Pin,Param, Value);
        }
        static private bool SetSavePinParam(bool Save,byte Pin,uDrumParam Param,byte Value)
        {
            byte P=0;
            switch (Param)
            {
                case uDrumParam.Note:
                case uDrumParam.CC:
                case uDrumParam.CloseNote: P = 0x00; break;
                case uDrumParam.Thresold: P = 0x01; break;
                case uDrumParam.ScanTime:
                case uDrumParam.SwitchTime: P = 0x02; break;
                case uDrumParam.MaskTime:
                case uDrumParam.ChokeTime: P = 0x03; break;
                case uDrumParam.Retrigger: P = 0x04; break;
                case uDrumParam.Curve: P = 0x05; break;
                case uDrumParam.XTalk: P = 0x06; break;
                case uDrumParam.XTalkGroup: P = 0x07; break;
                case uDrumParam.CurveForm: P = 0x08; break;
                case uDrumParam.ChokeNote: P = 0x09; break;
                case uDrumParam.HHC:
                case uDrumParam.Dual: P = 0x0A; break;
                case uDrumParam.OpenNote:
                case uDrumParam.DualNote:
                case uDrumParam.AlternativeNote: P = 0x0B; break;
                case uDrumParam.OThresold:
                case uDrumParam.DualThresold:
                case uDrumParam.AlternativeThresold: P = 0x0C; break;
                case uDrumParam.Type: P = 0x0D; break;
                case uDrumParam.Channel: P = 0x0E; break;
            }
            UtilityMIDI.MIDI_SysEx((byte)(Save?0x04:0x03), Pin, P, Value);
            return true;
        }

        static public bool SetHHParam(uDrumParam Param, byte Pin, byte Value)
        {
            return SetSaveHHParam(false, Param, Pin, Value);
        }
        static public bool SaveHHParam(uDrumParam Param, byte Pin, byte Value)
        {
            return SetSaveHHParam(true, Param, Pin, Value);
        }
        static private bool SetSaveHHParam(bool Save, uDrumParam Param, byte Pin, byte Value)
        {
            return false;
        }
        static public bool SetGeneralParam(uDrumGeneralParam Param,  byte Value)
        {
            return SetSaveGeneralParam(false, Param,Value);
        }
        static public bool SaveGeneralParam(uDrumGeneralParam Param,  byte Value)
        {
            return SetSaveGeneralParam(true, Param,Value);
        }
        static private bool SetSaveGeneralParam(bool Save, uDrumGeneralParam Param, byte Value)
        {
            byte P = 0;
            switch (Param)
            {
                case uDrumGeneralParam.Delay: P = 0x00; break;
                case uDrumGeneralParam.Version: P = 0x01; break;
                case uDrumGeneralParam.NSensor: P = 0x02; break;
                case uDrumGeneralParam.XTalk: P = 0x03; break;

            }
            UtilityMIDI.MIDI_SysEx((byte)(Save ? 0x04 : 0x03), 0x7E, P, Value);
            return true;
        }
        public static void GetPinParam(byte Pin,uDrumParam Param )
        {
            //Invia la richiesta
			byte P=0;
			switch (Param)
            {
                case uDrumParam.Note:
                case uDrumParam.CC:
                case uDrumParam.CloseNote: P = 0x00; break;
                case uDrumParam.Thresold: P = 0x01; break;
                case uDrumParam.ScanTime:
                case uDrumParam.SwitchTime: P = 0x02; break;
                case uDrumParam.MaskTime:
                case uDrumParam.ChokeTime: P = 0x03; break;
                case uDrumParam.Retrigger: P = 0x04; break;
                case uDrumParam.Curve: P = 0x05; break;
                case uDrumParam.XTalk: P = 0x06; break;
                case uDrumParam.XTalkGroup: P = 0x07; break;
                case uDrumParam.CurveForm: P = 0x08; break;
                case uDrumParam.ChokeNote: P = 0x09; break;
                case uDrumParam.HHC:
                case uDrumParam.Dual: P = 0x0A; break;
                case uDrumParam.OpenNote:
                case uDrumParam.DualNote:
                case uDrumParam.AlternativeNote: P = 0x0B; break;
                case uDrumParam.OThresold:
                case uDrumParam.DualThresold:
                case uDrumParam.AlternativeThresold: P = 0x0C; break;
                case uDrumParam.Type: P = 0x0D; break;
                case uDrumParam.Channel: P = 0x0E; break;
            }
            UtilityMIDI.MIDI_SysEx(0x02, Pin, P, 0);
        }

        public static void Profiling(bool Reset)
        {
            if (Reset) UtilityMIDI.MIDI_SysEx(0x6D, 0x00, 0x00, 0x00);
            else UtilityMIDI.MIDI_SysEx(0x6D, 0x01, 0x00, 0x00);
        }

        public static void GetState()
        {
            UtilityMIDI.MIDI_SysEx(0x00, 0x00, 0x00, 0x00);
        }
    }
}
