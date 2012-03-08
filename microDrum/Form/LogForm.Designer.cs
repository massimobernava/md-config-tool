namespace microDrum
{
    partial class LogForm
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
            this.pbLog = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLog)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLog
            // 
            this.pbLog.BackColor = System.Drawing.Color.White;
            this.pbLog.Location = new System.Drawing.Point(12, 12);
            this.pbLog.Name = "pbLog";
            this.pbLog.Size = new System.Drawing.Size(1000, 256);
            this.pbLog.TabIndex = 0;
            this.pbLog.TabStop = false;
            this.pbLog.Paint += new System.Windows.Forms.PaintEventHandler(this.pbLog_Paint);
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 274);
            this.Controls.Add(this.pbLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LogForm";
            this.Text = "LogForm";
            ((System.ComponentModel.ISupportInitialize)(this.pbLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLog;
    }
}