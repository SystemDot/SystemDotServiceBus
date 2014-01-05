using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriberRecieveChannelSchema : ISequenceOptionSchema, IDurableOptionSchema
    {
        public EndpointAddress Address { get; set; }

        public EndpointAddress ToAddress { get; set; }

        public Func<IMessageProcessor<object, object>> UnitOfWorkRunnerCreator { get; set; }

        public List<IMessageHook<object>> Hooks { get; set; }

        public List<IMessageHook<MessagePayload>> PreUnpackagingHooks { get; set; }

        public IMessageFilterStrategy FilterStrategy { get; set; }

        public bool IsSequenced { get; set; }

        public bool IsDurable { get; set; }

        public bool HandleEventsOnMainThread { get; set; }

        public bool ContinueOnException { get; set; }

        public bool BlockMessages { get; set; }

        public SubscriberRecieveChannelSchema()
        {
            Hooks = new List<IMessageHook<object>>();
            PreUnpackagingHooks = new List<IMessageHook<MessagePayload>>();
        }
    }
}