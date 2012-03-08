namespace microDrum
{
    partial class KeyboardForm
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
            this.pbKeyboard = new System.Windows.Forms.PictureBox();
            this.vsbKeyboard = new System.Windows.Forms.VScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.pbKeyboard)).BeginInit();
            this.SuspendLayout();
            // 
            // pbKeyboard
            // 
            this.pbKeyboard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbKeyboard.BackColor = System.Drawing.Color.Black;
            this.pbKeyboard.Location = new System.Drawing.Point(0, 0);
            this.pbKeyboard.Name = "pbKeyboard";
            this.pbKeyboard.Size = new System.Drawing.Size(207, 625);
            this.pbKeyboard.TabIndex = 0;
            this.pbKeyboard.TabStop = false;
            this.pbKeyboard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbKeyboard_MouseDown);
            this.pbKeyboard.Paint += new System.Windows.Forms.PaintEventHandler(this.pbKeyboard_Paint);
            // 
            // vsbKeyboard
            // 
            this.vsbKeyboard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vsbKeyboard.Location = new System.Drawing.Point(210, 0);
            this.vsbKeyboard.Maximum = 1440;
            this.vsbKeyboard.Name = "vsbKeyboard";
            this.vsbKeyboard.Size = new System.Drawing.Size(19, 625);
            this.vsbKeyboard.TabIndex = 1;
            this.vsbKeyboard.Value = 600;
            this.vsbKeyboard.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsbKeyboard_Scroll);
            // 
            // KeyboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 625);
            this.Controls.Add(this.vsbKeyboard);
            this.Controls.Add(this.pbKeyboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyboardForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Keyboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KeyboardForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbKeyboard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbKeyboard;
        private System.Windows.Forms.VScrollBar vsbKeyboard;
    }
}