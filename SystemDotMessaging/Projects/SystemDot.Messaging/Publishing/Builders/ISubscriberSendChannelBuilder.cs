using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing.Builders
{

    public interface ISubscriberSendChannelBuilder
    {
        IMessageInputter<MessagePayload> BuildChannel(SubscriberSendChannelSchema schema);
    }
}