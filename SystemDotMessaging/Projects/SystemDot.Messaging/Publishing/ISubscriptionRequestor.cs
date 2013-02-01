using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing
{
    public interface ISubscriptionRequestor : IMessageProcessor<MessagePayload>
    {
        void Start();
    }
}