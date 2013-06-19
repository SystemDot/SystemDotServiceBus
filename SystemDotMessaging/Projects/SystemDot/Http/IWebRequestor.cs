using System;
using System.IO;

namespace SystemDot.Http
{
    public interface IWebRequestor
    {
        void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest);
        
        void SendPut(
            FixedPortAddress address, 
            Action<Stream> toPerformOnRequest, 
            Action<Stream> toPerformOnResponse, 
            Action<Exception> toPerformOnError,
            Action toPerformOnCompletion);
        
    }
}