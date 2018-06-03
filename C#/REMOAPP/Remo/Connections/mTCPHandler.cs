using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTCP;
using System.Net.Sockets;
using Remo.Connections;
using System.Net;
using System.Threading;

namespace Remo.Connections
{

    public class mTCPHandler : SimpleTcpServer, TCP
    {
        //public List<TcpClient> HandledClientsList { get; }
        //public List<IMClient> Clients { get; }
        //public List<IMainClient> MainClients { get; }
        public Dictionary<TcpClient,IMClient> MainClientsDict { get; }
        public int CheckIsConnectedInterval_ms { get; set; } = 5000;
        int Port;
        private Thread RefreshThread;
        private static volatile mTCPHandler instance = null;
        private static object syncRoot = new Object();
        bool StopFlag = false;
        public IMClient ClientClass { get; set; }
        //DataHandler dh;

        private mTCPHandler()
        {
            this.Port = MainForm.Port;
            ClientConnected += Server_ClientConnected;
            ClientDisconnected += Server_ClientDisconnected;
            DataReceived += Server_DataReceived;
            //AutoTrimStrings = true;
            //dh = new DataHandler();
            //MainClients = new List<IMainClient>();
            MainClientsDict = new Dictionary<TcpClient, IMClient>();
            //Clients = new List<IMClient>();
            //HandledClientsList = new List<TcpClient>();
            StartServer(this.Port);
            Console.WriteLine("TCP Server Started!");
        }
        public void StartServer(int port)
        {
            Start(Port);

            RefreshThread = new Thread(delegate ()
            {
                while (!StopFlag)
                {
                    try
                    {
                        //Broadcast(Encoding.UTF8.GetBytes("Info\n"));
                        foreach (IMClient c in MainClientsDict.Values.ToList())
                        {
                            //if (c.isMainConn)
                            //{
                                send(((int)DataHandler.eDataType.DATA_TYPE_INFO).ToString(), c.tcpClient);
                            Thread.Sleep(CheckIsConnectedInterval_ms);
                            if ((DateTime.Now - c.LastChecked) > TimeSpan.FromMilliseconds(CheckIsConnectedInterval_ms))
                                {
                                    //MainClients.Remove(c);
                                    //Console.WriteLine(c.LastChecked);
                                    //Console.WriteLine(DateTime.Now);
                                
                                    c.tcpClient.Client.Disconnect(false);
                                }
                           // }
                        }
                    }
                    catch(Exception ex) { Console.WriteLine("Broadcast Exception: "+ ex.Message); }
                    //Thread.Sleep(CheckIsConnectedInterval_ms);
                }
            });

            //RefreshThread.Start();
        }

        public static mTCPHandler GetInstance()
        {
            
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new mTCPHandler();
                    }
                }

                return instance;
            
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
            while (offset < length)
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


        public void Server_ClientConnected(object sender, TcpClient e)
        {
            ////throw new NotImplementedException();
            ////foreach (mClient c1 in MainClients.ToList())
            ////{
            ////    if ((c1.tcpClient.MainClient.RemoteEndPoint as IPEndPoint).Address.ToString().Equals((e.MainClient.RemoteEndPoint as IPEndPoint).Address.ToString())  &&  c1.isMainConn)
            ////    {
            ////        MainClients.Remove(c1);
            ////        Console.WriteLine("Duplicates removed");
            ////    }
            ////}


            //Console.WriteLine("MainClient Connected: " + e.Client.RemoteEndPoint);
            //IMClient c = (IMClient)Activator.CreateInstance(ClientClass.GetType());///////////////////////
            //c.tcpClient = e;
            //c.isMainConn = false;
            //c.LastChecked = DateTime.Now;
            //Clients.Add(c);


        }



