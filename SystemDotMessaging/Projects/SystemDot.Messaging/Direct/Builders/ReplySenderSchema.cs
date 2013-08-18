using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Direct.Builders
{
    class ReplySenderSchema
    {
        public EndpointAddress Address { get; set; }
        public List<IMessageHook<MessagePayload>> Hooks { get; set; }

        public ReplySenderSchema()
        {
            Hooks = new List<IMessageHook<MessagePayload>>();
        }
    }
}