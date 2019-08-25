using System;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
    /// <summary>
    /// 1:如何连接socket
    /// 2:如何防止socket阻塞
    /// 3:如何大的并发访问 实现
    /// </summary>
    public static class SocketExt
    {

        /// <summary>
        /// 发送模型信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Socket"></param>
        /// <param name="Data"></param>
        public static void Send<T>(this Socket Socket, T Data) where T:class,new()
        {
            var theSendMesssage = JsonHelper.toJson(Data);
            Socket.SendMessage(theSendMesssage);
            //todo
        }
        /// <summary>
        /// 接收模型信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Socket"></param>
        /// <returns></returns>
        public static T Recv<T>(this Socket Socket) where T : class, new()
        {
            var theMessage = Socket.ReceiveMessage();
            return JsonHelper.fromJson<T>(theMessage);
            //todo
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="Socket"></param>
        /// <param name="Command"></param>
        public static void SendCommand(this Socket Socket,RequestCommand Command)
        {
            var theSendMesssage = JsonHelper.toJson(Command);
            Socket.SendMessage(theSendMesssage);
            //todo
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="Socket"></param>
        /// <param name="Command"></param>
        public static void SendCommand(this Socket Socket, ResponseCommand Command)
        {
            var theSendMesssage = JsonHelper.toJson(Command);
            Socket.SendMessage(theSendMesssage);
            //todo
        }
        /// <summary>
        /// 接收服务端命令
        /// </summary>
        /// <param name="Socket"></param>
        /// <returns></returns>
        public static ResponseCommand ReceiveServiceCommand(this Socket Socket)
        {
            var theMessage = Socket.ReceiveMessage();
            return JsonHelper.fromJson<ResponseCommand>(theMessage);
        }
        /// <summary>
        /// 接收客户端的命令
        /// </summary>
        /// <param name="Socket"></param>
        /// <returns></returns>
        public static RequestCommand ReceiveClientCommand(this Socket Socket)
        {
            var theMessage = Socket.ReceiveMessage();
            return JsonHelper.fromJson<RequestCommand>(theMessage);
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="Message"></param>
        public static void SendMessage(this Socket Socket, string Message)
        {
            var theBytes = Encoding.UTF8.GetBytes(Message);
            Socket.Send(theBytes);
            


        }

        /// <summary>
        /// 接收信息
        /// </summary>
        /// <returns></returns>
        public static String ReceiveMessage(this Socket Socket)
        {
            var theReceivedBuffer = new byte[100];
            var theResultMessage = "";
            int bytes = 0;

            // The following will block until the page is transmitted.
            do
            {
                bytes = Socket.Receive(theReceivedBuffer, theReceivedBuffer.Length, 0);
                theResultMessage = theResultMessage + Encoding.UTF8.GetString(theReceivedBuffer, 0, bytes);
            }
            while (bytes > theReceivedBuffer.Length);

            return theResultMessage;
        }
    }
}
