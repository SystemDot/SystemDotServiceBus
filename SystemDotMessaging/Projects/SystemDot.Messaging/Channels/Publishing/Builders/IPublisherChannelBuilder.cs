using SystemDot.Messaging.Messages.Distribution;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public interface IPublisherChannelBuilder
    {
        IDistributor Build();
    }
}