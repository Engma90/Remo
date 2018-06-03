using Remo.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{

    static class DataHandler
    {

        ////public DataHandler()
        ////{

        ////}



        public static void distribute(int dataType,byte[] data, IMClient c)
        {
            //Console.WriteLine(eDataType.DATA_TYPE_INFO);
            try {

                if (dataType == (int)eDataType.DATA_TYPE_INIT_CONNECTION)
                {
                    int Connection_type, Feature_type;
                    Console.WriteLine(Encoding.UTF8.GetString(data));
                    Int32.TryParse(Encoding.UTF8.GetString(data).Split(',')[0], out Connection_type);
                    Int32.TryParse(Encoding.UTF8.GetString(data).Split(',')[1], out Feature_type);
                    if (Connection_type == (int)eConnectionType.connection_type_Main)
                    {
                        //IMainClient c1 = (MainClient)Convert.ChangeType(c, typeof(MainClient));

                        //IMainClient c1 = new MainClient();
                        //Console.WriteLine(c1.GetType().ToString());
                        //c1.tcpClient = c.tcpClient;
                        //foreach (IMainClient mc in mTCPHandler.GetInstance().MainClientsDict.Values.ToList())
                        //{

                        //    if ((mc.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString().Equals(
                        //        (c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()
                        //        ))
                        //    {
                        mTCPHandler.GetInstance().MainClientsDict.Remove(c.tcpClient);
                        //        //mc.tcpClient.Client.Disconnect(false);
                        //        break;
                        //    }
                        //}

                        mTCPHandler.GetInstance().MainClientsDict.Add(c.tcpClient, c);




                    }
                    else if (Connection_type == (int)eConnectionType.connection_type_Feature)
                    {
                        IFClient fc = (IFClient)c;
                        //c1.tcpClient = c.tcpClient;

                        foreach (IMClient mc in mTCPHandler.GetInstance().MainClientsDict.Values.ToList())
                        {
                            if ((mc.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString().Equals(
                                (fc.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()
                                ))
                            {

                                fc.MainConnection = mc;
                                //fc.initFeature(Feature_type);
                                if (mc.FeatureClients.ContainsKey(Feature_type))
                                {
                                    //mc.FeatureClients.Remove(Feature_type);
                                    fc.tcpClient = c.tcpClient;
                                }
                                //mc.FeatureClients.Add(Feature_type, fc);

                                break;
                            }
                        }
                        //c.FeatureClients[dataType].MainConnection.FeatureClients.Add(dataType, c);
                        //c.initFeature(dataType);

                    }



                }

                else if (dataType == (int)eDataType.DATA_TYPE_INFO)
                {
                    Console.WriteLine("MainClientsCount:" + mTCPHandler.GetInstance().MainClientsDict.Values.Count);
                    Console.WriteLine("insideInfo:");

                    Console.WriteLine(Encoding.UTF8.GetString(data));
                    //IMainClient c1 = (IMainClient)c;
                    c.MANUFACTURER = Encoding.UTF8.GetString(data).Split('/')[0];
                    c.BATTERY_LEVEL = Encoding.UTF8.GetString(data).Split('/')[1];
                    c.LastChecked = DateTime.Now;
                    //c.isMainConn = true;
                    //c.MainConnection = c;
                    Console.WriteLine("Last Cheked Updated: " + DateTime.Now);
                }

                else {
                    //IFeatureClient c1 = (IFeatureClient)c;

                    //if (mTCPHandler.GetInstance().MainClients.Exists(x => c == x))
                    //{
                    //    mTCPHandler.GetInstance().MainClients.Remove(c);
                    //}

                    //IFeatureClient c2 = (IFeatureClient)c;
                    //c2.F.updateData(data);

                    //else if (dataType == (int)eDataType.DATA_TYPE_CAM_START)
                    //{
                    //if(!c.MainConnection.FeatureClients.ContainsKey(dataType))
                    //    c.MainConnection.FeatureClients.Add(dataType, c);
                    try
                    {
                        Console.WriteLine("Start Try");
                        IFClient fc = (IFClient)c;

                        //Console.WriteLine(fc.MainConnection.FeatureClients[dataType].F.GetType().ToString());
                        //fc.MainConnection.FeatureClients[dataType].F.updateData(data);
                        fc.F.updateData(data);
                        Console.WriteLine("end Try");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("No Feature Found Exception " + ex.Message);
                    }


                    //}

                    //else if (dataType == (int)eDataType.DATA_TYPE_MIC_START)
                    //{
                    //    c.MainConnection.Mic.updateData(data);

                    //}
                }
            }catch(Exception ex)
            {
                Console.WriteLine("distribute Exeption: " + ex.Message);
            }
            

        }


        //public enum eFeatures
        //{
        //    FEATURE_CAM,
        //    FEATURE_Mic,
        //    FEATURE_FM,
        //    FEATURE_FD,
        //}


        public enum eConnectionType
        {
            connection_type_Main,
            connection_type_Feature

        }
        public enum eDataType
        {
            DATA_TYPE_CAM_START,
            DATA_TYPE_MIC_START,
            DATA_TYPE_FM_LIST,
            DATA_TYPE_FM_DOWN_START,
            DATA_TYPE_INFO,
            DATA_TYPE_INIT_CONNECTION,
            DATA_TYPE_ERROR,
            DATA_TYPE_CAM_STOP,
            DATA_TYPE_MIC_STOP,
            DATA_TYPE_FM_DOWN_STOP
        }
    }
}
