using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Transport.Http.Remote.Clients.Configuration
{
    public class HttpRemoteClientTransportBuilder : ITransportBuilder
    {
        readonly LongPoller longPoller;

        public HttpRemoteClientTransportBuilder(LongPoller longPoller)
        {
            Contract.Requires(longPoller != null);
            this.longPoller = longPoller;
        }

        public void Build(EndpointAddress address)
        {
            this.longPoller.RegisterAddress(address);
        }
    }
}