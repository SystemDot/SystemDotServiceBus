using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport
{
    public interface IMessageReceiver : IMessageProcessor<MessagePayload, MessagePayload>
    {

    }
}