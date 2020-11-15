using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace HTTPServer
{
    public interface IHTTPServer
    {
        public void ClientHandler(TcpClient client);
    }
}
