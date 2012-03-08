namespace microDrum
{
    partial class NoteSlider
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
            this.dudNoteBase = new System.Windows.Forms.DomainUpDown();
            this.SuspendLayout();
            // 
            // dudNoteBase
            // 
            this.dudNoteBase.Location = new System.Drawing.Point(4, 0);
            this.dudNoteBase.Name = "dudNoteBase";
            this.dudNoteBase.ReadOnly = true;
            this.dudNoteBase.Size = new System.Drawing.Size(65, 20);
            this.dudNoteBase.TabIndex = 36;
            this.dudNoteBase.Text = "---";
            this.dudNoteBase.SelectedItemChanged += new System.EventHandler(this.dudNoteBase_SelectedItemChanged);
            // 
            // NoteSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dudNoteBase);
            this.Name = "NoteSlider";
            this.Size = new System.Drawing.Size(70, 20);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.NoteSlider_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NoteSlider_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NoteSlider_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NoteSlider_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DomainUpDown dudNoteBase;
    }
}
