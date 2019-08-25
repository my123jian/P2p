using System;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("开始启动任务");
                //var theService = new ServerService();
                var theClient = new ClientService("47.88.169.123");
                theClient.Start();
                Console.WriteLine("启动任务成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("出现了错误");
                Console.WriteLine(ex);
            }
          
        }
    }
}
