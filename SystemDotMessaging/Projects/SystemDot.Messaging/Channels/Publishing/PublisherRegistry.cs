using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class PublisherRegistry : IPublisherRegistry
    {
        readonly Dictionary<EndpointAddress, IPublisher> publishers;

        public PublisherRegistry()
        {
            this.publishers = new Dictionary<EndpointAddress, IPublisher>();
        }

        public void RegisterPublisher(EndpointAddress address, IPublisher publisher)
        {
            Contract.Requires(address != null);
            Contract.Requires(publisher != null);
            this.publishers.Add(address, publisher);
        }

        public IPublisher GetPublisher(EndpointAddress address)
        {
            Contract.Requires(address != null);
            return this.publishers[address];
        }
    }
}