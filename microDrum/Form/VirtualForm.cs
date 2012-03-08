using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace microDrum
{
    public partial class VirtualForm : Form
    {
        public static VirtualForm Singleton = null;
        MainForm mainForm;

        public VirtualForm(MainForm main)
        {
            Singleton = this;
            InitializeComponent();
            mainForm = main;
            
            string OptionPath = Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini";

            string sBassDrum=UtilityIniFile.GetIniString("Virtual", "BassDrum", OptionPath);
            string sSnare = UtilityIniFile.GetIniString("Virtual", "Snare", OptionPath);
            string sTom1 = UtilityIniFile.GetIniString("Virtual", "Tom1", OptionPath);
            string sTom2 = UtilityIniFile.GetIniString("Virtual", "Tom2", OptionPath);
            string sTom3 = UtilityIniFile.GetIniString("Virtual", "Tom3", OptionPath);
            string sFloorTom1 = UtilityIniFile.GetIniString("Virtual", "FloorTom1", OptionPath);
            string sFloorTom2 = UtilityIniFile.GetIniString("Virtual", "FloorTom2", OptionPath);
            string sHH = UtilityIniFile.GetIniString("Virtual", "HH", OptionPath);
            string sCrash1 = UtilityIniFile.GetIniString("Virtual", "Crash1", OptionPath);
            string sCrash2 = UtilityIniFile.GetIniString("Virtual", "Crash2", OptionPath);
            string sRide = UtilityIniFile.GetIniString("Virtual", "Ride", OptionPath);
            string sChina = UtilityIniFile.GetIniString("Virtual", "China", OptionPath);

            foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                cbBassDrum.Items.Add(k);
                cbSnare.Items.Add(k);
                cbTom1.Items.Add(k);
                cbTom2.Items.Add(k);
                cbTom3.Items.Add(k);
                cbFloorTom1.Items.Add(k);
                cbFloorTom2.Items.Add(k);
                cbCrash1.Items.Add(k);
                cbCrash2.Items.Add(k);
                cbHH.Items.Add(k);
                cbRide.Items.Add(k);
                cbChina.Items.Add(k);
                if (k.ToString() == sBassDrum.Split(',')[0]) cbBassDrum.SelectedItem = k;
                if (k.ToString() == sTom1.Split(',')[0]) cbTom1.SelectedItem = k;
                if (k.ToString() == sTom2.Split(',')[0]) cbTom2.SelectedItem = k;
                if (k.ToString() == sTom3.Split(',')[0]) cbTom3.SelectedItem = k;
                if (k.ToString() == sFloorTom1.Split(',')[0]) cbFloorTom1.SelectedItem = k;
                if (k.ToString() == sFloorTom2.Split(',')[0]) cbFloorTom2.SelectedItem = k;
                if (k.ToString() == sHH.Split(',')[0]) cbHH.SelectedItem = k;
                if (k.ToString() == sCrash1.Split(',')[0]) cbCrash1.SelectedItem = k;
                if (k.ToString() == sCrash2.Split(',')[0]) cbCrash2.SelectedItem = k;
                if (k.ToString() == sRide.Split(',')[0]) cbRide.SelectedItem = k;
                if (k.ToString() == sChina.Split(',')[0]) cbChina.SelectedItem = k;
                if (k.ToString() == sSnare.Split(',')[0]) cbSnare.SelectedItem = k;
            }

            for (byte n = 0; n < 84; n++)
            {
                Note note = new Note(n);
                cbNoteBassDrum.Items.Add(note);
                cbNoteSnare.Items.Add(note);
                cbNoteTom1.Items.Add(note);
                cbNoteTom2.Items.Add(note);
                cbNoteTom3.Items.Add(note);
                cbNoteFloorTom1.Items.Add(note);
                cbNoteFloorTom2.Items.Add(note);
                cbNoteHH.Items.Add(note);
                cbNoteCrash1.Items.Add(note);
                cbNoteCrash2.Items.Add(note);
                cbNoteRide.Items.Add(note);
                cbNoteChina.Items.Add(note);
                if (n == Convert.ToInt32(sBassDrum.Split(',')[1])) cbNoteBassDrum.SelectedItem = note;
                if (n == Convert.ToInt32(sSnare.Split(',')[1])) cbNoteSnare.SelectedItem = note;
                if (n == Convert.ToInt32(sTom1.Split(',')[1])) cbNoteTom1.SelectedItem = note;
                if (n == Convert.ToInt32(sTom2.Split(',')[1])) cbNoteTom2.SelectedItem = note;
                if (n == Convert.ToInt32(sTom3.Split(',')[1])) cbNoteTom3.SelectedItem = note;
                if (n == Convert.ToInt32(sFloorTom1.Split(',')[1])) cbNoteFloorTom1.SelectedItem = note;
                if (n == Convert.ToInt32(sFloorTom2.Split(',')[1])) cbNoteFloorTom2.SelectedItem = note;
                if (n == Convert.ToInt32(sHH.Split(',')[1])) cbNoteHH.SelectedItem = note;
                if (n == Convert.ToInt32(sCrash1.Split(',')[1])) cbNoteCrash1.SelectedItem = note;
                if (n == Convert.ToInt32(sCrash2.Split(',')[1])) cbNoteCrash2.SelectedItem = note;
                if (n == Convert.ToInt32(sRide.Split(',')[1])) cbNoteRide.SelectedItem = note;
                if (n == Convert.ToInt32(sChina.Split(',')[1])) cbNoteChina.SelectedItem = note;
            }

        }

        private void VirtualForm_KeyDown(object sender, KeyEventArgs e)
        {
            PublicKeyDown(e);
        }

        public void PublicKeyDown(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (cbBassDrum.SelectedIndex != -1 && e.KeyData == (Keys)cbBassDrum.SelectedItem) MIDI_NoteOn(((Note)cbNoteBassDrum.SelectedItem).Value, 100);
            if (cbSnare.SelectedIndex != -1 && e.KeyData == (Keys)cbSnare.SelectedItem) MIDI_NoteOn(((Note)cbNoteSnare.SelectedItem).Value, 100);
            if (cbTom1.SelectedIndex != -1 && e.KeyData == (Keys)cbTom1.SelectedItem) MIDI_NoteOn(((Note)cbNoteTom1.SelectedItem).Value, 100);
            if (cbTom2.SelectedIndex != -1 && e.KeyData == (Keys)cbTom2.SelectedItem) MIDI_NoteOn(((Note)cbNoteTom2.SelectedItem).Value, 100);
            if (cbTom3.SelectedIndex != -1 && e.KeyData == (Keys)cbTom3.SelectedItem) MIDI_NoteOn(((Note)cbNoteTom3.SelectedItem).Value, 100);
            if (cbFloorTom1.SelectedIndex != -1 && e.KeyData == (Keys)cbFloorTom1.SelectedItem) MIDI_NoteOn(((Note)cbNoteFloorTom1.SelectedItem).Value, 100);
            if (cbFloorTom2.SelectedIndex != -1 && e.KeyData == (Keys)cbFloorTom2.SelectedItem) MIDI_NoteOn(((Note)cbNoteFloorTom2.SelectedItem).Value, 100);
            if (cbHH.SelectedIndex != -1 && e.KeyData == (Keys)cbHH.SelectedItem) MIDI_NoteOn(((Note)cbNoteHH.SelectedItem).Value, 100);
            if (cbCrash1.SelectedIndex != -1 && e.KeyData == (Keys)cbCrash1.SelectedItem) MIDI_NoteOn(((Note)cbNoteCrash1.SelectedItem).Value, 100);
            if (cbCrash2.SelectedIndex != -1 && e.KeyData == (Keys)cbCrash2.SelectedItem) MIDI_NoteOn(((Note)cbNoteCrash2.SelectedItem).Value, 100);
            if (cbRide.SelectedIndex != -1 && e.KeyData == (Keys)cbRide.SelectedItem) MIDI_NoteOn(((Note)cbNoteRide.SelectedItem).Value, 100);
            if (cbChina.SelectedIndex != -1 && e.KeyData == (Keys)cbChina.SelectedItem) MIDI_NoteOn(((Note)cbNoteChina.SelectedItem).Value, 100);

        }
        void MIDI_NoteOn(byte Note, byte Velocity)
        {
            UtilityMIDI.MIDI_NoteOn(Note, Velocity);
           // UtilitySerial.Write(new byte[] { 0x90, Note, Velocity });
            UtilityAudio.MIDI_NoteOn(Note, Velocity);
            //if(mainForm!=null) mainForm.MIDI_NoteOn(Note, Velocity);
        }
        private void VirtualForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Singleton = null;

            string OptionPath = Application.StartupPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Option.ini";
            if (cbBassDrum.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "BassDrum", ((Keys)cbBassDrum.SelectedItem).ToString() + "," + ((Note)cbNoteBassDrum.SelectedItem).Value.ToString(), OptionPath);
            if (cbSnare.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "Snare", ((Keys)cbSnare.SelectedItem).ToString() + "," + ((Note)cbNoteSnare.SelectedItem).Value.ToString(), OptionPath);
            if (cbTom1.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "Tom1", ((Keys)cbTom1.SelectedItem).ToString() + "," + ((Note)cbNoteTom1.SelectedItem).Value.ToString(), OptionPath);
            if (cbTom2.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "Tom2", ((Keys)cbTom2.SelectedItem).ToString() + "," + ((Note)cbNoteTom2.SelectedItem).Value.ToString(), OptionPath);
            if (cbTom3.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "Tom3", ((Keys)cbTom3.SelectedItem).ToString() + "," + ((Note)cbNoteTom3.SelectedItem).Value.ToString(), OptionPath);
            if (cbFloorTom1.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "FloorTom1", ((Keys)cbFloorTom1.SelectedItem).ToString() + "," + ((Note)cbNoteFloorTom1.SelectedItem).Value.ToString(), OptionPath);
            if (cbFloorTom2.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "FloorTom2", ((Keys)cbFloorTom2.SelectedItem).ToString() + "," + ((Note)cbNoteFloorTom2.SelectedItem).Value.ToString(), OptionPath);
            if (cbHH.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "HH", ((Keys)cbHH.SelectedItem).ToString() + "," + ((Note)cbNoteHH.SelectedItem).Value.ToString(), OptionPath);
            if (cbCrash1.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "Crash1", ((Keys)cbCrash1.SelectedItem).ToString() + "," + ((Note)cbNoteCrash1.SelectedItem).Value.ToString(), OptionPath);
            if (cbCrash2.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "Crash2", ((Keys)cbCrash2.SelectedItem).ToString() + "," + ((Note)cbNoteCrash2.SelectedItem).Value.ToString(), OptionPath);
            if (cbRide.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "Ride", ((Keys)cbRide.SelectedItem).ToString() + "," + ((Note)cbNoteRide.SelectedItem).Value.ToString(), OptionPath);
            if (cbChina.SelectedIndex != -1) UtilityIniFile.SetIniString("Virtual", "China", ((Keys)cbChina.SelectedItem).ToString() + "," + ((Note)cbNoteChina.SelectedItem).Value.ToString(), OptionPath);
        }
    }
}
