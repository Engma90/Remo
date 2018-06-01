using Remo.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{
    public class m1Client
    {
        //public UDPReceiver udpr;
        public string MANUFACTURER;
        public string BATTERY_LEVEL;
        public bool isMainConn { get; set; }

        private mTCPHandler mTCPH;
        public TcpClient tcpClient { get; set; }
        public CamStream cam;
        public MicStream Mic;
        public FileMan FM;
        public DateTime LastChecked;
        public m1Client()
        {
            mTCPH = mTCPHandler.GetInstance();
            if (isMainConn)
            {
                cam = new CamStream();
                Mic = new MicStream();
                FM = new FileMan();
            }
        }

        public string[] getInfo()
        {
            return new string[] { tcpClient.Client.RemoteEndPoint.ToString(), MANUFACTURER, BATTERY_LEVEL };
        }

        public m1Client getMainConnection()
        {
            //foreach (mClient c in mTCPH.MainClients.ToList())
            //{
            //    if ((c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString().Equals((this.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()) && c.isMainConn)
            //    {
            //        return c;
            //    }
            //}
            return null;
        }
    }
}
