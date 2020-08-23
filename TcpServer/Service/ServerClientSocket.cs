using System;
using System.Net.Sockets;
using TcpServer.Model;

namespace TcpServer.Service
{
    public delegate void OnDataReceived(ServerClientSocket sender,byte[]Data);
    public class ServerClientSocket : IDisposable
    {
        public Socket Socket { get; set; }
        private ByteBuffer theDataBuffer;
        protected byte[] Buffer;
        public event OnDataReceived OnDataReceived;
        public ServerClientSocket()
        {
            this.Buffer = new byte[1024];
            this.theDataBuffer = new ByteBuffer();
        }

        protected virtual void DoRecv()
        {
            this.Socket.BeginReceive(Buffer, 0, 1024, SocketFlags.None, ReceiveCallBack, 0);
        }

        protected virtual void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int count = this.Socket.EndReceive(ar);
                if (count >= 0)
                {
                    this.theDataBuffer.Append(this.Buffer, 0, count);
                    this.DoRecv();
                }
                else
                {
                    this.OnReceived(this.theDataBuffer.ToBytes());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected virtual void OnReceived(byte[] Data)
        {
            this.OnDataReceived(this, Data);
        }

        public void Dispose()
        {
            if (this.Socket != null) {
                this.Socket.Close();
            }
            this.theDataBuffer.Dispose();
            
        }
    }
}
