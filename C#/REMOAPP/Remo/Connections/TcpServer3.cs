//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Reflection;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Remo.Connections
//{
//    public class TcpServer3 : IDisposable, TCP
//    {
//        private readonly TcpListener _listener;
//        private CancellationTokenSource _tokenSource;
//        private bool _listening;
//        private CancellationToken _token;
//        public event EventHandler<DataReceivedEventArgs> OnDataReceived;



//        int port = 4447;
//        public Dictionary<string, IMConnection> MainConnectionsDict { get; }
//        public Dictionary<string, IMConnection> FeatureConnectionsMapDict { get; }//string = IFConnection ip
//        public int Port { get; set; }
//        public int CheckIsConnectedInterval_ms { get; set; } = 5000;
//        public DateTime DateStarted { get; set; }






//        private TcpServer3(IPAddress address, int port)
//        {
//            _listener = new TcpListener(address, port);
//            OnDataReceived += TcpServer3_OnDataReceived;
//            MainConnectionsDict = new Dictionary<string, IMConnection>();
//            FeatureConnectionsMapDict = new Dictionary<string, IMConnection>();
//            Console.WriteLine("TCP Server Started!");

//        }

//        public bool Listening => _listening;

//        public async Task StartAsync(CancellationToken? token = null)
//        {
//            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token ?? new CancellationToken());
//            _token = _tokenSource.Token;
//            _listener.Start();
//            _listening = true;

//            DateStarted = DateTime.Now;


//            try
//            {
//                while (!_token.IsCancellationRequested)
//                {
//                    await Task.Run(async () =>
//                    {
//                        var tcpClientTask = _listener.AcceptTcpClientAsync();
//                        var result = await tcpClientTask;
//                        OnDataReceived?.Invoke(this, new DataReceivedEventArgs(result , result.GetStream()));
//                    }, _token);
//                }
//            }
//            finally
//            {
//                _listener.Stop();
//                _listening = false;
//            }
//        }

//        public void Stop()
//        {
//            _tokenSource?.Cancel();
//        }

//        public void Dispose()
//        {
//            Stop();
//        }
        

//        public class DataReceivedEventArgs : EventArgs
//        {
//            public NetworkStream Stream { get; private set; }
//            public TcpClient Client { get; set; }

//            public DataReceivedEventArgs(TcpClient client, NetworkStream stream)
//            {
//                Client = client;
//               // Stream = stream;
//            }
//        }







//        private void TcpServer3_OnDataReceived(object sender, DataReceivedEventArgs e)
//        {

//            TcpClient client = (TcpClient) e.Client;
//            Console.WriteLine("Client Connected: " + client.Client.RemoteEndPoint.ToString());


//          //  Boolean bClientConnected = true;

//            IMConnection c = null;
//          //  while (bClientConnected && _listening)
//          //  {

//                try
//                {
//                    byte[] bArray;
//                    int n, Length, DataType, Flag;
//                    //String Messsage = "";
//                    bArray = new byte[4];
//                    n = client.Client.Receive(bArray, 0, 4, SocketFlags.None);
//                    Length = readInt(bArray);
//                    //Console.WriteLine("DataLen = " + Length);

//                    n = client.Client.Receive(bArray, 0, 4, SocketFlags.None);
//                    DataType = readInt(bArray);
//                    //Console.WriteLine("DataType = " + DataType);

//                    n = client.Client.Receive(bArray, 0, 4, SocketFlags.None);
//                    Flag = readInt(bArray);

//                    byte[] data = readMessage(client, Length);
//                    //Console.WriteLine(client.Client.RemoteEndPoint.ToString());

//                    c = CheckClientExistance2(client, DataType);

//                    Console.WriteLine("Distributing to Client : " + c.tcpClient.Client.RemoteEndPoint.ToString());
//                    DataHandler.distribute(DataType, Flag, data, c);

//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("Recive Exception: " + ex.Message);
//            //        bClientConnected = false;
//                    //     clientDisconnected(c,client);
//                }

               

//          //  }

//        }









//        private static readonly object mutex = new object();
//        private static TcpServer3 instance = null;

//        public static TCP GetInstance()
//        {

