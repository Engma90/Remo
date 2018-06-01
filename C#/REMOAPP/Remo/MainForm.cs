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
        mUDPHandler mUDPH;
        private Thread RefreshThread;
        bool Stop = false;
        public static int Port = 4447;

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
            mTCPH = mTCPHandler.GetInstance();

            mTCPH.ClientClass = new Client();
            //udpr = UDPReceiver.GetInstance(4447);
            //Thread.Sleep(5000);

            //mUDPH = mUDPHandler.GetInstance();

            RefreshThread = new Thread(delegate ()
            {
                while (!Stop)
                {
                    refresView();
                    Thread.Sleep(5000);
                }
            });

            RefreshThread.Start();
        }



        private void btnMic_Click(object sender, EventArgs e)
        {
            getSelectedClient().Mic.c = getSelectedClient();
            getSelectedClient().Mic.Show();
        }

        private void btnCam_Click(object sender, EventArgs e)
        {
            //getSelectedClient().cam = new CamStream();
            getSelectedClient().cam.c = getSelectedClient();
            getSelectedClient().cam.Show();
            //CamStream cam = new CamStream(mTCPH);
            //cam.updateData(new byte[10]);
            //cam.Show();

        }

        private void btnFM_Click(object sender, EventArgs e)
        {
            getSelectedClient().FM.c = getSelectedClient();
            getSelectedClient().FM.Show();
        }


        private void refresView()
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
                foreach (IClient c in mTCPH.Clients.ToList())
                {
                    if (c.isMainConn)
                    {
                        dgv1.Rows.Add(c.getInfo());
                    }

                }
                dgv1.Rows[Selected].Selected = true;
            }
            catch { }

        }

        private IClient getSelectedClient()
        {

            foreach (IClient c in mTCPH.Clients.ToList())
            {
                if( dgv1.CurrentCell.OwningRow.Cells[0].Value.ToString().Equals(
                    c.tcpClient.Client.RemoteEndPoint.ToString()) && c.isMainConn)
                {
                    Console.WriteLine("getSelectedClient : Found");
                    return c;
                }

            }
            return mTCPH.Clients[0];
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
                RefreshThread.Abort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