        public void Server_DataReceived(object sender, Message e)
        {
            try
            {
                Console.WriteLine();
                int MessageLengthInt = readInt(e.Data);
                Console.WriteLine("DataLength " + MessageLengthInt);
                if(MessageLengthInt > 1048576)
                {
                    Console.WriteLine("Packet length > max");
                    return;
                }
                byte[] data = readMessage(e.TcpClient, MessageLengthInt);

                int DataType = readInt(data);
                Console.WriteLine("DataType: " + DataType);
                var myEnumMemberCount = Enum.GetNames(typeof(DataHandler.eDataType)).Length;
                if(DataType > myEnumMemberCount || DataType < 0)
                {
                    return;
                }

                byte[] finalData = new byte[MessageLengthInt - 4];

                Buffer.BlockCopy(data, 4, finalData, 0, finalData.Length);
                //Console.WriteLine("MessageLength Without Header: " + (finalData.Length));
                //Console.WriteLine("Message: " + Encoding.UTF8.GetString(finalData));

                //foreach (IMainClient c in MainClients.ToList())
                //{
                //if ((c.tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString().Equals((e.TcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString()))
                //{
                
                try
                {
                    
                    bool isBoundedBefore = false;
                    int BoundedInWhichDict = (int)DataHandler.eConnectionType.connection_type_Main;
                    IMClient BoundedInWhichClient = null;

                    IMClient c;
                    //if (MainClientsDict.TryGetValue(e.TcpClient, out c))
                    //{
                    //    Console.WriteLine("Succed");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Failed");
                    //}
                    if (MainClientsDict.ContainsKey(e.TcpClient))
                    {
                        Console.WriteLine(MainClientsDict.Count.ToString());
                        Console.WriteLine("Found in Main Dict");
                        BoundedInWhichDict = (int)DataHandler.eConnectionType.connection_type_Main;
                        isBoundedBefore = true;
                    }
                    foreach (IMClient ic in MainClientsDict.Values.ToList())
                    {

                        if (ic.FeatureClients.ContainsKey(DataType))
                        {
                            Console.WriteLine("Found in F Dict of " + ic.tcpClient.Client.RemoteEndPoint.ToString());
                            BoundedInWhichDict = (int)DataHandler.eConnectionType.connection_type_Feature;
                            isBoundedBefore = true;
                            BoundedInWhichClient = ic;
                            break;
                        }
                        
                    }

                    if (!isBoundedBefore)
                    {
                        Console.WriteLine("NotBoundedBefore");
                        c = (IMClient)Activator.CreateInstance(ClientClass.GetType());
                        c.tcpClient = e.TcpClient;
                    }
                    else
                    {
                        if (BoundedInWhichDict == (int)DataHandler.eConnectionType.connection_type_Main)
                        {
                            //Console.WriteLine("BoundedBefore");
                            c = MainClientsDict[e.TcpClient];
                        }
                        else //if (BoundedInWhichDict == (int)DataHandler.eConnectionType.connection_type_Feature)
                        {
                           
                            c = (IMClient)BoundedInWhichClient.FeatureClients[DataType];
                            c.tcpClient = e.TcpClient;

                        }
                    }

                    Console.WriteLine("Distributing to Client : " + c.tcpClient.Client.RemoteEndPoint.ToString());

                    DataHandler.distribute(DataType, finalData, c);
                    
                }
                catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                }

                
                
                //    }
                //}


                Console.WriteLine();

            }
            catch { }

        }

        private void Server_ClientDisconnected(object sender, TcpClient e)
        {
            Console.WriteLine("MainClient Disconnected: " + e.Client.RemoteEndPoint);
            MainClientsDict.Remove(e);
            //foreach (IMainClient c in MainClientsDict.Values.ToList())
            //{
            //    if(c.tcpClient == e)
            //    {
            //        MainClients.Remove(c);
                    
            //    }
            //}
            
        }

        public void send(String Message,TcpClient c)
        {
            c.Client.Send(Encoding.UTF8.GetBytes(Message+"\n"));
            
        }


        public void StopServer()
        {
            try
            {
                
                Stop();
                StopFlag = true;
                RefreshThread.Abort();
                instance = null;
            }
            catch { }
        }
    }
}
