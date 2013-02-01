using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport
{
    public interface IMessageSender : IMessageInputter<MessagePayload>
    {
    }
}