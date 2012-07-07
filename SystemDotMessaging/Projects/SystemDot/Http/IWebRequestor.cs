using System;
using System.IO;
using System.Threading.Tasks;

namespace SystemDot.Http
{
    public interface IWebRequestor
    {
        Task SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest);
        Task SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse);
        
    }
}