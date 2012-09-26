using SystemDot.Messaging.Channels.Filtering;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public interface IPublisherChannelBuilder
    {
        void Build(PublisherChannelSchema schema);
    }
}