using System;
namespace TcpServer
{
    public class Config
    {
        /// <summary>
        /// 接收延迟
        /// </summary>
        public int ReceiveTimeout { get; set; }
        /// <summary>
        /// 发送延迟
        /// </summary>
        public int SendTimeout { get; set; }
        /// <summary>
        /// 当前的配置
        /// </summary>
        public Config()
        {
        }
    }
}
