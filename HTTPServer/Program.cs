using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server on port 8080!");
            HTTPServer server = new HTTPServer(8080);
            server.Start();
        }
    }
}
