using Remo.Connections;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Remo.Connections
{
    interface Connection
    {
        void StartServer(int Port);
        void StopServer();
        //void send();
        //void Server_DataReceived();
    }
    interface TCP : Connection
    {
        //mTCPHandler GetInstance();
        void Server_ClientConnected(object sender, TcpClient e);
        void Server_DataReceived(object sender, Message e);
        void send(String Message, TcpClient c);
    }

    interface UDP : Connection
    {
        //mUDPHandler GetInstance();
        void Server_DataReceived(object sender, Message e);
    }
}
