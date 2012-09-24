using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class PublisherRegistry : IPublisherRegistry
    {
        readonly Dictionary<EndpointAddress, IDistributor> publishers;

        public PublisherRegistry()
        {
            this.publishers = new Dictionary<EndpointAddress, IDistributor>();
        }

        public void RegisterPublisher(EndpointAddress address, IDistributor distributor)
        {
            Contract.Requires(address != null);
            Contract.Requires(distributor != null);
            this.publishers.Add(address, distributor);
        }

        public IDistributor GetPublisher(EndpointAddress address)
        {
            Contract.Requires(address != null);
            return this.publishers[address];
        }
    }
}