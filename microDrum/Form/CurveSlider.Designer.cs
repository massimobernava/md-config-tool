namespace microDrum
{
    partial class CurveSlider
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
            this.timerPaint = new System.Windows.Forms.Timer(this.components);
            this.cbBase = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // timerPaint
            // 
            this.timerPaint.Enabled = true;
            this.timerPaint.Interval = 30;
            this.timerPaint.Tick += new System.EventHandler(this.timerPaint_Tick);
            // 
            // cbBase
            // 
            this.cbBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBase.FormattingEnabled = true;
            this.cbBase.Items.AddRange(new object[] {
            "Linear",
            "Exp",
            "Log",
            "Sigma",
            "Flat"});
            this.cbBase.Location = new System.Drawing.Point(0, 0);
            this.cbBase.Name = "cbBase";
            this.cbBase.Size = new System.Drawing.Size(100, 21);
            this.cbBase.TabIndex = 41;
            this.cbBase.SelectedIndexChanged += new System.EventHandler(this.cbBase_SelectedIndexChanged);
            // 
            // CurveSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbBase);
            this.Name = "CurveSlider";
            this.Size = new System.Drawing.Size(100, 21);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CurveSlider_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CurveSlider_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CurveSlider_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerPaint;
        private System.Windows.Forms.ComboBox cbBase;
    }
}
