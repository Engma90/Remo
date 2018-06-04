using Remo.Connections;
using Remo.Features;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remo
{
    public partial class MainForm : Form
    {
        mTCPHandler mTCPH;
        //UDPReceiver udpr;
        //mUDPHandler mUDPH;
        private Thread RefreshClientsListThread;
        bool Stop = false;
        public static int Port = 4447;
        private string SelectedClientIP = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            //IMClient fc = new Client();
            //fc.initFeature(0);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Start handler
            mTCPH = mTCPHandler.GetInstance();

            mTCPH.ClientClass = new Client();
            //udpr = UDPReceiver.GetInstance(4447);
            //Thread.Sleep(5000);

            //mUDPH = mUDPHandler.GetInstance();

            RefreshClientsListThread = new Thread(delegate ()
            {
                while (!Stop)
                {
                    RefreshClientsList();
                    Thread.Sleep(5000);
                }
            });

            RefreshClientsListThread.Start();
        }



        private void btnMic_Click(object sender, EventArgs e)
        {

            mTCPH.addFClient(SelectedClientIP, (int)DataHandler.eDataType.MIC).Show();

        }

        private void btnCam_Click(object sender, EventArgs e)
        {
            mTCPH.addFClient(SelectedClientIP, (int)DataHandler.eDataType.CAM).Show();
        }
        

        private void btnFM_Click(object sender, EventArgs e)
        {
            mTCPH.addFClient(SelectedClientIP, (int)DataHandler.eDataType.FM_LIST).Show();
        }


        private void RefreshClientsList()
        {
            int Selected = 0;
            try
            {
                try
                {
                    Selected = dgv1.CurrentCell.RowIndex;
                }
                catch { }
                dgv1.Rows.Clear();
                foreach (IMClient c in mTCPH.MainClientsDict.Values.ToList())
                {
                        dgv1.Rows.Add(c.getInfo());


                }
                dgv1.Rows[Selected].Selected = true;
            }
            catch { }

        }

        private IMClient getSelectedClient()
        {
            return mTCPH.getClientByIP(SelectedClientIP);
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
                mTCPH.StopServer();
                mTCPH = null;
                //udpr.StopReceive();
                Stop = true;
                RefreshClientsListThread.Abort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void dgv1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedClientIP = dgv1.CurrentCell.OwningRow.Cells[0].Value.ToString();
            }
            catch { }
        }
    }
}
