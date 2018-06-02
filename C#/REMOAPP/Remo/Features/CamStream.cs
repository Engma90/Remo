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
    public partial class CamStream : Form,IFeature
    {
        public CamStream()
        {
            InitializeComponent();

            DATA_TYPE = (int)DataHandler.eDataType.DATA_TYPE_CAM_START;
        }
        public IMainClient c { get; set; }

        public int DATA_TYPE { get; set; }

        private void CamStream_Load(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            start();
        }

        private void start()
        {
            Console.WriteLine("Cam Start");
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.DATA_TYPE_CAM_START).ToString(), c.tcpClient);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {

            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.DATA_TYPE_CAM_STOP).ToString(), c.tcpClient);
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
            try
            {
                mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.DATA_TYPE_CAM_STOP).ToString(), c.tcpClient);
            }
            catch { }
            Console.WriteLine();
            Console.WriteLine("The Cam server was stopped!");
        }

        public void updateData(byte[] data)
        {
            try
            {
                //Console.WriteLine("received " + data.Length);
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
