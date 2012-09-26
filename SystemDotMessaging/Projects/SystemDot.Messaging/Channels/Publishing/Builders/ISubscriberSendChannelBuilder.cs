using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public interface ISubscriberSendChannelBuilder
    {
        IMessageInputter<MessagePayload> BuildChannel(SubscriberSendChannelSchema subscriberSendChannelSchema);
    }
}