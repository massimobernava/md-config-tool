namespace microDrum
{
    partial class UpdateForm
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
            this.btnHex = new System.Windows.Forms.Button();
            this.txtHex = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.btnArduino = new System.Windows.Forms.Button();
            this.txtArduino = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtTrace = new System.Windows.Forms.TextBox();
            this.nudCOM = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioUNO = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudCOM)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHex
            // 
            this.btnHex.Location = new System.Drawing.Point(300, 36);
            this.btnHex.Name = "btnHex";
            this.btnHex.Size = new System.Drawing.Size(25, 23);
            this.btnHex.TabIndex = 13;
            this.btnHex.Text = "...";
            this.btnHex.UseVisualStyleBackColor = true;
            this.btnHex.Click += new System.EventHandler(this.btnHex_Click);
            // 
            // txtHex
            // 
            this.txtHex.Location = new System.Drawing.Point(55, 38);
            this.txtHex.Name = "txtHex";
            this.txtHex.Size = new System.Drawing.Size(239, 20);
            this.txtHex.TabIndex = 12;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(8, 41);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(52, 13);
            this.label31.TabIndex = 11;
            this.label31.Text = "Firmware:";
            // 
            // btnArduino
            // 
            this.btnArduino.Location = new System.Drawing.Point(300, 10);
            this.btnArduino.Name = "btnArduino";
            this.btnArduino.Size = new System.Drawing.Size(25, 23);
            this.btnArduino.TabIndex = 10;
            this.btnArduino.Text = "...";
            this.btnArduino.UseVisualStyleBackColor = true;
            this.btnArduino.Click += new System.EventHandler(this.btnArduino_Click);
            // 
            // txtArduino
            // 
            this.txtArduino.Location = new System.Drawing.Point(55, 12);
            this.txtArduino.Name = "txtArduino";
            this.txtArduino.Size = new System.Drawing.Size(239, 20);
            this.txtArduino.TabIndex = 9;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(8, 15);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(46, 13);
            this.label30.TabIndex = 8;
            this.label30.Text = "Arduino:";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(250, 76);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 49);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtTrace
            // 
            this.txtTrace.Location = new System.Drawing.Point(11, 131);
            this.txtTrace.Multiline = true;
            this.txtTrace.Name = "txtTrace";
            this.txtTrace.ReadOnly = true;
            this.txtTrace.Size = new System.Drawing.Size(322, 373);
            this.txtTrace.TabIndex = 14;
            // 
            // nudCOM
            // 
            this.nudCOM.Location = new System.Drawing.Point(176, 92);
            this.nudCOM.Name = "nudCOM";
            this.nudCOM.Size = new System.Drawing.Size(68, 20);
            this.nudCOM.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "COM:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioUNO);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(11, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(119, 61);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Arduino";
            // 
            // radioUNO
            // 
            this.radioUNO.AutoSize = true;
            this.radioUNO.Location = new System.Drawing.Point(6, 38);
            this.radioUNO.Name = "radioUNO";
            this.radioUNO.Size = new System.Drawing.Size(49, 17);
            this.radioUNO.TabIndex = 1;
            this.radioUNO.Text = "UNO";
            this.radioUNO.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(87, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Duemilanove";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(345, 516);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudCOM);
            this.Controls.Add(this.txtTrace);
            this.Controls.Add(this.btnHex);
            this.Controls.Add(this.txtHex);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.btnArduino);
            this.Controls.Add(this.txtArduino);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.btnUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update";
            this.VisibleChanged += new System.EventHandler(this.UpdateForm_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.nudCOM)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHex;
        private System.Windows.Forms.TextBox txtHex;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button btnArduino;
        private System.Windows.Forms.TextBox txtArduino;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox txtTrace;
        private System.Windows.Forms.NumericUpDown nudCOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioUNO;

    }
}