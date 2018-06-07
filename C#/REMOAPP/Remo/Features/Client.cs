using Remo.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Reflection;
using System.Net;

namespace Remo.Features
{

    class Client : IClient,IMClient,IFClient
    {
        public IMClient MainConnection { get; set; }
        public Dictionary<int, IFClient> FeatureClients { get; set; }
        public IFeature F { get; set; }
        public DateTime LastChecked { get; set; }
        public TcpClient tcpClient { get; set; }
        public Client()
        {
            FeatureClients = new Dictionary<int, IFClient>();
        }
    }
}
