using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HTTPServer
{
    
    public class HTTPServer
    {
        public const String _version = "HTTP/1.1";
        private bool running = false;
        Dictionary<string, string> messages = new Dictionary<string, string>();
        private TcpListener listener;

        public HTTPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        private void Run()
        {
            running = true;
            listener.Start();

            while (running)
            {
                Console.WriteLine("Waiting for connections...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected successfully!");
                ClientHandler(client);
                client.Close();
            }

            running = false;
            listener.Stop();
        }

        private void ClientHandler(TcpClient client)
        {
            Response response = new Response();
            StreamReader reader = new StreamReader(client.GetStream());
            string clientMsg = "";
            string consoleMsg = "";
            string status = "200";

            while (reader.Peek() != -1)
            {
                clientMsg += (char)reader.Read();
            }
            Requests request = new Requests(clientMsg);

            if (string.Compare(request.Method, "POST ") == 0)
            {
                messages.TryAdd(request.ID, request.Message);
                consoleMsg = "adding successful";
                clientMsg = "added successful: " + request.Message;
            }
            else if (string.Compare(request.Method, "GET ") == 0)
            {
                if (string.Compare(request.ID, "") == 0)
                {
                    StringBuilder mystring = new StringBuilder();
                    foreach (KeyValuePair<string, string> keyValuePair in messages)
                    {
                        mystring.AppendLine(keyValuePair.Key + ":\t" + keyValuePair.Value + "\r\n");
                    }
                    consoleMsg = "get all messages";
                    clientMsg = mystring.ToString();
                }
                else
                {
                    messages.TryGetValue(request.ID, out clientMsg);
                    consoleMsg = "requested message: " + request.ID;
                }
            }
            else if (string.Compare(request.Method, "PUT ") == 0)
            {
                if (string.Compare(request.ID, "") == 0)
                {
                    status = "400";
                    consoleMsg = "not existing message ID requested";
                    clientMsg = "message ID not existing";
                    
                }
                else
                {
                    messages.Remove(request.ID);
                    messages.Add(request.ID, request.Message);
                    consoleMsg = "changed message #" + request.ID;
                    clientMsg = "changed message #" + request.ID + ": " + request.Message;
                }
            }
            else if (string.Compare(request.Method, "DELETE ") == 0)
            {
                if (string.Compare(request.ID, "") == 0)
                {
                    status = "400";
                    consoleMsg = "not existing message ID requested";
                    clientMsg = "message ID not existing";
                }
                else
                {
                    messages.Remove(request.ID);
                    consoleMsg = "deleted message #" + request.ID;
                    clientMsg = "deleted message #" + request.ID;
                }
            }

            response.Post(client.GetStream(), clientMsg, status, "plain/text");

            Console.WriteLine(consoleMsg + " " + request.ExtractLog());
        }
    }
}