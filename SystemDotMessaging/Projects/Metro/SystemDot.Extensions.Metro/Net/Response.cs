using System;
using System.IO;

namespace System.Net
{
    public class Response
    {
        public Stream OutputStream { get; set; }
        public int StatusCode { get; set; }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}