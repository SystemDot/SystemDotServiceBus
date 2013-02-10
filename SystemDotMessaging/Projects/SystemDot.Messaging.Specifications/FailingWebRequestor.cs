using System;
using System.IO;
using SystemDot.Http;

namespace SystemDot.Messaging.Specifications
{
    public class FailingWebRequestor : IWebRequestor
    {
        public int RequestCount { get; private set; }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            RequestCount++;
            throw new Exception();
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse)
        {
            RequestCount++;
            throw new Exception();
        }
    }
}