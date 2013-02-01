using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.Publishing.Builders
{
    public class SubscriptionRequestChannelSchema : ChannelSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public EndpointAddress PublisherAddress { get; set; }
    }
}