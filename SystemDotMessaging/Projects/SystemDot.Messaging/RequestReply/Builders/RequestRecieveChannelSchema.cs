using System.Collections.Generic;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class RequestRecieveChannelSchema : RecieveChannelSchema
    {
        public List<IMessageProcessor<object, object>> Hooks { get; set; }

        public RequestRecieveChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
        }
    }
}