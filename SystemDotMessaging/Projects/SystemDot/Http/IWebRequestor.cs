using System;
using System.IO;

namespace SystemDot.Http
{
    public interface IWebRequestor
    {
        void SendPut(string address, Action<Stream> performOnRequestStream);
        void SendPut(string address, Action<Stream> performOnRequestStream, Action<Stream> performOnResponseStream);
        
    }
}