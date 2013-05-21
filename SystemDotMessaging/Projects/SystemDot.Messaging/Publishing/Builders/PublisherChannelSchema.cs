using System.Collections.Generic;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Publishing.Builders
{
    class PublisherChannelSchema : SendChannelSchema
    {
        public List<IMessageProcessor<object, object>> Hooks { get; set; }

        public IMessageFilterStrategy MessageFilterStrategy { get; set; }

        public PublisherChannelSchema()
        {
            Hooks = new List<IMessageProcessor<object, object>>();
        }
    }
}