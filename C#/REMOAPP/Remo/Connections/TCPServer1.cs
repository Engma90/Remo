//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using SimpleTCP;
//using System.Net.Sockets;
//using Remo.Connections;
//using System.Net;
//using System.Threading;
//using System.Reflection;

//namespace Remo.Connections
//{

//    public class TCPServer1 : SimpleTcpServer, TCP
//    {

//        //public Dictionary<TcpClient,IMConnection> MainConnectionsDict { get; }
//        public Dictionary<string, IMConnection> MainConnectionsDict { get; }
//        public Dictionary<string, IMConnection> FeatureConnectionsMapDict { get; }//string = IFConnection ip
//        public int CheckIsConnectedInterval_ms { get; set; } = 5000;
//        public int Port { get; set; }
//        private Thread AckClientsThread;
//        private static volatile TCPServer1 instance = null;
//        private static object syncRoot = new Object();
//        bool StopFlag = false;
//        public IMConnection ClientClass { get; set; }

//        private TCPServer1()
//        {
//            this.Port = MainForm.Port;
//            ClientConnected += Server_ClientConnected;
//            ClientDisconnected += Server_ClientDisconnected;
//            DataReceived += Server_DataReceived;
//            MainConnectionsDict = new Dictionary<string, IMConnection>();
//            FeatureConnectionsMapDict = new Dictionary<string, IMConnection>();
//            StartServer(this.Port);
//            Console.WriteLine("TCP Server Started!");
//        }
//        public void StartServer(int port)
//        {
//            Start(Port);

//            AckClientsThread = new Thread(delegate ()
//            {
//                while (!StopFlag)
//                {
//                    try
//                    {
//                        //Broadcast(Encoding.UTF8.GetBytes("Info\n"));
//                        foreach (IMConnection MainConnection in MainConnectionsDict.Values.ToList())
//                        {
//                            //if (MainConnection.isMainConn)
//                            //{
//                            send(((int)DataHandler.eDataType.INFO).ToString(), MainConnection.tcpClient);
//                            //Thread.Sleep(10);
//                            if ((DateTime.Now - MainConnection.LastChecked) > TimeSpan.FromMilliseconds(CheckIsConnectedInterval_ms * 2))
//                            {
//                                MainConnectionsDict.Remove((MainConnection.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
//                                //Console.WriteLine(MainConnection.LastChecked);
//                                //Console.WriteLine(DateTime.Now);
//                                foreach (IFConnection fc in MainConnection.FeatureClients.Values.ToList())
//                                {
//                                    fc.F = null;
//                                    fc.tcpClient.Client.Disconnect(false);
//                                    FeatureConnectionsMapDict.Remove(fc.tcpClient.Client.RemoteEndPoint.ToString());
//                                }
//                                MainConnection.tcpClient.Client.Disconnect(false);
//                            }
//                            // }
//                        }
//                    }
//                    catch (Exception ex) { Console.WriteLine("Broadcast Exception: " + ex.Message); }
//                    Thread.Sleep(CheckIsConnectedInterval_ms);
//                }
//            });

//            //AckClientsThread.Start();
//        }

//        public static TCPServer1 GetInstance()
//        {

//            if (instance == null)
//            {
//                lock (syncRoot)
//                {
//                    if (instance == null)
//                        instance = new TCPServer1();
//                }
//            }

//            return instance;

//        }

//        private int SwapEndianness(int value)
//        {
//            int num1 = value & (int)byte.MaxValue;
//            int num2 = value >> 8 & (int)byte.MaxValue;
//            int num3 = value >> 16 & (int)byte.MaxValue;
//            int num4 = value >> 24 & (int)byte.MaxValue;
//            int num5 = 24;
//            return num1 << num5 | num2 << 16 | num3 << 8 | num4;
//        }

//        private int readInt(byte[] data)
//        {
//            try
//            {
//                int retInt = -1;
//                byte[] MessageLength = new byte[4];
//                for (int i = 0; i < 4; i++)
//                {
//                    MessageLength[i] = data[i];
//                }
//                retInt = SwapEndianness(BitConverter.ToInt32(MessageLength, 0));
//                return retInt;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("ReadIntException");
//                return -1;
//            }

//        }

//        public byte[] readMessage(TcpClient client, int length)
//        {
//            try
//            {
//                int offset = 0;
//                int size = length;
//                byte[] numArray = new byte[length];
//                while (offset < length)
//                {
//                    int num2 = client.Client.Receive(numArray, offset, size, SocketFlags.None);
//                    if (num2 != 0)
//                    {
//                        offset += num2;
//                        size -= num2;
//                    }
//                    else
//                        break;
//                }
//                return numArray;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("readMessageException");
//                return null;
//            }
//        }


//        public void Server_ClientConnected(object sender, TcpClient e)
//        {
//            ////throw new NotImplementedException();
//            ////foreach (mClient c1 in MainClients.ToList())
//            ////{
//            ////    if ((c1.tcpClient.MainClient.RemoteEndPoint as IPEndPoint).Address.ToString().Equals((e.MainClient.RemoteEndPoint as IPEndPoint).Address.ToString())  &&  c1.isMainConn)
//            ////    {
//            ////        MainClients.Remove(c1);
//            ////        Console.WriteLine("Duplicates removed");
//            ////    }
//            ////}


//            //Console.WriteLine("MainClient Connected: " + e.Client.RemoteEndPoint);
//            //IMConnection MainConnection = (IMConnection)Activator.CreateInstance(ClientClass.GetType());///////////////////////
//            //MainConnection.tcpClient = e;
//            //MainConnection.isMainConn = false;
//            //MainConnection.LastChecked = DateTime.Now;
//            //Clients.Add(MainConnection);


