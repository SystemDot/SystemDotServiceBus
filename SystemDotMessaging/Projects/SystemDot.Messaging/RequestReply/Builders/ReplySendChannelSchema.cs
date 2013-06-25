using System.Collections.Generic;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplySendChannelSchema : SendChannelSchema
    {
        public List<IMessageProcessor<object, object>> Hooks { get; set; }

        public ReplySendChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
        }
    }
}