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
            //Wird verwendet um Message an den Client zu schreiben
            Response response = new Response();
            //Wird verwendet um Messages des Clients auszulesen
            StreamReader reader = new StreamReader(client.GetStream());
            //Zwischenspeicher für Messages von und an den Client
            string msg = "";
            //Zwischenspeicher für Messages an die Console
            string log = "";
            //Speichert den Status der bei der Response angegeben werden soll
            string responseStatus = "200";
            //Message wird aud dem Stream char für char in msg gespeichert
            while (reader.Peek() != -1)
            {
                msg += (char)reader.Read();
            }
            //Wird verwendet um die Request des Clients zu Parsen
            Requests request = new Requests(msg);
            //Funktionalitäten gehören aus der Konsole in eine Response ausgelagert
            if (String.Compare(request.GetMethod(), "GET ") == 0)
            {
                if (String.Compare(request.Identifier, "all") == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (KeyValuePair<string, string> kvp in messages)
                    {
                        sb.AppendLine(kvp.Key + ":\t" + kvp.Value + "\r\n");
                    }
                    log = "show all messages";
                    msg = sb.ToString();
                    
                }
                else
                {
                    messages.TryGetValue(request.Identifier, out msg);
                    log = "show message on position" + request.Identifier;
                    msg = "show message on position" + request.Identifier;
                }
            }
            else if (String.Compare(request.GetMethod(), "POST ") == 0)
            {
                messages.TryAdd(request.Identifier, request.Payload);
                log = "new message added";
                msg = "new message added";
            }
            else if (String.Compare(request.GetMethod(), "PUT ") == 0)
            {
                if (String.Compare(request.Identifier, "all") == 0)
                {
                    log = "message identifier not found";
                    msg = "message identifier not found";
                    responseStatus = "400";
                }
                else
                {
                    messages.Add(request.Identifier, request.Payload);
                    log = "put new message on position " + request.Identifier;
                    msg = "put new message on position " + request.Identifier;
                }
            }
            else if (String.Compare(request.GetMethod(), "DELETE ") == 0)
            {
                if (String.Compare(request.Identifier, "all") == 0)
                {
                    log = "message identifier not found";
                    responseStatus = "400";
                    msg = "message identifier not found";
                }
                else
                {
                    messages.Remove(request.Identifier);
                    log = "message deleted on position " + request.Identifier;
                    msg = "message deleted on position " + request.Identifier;
                }
            }
            response.Post(client.GetStream(), msg, responseStatus, "plain/text");

            Console.WriteLine(log + " " + request.GetLogEntry());

        }


    }
}