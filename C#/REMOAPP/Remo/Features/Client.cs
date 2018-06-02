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
        public Client()
        {
            FeatureClients = new Dictionary<int, IClient>();
        }
        public string BATTERY_LEVEL { get; set; }

        public IFeature F { get; set; }

        public Dictionary<int, IClient> FeatureClients { get; set; }

        public DateTime LastChecked { get; set; }

        public IClient MainConnection { get; set; }

        public string MANUFACTURER { get; set; }

        public TcpClient tcpClient { get; set; }

        public string[] getInfo()
        {
            return new string[] { tcpClient.Client.RemoteEndPoint.ToString(), MANUFACTURER, BATTERY_LEVEL };
        }

        public void initFeature(int dataType)
        {
            var types = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.Namespace.StartsWith("Remo.Features") && (t.GetInterface("IFeature")) != null);
            //int i = 0;
            foreach (var t in types)
            {
                IFeature TempFeature = (IFeature)Activator.CreateInstance(t);

                if (TempFeature.DATA_TYPE == dataType)
                {
                    this.F = TempFeature;
                    F.Show();
                    //break;
                }
                Console.WriteLine(t.Name);
            }
        }
    }

    //class MainClient : IMainClient
    //{
    //    public MainClient()
    //    {
    //        IFeatureClients = new Dictionary<int, IFeatureClient>();
    //        //var types = Assembly
    //        //        .GetExecutingAssembly()
    //        //        .GetTypes()
    //        //        .Where(t => t.Namespace.StartsWith("Remo.Features") && (t.BaseType).Name.StartsWith("Form"));
    //        //int i = 0;
    //        //foreach (var t in types)
    //        //{
    //        //    IFeatureClient FC = new FeatureClient();
    //        //    FC.F = (IFeature)Activator.CreateInstance(t);
    //        //    FC.
    //        //    IFeatureClients.Add(((IFeature)t).DATA_TYPE, FC);
    //        //    //FC[(int)DataHandler.eDataType.DATA_TYPE_CAM_START].F = (IFeature)Activator.CreateInstance(t);
    //        //    FC[(int)DataHandler.eDataType.DATA_TYPE_CAM_START].MainConnection = this;
    //        //    Console.WriteLine(t.Name);
    //        //    i++;
    //        //}
    //        //FC = new FeatureClient[4];

    //        //FC[(int)DataHandler.eDataType.DATA_TYPE_CAM_START].F = new CamStream();

    //    }

    //    public string BATTERY_LEVEL { get; set; }

    //    //public IFeature cam { get; set; }

    //    public IFeatureClient[] FC { get; set; }

    //    public Dictionary<int, IFeatureClient> IFeatureClients { get; set; }

    //    //public IFeature FM { get; set; }

    //    //public bool isMainConn { get; set; }

    //    public DateTime LastChecked { get; set; }

    //    //public IMainClient MainConnection { get; set; }

    //    public string MANUFACTURER { get; set; }

    //    //public IFeature Mic { get; set; }

    //    //public mTCPHandler mTCPH { get; set; }

    //    public TcpClient tcpClient { get; set; }

    //    //Dictionary<int, IFeature> IMainClient.Features { get; set; }

    //    public string[] getInfo()
    //    {
    //        return new string[] { tcpClient.Client.RemoteEndPoint.ToString(), MANUFACTURER, BATTERY_LEVEL };
    //    }

    //}

    //class FeatureClient : IFeatureClient
    //{
    //    public IFeature F { get; set; }



    //    public TcpClient tcpClient { get; set; }

    //    public IMainClient MainConnection { get; set; }

    //    public void initFeature(int dataType)
    //    {
    //        var types = Assembly
    //                .GetExecutingAssembly()
    //                .GetTypes()
    //                .Where(t => t.Namespace.StartsWith("Remo.Features") && (t.GetInterface("IFeature"))!=null);
    //        //int i = 0;
    //        foreach (var t in types)
    //        {
    //            IFeature TempFeature = (IFeature)Activator.CreateInstance(t);

    //            if (TempFeature.DATA_TYPE == dataType)
    //            {
    //                this.F = TempFeature;
    //                F.Show();
    //                //break;
    //            }
    //            Console.WriteLine(t.Name);
    //        }
    //    }
    //}
}
