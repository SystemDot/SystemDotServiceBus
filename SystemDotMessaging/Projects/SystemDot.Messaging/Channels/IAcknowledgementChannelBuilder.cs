using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing.Caching;

namespace SystemDot.Messaging.Channels
{
    public interface IAcknowledgementChannelBuilder
    {
        void Build(IMessageCache cache, EndpointAddress address);
    }
}