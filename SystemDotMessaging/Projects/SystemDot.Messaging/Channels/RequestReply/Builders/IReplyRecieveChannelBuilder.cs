namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IReplyRecieveChannelBuilder
    {
        void Build(EndpointAddress senderAddress, params IMessageProcessor<object, object>[] hooks);
    }
}