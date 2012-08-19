using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing.Filtering;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public interface IPublisherChannelBuilder
    {
        void Build(EndpointAddress address, IMessageFilterStrategy messageFilterStrategy);
    }
}