//            if (instance == null)
//            {
//                lock (mutex)
//                {
//                    if (instance == null)
//                    {
//                        instance = new TcpServer3(IPAddress.Any,4447);
//                    }
//                }
//            }

//            return instance;

//        }










//        public IMConnection CheckClientExistance2(TcpClient e, int DataType)
//        {
//            IMConnection c;
//            try
//            {


//                if (FeatureConnectionsMapDict.ContainsKey(e.Client.RemoteEndPoint.ToString()))
//                {
//                    Console.WriteLine("Found in FMap Dict");
//                    c = FeatureConnectionsMapDict[e.Client.RemoteEndPoint.ToString()];
//                    c.tcpClient = e;
//                }


//                else if (
//                    null != getMainConnectionByIP((e.Client.RemoteEndPoint as IPEndPoint).Address.ToString())
//                    &&
//                    getMainConnectionByIP((e.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients.ContainsKey(DataType))
//                {
//                    Console.WriteLine("Found initialized Before");
//                    c = (IMConnection)getMainConnectionByIP((e.Client.RemoteEndPoint as IPEndPoint).Address.ToString()).FeatureClients[DataType];
//                    c.tcpClient = e;

//                }

//                else if (MainConnectionsDict.ContainsKey((e.Client.RemoteEndPoint as IPEndPoint).Address.ToString()))
//                {
//                    Console.WriteLine("Found in Main Dict");
//                    c = MainConnectionsDict[(e.Client.RemoteEndPoint as IPEndPoint).Address.ToString()];
//                }
//                else
//                {
//                    Console.WriteLine("NotBoundedBefore");
//                    c = new Connection();//(IMConnection)Activator.CreateInstance(ClientClass.GetType());
//                    c.tcpClient = e;
//                }

//                return c;

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Bound Client Exception: " + ex.Message);
//                throw new Exception("Bound Client Exception");
//            }

//            // Monitor.Exit(this);

//            //    }
//            //}

//            //Monitor.Exit(GetInstance());
//            // Console.WriteLine();

//        }












//        public void Server_ClientConnected(object sender, TcpClient e)
//        {
//            throw new NotImplementedException();
//        }

//        public void Server_DataReceived(object sender, object e)
//        {
//            throw new NotImplementedException();
//        }

//        public async void StartServer(int Port)
//        {
//            await StartAsync();
//        }

//        public void StopServer()
//        {
//            Stop();
//        }



//        public IMConnection getMainConnectionByIP(String ip)
//        {
//            IMConnection ret;
//            MainConnectionsDict.TryGetValue(ip, out ret);
//            return ret;

//        }

//        public IFeature startFeature(string MainClientIP, int Feature_type)
//        {
//            IFConnection fc = new Connection();//(IMConnection)Activator.CreateInstance(ClientClass.GetType());                                                                // {
//                                           //         fc.MainConnection = getMainConnectionByIP(MainClientIP);               ////
//            try
//            {
//                getMainConnectionByIP(MainClientIP).FeatureClients.Add(Feature_type, fc);
//            }
//            catch { }
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
//                    fc.F.MainConnection = getMainConnectionByIP(MainClientIP);
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
//        //public void send(string Message, object c)
//        //{
//        //    ((TcpClient)c).Client.Send(Encoding.UTF8.GetBytes(Message + "\n"));

//        //}







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
//            int retInt = -1;
//            byte[] MessageLength = new byte[4];
//            for (int i = 0; i < 4; i++)
//            {
//                MessageLength[i] = data[i];
//            }
//            retInt = SwapEndianness(BitConverter.ToInt32(MessageLength, 0));
//            return retInt;
//        }

//        public byte[] readMessage(TcpClient client, int length)
//        {
//            int offset = 0;
//            int size = length;
//            byte[] numArray = new byte[length];
//            while (offset < length && _listening)
//            {
//                int num2 = client.Client.Receive(numArray, offset, size, SocketFlags.None);
//                if (num2 != 0)
//                {
//                    offset += num2;
//                    size -= num2;
//                }
//                else
//                    break;
//            }
//            return numArray;
//        }


















      
//        public void ClientHandler(object obj)
//        {
//            throw new NotImplementedException();
//        }
        
//    }
//}
