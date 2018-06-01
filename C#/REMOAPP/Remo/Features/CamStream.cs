using Remo.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remo.Features
{
    public partial class CamStream : Form,Feature
    {
        mTCPHandler mTCPH;
        //DataHandler dh;
        //UDPReceiver dgt;
        public CamStream()
        {
            InitializeComponent();
            this.mTCPH = mTCPHandler.GetInstance();
        }
        public IClient c
        {
            get;
            set;
        }

        private void CamStream_Load(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //dgt = new UDPReceiver(4447);
            //dgt.UDPDataReceived += Dgt_UDPDataReceived;
            //dgt.ReceiveMessages();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            start();
        }

        private void start()
        {
            Console.WriteLine("Cam UDPServer!");
            mTCPH.send(((int)DataHandler.eDataType.DATA_TYPE_CAM_START).ToString(), c.tcpClient);
        }

        //private void Dgt_UDPDataReceived(object sender, UDPReceiver.UDPDataEventArgs e)
        //{
        //    try
        //    {
        //        Console.WriteLine("received "+ e.data.Length);
        //        MemoryStream mStream = new MemoryStream();
        //        byte[] pData = e.data;
        //        mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
        //        Bitmap bm = new Bitmap(mStream, false);
        //        mStream.Dispose();

        //        pictureBox1.Image = bm;

        //    }
        //    catch
        //    {
        //    }

        //}

        private void btnStop_Click(object sender, EventArgs e)
        {
            
            mTCPH.send(((int)DataHandler.eDataType.DATA_TYPE_CAM_STOP).ToString(), c.tcpClient);
            //dgt.StopReceive();
            Console.WriteLine();
            Console.WriteLine("The Cam server was stopped!");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CamStream_FormClosing(object sender, FormClosingEventArgs e)
        {
            //dgt.StopReceive();
            try
            {
                mTCPH.send(((int)DataHandler.eDataType.DATA_TYPE_CAM_STOP).ToString(), c.tcpClient);
            }
            catch { }
            Console.WriteLine();
            Console.WriteLine("The Cam server was stopped!");
        }

        public void updateData(byte[] data)
        {
            try
            {
                Console.WriteLine("received " + data.Length);
                MemoryStream mStream = new MemoryStream();
                byte[] pData = data;
                mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                Bitmap bm = new Bitmap(mStream, false);
                mStream.Dispose();

                pictureBox1.Image = bm;

            }
            catch
            {
            }
        }

        public void onError(String error)
        {

        }
    }
}
