using Remo.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Reflection;

namespace Remo.Features
{

    class Client : IClient
    {
        public TcpClient tcpClient { get; set; }
    }
    class MainClient : IMainClient
    {
        public MainClient()
        {
            //var types = Assembly
            //        .GetExecutingAssembly()
            //        .GetTypes()
            //        .Where(t => t.Namespace.StartsWith("Remo.Features") && (t.BaseType).Name.StartsWith("Form"));
            //int i = 0;
            //foreach (var t in types)
            //{
            //    FC[(int)DataHandler.eDataType(i)] = new FeatureClient();
            //    FC[(int)DataHandler.eDataType.DATA_TYPE_CAM_START].F = (Feature)Activator.CreateInstance(t);
            //    FC[(int)DataHandler.eDataType.DATA_TYPE_CAM_START].MainConnection = this;
            //    Console.WriteLine(t.Name);
            //    i++;
            //}
            //FC = new FeatureClient[4];
            
            //FC[(int)DataHandler.eDataType.DATA_TYPE_CAM_START].F = new CamStream();
            
        }

        public string BATTERY_LEVEL { get; set; }

        //public Feature cam { get; set; }

        public IFeatureClient[] FC { get; set; }

        //public Feature FM { get; set; }

        //public bool isMainConn { get; set; }

        public DateTime LastChecked { get; set; }

        //public IMainClient MainConnection { get; set; }

        public string MANUFACTURER { get; set; }

        //public Feature Mic { get; set; }

        //public mTCPHandler mTCPH { get; set; }

        public TcpClient tcpClient { get; set; }

        public string[] getInfo()
        {
            return new string[] { tcpClient.Client.RemoteEndPoint.ToString(), MANUFACTURER, BATTERY_LEVEL };
        }

    }

    class FeatureClient : IFeatureClient
    {
        public Feature F { get; set; }



        public TcpClient tcpClient { get; set; }

        public IMainClient getMainConnection()
        {
            throw new NotImplementedException();
        }
    }
}
