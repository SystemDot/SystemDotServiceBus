using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplySendChannelSchema : IDurableOptionSchema
    {
        public EndpointAddress FromAddress { get; set; }

        public IMessageExpiryStrategy ExpiryStrategy { get; set; }

        public Action ExpiryAction { get; set; }

        public IRepeatStrategy RepeatStrategy { get; set; }

        public bool IsDurable { get; set; }
    
        public List<IMessageHook<object>> Hooks { get; set; }

        public ReplySendChannelSchema()
        {
            Hooks = new List<IMessageHook<object>>();
        }
    }
}