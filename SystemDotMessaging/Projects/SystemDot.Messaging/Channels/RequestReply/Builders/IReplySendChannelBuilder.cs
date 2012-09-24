namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IReplySendChannelBuilder
    {
        void Build(EndpointAddress fromAddress);
    }
}