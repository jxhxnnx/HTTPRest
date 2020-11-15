using HTTPServer;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;

namespace HTTPServerTests
{
    [TestFixture]
    public class HTTPServerTests
    {

        HTTPServer.HTTPServer server = new HTTPServer.HTTPServer(8080);

        [Test]
        public void TestIDIsValid()
        {
            bool testnumber = server.IDisValid("5");
            bool teststring = server.IDisValid("lolo");
            Assert.IsTrue(testnumber);
            Assert.IsFalse(teststring);
        }
        [Test]
        public void HTTPServerIsNotNull()
        {
            Assert.IsNotNull(server);
        }


    }
}