using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class RequestSendChannelSchema : SendChannelSchema
    {
        public IMessageFilterStrategy FilteringStrategy { get; set; }

        public EndpointAddress ReceiverAddress { get; set; }

        public List<IMessageHook<object>> Hooks { get; set; }

        public List<IMessageHook<MessagePayload>> PostPackagingHooks { get; set; }

        public RequestSendChannelSchema()
        {
            Hooks = new List<IMessageHook<object>>();
            PostPackagingHooks = new List<IMessageHook<MessagePayload>>();
        }
    }
}