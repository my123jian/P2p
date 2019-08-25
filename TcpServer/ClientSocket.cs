using System;
using System.Net.Sockets;

namespace TcpServer
{
    public class ClientSocket
    {
        /// <summary>
        /// 客户端的套接字
        /// </summary>
        public Socket Socket { get; set; }
        /// <summary>
        /// 客户端对应的令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 最后一次发送的消息
        /// </summary>
        public DateTime LastSendTime { get; set; }
        /// <summary>
        /// 最后一次发送消息时间
        /// </summary>
        public DateTime LastRecvTime { get; set; }

        public ClientSocket()
        {
        }
    }
}
