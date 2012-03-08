namespace microDrum
{
    partial class LoadButton
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
            this.SuspendLayout();
            // 
            // SendButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SendButton";
            this.Size = new System.Drawing.Size(27, 27);
            this.MouseLeave += new System.EventHandler(this.SendButton_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SendButton_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SendButton_MouseDown);
            this.MouseHover += new System.EventHandler(this.SendButton_MouseHover);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SendButton_MouseUp);
            this.MouseEnter += new System.EventHandler(this.SendButton_MouseEnter);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
