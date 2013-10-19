using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing.Builders
{
    class PublisherChannelSchema : IDurableOptionSchema
    {
        public EndpointAddress FromAddress { get; set; }

        public bool IsDurable { get; set; }
    
        public List<IMessageHook<object>> Hooks { get; set; }

        public List<IMessageHook<MessagePayload>> PostPackagingHooks { get; set; }

        public IMessageFilterStrategy MessageFilterStrategy { get; set; }

        public PublisherChannelSchema()
        {
            Hooks = new List<IMessageHook<object>>();
            PostPackagingHooks = new List<IMessageHook<MessagePayload>>();
        }
    }
}