using System.Diagnostics.Contracts;
using SystemDot.Messaging.Configuration.Publishers;
using SystemDot.Messaging.Configuration.Subscribers;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Configuration
{
    public class EndpointConfiguration
    {
        readonly EndpointAddress address;

        public EndpointConfiguration(EndpointAddress address)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            this.address = address;
        }

        public PublisherConfiguration AsPublisher()
        {
            return new PublisherConfiguration(this.address);
        }

        public SubscriberConfiguration Subscribes()
        {
            return new SubscriberConfiguration(this.address);
        }
    }
}