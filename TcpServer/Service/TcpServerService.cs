using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TcpServer.Model;

namespace TcpServer.Service
{
    /// <summary>
    /// 1:启动服务接收客户端的连接
    /// 2:记录客户端的连接信息
    /// </summary>
    public class TcpServerService : ServerServiceBase
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
        /// 监听线程
        /// </summary>
        private Thread theListenThread = null;
        /// <summary>
        /// 当前的监听的客户端线程
        /// </summary>
        private List<Thread> theClientThreads = null;

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

        public TcpServerService(SocketConfig config)
        {

        }
        /// <summary>
        /// 配置服务初始化
        /// </summary>
        public TcpServerService()
        {
            this.InitTcpServerSocker();
            this.initKeepWatchThread();
        }

      
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
                try
                {
                    //10秒钟检查一次连接是否成功
                    this.CheckValidSocket();
                    Console.WriteLine(string.Format("发送线程总数:{0},接收的线程总数:{1}", this.SendThreadCount, this.ReceiveThreadCount));
                    //5秒钟检查一次
                    System.Threading.Thread.Sleep(1000 * 5);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// 处理服务端的连接
        /// </summary>
        /// <param name="theClientSocket"></param>
        protected override void HandleConect(Socket theClientSocket)
        {
            base.HandleConect(theClientSocket);
            this.InitClientSocketThread(theClientSocket);

        }
        /// <summary>
        /// 处理单个客户端的连接
        /// </summary>
        /// <param name="theClientSocket"></param>
        private void InitClientSocketThread(Socket theClientSocket)
        {
            
            //开启一个线程进行处理
            Task.Factory.StartNew(() => {
                this.ReceiveThreadCount += 1;
                RunReceiveClientThread(theClientSocket);
            }).ContinueWith(m => {
                this.ReceiveThreadCount -= 1;
            });
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
                while (true)
                {
                    IPEndPoint theRemoteEndPoint = (IPEndPoint)theClientSocket.RemoteEndPoint;
                    var theMessage = theClientSocket.ReceiveMessage();
                    Console.WriteLine("接收到消息: " + theMessage);
                    Console.WriteLine("send消息: " + theMessage);
                    theClientSocket.SendMessage(theRemoteEndPoint.Address + ":" + theRemoteEndPoint.Port + "");
                    Console.WriteLine("send消息: " + theMessage);
                    Thread.Sleep(1000);
                }

            }

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
        ///  释放实例
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
          
            if (this.keepWatchThread != null)
            {
                this.keepWatchThread.Interrupt();
                this.keepWatchThread = null;
            }
           
        }

    }
}

