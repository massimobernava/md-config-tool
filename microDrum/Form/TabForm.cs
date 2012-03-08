using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace microDrum
{
    struct Misura
    {
        public int[,] Notes;
    }
    public partial class TabForm : Form
    {
        public static TabForm Singleton = null;

        int Tempo = 0;
        int Tick = 0;
        int nMisure = 0;
        int nNote = 0;
        float Interval = 0;
        int HitFinder = 2;

        string[] NameNote = null;
        int[] Note = null;
        char[] DrawNote = null;
        Misura[] Misure = null;
        long TimerTick = 0;
        int ttX = 0;
        int ttY = 0;
        long StartNoteTime = 0;
        List<Note> PlayedNotes = new List<Note>();

        private Font SheetFont = new Font(FontFamily.GenericSerif, 8);


        //NAudio.Midi.MidiOut midiOut = null;
        DateTime StartTime = DateTime.Now;

        public TabForm()
        {
            Singleton = this;

            InitializeComponent();

            //se è aperto il vst usare quello altrimenti MIDI
            //for (int i = 0; i < NAudio.Midi.MidiOut.NumberOfDevices; i++)
            //    tcbOutput.Items.Add(NAudio.Midi.MidiOut.DeviceInfo(i).ProductName);

            foreach (Control c in this.Controls)
                AddKeyDown(c);
        }
        void AddKeyDown(Control c)
        {
            c.KeyDown += new KeyEventHandler(c_KeyDown);
            foreach (Control cc in c.Controls)
                AddKeyDown(cc);
        }
        void c_KeyDown(object sender, KeyEventArgs e)
        {
            if (VirtualForm.Singleton != null)
                VirtualForm.Singleton.PublicKeyDown(e);
        }
        long Difference = 0;
        long nDifference = 1;
        //long Mis = 0;
        internal void MIDI(byte Note, byte Velocity)
        {
            Note note = new Note(Note);
            note.Time -= StartNoteTime-(tbCorrection.Value-500);

            for (int i = 0; i < nNote; i++)
            {
                if (Misure[TimerTick / Tick].Notes[TimerTick % Tick, i] == Note)
                {
                    note.Hit = true;
                    note.Error = (note.Time - (long)(Interval * TimerTick));
                    Difference += Math.Abs(note.Error);
                    nDifference++;
                    PlayedNotes.Add(note);
                    return;
                }
                else
                {
                    for (int tcount = 1; tcount < HitFinder; tcount++)
                    {
                        if ((((TimerTick - tcount) / Tick) >= 0 && (TimerTick - tcount) % Tick>=0 && Misure[(TimerTick - tcount) / Tick].Notes[(TimerTick - tcount) % Tick, i] == Note))
                        {
                            note.Hit = true;
                            note.Error = (note.Time - (long)(Interval * (TimerTick - tcount)));
                            Difference += Math.Abs(note.Error);
                            nDifference++;
                            PlayedNotes.Add(note);
                            return;
                        }
                        else if ((((TimerTick + tcount) / Tick) < nMisure && Misure[(TimerTick + tcount) / Tick].Notes[(TimerTick + tcount) % Tick, i] == Note))
                        {
                            note.Hit = true;
                            note.Error = (note.Time - (long)(Interval * (TimerTick + tcount)));
                            Difference += Math.Abs(note.Error);
                            nDifference++;
                            PlayedNotes.Add(note);
                            return;
                        }
                    }
                }
            }
            //if (note.Hit == false) Mis++;
             PlayedNotes.Add(note);

            //this.Text = "Error:" + (Difference / nDifference).ToString() + " Mis:" + Mis.ToString();
        }

        internal void LoadTab(string TabFile)
        {
            string[] Tabs = File.ReadAllLines(TabFile);

            Note = null;
            Misure = null;

            int CountNote = 0;
            int CountRows = 0;

            int Section = 0;
            foreach (string i in Tabs)
            {
                string line = i.Trim();
                if (String.IsNullOrEmpty(line)) continue;

                if (line.ToUpper() == "{GENERAL}") { Section = 1; continue; }
                else if (line.ToUpper() == "{NOTE}") { Section = 2; continue; }
                else if (line.ToUpper() == "{TAB}") { Section = 3; continue; }

                if (Section == 1) //general
                {
                    if (line.ToUpper().StartsWith("TEMPO")) Tempo = Convert.ToInt32(line.Substring(line.IndexOf('=') + 1));
                    else if (line.ToUpper().StartsWith("TICK")) Tick = Convert.ToInt32(line.Substring(line.IndexOf('=') + 1));
                    else if (line.ToUpper().StartsWith("MISURE")) nMisure = Convert.ToInt32(line.Substring(line.IndexOf('=') + 1));
                    else if (line.ToUpper().StartsWith("NOTE")) nNote = Convert.ToInt32(line.Substring(line.IndexOf('=') + 1));
                    else if (line.ToUpper().StartsWith("HITFINDER")) 
                        HitFinder = Convert.ToInt32(line.Substring(line.IndexOf('=') + 1));
                }
                else if (Section == 2)//note
                {
                    if (Note == null) Note = new int[nNote];
                    if (DrawNote == null) DrawNote = new char[nNote];
                    if (NameNote == null) NameNote = new string[nNote];
                    string[] tmp = line.Split('=')[1]/*.Substring(line.IndexOf('=') + 1)*/.Split(';');//Per avere in futuro più note per riga
                    DrawNote[CountNote] = tmp[0].Split(',')[1][0];
                    NameNote[CountNote] = line.Split('=')[0];
                    Note[CountNote++] = Convert.ToInt32(tmp[0].Split(',')[0]);
                }
                else if (Section == 3)//tabs
                {
                    if (Misure == null)
                    {
                        Misure = new Misura[nMisure];
                        for (int m = 0; m < nMisure; m++)
                            Misure[m].Notes = new int[Tick, nNote];
                    }

                    for (int c = 0; c < Tick; c++)
                        Misure[CountRows / nNote].Notes[c, CountRows % nNote] = line[c + 1] == '-' ? 0 : Note[CountRows % nNote];

                    CountRows++;
                }
            }

            //timer 
            Interval = (int)(60000.0f / ((float)Tempo * (float)Tick / 4.0f));
            timerTempo.Interval = (int)Interval - 5;


            /*for (int ii = 0; ii < 20; ii++)
            {
                Note n = new Note(38);
                n.Time = (long)(8.0f * (float)ii * Interval);
                PlayedNotes.Add(n);
            }*/
        }

        private void tsbPlay_Click(object sender, EventArgs e)
        {
            TimerTick = 0;
            StartNoteTime = System.Environment.TickCount;
            PlayedNotes.Clear();
            timerTempo.Start();
            StartTime=DateTime.Now;
        }

        private void TabForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            Singleton = null;
        }

        private void timerTempo_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < nNote; i++)
            {
                if (Misure[TimerTick / Tick].Notes[TimerTick % Tick, i] != 0)
                {
                    UtilityAudio.MIDI_NoteOn((byte)Misure[TimerTick / Tick].Notes[TimerTick % Tick, i], 100);

                    /*if (VSTForm.Singleton != null)
                    {
                        VSTForm.Singleton.MIDI((byte)Misure[TimerTick / Tick].Notes[TimerTick % Tick, i], 100);
                    }
                    */
                    UtilityMIDI.MIDI_NoteOn((byte)Misure[TimerTick / Tick].Notes[TimerTick % Tick, i], 100);
                }
            }

            TimerTick++;

            if (TimerTick >= nMisure * Tick)
            {
                if (!tsbMemory.Checked) { PlayedNotes.Clear(); pictureBox_Sheet.Invalidate(); }

                TimerTick = 0;
                StartNoteTime = System.Environment.TickCount;
            }
            ttX = (int)(((TimerTick / (long)Tick) % 4) * (long)Tick * 8 + 50 + (TimerTick % (long)Tick) * 8);
            ttY = (int)((TimerTick / (long)Tick) / 4) * nNote * 10 + 50;//Da Rivedere
            pictureBox_Sheet.Invalidate(new Rectangle(ttX - 50, ttY - 10, 100, 30 + nNote * 10));
            pictureBox_Info.Invalidate();

        }

        private void pictureBox_Sheet_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < nMisure; i++)
                DrawFrame(e.Graphics, i, i % 4, i / 4, e.ClipRectangle);

            e.Graphics.DrawLine(Pens.Red, ttX - 4, ttY - 10 /*- ScrollValue*/, ttX - 4, ttY + 10 + nNote * 10 /*- ScrollValue*/);

            //long TotalTime = timerTempo.Interval * Tick * nMisure;
            //long TotalLenght = Tick * 8 * nMisure;

            int cOldNote = PlayedNotes.Count;
            foreach (Note n in PlayedNotes)
            {
                for (int i = 0; i < nNote; i++)
                    if (n.Value == Note[i])
                    {
                        float Final = ((float)(n.Time * 8.0f) / Interval);
                        int FinalY = (i * 10) + 50 + ((int)Final / /*(int)TotalLenght*/(4 * Tick * 8));
                        int FinalX = 50 + ((int)Final % /*(int)TotalLenght*/(4 * Tick * 8));

                        //if (!e.ClipRectangle.Contains(FinalX, FinalY)) continue;

                        int CircleAlpha = (255 - cOldNote) < 0 ? 0 : 255 - cOldNote;

                        if (DrawNote[i] == 'O')
                            e.Graphics.DrawEllipse(n.Hit ? new Pen(Color.FromArgb(CircleAlpha, Color.Blue)) : new Pen(Color.FromArgb(CircleAlpha, Color.Red)), FinalX, FinalY, 8, 8);
                        else if (DrawNote[i] == 'X')
                        {
                            e.Graphics.DrawLine(n.Hit ? new Pen(Color.FromArgb(CircleAlpha, Color.Blue)) : new Pen(Color.FromArgb(CircleAlpha, Color.Red)), FinalX, FinalY, FinalX + 8, FinalY + 8);
                            e.Graphics.DrawLine(n.Hit ? new Pen(Color.FromArgb(CircleAlpha, Color.Blue)) : new Pen(Color.FromArgb(CircleAlpha, Color.Red)), FinalX + 8, FinalY, FinalX, FinalY + 8);
                        }
                        //e.Graphics.DrawString(n.Time.ToString(), this.Font, Brushes.Black, FinalX, FinalY);

                    }
                cOldNote--;
            }

        }

        private void DrawFrame(Graphics g, int misura, int x, int y, Rectangle Clip)
        {
            int X = x * Tick * 8 + 50;
            int Y = y * 140 + 50 - vScrollBar_Sheet.Value;//DA RIVEDERE

            if (Clip.IntersectsWith(new Rectangle(X, Y, Tick * 8, nNote * 10)))
            {
                g.DrawLine(Pens.Black, X, Y, X, Y + 8 + (10 * nNote));
                for (int i = 0; i < Tick; i++)
                {
                    if (i % 8 == 0) g.FillEllipse(Brushes.Red, X + i * 8 + 2, Y - 6, 3, 3);
                    for (int j = 0; j < nNote; j++)
                    {
                        int FinalX = X + i * 8;
                        int FinalY = Y + (10 * j);

                        if (Misure[misura].Notes[i, j] == 0)
                            g.DrawLine(Pens.Black, FinalX + 2, FinalY + 4, FinalX + 6, FinalY + 4);
                        else
                        {
                            if (DrawNote[j] == 'O')
                                g.DrawEllipse(Pens.Black, FinalX, FinalY, 8, 8);
                            else if (DrawNote[j] == 'X')
                            {
                                g.DrawLine(Pens.Black, FinalX, FinalY, FinalX + 8, FinalY + 8); g.DrawLine(Pens.Black, FinalX + 8, FinalY, FinalX, FinalY + 8);
                            }
                        }
                    }
                }
                g.DrawLine(Pens.Black, X + Tick * 8, Y, X + Tick * 8, Y + (10 * nNote));
                if (x == 0)
                {
                    for (int j = 0; j < nNote; j++)
                        g.DrawString(NameNote[j], SheetFont, Brushes.Black, X - 30, Y - 4 + j * 10);
                }
            }


        }

        private void tcbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {

            /*if (tcbOutput.SelectedIndex>=0)
            {
                if (midiOut != null)
                    midiOut.Close();

                midiOut = new NAudio.Midi.MidiOut(tcbOutput.SelectedIndex);
            }*/
            //UtilityAudio.SetMIDI(tcbOutput.SelectedIndex);
        }

        private void tsbStop_Click(object sender, EventArgs e)
        {
            TimerTick = 0;
            timerTempo.Stop();
        }

        private void pictureBox_Info_Paint(object sender, PaintEventArgs e)
        {
            int Center = pictureBox_Info.Height / 2;
            e.Graphics.DrawLine(Pens.LightSteelBlue, 50, Center - 100, 50, Center + 100);
            e.Graphics.DrawLine(Pens.LightSteelBlue, 50, Center, 50 + Tick * 32, Center);

            for (int i = 1; i < 10; i++)
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(32, Color.LightSteelBlue)), 50, Center - (i * 10), 50 + Tick * 32, Center - (i * 10));
                e.Graphics.DrawLine(new Pen(Color.FromArgb(32, Color.LightSteelBlue)), 50, Center + (i * 10), 50 + Tick * 32, Center + (i * 10));

                e.Graphics.DrawString((i * -10).ToString(), new Font(this.Font.FontFamily, 6.0f), Brushes.White, 25, Center - (i * 10) - 4);
                e.Graphics.DrawString((i * 10).ToString(), new Font(this.Font.FontFamily, 6.0f), Brushes.White, 25, Center + (i * 10) - 4);
            }
            e.Graphics.DrawString("0", new Font(this.Font.FontFamily, 6.0f), Brushes.White, 25, Center - 4);

            for (int misura = 0; misura < nMisure; misura++)
            {
                for (int i = 0; i < Tick; i++)
                {
                    for (int j = 0; j < nNote; j++)
                    {
                        int FinalX = 50 + (i * 8) + (misura * Tick * 8);
                        int FinalY = Center - 4;

                        if (Misure[misura].Notes[i, j] != 0)
                            e.Graphics.DrawEllipse(Pens.White, FinalX, FinalY, 8, 8);
                    }
                }
            }

            foreach (Note n in PlayedNotes)
            {
                float Final = ((float)((n.Time - n.Error) * 8.0f) / Interval);
                int FinalY = Center + (int)n.Error - 4;
                int FinalX = 50 + ((int)Final % /*(int)TotalLenght*/(4 * Tick * 8));

                e.Graphics.DrawEllipse(n.Hit ? new Pen(Color.FromArgb(255, Color.Blue)) : new Pen(Color.FromArgb(255, Color.Red)), FinalX, FinalY, 8, 8);
            }
            e.Graphics.DrawLine(Pens.Red, ttX - 4, Center - 100, ttX - 4, Center + 100);

            if(TimerTick>0) e.Graphics.DrawString("Time: " + (DateTime.Now-StartTime).ToString(), new Font(this.Font.FontFamily, 10.0f), Brushes.Red, 50, 5);
            e.Graphics.DrawString("Total Error: " + Difference + " ms", new Font(this.Font.FontFamily, 10.0f), Brushes.Red, pictureBox_Info.Width - 150, 5);
            e.Graphics.DrawString("Mean Error: " + (nDifference>0?((float)Difference / (float)nDifference).ToString(".00"):"") +" ms", new Font(this.Font.FontFamily, 10.0f), Brushes.Red, pictureBox_Info.Width - 150, 20);
            e.Graphics.DrawString("Correction: " + (tbCorrection.Value-500).ToString() + " ms", new Font(this.Font.FontFamily, 10.0f), Brushes.Red, pictureBox_Info.Width - 150, 35);
        }

        private void tsbMemory_CheckedChanged(object sender, EventArgs e)
        {
            tsbMemory.Image = (tsbMemory.Checked) ? global::microDrum.Properties.Resources.checkmark : global::microDrum.Properties.Resources.x;
        }

        private void tsbExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "xls files (*.xls)|*.xls";

            if (save.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Note\tTime\tError\tHit");
                foreach (Note n in PlayedNotes)
                {
                    sb.AppendLine(n.Value.ToString() + "\t" + n.Time.ToString() + "\t" + n.Error.ToString()+"\t"+(n.Hit?"Y":"N"));
                }

                StreamWriter sw = new StreamWriter(save.FileName);
                sw.Write(sb.ToString());
                sw.Close();
            }

        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            PlayedNotes.Clear(); 
            pictureBox_Sheet.Invalidate();
            pictureBox_Info.Invalidate();
            StartTime = DateTime.Now;
            Difference = 0;
            nDifference = 0;
        }


    }
}
