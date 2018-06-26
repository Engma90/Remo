using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Remo.Connections;

namespace Remo.Features
{
    public partial class MicStream : Form, IFeature
    {
        
        static WaveOut waveOut;
        static BufferedWaveProvider bufferedWaveProvider = null;
        //UDPServer dgt;
        //ServerFactory mTCPH;
        bool isPaused = false;

        public IConnection MainConnection{get;set;}

        public int DATA_TYPE { get; set; }

        public MicStream()
        {
            InitializeComponent();
            //this.mTCPH = MainForm.mTCPH;
            DATA_TYPE = (int)DataHandler.eDataType.MIC;
        }

        private void MicStream_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            btnStop.Enabled = false;
            btnPause.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnPause.Enabled = true;
            start();
            isPaused = false;
        }

        private void start()
        {
            if (!isPaused)
            {
                MainForm.mTCPH.send(((int)DataHandler.eDataType.MIC).ToString(),
                    ((int)DataHandler.eOrderType.START).ToString(),
                        MainConnection.tcpClient);
                waveOut = new WaveOut();
                bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));
                bufferedWaveProvider.BufferDuration = TimeSpan.FromMinutes(10);
                waveOut.Init(bufferedWaveProvider);
            }
            else
            {
                waveOut.Play();
            }

            //dgt = UDPServer.GetInstance(4447);
            //dgt.UDPDataReceived += Dgt_UDPDataReceived;
            //dgt.ReceiveMessages();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            isPaused = true;
            btnPause.Enabled = false;
            btnStart.Enabled = true;
            waveOut.Pause();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnPause.Enabled = false;
            btnStart.Enabled = true;
            Stop();

        }

        public void onDataReceived(int Flag, byte[] data)
        {
            try
            {

                bufferedWaveProvider.AddSamples(data, 0, data.Length);
                if (!isPaused)
                {
                    if (bufferedWaveProvider.BufferedDuration >= TimeSpan.FromSeconds(1))
                    {

                        waveOut.Play();
                    }
                    else
                    {
                        waveOut.Pause();
                    }
                }


                this.Invoke((MethodInvoker)delegate
                {
                    toolStripStatusLabel1.Text = "Buffered : "+ bufferedWaveProvider.BufferedDuration.ToString();
                    progressBar1.Value = (int)((float)bufferedWaveProvider.BufferedDuration.Ticks / (float)bufferedWaveProvider.BufferedDuration.Ticks);
                });



            }
            catch
            {
            }
        }

        public void onErrorReceived(String error)
        {

        }

        private void MicStream_FormClosing(object sender, FormClosingEventArgs e)
        {
            isPaused = false;
            Stop();
        }

        public void Stop()
        {
            MainForm.mTCPH.send(((int)DataHandler.eDataType.MIC).ToString(),
                    ((int)DataHandler.eOrderType.STOP).ToString(),"",
                        MainConnection.tcpClient);
            waveOut.Stop();
            waveOut.Dispose();
            bufferedWaveProvider = null;
            Console.WriteLine();
            Console.WriteLine("The MIC was stopped!");
        }


    }
}
