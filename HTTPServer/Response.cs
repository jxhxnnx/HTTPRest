using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace HTTPServer
{
    public class Response
    {
        public void Post(NetworkStream stream, string message, string status, string contentType)
        {
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(HTTPServer._version + " " + status);
            sb.AppendLine("Content-Type: " + contentType);
            sb.AppendLine("Content-Length: " + Encoding.UTF8.GetBytes(message).Length);
            sb.AppendLine();
            sb.AppendLine(message);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            writer.Write(sb.ToString());
        }
    }
    
}
