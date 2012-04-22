namespace microDrum
{
    partial class EffectControl
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
            this.btnAddRemove = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.panelEffect = new System.Windows.Forms.Panel();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.lblEffectName = new System.Windows.Forms.Label();
            this.panelEffect.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddRemove
            // 
            this.btnAddRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRemove.Location = new System.Drawing.Point(395, 3);
            this.btnAddRemove.Name = "btnAddRemove";
            this.btnAddRemove.Size = new System.Drawing.Size(20, 100);
            this.btnAddRemove.TabIndex = 0;
            this.btnAddRemove.Text = "Remove";
            this.btnAddRemove.UseVisualStyleBackColor = true;
            this.btnAddRemove.Click += new System.EventHandler(this.btnAddRemove_Click);
            // 
            // btnUp
            // 
            this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUp.Location = new System.Drawing.Point(3, 3);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(20, 50);
            this.btnUp.TabIndex = 1;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDown.Location = new System.Drawing.Point(3, 53);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(20, 50);
            this.btnDown.TabIndex = 2;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // panelEffect
            // 
            this.panelEffect.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelEffect.Controls.Add(this.chkEnabled);
            this.panelEffect.Controls.Add(this.lblEffectName);
            this.panelEffect.Location = new System.Drawing.Point(29, 3);
            this.panelEffect.Name = "panelEffect";
            this.panelEffect.Size = new System.Drawing.Size(360, 100);
            this.panelEffect.TabIndex = 3;
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Checked = true;
            this.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnabled.Location = new System.Drawing.Point(3, 41);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(65, 17);
            this.chkEnabled.TabIndex = 2;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // lblEffectName
            // 
            this.lblEffectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEffectName.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblEffectName.Location = new System.Drawing.Point(4, 3);
            this.lblEffectName.Name = "lblEffectName";
            this.lblEffectName.Size = new System.Drawing.Size(60, 42);
            this.lblEffectName.TabIndex = 0;
            this.lblEffectName.Text = "---";
            // 
            // EffectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.Controls.Add(this.panelEffect);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnAddRemove);
            this.Name = "EffectControl";
            this.Size = new System.Drawing.Size(421, 106);
            this.panelEffect.ResumeLayout(false);
            this.panelEffect.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddRemove;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Panel panelEffect;
        private System.Windows.Forms.Label lblEffectName;
        private System.Windows.Forms.CheckBox chkEnabled;
    }
}
