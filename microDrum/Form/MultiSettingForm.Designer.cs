namespace microDrum
{
    partial class MultiSettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiSettingForm));
            this.label1 = new System.Windows.Forms.Label();
            this.nudFrom = new System.Windows.Forms.NumericUpDown();
            this.nudTo = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.cbTypeMulti = new System.Windows.Forms.ComboBox();
            this.nudThresoldMulti = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.dudNoteMulti = new System.Windows.Forms.DomainUpDown();
            this.lblRimNote = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnSendMulti = new System.Windows.Forms.Button();
            this.pbSendMulti = new microDrum.LedBar();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThresoldMulti)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pin from:";
            // 
            // nudFrom
            // 
            this.nudFrom.Location = new System.Drawing.Point(60, 8);
            this.nudFrom.Maximum = new decimal(new int[] {
            46,
            0,
            0,
            0});
            this.nudFrom.Name = "nudFrom";
            this.nudFrom.Size = new System.Drawing.Size(42, 20);
            this.nudFrom.TabIndex = 1;
            // 
            // nudTo
            // 
            this.nudTo.Location = new System.Drawing.Point(60, 34);
            this.nudTo.Maximum = new decimal(new int[] {
            47,
            0,
            0,
            0});
            this.nudTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTo.Name = "nudTo";
            this.nudTo.Size = new System.Drawing.Size(42, 20);
            this.nudTo.TabIndex = 3;
            this.nudTo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "To:";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.ForeColor = System.Drawing.Color.Black;
            this.label40.Location = new System.Drawing.Point(7, 21);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(34, 13);
            this.label40.TabIndex = 78;
            this.label40.Text = "Type:";
            // 
            // cbTypeMulti
            // 
            this.cbTypeMulti.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeMulti.FormattingEnabled = true;
            this.cbTypeMulti.Location = new System.Drawing.Point(70, 19);
            this.cbTypeMulti.Name = "cbTypeMulti";
            this.cbTypeMulti.Size = new System.Drawing.Size(65, 21);
            this.cbTypeMulti.TabIndex = 77;
            // 
            // nudThresoldMulti
            // 
            this.nudThresoldMulti.Location = new System.Drawing.Point(80, 72);
            this.nudThresoldMulti.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThresoldMulti.Name = "nudThresoldMulti";
            this.nudThresoldMulti.Size = new System.Drawing.Size(55, 20);
            this.nudThresoldMulti.TabIndex = 76;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(7, 73);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(57, 13);
            this.label24.TabIndex = 75;
            this.label24.Text = "Threshold:";
            // 
            // dudNoteMulti
            // 
            this.dudNoteMulti.Location = new System.Drawing.Point(70, 46);
            this.dudNoteMulti.Name = "dudNoteMulti";
            this.dudNoteMulti.ReadOnly = true;
            this.dudNoteMulti.Size = new System.Drawing.Size(65, 20);
            this.dudNoteMulti.TabIndex = 80;
            this.dudNoteMulti.Text = "---";
            // 
            // lblRimNote
            // 
            this.lblRimNote.AutoSize = true;
            this.lblRimNote.ForeColor = System.Drawing.Color.Black;
            this.lblRimNote.Location = new System.Drawing.Point(7, 47);
            this.lblRimNote.Name = "lblRimNote";
            this.lblRimNote.Size = new System.Drawing.Size(33, 13);
            this.lblRimNote.TabIndex = 79;
            this.lblRimNote.Text = "Note:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSave);
            this.groupBox1.Controls.Add(this.cbTypeMulti);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.dudNoteMulti);
            this.groupBox1.Controls.Add(this.nudThresoldMulti);
            this.groupBox1.Controls.Add(this.lblRimNote);
            this.groupBox1.Controls.Add(this.label40);
            this.groupBox1.Location = new System.Drawing.Point(1, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(148, 168);
            this.groupBox1.TabIndex = 82;
            this.groupBox1.TabStop = false;
            // 
            // chkSave
            // 
            this.chkSave.AutoSize = true;
            this.chkSave.Location = new System.Drawing.Point(91, 145);
            this.chkSave.Name = "chkSave";
            this.chkSave.Size = new System.Drawing.Size(51, 17);
            this.chkSave.TabIndex = 81;
            this.chkSave.Text = "Save";
            this.chkSave.UseVisualStyleBackColor = true;
            // 
            // ilIcons
            // 
            this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
            this.ilIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilIcons.Images.SetKeyName(0, "Download");
            this.ilIcons.Images.SetKeyName(1, "Up");
            this.ilIcons.Images.SetKeyName(2, "Work");
            this.ilIcons.Images.SetKeyName(3, "Close");
            // 
            // btnSendMulti
            // 
            this.btnSendMulti.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSendMulti.FlatAppearance.BorderSize = 0;
            this.btnSendMulti.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendMulti.ImageKey = "Download";
            this.btnSendMulti.ImageList = this.ilIcons;
            this.btnSendMulti.Location = new System.Drawing.Point(113, 17);
            this.btnSendMulti.Name = "btnSendMulti";
            this.btnSendMulti.Size = new System.Drawing.Size(27, 27);
            this.btnSendMulti.TabIndex = 81;
            this.btnSendMulti.UseVisualStyleBackColor = false;
            this.btnSendMulti.Click += new System.EventHandler(this.btnSendMulti_Click);
            // 
            // pbSendMulti
            // 
            this.pbSendMulti.Location = new System.Drawing.Point(11, 238);
            this.pbSendMulti.MaximumSize = new System.Drawing.Size(128, 16);
            this.pbSendMulti.MinimumSize = new System.Drawing.Size(128, 16);
            this.pbSendMulti.Name = "pbSendMulti";
            this.pbSendMulti.Size = new System.Drawing.Size(128, 16);
            this.pbSendMulti.TabIndex = 84;
            // 
            // MultiSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(149, 262);
            this.Controls.Add(this.pbSendMulti);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSendMulti);
            this.Controls.Add(this.nudTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudFrom);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MultiSettingForm";
            this.ShowInTaskbar = false;
            this.Text = "Multi Setting";
            ((System.ComponentModel.ISupportInitialize)(this.nudFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThresoldMulti)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudFrom;
        private System.Windows.Forms.NumericUpDown nudTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox cbTypeMulti;
        private System.Windows.Forms.NumericUpDown nudThresoldMulti;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.DomainUpDown dudNoteMulti;
        private System.Windows.Forms.Label lblRimNote;
        private System.Windows.Forms.Button btnSendMulti;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.ImageList ilIcons;
        private LedBar pbSendMulti;
    }
}