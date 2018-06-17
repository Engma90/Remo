using Remo.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Reflection;
using System.Net;

namespace Remo.Connections
{

    class Connection : IConnection//,IMConnection,IFConnection
    {
        public IConnection MainConnection { get; set; }
        public Dictionary<int, IFeature> Features { get; set; }
        // public Dictionary<int, IFConnection> FeatureClients { get; set; }
        // public IFeature F { get; set; }
        public DateTime LastChecked { get; set; }
        public TcpClient tcpClient { get; set; }

        public string IP { get; set; }
        public string Port { get; set; }

        public bool isMainConnection { get; set; }



        public Connection()
        {
            Features = new Dictionary<int, IFeature>();
        }
    }
}
