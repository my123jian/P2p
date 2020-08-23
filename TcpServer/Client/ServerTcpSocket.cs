using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpServer.Client
{
    public class ServerTcpSocket
    {
        public Socket ServerSocket { get; set; }
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

        public ServerTcpSocket(Socket socket)
        {
            this.ServerSocket = socket;
            this.Clients = new List<Socket>();
            this.InitListen();
        }


        /// <summary>
        /// 初始化监听
        /// </summary>
        protected virtual void InitListen()
        {
           
            this.ServerSocket.Listen(10);
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
    }
}
