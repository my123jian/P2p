using System;
namespace TcpServer
{
    public enum Commands
    {
        /// <summary>
        /// 登录功能
        /// </summary>
        LOGIN=1,
        /// <summary>
        /// 登出功能
        /// </summary>
        LOGOFF=2,
        //心跳功能
        HEART=4,
        //转换地址
        REDIRECT=8

    }
}
