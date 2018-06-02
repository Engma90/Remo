using Remo.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{

    public interface IClient{
        TcpClient tcpClient { get; set; }
    }
    public interface IMainClient:IClient
    {
        string MANUFACTURER { get; set; }
        string BATTERY_LEVEL { get; set; }
        //Dictionary< int , IFeature> Features { get; set; }
        Dictionary<int, IFeatureClient> IFeatureClients { get; set; }
        //bool isMainConn { get; set; }
        DateTime LastChecked { get; set; }
        IFeatureClient[] FC { get; set; }
        //IFeature cam { get; set; }
        //IFeature Mic { get; set; }
        //IFeature FM { get; set; }
        //IMainClient MainConnection { get; set; }
        string[] getInfo();
    }

    public interface IFeatureClient: IClient
    {
        IMainClient MainConnection { get; set; }
        IFeature F { get; set; }
        void initFeature(int dataType);
    }
}
