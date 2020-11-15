using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace HTTPServer
{
    public class ClientWrapper : IClientWrapper
    {
        private TcpClient wrapperClient;
        public ClientWrapper()
        {
            this.wrapperClient = new TcpClient();
        }
        public ClientWrapper(TcpClient client)
        {
            this.wrapperClient = client;
        }
        public Stream GetStream()
        {
            return wrapperClient.GetStream();
        }
        public void Close()
        {
            this.wrapperClient.Close();
        }
    }
}
