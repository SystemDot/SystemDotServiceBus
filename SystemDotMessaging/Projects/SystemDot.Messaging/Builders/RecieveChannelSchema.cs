using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Builders
{
    class RecieveChannelSchema : ChannelSchema
    {
        public EndpointAddress Address { get; set; }

        public EndpointAddress ToAddress { get; set; }

        public Func<IMessageProcessor<object, object>> UnitOfWorkRunnerCreator { get; set; }

        public List<IMessageHook<object>> Hooks { get; set; }

        public List<IMessageHook<MessagePayload>> PreUnpackagingHooks { get; set; }

        public IMessageFilterStrategy FilterStrategy { get; set; }
        
        public RecieveChannelSchema()
        {
            Hooks = new List<IMessageHook<object>>();
            PreUnpackagingHooks = new List<IMessageHook<MessagePayload>>();
        }
    }
}