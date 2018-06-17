using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using SimpleTCP;
using System.Reflection;

namespace Remo.Connections
{
    public class TCPServer2 : TCP
    {
        private TcpListener _server;
        private static volatile Boolean _isRunning;
        int port = 4447;
    //    public Dictionary<string, IConnection> MainConnectionsDict { get; }
    //    public Dictionary<string, IConnection> FeatureConnectionsMapDict { get; }//string = IFConnection ip
        public int Port { get; set; }
    //    public int CheckIsConnectedInterval_ms { get; set; } = 5000;
        //    public IMConnection ClientClass { get; set; }
        public DateTime DateStarted { get; set; }
        private TCPServer2()
        {
            DateStarted = DateTime.Now;
      //      MainConnectionsDict = new Dictionary<string, IConnection>();
       //     FeatureConnectionsMapDict = new Dictionary<string, IConnection>();
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
                try
                {
                    Console.WriteLine("Waiting for Clients");
                    TcpClient newClient = _server.AcceptTcpClient();
                   // TcpClient newClient = newClient0.Clone

                    // client found.
                    // create a thread to handle communication
                    Thread t = new Thread(new ParameterizedThreadStart(ClientHandler));
                    t.Start(newClient);
                }
                catch
                {
                    Console.WriteLine("TCP Server Stopped");
                    _isRunning = false;
                    break;
                }
            }
        }

        public override void ClientHandler(object obj)
        {
            TcpClient client = (TcpClient)obj;
            Console.WriteLine("Client Connected: " + client.Client.RemoteEndPoint.ToString());

            Boolean bClientConnected = true;
          //  client.Client.ReceiveTimeout = 5000;
            //client.ReceiveTimeout = 5000;
            while (bClientConnected && _isRunning)
            {

                try
                {
                    byte[] bArray;
                    int n, Length, DataType,Flag;
                    //String Messsage = "";
                    bArray = new byte[4];
                    
                    n = client.Client.Receive(bArray, 0, 4, SocketFlags.None);
                    Length = readInt(bArray);
                    /// Console.WriteLine("DataLen = " + Length);

                    n = client.Client.Receive(bArray, 0, 4, SocketFlags.None);
                    DataType = readInt(bArray);
                    Console.WriteLine("DataType = " + DataType);

                    n = client.Client.Receive(bArray, 0, 4, SocketFlags.None);
                    Flag = readInt(bArray);

                    byte[] data = readMessage(client, Length);
                    // Console.WriteLine(client.Client.RemoteEndPoint.ToString());

                 //  c = CheckClientExistance2(client, DataType);

                    Console.WriteLine("Distributing to Client : " + client.Client.RemoteEndPoint.ToString());
                    DataHandler.distribute(DataType,Flag, data, client);

                }
                catch(Exception ex)
                {
                    Console.WriteLine("Recive Exception: " + ex.Message);
                    bClientConnected = false;
                    break;
                    //clientDisconnected(client);
                }







            }
        }



        //public void clientDisconnected(TcpClient c)
        //{

        //    try
        //    {

        //        if (MainConnectionsDict.ContainsKey((c.Client.RemoteEndPoint as IPEndPoint).Address.ToString()))
        //        {
        //            foreach (IFeature fc in getMainConnectionByIP(c.Client.RemoteEndPoint.ToString()).Features.Values.ToList())
        //            {
                        
        //                getMainConnectionByIP(c.Client.RemoteEndPoint.ToString()).Features.Remove(fc.DATA_TYPE);
        //                fc.Dispose();
        //            }
        //            MainConnectionsDict.Remove((c.Client.RemoteEndPoint as IPEndPoint).Address.ToString());
        //            c.Client.Disconnect(false);
        //        }
        //    }
        //    catch { Console.WriteLine("Disconnect Exception"); }
        //}
        
        public void Server_ClientConnected(object sender, TcpClient e)
        {
            throw new NotImplementedException();
        }

        public void Server_DataReceived(object sender, object e)
        {
            throw new NotImplementedException();
        }
        
        public override void StartServer(int Port)
        {
            throw new NotImplementedException();
        }

        public override void StopServer()
        {
            _isRunning = false;
            _server.Server.Blocking = false;
            _server.Server.Shutdown(SocketShutdown.Both);
            _server.Stop();
            _server.Server.Close();
            _server.Server.Dispose();

        }



        public override IConnection getMainConnectionByIP(String ip)
        {
            ip = ip.Split(':')[0];
            IConnection ret;
            MainConnectionsDict.TryGetValue(ip, out ret);
            return ret;
        }

        public override IFeature startFeature(string MainClientIP, int Feature_type)
        {
            if (getMainConnectionByIP(MainClientIP).Features.ContainsKey(Feature_type) && !getMainConnectionByIP(MainClientIP).Features[Feature_type].IsDisposed)
            {
                return getMainConnectionByIP(MainClientIP).Features[Feature_type];
            }
            var types = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(t => (t.GetInterface("IFeature")) != null);

            foreach (var t in types)
            {
                IFeature TempFeature = (IFeature)Activator.CreateInstance(t);

                if (TempFeature.DATA_TYPE == Feature_type)
                {

                    try
                    {
                        getMainConnectionByIP(MainClientIP).Features.Remove(Feature_type);
                    }
                    catch
                    {

                    }


                    try
                    {
                        getMainConnectionByIP(MainClientIP).Features.Add(Feature_type, TempFeature);
                    }
                    catch { }
                    getMainConnectionByIP(MainClientIP).Features[Feature_type] = TempFeature;
                    getMainConnectionByIP(MainClientIP).Features[Feature_type].MainConnection = getMainConnectionByIP(MainClientIP);
                    Console.WriteLine(t.Name);
                    return TempFeature;
                }
            }


            return null;
        }



        public override void send(string Message, string OrderType, string Parameters, object c)
        {
            ((TcpClient)c).Client.Send(Encoding.UTF8.GetBytes(Message + ":" + OrderType + ":" + Parameters + "\n"));

        }
        public override void send(string Message, string OrderType, object c)
        {
            ((TcpClient)c).Client.Send(Encoding.UTF8.GetBytes(Message + ":" + OrderType+":.." + "\n"));

        }







      

        public override byte[] readMessage(TcpClient client, int length)
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
