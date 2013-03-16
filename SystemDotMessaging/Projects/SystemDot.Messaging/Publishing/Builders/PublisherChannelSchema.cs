using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Publishing.Builders
{
    class PublisherChannelSchema : SendChannelSchema
    {
        public IMessageFilterStrategy MessageFilterStrategy { get; set; }
    }
}