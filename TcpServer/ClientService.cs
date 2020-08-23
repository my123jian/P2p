using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServer
{
   
    public class ClientService
    {
        private Socket theServiceSocket = null;
        private string theServiceAddress = null;
        private int theServerPort = 10000;
        private Thread theClientThread = null;
        private Thread theServiceThread = null;

        public ClientService(string Address="",int Port=10000)
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

        private void TestServerSocket()
        {
            var ipHostInfo = Dns.GetHostAddresses(this.theServiceAddress);
          
            IPAddress ipAddress = ipHostInfo[0];
            IPEndPoint ipe = new IPEndPoint(ipAddress, this.theServerPort);
            var theBeginTime = DateTime.Now;
            //var theSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //theSocket.Connect(ipe);
            //theSocket.ReceiveTimeout = 100000;
            //theSocket.SendTimeout = 100000;
            //var theMessage = theSocket.ReceiveMessage();
            //Console.WriteLine(theMessage);

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; i < 100; j++)
                {
                    var theSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    theSocket.Connect(ipe);
                    theSocket.ReceiveTimeout = 100000;
                    theSocket.SendTimeout = 100000;
                    var theMessage = "Hello wrld";
                    var theSendMessage = "";
                    for(var a = 0; a < 1000; a++) { theSendMessage += theMessage; }
                    theSocket.SendMessage(theSendMessage);
                    var theMessageT = theSocket.ReceiveMessage();
                    Console.WriteLine(theMessageT);
                }

            }
            var theEndTime = DateTime.Now;
            var theTotalTime = theEndTime - theBeginTime;
            Console.WriteLine(string.Format("总共耗时:{0}",theTotalTime));
        }
        private void InitSocket()
        {

            var ipHostInfo = Dns.GetHostAddresses(this.theServiceAddress);

            IPAddress ipAddress = ipHostInfo[0];
            IPEndPoint ipe = new IPEndPoint(ipAddress, this.theServerPort);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 10001);

            this.theServiceSocket= new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //this.theServiceSocket.DualMode = true;
            this.theServiceSocket.Bind(ipEndPoint);
            for (var i = 0; i < 1; i++)
            {
                try
                {
                    this.theServiceSocket.Connect(ipe);
                    new Thread(this.InitService).Start();
                   

                    while (true)
                    {
                        var theMessage = "Hello wrld";
                        var theSendMessage = "";
                        for (var a = 0; a < 1; a++) { theSendMessage += theMessage; }
                        this.theServiceSocket.SendMessage(theSendMessage);
                        var theSMessage = this.theServiceSocket.ReceiveMessage();

                        Console.WriteLine(theSMessage);
                        Thread.Sleep(10000);

                        var thePoint = new IPEndPoint(IPAddress.Any, int.Parse("10001"));// this.theServiceSocket.LocalEndPoint;
                        var theNewSocket = new Socket(thePoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        theNewSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        theNewSocket.Bind(thePoint);
                        var ipHostInfo1 = Dns.GetHostAddresses("61.144.117.251");

                        IPAddress ipAddress1 = ipHostInfo1[0];
                        IPEndPoint ip1e = new IPEndPoint(ipAddress1, this.theServerPort);
                        theNewSocket.Connect(ip1e);
                        theNewSocket.SendMessage("hello world!");

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
           var thePoint  = new IPEndPoint(IPAddress.Any, int.Parse("10001"));// this.theServiceSocket.LocalEndPoint;
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

        private void ConnectNat(Socket theSocket)
        {
            theSocket.Connect("", 80);
        }
    }
}
