using System.Collections.Generic;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Hooks;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplySendChannelSchema : SendChannelSchema
    {
        public List<IMessageHook<object>> Hooks { get; set; }

        public ReplySendChannelSchema()
        {
            Hooks = new List<IMessageHook<object>>();
        }
    }
}