//        }

//        //bool lockWasTaken = false;
//        // static int MessageLengthInt = 0;
//        public void Server_DataReceived(object sender, Message e)
//        {

//            try
//            {
//                // Monitor.Enter(GetInstance(), ref lockWasTaken);
//                Console.WriteLine();
//                int MessageLengthInt = readInt(e.Data);

//                Console.WriteLine("DataLength " + MessageLengthInt);
//                if (MessageLengthInt > 20000)
//                {
//                    Console.WriteLine("Packet length > max");
//                    return;
//                }
//                byte[] data = readMessage(e.TcpClient, MessageLengthInt);

//                int DataType = readInt(data);
//                Console.WriteLine("DataType: " + DataType);
//                var myEnumMemberCount = Enum.GetNames(typeof(DataHandler.eDataType)).Length;
//                if (DataType > myEnumMemberCount || DataType < 0)
//                {
//                    // e.TcpClient.GetStream().Flush();
//                    return;
//                }

//                byte[] finalData = new byte[MessageLengthInt - 4];

//                Buffer.BlockCopy(data, 4, finalData, 0, finalData.Length);



//                try
//                {

//                    IMConnection c;
//                    if (FeatureConnectionsMapDict.ContainsKey(e.TcpClient.Client.RemoteEndPoint.ToString()))
//                    {
//                        Console.WriteLine("Found in FMap Dict");
//                        c = FeatureConnectionsMapDict[e.TcpClient.Client.RemoteEndPoint.ToString()];
//                        c.tcpClient = e.TcpClient;
//                    }


//                    else if (
//                        null != getMainConnectionByIP((e.TcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString())
//                        &&
//                        getMainConnectionByIP((e.TcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients.ContainsKey(DataType))
//                    {
//                        Console.WriteLine("Found initialized Before");
//                        c = (IMConnection)getMainConnectionByIP((e.TcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients[DataType];
//                        c.tcpClient = e.TcpClient;

//                    }

//                    else if (MainConnectionsDict.ContainsKey((e.TcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()))
//                    {
//                        Console.WriteLine("Found in Main Dict");
//                        c = MainConnectionsDict[(e.TcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()];
//                    }
//                    else
//                    {
//                        Console.WriteLine("NotBoundedBefore");
//                        c = (IMConnection)Activator.CreateInstance(ClientClass.GetType());
//                        c.tcpClient = e.TcpClient;
//                    }
//                    Console.WriteLine("Distributing to Client : " + c.tcpClient.Client.RemoteEndPoint.ToString());
//                    DataHandler.distribute(DataType, finalData, c);

//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("Bound Client Exception: " + ex.Message);
//                }

//                // Monitor.Exit(this);

//                //    }
//                //}

//                //Monitor.Exit(GetInstance());
//                Console.WriteLine();

//            }
//            catch { }
//            finally
//            {
//                //if (lockWasTaken)
//                //{
//                // Monitor.Exit(GetInstance());
//                // }
//            }
//        }

//        private void Server_ClientDisconnected(object sender, TcpClient e)
//        {
//            Console.WriteLine("MainClient Disconnected: " + e.Client.RemoteEndPoint);
//            MainConnectionsDict.Remove(e.Client.RemoteEndPoint.ToString());
//            //foreach (IMainClient MainConnection in MainConnectionsDict.Values.ToList())
//            //{
//            //    if(MainConnection.tcpClient == e)
//            //    {
//            //        MainClients.Remove(MainConnection);

//            //    }
//            //}

//        }


//        public IMConnection getMainConnectionByIP(String ip)
//        {
//            IMConnection ret;
//            MainConnectionsDict.TryGetValue(ip, out ret);
//            return ret;

//        }

//        public IFeature startFeature(string MainClientIP, int Feature_type)
//        {
//            IFConnection fc = (IFConnection)(IMConnection)Activator.CreateInstance(ClientClass.GetType());                                                                // {
//    //        fc.MainConnection = getMainConnectionByIP(MainClientIP);//
//            getMainConnectionByIP(MainClientIP).FeatureClients.Add(Feature_type, fc);
//            var types = Assembly
//            .GetExecutingAssembly()
//            .GetTypes()
//            .Where(t => (t.GetInterface("IFeature")) != null);//t.Namespace.StartsWith("Remo.Features") && 

//            foreach (var t in types)
//            {
//                IFeature TempFeature = (IFeature)Activator.CreateInstance(t);

//                if (TempFeature.DATA_TYPE == Feature_type)
//                {
//                    fc.F = TempFeature;
//          //          fc.F.MainConnection = fc.MainConnection;          //
//                    Console.WriteLine(t.Name);
//                    return TempFeature;
//                }

//            }
//            return null;
//        }



//        public void send(string Message, string OrderType, string Parameters, object c)
//        {
//            ((TcpClient)c).Client.Send(Encoding.UTF8.GetBytes(Message + ":" + OrderType + ":" + Parameters + "\n"));

//        }
//        public void send(string Message, string OrderType, object c)
//        {
//            ((TcpClient)c).Client.Send(Encoding.UTF8.GetBytes(Message + ":" + OrderType + "\n"));

//        }
//        public void send(string Message, object c)
//        {
//            ((TcpClient)c).Client.Send(Encoding.UTF8.GetBytes(Message + "\n"));

//        }


//        public void StopServer()
//        {
//            try
//            {

//                Stop();
//                StopFlag = true;
//                AckClientsThread.Abort();
//                instance = null;
//            }
//            catch { }
//        }

//        public void ClientHandler(object obj)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
