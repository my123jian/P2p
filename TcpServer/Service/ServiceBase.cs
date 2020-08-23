using System;
using System.Net.Sockets;

namespace TcpServer.Service
{
    public abstract class ServiceBase
    {
        public Socket ServerSocket { get; set; }

        public ServiceBase()
        {
        }
    }
}
