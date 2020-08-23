using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TcpServer.Model;

namespace TcpServer.Service
{
    public class UdpServerService:ServerServiceBase
    {
        /// <summary>
        /// 最大支持1000人同时使用
        /// </summary>
        private int theMaxSocket = 1000;
        /// <summary>
        /// 当前的服务器的端口
        /// </summary>
        private int theServerPort = 10000;
        /// <summary>
        /// 监控线程 用于监控防止死掉
        /// </summary>
        private Thread keepWatchThread = null;
        /// <summary>
        /// 服务端口
        /// </summary>
        private Socket theServiceSocket = null;
        /// <summary>
        /// 监听线程
        /// </summary>
        private Thread theListenThread = null;
        /// <summary>
        /// 当前的监听的客户端线程
        /// </summary>
        private List<Thread> theClientThreads = null;
        /// <summary>
        /// 服务是否停止
        /// </summary>
        private bool IsStop = false;
        /// <summary>
        /// 当前的客户端的端口列表
        /// </summary>
        private List<Socket> theClientSockets = new List<Socket>();
        /// <summary>
        /// 当前的socket 处理列表
        /// </summary>
        private Queue<Socket> theTodoSocket = new Queue<Socket>();
        /// <summary>
        /// 发送的线程大小
        /// </summary>
        private int SendThreadCount = 0;
        /// <summary>
        /// 接收的线程大小
        /// </summary>
        private int ReceiveThreadCount = 0;

        public UdpServerService(SocketConfig config)
        {

        }
        /// <summary>
        /// 配置服务初始化
        /// </summary>
        public UdpServerService()
        {
            this.InitServerSocker();
            this.initKeepWatchThread();
        }

        #region 监控线程
        /// <summary>
        /// 初始化监控的线程
        /// </summary>
        private void initKeepWatchThread()
        {
            this.keepWatchThread = new Thread(this.runKeepWatchThread);
            this.keepWatchThread.Start();
        }

        /// <summary>
        /// 监控线程处理逻辑
        /// </summary>
        private void runKeepWatchThread()
        {
            while (IsStop == false)
            {
                //10秒钟检查一次连接是否成功
                this.CheckValidSocket();
                Console.WriteLine(string.Format("发送线程总数:{0},接收的线程总数:{1}", this.SendThreadCount, this.ReceiveThreadCount));
                //一秒钟检查一次
                System.Threading.Thread.Sleep(1000 * 5);


            }
        }
        /// <summary>
        /// 清理不必要的socket
        /// </summary>
        private void CheckValidSocket()
        {
            lock (this)
            {
                var theValidNum = 0;
                var theValidList = new List<Socket>();
                foreach (var item in this.theClientSockets)
                {
                    if (item.Connected)
                    {
                        theValidList.Add(item);
                        theValidNum += 1;
                    }
                }

                this.theClientSockets = theValidList;
                Console.WriteLine("当前有效连接数量 :" + theValidNum);
            }


        }
        #endregion
        /// <summary>
        ///  初始化服务端的端口
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        private void InitServerSocker(string Address = "127.0.0.1", int Port = 10000)
        {
            IPAddress ipAddress = IPAddress.Parse(Address);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            this.theServiceSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //绑定ip和端口  
            this.theServiceSocket.Bind(ipEndPoint);
            //设置最长的连接请求队列长度
            Console.WriteLine("绑定端口成功！");
            //this.theServiceSocket.Listen(10);
            Thread thread = new Thread(this.ListenThread);
            thread.Start();
            //this.ListenThread();

        }
        /// <summary>
        /// 当前的监听的线程
        /// </summary>
        private void ListenThread()
        {
            while (IsStop == false)
            {
                Console.WriteLine("开始接收连接！");
                //var theClientSocket = this.theServiceSocket.ReceiveFrom(;
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);
               byte[] buffer = new byte[1024];
                int length = theServiceSocket.ReceiveFrom(buffer, ref point);//接收数据报
                Console.WriteLine("接收到一个客户端的连接！");
                Console.WriteLine(string.Format("{0},{1}",((IPEndPoint)point).Address, ((IPEndPoint)point).Port));
                //this.InitClientSocket(theClientSocket);
            }
        }

