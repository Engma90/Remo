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
        
        string MANUFACTURER { get; set; }
        string BATTERY_LEVEL { get; set; }

        Dictionary<int, IFClient> FeatureClients { get; set; }


        

        string[] getInfo();

        //IFeatureClient[] FC { get; set; }
        //bool isMainConn { get; set; }
        //Dictionary< int , IFeature> Features { get; set; }
        //IFeature cam { get; set; }
        //IFeature Mic { get; set; }
        //IFeature FM { get; set; }
        //IMainClient MainConnection { get; set; }

    }

    public interface IFClient : IClient
    {
        IMClient MainConnection { get; set; }
        //void initFeature(int dataType);
        IFeature F { get; set; }
    }
}
