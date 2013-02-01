using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Publishing.Builders
{
    public class PublisherChannelSchema : SendChannelSchema
    {
        public IMessageFilterStrategy MessageFilterStrategy { get; set; }
    }
}