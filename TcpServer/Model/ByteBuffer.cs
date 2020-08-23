using System;
namespace TcpServer.Model
{
    public class ByteBuffer:IDisposable
    {
        private System.IO.MemoryStream theBufferStream = null;
        public ByteBuffer()
        {
            
        }
        public void Append(Byte[] data,int start,int end)
        {
            if (theBufferStream == null)
            {
                theBufferStream = new System.IO.MemoryStream();
            }
            for (var i = start; i < end; i++){
                theBufferStream.WriteByte(data[i]);
            }
           
        }

        public void Dispose()
        {
            if (theBufferStream != null)
            {
                theBufferStream.Close();
                theBufferStream.Dispose();
            }
        }

        public byte[] ToBytes()
        {
            var theBytes = this.theBufferStream.ToArray();
            this.theBufferStream = null;
            return theBytes;
        }
    }
}
