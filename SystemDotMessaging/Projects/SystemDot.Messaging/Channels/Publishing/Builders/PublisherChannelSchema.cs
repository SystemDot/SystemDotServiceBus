using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Filtering;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class PublisherChannelSchema
    {
        public EndpointAddress Address { get; private set; }

        public IMessageFilterStrategy MessageFilterStrategy { get; private set; }

        public PublisherChannelSchema(EndpointAddress address, IMessageFilterStrategy messageFilterStrategy)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(messageFilterStrategy != null);
            
            this.Address = address;
            this.MessageFilterStrategy = messageFilterStrategy;
        }
    }
}