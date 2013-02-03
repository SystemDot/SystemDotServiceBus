using System;
using System.IO;

namespace SystemDot.Http
{
    public class WebRequestor : IWebRequestor
    {
        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            SendPut(address, toPerformOnRequest, s => { });
        }

        public void SendPut(
            FixedPortAddress address,
            Action<Stream> toPerformOnRequest,
            Action<Stream> toPerformOnResponse)
        {
            PutSender.Send(toPerformOnRequest, toPerformOnResponse, address.Url);
        }
    }
}