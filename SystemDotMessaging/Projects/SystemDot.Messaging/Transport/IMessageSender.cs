using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Transport
{
    public interface IMessageSender : IMessageInputter<MessagePayload>
    {
    }
}