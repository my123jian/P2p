using System;
namespace TcpServer
{
    /// <summary>
    /// 客户端的信息记录
    /// </summary>
    public class ClientStateInfo
    {
        /// <summary>
        /// 当前的地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 当前的端口地址
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 上次的访问时间
        /// </summary>
        public DateTime LastVisitTime { get; set; }
        /// <summary>
        /// 连接的时间
        /// </summary>
        public DateTime ConnectedTime { get; set; }
        /// <summary>
        /// 当前的用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 当前的用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        ///客户端的令牌
        /// </summary>
        public string Token { get; set; }

        public ClientStateInfo()
        {
        }
    }
}
