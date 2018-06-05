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

        ////public DataHandler()
        ////{

        ////}



        public static void distribute(int dataType,byte[] data, IMClient c)
        {
            //Console.WriteLine(eDataType.DATA_TYPE_INFO);
            try {

                if (dataType == (int)eDataType.INIT_CONNECTION)
                {
                    int Connection_type, Feature_type;
                    Console.WriteLine(Encoding.UTF8.GetString(data));
                    Int32.TryParse(Encoding.UTF8.GetString(data).Split(',')[0], out Connection_type);
                    Int32.TryParse(Encoding.UTF8.GetString(data).Split(',')[1], out Feature_type);
                    if (Connection_type == (int)eConnectionType.Main)
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
                        try
                        {
                            mTCPHandler.GetInstance().MainClientsDict.Remove((c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
                            //        //mc.tcpClient.Client.Disconnect(false);
                            //        break;
                            //    }
                            //}

                            mTCPHandler.GetInstance().MainClientsDict.Add((c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString(), c);
                            c.LastChecked = DateTime.Now;
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("MainClientsDict Add Exception: "+ex.Message);
                        }


                    }
                    else if (Connection_type == (int)eConnectionType.Feature)
                    {
                        ////IFClient fc = mTCPHandler.GetInstance().getClientByIP(
                        ////        (c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients[Feature_type];
                        ////fc.tcpClient = c.tcpClient;
                        ////try
                        ////{
                        IFClient fc = mTCPHandler.GetInstance().getClientByIP((c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients[Feature_type];
                        fc.tcpClient = c.tcpClient;
                        mTCPHandler.GetInstance().FeatureClientsMapDict.Add(
                            c.tcpClient.Client.RemoteEndPoint.ToString(),
                            mTCPHandler.GetInstance().getClientByIP(
                                (c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()));
                        ////    //c = mTCPHandler.GetInstance().getClientByIP(
                        ////    //    (c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients[Feature_type];

                        ////}
                        ////catch (Exception ex)
                        ////{
                        ////    Console.WriteLine("FeatureClientsMapDict Add Exception: " + ex.Message);
                        ////}


                        //          IFClient fc = (IFClient)c;
                        //          //c1.tcpClient = c.tcpClient;


                        //          if( mTCPHandler.GetInstance().getClientByIP((fc.tcpClient.Client.RemoteEndPoint as IPEndPoint)
                        //              .ToString())
                        //              .FeatureClients
                        //              .ContainsKey(Feature_type)){
                        //              fc.tcpClient = c.tcpClient;
                        //          }


                        //          foreach (IMClient mc in mTCPHandler.GetInstance().MainClientsDict.Values.ToList())
                        //          {
                        //              if ((mc.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString().Equals(
                        //                  (fc.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()
                        //                  ))
                        //              {

                        ////                  fc.MainConnection = mc;
                        //                  //fc.initFeature(Feature_type);
                        //                  if (mc.FeatureClients.ContainsKey(Feature_type))
                        //                  {
                        //                      //mc.FeatureClients.Remove(Feature_type);
                        //                      fc.tcpClient = c.tcpClient;
                        //                  }
                        //                  //mc.FeatureClients.Add(Feature_type, fc);

                        //                  break;
                        //              }
                        //          }
                        //          //c.FeatureClients[dataType].MainConnection.FeatureClients.Add(dataType, c);
                        //          //c.initFeature(dataType);

                    }



                }

                else if (dataType == (int)eDataType.INFO)
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
                    //mTCPHandler.GetInstance().send("OK", c.tcpClient);
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
                        //Console.WriteLine("Start Try");
                        



                        IFClient fc = (IFClient)c;
                        //if (!fc.MainConnection.FeatureClients.ContainsKey(dataType))
                        //{
                        //    fc.MainConnection.FeatureClients.Add(dataType, fc);
                        //    var types = Assembly
                        //    .GetExecutingAssembly()
                        //    .GetTypes()
                        //    .Where(t => (t.GetInterface("IFeature")) != null);//t.Namespace.StartsWith("Remo.Features") && 
                        //                                                      //int i = 0;
                        //    foreach (var t in types)
                        //    {
                        //        IFeature TempFeature = (IFeature)Activator.CreateInstance(t);

                        //        if (TempFeature.DATA_TYPE == dataType)
                        //        {
                        //            fc.F = TempFeature;
                        //            fc.F.mc = fc.MainConnection;
                        //            Console.WriteLine(t.Name);
                        //            //F.Show();
                        //            //break;
                        //        }

                        //    }
                        //}

                            //Console.WriteLine(fc.MainConnection.FeatureClients[dataType].F.GetType().ToString());
                            //fc.MainConnection.FeatureClients[dataType].F.updateData(data);
                        fc.F.updateData(data);
                        mTCPHandler.GetInstance().send("OK", fc.tcpClient);
                        //Console.WriteLine("end Try");

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
                Console.WriteLine("Distribute Exeption: " + ex.Message);
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
            ERROR
        }
    }
}
