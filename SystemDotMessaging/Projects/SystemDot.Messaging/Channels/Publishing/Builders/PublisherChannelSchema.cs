using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Filtering;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class PublisherChannelSchema : SendChannelSchema
    {
        public IMessageFilterStrategy MessageFilterStrategy { get; set; }
    }
}