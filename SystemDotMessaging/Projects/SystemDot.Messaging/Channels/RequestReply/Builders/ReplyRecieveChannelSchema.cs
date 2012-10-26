using System.Collections.Generic;
using SystemDot.Messaging.Channels.Builders;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyRecieveChannelSchema : RecieveChannelSchema
    {
        public List<IMessageProcessor<object, object>> Hooks { get; set; }

        public ReplyRecieveChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
        }
    }
}