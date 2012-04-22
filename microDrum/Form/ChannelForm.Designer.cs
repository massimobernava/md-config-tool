namespace microDrum
{
    partial class ChannelForm
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
            this.lstChannel = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudChannel = new System.Windows.Forms.NumericUpDown();
            this.lbChannels = new microDrum.LedBar();
            this.sendChannels = new microDrum.SendButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannel)).BeginInit();
            this.SuspendLayout();
            // 
            // lstChannel
            // 
            this.lstChannel.FormattingEnabled = true;
            this.lstChannel.Location = new System.Drawing.Point(2, 9);
            this.lstChannel.Name = "lstChannel";
            this.lstChannel.Size = new System.Drawing.Size(161, 329);
            this.lstChannel.TabIndex = 0;
            this.lstChannel.SelectedIndexChanged += new System.EventHandler(this.lstChannel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 354);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Channel:";
            // 
            // nudChannel
            // 
            this.nudChannel.Enabled = false;
            this.nudChannel.Location = new System.Drawing.Point(73, 352);
            this.nudChannel.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudChannel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChannel.Name = "nudChannel";
            this.nudChannel.Size = new System.Drawing.Size(57, 20);
            this.nudChannel.TabIndex = 6;
            this.nudChannel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChannel.ValueChanged += new System.EventHandler(this.nudChannel_ValueChanged);
            // 
            // lbChannels
            // 
            this.lbChannels.Location = new System.Drawing.Point(2, 384);
            this.lbChannels.MaximumSize = new System.Drawing.Size(128, 16);
            this.lbChannels.MinimumSize = new System.Drawing.Size(128, 16);
            this.lbChannels.Name = "lbChannels";
            this.lbChannels.Size = new System.Drawing.Size(128, 16);
            this.lbChannels.TabIndex = 2;
            // 
            // sendChannels
            // 
            this.sendChannels.Location = new System.Drawing.Point(136, 373);
            this.sendChannels.Name = "sendChannels";
            this.sendChannels.Size = new System.Drawing.Size(27, 27);
            this.sendChannels.TabIndex = 1;
            this.sendChannels.Click += new System.EventHandler(this.sendChannels_Click);
            // 
            // ChannelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(165, 403);
            this.Controls.Add(this.nudChannel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbChannels);
            this.Controls.Add(this.sendChannels);
            this.Controls.Add(this.lstChannel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChannelForm";
            this.Text = "ChannelMap";
            ((System.ComponentModel.ISupportInitialize)(this.nudChannel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstChannel;
        private SendButton sendChannels;
        private LedBar lbChannels;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudChannel;
    }
}