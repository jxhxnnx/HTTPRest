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
        public int Status { get; }
        public string Version { get; }
        public Dictionary<string, string> HeaderVal { get; }
        public string Body { get; }

        public Response(int status, string version)
        {
            Status = status;
            Version = version;
            HeaderVal = new Dictionary<string, string>();
            Body = "";
        }
        /*public Response(int status, HTTPVersion version, Dictionary<string, string> headerVal)
        {
            Status = status;
            Version = version;
            HeaderVal = headerVal;
            Body = "";
        }
        public Response(int status, string version, Dictionary<string, string> headerVal, string body)
        {
            Status = status;
            Version = version;
            HeaderVal = headerVal;
            Body = body;
        }*/

        public string StringFormHTTP()
        {
            StringBuilder mystring = new StringBuilder();
            if (String.Compare(Version, "1.0") == 0)
            {
                mystring.Append("HTTP/1.0");
            }
            else if (String.Compare(Version, "1.1") == 0)
            {
                mystring.Append("HTTP/1.1");
            }
            else if (String.Compare(Version, "2.0") == 0)
            {
                mystring.Append("HTTP/2.0");
            }
            else if (String.Compare(Version, "3.0") == 0)
            {
                mystring.Append("HTTP/3.0");
            }
            else throw new Exception("no version");

            mystring.Append(" ");
            mystring.Append(Status);
            mystring.Append(" ");

            if (Status == 200)
            {
                mystring.Append("OK");
            }
            else if (Status == 400)
            {
                mystring.Append("Bad Request");
            }
            else if (Status == 404)
            {
                mystring.Append("Not Found");
            }
            else if (Status == 405)
            {
                mystring.Append("Method Not Allowed");
            }
            mystring.Append("\r\n");


            return mystring.ToString();
        }
        public override string ToString()
        {
            StringBuilder mystring = new StringBuilder();
            mystring.AppendLine("Response info:");
            mystring.AppendLine("\tHTTP Version: " +  Version);
            mystring.AppendLine("\tHTTP Status: " + Status);
            mystring.AppendLine("\tHeader Values (" + HeaderVal.Count + "):");
            foreach (var value in HeaderVal)
            {
                mystring.AppendLine("\t\t" + value.Key + " => " + value.Value);
            }
            if (Body.Length < 1)
            {
                mystring.Append("\tNo Body");
            }
            else if (Body.Length < 64)
            {
                mystring.Append("\tBody (" + Body.Length + "): " + Body);
            }
            else
            {
                mystring.Append("\tBody (" + Body.Length + "): " + Body.Substring(0, 49) + " [...truncated]");
            }
            return mystring.ToString();
        }
    }
}
