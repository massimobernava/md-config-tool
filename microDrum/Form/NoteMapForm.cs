using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace microDrum
{
    public partial class NoteMapForm : Form
    {
        public static SortedList<int, int> NoteMap = new SortedList<int, int>();


        public NoteMapForm()
        {
            InitializeComponent();
        }

        public void ClearMap()
        {
            flPanelMap.Controls.Clear();
        }

        public void AddMap(NoteMapControl nmc)
        {
            flPanelMap.Controls.Add(nmc);
        }

        private void btnSaveMap_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Select map file:";
            saveFile.Filter = "Mapping Files (*.mmp)|*.mmp";
            saveFile.AddExtension = true;
            saveFile.ShowDialog();

            if (String.IsNullOrEmpty(saveFile.FileName)) return;

            TextWriter writer = new StreamWriter(saveFile.FileName);
            //foreach (int From in NoteMap.Keys)
            foreach (NoteMapControl nmc in flPanelMap.Controls)
                writer.WriteLine(nmc.FromNote + ";" + NoteMap[nmc.FromNote]);
            writer.Flush();
            writer.Close();
        }

        private void btnAddMap_Click(object sender, EventArgs e)
        {
            AddMap(new NoteMapControl());
        }

        private void btnApplyMap_Click(object sender, EventArgs e)
        {
            NoteMap.Clear();

            foreach (NoteMapControl nmc in flPanelMap.Controls)
                if (!NoteMap.ContainsKey(nmc.FromNote))
                    NoteMap.Add(nmc.FromNote, nmc.ToNote);
                else
                    MessageBox.Show("Duplicate Entry");
        }

        private void btnRemoveMap_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.flPanelMap.Controls)
                if (c is NoteMapControl && ((NoteMapControl)c).Selected)
                    this.flPanelMap.Controls.Remove(c);
        }


        internal static void Map(ref byte Cmd, ref byte Note)
        {
            if (NoteMap.ContainsKey((Note & 0x00FF) | (Cmd << 8)))
            {
                int Map = NoteMap[(Note & 0x00FF) | (Cmd << 8)];
                Note = (byte)(Map & 0x00FF);
                Cmd = (byte)(Map >> 8);
            }
        }

        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Select map file:";
            openFile.Filter = "Mapping Files (*.mmp)|*.mmp";
            openFile.AddExtension = true;

            if (MainForm.LastDirectoryUsed.ContainsKey("NMapDir"))
                openFile.InitialDirectory = MainForm.LastDirectoryUsed["NMapDir"];
            
            openFile.ShowDialog();

            if (!File.Exists(openFile.FileName)) return;

            if (MainForm.LastDirectoryUsed.ContainsKey("NMapDir"))
                MainForm.LastDirectoryUsed["NMapDir"] = Directory.GetParent(openFile.FileName).FullName;
            else
                MainForm.LastDirectoryUsed.Add("NMapDir", Directory.GetParent(openFile.FileName).FullName);

            string[] Lines = File.ReadAllLines(openFile.FileName);

            flPanelMap.Controls.Clear();

            foreach (string line in Lines)
            {
                string[] Notes = line.Split(';');
                flPanelMap.Controls.Add(new NoteMapControl(Convert.ToInt32(Notes[0]), Convert.ToInt32(Notes[1])));
            }

        }

        private void NoteMapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }
    }
}
