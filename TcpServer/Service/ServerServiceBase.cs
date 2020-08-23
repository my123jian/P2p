using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpServer.Service
{
    public abstract class ServerServiceBase :ServiceBase,IDisposable
    {
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String Address { get; set; }
        /// <summary>
        /// 监听线程
        /// </summary>
        protected Thread ListenThread = null;
        /// <summary>
        /// 是否已停止
        /// </summary>
        public bool IsStop { get; protected set; }
        /// <summary>
        /// 当前的客户端列表
        /// </summary>
        protected List<Socket> Clients { get; set; }

        public ServerServiceBase()
        {
            this.Clients = new List<Socket>();
        }

        /// <summary>
        /// 初始化TCP
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        protected virtual void InitTcpServerSocker(string Address = "127.0.0.1", int Port = 10000)
        {
            IPAddress ipAddress = IPAddress.Parse(Address);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            this.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定ip和端口  
            this.ServerSocket.Bind(ipEndPoint);
            //设置最长的连接请求队列长度
            Console.WriteLine("绑定端口成功！");
            this.ServerSocket.Listen(10);
            this.ListenThread = new Thread(this.HandleListen);
            this.ListenThread.Start();

        }
        /// <summary>
        /// 初始化UDP
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        protected virtual void InitUdpServerSocker(string Address = "127.0.0.1", int Port = 10000)
        {
            IPAddress ipAddress = IPAddress.Parse(Address);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            this.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //绑定ip和端口  
            this.ServerSocket.Bind(ipEndPoint);
            //设置最长的连接请求队列长度
            Console.WriteLine("绑定端口成功！");
            //this.theServiceSocket.Listen(10);
            this.ListenThread = new Thread(this.HandleListen);
            this.ListenThread.Start();

        }

        /// <summary>
        /// 当前的监听的线程
        /// </summary>
        protected virtual void HandleListen()
        {
            while (IsStop == false)
            {
                Console.WriteLine("开始接收连接！");
                var theClientSocket = this.ServerSocket.Accept();
                Console.WriteLine("接收到一个客户端的连接！");
                this.HandleConect(theClientSocket);
            }
        }
        /// <summary>
        /// 处理套接字的连接
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void HandleConect(Socket theClientSocket)
        {
            IPEndPoint theRemoteEndPoint = (IPEndPoint)theClientSocket.RemoteEndPoint;

            theClientSocket.ReceiveTimeout = 1000 * 60;
            theClientSocket.SendTimeout = 1000 * 60;
            //var theMessage=theClientSocket.ReceiveMessage();
            //Console.WriteLine(theMessage);
            //theClientSocket.SendMessage("你已经连接上了！");
            var theIpAddress = theRemoteEndPoint.Address.ToString();
            var theIpPort = theRemoteEndPoint.Port;
            Console.WriteLine("客户端的IP地址" + theIpAddress);
            Console.WriteLine("客户端的端口地址" + theIpPort);

            lock (this)
            {
                this.Clients.Add(theClientSocket);
            }
            
        }
        protected void CheckValidSocket()
        {
            lock (this)
            {
                var theValidNum = 0;
                var theValidList = new List<Socket>();
                foreach (var item in this.Clients)
                {
                    if (item.Connected)
                    {
                        theValidList.Add(item);
                        theValidNum += 1;
                    }
                }

                this.Clients = theValidList;
                Console.WriteLine("当前有效连接数量 :" + theValidNum);
            }


        }
        public virtual void Dispose()
        {
            if (this.ListenThread != null) {
                this.ListenThread.Interrupt();
            }
            
            //throw new NotImplementedException();
        }
    }
}
