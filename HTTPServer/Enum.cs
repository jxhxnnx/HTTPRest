using System;
using System.Collections.Generic;
using System.Text;

namespace HTTPServer
{
    public enum HTTPMethod { GET, POST, PUT, DELETE };
    public enum HTTPVersion { v10, v11, v20, v30, none }
}
