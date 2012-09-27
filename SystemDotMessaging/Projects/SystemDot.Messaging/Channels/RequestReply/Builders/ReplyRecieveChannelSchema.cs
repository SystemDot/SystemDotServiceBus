using System.Collections.Generic;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyRecieveChannelSchema
    {
        public EndpointAddress SenderAddress { get; set; }

        public List<IMessageProcessor<object, object>> Hooks { get; private set; }

        public ReplyRecieveChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
        }
    }
}