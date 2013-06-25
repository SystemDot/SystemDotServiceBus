using System.Collections.Generic;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing.Builders
{
    class PublisherChannelSchema : SendChannelSchema
    {
        public List<IMessageProcessor<object, object>> Hooks { get; set; }

        public List<IMessageProcessor<MessagePayload, MessagePayload>> PostPackagingHooks { get; set; }

        public IMessageFilterStrategy MessageFilterStrategy { get; set; }

        public PublisherChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
            PostPackagingHooks = new List<IMessageProcessor<MessagePayload, MessagePayload>>();
        }
    }
}