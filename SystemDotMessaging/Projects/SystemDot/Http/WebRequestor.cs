using System;
using System.IO;

namespace SystemDot.Http
{
    public class WebRequestor : IWebRequestor
    {
        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            SendPut(address, toPerformOnRequest, s => { }, _ => { }, () => { });
        }

        public void SendPut(
            FixedPortAddress address,
            Action<Stream> toPerformOnRequest,
            Action<Stream> toPerformOnResponse,
            Action<Exception> toPerformOnError,
            Action toPerformOnCompletion)
        {
            PutSender.Send(toPerformOnRequest, toPerformOnResponse, toPerformOnError, toPerformOnCompletion, address.Url);
        }
    }
}