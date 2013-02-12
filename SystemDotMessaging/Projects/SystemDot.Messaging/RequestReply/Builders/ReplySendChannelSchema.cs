using System.Collections.Generic;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.RequestReply.Builders
{
    public class ReplySendChannelSchema : SendChannelSchema
    {
        public List<IMessageProcessor<object, object>> Hooks { get; set; }

        public ReplySendChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
        }
    }
}