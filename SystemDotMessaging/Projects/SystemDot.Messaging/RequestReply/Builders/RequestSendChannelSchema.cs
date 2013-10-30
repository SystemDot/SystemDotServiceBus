using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class RequestSendChannelSchema : IDurableOptionSchema
    {
        public EndpointAddress FromAddress { get; set; }

        public IMessageExpiryStrategy ExpiryStrategy { get; set; }

        public Action ExpiryAction { get; set; }

        public IRepeatStrategy RepeatStrategy { get; set; }

        public bool IsDurable { get; set; }

        public IMessageFilterStrategy FilteringStrategy { get; set; }

        public EndpointAddress ReceiverAddress { get; set; }

        public List<IMessageHook<object>> Hooks { get; set; }

        public List<IMessageHook<MessagePayload>> PostPackagingHooks { get; set; }

        public bool CorrelateReplyToRequest { get; set; }

        public RequestSendChannelSchema()
        {
            Hooks = new List<IMessageHook<object>>();
            PostPackagingHooks = new List<IMessageHook<MessagePayload>>();
        }
    }
}