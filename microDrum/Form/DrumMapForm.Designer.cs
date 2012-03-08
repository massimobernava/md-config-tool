namespace microDrum
{
    partial class DrumMapForm
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
            this.lbPads = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.nudHead = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudRim = new System.Windows.Forms.NumericUpDown();
            this.cbSingle = new System.Windows.Forms.CheckBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudHead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRim)).BeginInit();
            this.SuspendLayout();
            // 
            // lbPads
            // 
            this.lbPads.FormattingEnabled = true;
            this.lbPads.Location = new System.Drawing.Point(12, 12);
            this.lbPads.Name = "lbPads";
            this.lbPads.Size = new System.Drawing.Size(126, 316);
            this.lbPads.TabIndex = 0;
            this.lbPads.SelectedIndexChanged += new System.EventHandler(this.lbPads_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(144, 276);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(144, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // nudHead
            // 
            this.nudHead.Location = new System.Drawing.Point(186, 64);
            this.nudHead.Maximum = new decimal(new int[] {
            47,
            0,
            0,
            0});
            this.nudHead.Name = "nudHead";
            this.nudHead.Size = new System.Drawing.Size(33, 20);
            this.nudHead.TabIndex = 5;
            this.nudHead.Value = new decimal(new int[] {
            47,
            0,
            0,
            0});
            this.nudHead.ValueChanged += new System.EventHandler(this.nudHead_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(141, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Head:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Rim:";
            // 
            // nudRim
            // 
            this.nudRim.Location = new System.Drawing.Point(186, 90);
            this.nudRim.Maximum = new decimal(new int[] {
            47,
            0,
            0,
            0});
            this.nudRim.Name = "nudRim";
            this.nudRim.Size = new System.Drawing.Size(33, 20);
            this.nudRim.TabIndex = 7;
            this.nudRim.Value = new decimal(new int[] {
            47,
            0,
            0,
            0});
            this.nudRim.ValueChanged += new System.EventHandler(this.nudRim_ValueChanged);
            // 
            // cbSingle
            // 
            this.cbSingle.AutoSize = true;
            this.cbSingle.Location = new System.Drawing.Point(141, 116);
            this.cbSingle.Name = "cbSingle";
            this.cbSingle.Size = new System.Drawing.Size(55, 17);
            this.cbSingle.TabIndex = 9;
            this.cbSingle.Text = "Single";
            this.cbSingle.UseVisualStyleBackColor = true;
            this.cbSingle.CheckedChanged += new System.EventHandler(this.cbSingle_CheckedChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(144, 38);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(75, 20);
            this.txtName.TabIndex = 10;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Name:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(154, 139);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 23);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(183, 139);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(23, 23);
            this.btnRemove.TabIndex = 13;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // DrumMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(223, 333);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cbSingle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudRim);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudHead);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lbPads);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DrumMapForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DrumMap";
            ((System.ComponentModel.ISupportInitialize)(this.nudHead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbPads;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown nudHead;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudRim;
        private System.Windows.Forms.CheckBox cbSingle;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
    }
}