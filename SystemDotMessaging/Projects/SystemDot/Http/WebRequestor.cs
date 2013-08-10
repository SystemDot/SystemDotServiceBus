using System;
using System.IO;
using SystemDot.Logging;

namespace SystemDot.Http
{
    public class WebRequestor : IWebRequestor
    {
        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            Log(address);
            SendPut(address, toPerformOnRequest, s => { }, _ => { }, () => { });
        }

        public void SendPut(
            FixedPortAddress address,
            Action<Stream> toPerformOnRequest,
            Action<Stream> toPerformOnResponse,
            Action<Exception> toPerformOnError,
            Action toPerformOnCompletion)
        {
            Log(address);

            PutSender.Send(toPerformOnRequest, toPerformOnResponse, toPerformOnError, toPerformOnCompletion, address.Url);
        }

        static void Log(FixedPortAddress address)
        {
            Logger.Debug("Sending put to {0}", address);
        }
    }
}