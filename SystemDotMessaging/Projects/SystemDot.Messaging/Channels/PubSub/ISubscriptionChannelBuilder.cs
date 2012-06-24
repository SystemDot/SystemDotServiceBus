using SystemDot.Messaging.Channels.Messages;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.PubSub
{
    public interface ISubscriptionChannelBuilder 
    {
        IMessageInputter<MessagePayload> Build(SubscriptionSchema toSchema);
    }
}