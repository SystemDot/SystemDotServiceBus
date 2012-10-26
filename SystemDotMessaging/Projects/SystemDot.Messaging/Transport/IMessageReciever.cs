using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Transport
{
    public interface IMessageReciever : IMessageProcessor<MessagePayload> 
    {
        void StartPolling(EndpointAddress address);
    }
}