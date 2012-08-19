using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing.Filtering;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IRequestSendChannelBuilder
    {
        void Build(IMessageFilterStrategy filteringStrategy, EndpointAddress fromAddress, EndpointAddress recieverAddress);
    }
}