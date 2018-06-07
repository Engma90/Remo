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
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnMic = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCam = new System.Windows.Forms.Button();
            this.btnFM = new System.Windows.Forms.Button();
            this.btnContacts = new System.Windows.Forms.Button();
            this.btnSMS = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.camToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.micToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contactsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.callLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
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
            this.btnMic.Location = new System.Drawing.Point(193, 309);
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
            this.dgv1.ContextMenuStrip = this.contextMenuStrip1;
            this.dgv1.Location = new System.Drawing.Point(12, 12);
            this.dgv1.MultiSelect = false;
            this.dgv1.Name = "dgv1";
            this.dgv1.ReadOnly = true;
            this.dgv1.RowHeadersVisible = false;
            this.dgv1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv1.Size = new System.Drawing.Size(601, 291);
            this.dgv1.TabIndex = 4;
            this.dgv1.SelectionChanged += new System.EventHandler(this.dgv1_SelectionChanged);
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
            this.btnCam.Location = new System.Drawing.Point(279, 309);
            this.btnCam.Name = "btnCam";
            this.btnCam.Size = new System.Drawing.Size(75, 23);
            this.btnCam.TabIndex = 5;
            this.btnCam.Text = "Cam";
            this.btnCam.UseVisualStyleBackColor = true;
            this.btnCam.Click += new System.EventHandler(this.btnCam_Click);
            // 
            // btnFM
            // 
            this.btnFM.Location = new System.Drawing.Point(360, 309);
            this.btnFM.Name = "btnFM";
            this.btnFM.Size = new System.Drawing.Size(75, 23);
            this.btnFM.TabIndex = 6;
            this.btnFM.Text = "FM";
            this.btnFM.UseVisualStyleBackColor = true;
            this.btnFM.Click += new System.EventHandler(this.btnFM_Click);
            // 
            // btnContacts
            // 
            this.btnContacts.Location = new System.Drawing.Point(441, 309);
            this.btnContacts.Name = "btnContacts";
            this.btnContacts.Size = new System.Drawing.Size(75, 23);
            this.btnContacts.TabIndex = 7;
            this.btnContacts.Text = "Contacts";
            this.btnContacts.UseVisualStyleBackColor = true;
            this.btnContacts.Click += new System.EventHandler(this.btnContacts_Click);
            // 
            // btnSMS
            // 
            this.btnSMS.Location = new System.Drawing.Point(522, 309);
            this.btnSMS.Name = "btnSMS";
            this.btnSMS.Size = new System.Drawing.Size(75, 23);
            this.btnSMS.TabIndex = 8;
            this.btnSMS.Text = "SMS";
            this.btnSMS.UseVisualStyleBackColor = true;
            this.btnSMS.Click += new System.EventHandler(this.btnSMS_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.camToolStripMenuItem,
            this.micToolStripMenuItem,
            this.fileManagerToolStripMenuItem,
            this.contactsToolStripMenuItem,
            this.sMSToolStripMenuItem,
            this.callLogToolStripMenuItem,
            this.gPSToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 180);
            // 
            // camToolStripMenuItem
            // 
            this.camToolStripMenuItem.Name = "camToolStripMenuItem";
            this.camToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.camToolStripMenuItem.Text = "Cam";
            this.camToolStripMenuItem.Click += new System.EventHandler(this.camToolStripMenuItem_Click);
            // 
            // micToolStripMenuItem
            // 
            this.micToolStripMenuItem.Name = "micToolStripMenuItem";
            this.micToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.micToolStripMenuItem.Text = "Mic";
            this.micToolStripMenuItem.Click += new System.EventHandler(this.micToolStripMenuItem_Click);
            // 
            // fileManagerToolStripMenuItem
            // 
            this.fileManagerToolStripMenuItem.Name = "fileManagerToolStripMenuItem";
            this.fileManagerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fileManagerToolStripMenuItem.Text = "File Manager";
            this.fileManagerToolStripMenuItem.Click += new System.EventHandler(this.fileManagerToolStripMenuItem_Click);
            // 
            // contactsToolStripMenuItem
            // 
            this.contactsToolStripMenuItem.Name = "contactsToolStripMenuItem";
            this.contactsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.contactsToolStripMenuItem.Text = "Contacts";
            this.contactsToolStripMenuItem.Click += new System.EventHandler(this.contactsToolStripMenuItem_Click);
            // 
            // sMSToolStripMenuItem
            // 
            this.sMSToolStripMenuItem.Name = "sMSToolStripMenuItem";
            this.sMSToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sMSToolStripMenuItem.Text = "SMS";
            this.sMSToolStripMenuItem.Click += new System.EventHandler(this.sMSToolStripMenuItem_Click);
            // 
            // callLogToolStripMenuItem
            // 
            this.callLogToolStripMenuItem.Name = "callLogToolStripMenuItem";
            this.callLogToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.callLogToolStripMenuItem.Text = "Call log";
            this.callLogToolStripMenuItem.Click += new System.EventHandler(this.callLogToolStripMenuItem_Click);
            // 
            // gPSToolStripMenuItem
            // 
            this.gPSToolStripMenuItem.Name = "gPSToolStripMenuItem";
            this.gPSToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gPSToolStripMenuItem.Text = "GPS";
            this.gPSToolStripMenuItem.Click += new System.EventHandler(this.gPSToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 344);
            this.Controls.Add(this.btnSMS);
            this.Controls.Add(this.btnContacts);
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
            this.contextMenuStrip1.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnContacts;
        private System.Windows.Forms.Button btnSMS;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem camToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem micToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contactsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sMSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem callLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gPSToolStripMenuItem;
    }
}

