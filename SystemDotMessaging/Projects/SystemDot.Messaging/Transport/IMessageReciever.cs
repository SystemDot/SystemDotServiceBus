using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Transport
{
    public interface IMessageReciever : IMessageProcessor<MessagePayload> 
    {
        void StartPolling(EndpointAddress address);
    }
}