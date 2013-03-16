using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport
{
    interface IMessageSender : IMessageInputter<MessagePayload>
    {
    }
}