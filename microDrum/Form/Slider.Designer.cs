namespace microDrum
{
    partial class Slider
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
            this.lblValue = new System.Windows.Forms.Label();
            this.nudBase = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudBase)).BeginInit();
            this.SuspendLayout();
            // 
            // lblValue
            // 
            this.lblValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Calibri", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblValue.Location = new System.Drawing.Point(48, 4);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(20, 11);
            this.lblValue.TabIndex = 0;
            this.lblValue.Text = "000";
            // 
            // nudBase
            // 
            this.nudBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudBase.Location = new System.Drawing.Point(0, 0);
            this.nudBase.Name = "nudBase";
            this.nudBase.Size = new System.Drawing.Size(66, 20);
            this.nudBase.TabIndex = 1;
            this.nudBase.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Slider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudBase);
            this.Controls.Add(this.lblValue);
            this.Name = "Slider";
            this.Size = new System.Drawing.Size(66, 20);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Slider_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Slider_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Slider_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Slider_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.nudBase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.NumericUpDown nudBase;
    }
}
