using Remo.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{
        public interface IClient
        {
        TcpClient tcpClient { get; set; }
        DateTime LastChecked { get; set; }

    }
        public interface IMClient:IClient{
        Dictionary<int, IFClient> FeatureClients { get; set; }

    }

    public interface IFClient : IClient
    {
        //IMClient MainConnection { get; set; }
        IFeature F { get; set; }
    }
}
