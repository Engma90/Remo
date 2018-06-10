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
            DATA_TYPE = (int)DataHandler.eDataType.CAM;
        }
        public IMClient mc { get; set; }

        public int DATA_TYPE { get; set; }
        private int Rotation = 0;
        private static double size;

        private void CamStream_Load(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            start();
        }

        private void start()
        {
            Console.WriteLine("Cam Start");
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.CAM).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(),
                mc.tcpClient);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CamStream_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }
        public void Stop()
        {
            Console.WriteLine("Cam Stop");
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.CAM).ToString(),
                ((int)DataHandler.eOrderType.STOP).ToString(),
                    mc.tcpClient);
        }

        public void updateData(byte[] data)
        {

            this.Invoke((MethodInvoker)delegate
            {


                try
                {
                    //Console.WriteLine("received " + data.Length);
                    MemoryStream mStream = new MemoryStream();
                    byte[] pData = data;
                    mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                    Bitmap bm = new Bitmap(mStream, true);
                    mStream.Dispose();
                    RotateFlipType R = RotateFlipType.RotateNoneFlipNone;
                    if (Rotation == 90)
                    {
                        R = RotateFlipType.Rotate90FlipNone;
                    }
                    else if (Rotation == 180)
                    {
                        R = RotateFlipType.Rotate180FlipNone;
                    }
                    else if (Rotation == 270)
                    {
                        R = RotateFlipType.Rotate270FlipNone;
                    }
                    else
                    {
                        R = RotateFlipType.RotateNoneFlipNone;
                    }

                    bm.RotateFlip(R);
                    pictureBox1.Image = bm;
                    size += data.Length;
                    Console.WriteLine(DateTime.Now + "   " + data.Length);

                }
                catch
                {
                }
            });


        }

        public void onError(String error)
        {

        }

        private void btnRL_Click(object sender, EventArgs e)
        {
            int temp = Rotation -= 90;
            if (temp < 0)
                Rotation = 270;

        }

        private void btnRR_Click(object sender, EventArgs e)
        {
            int temp = Rotation += 90;
            if (temp >= 360)
                Rotation = 0;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.CAM).ToString(),
            ((int)DataHandler.eOrderType.UPDATE).ToString(),trackBar1.Value.ToString(),
            mc.tcpClient);
        }
    }
}
