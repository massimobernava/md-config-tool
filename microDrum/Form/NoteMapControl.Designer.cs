namespace microDrum
{
    partial class NoteMapControl
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rFromNote = new System.Windows.Forms.RadioButton();
            this.rFromCC = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rToCC = new System.Windows.Forms.RadioButton();
            this.rToNote = new System.Windows.Forms.RadioButton();
            this.chkSelected = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOpen = new System.Windows.Forms.Button();
            this.slToNote = new microDrum.NoteSlider();
            this.slFromNote = new microDrum.NoteSlider();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(279, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(37, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "From:";
            // 
            // rFromNote
            // 
            this.rFromNote.AutoSize = true;
            this.rFromNote.Checked = true;
            this.rFromNote.Location = new System.Drawing.Point(51, 11);
            this.rFromNote.Name = "rFromNote";
            this.rFromNote.Size = new System.Drawing.Size(62, 17);
            this.rFromNote.TabIndex = 9;
            this.rFromNote.TabStop = true;
            this.rFromNote.Text = "NoteOn";
            this.rFromNote.UseVisualStyleBackColor = true;
            // 
            // rFromCC
            // 
            this.rFromCC.AutoSize = true;
            this.rFromCC.Location = new System.Drawing.Point(6, 11);
            this.rFromCC.Name = "rFromCC";
            this.rFromCC.Size = new System.Drawing.Size(39, 17);
            this.rFromCC.TabIndex = 8;
            this.rFromCC.Text = "CC";
            this.rFromCC.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rFromCC);
            this.groupBox1.Controls.Add(this.rFromNote);
            this.groupBox1.Controls.Add(this.slFromNote);
            this.groupBox1.Location = new System.Drawing.Point(75, -4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 36);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rToCC);
            this.groupBox2.Controls.Add(this.rToNote);
            this.groupBox2.Controls.Add(this.slToNote);
            this.groupBox2.Location = new System.Drawing.Point(311, -4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 36);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            // 
            // rToCC
            // 
            this.rToCC.AutoSize = true;
            this.rToCC.Location = new System.Drawing.Point(6, 11);
            this.rToCC.Name = "rToCC";
            this.rToCC.Size = new System.Drawing.Size(39, 17);
            this.rToCC.TabIndex = 8;
            this.rToCC.Text = "CC";
            this.rToCC.UseVisualStyleBackColor = true;
            // 
            // rToNote
            // 
            this.rToNote.AutoSize = true;
            this.rToNote.Checked = true;
            this.rToNote.Location = new System.Drawing.Point(51, 11);
            this.rToNote.Name = "rToNote";
            this.rToNote.Size = new System.Drawing.Size(62, 17);
            this.rToNote.TabIndex = 9;
            this.rToNote.TabStop = true;
            this.rToNote.Text = "NoteOn";
            this.rToNote.UseVisualStyleBackColor = true;
            // 
            // chkSelected
            // 
            this.chkSelected.AutoSize = true;
            this.chkSelected.Location = new System.Drawing.Point(6, 7);
            this.chkSelected.Name = "chkSelected";
            this.chkSelected.Size = new System.Drawing.Size(15, 14);
            this.chkSelected.TabIndex = 18;
            this.chkSelected.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.chkSelected);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(28, 28);
            this.panel1.TabIndex = 19;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(517, 5);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 23);
            this.btnOpen.TabIndex = 20;
            this.btnOpen.Text = "_";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // slToNote
            // 
            this.slToNote.Location = new System.Drawing.Point(119, 11);
            this.slToNote.Name = "slToNote";
            this.slToNote.SelectedIndex = 94;
            this.slToNote.ShowSlider = false;
            this.slToNote.Size = new System.Drawing.Size(70, 20);
            this.slToNote.TabIndex = 10;
            // 
            // slFromNote
            // 
            this.slFromNote.Location = new System.Drawing.Point(119, 11);
            this.slFromNote.Name = "slFromNote";
            this.slFromNote.SelectedIndex = 94;
            this.slFromNote.ShowSlider = false;
            this.slFromNote.Size = new System.Drawing.Size(70, 20);
            this.slFromNote.TabIndex = 10;
            // 
            // NoteMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(545, 108);
            this.MinimumSize = new System.Drawing.Size(545, 36);
            this.Name = "NoteMapControl";
            this.Size = new System.Drawing.Size(543, 34);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private NoteSlider slFromNote;
        private System.Windows.Forms.RadioButton rFromNote;
        private System.Windows.Forms.RadioButton rFromCC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rToCC;
        private System.Windows.Forms.RadioButton rToNote;
        private NoteSlider slToNote;
        private System.Windows.Forms.CheckBox chkSelected;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOpen;
    }
}
