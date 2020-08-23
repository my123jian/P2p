using System;
namespace TcpServer.Model
{
    public class SocketConfig
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
        public SocketConfig()
        {
        }
    }
}
