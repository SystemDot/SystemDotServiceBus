using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    public interface ISubscriptionRequestor : IMessageProcessor<MessagePayload>
    {
        void Start();
    }
}