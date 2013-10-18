using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    class PointToPointSendChannelSchema : IDurableOptionSchema
    {
        public EndpointAddress FromAddress { get; set; }

        public IMessageExpiryStrategy ExpiryStrategy { get; set; }

        public Action ExpiryAction { get; set; }

        public IRepeatStrategy RepeatStrategy { get; set; }

        public bool IsDurable { get; set; }
    
        public EndpointAddress ReceiverAddress { get; set; }

        public IMessageFilterStrategy FilteringStrategy { get; set; }
    }
}