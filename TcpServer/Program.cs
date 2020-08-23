using System;
using TcpServer.Model;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ByteBuffer buffer = new ByteBuffer();
                for(var i = 0; i < 1000; i++)
                {
                    buffer.Append(new byte[1024], 0, 1024);
                }

                var theDatas = buffer.ToBytes();
                //new SocketTest().TestConnectTimeOut("www.baidu.com");
                //Console.WriteLine("开始启动任务");
                //var theService = new UdpServerService();
                //var theClient = new UdpClientServer("47.88.169.123");
                //theClient.Start();
                //Console.WriteLine("启动任务成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("出现了错误");
                Console.WriteLine(ex);
            }
          
        }
    }
}
