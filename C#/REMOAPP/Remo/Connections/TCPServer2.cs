using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleTCP;
using System.Reflection;

namespace Remo.Connections
{
    public class TCPServer2 : TCP
    {
        private TcpListener _server;
        private volatile Boolean _isRunning;
        int port = 4447;

        public Dictionary<string, IMClient> MainClientsDict { get; }
        public Dictionary<string, IMClient> FeatureClientsMapDict { get; }//string = IFClient ip
        public int Port { get; set; }
        public int CheckIsConnectedInterval_ms { get; set; } = 5000;
        public IMClient ClientClass { get; set; }

        private TCPServer2()
        {
            MainClientsDict = new Dictionary<string, IMClient>();
            FeatureClientsMapDict = new Dictionary<string, IMClient>();
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();

            _isRunning = true;
            Thread t = new Thread(new ThreadStart(LoopClients));
            t.Start();

            //LoopClients();
        }


        private static readonly object mutex = new object();
        private static TCPServer2 instance = null;

        public static TCP GetInstance()
        {

            if (instance == null)
            {
                lock (mutex)
                {
                    if (instance == null)
                    {
                        instance = new TCPServer2();
                    }
                }
            }

            return instance;

        }




        public void LoopClients()
        {
            Console.WriteLine("TCP Server Started");
            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();

                // client found.
                // create a thread to handle communication
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public void HandleClient(object obj)
        {

            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;
            Console.WriteLine("Client Connected: " + client.Client.RemoteEndPoint.ToString());
            // sets two streams

            //send(((int)DataHandler.eDataType.CAM).ToString(),
            //    ((int)DataHandler.eOrderType.START).ToString(),
            //        client);

            //Thread.Sleep(1000);
            //send(((int)DataHandler.eDataType.MIC).ToString(),
            //    ((int)DataHandler.eOrderType.START).ToString(),
            //        client);




            //       StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.UTF8);
            //       StreamReader sReader = new StreamReader(client.GetStream(), Encoding.UTF8);
            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested

            Boolean bClientConnected = true;
            String sData = null;
            IMClient c = null;
            while (bClientConnected && _isRunning)
            {
        //        Console.WriteLine("Loooop");
                // reads from stream
                //            sData = sReader.ReadLine();
                try
                {
                    byte[] bArray;
                    int n, Length, DataType;
                    //String Messsage = "";
                    bArray = new byte[4];
                    n = client.Client.Receive(bArray, 0, 4, SocketFlags.None);
                    Length = readInt(bArray);
                    /// Console.WriteLine("DataLen = " + Length);

                    n = client.Client.Receive(bArray, 0, 4, SocketFlags.None);
                    DataType = readInt(bArray);
            //        Console.WriteLine("DataType = " + DataType);

                    byte[] data = readMessage(client, Length);
                    // Console.WriteLine(client.Client.RemoteEndPoint.ToString());

                   c = CheckClientExistance2(client, DataType);

              //      Console.WriteLine("Distributing to Client : " + c.tcpClient.Client.RemoteEndPoint.ToString());
                    DataHandler.distribute(DataType, data, c);

                }
                catch(Exception ex)
                {
                    Console.WriteLine("Recive Exception: " + ex.Message);
                    bClientConnected = false;
               //     clientDisconnected(c,client);
                }







            }
        }



        public void clientDisconnected(IMClient mc,TcpClient c)
        {
            try
            {
                if (FeatureClientsMapDict.ContainsKey(c.Client.RemoteEndPoint.ToString()))
                {
                    IFClient fc = (IFClient)mc;
                    fc.F = null;
                    fc.tcpClient.Client.Disconnect(false);
                    FeatureClientsMapDict.Remove(c.Client.RemoteEndPoint.ToString());
                }
                else if (MainClientsDict.ContainsKey((c.Client.RemoteEndPoint as IPEndPoint).Address.ToString()))
                {
                    MainClientsDict.Remove((c.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
                    //Console.WriteLine(mc.LastChecked);
                    //Console.WriteLine(DateTime.Now);
                    foreach (IFClient fc in mc.FeatureClients.Values.ToList())
                    {
                        fc.F = null;
                        fc.tcpClient.Client.Disconnect(false);
                        FeatureClientsMapDict.Remove(fc.tcpClient.Client.RemoteEndPoint.ToString());
                    }
                    mc.tcpClient.Client.Disconnect(false);
                }
            }
            catch { Console.WriteLine("Disconnect Exception"); }
        }



        //public IMClient CheckClientExistance(TcpClient e ,int DataType )
        //{
        //    IMClient c;
        //    try
        //    {
        //        if(DataType == (int)DataHandler.eDataType.INIT_CONNECTION)
        //        {
        //            Console.WriteLine("NotBoundedBefore");
        //            c = (IMClient)Activator.CreateInstance(ClientClass.GetType());
        //            c.tcpClient = e;
        //        }
                

                
        //        return c;

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Bound Client Exception: " + ex.Message);
        //        throw new Exception("Bound Client Exception");
        //    }

        //    // Monitor.Exit(this);

        //    //    }
        //    //}

        //    //Monitor.Exit(GetInstance());
        //    Console.WriteLine();

        //}













        public IMClient CheckClientExistance2(TcpClient e, int DataType)
        {
            IMClient c;
            try
            {


                if (FeatureClientsMapDict.ContainsKey(e.Client.RemoteEndPoint.ToString()))
                {
                    Console.WriteLine("Found in FMap Dict");
                    c = FeatureClientsMapDict[e.Client.RemoteEndPoint.ToString()];
                    c.tcpClient = e;
                }


                else if (
                    null != getClientByIP((e.Client.RemoteEndPoint as IPEndPoint).Address.ToString())
                    &&
                    getClientByIP((e.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients.ContainsKey(DataType))
                {
  //                  Console.WriteLine("Found initialized Before");
                    c = (IMClient)getClientByIP((e.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients[DataType];
                    c.tcpClient = e;

                }

                else if (MainClientsDict.ContainsKey((e.Client.RemoteEndPoint as IPEndPoint).Address.ToString()))
                {
      //              Console.WriteLine("Found in Main Dict");
                    c = MainClientsDict[(e.Client.RemoteEndPoint as IPEndPoint).Address.ToString()];
                }
                else
                {
    //                Console.WriteLine("NotBoundedBefore");
                    c = (IMClient)Activator.CreateInstance(ClientClass.GetType());
                    c.tcpClient = e;
                }

                return c;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bound Client Exception: " + ex.Message);
                throw new Exception("Bound Client Exception");
            }

            // Monitor.Exit(this);

            //    }
            //}

            //Monitor.Exit(GetInstance());
            Console.WriteLine();

        }












        public void Server_ClientConnected(object sender, TcpClient e)
        {
            throw new NotImplementedException();
        }

        public void Server_DataReceived(object sender, Message e)
        {
            throw new NotImplementedException();
        }
        
        public void StartServer(int Port)
        {
            throw new NotImplementedException();
        }

        public void StopServer()
        {
            _isRunning = false;
        }



        public IMClient getClientByIP(String ip)
        {
            IMClient ret;
            MainClientsDict.TryGetValue(ip, out ret);
            return ret;

        }

        public IFeature addFClient(string MainClientIP, int Feature_type)
        {
            IFClient fc = (IFClient)(IMClient)Activator.CreateInstance(ClientClass.GetType());                                                                // {
   //         fc.MainConnection = getClientByIP(MainClientIP);               ////
            try
            {
                getClientByIP(MainClientIP).FeatureClients.Add(Feature_type, fc);
            }
            catch { }
            var types = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => (t.GetInterface("IFeature")) != null);//t.Namespace.StartsWith("Remo.Features") && 

            foreach (var t in types)
            {
                IFeature TempFeature = (IFeature)Activator.CreateInstance(t);

                if (TempFeature.DATA_TYPE == Feature_type)
                {
                    fc.F = TempFeature;
                    fc.F.mc = getClientByIP(MainClientIP);               
                    Console.WriteLine(t.Name);
                    return TempFeature;
                }

            }
            return null;
        }



        public void send(string Message, string OrderType, string Parameters, object c)
        {
            ((TcpClient)c).Client.Send(Encoding.UTF8.GetBytes(Message + ":" + OrderType + ":" + Parameters + "\n"));

        }
        public void send(string Message, string OrderType, object c)
        {
            ((TcpClient)c).Client.Send(Encoding.UTF8.GetBytes(Message + ":" + OrderType + "\n"));

        }
        public void send(string Message, object c)
        {
            ((TcpClient)c).Client.Send(Encoding.UTF8.GetBytes(Message + "\n"));

        }







        private int SwapEndianness(int value)
        {
            int num1 = value & (int)byte.MaxValue;
            int num2 = value >> 8 & (int)byte.MaxValue;
            int num3 = value >> 16 & (int)byte.MaxValue;
            int num4 = value >> 24 & (int)byte.MaxValue;
            int num5 = 24;
            return num1 << num5 | num2 << 16 | num3 << 8 | num4;
        }





        private int readInt(byte[] data)
        {
            int retInt = -1;
            byte[] MessageLength = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                MessageLength[i] = data[i];
            }
            retInt = SwapEndianness(BitConverter.ToInt32(MessageLength, 0));
            return retInt;
        }

        public byte[] readMessage(TcpClient client, int length)
        {
            int offset = 0;
            int size = length;
            byte[] numArray = new byte[length];
            while (offset < length && _isRunning)
            {
                int num2 = client.Client.Receive(numArray, offset, size, SocketFlags.None);
                if (num2 != 0)
                {
                    offset += num2;
                    size -= num2;
                }
                else
                    break;
            }
            return numArray;
        }


    }
}
