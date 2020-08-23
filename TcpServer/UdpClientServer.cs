using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpServer
{
    public class UdpClientServer
    {
        private Socket theServiceSocket = null;
        private string theServiceAddress = null;
        private int theServerPort = 10000;
        private Thread theClientThread = null;
        private Thread theServiceThread = null;

        public UdpClientServer(string Address = "", int Port = 10000)
        {
            this.theServiceAddress = Address;
            this.theServerPort = Port;
        }

        public void Start()
        {
            // Get host related information.
            var ipHostInfo = Dns.GetHostEntry("localhost");

            // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
            // an exception that occurs when the host IP Address is not compatible with the address family
            // (typical in the IPv6 case).
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipe = new IPEndPoint(ipAddress, this.theServerPort);

            this.theClientThread = new Thread(this.InitSocket);
            //this.theServiceThread = new Thread(this.InitService);
            this.theClientThread.Start();
            //this.theServiceThread.Start();
        }

        private void KeepWatch()
        {

        }
        public void Pause()
        {

        }
        public void Stop()
        {

        }

      
        private void InitSocket()
        {

            var ipHostInfo = Dns.GetHostAddresses(this.theServiceAddress);

            IPAddress ipAddress = ipHostInfo[0];
            IPEndPoint ipe = new IPEndPoint(ipAddress, this.theServerPort);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 10001);

            this.theServiceSocket = new Socket(ipe.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            //this.theServiceSocket.DualMode = true;
            this.theServiceSocket.Bind(ipEndPoint);
            for (var i = 0; i < 1; i++)
            {
                try
                {


                    while (true)
                    {
                        var theMessage = "Hello wrld";
                        var theSendMessage = "";
                        for (var a = 0; a < 1; a++) { theSendMessage += theMessage; }

                        this.theServiceSocket.SendTo(Encoding.UTF8.GetBytes( theSendMessage),ipe as EndPoint);
                        //var theSMessage = this.theServiceSocket.ReceiveMessage();

                        Console.WriteLine(theSendMessage);
                        Thread.Sleep(10000);
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }


        }

        private void InitService()
        {
            var thePoint = new IPEndPoint(IPAddress.Any, int.Parse("10001"));// this.theServiceSocket.LocalEndPoint;
            var theNewSocket = new Socket(thePoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            theNewSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            theNewSocket.Bind(thePoint);
            theNewSocket.Listen(10);
            while (true)
            {
                var theClientSocket = theNewSocket.Accept();
                IPEndPoint theRemoteEndPoint = (IPEndPoint)theClientSocket.RemoteEndPoint;
                Console.Write(theRemoteEndPoint.Port);

            }
        }
    }
}
