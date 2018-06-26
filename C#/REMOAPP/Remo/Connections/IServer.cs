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
    public abstract class IServer
    {
        int Port { get; set; }

        //IServer GetInstance(); // Singlton
        public abstract void StartServer(int Port);
        public abstract void StopServer();
        public abstract void ClientHandler(object obj);
        public abstract byte[] readMessage(TcpClient client, int length);




        protected int SwapEndianness(int value)
        {
            int num1 = value & (int)byte.MaxValue;
            int num2 = value >> 8 & (int)byte.MaxValue;
            int num3 = value >> 16 & (int)byte.MaxValue;
            int num4 = value >> 24 & (int)byte.MaxValue;
            int num5 = 24;
            return num1 << num5 | num2 << 16 | num3 << 8 | num4;
        }
        protected int readInt(byte[] data)
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
    }
    public abstract class TCP : IServer
    {
        public Dictionary<string, IConnection> MainConnectionsDict { get; } = new Dictionary<string, IConnection>();
        public int CheckIsConnectedInterval_ms { get; set; } = 5000;
        public abstract IFeature startFeature(string MainClientIP, int Feature_type);
        public DateTime DateStarted { get; set; }
        public abstract IConnection getMainConnectionByIP(String ip);

        public abstract void send(string Message, string OrderType, string Parameters, object c);
        public abstract void send(string Message, string OrderType, object c);

    }

    public abstract class UDP : IServer
    {
        public abstract void Server_DataReceived(object sender, object e);
    }
}
