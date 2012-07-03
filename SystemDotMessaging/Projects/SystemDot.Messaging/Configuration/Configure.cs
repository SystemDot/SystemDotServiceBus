using System.Net;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public class Configure
    {
        public static EndpointConfiguration Endpoint(EndpointAddress address)
        {
            return new EndpointConfiguration(address);
        }        
    }
}