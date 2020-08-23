using System;
using System.Net;
using System.Net.Sockets;

namespace TcpServer.Client
{
    public class ClientTcpSocket:Socket
    {
        public ClientTcpSocket():base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
        }

        public Socket GetServerSocket()
        {
           var theServerSocket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = (IPEndPoint)this.LocalEndPoint;
            ipEndPoint = new IPEndPoint(IPAddress.Any, ipEndPoint.Port);
            Console.WriteLine("当前的端口是"+ ipEndPoint.Port);
            theServerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            theServerSocket.Bind(ipEndPoint);
            return theServerSocket;
        }
    }
}
