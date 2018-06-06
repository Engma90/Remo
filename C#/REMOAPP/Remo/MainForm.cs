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
        TCP mTCPH;
        //UDPReceiver udpr;
        //mUDPHandler mUDPH;
        private Thread RefreshClientsListThread;
        bool Stop = false;
        public static int Port = 4447;
        //private string SelectedClientIP = "";

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

            mTCPH.addFClient(dgv1.SelectedRows[0].Cells[0].Value.ToString(), (int)DataHandler.eDataType.MIC).Show();

        }

        private void btnCam_Click(object sender, EventArgs e)
        {
            mTCPH.addFClient(dgv1.SelectedRows[0].Cells[0].Value.ToString(), (int)DataHandler.eDataType.CAM).Show();
        }
        

        private void btnFM_Click(object sender, EventArgs e)
        {
            mTCPH.addFClient(dgv1.SelectedRows[0].Cells[0].Value.ToString(), (int)DataHandler.eDataType.FM_LIST).Show();
        }


        private void RefreshClientsList()
        {
            int saveRow = -1;
            if (dgv1.Rows.Count > 0 && dgv1.SelectedRows[0] != null)
                saveRow = dgv1.SelectedRows[0].Index;
            dgv1.Rows.Clear();
                foreach (IMClient c in mTCPH.MainClientsDict.Values.ToList())
                {
                        dgv1.Rows.Add(c.getInfo());
                }
            if (saveRow != -1 && saveRow < dgv1.Rows.Count)
                dgv1.Rows[saveRow].Selected = true;
            //  dgv1.Rows[Selected].Selected = true;


        }

        private IMClient getSelectedClient()
        {
            Console.WriteLine(dgv1.SelectedRows[0].Cells[0].Value.ToString());
            return mTCPH.getClientByIP(dgv1.SelectedRows[0].Cells[0].Value.ToString());
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
            //try
            //{
            //    SelectedClientIP = dgv1.CurrentCell.OwningRow.Cells[0].Value.ToString();
            //}
            //catch { }
        }
    }
}
