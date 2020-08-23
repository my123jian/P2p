using System;
using System.Net;
using System.Net.Sockets;
using TcpServer.Client;

namespace TcpServer
{
    public class SocketTest
    {
        public SocketTest()
        {
        }
        public void TestConnectTimeOut(string Address)
        {
            var ipHostInfo = Dns.GetHostAddresses(Address);
            var thePoint = new IPEndPoint(ipHostInfo[0], int.Parse("80"));// this.theServiceSocket.LocalEndPoint;
            var theNewSocket = new ClientTcpSocket();
            theNewSocket.SendTimeout = 1000;
            theNewSocket.ReceiveTimeout = 1000;
            theNewSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, true);
            theNewSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, true);
            theNewSocket.Connect(thePoint);
           var theServerSocket= theNewSocket.GetServerSocket();
            var theServer = new ServerTcpSocket(theServerSocket);
        }
    }
}
