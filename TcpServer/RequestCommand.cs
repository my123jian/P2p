using System;
namespace TcpServer
{
    /// <summary>
    /// 命令发送
    /// </summary>
    public class RequestCommand
    {
        /// <summary>
        ///当前的令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 当前的名称
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 当前的操作
        /// </summary>
        public string Action { get; set; }
        public RequestCommand()
        {
        }
    }
}
