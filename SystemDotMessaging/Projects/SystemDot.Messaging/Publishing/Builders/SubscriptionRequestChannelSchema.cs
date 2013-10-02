using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriptionRequestChannelSchema : IDurableOptionSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public EndpointAddress PublisherAddress { get; set; }

        public bool IsDurable { get; set; }
    }
}