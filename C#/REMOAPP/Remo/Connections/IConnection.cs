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
        String IP { get; set; }
        String Port { get; set; }
        bool isMainConnection { get; set; }
        DateTime LastChecked { get; set; }
        Dictionary<int, IFeature> Features { get; set; }

    }


    //public interface IMConnection : IConnection
    //{
        

    //}

    //public interface IFConnection : IConnection
    //{
    //    IFeature F { get; set; }
    //}
}
