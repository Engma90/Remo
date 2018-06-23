using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SimpleTCP;
using System.Net.Sockets;
using Remo.Connections;
using System.Net;
using System.Threading;
using System.Reflection;

namespace Remo.Connections
{

    public class ServerFactory 
    {
        //private static volatile TCP instance = null;
        //private static object syncRoot = new Object();
        public static readonly string TYPE_TCP = "TCP";
        public static readonly string TYPE_UDP = "UDP";

        private ServerFactory()
        {
            Console.WriteLine("TCP Server Started!");
        }
        public static TCP GetInstance(String Type)
        {

            if (Type == TYPE_TCP)
            {
                return TCPServer2.GetInstance();
            }
            else
                return null;
            //else if (Type == TYPE_TCP)
            //{

            //}
                //if (instance == null)
                //{
                //    lock (syncRoot)
                //    {
                //        if (instance == null)
                //            instance = TCPServer2.GetInstance();
                //    }
                //}

                //return instance;

        }
        
    }
}
