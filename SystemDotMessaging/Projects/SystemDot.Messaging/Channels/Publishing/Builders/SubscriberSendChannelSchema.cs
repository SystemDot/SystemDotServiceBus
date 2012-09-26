using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriberSendChannelSchema
    {
        public EndpointAddress FromAddress { get; private set; }

        public EndpointAddress SubscriberAddress { get; private set; }

        public SubscriberSendChannelSchema(EndpointAddress fromAddress, EndpointAddress subscriberAddress)
        {
            Contract.Requires(fromAddress != null);
            Contract.Requires(fromAddress != EndpointAddress.Empty);
            Contract.Requires(subscriberAddress != null);
            Contract.Requires(subscriberAddress != EndpointAddress.Empty);

            this.FromAddress = fromAddress;
            this.SubscriberAddress = subscriberAddress;
        }
    }
}