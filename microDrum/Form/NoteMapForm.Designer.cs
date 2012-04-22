namespace microDrum
{
    partial class NoteMapForm
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
            this.flPanelMap = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddMap = new System.Windows.Forms.Button();
            this.btnLoadMap = new System.Windows.Forms.Button();
            this.btnSaveMap = new System.Windows.Forms.Button();
            this.btnRemoveMap = new System.Windows.Forms.Button();
            this.btnApplyMap = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flPanelMap
            // 
            this.flPanelMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flPanelMap.AutoScroll = true;
            this.flPanelMap.BackColor = System.Drawing.Color.LightSteelBlue;
            this.flPanelMap.Location = new System.Drawing.Point(5, 7);
            this.flPanelMap.Name = "flPanelMap";
            this.flPanelMap.Size = new System.Drawing.Size(569, 323);
            this.flPanelMap.TabIndex = 8;
            // 
            // btnAddMap
            // 
            this.btnAddMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMap.Location = new System.Drawing.Point(499, 336);
            this.btnAddMap.Name = "btnAddMap";
            this.btnAddMap.Size = new System.Drawing.Size(75, 23);
            this.btnAddMap.TabIndex = 9;
            this.btnAddMap.Text = "Add";
            this.btnAddMap.UseVisualStyleBackColor = true;
            this.btnAddMap.Click += new System.EventHandler(this.btnAddMap_Click);
            // 
            // btnLoadMap
            // 
            this.btnLoadMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadMap.Location = new System.Drawing.Point(86, 336);
            this.btnLoadMap.Name = "btnLoadMap";
            this.btnLoadMap.Size = new System.Drawing.Size(75, 23);
            this.btnLoadMap.TabIndex = 10;
            this.btnLoadMap.Text = "Load";
            this.btnLoadMap.UseVisualStyleBackColor = true;
            this.btnLoadMap.Click += new System.EventHandler(this.btnLoadMap_Click);
            // 
            // btnSaveMap
            // 
            this.btnSaveMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveMap.Location = new System.Drawing.Point(5, 336);
            this.btnSaveMap.Name = "btnSaveMap";
            this.btnSaveMap.Size = new System.Drawing.Size(75, 23);
            this.btnSaveMap.TabIndex = 11;
            this.btnSaveMap.Text = "Save";
            this.btnSaveMap.UseVisualStyleBackColor = true;
            this.btnSaveMap.Click += new System.EventHandler(this.btnSaveMap_Click);
            // 
            // btnRemoveMap
            // 
            this.btnRemoveMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveMap.Location = new System.Drawing.Point(418, 336);
            this.btnRemoveMap.Name = "btnRemoveMap";
            this.btnRemoveMap.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveMap.TabIndex = 12;
            this.btnRemoveMap.Text = "Remove";
            this.btnRemoveMap.UseVisualStyleBackColor = true;
            this.btnRemoveMap.Click += new System.EventHandler(this.btnRemoveMap_Click);
            // 
            // btnApplyMap
            // 
            this.btnApplyMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyMap.Location = new System.Drawing.Point(337, 336);
            this.btnApplyMap.Name = "btnApplyMap";
            this.btnApplyMap.Size = new System.Drawing.Size(75, 23);
            this.btnApplyMap.TabIndex = 13;
            this.btnApplyMap.Text = "Apply";
            this.btnApplyMap.UseVisualStyleBackColor = true;
            this.btnApplyMap.Click += new System.EventHandler(this.btnApplyMap_Click);
            // 
            // NoteMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(579, 371);
            this.Controls.Add(this.btnApplyMap);
            this.Controls.Add(this.btnRemoveMap);
            this.Controls.Add(this.btnSaveMap);
            this.Controls.Add(this.btnLoadMap);
            this.Controls.Add(this.btnAddMap);
            this.Controls.Add(this.flPanelMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NoteMapForm";
            this.Text = "NoteMap";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NoteMapForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flPanelMap;
        private System.Windows.Forms.Button btnAddMap;
        private System.Windows.Forms.Button btnLoadMap;
        private System.Windows.Forms.Button btnSaveMap;
        private System.Windows.Forms.Button btnRemoveMap;
        private System.Windows.Forms.Button btnApplyMap;
    }
}