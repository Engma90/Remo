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

        string MANUFACTURER { get; set; }
        string BATTERY_LEVEL { get; set; }
        bool isMainConn { get; set; }

        mTCPHandler mTCPH { get; set; }
        TcpClient tcpClient { get; set; }
        DateTime LastChecked { get; set; }
        Feature cam { get; set; }
        Feature Mic { get; set; }
        Feature FM { get; set; }
        IClient MainConnection { get; set; }
        

        string[] getInfo();
        


    }
}
