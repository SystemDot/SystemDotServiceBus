using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing
{
    interface ISubscriptionRequestor : IMessageProcessor<MessagePayload>
    {
        void Start();
    }
}