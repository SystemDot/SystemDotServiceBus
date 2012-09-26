using SystemDot.Messaging.Channels.Filtering;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IRequestSendChannelBuilder
    {
        void Build(RequestSendChannelSchema schema);
    }
}