using System;
using System.IO;
using SystemDot.Http;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    public class FailingWebRequestor : IWebRequestor
    {
        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            throw new Exception();
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse)
        {
            throw new Exception();
        }
    }
}