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
        public void Respond(NetworkStream stream, string message, string status, string contentType)
        {
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
            StringBuilder mystring = new StringBuilder();
            mystring.AppendLine(HTTPServer._version + " " + status);
            mystring.AppendLine("Content-Type: " + contentType);
            mystring.AppendLine("Content-Length: " + Encoding.UTF8.GetBytes(message).Length);
            mystring.AppendLine();
            mystring.AppendLine(message);
            System.Diagnostics.Debug.WriteLine(mystring.ToString());
            writer.Write(mystring.ToString());
        }
    }
    
}
