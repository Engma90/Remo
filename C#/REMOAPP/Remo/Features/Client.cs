using Remo.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Remo.Features
{
    class Client : IClient
    {
        public string BATTERY_LEVEL { get; set; }

        public Feature cam { get; set; }

        public Feature FM { get; set; }

        public bool isMainConn { get; set; }

        public DateTime LastChecked { get; set; }

        public IClient MainConnection { get; set; }

        public string MANUFACTURER { get; set; }

        public Feature Mic { get; set; }

        public mTCPHandler mTCPH { get; set; }

        public TcpClient tcpClient { get; set; }

        public string[] getInfo()
        {
            return new string[] { tcpClient.Client.RemoteEndPoint.ToString(), MANUFACTURER, BATTERY_LEVEL };
        }

    }
}
