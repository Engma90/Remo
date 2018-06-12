using Remo.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Remo.Connections
{
    public class UDPReceiver
    {
        private int Port;
        private Thread ListenerThread;
        private bool Stop = false;
        private bool messageReceived = false;
        private int count = 0;
        private UdpState s;
        private int ReceiveTimeout = 900; // (*10/1000) ==>> 15 sec
        //UDPConnectionEventArgs
        private UDPDataEventArgs udpDataEventArgs;
        public event EventHandler<UDPDataEventArgs> UDPDataReceived;
        private int ackcount = 0;
        private int ackAfter = 10;
        private static volatile UDPReceiver instance = null;
        private static object syncRoot = new Object();

        private UDPReceiver(int por)
        {
            this.Port = MainForm.Port;
            s = new UdpState();
            s.e = new IPEndPoint(IPAddress.Any, this.Port);
            s.u = new UdpClient(s.e);
            
            udpDataEventArgs = new UDPDataEventArgs();
            count = 0;
            //s.u.Client.ReceiveTimeout = 5000;
            //s.u.Client.SendTimeout = 5000;
            StartServer(this.Port);
            Console.WriteLine("UDP Server Started!");

        }

        public static UDPReceiver GetInstance(int Port)
        {

            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new UDPReceiver(Port);
                }
            }

            return instance;

        }

        public void StartServer(int port)
        {

        }


        public void ReceiveCallback(IAsyncResult ar)
        {
            
            try
            {
                if (!Stop)
                {



                    // mTCPHandler.GetInstance(port).MainClients[0].udpCamClient = (UdpState)(ar.AsyncState);
                    UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
                    IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;
                    foreach (IMConnection c in mTCPHandler.GetInstance().MainConnectionsDict.Values.ToList())
                    {
                        //if (c.tcpClient.Client.RemoteEndPoint as IPEndPoint == ((IPEndPoint)((UdpState)(ar.AsyncState)).e))
                        //{
                        Console.WriteLine();
                        Console.WriteLine();
                        try
                        {
                            Console.WriteLine(c.tcpClient.Client.RemoteEndPoint as IPEndPoint);
                            Console.WriteLine(((UdpState)(ar.AsyncState)).u.Client.RemoteEndPoint);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        //c.udpCamClient = (UdpState)(ar.AsyncState);
                        //}
                    }

                    Byte[] receiveBytes = u.EndReceive(ar, ref e);
                    string receiveString = Encoding.ASCII.GetString(receiveBytes);
                    udpDataEventArgs.data = receiveBytes;



                    




                    OnUDPDataReceived(udpDataEventArgs);
                    ackcount++;
                    if (ackcount == ackAfter)
                    {
                        String OK = "OK";
                        ackcount = 0;
                        s.u.Send(Encoding.ASCII.GetBytes(OK), Encoding.ASCII.GetBytes(OK).Length,e);
                        Console.WriteLine("Sending ACK");
                    }
                    
                }
                
            }
            catch { }

            messageReceived = true;
        }

        public void ReceiveMessages()
        {

            ListenerThread = new Thread(delegate ()
            {
                while (!Stop)
                {
                    //Console.WriteLine("listening for messages");
                    count = 0;
                    messageReceived = false;
                    s.u.BeginReceive(new AsyncCallback(ReceiveCallback), s);
                    Console.WriteLine("Receiving...");
                    while (!messageReceived && !Stop)
                    {
                        Thread.Sleep(10);
                        //count++;
                        //if (count > ReceiveTimeout)
                        //{
                        //    Console.WriteLine("Receive TimeOut");
                        //    StopReceive();
                        //    break;
                        //}
                    }

                }
            });

            ListenerThread.Start();


           
        }

        public void StopReceive()
        {
            Stop = true;
            
            try
            {
                s.u.Client.Close();
                s.u.Close();
                s = null;
                instance = null;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            try
            {
                if (ListenerThread.IsAlive)
                    ListenerThread.Abort();
                ListenerThread = null;
            }
            catch { }
            finally
            {
                Console.WriteLine("Stoped");
            }


        }
        public class UdpState
        {

            public UdpClient u;
            public IPEndPoint e;
        }


        protected virtual void OnUDPDataReceived(UDPDataEventArgs e)
        {
            UDPDataReceived?.Invoke(this, e);
        }

        public class UDPDataEventArgs : EventArgs
        {
            public byte[] data { get; set; }
        }



        

    }
}
