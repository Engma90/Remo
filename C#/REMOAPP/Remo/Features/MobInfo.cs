using Remo.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Features
{
    public class MobInfo:IFeature 
    {
        public int DATA_TYPE {get; set;}
        public string MANUFACTURER { get; set; }
        public string BATTERY_LEVEL { get; set; }

        public IMConnection MainConnection { get; set; }

        public MobInfo()
        {
            DATA_TYPE = (int)DataHandler.eDataType.INFO;
            Console.WriteLine("Info");

        }

        public void onErrorReceived(string error)
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            // throw new NotImplementedException();
            mTCPHandler.GetInstance().send(((int)DataHandler.eDataType.INFO).ToString(),
                ((int)DataHandler.eOrderType.START).ToString(),"aa",
                    MainConnection.tcpClient);
        }

        public void onDataReceived(byte[] data)
        {
            MANUFACTURER = Encoding.UTF8.GetString(data).Split('/')[0];
            BATTERY_LEVEL = Encoding.UTF8.GetString(data).Split('/')[1];
        }


        public string[] getInfo()
        {
            return new string[] { (MainConnection.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString(), MANUFACTURER, BATTERY_LEVEL };
        }

    }
}
