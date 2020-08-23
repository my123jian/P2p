using System;
using System.Net.Sockets;

namespace TcpServer.Model
{
    /// <summary>
    /// 当前的客户端的信息
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// 当前客户端的套接字
        /// </summary>
        public Socket Socket { get; set; }
        /// <summary>
        /// 当前客户端的地址
        /// </summary>
        public String  Address { get; set; }
        /// <summary>
        /// 当前客户端的端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 初次连接的时间
        /// </summary>
        public DateTime ConnectTime { get; set; }
        /// <summary>
        /// 最后一次的活动时间
        /// </summary>
        public DateTime LastActivityTime { get; set; }
        /// <summary>
        /// 最后一次发送的消息
        /// </summary>
        public DateTime LastSendTime { get; set; }
        /// <summary>
        /// 最后一次发送消息时间
        /// </summary>
        public DateTime LastRecvTime { get; set; }
        /// <summary>
        /// 当前的客户端ID
        /// </summary>
        public String ClientId { get; set; }
        /// <summary>
        /// 当前的客户端名称
        /// </summary>
        public String ClientName { get; set; }

       /// <summary>
       /// 构造函数
       /// </summary>
        public ClientInfo()
        {

        }
        /// <summary>
        /// 是否是有效的客户端
        /// </summary>
        /// <returns></returns>
        public bool IsValidClient()
        {
            return this.Socket != null;
        }
    }
}
