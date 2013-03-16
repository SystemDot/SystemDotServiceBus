using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport
{
    interface IMessageReceiver : IMessageProcessor<MessagePayload, MessagePayload>
    {

    }
}