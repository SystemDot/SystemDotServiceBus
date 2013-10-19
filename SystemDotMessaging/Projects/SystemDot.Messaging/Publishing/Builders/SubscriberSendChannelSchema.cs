using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriberSendChannelSchema : IDurableOptionSchema
    {       
        public EndpointAddress SubscriberAddress { get; set; }
    
        public EndpointAddress FromAddress { get; set; }

        public IMessageExpiryStrategy ExpiryStrategy { get; set; }

        public IRepeatStrategy RepeatStrategy { get; set; }

        public bool IsDurable { get; set; }
    }
}