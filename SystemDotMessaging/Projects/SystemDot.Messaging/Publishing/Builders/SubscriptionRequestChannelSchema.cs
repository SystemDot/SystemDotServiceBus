using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriptionRequestChannelSchema : IDurableOptionSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public EndpointAddress PublisherAddress { get; set; }

        public bool IsDurable { get; set; }

        public IRepeatStrategy RepeatStrategy { get; set; }
    }
}