using System;
namespace TcpServer
{
    /// <summary>
    /// 返回结果命令
    /// </summary>
    public class ResponseCommand
    {
        /// <summary>
        /// 当前的ip地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 当前的端口地址
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 端口保留时间
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 当前的令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 返回一个请求命令
        /// </summary>
        public ResponseCommand()
        {
        }

        public static ResponseCommand New()
        {
            return new ResponseCommand();
        }
        public ResponseCommand SetIpAddress(string Address)
        {
            this.IpAddress = Address;
            return this;
        }

        public ResponseCommand SetPort(int  Port)
        {
            this.Port = Port;
            return this;
        }

        public ResponseCommand SetToken(string Token)
        {
            this.Token = Token;
            return this;
        }
    }
}
