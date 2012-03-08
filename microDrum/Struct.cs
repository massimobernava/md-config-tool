using System;
using System.Text;

enum mDMode
{
    Off,
    StandBy,
    MIDI,
    Tool
}
enum Tabs
{
    Configuration = 0,
    Tools = 1,
    Monitor = 2,
    Sfz=3,
    Effects=4
}

public enum PinType
{
    Piezo=0,
    Switch=1,
    HHC=2,
    HH=3,
    HHs=4,
    YSwitch=5,
    Disabled=127//0x7F
}
public struct PinSetting
{

    public byte Note;
    public byte Channel;
    public byte Thresold;
    public byte ScanTime;
    public byte MaskTime;
    public byte Retrigger;
    public byte Curve;
    public byte Xtalk;
    public byte XtalkGroup;
    public byte CurveForm;
    public byte Choke;
    public byte Dual;
    public byte DualNote;
    public byte DualThresold;
    public PinType Type;

    public override string ToString()
    {
        return Note.ToString() + ";" +
            Thresold.ToString() + ";" +
            ScanTime.ToString() + ";" +
            MaskTime.ToString() + ";" +
            Retrigger.ToString() + ";" +
            Curve.ToString() + ";" +
            Xtalk.ToString() + ";" +
            XtalkGroup.ToString() + ";" +
            CurveForm.ToString() + ";" +
            Choke.ToString() + ";" +
            Dual.ToString() + ";" +
            DualNote.ToString() + ";" +
            DualThresold.ToString() + ";" +
            ((byte)Type).ToString()+ ";" +
            Channel.ToString();
    }
    public void FromString(string Set)
    {
        string[] tmp = Set.Split(';');
        try
        {
            Note = Convert.ToByte(tmp[0]);
            Thresold = Convert.ToByte(tmp[1]);
            ScanTime = Convert.ToByte(tmp[2]);
            MaskTime = Convert.ToByte(tmp[3]);
            Retrigger = Convert.ToByte(tmp[4]);
            Curve = Convert.ToByte(tmp[5]);
            Xtalk = Convert.ToByte(tmp[6]);
            XtalkGroup = Convert.ToByte(tmp[7]);
            CurveForm = Convert.ToByte(tmp[8]);
            Choke = Convert.ToByte(tmp[9]);
            Dual = Convert.ToByte(tmp[10]);
            DualNote = Convert.ToByte(tmp[11]);
            DualThresold = Convert.ToByte(tmp[12]);
            Type = (PinType)Convert.ToByte(tmp[13]);
            Channel = Convert.ToByte(tmp[14]);
        }
        catch (Exception)
        {

        }

    }
}
public struct GeneralSetting
{
    public byte DelayTime;
    //public byte HHC1;
    public byte NSensor;
    public byte Xtalk;
}

public struct HHSetting
{
    public byte A_Note;
    public byte B_Note;
    public byte C_Note;
    public byte D_Note;

    public byte A_Thresold;
    public byte B_Thresold;
    public byte C_Thresold;
    public byte D_Thresold;

    public byte FootCloseNote;
    public byte FootCloseThresold;

    public byte FootSplashNote;
    public byte FootSplashThresold;
}

public struct DMap
{
    public string Name;
    public byte Head;
    public byte Rim;
    public bool Single;

    public override string ToString()
    {
        return Name + " (" + Head + (Single ? "" : "," + Rim) + ")";
    }
}





public class MIDI
{
    int _Cmd;
    byte _Data1;
    byte _Data2;

    DateTime _PlayTime;

    public MIDI(int Cmd, byte Data1, byte Data2)
    {
        _Cmd = Cmd;
        _Data1 = Data1;
        _Data2=Data2;
        _PlayTime = DateTime.Now;//Multiple Note Play
    }

    public int Cmd { get { return _Cmd; } }
    public byte Data1 { get { return _Data1; } }
    public byte Data2 { get { return _Data2; } }

    public override string ToString()
    {
        switch (_Cmd & 0xF0)
        {
            case 0x90: //NOTE_ON
                return "NOTE ON(" + _Data1.ToString() + "," + _Data2.ToString() + ")";
            case 0xB0: //CC
                return "CC(" + _Data1.ToString() + "," + _Data2.ToString() + ")";
            default:
                return "???(" + _Data1.ToString() + "," + _Data2.ToString() + ")";
        }
    }
}
