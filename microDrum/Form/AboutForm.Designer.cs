namespace microDrum
{
    partial class AboutForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tbSoftwareKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbLicenseKey = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.imageListButton = new System.Windows.Forms.ImageList(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPayPal = new System.Windows.Forms.Button();
            this.btnHomePage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Calibri", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(2, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 82);
            this.label1.TabIndex = 1;
            this.label1.Text = "µ";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Velocette", 27.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(56, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 41);
            this.label2.TabIndex = 2;
            this.label2.Text = "microDRUM";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(205, 38);
            this.label3.TabIndex = 3;
            this.label3.Text = "by Massimo Bernava massimo.bernava@gmail.com ";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(233, 78);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(16, 13);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "---";
            // 
            // tbSoftwareKey
            // 
            this.tbSoftwareKey.Location = new System.Drawing.Point(12, 220);
            this.tbSoftwareKey.Name = "tbSoftwareKey";
            this.tbSoftwareKey.ReadOnly = true;
            this.tbSoftwareKey.Size = new System.Drawing.Size(276, 20);
            this.tbSoftwareKey.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "SoftwareKey:";
            // 
            // tbLicenseKey
            // 
            this.tbLicenseKey.Location = new System.Drawing.Point(12, 259);
            this.tbLicenseKey.Multiline = true;
            this.tbLicenseKey.Name = "tbLicenseKey";
            this.tbLicenseKey.Size = new System.Drawing.Size(276, 104);
            this.tbLicenseKey.TabIndex = 8;
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(12, 369);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(75, 23);
            this.btnRegister.TabIndex = 9;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "LicenseKey:";
            // 
            // imageListButton
            // 
            this.imageListButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListButton.ImageStream")));
            this.imageListButton.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListButton.Images.SetKeyName(0, "home.gif");
            this.imageListButton.Images.SetKeyName(1, "x.gif");
            this.imageListButton.Images.SetKeyName(2, "cart.gif");
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ImageKey = "x.gif";
            this.btnClose.ImageList = this.imageListButton;
            this.btnClose.Location = new System.Drawing.Point(272, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(17, 17);
            this.btnClose.TabIndex = 14;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPayPal
            // 
            this.btnPayPal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPayPal.FlatAppearance.BorderSize = 0;
            this.btnPayPal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayPal.ImageKey = "cart.gif";
            this.btnPayPal.ImageList = this.imageListButton;
            this.btnPayPal.Location = new System.Drawing.Point(250, 372);
            this.btnPayPal.Name = "btnPayPal";
            this.btnPayPal.Size = new System.Drawing.Size(17, 17);
            this.btnPayPal.TabIndex = 13;
            this.btnPayPal.UseVisualStyleBackColor = true;
            this.btnPayPal.Click += new System.EventHandler(this.btnPayPal_Click);
            // 
            // btnHomePage
            // 
            this.btnHomePage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHomePage.FlatAppearance.BorderSize = 0;
            this.btnHomePage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHomePage.ImageKey = "home.gif";
            this.btnHomePage.ImageList = this.imageListButton;
            this.btnHomePage.Location = new System.Drawing.Point(272, 372);
            this.btnHomePage.Name = "btnHomePage";
            this.btnHomePage.Size = new System.Drawing.Size(17, 17);
            this.btnHomePage.TabIndex = 12;
            this.btnHomePage.UseVisualStyleBackColor = true;
            this.btnHomePage.Click += new System.EventHandler(this.btnHomePage_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(300, 400);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPayPal);
            this.Controls.Add(this.btnHomePage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.tbLicenseKey);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbSoftwareKey);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AboutForm";
            this.Opacity = 0.95D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AboutForm";
            this.VisibleChanged += new System.EventHandler(this.AboutForm_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TextBox tbSoftwareKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbLicenseKey;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnHomePage;
        private System.Windows.Forms.ImageList imageListButton;
        private System.Windows.Forms.Button btnPayPal;
        private System.Windows.Forms.Button btnClose;
    }
}