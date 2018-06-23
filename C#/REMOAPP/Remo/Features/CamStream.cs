using Remo.Connections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
        public IConnection MainConnection { get; set; }

        public int DATA_TYPE { get; set; }
        private int Rotation = 0;
        private static double size;
        private bool Recording = false;
        private string RecordingPath = "";
        private long i = 0;

        private void CamStream_Load(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            comboBox1.SelectedIndex = 0;
            btnStart.Enabled = true;
            comboBox1.Enabled = true;
            trackBar1.Enabled = false;
            btnRR.Enabled = false;
            btnRL.Enabled = false;
            btnStop.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            comboBox1.Enabled = false;
            trackBar1.Enabled = true;
            btnRR.Enabled = true;
            btnRL.Enabled = true;
            btnStop.Enabled = true;
            start();
        }

        private void start()
        {
            Console.WriteLine("Cam Start");
            MainForm.mTCPH.send(((int)DataHandler.eDataType.CAM).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(),trackBar1.Value+"/"+ comboBox1.SelectedIndex,
                MainConnection.tcpClient);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            comboBox1.Enabled = true;
            trackBar1.Enabled = false;
            btnRR.Enabled = false;
            btnRL.Enabled = false;
            btnStop.Enabled = false;
            Stop();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = true;
                if(fbd.ShowDialog() == DialogResult.OK)
                {

                    RecordingPath = fbd.SelectedPath;
                    Recording = true;
                }

            }
            else
            {
                Recording = false;
                i = 0;

                Process ffmpeg = new Process
                {
                    StartInfo = {
                    FileName = "ffmpeg",
                    Arguments = "-f image2 -r 15 -i IM_%07d.bmp -vcodec libx264 out.mp4",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = RecordingPath
                }
               };

                ffmpeg.EnableRaisingEvents = true;
                ffmpeg.OutputDataReceived += (s, e1) => Console.WriteLine(e1.Data);
                ffmpeg.ErrorDataReceived += (s, e1) => Console.WriteLine($@"Error: {e1.Data}");
                ffmpeg.Start();
                ffmpeg.BeginOutputReadLine();
                ffmpeg.BeginErrorReadLine();
                ffmpeg.WaitForExit();




            }

        }

        private void CamStream_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }
        public void Stop()
        {
            Console.WriteLine("Cam Stop");
            MainForm.mTCPH.send(((int)DataHandler.eDataType.CAM).ToString(),
                ((int)DataHandler.eOrderType.STOP).ToString(),
                    MainConnection.tcpClient);
        }

        public void onDataReceived(int Flag, byte[] data)
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
                    if (Recording)
                    {
                        try
                        {
                            //bm.Save(RecordingPath + "\\IM_" + (i++).ToString().PadLeft(7, '0')
                            //    //+ DateTime.Now.Day + "-"
                            //    //+ DateTime.Now.Month + "-"
                            //    //+ DateTime.Now.Year + "_"
                            //    //+ DateTime.Now.Hour + "-"
                            //    //+ DateTime.Now.Minute + "-"
                            //    //+ DateTime.Now.Second + "-"
                            //    //+ DateTime.Now.Millisecond

                            //    + ".jpg", ImageFormat.Jpeg);

                            bm.Save(RecordingPath + "\\IM_" + (i++).ToString().PadLeft(7, '0')
                                //+ DateTime.Now.Day + "-"
                                //+ DateTime.Now.Month + "-"
                                //+ DateTime.Now.Year + "_"
                                //+ DateTime.Now.Hour + "-"
                                //+ DateTime.Now.Minute + "-"
                                //+ DateTime.Now.Second + "-"
                                //+ DateTime.Now.Millisecond

                                + ".bmp",ImageFormat.Bmp);


                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Path:" + RecordingPath + "\\Im" + DateTime.Now + ":" + DateTime.Now.Millisecond + ".jpg");
                            Console.WriteLine("SAVE IMAGE EX:" + e.Message);
                        }
                    }
                    pictureBox1.Image = bm;
                    size += data.Length;
                   // Console.WriteLine(DateTime.Now + "   " + data.Length);

                }
                catch
                {
                }
            });


        }

        public void onErrorReceived(String error)
        {

        }

        private void btnRL_Click(object sender, EventArgs e)
        {
            int temp = Math.Abs(Rotation - (90));
            //if (temp < 0)
            //    Rotation = 270;
            Rotation = (Math.Abs(360 - temp)) % 360;
            Console.WriteLine(Rotation.ToString());

        }

        private void btnRR_Click(object sender, EventArgs e)
        {
            int temp = Rotation += 90;
            //if (temp >= 360)
            //    Rotation = 0;
            Rotation = Math.Abs(temp) % 360;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            MainForm.mTCPH.send(((int)DataHandler.eDataType.CAM).ToString(),
            ((int)DataHandler.eOrderType.UPDATE).ToString(),trackBar1.Value.ToString(),
            MainConnection.tcpClient);
        }
    }
}
