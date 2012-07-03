using System;
using System.IO;

namespace SystemDot.Http
{
    public interface IWebRequestor
    {
        void SendPut(FixedPortAddress address, Action<Stream> performOnRequestStream);
        void SendPut(FixedPortAddress address, Action<Stream> performOnRequestStream, Action<Stream> performOnResponseStream);
        
    }
}