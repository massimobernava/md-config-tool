using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace microDrum
{
    public partial class KeyboardForm : Form
    {
        DMap[] _DrumMap = null;
        PinSetting[] _Setting = null;
        HHSetting _hhSetting;
        Point Click = new Point(0, 0);
        int ClickNote = -1;

        public KeyboardForm()
        {
            InitializeComponent();
        }

        struct KeyData
        {
            public string Name;
            public Color Color;
            public Color NColor;
            public bool Semitone;
        };

        private Color Cyan = Color.FromArgb(147, 172, 200);
        private Color Cyan2 = Color.FromArgb(182, 190, 194);
        private Color Cyan3 = Color.FromArgb(124, 139, 146);//-50 lum
        private Color Yellow = Color.FromArgb(200, 171, 128);
        private Color Yellow2 = Color.FromArgb(199, 190, 172);  
        private Color Yellow3 = Color.FromArgb(157, 141, 108);
        private Color Magenta = Color.FromArgb(213, 154, 140);
        private Color Magenta2 = Color.FromArgb(203, 184, 176);
        private Color Magenta3 = Color.FromArgb(160, 126, 112);
        private Color Green = Color.FromArgb(156, 178, 152);
        private Color Green2 = Color.FromArgb(186, 192, 180);
        private Color Green3 = Color.FromArgb(165, 172, 157);

        private void pbKeyboard_Paint(object sender, PaintEventArgs e)
        {
            int position = 2032 - vsbKeyboard.Value;


            KeyData[] Data = new KeyData[128];

            for (int i = 0; i < 128; i++)
            {
                if (i % 12 == 0 || i % 12 == 2 || i % 12 == 4 || i % 12 == 5 || i % 12 == 7 || i % 12 == 9 || i % 12 == 11)
                {
                    Data[i].Color = Color.White;
                    Data[i].NColor = Color.White;
                    Data[i].Semitone = false;
                }
                else
                {
                    Data[i].Color = Color.FromArgb(31,24,22);
                    Data[i].NColor = Color.White;
                    Data[i].Semitone = true;
                }
            }
            if (_DrumMap != null && _Setting != null)
                foreach (DMap dm in _DrumMap)
                {
                    Color C1 = Cyan;
                    Color C2 = Cyan2;
                    Color C3 = Cyan3;

                    if (!dm.Single)
                    {
                        if (_Setting[dm.Head].Note < 128)
                        {
                            string NameEx = " Head";
                            if (_Setting[dm.Head].Type == PinType.Piezo)
                            {
                                C1 = Cyan;
                                C2 = Cyan2;
                                C3 = Cyan3;
                            }
                            else if (_Setting[dm.Head].Type == PinType.Switch)
                            {
                                C1 = Yellow;
                                C2 = Yellow2;
                                C3 = Yellow3;
                            }
                            else if (_Setting[dm.Head].Type == PinType.HHC)
                            {
                                NameEx = " CC";
                                C1 = Magenta;
                                C2 = Magenta2;
                                C3 = Magenta3;
                            }
                            else if (_Setting[dm.Head].Type == PinType.HH)
                            {
                                NameEx = " Close";
                                C1 = Green;
                                C2 = Green2;
                                C3 = Green3;
                            }
                            Data[_Setting[dm.Head].Note].Name = dm.Name + NameEx;
                            Data[_Setting[dm.Head].Note].NColor = C1;
                            Data[_Setting[dm.Head].Note].Color = Data[_Setting[dm.Head].Note].Semitone ? C3 : C2;
                        }
                        if (_Setting[dm.Rim].Note < 128)
                        {
                            string NameEx = " Rim";
                            if (_Setting[dm.Rim].Type == PinType.Piezo)
                            {
                                C1 = Cyan;
                                C2 = Cyan2;
                                C3 = Cyan3;
                            }
                            else if (_Setting[dm.Rim].Type == PinType.Switch)
                            {
                                C1 = Yellow;
                                C2 = Yellow2;
                                C3 = Yellow3;
                            }
                            else if (_Setting[dm.Rim].Type == PinType.HHC)
                            {
                                NameEx = " CC";
                                C1 = Magenta;
                                C2 = Magenta2;
                                C3 = Magenta3;
                            }
                            else if (_Setting[dm.Rim].Type == PinType.HH)
                            {
                                NameEx = " Close";
                                C1 = Green;
                                C2 = Green2;
                                C3 = Green3;
                            }
                            Data[_Setting[dm.Rim].Note].Name = dm.Name + NameEx;
                            Data[_Setting[dm.Rim].Note].NColor = C1;
                            Data[_Setting[dm.Rim].Note].Color = Data[_Setting[dm.Rim].Note].Semitone ? C3 : C2;
                        }
                    }
                    else
                    {
                        if (_Setting[dm.Head].Note < 128)
                        {
                            if (_Setting[dm.Head].Type == PinType.Piezo)
                            {
                                C1 = Cyan;
                                C2 = Cyan2;
                                C3 = Cyan3;
                            }
                            else if (_Setting[dm.Head].Type == PinType.Switch)
                            {
                                C1 = Yellow;
                                C2 = Yellow2;
                                C3 = Yellow3;
                            }
                            else if (_Setting[dm.Head].Type == PinType.HHC)
                            {
                                C1 = Magenta;
                                C2 = Magenta2;
                                C3 = Magenta3;
                            }
                            else if (_Setting[dm.Head].Type == PinType.HH)
                            {
                                C1 = Green;
                                C2 = Green2;
                                C3 = Green3;
                            }
                            Data[_Setting[dm.Head].Note].Name = dm.Name;
                            Data[_Setting[dm.Head].Note].NColor = C1;
                            Data[_Setting[dm.Head].Note].Color = Data[_Setting[dm.Head].Note].Semitone ? C3 : C2;
                        }
                    }

                }

            Data[_hhSetting.A_Note].Name = "ANote";
            Data[_hhSetting.A_Note].NColor = Green;
            Data[_hhSetting.A_Note].Color = Data[_hhSetting.A_Note].Semitone ? Green3 : Green2;

            Data[_hhSetting.B_Note].Name = "BNote";
            Data[_hhSetting.B_Note].NColor = Green;
            Data[_hhSetting.B_Note].Color = Data[_hhSetting.B_Note].Semitone ? Green3 : Green2;

            Data[_hhSetting.C_Note].Name = "CNote";
            Data[_hhSetting.C_Note].NColor = Green;
            Data[_hhSetting.C_Note].Color = Data[_hhSetting.C_Note].Semitone ? Green3 : Green2;

            Data[_hhSetting.D_Note].Name = "DNote";
            Data[_hhSetting.D_Note].NColor = Green;
            Data[_hhSetting.D_Note].Color = Data[_hhSetting.D_Note].Semitone ? Green3 : Green2;

            //Foot Close
            //Foot Open

            int Vel = 0;
            for (int i = 0; i < 128; i++)
            {
                Vel = (int)(127.0f * (float)(Click.X - 29) / 90.0f);
                Rectangle rect = new Rectangle(29, position - 16 * i + 6, 90, 14);
                if (i % 12 == 0 || i % 12 == 2 || i % 12 == 4 || i % 12 == 5 || i % 12 == 7 || i % 12 == 9 || i % 12 == 11)
                {
                    rect = new Rectangle(29, position - 16 * i + 6, 160, 14);
                    Vel = (int)(127.0f * (float)(Click.X - 29) / 160.0f);
                }
                if (rect.Contains(Click))
                {
                    Data[i].Color = Color.Blue;
                    ClickNote = i;
                }
            }
            for (int i = 0; i < 128; i++)
            {
                SolidBrush A = new SolidBrush(Data[i].NColor);
                SolidBrush B = new SolidBrush(Color.BlueViolet);
                SolidBrush C = new SolidBrush(Data[i].Color);
                SolidBrush D = new SolidBrush(Color.BlueViolet);

                e.Graphics.FillEllipse(A, 4, position - 16 * i + 4, 16, 16);
                e.Graphics.FillRectangle(A, 12, position - 16 * i + 5, 15, 15);
                if (i % 12 == 0 || i % 12 == 2 || i % 12 == 4 || i % 12 == 5 || i % 12 == 7 || i % 12 == 9 || i % 12 == 11)
                {
                    e.Graphics.FillRectangle(C, 29, position - 16 * i + 6, 160, 14);
                }
                else
                {
                    B = new SolidBrush(Data[i - 1].Color);
                    D = new SolidBrush(Data[i + 1].Color);

                    e.Graphics.FillRectangle(C, 29, position - 16 * i + 6, 90, 14);

                    if (i % 12 == 1 || i % 12 == 5 || i % 12 == 6)
                    {
                        e.Graphics.FillRectangle(D, 29 + 93, position - 16 * i - 1, 67, 9);
                        e.Graphics.FillRectangle(B, 29 + 93, position - 16 * i + 10, 67, 12);
                    }
                    else if (i % 12 == 3 || i % 12 == 10)
                    {
                        e.Graphics.FillRectangle(D, 29 + 93, position - 16 * i + 4, 67, 10);
                        e.Graphics.FillRectangle(B, 29 + 93, position - 16 * i + 16, 67, 10);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(D, 29 + 93, position - 16 * i + 2, 67, 9);
                        e.Graphics.FillRectangle(B, 29 + 93, position - 16 * i + 13, 67, 10);
                    }
                }
                e.Graphics.DrawString(i.ToString(), this.Font, Brushes.Black, 6, position - 16 * i + 6);
                e.Graphics.DrawString(Data[i].Name, this.Font, Brushes.Black, 30, position - 16 * i + 6);
            }

            if (ClickNote != -1) UtilityAudio.MIDI_NoteOn((byte)ClickNote, (byte)Vel);
            ClickNote = -1;
        }

        private void vsbKeyboard_Scroll(object sender, ScrollEventArgs e)
        {
            Click = new Point(-1, -1);
            pbKeyboard.Refresh();
        }

        public void Draw(DMap[] DrumMap, PinSetting[] Setting, HHSetting hhSetting)
        {
            _DrumMap = DrumMap;
            _Setting = Setting;
            _hhSetting = hhSetting;
        }

        private void pbKeyboard_MouseDown(object sender, MouseEventArgs e)
        {
            Click = e.Location;
            pbKeyboard.Refresh();
        }

        private void KeyboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
