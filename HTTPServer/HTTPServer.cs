﻿using System;
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
        private bool running = false;
        Dictionary<string, string> messages = new Dictionary<string, string>();

        private TcpListener listener;

        public HTTPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            running = true;
            listener.Start();

            while (running)
            {
                //Eine Connection etablieren und diese halten sntatt immer wieder eine request-based erstellen
                Console.WriteLine("Waiting for connection..");

                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Client connected!");

                HandleClient(client);

                client.Close();
            }

            running = false;
            listener.Stop();
        }

        private void HandleClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());

            string msg = "";
            string output = "";
            while (reader.Peek() != -1)
            {
                msg += (char)reader.Read();
            }

            Debug.WriteLine("Request: \n" + msg);

            Requests request = new Requests(msg);
            Debug.WriteLine("Method:" + request.Method);
            Debug.WriteLine("Indentifier:" + request.Identifier);
            Debug.WriteLine("Command:" + request.Command);
            Debug.WriteLine("Version:" + request.Version);
            Debug.WriteLine("ContentType:" + request.ContentType);
            Debug.WriteLine("ContentLength:" + request.ContentLength);
            Debug.WriteLine("Payload:" + request.Payload);

            if (String.Compare(request.GetMethod(), "GET ") == 0)
            {
                if (String.Compare(request.Identifier, "all") == 0)
                {
                    foreach (KeyValuePair<string, string> kvp in messages)
                    {
                        Console.WriteLine(" {0}: Message = {1}",
                            kvp.Key, kvp.Value);
                    }
                    msg = "show all messages";
                }
                else
                {
                    messages.TryGetValue(request.Identifier, out output);
                    Console.WriteLine(output);
                    msg = "show message on position" + request.Identifier;
                }
            }
            else if (String.Compare(request.GetMethod(), "POST ") == 0)
            {
                messages.TryAdd(request.Identifier, request.Payload);
                msg = "new message added";
            }
            else if (String.Compare(request.GetMethod(), "PUT ") == 0)
            {
                if (String.Compare(request.Identifier, "all") == 0)
                {
                    msg = "message identifier not found";
                }
                else
                {
                    messages.Remove(request.Identifier);
                    messages.Add(request.Identifier, request.Payload);
                    msg = "put new message on position " + request.Identifier;
                }
            }
            else if (String.Compare(request.GetMethod(), "DELETE ") == 0)
            {
                if (String.Compare(request.Identifier, "all") == 0)
                {
                    msg = "message identifier not found";
                }
                else
                {
                    messages.Remove(request.Identifier);
                    msg = "message deleted on position " + request.Identifier;
                }
            }
            Console.WriteLine(msg + " " + request.GetLogEntry());
            NetworkStream stream = client.GetStream();
            Response response = new Response(200, request.Version);
            byte[] messg = Encoding.ASCII.GetBytes(response.StringFormHTTP());
            stream.Write(messg, 0, messg.Length);
         
        }

        
    }
}