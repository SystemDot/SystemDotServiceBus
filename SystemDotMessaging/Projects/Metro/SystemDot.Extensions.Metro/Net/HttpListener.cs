using System;
using System.Collections.Generic;

namespace System.Net
{
    public class HttpListener
    {
        public IList<object> Prefixes { get; set; }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetContext(AsyncCallback arg1, object arg2)
        {
            throw new NotImplementedException();
        }

        public HttpListenerContext EndGetContext(IAsyncResult arg)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}