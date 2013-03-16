using System.Collections.Generic;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplyReceiveChannelSchema : RecieveChannelSchema
    {
        public List<IMessageProcessor<object, object>> Hooks { get; set; }

        public ReplyReceiveChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
        }
    }
}