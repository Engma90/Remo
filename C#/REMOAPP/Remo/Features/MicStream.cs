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
    public partial class MicStream : Form, Feature
    {
        
        static WaveOut waveOut;
        static BufferedWaveProvider bufferedWaveProvider = null;
        UDPReceiver dgt;
        mTCPHandler mTCPH;

        public IMainClient c{get;set;}

        public MicStream()
        {
            InitializeComponent();
            this.mTCPH = mTCPHandler.GetInstance();
        }

        private void MicStream_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            start();
        }

        private void start()
        {
            Console.WriteLine("Mic UDPServer!");
            mTCPH.send("Start-Mic", c.tcpClient);
            waveOut = new WaveOut();
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(8000, 16, 1));
            bufferedWaveProvider.BufferDuration = TimeSpan.FromSeconds(20);
            waveOut.Init(bufferedWaveProvider);

            dgt = UDPReceiver.GetInstance(4447);
            dgt.UDPDataReceived += Dgt_UDPDataReceived;
            dgt.ReceiveMessages();
        }

        private void Dgt_UDPDataReceived(object sender, UDPReceiver.UDPDataEventArgs e)
        {
            try
            {

                bufferedWaveProvider.AddSamples(e.data, 0, e.data.Length);
                if(bufferedWaveProvider.BufferedDuration >= TimeSpan.FromSeconds(5))
                {
                    waveOut.Play();
                }
                else
                {
                    waveOut.Pause();
                }
                
            }
            catch {
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            dgt.StopReceive();
            waveOut.Stop();
            waveOut.Dispose();
            bufferedWaveProvider = null;
            Console.WriteLine();
            Console.WriteLine("The server was stopped!");
        }

        public void updateData(byte[] data)
        {
            //throw new NotImplementedException();
            Console.WriteLine("Mic updateData Called");
        }

        public void onError(String error)
        {

        }
    }
}
