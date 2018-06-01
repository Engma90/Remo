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
        public List<TcpClient> HandledClientsList { get; }
        public List<IClient> Clients { get; }
        public List<IMainClient> MainClients { get; }
        public int CheckIsConnectedInterval_ms { get; set; } = 5000;
        int Port;
        private Thread RefreshThread;
        private static volatile mTCPHandler instance = null;
        private static object syncRoot = new Object();
        bool StopFlag = false;
        public IClient ClientClass { get; set; }
        //DataHandler dh;

        private mTCPHandler()
        {
            this.Port = MainForm.Port;
            ClientConnected += Server_ClientConnected;
            ClientDisconnected += Server_ClientDisconnected;
            DataReceived += Server_DataReceived;
            //AutoTrimStrings = true;
            //dh = new DataHandler();
            MainClients = new List<IMainClient>();
            Clients = new List<IClient>();
            HandledClientsList = new List<TcpClient>();
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
                        foreach (IMainClient c in MainClients.ToList())
                        {
                            //if (c.isMainConn)
                            //{
                                send(((int)DataHandler.eDataType.DATA_TYPE_INFO).ToString(), c.tcpClient);
                                Thread.Sleep(CheckIsConnectedInterval_ms);
                                if ((DateTime.Now - c.LastChecked) > TimeSpan.FromMilliseconds(CheckIsConnectedInterval_ms))
                                {
                                    //MainClients.Remove(c);
                                    Console.WriteLine(c.LastChecked);
                                    Console.WriteLine(DateTime.Now);
                                    c.tcpClient.Client.Disconnect(false);
                                }
                           // }
                        }
                    }
                    catch { Console.WriteLine("Broadcast Ex"); }

                }
            });

            RefreshThread.Start();
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
            IClient c = (IClient)Activator.CreateInstance(ClientClass.GetType());
            c.tcpClient = e;
            //c.isMainConn = false;
            //c.LastChecked = DateTime.Now;
            Clients.Add(c);


        }



        public void Server_DataReceived(object sender, Message e)
        {
            try
            {
                Console.WriteLine();
                int MessageLengthInt = readInt(e.Data);
                //Console.WriteLine("MessageLength With Header: " + MessageLengthInt);
                byte[] data = readMessage(e.TcpClient, MessageLengthInt);

                int DataType = readInt(data);
                Console.WriteLine("DataType: " + DataType);

                byte[] finalData = new byte[MessageLengthInt - 4];

                Buffer.BlockCopy(data, 4, finalData, 0, finalData.Length);
                //Console.WriteLine("MessageLength Without Header: " + (finalData.Length));
                //Console.WriteLine("Message: " + Encoding.UTF8.GetString(finalData));

                foreach (IMainClient c in MainClients.ToList())
                {
                    if ((c.tcpClient.Client.RemoteEndPoint as IPEndPoint).ToString().Equals((e.TcpClient.Client.RemoteEndPoint as IPEndPoint).ToString()))
                    {
                        Console.WriteLine("Distributing");
                        DataHandler.distribute(DataType, finalData, c);
                    }
                }


                Console.WriteLine();

            }
            catch { }

        }

        private void Server_ClientDisconnected(object sender, TcpClient e)
        {
            Console.WriteLine("MainClient Disconnected: " + e.Client.RemoteEndPoint);
            foreach (IMainClient c in MainClients.ToList())
            {
                if(c.tcpClient == e)
                {
                    MainClients.Remove(c);
                    
                }
            }
            
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
