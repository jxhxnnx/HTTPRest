using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;

namespace HTTPServer
{
    public class Requests
    {
        public string Version;
        public string ContentType;
        public string ContentLength;
        public string Method { get; }
        public string Command;
        public string ID;
        public string Message;

        public Requests(string _request)
        {
            string[] line = _request.Split("\r\n");
            string[] insideLine = line[0].Split("/");
            Method = insideLine[0];
            Command = insideLine[1];
            string[] id = insideLine[2].Split(" ");
            Version = insideLine[3];

            if (!string.IsNullOrEmpty(id[0]))
            {
                ID = id[0];
            }
            else
            {
                ID = "";
            }

            foreach (var item in line)
            {
                string[] typeLength = item.Split(":");
                if (String.Compare(typeLength[0], "Content-Type") == 0)
                {
                    ContentType = typeLength[1];
                }
                if (String.Compare(typeLength[0], "Content-Length") == 0)
                {
                    ContentLength = typeLength[1];
                }
            }
            Message = ExtractMessage(_request);
        }
        private static string ExtractMessage(string request)
        {
            if (string.IsNullOrEmpty(request))
            {
                return null;
            }
            string[] tokens = request.Split("\r\n\r\n");
            string message = tokens[1];
            return message;
        }
        public string ExtractLog()
        {

            string log = Method + " /" + Command + "/" + ID;
            return log;
        }
    }
}
