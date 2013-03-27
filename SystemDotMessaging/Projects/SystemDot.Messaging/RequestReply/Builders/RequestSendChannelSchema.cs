using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class RequestSendChannelSchema : SendChannelSchema
    {
        public IMessageFilterStrategy FilteringStrategy { get; set; }

        public EndpointAddress RecieverAddress { get; set; }

        public List<IMessageProcessor<object, object>> Hooks { get; set; }

        public RequestSendChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
        }
    }
}