using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Publishing
{
    class PublisherRegistry : IPublisherRegistry
    {
        readonly Dictionary<string, IPublisher> publishers;

        public PublisherRegistry()
        {
            publishers = new Dictionary<string, IPublisher>();
        }

        public void RegisterPublisher(EndpointAddress address, IPublisher publisher)
        {
            Contract.Requires(address != null);
            Contract.Requires(publisher != null);
            this.publishers.Add(address.Channel, publisher);
        }

        public IPublisher GetPublisher(EndpointAddress address)
        {
            Contract.Requires(address != null);
            return publishers[address.Channel];
        }
    }
}