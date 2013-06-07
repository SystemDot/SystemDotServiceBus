using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Builders
{
    class RecieveChannelSchema : ChannelSchema
    {
        public EndpointAddress Address { get; set; }
        public EndpointAddress ToAddress { get; set; }
        public Func<IMessageProcessor<object, object>> UnitOfWorkRunnerCreator { get; set; }
        public List<IMessageProcessor<object, object>> Hooks { get; set; }

        public RecieveChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
        }
    }
}