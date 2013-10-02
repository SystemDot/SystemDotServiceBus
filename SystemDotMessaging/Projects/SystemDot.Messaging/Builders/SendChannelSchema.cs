using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Builders
{
    class SendChannelSchema : IDurableOptionSchema
    {
        public EndpointAddress FromAddress { get; set; }

        public IMessageExpiryStrategy ExpiryStrategy { get; set; }

        public Action ExpiryAction { get; set; }

        public IRepeatStrategy RepeatStrategy { get; set; }

        public bool IsDurable { get; set; }
    }
}