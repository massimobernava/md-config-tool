namespace microDrum
{
    partial class EffectsChainControl
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
            this.components = new System.ComponentModel.Container();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.contextMenuEffects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.volumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threeBandEQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.badBussMojoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pitchDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuEffects.SuspendLayout();
            this.SuspendLayout();
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(425, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 450);
            this.vScrollBar1.TabIndex = 4;
            // 
            // contextMenuEffects
            // 
            this.contextMenuEffects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.volumeToolStripMenuItem,
            this.threeBandEQToolStripMenuItem,
            this.badBussMojoToolStripMenuItem,
            this.pitchDownToolStripMenuItem});
            this.contextMenuEffects.Name = "contextMenuEffects";
            this.contextMenuEffects.Size = new System.Drawing.Size(153, 114);
            // 
            // volumeToolStripMenuItem
            // 
            this.volumeToolStripMenuItem.Name = "volumeToolStripMenuItem";
            this.volumeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.volumeToolStripMenuItem.Text = "Volume";
            this.volumeToolStripMenuItem.Click += new System.EventHandler(this.volumeToolStripMenuItem_Click);
            // 
            // threeBandEQToolStripMenuItem
            // 
            this.threeBandEQToolStripMenuItem.Name = "threeBandEQToolStripMenuItem";
            this.threeBandEQToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.threeBandEQToolStripMenuItem.Text = "ThreeBandEQ";
            this.threeBandEQToolStripMenuItem.Click += new System.EventHandler(this.threeBandEQToolStripMenuItem_Click);
            // 
            // badBussMojoToolStripMenuItem
            // 
            this.badBussMojoToolStripMenuItem.Name = "badBussMojoToolStripMenuItem";
            this.badBussMojoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.badBussMojoToolStripMenuItem.Text = "BadBussMojo";
            this.badBussMojoToolStripMenuItem.Click += new System.EventHandler(this.badBussMojoToolStripMenuItem_Click);
            // 
            // pitchDownToolStripMenuItem
            // 
            this.pitchDownToolStripMenuItem.Name = "pitchDownToolStripMenuItem";
            this.pitchDownToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pitchDownToolStripMenuItem.Text = "PitchDown";
            this.pitchDownToolStripMenuItem.Click += new System.EventHandler(this.pitchDownToolStripMenuItem_Click);
            // 
            // EffectsChainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vScrollBar1);
            this.Name = "EffectsChainControl";
            this.Size = new System.Drawing.Size(442, 450);
            this.contextMenuEffects.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuEffects;
        private System.Windows.Forms.ToolStripMenuItem volumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem threeBandEQToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem badBussMojoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pitchDownToolStripMenuItem;
    }
}
