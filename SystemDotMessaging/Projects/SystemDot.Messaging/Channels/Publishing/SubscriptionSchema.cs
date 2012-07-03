using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    [Serializable]
    public class SubscriptionSchema
    {
        public EndpointAddress SubscriberAddress { get; private set; }

        public SubscriptionSchema(EndpointAddress subscriberAddress)
        {
            SubscriberAddress = subscriberAddress;
        }
    }
}