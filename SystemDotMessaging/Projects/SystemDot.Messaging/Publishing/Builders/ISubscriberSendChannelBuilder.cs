using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing.Builders
{
    interface ISubscriberSendChannelBuilder
    {
        IMessageInputter<MessagePayload> BuildChannel(SubscriberSendChannelSchema schema);
    }
}