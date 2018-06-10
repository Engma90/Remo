using Remo.Connections;
//using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{
    public interface IServer
    {
        int Port { get; set; }

        //IServer GetInstance(); // Singlton
        void StartServer(int Port);
        void StopServer();
        void HandleClient(object obj);
        byte[] readMessage(TcpClient client, int length);

        //void send();
        //void Server_DataReceived();
    }
    public interface TCP : IServer
    {
        Dictionary<string, IMClient> MainClientsDict { get; }
        Dictionary<string, IMClient> FeatureClientsMapDict { get; }//string = IFClient ip
        int CheckIsConnectedInterval_ms { get; set; }
        IFeature addFClient(string MainClientIP, int Feature_type);


        DateTime DateStarted { get; set; }




        IMClient getClientByIP(String ip);
       // IMClient ClientClass { get; set; }



        void send(string Message, string OrderType, string Parameters, object c);
        void send(string Message, string OrderType, object c);
        //void send(string Message, object c);



        //mTCPHandler GetInstance();
        //   void Server_ClientConnected(object sender, TcpClient e);
        //  void Server_DataReceived(object sender, Message e);
        //void send(String Message, TcpClient c);
    }

    public interface UDP : IServer
    {
        //mUDPHandler GetInstance();
        void Server_DataReceived(object sender, object e);
    }
}
