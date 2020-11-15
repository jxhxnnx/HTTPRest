using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HTTPServer
{
    public interface IClientWrapper
    {
        Stream GetStream();
        void Close();
    }
}