        /// <summary>
        /// 初始化客户端的连接 获取对应的信息并给指定的线程进行处理 
        /// </summary>
        /// <param name="theClientSocket"></param>
        private void InitClientSocket(Socket theClientSocket)
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
                this.theClientSockets.Add(theClientSocket);
                //this.theTodoSocket.Enqueue(theClientSocket);

            }
            //开启一个线程进行处理
            this.InitClientSocketThread(theClientSocket);

        }
        private void InitClientSocketThread(Socket theClientSocket)
        {
            //Task.Factory.StartNew(() => {
            //    this.SendThreadCount += 1;
            //    RunSendClientThread(theClientSocket);
            //}).ContinueWith(m => {
            //    this.SendThreadCount -= 1;
            //});
            //开启一个线程进行处理
            Task.Factory.StartNew(() => {
                this.ReceiveThreadCount += 1;
                RunReceiveClientThread(theClientSocket);
            }).ContinueWith(m => {
                this.ReceiveThreadCount -= 1;
            });
        }
        /// <summary>
        /// 客户端发送消息处理
        /// </summary>
        /// <param name="theSocket"></param>
        private void RunSendClientThread(Socket theSocket)
        {
            var theClientSocket = theSocket;
            if (theClientSocket != null)
            {
                IPEndPoint theRemoteEndPoint = (IPEndPoint)theClientSocket.RemoteEndPoint;
                theClientSocket.SendMessage(theRemoteEndPoint.Port + "");
                //theClientSocket.SendCommand(ResponseCommand.New().SetIpAddress(theRemoteEndPoint.Address.ToString()).SetPort(theRemoteEndPoint.Port));
                Console.WriteLine("发送消息完成!");
            }

        }

        /// <summary>
        /// 客户端接收消息处理
        /// </summary>
        /// <param name="theSocket"></param>
        private void RunReceiveClientThread(Socket theSocket)
        {
            var theClientSocket = theSocket;
            if (theClientSocket != null)
            {
                var hasReceive = false;
                while (true)
                {
                    IPEndPoint theRemoteEndPoint = (IPEndPoint)theClientSocket.RemoteEndPoint;
                    var theMessage = theClientSocket.ReceiveMessage();
                    Console.WriteLine("接收到消息: " + theMessage);
                    Console.WriteLine("send消息: " + theMessage);
                    theClientSocket.SendMessage(theRemoteEndPoint.Address + ":" + theRemoteEndPoint.Port + "");
                    Console.WriteLine("send消息: " + theMessage);
                    // theClientSocket.ReceiveAsync(theRemoteEndPoint.Port + "");
                    if (hasReceive == false)
                    {
                        hasReceive = true;
                        this.TestClientConnect(theRemoteEndPoint);
                    }
                    Thread.Sleep(1000);
                }

            }

        }

        private void TestClientConnect(IPEndPoint Address)
        {
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("开始连接客户端");
                var theNewSocket = new Socket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, this.theServerPort);
                theNewSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                theNewSocket.Bind(ipEndPoint);
                Console.WriteLine("开始连接客户端bind sucess");
                theNewSocket.Connect(Address);
                Console.WriteLine("开始连接客户端 connect success");
                theNewSocket.SendMessage("服务器发送的消息！");
                Console.WriteLine("向客户端发送消息!");
                theNewSocket.Close();
            });

        }
        /// <summary>
        ///  释放实例
        /// </summary>
        public void Dispose()
        {
            if (this.theListenThread != null)
            {
                this.theListenThread.Interrupt();
            }
            if (this.keepWatchThread != null)
            {
                this.keepWatchThread.Interrupt();
            }
            if (this.theServiceSocket != null)
            {
                this.theServiceSocket.Close();
            }
        }
    }
}
