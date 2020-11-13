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
                if(string.Compare(request.ID, "") == 0 || !IDisDigit(request.ID))
                {
                    status = "400";
                    consoleMsg = "no valid ID";
                    clientMsg = "no valid ID";
                }
                else if(!messages.ContainsKey(request.ID))
                {
                    messages.Add(request.ID, request.Message);
                    consoleMsg = "added successfully";
                    clientMsg = "added successfully: " + request.Message + " at #" + request.ID;
                } 
                else
                { 
                    status = "400";
                    consoleMsg = "message #" + request.ID + "allready in use";
                    string msgWithRequestedID;
                    messages.TryGetValue(request.ID, out msgWithRequestedID);
                    clientMsg = "message #" + request.ID + " allready in use: " + msgWithRequestedID;
                }
                
            }
            else if (string.Compare(request.Method, "GET ") == 0)
            {
                if (string.Compare(request.ID, "") == 0)
                {
                    StringBuilder mystring = new StringBuilder();
                    foreach (KeyValuePair<string, string> keyValuePair in messages)
                    {
                        mystring.AppendLine("#" + keyValuePair.Key + "\t" + keyValuePair.Value);
                    }
                    consoleMsg = "get all messages";
                    clientMsg = mystring.ToString();
                }
                else if (!IDisDigit(request.ID))
                {
                    status = "400";
                    consoleMsg = "no valid ID";
                    clientMsg = "no valid ID";
                }
                else if(messages.ContainsKey(request.ID))
                {
                    messages.TryGetValue(request.ID, out clientMsg);
                    consoleMsg = "requested message #" + request.ID;
                }
                else
                {
                    status = "400";
                    consoleMsg = "no message #" + request.ID;
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
                else if (!IDisDigit(request.ID))
                {
                    status = "400";
                    consoleMsg = "no valid ID";
                    clientMsg = "no valid ID";
                }
                else
                {
                    if(messages.ContainsKey(request.ID))
                    {
                        messages.Remove(request.ID);
                        messages.Add(request.ID, request.Message);
                        consoleMsg = "changed message #" + request.ID + " to " + request.Message;
                        clientMsg = "changed message #" + request.ID + ": " + request.Message;
                    }
                    else
                    {
                        messages.Add(request.ID, request.Message);
                        consoleMsg = "new message #" + request.ID + " : " + request.Message;
                        clientMsg = "new message #" + request.ID + ": " + request.Message;
                    }
                    
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
                else if (!IDisDigit(request.ID))
                {
                    status = "400";
                    consoleMsg = "no valid ID";
                    clientMsg = "no valid ID";
                }
                else if(messages.ContainsKey(request.ID))
                {
                    messages.Remove(request.ID);
                    consoleMsg = "deleted message #" + request.ID;
                    clientMsg = "deleted message #" + request.ID;
                }
                else
                {
                    status = "400";
                    consoleMsg = "no message with #" + request.ID;
                    clientMsg = "no message with #" + request.ID;
                }
            }

            response.Respond(client.GetStream(), clientMsg, status, "plain/text");

            Console.WriteLine(consoleMsg + " " + request.ExtractLog());
        }

        bool IDisDigit(string ID)
        {
            for(int i = 0; i < ID.Length; i++)
            {
                if(!Char.IsNumber(ID[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}