namespace Remo
{
    partial class MainForm
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnMic = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCam = new System.Windows.Forms.Button();
            this.btnFM = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 309);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(72, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnMic
            // 
            this.btnMic.Location = new System.Drawing.Point(231, 309);
            this.btnMic.Name = "btnMic";
            this.btnMic.Size = new System.Drawing.Size(80, 23);
            this.btnMic.TabIndex = 1;
            this.btnMic.Text = "Mic";
            this.btnMic.UseVisualStyleBackColor = true;
            this.btnMic.Click += new System.EventHandler(this.btnMic_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(90, 309);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToAddRows = false;
            this.dgv1.AllowUserToDeleteRows = false;
            this.dgv1.AllowUserToResizeRows = false;
            this.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgv1.Location = new System.Drawing.Point(12, 12);
            this.dgv1.MultiSelect = false;
            this.dgv1.Name = "dgv1";
            this.dgv1.ReadOnly = true;
            this.dgv1.RowHeadersVisible = false;
            this.dgv1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv1.Size = new System.Drawing.Size(601, 291);
            this.dgv1.TabIndex = 4;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "IP/Port";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Manufacturer";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Battery Level";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // btnCam
            // 
            this.btnCam.Location = new System.Drawing.Point(317, 309);
            this.btnCam.Name = "btnCam";
            this.btnCam.Size = new System.Drawing.Size(75, 23);
            this.btnCam.TabIndex = 5;
            this.btnCam.Text = "Cam";
            this.btnCam.UseVisualStyleBackColor = true;
            this.btnCam.Click += new System.EventHandler(this.btnCam_Click);
            // 
            // btnFM
            // 
            this.btnFM.Location = new System.Drawing.Point(398, 309);
            this.btnFM.Name = "btnFM";
            this.btnFM.Size = new System.Drawing.Size(75, 23);
            this.btnFM.TabIndex = 6;
            this.btnFM.Text = "FM";
            this.btnFM.UseVisualStyleBackColor = true;
            this.btnFM.Click += new System.EventHandler(this.btnFM_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 344);
            this.Controls.Add(this.btnFM);
            this.Controls.Add(this.btnCam);
            this.Controls.Add(this.dgv1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnMic);
            this.Controls.Add(this.btnStart);
            this.Name = "MainForm";
            this.Text = "RemoApp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnMic;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button btnCam;
        private System.Windows.Forms.Button btnFM;
    }
}

