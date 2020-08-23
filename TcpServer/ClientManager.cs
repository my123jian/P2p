using System;
using System.Collections.Generic;
using TcpServer.Model;

namespace TcpServer
{
    public class ClientManager
    {
        public List<ClientInfo> Clients
        {
            get;set;
        }

        public ClientManager()
        {
            this.Clients = new List<ClientInfo>();
        }

        /// <summary>
        /// 添加一个客户端
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(ClientInfo client)
        {
            lock (this)
            {
                this.Clients.Add(client);
            }
            
        }
    }
}
