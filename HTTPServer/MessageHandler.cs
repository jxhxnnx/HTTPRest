using System;
using System.Collections.Generic;
using System.Text;

namespace HTTPServer
{
    public class MessageHandler
    {
        /*  private string[] message;
        private int prev;

        public MessageHandler()
        {
            message = new string[100];
            prev = -1;
        }

        public int Add(string msg)
        {
            Console.WriteLine("Add " + msg);
            ++prev;
            message[prev] = msg;
            return prev;
        }

        private void Replace(int nr, string msg)
        {
            Console.WriteLine("Replace with " + msg);
            message[nr] = msg;
        }
        private void Delete(int nr)
        {
            message[nr] = null;
        }

        public Response RequestHandler(Requests request)
        {
            var header = new Dictionary<string, string>();

            if(request.Method == HTTPMethod.GET)
            {
                int nr = Convert.ToInt32(request.URI[1]);
                if(NrExists(nr))
                {
                    Response response = new Response(200, HTTPVersion.v11, header, MessageFinder(nr));
                    return response;
                }
                else
                {
                    Response responseFail = new Response(404, HTTPVersion.v11);
                    return responseFail;
                }
            }
            else if (request.Method == HTTPMethod.POST)
            {
                if (request.Body == null || request.Body.Length < 1)
                {
                    Response responseFail = new Response(404, HTTPVersion.v11);
                    return responseFail;
                }
                int nr = Add(request.Body);
                Response response = new Response(200, HTTPVersion.v11, header, nr.ToString());
                return response;
            }
            else if (request.Method == HTTPMethod.PUT)
            {
                if (request.Body == null || request.Body.Length < 1)
                {
                    Response responseFail = new Response(404, HTTPVersion.v11);
                    return responseFail;
                }
                int nr = Convert.ToInt32(request.URI[1]);
                
                if (NrExists(nr))
                {
                    Replace(nr, request.Body);
                    Response response = new Response(200, HTTPVersion.v11);
                    return response;
                } 
                else
                {
                    Response responseFail = new Response(404, HTTPVersion.v11);
                    return responseFail;
                }
            }
            else if (request.Method == HTTPMethod.DELETE)
            {
                int nr = Convert.ToInt32(request.URI[1]);
                if (NrExists(nr))
                {
                    Delete(nr);
                }
                Response response = new Response(200, HTTPVersion.v11);
                return response;
            }
            else
            {
                Response responseFail = new Response(405, HTTPVersion.v11);
                return responseFail;
            }
        }
        public bool NrExists(int nr)
        {
            if (message[nr] != null && message[nr].Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string MessageFinder(int nr)
        {
            Console.WriteLine("Get Message with Number " + nr);

            return message[nr];
        }*/

    }
}
