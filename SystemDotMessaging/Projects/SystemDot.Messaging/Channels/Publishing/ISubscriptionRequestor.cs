using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    public interface ISubscriptionRequestor : IMessageProcessor<MessagePayload>
    {
        void Start();
    }
}