using SystemDot.Messaging.Channels.Caching;

namespace SystemDot.Messaging.Channels.Acknowledgement.Builders
{
    public interface IAcknowledgementChannelBuilder
    {
        void Build(IMessageCache cache, EndpointAddress address);
    }
}