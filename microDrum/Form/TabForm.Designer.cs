namespace microDrum
{
    partial class TabForm
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbPlay = new System.Windows.Forms.ToolStripButton();
            this.tsbStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbMemory = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.timerTempo = new System.Windows.Forms.Timer(this.components);
            this.vScrollBar_Sheet = new System.Windows.Forms.VScrollBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox_Sheet = new System.Windows.Forms.PictureBox();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.pictureBox_Info = new System.Windows.Forms.PictureBox();
            this.tbCorrection = new System.Windows.Forms.TrackBar();
            this.toolStrip.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Sheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Info)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCorrection)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPlay,
            this.tsbStop,
            this.toolStripSeparator1,
            this.tsbMemory,
            this.tsbExport,
            this.toolStripSeparator2,
            this.tsbDelete});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(634, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbPlay
            // 
            this.tsbPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPlay.Image = global::microDrum.Properties.Resources.play;
            this.tsbPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPlay.Name = "tsbPlay";
            this.tsbPlay.Size = new System.Drawing.Size(23, 22);
            this.tsbPlay.Text = "Play";
            this.tsbPlay.Click += new System.EventHandler(this.tsbPlay_Click);
            // 
            // tsbStop
            // 
            this.tsbStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStop.Image = global::microDrum.Properties.Resources.stop;
            this.tsbStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStop.Name = "tsbStop";
            this.tsbStop.Size = new System.Drawing.Size(23, 22);
            this.tsbStop.Text = "Stop";
            this.tsbStop.Click += new System.EventHandler(this.tsbStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbMemory
            // 
            this.tsbMemory.CheckOnClick = true;
            this.tsbMemory.Image = global::microDrum.Properties.Resources.x;
            this.tsbMemory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMemory.Name = "tsbMemory";
            this.tsbMemory.Size = new System.Drawing.Size(72, 22);
            this.tsbMemory.Text = "Memory";
            this.tsbMemory.CheckedChanged += new System.EventHandler(this.tsbMemory_CheckedChanged);
            // 
            // tsbExport
            // 
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExport.Image = global::microDrum.Properties.Resources.calendar;
            this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(23, 22);
            this.tsbExport.Text = "to Excel";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDelete
            // 
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelete.Image = global::microDrum.Properties.Resources.against;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(23, 22);
            this.tsbDelete.Text = "Delete All";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // timerTempo
            // 
            this.timerTempo.Tick += new System.EventHandler(this.timerTempo_Tick);
            // 
            // vScrollBar_Sheet
            // 
            this.vScrollBar_Sheet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar_Sheet.Location = new System.Drawing.Point(611, 3);
            this.vScrollBar_Sheet.Name = "vScrollBar_Sheet";
            this.vScrollBar_Sheet.Size = new System.Drawing.Size(20, 277);
            this.vScrollBar_Sheet.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox_Sheet);
            this.splitContainer1.Panel1.Controls.Add(this.vScrollBar_Sheet);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbCorrection);
            this.splitContainer1.Panel2.Controls.Add(this.hScrollBar1);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox_Info);
            this.splitContainer1.Size = new System.Drawing.Size(634, 389);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.TabIndex = 3;
            // 
            // pictureBox_Sheet
            // 
            this.pictureBox_Sheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Sheet.BackColor = System.Drawing.Color.White;
            this.pictureBox_Sheet.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_Sheet.Name = "pictureBox_Sheet";
            this.pictureBox_Sheet.Size = new System.Drawing.Size(605, 274);
            this.pictureBox_Sheet.TabIndex = 0;
            this.pictureBox_Sheet.TabStop = false;
            this.pictureBox_Sheet.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Sheet_Paint);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(3, 83);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(583, 20);
            this.hScrollBar1.TabIndex = 1;
            // 
            // pictureBox_Info
            // 
            this.pictureBox_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Info.BackColor = System.Drawing.Color.Black;
            this.pictureBox_Info.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_Info.Name = "pictureBox_Info";
            this.pictureBox_Info.Size = new System.Drawing.Size(580, 77);
            this.pictureBox_Info.TabIndex = 0;
            this.pictureBox_Info.TabStop = false;
            this.pictureBox_Info.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Info_Paint);
            // 
            // tbCorrection
            // 
            this.tbCorrection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCorrection.Location = new System.Drawing.Point(589, 3);
            this.tbCorrection.Maximum = 1000;
            this.tbCorrection.Name = "tbCorrection";
            this.tbCorrection.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbCorrection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbCorrection.Size = new System.Drawing.Size(45, 100);
            this.tbCorrection.TabIndex = 4;
            this.tbCorrection.TickFrequency = 100;
            this.tbCorrection.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbCorrection.Value = 500;
            // 
            // TabForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 416);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "TabForm";
            this.Text = "Tab";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TabForm_FormClosing);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Sheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Info)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbCorrection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Sheet;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbPlay;
        private System.Windows.Forms.Timer timerTempo;
        private System.Windows.Forms.VScrollBar vScrollBar_Sheet;
        private System.Windows.Forms.ToolStripButton tsbStop;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox_Info;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.ToolStripButton tsbMemory;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.TrackBar tbCorrection;
    }
}