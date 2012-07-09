using System;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionSchema
    {
        public EndpointAddress SubscriberAddress { get; set; }

        public SubscriptionSchema() {}

        public SubscriptionSchema(EndpointAddress subscriberAddress)
        {
            SubscriberAddress = subscriberAddress;
        }
    }
}