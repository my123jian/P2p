using System;
namespace TcpServer.Model
{
    public class ServerConfig
    {
        /// <summary>
        /// 绑定的地址
        /// </summary>
        public String BindAddress { get; set; }
        /// <summary>
        /// 绑定的端口
        /// </summary>
        public int BindPort { get; set; }
        /// <summary>
        /// 是否是异步socket
        /// </summary>
        public bool AsyncScoket { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        public ServerConfig()
        {
        }
    }
}
