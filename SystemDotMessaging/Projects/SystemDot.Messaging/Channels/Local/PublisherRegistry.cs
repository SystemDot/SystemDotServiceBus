using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Local
{
    public class PublisherRegistry
    {
        readonly Dictionary<Address, IDistributor> publishers;

        public PublisherRegistry()
        {
            this.publishers = new Dictionary<Address, IDistributor>();
        }

        public void RegisterPublisher(Address address, IDistributor distributor)
        {
            Contract.Requires(address != null);
            Contract.Requires(distributor != null);
            this.publishers.Add(address, distributor);
        }

        public IDistributor GetPublisher(Address address)
        {
            Contract.Requires(address != null);
            return this.publishers[address];
        }
    }
}