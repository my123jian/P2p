using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpServer
{
   public  class ServiceMain:IDisposable
    {
        private Socket theServiceSocket = null;
        private Thread theThread = null;
        private bool IsStop = false;
        public ServiceMain()
        {
            this.InitServerSocker();
        }
        private void InitServerSocker(string Address= "127.0.0.1", int Port=10000)
        {
            IPAddress ipAddress = IPAddress.Parse(Address);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Port);
            this.theServiceSocket=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定ip和端口  
            this.theServiceSocket.Bind(ipEndPoint);
            //设置最长的连接请求队列长度  
            this.theServiceSocket.Listen(10);
            Thread thread = new Thread(this.Listen);
            thread.Start();

        }
        private void Listen()
        {
            while (IsStop == false)
            {
              var theClientSocket=  this.theServiceSocket.Accept();
                IPEndPoint theRemoteEndPoint= (IPEndPoint)theClientSocket.RemoteEndPoint;
               Console.Write( theRemoteEndPoint.Port);

            }
        }
      

        public void Dispose()
        {
            if (this.theThread != null)
            {
                this.theThread.Interrupt();
            }
            if (this.theServiceSocket != null)
            {
                this.theServiceSocket.Close();
            }
        }
    }
}
