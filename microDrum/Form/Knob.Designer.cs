namespace microDrum
{
    partial class Knob
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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 50000;
            this.toolTip.InitialDelay = 10;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // Knob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.MaximumSize = new System.Drawing.Size(32, 32);
            this.MinimumSize = new System.Drawing.Size(32, 32);
            this.Name = "Knob";
            this.Size = new System.Drawing.Size(32, 32);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Knob_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Knob_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Knob_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Knob_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
    }
}
