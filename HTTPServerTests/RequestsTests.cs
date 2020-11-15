using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HTTPServerTests
{
    [TestFixture]
    public class RequestsTests
    {

        string requestGetAllMsg =
            "GET /message/ HTTP/1.1\r\n" +
            "Content - Type: text / plain\r\n" +
            "User - Agent: PostmanRuntime / 7.26.8\r\n" +
            "Accept: /\r\n" +
            "Postman-Token: 3ed1e032-1635-46ce-93ae-dcf7259be6c3\r\n" +
            "Host: localhost:8080\r\n" +
            "Accept-Encoding: gzip, deflate, br\r\n" +
            "Connection: keep-alive\r\n" +
            "Content-Length: 4\r\n\r\n";

        string requestPostAtID =
            "POST /message/3 HTTP/1.1\r\n" +
            "Content - Type: text / plain\r\n" +
            "User - Agent: PostmanRuntime / 7.26.8\r\n" +
            "Accept: /\r\n" +
            "Postman-Token: 3ed1e032-1635-46ce-93ae-dcf7259be6c3\r\n" +
            "Host: localhost:8080\r\n" +
            "Accept-Encoding: gzip, deflate, br\r\n" +
            "Connection: keep-alive\r\n" +
            "Content-Length: 4\r\n\r\n";

        string requestPutAtID =
            "PUT /message/2 HTTP/1.1\r\n" +
            "Content - Type: text / plain\r\n" +
            "User - Agent: PostmanRuntime / 7.26.8\r\n" +
            "Accept: /\r\n" +
            "Postman-Token: 3ed1e032-1635-46ce-93ae-dcf7259be6c3\r\n" +
            "Host: localhost:8080\r\n" +
            "Accept-Encoding: gzip, deflate, br\r\n" +
            "Connection: keep-alive\r\n" +
            "Content-Length: 4\r\n\r\n";

        string requestDeleteAtID=
            "DELETE /message/1 HTTP/2.0\r\n" +
            "Content - Type: text / plain\r\n" +
            "User - Agent: PostmanRuntime / 7.26.8\r\n" +
            "Accept: /\r\n" +
            "Postman-Token: 3ed1e032-1635-46ce-93ae-dcf7259be6c3\r\n" +
            "Host: localhost:8080\r\n" +
            "Accept-Encoding: gzip, deflate, br\r\n" +
            "Connection: keep-alive\r\n" +
            "Content-Length: 4\r\n\r\n";

        [Test]
        public void GetRequestPartsAreCorrect()
        {
            var request = new HTTPServer.Requests(requestGetAllMsg);

            Assert.AreEqual(request.ID, "");
            Assert.AreEqual(request.Method, "GET ");
            Assert.AreEqual(request.Version, "1.1");
        }
        [Test]
        public void PostRequestPartsAreCorrect()
        {
            var request = new HTTPServer.Requests(requestPostAtID);

            Assert.AreEqual(request.ID, "3");
            Assert.AreEqual(request.Method, "POST ");
            Assert.AreEqual(request.Version, "1.1");
        }
        [Test]
        public void PutRequestPartsAreCorrect()
        {
            var request = new HTTPServer.Requests(requestPutAtID);

            Assert.AreEqual(request.ID, "2");
            Assert.AreEqual(request.Method, "PUT ");
            Assert.AreEqual(request.Version, "1.1");
        }
        public void DeleteRequestPartsAreCorrect()
        {
            var request = new HTTPServer.Requests(requestDeleteAtID);

            Assert.AreEqual(request.ID, "1");
            Assert.AreEqual(request.Method, "DELETE ");
            Assert.AreEqual(request.Version, "2.0");
        }
    }
}
