using Remo.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{
    public interface IConnection
    {
        TcpClient tcpClient { get; set; }
        DateTime LastChecked { get; set; }

    }
    public interface IMConnection : IConnection
    {
        Dictionary<int, IFConnection> FeatureClients { get; set; }

    }

    public interface IFConnection : IConnection
    {
        IFeature F { get; set; }
    }
}
